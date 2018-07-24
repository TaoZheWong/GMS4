using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.Data; 
using Newtonsoft.Json;

namespace GMSCore
{
    public class SAPOperation
    {
        /*
        string baseAddress = System.Configuration.ConfigurationManager.AppSettings["SAP_URI"];
        string Key = System.Configuration.ConfigurationManager.AppSettings["SAP_KEY"];
        string DB = System.Configuration.ConfigurationManager.AppSettings["SAP_DB"]; 
        */

        public string baseAddress  { get; set; }
        public string Key  { get; set; }
        public string DB  { get; set; }

        public SAPOperation(string baseAddress, string Key, string DB)
        {
            this.baseAddress = baseAddress;
            this.Key = Key;
            this.DB = DB;
        }


        #region CheckBeforeCallSAP
        public void CheckBeforeCallSAP(short coyID, string documentNo, string module, bool checkPosted)
        {
            //1. check if SAP is ON
            string SAP_ON = System.Configuration.ConfigurationManager.AppSettings["SAP_ON"];
            if (SAP_ON == "false")
                throw new Exception("SAP is offline at the moment. No posting can be done. Please contact your System Administrator.");
            if (!checkPosted)
                return;
            //2. check if this transaction has been Posted
            try
            {
                CheckPostedTransaction(coyID, documentNo, module);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region PostPO
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coyID"></param>Error Updating Purchase Order LMS Document Number MR1701550 - No matching records found (ODBC -2028)
        /// <param name="module"></param>
        /// <param name="so"></param>
        public void PostPO(short coyID, string module, SAPPurchaseOrder so, bool checkPosted)
        {
            using (var client = new WebClient())
            {
                string json = ""; 
                try
                {
                    CheckBeforeCallSAP(coyID, so.GMSDocumentNumber, module, checkPosted);
                    UpdatePostedBeforeTransaction(coyID, so.GMSDocumentNumber, module);
                    var SAPPurchaseOrder = new
                    {
                        SAPPurchaseOrder = new List<SAPPurchaseOrder> { so }
                    };

                    client.Encoding = Encoding.UTF8;
                    string method = "POST_SAP_AddUpdate_PurchaseOrder";
                    string uri = baseAddress + method + "?Key=" + Key + "&DB=" + DB;
                    client.UseDefaultCredentials = true;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    json = JsonConvert.SerializeObject(SAPPurchaseOrder);
                    string jsonResponse = client.UploadString(uri, json);
                    SAPResponseResult rr = JsonConvert.DeserializeObject<SAPResponseResult>(jsonResponse);

                    if (rr.RecordStatus == "true")
                        UpdatePostedTransaction(coyID, so.GMSDocumentNumber, module, Convert.ToInt32(rr.ErrorDescription));
                    else
                        throw new Exception(rr.ErrorDescription);
                }
                catch (WebException ex)
                {
                    UpdatePostedErrorTransaction(coyID, so.GMSDocumentNumber, module, false);
                    LogException(coyID,"PostPO", json, ex.Message);
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse wrsp = (HttpWebResponse)ex.Response;
                        var statusCode = (int)wrsp.StatusCode;
                        var msg = wrsp.StatusDescription;
                        throw new HttpException(statusCode, msg);
                    }
                    else
                    {
                        throw new HttpException(500, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    UpdatePostedErrorTransaction(coyID, so.GMSDocumentNumber, module, false);
                    LogException(coyID,"PostPO", json, ex.Message);
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion

        #region CancelPO
        public void CancelPO(short coyID, string module, SAPDocument doc, string cancelledReason)
        {
            using (var client = new WebClient())
            {
                string json = ""; 
                try
                {
                    CheckBeforeCallSAP(coyID, doc.LMSDocumentNumber, module, false);
                    var SAPPurchaseOrder = new
                    {
                        SAPPurchaseOrder = new List<SAPDocument> { doc }
                    };

                    client.Encoding = Encoding.UTF8;
                    string method = "POST_SAP_CancelDelete_PurchaseOrder";
                    string uri = baseAddress + method + "?Key=" + Key + "&DB=" + DB;
                    client.UseDefaultCredentials = true;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    json = JsonConvert.SerializeObject(SAPPurchaseOrder);
                    string jsonResponse = client.UploadString(uri, json);
                    SAPResponseResult rr = JsonConvert.DeserializeObject<SAPResponseResult>(jsonResponse);

                    if (rr.RecordStatus == "true")
                        CancelTransaction(coyID, doc.LMSDocumentNumber, module, cancelledReason, 0);
                    else
                        throw new Exception(rr.ErrorDescription);
                }
                catch (WebException ex)
                {
                    LogException(coyID, "CancelPO", json, ex.Message);
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse wrsp = (HttpWebResponse)ex.Response;
                        var statusCode = (int)wrsp.StatusCode;
                        var msg = wrsp.StatusDescription;
                        throw new HttpException(statusCode, msg);
                    }
                    else
                    {
                        throw new HttpException(500, ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    LogException(coyID,"CancelPO", json, ex.Message);
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion       

        #region LogException
        protected void LogException(short coyID, string method, string json, string exceptionMessage)
        {          
            new GMSGeneralDALC().InsertSAPLog(coyID, method, json, exceptionMessage);
        }
        #endregion

        public static void CheckPostedTransaction(short coyID, string documentNo, string module)
        {
            bool posted = false;
            posted = (new GMSGeneralDALC()).GetPostedTransaction(coyID, documentNo, module);     
            if (posted)
                throw new Exception("The transaction has been posted.");
        }

        public static void UpdatePostedBeforeTransaction(short coyID, string documentNo, string module)
        {
            new GMSGeneralDALC().UpdatePostedBeforeTransaction(coyID, documentNo, module);
        }

        public static void UpdatePostedTransaction(short coyID, string documentNo, string module, int SAPNo)
        {
            new GMSGeneralDALC().UpdatePostedTransaction(coyID, documentNo, module, SAPNo);
        }

        public static void UpdatePostedErrorTransaction(short coyID, string documentNo, string module, bool isGLInfo)
        {
            new GMSGeneralDALC().UpdatePostedErrorTransaction(coyID, documentNo, module, isGLInfo);
        }

        public static void CancelTransaction(short coyID, string documentNo, string module, string cancelledReason, int SAPNo)
        {
            new GMSGeneralDALC().CancelTransaction(coyID, documentNo, module, cancelledReason, SAPNo);
        }

        #region GetProductAvailableBatch
        public DataSet GetProductAvailableBatch(short coyID, string prodCode, string warehouse)
        {
            using (var client = new WebClient())
            {
                string uri = "";
                try
                {
                    client.Encoding = Encoding.UTF8;
                    string method = "GET_SAP_Item_Available_Batch";
                    uri = baseAddress + method + "?Key=" + Key + "&DB=" + DB + "&ItemCode=" + prodCode + "&Warehouse=" + warehouse;
                    client.UseDefaultCredentials = true;
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    string jsonResponse = client.DownloadString(uri);
                    DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(jsonResponse);
                    DataTable dataTable = dataSet.Tables["SAPItem_Available_Batch"];
                    if (dataTable.Rows.Count > 0)
                    {
                        dataTable.Columns["ItemCode"].ColumnName = "ProdCode";
                        dataTable.Columns["BatchNumber"].ColumnName = "BatchNo";
                        dataTable.Columns["Quantity"].ColumnName = "Qty";
                        dataTable.Columns["ManufacturingDate"].ColumnName = "ManufacturingDate";
                        dataTable.Columns["ExpiryDate"].ColumnName = "ExpirationDate";
                        dataTable.Columns["AdmissionDate"].ColumnName = "AdmissionDate";
                    }
                    return dataSet;
                }
                catch (WebException ex)
                {                   
                    LogException(coyID, "GetProductAvailableBatch", uri, ex.Message);
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse wrsp = (HttpWebResponse)ex.Response;
                        var statusCode = (int)wrsp.StatusCode;
                        var msg = wrsp.StatusDescription;
                        throw new HttpException(statusCode, msg);
                    }
                    else
                    {
                        throw new HttpException(500, ex.Message);
                    }
                }
            }
        }
        #endregion

        #region GetProductAvailableSerial
        public DataSet GetProductAvailableSerial(short coyID, string prodCode, string warehouse)
        {
            using (var client = new WebClient())
            {
                string uri = "";
                try
                {
                    client.Encoding = Encoding.UTF8;
                    string method = "GET_SAP_Item_Available_Serial";
                    uri = baseAddress + method + "?Key=" + Key + "&DB=" + DB + "&ItemCode=" + prodCode + "&Warehouse=" + warehouse;
                    client.UseDefaultCredentials = true;
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    string jsonResponse = client.DownloadString(uri);
                    DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(jsonResponse);
                    DataTable dataTable = dataSet.Tables["SAPItem_Available_Serial"];
                    if (dataTable.Rows.Count > 0)
                    {
                        dataTable.Columns["ItemCode"].ColumnName = "ProdCode";
                        dataTable.Columns["Serial"].ColumnName = "SerialNo";
                        dataTable.Columns["ManufacturingDate"].ColumnName = "ManufacturingDate";
                        dataTable.Columns["ExpiryDate"].ColumnName = "ExpirationDate";
                        dataTable.Columns["AdmissionDate"].ColumnName = "AdmissionDate";
                    }
                    return dataSet;
                }
                catch (WebException ex)
                {                  
                    LogException(coyID, "GetProductAvailableSerial", uri, ex.Message);
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse wrsp = (HttpWebResponse)ex.Response;
                        var statusCode = (int)wrsp.StatusCode;
                        var msg = wrsp.StatusDescription;
                        throw new HttpException(statusCode, msg);
                    }
                    else
                    {
                        throw new HttpException(500, ex.Message);
                    }
                }
            }
        }
        #endregion

        #region GetProductCost
        public DataSet GetProductCost(short coyID, string prodCode)
        {
            using (var client = new WebClient())
            {
                string uri = "";
                try
                {
                    client.Encoding = Encoding.UTF8;
                    string method = "GET_SAP_Item_Cost";
                    uri = baseAddress + method + "?Key=" + Key + "&DB=" + DB + "&ItemCode=" + prodCode;
                    client.UseDefaultCredentials = true;
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    string jsonResponse = client.DownloadString(uri);
                    DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(jsonResponse);
                    DataTable dataTable = dataSet.Tables["SAPItem_Cost"];
                    if (dataTable.Rows.Count > 0)
                    {
                        dataTable.Columns["ItemCode"].ColumnName = "ProdCode";
                        dataTable.Columns["Cost"].ColumnName = "WeightedCost";
                    }
                    return dataSet;
                }
                catch (WebException ex)
                {                   
                    LogException(coyID, "GetProductCost", uri, ex.Message);
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse wrsp = (HttpWebResponse)ex.Response;
                        var statusCode = (int)wrsp.StatusCode;
                        var msg = wrsp.StatusDescription;
                        throw new HttpException(statusCode, msg);
                    }
                    else
                    {
                        throw new HttpException(500, ex.Message);
                    }
                }
            }
        }
        #endregion

        public DataSet GET_SAP_QueryData(short coyID, string query, 
            string field1, string field2, string field3, string field4, string field5, string field6, string field7, string field8, string field9, string field10,
            string field11, string field12, string field13, string field14, string field15, string field16, string field17, string field18, string field19, string field20,
            string field21, string field22, string field23, string field24, string field25, string field26, string field27, string field28, string field29, string field30)
        {
            using (var client = new WebClient())
            {
                string uri = "";
                try
                {
                    client.Encoding = Encoding.UTF8;
                    string method = "GET_SAP_QueryData";
                    uri = baseAddress + method + "?Key=" + Key + "&DB=" + DB + "&Query=" + query;
                    client.UseDefaultCredentials = true;
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    string jsonResponse = client.DownloadString(uri);
                    DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(jsonResponse);
                    DataTable dataTable = dataSet.Tables["SAPQuery_Data"];
                    if (dataTable.Rows.Count > 0)
                    {
                        dataTable.Columns["Field1"].ColumnName = field1;
                        dataTable.Columns["Field2"].ColumnName = field2;
                        dataTable.Columns["Field3"].ColumnName = field3;
                        dataTable.Columns["Field4"].ColumnName = field4;
                        dataTable.Columns["Field5"].ColumnName = field5;
                        dataTable.Columns["Field6"].ColumnName = field6;
                        dataTable.Columns["Field7"].ColumnName = field7;
                        dataTable.Columns["Field8"].ColumnName = field8;
                        dataTable.Columns["Field9"].ColumnName = field9;
                        dataTable.Columns["Field10"].ColumnName = field10;
                        dataTable.Columns["Field11"].ColumnName = field11;
                        dataTable.Columns["Field12"].ColumnName = field12;
                        dataTable.Columns["Field13"].ColumnName = field13;
                        dataTable.Columns["Field14"].ColumnName = field14;
                        dataTable.Columns["Field15"].ColumnName = field15;
                        dataTable.Columns["Field16"].ColumnName = field16;
                        dataTable.Columns["Field17"].ColumnName = field17;
                        dataTable.Columns["Field18"].ColumnName = field18;
                        dataTable.Columns["Field19"].ColumnName = field19;
                        dataTable.Columns["Field20"].ColumnName = field20;
                        dataTable.Columns["Field21"].ColumnName = field21;
                        dataTable.Columns["Field22"].ColumnName = field22;
                        dataTable.Columns["Field23"].ColumnName = field23;
                        dataTable.Columns["Field24"].ColumnName = field24;
                        dataTable.Columns["Field25"].ColumnName = field25;
                        dataTable.Columns["Field26"].ColumnName = field26;
                        dataTable.Columns["Field27"].ColumnName = field27;
                        dataTable.Columns["Field28"].ColumnName = field28;
                        dataTable.Columns["Field29"].ColumnName = field29;
                        dataTable.Columns["Field30"].ColumnName = field30;
                    }
                    return dataSet;
                }
                catch (WebException ex)
                {
                    LogException(coyID, "GET_SAP_QueryData", uri, ex.Message);
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse wrsp = (HttpWebResponse)ex.Response;
                        var statusCode = (int)wrsp.StatusCode;
                        var msg = wrsp.StatusDescription;
                        throw new HttpException(statusCode, msg);
                    }
                    else
                    {
                        throw new HttpException(500, ex.Message);
                    }
                }
            }
        }
    }
}