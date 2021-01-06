using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace GMSWeb.Products.Products
{
    public partial class ProductCategorySetup : GMSBasePage
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
                                                                            177);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            177);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!IsPostBack)
            {
                this.RadGrid1.CurrentPageIndex = 0;
                RetrieveProductCategory();
                this.RadGrid1.DataBind();
            }
        }


        #region Retrieve Data
        private void RetrieveProductCategory()
        {
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            resultList.Visible = true;
            try
            {
                ggdal.RetrieveProductCategory(session.CompanyId, ref ds);
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString());
            }

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                this.lblSearchSummary.Visible = false;
                this.RadGrid1.DataSource = ds;
                RadGrid1.MasterTableView.PageSize = 50;
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
            RetrieveProductCategory();
        }

        protected void radGrid_OnPageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            RetrieveProductCategory();
        }

        protected void radGrid_OnPageSizeChanged(object source, GridPageSizeChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageSize;
            RetrieveProductCategory();
        }

        protected void radGrid_OnCancel(object source, GridCommandEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.MasterTableView.ClearEditItems();
            RetrieveProductCategory();
        }

        protected void RadGrid1_OnUpdateCommand(object source, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.Item is GridEditableItem && e.Item.IsInEditMode && !(e.Item is IGridInsertItem))
            {
                try
                {
                    GridEditableItem editableItem = e.Item as GridEditableItem;
                    short categoryID = short.Parse(editableItem.GetDataKeyValue("CategoryID").ToString());
                    TextBox txtShortName = editableItem["ShortName"].Controls[0] as TextBox;
                    string shortName = txtShortName.Text;

                    BrandCategory bc = BrandCategory.RetrieveByKey(categoryID);
                    if (bc != null)
                    {
                        bc.ShortName = shortName;
                        bc.Save();
                        alertMessage("Product category Updated.");
                    }
                    else
                    {
                        alertMessage("Failed to Update.");
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
                ggdal.RetrieveProductCategory(session.CompanyId, ref ds);
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

        protected void radGrid_RowDrop(object sender, GridDragDropEventArgs e)
        {
            if (e.DestDataItem != null)
            {
                int destinationIndex =0;
                if (e.DropPosition == GridItemDropPosition.Above && e.DestDataItem.ItemIndex > e.DraggedItems[0].ItemIndex)
                {
                    destinationIndex -= 1;
                }
                if (e.DropPosition == GridItemDropPosition.Below && e.DestDataItem.ItemIndex < e.DraggedItems[0].ItemIndex)
                {
                    destinationIndex += 1;
                }
            }
        }


       

        #endregion

        protected void alertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        }
    }
}