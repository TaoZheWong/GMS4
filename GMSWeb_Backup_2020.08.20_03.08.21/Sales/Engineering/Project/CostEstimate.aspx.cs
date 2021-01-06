using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;
using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Text;
using System.Web.Services;
using AjaxControlToolkit;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace GMSWeb.Sales.Engineering.Project
{
    public partial class CostEstimate : GMSBasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId, 132);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {

            }
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetCostEstimateList(short CompanyId, string CEID, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetCostEstimateList(CompanyId, CEID, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> InsertCEDetails(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ProjectNo = "";
            string CEID = "";
            string CEDetailID = "";
            string ItemName = "";
            string ItemBrand = "";
            string ItemMaterial = "";
            string SupplierCode = "";
            string SupplierName = "";
            string CurrencyCode = "";
            decimal CurrencyRate = 0;
            decimal QuotedPrice = 0;
            string UOM = "";
            decimal Quantity = 0;
            decimal TotalAmount = 0;
            string Category = "";
            string Remarks = "";
            short CompanyId = 0;
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    ProjectNo = idict[key].ToString();
                else if (key.ToString() == "CEDetailID")
                    CEDetailID = idict[key].ToString();
                else if (key.ToString() == "CEID")
                    CEID = idict[key].ToString();
                else if (key.ToString() == "ItemName")
                    ItemName = idict[key].ToString();
                else if (key.ToString() == "ItemBrand")
                    ItemBrand = idict[key].ToString();
                else if (key.ToString() == "ItemMaterial")
                    ItemMaterial = idict[key].ToString();
                else if (key.ToString() == "SupplierCode")
                    SupplierCode = idict[key].ToString();
                else if (key.ToString() == "SupplierName")
                    SupplierName = idict[key].ToString();
                else if (key.ToString() == "UOM")
                    UOM = idict[key].ToString();
                else if (key.ToString() == "Quantity")
                    Quantity = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "CurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "CurrencyRate")
                    CurrencyRate = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "QuotedPrice")
                    QuotedPrice = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "TotalAmount")
                    TotalAmount = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "Category")
                    Category = idict[key].ToString();
                else if (key.ToString() == "Remarks")
                    Remarks = idict[key].ToString();
                else if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            if (CEDetailID.IndexOf("New")== -1)
                (new EngineeringDataDALC()).UpdateCEDetails(CompanyId, ProjectNo, CEID, CEDetailID, ItemName, ItemBrand, ItemMaterial, SupplierCode, SupplierName, UOM, Quantity, CurrencyCode, CurrencyRate, QuotedPrice, TotalAmount, Category, Remarks, UserID, ref dsTemp);
            else
                (new EngineeringDataDALC()).InsertCEDetails(CompanyId, ProjectNo, CEID, ItemName, ItemBrand, ItemMaterial, SupplierCode, SupplierName, UOM, Quantity, CurrencyCode, CurrencyRate, QuotedPrice, TotalAmount, Category, Remarks, UserID, ref dsTemp);


            return GMSUtil.ToJson(dsTemp, 0);
        }



        [WebMethod]
        public static List<Dictionary<string, string>> DeleteCEDetail(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ProjectNo = "";
            string CEID = "";
            string CEDetailID = "";
            short CompanyId = 0;
            short UserID = 0;

            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    ProjectNo = idict[key].ToString();
                else if (key.ToString() == "CEID")
                    CEID = idict[key].ToString();
                else if (key.ToString() == "CEDetailID")
                    CEDetailID = idict[key].ToString();
                else if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);

            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).DeleteCostEstimateDetail(CompanyId, ProjectNo, CEID, CEDetailID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);


        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetUOMList(short CompanyId)
        {
            DataSet dsTemp = new DataSet();
            (new QuotationDataDALC()).GetAllUOMByCoyIDSelect(CompanyId, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetCurrencyRate(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ProjectNo = "";
            string CurrencyCode = "";
            string type = "PrjNo";
            short CompanyId = 0;
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    ProjectNo = idict[key].ToString();
                else if (key.ToString() == "CurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetCurrencyRate(ProjectNo, CurrencyCode, type, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }
        
    }
}
