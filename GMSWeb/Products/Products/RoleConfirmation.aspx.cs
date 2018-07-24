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
using GMSWeb.CustomCtrl;

namespace GMSWeb.Products.Products
{
    public partial class RoleConfirmation : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentLink = "Products";
            //lblPageHeader.Text = "Products";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
            }

            Master.setCurrentLink(currentLink);

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            120);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!Page.IsPostBack)
            {
                //preload


            }

        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (radPM.Checked)
                Session["MRRole"] = "PM";
            else
                Session["MRRole"] = "SalesPerson";

            Response.Redirect("MRPurchaseInformation.aspx");
        }
    }
}
