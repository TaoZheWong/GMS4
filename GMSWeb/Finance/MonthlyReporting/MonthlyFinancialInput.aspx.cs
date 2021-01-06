using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace GMSWeb.Finance.MonthlyReporting
{
    public partial class MonthlyFinancialPerformance : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["authkey"] != null)//for access from GMS3 
            {
                base.loginByAuthKey(Request.Params["authkey"].ToString());
            }
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Products"));
                return;
            }

            string currentLink = "CompanyFinance";
            lblPageHeader.Text = "Finance";

            if (Request.Params["CurrentLink"] != null)
                currentLink = Request.Params["CurrentLink"].ToString().Trim();

            Master.setCurrentLink(currentLink);

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            182);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            182);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!IsPostBack)
            {
                LoadControl();
                LoadMainData();
            }
        }

        protected void LoadControl()
        {
            LogSession session = base.GetSessionInfo();
            //ddlyear
            var startYear = int.Parse(DateTime.Now.AddYears(-10).ToString("yyyy"));
            var rangeCount = 10;
            var ranges = Enumerable.Range(startYear, rangeCount).Select(y => (y + 1)).OrderByDescending(y => (y + 1));
            this.ddlYear.DataSource = ranges;
            this.ddlYear.DataBind();

            //ddlMonth
            for (int i = 1; i <= 12; i++)
            {
                this.ddlMonth.Items.Add(i.ToString());
            }
            this.ddlMonth.SelectedValue = DateTime.Now.AddMonths(-1).Month.ToString();

            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet dsProjects = new DataSet();
            //same dimension restriction as f21 report
            ggdal.GetCompanyProject(session.CompanyId, session.UserId, 426, ref dsProjects);

            for (int j = 0; j < dsProjects.Tables[0].Rows.Count; j++)
            {
                this.ddlSearchDim1.Items.Add(new ListItem(dsProjects.Tables[0].Rows[j]["ProjectName"].ToString(), dsProjects.Tables[0].Rows[j]["ReportProjectID"].ToString()));
                if (Convert.ToInt16(ddlSearchDim1.SelectedValue) > 0)
                {
                    DataSet dsDepartments = new DataSet();
                    ggdal.GetCompanyDepartment(session.CompanyId, Convert.ToInt16(ddlSearchDim1.SelectedValue), 0, session.UserId, short.Parse(this.ddlYear.SelectedValue), short.Parse(this.ddlMonth.SelectedValue), ref dsDepartments);
                    foreach (DataRow dr in dsDepartments.Tables[0].Rows)
                    {
                        ddlSearchDim2.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
                    }
                    ddlSearchDim2.Enabled = true;
                    //Bind Section if Department > 0
                    if (Convert.ToInt16(ddlSearchDim2.SelectedValue) > 0)
                    {
                        DataSet dsSections = new DataSet();
                        ggdal.GetCompanySection(session.CompanyId, Convert.ToInt16(ddlSearchDim2.SelectedValue), 0, session.UserId, short.Parse(this.ddlYear.SelectedValue), short.Parse(this.ddlMonth.SelectedValue), ref dsSections);
                        foreach (DataRow dr in dsSections.Tables[0].Rows)
                        {
                            ddlSearchDim3.Items.Add(new ListItem(dr["SectionName"].ToString(), dr["SectionID"].ToString()));
                        }
                        ddlSearchDim3.Enabled = true;
                    }
                }
            }
        }

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.RadGrid1.CurrentPageIndex = 0;
            this.RadGrid2.CurrentPageIndex = 0;
            this.RadGrid3.CurrentPageIndex = 0;
            this.RadGrid4.CurrentPageIndex = 0;
            LoadMainData();
            this.RadGrid1.DataBind();
            this.RadGrid2.DataBind();
            this.RadGrid3.DataBind();
            this.RadGrid4.DataBind();

        }
        #endregion

        protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            LoadMainData();
        }

        protected void LoadMainData()
        {
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();
            DataSet ds4 = new DataSet();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            short year = short.Parse(this.ddlYear.SelectedValue);
            short month = short.Parse(this.ddlMonth.SelectedValue);
            short dim1 = short.Parse(this.ddlSearchDim1.SelectedValue);
            short dim2 = -1;
            if (!string.IsNullOrEmpty(this.ddlSearchDim2.SelectedValue))
                dim2 = short.Parse(this.ddlSearchDim2.SelectedValue);
            short dim3 = -1;
            if (!string.IsNullOrEmpty(this.ddlSearchDim3.SelectedValue))
                dim3 = short.Parse(this.ddlSearchDim3.SelectedValue);
            short dim4 = -1;
            if (!string.IsNullOrEmpty(this.ddlSearchDim4.SelectedValue))
                dim4 = short.Parse(this.ddlSearchDim4.SelectedValue);

            ggdal.SelectMonthlyPerformance(session.CompanyId, year, month, session.UserId, "F18C", dim1, dim2, dim3, dim4, ref ds);
            RadGrid1.DataSource = ds;

            ggdal.SelectMonthlyPerformance(session.CompanyId, year, month, session.UserId, "F18Y", dim1, dim2, dim3, dim4, ref ds2);
            RadGrid2.DataSource = ds2;

            ggdal.SelectMonthlyPerformance(session.CompanyId, year, month, session.UserId, "F19C", dim1, dim2, dim3, dim4, ref ds3);
            RadGrid3.DataSource = ds3;

            ggdal.SelectMonthlyPerformance(session.CompanyId, year, month, session.UserId, "F19Y", dim1, dim2, dim3, dim4, ref ds4);
            RadGrid4.DataSource = ds4;

            this.RadMultiPage1.Visible = true;

        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.CommandName == "Save")
            {
                GridDataItem item = (GridDataItem)e.Item;
                short ID = 0;
                if (!string.IsNullOrEmpty(item.GetDataKeyValue("ID").ToString()))
                    ID = short.Parse(item.GetDataKeyValue("ID").ToString());
                RadTextBox txtReason = (RadTextBox)item.FindControl("txtReason");
                string reason = "-";
                if (!string.IsNullOrEmpty(txtReason.Text))
                    reason = txtReason.Text;
                MonthlyPerformance mp = MonthlyPerformance.RetrieveByID(session.CompanyId, ID);
                if (mp != null)
                {
                    mp.Reason = reason;
                    mp.UpdatedBy = session.UserId;
                    mp.UpdatedDate = DateTime.Now;
                    try
                    {
                        mp.Save();
                        alertMessage("Reason Saved.");
                        this.RadGrid1.Rebind();
                        this.RadGrid2.Rebind();
                    }
                    catch (Exception ex)
                    {
                        alertMessage(ex.ToString());
                    }
                }
            }
        }

        protected void RadGridForecast_ItemCommand(object sender, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.CommandName == "Save")
            {
                GridDataItem item = (GridDataItem)e.Item;
                short ID = 0;
                if (!string.IsNullOrEmpty(item.GetDataKeyValue("ID").ToString()))
                    ID = short.Parse(item.GetDataKeyValue("ID").ToString());
                RadTextBox txtActual = (RadTextBox)item.FindControl("txtActual");
                int actual = 0;
                if (!string.IsNullOrEmpty(txtActual.Text))
                    actual = int.Parse(txtActual.Text);
                RadTextBox txtReason = (RadTextBox)item.FindControl("txtReason");
                string reason = "-";
                if (!string.IsNullOrEmpty(txtReason.Text))
                    reason = txtReason.Text;
                MonthlyPerformance mp = MonthlyPerformance.RetrieveByID(session.CompanyId, ID);
                if (mp != null)
                {
                    mp.Actual = actual;
                    mp.Reason = reason;
                    mp.UpdatedBy = session.UserId;
                    mp.UpdatedDate = DateTime.Now;
                    try
                    {
                        mp.Save();
                        alertMessage("Actual & Reason Saved.");
                        this.RadGrid3.Rebind();
                        this.RadGrid4.Rebind();
                    }
                    catch (Exception ex)
                    {
                        alertMessage(ex.ToString());
                    }
                }
            }
        }

        protected void RadGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                string itemname = "";
                HtmlInputHidden hidItemName = (HtmlInputHidden)item.FindControl("hidItemName");
                RadTextBox txtActual = (RadTextBox)item.FindControl("txtActual");
                if (!string.IsNullOrEmpty(hidItemName.Value))
                    itemname = hidItemName.Value;

                if (itemname == "Profit from Operations")
                    txtActual.ReadOnly = true;
            }
        }

        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            bool canEdit = true;
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            169);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            169);

            //if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
            //    canEdit = false;

            if (RadGrid1.MasterTableView.HasDetailTables)
            {
                foreach (GridDataItem item in RadGrid1.MasterTableView.Items)
                {
                    HtmlInputHidden hidMethod = item.FindControl("hidMethod") as HtmlInputHidden;
                    string method = hidMethod.Value;
                    GridColumn DeleteColumn = RadGrid1.MasterTableView.GetColumn("DeleteColumn1");
                    if (!canEdit)
                    {
                        DeleteColumn.Visible = false;
                        RadGrid1.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                    }
                    if (item.HasChildItems)
                    {
                        foreach (GridTableView innerDetailView in item.ChildItem.NestedTableViews)
                        {
                            GridColumn Percentage = innerDetailView.GetColumn("Percentage");
                            GridColumn UOM = innerDetailView.GetColumn("UOM");
                            GridColumn Quantity = innerDetailView.GetColumn("Quantity");
                            GridColumn UnitCost = innerDetailView.GetColumn("UnitCost");
                            GridColumn AllocatedCost = innerDetailView.GetColumn("AllocatedCost");
                            GridColumn EditCommandColumn1 = innerDetailView.GetColumn("EditCommandColumn1");
                            GridColumn DeleteColumn1 = innerDetailView.GetColumn("DeleteColumn1");
                            if (!canEdit)
                            {
                                innerDetailView.CommandItemSettings.ShowAddNewRecordButton = false;
                                EditCommandColumn1.Visible = false;
                                DeleteColumn1.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        #region dimension drop down list selected index changed event in search 
        protected void ddlYearMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DropDownList ddlProjectID = this.ddlSearchDim1;
                DropDownList ddlDepartmentID = this.ddlSearchDim2;
                DropDownList ddlSectionID = this.ddlSearchDim3;
                DropDownList ddlUnitID = this.ddlSearchDim4;

                ddlProjectID.Items.Clear();
                ddlDepartmentID.Items.Clear();
                ddlSectionID.Items.Clear();
                ddlUnitID.Items.Clear();

                ddlDepartmentID.Enabled = false;
                ddlSectionID.Enabled = false;
                ddlUnitID.Enabled = false;
                DropDownList ddlYear = this.ddlYear;
                DropDownList ddlMonth = this.ddlMonth;
                short year = Convert.ToInt16(this.ddlYear.SelectedValue);
                short month = 0;
                try
                {
                    month = Convert.ToInt16(this.ddlMonth.SelectedValue);
                }
                catch (Exception ex)
                {
                    month = 4;
                }
                DataSet dsProjects = new DataSet();
                dacl.GetCompanyProject(session.CompanyId, session.UserId, 426, ref dsProjects);
                if (dsProjects.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsProjects.Tables[0].Rows.Count; j++)
                    {
                        ddlProjectID.Items.Add(new ListItem(dsProjects.Tables[0].Rows[j]["ProjectName"].ToString(), dsProjects.Tables[0].Rows[j]["ReportProjectID"].ToString()));
                    }
                    if (Convert.ToInt16(ddlProjectID.SelectedValue) > 0)
                    {
                        DataSet dsDepartments = new DataSet();
                        dacl.GetCompanyDepartment(session.CompanyId, Convert.ToInt16(ddlProjectID.SelectedValue), 0, session.UserId, year, month, ref dsDepartments);
                        foreach (DataRow dr in dsDepartments.Tables[0].Rows)
                        {
                            ddlDepartmentID.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
                        }
                        ddlDepartmentID.Enabled = true;
                        //Bind Section if Department > 0
                        if (Convert.ToInt16(ddlDepartmentID.SelectedValue) > 0)
                        {
                            DataSet dsSections = new DataSet();
                            dacl.GetCompanySection(session.CompanyId, Convert.ToInt16(ddlDepartmentID.SelectedValue), 0, session.UserId, year, month, ref dsSections);
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

            this.RadMultiPage1.Visible = false;
        }

        protected void ddlDim1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim1 = (DropDownList)sender;
            DropDownList ddlDim2 = this.ddlSearchDim2;
            DropDownList ddlDim3 = this.ddlSearchDim3;
            DropDownList ddlDim4 = this.ddlSearchDim4;
            short selectedvalue = Convert.ToInt16(ddlDim1.SelectedValue);

            ddlDim2.Items.Clear();
            ddlDim3.Items.Clear();
            ddlDim4.Items.Clear();

            if (selectedvalue > 0)
            {
                ddlDim2.Enabled = true;
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DataSet dsDepartments = new DataSet();
                short year = Convert.ToInt16(this.ddlYear.SelectedValue);
                short month = 0;

                try
                {
                    month = Convert.ToInt16(this.ddlMonth.SelectedValue);
                }
                catch (Exception ex)
                {
                    month = Convert.ToInt16(DateTime.Now.Month);
                }

                dacl.GetCompanyDepartment(session.CompanyId, selectedvalue, 0, session.UserId, year, month, ref dsDepartments);
                foreach (DataRow dr in dsDepartments.Tables[0].Rows)
                {
                    ddlDim2.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
                }
            }
            else
            {
                ddlDim2.Enabled = false;
            }
            this.RadMultiPage1.Visible = false;
            ddlDim3.Enabled = false;
            ddlDim4.Enabled = false;
        }

        protected void ddlDim2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim2 = (DropDownList)sender;
            DropDownList ddlDim3 = this.ddlSearchDim3;
            DropDownList ddlDim4 = this.ddlSearchDim4;
            short selectedvalue = Convert.ToInt16(ddlDim2.SelectedValue);
            ddlDim3.Items.Clear();
            ddlDim4.Items.Clear();

            if (selectedvalue > 0)
            {
                ddlDim3.Enabled = true;
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DataSet dsSections = new DataSet();

                short year = Convert.ToInt16(this.ddlYear.SelectedValue);
                short month = 0;
                short reportId = 0;
                short loginUserOrAlternateParty = 0;

                try
                {
                    month = Convert.ToInt16(this.ddlMonth.SelectedValue);
                }
                catch (Exception ex)
                {
                    month = Convert.ToInt16(DateTime.Now.Month);
                }

                dacl.GetCompanySection(session.CompanyId, selectedvalue, reportId, loginUserOrAlternateParty, year, month, ref dsSections);
                foreach (DataRow dr in dsSections.Tables[0].Rows)
                {
                    ddlDim3.Items.Add(new ListItem(dr["SectionName"].ToString(), dr["SectionID"].ToString()));
                }
            }
            else
            {
                ddlDim3.Enabled = false;
            }
            this.RadMultiPage1.Visible = false;
            ddlDim4.Enabled = false;
        }

        protected void ddlDim3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim3 = (DropDownList)sender;
            DropDownList ddlDim4 = this.ddlSearchDim4;
            short selectedvalue = Convert.ToInt16(ddlDim3.SelectedValue);
            ddlDim4.Items.Clear();
            if (selectedvalue > 0)
            {
                ddlDim4.Enabled = true;
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DataSet dsUnits = new DataSet();

                dacl.GetCompanyUnit(session.CompanyId, selectedvalue, ref dsUnits);
                foreach (DataRow dr in dsUnits.Tables[0].Rows)
                {
                    ddlDim4.Items.Add(new ListItem(dr["UnitName"].ToString(), dr["UnitID"].ToString()));
                }

            }
            else
            {
                ddlDim4.Enabled = false;
            }
            this.RadMultiPage1.Visible = false;
        }

        #endregion

        protected void alertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        }
    }
}