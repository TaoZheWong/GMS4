using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO; 

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Finance.Upload
{
    public partial class ProcessTrialBalance : GMSBasePage
    {
        private short coyId;
        private short year;
        private short month;
        LogSession session;

        protected void Page_Load(object sender, EventArgs e)
        {
            coyId = GMSUtil.ToShort(Request.Params["CoyID"]);
            year = GMSUtil.ToShort(Request.Params["Year"]);
            month = GMSUtil.ToShort(Request.Params["Month"]);
            session = base.GetSessionInfo();

            #region Step 1: Check if other user is processing the trial balance.
            SystemParameterActivity spa = new SystemParameterActivity();
            SystemParameter sp = spa.RetrieveProcessingTrialBalance();

            if (sp.ParameterValue == "1")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "", "alert('Trial Balance data is being processed by another user. Please wait for a while and try again later.')", true);
                return;
            }
            #endregion

            sp.ParameterValue = "1";
            spa.UpdateProcessingTrialBalance(ref sp, session); 

            Response.ContentType = "text/html";

            GMSGeneralDALC DALC = new GMSGeneralDALC();
            DateTime coadate = DALC.GetNewCOADate(coyId);
            bool is2016coa = false;
            if (((coadate.Year * 100) + coadate.Month) <= ((year * 100) + month))
            {
                is2016coa = true;
            }
            else
            {
                is2016coa = false;
            }
            processTrialBalance(is2016coa);

            sp.ParameterValue = "0";
            spa.UpdateProcessingTrialBalance(ref sp, session);

            Response.End();
        }

        private void processTrialBalance(bool is2016coa)
        {
            try
            {
                try
                {
                    LogSession session = base.GetSessionInfo();
                    DataSet ds = new DataSet();
                    DataSet ds4901 = new DataSet();
                    string accountingSystem = (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S") ? "SAP" : "A21"; 
                    //Use By Excel to cater multiple upload - Adam -13/11/2015
                    DataTable dt = new DataTable();
                    dt.Columns.Add("DBName");
                    dt.Columns.Add("Year");
                    dt.Columns.Add("Month");
                    dt.Columns.Add("AccountCode");
                    dt.Columns.Add("PrevBalance");
                    dt.Columns.Add("Debit");
                    dt.Columns.Add("Credit");


                    if (session.TBType == "N" || session.TBType == "P") // read from local A21 or remote A21
                    { 
                        Response.Output.Write("Connecting to " + accountingSystem + " ...<br>");
                        Response.Output.Write(new string(' ', 5000));
                        Response.Flush();
                        
                        GMSWebService.GMSWebService ws = new GMSWebService.GMSWebService();
                        if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                            ws.Url = session.WebServiceAddress.Trim();
                        else
                            ws.Url = "http://localhost/GMSWebService/GMSWebService.asmx";

                        Response.Output.Write("Retrieving Trial Balance data from " + accountingSystem + " ...<br>");
                        Response.Output.Write(new string(' ', 5000));
                        Response.Flush();

                        short financialYear = year;
                        short financialMonth = month;

                        if (session.FYE == 12)
                        {
                            financialYear = year;
                            financialMonth = month;
                        }
                        else if (session.FYE == 3)
                        {
                            if (is2016coa) //New COA
                            {
                                if (month >= 4 && month <= 12)
                                {
                                    financialMonth = Convert.ToInt16(month - 3);
                                }
                                else
                                {
                                    financialYear = Convert.ToInt16(year - 1);
                                    financialMonth = Convert.ToInt16(month + 9);
                                }
                            }
                            else  //old COA use back FTE=12
                            {
                                session.FYE = 12;
                                financialYear = year;
                                financialMonth = month;
                            }
                        }
                        else if (session.FYE == 6)
                        {
                            if (month >= 7 && month <= 12)
                            {
                                financialMonth = Convert.ToInt16(month - 6);
                            }
                            else
                            {
                                financialYear = Convert.ToInt16(year - 1);
                                financialMonth = Convert.ToInt16(month + 6);
                            }
                        }
                        else
                        {
                            Response.Output.Write("<SPAN STYLE='color: red'><B>Financial Year End is not set or not supported.</SPAN><br>");
                            Response.Output.Write(new string(' ', 5000));
                            Response.Flush();
                            return;
                        }

                        if (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S")
                        { 
                            string bcType = "Y";
                            if (session.FYE.ToString() == month.ToString())                                                       
                                bcType = "N";  
                                    
                            string query = "CALL \"AF_API_GET_SAP_TRIALBALANCE_DIMENSION\" (" + year + "," + month + ", '" + bcType + "')";
                            SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                            
                            ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                                "Year", "Month", "Project", "Department", "Section", "Unit", "AccountCode", "PrevBalance", "Debit", "Credit", 
                                "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                                "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                            DataView dvOri = new DataView(ds.Tables[0]);
                            dvOri.RowFilter = "AccountCode <> '4901'";
                            DataTable dtOri = dvOri.ToTable();
                            ds.Reset();
                            ds.Tables.Add(dtOri);
                            
                            query = "CALL \"AF_API_GET_SAP_TRIALBALANCE_DIMENSION\" (" + year + "," + month + ", 'Y')";
                            ds4901 = sop.GET_SAP_QueryData(session.CompanyId, query,
                            "Year", "Month", "Project", "Department", "Section", "Unit", "AccountCode", "PrevBalance", "Debit", "Credit",
                            "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                            "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                            if (ds4901 != null && ds4901.Tables.Count > 0 && ds4901.Tables[0].Rows.Count > 0) {
                                DataView dv = new DataView(ds4901.Tables[0]);
                                dv.RowFilter = "AccountCode = '4901'";
                                DataTable dt4901 = dv.ToTable();
                                foreach (DataRow row in dt4901.Rows) {
                                    ds.Tables[0].ImportRow(row);
                                }
                            }
                        }
                        else
                        { 
                            if (session.TBType == "N")
                                ds = ws.GetTrialBalance(coyId, financialYear, financialMonth, session.FYE, is2016coa);
                            else if (session.TBType == "P")
                                ds = ws.GetTrialBalanceForPDS(coyId, financialYear, financialMonth, session.FYE, is2016coa);
                            else
                            {
                                Response.Output.Write("<SPAN STYLE='color: red'><B>Trial Balance Type is not set or not supported.</SPAN><br>");
                                Response.Output.Write(new string(' ', 5000));
                                Response.Flush();
                                return;
                            }
                        }

                        Response.Output.Write("Inserting Trial Balance data into GMS ...<br>");
                        Response.Output.Write(new string(' ', 5000));
                        Response.Flush();

                        new FinanceDALC().InsertTempTransfer(coyId, year, month, ds.Tables[0].Rows, session.TBType, false, session.StatusType.ToString());
                        
                        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            Response.Output.Write("<SPAN STYLE='color: red'><B>No Trial Balance data.</SPAN><br>");
                            Response.Output.Write(new string(' ', 5000));
                            Response.Flush();
                            return;
                        }

                        Response.Output.Write("Processing Trial Balance data in GMS ...<br>");
                        Response.Output.Write(new string(' ', 5000));
                        Response.Flush();

                        new FinanceDALC().ProcessTrialBalance();
                    }
                    else if (session.TBType == "M")
                    {
                        Response.Output.Write("Retrieving excel file that has been ftp-ed ...<br>");
                        Response.Output.Write(new string(' ', 5000));
                        Response.Flush();

                        bool hasFile = false;
                        string excelDirectory = "C:\\Inetpub\\ftproot\\GMSOtherSystemMapping\\" + session.DBName + "\\IncomingMap\\";
                        string archiveDirectory = "C:\\Inetpub\\ftproot\\GMSOtherSystemMapping\\" + session.DBName + "\\IncomingMap\\Archive\\";
                        string[] TBfiles = Directory.GetFiles(excelDirectory);
                        foreach (string filepath in TBfiles)
                        {

                            Response.Output.Write("Reading excel data [" + Path.GetFileName(filepath) + "]...<br>");
                            Response.Output.Write(new string(' ', 5000));
                            Response.Flush();

                            hasFile = true;

                            //copy files to Archive folder 
                            File.Copy(filepath, archiveDirectory + Path.GetFileName(filepath), true);
                            Asiasoft.MSExcelFileReader.SheetDataLoader sheetDataLoader_TB = new Asiasoft.MSExcelFileReader.SheetDataLoader();
                            sheetDataLoader_TB.ExcelFilePath = filepath;
                            sheetDataLoader_TB.IsHeaderIncludedInExcelFile = true;
                            sheetDataLoader_TB.SheetName = "Sheet1";
                            sheetDataLoader_TB.LoadExcelData();
                            
                            //Data Cleaning to avoid Empty Columns
                            foreach (DataRow dr in sheetDataLoader_TB.ExcelData.Tables[0].Rows)
                            {
                                if (!string.IsNullOrEmpty(dr[0].ToString()))
                                {
                                    dt.ImportRow(dr);
                                }
                            }
                        }

                        if (!hasFile)
                        {
                            Response.Output.Write("<SPAN STYLE='color: red'><B>No excel file is found in the directory ...</SPAN><br>");
                            Response.Output.Write(new string(' ', 5000));
                            Response.Flush();
                            return;
                        }

                        try
                        {
                            Response.Output.Write("Inserting Trial Balance data into GMS ...<br>");
                            Response.Output.Write(new string(' ', 5000));
                            Response.Flush();

                            //Insert into Temptransfer
                            new FinanceDALC().InsertTempTransfer(coyId, year, month, dt.Rows, "N", true, session.StatusType.ToString());

                            if (dt == null || dt.Rows.Count == 0)
                            {
                                Response.Output.Write("<SPAN STYLE='color: red'><B>No Trial Balance data.</SPAN><br>");
                                Response.Output.Write(new string(' ', 5000));
                                Response.Flush();
                                return;
                            }

                            Response.Output.Write("Processing Trial Balance data in GMS ...<br>");
                            Response.Output.Write(new string(' ', 5000));
                            Response.Flush();

                            new FinanceDALC().ProcessTrialBalance();

                        }
                        catch (Exception ex)
                        {
                            Response.Output.Write("<SPAN STYLE='color: red'><B>Error:" + ex.Message.ToString() + ".</B></SPAN><br>");
                            Response.Output.Write(new string(' ', 5000));
                            Response.Flush();
                            return;
                        }
                        finally
                        {
                            foreach (string filepath in TBfiles)
                            {
                                File.Delete(filepath);
                            }
                        }
                    }
                    else
                    {
                        Response.Output.Write("<SPAN STYLE='color: red'><B>Trial Balance Type is not set or not supported.</SPAN><br>");
                        Response.Output.Write(new string(' ', 5000));
                        Response.Flush();
                        return;
                    }                                
                                 
                }
                catch (Exception ex)
                {
                    Response.Output.Write("<SPAN STYLE='color: red'><B>Error:" + ex.Message.ToString() + ".</B></SPAN><br>");
                    Response.Output.Write(new string(' ', 5000));
                    Response.Flush();
                    return; 
                }
                
                Response.Output.Write("<SPAN STYLE='color: green'>Trial Balance data has been imported! Please check the Finance Reports.<br>");
                Response.Output.Write(new string(' ', 5000));
                Response.Flush();                
            }
            catch (Exception ex)
            {
                Response.Output.Write(("<SPAN STYLE='color: red'><B>Error:" + ex.Message.ToString() + ".</B></SPAN><br>"));
                Response.Output.Write(new string(' ', 5000));
                Response.Flush();
            }
        }
    }
}
