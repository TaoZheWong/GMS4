using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.SysHR.Training
{
    public partial class EmployeeCourse : GMSBasePage
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
                                                                            86);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                //preload
                dgData.CurrentPageIndex = 0;
                LoadData();
            }
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            IList<GMSCore.Entity.EmployeeCourse> lstECourse = null;
            string course = "%";
            if (!string.IsNullOrEmpty(txtSearchCourse.Text))
                course = "%"+txtSearchCourse.Text.Trim()+"%";
            string employee = "%";
            if (!string.IsNullOrEmpty(txtSearchName.Text))
                employee = "%" + txtSearchName.Text.Trim() + "%";
            string RegistrationStatus = "%";
            if (rbRegistrationStatusPending.Checked)
                RegistrationStatus = "P";
            if (rbRegistrationStatusApproved.Checked)
                RegistrationStatus = "A";
            string TEFStatus = "%";
            if (rbTEFStatusPending.Checked)
                TEFStatus = "P";
            if (rbTEFStatusCompleted.Checked)
                TEFStatus = "C";
            string PTEFStatus = "%";
            if (rbPTEFStatusPending.Checked)
                PTEFStatus = "P";
            if (rbPTEFStatusCompleted.Checked)
                PTEFStatus = "R";
            DateTime dFrom = GMSCoreBase.DEFAULT_NO_DATE.AddYears(3);
            DateTime dTo = GMSCoreBase.DEFAULT_NO_DATE.AddYears(3);
            if (GMSUtil.ToDate(dateFrom.Text) != GMSCore.GMSCoreBase.DEFAULT_NO_DATE || GMSUtil.ToDate(dateTo.Text) != GMSCore.GMSCoreBase.DEFAULT_NO_DATE)
            {
                dFrom = GMSUtil.ToDate(dateFrom.Text);
                dTo = (GMSUtil.ToDate(dateTo.Text) == GMSCore.GMSCoreBase.DEFAULT_NO_DATE) ? DateTime.Now.AddYears(2) : GMSUtil.ToDate(dateTo.Text);
            }
            try
            {
                lstECourse = new SystemDataActivity().RetrieveEmployeeCourseListByCourseByEmployeeByDateByFormStatusSortByDateEmployeeNoCourseCode(course, employee, dFrom, dTo, RegistrationStatus);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }
            if (TEFStatus == "P")
            {
                for (int i=lstECourse.Count-1;i>=0;i--)
                {
                    GMSCore.Entity.EmployeeCourse ec = lstECourse[i];
                    if (ec.TEFList.Count == 0 || ec.TEFList[0].Status != "P")
                        lstECourse.Remove(ec);
                }
            }
            if (TEFStatus == "C")
            {
                for (int i = lstECourse.Count - 1; i >= 0; i--)
                {
                    GMSCore.Entity.EmployeeCourse ec = lstECourse[i];
                    if (ec.TEFList.Count == 0 || ec.TEFList[0].Status != "A")
                        lstECourse.Remove(ec);
                }
            }
            if (PTEFStatus == "P")
            {
                for (int i = lstECourse.Count - 1; i >= 0;i--)
                {
                    GMSCore.Entity.EmployeeCourse ec = lstECourse[i];
                    if (!ec.CourseSessionObject.CourseObject.RequirePTJNPTEF || ec.ActualValueAfterCourse != null)
                        lstECourse.Remove(ec);
                }
            }
            if (PTEFStatus == "R")
            {
                for (int i = lstECourse.Count - 1; i >= 0; i--)
                {
                    GMSCore.Entity.EmployeeCourse ec = lstECourse[i];
                    if (!ec.CourseSessionObject.CourseObject.RequirePTJNPTEF || ec.ActualValueAfterCourse == null)
                        lstECourse.Remove(ec);
                }
            }
            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lstECourse != null && lstECourse.Count > 0)
            {
                if (endIndex < lstECourse.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstECourse.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstECourse.Count.ToString() + " " + "of" + " " + lstECourse.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = lstECourse;
            this.dgData.DataBind();
        }
        #endregion

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        #region dgData_ItemDataBound
        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record? Please take note that all related forms will be deleted together.')");
            }
        }
        #endregion

        #region dgData_DeleteCommand
        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                HtmlInputHidden hidEmployeeCourseID = (HtmlInputHidden)e.Item.FindControl("hidEmployeeCourseID");

                if (hidEmployeeCourseID != null)
                {
                    LogSession session = base.GetSessionInfo();
                    try
                    {
                        GMSCore.Entity.EmployeeCourse eCourse = GMSCore.Entity.EmployeeCourse.RetrieveByKey(GMSUtil.ToInt(hidEmployeeCourseID.Value));
                        if (eCourse != null)
                        {
                            foreach(FormApproval fa in eCourse.FormApprovalList)
                            {
                                fa.Delete();
                            }
                            foreach(TEF tef in eCourse.TEFList)
                            {
                                tef.Delete();
                            }
                            eCourse.Delete();
                            LoadData();
                        }
                    }
                    catch (SqlException exSql)
                    {
                        if (exSql.Number == 547)
                        {
                            this.PageMsgPanel.ShowMessage("This employee course cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                            LoadData();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        LoadData();
                        return;
                    }
                }
            }
        }
        #endregion

        //#region dgData_ItemCommand
        //protected void dgUsers_ItemCommand(object sender, DataGridCommandEventArgs e)
        //{
        //    LogSession session = base.GetSessionInfo();
        //    switch (e.CommandName)
        //    {
        //        case "SendTNF":
        //            try
        //            {
        //                HtmlInputHidden hidRowID = (HtmlInputHidden)e.Item.FindControl("hidRowID");
        //                if (hidRowID != null)
        //                {
        //                    GMSCore.Entity.EmployeeCourse ec = (new EmployeeCourseActivity()).RetrieveEmployeeCourseByRowID(int.Parse(hidRowID.Value));
        //                    if (ec != null && ec.TNFID <= 0)
        //                    {
        //                        if (!string.IsNullOrEmpty(ec.EmployeeObject.EmailAddress))
        //                        {
        //                            TNF tnf = new TNF();
        //                            tnf.Save();
        //                            tnf.Resync();

        //                            GMSCore.Entity.Form form = new GMSCore.Entity.Form();
        //                            form.FormType = "TNF";
        //                            form.FormID = tnf.FormID;
        //                            form.FormStatus = "N";
        //                            form.CreatedBy = session.UserId;
        //                            form.CreatedDate = DateTime.Now;
        //                            form.SubmittedEmployeeID = ec.EmployeeObject.EmployeeID;

        //                            FormRandomID randomID = new FormRandomID();
        //                            randomID.FormType = "TNF";
        //                            randomID.FormID = tnf.FormID;
        //                            randomID.ApprovalLevel = 0;
        //                            randomID.FormOnlineID = FormsAuthentication.HashPasswordForStoringInConfigFile("FormOnlineID" + DateTime.Now.Ticks.ToString(), "MD5");
        //                            randomID.ApplicantID = FormsAuthentication.HashPasswordForStoringInConfigFile("ApplicantID" + DateTime.Now.Ticks.ToString(), "MD5");

        //                            SendEmail(ec.EmployeeObject.EmailAddress, ec.EmployeeObject.Name, "TNF", randomID.FormOnlineID, randomID.ApplicantID, ec);

        //                            form.Save();
        //                            randomID.Save();

        //                            ec.TNFID = tnf.FormID;
        //                            ec.Save();

        //                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"Sent successfully.\");</script>", false);
        //                            LoadData();
        //                            return;
        //                        }
        //                        else
        //                        {
        //                            TNF tnf = new TNF();
        //                            tnf.Save();
        //                            tnf.Resync();

        //                            FormConfig config = FormConfig.RetrieveByKey("TNF");
        //                            Employee sendObject = null;
        //                            string code = config.AlternateParty;

        //                            if (code == "SP")
        //                            {
        //                                sendObject = ec.EmployeeObject.SuperiorObject;
        //                            }
        //                            else if (code == "UH")
        //                            {
        //                                sendObject = (new EmployeeActivity()).RetrieveUnitHeadByDepartment(ec.EmployeeObject.Department);
        //                            }
        //                            else
        //                            {
        //                                sendObject = config.AlternatePartyFormPartyObject.EmployeeObject;
        //                            }

        //                            GMSCore.Entity.Form form = new GMSCore.Entity.Form();
        //                            form.FormType = "TNF";
        //                            form.FormID = tnf.FormID;
        //                            form.FormStatus = "N";
        //                            form.CreatedBy = session.UserId;
        //                            form.CreatedDate = DateTime.Now;
        //                            form.SubmittedEmployeeID = sendObject.EmployeeID;

        //                            FormRandomID randomID = new FormRandomID();
        //                            randomID.FormType = "TNF";
        //                            randomID.FormID = tnf.FormID;
        //                            randomID.ApprovalLevel = 0;
        //                            randomID.FormOnlineID = FormsAuthentication.HashPasswordForStoringInConfigFile("FormOnlineID" + DateTime.Now.Ticks.ToString(), "MD5");
        //                            randomID.ApplicantID = FormsAuthentication.HashPasswordForStoringInConfigFile("ApplicantID" + DateTime.Now.Ticks.ToString(), "MD5");

        //                            SendEmail(sendObject.EmailAddress, sendObject.Name, "TNF", randomID.FormOnlineID, randomID.ApplicantID, ec);

        //                            form.Save();
        //                            randomID.Save();

        //                            ec.TNFID = tnf.FormID;
        //                            ec.Save();

        //                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"Sent successfully.\");</script>", false);
        //                            LoadData();
        //                            return;
        //                        }
        //                    } 
        //                    else
        //                    {
        //                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"The TNF has already been sent to the respective parties.\");</script>", false);
        //                        LoadData();
        //                        return;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
        //                LoadData();
        //                return;
        //            }
        //            break;

        //        case "SendCEF":
        //            try
        //            {
        //                HtmlInputHidden hidRowID = (HtmlInputHidden)e.Item.FindControl("hidRowID");
        //                if (hidRowID != null)
        //                {
        //                    GMSCore.Entity.EmployeeCourse ec = (new EmployeeCourseActivity()).RetrieveEmployeeCourseByRowID(int.Parse(hidRowID.Value));
        //                    if (ec != null && ec.CEFID <= 0)
        //                    {
        //                        if (!string.IsNullOrEmpty(ec.EmployeeObject.EmailAddress))
        //                        {
        //                            CEF cef = new CEF();
        //                            cef.Save();
        //                            cef.Resync();

        //                            GMSCore.Entity.Form form = new GMSCore.Entity.Form();
        //                            form.FormType = "CEF";
        //                            form.FormID = cef.FormID;
        //                            form.FormStatus = "N";
        //                            form.CreatedBy = session.UserId;
        //                            form.CreatedDate = DateTime.Now;
        //                            form.SubmittedEmployeeID = ec.EmployeeObject.EmployeeID;

        //                            FormRandomID randomID = new FormRandomID();
        //                            randomID.FormType = "CEF";
        //                            randomID.FormID = cef.FormID;
        //                            randomID.ApprovalLevel = 0;
        //                            randomID.FormOnlineID = FormsAuthentication.HashPasswordForStoringInConfigFile("FormOnlineID" + DateTime.Now.Ticks.ToString(), "MD5");
        //                            randomID.ApplicantID = FormsAuthentication.HashPasswordForStoringInConfigFile("ApplicantID" + DateTime.Now.Ticks.ToString(), "MD5");

        //                            SendEmail(ec.EmployeeObject.EmailAddress, ec.EmployeeObject.Name, "CEF", randomID.FormOnlineID, randomID.ApplicantID, ec);

        //                            form.Save();
        //                            randomID.Save();

        //                            ec.CEFID = cef.FormID;
        //                            ec.Save();

        //                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"Sent successfully.\");</script>", false);
        //                            LoadData();
        //                            return;
        //                        }
        //                        else
        //                        {
        //                            CEF cef = new CEF();
        //                            cef.Save();
        //                            cef.Resync();

        //                            FormConfig config = FormConfig.RetrieveByKey("CEF");
        //                            Employee sendObject = null;
        //                            string code = config.AlternateParty;

        //                            if (code == "SP")
        //                            {
        //                                sendObject = ec.EmployeeObject.SuperiorObject;
        //                            }
        //                            else if (code == "UH")
        //                            {
        //                                sendObject = (new EmployeeActivity()).RetrieveUnitHeadByDepartment(ec.EmployeeObject.Department);
        //                            }
        //                            else
        //                            {
        //                                sendObject = config.AlternatePartyFormPartyObject.EmployeeObject;
        //                            }

        //                            GMSCore.Entity.Form form = new GMSCore.Entity.Form();
        //                            form.FormType = "CEF";
        //                            form.FormID = cef.FormID;
        //                            form.FormStatus = "N";
        //                            form.CreatedBy = session.UserId;
        //                            form.CreatedDate = DateTime.Now;
        //                            form.SubmittedEmployeeID = sendObject.EmployeeID;

        //                            FormRandomID randomID = new FormRandomID();
        //                            randomID.FormType = "CEF";
        //                            randomID.FormID = cef.FormID;
        //                            randomID.ApprovalLevel = 0;
        //                            randomID.FormOnlineID = FormsAuthentication.HashPasswordForStoringInConfigFile("FormOnlineID" + DateTime.Now.Ticks.ToString(), "MD5");
        //                            randomID.ApplicantID = FormsAuthentication.HashPasswordForStoringInConfigFile("ApplicantID" + DateTime.Now.Ticks.ToString(), "MD5");

        //                            SendEmail(sendObject.EmailAddress, sendObject.Name, "CEF", randomID.FormOnlineID, randomID.ApplicantID, ec);

        //                            form.Save();
        //                            randomID.Save();

        //                            ec.CEFID = cef.FormID;
        //                            ec.Save();

        //                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"Sent successfully.\");</script>", false);
        //                            LoadData();
        //                            return;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"The CEF has already been sent to the respective parties.\");</script>", false);
        //                        //LoadData();
        //                        //return;
        //                        if (!string.IsNullOrEmpty(ec.EmployeeObject.EmailAddress))
        //                        {
        //                            CEF cef = CEF.RetrieveByKey(ec.CEFID);
        //                            if (cef != null)
        //                            {
        //                                cef.ContentRelevant = 0;
        //                                cef.ContentWellOrganized = 0;
        //                                cef.CourseClear = 0;
        //                                cef.EncourageParticipation = 0;
        //                                cef.MethodEffective = 0;
        //                                cef.CourseMeetObjects = 0;
        //                                cef.CourseMeetExpectation = 0;
        //                                cef.SatisfiedWithCourse = 0;
        //                                cef.BestArea = "";
        //                                cef.AreaNeedImprovement = "";
        //                                cef.OtherComments = "";
        //                            }
        //                            cef.Save();
        //                            cef.Resync();

        //                            FormActivity fActivity = new FormActivity();
        //                            GMSCore.Entity.Form form = fActivity.RetrieveFormByFormTypeFormID("CEF", cef.FormID);
        //                            form.FormStatus = "N";

        //                            IList<FormApproval> faList = fActivity.RetrieveFormApprovalListByFormTypeFormID("CEF", cef.FormID);
        //                            if (faList != null && faList.Count > 0)
        //                            {
        //                                foreach (FormApproval faTemp in faList)
        //                                {
        //                                    faTemp.Delete();
        //                                    faTemp.Resync();
        //                                }
        //                            }

        //                            FormConfig config = FormConfig.RetrieveByKey("CEF");

        //                            for (byte i = 0; i <= config.ApprovalLevel.Value; i++)
        //                            {
        //                                FormRandomID randomIDTemp = fActivity.RetrieveFormRandomIDByFormTypeFormIDApprovalLevel("CEF", cef.FormID, i);
        //                                if (randomIDTemp != null)
        //                                {
        //                                    randomIDTemp.Delete();
        //                                    randomIDTemp.Resync();
        //                                }
        //                            }

        //                            FormRandomID randomID = new FormRandomID();
        //                            randomID.FormType = "CEF";
        //                            randomID.FormID = cef.FormID;
        //                            randomID.ApprovalLevel = 0;
        //                            randomID.FormOnlineID = FormsAuthentication.HashPasswordForStoringInConfigFile("FormOnlineID" + DateTime.Now.Ticks.ToString(), "MD5");
        //                            randomID.ApplicantID = FormsAuthentication.HashPasswordForStoringInConfigFile("ApplicantID" + DateTime.Now.Ticks.ToString(), "MD5");

        //                            SendEmail(ec.EmployeeObject.EmailAddress, ec.EmployeeObject.Name, "CEF", randomID.FormOnlineID, randomID.ApplicantID, ec);

        //                            form.Save();
        //                            randomID.Save();

        //                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"Re-sent successfully.\");</script>", false);
        //                            LoadData();
        //                            return;
        //                        }
        //                        else
        //                        {
        //                            CEF cef = CEF.RetrieveByKey(ec.CEFID);
        //                            if (cef != null)
        //                            {
        //                                cef.ContentRelevant = 0;
        //                                cef.ContentWellOrganized = 0;
        //                                cef.CourseClear = 0;
        //                                cef.EncourageParticipation = 0;
        //                                cef.MethodEffective = 0;
        //                                cef.CourseMeetObjects = 0;
        //                                cef.CourseMeetExpectation = 0;
        //                                cef.SatisfiedWithCourse = 0;
        //                                cef.BestArea = "";
        //                                cef.AreaNeedImprovement = "";
        //                                cef.OtherComments = "";
        //                            }
        //                            cef.Save();
        //                            cef.Resync();

        //                            FormConfig config = FormConfig.RetrieveByKey("CEF");
        //                            Employee sendObject = null;
        //                            string code = config.AlternateParty;

        //                            if (code == "SP")
        //                            {
        //                                sendObject = ec.EmployeeObject.SuperiorObject;
        //                            }
        //                            else if (code == "UH")
        //                            {
        //                                sendObject = (new EmployeeActivity()).RetrieveUnitHeadByDepartment(ec.EmployeeObject.Department);
        //                            }
        //                            else
        //                            {
        //                                sendObject = config.AlternatePartyFormPartyObject.EmployeeObject;
        //                            }

        //                            FormActivity fActivity = new FormActivity();
        //                            GMSCore.Entity.Form form = fActivity.RetrieveFormByFormTypeFormID("CEF", cef.FormID);
        //                            form.FormStatus = "N";

        //                            IList<FormApproval> faList = fActivity.RetrieveFormApprovalListByFormTypeFormID("CEF", cef.FormID);
        //                            if (faList != null && faList.Count > 0)
        //                            {
        //                                foreach (FormApproval faTemp in faList)
        //                                {
        //                                    faTemp.Delete();
        //                                    faTemp.Resync();
        //                                }
        //                            }

        //                            for (byte i = 0; i <= config.ApprovalLevel.Value; i++)
        //                            {
        //                                FormRandomID randomIDTemp = fActivity.RetrieveFormRandomIDByFormTypeFormIDApprovalLevel("CEF", cef.FormID, i);
        //                                if (randomIDTemp != null)
        //                                {
        //                                    randomIDTemp.Delete();
        //                                    randomIDTemp.Resync();
        //                                }
        //                            }

        //                            FormRandomID randomID = new FormRandomID();
        //                            randomID.FormType = "CEF";
        //                            randomID.FormID = cef.FormID;
        //                            randomID.ApprovalLevel = 0;
        //                            randomID.FormOnlineID = FormsAuthentication.HashPasswordForStoringInConfigFile("FormOnlineID" + DateTime.Now.Ticks.ToString(), "MD5");
        //                            randomID.ApplicantID = FormsAuthentication.HashPasswordForStoringInConfigFile("ApplicantID" + DateTime.Now.Ticks.ToString(), "MD5");

        //                            SendEmail(sendObject.EmailAddress, sendObject.Name, "CEF", randomID.FormOnlineID, randomID.ApplicantID, ec);

        //                            form.Save();
        //                            randomID.Save();

        //                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"Re-sent successfully.\");</script>", false);
        //                            LoadData();
        //                            return;
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
        //                LoadData();
        //                return;
        //            }
        //            break;
        //    }
        //}
        //#endregion

        //#region SendEmail
        //private void SendEmail(string userEmail, string userRealName, string type, string id1, string id2, GMSCore.Entity.EmployeeCourse eCourse)
        //{
        //    LogSession session = base.GetSessionInfo();
        //    System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
        //    System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
        //    string smtpServer = "smtp.leedenlimited.com";

        //    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);

        //    mail.ReplyTo = new System.Net.Mail.MailAddress("ray.tong@leedenlimited.com", "Tong Rui, Ray");
        //    mail.BodyEncoding = System.Text.Encoding.UTF8;
        //    switch (type)
        //    {
        //        case "TNF":
        //            mail.Subject = "[GMS - Training Nomination Form]";
        //            mail.IsBodyHtml = true;
        //            mail.Body = "<p>Dear " + userRealName + ",</p>\n" +
        //                        "<p>Please go to Group Management System (GMS) to fill in the training nomination form" + ".\n<br />" +
        //                        "<a href=\"http://sgcitrix.leedenlimited.com/GMS/SysHR/Form.aspx?FORMONLINEID=" + id1 + "&APPLICANTID=" + id2 + "\">Click here.</a></p>\n" +
        //                        "<p><table><tr><td>Course Provider</td><td>:</td><td>" + eCourse.CourseObject.CourseOrganizerObject.OrganizerName + "</td></tr>" +
        //                        "<tr><td>Course Title</td><td>:</td><td>" + eCourse.CourseObject.CourseTitle + "</td></tr>" +
        //                        "<tr><td>Course Date</td><td>:</td><td>" + eCourse.DateFrom.ToString("dd/MM/yyyy") + " to " + eCourse.DateTo.ToString("dd/MM/yyyy") + "</td></tr>" +
        //                        "<tr><td>Employee Enrolled</td><td>:</td><td>" + eCourse.EmployeeObject.Name + "</td></tr></table></p>" + 
        //                        "***** This is a computer-generated email. Please do not reply.*****";
        //            break;

        //        case "CEF":
        //            mail.Subject = "[GMS - Course Evaluation Form]";
        //            mail.IsBodyHtml = true;
        //            mail.Body = "<p>Dear " + userRealName + ",</p>\n" +
        //                        "<p>Please go to Group Management System (GMS) to fill in the course evaluation form" + ".\n<br />" +
        //                        "<a href=\"http://sgcitrix.leedenlimited.com/GMS/SysHR/Form.aspx?FORMONLINEID=" + id1 + "&APPLICANTID=" + id2 + "\">Click here.</a></p>\n" +
        //                        "<p><table><tr><td>Course Provider</td><td>:</td><td>" + eCourse.CourseObject.CourseOrganizerObject.OrganizerName + "</td></tr>" +
        //                        "<tr><td>Course Title</td><td>:</td><td>" + eCourse.CourseObject.CourseTitle + "</td></tr>" +
        //                        "<tr><td>Course Date</td><td>:</td><td>" + eCourse.DateFrom.ToString("dd/MM/yyyy") + " to " + eCourse.DateTo.ToString("dd/MM/yyyy") + "</td></tr>" +
        //                        "<tr><td>Employee Enrolled</td><td>:</td><td>" + eCourse.EmployeeObject.Name + "</td></tr></table></p>" + 
        //                        "***** This is a computer-generated email. Please do not reply.*****";
        //            break;
        //    }

        //    try
        //    {
        //        System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
        //        mailClient.Host = smtpServer;
        //        mailClient.Port = 25;
        //        mailClient.UseDefaultCredentials = false;
        //        System.Net.NetworkCredential authentication = new System.Net.NetworkCredential("gmsadmin@leedenlimited.com", "admin2008");
        //        mailClient.Credentials = authentication;
        //        mailClient.Send(mail);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
        }
        #endregion
    }
}
