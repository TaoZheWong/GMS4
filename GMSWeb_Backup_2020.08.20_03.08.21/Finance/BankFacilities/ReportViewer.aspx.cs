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
    public partial class ReportViewer : GMSBasePage
    {
        protected ReportDocument crReportDocument;
        private short reportId = 0;
        private string trnNo = "";
        private string trnType = "";
        private int bankCOAID = 0;
        string fileName = "";
        protected short loginUserOrAlternateParty = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            //UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
            //                                                                49);
            //if (uAccess == null)
            //    Response.Redirect("../../Unauthorized.htm");

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Finance Report", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {

                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;
            
            this.reportId = GMSUtil.ToShort(Request.QueryString["REPORTID"]);
            this.trnNo = Request.QueryString["TRNNO"].Trim();


            if (reportId != -1 && reportId != -2 && reportId != -3 && reportId != -4)
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
                fileName = Request.QueryString["REPORT"].Trim()+".rpt";
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
                        crReportDocument.SetParameterValue("@BUIDList", trnNo);

                    }

                    if (reportId == -4 && crReportDocument.ParameterFields["MRNo"] != null)
                        crReportDocument.SetParameterValue("MRNo", trnNo);
                  

                    crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
                    crReportDocument.SetParameterValue("CoyID", session.CompanyId);

                    if (crReportDocument.ParameterFields["UserNumID"] != null)
                        crReportDocument.SetParameterValue("UserNumID", loginUserOrAlternateParty);


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
                    if (reportId == -1 && crReportDocument.ParameterFields["@BUIDList"] != null)
                        crReportDocument.SetParameterValue("@BUIDList", trnNo);
                    if (reportId == -3 && crReportDocument.ParameterFields["Quotation No"] != null)
                        crReportDocument.SetParameterValue("Quotation No", trnNo);
                    if (reportId == -4 && crReportDocument.ParameterFields["MRNo"] != null)
                        crReportDocument.SetParameterValue("MRNo", trnNo);

                   

                    if (crReportDocument.ParameterFields["@CoyID"] != null)
                        crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
                    if (crReportDocument.ParameterFields["CoyID"] != null)
                        crReportDocument.SetParameterValue("CoyID", session.CompanyId);
                    if (crReportDocument.ParameterFields["@UserNumID"] != null)
                        crReportDocument.SetParameterValue("@UserNumID", loginUserOrAlternateParty);
                    if (crReportDocument.ParameterFields["UserNumID"] != null)
                        crReportDocument.SetParameterValue("UserNumID", loginUserOrAlternateParty);
                    cyReportViewer.ReportSource = crReportDocument;
                }
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
            crReportDocument.Close();
            crReportDocument.Dispose();
            GC.Collect();
        }
    }
}
