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
using GMSCore.Entity;
using GMSCore.Activity;
using System.Text;
using GMSWeb.CustomCtrl;
using System.Data.SqlClient;
using System.IO;

namespace GMSWeb.Procurement.Forms
{
    public partial class VendorEvaluationForm8 : GMSBasePage
    {
        protected string folderPath = @"D:\GMSDocuments\VendorDocuments\HealthSafetyEnvironment\ReferenceProjects";
        protected string folderPath2 = @"D:\GMSDocuments\VendorDocuments\HealthSafetyEnvironment\QualificationCertificates";
        protected string folderPath3 = @"D:\GMSDocuments\VendorDocuments\HealthSafetyEnvironment\RelevantRecords";
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!Page.IsPostBack)
            {

                if (Request.Params["FORMID"] != null)
                {
                    hidFormID4.Value = Request.Params["FORMID"].ToString();
                    hidFormID5.Value = Request.Params["FORMID"].ToString();
                    hidRandomID.Value = Request.Params["RANDOMID"].ToString();

                    LoadData();
                }

            }
            string appPath = HttpRuntime.AppDomainAppVirtualPath;

            string javaScript = "";
            javaScript = "<script language=\"javascript\" type=\"text/javascript\" src=\"" + appPath + "/scripts/popcalendar.js\"></script>";

            javaScript += @"
            <script type=""text/javascript\"">
		    function dataSplit(ctr)
		    {
                var addy_parts = ctr.value.split("":"");

                if (addy_parts[0].length < 1 && addy_parts[1].length < 1){

                    alert(""Time format should be hh:mm"");
                    ctr.value = '';
                    return;
                }
                else
                {
                    IntegerBoxControl_Validate(addy_parts[0],addy_parts[1], ctr);
                }
            }
            
            function IntegerBoxControl_Validate(data, data1, ctr)
            {
            // parse the input as an integer
            // var intValue = parseInt(document.getElementById('txtRefilledTime').value, 10);
            var intValue = parseInt(data, 10);
            var intValue1 = parseInt(data1, 10);

            // if this is not an integer
            if (isNaN(intValue))
            {
                // clear text box
                ctr.value = '';
                alert(""Time format should be hh:mm"");
                return;
            }
            // if this is an integer
            else
            {
       
                switch (true)
                {  
                    case (intValue == 0) :

                    // clear text box
                    ctr.value = ctr.value;
                    break;
                    case (intValue >= 0) :
                        // put the parsed integer value in the text box
                        ctr.value = ctr.value;
                        break;
                    case (intValue < 0) :
                    {// put the positive parsed integer value in the text box
                        alert(""Time format should be hh:mm"");
                        ctr.value = '';
		            }
                    break;
                }
          
            }

            // if this is not an integer
            if (isNaN(intValue1))
            {
                // clear text box
                ctr.value = '';
                alert(""Time format should be hh:mm"");
                return;
            }
            // if this is an integer
            else
            {
                switch (true)
                {
                    case (intValue1 == 0) :
                    // clear text box
                    ctr.value =  ctr.value;
                    break;
                    case (intValue1 >= 0) :
                    // put the parsed integer value in the text box
                    ctr.value = ctr.value;
                    break;
                    case (intValue1 < 0) :
                    {
                        // put the positive parsed integer value in the text box
                        alert(""Time format should be hh:mm"");
		                ctr.value = '';
		            }
                    break;
                 }
          
            }
            }
            </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {
            #region Load By FormID


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


            GMSCore.Entity.VendorApplicationForm vendor1 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID5.Value));
            if (vendor1 != null)
            {

                if (vendor1.JobScopeExperience == true)
                    chkExperienceYes.Checked = true;

                //chkExperienceNo.Checked = vendor1.JobScopeExperience;
                //chkExperienceNA.Checked = vendor1.JobScopeExperience;
                txtVendorComment3.Text = vendor1.VendorComment3;

                if (vendor1.QualificationOfWorker == true)
                    chkQualificationYes.Checked = true;
                //chkQualificationNo.Checked = vendor1.QualificationOfWorker;
                //chkQualificationNA.Checked = vendor1.QualificationOfWorker;
                txtVendorComment4.Text = vendor1.VendorComment4;

                if (vendor1.StatutoryRequiredHSE == true)
                    chkStatutoryHSEYes.Checked = true;
                //chkStatutoryHSENo.Checked = vendor1.StatutoryRequiredHSE;
                //chkStatutoryHSENA.Checked = vendor1.StatutoryRequiredHSE;
                txtVendorComment5.Text = vendor1.VendorComment5;
                txtDocumentName.Text = vendorapplicationform.ReferenceProjectsDocumentName;
                linkfileName.Text = vendorapplicationform.ReferenceProjectsFileName;
                txtDocumentName2.Text = vendorapplicationform.QualificationCertificatesDocumentName;
                linkfileName2.Text = vendorapplicationform.QualificationCertificatesFileName;
                txtDocumentName3.Text = vendorapplicationform.RelevantRecordsDocumentName;
                linkfileName3.Text = vendorapplicationform.RelevantRecordsFileName;

                btnUpdate.Visible = true;
                btnBack.Visible = true;

                //string appPath = HttpRuntime.AppDomainAppVirtualPath;

                //lnkVendorEvaluation.Text = lnkVendorEvaluation.NavigateUrl = "localhost" + appPath + "/Procurement/Forms/VendorEvaluationForm1.aspx?VENDORID=" + hidVendorID4.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim();
            }

            #endregion
        }
        #endregion

        #region btnBack_Click
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Forms/VendorEvaluationForm7.aspx?RANDOMID=" + hidRandomID.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim());
        }
        #endregion

        #region btnUpdate_Click
        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            GMSCore.Entity.VendorApplicationForm vendorApplicationForm = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID5.Value.Trim()));

          
                if (chkExperienceYes.Checked)
                    vendorApplicationForm.JobScopeExperience = true;
                else
                    vendorApplicationForm.JobScopeExperience = false;
                //vendorApplicationForm.JobScopeExperience = chkExperienceNo.Checked;
                //vendorApplicationForm.JobScopeExperience = chkExperienceNA.Checked;
                vendorApplicationForm.VendorComment3 = txtVendorComment3.Text.Trim();

                if (chkQualificationYes.Checked)
                    vendorApplicationForm.QualificationOfWorker = true;
                else
                    vendorApplicationForm.QualificationOfWorker = false;

                //vendorApplicationForm.QualificationOfWorker = chkQualificationNo.Checked;
                //vendorApplicationForm.QualificationOfWorker = chkQualificationNA.Checked;
                vendorApplicationForm.VendorComment4 = txtVendorComment4.Text.Trim();

                if (chkStatutoryHSEYes.Checked)
                    vendorApplicationForm.StatutoryRequiredHSE = true;
                else
                    vendorApplicationForm.StatutoryRequiredHSE = false;
                //vendorApplicationForm.StatutoryRequiredHSE = chkStatutoryHSENo.Checked;
                //vendorApplicationForm.StatutoryRequiredHSE = chkStatutoryHSENA.Checked;
                vendorApplicationForm.VendorComment5 = txtVendorComment5.Text.Trim();

                vendorApplicationForm.Save();
                vendorApplicationForm.Resync();
                hidFormID5.Value = vendorApplicationForm.FormID.ToString();

                LoadData();

                Response.Redirect("../Forms/VendorEvaluationForm9.aspx?RANDOMID=" + hidRandomID.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim());

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
            str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
            str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm.aspx\";} else " + " {window.close();} ");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        }
        #endregion

        #region btnDownload_Click
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            linkfileName.Text = vendorapplicationform.ReferenceProjectsFileName;
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
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + vendorapplicationform.ReferenceProjectsFileName);
                Response.TransmitFile(@"D:/GMSDocuments/VendorDocuments/HealthSafetyEnvironment/ReferenceProjects/" + vendorapplicationform.ReferenceProjectsFileName);
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
                            vendorapplicationform.ReferenceProjectsDocumentName = txtDocumentName.Text.Trim();
                        fileName = string.Concat(vendorapplicationform.VendorID, vendorapplicationform.ReferenceProjectsDocumentName, vendorapplicationform.FormID, Path.GetExtension(this.FileUpload1.FileName));
                        vendorapplicationform.ReferenceProjectsFileName = fileName;
                        FileUpload1.SaveAs(folderPath + "\\" + fileName);

                        vendorapplicationform.Save();
                        vendorapplicationform.Resync();
                        JScriptAlertMsg("Document is uploaded or updated.");
                        linkfileName.Text = vendorapplicationform.ReferenceProjectsFileName;
                        lblMsg.Text = "";
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

        #region btnDownload2_Click
        protected void btnDownload2_Click(object sender, EventArgs e)
        {
            GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            linkfileName.Text = vendorapplicationform.QualificationCertificatesFileName;
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
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + vendorapplicationform.QualificationCertificatesFileName);
                Response.TransmitFile(@"D:/GMSDocuments/VendorDocuments/HealthSafetyEnvironment/QualificationCertificates/" + vendorapplicationform.QualificationCertificatesFileName);
                Response.End();
            }
        }
        #endregion

        #region btnUpload2_Click
        protected void btnUpload2_Click(object sender, EventArgs e)
        {


            if (txtDocumentName2.Text != "" && FileUpload2.HasFile)
            {
                string fileName2 = "";
            
                    if (!Directory.Exists(folderPath2))
                    {
                        Directory.CreateDirectory(folderPath2);
                    }
                    try
                    {
                        GMSCore.Entity.VendorApplicationForm vendorapplicationform2 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value.Trim()));
                        if (vendorapplicationform2 != null)
                            vendorapplicationform2.QualificationCertificatesDocumentName = txtDocumentName2.Text.Trim();
                        fileName2 = string.Concat(vendorapplicationform2.VendorID, vendorapplicationform2.QualificationCertificatesDocumentName, vendorapplicationform2.FormID, Path.GetExtension(this.FileUpload2.FileName));
                        vendorapplicationform2.QualificationCertificatesFileName = fileName2;
                        FileUpload2.SaveAs(folderPath2 + "\\" + fileName2);

                        vendorapplicationform2.Save();
                        vendorapplicationform2.Resync();
                        JScriptAlertMsg("Document is uploaded or updated.");
                        linkfileName2.Text = vendorapplicationform2.QualificationCertificatesFileName;
                        lblMsg2.Text = "";
                }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }
                }
                else
                {
                
                lblMsg2.Text = "You must key in the Document Name or specify a file.";
                linkfileName2.Text = "";
            }
        }
        #endregion

        #region btnDownload3_Click
        protected void btnDownload3_Click(object sender, EventArgs e)
        {
            GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            linkfileName.Text = vendorapplicationform.RelevantRecordsFileName;
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
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + vendorapplicationform.RelevantRecordsFileName);
                Response.TransmitFile(@"D:/GMSDocuments/VendorDocuments/HealthSafetyEnvironment/RelevantRecords" + vendorapplicationform.RelevantRecordsFileName);
                Response.End();
            }
        }
        #endregion
        #region btnUpload3_Click
        protected void btnUpload3_Click(object sender, EventArgs e)
        {


            if (txtDocumentName3.Text != "" && FileUpload3.HasFile)
            {
                string fileName = "";

                  if (!Directory.Exists(folderPath3))
                    {
                        Directory.CreateDirectory(folderPath3);
                    }
                    try
                    {
                        GMSCore.Entity.VendorApplicationForm vendorapplicationform3 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value.Trim()));
                        if (vendorapplicationform3 != null)
                            vendorapplicationform3.RelevantRecordsDocumentName = txtDocumentName3.Text.Trim();
                        fileName = string.Concat(vendorapplicationform3.VendorID, vendorapplicationform3.RelevantRecordsDocumentName, vendorapplicationform3.FormID, Path.GetExtension(this.FileUpload3.FileName));
                        vendorapplicationform3.RelevantRecordsFileName = fileName;
                        FileUpload3.SaveAs(folderPath3 + "\\" + fileName);

                        vendorapplicationform3.Save();
                        vendorapplicationform3.Resync();
                        JScriptAlertMsg("Document is uploaded or updated.");
                        linkfileName3.Text = vendorapplicationform3.RelevantRecordsFileName;
                        lblMsg3.Text = "";
                }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }
                }
                else
                {
      
                lblMsg3.Text = "You must key in the Document Name or specify a file.";
                linkfileName3.Text = "";
            }
        }
        #endregion


        public class ResponseModel
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public IDictionary<string, object> Params { get; set; }

            public ResponseModel()
            {
                Status = 0; // 0=success; 1=error
                Message = "Success";
            }

            public ResponseModel(int status, string message)
            {
                Status = status;
                Message = message;
            }
        }

    }
}