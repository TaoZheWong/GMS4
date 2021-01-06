using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GMSWeb.Claim
{
    public partial class ViewImage : System.Web.UI.Page
    {
        string fileName = "";
        string coyID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["FileName"] != null && Request.Params["FileName"].ToString() != "" && Request.Params["ID"] != null && Request.Params["ID"].ToString() != "")
            {
                if (!IsPostBack)
                {
                    this.fileName = Request.Params["FileName"].ToString();
                    this.coyID =Request.Params["ID"].ToString();
                    Response.ContentType = "image/png";
                    Response.WriteFile("D://GMSDocuments//Claim//" + this.coyID + "/" + this.fileName);
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }
}