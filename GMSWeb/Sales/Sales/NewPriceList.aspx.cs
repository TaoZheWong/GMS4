using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GMSWeb.Sales.Sales
{
    public partial class NewPriceList : GMSBasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
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
                                                                            162);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!IsPostBack)
            {
                LoadData();
            }

            string javaScript =
            @"<script type=""text/javascript"">
		    function toggleAccessRow(n)
		    {
			    if( document.getElementById(""rppToggle_"" + n) )
			    {
				    var current = document.getElementById(""rppToggle_"" + n).style.display;
				    document.getElementById(""rppToggle_"" + n).style.display = (current == null || current == ""none"")?"""":""none"";
				    document[""imgAccessBox_"" + n].src = (current == null || current == ""none"")? sDOMAIN+""/App_Themes/Default/images/checkCloseIcon.gif"" : sDOMAIN+""/App_Themes/Default/images/checkOpenIcon.gif"";
			    }
		    }
		    </script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet ds1 = new DataSet();
            ggdal.RetrieveProductBrand(ref ds1);
            ds1.Tables[0].Rows.Add(0, "OthWS", "OthWS");
            this.ddlSearchBrandName.DataSource = ds1.Tables[0];
            this.ddlSearchBrandName.DataBind();
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.DataGrid1.CurrentPageIndex = 0;
            RetrieveProduct();
        }
        #endregion

        #region DataGrid1 PageIndexChanged event handling
        protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            RetrieveProduct();
        }
        #endregion

        #region DataGrid1_ItemDataBound
        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewBrand = (DropDownList)e.Item.FindControl("ddlNewBrand");
                if (ddlNewBrand != null)
                {
                    GMSGeneralDALC ggdal = new GMSGeneralDALC();
                    DataSet ds1 = new DataSet();
                    ggdal.RetrieveProductBrand(ref ds1);
                    ds1.Tables[0].Rows.Add(0, "OthWS", "OthWS");
                    ddlNewBrand.DataSource = ds1.Tables[0];
                    ddlNewBrand.DataBind();
                    //ddlNewTeamName.SelectedValue = lstSalesGroupTeam.
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblStockStatus = (Label)e.Item.FindControl("lblStockStatus");
                HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");

                #region Retrieve live stock balance
                bool isGasDivision = false;
                bool isWeldingDivision = false;
                bool canAccessProductStatus = false;
                DataSet ds_lms = new DataSet();
                DataSet ds = new DataSet();
                try
                {
                    if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                    {
                        GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                        if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                        {
                            sc.Url = session.WebServiceAddress.Trim();
                        }
                        else
                            sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                        ds = sc.GetProductStockStatus(session.CompanyId, hidProductCode.Value.Trim());
                    }
                    else if (session.StatusType.ToString() == "L")
                    {
                        CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        {
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                        ds = sc1.GetProductWarehouse(hidProductCode.Value.Trim());
                    }
                    else if (session.StatusType.ToString() == "S")
                    {

                        string query = "CALL \"AF_API_GET_SAP_STOCK_STATUS\" ('" + hidProductCode.Value.Trim() + "', '', '', '', '', '2099-12-31', 'Y')";
                        SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                        ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                        "ItemCode", "Warehouse", "OnHand", "Committed", "Quantity", "WarehouseName", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                    }
                }
                catch (Exception ex)
                {
                    this.JScriptAlertMsg(ex.Message);
                }
                DivisionUser du = DivisionUser.RetrieveByKey(session.CompanyId, session.UserId);
                if (du != null)
                {
                    if (du.DivisionID == "GAS" && isGasDivision)
                    {
                        canAccessProductStatus = true;
                    }
                    else if (du.DivisionID == "WSD" && isWeldingDivision)
                    {
                        canAccessProductStatus = true;
                    }
                }
                else
                    canAccessProductStatus = true;

                if (canAccessProductStatus && ((ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)))
                {

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["Quantity"].ToString() != "0" && Decimal.Round(Convert.ToDecimal(dr["Quantity"]), 6).ToString() != "0.000000")
                                lblStockStatus.Text = lblStockStatus.Text +  Decimal.Round(Convert.ToDecimal(dr["Quantity"]), 6).ToString() + "<br>";
                        }
                    }
                    
                    if (lblStockStatus.Text.Length >= 4)
                        lblStockStatus.Text = lblStockStatus.Text.Substring(0, lblStockStatus.Text.Length - 4);

                    if(string.IsNullOrEmpty(lblStockStatus.Text))
                        lblStockStatus.Text = "0";
                }
                #endregion

                foreach (DataGridItem item in DataGrid1.Items)
                {   //add id for each row for drag & drop to save order
                    hidProductCode = (HtmlInputHidden)item.FindControl("hidProductCode");
                    item.Attributes.Add("id", hidProductCode.Value.ToString());
                }
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete2");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                163);
                if (uAccess == null)
                {   //disable column & edit function for sales person
                    this.DataGrid1.ShowFooter = false;
                    this.DataGrid1.Attributes.Add("style","pointer-events: none");
                    this.DataGrid1.Columns[7].Visible = false;
                    this.DataGrid1.Columns[9].Visible = false;
                    this.DataGrid1.Columns[11].Visible = false;
                    this.DataGrid1.Columns[12].Visible = false;
                    this.DataGrid1.Columns[13].Visible = false;
                    this.DataGrid1.Columns[14].Visible = false;
                    this.DataGrid1.Columns[15].Visible = false;
                    this.DataGrid1.Columns[16].Visible = false;
                }
            }
        }
        #endregion

        #region DataGrid1_EditCommand
        protected void DataGrid1_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.DataGrid1.EditItemIndex = e.Item.ItemIndex;
            RetrieveProduct();
        }
        #endregion

        #region DataGrid1_CancelCommand
        protected void DataGrid1_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.DataGrid1.EditItemIndex = -1;
            RetrieveProduct();
        }
        #endregion

        #region DataGrid1_CreateCommand
        protected void DataGrid1_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                163);
                if (uAccess == null)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "You don't have access." + "\");</script>", false);
                    return;
                }

                DropDownList ddlNewProduct = (DropDownList)e.Item.FindControl("ddlNewProduct");
                TextBox txtNewDealerPrice = (TextBox)e.Item.FindControl("txtNewDealerPrice");
                TextBox txtNewUserPrice = (TextBox)e.Item.FindControl("txtNewUserPrice");
                TextBox txtNewRetailPrice = (TextBox)e.Item.FindControl("txtNewRetailPrice");

                if (ddlNewProduct != null && !string.IsNullOrEmpty(ddlNewProduct.SelectedValue) &&
                     txtNewDealerPrice != null && !string.IsNullOrEmpty(txtNewDealerPrice.Text)&&
                     txtNewUserPrice != null && !string.IsNullOrEmpty(txtNewUserPrice.Text)&&
                     txtNewRetailPrice != null && !string.IsNullOrEmpty(txtNewRetailPrice.Text))
                {
                    try
                    {
                        Product product = Product.RetrieveByKey(session.CompanyId, ddlNewProduct.SelectedValue);
                        ProductPrice pp_old = ProductPrice.RetrieveByKey(session.CompanyId, ddlNewProduct.SelectedValue);
                        if(pp_old != null)
                        {
                            pp_old.IsExpired = true;
                            pp_old.Save();
                        }
                        ProductPrice pp = new ProductPrice();
                        pp.CoyID = session.CompanyId;
                        pp.ProductCode = ddlNewProduct.SelectedValue;
                        pp.WeightedCost = Decimal.Parse(product.WeightedCost.ToString());
                        pp.DealerPrice = decimal.Parse(txtNewDealerPrice.Text.Trim());
                        pp.UserPrice = decimal.Parse(txtNewUserPrice.Text.Trim());
                        pp.RetailPrice = decimal.Parse(txtNewRetailPrice.Text.Trim());
                        pp.UpdatedBy = session.UserId;
                        pp.UpdatedDate = DateTime.Now;
                        pp.IsExpired = false;
                        pp.Save();
                        RetrieveProduct();
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region DataGrid1_UpdateCommand
        protected void DataGrid1_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            153);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            TextBox txtEditDealerPrice = (TextBox)e.Item.FindControl("txtEditDealerPrice");
            TextBox txtEditUserPrice = (TextBox)e.Item.FindControl("txtEditUserPrice");
            TextBox txtEditRetailPrice = (TextBox)e.Item.FindControl("txtEditRetailPrice");
            HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");
            Label lblUpdatedDateEdit = (Label)e.Item.FindControl("lblUpdatedDateEdit");

            if (hidProductCode != null && txtEditDealerPrice != null && !string.IsNullOrEmpty(txtEditDealerPrice.Text)
                 && txtEditUserPrice != null && !string.IsNullOrEmpty(txtEditUserPrice.Text)
                 && txtEditRetailPrice != null && !string.IsNullOrEmpty(txtEditRetailPrice.Text))
            {
                Product product = Product.RetrieveByKey(session.CompanyId, hidProductCode.Value.ToString());
                ProductPrice pp = ProductPrice.RetrieveByKey(session.CompanyId, hidProductCode.Value.ToString());
                pp.WeightedCost = (Decimal)product.WeightedCost;
                pp.DealerPrice = decimal.Parse(txtEditDealerPrice.Text.Trim());
                pp.UserPrice = decimal.Parse(txtEditUserPrice.Text.Trim());
                pp.RetailPrice = decimal.Parse(txtEditRetailPrice.Text.Trim());
                pp.UpdatedBy = session.UserId;
                pp.UpdatedDate = DateTime.Now;
                try
                {
                    pp.Save();
                    this.DataGrid1.EditItemIndex = -1;
                    RetrieveProduct();
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }
        }
        #endregion

        #region DataGrid1_DeleteCommand
        protected void DataGrid1_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                153);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");
                Label lblUpdatedDate = (Label)e.Item.FindControl("lblUpdatedDate");

                if (hidProductCode != null)
                {
                    try
                    {
                        ProductPrice pp = ProductPrice.RetrieveByKey(session.CompanyId, hidProductCode.Value.ToString());
                        pp.Delete();
                        pp.Resync();
                        ProductPrice pp_old = ProductPrice.RetrieveByKeyExpired(session.CompanyId, hidProductCode.Value.ToString());
                        if (pp_old != null)
                        {
                            pp_old.IsExpired = false;
                            pp_old.Save();
                        }
                        this.DataGrid1.EditItemIndex = -1;
                        RetrieveProduct();
                    }
                    catch (SqlException exSql)
                    {
                        this.PageMsgPanel.ShowMessage(exSql.Message, MessagePanelControl.MessageEnumType.Alert);
                        RetrieveProduct();
                        return;
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        RetrieveProduct();
                        return;
                    }
                }
            }
        }
        #endregion

        #region RetrieveProduct
        private void RetrieveProduct()
        {
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();

            string ProductCode = "";
            string ProductName = "";
            string ProductGroupCode = "";
            int Brand =0 ;
            if (string.IsNullOrEmpty(txtProductCode.Text.Trim()) && string.IsNullOrEmpty(txtProductName.Text.Trim()) && 
                string.IsNullOrEmpty(ddlSearchBrandName.SelectedValue))
            {
                this.MsgPanel2.ShowMessage("Please input product to search", MessagePanelControl.MessageEnumType.Alert);
                resultList.Visible = false;
            }
            else
            {
                ProductCode = "%" + txtProductCode.Text.Trim() + "%";
                ProductName = "%" + txtProductName.Text.Trim() + "%";
                ProductGroupCode = "%" + ddlSearchProductGroup.SelectedValue + "%";
                Brand = int.Parse(ddlSearchBrandName.SelectedValue);
                resultList.Visible = true;
            }
            try
            {
                ggdal.RetrieveProductPrice(session.CompanyId, Brand, ProductCode, ProductName,ProductGroupCode,ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                this.lblSearchSummary.Text = ds.Tables[0].Rows.Count.ToString()+" Results";
                this.lblSearchSummary.Visible = true;
                this.DataGrid1.Visible = true;
                this.DataGrid1.DataSource = ds;
                this.DataGrid1.DataBind();
            }
            else
            {
                this.lblSearchSummary.Text = "No records.";
                this.lblSearchSummary.Visible = true;
                this.DataGrid1.Visible = true;
                this.DataGrid1.DataSource = null;
                this.DataGrid1.DataBind();
                return;
            }
        }
        #endregion

        #region ddlNewBrand_SelectedIndexChanged
        protected void ddlNewBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            DropDownList ddlNewBrand = (DropDownList)sender;
            int brand = int.Parse(ddlNewBrand.SelectedValue);

            ProductsDataDALC ppdac = new ProductsDataDALC();
            TableRow tr = (TableRow)ddlNewBrand.Parent.Parent;
            DropDownList ddlNewProduct = (DropDownList)tr.FindControl("ddlNewProduct");
            if (ddlNewProduct != null)
            {
                DataSet dsProducts = new DataSet();
                ppdac.GetProductByBrand(session.CompanyId, brand, ref dsProducts);
                ddlNewProduct.DataSource = dsProducts.Tables[0];
                ddlNewProduct.DataBind();
            }
        }
        #endregion

        #region ddlSearchBrandName_SelectedIndexChanged
        protected void ddlSearchBrandName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            DropDownList ddlSearchBrandName = (DropDownList)sender;
            int brand = int.Parse(ddlSearchBrandName.SelectedValue);

            if (ddlSearchBrandName.SelectedValue.Equals("0")|| ddlSearchBrandName.SelectedValue.Equals("62"))
            {
                GMSGeneralDALC ggdal = new GMSGeneralDALC();
                DataSet ds = new DataSet();
                ggdal.RetrieveProductPriceProductGroup(session.CompanyId, brand,ref ds);
                this.ddlSearchProductGroup.DataSource = ds.Tables[0];
                this.ddlSearchProductGroup.DataBind();
                this.hiddenLabel.Visible = true;
            }
            else
            {
                this.hiddenLabel.Visible = false;
            }
        }
        #endregion

        #region Drag and Drop GridViewReorders
        [WebMethod]
        public static void GridViewReorders(string Reorder)
        {
            GMSGeneralDALC ggdal = new GMSGeneralDALC();

            string[] ListID = Reorder.Split('|');
            for (int i = 0; i < ListID.Length; i++){
                if (ListID[i] != "" && ListID[i] != null)
                {
                    ggdal.UpdateProductPriceOrder(ListID[i], i+1);
                }
            }
        }
        #endregion
    }
}