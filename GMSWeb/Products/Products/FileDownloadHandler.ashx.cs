using System;
using System.Data;
using System.Web;
using System.IO;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace GMSWeb.Products.Products
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class FileDownloadHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {            
            string ContentType = "";
            string FileName = context.Request.QueryString["FileName"];
            string CompanyId = context.Request.QueryString["CompanyId"];
            ContentType = ReturnFiletype(FileName);
            context.Response.ContentType = ContentType.ToString();
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
            context.Response.TransmitFile(System.Web.Configuration.WebConfigurationManager.AppSettings["MR_ATTACHMENT_DOWNLOAD_PATH"].ToString() + CompanyId + "\\" + FileName);
            context.Response.End();            
        }

        public static string ReturnFiletype(string ext)
        {
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

            return ContentType;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
