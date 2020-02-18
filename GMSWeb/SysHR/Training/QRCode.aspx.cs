using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GMSWeb.SysHR.Training
{
    public partial class QRCode : System.Web.UI.Page
    {
        public string path;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["COURSESESSIONID"] != null)
            {
                if (!IsPostBack)
                {
                    string sessionid = Request.Params["COURSESESSIONID"].ToString();

                    string path = "http://chart.apis.google.com/chart?cht=qr&chs=300x300&chl=http://gms.leedenlimited.com/GMS3/SysHR/Training/TEFsignin.aspx?COURSESESSIONID=" + sessionid + "%0D%0A&chld=H|0";

                    this.Qrimage.Attributes["src"] = path;
                }
            }
        }
    }
}