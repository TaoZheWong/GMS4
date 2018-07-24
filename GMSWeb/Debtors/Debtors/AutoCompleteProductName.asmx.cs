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
    public class AutoCompleteProductName : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string[] GetCompletionList(string prefixText, int count)
        {
            short companyId = 1;

            if (Session[GMSCoreBase.SESSIONNAME] != null)
                companyId = ((LogSession)Session[GMSCoreBase.SESSIONNAME]).CompanyId;

            SystemDataActivity sDataActivity = new SystemDataActivity();
            ProductsDataDALC dacl = new ProductsDataDALC();

            DataSet ds = new DataSet();
            try
            {
                dacl.GetProductDetailForAutoComplete(companyId, prefixText, ref ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            List<string> items = new List<string>(count);

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                items.Add(r["ProductCode"].ToString().Trim() + " " + r["ProductName"].ToString().Trim());
            }

            return items.ToArray();
        }
    }
}
