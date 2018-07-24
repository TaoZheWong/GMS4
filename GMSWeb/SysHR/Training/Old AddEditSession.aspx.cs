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

namespace GMSWeb.SysHR.Training
{
    public partial class AddEditSession : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            39);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!IsPostBack)
            {
                LoadCourseLanguageDDL();
                if (Request.Params["COURSESESSIONID"] != null)
                {
                    hidCourseSessionID.Value = Request.Params["COURSESESSIONID"].ToString();
                    LoadData();
                }
            }
        }

        #region LoadDDL
        private void LoadCourseLanguageDDL()
        {
            IList<CourseLanguage> lstLanguage = new SystemDataActivity().RetrieveAllCourseLanguageListSortByLanguageName();
            this.ddlCourseLanguage.DataSource = lstLanguage;
            this.ddlCourseLanguage.DataBind();
            //this.ddlCourseLanguage.Items.Insert(0, new ListItem("[SELECT]", "0"));
        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            if (hidCourseSessionID.Value != "")
            {
                GMSCore.Entity.CourseSession courseSession = GMSCore.Entity.CourseSession.RetrieveByKey(GMSUtil.ToInt(hidCourseSessionID.Value.Trim()));
                if (courseSession != null)
                {
                    txtCourseTitle.Text = courseSession.CourseObject.CourseTitle;
                    hidCourseTitle.Value = courseSession.CourseObject.CourseTitle;
                    txtDateFrom.Text = courseSession.DateFrom.Value.ToString("dd/MM/yyyy");
                    txtDateFromTime.Text = courseSession.DateFrom.Value.ToString("HH:mm");
                    txtDateTo.Text = courseSession.DateTo.Value.ToString("dd/MM/yyyy");
                    txtDateToTime.Text = courseSession.DateTo.Value.ToString("HH:mm");
                    ddlCourseLanguage.SelectedValue = courseSession.LanguageID;
                    txtVenue.Text = courseSession.Venue;
                    txtFacilitator.Text = courseSession.Facilitator;
                    if (courseSession.LocalCourseFee != null)
                        txtLocalCourseFee.Text = courseSession.LocalCourseFee.Value.ToString("#0.00");
                    if (courseSession.LocalRegistrationFee != null)
                        txtLocalRegistrationFee.Text = courseSession.LocalRegistrationFee.Value.ToString("#0.00");
                    if (courseSession.LocalExaminationFee != null)
                        txtLocalExaminationFee.Text = courseSession.LocalExaminationFee.Value.ToString("#0.00");
                    if (courseSession.LocalMembershipFee != null)
                        txtLocalMembershipFee.Text = courseSession.LocalMembershipFee.Value.ToString("#0.00");
                    if (courseSession.LocalGST != null)
                        txtLocalGST.Text = courseSession.LocalGST.Value.ToString("#0.00");
                    if (courseSession.OverseasFlightCost != null)
                        txtOverseasFlightCost.Text = courseSession.OverseasFlightCost.Value.ToString("#0.00");
                    if (courseSession.OverseasHotelCost != null)
                        txtOverseasHotelCost.Text = courseSession.OverseasHotelCost.Value.ToString("#0.00");
                    if (courseSession.OverseasTransportCost != null)
                        txtOverseasTransportCost.Text = courseSession.OverseasTransportCost.Value.ToString("#0.00");
                    if (courseSession.OverseasMealCost != null)
                        txtOverseasMealCost.Text = courseSession.OverseasMealCost.Value.ToString("#0.00");
                    if (courseSession.OverseasOthers != null)
                        txtOverseasOthers.Text = courseSession.OverseasOthers.Value.ToString("#0.00");
                    if (courseSession.OverseasSDF != null)
                        txtOverseasSDF.Text = courseSession.OverseasSDF.Value.ToString("#0.00");
                    lnkCourseRegistration.NavigateUrl = "http://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS/SysHR/Training/CourseRegistration.aspx?COURSESESSIONID=" + courseSession.CourseSessionID;
                    lnkCourseRegistration.Text = "http://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS/SysHR/Training/CourseRegistration.aspx?COURSESESSIONID=" + courseSession.CourseSessionID;
                }
                btnDuplicate.Visible = true;
            }
            else
            {
                btnDuplicate.Visible = false;
            }
        }
        #endregion

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (string.IsNullOrEmpty(hidCourseSessionID.Value.Trim()))
            {
                #region Add New Record.
                try
                {
                    GMSCore.Entity.CourseSession courseSession = new GMSCore.Entity.CourseSession();

                    if (txtCourseTitle.Text.Trim() == "")
                    {
                        base.JScriptAlertMsg("Course title cannot be empty.");
                        return;
                    }
                    else
                    {
                        GMSCore.Entity.Course existingCourse = new CourseActivity().RetrieveCourseByCourseTitle(txtCourseTitle.Text.Trim());
                        if (existingCourse == null)
                        {
                            base.JScriptAlertMsg("Course cannot be found in database. Please check your course title or create the course first");
                            return;
                        }
                        else
                        {
                            courseSession.CourseID = existingCourse.CourseID;
                        }
                    }
                    if (GMSUtil.ToDate(txtDateFrom.Text) != GMSCoreBase.DEFAULT_NO_DATE && txtDateFromTime.Text.Trim() != "")
                        courseSession.DateFrom = GMSUtil.ToDate(txtDateFrom.Text + " " + txtDateFromTime.Text + ":00");
                    if (GMSUtil.ToDate(txtDateTo.Text) != GMSCoreBase.DEFAULT_NO_DATE && txtDateToTime.Text.Trim() != "")
                        courseSession.DateTo = GMSUtil.ToDate(txtDateTo.Text + " " + txtDateToTime.Text + ":00");
                    courseSession.LanguageID = ddlCourseLanguage.SelectedValue;
                    courseSession.Venue = txtVenue.Text.Trim();
                    courseSession.Facilitator = txtFacilitator.Text.Trim();
                    if (!string.IsNullOrEmpty(txtLocalCourseFee.Text.Trim()))
                        courseSession.LocalCourseFee = GMSUtil.ToDecimal(txtLocalCourseFee.Text.Trim());
                    else
                        courseSession.LocalCourseFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalRegistrationFee.Text.Trim()))
                        courseSession.LocalRegistrationFee = GMSUtil.ToDecimal(txtLocalRegistrationFee.Text.Trim());
                    else
                        courseSession.LocalRegistrationFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalExaminationFee.Text.Trim()))
                        courseSession.LocalExaminationFee = GMSUtil.ToDecimal(txtLocalExaminationFee.Text.Trim());
                    else
                        courseSession.LocalExaminationFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalMembershipFee.Text.Trim()))
                        courseSession.LocalMembershipFee = GMSUtil.ToDecimal(txtLocalMembershipFee.Text.Trim());
                    else
                        courseSession.LocalMembershipFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalGST.Text.Trim()))
                        courseSession.LocalGST = GMSUtil.ToDecimal(txtLocalGST.Text.Trim());
                    else
                        courseSession.LocalGST = 0;
                    if (!string.IsNullOrEmpty(txtOverseasFlightCost.Text.Trim()))
                        courseSession.OverseasFlightCost = GMSUtil.ToDecimal(txtOverseasFlightCost.Text.Trim());
                    else
                        courseSession.OverseasFlightCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasHotelCost.Text.Trim()))
                        courseSession.OverseasHotelCost = GMSUtil.ToDecimal(txtOverseasHotelCost.Text.Trim());
                    else
                        courseSession.OverseasHotelCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasTransportCost.Text.Trim()))
                        courseSession.OverseasTransportCost = GMSUtil.ToDecimal(txtOverseasTransportCost.Text.Trim());
                    else
                        courseSession.OverseasTransportCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasMealCost.Text.Trim()))
                        courseSession.OverseasMealCost = GMSUtil.ToDecimal(txtOverseasMealCost.Text.Trim());
                    else
                        courseSession.OverseasMealCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasOthers.Text.Trim()))
                        courseSession.OverseasOthers = GMSUtil.ToDecimal(txtOverseasOthers.Text.Trim());
                    else
                        courseSession.OverseasOthers = 0;
                    if (!string.IsNullOrEmpty(txtOverseasSDF.Text.Trim()))
                        courseSession.OverseasSDF = GMSUtil.ToDecimal(txtOverseasSDF.Text.Trim());
                    else
                        courseSession.OverseasSDF = 0;

                    courseSession.Save();
                    courseSession.Resync();
                    hidCourseSessionID.Value = courseSession.CourseSessionID.ToString();
                    LoadData();
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("var result = confirm('Record added successfully! Add another one?'); if (result) {window.navigate(\"../../SysHR/Training/AddEditSession.aspx\");}");
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
                    GMSCore.Entity.CourseSession courseSession = GMSCore.Entity.CourseSession.RetrieveByKey(GMSUtil.ToInt(hidCourseSessionID.Value.Trim()));
                    if (courseSession == null)
                    {
                        base.JScriptAlertMsg("This session cannot be found in database.");
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
                            if (existingCourse == null)
                            {
                                base.JScriptAlertMsg("Course cannot be found in database. Please check your course title or create the course first");
                                return;
                            }
                            else
                            {
                                courseSession.CourseID = existingCourse.CourseID;
                            }
                        }
                    }

                    if (GMSUtil.ToDate(txtDateFrom.Text) != GMSCoreBase.DEFAULT_NO_DATE && txtDateFromTime.Text.Trim() != "")
                        courseSession.DateFrom = GMSUtil.ToDate(txtDateFrom.Text + " " + txtDateFromTime.Text + ":00");
                    if (GMSUtil.ToDate(txtDateTo.Text) != GMSCoreBase.DEFAULT_NO_DATE && txtDateToTime.Text.Trim() != "")
                        courseSession.DateTo = GMSUtil.ToDate(txtDateTo.Text + " " + txtDateToTime.Text + ":00");
                    courseSession.LanguageID = ddlCourseLanguage.SelectedValue;
                    courseSession.Venue = txtVenue.Text.Trim();
                    courseSession.Facilitator = txtFacilitator.Text.Trim();
                    if (!string.IsNullOrEmpty(txtLocalCourseFee.Text.Trim()))
                        courseSession.LocalCourseFee = GMSUtil.ToDecimal(txtLocalCourseFee.Text.Trim());
                    else
                        courseSession.LocalCourseFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalRegistrationFee.Text.Trim()))
                        courseSession.LocalRegistrationFee = GMSUtil.ToDecimal(txtLocalRegistrationFee.Text.Trim());
                    else
                        courseSession.LocalRegistrationFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalExaminationFee.Text.Trim()))
                        courseSession.LocalExaminationFee = GMSUtil.ToDecimal(txtLocalExaminationFee.Text.Trim());
                    else
                        courseSession.LocalExaminationFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalMembershipFee.Text.Trim()))
                        courseSession.LocalMembershipFee = GMSUtil.ToDecimal(txtLocalMembershipFee.Text.Trim());
                    else
                        courseSession.LocalMembershipFee = 0;
                    if (!string.IsNullOrEmpty(txtLocalGST.Text.Trim()))
                        courseSession.LocalGST = GMSUtil.ToDecimal(txtLocalGST.Text.Trim());
                    else
                        courseSession.LocalGST = 0;
                    if (!string.IsNullOrEmpty(txtOverseasFlightCost.Text.Trim()))
                        courseSession.OverseasFlightCost = GMSUtil.ToDecimal(txtOverseasFlightCost.Text.Trim());
                    else
                        courseSession.OverseasFlightCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasHotelCost.Text.Trim()))
                        courseSession.OverseasHotelCost = GMSUtil.ToDecimal(txtOverseasHotelCost.Text.Trim());
                    else
                        courseSession.OverseasHotelCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasTransportCost.Text.Trim()))
                        courseSession.OverseasTransportCost = GMSUtil.ToDecimal(txtOverseasTransportCost.Text.Trim());
                    else
                        courseSession.OverseasTransportCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasMealCost.Text.Trim()))
                        courseSession.OverseasMealCost = GMSUtil.ToDecimal(txtOverseasMealCost.Text.Trim());
                    else
                        courseSession.OverseasMealCost = 0;
                    if (!string.IsNullOrEmpty(txtOverseasOthers.Text.Trim()))
                        courseSession.OverseasOthers = GMSUtil.ToDecimal(txtOverseasOthers.Text.Trim());
                    else
                        courseSession.OverseasOthers = 0;
                    if (!string.IsNullOrEmpty(txtOverseasSDF.Text.Trim()))
                        courseSession.OverseasSDF = GMSUtil.ToDecimal(txtOverseasSDF.Text.Trim());
                    else
                        courseSession.OverseasSDF = 0;

                    courseSession.Save();
                    courseSession.Resync();
                    hidCourseSessionID.Value = courseSession.CourseSessionID.ToString();
                    LoadData();
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("var result = confirm('Record modified successfully! Add another new one?'); if (result) {window.navigate(\"../../SysHR/Training/AddEditSession.aspx\");}");
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

        #region SetCourseInfo
        protected void SetCourseInfo(object sender, EventArgs e)
        {
            GMSCore.Entity.Course course = new CourseActivity().RetrieveCourseByCourseTitle(txtCourseTitle.Text.Trim());
            if (course != null)
            {
                txtLocalCourseFee.Text = course.LocalCourseFee.Value.ToString("#0.00");
                txtLocalRegistrationFee.Text = course.LocalRegistrationFee.Value.ToString("#0.00");
                txtLocalExaminationFee.Text = course.LocalExaminationFee.Value.ToString("#0.00");
                txtLocalMembershipFee.Text = course.LocalMembershipFee.Value.ToString("#0.00");
                txtLocalGST.Text = course.LocalGST.Value.ToString("#0.00");
                txtOverseasFlightCost.Text = course.OverseasFlightCost.Value.ToString("#0.00");
                txtOverseasHotelCost.Text = course.OverseasHotelCost.Value.ToString("#0.00");
                txtOverseasTransportCost.Text = course.OverseasTransportCost.Value.ToString("#0.00");
                txtOverseasMealCost.Text = course.OverseasMealCost.Value.ToString("#0.00");
                txtOverseasOthers.Text = course.OverseasOthers.Value.ToString("#0.00");
                txtOverseasSDF.Text = course.OverseasSDF.Value.ToString("#0.00");
            }
            else
            {
                txtLocalCourseFee.Text = "";
                txtLocalRegistrationFee.Text = "";
                txtLocalExaminationFee.Text = "";
                txtLocalMembershipFee.Text = "";
                txtLocalGST.Text = "";
                txtOverseasFlightCost.Text = "";
                txtOverseasHotelCost.Text = "";
                txtOverseasTransportCost.Text = "";
                txtOverseasMealCost.Text = "";
                txtOverseasOthers.Text = "";
                txtOverseasSDF.Text = "";
                base.JScriptAlertMsg("Course cannot be found in database. Please check your course title or create the course first");
                return;
            }
        }
        #endregion

        #region btnDuplicate_Click
        protected void btnDuplicate_Click(object sender, EventArgs e)
        {
            hidCourseSessionID.Value = "";
            txtDateFrom.Text = "";
            txtDateFromTime.Text = "";
            txtDateTo.Text = "";
            txtDateToTime.Text = "";
            btnDuplicate.Visible = false;
        }
        #endregion
    }
}
