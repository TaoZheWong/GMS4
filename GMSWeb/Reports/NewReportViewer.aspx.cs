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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GMSCore.Activity;

namespace GMSWeb.Reports
{
    public partial class NewReportViewer : GMSBasePage
    {
        protected ReportDocument crReportDocument;
        private short reportId = 0;
        string fileName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            //UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
            //                                                                49);
            //if (uAccess == null)
            //    Response.Redirect("../../Unauthorized.htm");

            this.reportId = GMSUtil.ToShort(Request.QueryString["REPORTID"]);

            if (reportId != -1 && reportId != -2 && reportId != -3)
            {
                fileName = new ReportsActivity().RetrieveReportById(this.reportId).FileName;
            }
            else
            {
                fileName = Request.QueryString["REPORT"].Trim() + ".rpt";
                if (fileName == "Pricelist.rpt")
                    cyReportViewer.HasToggleGroupTreeButton = true;
            }

            try
            {
                crReportDocument = new ReportDocument();
                crReportDocument.Load(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.DOC_PATH + "\\" + fileName);

            }
            catch (Exception ex)
            {
                this.lblFeedback.Text = ex.Message;
            }

            if (!IsPostBack)
            {
                if (crReportDocument.ParameterFields["@Year"] != null || crReportDocument.ParameterFields["Year"] != null)
                {
                    ddlYear.Items.Clear();
                    for (int i = 2011; i <= 2020; i++)
                    {
                        ddlYear.Items.Add(i.ToString());
                    }
                    trYear.Visible = true;
                    btnSubmit.Visible = true;
                }
                if (crReportDocument.ParameterFields["@Month"] != null || crReportDocument.ParameterFields["Month"] != null)
                {
                    ddlMonth.Items.Clear();
                    for (int i = 1; i <= 12; i++)
                    {
                        ddlMonth.Items.Add(i.ToString());
                    }
                    trMonth.Visible = true;
                    btnSubmit.Visible = true;
                }
                if (crReportDocument.ParameterFields["@ProjectID"] != null && session.CompanyId == 28
                    && !fileName.Contains("Department"))
                {
                    ddlProjectID.Items.Clear();
                    ddlProjectID.Items.Add(new ListItem("COMPANY", "-1"));
                    ddlProjectID.Items.Add(new ListItem("WELDING SALES", "1"));
                    ddlProjectID.Items.Add(new ListItem("WELDING CONSUMABLES", "2"));
                    ddlProjectID.Items.Add(new ListItem("WELDING EQUIPMENT", "3"));
                    ddlProjectID.Items.Add(new ListItem("WELDING ACCESSORIES", "4"));

                    trProjectID.Visible = true;
                    btnSubmit.Visible = true;
                }
                else if (crReportDocument.ParameterFields["@ProjectID"] != null && session.CompanyId == 28
                    && fileName.Contains("Department"))
                {
                    ddlProjectID.Items.Clear();
                    ddlProjectID.Items.Add(new ListItem("CUSTOMER SERVICE", "5"));
                    ddlProjectID.Items.Add(new ListItem("PURCHASING", "6"));
                    ddlProjectID.Items.Add(new ListItem("LOGISTICS & WAREHOUSING", "7"));
                    ddlProjectID.Items.Add(new ListItem("WORKSHOP", "8"));
                    ddlProjectID.Items.Add(new ListItem("INTERNATIONAL SALES", "9"));
                    ddlProjectID.Items.Add(new ListItem("RENTAL DIVISION", "10"));

                    trProjectID.Visible = true;
                    btnSubmit.Visible = true;
                }
                else
                {
                    trProjectID.Visible = false;
                }
                if (crReportDocument.ParameterFields["@Type"] != null)
                {
                    ddlType.Items.Clear();
                    ddlType.Items.Add(new ListItem("ALL", "All"));
                    ddlType.Items.Add(new ListItem("DIRECT CUSTOMERS", "External"));
                    ddlType.Items.Add(new ListItem("INTERCO CUSTOMERS", "Internal"));

                    trType.Visible = true;
                    btnSubmit.Visible = true;
                }
            }

            if (IsPostBack)
            {
                try
                {
                    ConnectionInfo connection = new ConnectionInfo();
                    connection.DatabaseName = DBManager.GetInstance().DatabaseName;
                    connection.ServerName = DBManager.GetInstance().ServerName;
                    connection.UserID = DBManager.GetInstance().UserLoginName;
                    connection.Password = DBManager.GetInstance().UserLoginPwd;

                    foreach (CrystalDecisions.CrystalReports.Engine.Table table in crReportDocument.Database.Tables)
                    {
                        // Cache the logon info block
                        TableLogOnInfo logOnInfo = table.LogOnInfo;

                        // Set the connection
                        logOnInfo.ConnectionInfo = connection;

                        // Apply the connection to the table!
                        table.ApplyLogOnInfo(logOnInfo);
                    }

                    foreach (CrystalDecisions.CrystalReports.Engine.Section section in crReportDocument.ReportDefinition.Sections)
                    {
                        // In each section we need to loop through all the reporting objects
                        foreach (CrystalDecisions.CrystalReports.Engine.ReportObject reportObject in section.ReportObjects)
                        {
                            if (reportObject.Kind == ReportObjectKind.SubreportObject)
                            {
                                SubreportObject subReport = (SubreportObject)reportObject;
                                ReportDocument subDocument = subReport.OpenSubreport(subReport.SubreportName);

                                foreach (CrystalDecisions.CrystalReports.Engine.Table table in subDocument.Database.Tables)
                                {
                                    // Cache the logon info block
                                    TableLogOnInfo logOnInfo = table.LogOnInfo;

                                    // Set the connection
                                    logOnInfo.ConnectionInfo = connection;

                                    // Apply the connection to the table!
                                    table.ApplyLogOnInfo(logOnInfo);
                                }
                            }
                        }
                    }

                    if (crReportDocument.ParameterFields["@Year"] != null)
                        crReportDocument.SetParameterValue("@Year", GMSUtil.ToInt(ddlYear.SelectedValue));
                    if (crReportDocument.ParameterFields["Year"] != null)
                        crReportDocument.SetParameterValue("Year", GMSUtil.ToInt(ddlYear.SelectedValue));
                    if (crReportDocument.ParameterFields["@Month"] != null)
                        crReportDocument.SetParameterValue("@Month", GMSUtil.ToInt(ddlMonth.SelectedValue));
                    if (crReportDocument.ParameterFields["Month"] != null)
                        crReportDocument.SetParameterValue("Month", GMSUtil.ToInt(ddlMonth.SelectedValue));
                    if (crReportDocument.ParameterFields["@ProjectID"] != null && session.CompanyId == 28)
                        crReportDocument.SetParameterValue("@ProjectID", GMSUtil.ToInt(ddlProjectID.SelectedValue));
                    else if (crReportDocument.ParameterFields["@ProjectID"] != null)
                        crReportDocument.SetParameterValue("@ProjectID", -1);
                    if (crReportDocument.ParameterFields["@Type"] != null)
                        crReportDocument.SetParameterValue("@Type", GMSUtil.ToInt(ddlType.SelectedValue));

                    if (crReportDocument.ParameterFields["@CoyID"] != null)
                        crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
                    if (crReportDocument.ParameterFields["CoyID"] != null)
                        crReportDocument.SetParameterValue("CoyID", session.CompanyId);
                    if (crReportDocument.ParameterFields["@UserNumID"] != null)
                        crReportDocument.SetParameterValue("@UserNumID", session.UserId);
                    cyReportViewer.ReportSource = crReportDocument;
                }
                catch (Exception ex)
                {
                    this.lblFeedback.Text = ex.Message;
                }
            }
        }

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
        }
        #endregion

        protected void Page_Unload(object sender, EventArgs e)
        {
            cyReportViewer.Dispose();
            cyReportViewer = null;
            crReportDocument.Close();
            crReportDocument.Dispose();
            GC.Collect();
        }
    }
}
