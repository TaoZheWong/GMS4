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

namespace GMSWeb.HR.PMP
{
    public partial class PMPHome : GMSBasePage
    {
        public bool CanDelete = false;

        protected string folderPath = @"F:\GMSDocuments\Resources\"; 

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
                LoadDDLs();
            }
                       
            PopulateRepeater();
            PopulateYourPMPRepeater();

            UserAccessModule uAccessHR = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            45);
            if (uAccessHR == null)
            {
                PopulateStaffRepeater(false);
                PopulateIndirectStaffRepeater();
            }
            else
            {//if user is a HR personnel, can see all staff's PMP. 
                PopulateStaffRepeater(true);
            }       

            UserAccessDocumentOperation uAccess = new GMSUserActivity().RetrieveUserAccessDocumentOperationByUserIdModuleCategoryId(session.UserId,
                                                                            short.Parse(this.hidModuleCategoryID.Value));
            if (uAccess == null && short.Parse(this.hidModuleCategoryID.Value) != 4)
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
            PopulateDocument();
        }

        #region PopulateDocument
        protected void PopulateDocument()
        {
            //IList<Document> lstDocument = new SystemDataActivity().RetrieveAllDocumentByDocumentCategoryID(short.Parse(this.ddlDocumentCategory.SelectedValue));
            //if (lstDocument != null && lstDocument.Count > 0)
            //{
            //this.ddlDocument.DataSource = lstDocument;
            //this.ddlDocument.DataBind();
            //}
            LogSession session = base.GetSessionInfo();
            DocumentDataDALC dalc = new DocumentDataDALC();
            DataSet ds = new DataSet();
            dalc.GetActiveDocuments(session.CompanyId, short.Parse(this.ddlDocumentCategory.SelectedValue), ref ds);
            this.ddlDocument.DataSource = ds.Tables[0]; 
            this.ddlDocument.DataBind();
                        
            ddlDocument.Items.Insert(0, new ListItem("[Select Existing Document]", "0")); 

            //populate Sequence
            DataTable dtt1 = new DataTable();
            dtt1.Columns.Add("SeqID", typeof(string));

            for (int i = 1; i <= ds.Tables[0].Rows.Count + 1; i++)
            {
                DataRow dr1 = dtt1.NewRow();
                dr1["SeqID"] = i; 
                dtt1.Rows.Add(dr1);
            }

            this.ddlSequence.DataSource = dtt1;
            this.ddlSequence.DataBind();
            this.RefreshPageTitle(); 
        }
        #endregion

        #region RefreshPageTitle
        protected void RefreshPageTitle()
        {
            this.Title = Request.Params["PageTitle"].ToString(); 
        }
        #endregion
        
        #region PopulateRepeater
        private void PopulateRepeater()
        {
            LogSession session = base.GetSessionInfo();

            UserAccessDocumentOperation uAccess = new GMSUserActivity().RetrieveUserAccessDocumentOperationByUserIdModuleCategoryId(session.UserId,
                                                                           short.Parse(this.hidModuleCategoryID.Value));
            if (uAccess == null && short.Parse(this.hidModuleCategoryID.Value) != 4)
                Response.Redirect(base.UnauthorizedPage(this.hidModuleCategoryName.Value));

            if (uAccess != null && uAccess.Operation == "E")
                CanDelete = true;
            else
                CanDelete = false;

            IList<DocumentCategory> lstCategory = new SystemDataActivity().RetrieveAllDocumentCategoryByModuleCategoryID(short.Parse(this.hidModuleCategoryID.Value));

            if (lstCategory != null && lstCategory.Count > 0)
            {
                //DocumentCategory dc = new DocumentCategory(); 
                //dc.CategoryName = "Staff PMP"; 
                //lstCategory.Add(dc); 

                rppCategoryList.DataSource = lstCategory;
                rppCategoryList.DataBind();
                
                int i = 0;
                foreach (DocumentCategory rCategory in lstCategory)
                {
                    //if (rCategory.CategoryName != "Staff PMP")
                    //{
                        DocumentDataDALC dalc = new DocumentDataDALC();
                        DataSet ds = new DataSet();
                        dalc.GetActiveDocuments(session.CompanyId,rCategory.DocumentCategoryID, ref ds);

                        // Bind Data to sub repeater
                        RepeaterItem item = this.rppCategoryList.Items[i];
                        Repeater rppReportList = (Repeater)item.FindControl("rppReportList");

                        if (rppReportList != null)
                        {
                            rppReportList.DataSource = ds.Tables[0];
                            rppReportList.DataBind();
                        }
                    //}

                    /*
                    lstDocument = ds.Tables[0].

                    if (lstDocument != null && lstDocument.Count > 0)
                    {
                        for (int j = lstDocument.Count - 1; j >= 0; j--)
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
                    }*/
                    i++;
                     
                }
            }
        }
        #endregion

        #region PopulateYourPMPRepeater
        private void PopulateYourPMPRepeater()
        {
            LogSession session = base.GetSessionInfo();

            List<string> lstStaff = new List<string>();
            lstStaff.Add("Your PMP");

            rppYourList.DataSource = lstStaff;
            rppYourList.DataBind();

            EmployeeDataDALC dalc = new EmployeeDataDALC();
            DataSet ds = new DataSet();
            dalc.GetEmployeeSelfByCoyIDAndUserID(session.CompanyId, session.UserId, ref ds);

            RepeaterItem item = this.rppYourList.Items[0];
            Repeater rppYourPMPList = (Repeater)item.FindControl("rppYourPMPList");

            if (rppYourPMPList != null)
            {
                rppYourPMPList.DataSource = ds.Tables[0]; 
                rppYourPMPList.DataBind();
            }
        }
        #endregion

        #region PopulateIndirectStaffRepeater
        private void PopulateIndirectStaffRepeater()
        {
            LogSession session = base.GetSessionInfo();

            List<string> lstStaff = new List<string>();
            lstStaff.Add("Indirect Staff PMP");

            rppIndirectStaffList.DataSource = lstStaff;
            rppIndirectStaffList.DataBind();

            try
            {
                EmployeeDataDALC dalc = new EmployeeDataDALC();
                DataSet ds = new DataSet();
                dalc.GetEmployeeIndirectListByCoyIDAndUserID(session.CompanyId, session.UserId, ref ds);

                RepeaterItem item = this.rppIndirectStaffList.Items[0];
                Repeater rppIndirectStaffPMPList = (Repeater)item.FindControl("rppIndirectStaffPMPList");

                if (rppIndirectStaffPMPList != null)
                {
                    rppIndirectStaffPMPList.DataSource = ds.Tables[0];
                    rppIndirectStaffPMPList.DataBind();
                }
            }
            catch (Exception ex)
            {
                //JScriptAlertMsg(ex.Message);
            }
        }
        #endregion

        #region PopulateStaffRepeater
        private void PopulateStaffRepeater(bool allEmployees)
        {
            LogSession session = base.GetSessionInfo();

            List<string> lstStaff = new List<string>();
            lstStaff.Add("Direct Staff PMP");

            rppStaffList.DataSource = lstStaff;
            rppStaffList.DataBind();

            EmployeeDataDALC dalc = new EmployeeDataDALC();
            DataSet ds = new DataSet();
            if (allEmployees)
                dalc.GetEmployeeListByCoyID(session.CompanyId, ref ds);
            else
                dalc.GetEmployeeListByCoyIDAndUserID(session.CompanyId, session.UserId, ref ds);

            RepeaterItem item = this.rppStaffList.Items[0];
            Repeater rppEmployeeList = (Repeater)item.FindControl("rppEmployeeList");

            if (rppEmployeeList != null)
            {
                rppEmployeeList.DataSource = ds.Tables[0];
                rppEmployeeList.DataBind();   
            }
        }
        #endregion
       
        #region rppReportList_ItemDataBound
        protected void rppReportList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlInputHidden hidNumOfDocs = (HtmlInputHidden)e.Item.FindControl("hidNumOfDocs");
                LinkButton lnkViewHistory = (LinkButton)e.Item.FindControl("lnkViewHistory");
                if (int.Parse(hidNumOfDocs.Value) > 1)
                    lnkViewHistory.Visible = true;
                else
                    lnkViewHistory.Visible = false;
            }
        }
        #endregion

        #region rppEmployeeList_ItemDataBound
        protected void rppEmployeeList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlInputHidden hidNumOfPMP = (HtmlInputHidden)e.Item.FindControl("hidNumOfPMP");
                LinkButton lnkName = (LinkButton)e.Item.FindControl("lnkName");
                if (int.Parse(hidNumOfPMP.Value) > 0) 
                    lnkName.Enabled = true; 
                else
                    lnkName.Enabled = false; 
            }
        }
        #endregion

        #region rppEmployeeList_ItemCommand
        protected void rppEmployeeList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "LoadStaffPMP")
            {
                short employeeID = short.Parse(e.CommandArgument.ToString());
                Employee emp = new EmployeeActivity().RetrieveEmployeeByEmployeeID(employeeID); 
                
                DocumentForEmployee doc = new DocumentActivity().RetrieveDocumentByEmployeeID(employeeID);
                string filename = doc.FileName;
                string ext = Path.GetExtension(filename); 

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
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                try
                {
                    Response.TransmitFile(@"F:/GMSDocuments/PMP/" + emp.EmployeeNo + "/" + filename);
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
                Response.End();
            }
            else if (e.CommandName == "UpdateViewPMP")
            {
                HtmlInputHidden hidDocumentID = (HtmlInputHidden)e.Item.FindControl("hidEmployeeID");
                ClientScript.RegisterStartupScript(typeof(string), "",
                    string.Format("jsOpenReport('HR/PMP/UpdateViewPMP.aspx?EmployeeID={0}');",
                                  hidDocumentID.Value), true);
                RefreshPageTitle();
            }            
        }
        #endregion

        #region rppReportList_ItemCommand
        protected void rppReportList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (e.CommandName == "Delete")
            {
                short documentId = short.Parse(e.CommandArgument.ToString());
                Document doc = Document.RetrieveByKey(documentId);
                //string filePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\Resources\\" + doc.FileName;
                string filePath = folderPath + doc.FileName;
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
                PopulateRepeater();
                PopulateDocument(); 
            } else if (e.CommandName == "ViewHistory") 
            {
                HtmlInputHidden hidDocumentID = (HtmlInputHidden)e.Item.FindControl("hidDocumentID");
                ClientScript.RegisterStartupScript(typeof(string), "",
                    string.Format("jsOpenReport('UsefulResources/Resources/ViewResourcesHistory.aspx?DOCUMENTID={0}');",
                                  hidDocumentID.Value),true);
                RefreshPageTitle();
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
                    Response.TransmitFile(@"F:/GMSDocuments/Resources/" + e.CommandArgument.ToString());
                } catch(Exception ex)
                {
                    JScriptAlertMsg(ex.Message); 
                }
                Response.End();
            }
        }
        #endregion

        #region rppYourPMPList_ItemCommand
        protected void rppYourPMPList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "LoadYourPMP")
            {
                short employeeID = short.Parse(e.CommandArgument.ToString());
                Employee emp = new EmployeeActivity().RetrieveEmployeeByEmployeeID(employeeID);

                DocumentForEmployee doc = new DocumentActivity().RetrieveDocumentByEmployeeID(employeeID);
                string filename = doc.FileName;
                string ext = Path.GetExtension(filename);

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
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                try
                {
                    Response.TransmitFile(@"F:/GMSDocuments/PMP/" + emp.EmployeeNo + "/" + filename);
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
                Response.End();
            }
            else if (e.CommandName == "UpdateViewPMP")
            {
                HtmlInputHidden hidDocumentID = (HtmlInputHidden)e.Item.FindControl("hidEmployeeID_1");
                ClientScript.RegisterStartupScript(typeof(string), "",
                    string.Format("jsOpenReport('HR/PMP/UpdateViewPMP.aspx?EmployeeID={0}');",
                                  hidDocumentID.Value), true);
                RefreshPageTitle();
            }         
        }
        #endregion

        #region rppYourPMPList_ItemDataBound
        protected void rppYourPMPList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlInputHidden hidNumOfPMP = (HtmlInputHidden)e.Item.FindControl("hidNumOfPMP_1");
                LinkButton lnkName = (LinkButton)e.Item.FindControl("lnkName_1");
                if (int.Parse(hidNumOfPMP.Value) > 0)
                    lnkName.Enabled = true;
                else
                    lnkName.Enabled = false;
            }
        }
        #endregion

        #region rppIndirectStaffPMPList_ItemCommand
        protected void rppIndirectStaffPMPList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "LoadIndirectStaffPMP")
            {
                short employeeID = short.Parse(e.CommandArgument.ToString());
                Employee emp = new EmployeeActivity().RetrieveEmployeeByEmployeeID(employeeID);

                DocumentForEmployee doc = new DocumentActivity().RetrieveDocumentByEmployeeID(employeeID);
                string filename = doc.FileName;
                string ext = Path.GetExtension(filename);

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
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                try
                {
                    Response.TransmitFile(@"F:/GMSDocuments/PMP/" + emp.EmployeeNo + "/" + filename);
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
                Response.End();
            }
            else if (e.CommandName == "UpdateViewPMP")
            {
                HtmlInputHidden hidDocumentID = (HtmlInputHidden)e.Item.FindControl("hidEmployeeID_3");
                ClientScript.RegisterStartupScript(typeof(string), "",
                    string.Format("jsOpenReport('HR/PMP/UpdateViewPMP.aspx?EmployeeID={0}');",
                                  hidDocumentID.Value), true);
                RefreshPageTitle();
            }         
        }
        #endregion

        #region rppIndirectStaffPMPList_ItemDataBound
        protected void rppIndirectStaffPMPList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlInputHidden hidNumOfPMP = (HtmlInputHidden)e.Item.FindControl("hidNumOfPMP_3");
                LinkButton lnkName = (LinkButton)e.Item.FindControl("lnkName_3");
                if (int.Parse(hidNumOfPMP.Value) > 0)
                    lnkName.Enabled = true;
                else
                    lnkName.Enabled = false;
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

            if (!(ddlDocument.SelectedValue == "0" && (txtDocumentName.Text == "" || (!FileUpload1.HasFile))) &&
                 (!(!FileUpload1.HasFile && rblOverwriteDocument.SelectedValue == "0")))
            {
                string fileName = "";
                string docNo = ""; 

                if (FileUpload1.HasFile)
                {
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    try
                    {
                        DocumentNumber documentNumber = DocumentNumber.RetrieveByKey(61, (short)DateTime.Now.Year);
                        if (documentNumber != null)
                        {
                            docNo = documentNumber.DocumentNo;
                            string nextDocumentNo = ((short)(short.Parse(documentNumber.DocumentNo) + 1)).ToString();
                            for (int j = nextDocumentNo.Length; j < documentNumber.DocumentNo.Length; j++)
                            {
                                nextDocumentNo = "0" + nextDocumentNo;
                            }
                            documentNumber.DocumentNo = nextDocumentNo;
                            documentNumber.Save();
                        }
                        fileName = docNo + Path.GetExtension(this.FileUpload1.FileName);
                        FileUpload1.SaveAs(folderPath + "\\" + fileName);
                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }
                }

                if (ddlDocument.SelectedValue != "0" && rblOverwriteDocument.SelectedValue == "1")
                {   //update existing document
                    Document doc = Document.RetrieveByKey(short.Parse(ddlDocument.SelectedValue));
                    if (doc != null)
                    {
                        if (txtDocumentName.Text.ToString() != "")
                            doc.DocumentName = txtDocumentName.Text.ToString().Trim().ToUpper();
                        if (FileUpload1.HasFile)
                        {
                            string previousFilePath = folderPath + "\\" + doc.FileName; 
                            //delete existing document 
                            try
                            {
                                if (File.Exists(previousFilePath))
                                {
                                    File.Delete(previousFilePath);
                                }
                            }
                            catch (Exception ex)
                            {
                                JScriptAlertMsg(ex.Message);
                            }
                            //update new filename
                            doc.FileName = fileName;
                        }
                        doc.SeqID = short.Parse(ddlSequence.SelectedValue);
                        doc.DateUploaded = DateTime.Now; 
                        doc.Save();
                        doc.Resync();
                    }
                }
                else
                {   //new document 
                    //check if the document existed before
                    string documentName; 
                    if (this.txtDocumentName.Text.ToString() == "")
                        documentName = ddlDocument.SelectedItem.Text; 
                    else
                        documentName = this.txtDocumentName.Text.Trim().ToUpper();
                                    
                    Document sameDoc = new DocumentActivity().RetrieveDocumentByDocumentNameFileName(documentName, fileName);
                    if ((rblOverwriteDocument.SelectedValue == "1" && sameDoc == null) || 
                         rblOverwriteDocument.SelectedValue == "0")
                    {
                        Document doc = new Document();
                        doc.DocumentCategoryID = short.Parse(this.ddlDocumentCategory.SelectedValue);
                        doc.DocumentName = documentName;
                        doc.FileName = fileName;
                        doc.SeqID = short.Parse(ddlSequence.SelectedValue);
                        doc.DateUploaded = DateTime.Now;
                        doc.Save();
                        doc.Resync();
                    }
                }

                JScriptAlertMsg("Document is uploaded or updated.");
                PopulateRepeater();
                PopulateDocument(); 
                lblMsg.Text = "";
                txtDocumentName.Text = "";
                this.Title = Request.Params["PageTitle"].ToString(); 
            }
            else
            {
                lblMsg.Text = "You must key in the Document Name or specify a file.";
            }
        }
        #endregion

        #region ddlDocumentCategory_SelectedIndexChanged
        protected void ddlDocumentCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateDocument();
            this.Title = Request.Params["PageTitle"].ToString(); 
        }
        #endregion
        
        #region ddlDocument_SelectedIndexChanged
        protected void ddlDocument_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDocument.SelectedValue == "0") return; 
            Document doc = new DocumentActivity().RetrieveDocumentByDocumentID(short.Parse(ddlDocument.SelectedValue));
            if (doc.SeqID == 0)
                ddlSequence.SelectedValue = "1"; 
            else 
                ddlSequence.SelectedValue = doc.SeqID.ToString();

            this.Title = Request.Params["PageTitle"].ToString(); 
        }
        #endregion
    }
}
