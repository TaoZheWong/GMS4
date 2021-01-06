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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.XPath;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Finance.Reports
{
    public partial class FinanceManufactureAccount : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string currentLink = "CompanyFinance";


            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();

            }

            Master.setCurrentLink(currentLink);
            //Master.setCurrentLink("CompanyFinance");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            52);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            52);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            PopulateRepeater();

            string javaScript =
            @"<script type=""text/javascript"">
		
		    function toggleAccessRow(n)
		    {
			    if( document.getElementById(""rppToggle_"" + n) )
			    {
				    var current = document.getElementById(""rppToggle_"" + n).style.display;
				    document.getElementById(""rppToggle_"" + n).style.display = (current == null || current == ""none"")?"""":""none"";
				    document[""imgAccessBox_"" + n].src = (current == null || current == ""none"")? sDOMAIN+""/App_Themes/Default/images/checkCloseIcon.gif"" : sDOMAIN+""/App_Themes/Default/images/checkOpenIcon.gif"";
			    }
		    }
    		</script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region PopulateRepeater
        private void PopulateRepeater()
        {
            LogSession session = base.GetSessionInfo();
            IList<ReportCategory> lstCategory = new List<ReportCategory>();


            //this one affect the dropdown bar for those reports
            //ReportCategory cat1 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Finance");
            //if (cat1 != null)
            //{
            //    cat1.Name = "Finance Reports";
            //    lstCategory.Add(cat1);
            //}

            //ReportCategory cat2 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Finance Something");
            //if (cat2 != null)
            //{
            //    cat2.Name = "Sales Reports";
            //    lstCategory.Add(cat2);
            //}
            //ReportCategory cat3 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Finance Sales");
            //if (cat3 != null)
            //{
            //    cat3.Name = "Debtors Reports";
            //    lstCategory.Add(cat3);
            //}
            //ReportCategory cat4 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Finance Suppliers");
            //if (cat4 != null)
            //{
            //    cat4.Name = "Suppliers Reports";
            //    lstCategory.Add(cat4);
            //}
            //ReportCategory cat5 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Finance Products");
            //if (cat5 != null)
            //{
            //    cat5.Name = "Inventory Reports";
            //    lstCategory.Add(cat5);
            //}
            //ReportCategory cat6 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Finance Fixed Asset");
            //if (cat6 != null)
            //{
            //    cat6.Name = "Fixed Asset Reports";
            //    lstCategory.Add(cat6);
            //}

            ////Additional
            //ReportCategory cat7 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Finance Cash Flow");
            //if (cat7 != null)
            //{
            //    cat7.Name = "Cash Flow & Banking Reports";
            //    lstCategory.Add(cat7);
            //}
            //ReportCategory cat8 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Finance Audit");
            //if (cat8 != null)
            //{
            //    cat8.Name = "Audit Report";
            //    lstCategory.Add(cat8);
            //}
            ReportCategory cat9 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Finance Manufacture");
            if (cat9 != null)
            {
                cat9.Name = "Production Reports";
                lstCategory.Add(cat9);
            }

            //this one is display the report based on the category of the report
            //need to copy this one also, to display those reports
            if (lstCategory != null && lstCategory.Count > 0)
            {
                rppCategoryList.DataSource = lstCategory;
                rppCategoryList.DataBind();

                int i = 0;
                foreach (ReportCategory rCategory in lstCategory)
                {
                    IList<VwReportListingForCompany> lstCompanyReport = null;
                    //retrieve company report de store prod
                    lstCompanyReport = new ReportsActivity().RetrieveCompanyReportByCategoryIdUserAccessId(session.CompanyId, rCategory.ReportCategoryID, session.UserId);
                    if (lstCompanyReport != null && lstCompanyReport.Count > 0)
                    {
                        // Bind Data to sub repeater
                        RepeaterItem item = this.rppCategoryList.Items[i];
                        Repeater rppReportList = (Repeater)item.FindControl("rppReportList");

                        if (rppReportList != null)
                        {
                            rppReportList.DataSource = lstCompanyReport;
                            rppReportList.DataBind();
                        }
                    }
                    else
                    {
                        IList<VwReportListing> lstReport = null;
                        lstReport = new ReportsActivity().RetrieveReportByCategoryIdUserAccessId(rCategory.ReportCategoryID, session.UserId);
                        if (lstReport != null && lstReport.Count > 0)
                        {
                            // Bind Data to sub repeater
                            RepeaterItem item = this.rppCategoryList.Items[i];
                            Repeater rppReportList = (Repeater)item.FindControl("rppReportList");

                            if (rppReportList != null)
                            {
                                rppReportList.DataSource = lstReport;
                                rppReportList.DataBind();
                            }
                        }
                    }
                    i++;
                }
            }
        }
        #endregion

        protected void rppReportList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            UserAccessReport uAccess = new GMSUserActivity().RetrieveUserAccessReportByUserIdReportId(session.UserId,
                                                                           GMSUtil.ToShort(e.CommandArgument.ToString()));

            IList<UserAccessReportForCompany> uAccessReportForCompanyList = new GMSUserActivity().RetrieveUserAccessReportForCompanyByUserIdReportId(session.CompanyId, session.UserId, GMSUtil.ToShort(e.CommandArgument.ToString()));

            if (uAccess == null && (uAccessReportForCompanyList != null && uAccessReportForCompanyList.Count == 0))
                Response.Redirect("../../Unauthorized.htm");

            //UserAccessReport uAccess = new GMSUserActivity().RetrieveUserAccessReportByUserIdReportId(session.UserId,
            //                                                                GMSUtil.ToShort(e.CommandArgument.ToString()));
            //if (uAccess == null)
            //    Response.Redirect("../../Unauthorized.htm");

            ClientScript.RegisterStartupScript(typeof(string), "Report",
                string.Format("jsOpenOperationalReport('Reports/Report/FinanceReportViewer.aspx?REPORTID={0}&&TRNNO=1&&REPORT=1');",
                                    e.CommandArgument.ToString())
                                    , true);
            PopulateRepeater();
        }
    }
}
