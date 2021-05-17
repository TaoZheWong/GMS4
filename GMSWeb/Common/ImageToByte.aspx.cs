using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GMSWeb.Common
{
    public partial class ImageToByte : System.Web.UI.Page
    {
        string filePath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["Path"] != null)
            {
                if (!IsPostBack)
                {
                    this.filePath = Request.Params["Path"].ToString();
                    Response.ContentType = "image/png";
                    Response.WriteFile(this.filePath);
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }
}