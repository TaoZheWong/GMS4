using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace GMSWeb.Sales.Sales
{
    public partial class SalespersonTeamSetup : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["authkey"] != null)//for access from GMS3 
            {
                base.loginByAuthKey(Request.Params["authkey"].ToString());
            }
            string currentLink = "Sales";
            lblPageHeader.Text = "Sales";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                if (Request.Params["CurrentLink"].ToString().Trim() == "Sales")
                    lblPageHeader.Text = "Sales";
                else
                    lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);

            LogSession session = base.GetSessionInfo();

            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            173);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            173);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!IsPostBack)
            {
                this.RadGrid1.CurrentPageIndex = 0;
                RetrieveSalespersonWithDim();
                this.RadGrid1.DataBind();
            }
        }


        #region Retrieve Data
        private void RetrieveSalespersonWithDim()
        {
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();

            resultList.Visible = true;

            try
            {
                ggdal.RetrieveSalespersonWithDim(session.CompanyId, session.UserId, ref ds);
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString());
            }

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                this.lblSearchSummary.Visible = false;
                this.RadGrid1.DataSource = ds.Tables[0];
            }
            else
            {
                this.lblSearchSummary.Text = "No records.";
                this.lblSearchSummary.Visible = true;
                this.RadGrid1.DataSource = null;
            }
        }
        #endregion

        #region Radgrid Control
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RetrieveSalespersonWithDim();
        }

        protected void radGrid_OnPageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            RetrieveSalespersonWithDim();
        }

        protected void radGrid_OnPageSizeChanged(object source, GridPageSizeChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageSize;
            RetrieveSalespersonWithDim();
        }

        protected void radGrid_OnCancel(object source, GridCommandEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.MasterTableView.ClearEditItems();
            RetrieveSalespersonWithDim();
        }

        protected void RadGrid1_OnUpdateCommand(object source, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.Item is GridEditableItem && e.Item.IsInEditMode && !(e.Item is IGridInsertItem))
            {
                try
                {
                    GridEditableItem editableItem = e.Item as GridEditableItem;
                    string salespersonID =editableItem.GetDataKeyValue("SalespersonID").ToString();
                    DropDownList ddlDim1 = editableItem.FindControl("ddlDim1") as DropDownList;
                    short dim1 = -1;
                    if (!string.IsNullOrEmpty(ddlDim1.SelectedValue))
                        dim1 = short.Parse(ddlDim1.SelectedValue);
                    DropDownList ddlDim2 = editableItem.FindControl("ddlDim2") as DropDownList;
                    short dim2 = -1;
                    if (!string.IsNullOrEmpty(ddlDim2.SelectedValue))
                        dim2 = short.Parse(ddlDim2.SelectedValue);
                    DropDownList ddlDim3 = editableItem.FindControl("ddlDim3") as DropDownList;
                    short dim3 = -1;
                    if (!string.IsNullOrEmpty(ddlDim3.SelectedValue))
                        dim3 = short.Parse(ddlDim3.SelectedValue);
                    DropDownList ddlDim4 = editableItem.FindControl("ddlDim4") as DropDownList;
                    short dim4 = -1;
                    if (!string.IsNullOrEmpty(ddlDim4.SelectedValue))
                        dim4 = short.Parse(ddlDim4.SelectedValue);
                    TextBox txtShortname = editableItem["Shortname"].Controls[0] as TextBox;
                    string Shortname = txtShortname.Text;
                    CheckBox isBudget = editableItem["IsBudget"].Controls[0] as CheckBox;
                    SalesPerson sp = SalesPerson.RetrieveByKey(session.CompanyId,salespersonID);

                    if (sp != null)
                    {
                        sp.Dim1 = dim1;
                        sp.Dim2 = dim2;
                        sp.Dim3 = dim3;
                        sp.Dim4 = dim4;
                        sp.ShortName = Shortname;
                        sp.IsBudget = isBudget.Checked;
                        sp.Save();
                        alertMessage("Salesperson Updated.");
                    }
                    else
                    {
                        alertMessage("Salesperson not exists");
                        e.Canceled = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    alertMessage(ex.ToString());
                    e.Canceled = true;
                    return;
                }
            }
        }

        protected void RadGrid1_OnItemCreated(object sender, GridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();

            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem editableItem = e.Item as GridEditableItem;
                short year = short.Parse(DateTime.Now.Year.ToString());
                short month = short.Parse(DateTime.Now.Month.ToString());
                DropDownList ddlDim1 = editableItem.FindControl("ddlDim1") as DropDownList;
                DropDownList ddlDim2 = editableItem.FindControl("ddlDim2") as DropDownList;
                DropDownList ddlDim3 = editableItem.FindControl("ddlDim3") as DropDownList;
                DropDownList ddlDim4 = editableItem.FindControl("ddlDim4") as DropDownList;
                DataSet dsProjects = new DataSet();
                ggdal.GetCompanyProject(session.CompanyId, ref dsProjects);
                for (int j = 0; j < dsProjects.Tables[0].Rows.Count; j++)
                {
                    ddlDim1.Items.Add(new ListItem(dsProjects.Tables[0].Rows[j]["ProjectName"].ToString(), dsProjects.Tables[0].Rows[j]["ProjectID"].ToString()));
                }
            }
        }

        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
            {
                foreach (GridColumn column in RadGrid1.MasterTableView.Columns)
                {
                    column.ListOfFilterValues = null; // CheckList values set to null will uncheck all the checkboxes

                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;

                    column.AndCurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.AndCurrentFilterValue = string.Empty;
                }
                RadGrid1.MasterTableView.FilterExpression = string.Empty;
                RadGrid1.MasterTableView.Rebind();
            }
        }

        protected void RadGrid1_FilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
        {
            string DataField = (e.Column as IGridDataColumn).GetActiveDataField();
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            try
            {
                ggdal.RetrieveSalespersonWithDim(session.CompanyId, session.UserId, ref ds);
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString());
            }
            DataView dvOri = new DataView(ds.Tables[0]);
            DataTable dtOri = dvOri.ToTable(true, DataField);
            e.ListBox.DataSource = dtOri;
            e.ListBox.DataKeyField = DataField;
            e.ListBox.DataTextField = DataField;
            e.ListBox.DataValueField = DataField;
            e.ListBox.DataBind();
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            short year = Convert.ToInt16(DateTime.Now.Year);
            short month = Convert.ToInt16(DateTime.Now.Month);

            if (e.Item is GridEditableItem && e.Item.IsInEditMode && !(e.Item is IGridInsertItem))
            {
                GridEditableItem editableItem = e.Item as GridEditableItem;
                DropDownList ddlDim1 = editableItem.FindControl("ddlDim1") as DropDownList;
                DropDownList ddlDim2 = editableItem.FindControl("ddlDim2") as DropDownList;
                DropDownList ddlDim3 = editableItem.FindControl("ddlDim3") as DropDownList;
                DropDownList ddlDim4 = editableItem.FindControl("ddlDim4") as DropDownList;

                HtmlInputHidden hidDim1 = (HtmlInputHidden)editableItem.FindControl("hidDim1");
                HtmlInputHidden hidDim2 = (HtmlInputHidden)editableItem.FindControl("hidDim2");
                HtmlInputHidden hidDim3 = (HtmlInputHidden)editableItem.FindControl("hidDim3");
                HtmlInputHidden hidDim4 = (HtmlInputHidden)editableItem.FindControl("hidDim4");
                if (!string.IsNullOrEmpty(hidDim1.Value) && !string.IsNullOrEmpty(hidDim2.Value) &&
                    !string.IsNullOrEmpty(hidDim3.Value) && !string.IsNullOrEmpty(hidDim4.Value))
                {
                    ddlDim1.SelectedValue = hidDim1.Value;

                    DataSet dsDepartments = new DataSet();
                    ggdal.GetCompanyDepartment(session.CompanyId, Convert.ToInt16(ddlDim1.SelectedValue), 0, session.UserId, year, month, ref dsDepartments);
                    foreach (DataRow dr in dsDepartments.Tables[0].Rows)
                    {
                        ddlDim2.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
                    }
                    ddlDim2.SelectedValue = hidDim2.Value;

                    DataSet dsSections = new DataSet();
                    ggdal.GetCompanySection(session.CompanyId, Convert.ToInt16(ddlDim2.SelectedValue), 0, session.UserId, year, month, ref dsSections);
                    foreach (DataRow dr in dsSections.Tables[0].Rows)
                    {
                        ddlDim3.Items.Add(new ListItem(dr["SectionName"].ToString(), dr["SectionID"].ToString()));
                    }
                    ddlDim3.SelectedValue = hidDim3.Value;

                    DataSet dsUnits = new DataSet();
                    ggdal.GetCompanyUnit(session.CompanyId, Convert.ToInt16(ddlDim3.SelectedValue), ref dsUnits);
                    foreach (DataRow dr in dsUnits.Tables[0].Rows)
                    {
                        ddlDim4.Items.Add(new ListItem(dr["UnitName"].ToString(), dr["UnitID"].ToString()));
                    }
                    ddlDim4.SelectedValue = hidDim4.Value;
                }
            }
        }
        #endregion

        #region drop down list index changed
        protected void ddlProjectID_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim1 = (DropDownList)sender;
            DropDownList ddlDim2 = null;
            DropDownList ddlDim3 = null;
            DropDownList ddlDim4 = null;
            short selectedvalue = Convert.ToInt16(ddlDim1.SelectedValue);

            GridEditFormItem item = (GridEditFormItem)ddlDim1.NamingContainer;
            ddlDim2 = (DropDownList)item.FindControl("ddlDim2");
            ddlDim3 = (DropDownList)item.FindControl("ddlDim3");
            ddlDim4 = (DropDownList)item.FindControl("ddlDim4");

            ddlDim2.Items.Clear();
            ddlDim3.Items.Clear();
            ddlDim4.Items.Clear();

            if (selectedvalue > 0)
            {
                ddlDim2.Enabled = true;
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DataSet dsDepartments = new DataSet();

                short year = Convert.ToInt16(DateTime.Now.Year);
                short month = Convert.ToInt16(DateTime.Now.Month);

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
            ddlDim3.Enabled = false;
            ddlDim4.Enabled = false;
        }

        protected void ddlDepartmentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim2 = (DropDownList)sender;
            DropDownList ddlDim3 = null;
            DropDownList ddlDim4 = null;
            short selectedvalue = Convert.ToInt16(ddlDim2.SelectedValue);

            GridEditFormItem item = (GridEditFormItem)ddlDim2.NamingContainer;
            ddlDim3 = (DropDownList)item.FindControl("ddlDim3");
            ddlDim4 = (DropDownList)item.FindControl("ddlDim4");

            ddlDim3.Items.Clear();
            ddlDim4.Items.Clear();

            if (selectedvalue > 0)
            {
                ddlDim3.Enabled = true;
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DataSet dsSections = new DataSet();

                short year = Convert.ToInt16(DateTime.Now.Year);
                short month = Convert.ToInt16(DateTime.Now.Month);
                short reportId = 0;
                short loginUserOrAlternateParty = 0;

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
            ddlDim4.Enabled = false;
        }

        protected void ddlSectionID_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim3 = (DropDownList)sender;
            DropDownList ddlDim4 = null;
            short selectedvalue = Convert.ToInt16(ddlDim3.SelectedValue);

            GridEditFormItem item = (GridEditFormItem)ddlDim3.NamingContainer;
            ddlDim4 = (DropDownList)item.FindControl("ddlDim4");

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
        }
        #endregion

        protected void alertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        }
    }
}