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

namespace GMSWeb.Procurement.Records
{
    public partial class VendorRecords1 : GMSBasePage
    {
            protected void Page_Load(object sender, EventArgs e)
            {
                LogSession session = base.GetSessionInfo();
                if (!Page.IsPostBack)
                {

                    if (Request.Params["FORMID"] != null)
                    {
                        hidFormID4.Value = Request.Params["FORMID"].ToString();
                        //hidVendorID4.Value = Request.Params["VENDORID"].ToString();
                        hidFormID5.Value = Request.Params["FORMID"].ToString();

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


                GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
                this.lblVendorName1.Text = vendorapplicationform.VendorObject.CompanyName;
                this.lblEmail1.Text = vendorapplicationform.VendorObject.Email;
                this.lblStatus1.Text = vendorapplicationform.ApprovedStatus;
                this.hidVendorID4.Value = vendorapplicationform.VendorID.ToString();

            switch (vendorapplicationform.ApprovedStatus)
            {
                case "1":
                    lblStatus1.Text = "Approved";
                    break;
                case "2":
                    lblStatus1.Text = "Rejected";
                    break;
                default:
                    lblStatus1.Text = "Pending";
                    break;
            }



            GMSCore.Entity.VendorApplicationForm vendor1 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID5.Value));
                if (vendor1 != null)
                {
                    txtCompanyName.Text = vendor1.CompanyName;
                    hidCompanyName.Value = vendor1.CompanyName;
                    txtBusinessAddress.Text = vendor1.BusinessAddress;
                    txtCompanyRegNo.Text = vendor1.CompanyRegNo;
                    txtCompanyRegDate.Text = vendor1.CompanyRegDate.ToString("dd/MM/yyyy");
                    txtGSTRegNo.Text = vendor1.GSTRegNo;
                    txtCompanyTelNo.Text = vendor1.CompanyTelNo;
                    txtCompanyFaxNo.Text = vendor1.CompanyFaxNo;

                    txtContactPersonName.Text = vendor1.ContactPersonName;
                    txtContactPersonDesignation.Text = vendor1.ContactPersonDesignation;
                    txtContactPersonTelNo.Text = vendor1.ContactPersonTelNo;
                    txtContactPersonEmail.Text = vendor1.ContactPersonEmail;



                    btnUpdate.Visible = true;
            

                    //string appPath = HttpRuntime.AppDomainAppVirtualPath;

                    //lnkVendorEvaluation.Text = lnkVendorEvaluation.NavigateUrl = "localhost" + appPath + "/Procurement/Forms/VendorEvaluationForm1.aspx?VENDORID=" + hidVendorID4.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim();
                }

                #endregion
            }
            #endregion


            #region btnUpdate_Click
            protected void btnUpdate_Click(object sender, EventArgs e)
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect("../../SessionTimeout.htm");
                    return;
                }

                GMSCore.Entity.VendorApplicationForm vendorApplicationForm = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID5.Value.Trim()));

                if (txtCompanyName.Text.Trim() == "")
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "Please fill in all the fields.");
                    return;
                }
                else
                {
                    vendorApplicationForm.CompanyName = txtCompanyName.Text.Trim();
                    vendorApplicationForm.BusinessAddress = txtBusinessAddress.Text.Trim();
                    vendorApplicationForm.CompanyRegNo = txtCompanyRegNo.Text.Trim();
                    vendorApplicationForm.CompanyRegDate = GMSUtil.ToDate(txtCompanyRegDate.Text.Trim());
                    vendorApplicationForm.GSTRegNo = txtGSTRegNo.Text.Trim();
                    vendorApplicationForm.CompanyTelNo = txtCompanyTelNo.Text.Trim();
                    vendorApplicationForm.CompanyFaxNo = txtCompanyFaxNo.Text.Trim();

                    vendorApplicationForm.ContactPersonName = txtContactPersonName.Text.Trim();
                    vendorApplicationForm.ContactPersonDesignation = txtContactPersonDesignation.Text.Trim();
                    vendorApplicationForm.ContactPersonTelNo = txtContactPersonTelNo.Text.Trim();
                    vendorApplicationForm.ContactPersonEmail = txtContactPersonEmail.Text.Trim();


                    vendorApplicationForm.VEFStatus = "1";
                    vendorApplicationForm.ApprovedStatus = "0";
                    vendorApplicationForm.Save();
                    vendorApplicationForm.Resync();
                    hidFormID5.Value = vendorApplicationForm.FormID.ToString();

                    LoadData();

                    Response.Redirect("../Records/VendorRecords2.aspx?FORMID=" + hidFormID4.Value.Trim());

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
