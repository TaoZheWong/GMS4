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

namespace GMSWeb.Sales.Reports
{
    public partial class View : GMSBasePage
    {
        protected short loginUserOrAlternateParty = 0;
        protected string currentLink = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            currentLink = "Sales";
            LogSession session = base.GetSessionInfo();

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                
            }
            Master.setCurrentLink(currentLink);

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Product Report", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {

                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;

            
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
                                                                            60);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            60);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if(session.TableSuffix != "")
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
            ReportCategory cat1 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Sales For Management");
            if (cat1 != null)
            {
                cat1.Name = "For Management";
                lstCategory.Add(cat1);
            }

            ReportCategory cat2 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Sales For Sales Exec");
            if (cat2 != null)
            {
                cat2.Name = "For Sales Exec";
                lstCategory.Add(cat2);
            }

            ReportCategory cat3 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Sales For Product Manager");
            if (cat3 != null)
            {
                cat3.Name = "For Product Manager";
                lstCategory.Add(cat3);
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
                Response.Redirect(base.UnauthorizedPage("Sales"));

            HiddenField hidIsExternalReport = (HiddenField)e.Item.FindControl("hidIsExternalReport");

            if (hidIsExternalReport.Value.ToString() == "True")
            {
                ClientScript.RegisterStartupScript(typeof(string), "Report",
               string.Format("jsOpenOperationalReport('Reports/Report/ExternalReportViewer.aspx?CurrentLink=" + this.currentLink + "&REPORTID={0}');",
                                    e.CommandArgument.ToString()), true);
            }
            else
            {
                ClientScript.RegisterStartupScript(typeof(string), "Report",
               string.Format("jsOpenOperationalReport('Reports/Report/SalesReportViewer.aspx?CurrentLink=" + this.currentLink + "&REPORTID={0}');",
                                    e.CommandArgument.ToString()) , true);
            }           

            PopulateRepeater();
        }
    }
}
