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

namespace GMSWeb.Procurement.Records
{
    public partial class VendorRecords9 : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (!Page.IsPostBack)
            {

                if (Request.Params["FORMID"] != null)
                {
                    hidFormID4.Value = Request.Params["FORMID"].ToString();
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

            LogSession session = base.GetSessionInfo();

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

                if (vendor1.ContractorSuperVisorProvided == true)
                    chkContractorYes.Checked = true;
                //chkContractorNo.Checked = vendor1.ContractorSuperVisorProvided;
                //chkContractorNA.Checked = vendor1.ContractorSuperVisorProvided;
                txtVendorComment6.Text = vendor1.VendorComment6;

                if (vendor1.DesignatedHSERepresentative == true)
                    chkRepresentativeYes.Checked = true;
                //chkRepresentativeNo.Checked = vendor1.DesignatedHSERepresentative;
                //chkRepresentativeNA.Checked = vendor1.DesignatedHSERepresentative;
                txtVendorComment7.Text = vendor1.VendorComment7;

                if (vendor1.StatutoryRequiredME == true)
                    chkStatutoryMEYes.Checked = true;
                //chkStatutoryMENo.Checked = vendor1.StatutoryRequiredHSE;
                //chkStatutoryMENA.Checked = vendor1.StatutoryRequiredHSE;
                txtVendorComment8.Text = vendor1.VendorComment8;

                if (vendor1.HSEStandardRequirement == true)
                    chkRequirementYes.Checked = true;

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
            Response.Redirect("../Records/VendorRecords8.aspx?FORMID=" + hidFormID4.Value.Trim());
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

        
                if (chkContractorYes.Checked)
                    vendorApplicationForm.ContractorSuperVisorProvided = true;
                else
                    vendorApplicationForm.ContractorSuperVisorProvided = false;
                //vendorApplicationForm.ContractorSuperVisorProvided = chkContractorNo.Checked;
                //vendorApplicationForm.ContractorSuperVisorProvided = chkContractorNA.Checked;
                vendorApplicationForm.VendorComment6 = txtVendorComment6.Text.Trim();

                if (chkRepresentativeYes.Checked)
                    vendorApplicationForm.DesignatedHSERepresentative = true;
                else
                    vendorApplicationForm.DesignatedHSERepresentative = false;
                //vendorApplicationForm.DesignatedHSERepresentative = chkRepresentativeNo.Checked;
                //vendorApplicationForm.DesignatedHSERepresentative = chkRepresentativeNA.Checked;
                vendorApplicationForm.VendorComment7 = txtVendorComment7.Text.Trim();

                if (chkStatutoryMEYes.Checked)
                    vendorApplicationForm.StatutoryRequiredME = true;
                else
                    vendorApplicationForm.StatutoryRequiredME = false;
                //vendorApplicationForm.StatutoryRequiredME = chkStatutoryMENo.Checked;
                //vendorApplicationForm.StatutoryRequiredME = chkStatutoryMENA.Checked;
                vendorApplicationForm.VendorComment8 = txtVendorComment8.Text.Trim();

                if (chkRequirementYes.Checked)
                    vendorApplicationForm.HSEStandardRequirement = true;
                else
                    vendorApplicationForm.HSEStandardRequirement = false;

                vendorApplicationForm.Save();
                vendorApplicationForm.Resync();
                hidFormID5.Value = vendorApplicationForm.FormID.ToString();

                LoadData();

                Response.Redirect("../Records/VendorRecords10.aspx?FORMID=" + hidFormID4.Value.Trim());


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

    }

}