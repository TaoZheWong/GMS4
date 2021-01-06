using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Mail;
using System.Security.Cryptography;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Admin.Accounts
{
    public partial class Users1 : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //IsCallService();
            Master.setCurrentLink("Admin"); 
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Admin"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            11);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Admin"));

            if (!Page.IsPostBack)
            {
                LoadUsersData();
            }
        }

        //Load Data
        #region LoadUsersData
        private void LoadUsersData()
        {
            LogSession session = base.GetSessionInfo();
            string userName = "%";
            if (!string.IsNullOrEmpty(searchUserName.Text))
                userName = "%" + searchUserName.Text.Trim() + "%";
            IList<GMSUser> lstUsersMember = null;
            try
            {
                lstUsersMember = new GMSUserActivity().RetrieveAllUsersMemberByUserName(userName);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            this.dgUsers.DataSource = lstUsersMember;
            this.dgUsers.DataBind();
        }
        #endregion

        #region dgUsers datagrid PageIndexChanged event handling
        protected void dgUsers_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            // lblSearchSummary.Text = e.NewPageIndex.ToString();

            // lblSearchSummary.Visible = true;
            LoadUsersData();

        }
        #endregion

        #region dgUsers_ItemDataBound
        protected void dgUsers_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                CheckBox chkActive = (CheckBox)e.Item.FindControl("chkActive");

                short sUserId = GMSUtil.ToShort(this.dgUsers.DataKeys[e.Item.ItemIndex]);

                if (sUserId > 0 && chkActive != null)
                {
                    LogSession session = base.GetSessionInfo();
                    GMSUserActivity userActivity = new GMSUserActivity();
                    GMSUser user = null;

                    try
                    {
                        user = userActivity.RetrieveUserById(sUserId, session);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    Member memUser = Member.RetrieveByKey(user.UserId);
                    chkActive.Checked = memUser.IsApproved;
                }

                //Bind Allow Remote Access
                CheckBox chkEditAllowRemoteAccess = (CheckBox)e.Item.FindControl("chkEditAllowRemoteAccess");

                if (sUserId > 0 && chkEditAllowRemoteAccess != null)
                {
                    LogSession session = base.GetSessionInfo();
                    GMSUserActivity userActivity = new GMSUserActivity();
                    GMSUser user = null;

                    try
                    {
                        user = userActivity.RetrieveUserById(sUserId, session);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                    chkEditAllowRemoteAccess.Checked = user.AllowRemoteAccess;
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");

                LinkButton hypResetPassword = (LinkButton)e.Item.FindControl("hypResetPassword");
                if (hypResetPassword != null)
                    hypResetPassword.Attributes.Add("onclick", "return confirm('Do you really want to RESET this user\\'s password?')");


                HyperLink hypEditAccessRights = (HyperLink)e.Item.FindControl("hypEditAccessRights");
                if (hypEditAccessRights != null)
                {
                    short sUserId = GMSUtil.ToShort(this.dgUsers.DataKeys[e.Item.ItemIndex]);

                    hypEditAccessRights.Target = "_self";
                    hypEditAccessRights.NavigateUrl = "~/Admin/Accounts/UserAccessRights.aspx?USERID=" + sUserId;
                }
            }
        }
        #endregion

        #region dgUsers_EditCommand
        protected void dgUsers_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgUsers.EditItemIndex = e.Item.ItemIndex;
            LoadUsersData();
        }
        #endregion

        #region dgUsers_CancelCommand
        protected void dgUsers_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgUsers.EditItemIndex = -1;
            LoadUsersData();
        }
        #endregion

        #region dgUsers_UpdateCommand
        protected void dgUsers_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            TextBox txtEditUserRealName = (TextBox)e.Item.FindControl("txtEditUserRealName");
            TextBox txtEditLoginID = (TextBox)e.Item.FindControl("txtEditLoginID");
            TextBox txtEditEmail = (TextBox)e.Item.FindControl("txtEditEmail");
            CheckBox chkActive = (CheckBox)e.Item.FindControl("chkActive");
            CheckBox chkEditAllowRemoteAccess = (CheckBox)e.Item.FindControl("chkEditAllowRemoteAccess");

            if (txtEditUserRealName != null && txtEditLoginID != null && txtEditEmail != null && chkActive != null &&
                !string.IsNullOrEmpty(txtEditUserRealName.Text) && !string.IsNullOrEmpty(txtEditLoginID.Text) &&
                !string.IsNullOrEmpty(txtEditEmail.Text) && chkEditAllowRemoteAccess != null)
            {
                short sUserId = GMSUtil.ToShort(this.dgUsers.DataKeys[e.Item.ItemIndex]);

                if (sUserId > 0)
                {
                    LogSession session = base.GetSessionInfo();
                    GMSUserActivity userActivity = new GMSUserActivity();
                    GMSUser user = null;

                    try
                    {
                        user = userActivity.RetrieveUserById(sUserId, session);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    string oldUserLoginName = user.UserName;
                    string email = "";

                    user.UserEmail = email = txtEditEmail.Text.Trim();
                    user.UserName = txtEditLoginID.Text.Trim();
                    user.LoweredUserName = txtEditLoginID.Text.Trim().ToLower();
                    user.UserRealName = txtEditUserRealName.Text.Trim();
                    user.MemberObject.IsApproved = chkActive.Checked;
                    user.AllowRemoteAccess = chkEditAllowRemoteAccess.Checked;

                    try
                    {
                        ResultType result = userActivity.UpdateUserAccount(ref user, oldUserLoginName, email, chkActive.Checked, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgUsers.EditItemIndex = -1;
                                LoadUsersData();
                                break;
                            default:
                                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgUsers_ItemCommand
        protected void dgUsers_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Create":
                    string newPassword = GeneratePassword(4);

                    TextBox txtNewUserRealName = (TextBox)e.Item.FindControl("txtNewUserRealName");
                    TextBox txtNewLoginID = (TextBox)e.Item.FindControl("txtNewLoginID");
                    TextBox txtNewEmail = (TextBox)e.Item.FindControl("txtNewEmail");
                    CheckBox chkNewAllowRemoteAccess = (CheckBox)e.Item.FindControl("chkNewAllowRemoteAccess");

                    if (txtNewUserRealName != null && !string.IsNullOrEmpty(txtNewUserRealName.Text) &&
                        txtNewLoginID != null && !string.IsNullOrEmpty(txtNewLoginID.Text) &&
                        txtNewEmail != null && !string.IsNullOrEmpty(txtNewEmail.Text) && chkNewAllowRemoteAccess != null)
                    {
                        try
                        {
                            ResultType result = new GMSUserActivity().CreateGMSUser(txtNewUserRealName.Text.Trim(),
                                                                                            txtNewLoginID.Text.Trim(),
                                                                                            newPassword,
                                                                                            txtNewEmail.Text.Trim(), chkNewAllowRemoteAccess.Checked
                                                                                            );
                            switch (result)
                            {
                                case ResultType.Ok:
                                    SendEmail(txtNewLoginID.Text.Trim(), txtNewEmail.Text.Trim(), newPassword, txtNewUserRealName.Text.Trim());
                                    LoadUsersData();
                                    break;
                                default:
                                    this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                    return;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }
                    }
                    break;

                case "ResetPassword":
                    string newResetPassword = GeneratePassword(4);
                    HtmlInputHidden hidUserID = (HtmlInputHidden)e.Item.FindControl("hidUserID");
                    short sUserId = GMSUtil.ToShort(hidUserID.Value);

                    GMSUser user = new GMSUserActivity().RetrieveUser(sUserId);
                    try
                    {
                        MembershipUser member = Membership.GetUser(user.UserName);
                        string oldPassword = member.ResetPassword();
                        member.ChangePassword(oldPassword, newResetPassword);

                        Membership.UpdateUser(member);
                        SendResettedPassword(user.UserName, newResetPassword, user.UserRealName, user.UserEmail);
                        this.PageMsgPanel.ShowMessage("Password successfully resetted.", MessagePanelControl.MessageEnumType.Alert);
                        LoadUsersData();
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                    break;
            }
        }
        #endregion

        #region GeneratePassword
        private string GeneratePassword(int length)
        {
            Random rGen = new Random();
            string[] strCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

            string strPwd = "";
            int p = 0;
            for (int i = 0; i < length; i++)
            {
                p = rGen.Next(0, 10);
                strPwd += strCharacters[p];
            }
            return strPwd;
        }
        #endregion

        #region SendEmail
        private void SendEmail(string loginId, string userEmail, string password, string userRealName)
        {
            LogSession session = base.GetSessionInfo();
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);

            //mail.ReplyTo = new System.Net.Mail.MailAddress("ray.tong@leedenlimited.com", "Tong Rui, Ray");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[GMS - New Account]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Hi " + userRealName + ",</p>\n" +
                        "<p>Your Group Management System (GMS) account has been created by " + "GMS Administrator" + ".</p>\n" +
                        "<p>Login ID: " + loginId + "<br />\n" +
                        "Password: " + password + "</p>\n" +
                        "<br />" +
                        "<p>Access GMS: <a href=\"https://" +
                        (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + 
                        "/GMS3/\">https://" +
                        (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + 
                        "/GMS3/</a></p>\n" +
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

        #region SendResettedPassword
        private void SendResettedPassword(string loginId, string password, string userRealName, string userEmail)
        {
            LogSession session = base.GetSessionInfo();
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);
            //mail.ReplyTo = new System.Net.Mail.MailAddress("ray.tong@leedenlimited.com", "Tong Rui, Ray");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[GMS - Password Reset]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Hi " + userRealName + ",</p>\n" +
                        "<p>Your Group Management System (GMS) password has been resetted.</p>\n" +
                        "<p>Login ID: " + loginId + "<br />\n" +
                        "New Password: " + password + "</p>\n" +
                        "<br />" +
                        "<p>Access GMS: <a href=\"https://" +
                        (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue +
                        "/GMS3/\">https://" + 
                        (new SystemParameterActivity()).RetrieveSystemParameterByParameterName("Domain").ParameterValue + 
                        "/GMS3/</a></p>\n" +
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

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgUsers.CurrentPageIndex = 0;
            LoadUsersData();
        }
        #endregion
    }
}
