using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GMSWeb.HR.Recruitment
{
    public partial class getPDF : System.Web.UI.Page
    {
        string fileName = "";
        string coyID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["FileName"] != null && Request.Params["FileName"].ToString() != "" && Request.Params["ID"] != null && Request.Params["ID"].ToString() != "")
            {
                if (!IsPostBack)
                {
                    this.fileName = Decrypt(Request.Params["FileName"].ToString());
                    this.coyID = Decrypt(Request.Params["ID"].ToString());
                    Response.ContentType = "image/png";
                    Response.WriteFile("F://GMSDocuments/Resume/"+this.coyID+"/"+this.fileName+"_/"+this.fileName+".png");
                    Response.Flush();
                    Response.End();
                }
            }
        }
        private static string Decrypt(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}