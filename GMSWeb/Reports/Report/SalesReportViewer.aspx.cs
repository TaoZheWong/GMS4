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
        bool IsCOA2016 = false;
        string isLargeFont, isOptimizedTable;
        private string currentLink = "";

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

            this.currentLink = Request.QueryString["CurrentLink"];

            DataSet lstAlterParty = new DataSet();

            if (currentLink == "Products")
                new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Product Report", ref lstAlterParty);
            else
                new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Sales Detail", ref lstAlterParty);
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
                    IsCOA2016 = new ReportsActivity().RetrieveReportById(this.reportId).IsCOA2016;

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
            } else
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

            string javaScript = @"<script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>";
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
            new GMSGeneralDALC().GetSalesExecForReport(session.CompanyId, loginUserOrAlternateParty, year, month, dept, ref dsSalesPerson);
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
            new GMSGeneralDALC().GetSalesExecForReport(session.CompanyId, loginUserOrAlternateParty, year, month, dept, ref dsSalesPerson);
            if (dsSalesPerson.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < dsSalesPerson.Tables[0].Rows.Count; j++)
                {
                    ddlSalesPersonID.Items.Add(new ListItem(dsSalesPerson.Tables[0].Rows[j]["SalesPersonID"].ToString() + " - " + dsSalesPerson.Tables[0].Rows[j]["SalesPersonName"].ToString(), dsSalesPerson.Tables[0].Rows[j]["SalesPersonID"].ToString()));
                }
            }
        }

        private void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            short selectedvalue = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlGroup")).SelectedValue);
            DropDownList ddlTeam = pnlParameter.FindControl("ddlTeam") as DropDownList;
            ddlTeam.Items.Clear();

            if (selectedvalue > 0)
            {
                ddlTeam.Enabled = true;
                LogSession session = base.GetSessionInfo();
                SystemDataActivity sDA = new SystemDataActivity();
                IList<GMSCore.Entity.SalesGroupTeam> lstEE = null;
                lstEE = sDA.RetrieveTeamSetupSalesTeamByGroupID(session.CompanyId, selectedvalue);
                ddlTeam.Items.Add(new ListItem("N/A", "0"));
                foreach (var x in lstEE)
                {
                    ddlTeam.Items.Add(new ListItem(x.TeamName, x.TeamID.ToString()));
                }
            }
            else
            {
                ddlTeam.Enabled = false;
            }

            cyReportViewer.Visible = false;
        }

        #region SelectedIndexChanged
        private void ddlProjectID_SelectedIndexChanged(object sender, EventArgs e)
        {
            short selectedvalue = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlProjectID")).SelectedValue);
            DropDownList ddlDepartmentID = pnlParameter.FindControl("ddlDepartmentID") as DropDownList;
            ddlDepartmentID.Items.Clear();
            DropDownList ddlSectionID = pnlParameter.FindControl("ddlSectionID") as DropDownList;
            ddlSectionID.Items.Clear();
            DropDownList ddlUnitID = pnlParameter.FindControl("ddlUnitID") as DropDownList;
            ddlUnitID.Items.Clear();

            if (selectedvalue > 0)
            {
                ddlDepartmentID.Enabled = true;
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DataSet dsDepartments = new DataSet();

                short year = Convert.ToInt16(DateTime.Now.Year);
                try
                {
                    year = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue);
                } catch (Exception ex)
                {
                    try
                    {
                        TextBox txtDate = pnlParameter.FindControl("txtDateTo") as TextBox;
                        DateTime date = DateTime.Parse(txtDate.Text.ToString());
                        year = Convert.ToInt16(date.Year.ToString());
                    } catch (Exception ex2) { }
                }

                short month = Convert.ToInt16(DateTime.Now.Month);
                try
                {
                    month = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue);
                }
                catch (Exception ex)
                {
                    try
                    {
                        TextBox txtDate = pnlParameter.FindControl("txtDateTo") as TextBox;
                        DateTime date = DateTime.Parse(txtDate.Text.ToString());
                        month = Convert.ToInt16(date.Month.ToString());
                    }
                    catch (Exception ex2) { }
                }

                dacl.GetCompanyDepartment(session.CompanyId, selectedvalue, 0, loginUserOrAlternateParty, year, month, ref dsDepartments);
                foreach (DataRow dr in dsDepartments.Tables[0].Rows)
                {
                    ddlDepartmentID.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
                }
            }
            else
            {
                ddlDepartmentID.Enabled = false;
            }

            ddlSectionID.Enabled = false;
            ddlUnitID.Enabled = false;
            cyReportViewer.Visible = false;
        }
        private void ddlDepartmentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            short selectedvalue = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlDepartmentID")).SelectedValue);
            DropDownList ddlSectionID = pnlParameter.FindControl("ddlSectionID") as DropDownList;
            ddlSectionID.Items.Clear();
            DropDownList ddlUnitID = pnlParameter.FindControl("ddlUnitID") as DropDownList;
            ddlUnitID.Items.Clear();

            if (selectedvalue > 0)
            {
                ddlSectionID.Enabled = true;
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DataSet dsSections = new DataSet();

                short year = Convert.ToInt16(DateTime.Now.Year);
                try
                {
                    year = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue);
                }
                catch (Exception ex)
                {
                    try
                    {
                        TextBox txtDate = pnlParameter.FindControl("txtDateTo") as TextBox;
                        DateTime date = DateTime.Parse(txtDate.Text.ToString());
                        year = Convert.ToInt16(date.Year.ToString());
                    }
                    catch (Exception ex2) { }
                }

                short month = 0;
                short reportId = 0;
                short loginUserOrAlternateParty = 0;

                try
                {
                    month = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue);
                }
                catch (Exception ex)
                {
                    try
                    {
                        TextBox txtDate = pnlParameter.FindControl("txtDateTo") as TextBox;
                        DateTime date = DateTime.Parse(txtDate.Text.ToString());
                        month = Convert.ToInt16(date.Month.ToString());
                    }
                    catch (Exception ex2)
                    {
                        month = short.Parse(DateTime.Now.Month.ToString());
                    }
                }

                dacl.GetCompanySection(session.CompanyId, selectedvalue, reportId, loginUserOrAlternateParty, year, month, ref dsSections);
                foreach (DataRow dr in dsSections.Tables[0].Rows)
                {
                    ddlSectionID.Items.Add(new ListItem(dr["SectionName"].ToString(), dr["SectionID"].ToString()));
                }
            }
            else
            {
                ddlSectionID.Enabled = false;
            }

            ddlUnitID.Enabled = false;
            cyReportViewer.Visible = false;
        }

        private void ddlSectionID_SelectedIndexChanged(object sender, EventArgs e)
        {
            short selectedvalue = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlSectionID")).SelectedValue);
            DropDownList ddlUnitID = pnlParameter.FindControl("ddlUnitID") as DropDownList;
            ddlUnitID.Items.Clear();
            if (selectedvalue > 0)
            {
                ddlUnitID.Enabled = true;
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DataSet dsUnits = new DataSet();

                dacl.GetCompanyUnit(session.CompanyId, selectedvalue, ref dsUnits);
                foreach (DataRow dr in dsUnits.Tables[0].Rows)
                {
                    ddlUnitID.Items.Add(new ListItem(dr["UnitName"].ToString(), dr["UnitID"].ToString()));
                }
            }
            else
            {
                ddlUnitID.Enabled = false;
            }
            cyReportViewer.Visible = false;
        }

        private void txtDateTo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DropDownList ddlProjectID = pnlParameter.FindControl("ddlProjectID") as DropDownList;
                DropDownList ddlDepartmentID = pnlParameter.FindControl("ddlDepartmentID") as DropDownList;
                DropDownList ddlSectionID = pnlParameter.FindControl("ddlSectionID") as DropDownList;
                DropDownList ddlUnitID = pnlParameter.FindControl("ddlUnitID") as DropDownList;

                ddlProjectID.Items.Clear();
                ddlDepartmentID.Items.Clear();
                ddlSectionID.Items.Clear();
                ddlUnitID.Items.Clear();

                ddlDepartmentID.Enabled = false;
                ddlSectionID.Enabled = false;
                ddlUnitID.Enabled = false;
                short year = Convert.ToInt16(DateTime.Now.Year);
                try
                {
                    year = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue);
                }
                catch (Exception ex)
                {
                    try
                    {
                        TextBox txtDate = pnlParameter.FindControl("txtDateTo") as TextBox;
                        DateTime date = DateTime.Parse(txtDate.Text.ToString());
                        year = Convert.ToInt16(date.Year.ToString());
                    }
                    catch (Exception ex2) { }
                }

                short month = Convert.ToInt16(DateTime.Now.Month);
                try
                {
                    month = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue);
                }
                catch (Exception ex)
                {
                    try
                    {
                        TextBox txtDate = pnlParameter.FindControl("txtDateTo") as TextBox;
                        DateTime date = DateTime.Parse(txtDate.Text.ToString());
                        month = Convert.ToInt16(date.Month.ToString());
                    }
                    catch (Exception ex2) { }
                }
                DataSet dsProjects = new DataSet();
                dacl.GetCompanyProject(session.CompanyId, loginUserOrAlternateParty, reportId, ref dsProjects);
                if (dsProjects.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsProjects.Tables[0].Rows.Count; j++)
                    {
                        ddlProjectID.Items.Add(new ListItem(dsProjects.Tables[0].Rows[j]["ProjectName"].ToString(), dsProjects.Tables[0].Rows[j]["ReportProjectID"].ToString()));
                    }
                    if (Convert.ToInt16(ddlProjectID.SelectedValue) > 0)
                    {
                        DataSet dsDepartments = new DataSet();
                        dacl.GetCompanyDepartment(session.CompanyId, Convert.ToInt16(ddlProjectID.SelectedValue), reportId, loginUserOrAlternateParty, year, month, ref dsDepartments);
                        foreach (DataRow dr in dsDepartments.Tables[0].Rows)
                        {
                            ddlDepartmentID.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
                        }
                        ddlDepartmentID.Enabled = true;
                        //Bind Section if Department > 0
                        if (Convert.ToInt16(ddlDepartmentID.SelectedValue) > 0)
                        {
                            DataSet dsSections = new DataSet();
                            dacl.GetCompanySection(session.CompanyId, Convert.ToInt16(ddlDepartmentID.SelectedValue), reportId, loginUserOrAlternateParty, year, month, ref dsSections);
                            foreach (DataRow dr in dsSections.Tables[0].Rows)
                            {
                                ddlSectionID.Items.Add(new ListItem(dr["SectionName"].ToString(), dr["SectionID"].ToString()));
                            }
                            ddlSectionID.Enabled = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            cyReportViewer.Visible = false;

        }
        #endregion

        private void AddControls()
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();

            int controlCount = 0;

            pnlParameter.Controls.Add(new LiteralControl("<div class='form-horizontal m-t-20'>"));

            #region commented
            //#region @PROJECTID
            //if (crReportDocument.ParameterFields["@ProjectID"] != null || crReportDocument.ParameterFields["ProjectID"] != null)
            //{
            //    GMSGeneralDALC dacl2 = new GMSGeneralDALC();
            //    DataSet dsProjects = new DataSet();
            //    dacl2.GetCompanyProject(session.CompanyId, loginUserOrAlternateParty, reportId, ref dsProjects);

            //    if (dsProjects.Tables[0].Rows.Count > 0)
            //    {
            //        pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //        pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Dim 1 : "));
            //        pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //        pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //        DropDownList ddlProjectID = new DropDownList();
            //        ddlProjectID.ID = "ddlProjectID";
            //        ddlProjectID.CssClass = "form-control";
            //        ddlProjectID.AutoPostBack = true;
            //        ddlProjectID.SelectedIndexChanged += new EventHandler(ddlProjectID_SelectedIndexChanged);
            //        ddlProjectID.Items.Clear();

            //        for (int j = 0; j < dsProjects.Tables[0].Rows.Count; j++)
            //        {
            //            ddlProjectID.Items.Add(new ListItem(dsProjects.Tables[0].Rows[j]["ProjectName"].ToString(), dsProjects.Tables[0].Rows[j]["ReportProjectID"].ToString()));
            //        }

            //        pnlParameter.Controls.Add(ddlProjectID);
            //        pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //        pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //        controlCount = controlCount + 1;


            //        if (crReportDocument.ParameterFields["@DepartmentID"] != null || crReportDocument.ParameterFields["DepartmentID"] != null)
            //        {
            //            //Add in Department DropDown
            //            pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //            pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Dim 2 : "));
            //            pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //            pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));

            //            DropDownList ddlDepartmentID = new DropDownList();
            //            ddlDepartmentID.ID = "ddlDepartmentID";
            //            ddlDepartmentID.CssClass = "form-control";
            //            ddlDepartmentID.Enabled = false;
            //            ddlDepartmentID.AutoPostBack = true;
            //            ddlDepartmentID.SelectedIndexChanged += new EventHandler(ddlDepartmentID_SelectedIndexChanged);
            //            ddlDepartmentID.Items.Clear();

            //            pnlParameter.Controls.Add(ddlDepartmentID);
            //            pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //            pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //            controlCount = controlCount + 1;

            //            if (ViewState["ddlDepartmentID"] == null)
            //                ViewState["ddlDepartmentID"] = -1;



            //            //Add in Section DropDown
            //            pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //            pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Dim 3 : "));
            //            pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //            pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));

            //            DropDownList ddlSectionID = new DropDownList();
            //            ddlSectionID.ID = "ddlSectionID";
            //            ddlSectionID.CssClass = "form-control";
            //            ddlSectionID.Enabled = false;
            //            ddlSectionID.AutoPostBack = true;
            //            ddlSectionID.SelectedIndexChanged += new EventHandler(ddlSectionID_SelectedIndexChanged);
            //            ddlSectionID.Items.Clear();

            //            pnlParameter.Controls.Add(ddlSectionID);
            //            pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //            pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //            controlCount = controlCount + 1;

            //            if (ViewState["ddlSectionID"] == null)
            //                ViewState["ddlSectionID"] = -1;


            //            //Add in Unit DropDown
            //            pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //            pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Dim 4 : "));
            //            pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //            pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));

            //            DropDownList ddlUnitID = new DropDownList();
            //            ddlUnitID.ID = "ddlUnitID";
            //            ddlUnitID.CssClass = "form-control";
            //            ddlUnitID.Enabled = false;
            //            ddlUnitID.Items.Clear();

            //            pnlParameter.Controls.Add(ddlUnitID);
            //            pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //            pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //            controlCount = controlCount + 1;

            //            if (ViewState["ddlUnitID"] == null)
            //                ViewState["ddlUnitID"] = -1;


            //            //Bind Department if Project > 0
            //            if (Convert.ToInt16(ddlProjectID.SelectedValue) > 0 && !IsPostBack)
            //            {

            //                short year = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue);
            //                short month = 0;
            //                //if (((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue == null)
            //                //{
            //                //    month = 4;
            //                //}
            //                //else
            //                //{
            //                //    month = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue);
            //                //}

            //                try
            //                {
            //                    month = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue);
            //                }
            //                catch (Exception ex)
            //                {
            //                    month = Convert.ToInt16(DateTime.Now.Month);
            //                }

            //                ddlDepartmentID.Enabled = true;
            //                DataSet dsDepartments = new DataSet();
            //                dacl.GetCompanyDepartment(session.CompanyId, Convert.ToInt16(ddlProjectID.SelectedValue), reportId, loginUserOrAlternateParty, year, month, ref dsDepartments);

            //                foreach (DataRow dr in dsDepartments.Tables[0].Rows)
            //                {
            //                    ddlDepartmentID.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
            //                }

            //                //Bind Section if Department > 0
            //                if (Convert.ToInt16(ddlDepartmentID.SelectedValue) > 0 && !IsPostBack)
            //                {
            //                    DataSet dsSections = new DataSet();
            //                    dacl.GetCompanySection(session.CompanyId, Convert.ToInt16(ddlDepartmentID.SelectedValue), reportId, loginUserOrAlternateParty, year, month, ref dsSections);
            //                    foreach (DataRow dr in dsSections.Tables[0].Rows)
            //                    {
            //                        ddlSectionID.Items.Add(new ListItem(dr["SectionName"].ToString(), dr["SectionID"].ToString()));
            //                    }
            //                    ddlSectionID.Enabled = true;
            //                }

            //            }



            //        }



            //    }
            //    if (ViewState["ddlProjectID"] == null)
            //        ViewState["ddlProjectID"] = -1;

            //}
            //#endregion

            //if (crReportDocument.ParameterFields["@Type"] != null || crReportDocument.ParameterFields["Type"] != null)
            //{

            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Customer Type :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    DropDownList ddlType = new DropDownList();
            //    ddlType.ID = "ddlType";
            //    ddlType.CssClass = "form-control";
            //    ddlType.Items.Clear();
            //    ddlType.Items.Add(new ListItem("ALL", "All"));
            //    ddlType.Items.Add(new ListItem("EXTERNAL CUSTOMERS", "External"));
            //    ddlType.Items.Add(new ListItem("INTERCO CUSTOMERS", "Internal"));
            //    if (session.CompanyId.ToString() == "14")
            //    {
            //        ddlType.Items.Add(new ListItem("NOX CUSTOMERS", "NOX"));
            //    }
            //    pnlParameter.Controls.Add(ddlType);
            //    if (ViewState["ddlType"] == null)
            //        ViewState["ddlType"] = "All";
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}

            //if (crReportDocument.ParameterFields["@ProductCategory"] != null || crReportDocument.ParameterFields["ProductCategory"] != null)
            //{

            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Product Category :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    DropDownList ddlProductCategory = new DropDownList();
            //    ddlProductCategory.ID = "ddlProductCategory";
            //    ddlProductCategory.CssClass = "form-control";
            //    ddlProductCategory.Items.Clear();

            //    GMSGeneralDALC GSdacl = new GMSGeneralDALC();
            //    DataSet dsPC = new DataSet();
            //    GSdacl.GetSAPProductCategoryForReport(ref dsPC);
            //    if (dsPC.Tables != null && dsPC.Tables[0] != null)
            //    {
            //        foreach (DataRow dr in dsPC.Tables[0].Rows)
            //        {
            //            ddlProductCategory.Items.Add(new ListItem(dr["SubCategoryName"].ToString(), dr["SubCategoryName"].ToString()));
            //        }
            //    }

            //    pnlParameter.Controls.Add(ddlProductCategory);
            //    //if (ViewState["ddlCategory"] == null)
            //    //    ViewState["ddlCategory"] = "All";
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}

            //if (crReportDocument.ParameterFields["@Year"] != null || crReportDocument.ParameterFields["Year"] != null)
            //{

            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Year :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    DropDownList ddlYear = new DropDownList();
            //    ddlYear.ID = "ddlYear";
            //    ddlYear.CssClass = "form-control";

            //    if (reportId.ToString() == "556")
            //    {
            //        ddlYear.AutoPostBack = true;
            //        ddlYear.SelectedIndexChanged += new EventHandler(ddlYear_SelectedIndexChanged);
            //    }

            //    ddlYear.Items.Clear();
            //    for (int i = 2005; i <= 2020; i++)
            //    {
            //        ddlYear.Items.Add(i.ToString());
            //    }

            //    if ((fileDescription == "S") || (fileDescription == "P"))
            //        ddlYear.SelectedValue = DateTime.Now.Year.ToString();
            //    else
            //        ddlYear.SelectedValue = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)).Year.ToString();

            //    pnlParameter.Controls.Add(ddlYear);
            //    if (ViewState["ddlYear"] == null)
            //        ViewState["ddlYear"] = DateTime.Now.Year.ToString();
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}

            //if (crReportDocument.ParameterFields["@Month"] != null || crReportDocument.ParameterFields["Month"] != null)
            //{
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Month :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    DropDownList ddlMonth = new DropDownList();
            //    ddlMonth.ID = "ddlMonth";
            //    ddlMonth.CssClass = "form-control";
            //    if (reportId.ToString() == "556")
            //    {
            //        ddlMonth.AutoPostBack = true;
            //        ddlMonth.SelectedIndexChanged += new EventHandler(ddlMonth_SelectedIndexChanged);
            //    }

            //    ddlMonth.Items.Clear();
            //    for (int i = 1; i <= 12; i++)
            //    {
            //        ddlMonth.Items.Add(i.ToString());
            //    }

            //    if ((fileDescription == "S") || (fileDescription == "P"))
            //        ddlMonth.SelectedValue = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).Month.ToString();
            //    else
            //        ddlMonth.SelectedValue = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)).Month.ToString();


            //    pnlParameter.Controls.Add(ddlMonth);
            //    if (ViewState["ddlMonth"] == null)
            //        ViewState["ddlMonth"] = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)).Month.ToString();
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;

            //}

            //if (crReportDocument.ParameterFields["Brand Code"] != null || crReportDocument.ParameterFields["BrandCode"] != null)
            //{
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Brand/Product Code :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    TextBox txtBrandCode = new TextBox();
            //    txtBrandCode.ID = "txtBrandCode";
            //    txtBrandCode.CssClass = "form-control";
            //    txtBrandCode.Attributes["placeholder"] = "e.g. B11";
            //    pnlParameter.Controls.Add(txtBrandCode);
            //    if (ViewState["txtBrandCode"] == null)
            //        ViewState["txtBrandCode"] = "";
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}

            //if (crReportDocument.ParameterFields["Sales Person"] != null)
            //{
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sales Person Name :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    TextBox txtSalesPersonName = new TextBox();
            //    txtSalesPersonName.ID = "txtSalesPersonName";
            //    txtSalesPersonName.Text = "";
            //    txtSalesPersonName.CssClass = "form-control";
            //    txtSalesPersonName.Attributes["placeholder"] = "e.g. Alex";
            //    pnlParameter.Controls.Add(txtSalesPersonName);
            //    if (ViewState["txtSalesPersonName"] == null)
            //        ViewState["txtSalesPersonName"] = "";
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}


            //if (crReportDocument.ParameterFields["Brand"] != null || crReportDocument.ParameterFields["Product Group"] != null)
            //{
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Brand/Product Name :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    TextBox txtBrand = new TextBox();
            //    txtBrand.ID = "txtBrand";
            //    txtBrand.CssClass = "form-control";
            //    txtBrand.Attributes["placeholder"] = "e.g. BLUEMETALS";
            //    pnlParameter.Controls.Add(txtBrand);
            //    if (ViewState["txtBrand"] == null)
            //        ViewState["txtBrand"] = "";
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}

            //if (crReportDocument.ParameterFields["Product Code"] != null)
            //{

            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Item Code :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    TextBox txtProductCode = new TextBox();
            //    txtProductCode.ID = "txtProductCode";
            //    txtProductCode.CssClass = "form-control";
            //    txtProductCode.Attributes["placeholder"] = "e.g. B1110535616";
            //    pnlParameter.Controls.Add(txtProductCode);
            //    if (ViewState["txtProductCode"] == null)
            //        ViewState["txtProductCode"] = "";
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}


            //if (crReportDocument.ParameterFields["Customer Account Code"] != null || crReportDocument.ParameterFields["@AccountCode"] != null)
            //{
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Customer Code :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    TextBox txtAccountCode = new TextBox();
            //    txtAccountCode.ID = "txtAccountCode";
            //    txtAccountCode.Text = "";
            //    txtAccountCode.CssClass = "form-control";
            //    txtAccountCode.Attributes["placeholder"] = "e.g. DLK690";
            //    pnlParameter.Controls.Add(txtAccountCode);
            //    if (ViewState["txtAccountCode"] == null)
            //        ViewState["txtAccountCode"] = "";
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}

            //if (crReportDocument.ParameterFields["Customer Account Name"] != null || crReportDocument.ParameterFields["Customer Name"] != null)
            //{
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Customer Name :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    TextBox txtAccountName = new TextBox();
            //    txtAccountName.ID = "txtAccountName";
            //    txtAccountName.Text = "";
            //    txtAccountName.CssClass = "form-control";
            //    txtAccountName.Attributes["placeholder"] = "e.g. Keppel";
            //    pnlParameter.Controls.Add(txtAccountName);
            //    if (ViewState["txtAccountName"] == null)
            //        ViewState["txtAccountName"] = "";
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}

            //if (crReportDocument.ParameterFields["SalesPersonID"] != null)
            //{

            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sales Person Code :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    TextBox txtSalesPersonID = new TextBox();
            //    txtSalesPersonID.ID = "txtSalesPersonID";
            //    txtSalesPersonID.CssClass = "form-control";
            //    txtSalesPersonID.Attributes["placeholder"] = "e.g. Adrian";
            //    pnlParameter.Controls.Add(txtSalesPersonID);
            //    if (ViewState["txtSalesPersonID"] == null)
            //        ViewState["txtSalesPersonID"] = "";
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}

            //if (crReportDocument.ParameterFields["SalesPersonName"] != null || crReportDocument.ParameterFields["Sales Person Name"] != null || crReportDocument.ParameterFields["Salesperson"] != null)
            //{

            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sales Person Name :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    TextBox txtSalesPersonName = new TextBox();
            //    txtSalesPersonName.ID = "txtSalesPersonName";
            //    txtSalesPersonName.CssClass = "form-control";
            //    txtSalesPersonName.Attributes["placeholder"] = "e.g. Adrian";
            //    pnlParameter.Controls.Add(txtSalesPersonName);
            //    if (ViewState["txtSalesPersonName"] == null)
            //        ViewState["txtSalesPersonName"] = "";
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}

            //if (crReportDocument.ParameterFields["Industry"] != null)
            //{

            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Industry :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    TextBox txtIndustry = new TextBox();
            //    txtIndustry.ID = "txtIndustry";
            //    txtIndustry.CssClass = "form-control";
            //    txtIndustry.Attributes["placeholder"] = "e.g. Shipyards";
            //    pnlParameter.Controls.Add(txtIndustry);
            //    if (ViewState["txtIndustry"] == null)
            //        ViewState["txtIndustry"] = "";
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}

            //if (crReportDocument.ParameterFields["Product Manager"] != null)
            //{
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Product Manager :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    TextBox txtPM = new TextBox();
            //    txtPM.ID = "txtPM";
            //    txtPM.CssClass = "form-control";
            //    txtPM.Attributes["placeholder"] = "e.g. Alan";
            //    pnlParameter.Controls.Add(txtPM);
            //    if (ViewState["txtPM"] == null)
            //        ViewState["txtPM"] = "";
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}

            //if (crReportDocument.ParameterFields["@SalesPersonType"] != null || crReportDocument.ParameterFields["SalesPersonType"] != null)
            //{

            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
            //    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sales Person Type :"));
            //    pnlParameter.Controls.Add(new LiteralControl("</label>"));
            //    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
            //    DropDownList ddlSalesPersonType = new DropDownList();
            //    ddlSalesPersonType.ID = "ddlSalesPersonType";
            //    ddlSalesPersonType.CssClass = "form-control";
            //    ddlSalesPersonType.Items.Clear();
            //    ddlSalesPersonType.Items.Add(new ListItem("Customer", "Customer"));
            //    ddlSalesPersonType.Items.Add(new ListItem("Invoice", "Invoice"));
            //    if (ViewState["ddlSalesPersonType"] == null)
            //        ViewState["ddlSalesPersonType"] = "Customer";
            //    pnlParameter.Controls.Add(ddlSalesPersonType);
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    pnlParameter.Controls.Add(new LiteralControl("</div>"));
            //    controlCount = controlCount + 1;
            //}
            #endregion

            

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
                    if (reportId == 557)
                    {
                        txtDateTo.AutoPostBack = true;
                        txtDateTo.TextChanged += txtDateTo_TextChanged;
                    }
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
                    for (int i = 2005; i <= DateTime.Now.Year; i++)
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

                if (crReportDocument.ParameterFields["Supplier"] != null)
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

                if (crReportDocument.ParameterFields["Brand Code"] != null || crReportDocument.ParameterFields["BrandCode"] != null || crReportDocument.ParameterFields["@ProductGroupCode"] != null)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Brand/Product Code :"));
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

                if (crReportDocument.ParameterFields["Sales Person"] != null)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Sales Person Name :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    TextBox txtSalesPersonName = new TextBox();
                    txtSalesPersonName.ID = "txtSalesPersonName";
                    txtSalesPersonName.Text = "";
                    txtSalesPersonName.CssClass = "form-control";
                    txtSalesPersonName.Attributes["placeholder"] = "e.g. Alex";
                    pnlParameter.Controls.Add(txtSalesPersonName);
                    if (ViewState["txtSalesPersonName"] == null)
                        ViewState["txtSalesPersonName"] = "";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                }


                if (crReportDocument.ParameterFields["Brand"] != null || crReportDocument.ParameterFields["Product Group"] != null || crReportDocument.ParameterFields["@ProductGroupName"] != null)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Brand/Product Name :"));
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

                if (crReportDocument.ParameterFields["Product Code"] != null || crReportDocument.ParameterFields["@ProductCode"] != null)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Item Code :"));
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

                #region PROJECTID
                if (crReportDocument.ParameterFields["ProjectID"] != null || crReportDocument.ParameterFields["@ProjectID"] != null)
                {
                    //GMSGeneralDALC dacl = new GMSGeneralDALC();
                    DataSet dsProjects = new DataSet();
                    dacl.GetCompanyProject(session.CompanyId, loginUserOrAlternateParty, reportId, ref dsProjects);

                    if (dsProjects.Tables[0].Rows.Count > 0)
                    {
                        pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                        pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Dim 1 : "));
                        pnlParameter.Controls.Add(new LiteralControl("</label>"));
                        pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                        DropDownList ddlProjectID = new DropDownList();
                        ddlProjectID.ID = "ddlProjectID";
                        ddlProjectID.CssClass = "form-control";
                        ddlProjectID.AutoPostBack = true;
                        ddlProjectID.SelectedIndexChanged += new EventHandler(ddlProjectID_SelectedIndexChanged);
                        ddlProjectID.Items.Clear();

                        for (int j = 0; j < dsProjects.Tables[0].Rows.Count; j++)
                        {
                            ddlProjectID.Items.Add(new ListItem(dsProjects.Tables[0].Rows[j]["ProjectName"].ToString(), dsProjects.Tables[0].Rows[j]["ReportProjectID"].ToString()));
                        }

                        pnlParameter.Controls.Add(ddlProjectID);
                        pnlParameter.Controls.Add(new LiteralControl("</div>"));
                        pnlParameter.Controls.Add(new LiteralControl("</div>"));
                        controlCount = controlCount + 1;

                        if (ViewState["ddlProjectID"] == null)
                           ViewState["ddlProjectID"] = -1;

                            if (crReportDocument.ParameterFields["DepartmentID"] != null || crReportDocument.ParameterFields["@DepartmentID"] != null)
                        {
                            //Add in Department DropDown
                            pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                            pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Dim 2 : "));
                            pnlParameter.Controls.Add(new LiteralControl("</label>"));
                            pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));

                            DropDownList ddlDepartmentID = new DropDownList();
                            ddlDepartmentID.ID = "ddlDepartmentID";
                            ddlDepartmentID.CssClass = "form-control";
                            ddlDepartmentID.Enabled = false;
                            ddlDepartmentID.AutoPostBack = true;
                            ddlDepartmentID.SelectedIndexChanged += new EventHandler(ddlDepartmentID_SelectedIndexChanged);
                            ddlDepartmentID.Items.Clear();

                            pnlParameter.Controls.Add(ddlDepartmentID);
                            pnlParameter.Controls.Add(new LiteralControl("</div>"));
                            pnlParameter.Controls.Add(new LiteralControl("</div>"));
                            controlCount = controlCount + 1;

                            if (ViewState["ddlDepartmentID"] == null)
                                ViewState["ddlDepartmentID"] = -1;



                            //Add in Section DropDown
                            pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                            pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Dim 3 : "));
                            pnlParameter.Controls.Add(new LiteralControl("</label>"));
                            pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));

                            DropDownList ddlSectionID = new DropDownList();
                            ddlSectionID.ID = "ddlSectionID";
                            ddlSectionID.CssClass = "form-control";
                            ddlSectionID.Enabled = false;
                            ddlSectionID.AutoPostBack = true;
                            ddlSectionID.SelectedIndexChanged += new EventHandler(ddlSectionID_SelectedIndexChanged);
                            ddlSectionID.Items.Clear();

                            pnlParameter.Controls.Add(ddlSectionID);
                            pnlParameter.Controls.Add(new LiteralControl("</div>"));
                            pnlParameter.Controls.Add(new LiteralControl("</div>"));
                            controlCount = controlCount + 1;

                            if (ViewState["ddlSectionID"] == null)
                                ViewState["ddlSectionID"] = -1;


                            //Add in Unit DropDown
                            pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                            pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Dim 4 : "));
                            pnlParameter.Controls.Add(new LiteralControl("</label>"));
                            pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));

                            DropDownList ddlUnitID = new DropDownList();
                            ddlUnitID.ID = "ddlUnitID";
                            ddlUnitID.CssClass = "form-control";
                            ddlUnitID.Enabled = false;
                            ddlUnitID.Items.Clear();

                            pnlParameter.Controls.Add(ddlUnitID);
                            pnlParameter.Controls.Add(new LiteralControl("</div>"));
                            pnlParameter.Controls.Add(new LiteralControl("</div>"));
                            controlCount = controlCount + 1;

                            if (ViewState["ddlUnitID"] == null)
                                ViewState["ddlUnitID"] = -1;


                            //Bind Department if Project > 0
                            if (Convert.ToInt16(ddlProjectID.SelectedValue) > 0 && !IsPostBack)
                            {
                                ddlDepartmentID.Enabled = true;
                                DataSet dsDepartments = new DataSet();
                                short year = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue);
                                short month = Convert.ToInt16(((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue);


                                dacl.GetCompanyDepartment(session.CompanyId, Convert.ToInt16(ddlProjectID.SelectedValue), reportId, loginUserOrAlternateParty, year, month, ref dsDepartments);

                                foreach (DataRow dr in dsDepartments.Tables[0].Rows)
                                {
                                    ddlDepartmentID.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
                                }

                                //Bind Section if Department > 0
                                if (Convert.ToInt16(ddlDepartmentID.SelectedValue) > 0 && !IsPostBack)
                                {
                                    DataSet dsSections = new DataSet();


                                    dacl.GetCompanySection(session.CompanyId, Convert.ToInt16(ddlDepartmentID.SelectedValue), reportId, loginUserOrAlternateParty, year, month, ref dsSections);
                                    foreach (DataRow dr in dsSections.Tables[0].Rows)
                                    {
                                        ddlSectionID.Items.Add(new ListItem(dr["SectionName"].ToString(), dr["SectionID"].ToString()));
                                    }
                                    ddlSectionID.Enabled = true;
                                }
                            }
                        }
                    }
                    if (ViewState["ddlProjectID"] == null)
                        ViewState["ddlProjectID"] = -1;
                }
                #endregion

                if (crReportDocument.ParameterFields["Requestor"] != null)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Requestor :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    TextBox txtRequestor = new TextBox();
                    txtRequestor.ID = "txtRequestor";
                    txtRequestor.CssClass = "form-control";
                    //txtRequestor.Attributes["placeholder"] = "e.g. T02";
                    pnlParameter.Controls.Add(txtRequestor);
                    if (ViewState["txtRequestor"] == null)
                        ViewState["txtRequestor"] = "";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                }

                if (crReportDocument.ParameterFields["RefNo"] != null)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">RefNo :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    TextBox txtRefNo = new TextBox();
                    txtRefNo.ID = "txtRefNo";
                    txtRefNo.CssClass = "form-control";
                    //txtRequestor.Attributes["placeholder"] = "e.g. T02";
                    pnlParameter.Controls.Add(txtRefNo);
                    if (ViewState["txtRefNo"] == null)
                        ViewState["txtRefNo"] = "";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                }

                if (crReportDocument.ParameterFields["ProjNo"] != null)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">ProjNo :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    TextBox txtProjNo = new TextBox();
                    txtProjNo.ID = "txtProjNo";
                    txtProjNo.CssClass = "form-control";
                    //txtRequestor.Attributes["placeholder"] = "e.g. T02";
                    pnlParameter.Controls.Add(txtProjNo);
                    if (ViewState["txtProjNo"] == null)
                        ViewState["txtProjNo"] = "";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                }

                if (crReportDocument.ParameterFields["BudgetCode"] != null)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">BudgetCode :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    TextBox txtBudgetCode = new TextBox();
                    txtBudgetCode.ID = "txtBudgetCode";
                    txtBudgetCode.CssClass = "form-control";
                    //txtRequestor.Attributes["placeholder"] = "e.g. T02";
                    pnlParameter.Controls.Add(txtBudgetCode);
                    if (ViewState["txtBudgetCode"] == null)
                        ViewState["txtBudgetCode"] = "";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                }

                if (crReportDocument.ParameterFields["MRStatus"] != null)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">MRStatus :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    TextBox txtMRStatus = new TextBox();
                    txtMRStatus.ID = "txtMRStatus";
                    txtMRStatus.CssClass = "form-control";
                    //txtRequestor.Attributes["placeholder"] = "e.g. T02";
                    pnlParameter.Controls.Add(txtMRStatus);
                    if (ViewState["txtMRStatus"] == null)
                        ViewState["txtMRStatus"] = "";
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
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Item Description :"));
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


                if (crReportDocument.ParameterFields["@TopNumber"] != null || crReportDocument.ParameterFields["TopNumber"] != null)
                {

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Top Number :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlTopNumber = new DropDownList();
                    ddlTopNumber.ID = "ddlTopNumber";
                    ddlTopNumber.CssClass = "form-control";
                    ddlTopNumber.Items.Clear();
                    //ddlTopNumber.Items.Add(new ListItem("TOP 50", "TOP 50"));
                    //ddlTopNumber.Items.Add(new ListItem("TOP 100", "TOP 100"));
                    ddlTopNumber.Items.Add(new ListItem("TOP SALES", "TOP SALES"));
                    ddlTopNumber.Items.Add(new ListItem("TOP STOCK", "TOP STOCK"));

                    pnlParameter.Controls.Add(ddlTopNumber);
                    if (ViewState["ddlTopNumber"] == null)
                        ViewState["ddlTopNumber"] = "All";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                }


                if (crReportDocument.ParameterFields["@Category"] != null || crReportDocument.ParameterFields["Category"] != null)
                {

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Category :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlCategory = new DropDownList();
                    ddlCategory.ID = "ddlCategory";
                    ddlCategory.CssClass = "form-control";
                    ddlCategory.Items.Clear();

                    GMSGeneralDALC GSdacl = new GMSGeneralDALC();
                    DataSet dsPC = new DataSet();
                    GSdacl.GetProductCategoryForReport(ref dsPC);
                    if (dsPC.Tables != null && dsPC.Tables[0] != null)
                    {
                        foreach (DataRow dr in dsPC.Tables[0].Rows)
                        {
                            ddlCategory.Items.Add(new ListItem(dr["SubCategoryName"].ToString(), dr["SubCategoryName"].ToString()));
                        }
                    }

                    pnlParameter.Controls.Add(ddlCategory);
                    //if (ViewState["ddlCategory"] == null)
                    //    ViewState["ddlCategory"] = "All";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                }

                if (crReportDocument.ParameterFields["@ProductCategory"] != null || crReportDocument.ParameterFields["ProductCategory"] != null)
                {

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Product Category :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlProductCategory = new DropDownList();
                    ddlProductCategory.ID = "ddlProductCategory";
                    ddlProductCategory.CssClass = "form-control";
                    ddlProductCategory.Items.Clear();

                    GMSGeneralDALC GSdacl = new GMSGeneralDALC();
                    DataSet dsPC = new DataSet();
                    GSdacl.GetSAPProductCategoryForReport(ref dsPC);
                    if (dsPC.Tables != null && dsPC.Tables[0] != null)
                    {
                        foreach (DataRow dr in dsPC.Tables[0].Rows)
                        {
                            ddlProductCategory.Items.Add(new ListItem(dr["SubCategoryName"].ToString(), dr["SubCategoryName"].ToString()));
                        }
                    }

                    pnlParameter.Controls.Add(ddlProductCategory);
                    //if (ViewState["ddlCategory"] == null)
                    //    ViewState["ddlCategory"] = "All";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                }

            if (crReportDocument.ParameterFields["@BrandCategory"] != null || crReportDocument.ParameterFields["BrandCategory"] != null)
            {

                pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Brand Category :"));
                pnlParameter.Controls.Add(new LiteralControl("</label>"));
                pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                DropDownList ddlBrandCategory = new DropDownList();
                ddlBrandCategory.ID = "ddlBrandCategory";
                ddlBrandCategory.CssClass = "form-control";
                ddlBrandCategory.Items.Clear();
                ddlBrandCategory.Items.Add(new ListItem("All", "All"));

                GMSGeneralDALC GSdacl = new GMSGeneralDALC();
                DataSet dsPC = new DataSet();
                GSdacl.GetBrandCategoryForReport(ref dsPC);
                if (dsPC.Tables != null && dsPC.Tables[0] != null)
                {
                    foreach (DataRow dr in dsPC.Tables[0].Rows)
                    {
                        ddlBrandCategory.Items.Add(new ListItem(dr["Shortname"].ToString(), dr["Shortname"].ToString()));
                    }
                }

                pnlParameter.Controls.Add(ddlBrandCategory);
                if (ViewState["ddlCategory"] == null)
                    ViewState["ddlCategory"] = "All";
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

                if (crReportDocument.ParameterFields["@EC"] != null)
                {

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">EC Product Group:"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlEC = new DropDownList();
                    ddlEC.ID = "ddlEC";
                    ddlEC.CssClass = "form-control";
                    ddlEC.Items.Clear();
                    ddlEC.Items.Add(new ListItem("EXCLUDE", "EXCLUDE"));
                    ddlEC.Items.Add(new ListItem("INCLUDE", "INCLUDE"));

                    pnlParameter.Controls.Add(ddlEC);
                    if (ViewState["ddlEC"] == null)
                        ViewState["ddlEC"] = "All";
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

                if (crReportDocument.ParameterFields["@DeliveryStatus"] != null || crReportDocument.ParameterFields["DeliveryStatus"] != null)
                {

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Delivery Status :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlDeliveryStatus = new DropDownList();
                    ddlDeliveryStatus.ID = "ddlDeliveryStatus";
                    ddlDeliveryStatus.CssClass = "form-control";
                    ddlDeliveryStatus.Items.Clear();

                    ddlDeliveryStatus.Items.Add(new ListItem("PARTIAL DELIVERED", "PARTIAL DELIVERED"));
                    ddlDeliveryStatus.Items.Add(new ListItem("FULLY DELIVERED", "FULLY DELIVERED"));

                    pnlParameter.Controls.Add(ddlDeliveryStatus);
                    if (ViewState["ddlDeliveryStatus"] == null)
                        ViewState["ddlDeliveryStatus"] = "All";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                }

                if (crReportDocument.ParameterFields["@Action"] != null || crReportDocument.ParameterFields["Action"] != null)
                {

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Action :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlAction = new DropDownList();
                    ddlAction.ID = "ddlAction";
                    ddlAction.CssClass = "form-control";
                    ddlAction.Items.Clear();

                    ddlAction.Items.Add(new ListItem("ALL", "ALL"));
                    ddlAction.Items.Add(new ListItem("ON TIME", "ON TIME"));
                    ddlAction.Items.Add(new ListItem("EXPEDITE", "EXPEDITE"));
                    ddlAction.Items.Add(new ListItem("LATE", "LATE"));
                    ddlAction.Items.Add(new ListItem("CLOSED", "CLOSED"));


                    pnlParameter.Controls.Add(ddlAction);
                    if (ViewState["ddlAction"] == null)
                        ViewState["ddlAction"] = "All";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                }

                if (crReportDocument.ParameterFields["@TrnDateType"] != null)
                {

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Credit Term :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlTrnDateType = new DropDownList();
                    ddlTrnDateType.ID = "ddlTrnDateType";
                    ddlTrnDateType.CssClass = "form-control";
                    ddlTrnDateType.Items.Clear();
                    ddlTrnDateType.Items.Add(new ListItem("Default (Without Credit Term)", "Default"));
                    ddlTrnDateType.Items.Add(new ListItem("With Credit Term", "CreditDate"));

                    pnlParameter.Controls.Add(ddlTrnDateType);
                    if (ViewState["ddlTrnDateType"] == null)
                        ViewState["ddlTrnDateType"] = "All";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));

                    controlCount = controlCount + 1;
                }


                if (crReportDocument.ParameterFields["@DivisionType"] != null)
                {

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Division :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlDivision = new DropDownList();
                    ddlDivision.ID = "ddlDivision";
                    ddlDivision.CssClass = "form-control";
                    ddlDivision.Items.Clear();
                    ddlDivision.Items.Add(new ListItem("Default", "Default"));
                    short coyid = session.CompanyId;
                    if (coyid == 120)
                    {
                        ddlDivision.Items.Add(new ListItem("GAS", "GAS"));
                        ddlDivision.Items.Add(new ListItem("WSD", "WSD"));
                    }
                    //else if (coyid == 104)
                    //{
                    //    ddlDivision.Items.Add(new ListItem("LGS", "LGS"));
                    //    ddlDivision.Items.Add(new ListItem("NIT", "NIT"));
                    //}else if (coyid == 17)
                    //{
                    //    ddlDivision.Items.Add(new ListItem("AS", "AS"));
                    //    ddlDivision.Items.Add(new ListItem("SP", "SP"));
                    //    ddlDivision.Items.Add(new ListItem("BM", "BM"));
                    //    ddlDivision.Items.Add(new ListItem("BS", "BS"));
                    //    ddlDivision.Items.Add(new ListItem("IP", "IP"));
                    //    ddlDivision.Items.Add(new ListItem("TI", "TI"));
                    //    ddlDivision.Items.Add(new ListItem("SA", "SA"));
                    //    ddlDivision.Items.Add(new ListItem("JB", "JB"));
                    //    ddlDivision.Items.Add(new ListItem("KT", "KT"));
                    //    ddlDivision.Items.Add(new ListItem("LSBMT", "LSBMT"));
                    //    ddlDivision.Items.Add(new ListItem("LSBM", "LSBM"));
                    //    ddlDivision.Items.Add(new ListItem("RW", "RW"));
                    //}
                    //else if (coyid == 20)
                    //{
                    //    ddlDivision.Items.Add(new ListItem("MMI", "MMI"));
                    //    ddlDivision.Items.Add(new ListItem("MALACCA", "MALACCA"));
                    //}

                    pnlParameter.Controls.Add(ddlDivision);
                    if (ViewState["ddlDivision"] == null)
                        ViewState["ddlDivision"] = "All";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));

                    controlCount = controlCount + 1;
                }

                if (crReportDocument.ParameterFields["@Doctype"] != null || crReportDocument.ParameterFields["DocType"] != null)
                {

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Document Type :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlDocument = new DropDownList();
                    ddlDocument.ID = "ddlDocument";
                    ddlDocument.CssClass = "form-control";
                    ddlDocument.Items.Clear();
                    ddlDocument.Items.Add(new ListItem("Trn Date", "TrnDate"));
                    ddlDocument.Items.Add(new ListItem("Doc Date", "DocDate"));

                    pnlParameter.Controls.Add(ddlDocument);
                    if (ViewState["ddlDocument"] == null)
                        ViewState["ddlDocument"] = "All";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                }


                if (crReportDocument.ParameterFields["@GroupBy"] != null || crReportDocument.ParameterFields["GroupBy"] != null)
                {

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Group By :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlGroupBy = new DropDownList();
                    ddlGroupBy.ID = "ddlGroupBy";
                    ddlGroupBy.CssClass = "form-control";
                    ddlGroupBy.Items.Clear();
                    ddlGroupBy.Items.Add(new ListItem("Customer", "C"));
                    ddlGroupBy.Items.Add(new ListItem("Product Code", "P"));

                    pnlParameter.Controls.Add(ddlGroupBy);
                    if (ViewState["ddlGroupBy"] == null)
                        ViewState["ddlGroupBy"] = "All";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));

                    controlCount = controlCount + 1;

                }

                if (crReportDocument.ParameterFields["@Grouping"] != null || crReportDocument.ParameterFields["Grouping"] != null)
                {

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Group By :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlGrouping = new DropDownList();
                    ddlGrouping.ID = "ddlGrouping";
                    ddlGrouping.CssClass = "form-control";
                    ddlGrouping.Items.Clear();
                    ddlGrouping.Items.Add(new ListItem("Customer", "C"));
                    ddlGrouping.Items.Add(new ListItem("Product Code", "P"));
                    ddlGrouping.Items.Add(new ListItem("Product Group Only", "O"));

                    pnlParameter.Controls.Add(ddlGrouping);
                    if (ViewState["ddlGrouping"] == null)
                        ViewState["ddlGrouping"] = "All";
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
                    if (ViewState["ddlSalesPersonType"] == null)
                        ViewState["ddlSalesPersonType"] = "Customer";
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
                        ddlRental.Items.Add(new ListItem("WITHOUT ZL", "WITHOUT ZL"));
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
                    if (reportId.ToString() == "137")
                    {
                        ddlCreditLimit.Items.Add(new ListItem("ALL", ""));
                        ddlCreditLimit.Items.Add(new ListItem("0", "0"));
                    }
                    else
                    {
                        ddlCreditLimit.Items.Add(new ListItem("ALL", "A"));
                        ddlCreditLimit.Items.Add(new ListItem("5000 AND ABOVE", "G"));
                        ddlCreditLimit.Items.Add(new ListItem("BELOW 5000", "L"));
                    }

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
                    DropDownList ddlScheme = new DropDownList();
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
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Enter Status :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"tbLabel\" valign=\"top\">"));
                    //pnlParameter.Controls.Add(new LiteralControl("</td>"));
                    //pnlParameter.Controls.Add(new LiteralControl("<td valign=\"top\"></td>"));
                    //pnlParameter.Controls.Add(new LiteralControl("<td>"));
                    ListBox lbStatus = new ListBox();
                    lbStatus.ID = "lbStatus";
                    lbStatus.Rows = 15;
                    lbStatus.Height = 150;
                    lbStatus.CssClass = "form-control";
                    lbStatus.SelectionMode = ListSelectionMode.Multiple;


                    lbStatus.Items.Clear();
                    lbStatus.AutoPostBack = false;

                    string[] tempStatus = new string[] { "New", "Ongoing", "Completed", "Closed", "Cancelled" };

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
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));


                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\" align=\"middle\" valign=\"middle\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<td>"));
                    ImageButton ImgbtnRight = new ImageButton();
                    ImgbtnRight.Click += new ImageClickEventHandler(btnRight_Click);
                    ImgbtnRight.ImageUrl = "../../images/image1.jpg";
                    ImgbtnRight.ImageAlign = ImageAlign.Middle;

                    ImageButton ImgbtnLeft = new ImageButton();
                    ImgbtnLeft.Click += new ImageClickEventHandler(btnLeft_Click);
                    ImgbtnLeft.ImageUrl = "../../images/image2.jpg";
                    ImgbtnLeft.ImageAlign = ImageAlign.Middle;

                    pnlParameter.Controls.Add(new LiteralControl("<br /><br />"));
                    pnlParameter.Controls.Add(ImgbtnRight);
                    pnlParameter.Controls.Add(new LiteralControl("<br /><br />"));
                    pnlParameter.Controls.Add(ImgbtnLeft);


                    pnlParameter.Controls.Add(new LiteralControl("</td>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<td>"));
                    ListBox lbStatusSelected = new ListBox();
                    lbStatusSelected.ID = "lbStatusSelected";
                    lbStatusSelected.Rows = 15;
                    lbStatusSelected.Height = 150;
                    lbStatusSelected.AutoPostBack = false;
                    lbStatusSelected.CssClass = "form-control";
                    lbStatusSelected.SelectionMode = ListSelectionMode.Multiple;
                    lbStatusSelected.Items.Clear();
                    pnlParameter.Controls.Add(lbStatusSelected);

                    if (ViewState["lbStatusSelected"] == null)
                        ViewState["lbStatusSelected"] = null;

                    pnlParameter.Controls.Add(new LiteralControl("</td>"));
                    pnlParameter.Controls.Add(new LiteralControl("</tr>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
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

                if (crReportDocument.ParameterFields["@MRScheme"] != null)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Division :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlMRScheme = new DropDownList();
                    ddlMRScheme.ID = "ddlMRScheme";
                    ddlMRScheme.CssClass = "form-control";
                    ddlMRScheme.Items.Clear();
                    if (session.CompanyId == 120)
                    {
                        ddlMRScheme.Items.Add(new ListItem("Gas", "Department"));
                        ddlMRScheme.Items.Add(new ListItem("Welding", "Product"));
                    }
                    else
                    {
                        ddlMRScheme.Items.Add(new ListItem("--", "Department"));
                    }
                    pnlParameter.Controls.Add(ddlMRScheme);
                    if (ViewState["ddlMRScheme"] == null)
                        ViewState["ddlMRScheme"] = "Department";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                }



                if (crReportDocument.ParameterFields["@GroupID"] != null || crReportDocument.ParameterFields["GroupID"] != null)
                {
                    SystemDataActivity sDA = new SystemDataActivity();
                    IList<GMSCore.Entity.SalesGroup> lstEE = null;
                    lstEE = sDA.RetrieveTeamSetupSalesGroup(session.CompanyId);

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Group :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlGroup = new DropDownList();
                    ddlGroup.ID = "ddlGroup";
                    ddlGroup.CssClass = "form-control";
                    ddlGroup.AutoPostBack = true;
                    ddlGroup.SelectedIndexChanged += new EventHandler(ddlGroup_SelectedIndexChanged);

                    ddlGroup.Items.Clear();
                    foreach (var x in lstEE)
                    {
                        ddlGroup.Items.Add(new ListItem(x.GroupName, x.GroupID.ToString()));
                    }

                    pnlParameter.Controls.Add(ddlGroup);
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;

                    if (ViewState["ddlGroup"] == null)
                        ViewState["ddlGroup"] = 0;

                    if (crReportDocument.ParameterFields["@TeamID"] != null || crReportDocument.ParameterFields["TeamID"] != null)
                    {
                        pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                        pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Team :"));
                        pnlParameter.Controls.Add(new LiteralControl("</label>"));
                        pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                        DropDownList ddlTeam = new DropDownList();
                        ddlTeam.ID = "ddlTeam";
                        ddlTeam.CssClass = "form-control";
                        ddlTeam.Enabled = false;
                        ddlTeam.AutoPostBack = true;

                        pnlParameter.Controls.Add(ddlTeam);
                        pnlParameter.Controls.Add(new LiteralControl("</div>"));
                        pnlParameter.Controls.Add(new LiteralControl("</div>"));
                        controlCount = controlCount + 1;
                        if (ViewState["ddlTeam"] == null)
                            ViewState["ddlTeam"] = 0;

                    }

                }

                if (crReportDocument.ParameterFields["@FilterBy"] != null)
                {

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Filter By :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlFilterBy = new DropDownList();
                    ddlFilterBy.ID = "ddlFilterBy";
                    ddlFilterBy.CssClass = "form-control";
                    ddlFilterBy.Items.Clear();
                    ddlFilterBy.Items.Add(new ListItem("Creation Date", "CreationDate"));
                    ddlFilterBy.Items.Add(new ListItem("Date based on Status", "OtherDate"));

                    pnlParameter.Controls.Add(ddlFilterBy);
                    if (ViewState["ddlFilterBy"] == null)
                        ViewState["ddlFilterBy"] = "All";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));

                    controlCount = controlCount + 1;
                }


                if (crReportDocument.ParameterFields["@TransactionType"] != null)
                {

                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Transaction Type:"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    DropDownList ddlTT = new DropDownList();
                    ddlTT.ID = "ddlTT";
                    ddlTT.CssClass = "form-control";
                    ddlTT.Items.Clear();
                    ddlTT.Items.Add(new ListItem("Product", "Product"));
                    ddlTT.Items.Add(new ListItem("Invoice", "Invoice"));

                    pnlParameter.Controls.Add(ddlTT);
                    if (ViewState["ddlTT"] == null)
                        ViewState["ddlTT"] = "All";
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));

                    controlCount = controlCount + 1;
                }

                if (crReportDocument.ParameterFields["@Effectivedate"] != null)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Effective Date :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"input-group date\">"));
                    TextBox txtEffectiveDate = new TextBox();
                    txtEffectiveDate.ID = "txtEffectiveDate";
                    txtEffectiveDate.Columns = 15;
                    txtEffectiveDate.Text = System.DateTime.Today.ToString("dd/MM/yyyy");
                    txtEffectiveDate.CssClass = "form-control datepicker";
                    pnlParameter.Controls.Add(txtEffectiveDate);
                    if (ViewState["txtEffectiveDate"] == null)
                        ViewState["txtEffectiveDate"] = txtEffectiveDate.Text;
                    pnlParameter.Controls.Add(new LiteralControl("<span class=\"input-group-addon\"><i class=\"ti-calendar\"></i></span>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
                    controlCount = controlCount + 1;
                }

                if (crReportDocument.ParameterFields["@ReviewDate"] != null)
                {
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"form-group col-lg-6 col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<label class=\"col-sm-6 control-label text-left\">Review Date :"));
                    pnlParameter.Controls.Add(new LiteralControl("</label>"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"col-sm-6\">"));
                    pnlParameter.Controls.Add(new LiteralControl("<div class=\"input-group date\">"));
                    TextBox txtReviewDate = new TextBox();
                    txtReviewDate.ID = "txtReviewDate";
                    txtReviewDate.Columns = 15;
                    txtReviewDate.Text = System.DateTime.Today.ToString("dd/MM/yyyy");
                    txtReviewDate.CssClass = "form-control datepicker";
                    pnlParameter.Controls.Add(txtReviewDate);
                    if (ViewState["txtReviewDate"] == null)
                        ViewState["txtReviewDate"] = txtReviewDate.Text;
                    pnlParameter.Controls.Add(new LiteralControl("<span class=\"input-group-addon\"><i class=\"ti-calendar\"></i></span>"));
                    pnlParameter.Controls.Add(new LiteralControl("</div>"));
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
            cyReportViewer.Visible = true;


            if (crReportDocument.ParameterFields["@Year"] != null)
                ViewState["ddlYear"] = ((DropDownList)pnlParameter.FindControl("ddlYear")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@Month"] != null)
                ViewState["ddlMonth"] = ((DropDownList)pnlParameter.FindControl("ddlMonth")).SelectedValue.ToString();
            if (crReportDocument.ParameterFields["@ProjectID"] != null && (DropDownList)pnlParameter.FindControl("ddlProjectID") != null || crReportDocument.ParameterFields["ProjectID"] != null && (DropDownList)pnlParameter.FindControl("ddlProjectID") != null)
                    ViewState["ddlProjectID"] = ((DropDownList)pnlParameter.FindControl("ddlProjectID")).SelectedValue.ToString();
                else
                    ViewState["ddlProjectID"] = -1;
            if (crReportDocument.ParameterFields["@DepartmentID"] != null && (DropDownList)pnlParameter.FindControl("ddlDepartmentID") != null
                     && !string.IsNullOrEmpty(((DropDownList)pnlParameter.FindControl("ddlDepartmentID")).SelectedValue.ToString()) || crReportDocument.ParameterFields["DepartmentID"] != null && (DropDownList)pnlParameter.FindControl("ddlDepartmentID") != null)
                    ViewState["ddlDepartmentID"] = ((DropDownList)pnlParameter.FindControl("ddlDepartmentID")).SelectedValue.ToString();
                else
                    ViewState["ddlDepartmentID"] = -1;
                if (crReportDocument.ParameterFields["@SectionID"] != null && (DropDownList)pnlParameter.FindControl("ddlSectionID") != null
                    && !string.IsNullOrEmpty(((DropDownList)pnlParameter.FindControl("ddlSectionID")).SelectedValue.ToString()) || crReportDocument.ParameterFields["UnitID"] != null && (DropDownList)pnlParameter.FindControl("ddlUnitID") != null)
                    ViewState["ddlSectionID"] = ((DropDownList)pnlParameter.FindControl("ddlSectionID")).SelectedValue.ToString();
                else
                    ViewState["ddlSectionID"] = -1;
                if (crReportDocument.ParameterFields["@UnitID"] != null && (DropDownList)pnlParameter.FindControl("ddlUnitID") != null
                    && !string.IsNullOrEmpty(((DropDownList)pnlParameter.FindControl("ddlUnitID")).SelectedValue.ToString()) || crReportDocument.ParameterFields["UnitID"] != null && (DropDownList)pnlParameter.FindControl("ddlUnitID") != null)
                    ViewState["ddlUnitID"] = ((DropDownList)pnlParameter.FindControl("ddlUnitID")).SelectedValue.ToString();
                else
                    ViewState["ddlUnitID"] = -1;
                if (crReportDocument.ParameterFields["@Currency"] != null && session.DefaultCurrency != "SGD")
                    ViewState["ddlCurrencyCode"] = ((DropDownList)pnlParameter.FindControl("ddlCurrencyCode")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["Brand Code"] != null || crReportDocument.ParameterFields["@ProductGroupCode"] != null)
                ViewState["txtBrandCode"] = ((TextBox)pnlParameter.FindControl("txtBrandCode")).Text.ToString();
                if (crReportDocument.ParameterFields["BrandCode"] != null)
                    ViewState["txtBrandCode"] = ((TextBox)pnlParameter.FindControl("txtBrandCode")).Text.ToString();
                if (crReportDocument.ParameterFields["Brand"] != null || crReportDocument.ParameterFields["Product Group"] != null || crReportDocument.ParameterFields["@ProductGroupName"] != null)
                    ViewState["txtBrand"] = ((TextBox)pnlParameter.FindControl("txtBrand")).Text.ToString();
                if (crReportDocument.ParameterFields["Product Code"] != null || crReportDocument.ParameterFields["@ProductCode"] != null)
                    ViewState["txtProductCode"] = ((TextBox)pnlParameter.FindControl("txtProductCode")).Text.ToString();
                if (crReportDocument.ParameterFields["@SalesPersonID"] != null || crReportDocument.ParameterFields["SalesPersonID"] != null)
                    ViewState["ddlSalesPersonID"] = ((DropDownList)pnlParameter.FindControl("ddlSalesPersonID")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@Type"] != null || crReportDocument.ParameterFields["Type"] != null)
                    ViewState["ddlType"] = ((DropDownList)pnlParameter.FindControl("ddlType")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@SalesPersonType"] != null || crReportDocument.ParameterFields["SalesPersonType"] != null)
                    ViewState["ddlSalesPersonType"] = ((DropDownList)pnlParameter.FindControl("ddlSalesPersonType")).SelectedValue.ToString();
          
          
            


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
                if (crReportDocument.ParameterFields["@Doctype"] != null || crReportDocument.ParameterFields["Doctype"] != null)
                    ViewState["ddlDocument"] = ((DropDownList)pnlParameter.FindControl("ddlDocument")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@TopNumber"] != null || crReportDocument.ParameterFields["TopNumber"] != null)
                    ViewState["ddlTopNumber"] = ((DropDownList)pnlParameter.FindControl("ddlTopNumber")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@DeliveryStatus"] != null || crReportDocument.ParameterFields["DeliveryStatus"] != null)
                    ViewState["ddlDeliveryStatus"] = ((DropDownList)pnlParameter.FindControl("ddlDeliveryStatus")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@Action"] != null || crReportDocument.ParameterFields["Action"] != null)
                    ViewState["ddlAction"] = ((DropDownList)pnlParameter.FindControl("ddlAction")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@Category"] != null || crReportDocument.ParameterFields["Category"] != null)
                    ViewState["ddlCategory"] = ((DropDownList)pnlParameter.FindControl("ddlCategory")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@ProductCategory"] != null || crReportDocument.ParameterFields["ProductCategory"] != null)
                    ViewState["ddlProductCategory"] = ((DropDownList)pnlParameter.FindControl("ddlProductCategory")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@BrandCategory"] != null || crReportDocument.ParameterFields["BrandCategory"] != null)
                ViewState["ddlBrandCategory"] = ((DropDownList)pnlParameter.FindControl("ddlBrandCategory")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@Zcode"] != null || crReportDocument.ParameterFields["Zcode"] != null)
                    ViewState["ddlZcode"] = ((DropDownList)pnlParameter.FindControl("ddlZcode")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@EC"] != null || crReportDocument.ParameterFields["EC"] != null)
                    ViewState["ddlEC"] = ((DropDownList)pnlParameter.FindControl("ddlEC")).SelectedValue.ToString();
                //if (crReportDocument.ParameterFields["@SalesPersonType"] != null || crReportDocument.ParameterFields["SalesPersonType"] != null)
                //    ViewState["ddlSalesPersonType"] = ((DropDownList)pnlParameter.FindControl("ddlSalesPersonType")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@GroupBy"] != null || crReportDocument.ParameterFields["GroupBy"] != null)
                    ViewState["ddlGroupBy"] = ((DropDownList)pnlParameter.FindControl("ddlGroupBy")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@Grouping"] != null || crReportDocument.ParameterFields["Grouping"] != null)
                    ViewState["ddlGrouping"] = ((DropDownList)pnlParameter.FindControl("ddlGrouping")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@SalesType"] != null || crReportDocument.ParameterFields["SalesType"] != null)
                    ViewState["ddlSalesType"] = ((DropDownList)pnlParameter.FindControl("ddlSalesType")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@Rental"] != null)
                    ViewState["ddlRental"] = ((DropDownList)pnlParameter.FindControl("ddlRental")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@RentalType"] != null)
                    ViewState["ddlRentalType"] = ((DropDownList)pnlParameter.FindControl("ddlRentalType")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@SalesPersonID"] != null || crReportDocument.ParameterFields["SalesPersonID"] != null)
                    ViewState["ddlSalesPersonID"] = ((DropDownList)pnlParameter.FindControl("ddlSalesPersonID")).SelectedValue.ToString();
                if (crReportDocument.ParameterFields["@SalesPersonType"] != null || crReportDocument.ParameterFields["SalesPersonType"] != null)
                    ViewState["ddlSalesPersonType"] = ((DropDownList)pnlParameter.FindControl("ddlSalesPersonType")).SelectedValue.ToString();
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
                if (crReportDocument.ParameterFields["Supplier"] != null)
                    ViewState["txtSupplier"] = ((TextBox)pnlParameter.FindControl("txtSupplier")).Text.ToString();
                if (crReportDocument.ParameterFields["Brand Code"] != null || crReportDocument.ParameterFields["@ProductGroupCode"] != null)
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

                //if (crReportDocument.ParameterFields["ProjectID"] != null && (DropDownList)pnlParameter.FindControl("ddlProjectID") != null)
                //    ViewState["ddlProjectID"] = ((DropDownList)pnlParameter.FindControl("ddlProjectID")).SelectedValue.ToString();
                //else
                //    ViewState["ddlProjectID"] = -1;

                //if (crReportDocument.ParameterFields["DepartmentID"] != null && (DropDownList)pnlParameter.FindControl("ddlDepartmentID") != null)
                //    ViewState["ddlDepartmentID"] = ((DropDownList)pnlParameter.FindControl("ddlDepartmentID")).SelectedValue.ToString();
                //else
                //    ViewState["ddlDepartmentID"] = -1;
                //if (crReportDocument.ParameterFields["SectionID"] != null && (DropDownList)pnlParameter.FindControl("ddlSectionID") != null)
                //    ViewState["ddlSectionID"] = ((DropDownList)pnlParameter.FindControl("ddlSectionID")).SelectedValue.ToString();
                //else
                //    ViewState["ddlSectionID"] = -1;

                //if (crReportDocument.ParameterFields["UnitID"] != null && (DropDownList)pnlParameter.FindControl("ddlUnitID") != null)
                //    ViewState["ddlUnitID"] = ((DropDownList)pnlParameter.FindControl("ddlUnitID")).SelectedValue.ToString();
                //else
                //    ViewState["ddlUnitID"] = -1;

                if (crReportDocument.ParameterFields["Requestor"] != null)
                    ViewState["txtRequestor"] = ((TextBox)pnlParameter.FindControl("txtRequestor")).Text.ToString();
                if (crReportDocument.ParameterFields["RefNo"] != null)
                    ViewState["txtRefNo"] = ((TextBox)pnlParameter.FindControl("txtRefNo")).Text.ToString();
                if (crReportDocument.ParameterFields["ProjNo"] != null)
                    ViewState["txtProjNo"] = ((TextBox)pnlParameter.FindControl("txtProjNo")).Text.ToString();
                if (crReportDocument.ParameterFields["BudgetCode"] != null)
                    ViewState["txtBudgetCode"] = ((TextBox)pnlParameter.FindControl("txtBudgetCode")).Text.ToString();
                if (crReportDocument.ParameterFields["MRStatus"] != null)
                    ViewState["txtMRStatus"] = ((TextBox)pnlParameter.FindControl("txtMRStatus")).Text.ToString();

                if (crReportDocument.ParameterFields["@TransactionType"] != null)
                    ViewState["ddlTT"] = ((DropDownList)pnlParameter.FindControl("ddlTT")).SelectedValue.ToString();

                if (crReportDocument.ParameterFields["@TrnDateType"] != null)
                    ViewState["ddlTrnDateType"] = ((DropDownList)pnlParameter.FindControl("ddlTrnDateType")).SelectedValue.ToString();

                if (crReportDocument.ParameterFields["@FilterBy"] != null)
                    ViewState["ddlFilterBy"] = ((DropDownList)pnlParameter.FindControl("ddlFilterBy")).SelectedValue.ToString();

                if (crReportDocument.ParameterFields["@DivisionType"] != null)
                    ViewState["ddlDivision"] = ((DropDownList)pnlParameter.FindControl("ddlDivision")).SelectedValue.ToString();

                if (crReportDocument.ParameterFields["@Effectivedate"] != null)
                    ViewState["txtEffectiveDate"] = ((TextBox)pnlParameter.FindControl("txtEffectiveDate")).Text.ToString();

                if (crReportDocument.ParameterFields["@ReviewDate"] != null)
                    ViewState["txtReviewDate"] = ((TextBox)pnlParameter.FindControl("txtReviewDate")).Text.ToString();

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

                if (crReportDocument.ParameterFields["@GroupID"] != null && (DropDownList)pnlParameter.FindControl("ddlGroup") != null
                        && !string.IsNullOrEmpty(((DropDownList)pnlParameter.FindControl("ddlGroup")).SelectedValue.ToString()))
                    ViewState["ddlGroup"] = ((DropDownList)pnlParameter.FindControl("ddlGroup")).SelectedValue;
                else
                    ViewState["ddlGroup"] = 0;

                if (crReportDocument.ParameterFields["@TeamID"] != null && (DropDownList)pnlParameter.FindControl("ddlTeam") != null
                        && !string.IsNullOrEmpty(((DropDownList)pnlParameter.FindControl("ddlTeam")).SelectedValue.ToString()))
                    ViewState["ddlTeam"] = ((DropDownList)pnlParameter.FindControl("ddlTeam")).SelectedValue;
                else
                    ViewState["ddlTeam"] = 0;

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
                        if (crReportDocument.ParameterFields["@Month"] != null && ViewState["ddlMonth"] != null)
                            crReportDocument.SetParameterValue("@Month", GMSUtil.ToInt(ViewState["ddlMonth"].ToString()));
                        if (crReportDocument.ParameterFields["@ProjectID"] != null && ViewState["ddlProjectID"] != null)
                            crReportDocument.SetParameterValue("@ProjectID", GMSUtil.ToInt(ViewState["ddlProjectID"].ToString()));
                        if (crReportDocument.ParameterFields["ProjectID"] != null && ViewState["ddlProjectID"] != null)
                        crReportDocument.SetParameterValue("ProjectID", ViewState["ddlProjectID"].ToString());
                        if (crReportDocument.ParameterFields["@DepartmentID"] != null && ViewState["ddlDepartmentID"] != null)
                            crReportDocument.SetParameterValue("@DepartmentID", GMSUtil.ToInt(ViewState["ddlDepartmentID"].ToString()));
                        if (crReportDocument.ParameterFields["DepartmentID"] != null && ViewState["ddlDepartmentID"] != null)
                        crReportDocument.SetParameterValue("DepartmentID", ViewState["ddlDepartmentID"].ToString());
                        if (crReportDocument.ParameterFields["@SectionID"] != null && ViewState["ddlSectionID"] != null)
                            crReportDocument.SetParameterValue("@SectionID", GMSUtil.ToInt(ViewState["ddlSectionID"].ToString()));
                        if (crReportDocument.ParameterFields["SectionID"] != null && ViewState["ddlSectionID"] != null)
                            crReportDocument.SetParameterValue("SectionID", ViewState["ddlSectionID"].ToString());
                        if (crReportDocument.ParameterFields["@UnitID"] != null && ViewState["ddlUnitID"] != null)
                            crReportDocument.SetParameterValue("@UnitID", GMSUtil.ToInt(ViewState["ddlUnitID"].ToString()));
                        if (crReportDocument.ParameterFields["UnitID"] != null && ViewState["ddlUnitID"] != null)
                            crReportDocument.SetParameterValue("UnitID", ViewState["ddlUnitID"].ToString());

                    if (crReportDocument.ParameterFields["@CoyID"] != null)
                            crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
                        if (crReportDocument.ParameterFields["CoyID"] != null)
                            crReportDocument.SetParameterValue("CoyID", session.CompanyId);
                        if (crReportDocument.ParameterFields["@UserNumID"] != null)
                            crReportDocument.SetParameterValue("@UserNumID", loginUserOrAlternateParty);
                        if (crReportDocument.ParameterFields["@Currency"] != null && ViewState["ddlCurrencyCode"] != null)
                            crReportDocument.SetParameterValue("@Currency", ViewState["ddlCurrencyCode"].ToString());
                        if (crReportDocument.ParameterFields["@Currency"] != null && session.DefaultCurrency == "SGD")
                            crReportDocument.SetParameterValue("@Currency", "SGD");
                        if (crReportDocument.ParameterFields["@Type"] != null && ViewState["ddlType"] != null)
                            crReportDocument.SetParameterValue("@Type", ViewState["ddlType"].ToString());
                        if (crReportDocument.ParameterFields["Type"] != null && ViewState["ddlType"] != null)
                            crReportDocument.SetParameterValue("Type", ViewState["ddlType"].ToString());
                        if (crReportDocument.ParameterFields["@SalesPersonType"] != null && ViewState["ddlSalesPersonType"] != null)
                            crReportDocument.SetParameterValue("@SalesPersonType", ViewState["ddlSalesPersonType"].ToString());
                        if (crReportDocument.ParameterFields["SalesPersonType"] != null && ViewState["ddlSalesPersonType"] != null)
                            crReportDocument.SetParameterValue("SalesPersonType", ViewState["ddlSalesPersonType"].ToString());
                    

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
                    if (crReportDocument.ParameterFields["@Doctype"] != null && ViewState["ddlDocument"] != null)
                        crReportDocument.SetParameterValue("@Doctype", ViewState["ddlDocument"].ToString());
                    if (crReportDocument.ParameterFields["Doctype"] != null && ViewState["ddlDocument"] != null)
                        crReportDocument.SetParameterValue("Doctype", ViewState["ddlDocument"].ToString());
                    if (crReportDocument.ParameterFields["@TopNumber"] != null && ViewState["ddlTopNumber"] != null)
                        crReportDocument.SetParameterValue("@TopNumber", ViewState["ddlTopNumber"].ToString());
                    if (crReportDocument.ParameterFields["TopNumber"] != null && ViewState["ddlTopNumber"] != null)
                        crReportDocument.SetParameterValue("TopNumber", ViewState["ddlTopNumber"].ToString());
                    if (crReportDocument.ParameterFields["@DeliveryStatus"] != null && ViewState["ddlDeliveryStatus"] != null)
                        crReportDocument.SetParameterValue("@DeliveryStatus", ViewState["ddlDeliveryStatus"].ToString());
                    if (crReportDocument.ParameterFields["DeliveryStatus"] != null && ViewState["ddlDeliveryStatus"] != null)
                        crReportDocument.SetParameterValue("DeliveryStatus", ViewState["ddlDeliveryStatus"].ToString());
                    if (crReportDocument.ParameterFields["@Action"] != null && ViewState["ddlAction"] != null)
                        crReportDocument.SetParameterValue("@Action", ViewState["ddlAction"].ToString());
                    if (crReportDocument.ParameterFields["Action"] != null && ViewState["Action"] != null)
                        crReportDocument.SetParameterValue("Action", ViewState["ddlAction"].ToString());
                    if (crReportDocument.ParameterFields["@Category"] != null && ViewState["ddlCategory"] != null)
                        crReportDocument.SetParameterValue("@Category", ViewState["ddlCategory"].ToString());
                    if (crReportDocument.ParameterFields["Category"] != null && ViewState["ddlCategory"] != null)
                        crReportDocument.SetParameterValue("Category", ViewState["ddlCategory"].ToString());
                    if (crReportDocument.ParameterFields["@ProductCategory"] != null && ViewState["ddlProductCategory"] != null)
                        crReportDocument.SetParameterValue("@ProductCategory", ViewState["ddlProductCategory"].ToString());
                    if (crReportDocument.ParameterFields["ProductCategory"] != null && ViewState["ddlProductCategory"] != null)
                        crReportDocument.SetParameterValue("ProductCategory", ViewState["ddlProductCategory"].ToString());
                    if (crReportDocument.ParameterFields["@BrandCategory"] != null && ViewState["ddlBrandCategory"] != null)
                        crReportDocument.SetParameterValue("@BrandCategory", ViewState["ddlBrandCategory"].ToString());
                    if (crReportDocument.ParameterFields["BrandCategory"] != null && ViewState["ddlBrandCategory"] != null)
                        crReportDocument.SetParameterValue("BrandCategory", ViewState["ddlCategory"].ToString());
                    if (crReportDocument.ParameterFields["@Zcode"] != null && ViewState["ddlZcode"] != null)
                        crReportDocument.SetParameterValue("@Zcode", ViewState["ddlZcode"].ToString());
                    if (crReportDocument.ParameterFields["@EC"] != null && ViewState["ddlEC"] != null)
                        crReportDocument.SetParameterValue("@EC", ViewState["ddlEC"].ToString());

                    if (crReportDocument.ParameterFields["@GroupBy"] != null && ViewState["ddlGroupBy"] != null)
                        crReportDocument.SetParameterValue("@GroupBy", ViewState["ddlGroupBy"].ToString());
                    if (crReportDocument.ParameterFields["GroupBy"] != null && ViewState["ddlGroupBy"] != null)
                        crReportDocument.SetParameterValue("GroupBy", ViewState["ddlGroupBy"].ToString());
                    if (crReportDocument.ParameterFields["Grouping"] != null && ViewState["ddlGrouping"] != null)
                        crReportDocument.SetParameterValue("Grouping", ViewState["ddlGrouping"].ToString());

                    if (crReportDocument.ParameterFields["@SalesPersonType"] != null && ViewState["ddlSalesPersonType"] != null)
                        crReportDocument.SetParameterValue("@SalesPersonType", ViewState["ddlSalesPersonType"].ToString());
                    if (crReportDocument.ParameterFields["SalesPersonType"] != null && ViewState["ddlSalesPersonType"] != null)
                        crReportDocument.SetParameterValue("SalesPersonType", ViewState["ddlSalesPersonType"].ToString());

                    if (crReportDocument.ParameterFields["@SalesPersonID"] != null && ViewState["ddlSalesPersonID"] != null)
                        crReportDocument.SetParameterValue("@SalesPersonID", ViewState["ddlSalesPersonID"].ToString());

                    if (crReportDocument.ParameterFields["@Department"] != null && ViewState["ddlDepartment"] != null)
                        crReportDocument.SetParameterValue("@Department", ViewState["ddlDepartment"].ToString());
            
                    if (crReportDocument.ParameterFields["@SalesType"] != null && ViewState["ddlSalesType"] != null)
                        crReportDocument.SetParameterValue("@SalesType", ViewState["ddlSalesType"].ToString());
                   

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

                    if (crReportDocument.ParameterFields["@ProductGroupCode"] != null && ViewState["txtBrandCode"] != null)
                        crReportDocument.SetParameterValue("@ProductGroupCode", ViewState["txtBrandCode"].ToString());

                    if (crReportDocument.ParameterFields["BrandCode"] != null && ViewState["txtBrandCode"] != null)
                        crReportDocument.SetParameterValue("BrandCode", ViewState["txtBrandCode"].ToString());

                    if (crReportDocument.ParameterFields["Brand"] != null && ViewState["txtBrand"] != null)
                        crReportDocument.SetParameterValue("Brand", ViewState["txtBrand"].ToString());

                    if (crReportDocument.ParameterFields["Product Group"] != null && ViewState["txtBrand"] != null)
                        crReportDocument.SetParameterValue("Product Group", ViewState["txtBrand"].ToString());

                    if (crReportDocument.ParameterFields["@ProductGroupName"] != null && ViewState["txtBrand"] != null)
                        crReportDocument.SetParameterValue("@ProductGroupName", ViewState["txtBrand"].ToString());

                    if (crReportDocument.ParameterFields["@ProductCode"] != null && ViewState["txtProductCode"] != null)
                        crReportDocument.SetParameterValue("@ProductCode", ViewState["txtProductCode"].ToString());

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

                    if (crReportDocument.ParameterFields["@TrnDateType"] != null && ViewState["ddlTrnDateType"] != null)
                        crReportDocument.SetParameterValue("@TrnDateType", ViewState["ddlTrnDateType"].ToString());

                    if (crReportDocument.ParameterFields["@FilterBy"] != null && ViewState["ddlFilterBy"] != null)
                        crReportDocument.SetParameterValue("@FilterBy", ViewState["ddlFilterBy"].ToString());

                    if (crReportDocument.ParameterFields["@DivisionType"] != null && ViewState["ddlDivision"] != null)
                        crReportDocument.SetParameterValue("@DivisionType", ViewState["ddlDivision"].ToString());

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
                    
                    if (crReportDocument.ParameterFields["@GroupID"] != null && ViewState["ddlGroup"] != null )
                        crReportDocument.SetParameterValue("@GroupID", GMSUtil.ToInt(ViewState["ddlGroup"].ToString()));
                    
                    if (crReportDocument.ParameterFields["@TeamID"] != null && ViewState["ddlTeam"] != null)
                        crReportDocument.SetParameterValue("@TeamID", GMSUtil.ToInt(ViewState["ddlTeam"].ToString()));

                    if (crReportDocument.ParameterFields["ProjectID"] != null && ViewState["ddlProjectID"] != null)
                        crReportDocument.SetParameterValue("ProjectID", GMSUtil.ToInt(ViewState["ddlProjectID"].ToString()));
                    if (crReportDocument.ParameterFields["DepartmentID"] != null && ViewState["ddlDepartmentID"] != null)
                        crReportDocument.SetParameterValue("DepartmentID", GMSUtil.ToInt(ViewState["ddlDepartmentID"].ToString()));
                    if (crReportDocument.ParameterFields["SectionID"] != null && ViewState["ddlSectionID"] != null)
                        crReportDocument.SetParameterValue("SectionID", GMSUtil.ToInt(ViewState["ddlSectionID"].ToString()));
                    if (crReportDocument.ParameterFields["UnitID"] != null && ViewState["ddlUnitID"] != null)
                        crReportDocument.SetParameterValue("UnitID", GMSUtil.ToInt(ViewState["ddlUnitID"].ToString()));

                    if (crReportDocument.ParameterFields["Requestor"] != null && ViewState["txtRequestor"] != null)
                        crReportDocument.SetParameterValue("Requestor", ViewState["txtRequestor"].ToString());
                    if (crReportDocument.ParameterFields["RefNo"] != null && ViewState["txtRefNo"] != null)
                        crReportDocument.SetParameterValue("RefNo", ViewState["txtRefNo"].ToString());
                    if (crReportDocument.ParameterFields["ProjNo"] != null && ViewState["txtProjNo"] != null)
                        crReportDocument.SetParameterValue("ProjNo", ViewState["txtProjNo"].ToString());
                    if (crReportDocument.ParameterFields["BudgetCode"] != null && ViewState["txtBudgetCode"] != null)
                        crReportDocument.SetParameterValue("BudgetCode", ViewState["txtBudgetCode"].ToString());
                    if (crReportDocument.ParameterFields["MRStatus"] != null && ViewState["txtMRStatus"] != null)
                        crReportDocument.SetParameterValue("MRStatus", ViewState["txtMRStatus"].ToString());

                    if (crReportDocument.ParameterFields["@TransactionType"] != null && ViewState["ddlTT"] != null)
                        crReportDocument.SetParameterValue("@TransactionType", ViewState["ddlTT"].ToString());

                    if (crReportDocument.ParameterFields["@Effectivedate"] != null && ViewState["txtEffectiveDate"] != null)
                        crReportDocument.SetParameterValue("@Effectivedate", GMSUtil.ToDate(ViewState["txtEffectiveDate"]));

                    if (crReportDocument.ParameterFields["@ReviewDate"] != null && ViewState["txtReviewDate"] != null)
                        crReportDocument.SetParameterValue("@ReviewDate", GMSUtil.ToDate(ViewState["txtReviewDate"]));

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