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
    public partial class Payment : GMSBasePage
    {
        protected static short CoyID = 0;
        protected static short UserID = 0;
        //protected static short GMSCoyID = 0;

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

            CoyID = session.CompanyId;
            UserID = session.UserId;
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetPaymentList(string ProjectNo)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetPaymentList(CoyID, ProjectNo, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> InsertPayment(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ProjectNo = "";
            short PCId = 0;
            string ClaimOrder = "";
            Nullable<DateTime> ClaimDate = null;
            string CurrencyCode = "";
            decimal ClaimAmount = 0;
            decimal Balance = 0;
            string Retention = "";
            string Ref = "";
            string Remarks = "";
            string IsNew = "";

            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    ProjectNo = idict[key].ToString();
                else if (key.ToString() == "PCId")
                    PCId = GMSUtil.ToShort(idict[key]);
                else if (key.ToString() == "ClaimOrder")
                    ClaimOrder = idict[key].ToString();
                else if (key.ToString() == "ClaimDate")
                    ClaimDate = GMSUtil.ToDate(idict[key]);
                else if (key.ToString() == "CurrencyCode")
                    CurrencyCode = idict[key].ToString();
                else if (key.ToString() == "ClaimAmount")
                    ClaimAmount = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "Balance")
                    Balance = GMSUtil.ToDecimal(idict[key]);
                else if (key.ToString() == "Retention")
                    Retention = idict[key].ToString();
                else if (key.ToString() == "Ref")
                    Ref = idict[key].ToString();
                else if (key.ToString() == "Remarks")
                    Remarks = idict[key].ToString();
                else if (key.ToString() == "IsNew")
                    IsNew = idict[key].ToString();
            }
            DataSet dsTemp = new DataSet();
            if (IsNew == "Yes")
                (new EngineeringDataDALC()).InsertPaymentList(CoyID, ProjectNo, ClaimOrder, ClaimDate, CurrencyCode, ClaimAmount, Balance, Retention, Ref, Remarks, UserID, ref dsTemp);
            else
                (new EngineeringDataDALC()).UpdatePaymentList(CoyID, ProjectNo, PCId, ClaimOrder, ClaimDate, CurrencyCode, ClaimAmount, Balance, Retention, Ref, Remarks, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }


        [WebMethod]
        public static List<Dictionary<string, string>> DeletePayment(Object Info)
        {
            IDictionary idict = (IDictionary)Info;
            string ProjectNo = "";
            short PCId = 0;

            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (object key in idict.Keys)
            {
                if (key.ToString() == "ProjectNo")
                    ProjectNo = idict[key].ToString();
                else if (key.ToString() == "PCId")
                    PCId = GMSUtil.ToShort(idict[key]);

            }
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).DeletePaymentList(CoyID, ProjectNo, PCId, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);


        }
    }
}
