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
using GMSCore.Activity;
using System.Collections.Generic;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Sales.Sales
{
    public partial class EditSalesPacakgeDetail : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Master.setCurrentLink("Sales");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            111);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                if (Request.Params["PackageID"] != null)
                {
                    ViewState["PackageID"] = Request.Params["PackageID"].ToString().Trim();
                }
                //preload
                this.dgProduct.CurrentPageIndex = 0;
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
                </script>
                ";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);

        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            Package pkg = Package.RetrieveByKey(GMSUtil.ToInt(ViewState["PackageID"].ToString()));
            lblPackageProductCode.Text = pkg.ProductCode;
            lblPackageProductDescription.Text = pkg.ProductDescription;

            IList<GMSCore.Entity.PackageProduct> lstPackageProduct = null;
            try
            {
                lstPackageProduct = new SystemDataActivity().RetrieveAllPackageProductByPackageID(GMSUtil.ToInt(ViewState["PackageID"].ToString()));
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            this.dgProduct.DataSource = lstPackageProduct;
            this.dgProduct.DataBind();

            IList<GMSCore.Entity.PackageDetail> lstPackageDetail = null;
            try
            {
                lstPackageDetail = new SystemDataActivity().RetrieveAllPackageDetailByPackageID(GMSUtil.ToInt(ViewState["PackageID"].ToString()));
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            this.dgDetail.DataSource = lstPackageDetail;
            this.dgDetail.DataBind();
        }
        #endregion

        #region dgProduct datagrid PageIndexChanged event handling
        protected void dgProduct_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
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
                    TextBox txtNewProductDescription = (TextBox)tr.FindControl("txtNewProductDescription");
                    txtNewProductDescription.Text = ds.Tables[0].Rows[0]["ProductName"].ToString();
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

        #region dgProduct_CreateCommand
        protected void dgProduct_CreateCommand(object sender, DataGridCommandEventArgs e)
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

                DataSet ds = new DataSet();
                (new GMSGeneralDALC()).IsProductManager(session.CompanyId, lblPackageProductCode.Text.Trim(), session.UserId, ref ds);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to edit package detail for this product!')", true);
                    return;
                }

                TextBox txtNewProductCode = (TextBox)e.Item.FindControl("txtNewProductCode");
                TextBox txtNewProductDescription = (TextBox)e.Item.FindControl("txtNewProductDescription");
                TextBox txtNewQty = (TextBox)e.Item.FindControl("txtNewQty");

                try
                {
                    GMSCore.Entity.PackageProduct pkg = new GMSCore.Entity.PackageProduct();
                    pkg.CoyID = session.CompanyId;
                    pkg.PackageID = GMSUtil.ToInt(ViewState["PackageID"].ToString());
                    pkg.ProductCode = txtNewProductCode.Text.Trim();
                    pkg.ProductDescription = txtNewProductDescription.Text.Trim();
                    pkg.Qty = GMSUtil.ToDouble(txtNewQty.Text.Trim());
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

        #region dgProduct_EditCommand
        protected void dgProduct_EditCommand(object sender, DataGridCommandEventArgs e)
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

            DataSet ds = new DataSet();
            (new GMSGeneralDALC()).IsProductManager(session.CompanyId, lblPackageProductCode.Text.Trim(), session.UserId, ref ds);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to edit package detail for this product!')", true);
                return;
            }

            this.dgProduct.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }
        #endregion

        #region dgProduct_CancelCommand
        protected void dgProduct_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgProduct.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgProduct_UpdateCommand
        protected void dgProduct_UpdateCommand(object sender, DataGridCommandEventArgs e)
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

            DataSet ds = new DataSet();
            (new GMSGeneralDALC()).IsProductManager(session.CompanyId, lblPackageProductCode.Text.Trim(), session.UserId, ref ds);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to edit package detail for this product!')", true);
                return;
            }

            TextBox txtEditProductDescription = (TextBox)e.Item.FindControl("txtEditProductDescription");
            TextBox txtEditQty = (TextBox)e.Item.FindControl("txtEditQty");
            HtmlInputHidden hidPackageID = (HtmlInputHidden)e.Item.FindControl("hidPackageID");
            HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");

            PackageProduct pkg = PackageProduct.RetrieveByKey(GMSUtil.ToInt(hidPackageID.Value.Trim()), hidProductCode.Value.Trim());
            if (pkg != null)
            {
                pkg.ProductDescription = txtEditProductDescription.Text.Trim();
                pkg.Qty = GMSUtil.ToDouble(txtEditQty.Text.Trim());
                pkg.Save();
                pkg.Resync();
                this.dgProduct.EditItemIndex = -1;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Update", "alert('Pacakge not found!')", true);
            }
            LoadData();
        }
        #endregion

        #region dgProduct_DeleteCommand
        protected void dgProduct_DeleteCommand(object sender, DataGridCommandEventArgs e)
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

            DataSet ds = new DataSet();
            (new GMSGeneralDALC()).IsProductManager(session.CompanyId, lblPackageProductCode.Text.Trim(), session.UserId, ref ds);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to edit package detail for this product!')", true);
                return;
            }

            HtmlInputHidden hidPackageID = (HtmlInputHidden)e.Item.FindControl("hidPackageID");
            HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");

            PackageProduct pkg = PackageProduct.RetrieveByKey(GMSUtil.ToInt(hidPackageID.Value.Trim()), hidProductCode.Value.Trim());
            if (pkg != null)
            {
                pkg.Delete();
                pkg.Resync();
                this.dgProduct.EditItemIndex = -1;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Delete", "alert('Pacakge not found!')", true);
            }
            LoadData();
        }
        #endregion

        #region dgDetail datagrid PageIndexChanged event handling
        protected void dgDetail_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        #region dgDetail_ItemCommand
        protected void dgDetail_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            #region Create
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

                DataSet ds = new DataSet();
                (new GMSGeneralDALC()).IsProductManager(session.CompanyId, lblPackageProductCode.Text.Trim(), session.UserId, ref ds);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to edit package detail for this product!')", true);
                    return;
                }

                TextBox txtNewProductDescription = (TextBox)e.Item.FindControl("txtNewProductDescription");

                try
                {
                    GMSCore.Entity.PackageDetail pkg = new GMSCore.Entity.PackageDetail();
                    pkg.CoyID = session.CompanyId;
                    pkg.PackageID = GMSUtil.ToInt(ViewState["PackageID"].ToString());

                    IList<GMSCore.Entity.PackageDetail> lstPackageDetail = null;
                    try
                    {
                        lstPackageDetail = new SystemDataActivity().RetrieveAllPackageDetailByPackageID(GMSUtil.ToInt(ViewState["PackageID"].ToString()));
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    }

                    byte maxDetailID = 0;
                    byte maxSeqID = 0;
                    if (lstPackageDetail != null && lstPackageDetail.Count > 0)
                    {
                        foreach (PackageDetail pDetail in lstPackageDetail)
                        {
                            if (pDetail.DetailID > maxDetailID)
                                maxDetailID = pDetail.DetailID;
                            if (pDetail.SeqID > maxSeqID)
                                maxSeqID = pDetail.SeqID.Value;
                        }
                        maxDetailID++;
                        maxSeqID++;
                    }

                    pkg.DetailID = maxDetailID;
                    pkg.Description = txtNewProductDescription.Text.Trim();
                    pkg.SeqID = maxSeqID;
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
            #endregion

            #region GoUp
            if (e.CommandName == "GoUp")
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

                DataSet ds = new DataSet();
                (new GMSGeneralDALC()).IsProductManager(session.CompanyId, lblPackageProductCode.Text.Trim(), session.UserId, ref ds);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to edit package detail for this product!')", true);
                    return;
                }

                HtmlInputHidden hidPackageID = (HtmlInputHidden)e.Item.FindControl("hidPackageID");
                HtmlInputHidden hidDetailID = (HtmlInputHidden)e.Item.FindControl("hidDetailID");

                PackageDetail pkg = PackageDetail.RetrieveByKey(GMSUtil.ToInt(hidPackageID.Value.Trim()), GMSUtil.ToByte(hidDetailID.Value.Trim()));
                if (pkg != null)
                {
                    IList<GMSCore.Entity.PackageDetail> lstPackageDetail = null;
                    try
                    {
                        lstPackageDetail = new SystemDataActivity().RetrieveAllPackageDetailByPackageID(GMSUtil.ToInt(ViewState["PackageID"].ToString()));
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    }

                    PackageDetail swatPackage = null;
                    if (lstPackageDetail != null && lstPackageDetail.Count > 0)
                    {
                        foreach (PackageDetail pDetail in lstPackageDetail)
                        {
                            if (pDetail.SeqID < pkg.SeqID)
                                swatPackage = pDetail;
                        }
                        if (swatPackage != null)
                        {
                            foreach (PackageDetail pDetail in lstPackageDetail)
                            {
                                if (pDetail.SeqID > swatPackage.SeqID && pDetail.SeqID < pkg.SeqID)
                                    swatPackage = pDetail;
                            }
                        }
                    }
                    if (swatPackage != null && swatPackage.SeqID < pkg.SeqID)
                    {
                        byte swat = pkg.SeqID.Value;
                        pkg.SeqID = swatPackage.SeqID.Value;
                        pkg.Save();
                        pkg.Resync();
                        swatPackage.SeqID = swat;
                        swatPackage.Save();
                        swatPackage.Resync();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Delete", "alert('Pacakge not found!')", true);
                }
                LoadData();
            }
            #endregion

            #region GoDown
            if (e.CommandName == "GoDown")
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

                DataSet ds = new DataSet();
                (new GMSGeneralDALC()).IsProductManager(session.CompanyId, lblPackageProductCode.Text.Trim(), session.UserId, ref ds);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to edit package detail for this product!')", true);
                    return;
                }

                HtmlInputHidden hidPackageID = (HtmlInputHidden)e.Item.FindControl("hidPackageID");
                HtmlInputHidden hidDetailID = (HtmlInputHidden)e.Item.FindControl("hidDetailID");

                PackageDetail pkg = PackageDetail.RetrieveByKey(GMSUtil.ToInt(hidPackageID.Value.Trim()), GMSUtil.ToByte(hidDetailID.Value.Trim()));
                if (pkg != null)
                {
                    IList<GMSCore.Entity.PackageDetail> lstPackageDetail = null;
                    try
                    {
                        lstPackageDetail = new SystemDataActivity().RetrieveAllPackageDetailByPackageID(GMSUtil.ToInt(ViewState["PackageID"].ToString()));
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    }

                    PackageDetail swatPackage = null;
                    if (lstPackageDetail != null && lstPackageDetail.Count > 0)
                    {
                        foreach (PackageDetail pDetail in lstPackageDetail)
                        {
                            if (pDetail.SeqID > pkg.SeqID)
                                swatPackage = pDetail;
                        }
                        if (swatPackage != null)
                        {
                            foreach (PackageDetail pDetail in lstPackageDetail)
                            {
                                if (pDetail.SeqID < swatPackage.SeqID && pDetail.SeqID > pkg.SeqID)
                                    swatPackage = pDetail;
                            }
                        }
                    }
                    if (swatPackage != null && swatPackage.SeqID > pkg.SeqID)
                    {
                        byte swat = pkg.SeqID.Value;
                        pkg.SeqID = swatPackage.SeqID.Value;
                        pkg.Save();
                        pkg.Resync();
                        swatPackage.SeqID = swat;
                        swatPackage.Save();
                        swatPackage.Resync();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Delete", "alert('Pacakge not found!')", true);
                }
                LoadData();
            }
            #endregion
        }
        #endregion

        #region dgDetail_EditCommand
        protected void dgDetail_EditCommand(object sender, DataGridCommandEventArgs e)
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

            DataSet ds = new DataSet();
            (new GMSGeneralDALC()).IsProductManager(session.CompanyId, lblPackageProductCode.Text.Trim(), session.UserId, ref ds);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to edit package detail for this product!')", true);
                return;
            }

            this.dgDetail.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }
        #endregion

        #region dgDetail_CancelCommand
        protected void dgDetail_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgDetail.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgDetail_UpdateCommand
        protected void dgDetail_UpdateCommand(object sender, DataGridCommandEventArgs e)
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

            DataSet ds = new DataSet();
            (new GMSGeneralDALC()).IsProductManager(session.CompanyId, lblPackageProductCode.Text.Trim(), session.UserId, ref ds);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to edit package detail for this product!')", true);
                return;
            }

            TextBox txtEditProductDescription = (TextBox)e.Item.FindControl("txtEditProductDescription");
            HtmlInputHidden hidPackageID = (HtmlInputHidden)e.Item.FindControl("hidPackageID");
            HtmlInputHidden hidDetailID = (HtmlInputHidden)e.Item.FindControl("hidDetailID");

            PackageDetail pkg = PackageDetail.RetrieveByKey(GMSUtil.ToInt(hidPackageID.Value.Trim()), GMSUtil.ToByte(hidDetailID.Value.Trim()));
            if (pkg != null)
            {
                pkg.Description = txtEditProductDescription.Text.Trim();
                pkg.Save();
                pkg.Resync();
                this.dgDetail.EditItemIndex = -1;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Update", "alert('Pacakge not found!')", true);
            }
            LoadData();
        }
        #endregion

        #region dgDetail_DeleteCommand
        protected void dgDetail_DeleteCommand(object sender, DataGridCommandEventArgs e)
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

            DataSet ds = new DataSet();
            (new GMSGeneralDALC()).IsProductManager(session.CompanyId, lblPackageProductCode.Text.Trim(), session.UserId, ref ds);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "no access", "alert('You do not have access to edit package detail for this product!')", true);
                return;
            }

            HtmlInputHidden hidPackageID = (HtmlInputHidden)e.Item.FindControl("hidPackageID");
            HtmlInputHidden hidDetailID = (HtmlInputHidden)e.Item.FindControl("hidDetailID");

            PackageDetail pkg = PackageDetail.RetrieveByKey(GMSUtil.ToInt(hidPackageID.Value.Trim()), GMSUtil.ToByte(hidDetailID.Value.Trim()));
            if (pkg != null)
            {
                pkg.Delete();
                pkg.Resync();
                this.dgDetail.EditItemIndex = -1;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Delete", "alert('Pacakge not found!')", true);
            }
            LoadData();
        }
        #endregion
    }
}
