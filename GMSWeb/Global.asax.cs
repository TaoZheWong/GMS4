using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;

namespace GMSWeb
{
    public class Global : System.Web.HttpApplication
    {

        /// <summary>
        /// Constant string to store the transaction log id name
        /// </summary>
        public const string TRANSACTIONLOGID = "TRANSACTIONLOGID";

        /// <summary>
        /// Static boolean to determine if application is running in debug mode
        /// </summary>
        public static readonly bool IS_DEBUG_MODE = (GMSUtil.ToInt(ConfigurationManager.AppSettings["IS_DEBUG"]) == 1);

        /// <summary>
        /// Static value to store the Default Country Id.
        /// </summary>
        public static readonly int DEFAULT_COUNTRY_ID = GMSUtil.ToInt(ConfigurationManager.AppSettings["DEFAULT_COUNTRY_ID"]);


        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }


        protected void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        protected void Session_Start(object sender, EventArgs e)
        {
             //Code that runs when a new session is started
            if (Global.IS_DEBUG_MODE)
            {
                #region Load default user
                LogSession sess = new LogSession();
                try
                {
                    GMSUser user = null;
                    user = new GMSUserActivity().RetrieveUser("Admin".ToLower());
                    sess.CountryId = 1;
                    sess.DivisionId = 1;
                    sess.CompanyId = 1;
                    sess.UserId = user.Id;
                    sess.UserName = user.UserName;
                    sess.UserRealName = user.UserRealName;
                    sess.IPAddress = Request.UserHostAddress;
                    sess.LastLoginDate = DateTime.Now.ToString();
                    IList<UserAccessModuleCategory> lstModuleCategory = new GMSUserActivity().RetrieveUserAccessModuleCategoryByUserId(user.Id);
                    List<short> userAccessModuleCategory = new List<short>();
                    foreach (UserAccessModuleCategory mc in lstModuleCategory)
                    {
                        userAccessModuleCategory.Add(mc.ModuleCategoryID);
                    }
                    sess.UserAccessModuleCategory = userAccessModuleCategory;

                    //#region Plug in user's available Access Items/Operations
                    //sess.dictAccessItem_AccessOperationEMSum = new Dictionary<AccessItemEMType, AccessOperationEMType>();
                    //if (user.EncoreRoleObject.AccessUserRoleList != null && user.EncoreRoleObject.AccessUserRoleList.Count > 0)
                    //{
                    //    foreach (AccessUserRole userRole in user.EncoreRoleObject.AccessUserRoleList)
                    //    {
                    //        sess.dictAccessItem_AccessOperationEMSum.Add((AccessItemEMType)userRole.AccessItemId, (AccessOperationEMType)userRole.AccessOperationEMSum);
                    //    }
                    //}
                    //#endregion

                    Session[GMSCoreBase.SESSIONNAME] = sess;

                    //update user's last login date
                    user.GMSLastLoginDate = DateTime.Now;
                    user.Save();
                    user = user.Resync();
                }
                catch
                {
                }

                Session[GMSCoreBase.SESSIONNAME] = sess;
                #endregion
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }
    }
}