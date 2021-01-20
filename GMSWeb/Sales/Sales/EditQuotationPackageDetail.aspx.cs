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
using AjaxControlToolkit;
using System.Text;
using System.Web.Services;

namespace GMSWeb.Sales.Sales
{
    public partial class EditQuotationPackageDetail : GMSBasePage
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
                                                                            105);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                if (Request.Params["QuotationNo"] != null)
                {
                    ViewState["QuotationNo"] = Request.Params["QuotationNo"].ToString().Trim();
                }
                if (Request.Params["SNNo"] != null)
                {
                    ViewState["SNNo"] = Request.Params["SNNo"].ToString().Trim();
                }
                ViewState["RevisionNo"] = Request.Params["RevisionNo"].ToString().Trim();
                //preload
                this.dgProduct.CurrentPageIndex = 0;
                LoadData();
            }

            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>
                <script language=""javascript"" type=""text/javascript"">
                    var txtNewProductCode_id;
                    function SearchProduct(ctl)
                    {	
                        txtNewProductCode_id = ctl.id.replace('lnkFindProduct', 'txtNewProductCode');
                        var acctCode = document.getElementById('" + hidAccountCode.ClientID + @"').value; 
                        if (acctCode != """")
                        {		
	                        var url = 'ProductSearch.aspx?AccountCode=' + acctCode; 
                            window.open(url,"""",""width="" + 650 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
                        }
                        else
                        {
                            var url = 'ProductSearch.aspx?AccountCode=xxx'; 
                            window.open(url,"""",""width="" + 650 + "",height="" + 400 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");
                        }
	                    return false;
                    }	

                    function SetSelectedProductCode(productCode)
                    {
                        if(productCode != null)
                        {					
	                        document.getElementById(txtNewProductCode_id).value = productCode.replace(/^\s+|\s+$/g, '');
                            if (document.getElementById(txtNewProductCode_id).value != """")
                            {	
                                if (document.all)			
                                {	
                                    //IE			
                                    document.getElementById(txtNewProductCode_id).fireEvent('onchange');
                                }
                                else
                                {
                                    //FireFox
                                    var e = document.createEvent('HTMLEvents');
                                    e.initEvent('change', false, false);
                                    document.getElementById(txtNewProductCode_id).dispatchEvent(e);
                                }
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

            QuotationPackage qp = QuotationPackage.RetrieveByKey(session.CompanyId, ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["SNNo"]), GMSUtil.ToByte(ViewState["RevisionNo"]));
            lblPackageProductCode.Text = qp.ProductCode;
            lblPackageProductDescription.Text = qp.ProductDescription;
            txtEditUnitPrice.Text = qp.UnitPrice.Value.ToString("#0.00");
            lblUnitPackagePrice.Text = qp.UnitPackagePrice.Value.ToString("#0.00");
            QuotationHeader qh = QuotationHeader.RetrieveByKey(session.CompanyId, ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["RevisionNo"]));
            hidCurrencyRate.Value = qh.CurrencyRate.ToString();
            hidAccountCode.Value = qh.AccountCode;

            IList<GMSCore.Entity.QuotationPackageProduct> lstPackageProduct = null;
            try
            {
                lstPackageProduct = new SystemDataActivity().RetrieveAllPackageProductByQuotationNoSNNo(session.CompanyId,
                                         ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["SNNo"]), GMSUtil.ToByte(ViewState["RevisionNo"]));
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            this.dgProduct.DataSource = lstPackageProduct;
            this.dgProduct.DataBind();
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
                                                                           102);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                if (ViewState["RevisionNo"].ToString() != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('You are not allowed to edit this revision!')", true);
                    return;
                }

                TextBox txtNewProductCode = (TextBox)e.Item.FindControl("txtNewProductCode");
                TextBox txtNewProductDescription = (TextBox)e.Item.FindControl("txtNewProductDescription");
                TextBox txtNewQty = (TextBox)e.Item.FindControl("txtNewQty");
                TextBox txtNewUnitPrice = (TextBox)e.Item.FindControl("txtNewUnitPrice");

                try
                {
                    GMSCore.Entity.QuotationPackageProduct qpp = new GMSCore.Entity.QuotationPackageProduct();
                    qpp.CoyID = session.CompanyId;
                    qpp.QuotationNo = ViewState["QuotationNo"].ToString();
                    qpp.SNNo = GMSUtil.ToByte(ViewState["SNNo"]);
                    qpp.ProductCode = txtNewProductCode.Text.Trim();
                    qpp.ProductDescription = txtNewProductDescription.Text.Trim();
                    qpp.Qty = GMSUtil.ToDouble(txtNewQty.Text.Trim());
                    qpp.UnitPrice = GMSUtil.ToDecimal(txtNewUnitPrice.Text.Trim());
                    qpp.CreatedBy = session.UserId;
                    qpp.CreatedDate = DateTime.Now;
                    qpp.RevisionNo = 0;

                    decimal currencyRate = 1;
                    if (hidCurrencyRate.Value.Trim() != "")
                    {
                        currencyRate = GMSUtil.ToDecimal(hidCurrencyRate.Value.Trim());
                    }
                    if (currencyRate == 0) currencyRate = 1;
                    ProductsDataDALC dalc = new ProductsDataDALC();
                    DataSet ds = new DataSet();
                    dalc.GetProductPrice(session.CompanyId, txtNewProductCode.Text.Trim(), session.UserId, ref ds);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        qpp.Cost = GMSUtil.ToDecimal(ds.Tables[0].Rows[0]["WeightedCost"].ToString()) / currencyRate;
                    }
                    else
                    {
                        qpp.Cost = 0;
                    }

                    qpp.Save();
                    qpp.Resync();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "click", "alert('" + ex.Message + "')", true);
                }

                RecalculatePackagePrice();
                LoadData();
            }
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
                                                                            102);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (ViewState["RevisionNo"].ToString() != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('You are not allowed to edit this revision!')", true);
                return;
            }

            //TextBox txtEditProductDescription = (TextBox)e.Item.FindControl("txtEditProductDescription");
            TextBox txtEditQty = (TextBox)e.Item.FindControl("txtEditQty");
            TextBox txtEditUnitPrice = (TextBox)e.Item.FindControl("txtEditUnitPrice");
            HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");

            QuotationPackageProduct qpp = QuotationPackageProduct.RetrieveByKey(session.CompanyId, ViewState["QuotationNo"].ToString(),
                                            hidProductCode.Value.Trim(), GMSUtil.ToByte(ViewState["SNNo"]), 0);
            if (qpp != null)
            {
                //qpp.ProductDescription = txtEditProductDescription.Text.Trim();
                qpp.Qty = GMSUtil.ToDouble(txtEditQty.Text.Trim());
                qpp.UnitPrice = GMSUtil.ToDecimal(txtEditUnitPrice.Text.Trim());
                qpp.Save();
                qpp.Resync();
                this.dgProduct.EditItemIndex = -1;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Update", "alert('Pacakge product not found!')", true);
            }

            RecalculatePackagePrice();
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
                                                                            102);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (ViewState["RevisionNo"].ToString() != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('You are not allowed to edit this revision!')", true);
                return;
            }

            HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");

            QuotationPackageProduct qpp = QuotationPackageProduct.RetrieveByKey(session.CompanyId, ViewState["QuotationNo"].ToString(),
                                            hidProductCode.Value.Trim(), GMSUtil.ToByte(ViewState["SNNo"]), 0);
            if (qpp != null)
            {
                qpp.Delete();
                qpp.Resync();
                this.dgProduct.EditItemIndex = -1;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(PageMsgPanel, this.GetType(), "Update", "alert('Pacakge product not found!')", true);
            }

            RecalculatePackagePrice();
            LoadData();
        }
        #endregion

        #region dgProduct_ItemDataBound
        protected void dgProduct_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PopupControlExtender pce = e.Item.FindControl("PopupControlExtender1") as PopupControlExtender;

                string behaviorID = "pce_" + e.Item.ItemIndex;
                pce.BehaviorID = behaviorID;

                Image img = (Image)e.Item.FindControl("imgMagnify");

                string OnMouseOverScript = string.Format("$find('{0}').showPopup();", behaviorID);
                string OnMouseOutScript = string.Format("$find('{0}').hidePopup();", behaviorID);

                img.Attributes.Add("onmouseover", OnMouseOverScript);
                img.Attributes.Add("onmouseout", OnMouseOutScript); 

                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region btnGenerate_Click
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            102);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (ViewState["RevisionNo"].ToString() != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('You are not allowed to edit this revision!')", true);
                return;
            }

            decimal totalCost = 0;
            decimal gp = 0;
            decimal unitPrice = 0;
            decimal totalPrice = GMSUtil.ToDecimal(txtTotalPackagePrice.Text);

            QuotationPackage qp = QuotationPackage.RetrieveByKey(session.CompanyId, ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["SNNo"]), 0);
            IList<GMSCore.Entity.QuotationPackageProduct> lstPackageProduct = null;
            try
            {
                lstPackageProduct = new SystemDataActivity().RetrieveAllPackageProductByQuotationNoSNNo(session.CompanyId,
                                         ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["SNNo"]), 0);

                GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                {
                    sc.Url = session.WebServiceAddress.Trim();
                }
                else
                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                foreach (QuotationPackageProduct qpp in lstPackageProduct)
                {
                    if (sc.IsProductCodeValid(session.CompanyId, qpp.ProductCode))
                    {
                        decimal cost = GMSUtil.ToDecimal(sc.GetProductCostByProductCode(session.CompanyId, qpp.ProductCode));
                        totalCost += GMSUtil.ToDecimal(qpp.Qty) * cost;
                        qpp.UnitPrice = GMSUtil.ToDecimal(cost); //temporarily borrow unit price property to store cost
                    }
                    else
                    {
                        this.PageMsgPanel.ShowMessage("Product Code is not valid", MessagePanelControl.MessageEnumType.Alert);
                        LoadData();
                        return;
                    }
                }

                if (sc.IsProductCodeValid(session.CompanyId, qp.ProductCode))
                {
                    decimal cost = GMSUtil.ToDecimal(sc.GetProductCostByProductCode(session.CompanyId, qp.ProductCode));
                    totalCost += cost;
                    qp.UnitPrice = GMSUtil.ToDecimal(cost); //temporarily borrow unit price property to store cost
                }
                else
                {
                    this.PageMsgPanel.ShowMessage("Product Code is not valid", MessagePanelControl.MessageEnumType.Alert);
                    LoadData();
                    return;
                }
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            gp = (totalPrice - totalCost) / totalCost;

            /*
            if (gp < 0 || gp == 0)
            {
                this.PageMsgPanel.ShowMessage("GP% is zero or negative!", MessagePanelControl.MessageEnumType.Alert);
            }
            */

            foreach (QuotationPackageProduct qpp in lstPackageProduct)
            {
                unitPrice = Math.Round(qpp.UnitPrice.Value + (qpp.UnitPrice.Value * gp), 2);
                qpp.UnitPrice = unitPrice;
                qpp.Save();
                qpp.Resync();
            }

            unitPrice = Math.Round(qp.UnitPrice.Value + (qp.UnitPrice.Value * gp), 2);
            qp.UnitPrice = unitPrice;
            qp.Save();
            qp.Resync();

            RecalculatePackagePrice();
            LoadData();
        }
        #endregion

        #region RecalculatePackagePrice
        protected void RecalculatePackagePrice()
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            102);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (ViewState["RevisionNo"].ToString() != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('You are not allowed to edit this revision!')", true);
                return;
            }

            QuotationPackage qp = QuotationPackage.RetrieveByKey(session.CompanyId, ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["SNNo"]), 0);
            if (qp != null)
            {
                qp.UnitPackagePrice = qp.UnitPrice;
                qp.UnitPackageCost = qp.Cost;
                IList<GMSCore.Entity.QuotationPackageProduct> lstPackageProduct = null;
                try
                {
                    lstPackageProduct = new SystemDataActivity().RetrieveAllPackageProductByQuotationNoSNNo(session.CompanyId,
                                             ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["SNNo"]), 0);

                    foreach (QuotationPackageProduct qpp in lstPackageProduct)
                    {
                        qp.UnitPackagePrice = qp.UnitPackagePrice.Value + ((decimal)qpp.Qty * qpp.UnitPrice.Value);
                        qp.UnitPackageCost = qp.UnitPackageCost + ((decimal)qpp.Qty * qpp.Cost);
                    }
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                }

                qp.Save();
                qp.Resync();
            }
        }
        #endregion

        #region txtEditUnitPrice_OnTextChanged
        protected void txtEditUnitPrice_OnTextChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            102);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (ViewState["RevisionNo"].ToString() != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('You are not allowed to edit this revision!')", true);
                return;
            }

            QuotationPackage qp = QuotationPackage.RetrieveByKey(session.CompanyId, ViewState["QuotationNo"].ToString(), GMSUtil.ToByte(ViewState["SNNo"]), 0);
            qp.UnitPrice = GMSUtil.ToDecimal(txtEditUnitPrice.Text.Trim());
            qp.Save();
            qp.Resync();
            RecalculatePackagePrice();
            LoadData();
        }
        #endregion

        #region GetDynamicContent
        [WebMethod]
        public static string GetDynamicContent(string contextKey)
        {
            short coyId = 1;
            string prodCode = "";
            string[] str = contextKey.Split(';');
            if (str != null && str.Length == 2)
            {
                coyId = GMSUtil.ToShort(str[0].Trim());
                prodCode = str[1].Trim();
            }

            DataSet ds = new DataSet();
            GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
            Company coy = Company.RetrieveByKey(coyId);
            if (coy.WebServiceAddress != null && coy.WebServiceAddress.Trim() != "")
            {
                sc.Url = coy.WebServiceAddress.Trim();
            }
            else
                sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
            ds = sc.GetProductStockStatus(coyId, prodCode);

            StringBuilder b = new StringBuilder();

            //b.Append("<table style='background-color:#f3f3f3; border: #336699 3px solid; ");
            b.Append("<table class='tInfoTable'>");
            b.Append("<tr><td colspan='2' style='background-color:#FAD696;'>");
            b.Append("<b><i>Stock Status For Product " + prodCode + "</i></b>");
            b.Append("</td></tr>");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                b.Append("<tr><td style='width:250px;'><i>Warehouse</i></td>");
                b.Append("<td><i>Quantity</i></td></tr>");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    b.Append("<tr>");
                    b.Append("<td><i>" + dr["Warehouse"].ToString() + " - " + dr["WarehouseName"].ToString() + "</i></td>");
                    b.Append("<td><i>" + dr["Quantity"].ToString() + "</i></td>");
                    b.Append("</tr>");
                }
            }
            else
            {
                b.Append("<tr>");
                b.Append("<td colspan='2'><i>Not Available.</i></td>");
                b.Append("</tr>");
            }
           
            b.Append("</table>");

            return b.ToString();
        }
        #endregion
    }
}
