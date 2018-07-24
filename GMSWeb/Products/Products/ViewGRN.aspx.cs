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

namespace GMSWeb.Products.Products
{
    public partial class ViewGRN : GMSBasePage
    {
        protected short loginUserOrAlternateParty = 0;


        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
           
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "MR", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {
                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());

                }
            }
            else
                loginUserOrAlternateParty = session.UserId;


            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
                                                                            121);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                

                if (Request.Params["MRNo"] != null && Request.Params["TrnNo"] != null)
                {
                    DataSet lstPOGRNAccess = new DataSet();

                    new GMSGeneralDALC().GetAccessToViewPOAndGRN(session.CompanyId, Request.Params["MRNo"], loginUserOrAlternateParty, "Y", ref lstPOGRNAccess);

                    if ((lstPOGRNAccess != null) && (lstPOGRNAccess.Tables[0].Rows.Count > 0))
                    {
                        hidGRNNo.Value = Request.Params["TrnNo"].ToString().Trim();
                        LoadData();
                    }
                }
                else
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

            }



        }
        #endregion

        #region LoadData
        private void LoadData()
        {

            LogSession session = base.GetSessionInfo();

            if (this.hidGRNNo.Value.Trim() == "")
            {
                base.JScriptAlertMsg("Please input a GRN No. to view.");
                return;
            }

            string trnNo = this.hidGRNNo.Value.Trim();
          
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

                    ds = sc.GetGRNHeaderFromA21(session.CompanyId, trnNo);
                }
                else if (session.StatusType.ToString() == "L")
                {
                    string query = "CALL \"AF_API_GET_SAP_GRN_HEADER\" ('" + trnNo + "', '" + trnNo + "', '', '')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());

                    ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "trnno", "TrnDate", "RefNo", "code", "cname", "Add1", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.lblGRNNo.Text = trnNo.ToString();
                    this.lblGRNDate.Text = string.Format("{0:dd/MM/yyyy}", ds.Tables[0].Rows[0]["TrnDate"]);
                    if (session.SAPStartDate.ToString() == "")
                    {
                        this.lblTrnNo.Text = ds.Tables[0].Rows[0]["trntype"].ToString() + ds.Tables[0].Rows[0]["trnno"].ToString();
                        this.lblAdd2.Text = ds.Tables[0].Rows[0]["Add2"].ToString();
                        this.lblAdd3.Text = ds.Tables[0].Rows[0]["Add3"].ToString();
                    }
                    else
                    {
                        this.lblTrnNo.Text = ds.Tables[0].Rows[0]["trnno"].ToString();
                    }
                    this.lblSupplier.Text = ds.Tables[0].Rows[0]["code"].ToString();
                    this.lblSupplierName.Text = ds.Tables[0].Rows[0]["cname"].ToString();
                    this.lblAdd1.Text = ds.Tables[0].Rows[0]["Add1"].ToString();
                    
                }
            }
            catch (Exception ex)
            {

            }            
           
            DataSet dsTemp = new DataSet();

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

                    dsTemp = sc.GetGRNDetailFromA21(session.CompanyId, trnNo);
                }
                else if (session.StatusType.ToString() == "L")
                {
                    string query = "CALL \"AF_API_GET_SAP_GRN_DETAIL\" ('" + trnNo + "', '" + trnNo + "', '', '')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    dsTemp = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "TrnNo", "TrnDate", "DetailNo", "PONo", "PODate", "ProductCode", "ProductDescription", "ProductGroupCode", "ProductGroupName", "UOM", "Quantity", "UnitPrice", "LandedCostUnitPrice", "Cost", "Currency", "WH", "ExchangeRate", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                }

                if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                {
                    this.dgData.DataSource = dsTemp;
                    this.dgData.DataBind();
                }


            }
            catch (Exception ex)
            {
                //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
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
            /*
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
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



            }
            */



        }
    }
}
