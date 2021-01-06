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
using System.Globalization;
using System.Web.Services;

using CrystalDecisions;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace GMSWeb.Procurement.Records
{
    public partial class VendorDetails : GMSBasePage
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

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId, 155);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Procurement"));

            if (!Page.IsPostBack)
            {
                //preload
                LoadData();
            }

            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }


        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            IList<GMSCore.Entity.VendorApplicationForm> lstData = null;

            string CompanyName = "%%";
            if (!string.IsNullOrEmpty(txtNewVendorName.Text))
                CompanyName = "%" + txtNewVendorName.Text.Trim() + "%";
            string Email = "%%";
            if (!string.IsNullOrEmpty(txtNewVendorEmail.Text))
                Email = "%" + txtNewVendorEmail.Text.Trim() + "%";
            try
            {
                lstData = new SystemDataActivity().RetrieveVendorListByVendorByEmail(CompanyName, Email);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }


            //Update search result
            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;


            if (lstData != null && lstData.Count > 0)
            {
                if (endIndex < lstData.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstData.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstData.Count.ToString() + " " + "of" + " " + lstData.Count.ToString();
            }
            else
            this.lblSearchSummary.Text = "No records.";
            this.lblSearchSummary.Visible = true;

            this.dgData.DataSource = lstData;
            this.dgData.DataBind();
        }
        #endregion
        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            lblSearchSummary.Text = e.NewPageIndex.ToString();

            lblSearchSummary.Visible = true;
            LoadData();

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

        #region DuplicateFunction
        protected void DuplicateFunction(short formid)
        {
            LogSession session = base.GetSessionInfo();

            GMSCore.Entity.VendorApplicationForm vendorApplicationForm = new GMSCore.Entity.VendorApplicationForm();
            GMSCore.Entity.VendorApplicationForm vendorApplicationForm1 = GMSCore.Entity.VendorApplicationForm.RetrieveByKey(formid);
            GMSCore.Entity.VendorCompanyKeyPersonnel vendorCompanyKeyPersonnel = new GMSCore.Entity.VendorCompanyKeyPersonnel();
            IList<GMSCore.Entity.VendorCompanyKeyPersonnel> vendorCompanyKeyPersonnel1 = null;
            vendorCompanyKeyPersonnel1 = new SystemDataActivity().RetrieveVendorCompanyPersonnelByVendor(session.CompanyId,GMSUtil.ToShort(hidNewVendorID.Value.Trim()));
            GMSCore.Entity.VendorCustomerProjectRecords vendorCustomerProjectRecords = new GMSCore.Entity.VendorCustomerProjectRecords();
            IList<GMSCore.Entity.VendorCustomerProjectRecords> vendorCustomerProjectRecords1 = null;
            vendorCustomerProjectRecords1 = new SystemDataActivity().RetrieveVendorCustomerProjectRecordsByVendor(session.CompanyId, GMSUtil.ToShort(hidNewVendorID.Value.Trim()));
          
            vendorApplicationForm.CoyID = session.CompanyId;
            vendorApplicationForm.VendorID = vendorApplicationForm1.VendorID;
            vendorApplicationForm.CompanyName = vendorApplicationForm1.CompanyName;
            vendorApplicationForm.BusinessAddress = vendorApplicationForm1.BusinessAddress;
            vendorApplicationForm.CompanyRegNo = vendorApplicationForm1.CompanyRegNo;
            vendorApplicationForm.CompanyRegDate = GMSUtil.ToDate(vendorApplicationForm1.CompanyRegDate);
            vendorApplicationForm.GSTRegNo = vendorApplicationForm1.GSTRegNo;
            vendorApplicationForm.CompanyTelNo = vendorApplicationForm1.CompanyTelNo;
            vendorApplicationForm.CompanyFaxNo = vendorApplicationForm1.CompanyFaxNo;

            vendorApplicationForm.ContactPersonName = vendorApplicationForm1.ContactPersonName;
            vendorApplicationForm.ContactPersonDesignation = vendorApplicationForm1.ContactPersonDesignation;
            vendorApplicationForm.ContactPersonTelNo = vendorApplicationForm1.ContactPersonTelNo;
            vendorApplicationForm.ContactPersonEmail = vendorApplicationForm1.ContactPersonEmail;

            vendorApplicationForm.TypeOfOwnership = vendorApplicationForm1.TypeOfOwnership;
            vendorApplicationForm.NatureOfBusiness = vendorApplicationForm1.NatureOfBusiness;
            vendorApplicationForm.ScopeOfWork = vendorApplicationForm1.ScopeOfWork;
            vendorApplicationForm.OthersType = vendorApplicationForm1.OthersType;
            vendorApplicationForm.OthersNature = vendorApplicationForm1.OthersNature;

          
            vendorApplicationForm.PaidUpCapital = vendorApplicationForm1.PaidUpCapital;
            vendorApplicationForm.AnnualTurnover = vendorApplicationForm1.AnnualTurnover;
            vendorApplicationForm.AuthorizedCapital = vendorApplicationForm1.AuthorizedCapital;
            vendorApplicationForm.BankInformation = vendorApplicationForm1.BankInformation;
            vendorApplicationForm.BankName = vendorApplicationForm1.BankName;
            vendorApplicationForm.AccountNo = vendorApplicationForm1.AccountNo;
            vendorApplicationForm.BankerAddress = vendorApplicationForm1.BankerAddress;
            vendorApplicationForm.TradeCurrency = vendorApplicationForm1.TradeCurrency;
            vendorApplicationForm.CreditTerm = vendorApplicationForm1.CreditTerm;
            vendorApplicationForm.SwiftCode = vendorApplicationForm1.SwiftCode;

            foreach(var x in vendorCompanyKeyPersonnel1) { 
            vendorCompanyKeyPersonnel.PersonnelName = x.PersonnelName;
            vendorCompanyKeyPersonnel.PersonnelDesignation = x.PersonnelDesignation;
            vendorCompanyKeyPersonnel.PersonnelYearOfExperience = x.PersonnelYearOfExperience;

            vendorCompanyKeyPersonnel.Save();
            }

            vendorApplicationForm.QANoOfPersonnel = vendorApplicationForm1.QANoOfPersonnel;
            vendorApplicationForm.QAContactPerson = vendorApplicationForm1.QAContactPerson;
            vendorApplicationForm.QAContactPersonDesignation = vendorApplicationForm1.QAContactPersonDesignation;
            vendorApplicationForm.HSEContactPerson = vendorApplicationForm1.HSEContactPerson;
            vendorApplicationForm.HSEContactPersonDesignation = vendorApplicationForm1.HSEContactPersonDesignation;
            vendorApplicationForm.QAManual = vendorApplicationForm1.QAManual;

            vendorApplicationForm.ISOAccreditedBy = vendorApplicationForm1.ISOAccreditedBy;
            vendorApplicationForm.OHSASAccreditedBy = vendorApplicationForm1.OHSASAccreditedBy;
            vendorApplicationForm.ISO1AccreditedBy = vendorApplicationForm1.ISO1AccreditedBy;
            vendorApplicationForm.FSSCAccreditedBy = vendorApplicationForm1.FSSCAccreditedBy;
            vendorApplicationForm.BCAGrade = vendorApplicationForm1.BCAGrade;
            vendorApplicationForm.OtherCertfications = vendorApplicationForm1.OtherCertfications;
            vendorApplicationForm.Certifications = vendorApplicationForm1.Certifications;
            vendorApplicationForm.Societies = vendorApplicationForm1.Societies;

            foreach(var y in vendorCustomerProjectRecords1) { 
            vendorCustomerProjectRecords.CustomerName = y.CustomerName;
            vendorCustomerProjectRecords.ProjectNo = y.ProjectNo;
            vendorCustomerProjectRecords.YearOfCompletion = y.YearOfCompletion;

            vendorCustomerProjectRecords.Save();
            }

            vendorApplicationForm.VendorComment1 = vendorApplicationForm1.VendorComment1;
            vendorApplicationForm.ViolationOfHSE = vendorApplicationForm1.ViolationOfHSE;
            vendorApplicationForm.VendorComment2 = vendorApplicationForm1.VendorComment2;
            vendorApplicationForm.PoliciesManagementSystem = vendorApplicationForm1.PoliciesManagementSystem;

            vendorApplicationForm.JobScopeExperience = vendorApplicationForm1.JobScopeExperience;
            vendorApplicationForm.VendorComment3 = vendorApplicationForm1.VendorComment3;
            vendorApplicationForm.QualificationOfWorker = vendorApplicationForm1.QualificationOfWorker;
            vendorApplicationForm.VendorComment4 = vendorApplicationForm1.VendorComment4;
            vendorApplicationForm.StatutoryRequiredHSE = vendorApplicationForm1.StatutoryRequiredHSE;
            vendorApplicationForm.VendorComment5 = vendorApplicationForm1.VendorComment5;

            vendorApplicationForm.ContractorSuperVisorProvided = vendorApplicationForm1.ContractorSuperVisorProvided;
            vendorApplicationForm.VendorComment6 = vendorApplicationForm1.VendorComment6;
            vendorApplicationForm.DesignatedHSERepresentative = vendorApplicationForm1.DesignatedHSERepresentative;
            vendorApplicationForm.VendorComment7 = vendorApplicationForm1.VendorComment7;
            vendorApplicationForm.StatutoryRequiredME = vendorApplicationForm1.StatutoryRequiredME;
            vendorApplicationForm.VendorComment8 = vendorApplicationForm1.VendorComment8;
            vendorApplicationForm.HSEStandardRequirement = vendorApplicationForm1.HSEStandardRequirement;

            vendorApplicationForm.DeclarationName = vendorApplicationForm1.DeclarationName;
            vendorApplicationForm.DeclarationDesignation = vendorApplicationForm1.DeclarationDesignation;
            vendorApplicationForm.DeclarationDate = GMSUtil.ToDate(vendorApplicationForm1.DeclarationDate);

            vendorApplicationForm.ChkFinancialReport = vendorApplicationForm1.ChkFinancialReport;
            vendorApplicationForm.ChkCompanyACRA = vendorApplicationForm1.ChkCompanyACRA;
            vendorApplicationForm.ChkAuditReport = vendorApplicationForm1.ChkAuditReport;
            vendorApplicationForm.ChkEFTInformation = vendorApplicationForm1.ChkEFTInformation;
            vendorApplicationForm.ChkOrganizationChart = vendorApplicationForm1.ChkOrganizationChart;
            vendorApplicationForm.ChkHeadCount = vendorApplicationForm1.ChkHeadCount;
            vendorApplicationForm.ChkQualityManual = vendorApplicationForm1.ChkQualityManual;
            vendorApplicationForm.ChkAccreditedCertificates = vendorApplicationForm1.ChkAccreditedCertificates;
            vendorApplicationForm.ChkInsuranceCertificate = vendorApplicationForm1.ChkInsuranceCertificate;
            vendorApplicationForm.ChkTrackRecords = vendorApplicationForm1.ChkTrackRecords;
            vendorApplicationForm.ChkHSEPolicy = vendorApplicationForm1.ChkHSEPolicy;
            vendorApplicationForm.ChkQualificationCertificates = vendorApplicationForm1.ChkQualificationCertificates;
            vendorApplicationForm.ChkSafeLevel = vendorApplicationForm1.ChkSafeLevel;
            vendorApplicationForm.ChkHSETrackRecords = vendorApplicationForm1.ChkHSETrackRecords;
            vendorApplicationForm.ChkTrainingRecords = vendorApplicationForm1.ChkTrainingRecords;
            vendorApplicationForm.ChkCompanyBackground = vendorApplicationForm1.ChkCompanyBackground;
            vendorApplicationForm.ChkProductInformation = vendorApplicationForm1.ChkProductInformation;
            vendorApplicationForm.ChkTechnicalInformation = vendorApplicationForm1.ChkTechnicalInformation;
            vendorApplicationForm.ChkBrochure = vendorApplicationForm1.ChkBrochure;
            vendorApplicationForm.RandomID = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssfff"), vendorApplicationForm.VendorID);

            vendorApplicationForm.Save();
            vendorApplicationForm.Resync();
            //hidFormID.Value = vendorApplicationForm.FormID.ToString();
            LoadData();

         
            VendorApplicationForm vendorformid = new SystemDataActivity().RetrieveVendorApplicationFormIDByRandomID(session.CompanyId, vendorApplicationForm.RandomID);


            var companyID = session.CompanyId;
            var randomID = vendorApplicationForm.RandomID;
            var companyName = vendorApplicationForm1.VendorObject.CompanyName;
            var email = vendorApplicationForm1.VendorObject.Email;
            string appPath = HttpRuntime.AppDomainAppVirtualPath;
            var linktopass = "https://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS3/Procurement/Forms/VendorEvaluationForm1.aspx?RANDOMID=" + randomID + "&FORMID=" + vendorformid.FormID.ToString();
            //localhost
            //"localhost" + appPath + "/Procurement/Forms/VendorEvaluationForm1.aspx?RANDOMID=" + randomID + "&FORMID=" + vendorformid.FormID.ToString();

            //lnkVendorEvaluation.Text = lnkVendorEvaluation.NavigateUrl =  "https://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS3/Procurement/Forms/VendorEvaluationForm1.aspx?RANDOMID=" + randomID + "&FORMID=" + vendorformid.FormID.ToString();

            SystemDataActivity sDataActivity = new SystemDataActivity();
            GMSCore.Entity.VendorApplicationForm vendorAppForm = sDataActivity.RetrieveVendorApplicationFormByVendorID(GMSUtil.ToInt(hidNewVendorID.Value.Trim()));

            


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
      

            StringBuilder str = new StringBuilder();
            str.Append("<script language='javascript'>");
            str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
            str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
            str.Append("if (r) {window.location.href = \"../../Procurement/Forms/VendorEvaluationForm1.aspx\";} else " + " {window.close();} ");
            str.Append("</script>");
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);

 
        }
        #endregion

        #region dgData_ItemCommand
        protected void dgData_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DuplicateForm")
            {
                HtmlInputHidden hidFormID2 = (HtmlInputHidden)e.Item.FindControl("hidFormID2");
                short sFormID = GMSUtil.ToShort(hidFormID2.Value);
                this.DuplicateFunction(sFormID);
            }
        }
        #endregion


        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
        }
        #endregion

        #region btnAdd_Click
              protected void btnAdd_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (string.IsNullOrEmpty(hidNewVendorID.Value.Trim()))
            {
                #region Add New Record.

                GMSCore.Entity.Vendor vendor = new GMSCore.Entity.Vendor();
         

                if (txtNewVendorName.Text.Trim() == "")
                {
                    base.JScriptAlertMsg("Field cannot be empty.");
                    return;
                }
                else
                {
                        vendor.CoyID = session.CompanyId;
                        vendor.CompanyName = txtNewVendorName.Text.Trim();
                        vendor.Email = txtNewVendorEmail.Text.Trim();
                        vendor.Save();

                        txtNewVendorName.Text = "";
                        txtNewVendorEmail.Text = "";

                        this.PageMsgPanel.ShowMessage("Vendor successfully created.", MessagePanelControl.MessageEnumType.Alert);
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
            
            else
            {
                #region Update Record.

                GMSCore.Entity.Vendor vendor = GMSCore.Entity.Vendor.RetrieveByKey(GMSUtil.ToInt(hidNewVendorID.Value.Trim()));
                //if (vendorApplicationForm == null)
                //{
                //    base.JScriptAlertMsg("This session cannot be found in database.");
                //    return;
                //}
                if (txtNewVendorName.Text.Trim() == "")
                {
                    base.JScriptAlertMsg("Vendor Name cannot be empty.");
                    return;
                }
                else
                {
                    vendor.CompanyName = txtNewVendorName.Text.Trim();
                    vendor.Email = txtNewVendorEmail.Text.Trim();

                    vendor.Save();
                    vendor.Resync();
                    hidNewVendorID.Value = vendor.VendorID.ToString();
                    //LoadData();

                    Response.Redirect("VendorDetails.aspx");

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

    }
}