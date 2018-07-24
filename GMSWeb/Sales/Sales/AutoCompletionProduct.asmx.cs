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

namespace GMSWeb.Sales.Sales
{
    /// <summary>
    /// Summary description for AutoCompletionProduct
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AutoCompletionProduct : System.Web.Services.WebService
    {
        public AutoCompletionProduct()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

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

        [WebMethod(EnableSession = true)]
        public string[] GetCompletionListByName(string prefixText, int count)
        {
            short companyId = 1;

            if (Session[GMSCoreBase.SESSIONNAME] != null)
                companyId = ((LogSession)Session[GMSCoreBase.SESSIONNAME]).CompanyId;

            SystemDataActivity sDataActivity = new SystemDataActivity();
            ProductsDataDALC dacl = new ProductsDataDALC();

            DataSet ds = new DataSet();
            try
            {
                dacl.GetProductDetailByNameForAutoComplete(companyId, prefixText, ref ds);
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
                        items.Add(r["ProductCode"].ToString().Trim() + " " + r["ProductName"].ToString().Trim());
                    }
                }
            }

            return items.ToArray();
        }

        [WebMethod(EnableSession = true)]
        public string[] GetTNCListByName(string prefixText, int count)
        {
            QuotationDataDALC dacl = new QuotationDataDALC();

            DataSet ds = new DataSet();
            try
            {
                dacl.GetTNCByNameForAutoComplete(prefixText, ref ds);
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
                        items.Add(r["Name"].ToString().Trim());
                    }
                }
            }

            return items.ToArray();
        }

        [WebMethod(EnableSession = true)]
        public string[] GetPackageListByName(string prefixText, int count)
        {
            short companyId = 1;

            if (Session[GMSCoreBase.SESSIONNAME] != null)
                companyId = ((LogSession)Session[GMSCoreBase.SESSIONNAME]).CompanyId;

            SystemDataActivity sDataActivity = new SystemDataActivity();
            ProductsDataDALC dacl = new ProductsDataDALC();

            DataSet ds = new DataSet();
            try
            {
                dacl.GetProductPackageByNameForAutoComplete(companyId, prefixText, ref ds);
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
                        items.Add(r["PackageID"].ToString().Trim() + " " + r["ProductDescription"].ToString().Trim());
                    }
                }
            }

            return items.ToArray();
        }
    }
}
