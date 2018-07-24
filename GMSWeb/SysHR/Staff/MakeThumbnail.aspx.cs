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
using System.IO;
using System.Drawing.Imaging;

namespace GMSWeb.SysHR.Staff
{
    public partial class MakeThumbnail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string path = Request.QueryString["path"];
            System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(path));
            int widthOrig = image.Width;
            int heightOrig = image.Height;
            double fx = widthOrig / 85;
            double fy = heightOrig / 108; // subsampling factors
            if (Request.QueryString["size"] == "large")
            {
                fx = widthOrig / 190;
                fy = heightOrig / 220;
            }
            // must fit in thumbnail size
            double f = Math.Max(fx, fy); if (f < 1) f = 1;
            int widthTh = (int)(widthOrig / f); int heightTh = (int)(heightOrig / f);

            System.Drawing.Image thumbnailImage = image.GetThumbnailImage(widthTh, heightTh, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            MemoryStream imageStream = new MemoryStream();
            thumbnailImage.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] imageContent = new Byte[imageStream.Length];
            imageStream.Position = 0;
            imageStream.Read(imageContent, 0, (int)imageStream.Length);
            Response.ContentType = "image/jpeg";
            Response.BinaryWrite(imageContent);
            imageStream.Dispose();
            image.Dispose();
        }

        /// <summary>
        /// Required, but not used
        /// </summary>
        /// <returns>true</returns>
        public bool ThumbnailCallback()
        {
            return true;
        }
    }
}
