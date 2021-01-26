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
using System.Data.SqlClient;
using System.IO;


namespace GMSWeb.Procurement.Forms
{
    
    public partial class VendorEvaluationForm4 : GMSBasePage
    {
        protected string folderPath = @"D:\GMSDocuments\VendorDocuments\CompanyOrganisation";
        protected void Page_Load(object sender, EventArgs e)
        {

            //UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
            //                                                                152);


            if (!Page.IsPostBack)
            {
                //preload
                hidFormID4.Value = Request.Params["FORMID"].ToString();
                hidRandomID.Value = Request.Params["RANDOMID"].ToString();
                dgData.CurrentPageIndex = 0;
                LoadData();
            }

            string javaScript =
            @"
            <script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>

            <script type=""text/javascript"">
            function SelectOthers(chkbox)
	        {
	            var prefix = chkbox.id.substring(0,chkbox.id.lastIndexOf(""_"")+1);
	            if (chkbox.checked)
	            {
	                document.getElementById(prefix+""spanSalesPersonMasterName"").style.visibility = 'hidden';
	                document.getElementById(prefix+""spanArea"").style.visibility = 'hidden';
	            } else
	            {
	                document.getElementById(prefix+""spanSalesPersonMasterName"").style.visibility = 'visible';
	                document.getElementById(prefix+""spanArea"").style.visibility = 'visible';
	            }
	        } 
        	 
            </script>
            ";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {

            IList<GMSCore.Entity.VendorApplicationForm> lstData = null;
            SystemDataActivity sDataActivity = new SystemDataActivity();
            lstData = sDataActivity.CheckVendorFormWithRandomID(hidRandomID.Value.ToString(), hidFormID4.Value.ToString());
            if (lstData.Count <= 0)
            {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "This vendor is not valid.");
                return;
            }

            GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            this.lblVendorName1.Text = vendorapplicationform.VendorObject.CompanyName;
            this.lblEmail1.Text = vendorapplicationform.VendorObject.Email;
            this.hidVendorID4.Value = vendorapplicationform.VendorID.ToString();
            this.hidCoyID.Value = vendorapplicationform.CoyID.ToString();

            IList<GMSCore.Entity.VendorCompanyKeyPersonnel> lstEE = null;
            lstEE = new SystemDataActivity().RetrieveVendorCompanyKeyPersonnel(GMSUtil.ToShort(hidCoyID.Value.Trim()), GMSUtil.ToShort(hidVendorID4.Value.Trim()));
            this.dgData.DataSource = lstEE;
            this.dgData.DataBind();

            txtDocumentName.Text = vendorapplicationform.CompanyOrganisationDocumentName;
            linkfileName.Text = vendorapplicationform.CompanyOrganisationFileName;

        }
        #endregion

        #region RefreshPageTitle
        protected void RefreshPageTitle()
        {
            this.Title = Request.Params["PageTitle"].ToString();
        }
        #endregion

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        #region dgData_ItemDataBound
        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
     

        }
        #endregion



        #region dgData_EditCommand
        protected void dgData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }
        #endregion

        #region dgData_CancelCommand
        protected void dgData_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgData_CreateCommand
        protected void dgData_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {

                TextBox txtNewPersonnelName = (TextBox)e.Item.FindControl("txtNewPersonnelName");
                TextBox txtNewPersonnelDesignation = (TextBox)e.Item.FindControl("txtNewPersonnelDesignation");
                TextBox txtNewPersonnelYearOfExperience = (TextBox)e.Item.FindControl("txtNewPersonnelYearOfExperience");


                if (txtNewPersonnelName != null && !string.IsNullOrEmpty(txtNewPersonnelName.Text) &&
                     txtNewPersonnelDesignation != null && !string.IsNullOrEmpty(txtNewPersonnelDesignation.Text)
                     && txtNewPersonnelYearOfExperience != null && !string.IsNullOrEmpty(txtNewPersonnelYearOfExperience.Text))
                {
                    try
                    {

                        GMSCore.Entity.VendorCompanyKeyPersonnel sgt = new GMSCore.Entity.VendorCompanyKeyPersonnel();
                        sgt.CoyID = GMSUtil.ToInt(hidCoyID.Value.Trim());
                        sgt.VendorID = GMSUtil.ToInt(hidVendorID4.Value.Trim());
                        sgt.PersonnelName = txtNewPersonnelName.Text.Trim();
                        sgt.PersonnelDesignation = txtNewPersonnelDesignation.Text.Trim();
                        sgt.PersonnelYearOfExperience = GMSUtil.ToInt(txtNewPersonnelYearOfExperience.Text.Trim());
                        sgt.Save();
                        LoadData();

                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgData_UpdateCommand
        protected void dgData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {

            TextBox txtEditPersonnelName = (TextBox)e.Item.FindControl("txtEditPersonnelName");
            TextBox txtEditPersonnelDesignation = (TextBox)e.Item.FindControl("txtEditPersonnelDesignation");
            TextBox txtEditPersonnelYearOfExperience = (TextBox)e.Item.FindControl("txtEditPersonnelYearOfExperience");
            HtmlInputHidden hidPersonnelID = (HtmlInputHidden)e.Item.FindControl("hidPersonnelID");

            if (txtEditPersonnelName != null && !string.IsNullOrEmpty(txtEditPersonnelName.Text) &&
                     txtEditPersonnelDesignation != null && !string.IsNullOrEmpty(txtEditPersonnelDesignation.Text)
                     && txtEditPersonnelYearOfExperience != null && !string.IsNullOrEmpty(txtEditPersonnelYearOfExperience.Text))
            {

                GMSCore.Entity.VendorCompanyKeyPersonnel ee = GMSCore.Entity.VendorCompanyKeyPersonnel.RetrieveByKey(int.Parse(hidPersonnelID.Value));
                ee.CoyID = GMSUtil.ToInt(hidCoyID.Value.Trim());
                ee.PersonnelName = txtEditPersonnelName.Text.Trim();
                ee.PersonnelDesignation = txtEditPersonnelDesignation.Text.Trim();
                ee.PersonnelYearOfExperience = GMSUtil.ToInt(txtEditPersonnelYearOfExperience.Text.Trim());

                try
                {
                    ee.Save();
                    this.dgData.EditItemIndex = -1;
                    //chkSearchOthers.Checked = chkEditOthers.Checked;
                    LoadData();
                }
                catch (Exception ex)
                {
                    this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }
        }
        #endregion

        #region dgData_DeleteCommand
        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {

                HtmlInputHidden hidPersonnelID = (HtmlInputHidden)e.Item.FindControl("hidPersonnelID");

                if (hidPersonnelID != null)
                {
                    try
                    {
                        SystemDataActivity sDataActivity = new SystemDataActivity();
                        GMSCore.Entity.VendorCompanyKeyPersonnel ee = sDataActivity.RetrieveVendorCompanyKeyPersonnelByPersonnelID(GMSUtil.ToShort(hidCoyID.Value.Trim()), short.Parse(hidPersonnelID.Value));
                        ee.Delete();
                        ee.Resync();
                        this.dgData.EditItemIndex = -1;
                        this.dgData.CurrentPageIndex = 0;
                        LoadData();
                    }
                    catch (SqlException exSql)
                    {
                        if (exSql.Number == 547)
                        {
                            this.MsgPanel1.ShowMessage("This record cannot be deleted because sales person has been assigned to this team.", MessagePanelControl.MessageEnumType.Alert);
                            LoadData();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        LoadData();
                        return;
                    }
                }
            }
        }
        #endregion



        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
        }
        #endregion


        #region btnNext_Click
        protected void btnNext_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Forms/VendorEvaluationForm5.aspx?RANDOMID=" + hidRandomID.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim());
        }
        #endregion

        #region btnBack_Click
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Forms/VendorEvaluationForm3.aspx?RANDOMID=" + hidRandomID.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim());
        }
        #endregion

        #region btnDownload_Click
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            linkfileName.Text = vendorapplicationform.CompanyOrganisationFileName;
            if (linkfileName.Text != string.Empty)
            {
                string ext = linkfileName.Text;
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
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + vendorapplicationform.CompanyOrganisationFileName);
                Response.TransmitFile(@"D:/GMSDocuments/VendorDocuments/CompanyOrganisation/" + vendorapplicationform.CompanyOrganisationFileName);
                Response.End();
            }
        }
        #endregion

        #region btnUpload_Click
        protected void btnUpload_Click(object sender, EventArgs e)
        {

            if (txtDocumentName.Text != "" && FileUpload1.HasFile)
            {
                string fileName = "";

               
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    try
                    {
                        GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value.Trim()));
                        if (vendorapplicationform != null)
                            vendorapplicationform.CompanyOrganisationDocumentName = txtDocumentName.Text.Trim();
                        fileName = string.Concat(vendorapplicationform.VendorID, vendorapplicationform.CompanyOrganisationDocumentName, vendorapplicationform.FormID, Path.GetExtension(this.FileUpload1.FileName));
                        vendorapplicationform.CompanyOrganisationFileName = fileName;
                        FileUpload1.SaveAs(folderPath + "\\" + fileName);

                        vendorapplicationform.Save();
                        vendorapplicationform.Resync();
                        JScriptAlertMsg("Document is uploaded or updated.");
                        lblMsg.Text = "";
                        linkfileName.Text = vendorapplicationform.CompanyOrganisationFileName;
                }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }
                }
                else
                {
                
                lblMsg.Text = "You must key in the Document Name or specify a file.";
                linkfileName.Text = "";
            }
        }
        #endregion
    }
}

