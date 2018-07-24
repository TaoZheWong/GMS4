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
    public partial class Test : GMSBasePage
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {


            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            121);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));



            if (!Page.IsPostBack)
            {
                //preload
                if (Request.Params["TrnNo"] != null)
                {
                    hidPOCode.Value = Request.Params["TrnNo"].ToString().Trim();

                    LoadData();
                }
            }

            string javaScript =
@"
<script language=""javascript"" type=""text/javascript"" src=""/GMS/scripts/popcalendar.js""></script>

";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);



        }
        #endregion

        #region LoadData
        private void LoadData()
        {

            
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


            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                /*
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
                */



            }


        }


    }
}