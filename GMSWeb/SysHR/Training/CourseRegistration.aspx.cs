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
using GMSCore.Activity;
using System.Collections.Generic;
using System.Text;
using GMSCore.Entity;

namespace GMSWeb.SysHR.Training
{
    public partial class CourseRegistration : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //LoadCourseLanguageDDL();
                if (Request.Params["COURSESESSIONID"] != null)
                {
                    LoadDDL();
                    hidCourseSessionID.Value = Request.Params["COURSESESSIONID"].ToString();
                    LoadData("NEW");
                }
                string formType = "NEW";
                if (Request.Params["FORMTYPE"] != null)
                {
                    formType = Request.Params["FORMTYPE"].ToString();
                }
                if (Request.Params["RANDOMID"] != null)
                {
                    hidRandomID.Value = Request.Params["RANDOMID"].ToString();
                }
                if (Request.Params["EMPLOYEECOURSEID"] != null)
                {
                    hidEmployeeCourseID.Value = Request.Params["EMPLOYEECOURSEID"].ToString();
                }
                switch (formType)
                {
                    case "PRETR": ConfirmRegistration();
                        break;
                    case "TR": LoadData("TR");
                        break;
                    case "EC": LoadData("EC");
                        break;
                    case "PTEF": LoadData("PTEF");
                        break;
                }
            }
        }

        private void LoadDDL()
        {
            if (ddlEmployee != null)
            {
                SystemDataActivity sDataActivity = new SystemDataActivity();
                // fill in employee dropdown list
                IList<GMSCore.Entity.Employee> lstEmployee = null;
                lstEmployee = sDataActivity.RetrieveAllEmployeeListSortByEmployeeNo();
                ddlEmployee.DataSource = lstEmployee;
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("0", "0"));
                ddlEmployee.SelectedIndex = 0;
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlNewEmployee, ddlNewEmployee.GetType(), "script1", "<script type=\"text/javascript\"> var ddlNewEmployeeID = '" + ddlNewEmployee.ClientID + "';</script>", false);
            }
        }

        #region LoadData
        private void LoadData(string formType)
        {
            #region Register
            if (formType == "NEW")
            {
                if (hidCourseSessionID.Value != "")
                {
                    divRegister.Visible = true;
                    divApproval.Visible = false;
                    divStatus.Visible = false;
                    divAdminFunctions.Visible = false;
                    fsAfterCourseEvaluation.Visible = false;
                    this.Title = "Course Registration Page";
                    PageHeader.InnerText = "Training > Course Registration";
                    GMSCore.Entity.CourseSession courseSession = GMSCore.Entity.CourseSession.RetrieveByKey(GMSUtil.ToInt(hidCourseSessionID.Value.Trim()));
                    if (courseSession != null)
                    {
                        lblCourseCode.Text = courseSession.CourseObject.CourseCode;
                        lblCourseTitle.Text = courseSession.CourseObject.CourseTitle;
                        lblCourseOrganizer.Text = courseSession.CourseObject.CourseOrganizerObject.OrganizerName;
                        lblCourseType.Text = (courseSession.CourseObject.CourseType == "I") ? "Internal" : "External";
                        lblDateAndTime.Text = courseSession.DateFrom.Value.ToString("dd/MM/yyyy HH:mm") +
                                                " to " + courseSession.DateTo.Value.ToString("dd/MM/yyyy HH:mm");
                        lblCourseLanguage.Text = courseSession.CourseLanguageObject.LanguageName;
                        lblVenue.Text = courseSession.Venue;
                        lblFacilitator.Text = courseSession.Facilitator;
                        lblTargetAudience.Text = courseSession.CourseObject.TargetAudience;
                        lblCourseObjective.Text = courseSession.CourseObject.CourseObjective;
                        lblCourseDescription.Text = courseSession.CourseObject.CourseDescription;
                        lblPrerequisite.Text = courseSession.CourseObject.Prerequisite;
                        if (courseSession.LocalCourseFee != null)
                            lblLocalCourseFee.Text = courseSession.LocalCourseFee.Value.ToString("#0.00");
                        if (courseSession.OverseasFlightCost != null)
                            lblOverseasFlightCost.Text = courseSession.OverseasFlightCost.Value.ToString("#0.00");
                        if (courseSession.LocalRegistrationFee != null)
                            lblLocalRegistrationFee.Text = courseSession.LocalRegistrationFee.Value.ToString("#0.00");
                        if (courseSession.OverseasHotelCost != null)
                            lblOverseasHotelCost.Text = courseSession.OverseasHotelCost.Value.ToString("#0.00");
                        if (courseSession.LocalExaminationFee != null)
                            lblLocalExaminationFee.Text = courseSession.LocalExaminationFee.Value.ToString("#0.00");
                        if (courseSession.OverseasTransportCost != null)
                            lblOverseasTransportCost.Text = courseSession.OverseasTransportCost.Value.ToString("#0.00");
                        if (courseSession.LocalMembershipFee != null)
                            lblLocalMembershipFee.Text = courseSession.LocalMembershipFee.Value.ToString("#0.00");
                        if (courseSession.OverseasMealCost != null)
                            lblOverseasMealCost.Text = courseSession.OverseasMealCost.Value.ToString("#0.00");
                        if (courseSession.LocalGST != null)
                            lblLocalGST.Text = courseSession.LocalGST.Value.ToString("#0.00");
                        if (courseSession.OverseasOthers != null)
                            lblOverseasOthers.Text = courseSession.OverseasOthers.Value.ToString("#0.00");
                        if (courseSession.CourseObject.RequirePTJNPTEF)
                        {
                            this.preJustification.Visible = true;
                        }
                        else
                        {
                            this.preJustification.Visible = false;
                        }

                        if (!base.IsSessionActive())
                        {
                            LogSession session = base.GetSessionInfo();
                            if (session != null)
                            {
                                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                            86);
                                if (uAccess != null)
                                {
                                    btnAdd.Visible = true;
                                    btnAddSendTEF.Visible = true;
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Approval
            if (formType == "TR")
            {
                FormApproval fa = new FormActivity().RetrieveFormApporvalByRandomID(hidRandomID.Value);
                if (fa != null)
                {
                    divRegister.Visible = false;
                    divApproval.Visible = true;
                    divStaffDetails.Visible = true;
                    divStatus.Visible = false;
                    divAdminFunctions.Visible = false;
                    this.fsAfterCourseEvaluation.Visible = false;
                    this.Title = "Course Approval";
                    this.PageHeader.InnerText = "Training > Course Approval";
                    GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(fa.FormID);
                    GMSCore.Entity.CourseSession courseSession = eCourse.CourseSessionObject;
                    if (courseSession != null)
                    {
                        lblCourseCode.Text = courseSession.CourseObject.CourseCode;
                        lblCourseTitle.Text = courseSession.CourseObject.CourseTitle;
                        lblCourseOrganizer.Text = courseSession.CourseObject.CourseOrganizerObject.OrganizerName;
                        lblCourseType.Text = (courseSession.CourseObject.CourseType == "I") ? "Internal" : "External";
                        lblDateAndTime.Text = courseSession.DateFrom.Value.ToString("dd/MM/yyyy HH:mm") +
                                                " to " + courseSession.DateTo.Value.ToString("dd/MM/yyyy HH:mm");
                        lblCourseLanguage.Text = courseSession.CourseLanguageObject.LanguageName;
                        lblVenue.Text = courseSession.Venue;
                        lblFacilitator.Text = courseSession.Facilitator;
                        lblTargetAudience.Text = courseSession.CourseObject.TargetAudience;
                        lblCourseObjective.Text = courseSession.CourseObject.CourseObjective;
                        lblCourseDescription.Text = courseSession.CourseObject.CourseDescription;
                        lblPrerequisite.Text = courseSession.CourseObject.Prerequisite;
                        if (courseSession.LocalCourseFee != null)
                            lblLocalCourseFee.Text = courseSession.LocalCourseFee.Value.ToString("#0.00");
                        if (courseSession.OverseasFlightCost != null)
                            lblOverseasFlightCost.Text = courseSession.OverseasFlightCost.Value.ToString("#0.00");
                        if (courseSession.LocalRegistrationFee != null)
                            lblLocalRegistrationFee.Text = courseSession.LocalRegistrationFee.Value.ToString("#0.00");
                        if (courseSession.OverseasHotelCost != null)
                            lblOverseasHotelCost.Text = courseSession.OverseasHotelCost.Value.ToString("#0.00");
                        if (courseSession.LocalExaminationFee != null)
                            lblLocalExaminationFee.Text = courseSession.LocalExaminationFee.Value.ToString("#0.00");
                        if (courseSession.OverseasTransportCost != null)
                            lblOverseasTransportCost.Text = courseSession.OverseasTransportCost.Value.ToString("#0.00");
                        if (courseSession.LocalMembershipFee != null)
                            lblLocalMembershipFee.Text = courseSession.LocalMembershipFee.Value.ToString("#0.00");
                        if (courseSession.OverseasMealCost != null)
                            lblOverseasMealCost.Text = courseSession.OverseasMealCost.Value.ToString("#0.00");
                        if (courseSession.LocalGST != null)
                            lblLocalGST.Text = courseSession.LocalGST.Value.ToString("#0.00");
                        if (courseSession.OverseasOthers != null)
                            lblOverseasOthers.Text = courseSession.OverseasOthers.Value.ToString("#0.00");
                        if (courseSession.CourseObject.RequirePTJNPTEF)
                        {
                            this.preJustification.Visible = true;
                            txtLearningObjectives.Text = eCourse.LearningObjectives;
                            txtLearningObjectives.ReadOnly = true;
                            txtActionPlan.Text = eCourse.ActionPlan;
                            txtActionPlan.ReadOnly = true;
                            txtActionPlanDueDate.Text = eCourse.ActionPlanDueDate.Value.ToString("dd/MM/yyyy");
                            txtActionPlanDueDate.ReadOnly = true;
                            txtPurposeOfActionPlan.Text = eCourse.PurposeOfActionPlan;
                            txtPurposeOfActionPlan.ReadOnly = true;
                            txtTypeOfIndicator.Text = eCourse.TypeOfIndicator;
                            txtTypeOfIndicator.ReadOnly = true;
                            if (eCourse.CurrentValue != null)
                                txtCurrentValue.Text = eCourse.CurrentValue.Value.ToString("#0.00");
                            txtCurrentValue.ReadOnly = true;
                            if (eCourse.ExpectedValue != null)
                                txtExpectedValue.Text = eCourse.ExpectedValue.Value.ToString("#0.00");
                            txtExpectedValue.ReadOnly = true;
                        }
                        else
                        {
                            this.preJustification.Visible = false;
                        }
                        lblEmployeeNo.Text = eCourse.EmployeeObject.EmployeeNo;
                        lblEmployeeName.Text = eCourse.EmployeeObject.Name;
                        lblDepartment.Text = eCourse.EmployeeObject.Department;
                        lblDesignation.Text = eCourse.EmployeeObject.Designation;
                        if (eCourse.EmployeeObject.SuperiorObject != null)
                            lblSupervisor.Text = eCourse.EmployeeObject.SuperiorObject.Name;
                    }
                }
                else
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The registration record cannot be found.");
                    return;
                }
            }
            #endregion

            #region EmployeeCourse
            if (formType == "EC")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("HR"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                86);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("HR"));

                GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(GMSUtil.ToInt(hidEmployeeCourseID.Value));
                if (eCourse != null)
                {
                    divRegister.Visible = false;
                    divApproval.Visible = false;
                    divStaffDetails.Visible = true;
                    divStatus.Visible = true;
                    divAdminFunctions.Visible = true;
                    trUpdatePTEF.Visible = false;
                    this.Title = "Training Record";
                    this.PageHeader.InnerText = "Training > Training Record";
                    GMSCore.Entity.CourseSession courseSession = eCourse.CourseSessionObject;
                    if (courseSession != null)
                    {
                        lblCourseCode.Text = courseSession.CourseObject.CourseCode;
                        lblCourseTitle.Text = courseSession.CourseObject.CourseTitle;
                        lblCourseOrganizer.Text = courseSession.CourseObject.CourseOrganizerObject.OrganizerName;
                        lblCourseType.Text = (courseSession.CourseObject.CourseType == "I") ? "Internal" : "External";
                        lblDateAndTime.Text = courseSession.DateFrom.Value.ToString("dd/MM/yyyy HH:mm") +
                                                " to " + courseSession.DateTo.Value.ToString("dd/MM/yyyy HH:mm");
                        lblCourseLanguage.Text = courseSession.CourseLanguageObject.LanguageName;
                        lblVenue.Text = courseSession.Venue;
                        lblFacilitator.Text = courseSession.Facilitator;
                        lblTargetAudience.Text = courseSession.CourseObject.TargetAudience;
                        lblCourseObjective.Text = courseSession.CourseObject.CourseObjective;
                        lblCourseDescription.Text = courseSession.CourseObject.CourseDescription;
                        lblPrerequisite.Text = courseSession.CourseObject.Prerequisite;
                        if (courseSession.LocalCourseFee != null)
                            lblLocalCourseFee.Text = courseSession.LocalCourseFee.Value.ToString("#0.00");
                        if (courseSession.OverseasFlightCost != null)
                            lblOverseasFlightCost.Text = courseSession.OverseasFlightCost.Value.ToString("#0.00");
                        if (courseSession.LocalRegistrationFee != null)
                            lblLocalRegistrationFee.Text = courseSession.LocalRegistrationFee.Value.ToString("#0.00");
                        if (courseSession.OverseasHotelCost != null)
                            lblOverseasHotelCost.Text = courseSession.OverseasHotelCost.Value.ToString("#0.00");
                        if (courseSession.LocalExaminationFee != null)
                            lblLocalExaminationFee.Text = courseSession.LocalExaminationFee.Value.ToString("#0.00");
                        if (courseSession.OverseasTransportCost != null)
                            lblOverseasTransportCost.Text = courseSession.OverseasTransportCost.Value.ToString("#0.00");
                        if (courseSession.LocalMembershipFee != null)
                            lblLocalMembershipFee.Text = courseSession.LocalMembershipFee.Value.ToString("#0.00");
                        if (courseSession.OverseasMealCost != null)
                            lblOverseasMealCost.Text = courseSession.OverseasMealCost.Value.ToString("#0.00");
                        if (courseSession.LocalGST != null)
                            lblLocalGST.Text = courseSession.LocalGST.Value.ToString("#0.00");
                        if (courseSession.OverseasOthers != null)
                            lblOverseasOthers.Text = courseSession.OverseasOthers.Value.ToString("#0.00");
                        if (courseSession.CourseObject.RequirePTJNPTEF)
                        {
                            this.preJustification.Visible = true;
                            txtLearningObjectives.Text = eCourse.LearningObjectives;
                            txtActionPlan.Text = eCourse.ActionPlan;
                            txtActionPlanDueDate.Text = eCourse.ActionPlanDueDate.Value.ToString("dd/MM/yyyy");
                            txtPurposeOfActionPlan.Text = eCourse.PurposeOfActionPlan;
                            txtTypeOfIndicator.Text = eCourse.TypeOfIndicator;
                            if (eCourse.CurrentValue != null)
                                txtCurrentValue.Text = eCourse.CurrentValue.Value.ToString("#0.00");
                            if (eCourse.ExpectedValue != null)
                                txtExpectedValue.Text = eCourse.ExpectedValue.Value.ToString("#0.00");

                            this.fsAfterCourseEvaluation.Visible = true;
                            if (eCourse.IsObjectiveAchievedAfterCourse != null)
                            {
                                if (eCourse.IsObjectiveAchievedAfterCourse.Value)
                                {
                                    rbIsObjectiveAchievedAfterCourse.Checked = true;
                                    this.trObjectiveNotAchievedRemark1.Attributes["style"] = "display:none";
                                    this.trObjectiveNotAchievedRemark2.Attributes["style"] = "display:none";
                                }
                                else
                                {
                                    rbIsObjectiveNotAchievedAfterCourse.Checked = true;
                                    this.trObjectiveNotAchievedRemark1.Attributes["style"] = "";
                                    this.trObjectiveNotAchievedRemark2.Attributes["style"] = "";
                                }
                            }
                            if (eCourse.IsActionPlanCompletedAfterCourse != null)
                            {
                                if (eCourse.IsActionPlanCompletedAfterCourse.Value)
                                {
                                    rbIsActionPlanCompletedAfterCourse.Checked = true;
                                    this.trActionPlanNotCompletedRemark1.Attributes["style"] = "display:none";
                                    this.trActionPlanNotCompletedRemark2.Attributes["style"] = "display:none";
                                }
                                else
                                {
                                    rbIsActionPlanNotCompletedAfterCourse.Checked = true;
                                    this.trActionPlanNotCompletedRemark1.Attributes["style"] = "";
                                    this.trActionPlanNotCompletedRemark2.Attributes["style"] = "";
                                }
                            }
                            else
                            {
                                //this.trActionExtendedDuedate1.Attributes["style"] = "display:none";
                                //this.trActionExtendedDuedate2.Attributes["style"] = "display:none";
                            }
                            if (eCourse.ActionPlanExtendedDueDate != null)
                                txtActionPlanExtendedDueDate.Text = eCourse.ActionPlanExtendedDueDate.Value.ToString("dd/MM/yyyy");
                            if (eCourse.ActualValueAfterCourse != null)
                                txtActualValue.Text = eCourse.ActualValueAfterCourse.Value.ToString("#0.00");
                            if (eCourse.ObjectiveNotAchievedRemark != null)
                                txtObjectiveNotAchievedRemark.Text = eCourse.ObjectiveNotAchievedRemark;
                            if (eCourse.ActionPlanNotCompletedRemark != null)
                                txtActionPlanNotCompletedRemark.Text = eCourse.ActionPlanNotCompletedRemark;
                        }
                        else
                        {
                            this.preJustification.Visible = false;
                        }
                        lblEmployeeNo.Text = eCourse.EmployeeObject.EmployeeNo;
                        lblEmployeeName.Text = eCourse.EmployeeObject.Name;
                        lblDepartment.Text = eCourse.EmployeeObject.Department;
                        lblDesignation.Text = eCourse.EmployeeObject.Designation;
                        if (eCourse.EmployeeObject.SuperiorObject != null)
                            lblSupervisor.Text = eCourse.EmployeeObject.SuperiorObject.Name;
                        if (eCourse.Status == "A")
                        {
                            btnApproveOnBehalf.Visible = false;
                            btnRejectOnBehalf.Visible = false;
                        }

                        //if (eCourse.ROIAfterCourse != null)
                        //    txtROI.Text = eCourse.ROIAfterCourse.Value.ToString("#0.00");
                        if (eCourse.IsBonded != null && eCourse.IsBonded.Value)
                            chkIsBonded.Checked = true;
                        if (eCourse.NoOfMonthsBonded != null)
                            txtNoOfMonthsBonded.Text = eCourse.NoOfMonthsBonded.ToString();
                        if (eCourse.BondContractLocation != null)
                            txtBondContractLocation.Text = eCourse.BondContractLocation;
                        if (eCourse.CertificateLocation != null)
                            txtCertificateLocation.Text = eCourse.CertificateLocation;
                        if (eCourse.BondExpiredDate != null)
                            txtBondExpiredDate.Text = eCourse.BondExpiredDate.Value.ToString("dd/MM/yyyy");
                        if (eCourse.LicenceExpiredDate != null)
                            txtLicenceExpiredDate.Text = eCourse.LicenceExpiredDate.Value.ToString("dd/MM/yyyy");
                        if (eCourse.PaymentLocation != null)
                            txtPaymentLocation.Text = eCourse.PaymentLocation;
                        if (eCourse.SDF != null)
                            txtSDF.Text = eCourse.SDF.Value.ToString("#0.00");
                        if (eCourse.SRP != null)
                            txtSRP.Text = eCourse.SRP.Value.ToString("#0.00");
                        if (eCourse.SDFApplicationDate != null)
                            txtSDFApplicationDate.Text = eCourse.SDFApplicationDate.Value.ToString("dd/MM/yyyy");
                        if (eCourse.SDFApplicationNo != null)
                            txtSDFApplicationNo.Text = eCourse.SDFApplicationNo;
                        if (eCourse.SDFDisbursementEmailLocation != null)
                            txtSDFDisbursementEmailLocation.Text = eCourse.SDFDisbursementEmailLocation;
                        if (eCourse.Remarks != null)
                            txtRemarks.Text = eCourse.Remarks;

                        lblEmployeeCourseStatus.Text = "Course Registration Status : " + ((eCourse.Status == "A") ? "<span style=\"color:#ff0000\">Approved</span>" : ((eCourse.Status == "R") ? "<span style=\"color:#ff0000\">Rejected</span>" : "<span style=\"color:#ff0000\">Pending</span>"));
                        lblEmployeeCourseStatus.Attributes.Add("style", "display:block");
                        if (eCourse.FormApprovalList != null && eCourse.FormApprovalList.Count > 0)
                        {
                            for (int i = 0; i < eCourse.FormApprovalList.Count; i++)
                            {
                                FormApproval fa = eCourse.FormApprovalList[i];
                                if (fa.FormType == "PRETR")
                                {
                                    switch (fa.ApprovalStatus)
                                    {
                                        case "P":
                                            Label lbConfirmed = new Label();
                                            lbConfirmed.Text = "- Pending Confirmation By " + fa.ApprovedEmployeeObject.Name;
                                            lbConfirmed.Attributes.Add("style", "display:block");
                                            tdStatus.Controls.Add(lbConfirmed);
                                            this.btnApproveOnBehalf.Visible = false;
                                            this.btnRejectOnBehalf.Visible = false;
                                            break;
                                        case "A":
                                            Label lbConfirmed2 = new Label();
                                            lbConfirmed2.Text = "- Confirmed By " + fa.ApprovedEmployeeObject.Name
                                                        + " On " + fa.ApprovalModifiedDate.Value.ToString("dd/MM/yyyy HH:mm");
                                            lbConfirmed2.Attributes.Add("style", "display:block");
                                            tdStatus.Controls.Add(lbConfirmed2);
                                            break;
                                    }
                                }
                            }
                            for (int i = 0; i < eCourse.FormApprovalList.Count; i++)
                            {
                                FormApproval fa = eCourse.FormApprovalList[i];
                                if (fa.FormType == "TR")
                                {
                                    switch (fa.ApprovalStatus)
                                    {
                                        case "P":
                                            Label lb = new Label();
                                            lb.Text = "- Pending Approval By " + fa.ApprovedEmployeeObject.Name;
                                            lb.Attributes.Add("style", "display:block");
                                            tdStatus.Controls.Add(lb);
                                            break;
                                        case "A":
                                            if (fa.ApprovedOnBehalfUser != null && fa.ApprovedOnBehalfUser.Value > 0)
                                            {
                                                Label lb2 = new Label();
                                                lb2.Text = "- Approved On Behalf Of " + fa.ApprovedEmployeeObject.Name + " By " + fa.ApprovedOnBehalfUserGMSUsersObject.UserRealName +
                                                            " On " + fa.ApprovedOnBehalfDate.Value.ToString("dd/MM/yyyy HH:mm");
                                                lb2.Attributes.Add("style", "display:block");
                                                tdStatus.Controls.Add(lb2);
                                            }
                                            else
                                            {
                                                Label lb3 = new Label();
                                                lb3.Text = "- Approved By " + fa.ApprovedEmployeeObject.Name
                                                            + " On " + fa.ApprovalModifiedDate.Value.ToString("dd/MM/yyyy HH:mm");
                                                lb3.Attributes.Add("style", "display:block");
                                                tdStatus.Controls.Add(lb3);
                                            }
                                            break;
                                        case "R":
                                            if (fa.ApprovedOnBehalfUser != null && fa.ApprovedOnBehalfUser.Value > 0)
                                            {
                                                Label lb4 = new Label();
                                                lb4.Text = "- Rejected On Behalf Of " + fa.ApprovedEmployeeObject.Name + " By " + fa.ApprovedOnBehalfUserGMSUsersObject.UserRealName +
                                                            " On " + fa.ApprovedOnBehalfDate.Value.ToString("dd/MM/yyyy HH:mm");
                                                lb4.Attributes.Add("style", "display:block");
                                                tdStatus.Controls.Add(lb4);
                                            }
                                            else
                                            {
                                                Label lb5 = new Label();
                                                lb5.Text = "- Rejected By " + fa.ApprovedEmployeeObject.Name
                                                            + " On " + fa.ApprovalModifiedDate.Value.ToString("dd/MM/yyyy HH:mm");
                                                lb5.Attributes.Add("style", "display:block");
                                                tdStatus.Controls.Add(lb5);
                                            }
                                            break;
                                    }
                                }
                            }
                            for (int i = 0; i < eCourse.FormApprovalList.Count; i++)
                            {
                                FormApproval fa = eCourse.FormApprovalList[i];
                                if (fa.FormType == "TEF")
                                {
                                    switch (fa.ApprovalStatus)
                                    {
                                        case "P":
                                            Label lbTEF = new Label();
                                            lbTEF.Text = "- Pending Training Evaluation Form By " + fa.ApprovedEmployeeObject.Name;
                                            lbTEF.Attributes.Add("style", "display:block");
                                            tdStatus.Controls.Add(lbTEF);
                                            break;
                                        case "A":
                                            if (fa.ApprovedOnBehalfUser != null && fa.ApprovedOnBehalfUser.Value > 0)
                                            {
                                                Label lbTEF1 = new Label();
                                                lbTEF1.Text = "- Training Evaluation Form Updated By " + fa.ApprovedOnBehalfUserGMSUsersObject.UserRealName
                                                            + " On " + fa.ApprovedOnBehalfDate.Value.ToString("dd/MM/yyyy HH:mm");
                                                lbTEF1.Attributes.Add("style", "display:block");
                                                tdStatus.Controls.Add(lbTEF1);
                                            }
                                            else
                                            {
                                                Label lbTEF2 = new Label();
                                                lbTEF2.Text = "- Training Evaluation Form Filled By " + fa.ApprovedEmployeeObject.Name
                                                            + " On " + fa.ApprovalModifiedDate.Value.ToString("dd/MM/yyyy HH:mm");
                                                lbTEF2.Attributes.Add("style", "display:block");
                                                tdStatus.Controls.Add(lbTEF2);
                                            }
                                            break;
                                    }
                                }
                            }
                            for (int i = 0; i < eCourse.FormApprovalList.Count; i++)
                            {
                                FormApproval fa = eCourse.FormApprovalList[i];
                                if (fa.FormType == "PTEF")
                                {
                                    switch (fa.ApprovalStatus)
                                    {
                                        case "P":
                                            Label lbPTEF = new Label();
                                            lbPTEF.Text = "- Pending Post Training Evaluation Form By " + fa.ApprovedEmployeeObject.Name;
                                            lbPTEF.Attributes.Add("style", "display:block");
                                            tdStatus.Controls.Add(lbPTEF);
                                            break;
                                        case "A":
                                            if (fa.ApprovedOnBehalfUser != null && fa.ApprovedOnBehalfUser.Value > 0)
                                            {
                                                Label lbPTEF1 = new Label();
                                                lbPTEF1.Text = "- Post Training Evaluation Filled Form Updated By " + fa.ApprovedOnBehalfUserGMSUsersObject.UserRealName
                                                            + " On " + fa.ApprovedOnBehalfDate.Value.ToString("dd/MM/yyyy HH:mm");
                                                lbPTEF1.Attributes.Add("style", "display:block");
                                                tdStatus.Controls.Add(lbPTEF1);
                                            }
                                            else
                                            {
                                                Label lbPTEF2 = new Label();
                                                lbPTEF2.Text = "- Post Training Evaluation Filled Form Filled By " + fa.ApprovedEmployeeObject.Name
                                                            + " On " + fa.ApprovalModifiedDate.Value.ToString("dd/MM/yyyy HH:mm");
                                                lbPTEF2.Attributes.Add("style", "display:block");
                                                tdStatus.Controls.Add(lbPTEF2);
                                            }
                                            break;
                                    }
                                }
                            }
                            foreach (FormApproval fa in eCourse.FormApprovalList)
                            {
                                if (fa.ApprovalStatus == "P" && (fa.FormType == "PRETR" || fa.FormType == "TR" || fa.FormType == "TEF" || fa.FormType == "PTEF"))
                                {
                                    switch (fa.FormType)
                                    {
                                        case "PRETR":
                                            btnResendPRETR.Visible = true;
                                            break;
                                        case "TR":
                                            btnResendTR.Visible = true;
                                            break;
                                        case "TEF":
                                            btnResendTEF.Visible = true;
                                            break;
                                        case "PTEF":
                                            btnResendPTEF.Visible = true;
                                            break;

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The training record cannot be found.");
                    return;
                }
            }
            #endregion

            #region PTEF
            if (formType == "PTEF")
            {
                FormApproval fa = new FormActivity().RetrieveFormApporvalByRandomID(hidRandomID.Value);
                if (fa != null)
                {
                    divRegister.Visible = false;
                    divApproval.Visible = false;
                    divStaffDetails.Visible = true;
                    divStatus.Visible = false;
                    divAdminFunctions.Visible = false;
                    this.fsAfterCourseEvaluation.Visible = true;
                    this.Title = "Course Post Course Evaluation";
                    this.PageHeader.InnerText = "Training > Post Course Evaluation";
                    GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(fa.FormID);
                    GMSCore.Entity.CourseSession courseSession = eCourse.CourseSessionObject;
                    if (courseSession != null)
                    {
                        lblCourseCode.Text = courseSession.CourseObject.CourseCode;
                        lblCourseTitle.Text = courseSession.CourseObject.CourseTitle;
                        lblCourseOrganizer.Text = courseSession.CourseObject.CourseOrganizerObject.OrganizerName;
                        lblCourseType.Text = (courseSession.CourseObject.CourseType == "I") ? "Internal" : "External";
                        lblDateAndTime.Text = courseSession.DateFrom.Value.ToString("dd/MM/yyyy HH:mm") +
                                                " to " + courseSession.DateTo.Value.ToString("dd/MM/yyyy HH:mm");
                        lblCourseLanguage.Text = courseSession.CourseLanguageObject.LanguageName;
                        lblVenue.Text = courseSession.Venue;
                        lblFacilitator.Text = courseSession.Facilitator;
                        lblTargetAudience.Text = courseSession.CourseObject.TargetAudience;
                        lblCourseObjective.Text = courseSession.CourseObject.CourseObjective;
                        lblCourseDescription.Text = courseSession.CourseObject.CourseDescription;
                        lblPrerequisite.Text = courseSession.CourseObject.Prerequisite;
                        if (courseSession.LocalCourseFee != null)
                            lblLocalCourseFee.Text = courseSession.LocalCourseFee.Value.ToString("#0.00");
                        if (courseSession.OverseasFlightCost != null)
                            lblOverseasFlightCost.Text = courseSession.OverseasFlightCost.Value.ToString("#0.00");
                        if (courseSession.LocalRegistrationFee != null)
                            lblLocalRegistrationFee.Text = courseSession.LocalRegistrationFee.Value.ToString("#0.00");
                        if (courseSession.OverseasHotelCost != null)
                            lblOverseasHotelCost.Text = courseSession.OverseasHotelCost.Value.ToString("#0.00");
                        if (courseSession.LocalExaminationFee != null)
                            lblLocalExaminationFee.Text = courseSession.LocalExaminationFee.Value.ToString("#0.00");
                        if (courseSession.OverseasTransportCost != null)
                            lblOverseasTransportCost.Text = courseSession.OverseasTransportCost.Value.ToString("#0.00");
                        if (courseSession.LocalMembershipFee != null)
                            lblLocalMembershipFee.Text = courseSession.LocalMembershipFee.Value.ToString("#0.00");
                        if (courseSession.OverseasMealCost != null)
                            lblOverseasMealCost.Text = courseSession.OverseasMealCost.Value.ToString("#0.00");
                        if (courseSession.LocalGST != null)
                            lblLocalGST.Text = courseSession.LocalGST.Value.ToString("#0.00");
                        if (courseSession.OverseasOthers != null)
                            lblOverseasOthers.Text = courseSession.OverseasOthers.Value.ToString("#0.00");
                        if (courseSession.CourseObject.RequirePTJNPTEF)
                        {
                            this.preJustification.Visible = true;
                            txtLearningObjectives.Text = eCourse.LearningObjectives;
                            txtLearningObjectives.ReadOnly = true;
                            txtActionPlan.Text = eCourse.ActionPlan;
                            txtActionPlan.ReadOnly = true;
                            txtActionPlanDueDate.Text = eCourse.ActionPlanDueDate.Value.ToString("dd/MM/yyyy");
                            txtActionPlanDueDate.ReadOnly = true;
                            txtPurposeOfActionPlan.Text = eCourse.PurposeOfActionPlan;
                            txtPurposeOfActionPlan.ReadOnly = true;
                            txtTypeOfIndicator.Text = eCourse.TypeOfIndicator;
                            txtTypeOfIndicator.ReadOnly = true;
                            if (eCourse.CurrentValue != null)
                                txtCurrentValue.Text = eCourse.CurrentValue.Value.ToString("#0.00");
                            txtCurrentValue.ReadOnly = true;
                            if (eCourse.ExpectedValue != null)
                                txtExpectedValue.Text = eCourse.ExpectedValue.Value.ToString("#0.00");
                            txtExpectedValue.ReadOnly = true;

                            this.fsAfterCourseEvaluation.Visible = true;
                            if (eCourse.IsObjectiveAchievedAfterCourse != null)
                            {
                                if (eCourse.IsObjectiveAchievedAfterCourse.Value)
                                    rbIsObjectiveAchievedAfterCourse.Checked = true;
                                else
                                    rbIsObjectiveNotAchievedAfterCourse.Checked = true;
                            }
                            if (eCourse.IsActionPlanCompletedAfterCourse != null)
                            {
                                if (eCourse.IsActionPlanCompletedAfterCourse.Value)
                                {
                                    rbIsActionPlanCompletedAfterCourse.Checked = true;
                                    //this.trActionExtendedDuedate1.Attributes["style"] = "display:none";
                                    //this.trActionExtendedDuedate2.Attributes["style"] = "display:none";
                                }
                                else
                                {
                                    rbIsActionPlanNotCompletedAfterCourse.Checked = true;
                                    //this.trActionExtendedDuedate1.Attributes["style"] = "";
                                    //this.trActionExtendedDuedate2.Attributes["style"] = "";
                                }
                            }
                            else
                            {
                                //this.trActionExtendedDuedate1.Attributes["style"] = "display:none";
                                //this.trActionExtendedDuedate2.Attributes["style"] = "display:none";
                            }
                            if (eCourse.ActionPlanExtendedDueDate != null)
                                txtActionPlanExtendedDueDate.Text = eCourse.ActionPlanExtendedDueDate.Value.ToString("dd/MM/yyyy");
                            if (eCourse.ActualValueAfterCourse != null)
                                txtActualValue.Text = eCourse.ActualValueAfterCourse.Value.ToString("#0.00");
                        }
                        else
                        {
                            this.preJustification.Visible = false;
                        }
                        lblEmployeeNo.Text = eCourse.EmployeeObject.EmployeeNo;
                        lblEmployeeName.Text = eCourse.EmployeeObject.Name;
                        lblDepartment.Text = eCourse.EmployeeObject.Department;
                        lblDesignation.Text = eCourse.EmployeeObject.Designation;
                        if (eCourse.EmployeeObject.SuperiorObject != null)
                            lblSupervisor.Text = eCourse.EmployeeObject.SuperiorObject.Name;
                    }
                }
                else
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The registration record cannot be found.");
                    return;
                }
            }
            #endregion
        }
        #endregion

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            #region Add New Record.
            try
            {
                GMSCore.Entity.EmployeeCourse eCourse = new GMSCore.Entity.EmployeeCourse();

                if (txtEmployeeName.Text.Trim() == "")
                {
                    base.JScriptAlertMsg("Please key in your name.");
                    return;
                }
                if (ddlEmployee.SelectedIndex == 0)
                {
                    base.JScriptAlertMsg("The employee name is incorrect. Please input the correct name.");
                    return;
                }
                else
                {
                    eCourse.EmployeeID = short.Parse(ddlEmployee.SelectedValue);
                }

                GMSCore.Entity.Employee em = GMSCore.Entity.Employee.RetrieveByKey(eCourse.EmployeeID);
                if (em == null)
                {
                    base.JScriptAlertMsg("Employee record cannot be found. Please check with your administrator.");
                    return;
                }
                if (string.IsNullOrEmpty(em.EmailAddress))
                {
                    base.JScriptAlertMsg("The Email address of " + em.Name + " has not been set yet. Please contact HR to update the staff record.");
                    return;
                }
                GMSCore.Entity.CourseSession courseSession = GMSCore.Entity.CourseSession.RetrieveByKey(GMSUtil.ToInt(hidCourseSessionID.Value.Trim()));
                if (courseSession.CourseObject.RequirePTJNPTEF)
                {
                    if (txtLearningObjectives.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("You need to fill in your learning objectives.");
                        return;
                    }
                    else
                    {
                        eCourse.LearningObjectives = txtLearningObjectives.Text.Trim();
                    }
                    if (txtActionPlan.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("You need to fill in your action plan.");
                        return;
                    }
                    else
                    {
                        eCourse.ActionPlan = txtActionPlan.Text.Trim();
                    }
                    if (txtActionPlanDueDate.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("You need to fill in your action plan due date.");
                        return;
                    }
                    else
                    {
                        eCourse.ActionPlanDueDate = GMSUtil.ToDate(txtActionPlanDueDate.Text.Trim());
                    }
                    if (txtPurposeOfActionPlan.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("You need to fill in your purpose of action plan.");
                        return;
                    }
                    else
                    {
                        eCourse.PurposeOfActionPlan = txtPurposeOfActionPlan.Text.Trim();
                    }
                    if (txtTypeOfIndicator.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("You need to fill in your type of indicator for KPI.");
                        return;
                    }
                    else
                    {
                        eCourse.TypeOfIndicator = txtTypeOfIndicator.Text.Trim();
                    }
                    if (txtCurrentValue.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("You need to fill in your current KPI value.");
                        return;
                    }
                    else
                    {
                        eCourse.CurrentValue = GMSUtil.ToDecimal(txtCurrentValue.Text.Trim());
                    }
                    if (txtExpectedValue.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("You need to fill in your expected KPI value.");
                        return;
                    }
                    else
                    {
                        eCourse.ExpectedValue = GMSUtil.ToDecimal(txtExpectedValue.Text.Trim());
                    }
                }
                eCourse.CourseSessionID = courseSession.CourseSessionID;
                eCourse.Status = "P";
                eCourse.Save();
                eCourse.Resync();
                GMSCore.Entity.FormApproval fa = new GMSCore.Entity.FormApproval();
                fa.FormType = "PRETR";
                fa.FormID = eCourse.EmployeeCourseID;
                fa.ApprovalLevel = 0;
                fa.ApprovalStatus = "P";
                fa.ApprovalCreatedDate = DateTime.Now;
                fa.ApprovedEmployeeID = em.EmployeeID;
                fa.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("Pre-TR" + DateTime.Now.Ticks.ToString(), "MD5");
                fa.Save();
                fa.Resync();
                SendConfirmationEmail(eCourse.EmployeeCourseID);

                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have successfully registered this course. A confirmation Email will be sent to your mailbox.");
                return;
            }
            catch (Exception ex)
            {
                base.JScriptAlertMsg(ex.Message);
                return;
            }
            #endregion
        }
        #endregion

        #region btnAdd_Click
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            #region Add New Record.
            try
            {
                GMSCore.Entity.EmployeeCourse eCourseTemp = new EmployeeCourseActivity().RetrieveEmployeeCourseByCourseSessionIDEmployeeID(GMSUtil.ToInt(hidCourseSessionID.Value.Trim()), short.Parse(ddlEmployee.SelectedValue));

                if (eCourseTemp == null)
                {
                    GMSCore.Entity.EmployeeCourse eCourse = new GMSCore.Entity.EmployeeCourse();

                    if (txtEmployeeName.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("Please key in your name.");
                        return;
                    }
                    if (ddlEmployee.SelectedIndex == 0)
                    {
                        base.JScriptAlertMsg("The employee name is incorrect. Please input the correct name.");
                        return;
                    }
                    else
                    {
                        eCourse.EmployeeID = short.Parse(ddlEmployee.SelectedValue);
                    }
                    GMSCore.Entity.Employee em = GMSCore.Entity.Employee.RetrieveByKey(eCourse.EmployeeID);
                    if (em == null)
                    {
                        base.JScriptAlertMsg("Employee record cannot be found. Please check with your administrator.");
                        return;
                    }
                    if (string.IsNullOrEmpty(em.EmailAddress))
                    {
                        base.JScriptAlertMsg("The Email address of " + em.Name + " has not been set yet. Please contact HR to update the staff record.");
                        return;
                    }
                    GMSCore.Entity.CourseSession courseSession = GMSCore.Entity.CourseSession.RetrieveByKey(GMSUtil.ToInt(hidCourseSessionID.Value.Trim()));
                    if (courseSession.CourseObject.RequirePTJNPTEF)
                    {
                        if (txtLearningObjectives.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your learning objectives.");
                            return;
                        }
                        else
                        {
                            eCourse.LearningObjectives = txtLearningObjectives.Text.Trim();
                        }
                        if (txtActionPlan.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your action plan.");
                            return;
                        }
                        else
                        {
                            eCourse.ActionPlan = txtActionPlan.Text.Trim();
                        }
                        if (txtActionPlanDueDate.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your action plan due date.");
                            return;
                        }
                        else
                        {
                            eCourse.ActionPlanDueDate = GMSUtil.ToDate(txtActionPlanDueDate.Text.Trim());
                        }
                        if (txtPurposeOfActionPlan.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your purpose of action plan.");
                            return;
                        }
                        else
                        {
                            eCourse.PurposeOfActionPlan = txtPurposeOfActionPlan.Text.Trim();
                        }
                        if (txtTypeOfIndicator.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your type of indicator for KPI.");
                            return;
                        }
                        else
                        {
                            eCourse.TypeOfIndicator = txtTypeOfIndicator.Text.Trim();
                        }
                        if (txtCurrentValue.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your current KPI value.");
                            return;
                        }
                        else
                        {
                            eCourse.CurrentValue = GMSUtil.ToDecimal(txtCurrentValue.Text.Trim());
                        }
                        if (txtExpectedValue.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your expected KPI value.");
                            return;
                        }
                        else
                        {
                            eCourse.ExpectedValue = GMSUtil.ToDecimal(txtExpectedValue.Text.Trim());
                        }
                    }
                    eCourse.CourseSessionID = courseSession.CourseSessionID;
                    eCourse.Status = "A";
                    eCourse.Save();
                    eCourse.Resync();
                    FormApproval tefApproval = new FormApproval();
                    tefApproval.FormType = "TEF";
                    tefApproval.FormID = eCourse.EmployeeCourseID;
                    tefApproval.ApprovalLevel = 0;
                    tefApproval.ApprovalCreatedDate = DateTime.Now;
                    tefApproval.ApprovalStatus = "P";
                    tefApproval.ApprovedEmployeeID = eCourse.EmployeeID;
                    tefApproval.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TEF" + DateTime.Now.Ticks.ToString(), "MD5");
                    TEF tef = new TEF();
                    tef.FormID = eCourse.EmployeeCourseID;
                    tef.Status = "P";
                    if (courseSession.CourseObject.RequirePTJNPTEF)
                    {
                        FormApproval faPTEF = new FormApproval();
                        faPTEF.FormType = "PTEF";
                        faPTEF.FormID = eCourse.EmployeeCourseID;
                        faPTEF.ApprovalLevel = 0;
                        faPTEF.ApprovalCreatedDate = DateTime.Now;
                        faPTEF.ApprovalStatus = "P";
                        faPTEF.ApprovedEmployeeID = em.SuperiorID;
                        faPTEF.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("PTEF" + DateTime.Now.Ticks.ToString(), "MD5");
                        faPTEF.Save();
                        faPTEF.Resync();
                    }
                    tefApproval.Save();
                    tefApproval.Resync();
                    tef.Save();
                    tef.Resync();


                    //SendTEFEmail(GMSUtil.ToInt(eCourse.EmployeeCourseID));

                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=You have added the record! Add another new one?';");
                    str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
                    str.Append("if (r) {window.location.href = \"CourseRegistration.aspx?COURSESESSIONID=" + courseSession.CourseSessionID + "\";} else " +
                                        " {window.close();} ");
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);


                    return;
                }
                else
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Employee existed in this course session!. Add another new one?';");
                    str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
                    str.Append("if (r) {window.location.href = \"CourseRegistration.aspx?COURSESESSIONID=" + hidCourseSessionID.Value.Trim() + "\";} else " +
                                        " {window.close();} ");
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                    
                }
            }
            catch (Exception ex)
            {
                base.JScriptAlertMsg(ex.Message);
                return;
            }
            #endregion
        }
        #endregion

        #region btnAddSendTEF_Click
        protected void btnAddSendTEF_Click(object sender, EventArgs e)
        {
            #region Add New Record.
            try
            {
                GMSCore.Entity.EmployeeCourse eCourseTemp = new EmployeeCourseActivity().RetrieveEmployeeCourseByCourseSessionIDEmployeeID(GMSUtil.ToInt(hidCourseSessionID.Value.Trim()), short.Parse(ddlEmployee.SelectedValue));

                if (eCourseTemp == null)
                {
                    GMSCore.Entity.EmployeeCourse eCourse = new GMSCore.Entity.EmployeeCourse();

                    if (txtEmployeeName.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("Please key in your name.");
                        return;
                    }
                    if (ddlEmployee.SelectedIndex == 0)
                    {
                        base.JScriptAlertMsg("The employee name is incorrect. Please input the correct name.");
                        return;
                    }
                    else
                    {
                        eCourse.EmployeeID = short.Parse(ddlEmployee.SelectedValue);
                    }
                    GMSCore.Entity.Employee em = GMSCore.Entity.Employee.RetrieveByKey(eCourse.EmployeeID);
                    if (em == null)
                    {
                        base.JScriptAlertMsg("Employee record cannot be found. Please check with your administrator.");
                        return;
                    }
                    if (string.IsNullOrEmpty(em.EmailAddress))
                    {
                        base.JScriptAlertMsg("The Email address of " + em.Name + " has not been set yet. Please contact HR to update the staff record.");
                        return;
                    }
                    GMSCore.Entity.CourseSession courseSession = GMSCore.Entity.CourseSession.RetrieveByKey(GMSUtil.ToInt(hidCourseSessionID.Value.Trim()));
                    if (courseSession.CourseObject.RequirePTJNPTEF)
                    {
                        if (txtLearningObjectives.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your learning objectives.");
                            return;
                        }
                        else
                        {
                            eCourse.LearningObjectives = txtLearningObjectives.Text.Trim();
                        }
                        if (txtActionPlan.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your action plan.");
                            return;
                        }
                        else
                        {
                            eCourse.ActionPlan = txtActionPlan.Text.Trim();
                        }
                        if (txtActionPlanDueDate.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your action plan due date.");
                            return;
                        }
                        else
                        {
                            eCourse.ActionPlanDueDate = GMSUtil.ToDate(txtActionPlanDueDate.Text.Trim());
                        }
                        if (txtPurposeOfActionPlan.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your purpose of action plan.");
                            return;
                        }
                        else
                        {
                            eCourse.PurposeOfActionPlan = txtPurposeOfActionPlan.Text.Trim();
                        }
                        if (txtTypeOfIndicator.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your type of indicator for KPI.");
                            return;
                        }
                        else
                        {
                            eCourse.TypeOfIndicator = txtTypeOfIndicator.Text.Trim();
                        }
                        if (txtCurrentValue.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your current KPI value.");
                            return;
                        }
                        else
                        {
                            eCourse.CurrentValue = GMSUtil.ToDecimal(txtCurrentValue.Text.Trim());
                        }
                        if (txtExpectedValue.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your expected KPI value.");
                            return;
                        }
                        else
                        {
                            eCourse.ExpectedValue = GMSUtil.ToDecimal(txtExpectedValue.Text.Trim());
                        }
                    }
                    eCourse.CourseSessionID = courseSession.CourseSessionID;
                    eCourse.Status = "A";
                    eCourse.Save();
                    eCourse.Resync();
                    FormApproval tefApproval = new FormApproval();
                    tefApproval.FormType = "TEF";
                    tefApproval.FormID = eCourse.EmployeeCourseID;
                    tefApproval.ApprovalLevel = 0;
                    tefApproval.ApprovalCreatedDate = DateTime.Now;
                    tefApproval.ApprovalStatus = "P";
                    tefApproval.ApprovedEmployeeID = eCourse.EmployeeID;
                    tefApproval.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TEF" + DateTime.Now.Ticks.ToString(), "MD5");
                    TEF tef = new TEF();
                    tef.FormID = eCourse.EmployeeCourseID;
                    tef.Status = "P";
                    if (courseSession.CourseObject.RequirePTJNPTEF)
                    {
                        FormApproval faPTEF = new FormApproval();
                        faPTEF.FormType = "PTEF";
                        faPTEF.FormID = eCourse.EmployeeCourseID;
                        faPTEF.ApprovalLevel = 0;
                        faPTEF.ApprovalCreatedDate = DateTime.Now;
                        faPTEF.ApprovalStatus = "P";
                        faPTEF.ApprovedEmployeeID = em.SuperiorID;
                        faPTEF.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("PTEF" + DateTime.Now.Ticks.ToString(), "MD5");
                        faPTEF.Save();
                        faPTEF.Resync();
                    }
                    tefApproval.Save();
                    tefApproval.Resync();
                    tef.Save();
                    tef.Resync();


                    SendTEFEmail(GMSUtil.ToInt(eCourse.EmployeeCourseID));

                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=You have added the record! Add another new one?';");
                    str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
                    str.Append("if (r) {window.location.href = \"CourseRegistration.aspx?COURSESESSIONID=" + courseSession.CourseSessionID + "\";} else " +
                                        " {window.close();} ");
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);


                    return;
                }
                else
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Employee existed in this course session!. Add another new one?';");
                    str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
                    str.Append("if (r) {window.location.href = \"CourseRegistration.aspx?COURSESESSIONID=" + hidCourseSessionID.Value.Trim() + "\";} else " +
                                        " {window.close();} ");
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);

                }
            }
            catch (Exception ex)
            {
                base.JScriptAlertMsg(ex.Message);
                return;
            }
            #endregion
        }
        #endregion

        #region SendConfirmationEmail
        private void SendConfirmationEmail(int eCourseID)
        {
            GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(eCourseID);
            GMSCore.Entity.FormApproval fa = FormApproval.RetrieveByKey("PRETR", eCourseID, 0);
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(eCourse.EmployeeObject.EmailAddress, eCourse.EmployeeObject.Name);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);

            mail.ReplyTo = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Admin");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[Training - Confirmation of Course Registration]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dear " + eCourse.EmployeeObject.Name + ",</p>" +
                        "<p>You have registered the following course. <br />" +
                        "<table><tr><td>Course Provider</td><td>:</td><td>" + eCourse.CourseSessionObject.CourseObject.CourseOrganizerObject.OrganizerName + "</td></tr>" +
                        "<tr><td>Course Title</td><td>:</td><td>" + eCourse.CourseSessionObject.CourseObject.CourseTitle + "</td></tr>" +
                        "<tr><td>Course Date</td><td>:</td><td>" + eCourse.CourseSessionObject.DateFrom.Value.ToString("dd/MM/yyyy HH:mm") + " to " + eCourse.CourseSessionObject.DateTo.Value.ToString("dd/MM/yyyy HH:mm") + "</td></tr>" +
                        "<tr><td>Employee Enrolled</td><td>:</td><td>" + eCourse.EmployeeObject.Name + "</td></tr></table></p>" +
                        "<p>Please click the link below to confirm your registration. Please ignore this Email if you have not registered for this course." + ".<br />" +
                        "<a href=\"https://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS4/SysHR/Training/CourseRegistration.aspx?RANDOMID=" + fa.ApprovalRandomID + "&FORMTYPE=PRETR" + "\">Click here.</a></p>" +

                        "***** This is a computer-generated email. Please do not reply.*****";
            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.Host = smtpServer;
                mailClient.Port = 25;
                mailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential authentication = new System.Net.NetworkCredential("gmsadmin@leedenlimited.com", "admin2008");
                mailClient.Credentials = authentication;
                mailClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ConfirmRegistration
        private void ConfirmRegistration()
        {
            FormApproval fa = new FormActivity().RetrieveFormApporvalByRandomID(hidRandomID.Value);
            if (fa != null && fa.ApprovalStatus == "P")
            {
                fa.ApprovalRandomID = "";
                fa.ApprovalStatus = "A";
                fa.ApprovalModifiedDate = DateTime.Now;
                GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(fa.FormID);
                if (!eCourse.EmployeeObject.IsUnitHead)
                {
                    if (eCourse.EmployeeObject.SuperiorObject != null && !string.IsNullOrEmpty(eCourse.EmployeeObject.SuperiorObject.EmailAddress))
                    {
                        FormApproval fa2 = new FormApproval();
                        fa2.FormType = "TR";
                        fa2.FormID = eCourse.EmployeeCourseID;
                        fa2.ApprovalLevel = 1;
                        fa2.ApprovalStatus = "P";
                        fa2.ApprovalCreatedDate = DateTime.Now;
                        fa2.ApprovedEmployeeID = eCourse.EmployeeObject.SuperiorObject.EmployeeID;
                        fa2.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TR" + DateTime.Now.Ticks.ToString(), "MD5");
                        fa2.Save();
                        fa2.Resync();
                        SendApprovalEmail(eCourse, eCourse.EmployeeObject.SuperiorObject, fa2);
                        fa.Save();
                        fa.Resync();
                        Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The registration for this course has been confirmed and it is pending for approval. You will be notified when the course has been approved by the relevant parties.");
                        return;
                    }
                    else
                    {
                        Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The supervisor information of Employee " + eCourse.EmployeeObject.Name + " is not updated.");
                        return;
                    }
                }
                else
                {
                    if (eCourse.EmployeeObject.EmployeeNo != "0001")
                    {
                        FormApproval fa2 = new FormApproval();
                        fa2.FormType = "TR";
                        fa2.FormID = eCourse.EmployeeCourseID;
                        fa2.ApprovalLevel = 1;
                        fa2.ApprovalStatus = "P";
                        fa2.ApprovalCreatedDate = DateTime.Now;
                        fa2.ApprovedEmployeeID = eCourse.EmployeeObject.SuperiorObject.EmployeeID;
                        fa2.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TR" + DateTime.Now.Ticks.ToString(), "MD5");
                        fa2.Save();
                        fa2.Resync();
                        SendApprovalEmail(eCourse, eCourse.EmployeeObject.SuperiorObject, fa2);
                        fa.Save();
                        fa.Resync();
                        Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The registration for this course has been confirmed and it is pending for approval. You will be notified when the course has been approved by the relevant parties.");
                        return;
                    }
                    else
                    {
                        eCourse.Status = "A";
                        SendNotificationEmail(eCourse.EmployeeObject.EmailAddress, eCourse.EmployeeObject.Name,
                                            eCourse.EmployeeObject.Name, eCourse);
                        SendNotificationEmail(eCourse.EmployeeObject.SuperiorObject.EmailAddress, eCourse.EmployeeObject.SuperiorObject.Name,
                                            eCourse.EmployeeObject.Name, eCourse);
                        IList<NotificationParty> fsnList = new SystemDataActivity().RetrieveNotificationPartyListByFormTypeStatus("TR", "A");
                        foreach (NotificationParty fsn in fsnList)
                        {
                            SendNotificationEmail(fsn.EmployeeObject.EmailAddress, "", eCourse.EmployeeObject.Name,
                                        eCourse);
                        }
                        FormApproval tefApproval = new FormApproval();
                        tefApproval.FormType = "TEF";
                        tefApproval.FormID = eCourse.EmployeeCourseID;
                        tefApproval.ApprovalLevel = 0;
                        tefApproval.ApprovalCreatedDate = DateTime.Now;
                        tefApproval.ApprovalStatus = "P";
                        tefApproval.ApprovedEmployeeID = eCourse.EmployeeID;
                        tefApproval.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TEF" + DateTime.Now.Ticks.ToString(), "MD5");
                        TEF tef = new TEF();
                        tef.FormID = eCourse.EmployeeCourseID;
                        tef.Status = "P";
                        if (eCourse.CourseSessionObject.CourseObject.RequirePTJNPTEF)
                        {
                            FormApproval faPTEF = new FormApproval();
                            faPTEF.FormType = "PTEF";
                            faPTEF.FormID = eCourse.EmployeeCourseID;
                            faPTEF.ApprovalLevel = 0;
                            faPTEF.ApprovalCreatedDate = DateTime.Now;
                            faPTEF.ApprovalStatus = "P";
                            faPTEF.ApprovedEmployeeID = eCourse.EmployeeObject.SuperiorID;
                            faPTEF.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("PTEF" + DateTime.Now.Ticks.ToString(), "MD5");
                            faPTEF.Save();
                            faPTEF.Resync();
                        }
                        eCourse.Save();
                        eCourse.Resync();
                        fa.Save();
                        fa.Resync();
                        tefApproval.Save();
                        tefApproval.Resync();
                        tef.Save();
                        tef.Resync();
                        Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The registration for this course has been confirmed and it is pending for approval. You will be notified when the course has been approved by the relevant parties.");
                        return;
                    }
                }
            }
            else
            {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The registration record cannot be found.");
                return;
            }

        }
        #endregion

        #region SendApprovalEmail
        private void SendApprovalEmail(GMSCore.Entity.EmployeeCourse eCourse, GMSCore.Entity.Employee em, FormApproval fa)
        {
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(em.EmailAddress, em.Name);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);

            mail.ReplyTo = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Admin");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[Training - Approval of Course Registration]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dear " + em.Name + ",</p>" +
                        "<p>" + eCourse.EmployeeObject.Name + " has registered the following course. <br />" +
                        "<table><tr><td>Course Provider</td><td>:</td><td>" + eCourse.CourseSessionObject.CourseObject.CourseOrganizerObject.OrganizerName + "</td></tr>" +
                        "<tr><td>Course Title</td><td>:</td><td>" + eCourse.CourseSessionObject.CourseObject.CourseTitle + "</td></tr>" +
                        "<tr><td>Course Date</td><td>:</td><td>" + eCourse.CourseSessionObject.DateFrom.Value.ToString("dd/MM/yyyy HH:mm") + " to " + eCourse.CourseSessionObject.DateTo.Value.ToString("dd/MM/yyyy HH:mm") + "</td></tr>" +
                        "<tr><td>Employee Enrolled</td><td>:</td><td>" + eCourse.EmployeeObject.Name + "</td></tr></table></p>" +
                        "<p>Please click the link below to approve or reject the enrolment." + ".<br />" +
                        "<a href=\"https://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS4/SysHR/Training/CourseRegistration.aspx?RANDOMID=" + fa.ApprovalRandomID + "&FORMTYPE=TR" + "\">Click here.</a></p>" +

                        "***** This is a computer-generated email. Please do not reply.*****";
            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.Host = smtpServer;
                mailClient.Port = 25;
                mailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential authentication = new System.Net.NetworkCredential("gmsadmin@leedenlimited.com", "admin2008");
                mailClient.Credentials = authentication;
                mailClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SendNotificationEmail
        private void SendNotificationEmail(string userEmail, string userRealName, string applicantName, GMSCore.Entity.EmployeeCourse eCourse)
        {
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);

            mail.ReplyTo = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Admin");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[Training - Course Registration Approved]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dear " + userRealName + ",</p>\n" +
                        "<p>The course registration submitted by " + applicantName + " has been approved.\n<br /></p>" +
                //"<a href=\"http://sgcitrix.leedenlimited.com/GMS4/SysHR/Training/AddEditCEF.aspx?CEFID=" + eCourse.CEFID.ToString() + "\">Click here to view.</a></p>\n" +
                        "<p><table><tr><td>Course Provider</td><td>:</td><td>" + eCourse.CourseSessionObject.CourseObject.CourseOrganizerObject.OrganizerName + "</td></tr>" +
                        "<tr><td>Course Title</td><td>:</td><td>" + eCourse.CourseSessionObject.CourseObject.CourseTitle + "</td></tr>" +
                        "<tr><td>Course Date</td><td>:</td><td>" + eCourse.CourseSessionObject.DateFrom.Value.ToString("dd/MM/yyyy HH:mm") + " to " + eCourse.CourseSessionObject.DateTo.Value.ToString("dd/MM/yyyy HH:mm") + "</td></tr>" +
                        "<tr><td>Employee Enrolled</td><td>:</td><td>" + eCourse.EmployeeObject.Name + "</td></tr></table></p>" +
                        "***** This is a computer-generated email. Please do not reply.*****";

            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.Host = smtpServer;
                mailClient.Port = 25;
                mailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential authentication = new System.Net.NetworkCredential("gmsadmin@leedenlimited.com", "admin2008");
                mailClient.Credentials = authentication;
                mailClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ApproveRegistration
        protected void ApproveRegistration(object sender, EventArgs e)
        {
            FormApproval fa = new FormActivity().RetrieveFormApporvalByRandomID(hidRandomID.Value);
            if (fa != null && fa.ApprovalStatus == "P")
            {
                fa.ApprovalRandomID = "";
                fa.ApprovalStatus = "A";
                fa.ApprovalModifiedDate = DateTime.Now;
                GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(fa.FormID);
                if (!eCourse.EmployeeObject.IsUnitHead)
                {
                    Employee superior = Employee.RetrieveByKey(fa.ApprovedEmployeeID.Value);
                    if (!superior.IsUnitHead)
                    {
                        if (superior.SuperiorObject != null && !string.IsNullOrEmpty(eCourse.EmployeeObject.SuperiorObject.EmailAddress))
                        {
                            FormApproval fa2 = new FormApproval();
                            fa2.FormType = "TR";
                            fa2.FormID = eCourse.EmployeeCourseID;
                            fa2.ApprovalLevel = fa.ApprovalLevel++;
                            fa2.ApprovalStatus = "P";
                            fa2.ApprovalCreatedDate = DateTime.Now;
                            fa2.ApprovedEmployeeID = superior.SuperiorObject.EmployeeID;
                            fa2.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TR" + DateTime.Now.Ticks.ToString(), "MD5");
                            fa2.Save();
                            fa2.Resync();
                            SendApprovalEmail(eCourse, superior.SuperiorObject, fa2);
                            fa.Save();
                            fa.Resync();
                            Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have approved the application");
                            return;
                        }
                        else
                        {
                            Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The supervisor information of Employee " + superior.Name + " is not updated.");
                            return;
                        }
                    }
                    else
                    {
                        eCourse.Status = "A";
                        SendNotificationEmail(eCourse.EmployeeObject.EmailAddress, eCourse.EmployeeObject.Name,
                                            eCourse.EmployeeObject.Name, eCourse);
                        SendNotificationEmail(eCourse.EmployeeObject.SuperiorObject.EmailAddress, eCourse.EmployeeObject.SuperiorObject.Name,
                                            eCourse.EmployeeObject.Name, eCourse);
                        IList<NotificationParty> fsnList = new SystemDataActivity().RetrieveNotificationPartyListByFormTypeStatus("TR", "A");
                        foreach (NotificationParty fsn in fsnList)
                        {
                            SendNotificationEmail(fsn.EmployeeObject.EmailAddress, "", eCourse.EmployeeObject.Name,
                                       eCourse);
                        }
                        FormApproval tefApproval = new FormApproval();
                        tefApproval.FormType = "TEF";
                        tefApproval.FormID = eCourse.EmployeeCourseID;
                        tefApproval.ApprovalLevel = 0;
                        tefApproval.ApprovalCreatedDate = DateTime.Now;
                        tefApproval.ApprovalStatus = "P";
                        tefApproval.ApprovedEmployeeID = eCourse.EmployeeID;
                        tefApproval.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TEF" + DateTime.Now.Ticks.ToString(), "MD5");
                        TEF tef = new TEF();
                        tef.FormID = eCourse.EmployeeCourseID;
                        tef.Status = "P";
                        if (eCourse.CourseSessionObject.CourseObject.RequirePTJNPTEF)
                        {
                            FormApproval faPTEF = new FormApproval();
                            faPTEF.FormType = "PTEF";
                            faPTEF.FormID = eCourse.EmployeeCourseID;
                            faPTEF.ApprovalLevel = 0;
                            faPTEF.ApprovalCreatedDate = DateTime.Now;
                            faPTEF.ApprovalStatus = "P";
                            faPTEF.ApprovedEmployeeID = eCourse.EmployeeObject.SuperiorID;
                            faPTEF.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("PTEF" + DateTime.Now.Ticks.ToString(), "MD5");
                            faPTEF.Save();
                            faPTEF.Resync();
                        }
                        eCourse.Save();
                        eCourse.Resync();
                        fa.Save();
                        fa.Resync();
                        tefApproval.Save();
                        tefApproval.Resync();
                        tef.Save();
                        tef.Resync();
                        Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have approved the application");
                        return;
                    }
                }
                else
                {
                    Employee superior = Employee.RetrieveByKey(fa.ApprovedEmployeeID.Value);
                    if (superior.EmployeeNo != "0001")
                    {
                        if (superior.SuperiorObject != null && !string.IsNullOrEmpty(eCourse.EmployeeObject.SuperiorObject.EmailAddress))
                        {
                            FormApproval fa2 = new FormApproval();
                            fa2.FormType = "TR";
                            fa2.FormID = eCourse.EmployeeCourseID;
                            fa2.ApprovalLevel = fa.ApprovalLevel++;
                            fa2.ApprovalStatus = "P";
                            fa2.ApprovalCreatedDate = DateTime.Now;
                            fa2.ApprovedEmployeeID = superior.SuperiorObject.EmployeeID;
                            fa2.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TR" + DateTime.Now.Ticks.ToString(), "MD5");
                            fa2.Save();
                            fa2.Resync();
                            SendApprovalEmail(eCourse, superior.SuperiorObject, fa2);
                            fa.Save();
                            fa.Resync();
                            Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have approved the application");
                            return;
                        }
                        else
                        {
                            Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The supervisor information of Employee " + superior.Name + " is not updated.");
                            return;
                        }
                    }
                    else
                    {
                        eCourse.Status = "A";
                        SendNotificationEmail(eCourse.EmployeeObject.EmailAddress, eCourse.EmployeeObject.Name,
                                            eCourse.EmployeeObject.Name, eCourse);
                        SendNotificationEmail(eCourse.EmployeeObject.SuperiorObject.EmailAddress, eCourse.EmployeeObject.SuperiorObject.Name,
                                            eCourse.EmployeeObject.Name, eCourse);
                        IList<NotificationParty> fsnList = new SystemDataActivity().RetrieveNotificationPartyListByFormTypeStatus("TR", "A");
                        foreach (NotificationParty fsn in fsnList)
                        {
                            SendNotificationEmail(fsn.EmployeeObject.EmailAddress, "", eCourse.EmployeeObject.Name,
                                       eCourse);
                        }
                        FormApproval tefApproval = new FormApproval();
                        tefApproval.FormType = "TEF";
                        tefApproval.FormID = eCourse.EmployeeCourseID;
                        tefApproval.ApprovalLevel = 0;
                        tefApproval.ApprovalCreatedDate = DateTime.Now;
                        tefApproval.ApprovalStatus = "P";
                        tefApproval.ApprovedEmployeeID = eCourse.EmployeeID;
                        tefApproval.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TEF" + DateTime.Now.Ticks.ToString(), "MD5");
                        TEF tef = new TEF();
                        tef.FormID = eCourse.EmployeeCourseID;
                        tef.Status = "P";
                        if (eCourse.CourseSessionObject.CourseObject.RequirePTJNPTEF)
                        {
                            FormApproval faPTEF = new FormApproval();
                            faPTEF.FormType = "PTEF";
                            faPTEF.FormID = eCourse.EmployeeCourseID;
                            faPTEF.ApprovalLevel = 0;
                            faPTEF.ApprovalCreatedDate = DateTime.Now;
                            faPTEF.ApprovalStatus = "P";
                            faPTEF.ApprovedEmployeeID = eCourse.EmployeeObject.SuperiorID;
                            faPTEF.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("PTEF" + DateTime.Now.Ticks.ToString(), "MD5");
                            faPTEF.Save();
                            faPTEF.Resync();
                        }
                        eCourse.Save();
                        eCourse.Resync();
                        fa.Save();
                        fa.Resync();
                        tefApproval.Save();
                        tefApproval.Resync();
                        tef.Save();
                        tef.Resync();
                        Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have approved the application");
                        return;
                    }
                }
            }
            else
            {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The registration record cannot be found. Please try again or contact Administrator");
                return;
            }
        }
        #endregion

        #region RejectRegistration
        protected void RejectRegistration(object sender, EventArgs e)
        {
            FormApproval fa = new FormActivity().RetrieveFormApporvalByRandomID(hidRandomID.Value);
            if (fa != null && fa.ApprovalStatus == "P")
            {
                fa.ApprovalRandomID = "";
                fa.ApprovalStatus = "R";
                fa.ApprovalModifiedDate = DateTime.Now;
                GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(fa.FormID);
                Employee rejectPerson = Employee.RetrieveByKey(fa.ApprovedEmployeeID.Value);
                eCourse.Status = "R";
                //Send to applicant
                SendRejectEmail(eCourse.EmployeeObject.EmailAddress, eCourse.EmployeeObject.Name, eCourse.EmployeeObject.Name,
                                rejectPerson.Name, eCourse);
                //Send to reject person
                SendRejectEmail(rejectPerson.EmailAddress, rejectPerson.Name, eCourse.EmployeeObject.Name,
                                rejectPerson.Name, eCourse);
                //Send to notification party
                IList<NotificationParty> fsnList = new SystemDataActivity().RetrieveNotificationPartyListByFormTypeStatus("TR", "R");
                foreach (NotificationParty fsn in fsnList)
                {
                    SendRejectEmail(fsn.EmployeeObject.EmailAddress, "", eCourse.EmployeeObject.Name,
                                rejectPerson.Name, eCourse);
                }
                fa.Save();
                fa.Resync();
                eCourse.Save();
                eCourse.Resync();
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have rejected the application");
                return;
            }
            else
            {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The registration record cannot be found. Please try again or contact Administrator");
                return;
            }
        }
        #endregion

        #region SendRejectEmail
        private void SendRejectEmail(string userEmail, string userRealName, string applicantName, string rejectedName, GMSCore.Entity.EmployeeCourse eCourse)
        {
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);

            mail.ReplyTo = new System.Net.Mail.MailAddress("ray.tong@leedenlimited.com", "Tong Rui, Ray");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[Training - Course Registration Rejected]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dear " + userRealName + ",</p>\n" +
                        "<p>The course registration submitted by " + applicantName + " has been rejected by " + rejectedName + ".\n</p>" +
                        "<p><table><tr><td>Course Provider</td><td>:</td><td>" + eCourse.CourseSessionObject.CourseObject.CourseOrganizerObject.OrganizerName + "</td></tr>" +
                        "<tr><td>Course Title</td><td>:</td><td>" + eCourse.CourseSessionObject.CourseObject.CourseTitle + "</td></tr>" +
                        "<tr><td>Course Date</td><td>:</td><td>" + eCourse.CourseSessionObject.DateFrom.Value.ToString("dd/MM/yyyy HH:mm") + " to " + eCourse.CourseSessionObject.DateTo.Value.ToString("dd/MM/yyyy HH:mm") + "</td></tr>" +
                        "<tr><td>Employee Enrolled</td><td>:</td><td>" + eCourse.EmployeeObject.Name + "</td></tr></table></p>" +
                        "***** This is a computer-generated email. Please do not reply.*****";

            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.Host = smtpServer;
                mailClient.Port = 25;
                mailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential authentication = new System.Net.NetworkCredential("gmsadmin@leedenlimited.com", "admin2008");
                mailClient.Credentials = authentication;
                mailClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ApproveOnBehalf
        protected void ApproveOnBehalf(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("HR"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            86);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("HR"));

            GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(GMSUtil.ToInt(hidEmployeeCourseID.Value));
            if (eCourse != null)
            {
                FormApproval fa = null;
                foreach (FormApproval fa1 in eCourse.FormApprovalList)
                {
                    if (fa1.ApprovalStatus == "P" && fa1.FormType == "TR")
                    {
                        fa = fa1;
                    }
                }
                if (fa != null)
                {
                    fa.ApprovalRandomID = "";
                    fa.ApprovalStatus = "A";
                    fa.ApprovedOnBehalfUser = session.UserId;
                    fa.ApprovedOnBehalfDate = DateTime.Now;
                    if (!eCourse.EmployeeObject.IsUnitHead)
                    {
                        Employee superior = Employee.RetrieveByKey(fa.ApprovedEmployeeID.Value);
                        if (!superior.IsUnitHead)
                        {
                            if (superior.SuperiorObject != null && !string.IsNullOrEmpty(eCourse.EmployeeObject.SuperiorObject.EmailAddress))
                            {
                                FormApproval fa2 = new FormApproval();
                                fa2.FormType = "TR";
                                fa2.FormID = eCourse.EmployeeCourseID;
                                fa2.ApprovalLevel = fa.ApprovalLevel++;
                                fa2.ApprovalStatus = "P";
                                fa2.ApprovalCreatedDate = DateTime.Now;
                                fa2.ApprovedEmployeeID = superior.SuperiorObject.EmployeeID;
                                fa2.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TR" + DateTime.Now.Ticks.ToString(), "MD5");
                                fa2.Save();
                                fa2.Resync();
                                SendApprovalEmail(eCourse, superior.SuperiorObject, fa2);
                                fa.Save();
                                fa.Resync();
                                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have approved the application");
                                return;
                            }
                            else
                            {
                                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The supervisor information of Employee " + superior.Name + " is not updated.");
                                return;
                            }
                        }
                        else
                        {
                            eCourse.Status = "A";
                            SendNotificationEmail(eCourse.EmployeeObject.EmailAddress, eCourse.EmployeeObject.Name,
                                                eCourse.EmployeeObject.Name, eCourse);
                            SendNotificationEmail(eCourse.EmployeeObject.SuperiorObject.EmailAddress, eCourse.EmployeeObject.SuperiorObject.Name,
                                            eCourse.EmployeeObject.Name, eCourse);
                            IList<NotificationParty> fsnList = new SystemDataActivity().RetrieveNotificationPartyListByFormTypeStatus("TR", "A");
                            foreach (NotificationParty fsn in fsnList)
                            {
                                SendNotificationEmail(fsn.EmployeeObject.EmailAddress, "", eCourse.EmployeeObject.Name,
                                           eCourse);
                            }
                            FormApproval tefApproval = new FormApproval();
                            tefApproval.FormType = "TEF";
                            tefApproval.FormID = eCourse.EmployeeCourseID;
                            tefApproval.ApprovalLevel = 0;
                            tefApproval.ApprovalCreatedDate = DateTime.Now;
                            tefApproval.ApprovalStatus = "P";
                            tefApproval.ApprovedEmployeeID = eCourse.EmployeeID;
                            tefApproval.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TEF" + DateTime.Now.Ticks.ToString(), "MD5");
                            TEF tef = new TEF();
                            tef.FormID = eCourse.EmployeeCourseID;
                            tef.Status = "P";
                            if (eCourse.CourseSessionObject.CourseObject.RequirePTJNPTEF)
                            {
                                FormApproval faPTEF = new FormApproval();
                                faPTEF.FormType = "PTEF";
                                faPTEF.FormID = eCourse.EmployeeCourseID;
                                faPTEF.ApprovalLevel = 0;
                                faPTEF.ApprovalCreatedDate = DateTime.Now;
                                faPTEF.ApprovalStatus = "P";
                                faPTEF.ApprovedEmployeeID = eCourse.EmployeeObject.SuperiorID;
                                faPTEF.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("PTEF" + DateTime.Now.Ticks.ToString(), "MD5");
                                faPTEF.Save();
                                faPTEF.Resync();
                            }
                            eCourse.Save();
                            eCourse.Resync();
                            fa.Save();
                            fa.Resync();
                            tefApproval.Save();
                            tefApproval.Resync();
                            tef.Save();
                            tef.Resync();
                            Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have approved the application");
                            return;
                        }
                    }
                    else
                    {
                        Employee superior = Employee.RetrieveByKey(fa.ApprovedEmployeeID.Value);
                        if (superior.EmployeeNo != "0001")
                        {
                            if (superior.SuperiorObject != null && !string.IsNullOrEmpty(eCourse.EmployeeObject.SuperiorObject.EmailAddress))
                            {
                                FormApproval fa2 = new FormApproval();
                                fa2.FormType = "TR";
                                fa2.FormID = eCourse.EmployeeCourseID;
                                fa2.ApprovalLevel = fa.ApprovalLevel++;
                                fa2.ApprovalStatus = "P";
                                fa2.ApprovalCreatedDate = DateTime.Now;
                                fa2.ApprovedEmployeeID = superior.SuperiorObject.EmployeeID;
                                fa2.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TR" + DateTime.Now.Ticks.ToString(), "MD5");
                                fa2.Save();
                                fa2.Resync();
                                SendApprovalEmail(eCourse, superior.SuperiorObject, fa2);
                                fa.Save();
                                fa.Resync();
                                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have approved the application");
                                return;
                            }
                            else
                            {
                                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The supervisor information of Employee " + superior.Name + " is not updated.");
                                return;
                            }
                        }
                        else
                        {
                            eCourse.Status = "A";
                            SendNotificationEmail(eCourse.EmployeeObject.EmailAddress, eCourse.EmployeeObject.Name,
                                                eCourse.EmployeeObject.Name, eCourse);
                            SendNotificationEmail(eCourse.EmployeeObject.SuperiorObject.EmailAddress, eCourse.EmployeeObject.SuperiorObject.Name,
                                            eCourse.EmployeeObject.Name, eCourse);
                            IList<NotificationParty> fsnList = new SystemDataActivity().RetrieveNotificationPartyListByFormTypeStatus("TR", "A");
                            foreach (NotificationParty fsn in fsnList)
                            {
                                SendNotificationEmail(fsn.EmployeeObject.EmailAddress, "", eCourse.EmployeeObject.Name,
                                           eCourse);
                            }
                            FormApproval tefApproval = new FormApproval();
                            tefApproval.FormType = "TEF";
                            tefApproval.FormID = eCourse.EmployeeCourseID;
                            tefApproval.ApprovalLevel = 0;
                            tefApproval.ApprovalCreatedDate = DateTime.Now;
                            tefApproval.ApprovalStatus = "P";
                            tefApproval.ApprovedEmployeeID = eCourse.EmployeeID;
                            tefApproval.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TEF" + DateTime.Now.Ticks.ToString(), "MD5");
                            TEF tef = new TEF();
                            tef.FormID = eCourse.EmployeeCourseID;
                            tef.Status = "P";
                            if (eCourse.CourseSessionObject.CourseObject.RequirePTJNPTEF)
                            {
                                FormApproval faPTEF = new FormApproval();
                                faPTEF.FormType = "PTEF";
                                faPTEF.FormID = eCourse.EmployeeCourseID;
                                faPTEF.ApprovalLevel = 0;
                                faPTEF.ApprovalCreatedDate = DateTime.Now;
                                faPTEF.ApprovalStatus = "P";
                                faPTEF.ApprovedEmployeeID = eCourse.EmployeeObject.SuperiorID;
                                faPTEF.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("PTEF" + DateTime.Now.Ticks.ToString(), "MD5");
                                faPTEF.Save();
                                faPTEF.Resync();
                            }
                            eCourse.Save();
                            eCourse.Resync();
                            fa.Save();
                            fa.Resync();
                            tefApproval.Save();
                            tefApproval.Resync();
                            tef.Save();
                            tef.Resync();
                            Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have approved the application");
                            return;
                        }
                    }
                }
                else
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The registration record has been approved/rejected or cannot be found. Please try again or contact Administrator");
                    return;
                }
            }
            else
            {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The registration record cannot be found. Please try again or contact Administrator");
                return;
            }
        }
        #endregion

        #region RejectOnBehalf
        protected void RejectOnBehalf(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("HR"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            86);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("HR"));

            GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(GMSUtil.ToInt(hidEmployeeCourseID.Value));
            if (eCourse != null)
            {
                FormApproval fa = null;
                foreach (FormApproval fa1 in eCourse.FormApprovalList)
                {
                    if (fa1.ApprovalStatus == "P" && fa1.FormType == "TR")
                    {
                        fa = fa1;
                    }
                }
                if (fa != null)
                {
                    fa.ApprovalRandomID = "";
                    fa.ApprovalStatus = "R";
                    fa.ApprovedOnBehalfUser = session.UserId;
                    fa.ApprovedOnBehalfDate = DateTime.Now;
                    Employee rejectPerson = Employee.RetrieveByKey(fa.ApprovedEmployeeID.Value);
                    eCourse.Status = "R";
                    //Send to applicant
                    SendRejectEmail(eCourse.EmployeeObject.EmailAddress, eCourse.EmployeeObject.Name, eCourse.EmployeeObject.Name,
                                    rejectPerson.Name, eCourse);
                    //Send to reject person
                    SendRejectEmail(rejectPerson.EmailAddress, rejectPerson.Name, eCourse.EmployeeObject.Name,
                                    rejectPerson.Name, eCourse);
                    //Send to notification party
                    IList<NotificationParty> fsnList = new SystemDataActivity().RetrieveNotificationPartyListByFormTypeStatus("TR", "R");
                    foreach (NotificationParty fsn in fsnList)
                    {
                        SendRejectEmail(fsn.EmployeeObject.EmailAddress, "", eCourse.EmployeeObject.Name,
                                    rejectPerson.Name, eCourse);
                    }
                    fa.Save();
                    fa.Resync();
                    eCourse.Save();
                    eCourse.Resync();
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have rejected the application");
                    return;
                }
                else
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The registration record has been approved/rejected or cannot be found. Please try again or contact Administrator");
                    return;
                }
            }
            else
            {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The registration record cannot be found. Please try again or contact Administrator");
                return;
            }
        }
        #endregion

        #region btnUpdate_Click
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            #region Update a Record.
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            86);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");
            try
            {
                GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(GMSUtil.ToInt(hidEmployeeCourseID.Value));
                if (eCourse != null)
                {
                    #region PTJNPTEF
                    if (eCourse.CourseSessionObject.CourseObject.RequirePTJNPTEF)
                    {
                        if (txtLearningObjectives.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your learning objectives.");
                            return;
                        }
                        else
                        {
                            eCourse.LearningObjectives = txtLearningObjectives.Text.Trim();
                        }
                        if (txtActionPlan.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your action plan.");
                            return;
                        }
                        else
                        {
                            eCourse.ActionPlan = txtActionPlan.Text.Trim();
                        }
                        if (txtActionPlanDueDate.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your action plan due date.");
                            return;
                        }
                        else
                        {
                            eCourse.ActionPlanDueDate = GMSUtil.ToDate(txtActionPlanDueDate.Text.Trim());
                        }
                        if (txtPurposeOfActionPlan.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your purpose of action plan.");
                            return;
                        }
                        else
                        {
                            eCourse.PurposeOfActionPlan = txtPurposeOfActionPlan.Text.Trim();
                        }
                        if (txtTypeOfIndicator.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your type of indicator for KPI.");
                            return;
                        }
                        else
                        {
                            eCourse.TypeOfIndicator = txtTypeOfIndicator.Text.Trim();
                        }
                        if (txtCurrentValue.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your current KPI value.");
                            return;
                        }
                        else
                        {
                            eCourse.CurrentValue = GMSUtil.ToDecimal(txtCurrentValue.Text.Trim());
                        }
                        if (txtExpectedValue.Text.Trim() == "")
                        {
                            base.JScriptAlertMsg("You need to fill in your expected KPI value.");
                            return;
                        }
                        else
                        {
                            eCourse.ExpectedValue = GMSUtil.ToDecimal(txtExpectedValue.Text.Trim());
                        }

                        if (txtActualValue.Text.Trim() != "")
                        {
                            if (rbIsObjectiveAchievedAfterCourse.Checked)
                                eCourse.IsObjectiveAchievedAfterCourse = true;
                            else
                                eCourse.IsObjectiveAchievedAfterCourse = false;
                            if (rbIsActionPlanCompletedAfterCourse.Checked)
                                eCourse.IsActionPlanCompletedAfterCourse = true;
                            else
                                eCourse.IsActionPlanCompletedAfterCourse = false;
                            if (!eCourse.IsActionPlanCompletedAfterCourse.Value)
                            {
                                if (txtActionPlanExtendedDueDate.Text.Trim() != "")
                                {
                                    eCourse.ActionPlanExtendedDueDate = GMSUtil.ToDate(txtActionPlanExtendedDueDate.Text.Trim());
                                    FormApproval faPTEF = new FormApproval();
                                    faPTEF.FormType = "PTEF";
                                    faPTEF.FormID = eCourse.EmployeeCourseID;
                                    faPTEF.ApprovalLevel = 0;
                                    faPTEF.ApprovalCreatedDate = DateTime.Now;
                                    faPTEF.ApprovalStatus = "P";
                                    faPTEF.ApprovedEmployeeID = eCourse.EmployeeObject.SuperiorID;
                                    faPTEF.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("PTEF" + DateTime.Now.Ticks.ToString(), "MD5");
                                    faPTEF.Save();
                                    faPTEF.Resync();
                                }
                                else
                                {
                                    base.JScriptAlertMsg("Please input action extended due date.");
                                    return;
                                }
                            }
                            if (txtActualValue.Text.Trim() != "")
                            {
                                eCourse.ActualValueAfterCourse = GMSUtil.ToDecimal(txtActualValue.Text.Trim());
                            }
                            else
                            {
                                base.JScriptAlertMsg("Please input Actual Percentage.");
                                return;
                            }
                            if (txtObjectiveNotAchievedRemark.Text.Trim() != "")
                            {
                                eCourse.ObjectiveNotAchievedRemark = txtObjectiveNotAchievedRemark.Text.Trim();
                            }
                            if (txtActualValue.Text.Trim() != "")
                            {
                                eCourse.ActionPlanNotCompletedRemark = txtActionPlanNotCompletedRemark.Text.Trim();
                            }
                            foreach (FormApproval fa in eCourse.FormApprovalList)
                            {
                                if (fa.FormType == "PTEF" && fa.ApprovalStatus == "P")
                                {
                                    fa.ApprovalStatus = "A";
                                    fa.ApprovalRandomID = "";
                                    fa.ApprovedOnBehalfDate = DateTime.Now;
                                    fa.ApprovedOnBehalfUser = session.UserId;
                                    fa.Save();
                                }
                            }
                        }

                    }
                    #endregion

                    #region Admin Input
                    //if (txtROI.Text.Trim() != "")
                    //    eCourse.ROIAfterCourse = GMSUtil.ToDecimal(txtROI.Text.Trim());
                    if (chkIsBonded.Checked)
                        eCourse.IsBonded = true;
                    else
                        eCourse.IsBonded = false;
                    if (txtNoOfMonthsBonded.Text.Trim() != "")
                        eCourse.NoOfMonthsBonded = GMSUtil.ToByte(txtNoOfMonthsBonded.Text.Trim());
                    if (txtBondContractLocation.Text.Trim() != "")
                        eCourse.BondContractLocation = txtBondContractLocation.Text.Trim();
                    if (txtCertificateLocation.Text.Trim() != "")
                        eCourse.CertificateLocation = txtCertificateLocation.Text.Trim();
                    if (txtBondExpiredDate.Text.Trim() != "")
                        eCourse.BondExpiredDate = GMSUtil.ToDate(txtBondExpiredDate.Text.Trim());
                    if (txtLicenceExpiredDate.Text.Trim() != "")
                        eCourse.LicenceExpiredDate = GMSUtil.ToDate(txtLicenceExpiredDate.Text.Trim());
                    if (txtPaymentLocation.Text.Trim() != "")
                        eCourse.PaymentLocation = txtPaymentLocation.Text.Trim();
                    if (!string.IsNullOrEmpty(txtSDF.Text.Trim()))
                        eCourse.SDF = GMSUtil.ToDecimal(txtSDF.Text.Trim());
                    else
                        eCourse.SDF = 0;
                    if (!string.IsNullOrEmpty(txtSRP.Text.Trim()))
                        eCourse.SRP = GMSUtil.ToDecimal(txtSRP.Text.Trim());
                    else
                        eCourse.SRP = 0;
                    if (txtSDFApplicationDate.Text.Trim() != "")
                        eCourse.SDFApplicationDate = GMSUtil.ToDate(txtSDFApplicationDate.Text.Trim());
                    if (txtSDFApplicationNo.Text.Trim() != "")
                        eCourse.SDFApplicationNo = txtSDFApplicationNo.Text.Trim();
                    if (txtSDFDisbursementEmailLocation.Text.Trim() != "")
                        eCourse.SDFDisbursementEmailLocation = txtSDFDisbursementEmailLocation.Text.Trim();
                    if (txtRemarks.Text.Trim() != "")
                        eCourse.Remarks = txtRemarks.Text.Trim();
                    #endregion

                    eCourse.Save();
                    eCourse.Resync();
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have successfully updated the record.");
                    return;
                }
            }
            catch (Exception ex)
            {
                base.JScriptAlertMsg(ex.Message);
                return;
            }
            #endregion
        }
        #endregion

        #region UpdatePTEF
        protected void UpdatePTEF(object sender, EventArgs e)
        {
            FormApproval fa = new FormActivity().RetrieveFormApporvalByRandomID(hidRandomID.Value);
            if (fa != null && fa.ApprovalStatus == "P")
            {
                fa.ApprovalRandomID = "";
                fa.ApprovalStatus = "A";
                GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(fa.FormID);
                if (rbIsObjectiveAchievedAfterCourse.Checked)
                    eCourse.IsObjectiveAchievedAfterCourse = true;
                else
                    eCourse.IsObjectiveAchievedAfterCourse = false;
                if (rbIsActionPlanCompletedAfterCourse.Checked)
                    eCourse.IsActionPlanCompletedAfterCourse = true;
                else
                    eCourse.IsActionPlanCompletedAfterCourse = false;
                if (!eCourse.IsActionPlanCompletedAfterCourse.Value)
                {
                    if (txtActionPlanExtendedDueDate.Text.Trim() != "")
                    {
                        eCourse.ActionPlanExtendedDueDate = GMSUtil.ToDate(txtActionPlanExtendedDueDate.Text.Trim());
                        FormApproval faPTEF = new FormApproval();
                        faPTEF.FormType = "PTEF";
                        faPTEF.FormID = eCourse.EmployeeCourseID;
                        faPTEF.ApprovalLevel = 0;
                        faPTEF.ApprovalCreatedDate = DateTime.Now;
                        faPTEF.ApprovalStatus = "P";
                        faPTEF.ApprovedEmployeeID = eCourse.EmployeeObject.SuperiorID;
                        faPTEF.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("PTEF" + DateTime.Now.Ticks.ToString(), "MD5");
                        faPTEF.Save();
                        faPTEF.Resync();
                    }
                    else
                    {
                        base.JScriptAlertMsg("Please input action extended due date.");
                        return;
                    }
                }
                if (txtActualValue.Text.Trim() != "")
                {
                    eCourse.ActualValueAfterCourse = GMSUtil.ToDecimal(txtActualValue.Text.Trim());
                }
                else
                {
                    base.JScriptAlertMsg("Please input Actual Percentage.");
                    return;
                }
                if (txtObjectiveNotAchievedRemark.Text.Trim() != "")
                {
                    eCourse.ObjectiveNotAchievedRemark = txtObjectiveNotAchievedRemark.Text.Trim();
                }
                if (txtActualValue.Text.Trim() != "")
                {
                    eCourse.ActionPlanNotCompletedRemark = txtActionPlanNotCompletedRemark.Text.Trim();
                }

                fa.ApprovalModifiedDate = DateTime.Now; 
                fa.Save();
                fa.Resync();
                eCourse.Save();
                eCourse.Resync();
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have successfully updated the record.");
                return;
            }
            else
            {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The registration record cannot be found. Please try again or contact Administrator");
                return;
            }
        }
        #endregion

        #region SendTEFEmail
        private void SendTEFEmail(int eCourseID)
        {
            GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(eCourseID);
            GMSCore.Entity.FormApproval fa = FormApproval.RetrieveByKey("TEF", eCourseID, 0);
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(eCourse.EmployeeObject.EmailAddress, eCourse.EmployeeObject.Name);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);

            mail.ReplyTo = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Admin");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[Training - Reminder To Fill Up Training Evaluation Form - " + eCourse.CourseSessionObject.CourseObject.CourseTitle + "]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dear " + eCourse.EmployeeObject.Name + ",</p>" +
                        "<p>Please click the below link to fill up your training evaluation form. </p>" +
                        "<p><a href=\"https://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS4/SysHR/Training/AddEditTEF.aspx?RANDOMID=" + fa.ApprovalRandomID + "\">Click here.</a></p>" +

                        "***** This is a computer-generated email. Please do not reply.*****";
            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.Host = smtpServer;
                mailClient.Port = 25;
                mailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential authentication = new System.Net.NetworkCredential("gmsadmin@leedenlimited.com", "admin2008");
                mailClient.Credentials = authentication;
                mailClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SendPTEFEmail
        private void SendPTEFEmail(int eCourseID)
        {
            GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(eCourseID);
            GMSCore.Entity.FormApproval fa = FormApproval.RetrieveByKey("PTEF", eCourseID, 0);
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(eCourse.EmployeeObject.SuperiorObject.EmailAddress, eCourse.EmployeeObject.SuperiorObject.Name);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);

            mail.ReplyTo = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Admin");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[Training - Reminder To Fill Up Post Training Evaluation Form]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dear " + eCourse.EmployeeObject.SuperiorObject.Name + ",</p>" +
                        "<p>Please click the below link to fill up the post training evaluation form. </p>" +
                        "<p><a href=\"https://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS4/SysHR/Training/CourseRegistration.aspx?RANDOMID=" + fa.ApprovalRandomID + "&FORMTYPE=PTEF" + "\">Click here.</a></p>" +

                        "***** This is a computer-generated email. Please do not reply.*****";
            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.Host = smtpServer;
                mailClient.Port = 25;
                mailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential authentication = new System.Net.NetworkCredential("gmsadmin@leedenlimited.com", "admin2008");
                mailClient.Credentials = authentication;
                mailClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ReSend
        protected void ReSend(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            switch (lb.ID)
            {
                case "btnResendPRETR":
                    this.SendConfirmationEmail(GMSUtil.ToInt(hidEmployeeCourseID.Value));
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The confirmation Email has been re-sent to related party.");
                    break;
                case "btnResendTR":
                    FormApproval fa = new FormActivity().RetrievePendingFormApprovalByFormTypeFormID("TR", GMSUtil.ToInt(hidEmployeeCourseID.Value));
                    GMSCore.Entity.EmployeeCourse ec = GMSCore.Entity.EmployeeCourse.RetrieveByKey(GMSUtil.ToInt(hidEmployeeCourseID.Value));
                    Employee em = Employee.RetrieveByKey(fa.ApprovedEmployeeID.Value);
                    this.SendApprovalEmail(ec, em, fa);
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The Approval Email has been re-sent to related party.");
                    break;

                case "btnResendTEF":
                    SendTEFEmail(GMSUtil.ToInt(hidEmployeeCourseID.Value));
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The training evaluation Email has been re-sent to related party.");
                    break;

                case "btnResendPTEF":
                    SendPTEFEmail(GMSUtil.ToInt(hidEmployeeCourseID.Value));
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The post training evaluation Email has been re-sent to related party.");
                    break;

            }
        }
        #endregion

        #region ChangeApprovalStatus
        protected void ChangeApprovalStatus(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("HR"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            86);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("HR"));

            GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(GMSUtil.ToInt(hidEmployeeCourseID.Value));
            if (eCourse.TEFList != null && eCourse.TEFList.Count > 0 && eCourse.TEFList[0].Status != "P")
            {
                base.JScriptAlertMsg("You cannot change the approval status after training evaluation form has been filled in");
                return;
            }
            int max = -1;
            int pos = -1;
            if (eCourse.FormApprovalList != null && eCourse.FormApprovalList.Count > 0)
            {
                for (int i = 0; i < eCourse.FormApprovalList.Count; i++)
                {
                    FormApproval fa = eCourse.FormApprovalList[i];
                    if (fa.FormType == "TR" && fa.ApprovalStatus != "P" && fa.ApprovalLevel > max)
                    {
                        max = fa.ApprovalLevel;
                        pos = i;
                    }
                }
                if (pos < 0)
                {
                    base.JScriptAlertMsg("Approval Record cannot be found.");
                    return;
                }
                FormApproval faMax = eCourse.FormApprovalList[pos];
                foreach (FormApproval fa in eCourse.FormApprovalList)
                {
                    if (fa.FormType == "TR" && fa.ApprovalStatus == "P" && fa.ApprovalLevel > max)
                    {
                        fa.Delete();
                    }
                }
                if (faMax.ApprovalStatus == "A")
                {
                    faMax.ApprovalStatus = "R";
                    faMax.ApprovedOnBehalfUser = session.UserId;
                    faMax.ApprovedOnBehalfDate = DateTime.Now;
                    Employee rejectPerson = Employee.RetrieveByKey(faMax.ApprovedEmployeeID.Value);
                    eCourse.Status = "R";
                    //Send to applicant
                    SendRejectEmail(eCourse.EmployeeObject.EmailAddress, eCourse.EmployeeObject.Name, eCourse.EmployeeObject.Name,
                                    rejectPerson.Name, eCourse);
                    //Send to reject person
                    SendRejectEmail(rejectPerson.EmailAddress, rejectPerson.Name, eCourse.EmployeeObject.Name,
                                    rejectPerson.Name, eCourse);
                    //Send to notification party
                    IList<NotificationParty> fsnList = new SystemDataActivity().RetrieveNotificationPartyListByFormTypeStatus("TR", "R");
                    foreach (NotificationParty fsn in fsnList)
                    {
                        SendRejectEmail(fsn.EmployeeObject.EmailAddress, "", eCourse.EmployeeObject.Name,
                                    rejectPerson.Name, eCourse);
                    }
                    faMax.Save();
                    faMax.Resync();
                    eCourse.Save();
                    eCourse.Resync();
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have rejected the application");
                    return;
                }
                else
                {
                    faMax.ApprovalStatus = "A";
                    faMax.ApprovedOnBehalfUser = session.UserId;
                    faMax.ApprovedOnBehalfDate = DateTime.Now;
                    if (faMax.EmployeeCourseObject.Status == "R")
                    {
                        faMax.EmployeeCourseObject.Status = "A";
                        faMax.EmployeeCourseObject.Save();
                        faMax.EmployeeCourseObject.Resync();
                    }
                    if (!eCourse.EmployeeObject.IsUnitHead)
                    {
                        Employee superior = Employee.RetrieveByKey(faMax.ApprovedEmployeeID.Value);
                        if (!superior.IsUnitHead)
                        {
                            if (superior.SuperiorObject != null && !string.IsNullOrEmpty(eCourse.EmployeeObject.SuperiorObject.EmailAddress))
                            {
                                FormApproval fa2 = new FormApproval();
                                fa2.FormType = "TR";
                                fa2.FormID = eCourse.EmployeeCourseID;
                                fa2.ApprovalLevel = faMax.ApprovalLevel++;
                                fa2.ApprovalStatus = "P";
                                fa2.ApprovalCreatedDate = DateTime.Now;
                                fa2.ApprovedEmployeeID = superior.SuperiorObject.EmployeeID;
                                fa2.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TR" + DateTime.Now.Ticks.ToString(), "MD5");
                                fa2.Save();
                                fa2.Resync();
                                SendApprovalEmail(eCourse, superior.SuperiorObject, fa2);
                                faMax.Save();
                                faMax.Resync();
                                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have approved the application");
                                return;
                            }
                            else
                            {
                                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The supervisor information of Employee " + superior.Name + " is not updated.");
                                return;
                            }
                        }
                        else
                        {
                            eCourse.Status = "A";
                            SendNotificationEmail(eCourse.EmployeeObject.EmailAddress, eCourse.EmployeeObject.Name,
                                                eCourse.EmployeeObject.Name, eCourse);
                            SendNotificationEmail(eCourse.EmployeeObject.SuperiorObject.EmailAddress, eCourse.EmployeeObject.SuperiorObject.Name,
                                            eCourse.EmployeeObject.Name, eCourse);
                            IList<NotificationParty> fsnList = new SystemDataActivity().RetrieveNotificationPartyListByFormTypeStatus("TR", "A");
                            foreach (NotificationParty fsn in fsnList)
                            {
                                SendNotificationEmail(fsn.EmployeeObject.EmailAddress, "", eCourse.EmployeeObject.Name,
                                           eCourse);
                            }
                            FormApproval tefApproval = new FormApproval();
                            tefApproval.FormType = "TEF";
                            tefApproval.FormID = eCourse.EmployeeCourseID;
                            tefApproval.ApprovalLevel = 0;
                            tefApproval.ApprovalCreatedDate = DateTime.Now;
                            tefApproval.ApprovalStatus = "P";
                            tefApproval.ApprovedEmployeeID = eCourse.EmployeeID;
                            tefApproval.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TEF" + DateTime.Now.Ticks.ToString(), "MD5");
                            TEF tef = new TEF();
                            tef.FormID = eCourse.EmployeeCourseID;
                            tef.Status = "P";
                            if (eCourse.CourseSessionObject.CourseObject.RequirePTJNPTEF)
                            {
                                FormApproval faPTEF = new FormApproval();
                                faPTEF.FormType = "PTEF";
                                faPTEF.FormID = eCourse.EmployeeCourseID;
                                faPTEF.ApprovalLevel = 0;
                                faPTEF.ApprovalCreatedDate = DateTime.Now;
                                faPTEF.ApprovalStatus = "P";
                                faPTEF.ApprovedEmployeeID = eCourse.EmployeeObject.SuperiorID;
                                faPTEF.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("PTEF" + DateTime.Now.Ticks.ToString(), "MD5");
                                faPTEF.Save();
                                faPTEF.Resync();
                            }
                            eCourse.Save();
                            eCourse.Resync();
                            faMax.Save();
                            faMax.Resync();
                            tefApproval.Save();
                            tefApproval.Resync();
                            tef.Save();
                            tef.Resync();
                            Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have approved the application");
                            return;
                        }
                    }
                    else
                    {
                        Employee superior = Employee.RetrieveByKey(faMax.ApprovedEmployeeID.Value);
                        if (superior.EmployeeNo != "0001")
                        {
                            if (superior.SuperiorObject != null && !string.IsNullOrEmpty(eCourse.EmployeeObject.SuperiorObject.EmailAddress))
                            {
                                FormApproval fa2 = new FormApproval();
                                fa2.FormType = "TR";
                                fa2.FormID = eCourse.EmployeeCourseID;
                                fa2.ApprovalLevel = faMax.ApprovalLevel++;
                                fa2.ApprovalStatus = "P";
                                fa2.ApprovalCreatedDate = DateTime.Now;
                                fa2.ApprovedEmployeeID = superior.SuperiorObject.EmployeeID;
                                fa2.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TR" + DateTime.Now.Ticks.ToString(), "MD5");
                                fa2.Save();
                                fa2.Resync();
                                SendApprovalEmail(eCourse, superior.SuperiorObject, fa2);
                                faMax.Save();
                                faMax.Resync();
                                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have approved the application");
                                return;
                            }
                            else
                            {
                                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The supervisor information of Employee " + superior.Name + " is not updated.");
                                return;
                            }
                        }
                        else
                        {
                            eCourse.Status = "A";
                            SendNotificationEmail(eCourse.EmployeeObject.EmailAddress, eCourse.EmployeeObject.Name,
                                                eCourse.EmployeeObject.Name, eCourse);
                            SendNotificationEmail(eCourse.EmployeeObject.SuperiorObject.EmailAddress, eCourse.EmployeeObject.SuperiorObject.Name,
                                            eCourse.EmployeeObject.Name, eCourse);
                            IList<NotificationParty> fsnList = new SystemDataActivity().RetrieveNotificationPartyListByFormTypeStatus("TR", "A");
                            foreach (NotificationParty fsn in fsnList)
                            {
                                SendNotificationEmail(fsn.EmployeeObject.EmailAddress, "", eCourse.EmployeeObject.Name,
                                           eCourse);
                            }
                            FormApproval tefApproval = new FormApproval();
                            tefApproval.FormType = "TEF";
                            tefApproval.FormID = eCourse.EmployeeCourseID;
                            tefApproval.ApprovalLevel = 0;
                            tefApproval.ApprovalCreatedDate = DateTime.Now;
                            tefApproval.ApprovalStatus = "P";
                            tefApproval.ApprovedEmployeeID = eCourse.EmployeeID;
                            tefApproval.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("TEF" + DateTime.Now.Ticks.ToString(), "MD5");
                            TEF tef = new TEF();
                            tef.FormID = eCourse.EmployeeCourseID;
                            tef.Status = "P";
                            if (eCourse.CourseSessionObject.CourseObject.RequirePTJNPTEF)
                            {
                                FormApproval faPTEF = new FormApproval();
                                faPTEF.FormType = "PTEF";
                                faPTEF.FormID = eCourse.EmployeeCourseID;
                                faPTEF.ApprovalLevel = 0;
                                faPTEF.ApprovalCreatedDate = DateTime.Now;
                                faPTEF.ApprovalStatus = "P";
                                faPTEF.ApprovedEmployeeID = eCourse.EmployeeObject.SuperiorID;
                                faPTEF.ApprovalRandomID = FormsAuthentication.HashPasswordForStoringInConfigFile("PTEF" + DateTime.Now.Ticks.ToString(), "MD5");
                                faPTEF.Save();
                                faPTEF.Resync();
                            }
                            eCourse.Save();
                            eCourse.Resync();
                            faMax.Save();
                            faMax.Resync();
                            tefApproval.Save();
                            tefApproval.Resync();
                            tef.Save();
                            tef.Resync();
                            Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "You have approved the application");
                            return;
                        }
                    }
                }

            }
        }
        #endregion
    }
}
