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

namespace GMSWeb.Products.Reports
{
	public partial class View : GMSBasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Master.setCurrentLink("Products"); 
			LogSession session = base.GetSessionInfo();
			if (session == null)
			{
				Response.Redirect(base.SessionTimeOutPage("Products"));
				return;
			}
			UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
																			62);

			IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
																			62);

			if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
				Response.Redirect(base.UnauthorizedPage("Products"));

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
			ReportCategory cat1 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Product");
			if (cat1 != null)
			{
				cat1.Name = "Product Reports";
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
				Response.Redirect(base.UnauthorizedPage("Products"));

			//if (e.CommandArgument.ToString() == "417")
			//{
			//    ClientScript.RegisterStartupScript(typeof(string), "Report",
			//        string.Format("jsOpenOperationalReport('Reports/Report/WebServiceReportViewer.aspx?REPORTID={0}');",
			//                            e.CommandArgument.ToString())
			//                            , true);
			//}
			//else
			//{
				ClientScript.RegisterStartupScript(typeof(string), "Report",
					string.Format("jsOpenOperationalReport('Reports/Report/SalesReportViewer.aspx?REPORTID={0}');",
										e.CommandArgument.ToString())
										, true);
			//}

			PopulateRepeater();
		}
	}
}
