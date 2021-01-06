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
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;


using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;


namespace GMSWeb.SysHR.Upload
{
    public partial class ImporterExporter : GMSBasePage
    {
        private int coyId;
        private string companyCode;
        LogSession session;
        private short year;
        private short month;
        private HSSFWorkbook hssfworkbook;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("CompanyHR");
            session = base.GetSessionInfo();
            Company coy = new SystemDataActivity().RetrieveCompanyById(session.CompanyId, session);
            companyCode = coy.Code;

            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                return;
            }
            coyId = session.CompanyId;

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            129);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyHR"));

            if (!IsPostBack)
            {
                // load year ddl
                DataTable dtt1 = new DataTable();
                dtt1.Columns.Add("Year", typeof(string));

                for (int i = -1; i < 1; i++)
                {
                    DataRow dr1 = dtt1.NewRow();
                    dr1["Year"] = DateTime.Now.Year + i;

                    dtt1.Rows.Add(dr1);
                }

                this.ddlYear.DataSource = dtt1;
                this.ddlYear.DataBind();

                DateTime lastMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                this.ddlYear.SelectedValue = lastMonthDate.Year.ToString();

                // load month ddl
                DataTable dtt2 = new DataTable();
                dtt2.Columns.Add("Month", typeof(string));

                for (int i = 1; i <= 12; i++)
                {
                    DataRow dr1 = dtt2.NewRow();
                    dr1["Month"] = i;
                    dtt2.Rows.Add(dr1);
                }

                this.ddlMonth.DataSource = dtt2;
                this.ddlMonth.DataBind();

                this.ddlMonth.SelectedValue = lastMonthDate.Month.ToString();
            }
            //btnImport.Attributes.Add("onclick", "javascript:" + btnImport.ClientID + ".disabled=true;" + this.GetPostBackEventReference(btnImport));
        }
        #endregion

        #region btnImport_Click
        
        protected void btnImport_Click(object sender, EventArgs e)
        {      
            /*
            DataSet dsExcel = new DataSet();          
          
            DataTable workTable = null;
            workTable = new DataTable("HRFinanceData");
            workTable.Columns.Add("HRCostCentre", typeof(String));
            workTable.Columns.Add("HRItemName", typeof(String));
            workTable.Columns.Add("FinanceCostCentre", typeof(String));
            workTable.Columns.Add("FinanceItemName", typeof(String));
            workTable.Columns.Add("Amount", typeof(decimal));            

            LogSession session = base.GetSessionInfo();
            Company coy = new SystemDataActivity().RetrieveCompanyById(session.CompanyId, session);
            
               
                if (coy != null)
                {
                    string filePath = @"F:\HRFolder\\" + coy.Code + "\\" + ddlYear.SelectedValue.ToString() + "\\" + ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString().PadLeft(2, '0') + ".xls";

                    if (File.Exists(filePath))
                    {
                        new GMSGeneralDALC().DeleteCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month);
                       
                        workTable.Rows.Clear();

                        DataSet dsCostCentre = new DataSet();
                        new GMSGeneralDALC().GetHRFinanceItem(coy.CoyID, "CostCentre", ref dsCostCentre);

                        DataSet dsItemName = new DataSet();
                        new GMSGeneralDALC().GetHRFinanceItem(coy.CoyID, "ItemName", ref dsItemName);

                        DataSet dsCostCentreMapping = new DataSet();
                        new GMSGeneralDALC().GetHRFinanceMapping(coy.CoyID, "CostCentre", ref dsCostCentreMapping);

                        DataSet dsItemNameMapping = new DataSet();
                        new GMSGeneralDALC().GetHRFinanceMapping(coy.CoyID, "ItemName", ref dsItemNameMapping);
                        
                        dsExcel = ExcelToDataTable(filePath);

                        string costCentre = "";
                        string oldCostCentre = "";

                        string ItemName1 = "";
                        string ItemName2 = "";
                        string ItemName3 = "";
                        string ItemName4 = "";
                        string ItemName5 = "";
                        string ItemName6 = "";
                        string ItemName7 = "";
                        string ItemName8 = "";
                        string ItemName9 = "";
                        string ItemName10 = "";
                        string ItemName11 = "";
                        string ItemName12 = "";
                        string ItemName13 = "";
                        string ItemName14 = "";
                        string ItemName15 = "";
                        string ItemName16 = "";
                        string ItemName17 = "";
                        string ItemName18 = "";
                        string ItemName19 = "";
                        string ItemName20 = "";
                        string ItemName21 = "";
                        string ItemName22 = "";


                        for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
                        {
                            // Get Header ItemName
                            if (dsExcel.Tables[0].Rows[i][0].ToString().Trim() == "Cost Centre" && ItemName1 == "")
                            {
                                ItemName1 = dsExcel.Tables[0].Rows[i + 1][0].ToString().Trim();
                                ItemName2 = dsExcel.Tables[0].Rows[i + 1][1].ToString().Trim();
                                ItemName3 = dsExcel.Tables[0].Rows[i + 1][2].ToString().Trim();
                                ItemName4 = dsExcel.Tables[0].Rows[i + 1][3].ToString().Trim();
                                ItemName5 = dsExcel.Tables[0].Rows[i + 1][4].ToString().Trim();
                                ItemName6 = dsExcel.Tables[0].Rows[i + 1][5].ToString().Trim();
                                ItemName7 = dsExcel.Tables[0].Rows[i + 1][6].ToString().Trim();
                                ItemName8 = dsExcel.Tables[0].Rows[i + 1][7].ToString().Trim();
                                ItemName9 = dsExcel.Tables[0].Rows[i + 1][8].ToString().Trim();
                                ItemName10 = dsExcel.Tables[0].Rows[i + 1][9].ToString().Trim();
                                ItemName11 = dsExcel.Tables[0].Rows[i + 1][10].ToString().Trim();
                                ItemName12 = dsExcel.Tables[0].Rows[i + 2][0].ToString().Trim();
                                ItemName13 = dsExcel.Tables[0].Rows[i + 2][1].ToString().Trim();
                                ItemName14 = dsExcel.Tables[0].Rows[i + 2][2].ToString().Trim();
                                ItemName15 = dsExcel.Tables[0].Rows[i + 2][3].ToString().Trim();
                                ItemName16 = dsExcel.Tables[0].Rows[i + 2][4].ToString().Trim();
                                ItemName17 = dsExcel.Tables[0].Rows[i + 2][5].ToString().Trim();
                                ItemName18 = dsExcel.Tables[0].Rows[i + 2][6].ToString().Trim();
                                ItemName19 = dsExcel.Tables[0].Rows[i + 2][7].ToString().Trim();
                                ItemName20 = dsExcel.Tables[0].Rows[i + 2][8].ToString().Trim();
                                ItemName21 = dsExcel.Tables[0].Rows[i + 2][9].ToString().Trim();
                                ItemName22 = dsExcel.Tables[0].Rows[i + 2][10].ToString().Trim();

                            }

                            if (dsExcel.Tables[0].Rows[i][2].ToString().Trim() == "Total")
                            {
                                oldCostCentre = costCentre;
                                costCentre = "";
                            }

                            if (dsExcel.Tables[0].Rows[i][0].ToString().Trim() == "Cost Centre" && dsExcel.Tables[0].Rows[i + 1][0].ToString().Trim() != ItemName1)
                            {
                                oldCostCentre = costCentre;
                                costCentre = dsExcel.Tables[0].Rows[i][1].ToString().Trim();
                                if (ItemName1 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName1,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][0].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName1;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][0].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName2 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName2,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][1].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName2;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][1].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName3 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName3,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][2].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName3;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][2].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName4 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName4,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][3].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName4;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][3].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName5 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName5,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][4].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName5;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][4].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName6 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName6,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][5].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName6;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][0].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName7 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName7,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][6].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName7;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][6].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName8 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName8,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][7].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName8;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][7].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName9 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName9,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][8].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName9;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][8].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName10 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName10,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][9].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName10;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][9].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName11 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName11,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][10].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName11;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 1][10].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName12 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName12,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][0].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName12;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][0].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName13 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName13,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][1].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName13;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][1].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName14 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName14,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][2].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName14;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][2].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName15 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName15,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][3].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName15;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][3].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName16 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName16,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][4].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName16;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][4].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName17 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName17,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][5].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName17;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][5].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName18 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName18,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][6].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName18;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][6].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName19 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName19,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][7].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName19;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][7].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName20 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName20,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][8].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName20;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][8].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName21 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName21,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][9].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName21;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][9].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                                if (ItemName22 != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, ItemName22,
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][10].ToString().Trim()));
                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = ItemName22;
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i + 2][10].ToString().Trim());
                                    workTable.Rows.Add(workRow);
                                }

                            }


                            double amount;
                            if (double.TryParse(dsExcel.Tables[0].Rows[i][4].ToString().Trim(), out amount))


                                if (amount.ToString() != "0"
                                    && !Regex.IsMatch(dsExcel.Tables[0].Rows[i][0].ToString().Trim(), @"^(\+|-)?\d+(\.\d+)?$")
                                    && dsExcel.Tables[0].Rows[i][0].ToString().Trim() != ""
                                    && dsExcel.Tables[0].Rows[i][4].ToString().Trim() != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, dsExcel.Tables[0].Rows[i][0].ToString().Trim(),
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i][4].ToString().Trim()));

                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = dsExcel.Tables[0].Rows[i][0].ToString().Trim();
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i][4].ToString().Trim());
                                    workTable.Rows.Add(workRow);

                                }

                            if (double.TryParse(dsExcel.Tables[0].Rows[i][10].ToString().Trim(), out amount))

                                if (amount.ToString() != "0"
                                    && !Regex.IsMatch(dsExcel.Tables[0].Rows[i][6].ToString().Trim(), @"^(\+|-)?\d+(\.\d+)?$")
                                    && dsExcel.Tables[0].Rows[i][6].ToString().Trim() != ""
                                    && dsExcel.Tables[0].Rows[i][10].ToString().Trim() != "")
                                {
                                    new GMSGeneralDALC().InsertCostCenterForMonth(GMSUtil.ToByte(coy.CoyID), year, month, costCentre, dsExcel.Tables[0].Rows[i][6].ToString().Trim(),
                                    GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i][10].ToString().Trim()));

                                    DataRow workRow = workTable.NewRow();
                                    workRow["HRCostCentre"] = costCentre;
                                    workRow["HRItemName"] = dsExcel.Tables[0].Rows[i][6].ToString().Trim();
                                    workRow["FinanceCostCentre"] = "";
                                    workRow["FinanceItemName"] = "";
                                    workRow["Amount"] = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i][10].ToString().Trim());
                                    workTable.Rows.Add(workRow);

                                }

                        }

                        // Update Finance Cost Centre

                        foreach (DataRow row1 in workTable.Rows)
                        {
                            for (int i = 0; i < dsCostCentreMapping.Tables[0].Rows.Count; i++)
                            {
                                if (dsCostCentreMapping.Tables[0].Rows[i]["HR"].ToString() == row1["HRCostCentre"].ToString().Trim())
                                    row1["FinanceCostCentre"] = dsCostCentreMapping.Tables[0].Rows[i]["Finance"].ToString();
                            }
                        }


                        // Update Finance Item Name

                        foreach (DataRow row1 in workTable.Rows)
                        {
                            for (int i = 0; i < dsItemNameMapping.Tables[0].Rows.Count; i++)
                            {
                                if (dsItemNameMapping.Tables[0].Rows[i]["HR"].ToString() == row1["HRItemName"].ToString().Trim())
                                    row1["FinanceItemName"] = dsItemNameMapping.Tables[0].Rows[i]["Finance"].ToString();
                            }
                        }


                        //getting distinct values for group column
                        DataTable dtGroup = workTable.DefaultView.ToTable(true, "FinanceCostCentre", "FinanceItemName");

                        //adding column for the row count
                        dtGroup.Columns.Add("Total", typeof(decimal));

                        //looping thru distinct values for the group, counting
                        foreach (DataRow dr in dtGroup.Rows)
                        {
                            dr["Total"] = workTable.Compute("SUM(Amount)", "FinanceCostCentre = '" + dr["FinanceCostCentre"] + "' AND FinanceItemName = '" + dr["FinanceItemName"] + "'");
                        }

                        //export excel                        
                        string fileName = "" + companyCode + "_" + ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString().PadLeft(2, '0') + ".xls";
                       
                        //HSSFWorkbook hssfworkbook;
                        using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            hssfworkbook = new HSSFWorkbook(file);
                        }
                       
                        ISheet sheet1 = hssfworkbook.CreateSheet("Finance");
                        IRow row;
                        row = sheet1.CreateRow(0);

                        row.CreateCell(0).SetCellValue("");
                        for (int i = 0; i < dsItemName.Tables[0].Rows.Count; i++)
                        {
                            row.CreateCell(i+1).SetCellValue(dsItemName.Tables[0].Rows[i]["Finance"].ToString());
                        }

                        for (int i = 0; i < dsCostCentre.Tables[0].Rows.Count; i++)
                        {
                            row = sheet1.CreateRow(i+1);
                            row.CreateCell(0).SetCellValue(dsCostCentre.Tables[0].Rows[i]["Finance"].ToString());

                            for (int j = 0; j < dsItemName.Tables[0].Rows.Count; j++)
                            {
                                if (dsCostCentre.Tables[0].Rows[i]["Finance"].ToString() == "ALL")
                                {
                                    DataRow[] result = dtGroup.Select("FinanceItemName = '" + dsItemName.Tables[0].Rows[j]["Finance"].ToString() + "'");                                    
                                    foreach (DataRow r in result)
                                    {
                                        row.CreateCell(j + 1).SetCellFormula("SUM(" + dsItemName.Tables[0].Rows[j]["Col"].ToString() + "2:" + dsItemName.Tables[0].Rows[j]["Col"].ToString() + dsCostCentre.Tables[0].Rows.Count.ToString() + ")");                                        
                                    }                                    
                                }
                                else
                                {

                                    DataRow[] result = dtGroup.Select("FinanceCostCentre = '" + dsCostCentre.Tables[0].Rows[i]["Finance"].ToString() + "' AND FinanceItemName = '" + dsItemName.Tables[0].Rows[j]["Finance"].ToString() + "'");
                                    foreach (DataRow r in result)
                                    {
                                        row.CreateCell(j + 1).SetCellValue(Double.Parse(r["Total"].ToString()));                                        
                                    }
                                }
                            }
                        }
                        
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                        Response.ContentType = "application/vnd.ms-excel";
                        GetExcelStream().WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();                        

                    }

                }

             */

            this.IFrame1.Attributes["style"] = "";
            this.IFrame1.Attributes["src"] = String.Format("ParsingData.aspx?CoyID={0}&Year={1}&Month={2}",
                                                        coyId, ddlYear.SelectedValue, ddlMonth.SelectedValue);
            
        }

        public static DataSet ExcelToDataTable(string filePath)
        {
            DataSet dsExcel = new DataSet();
              
              DataTable dt = new DataTable();
  
              HSSFWorkbook hssfworkbook;
              using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
              {
                  hssfworkbook = new HSSFWorkbook(file);
              }
             ISheet sheet = hssfworkbook.GetSheetAt(0);
             System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
 
             IRow headerRow = sheet.GetRow(0);
             //int cellCount = headerRow.LastCellNum;
             int cellCount = 12;
 
             for (int j = 0; j <cellCount; j++)
             {
                 ICell cell = headerRow.GetCell(j);
                 if (cell == null)
                 {
                     dt.Columns.Add("");
                 }
                 else
                 {
                     dt.Columns.Add(cell.ToString());
                 }

             }
 
             for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
             {
                 IRow row = sheet.GetRow(i);
                 DataRow dataRow = dt.NewRow();
                 if (row == null)
                 {
                     break;
                 }
                 for (int j = row.FirstCellNum; j <cellCount; j++)
                 {
                     if (row.GetCell(j) != null)
                         dataRow[j] = row.GetCell(j).ToString();
                 }
 
                 dt.Rows.Add(dataRow);
             }

             dsExcel.Tables.Add(dt);

             return dsExcel;
         }

        MemoryStream GetExcelStream()
        {
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            return file;
        }

        #endregion

        
    }
}

