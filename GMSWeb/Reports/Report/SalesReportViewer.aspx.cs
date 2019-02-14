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
    public partial class SalesReportViewer : GMSBasePage
    {
        protected ReportDocument crReportDocument;
        private short reportId = 0;
        private string isClaim = "";
        string fileName = "";
        string fileDescription = "";
        string isLargeFont, isOptimizedTable;

        protected short loginUserOrAlternateParty = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }

            //Getting LargerFont Cookies
            HttpCookie isLargeFontCookie = Request.Cookies["isLargeFont"];
            if (null == isLargeFontCookie)
                isLargeFont = "";
            else
                isLargeFont = isLargeFontCookie.Value == "true" ? "largeFont" : "";

            //Getting optimizedtable Cookies
            HttpCookie isOptimizedTableCookie = Request.Cookies["isOptimizedTable"];
            if (null == isOptimizedTableCookie)
                isOptimizedTable = "";
            else
                isOptimizedTable = isOptimizedTableCookie.Value == "true" ? "optimizedTable" : "";
            //UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
            //                                                                49);
            //if (uAccess == null)
            //    Response.Redirect("../../Unauthorized.htm");

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
            this.isClaim = GMSUtil.ToStr(Request.QueryString["ISCLAIM"]);

            if (isClaim != "YES")
            {
                if (reportId != -1 && reportId != -2 && reportId != -3)
                {

                    GMSCore.Entity.UserAccessReport accessreport = UserAccessReport.RetrieveByKey(loginUserOrAlternateParty, reportId);
                    GMSCore.Entity.UserAccessReportForCompany accessreportcoy = UserAccessReportForCompany.RetrieveByKey(session.CompanyId, loginUserOrAlternateParty, reportId);

                    if (accessreport == null && accessreportcoy == null)
                    {
                        this.lblFeedback.Text = "You do not have access to this data.";
                        return;
                    }

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
            }else
            {
                fileName = "EntertainmentClaimForm.rpt";
                fileDescription = "Entertainment Claim Form";
                fileDescription = fileDescription.Substring(0, 1);
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
        private void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            short year = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue);
            short month = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue);
            string dept = (((DropDownList)pnlParameter.FindControl("ddlDepartment")).SelectedValue);
            DropDownList ddlSalesPersonID = pnlParameter.FindControl("ddlSalesPersonID") as DropDownList;
            ddlSalesPersonID.Items.Clear();


            DataSet dsSalesPerson = new DataSet();
            new GMSGeneralDALC().GetSalesExecForReport(session.CompanyId, loginUserOrAlternateParty, year, month, dept, ref dsSalesPerson);
            if (dsSalesPerson.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < dsSalesPerson.Tables[0].Rows.Count; j++)
                {
                    ddlSalesPersonID.Items.Add(new ListItem(dsSalesPerson.Tables[0].Rows[j]["SalesPersonID"].ToString() + " - " + dsSalesPerson.Tables[0].Rows[j]["SalesPersonName"].ToString(), dsSalesPerson.Tables[0].Rows[j]["SalesPersonID"].ToString()));
                }
            }

            ViewState["ddlDepartment"] = dept;
        }

        private void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            short year = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue);
            short month = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue);
            string dept = (((DropDownList)pnlParameter.FindControl("ddlDepartment")).SelectedValue);
            DropDownList ddlSalesPersonID = pnlParameter.FindControl("ddlSalesPersonID") as DropDownList;
            ddlSalesPersonID.Items.Clear();

            DataSet dsSalesPerson = new DataSet();
            new GMSGeneralDALC().GetSalesExecForReport(session.CompanyId, loginUserOrAlternateParty, year, month, dept ,ref dsSalesPerson);
            if (dsSalesPerson.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < dsSalesPerson.Tables[0].Rows.Count; j++)
                {
                    ddlSalesPersonID.Items.Add(new ListItem(dsSalesPerson.Tables[0].Rows[j]["SalesPersonID"].ToString() + " - " + dsSalesPerson.Tables[0].Rows[j]["SalesPersonName"].ToString(), dsSalesPerson.Tables[0].Rows[j]["SalesPersonID"].ToString()));
                }
            }
        }

        private void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            short year = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue);
            short month = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue);
            string dept = (((DropDownList)pnlParameter.FindControl("ddlDepartment")).SelectedValue);
            DropDownList ddlSalesPersonID = pnlParameter.FindControl("ddlSalesPersonID") as DropDownList;
            ddlSalesPersonID.Items.Clear();

            DataSet dsSalesPerson = new DataSet();
            new GMSGeneralDALC().GetSalesExecForReport(session.CompanyId, loginUserOrAlternateParty, year, month, dept,ref dsSalesPerson);
            if (dsSalesPerson.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < dsSalesPerson.Tables[0].Rows.Count; j++)
                {
                    ddlSalesPersonID.Items.Add(new ListItem(dsSalesPerson.Tables[0].Rows[j]["SalesPersonID"].ToString() + " - " + dsSalesPerson.Tables[0].Rows[j]["SalesPersonName"].ToString(), dsSalesPerson.Tables[0].Rows[j]["SalesPersonID"].ToString()));
                }
            }
        }

        private void AddControls()
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();

            int controlCount = 0;

            pnlParameter.Controls.Add(new LiteralControl("<div class='form-horizontal m-t-20'>"));

            if (crReportDocument.ParameterFields["@Date"] != null || crReportDocument.ParameterFields["@AsOfDate"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Date :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"input-group date\">"));
                TextBox txtDate = new TextBox();
                txtDate.ID = "txtDate";
                txtDate.Columns = 15;
                txtDate.CssClass = "form-control datepicker";
                txtDate.Text = System.DateTime.Today.ToString("dd/MM/yyyy");
                pnlParameter.Controls.Add(txtDate);
                if (ViewState["txtDate"] == null)
                    ViewState["txtDate"] = System.DateTime.Today.ToString();
                pnlParameter.Controls.Add(new LiteralControl("<span class=\"input-group-addon\"><i class=\"ti-calendar\"></i></span>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Company"] != null)
            {

                GMSGeneralDALC dacl2 = new GMSGeneralDALC();
                DataSet dsCompanyName = new DataSet();
                dacl2.GetCompanyName(ref dsCompanyName);


                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Company :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlCompany = new DropDownList();
                ddlCompany.ID = "ddlCompany";
                ddlCompany.CssClass = "form-control";
                ddlCompany.Items.Clear();

                for (int j = 0; j < dsCompanyName.Tables[0].Rows.Count; j++)
                {
                    ddlCompany.Items.Add(new ListItem(dsCompanyName.Tables[0].Rows[j]["CompanyName"].ToString(), dsCompanyName.Tables[0].Rows[j]["CoyID"].ToString()));
                }


                pnlParameter.Controls.Add(ddlCompany);
                if (ViewState["ddlCompany"] == null)
                    ViewState["ddlCompany"] = "0";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@StartDate"] != null || crReportDocument.ParameterFields["@StartDate"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Start Date :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"input-group date\">"));
                TextBox txtStartDate = new TextBox();
                txtStartDate.ID = "txtStartDate";
                txtStartDate.Columns = 15;
                txtStartDate.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("dd/MM/yyyy");
                txtStartDate.CssClass = "form-control datepicker";
                pnlParameter.Controls.Add(txtStartDate);
                if (ViewState["txtStartDate"] == null)
                    ViewState["txtStartDate"] = txtStartDate.Text;
                pnlParameter.Controls.Add(new LiteralControl("<span class=\"input-group-addon\"><i class=\"ti-calendar\"></i></span>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@DateFrom"] != null || crReportDocument.ParameterFields["DateFrom"] != null || crReportDocument.ParameterFields["Date From"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Date From :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"input-group date\">"));
                TextBox txtDateFrom = new TextBox();
                txtDateFrom.ID = "txtDateFrom";
                txtDateFrom.Columns = 15;
                txtDateFrom.CssClass = "form-control datepicker";
                txtDateFrom.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("dd/MM/yyyy");
                pnlParameter.Controls.Add(txtDateFrom);
                if (ViewState["txtDateFrom"] == null)
                    ViewState["txtDateFrom"] = txtDateFrom.Text;
                pnlParameter.Controls.Add(new LiteralControl("<span class=\"input-group-addon\"><i class=\"ti-calendar\"></i></span>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@EndDate"] != null || crReportDocument.ParameterFields["@EndDate"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">End Date :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"input-group date\">"));
                TextBox txtEndDate = new TextBox();
                txtEndDate.ID = "txtEndDate";
                txtEndDate.Columns = 15;
                txtEndDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtEndDate.CssClass = "form-control datepicker";
                pnlParameter.Controls.Add(txtEndDate);
                if (ViewState["txtEndDate"] == null)
                    ViewState["txtEndDate"] = txtEndDate.Text;
                pnlParameter.Controls.Add(new LiteralControl("<span class=\"input-group-addon\"><i class=\"ti-calendar\"></i></span>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@DateTo"] != null || crReportDocument.ParameterFields["@@DateTo"] != null || crReportDocument.ParameterFields["DateTo"] != null || crReportDocument.ParameterFields["Date To"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Date To :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"input-group date\">"));
                TextBox txtDateTo = new TextBox();
                txtDateTo.ID = "txtDateTo";
                txtDateTo.Columns = 15;
                txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtDateTo.CssClass = "form-control datepicker";
                pnlParameter.Controls.Add(txtDateTo);
                if (ViewState["txtDateTo"] == null)
                    ViewState["txtDateTo"] = txtDateTo.Text;
                pnlParameter.Controls.Add(new LiteralControl("<span class=\"input-group-addon\"><i class=\"ti-calendar\"></i></span>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@Year"] != null || crReportDocument.ParameterFields["Year"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Year :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlYear = new DropDownList();
                ddlYear.ID = "ddlYear";
                ddlYear.CssClass = "form-control";

                if (reportId.ToString() == "556")
                {
                    ddlYear.AutoPostBack = true;
                    ddlYear.SelectedIndexChanged += new EventHandler(ddlYear_SelectedIndexChanged);
                }

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
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@Month"] != null || crReportDocument.ParameterFields["Month"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Month :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlMonth = new DropDownList();
                ddlMonth.ID = "ddlMonth";
                ddlMonth.CssClass = "form-control";
                if (reportId.ToString() == "556")
                {
                    ddlMonth.AutoPostBack = true;
                    ddlMonth.SelectedIndexChanged += new EventHandler(ddlMonth_SelectedIndexChanged);
                }

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
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;

            }
            if (crReportDocument.ParameterFields["@SelectedCurrency"] != null && session.DefaultCurrency != "SGD")
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Currency :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlCurrency = new DropDownList();
                ddlCurrency.ID = "ddlCurrency";
                ddlCurrency.CssClass = "form-control";
                ddlCurrency.Items.Clear();
                ddlCurrency.Items.Add(new ListItem("DEFAULT", "1"));
                ddlCurrency.Items.Add(new ListItem("SGD", "2"));
                pnlParameter.Controls.Add(ddlCurrency);
                if (ViewState["ddlCurrency"] == null)
                    ViewState["ddlCurrency"] = "1";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Currency"] != null && session.DefaultCurrency != "SGD")
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Currency :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlCurrency = new DropDownList();
                ddlCurrency.ID = "ddlCurrency";
                ddlCurrency.CssClass = "form-control";
                ddlCurrency.Items.Clear();
                ddlCurrency.Items.Add(new ListItem("DEFAULT", "DEFAULT"));
                ddlCurrency.Items.Add(new ListItem("SGD", "SGD"));
                pnlParameter.Controls.Add(ddlCurrency);
                if (ViewState["ddlCurrency"] == null)
                    ViewState["ddlCurrency"] = "DEFAULT";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }


            if (crReportDocument.ParameterFields["Product Manager"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Product Manager :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtPM = new TextBox();
                txtPM.ID = "txtPM";
                txtPM.CssClass = "form-control";
                txtPM.Attributes["placeholder"] = "e.g. Alan";
                pnlParameter.Controls.Add(txtPM);
                if (ViewState["txtPM"] == null)
                    ViewState["txtPM"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Supplier"] != null )
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Supplier Name :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtSupplier = new TextBox();
                txtSupplier.ID = "txtSupplier";
                txtSupplier.CssClass = "form-control";
                txtSupplier.Attributes["placeholder"] = "e.g. ABC PTE LTD";
                pnlParameter.Controls.Add(txtSupplier);
                if (ViewState["txtSupplier"] == null)
                    ViewState["txtSupplier"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Brand Code"] != null || crReportDocument.ParameterFields["BrandCode"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Product Group Code :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtBrandCode = new TextBox();
                txtBrandCode.ID = "txtBrandCode";
                txtBrandCode.CssClass = "form-control";
                txtBrandCode.Attributes["placeholder"] = "e.g. B11";
                pnlParameter.Controls.Add(txtBrandCode);
                if (ViewState["txtBrandCode"] == null)
                    ViewState["txtBrandCode"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }


            if (crReportDocument.ParameterFields["Brand"] != null || crReportDocument.ParameterFields["Product Group"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Product Group Name :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtBrand = new TextBox();
                txtBrand.ID = "txtBrand";
                txtBrand.CssClass = "form-control";
                txtBrand.Attributes["placeholder"] = "e.g. BLUEMETALS";
                pnlParameter.Controls.Add(txtBrand);
                if (ViewState["txtBrand"] == null)
                    ViewState["txtBrand"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Product Code"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Product Code :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtProductCode = new TextBox();
                txtProductCode.ID = "txtProductCode";
                txtProductCode.CssClass = "form-control";
                txtProductCode.Attributes["placeholder"] = "e.g. B1110535616";
                pnlParameter.Controls.Add(txtProductCode);
                if (ViewState["txtProductCode"] == null)
                    ViewState["txtProductCode"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@ProductCodeHead"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Product Code Start with :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtProductCodeHead = new TextBox();
                txtProductCodeHead.ID = "txtProductCodeHead";
                txtProductCodeHead.CssClass = "form-control";
                txtProductCodeHead.Attributes["placeholder"] = "e.g. T02";
                pnlParameter.Controls.Add(txtProductCodeHead);
                if (ViewState["txtProductCodeHead"] == null)
                    ViewState["txtProductCodeHead"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@Location"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Product Location :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtLocation = new TextBox();
                txtLocation.ID = "txtLocation";
                txtLocation.CssClass = "form-control";
                txtLocation.Attributes["placeholder"] = "e.g. T02";
                pnlParameter.Controls.Add(txtLocation);
                if (ViewState["txtLocation"] == null)
                    ViewState["txtLocation"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Product Description"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Product Name :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtProductDescription = new TextBox();
                txtProductDescription.ID = "txtProductDescription";
                txtProductDescription.CssClass = "form-control";
                txtProductDescription.Attributes["placeholder"] = "e.g. BLUE-TIG 5356";
                pnlParameter.Controls.Add(txtProductDescription);
                if (ViewState["txtProductDescription"] == null)
                    ViewState["txtProductDescription"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Customer Account Code"] != null || crReportDocument.ParameterFields["@AccountCode"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Customer Code :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtAccountCode = new TextBox();
                txtAccountCode.ID = "txtAccountCode";
                txtAccountCode.Text = "";
                txtAccountCode.CssClass = "form-control";
                txtAccountCode.Attributes["placeholder"] = "e.g. DLK690";
                pnlParameter.Controls.Add(txtAccountCode);
                if (ViewState["txtAccountCode"] == null)
                    ViewState["txtAccountCode"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Customer Account Name"] != null || crReportDocument.ParameterFields["Customer Name"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Customer Name :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtAccountName = new TextBox();
                txtAccountName.ID = "txtAccountName";
                txtAccountName.Text = "";
                txtAccountName.CssClass = "form-control";
                txtAccountName.Attributes["placeholder"] = "e.g. Keppel";
                pnlParameter.Controls.Add(txtAccountName);
                if (ViewState["txtAccountName"] == null)
                    ViewState["txtAccountName"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["SalesPersonID"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sales Person Code :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtSalesPersonID = new TextBox();
                txtSalesPersonID.ID = "txtSalesPersonID";
                txtSalesPersonID.CssClass = "form-control";
                txtSalesPersonID.Attributes["placeholder"] = "e.g. Adrian";
                pnlParameter.Controls.Add(txtSalesPersonID);
                if (ViewState["txtSalesPersonID"] == null)
                    ViewState["txtSalesPersonID"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["SalesPersonName"] != null || crReportDocument.ParameterFields["Sales Person Name"] != null || crReportDocument.ParameterFields["Salesperson"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sales Person Name :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtSalesPersonName = new TextBox();
                txtSalesPersonName.ID = "txtSalesPersonName";
                txtSalesPersonName.CssClass = "form-control";
                txtSalesPersonName.Attributes["placeholder"] = "e.g. Adrian";
                pnlParameter.Controls.Add(txtSalesPersonName);
                if (ViewState["txtSalesPersonName"] == null)
                    ViewState["txtSalesPersonName"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Industry"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Industry :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtIndustry = new TextBox();
                txtIndustry.ID = "txtIndustry";
                txtIndustry.CssClass = "form-control";
                txtIndustry.Attributes["placeholder"] = "e.g. Shipyards";
                pnlParameter.Controls.Add(txtIndustry);
                if (ViewState["txtIndustry"] == null)
                    ViewState["txtIndustry"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@TWHCode"] != null)
            {
 
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Target Warehouse Code :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtTWHCode = new TextBox();
                txtTWHCode.ID = "txtTWHCode";
                txtTWHCode.CssClass = "form-control";
                txtTWHCode.Attributes["placeholder"] = "e.g. A1";
                pnlParameter.Controls.Add(txtTWHCode);
                if (ViewState["txtTWHCode"] == null)
                    ViewState["txtTWHCode"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@Type"] != null || crReportDocument.ParameterFields["Type"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Customer Type :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlType = new DropDownList();
                ddlType.ID = "ddlType";
                ddlType.CssClass = "form-control";
                ddlType.Items.Clear();
                ddlType.Items.Add(new ListItem("ALL", "All"));
                ddlType.Items.Add(new ListItem("EXTERNAL CUSTOMERS", "External"));
                ddlType.Items.Add(new ListItem("INTERCO CUSTOMERS", "Internal"));
                if (session.CompanyId.ToString() == "14")
                {
                    ddlType.Items.Add(new ListItem("NOX CUSTOMERS", "NOX"));
                }
                pnlParameter.Controls.Add(ddlType);
                if (ViewState["ddlType"] == null)
                    ViewState["ddlType"] = "All";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }


            if (crReportDocument.ParameterFields["@Zcode"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Z Code :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlZcode = new DropDownList();
                ddlZcode.ID = "ddlZcode";
                ddlZcode.CssClass = "form-control";
                ddlZcode.Items.Clear();
                ddlZcode.Items.Add(new ListItem("EXCLUDE", "EXCLUDE"));
                ddlZcode.Items.Add(new ListItem("INCLUDE", "INCLUDE"));


                pnlParameter.Controls.Add(ddlZcode);
                if (ViewState["ddlZcode"] == null)
                    ViewState["ddlZcode"] = "All";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));

                controlCount = controlCount + 1;

            }

            if (crReportDocument.ParameterFields["@RentalType"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Rental :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlRentalType = new DropDownList();
                ddlRentalType.ID = "ddlRentalType";
                ddlRentalType.CssClass = "form-control";
                ddlRentalType.Items.Clear();
                ddlRentalType.Items.Add(new ListItem("EXCLUDE RENTAL", "EXCLUDE RENTAL"));
                ddlRentalType.Items.Add(new ListItem("INCLUDE RENTAL", "INCLUDE RENTAL"));


                pnlParameter.Controls.Add(ddlRentalType);
                if (ViewState["ddlRentalType"] == null)
                    ViewState["ddlRentalType"] = "All";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));

                controlCount = controlCount + 1;

            }


            if (crReportDocument.ParameterFields["@SalesPersonType"] != null || crReportDocument.ParameterFields["SalesPersonType"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sales Person Type :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlSalesPersonType = new DropDownList();
                ddlSalesPersonType.ID = "ddlSalesPersonType";
                ddlSalesPersonType.CssClass = "form-control";
                ddlSalesPersonType.Items.Clear();
                ddlSalesPersonType.Items.Add(new ListItem("Customer", "Customer"));
                ddlSalesPersonType.Items.Add(new ListItem("Invoice", "Invoice"));
                pnlParameter.Controls.Add(ddlSalesPersonType);
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }
            if (crReportDocument.ParameterFields["@SalesType"] != null || crReportDocument.ParameterFields["SalesType"] != null)
            {
 
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sales Team :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlSalesType = new DropDownList();
                ddlSalesType.ID = "ddlSalesType";
                ddlSalesType.CssClass = "form-control";
                ddlSalesType.Items.Clear();
                ddlSalesType.Items.Add(new ListItem("All", "All"));
                if (session.CompanyId == 116 || session.CompanyId == 120)
                {
                    ddlSalesType.Items.Add(new ListItem("Sales Team A", "Welding"));
                    ddlSalesType.Items.Add(new ListItem("Sales Team B", "Safety"));
                }
                pnlParameter.Controls.Add(ddlSalesType);
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                if (session.CompanyId == 116 || session.CompanyId == 120)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<p>*Team A consists of Team A + Shared Accounts (Welding) <br> Team B consists of Team B + Shared Accounts (Safety) "));
                    pnlParameter.Controls.Add(new LiteralControl("</p>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                }
                controlCount = controlCount + 1;
            }
            if (crReportDocument.ParameterFields["@Rental"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sales Type :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlRental = new DropDownList();
                ddlRental.ID = "ddlRental";
                ddlRental.CssClass = "form-control";
                ddlRental.Items.Clear();
                ddlRental.Items.Add(new ListItem("ALL", "ALL"));
                if (session.DivisionId.ToString() == "4")
                {
                    ddlRental.Items.Add(new ListItem("WITH RENTAL", "WITH RENTAL"));
                }

                pnlParameter.Controls.Add(ddlRental);
                if (ViewState["ddlRental"] == null)
                    ViewState["ddlRental"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Sort By"] != null)
            {
           
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sort By :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlSortBy = new DropDownList();
                ddlSortBy.ID = "ddlSortBy";
                ddlSortBy.CssClass = "form-control";
                ddlSortBy.Items.Clear();
                ddlSortBy.Items.Add(new ListItem("Date", "Date"));
                ddlSortBy.Items.Add(new ListItem("Keyed User", "Keyed User"));
                ddlSortBy.Items.Add(new ListItem("Product Name", "Product Name"));
                ddlSortBy.Items.Add(new ListItem("Product Code", "Product Code"));
                pnlParameter.Controls.Add(ddlSortBy);
                if (ViewState["ddlSortBy"] == null)
                    ViewState["ddlSortBy"] = "Date";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["CreditSortBy"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sort By :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlCreditSortBy = new DropDownList();
                ddlCreditSortBy.ID = "ddlCreditSortBy";
                ddlCreditSortBy.CssClass = "form-control";
                ddlCreditSortBy.Items.Clear();
                ddlCreditSortBy.Items.Add(new ListItem("Credit Limit", "CreditLimit"));
                ddlCreditSortBy.Items.Add(new ListItem("Customer Name", "AccountName"));
                ddlCreditSortBy.Items.Add(new ListItem("Sales Person Name", "SalesPersonName"));
                pnlParameter.Controls.Add(ddlCreditSortBy);
                if (ViewState["ddlCreditSortBy"] == null)
                    ViewState["ddlCreditSortBy"] = "Credit Limit";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (session.DivisionId.ToString() == "4" && crReportDocument.ParameterFields["AccountType"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Account Type :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlAccountType = new DropDownList();
                ddlAccountType.ID = "ddlAccountType";
                ddlAccountType.CssClass = "form-control";
                ddlAccountType.Items.Clear();


                ddlAccountType.Items.Add(new ListItem("Normal", "Normal"));
                ddlAccountType.Items.Add(new ListItem("Bill Lost", "BL"));
                ddlAccountType.Items.Add(new ListItem("ALL", "ALL"));



                pnlParameter.Controls.Add(ddlAccountType);
                if (ViewState["ddlAccountType"] == null)
                    ViewState["ddlAccountType"] = "ALL";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["DebtorSortBy"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sort By :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlDebtorSortBy = new DropDownList();
                ddlDebtorSortBy.ID = "ddlDebtorSortBy";
                ddlDebtorSortBy.CssClass = "form-control";
                ddlDebtorSortBy.Items.Clear();
                ddlDebtorSortBy.Items.Add(new ListItem("Credit Limit", "Credit Limit"));
                ddlDebtorSortBy.Items.Add(new ListItem("Customer Name", "Customer Name"));

                pnlParameter.Controls.Add(ddlDebtorSortBy);
                if (ViewState["ddlDebtorSortBy"] == null)
                    ViewState["ddlDebtorSortBy"] = "Credit Limit";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Country"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Country :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlCountry = new DropDownList();
                ddlCountry.ID = "ddlCountry";
                ddlCountry.CssClass = "form-control";
                ddlCountry.Items.Clear();
                ddlCountry.Items.Add(new ListItem("ALL", "THAILAND,PHILIPPINES,VIETNAM,INDONESIA"));
                ddlCountry.Items.Add(new ListItem("THAILAND", "THAILAND"));
                ddlCountry.Items.Add(new ListItem("PHILIPPINES", "PHILIPPINES"));
                ddlCountry.Items.Add(new ListItem("VIETNAM", "VIETNAM"));
                ddlCountry.Items.Add(new ListItem("INDONESIA", "INDONESIA"));

                pnlParameter.Controls.Add(ddlCountry);
                if (ViewState["ddlCountry"] == null)
                    ViewState["ddlCountry"] = "ALL";

                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Territory"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Country :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlTerritory = new DropDownList();
                ddlTerritory.ID = "ddlTerritory";
                ddlTerritory.CssClass = "form-control";
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

                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }





            if (crReportDocument.ParameterFields["Order By"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sort By :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlOrderBy = new DropDownList();
                ddlOrderBy.ID = "ddlOrderBy";
                ddlOrderBy.CssClass = "form-control";
                ddlOrderBy.Items.Clear();
                ddlOrderBy.Items.Add(new ListItem("Payment Date", "Payment Date"));
                ddlOrderBy.Items.Add(new ListItem("Customer Name", "Customer Name"));
                ddlOrderBy.Items.Add(new ListItem("Invoice Date", "Invoice Date"));

                pnlParameter.Controls.Add(ddlOrderBy);
                if (ViewState["ddlOrderBy"] == null)
                    ViewState["ddlOrderBy"] = "Payment Date";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
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
                lbProducts.CssClass = "form-control";
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
                lbProdctsSelected.CssClass = "form-control";
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

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Employee Name :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtEmployeeName = new TextBox();
                txtEmployeeName.ID = "txtEmployeeName";
                txtEmployeeName.CssClass = "form-control";
                txtEmployeeName.Attributes["placeholder"] = "e.g. Adrian";
                pnlParameter.Controls.Add(txtEmployeeName);
                if (ViewState["txtEmployeeName"] == null)
                    ViewState["txtEmployeeName"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Course Title"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Course Title :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtCourseTitle = new TextBox();
                txtCourseTitle.ID = "txtCourseTitle";
                txtCourseTitle.CssClass = "form-control";
                txtCourseTitle.Attributes["placeholder"] = "e.g. Excel";
                pnlParameter.Controls.Add(txtCourseTitle);
                if (ViewState["txtCourseTitle"] == null)
                    ViewState["txtCourseTitle"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if ((crReportDocument.ParameterFields["NarrationType"] != null) || (crReportDocument.ParameterFields["@NarrationType"] != null))
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Type :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlNarrationType = new DropDownList();
                ddlNarrationType.ID = "ddlNarrationType";
                ddlNarrationType.CssClass = "form-control";
                ddlNarrationType.Items.Clear();
                ddlNarrationType.Items.Add(new ListItem("All", ""));
                ddlNarrationType.Items.Add(new ListItem("Blanket", "blanket"));
                ddlNarrationType.Items.Add(new ListItem("Reserved", "reserv"));
                ddlNarrationType.Items.Add(new ListItem("Closed Order", "CLOSED ORDER"));
                ddlNarrationType.Items.Add(new ListItem("Others", "others"));
                pnlParameter.Controls.Add(ddlNarrationType);
                if (ViewState["ddlNarrationType"] == null)
                    ViewState["ddlNarrationType"] = "All";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Credit Limit"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Credit Limit :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlCreditLimit = new DropDownList();
                ddlCreditLimit.ID = "ddlCreditLimit";
                ddlCreditLimit.CssClass = "form-control";
                ddlCreditLimit.Items.Clear();
                ddlCreditLimit.Items.Add(new ListItem("ALL", "A"));
                ddlCreditLimit.Items.Add(new ListItem("5000 AND ABOVE", "G"));
                ddlCreditLimit.Items.Add(new ListItem("BELOW 5000", "L"));

                pnlParameter.Controls.Add(ddlCreditLimit);
                if (ViewState["ddlCreditLimit"] == null)
                    ViewState["ddlCreditLimit"] = "A";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@DayType"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Overdue Days :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlDayType = new DropDownList();
                ddlDayType.ID = "ddlDayType";
                ddlDayType.CssClass = "form-control";
                ddlDayType.Items.Clear();
                if (Request.QueryString["REPORTID"] == "266")
                {
                    ddlDayType.Items.Add(new ListItem("> 90 Days", "90"));
                }
                else if (Request.QueryString["REPORTID"] == "267")
                {
                    ddlDayType.Items.Add(new ListItem("> 120 Days", "120"));
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
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Invoice No"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Invoice No :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtInvoiceNo = new TextBox();
                txtInvoiceNo.ID = "txtInvoiceNo";
                txtInvoiceNo.CssClass = "form-control";
                txtInvoiceNo.Attributes["placeholder"] = "e.g. DN000123";
                pnlParameter.Controls.Add(txtInvoiceNo);
                if (ViewState["txtInvoiceNo"] == null)
                    ViewState["txtInvoiceNo"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Group"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Group Name :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtGroupName = new TextBox();
                txtGroupName.ID = "txtGroupName";
                txtGroupName.CssClass = "form-control";
                txtGroupName.Attributes["placeholder"] = "e.g. Keppel";
                pnlParameter.Controls.Add(txtGroupName);
                if (ViewState["txtGroupName"] == null)
                    ViewState["txtGroupName"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@COARangeStart"] != null || crReportDocument.ParameterFields["@COARangeEnd"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">COA Range :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));

                TextBox txtCOARangeStart = new TextBox();
                txtCOARangeStart.ID = "txtCOARangeStart";
                txtCOARangeStart.Text = "1000";
                txtCOARangeStart.CssClass = "form-control";
                pnlParameter.Controls.Add(txtCOARangeStart);

                pnlParameter.Controls.Add(new LiteralControl(" to "));

                TextBox txtCOARangeEnd = new TextBox();
                txtCOARangeEnd.ID = "txtCOARangeEnd";
                txtCOARangeEnd.Text = "9999";
                txtCOARangeEnd.CssClass = "form-control";
                pnlParameter.Controls.Add(txtCOARangeEnd);

                if (ViewState["txtCOARangeStart"] == null)
                    ViewState["txtCOARangeStart"] = "";
                if (ViewState["txtCOARangeEnd"] == null)
                    ViewState["txtCOARangeEnd"] = "";

                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));

                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@CompanyCode"] != null)
            {
                GMSGeneralDALC dacl3 = new GMSGeneralDALC();
                DataSet dsCompanyCode = new DataSet();
                dacl3.GetCompanyName(ref dsCompanyCode);

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Company Code :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlCompanyCode = new DropDownList();
                ddlCompanyCode.ID = "ddlCompanyCode";
                ddlCompanyCode.CssClass = "form-control";
                ddlCompanyCode.Items.Clear();

                for (int j = 0; j < dsCompanyCode.Tables[0].Rows.Count; j++)
                {
                    ddlCompanyCode.Items.Add(new ListItem(dsCompanyCode.Tables[0].Rows[j]["CompanyCode"].ToString(), dsCompanyCode.Tables[0].Rows[j]["CoyID"].ToString()));
                }


                pnlParameter.Controls.Add(ddlCompanyCode);
                if (ViewState["ddlCompanyCode"] == null)
                    ViewState["ddlCompanyCode"] = "0";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@Scheme"] != null || crReportDocument.ParameterFields["Scheme"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Type :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlScheme= new DropDownList();
                ddlScheme.ID = "ddlScheme";
                ddlScheme.CssClass = "form-control";
                ddlScheme.Items.Clear();
                ddlScheme.Items.Add(new ListItem("Standard", "Standard"));
                ddlScheme.Items.Add(new ListItem("Rental", "Rental"));
                
                pnlParameter.Controls.Add(ddlScheme);
                if (ViewState["ddlScheme"] == null)
                    ViewState["ddlScheme"] = "Standard";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@MRNo"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">MR No :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                TextBox txtMRNo = new TextBox();
                txtMRNo.ID = "txtMRNo";
                txtMRNo.CssClass = "form-control";
                txtMRNo.Attributes["placeholder"] = "e.g. MR0000000";
                pnlParameter.Controls.Add(txtMRNo);
                if (ViewState["txtMRNo"] == null)
                    ViewState["txtMRNo"] = "";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["Status"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\" valign=\"top\">Enter Status"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td valign=\"top\">:</td>"));
                
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\" valign=\"top\">"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td valign=\"top\"></td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                ListBox lbStatus = new ListBox();
                lbStatus.ID = "lbStatus";
                lbStatus.Width = 200;
                lbStatus.Height = 200;
                lbStatus.CssClass = "form-control";
                lbStatus.SelectionMode = ListSelectionMode.Multiple;


                lbStatus.Items.Clear();
                lbStatus.AutoPostBack = false;

                string[] tempStatus = new string[] { "New","Ongoing","Completed","Closed","Cancelled" };

                foreach (string item in tempStatus)
                {
                    ListItem newitem = new ListItem();
                    newitem.Text = item;
                    newitem.Value = item;
                    lbStatus.Items.Add(newitem);
                }
                
                pnlParameter.Controls.Add(lbStatus);
                if (ViewState["lbStatus"] == null)
                    ViewState["lbStatus"] = "";
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
                ListBox lbStatusSelected = new ListBox();
                lbStatusSelected.ID = "lbStatusSelected";
                lbStatusSelected.Width = 200;
                lbStatusSelected.Height = 200;
                lbStatusSelected.AutoPostBack = false;
                lbStatusSelected.CssClass = "form-control";
                lbStatusSelected.SelectionMode = ListSelectionMode.Multiple;
                lbStatusSelected.Items.Clear();
                pnlParameter.Controls.Add(lbStatusSelected);

                if (ViewState["lbStatusSelected"] == null)
                    ViewState["lbStatusSelected"] = null;

                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@Display"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Display :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlDisplay = new DropDownList();
                ddlDisplay.ID = "ddlDisplay";
                ddlDisplay.CssClass = "form-control";
                ddlDisplay.Items.Clear();
                ddlDisplay.Items.Add(new ListItem("In thousands", "1000"));
                ddlDisplay.Items.Add(new ListItem("Actual figure", "1"));
                pnlParameter.Controls.Add(ddlDisplay);
                if (ViewState["ddlDisplay"] == null)
                    ViewState["ddlDisplay"] = "1000";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@Department"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Department :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlDepartment = new DropDownList();
                ddlDepartment.ID = "ddlDepartment";
                ddlDepartment.CssClass = "form-control";              
                ddlDepartment.Items.Clear();

                DataSet dsDepartment = new DataSet();
                new GMSGeneralDALC().GetDepartmentForReport(session.CompanyId, reportId, loginUserOrAlternateParty, ref dsDepartment);
                if (dsDepartment.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsDepartment.Tables[0].Rows.Count; j++)
                    {
                        ddlDepartment.Items.Add(new ListItem(dsDepartment.Tables[0].Rows[j]["DepartmentName"].ToString(), dsDepartment.Tables[0].Rows[j]["DepartmentName"].ToString()));
                    }
                }
                                   
                    if (reportId.ToString() == "556")
                    {
                        ddlDepartment.AutoPostBack = true;
                        ddlDepartment.SelectedIndexChanged += new EventHandler(ddlDepartment_SelectedIndexChanged);
                    }

                pnlParameter.Controls.Add(ddlDepartment);
                
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;

                if (ViewState["ddlDepartment"] == null)
                    ViewState["ddlDepartment"] = "NONE";

            }


            if (crReportDocument.ParameterFields["@SalesPersonID"] != null)
            {

                short year = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue);
                short month = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue);
                string dept = (((DropDownList)pnlParameter.FindControl("ddlDepartment")).SelectedValue);
                DataSet dsSalesPerson = new DataSet();
                new GMSGeneralDALC().GetSalesExecForReport(session.CompanyId, loginUserOrAlternateParty, year, month, dept, ref dsSalesPerson);
                if (dsSalesPerson.Tables[0].Rows.Count > 0)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sales Person :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlSalesPersonID = new DropDownList();
                    ddlSalesPersonID.ID = "ddlSalesPersonID";
                    ddlSalesPersonID.CssClass = "form-control";

                    ddlSalesPersonID.Items.Clear();

                    for (int j = 0; j < dsSalesPerson.Tables[0].Rows.Count; j++)
                    {
                        ddlSalesPersonID.Items.Add(new ListItem(dsSalesPerson.Tables[0].Rows[j]["SalesPersonID"].ToString() + " - " + dsSalesPerson.Tables[0].Rows[j]["SalesPersonName"].ToString(), dsSalesPerson.Tables[0].Rows[j]["SalesPersonID"].ToString()));
                    }

                    pnlParameter.Controls.Add(ddlSalesPersonID);


                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;

                }
                if (ViewState["ddlSalesPersonID"] == null)
                    ViewState["ddlSalesPersonID"] = -1;

            }

            if (crReportDocument.ParameterFields["@MRScheme"] != null) {
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Division :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlMRScheme = new DropDownList();
                ddlMRScheme.ID = "ddlMRScheme";
                ddlMRScheme.CssClass = "form-control";
                ddlMRScheme.Items.Clear();
                if(session.CompanyId == 120) {
                    ddlMRScheme.Items.Add(new ListItem("Gas", "Department"));
                    ddlMRScheme.Items.Add(new ListItem("Welding", "Product"));
                } else {
                    ddlMRScheme.Items.Add(new ListItem("--", "Department"));
                }
                pnlParameter.Controls.Add(ddlMRScheme);
                if (ViewState["ddlMRScheme"] == null)
                    ViewState["ddlMRScheme"] = "Department";
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                pnlParameter.Controls.Add(new LiteralControl("</div>"));
                controlCount = controlCount + 1;
            }

            Button dynamicbutton = new Button();
            dynamicbutton.Click += new System.EventHandler(btnSubmit_Click);
            dynamicbutton.Text = "Submit";
            dynamicbutton.CssClass = "btn btn-primary pull-right";
            dynamicbutton.ID = "btnSubmit";

            pnlParameter.Controls.Add(new LiteralControl("</div>"));
            pnlParameter.Controls.Add(new LiteralControl("</div>"));
            pnlParameter.Controls.Add(new LiteralControl("<div class='panel-footer clearfix'>"));
            pnlParameter.Controls.Add(dynamicbutton);
            pnlParameter.Controls.Add(new LiteralControl("</div>"));


            IsControlAdded = true;

            if (controlCount == 0)
            {
                pnlParameter.Visible = false;
                RefreshCrystalReport();
            }

        }

        protected void btnRight_Click(object sender, EventArgs e)
        {
            ListBox lbListItem = null;
            ListBox lbListItemSelected = null;

            if (crReportDocument.ParameterFields["Brand 1"] != null)
            {
                lbListItem = ((ListBox)pnlParameter.FindControl("lbProdcts"));
                lbListItemSelected = ((ListBox)pnlParameter.FindControl("lbProdctsSelected"));
            }

            if (crReportDocument.ParameterFields["Status"] != null)
            {
                lbListItem = ((ListBox)pnlParameter.FindControl("lbStatus"));
                lbListItemSelected = ((ListBox)pnlParameter.FindControl("lbStatusSelected"));
            }

            int j = 0;
            int count = 0;

            while (j < lbListItem.Items.Count)
            {
                if (lbListItem.Items[j].Selected)
                {
                    count = count + 1;
                }
                j++;

            }


            if ((lbListItemSelected.Items.Count + count) > 10)
                JScriptAlertMsg("Please select maximum of 10 Product Group Code.");
            else
            {

                int i = 0;

                while (i < lbListItem.Items.Count)
                {
                    if (lbListItem.Items[i].Selected)
                    {
                        string s_t = lbListItem.Items[i].Text;
                        string s_v = lbListItem.Items[i].Value;
                        lbListItemSelected.Items.Add(new ListItem(s_t, s_v));
                        lbListItem.Items.RemoveAt(i);
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
            ListBox lbListItem = null;
            ListBox lbListItemSelected = null;

            if (crReportDocument.ParameterFields["Brand 1"] != null)
            {
                lbListItem = ((ListBox)pnlParameter.FindControl("lbProdcts"));
                lbListItemSelected = ((ListBox)pnlParameter.FindControl("lbProdctsSelected"));
            }

            if (crReportDocument.ParameterFields["Status"] != null)
            {
                lbListItem = ((ListBox)pnlParameter.FindControl("lbStatus"));
                lbListItemSelected = ((ListBox)pnlParameter.FindControl("lbStatusSelected"));
            }

            int i = 0;
            while (i < lbListItemSelected.Items.Count)
            {
                if (lbListItemSelected.Items[i].Selected)
                {
                    string s_t = lbListItemSelected.Items[i].Text;
                    string s_v = lbListItemSelected.Items[i].Value;
                    lbListItem.Items.Add(new ListItem(s_t, s_v));
                    lbListItemSelected.Items.RemoveAt(i);
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
            if (crReportDocument.ParameterFields["@Zcode"] != null || crReportDocument.ParameterFields["Zcode"] != null)
                ViewState["ddlZcode"] = ((DropDownList)pnlParameter.FindControl("ddlZcode")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@SalesPersonType"] != null || crReportDocument.ParameterFields["SalesPersonType"] != null)
                ViewState["ddlSalesPersonType"] = ((DropDownList)pnlParameter.FindControl("ddlSalesPersonType")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@SalesType"] != null || crReportDocument.ParameterFields["SalesType"] != null)
                ViewState["ddlSalesType"] = ((DropDownList)pnlParameter.FindControl("ddlSalesType")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@Rental"] != null)
                ViewState["ddlRental"] = ((DropDownList)pnlParameter.FindControl("ddlRental")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@RentalType"] != null)
                ViewState["ddlRentalType"] = ((DropDownList)pnlParameter.FindControl("ddlRentalType")).SelectedValue.ToString();

            if (crReportDocument.ParameterFields["@SalesPersonID"] != null || crReportDocument.ParameterFields["SalesPersonID"] != null)
                ViewState["ddlSalesPersonID"] = ((DropDownList)pnlParameter.FindControl("ddlSalesPersonID")).SelectedValue.ToString();

            if (crReportDocument.ParameterFields["@Departement"] != null || crReportDocument.ParameterFields["Department"] != null)
                ViewState["ddlDepartment"] = ((DropDownList)pnlParameter.FindControl("ddlDepartment")).SelectedValue.ToString();

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
            if (crReportDocument.ParameterFields["Supplier"] != null )
                ViewState["txtSupplier"] = ((TextBox)pnlParameter.FindControl("txtSupplier")).Text.ToString();
            if (crReportDocument.ParameterFields["Brand Code"] != null)
                ViewState["txtBrandCode"] = ((TextBox)pnlParameter.FindControl("txtBrandCode")).Text.ToString();
            if (crReportDocument.ParameterFields["BrandCode"] != null)
                ViewState["txtBrandCode"] = ((TextBox)pnlParameter.FindControl("txtBrandCode")).Text.ToString();
            if (crReportDocument.ParameterFields["Brand"] != null || crReportDocument.ParameterFields["Product Group"] != null)
                ViewState["txtBrand"] = ((TextBox)pnlParameter.FindControl("txtBrand")).Text.ToString();
            if (crReportDocument.ParameterFields["Product Code"] != null)
                ViewState["txtProductCode"] = ((TextBox)pnlParameter.FindControl("txtProductCode")).Text.ToString();
            if (crReportDocument.ParameterFields["@ProductCodeHead"] != null)
                ViewState["txtProductCodeHead"] = ((TextBox)pnlParameter.FindControl("txtProductCodeHead")).Text.ToString();
            if (crReportDocument.ParameterFields["@Location"] != null)
                ViewState["txtLocation"] = ((TextBox)pnlParameter.FindControl("txtLocation")).Text.ToString();
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
            if (crReportDocument.ParameterFields["SalesPersonID"] != null)
                ViewState["txtSalesPersonID"] = ((TextBox)pnlParameter.FindControl("txtSalesPersonID")).Text.ToString();
            if (crReportDocument.ParameterFields["SalesPersonName"] != null || crReportDocument.ParameterFields["Sales Person Name"] != null || crReportDocument.ParameterFields["Salesperson"] != null)
                ViewState["txtSalesPersonName"] = ((TextBox)pnlParameter.FindControl("txtSalesPersonName")).Text.ToString();
            if (crReportDocument.ParameterFields["Industry"] != null) 
                ViewState["txtIndustry"] = ((TextBox)pnlParameter.FindControl("txtIndustry")).Text.ToString();
            if (crReportDocument.ParameterFields["@MRNo"] != null)
                ViewState["txtMRNo"] = ((TextBox)pnlParameter.FindControl("txtMRNo")).Text.ToString();

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

            if (crReportDocument.ParameterFields["@Scheme"] != null)
                ViewState["ddlScheme"] = ((DropDownList)pnlParameter.FindControl("ddlScheme")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["Scheme"] != null)
                ViewState["ddlScheme"] = ((DropDownList)pnlParameter.FindControl("ddlScheme")).SelectedValue.ToString();

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

            if (crReportDocument.ParameterFields["Status"] != null)
            {

                //ArrayList listProductSelected = new ArrayList();             
                ListBox lbStatusSelected = ((ListBox)pnlParameter.FindControl("lbStatusSelected"));

                if (lbStatusSelected.Items.Count > 0)
                {
                    string[] listStatusSelected = new string[lbStatusSelected.Items.Count];
                    int i = 0;
                    for (i = 0; i < lbStatusSelected.Items.Count; i++)
                    {
                        listStatusSelected[i] = lbStatusSelected.Items[i].Value;
                    }
                    ViewState.Add("lbStatusSelected()", listStatusSelected);
                }
                else
                {
                    ViewState.Add("lbStatusSelected()", null);
                }
            }

            if (crReportDocument.ParameterFields["@Display"] != null)
                ViewState["ddlDisplay"] = ((DropDownList)pnlParameter.FindControl("ddlDisplay")).SelectedValue;

            if (crReportDocument.ParameterFields["@MRScheme"] != null)
                ViewState["ddlMRScheme"] = ((DropDownList)pnlParameter.FindControl("ddlMRScheme")).SelectedValue;

            RefreshCrystalReport();
        }
        #endregion


        protected void Page_Unload(object sender, EventArgs e)
        {
            cyReportViewer.Dispose();
            cyReportViewer = null;


            crReportDocument.Close();
            crReportDocument.Dispose();


            GC.Collect();
        }

        private void RefreshCrystalReport()
        {
            LogSession session = base.GetSessionInfo();

            try
            {
                bool allow = true;
                if (crReportDocument.ParameterFields["Brand 1"] != null)
                {
                    string[] listProductSelected = (string[])ViewState["lbProdctsSelected()"];
                    if (listProductSelected != null)
                    {
                        allow = true;
                    }
                    else
                        allow = false;
                }

                if (crReportDocument.ParameterFields["Status"] != null)
                {
                    string[] listStatusSelected = (string[])ViewState["lbStatusSelected()"];
                    if (listStatusSelected != null)
                    {
                        allow = true;
                    }
                    else
                        allow = false;
                }

                if (allow)
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
                    /*

                    //JScriptAlertMsg(((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue.ToString());
                    */

                    //JScriptAlertMsg(ViewState["ddlYear"].ToString());

                    if (crReportDocument.ParameterFields["@Year"] != null && ViewState["ddlYear"] != null)
                        crReportDocument.SetParameterValue("@Year", GMSUtil.ToInt(ViewState["ddlYear"].ToString()));
                    if (crReportDocument.ParameterFields["Year"] != null && ViewState["ddlYear"] != null)
                        crReportDocument.SetParameterValue("Year", GMSUtil.ToInt(ViewState["ddlYear"].ToString()));
                    if (crReportDocument.ParameterFields["@Month"] != null && ViewState["ddlMonth"] != null)
                        crReportDocument.SetParameterValue("@Month", GMSUtil.ToInt(ViewState["ddlMonth"].ToString()));
                    if (crReportDocument.ParameterFields["Month"] != null && ViewState["ddlMonth"] != null)
                        crReportDocument.SetParameterValue("Month", GMSUtil.ToInt(ViewState["ddlMonth"].ToString()));
                    if (crReportDocument.ParameterFields["@Type"] != null && ViewState["ddlType"] != null)
                        crReportDocument.SetParameterValue("@Type", ViewState["ddlType"].ToString());
                    if (crReportDocument.ParameterFields["Type"] != null && ViewState["ddlType"] != null)
                        crReportDocument.SetParameterValue("Type", ViewState["ddlType"].ToString());
                    if (crReportDocument.ParameterFields["@Zcode"] != null && ViewState["ddlZcode"] != null)
                        crReportDocument.SetParameterValue("@Zcode", ViewState["ddlZcode"].ToString());

                    if (crReportDocument.ParameterFields["@SalesPersonType"] != null && ViewState["ddlSalesPersonType"] != null)
                        crReportDocument.SetParameterValue("@SalesPersonType", ViewState["ddlSalesPersonType"].ToString());

                    if (crReportDocument.ParameterFields["@SalesPersonID"] != null && ViewState["ddlSalesPersonID"] != null)
                        crReportDocument.SetParameterValue("@SalesPersonID", ViewState["ddlSalesPersonID"].ToString());

                    if (crReportDocument.ParameterFields["@Department"] != null && ViewState["ddlDepartment"] != null)
                        crReportDocument.SetParameterValue("@Department", ViewState["ddlDepartment"].ToString());

                    if (crReportDocument.ParameterFields["@SalesType"] != null && ViewState["ddlSalesType"] != null)
                        crReportDocument.SetParameterValue("@SalesType", ViewState["ddlSalesType"].ToString());
                    if (crReportDocument.ParameterFields["SalesPersonType"] != null && ViewState["ddlSalesPersonType"] != null)
                        crReportDocument.SetParameterValue("SalesPersonType", ViewState["ddlSalesPersonType"].ToString());

                    if (crReportDocument.ParameterFields["@Rental"] != null && ViewState["ddlRental"] != null)
                        crReportDocument.SetParameterValue("@Rental", ViewState["ddlRental"].ToString());
                    if (crReportDocument.ParameterFields["@RentalType"] != null && ViewState["ddlRentalType"] != null)
                        crReportDocument.SetParameterValue("@RentalType", ViewState["ddlRentalType"].ToString());

                    if (crReportDocument.ParameterFields["@SelectedCurrency"] != null && ViewState["ddlCurrency"] != null)
                        crReportDocument.SetParameterValue("@SelectedCurrency", GMSUtil.ToInt(ViewState["ddlCurrency"].ToString()));

                    if (crReportDocument.ParameterFields["@SelectedCurrency"] != null && session.DefaultCurrency == "SGD")
                        crReportDocument.SetParameterValue("@SelectedCurrency", GMSUtil.ToInt("1"));

                    if (crReportDocument.ParameterFields["Currency"] != null && ViewState["ddlCurrency"] != null)
                        crReportDocument.SetParameterValue("Currency", ViewState["ddlCurrency"].ToString());

                    if (crReportDocument.ParameterFields["Currency"] != null && session.DefaultCurrency == "SGD")
                        crReportDocument.SetParameterValue("Currency", "SGD");

                    if (crReportDocument.ParameterFields["@StartDate"] != null && ViewState["txtStartDate"] != null)
                        crReportDocument.SetParameterValue("@StartDate", GMSUtil.ToDate(ViewState["txtStartDate"]));

                    if (crReportDocument.ParameterFields["@Date"] != null && ViewState["txtDate"] != null)
                        crReportDocument.SetParameterValue("@Date", GMSUtil.ToDate(ViewState["txtDate"]));

                    if (crReportDocument.ParameterFields["@AsOfDate"] != null && ViewState["txtDate"] != null)
                        crReportDocument.SetParameterValue("@AsOfDate", GMSUtil.ToDate(ViewState["txtDate"]));

                    if (crReportDocument.ParameterFields["@DateFrom"] != null && ViewState["txtDateFrom"] != null)
                        crReportDocument.SetParameterValue("@DateFrom", GMSUtil.ToDate(ViewState["txtDateFrom"]));

                    if (crReportDocument.ParameterFields["DateFrom"] != null && ViewState["txtDateFrom"] != null)
                        crReportDocument.SetParameterValue("DateFrom", GMSUtil.ToDate(ViewState["txtDateFrom"]));

                    if (crReportDocument.ParameterFields["Date From"] != null && ViewState["txtDateFrom"] != null)
                        crReportDocument.SetParameterValue("Date From", GMSUtil.ToDate(ViewState["txtDateFrom"]));

                    if (crReportDocument.ParameterFields["@EndDate"] != null && ViewState["txtEndDate"] != null)
                        crReportDocument.SetParameterValue("@EndDate", GMSUtil.ToDate(ViewState["txtEndDate"]));

                    if (crReportDocument.ParameterFields["@DateTo"] != null && ViewState["txtDateTo"] != null)
                        crReportDocument.SetParameterValue("@DateTo", GMSUtil.ToDate(ViewState["txtDateTo"]));

                    if (crReportDocument.ParameterFields["DateTo"] != null && ViewState["txtDateTo"] != null)
                        crReportDocument.SetParameterValue("DateTo", GMSUtil.ToDate(ViewState["txtDateTo"]));

                    if (crReportDocument.ParameterFields["Date To"] != null && ViewState["txtDateTo"] != null)
                        crReportDocument.SetParameterValue("Date To", GMSUtil.ToDate(ViewState["txtDateTo"]));


                    if (crReportDocument.ParameterFields["Product Manager"] != null && ViewState["txtPM"] != null)
                        crReportDocument.SetParameterValue("Product Manager", ViewState["txtPM"].ToString());

                    if (crReportDocument.ParameterFields["Supplier"] != null && ViewState["txtSupplier"] != null)
                        crReportDocument.SetParameterValue("Supplier", ViewState["txtSupplier"].ToString());

                    if (crReportDocument.ParameterFields["Brand Code"] != null && ViewState["txtBrandCode"] != null)
                        crReportDocument.SetParameterValue("Brand Code", ViewState["txtBrandCode"].ToString());

                    if (crReportDocument.ParameterFields["BrandCode"] != null && ViewState["txtBrandCode"] != null)
                        crReportDocument.SetParameterValue("BrandCode", ViewState["txtBrandCode"].ToString());

                    if (crReportDocument.ParameterFields["Brand"] != null && ViewState["txtBrand"] != null)
                        crReportDocument.SetParameterValue("Brand", ViewState["txtBrand"].ToString());

                    if (crReportDocument.ParameterFields["Product Group"] != null && ViewState["txtBrand"] != null)
                        crReportDocument.SetParameterValue("Product Group", ViewState["txtBrand"].ToString());

                    if (crReportDocument.ParameterFields["Product Code"] != null && ViewState["txtProductCode"] != null)
                        crReportDocument.SetParameterValue("Product Code", ViewState["txtProductCode"].ToString());

                    if (crReportDocument.ParameterFields["@ProductCodeHead"] != null && ViewState["txtProductCodeHead"] != null)
                        crReportDocument.SetParameterValue("@ProductCodeHead", ViewState["txtProductCodeHead"].ToString());

                    if (crReportDocument.ParameterFields["@Location"] != null && ViewState["txtLocation"] != null)
                        crReportDocument.SetParameterValue("@Location", ViewState["txtLocation"].ToString());

                    if (crReportDocument.ParameterFields["Product Description"] != null && ViewState["txtProductDescription"] != null)
                        crReportDocument.SetParameterValue("Product Description", ViewState["txtProductDescription"].ToString());

                    if (crReportDocument.ParameterFields["@TWHCode"] != null && ViewState["txtTWHCode"] != null)
                        crReportDocument.SetParameterValue("@TWHCode", ViewState["txtTWHCode"].ToString());

                    if (crReportDocument.ParameterFields["@CoyID"] != null)
                        crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
                    if (crReportDocument.ParameterFields["CoyID"] != null)
                        crReportDocument.SetParameterValue("CoyID", session.CompanyId);
                    if (crReportDocument.ParameterFields["@UserNumID"] != null)
                        crReportDocument.SetParameterValue("@UserNumID", loginUserOrAlternateParty);
                    if (crReportDocument.ParameterFields["@ReportType"] != null)
                        crReportDocument.SetParameterValue("@ReportType", GetFinanceReportType());

                    if (crReportDocument.ParameterFields["Sort By"] != null && ViewState["ddlSortBy"] != null)
                        crReportDocument.SetParameterValue("Sort By", ViewState["ddlSortBy"].ToString());

                    if (crReportDocument.ParameterFields["Credit Limit"] != null && ViewState["ddlCreditLimit"] != null)
                        crReportDocument.SetParameterValue("Credit Limit", ViewState["ddlCreditLimit"].ToString());

                    if (crReportDocument.ParameterFields["@DayType"] != null && ViewState["ddlDayType"] != null)
                        crReportDocument.SetParameterValue("@DayType", ViewState["ddlDayType"]);


                    if (crReportDocument.ParameterFields["NarrationType"] != null && ViewState["ddlNarrationType"] != null)
                        crReportDocument.SetParameterValue("NarrationType", ViewState["ddlNarrationType"].ToString());

                    if (crReportDocument.ParameterFields["@NarrationType"] != null && ViewState["ddlNarrationType"] != null)
                        crReportDocument.SetParameterValue("@NarrationType", ViewState["ddlNarrationType"].ToString());

                    if (crReportDocument.ParameterFields["CreditSortBy"] != null && ViewState["ddlCreditSortBy"] != null)
                        crReportDocument.SetParameterValue("CreditSortBy", ViewState["ddlCreditSortBy"].ToString());

                    if (crReportDocument.ParameterFields["AccountType"] != null && ViewState["ddlAccountType"] != null)
                        crReportDocument.SetParameterValue("AccountType", ViewState["ddlAccountType"].ToString());

                    if (crReportDocument.ParameterFields["Country"] != null && ViewState["ddlCountry"] != null)
                        crReportDocument.SetParameterValue("Country", ViewState["ddlCountry"].ToString());

                    if (crReportDocument.ParameterFields["Territory"] != null && ViewState["ddlTerritory"] != null)
                        crReportDocument.SetParameterValue("Territory", ViewState["ddlTerritory"].ToString());

                    if (crReportDocument.ParameterFields["DebtorSortBy"] != null && ViewState["ddlDebtorSortBy"] != null)
                        crReportDocument.SetParameterValue("DebtorSortBy", ViewState["ddlDebtorSortBy"].ToString());

                    if (crReportDocument.ParameterFields["Order By"] != null && ViewState["ddlOrderBy"] != null)
                        crReportDocument.SetParameterValue("Order By", ViewState["ddlOrderBy"].ToString());

                    if (crReportDocument.ParameterFields["Customer Account Name"] != null && ViewState["txtAccountName"] != null)
                        crReportDocument.SetParameterValue("Customer Account Name", ViewState["txtAccountName"].ToString());

                    if (crReportDocument.ParameterFields["Customer Name"] != null && ViewState["txtAccountName"] != null)
                        crReportDocument.SetParameterValue("Customer Name", ViewState["txtAccountName"].ToString());

                    if (crReportDocument.ParameterFields["Customer Account Code"] != null && ViewState["txtAccountCode"] != null)
                        crReportDocument.SetParameterValue("Customer Account Code", ViewState["txtAccountCode"].ToString());

                    if (crReportDocument.ParameterFields["@AccountCode"] != null && ViewState["txtAccountCode"] != null)
                        crReportDocument.SetParameterValue("@AccountCode", ViewState["txtAccountCode"].ToString());

                    if (crReportDocument.ParameterFields["SalesPersonID"] != null && ViewState["txtSalesPersonID"] != null)
                        crReportDocument.SetParameterValue("SalesPersonID", ViewState["txtSalesPersonID"].ToString());

                    if (crReportDocument.ParameterFields["SalesPersonName"] != null && ViewState["txtSalesPersonName"] != null)
                        crReportDocument.SetParameterValue("SalesPersonName", ViewState["txtSalesPersonName"].ToString());

                    if (crReportDocument.ParameterFields["Sales Person Name"] != null && ViewState["txtSalesPersonName"] != null)
                        crReportDocument.SetParameterValue("Sales Person Name", ViewState["txtSalesPersonName"].ToString());

                    if (crReportDocument.ParameterFields["Salesperson"] != null && ViewState["txtSalesPersonName"] != null)
                        crReportDocument.SetParameterValue("Salesperson", ViewState["txtSalesPersonName"].ToString());

                    if (crReportDocument.ParameterFields["Industry"] != null && ViewState["txtIndustry"] != null)
                        crReportDocument.SetParameterValue("Industry", ViewState["txtIndustry"].ToString());

                    if (crReportDocument.ParameterFields["Employee Name"] != null && ViewState["txtEmployeeName"] != null)
                        crReportDocument.SetParameterValue("Employee Name", ViewState["txtEmployeeName"].ToString());

                    if (crReportDocument.ParameterFields["Course Title"] != null && ViewState["txtCourseTitle"] != null)
                        crReportDocument.SetParameterValue("Course Title", ViewState["txtCourseTitle"].ToString());

                    if (crReportDocument.ParameterFields["Invoice No"] != null && ViewState["txtInvoiceNo"] != null)
                        crReportDocument.SetParameterValue("Invoice No", ViewState["txtInvoiceNo"].ToString());

                    if (crReportDocument.ParameterFields["Group"] != null && ViewState["txtGroupName"] != null)
                        crReportDocument.SetParameterValue("Group", ViewState["txtGroupName"].ToString());

                    if (crReportDocument.ParameterFields["Company"] != null)
                        crReportDocument.SetParameterValue("Company", ViewState["ddlCompany"].ToString());

                    if (crReportDocument.ParameterFields["@COARangeStart"] != null && ViewState["txtCOARangeStart"] != null)
                        crReportDocument.SetParameterValue("@COARangeStart", ViewState["txtCOARangeStart"].ToString());
                    if (crReportDocument.ParameterFields["@COARangeEnd"] != null && ViewState["txtCOARangeEnd"] != null)
                        crReportDocument.SetParameterValue("@COARangeEnd", ViewState["txtCOARangeEnd"].ToString());

                    if (crReportDocument.ParameterFields["@CompanyCode"] != null)
                        crReportDocument.SetParameterValue("@CompanyCode", ViewState["ddlCompanyCode"].ToString());

                    if (crReportDocument.ParameterFields["Brand 1"] != null)
                    {
                        string[] listProductSelected = (string[])ViewState["lbProdctsSelected()"];
                        if (listProductSelected != null)
                            crReportDocument.SetParameterValue("Brand 1", listProductSelected);
                        else
                        {
                            string[] listProductSelectedEmpty = new string[1];
                            listProductSelectedEmpty[0] = "";
                            crReportDocument.SetParameterValue("Brand 1", listProductSelectedEmpty);
                        }
                    }

                    if (crReportDocument.ParameterFields["Status"] != null)
                    {
                        string[] listStatusSelected = (string[])ViewState["lbStatusSelected()"];
                        if (listStatusSelected != null)
                            crReportDocument.SetParameterValue("Status", listStatusSelected);
                        else
                        {
                            string[] listStatusSelectedEmpty = new string[1];
                            listStatusSelectedEmpty[0] = "";
                            crReportDocument.SetParameterValue("Status", listStatusSelectedEmpty);
                        }
                    }


                    if (crReportDocument.ParameterFields["@Scheme"] != null && ViewState["ddlScheme"] != null)
                        crReportDocument.SetParameterValue("@Scheme", ViewState["ddlScheme"].ToString());
                    if (crReportDocument.ParameterFields["Scheme"] != null && ViewState["ddlScheme"] != null)
                        crReportDocument.SetParameterValue("Scheme", ViewState["ddlScheme"].ToString());

                    if (crReportDocument.ParameterFields["@MRNo"] != null && ViewState["txtMRNo"] != null)
                        crReportDocument.SetParameterValue("@MRNo", ViewState["txtMRNo"].ToString());

                    if (crReportDocument.ParameterFields["@UserID"] != null)
                        crReportDocument.SetParameterValue("@UserID", session.UserId);

                    if (crReportDocument.ParameterFields["@Display"] != null)
                        crReportDocument.SetParameterValue("@Display", ViewState["ddlDisplay"].ToString());

                    if (crReportDocument.ParameterFields["@MRScheme"] != null && ViewState["ddlMRScheme"] != null)
                        crReportDocument.SetParameterValue("@MRScheme", ViewState["ddlMRScheme"].ToString());

                    cyReportViewer.ReportSource = crReportDocument;

                }

            }
            catch (Exception ex)
            {
                this.lblFeedback.Text = ex.Message;
            }


        }

        private string GetFinanceReportType()
        {
            string ReportType = string.Empty;

            if (reportId == 441 || reportId == 448 || reportId == 452 || reportId == 456 || reportId == 460)
            {
                return "PNL";
            }
            else if (reportId == 445 || reportId == 449 || reportId == 453 || reportId == 457 || reportId == 461)
            {
                return "PNLD";
            }
            else if (reportId == 446 || reportId == 450 || reportId == 454 || reportId == 458 || reportId == 462)
            {
                return "BS";
            }
            else //447, 451, 455, 459, 463
            {
                return "BSD";
            }

        }

        public string getIsOptimizedTable
        {
            get { return isOptimizedTable; }
        }

        public string getIsLargeFont
        {
            get { return isLargeFont; }
        }
    }
}