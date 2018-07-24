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
using System.Collections.Generic;
using GMSCore.Activity;
using GMSCore;
using GMSWeb.CustomCtrl;
using System.Text;
using GMSCore.Entity;

namespace GMSWeb.SysHR.Training
{
    public partial class AddEmployeeCourse : GMSBasePage
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
                                                                            78);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!IsPostBack)
            {
                LoadDDL();
                //txtNewEmployeeName.Focus();
                if (Request.Params["ROWID"] != null)
                {
                    hidRowID.Value = Request.Params["ROWID"].ToString();
                    LoadData();
                }
            }
            if (hidRowID.Value != "")
            {
                btnAdd.Visible = false;
                btnUpdate.Visible = true;
                btnDuplicate.Visible = true;
            }
            else
            {
                btnAdd.Visible = true;
                btnUpdate.Visible = false;
                btnDuplicate.Visible = false;
            }
        }

        private void LoadDDL()
        {
            if (ddlNewEmployee != null)
            {
                SystemDataActivity sDataActivity = new SystemDataActivity();
                // fill in employee dropdown list
                IList<GMSCore.Entity.Employee> lstEmployee = null;
                lstEmployee = sDataActivity.RetrieveAllEmployeeListSortByEmployeeNo();
                ddlNewEmployee.DataSource = lstEmployee;
                ddlNewEmployee.DataBind();
                ddlNewEmployee.Items.Insert(0, new ListItem("0", "0"));
                ddlNewEmployee.SelectedIndex = 0;
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlNewEmployee, ddlNewEmployee.GetType(), "script1", "<script type=\"text/javascript\"> var ddlNewEmployeeID = '" + ddlNewEmployee.ClientID + "';</script>", false);
            }
            if (ddlNewCurrency != null)
            {
                SystemDataActivity sDataActivity = new SystemDataActivity();

                // fill in currency dropdown list
                IList<GMSCore.Entity.Currency> lstCurrency = null;
                lstCurrency = sDataActivity.RetrieveAllCurrencyListSortByCode();
                ddlNewCurrency.DataSource = lstCurrency;
                ddlNewCurrency.DataBind();
                ddlNewCurrency.SelectedValue = "SGD";
            }
        }

        #region LoadData
        private void LoadData()
        {
            if (hidRowID.Value != "")
            {
                GMSCore.Entity.EmployeeCourse eCourse = new EmployeeCourseActivity().RetrieveEmployeeCourseByRowID(GMSUtil.ToInt(hidRowID.Value));
                if (eCourse != null)
                {
                    txtNewEmployeeName.Text = eCourse.EmployeeObject.Name;
                    ddlNewEmployee.SelectedValue = eCourse.EmployeeObject.EmployeeID.ToString();
                    txtNewOrganizerName.Text = eCourse.CourseSessionObject.CourseObject.CourseOrganizerObject.OrganizerName;
                    txtNewOrganizerName.ReadOnly = true;
                    txtNewCourseTitle.Text = eCourse.CourseSessionObject.CourseObject.CourseTitle;
                    ddlNewCourseType.SelectedValue = eCourse.CourseSessionObject.CourseObject.CourseType;
                    ddlNewCourseType.Enabled = false;
                    //newDateFrom.Text = eCourse.DateFrom.ToString().Equals("1/01/1900 12:00:00 AM") ? "" : eCourse.DateFrom.ToString("dd/MM/yyyy");
                    //newDateTo.Text = eCourse.DateTo.ToString().Equals("1/01/1900 12:00:00 AM") ? "" : eCourse.DateTo.ToString("dd/MM/yyyy");
                    //txtNewHour.Text = eCourse.TrainingHours.ToString();
                    //txtNewOffHours.Text = eCourse.OutsideOffHours.ToString();
                    //ddlNewCurrency.SelectedValue = eCourse.Currency;
                    //txtNewCourseFee.Text = eCourse.CourseFee.ToString();
                    //txtNewSDF.Text = eCourse.SDF.Value.ToString("#0.00");
                    //txtNewSRP.Text = eCourse.SRP.ToString();
                    //chkNewTNForm.Checked = eCourse.TrainingNominationForm.Value;
                    //chkNewCEForm.Checked = eCourse.CourseEvaluationForm.Value;
                    txtNewRemarks.Text = eCourse.Remarks;
                }
            }
        }
        #endregion

        //#region btnAdd_Click
        //protected void btnAdd_Click(object sender, EventArgs e)
        //{
        //    LogSession session = base.GetSessionInfo();

        //    try
        //    {
        //        GMSCore.Entity.EmployeeCourse eCourse = new GMSCore.Entity.EmployeeCourse();
        //        if (ddlNewEmployee.SelectedIndex == 0)
        //        {
        //            base.JScriptAlertMsg("The employee name is incorrect. Please input the correct name.");
        //            return;
        //        }
        //        else
        //        {
        //            eCourse.EmployeeID = short.Parse(ddlNewEmployee.SelectedValue);
        //        }

        //        eCourse.Type = ddlNewCourseType.SelectedValue;
        //        GMSCore.Entity.DocumentNumber documentNumber = GMSCore.Entity.DocumentNumber.RetrieveByKey(1, (short)DateTime.Now.Year);

        //        GMSCore.Entity.CourseOrganizer organizer = new CourseOrganizerActivity().RetrieveOrganizerByOrganizerName(txtNewOrganizerName.Text.Trim());
        //        if (organizer == null)
        //        {
        //            organizer = new GMSCore.Entity.CourseOrganizer();
        //            //organizer.OrganizerID = documentNumber.OrganizerID;
        //            organizer.OrganizerName = txtNewOrganizerName.Text.Trim();
        //            new CourseOrganizerActivity().CreateOrganizer(ref organizer, session);
        //            organizer.Resync();
        //            //documentNumber.OrganizerID++;
        //        }
        //        GMSCore.Entity.Course course = new CourseActivity().RetrieveCourseByCourseTitleByOrganizerID(txtNewCourseTitle.Text.Trim(), organizer.OrganizerID);
        //        if (course == null)
        //        {
        //            course = new GMSCore.Entity.Course();
        //            if (eCourse.Type == "Internal")
        //            {
        //                course.CourseCode = documentNumber.InternalCourseCodePrefix.Trim() + DateTime.Now.ToString("yy") + documentNumber.InternalCourseCodeNumber.Trim();
        //                string nxtStr = ((short)(short.Parse(documentNumber.InternalCourseCodeNumber) + 1)).ToString();
        //                for (int i=nxtStr.Length;i<documentNumber.InternalCourseCodeNumber.Length;i++)
        //                {
        //                    nxtStr = "0"+nxtStr;
        //                }
        //                documentNumber.InternalCourseCodeNumber = nxtStr;
        //            }
        //            else
        //            {
        //                course.CourseCode = documentNumber.ExternalCourseCodePrefix.Trim() + DateTime.Now.ToString("yy") + documentNumber.ExternalCourseCodeNumber.Trim();
        //                string nxtStr = ((byte)(byte.Parse(documentNumber.ExternalCourseCodeNumber) + 1)).ToString();
        //                for (int i = nxtStr.Length; i < documentNumber.ExternalCourseCodeNumber.Length; i++)
        //                {
        //                    nxtStr = "0" + nxtStr;
        //                }
        //                documentNumber.ExternalCourseCodeNumber = nxtStr;
        //            }
        //            course.CourseTitle = txtNewCourseTitle.Text.Trim();
        //            course.OrganizerID = organizer.OrganizerID;
        //            new CourseActivity().CreateCourse(ref course, session);
        //            course.Resync();
        //        }
        //        eCourse.CourseCode = course.CourseCode;
        //        if (GMSUtil.ToDate(newDateFrom.Text) != GMSCoreBase.DEFAULT_NO_DATE)
        //            eCourse.DateFrom = GMSUtil.ToDate(newDateFrom.Text);
        //        if (GMSUtil.ToDate(newDateTo.Text) != GMSCoreBase.DEFAULT_NO_DATE)
        //            eCourse.DateTo = GMSUtil.ToDate(newDateTo.Text);
        //        if (txtNewHour.Text.Trim() != "")
        //            eCourse.TrainingHours = GMSUtil.ToDouble(txtNewHour.Text.Trim());
        //        else
        //            eCourse.TrainingHours = 0;
        //        if (txtNewOffHours.Text.Trim() != "")
        //            eCourse.OutsideOffHours = GMSUtil.ToDouble(txtNewOffHours.Text.Trim());
        //        else
        //            eCourse.OutsideOffHours = 0;
        //        eCourse.Currency = ddlNewCurrency.SelectedValue;
        //        if (txtNewCourseFee.Text.Trim() != "")
        //            eCourse.CourseFee = GMSUtil.ToDouble(txtNewCourseFee.Text.Trim());
        //        else
        //            eCourse.CourseFee = 0;
        //        if (txtNewSDF.Text.Trim() != null)
        //            eCourse.SDF = GMSUtil.ToDouble(txtNewSDF.Text.Trim());
        //        else
        //            eCourse.SDF = 0;
        //        if (txtNewSRP.Text.Trim() != null)
        //            eCourse.SRP = GMSUtil.ToDecimal(txtNewSRP.Text.Trim());
        //        else
        //            eCourse.SRP = 0;
        //        eCourse.TrainingNominationForm = chkNewTNForm.Checked;
        //        eCourse.CourseEvaluationForm = chkNewCEForm.Checked;
        //        eCourse.Remarks = txtNewRemarks.Text.Trim();
        //        //eCourse.RowID = documentNumber.EmployeeCourseRowID;
        //        //documentNumber.EmployeeCourseRowID++;

        //        ResultType result = new EmployeeCourseActivity().CreateEmployeeCourse(ref eCourse, session);

        //        switch (result)
        //        {
        //            case ResultType.Ok:
        //                documentNumber.Save();
        //                eCourse.Resync();
        //                hidRowID.Value = eCourse.RowID.ToString();
        //                btnAdd.Visible = false;
        //                btnUpdate.Visible = true;
        //                btnDuplicate.Visible = true;
        //                LoadData();
        //                StringBuilder str = new StringBuilder();
        //                str.Append("<script language='javascript'>");
        //                str.Append("var result = confirm('Record added successfully! Add another one?'); if (result) {window.navigate(\"../../SysHR/Training/AddEmployeeCourse.aspx\");}");
        //                str.Append("</script>");
        //                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
        //                break;
        //            default:
        //                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
        //                return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
        //        return;
        //    }
        //}
        //#endregion

        //#region btnUpdate_Click
        //protected void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    LogSession session = base.GetSessionInfo();

        //    try
        //    {
        //        GMSCore.Entity.EmployeeCourse eCourse = new EmployeeCourseActivity().RetrieveEmployeeCourseByRowID(GMSUtil.ToInt(hidRowID.Value));
        //        if (ddlNewEmployee.SelectedIndex == 0)
        //        {
        //            base.JScriptAlertMsg("The employee name is incorrect. Please input the correct name.");
        //            return;
        //        }
        //        else
        //        {
        //            eCourse.EmployeeID = short.Parse(ddlNewEmployee.SelectedValue);
        //        }

        //        eCourse.Type = ddlNewCourseType.SelectedValue;
        //        GMSCore.Entity.DocumentNumber documentNumber = GMSCore.Entity.DocumentNumber.RetrieveByKey(1, (short)DateTime.Now.Year);

        //        GMSCore.Entity.CourseOrganizer organizer = eCourse.CourseObject.CourseOrganizerObject;

        //        if (organizer.OrganizerName != txtNewOrganizerName.Text.Trim())
        //        {
        //            organizer = new CourseOrganizerActivity().RetrieveOrganizerByOrganizerName(txtNewOrganizerName.Text.Trim());
        //            if (organizer == null)
        //            {
        //                organizer = new GMSCore.Entity.CourseOrganizer();
        //                //organizer.OrganizerID = documentNumber.OrganizerID;
        //                organizer.OrganizerName = txtNewOrganizerName.Text.Trim();
        //                new CourseOrganizerActivity().CreateOrganizer(ref organizer, session);
        //                organizer.Resync();
        //                //documentNumber.OrganizerID++;
        //            }
        //        }

        //        if (eCourse.CourseObject.CourseTitle != txtNewCourseTitle.Text.Trim())
        //        {
        //            GMSCore.Entity.Course course = new CourseActivity().RetrieveCourseByCourseTitleByOrganizerID(txtNewCourseTitle.Text.Trim(), organizer.OrganizerID);
        //            if (course == null)
        //            {
        //                course = new GMSCore.Entity.Course();
        //                if (eCourse.Type == "Internal")
        //                {
        //                    course.CourseCode = documentNumber.InternalCourseCodePrefix.Trim() + DateTime.Now.ToString("yy") + documentNumber.InternalCourseCodeNumber.Trim();
        //                    string nxtStr = ((short)(short.Parse(documentNumber.InternalCourseCodeNumber) + 1)).ToString();
        //                    for (int i = nxtStr.Length; i < documentNumber.InternalCourseCodeNumber.Length; i++)
        //                    {
        //                        nxtStr = "0" + nxtStr;
        //                    }
        //                    documentNumber.InternalCourseCodeNumber = nxtStr;
        //                }
        //                else
        //                {
        //                    course.CourseCode = documentNumber.ExternalCourseCodePrefix.Trim() + DateTime.Now.ToString("yy") + documentNumber.ExternalCourseCodeNumber.Trim();
        //                    string nxtStr = ((byte)(byte.Parse(documentNumber.ExternalCourseCodeNumber) + 1)).ToString();
        //                    for (int i = nxtStr.Length; i < documentNumber.ExternalCourseCodeNumber.Length; i++)
        //                    {
        //                        nxtStr = "0" + nxtStr;
        //                    }
        //                    documentNumber.ExternalCourseCodeNumber = nxtStr;
        //                }
        //                course.CourseTitle = txtNewCourseTitle.Text.Trim();
        //                course.OrganizerID = organizer.OrganizerID;
        //                new CourseActivity().CreateCourse(ref course, session);
        //                course.Resync();
        //            }
        //            eCourse.CourseCode = course.CourseCode;
        //        }
        //        if (GMSUtil.ToDate(newDateFrom.Text) != GMSCoreBase.DEFAULT_NO_DATE)
        //            eCourse.DateFrom = GMSUtil.ToDate(newDateFrom.Text);
        //        if (GMSUtil.ToDate(newDateTo.Text) != GMSCoreBase.DEFAULT_NO_DATE)
        //            eCourse.DateTo = GMSUtil.ToDate(newDateTo.Text);
        //        if (txtNewHour.Text.Trim() != "")
        //            eCourse.TrainingHours = GMSUtil.ToDouble(txtNewHour.Text.Trim());
        //        else
        //            eCourse.TrainingHours = 0;
        //        if (txtNewOffHours.Text.Trim() != "")
        //            eCourse.OutsideOffHours = GMSUtil.ToDouble(txtNewOffHours.Text.Trim());
        //        else
        //            eCourse.OutsideOffHours = 0;
        //        eCourse.Currency = ddlNewCurrency.SelectedValue;
        //        if (txtNewCourseFee.Text.Trim() != "")
        //            eCourse.CourseFee = GMSUtil.ToDouble(txtNewCourseFee.Text.Trim());
        //        else
        //            eCourse.CourseFee = 0;
        //        if (txtNewSDF.Text.Trim() != "")
        //            eCourse.SDF = GMSUtil.ToDouble(txtNewSDF.Text.Trim());
        //        else
        //            eCourse.SDF = 0;
        //        if (txtNewSRP.Text.Trim() != "")
        //            eCourse.SRP = GMSUtil.ToDecimal(txtNewSRP.Text.Trim());
        //        else
        //            eCourse.SRP = 0;
        //        eCourse.TrainingNominationForm = chkNewTNForm.Checked;
        //        eCourse.CourseEvaluationForm = chkNewCEForm.Checked;
        //        eCourse.Remarks = txtNewRemarks.Text.Trim();

        //        ResultType result = new EmployeeCourseActivity().CreateEmployeeCourse(ref eCourse, session);

        //        switch (result)
        //        {
        //            case ResultType.Ok:
        //                documentNumber.Save();
        //                eCourse.Resync();
        //                hidRowID.Value = eCourse.RowID.ToString();
        //                LoadData();
        //                StringBuilder str = new StringBuilder();
        //                str.Append("<script language='javascript'>");
        //                str.Append("var result = confirm('Record updated successfully! Add another one?'); if (result) {window.navigate(\"../../SysHR/Training/AddEmployeeCourse.aspx\");}");
        //                str.Append("</script>");
        //                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "update", str.ToString(), false);
        //                break;
        //            default:
        //                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
        //                return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
        //        return;
        //    }
        //}
        //#endregion

        //#region SetOrganizer
        //protected void SetOrganizer(object sender, EventArgs e)
        //{
        //    GMSCore.Entity.Course course = new CourseActivity().RetrieveCourseByCourseTitle(txtNewCourseTitle.Text.Trim());
        //    if (course != null)
        //    {
        //        txtNewOrganizerName.Text = course.CourseOrganizerObject.OrganizerName;
        //        txtNewOrganizerName.ReadOnly = true;
        //        ddlNewCourseType.SelectedValue = (course.CourseCode.Substring(0, 1) == "E") ? "External" : "Internal";
        //        ddlNewCourseType.Enabled = false;
        //        newDateFrom.Focus();
        //    }
        //    else
        //    {
        //        txtNewOrganizerName.Text = "";
        //        txtNewOrganizerName.ReadOnly = false;
        //        ddlNewCourseType.Enabled = true;
        //        //txtNewOrganizerName.Focus();
        //        //StringBuilder str = new StringBuilder();
        //        //str.Append("<script language='javascript'>");
        //        //str.Append("document.getElementById('txtNewOrganizerName').fireEvent('onfocus');");
        //        //str.Append("</script>");
        //        //System.Web.UI.ScriptManager.RegisterStartupScript(PageMsgPanel, PageMsgPanel.GetType(), "click", str.ToString(), false);
        //    }
        //}
        //#endregion

        #region btnDuplicate_Click
        protected void btnDuplicate_Click(object sender, EventArgs e)
        {
            hidRowID.Value = "";
            txtNewEmployeeName.Text = "";
            btnAdd.Visible = true;
            btnDuplicate.Visible = false;
            btnUpdate.Visible = false;
        }
        #endregion
    }
}
