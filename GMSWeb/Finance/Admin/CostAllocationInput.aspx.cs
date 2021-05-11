using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace GMSWeb.Finance.Admin
{
    public partial class CostAllocationInput : GMSBasePage
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
                                                                            168);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            168);

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
            }
        }

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.RadGrid1.CurrentPageIndex = 0;
            LoadMainData();
            this.RadGrid1.DataBind();
        }
        #endregion

        protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            if (!e.IsFromDetailTable)
            {
                LoadMainData();
            }
        }

        protected void LoadMainData()
        {
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
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

            ggdal.SelectCostAllocation(session.CompanyId, year, month, dim1, dim2, dim3, dim4, ref ds);
            RadGrid1.DataSource = ds;
        }

        protected void RadGrid1_OnPageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadMainData();
        }

        protected void RadGrid1_OnPageSizeChanged(object source, GridPageSizeChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageSize;
            LoadMainData();
        }

        protected void RadGrid1_DetailTableDataBind(object source, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            DataSet ds = new DataSet();
            LogSession session = base.GetSessionInfo();
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
            ggdal.SelectCostAllocationDetailItem(session.CompanyId, year, month, dim1, dim2, dim3, dim4, short.Parse(dataItem.GetDataKeyValue("ID").ToString()), ref ds);
            e.DetailTableView.DataSource = ds;
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                switch (e.Item.OwnerTableView.Name)
                {
                    case "Parent":
                        {
                            GridEditableItem editableItem = e.Item as GridEditableItem;
                            short dim1 = -1;
                            if (!string.IsNullOrEmpty(this.ddlSearchDim1.SelectedValue))
                                dim1 = short.Parse(ddlSearchDim1.SelectedValue);
                            short dim2 = -1;
                            if (!string.IsNullOrEmpty(this.ddlSearchDim2.SelectedValue))
                                dim2 = short.Parse(this.ddlSearchDim2.SelectedValue);
                            short dim3 = -1;
                            if (!string.IsNullOrEmpty(this.ddlSearchDim3.SelectedValue))
                                dim3 = short.Parse(this.ddlSearchDim3.SelectedValue);
                            short dim4 = -1;
                            if (!string.IsNullOrEmpty(this.ddlSearchDim4.SelectedValue))
                                dim4 = short.Parse(this.ddlSearchDim4.SelectedValue);
                            RadAutoCompleteBox txtItemName = (RadAutoCompleteBox)editableItem.FindControl("txtItemName");
                            short itemid = 0;
                            AutoCompleteBoxEntryCollection entries = txtItemName.Entries;
                            if (entries.Count > 0)
                                itemid = short.Parse(entries[0].Value);
                            else
                            {
                                alertMessage("Invalid Item Name");
                                e.Canceled = true;
                                return;
                            }
                            int year = int.Parse(this.ddlYear.SelectedValue);
                            int month = int.Parse(this.ddlMonth.SelectedValue);
                            DropDownList ddlAllocationMethod = (DropDownList)editableItem.FindControl("ddlAllocationMethod");
                            HtmlInputHidden hidTotal = (HtmlInputHidden)editableItem.FindControl("hidTotal");

                            CostAllocationMain cam = CostAllocationMain.RetrieveByYearByDim(session.CompanyId, year, month, dim1, dim2, dim3, dim4, itemid);
                            if (cam != null)
                            {
                                alertMessage("Cost allocation in this dimension already exists.");
                                e.Canceled = true;
                                return;
                            }
                            else
                            {
                                try
                                {
                                    ggdal.InsertCostAllocationParent(session.CompanyId, year, month, dim1, dim2, dim3, dim4,
                                        itemid, ddlAllocationMethod.SelectedValue, session.UserId);
                                    alertMessage("Cost Allocation Main Item Saved.");
                                }
                                catch (Exception ex)
                                {
                                    alertMessage(ex.ToString());
                                    e.Canceled = true;
                                    return;
                                }
                            }
                            break;
                        }
                    case "ChildItem":
                        {
                            try
                            {
                                double total = 0;
                                foreach (GridDataItem item in e.Item.OwnerTableView.Items)
                                {
                                    HtmlInputHidden hidAllocatedCost = (HtmlInputHidden)item.FindControl("hidAllocatedCost");
                                    if (!string.IsNullOrEmpty(hidAllocatedCost.Value))
                                        total = total + double.Parse(hidAllocatedCost.Value);
                                }
                                GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
                                double parentTotal = double.Parse(parentItem["TOTAL"].Text);
                                HtmlInputHidden hidMethod = parentItem.FindControl("hidMethod") as HtmlInputHidden;
                                string method = hidMethod.Value;
                                int parentItemID = int.Parse(parentItem.GetDataKeyValue("ID").ToString());
                                GridEditableItem editableItem = e.Item as GridEditableItem;
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
                                TextBox txtPercent = editableItem.FindControl("txtPercentage") as TextBox;
                                short percent = 0;
                                if (!string.IsNullOrEmpty(txtPercent.Text))
                                    percent = short.Parse(txtPercent.Text);
                                DropDownList ddlUOM = editableItem.FindControl("ddlUOM") as DropDownList;
                                TextBox txtQuantity = editableItem["Quantity"].Controls[0] as TextBox;
                                int quantity = 0;
                                if (!string.IsNullOrEmpty(txtQuantity.Text))
                                    quantity = int.Parse(txtQuantity.Text);
                                TextBox txtUnitCost = editableItem["UnitCost"].Controls[0] as TextBox;
                                int unitCost = 0;
                                if (!string.IsNullOrEmpty(txtUnitCost.Text))
                                    unitCost = int.Parse(txtUnitCost.Text);
                                TextBox txtAllocatedCost = editableItem["AllocatedCost"].Controls[0] as TextBox;
                                double allocatedCost = 0;
                                if (!string.IsNullOrEmpty(txtAllocatedCost.Text))
                                    allocatedCost = double.Parse(txtAllocatedCost.Text);
                                double allocatedCost_ = 0;
                                int year = int.Parse(this.ddlYear.SelectedValue);
                                int month = int.Parse(this.ddlMonth.SelectedValue);
                                RadAutoCompleteBox txtItemName = (RadAutoCompleteBox)editableItem.FindControl("txtItemName");
                                short childitemid = 0;
                                AutoCompleteBoxEntryCollection entries = txtItemName.Entries;
                                if (entries.Count > 0)
                                    childitemid = short.Parse(entries[0].Value);
                                else
                                {
                                    alertMessage("Invalid Item Name");
                                    e.Canceled = true;
                                    return;
                                }

                                #region validation
                                switch (method)
                                {
                                    case "%":
                                        {
                                            if (!string.IsNullOrEmpty(txtPercent.Text))
                                            {
                                                double percent_ = double.Parse(txtPercent.Text) / 100;
                                                allocatedCost_ = parentTotal * percent_;
                                                if ((total + allocatedCost_) > parentTotal)
                                                {
                                                    alertMessage("Cost Allocated Cannot Exceed Main Item Cost");
                                                    e.Canceled = true;
                                                    return;
                                                }
                                            }
                                            break;
                                        }

                                    case "Amount":
                                        {
                                            if (!string.IsNullOrEmpty(txtAllocatedCost.Text))
                                            {
                                                allocatedCost_ = double.Parse(txtAllocatedCost.Text);
                                                if ((total + allocatedCost_) > parentTotal)
                                                {
                                                    alertMessage("Cost Allocated Cannot Exceed Main Item Cost");
                                                    e.Canceled = true;
                                                    return;
                                                }
                                            }
                                            break;
                                        }

                                    case "UOM":
                                        {
                                            if ((!string.IsNullOrEmpty(txtQuantity.Text) && !string.IsNullOrEmpty(txtUnitCost.Text)))
                                            {
                                                allocatedCost_ = double.Parse(txtQuantity.Text) * double.Parse(txtUnitCost.Text);
                                                if ((total + allocatedCost_) > parentTotal)
                                                {
                                                    alertMessage("Cost Allocated Cannot Exceed Main Item Cost");
                                                    e.Canceled = true;
                                                    return;
                                                }
                                            }
                                            break;
                                        }
                                }
                                #endregion
                                allocatedCost = double.Parse(allocatedCost_.ToString("0.00"));

                                CostAllocation ca_old = CostAllocation.RetrieveByYearByDim(session.CompanyId, year, month, dim1, dim2, dim3, dim4, parentItemID, childitemid);
                                if (ca_old != null)
                                {
                                    alertMessage("Cost allocation in this dimension already exists.");
                                    e.Canceled = true;
                                    return;
                                }
                                else
                                {
                                    CostAllocation ca = new CostAllocation();
                                    ca.CoyID = session.CompanyId;
                                    ca.tbYear = year;
                                    ca.tbMonth = month;
                                    ca.Dim1 = dim1;
                                    ca.Dim2 = dim2;
                                    ca.Dim3 = dim3;
                                    ca.Dim4 = dim4;
                                    ca.Percentage = percent;
                                    ca.AllocatedCost = allocatedCost;
                                    ca.UOM = ddlUOM.SelectedValue;
                                    ca.Quantity = quantity;
                                    ca.UnitCost = unitCost;
                                    ca.ParentID = parentItemID;
                                    ca.ChildID = childitemid;
                                    ca.UpdatedBy = session.UserId;
                                    ca.UpdatedDate = DateTime.Now;
                                    ca.Save();
                                    alertMessage("Cost allocation saved.");
                                }
                            }
                            catch (Exception ex)
                            {
                                alertMessage(ex.ToString());
                                e.Canceled = true;
                                return;
                            }
                            break;
                        }
                }
            }
        }

        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.Item is GridEditableItem && e.Item.IsInEditMode && !(e.Item is IGridInsertItem))
            {
                try
                {
                    GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
                    double parentTotal = double.Parse(parentItem["TOTAL"].Text);

                    HtmlInputHidden hidMethod = parentItem.FindControl("hidMethod") as HtmlInputHidden;
                    string method = hidMethod.Value;
                    int parentItemID = int.Parse(parentItem.GetDataKeyValue("ID").ToString());
                    GridEditableItem editableItem = e.Item as GridEditableItem;
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
                    TextBox txtPercent = editableItem.FindControl("txtPercentage") as TextBox;
                    short percent = 0;
                    if (!string.IsNullOrEmpty(txtPercent.Text))
                        percent = short.Parse(txtPercent.Text);
                    DropDownList ddlUOM = editableItem.FindControl("ddlUOM") as DropDownList;
                    TextBox txtQuantity = editableItem["Quantity"].Controls[0] as TextBox;
                    int quantity = 0;
                    if (!string.IsNullOrEmpty(txtQuantity.Text))
                        quantity = int.Parse(txtQuantity.Text);
                    TextBox txtUnitCost = editableItem["UnitCost"].Controls[0] as TextBox;
                    int unitCost = 0;
                    if (!string.IsNullOrEmpty(txtUnitCost.Text))
                        unitCost = int.Parse(txtUnitCost.Text);
                    TextBox txtAllocatedCost = editableItem["AllocatedCost"].Controls[0] as TextBox;
                    double allocatedCost = 0;
                    if (!string.IsNullOrEmpty(txtAllocatedCost.Text))
                        allocatedCost = double.Parse(txtAllocatedCost.Text);
                    HtmlInputHidden hiddenTotal = editableItem.FindControl("hidTotal") as HtmlInputHidden;
                    double total = 0;
                    if (!string.IsNullOrEmpty(hiddenTotal.Value))
                        total = double.Parse(hiddenTotal.Value);
                    HtmlInputHidden hiddenID = editableItem.FindControl("hidDetailItemNo") as HtmlInputHidden;

                    RadAutoCompleteBox txtItemName = (RadAutoCompleteBox)editableItem.FindControl("txtItemName");
                    short childitemid = 0;
                    AutoCompleteBoxEntryCollection entries = txtItemName.Entries;
                    if (entries.Count > 0)
                        childitemid = short.Parse(entries[0].Value);
                    else
                    {
                        alertMessage("Invalid Item Name");
                        e.Canceled = true;
                        return;
                    }

                    double allocatedCost_ = 0;
                    int year = int.Parse(this.ddlYear.SelectedValue);
                    int month = int.Parse(this.ddlMonth.SelectedValue);

                    CostAllocation ca = CostAllocation.RetrieveByKey(int.Parse(hiddenID.Value));

                    #region validation
                    switch (method)
                    {
                        case "%":
                            {
                                if (!string.IsNullOrEmpty(txtPercent.Text))
                                {
                                    double percent_ = double.Parse(txtPercent.Text) / 100;
                                    allocatedCost_ = parentTotal * percent_;
                                    if ((total - ca.AllocatedCost + allocatedCost_) > parentTotal)
                                    {
                                        alertMessage("Cost Allocated Cannot Exceed Main Item Cost");
                                        e.Canceled = true;
                                        return;
                                    }
                                }
                                break;
                            }

                        case "Amount":
                            {
                                if (!string.IsNullOrEmpty(txtAllocatedCost.Text))
                                {
                                    allocatedCost_ = double.Parse(txtAllocatedCost.Text);
                                    if ((total - ca.AllocatedCost + allocatedCost_) > parentTotal)
                                    {
                                        alertMessage("Cost Allocated Cannot Exceed Main Item Cost");
                                        e.Canceled = true;
                                        return;
                                    }
                                }
                                break;
                            }

                        case "UOM":
                            {
                                if ((!string.IsNullOrEmpty(txtQuantity.Text) && !string.IsNullOrEmpty(txtUnitCost.Text)))
                                {
                                    allocatedCost_ = double.Parse(txtQuantity.Text) * double.Parse(txtUnitCost.Text);
                                    if ((total - ca.AllocatedCost + allocatedCost_) > parentTotal)
                                    {
                                        alertMessage("Cost Allocated Cannot Exceed Main Item Cost");
                                        e.Canceled = true;
                                        return;
                                    }
                                }
                                break;
                            }
                    }
                    #endregion
                    allocatedCost = double.Parse(allocatedCost_.ToString("0.00"));

                    if (ca != null)
                    {
                        ca.Dim1 = dim1;
                        ca.Dim2 = dim2;
                        ca.Dim3 = dim3;
                        ca.Dim4 = dim4;
                        ca.Percentage = percent;
                        ca.AllocatedCost = allocatedCost;
                        ca.UOM = ddlUOM.SelectedValue;
                        ca.Quantity = quantity;
                        ca.UnitCost = unitCost;
                        ca.ParentID = parentItemID;
                        ca.ChildID = childitemid;
                        ca.UpdatedBy = session.UserId;
                        ca.UpdatedDate = DateTime.Now;
                        ca.Save();
                        alertMessage("Cost allocation Updated.");
                    }
                    else
                    {
                        alertMessage("Cost allocation in this dimension already exists.");
                        e.Canceled = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    alertMessage(ex.ToString());
                    return;
                }
            }
        }

        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.Item is GridDataItem)
            {
                switch (e.Item.OwnerTableView.Name)
                {
                    case "Parent":
                        {
                            try
                            {
                                GridDataItem item = e.Item as GridDataItem;
                                HtmlInputHidden hiddenID = item.FindControl("hidID") as HtmlInputHidden;

                                CostAllocationMain cam = CostAllocationMain.RetrieveByKey(int.Parse(hiddenID.Value));

                                if (cam != null)
                                {
                                    cam.Delete();
                                    cam.Resync();
                                    alertMessage("Data deleted.");
                                }
                                else
                                {
                                    alertMessage("Data not found.");
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                alertMessage(ex.ToString());
                                return;
                            }
                            break;
                        }
                    case "ChildItem":
                        {
                            try
                            {
                                GridDataItem item = e.Item as GridDataItem;
                                HtmlInputHidden hiddenID = item.FindControl("hidDetailItemNo") as HtmlInputHidden;

                                CostAllocation ca = CostAllocation.RetrieveByKey(int.Parse(hiddenID.Value));

                                if (ca != null)
                                {
                                    ca.Delete();
                                    ca.Resync();
                                    alertMessage("Data deleted.");
                                }
                                else
                                {
                                    alertMessage("Data not found.");
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                alertMessage(ex.ToString());
                                return;
                            }
                            break;
                        }
                }

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

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                canEdit = false;

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
                            switch (method)
                            {
                                case "%":
                                    {
                                        UOM.Visible = false;
                                        Quantity.Visible = false;
                                        UnitCost.Visible = false;
                                        break;
                                    }

                                case "Amount":
                                    {
                                        Percentage.Visible = false;
                                        UOM.Visible = false;
                                        Quantity.Visible = false;
                                        UnitCost.Visible = false;
                                        break;
                                    }

                                case "UOM":
                                    {
                                        Percentage.Visible = false;
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();

            if (e.Item is GridEditableItem && e.Item.IsInEditMode && !(e.Item is IGridInsertItem))
            {
                GridEditableItem editableItem = e.Item as GridEditableItem;
                switch (editableItem.OwnerTableView.Name)
                {
                    case "ChildItem":
                        {
                            HtmlInputHidden hidItemName = (HtmlInputHidden)editableItem.FindControl("hidItemName");
                            HtmlInputHidden hidChildID = (HtmlInputHidden)editableItem.FindControl("hidChildID");
                            RadAutoCompleteBox txtItemName = (RadAutoCompleteBox)editableItem.FindControl("txtItemName");
                            if (txtItemName != null)
                                txtItemName.Entries.Add(new AutoCompleteBoxEntry(hidItemName.Value, hidChildID.Value));

                            short year = short.Parse(this.ddlYear.SelectedValue);
                            short month = short.Parse(this.ddlMonth.SelectedValue);

                            DropDownList ddlDim1 = editableItem.FindControl("ddlDim1") as DropDownList;
                            DropDownList ddlDim2 = editableItem.FindControl("ddlDim2") as DropDownList;
                            DropDownList ddlDim3 = editableItem.FindControl("ddlDim3") as DropDownList;
                            DropDownList ddlDim4 = editableItem.FindControl("ddlDim4") as DropDownList;

                            HtmlInputHidden hidDivision = (HtmlInputHidden)editableItem.FindControl("hidDivision");
                            HtmlInputHidden hidDepartment = (HtmlInputHidden)editableItem.FindControl("hidDepartment");
                            HtmlInputHidden hidSection = (HtmlInputHidden)editableItem.FindControl("hidSection");
                            HtmlInputHidden hidUnit = (HtmlInputHidden)editableItem.FindControl("hidUnit");
                            ddlDim1.SelectedValue = hidDivision.Value;

                            DataSet dsDepartments = new DataSet();
                            ggdal.GetCompanyDepartment(session.CompanyId, Convert.ToInt16(ddlDim1.SelectedValue), 0, session.UserId, year, month, ref dsDepartments);
                            foreach (DataRow dr in dsDepartments.Tables[0].Rows)
                            {
                                ddlDim2.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
                            }
                            ddlDim2.SelectedValue = hidDepartment.Value;

                            DataSet dsSections = new DataSet();
                            ggdal.GetCompanySection(session.CompanyId, Convert.ToInt16(ddlDim2.SelectedValue), 0, session.UserId, year, month, ref dsSections);
                            foreach (DataRow dr in dsSections.Tables[0].Rows)
                            {
                                ddlDim3.Items.Add(new ListItem(dr["SectionName"].ToString(), dr["SectionID"].ToString()));
                            }
                            ddlDim3.SelectedValue = hidSection.Value;

                            DataSet dsUnits = new DataSet();
                            ggdal.GetCompanyUnit(session.CompanyId, Convert.ToInt16(ddlDim3.SelectedValue), ref dsUnits);
                            foreach (DataRow dr in dsUnits.Tables[0].Rows)
                            {
                                ddlDim4.Items.Add(new ListItem(dr["UnitName"].ToString(), dr["UnitID"].ToString()));
                            }
                            ddlDim4.SelectedValue = hidUnit.Value;

                            break;
                        }
                }
            }
            else if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                switch (item.OwnerTableView.Name)
                {
                    case "ChildItem":
                        {
                            RadButton btnViewFile = (RadButton)item.FindControl("btnViewFile");
                            HtmlInputHidden hidFileName = (HtmlInputHidden)item.FindControl("hidFileName");
                            if (hidFileName.Value.Trim() == "-")
                            {
                                btnViewFile.Text = "File Not Found.";
                                btnViewFile.Enabled = false;
                            }
                            break;
                        }
                }
            }
        }

        protected void RadAsycnUpload1_onFileUploaded(object sender, FileUploadedEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string randomFileName = DateTime.Now.Ticks.ToString() + e.File.GetExtension();
            string folderPath = @"F:\\GMSDocuments\\CostAllocation\\" + session.CompanyId + "\\";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            RadAsyncUpload RadAsyncUpload1 = (RadAsyncUpload)sender;
            RadAsyncUpload1.TargetFolder = folderPath;

            string contentType = e.File.ContentType;

            GridEditableItem item = (sender as Control).NamingContainer as GridEditableItem;//get current row grid editable item in radgrid
            HtmlInputHidden hidDetailItemNo = (HtmlInputHidden)item.FindControl("hidDetailItemNo");

            try
            {
                CostAllocation ca = CostAllocation.RetrieveByKey(int.Parse(hidDetailItemNo.Value));
                if (ca != null)
                {
                    if (File.Exists(folderPath + ca.FileName))
                    {
                        File.Delete(folderPath + ca.FileName);
                    }
                    ca.FileName = randomFileName;
                    ca.Save();

                    e.File.SaveAs(Path.Combine(RadAsyncUpload1.TargetFolder, randomFileName));
                    alertMessage("Data & File Updated.");
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString());
            }
        }

        #region btnViewFile_Click
        protected void btnViewFile_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            string folderPath = @"F:\\GMSDocuments\\CostAllocation\\" + session.CompanyId ;

            GridEditableItem item = (sender as Control).NamingContainer as GridEditableItem;//get current row grid editable item in radgrid
            HtmlInputHidden hidDetailItemNo = (HtmlInputHidden)item.FindControl("hidDetailItemNo");

            try
            {
                CostAllocation ca = CostAllocation.RetrieveByKey(int.Parse(hidDetailItemNo.Value));
                if (ca != null)
                {
                    Byte[] bytes = File.ReadAllBytes(folderPath+"\\"+ca.FileName);
                    String file = Convert.ToBase64String(bytes);
                    this.RadPdfViewer1.PdfjsProcessingSettings.FileSettings.Data = file;
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString());
            }
        }
        #endregion

        protected void RadGrid1_OnItemCreated(object sender, GridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();

            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem editableItem = e.Item as GridEditableItem;
                switch (editableItem.OwnerTableView.Name)
                {
                    case "Parent":
                        {
                            RadAutoCompleteBox txtItemName = (RadAutoCompleteBox)editableItem.FindControl("txtItemName");
                            DataSet ds = new DataSet();
                            ggdal.SelectFinanceItem(ref ds);
                            txtItemName.DataSource = ds.Tables[0];
                            //ddlyear
                            var startYear = int.Parse(DateTime.Now.AddYears(-10).ToString("yyyy"));
                            var rangeCount = 10;
                            var ranges = Enumerable.Range(startYear, rangeCount).Select(y => (y + 1)).OrderByDescending(y => (y + 1));
                            DropDownList ddlYear = (DropDownList)editableItem.FindControl("ddlYear");
                            ddlYear.DataSource = ranges;
                            ddlYear.DataBind();
                            ddlYear.SelectedValue = this.ddlYear.SelectedValue;
                            //ddlMonth
                            DropDownList ddlMonth = (DropDownList)editableItem.FindControl("ddlMonth");
                            for (int i = 1; i <= 12; i++)
                            {
                                ddlMonth.Items.Add(i.ToString());
                            }
                            ddlMonth.SelectedValue = this.ddlMonth.SelectedValue;

                            #region dimension drop down list
                            DropDownList ddlDim1 = editableItem.FindControl("ddlDim1") as DropDownList;
                            DataSet dsProjects = new DataSet();
                            //same dimension restriction as f21 report
                            ggdal.GetCompanyProject(session.CompanyId, session.UserId, 426, ref dsProjects);
                            for (int j = 0; j < dsProjects.Tables[0].Rows.Count; j++)
                            {
                                ddlDim1.Items.Add(new ListItem(dsProjects.Tables[0].Rows[j]["ProjectName"].ToString(), dsProjects.Tables[0].Rows[j]["ReportProjectID"].ToString()));
                            }
                            DropDownList ddlDim2 = editableItem.FindControl("ddlDim2") as DropDownList;
                            DropDownList ddlDim3 = editableItem.FindControl("ddlDim3") as DropDownList;
                            DropDownList ddlDim4 = editableItem.FindControl("ddlDim4") as DropDownList;

                            string dim1 = this.ddlSearchDim1.SelectedValue;
                            string dim2 = "-1";
                            if (!string.IsNullOrEmpty(this.ddlSearchDim2.SelectedValue))
                                dim2 = this.ddlSearchDim2.SelectedValue;
                            string dim3 = "-1";
                            if (!string.IsNullOrEmpty(this.ddlSearchDim3.SelectedValue))
                                dim3 = this.ddlSearchDim3.SelectedValue;
                            string dim4 = "-1";
                            if (!string.IsNullOrEmpty(this.ddlSearchDim4.SelectedValue))
                                dim4 = this.ddlSearchDim4.SelectedValue;

                            ddlDim1.Items.FindByValue(dim1).Selected = true;
                            if (Convert.ToInt16(ddlDim1.SelectedValue.Count()) > 0)
                            {
                                short year = short.Parse(this.ddlYear.SelectedValue);
                                short month = short.Parse(this.ddlMonth.SelectedValue);
                                short reportId = 426;

                                ddlDim2.Enabled = true;
                                DataSet dsDepartments = new DataSet();
                                ggdal.GetCompanyDepartment(session.CompanyId, Convert.ToInt16(ddlDim1.SelectedValue), reportId, session.UserId, year, month, ref dsDepartments);

                                foreach (DataRow dr in dsDepartments.Tables[0].Rows)
                                {
                                    ddlDim2.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
                                }
                                ddlDim2.Items.FindByValue(dim2).Selected = true;

                                //Bind Section if Department > 0
                                if (Convert.ToInt16(ddlDim2.SelectedValue.Count()) > 0)
                                {
                                    DataSet dsSections = new DataSet();
                                    ggdal.GetCompanySection(session.CompanyId, Convert.ToInt16(ddlDim2.SelectedValue), reportId, session.UserId, year, month, ref dsSections);
                                    foreach (DataRow dr in dsSections.Tables[0].Rows)
                                    {
                                        ddlDim3.Items.Add(new ListItem(dr["SectionName"].ToString(), dr["SectionID"].ToString()));
                                    }
                                    ddlDim3.Items.FindByValue(dim3).Selected = true;
                                    ddlDim3.Enabled = true;
                                }

                                if (Convert.ToInt16(ddlDim3.SelectedValue.Count()) > 0)
                                {
                                    DataSet dsSections = new DataSet();
                                    ggdal.GetCompanyUnit(session.CompanyId, Convert.ToInt16(ddlDim3.SelectedValue), ref dsSections);
                                    foreach (DataRow dr in dsSections.Tables[0].Rows)
                                    {
                                        ddlDim4.Items.Add(new ListItem(dr["UnitName"].ToString(), dr["UnitID"].ToString()));
                                    }
                                    ddlDim4.Items.FindByValue(dim4).Selected = true;
                                    ddlDim4.Enabled = true;
                                }
                            }
                            #endregion
                            break;
                        }
                    case "ChildItem":
                        {
                            RadAutoCompleteBox txtItemName = (RadAutoCompleteBox)editableItem.FindControl("txtItemName");
                            DataSet ds = new DataSet();
                            ggdal.SelectFinanceChildItem(ref ds);
                            txtItemName.DataSource = ds.Tables[0];

                            DropDownList ddlUOM = (DropDownList)editableItem.FindControl("ddlUOM");
                            DataSet dsUOM = new DataSet();
                            ggdal.SelectUOM(session.CompanyId, ref dsUOM);
                            for (int i = 0; i < dsUOM.Tables[0].Rows.Count; i++)
                            {
                                ddlUOM.Items.Add(new ListItem(dsUOM.Tables[0].Rows[i]["Code"].ToString(), dsUOM.Tables[0].Rows[i]["Code"].ToString()));
                            }

                            GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
                            HtmlInputHidden hidMethod = parentItem.FindControl("hidMethod") as HtmlInputHidden;
                            string method = hidMethod.Value;

                            short year = short.Parse(this.ddlYear.SelectedValue);
                            short month = short.Parse(this.ddlMonth.SelectedValue);
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

                            switch (method)
                            {
                                case "%":
                                    {
                                        editableItem["UOM"].Parent.Visible = false;
                                        editableItem["Quantity"].Parent.Visible = false;
                                        editableItem["UnitCost"].Parent.Visible = false;
                                        editableItem["AllocatedCost"].Parent.Visible = false;
                                        break;
                                    }
                                case "Amount":
                                    {
                                        editableItem["Percentage"].Parent.Visible = false;
                                        editableItem["UOM"].Parent.Visible = false;
                                        editableItem["Quantity"].Parent.Visible = false;
                                        editableItem["UnitCost"].Parent.Visible = false;
                                        break;
                                    }
                                case "UOM":
                                    {
                                        editableItem["Percentage"].Parent.Visible = false;
                                        editableItem["AllocatedCost"].Parent.Visible = false;
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
        }

        #region dimension drop down list selected index changed event inside Radgrid
        protected void ddlGridYearMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DropDownList ddlYearMonth = (DropDownList)sender;
                GridEditFormInsertItem item = (GridEditFormInsertItem)ddlYearMonth.NamingContainer;
                DropDownList ddlDim1 = (DropDownList)item.FindControl("ddlDim1");
                DropDownList ddlDim2 = (DropDownList)item.FindControl("ddlDim2");
                DropDownList ddlDim3 = (DropDownList)item.FindControl("ddlDim3");
                DropDownList ddlDim4 = (DropDownList)item.FindControl("ddlDim4");

                ddlDim1.Items.Clear();
                ddlDim2.Items.Clear();
                ddlDim3.Items.Clear();
                ddlDim4.Items.Clear();

                ddlDim2.Enabled = false;
                ddlDim3.Enabled = false;
                ddlDim4.Enabled = false;
                DropDownList ddlYear = (DropDownList)item.FindControl("ddlYear");
                DropDownList ddlMonth = (DropDownList)item.FindControl("ddlMonth");
                short year = Convert.ToInt16(ddlYear.SelectedValue);
                short month = Convert.ToInt16(ddlMonth.SelectedValue);

                DataSet dsProjects = new DataSet();
                dacl.GetCompanyProject(session.CompanyId, session.UserId, 426, ref dsProjects);
                if (dsProjects.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsProjects.Tables[0].Rows.Count; j++)
                    {
                        ddlDim1.Items.Add(new ListItem(dsProjects.Tables[0].Rows[j]["ProjectName"].ToString(), dsProjects.Tables[0].Rows[j]["ReportProjectID"].ToString()));
                    }
                    if (Convert.ToInt16(ddlDim1.SelectedValue) > 0)
                    {
                        DataSet dsDepartments = new DataSet();
                        dacl.GetCompanyDepartment(session.CompanyId, Convert.ToInt16(ddlDim1.SelectedValue), 0, session.UserId, year, month, ref dsDepartments);
                        foreach (DataRow dr in dsDepartments.Tables[0].Rows)
                        {
                            ddlDim2.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
                        }
                        ddlDim2.Enabled = true;
                        //Bind Section if Department > 0
                        if (Convert.ToInt16(ddlDim2.SelectedValue) > 0)
                        {
                            DataSet dsSections = new DataSet();
                            dacl.GetCompanySection(session.CompanyId, Convert.ToInt16(ddlDim2.SelectedValue), 0, session.UserId, year, month, ref dsSections);
                            foreach (DataRow dr in dsSections.Tables[0].Rows)
                            {
                                ddlDim3.Items.Add(new ListItem(dr["SectionName"].ToString(), dr["SectionID"].ToString()));
                            }
                            ddlDim3.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlProjectID_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim1 = (DropDownList)sender;
            DropDownList ddlDim2 = null;
            DropDownList ddlDim3 = null;
            DropDownList ddlDim4 = null;
            short selectedvalue = Convert.ToInt16(ddlDim1.SelectedValue);
            try
            {
                GridEditFormInsertItem item = (GridEditFormInsertItem)ddlDim1.NamingContainer;
                ddlDim2 = (DropDownList)item.FindControl("ddlDim2");
                ddlDim3 = (DropDownList)item.FindControl("ddlDim3");
                ddlDim4 = (DropDownList)item.FindControl("ddlDim4");
            }
            catch (Exception ex)
            {
                GridEditFormItem item = (GridEditFormItem)ddlDim1.NamingContainer;
                ddlDim2 = (DropDownList)item.FindControl("ddlDim2");
                ddlDim3 = (DropDownList)item.FindControl("ddlDim3");
                ddlDim4 = (DropDownList)item.FindControl("ddlDim4");
            }
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
            ddlDim3.Enabled = false;
            ddlDim4.Enabled = false;
        }

        protected void ddlDepartmentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim2 = (DropDownList)sender;
            DropDownList ddlDim3 = null;
            DropDownList ddlDim4 = null;
            short selectedvalue = Convert.ToInt16(ddlDim2.SelectedValue);
            try
            {
                GridEditFormInsertItem item = (GridEditFormInsertItem)ddlDim2.NamingContainer;
                ddlDim3 = (DropDownList)item.FindControl("ddlDim3");
                ddlDim4 = (DropDownList)item.FindControl("ddlDim4");
            }
            catch (Exception ex)
            {
                GridEditFormItem item = (GridEditFormItem)ddlDim2.NamingContainer;
                ddlDim3 = (DropDownList)item.FindControl("ddlDim3");
                ddlDim4 = (DropDownList)item.FindControl("ddlDim4");
            }
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
            ddlDim4.Enabled = false;
        }

        protected void ddlSectionID_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim3 = (DropDownList)sender;
            DropDownList ddlDim4 = null;
            short selectedvalue = Convert.ToInt16(ddlDim3.SelectedValue);
            try
            {
                GridEditFormInsertItem item = (GridEditFormInsertItem)ddlDim3.NamingContainer;
                ddlDim4 = (DropDownList)item.FindControl("ddlDim4");
            }
            catch (Exception ex)
            {
                GridEditFormItem item = (GridEditFormItem)ddlDim3.NamingContainer;
                ddlDim4 = (DropDownList)item.FindControl("ddlDim4");
            }
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
        }

        #endregion

        protected void alertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        }
    }
}