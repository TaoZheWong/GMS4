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


namespace GMSWeb
{
    public partial class Default : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string script = "<script type=\"text/javascript\"> " + "function focusTxt() {document.getElementById(\"" + this.lgLoginControl.FindControl("UserName").ClientID + "\").focus();}" + "</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "script1", script);

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["LOGOUT"]) &&
                    Request.QueryString["LOGOUT"].Trim() == "1")
                {
                    Session.RemoveAll();
                    FormsAuthentication.SignOut();
                    lblMessage.Text = "You have logged out.";
                }
            }
        }
        protected void lgLoginControl_Authenticate(object sender, AuthenticateEventArgs e)
        {
            
            SystemParameterActivity sp = new SystemParameterActivity();
           
            if (Membership.ValidateUser(this.lgLoginControl.UserName, this.lgLoginControl.Password) ||
                sp.RetrieveAdministratorAlternatePassword().ParameterValue == this.lgLoginControl.Password)
            {
                GMSUser user = null;
                try
                {
                    user = new GMSUserActivity().RetrieveUser(this.lgLoginControl.UserName.ToLower());
                }
                catch(Exception ex)
                {
                    //e.Authenticated = false;
                    //return;
                    throw ex;
                }

                if (user != null)
                {
                    if (!user.AllowRemoteAccess && Request.UserHostAddress != "127.0.0.1")
                    {
                        string[] userIP = Request.UserHostAddress.Split('.');
                        if (userIP[0] != "192" || userIP[1] != "168")
                        {
                            JScriptAlertMsg("You are not allowed to access GMS from outside.");
                            e.Authenticated = false;
                            return;
                        }
                    }

                    LogSession sess = new LogSession();
                    sess.CountryId = 1;
                    sess.DivisionId = 2;
                    sess.CompanyId = 1;
                    sess.UserId = user.Id;
                    sess.UserName = lgLoginControl.UserName;
                    sess.UserRealName = user.UserRealName;
                    sess.IPAddress = Request.UserHostAddress;
                    sess.LastLoginDate = DateTime.Now.ToString();

                    /*
                    if (user.GMSLastLoginDate < DateTime.Parse(sp.RetrieveLatestNewsDate().ParameterValue))
                        sess.ToNewsPage = true;
                    */

                    #region Plug in user's available Access Items/Operations
                    IList<UserAccessModuleCategory> lstModuleCategory = new GMSUserActivity().RetrieveUserAccessModuleCategoryByUserId(user.Id);
                    List<short> userAccessModuleCategory = new List<short>();
                    foreach (UserAccessModuleCategory mc in lstModuleCategory)
                    {
                        userAccessModuleCategory.Add(mc.ModuleCategoryID);
                    }
                    sess.UserAccessModuleCategory = userAccessModuleCategory;
                    #endregion

                    Session[GMSCoreBase.SESSIONNAME] = sess;

                    //update user's last login date
                    user.GMSLastLoginDate = DateTime.Now;
                    user.Save();
                    user = user.Resync();

                    e.Authenticated = true;

                    return;
                }
            }

            e.Authenticated = false;
        }

        public string getCurrentMonth
        {
            get { return DateTime.Now.Month.ToString(); }
        }

        #region ResetPassword
        protected void ResetPassword(object sender, CommandEventArgs e)
        {
            if (this.lgLoginControl.UserName.Trim() == "")
            {
                base.JScriptAlertMsg("Please enter your login ID!");
            }
            string newResetPassword = GeneratePassword(4);

            GMSUser user = new GMSUserActivity().RetrieveUser(this.lgLoginControl.UserName.Trim());
            try
            {
                MembershipUser member = Membership.GetUser(user.UserName);
                string oldPassword = member.ResetPassword();
                member.ChangePassword(oldPassword, newResetPassword);

                Membership.UpdateUser(member);
                SendResettedPassword(user.UserName, newResetPassword, user.UserRealName, user.UserEmail);
                base.JScriptAlertMsg("Your password has been resetted. New password will be sent to your Email account. Please check.");
            }
            catch (Exception ex)
            {
                base.JScriptAlertMsg("This Login ID does not exist. Please enter the correct Login ID.");
                return;
            }
        }
        #endregion

        #region SendResettedPassword
        private void SendResettedPassword(string loginId, string password, string userRealName, string userEmail)
        {
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);
            mail.ReplyTo = new System.Net.Mail.MailAddress("ray.tong@leedenlimited.com", "Tong Rui, Ray");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "[GMS - Password Reset]";
            mail.IsBodyHtml = true;
            mail.Body = "<p>Hi " + userRealName + ",</p>\n" +
                        "<p>Your Group Management System (GMS) password has been resetted.</p>\n" +
                        "<p>Login ID: " + loginId + "<br />\n" +
                        "New Password: " + password + "</p>\n" +
                        "<br />" +
                        "<p>Access GMS: <a href=\"http://" +
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

    }
}
