using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using GMSCore;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GMSCore.Activity;
using GMSCore.Entity;
using System.Data.SqlClient;


namespace GMSWeb.Reports.Report
{
    public partial class WebServiceReportViewer : GMSBasePage
    {
        protected ReportDocument crReportDocument;
        private short reportId = 0;
        string fileName = "";
        string fileDescription = "";
        protected short loginUserOrAlternateParty = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Product Report", ref lstAlterParty);
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


            if (reportId != -1 && reportId != -2 && reportId != -3)
            {

                GMSCore.Entity.UserAccessReport accessreport = UserAccessReport.RetrieveByKey(loginUserOrAlternateParty, reportId);
                GMSCore.Entity.UserAccessReportForCompany accessreportcoy = UserAccessReportForCompany.RetrieveByKey(session.CompanyId, loginUserOrAlternateParty, reportId);

                //if (accessreport == null && accessreportcoy == null)
                //{
                //    this.lblFeedback.Text = "You do not have access to this data.";
                //    return;
                //}

                fileName = new ReportsActivity().RetrieveReportById(this.reportId).FileName;
                fileDescription = new ReportsActivity().RetrieveReportById(this.reportId).Description;
                fileDescription = fileDescription.Substring(0, 1);

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


            if (IsControlAdded)
            {
                //JScriptAlertMsg("IsControlAdded");
                AddControls();
            }

            if (!IsPostBack)
            {
                AddControls();

            }
            //JScriptAlertMsg(DBManager.GetInstance().DatabaseName + DBManager.GetInstance().ServerName + DBManager.GetInstance().UserLoginName + DBManager.GetInstance().UserLoginPwd);
            if (IsPostBack)
            {
                RefreshCrystalReport();
            }

            string javaScript = @"<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        public bool IsControlAdded
        {
            get
            {
                if (ViewState["IsControlAdded"] == null)
                    ViewState["IsControlAdded"] = false;
                return (bool)ViewState["IsControlAdded"];
            }
            set { ViewState["IsControlAdded"] = value; }
        }

        private void AddControls()
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();


            int controlCount = 0;

            pnlParameter.Controls.Add(new LiteralControl("<table class=\"tTable1\" style=\"margin-left: 8px\" cellspacing=\"5\" cellpadding=\"5\" border=\"0\" width=\"709px\">"));

            if (crReportDocument.ParameterFields["@Date"] != null || crReportDocument.ParameterFields["@AsOfDate"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                // add new control
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Date"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                TextBox txtDate = new TextBox();
                txtDate.ID = "txtDate";
                txtDate.Columns = 15;
                txtDate.CssClass = "textbox";
                txtDate.Text = System.DateTime.Today.ToString("dd/MM/yyyy");
                pnlParameter.Controls.Add(txtDate);
                if (ViewState["txtDate"] == null)
                    ViewState["txtDate"] = System.DateTime.Today.ToString();
                pnlParameter.Controls.Add(new LiteralControl("<img id=\"img5\" src=\"../../images/imgCalendar.gif\" onclick=\"showCalendar(this, document.getElementById('txtDate'),  'dd/mm/yyyy', null, 1);\" height=\"20\" width=\"17\"  align=\"absMiddle\"  border=\"0\"></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Company"] != null)
            {

                GMSGeneralDALC dacl2 = new GMSGeneralDALC();
                DataSet dsCompanyName = new DataSet();
                dacl2.GetCompanyName(ref dsCompanyName);


                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Company"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlCompany = new DropDownList();
                ddlCompany.ID = "ddlCompany";
                ddlCompany.CssClass = "dropdownlist";
                ddlCompany.Items.Clear();

                for (int j = 0; j < dsCompanyName.Tables[0].Rows.Count; j++)
                {
                    ddlCompany.Items.Add(new ListItem(dsCompanyName.Tables[0].Rows[j]["CompanyName"].ToString(), dsCompanyName.Tables[0].Rows[j]["CoyID"].ToString()));
                }


                pnlParameter.Controls.Add(ddlCompany);
                if (ViewState["ddlCompany"] == null)
                    ViewState["ddlCompany"] = "0";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@StartDate"] != null || crReportDocument.ParameterFields["@StartDate"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                // add new control
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Start Date"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                TextBox txtStartDate = new TextBox();
                txtStartDate.ID = "txtStartDate";
                txtStartDate.Columns = 15;
                txtStartDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
                txtStartDate.CssClass = "textbox";
                pnlParameter.Controls.Add(txtStartDate);
                if (ViewState["txtStartDate"] == null)
                    ViewState["txtStartDate"] = txtStartDate.Text;
                pnlParameter.Controls.Add(new LiteralControl("<img id=\"img5\" src=\"../../images/imgCalendar.gif\" onclick=\"showCalendar(this, document.getElementById('txtStartDate'), 'dd/mm/yyyy', null, 1);\" height=\"20\" width=\"17\"  align=\"absMiddle\"  border=\"0\"></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@DateFrom"] != null || crReportDocument.ParameterFields["DateFrom"] != null || crReportDocument.ParameterFields["Date From"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Date From"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                TextBox txtDateFrom = new TextBox();
                txtDateFrom.ID = "txtDateFrom";
                txtDateFrom.Columns = 15;
                txtDateFrom.CssClass = "textbox";
                txtDateFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
                pnlParameter.Controls.Add(txtDateFrom);
                if (ViewState["txtDateFrom"] == null)
                    ViewState["txtDateFrom"] = txtDateFrom.Text;
                pnlParameter.Controls.Add(new LiteralControl("<img id=\"img5\" src=\"../../images/imgCalendar.gif\" onclick=\"showCalendar(this, document.getElementById('txtDateFrom'), 'dd/mm/yyyy', null, 1);\" height=\"20\" width=\"17\"  align=\"absMiddle\"  border=\"0\"></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@EndDate"] != null || crReportDocument.ParameterFields["@EndDate"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">End Date"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                TextBox txtEndDate = new TextBox();
                txtEndDate.ID = "txtEndDate";
                txtEndDate.Columns = 15;
                txtEndDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtEndDate.CssClass = "textbox";
                pnlParameter.Controls.Add(txtEndDate);
                if (ViewState["txtEndDate"] == null)
                    ViewState["txtEndDate"] = txtEndDate.Text;
                pnlParameter.Controls.Add(new LiteralControl("<img id=\"img6\" src=\"../../images/imgCalendar.gif\" onclick=\"showCalendar(this, document.getElementById('txtEndDate'), 'dd/mm/yyyy', null, 1);\" height=\"20\" width=\"17\"  align=\"absMiddle\"  border=\"0\"></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@DateTo"] != null || crReportDocument.ParameterFields["@@DateTo"] != null || crReportDocument.ParameterFields["DateTo"] != null || crReportDocument.ParameterFields["Date To"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Date To"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                TextBox txtDateTo = new TextBox();
                txtDateTo.ID = "txtDateTo";
                txtDateTo.Columns = 15;
                txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtDateTo.CssClass = "textbox";
                pnlParameter.Controls.Add(txtDateTo);
                if (ViewState["txtDateTo"] == null)
                    ViewState["txtDateTo"] = txtDateTo.Text;
                pnlParameter.Controls.Add(new LiteralControl("<img id=\"img6\" src=\"../../images/imgCalendar.gif\" onclick=\"showCalendar(this, document.getElementById('txtDateTo'), 'dd/mm/yyyy', null, 1);\" height=\"20\" width=\"17\"  align=\"absMiddle\"  border=\"0\"></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@Year"] != null || crReportDocument.ParameterFields["Year"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Year"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlYear = new DropDownList();
                ddlYear.ID = "ddlYear";
                ddlYear.CssClass = "dropdownlist";
                ddlYear.Items.Clear();
                for (int i = 2005; i <= 2020; i++)
                {
                    ddlYear.Items.Add(i.ToString());
                }

                if ((fileDescription == "S") || (fileDescription == "P"))
                    ddlYear.SelectedValue = DateTime.Now.Year.ToString();
                else
                    ddlYear.SelectedValue = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)).Year.ToString();

                pnlParameter.Controls.Add(ddlYear);
                if (ViewState["ddlYear"] == null)
                    ViewState["ddlYear"] = DateTime.Now.Year.ToString();
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@Month"] != null || crReportDocument.ParameterFields["Month"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Month"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlMonth = new DropDownList();
                ddlMonth.ID = "ddlMonth";
                ddlMonth.CssClass = "dropdownlist";
                ddlMonth.Items.Clear();
                for (int i = 1; i <= 12; i++)
                {
                    ddlMonth.Items.Add(i.ToString());
                }

                if ((fileDescription == "S") || (fileDescription == "P"))
                    ddlMonth.SelectedValue = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).Month.ToString();
                else
                    ddlMonth.SelectedValue = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)).Month.ToString();


                pnlParameter.Controls.Add(ddlMonth);
                if (ViewState["ddlMonth"] == null)
                    ViewState["ddlMonth"] = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)).Month.ToString();
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;

            }
            if (crReportDocument.ParameterFields["@SelectedCurrency"] != null && session.DefaultCurrency != "SGD")
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Currency"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlCurrency = new DropDownList();
                ddlCurrency.ID = "ddlCurrency";
                ddlCurrency.CssClass = "dropdownlist";
                ddlCurrency.Items.Clear();
                ddlCurrency.Items.Add(new ListItem("DEFAULT", "1"));
                ddlCurrency.Items.Add(new ListItem("SGD", "2"));
                pnlParameter.Controls.Add(ddlCurrency);
                if (ViewState["ddlCurrency"] == null)
                    ViewState["ddlCurrency"] = "1";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Currency"] != null && session.DefaultCurrency != "SGD")
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Currency"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlCurrency = new DropDownList();
                ddlCurrency.ID = "ddlCurrency";
                ddlCurrency.CssClass = "dropdownlist";
                ddlCurrency.Items.Clear();
                ddlCurrency.Items.Add(new ListItem("DEFAULT", "DEFAULT"));
                ddlCurrency.Items.Add(new ListItem("SGD", "SGD"));
                pnlParameter.Controls.Add(ddlCurrency);
                if (ViewState["ddlCurrency"] == null)
                    ViewState["ddlCurrency"] = "DEFAULT";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }


            if (crReportDocument.ParameterFields["Product Manager"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Product Manager"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                TextBox txtPM = new TextBox();
                txtPM.ID = "txtPM";
                txtPM.CssClass = "textbox";
                pnlParameter.Controls.Add(txtPM);
                if (ViewState["txtPM"] == null)
                    ViewState["txtPM"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. Alan</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Brand Code"] != null || crReportDocument.ParameterFields["BrandCode"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Product Group Code"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                TextBox txtBrandCode = new TextBox();
                txtBrandCode.ID = "txtBrandCode";
                txtBrandCode.CssClass = "textbox";
                pnlParameter.Controls.Add(txtBrandCode);
                if (ViewState["txtBrandCode"] == null)
                    ViewState["txtBrandCode"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. B11</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }


            if (crReportDocument.ParameterFields["Brand"] != null || crReportDocument.ParameterFields["Product Group"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Product Group Name"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                TextBox txtBrand = new TextBox();
                txtBrand.ID = "txtBrand";
                txtBrand.CssClass = "textbox";
                pnlParameter.Controls.Add(txtBrand);
                if (ViewState["txtBrand"] == null)
                    ViewState["txtBrand"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. BLUEMETALS</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Product Code"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Product Code"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                TextBox txtProductCode = new TextBox();
                txtProductCode.ID = "txtProductCode";
                txtProductCode.CssClass = "textbox";
                pnlParameter.Controls.Add(txtProductCode);
                if (ViewState["txtProductCode"] == null)
                    ViewState["txtProductCode"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. B1110535616</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Product Description"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Product Name"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                TextBox txtProductDescription = new TextBox();
                txtProductDescription.ID = "txtProductDescription";
                txtProductDescription.CssClass = "textbox";
                pnlParameter.Controls.Add(txtProductDescription);
                if (ViewState["txtProductDescription"] == null)
                    ViewState["txtProductDescription"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. BLUE-TIG 5356</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Customer Account Code"] != null || crReportDocument.ParameterFields["@AccountCode"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Customer Code"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                TextBox txtAccountCode = new TextBox();
                txtAccountCode.ID = "txtAccountCode";
                txtAccountCode.Text = "";
                txtAccountCode.CssClass = "textbox";
                pnlParameter.Controls.Add(txtAccountCode);
                if (ViewState["txtAccountCode"] == null)
                    ViewState["txtAccountCode"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. DLK690</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Customer Account Name"] != null || crReportDocument.ParameterFields["Customer Name"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Customer Name"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                TextBox txtAccountName = new TextBox();
                txtAccountName.ID = "txtAccountName";
                txtAccountName.Text = "";
                txtAccountName.CssClass = "textbox";
                pnlParameter.Controls.Add(txtAccountName);
                if (ViewState["txtAccountName"] == null)
                    ViewState["txtAccountName"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. Keppel</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["SalesPersonName"] != null || crReportDocument.ParameterFields["Sales Person Name"] != null || crReportDocument.ParameterFields["Salesperson"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Sales Person Name"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td colspan=\"2\">"));
                TextBox txtSalesPersonName = new TextBox();
                txtSalesPersonName.ID = "txtSalesPersonName";
                txtSalesPersonName.CssClass = "textbox";
                pnlParameter.Controls.Add(txtSalesPersonName);
                if (ViewState["txtSalesPersonName"] == null)
                    ViewState["txtSalesPersonName"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. Adrian</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Industry"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Industry"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td colspan=\"2\">"));
                TextBox txtIndustry = new TextBox();
                txtIndustry.ID = "txtIndustry";
                txtIndustry.CssClass = "textbox";
                pnlParameter.Controls.Add(txtIndustry);
                if (ViewState["txtIndustry"] == null)
                    ViewState["txtIndustry"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. Shipyards</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@TWHCode"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Target Warehouse Code"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                TextBox txtTWHCode = new TextBox();
                txtTWHCode.ID = "txtTWHCode";
                txtTWHCode.CssClass = "textbox";
                pnlParameter.Controls.Add(txtTWHCode);
                if (ViewState["txtTWHCode"] == null)
                    ViewState["txtTWHCode"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. A1</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@Type"] != null || crReportDocument.ParameterFields["Type"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Customer Type"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlType = new DropDownList();
                ddlType.ID = "ddlType";
                ddlType.CssClass = "dropdownlist";
                ddlType.Items.Clear();
                ddlType.Items.Add(new ListItem("ALL", "All"));
                ddlType.Items.Add(new ListItem("DIRECT CUSTOMERS", "External"));
                ddlType.Items.Add(new ListItem("INTERCO CUSTOMERS", "Internal"));
                if (session.CompanyId.ToString() == "14")
                {
                    ddlType.Items.Add(new ListItem("NOX CUSTOMERS", "NOX"));
                }
                pnlParameter.Controls.Add(ddlType);
                if (ViewState["ddlType"] == null)
                    ViewState["ddlType"] = "All";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@SalesPersonType"] != null || crReportDocument.ParameterFields["SalesPersonType"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Sales Person Type"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlSalesPersonType = new DropDownList();
                ddlSalesPersonType.ID = "ddlSalesPersonType";
                ddlSalesPersonType.CssClass = "dropdownlist";
                ddlSalesPersonType.Items.Clear();
                ddlSalesPersonType.Items.Add(new ListItem("Customer", "Customer"));
                ddlSalesPersonType.Items.Add(new ListItem("Invoice", "Invoice"));
                pnlParameter.Controls.Add(ddlSalesPersonType);
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@Rental"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Sales Type"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlRental = new DropDownList();
                ddlRental.ID = "ddlRental";
                ddlRental.CssClass = "dropdownlist";
                ddlRental.Items.Clear();
                ddlRental.Items.Add(new ListItem("ALL", "ALL"));
                if (session.DivisionId.ToString() == "4")
                {
                    ddlRental.Items.Add(new ListItem("WITH RENTAL", "WITH RENTAL"));
                }

                pnlParameter.Controls.Add(ddlRental);
                if (ViewState["ddlRental"] == null)
                    ViewState["ddlRental"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Sort By"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Sort By"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlSortBy = new DropDownList();
                ddlSortBy.ID = "ddlSortBy";
                ddlSortBy.CssClass = "dropdownlist";
                ddlSortBy.Items.Clear();
                ddlSortBy.Items.Add(new ListItem("Date", "Date"));
                ddlSortBy.Items.Add(new ListItem("Keyed User", "Keyed User"));
                ddlSortBy.Items.Add(new ListItem("Product Name", "Product Name"));
                ddlSortBy.Items.Add(new ListItem("Product Code", "Product Code"));
                pnlParameter.Controls.Add(ddlSortBy);
                if (ViewState["ddlSortBy"] == null)
                    ViewState["ddlSortBy"] = "Date";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["CreditSortBy"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Sort By"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlCreditSortBy = new DropDownList();
                ddlCreditSortBy.ID = "ddlCreditSortBy";
                ddlCreditSortBy.CssClass = "dropdownlist";
                ddlCreditSortBy.Items.Clear();
                ddlCreditSortBy.Items.Add(new ListItem("Credit Limit", "CreditLimit"));
                ddlCreditSortBy.Items.Add(new ListItem("Customer Name", "AccountName"));
                ddlCreditSortBy.Items.Add(new ListItem("Sales Person Name", "SalesPersonName"));
                pnlParameter.Controls.Add(ddlCreditSortBy);
                if (ViewState["ddlCreditSortBy"] == null)
                    ViewState["ddlCreditSortBy"] = "Credit Limit";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (session.DivisionId.ToString() == "4" && crReportDocument.ParameterFields["AccountType"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Account Type"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlAccountType = new DropDownList();
                ddlAccountType.ID = "ddlAccountType";
                ddlAccountType.CssClass = "dropdownlist";
                ddlAccountType.Items.Clear();


                ddlAccountType.Items.Add(new ListItem("Normal", "Normal"));
                ddlAccountType.Items.Add(new ListItem("Bill Lost", "BL"));
                ddlAccountType.Items.Add(new ListItem("ALL", "ALL"));



                pnlParameter.Controls.Add(ddlAccountType);
                if (ViewState["ddlAccountType"] == null)
                    ViewState["ddlAccountType"] = "ALL";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["DebtorSortBy"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Sort By"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlDebtorSortBy = new DropDownList();
                ddlDebtorSortBy.ID = "ddlDebtorSortBy";
                ddlDebtorSortBy.CssClass = "dropdownlist";
                ddlDebtorSortBy.Items.Clear();
                ddlDebtorSortBy.Items.Add(new ListItem("Credit Limit", "Credit Limit"));
                ddlDebtorSortBy.Items.Add(new ListItem("Customer Name", "Customer Name"));

                pnlParameter.Controls.Add(ddlDebtorSortBy);
                if (ViewState["ddlDebtorSortBy"] == null)
                    ViewState["ddlDebtorSortBy"] = "Credit Limit";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Country"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Country"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlCountry = new DropDownList();
                ddlCountry.ID = "ddlCountry";
                ddlCountry.CssClass = "dropdownlist";
                ddlCountry.Items.Clear();
                ddlCountry.Items.Add(new ListItem("ALL", "THAILAND,PHILIPPINES,VIETNAM,INDONESIA"));
                ddlCountry.Items.Add(new ListItem("THAILAND", "THAILAND"));
                ddlCountry.Items.Add(new ListItem("PHILIPPINES", "PHILIPPINES"));
                ddlCountry.Items.Add(new ListItem("VIETNAM", "VIETNAM"));
                ddlCountry.Items.Add(new ListItem("INDONESIA", "INDONESIA"));

                pnlParameter.Controls.Add(ddlCountry);
                if (ViewState["ddlCountry"] == null)
                    ViewState["ddlCountry"] = "ALL";

                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Territory"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Country"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlTerritory = new DropDownList();
                ddlTerritory.ID = "ddlTerritory";
                ddlTerritory.CssClass = "dropdownlist";
                ddlTerritory.Items.Clear();

                DataSet coyTerritory = new DataSet();
                new GMSGeneralDALC().GetCountryByCoy(session.CompanyId, ref coyTerritory);
                if (coyTerritory != null && coyTerritory.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < coyTerritory.Tables[0].Rows.Count; j++)
                    {

                        ddlTerritory.Items.Add(new ListItem(coyTerritory.Tables[0].Rows[j]["country"].ToString(), coyTerritory.Tables[0].Rows[j]["country"].ToString()));
                    }

                }

                ddlTerritory.Items.Add(new ListItem("ALL", ""));


                pnlParameter.Controls.Add(ddlTerritory);
                if (ViewState["ddlTerritory"] == null)
                {
                    ViewState["ddlTerritory"] = "";
                    ddlTerritory.SelectedValue = "";
                }

                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }





            if (crReportDocument.ParameterFields["Order By"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Sort By"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlOrderBy = new DropDownList();
                ddlOrderBy.ID = "ddlOrderBy";
                ddlOrderBy.CssClass = "dropdownlist";
                ddlOrderBy.Items.Clear();
                ddlOrderBy.Items.Add(new ListItem("Payment Date", "Payment Date"));
                ddlOrderBy.Items.Add(new ListItem("Customer Name", "Customer Name"));
                ddlOrderBy.Items.Add(new ListItem("Invoice Date", "Invoice Date"));

                pnlParameter.Controls.Add(ddlOrderBy);
                if (ViewState["ddlOrderBy"] == null)
                    ViewState["ddlOrderBy"] = "Payment Date";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;

            }

            if (crReportDocument.ParameterFields["Brand 1"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\" valign=\"top\">Product Group Name"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td valign=\"top\">:</td>"));


                pnlParameter.Controls.Add(new LiteralControl("<td> <span style=\"color:Green; size:7px; font-style:italic;\">Select Maximum of 10 Brands Only.</span>"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));



                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\" valign=\"top\">"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td valign=\"top\"></td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                ListBox lbProducts = new ListBox();
                lbProducts.ID = "lbProdcts";
                lbProducts.Width = 200;
                lbProducts.Height = 200;
                lbProducts.CssClass = "dropdownlist";
                lbProducts.SelectionMode = ListSelectionMode.Multiple;


                lbProducts.Items.Clear();
                lbProducts.AutoPostBack = false;
                ProductsDataDALC proddacl = new ProductsDataDALC();
                DataSet dsProducts = new DataSet();
                proddacl.GetProductGroup(session.CompanyId, ref dsProducts);

                if (dsProducts.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsProducts.Tables[0].Rows.Count; j++)
                    {
                        ListItem newitem = new ListItem();
                        newitem.Text = dsProducts.Tables[0].Rows[j]["ProductGroupCode"].ToString() + " - " + dsProducts.Tables[0].Rows[j]["ProductGroupName"].ToString();
                        newitem.Value = dsProducts.Tables[0].Rows[j]["ProductGroupCode"].ToString();
                        lbProducts.Items.Add(newitem);
                    }
                }


                pnlParameter.Controls.Add(lbProducts);
                if (ViewState["lbProdcts"] == null)
                    ViewState["lbProdcts"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                ImageButton ImgbtnRight = new ImageButton();
                ImgbtnRight.Click += new ImageClickEventHandler(btnRight_Click);
                ImgbtnRight.ImageUrl = "../../images/image1.jpg";
                ImgbtnRight.ImageAlign = ImageAlign.Middle;

                ImageButton ImgbtnLeft = new ImageButton();
                ImgbtnLeft.Click += new ImageClickEventHandler(btnLeft_Click);
                ImgbtnLeft.ImageUrl = "../../images/image2.jpg";
                ImgbtnLeft.ImageAlign = ImageAlign.Middle;

                pnlParameter.Controls.Add(ImgbtnRight);
                pnlParameter.Controls.Add(new LiteralControl("<br /><br /><br />"));
                pnlParameter.Controls.Add(ImgbtnLeft);

                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                ListBox lbProdctsSelected = new ListBox();
                lbProdctsSelected.ID = "lbProdctsSelected";
                lbProdctsSelected.Width = 200;
                lbProdctsSelected.Height = 200;
                lbProdctsSelected.AutoPostBack = false;
                lbProdctsSelected.CssClass = "dropdownlist";
                lbProdctsSelected.SelectionMode = ListSelectionMode.Multiple;
                lbProdctsSelected.Items.Clear();
                pnlParameter.Controls.Add(lbProdctsSelected);

                if (ViewState["lbProdctsSelected"] == null)
                    ViewState["lbProdctsSelected"] = null;

                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;

            }



            if (crReportDocument.ParameterFields["Employee Name"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Employee Name"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td colspan=\"2\">"));
                TextBox txtEmployeeName = new TextBox();
                txtEmployeeName.ID = "txtEmployeeName";
                txtEmployeeName.CssClass = "textbox";
                pnlParameter.Controls.Add(txtEmployeeName);
                if (ViewState["txtEmployeeName"] == null)
                    ViewState["txtEmployeeName"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. Adrian</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Course Title"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Course Title"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td colspan=\"2\">"));
                TextBox txtCourseTitle = new TextBox();
                txtCourseTitle.ID = "txtCourseTitle";
                txtCourseTitle.CssClass = "textbox";
                pnlParameter.Controls.Add(txtCourseTitle);
                if (ViewState["txtCourseTitle"] == null)
                    ViewState["txtCourseTitle"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. Excel</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if ((crReportDocument.ParameterFields["NarrationType"] != null) || (crReportDocument.ParameterFields["@NarrationType"] != null))
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Type"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlNarrationType = new DropDownList();
                ddlNarrationType.ID = "ddlNarrationType";
                ddlNarrationType.CssClass = "dropdownlist";
                ddlNarrationType.Items.Clear();
                ddlNarrationType.Items.Add(new ListItem("All", ""));
                ddlNarrationType.Items.Add(new ListItem("Blanket", "blanket"));
                ddlNarrationType.Items.Add(new ListItem("Reserved", "reserv"));
                ddlNarrationType.Items.Add(new ListItem("Others", "others"));
                pnlParameter.Controls.Add(ddlNarrationType);
                if (ViewState["ddlNarrationType"] == null)
                    ViewState["ddlNarrationType"] = "All";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Credit Limit"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Credit Limit"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlCreditLimit = new DropDownList();
                ddlCreditLimit.ID = "ddlCreditLimit";
                ddlCreditLimit.CssClass = "dropdownlist";
                ddlCreditLimit.Items.Clear();
                ddlCreditLimit.Items.Add(new ListItem("ALL", "A"));
                ddlCreditLimit.Items.Add(new ListItem("5000 AND ABOVE", "G"));
                ddlCreditLimit.Items.Add(new ListItem("BELOW 5000", "L"));

                pnlParameter.Controls.Add(ddlCreditLimit);
                if (ViewState["ddlCreditLimit"] == null)
                    ViewState["ddlCreditLimit"] = "A";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@DayType"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Overdue Days"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlDayType = new DropDownList();
                ddlDayType.ID = "ddlDayType";
                ddlDayType.CssClass = "dropdownlist";
                ddlDayType.Items.Clear();
                if (Request.QueryString["REPORTID"] == "266")
                {
                    ddlDayType.Items.Add(new ListItem("> 90 Days", "90"));
                }
                else if (Request.QueryString["REPORTID"] == "267")
                {
                    ddlDayType.Items.Add(new ListItem("> 180 Days", "180"));
                }
                else
                {
                    ddlDayType.Items.Add(new ListItem("ALL", "0"));
                    ddlDayType.Items.Add(new ListItem("> 90 Days", "90"));
                    ddlDayType.Items.Add(new ListItem("> 180 Days", "180"));
                }

                pnlParameter.Controls.Add(ddlDayType);
                if (ViewState["ddlDayType"] == null)
                    ViewState["ddlDayType"] = "0";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Invoice No"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Invoice No"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td colspan=\"2\">"));
                TextBox txtInvoiceNo = new TextBox();
                txtInvoiceNo.ID = "txtInvoiceNo";
                txtInvoiceNo.CssClass = "textbox";
                pnlParameter.Controls.Add(txtInvoiceNo);
                if (ViewState["txtInvoiceNo"] == null)
                    ViewState["txtInvoiceNo"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. DN000123</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Group"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Group Name"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td colspan=\"2\">"));
                TextBox txtGroupName = new TextBox();
                txtGroupName.ID = "txtGroupName";
                txtGroupName.CssClass = "textbox";
                pnlParameter.Controls.Add(txtGroupName);
                if (ViewState["txtGroupName"] == null)
                    ViewState["txtGroupName"] = "";
                pnlParameter.Controls.Add(new LiteralControl(" <span style=\"color:Green; size:7px; font-style:italic;\">e.g. Keppel</span></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@COARangeStart"] != null || crReportDocument.ParameterFields["@COARangeEnd"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">COA Range"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>From "));

                TextBox txtCOARangeStart = new TextBox();
                txtCOARangeStart.ID = "txtCOARangeStart";
                txtCOARangeStart.Text = "1000";
                txtCOARangeStart.CssClass = "textbox";
                pnlParameter.Controls.Add(txtCOARangeStart);

                pnlParameter.Controls.Add(new LiteralControl(" to "));

                TextBox txtCOARangeEnd = new TextBox();
                txtCOARangeEnd.ID = "txtCOARangeEnd";
                txtCOARangeEnd.Text = "9999";
                txtCOARangeEnd.CssClass = "textbox";
                pnlParameter.Controls.Add(txtCOARangeEnd);

                if (ViewState["txtCOARangeStart"] == null)
                    ViewState["txtCOARangeStart"] = "";
                if (ViewState["txtCOARangeEnd"] == null)
                    ViewState["txtCOARangeEnd"] = "";

                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@CompanyCode"] != null)
            {
                GMSGeneralDALC dacl3 = new GMSGeneralDALC();
                DataSet dsCompanyCode = new DataSet();
                dacl3.GetCompanyName(ref dsCompanyCode);


                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Company Code"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlCompanyCode = new DropDownList();
                ddlCompanyCode.ID = "ddlCompanyCode";
                ddlCompanyCode.CssClass = "dropdownlist";
                ddlCompanyCode.Items.Clear();

                for (int j = 0; j < dsCompanyCode.Tables[0].Rows.Count; j++)
                {
                    ddlCompanyCode.Items.Add(new ListItem(dsCompanyCode.Tables[0].Rows[j]["CompanyCode"].ToString(), dsCompanyCode.Tables[0].Rows[j]["CoyID"].ToString()));
                }


                pnlParameter.Controls.Add(ddlCompanyCode);
                if (ViewState["ddlCompanyCode"] == null)
                    ViewState["ddlCompanyCode"] = "0";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }



            Button dynamicbutton = new Button();
            dynamicbutton.Click += new System.EventHandler(btnSubmit_Click);
            dynamicbutton.Text = "Submit";
            dynamicbutton.CssClass = "button";
            dynamicbutton.ID = "btnSubmit";

            pnlParameter.Controls.Add(new LiteralControl("<tr>"));
            pnlParameter.Controls.Add(new LiteralControl("<td></td><td></td><td></td><td></td><td></td><td></td>"));
            pnlParameter.Controls.Add(new LiteralControl("<td align=\"right\">"));
            pnlParameter.Controls.Add(dynamicbutton);
            pnlParameter.Controls.Add(new LiteralControl("</td>"));
            pnlParameter.Controls.Add(new LiteralControl("</table>"));


            IsControlAdded = true;

            if (controlCount == 0)
            {
                pnlParameter.Visible = false;
                RefreshCrystalReport();
            }

        }

        protected void btnRight_Click(object sender, EventArgs e)
        {

            ListBox lbProdcts = ((ListBox)pnlParameter.FindControl("lbProdcts"));
            ListBox lbProdctsSelected = ((ListBox)pnlParameter.FindControl("lbProdctsSelected"));



            int j = 0;
            int count = 0;

            while (j < lbProdcts.Items.Count)
            {
                if (lbProdcts.Items[j].Selected)
                {
                    count = count + 1;
                }
                j++;

            }


            if ((lbProdctsSelected.Items.Count + count) > 10)
                JScriptAlertMsg("Please select maximum of 10 Product Group Code.");
            else
            {

                int i = 0;

                while (i < lbProdcts.Items.Count)
                {
                    if (lbProdcts.Items[i].Selected)
                    {
                        string s_t = lbProdcts.Items[i].Text;
                        string s_v = lbProdcts.Items[i].Value;
                        lbProdctsSelected.Items.Add(new ListItem(s_t, s_v));
                        lbProdcts.Items.RemoveAt(i);
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        protected void btnLeft_Click(object sender, EventArgs e)
        {
            ListBox lbProdcts = ((ListBox)pnlParameter.FindControl("lbProdcts"));
            ListBox lbProdctsSelected = ((ListBox)pnlParameter.FindControl("lbProdctsSelected"));
            int i = 0;
            while (i < lbProdctsSelected.Items.Count)
            {
                if (lbProdctsSelected.Items[i].Selected)
                {
                    string s_t = lbProdctsSelected.Items[i].Text;
                    string s_v = lbProdctsSelected.Items[i].Value;
                    lbProdcts.Items.Add(new ListItem(s_t, s_v));
                    lbProdctsSelected.Items.RemoveAt(i);
                    i = 0;
                }
                else
                {
                    i++;
                }
            }

        }

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (crReportDocument.ParameterFields["@Year"] != null)
                ViewState["ddlYear"] = ((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["Year"] != null)
                ViewState["ddlYear"] = ((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@Month"] != null)
                ViewState["ddlMonth"] = ((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["Month"] != null)
                ViewState["ddlMonth"] = ((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@Type"] != null || crReportDocument.ParameterFields["Type"] != null)
                ViewState["ddlType"] = ((DropDownList)pnlParameter.FindControl("ddlType")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@SalesPersonType"] != null || crReportDocument.ParameterFields["SalesPersonType"] != null)
                ViewState["ddlSalesPersonType"] = ((DropDownList)pnlParameter.FindControl("ddlSalesPersonType")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@Rental"] != null)
                ViewState["ddlRental"] = ((DropDownList)pnlParameter.FindControl("ddlRental")).SelectedValue.ToString();



            if (crReportDocument.ParameterFields["@SelectedCurrency"] != null && session.DefaultCurrency != "SGD")
                ViewState["ddlCurrency"] = ((DropDownList)pnlParameter.FindControl("ddlCurrency")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["Currency"] != null && session.DefaultCurrency != "SGD")
                ViewState["ddlCurrency"] = ((DropDownList)pnlParameter.FindControl("ddlCurrency")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@Date"] != null || crReportDocument.ParameterFields["@AsOfDate"] != null)
                ViewState["txtDate"] = ((TextBox)pnlParameter.FindControl("txtDate")).Text.ToString();
            if (crReportDocument.ParameterFields["@StartDate"] != null)
                ViewState["txtStartDate"] = ((TextBox)pnlParameter.FindControl("txtStartDate")).Text.ToString();
            if (crReportDocument.ParameterFields["@DateFrom"] != null)
                ViewState["txtDateFrom"] = ((TextBox)pnlParameter.FindControl("txtDateFrom")).Text.ToString();
            if (crReportDocument.ParameterFields["DateFrom"] != null)
                ViewState["txtDateFrom"] = ((TextBox)pnlParameter.FindControl("txtDateFrom")).Text.ToString();
            if (crReportDocument.ParameterFields["Date From"] != null)
                ViewState["txtDateFrom"] = ((TextBox)pnlParameter.FindControl("txtDateFrom")).Text.ToString();
            if (crReportDocument.ParameterFields["@EndDate"] != null)
                ViewState["txtEndDate"] = ((TextBox)pnlParameter.FindControl("txtEndDate")).Text.ToString();
            if (crReportDocument.ParameterFields["@DateTo"] != null)
                ViewState["txtDateTo"] = ((TextBox)pnlParameter.FindControl("txtDateTo")).Text.ToString();
            if (crReportDocument.ParameterFields["DateTo"] != null)
                ViewState["txtDateTo"] = ((TextBox)pnlParameter.FindControl("txtDateTo")).Text.ToString();
            if (crReportDocument.ParameterFields["Date To"] != null)
                ViewState["txtDateTo"] = ((TextBox)pnlParameter.FindControl("txtDateTo")).Text.ToString();
            if (crReportDocument.ParameterFields["Product Manager"] != null)
                ViewState["txtPM"] = ((TextBox)pnlParameter.FindControl("txtPM")).Text.ToString();
            if (crReportDocument.ParameterFields["Brand Code"] != null)
                ViewState["txtBrandCode"] = ((TextBox)pnlParameter.FindControl("txtBrandCode")).Text.ToString();
            if (crReportDocument.ParameterFields["BrandCode"] != null)
                ViewState["txtBrandCode"] = ((TextBox)pnlParameter.FindControl("txtBrandCode")).Text.ToString();
            if (crReportDocument.ParameterFields["Brand"] != null || crReportDocument.ParameterFields["Product Group"] != null)
                ViewState["txtBrand"] = ((TextBox)pnlParameter.FindControl("txtBrand")).Text.ToString();
            if (crReportDocument.ParameterFields["Product Code"] != null)
                ViewState["txtProductCode"] = ((TextBox)pnlParameter.FindControl("txtProductCode")).Text.ToString();
            if (crReportDocument.ParameterFields["Product Description"] != null)
                ViewState["txtProductDescription"] = ((TextBox)pnlParameter.FindControl("txtProductDescription")).Text.ToString();
            if (crReportDocument.ParameterFields["@TWHCode"] != null)
                ViewState["txtTWHCode"] = ((TextBox)pnlParameter.FindControl("txtTWHCode")).Text.ToString();
            if (crReportDocument.ParameterFields["DebtorSortBy"] != null)
                ViewState["ddlDebtorSortBy"] = ((DropDownList)pnlParameter.FindControl("ddlDebtorSortBy")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["CreditSortBy"] != null)
                ViewState["ddlCreditSortBy"] = ((DropDownList)pnlParameter.FindControl("ddlCreditSortBy")).SelectedValue.ToString();

            if (session.DivisionId.ToString() == "4" && crReportDocument.ParameterFields["AccountType"] != null)
                ViewState["ddlAccountType"] = ((DropDownList)pnlParameter.FindControl("ddlAccountType")).SelectedValue.ToString();
            else if (session.DivisionId.ToString() != "4" && crReportDocument.ParameterFields["AccountType"] != null)
                ViewState["ddlAccountType"] = "ALL";

            if (crReportDocument.ParameterFields["Sort By"] != null)
                ViewState["ddlSortBy"] = ((DropDownList)pnlParameter.FindControl("ddlSortBy")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["Credit Limit"] != null)
                ViewState["ddlCreditLimit"] = ((DropDownList)pnlParameter.FindControl("ddlCreditLimit")).SelectedValue.ToString();

            if (crReportDocument.ParameterFields["@DayType"] != null)
                ViewState["ddlDayType"] = ((DropDownList)pnlParameter.FindControl("ddlDayType")).SelectedValue;

            if (crReportDocument.ParameterFields["Country"] != null)
                ViewState["ddlCountry"] = ((DropDownList)pnlParameter.FindControl("ddlCountry")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["Territory"] != null)
                ViewState["ddlTerritory"] = ((DropDownList)pnlParameter.FindControl("ddlTerritory")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["Order By"] != null)
                ViewState["ddlOrderBy"] = ((DropDownList)pnlParameter.FindControl("ddlOrderBy")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["Customer Account Name"] != null || crReportDocument.ParameterFields["Customer Name"] != null)
                ViewState["txtAccountName"] = ((TextBox)pnlParameter.FindControl("txtAccountName")).Text.ToString();
            if (crReportDocument.ParameterFields["Customer Account Code"] != null || crReportDocument.ParameterFields["@AccountCode"] != null)
                ViewState["txtAccountCode"] = ((TextBox)pnlParameter.FindControl("txtAccountCode")).Text.ToString();
            if (crReportDocument.ParameterFields["SalesPersonName"] != null || crReportDocument.ParameterFields["Sales Person Name"] != null || crReportDocument.ParameterFields["Salesperson"] != null)
                ViewState["txtSalesPersonName"] = ((TextBox)pnlParameter.FindControl("txtSalesPersonName")).Text.ToString();
            if (crReportDocument.ParameterFields["Industry"] != null)
                ViewState["txtIndustry"] = ((TextBox)pnlParameter.FindControl("txtIndustry")).Text.ToString();

            if ((crReportDocument.ParameterFields["NarrationType"] != null) || (crReportDocument.ParameterFields["@NarrationType"] != null))
                ViewState["ddlNarrationType"] = ((DropDownList)pnlParameter.FindControl("ddlNarrationType")).SelectedValue.ToString();

            if (crReportDocument.ParameterFields["Company"] != null)
                ViewState["ddlCompany"] = ((DropDownList)pnlParameter.FindControl("ddlCompany")).SelectedValue.ToString();

            if (crReportDocument.ParameterFields["Employee Name"] != null)
                ViewState["txtEmployeeName"] = ((TextBox)pnlParameter.FindControl("txtEmployeeName")).Text.ToString();

            if (crReportDocument.ParameterFields["Course Title"] != null)
                ViewState["txtCourseTitle"] = ((TextBox)pnlParameter.FindControl("txtCourseTitle")).Text.ToString();


            if (crReportDocument.ParameterFields["Invoice No"] != null)
                ViewState["txtInvoiceNo"] = ((TextBox)pnlParameter.FindControl("txtInvoiceNo")).Text.ToString();

            if (crReportDocument.ParameterFields["Group"] != null)
                ViewState["txtGroupName"] = ((TextBox)pnlParameter.FindControl("txtGroupName")).Text.ToString();

            if (crReportDocument.ParameterFields["@COARangeStart"] != null)
                ViewState["txtCOARangeStart"] = ((TextBox)pnlParameter.FindControl("txtCOARangeStart")).Text.ToString();

            if (crReportDocument.ParameterFields["@COARangeEnd"] != null)
                ViewState["txtCOARangeEnd"] = ((TextBox)pnlParameter.FindControl("txtCOARangeEnd")).Text.ToString();

            if (crReportDocument.ParameterFields["@CompanyCode"] != null)
                ViewState["ddlCompanyCode"] = ((DropDownList)pnlParameter.FindControl("ddlCompanyCode")).SelectedValue.ToString();

            if (crReportDocument.ParameterFields["Brand 1"] != null)
            {

                //ArrayList listProductSelected = new ArrayList();             
                ListBox lbProdctsSelected = ((ListBox)pnlParameter.FindControl("lbProdctsSelected"));

                if (lbProdctsSelected.Items.Count > 0)
                {
                    string[] listProductSelected = new string[lbProdctsSelected.Items.Count];
                    int i = 0;
                    for (i = 0; i < lbProdctsSelected.Items.Count; i++)
                    {
                        listProductSelected[i] = lbProdctsSelected.Items[i].Value;
                    }
                    ViewState.Add("lbProdctsSelected()", listProductSelected);
                }
                else
                {
                    ViewState.Add("lbProdctsSelected()", null);

                }


            }


            RefreshCrystalReport();
        }
        #endregion

        protected void Page_Unload(object sender, EventArgs e)
        {
            cyReportViewer.Dispose();
            cyReportViewer = null;


            //crReportDocument.Close();
            //crReportDocument.Dispose();


            GC.Collect();
        }

        private void RefreshCrystalReport()
        {
            //LogSession session = base.GetSessionInfo();
            //try
            //{
            //    bool allow = true;
            //    if (crReportDocument.ParameterFields["Brand 1"] != null)
            //    {
            //        string[] listProductSelected = (string[])ViewState["lbProdctsSelected()"];
            //        if (listProductSelected != null)
            //        {
            //            allow = true;
            //        }
            //        else
            //            allow = false;
            //    }

            //    if (allow)
            //    {

            //        //For report to check for GM,CM,SM,PM
            //        #region Checker
            //        DataSet ResultList = new DataSet();
            //        string AccessRight = string.Empty;
            //        string listing = string.Empty;
            //        new GMSGeneralDALC().WSVerifyUserRole(session.CompanyId, session.UserId, ref ResultList);
            //        if ((ResultList.Tables[0].Rows.Count > 0))
            //        {
            //            foreach (DataRow dr in ResultList.Tables[0].Rows)
            //            {
            //                if (!string.IsNullOrEmpty(dr["Result"].ToString()))
            //                {
            //                    //Determine AccessRight
            //                    if (string.IsNullOrEmpty(AccessRight))
            //                    {
            //                        if (dr["Result"].ToString() == "*GM*" || dr["Result"].ToString() == "*CM*")
            //                        {
            //                            AccessRight = "All";
            //                        }
            //                        else if (dr["Result"].ToString() == "*SM*")
            //                        {
            //                            AccessRight = "Sales";
            //                        }
            //                        else if (dr["Result"].ToString() == "*PM*")
            //                        {
            //                            AccessRight = "PM";
            //                        }
            //                    }

            //                    //Populate Listing
            //                    if (!dr["Result"].ToString().Contains("*"))
            //                    {
            //                        if (string.IsNullOrEmpty(listing))
            //                        {
            //                            listing = "'" + dr["Result"].ToString() + "'";
            //                        }
            //                        else
            //                        {
            //                            listing = listing + ",'" + dr["Result"].ToString() + "'";
            //                        }
            //                    }
            //                }
            //            }

            //        }
            //        #endregion

            //        //Only for report 417
            //        #region Report 417
            //        GMSWebService.GMSWebService ws = new GMSWebService.GMSWebService();
            //        if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
            //            ws.Url = session.WebServiceAddress.Trim();
            //        else
            //            ws.Url = "http://192.168.1.236/GMSWebService/GMSWebService.asmx";

            //        DataSet ds = ws.GetProductOnBODetail(session.CompanyId, AccessRight, listing);
            //        DataTable dt = ds.Tables[0];
            //        crReportDocument.SetDataSource(ds.Tables[0]);

            //        cyReportViewer.ReportSource = crReportDocument; 
            //        #endregion
            //    }
            //}
            //catch (Exception ex)
            //{
            //    this.lblFeedback.Text = ex.ToString();
            //}


        }
    }
}