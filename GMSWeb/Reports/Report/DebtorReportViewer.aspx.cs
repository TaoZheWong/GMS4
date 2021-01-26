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
using GMSCore;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GMSCore.Activity;
using GMSCore.Entity;

namespace GMSWeb.Reports.Report
{
    public partial class DebtorReportViewer : GMSBasePage
    {
        protected ReportDocument crReportDocument;
        private short reportId = 0;
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

            //UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
            //                                                                49);
            //if (uAccess == null)
            //    Response.Redirect("../../Unauthorized.htm");




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
<script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>
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
                pnlParameter.Controls.Add(new LiteralControl("<img id=\"img5\" src=\"../../images/imgCalendar.gif\" onclick=\"showCalendar(this, document.getElementById('txtDate'), 'dd/mm/yyyy', null, 1);\" height=\"20\" width=\"17\"  align=\"absMiddle\"  border=\"0\"></td>"));
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
                ddlSortBy.Items.Add(new ListItem("Credit Limit", "Credit Limit"));
                ddlSortBy.Items.Add(new ListItem("Customer Name", "Customer Name"));
                
                pnlParameter.Controls.Add(ddlSortBy);
                if (ViewState["ddlSortBy"] == null)
                    ViewState["ddlSortBy"] = "Date";
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
        

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();           
           
            if (crReportDocument.ParameterFields["@Date"] != null || crReportDocument.ParameterFields["@AsOfDate"] != null)
                ViewState["txtDate"] = ((TextBox)pnlParameter.FindControl("txtDate")).Text.ToString();           
            if (crReportDocument.ParameterFields["Sort By"] != null)
                ViewState["ddlSortBy"] = ((DropDownList)pnlParameter.FindControl("ddlSortBy")).SelectedValue.ToString();
           
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
                                     

                    if (crReportDocument.ParameterFields["@Date"] != null && ViewState["txtDate"] != null)
                        crReportDocument.SetParameterValue("@Date", GMSUtil.ToDate(ViewState["txtDate"]));

                    if (crReportDocument.ParameterFields["@AsOfDate"] != null && ViewState["txtDate"] != null)
                        crReportDocument.SetParameterValue("@AsOfDate", GMSUtil.ToDate(ViewState["txtDate"]));
                    
                    if (crReportDocument.ParameterFields["Sort By"] != null && ViewState["ddlSortBy"] != null)
                        crReportDocument.SetParameterValue("Sort By", ViewState["ddlSortBy"].ToString());                     

                    if (crReportDocument.ParameterFields["@CoyID"] != null)
                        crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
                    if (crReportDocument.ParameterFields["CoyID"] != null)
                        crReportDocument.SetParameterValue("CoyID", session.CompanyId);
                    if (crReportDocument.ParameterFields["@UserNumID"] != null)
                        crReportDocument.SetParameterValue("@UserNumID", loginUserOrAlternateParty);


                    cyReportViewer.ReportSource = crReportDocument;

                
            }
            catch (Exception ex)
            {
                this.lblFeedback.Text = ex.Message;
            }


        }
    }
}
