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
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Products.Products
{
    public partial class ProductOrderDetail : GMSBasePage
    {
        private string productCode = "", type = "", productGroupCode = "", warehouse = "";
        protected short loginUserOrAlternateParty = 0;
        string isLargeFont, isOptimizedTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            this.productCode = Request.Params["PRODUCTCODE"];
            this.productGroupCode = Request.Params["PRODUCTGROUPCODE"];
            
            this.type = Request.Params["TYPE"];
            this.warehouse = Request.Params["WAREHOUSE"];


            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Product Search", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {

                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;

            //Getting LargerFont Cookies
            HttpCookie isLargeFontCookie = Request.Cookies["isLargeFont"];
            if (null == isLargeFontCookie)
                isLargeFont = "";
            else
                isLargeFont = isLargeFontCookie.Value == "true" ? "largeFont" : "";

            //Getting optimizedtable Cookies
            HttpCookie isOptimizedTableCookie = Request.Cookies["isOptimizedTable"];
            if (null == isOptimizedTableCookie)
                isOptimizedTable = "";
            else
                isOptimizedTable = isOptimizedTableCookie.Value == "true" ? "optimizedTable" : "";

            LoadData();
 
            //hidStatusType.Value = session.StatusType.ToString();

        }

        protected void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            hidStatusType.Value = session.StatusType.ToString();

            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();

            if (this.type == "SO")
            {
                try
                {
                    if ((session.StatusType.ToString() == "H" && (DateTime.Now <= (DateTime.Parse(session.LMSParallelRunEndDate)))) || session.StatusType.ToString() == "A")
                    {
                        GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                        if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                        {
                            sc.Url = session.WebServiceAddress.Trim();
                        }
                        else
                            sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                        ds = sc.GetProductOnSODetail(session.CompanyId, productCode);

                    }
                    else if ((session.StatusType.ToString() == "H" && (DateTime.Now > (DateTime.Parse(session.LMSParallelRunEndDate)))))
                    {
                        CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        {
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                        ds = sc1.GetProductOnSODetail(productCode);
                    }
                    else if (session.StatusType.ToString() == "L")
                    {
                        CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                            ds = sc1.GetProductOnSODetail(productCode);

                        if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                            sc1.Url = session.GASLMSWebServiceAddress.Trim();
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                        if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                            ds = sc1.GetProductOnSODetail(productCode);

                        if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                            sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                        if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                            ds1 = sc1.GetProductOnSODetail(productCode);

                    }
                    else if (session.StatusType.ToString() == "S")
                    { 
                        string query = "SELECT T1.\"DocNum\",T1.\"DocDate\",T0.\"Quantity\",T0.\"DelivrdQty\",T1.\"CardName\",T0.\"OpenCreQty\",T1.\"Comments\",T0.\"ItemCode\" FROM RDR1 T0 INNER JOIN ORDR T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" WHERE T0.\"LineStatus\" =\'O\' AND T0.\"ItemCode\" = '"+ productCode + "'";

                        SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                        ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                        "TrnNo", "TrnDate", "OrderQuantity", "DelQuantity", "AccountName", "Qty", "Narration", "ProductCode", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                       
                    }

                    if (session.StatusType.ToString() == "L" && ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {                            
                            ds.Tables[0].ImportRow(ds1.Tables[0].Rows[i]);                            
                        }
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        this.dgData.DataSource = ds;
                        this.dgData.DataBind();
                        
                    }
                    else
                    {
                        lblMsg.Text = "No record is found.";
                    }
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }
            else if (this.type == "BO")
            {
                try
                {
                    if ((session.StatusType.ToString() == "H" && (DateTime.Now <= (DateTime.Parse(session.LMSParallelRunEndDate)))) || session.StatusType.ToString() == "A")
                    {
                        GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                        if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                        {
                            sc.Url = session.WebServiceAddress.Trim();
                        }
                        else
                            sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                        ds = sc.GetProductOnBODetail(session.CompanyId, productCode);
                    }

                    else if ((session.StatusType.ToString() == "H" && (DateTime.Now > (DateTime.Parse(session.LMSParallelRunEndDate)))))
                    {
                        CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        {
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                        ds = sc1.GetProductOnBODetail(productCode);
                    }
                    else if (session.StatusType.ToString() == "L")
                    {
                        CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                            ds = sc1.GetProductOnBODetail(productCode);

                        if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                            sc1.Url = session.GASLMSWebServiceAddress.Trim();
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                        if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                            ds = sc1.GetProductOnBODetail(productCode);

                        if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                            sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                        if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                            ds1 = sc1.GetProductOnBODetail(productCode);

                    }

                    if (session.StatusType.ToString() == "L" && ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {
                            ds.Tables[0].ImportRow(ds1.Tables[0].Rows[i]);
                        }
                    }


                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        this.dgData.DataSource = ds;
                        this.dgData.DataBind();
                    }
                    else
                    {
                        lblMsg.Text = "No record is found.";
                    }
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }
            else if (this.type == "TN")
            {
                try
                {
                    if ((session.StatusType.ToString() == "H" && (DateTime.Now > (DateTime.Parse(session.LMSParallelRunEndDate)))) || session.StatusType.ToString() == "L")
                    {
                        CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        {
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                        ds = sc1.GetProductOnTNDetail(productCode);
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        this.dgData.DataSource = ds;
                        this.dgData.DataBind();
                    }
                    else
                    {
                        lblMsg.Text = "No record is found.";
                    }
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }
            else if (this.type == "Detail")
            {
                try
                {
                    string query = "CALL \"AF_API_GET_SAP_BP_STOCK_BALANCE\" ('" + warehouse + "', '" + productCode + "', '', '')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "Warehouse", "Product", "Customer", "CustomerName", "AccountGroup", "Quantity", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        this.dgDetail.DataSource = ds;
                        this.dgDetail.DataBind();
                    }
                    else
                    {
                        lblMsg.Text = "No record is found.";
                    }
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }
            else
            {
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
                        ds = sc.GetProductOnPODetail(session.CompanyId, productCode);

                        
                    }
                    else if (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S")
                    {
                        string query = "CALL \"AF_API_GET_SAP_PO_ON_PO_TRANSACTION\" ('','','','','" + productCode + "','" + productCode + "','','','','','')";
                        SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                        ds = sop.GET_SAP_QueryData(session.CompanyId, query, 
                        "PONo", "TrnNo", "OrderQuantity", "DelQuantity", "AccountName", "Qty", "DelDate", "DelMode", "CrtUser", "Narration", "TrnDate", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                       
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        this.dgPO.DataSource = ds;
                        this.dgPO.DataBind();
                    }
                    else
                    {
                        lblMsg.Text = "No record is found.";
                    }
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }
        }

        #region dgDetail_ItemDataBound
        protected void dgDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
        }
        #endregion

        #region dgData_ItemDataBound
        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();

            //(new GMSGeneralDALC()).IsProductManagerByProductGroupCode(session.CompanyId, this.productGroupCode, loginUserOrAlternateParty, ref ds1);
            ////Is product manager
            //if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            //{
            //    this.dgPO.Columns[3].Visible = true;
            //}
            
                Label lblTrnDate = (Label)e.Item.FindControl("lblTrnDate");
      
                if (lblTrnDate != null)
                {
                    try
                    {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //dd-MMM-yyyy  
                        if (ds.Tables[0].Rows[0]["TrnDate"].ToString() != "1/01/1900 12:00:00 AM")
                            lblTrnDate.Text = String.Format("{0:dd-MMM-yyyy}", ds.Tables[0].Rows[0]["TrnDate"]);
                    }
                    }
                    catch (Exception ex)
                    {
                        //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    }

                }
            
        }
        #endregion

        #region dgPO_ItemDataBound
        protected void dgPO_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();

            (new GMSGeneralDALC()).IsProductManagerByProductGroupCode(session.CompanyId, this.productGroupCode, loginUserOrAlternateParty, ref ds1);
            //Is product manager
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {               
                this.dgPO.Columns[3].Visible = true;
            }


            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblPONo = (Label)e.Item.FindControl("lblPONo");
                Label lblCRD = (Label)e.Item.FindControl("lblCRD");
                Label lblETD = (Label)e.Item.FindControl("lblETD");
                Label lblETA = (Label)e.Item.FindControl("lblETA");     
                Label lblRemarks = (Label)e.Item.FindControl("lblRemarks"); 
                
                if (lblPONo != null)
                {
                     try
                     {
                         dacl.GetProductDeliveryInfo(session.CompanyId, lblPONo.Text.ToString(), this.productCode,  ref ds);
                         if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                         {
                             //dd-MMM-yyyy  
                            if (ds.Tables[0].Rows[0]["CRD"].ToString() != "1/01/1900 12:00:00 AM")
                                lblCRD.Text = String.Format("{0:dd-MMM-yyyy}", ds.Tables[0].Rows[0]["CRD"]);
                            if (ds.Tables[0].Rows[0]["ETD"].ToString() != "1/01/1900 12:00:00 AM")
                                lblETD.Text = String.Format("{0:dd-MMM-yyyy}", ds.Tables[0].Rows[0]["ETD"]);
                            if (ds.Tables[0].Rows[0]["ETA"].ToString() != "1/01/1900 12:00:00 AM")
                                lblETA.Text = String.Format("{0:dd-MMM-yyyy}", ds.Tables[0].Rows[0]["ETA"]);

                            lblRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();

                        }
                     }
                     catch (Exception ex)
                     {
                         //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                     }
                   
                }
            }
        }
        #endregion
        protected void dgPOSAP_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();

            (new GMSGeneralDALC()).IsProductManagerByProductGroupCode(session.CompanyId, this.productGroupCode, loginUserOrAlternateParty, ref ds1);
            //Is product manager
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                this.dgPO.Columns[3].Visible = true;
            }


            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblPONo = (Label)e.Item.FindControl("lblPONo");
                Label lblCRD = (Label)e.Item.FindControl("lblCRD");
                Label lblETD = (Label)e.Item.FindControl("lblETD");
                Label lblETA = (Label)e.Item.FindControl("lblETA");
                Label lblRemarks = (Label)e.Item.FindControl("lblRemarks");

                if (lblPONo != null)
                {
                    try
                    {
                        dacl.GetProductDeliveryInfo(session.CompanyId, lblPONo.Text.ToString(), this.productCode, ref ds);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            //dd-MMM-yyyy  
                            if (ds.Tables[0].Rows[0]["CRD"].ToString() != "1/01/1900 12:00:00 AM")
                                lblCRD.Text = String.Format("{0:dd-MMM-yyyy}", ds.Tables[0].Rows[0]["CRD"]);
                            if (ds.Tables[0].Rows[0]["ETD"].ToString() != "1/01/1900 12:00:00 AM")
                                lblETD.Text = String.Format("{0:dd-MMM-yyyy}", ds.Tables[0].Rows[0]["ETD"]);
                            if (ds.Tables[0].Rows[0]["ETA"].ToString() != "1/01/1900 12:00:00 AM")
                                lblETA.Text = String.Format("{0:dd-MMM-yyyy}", ds.Tables[0].Rows[0]["ETA"]);



                            lblRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();

                        }
                    }
                    catch (Exception ex)
                    {
                        //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    }

                }
            }
        }

        public string getIsOptimizedTable
        {
            get { return isOptimizedTable; }
        }

        public string getIsLargeFont
        {
            get { return isLargeFont; }
        }
    }
}
