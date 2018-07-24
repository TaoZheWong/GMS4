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
    public class AccountContactActivity : ActivityBase
    {
        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public AccountContactActivity()
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


        #region RetrieveAccountContactByID
        public AccountContacts RetrieveAccountContactByID(short companyID, string ContactID)
        {

            QueryHelper helper = base.GetHelper();
            StringBuilder stb = new StringBuilder(200);

            stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("AccountContacts.ContactID"),
                               helper.CleanValue(ContactID));
            stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("AccountContacts.CoyID"),
                                helper.CleanValue(companyID));

            return AccountContacts.RetrieveFirst(stb.ToString());
        }
        #endregion

        #region DeleteAccountContact
        public ResultType DeleteAccountContact(string ID, LogSession session)
        {
            if (ID == null)
                return ResultType.NullMainData;

            if (session == null)
                throw new NullSessionException();

            AccountContacts ac = RetrieveAccountContactByID(session.CompanyId, ID);
            if (ac == null)
                return ResultType.Error;

            ac.Delete();
            return ResultType.Ok;
        }
        #endregion
    }
}
