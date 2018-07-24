using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using GMSCore.Activity;
using System.Collections.Generic;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Debtors.Debtors
{
    /// <summary>
    /// Summary description for AutoCompleteCourseTtitle
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AutoCompleteAccountCode : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string[] GetCompletionList(string prefixText, int count)
        {
            short companyId = 1;

            if (Session[GMSCoreBase.SESSIONNAME] != null)
                companyId = ((LogSession)Session[GMSCoreBase.SESSIONNAME]).CompanyId;

            SystemDataActivity sDataActivity = new SystemDataActivity();
            // fill in employee dropdown list
            IList<GMSCore.Entity.A21Account> lstA21Acct = null;
            lstA21Acct = sDataActivity.RetrieveAllCustomerAccountsListByPrefixByCompanyIDSortByAccountName("%" + prefixText + "%", companyId);

            List<string> items = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                if (i < lstA21Acct.Count && lstA21Acct[i] != null)
                    items.Add(lstA21Acct[i].AccountCode.Trim());
            }

            return items.ToArray();
        }
    }
}
