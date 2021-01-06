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

namespace GMSWeb.Common
{
    public partial class YesNo_PopUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request.Params["Msg"] != null)
            {
                lblMsg.Text = Request.Params["Msg"].ToString();
            }
        }
    }
}
