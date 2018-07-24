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
    public class BankActivity : ActivityBase
    {
         #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public BankActivity()
        {
        }
        #endregion

        #region RetrieveBankByBankCode
        public Bank RetrieveBankByBankCode(string bankCode)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Bank.BankCode"),
                                helper.CleanValue(bankCode));

            return Bank.RetrieveFirst(stb.ToString());
        }
        #endregion 

        

        #region UpdateBank
        /// <summary>
        /// Function to update Bank
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a Bank object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateBank(ref Bank bankToUpdate, LogSession session)
        {
            if (bankToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!bankToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            bankToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateBank
        /// <summary>
        /// Function to create a new bank
        /// </summary>
        /// <param name="bankToCreate">Reference to a Bank object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateBank(ref Bank bankToCreate, LogSession session)
        {
            if (bankToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!bankToCreate.IsValid())
                return ResultType.MainDataNotValid;

            bankToCreate.Save();

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

        #region DeleteBank
        public ResultType DeleteBank(String bankCode, LogSession session)
        {
            if (bankCode == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            Bank bank = RetrieveBankByBankCode(bankCode);
            if (bank == null)
                return ResultType.Error;

            bank.Delete();
            return ResultType.Ok;
        }
        #endregion

        #region RetrieveBankUtilisationByTransactionNo
        public BankUtilisation RetrieveBankUtilisationByCoyIDTransactionNo(short companyID, string transactionNo, string transactionType, short bankcoaid)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("BankUtilisation.TransactionNo"),
                                helper.CleanValue(transactionNo));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BankUtilisation.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BankUtilisation.Type"),
                                helper.CleanValue(transactionType));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BankUtilisation.BankCOAID"),
                                helper.CleanValue(bankcoaid));
            stb.AppendFormat(" and {0} is null ", helper.GetFieldName("BankUtilisation.IsOld"));

            return BankUtilisation.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region UpdateBankUtilisation
        /// <summary>
        /// Function to update Bank Utilisation
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a BankUtilisation object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateBankUtilisation(ref BankUtilisation bankUtilisationToUpdate, LogSession session)
        {
            if (bankUtilisationToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!bankUtilisationToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            bankUtilisationToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateBankUtilisation
        /// <summary>
        /// Function to create a new bank utilisation
        /// </summary>
        /// <param name="bankUtilisationToCreate">Reference to a Bank Utilisation object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateBankUtilisation(ref BankUtilisation bankUtilisationToCreate, LogSession session)
        {
            if (bankUtilisationToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!bankUtilisationToCreate.IsValid())
                return ResultType.MainDataNotValid;

            bankUtilisationToCreate.Save();

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

        #region DeleteBankUtilisation
        public ResultType DeleteBankUtilisation(short companyId, String transactionNo, LogSession session, string transactionType, short coaid)
        {
            if (transactionNo == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            BankUtilisation bankUtilisation = RetrieveBankUtilisationByCoyIDTransactionNo(companyId, transactionNo, transactionType, coaid);
            if (bankUtilisation == null)
                return ResultType.Error;

            bankUtilisation.Delete();
            return ResultType.Ok;
        }
        #endregion

        #region RetrieveBankAccountByCOAID
        public BankAccount RetrieveBankAccountByCOAID(short cOAID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("BankAccount.COAID"),
                                helper.CleanValue(cOAID));

            return BankAccount.RetrieveFirst(stb.ToString());
        }
        #endregion 

        #region RetrieveBankAccountByBankCOAByCompanyId
        public BankAccount RetrieveBankAccountByBankCOAByCompanyId(short companyID, string bankCOA)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("BankAccount.BankCOA"),
                                helper.CleanValue(bankCOA));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("BankAccount.CoyID"),
                               helper.CleanValue(companyID));

           return BankAccount.RetrieveFirst(stb.ToString());

        }
        #endregion 

        public Bank RetrieveBankByNumericCode(string bankNumericCode)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Bank.BankNumericCode"),
                                helper.CleanValue(bankNumericCode));         

           return Bank.RetrieveFirst(stb.ToString());
            
        }


        #region UpdateBankAccount
        /// <summary>
        /// Function to update BankAccount
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a BankAccount object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateBankAccount(ref BankAccount bankAccountToUpdate, LogSession session)
        {
            if (bankAccountToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            //if ((session.GetAccessOperationEMSum(AccessItemEMType.BillingTerm) & AccessOperationEMType.Edit) == 0)
            //    throw new SecurityException("edit Billing Term.");

            if (!bankAccountToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            bankAccountToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region CreateBankAccount
        /// <summary>
        /// Function to create a new bank account
        /// </summary>
        /// <param name="bankAccountToCreate">Reference to a BankAccount object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateBankAccount(ref BankAccount bankAccountToCreate, LogSession session)
        {
            if (bankAccountToCreate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!bankAccountToCreate.IsValid())
                return ResultType.MainDataNotValid;

            bankAccountToCreate.Save();

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

        #region DeleteBankAccount
        public ResultType DeleteBankAccount(short cOAID, LogSession session)
        {
            if (cOAID == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            BankAccount bankAccount = RetrieveBankAccountByCOAID(cOAID);
            if (bankAccount == null)
                return ResultType.Error;

            bankAccount.Delete();
            return ResultType.Ok;
        }
        #endregion

        
    }
}
