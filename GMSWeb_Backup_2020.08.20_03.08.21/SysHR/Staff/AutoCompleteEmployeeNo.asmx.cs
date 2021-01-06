using GMSCore.Activity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

namespace GMSWeb.SysHR.Staff
{
    /// <summary>
    /// Summary description for AutoCompleteEmployeeNo
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AutoCompleteEmployeeNo : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] GetCompletionListByEmployeeNo(string prefixText, int count)
        {
            SystemDataActivity sDataActivity = new SystemDataActivity();
            // fill in employee dropdown list
            IList<GMSCore.Entity.Employee> lstEmployee = null;
            lstEmployee = sDataActivity.RetrieveEmployeeListByEmployeeNo("%" + prefixText + "%");

            List<string> items = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                if (i < lstEmployee.Count && lstEmployee[i] != null)
                    items.Add(lstEmployee[i].EmployeeNo.Trim() + " - " + lstEmployee[i].Name.Trim() + " - " + lstEmployee[i].CompanyObject.Name.Trim());
            }

            return items.ToArray();
        }
    }
}
