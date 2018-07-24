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
    public class AccountCommActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public AccountCommActivity()
        {
        }
        #endregion

        public ResultType CreateAccountContact(ref AccountContacts ac, LogSession session)
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

        #region DeleteAccountComm
        public ResultType DeleteAccountComm(string ID, LogSession session)
        {
            if (ID == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            AccountCommRecord ac = RetrieveAccountCommRecordByID(session.CompanyId, ID);
            if (ac == null)
                return ResultType.Error;

            ac.Delete();
            return ResultType.Ok;
        }
        #endregion

        #region DeleteAccountCommComment
        public ResultType DeleteAccountCommComment(string ID, LogSession session)
        {
            if (ID == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            AccountCommRecordComment ac = RetrieveAccountCommRecordCommentByID(session.CompanyId, ID);
            if (ac == null)
                return ResultType.Error;

            ac.Delete();
            return ResultType.Ok;
        }
        #endregion

        #region RetrieveAccountCommRecordCommentByID
        public AccountCommRecordComment RetrieveAccountCommRecordCommentByID(short companyID, string CommentID)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("AccountCommRecordComment.CommentID"),
                               helper.CleanValue(CommentID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("AccountCommRecordComment.CoyID"),
                                helper.CleanValue(companyID));

            return AccountCommRecordComment.RetrieveFirst(stb.ToString());
        }
        #endregion


        #region RetrieveAccountCommRecordByID
        public AccountCommRecord RetrieveAccountCommRecordByID(short companyID, string CommID)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("AccountCommRecord.CommID"),
                               helper.CleanValue(CommID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("AccountCommRecord.CoyID"),
                                helper.CleanValue(companyID));

            return AccountCommRecord.RetrieveFirst(stb.ToString());
        }
        #endregion



    }
}
