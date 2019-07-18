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
    public partial class VendorEvaluationForm6 : GMSBasePage
    {
        protected string folderPath = @"D:\GMSDocuments\VendorDocuments\Insurances";
        protected string folderPath2 = @"D:\GMSDocuments\VendorDocuments\ProjectRecords";
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!Page.IsPostBack)
            {

                if (Request.Params["FORMID"] != null)
                {
                    hidFormID4.Value = Request.Params["FORMID"].ToString();
                    hidFormID5.Value = Request.Params["FORMID"].ToString();
                    hidRandomID.Value = Request.Params["RANDOMID"].ToString();

                    dgData1.CurrentPageIndex = 0;
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

            IList<GMSCore.Entity.VendorCustomerProjectRecords> lstEE = null;
            lstEE = new SystemDataActivity().RetrieveVendorCustomerProjectRecords(GMSUtil.ToShort(hidCoyID.Value.Trim()));
            this.dgData1.DataSource = lstEE;
            this.dgData1.DataBind();

            GMSCore.Entity.VendorApplicationForm vendor1 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID5.Value));
            if (vendor1 != null)
            {

                if (vendor1.IsInsuranceSchemes == true)
                    chkInsuranceYes.Checked = true;

                //chkInsuranceNo.Checked = vendor1.IsInsuranceSchemes;
                txtTypesOfInsuranceSchemes.Text  = vendor1.TypesOfInsuranceSchemes;
                //txtDocumentName.Text = vendor1.InsuranceCertificatesDocumentName;
                //txtDocumentName2.Text = vendor1.ProjectRecordsDocumentName;

                txtDocumentName.Text = vendorapplicationform.InsuranceCertificatesDocumentName;
                linkfileName.Text = vendorapplicationform.InsuranceCertificatesFileName;

                txtDocumentName2.Text = vendorapplicationform.ProjectRecordsDocumentName;
                linkfileName2.Text = vendorapplicationform.ProjectRecordsFileName;

                btnUpdate.Visible = true;
                btnBack.Visible = true;

                //string appPath = HttpRuntime.AppDomainAppVirtualPath;

                //lnkVendorEvaluation.Text = lnkVendorEvaluation.NavigateUrl = "localhost" + appPath + "/Procurement/Forms/VendorEvaluationForm1.aspx?VENDORID=" + hidVendorID4.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim();
            }

            //SystemDataActivity sDataActivity = new SystemDataActivity();
            //var test = session.CompanyId;
            ////IList<GMSCore.Entity.VendorCustomerProjectRecords> lstEE = sDataActivity.RetrieveVendorCustomerProjectRecords(session.CompanyId);
            ////lstEE = sDataActivity.RetrieveVendorCustomerProjectRecords(session.CompanyId);
            ////this.dgData.DataSource = lstEE;
            //this.dgData1.DataBind();

            #endregion
        }
        #endregion

        #region dgData1 datagrid PageIndexChanged event handling
        protected void dgData1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        #region dgData1_ItemDataBound
        protected void dgData1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
           

        }
        #endregion

        #region dgData1_EditCommand
        protected void dgData1_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData1.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }
        #endregion

        #region dgData1_CancelCommand
        protected void dgData1_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData1.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgData1_CreateCommand
        protected void dgData1_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
          
                TextBox txtNewCustomerName = (TextBox)e.Item.FindControl("txtNewCustomerName");
                TextBox txtNewProjectNo = (TextBox)e.Item.FindControl("txtNewProjectNo");
                TextBox txtNewYearOfCompletion = (TextBox)e.Item.FindControl("txtNewYearOfCompletion");


                if (txtNewCustomerName != null && !string.IsNullOrEmpty(txtNewCustomerName.Text) &&
                     txtNewProjectNo != null && !string.IsNullOrEmpty(txtNewProjectNo.Text)
                     && txtNewYearOfCompletion != null && !string.IsNullOrEmpty(txtNewYearOfCompletion.Text))
                {
                    try
                    {

                        GMSCore.Entity.VendorCustomerProjectRecords sgt = new GMSCore.Entity.VendorCustomerProjectRecords();
                        sgt.CoyID = GMSUtil.ToInt(hidCoyID.Value.Trim());
                        sgt.VendorID = GMSUtil.ToInt(hidVendorID4.Value.Trim());
                        sgt.CustomerName = txtNewCustomerName.Text.Trim();
                        sgt.ProjectNo = txtNewProjectNo.Text.Trim();
                        sgt.YearOfCompletion = txtNewYearOfCompletion.Text.Trim();
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

        #region dgData1_UpdateCommand
        protected void dgData1_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {  

            TextBox txtEditCustomerName = (TextBox)e.Item.FindControl("txtEditCustomerName");
            TextBox txtEditProjectNo = (TextBox)e.Item.FindControl("txtEditProjectNo");
            TextBox txtEditYearOfCompletion = (TextBox)e.Item.FindControl("txtEditYearOfCompletion");
            HtmlInputHidden hidRecordID = (HtmlInputHidden)e.Item.FindControl("hidRecordID");

            if (txtEditCustomerName != null && !string.IsNullOrEmpty(txtEditCustomerName.Text) &&
                     txtEditProjectNo != null && !string.IsNullOrEmpty(txtEditProjectNo.Text)
                     && txtEditYearOfCompletion != null && !string.IsNullOrEmpty(txtEditYearOfCompletion.Text))
            {

                GMSCore.Entity.VendorCustomerProjectRecords ee = GMSCore.Entity.VendorCustomerProjectRecords.RetrieveByKey(int.Parse(hidRecordID.Value));
                ee.CustomerName = txtEditCustomerName.Text.Trim();
                ee.ProjectNo = txtEditProjectNo.Text.Trim();
                ee.YearOfCompletion = txtEditYearOfCompletion.Text.Trim();

                try
                {
                    ee.Save();
                    this.dgData1.EditItemIndex = -1;
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


        #region dgData1_DeleteCommand
        protected void dgData1_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {

                HtmlInputHidden hidRecordID = (HtmlInputHidden)e.Item.FindControl("hidRecordID");

                if (hidRecordID != null)
                {
                    try
                    {
                        SystemDataActivity sDataActivity = new SystemDataActivity();
                        GMSCore.Entity.VendorCustomerProjectRecords ee = sDataActivity.RetrieveVendorCustomerProjectRecordsByRecordID(GMSUtil.ToShort(hidCoyID.Value.Trim()), short.Parse(hidRecordID.Value));
                        ee.Delete();
                        ee.Resync();
                        this.dgData1.EditItemIndex = -1;
                        this.dgData1.CurrentPageIndex = 0;
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
            this.dgData1.CurrentPageIndex = 0;
            LoadData();
        }
        #endregion


        #region btnBack_Click
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Forms/VendorEvaluationForm5.aspx?RANDOMID=" + hidRandomID.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim());
        }
        #endregion


        #region btnUpdate_Click
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
           
            GMSCore.Entity.VendorApplicationForm vendorApplicationForm = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID5.Value.Trim()));

            if (txtTypesOfInsuranceSchemes.Text.Trim() == "")
            {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "Please fill in all the fields.");
                return;
            }
            else
            {
                vendorApplicationForm.CoyID = GMSUtil.ToInt(this.hidCoyID.Value.Trim());
                vendorApplicationForm.VendorID = GMSUtil.ToInt(this.hidVendorID4.Value.Trim());
                if (chkInsuranceYes.Checked)
                    vendorApplicationForm.IsInsuranceSchemes = true;
                else
                    vendorApplicationForm.IsInsuranceSchemes = false;

                //vendorApplicationForm.IsInsuranceSchemes = chkInsuranceNo.Checked;
                vendorApplicationForm.TypesOfInsuranceSchemes = txtTypesOfInsuranceSchemes.Text.Trim();

                vendorApplicationForm.Save();
                vendorApplicationForm.Resync();
                hidFormID5.Value = vendorApplicationForm.FormID.ToString();

                LoadData();

                Response.Redirect("../Forms/VendorEvaluationForm7.aspx?RANDOMID=" + hidRandomID.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim());

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

        #region btnDownload_Click
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            linkfileName.Text = vendorapplicationform.InsuranceCertificatesFileName;
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
                Response.TransmitFile(@"D:/GMSDocuments/VendorDocuments/Insurances/" + vendorapplicationform.CertificatesFileName);
                Response.End();
            }
        }
        #endregion

        #region btnUpload_Click
        protected void btnUpload_Click(object sender, EventArgs e)
        {
  
            if (!(txtDocumentName.Text == "" || (!FileUpload1.HasFile)))
            {
                string fileName = "";

                if (FileUpload1.HasFile)
                {
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    try
                    {
                        GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value.Trim()));
                        if (vendorapplicationform != null)
                            vendorapplicationform.InsuranceCertificatesDocumentName = txtDocumentName.Text.Trim();
                        fileName = string.Concat(vendorapplicationform.VendorID, vendorapplicationform.InsuranceCertificatesDocumentName, vendorapplicationform.FormID, Path.GetExtension(this.FileUpload1.FileName));
                        vendorapplicationform.InsuranceCertificatesFileName = fileName;
                        FileUpload1.SaveAs(folderPath + "\\" + fileName);

                        vendorapplicationform.Save();
                        vendorapplicationform.Resync();
                        JScriptAlertMsg("Document is uploaded or updated.");

                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }
                }
                else
                {

                    //new document 
                    //check if the document existed before
                    //string documentName;

                    // documentName = this.txtDocumentName.Text.Trim().ToUpper();

                    GMSCore.Entity.VendorApplicationForm vendorApplicationForm = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value.Trim()));

                    vendorApplicationForm.InsuranceCertificatesDocumentName = txtDocumentName.Text.Trim().ToUpper();
                    fileName = string.Concat(vendorApplicationForm.VendorID, vendorApplicationForm.InsuranceCertificatesDocumentName, vendorApplicationForm.FormID, Path.GetExtension(this.FileUpload1.FileName));
                    vendorApplicationForm.InsuranceCertificatesFileName = fileName;
                    FileUpload1.SaveAs(folderPath + "\\" + fileName);
                    vendorApplicationForm.Save();
                    vendorApplicationForm.Resync();

                    JScriptAlertMsg("Document is uploaded or updated.");
                    //lblMsg.Text = "";
                    txtDocumentName.Text = "";
                    //this.Title = Request.Params["PageTitle"].ToString();
                    LoadData();
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
            linkfileName2.Text = vendorapplicationform.ProjectRecordsFileName;
            if (linkfileName2.Text != string.Empty)
            {
                string ext = linkfileName2.Text;
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
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + vendorapplicationform.ProjectRecordsFileName);
                Response.TransmitFile(@"D:/GMSDocuments/VendorDocuments/ProjectRecords/" + vendorapplicationform.ProjectRecordsFileName);
                Response.End();
            }
        }
        #endregion

        #region btnUpload2_Click
        protected void btnUpload2_Click(object sender, EventArgs e)
        {
  

            if (!(txtDocumentName2.Text == "" || (!FileUpload2.HasFile)))
            {
                string fileName2 = "";

                if (FileUpload2.HasFile)
                {
                    if (!Directory.Exists(folderPath2))
                    {
                        Directory.CreateDirectory(folderPath2);
                    }
                    try
                    {
                        GMSCore.Entity.VendorApplicationForm vendorapplicationform2 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value.Trim()));
                        if (vendorapplicationform2 != null)
                            vendorapplicationform2.ProjectRecordsDocumentName = txtDocumentName2.Text.Trim();
                        fileName2 = string.Concat(vendorapplicationform2.VendorID, vendorapplicationform2.ProjectRecordsDocumentName, vendorapplicationform2.FormID, Path.GetExtension(this.FileUpload2.FileName));
                        vendorapplicationform2.ProjectRecordsFileName = fileName2;
                        FileUpload2.SaveAs(folderPath2 + "\\" + fileName2);

                        vendorapplicationform2.Save();
                        vendorapplicationform2.Resync();
                        JScriptAlertMsg("Document is uploaded or updated.");
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }
                }
                else
                {

                    //new document 
                    //check if the document existed before
                    //string documentName;

                    // documentName = this.txtDocumentName.Text.Trim().ToUpper();

                    GMSCore.Entity.VendorApplicationForm vendorApplicationForm2 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value.Trim()));

                    vendorApplicationForm2.ProjectRecordsDocumentName = txtDocumentName2.Text.Trim().ToUpper();
                    fileName2 = string.Concat(vendorApplicationForm2.VendorID, vendorApplicationForm2.ProjectRecordsDocumentName, vendorApplicationForm2.FormID, Path.GetExtension(this.FileUpload2.FileName));
                    vendorApplicationForm2.ProjectRecordsFileName = fileName2;
                    FileUpload2.SaveAs(folderPath + "\\" + fileName2);
                    vendorApplicationForm2.Save();
                    vendorApplicationForm2.Resync();

                    JScriptAlertMsg("Document is uploaded or updated.");
                    //lblMsg.Text = "";
                    txtDocumentName2.Text = "";
                    //this.Title = Request.Params["PageTitle"].ToString();
                    LoadData();
                }
            }
            else
            {
                lblMsg2.Text = "You must key in the Document Name or specify a file.";
                linkfileName2.Text = "";
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