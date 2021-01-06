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

namespace GMSWeb.SysFinance.BankFacilities
{
    public partial class ReportViewer : GMSBasePage
    {
        protected ReportDocument crReportDocument;
        private short reportId = 0;
        private string trnNo = "";
        string fileName = "";

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

            this.reportId = GMSUtil.ToShort(Request.QueryString["REPORTID"]);
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
                cyReportViewer.ReportSource = crReportDocument;
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
