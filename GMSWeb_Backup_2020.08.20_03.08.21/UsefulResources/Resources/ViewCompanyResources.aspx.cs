using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System.IO;

namespace GMSWeb.UsefulResources.Resources
{
    public partial class ViewCompanyResources : GMSBasePage
    {
        public bool CanDelete = false;
        public string coyid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("UsefulResources"));
                return;
            }

            if (!Page.IsPostBack)
            {
                if (Request.Params["PageHeader"] != null)
                    this.lblPageHeader.Text = Request.Params["PageHeader"].ToString();
                if (Request.Params["PageTitle"] != null)
                    this.Title = Request.Params["PageTitle"].ToString();
                if (Request.Params["ModuleCategoryID"] != null)
                    this.hidModuleCategoryID.Value = Request.Params["ModuleCategoryID"].ToString();
                if (Request.Params["ModuleCategoryName"] != null)
                    this.hidModuleCategoryName.Value = Request.Params["ModuleCategoryName"].ToString();

                //if (this.lblPageHeader.Text == "")
                //    this.lblPageHeader.Text = "Useful Resources &gt; View";
                //if (this.Title == "")
                //    this.Title = "Resources - View";
                coyid = session.CompanyId.ToString();

                LoadDDLs();
                PopulateRepeater();

                //UserAccessModule uAccess2 = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                //                                                            98);
            }

            UserAccessDocumentOperation uAccess = new GMSUserActivity().RetrieveUserAccessDocumentOperationByUserIdModuleCategoryId(session.UserId,
                                                                            short.Parse(this.hidModuleCategoryID.Value));
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage(this.hidModuleCategoryName.Value));

            Master.setCurrentLink(this.hidModuleCategoryName.Value);

            if (uAccess == null || uAccess.Operation != "E")
            {
                tbUpload.Visible = false;
                CanDelete = false;
            }
            else
            {
                tbUpload.Visible = true;
                CanDelete = true;
            }

            string javaScript =
            @"<script type=""text/javascript"">
		    function toggleAccessRow(n)
		    {
			    if( document.getElementById(""rppToggle_"" + n) )
			    {
				    var current = document.getElementById(""rppToggle_"" + n).style.display;
				    document.getElementById(""rppToggle_"" + n).style.display = (current == null || current == ""none"")?"""":""none"";
				    document[""imgAccessBox_"" + n].src = (current == null || current == ""none"")? sDOMAIN+""/App_Themes/Default/images/checkCloseIcon.gif"" : sDOMAIN+""/App_Themes/Default/images/checkOpenIcon.gif"";
			    }
		    }
		    </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        protected void LoadDDLs()
        {
            IList<DocumentCategory> lstCategory = new SystemDataActivity().RetrieveAllDocumentCategoryByModuleCategoryID(short.Parse(this.hidModuleCategoryID.Value));

            if (lstCategory != null && lstCategory.Count > 0)
            {
                this.ddlDocumentCategory.DataSource = lstCategory;
                this.ddlDocumentCategory.DataBind();
            }

        }

        #region PopulateRepeater
        private void PopulateRepeater()
        {
            LogSession session = base.GetSessionInfo();

            UserAccessDocumentOperation uAccess = new GMSUserActivity().RetrieveUserAccessDocumentOperationByUserIdModuleCategoryId(session.UserId,
                                                                           short.Parse(this.hidModuleCategoryID.Value));
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage(this.hidModuleCategoryName.Value));

            if (uAccess != null && uAccess.Operation == "E")
                CanDelete = true;
            else
                CanDelete = false;

            IList<DocumentCategory> lstCategory = new SystemDataActivity().RetrieveAllDocumentCategoryByModuleCategoryID(short.Parse(this.hidModuleCategoryID.Value));

            if (lstCategory != null && lstCategory.Count > 0)
            {
                rppCategoryList.DataSource = lstCategory;
                rppCategoryList.DataBind();
                
                int i = 0;
                foreach (DocumentCategory rCategory in lstCategory)
                {
                    IList<DocumentForCompany> lstDocument = null;
                    lstDocument = new SystemDataActivity().RetrieveAllDocumentForCompanyBySeqID(rCategory.DocumentCategoryID, session.CompanyId);
                    if (lstDocument != null && lstDocument.Count > 0)
                    {                  
                            
                            // Bind Data to sub repeater
                            RepeaterItem item = this.rppCategoryList.Items[i];
                            Repeater rppReportList = (Repeater)item.FindControl("rppReportList");

                            if (rppReportList != null)
                            {
                                rppReportList.DataSource = lstDocument;
                                rppReportList.DataBind();
                            }
                        
                    }
                    i++;
                }
            }
        }
        #endregion

        #region rppReportList_ItemCommand
        protected void rppReportList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            UserAccessDocumentOperation uAccess = new GMSUserActivity().RetrieveUserAccessDocumentOperationByUserIdModuleCategoryId(session.UserId,
                                                                            short.Parse(this.hidModuleCategoryID.Value));
            if (uAccess == null || uAccess.Operation != "E")
                Response.Redirect(base.UnauthorizedPage(this.hidModuleCategoryName.Value));


            if (e.CommandName == "Delete")
            {
                short documentId = short.Parse(e.CommandArgument.ToString());
                DocumentForCompany doc = DocumentForCompany.RetrieveByKey(documentId);
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\Company Resources\\" + session.CompanyId.ToString();
                doc.Delete();
                doc.Resync();

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                JScriptAlertMsg("Document deleted.");
                PopulateRepeater();
            }
        }
        #endregion

        #region btnUpload_Click
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            UserAccessDocumentOperation uAccess = new GMSUserActivity().RetrieveUserAccessDocumentOperationByUserIdModuleCategoryId(session.UserId,
                                                                            short.Parse(this.hidModuleCategoryID.Value));
            if (uAccess == null || uAccess.Operation != "E")
                Response.Redirect(base.UnauthorizedPage(this.hidModuleCategoryName.Value));

            if (FileUpload1.HasFile)
            {
                string folderPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\Company Resources\\" + session.CompanyId.ToString();
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                FileUpload1.SaveAs(folderPath + "\\" + FileUpload1.FileName);
                DocumentForCompany doc = new DocumentForCompany();
                doc.CoyID = session.CompanyId;
                doc.DocumentCategoryID = short.Parse(this.ddlDocumentCategory.SelectedValue);
                doc.DocumentName = this.txtDocumentName.Text.Trim().ToUpper();
                doc.FileName = FileUpload1.FileName;
                doc.Save();
                doc.Resync();
                JScriptAlertMsg("Document uploaded.");
                PopulateRepeater();
                lblMsg.Text = "";
            }
            else
            {
                lblMsg.Text = "You have not specified a file.";
            }
        }
        #endregion
    }
}
