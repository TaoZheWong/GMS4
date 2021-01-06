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
using System.Data.SqlClient;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Sales.Sales
{
    public partial class SalesBreakdown : GMSBasePage
    {
        IList<ProductInfo> lstProductInfo = null; 

        [Serializable]
        public class ProductInfo
        {
            private string _prodCode = string.Empty;
            private string _prodDesc = string.Empty;
            private double _quantity = 0;
            private double _cost = 0; 
            private double _sellingPrice = 0;

            public ProductInfo()
            {}

            public ProductInfo(string prodCode, double quantity)
            {
                this._prodCode = prodCode;
                //this._prodDesc = prodDesc;
                this._quantity = quantity;
            }

            public void SetQuantity(double quantity)
            {
                this._quantity = quantity;
            }

            public void SetCost(double cost)
            {
                this._cost = cost; 
            }

            public void SetSellingPrice(double sellingPrice)
            {
                this._sellingPrice = sellingPrice;
            }
            
            public string ProdCode
            {
                get
                {
                    return _prodCode;
                }
            }
            
            public string ProdDesc
            {
                get
                {
                    return _prodDesc;
                }
            }

            public double Quantity
            {
                get
                {
                    return _quantity;
                }
            }

            public double SellingPrice
            {
                get
                {
                    return _sellingPrice;
                }
            }

            public double Cost
            {
                get
                {
                    return _cost; 
                }
            }
        } 

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Sales");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            100);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            100);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                //preload
                dgData.CurrentPageIndex = 0;
                lstProductInfo = new List<ProductInfo>();
                ViewState["lstProductInfo"] = lstProductInfo; 
                LoadData(); 
            }
        }
        
        #region LoadData
        private void LoadData()
        {
            lstProductInfo = (IList<ProductInfo>)ViewState["lstProductInfo"];

            LogSession session = base.GetSessionInfo();
            
            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lstProductInfo != null && lstProductInfo.Count > 0)
            {
                if (endIndex < lstProductInfo.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstProductInfo.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstProductInfo.Count.ToString() + " " + "of" + " " + lstProductInfo.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = lstProductInfo;
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
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            100);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

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
                                                                            100);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            lstProductInfo = (IList<ProductInfo>)ViewState["lstProductInfo"];

            HtmlInputHidden hidProdCode = (HtmlInputHidden)e.Item.FindControl("hidProdCode");
            TextBox txtEditQuantity = (TextBox)e.Item.FindControl("txtEditQuantity");

            //Todo: 
            //call webservice to get the cost of the product before insert into lstProdInfo 
            foreach (ProductInfo prodInfo in lstProductInfo)
            {
                if (prodInfo.ProdCode == GMSUtil.ToStr(hidProdCode.Value))
                {
                    prodInfo.SetQuantity(GMSUtil.ToDouble(txtEditQuantity.Text));
                    break; 
                }
            }
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
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                100);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                lstProductInfo = (IList<ProductInfo>)ViewState["lstProductInfo"]; 

                TextBox txtNewProdCode = (TextBox)e.Item.FindControl("txtNewProdCode");
                TextBox txtNewProdDesc = (TextBox)e.Item.FindControl("txtNewProdDesc");
                TextBox txtNewQuantity = (TextBox)e.Item.FindControl("txtNewQuantity");

                if (IsProdCodeExists(txtNewProdCode.Text.Trim()))
                {
                    this.PageMsgPanel.ShowMessage("Product Code is exists!", MessagePanelControl.MessageEnumType.Alert);
                    LoadData(); 
                    return; 
                }

                //Todo: 
                //call webservice to set the cost of the product. 
                ProductInfo prodInfo = new ProductInfo(txtNewProdCode.Text.Trim(), GMSUtil.ToDouble(txtNewQuantity.Text));
                double cost = 0;
                string prodCode = ""; 

                if (txtNewProdCode.Text.Trim().Length < 11)
                {
                    this.PageMsgPanel.ShowMessage("Product Code is not valid!", MessagePanelControl.MessageEnumType.Alert);
                    LoadData();
                    return;
                }
                else
                {
                    //prodCode = txtNewProdCode.Text.Trim().Substring(0, 11);
                    string[] singleProdCode = txtNewProdCode.Text.Trim().Split(' ');
                    if (singleProdCode[0].ToString() != "")
                        prodCode = singleProdCode[0].ToString();
                    
                }

                try
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    {
                        sc.Url = session.WebServiceAddress.Trim();
                    }
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";

                    if (sc.IsProductCodeValid(session.CompanyId, prodCode))
                    {
                        cost = sc.GetProductCostByProductCode(session.CompanyId, prodCode);
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

                prodInfo.SetCost(cost); 
                lstProductInfo.Add(prodInfo);
                LoadData(); 
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
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                100);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                lstProductInfo = (IList<ProductInfo>)ViewState["lstProductInfo"]; 

                HtmlInputHidden hidProdcode = (HtmlInputHidden)e.Item.FindControl("hidProdCode");

                foreach (ProductInfo prodInfo in lstProductInfo)
                {
                    if (prodInfo.ProdCode == GMSUtil.ToStr(hidProdcode.Value))
                    {
                        lstProductInfo.Remove(prodInfo);
                        break;
                    }
                }

                this.dgData.EditItemIndex = -1;
                this.dgData.CurrentPageIndex = 0;
                LoadData();
            }
        }
        #endregion

        #region IsProdCodeExists
        public bool IsProdCodeExists(string prodCode)
        {
            foreach(ProductInfo prodInfo in lstProductInfo)
            {
                if (prodInfo.ProdCode == prodCode) return true; 
            }
            return false; 
        }
        #endregion

        #region btnGenerate_Click
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            double totalCost = 0;
            double gp = 0;
            double unitPrice = 0; 
            double totalPrice = GMSUtil.ToDouble(txtTotalPrice.Text); 
          
            lstProductInfo = (IList<ProductInfo>)ViewState["lstProductInfo"];

            foreach (ProductInfo prodInfo in lstProductInfo)
            {
                totalCost += prodInfo.Quantity * prodInfo.Cost;             
            }

            gp = (totalPrice - totalCost) / totalCost;

            /*
            if (gp < 0 || gp == 0)
            {
                this.PageMsgPanel.ShowMessage("GP% is zero or negative!", MessagePanelControl.MessageEnumType.Alert);
            }
            */

            foreach (ProductInfo prodInfo in lstProductInfo)
            {
                unitPrice = Math.Round(prodInfo.Cost + (prodInfo.Cost * gp), 2); 
                prodInfo.SetSellingPrice(unitPrice); 
            }

            LoadData(); 
        }
        #endregion
    }
}
