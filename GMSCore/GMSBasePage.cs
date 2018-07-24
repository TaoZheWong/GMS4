using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Globalization;
using System.Threading;

namespace GMSCore
{
    using GMSCore;

    public abstract class GMSBasePage : System.Web.UI.Page
    {
        string appPath = HttpRuntime.AppDomainAppVirtualPath; 

        #region GetSessionInfo
        /// <summary>
        /// Function to get the current page's session information
        /// </summary>
        /// <returns>LogSession object</returns>
        public LogSession GetSessionInfo()
        {
            LogSession sess = (LogSession)Session[GMSCoreBase.SESSIONNAME];

            if (sess == null)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            return sess;
        }
        #endregion

        #region IsSessionActive
        /// <summary>
        /// Function to determine if the current session is still active.
        /// </summary>
        /// <returns>true or false</returns>
        protected bool IsSessionActive()
        {
            LogSession sess = (LogSession)Session[GMSCoreBase.SESSIONNAME];

            return (sess == null);
        }
        #endregion

        protected string SessionTimeOutPage(string moduleID)
        {
            return appPath + "/SessionTimeout.aspx?ModuleID=" + Server.UrlDecode(moduleID); 
        }

        protected string UnauthorizedPage(string moduleID)
        {
            return appPath + "/Unauthorized.aspx?ModuleID=" + Server.UrlDecode(moduleID); 
        }

        protected string OfflinePage(string moduleID)
        {
            return appPath + "/Offline.aspx?ModuleID=" + Server.UrlDecode(moduleID);
        }

        #region InitializeCulture
        protected override void InitializeCulture()
        {
            ///<remarks>
            ///Get the culture from the session if the control is tranferred to a
            ///new page in the same application.
            ///</remarks>
            if (Session[GMSCoreBase.SESSION_CULTURE_CODE] != null && Session[GMSCoreBase.SESSION_UICULTURE_CODE] != null)
            {
                Thread.CurrentThread.CurrentUICulture = (CultureInfo)Session[GMSCoreBase.SESSION_UICULTURE_CODE];
                Thread.CurrentThread.CurrentCulture = (CultureInfo)Session[GMSCoreBase.SESSION_CULTURE_CODE];
            }
            base.InitializeCulture();
        }
        #endregion

        /// <summary>
        /// Function to add a Javascript alert messagebox on postback
        /// </summary>
        /// <param name="strMessage">string value of the message</param>
        protected void JScriptAlertMsg(string strMessage)
        {
            this.JScriptAlertMsg(strMessage, "alert");
        }

        /// <summary>
        /// Function to add a Javascript alert messagebox on postback/load
        /// </summary>
        /// <param name="strMessage">string value of the message</param>
        /// <param name="strMessageKey">string value of the message's key</param>
        protected void JScriptAlertMsg(string strMessage, string strMessageKey)
        {
            Page.RegisterStartupScript(strMessageKey,
                "<script type='text/javascript'>\n  alert(\"" + strMessage.Replace("\"", "'") + "\");\n</script>");
        }

        /// <summary>
        /// Function to add a Javascript confirm messagebox on postback
        /// </summary>
        /// <param name="strMessage">string value of the message</param>
        protected void JScriptConfirmMsg(string strMessage)
        {
            this.JScriptConfirmMsg(strMessage, "alert");
        }

        /// <summary>
        /// Function to add a Javascript confirm messagebox on postback/load
        /// </summary>
        /// <param name="strMessage">string value of the message</param>
        /// <param name="strMessageKey">string value of the message's key</param>
        protected void JScriptConfirmMsg(string strMessage, string strMessageKey)
        {
            Page.RegisterStartupScript(strMessageKey,
                "<script type='text/javascript'>\n  confirm(\"" + strMessage.Replace("\"", "'") + "\");\n</script>");
        }

    }
}
