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
using System.Globalization;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using System.Text;
using GMSWeb.CustomCtrl;

namespace GMSWeb.SysHR.Training
{
    public partial class AddEditCourse : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("HR"); 
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("HR"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            38);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("HR"));

            if (!IsPostBack)
            {
                if (Request.Params["COURSEID"] != null)
                {
                    hidCourseID.Value = Request.Params["COURSEID"].ToString();
                    LoadData();
                }
            }
        }

        #region LoadData
        private void LoadData()
        {
            if (hidCourseID.Value != "")
            {
                GMSCore.Entity.Course course = GMSCore.Entity.Course.RetrieveByKey(GMSUtil.ToInt(hidCourseID.Value.Trim()));
                if (course != null)
                {
                    txtCourseCode.Text = course.CourseCode.ToString();
                    txtCourseTitle.Text = course.CourseTitle;
                    txtOrganizerName.Text = course.CourseOrganizerObject.OrganizerName;
                    ddlCourseType.SelectedValue = course.CourseType;
                    chkRequirePTJNPTEF.Checked = course.RequirePTJNPTEF;
                    if (course.IsBonded != null)
                        chkIsBonded.Checked = course.IsBonded.Value;
                    txtNoOfMonthsBonded.Text = course.NoOfMonthsBonded.ToString();
                    txtTotalTrainingHours.Text = course.TotalTrainingHours.ToString();
                    txtTargetAudience.Text = course.TargetAudience.ToString();
                    txtCourseObjective.Text = course.CourseObjective;
                    txtCourseDescription.Text = course.CourseDescription;
                    txtPrerequisite.Text = course.Prerequisite;
                    if (course.LocalCourseFee != null)
                        txtLocalCourseFee.Text = course.LocalCourseFee.Value.ToString("#0.00");
                    if (course.LocalRegistrationFee != null)
                        txtLocalRegistrationFee.Text = course.LocalRegistrationFee.Value.ToString("#0.00");
                    if (course.LocalExaminationFee != null)
                        txtLocalExaminationFee.Text = course.LocalExaminationFee.Value.ToString("#0.00");
                    if (course.LocalMembershipFee != null)
                        txtLocalMembershipFee.Text = course.LocalMembershipFee.Value.ToString("#0.00");
                    if (course.LocalGST != null)
                        txtLocalGST.Text = course.LocalGST.Value.ToString("#0.00");
                    if (course.OverseasFlightCost != null)
                        txtOverseasFlightCost.Text = course.OverseasFlightCost.Value.ToString("#0.00");
                    if (course.OverseasHotelCost != null)
                        txtOverseasHotelCost.Text = course.OverseasHotelCost.Value.ToString("#0.00");
                    if (course.OverseasTransportCost != null)
                        txtOverseasTransportCost.Text = course.OverseasTransportCost.Value.ToString("#0.00");
                    if (course.OverseasMealCost != null)
                        txtOverseasMealCost.Text = course.OverseasMealCost.Value.ToString("#0.00");
                    if (course.OverseasOthers != null)
                        txtOverseasOthers.Text = course.OverseasOthers.Value.ToString("#0.00");
                    if (course.OverseasSDF != null)
                        txtOverseasSDF.Text = course.OverseasSDF.Value.ToString("#0.00");
                    if (course.OtherFunding1 != null)
                        txtOtherFunding1.Text = course.OtherFunding1.Value.ToString("#0.00");
                    if (course.OtherFunding2 != null)
                        txtOtherFunding2.Text = course.OtherFunding2.Value.ToString("#0.00");
                    if (course.OtherFunding3 != null)
                        txtOtherFunding3.Text = course.OtherFunding3.Value.ToString("#0.00");
                    hidCourseCode.Value = course.CourseCode;
                    hidCourseTitle.Value = course.CourseTitle;
                }
            }
        }
        #endregion


        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (string.IsNullOrEmpty(hidCourseID.Value.Trim()))
            {
                #region Add New Record.
                try
                {
                    GMSCore.Entity.Course course = new GMSCore.Entity.Course();
                    GMSCore.Entity.DocumentNumber documentNumber = GMSCore.Entity.DocumentNumber.RetrieveByKey(61, (short)DateTime.Now.Year);

                    if (documentNumber == null)
                    {
                        documentNumber = new DocumentNumber();
                        documentNumber.CoyID = session.CompanyId;
                        documentNumber.Year = (short)DateTime.Now.Year;
                        documentNumber.QuotationNo = "0001";
                        documentNumber.ExternalCourseCodePrefix = "E";
                        documentNumber.ExternalCourseCodeNumber = "001";
                        documentNumber.InternalCourseCodePrefix = "I";
                        documentNumber.InternalCourseCodeNumber = "001";
                        documentNumber.OrganizerID = 0;
                        documentNumber.EmployeeCourseRowID = 0;
                        documentNumber.EmployeeID = 0;
                        documentNumber.AttachmentNo = "0001";
                        documentNumber.ProspectNo = "0001";
                        documentNumber.ContactNo = "0001";
                        documentNumber.CommNo = "0001";
                        documentNumber.CommCommentNo = "0001";
                        documentNumber.PurchaseNo = "0001";
                        documentNumber.MRNo = "00001";
                    }                    

                    if (txtCourseTitle.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("Course title cannot be empty.");
                        return;
                    }
                    else
                    {
                        GMSCore.Entity.Course existingCourse = new CourseActivity().RetrieveCourseByCourseTitle(txtCourseTitle.Text.Trim());
                        if (existingCourse != null)
                        {
                            base.JScriptAlertMsg("Course title existed in database.");
                            return;
                        }
                        else
                        {
                            course.CourseTitle = txtCourseTitle.Text.Trim();
                        }
                    }

                    GMSCore.Entity.CourseOrganizer organizer = new CourseOrganizerActivity().RetrieveOrganizerByOrganizerName(txtOrganizerName.Text.Trim());
                    if (organizer == null)
                    {
                        base.JScriptAlertMsg("The organizer can not be found. Please check the name or create the organizer first.");
                        return;
                    }
                    else
                    {
                        course.CourseOrganizerID = organizer.OrganizerID;
                    }
                    if (txtCourseDescription.Text.Trim().Length >= 500)
                    {
                        base.JScriptAlertMsg("Couse description must be within 500 characters.");
                        return;
                    }
                    if (txtCourseCode.Text.Trim() != "")
                    {
                        GMSCore.Entity.Course existingCourse = new CourseActivity().RetrieveCourseByCourseCode(txtCourseCode.Text.Trim());
                        if (existingCourse != null)
                        {
                            base.JScriptAlertMsg("Course code existed in database.");
                            return;
                        }
                        else
                        {
                            course.CourseCode = txtCourseCode.Text.Trim();
                        }
                    }

                    else
                    {
                        if (ddlCourseType.SelectedValue == "I")
                        {
                            string courseCode = documentNumber.InternalCourseCodePrefix.Trim() + DateTime.Now.ToString("yy") + documentNumber.InternalCourseCodeNumber.Trim();
                            GMSCore.Entity.Course existingCourse = new CourseActivity().RetrieveCourseByCourseCode(courseCode);
                            while (existingCourse != null)
                            {
                                string nxtStr = ((short)(short.Parse(documentNumber.InternalCourseCodeNumber) + 1)).ToString();
                                for (int i = nxtStr.Length; i < documentNumber.InternalCourseCodeNumber.Length; i++)
                                {
                                    nxtStr = "0" + nxtStr;
                                }
                                documentNumber.InternalCourseCodeNumber = nxtStr;
                                courseCode = documentNumber.InternalCourseCodePrefix.Trim() + DateTime.Now.ToString("yy") + documentNumber.InternalCourseCodeNumber.Trim();
                                existingCourse = new CourseActivity().RetrieveCourseByCourseCode(courseCode);
                            }
                            course.CourseCode = courseCode;
                            string nxtStr1 = ((short)(short.Parse(documentNumber.InternalCourseCodeNumber) + 1)).ToString();
                            for (int i = nxtStr1.Length; i < documentNumber.InternalCourseCodeNumber.Length; i++)
                            {
                                nxtStr1 = "0" + nxtStr1;
                            }
                            documentNumber.InternalCourseCodeNumber = nxtStr1;
                        }
                        else
                        {
                            string courseCode = documentNumber.ExternalCourseCodePrefix.Trim() + DateTime.Now.ToString("yy") + documentNumber.ExternalCourseCodeNumber.Trim();
                            GMSCore.Entity.Course existingCourse = new CourseActivity().RetrieveCourseByCourseCode(courseCode);
                            while (existingCourse != null)
                            {
                                string nxtStr = ((byte)(byte.Parse(documentNumber.ExternalCourseCodeNumber) + 1)).ToString();
                                for (int i = nxtStr.Length; i < documentNumber.ExternalCourseCodeNumber.Length; i++)
                                {
                                    nxtStr = "0" + nxtStr;
                                }
                                documentNumber.ExternalCourseCodeNumber = nxtStr;
                                courseCode = documentNumber.ExternalCourseCodePrefix.Trim() + DateTime.Now.ToString("yy") + documentNumber.ExternalCourseCodeNumber.Trim();
                                existingCourse = new CourseActivity().RetrieveCourseByCourseCode(courseCode);
                            }
                            course.CourseCode = courseCode;
                            string nxtStr1 = ((byte)(byte.Parse(documentNumber.ExternalCourseCodeNumber) + 1)).ToString();
                            for (int i = nxtStr1.Length; i < documentNumber.ExternalCourseCodeNumber.Length; i++)
                            {
                                nxtStr1 = "0" + nxtStr1;
                            }
                            documentNumber.ExternalCourseCodeNumber = nxtStr1;
                        }
                    }
                    course.CourseType = ddlCourseType.SelectedValue;
                    course.RequirePTJNPTEF = chkRequirePTJNPTEF.Checked;
                    course.IsBonded = chkIsBonded.Checked;
                    if (!string.IsNullOrEmpty(txtNoOfMonthsBonded.Text.Trim()))
                        course.NoOfMonthsBonded = GMSUtil.ToByte(txtNoOfMonthsBonded.Text.Trim());
                    else
                        course.NoOfMonthsBonded = 0;
                    if (!string.IsNullOrEmpty(txtTotalTrainingHours.Text.Trim()))
                        course.TotalTrainingHours = GMSUtil.ToByte(txtTotalTrainingHours.Text.Trim());
                    else
                        course.TotalTrainingHours = 0;
                    course.TargetAudience = txtTargetAudience.Text.Trim();
                    course.CourseObjective = txtCourseObjective.Text.Trim();
                    course.CourseDescription = txtCourseDescription.Text.Trim();
                    course.Prerequisite = txtPrerequisite.Text.Trim();
                    if (!string.IsNullOrEmpty(txtLocalCourseFee.Text.Trim()))
                        course.LocalCourseFee = GMSUtil.ToDecimal(txtLocalCourseFee.Text.Trim());
                    else
                        course.LocalCourseFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalRegistrationFee.Text.Trim()))
                        course.LocalRegistrationFee = GMSUtil.ToDecimal(txtLocalRegistrationFee.Text.Trim());
                    else
                        course.LocalRegistrationFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalExaminationFee.Text.Trim()))
                        course.LocalExaminationFee = GMSUtil.ToDecimal(txtLocalExaminationFee.Text.Trim());
                    else
                        course.LocalExaminationFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalMembershipFee.Text.Trim()))
                        course.LocalMembershipFee = GMSUtil.ToDecimal(txtLocalMembershipFee.Text.Trim());
                    else
                        course.LocalMembershipFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalGST.Text.Trim()))
                        course.LocalGST = GMSUtil.ToDecimal(txtLocalGST.Text.Trim());
                    else
                        course.LocalGST = 0;
                    if (!string.IsNullOrEmpty(txtOverseasFlightCost.Text.Trim()))
                        course.OverseasFlightCost = GMSUtil.ToDecimal(txtOverseasFlightCost.Text.Trim());
                    else
                        course.OverseasFlightCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasHotelCost.Text.Trim()))
                        course.OverseasHotelCost = GMSUtil.ToDecimal(txtOverseasHotelCost.Text.Trim());
                    else
                        course.OverseasHotelCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasTransportCost.Text.Trim()))
                        course.OverseasTransportCost = GMSUtil.ToDecimal(txtOverseasTransportCost.Text.Trim());
                    else
                        course.OverseasTransportCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasMealCost.Text.Trim()))
                        course.OverseasMealCost = GMSUtil.ToDecimal(txtOverseasMealCost.Text.Trim());
                    else
                        course.OverseasMealCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasOthers.Text.Trim()))
                        course.OverseasOthers = GMSUtil.ToDecimal(txtOverseasOthers.Text.Trim());
                    else
                        course.OverseasOthers = 0;
                    if (!string.IsNullOrEmpty(txtOverseasSDF.Text.Trim()))
                        course.OverseasSDF = GMSUtil.ToDecimal(txtOverseasSDF.Text.Trim());
                    else
                        course.OverseasSDF = 0;
                    if (!string.IsNullOrEmpty(txtOtherFunding1.Text.Trim()))
                        course.OtherFunding1 = GMSUtil.ToDecimal(txtOtherFunding1.Text.Trim());
                    else
                        course.OtherFunding1 = 0;
                    if (!string.IsNullOrEmpty(txtOtherFunding2.Text.Trim()))
                        course.OtherFunding2 = GMSUtil.ToDecimal(txtOtherFunding2.Text.Trim());
                    else
                        course.OtherFunding2 = 0;
                    if (!string.IsNullOrEmpty(txtOtherFunding2.Text.Trim()))
                        course.OtherFunding2 = GMSUtil.ToDecimal(txtOtherFunding2.Text.Trim());
                    else
                        course.OtherFunding2 = 0;
                    if (!string.IsNullOrEmpty(txtOtherFunding3.Text.Trim()))
                        course.OtherFunding3 = GMSUtil.ToDecimal(txtOtherFunding3.Text.Trim());
                    else
                        course.OtherFunding3 = 0;

                    course.Save();
                    course.Resync();
                    documentNumber.Save();
                    documentNumber.Resync();
                    hidCourseID.Value = course.CourseID.ToString();
                    LoadData();
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("var result = confirm('Record added successfully! Add another one?'); if (result) {window.location.href = \"../../SysHR/Training/AddEditCourse.aspx\";}");
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
                #endregion
            }
            else
            {
                #region Update Record.
                try
                {
                    GMSCore.Entity.Course course = GMSCore.Entity.Course.RetrieveByKey(GMSUtil.ToInt(hidCourseID.Value.Trim()));
                    if (course == null)
                    {
                        base.JScriptAlertMsg("This course cannot be found in database.");
                        return;
                    }
                    if (txtCourseTitle.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("Course title cannot be empty.");
                        return;
                    }
                    else
                    {
                        if (hidCourseTitle.Value.Trim() != txtCourseTitle.Text.Trim())
                        {
                            GMSCore.Entity.Course existingCourse = new CourseActivity().RetrieveCourseByCourseTitle(txtCourseTitle.Text.Trim());
                            if (existingCourse != null)
                            {
                                base.JScriptAlertMsg("Course title existed in database.");
                                return;
                            }
                            else
                            {
                                course.CourseTitle = txtCourseTitle.Text.Trim();
                            }
                        }
                    }
                    if (txtCourseCode.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("Course code cannot be empty.");
                        return;
                    }
                    else
                    {
                        if (hidCourseCode.Value.Trim() != txtCourseCode.Text.Trim())
                        {
                            GMSCore.Entity.Course existingCourse = new CourseActivity().RetrieveCourseByCourseCode(txtCourseCode.Text.Trim());
                            if (existingCourse != null)
                            {
                                base.JScriptAlertMsg("Course code existed in database.");
                                return;
                            }
                            else
                            {
                                course.CourseCode = txtCourseCode.Text.Trim();
                            }
                        }
                    }

                    GMSCore.Entity.CourseOrganizer organizer = new CourseOrganizerActivity().RetrieveOrganizerByOrganizerName(txtOrganizerName.Text.Trim());
                    if (organizer == null)
                    {
                        base.JScriptAlertMsg("The organizer can not be found. Please check the name or create the organizer first.");
                        return;
                    }
                    else
                    {
                        course.CourseOrganizerID = organizer.OrganizerID;
                    }
                    if (txtCourseDescription.Text.Trim().Length >= 500)
                    {
                        base.JScriptAlertMsg("Couse description must be within 500 characters.");
                        return;
                    }

                    course.CourseType = ddlCourseType.SelectedValue;
                    course.RequirePTJNPTEF = chkRequirePTJNPTEF.Checked;
                    course.IsBonded = chkIsBonded.Checked;
                    if (!string.IsNullOrEmpty(txtNoOfMonthsBonded.Text.Trim()))
                        course.NoOfMonthsBonded = GMSUtil.ToByte(txtNoOfMonthsBonded.Text.Trim());
                    else
                        course.NoOfMonthsBonded = 0;
                    if (!string.IsNullOrEmpty(txtTotalTrainingHours.Text.Trim()))
                        course.TotalTrainingHours = GMSUtil.ToDecimal(txtTotalTrainingHours.Text.Trim());
                    else
                        course.TotalTrainingHours = 0;
                    course.TargetAudience = txtTargetAudience.Text.Trim();
                    course.CourseObjective = txtCourseObjective.Text.Trim();
                    course.CourseDescription = txtCourseDescription.Text.Trim();
                    course.Prerequisite = txtPrerequisite.Text.Trim();
                    if (!string.IsNullOrEmpty(txtLocalCourseFee.Text.Trim()))
                        course.LocalCourseFee = GMSUtil.ToDecimal(txtLocalCourseFee.Text.Trim());
                    else
                        course.LocalCourseFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalRegistrationFee.Text.Trim()))
                        course.LocalRegistrationFee = GMSUtil.ToDecimal(txtLocalRegistrationFee.Text.Trim());
                    else
                        course.LocalRegistrationFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalExaminationFee.Text.Trim()))
                        course.LocalExaminationFee = GMSUtil.ToDecimal(txtLocalExaminationFee.Text.Trim());
                    else
                        course.LocalExaminationFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalMembershipFee.Text.Trim()))
                        course.LocalMembershipFee = GMSUtil.ToDecimal(txtLocalMembershipFee.Text.Trim());
                    else
                        course.LocalMembershipFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalGST.Text.Trim()))
                        course.LocalGST = GMSUtil.ToDecimal(txtLocalGST.Text.Trim());
                    else
                        course.LocalGST = 0;
                    if (!string.IsNullOrEmpty(txtOverseasFlightCost.Text.Trim()))
                        course.OverseasFlightCost = GMSUtil.ToDecimal(txtOverseasFlightCost.Text.Trim());
                    else
                        course.OverseasFlightCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasHotelCost.Text.Trim()))
                        course.OverseasHotelCost = GMSUtil.ToDecimal(txtOverseasHotelCost.Text.Trim());
                    else
                        course.OverseasHotelCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasTransportCost.Text.Trim()))
                        course.OverseasTransportCost = GMSUtil.ToDecimal(txtOverseasTransportCost.Text.Trim());
                    else
                        course.OverseasTransportCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasMealCost.Text.Trim()))
                        course.OverseasMealCost = GMSUtil.ToDecimal(txtOverseasMealCost.Text.Trim());
                    else
                        course.OverseasMealCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasOthers.Text.Trim()))
                        course.OverseasOthers = GMSUtil.ToDecimal(txtOverseasOthers.Text.Trim());
                    else
                        course.OverseasOthers = 0;
                    if (!string.IsNullOrEmpty(txtOverseasSDF.Text.Trim()))
                        course.OverseasSDF = GMSUtil.ToDecimal(txtOverseasSDF.Text.Trim());
                    else
                        course.OverseasSDF = 0;
                    if (!string.IsNullOrEmpty(txtOtherFunding1.Text.Trim()))
                        course.OtherFunding1 = GMSUtil.ToDecimal(txtOtherFunding1.Text.Trim());
                    else
                        course.OtherFunding1 = 0;
                    if (!string.IsNullOrEmpty(txtOtherFunding2.Text.Trim()))
                        course.OtherFunding2 = GMSUtil.ToDecimal(txtOtherFunding2.Text.Trim());
                    else
                        course.OtherFunding2 = 0;
                    if (!string.IsNullOrEmpty(txtOtherFunding2.Text.Trim()))
                        course.OtherFunding2 = GMSUtil.ToDecimal(txtOtherFunding2.Text.Trim());
                    else
                        course.OtherFunding2 = 0;
                    if (!string.IsNullOrEmpty(txtOtherFunding3.Text.Trim()))
                        course.OtherFunding3 = GMSUtil.ToDecimal(txtOtherFunding3.Text.Trim());
                    else
                        course.OtherFunding3 = 0;

                    course.Save();
                    course.Resync();
                    hidCourseID.Value = course.CourseID.ToString();
                    LoadData();
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("var result = confirm('Record modified successfully! Add another new one?'); if (result) {window.location.href = \"../../SysHR/Training/AddEditCourse.aspx\";}");
                    str.Append("</script>");
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
                #endregion
            }
        }
        #endregion
    }
}
