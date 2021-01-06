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
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;
using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Text;
using System.Web.Services;
using AjaxControlToolkit;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace GMSWeb.Sales.Engineering.Project
{
    public partial class Attachment : GMSBasePage
    {
        protected static short CoyID = 0;
        protected static short UserID = 0;
        protected static string ProjectNo = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId, 132);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {

            }

            CoyID = session.CompanyId;
            UserID = session.UserId;
            if (Request.Params["ProjectNo"] != null && Request.Params["ProjectNo"] != "")
            {
                ProjectNo = Request.Params["ProjectNo"];
            }
            else
            {
                ProjectNo = "";
            }
        }

        protected void btnSubmit_ServerClick(object sender, System.EventArgs e)
        {

            bool saveFileSuccess = false;
            string prjno = Request.Form["txtPrjNo"];
            if (FileUpload.HasFile)
            {

                string fn = System.IO.Path.GetFileName(FileUpload.PostedFile.FileName);
                string extension = Path.GetExtension(fn);
                string filename = prjno + " - " + fn;

                SaveFile(FileUpload.PostedFile, prjno, null);
                saveFileSuccess = true;
            }

            if (saveFileSuccess == true)
            {
                Response.Redirect("EditProject.aspx?CurrentLink=Sales&CoyID="+CoyID+"&ProjectNo=" + prjno);
            }
        }

        void SaveFile(HttpPostedFile file, string prjno, string dtime)
        {


            string namefile = System.IO.Path.GetFileName(FileUpload.PostedFile.FileName);
            string type = "Attachments";
            string filename = prjno + " - " + namefile;
            string extension = Path.GetExtension(namefile);
            if (dtime != null)
            {
                filename = filename.Replace(extension, "(" + dtime + ")" + extension);
            }
            string filetype = string.Empty;
            Stream str = FileUpload.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(str);
            Byte[] size = br.ReadBytes((int)str.Length);


            switch (extension)
            {
                case ".jpg":
                    filetype = "image/jpg";
                    break;
                case ".jpeg":
                    filetype = "image/jpg";
                    break;
                case ".png":
                    filetype = "image/png";
                    break;
                case ".gif":
                    filetype = "image/gif";
                    break;
                case ".pdf":
                    filetype = "application/pdf";
                    break;
                case ".doc":
                    filetype = "application/vnd.ms-word";
                    break;
                case ".docx":
                    filetype = "application/vnd.ms-word";
                    break;
                case ".xls":
                    filetype = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    filetype = "application/vnd.ms-excel";
                    break;

            }


            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).InsertAttachmentList(CoyID, prjno, filename, filetype, size, extension, type, UserID, ref dsTemp);
            //return GMSUtil.ToJson(dsTemp, 0);
        }

        //protected void DownloadAF_ServerClick(object sender, System.EventArgs e)
        //{
        //    string prjno = Request.Form["Prj"];
        //    string fid = Request.Form["fileid"];
        //    byte[] bytes;
        //    string fileName, contentType, extension;
        //    string prjAttachmentPath = ((new CMS.Data.AccessLogic.Common.CommonDALC()).GetProjectAttachmentPath(new object[] { coyID })).ToString();
        //    using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnection"]))
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            cmd.CommandText = "SELECT FileName, FileData, FileType, Extension from tbProjectFileAttachment where FileID=@id AND ProjectNo = @ProjectNo";
        //            cmd.Parameters.AddWithValue("@id", fid);
        //            cmd.Parameters.AddWithValue("@ProjectNo", prjno);
        //            cmd.Connection = con;
        //            con.Open();
        //            using (SqlDataReader sdr = cmd.ExecuteReader())
        //            {
        //                sdr.Read();
        //                bytes = (byte[])sdr["FileData"];
        //                contentType = sdr["FileType"].ToString();
        //                fileName = sdr["FileName"].ToString();
        //                extension = sdr["Extension"].ToString();
        //            }
        //            con.Close();
        //        }
        //    }

        //    string filename1 = @prjAttachmentPath + prjno + "\\" + fileName;
        //    FileInfo fileInfo = new FileInfo(filename1);

        //    if (fileInfo.Exists)
        //    {
        //        Response.Clear();
        //        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
        //        Response.AddHeader("Content-Length", fileInfo.Length.ToString());
        //        Response.ContentType = "application/octet-stream";
        //        Response.Flush();
        //        Response.TransmitFile(fileInfo.FullName);
        //        Response.End();
        //    }
        //    else
        //    {
        //        this.lblMsg.Text = "Error: Please contact your administrator to check the uploaded file permission (Non Read-Only).";
        //        this.lblMsg.Visible = true;

        //    }
        //    //Response.Clear();
        //    //Response.Buffer = true;
        //    //Response.Charset = "";
        //    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    //Response.ContentType = contentType;
        //    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + extension);
        //    //Response.BinaryWrite(bytes);
        //    //Response.Flush();
        //    //Response.End();
        //}

        [WebMethod]
        public static List<Dictionary<string, string>> GetAttachmentList(string ProjectNo)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetAttachmentList(CoyID, ProjectNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);


        }

    }
}
