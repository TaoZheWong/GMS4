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
    public class AccountProspectActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public AccountProspectActivity()
        {
        }
        #endregion

        #region RetrieveAccountProspectByAccountCode
        public AccountProspect RetrieveAccountProspectByAccountCode(short companyID, string accountcode)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("AccountProspect.AccountCode"),
                               helper.CleanValue(accountcode));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("AccountProspect.CoyID"),
                                helper.CleanValue(companyID));

            return AccountProspect.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region UpdateAccountProspect
        /// <summary>
        /// Function to update Course
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a AccountProspect object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateAccountProspect(ref AccountProspect accountProspectToUpdate, LogSession session)
        {
            if (accountProspectToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!accountProspectToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            accountProspectToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateA21Account
        public ResultType CreateA21Account(ref A21Account newA21Account, LogSession session)
        {
            if (newA21Account == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!newA21Account.IsValid())
                return ResultType.MainDataNotValid;

            newA21Account.Save();

            return ResultType.Ok;

        }
        #endregion
    }
}
