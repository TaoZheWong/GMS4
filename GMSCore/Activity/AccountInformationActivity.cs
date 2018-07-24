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
    public class AccountInformationActivity : ActivityBase
    {

        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public AccountInformationActivity()
        {
        }
        #endregion

        public ResultType CreateAccountInformation(ref AccountInformation ai, LogSession session)
        {
            if (ai == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!ai.IsValid())
                return ResultType.MainDataNotValid;

            ai.Save();

            return ResultType.Ok;

        }

        #region RetrieveAccountInformationByAccountCode
        public AccountInformation RetrieveAccountInformationByAccountCode(short companyID, string accountcode)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("AccountInformation.AccountCode"),
                               helper.CleanValue(accountcode));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("AccountInformation.CoyID"),
                                helper.CleanValue(companyID));

            return AccountInformation.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region UpdateAccountInformation
        /// <summary>
        /// Function to update Course
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a AccountInformation object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateAccountInformation(ref AccountInformation accountInformationToUpdate, LogSession session)
        {
            if (accountInformationToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!accountInformationToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            accountInformationToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion


        #region RetrieveAccountByCoyID
        public IList<A21Account> RetrieveAccountByCoyID(short coyId)
        {
            if (coyId <= 0)
                return null;

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("A21Account.CoyID"),
                                helper.CleanValue(coyId));

            return A21Account.RetrieveQuery(stb.ToString());
            

           
        }
        #endregion

        #region DeleteAccountForNonA21
        public ResultType DeleteAccountForNonA21(short coyId)
        {
            if (coyId <= 0)
                return ResultType.NullMainData;

            IList<A21Account> lstAcct = this.RetrieveAccountByCoyID(coyId);
            foreach (A21Account acct in lstAcct)
            {
                acct.Delete();
                acct.Resync();
            }

            return ResultType.Ok;
        }
        #endregion  





    }
}