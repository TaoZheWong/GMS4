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
    public partial class AddEditTEF : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.Params["RANDOMID"] != null)
                {
                    hidRandomID.Value = Request.Params["RANDOMID"].ToString();
                }
                if (Request.Params["EMPLOYEECOURSEID"] != null)
                {
                    hidEmployeeCourseID.Value = Request.Params["EMPLOYEECOURSEID"].ToString();

                }
                LoadData();
            }

        }

        #region LoadData
        private void LoadData()
        {
            #region Load By RandomID
            if (hidRandomID.Value != "")
            {

                FormApproval fa = new FormActivity().RetrieveFormApporvalByRandomID(hidRandomID.Value);
                if (fa != null && fa.FormType == "TEF")
                {
                    GMSCore.Entity.EmployeeCourse eCourse = fa.EmployeeCourseObject;
                    if (eCourse != null)
                    {
                        lblEmployeeName.Text = eCourse.EmployeeObject.Name;
                        lblEmployeeNumber.Text = eCourse.EmployeeObject.EmployeeNo;
                        lblDepartment.Text = eCourse.EmployeeObject.Department;
                        lblSuperiorName.Text = (eCourse.EmployeeObject.SuperiorObject != null) ? eCourse.EmployeeObject.SuperiorObject.Name : "";
                        lblCourseTitle.Text = eCourse.CourseSessionObject.CourseObject.CourseTitle;
                        lblCourseType.Text = (eCourse.CourseSessionObject.CourseObject.CourseType == "I") ? "Internal" : "External";
                        lblOrganizerName.Text = eCourse.CourseSessionObject.CourseObject.CourseOrganizerObject.OrganizerName;
                        lblCourseDate.Text = (eCourse.CourseSessionObject.DateFrom.ToString().Equals("1/01/1900 12:00:00 AM") ? "" : eCourse.CourseSessionObject.DateFrom.Value.ToString("dd/MM/yyyy HH:mm")) + " To " +
                            (eCourse.CourseSessionObject.DateTo.ToString().Equals("1/01/1900 12:00:00 AM") ? "" : eCourse.CourseSessionObject.DateTo.Value.ToString("dd/MM/yyyy HH:mm"));
                    }
                    TEF tef = fa.EmployeeCourseObject.TEFList[0];
                    if (tef != null && tef.Status != "P")
                    {
                        switch (tef.ContentRelevant)
                        {
                            case 1:
                                rbContentRelevant1.Checked = true;
                                break;
                            case 2:
                                rbContentRelevant2.Checked = true;
                                break;
                            case 3:
                                rbContentRelevant3.Checked = true;
                                break;
                            case 4:
                                rbContentRelevant4.Checked = true;
                                break;
                            case 5:
                                rbContentRelevant5.Checked = true;
                                break;
                        }

                        switch (tef.ContentWellOrganized)
                        {
                            case 1:
                                rbContentWellOrganized1.Checked = true;
                                break;
                            case 2:
                                rbContentWellOrganized2.Checked = true;
                                break;
                            case 3:
                                rbContentWellOrganized3.Checked = true;
                                break;
                            case 4:
                                rbContentWellOrganized4.Checked = true;
                                break;
                            case 5:
                                rbContentWellOrganized5.Checked = true;
                                break;
                        }

                        switch (tef.CourseClear)
                        {
                            case 1:
                                rbCourseClear1.Checked = true;
                                break;
                            case 2:
                                rbCourseClear2.Checked = true;
                                break;
                            case 3:
                                rbCourseClear3.Checked = true;
                                break;
                            case 4:
                                rbCourseClear4.Checked = true;
                                break;
                            case 5:
                                rbCourseClear5.Checked = true;
                                break;
                        }

                        switch (tef.EncourageParticipation)
                        {
                            case 1:
                                rbEncourageParticipation1.Checked = true;
                                break;
                            case 2:
                                rbEncourageParticipation2.Checked = true;
                                break;
                            case 3:
                                rbEncourageParticipation3.Checked = true;
                                break;
                            case 4:
                                rbEncourageParticipation4.Checked = true;
                                break;
                            case 5:
                                rbEncourageParticipation5.Checked = true;
                                break;
                        }

                        switch (tef.MethodEffective)
                        {
                            case 1:
                                rbMethodEffective1.Checked = true;
                                break;
                            case 2:
                                rbMethodEffective2.Checked = true;
                                break;
                            case 3:
                                rbMethodEffective3.Checked = true;
                                break;
                            case 4:
                                rbMethodEffective4.Checked = true;
                                break;
                            case 5:
                                rbMethodEffective5.Checked = true;
                                break;
                        }

                        switch (tef.CourseMeetObjects)
                        {
                            case 1:
                                rbCourseMeetObjects1.Checked = true;
                                break;
                            case 2:
                                rbCourseMeetObjects2.Checked = true;
                                break;
                            case 3:
                                rbCourseMeetObjects3.Checked = true;
                                break;
                            case 4:
                                rbCourseMeetObjects4.Checked = true;
                                break;
                            case 5:
                                rbCourseMeetObjects5.Checked = true;
                                break;
                        }

                        switch (tef.CourseMeetExpectation)
                        {
                            case 1:
                                rbCourseMeetExpectation1.Checked = true;
                                break;
                            case 2:
                                rbCourseMeetExpectation2.Checked = true;
                                break;
                            case 3:
                                rbCourseMeetExpectation3.Checked = true;
                                break;
                            case 4:
                                rbCourseMeetExpectation4.Checked = true;
                                break;
                            case 5:
                                rbCourseMeetExpectation5.Checked = true;
                                break;
                        }

                        switch (tef.SatisfiedWithCourse)
                        {
                            case 1:
                                rbSatisfiedWithCourse1.Checked = true;
                                break;
                            case 2:
                                rbSatisfiedWithCourse2.Checked = true;
                                break;
                            case 3:
                                rbSatisfiedWithCourse3.Checked = true;
                                break;
                            case 4:
                                rbSatisfiedWithCourse4.Checked = true;
                                break;
                            case 5:
                                rbSatisfiedWithCourse5.Checked = true;
                                break;
                        }

                        txtBestArea.Text = tef.BestArea;
                        txtAreaNeedImprovement.Text = tef.AreaNeedImprovement;
                        txtOtherComments.Text = tef.OtherComments;

                        rbContentRelevant1.Enabled = false;
                        rbContentRelevant2.Enabled = false;
                        rbContentRelevant3.Enabled = false;
                        rbContentRelevant4.Enabled = false;
                        rbContentRelevant5.Enabled = false;
                        rbContentWellOrganized1.Enabled = false;
                        rbContentWellOrganized2.Enabled = false;
                        rbContentWellOrganized3.Enabled = false;
                        rbContentWellOrganized4.Enabled = false;
                        rbContentWellOrganized5.Enabled = false;
                        rbCourseClear1.Enabled = false;
                        rbCourseClear2.Enabled = false;
                        rbCourseClear3.Enabled = false;
                        rbCourseClear4.Enabled = false;
                        rbCourseClear5.Enabled = false;
                        rbEncourageParticipation1.Enabled = false;
                        rbEncourageParticipation2.Enabled = false;
                        rbEncourageParticipation3.Enabled = false;
                        rbEncourageParticipation4.Enabled = false;
                        rbEncourageParticipation5.Enabled = false;
                        rbMethodEffective1.Enabled = false;
                        rbMethodEffective2.Enabled = false;
                        rbMethodEffective3.Enabled = false;
                        rbMethodEffective4.Enabled = false;
                        rbMethodEffective5.Enabled = false;
                        rbCourseMeetObjects1.Enabled = false;
                        rbCourseMeetObjects2.Enabled = false;
                        rbCourseMeetObjects3.Enabled = false;
                        rbCourseMeetObjects4.Enabled = false;
                        rbCourseMeetObjects5.Enabled = false;
                        rbCourseMeetExpectation1.Enabled = false;
                        rbCourseMeetExpectation2.Enabled = false;
                        rbCourseMeetExpectation3.Enabled = false;
                        rbCourseMeetExpectation4.Enabled = false;
                        rbCourseMeetExpectation5.Enabled = false;
                        rbSatisfiedWithCourse1.Enabled = false;
                        rbSatisfiedWithCourse2.Enabled = false;
                        rbSatisfiedWithCourse3.Enabled = false;
                        rbSatisfiedWithCourse4.Enabled = false;
                        rbSatisfiedWithCourse5.Enabled = false;
                        txtBestArea.ReadOnly = true;
                        txtAreaNeedImprovement.ReadOnly = true;
                        txtOtherComments.ReadOnly = true;
                        btnSubmit.Visible = false;
                        btnUpdate.Visible = true;
                    }
                    else
                    {
                        btnSubmit.Visible = true;
                        btnUpdate.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The link has expired.");
                    return;
                }
            }
            #endregion

            #region Load By EmployeeCourseID
            if (hidEmployeeCourseID.Value != "")
            {
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

                GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(GMSUtil.ToInt(hidEmployeeCourseID.Value));
                if (eCourse != null && eCourse.TEFList != null && eCourse.TEFList.Count > 0)
                {
                    if (eCourse != null)
                    {
                        lblEmployeeName.Text = eCourse.EmployeeObject.Name;
                        lblEmployeeNumber.Text = eCourse.EmployeeObject.EmployeeNo;
                        lblDepartment.Text = eCourse.EmployeeObject.Department;
                        lblSuperiorName.Text = (eCourse.EmployeeObject.SuperiorObject != null) ? eCourse.EmployeeObject.SuperiorObject.Name : "";
                        lblCourseTitle.Text = eCourse.CourseSessionObject.CourseObject.CourseTitle;
                        lblCourseType.Text = (eCourse.CourseSessionObject.CourseObject.CourseType == "I") ? "Internal" : "External";
                        lblOrganizerName.Text = eCourse.CourseSessionObject.CourseObject.CourseOrganizerObject.OrganizerName;
                        lblCourseDate.Text = (eCourse.CourseSessionObject.DateFrom.ToString().Equals("1/01/1900 12:00:00 AM") ? "" : eCourse.CourseSessionObject.DateFrom.Value.ToString("dd/MM/yyyy HH:mm")) + " To " +
                            (eCourse.CourseSessionObject.DateTo.ToString().Equals("1/01/1900 12:00:00 AM") ? "" : eCourse.CourseSessionObject.DateTo.Value.ToString("dd/MM/yyyy HH:mm"));
                    }
                    TEF tef = eCourse.TEFList[0];
                    if (tef != null && tef.Status != "P")
                    {
                        switch (tef.ContentRelevant)
                        {
                            case 1:
                                rbContentRelevant1.Checked = true;
                                break;
                            case 2:
                                rbContentRelevant2.Checked = true;
                                break;
                            case 3:
                                rbContentRelevant3.Checked = true;
                                break;
                            case 4:
                                rbContentRelevant4.Checked = true;
                                break;
                            case 5:
                                rbContentRelevant5.Checked = true;
                                break;
                        }

                        switch (tef.ContentWellOrganized)
                        {
                            case 1:
                                rbContentWellOrganized1.Checked = true;
                                break;
                            case 2:
                                rbContentWellOrganized2.Checked = true;
                                break;
                            case 3:
                                rbContentWellOrganized3.Checked = true;
                                break;
                            case 4:
                                rbContentWellOrganized4.Checked = true;
                                break;
                            case 5:
                                rbContentWellOrganized5.Checked = true;
                                break;
                        }

                        switch (tef.CourseClear)
                        {
                            case 1:
                                rbCourseClear1.Checked = true;
                                break;
                            case 2:
                                rbCourseClear2.Checked = true;
                                break;
                            case 3:
                                rbCourseClear3.Checked = true;
                                break;
                            case 4:
                                rbCourseClear4.Checked = true;
                                break;
                            case 5:
                                rbCourseClear5.Checked = true;
                                break;
                        }

                        switch (tef.EncourageParticipation)
                        {
                            case 1:
                                rbEncourageParticipation1.Checked = true;
                                break;
                            case 2:
                                rbEncourageParticipation2.Checked = true;
                                break;
                            case 3:
                                rbEncourageParticipation3.Checked = true;
                                break;
                            case 4:
                                rbEncourageParticipation4.Checked = true;
                                break;
                            case 5:
                                rbEncourageParticipation5.Checked = true;
                                break;
                        }

                        switch (tef.MethodEffective)
                        {
                            case 1:
                                rbMethodEffective1.Checked = true;
                                break;
                            case 2:
                                rbMethodEffective2.Checked = true;
                                break;
                            case 3:
                                rbMethodEffective3.Checked = true;
                                break;
                            case 4:
                                rbMethodEffective4.Checked = true;
                                break;
                            case 5:
                                rbMethodEffective5.Checked = true;
                                break;
                        }

                        switch (tef.CourseMeetObjects)
                        {
                            case 1:
                                rbCourseMeetObjects1.Checked = true;
                                break;
                            case 2:
                                rbCourseMeetObjects2.Checked = true;
                                break;
                            case 3:
                                rbCourseMeetObjects3.Checked = true;
                                break;
                            case 4:
                                rbCourseMeetObjects4.Checked = true;
                                break;
                            case 5:
                                rbCourseMeetObjects5.Checked = true;
                                break;
                        }

                        switch (tef.CourseMeetExpectation)
                        {
                            case 1:
                                rbCourseMeetExpectation1.Checked = true;
                                break;
                            case 2:
                                rbCourseMeetExpectation2.Checked = true;
                                break;
                            case 3:
                                rbCourseMeetExpectation3.Checked = true;
                                break;
                            case 4:
                                rbCourseMeetExpectation4.Checked = true;
                                break;
                            case 5:
                                rbCourseMeetExpectation5.Checked = true;
                                break;
                        }

                        switch (tef.SatisfiedWithCourse)
                        {
                            case 1:
                                rbSatisfiedWithCourse1.Checked = true;
                                break;
                            case 2:
                                rbSatisfiedWithCourse2.Checked = true;
                                break;
                            case 3:
                                rbSatisfiedWithCourse3.Checked = true;
                                break;
                            case 4:
                                rbSatisfiedWithCourse4.Checked = true;
                                break;
                            case 5:
                                rbSatisfiedWithCourse5.Checked = true;
                                break;
                        }

                        txtBestArea.Text = tef.BestArea;
                        txtAreaNeedImprovement.Text = tef.AreaNeedImprovement;
                        txtOtherComments.Text = tef.OtherComments;

                        //rbContentRelevant1.Enabled = false;
                        //rbContentRelevant2.Enabled = false;
                        //rbContentRelevant3.Enabled = false;
                        //rbContentRelevant4.Enabled = false;
                        //rbContentRelevant5.Enabled = false;
                        //rbContentWellOrganized1.Enabled = false;
                        //rbContentWellOrganized2.Enabled = false;
                        //rbContentWellOrganized3.Enabled = false;
                        //rbContentWellOrganized4.Enabled = false;
                        //rbContentWellOrganized5.Enabled = false;
                        //rbCourseClear1.Enabled = false;
                        //rbCourseClear2.Enabled = false;
                        //rbCourseClear3.Enabled = false;
                        //rbCourseClear4.Enabled = false;
                        //rbCourseClear5.Enabled = false;
                        //rbEncourageParticipation1.Enabled = false;
                        //rbEncourageParticipation2.Enabled = false;
                        //rbEncourageParticipation3.Enabled = false;
                        //rbEncourageParticipation4.Enabled = false;
                        //rbEncourageParticipation5.Enabled = false;
                        //rbMethodEffective1.Enabled = false;
                        //rbMethodEffective2.Enabled = false;
                        //rbMethodEffective3.Enabled = false;
                        //rbMethodEffective4.Enabled = false;
                        //rbMethodEffective5.Enabled = false;
                        //rbCourseMeetObjects1.Enabled = false;
                        //rbCourseMeetObjects2.Enabled = false;
                        //rbCourseMeetObjects3.Enabled = false;
                        //rbCourseMeetObjects4.Enabled = false;
                        //rbCourseMeetObjects5.Enabled = false;
                        //rbCourseMeetExpectation1.Enabled = false;
                        //rbCourseMeetExpectation2.Enabled = false;
                        //rbCourseMeetExpectation3.Enabled = false;
                        //rbCourseMeetExpectation4.Enabled = false;
                        //rbCourseMeetExpectation5.Enabled = false;
                        //rbSatisfiedWithCourse1.Enabled = false;
                        //rbSatisfiedWithCourse2.Enabled = false;
                        //rbSatisfiedWithCourse3.Enabled = false;
                        //rbSatisfiedWithCourse4.Enabled = false;
                        //rbSatisfiedWithCourse5.Enabled = false;
                        //txtBestArea.ReadOnly = true;
                        //txtAreaNeedImprovement.ReadOnly = true;
                        //txtOtherComments.ReadOnly = true;

                        
                    }
                    btnSubmit.Visible = false;
                    btnUpdate.Visible = true;
                }
            }
            #endregion
        }
        #endregion

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            FormApproval fa = new FormActivity().RetrieveFormApporvalByRandomID(hidRandomID.Value);
            if (fa != null && fa.FormType == "TEF" && fa.ApprovalStatus == "P")
            {
                try
                {
                    GMSCore.Entity.TEF tef = fa.EmployeeCourseObject.TEFList[0];
                    if (tef == null)
                    {
                        Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The page has expired.");
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
                    tef.ContentRelevant = contentRelevant;

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
                    tef.ContentWellOrganized = contentWellOrganized;

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
                    tef.CourseClear = courseClear;

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
                    tef.EncourageParticipation = encourageParticipation;

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
                    tef.MethodEffective = methodEffective;

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
                    tef.CourseMeetObjects = courseMeetObjects;

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
                    tef.CourseMeetExpectation = courseMeetExpectation;

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

                    tef.SatisfiedWithCourse = satisfiedWithCourse;

                    tef.BestArea = txtBestArea.Text.Trim();
                    tef.AreaNeedImprovement = txtAreaNeedImprovement.Text.Trim();
                    tef.OtherComments = txtOtherComments.Text.Trim();
                    tef.Status = "A";

                    fa.ApprovalStatus = "A";
                    fa.ApprovalRandomID = "";
                    fa.ApprovalModifiedDate = DateTime.Now;
                    fa.Save();

                    tef.Save();
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The form has been submitted.");
                    return;
                }
                catch (Exception ex)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + ex.Message + "\");</script>", false);
                    return;
                }
            }
            else
            {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The record has been submitted before.");
                return;
            }
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
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            86);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(GMSUtil.ToInt(hidEmployeeCourseID.Value));
            if (eCourse != null && eCourse.TEFList != null && eCourse.TEFList.Count > 0)
            {
                try
                {
                    GMSCore.Entity.TEF tef = eCourse.TEFList[0];
                    if (tef == null)
                    {
                        Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The page has expired.");
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
                    tef.ContentRelevant = contentRelevant;

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
                    tef.ContentWellOrganized = contentWellOrganized;

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
                    tef.CourseClear = courseClear;

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
                    tef.EncourageParticipation = encourageParticipation;

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
                    tef.MethodEffective = methodEffective;

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
                    tef.CourseMeetObjects = courseMeetObjects;

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
                    tef.CourseMeetExpectation = courseMeetExpectation;

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

                    tef.SatisfiedWithCourse = satisfiedWithCourse;

                    tef.BestArea = txtBestArea.Text.Trim();
                    tef.AreaNeedImprovement = txtAreaNeedImprovement.Text.Trim();
                    tef.OtherComments = txtOtherComments.Text.Trim();
                    tef.Status = "A";

                    tef.Save();

                    foreach (FormApproval fa in eCourse.FormApprovalList)
                    {
                        if (fa.FormType == "TEF")
                        {
                            fa.ApprovalRandomID = "";
                            //fa.ApprovedOnBehalfDate = DateTime.Now;
                            //fa.ApprovedOnBehalfUser = session.UserId;
                            fa.ApprovalStatus = "A";
                            fa.ApprovalModifiedDate = DateTime.Now;
                            fa.Save();
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
            else
            {
                Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "Training evaluation record cannot be found.");
                return;
            }
        }
        #endregion
    }
}
