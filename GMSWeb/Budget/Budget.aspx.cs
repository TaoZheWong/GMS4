using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.Spreadsheet;
using Telerik.Web.UI;

namespace GMSWeb.Budget
{
    public partial class Budget : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["authkey"] != null)//for access from GMS3 
            {
                base.loginByAuthKey(Request.Params["authkey"].ToString());
            }
            string currentLink = "Sales";
            lblPageHeader.Text = "Sales";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                if (Request.Params["CurrentLink"].ToString().Trim() == "Sales")
                    lblPageHeader.Text = "Sales";
                else
                    lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);

            LogSession session = base.GetSessionInfo();

            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            179);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            179);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!IsPostBack)
            {
                LoadControl();
            }
        }

        protected void LoadControl()
        {
            LogSession session = base.GetSessionInfo();
            //ddlyear
            var startYear = int.Parse(DateTime.Now.AddYears(-2).ToString("yyyy"));
            int currentYear = DateTime.Now.Year;
            for (int i = startYear; i <= currentYear; i++)
            {
                ddlYear.Items.Add(new ListItem(i.ToString() + "/" + (i + 1).ToString().Substring(2), Convert.ToString(i)));
            }
            this.ddlYear.SelectedValue = currentYear.ToString();

            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet dsProjects = new DataSet();
            //same dimension restriction as f21 report
            ggdal.GetCompanyProject(session.CompanyId, session.UserId, 426, ref dsProjects);

            for (int j = 0; j < dsProjects.Tables[0].Rows.Count; j++)
            {
                this.ddlSearchDim1.Items.Add(new ListItem(dsProjects.Tables[0].Rows[j]["ProjectName"].ToString(), dsProjects.Tables[0].Rows[j]["ReportProjectID"].ToString()));
            }
        }

        #region btnStart_Click
        protected void btnStart_Click(object sender, EventArgs e)
        {
            LoadSpreadSheet();
        }
        #endregion

        #region RadSpreadsheet 
        protected void LoadSpreadSheet()
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();
            DataSet ds4 = new DataSet();
            DataSet ds5 = new DataSet();
            DataSet ds6 = new DataSet();
            DataSet ds7 = new DataSet();
            DataSet ds8 = new DataSet();
            DataSet ds9 = new DataSet();
            short year = 0; short lastYear = 0; short nextYear = 0; short yearBeforeLastYear = 0; short yearAfterNextYear = 0;
            string customerType = "";
            string classType = "";
            string salespersonID = "";
            string salesexec = "ALL";
            short dim1 = -1; string d1 = "";
            short dim2 = -1; string d2 = "";
            short dim3 = -1; string d3 = "";
            short dim4 = -1; string d4 = "";

            year = short.Parse(this.ddlYear.SelectedValue);
            nextYear = short.Parse((int.Parse(this.ddlYear.SelectedValue) + 1).ToString());
            lastYear = short.Parse((int.Parse(this.ddlYear.SelectedValue) - 1).ToString());
            yearBeforeLastYear = short.Parse((int.Parse(this.ddlYear.SelectedValue) - 2).ToString());
            yearAfterNextYear = short.Parse((int.Parse(this.ddlYear.SelectedValue) + 2).ToString());
            customerType = this.ddlCustomerType.SelectedValue;
            classType = this.ddlClassType.SelectedValue;
            salespersonID = this.ddlSalesperson.SelectedValue;
            dim1 = short.Parse(this.ddlSearchDim1.SelectedValue);
            d1 = this.ddlSearchDim1.SelectedItem.Text;
            if (!string.IsNullOrEmpty(this.ddlSearchDim2.SelectedValue))
            {
                dim2 = short.Parse(this.ddlSearchDim2.SelectedValue);
                d2 = this.ddlSearchDim2.SelectedItem.Text;
            }
            if (!string.IsNullOrEmpty(this.ddlSearchDim3.SelectedValue))
            {
                dim3 = short.Parse(this.ddlSearchDim3.SelectedValue);
                d3 = this.ddlSearchDim3.SelectedItem.Text;
            }
            if (!string.IsNullOrEmpty(this.ddlSearchDim4.SelectedValue))
            {
                dim4 = short.Parse(this.ddlSearchDim4.SelectedValue);
                d4 = this.ddlSearchDim4.SelectedItem.Text;
            }
            if (!string.IsNullOrEmpty(this.ddlSalesperson.SelectedValue))
                salesexec = this.ddlSalesperson.SelectedItem.Text.Substring(0, this.ddlSalesperson.SelectedItem.Text.IndexOf("-"));

            try
            {
                //Actual
                ggdal.RetrieveBudgetActualSales(session.CompanyId, lastYear, session.UserId, customerType,
                   salespersonID, classType, dim1, dim2, dim3, dim4, ref ds);//monthly figure

                ggdal.RetrieveBudgetDistributionRatio(session.CompanyId, lastYear, session.UserId, customerType,
                   salespersonID, classType, dim1, dim2, dim3, dim4, "Actual", ref ds2);//current year distribution ratio

                ggdal.RetrieveBudgetDistributionRatio(session.CompanyId, yearBeforeLastYear, session.UserId, customerType,
                   salespersonID, classType, dim1, dim2, dim3, dim4, "Actual", ref ds3);//prior year distribution ratio

                //Forecast
                ggdal.RetrieveBudgetForecastSales(session.CompanyId, year, session.UserId, customerType,
                    salespersonID, classType, dim1, dim2, dim3, dim4, ref ds4);//monthly figure

                //ggdal.RetrieveBudgetDistributionRatio(session.CompanyId, year, session.UserId, customerType,
                //   salespersonID, classType, dim1, dim2, dim3, dim4, "Forecast", ref ds5);//current year distribution ratio

                ggdal.RetrieveBudgetDistributionRatio(session.CompanyId, lastYear, session.UserId, customerType,
                   salespersonID, classType, dim1, dim2, dim3, dim4, "Actual", ref ds6);//prior year distribution ratio

                //Budget
                ggdal.RetrieveBudgetBudgetSales(session.CompanyId, nextYear, session.UserId, customerType,
                    salespersonID, classType, dim1, dim2, dim3, dim4, ref ds7);//monthly figure

                //ggdal.RetrieveBudgetDistributionRatio(session.CompanyId, nextYear, session.UserId, customerType,
                //   salespersonID, classType, dim1, dim2, dim3, dim4, "Budget", ref ds8);//current year distribution ratio

                ggdal.RetrieveBudgetDistributionRatio(session.CompanyId, year, session.UserId, customerType,
                   salespersonID, classType, dim1, dim2, dim3, dim4, "Forecast", ref ds9);//prior year distribution ratio
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString());
            }
            if (ds.Tables[0] != null && ds2.Tables[0] != null && ds3.Tables[0] != null &&
                ds4.Tables[0] != null && ds6.Tables[0] != null &&
                ds7.Tables[0] != null && ds9.Tables[0] != null)
            {
                this.resultList.Visible = true;
                this.lblSearchSummary.Visible = false;
                var sheet1 = FillWorksheet(ds.Tables[0], ds2.Tables[0], ds3.Tables[0], session, "B11A ACTUAL SALES BY SALES EXEC BY CUSTOMER BY YEAR", classType,
                    lastYear + "/" + year.ToString(), d1, d2, d3, d4, salesexec, "Actual");
                this.RadSpreadsheet1.Sheets.Add(sheet1);

                this.resultList2.Visible = true;
                this.lblSearchSummary2.Visible = false;
                var sheet2 = FillWorksheet(ds4.Tables[0], ds6.Tables[0], ds6.Tables[0], session, "B11F FORECAST SALES BY SALES EXEC BY CUSTOMER BY YEAR", classType,
                    year + "/" + nextYear.ToString(), d1, d2, d3, d4, salesexec, "Forecast");
                this.RadSpreadsheet2.Sheets.Add(sheet2);

                this.resultList3.Visible = true;
                this.lblSearchSummary3.Visible = false;
                var sheet3 = FillWorksheet(ds7.Tables[0], ds9.Tables[0], ds9.Tables[0], session, "B11B BUDGET SALES BY SALES EXEC BY CUSTOMER BY YEAR", classType,
                    nextYear + "/" + yearAfterNextYear.ToString(), d1, d2, d3, d4, salesexec, "Budget");
                this.RadSpreadsheet3.Sheets.Add(sheet3);
            }
            var workbook = new Workbook();
            workbook.Sheets = RadSpreadsheet1.Sheets;
            SpreadsheetStylingB11A(workbook);

            var json = workbook.ToJson();
            HiddenField1.Value = json;

            var workbook2 = new Workbook();
            workbook2.Sheets = RadSpreadsheet2.Sheets;
            SpreadsheetStylingB11F(workbook2);

            var json2 = workbook2.ToJson();
            HiddenField2.Value = json2;

            var workbook3 = new Workbook();
            workbook3.Sheets = RadSpreadsheet3.Sheets;
            SpreadsheetStylingB11B(workbook3);

            var json3 = workbook3.ToJson();
            HiddenField3.Value = json3;
        }

        private static Worksheet FillWorksheet(DataTable data, DataTable data2, DataTable data3, LogSession session, string title, string type, string year,
            string d1, string d2, string d3, string d4, string salesExec, string budgetType)
        {
            var workbook = new Workbook();
            var sheet = workbook.AddSheet();
            sheet.Columns = new List<Column>();
            sheet.FrozenRows = 4;//freeze row when scrolling
            int totalCol = 19; int typeIndex = 10; int yearIndex = 13; int currIndex = 16;
            if (budgetType == "Budget")
            {
                totalCol = 20;
                typeIndex = 7;
                yearIndex = 10;
                currIndex = 13;
            }

            #region first row header
            var firstRow = new Row() { Index = 0 };
            var cellInFirstRow = new Cell() { Index = 0, Value = title, Bold = true };
            firstRow.AddCell(cellInFirstRow);
            for (int i = 6; i <= totalCol; i++)//fill row with cell first
            {
                var cell = new Cell() { Index = i, Value = "", Bold = true };
                firstRow.AddCell(cell);
            }
            var cellType = new Cell() { Index = typeIndex, Value = "TYPE: " + type, Bold = true };
            firstRow.AddCell(cellType);
            var cellYear = new Cell() { Index = yearIndex, Value = "YEAR: " + year, Bold = true };
            firstRow.AddCell(cellYear);
            var cellCurr = new Cell() { Index = currIndex, Value = "CURR: " + session.DefaultCurrency + " 000s", Bold = true };
            firstRow.AddCell(cellCurr);
            if (budgetType == "Budget")
            {
                var cellTarget = new Cell() { Index = 15, Value = "Target Inc%: ", Bold = true };
                firstRow.AddCell(cellTarget);
                var targetInc = new Cell() { Index = 17, Value = 5, Bold = true };
                firstRow.AddCell(targetInc);

                var cellBudget = new Cell() { Index = 18, Value = "Budget Inc%: ", Bold = true };
                firstRow.AddCell(cellBudget);
                var budgetInc = new Cell() { Index = 20, Value = 5, Bold = true, Format = "#" };
                firstRow.AddCell(budgetInc);
            }
            sheet.AddRow(firstRow);
            sheet.AddMergedCells("A1:F1");
            if (budgetType == "Budget")
            {
                sheet.AddMergedCells("H1:J1");
                sheet.AddMergedCells("K1:M1");
                sheet.AddMergedCells("N1:O1");
                sheet.AddMergedCells("P1:Q1");
                sheet.AddMergedCells("S1:T1");
            }
            else
            {
                sheet.AddMergedCells("K1:M1");
                sheet.AddMergedCells("N1:P1");
                sheet.AddMergedCells("Q1:T1");
            }
            #endregion

            #region second row header
            var secondRow = new Row() { Index = 1 };
            var cellInSecondRow = new Cell() { Index = 0, Value = "LEEDEN NATIONAL OXYGEN LTD", Bold = true };
            secondRow.AddCell(cellInSecondRow);
            for (int i = 2; i <= totalCol; i++)
            {
                var cell = new Cell() { Index = i, Value = "", Bold = true };
                secondRow.AddCell(cell);
            }
            var cellDim1 = new Cell() { Index = 4, Value = "D1: " + d1, Bold = true };
            secondRow.AddCell(cellDim1);
            var cellDim2 = new Cell() { Index = 7, Value = "D2: " + d2, Bold = true };
            secondRow.AddCell(cellDim2);
            var cellDim3 = new Cell() { Index = 10, Value = "D3: " + d3, Bold = true };
            secondRow.AddCell(cellDim3);
            var cellDim4 = new Cell() { Index = 13, Value = "D4: " + d4, Bold = true };
            secondRow.AddCell(cellDim4);
            var cellSalesExec = new Cell() { Index = 16, Value = "SALES EXEC: " + salesExec, Bold = true };
            secondRow.AddCell(cellSalesExec);

            sheet.AddRow(secondRow);
            sheet.AddMergedCells("A2:C2");
            sheet.AddMergedCells("E2:F2");
            sheet.AddMergedCells("H2:I2");
            sheet.AddMergedCells("K2:L2");
            sheet.AddMergedCells("N2:O2");
            sheet.AddMergedCells("Q2:T2");
            #endregion

            #region data column header
            var row = new Row() { Index = 2 };
            int columnIndex = 0;
            foreach (DataColumn dataColumn in data.Columns)
            {
                sheet.Columns.Add(new Column());
                string cellValue = dataColumn.ColumnName;
                var cell = new Cell() { Index = columnIndex++, Value = cellValue, Bold = true, TextAlign = "center" };
                row.AddCell(cell);
            }
            sheet.AddRow(row);

            var row2 = new Row() { Index = 3 };
            for (int i = 0; i <= totalCol; i++)
            {
                var cell = new Cell() { Index = i, Value = "", Bold = true };
                row2.AddCell(cell);
            }
            for (int i = 7; i <= 18; i++)//set month is actual or forecast or budget
            {
                if (budgetType == "Actual")
                {
                    var cell = new Cell() { Index = i, Value = "Act", Bold = true };
                    row2.AddCell(cell);
                }
                else if (budgetType == "Forecast")
                {
                    Cell cell;
                    if (i <= 13)
                        cell = new Cell() { Index = i, Value = "Act", Bold = true };
                    else
                        cell = new Cell() { Index = i, Value = "Fore", Bold = true };
                    row2.AddCell(cell);
                }
                else if (budgetType == "Budget")
                {
                    var cell = new Cell() { Index = i, Value = "Budget", Bold = true };
                    row2.AddCell(cell);
                }
            }
            sheet.AddRow(row2);
            for (int i = 1; i <= 21; i++)
            {
                if (i <= 7 || i > 19)
                {
                    var alphabet = IntToAlpha(i);
                    sheet.AddMergedCells(alphabet + "3:" + alphabet + "4");
                }
            }

            #endregion

            #region prior year distribution ratio
            int rowIndex = 4;
            foreach (DataRow dataRow in data3.Rows)
            {
                row = new Row() { Index = rowIndex++ };
                columnIndex = 0;
                foreach (DataColumn dataColumn in data3.Columns)
                {
                    string cellValue = dataRow[dataColumn.ColumnName].ToString();
                    if (columnIndex == 2)
                    {
                        var cell = new Cell() { Index = columnIndex++, Value = "Prior Year Distribution Ratio", Italic = true, Format = "#.0" };
                        row.AddCell(cell);
                    }
                    else
                    {
                        var cell = new Cell() { Index = columnIndex++, Value = cellValue, Italic = true };
                        row.AddCell(cell);
                    }

                }
                sheet.AddRow(row);
            }
            #endregion

            #region load monthly figures
            foreach (DataRow dataRow in data.Rows)
            {
                row = new Row() { Index = rowIndex++ };
                columnIndex = 0;
                foreach (DataColumn dataColumn in data.Columns)
                {
                    string cellValue = dataRow[dataColumn.ColumnName].ToString();
                    var cell = new Cell() { Index = columnIndex++, Value = cellValue, Format = "0" };
                    row.AddCell(cell);
                }
                sheet.AddRow(row);
            }
            #endregion

            #region add total row
            row = new Row() { Index = rowIndex++ };
            columnIndex = 0;
            for (columnIndex = 0; columnIndex <= 40; columnIndex++)
            {
                var alphabet = IntToAlpha(columnIndex + 1);
                var alphabet2 = IntToAlpha(columnIndex + 21);
                if (columnIndex == 2)
                {
                    var cell = new Cell() { Index = columnIndex, Value = "Total", Bold = true };
                    row.AddCell(cell);
                }
                else if (columnIndex >= 7 && columnIndex <= 19)
                {
                    var cell = new Cell() { Index = columnIndex, Value = 0, Bold = true, Formula = "SUM(" + alphabet + "6:" + alphabet + (rowIndex - 1) + ")", Format = "#" };
                    row.AddCell(cell);
                }
                else if ((columnIndex >= 27 && columnIndex <= 40) && budgetType == "Budget")//add this to calculate b11f total in B11B
                {
                    var cell = new Cell() { Index = columnIndex, Value = 0, Bold = true, Formula = "SUM(" + alphabet + "6:" + alphabet + (rowIndex - 1) + ")", Format = "#" };
                    row.AddCell(cell);
                }
                else
                {
                    var cell = new Cell() { Index = columnIndex, Value = "" };
                    row.AddCell(cell);
                }
            }
            sheet.AddRow(row);
            #endregion

            #region add current year distribution ratio row
            foreach (DataRow dataRow in data2.Rows)
            {
                row = new Row() { Index = rowIndex++ };
                columnIndex = 0;
                foreach (DataColumn dataColumn in data2.Columns)
                {
                    var alphabet = IntToAlpha(columnIndex + 1);
                    string cellValue = dataRow[dataColumn.ColumnName].ToString();
                    var cell = new Cell() { Index = columnIndex++, Value = cellValue, Italic = true, Format = "0.0" };
                    row.AddCell(cell);
                }
                sheet.AddRow(row);
            }
            #endregion

            sheet.Columns[0].Width = 30;
            sheet.Columns[1].Width = 70;
            sheet.Columns[2].Width = 200;
            for (int i = 3; i < sheet.Columns.Count; i++)
            {
                sheet.Columns[i].Width = 55;
            }
            return sheet;
        }

        protected static string IntToAlpha(int x)
        {
            int lowChar;
            StringBuilder result = new StringBuilder();
            do
            {
                lowChar = (x - 1) % 26;
                x = (x - 1) / 26;
                result.Insert(0, (char)(lowChar + 65));
            } while (x > 0);
            return result.ToString();
        }

        protected void SpreadsheetStylingB11A(Workbook workbook)
        {

            #region header style
            for (int i = 0; i <= 1; i++)
            {
                foreach (var cell in workbook.Sheets[0].Rows[i].Cells)
                {
                    cell.Wrap = true;
                    cell.Bold = true;
                    cell.Background = "#769fcd";
                    cell.Color = "#F6F6F6";
                    cell.TextAlign = "Left";
                    cell.VerticalAlign = "center";
                }
            }
            #endregion

            #region header column name style
            for (int i = 2; i <= 3; i++)
            {
                foreach (var cell in workbook.Sheets[0].Rows[i].Cells)
                {
                    cell.Wrap = true;
                    cell.Bold = true;
                    cell.Background = "#b9d7ea";
                    cell.Color = "#F6F6F6";
                    cell.TextAlign = "Center";
                    cell.VerticalAlign = "center";
                }
            }
            #endregion

            #region item style
            for (int i = 4; i < workbook.Sheets[0].Rows.Count; i++)
            {
                var row = workbook.Sheets[0].Rows[i];
                //set all cell to double to enable formula calculation
                for (int j = 7; j <= 19; j++)
                {
                    row.Cells[j].Value = double.Parse(row.Cells[j].Value.ToString());
                }
                foreach (var cell in row.Cells)
                {
                    cell.Enable = true;
                    cell.FontSize = 12;
                    //vertical align
                    cell.VerticalAlign = "center";
                    cell.Color = "#000000";//font color
                    //background for each row
                    if (IsEven(i))
                        cell.Background = "#f7fbfc";
                    else
                        cell.Background = "#d6e6f2";
                }
                //text align
                for (int j = 0; j <= 6; j++)
                {
                    if (j == 2)//only customer column align left
                        row.Cells[j].TextAlign = "left";
                    else
                        row.Cells[j].TextAlign = "center";
                }
                for (int j = 7; j <= 19; j++)
                {
                    row.Cells[j].TextAlign = "right";
                }
                if (i == workbook.Sheets[0].Rows.Count - 2)
                {
                    {
                        //row.Cells[19].Formula = "ROUNDUP(SUM(G" + (i + 1) + ":R" + (i + 1) + "),0)";
                    }
                }
            }
            #endregion
        }

        protected void SpreadsheetStylingB11F(Workbook workbook)
        {
            #region header style
            for (int i = 0; i <= 1; i++)
            {
                foreach (var cell in workbook.Sheets[0].Rows[i].Cells)
                {
                    cell.Wrap = true;
                    cell.Bold = true;
                    cell.Background = "#878ecd";
                    cell.Color = "#F6F6F6";
                    cell.TextAlign = "Left";
                    cell.VerticalAlign = "center";
                }
            }
            #endregion

            #region header column name style
            for (int i = 2; i <= 3; i++)
            {
                foreach (var cell in workbook.Sheets[0].Rows[i].Cells)
                {
                    cell.Wrap = true;
                    cell.Bold = true;
                    cell.Background = "#b9bbdf";
                    cell.Color = "#F6F6F6";
                    cell.TextAlign = "Center";
                    cell.VerticalAlign = "center";
                }
            }
            #endregion

            #region item style
            for (int i = 4; i < workbook.Sheets[0].Rows.Count; i++)
            {
                var row = workbook.Sheets[0].Rows[i];
                foreach (var cell in row.Cells)
                {
                    cell.FontSize = 12;
                    //vertical align
                    cell.VerticalAlign = "center";
                    cell.Color = "#000000";//font color
                    //background
                    if (IsEven(i))
                        cell.Background = "#f7fbfc";
                    else
                        cell.Background = "#dff4f3";
                }
                //disable cells
                for (int j = 0; j <= 13; j++)
                {
                    //row.Cells[j].Enable = false;
                }
                //text align
                for (int j = 0; j <= 6; j++)
                {
                    if (j == 2)//only customer column align left
                        row.Cells[j].TextAlign = "left";
                    else
                        row.Cells[j].TextAlign = "center";
                }
                for (int j = 7; j <= 19; j++)
                {
                    row.Cells[j].TextAlign = "right";
                }
                //set all cell to double to enable formula calculation
                for (int j = 7; j <= 18; j++)
                {
                    row.Cells[j].Value = double.Parse(row.Cells[j].Value.ToString());
                }
                //set formula for the forecast month
                if (i > 4)
                {
                    if (i < workbook.Sheets[0].Rows.Count - 2)//to exclude total row
                    {
                        if (string.IsNullOrEmpty(row.Cells[21].Value.ToString()))//check status column value
                        {
                            for (int j = 14; j <= 18; j++)
                            {
                                var alphabet = IntToAlpha(j + 1);
                                row.Cells[j].Color = "#0033FF";
                                row.Cells[j].Formula = "ROUND((SUM(G" + (i + 1) + ":M" + (i + 1) + ")/SUM(G5:M5)) *" + alphabet + "5,0)";
                            }
                        }
                        row.Cells[19].Formula = "SUM(H" + (i + 1) + ":S" + (i + 1) + ")";
                    }
                    else if (i == workbook.Sheets[0].Rows.Count - 1)
                    {
                        for (int j = 7; j <= 19; j++)
                        {
                            var alphabet = IntToAlpha(j + 1);
                            row.Cells[j].Formula = "(" + alphabet + (i) + "/T" + (i) + ")*100";
                        }
                    }
                    else
                    {
                        //row.Cells[19].Formula = "ROUND(SUM(H" + (i + 1) + ":S" + (i + 1) + "),0)";
                    }
                }
            }
            #endregion
        }

        protected void SpreadsheetStylingB11B(Workbook workbook)
        {

            #region header style
            for (int i = 0; i <= 1; i++)
            {
                foreach (var cell in workbook.Sheets[0].Rows[i].Cells)
                {
                    cell.Wrap = true;
                    cell.Bold = true;
                    cell.Background = "#1fab89";
                    cell.Color = "#F6F6F6";
                    cell.TextAlign = "Left";
                    cell.VerticalAlign = "center";
                }
            }
            workbook.Sheets[0].Rows[0].Cells[19].TextAlign = "right";
            workbook.Sheets[0].Rows[0].Cells[21].TextAlign = "right";
            int currentTotalRow = workbook.Sheets[0].Rows.Count - 1;
            workbook.Sheets[0].Rows[0].Cells[22].Formula = "((T" + currentTotalRow + "-AO" + currentTotalRow + ")/AO" + currentTotalRow + ")*100";//formula to get Budget Inc%
            #endregion

            #region header column name style
            for (int i = 2; i <= 3; i++)
            {
                foreach (var cell in workbook.Sheets[0].Rows[i].Cells)
                {
                    cell.Wrap = true;
                    cell.Bold = true;
                    cell.Background = "#62d2a2";
                    cell.Color = "#F6F6F6";
                    cell.TextAlign = "Center";
                    cell.VerticalAlign = "center";
                }
            }
            #endregion

            #region item style
            for (int i = 4; i < workbook.Sheets[0].Rows.Count; i++)
            {
                var row = workbook.Sheets[0].Rows[i];
                foreach (var cell in row.Cells)
                {
                    cell.FontSize = 12;
                    //vertical align
                    cell.VerticalAlign = "center";
                    cell.Color = "#000000";//font color
                    //background
                    if (IsEven(i))
                        cell.Background = "#f7fbfc";
                    else
                        cell.Background = "#d7fbe8";
                }
                //disable cells
                for (int j = 0; j <= 13; j++)
                {
                    //row.Cells[j].Enable = false;
                }
                //text align
                for (int j = 0; j <= 6; j++)
                {
                    if (j == 2)//only customer column align left
                        row.Cells[j].TextAlign = "left";
                    else
                        row.Cells[j].TextAlign = "center";
                }
                for (int j = 7; j <= 20; j++)
                {
                    row.Cells[j].TextAlign = "right";
                }
                //set all cell to double to enable formula calculation
                for (int j = 7; j <= 18; j++)
                {
                    row.Cells[j].Value = double.Parse(row.Cells[j].Value.ToString());
                }
                //set formula for the forecast month
                if (i > 4)
                {
                    if (i < workbook.Sheets[0].Rows.Count - 2)//to exclude total row
                    {
                        if (string.IsNullOrEmpty(row.Cells[22].Value.ToString()))//check status column value
                        {
                            for (int j = 7; j <= 18; j++)
                            {
                                var alphabet = IntToAlpha(j + 1);
                                var alphabet2 = IntToAlpha(j + 21);
                                row.Cells[j].Color = "#0033FF";
                                row.Cells[j].Formula = "(" + alphabet2 + (i + 1) + "*(1+R1/100))";//b11f figure * target inc value
                            }
                        }
                        row.Cells[19].Formula = "SUM(H" + (i + 1) + ":S" + (i + 1) + ")";//total of each month

                        row.Cells[20].Formula = "IFERROR(((SUM(H" + (i + 1) + ":S" + (i + 1) + ")-SUM(AB" + (i + 1) + ":AM" + (i + 1) + "))/SUM(AB" + (i + 1) + ":AM" + (i + 1) + "))*100,0)";//Inc% of current 12 mth/ b11f 12 mth
                    }
                    else if (i == workbook.Sheets[0].Rows.Count - 1)
                    {
                        for (int j = 7; j <= 18; j++)
                        {
                            var alphabet = IntToAlpha(j + 1);
                            row.Cells[j].Formula = "(" + alphabet + (i) + "/T" + (i) + ")*100";//current distribution ratio
                        }
                    }
                    else
                    {
                        row.Cells[40].Formula = "SUM(AB" + (i + 1) + ":AM" + (i + 1) + ")";//total of 12 month b11f
                    }
                }
            }
            #endregion
        }
        #endregion


        protected void RadButton1_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            var workbook = Workbook.FromJson(HiddenField1.Value);
            workbook.Sheets[0].Rows.Remove(workbook.Sheets[0].Rows[0]);
            if (workbook.Sheets[0].Rows.Count > 0)
            {
                try
                {
                    foreach (var row in workbook.Sheets[0].Rows)
                    {

                    }
                    alertMessage("Price Changes are submitted for approval.");
                }
                catch (Exception ex)
                {
                    alertMessage(ex.ToString());
                }
            }
        }

        protected void RadButton2_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            var workbook = Workbook.FromJson(HiddenField2.Value);
            for (int i = 0; i < 5; i++)
            {   //exclude title, header column,& prior year distirbution ratio
                workbook.Sheets[0].Rows.Remove(workbook.Sheets[0].Rows[0]);
            }
            if (workbook.Sheets[0].Rows.Count > 0)
            {
                try
                {
                    int rowIndex = 0;
                    foreach (var row in workbook.Sheets[0].Rows)
                    {
                        if (rowIndex < workbook.Sheets[0].Rows.Count - 1)
                        {
                            int year = int.Parse(this.ddlYear.SelectedValue);
                            double nov = double.Parse(row.Cells[14].Value.ToString());
                            double dec = double.Parse(row.Cells[15].Value.ToString());
                            double jan = double.Parse(row.Cells[16].Value.ToString());
                            double feb = double.Parse(row.Cells[17].Value.ToString());
                            double mar = double.Parse(row.Cells[18].Value.ToString());
                            double total = double.Parse(row.Cells[19].Value.ToString());
                            int ID = 0; bool isTotalNml = false;
                            if (!string.IsNullOrEmpty(row.Cells[20].Value.ToString()) && !string.IsNullOrEmpty(row.Cells[26].Value.ToString()))
                            {
                                ID = int.Parse(row.Cells[20].Value.ToString());
                                isTotalNml = bool.Parse(row.Cells[26].Value.ToString());
                                ggdal.UpdateBudgetForecast(session.CompanyId, session.UserId, year, ID, nov, dec, jan, feb, mar, total, isTotalNml);
                            }
                        }
                        rowIndex++;
                    }
                    alertMessage("B11F Saved.");
                }
                catch (Exception ex)
                {
                    alertMessage(ex.ToString());
                }
            }
            LoadSpreadSheet();
        }

        protected void RadButton3_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            var workbook = Workbook.FromJson(HiddenField3.Value);
            for (int i = 0; i < 5; i++)
            {   //exclude title, header column,& prior year distirbution ratio
                workbook.Sheets[0].Rows.Remove(workbook.Sheets[0].Rows[0]);
            }
            if (workbook.Sheets[0].Rows.Count > 0)
            {
                try
                {
                    int rowIndex = 0;
                    foreach (var row in workbook.Sheets[0].Rows)
                    {
                        if (rowIndex < workbook.Sheets[0].Rows.Count - 2)
                        {
                            int year = int.Parse(this.ddlYear.SelectedValue)+1;
                            string newAccName = row.Cells[2].Value.ToString();
                            string salespersonID = this.ddlSalesperson.SelectedValue;
                            double apr = double.Parse(row.Cells[7].Value.ToString());
                            double may = double.Parse(row.Cells[8].Value.ToString());
                            double jun = double.Parse(row.Cells[9].Value.ToString());
                            double jul = double.Parse(row.Cells[10].Value.ToString());
                            double aug = double.Parse(row.Cells[11].Value.ToString());
                            double sep = double.Parse(row.Cells[12].Value.ToString());
                            double oct = double.Parse(row.Cells[13].Value.ToString());
                            double nov = double.Parse(row.Cells[14].Value.ToString());
                            double dec = double.Parse(row.Cells[15].Value.ToString());
                            double jan = double.Parse(row.Cells[16].Value.ToString());
                            double feb = double.Parse(row.Cells[17].Value.ToString());
                            double mar = double.Parse(row.Cells[18].Value.ToString());
                            double total = double.Parse(row.Cells[19].Value.ToString());
                            int ID = 0; bool isTotalNml = false;
                            if (!string.IsNullOrEmpty(row.Cells[21].Value.ToString()) && !string.IsNullOrEmpty(row.Cells[40].Value.ToString()))
                            {
                                ID = int.Parse(row.Cells[21].Value.ToString());
                                isTotalNml = bool.Parse(row.Cells[40].Value.ToString());
                                ggdal.UpdateBudgetBudget(session.CompanyId, session.UserId, year, ID,
                                    apr,may,jun,jul,aug,sep,oct,
                                    nov, dec, jan, feb, mar, total, isTotalNml);
                            }
                            else
                            {
                                if(salespersonID != "")
                                {
                                    ggdal.InsertBudgetNewAccount(session.CompanyId, session.UserId, year, newAccName,
                                    apr, may, jun, jul, aug, sep, oct,
                                    nov, dec, jan, feb, mar, total, salespersonID);
                                }  
                            }
                        }
                        rowIndex++;
                    }
                    alertMessage("B11B Saved.");
                }
                catch (Exception ex)
                {
                    alertMessage(ex.ToString());
                }
            }
            LoadSpreadSheet();
        }

        #region dimension drop down list selected index changed event in search 
        protected void ddlYearMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DropDownList ddlProjectID = this.ddlSearchDim1;
                DropDownList ddlDepartmentID = this.ddlSearchDim2;
                DropDownList ddlSectionID = this.ddlSearchDim3;
                DropDownList ddlUnitID = this.ddlSearchDim4;

                ddlProjectID.Items.Clear();
                ddlDepartmentID.Items.Clear();
                ddlSectionID.Items.Clear();
                ddlUnitID.Items.Clear();
                ddlSalesperson.Items.Clear();

                ddlDepartmentID.Enabled = false;
                ddlSectionID.Enabled = false;
                ddlUnitID.Enabled = false;
                DropDownList ddlYear = this.ddlYear;

                short year = Convert.ToInt16(this.ddlYear.SelectedValue);
                short month = short.Parse(DateTime.Now.Month.ToString());

                DataSet dsProjects = new DataSet();
                dacl.GetCompanyProject(session.CompanyId, session.UserId, 426, ref dsProjects);
                if (dsProjects.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsProjects.Tables[0].Rows.Count; j++)
                    {
                        ddlProjectID.Items.Add(new ListItem(dsProjects.Tables[0].Rows[j]["ProjectName"].ToString(), dsProjects.Tables[0].Rows[j]["ReportProjectID"].ToString()));
                    }
                    if (Convert.ToInt16(ddlProjectID.SelectedValue) > 0)
                    {
                        DataSet dsDepartments = new DataSet();
                        dacl.GetCompanyDepartment(session.CompanyId, Convert.ToInt16(ddlProjectID.SelectedValue), 0, session.UserId, year, month, ref dsDepartments);
                        foreach (DataRow dr in dsDepartments.Tables[0].Rows)
                        {
                            ddlDepartmentID.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
                        }
                        ddlDepartmentID.Enabled = true;
                        //Bind Section if Department > 0
                        if (Convert.ToInt16(ddlDepartmentID.SelectedValue) > 0)
                        {
                            DataSet dsSections = new DataSet();
                            dacl.GetCompanySection(session.CompanyId, Convert.ToInt16(ddlDepartmentID.SelectedValue), 0, session.UserId, year, month, ref dsSections);
                            foreach (DataRow dr in dsSections.Tables[0].Rows)
                            {
                                ddlSectionID.Items.Add(new ListItem(dr["SectionName"].ToString(), dr["SectionID"].ToString()));
                            }
                            ddlSectionID.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlDim1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim1 = (DropDownList)sender;
            DropDownList ddlDim2 = this.ddlSearchDim2;
            DropDownList ddlDim3 = this.ddlSearchDim3;
            DropDownList ddlDim4 = this.ddlSearchDim4;
            short selectedvalue = Convert.ToInt16(ddlDim1.SelectedValue);

            ddlDim2.Items.Clear();
            ddlDim3.Items.Clear();
            ddlDim4.Items.Clear();
            ddlSalesperson.Items.Clear();


            ddlDim2.Enabled = true;
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsDepartments = new DataSet();
            short year = Convert.ToInt16(this.ddlYear.SelectedValue);
            short month = short.Parse(DateTime.Now.Month.ToString());

            dacl.GetCompanyDepartment(session.CompanyId, selectedvalue, 0, session.UserId, year, month, ref dsDepartments);
            foreach (DataRow dr in dsDepartments.Tables[0].Rows)
            {
                ddlDim2.Items.Add(new ListItem(dr["DepartmentName"].ToString(), dr["DepartmentID"].ToString()));
            }

            ddlSalesperson.Items.Clear();
            DataSet dsSp = new DataSet();
            dacl.RetrieveSalespersonBudget(session.CompanyId, session.UserId, selectedvalue, -1, -1, -1, ref dsSp);
            foreach (DataRow dr in dsSp.Tables[0].Rows)
            {
                ddlSalesperson.Items.Add(new ListItem(dr["SalesPersonNameID"].ToString(), dr["SalesPersonID"].ToString()));
            }


            ddlDim3.Enabled = false;
            ddlDim4.Enabled = false;
        }

        protected void ddlDim2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim2 = (DropDownList)sender;
            DropDownList ddlDim3 = this.ddlSearchDim3;
            DropDownList ddlDim4 = this.ddlSearchDim4;
            short selectedvalue = Convert.ToInt16(ddlDim2.SelectedValue);
            ddlDim3.Items.Clear();
            ddlDim4.Items.Clear();
            ddlSalesperson.Items.Clear();


            ddlDim3.Enabled = true;
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsSections = new DataSet();

            short year = Convert.ToInt16(this.ddlYear.SelectedValue);
            short month = short.Parse(DateTime.Now.Month.ToString());
            short reportId = 0;
            short loginUserOrAlternateParty = 0;

            dacl.GetCompanySection(session.CompanyId, selectedvalue, reportId, loginUserOrAlternateParty, year, month, ref dsSections);
            foreach (DataRow dr in dsSections.Tables[0].Rows)
            {
                ddlDim3.Items.Add(new ListItem(dr["SectionName"].ToString(), dr["SectionID"].ToString()));
            }

            ddlSalesperson.Items.Clear();
            DataSet dsSp = new DataSet();
            dacl.RetrieveSalespersonBudget(session.CompanyId, session.UserId, short.Parse(this.ddlSearchDim1.SelectedValue), selectedvalue, -1, -1, ref dsSp);
            foreach (DataRow dr in dsSp.Tables[0].Rows)
            {
                ddlSalesperson.Items.Add(new ListItem(dr["SalesPersonNameID"].ToString(), dr["SalesPersonID"].ToString()));
            }

            ddlDim4.Enabled = false;
        }

        protected void ddlDim3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim3 = (DropDownList)sender;
            DropDownList ddlDim4 = this.ddlSearchDim4;
            short selectedvalue = Convert.ToInt16(ddlDim3.SelectedValue);
            ddlDim4.Items.Clear();
            ddlSalesperson.Items.Clear();

            ddlDim4.Enabled = true;
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsUnits = new DataSet();

            dacl.GetCompanyUnit(session.CompanyId, selectedvalue, ref dsUnits);
            foreach (DataRow dr in dsUnits.Tables[0].Rows)
            {
                ddlDim4.Items.Add(new ListItem(dr["UnitName"].ToString(), dr["UnitID"].ToString()));
            }

            ddlSalesperson.Items.Clear();
            DataSet dsSp = new DataSet();
            dacl.RetrieveSalespersonBudget(session.CompanyId, session.UserId, short.Parse(this.ddlSearchDim1.SelectedValue), short.Parse(this.ddlSearchDim2.SelectedValue), selectedvalue, -1, ref dsSp);
            foreach (DataRow dr in dsSp.Tables[0].Rows)
            {
                ddlSalesperson.Items.Add(new ListItem(dr["SalesPersonNameID"].ToString(), dr["SalesPersonID"].ToString()));
            }

        }

        protected void ddlDim4_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim4 = (DropDownList)sender;
            short selectedvalue = Convert.ToInt16(ddlDim4.SelectedValue);

            ddlDim4.Enabled = true;
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();

            ddlSalesperson.Items.Clear();
            DataSet dsSp = new DataSet();
            dacl.RetrieveSalespersonBudget(session.CompanyId, session.UserId, short.Parse(this.ddlSearchDim1.SelectedValue), short.Parse(this.ddlSearchDim2.SelectedValue), short.Parse(this.ddlSearchDim3.SelectedValue), selectedvalue, ref dsSp);
            foreach (DataRow dr in dsSp.Tables[0].Rows)
            {
                ddlSalesperson.Items.Add(new ListItem(dr["SalesPersonNameID"].ToString(), dr["SalesPersonID"].ToString()));
            }

        }
        #endregion

        protected static bool IsEven(int value)
        {
            return value % 2 == 0;
        }

        protected void alertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        }
    }
}