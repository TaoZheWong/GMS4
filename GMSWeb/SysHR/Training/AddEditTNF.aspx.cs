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

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using System.Text;

namespace GMSWeb.SysHR.Training
{
    public partial class AddEditTNF : GMSBasePage
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string formOnlineID = Request.Params["FORMONLINEID"];
                string applicantID = Request.Params["APPLICANTID"];
                string approvalID = Request.Params["APPROVALID"];
                string TNFID = Request.Params["TNFID"];

                if (!string.IsNullOrEmpty(formOnlineID) && !string.IsNullOrEmpty(applicantID))
                {
                    FormRandomID randomID = (new FormActivity()).RetrieveFormRandomIDByFormOnlineIDApplicantID(formOnlineID, applicantID);
                    if (randomID != null && randomID.FormType == "TNF")
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
                    if (randomID != null && randomID.FormType == "TNF")
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
                else if (!string.IsNullOrEmpty(TNFID))
                {
                    hidFormID.Value = TNFID;
                    hidView.Value = "True";
                    LoadData();
                }
                else
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The link has already been processed or expired.");
                    return;
                }
            }
        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            if (hidFormID.Value != "")
            {
                GMSCore.Entity.EmployeeCourse eCourse = (new FormActivity()).RetrieveEmployeeCourseByTNFID(int.Parse(hidFormID.Value));
                if (eCourse != null)
                {
                    txtNewEmployeeName.Text = eCourse.EmployeeObject.Name;
                    txtNewEmployeeNumber.Text = eCourse.EmployeeObject.EmployeeNo;
                    txtNewDesignation.Text = eCourse.EmployeeObject.Designation;
                    txtNewDepartment.Text = eCourse.EmployeeObject.Department;
                    txtNewSuperiorName.Text = eCourse.EmployeeObject.SuperiorObject.Name;
                    txtNewSuperiorDeisignation.Text = eCourse.EmployeeObject.SuperiorObject.Designation;
                    txtNewCourseTitle.Text = eCourse.CourseObject.CourseTitle;
                    ddlNewCourseType.SelectedValue = eCourse.Type;
                    txtNewOrganizerName.Text = eCourse.CourseObject.CourseOrganizerObject.OrganizerName;
                    newDateFrom.Text = eCourse.DateFrom.ToString().Equals("1/01/1900 12:00:00 AM") ? "" : eCourse.DateFrom.ToString("dd/MM/yyyy");
                    newDateTo.Text = eCourse.DateTo.ToString().Equals("1/01/1900 12:00:00 AM") ? "" : eCourse.DateTo.ToString("dd/MM/yyyy");
                    txtNewHour.Text = eCourse.TrainingHours.ToString();
                    txtNewCourseFee.Text = eCourse.CourseFee.ToString();
                    txtNewCourseCode.Text = eCourse.CourseObject.CourseCode;

                    TNF tnf = TNF.RetrieveByKey(int.Parse(hidFormID.Value));
                    if (tnf != null)
                    {
                        txtNewLearningObjectives.Text = tnf.LearningObjectives;
                        txtNewWhatToDo.Text = tnf.WhatToDo;
                        txtWhyDoIt.Text = tnf.WhyDoIt;
                        txtDoItByWhen.Text = tnf.DoItByWhen;
                        txtImproveWhichIndicator.Text = tnf.ImproveWhichIndicator;

                        if (tnf.InternallyNotHave.HasValue)
                            chkNewInternalNotHave.Checked = tnf.InternallyNotHave.Value;

                        if (tnf.ExternalAcceptability.HasValue)
                            chkNewExternalAcceptibility.Checked = tnf.ExternalAcceptability.Value;

                        if (tnf.AdvantageGained.HasValue)
                            chkNewAdvantageGained.Checked = tnf.AdvantageGained.Value;
                        
                    }
                }

                GMSCore.Entity.Form form = (new FormActivity()).RetrieveFormByFormTypeFormID("TNF", int.Parse(hidFormID.Value));
                if (form != null && form.FormStatus != "N")
                {
                    txtNewLearningObjectives.ReadOnly = true;
                    txtNewWhatToDo.ReadOnly = true;
                    txtWhyDoIt.ReadOnly = true;
                    txtDoItByWhen.ReadOnly = true;
                    txtImproveWhichIndicator.ReadOnly = true;
                    chkNewInternalNotHave.Enabled = false;
                    chkNewExternalAcceptibility.Enabled = false;
                    chkNewAdvantageGained.Enabled = false;

                    btnSubmit.Visible = false;
                    btnAccept.Visible = true;
                    btnReject.Visible = true;
                }

                if (hidView.Value == "True")
                {
                    if (form.FormStatus == "N")
                    {
                        SubmitPanel.Visible = true;
                        btnSubmit.Text = "Update";
                    }
                    else
                        SubmitPanel.Visible = false;
                    ApprovalStatusPanel.Visible = true;

                    if (form != null && form.FormStatus == "S")
                    {
                        ApplicationStatus.Text = "Form Status = Submitted on " + form.SubmittedDate.ToString();
                    }
                    else if (form != null && form.FormStatus == "N")
                    {
                        ApplicationStatus.Text = "Form Status = New on " + form.CreatedDate.ToString();
                    }
                    else if (form != null && form.FormStatus == "P")
                    {
                        ApplicationStatus.Text = "Form Status = Pending";
                    }
                    else if (form != null && form.FormStatus == "R")
                    {
                        ApplicationStatus.Text = "Form Status = Rejected";
                    }
                    else if (form != null && form.FormStatus == "A")
                    {
                        ApplicationStatus.Text = "Form Status = Approved";
                    }

                    IList<FormApproval> faList = (new FormActivity()).RetrieveFormApprovalListByFormTypeFormID("TNF", int.Parse(hidFormID.Value));
                    if (faList != null && faList.Count > 0)
                    {
                        foreach (FormApproval fa in faList)
                        {
                            string status = "Pending";
                            if (fa.ApprovalStatus == "A")
                                status = "Accepted on " + fa.ApprovalCreatedDate.ToString() + " by " + fa.ApprovedEmployeeObject.Name;
                            else if (fa.ApprovalStatus == "R")
                                status = "Rejected on " + fa.ApprovalCreatedDate.ToString() + " by " + fa.ApprovedEmployeeObject.Name;
                            switch (fa.ApprovalLevel)
                            {
                                case 1:
                                    Level1Status.Text = "Level 1 Approval Status = " + status;
                                    break;
                                case 2:
                                    Level2Status.Text = "Level 2 Approval Status = " + status;
                                    break;
                                case 3:
                                    Level3Status.Text = "Level 3 Approval Status = " + status;
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    SubmitPanel.Visible = true;
                    ApprovalStatusPanel.Visible = false;
                }
            }
        }
        #endregion

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                FormActivity fActivity = new FormActivity();
                GMSCore.Entity.TNF tnf = TNF.RetrieveByKey(int.Parse(hidFormID.Value));
                if (tnf == null)
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The page has expired.");
                    return;
                }

                GMSCore.Entity.Form form = fActivity.RetrieveFormByFormTypeFormID("TNF", tnf.FormID);
                if (form == null || form.FormStatus != "N")
                {
                    Response.Redirect("../../Common/Message.aspx?MESSAGE=" + "The form has already been submitted before.");
                    return;
                }

                if (hidView.Value != "True")
                {
                    form.FormStatus = "S";
                    form.SubmittedDate = DateTime.Now;
                    form.Save();
                }
                
                tnf.LearningObjectives = txtNewLearningObjectives.Text;
                tnf.WhatToDo = txtNewWhatToDo.Text;
                tnf.WhyDoIt = txtWhyDoIt.Text;
                tnf.DoItByWhen = txtDoItByWhen.Text;
                tnf.ImproveWhichIndicator = txtImproveWhichIndicator.Text;
                tnf.InternallyNotHave = chkNewInternalNotHave.Checked;
                tnf.ExternalAcceptability = chkNewExternalAcceptibility.Checked;
                tnf.AdvantageGained = chkNewAdvantageGained.Checked;
                tnf.Save();

                if (hidView.Value != "True")
                {
                    FormRandomID randomID = fActivity.RetrieveFormRandomIDByFormTypeFormIDApprovalLevel("TNF", tnf.FormID, 0);
                    randomID.Delete();
                    randomID.Resync();


                    FormConfig config = FormConfig.RetrieveByKey("TNF");

                    if (config.ApprovalLevel > 0)
                    {
                        PrepareEmail(1, config, tnf.FormID);
                    }

                    if (config.ApprovalProcess == "C")
                    {
                        if (config.ApprovalLevel >= 2)
                        {
                            PrepareEmail(2, config, tnf.FormID);
                        }

                        if (config.ApprovalLevel >= 3)
                        {
                            PrepareEmail(3, config, tnf.FormID);
                        }

                        if (config.ApprovalLevel >= 4)
                        {
                            PrepareEmail(4, config, tnf.FormID);
                        }

                        if (config.ApprovalLevel >= 5)
                        {
                            PrepareEmail(5, config, tnf.FormID);
                        }
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
                sendObject = fActivity.RetrieveEmployeeCourseByTNFID(int.Parse(hidFormID.Value)).EmployeeObject.SuperiorObject;
            }
            else if (code == "UH")
            {
                Employee unitHead = (new EmployeeActivity()).RetrieveUnitHeadByDepartment(fActivity.RetrieveEmployeeCourseByTNFID(int.Parse(hidFormID.Value)).EmployeeObject.Department);
                Employee superior = fActivity.RetrieveEmployeeCourseByTNFID(int.Parse(hidFormID.Value)).EmployeeObject.SuperiorObject;
                if (unitHead.EmployeeID == superior.EmployeeID && level == 2)
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
            fa.FormType = "TNF";
            fa.FormID = formID;
            fa.ApprovalLevel = level;
            fa.ApprovalStatus = "P";
            fa.ApprovalCreatedDate = DateTime.Now;
            fa.ApprovedEmployeeID = sendObject.EmployeeID;

            FormRandomID randomID = new FormRandomID();
            randomID.FormType = "TNF";
            randomID.FormID = formID;
            randomID.ApprovalLevel = level;
            randomID.FormOnlineID = FormsAuthentication.HashPasswordForStoringInConfigFile("FormOnlineID" + DateTime.Now.Ticks.ToString(), "MD5");
            randomID.ApprovalID = FormsAuthentication.HashPasswordForStoringInConfigFile("ApprovalID" + DateTime.Now.Ticks.ToString(), "MD5");

            if (sendObject != null && sendObject.EmailAddress != "")
            {
                GMSCore.Entity.EmployeeCourse eCourse = (new FormActivity()).RetrieveEmployeeCourseByTNFID(int.Parse(hidFormID.Value));
                SendEmail(sendObject.EmailAddress, sendObject.Name, "TNF", randomID.FormOnlineID, randomID.ApprovalID, eCourse);

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
                case "TNF":
                    mail.Subject = "[GMS - Training Nomination Form Endorsement]";
                    mail.IsBodyHtml = true;
                    mail.Body = "<p>Dear " + userRealName + ",</p>\n" +
                                "<p>Please go to Group Management System (GMS) to endorse the training nomination form" + ".\n<br />" +
                                "<a href=\"http://sgcitrix.leedenlimited.com/GMS/SysHR/Form.aspx?FORMONLINEID=" + id1 + "&ApprovalID=" + id2 + "\">Click here.</a></p>\n" +
                                "<p><table><tr><td>Course Provider</td><td>:</td><td>" + eCourse.CourseObject.CourseOrganizerObject.OrganizerName + "</td></tr>" +
                                "<tr><td>Course Title</td><td>:</td><td>" + eCourse.CourseObject.CourseTitle + "</td></tr>" +
                                "<tr><td>Course Date</td><td>:</td><td>" + eCourse.DateFrom.ToString("dd/MM/yyyy") + " to " + eCourse.DateTo.ToString("dd/MM/yyyy") + "</td></tr>" +
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
                FormRandomID randomID = fActivity.RetrieveFormRandomIDByFormTypeFormIDApprovalLevel("TNF", int.Parse(hidFormID.Value), byte.Parse(hidApprovalLevel.Value));
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

                FormApproval fa = fActivity.RetrieveFormApprovalByFormTypeFormIDApprovalLevel("TNF", int.Parse(hidFormID.Value), byte.Parse(hidApprovalLevel.Value));
                fa.ApprovalStatus = "R";
                fa.Save();

                GMSCore.Entity.Form form = fActivity.RetrieveFormByFormTypeFormID("TNF", int.Parse(hidFormID.Value));
                form.FormStatus = "R";
                form.Save();

                GMSCore.Entity.EmployeeCourse eCourse = (new FormActivity()).RetrieveEmployeeCourseByTNFID(int.Parse(hidFormID.Value));
                FormConfig config = FormConfig.RetrieveByKey("TNF");
                string code = "";
                Employee rejectParty = null;
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
            mail.Subject = "[GMS - Training Nomination Form Rejected]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dear " + userRealName + ",</p>\n" +
                        "<p>The training nomination form submitted by " + applicantName + " has been rejected by " + rejectedName + ".\n</p>" +
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
                FormRandomID randomID = fActivity.RetrieveFormRandomIDByFormTypeFormIDApprovalLevel("TNF", formID, approvalLevel);
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

                FormApproval fa = fActivity.RetrieveFormApprovalByFormTypeFormIDApprovalLevel("TNF", int.Parse(hidFormID.Value), byte.Parse(hidApprovalLevel.Value));
                fa.ApprovalStatus = "A";
                fa.Save();

                approvalLevel++;

                FormConfig config = FormConfig.RetrieveByKey("TNF");
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
                        Employee unitHead = (new EmployeeActivity()).RetrieveUnitHeadByDepartment(fActivity.RetrieveEmployeeCourseByTNFID(int.Parse(hidFormID.Value)).EmployeeObject.Department);
                        Employee superior = fActivity.RetrieveEmployeeCourseByTNFID(int.Parse(hidFormID.Value)).EmployeeObject.SuperiorObject;
                        if (unitHead.EmployeeID == superior.EmployeeID && approvalLevel == 2)
                        {
                            approvalLevel++;
                        }
                    }

                    PrepareEmail(approvalLevel, config, formID);
                }

                GMSCore.Entity.Form form = fActivity.RetrieveFormByFormTypeFormID("TNF", int.Parse(hidFormID.Value));
                if (form.FormStatus == "P" || form.FormStatus == "S")
                {
                    if (config.ApprovalLevel < approvalLevel)
                    {
                        form.FormStatus = "A";
                        form.Save();
                        if (!string.IsNullOrEmpty(config.ApprovedRejectedPartyFormPartyObject.EmployeeObject.EmailAddress))
                        {
                            GMSCore.Entity.EmployeeCourse eCourse = (new FormActivity()).RetrieveEmployeeCourseByTNFID(int.Parse(hidFormID.Value));
                            this.SendApprovedEmail(form.SubmittedEmployeeObject.EmailAddress, form.SubmittedEmployeeObject.Name, form.SubmittedEmployeeObject.Name, eCourse);
                            this.SendApprovedEmail(config.ApprovedRejectedPartyFormPartyObject.EmployeeObject.EmailAddress, config.ApprovedRejectedPartyFormPartyObject.EmployeeObject.Name, form.SubmittedEmployeeObject.Name, eCourse);
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
                        form.Save();
                    }
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
            mail.Subject = "[GMS - Training Nomination Form Approved]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Dear " + userRealName + ",</p>\n" +
                        "<p>The training nomination form submitted by " + applicantName + " has been approved.\n</p>" +
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
            ClientScript.RegisterStartupScript(typeof(string), "TNF Report",
                string.Format("jsOpenOperationalReport('SysHR/Reports/ReportViewer.aspx?REPORT={0}&&FORMID={1}&&REPORTID=-1');",
                                    "TNFReport", hidFormID.Value)
                                    , true);
        }
        #endregion
    }
}
