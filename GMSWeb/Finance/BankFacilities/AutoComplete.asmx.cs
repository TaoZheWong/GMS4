using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System.Linq;

namespace GMSWeb.Finance.BankFacilities
{
    /// <summary>
    /// Summary description for AutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AutoComplete : System.Web.Services.WebService
    {

        public AutoComplete()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod (EnableSession = true)]
        public string[] GetCompletionList(string prefixText, int count)
        {
            short companyId = 1;

            if (Session[GMSCoreBase.SESSIONNAME] != null)
                companyId = ((LogSession)Session[GMSCoreBase.SESSIONNAME]).CompanyId;

            IList<BankReceiverPayer> lstBRP = new SystemDataActivity().RetrieveAllBankReceiverPayerListByPrefixByCompanyIDSortByName(prefixText + "%", companyId);

            List<string> items = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                if (i< lstBRP.Count && lstBRP[i] != null)
                    if (items.Count < count)
                        items.Add(lstBRP[i].Name);
            }

            if (items.Count < count)
            {
                IList<A21Account> lstAccount = new SystemDataActivity().RetrieveAllCustomerAccountsListByPrefixByCompanyIDSortByAccountName(prefixText + "%", companyId);

                for (int i = 0; i < count-items.Count; i++)
                {
                    if (i<lstAccount.Count && lstAccount[i] != null)
                        if (items.Count < count)
                            items.Add(lstAccount[i].AccountName);
                }
            }

            return items.Distinct().ToArray();
        }

    }
}

