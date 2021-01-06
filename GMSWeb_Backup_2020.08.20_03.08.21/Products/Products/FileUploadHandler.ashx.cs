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
    public class FileUploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();      
            if (context.Request.Files.Count > 0)
            {                
                HttpFileCollection files = context.Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    Dictionary<string, string> list2 = new Dictionary<string, string>();

                    int index = Convert.ToInt32(context.Request.Form["file_id"]);
                    string renamed = context.Request.Form["new_" + (index*3)];
                    string category = context.Request.Form["category_" + (index*3+1)];
                    string coyid  = context.Request.Form["coyid_" + (index*3 + 2)]; 
                    HttpPostedFile file = files[i];
                    string folderPath = System.Web.Configuration.WebConfigurationManager.AppSettings["MR_DOWNLOAD_PATH"].ToString() + coyid;
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    string randomIDFileName = DateTime.Now.Ticks.ToString() + extension;
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    

                    if (randomIDFileName.Length > 100)
                    {
                        randomIDFileName = randomIDFileName.Substring(0, 100 - extension.Length);
                        randomIDFileName = randomIDFileName + extension;
                    }
                    file.SaveAs(folderPath + "/" + randomIDFileName);
                    list2.Add("FileID", "MR" + DateTime.Now.Ticks);
                    list2.Add("FileName", randomIDFileName);
                    list2.Add("FileDisplayName", renamed);
                    list2.Add("DocumentCategory", category);
                    list2.Add("RowStatus", "Insert");
                    list.Add(list2);
                }

                context.Response.ContentType = "application/json";

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string temp = serializer.Serialize(list);
                context.Response.Write(temp);                
            }
        }

        public static string ToJSON(object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
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
