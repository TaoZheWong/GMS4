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
    public partial class ProductMangerSetup : GMSBasePage
    {
        short loginUserOrAlternateParty = 0;
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
                                                                            176);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            176);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!IsPostBack)
            {
                this.RadGrid1.CurrentPageIndex = 0;
                RetrieveBrandProductByPM();
                this.RadGrid1.DataBind();
            }
        }

        #region Retrieve Data
        private void RetrieveBrandProductByPM()
        {
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            resultList.Visible = true;

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

            try
            {
                ggdal.RetrieveBrandProductByPM(session.CompanyId, loginUserOrAlternateParty, ref ds);
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
            RetrieveBrandProductByPM();
        }

        protected void radGrid_OnPageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            RetrieveBrandProductByPM();
        }

        protected void radGrid_OnPageSizeChanged(object source, GridPageSizeChangedEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.CurrentPageIndex = e.NewPageSize;
            RetrieveBrandProductByPM();
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
                ggdal.RetrieveBrandProductByPM(session.CompanyId, session.UserId, ref ds);
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

        protected void radGrid_OnCancel(object source, GridCommandEventArgs e)
        {
            RadGrid dtg = (RadGrid)source;
            dtg.MasterTableView.ClearEditItems();
            RetrieveBrandProductByPM();
        }

        protected void RadGrid1_OnUpdateCommand(object source, GridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.Item is GridEditableItem && e.Item.IsInEditMode && !(e.Item is IGridInsertItem))
            {
                try
                {
                    GridEditableItem editableItem = e.Item as GridEditableItem;
                    string productGroupCode = editableItem.GetDataKeyValue("ProductGroupCode").ToString();
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
                    TextBox txtShortName = editableItem["ShortName"].Controls[0] as TextBox;
                    string shortName = txtShortName.Text;
                    TextBox txtProductCategory = editableItem["ProductCategory"].Controls[0] as TextBox;
                    string productCategory = txtProductCategory.Text;
                    TextBox txtMainCategory = editableItem["MainCategory"].Controls[0] as TextBox;
                    string mainCategory = txtMainCategory.Text;
                    CheckBox isBudget = editableItem["IsBudget"].Controls[0] as CheckBox;
                    CheckBox isActive = editableItem["IsActive"].Controls[0] as CheckBox;
                    ProductGroup pg = ProductGroup.RetrieveByKey(session.CompanyId, productGroupCode);
                    if (pg != null)
                    {
                        pg.ShortName = shortName;
                        pg.ProductCategory = productCategory;
                        pg.MainCategory = mainCategory;
                        pg.IsBudget = isBudget.Checked;
                        pg.Dim1 = dim1;
                        pg.Dim2 = dim2;
                        pg.Dim3 = dim3;
                        pg.Dim4 = dim4;
                        pg.IsActive = isActive.Checked;
                        pg.Save();
                        alertMessage("Brand/Product Updated.");
                    }
                    else
                    {
                        alertMessage("Brand/Product not exists");
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
            DataSet ds = new DataSet();
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                RadDropDownList ddlPM = item.FindControl("ddlPM") as RadDropDownList;
                ggdal.RetrievePMUser(session.CompanyId, ref ds);
                if (ds != null)
                {
                    ddlPM.DataSource = ds.Tables[0];
                    ddlPM.DataBind();
                    ddlPM.Attributes["onChange"] = "handleChange(this)";
                    ddlPM.Attributes["onFocus"] = "this.oldIndex = this.selectedIndex";
                }
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                HtmlInputHidden hidPM = item.FindControl("hidPM") as HtmlInputHidden;
                RadDropDownList ddlPM = item.FindControl("ddlPM") as RadDropDownList;
                if (!string.IsNullOrEmpty(hidPM.Value)) 
                    ddlPM.SelectedValue = hidPM.Value;
            }
        }

        protected void ddlPM_onSelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            RadDropDownList ddlPM = (RadDropDownList)sender;
            GridDataItem item = (GridDataItem)ddlPM.NamingContainer;
            string productGroupCode = item.GetDataKeyValue("ProductGroupCode").ToString();
            if (productGroupCode != null)
            {
                try
                {
                    ProductManagerProduct pmp = ProductManagerProduct.RetrieveByKey(session.CompanyId, productGroupCode);
                    pmp.PMUserID = int.Parse(ddlPM.SelectedValue);
                    pmp.Save();
                    alertMessage("Success Update PM.");

                }
                catch (Exception ex)
                {
                    alertMessage(ex.ToString());
                    return;
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