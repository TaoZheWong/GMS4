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
using System.Transactions;
using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Text;
using System.Web.Services;
using AjaxControlToolkit;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace GMSWeb.Products.Products
{    
    public partial class MaterialRequisitionSearch : GMSBasePage  
    {        
        protected void Page_Load(object sender, EventArgs e)
        {          
            LogSession session = base.GetSessionInfo();
            string currentLink = "";
            currentLink = "Products";
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
                
            if(CheckUserAccessPage(session.CompanyId, session.UserId) == false)
                Response.Redirect(base.UnauthorizedPage(currentLink));

            hidCoyID.Value = session.CompanyId.ToString();
            hidUserID.Value = session.UserId.ToString();            
            string userRole = "";
            userRole = GetUserRole(session.CompanyId, session.UserId);
            hidUserRole.Value = userRole;
            hidCurrentLink.Value = currentLink;
            hidMRScheme.Value = session.MRScheme;
        }

        public static bool CheckUserAccessPage(short CompanyId, short UserId)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,120);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(CompanyId, loginUserOrAlternateParty,
                                                                            120);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                return false;
            return true;

        }



        public static string GetUserRole(short CompanyId, short UserId)
        {
            string userRole = "";
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            DataSet lstUserRole = new DataSet();
            new GMSGeneralDALC().GetMRUserRoleByUserNumIDCoyID(CompanyId, loginUserOrAlternateParty, ref lstUserRole);
            if ((lstUserRole != null) && (lstUserRole.Tables[0].Rows.Count > 0))
            {
                userRole = lstUserRole.Tables[0].Rows[0]["UserRole"].ToString();
            }
            return userRole; 
        }

       
        public static short GetloginUserOrAlternateParty(short CompanyId, short UserID)
        {
            short loginUserOrAlternateParty = UserID;
            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(CompanyId, UserID, "MR", ref lstAlterParty);
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
        public static List<Dictionary<string, string>> GetMR(short CoyID, string DateFrom, string DateTo, string MRNo, string Purchaser, string CustomerCode, string CustomerName, string ProductCode, string ProductName, string ProductGroupCode, string ProductGroupName, string Vendor, string PO, string Status, string RequestorRemarks, string Requestor, string ProductManagerID, string BudgetCode, string RefNo, string ProjectNo, string UserRole, short UserID, string MRScheme)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CoyID, UserID);
            //new GMSGeneralDALC().InsertMRLog(CoyID, MRNo, UserID, loginUserOrAlternateParty, "Search");
            DataSet dsTemp = new DataSet(); 
            DateTime dateFrom = GMSUtil.ToDate(DateFrom);
            DateTime dateTo = GMSUtil.ToDate(DateTo);
            string accountCode = "%" + CustomerCode + "%";
            string accountName = "%" + CustomerName + "%";
            string productCode = "%" + ProductCode + "%";
            string productName = "%" + ProductName + "%";
            string productGroup = "%" + ProductGroupCode + "%";
            string productGroupName = "%" + ProductGroupName + "%";
            string productManagerID = ProductManagerID;
            string requestor = "%" + Requestor + "%";
            string mrNo = "%" + MRNo + "%";
            string vendor = "%" + Vendor + "%";
            string poNo = "%" + PO + "%";
            string purchaser = "%" + Purchaser + "%";
            string refNo = "%" + RefNo + "%";
            string budgetCode = "%" + BudgetCode + "%";
            string projectNo = "%" + ProjectNo + "%";
            string requestorRemarks = "%" + RequestorRemarks + "%";
            string status = Status;
            //new GMSGeneralDALC().InsertMRLog(CoyID, MRNo, UserID, loginUserOrAlternateParty, "Search 2");
            dsTemp.Clear();
            new GMSGeneralDALC().GetMaterialRequisition(CoyID, loginUserOrAlternateParty, dateFrom, dateTo, status, accountCode, accountName, productCode, productName, productGroup, productGroupName, productManagerID, requestor, UserRole, mrNo, vendor, poNo, purchaser, refNo, budgetCode, projectNo, requestorRemarks, MRScheme, ref dsTemp);
            //new GMSGeneralDALC().InsertMRLog(CoyID, MRNo, UserID, loginUserOrAlternateParty, "Search 4");
            return GMSUtil.ToJson(dsTemp, 0);
            //return dsTemp;
        }
    }
}
