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
    public class BankReceiverPayerActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public BankReceiverPayerActivity()
        {
        }
        #endregion

        #region RetrieveBankReceiverPayerByName
        public BankReceiverPayer RetrieveBankReceiverPayerByNameCompanyId(string Name, short companyId)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("BankReceiverPayer.Name"),
                                helper.CleanValue(Name));
            stb.AppendFormat(" and {0} = {1} ", helper.GetFieldName("BankReceiverPayer.CoyID"),
                                helper.CleanValue(companyId));

            return BankReceiverPayer.RetrieveFirst(stb.ToString());
        }
        #endregion 

        //#region UpdateOrganizer
        ///// <summary>
        ///// Function to update Course Organizer
        ///// </summary>
        ///// <param name="parameterToUpdate">Reference to a CompanySpecialData object to be updated</param>
        ///// <param name="session">Logsession of the User</param>
        ///// <returns>ResultType enum of the result</returns>
        //public ResultType UpdateOrganizer(ref CourseOrganizer organizerToUpdate, LogSession session)
        //{
        //    if (organizerToUpdate == null)
        //        return ResultType.NullMainData;

        //    if (session == null)
        //        throw new NullSessionException();

        //    //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
        //    //    throw new SecurityException("edit Billing Term.");

        //    if (!organizerToUpdate.IsValid())
        //        return ResultType.MainDataNotValid;

        //    organizerToUpdate.Save();

        //    return ResultType.Ok;
        //}
        //#endregion

        #region CreateBankReceiverPayer
        /// <summary>
        /// Function to create a new BankReceiverPayer
        /// </summary>
        /// <param name="bankReceiverPayerToCreate">Reference to a BankReceiverPayer object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateBankReceiverPayer(ref BankReceiverPayer bankReceiverPayerToCreate, LogSession session)
        {
            if (bankReceiverPayerToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!bankReceiverPayerToCreate.IsValid())
                return ResultType.MainDataNotValid;

            bankReceiverPayerToCreate.Save();

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

        #region DeleteBankReceiverPayer
        public ResultType DeleteBankReceiverPayer(String Name, LogSession session)
        {
            if (Name == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            BankReceiverPayer BRP = RetrieveBankReceiverPayerByNameCompanyId(Name, session.CompanyId);
            if (BRP == null)
                return ResultType.Error;

            BRP.Delete();
            return ResultType.Ok;
        }
        #endregion
    }
}
