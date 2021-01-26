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
using System.Web.Mail;
using System.Security.Cryptography;

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using System.Text;
using GMSWeb.CustomCtrl;
using System.Globalization;
using System.Web.Services;

using CrystalDecisions;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;


namespace GMSWeb.Procurement.Forms
{
    public partial class AddEditVendorApplicationForm : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Procurement");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Procurement"));
                return;
            }

            if (!IsPostBack)
            {
                if (Request.Params["VENDORID"] != null)
                {
                    hidVendorID.Value = Request.Params["VENDORID"].ToString();
                    //hidRandomID.Value = Request.Params["RANDOMID"].ToString();
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
            if (hidVendorID.Value != "")
            {
                GMSCore.Entity.Vendor vendor = GMSCore.Entity.Vendor.RetrieveByKey(GMSUtil.ToInt(hidVendorID.Value.Trim()));
                GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID.Value.Trim()));
                //GMSCore.Entity.VendorApplicationForm vendorapplicationform1 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidRandomID.Value.Trim()));
                if (vendor != null)
                {
                    txtVendorName.Text = vendor.CompanyName;
                    hidVendorName.Value = vendor.CompanyName;
                    txtEmail.Text = vendor.Email;

                    btnSendEmail.Visible = true;

                    string appPath = HttpRuntime.AppDomainAppVirtualPath;

                    //localhost



                }
             

            }
        }
        #endregion

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (string.IsNullOrEmpty(hidVendorID.Value.Trim()))
            {
                #region Add New Record.

                  GMSCore.Entity.Vendor vendor = new GMSCore.Entity.Vendor();
                  GMSCore.Entity.VendorApplicationForm vendorApp = new GMSCore.Entity.VendorApplicationForm();

                if (txtVendorName.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("Field cannot be empty.");
                        return;
                    }
                    else
                    {
                   
                        if (vendor == null)
                        {
                            base.JScriptAlertMsg("Vendor Name cannot be found in database.");
                            return;
                        }
                        else
                        {

                        vendor.CoyID = session.CompanyId;
                        vendor.CompanyName = txtVendorName.Text.Trim();
                        vendor.Email = txtEmail.Text.Trim();
                        vendorApp.VEFStatus = "0";
                        vendorApp.ApprovedStatus = "0";
                        vendor.Save();         
                        LoadData();
                    }
                }

                    //vendorApplicationForm.Save();
                    //vendorApplicationForm.Resync();

                    //LoadData();
                    //StringBuilder str = new StringBuilder();
                    //str.Append("<script language='javascript'>");
                    //str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record added successfully! Add another one?';");
                    //str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
                    //str.Append("if (r) {window.location.href = \"../../Procurement/Form/AddEditVendorApplicationForm.aspx\";}");
                    //str.Append("</script>");
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
               
                #endregion
            }
            else
            {
                #region Update Record.

                GMSCore.Entity.Vendor vendor = GMSCore.Entity.Vendor.RetrieveByKey(GMSUtil.ToInt(hidVendorID.Value.Trim()));
                //if (vendorApplicationForm == null)
                //{
                //    base.JScriptAlertMsg("This session cannot be found in database.");
                //    return;
                //}
                if (txtVendorName.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("Vendor Name cannot be empty.");
                        return;
                    }
                    else
                    {
                    vendor.CompanyName = txtVendorName.Text.Trim();
                    vendor.Email = txtEmail.Text.Trim();
   
                    vendor.Save();
                    vendor.Resync();
                    hidVendorID.Value = vendor.VendorID.ToString();            
                    LoadData();

                }

                    
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
                    str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
                    str.Append("if (r) {window.location.href = \"../../SysHR/Training/AddEditSession.aspx\";}");
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
      
                #endregion
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


        #region btnSendEmail_Click
        protected void btnSendEmail_Click(object sender, EventArgs e)
        {

            LogSession session = base.GetSessionInfo();
            var companyID = session.CompanyId;
            var randomID = hidRandomID.Value;
            var companyName = txtVendorName.Text.Trim();
            var email = txtEmail.Text.Trim();
            var linktopass = lnkVendorEvaluation.Text.Trim();

            string appPath = HttpRuntime.AppDomainAppVirtualPath;

            SystemDataActivity sDataActivity = new SystemDataActivity();
            GMSCore.Entity.VendorApplicationForm vendorAppForm = sDataActivity.RetrieveVendorApplicationFormByVendorID(GMSUtil.ToInt(hidVendorID.Value.Trim()));

            btnSendEmail.Visible = true;

            //localhost
            linktopass = "https://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS4/Procurement/Forms/VendorEvaluationForm1.aspxRANDOMID=" + vendorAppForm.RandomID.ToString() + "&FORMID=" + vendorAppForm.FormID.ToString();

            //lnkVendorEvaluation.Text = lnkVendorEvaluation.NavigateUrl = "https://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS4/Procurement/Forms/VendorEvaluationForm.aspx?FORMID=" + vendorApplicationForm.FormID;
            //linktopass = "https://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS4/Procurement/Forms/VendorEvaluationForm1.aspxRANDOMID=" + vendorAppForm.RandomID.ToString() + "&FORMID=" + vendorAppForm.FormID.ToString();

            var m = new ResponseModel();

            try
            {
                new GMSGeneralDALC().SendVendorEmail(companyID, companyName, email, linktopass);
                this.PageMsgPanel.ShowMessage("Email successfully sent.", MessagePanelControl.MessageEnumType.Alert);
            }
            catch (Exception x)
            {
                m.Status = 1;
                m.Message = x.Message;
            }

            //return m;
        }

        #endregion

    }
}
