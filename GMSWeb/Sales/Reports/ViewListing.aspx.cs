using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GMSWeb.Sales.Reports
{
    public partial class ViewListing : GMSBasePage
    {
        protected short loginUserOrAlternateParty = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Sales");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Sales Detail", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {
                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
                                                                            60);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            60);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (session.TableSuffix != "")
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

            ReportCategory cat1 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Sales Listing");
            if (cat1 != null)
            {
                cat1.Name = "Listing";
                lstCategory.Add(cat1);
            }

            if (lstCategory != null && lstCategory.Count > 0)
            {
                rppCategoryList.DataSource = lstCategory;
                rppCategoryList.DataBind();

                int i = 0;
                foreach (ReportCategory rCategory in lstCategory)
                {
                    IList<VwReportListingForCompany> lstCompanyReport = null;
                    lstCompanyReport = new ReportsActivity().RetrieveCompanyReportByCategoryIdUserAccessId(session.CompanyId, rCategory.ReportCategoryID, loginUserOrAlternateParty);
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
                        lstReport = new ReportsActivity().RetrieveReportByCategoryIdUserAccessId(rCategory.ReportCategoryID, loginUserOrAlternateParty);
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

            UserAccessReport uAccess = new GMSUserActivity().RetrieveUserAccessReportByUserIdReportId(loginUserOrAlternateParty,
                                                                            GMSUtil.ToShort(e.CommandArgument.ToString()));

            IList<UserAccessReportForCompany> uAccessReportForCompanyList = new GMSUserActivity().RetrieveUserAccessReportForCompanyByUserIdReportId(session.CompanyId, loginUserOrAlternateParty, GMSUtil.ToShort(e.CommandArgument.ToString()));

            if (uAccess == null && (uAccessReportForCompanyList != null && uAccessReportForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Products"));

            ClientScript.RegisterStartupScript(typeof(string), "Report",
                string.Format("jsOpenOperationalReport('Reports/Report/SalesReportViewer.aspx?CurrentLink=Products&REPORTID={0}');",
                                    e.CommandArgument.ToString())
                                    , true);
            //}

            PopulateRepeater();
        }
    }
}