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

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using System.Text;
using GMSWeb.CustomCtrl;
using System.Collections.Generic;
using System.IO;

namespace GMSWeb.Procurement.Forms
{
    public partial class VendorEvaluationForm5 : GMSBasePage
    {
        protected string folderPath = @"D:\GMSDocuments\VendorDocuments\QualitySafetyEnvironment";
        protected string folderPath2 = @"D:\GMSDocuments\VendorDocuments\Certificates";
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!Page.IsPostBack)
            {

                if (Request.Params["FORMID"] != null)
                {
                    hidFormID4.Value = Request.Params["FORMID"].ToString();
                    //hidVendorID4.Value = Request.Params["VENDORID"].ToString();
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
                this.hidCoyID.Value = vendorapplicationform.CoyID.ToString();
                txtQANoOfPersonnel.Text = vendor1.QANoOfPersonnel.ToString();
                hidQANoOfPersonnel.Value = vendor1.QANoOfPersonnel.ToString();
                txtQAContactPerson.Text = vendor1.QAContactPerson;
                txtQAContactPersonDesignation.Text = vendor1.QAContactPersonDesignation;
                txtHSEContactPerson.Text = vendor1.HSEContactPerson;
                txtHSEContactPersonDesignation.Text = vendor1.HSEContactPersonDesignation;
                if(vendor1.QAManual == true)
                chkManualYes.Checked = true;

              
                    if (vendorapplicationform.Certifications != null)
                    {

                        string tempCertifications = vendorapplicationform.Certifications;

                        string[] certs = tempCertifications.Split(',');

                        foreach (string cert in certs)
                        {
                            switch (cert)
                            {
                                //default: throw new ApplicationException("Unknown checkbox: " + type);
                                case "ISO 9001": chkISO9001.Checked = true; break;
                                case "OHSAS 18001": chkOHSAS18001.Checked = true; break;
                                case "ISO 14001": chkISO14001.Checked = true; break;
                                case "FSSC-22000": chkFSSC22000.Checked = true; break;              
                            }
                        }
                    }

                txtISOAccreditedBy.Text = vendor1.ISOAccreditedBy;        
                txtOHSASAccreditedBy.Text = vendor1.OHSASAccreditedBy;
                txtISO1AccreditedBy.Text = vendor1.ISO1AccreditedBy;
                txtFSSCAccreditedBy.Text = vendor1.FSSCAccreditedBy;
                txtBCAGrade.Text = vendor1.BCAGrade;
                txtOtherCertifications.Text = vendor1.OtherCertfications;

                if (vendorapplicationform.Societies != null)
                {

                    string tempSocieties = vendorapplicationform.Societies;

                    string[] societies = tempSocieties.Split(',');

                    foreach (string society in societies)
                    {
                        switch (society)
                        {
                            //default: throw new ApplicationException("Unknown checkbox: " + type);
                            case "Lloyds": chkLloyds.Checked = true; break;
                            case "TUV": chkTUV.Checked = true; break;
                            case "ABS": chkABS.Checked = true; break;
                            case "DNV": chkDNV.Checked = true; break;
                            case "BV (Bureau Veritas)": chkBV.Checked = true; break;
                            case "Have not worked with any Accredited classification societies": chkWithout.Checked = true; break;
                        }
                    }
                }

                txtDocumentName.Text = vendorapplicationform.QAManualDocumentName;
                linkfileName.Text = vendorapplicationform.QAManualFileName;

                txtDocumentName2.Text = vendorapplicationform.CertificatesDocumentName;
                linkfileName2.Text = vendorapplicationform.CertificatesFileName;

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
            Response.Redirect("../Forms/VendorEvaluationForm4.aspx?RANDOMID=" + hidRandomID.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim());
        }
        #endregion

        #region btnUpdate_Click
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
           
          

            GMSCore.Entity.VendorApplicationForm vendorApplicationForm = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID5.Value.Trim()));

            if (txtQAContactPerson.Text.Trim() == "")
            {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "Please fill in all the fields.");
                return;
            }
            else
            {
                vendorApplicationForm.CoyID = GMSUtil.ToInt(this.hidCoyID.Value.Trim());
                vendorApplicationForm.VendorID = GMSUtil.ToInt(this.hidVendorID4.Value.Trim());
                vendorApplicationForm.QANoOfPersonnel = GMSUtil.ToInt(txtQANoOfPersonnel.Text.Trim());
                vendorApplicationForm.QAContactPerson = txtQAContactPerson.Text.Trim();
                vendorApplicationForm.QAContactPersonDesignation = txtQAContactPersonDesignation.Text.Trim();
                vendorApplicationForm.HSEContactPerson = txtHSEContactPerson.Text.Trim();
                vendorApplicationForm.HSEContactPersonDesignation = txtHSEContactPersonDesignation.Text.Trim();
                if (chkManualYes.Checked)
                    vendorApplicationForm.QAManual = true;
                else 
                    vendorApplicationForm.QAManual = false;

                var tempCertifications = "";

                if (chkISO9001.Checked)
                    tempCertifications = string.Concat(tempCertifications, "ISO 9001,");
                if (chkOHSAS18001.Checked)
                    tempCertifications = string.Concat(tempCertifications, "OHSAS 18001,");
                if (chkISO14001.Checked)
                    tempCertifications = string.Concat(tempCertifications, "ISO 14001,");
                if (chkFSSC22000.Checked)
                    tempCertifications = string.Concat(tempCertifications, "FSSC-22000,");
                
                vendorApplicationForm.ISOAccreditedBy = txtISOAccreditedBy.Text.Trim();
                vendorApplicationForm.OHSASAccreditedBy = txtOHSASAccreditedBy.Text.Trim();
                vendorApplicationForm.ISO1AccreditedBy = txtISO1AccreditedBy.Text.Trim();
                vendorApplicationForm.FSSCAccreditedBy = txtFSSCAccreditedBy.Text.Trim();
                vendorApplicationForm.BCAGrade = txtBCAGrade.Text.Trim();
                vendorApplicationForm.OtherCertfications = txtOtherCertifications.Text.Trim();

                var tempSocieties = "";

                if (chkLloyds.Checked)
                    tempSocieties = string.Concat(tempSocieties, "Lloyds,");
                if (chkTUV.Checked)
                    tempSocieties = string.Concat(tempSocieties, "TUV,");
                if (chkABS.Checked)
                    tempSocieties = string.Concat(tempSocieties, "ABS,");
                if (chkDNV.Checked)
                    tempSocieties = string.Concat(tempSocieties, "DNV,");
                if (chkBV.Checked)
                    tempSocieties = string.Concat(tempSocieties, "BV (Bureau Veritas),");
                if (chkWithout.Checked)
                    tempSocieties = string.Concat(tempSocieties, "Have not worked with any Accredited classification societies,");

                vendorApplicationForm.Certifications = tempCertifications;
                vendorApplicationForm.Societies = tempSocieties;
                vendorApplicationForm.Save();
                vendorApplicationForm.Resync();
                hidFormID5.Value = vendorApplicationForm.FormID.ToString();

                LoadData();

                Response.Redirect("../Forms/VendorEvaluationForm6.aspx?RANDOMID=" + hidRandomID.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim());

            }

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
            str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
            str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm.aspx\";} else " + " {window.close();} ");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        }
        #endregion

        #region RefreshPageTitle
        protected void RefreshPageTitle()
        {
            this.Title = Request.Params["PageTitle"].ToString();
        }
        #endregion


        #region btnDownload_Click
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            linkfileName.Text = vendorapplicationform.QAManualFileName;
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
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + vendorapplicationform.QAManualFileName);
                Response.TransmitFile(@"D:/GMSDocuments/VendorDocuments/QualitySafetyEnvironment/" + vendorapplicationform.QAManualFileName);
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
                        vendorapplicationform.QAManualDocumentName = txtDocumentName.Text.Trim();
                    fileName = string.Concat(vendorapplicationform.VendorID, vendorapplicationform.QAManualDocumentName, vendorapplicationform.FormID, Path.GetExtension(this.FileUpload1.FileName));
                    vendorapplicationform.QAManualFileName = fileName;
                    FileUpload1.SaveAs(folderPath + "\\" + fileName);

                    vendorapplicationform.Save();
                    vendorapplicationform.Resync();
                    JScriptAlertMsg("Document is uploaded or updated.");
                    lblMsg.Text = "";
                    linkfileName.Text = vendorapplicationform.QAManualFileName;
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
            }
                else
             {      
                linkfileName.Text = "";
                lblMsg.Text = "You must key in the Document Name or specify a file.";
            }
        }
        #endregion

        #region btnDownload2_Click
        protected void btnDownload2_Click(object sender, EventArgs e)
        {
            GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            linkfileName.Text = vendorapplicationform.CertificatesFileName;
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
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + vendorapplicationform.CertificatesFileName);
                Response.TransmitFile(@"D:/GMSDocuments/VendorDocuments/Certificates/" + vendorapplicationform.CertificatesFileName);
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
                            vendorapplicationform2.CertificatesDocumentName = txtDocumentName2.Text.Trim();
                        fileName2 = string.Concat(vendorapplicationform2.VendorID, vendorapplicationform2.CertificatesDocumentName, vendorapplicationform2.FormID, Path.GetExtension(this.FileUpload2.FileName));
                        vendorapplicationform2.CertificatesFileName = fileName2;
                        FileUpload2.SaveAs(folderPath2 + "\\" + fileName2);
                        vendorapplicationform2.Save();
                        vendorapplicationform2.Resync();
                        JScriptAlertMsg("Document is uploaded or updated.");
                        lblMsg2.Text = "";
                        linkfileName2.Text = vendorapplicationform2.CertificatesFileName;
                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }
                }
                else
                {
       
                linkfileName2.Text = "";
                lblMsg2.Text = "You must key in the Document Name or specify a file.";
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
