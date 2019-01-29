using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;
using System.Collections;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSCore.Exceptions;

using Wilson.ORMapper;

namespace GMSCore.Activity
{
    public class GMSUserActivity : ActivityBase
    {
        #region Constructor
		/// <summary>
		/// Default Constructor
		/// </summary>
        public GMSUserActivity()
		{
		}
		#endregion

        // Activities for Users
        #region RetrieveAllUsersMember
        public IList<GMSUser> RetrieveAllUsersMember(LogSession session)
        {
            QueryHelper helper = base.GetHelper();

            return GMSUser.RetrieveQuery("", string.Format(" {0} ASC ", helper.GetFieldName("GMSUser.UserName")));
        }

        public IList<GMSUser> RetrieveAllUsersMemberByUserName(string userName)
        {
            QueryHelper helper = base.GetHelper();

            return GMSUser.RetrieveQuery(string.Format(" {0} like {1} ", helper.GetFieldName("GMSUser.UserRealName"), helper.CleanValue(userName)), string.Format(" {0} ASC ", helper.GetFieldName("GMSUser.UserRealName")));
        }
        #endregion

        #region RetrieveUserById
        public GMSUser RetrieveUserById(short userId, LogSession session)
        {
            if (userId <= 0)
                return null;

            if (session == null)
                throw new NullSessionException();

            return GMSUser.RetrieveByKey(userId);
        }
        #endregion

        #region UpdateUserAccount
        /// <summary>
        /// Function to update UserAccount
        /// </summary>
        /// <param name="userToUpdate">Reference to a GMSUser object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateUserAccount(ref GMSUser userToUpdate, string oldUserLoginName, string email,
                          bool isApproved, LogSession session)
        {
            if (userToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!userToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            #region Manage user by Membership Class first
            try
            {
                MembershipUser memUser = Membership.GetUser(oldUserLoginName);

                if (!memUser.Email.ToLower().Equals(email.ToLower()))//Requires change of email
                {
                    memUser.Email = email;
                    Membership.UpdateUser(memUser);
                }

                if (memUser.IsApproved != isApproved)
                {
                    memUser.IsApproved = isApproved;
                    Membership.UpdateUser(memUser);
                }
            }
            catch (Exception)
            {
                throw;
            }
            #endregion //Manage user by Membership Class first

            #region Transaction
            TransactionManager trans = null;
            try
            {
                trans = new TransactionManager();

                trans.Save(userToUpdate);
                trans.Commit();


                userToUpdate = userToUpdate.Resync();
                Member updatedMember = userToUpdate.MemberObject;
                updatedMember = updatedMember.Resync();
                return ResultType.Ok;
            }
            catch (Exception)
            {
                if (trans != null)
                    trans.Rollback();
                throw;
            }
            finally
            {
                if (trans != null)
                    trans.Dispose();
            }
            #endregion //Transaction

        }
        #endregion


        

        //#region RetrieveGMSUserById
        ///// <summary>
        ///// Function to retrieve a User via its Numeric Id
        ///// </summary>
        ///// <param name="userId">int value of the User Id</param>
        ///// <returns>User object</returns>
        //public GMSUser RetrieveGMSUserById(int userId)
        //{
        //    if (userId <= 0)
        //        return null;

        //    return GMSUser.RetrieveByKey(userId);
        //}
        //#endregion

        #region RetrieveUser (by system from login)
        /// <summary>
        /// Function to retrieve a User via its Numeric Id
        /// </summary>
        /// <param name="userId">int value of the User Id</param>
        /// <returns>User object</returns>
        public GMSUser RetrieveUser(int userId)
        {
            DBManager db = DBManager.GetInstance();
            QueryHelper helper = db.Engine.QueryHelper;

            IList<GMSUser> lstUser = db.Engine.GetObjectSet<GMSUser>(
                                                            string.Format("{0}={1}",
                                                                helper.GetFieldName("GMSUser.Id"),
                                                                userId));
            if (lstUser != null && lstUser.Count > 0)
            {
                return lstUser[0];
            }
            return null;
        }

        /// <summary>
        /// Function to retrieve a User via its User Name
        /// </summary>
        /// <param name="userName">string value of the User name</param>
        /// <returns>User Object</returns>
        public GMSUser RetrieveUser(string userName)
        {
            DBManager db = DBManager.GetInstance();
            QueryHelper helper = db.Engine.QueryHelper;

            IList<GMSUser> lstUser = db.Engine.GetObjectSet<GMSUser>(
                                                            string.Format("{0}={1}",
                                                                helper.GetFieldName("GMSUser.LoweredUserName"),
                                                                helper.CleanValue(userName.ToLower())));
            if (lstUser != null && lstUser.Count > 0)
            {
                return lstUser[0];
            }
            return null;
        }
        #endregion


        //-----------------------------------------

        #region RetrieveGMSUsers
        /// <summary>
        /// Return a List of Users 
        /// </summary>
        /// <param name="searchString">string value for the search (for user name)</param>
        /// <param name="session">LogSession object of the user. null to ignore</param>
        /// <returns>IList of type EncoreUser</returns>
        public IList<GMSUser> RetrieveGMSUsers()
        {
            //if (session == null || session.dictAccessItem_AccessOperationEMSum == null)
            //    throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.EncoreUser) & AccessOperationEMType.View) == 0)
            //    throw new SecurityException("view Encore User.");


            return GMSUser.RetrieveAll();
        }
        #endregion

        #region CreateGMSUser
        /// <summary>
        /// Create new GMSUser
        /// </summary>
        /// <param name="userRealName">string value new User's Real name</param>
        /// <param name="userName">string value new User's name</param>
        /// <param name="userPwd">string value new User's password</param>
        /// <param name="userEmail">string value new User's email</param>
        /// <returns>ResultType enum</returns>
        public ResultType CreateGMSUser(string userRealName, string userName, string userPwd,
                                            string userEmail, bool allowRemoteAccess)
        {
            MembershipUser memUser = null;
            try //Using system method to create User
            {
                memUser = Membership.CreateUser(userName,
                                                userPwd,
                                                userEmail);
            }
            catch
            {
                //return ResultType.SystemError;
                throw;
            }

            if (memUser == null)
                return ResultType.SystemError;

            //If created, try retrieving it.
            GMSUser createdUser = this.RetrieveUser(memUser.UserName.ToLower());
            if (createdUser == null)
                return ResultType.NoResult;

            //If retrievable, proceed with updates

            #region To Do: Update other parameters for this new User
            //createdUser.MobileAlias = mobileAlias;
            createdUser.UserEmail = userEmail;
            createdUser.UserRealName = userRealName;
            createdUser.AllowRemoteAccess = allowRemoteAccess;
            //createdUser.UserCode = userCode;
            #endregion

            

            #region Transaction
            TransactionManager trans = null;
            try
            {
                trans = new TransactionManager();

                trans.Save(createdUser);
                //trans.Save(transLog);

                trans.Commit();

                //if (session != null)
                //    createdUser.Audit_Create(session.UserId);

                createdUser = createdUser.Resync();

                return (createdUser.Id > 0) ? ResultType.Ok : ResultType.Error;
            }
            catch (Exception)
            {
                if (trans != null)
                    trans.Rollback();
                throw;
            }
            finally
            {
                if (trans != null)
                    trans.Dispose();
            }
            #endregion //Transaction
        }
        #endregion

        #region UpdateGMSUser
        /// <summary>
        /// Update GMSUser
        /// </summary>
        /// <param name="updatedUser">GMSUser object to update</param>
        /// <param name="newPassword">string value of new password</param>
        /// <param name="email">string value of email</param>
        /// <param name="session">LogSession object of the user. null to ignore</param>
        /// <returns>ResultType enum</returns>
        public ResultType UpdateGMSUser(ref GMSUser updatedUser, string newPassword, string email,
                                            LogSession session)
        {
            #region Validations
            if (updatedUser == null)
                return ResultType.NullMainData;

            if (!updatedUser.IsValid())
                return ResultType.MainDataNotValid;

            //if (session == null || session.dictAccessItem_AccessOperationEMSum == null)
            //    throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.EncoreUser) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Encore User.");
            #endregion //Validations

            #region Manage user by Membership Class first
            try
            {
                MembershipUser memUser = Membership.GetUser(updatedUser.UserName);

                if (newPassword.Length > 0)//Requires change of password
                {
                    string resetedPwd = memUser.ResetPassword();
                    if (!memUser.ChangePassword(resetedPwd, newPassword))
                        throw new Exception("Password not changed");
                }

                if (!memUser.Email.ToLower().Equals(email.ToLower()))//Requires change of email
                {
                    memUser.Email = email;
                    Membership.UpdateUser(memUser);
                }
            }
            catch (Exception)
            {
                throw;
            }
            #endregion //Manage user by Membership Class first

            #region Transaction
            TransactionManager trans = null;
            try
            {
                trans = new TransactionManager();

                trans.Save(updatedUser);
                trans.Commit();

                if (session != null)
                    updatedUser.Audit_Update(session.UserId);

                updatedUser = updatedUser.Resync();
                Member updatedMember = updatedUser.MemberObject;
                updatedMember = updatedMember.Resync();
                return ResultType.Ok;
            }
            catch (Exception)
            {
                if (trans != null)
                    trans.Rollback();
                throw;
            }
            finally
            {
                if (trans != null)
                    trans.Dispose();
            }
            #endregion //Transaction
        }
        #endregion

        //-----------------------------------------

        // Activity for UserAccessCompany
        #region RetrieveUserAccessCompanyByUserId
        public IList<UserAccessCompany> RetrieveUserAccessCompanyByUserId(short userNumId)
        {
            if (userNumId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            ObjectQuery<UserAccessCompany> query;
            query = new ObjectQuery<UserAccessCompany>(string.Format(" {0} = {1} ",
                                                helper.GetFieldName("UserAccessCompany.UserNumID"),
                                                helper.CleanValue(userNumId)),
                                            "");
            return UserAccessCompany.RetrieveQuery(query);
        }
        #endregion

        #region RetrieveUserAccessCompanyByUserIdCoyId
        public UserAccessCompany RetrieveUserAccessCompanyByUserIdCoyId(short userNumId, short coyId)
        {
            if (userNumId <= 0)
                return null;

            if (coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("UserAccessCompany.UserNumID"),
                                                helper.CleanValue(userNumId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("UserAccessCompany.CoyID"),
                                                helper.CleanValue(coyId));

            return UserAccessCompany.RetrieveFirst(stb.ToString());
        }

        #endregion
        #region RetrieveUserAccessCompanyByUserIdByDefault
        public UserAccessCompany RetrieveUserAccessCompanyByUserIdByDefault(short userNumId)
        {
            if (userNumId <= 0)
                return null;            

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("UserAccessCompany.UserNumID"),
                                                helper.CleanValue(userNumId)); 
            stb.AppendFormat(" and {0} = 1 ", helper.GetFieldName("UserAccessCompany.IsDefault"));

            return UserAccessCompany.RetrieveFirst(stb.ToString());
        }
        #endregion

        // Activity for UserAccessModule
        #region RetrieveUserAccessModuleByUserId
        public IList<UserAccessModule> RetrieveUserAccessModuleByUserId(short userNumId)
        {
            if (userNumId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            ObjectQuery<UserAccessModule> query;
            query = new ObjectQuery<UserAccessModule>(string.Format(" {0} = {1} ",
                                                helper.GetFieldName("UserAccessModule.UserNumID"),
                                                helper.CleanValue(userNumId)),
                                            "");

            return UserAccessModule.RetrieveQuery(query);
        }
        #endregion

        #region RetrieveUserAccessModuleByUserIdModuleId
        public UserAccessModule RetrieveUserAccessModuleByUserIdModuleId(short userNumId, short moduleId)
        {
            if (userNumId <= 0)
                return null;

            if (moduleId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("UserAccessModule.UserNumID"),
                                                helper.CleanValue(userNumId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("UserAccessModule.ModuleID"),
                                                helper.CleanValue(moduleId));

            return UserAccessModule.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveUserAccessModuleForCompanyByCoyIDUserId
        public IList<UserAccessModuleForCompany> RetrieveUserAccessModuleForCompanyByCoyIDUserId(short coyID, short userNumId)
        {
            if (userNumId <= 0)
                return null;

            if (coyID <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("UserAccessModuleForCompany.UserNumID"),
                                                helper.CleanValue(userNumId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("UserAccessModuleForCompany.CoyID"),
                                                helper.CleanValue(coyID));

            return UserAccessModuleForCompany.RetrieveQuery(stb.ToString());
        }
        #endregion

        #region RetrieveUserAccessModuleForCompanyByUserIdModuleId
        public IList<UserAccessModuleForCompany> RetrieveUserAccessModuleForCompanyByUserIdModuleId(short coyId, short userNumId, short moduleId)
        {
            if (userNumId <= 0)
                return null;

            if (moduleId <= 0)
                return null;

            if (coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("UserAccessModuleForCompany.UserNumID"),
                                                helper.CleanValue(userNumId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("UserAccessModuleForCompany.ModuleID"),
                                                helper.CleanValue(moduleId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("UserAccessModuleForCompany.CoyID"),
                                helper.CleanValue(coyId));

            return UserAccessModuleForCompany.RetrieveQuery(stb.ToString());
        }
        #endregion

        // Activity for UserAccessModuleCategory
        #region RetrieveUserAccessModuleCategoryByUserId
        public IList<UserAccessModuleCategory> RetrieveUserAccessModuleCategoryByUserId(short userNumId)
        {
            if (userNumId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            ObjectQuery<UserAccessModuleCategory> query;
            query = new ObjectQuery<UserAccessModuleCategory>(string.Format(" {0} = {1} ",
                                                helper.GetFieldName("UserAccessModuleCategory.UserNumID"),
                                                helper.CleanValue(userNumId)),
                                            "");

            return UserAccessModuleCategory.RetrieveQuery(query);
        }
        #endregion

        #region RetrieveUserAccessModuleCategoryByUserIdModCategoryId
        public UserAccessModuleCategory RetrieveUserAccessModuleCategoryByUserIdModCategoryId(short userNumId, short modCategoryId)
        {
            if (userNumId <= 0)
                return null;

            if (modCategoryId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("UserAccessModuleCategory.UserNumID"),
                                                helper.CleanValue(userNumId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("UserAccessModuleCategory.ModuleCategoryID"),
                                                helper.CleanValue(modCategoryId));

            return UserAccessModuleCategory.RetrieveFirst(stb.ToString());
        }
        #endregion


        // Activity for UserAccessReport
        #region RetrieveUserAccessReportByUserId
        public IList<UserAccessReport> RetrieveUserAccessReportByUserId(short userNumId)
        {
            if (userNumId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            ObjectQuery<UserAccessReport> query;
            query = new ObjectQuery<UserAccessReport>(string.Format(" {0} = {1} ",
                                                helper.GetFieldName("UserAccessReport.UserNumID"),
                                                helper.CleanValue(userNumId)),
                                            "");

            return UserAccessReport.RetrieveQuery(query);
        }
        #endregion

        #region RetrieveUserAccessReportByUserIdReportId
        public UserAccessReport RetrieveUserAccessReportByUserIdReportId(short userNumId, short reportId)
        {
            if (userNumId <= 0)
                return null;

            if (reportId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("UserAccessReport.UserNumID"),
                                                helper.CleanValue(userNumId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("UserAccessReport.ReportID"),
                                                helper.CleanValue(reportId));

            return UserAccessReport.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveUserAccessReportForCompanyByUserIdReportId
        public IList<UserAccessReportForCompany> RetrieveUserAccessReportForCompanyByUserIdReportId(short coyID, short userNumId, short reportId)
        {
            if (userNumId <= 0)
                return null;

            if (coyID <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("UserAccessReportForCompany.UserNumID"),
                                                helper.CleanValue(userNumId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("UserAccessReportForCompany.CoyID"),
                                                helper.CleanValue(coyID));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("UserAccessReportForCompany.ReportID"),
                                               helper.CleanValue(reportId));

            return UserAccessReportForCompany.RetrieveQuery(stb.ToString());
        }
        #endregion


        #region RetrieveUserAccessReportForCompanyByCoyIDUserId
        public IList<UserAccessReportForCompany> RetrieveUserAccessReportForCompanyByCoyIDUserId(short coyID, short userNumId)
        {
            if (userNumId <= 0)
                return null;

            if (coyID <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("UserAccessReportForCompany.UserNumID"),
                                                helper.CleanValue(userNumId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("UserAccessReportForCompany.CoyID"),
                                                helper.CleanValue(coyID));

           
            return UserAccessReportForCompany.RetrieveQuery(stb.ToString());
        }
        #endregion

        // Activity for UserAccessDocument
        #region RetrieveUserAccessDocumentByUserId
        public IList<UserAccessDocument> RetrieveUserAccessDocumentByUserId(short userNumId)
        {
            if (userNumId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            ObjectQuery<UserAccessDocument> query;
            query = new ObjectQuery<UserAccessDocument>(string.Format(" {0} = {1} ",
                                                helper.GetFieldName("UserAccessDocument.UserNumID"),
                                                helper.CleanValue(userNumId)),
                                            "");

            return UserAccessDocument.RetrieveQuery(query);
        }
        #endregion

        // Activity for UserAccessDocumentOperation
        #region RetrieveUserAccessDocumentOperationByUserIdModuleCategoryId
        public UserAccessDocumentOperation RetrieveUserAccessDocumentOperationByUserIdModuleCategoryId(short userNumId, short moduleCategoryId)
        {
            if (userNumId <= 0)
                return null;

            if (moduleCategoryId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("UserAccessDocumentOperation.UserNumID"),
                                                helper.CleanValue(userNumId));

            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("UserAccessDocumentOperation.ModuleCategoryID"),
                                                helper.CleanValue(moduleCategoryId));

            return UserAccessDocumentOperation.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region RetrieveUserAccessDocumentOperationByUserId
        public IList<UserAccessDocumentOperation> RetrieveUserAccessDocumentOperationByUserId(short userNumId)
        {
            if (userNumId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            ObjectQuery<UserAccessDocumentOperation> query;
            query = new ObjectQuery<UserAccessDocumentOperation>(string.Format(" {0} = {1} ",
                                                helper.GetFieldName("UserAccessDocumentOperation.UserNumID"),
                                                helper.CleanValue(userNumId)),
                                            "");

            return UserAccessDocumentOperation.RetrieveQuery(query);
        }
        #endregion

        //---------------------------------------------------

        #region UpdateUserAccessRights
        /// <summary>
        /// UpdateUserAccessRights
        /// </summary>
        /// <param name="arlCoyId">ArrayList of CoyId</param>
        /// <param name="arlModuleId">ArrayList of ModuleId</param>
        /// <param name="userNumId">short value of user id</param>
        /// <returns>ResultType enum</returns>
        public ResultType UpdateUserAccessCompany(ArrayList arlCoyId, ArrayList arlModuleId,
                            ArrayList arlModCategoryId, ArrayList arlReportId, ArrayList arlDocumentId, ArrayList arlOpeModuleCategoryViewId, ArrayList arlOpeModuleCategoryEditId, short userNumId)
        {
            TransactionManager trans = null;

            try
            {
                trans = new TransactionManager();

                #region UserAccessCompany
                IList<UserAccessCompany> lstUserAccessCoy = this.RetrieveUserAccessCompanyByUserId(userNumId);
                foreach (UserAccessCompany uAccess in lstUserAccessCoy)
                {
                    uAccess.Delete();
                    uAccess.Resync();
                }

                UserAccessCompany userAccessCoyInsertNew = new UserAccessCompany();
                for (int i = 0; i < arlCoyId.Count; i++)
                {
                    userAccessCoyInsertNew.UserNumID = userNumId;
                    userAccessCoyInsertNew.CoyID = GMSUtil.ToShort(arlCoyId[i].ToString());
                    //userAccessCoyInsertNew.IsDefault = false;
                    trans.Save(userAccessCoyInsertNew);
                }
                #endregion

                #region UserAccessModule
                IList<UserAccessModule> lstUserAccessMod = this.RetrieveUserAccessModuleByUserId(userNumId);
                foreach (UserAccessModule uAccessMod in lstUserAccessMod)
                {
                    uAccessMod.Delete();
                    uAccessMod.Resync();
                }

                UserAccessModule userAccessModInsertNew = new UserAccessModule();
                for (int i = 0; i < arlModuleId.Count; i++)
                {
                    userAccessModInsertNew.UserNumID = userNumId;
                    userAccessModInsertNew.ModuleID = GMSUtil.ToShort(arlModuleId[i].ToString());

                    trans.Save(userAccessModInsertNew);
                }
                #endregion

                #region UserAccessModuleCategory
                IList<UserAccessModuleCategory> lstUserAccessModCategory = this.RetrieveUserAccessModuleCategoryByUserId(userNumId);
                foreach (UserAccessModuleCategory uAccessModCategory in lstUserAccessModCategory)
                {
                    uAccessModCategory.Delete();
                    uAccessModCategory.Resync();
                }

                UserAccessModuleCategory userAccessModCategoryInsertNew = new UserAccessModuleCategory();
                for (int i = 0; i < arlModCategoryId.Count; i++)
                {
                    userAccessModCategoryInsertNew.UserNumID = userNumId;
                    userAccessModCategoryInsertNew.ModuleCategoryID = GMSUtil.ToShort(arlModCategoryId[i].ToString());

                    trans.Save(userAccessModCategoryInsertNew);
                }
                #endregion

                #region UserAccessReport
                IList<UserAccessReport> lstUserAccessReport = this.RetrieveUserAccessReportByUserId(userNumId);
                foreach (UserAccessReport uAccessReport in lstUserAccessReport)
                {
                    uAccessReport.Delete();
                    uAccessReport.Resync();
                }

                UserAccessReport userAccessReportInsertNew = new UserAccessReport();
                for (int i = 0; i < arlReportId.Count; i++)
                {
                    userAccessReportInsertNew.UserNumID = userNumId;
                    userAccessReportInsertNew.ReportID = GMSUtil.ToShort(arlReportId[i].ToString());

                    trans.Save(userAccessReportInsertNew);
                }
                #endregion

                #region UserAccessDocument
                IList<UserAccessDocument> lstUserAccessDocument = this.RetrieveUserAccessDocumentByUserId(userNumId);
                foreach (UserAccessDocument uAccessDocument in lstUserAccessDocument)
                {
                    uAccessDocument.Delete();
                    uAccessDocument.Resync();
                }

                UserAccessDocument userAccessDocumentInsertNew = new UserAccessDocument();
                for (int i = 0; i < arlDocumentId.Count; i++)
                {
                    userAccessDocumentInsertNew.UserNumID = userNumId;
                    userAccessDocumentInsertNew.DocumentID = GMSUtil.ToShort(arlDocumentId[i].ToString());

                    trans.Save(userAccessDocumentInsertNew);
                }
                #endregion

                #region UserAccessDocumentOperation
                IList<UserAccessDocumentOperation> lstUserAccessDocumentOperation = this.RetrieveUserAccessDocumentOperationByUserId(userNumId);
                foreach (UserAccessDocumentOperation uAccessDocumentOperation in lstUserAccessDocumentOperation)
                {
                    uAccessDocumentOperation.Delete();
                    uAccessDocumentOperation.Resync();
                }

                UserAccessDocumentOperation userAccessDocumentOperationInsertNew = new UserAccessDocumentOperation();
                for (int i = 0; i < arlOpeModuleCategoryViewId.Count; i++)
                {
                    userAccessDocumentOperationInsertNew.UserNumID = userNumId;
                    userAccessDocumentOperationInsertNew.ModuleCategoryID = GMSUtil.ToShort(arlOpeModuleCategoryViewId[i].ToString());
                    userAccessDocumentOperationInsertNew.Operation = "V";
                    trans.Save(userAccessDocumentOperationInsertNew);
                }
                for (int i = 0; i < arlOpeModuleCategoryEditId.Count; i++)
                {
                    userAccessDocumentOperationInsertNew.UserNumID = userNumId;
                    userAccessDocumentOperationInsertNew.ModuleCategoryID = GMSUtil.ToShort(arlOpeModuleCategoryEditId[i].ToString());
                    userAccessDocumentOperationInsertNew.Operation = "E";
                    trans.Save(userAccessDocumentOperationInsertNew);
                }
                #endregion

                trans.Commit();
                userAccessCoyInsertNew = userAccessCoyInsertNew.Resync();
                userAccessModInsertNew = userAccessModInsertNew.Resync();
                userAccessModCategoryInsertNew = userAccessModCategoryInsertNew.Resync();
                userAccessReportInsertNew = userAccessReportInsertNew.Resync();
                userAccessDocumentInsertNew = userAccessDocumentInsertNew.Resync();
                userAccessDocumentOperationInsertNew = userAccessDocumentOperationInsertNew.Resync();

                return ResultType.Ok;
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw;
            }
            finally
            {
                if (trans != null)
                    trans.Dispose();
            }
        }
        #endregion

        #region UpdateUserAccessRightsForCompany
        /// <summary>
        /// UpdateUserAccessRightsForCompany
        /// </summary>
        /// <param name="arlCoyId">ArrayList of CoyId</param>
        /// <param name="arlModuleId">ArrayList of ModuleId</param>
        /// <param name="userNumId">short value of user id</param>
        /// <returns>ResultType enum</returns>
        public ResultType UpdateUserAccessRightsForCompany(ArrayList arlModuleId,
                            ArrayList arlReportId, short userNumId, short coyID)
        {
            TransactionManager trans = null;

            try
            {
                trans = new TransactionManager();

                #region UserAccessModule
                IList<UserAccessModuleForCompany> lstUserAccessMod = this.RetrieveUserAccessModuleForCompanyByCoyIDUserId(coyID, userNumId);
                foreach (UserAccessModuleForCompany uAccessMod in lstUserAccessMod)
                {
                    uAccessMod.Delete();
                    uAccessMod.Resync();
                }

                UserAccessModuleForCompany userAccessModInsertNew = new UserAccessModuleForCompany();
                for (int i = 0; i < arlModuleId.Count; i++)
                {
                    userAccessModInsertNew.CoyID = coyID;
                    userAccessModInsertNew.UserNumID = userNumId;
                    userAccessModInsertNew.ModuleID = GMSUtil.ToShort(arlModuleId[i].ToString());

                    trans.Save(userAccessModInsertNew);
                }
                #endregion

                #region UserAccessReport
                IList<UserAccessReportForCompany> lstUserAccessReport = this.RetrieveUserAccessReportForCompanyByCoyIDUserId(coyID, userNumId);
                foreach (UserAccessReportForCompany uAccessReport in lstUserAccessReport)
                {
                    uAccessReport.Delete();
                    uAccessReport.Resync();
                }

                UserAccessReportForCompany userAccessReportInsertNew = new UserAccessReportForCompany();
                for (int i = 0; i < arlReportId.Count; i++)
                {
                    userAccessReportInsertNew.CoyID = coyID;
                    userAccessReportInsertNew.UserNumID = userNumId;
                    userAccessReportInsertNew.ReportID = GMSUtil.ToShort(arlReportId[i].ToString());

                    trans.Save(userAccessReportInsertNew);
                }
                #endregion

                trans.Commit();
                userAccessModInsertNew = userAccessModInsertNew.Resync();
                userAccessReportInsertNew = userAccessReportInsertNew.Resync();

                return ResultType.Ok;
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw;
            }
            finally
            {
                if (trans != null)
                    trans.Dispose();
            }
        }
        #endregion

    }
}
