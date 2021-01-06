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

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace GMSWeb.HR.Reports
{
    public partial class ReportViewer : GMSBasePage
    {
        protected ReportDocument crReportDocument;
        private short reportId = 0;
        string fileName = "";
        private string formID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //IsCallService();
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            46);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            this.reportId = GMSUtil.ToShort(Request.QueryString["REPORTID"]);
            this.formID = Request.Params["FORMID"];

            if (reportId != -1)
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
                if (!string.IsNullOrEmpty(formID))
                {
                    crReportDocument.SetParameterValue("FORMID", int.Parse(formID));
                }
               //crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, "C:\test.pdf");
                //cyReportViewer.HasExportButton = false;
                //cyReportViewer.HasPrintButton = false;
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

        private bool IsCallService()
        {
            Response.CacheControl = "no-cache";
            if (this.Request.Params["CallService"] != null)
            {
                this.reportId = 41;
                //this.formID = Request.Params["FORMID"];

                if (reportId != -1)
                {
                    fileName = new ReportsActivity().RetrieveReportById(this.reportId).FileName;
                }
                else
                {
                    fileName = Request.QueryString["REPORT"].Trim() + ".rpt";
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

                    crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + Path.DirectorySeparatorChar + "List of Training Courses.pdf");
                    crReportDocument.Clone();
                    crReportDocument.Dispose();
                    //cyReportViewer.HasExportButton = false;
                    //cyReportViewer.HasPrintButton = false;
                    //cyReportViewer.ReportSource = crReportDocument;
                }
                catch (Exception ex)
                {
                    this.lblFeedback.Text = ex.Message;
                }


                //LogSession session = base.GetSessionInfo();
                System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
                System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress("ray.tong@leedenlimited.com", "Tong Rui");
                System.Net.Mail.MailAddress user2EmailAddress = new System.Net.Mail.MailAddress("siewsiew.ong@leedenlimited.com", "Ong Siew Siew");
                string smtpServer = "smtp.leedenlimited.com";

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.From = adminEmailAddress;
                mail.To.Add(userEmailAddress);
                mail.To.Add(user2EmailAddress);

                mail.ReplyTo = new System.Net.Mail.MailAddress("ray.tong@leedenlimited.com", "Tong Rui, Ray");
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.Subject = "[GMS - Test Schedule Job]";
                mail.IsBodyHtml = true;
                mail.Body = "This test email is sent by schedule job at " + DateTime.Now.ToString();

                string filePath = AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + Path.DirectorySeparatorChar + "List of Training Courses.pdf";
                if (File.Exists(filePath))
                    mail.Attachments.Add(new System.Net.Mail.Attachment(filePath));

                try
                {
                    System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                    mailClient.Host = smtpServer;
                    mailClient.Port = 25;
                    mailClient.UseDefaultCredentials = false;
                    System.Net.NetworkCredential authentication = new System.Net.NetworkCredential("gmsadmin@leedenlimited.com", "admin2008");
                    mailClient.Credentials = authentication;
                    mailClient.Send(mail);
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                mail.Dispose();
                if (File.Exists(filePath))
                     File.Delete(filePath);
                return true;
            }
            else return false;
        }
    }
}
