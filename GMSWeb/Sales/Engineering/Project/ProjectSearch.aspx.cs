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
using System.Transactions;


namespace GMSWeb.Sales.Engineering.Project
{
    public partial class ProjectSearch : GMSBasePage
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

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        private void Localize()
        {

        }

        public static bool CheckUserAccessPage(short CompanyId, short UserId)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 132);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(CompanyId, loginUserOrAlternateParty,
                                                                            132);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
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
        public static List<Dictionary<string, string>> GetProjectNo(short CompanyId, string projectno)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetProjectNo(CompanyId, projectno, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }


        [WebMethod]
        public static List<Dictionary<string, string>> GetPrevProjectNo(short CompanyId, string prevprojectno)
        {
            
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetPrevProjectNo(CompanyId, prevprojectno, ref dsTemp);
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
        public static List<Dictionary<string, string>> GetStatusList()
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetStatusList(ref dsTemp);
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
        public static List<Dictionary<string, string>> GetProjectList(short CoyID, string ProjectNo, string PrevProjectNo, string AccountCode, string AccountName, string CustomerPo, 
            string EngineerId, string SalespersonId, string IsBillable, string IsProgressiveClaim, string StatusId, string CreatedDateFrom, string CreatedDateTo, string CommencementDateFrom, 
            string CommencementDateTo, string ClosingDateFrom, string ClosingDateTo, short UserID)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CoyID, UserID);
            DataSet dsTemp = new DataSet();
            DateTime createddatefrom = GMSUtil.ToDate(CreatedDateFrom);
            DateTime createddateto = GMSUtil.ToDate(CreatedDateTo);
            DateTime commencementdatefrom = GMSUtil.ToDate(CommencementDateFrom);
            DateTime commencementdateto = GMSUtil.ToDate(CommencementDateTo);
            DateTime closingdatefrom = GMSUtil.ToDate(ClosingDateFrom);
            DateTime closingdateto = GMSUtil.ToDate(ClosingDateTo);
            dsTemp.Clear();
            new EngineeringDataDALC().GetProjectList(CoyID, ProjectNo, PrevProjectNo, AccountCode, AccountName, CustomerPo, EngineerId, SalespersonId, IsBillable, IsProgressiveClaim, StatusId,
                createddatefrom, createddateto, commencementdatefrom, commencementdateto, closingdatefrom, closingdateto, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }
        
    }
}
