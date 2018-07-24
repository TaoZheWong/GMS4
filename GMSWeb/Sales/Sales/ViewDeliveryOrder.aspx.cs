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
using System.Collections.Generic;

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Text;
using System.Web.Services;
using AjaxControlToolkit;

namespace GMSWeb.Sales.Sales
{
    public partial class ViewDeliveryOrder : GMSBasePage
    {
        #region Page_Load
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
                                                                            115);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            115);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                //preload
                if (Request.Params["TrnNo"] != null)
                {
                    hidSOCode.Value = Request.Params["TrnNo"].ToString().Trim();
                    hidTrnType.Value = Request.Params["TrnType"].ToString().Trim();
                    hidDBVersion.Value = Request.Params["DBVersion"].ToString().Trim();
                    hidStatusType.Value = Request.Params["StatusType"].ToString().Trim();
                    LoadData();
                }
            }



        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            if (this.hidSOCode.Value.Trim() == "")
            {
                base.JScriptAlertMsg("Please input a sales order to view.");
                return;
            }

            string trnNo = this.hidSOCode.Value.Trim();
            string trnType = this.hidTrnType.Value.Trim();
            short dbVersion = GMSUtil.ToShort(this.hidDBVersion.Value.Trim());
            string statusType = this.hidStatusType.Value.Trim();


            string salesmanID = "'0'".ToString();

            GMSGeneralDALC dac2 = new GMSGeneralDALC();

            DataSet dsSalesPerson = new DataSet();

            try
            {
                dac2.GetSalesPersonListSelect(session.CompanyId, session.UserId, ref dsSalesPerson);
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
            }

            if (dsSalesPerson != null && dsSalesPerson.Tables.Count > 0 && dsSalesPerson.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsSalesPerson.Tables[0].Rows)
                {
                    salesmanID += ",'" + dr["salespersonid"].ToString() + "'";
                }
            }



            ProductsDataDALC dacl = new ProductsDataDALC();
            DataSet ds = new DataSet();


            try
            {
                if (statusType == "A21")
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    {
                        sc.Url = session.WebServiceAddress.Trim();
                    }
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                    ds = sc.GetDeliveryOrderByTrnNo(session.CompanyId, trnNo, trnType, salesmanID, dbVersion);
                }
                else if (statusType != "A21")
                {
                    DateTime LMSParallelRunEndDate = GMSUtil.ToDate(session.LMSParallelRunEndDate);
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                    {
                        // sc1.Url = session.CMSWebServiceAddress.Trim();
                        string tmpURL = session.CMSWebServiceAddress.Trim();
                        var valueArray = session.CMSWebServiceAddress.Trim().Split('/');
                        sc1.Url = tmpURL.Replace(valueArray[3], statusType + ".WebServices");
                    }
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    ds = sc1.GetDeliveryOrderByTrnNo(trnNo, trnType, salesmanID, dbVersion);
                }
               
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            double gst = 0;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.lblTrnNo.Text = ds.Tables[0].Rows[0]["TrnNo"].ToString();
                this.lblAccountCode.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString();
                this.lblAccountName.Text = ds.Tables[0].Rows[0]["AccountName"].ToString();
                this.lblCustPONo.Text = ds.Tables[0].Rows[0]["CustPONo"].ToString();
                //this.lblStatus.Text = ds.Tables[0].Rows[0]["status"].ToString();
                this.lblTrnDate.Text = string.Format("{0:dd/MM/yyyy}", ds.Tables[0].Rows[0]["TrnDate"]);
                this.lblSalesPerson.Text = ds.Tables[0].Rows[0]["salespersonname"].ToString();
                this.lblDONo.Text = ds.Tables[0].Rows[0]["docno"].ToString();
                this.txtAddress1.Text = ds.Tables[0].Rows[0]["DeliveryAddress1"].ToString();
                this.txtAddress2.Text = ds.Tables[0].Rows[0]["DeliveryAddress2"].ToString();
                this.txtAddress3.Text = ds.Tables[0].Rows[0]["DeliveryAddress3"].ToString();
                this.txtAddress4.Text = ds.Tables[0].Rows[0]["DeliveryAddress4"].ToString();
                this.txtNarration.Text = ds.Tables[0].Rows[0]["Narration"].ToString();
                this.txtSONo.Text = ds.Tables[0].Rows[0]["SONo"].ToString();
                this.txtMobilePhone.Text = ds.Tables[0].Rows[0]["mobile"].ToString();
                this.txtOfficePhone.Text = ds.Tables[0].Rows[0]["OfficePhone"].ToString();
                this.txtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                this.txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                this.txtAttentionTo.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                this.txtTaxRate.Text = string.Format("{0:P2}", ds.Tables[0].Rows[0]["gst"]);
                this.lblCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                this.lblCurrency2.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                this.lblCurrency3.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                //this.lblSubTotal.Text = string.Format("{0:0.00}", ds.Tables[0].Rows[0]["billamt"]);

                gst = (double)ds.Tables[0].Rows[0]["gst"];
              

                //double subtotal = billamt / (gst + 1);
               
                
                

                //this.lblCreatedBy.Text = ds.Tables[0].Rows[0]["crtuser"].ToString();
                //this.lblModifiedBy.Text = ds.Tables[0].Rows[0]["upduser"].ToString();


            }

            DataSet dsTemp = new DataSet();

            try
            {
                if (statusType == "A21")
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    {
                        sc.Url = session.WebServiceAddress.Trim();
                    }
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                    dsTemp = sc.GetDeliveryOrderDetails(session.CompanyId, trnNo, trnType, salesmanID, dbVersion);
                }
                else if (statusType != "A21")
                {
                    DateTime LMSParallelRunEndDate = GMSUtil.ToDate(session.LMSParallelRunEndDate);
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                    {
                        //sc1.Url = session.CMSWebServiceAddress.Trim();
                        string tmpURL = session.CMSWebServiceAddress.Trim();
                        var valueArray = session.CMSWebServiceAddress.Trim().Split('/');
                        sc1.Url = tmpURL.Replace(valueArray[3], statusType + ".WebServices");
                    }
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                    dsTemp = sc1.GetDeliveryOrderDetails(trnNo, trnType, salesmanID, dbVersion);

                }
                
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            double totalItemAmount = 0;

            if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {

                    double discount = 0;
                    Double.TryParse(dsTemp.Tables[0].Rows[i]["discount"].ToString(), out discount);
                    totalItemAmount += (((double)dsTemp.Tables[0].Rows[i]["unitprice"] * (double)dsTemp.Tables[0].Rows[i]["quantity"]) - ((double)dsTemp.Tables[0].Rows[i]["unitprice"] * (double)dsTemp.Tables[0].Rows[i]["quantity"] * discount));

                }

                this.lblSubTotal.Text = string.Format("{0:0.00}", totalItemAmount);
                this.lblTaxAmount.Text = string.Format("{0:0.00}", gst * totalItemAmount);
                this.lblGrandTotal.Text = string.Format("{0:0.00}", totalItemAmount * (gst + 1));


                this.dgData.DataSource = dsTemp;
                this.dgData.DataBind();
            }
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

        #region dgData2 datagrid PageIndexChanged event handling
        protected void dgData2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                HtmlInputHidden hidDoNo = (HtmlInputHidden)e.Item.FindControl("hidDoNo");
                HtmlInputHidden hidSrNo = (HtmlInputHidden)e.Item.FindControl("hidSrNo");
                HtmlInputHidden hidDOType = (HtmlInputHidden)e.Item.FindControl("hidDOType");
                Label lblProdDetailDesc = (Label)e.Item.FindControl("lblProdDetailDesc");

                Label lblUnitPrice = (Label)e.Item.FindControl("lblUnitPrice");
                Label lblOrderQuantity = (Label)e.Item.FindControl("lblOrderQuantity");
                Label lblDiscount = (Label)e.Item.FindControl("lblDiscount");
                Label lblAmount = (Label)e.Item.FindControl("lblAmount");
                double qty = 0;
                double discount = 0.00;
                double unitprice = 0.00;
                Double.TryParse(lblOrderQuantity.Text, out  qty);
                Double.TryParse(lblUnitPrice.Text, out unitprice);
                Double.TryParse(lblDiscount.Text, out discount);
                double amount = (qty * unitprice) - ((qty * unitprice * discount));
                lblAmount.Text = String.Format("{0:0.00}", amount);

                DataSet ds = new DataSet();

                try
                {
                    string statusType = this.hidStatusType.Value.Trim();
                    if (statusType == "A21")
                    {
                        GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                        if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                        {
                            sc.Url = session.WebServiceAddress.Trim();
                        }
                        else
                            sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                        ds = sc.GetDeliveryProductDesc(session.CompanyId, hidSOCode.Value, hidTrnType.Value, hidSrNo.Value, GMSUtil.ToShort(hidDBVersion));
                    }
                    else if (statusType == "LMS")
                    {
                        DateTime LMSParallelRunEndDate = GMSUtil.ToDate(session.LMSParallelRunEndDate);
                        CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        {
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                        ds = sc1.GetDeliveryProductDesc(hidSOCode.Value, hidTrnType.Value, hidSrNo.Value, GMSUtil.ToShort(hidDBVersion));
                    }
                    
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                }

                string ProdDetailDesc = "";

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ProdDetailDesc == "")
                            ProdDetailDesc = ds.Tables[0].Rows[i]["Description"].ToString();
                        else
                            ProdDetailDesc = ProdDetailDesc + "<br />" + ds.Tables[0].Rows[i]["Description"].ToString();
                    }

                }

                lblProdDetailDesc.Text = ProdDetailDesc;

            }


        }
    }
}
