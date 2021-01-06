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
    /// Summary description for AutoCompleteOrganizerName
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AutoCompleteOrganizerName : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] GetCompletionList(string prefixText, int count)
        {
            SystemDataActivity sDataActivity = new SystemDataActivity();
            // fill in employee dropdown list
            IList<GMSCore.Entity.CourseOrganizer> lstOrganizer = null;
            lstOrganizer = sDataActivity.RetrieveAllOrganizerListByOrganizerNameSortByOrganizerName("%" + prefixText + "%");

            List<string> items = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                if (i < lstOrganizer.Count && lstOrganizer[i] != null)
                    items.Add(lstOrganizer[i].OrganizerName.Trim());
            }

            return items.ToArray();
        }
    }
}
