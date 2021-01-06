using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using System.Collections.Generic;

namespace GMSWeb
{
    public class PageHandler : GMSBasePage
    {
        string appPath = HttpRuntime.AppDomainAppVirtualPath;

        public string LoadTabs()
        {
            return "";
        }

        public string LoadTabs(string currentLink)
        {
            LogSession session = base.GetSessionInfo();
            List<short> lstModuleCategory = session.UserAccessModuleCategory;
            string inner = "";
            string address = "";
            string currentLinkLI = "<li class=\"selected\">";

            if (session.DivisionId == 1)
            {
                //address = "<a href=\"" + appPath + "/News/\" id=\"lnkNEWS\"><span>What's New</span></a></li>";
                //if (currentLink == "News") inner += currentLinkLI + address;
                //else inner += "<li>" + address; 

                if (lstModuleCategory.Contains(3))
                {
                    address = "<a href=\"" + appPath + "/SysHR/\" id=\"lnkHR\"><span>HR</span></a></li>";
                    if (currentLink == "HR") inner += "<li class=\"selected red\">" + address;
                    else inner += "<li class='red' >" + address;
                }
                if (lstModuleCategory.Contains(2))
                {
                    address = "<a href=\"" + appPath + "/SysFinance/\" id=\"lnkFINANCE\"><span>Finance</span></a></li>";
                    if (currentLink == "Finance") inner += "<li class=\"selected green\">" + address;
                    else inner += "<li class='green'>" + address;
                }
                if (lstModuleCategory.Contains(5))
                {
                    address = "<a href=\"" + appPath + "/Admin/\" id=\"lnkADMIN\"><span>Admin</span></a></li>";
                    if (currentLink == "Admin") inner += "<li class=\"selected blue\">" + address;
                    else inner += "<li class='blue'>" + address;
                }
                //inner += "<li><a class=\"lastcorner\" href=\"" + appPath + "/Logout.aspx\" id=\"lnkLogout\" target=\"_top\"><span>Logout</span></a></li>";
            }
            else
            {
                //address = "<a href=\"" + appPath + "/News/\" id=\"lnkNEWS\"><span>What's New</span></a></li>";
                //if (currentLink == "News") inner += currentLinkLI + address;
                //else inner += "<li>" + address; 

                if (lstModuleCategory.Contains(8))
                {
                    address = "<a href=\"" + appPath + "/Sales/\" id=\"lnkSALES\"><span>Sales</span></a></li>";
                    if (currentLink == "Sales") inner += "<li class=\"selected green\">" + address;
                    else inner += "<li class='green'>" + address;
                }
                /*
				if (lstModuleCategory.Contains(9))
				{
					address = "<a href=\"" + appPath + "/Debtors/\" id=\"lnkDebtors\"><span>Debtors</span></a></li>";
					if (currentLink == "Debtors") inner += currentLinkLI + address;
					else inner += "<li>" + address;
				}*/
                if (lstModuleCategory.Contains(7))
                {
                    address = "<a href=\"" + appPath + "/Products/\" id=\"lnkPRODUCTS\"><span>Products</span></a></li>";
                    if (currentLink == "Products") inner += "<li class=\"selected orange\">" + address;
                    else inner += "<li class='orange'>" + address;
                }/*
				if (lstModuleCategory.Contains(6))
				{
					address = "<a href=\"" + appPath + "/Suppliers/\" id=\"lnkSUPPLIERS\"><span>Suppliers</span></a></li>";
					if (currentLink == "Suppliers") inner += currentLinkLI + address;
					else inner += "<li>" + address;
				}*/
                 //if (lstModuleCategory.Contains(6))
                 //{
                 //    address = "<a href=\"" + appPath + "/Suppliers/\" id=\"lnkSuppliers\"><span>Suppliers</span></a></li>";
                 //    if (currentLink == "Suppliers") inner += currentLinkLI + address;
                 //    else inner += "<li>" + address; 
                 //}                
                if (lstModuleCategory.Contains(10))
                {
                    address = "<a href=\"" + appPath + "/Logistic/\" id=\"lnkFinance\"><span>Logistic</span></a></li>";
                    if (currentLink == "CompanyLogistic") inner += "<li class=\" selected\">" + address;
                    else inner += "<li class='red'>" + address;
                }
                if (lstModuleCategory.Contains(10))
                {
                    address = "<a href=\"" + appPath + "/Finance/\" id=\"lnkFinance\"><span>Finance</span></a></li>";
                    if (currentLink == "CompanyFinance") inner += "<li class=\"selected blue\">" + address;
                    else inner += "<li class='blue'>" + address;
                }

                if (lstModuleCategory.Contains(11))
                {
                    address = "<a href=\"" + appPath + "/HR/\" id=\"lnkHR\"><span>HR</span></a></li>";
                    if (currentLink == "CompanyHR") inner += "<li class=\"selected purple\">" + address;
                    else inner += "<li class='purple'>" + address;
                }

                if (lstModuleCategory.Contains(22))
                {
                    address = "<a href=\"" + appPath + "/Procurement/\" id=\"lnkProcurement\"><span>Procurement</span></a></li>";
                    if (currentLink == "Procurement") inner += "<li class=\"selected purple\">" + address;
                    else inner += "<li class='purple'>" + address;
                }
                //if (lstModuleCategory.Contains(4))
                //{
                //    address = "<a href=\"" + appPath + "/UsefulResources/\" id=\"lnkUR\"><i class='ti-bar-chart'></i> <span>Resources</span></a></li>";
                //    if (currentLink == "UsefulResources") inner += "<li class=\"selected purple\">" + address;
                //	else inner += "<li class='purple'>" + address;

                //	address = "<a href=\"https://leedennox.atlassian.net/servicedesk/customer/portal/4\" target=\"_blank\"><span>Issues</span></a></li>";
                //	if (currentLink == "Issue") inner += currentLinkLI + address;
                //	else inner += "<li>" + address;
                //}
                //inner += "<li><a class=\"lastcorner\" href=\"" + appPath + "/Logout.aspx\" id=\"lnkLogout\"><span>Logout</span></a></li>";
            }
            return inner;
        }

        public string LoadSidebar(string currentLink)
        {
            LogSession session = base.GetSessionInfo();
            List<short> lstModuleCategory = session.UserAccessModuleCategory;
            string sideMenu = string.Empty;

            if (currentLink == "HR")
            {
                sideMenu =
                "<li class=\"has-sub list-of-people-menu\">" +
                    "<a href='javascript:;'> " +
                        "<i class='ti-user'></i> " +
                        "<span>List of People</span>" +
                        "<ul class=\"sub-menu\">" +
                            "<li class='sub-staff'><a href=\"" + appPath + "/SysHR/Staff/Staff.aspx?TYPE=HR\">Staff</a></li>" +
                            "<li class='sub-add-staff'><a href=\"" + appPath + "/SysHR/Staff/AddNewStaff.aspx?TYPE=HR\">Add New Staff</a></li>" +
                        "</ul>" +
                    "</a>" +
                "</li>" +
                "<li class=\"has-sub training-menu\">" +
                    "<a href='javascript:;'> " +
                        "<i class='ti-rocket'></i> " +
                        "<span>Training</span>" +
                        "<ul class=\"sub-menu\">" +
                            "<li class='sub-organizer'><a href=\"" + appPath + "/SysHR/Training/Organizer.aspx\">Organizer</a></li>" +
                            "<li class='sub-course'><a href=\"" + appPath + "/SysHR/Training/Course.aspx\">Course</a></li>" +
                            "<li class='sub-course-session'><a href=\"" + appPath + "/SysHR/Training/CourseSession.aspx\">Session</a></li>" +
                            "<li class='sub-employee-course'><a href=\"" + appPath + "/SysHR/Training/EmployeeCourse.aspx\">Records</a></li>" +
                        "</ul>" +
                    "</a>" +
                "</li>" +
                "<li class=\"has-sub reports-menu\">" +
                    "<a href='javascript:;'> " +
                        "<i class='ti-stats-up'></i> " +
                        "<span>Reports</span>" +
                        "<ul class=\"sub-menu\">" +
                            "<li class='sub-view-reports'><a href=\"" + appPath + "/SysHR/Reports/SystemGenerated.aspx\">View Reports</a></li>" +
                        "</ul>" +
                    "</a>" +
                "</li>";
            }
            else if (currentLink == "Finance")
            {
                sideMenu =
                "<li class=\"has-sub reports-menu\">" +
                    "<a href='javascript:;'> " +
                        "<i class='ti-stats-up'></i> " +
                        "<span>Reports</span>" +
                        "<ul class=\"sub-menu\">" +
                            "<li class='sub-bank-report'><a href=\"" + appPath + "/SysFinance/Reports/BankReport.aspx\">Reports</a></li>" +
                        "</ul>" +
                    "</a>" +
                "</li>" +
                    "<li class=\"has-sub administration-menu \">" +
                    "<a href='javascript:;'> " +
                        "<i class='ti-ruler-pencil'></i> " +
                        "<span>Admin</span>" +
                        "<ul class=\"sub-menu\">" +
                            "<li class='sub-exporter'><a href=\"" + appPath + "/SysFinance/Administration/Exporter.aspx\">Exporter</a></li>" +
                            "<li class='sub-bank-info'><a href=\"" + appPath + "/SysFinance/BankFacilities/BankInfo.aspx\">Bank Info</a></li>" +
                        "</ul>" +
                    "</a>" +
                "</li>";
            }
            else if (currentLink == "Admin")
            {
                sideMenu =
                    /*
                    "<h1>System</h1>" + 
                    "<ul class=\"sidemenu\">" +
                        "<li><a href=\"" + appPath + "/Admin/SystemData/Country.aspx\">Country</a></li>" +
                        "<li><a href=\"" + appPath + "/Admin/SystemData/DivisionData.aspx\">Division</a></li>" +
                        "<li><a href=\"" + appPath + "/Admin/SystemData/CompanyData.aspx\">Company</a></li>" +
                        "<li><a href=\"" + appPath + "/Admin/SystemData/ItemListing.aspx\">Item</a></li>" +
                        "<li><a href=\"" + appPath + "/Admin/SystemData/ItemStructureListing.aspx\">Item Structure</a></li>" +
                        "<li><a href=\"" + appPath + "/Admin/SystemData/CurrencyListing.aspx\">Currency</a></li>" +
                    "</ul>" +
                    "<h1>Company Reports</h1>" +
                    "<ul class=\"sidemenu\">" +
                        "<li><a href=\"" + appPath + "/Admin/CompanyReports/Category.aspx\">Category</a></li>" +
                        "<li><a href=\"" + appPath + "/Admin/CompanyReports/Modify.aspx\">Modify</a></li>" +
                        "<li><a href=\"" + appPath + "/Admin/CompanyReports/Upload.aspx\">Upload</a></li>" +
                    "</ul>" +
                    "<h1>Module Reports</h1>" +
                    "<ul class=\"sidemenu\">" +
                        "<li><a href=\"" + appPath + "/Admin/ModuleReports/Category.aspx\">Category</a></li>" +
                    "</ul>" +
                     */
                    "<li class=\"has-sub account-menu\">" +
                        "<a href='javascript:;'> " +
                            "<i class='ti-user'></i> " +
                            "<span>Accounts</span>" +
                            "<ul class=\"sub-menu\">" +
                                "<li class='sub-users'><a href=\"" + appPath + "/Admin/Accounts/Users.aspx\">Users</a></li>" +
                                //"<li><a href=\"" + appPath + "/Admin/Accounts/ChangePassword.aspx\">Change Password</a></li>" +
                                "<li class='sub-salesman-map'><a href=\"" + appPath + "/Admin/Accounts/SalesPersonMapping.aspx\">Salesmen Map</a></li>" +
                            "</ul>" +
                        "</a>" +
                    "</li>";
            }
            else if (currentLink == "Sales")
            {
                sideMenu =
                 //    "<li class='has-sub report-menu'>" +
                 //    "<a href='javascript:;'>" +
                 //    "<i class='ti-stats-up'></i>" +
                 //    "<span>Report</span>" +
                 //    "</a>" +
                 //    "<ul class=\"sub-menu\">" +
                 //    "<li class='sub-sales'><a href=\"" + appPath + "/Sales/Reports/View.aspx\">Sales</a></li>" +
                 //    "<li class='sub-debtors'><a href=\"" + appPath + "/Debtors/Reports/View.aspx\">Debtors</a></li>" +
                 //    "<li class='sub-financials'><a href=\"" + appPath + "/Finance/Reports/FinanceReport.aspx?CurrentLink=Sales\">Financials</a></li>" +
                 //    "<li class='sub-budget'><a href=\"" + appPath + "/Finance/Reports/FinanceBudgetReport.aspx?CurrentLink=Sales\">Budget</a></li>" +
                 //    "</ul>" +
                 //    "</li>" +
                 //    "<li class='has-sub sale-menu'>" +
                 //    "<a href='javascript:;'>" +
                 //    "<i class='ti-bar-chart'></i>" +
                 //    "<span>Sales</span>" +
                 //    "</a>" + 
                 //    "<ul class=\"sub-menu\">" +
                 //    "<li class='sub-quotation'><a href=\"" + appPath + "/Sales/Sales/QuotationSearch.aspx\">Quotation</a></li>" +
                 //    "<li class='sub-sales-order'><a href=\"" + appPath + "/Sales/Sales/SalesOrderSearch.aspx\">Sales Order</a></li>" +
                 //    "<li class='sub-delivery-order'><a href=\"" + appPath + "/Sales/Sales/DeliveryOrderSearch.aspx\">Delivery Order</a></li>" +
                 //    "<li class='sub-invoice'><a href=\"" + appPath + "/Sales/Sales/Invoice.aspx\">Invoice</a></li>" +
                 //    "<li class='sub-sales-detail'><a href=\"" + appPath + "/Sales/Sales/SalesSearch.aspx\">Sales Detail</a></li>" +
                 //    "</ul>" +
                 //    "</li>" +
                 //    "<li class='has-sub project-menu'>" +
                 //    "<a href='javascript:;'>" +
                 //    "<i class='ti-briefcase'></i>" +
                 //    "<span>Project</span>" +
                 //    "</a>" +
                 //    "<ul class=\"sub-menu\">" +
                 //    "<li class='sub-material'><a href=\"" + appPath + "/Sales/Engineering/Material/MaterialSearch.aspx\">Material</a></li>" +
                 //    "<li class='sub-cost-estimate'><a href=\"" + appPath +
                 //    "/Sales/Engineering/CostEstimate/CostEstimateSearch.aspx\">Cost Estimate</a></li>" +
                 //    "<li class='sub-project-search'><a href=\"" + appPath + "/Sales/Engineering/Project/ProjectSearch.aspx\">Project</a></li>" +
                 //    "</ul>" +
                 //    "</li>" +
                 //    "<li class='has-sub products-menu'>" +
                 //    "<a href='javascript:;'>" +
                 //    "<i class='ti-layout-grid2'></i>" +
                 //    "<span>Products</span>" +
                 //    "</a>" +
                 //    "<ul class=\"sub-menu\">" +
                 //    "<li class='sub-product-search-sale'><a href=\"" + appPath +
                 //    "/Products/Products/ProductsSearch.aspx?CurrentLink=Sales\">Product Search</a></li>" +
                 //    "<li class='sub-product-movement'><a href=\"" + appPath +
                 //    "/Products/Products/ProductMovement.aspx?CurrentLink=Sales\">Product Movement</a></li>" +
                 //    "<li class='sub-material-req'><a href=\"" + appPath +
                 //    "/Products/Products/MaterialRequisitionDivision.aspx?CurrentLink=Sales\">Material Req</a></li>" +
                 //    //"<li><a href=\"" + appPath + "/UnderConstruction.aspx?ModuleID=Sales\">Product Loan</a></li>" +                
                 //    "</ul>" +
                 //    "</li>" +
                 //    "<li class='has-sub customer-info-menu'>" +
                 //    "<a href='javascript:;'>" +
                 //    "<i class='ti-id-badge'></i>" +
                 //    "<span>Customer Info</span>" +
                 //    "</a>" +
                 //    "<ul class=\"sub-menu\">" +
                 //    "<li class='sub-debtors-search'><a href=\"" + appPath + "/Debtors/Debtors/DebtorsSearch.aspx\">Customer Detail</a></li>" +
                 //    "<li class='sub-debtors-search-all'><a href=\"" + appPath + "/Debtors/Debtors/DebtorsSearchAll.aspx\">Search</a></li>" +
                 //    "<li class='sub-commentary'><a href=\"" + appPath + "/Debtors/Commentary/Commentary.aspx\">Overdue Debts</a></li>" +
                 //    "<li class='sub-finance-search'><a href=\"" + appPath + "/Debtors/Debtors/FinanceSearch.aspx\">Finance Search</a></li>" +
                 //    "</ul>" +
                 //    "</li>" +
                 //    "<li class='has-sub administration-menu'>" +
                 //    "<a href='javascript:;'>" +
                 //    "<i class='ti-ruler-pencil'></i>" +
                 //    "<span>Administration</span>" +
                 //    "</a>" +
                 //    "<ul class=\"sub-menu\">" +
                 //    // "<li class='sub-upload'><a href=\"" + appPath + "/Sales/Sales/Upload.aspx?CurrentLink=Sales\">Upload</a></li>" +
                 //    // "<li class='sub-importer'><a href=\"" + appPath + "/Sales/Sales/Import.aspx?CurrentLink=Sales\">Importer</a></li>" +
                 //    "<li class='sub-claim'><a href=\"" + appPath + "/Claim/Default.aspx\">Claim</a></li>" +
                 //    "<li class='sub-salesman-detail'><a href=\"" + appPath + "/Sales/Sales/Salesman.aspx\">Salesman Detail</a></li>" +
                 //    "<li class='sub-forms'><a href=\"" + appPath +
                 //    "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Administration &gt; Forms&PageTitle=Sales - Forms&ModuleCategoryID=8&ModuleCategoryName=Sales\">Forms</a></li>" +
                 //    // "<li class='sub-package-costing'><a href=\"" + appPath + "/Sales/Sales/SalesBreakdown.aspx\">Package Costing</a></li>" +
                 //    "</ul>";

                 ////if (lstModuleCategory.Contains(4))
                 ////{
                 //sideMenu +=
                 //"</li>" +
                 // "<li class='has-sub resources-menu'>" +
                 //"<a href='javascript:;'>" +
                 //"<i class='ti-package'></i>" +
                 //"<span>Resources</span>" +
                 //"</a>" +
                 //"<ul class=\"sub-menu\">" +
                 //"<li class='sub-resource-useful-resource'><a href=\"" + appPath + "/UsefulResources/Resources/ViewUsefulResources.aspx?PageHeader=Resources &gt; Resources&PageTitle=Resources&ModuleCategoryID=8&ModuleCategoryName=Sales\">Useful Resources</a></li>" +
                 //"</ul>" +
                 //"</li>";
                 ////}

                 //New


                 "<li class='has-sub report-menu'>" +
                    "<a href=\"" + appPath + "/Sales/Reports/View.aspx\">" +
                    "<i class='ti-stats-up'></i>" +
                    "<span>Sales Reports</span>" +
                    "</a>" +
                   "</li>" +

                    "<li class='has-sub sale-menu'>" +
                    "<a href='javascript:;'>" +
                    "<i class='ti-bar-chart'></i>" +
                    "<span>Transactions</span>" +
                    "</a>" +
                    "<ul class=\"sub-menu\">" +
                    "<li class='sub-quotation'><a href=\"" + appPath + "/Sales/Sales/QuotationSearch.aspx\">Quotation</a></li>" +
                    "<li class='sub-sales-order'><a href=\"" + appPath + "/Sales/Sales/SalesOrderSearch.aspx\">Sales Order</a></li>" +
                    "<li class='sub-delivery-order'><a href=\"" + appPath + "/Sales/Sales/DeliveryOrderSearch.aspx\">Delivery Order</a></li>" +
                    "<li class='sub-invoice'><a href=\"" + appPath + "/Sales/Sales/Invoice.aspx\">Invoice</a></li>" +
                    "<li class='sub-sales-detail'><a href=\"" + appPath + "/Sales/Sales/SalesSearch.aspx\">Sales Detail</a></li>" +
                    "</ul>" +
                    "</li>" +

                     "<li class='has-sub customer-info-menu'>" +
                    "<a href='javascript:;'>" +
                    "<i class='ti-id-badge'></i>" +
                    "<span>Customer Info</span>" +
                    "</a>" +
                    "<ul class=\"sub-menu\">" +
                    "<li class='sub-debtors-search'><a href=\"" + appPath + "/Debtors/Debtors/DebtorsSearch.aspx\">Customer Detail</a></li>" +
                    "<li class='sub-debtors-search-all'><a href=\"" + appPath + "/Debtors/Debtors/DebtorsSearchAll.aspx\">Search</a></li>" +
                    "<li class='sub-commentary'><a href=\"" + appPath + "/Debtors/Commentary/Commentary.aspx\">Overdue Debts</a></li>" +
                    "<li class='sub-finance-search'><a href=\"" + appPath + "/Debtors/Debtors/FinanceSearch.aspx\">Finance Search</a></li>" +
                    "</ul>" +
                    "</li>" +

                     "<li class='has-sub products-menu'>" +
                    "<a href='javascript:;'>" +
                    "<i class='ti-layout-grid2'></i>" +
                    "<span>Products Info</span>" +
                    "</a>" +
                    "<ul class=\"sub-menu\">" +
                    "<li class='sub-product-search-sale'><a href=\"" + appPath +
                    "/Products/Products/ProductsSearch.aspx?CurrentLink=Sales\">Product Search</a></li>" +
                    "<li class='sub-product-movement'><a href=\"" + appPath +
                    "/Products/Products/ProductMovement.aspx?CurrentLink=Sales\">Product Movement</a></li>" +
                    "<li class='sub-material-req'><a href=\"" + appPath +
                    "/Products/Products/MaterialRequisitionDivision.aspx?CurrentLink=Sales\">Material Req</a></li>" +
                    //"<li><a href=\"" + appPath + "/UnderConstruction.aspx?ModuleID=Sales\">Product Loan</a></li>" +                
                    "</ul>" +
                    "</li>" +

                     "<li class='sub-pricelist'>" +
                    "<a href=\"" + appPath + "/Sales/Sales/PriceList.aspx?CurrentLink=Sales\">" +
                    "<i class='ti-money'></i>" +
                    "<span>Price List</span>" +
                    "</a>" +
                    "</li>" +

                    "<li class='sub-latestpricelist'>" +
                    "<a href=\"" + appPath + "/Sales/Sales/LatestPriceList.aspx?CurrentLink=Sales&PageHeader=Sales&PageTitle=Latest%20Price%20List&ModuleCategoryID=8&ModuleCategoryName=Sales&isPriceList=yes\">" +
                    "<i class='ti-shopping-cart'></i>" +
                    "<span>Latest Price List</span>" +
                    "</a>" +
                    "</li>" +

                 "<li class='has-sub debtors-menu'>" +
                    "<a href =\"" + appPath + "/Debtors/Reports/View.aspx\">" +
                    "<i class='ti-credit-card'></i>" +
                    "<span>Debtors</span>" +
                    "</a>" +
                   "</li>" +

                    "<li class='has-sub project-menu'>" +
                    "<a href='javascript:;'>" +
                    "<i class='ti-briefcase'></i>" +
                    "<span>Project</span>" +
                    "</a>" +
                    "<ul class=\"sub-menu\">" +
                    "<li class='sub-material'><a href=\"" + appPath + "/Sales/Engineering/Material/MaterialSearch.aspx\">Material</a></li>" +
                    "<li class='sub-cost-estimate'><a href=\"" + appPath +
                    "/Sales/Engineering/CostEstimate/CostEstimateSearch.aspx\">Cost Estimate</a></li>" +
                    "<li class='sub-project-search'><a href=\"" + appPath + "/Sales/Engineering/Project/ProjectSearch.aspx\">Project</a></li>" +
                    "</ul>" +
                    "</li>" +

                    "<li class='has-sub budget-menu'>" +
                    "<a href=\"" + appPath + "/Sales/Reports/SalesBudgetReport.aspx\">" +
                    "<i class='ti-receipt'></i>" +
                    "<span>Budget</span>" +
                    "</a>" +
                   "</li>" +

                    "<li class='has-sub administration-menu'>" +
                    "<a href='javascript:;'>" +
                    "<i class='ti-ruler-pencil'></i>" +
                    "<span>Admin</span>" +
                    "</a>" +
                    "<ul class=\"sub-menu\">" +
                    // "<li class='sub-upload'><a href=\"" + appPath + "/Sales/Sales/Upload.aspx?CurrentLink=Sales\">Upload</a></li>" +
                    // "<li class='sub-importer'><a href=\"" + appPath + "/Sales/Sales/Import.aspx?CurrentLink=Sales\">Importer</a></li>" +
                    "<li class='sub-claim'><a href=\"" + appPath + "/Claim/Default.aspx\">Claim</a></li>" +
                    "<li class='sub-salesman-detail'><a href=\"" + appPath + "/Sales/Sales/Salesman.aspx\">Salesman Detail</a></li>" +
                    "<li class='sub-forms'><a href=\"" + appPath +
                    "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Administration &gt; Forms&PageTitle=Sales - Forms&ModuleCategoryID=8&ModuleCategoryName=Sales\">Forms</a></li>" +
                    "<li class='sub-team-setup'><a href=\"" + appPath + "/Sales/Sales/TeamSetup.aspx\">Team Setup</a></li>" +
                    "<li class='sub-customertype-setup'><a href=\"" + appPath + "/Sales/Sales/CustomerTypeSetup.aspx\">Customer Type Setup</a></li>" +
                    // "<li class='sub-package-costing'><a href=\"" + appPath + "/Sales/Sales/SalesBreakdown.aspx\">Package Costing</a></li>" +
                    "</ul>";

                //if (lstModuleCategory.Contains(4))
                //{
                sideMenu +=
                "</li>" +
                 "<li class='has-sub resources-menu'>" +
                "<a href='javascript:;'>" +
                "<i class='ti-package'></i>" +
                "<span>Resources</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">" +
                "<li class='sub-resource-useful-resource'><a href=\"" + appPath + "/UsefulResources/Resources/ViewUsefulResources.aspx?PageHeader=Resources &gt; Resources&PageTitle=Resources&ModuleCategoryID=8&ModuleCategoryName=Sales\">Useful Resources</a></li>" +
                "</ul>" +
                "</li>";
                //}

            }
            else if (currentLink == "CompanyHR")
            {
                sideMenu =

                "<li class='has-sub hr-menu'>" +
                "<a href='javascript:;'>" +
                "<i class='ti-id-badge'></i>" +
                "<span>HR Organisation</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">" +
                "<li class='sub-hr'><a href=\"" + appPath + "/UsefulResources/Resources/ViewResources.aspx?PageHeader=HR Organisation &gt; Organisation Chart&PageTitle=Staff - Organisation Chart&ModuleCategoryID=18&ModuleCategoryName=CompanyHR\">Org Chart</a></li>" +
                "<li class='sub-staff'><a href=\"" + appPath + "/SysHR/Staff/Staff.aspx?TYPE=CompanyHR\">Employee List</a></li>" +
                "<li class='sub-add-staff'><a href=\"" + appPath + "/SysHR/Staff/AddNewStaff.aspx?TYPE=CompanyHR\">Add Employee</a></li>" +
                "<li class='sub-update-photo'><a href=\"" + appPath + "/SysHR/Staff/PhotoIDUpdate.aspx?TYPE=CompanyHR\">Photo ID Update</a></li>" +
                "</ul>" +
                "</li>" +

                "<li class='has-sub pmp-menu'>" +
                "<a href='javascript:;'>" +
                "<i class='ti-book'></i>" +
                "<span>PMP</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">" +
                "<li class='sub-pmp'><a href=\"" + appPath + "/HR/PMP/PMPHome.aspx?PageHeader=PMP &gt; PMP&PageTitle=Sales - PMP&ModuleCategoryID=21&ModuleCategoryName=CompanyHR\">PMP</a></li>" +
                "</ul>" +
                "</li>" +

                "<li class='has-sub incentive-menu'>" +
                "<a href='javascript:;'>" +
                "<i class='ti-gift'></i>" +
                "<span>Incentive</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">" +
                "<li class='sub-commission'><a href=\"" + appPath + "/HR/Commission/CommissionNGPQ.aspx\">Setup</a></li>" +
                "<li class='sub-monthly'><a href=\"" + appPath + "/HR/Commission/MonthlyRecord.aspx\">Records</a></li>" +
                "<li class='sub-entertainment-expense'><a href=\"" + appPath + "/HR/Commission/EntertainmentExpense.aspx\">Entertainment</a></li>" +
                "<li class='sub-key-customer'><a href=\"" + appPath + "/HR/Commission/KeyCustomers.aspx\">Key Customers</a></li>" +
                "</ul>" +
                "</li>" +

                "<li class='has-sub training-menu'>" +
                "<a href='javascript:;'>" +
                "<i class='ti-rocket'></i>" +
                "<span>Training</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">" +
                "<li class='sub-organizer'><a href=\"" + appPath + "/HR/Training/Organizer.aspx?ModuleID=CompanyHR\">Organizer</a></li>" +
                "<li class='sub-course'><a href=\"" + appPath + "/HR/Training/Course.aspx?ModuleID=CompanyHR\">Course</a></li>" +
                "<li class='sub-session'><a href=\"" + appPath + "/HR/Training/Session.aspx?ModuleID=CompanyHR\">Session</a></li>" +
                "<li class='sub-records'><a href=\"" + appPath + "/HR/Training/Records.aspx?ModuleID=CompanyHR\">Records</a></li>" +
                "</ul>" +
                "</li>" +
                    "<li class='has-sub administration-menu'>" +
                    "<a href='javascript:;'>" +
                    "<i class='ti-ruler-pencil'></i>" +
                    "<span>Admin</span>" +
                    "</a>" +
                    "<ul class=\"sub-menu\">" +
                        "<li class='sub-claim'><a href=\"" + appPath + "/Claim/Default.aspx?CurrentLink=CompanyHR\">Claim</a></li>" +
                    "</ul>" +
                    "</li>" +

                    "<li class='has-sub recruitment-menu'>" +
                    "<a href=\"" + appPath + "/HR/Recruitment/Recruitment.aspx?CurrentLink=CompanyHR\">" +
                    "<i class='ti-user'></i>" +
                    "<span>Recruitment</span>" +
                    "</a>" +
                    "</li>" +

                /*"<li class='has-sub recruitment-menu'>" +
                "<a href='javascript:;'>" +
                "<span>Recruitment</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">" +
                    "<li class='sub-resume'><a href=\"" + appPath + "/HR/Recruitment/Recruitment.aspx\">Recruitment</a></li>" +
                "</ul>" +
                "</li>" +*/

                //"<li class='has-sub'>" +
                //"<a href='javascript:;'>" +
                //"<i class='ti-package'></i>" +
                //"<span>Administration</span>" +
                //"</a>" +
                //"<ul class=\"sub-menu\">" +
                //"<li><a href=\"" + appPath + "/SysHR/Upload/ImporterExporter.aspx\">Importer</a></li>" +
                // "</ul>" +
                //"</li>" +

                "<li class='has-sub resources-menu'>" +
                "<a href='javascript:;'>" +
                "<i class='ti-package'></i>" +
                "<span>Resources</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">";
                //if (lstModuleCategory.Contains(4))
                sideMenu += "<li class=' sub-resource-useful-resource'><a href=\"" + appPath + "/UsefulResources/Resources/ViewUsefulResources.aspx?PageHeader=Resources &gt; Resources&PageTitle=HR - Resources&ModuleCategoryID=11&ModuleCategoryName=CompanyHR\">Useful Resources</a></li>";

                sideMenu +=
                "<li class=' sub-resource-resource'><a href=\"" + appPath + "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Resources &gt; Resources&PageTitle=HR - Resources&ModuleCategoryID=11&ModuleCategoryName=CompanyHR\">Resources</a></li>" +
                "</ul>" +
                "</li>";

            }/*
			else if (currentLink == "Suppliers")
			{
				return
				"<h1>Reports</h1>" +
				"<ul class=\"sidemenu\">" +
				"<li><a href=\"" + appPath + "/Suppliers/Reports/View.aspx\">View Reports</a></li>" +
				"</ul>"; 
			}*/
            else if (currentLink == "Debtors")
            {
                sideMenu =
                "<h1>Debtors</h1>" +
                "<ul class=\"sidemenu\">" +
                "<li><a href=\"" + appPath + "/Debtors/Debtors/DebtorsSearch.aspx\">Search</a></li>" +
                "<li><a href=\"" + appPath + "/Debtors/Debtors/DebtorsSearchAll.aspx\">Search All</a></li>" +
                "</ul>" +
                "<h1>Commentary</h1>" +
                "<ul class=\"sidemenu\">" +
                "<li><a href=\"" + appPath + "/Debtors/Commentary/Commentary.aspx\">Commentary</a></li>" +
                "</ul>" +
                "<h1>Reports</h1>" +
                "<ul class=\"sidemenu\">" +
                "<li><a href=\"" + appPath + "/Debtors/Reports/View.aspx\">View Reports</a></li>" +
                "</ul>";
            }
            else if (currentLink == "Products")
            {
                sideMenu =
                    "<li class='has-sub products-menu'>" +
                    "<a href=\"" + appPath +"/Products/Products/ProductsSearch.aspx?CurrentLink=Products\">" +
                    "<i class='ti-layout-grid2'></i>" +
                    "<span>Product Info</span>" +
                    "</a>" +
                    "</li>" +

                    "<li class='has-sub pricing-menu'>" +
                    "<a href='javascript:;'>" +
                    "<i class='ti-money'></i>" +
                    "<span>Pricing</span>" +
                    "</a>" +
                    "<ul class=\"sub-menu\">" +
                        "<li class='sub-radpricelist'><a href=\"" + appPath + "/Sales/Sales/RadPriceList.aspx?CurrentLink=Products\">Price Input</a></li>" +
                         
                         "<li class='sub-products'><a href=\"" + appPath + "/Products/Reports/ViewPricing.aspx\">Reports</a></li>" +
                         "<li class='sub-pricelist'><a href=\"" + appPath + "/Sales/Sales/PriceList.aspx?CurrentLink=Products\">Price List Upload</a></li>" +
                    "</ul>" +
                    "</li>" +

                    "<li class='has-sub report-menu'>" +
                    "<a href='javascript:;'>" +
                    "<i class='ti-stats-up'></i>" +
                    "<span>Reports</span>" +
                    "</a>" +
                    "<ul class=\"sub-menu\">" +
                        "<li class='sub-sales'><a href=\"" + appPath + "/Sales/Reports/View.aspx?CurrentLink=Products\">Sales</a></li>" +
                        "<li class='sub-products'><a href=\"" + appPath + "/Products/Reports/View.aspx\">Products</a></li>" +
                        "<li class='sub-financials'><a href=\"" + appPath + "/Finance/Reports/FinanceReport.aspx?CurrentLink=Products\">Financials</a></li>" +
                    "</ul>" +
                    "</li>" +

                    "<li class='has-sub products-menu'>" +
                    "<a href='javascript:;'>" +
                    "<i class='ti-layout-grid2'></i>" +
                    "<span>Products</span>" +
                    "</a>" +
                    "<ul class=\"sub-menu\">" +
                        "<li class='sub-product-movement'><a href=\"" + appPath +
                        "/Products/Products/ProductMovement.aspx?CurrentLink=Products\">Product Movement</a></li>" +
                        "<li class='sub-sup-in-detail'><a href=\"" + appPath + "/Products/Products/ProductPurchase.aspx\">Sup Inv Detail</a></li>" +
                        "<li class='sub-material-req'><a href=\"" + appPath +
                        "/Products/Products/MaterialRequisitionDivision.aspx?CurrentLink=Products\">Material Req</a></li>" +
                        "<li class='sub-GRNSearch'><a href=\"" + appPath + "/Products/Products/GRNSearch.aspx?CurrentLink=Products\">GRN Search</a></li>" +
                    "</ul>" +
                    "</li>" +
                    "<li class='has-sub sale-menu'>" +
                    "<a href='javascript:;'>" +
                    "<i class='ti-bar-chart'></i>" +
                    "<span>Sales</span>" +
                    "</a>" +
                    "<ul class=\"sub-menu\">" +
                        "<li class='sub-sales-detail'><a href=\"" + appPath + "/Sales/Sales/SalesSearch.aspx?CurrentLink=Products\">Sales Detail</a></li>" +
                    "</ul>" +
                    "</li>" +
                    "<li class='has-sub customer-info-menu'>" +
                    "<a href='javascript:;'>" +
                    "<i class='ti-id-badge'></i>" +
                    "<span>Customer Info</span>" +
                    "</a>" +
                    "<ul class=\"sub-menu\">" +
                        "<li class='sub-debtors-search'><a href=\"" + appPath + "/Debtors/Debtors/DebtorsSearchAll.aspx?CurrentLink=Products\">Search</a></li>" +
                    "</ul>" +
                    "</li>" +
                    "<li class='has-sub administration-menu'>" +
                    "<a href='javascript:;'>" +
                    "<i class='ti-ruler-pencil'></i>" +
                    "<span>Admin</span>" +
                    "</a>" +
                    "<ul class=\"sub-menu\">" +
                        "<li class='sub-upload'><a href=\"" + appPath + "/Sales/Sales/Upload.aspx?CurrentLink=Products\">Upload</a></li>" +
                        "<li class='sub-forms'><a href=\"" + appPath +
                        "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Administration &gt; Forms&PageTitle=Products - Forms&ModuleCategoryID=7&ModuleCategoryName=Products\">Forms</a></li>" +
                        //"<li class='sub-pricelist'><a href=\"" + appPath + "/Sales/Sales/PriceList.aspx?CurrentLink=Products\">Price List</a></li>" +
                        "<li class='sub-sales-package'><a href=\"" + appPath + "/Sales/Sales/SalesPackage.aspx\">Sales Package</a></li>" +
                        "<li class='sub-claim'><a href=\"" + appPath + "/Claim/Default.aspx?CurrentLink=Products\">Claim</a></li>" +
                        "<li class='sub-productgroup-setup'><a href=\"" + appPath + "/Products/Products/ProductGroupSetup.aspx\">Product Group Setup</a></li>" +
                        "<li class='sub-product-setup'><a href=\"" + appPath + "/Products/Products/ProductSetup.aspx\">Product Setup</a></li>" +
                    "</ul>" +
                    "</li>" +
                    "</li>";

                //if (lstModuleCategory.Contains(4))
                // {
                sideMenu +=
                "<li class='has-sub resources-menu'>" +
                "<a href='javascript:;'>" +
                "<i class='ti-package'></i>" +
                "<span>Resources</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">" +
                "<li class='sub-resource-useful-resource'><a href=\"" + appPath + "/UsefulResources/Resources/ViewUsefulResources.aspx?PageHeader=Resources &gt; Resources&PageTitle=Resources&ModuleCategoryID=7&ModuleCategoryName=Products\">Useful Resources</a></li>" +
                "</ul>" +
                "</li>" +
                "</ul>";
                // }
            }
            else if (currentLink == "CompanyFinance")
            {
                sideMenu =
                 "<li class='has-sub group-menu'>" +
                "<a href=\"" + appPath + "/Finance/Reports/FinanceGroupReport.aspx\">" +
                "<i class='ti-user'></i>" +
                "<span>Group Reports</span>" +
                "</a>" +
                //"<ul class=\"sub-menu\">" +
                //    "<li class='sub-reports'><a href=\"" + appPath + "/Finance/Reports/Reports.aspx\">Reports</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceReport.aspx\">Financials</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceSalesReport.aspx\">Sales & Debtors</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceProductReport.aspx\">Products</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCapexReport.aspx\">Capex</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCashFlowReport.aspx\">Cash Flow</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/CashFlow/CashFlowProjections.aspx\">Cash Flow Proj</a></li>" +
                //"</ul>" +
                "</li>" +

                "<li class='has-sub reports-menu'>" +
                "<a href=\"" + appPath + "/Finance/Reports/Reports.aspx\">" +
                "<i class='ti-money'></i>" +
                "<span>Finance Reports</span>" +
                "</a>" +
                //"<ul class=\"sub-menu\">" +
                //    "<li class='sub-reports'><a href=\"" + appPath + "/Finance/Reports/Reports.aspx\">Trading Accounts</a></li>" +
                //    "<li class='sub-manufacture'><a href=\"" + appPath + "/Finance/Reports/FinanceManufactureAccount.aspx\">Manufacturing Accounts</a></li>" +
                //    "<li class='sub-sales'><a href=\"" + appPath + "/Finance/Reports/FinanceSalesReport.aspx\">Sales & Debtors</a></li>" +
                //    "<li class='sub-purchase'><a href=\"" + appPath + "/Finance/Reports/FinancePurchaseCreditor.aspx\">Purchase & Creditor</a></li>" +
                //    "<li class='sub-product'><a href=\"" + appPath + "/Finance/Reports/FinanceProductReport.aspx\">Inventory</a></li>" +
                //"</ul>" +
                "</li>" +

                 "<li class='has-sub manufacture-menu'>" +
                "<a href=\"" + appPath + "/Finance/Reports/FinanceManufactureAccount.aspx\">" +
                "<i class='ti-server'></i>" +
                "<span>Production Reports</span>" +
                "</a>" +
                //"<ul class=\"sub-menu\">" +
                //    "<li class='sub-reports'><a href=\"" + appPath + "/Finance/Reports/Reports.aspx\">Reports</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceReport.aspx\">Financials</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceSalesReport.aspx\">Sales & Debtors</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceProductReport.aspx\">Products</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCapexReport.aspx\">Capex</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCashFlowReport.aspx\">Cash Flow</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/CashFlow/CashFlowProjections.aspx\">Cash Flow Proj</a></li>" +
                //"</ul>" +
                "</li>" +

                "<li class='has-sub sales-menu'>" +
                "<a href=\"" + appPath + "/Finance/Reports/FinanceSalesReport.aspx\">" +
                "<i class='ti-pulse'></i>" +
                "<span>Sales Reports</span>" +
                "</a>" +
                //"<ul class=\"sub-menu\">" +
                //    "<li class='sub-reports'><a href=\"" + appPath + "/Finance/Reports/Reports.aspx\">Reports</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceReport.aspx\">Financials</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceSalesReport.aspx\">Sales & Debtors</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceProductReport.aspx\">Products</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCapexReport.aspx\">Capex</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCashFlowReport.aspx\">Cash Flow</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/CashFlow/CashFlowProjections.aspx\">Cash Flow Proj</a></li>" +
                //"</ul>" +
                  "</li>" +

                    "<li class='has-sub purchase-menu'>" +
                "<a href=\"" + appPath + "/Finance/Reports/FinancePurchaseCreditor.aspx\">" +
                "<i class='ti-bar-chart'></i>" +
                "<span>Purchase & Creditors</span>" +
                "</a>" +
                //"<ul class=\"sub-menu\">" +
                //    "<li class='sub-reports'><a href=\"" + appPath + "/Finance/Reports/Reports.aspx\">Reports</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceReport.aspx\">Financials</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceSalesReport.aspx\">Sales & Debtors</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceProductReport.aspx\">Products</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCapexReport.aspx\">Capex</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCashFlowReport.aspx\">Cash Flow</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/CashFlow/CashFlowProjections.aspx\">Cash Flow Proj</a></li>" +
                //"</ul>" +
                "</li>" +

                 "<li class='has-sub inventory-menu'>" +
                "<a href=\"" + appPath + "/Finance/Reports/FinanceProductReport.aspx\">" +
                "<i class='ti-archive'></i>" +
                "<span>Inventory</span>" +
                "</a>" +
                //"<ul class=\"sub-menu\">" +
                //"<li class='sub-reports'><a href=\"" + appPath + "/Finance/Reports/Reports.aspx\">Reports</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceReport.aspx\">Financials</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceSalesReport.aspx\">Sales & Debtors</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceProductReport.aspx\">Products</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCapexReport.aspx\">Capex</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCashFlowReport.aspx\">Cash Flow</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/CashFlow/CashFlowProjections.aspx\">Cash Flow Proj</a></li>" +
                //"</ul>" +
                "</li>" +

                  "<li class='has-sub debtors-menu'>" +
                "<a href=\"" + appPath + "/Finance/Reports/FinanceDebtorsReport.aspx\">" +
                "<i class='ti-agenda'></i>" +
                "<span>Debtors</span>" +
                "</a>" +
                //"<ul class=\"sub-menu\">" +
                //    "<li class='sub-reports'><a href=\"" + appPath + "/Finance/Reports/Reports.aspx\">Reports</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceReport.aspx\">Financials</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceSalesReport.aspx\">Sales & Debtors</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceProductReport.aspx\">Products</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCapexReport.aspx\">Capex</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCashFlowReport.aspx\">Cash Flow</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/CashFlow/CashFlowProjections.aspx\">Cash Flow Proj</a></li>" +
                //"</ul>" +
                "</li>" +

                 "<li class='has-sub fixed-menu'>" +
                "<a href=\"" + appPath + "/Finance/Reports/FinanceFixedAssets.aspx\">" +
                "<i class='ti-wallet'></i>" +
                "<span>Fixed Assets</span>" +
                "</a>" +
                //"<ul class=\"sub-menu\">" +
                //    "<li class='sub-reports'><a href=\"" + appPath + "/Finance/Reports/Reports.aspx\">Reports</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceReport.aspx\">Financials</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceSalesReport.aspx\">Sales & Debtors</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceProductReport.aspx\">Products</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCapexReport.aspx\">Capex</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCashFlowReport.aspx\">Cash Flow</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/CashFlow/CashFlowProjections.aspx\">Cash Flow Proj</a></li>" +
                //"</ul>" +
                "</li>" +

                 "<li class='has-sub cash-flow-menu'>" +
                "<a href='javascript:;'>" +
                "<i class='ti-credit-card'></i>" +
                "<span>Cash Flow & Banking</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">" +
                    "<li class='sub-cash-flow'><a href=\"" + appPath + "/Finance/CashFlow/CashFlowProjections.aspx\">Cash Flow Proj</a></li>" +
                    "<li class='sub-forex-rate'><a href=\"" + appPath + "/Finance/Forex/ForexRate.aspx\">Month End Rate</a></li>" +
                    "<li class='sub-bank-info'><a href=\"" + appPath + "/Finance/BankFacilities/BankInfo.aspx\">Bank Facility</a></li>" +
                    "<li class='sub-bank-utilisation'><a href=\"" + appPath + "/Finance/BankFacilities/BankUtilisation.aspx\">Bank Utilisation</a></li>" +
                    "<li class='sub-term-sheet'><a href=\"" + appPath + "/Finance/CashFlow/TermSheet.aspx?ModuleID=CompanyFinance\">Term Sheet</a></li>" +
                    "<li class='sub-cashflow'><a href=\"" + appPath + "/Finance/Reports/FinanceCashFlowReport.aspx\">Reports</a></li>" +
                "</ul>" +
                "</li>" +


                "<li class='has-sub budget-menu'>" +
                "<a href='javascript:;'>" +
                "<i class='ti-shield'></i>" +
                "<span>Budget</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">" +
                    "<li class='sub-upload-budget'><a href=\"" + appPath + "/Finance/Upload/UploadBudget.aspx\">Upload</a></li>" +
                    "<li class='sub-budget-report'><a href=\"" + appPath + "/Finance/Reports/FinanceBudgetReport.aspx\">Reports</a></li>" +
                "</ul>" +
                "</li>" +

                "<li class='has-sub audited-menu'>" +
                "<a href='javascript:;'>" +
                "<i class='ti-stamp'></i>" +
                "<span>Audited</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">" +
                    "<li class='sub-upload-audit'><a href=\"" + appPath + "/Finance/Upload/UploadAudit.aspx\">Upload</a></li>" +
                    "<li class='sub-audit-report'><a href=\"" + appPath + "/Finance/Reports/FinanceAuditReport.aspx\">Reports</a></li>" +
                "</ul>" +
                //"<ul class=\"sub-menu\">" +
                //    "<li class='sub-reports'><a href=\"" + appPath + "/Finance/Reports/Reports.aspx\">Reports</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceReport.aspx\">Financials</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceSalesReport.aspx\">Sales & Debtors</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceProductReport.aspx\">Products</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCapexReport.aspx\">Capex</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceCashFlowReport.aspx\">Cash Flow</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/CashFlow/CashFlowProjections.aspx\">Cash Flow Proj</a></li>" +
                //"</ul>" +
                "</li>" +


                //"<li class='has-sub accounting-menu'>" +
                //"<a href='javascript:;'>" +
                //"<i class='ti-text'></i>" +
                //"<span>Accounting/Tax</span>" +
                //"</a>" +
                //"<ul class=\"sub-menu\">" +
                //    "<li class='sub-upload-audit'><a href=\"" + appPath + "/Finance/Upload/UploadAudit.aspx\">Audit Upload</a></li>" +
                //    //"<li><a href=\"" + appPath + "/Finance/Reports/FinanceAuditReport.aspx\">Audit Reports</a></li>" +
                //    "<li class='sub-company-resource'><a href=\"" + appPath + "/UsefulResources/Resources/ViewCompanyResources.aspx?PageHeader=Accounting/Tax%20>%20Audited%20FS&PageTitle=Finance%20-%20Resources&ModuleCategoryID=20&ModuleCategoryName=CompanyFinance\">Audited FS</a></li>" +
                //    "<li class='sub-intercompany'><a href=\"" + appPath + "/Finance/InterCompany/InterCompany.aspx\">Inter-Company</a></li>" +
                ////"<li class='sub-accounting-report'><a href=\"" + appPath + "/Finance/Reports/AccountingTaxReports.aspx\">Reports</a></li>" +
                //"</ul>" +
                //"</li>" +

                "<li class='has-sub administration-menu'>" +
                "<a href='javascript:;'>" +
                "<i class='ti-ruler-pencil'></i>" +
                "<span>Admin</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">" +
                    //"<li><a href=\"" + appPath + "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Administration &gt; Resources&PageTitle=Finance - Resources&ModuleCategoryID=10&ModuleCategoryName=CompanyFinance\">Resources</a></li>" +
                    "<li class='sub-import-export'><a href=\"" + appPath + "/Finance/Upload/ImportTrialBalance.aspx\">Importer/Exporter</a></li>" +
                    "<li class='sub-coa-mapping'><a href=\"" + appPath + "/Finance/Upload/COAMapping.aspx\">COA Mapping</a></li>" +
                    "<li class='sub-upload-finance'><a href=\"" + appPath + "/Finance/Upload/UploadFinanceData.aspx\">Financial Upload</a></li>" +
                    "<li class='sub-data-setup'><a href=\"" + appPath + "/Finance/Upload/UploadSpecialData.aspx\">Data Setup</a></li>" +
                    "<li class='sub-entertainment-expense'><a href=\"" + appPath + "/HR/Commission/EntertainmentExpense.aspx?CurrentLink=CompanyFinance\">Entertainment</a></li>" +
                    "<li class='sub-claim'><a href=\"" + appPath + "/Claim/Default.aspx?CurrentLink=CompanyFinance\">Claim</a></li>" +
                    "<li class='sub-dimension-setup'><a href=\"" + appPath + "/Finance/Upload/DimensionSetup.aspx\">Dimension Setup</a></li>" +
                //"<li><a href=\"" + appPath + "/Finance/BankFacilities/CustomerList.aspx\">Receiver/Payer</a></li>" +                
                "</ul>" +
                "</li>" +

                "<li class='has-sub resources-menu'>" +
                "<a href='javascript:;'>" +
                "<i class='ti-package'></i>" +
                "<span>Resources</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">" +
                //    "<li class='sub-resource-resource'><a href=\"" + appPath + "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Resources &gt; Resources&PageTitle=Finance - Resources&ModuleCategoryID=10&ModuleCategoryName=CompanyFinance\">Resources</a></li>";
                ////if (lstModuleCategory.Contains(4))
                //sideMenu += "<li class='sub-resource-useful-resource'><a href=\"" + appPath + "/UsefulResources/Resources/ViewUsefulResources.aspx?PageHeader=Resources &gt; Resources&PageTitle=Finance - Resources&ModuleCategoryID=10&ModuleCategoryName=CompanyFinance\">Useful Resources</a></li>";

                //sideMenu += "</ul>" +
                    "<li class='sub-resource-accounting-regulations'><a href=\"" + appPath + "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Resources &gt; Accounting Regulations&PageTitle=Finance - Resources&ModuleCategoryID=10&ModuleCategoryName=CompanyFinance&DocumentCategoryID=31\">Accounting Regulations</a></li>";
                //if (lstModuleCategory.Contains(4))
                sideMenu +=
                    "<li class='sub-resource-company-information'><a href=\"" + appPath + "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Resources &gt; Company Information&PageTitle=Finance - Resources&ModuleCategoryID=4&ModuleCategoryName=CompanyFinance&DocumentCategoryID=1\">Company Information</a></li>" +
                    "<li class='sub-resource-listing-of-codes'><a href=\"" + appPath + "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Resources &gt; Listing of Codes&PageTitle=Finance - Resources&ModuleCategoryID=10&ModuleCategoryName=CompanyFinance&DocumentCategoryID=14\">Listing of Codes</a></li>" +
                    "<li class='sub-resource-forms'><a href=\"" + appPath + "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Resources &gt; Forms&PageTitle=Finance - Resources&ModuleCategoryID=10&ModuleCategoryName=CompanyFinance&DocumentCategoryID=15\">Forms</a></li>" +
                    "<li class='sub-resource-finance-seminar'><a href=\"" + appPath + "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Resources &gt; Finance Seminar&PageTitle=Finance - Resources&ModuleCategoryID=10&ModuleCategoryName=CompanyFinance&DocumentCategoryID=40,45,35,29\">Finance Seminar</a></li>" +
                    "<li class='sub-resource-guides'><a href=\"" + appPath + "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Resources &gt; Guides&PageTitle=Finance - Resources&ModuleCategoryID=10&ModuleCategoryName=CompanyFinance&DocumentCategoryID=39,33,28\">Guides</a></li>" +
                    "<li class='sub-resource-currency-exchange-rate'><a href=\"" + appPath + "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Resources &gt; Foreign Currency Exchange Rate&PageTitle=Finance - Resources&ModuleCategoryID=4&ModuleCategoryName=CompanyFinance&DocumentCategoryID=18\">FX Rate</a></li>";
                sideMenu += "</ul>" +
                "</li>";

                /* Commented By OSS on 2012-06-29
				return
				"<h1>Bank Facility</h1>" +
				"<ul class=\"sidemenu\">" +
				"<li><a href=\"" + appPath + "/Finance/BankFacilities/BankInfo.aspx\">Bank Info</a></li>" +
				"<li><a href=\"" + appPath + "/Finance/BankFacilities/BankUtilisation.aspx\">Bank Utilisation</a></li>" +
				"<li><a href=\"" + appPath + "/Finance/BankFacilities/CustomerList.aspx\">Customers</a></li>" +
				"</ul>" +
				"<h1>Data</h1>" +
				"<ul class=\"sidemenu\">" +
				"<li><a href=\"" + appPath + "/Finance/Upload/UploadBudget.aspx\">Budget</a></li>" +
				"<li><a href=\"" + appPath + "/Finance/Upload/UploadAudit.aspx\">Audit</a></li>" +
				"<li><a href=\"" + appPath + "/Finance/Upload/UploadFinanceData.aspx\">Finance Data</a></li>" +
				"<li><a href=\"" + appPath + "/Finance/Upload/COAMapping.aspx\">COA Mapping</a></li>" +
				"<li><a href=\"" + appPath + "/Finance/Upload/UploadSpecialData.aspx\">Special Data</a></li>" +
				"</ul>" +
				"<h1>Reports</h1>" +
				"<ul class=\"sidemenu\">" +
				"<li><a href=\"" + appPath + "/Finance/Reports/BankReport.aspx\">View Reports</a></li>" +
				"</ul>" + 
				 "<h1>Forex</h1>" +
				"<ul class=\"sidemenu\">" +
				"<li><a href=\"" + appPath + "/Finance/Forex/ForexRate.aspx\">Exchange Rate</a></li>" +
				"</ul>"; 
				 * */
            }
            else if (currentLink == "UsefulResources")
            {
                sideMenu =

                "<li class='has-sub'>" +
                "<a href='javascript:;'>" +
                "<i class='ti-package'></i>" +
                "<span>Resources</span>" +
                "</a>" +
                "<ul class=\"sub-menu\">" +
                "<li><a href=\"" + appPath + "/UsefulResources/Resources/ViewResources.aspx?PageHeader=Resources &gt; Resources&PageTitle=Resources - View&ModuleCategoryID=4&ModuleCategoryName=UsefulResources\">Resources</a></li>" +
                "</ul>" +
                "</li>";

            }
            else if (currentLink == "Issue")
            {
                sideMenu =
                 "<h1>GMS</h1>" +
                 "<ul class=\"sidemenu\">" +
                 "<li><a href=\"" + appPath + "/Issue/Issue.aspx?System=G&Type=B\">Errors(Bugs)</a></li>" +
                 "<li><a href=\"" + appPath + "/Issue/Issue.aspx?System=G&Type=C\">Changes</a></li>" +
                 "</ul>" +
                 "<h1>LMS</h1>" +
                 "<ul class=\"sidemenu\">" +
                 "<li><a href=\"" + appPath + "/Issue/Issue.aspx?System=L&Type=B\">Errors(Bugs)</a></li>" +
                 "<li><a href=\"" + appPath + "/Issue/Issue.aspx?System=L&Type=C\">Changes</a></li>" +
                 "</ul>";
            }
            else if (currentLink == "Procurement")
            {
                sideMenu =
                  "<li class='has-sub vendor-menu'>" +
                  "<a href='javascript:;'>" +
                 "<i class='ti-user'></i>" +
                 "<span>Vendor</span>" +
                 "</a>" +
                 "<ul class=\"sub-menu\">" +
                 //"<li class='sub-forms'><a href=\"" + appPath + "/Procurement/Forms/AddEditVendorApplicationForm.aspx\">Forms</a></li>" +
                 "<li class='sub-records'><a href=\"" + appPath + "/Procurement/Records/VendorDetails.aspx\">Records</a></li>";

            }



            return sideMenu;
        }

        /*
		public void LoadCountryList(object sender)
		{
			LogSession session = base.GetSessionInfo();

			DropDownList ddlCountry = (DropDownList)sender;
			if (session == null)
			{
				ddlCountry.Page.Response.Redirect("../blank.htm");
				return;
			}

			IList<Country> lstCountry = null;
			try
			{
				lstCountry = new SystemDataActivity().RetrieveAllCountryListSortBySeqID();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			ddlCountry.DataSource = lstCountry;
			ddlCountry.DataBind();

			ddlCountry.SelectedValue = session.CountryId.ToString();
		}

		public void LoadDivisionList(object sender)
		{
			LogSession session = base.GetSessionInfo();

			DropDownList ddlDivision = (DropDownList)sender;
			if (session == null)
			{
				ddlDivision.Page.Response.Redirect("../blank.htm");
				return;
			}

			IList<Division> lstDivision = null;
			try
			{
				lstDivision = new SystemDataActivity().RetrieveAllDivisionListSortBySeqID();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			ddlDivision.DataSource = lstDivision;
			ddlDivision.DataBind();

			ddlDivision.SelectedValue = session.DivisionId.ToString();
		}

		public void LoadCompanyList(object sender)
		{
			LogSession session = base.GetSessionInfo();

			DropDownList ddlCompany = (DropDownList)sender;
			if (session == null)
			{
				ddlCompany.Page.Response.Redirect("../blank.htm");
				return;
			}

			IList<Company> lstCompany = null;
			try
			{
				lstCompany = new SystemDataActivity().RetrieveCompanyByCountryIdDivisionId(session.CountryId,
																					session.DivisionId
																					);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			if (lstCompany != null)
			{
				ddlCompany.DataSource = lstCompany;
				ddlCompany.DataBind();

				if (lstCompany.Count <= 0)
					ddlCompany.Items.Insert(0, new ListItem("", "0"));

				ddlCompany.SelectedValue = session.CompanyId.ToString();
			}
		}

		public void ddlCountry_SelectedIndexChanged(short countryId, short divisionId, object sender)
		{
			LogSession session = base.GetSessionInfo();
			DropDownList ddlCountry = (DropDownList)sender;
			IList<Company> lstCompany = null;

			if (session.DivisionId == 1)
			{
				divisionId = 2;
				
				lstCompany = new SystemDataActivity().RetrieveCompanyByCountryIdDivisionId(countryId, divisionId);
				if (lstCompany != null && lstCompany.Count > 0)
				{
					foreach (Company coy in lstCompany)
					{
						UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
																			   coy.Id);
						if (uAccess != null)
						{
							session.CompanyId = coy.Id;
							session.CountryId = coy.CountryID;
							session.DivisionId = coy.DivisionID;
							List<short> lstModuleCategory = session.UserAccessModuleCategory;
							if (lstModuleCategory.Contains(10))
								//ddlDivision.Page.Response.Redirect("../Finance/Default.aspx");
								ddlCountry.Page.RegisterStartupScript("3",
									"<script type='text/javascript'>\n  window.top.navigate(\"../Finance/Default.aspx\");\n</script>");
							else if (lstModuleCategory.Contains(8))
								ddlCountry.Page.RegisterStartupScript("2",
									"<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>");
							else if (lstModuleCategory.Contains(7))
								ddlCountry.Page.RegisterStartupScript("2",
									"<script type='text/javascript'>\n  window.top.navigate(\"../Products/Default.aspx\");\n</script>");
							else if (lstModuleCategory.Contains(11))
								ddlCountry.Page.RegisterStartupScript("2",
									"<script type='text/javascript'>\n  window.top.navigate(\"../HR/Default.aspx\");\n</script>");
							else ddlCountry.Page.RegisterStartupScript("2",
							   "<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>");
							return;
						}
					}
					ddlCountry.SelectedValue = session.CountryId.ToString();
					ddlCountry.Page.RegisterStartupScript("2",
					"<script type='text/javascript'>\n  alert(\"" + "You are not authorized.".Replace("\"", "'") + "\");\n</script>");
					return;
				}
				else
				{
					session.CompanyId = 0;
					session.DivisionId = 2;
					session.CountryId = GMSUtil.ToShort(ddlCountry.SelectedValue);
					List<short> lstModuleCategory = session.UserAccessModuleCategory;
					if (lstModuleCategory.Contains(10))
						//ddlDivision.Page.Response.Redirect("../Finance/Default.aspx");
						ddlCountry.Page.RegisterStartupScript("3",
							"<script type='text/javascript'>\n  window.top.navigate(\"../Finance/Default.aspx\");\n</script>");
					else if (lstModuleCategory.Contains(8))
						ddlCountry.Page.RegisterStartupScript("2",
							"<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>");
					else if (lstModuleCategory.Contains(7))
						ddlCountry.Page.RegisterStartupScript("2",
							"<ddlCountry type='text/javascript'>\n  window.top.navigate(\"../Products/Default.aspx\");\n</script>");
					else if (lstModuleCategory.Contains(11))
						ddlCountry.Page.RegisterStartupScript("2",
							"<script type='text/javascript'>\n  window.top.navigate(\"../HR/Default.aspx\");\n</script>");
					else ddlCountry.Page.RegisterStartupScript("2",
					   "<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>");
					return;
				}
			}

			lstCompany = null;
			lstCompany = new SystemDataActivity().RetrieveCompanyByCountryIdDivisionId(countryId, divisionId);

			if (lstCompany != null && lstCompany.Count > 0)
			{
				foreach (Company coy in lstCompany)
				{
					UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
																		   coy.Id);
					if (uAccess != null)
					{
						session.CompanyId = coy.Id;
						session.CountryId = coy.CountryID;
						session.DivisionId = coy.DivisionID;
						System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCountry.Page, ddlCountry.Page.GetType(), "script1", "<script type=\"text/javascript\"> parent.window.location.reload();</script>", false);
						return;
					}
				}
				ddlCountry.SelectedValue = session.CountryId.ToString();
				ddlCountry.Page.RegisterStartupScript("2",
				"<script type='text/javascript'>\n  alert(\"" + "You are not authorized.".Replace("\"", "'") + "\");\n</script>");
				return;
			}
			else
			{
				session.CompanyId = 0;
				session.CountryId = GMSUtil.ToShort(ddlCountry.SelectedValue);
				System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCountry.Page, ddlCountry.Page.GetType(), "script1", "<script type=\"text/javascript\"> parent.window.location.reload();</script>", false); 
			}
		}

		public void ddlDivision_SelectedIndexChanged(short countryId, short divisionId, object sender)
		{
			LogSession session = base.GetSessionInfo();

			if (divisionId == 1)
			{
				GroupSelected(sender);
				return;
			}
			DropDownList ddlDivision = (DropDownList)sender;

			IList<Company> lstCompany = null;
			lstCompany = new SystemDataActivity().RetrieveCompanyByCountryIdDivisionId(countryId, divisionId);

			if (lstCompany != null && lstCompany.Count > 0)
			{
				foreach (Company coy in lstCompany)
				{
					UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
																		   coy.Id);
					if (uAccess != null)
					{
						if (session.DivisionId == 1)
						{
							session.CountryId = coy.CountryID;
							session.DivisionId = coy.DivisionID;
							session.CompanyId = coy.Id;
							List<short> lstModuleCategory = session.UserAccessModuleCategory;
							if (lstModuleCategory.Contains(10))
								//ddlDivision.Page.Response.Redirect("../Finance/Default.aspx");
								ddlDivision.Page.RegisterStartupScript("3",
									"<script type='text/javascript'>\n  window.top.navigate(\"../Finance/Default.aspx\");\n</script>");
							else if (lstModuleCategory.Contains(8))
								ddlDivision.Page.RegisterStartupScript("2",
									"<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>");
							else if (lstModuleCategory.Contains(7))
								ddlDivision.Page.RegisterStartupScript("2",
									"<script type='text/javascript'>\n  window.top.navigate(\"../Products/Default.aspx\");\n</script>");
							else if (lstModuleCategory.Contains(11))
								ddlDivision.Page.RegisterStartupScript("2",
									"<script type='text/javascript'>\n  window.top.navigate(\"../HR/Default.aspx\");\n</script>");
							else ddlDivision.Page.RegisterStartupScript("2",
							   "<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>");
							return;
						}
						else
						{
							session.CompanyId = coy.Id;
							session.CountryId = coy.CountryID;
							session.DivisionId = coy.DivisionID;
							System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlDivision.Page, ddlDivision.Page.GetType(), "script1", "<script type=\"text/javascript\"> parent.window.location.reload();</script>", false);
							return;
						}
					}
				}
				ddlDivision.SelectedValue = session.DivisionId.ToString();
				ddlDivision.Page.RegisterStartupScript("4",
				"<script type='text/javascript'>\n  alert(\"" + "You are not authorized.".Replace("\"", "'") + "\");\n</script>");
				return;
			}
			else
			{
				if (session.DivisionId == 1)
				{
					session.DivisionId = GMSUtil.ToShort(ddlDivision.SelectedValue);
					session.CompanyId = 0;
					List<short> lstModuleCategory = session.UserAccessModuleCategory;
					if (lstModuleCategory.Contains(10))
						ddlDivision.Page.RegisterStartupScript("3",
							"<script type='text/javascript'>\n  window.top.navigate(\"../Finance/Default.aspx\");\n</script>");
					else if (lstModuleCategory.Contains(8))
						ddlDivision.Page.RegisterStartupScript("2",
							"<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>");
					else if (lstModuleCategory.Contains(7))
						ddlDivision.Page.RegisterStartupScript("2",
							"<script type='text/javascript'>\n  window.top.navigate(\"../Products/Default.aspx\");\n</script>");
					else if (lstModuleCategory.Contains(11))
						ddlDivision.Page.RegisterStartupScript("2",
							"<script type='text/javascript'>\n  window.top.navigate(\"../HR/Default.aspx\");\n</script>");
					else ddlDivision.Page.RegisterStartupScript("2",
					   "<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>");
					return;
				}
				else
				{
					session.DivisionId = GMSUtil.ToShort(ddlDivision.SelectedValue);
					session.CompanyId = 0;
					System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlDivision.Page, ddlDivision.Page.GetType(), "script1", "<script type=\"text/javascript\"> parent.window.location.reload();</script>", false);
					return;
				}
			}
		}
		public void ddlCompany_SelectedIndexChanged(short companyId, object sender)
		{
			LogSession session = base.GetSessionInfo();
			DropDownList ddlCompany = (DropDownList)sender;

			UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
																			companyId);
			if (uAccess == null)
			{
				ddlCompany.SelectedValue = session.CompanyId.ToString();
				ddlCompany.Page.RegisterStartupScript("2",
				"<script type='text/javascript'>\n  alert(\"" + "You are not authorized.".Replace("\"", "'") + "\");\n</script>");
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
					GroupSelected(sender);
					return;
				}
				if (oldCoy.CountryID == 9 && newCoy.CountryID != 9)
				{
					List<short> lstModuleCategory = session.UserAccessModuleCategory;
									if (lstModuleCategory.Contains(10))
										//ddlDivision.Page.Response.Redirect("../Finance/Default.aspx");
										ddlCompany.Page.RegisterStartupScript("3",
											"<script type='text/javascript'>\n  window.top.navigate(\"../Finance/Default.aspx\");\n</script>");
									else if (lstModuleCategory.Contains(8))
										ddlCompany.Page.RegisterStartupScript("2",
											"<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>");
									else if (lstModuleCategory.Contains(7))
										ddlCompany.Page.RegisterStartupScript("2",
											"<script type='text/javascript'>\n  window.top.navigate(\"../Products/Default.aspx\");\n</script>");
									else if (lstModuleCategory.Contains(11))
										ddlCompany.Page.RegisterStartupScript("2",
											"<script type='text/javascript'>\n  window.top.navigate(\"../HR/Default.aspx\");\n</script>");
									else ddlCompany.Page.RegisterStartupScript("2",
									   "<script type='text/javascript'>\n  window.top.navigate(\"../Sales/Default.aspx\");\n</script>");
									return;
				}
				System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlCompany.Page, ddlCompany.Page.GetType(), "script1", "<script type=\"text/javascript\"> parent.window.location.reload();</script>", false);
			}
		}*/

        /*
		protected void GroupSelected(object sender)
		{
			LogSession session = base.GetSessionInfo();
			DropDownList ddlDivision = (DropDownList)sender;

			List<short> lstModuleCategory = session.UserAccessModuleCategory;
			if (lstModuleCategory.Contains(5))
			{
				ddlDivision.Page.RegisterStartupScript("2",
				"<script type='text/javascript'>\n  window.top.navigate(\"../Admin/Default.aspx\");\n</script>");
				return;
			}
			else if (lstModuleCategory.Contains(2))
			{
				ddlDivision.Page.RegisterStartupScript("2",
				"<script type='text/javascript'>\n  window.top.navigate(\"../SysFinance/Default.aspx\");\n</script>");
				return;
			}
			else if (lstModuleCategory.Contains(3))
			{
				ddlDivision.Page.RegisterStartupScript("2",
				"<script type='text/javascript'>\n  window.top.navigate(\"../SysHR/Default.aspx\");\n</script>");
				return;
			}
			else if (lstModuleCategory.Contains(4))
			{
				ddlDivision.Page.RegisterStartupScript("2",
				"<script type='text/javascript'>\n  window.top.navigate(\"../CorporateServices/Default.aspx\");\n</script>");
				return;
			}
			else ddlDivision.Page.RegisterStartupScript("2",
				"<script type='text/javascript'>\n  alert(\"" + "You are not authorized.".Replace("\"", "'") + "\");\n</script>");
			return;
		}*/
    }
}
