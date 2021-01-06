using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using SharpPieces.Web.Controls;

namespace GMSWeb.Admin
{
    public partial class adminbanner : GMSBasePage
    {
        PageHandler pageHandler = new PageHandler();

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../SessionTimeout.htm");
                return;
            }

            if (!IsPostBack)
            {
                PopulateDDL();
            }

            string inner = pageHandler.LoadTabs();
            linkTag.InnerHtml = inner;
        }

        private void PopulateDDL()
        {
            LogSession session = base.GetSessionInfo();
            IList<Country> countryList = (new SystemDataActivity()).RetrieveAllCountryListSortBySeqID();
            foreach (Country country in countryList)
            {
                bool isNewGrp = true;
                IList<Company> companyList = (new SystemDataActivity()).RetrieveCompanyByCountryId(country.CountryID);
                foreach (Company company in companyList)
                {
                    UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
                                                                              company.Id);
                    if (uAccess != null)
                    {
                        if (isNewGrp)
                            this.ddlCompany.ExtendedItems.Add(new ExtendedListItem(company.Name, company.Id.ToString(), true, ListItemGroupingType.New, country.Name));
                        else
                            this.ddlCompany.ExtendedItems.Add(new ExtendedListItem(company.Name, company.Id.ToString(), ListItemGroupingType.Inherit));
                        isNewGrp = false;
                    }
                }
            }
            ddlCompany.SelectedValue = session.CompanyId.ToString();
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            PageHandler pageHandler = new PageHandler();
            this.ddlCompany_SelectedIndexChanged(GMSUtil.ToShort(this.ddlCompany.SelectedValue));
            ddlCompany.SelectedValue = session.CompanyId.ToString();
        }

        public void ddlCompany_SelectedIndexChanged(short companyId)
        {
            LogSession session = base.GetSessionInfo();

            UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
                                                                            companyId);
            if (uAccess == null)
            {
                this.ddlCompany.SelectedValue = session.CompanyId.ToString();
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                "<script type='text/javascript'>\n  alert(\"" + "You are not authorized.".Replace("\"", "'") + "\");\n</script>", false);
            }
            else
            {
                Company oldCoy = Company.RetrieveByKey(session.CompanyId);
                Company newCoy = Company.RetrieveByKey(companyId);
                session.CompanyId = newCoy.Id;
                session.CountryId = newCoy.CountryID;
                session.DivisionId = newCoy.DivisionID;
                if (oldCoy.CountryID != 9 && newCoy.CountryID == 9)
                {
                    List<short> lstModuleCategory = session.UserAccessModuleCategory;
                    if (lstModuleCategory.Contains(5))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  window.top.navigate(\"../Admin/Default.aspx\");\n</script>", false);
                    }
                    else if (lstModuleCategory.Contains(2))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  window.top.navigate(\"../SysFinance/Default.aspx\");\n</script>", false);
                    }
                    else if (lstModuleCategory.Contains(3))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  window.top.navigate(\"../SysHR/Default.aspx\");\n</script>", false);
                    }
                    else if (lstModuleCategory.Contains(4))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  window.top.navigate(\"../CorporateServices/Default.aspx\");\n</script>", false);
                    }
                    else System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  alert(\"" + "You are not authorized.".Replace("\"", "'") + "\");\n</script>", false);
                }
                else if (oldCoy.CountryID == 9 && newCoy.CountryID != 9)
                {
                    List<short> lstModuleCategory = session.UserAccessModuleCategory;
                    if (lstModuleCategory.Contains(10))
                        //ddlDivision.Page.Response.Redirect("../Finance/Default.aspx");
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "3",
                            "<script type='text/javascript'>\n  window.top.navigate(\"../Finance/Default.aspx\");\n</script>", false);
                    else if (lstModuleCategory.Contains(8))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>", false);
                    else if (lstModuleCategory.Contains(7))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.top.navigate(\"../Products/Default.aspx\");\n</script>", false);
                    else if (lstModuleCategory.Contains(11))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.top.navigate(\"../HR/Default.aspx\");\n</script>", false);
                    else System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                       "<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>", false);
                }
                else
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "script1", "<script type=\"text/javascript\"> parent.window.location.reload();</script>", false);
            }
        }
    }
}
