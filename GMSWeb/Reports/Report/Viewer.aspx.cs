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

public partial class Reports_Report_Viewer : GMSBasePage
{
    protected ReportDocument crReportDocument = null;
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
            //JScriptAlertMsg("!IsPostBack");
        }

        if (IsPostBack)
        {
            RefreshCrystalReport();
        }

        string javaScript =
@"
<script language=""javascript"" type=""text/javascript"" src=""/GMS/scripts/popcalendar.js""></script>
";
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

        int controlCount = 0;

        pnlParameter.Controls.Add(new LiteralControl("<table class=\"tTable1\" style=\"margin-left: 8px; border: 1px; width:702px\" cellspacing=\"5\" cellpadding=\"5\" border=\"0\">"));

        if (fileDescription == "F")
        {
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
                pnlParameter.Controls.Add(new LiteralControl("<img id=\"img4\" src=\"../../images/imgCalendar.gif\" onclick=\"showCalendar(this, document.getElementById('txtDate'), 'dd/mm/yyyy', null, 1);\" height=\"20\" width=\"17\"  align=\"absMiddle\"  border=\"0\"></td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }


            if (crReportDocument.ParameterFields["DateFrom"] != null || crReportDocument.ParameterFields["@DateFrom"] != null)
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

            if (crReportDocument.ParameterFields["DateTo"] != null || crReportDocument.ParameterFields["@@DateTo"] != null)
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

                if ((controlCount % 3) == 0)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                }
                // add new control
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">Year :"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">"));
                DropDownList ddlYear = new DropDownList();
                ddlYear.ID = "ddlYear";
                ddlYear.CssClass = "dropdownlist";
                //ddlYear.AutoPostBack = true;                
                //ddlYear.SelectedIndexChanged += DropDownListYear_SelectedIndexChanged; 
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

                if ((controlCount % 3) == 2)
                {
                    pnlParameter.Controls.Add(new LiteralControl("</tr>"));

                }
                controlCount = controlCount + 1;


                // end of creating new control



            }
            if (crReportDocument.ParameterFields["@Month"] != null || crReportDocument.ParameterFields["Month"] != null)
            {
                if ((controlCount % 3) == 0)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                }


                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">Month :"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">"));
                DropDownList ddlMonth = new DropDownList();
                ddlMonth.ID = "ddlMonth";
                ddlMonth.CssClass = "dropdownlist";
                //ddlMonth.AutoPostBack = true;
                //ddlMonth.SelectedIndexChanged += DropDownListMonth_SelectedIndexChanged;
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



                if ((controlCount % 3) == 2)
                {
                    pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                }

                controlCount = controlCount + 1;



            }
            if (crReportDocument.ParameterFields["@Week"] != null)
            {
                if ((controlCount % 3) == 0)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                }


                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">Week :"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">"));
                DropDownList ddlWeek = new DropDownList();
                ddlWeek.ID = "ddlWeek";
                ddlWeek.CssClass = "dropdownlist";
                ddlWeek.Items.Clear();
                for (int i = 1; i <= 52; i++)
                {
                    ddlWeek.Items.Add(i.ToString());
                }

                ddlWeek.SelectedValue = "1".ToString();


                pnlParameter.Controls.Add(ddlWeek);
                if (ViewState["ddlWeek"] == null)
                    ViewState["ddlWeek"] = "1".ToString();
                pnlParameter.Controls.Add(new LiteralControl("</td>"));



                if ((controlCount % 3) == 2)
                {
                    pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                }

                controlCount = controlCount + 1;



            }
            if ((crReportDocument.ParameterFields["@SelectedCurrency"] != null) && session.DefaultCurrency != "SGD")
            {
                if ((controlCount % 3) == 0)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                }


                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">Currency :"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">"));
                DropDownList ddlCurrency = new DropDownList();
                ddlCurrency.ID = "ddlCurrency";
                ddlCurrency.CssClass = "dropdownlist";
                //ddlCurrency.AutoPostBack = true;
                //ddlCurrency.SelectedIndexChanged += DropDownListCurrency_SelectedIndexChanged;
                ddlCurrency.Items.Clear();

                ddlCurrency.Items.Add(new ListItem("DEFAULT", "1"));
                ddlCurrency.Items.Add(new ListItem("SGD", "2"));
                ddlCurrency.Items.Add(new ListItem("USD", "3"));
                pnlParameter.Controls.Add(ddlCurrency);
                if (ViewState["ddlCurrency"] == null)
                    ViewState["ddlCurrency"] = "1";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));


                if ((controlCount % 3) == 2)
                {
                    pnlParameter.Controls.Add(new LiteralControl("</tr>"));

                }
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
                ddlCurrency.Items.Add(new ListItem("USD", "USD"));
                pnlParameter.Controls.Add(ddlCurrency);
                if (ViewState["ddlCurrency"] == null)
                    ViewState["ddlCurrency"] = "DEFAULT";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                controlCount = controlCount + 1;
            }

            if (crReportDocument.ParameterFields["@ProjectID"] != null)
            {
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DataSet dsProjects = new DataSet();
                dacl.GetCompanyProject(session.CompanyId, loginUserOrAlternateParty, reportId, ref dsProjects);

                if (dsProjects.Tables[0].Rows.Count > 0)
                {
                    if ((controlCount % 3) == 0)
                    {
                        pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                    }

                    pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">Project ID :"));
                    pnlParameter.Controls.Add(new LiteralControl("</td>"));
                    pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">"));
                    DropDownList ddlProjectID = new DropDownList();
                    ddlProjectID.ID = "ddlProjectID";


                    ddlProjectID.Items.Clear();

                    for (int j = 0; j < dsProjects.Tables[0].Rows.Count; j++)
                    {

                        ddlProjectID.Items.Add(new ListItem(dsProjects.Tables[0].Rows[j]["ProjectName"].ToString(), dsProjects.Tables[0].Rows[j]["ReportProjectID"].ToString()));
                    }


                    pnlParameter.Controls.Add(ddlProjectID);


                    pnlParameter.Controls.Add(new LiteralControl("</td>"));

                    if ((controlCount % 3) == 2)
                    {
                        pnlParameter.Controls.Add(new LiteralControl("</tr>"));

                    }
                    controlCount = controlCount + 1;

                }
                if (ViewState["ddlProjectID"] == null)
                    ViewState["ddlProjectID"] = -1;

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


            if (crReportDocument.ParameterFields["@Type"] != null)
            {


                if ((controlCount % 3) == 0)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                }

                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">Customer Type :"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabelInline\">"));
                DropDownList ddlType = new DropDownList();
                ddlType.ID = "ddlType";

                //ddlType.AutoPostBack = true;
                //ddlType.SelectedIndexChanged += DropDownListType_SelectedIndexChanged;
                ddlType.Items.Clear();
                ddlType.Items.Add(new ListItem("ALL", "All"));
                ddlType.Items.Add(new ListItem("DIRECT CUSTOMERS", "External"));
                ddlType.Items.Add(new ListItem("INTERCO CUSTOMERS", "Internal"));


                pnlParameter.Controls.Add(ddlType);
                if (ViewState["ddlType"] == null)
                    ViewState["ddlType"] = "All";
                pnlParameter.Controls.Add(new LiteralControl("</td>"));



                if ((controlCount % 3) == 2)
                {
                    pnlParameter.Controls.Add(new LiteralControl("</tr>"));

                }

                controlCount = controlCount + 1;

            }
        }
        else
        {
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
                ddlCurrency.Items.Add(new ListItem("USD", "3"));
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
                ddlCurrency.Items.Add(new ListItem("USD", "USD"));
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

            if (crReportDocument.ParameterFields["Overdue Days"] != null)
            {
                pnlParameter.Controls.Add(new LiteralControl("<tr>"));
                pnlParameter.Controls.Add(new LiteralControl("<td class=\"tbLabel\">Overdue Days"));
                pnlParameter.Controls.Add(new LiteralControl("</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>:</td>"));
                pnlParameter.Controls.Add(new LiteralControl("<td>"));
                DropDownList ddlOverdueDays = new DropDownList();
                ddlOverdueDays.ID = "ddlOverdueDays";
                ddlOverdueDays.CssClass = "dropdownlist";
                ddlOverdueDays.Items.Clear();
                ddlOverdueDays.Items.Add(new ListItem("ALL", "A"));
                ddlOverdueDays.Items.Add(new ListItem("> 90 Days", "B"));
                ddlOverdueDays.Items.Add(new ListItem("> 120 Days", "C"));
                ddlOverdueDays.Items.Add(new ListItem("> 180 Days", "D"));
                ddlOverdueDays.Items.Add(new ListItem("> 365 Days", "E"));


                pnlParameter.Controls.Add(ddlOverdueDays);
                if (ViewState["ddlOverdueDays"] == null)
                    ViewState["ddlOverdueDays"] = "A";
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
        }



        Button dynamicbutton = new Button();
        dynamicbutton.Click += new System.EventHandler(btnSubmit_Click);
        dynamicbutton.Text = "Submit";
        dynamicbutton.CssClass = "button";
        dynamicbutton.ID = "btnSubmit";

        pnlParameter.Controls.Add(new LiteralControl("<tr>"));
        pnlParameter.Controls.Add(new LiteralControl("<td></td><td></td><td></td><td></td><td></td>"));
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        LogSession session = base.GetSessionInfo();

        if (fileDescription == "F")
        {

            if (crReportDocument.ParameterFields["@Year"] != null)
                ViewState["ddlYear"] = ((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["Year"] != null)
                ViewState["ddlYear"] = ((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@Month"] != null)
                ViewState["ddlMonth"] = ((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["DateFrom"] != null)
                ViewState["txtDateFrom"] = ((TextBox)pnlParameter.FindControl("txtDateFrom")).Text.ToString();
            if (crReportDocument.ParameterFields["DateTo"] != null)
                ViewState["txtDateTo"] = ((TextBox)pnlParameter.FindControl("txtDateTo")).Text.ToString();
            if (crReportDocument.ParameterFields["@Week"] != null)
                ViewState["ddlWeek"] = ((DropDownList)pnlParameter.FindControl("ddlWeek")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["Month"] != null)
                ViewState["ddlMonth"] = ((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@ProjectID"] != null && (DropDownList)pnlParameter.FindControl("ddlProjectID") != null)
                ViewState["ddlProjectID"] = ((DropDownList)pnlParameter.FindControl("ddlProjectID")).SelectedValue.ToString();
            else
                ViewState["ddlProjectID"] = -1;
            if (crReportDocument.ParameterFields["@Type"] != null)
                ViewState["ddlType"] = ((DropDownList)pnlParameter.FindControl("ddlType")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@SelectedCurrency"] != null)
                ViewState["ddlCurrency"] = ((DropDownList)pnlParameter.FindControl("ddlCurrency")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["Currency"] != null && session.DefaultCurrency != "SGD")
                ViewState["ddlCurrency"] = ((DropDownList)pnlParameter.FindControl("ddlCurrency")).SelectedValue.ToString();

            if (crReportDocument.ParameterFields["@Date"] != null || crReportDocument.ParameterFields["@AsOfDate"] != null)
                ViewState["txtDate"] = ((TextBox)pnlParameter.FindControl("txtDate")).Text.ToString();

            if (crReportDocument.ParameterFields["Customer Account Name"] != null || crReportDocument.ParameterFields["Customer Name"] != null)
                ViewState["txtAccountName"] = ((TextBox)pnlParameter.FindControl("txtAccountName")).Text.ToString();
            if (crReportDocument.ParameterFields["Customer Account Code"] != null || crReportDocument.ParameterFields["@AccountCode"] != null)
                ViewState["txtAccountCode"] = ((TextBox)pnlParameter.FindControl("txtAccountCode")).Text.ToString();
            if (crReportDocument.ParameterFields["SalesPersonName"] != null || crReportDocument.ParameterFields["Sales Person Name"] != null || crReportDocument.ParameterFields["Salesperson"] != null)
                ViewState["txtSalesPersonName"] = ((TextBox)pnlParameter.FindControl("txtSalesPersonName")).Text.ToString();
        }
        else
        {
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

            if (crReportDocument.ParameterFields["Overdue Days"] != null)
                ViewState["ddlOverdueDays"] = ((DropDownList)pnlParameter.FindControl("ddlOverdueDays")).SelectedValue.ToString();

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
        }


        RefreshCrystalReport();
    }

    private void RefreshCrystalReport()
    {
        LogSession session = base.GetSessionInfo();

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
            /*

            //JScriptAlertMsg(((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue.ToString());
            */

            //JScriptAlertMsg(ViewState["ddlYear"].ToString());
            if (fileDescription == "F")
            {
                if (crReportDocument.ParameterFields["@Year"] != null && ViewState["ddlYear"] != null)
                    crReportDocument.SetParameterValue("@Year", GMSUtil.ToInt(ViewState["ddlYear"].ToString()));
                if (crReportDocument.ParameterFields["Year"] != null && ViewState["ddlYear"] != null)
                    crReportDocument.SetParameterValue("Year", GMSUtil.ToInt(ViewState["ddlYear"].ToString()));
                if (crReportDocument.ParameterFields["@Month"] != null && ViewState["ddlMonth"] != null)
                    crReportDocument.SetParameterValue("@Month", GMSUtil.ToInt(ViewState["ddlMonth"].ToString()));
                if (crReportDocument.ParameterFields["@Week"] != null && ViewState["ddlWeek"] != null)
                    crReportDocument.SetParameterValue("@Week", GMSUtil.ToInt(ViewState["ddlWeek"].ToString()));
                if (crReportDocument.ParameterFields["Month"] != null && ViewState["ddlMonth"] != null)
                    crReportDocument.SetParameterValue("Month", GMSUtil.ToInt(ViewState["ddlMonth"].ToString()));
                if (crReportDocument.ParameterFields["@ProjectID"] != null && ViewState["ddlProjectID"] != null)
                    crReportDocument.SetParameterValue("@ProjectID", GMSUtil.ToInt(ViewState["ddlProjectID"].ToString()));
                if (crReportDocument.ParameterFields["@Type"] != null && ViewState["ddlType"] != null)
                    crReportDocument.SetParameterValue("@Type", GMSUtil.ToInt(ViewState["ddlType"].ToString()));

                if (crReportDocument.ParameterFields["@SelectedCurrency"] != null && ViewState["ddlCurrency"] != null)
                    crReportDocument.SetParameterValue("@SelectedCurrency", GMSUtil.ToInt(ViewState["ddlCurrency"].ToString()));

                if (crReportDocument.ParameterFields["@SelectedCurrency"] != null && session.DefaultCurrency == "SGD")
                    crReportDocument.SetParameterValue("@SelectedCurrency", GMSUtil.ToInt("1"));


                if (crReportDocument.ParameterFields["Currency"] != null && ViewState["ddlCurrency"] != null)
                    crReportDocument.SetParameterValue("Currency", ViewState["ddlCurrency"].ToString());

                if (crReportDocument.ParameterFields["Currency"] != null && session.DefaultCurrency == "SGD")
                    crReportDocument.SetParameterValue("Currency", "SGD");

                if (crReportDocument.ParameterFields["DateFrom"] != null && ViewState["txtDateFrom"] != null)
                    crReportDocument.SetParameterValue("DateFrom", GMSUtil.ToDate(ViewState["txtDateFrom"]));

                if (crReportDocument.ParameterFields["DateTo"] != null && ViewState["txtDateTo"] != null)
                    crReportDocument.SetParameterValue("DateTo", GMSUtil.ToDate(ViewState["txtDateTo"]));





                if (crReportDocument.ParameterFields["@Date"] != null && ViewState["txtDate"] != null)
                    crReportDocument.SetParameterValue("@Date", GMSUtil.ToDate(ViewState["txtDate"]));

                if (crReportDocument.ParameterFields["@AsOfDate"] != null && ViewState["txtDate"] != null)
                    crReportDocument.SetParameterValue("@AsOfDate", GMSUtil.ToDate(ViewState["txtDate"]));

                if (crReportDocument.ParameterFields["Customer Account Name"] != null && ViewState["txtAccountName"] != null)
                    crReportDocument.SetParameterValue("Customer Account Name", ViewState["txtAccountName"].ToString());

                if (crReportDocument.ParameterFields["Customer Name"] != null && ViewState["txtAccountName"] != null)
                    crReportDocument.SetParameterValue("Customer Name", ViewState["txtAccountName"].ToString());

                if (crReportDocument.ParameterFields["Customer Account Code"] != null && ViewState["txtAccountCode"] != null)
                    crReportDocument.SetParameterValue("Customer Account Code", ViewState["txtAccountCode"].ToString());

                if (crReportDocument.ParameterFields["@AccountCode"] != null && ViewState["txtAccountCode"] != null)
                    crReportDocument.SetParameterValue("@AccountCode", ViewState["txtAccountCode"].ToString());

                if (crReportDocument.ParameterFields["SalesPersonName"] != null && ViewState["txtSalesPersonName"] != null)
                    crReportDocument.SetParameterValue("SalesPersonName", ViewState["txtSalesPersonName"].ToString());

                if (crReportDocument.ParameterFields["Sales Person Name"] != null && ViewState["txtSalesPersonName"] != null)
                    crReportDocument.SetParameterValue("Sales Person Name", ViewState["txtSalesPersonName"].ToString());

                if (crReportDocument.ParameterFields["Salesperson"] != null && ViewState["txtSalesPersonName"] != null)
                    crReportDocument.SetParameterValue("Salesperson", ViewState["txtSalesPersonName"].ToString());

                if (crReportDocument.ParameterFields["@CoyID"] != null)
                    crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
                if (crReportDocument.ParameterFields["CoyID"] != null)
                    crReportDocument.SetParameterValue("CoyID", session.CompanyId);
                if (crReportDocument.ParameterFields["@UserNumID"] != null)
                    crReportDocument.SetParameterValue("@UserNumID", loginUserOrAlternateParty);
            }
            else
            {
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

                if (crReportDocument.ParameterFields["Sort By"] != null && ViewState["ddlSortBy"] != null)
                    crReportDocument.SetParameterValue("Sort By", ViewState["ddlSortBy"].ToString());

                if (crReportDocument.ParameterFields["Credit Limit"] != null && ViewState["ddlCreditLimit"] != null)
                    crReportDocument.SetParameterValue("Credit Limit", ViewState["ddlCreditLimit"].ToString());

                if (crReportDocument.ParameterFields["Overdue Days"] != null && ViewState["ddlOverdueDays"] != null)
                    crReportDocument.SetParameterValue("Overdue Days", ViewState["ddlOverdueDays"].ToString());


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


                if (crReportDocument.ParameterFields["Company"] != null)
                    crReportDocument.SetParameterValue("Company", ViewState["ddlCompany"].ToString());


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
            }

            cyReportViewer.ReportSource = crReportDocument;


        }
        catch (Exception ex)
        {
            this.lblFeedback.Text = ex.Message;
        }


    }
}
