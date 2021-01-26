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
using GMSCore.Entity;
using GMSCore.Activity;
using System.Collections.Generic;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Sales.Sales
{
    public partial class SalesPackage : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Products");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Products"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            111);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            111);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Products"));

            if (!Page.IsPostBack)
            {
                //preload
                this.dgData.CurrentPageIndex = 0;
                LoadData();
            }

            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>
                <script language=""javascript"" type=""text/javascript"">
                    var ctlt;
                    function SearchProduct(ctl)
                    {		
                        ctlt = ctl;	
	                    var url = 'ProductSearch.aspx'; 
                        window.open(url,"""",""width="" + 650 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
	                    return false;
                    }	

                    function SetSelectedProductCode(productCode)
                    {
                        if(productCode != null)
                        {					
	                        ctlt.value = productCode;
                            if (ctlt.value != """")
		                    {								
			                    ctlt.fireEvent('onchange');
		                    }	
                        }
                    }
	
                    function viewDetail(packageID)
                    {							
	                    var url = ""EditSalesPacakgeDetail.aspx?PackageID="" + packageID; 
                        window.open(url,"""",""width="" + 1000 + "",height="" + 650 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");		
	                    return false;
                    }		
                </script>
                ";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);

        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            IList<GMSCore.Entity.Package> lstPackage = null;
            try
            {
                lstPackage = new SystemDataActivity().RetrieveAllPackageByCoyID(session.CompanyId);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lstPackage.Count > 0)
            {
                if (endIndex < lstPackage.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstPackage.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstPackage.Count.ToString() + " " + "of" + " " + lstPackage.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";
            this.lblSearchSummary.Visible = true;

            this.dgData.DataSource = lstPackage;
            this.dgData.DataBind();

        }
        #endregion

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            lblSearchSummary.Text = e.NewPageIndex.ToString();

            lblSearchSummary.Visible = true;
            LoadData();

        }
        #endregion

        #region txtNewProductCode_OnTextChanged
        protected void txtNewProductCode_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            TextBox txtNewProductCode = (TextBox)sender;
            string prodCode = "";

            prodCode = txtNewProductCode.Text.Trim();

            DataSet ds = new DataSet();

            try
            {
                ProductsDataDALC dalc = new ProductsDataDALC();
                dalc.GetProductDetail(session.CompanyId, prodCode, ref ds);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    TableRow tr = (TableRow)txtNewProductCode.Parent.Parent;
                    DropDownList ddlNewUOM = (DropDownList)tr.FindControl("ddlNewUOM");
                    TextBox txtNewProductDescription = (TextBox)tr.FindControl("txtNewProductDescription");

                    txtNewProductDescription.Text = ds.Tables[0].Rows[0]["ProductName"].ToString();
                    ddlNewUOM.SelectedValue = ds.Tables[0].Rows[0]["UOM"].ToString();

                    ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                    scriptManager.SetFocus(txtNewProductDescription);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "click", "alert('Product is not found!')", true);
                    txtNewProductCode.Text = "";
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "click", "alert('" + ex.Message + "')", true);
            }
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
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                           112);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                TextBox txtNewProductCode = (TextBox)e.Item.FindControl("txtNewProductCode");
                TextBox txtNewProductDescription = (TextBox)e.Item.FindControl("txtNewProductDescription");
                DropDownList ddlNewUOM = (DropDownList)e.Item.FindControl("ddlNewUOM");
               
                DataSet ds = new DataSet();
                (new GMSGeneralDALC()).IsProductManager(session.CompanyId, txtNewProductCode.Text.Trim(), session.UserId, ref ds);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to create package for this product!')", true);
                    return;
                }

                try
                {
                    GMSCore.Entity.Package pkg = new GMSCore.Entity.Package();
                    pkg.CoyID = session.CompanyId;
                    pkg.ProductCode = txtNewProductCode.Text.Trim();
                    pkg.ProductDescription = txtNewProductDescription.Text.Trim();
                    pkg.UOM = ddlNewUOM.SelectedValue.Trim();
                    pkg.CreatedBy = session.UserId;
                    pkg.CreatedDate = DateTime.Now;
                    pkg.Save();
                    pkg.Resync();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "click", "alert('" + ex.Message + "')", true);
                }

                LoadData();
            }
        }
        #endregion

        #region dgData_EditCommand
        protected void dgData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                           112);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");
            DataSet ds = new DataSet();
            (new GMSGeneralDALC()).IsProductManager(session.CompanyId, hidProductCode.Value.Trim(), session.UserId, ref ds);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to edit package for this product!')", true);
                return;
            }

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

        #region dgData_UpdateCommand
        protected void dgData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            112);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");
            DataSet ds = new DataSet();
            (new GMSGeneralDALC()).IsProductManager(session.CompanyId, hidProductCode.Value.Trim(), session.UserId, ref ds);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to edit package for this product!')", true);
                return;
            }

            TextBox txtEditProductDescription = (TextBox)e.Item.FindControl("txtEditProductDescription");
            HtmlInputHidden hidPackageID = (HtmlInputHidden)e.Item.FindControl("hidPackageID");
            DropDownList ddlEditUOM = (DropDownList)e.Item.FindControl("ddlEditUOM");

            Package pkg = Package.RetrieveByKey(GMSUtil.ToInt(hidPackageID.Value.Trim()));
            if (pkg != null)
            {
                pkg.ProductDescription = txtEditProductDescription.Text.Trim();
                pkg.UOM = ddlEditUOM.SelectedValue.Trim();
                pkg.Save();
                pkg.Resync();
                this.dgData.EditItemIndex = -1;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Update", "alert('Pacakge not found!')", true);
            }
            LoadData();
        }
        #endregion

        #region dgData_DeleteCommand
        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            112);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");
            DataSet ds = new DataSet();
            (new GMSGeneralDALC()).IsProductManager(session.CompanyId, hidProductCode.Value.Trim(), session.UserId, ref ds);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to delete package for this product!')", true);
                return;
            }

            HtmlInputHidden hidPackageID = (HtmlInputHidden)e.Item.FindControl("hidPackageID");

            Package pkg = Package.RetrieveByKey(GMSUtil.ToInt(hidPackageID.Value.Trim()));
            if (pkg != null)
            {
                SystemDataActivity sActivity = new SystemDataActivity();
                IList<PackageProduct> lstPackageProduct = sActivity.RetrieveAllPackageProductByPackageID(GMSUtil.ToInt(hidPackageID.Value.Trim()));
                for (int i = lstPackageProduct.Count - 1; i >= 0; i--)
                {
                    PackageProduct pp = lstPackageProduct[i];
                    pp.Delete();
                    pp.Resync();
                }
                IList<PackageDetail> lstPackageDetail = sActivity.RetrieveAllPackageDetailByPackageID(GMSUtil.ToInt(hidPackageID.Value.Trim()));
                for (int i = lstPackageDetail.Count - 1; i >= 0; i--)
                {
                    PackageDetail pd = lstPackageDetail[i];
                    pd.Delete();
                    pd.Resync();
                }
                pkg.Delete();
                pkg.Resync();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Update", "alert('Pacakge not found!')", true);
            }
            LoadData();
        }
        #endregion

        #region dgData_ItemDataBound
        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
            else if (e.Item.ItemType == ListItemType.EditItem)
            {

                DropDownList ddlEditUOM = (DropDownList)e.Item.FindControl("ddlEditUOM");
                if (ddlEditUOM != null)
                {
                    DataSet dsTemp = new DataSet();
                    (new QuotationDataDALC()).GetAllUOMByCoyIDSelect(session.CompanyId, ref dsTemp);
                    ddlEditUOM.DataSource = dsTemp;
                    ddlEditUOM.DataValueField = "UOM";
                    ddlEditUOM.DataTextField = "UOM";
                    ddlEditUOM.DataBind();
                    GMSCore.Entity.Package qd = (GMSCore.Entity.Package)e.Item.DataItem;
                    ddlEditUOM.SelectedValue = qd.UOM;
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewUOM = (DropDownList)e.Item.FindControl("ddlNewUOM");
                DataSet dsTemp = new DataSet();
                (new QuotationDataDALC()).GetAllUOMByCoyIDSelect(session.CompanyId, ref dsTemp);
                ddlNewUOM.DataSource = dsTemp;
                ddlNewUOM.DataValueField = "UOM";
                ddlNewUOM.DataTextField = "UOM";
                ddlNewUOM.DataBind();
            }
        }
        #endregion
    }
}
