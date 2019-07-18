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
    public partial class VendorEvaluationForm1 : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //LogSession session = base.GetSessionInfo();
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
            lstData = sDataActivity.CheckVendorFormWithRandomID(hidRandomID.Value.ToString(),hidFormID4.Value.ToString());
            if(lstData.Count <= 0) {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "This vendor is not valid.");
                return;
            } //direct them to unauthorize page


            GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            this.lblVendorName1.Text = vendorapplicationform.VendorObject.CompanyName;
            this.lblEmail1.Text = vendorapplicationform.VendorObject.Email;
            this.hidVendorID4.Value = vendorapplicationform.VendorID.ToString();

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

        //#region btnSubmit_Click
        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    LogSession session = base.GetSessionInfo();

        // //if (string.IsNullOrEmpty(hidFormID4.Value.Trim()))
        //    //{ 
        //    #region Add new record

        //    GMSCore.Entity.VendorApplicationForm vendorApplicationForm = new GMSCore.Entity.VendorApplicationForm();
                  

        //    if (txtCompanyName.Text.Trim() == vendorApplicationForm.CompanyName)
        //    {
        //        Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The record has been submitted before.");

        //        return;
        //    }
        //    else
        //    {          

        //            if (txtCompanyName.Text.Trim() != "")
        //        {
                   
        //            vendorApplicationForm.CoyID = session.CompanyId;
        //            vendorApplicationForm.VendorID = GMSUtil.ToInt(this.hidVendorID4.Value.Trim());
        //            vendorApplicationForm.CompanyName = txtCompanyName.Text.Trim();
        //            vendorApplicationForm.BusinessAddress = txtBusinessAddress.Text.Trim();
        //            vendorApplicationForm.CompanyRegNo = txtCompanyRegNo.Text.Trim();
        //            if (GMSUtil.ToDate(txtCompanyRegDate.Text) != GMSCoreBase.DEFAULT_NO_DATE && txtCompanyRegDate.Text.Trim() != "")
        //                vendorApplicationForm.CompanyRegDate = GMSUtil.ToDate(txtCompanyRegDate.Text.Trim());
        //            vendorApplicationForm.GSTRegNo = txtGSTRegNo.Text.Trim();
        //            vendorApplicationForm.CompanyTelNo = txtCompanyTelNo.Text.Trim();
        //            vendorApplicationForm.CompanyFaxNo = txtCompanyFaxNo.Text.Trim();

        //            vendorApplicationForm.ContactPersonName = txtContactPersonName.Text.Trim();
        //            vendorApplicationForm.ContactPersonDesignation = txtContactPersonDesignation.Text.Trim();
        //            vendorApplicationForm.ContactPersonTelNo = txtContactPersonTelNo.Text.Trim();
        //            vendorApplicationForm.ContactPersonEmail = txtContactPersonEmail.Text.Trim();

        //            vendorApplicationForm.VEFStatus = "1";
        //            vendorApplicationForm.ApprovedStatus = "0";
                  
        //            vendorApplicationForm.Save();
        //            vendorApplicationForm.Resync();
        //            //hidFormID5.Value = vendorApplicationForm.FormID.ToString();
        //            LoadData();

        //            Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The form has been submitted.");
        //            return;
        //        }
        //        else
        //        {
        //            Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "Please fill in all the fields.");

        //        }

          
        //    }

            

        //    StringBuilder str = new StringBuilder();
        //    str.Append("<script language='javascript'>");
        //    str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
        //    str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
        //    str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm1.aspx\";} else " + " {window.close();} ");
        //    str.Append("</script>");
        //    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);

        //        #endregion
        //    }

        //}


        #region btnUpdate_Click
        protected void btnUpdate_Click(object sender, EventArgs e)
         {
            //LogSession session = base.GetSessionInfo();
            //if (session == null) 
            //{
            //    Response.Redirect("../../SessionTimeout.htm");
            //    return;
            //}

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

                vendorApplicationForm.Save();
                vendorApplicationForm.Resync();
                hidFormID5.Value = vendorApplicationForm.FormID.ToString();

                LoadData();

                Response.Redirect("../Forms/VendorEvaluationForm2.aspx?RANDOMID=" + hidRandomID.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim());

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

    }


    //#endregion
}
