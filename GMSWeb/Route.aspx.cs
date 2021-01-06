using GMSCore;
using System;
using System.Collections.Generic;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GMSWeb
{
    public partial class Route : GMSBasePage
    {
        string type = "";
        string currentLink = "Sales";
        string productCode = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string appPath = HttpRuntime.AppDomainAppVirtualPath;
            if (Request.Params["Type"] != null && Request.Params["CurrentLink"] != null)
            {
                type = Request.Params["Type"].ToString();
                currentLink = Request.Params["CurrentLink"].ToString();
            }
            if (Request.Params["ProductCode"] != null)
                productCode = Request.Params["ProductCode"].ToString();

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }
            string RandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("RandomID" + DateTime.Now.Ticks.ToString(), "MD5");
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            ggdal.UpdateAuthKey(session.UserId, RandomID,session.CompanyId);

            switch (type)
            {
                case "PriceList":
                    Response.Redirect("https://gms.leedenlimited.com/GMS4/Sales/Sales/RadPriceList.aspx?CurrentLink=" + currentLink + "&authkey=" + RandomID);
                    break;
                case "ProductInfo":
                    Response.Redirect("https://gms.leedenlimited.com/GMS4/Products/Products/ProductDetail.aspx?ProductCode=" + productCode + "&CurrentLink="+ currentLink + "&authkey=" + RandomID);
                    break;
                case "CostAllocation":
                    Response.Redirect("https://gms.leedenlimited.com/GMS4/Finance/Admin/CostAllocationInput.aspx?CurrentLink=" + currentLink + "&authkey=" + RandomID);
                    break;
                default:
                    Response.Redirect(base.SessionTimeOutPage(currentLink));
                    break;
            }
        }
    }
}