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

namespace GMSWeb
{
    public partial class GetFile : System.Web.UI.Page
    {

        string fileName = "";
        string coyID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["FileID"] != null && Request.Params["FileID"].ToString() != "" && Request.Params["ID"] != null && Request.Params["ID"].ToString() != "")
            {
                this.fileName = Request.Params["FileID"].ToString();
                this.coyID = Request.Params["ID"].ToString();
                string ext = Path.GetExtension(fileName.ToString());
                string ContentType = "";

                
                        if (ext == ".asf")
                            ContentType = "video/x-ms-asf";
                        else if (ext == ".avi")
                            ContentType = "video/avi";
                        else if (ext == ".doc")
                            ContentType = "application/msword";
                        else if (ext == ".zip")
                            ContentType = "application/zip";
                        else if (ext == ".xls")
                            ContentType = "application/vnd.ms-excel";
                        else if (ext == ".gif")
                            ContentType = "image/gif";
                        else if (ext == ".jpg" || ext == "jpeg")
                            ContentType = "image/jpeg";
                        else if (ext == ".wav")
                            ContentType = "audio/wav";
                        else if (ext == ".mp3")
                            ContentType = "audio/mpeg3";
                        else if (ext == ".mpg" || ext == "mpeg")
                            ContentType = "video/mpeg";
                        else if (ext == ".mp3")
                            ContentType = "audio/mpeg3";
                        else if (ext == ".rtf")
                            ContentType = "application/rtf";
                        else if (ext == ".htm" || ext == "html")
                            ContentType = "text/html";
                        else if (ext == ".asp")
                            ContentType = "text/asp";
                        else
                            ContentType = "application/octet-stream";

                        try
                        {

                            Response.ContentType = ContentType.ToString();
                            Response.AppendHeader("Content-Disposition", "attachment; filename=" + this.fileName.ToString());
                            Response.TransmitFile("D://GMSDocuments/Salesman/" + this.coyID + "/" + this.fileName.ToString());
                            Response.End();
                        }
                        catch (Exception ex)
                        {                            
                            return;
                        }                      

            }


        }
    }
}
