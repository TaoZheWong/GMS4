using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using GMSCore.Activity;
using System.Collections.Generic;

namespace GMSWeb.Procurement.Records
{
    /// <summary>
    /// Summary description for AutoCompleteVendorName
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    //[System.ComponentModel.ToolboxItem(false)]

    public class AutoCompleteVendorName : System.Web.Services.WebService
    {
        [WebMethod]
        public string[] GetCompletionList(string prefixText, int count)
        {
            SystemDataActivity sDataActivity = new SystemDataActivity();

            IList<GMSCore.Entity.Vendor> lstVendor = null;
            lstVendor = new SystemDataActivity().RetrieveAllVendorListByVendorNameSortByVendorName("%" + prefixText + "%");

            List<string> items = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                if (i < lstVendor.Count && lstVendor[i] != null)
                    items.Add(lstVendor[i].CompanyName.Trim());
            }

            return items.ToArray();
        }
    }
}
