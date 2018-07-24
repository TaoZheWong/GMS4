using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace GMSWeb.Reports
{
    public partial class ReportViewer : GMSBasePage
    {
        protected ReportDocument crReportDocument;
        private short reportId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            this.reportId = GMSUtil.ToShort(Request.QueryString["REPORTID"]);

            GMSCore.Entity.Report report = new ReportsActivity().RetrieveReportById(this.reportId);

            GMSCore.Entity.AuditForReportAccess audit = AuditForReportAccess.RetrieveByKey(session.CompanyId, session.UserId, reportId, GMSUtil.ToDate(session.LastLoginDate));
            if (audit == null)
            {
                audit = new AuditForReportAccess();
                audit.CoyID = session.CompanyId;
                audit.UserID = session.UserId;
                audit.ReportID = reportId;
                audit.AccessDate = GMSUtil.ToDate(session.LastLoginDate);
                audit.Save();
            }

            if (report.FileName == "S39-SalesBySalesExecutives.rpt")
                this.cyReportViewer.DisplayGroupTree = true;

            try
            {
                crReportDocument = new ReportDocument();
                if (report.FileName == "S31-SummarySalesByProductGroup.rpt")
                {
                    switch (session.CompanyId)
                    {
                        case 2: report.FileName = "S31-SummarySalesByProductGroup" + "_BPC" + ".rpt"; break;
                        case 8: report.FileName = "S31-SummarySalesByProductGroup" + "_LDL" + ".rpt"; break;
                        case 14: report.FileName = "S31-SummarySalesByProductGroup" + "_NIG" + ".rpt"; break;
                        case 57: report.FileName = "S31-SummarySalesByProductGroup" + "_LDL" + ".rpt"; break;
                        default: break;
                    }
                }
                hidPath.Value = AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.DOC_PATH + "\\" + report.FileName;
                crReportDocument.Load(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.DOC_PATH + "\\" + report.FileName);

            }
            catch (Exception ex)
            {
                this.lblFeedback.Text = ex.Message;
            }

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
                if (crReportDocument.ParameterFields["@CoyID"] != null)
                    crReportDocument.SetParameterValue("@CoyID", session.CompanyId);
                if (crReportDocument.ParameterFields["CoyID"] != null)
                    crReportDocument.SetParameterValue("CoyID", session.CompanyId);
               /* if (report.FileName == "S41-SalesByProductManager_Manager.rpt" ||
                    report.FileName == "S42-SalesByProductManager_Manager.rpt" ||
                    report.FileName == "S52-SalesBySalesExecutives_Salesman.rpt" ||
                    report.FileName == "S51-SalesBySalesExecutives_Manager.rpt" ||
                    report.FileName == "P41-ProductCostAndQuantity_Manager.rpt" ||
                    report.FileName == "S52A-SalesBySalesExecutives_Salesman.rpt" ||
                    report.FileName == "D52-DebtorsAgeingBySalesExecutives.rpt" ||
                    report.FileName == "M42-InventoryAgeingList_ProductManager.rpt" ||
                    report.FileName == "S11-SalesBySalesExec.rpt" ||
                    report.FileName == "S12-SalesByTop100Customers.rpt" ||
                    report.FileName == "S13-SalesByTop100CustomersWithGPPercentage.rpt" ||
                    report.FileName == "S20-CustomerListingBySalesExec.rpt" ||
                    report.FileName == "S31-SalesBySalesExecByCustomer.rpt" ||
                    report.FileName == "S32-SalesBySalesExecByCustomerByBrandByProduct.rpt" ||
                    report.FileName == "S33-SalesBySalesExecByBrandByCustomer.rpt" ||
                    report.FileName == "S34-SalesDetailBySalesExecByCustomerByProduct.rpt" ||
                    report.FileName == "S35-SalesDetailBySalesExecByProductByCustomer.rpt" ||
                    report.FileName == "S41-CustomerStatementOfAccounts.rpt" ||
                    report.FileName == "S42-DebtorAgeingListBySalesExec.rpt" ||
                    report.FileName == "P02-InventoryAgeingByBrand.rpt" || 
                    report.FileName == "P11-SalesByProductManager.rpt" || 
                    report.FileName == "P31-SalesByProductManagerByBrand.rpt" || 
                    report.FileName == "P32-SalesByProductManagerByBrandByCustomer.rpt" || 
                    report.FileName == "P33-SalesDetailByProductManagerByBrandByProduct.rpt" || 
                    report.FileName == "P34-SalesDetailByProductManagerByBrandByProductWithTransaction.rpt" || 
                    report.FileName == "P21-ProductListingByProductManagerByBrandByProductRealTime.rpt" || 
                    report.FileName == "P51-InventoryAgeingByProductManagerByBrand.rpt" || 
                    report.FileName == "P52-InventoryAgeingDetailByProductManagerByBrandByProduct.rpt" || 
                    report.FileName == "P53-ListOfObsoleteStocksByProductManagerByBrandByProduct.rpt" ||
                    report.FileName == "P32S-SalesByProductManagerByBrandByCustomer(Top30Customers).rpt" ||
                    report.FileName == "S31S-SalesBySalesExecByCustomer(Top30Customers).rpt" ||
                    report.FileName == "S14-SalesByTerritory.rpt")*/
                if (crReportDocument.ParameterFields["@UserNumID"] != null)
                    crReportDocument.SetParameterValue("@UserNumID", session.UserId);

                /*if (report.FileName == "P02-InventoryAgeingByBrand.rpt" ||
                    report.FileName == "P51-InventoryAgeingByProductManagerByBrand.rpt" ||
                    report.FileName == "P52-InventoryAgeingDetailByProductManagerByBrandByProduct.rpt")*/
                if (crReportDocument.ParameterFields["Current Date"] != null)
                {
                    crReportDocument.SetParameterValue("Current Date", Convert.ToDateTime((System.DateTime.Today)));
                }
                cyReportViewer.ReportSource = crReportDocument;

            }
            catch (Exception ex)
            {
                this.lblFeedback.Text = ex.Message;
            }
        }

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
