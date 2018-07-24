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

namespace GMSWeb.Sales.Engineering.CostEstimate
{
    public partial class AddEditCostEstimate : GMSBasePage
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
            UserAccessModule uAccess1 = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 134);
            UserAccessModule uAccess2 = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 136);
            UserAccessModule uAccess3 = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 139);
            IList<UserAccessModuleForCompany> uAccessForCompanyList1 = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(CompanyId, loginUserOrAlternateParty,
                                                                            134);
            IList<UserAccessModuleForCompany> uAccessForCompanyList2 = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(CompanyId, loginUserOrAlternateParty,
                                                                            136);
            IList<UserAccessModuleForCompany> uAccessForCompanyList3 = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(CompanyId, loginUserOrAlternateParty,
                                                                            139);
            if (uAccess1 == null && uAccess2 == null && uAccess3 == null &&  (uAccessForCompanyList1 != null && uAccessForCompanyList1.Count == 0) && (uAccessForCompanyList2 != null && uAccessForCompanyList2.Count == 0) && (uAccessForCompanyList3 != null && uAccessForCompanyList3.Count == 0))
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
        public static List<Dictionary<string, string>> GetAccountList(short CompanyId, string account)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetAccountList(CompanyId, account, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetAccountAddressList(short CompanyId, string account)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetAccountAddressList(CompanyId, account, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetAccountBillingAddressList(short CompanyId, string account)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetAccountBillingAddressList(CompanyId, account, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetCostEstimateInfo(short CompanyId, string CEID, int Revision, short UserID)
        {
           
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetCostEstimateInfo(CompanyId, CEID, Revision, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
           
        }

        [WebMethod]
        public static List<Dictionary<string, string>> SaveCostEstimateHeaderInfo(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            short CompanyId = 0;
            string AccountCode = "";
            string AccountName = "";
            string SalesPersonID = "";
            string EngineerID = "";
            bool IsBillable = false;
            bool IsProgressiveClaim = false;
            string CurrencyCode = "";
            decimal TotalAmtQuoted = 0;
            Nullable<DateTime> ContractDateFrom = null;
            Nullable<DateTime> ContractDateTo = null;
            Nullable<DateTime> CommencementDate = null;
            string CustomerPIC = "";
            string OfficePhone = "";
            string Fax = "";
            string BillingAddress = "";
            string OnsiteLocation = "";
            string Description = "";
            string Remarks = "";
            int Revision = 0;
            short UserID =0;
            try {
                Dictionary<string, string> newDict = new Dictionary<string, string>();
                foreach (object key in idict.Keys)
                {
                    if (key.ToString() == "CompanyId")
                        CompanyId = GMSUtil.ToShort(idict[key]);
                    else if (key.ToString() == "AccountCode")
                        AccountCode = idict[key].ToString();
                    else if (key.ToString() == "AccountName")
                        AccountName = idict[key].ToString();
                    else if (key.ToString() == "SalesPersonID")
                        SalesPersonID = idict[key].ToString();
                    else if (key.ToString() == "EngineerID")
                        EngineerID = idict[key].ToString();
                    else if (key.ToString() == "IsBillable" && idict[key].ToString() == "False")
                        IsBillable = false;
                    else if (key.ToString() == "IsBillable" && idict[key].ToString() == "True")
                        IsBillable = true;
                    else if (key.ToString() == "IsProgressiveClaim" && idict[key].ToString() == "False")
                        IsProgressiveClaim = false;
                    else if (key.ToString() == "IsProgressiveClaim" && idict[key].ToString() == "True")
                        IsProgressiveClaim = true;
                    else if (key.ToString() == "CurrencyCode")
                        CurrencyCode = idict[key].ToString();
                    else if (key.ToString() == "TotalAmtQuoted")
                        TotalAmtQuoted = GMSUtil.ToDecimal(idict[key]);
                    else if (key.ToString() == "ContractDateFrom")
                        ContractDateFrom = GMSUtil.ToDate(idict[key]);
                    else if (key.ToString() == "ContractDateTo")
                        ContractDateTo = GMSUtil.ToDate(idict[key]);
                    else if (key.ToString() == "CommencementDate")
                        CommencementDate = GMSUtil.ToDate(idict[key]);
                    else if (key.ToString() == "CustomerPIC")
                        CustomerPIC = idict[key].ToString();
                    else if (key.ToString() == "OfficePhone")
                        OfficePhone = idict[key].ToString();
                    else if (key.ToString() == "Fax")
                        Fax = idict[key].ToString();
                    else if (key.ToString() == "BillingAddress")
                        BillingAddress = idict[key].ToString();
                    else if (key.ToString() == "OnsiteLocation")
                        OnsiteLocation = idict[key].ToString();
                    else if (key.ToString() == "Description")
                        Description = idict[key].ToString();
                    else if (key.ToString() == "Remarks")
                        Remarks = idict[key].ToString();
                    else if (key.ToString() == "Revision")
                        Revision = GMSUtil.ToInt(idict[key]);
                    else if (key.ToString() == "UserID")
                        UserID = GMSUtil.ToShort(idict[key]);

                }
                DataSet dsTemp = new DataSet();
                (new EngineeringDataDALC()).InsertCostEstimateHeaderInfo(CompanyId, AccountCode, AccountName, SalesPersonID, EngineerID, IsBillable, IsProgressiveClaim,
                    CurrencyCode, TotalAmtQuoted, ContractDateFrom, ContractDateTo, CommencementDate, CustomerPIC, OfficePhone, Fax, BillingAddress, OnsiteLocation, Description,
                    Remarks, Revision, UserID, ref dsTemp);
                return GMSUtil.ToJson(dsTemp, 0);
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
        }

        [WebMethod]
        public static List<Dictionary<string, string>> CancelCostEstimate(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            short CompanyId = 0;
            string CancelPurpose = "";
            string Type = "";
            string CEID = "";
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "CancelPurpose")
                    CancelPurpose = idict[key].ToString();
                else if (key.ToString() == "CEID")
                    CEID = idict[key].ToString();
                else if (key.ToString() == "Type")
                    Type = idict[key].ToString();
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).CancelCostEstimate(CompanyId, CEID, CancelPurpose,Type, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> ApproveCostEstimate(short CompanyId, string CEID, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).ApproveCostEstimate(CompanyId, CEID, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> SubmitCostEstimate(short CompanyId, string CEID, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).SubmitCostEstimate(CompanyId, CEID, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetCostEstimateList(string CEID, int Revision, short CompanyId, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetCostEstimateListByCEID(CompanyId, CEID, Revision, UserID, ref dsTemp);
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
        public static List<Dictionary<string, string>> GetCurrencyRate(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string CEID = "";
            string CurrencyCode = "";
            string Type = "";

            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "CEID")
                    CEID = idict[key].ToString();
                else if (key.ToString() == "CurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "Type")
                    Type = idict[key].ToString();
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetCurrencyRate(CEID, CurrencyCode, Type, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> EditCostEstimateHeaderInfo(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            short CompanyId = 0;
            string CEID = "";
            string AccountCode = "";
            string AccountName = "";
            string CEStatusID = "";
            string SalesPersonID = "";
            string EngineerID = "";
            bool IsBillable = false;
            bool IsProgressiveClaim = false;
            string CurrencyCode = "";
            decimal GrandTotal = 0;
            decimal TotalAmtQuoted = 0;
            Nullable<DateTime> ContractDateFrom = null;
            Nullable<DateTime> ContractDateTo = null;
            Nullable<DateTime> CommencementDate = null;
            string CustomerPIC = "";
            string OfficePhone = "";
            string Fax = "";
            string BillingAddress = "";
            string OnsiteLocation = "";
            string Description = "";
            string Remarks = "";
            int Revision = 0;
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "CEID")
                    CEID = idict[key].ToString();
                else if (key.ToString() == "AccountCode")
                    AccountCode = idict[key].ToString();
                else if (key.ToString() == "AccountName")
                    AccountName = idict[key].ToString();
                else if (key.ToString() == "StatusID")
                    CEStatusID = idict[key].ToString();
                else if (key.ToString() == "SalesPersonID")
                    SalesPersonID = idict[key].ToString();
                else if (key.ToString() == "EngineerID")
                    EngineerID = idict[key].ToString();
                else if (key.ToString() == "IsBillable" && idict[key].ToString() == "False")
                    IsBillable = false;
                else if (key.ToString() == "IsBillable" && idict[key].ToString() == "True")
                    IsBillable = true;
                else if (key.ToString() == "IsProgressiveClaim" && idict[key].ToString() == "False")
                    IsProgressiveClaim = false;
                else if (key.ToString() == "IsProgressiveClaim" && idict[key].ToString() == "True")
                    IsProgressiveClaim = true;
                else if (key.ToString() == "CurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "TotalAmtQuoted")
                    TotalAmtQuoted = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "GrandTotal")
                    GrandTotal = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "ContractDateFrom")
                    ContractDateFrom = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "ContractDateTo")
                    ContractDateTo = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "CommencementDate")
                    CommencementDate = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "CustomerPIC")
                    CustomerPIC = idict[key].ToString();
                else if (key.ToString() == "OfficePhone")
                    OfficePhone = idict[key].ToString();
                else if (key.ToString() == "Fax")
                    Fax = idict[key].ToString();
                else if (key.ToString() == "BillingAddress")
                    BillingAddress = idict[key].ToString();
                else if (key.ToString() == "OnsiteLocation")
                    OnsiteLocation = idict[key].ToString();
                else if (key.ToString() == "Description")
                    Description = idict[key].ToString();
                else if (key.ToString() == "Remarks")
                    Remarks = idict[key].ToString();
                else if (key.ToString() == "Revision")
                    Revision = GMSUtil.ToInt(idict[key]);
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).UpdateCostEstimateHeaderInfo(CompanyId, CEID, AccountCode, AccountName, CEStatusID, SalesPersonID, EngineerID, IsBillable, IsProgressiveClaim, 
                CurrencyCode, TotalAmtQuoted, ContractDateFrom, ContractDateTo, CommencementDate, CustomerPIC, OfficePhone, Fax, BillingAddress, OnsiteLocation, Description,
                Remarks, Revision, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetEngineerInfo(short CompanyId, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetEngineerInfo(CompanyId, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetEngineerList(short CompanyId, string engineer)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetEngineerList(CompanyId, engineer, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetSalesPersonList(short CompanyId, string salesperson)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetSalesPersonList(CompanyId, salesperson, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialItemList(short CompanyId, string item)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetMaterialItemList(CompanyId, item, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> SaveCostEstimateItem(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            short CompanyId = 0;
            string CEID = "";
            string CurrencyCode = "";
            string ItemName = "";
            Byte chkIsOthers = 0;
            string ItemBrand = "";
            string ItemMaterial = "";
            string SupplierName = "";
            string UOM = "";
            decimal Quantity = 0;
            decimal CurrencyRate = 0;
            decimal QuotedPrice = 0;
            decimal MarkUpPrice = 0;
            decimal TotalAmount = 0;
            string Category = "";
            string Remarks = "";
            string ItemSize = "";
            short UserID = 0;

            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "CEID")
                    CEID = idict[key].ToString();
                else if (key.ToString() == "CurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "chkIsOthers")
                    chkIsOthers = GMSUtil.ToByte(idict[key]);
                else if (key.ToString() == "ItemDescription")
                    ItemName = idict[key].ToString();
                else if (key.ToString() == "ItemBrand")
                    ItemBrand = idict[key].ToString();
                else if (key.ToString() == "ItemMaterial")
                    ItemMaterial = idict[key].ToString();
                else if (key.ToString() == "SupplierName")
                    SupplierName = idict[key].ToString();
                else if (key.ToString() == "UOM")
                    UOM = idict[key].ToString();
                else if (key.ToString() == "Quantity")
                    Quantity = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "CurrencyRate")
                    CurrencyRate = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "QuotedPrice")
                    QuotedPrice = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "MarkUpPrice")
                    MarkUpPrice = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "TotalAmount")
                    TotalAmount = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "ItemCategory")
                    Category = idict[key].ToString();
                else if (key.ToString() == "ItemSize")
                    ItemSize = idict[key].ToString();
                else if (key.ToString() == "Remarks")
                    Remarks = idict[key].ToString();
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).SaveCostEstimateItem(CompanyId, CEID, ItemName, chkIsOthers, ItemBrand, ItemMaterial, SupplierName, UOM, Quantity, CurrencyCode, CurrencyRate, QuotedPrice, MarkUpPrice, TotalAmount, Category, Remarks, ItemSize, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> EditCostEstimateItem(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            short CompanyId = 0;
            string CEID = "";
            string CEDetailID = "";
            string CurrencyCode = "";
            string ItemName = "";
            Byte chkIsOthers = 0;
            string ItemBrand = "";
            string ItemMaterial = "";
            string SupplierName = "";
            string UOM = "";
            decimal Quantity = 0;
            decimal CurrencyRate = 0;
            decimal QuotedPrice = 0;
            decimal MarkUpPrice = 0;
            decimal TotalAmount = 0;
            string Category = "";
            string Remarks = "";
            string ItemSize = "";
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "CEID")
                    CEID = idict[key].ToString();
                else if (key.ToString() == "CEDetailID")
                    CEDetailID = idict[key].ToString();
                else if (key.ToString() == "CurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "ItemDescription")
                    ItemName = idict[key].ToString();
                else if (key.ToString() == "chkIsOthers")
                    chkIsOthers = GMSUtil.ToByte(idict[key]);
                else if (key.ToString() == "ItemBrand")
                    ItemBrand = idict[key].ToString();
                else if (key.ToString() == "ItemMaterial")
                    ItemMaterial = idict[key].ToString();
                else if (key.ToString() == "SupplierName")
                    SupplierName = idict[key].ToString();
                else if (key.ToString() == "UOM")
                    UOM = idict[key].ToString();
                else if (key.ToString() == "Quantity")
                    Quantity = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "CurrencyRate")
                    CurrencyRate = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "QuotedPrice")
                    QuotedPrice = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "MarkUpPrice")
                    MarkUpPrice = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "TotalAmount")
                    TotalAmount = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "ItemCategory")
                    Category = idict[key].ToString();
                else if (key.ToString() == "Remarks")
                    Remarks = idict[key].ToString();
                else if (key.ToString() == "ItemSize")
                    ItemSize = idict[key].ToString();
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).EditCostEstimateItem(CompanyId, CEID, CEDetailID, ItemName, chkIsOthers, ItemBrand, ItemMaterial, SupplierName, UOM, Quantity, CurrencyCode, CurrencyRate, QuotedPrice, MarkUpPrice, TotalAmount, Category, Remarks, ItemSize, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetCostEstimateItemDetail(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            short CompanyId = 0;
            string CEID = "";
            string CEDetailID = "";
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "CEID")
                    CEID = idict[key].ToString();
                else if (key.ToString() == "CEDetailID")
                    CEDetailID = idict[key].ToString();
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetCostEstimateItemDetail(CompanyId, CEID, CEDetailID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> DeleteCEItem(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            short CompanyId = 0;
            string CEID = "";
            string CEDetailID = "";

            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "CEID")
                    CEID = idict[key].ToString();
                else if (key.ToString() == "CEDetailID")
                    CEDetailID = idict[key].ToString();
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).DeleteCostEstimateItem(CompanyId, CEID, CEDetailID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> ConvertCE(short CompanyId, string CEID, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).ConvertCostEstimate(CompanyId, CEID, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }
        [WebMethod]
        public static List<Dictionary<string, string>> ReviseCE(short CompanyId, string CEID, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).ReviseCostEstimate(CompanyId, CEID, UserID, ref dsTemp);
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
        public static List<Dictionary<string, string>> GetRevisionList(string DocNo, short CompanyId, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetRevisionList(CompanyId, DocNo, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterialCategory(short CompanyId, string term, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetMaterialCatList(CompanyId, term, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetMaterial(short CompanyId, string category, string material, string size, string suppliername, string brand, string description, string field, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetMaterial(CompanyId, category, material, size, suppliername, brand, description, field, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

    }
}
