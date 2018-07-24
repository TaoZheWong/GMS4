using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb
{
    public partial class main : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                //Response.Redirect("SessionTimeout.htm");
                Response.Redirect(base.SessionTimeOutPage("")); 
                return;
            }

            redirect();
            //this.username.InnerText = session.UserRealName;
        }

        /*
        protected void linkButton_Click(object sender, CommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            session.CountryId = GMSUtil.ToShort(e.CommandName);
            session.DivisionId = GMSUtil.ToShort(e.CommandArgument);

            IList<Company> lstCompany = null;
            lstCompany = new SystemDataActivity().RetrieveCompanyByCountryIdDivisionIdSortByCountryId(session.CountryId, session.DivisionId);
            if (lstCompany != null && lstCompany.Count > 0)
            {
                foreach (Company coy in lstCompany)
                {
                    UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
                                                                           coy.CoyID);
                    if (uAccess != null)
                    {
                        session.CompanyId = coy.CoyID;
                        List<short> lstModuleCategory = session.UserAccessModuleCategory;
                        if (lstModuleCategory.Contains(10))
                        Response.Redirect("~/Finance/Default.aspx");
                        else if (lstModuleCategory.Contains(8))
                        Response.Redirect("~/Sales/Default.aspx");
                        else if (lstModuleCategory.Contains(7))
                        Response.Redirect("~/Products/Default.aspx");
                        else if (lstModuleCategory.Contains(11))
                        Response.Redirect("~/HR/Default.aspx");
                        else Response.Redirect("~/Sales/Default.aspx");
                    }
                }
                base.JScriptAlertMsg("You are not authorized to view selected division in this country.");
                return;
            }
            else
                session.CompanyId = 0;
            Response.Redirect("~/Sales/Default.aspx");
        }

        protected void lnkCorporate_Click(object sender, CommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            List<short> lstModuleCategory = session.UserAccessModuleCategory;
            if (lstModuleCategory.Contains(5))
                Response.Redirect("~/Admin/Default.aspx");
            else if (lstModuleCategory.Contains(3))
                Response.Redirect("~/SysHR/Default.aspx");
            else if (lstModuleCategory.Contains(2))
                Response.Redirect("~/SysFinance/Default.aspx");
            else if (lstModuleCategory.Contains(4))
                Response.Redirect("~/CorporateServices/Default.aspx");
            else base.JScriptAlertMsg("You are not authorized to view Corporate Offices section.");
            return;
        }
        */

        protected void redirect()
        {
            LogSession session = base.GetSessionInfo();

            //Admin directly go into Admin page
            List<short> lstModuleCategory = session.UserAccessModuleCategory;
            if (lstModuleCategory.Contains(5))
            {
                session.CountryId = 1;
                session.DivisionId = 1;
                session.CompanyId = 61;
                Response.Redirect("~/Admin/Default.aspx");
            }

            //Access company session first 
            IList<Company> lstCompany = null;
            for (short i = 1; i < 12; i++)
            {
                if (i != 9)
                {
                    for (short j = 5; j > 1; j--)
                    {
                        lstCompany = new SystemDataActivity().RetrieveCompanyByCountryIdDivisionIdSortByCountryIdAndSeqID(i, j);
                        if (lstCompany != null && lstCompany.Count > 0)
                        {
                            foreach (Company coy in lstCompany)
                            {
                                UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
                                                                                       coy.CoyID);
                                if (uAccess != null)
                                {
                                    session.CountryId = i;
                                    session.DivisionId = j;
                                    session.CompanyId = coy.CoyID;
                                    session.WebServiceAddress = coy.WebServiceAddress;
                                    session.CMSWebServiceAddress = coy.CMSWebServiceAddress;
                                    session.DefaultCurrency = coy.DefaultCurrencyCode;
                                    session.TBType = coy.TBType;
                                    session.FYE = coy.FYE;
                                    session.DBName = coy.DBName;
                                    session.StatusType = coy.StatusType;
                                    session.LMSParallelRunEndDate = coy.LMSParallelRunEndDate;
                                    session.IsOffline = coy.IsOffline;
                                    session.SAPStartDate = coy.SAPStartDate;                                   
                                    session.GASLMSWebServiceAddress = coy.GASLMSWebServiceAddress;
                                    session.WSDLMSWebServiceAddress = coy.WSDLMSWebServiceAddress;
                                    session.TableSuffix = coy.TableSuffix;
                                    session.MRScheme = coy.MRScheme;
                                    session.SAPURI = coy.SAPURI;
                                    session.SAPKEY = coy.SAPKEY;
                                    session.SAPDB = coy.SAPDB;
                                    session.DimensionL1 = coy.DimensionL1;
                                    session.DefaultWarehouse = coy.DefaultWarehouse;


                                    if (session.ToNewsPage)
                                        Response.Redirect("~/News/Default.aspx");
                                    else if (lstModuleCategory.Contains(10))
                                        Response.Redirect("~/Finance/Default.aspx");
                                    else if (lstModuleCategory.Contains(8))
                                        Response.Redirect("~/Sales/Default.aspx");
                                    else if (lstModuleCategory.Contains(7))
                                        Response.Redirect("~/Products/Default.aspx");
                                    else if (lstModuleCategory.Contains(9))
                                        Response.Redirect("~/Debtors/Default.aspx");
                                    else if (lstModuleCategory.Contains(6))
                                        Response.Redirect("~/Suppliers/Default.aspx");
                                    else if (lstModuleCategory.Contains(11))
                                        Response.Redirect("~/HR/Default.aspx");
                                    else if (lstModuleCategory.Contains(4))
                                        Response.Redirect("~/UsefulResources/Default.aspx");
                                    else Response.Redirect("~/Sales/Default.aspx");
                                     
                                }
                            }
                        }
                    }
                }
            }

            //If no company access found, go into group access
            if (lstModuleCategory.Contains(3))
            {
                session.CountryId = 1;
                session.DivisionId = 1;
                session.CompanyId = 61;
                Response.Redirect("~/SysHR/Default.aspx");
            }
            if (lstModuleCategory.Contains(2))
            {
                session.CountryId = 1;
                session.DivisionId = 1;
                session.CompanyId = 61;
                Response.Redirect("~/SysFinance/Default.aspx");
            }

            //if no access rights assgin
            base.JScriptAlertMsg("Your access rights have not been assgined yet. Please contact the administrator.");
            ClientScript.RegisterStartupScript(this.GetType(), "1",
                "<script type='text/javascript'>\n  window.top.navigate(\"Logout.aspx\");\n</script>");
        }
    }
}
