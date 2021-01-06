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
            Master.setCurrentLink("HR"); 
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("HR"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            39);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("HR"));

            if (!IsPostBack)
            {
                LoadCourseLanguageDDL();
                if (Request.Params["COURSESESSIONID"] != null)
                {
                    hidCourseSessionID.Value = Request.Params["COURSESESSIONID"].ToString();
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
                    if (courseSession.OtherCost != null)
                        txtOtherCost.Text = courseSession.OtherCost.Value.ToString("#0.00");
                    if (courseSession.Remarks != null)
                        txtRemarks.Text = courseSession.Remarks;
                    lnkCourseRegistration.NavigateUrl = "https://" + (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + "/GMS3/SysHR/Training/CourseRegistration.aspx?COURSESESSIONID=" + courseSession.CourseSessionID;
                    lnkCourseRegistration.Text = "Click here to register new participants";
                }
                btnDuplicate.Visible = true;
                btnAttendee.Visible = true;
            }
            else
            {
                btnDuplicate.Visible = false;
                btnAttendee.Visible = false;
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
                    if (!string.IsNullOrEmpty(txtOtherCost.Text.Trim()))
                        courseSession.OtherCost = GMSUtil.ToDecimal(txtOtherCost.Text.Trim());
                    else
                        courseSession.OtherCost = 0;
                    courseSession.Remarks = txtRemarks.Text.Trim();

                    courseSession.Save();
                    courseSession.Resync();
                    hidCourseSessionID.Value = courseSession.CourseSessionID.ToString();
                    LoadData();
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record added successfully! Add another one?';");
                    str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
                    str.Append("if (r) {window.location.href = \"../../SysHR/Training/AddEditSession.aspx\";}");
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
                    if (!string.IsNullOrEmpty(txtOtherCost.Text.Trim()))
                        courseSession.OtherCost = GMSUtil.ToDecimal(txtOtherCost.Text.Trim());
                    else
                        courseSession.OtherCost = 0;
                    courseSession.Remarks = txtRemarks.Text.Trim();

                    courseSession.Save();
                    courseSession.Resync();
                    hidCourseSessionID.Value = courseSession.CourseSessionID.ToString();
                    LoadData();
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("var url = '../../Common/YesNo_PopUp.aspx?Msg=Record modified successfully! Add another new one?';");
                    str.Append("var r = showModalDialog(url,'','dialogHeight:150px;dialogWidth:300px;status:no;');");
                    str.Append("if (r) {window.location.href = \"../../SysHR/Training/AddEditSession.aspx\";}");
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
                base.JScriptAlertMsg("Course cannot be found in database. Please check your course title or create the course first.");
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
            btnAttendee.Visible = false;
        }
        #endregion

        #region btnAttendee_Click
        protected void btnAttendee_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(typeof(string), "Attendees",
                         string.Format("jsOpenReport('SysHR/Training/TrainingAttendee.aspx?CourseSessionID={0}');",
                                        hidCourseSessionID.Value.Trim()),
                                        true);
        }
        #endregion
    }
}
