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

using GMSCore;
using GMSWeb.CustomCtrl;
using GMSCore.Entity;
using GMSCore.Activity;
using System.Collections.Generic;

namespace GMSWeb.Products.Products
{
    public partial class MaterialRequisitionDivision : GMSBasePage
    {
        protected short loginUserOrAlternateParty = 0;
        protected bool canAccessCost = false;
        string currentLink = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            
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

            if (CheckUserAccessPage(session.CompanyId, session.UserId) == false)
                Response.Redirect(base.UnauthorizedPage(currentLink));

            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(session.CompanyId, session.UserId);

            if (session.CompanyId.ToString() == "120")
            {
                DivisionUser du = DivisionUser.RetrieveByKey(session.CompanyId, loginUserOrAlternateParty);
                if (du != null)
                {
                    if (du.DivisionID == "GAS")
                    {
                        session.MRScheme = "Department";                       
                        session.DefaultWarehouse = "G02";
                        session.DimensionL1 = "GAS";
                    }
                    else if (du.DivisionID == "WSD")
                    {
                        session.MRScheme = "Product";
                        session.DefaultWarehouse = "W02";
                        session.DimensionL1 = "WSD";
                    }
                    Response.Redirect("MaterialRequisitionSearch.aspx?v1=9&CurrentLink=" + currentLink);
                }
            }
            else
            {               
                Response.Redirect("MaterialRequisitionSearch.aspx?v1=9&CurrentLink=" + currentLink);
            }


        }

        public static bool CheckUserAccessPage(short CompanyId, short UserId)
        {
            short loginUserOrAlternateParty = GetloginUserOrAlternateParty(CompanyId, UserId);
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 120);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(CompanyId, loginUserOrAlternateParty,
                                                                            120);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                return false;
            return true;

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

        protected void btnGas_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            session.MRScheme = "Department";
            session.DefaultWarehouse = "G02";
            session.DimensionL1 = "GAS";
            Response.Redirect("MaterialRequisitionSearch.aspx?v1=9&CurrentLink=" + currentLink);
        }

        protected void btnWelding_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            session.MRScheme = "Product";
            session.DefaultWarehouse = "W02";
            session.DimensionL1 = "WSD";
            Response.Redirect("MaterialRequisitionSearch.aspx?v1=9&CurrentLink=" + currentLink);
        }
    }
}