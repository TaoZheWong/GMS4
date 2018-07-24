using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.HR.Commission
{
    /// <summary>
    /// Summary description for AutoCompletionProduct
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AutoCompletionAccount : System.Web.Services.WebService
    {
        public AutoCompletionAccount()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod(EnableSession = true)]
        public string[] GetCompletionListByName(string prefixText, int count)
        {
            short companyId = 1;

            if (Session[GMSCoreBase.SESSIONNAME] != null)
                companyId = ((LogSession)Session[GMSCoreBase.SESSIONNAME]).CompanyId;

            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetAccountByNameForAutoComplete(companyId, prefixText, ref ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            List<string> items = new List<string>(count);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < count && i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow r = ds.Tables[0].Rows[i];
                    if (r != null)
                    {
                        items.Add(r["AccountCode"].ToString().Trim() + " " + r["AccountName"].ToString().Trim());
                    }
                }
            }

            return items.ToArray();
        }
    }
}
