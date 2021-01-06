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
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System.Data.SqlClient;

namespace GMSWeb.Products.Products
{
    public partial class ProductSetup : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentLink = "Products";
            lblPageHeader.Text = "Products";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                if (Request.Params["CurrentLink"].ToString().Trim() == "Products")
                    lblPageHeader.Text = "Administration";
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
                                                                            156);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!Page.IsPostBack)
            {
                //preload
                dgData.CurrentPageIndex = 0;
                LoadData();
            }           

            string javaScript =
            @"
            <script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>

            <script type=""text/javascript"">
            function SelectOthers(chkbox)
	        {
	            var prefix = chkbox.id.substring(0,chkbox.id.lastIndexOf(""_"")+1);
	            if (chkbox.checked)
	            {
	                document.getElementById(prefix+""spanSalesPersonMasterName"").style.visibility = 'hidden';
	                document.getElementById(prefix+""spanArea"").style.visibility = 'hidden';
	            } else
	            {
	                document.getElementById(prefix+""spanSalesPersonMasterName"").style.visibility = 'visible';
	                document.getElementById(prefix+""spanArea"").style.visibility = 'visible';
	            }
	        } 
        	 
            </script>
            ";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            ProductsDataDALC proddacl = new ProductsDataDALC();
            DataSet dsProducts = new DataSet();
            proddacl.GetProductWithShortName(session.CompanyId, ref dsProducts);
            this.dgData.DataSource = dsProducts.Tables[0];
            this.dgData.DataBind();
        }
        #endregion

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        #region dgData_ItemDataBound
        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DataRowView dataItem1 = (DataRowView)e.Item.DataItem;
                DropDownList ddlEditBrand = (DropDownList)e.Item.FindControl("ddlEditBrand");
                GMSGeneralDALC ggdal = new GMSGeneralDALC();
                if (ddlEditBrand != null)
                {
                    DataSet dsBrand = new DataSet();
                    ggdal.RetrieveProductBrand(ref dsBrand);
                    ddlEditBrand.DataSource = dsBrand;
                    ddlEditBrand.DataBind();
                    ddlEditBrand.SelectedValue = dataItem1.Row["BrandID"].ToString();
                }
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                ProductsDataDALC proddacl = new ProductsDataDALC();
                DropDownList ddlNewProductGroup = (DropDownList)e.Item.FindControl("ddlNewProductGroup");
                if (ddlNewProductGroup != null)
                {                    
                    DataSet dsProductGroup = new DataSet();
                    proddacl.GetProductGroupInProduct(session.CompanyId, ref dsProductGroup);
                    ddlNewProductGroup.DataSource = dsProductGroup.Tables[0];
                    ddlNewProductGroup.DataBind();
                }
                GMSGeneralDALC ggdal = new GMSGeneralDALC();
                DropDownList ddlNewBrand = (DropDownList)e.Item.FindControl("ddlNewBrand");
                if (ddlNewBrand != null)
                {
                    DataSet dsBrand = new DataSet();
                    ggdal.RetrieveProductBrand(ref dsBrand);
                    ddlNewBrand.DataSource = dsBrand.Tables[0];
                    ddlNewBrand.DataBind();
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region dgData_EditCommand
        protected void dgData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }
        #endregion

        #region dgData_CancelCommand
        protected void dgData_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgData_CreateCommand
        protected void dgData_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Products"));
                    return;
                }

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                157);
                if (uAccess == null)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "You don't have access." + "\");</script>", false);
                    return;
                }

                DropDownList ddlNewProduct = (DropDownList)e.Item.FindControl("ddlNewProduct");
                TextBox txtNewShortName = (TextBox)e.Item.FindControl("txtNewShortName");
                DropDownList ddlNewbrand = (DropDownList)e.Item.FindControl("ddlNewBrand");

                if (ddlNewProduct != null && !string.IsNullOrEmpty(ddlNewProduct.SelectedValue) &&
                   txtNewShortName != null && !string.IsNullOrEmpty(txtNewShortName.Text) &&
                   ddlNewbrand != null && !string.IsNullOrEmpty(ddlNewbrand.SelectedValue))
                { 
                    try
                    {
                        new GMSGeneralDALC().UpdateProductShortName(session.CompanyId, ddlNewProduct.SelectedValue, txtNewShortName.Text.Trim(),ddlNewbrand.SelectedValue);     
                        this.dgData.EditItemIndex = -1;
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgData_UpdateCommand
        protected void dgData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Products"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            157);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Products"));

            HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");
            TextBox txtEditShortName = (TextBox)e.Item.FindControl("txtEditShortName");
            DropDownList ddlEditBrand = (DropDownList)e.Item.FindControl("ddlEditBrand");

            if (hidProductCode != null &&
                txtEditShortName != null && !string.IsNullOrEmpty(txtEditShortName.Text) &&
                ddlEditBrand != null && !string.IsNullOrEmpty(ddlEditBrand.SelectedValue))
            {
                try
                {
                    new GMSGeneralDALC().UpdateProductShortName(session.CompanyId, hidProductCode.Value, txtEditShortName.Text.Trim(),ddlEditBrand.SelectedValue);
                    this.dgData.EditItemIndex = -1;
                    LoadData();
                }
                catch (Exception ex)
                {
                    this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }
        }
        #endregion

        #region dgData_DeleteCommand
        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Products"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                157);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Products"));

                HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");

                if (hidProductCode != null)
                {
                    try
                    {
                        new GMSGeneralDALC().UpdateProductShortName(session.CompanyId, hidProductCode.Value, "","0");                    
                        this.dgData.EditItemIndex = -1;
                        this.dgData.CurrentPageIndex = 0;
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            RetrieveProduct();
        }
        #endregion

        #region RetrieveProduct
        private void RetrieveProduct()
        {

            LogSession session = base.GetSessionInfo();
            ProductsDataDALC proddacl = new ProductsDataDALC();
            DataSet dsProducts = new DataSet();

            string ProductCode = "";
            string ProductName = "";
            string ShortName = "";
            string Brand = "";
            if (string.IsNullOrEmpty(txtProductCode.Text.Trim()) && string.IsNullOrEmpty(txtProductName.Text.Trim()) && string.IsNullOrEmpty(txtShortName.Text.Trim()) && string.IsNullOrEmpty(txtBrand.Text.Trim()))
            {
                this.MsgPanel2.ShowMessage("Please input product to search", MessagePanelControl.MessageEnumType.Alert);
                resultList.Visible = false;
            }
            else
            {
                ProductCode = "%" + txtProductCode.Text.Trim() + "%";
                ProductName = "%" + txtProductName.Text.Trim() + "%";
                ShortName = "%" + txtShortName.Text.Trim() + "%";
                Brand = "%" + txtBrand.Text.Trim() + "%";
                resultList.Visible = true;
            }
            try
            {
                proddacl.GetProductCodeWithShortName(session.CompanyId, ProductCode, ProductName, ShortName, Brand, ref dsProducts);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (dsProducts != null && dsProducts.Tables[0].Rows.Count > 0)
            {
                if (endIndex < dsProducts.Tables[0].Rows.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + dsProducts.Tables[0].Rows.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        dsProducts.Tables[0].Rows.Count.ToString() + " " + "of" + " " + dsProducts.Tables[0].Rows.Count.ToString();

                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = dsProducts;
                this.dgData.DataBind();

            }
            else
            {
                this.lblSearchSummary.Text = "No records.";
                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = null;
                this.dgData.DataBind();
                return;

            }
        }
        #endregion

        protected void ddlNewProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();  
            DropDownList ddlNewProductGroup = (DropDownList)sender;
            string productGroup = ddlNewProductGroup.SelectedValue;

            ProductsDataDALC proddacl = new ProductsDataDALC();
            TableRow tr = (TableRow)ddlNewProductGroup.Parent.Parent;
            DropDownList ddlNewProduct = (DropDownList)tr.FindControl("ddlNewProduct");
            if (ddlNewProduct != null)
            {
                DataSet dsProducts = new DataSet();
                proddacl.GetProductWithNoShortName(session.CompanyId, productGroup, ref dsProducts);
                ddlNewProduct.DataSource = dsProducts.Tables[0];
                ddlNewProduct.DataBind();
            }   
        }
    }
}