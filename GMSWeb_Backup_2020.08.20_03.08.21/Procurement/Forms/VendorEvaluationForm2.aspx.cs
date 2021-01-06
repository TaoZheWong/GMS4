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
    public partial class VendorEvaluationForm2 : GMSBasePage
    {
        protected string folderPath = @"D:\GMSDocuments\VendorDocuments\AgentDistributor";
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
            if (lstData.Count <= 0) {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "This vendor is not valid.");
                return;
            } //diret them to unauthorize page

            GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            this.lblVendorName1.Text = vendorapplicationform.VendorObject.CompanyName;
            this.lblEmail1.Text = vendorapplicationform.VendorObject.Email;
            this.hidVendorID4.Value = vendorapplicationform.VendorID.ToString();

            //GMSCore.Entity.VendorApplicationForm vendor1 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
            if (vendorapplicationform != null)
            {
                if(vendorapplicationform.TypeOfOwnership != null)
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
                        //case "Others (Please specify)": chkOthers.Checked = true; break;
                    }
                        txtOthersType.Text = vendorapplicationform.OthersType;
                    }                 
                }

                

                if (vendorapplicationform.NatureOfBusiness != null) { 
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
                        //case "Others (Please specify)": chkOthers1.Checked = true; break;
                    }
                }
                    txtOthersNature.Text = vendorapplicationform.OthersNature;
                }

                if (vendorapplicationform.ScopeOfWork != null){ 

                string tempScopeOfWork = vendorapplicationform.ScopeOfWork;

                string[] scopes = tempScopeOfWork.Split(';');

                foreach (string scope in scopes)
                {
                    switch (scope)
                    {
                        //default: throw new ApplicationException("Unknown checkbox: " + scope);
                        case "Supply of material, equipment": chkSupply.Checked = true; break;
                        case "Building related construction, renovation, addition and alteration": chkBuilding.Checked = true; break;
                        case "Process Machinery installation, maintenance or servicing works": chkProcess.Checked = true; break;
                        case "On site contractors": chkContractors.Checked = true; break;
                        case "Waste Collectors": chkCollectors.Checked = true; break;
                        case "Transporters": chkTransporters.Checked = true; break;
                    }
                }
                }

                txtDocumentName.Text = vendorapplicationform.AgentDistributorDocumentName;
                linkfileName.Text = vendorapplicationform.AgentDistributorFileName;


                //txtOthersType.Visible = false;
                //txtOthersNature.Visible = false;
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
            Response.Redirect("../Forms/VendorEvaluationForm1.aspx?RANDOMID=" + hidRandomID.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim());
        }
        #endregion

        #region btnUpdate_Click
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
         
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
            //if (chkOthers.Checked) {
            //    txtOthersType.Visible = true;
            //   vendorApplicationForm.OthersType = txtOthersType.Text.Trim();
            //}
            vendorApplicationForm.OthersType = txtOthersType.Text.Trim();

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
            //if (chkOthers1.Checked)
            //    txtOthersNature.Visible = true;
                vendorApplicationForm.OthersNature = txtOthersNature.Text.Trim();

            var tempScopeOfWork= "";

            if (chkSupply.Checked)
                tempScopeOfWork =  string.Concat(tempScopeOfWork, "Supply of material, equipment;");
            if (chkBuilding.Checked)
                tempScopeOfWork = string.Concat(tempScopeOfWork, "Building related construction, renovation, addition and alteration;");
            if (chkProcess.Checked)
                tempScopeOfWork = string.Concat(tempScopeOfWork, "Process Machinery installation, maintenance or servicing works;");
            if (chkContractors.Checked)
                tempScopeOfWork = string.Concat(tempScopeOfWork, "On site contractors;");
            if (chkCollectors.Checked)
                tempScopeOfWork = string.Concat(tempScopeOfWork, "Waste Collectors;");
            if (chkTransporters.Checked)
                tempScopeOfWork = string.Concat(tempScopeOfWork, "Transporters;");

            vendorApplicationForm.TypeOfOwnership = tempTypeOfOwnership;
            vendorApplicationForm.NatureOfBusiness = tempNatureOfBusiness;
            vendorApplicationForm.ScopeOfWork = tempScopeOfWork;

            vendorApplicationForm.Save();
            vendorApplicationForm.Resync();
            hidFormID4.Value = vendorApplicationForm.FormID.ToString();

            LoadData();

            Response.Redirect("../Forms/VendorEvaluationForm3.aspx?RANDOMID=" + hidRandomID.Value.Trim() + "&FORMID=" + hidFormID4.Value.Trim());

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
            str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
            str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm2.aspx\";} else " + " {window.close();} ");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        }
        #endregion

        #region rppReportList_ItemCommand
        protected void rppReportList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "Delete")
            {
                short documentId = short.Parse(e.CommandArgument.ToString());
                GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
                //string filePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\Resources\\" + doc.FileName;
                string filePath = folderPath + vendorapplicationform.AgentDistributorFileName;
                vendorapplicationform.Delete();
                vendorapplicationform.Resync();

                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }

                JScriptAlertMsg("Document deleted.");
                //PopulateRepeater();

                //    }
                //    //else if (e.CommandName == "ViewHistory")
                //    //{
                //    //    HtmlInputHidden hidDocumentID = (HtmlInputHidden)e.Item.FindControl("hidDocumentID");
                //    //    ClientScript.RegisterStartupScript(typeof(string), "",
                //    //        string.Format("jsOpenReport('UsefulResources/Resources/ViewResourcesHistory.aspx?DOCUMENTID={0}');",
                //    //                      hidDocumentID.Value), true);
                //    //    RefreshPageTitle();
            }
            else if (e.CommandName == "Load")
            {
                GMSCore.Entity.VendorApplicationForm vendorapplicationform = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(GMSUtil.ToInt(hidFormID4.Value));
                var CommandArgument = vendorapplicationform.AgentDistributorFileName;
                string ext = Path.GetExtension(e.CommandArgument.ToString());
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
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + e.CommandArgument.ToString());
                try
                {
                    Response.TransmitFile(@"D:/GMSDocuments/VendorDocuments/AgentDistributor" + e.CommandArgument.ToString());
                }
                catch (Exception ex)
                {
                    JScriptAlertMsg(ex.Message);
                }
                Response.End();
            }
        }
        #endregion


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
                Response.TransmitFile(@"D:/GMSDocuments/VendorDocuments/AgentDistributor/" + vendorapplicationform.AgentDistributorFileName);
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
                        vendorapplicationform.AgentDistributorDocumentName = txtDocumentName.Text.Trim();
                    fileName = string.Concat(vendorapplicationform.VendorID, vendorapplicationform.AgentDistributorDocumentName, vendorapplicationform.FormID, Path.GetExtension(this.FileUpload1.FileName));
                    vendorapplicationform.AgentDistributorFileName = fileName;
                    FileUpload1.SaveAs(@"D:\GMSDocuments\VendorDocuments\AgentDistributor" + "\\" + FileUpload1.FileName);

                    vendorapplicationform.Save();
                    vendorapplicationform.Resync();
                    JScriptAlertMsg("Document is uploaded or updated.");
                    linkfileName.Text = vendorapplicationform.AgentDistributorFileName;
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

