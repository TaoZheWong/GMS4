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
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Text;
using System.Web.Services;
using AjaxControlToolkit;


namespace GMSWeb.Sales.Engineering.Project
{
    public partial class EditProject : GMSBasePage
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

            short CompanyId = 0;
            if (Request.Params["CoyID"] != null && Request.Params["CoyID"].ToString() != "")
            {
                CompanyId = GMSUtil.ToShort(Request.Params["CoyID"]);
            }
            else
            {
                Response.Redirect("EditProject.aspx?CurrentLink=" + currentLink);
            }

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

        public static DataSet CanUserAccessDocument(short CompanyId, short UserId, string projectno)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet ds1 = new DataSet();
            new EngineeringDataDALC().GetUserAccessList(CompanyId, projectno, loginUserOrAlternateParty, ref ds1);
            return ds1;
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetProjectInformation(short CoyID, string ProjectNo, short UserId)
        {

            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetProjectInformation(CoyID, ProjectNo, UserId, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }


        [WebMethod]
        public static List<Dictionary<string, string>> CheckUserAccess(short CompanyId, string DocNo, short UserID)
        {
            DataSet dsTemp = new DataSet();
            (new EngineeringDataDALC()).GetUserAccessList(CompanyId, DocNo, UserID, ref dsTemp);
            return GMSUtil.ToJson(dsTemp, 0);
        }

    }
}
