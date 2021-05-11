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
    public partial class VendorRecords2 : GMSBasePage
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


            //GMSCore.Entity.VendorApplicationForm vendor1 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            if (vendorapplicationform != null)
            {
                if (vendorapplicationform.TypeOfOwnership != null)
                {

                    string tempTypeOfOwnership = vendorapplicationform.TypeOfOwnership;

                    string[] types = tempTypeOfOwnership.Split(',');

                    foreach (string type in types)
                    {
                        switch (type)
                        {
                            //default: throw new ApplicationException("Unknown checkbox: " + type);
                            case "Proprietorship": chkProprietorship.Checked = true; break;
                            case "Partnership": chkPartnership.Checked = true; break;
                            case "Corporation": chkCorporation.Checked = true; break;
                            case "Private Limited": chkPrivateLimited.Checked = true; break;
                            case "Public Limited": chkPublicLimited.Checked = true; break;
                            case "Others (Please specify)": chkOthers.Checked = true; break;
                        }
                    }
                }

                if (vendorapplicationform.NatureOfBusiness != null)
                {
                    string tempNatureOfBusiness = vendorapplicationform.NatureOfBusiness;

                    string[] natures = tempNatureOfBusiness.Split(',');

                    foreach (string nature in natures)
                    {
                        switch (nature)
                        {
                            //default: throw new ApplicationException("Unknown checkbox: " + nature);
                            case "Manufacturer": chkManufacturer.Checked = true; break;
                            case "Sub-contractor": chkSubcontractor.Checked = true; break;
                            case "Agent/Distributor": chkAgent.Checked = true; break;
                            case "Stockiest": chkStockiest.Checked = true; break;
                            case "Fabricator": chkFabricator.Checked = true; break;
                            case "Trading House": chkTradingHouse.Checked = true; break;
                            case "Others (Please specify)": chkOthers1.Checked = true; break;
                        }
                    }
                }

                if (vendorapplicationform.ScopeOfWork != null)
                {

                    string tempScopeOfWork = vendorapplicationform.ScopeOfWork;

                    string[] scopes = tempScopeOfWork.Split(',');

                    foreach (string scope in scopes)
                    {
                        switch (scope)
                        {
                            //default: throw new ApplicationException("Unknown checkbox: " + scope);
                            case "Supply of material, equipment": chkManufacturer.Checked = true; break;
                            case "Building related construction, renovation, addition and alteration": chkSubcontractor.Checked = true; break;
                            case "Process Machinery installation, maintenance or servicing worksAgent/Distributor": chkAgent.Checked = true; break;
                            case "On site contractors": chkStockiest.Checked = true; break;
                            case "Waste Collectors": chkFabricator.Checked = true; break;
                            case "Transporters": chkTradingHouse.Checked = true; break;
                        }
                    }
                }

                linkfileName.Text = vendorapplicationform.AgentDistributorFileName;
                btnUpdate.Visible = true;
                btnBack.Visible = true;

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

        //    //if (string.IsNullOrEmpty(hidFormID4.Value.Trim()))
        //    //{ 
        //    #region Add new record

        //    GMSCore.Entity.VendorApplicationForm vendorApplicationForm = new GMSCore.Entity.VendorApplicationForm();



        //        if (vendorApplicationForm.TypeOfOwnership!= false)
        //        {

        //            vendorApplicationForm.CoyID = session.CompanyId;
        //            vendorApplicationForm.VendorID = GMSUtil.ToInt(this.hidVendorID4.Value.Trim());

        //            vendorApplicationForm.TypeOfOwnership = chkProprietorship.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkPartnership.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkCorporation.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkPrivateLimited.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkPublicLimited.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkOthers.Checked;

        //            vendorApplicationForm.TypeOfOwnership = chkManufacturer.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkSubcontractor.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkAgent.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkStockiest.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkFabricator.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkTradingHouse.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkOthers1.Checked;

        //            vendorApplicationForm.TypeOfOwnership = chkSupply.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkBuilding.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkProcess.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkContractors.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkCollectors.Checked;
        //            vendorApplicationForm.TypeOfOwnership = chkTransporters.Checked;

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


        //    StringBuilder str = new StringBuilder();
        //    str.Append("<script language='javascript'>");
        //    str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
        //    str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
        //    str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm.aspx\";} else " + " {window.close();} ");
        //    str.Append("</script>");
        //    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);

        //    #endregion
        //}

        //}

        #region btnDownload_Click
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            linkfileName.Text = vendorapplicationform.AgentDistributorFileName;
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
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + vendorapplicationform.AgentDistributorFileName);
                Response.TransmitFile(@"F:/GMSDocuments/VendorDocuments/AgentDistributor/" + vendorapplicationform.AgentDistributorFileName);
                Response.End();
            }
        }
        #endregion


        #region btnBack_Click
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Records/VendorRecords1.aspx?FORMID=" + hidFormID4.Value.Trim());
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

            GMSCore.Entity.VendorApplicationForm vendorApplicationForm = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value.Trim()));

            var tempTypeOfOwnership = "";

            if (chkProprietorship.Checked)
                tempTypeOfOwnership = string.Concat(tempTypeOfOwnership, "Proprietorship,");
            if (chkPartnership.Checked)
                tempTypeOfOwnership = string.Concat(tempTypeOfOwnership, "Partnership,");
            if (chkCorporation.Checked)
                tempTypeOfOwnership = string.Concat(tempTypeOfOwnership, "Corporation,");
            if (chkPrivateLimited.Checked)
                tempTypeOfOwnership = string.Concat(tempTypeOfOwnership, "Private Limited,");
            if (chkPublicLimited.Checked)
                tempTypeOfOwnership = string.Concat(tempTypeOfOwnership, "Public Limited,");
            if (chkOthers.Checked)
                tempTypeOfOwnership = string.Concat(tempTypeOfOwnership, "Others (Please specify),");

            var tempNatureOfBusiness = "";

            if (chkManufacturer.Checked)
                tempNatureOfBusiness = string.Concat(tempNatureOfBusiness, "Manufacturer,");
            if (chkSubcontractor.Checked)
                tempNatureOfBusiness = string.Concat(tempNatureOfBusiness, "Sub-contractor,");
            if (chkAgent.Checked)
                tempNatureOfBusiness = string.Concat(tempNatureOfBusiness, "Agent/Distributor,");
            if (chkStockiest.Checked)
                tempNatureOfBusiness = string.Concat(tempNatureOfBusiness, "Stockiest,");
            if (chkFabricator.Checked)
                tempNatureOfBusiness = string.Concat(tempNatureOfBusiness, "Fabricator,");
            if (chkTradingHouse.Checked)
                tempNatureOfBusiness = string.Concat(tempNatureOfBusiness, "Trading House,");
            if (chkOthers1.Checked)
                tempNatureOfBusiness = string.Concat(tempNatureOfBusiness, "Other Please Specify,");

            var tempScopeOfWork = "";

            if (chkSupply.Checked)
                tempScopeOfWork = string.Concat(tempScopeOfWork, "Supply of material, equipment,");
            if (chkBuilding.Checked)
                tempScopeOfWork = string.Concat(tempScopeOfWork, "Building related construction, renovation, addition and alteration,");
            if (chkProcess.Checked)
                tempScopeOfWork = string.Concat(tempScopeOfWork, "Process Machinery installation, maintenance or servicing works,");
            if (chkContractors.Checked)
                tempScopeOfWork = string.Concat(tempScopeOfWork, "On site contractors,");
            if (chkCollectors.Checked)
                tempScopeOfWork = string.Concat(tempScopeOfWork, "Waste Collectors,");
            if (chkTransporters.Checked)
                tempScopeOfWork = string.Concat(tempScopeOfWork, "Transporters,");

            vendorApplicationForm.TypeOfOwnership = tempTypeOfOwnership;
            vendorApplicationForm.NatureOfBusiness = tempNatureOfBusiness;
            vendorApplicationForm.ScopeOfWork = tempScopeOfWork;

            vendorApplicationForm.Save();
            vendorApplicationForm.Resync();
            hidFormID4.Value = vendorApplicationForm.FormID.ToString();

            LoadData();

            Response.Redirect("../Records/VendorRecords3.aspx?FORMID=" + hidFormID4.Value.Trim());

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
