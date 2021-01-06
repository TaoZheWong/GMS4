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

namespace GMSWeb.SysSales
{
    public partial class main : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.theBody.Attributes.Add("onload", "SafeAddOnload(SetBannerHighLight(" + ((byte)GMSCore.SystemType.Sales).ToString() + "));");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../SessionTimeout.htm");
                return;
            }
        }
    }
}
