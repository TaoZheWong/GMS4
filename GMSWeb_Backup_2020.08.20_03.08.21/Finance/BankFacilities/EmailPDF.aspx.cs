using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.XPath;

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;



namespace GMSWeb.Finance.BankFacilities
{
    public partial class EmailPDF : GMSBasePage
    {
        protected ReportDocument crReportDocument;
        private short reportId = 0;
        private string trnNo = "";        
        string filePath = "";
        string fileName = "";
        string fileURLPath = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }

            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"">
                    

                    var nameSpace = null; 
                    var mailFolder = null; 
                    var mailItem = null; 
                    var tempDoc = null; 
                    var outlookApp = null;
                    function OpenOutlookDoc(whatform,file) 
                    { 
                          try 
                          { 
                          outlookApp = new ActiveXObject('Outlook.Application'); 
                          nameSpace = outlookApp.getNameSpace('MAPI'); 
                          mailFolder = nameSpace.getDefaultFolder(6); 
                          mailItem = mailFolder.Items.add(whatform); 
                          mailItem.Attachments.Add(file);  
                          mailItem.Display(true) 
                          } 
                          catch(e) 
                          {                           
                          } 
                   } 
      
            </script>
                ";            
             System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onload", javaScript, false);


            

            //UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
            //                                                                49);
            //if (uAccess == null)
            //    Response.Redirect("../../Unauthorized.htm");

            this.reportId = GMSUtil.ToShort(Request.QueryString["REPORTID"]);
            this.trnNo = Request.QueryString["TRNNO"].Trim();

            //Session["Rpt_Name"] = "Quotation_" + this.trnNo;
            this.filePath = "D://GMS/GMSWeb/PDF/" + "Quotation_" + this.trnNo.ToString() + ".pdf";

            if (reportId != -1 && reportId != -2 && reportId != -3)
            {
                fileName = new ReportsActivity().RetrieveReportById(this.reportId).FileName;
                GMSCore.Entity.AuditForReportAccess audit = AuditForReportAccess.RetrieveByKey(session.CompanyId, session.UserId, reportId, GMSUtil.ToDate(session.LastLoginDate));
                if (audit == null)
                {
                    audit = new AuditForReportAccess();
                    audit.CoyID = session.CompanyId;
                    audit.UserID = session.UserId;
                    audit.ReportID = reportId;
                    audit.AccessDate = GMSUtil.ToDate(session.LastLoginDate);
                    audit.Save();
                }
            }
            else
            {
                fileName = Request.QueryString["REPORT"].Trim() + ".rpt";
                if (fileName == "Pricelist.rpt")
                    cyReportViewer.HasToggleGroupTreeButton = true;
            }

            try
            {
                crReportDocument = new ReportDocument();
                crReportDocument.Load(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.DOC_PATH + "\\" + fileName);

            }
            catch (Exception ex)
            {
                this.lblFeedback.Text = ex.Message;
            }

            try
            {
                if (reportId >= 180 && reportId <= 187)
                {
                    ConnectionInfo connection = new ConnectionInfo();
                    connection.DatabaseName = DBManager.GetInstance().ArchiveDatabaseName;
                    connection.ServerName = DBManager.GetInstance().ArchiveServerName;
                    connection.UserID = DBManager.GetInstance().ArchiveUserLoginName;
                    connection.Password = DBManager.GetInstance().ArchiveUserLoginPwd;

                    foreach (CrystalDecisions.CrystalReports.Engine.Table table in crReportDocument.Database.Tables)
                    {
                        // Cache the logon info block
                        TableLogOnInfo logOnInfo = table.LogOnInfo;

                        // Set the connection
                        logOnInfo.ConnectionInfo = connection;

                        // Apply the connection to the table!
                        table.ApplyLogOnInfo(logOnInfo);
                    }

                    foreach (CrystalDecisions.CrystalReports.Engine.Section section in crReportDocument.ReportDefinition.Sections)
                    {
                        // In each section we need to loop through all the reporting objects
                        foreach (CrystalDecisions.CrystalReports.Engine.ReportObject reportObject in section.ReportObjects)
                        {
                            if (reportObject.Kind == ReportObjectKind.SubreportObject)
                            {
                                SubreportObject subReport = (SubreportObject)reportObject;
                                ReportDocument subDocument = subReport.OpenSubreport(subReport.SubreportName);

                                foreach (CrystalDecisions.CrystalReports.Engine.Table table in subDocument.Database.Tables)
                                {
                                    // Cache the logon info block
                                    TableLogOnInfo logOnInfo = table.LogOnInfo;

                                    // Set the connection
                                    logOnInfo.ConnectionInfo = connection;

                                    // Apply the connection to the table!
                                    table.ApplyLogOnInfo(logOnInfo);
                                }
                            }
                        }
                    }

                    //crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
                    //crReportDocument.SetParameterValue("CoyID", session.CompanyId);
                    if (reportId == -1)
                    {
                        crReportDocument.SetParameterValue("TransactionNo", trnNo);


                    }



                    crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
                    crReportDocument.SetParameterValue("CoyID", session.CompanyId);


                    cyReportViewer.ReportSource = crReportDocument;
                }
                else
                {
                    ConnectionInfo connection = new ConnectionInfo();
                    connection.DatabaseName = DBManager.GetInstance().DatabaseName;
                    connection.ServerName = DBManager.GetInstance().ServerName;
                    connection.UserID = DBManager.GetInstance().UserLoginName;
                    connection.Password = DBManager.GetInstance().UserLoginPwd;

                    foreach (CrystalDecisions.CrystalReports.Engine.Table table in crReportDocument.Database.Tables)
                    {
                        // Cache the logon info block
                        TableLogOnInfo logOnInfo = table.LogOnInfo;

                        // Set the connection
                        logOnInfo.ConnectionInfo = connection;

                        // Apply the connection to the table!
                        table.ApplyLogOnInfo(logOnInfo);
                    }

                    foreach (CrystalDecisions.CrystalReports.Engine.Section section in crReportDocument.ReportDefinition.Sections)
                    {
                        // In each section we need to loop through all the reporting objects
                        foreach (CrystalDecisions.CrystalReports.Engine.ReportObject reportObject in section.ReportObjects)
                        {
                            if (reportObject.Kind == ReportObjectKind.SubreportObject)
                            {
                                SubreportObject subReport = (SubreportObject)reportObject;
                                ReportDocument subDocument = subReport.OpenSubreport(subReport.SubreportName);

                                foreach (CrystalDecisions.CrystalReports.Engine.Table table in subDocument.Database.Tables)
                                {
                                    // Cache the logon info block
                                    TableLogOnInfo logOnInfo = table.LogOnInfo;

                                    // Set the connection
                                    logOnInfo.ConnectionInfo = connection;

                                    // Apply the connection to the table!
                                    table.ApplyLogOnInfo(logOnInfo);
                                }
                            }
                        }
                    }

                    //crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
                    //crReportDocument.SetParameterValue("CoyID", session.CompanyId);
                    if (reportId == -1 && crReportDocument.ParameterFields["TransactionNo"] != null)
                        crReportDocument.SetParameterValue("TransactionNo", trnNo);
                    if (reportId == -3 && crReportDocument.ParameterFields["Quotation No"] != null)
                        crReportDocument.SetParameterValue("Quotation No", trnNo);




                    if (crReportDocument.ParameterFields["@CoyID"] != null)
                        crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
                    if (crReportDocument.ParameterFields["CoyID"] != null)
                        crReportDocument.SetParameterValue("CoyID", session.CompanyId);
                    if (crReportDocument.ParameterFields["@UserNumID"] != null)
                        crReportDocument.SetParameterValue("@UserNumID", session.UserId);
                    //cyReportViewer.ReportSource = crReportDocument;
                }
                
                crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);               
                string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                this.fileURLPath = strUrl + "GMS/PDF/" + "Quotation_" + this.trnNo.ToString() + ".pdf";
                ClientScript.RegisterStartupScript(GetType(), "Email", "OpenOutlookDoc('IPM.Note.FormA','" + fileURLPath + "');", true); 
            }
            catch (Exception ex)
            {
                this.lblFeedback.Text = ex.Message;
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            cyReportViewer.Dispose();
            cyReportViewer = null;
            //crReportDocument.Close();
            //crReportDocument.Dispose();
            GC.Collect();
        }

        public void Test()
        {
        }
    }
}
