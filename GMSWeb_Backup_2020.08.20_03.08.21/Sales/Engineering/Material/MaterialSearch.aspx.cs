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
    public partial class MaterialSearch : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            LogSession session = base.GetSessionInfo();
            string currentLink = "";
            currentLink = "Sales";
            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }

            if (CheckUserAccessPage(session.CompanyId, session.UserId) == false)
                Response.Redirect(base.UnauthorizedPage(currentLink));


            hidCoyID.Value = session.CompanyId.ToString();
            hidUserID.Value = session.UserId.ToString();
            hidCurrentLink.Value = currentLink;

            if (!Page.IsPostBack)
            {
            }
        }

        public static bool CheckUserAccessPage(short CompanyId, short UserId)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 140);
            UserAccessModule uAccess1 = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 137);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(CompanyId, loginUserOrAlternateParty,
                                                                            140);
            IList<UserAccessModuleForCompany> uAccessForCompanyList1 = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(CompanyId, loginUserOrAlternateParty,
                                                                            137);
            if (uAccess == null && uAccess1 == null &&  (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0) && (uAccessForCompanyList1 != null && uAccessForCompanyList1.Count == 0))
                return false;
            return true;

        }


        public static short GetloginUserOrAlternateParty(short CompanyId, short UserID)
        {
            short loginUserOrAlternateParty = UserID;
            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(CompanyId, UserID, "Project", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {
                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
            {
                loginUserOrAlternateParty = UserID;
            }
            return loginUserOrAlternateParty;
        }

        [WebMethod]
        public static List<Dictionary<string, string>> CheckUserAccess(short CompanyId, string DocNo, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetUserAccessList(CompanyId, DocNo, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetCurrencyList()
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetCurrencyList(ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialList(short CoyID, string modelno, string description, string itemcategory, string suppliername, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetMaterialList(CoyID, modelno, description, itemcategory, suppliername, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }
        
        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialInformation(short CompanyId, string ItemID, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetMaterialInformation(CompanyId, ItemID, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> SaveMaterialInformation(Object Info)
        {
            IDictionary idict                   = (IDictionary)Info;
            short CompanyId                     = 0;
            string ModelNo                     = "";
            //string ItemName                     = "";
            string ItemCategory                 = "";
            //string ItemType                     = "";
            string ItemMaterial                 = "";
            //string SupplierCode                 = "";
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
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();

            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "ModelNo")
                    ModelNo = idict[key].ToString();
                //else if (key.ToString() == "ItemName")
                //    ItemName = idict[key].ToString();
                else if (key.ToString() == "ItemCategory")
                    ItemCategory = idict[key].ToString();
                //else if (key.ToString() == "ItemType")
                //    ItemType = idict[key].ToString();
                else if (key.ToString() == "ItemMaterial")
                    ItemMaterial = idict[key].ToString();
                //else if (key.ToString() == "SupplierCode")
                //    SupplierCode = idict[key].ToString();
                else if (key.ToString() == "SupplierName")
                    SupplierName = idict[key].ToString();
                //else if (key.ToString() == "SupplierCode")
                //    SupplierCode = idict[key].ToString();
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
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).SaveMaterialInformation(CompanyId, ModelNo, ItemCategory, ItemMaterial, SupplierName, ItemSize,
                ItemBrand, NewCurrencyCode, NewUnitPrice, NewQuotationDate, NewQuotationValidity, ItemDescription, ItemLeadtime, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

         

        [WebMethod]
        public static List<Dictionary<string, string>> EditMaterialInformation(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            short CompanyId = 0;
            string ItemID = "";
            string ModelNo = "";
            string ItemCategory = "";
            string ItemMaterial = "";
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
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();

            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "ItemID")
                    ItemID = idict[key].ToString();
                else if (key.ToString() == "ModelNo")
                    ModelNo = idict[key].ToString();
                else if (key.ToString() == "ItemCategory")
                    ItemCategory = idict[key].ToString();
                else if (key.ToString() == "ItemMaterial")
                    ItemMaterial = idict[key].ToString();
                else if (key.ToString() == "SupplierName")
                    SupplierName = idict[key].ToString();
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
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).EditMaterialInformation(CompanyId, ItemID, ModelNo, ItemCategory, ItemMaterial, SupplierName, ItemSize,
                ItemBrand, CurrencyCode, NewCurrencyCode, UnitPrice, NewUnitPrice, QuotationDate, NewQuotationDate, QuotationValidity, NewQuotationValidity, ItemDescription, ItemLeadtime, IsActive, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

   
        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialPriceList(short CompanyID, string ItemID)
        {
                DataSet dsTemp = new DataSet();
                (new EngineeringDataDALC()).GetMaterialPriceList(CompanyID, ItemID, ref dsTemp);
                return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> MaterialUploadAttachment(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            short CompanyId = 0;
            string ItemID = "";
            string FileID = "";
            string FileName = "";
            string FileDisplayName = "";
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();

            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "ItemID")
                    ItemID = idict[key].ToString();
                else if (key.ToString() == "FileID")
                    FileID = idict[key].ToString();
                else if (key.ToString() == "FileName")
                    FileName = idict[key].ToString();
                else if (key.ToString() == "FileDisplayName")
                    FileDisplayName = idict[key].ToString();
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).MaterialUploadAttachment(CompanyId, ItemID, FileID, FileName, FileDisplayName, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialAttachment(short CompanyId, string ItemID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetMaterialAttachment(CompanyId, ItemID, ref dsTemp);
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

        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialCategory(short CompanyId, string term, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetMaterialCatList(CompanyId, term, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }


        
    }
}
