using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using GMSCore.Activity;
using System.Collections.Generic;

namespace GMSWeb.SysHR.Training
{
    /// <summary>
    /// Summary description for AutoCompleteEmployeeName
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AutoCompleteEmployeeName : System.Web.Services.WebService
    {
        [WebMethod]
        public string[] GetCompletionList(string prefixText, int count)
        {
            SystemDataActivity sDataActivity = new SystemDataActivity();
            // fill in employee dropdown list
            IList<GMSCore.Entity.Employee> lstEmployee = null;
            lstEmployee = sDataActivity.RetrieveEmployeeListByEmployeeNameSortByEmployeeName("%" + prefixText + "%");

            List<string> items = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                if (i < lstEmployee.Count && lstEmployee[i] != null)
                    items.Add(lstEmployee[i].Name.Trim() + " - " + lstEmployee[i].CompanyObject.Name.Trim());
            }

            return items.ToArray();
        }
    }
}

