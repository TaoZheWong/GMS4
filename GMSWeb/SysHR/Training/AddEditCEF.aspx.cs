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
using System.Collections.Generic;

namespace GMSWeb.SysHR.Training
{
    public partial class AddEditCEF : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string formOnlineID = Request.Params["FORMONLINEID"];
                string applicantID = Request.Params["APPLICANTID"];
                string approvalID = Request.Params["APPROVALID"];
                string CEFID = Request.Params["CEFID"];
                string ISHR = Request.Params["ISHR"];

                if (!string.IsNullOrEmpty(formOnlineID) && !string.IsNullOrEmpty(applicantID))
                {
                    FormRandomID randomID = (new FormActivity()).RetrieveFormRandomIDByFormOnlineIDApplicantID(formOnlineID, applicantID);
                    if (randomID != null && randomID.FormType == "CEF")
                    {
                        hidFormID.Value = randomID.FormID.ToString();
                        hidApprovalLevel.Value = "0";
                        LoadData();
                    }
                    else
                    {
                        Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The link has already been processed or expired.");
                        return;
                    }
                }
                else if (!string.IsNullOrEmpty(formOnlineID) && !string.IsNullOrEmpty(approvalID))
                {
                    FormRandomID randomID = (new FormActivity()).RetrieveFormRandomIDByFormOnlineIDApprovalID(formOnlineID, approvalID);
                    if (randomID != null && randomID.FormType == "CEF")
                    {
                        hidFormID.Value = randomID.FormID.ToString();
                        hidApprovalLevel.Value = randomID.ApprovalLevel.ToString();
                        LoadData();
                    }
                    else
                    {
                        Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The link has already been processed or expired.");
                        return;
                    }
                }
                else if (!string.IsNullOrEmpty(CEFID))
                {
                    hidFormID.Value = CEFID;
                    hidView.Value = "True";
                    if (ISHR == "TRUE")
                        hidIsHR.Value = "True";
                    LoadData();
                }
                else
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The link has already been processed or expired.");
                    return;
                }
            }

        }

        //#region LoadData
        //private void LoadData()
        //{
        //    if (hidFormID.Value != "")
        //    {
        //        GMSCore.Entity.EmployeeCourse eCourse = (new FormActivity()).RetrieveEmployeeCourseByCEFID(int.Parse(hidFormID.Value));
        //        if (eCourse != null)
        //        {
        //            txtNewEmployeeName.Text = eCourse.EmployeeObject.Name;
        //            txtNewEmployeeNumber.Text = eCourse.EmployeeObject.EmployeeNo;
        //            txtNewDepartment.Text = eCourse.EmployeeObject.Department;
        //            txtNewSuperiorName.Text = (eCourse.EmployeeObject.SuperiorObject != null)?eCourse.EmployeeObject.SuperiorObject.Name:"";
        //            txtNewCourseTitle.Text = eCourse.CourseObject.CourseTitle;
        //            ddlNewCourseType.SelectedValue = eCourse.Type;
        //            txtNewOrganizerName.Text = eCourse.CourseObject.CourseOrganizerObject.OrganizerName;
        //            newDateFrom.Text = eCourse.DateFrom.ToString().Equals("1/01/1900 12:00:00 AM") ? "" : eCourse.DateFrom.ToString("dd/MM/yyyy");
        //            newDateTo.Text = eCourse.DateTo.ToString().Equals("1/01/1900 12:00:00 AM") ? "" : eCourse.DateTo.ToString("dd/MM/yyyy");
        //            txtNewCourseFee.Text = eCourse.CourseFee.ToString();
        //        }

        //        GMSCore.Entity.Form form = (new FormActivity()).RetrieveFormByFormTypeFormID("CEF", int.Parse(hidFormID.Value));
        //        if (form != null && form.FormStatus != "N")
        //        {
        //            CEF cef = CEF.RetrieveByKey(int.Parse(hidFormID.Value));
        //            if (cef != null)
        //            {
        //                switch (cef.ContentRelevant)
        //                {
        //                    case 1:
        //                        rbContentRelevant1.Checked = true;
        //                        break;
        //                    case 2:
        //                        rbContentRelevant2.Checked = true;
        //                        break;
        //                    case 3:
        //                        rbContentRelevant3.Checked = true;
        //                        break;
        //                    case 4:
        //                        rbContentRelevant4.Checked = true;
        //                        break;
        //                    case 5:
        //                        rbContentRelevant5.Checked = true;
        //                        break;
        //                }

        //                switch (cef.ContentWellOrganized)
        //                {
        //                    case 1:
        //                        rbContentWellOrganized1.Checked = true;
        //                        break;
        //                    case 2:
        //                        rbContentWellOrganized2.Checked = true;
        //                        break;
        //                    case 3:
        //                        rbContentWellOrganized3.Checked = true;
        //                        break;
        //                    case 4:
        //                        rbContentWellOrganized4.Checked = true;
        //                        break;
        //                    case 5:
        //                        rbContentWellOrganized5.Checked = true;
        //                        break;
        //                }

        //                switch (cef.CourseClear)
        //                {
        //                    case 1:
        //                        rbCourseClear1.Checked = true;
        //                        break;
        //                    case 2:
        //                        rbCourseClear2.Checked = true;
        //                        break;
        //                    case 3:
        //                        rbCourseClear3.Checked = true;
        //                        break;
        //                    case 4:
        //                        rbCourseClear4.Checked = true;
        //                        break;
        //                    case 5:
        //                        rbCourseClear5.Checked = true;
        //                        break;
        //                }

        //                switch (cef.EncourageParticipation)
        //                {
        //                    case 1:
        //                        rbEncourageParticipation1.Checked = true;
        //                        break;
        //                    case 2:
        //                        rbEncourageParticipation2.Checked = true;
        //                        break;
        //                    case 3:
        //                        rbEncourageParticipation3.Checked = true;
        //                        break;
        //                    case 4:
        //                        rbEncourageParticipation4.Checked = true;
        //                        break;
        //                    case 5:
        //                        rbEncourageParticipation5.Checked = true;
        //                        break;
        //                }

        //                switch (cef.MethodEffective)
        //                {
        //                    case 1:
        //                        rbMethodEffective1.Checked = true;
        //                        break;
        //                    case 2:
        //                        rbMethodEffective2.Checked = true;
        //                        break;
        //                    case 3:
        //                        rbMethodEffective3.Checked = true;
        //                        break;
        //                    case 4:
        //                        rbMethodEffective4.Checked = true;
        //                        break;
        //                    case 5:
        //                        rbMethodEffective5.Checked = true;
        //                        break;
        //                }

        //                switch (cef.CourseMeetObjects)
        //                {
        //                    case 1:
        //                        rbCourseMeetObjects1.Checked = true;
        //                        break;
        //                    case 2:
        //                        rbCourseMeetObjects2.Checked = true;
        //                        break;
        //                    case 3:
        //                        rbCourseMeetObjects3.Checked = true;
        //                        break;
        //                    case 4:
        //                        rbCourseMeetObjects4.Checked = true;
        //                        break;
        //                    case 5:
        //                        rbCourseMeetObjects5.Checked = true;
        //                        break;
        //                }

        //                switch (cef.CourseMeetExpectation)
        //                {
        //                    case 1:
        //                        rbCourseMeetExpectation1.Checked = true;
        //                        break;
        //                    case 2:
        //                        rbCourseMeetExpectation2.Checked = true;
        //                        break;
        //                    case 3:
        //                        rbCourseMeetExpectation3.Checked = true;
        //                        break;
        //                    case 4:
        //                        rbCourseMeetExpectation4.Checked = true;
        //                        break;
        //                    case 5:
        //                        rbCourseMeetExpectation5.Checked = true;
        //                        break;
        //                }

        //                switch (cef.SatisfiedWithCourse)
        //                {
        //                    case 1:
        //                        rbSatisfiedWithCourse1.Checked = true;
        //                        break;
        //                    case 2:
        //                        rbSatisfiedWithCourse2.Checked = true;
        //                        break;
        //                    case 3:
        //                        rbSatisfiedWithCourse3.Checked = true;
        //                        break;
        //                    case 4:
        //                        rbSatisfiedWithCourse4.Checked = true;
        //                        break;
        //                    case 5:
        //                        rbSatisfiedWithCourse5.Checked = true;
        //                        break;
        //                }

        //                txtBestArea.Text = cef.BestArea;
        //                txtAreaNeedImprovement.Text = cef.AreaNeedImprovement;
        //                txtOtherComments.Text = cef.OtherComments;

        //                rbContentRelevant1.Enabled = false;
        //                rbContentRelevant2.Enabled = false;
        //                rbContentRelevant3.Enabled = false;
        //                rbContentRelevant4.Enabled = false;
        //                rbContentRelevant5.Enabled = false;
        //                rbContentWellOrganized1.Enabled = false;
        //                rbContentWellOrganized2.Enabled = false;
        //                rbContentWellOrganized3.Enabled = false;
        //                rbContentWellOrganized4.Enabled = false;
        //                rbContentWellOrganized5.Enabled = false;
        //                rbCourseClear1.Enabled = false;
        //                rbCourseClear2.Enabled = false;
        //                rbCourseClear3.Enabled = false;
        //                rbCourseClear4.Enabled = false;
        //                rbCourseClear5.Enabled = false;
        //                rbEncourageParticipation1.Enabled = false;
        //                rbEncourageParticipation2.Enabled = false;
        //                rbEncourageParticipation3.Enabled = false;
        //                rbEncourageParticipation4.Enabled = false;
        //                rbEncourageParticipation5.Enabled = false;
        //                rbMethodEffective1.Enabled = false;
        //                rbMethodEffective2.Enabled = false;
        //                rbMethodEffective3.Enabled = false;
        //                rbMethodEffective4.Enabled = false;
        //                rbMethodEffective5.Enabled = false;
        //                rbCourseMeetObjects1.Enabled = false;
        //                rbCourseMeetObjects2.Enabled = false;
        //                rbCourseMeetObjects3.Enabled = false;
        //                rbCourseMeetObjects4.Enabled = false;
        //                rbCourseMeetObjects5.Enabled = false;
        //                rbCourseMeetExpectation1.Enabled = false;
        //                rbCourseMeetExpectation2.Enabled = false;
        //                rbCourseMeetExpectation3.Enabled = false;
        //                rbCourseMeetExpectation4.Enabled = false;
        //                rbCourseMeetExpectation5.Enabled = false;
        //                rbSatisfiedWithCourse1.Enabled = false;
        //                rbSatisfiedWithCourse2.Enabled = false;
        //                rbSatisfiedWithCourse3.Enabled = false;
        //                rbSatisfiedWithCourse4.Enabled = false;
        //                rbSatisfiedWithCourse5.Enabled = false;
        //                txtBestArea.ReadOnly = true;
        //                txtAreaNeedImprovement.ReadOnly = true;
        //                txtOtherComments.ReadOnly = true;
        //            }
        //            btnSubmit.Visible = false;
        //            btnAccept.Visible = true;
        //            btnReject.Visible = true;
        //        }

        //        if (hidView.Value == "True")
        //        {
        //            SubmitPanel.Visible = false;
        //            ApprovalStatusPanel.Visible = true;
        //            if (hidIsHR.Value == "True")
        //                trPrintReport.Visible = true;
        //            else
        //                trPrintReport.Visible = false;

        //            if (form != null && form.FormStatus == "S")
        //            {
        //                ApplicationStatus.Text = "Form Status = Submitted on " + form.SubmittedDate.ToString();
        //            }
        //            else if (form != null && form.FormStatus == "N")
        //            {
        //                ApplicationStatus.Text = "Form Status = New on " + form.CreatedDate.ToString();
        //            }
        //            else if (form != null && form.FormStatus == "P")
        //            {
        //                ApplicationStatus.Text = "Form Status = Pending";
        //            }
        //            else if (form != null && form.FormStatus == "R")
        //            {
        //                ApplicationStatus.Text = "Form Status = Rejected";
        //            }
        //            else if (form != null && form.FormStatus == "A")
        //            {
        //                ApplicationStatus.Text = "Form Status = Approved";
        //            }

        //            IList<FormApproval> faList = (new FormActivity()).RetrieveFormApprovalListByFormTypeFormID("CEF", int.Parse(hidFormID.Value));
        //            if (faList != null && faList.Count > 0)
        //            {
        //                foreach (FormApproval fa in faList)
        //                {
        //                    string status = "Pending";
        //                    if (fa.ApprovalStatus == "A")
        //                        status = "Accepted on " + fa.ApprovalCreatedDate.ToString() + " by " + fa.ApprovedEmployeeObject.Name; 
        //                    else if (fa.ApprovalStatus == "R")
        //                        status = "Rejected on " + fa.ApprovalCreatedDate.ToString() + " by " + fa.ApprovedEmployeeObject.Name;
        //                    switch (fa.ApprovalLevel)
        //                    {
        //                        case 1:
        //                            Level1Status.Text = "Level 1 Approval Status = " + status;
        //                            break;
        //                    }
        //                }
        //            }
        //            if (form.FormStatus == "A" && form.NotifiedEmployeeID > 0)
        //            {
        //                NotificationStatus.Text = form.NotifiedEmployeeObject.Name + " has been notified.";
        //            }
        //        }
        //        else
        //        {
        //            SubmitPanel.Visible = true;
        //            ApprovalStatusPanel.Visible = false;
        //        }
        //    }
        //}
        //#endregion

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                FormActivity fActivity = new FormActivity();
                GMSCore.Entity.CEF cef = CEF.RetrieveByKey(int.Parse(hidFormID.Value));
                if (cef == null)
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The page has expired.");
                    return;
                }

                GMSCore.Entity.Form form = fActivity.RetrieveFormByFormTypeFormID("CEF", cef.FormID);
                if (form == null || form.FormStatus != "N")
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The form has already been submitted before.");
                    return;
                }

                byte contentRelevant = 0;
                if (rbContentRelevant1.Checked)
                    contentRelevant = 1;
                else if (rbContentRelevant2.Checked)
                    contentRelevant = 2;
                else if (rbContentRelevant3.Checked)
                    contentRelevant = 3;
                else if (rbContentRelevant4.Checked)
                    contentRelevant = 4;
                else if (rbContentRelevant5.Checked)
                    contentRelevant = 5;
                else
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "Please choose your selection for all questions." + "\");</script>", false);
                    return;
                }
                cef.ContentRelevant = contentRelevant;

                byte contentWellOrganized = 0;
                if (rbContentWellOrganized1.Checked)
                    contentWellOrganized = 1;
                else if (rbContentWellOrganized2.Checked)
                    contentWellOrganized = 2;
                else if (rbContentWellOrganized3.Checked)
                    contentWellOrganized = 3;
                else if (rbContentWellOrganized4.Checked)
                    contentWellOrganized = 4;
                else if (rbContentWellOrganized5.Checked)
                    contentWellOrganized = 5;
                else
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "Please choose your selection for all questions." + "\");</script>", false);
                    return;
                }
                cef.ContentWellOrganized = contentWellOrganized;

                byte courseClear = 0;
                if (rbCourseClear1.Checked)
                    courseClear = 1;
                else if (rbCourseClear2.Checked)
                    courseClear = 2;
                else if (rbCourseClear3.Checked)
                    courseClear = 3;
                else if (rbCourseClear4.Checked)
                    courseClear = 4;
                else if (rbCourseClear5.Checked)
                    courseClear = 5;
                else
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "Please choose your selection for all questions." + "\");</script>", false);
                    return;
                }
                cef.CourseClear = courseClear;

                byte encourageParticipation = 0;
                if (rbEncourageParticipation1.Checked)
                    encourageParticipation = 1;
                else if (rbEncourageParticipation2.Checked)
                    encourageParticipation = 2;
                else if (rbEncourageParticipation3.Checked)
                    encourageParticipation = 3;
                else if (rbEncourageParticipation4.Checked)
                    encourageParticipation = 4;
                else if (rbEncourageParticipation5.Checked)
                    encourageParticipation = 5;
                else
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "Please choose your selection for all questions." + "\");</script>", false);
                    return;
                }
                cef.EncourageParticipation = encourageParticipation;

                byte methodEffective = 0;
                if (rbMethodEffective1.Checked)
                    methodEffective = 1;
                else if (rbMethodEffective2.Checked)
                    methodEffective = 2;
                else if (rbMethodEffective3.Checked)
                    methodEffective = 3;
                else if (rbMethodEffective4.Checked)
                    methodEffective = 4;
                else if (rbMethodEffective5.Checked)
                    methodEffective = 5;
                else
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "Please choose your selection for all questions." + "\");</script>", false);
                    return;
                }
                cef.MethodEffective = methodEffective;

                byte courseMeetObjects = 0;
                if (rbCourseMeetObjects1.Checked)
                    courseMeetObjects = 1;
                else if (rbCourseMeetObjects2.Checked)
                    courseMeetObjects = 2;
                else if (rbCourseMeetObjects3.Checked)
                    courseMeetObjects = 3;
                else if (rbCourseMeetObjects4.Checked)
                    courseMeetObjects = 4;
                else if (rbCourseMeetObjects5.Checked)
                    courseMeetObjects = 5;
                else
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "Please choose your selection for all questions." + "\");</script>", false);
                    return;
                }
                cef.CourseMeetObjects = courseMeetObjects;

                byte courseMeetExpectation = 0;
                if (rbCourseMeetExpectation1.Checked)
                    courseMeetExpectation = 1;
                else if (rbCourseMeetExpectation2.Checked)
                    courseMeetExpectation = 2;
                else if (rbCourseMeetExpectation3.Checked)
                    courseMeetExpectation = 3;
                else if (rbCourseMeetExpectation4.Checked)
                    courseMeetExpectation = 4;
                else if (rbCourseMeetExpectation5.Checked)
                    courseMeetExpectation = 5;
                else
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "Please choose your selection for all questions." + "\");</script>", false);
                    return;
                }
                cef.CourseMeetExpectation = courseMeetExpectation;

                byte satisfiedWithCourse = 0;
                if (rbSatisfiedWithCourse1.Checked)
                    satisfiedWithCourse = 1;
                else if (rbSatisfiedWithCourse2.Checked)
                    satisfiedWithCourse = 2;
                else if (rbSatisfiedWithCourse3.Checked)
                    satisfiedWithCourse = 3;
                else if (rbSatisfiedWithCourse4.Checked)
                    satisfiedWithCourse = 4;
                else if (rbSatisfiedWithCourse5.Checked)
                    satisfiedWithCourse = 5;
                else
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "Please choose your selection for all questions." + "\");</script>", false);
                    return;
                }

                if (txtBestArea.Text.Trim() == "")
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "Please key in the best areaa you like." + "\");</script>", false);
                    return;
                }

                if (txtAreaNeedImprovement.Text.Trim() == "")
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "Please key in the areas that need improvement." + "\");</script>", false);
                    return;
                }

                cef.SatisfiedWithCourse = satisfiedWithCourse;

                cef.BestArea = txtBestArea.Text.Trim();
                cef.AreaNeedImprovement = txtAreaNeedImprovement.Text.Trim();
                cef.OtherComments = txtOtherComments.Text.Trim();

                form.FormStatus = "S";
                form.SubmittedDate = DateTime.Now;
                form.Save();

                cef.Save();

                FormRandomID randomID = fActivity.RetrieveFormRandomIDByFormTypeFormIDApprovalLevel("CEF", cef.FormID, 0);
                randomID.Delete();
                randomID.Resync();

                FormConfig config = FormConfig.RetrieveByKey("CEF");

                if (config.ApprovalLevel > 0)
                {
                    PrepareEmail(1, config, cef.FormID);
                }

                if (config.ApprovalProcess == "C")
                {
                    if (config.ApprovalLevel >= 2)
                    {
                        PrepareEmail(2, config, cef.FormID);
                    }

                    if (config.ApprovalLevel >= 3)
                    {
                        PrepareEmail(3, config, cef.FormID);
                    }

                    if (config.ApprovalLevel >= 4)
                    {
                        PrepareEmail(4, config, cef.FormID);
                    }

                    if (config.ApprovalLevel >= 5)
                    {
                        PrepareEmail(5, config, cef.FormID);
                    }
                }

                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The form has been submitted.");
                return;
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + ex.Message + "\");</script>", false);
                return;
            }
        }
        #endregion

        #region PrepareEmail
        private void PrepareEmail(byte level, FormConfig config, int formID)
        {
            FormActivity fActivity = new FormActivity();
            Employee sendObject = null;
            string code = "";
            switch (level)
            {
                case 1:
                    code = config.Approval1;
                    break;
                case 2:
                    code = config.Approval2;
                    break;
                case 3:
                    code = config.Approval3;
                    break;
                case 4:
                    code = config.Approval4;
                    break;
                case 5:
                    code = config.Approval5;
                    break;
            }

            if (code == "SP")
            {
                sendObject = fActivity.RetrieveEmployeeCourseByCEFID(int.Parse(hidFormID.Value)).EmployeeObject.SuperiorObject;
            }
            else if (code == "UH")
            {
                Employee unitHead = (new EmployeeActivity()).RetrieveUnitHeadByDepartment(fActivity.RetrieveEmployeeCourseByCEFID(int.Parse(hidFormID.Value)).EmployeeObject.Department);
                Employee superior = fActivity.RetrieveEmployeeCourseByCEFID(int.Parse(hidFormID.Value)).EmployeeObject.SuperiorObject;
                if (unitHead.EmployeeID == superior.EmployeeID)
                {
                    return;
                }
                sendObject = unitHead;
            }
            else
            {
                switch (level)
                {
                    case 1:
                        sendObject = config.Approval1FormPartyObject.EmployeeObject;
                        break;
                    case 2:
                        sendObject = config.Approval2FormPartyObject.EmployeeObject;
                        break;
                    case 3:
                        sendObject = config.Approval3FormPartyObject.EmployeeObject;
                        break;
                    case 4:
                        sendObject = config.Approval4FormPartyObject.EmployeeObject;
                        break;
                    case 5:
                        sendObject = config.Approval5FormPartyObject.EmployeeObject;
                        break;
                }
            }

            FormApproval fa = new FormApproval();
            fa.FormType = "CEF";
            fa.FormID = formID;
            fa.ApprovalLevel = level;
            fa.ApprovalStatus = "P";
            fa.ApprovalCreatedDate = DateTime.Now;
            fa.ApprovedEmployeeID = sendObject.EmployeeID;

            FormRandomID randomID = new FormRandomID();
            randomID.FormType = "CEF";
            randomID.FormID = formID;
            randomID.ApprovalLevel = level;
            randomID.FormOnlineID = FormsAuthentication.HashPasswordForStoringInConfigFile("FormOnlineID" + DateTime.Now.Ticks.ToString(), "MD5");
            randomID.ApprovalID = FormsAuthentication.HashPasswordForStoringInConfigFile("ApprovalID" + DateTime.Now.Ticks.ToString(), "MD5");

            if (sendObject != null && sendObject.EmailAddress != "")
            {
                GMSCore.Entity.EmployeeCourse eCourse = (new FormActivity()).RetrieveEmployeeCourseByCEFID(int.Parse(hidFormID.Value));
                SendEmail(sendObject.EmailAddress, sendObject.Name, "CEF", randomID.FormOnlineID, randomID.ApprovalID, eCourse);

                fa.Save();
                randomID.Save();
            }
            else
            {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The Email address is not properly set up. Please contact your System Administrator.");
                return;
            }
        }
        #endregion

        #region SendEmail
        private void SendEmail(string userEmail, string userRealName, string type, string id1, string id2, GMSCore.Entity.EmployeeCourse eCourse)
        {
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);

            mail.ReplyTo = new System.Net.Mail.MailAddress("ray.tong@leedenlimited.com", "Tong Rui, Ray");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            switch (type)
            {
                case "CEF":
                    mail.Subject = "[GMS - Course Evaluation Form Endorsement]";
                    mail.IsBodyHtml = true;
                    mail.Body = "<p>Dear " + userRealName + ",</p>\n" +
                                "<p>Please go to Group Management System (GMS) to endorse the course evaluation form" + ".\n<br />" +
                                "<a href=\"http://sgcitrix.leedenlimited.com/GMS/SysHR/Form.aspx?FORMONLINEID=" + id1 + "&ApprovalID=" + id2 + "\">Click here.</a></p>\n" +
                                "<p><table><tr><td>Course Provider</td><td>:</td><td>" + eCourse.CourseObject.CourseOrganizerObject.OrganizerName + "</td></tr>" +
                                "<tr><td>Course Title</td><td>:</td><td>" + eCourse.CourseObject.CourseTitle + "</td></tr>" +
                                "<tr><td>Course Date</td><td>:</td><td>" + eCourse.DateFrom.ToString("dd/MM/yyyy") + " to " + eCourse.DateTo.ToString("dd/MM/yyyy") +"</td></tr>" +
                                "<tr><td>Employee Enrolled</td><td>:</td><td>" + eCourse.EmployeeObject.Name + "</td></tr></table></p>" + 
                                "***** This is a computer-generated email. Please do not reply.*****";
                    break;
            }

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

        #region btnReject_Click
        protected void btnReject_Click(object sender, EventArgs e)
        {

            try
            {
                FormActivity fActivity = new FormActivity();
                FormRandomID randomID = fActivity.RetrieveFormRandomIDByFormTypeFormIDApprovalLevel("CEF", int.Parse(hidFormID.Value), byte.Parse(hidApprovalLevel.Value));
                if (randomID != null)
                {
                    randomID.Delete();
                    randomID.Resync();
                }
                else
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The link has already been processed or expired.");
                    return;
                }

                FormApproval fa = fActivity.RetrieveFormApprovalByFormTypeFormIDApprovalLevel("CEF", int.Parse(hidFormID.Value), byte.Parse(hidApprovalLevel.Value));
                fa.ApprovalStatus = "R";
                fa.Save();

                GMSCore.Entity.Form form = fActivity.RetrieveFormByFormTypeFormID("CEF", int.Parse(hidFormID.Value));
                form.FormStatus = "R";
                form.Save();

                GMSCore.Entity.EmployeeCourse eCourse = (new FormActivity()).RetrieveEmployeeCourseByCEFID(int.Parse(hidFormID.Value));
                Employee rejectParty = null;
                FormConfig config = FormConfig.RetrieveByKey("CEF");
                string code = "";
                switch (byte.Parse(hidApprovalLevel.Value))
                {
                    case 1:
                        code = config.Approval1;
                        break;
                    case 2:
                        code = config.Approval2;
                        break;
                    case 3:
                        code = config.Approval3;
                        break;
                    case 4:
                        code = config.Approval4;
                        break;
                    case 5:
                        code = config.Approval5;
                        break;
                }
                if (code == "SP")
                {
                    rejectParty = eCourse.EmployeeObject.SuperiorObject;
                }
                else if (code == "UH")
                {
                    rejectParty = (new EmployeeActivity()).RetrieveUnitHeadByDepartment(eCourse.EmployeeObject.Department);
                }
                else
                {
                    switch (byte.Parse(hidApprovalLevel.Value))
                    {
                        case 1:
                            rejectParty = config.Approval1FormPartyObject.EmployeeObject;
                            break;
                        case 2:
                            rejectParty = config.Approval2FormPartyObject.EmployeeObject;
                            break;
                        case 3:
                            rejectParty = config.Approval3FormPartyObject.EmployeeObject;
                            break;
                        case 4:
                            rejectParty = config.Approval4FormPartyObject.EmployeeObject;
                            break;
                        case 5:
                            rejectParty = config.Approval5FormPartyObject.EmployeeObject;
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(form.SubmittedEmployeeObject.EmailAddress) && !string.IsNullOrEmpty(config.ApprovedRejectedPartyFormPartyObject.EmployeeObject.EmailAddress))
                {
                    this.SendRejectEmail(form.SubmittedEmployeeObject.EmailAddress, form.SubmittedEmployeeObject.Name, form.SubmittedEmployeeObject.Name, rejectParty.Name, eCourse);
                    this.SendRejectEmail(config.ApprovedRejectedPartyFormPartyObject.EmployeeObject.EmailAddress, config.ApprovedRejectedPartyFormPartyObject.EmployeeObject.Name, form.SubmittedEmployeeObject.Name, rejectParty.Name, eCourse);
                }
                else
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The Email address is not properly set up. Please contact your System Administrator.");
                    return;
                }

                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The form has been processed.");
                return;
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + ex.Message + "\");</script>", false);
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
            mail.Subject = "[GMS - Course Evaluation Form Rejected]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dear " + userRealName + ",</p>\n" +
                        "<p>The course evaluation form submitted by " + applicantName + " has been rejected by " + rejectedName + ".\n</p>" +
                        "<p><table><tr><td>Course Provider</td><td>:</td><td>" + eCourse.CourseObject.CourseOrganizerObject.OrganizerName + "</td></tr>" +
                        "<tr><td>Course Title</td><td>:</td><td>" + eCourse.CourseObject.CourseTitle + "</td></tr>" +
                        "<tr><td>Course Date</td><td>:</td><td>" + eCourse.DateFrom.ToString("dd/MM/yyyy") + " to " + eCourse.DateTo.ToString("dd/MM/yyyy") + "</td></tr>" +
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

        #region btnAccept_Click
        protected void btnAccept_Click(object sender, EventArgs e)
        {

            try
            {
                FormActivity fActivity = new FormActivity();
                int formID = int.Parse(hidFormID.Value);
                byte approvalLevel = byte.Parse(hidApprovalLevel.Value);
                FormRandomID randomID = fActivity.RetrieveFormRandomIDByFormTypeFormIDApprovalLevel("CEF", formID, approvalLevel);
                if (randomID != null)
                {
                    randomID.Delete();
                    randomID.Resync();
                }
                else
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The link has already been processed or expired.");
                    return;
                }

                FormApproval fa = fActivity.RetrieveFormApprovalByFormTypeFormIDApprovalLevel("CEF", int.Parse(hidFormID.Value), byte.Parse(hidApprovalLevel.Value));
                fa.ApprovalStatus = "A";
                fa.Save();

                approvalLevel++;

                FormConfig config = FormConfig.RetrieveByKey("CEF");
                if (config.ApprovalProcess == "S" && config.ApprovalLevel >= approvalLevel)
                {
                    string code = "";
                    switch (approvalLevel)
                    {
                        case 1:
                            code = config.Approval1;
                            break;
                        case 2:
                            code = config.Approval2;
                            break;
                        case 3:
                            code = config.Approval3;
                            break;
                        case 4:
                            code = config.Approval4;
                            break;
                        case 5:
                            code = config.Approval5;
                            break;
                    }

                    if (code == "UH")
                    {
                        Employee unitHead = (new EmployeeActivity()).RetrieveUnitHeadByDepartment(fActivity.RetrieveEmployeeCourseByCEFID(int.Parse(hidFormID.Value)).EmployeeObject.Department);
                        Employee superior = fActivity.RetrieveEmployeeCourseByCEFID(int.Parse(hidFormID.Value)).EmployeeObject.SuperiorObject;
                        if (unitHead.EmployeeID == superior.EmployeeID)
                        {
                            approvalLevel++;
                        }
                    }

                    PrepareEmail(approvalLevel, config, formID);
                }

                GMSCore.Entity.Form form = fActivity.RetrieveFormByFormTypeFormID("CEF", int.Parse(hidFormID.Value));
                if (form.FormStatus != "R")
                {
                    bool approved = true;
                    IList<FormApproval> faList = fActivity.RetrieveFormApprovalListByFormTypeFormID("CEF", int.Parse(hidFormID.Value));
                    if (faList != null && faList.Count > 0)
                    {
                        foreach (FormApproval faTemp in faList)
                        {
                            if (faTemp.ApprovalStatus != "A")
                            {
                                approved = false;
                            }
                        }
                    }
                    if (approved)
                    {
                        form.FormStatus = "A";
                        if (!string.IsNullOrEmpty(config.ApprovedRejectedPartyFormPartyObject.EmployeeObject.EmailAddress) && !string.IsNullOrEmpty(form.SubmittedEmployeeObject.EmailAddress))
                        {
                            GMSCore.Entity.EmployeeCourse eCourse = (new FormActivity()).RetrieveEmployeeCourseByCEFID(int.Parse(hidFormID.Value));
                            this.SendApprovedEmail(form.SubmittedEmployeeObject.EmailAddress, form.SubmittedEmployeeObject.Name, form.SubmittedEmployeeObject.Name, eCourse);
                            this.SendApprovedEmail(config.ApprovedRejectedPartyFormPartyObject.EmployeeObject.EmailAddress, config.ApprovedRejectedPartyFormPartyObject.EmployeeObject.Name, form.SubmittedEmployeeObject.Name, eCourse);

                            GMSCore.Entity.Employee notificationParty = null;
                            if (config.NotificationParty == "SP")
                            {
                                notificationParty = eCourse.EmployeeObject.SuperiorObject;
                            }
                            else if (config.NotificationParty == "UH")
                            {
                                notificationParty = (new EmployeeActivity()).RetrieveUnitHeadByDepartment(eCourse.EmployeeObject.Department);
                            }
                            else
                            {
                                notificationParty = config.NotificationPartyFormPartyObject.EmployeeObject;
                            }
                            if (notificationParty != null && !string.IsNullOrEmpty(notificationParty.EmailAddress))
                            {
                                SendNotificationEmail(notificationParty.EmailAddress, notificationParty.Name, form.SubmittedEmployeeObject.Name, eCourse);
                                form.NotifiedEmployeeID = notificationParty.EmployeeID;
                            }
                            
                        }
                        else
                        {
                            Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The Email address is not properly set up. Please contact your System Administrator.");
                            return;
                        }
                    }
                    else
                    {
                        form.FormStatus = "P";
                    }
                    form.Save();
                }

                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The form has been processed.");
                return;
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + ex.Message + "\");</script>", false);
                return;
            }
        }
        #endregion

        #region SendApprovedEmail
        private void SendApprovedEmail(string userEmail, string userRealName, string applicantName, GMSCore.Entity.EmployeeCourse eCourse)
        {
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);

            mail.ReplyTo = new System.Net.Mail.MailAddress("ray.tong@leedenlimited.com", "Tong Rui, Ray");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[GMS - Course Evaluation Form Approved]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dear " + userRealName + ",</p>\n" +
                        "<p>The course evaluation form submitted by " + applicantName + " has been approved.\n</p>" +
                        "<p><table><tr><td>Course Provider</td><td>:</td><td>" + eCourse.CourseObject.CourseOrganizerObject.OrganizerName + "</td></tr>" +
                        "<tr><td>Course Title</td><td>:</td><td>" + eCourse.CourseObject.CourseTitle + "</td></tr>" +
                        "<tr><td>Course Date</td><td>:</td><td>" + eCourse.DateFrom.ToString("dd/MM/yyyy") + " to " + eCourse.DateTo.ToString("dd/MM/yyyy") + "</td></tr>" +
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

        #region SendNotificationEmail
        private void SendNotificationEmail(string userEmail, string userRealName, string applicantName, GMSCore.Entity.EmployeeCourse eCourse)
        {
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);

            mail.ReplyTo = new System.Net.Mail.MailAddress("ray.tong@leedenlimited.com", "Tong Rui, Ray");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[GMS - Course Evaluation Form Approved]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dear " + userRealName + ",</p>\n" +
                        "<p>The course evaluation form submitted by " + applicantName + " has been approved.\n<br />" +
                        "<a href=\"http://sgcitrix.leedenlimited.com/GMS/SysHR/Training/AddEditCEF.aspx?CEFID=" + eCourse.CEFID.ToString() + "\">Click here to view.</a></p>\n" +
                        "<p><table><tr><td>Course Provider</td><td>:</td><td>" + eCourse.CourseObject.CourseOrganizerObject.OrganizerName + "</td></tr>" +
                        "<tr><td>Course Title</td><td>:</td><td>" + eCourse.CourseObject.CourseTitle + "</td></tr>" +
                        "<tr><td>Course Date</td><td>:</td><td>" + eCourse.DateFrom.ToString("dd/MM/yyyy") + " to " + eCourse.DateTo.ToString("dd/MM/yyyy") + "</td></tr>" +
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

        #region printReport
        protected void printReport(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(typeof(string), "CEF Report",
                string.Format("jsOpenOperationalReport('SysHR/Reports/ReportViewer.aspx?REPORT={0}&&FORMID={1}&&REPORTID=-1');",
                                    "CEFReport", hidFormID.Value)
                                    , true);
        }
        #endregion
    }
}
