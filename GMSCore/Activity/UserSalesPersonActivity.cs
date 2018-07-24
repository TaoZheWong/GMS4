using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSCore.Exceptions;

using Wilson.ORMapper;

namespace GMSCore.Activity
{
    public class UserSalesPersonActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public UserSalesPersonActivity()
        {
        }
        #endregion

        #region RetrieveUserSalesPersonByUserNumIDCoyIDSalesPersonID
        public SalesPersonUser RetrieveUserSalesPersonByUserNumIDCoyIDSalesPersonID(short userNumId, short companyId, string salesPersonId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("SalesPersonUser.UserID"),
                                helper.CleanValue(userNumId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("SalesPersonUser.CoyID"),
                                helper.CleanValue(companyId));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("SalesPersonUser.SalesPersonID"),
                                helper.CleanValue(salesPersonId));

            return SalesPersonUser.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region CreateUserSalesPerson
        /// <summary>
        /// Function to create a new UserSalesPerson Mapping
        /// </summary>
        /// <param name="userSalesPersonToCreate">Reference to a UserSalesPerson object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateUserSalesPerson(ref SalesPersonUser userSalesPersonToCreate, LogSession session)
        {
            if (userSalesPersonToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!userSalesPersonToCreate.IsValid())
                return ResultType.MainDataNotValid;

            userSalesPersonToCreate.Save();

            return ResultType.Ok;

            //#region Transaction
            //TransactionManager trans = null;
            //try
            //{
            //    trans = new TransactionManager();

            //    trans.Save(parameterToCreate);

            //    trans.Commit();
            //    parameterToCreate.Resync();
            //    return ResultType.Ok;
            //}
            //catch (Exception)
            //{
            //    if (trans != null)
            //        trans.Rollback();
            //    throw;
            //}
            //finally
            //{
            //    if (trans != null)
            //        trans.Dispose();
            //}
            //#endregion //Transaction

        }
        #endregion

        #region DeleteUserSalesPerson
        public ResultType DeleteUserSalesPerson(short userNumId, short companyId, String salesPersonId, LogSession session)
        {
            if (salesPersonId == null)
                return ResultType.NullMainData;

            if (userNumId <= 0)
                return ResultType.NullMainData;

            if (companyId <= 0)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            SalesPersonUser mapping = RetrieveUserSalesPersonByUserNumIDCoyIDSalesPersonID(userNumId, companyId, salesPersonId);
            if (mapping == null)
                return ResultType.Error;

            mapping.Delete();
            return ResultType.Ok;
        }
        #endregion
    }
}
