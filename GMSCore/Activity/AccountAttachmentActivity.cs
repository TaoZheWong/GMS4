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
    public class AccountAttachmentActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public AccountAttachmentActivity()
        {
        }
        #endregion

        #region AccountAttachmentActivity
        /// <summary>
        /// Function to create a new AccountAttachment
        /// </summary>
        /// <param name="bankToCreate">Reference to a Bank object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateAccountAttachment(ref AccountAttachment ac, LogSession session)
        {
            if (ac == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!ac.IsValid())
                return ResultType.MainDataNotValid;

            ac.Save();

            return ResultType.Ok;            

        }
        #endregion



        #region RetrieveAllAccountAttachmentByAccountCode
        public IList<AccountAttachment> RetrieveAllAccountAttachmentByAccountCode(short companyID, string accountcode)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("AccountAttachment.AccountCode"),
                                helper.CleanValue(accountcode));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("AccountAttachment.CoyID"),
                                helper.CleanValue(companyID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("AccountAttachment.IsActive"),
                                helper.CleanValue(1));
            
            return AccountAttachment.RetrieveQuery(stb.ToString(), string.Format(" {0} DESC ",
                                                    helper.GetFieldName("AccountAttachment.CreatedDate")));
        }
        #endregion 


        #region UpdateBankAttachment
        /// <summary>
        /// Function to update BankAttachment
        /// </summary>
        /// <param name="parameterToUpdate">Reference to a Bank object to be updated</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType UpdateBankAttachment(ref AccountAttachment accountAttachmentToUpdate, LogSession session)
        {
            if (accountAttachmentToUpdate == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();



            if (!accountAttachmentToUpdate.IsValid())
                return ResultType.MainDataNotValid;

            accountAttachmentToUpdate.Save();

            return ResultType.Ok;
        }
        #endregion

        #region RetrieveAccountAttachmentByDocumentID
        public AccountAttachment RetrieveAccountAttachmentByDocumentID(short companyID, short documentID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("AccountAttachment.DocumentID"),
                                helper.CleanValue(documentID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("AccountAttachment.CoyID"),
                                helper.CleanValue(companyID));

            return AccountAttachment.RetrieveFirst(stb.ToString());
        }
        #endregion 


         /// <summary>
        /// Function to create a new AccountFinananceAttachment
        /// </summary>
        /// <param name="bankToCreate">Reference to a Bank object to be created</param>
        /// <param name="session">Logsession of the User</param>
        /// <returns>ResultType enum of the result</returns>
        public ResultType CreateFinanceAttachment(ref FinanceAttachment ac, LogSession session)
        {
            if (ac == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            if (!ac.IsValid())
                return ResultType.MainDataNotValid;

            ac.Save();

            return ResultType.Ok;

        }

        #region RetrieveFinanceAttachmentByDocumentID
        public FinanceAttachment RetrieveFinanceAttachmentByDocumentID(short documentID)
        {
            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);
            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("FinanceAttachment.Id"),
                                helper.CleanValue(documentID));

            return FinanceAttachment.RetrieveFirst(stb.ToString());
        }
        #endregion
       
    }
}
