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
    public partial class ViewGRNWithoutSupplierInfo : GMSBasePage
    {
        protected short loginUserOrAlternateParty = 0;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {


            LogSession session = base.GetSessionInfo();

            string currentLink = "Products";
            //lblPageHeader.Text = "Products";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
            }


            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
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
                                                                            120);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!Page.IsPostBack)
            {
                //preload
                
                if (Request.Params["MRNo"] != null && Request.Params["TrnNo"] != null)
                {
                    DataSet lstPOGRNAccess = new DataSet();

                    new GMSGeneralDALC().GetAccessToViewPOAndGRN(session.CompanyId, Request.Params["MRNo"], loginUserOrAlternateParty, "N", ref lstPOGRNAccess);

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
                GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                {
                    sc.Url = session.WebServiceAddress.Trim();
                }
                else
                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";

                ds = sc.GetGRNHeaderFromA21(session.CompanyId, trnNo);

            }
            catch (Exception ex)
            {

            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.lblGRNNo.Text = trnNo.ToString();
                this.lblGRNDate.Text = string.Format("{0:dd/MM/yyyy}", ds.Tables[0].Rows[0]["TrnDate"]);
                this.lblTrnNo.Text = ds.Tables[0].Rows[0]["trntype"].ToString() + ds.Tables[0].Rows[0]["trnno"].ToString();
               



            }

            DataSet dsTemp = new DataSet();

            try
            {
                GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                {
                    sc.Url = session.WebServiceAddress.Trim();
                }
                else
                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";

                dsTemp = sc.GetGRNDetailFromA21(session.CompanyId, trnNo);

                //dacl.GetProductStockStatus(session.CompanyId, productCode, ref ds2);
            }
            catch (Exception ex)
            {
                //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
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
