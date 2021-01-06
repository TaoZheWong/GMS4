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
using SharpPieces.Web.Controls;
using GMSCore.Activity;
using System.Collections.Generic;
using GMSCore.Entity;

namespace GMSWeb.Common
{
    public partial class banner : GMSBasePage
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

            lblWelcome.Text = "Welcome " + session.UserRealName + "!";
        }

        private void PopulateDDL()
        {
            LogSession session = base.GetSessionInfo();
            IList<Country> countryList = (new SystemDataActivity()).RetrieveAllCountryListSortBySeqID();
            foreach (Country country in countryList)
            {
                if (country.CountryID != 9)
                {
                    bool isNewGrp = true;
                    IList<Company> companyList = (new SystemDataActivity()).RetrieveCompanyByCountryId(country.CountryID);
                    foreach (Company company in companyList)
                    {
                        UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
                                                                                  company.CoyID);
                        if (uAccess != null)
                        {
                            if (isNewGrp)
                                this.ddlCompany.ExtendedItems.Add(new ExtendedListItem(company.Name, company.CoyID.ToString(), true, ListItemGroupingType.New, country.Name));
                            else
                                this.ddlCompany.ExtendedItems.Add(new ExtendedListItem(company.Name, company.CoyID.ToString(), ListItemGroupingType.Inherit));
                            isNewGrp = false;
                        }
                    }
                }
                else
                {
                    List<short> lstModuleCategory = session.UserAccessModuleCategory;
                    if (lstModuleCategory.Contains(5) || lstModuleCategory.Contains(3) || lstModuleCategory.Contains(2) || lstModuleCategory.Contains(4))
                    {
                        this.ddlCompany.ExtendedItems.Add(new ExtendedListItem("Leeden Group", "61", true, ListItemGroupingType.New, "Group"));
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
            if (uAccess == null && companyId != 61)
            {
                this.ddlCompany.SelectedValue = session.CompanyId.ToString();
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page,ddlCompany.Page.GetType(),"2",
                "<script type='text/javascript'>\n  alert(\"" + "You are not authorized.".Replace("\"", "'") + "\");\n</script>", false);
            }
            else
            {
                Company oldCoy = Company.RetrieveByKey(session.CompanyId);
                Company newCoy = Company.RetrieveByKey(companyId);
                session.CompanyId = newCoy.CoyID;
                session.CountryId = newCoy.CountryID;
                session.DivisionId = newCoy.DivisionID;
                if (oldCoy.CountryID != 9 && newCoy.CountryID == 9)
                {
                    List<short> lstModuleCategory = session.UserAccessModuleCategory;
                    if (lstModuleCategory.Contains(5))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page,ddlCompany.Page.GetType(),"2",
                        "<script type='text/javascript'>\n  window.top.navigate(\"../Admin/Default.aspx\");\n</script>", false);
                    }
                    else if (lstModuleCategory.Contains(2))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  window.top.navigate(\"../SysFinance/Default.aspx\");\n</script>",false);
                    }
                    else if (lstModuleCategory.Contains(3))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  window.top.navigate(\"../SysHR/Default.aspx\");\n</script>",false);
                    }
                    else if (lstModuleCategory.Contains(4))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  window.top.navigate(\"../CorporateServices/Default.aspx\");\n</script>",false);
                    }
                    else System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  alert(\"" + "You are not authorized.".Replace("\"", "'") + "\");\n</script>",false);
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
                            "<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>",false);
                    else if (lstModuleCategory.Contains(7))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.top.navigate(\"../Products/Default.aspx\");\n</script>",false);
                    else if (lstModuleCategory.Contains(11))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.top.navigate(\"../HR/Default.aspx\");\n</script>",false);
                    else System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                       "<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>",false);
                }
                else
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "script1", "<script type=\"text/javascript\"> parent.window.location.reload();</script>", false);
            }
        }
    }
}
