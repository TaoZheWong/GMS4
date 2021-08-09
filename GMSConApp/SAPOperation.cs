using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.Data;
using Newtonsoft.Json;

namespace GMSConApp
{
    public class SAPOperation
    {
        private string baseAddress = "http://52.163.250.190/AF_LEEDENNOX/api/AFON/";
        private string Key = "DR6w2@888LnePo*!";
        private string DB = "SBO_LNOX_LIVE";
        static DAL oDAL = null;
        
        public string BaseAddress
        {
            get { return this.baseAddress; }
            set { this.baseAddress = value; }
        }

        public string SAPKey
        {
            get { return this.Key; }
            set { this.Key = value; }
        }

        public string SAPDB
        {
            get { return this.DB; }
            set { this.DB = value; }
        }

        public DataSet GET_SAP_QueryData(short coyID, string query, 
            string field1, string field2, string field3, string field4, string field5, string field6, string field7, string field8, string field9, string field10,
            string field11, string field12, string field13, string field14, string field15, string field16, string field17, string field18, string field19, string field20,
            string field21, string field22, string field23, string field24, string field25, string field26, string field27, string field28, string field29, string field30)
        {
            DataSet dataSet = null;
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
                    dataSet = JsonConvert.DeserializeObject<DataSet>(jsonResponse);                                       
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
                    
                }
                catch (WebException ex)
                {
                    oDAL.LogException(coyID, "GET_SAP_QueryData", uri, ex.Message);                    
                }
            }
            return dataSet;
        }

        public DataSet LMS_GET_SAP_QueryData(string code, string query,
           string field1, string field2, string field3, string field4, string field5, string field6, string field7, string field8, string field9, string field10,
           string field11, string field12, string field13, string field14, string field15, string field16, string field17, string field18, string field19, string field20,
           string field21, string field22, string field23, string field24, string field25, string field26, string field27, string field28, string field29, string field30)
        {
            DataSet dataSet = null;
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
                    dataSet = JsonConvert.DeserializeObject<DataSet>(jsonResponse);
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

                }
                catch (WebException ex)
                {
                    //oDAL.LogException(coyID, "GET_SAP_QueryData", uri, ex.Message);
                }
            }
            return dataSet;
        }
    }
}
