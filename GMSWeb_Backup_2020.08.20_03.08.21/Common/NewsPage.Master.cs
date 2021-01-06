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
using SharpPieces.Web.Controls;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
namespace GMSWeb
{
    public partial class NewsPage : System.Web.UI.MasterPage
    {
        PageHandler pageHandler = new PageHandler();
        string currentLink;
        string appPath = HttpRuntime.AppDomainAppVirtualPath; 

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = GetSessionInfo();
            if (session == null)
            {
                //Response.Redirect("../SessionTimeout.htm");
                Response.Redirect(appPath + "/SessionTimeoutMain.aspx"); 
                return;
            }

            if (!IsPostBack)
            {
                PopulateDDL();
            }

            string inner = pageHandler.LoadTabs(this.currentLink);
            linkTag.InnerHtml = inner;
            //string innerSidebar = pageHandler.LoadSidebar(this.currentLink);
            //Sidebar.InnerHtml = innerSidebar; 
            
            lblWelcome.Text = session.UserRealName;

        }

        public string getCurrentLink
        {
            get
            {
                return this.currentLink; 
            }
        }

        public void setCurrentLink(string currentLink)
        {
            this.currentLink = currentLink; 
        }

        #region GetSessionInfo
        /// <summary>
        /// Function to get the current page's session information
        /// </summary>
        /// <returns>LogSession object</returns>
        public LogSession GetSessionInfo()
        {
            LogSession sess = (LogSession)Session[GMSCoreBase.SESSIONNAME];

            if (sess == null)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            return sess;
        }
        #endregion

        private void PopulateDDL()
        {
            LogSession session = GetSessionInfo();
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
                    if (lstModuleCategory.Contains(5) || lstModuleCategory.Contains(3) || lstModuleCategory.Contains(2))
                    {
                        this.ddlCompany.ExtendedItems.Add(new ExtendedListItem("Leeden Group", "61", true, ListItemGroupingType.New, "Group"));
                    }
                }
            }
            ddlCompany.SelectedValue = session.CompanyId.ToString();
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = GetSessionInfo();
            PageHandler pageHandler = new PageHandler();
            this.ddlCompany_SelectedIndexChanged(GMSUtil.ToShort(this.ddlCompany.SelectedValue));
            ddlCompany.SelectedValue = session.CompanyId.ToString();
        }

        public void ddlCompany_SelectedIndexChanged(short companyId)
        {
            LogSession session = GetSessionInfo();

            UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
                                                                            companyId);
            if (uAccess == null && companyId != 61)
            {
                this.ddlCompany.SelectedValue = session.CompanyId.ToString();
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                "<script type='text/javascript'>\n  alert(\"" + "You are not authorized.".Replace("\"", "'") + "\");\n</script>", false);
            }

            else
            {
                Company oldCoy = Company.RetrieveByKey(session.CompanyId);
                Company newCoy = Company.RetrieveByKey(companyId);
                session.CompanyId = newCoy.CoyID;
                session.CountryId = newCoy.CountryID;
                session.DivisionId = newCoy.DivisionID;
                session.WebServiceAddress = newCoy.WebServiceAddress;
                session.DefaultCurrency = newCoy.DefaultCurrencyCode;
                session.CMSWebServiceAddress = newCoy.CMSWebServiceAddress;
                session.TBType = newCoy.TBType;
                session.IsOffline = newCoy.IsOffline;
                session.FYE = newCoy.FYE;
                session.DBName = newCoy.DBName;
                session.StatusType = newCoy.StatusType;
                session.LMSParallelRunEndDate = newCoy.LMSParallelRunEndDate;
                session.SAPStartDate = newCoy.SAPStartDate;               
                session.GASLMSWebServiceAddress = newCoy.GASLMSWebServiceAddress;
                session.WSDLMSWebServiceAddress = newCoy.WSDLMSWebServiceAddress;
                session.TableSuffix = newCoy.TableSuffix;
                session.MRScheme = newCoy.MRScheme;                       
                string appPath = HttpRuntime.AppDomainAppVirtualPath;

                //commented by OSS 12 Sep 2011
                //if (oldCoy.CountryID != 9 && newCoy.CountryID == 9)
                if (newCoy.CountryID == 9)
                {
                    List<short> lstModuleCategory = session.UserAccessModuleCategory;
                    if (lstModuleCategory.Contains(5))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Admin/Default.aspx\";\n</script>", false);
                    }
                    else if (lstModuleCategory.Contains(2))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/SysFinance/Default.aspx\";\n</script>", false);
                    }
                    else if (lstModuleCategory.Contains(3))
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/SysHR/Default.aspx\";\n</script>", false);
                    }
                    else System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                        "<script type='text/javascript'>\n  alert(\"" + "You are not authorized.".Replace("\"", "'") + "\");\n</script>", false);
                }
                //commented by OSS 12 Sep 2011
                //else if (oldCoy.CountryID == 9 && newCoy.CountryID != 9)
                else if (newCoy.CountryID != 9)
                {
                    List<short> lstModuleCategory = session.UserAccessModuleCategory;
                    if (lstModuleCategory.Contains(10))
                        //ddlDivision.Page.Response.Redirect("../Finance/Default.aspx");
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "3",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Finance/Default.aspx\";\n</script>", false);
                    else if (lstModuleCategory.Contains(8))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Sales/Default.aspx\";\n</script>", false);
                    else if (lstModuleCategory.Contains(9))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Debtors/Default.aspx\";\n</script>", false);
                    else if (lstModuleCategory.Contains(7))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Products/Default.aspx\";\n</script>", false);
                    else if (lstModuleCategory.Contains(6))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Suppliers/Default.aspx\";\n</script>", false);
                    else if (lstModuleCategory.Contains(11))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/HR/Default.aspx\";\n</script>", false);
                    else if (lstModuleCategory.Contains(4))
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                            "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/UsefulResources/Default.aspx\";\n</script>", false);
                    else System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "2",
                  "<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/Sales/Default.aspx\";\n</script>", false);
                }
                //else
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "script1", "<script type=\"text/javascript\"> parent.window.location.reload();</script>", false);
            }
        }

    }
}
