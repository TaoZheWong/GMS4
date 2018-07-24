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

using CrystalDecisions.CrystalReports.Engine;
namespace GMSWeb.Reports
{
    public partial class DocViewer : GMSBasePage
    {
        private string type = "";
        private DateTime dateFrom = GMSCoreBase.DEFAULT_NO_DATE;
        private DateTime dateTo = GMSCoreBase.DEFAULT_NO_DATE;

        private ReportDocument report = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.type = Request.QueryString["TYPE"];
            this.dateFrom = GMSUtil.ToDate(Request.QueryString["DATEFROM"]);
            this.dateTo = GMSUtil.ToDate(Request.QueryString["DATETO"]);

            if (!Page.IsPostBack)
            {
                switch (this.type.ToUpper())
                {
                    case "NAMEREPORT":
                        this.report = LoadDocument("NAMEREPORT");
                        break;

                    case "PLREPORT":
                        this.report = LoadDocument("PLREPORT");
                        break;

                    default:
                        break;
                }
            }
        }
        protected void Page_UnLoad(object sender, EventArgs e)
        {
            if (this.report != null)
            {
                this.report.Close();
                this.report.Dispose();
            }
        }


        #region LoadDocument
        private ReportDocument LoadDocument(string docType)
        {
            ReportDocument report = null;
            LogSession session = base.GetSessionInfo();

            try
            {
                switch (docType)
                {
                    case "NAMEREPORT":
                        report = new PrintReportsActivity().GetNameReport(session);
                        break;

                    case "PLREPORT":
                        report = new PrintReportsActivity().GetPLReport();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.lblFeedback.Text = ex.Message;
            }

            if (report != null)
            {
                this.cyReportViewer.ReportSource = report;
            }
            else
            {
                //this.lblFeedback.Text = "Report not found";
            }
            return report;
        }
        #endregion

    }
}
