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
    public partial class VendorRecords13 : GMSBasePage
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
        Response.Redirect("../Records/VendorRecords12.aspx?FORMID=" + hidFormID4.Value.Trim());
    }
    #endregion


    #region btnUpdate_Click
    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        Response.Redirect("../Records/VendorRecords14.aspx?FORMID=" + hidFormID4.Value.Trim());

    }

}
    #endregion

}


