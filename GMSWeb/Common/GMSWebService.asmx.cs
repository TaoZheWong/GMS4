using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic; 
using System.Xml.Serialization;
using GMSCore.Entity;
using GMSCore; 

namespace GMSWeb.Common
{
    /// <summary>
    /// Summary description for GMSWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class GMSWebService : System.Web.Services.WebService
    {
        [WebMethod]
        [XmlInclude(typeof(List<MRItem>))]
        public List<MRItem> GetMRItems(short coyID, string projectNo)
        {
            List<MRItem> mrl = new List<MRItem>();
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetMRDetailByProjectNo(coyID, projectNo, ref dsTemp);
            if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsTemp.Tables[0].Rows)
                {
                    MRItem mi = new MRItem();
                    mi.mrNo = dr["MRNo"].ToString();
                    mi.MRDate = Convert.ToDateTime(dr["MRDate"].ToString());
                    mi.vendorName = dr["VendorName"].ToString();
                    mi.requestor = dr["Requestor"].ToString();
                    mi.purchaser = dr["Purchaser"].ToString();
                    mi.purchaseCurrency = dr["purchaseCurrency"].ToString();
                    mi.purchasePrice = Convert.ToDecimal(dr["PurchasePrice"]);
                    mi.status = dr["Status"].ToString();
                    mi.remarks = dr["Remarks"].ToString();
                    mrl.Add(mi);
                }
                return mrl;
            }
            else
                return null;
        }

        [WebMethod]
        public DataSet GetAccountAgeing(short companyId, string accountCode, string creditTermValidation, DateTime trnDate)
        {
            DataSet dsTemp = new DataSet();
            (new GMSGeneralDALC()).GetAccountAgeing(companyId, accountCode, creditTermValidation, trnDate, ref dsTemp);
            if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
            {
                return dsTemp;
            }
            else
                return null;
               
        }
    }
}
