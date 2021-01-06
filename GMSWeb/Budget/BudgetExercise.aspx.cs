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
   
    public partial class BudgetExercise : GMSBasePage
    {
        protected short loginUserOrAlternateParty = 0;
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
                                                                            179);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            179);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            UserAccessModule uAccessImport = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
                                                                            180);

            IList<UserAccessModuleForCompany> uAccessImportForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            180);

            //if (uAccessImport == null && (uAccessImportForCompanyList != null && uAccessImportForCompanyList.Count == 0))
            //    this.btnStart.Visible = false;

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
            ggdal.GetCompanyProject(session.CompanyId, session.UserId, 0, ref dsProjects);

            for (int j = 0; j < dsProjects.Tables[0].Rows.Count; j++)
            {
                this.ddlSearchDim1.Items.Add(new ListItem(dsProjects.Tables[0].Rows[j]["ProjectName"].ToString(), dsProjects.Tables[0].Rows[j]["ReportProjectID"].ToString()));
            }

            ddlSalesperson.Items.Clear();

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

            DataSet dsSp = new DataSet();
            ggdal.RetrieveSalespersonBudget(session.CompanyId, loginUserOrAlternateParty, -1, -1, -1, -1, ref dsSp);
            foreach (DataRow dr in dsSp.Tables[0].Rows)
            {
                ddlSalesperson.Items.Add(new ListItem(dr["SalesPersonNameID"].ToString(), dr["SalesPersonID"].ToString()));
            }
        }

        #region btnStart_Click
        protected void btnStart_Click(object sender, EventArgs e)
        {
            ImportBudgetData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadSpreadSheet();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ImportBudgetData();
        }

        protected void ImportBudgetData()
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
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

            short year = 0;
            string customerType = "";
            string type = "";
            string classType = "";
            string salespersonID = "";
            string salesexec = "ALL";
            short dim1 = -1; string d1 = "";
            short dim2 = -1; string d2 = "";
            short dim3 = -1; string d3 = "";
            short dim4 = -1; string d4 = "";

            year = short.Parse(this.ddlYear.SelectedValue);
            type = this.ddlType.SelectedValue; this.hidType.Value = this.ddlType.SelectedValue;
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


            //check is other import
            int noOfSalespersonImport = 0;
            DataSet dsImport = new DataSet();
            ggdal.CheckBudgetSetup(session.CompanyId, session.UserId, "ImportCheck", salespersonID,
                dim1, dim2, dim3, dim4, ref dsImport);
            noOfSalespersonImport = int.Parse(dsImport.Tables[0].Rows[0][0].ToString());
            if (noOfSalespersonImport > 0)
            {
                alertMessage("Someone is importing data. Please try again later.");
                return;
            }
            ggdal.CheckBudgetSetup(session.CompanyId, session.UserId, "ImportUpdate", salespersonID,
                dim1, dim2, dim3, dim4, ref dsImport);

            int noOfBudgetProduct;
            int noOfBudgetSalesperson;
            DataSet dsCheck = new DataSet();
            ggdal.CheckBudgetSetup(session.CompanyId, session.UserId, "SearchCheck", salespersonID,
                dim1, dim2, dim3, dim4, ref dsCheck);
            noOfBudgetProduct = int.Parse(dsCheck.Tables[0].Rows[0][0].ToString());
            noOfBudgetSalesperson = int.Parse(dsCheck.Tables[0].Rows[1][0].ToString());
            this.lblmessage.Text = "";
            if (noOfBudgetProduct < 3 || noOfBudgetSalesperson < 1)
            {
                string message = "";
                if (noOfBudgetProduct < 3)
                    message = message + " Less than 3 products are tagged to budget products. ";

                if (noOfBudgetSalesperson < 1)
                    message = message + " No Salesperson is tagged to budget salesperson. ";

                this.lblmessage.Text = "<p style=\"color: red\">" + message + "</p>";
            }
            else
            {
                ggdal.InsertBudgetData(session.CompanyId, year, loginUserOrAlternateParty, customerType,
                       salespersonID, classType, dim1, dim2, dim3, dim4);

                alertMessage("Budget Data Imported.");

                ggdal.CheckBudgetSetup(session.CompanyId, session.UserId, "ImportClear", salespersonID,
                dim1, dim2, dim3, dim4, ref dsImport);

                LoadSpreadSheet();
            }
        }
        #endregion

        #region RadSpreadsheet 
        protected void LoadSpreadSheet()
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet ds = new DataSet(); DataSet ds2 = new DataSet(); DataSet ds3 = new DataSet();
            DataSet ds4 = new DataSet(); DataSet ds5 = new DataSet(); DataSet ds6 = new DataSet();
            DataSet ds7 = new DataSet(); DataSet ds8 = new DataSet(); DataSet ds9 = new DataSet();
            DataSet ds10 = new DataSet(); DataSet ds11 = new DataSet(); DataSet ds12 = new DataSet();
            DataSet ds13 = new DataSet();
            string companyName = "";
            try
            {
                ggdal.RetrieveCompanyName(session.CompanyId, ref ds);
                companyName = ds.Tables[0].Rows[0][0].ToString();
                ds.Reset();
            }
            catch (Exception ex) { }

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

            short year = 0; short lastYear = 0; short nextYear = 0; short yearBeforeLastYear = 0; short yearAfterNextYear = 0;
            string customerType = "";
            string type = "";
            string classType = "";
            string salespersonID = "";
            string salesexec = "ALL";
            string customerCode = ""; string customerName = "";
            short dim1 = -1; string d1 = "";
            short dim2 = -1; string d2 = "";
            short dim3 = -1; string d3 = "";
            short dim4 = -1; string d4 = "";
           
            year = short.Parse(this.ddlYear.SelectedValue);
            nextYear = short.Parse((int.Parse(this.ddlYear.SelectedValue) + 1).ToString());
            lastYear = short.Parse((int.Parse(this.ddlYear.SelectedValue) - 1).ToString());
            yearBeforeLastYear = short.Parse((int.Parse(this.ddlYear.SelectedValue) - 2).ToString());
            yearAfterNextYear = short.Parse((int.Parse(this.ddlYear.SelectedValue) + 2).ToString());
            type = this.ddlType.SelectedValue; this.hidType.Value = this.ddlType.SelectedValue;
            customerType = this.ddlCustomerType.SelectedValue;
            classType = this.ddlClassType.SelectedValue;
            salespersonID = this.ddlSalesperson.SelectedValue;
            customerCode = this.ddlAccount.SelectedValue;
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
            if (this.ddlAccount.SelectedItem != null)
                customerName = this.ddlAccount.SelectedItem.Text.Substring(this.ddlAccount.SelectedItem.Text.IndexOf("-") + 1).TrimStart();//get string after "-"

            int noOfBudgetProduct;
            int noOfBudgetSalesperson;
            DataSet dsCheck = new DataSet();
            ggdal.CheckBudgetSetup(session.CompanyId, session.UserId, "SearchCheck", salespersonID,
                 dim1, dim2, dim3, dim4, ref dsCheck);
            noOfBudgetProduct = int.Parse(dsCheck.Tables[0].Rows[0][0].ToString());
            noOfBudgetSalesperson = int.Parse(dsCheck.Tables[0].Rows[1][0].ToString());
            this.lblmessage.Text = "";
            if (noOfBudgetProduct < 3 || noOfBudgetSalesperson < 1)
            {
                string message = "";
                if (noOfBudgetProduct < 3)
                    message = message + " Less than 3 products are tagged to budget products. ";

                if (noOfBudgetSalesperson < 1)
                    message = message + " No Salesperson is tagged to budget salesperson. ";

                this.lblmessage.Text = "<p style=\"color: red\">" + message + "</p>";
            }
            else
            {
                //try
                //{
                //ggdal.InsertBudgetData(session.CompanyId, year, session.UserId, customerType,
                //   salespersonID, classType, dim1, dim2, dim3, dim4);
                #region B11A
                //Actual
                ggdal.RetrieveBudgetActualSales(session.CompanyId, lastYear, loginUserOrAlternateParty, customerType,
                   salespersonID, classType, dim1, dim2, dim3, dim4, ref ds);//monthly figure

                ggdal.RetrieveBudgetDistributionRatio(session.CompanyId, lastYear, loginUserOrAlternateParty, customerType,
                   salespersonID, classType, dim1, dim2, dim3, dim4, "Actual", ref ds2);//current year distribution ratio

                ggdal.RetrieveBudgetDistributionRatio(session.CompanyId, yearBeforeLastYear, loginUserOrAlternateParty, customerType,
                   salespersonID, classType, dim1, dim2, dim3, dim4, "Actual", ref ds3);//prior year distribution ratio
                #endregion

                #region B12F
                //Forecast
                ggdal.RetrieveBudgetForecastSales(session.CompanyId, year, loginUserOrAlternateParty, customerType,
                    salespersonID, classType, dim1, dim2, dim3, dim4, ref ds4);//monthly figure

                ggdal.RetrieveBudgetDistributionRatio(session.CompanyId, lastYear, loginUserOrAlternateParty, customerType,
                   salespersonID, classType, dim1, dim2, dim3, dim4, "Actual", ref ds5);//prior year distribution ratio
                #endregion

                #region B13B
                //Budget
                ggdal.RetrieveBudgetBudgetSales(session.CompanyId, nextYear, loginUserOrAlternateParty, customerType,
                    salespersonID, classType, dim1, dim2, dim3, dim4, ref ds6);//monthly figure

                ggdal.RetrieveBudgetDistributionRatio(session.CompanyId, year, loginUserOrAlternateParty, customerType,
                   salespersonID, classType, dim1, dim2, dim3, dim4, "Forecast", ref ds7);//prior year distribution ratio
                #endregion

                #region B14A
                ggdal.RetrieveBudgetActualProductByCustomer(session.CompanyId, lastYear, loginUserOrAlternateParty, customerType,
               salespersonID, customerCode, dim1, dim2, dim3, dim4, ref ds8);//monthly figure

                ggdal.RetrieveBudgetCustomerMonthlySales(session.CompanyId, lastYear, loginUserOrAlternateParty,
                    salespersonID, customerCode, "Actual", dim1, dim2, dim3, dim4, customerType, ref ds9);//get customer monthly sales from B1A figure
                #endregion

                #region B15F
                ggdal.RetrieveBudgetForecastProductByCustomer(session.CompanyId, year, loginUserOrAlternateParty, customerType,
                  salespersonID, customerCode, dim1, dim2, dim3, dim4, ref ds10);//monthly figure

                ggdal.RetrieveBudgetCustomerMonthlySales(session.CompanyId, year, loginUserOrAlternateParty,
                    salespersonID, customerCode, "Forecast", dim1, dim2, dim3, dim4, customerType, ref ds11);//get customer monthly sales from B2F figure
                #endregion

                #region B16B
                ggdal.RetrieveBudgetBudgetProductByCustomer(session.CompanyId, nextYear, loginUserOrAlternateParty, customerType,
                  salespersonID, customerCode, dim1, dim2, dim3, dim4, ref ds12);//monthly figure

                ggdal.RetrieveBudgetCustomerMonthlySales(session.CompanyId, nextYear, loginUserOrAlternateParty,
                    salespersonID, customerCode, "Budget", dim1, dim2, dim3, dim4, customerType, ref ds13);//get customer monthly sales from B3B figure
                #endregion


                if (ds.Tables[0] != null &&  ds3.Tables[0] != null &&
                    ds4.Tables[0] != null && ds5.Tables[0] != null &&
                    ds6.Tables[0] != null && ds7.Tables[0] != null)
                {
                    #region Generate Worksheet from DataTable
                    this.resultList.Visible = true;
                    this.lblSearchSummary.Visible = false;
                    var sheet1 = FillWorksheetForSales(ds.Tables[0], ds2.Tables[0], ds3.Tables[0], session, "B11A ACTUAL SALES BY SALES EXEC BY CUSTOMER BY YEAR", classType,
                        lastYear + "/" + year.ToString(), d1, d2, d3, d4, salesexec, "Actual", companyName);
                    this.RadSpreadsheet1.Sheets.Add(sheet1);

                    this.resultList2.Visible = true;
                    this.lblSearchSummary2.Visible = false;
                    var sheet2 = FillWorksheetForSales(ds4.Tables[0], ds5.Tables[0], ds5.Tables[0], session, "B12F FORECAST SALES BY SALES EXEC BY CUSTOMER BY YEAR", classType,
                        year + "/" + nextYear.ToString(), d1, d2, d3, d4, salesexec, "Forecast", companyName);
                    this.RadSpreadsheet2.Sheets.Add(sheet2);

                    this.resultList3.Visible = true;
                    this.lblSearchSummary3.Visible = false;
                    var sheet3 = FillWorksheetForSales(ds6.Tables[0], ds7.Tables[0], ds7.Tables[0], session, "B13B BUDGET SALES BY SALES EXEC BY CUSTOMER BY YEAR", classType,
                        nextYear + "/" + yearAfterNextYear.ToString(), d1, d2, d3, d4, salesexec, "Budget", companyName);
                    this.RadSpreadsheet3.Sheets.Add(sheet3);


                    this.resultList4.Visible = true;
                    var sheet4 = FillWorksheetForProduct(ds8.Tables[0], ds9.Tables[0], session, "B14A ACTUAL SALES BY PRODUCT BY CUSTOMER", type,
                      lastYear + "/" + year.ToString(), d1, d2, d3, d4, salesexec, "Actual", customerName, yearBeforeLastYear.ToString());
                    this.RadSpreadsheet4.Sheets.Add(sheet4);

                    this.resultList5.Visible = true;
                    var sheet5 = FillWorksheetForProduct(ds10.Tables[0], ds11.Tables[0], session, "B15F FORECAST SALES BY PRODUCT BY CUSTOMER", type,
                      year + "/" + nextYear.ToString(), d1, d2, d3, d4, salesexec, "Forecast", customerName, lastYear.ToString());
                    this.RadSpreadsheet5.Sheets.Add(sheet5);

                    this.resultList6.Visible = true;
                    var sheet6 = FillWorksheetForProduct(ds12.Tables[0], ds13.Tables[0], session, "B16B BUDGET SALES BY PRODUCT BY CUSTOMER", type,
                      nextYear + "/" + yearAfterNextYear.ToString(), d1, d2, d3, d4, salesexec, "Budget", customerName, year.ToString());
                    this.RadSpreadsheet6.Sheets.Add(sheet6);

                    #endregion
                }
                #region styling & parse workboook value to hidden field for reload when postback
                var workbook = new Workbook();
                workbook.Sheets = RadSpreadsheet1.Sheets;
                SpreadsheetStylingB1A(workbook);
                var json = workbook.ToJson();
                HiddenField1.Value = json;

                var workbook2 = new Workbook();
                workbook2.Sheets = RadSpreadsheet2.Sheets;
                SpreadsheetStylingB2F(workbook2);
                var json2 = workbook2.ToJson();
                HiddenField2.Value = json2;

                var workbook3 = new Workbook();
                workbook3.Sheets = RadSpreadsheet3.Sheets;
                SpreadsheetStylingB3B(workbook3);
                var json3 = workbook3.ToJson();
                HiddenField3.Value = json3;


                var workbook4 = new Workbook();
                workbook4.Sheets = RadSpreadsheet4.Sheets;
                SpreadsheetStylingB4A(workbook4);
                var json4 = workbook4.ToJson();
                HiddenField4.Value = json4;

                var workbook5 = new Workbook();
                workbook5.Sheets = RadSpreadsheet5.Sheets;
                SpreadsheetStylingB5F(workbook5, type);
                var json5 = workbook5.ToJson();
                HiddenField5.Value = json5;

                var workbook6 = new Workbook();
                workbook6.Sheets = RadSpreadsheet6.Sheets;
                SpreadsheetStylingB6B(workbook6, type);
                var json6 = workbook6.ToJson();
                HiddenField6.Value = json6;

                #endregion

                this.hidRowChangedB12.Value = "";
                this.hidRowChangedB13.Value = "";
                this.hidRowChangedB15.Value = "";
                this.hidRowChangedB16.Value = "";
                this.RadMultiPage1.Visible = true;
                this.RadTabStrip1.Visible = true;
                if (this.ddlCustomerType.SelectedValue == "All")
                    this.btnGPB16.Visible = false;
                else
                {
                    if (!string.IsNullOrEmpty(salespersonID))
                        this.btnGPB16.Visible = false;
                    else
                        this.btnGPB16.Visible = true;
                }
                //}
                //catch (Exception ex)
                //{
                //    alertMessage(ex.ToString());
                //}
            }
        }

        private static Worksheet FillWorksheetForSales(DataTable data, DataTable data2, DataTable data3, LogSession session, string title, string type, string year,
            string d1, string d2, string d3, string d4, string salesExec, string budgetType, string companyName)
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
            var cellCurr = new Cell() { Index = currIndex, Value = "CURR: " + session.DefaultCurrency, Bold = true };
            firstRow.AddCell(cellCurr);
            if (budgetType == "Budget")
            {
                var cellTarget = new Cell() { Index = 15, Value = "Target Inc%: ", Bold = true };
                firstRow.AddCell(cellTarget);
                var targetInc = new Cell() { Index = 17, Value = 5, Bold = true, Format = "0" };
                firstRow.AddCell(targetInc);

                var cellBudget = new Cell() { Index = 18, Value = "Budget Inc%: ", Bold = true };
                firstRow.AddCell(cellBudget);
                var budgetInc = new Cell() { Index = 20, Value = 5, Bold = true, Format = "0 %" };
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
            var cellInSecondRow = new Cell() { Index = 0, Value = companyName, Bold = true };
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
                    var cell = new Cell() { Index = columnIndex++, Value = cellValue, Format = "#,###" };
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
                    var cell = new Cell() { Index = columnIndex, Value = 0, Bold = true, Formula = "IFERROR(SUM(" + alphabet + "6:" + alphabet + (rowIndex - 1) + "),0)", Format = "#,###" };
                    row.AddCell(cell);
                }
                else if ((columnIndex >= 27 && columnIndex <= 40) && budgetType == "Budget")//add this to calculate b11f total in B11B
                {
                    var cell = new Cell() { Index = columnIndex, Value = 0, Bold = true, Formula = "IFERROR(SUM(" + alphabet + "6:" + alphabet + (rowIndex - 1) + "),0)", Format = "#,###" };
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

        private static Worksheet FillWorksheetForProduct(DataTable data, DataTable data2, LogSession session, string title, string type, string year,
           string d1, string d2, string d3, string d4, string salesExec, string budgetType, string customer, string lastyear)
        {
            var workbook = new Workbook();
            var sheet = workbook.AddSheet();
            sheet.Columns = new List<Column>();
            sheet.FrozenRows = 5;//freeze row when scrolling
            int totalCol = 33; int typeIndex = 13; int yearIndex = 19; int currIndex = 25;

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
            var cellCurr = new Cell() { Index = currIndex, Value = "CURR: " + session.DefaultCurrency, Bold = true };
            firstRow.AddCell(cellCurr);

            if (budgetType == "Budget")
            {
                var cellBudget = new Cell() { Index = 29, Value = "Budget Inc%: ", Bold = true };
                firstRow.AddCell(cellBudget);
                var budgetInc = new Cell() { Index = 32, Value = 5, Bold = true, Format = "0 %" };
                firstRow.AddCell(budgetInc);
            }
            sheet.AddRow(firstRow);
            sheet.AddMergedCells("A1:F1");
            sheet.AddMergedCells("N1:Q1");
            sheet.AddMergedCells("T1:W1");
            sheet.AddMergedCells("Z1:AC1");
            if (budgetType == "Budget")
                sheet.AddMergedCells("AD1:AF1");

            #endregion

            #region second row header
            var secondRow = new Row() { Index = 1 };
            var cellInSecondRow = new Cell() { Index = 0, Value = customer, Bold = true };
            secondRow.AddCell(cellInSecondRow);
            for (int i = 2; i <= totalCol; i++)
            {
                var cell = new Cell() { Index = i, Value = "", Bold = true };
                secondRow.AddCell(cell);
            }
            var cellDim1 = new Cell() { Index = 7, Value = "D1: " + d1, Bold = true };
            secondRow.AddCell(cellDim1);
            var cellDim2 = new Cell() { Index = 13, Value = "D2: " + d2, Bold = true };
            secondRow.AddCell(cellDim2);
            var cellDim3 = new Cell() { Index = 19, Value = "D3: " + d3, Bold = true };
            secondRow.AddCell(cellDim3);
            var cellDim4 = new Cell() { Index = 25, Value = "D4: " + d4, Bold = true };
            secondRow.AddCell(cellDim4);
            var cellSalesExec = new Cell() { Index = 29, Value = "SALES EXEC: " + salesExec, Bold = true };
            secondRow.AddCell(cellSalesExec);

            sheet.AddRow(secondRow);
            sheet.AddMergedCells("A2:C2");
            sheet.AddMergedCells("H2:K2");
            sheet.AddMergedCells("N2:Q2");
            sheet.AddMergedCells("T2:W2");
            sheet.AddMergedCells("Z2:AC2");
            sheet.AddMergedCells("AD2:AG2");
            #endregion

            #region data column header
            #region monthly column
            var row = new Row() { Index = 2 };
            int columnIndex = 0;
            foreach (DataColumn dataColumn in data.Columns)
            {
                sheet.Columns.Add(new Column());
                string cellValue = dataColumn.ColumnName;

                if (cellValue.Contains("Qty") && type == "Quantity")
                    cellValue = cellValue.Replace("Qty", string.Empty);

                var cell = new Cell() { Index = columnIndex++, Value = cellValue, Bold = true, TextAlign = "center" };
                row.AddCell(cell);
            }
            var cellQty = new Cell() { Index = 28, Value = "Total", Bold = true, TextAlign = "center" };//column header created in for loop not allow duplicate name, need to overwrite
            row.AddCell(cellQty);
            var cellQty2 = new Cell() { Index = 31, Value = lastyear, Bold = true, TextAlign = "center" };//column header created in for loop not allow duplicate name, need to overwrite
            row.AddCell(cellQty2);
            if (budgetType == "Actual")
            {
                var cell = new Cell() { Index = 30, Value = "B14A GP %", Bold = true, TextAlign = "center" };//column header created in for loop not allow duplicate name, need to overwrite
                row.AddCell(cell);

                var cellGP = new Cell() { Index = 33, Value = lastyear+" GP %", Bold = true, TextAlign = "center" };//column header created in for loop not allow duplicate name, need to overwrite
                row.AddCell(cellGP);
            }
            if (budgetType == "Budget")
            {
                var cell = new Cell() { Index = 30, Value = "Budget GP %", Bold = true, TextAlign = "center" };//column header created in for loop not allow duplicate name, need to overwrite
                row.AddCell(cell);

                var cellGP = new Cell() { Index = 33, Value = "B15F GP %", Bold = true, TextAlign = "center" };//column header created in for loop not allow duplicate name, need to overwrite
                row.AddCell(cellGP);
            }
            if (budgetType == "Forecast")
            {
                var cell = new Cell() { Index = 30, Value = "B15F GP %", Bold = true, TextAlign = "center" };//column header created in for loop not allow duplicate name, need to overwrite
                row.AddCell(cell);

                var cellGP = new Cell() { Index = 33, Value = "B14A GP %", Bold = true, TextAlign = "center" };//column header created in for loop not allow duplicate name, need to overwrite
                row.AddCell(cellGP);
            }
            sheet.AddRow(row);
            #endregion
            #region actual or forecast or budget
            var row2 = new Row() { Index = 3 };
            for (int i = 0; i <= totalCol; i++)
            {
                var cell = new Cell() { Index = i, Value = "", Bold = true };
                row2.AddCell(cell);
            }
            for (int i = 4; i <= 27; i++)//set month is actual or forecast or budget
            {
                if (budgetType == "Actual")
                {
                    var cell = new Cell() { Index = i, Value = "Act", Bold = true };
                    row2.AddCell(cell);
                }
                else if (budgetType == "Forecast")
                {
                    Cell cell;
                    if (i <= 17)
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
            #endregion
            for (int i = 1; i <= 34; i++)
            {
                var alphabet = IntToAlpha(i);
                var alphabet2 = IntToAlpha(i + 1);
                var alphabet3 = IntToAlpha(i + 2);
                if (i <= 4 || i > 28)
                {
                    if (type != "Quantity")
                        sheet.AddMergedCells(alphabet + "3:" + alphabet + "5");
                    else
                    {
                        if (i == 29 || i == 32)
                            sheet.AddMergedCells(alphabet + "3:" + alphabet3 + "4");
                    }
                }
                else
                {
                    if (!IsEven(i) && type == "Quantity" && i < 28)
                    {
                        sheet.AddMergedCells(alphabet + "3:" + alphabet2 + "3");
                        sheet.AddMergedCells(alphabet + "4:" + alphabet2 + "4");
                    }
                }
            }
            #endregion

            #region row for gas
            var row3 = new Row() { Index = 4 };
            for (int i = 0; i <= totalCol; i++)
            {
                var cell = new Cell() { Index = i, Value = "", Bold = true };
                row3.AddCell(cell);
            }
            for (int i = 4; i <= 32; i++)//set month is actual or forecast or budget
            {
                if (type == "Quantity" && i != 30)
                {
                    if ((IsEven(i) && i != 32) || i == 31)
                    {
                        var cell = new Cell() { Index = i, Value = "Vol", Bold = true };
                        row3.AddCell(cell);
                    }
                    else if (!IsEven(i) || i == 32)
                    {
                        var cell = new Cell() { Index = i, Value = "Sales", Bold = true };
                        row3.AddCell(cell);
                    }
                }
            }
            if (type == "Quantity")
            {
                var cell = new Cell() { Index = 30, Value = "GP %", Bold = true };
                row3.AddCell(cell);
                var cellGP2 = new Cell() { Index = 33, Value = "GP %", Bold = true };
                row3.AddCell(cellGP2);
            }
            sheet.AddRow(row3);
            if (type != "Quantity")
                row3.Height = 0;
            #endregion

            int rowIndex = 5;
            #region add customer's monthly sales

            foreach (DataRow dataRow in data2.Rows)
            {
                row = new Row() { Index = rowIndex++ };
                columnIndex = 0;
                for (int i = 0; i <= totalCol; i++)//fill row with cell first
                {
                    var cell = new Cell() { Index = i, Value = "", Format = "0", Bold = true };
                    row.AddCell(cell);
                }
                foreach (DataColumn dataColumn in data2.Columns)
                {
                    string cellValue = dataRow[dataColumn.ColumnName].ToString();
                    var cell = new Cell() { Index = columnIndex++, Value = cellValue, Format = "#,###", Bold = true };
                    row.AddCell(cell);
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
                    if (dataColumn.ColumnName.Contains("GP"))
                    {
                        string cellValue = dataRow[dataColumn.ColumnName].ToString();
                        var cell = new Cell() { Index = columnIndex++, Value = cellValue, Format = "0.0", Italic = true };
                        row.AddCell(cell);
                    }
                    else
                    {
                        string cellValue = dataRow[dataColumn.ColumnName].ToString();
                        var cell = new Cell() { Index = columnIndex++, Value = cellValue, Format = "#,###" };
                        row.AddCell(cell);
                    }

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
                else if ((columnIndex >= 4 && columnIndex <= 29) || columnIndex == 31 || columnIndex == 32)
                {
                    var cell = new Cell() { Index = columnIndex, Value = 0, Bold = true, Formula = "IFERROR(ROUND(SUM(" + alphabet + "7:" + alphabet + (rowIndex - 1) + "),0),0)", Format = "#,###" };
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

            #region add variance row
            row = new Row() { Index = rowIndex++ };
            columnIndex = 0;
            for (columnIndex = 0; columnIndex <= totalCol; columnIndex++)
            {
                var alphabet = IntToAlpha(columnIndex + 1);
                var list = new List<int> { 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29 };//amount column index for every month & total
                if (columnIndex == 2)
                {
                    var cell = new Cell() { Index = columnIndex, Value = "Variance", Bold = true };
                    row.AddCell(cell);
                }
                else if (list.Contains(columnIndex))//to specific Amount column only
                {
                    var cell = new Cell() { Index = columnIndex, Value = 0, Bold = true, Formula = "IFERROR(ABS(" + alphabet + (rowIndex - 1) + " - " + alphabet + "6),0)", Format = "0",Enable=false};
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

            sheet.Columns[0].Width = 30;
            sheet.Columns[1].Width = 190;
            sheet.Columns[2].Width = 60;
            for (int i = 3; i < sheet.Columns.Count; i++)
            {
                sheet.Columns[i].Width = 60;
                if (((IsEven(i) && i <= 29) || i == 3 || i == 31) && type != "Quantity")
                    sheet.Columns[i].Width = 0;
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

        protected void SpreadsheetStylingB1A(Workbook workbook)
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
                //if (i == workbook.Sheets[0].Rows.Count - 1)
                //{
                //    for (int j = 7; j <= 19; j++)
                //    {
                //        var alphabet = IntToAlpha(j + 1);
                //        row.Cells[j].Formula = "IFERROR((" + alphabet + (i) + "/T" + (i) + ")*100,0)";
                //    }
                //}
            }
            #endregion
        }

        protected void SpreadsheetStylingB2F(Workbook workbook)
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
            var forecastRow = workbook.Sheets[0].Rows[3];
            for (int i = 27; i <= 31; i++)
            {
                forecastRow.Cells[i].Color = "#FF6600";
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
                            for (int j = 14; j <= 18; j++)//set forecast's font color & formula
                            {
                                var alphabet = IntToAlpha(j + 1);
                                row.Cells[j].Color = "#0033FF";
                                row.Cells[j].Formula = "IFERROR(ROUND((SUM(H" + (i + 1) + ":N" + (i + 1) + ")/SUM(H5:N5)) *" + alphabet + "5,0),0)";
                            }
                        }
                        row.Cells[19].Formula = "IFERROR(SUM(H" + (i + 1) + ":S" + (i + 1) + "),0)";
                    }
                    else if (i == workbook.Sheets[0].Rows.Count - 1)
                    {
                        for (int j = 7; j <= 19; j++)
                        {
                            var alphabet = IntToAlpha(j + 1);
                            row.Cells[j].Formula = "IFERROR((" + alphabet + (i) + "/T" + (i) + ")*100,0)";
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

        protected void SpreadsheetStylingB3B(Workbook workbook)
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
            workbook.Sheets[0].Rows[0].Cells[22].Formula = "IFERROR((T" + currentTotalRow + "-AO" + currentTotalRow + ")/AO" + currentTotalRow + ",0)";//formula to get Budget Inc%
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
                for (int j = 7; j <= 19; j++)
                {
                    row.Cells[j].TextAlign = "right";
                }
                row.Cells[20].TextAlign = "center";
                //set all cell to double to enable formula calculation
                for (int j = 7; j <= 18; j++)
                {
                    row.Cells[j].Value = double.Parse(row.Cells[j].Value.ToString());
                }
                //set formula for the budget month
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
                                row.Cells[j].Formula = "IFERROR((" + alphabet2 + (i + 1) + "*(1+R1/100)),0)";//b11f figure * target inc value
                            }
                        }
                        row.Cells[19].Formula = "IFERROR(SUM(H" + (i + 1) + ":S" + (i + 1) + "),0)";//total of each month

                        row.Cells[20].Format = "0 %";
                        row.Cells[20].Formula = "IFERROR(((SUM(H" + (i + 1) + ":S" + (i + 1) + ")-SUM(AB" + (i + 1) + ":AM" + (i + 1) + "))/SUM(AB" + (i + 1) + ":AM" + (i + 1) + ")),0)";//Inc% of current 12 mth/ b11f 12 mth
                    }
                    else if (i == workbook.Sheets[0].Rows.Count - 1)
                    {
                        for (int j = 7; j <= 18; j++)
                        {
                            var alphabet = IntToAlpha(j + 1);
                            row.Cells[j].Formula = "IFERROR((" + alphabet + (i) + "/T" + (i) + ")*100,0)";//current distribution ratio
                        }
                    }
                    else
                    {
                        row.Cells[40].Formula = "IFERROR(SUM(AB" + (i + 1) + ":AM" + (i + 1) + "),0)";//total of 12 month b11f
                    }
                }
            }
            #endregion
        }

        protected void SpreadsheetStylingB4A(Workbook workbook)
        {
            #region header style
            for (int i = 0; i <= 1; i++)
            {
                foreach (var cell in workbook.Sheets[0].Rows[i].Cells)
                {
                    cell.Wrap = false;
                    cell.Bold = true;
                    cell.Background = "#769fcd";
                    cell.Color = "#F6F6F6";
                    cell.TextAlign = "Left";
                    cell.VerticalAlign = "center";
                }
            }
            #endregion

            #region header column name style
            for (int i = 2; i <= 4; i++)
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
            for (int i = 5; i < workbook.Sheets[0].Rows.Count; i++)
            {
                var row = workbook.Sheets[0].Rows[i];
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
                for (int j = 0; j <= 3; j++)
                {
                    if (j == 1)//only customer column align left
                        row.Cells[j].TextAlign = "left";
                    else
                        row.Cells[j].TextAlign = "center";
                }
                for (int j = 4; j <= 33; j++)
                {
                    try
                    {
                        row.Cells[j].TextAlign = "right";
                    }
                    catch (Exception ex) { }
                }
            }
            #endregion
        }

        protected void SpreadsheetStylingB5F(Workbook workbook, string type)
        {
            #region header style
            for (int i = 0; i <= 1; i++)
            {
                foreach (var cell in workbook.Sheets[0].Rows[i].Cells)
                {
                    cell.Wrap = false;
                    cell.Bold = true;
                    cell.Background = "#878ecd";
                    cell.Color = "#F6F6F6";
                    cell.TextAlign = "Left";
                    cell.VerticalAlign = "center";
                }
            }
            #endregion

            #region header column name style
            for (int i = 2; i <= 4; i++)
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
            var forecastRow = workbook.Sheets[0].Rows[3];
            for (int i = 48; i <= 57; i++)
            {
                forecastRow.Cells[i].Color = "#FF6600";//set orange color for forecast colomn
            }
            #endregion

            #region item style
            for (int i = 5; i < workbook.Sheets[0].Rows.Count; i++)
            {
                var row = workbook.Sheets[0].Rows[i];

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
                        cell.Background = "#dff4f3";
                }
                //text align
                for (int j = 0; j <= 3; j++)
                {
                    if (j == 1)//only customer column align left
                        row.Cells[j].TextAlign = "left";
                    else
                        row.Cells[j].TextAlign = "center";
                }
                for (int j = 4; j <= 33; j++)
                {
                    try
                    {
                        row.Cells[j].TextAlign = "right";
                    }
                    catch (Exception ex) { }
                }
                //set formula for the forecast month
                if (i > 5)
                {
                    if (i < workbook.Sheets[0].Rows.Count - 2)//to exclude total row
                    {
                        if (string.IsNullOrEmpty(row.Cells[35].Value.ToString()))//check status column value
                        {
                            var listVol = new List<int> { 18, 20, 22, 24, 26 };//volume column index for every month & total
                            var listAmount = new List<int> { 19, 21, 23, 25, 27 };//amount column index for every month
                            if (type == "Quantity")
                            {
                                foreach (var j in listAmount)
                                {
                                    var alphabet = IntToAlpha(j);
                                    row.Cells[j].Color = "#0033FF";//set font color
                                    row.Cells[j].Formula = "IFERROR(D" + (i + 1) + "*" + alphabet + (i + 1) + "/1000,0)";//b11f figure * target inc value
                                }
                                foreach (var j in listVol)
                                {
                                    row.Cells[j].Color = "#0033FF";//set font color
                                }
                            }
                            else
                            {
                                foreach (var j in listAmount)
                                {
                                    row.Cells[j].Color = "#0033FF";//set font color
                                }
                            }
                        }
                        row.Cells[28].Formula = "IFERROR(ROUND(SUM(E" + (i + 1) + ",G" + (i + 1) + ",I" + (i + 1) + ",K" + (i + 1) +
                                                  ",M" + (i + 1) + ",O" + (i + 1) + ",Q" + (i + 1) + ",S" + (i + 1) +
                                                  ",U" + (i + 1) + ",W" + (i + 1) + ",Y" + (i + 1) + ",AA" + (i + 1) + "),0),0)";//set formula total qty of the row
                        row.Cells[29].Formula = "IFERROR(ROUND(SUM(F" + (i + 1) + ",H" + (i + 1) + ",J" + (i + 1) + ",L" + (i + 1) +
                                                   ",N" + (i + 1) + ",P" + (i + 1) + ",R" + (i + 1) + ",T" + (i + 1) +
                                                   ",V" + (i + 1) + ",X" + (i + 1) + ",Z" + (i + 1) + ",AB" + (i + 1) + "),0),0)";//set formula total of the row
                    }
                }
            }
            #endregion
        }

        protected void SpreadsheetStylingB6B(Workbook workbook, string type)
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
            workbook.Sheets[0].Rows[0].Cells[32].TextAlign = "right";
            int currentTotalRow = workbook.Sheets[0].Rows.Count - 1;
            workbook.Sheets[0].Rows[0].Cells[33].Formula = "IFERROR((AD" + currentTotalRow + "-AG" + currentTotalRow + ")/AG" + currentTotalRow + ",0)";//formula to get Budget Inc%
            #endregion

            #region header column name style
            for (int i = 2; i <= 4; i++)
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
            for (int i = 5; i < workbook.Sheets[0].Rows.Count; i++)
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
                for (int j = 0; j <= 3; j++)
                {
                    if (j == 1)//only customer column align left
                        row.Cells[j].TextAlign = "left";
                    else
                        row.Cells[j].TextAlign = "center";
                }
                for (int j = 4; j <= 33; j++)
                {
                    try
                    {
                        row.Cells[j].TextAlign = "right";
                    }
                    catch (Exception ex) { }
                }
                //set formula for the budget month
                if (i > 4)
                {
                    if (i < workbook.Sheets[0].Rows.Count - 2)//to exclude total row
                    {
                        if (string.IsNullOrEmpty(row.Cells[35].Value.ToString()) && i > 4)//check status column value
                        {
                            var listVol = new List<int> { 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26 };//volume column index for every month
                            var listSales = new List<int> { 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27 };//amount column index for every month
                            if (type == "Quantity")
                            {
                                foreach (var j in listSales)
                                {
                                    var alphabet = IntToAlpha(j);
                                    row.Cells[j].Color = "#0033FF";//set font color
                                    row.Cells[j].Formula = "IFERROR(D" + (i + 1) + "*" + alphabet + (i + 1) + "/1000,0)";//b11f figure * target inc value
                                }
                                foreach (var j in listVol)
                                {
                                    var alphabet2 = IntToAlpha(j + 33);
                                    row.Cells[j].Color = "#0033FF";//set font color
                                    row.Cells[j].Formula = "IFERROR((" + alphabet2 + (i + 1) + "*(1+5/100)),0)";//b6f figure * target inc value
                                }
                            }
                            else
                            {
                                foreach (var j in listSales)
                                {
                                    var alphabet2 = IntToAlpha(j + 33);
                                    row.Cells[j].Color = "#0033FF";//set font color
                                    row.Cells[j].Formula = "IFERROR((" + alphabet2 + (i + 1) + "*(1+5/100)),0)";//b5f figure * 5%
                                }
                            }
                        }
                        row.Cells[28].Formula = "IFERROR(ROUND(SUM(E" + (i + 1) + ",G" + (i + 1) + ",I" + (i + 1) + ",K" + (i + 1) +
                                                  ",M" + (i + 1) + ",O" + (i + 1) + ",Q" + (i + 1) + ",S" + (i + 1) +
                                                  ",U" + (i + 1) + ",W" + (i + 1) + ",Y" + (i + 1) + ",AA" + (i + 1) + "),0),0)";//set formula total qty of the row
                        row.Cells[29].Formula = "IFERROR(ROUND(SUM(F" + (i + 1) + ",H" + (i + 1) + ",J" + (i + 1) + ",L" + (i + 1) +
                                                   ",N" + (i + 1) + ",P" + (i + 1) + ",R" + (i + 1) + ",T" + (i + 1) +
                                                   ",V" + (i + 1) + ",X" + (i + 1) + ",Z" + (i + 1) + ",AB" + (i + 1) + "),0),0)";//set formula total of the row
                    }
                }
            }
            #endregion
        }
        #endregion

        #region button click event
        protected void btnB2F_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            var workbook = Workbook.FromJson(HiddenField2.Value);
            string rowChanged = this.hidRowChangedB12.Value;//get row changed value
            int[] listRowChanged = rowChanged.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToInt32(c)).ToArray();
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
                        if (rowIndex < workbook.Sheets[0].Rows.Count - 2)//to exclude total row
                        {
                            if (row.Cells[14].Color == "#000000")//only black color font means data is from database.
                            {
                                if (listRowChanged.Length != 0)//check whether there is row changed that stored in array of the hidden field
                                {
                                    bool isRowChanged = listRowChanged.Contains(rowIndex + 5);
                                    if (isRowChanged)//save only the row changed
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
                                }
                            }
                            else//for those first time open B11F and did not save any b11f data 
                            {//this will loop all workbook to save the data into database
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
                        }
                        rowIndex++;
                    }
                    alertMessage("B12F Saved.");
                    LoadSpreadSheet();
                }
                catch (Exception ex)
                {
                    alertMessage(ex.ToString());
                }
            }
        }

        protected void btnB3B_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            var workbook = Workbook.FromJson(HiddenField3.Value);
            string rowChanged = this.hidRowChangedB13.Value;//get row changed value
            int[] listRowChanged = rowChanged.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToInt32(c)).ToArray();
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
                        if (rowIndex < workbook.Sheets[0].Rows.Count - 2)//to exclude total row
                        {
                            if (row.Cells[7].Color == "#000000")//only black color font means data is from database.
                            {
                                if (listRowChanged.Length != 0)//check whether there is row changed that stored in array of the hidden field
                                {
                                    bool isRowChanged = listRowChanged.Contains(rowIndex + 5);
                                    if (isRowChanged)//save only the row changed
                                    {
                                        int year = int.Parse(this.ddlYear.SelectedValue) + 1;
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
                                                apr, may, jun, jul, aug, sep, oct,
                                                nov, dec, jan, feb, mar, total, isTotalNml);
                                        }
                                        else
                                        {
                                            if (salespersonID != "")
                                            {
                                                ggdal.InsertBudgetNewAccount(session.CompanyId, session.UserId, year, newAccName,
                                                apr, may, jun, jul, aug, sep, oct,
                                                nov, dec, jan, feb, mar, total, salespersonID);
                                            }
                                        }
                                    }
                                }
                            }else
                            {
                                int year = int.Parse(this.ddlYear.SelectedValue) + 1;
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
                                        apr, may, jun, jul, aug, sep, oct,
                                        nov, dec, jan, feb, mar, total, isTotalNml);
                                }
                                else
                                {
                                    if (salespersonID != "")
                                    {
                                        ggdal.InsertBudgetNewAccount(session.CompanyId, session.UserId, year, newAccName,
                                        apr, may, jun, jul, aug, sep, oct,
                                        nov, dec, jan, feb, mar, total, salespersonID);
                                    }
                                }
                            }
                        }
                        rowIndex++;
                    }
                    alertMessage("B13B Saved.");
                    LoadSpreadSheet();
                }
                catch (Exception ex)
                {
                    alertMessage(ex.ToString());
                }
            }
        }

        protected void btnB5F_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            var workbook = Workbook.FromJson(HiddenField5.Value);
            string rowChanged = this.hidRowChangedB15.Value;//get row changed value
            int[] listRowChanged = rowChanged.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToInt32(c)).ToArray();
            for (int i = 0; i < 6; i++)
            {   //exclude title, header column,& Monthly Sales
                workbook.Sheets[0].Rows.Remove(workbook.Sheets[0].Rows[0]);
            }
            if (workbook.Sheets[0].Rows.Count > 0)
            {
                try
                {
                    int rowIndex2 = 0;
                    foreach (var row in workbook.Sheets[0].Rows)
                    {
                        if (rowIndex2 == workbook.Sheets[0].Rows.Count - 1)
                        {
                            double nov = double.Parse(row.Cells[19].Value.ToString());
                            double dec = double.Parse(row.Cells[21].Value.ToString());
                            double jan = double.Parse(row.Cells[23].Value.ToString());
                            double feb = double.Parse(row.Cells[25].Value.ToString());
                            double mar = double.Parse(row.Cells[27].Value.ToString());
                            double total = double.Parse(row.Cells[29].Value.ToString());

                            if (nov != 0 || dec != 0 || jan != 0 || feb != 0 || mar != 0)
                            {
                                alertMessage("All variance must be zero.");
                                return;
                            }
                        }
                        rowIndex2++;
                    }
                    int rowIndex = 0;
                    foreach (var row in workbook.Sheets[0].Rows)
                    {
                        if (rowIndex < workbook.Sheets[0].Rows.Count - 2)//to exclude total row
                        {
                            if (row.Cells[19].Color == "#000000")// black color font means data is from database.
                            {
                                if (listRowChanged.Length != 0)//check whether there is row changed that stored in array of the hidden field
                                {
                                    bool isRowChanged = listRowChanged.Contains(rowIndex + 6);
                                    if (isRowChanged)//save only the row changed
                                    {
                                        int year = int.Parse(this.ddlYear.SelectedValue);
                                        double qtyNov = double.Parse(row.Cells[18].Value.ToString());
                                        double qtyDec = double.Parse(row.Cells[20].Value.ToString());
                                        double qtyJan = double.Parse(row.Cells[22].Value.ToString());
                                        double qtyFeb = double.Parse(row.Cells[24].Value.ToString());
                                        double qtyMar = double.Parse(row.Cells[26].Value.ToString());
                                        double qtyTotal = double.Parse(row.Cells[28].Value.ToString());

                                        double nov = double.Parse(row.Cells[19].Value.ToString());
                                        double dec = double.Parse(row.Cells[21].Value.ToString());
                                        double jan = double.Parse(row.Cells[23].Value.ToString());
                                        double feb = double.Parse(row.Cells[25].Value.ToString());
                                        double mar = double.Parse(row.Cells[27].Value.ToString());
                                        double total = double.Parse(row.Cells[29].Value.ToString());

                                        int ID = 0;
                                        if (!string.IsNullOrEmpty(row.Cells[34].Value.ToString()))
                                        {
                                            ID = int.Parse(row.Cells[34].Value.ToString());
                                            ggdal.UpdateBudgetProductForecast(session.CompanyId, session.UserId, year, ID, nov, dec, jan, feb, mar, total,
                                                qtyNov, qtyDec, qtyJan, qtyFeb, qtyMar, qtyTotal);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                int year = int.Parse(this.ddlYear.SelectedValue);
                                double qtyNov = double.Parse(row.Cells[18].Value.ToString());
                                double qtyDec = double.Parse(row.Cells[20].Value.ToString());
                                double qtyJan = double.Parse(row.Cells[22].Value.ToString());
                                double qtyFeb = double.Parse(row.Cells[24].Value.ToString());
                                double qtyMar = double.Parse(row.Cells[26].Value.ToString());
                                double qtyTotal = double.Parse(row.Cells[28].Value.ToString());

                                double nov = double.Parse(row.Cells[19].Value.ToString());
                                double dec = double.Parse(row.Cells[21].Value.ToString());
                                double jan = double.Parse(row.Cells[23].Value.ToString());
                                double feb = double.Parse(row.Cells[25].Value.ToString());
                                double mar = double.Parse(row.Cells[27].Value.ToString());
                                double total = double.Parse(row.Cells[29].Value.ToString());

                                int ID = 0;
                                if (!string.IsNullOrEmpty(row.Cells[34].Value.ToString()))
                                {
                                    ID = int.Parse(row.Cells[34].Value.ToString());
                                    ggdal.UpdateBudgetProductForecast(session.CompanyId, session.UserId, year, ID, nov, dec, jan, feb, mar, total,
                                        qtyNov, qtyDec, qtyJan, qtyFeb, qtyMar, qtyTotal);
                                }
                            }
                        }
                        rowIndex++;
                    }
                    alertMessage("B15F Saved.");
                    LoadSpreadSheet();
                }
                catch (Exception ex)
                {
                    alertMessage(ex.ToString());
                }
            }
        }

        protected void btnB6B_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            var workbook = Workbook.FromJson(HiddenField6.Value);
            string rowChanged = this.hidRowChangedB16.Value;//get row changed value
            int[] listRowChanged = rowChanged.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToInt32(c)).ToArray();
            for (int i = 0; i < 6; i++)
            {   //exclude title, header column,& Monthly Sales
                workbook.Sheets[0].Rows.Remove(workbook.Sheets[0].Rows[0]);
            }
            if (workbook.Sheets[0].Rows.Count > 0)
            {
                try
                {
                    int rowIndex2 = 0;
                    foreach (var row in workbook.Sheets[0].Rows)
                    {
                        if (rowIndex2 == workbook.Sheets[0].Rows.Count - 1)
                        {
                            double apr = double.Parse(row.Cells[5].Value.ToString());
                            double may = double.Parse(row.Cells[7].Value.ToString());
                            double jun = double.Parse(row.Cells[9].Value.ToString());
                            double jul = double.Parse(row.Cells[11].Value.ToString());
                            double aug = double.Parse(row.Cells[13].Value.ToString());
                            double sep = double.Parse(row.Cells[15].Value.ToString());
                            double oct = double.Parse(row.Cells[17].Value.ToString());
                            double nov = double.Parse(row.Cells[19].Value.ToString());
                            double dec = double.Parse(row.Cells[21].Value.ToString());
                            double jan = double.Parse(row.Cells[23].Value.ToString());
                            double feb = double.Parse(row.Cells[25].Value.ToString());
                            double mar = double.Parse(row.Cells[27].Value.ToString());
                            double total = double.Parse(row.Cells[29].Value.ToString());

                            if (apr != 0 || may != 0 || jun != 0 || jul != 0 || aug != 0 || sep != 0 ||
                                oct != 0 || nov != 0 || dec != 0 || jan != 0 || feb != 0 || mar != 0)
                            {
                                alertMessage("All variance must be zero.");
                                return;
                            }
                        }
                        rowIndex2++;
                    }
                    int rowIndex = 0;
                    foreach (var row in workbook.Sheets[0].Rows)
                    {
                        if (rowIndex < workbook.Sheets[0].Rows.Count - 2)//to exclude total row
                        {
                            if (row.Cells[5].Color == "#000000")// black color font means data is from database.
                            {
                                if (listRowChanged.Length != 0)//check whether there is row changed that stored in array of the hidden field
                                {
                                    bool isRowChanged = listRowChanged.Contains(rowIndex + 6);
                                    if (isRowChanged)//save only the row changed
                                    {
                                        int year = int.Parse(this.ddlYear.SelectedValue) + 1;
                                        string newAccName = row.Cells[2].Value.ToString();
                                        string salespersonID = this.ddlSalesperson.SelectedValue;

                                        double qtyApr = double.Parse(row.Cells[4].Value.ToString());
                                        double qtyMay = double.Parse(row.Cells[6].Value.ToString());
                                        double qtyJun = double.Parse(row.Cells[8].Value.ToString());
                                        double qtyJul = double.Parse(row.Cells[10].Value.ToString());
                                        double qtyAug = double.Parse(row.Cells[12].Value.ToString());
                                        double qtySep = double.Parse(row.Cells[14].Value.ToString());
                                        double qtyOct = double.Parse(row.Cells[16].Value.ToString());
                                        double qtyNov = double.Parse(row.Cells[18].Value.ToString());
                                        double qtyDec = double.Parse(row.Cells[20].Value.ToString());
                                        double qtyJan = double.Parse(row.Cells[22].Value.ToString());
                                        double qtyFeb = double.Parse(row.Cells[24].Value.ToString());
                                        double qtyMar = double.Parse(row.Cells[26].Value.ToString());
                                        double qtyTotal = double.Parse(row.Cells[28].Value.ToString());

                                        double apr = double.Parse(row.Cells[5].Value.ToString());
                                        double may = double.Parse(row.Cells[7].Value.ToString());
                                        double jun = double.Parse(row.Cells[9].Value.ToString());
                                        double jul = double.Parse(row.Cells[11].Value.ToString());
                                        double aug = double.Parse(row.Cells[13].Value.ToString());
                                        double sep = double.Parse(row.Cells[15].Value.ToString());
                                        double oct = double.Parse(row.Cells[17].Value.ToString());
                                        double nov = double.Parse(row.Cells[19].Value.ToString());
                                        double dec = double.Parse(row.Cells[21].Value.ToString());
                                        double jan = double.Parse(row.Cells[23].Value.ToString());
                                        double feb = double.Parse(row.Cells[25].Value.ToString());
                                        double mar = double.Parse(row.Cells[27].Value.ToString());
                                        double total = double.Parse(row.Cells[29].Value.ToString());

                                        int ID = 0;
                                        if (!string.IsNullOrEmpty(row.Cells[34].Value.ToString()))
                                        {
                                            ID = int.Parse(row.Cells[34].Value.ToString());
                                            ggdal.UpdateBudgetProductBudget(session.CompanyId, session.UserId, year, ID,
                                                apr, may, jun, jul, aug, sep, oct,
                                                nov, dec, jan, feb, mar, total,
                                                qtyApr, qtyMay, qtyJun, qtyJul, qtyAug, qtySep, qtyOct,
                                                qtyNov, qtyDec, qtyJan, qtyFeb, qtyMar, qtyTotal
                                                );
                                        }
                                        else
                                        {
                                            if (salespersonID != "")
                                            {
                                                ggdal.InsertBudgetNewAccount(session.CompanyId, session.UserId, year, newAccName,
                                                apr, may, jun, jul, aug, sep, oct,
                                                nov, dec, jan, feb, mar, total, salespersonID);
                                            }
                                        }
                                    }
                                }
                            }else
                            {
                                int year = int.Parse(this.ddlYear.SelectedValue) + 1;
                                string newAccName = row.Cells[2].Value.ToString();
                                string salespersonID = this.ddlSalesperson.SelectedValue;

                                double qtyApr = double.Parse(row.Cells[4].Value.ToString());
                                double qtyMay = double.Parse(row.Cells[6].Value.ToString());
                                double qtyJun = double.Parse(row.Cells[8].Value.ToString());
                                double qtyJul = double.Parse(row.Cells[10].Value.ToString());
                                double qtyAug = double.Parse(row.Cells[12].Value.ToString());
                                double qtySep = double.Parse(row.Cells[14].Value.ToString());
                                double qtyOct = double.Parse(row.Cells[16].Value.ToString());
                                double qtyNov = double.Parse(row.Cells[18].Value.ToString());
                                double qtyDec = double.Parse(row.Cells[20].Value.ToString());
                                double qtyJan = double.Parse(row.Cells[22].Value.ToString());
                                double qtyFeb = double.Parse(row.Cells[24].Value.ToString());
                                double qtyMar = double.Parse(row.Cells[26].Value.ToString());
                                double qtyTotal = double.Parse(row.Cells[28].Value.ToString());

                                double apr = double.Parse(row.Cells[5].Value.ToString());
                                double may = double.Parse(row.Cells[7].Value.ToString());
                                double jun = double.Parse(row.Cells[9].Value.ToString());
                                double jul = double.Parse(row.Cells[11].Value.ToString());
                                double aug = double.Parse(row.Cells[13].Value.ToString());
                                double sep = double.Parse(row.Cells[15].Value.ToString());
                                double oct = double.Parse(row.Cells[17].Value.ToString());
                                double nov = double.Parse(row.Cells[19].Value.ToString());
                                double dec = double.Parse(row.Cells[21].Value.ToString());
                                double jan = double.Parse(row.Cells[23].Value.ToString());
                                double feb = double.Parse(row.Cells[25].Value.ToString());
                                double mar = double.Parse(row.Cells[27].Value.ToString());
                                double total = double.Parse(row.Cells[29].Value.ToString());

                                int ID = 0;
                                if (!string.IsNullOrEmpty(row.Cells[34].Value.ToString()))
                                {
                                    ID = int.Parse(row.Cells[34].Value.ToString());
                                    ggdal.UpdateBudgetProductBudget(session.CompanyId, session.UserId, year, ID,
                                        apr, may, jun, jul, aug, sep, oct,
                                        nov, dec, jan, feb, mar, total,
                                        qtyApr, qtyMay, qtyJun, qtyJul, qtyAug, qtySep, qtyOct,
                                        qtyNov, qtyDec, qtyJan, qtyFeb, qtyMar, qtyTotal
                                        );
                                }
                                else
                                {
                                    if (salespersonID != "")
                                    {
                                        ggdal.InsertBudgetNewAccount(session.CompanyId, session.UserId, year, newAccName,
                                        apr, may, jun, jul, aug, sep, oct,
                                        nov, dec, jan, feb, mar, total, salespersonID);
                                    }
                                }
                            }
                        }
                        rowIndex++;
                    }
                    alertMessage("B16B Saved.");
                    LoadSpreadSheet();
                }
                catch (Exception ex)
                {
                    alertMessage(ex.ToString());
                }
            }
        }

        protected void btnGPB16_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            var workbook = Workbook.FromJson(HiddenField6.Value);
            string rowChanged = this.hidRowChangedB16.Value;//get row changed value
            int[] listRowChanged = rowChanged.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToInt32(c)).ToArray();
            for (int i = 0; i < 6; i++)
            {   //exclude title, header column,& Monthly Sales
                workbook.Sheets[0].Rows.Remove(workbook.Sheets[0].Rows[0]);
            }
            if (workbook.Sheets[0].Rows.Count > 0)
            {
                int rowIndex = 0;
                foreach (var row in workbook.Sheets[0].Rows)
                {
                    if (rowIndex < workbook.Sheets[0].Rows.Count - 2)//to exclude total row
                    {
                        if (listRowChanged.Length != 0)//check whether there is row changed that stored in array of the hidden field
                        {
                            bool isRowChanged = listRowChanged.Contains(rowIndex + 6);
                            if (isRowChanged)//save only the row changed
                            {
                                short year = short.Parse((int.Parse(this.ddlYear.SelectedValue) + 1).ToString());
                                string budgetProduct = row.Cells[1].Value.ToString().Substring(0, row.Cells[1].Value.ToString().IndexOf("-")).Trim();
                                double GP = double.Parse(row.Cells[30].Value.ToString());

                                short dim1 = -1; short dim2 = -1; short dim3 = -1; short dim4 = -1;
                                dim1 = short.Parse(this.ddlSearchDim1.SelectedValue);
                                if (!string.IsNullOrEmpty(this.ddlSearchDim2.SelectedValue))
                                    dim2 = short.Parse(this.ddlSearchDim2.SelectedValue);
                                if (!string.IsNullOrEmpty(this.ddlSearchDim3.SelectedValue))
                                    dim3 = short.Parse(this.ddlSearchDim3.SelectedValue);
                                if (!string.IsNullOrEmpty(this.ddlSearchDim4.SelectedValue))
                                    dim4 = short.Parse(this.ddlSearchDim4.SelectedValue);

                                string type = "";
                                if (!string.IsNullOrEmpty(this.ddlCustomerType.SelectedValue))
                                    type = this.ddlCustomerType.SelectedValue;

                                int ID = 0;
                                if (!string.IsNullOrEmpty(row.Cells[34].Value.ToString()))
                                {
                                    ID = int.Parse(row.Cells[34].Value.ToString());
                                    if (ID == 0)
                                        ggdal.UpdateBudgetGP(session.CompanyId, year, session.UserId, budgetProduct,
                                            GP, "B", dim1, dim2, dim3, dim4,type);
                                }
                            }
                        }
                    }
                    rowIndex++;
                }
                alertMessage("B16F's GP Saved.");
                LoadSpreadSheet();
            }
        }
        #endregion

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

                short year = Convert.ToInt16((int.Parse(this.ddlYear.SelectedValue) + 1).ToString());
                short month = 3;

                DataSet dsProjects = new DataSet();
                dacl.GetCompanyProject(session.CompanyId, session.UserId, 0, ref dsProjects);
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

                short dim1 = -1; short dim2 = -1; short dim3 = -1; short dim4 = -1;
                dim1 = short.Parse(this.ddlSearchDim1.SelectedValue);
                if (!string.IsNullOrEmpty(this.ddlSearchDim2.SelectedValue))
                    dim2 = short.Parse(this.ddlSearchDim2.SelectedValue);
                if (!string.IsNullOrEmpty(this.ddlSearchDim3.SelectedValue))
                    dim3 = short.Parse(this.ddlSearchDim3.SelectedValue);
                if (!string.IsNullOrEmpty(this.ddlSearchDim4.SelectedValue))
                    dim4 = short.Parse(this.ddlSearchDim4.SelectedValue);

                ddlSalesperson.Items.Clear();
                DataSet dsSp = new DataSet();
                dacl.RetrieveSalespersonBudget(session.CompanyId, session.UserId, dim1, dim2, dim3, dim4, ref dsSp);
                foreach (DataRow dr in dsSp.Tables[0].Rows)
                {
                    ddlSalesperson.Items.Add(new ListItem(dr["SalesPersonNameID"].ToString(), dr["SalesPersonID"].ToString()));
                }
            }
            catch (Exception ex)
            {
            }
            this.RadMultiPage1.Visible = false;
            this.RadTabStrip1.Visible = false;
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
            this.ddlAccount.Items.Clear();
            this.ddlAccField.Visible = false;

            ddlDim2.Enabled = true;
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsDepartments = new DataSet();
            short year = Convert.ToInt16((int.Parse(this.ddlYear.SelectedValue) + 1).ToString());
            short month = 3;

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

            this.RadMultiPage1.Visible = false;
            this.RadTabStrip1.Visible = false;
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
            this.ddlAccount.Items.Clear();
            this.ddlAccField.Visible = false;

            ddlDim3.Enabled = true;
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsSections = new DataSet();

            short year = Convert.ToInt16((int.Parse(this.ddlYear.SelectedValue) + 1).ToString());
            short month = 3;
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
            this.RadMultiPage1.Visible = false;
            this.RadTabStrip1.Visible = false;
            ddlDim4.Enabled = false;
        }

        protected void ddlDim3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim3 = (DropDownList)sender;
            DropDownList ddlDim4 = this.ddlSearchDim4;
            short selectedvalue = Convert.ToInt16(ddlDim3.SelectedValue);
            ddlDim4.Items.Clear();
            ddlSalesperson.Items.Clear();
            this.ddlAccount.Items.Clear();
            this.ddlAccField.Visible = false;

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
            this.RadMultiPage1.Visible = false;
            this.RadTabStrip1.Visible = false;
        }

        protected void ddlDim4_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDim4 = (DropDownList)sender;
            short selectedvalue = Convert.ToInt16(ddlDim4.SelectedValue);

            ddlDim4.Enabled = true;
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();

            this.ddlAccount.Items.Clear();
            this.ddlAccField.Visible = false;
            ddlSalesperson.Items.Clear();
            DataSet dsSp = new DataSet();
            dacl.RetrieveSalespersonBudget(session.CompanyId, session.UserId, short.Parse(this.ddlSearchDim1.SelectedValue), short.Parse(this.ddlSearchDim2.SelectedValue), short.Parse(this.ddlSearchDim3.SelectedValue), selectedvalue, ref dsSp);
            foreach (DataRow dr in dsSp.Tables[0].Rows)
            {
                ddlSalesperson.Items.Add(new ListItem(dr["SalesPersonNameID"].ToString(), dr["SalesPersonID"].ToString()));
            }
            this.RadMultiPage1.Visible = false;
            this.RadTabStrip1.Visible = false;
        }

        protected void ddlSalesperson_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlSalesperson = (DropDownList)sender;
            string selectedvalue = ddlSalesperson.SelectedValue;
            this.ddlAccount.Items.Clear();
            this.ddlAccField.Visible = false;
            if (selectedvalue != "")
            {
                LogSession session = base.GetSessionInfo();
                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DataSet dsSp = new DataSet();
                dacl.RetrieveCustomerBudget(session.CompanyId, short.Parse(this.ddlYear.SelectedValue), ddlSalesperson.SelectedValue, ref dsSp);
                this.ddlAccField.Visible = false;
                foreach (DataRow dr in dsSp.Tables[0].Rows)
                {
                    this.ddlAccount.Items.Add(new ListItem(dr["AccountName"].ToString(), dr["AccountCode"].ToString()));
                    this.ddlAccField.Visible = true;
                }
            }
            this.RadMultiPage1.Visible = false;
            this.RadTabStrip1.Visible = false;
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

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            (sender as RadGrid).DataSource = RetrieveVariance("F");
        }

        protected void RadGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            (sender as RadGrid).DataSource = RetrieveVariance("B");
        }

        protected void RadGrid1_FilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
        {
            string DataField = (e.Column as IGridDataColumn).GetActiveDataField();
            DataTable dt = RetrieveVariance("F");


            DataView dvOri = new DataView(dt);
            DataTable dtOri = dvOri.ToTable(true, DataField);
            e.ListBox.DataSource = dtOri;
            e.ListBox.DataKeyField = DataField;
            e.ListBox.DataTextField = DataField;
            e.ListBox.DataValueField = DataField;
            e.ListBox.DataBind();
        }

        protected void RadGrid2_FilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
        {
            string DataField = (e.Column as IGridDataColumn).GetActiveDataField();
            DataTable dt = RetrieveVariance("B");

            DataView dvOri = new DataView(dt);
            DataTable dtOri = dvOri.ToTable(true, DataField);
            e.ListBox.DataSource = dtOri;
            e.ListBox.DataKeyField = DataField;
            e.ListBox.DataTextField = DataField;
            e.ListBox.DataValueField = DataField;
            e.ListBox.DataBind();
        }

        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
            {
                foreach (GridColumn column in RadGrid1.MasterTableView.Columns)
                {
                    column.ListOfFilterValues = null; // CheckList values set to null will uncheck all the checkboxes

                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;

                    column.AndCurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.AndCurrentFilterValue = string.Empty;
                }
                RadGrid1.MasterTableView.FilterExpression = string.Empty;
                RadGrid1.MasterTableView.Rebind();
            }
        }

        protected void RadGrid2_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
            {
                foreach (GridColumn column in RadGrid2.MasterTableView.Columns)
                {
                    column.ListOfFilterValues = null; // CheckList values set to null will uncheck all the checkboxes

                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;

                    column.AndCurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.AndCurrentFilterValue = string.Empty;
                }
                RadGrid2.MasterTableView.FilterExpression = string.Empty;
                RadGrid2.MasterTableView.Rebind();
            }
        }

        protected void btnVariance_Click(object sender, EventArgs e)
        {
            this.RadGrid1.Rebind();
            this.RadGrid2.Rebind();
        }

        protected DataTable RetrieveVariance(string doctype)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            short year = short.Parse(this.ddlYear.SelectedValue);
            if (doctype == "B")
                year = short.Parse((int.Parse(this.ddlYear.SelectedValue) + 1).ToString());
            short dim1 = -1; short dim2 = -1; short dim3 = -1; short dim4 = -1;
            dim1 = short.Parse(this.ddlSearchDim1.SelectedValue);
            if (!string.IsNullOrEmpty(this.ddlSearchDim2.SelectedValue))
                dim2 = short.Parse(this.ddlSearchDim2.SelectedValue);
            if (!string.IsNullOrEmpty(this.ddlSearchDim3.SelectedValue))
                dim3 = short.Parse(this.ddlSearchDim3.SelectedValue);
            if (!string.IsNullOrEmpty(this.ddlSearchDim4.SelectedValue))
                dim4 = short.Parse(this.ddlSearchDim4.SelectedValue);

            ggdal.SelectBudgetVariance(session.CompanyId, session.UserId, doctype, year, dim1, dim2, dim3, dim4, ref ds);

            return ds.Tables[0];
        }
    }
}