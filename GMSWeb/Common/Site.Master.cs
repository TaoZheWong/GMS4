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
    public partial class Site : System.Web.UI.MasterPage
    {
        PageHandler pageHandler = new PageHandler();
        string currentLink, isLargeFont, isSidebarMinified , isOptimizedTable;
        string appPath = HttpRuntime.AppDomainAppVirtualPath;
        string sideBarFeatureString = @"
                                    <li class='nav-header'>&nbsp;</li>
                                    <li class='hidden-xs'>
                                        <a href='javascript:;' data-minified='false' id='setting_sidebar_minified'>
                                            <i class='hidden ti-layout-sidebar-left'></i>
                                            <i class='hidden ti-layout-sidebar-right'></i>
                                            <span class=''>Collapse</span>
                                        </a>
                                    </li>
                                    <li class='nav-divider hidden-xs'></li>
                                ";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();

            LogSession session = GetSessionInfo();
            if (session == null)
            {
                //Response.Redirect("../SessionTimeout.htm");
                Response.Redirect(appPath + "/SessionTimeoutMain.aspx");
                return;
            }

            //Getting LargerFont Cookies
            HttpCookie isLargeFontCookie = Request.Cookies["isLargeFont"];
            if (null == isLargeFontCookie)
                isLargeFont = "";
            else
                isLargeFont = isLargeFontCookie.Value == "true" ? "largeFont" : "";

            //Getting SideBarMinified Cookies
            HttpCookie isSidebarMinifiedCookie = Request.Cookies["sidebar-minified"];
            if (null == isSidebarMinifiedCookie)
                isSidebarMinified = "";
            else
                isSidebarMinified = isSidebarMinifiedCookie.Value == "true" ? "page-sidebar-minified" : "";

            //Getting optimizedtable Cookies
            HttpCookie isOptimizedTableCookie = Request.Cookies["isOptimizedTable"];
            if (null == isOptimizedTableCookie)
                isOptimizedTable = "";
            else
                isOptimizedTable = isOptimizedTableCookie.Value == "true" ? "optimizedTable" : "";


            if (!IsPostBack)
            {
                PopulateDDL();
                PopulateRepeater();
            }
            

            string inner = pageHandler.LoadTabs(this.currentLink);
            linkTag.InnerHtml = inner;
            linkTagDesktop.InnerHtml = inner;
            string innerSidebar = pageHandler.LoadSidebar(this.currentLink);
            Sidebar.InnerHtml = sideBarFeatureString + innerSidebar;
            
            lblWelcome.Text = session.UserRealName;


            globalHidCoyID.Value = session.CompanyId.ToString();
            globalHidUserID.Value = session.UserId.ToString();
            globalHidCurrency.Value = session.DefaultCurrency.ToString();
        }

        public string getIsOptimizedTable
        {
            get { return isOptimizedTable; }
        }

        public string getIsSidebarMinified
        {
            get { return isSidebarMinified; }
        }

        public string getIsLargeFont
        {
            get { return isLargeFont; }
        }

        public string getSelectedCompanyId {
            get { 
                LogSession session = GetSessionInfo();
                return session.CompanyId.ToString();
            }
        }

        public string getSelectedCountryId
        {
            get
            {
                LogSession session = GetSessionInfo();
                return session.CountryId.ToString();
            }
        }

        public string getSelectedCompanyName
        {
            get 
            {       
                LogSession session = GetSessionInfo();
                Company comp = Company.RetrieveByKey(session.CompanyId);

                return comp.Name;
            }
            
        }

        public string getAppPath {
            get {
                return appPath;
            }
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

        private void PopulateRepeater()
        {
            LogSession session = GetSessionInfo();
            List<short> lstModuleCategory = session.UserAccessModuleCategory;
            IList<Country> countryList = (new SystemDataActivity()).RetrieveAllCountryListSortBySeqID();

            RpCountryList.DataSource = countryList;
            RpCountryList.DataBind();

            int i = 0;
            foreach (Country country in countryList)
            {
                IList<Company> companyList = (new SystemDataActivity()).RetrieveCompanyByCountryId(country.CountryID);
                IList<Company> filteredList = new List<Company>();
                
                // Bind Data to sub repeater
                RepeaterItem item = this.RpCountryList.Items[i];
                
                // Append Flag
                Label label = (Label)item.FindControl("countryFlag");
                label.CssClass = appendCountryFlag(country.Name);

                Repeater rppCompanyList = (Repeater)item.FindControl("rppCompanyList");

                //Company Access control
                foreach (Company company in companyList)
                {
                    UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
                                                                              company.CoyID);
                    if (uAccess != null)
                        filteredList.Add(company);
                }

                if (rppCompanyList != null)
                {
                    //Access Control to Leeden Group
                    if(country.CountryID != 9){
                        rppCompanyList.DataSource = filteredList;
                        rppCompanyList.DataBind();
                    }else{
                        if(lstModuleCategory.Contains(5) || lstModuleCategory.Contains(3) || lstModuleCategory.Contains(2)){
                            rppCompanyList.DataSource = filteredList;
                            rppCompanyList.DataBind();
                        }
                    }
                }
                i++;
            }

        }

        protected void rppCompanyList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
           
            if (e.CommandName == "UpdateCountry")
            {
                LogSession session = GetSessionInfo();
                PageHandler pageHandler = new PageHandler();
                this.ddlCompany_SelectedIndexChanged(GMSUtil.ToShort(e.CommandArgument.ToString()));
                ddlCompany.SelectedValue = session.CompanyId.ToString();
            }
            
        }

        private string appendCountryFlag(string countryName)
        {
            string icon = "region-icon ";

            switch (countryName)
            {
                case "Group":
                    icon += "ti-world ";
                    break;
                case "Singapore":
                    icon += "flag-icon flag-icon-sg ";
                    break;
                case "Malaysia":
                    icon += "flag-icon flag-icon-my ";
                    break;
                case "Indonesia":
                    icon += "flag-icon flag-icon-id ";
                    break;
                case "China":
                    icon += "flag-icon flag-icon-cn ";
                    break;
                case "Philippines":
                    icon += "flag-icon flag-icon-ph ";
                    break;
                case "Vietnam":
                    icon += "flag-icon flag-icon-vn ";
                    break;
                case "Australia":
                    icon += "flag-icon flag-icon-au ";
                    break;
                case "Myanmar":
                    icon += "flag-icon flag-icon-ph ";
                    break;
                case "Thailand":
                    icon += "flag-icon flag-icon-th ";
                    break;
                case "Hong Kong":
                    icon += "flag-icon flag-icon-hk ";
                    break;
                case "United States":
                    icon += "flag-icon flag-icon-us ";
                    break;
                case "Taiwan":
                    icon += "flag-icon flag-icon-tw ";
                    break;
                case "Saudi Arabia":
                    icon += "flag-icon flag-icon-sa ";
                    break;
                default:
                    icon += "ti-folder ";
                    break;
            }

            return icon;
        }

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
                session.CMSWebServiceAddress = newCoy.CMSWebServiceAddress;
                session.DefaultCurrency = newCoy.DefaultCurrencyCode;
                session.TBType = newCoy.TBType;
                session.IsOffline = newCoy.IsOffline;
                session.FYE = newCoy.FYE;
                session.DBName = newCoy.DBName;
                session.StatusType = newCoy.StatusType;
                session.LMSParallelRunEndDate = newCoy.LMSParallelRunEndDate;
                session.SAPStartDate = newCoy.SAPStartDate;                
                session.GASLMSWebServiceAddress = newCoy.GASLMSWebServiceAddress;
                session.WSDLMSWebServiceAddress = newCoy.WSDLMSWebServiceAddress;
                session.MRScheme = newCoy.MRScheme;
                session.TableSuffix = newCoy.TableSuffix;
                session.SAPURI = newCoy.SAPURI;
                session.SAPKEY = newCoy.SAPKEY;
                session.SAPDB = newCoy.SAPDB;
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
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "3",
                          //"<script type='text/javascript'>\n  window.location.href = \"" + appPath + "/News/Default.aspx\";\n</script>", false);
                    
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
