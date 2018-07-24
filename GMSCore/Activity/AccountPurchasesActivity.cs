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
    public class AccountPurchasesActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public AccountPurchasesActivity()
        {
        }
        #endregion

        #region AccountPurchasesActivity
        /// <summary>
        /// Function to create a new AccountAttachment
        /// </summary>
        /// <param name="bankToCreate">Reference to a Bank object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateAccountPurchase(ref AccountPurchases ap, LogSession session)
        {
            if (ap == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!ap.IsValid())
                return ResultType.MainDataNotValid;

            ap.Save();

            return ResultType.Ok;            

        }
        #endregion

        #region RetrieveAccountPurchaseByID
        public AccountPurchases RetrieveAccountPurchaseByID(short companyID, string PurchaseID)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("AccountPurchases.PurchaseID"),
                               helper.CleanValue(PurchaseID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("AccountPurchases.CoyID"),
                                helper.CleanValue(companyID));

            return AccountPurchases.RetrieveFirst(stb.ToString());
        }
        #endregion


        #region DeleteAccountPurchases
        public ResultType DeleteAccountPurchases(string PurchaseID, LogSession session)
        {
            if (PurchaseID == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            AccountPurchases ap = RetrieveAccountPurchaseByID(session.CompanyId,PurchaseID);
            if (ap == null)
                return ResultType.Error;

            ap.Delete();
            return ResultType.Ok;
        }
        #endregion

        #region UpdateAccountPurchases
        /// <summary>
        /// Function to update AccountPurchases
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a AccountPurchases object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateAccountPurchases(ref AccountPurchases accountPurchasesToUpdate, LogSession session)
        {
            if (accountPurchasesToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!accountPurchasesToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            accountPurchasesToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion
    }
}
