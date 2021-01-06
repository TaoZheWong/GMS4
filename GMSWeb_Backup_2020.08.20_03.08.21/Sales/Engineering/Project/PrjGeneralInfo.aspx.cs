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
    public partial class PrjGeneralInfo : GMSBasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {

            }
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetAccountBillingAddressList(short CompanyId, string account)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetAccountBillingAddressList(CompanyId, account, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> InsertProjectInformation(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            short CompanyId = 0;
            string projectno1 = "";
            string PrevProjectNo = "";
            string AccountCode = "";
            string AccountName = "";
            string QuotationNo = "";
            string RefNo = "";
            string StatusID = "";
            string SalesPersonID = "";
            string EngineerID = "";

            Nullable<bool> IsBillable = true;
            Nullable<bool> IsProgressiveClaim = true;
            string CurrencyCode = "";
            decimal TotalBillableAmt = 0;
            string ContractNo = "";
            Nullable<DateTime> ContractDateFrom = null;

            Nullable<DateTime> ContractDateTo = null;
            Nullable<DateTime> CommencementDate = null;
            Nullable<DateTime> CompletionDate = null;
            Nullable<DateTime> ClosingDate = null;
            string CustomerPO = "";
            string CustomerPIC = "";
            string OfficePhone = "";
            string Fax = "";
            string BillingAddress = "";
            string OnsiteLocation = "";

            string Description = "";
            string Remarks = "";
            short UserID = 0;
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "txtProjectNo")
                    projectno1 = idict[key].ToString();
                else if (key.ToString() == "txtPrevProjectNo")
                    PrevProjectNo = idict[key].ToString();
                else if (key.ToString() == "txtAccountCode")
                    AccountCode = idict[key].ToString();
                else if (key.ToString() == "txtAccountName")
                    AccountName = idict[key].ToString();
                else if (key.ToString() == "txtQuotationNo")
                    QuotationNo = idict[key].ToString();
                else if (key.ToString() == "txtRefNo")
                    RefNo = idict[key].ToString();
                else if (key.ToString() == "txtStatusID")
                    StatusID = idict[key].ToString();
                else if (key.ToString() == "txtSalesPersonID")
                    SalesPersonID = idict[key].ToString();
                else if (key.ToString() == "txtEngineerID")
                    EngineerID = idict[key].ToString();
                else if (key.ToString() == "txtIsBillable" && idict[key].ToString() == "False")
                    IsBillable = false;
                else if (key.ToString() == "txtIsBillable" && idict[key].ToString() == "True")
                    IsBillable = true;
                else if (key.ToString() == "txtIsProgressiveClaim" && idict[key].ToString() == "False")
                    IsProgressiveClaim = false;
                else if (key.ToString() == "txtIsProgressiveClaim" && idict[key].ToString() == "True")
                    IsProgressiveClaim = true;
                else if (key.ToString() == "txtCurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "txtTotalBillableAmt")
                    TotalBillableAmt = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "txtContractNo")
                    ContractNo = idict[key].ToString();
                else if (key.ToString() == "txtContractDateFrom")
                    ContractDateFrom = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "txtContractDateTo")
                    ContractDateTo = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "txtCommencementDate")
                    CommencementDate = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "txtCompletionDate")
                    CompletionDate = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "txtClosingDate")
                    ClosingDate = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "txtCustomerPO")
                    CustomerPO = idict[key].ToString();
                else if (key.ToString() == "txtCustomerPIC")
                    CustomerPIC = idict[key].ToString();
                else if (key.ToString() == "txtOfficePhone")
                    OfficePhone = idict[key].ToString();
                else if (key.ToString() == "txtFax")
                    Fax = idict[key].ToString();
                else if (key.ToString() == "txtBillingAddress")
                    BillingAddress = idict[key].ToString();
                else if (key.ToString() == "txtOnsiteLocation")
                    OnsiteLocation = idict[key].ToString();
                else if (key.ToString() == "txtDescription")
                    Description = idict[key].ToString();
                else if (key.ToString() == "txtRemarks")
                    Remarks = idict[key].ToString();
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
                (new EngineeringDataDALC()).InsertProjectInformation(CompanyId, PrevProjectNo, AccountCode, AccountName, QuotationNo, RefNo, StatusID, SalesPersonID, EngineerID,
                    IsBillable, IsProgressiveClaim, CurrencyCode, TotalBillableAmt, ContractNo, GMSUtil.ToDate(ContractDateFrom), GMSUtil.ToDate(ContractDateTo), GMSUtil.ToDate(CommencementDate), GMSUtil.ToDate(CompletionDate), GMSUtil.ToDate(ClosingDate),
                    CustomerPO, CustomerPIC, OfficePhone, Fax, BillingAddress, OnsiteLocation, Description, Remarks, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> UpdateProjectInformation(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            short CompanyId = 0;
            string projectno1 = "";
            string PrevProjectNo = "";
            string AccountCode = "";
            string AccountName = "";
            string QuotationNo = "";
            string RefNo = "";
            string StatusID = "";
            string SalesPersonID = "";
            string EngineerID = "";

            bool IsBillable = true;
            bool IsProgressiveClaim = true;
            string CurrencyCode = "";
            decimal TotalBillableAmt = 0;
            string ContractNo = "";
            Nullable<DateTime> ContractDateFrom = null;

            Nullable<DateTime> ContractDateTo = null;
            Nullable<DateTime> CommencementDate = null;
            Nullable<DateTime> CompletionDate = null;
            Nullable<DateTime> ClosingDate = null;
            string CustomerPO = "";
            string CustomerPIC = "";
            string OfficePhone = "";
            string Fax = "";
            string BillingAddress = "";
            string OnsiteLocation = "";

            string Description = "";
            string Remarks = "";
            
            short UserID = 0;

            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "CompanyId")
                    CompanyId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "txtProjectNo")
                    projectno1 = idict[key].ToString();
                else if (key.ToString() == "txtPrevProjectNo")
                    PrevProjectNo = idict[key].ToString();
                else if (key.ToString() == "txtAccountCode")
                    AccountCode = idict[key].ToString();
                else if (key.ToString() == "txtAccountName")
                    AccountName = idict[key].ToString();
                else if (key.ToString() == "txtQuotationNo")
                    QuotationNo = idict[key].ToString();
                else if (key.ToString() == "txtRefNo")
                    RefNo = idict[key].ToString();
                else if (key.ToString() == "txtStatusID")
                    StatusID = idict[key].ToString();
                else if (key.ToString() == "txtSalesPersonID")
                    SalesPersonID = idict[key].ToString();
                else if (key.ToString() == "txtEngineerID")
                    EngineerID = idict[key].ToString();
                else if (key.ToString() == "txtIsBillable" && idict[key].ToString() == "False")
                    IsBillable = false;
                else if (key.ToString() == "txtIsBillable" && idict[key].ToString() == "True")
                    IsBillable = true;
                else if (key.ToString() == "txtIsProgressiveClaim" && idict[key].ToString() == "False")
                    IsProgressiveClaim = false;
                else if (key.ToString() == "txtIsProgressiveClaim" && idict[key].ToString() == "True")
                    IsProgressiveClaim = true;
                else if (key.ToString() == "txtCurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "txtTotalBillableAmt")
                    TotalBillableAmt = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "txtContractNo")
                    ContractNo = idict[key].ToString();
                else if (key.ToString() == "txtContractDateFrom")
                    ContractDateFrom = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "txtContractDateTo")
                    ContractDateTo = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "txtCommencementDate")
                    CommencementDate = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "txtCompletionDate")
                    CompletionDate = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "txtClosingDate")
                    ClosingDate = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "txtCustomerPO")
                    CustomerPO = idict[key].ToString();
                else if (key.ToString() == "txtCustomerPIC")
                    CustomerPIC = idict[key].ToString();
                else if (key.ToString() == "txtOfficePhone")
                    OfficePhone = idict[key].ToString();
                else if (key.ToString() == "txtFax")
                    Fax = idict[key].ToString();
                else if (key.ToString() == "txtBillingAddress")
                    BillingAddress = idict[key].ToString();
                else if (key.ToString() == "txtOnsiteLocation")
                    OnsiteLocation = idict[key].ToString();
                else if (key.ToString() == "txtDescription")
                    Description = idict[key].ToString();
                else if (key.ToString() == "txtRemarks")
                    Remarks = idict[key].ToString();
                else if (key.ToString() == "UserID")
                    UserID = GMSUtil.ToShort(idict[key]);
            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).UpdateProjectInformation(CompanyId, projectno1, PrevProjectNo, AccountCode, AccountName, QuotationNo, RefNo, StatusID, SalesPersonID, EngineerID,
                    IsBillable, IsProgressiveClaim, CurrencyCode, TotalBillableAmt, ContractNo, GMSUtil.ToDate(ContractDateFrom), GMSUtil.ToDate(ContractDateTo), GMSUtil.ToDate(CommencementDate), GMSUtil.ToDate(CompletionDate), GMSUtil.ToDate(ClosingDate),
                    CustomerPO, CustomerPIC, OfficePhone, Fax, BillingAddress, OnsiteLocation, Description, Remarks, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }
    }


}
