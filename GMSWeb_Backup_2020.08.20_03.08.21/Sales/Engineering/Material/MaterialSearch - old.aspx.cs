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


namespace GMSWeb.Sales.Engineering.Material
{
    public partial class MaterialSearchOld : GMSBasePage
    {
        protected static short CoyID = 0;
        protected static short UserID = 0;
        protected static short AccessAll = 0;
        protected static string sessiondata = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Sales");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                140);
            UserAccessModule uAccess1 = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            137);

            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (uAccess1 == null)
            {
                AccessAll = 0;
            }
            else
            {
                AccessAll = 1;
            }

            CoyID = session.CompanyId;
            UserID = session.UserId;

            if (!Page.IsPostBack)
            {
              
            }

         
        }

        [WebMethod]
        public static List<Dictionary<string, string>> CheckUserAccess()
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            Dictionary<string, string> list2 = new Dictionary<string, string>();
            list2.Add("Access", AccessAll.ToString());
            list2.Add("UserID", UserID.ToString());
            list.Add(list2);
            return list;
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetCurrencyList()
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetCurrencyList(ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialList(Object Search)
        {
                IDictionary idict = (IDictionary)Search;
                string ProdCode = "";
                string ItemName = "";
                string ItemCategory = "";
                string SupplierName = "";

                Dictionary<string, string> newDict = new Dictionary<string, string>();
                foreach (object key in idict.Keys)
                {
                    if (key.ToString() == "prodcode")
                        ProdCode = '%' + idict[key].ToString() + '%';
                    else if (key.ToString() == "itemname")
                        ItemName = '%' + idict[key].ToString() + '%';
                    else if (key.ToString() == "itemcategory")
                        ItemCategory = '%' + idict[key].ToString() + '%';
                    else if (key.ToString() == "suppliername")
                        SupplierName = '%' + idict[key].ToString() + '%';
                }

                DataSet dsTemp = new DataSet();
                (new EngineeringDataDALC()).GetMaterialList(CoyID, ProdCode, ItemName, ItemCategory, SupplierName, UserID, ref dsTemp);
                return GMSUtil.ToJson(dsTemp, 0);
        }
        
        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialInformation(string ItemID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetMaterialInformation(CoyID, ItemID, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> SaveMaterialInformation(Object Info)
        {
            IDictionary idict                   = (IDictionary)Info;
            string ProdCode                     = "";
            string ItemName                     = "";
            string ItemCategory                 = "";
            string ItemType                     = "";
            string ItemMaterial                 = "";
            string SupplierCode                 = "";
            string SupplierName                 = "";
            string ItemSize                     = "";
            string ItemBrand                    = "";
            string CurrencyCode                 = "";
            string NewCurrencyCode              = "";
            decimal UnitPrice                   = 0;
            decimal NewUnitPrice                = 0;
            Nullable<DateTime> QuotationDate    = null;
            Nullable<DateTime> NewQuotationDate = null;
            decimal QuotationValidity = 0;
            decimal NewQuotationValidity        = 0;
            string ItemDescription              = "";
            decimal ItemLeadtime                = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();

            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProdCode")
                    ProdCode = idict[key].ToString();
                else if (key.ToString() == "ItemName")
                    ItemName = idict[key].ToString();
                else if (key.ToString() == "ItemCategory")
                    ItemCategory = idict[key].ToString();
                else if (key.ToString() == "ItemType")
                    ItemType = idict[key].ToString();
                else if (key.ToString() == "ItemMaterial")
                    ItemMaterial = idict[key].ToString();
                else if (key.ToString() == "SupplierCode")
                    SupplierCode = idict[key].ToString();
                else if (key.ToString() == "SupplierName")
                    SupplierName = idict[key].ToString();
                else if (key.ToString() == "SupplierCode")
                    SupplierCode = idict[key].ToString();
                else if (key.ToString() == "ItemSize")
                    ItemSize = idict[key].ToString();
                else if (key.ToString() == "ItemBrand")
                    ItemBrand = idict[key].ToString();
                else if (key.ToString() == "ItemBrand")
                    ItemBrand = idict[key].ToString();
                else if (key.ToString() == "CurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "NewCurrencyCode")
                    NewCurrencyCode = idict[key].ToString();
                else if (key.ToString() == "UnitPrice")
                    UnitPrice = GMSUtil.ConvertMoney(idict[key]);
                else if (key.ToString() == "NewUnitPrice")
                    NewUnitPrice = GMSUtil.ConvertMoney(idict[key]);
                else if (key.ToString() == "QuotationDate")
                    QuotationDate = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "NewQuotationDate")
                    NewQuotationDate = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "QuotationValidity")
                    QuotationValidity = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "NewQuotationValidity")
                    NewQuotationValidity = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "ItemDescription")
                    ItemDescription = idict[key].ToString();
                else if (key.ToString() == "ItemLeadtime")
                    ItemLeadtime = GMSUtil.ToDecimal(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).SaveMaterialInformation(CoyID, ProdCode, ItemName, ItemCategory, ItemType, ItemMaterial, SupplierCode, SupplierName, ItemSize,
                ItemBrand, NewCurrencyCode, NewUnitPrice, NewQuotationDate, NewQuotationValidity, ItemDescription, ItemLeadtime, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

         

        [WebMethod]
        public static List<Dictionary<string, string>> EditMaterialInformation(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ItemID = "";
            string ProdCode = "";
            string ItemName = "";
            string ItemCategory = "";
            string ItemType = "";
            string ItemMaterial = "";
            string SupplierCode = "";
            string SupplierName = "";
            string ItemSize = "";
            string ItemBrand = "";
            string CurrencyCode = "";
            string NewCurrencyCode = "";
            decimal UnitPrice = 0;
            decimal NewUnitPrice = 0;
            Nullable<DateTime> QuotationDate = null;
            Nullable<DateTime> NewQuotationDate = null;
            decimal QuotationValidity = 0;
            decimal NewQuotationValidity = 0;
            string ItemDescription = "";
            decimal ItemLeadtime = 0;
            Nullable<Boolean> IsActive = null;

            Dictionary<string, string> newDict = new Dictionary<string, string>();

            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ItemID")
                    ItemID = idict[key].ToString();
                else if (key.ToString() == "ProdCode")
                    ProdCode = idict[key].ToString();
                else if (key.ToString() == "ItemName")
                    ItemName = idict[key].ToString();
                else if (key.ToString() == "ItemCategory")
                    ItemCategory = idict[key].ToString();
                else if (key.ToString() == "ItemType")
                    ItemType = idict[key].ToString();
                else if (key.ToString() == "ItemMaterial")
                    ItemMaterial = idict[key].ToString();
                else if (key.ToString() == "SupplierCode")
                    SupplierCode = idict[key].ToString();
                else if (key.ToString() == "SupplierName")
                    SupplierName = idict[key].ToString();
                else if (key.ToString() == "SupplierCode")
                    SupplierCode = idict[key].ToString();
                else if (key.ToString() == "ItemSize")
                    ItemSize = idict[key].ToString();
                else if (key.ToString() == "ItemBrand")
                    ItemBrand = idict[key].ToString();
                else if (key.ToString() == "ItemBrand")
                    ItemBrand = idict[key].ToString();
                else if (key.ToString() == "CurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "NewCurrencyCode")
                    NewCurrencyCode = idict[key].ToString();
                else if (key.ToString() == "UnitPrice")
                    UnitPrice = GMSUtil.ConvertMoney(idict[key]);
                else if (key.ToString() == "NewUnitPrice")
                    NewUnitPrice = GMSUtil.ConvertMoney(idict[key]);
                else if (key.ToString() == "QuotationDate")
                    QuotationDate = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "NewQuotationDate")
                    NewQuotationDate = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "QuotationValidity")
                    QuotationValidity = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "NewQuotationValidity")
                    NewQuotationValidity = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "ItemDescription")
                    ItemDescription = idict[key].ToString();
                else if (key.ToString() == "ItemLeadtime")
                    ItemLeadtime = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "IsActive")
                    IsActive = Convert.ToBoolean(idict[key].ToString());
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).EditMaterialInformation(CoyID, ItemID, ProdCode, ItemName, ItemCategory, ItemType, ItemMaterial, SupplierCode, SupplierName, ItemSize,
                ItemBrand, CurrencyCode, NewCurrencyCode, UnitPrice, NewUnitPrice, QuotationDate, NewQuotationDate, QuotationValidity, NewQuotationValidity, ItemDescription, ItemLeadtime, IsActive, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

   
        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialPriceList(string ItemID)
        {
                DataSet dsTemp = new DataSet();
                (new EngineeringDataDALC()).GetMaterialPriceList(CoyID, ItemID, ref dsTemp);
                return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> MaterialUploadAttachment(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ItemID = "";
            string FileID = "";
            string FileName = "";
            string FileDisplayName = "";
            Dictionary<string, string> newDict = new Dictionary<string, string>();

            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ItemID")
                    ItemID = idict[key].ToString();
                else if (key.ToString() == "FileID")
                    FileID = idict[key].ToString();
                else if (key.ToString() == "FileName")
                    FileName = idict[key].ToString();
                else if (key.ToString() == "FileDisplayName")
                    FileDisplayName = idict[key].ToString();
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).MaterialUploadAttachment(CoyID, ItemID, FileID, FileName, FileDisplayName, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialAttachment(string ItemID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetMaterialAttachment(CoyID, ItemID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        public void DownloadSelectedFile(string SelectedFileName)
        {
            string folderPath = System.Web.Configuration.WebConfigurationManager.AppSettings["PRJ_DOWNLOAD_PATH"].ToString();
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "Filename=" + SelectedFileName);
            Response.TransmitFile(Server.MapPath(folderPath + "/" + SelectedFileName));
            Response.End();
        }

    }
}
