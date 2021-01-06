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
    public partial class CostEstimateSearch : GMSBasePage
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
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 136);
            UserAccessModule uAccess1 = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 139);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(CompanyId, loginUserOrAlternateParty,
                                                                            136);
            IList<UserAccessModuleForCompany> uAccessForCompanyList1 = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(CompanyId, loginUserOrAlternateParty,
                                                                            139);
            if (uAccess == null && uAccess1 == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0) && (uAccessForCompanyList1 != null && uAccessForCompanyList1.Count == 0))
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
        public static List<Dictionary<string, string>> GetCostEstimate(short CompanyId, string ceid, string accountcode, string accountname, string cestatusid, DateTime createddatefrom, DateTime createddateto,string engineername, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetCostEstimate(CompanyId, ceid, accountcode, accountname, cestatusid, createddatefrom, createddateto, engineername, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetAccountList(short CompanyID, string account)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetAccountList(CompanyID, account, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetEngineerList(short CompanyID, string engineer)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetEngineerList(CompanyID, engineer, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

    }
}
