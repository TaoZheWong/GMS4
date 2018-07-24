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

namespace GMSWeb.Reports.Report
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
                                                                            22);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            PopulateRepeater();
        }

        #region lnkSearchGo_Click
        protected void lnkSearchGo_Click(object sender, EventArgs e)
        {
            #region Get session
            LogSession session = base.GetSessionInfo();
            //if (session == null)
            //{
            //    this.PageMsgPanel.ShowMessage("Your session has expired. Please login again.", MessagePanelControl.MessageEnumType.Alert);
            //    return;
            //}
            #endregion

            //string reportType = "0";

            #region Get reportType
            //reportType = this.ddlSalesReport.SelectedValue;
            //if (reportType == "0")
            //{
            //    this.PageMsgPanel.ShowMessage("Please select a report.", MessagePanelControl.MessageEnumType.Alert);
            //    return;
            //}
            #endregion

           

        }
        #endregion

        #region PopulateRepeater
        private void PopulateRepeater()
        {
            LogSession session = base.GetSessionInfo();
            IList<ReportCategory> lstCategory = null;
            lstCategory = new ReportsActivity().RetrieveAllReportCategoryListSortBySeqID();

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
                    } else
                        if (lstReport.Count == 0)
                        {
                            this.rppCategoryList.Items[i].Visible = false;
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
