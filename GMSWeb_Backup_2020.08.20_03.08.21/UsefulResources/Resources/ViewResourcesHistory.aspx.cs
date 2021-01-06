using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.UsefulResources.Resources
{
    public partial class ViewResourcesHistory : GMSBasePage
    {
        private string documentID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.documentID = Request.Params["DOCUMENTID"];
            LoadDataGrid();
        }

        protected void LoadDataGrid()
        {
            Document doc = Document.RetrieveByKey(short.Parse(this.documentID)); 
            //IList<Document> lstDocument = new SystemDataActivity().RetrieveAllDocumentBySeqID(14);

            DocumentDataDALC dalc = new DocumentDataDALC();
            DataSet ds = new DataSet();
            dalc.GetArchivedDocuments(short.Parse(this.documentID), ref ds);

            this.dgDocument.DataSource = ds.Tables[0]; 
            this.dgDocument.DataBind();

            this.lblTitle.Text = "<h3>Archived Documents for <b>" + doc.DocumentName + "</b></h3>";
        }

        #region dgDocument_ItemCommand
        protected void dgDocument_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Load")
            {
                string ext = Path.GetExtension(e.CommandArgument.ToString());
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

                Response.ContentType = ContentType.ToString();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + e.CommandArgument.ToString());
                try
                {
                    Response.TransmitFile(@"D:/GMSDocuments/Resources/" + e.CommandArgument.ToString());
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
                Response.End();
            }
        }
        #endregion

        #region dgDocument_PageIndexChanged event handling
        protected void dgDocument_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)source;
            dg.CurrentPageIndex = e.NewPageIndex;
            LoadDataGrid();
        }
        #endregion
    }
}
