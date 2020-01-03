using GMSCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhostscriptSharp;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace GMSWeb.HR.Recruitment
{
    
    public partial class ViewPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["FileName"] != null && Request.Params["FileName"].ToString() != "" && Request.Params["ID"] != null && Request.Params["ID"].ToString() != "")
            {
                this.pdfToImage.ImageUrl = Page.ResolveUrl("getPDF.aspx?FileName=" + Request.Params["FileName"].ToString() + "&ID=" + Request.Params["ID"].ToString());  
            }
        }
    }
}