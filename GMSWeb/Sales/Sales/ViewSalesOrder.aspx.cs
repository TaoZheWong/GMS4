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
    public partial class ViewSalesOrder : GMSBasePage
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
                                                                            114);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            114);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                //preload

                if (Request.Params["TrnNo"] != null)
                {
                    hidSOCode.Value = Request.Params["TrnNo"].ToString().Trim();
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
                if (session.StatusType.ToString() == "A")
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    {
                        sc.Url = session.WebServiceAddress.Trim();
                    }
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                    ds = sc.GetSalesOrderByTrnNo(session.CompanyId, trnNo, salesmanID, dbVersion);

                    System.Data.DataColumn newColumn = new System.Data.DataColumn("SOStatusName", typeof(string));
                    newColumn.DefaultValue = "";
                    ds.Tables[0].Columns.Add(newColumn);
                }
                else if (session.StatusType.ToString() == "L")
                {
                    DateTime LMSParallelRunEndDate = GMSUtil.ToDate(session.LMSParallelRunEndDate);
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                    {                      
                        string tmpURL = session.CMSWebServiceAddress.Trim();
                        var valueArray = session.CMSWebServiceAddress.Trim().Split('/');    
                        sc1.Url = tmpURL.Replace(valueArray[3], statusType+ ".WebServices");                        
                    }
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                    ds = sc1.GetSalesOrderByTrnNo(trnNo, salesmanID, 0);
                }
                else if (session.StatusType.ToString() == "S")
                {
                    string query = "CALL \"AF_API_GET_SAP_GMS_SO_HEADER\" ('', '', '', '', '', '', '', '', '', '', '" + trnNo + "', '')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());

                    ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "TrnNo", "TrnDate", "SOStatusName", "crtuser", "custponumber", "AccountName", "AccountCode", "salespersonname", "salesperson", "narration", "DeliveryAddress1", "OrderBy", "Currency", "RefNo1", "RefNo2", "RefNo3", "gst", "discount", "billamt", "GrandTotal",
                    "mobile", "OfficePhone", "fax", "email", "contact", "upduser", "Field27", "Field28", "Field29", "Field30");
                    
                    
                }
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.lblSOStatus.Text = ds.Tables[0].Rows[0]["SOStatusName"].ToString();
                this.lblTrnNo.Text = ds.Tables[0].Rows[0]["TrnNo"].ToString();
                this.lblAccountCode.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString();
                this.lblAccountName.Text = ds.Tables[0].Rows[0]["AccountName"].ToString();
                //this.lblStatus.Text = ds.Tables[0].Rows[0]["status"].ToString();
                this.lblTrnDate.Text = string.Format("{0:dd/MM/yyyy}", ds.Tables[0].Rows[0]["TrnDate"]);
                this.lblPONo.Text = ds.Tables[0].Rows[0]["custponumber"].ToString();
                this.lblSalesPerson.Text = ds.Tables[0].Rows[0]["salespersonname"].ToString();
                this.txtCustomerPONo.Text = ds.Tables[0].Rows[0]["custponumber"].ToString();                
                this.txtAddress1.Text = ds.Tables[0].Rows[0]["DeliveryAddress1"].ToString();
                if (session.StatusType.ToString() != "S")
                {
                    this.txtAddress2.Text = ds.Tables[0].Rows[0]["DeliveryAddress2"].ToString();
                    this.txtAddress3.Text = ds.Tables[0].Rows[0]["DeliveryAddress3"].ToString();
                    this.txtAddress4.Text = ds.Tables[0].Rows[0]["DeliveryAddress4"].ToString();
                }
                this.txtNarration.Text = ds.Tables[0].Rows[0]["Narration"].ToString();
                this.txtMobilePhone.Text = ds.Tables[0].Rows[0]["mobile"].ToString();
                this.txtOfficePhone.Text = ds.Tables[0].Rows[0]["OfficePhone"].ToString();
                this.txtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                this.txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                this.txtAttentionTo.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                this.txtTaxRate.Text = string.Format("{0:P2}",ds.Tables[0].Rows[0]["gst"]);
                this.lblCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                this.lblCurrency2.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                this.lblCurrency3.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                if (session.StatusType.ToString() == "S")
                    this.lblSubTotal.Text = string.Format("{0:0.00}", Convert.ToDouble(ds.Tables[0].Rows[0]["billamt"]));
                else
                    this.lblSubTotal.Text = string.Format("{0:0.00}", ds.Tables[0].Rows[0]["billamt"]);

                //double gst = (double)ds.Tables[0].Rows[0]["gst"];
                double gst = Convert.ToDouble(ds.Tables[0].Rows[0]["gst"]);

                if (session.StatusType.ToString() == "S")
                    gst = gst / 100;
                
                //double billamt = (double)ds.Tables[0].Rows[0]["billamt"];     
                double billamt = Convert.ToDouble(ds.Tables[0].Rows[0]["billamt"]);
                this.lblTaxAmount.Text = string.Format("{0:0.00}", gst * billamt);
                if (session.StatusType.ToString() == "S")
                    this.lblGrandTotal.Text = string.Format("{0:0.00}", Convert.ToDouble(ds.Tables[0].Rows[0]["GrandTotal"]));
                else
                    this.lblGrandTotal.Text = string.Format("{0:0.00}", billamt + (gst * billamt));

                //this.lblCreatedBy.Text = ds.Tables[0].Rows[0]["crtuser"].ToString();
                //this.lblModifiedBy.Text = ds.Tables[0].Rows[0]["upduser"].ToString();
                
                
            }

            DataSet dsTemp = new DataSet();

            try
            {
                if (session.StatusType.ToString() == "A")
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    {
                        sc.Url = session.WebServiceAddress.Trim();
                    }
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                    dsTemp = sc.GetSalesOrderDetails(session.CompanyId, trnNo, salesmanID, dbVersion);
                }
                else if (session.StatusType.ToString() == "L")
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

                    dsTemp = sc1.GetSalesOrderDetails(trnNo, salesmanID, dbVersion); 
                }
                else if (session.StatusType.ToString() == "S")
                {                   
                    string query = "CALL \"AF_API_GET_SAP_GMS_SO_DETAIL\" ('" + trnNo + "')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    dsTemp = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "ProductCode", "ProductDescription", "clearflag", "Currency", "uom", "TrnNo", "UnitPrice", "Discount", "SODate","OrderQuantity", "AccountCode", "AccountName", "SrNo", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");  
                }

            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
            {
                
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
            string statusType = this.hidStatusType.Value.Trim();
            
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {                
                    HtmlInputHidden hidSrNo = (HtmlInputHidden)e.Item.FindControl("hidSrNo");
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
                    Double.TryParse(lblDiscount .Text, out discount);
                    double amount = (qty * unitprice) - ((qty * unitprice *discount));
                    lblAmount.Text = String.Format("{0:0.00}", amount);

                    DataSet ds = new DataSet();

                    try
                    {
                        if (session.StatusType.ToString() == "A")
                        {
                            GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                            if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                            {
                                sc.Url = session.WebServiceAddress.Trim();
                            }
                            else
                                sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                            ds = sc.GetSalesOrderProductDesc(session.CompanyId, hidSOCode.Value, "CO", hidSrNo.Value, GMSUtil.ToShort(hidDBVersion));
                        }
                        else if (session.StatusType.ToString() == "L")
                        {
                            DateTime LMSParallelRunEndDate = GMSUtil.ToDate(session.LMSParallelRunEndDate);
                            CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                            if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                            {
                                sc1.Url = session.CMSWebServiceAddress.Trim();
                            }
                            else
                                sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                            ds = sc1.GetSalesOrderProductDesc(hidSOCode.Value, hidSrNo.Value, GMSUtil.ToShort(hidDBVersion));

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
