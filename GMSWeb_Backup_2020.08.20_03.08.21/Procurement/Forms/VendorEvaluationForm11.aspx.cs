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

namespace GMSWeb.Procurement.Forms
{
    public partial class VendorApplicationForm11 : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            {

                if (Request.Params["FORMID"] != null)
                {
                    hidFormID4.Value = Request.Params["FORMID"].ToString();
                    hidRandomID.Value = Request.Params["RANDOMID"].ToString();
                    //hidVendorID4.Value = Request.Params["VENDORID"].ToString();
                    //hidFormID5.Value = Request.Params["FORMID"].ToString();

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

            //GMSCore.Entity.VendorApplicationForm vendor1 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID5.Value));
            if (vendorapplicationform != null)
            {
                if (vendorapplicationform.ChkFinancialReport == true)
                    chkFinancialReport.Checked = true;

                if (vendorapplicationform.ChkCompanyACRA == true)
                    chkCompanyACRA.Checked = true;

                if (vendorapplicationform.ChkAuditReport == true)
                    chkAuditReport.Checked = true;

                if (vendorapplicationform.ChkEFTInformation == true)
                    chkEFTInformation.Checked = true;

                if (vendorapplicationform.ChkOrganizationChart == true)
                    chkOrganizationChart.Checked = true;

                if (vendorapplicationform.ChkHeadCount == true)
                    chkHeadCount.Checked = true;

                if (vendorapplicationform.ChkQualityManual == true)
                    chkQualityManual.Checked = true;

                if (vendorapplicationform.ChkAccreditedCertificates == true)
                    chkAccreditedCertificates.Checked = true;

                if (vendorapplicationform.ChkInsuranceCertificate == true)
                    chkInsuranceCertificate.Checked = true;

                if (vendorapplicationform.ChkTrackRecords == true)
                    chkTrackRecords.Checked = true;

                if (vendorapplicationform.ChkHSEPolicy == true)
                    chkHSEPolicy.Checked = true;

                if (vendorapplicationform.ChkQualificationCertificates == true)
                    chkQualificationCertificates.Checked = true;

                if (vendorapplicationform.ChkSafeLevel == true)
                    chkSafeLevel.Checked = true;

                if (vendorapplicationform.ChkHSETrackRecords == true)
                    chkHSETrackRecords.Checked = true;

                if (vendorapplicationform.ChkTrainingRecords == true)
                    chkTrainingRecords.Checked = true;

                if (vendorapplicationform.ChkCompanyBackground == true)
                    chkCompanyBackground.Checked = true;

                if (vendorapplicationform.ChkProductInformation == true)
                    chkProductInformation.Checked = true;

                if (vendorapplicationform.ChkTechnicalInformation == true)
                   chkTechnicalInformation.Checked = true;

                if (vendorapplicationform.ChkBrochure == true)
                    chkBrochure.Checked = true;

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
            Response.Redirect("../Forms/VendorEvaluationForm10.aspx?RANDOMID=" + hidRandomID.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim());
        }
        #endregion


        #region btnUpdate_Click
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
        

            GMSCore.Entity.VendorApplicationForm vendorApplicationForm = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value.Trim()));

            //if (txtDeclarationName.Text.Trim() == "")
            //{
            //    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "Please fill in all the fields.");
            //    return;
            //}
            //else
            //{
                if (chkFinancialReport.Checked)
                    vendorApplicationForm.ChkFinancialReport = true;
                else
                    vendorApplicationForm.ChkFinancialReport = false;

            if (chkCompanyACRA.Checked)
                vendorApplicationForm.ChkCompanyACRA = true;
            else
                vendorApplicationForm.ChkCompanyACRA = false;

            if (chkAuditReport.Checked)
                vendorApplicationForm.ChkAuditReport = true;
            else
                vendorApplicationForm.ChkAuditReport = false;

            if (chkEFTInformation.Checked)
                vendorApplicationForm.ChkEFTInformation = true;
            else
                vendorApplicationForm.ChkEFTInformation = false;

            if (chkOrganizationChart.Checked)
                vendorApplicationForm.ChkOrganizationChart = true;
            else
                vendorApplicationForm.ChkOrganizationChart = false;

            if (chkHeadCount.Checked)
                vendorApplicationForm.ChkHeadCount = true;
            else
                vendorApplicationForm.ChkHeadCount = false;

            if (chkQualityManual.Checked)
                vendorApplicationForm.ChkQualityManual = true;
            else
                vendorApplicationForm.ChkQualityManual = false;

            if (chkAccreditedCertificates.Checked)
                vendorApplicationForm.ChkAccreditedCertificates = true;
            else
                vendorApplicationForm.ChkAccreditedCertificates = false;

            if (chkInsuranceCertificate.Checked)
                vendorApplicationForm.ChkInsuranceCertificate = true;
            else
                vendorApplicationForm.ChkInsuranceCertificate = false;

            if (chkTrackRecords.Checked)
                vendorApplicationForm.ChkTrackRecords = true;
            else
                vendorApplicationForm.ChkTrackRecords = false;

            if (chkHSEPolicy.Checked)
                vendorApplicationForm.ChkHSEPolicy = true;
            else
                vendorApplicationForm.ChkHSEPolicy = false;

            if (chkQualificationCertificates.Checked)
                vendorApplicationForm.ChkQualificationCertificates = true;
            else
                vendorApplicationForm.ChkQualificationCertificates = false;

            if (chkSafeLevel.Checked)
                vendorApplicationForm.ChkSafeLevel = true;
            else
                vendorApplicationForm.ChkSafeLevel = false;

            if (chkHSETrackRecords.Checked)
                vendorApplicationForm.ChkHSETrackRecords = true;
            else
                vendorApplicationForm.ChkHSETrackRecords = false;

            if (chkTrainingRecords.Checked)
                vendorApplicationForm.ChkTrainingRecords = true;
            else
                vendorApplicationForm.ChkTrainingRecords = false;

            if (chkCompanyBackground.Checked)
                vendorApplicationForm.ChkCompanyBackground = true;
            else
                vendorApplicationForm.ChkCompanyBackground = false;

            if (chkProductInformation.Checked)
                vendorApplicationForm.ChkProductInformation = true;
            else
                vendorApplicationForm.ChkProductInformation = false;

            if (chkTechnicalInformation.Checked)
                vendorApplicationForm.ChkTechnicalInformation = true;
            else
                vendorApplicationForm.ChkTechnicalInformation = false;

            if (chkBrochure.Checked)
                vendorApplicationForm.ChkBrochure = true;
            else
                vendorApplicationForm.ChkBrochure = false;

            vendorApplicationForm.Save();
            vendorApplicationForm.Resync();
            hidFormID4.Value = vendorApplicationForm.FormID.ToString();

            LoadData();

            Response.Redirect("../Forms/VendorEvaluationForm12.aspx?RANDOMID=" + hidRandomID.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim());

            //}

            //StringBuilder str = new StringBuilder();
            //str.Append("<script language='javascript'>");
            //str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
            //str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
            //str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm.aspx\";} else " + " {window.close();} ");
            //str.Append("</script>");
            //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
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

        //#region btnSendEmail_Click
        //protected void btnSendEmail_Click(object sender, EventArgs e)
        //{

        //    LogSession session = base.GetSessionInfo();
        //    var companyID = session.CompanyId;
        //    var vendorID = hidVendorID4.Value;
        //    var companyName = lblVendorName1.Text;
        //    var email = lblEmail1.Text;
        //    var linktopass = lnkVendorEvaluation.Text.Trim();

        //    btnDuplicate.Visible = false;
        //    btnSendEmail.Visible = false;


        //    SystemDataActivity sDataActivity = new SystemDataActivity();
        //    GMSCore.Entity.VendorApplicationForm vendorAppForm = sDataActivity.RetrieveVendorApplicationFormByVendorID(GMSUtil.ToInt(hidVendorID1.Value.Trim()));

        //    btnSendEmail.Visible = true;

        //    //linktopass += vendorAppForm.FormID.ToString();

        //    var m = new ResponseModel();

        //    try
        //    {
        //        new GMSGeneralDALC().SendVendorEmail(companyID, companyName, email, linktopass);
        //    }
        //    catch (Exception x)
        //    {
        //        m.Status = 1;
        //        m.Message = x.Message;
        //    }

        //    //return m;
        //}
        //#endregion

        //#region btnDuplicate_Click
        //protected void btnDuplicate_Click(object sender, EventArgs e)
        //{
        //    //hidCourseSessionID.Value = "";
        //    //txtDateFrom.Text = "";
        //    //txtDateFromTime.Text = "";
        //    //txtDateTo.Text = "";
        //    //txtDateToTime.Text = "";
        //    //btnDuplicate.Visible = false;
        //    //btnAttendee.Visible = false;
        //}
        //#endregion

    }
}
