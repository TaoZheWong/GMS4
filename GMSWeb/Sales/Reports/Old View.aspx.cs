using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Web.UI.WebControls;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Sales.Reports
{
    public partial class View : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            60);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            PopulateRepeater();
        }

        #region PopulateRepeater
        private void PopulateRepeater()
        {
            LogSession session = base.GetSessionInfo();
            IList<ReportCategory> lstCategory = new List<ReportCategory>();
            ReportCategory cat1 = new ReportsActivity().RetrieveReportCategoryByCategoryName("Sales");
            if (cat1 != null)
            {
                cat1.Name = "Sales Reports";
                lstCategory.Add(cat1);
            }

            if (lstCategory != null && lstCategory.Count > 0)
            {
                rppCategoryList.DataSource = lstCategory;
                rppCategoryList.DataBind();

                int i = 0;
                foreach (ReportCategory rCategory in lstCategory)
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
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");


            ClientScript.RegisterStartupScript(typeof(string), "Report",
                string.Format("jsOpenOperationalReport('Reports/ReportViewer.aspx?REPORTID={0}');",
                                    e.CommandArgument.ToString())
                                    , true);

            PopulateRepeater();
        }
    }
}
