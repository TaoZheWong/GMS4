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

namespace GMSWeb.HR.PMP
{
    public partial class UpdateViewPMP : GMSBasePage
    {
        private string employeeID = "";
        private string employeeNo = "";
        protected string folderPath = @"D:\GMSDocuments\PMP\";
        private short coyId;
        public bool CanDelete = false;
        private Employee emp; 

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("UsefulResources"));
                return;
            }

            UserAccessDocumentOperation uAccess = new GMSUserActivity().RetrieveUserAccessDocumentOperationByUserIdModuleCategoryId(session.UserId, 21);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("PMP"));
            
            if (uAccess == null || uAccess.Operation != "E")
                CanDelete = false;
            else
                CanDelete = true;
            
            this.coyId = session.CompanyId; 
            this.employeeID = Request.Params["EMPLOYEEID"];
            emp = Employee.RetrieveByKey(short.Parse(this.employeeID));
            this.employeeNo = emp.EmployeeNo.ToString(); 

            if (!Page.IsPostBack)
                LoadDataGrid();
         
            string javaScript =
            @"<script type=""text/javascript""> 
            window.onunload = refreshParent;
            function refreshParent() {
                window.opener.location.href = window.opener.location.href;
            }
            </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        protected void LoadDataGrid()
        {
            Document doc = Document.RetrieveByKey(short.Parse("1")); 
            //IList<Document> lstDocument = new SystemDataActivity().RetrieveAllDocumentBySeqID(14);

            IList<DocumentForEmployee> lstDocument = new SystemDataActivity().RetrieveAllDocumentsByEmployeeID(short.Parse(this.employeeID));
            this.dgDocument.DataSource = lstDocument; 
            this.dgDocument.DataBind();

            if (CanDelete)
            {
                this.dgDocument.Columns[4].Visible = true;
                this.dgDocument.ShowFooter = true;
            }
            else
            {
                this.dgDocument.Columns[4].Visible = false;
                this.dgDocument.ShowFooter = false; 
            }

            this.lblTitle.Text = "PMP for <b>" + emp.Name + "</b>";
        }

        #region dgDocument_ItemCommand
        protected void dgDocument_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                string fileName = "";
                string docNo = "";
                FileUpload FileUpload1 = (FileUpload) e.Item.FindControl("FileUpload1");
                DropDownList ddlNewYear = (DropDownList) e.Item.FindControl("ddlNewYear");
                DropDownList ddlNewType = (DropDownList) e.Item.FindControl("ddlNewType");

                if (FileUpload1.HasFile)
                {
                    if (!Directory.Exists(folderPath + "\\" + employeeNo))
                    {
                        Directory.CreateDirectory(folderPath + "\\" + employeeNo);
                    }
                    try
                    {
                        DocumentNumber documentNumber = DocumentNumber.RetrieveByKey(61, (short)DateTime.Now.Year);
                        if (documentNumber != null)
                        {
                            string year = DateTime.Now.Year.ToString().Substring(2, 2);
                            docNo = year + documentNumber.DocumentNoForEmployee;
                            
                            string nextDocumentNo = (int.Parse(documentNumber.DocumentNoForEmployee) + 1).ToString(); 
                            for (int j = nextDocumentNo.Length; j < documentNumber.DocumentNoForEmployee.Length; j++)
                            {
                                nextDocumentNo = "0" + nextDocumentNo;
                            }
                            documentNumber.DocumentNoForEmployee = nextDocumentNo;
                            documentNumber.Save();
                        }
                        fileName = docNo + Path.GetExtension(FileUpload1.FileName);
                        FileUpload1.SaveAs(folderPath + "\\" + employeeNo + "\\" + fileName);
                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }
                }//end if FileUpload1.HasFile
                else
                {
                    JScriptAlertMsg("You must choose a file to upload.");
                    return; 
                }

                //new document 
                DocumentForEmployee doc = new DocumentForEmployee();
                doc.CoyID = this.coyId; 
                doc.EmployeeID = short.Parse(this.employeeID.ToString());
                doc.FileName = fileName; 
                doc.DateUploaded = DateTime.Now;
                doc.Year = short.Parse(ddlNewYear.SelectedValue);
                doc.Type = ddlNewType.SelectedValue; 
                doc.Save();
                doc.Resync();
                JScriptAlertMsg("The PMP file has been uploaded.");
                LoadDataGrid(); 
            }
            else if (e.CommandName == "Delete")
            {
                //short documentId = short.Parse(e.CommandArgument.ToString());
                HtmlInputHidden hidDocumentID = (HtmlInputHidden)e.Item.FindControl("hidDocumentID");
                short documentId = short.Parse(hidDocumentID.Value.ToString()); 
                DocumentForEmployee doc = DocumentForEmployee.RetrieveByKey(documentId);
                string filePath = folderPath + "\\" + this.employeeNo + "\\" + doc.FileName;
                doc.Delete();
                doc.Resync();

                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }

                JScriptAlertMsg("Document deleted.");
                LoadDataGrid(); 
            }
            else if (e.CommandName == "Load")
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
                    Response.TransmitFile(@"D:/GMSDocuments/PMP/" + this.employeeNo + "/" + e.CommandArgument.ToString());
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

        #region dgDocument_ItemDataBound
        protected void dgDocument_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblType = (Label)e.Item.FindControl("lblType");
                switch (lblType.Text)
                {
                    case "F":
                        lblType.Text = "Full Year";
                        break; 
                    case "M":
                        lblType.Text = "Mid Year";
                        break; 
                    default:
                        break; 
                }

                if (e.Item.DataItem != null) {
                    Label lblNextYear = (Label)e.Item.FindControl("lblNextYear");
                    int nextYear = int.Parse(DataBinder.Eval(e.Item.DataItem, "Year").ToString()) + 1;
                    lblNextYear.Text = nextYear.ToString();
                }
                     
               
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                //ddlNewYear
                DropDownList ddlNewYear = (DropDownList)e.Item.FindControl("ddlNewYear");

                DataTable dtt1 = new DataTable();
                dtt1.Columns.Add("Year", typeof(string));

                for (int i = -2; i < 1; i++)
                {
                    DataRow dr1 = dtt1.NewRow();
                    dr1["Year"] = DateTime.Now.Year + i;

                    dtt1.Rows.Add(dr1);
                }

                ddlNewYear.DataSource = dtt1;
                //ddlNewYear.DataValueField = "Year";
                //ddlNewYear.DataTextField = "Year"; 
                ddlNewYear.DataBind();
                ddlNewYear.SelectedValue = DateTime.Now.Year.ToString(); 

                //ddlNewType
                DropDownList ddlNewType = (DropDownList)e.Item.FindControl("ddlNewType");

                ddlNewType.Items.Add(new ListItem("Full Year","F")); 
                ddlNewType.Items.Add(new ListItem("Mid Year","M"));                
            }
        }
        #endregion
    }
}
