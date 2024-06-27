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
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Organization.Upload
{
    public partial class UploadParsing : GMSBasePage
    {
        protected string excelFilePath = "", excelFileName="";
        private short year = 0;
        private short itemPurposeId = 0;
        private short ProjectID = -1;
        private short DepartmentID = -1;
        private short SectionID = -1;
        private short UnitID = -1;
        private string customerType = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            excelFileName = Server.UrlDecode(Request.Params["FILENAME"]);
            year = GMSUtil.ToShort(Request.Params["YEAR"]);
            itemPurposeId = GMSUtil.ToShort(Request.Params["PURPOSEID"]);
            //projectId = GMSUtil.ToShort(Request.Params["PROJECTID"]);
            //customerType = Request.Params["CUSTOMERTYPE"].ToString();
            //departmentId = GMSUtil.ToShort(Request.Params["DEPARTMENTID"]);

            excelFilePath = AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + Path.DirectorySeparatorChar + excelFileName;

            Response.ContentType = "text/html";

            if (!Page.IsPostBack)
                ParseExcelFile();
        }

        private void parseFinanceExcelFile() {

            DataSet budgetExcel = new DataSet();

            Asiasoft.MSExcelFileReader.SheetDataLoader financeSheetDataLoader_Budget = new Asiasoft.MSExcelFileReader.SheetDataLoader();

            financeSheetDataLoader_Budget.ExcelFilePath = excelFilePath;
            financeSheetDataLoader_Budget.IsHeaderIncludedInExcelFile = false;
            financeSheetDataLoader_Budget.SheetName = "Sheet1";
            financeSheetDataLoader_Budget.LoadExcelData();
            budgetExcel = financeSheetDataLoader_Budget.ExcelData;

            LogSession sess = base.GetSessionInfo();
            bool IsBudgetCOA2016 = false;
            int hiddenRowCount = budgetExcel.Tables[0].Rows.Count - 1;
            string[] hiddenValue = budgetExcel.Tables[0].Rows[hiddenRowCount][1].ToString().Split(',');
            string companyName = budgetExcel.Tables[0].Rows[0][1].ToString();
            string dim1 = hiddenValue[1];
            string dim2 = hiddenValue[2];
            string dim3 = hiddenValue[3];
            string dim4 = hiddenValue[4];

            


            Response.Output.Write("Analysing excel file...<br>");
            try
            {

                //isCoa2016
                Company CompanyObj = new SystemDataActivity().RetrieveCompanyById(sess.CompanyId, sess);
                if (CompanyObj.Is2016COA != null && CompanyObj.Is2016COA.CompareTo(DateTime.Now) < 0)
                    IsBudgetCOA2016 = true;

                //company verification
                if (!CompanyObj.Name.ToUpper().Equals(companyName))
                    throw new Exception("Different Company");

                //get dimension 1 id
                CompanyProject ProjectObj = new SystemDataActivity().RetrieveCompanyProjectByCountryAndName(dim1, sess.CompanyId);
                if (ProjectObj != null)
                    ProjectID = (short)ProjectObj.ProjectID;
                //get dimention 2 id
                CompanyDepartment DepartmentObj = new SystemDataActivity().RetrieveCompanyDepartmentByCountryAndName(dim2, sess.CompanyId);
                if (DepartmentObj != null)
                    DepartmentID = (short)DepartmentObj.DepartmentID;

                //get dimention 3 id
                int retrievedSectionID = new SystemDataActivity().GetSectionIDByNameAndCountry(dim3, sess.CompanyId);
                if (retrievedSectionID > 0)
                    SectionID = (short)retrievedSectionID;

                //get dimention 4 id         
                int retrievedUnitID = new SystemDataActivity().GetUnitIDByNameAndCountry(dim4, sess.CompanyId);
                if (retrievedUnitID > 0)
                    UnitID = (short)retrievedUnitID;

                Response.Output.Write("Company : " + companyName + "<br>");
                Response.Output.Write("Is2016COA : " + IsBudgetCOA2016 + "<br>");
                Response.Output.Write("DIM 1 : " + dim1  + "<br>");
                Response.Output.Write("DIM 2 : " + dim2  + "<br>");
                Response.Output.Write("DIM 3 : " + dim3  + "<br>");
                Response.Output.Write("DIM 4 : " + dim4  + "<br>");


                // item name
                for (int i = 4; i < budgetExcel.Tables[0].Rows.Count-1; i++)
                {
                    string itemName = budgetExcel.Tables[0].Rows[i][1].ToString();
                    IList<FinanceItem> lstItem = new SystemDataActivity().RetrieveFinanceItemByName(itemName);

                    if (lstItem != null && lstItem.Count > 0)
                    {
                        FinanceItem item = (FinanceItem)lstItem[0];

                        int escape = 1; // 1 because of escaping sn cell
                        
                        //delete from tbBudgetForFinance records related to this itemID and year then insert new record
                        ResultType result = new BudgetActivity().DeleteBudgetForFinanceByYearItemID(item.ItemID, this.year, IsBudgetCOA2016 ? 3 : 12, sess.CompanyId, ProjectID, DepartmentID, SectionID, UnitID);

                        if (result == ResultType.Ok)
                        {
                            #region insert for each month
                            for (short j = 1; j <= 12; j++)
                            {
                                BudgetForFinance budget = new BudgetForFinance();
                                budget.CoyID = sess.CompanyId;
                                budget.ProjectID = ProjectID;
                                budget.DepartmentID = DepartmentID;
                                budget.SectionID = SectionID;
                                budget.UnitID = UnitID;
                                budget.BudgetYear = this.year;
                                budget.BudgetMonth = j;
                                budget.ItemID = item.ItemID;
                                budget.CreatedBy = sess.UserId;
                                budget.CreatedDate = DateTime.Now;
                                
                                if (IsBudgetCOA2016)
                                {
                                    #region switch j COA2016
                                    switch (j)
                                    {
                                        case 1:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][1 + escape].ToString());
                                            budget.BudgetMonth = 4;
                                            break;
                                        case 2:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][2 + escape].ToString());
                                            budget.BudgetMonth = 5;
                                            break;
                                        case 3:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][3 + escape].ToString());
                                            budget.BudgetMonth = 6;
                                            break;
                                        case 4:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][4 + escape].ToString());
                                            budget.BudgetMonth = 7;
                                            break;
                                        case 5:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][5 + escape].ToString());
                                            budget.BudgetMonth = 8;
                                            break;
                                        case 6:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][6 + escape].ToString());
                                            budget.BudgetMonth = 9;
                                            break;
                                        case 7:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][7 + escape].ToString());
                                            budget.BudgetMonth = 10;
                                            break;
                                        case 8:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][8 + escape].ToString());
                                            budget.BudgetMonth = 11;
                                            break;
                                        case 9:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][9 + escape].ToString());
                                            budget.BudgetMonth = 12;
                                            break;
                                        case 10:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][10 + escape].ToString());
                                            budget.BudgetYear = (Int16)(this.year + 1);
                                            budget.BudgetMonth = 1;
                                            break;
                                        case 11:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][11 + escape].ToString());
                                            budget.BudgetYear = (Int16)(this.year + 1);
                                            budget.BudgetMonth = 2;
                                            break;
                                        case 12:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][12 + escape].ToString());
                                            budget.BudgetYear = (Int16)(this.year + 1);
                                            budget.BudgetMonth = 3;
                                            break;
                                    }
                                    #endregion switch j
                                }
                                else
                                {
                                    #region switch j COA2011
                                    switch (j)
                                    {
                                        case 1:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][j + escape].ToString());
                                            break;
                                        case 2:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][j + escape].ToString());
                                            break;
                                        case 3:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][j + escape].ToString());
                                            break;
                                        case 4:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][j + escape].ToString());
                                            break;
                                        case 5:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][j + escape].ToString());
                                            break;
                                        case 6:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][j + escape].ToString());
                                            break;
                                        case 7:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][j + escape].ToString());
                                            break;
                                        case 8:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][8 + escape].ToString());
                                            break;
                                        case 9:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][9 + escape].ToString());
                                            break;
                                        case 10:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][10 + escape].ToString());
                                            break;
                                        case 11:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][11 + escape].ToString());
                                            break;
                                        case 12:
                                            budget.Total = GMSUtil.ToFloat(budgetExcel.Tables[0].Rows[i][12 + escape].ToString());
                                            break;
                                    }
                                    #endregion switch j
                                }

                                Response.Output.Write("Inserting Budget detail for Item Name: '" + item.ItemName + "' for month " + j + "...<br>");
                                Response.Flush();
                                try
                                {
                                    //insert
                                    
                                    ResultType create = new BudgetActivity().CreateBudgetForFinance(ref budget, sess);

                                    if (create == ResultType.Ok)
                                    {
                                        Response.Output.Write("Inserting successful.<br>");
                                        Response.Flush();
                                    }
                                    else
                                    {
                                        //Response.Output.Write("<SPAN STYLE='color: red'>Processing error of type : " + result.ToString() + ".</SPAN><br>");
                                        Response.Flush();
                                    }                                
                                }
                                catch (Exception ex)
                                {
                                    Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error:" + ex.Message + ".</SPAN><br>");
                                    Response.Flush();
                                }

                                //if(item.ItemName == "Profit After Taxation %" && j.ToString() == "12")
                                    

                            }
                            #endregion
                           
                        }
                        
                        Response.Output.Write("<br>");
                    }
                    else
                    {
                        Response.Output.Write("<SPAN STYLE='color: red'>Item Name '" + budgetExcel.Tables[0].Rows[i][i].ToString() + "' cannot be found at row " + (i) + ".</SPAN><br><br>");
                        Response.Flush();
                    }                    
                    
                    File.Delete(excelFilePath);
                }
                //Response.Output.Write("Updating YTD total for all items...<br>");
                //Response.Flush();
                //new GMSGeneralDALC().procUpdateBudgetSummary(sess.CompanyId, sess.FYE, this.ProjectID, this.DepartmentID, this.SectionID, this.UnitID, this.year);
                //new GMSGeneralDALC().procUpdateBudgetPerformanceIndicators(sess.CompanyId, sess.FYE, this.ProjectID, this.DepartmentID, this.SectionID, this.UnitID, this.year);
            }
            catch (Exception ex)
            {
                Response.Output.Write("<SPAN STYLE='color: red'>Error:" + ex.Message + ".</SPAN><br>");
                Response.Flush();
            }
            finally
            {
                Response.Output.Write("Updating YTD total for all items....<br>");
                Response.Flush();
                new GMSGeneralDALC().procUpdateBudgetSummary(sess.CompanyId, sess.FYE, this.ProjectID, this.DepartmentID, this.SectionID, this.UnitID, this.year);
                new GMSGeneralDALC().procUpdateBudgetPerformanceIndicators(sess.CompanyId, sess.FYE, this.ProjectID, this.DepartmentID, this.SectionID, this.UnitID, this.year);
                Response.Output.Write("Updating YTD total successful.....<br>");
                Response.Flush();
                Response.Output.Write("<SPAN STYLE='color: red'>End of insertion.</SPAN><br><br>");
                Response.Flush();
            }

        }

        private void ParseExcelFile()
        {
            LogSession sess = base.GetSessionInfo();
            DataSet dsExcel = new DataSet();
            
            try
            {
                Response.Output.Write("Parsing excel file...<br>");
                Response.Flush();

                Asiasoft.MSExcelFileReader.SheetDataLoader sheetDataLoader_Budget = new Asiasoft.MSExcelFileReader.SheetDataLoader();
                sheetDataLoader_Budget.ExcelFilePath = excelFilePath;
                sheetDataLoader_Budget.IsHeaderIncludedInExcelFile = true;
                sheetDataLoader_Budget.SheetName = "Sheet1";
                sheetDataLoader_Budget.LoadExcelData();

                dsExcel = sheetDataLoader_Budget.ExcelData;

                //Modified By OSS 14 Oct 2011 to add budget for product and customer 
                //and budget for sales/product unit
                if (dsExcel.Tables[0].Columns.Contains("ProductGroupCode"))
                {
                    #region upload budget for product
                    

                    for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
                    {
                        #region for each row
                        
                        string pg = dsExcel.Tables[0].Rows[i]["ProductGroupCode"].ToString();
                        string type = dsExcel.Tables[0].Rows[i]["Sales&GP%"].ToString();

                        ProductGroup prodGroup = ProductGroup.RetrieveByKey(sess.CompanyId, pg);

                        if (prodGroup != null)
                        {
                            // delete from tbBudgetForProduct records related to this productGroup and year before insertion
                            // only delete if type = "Sales"

                            ResultType result; 
                            
                            if (type == "Sales")
                                result = new BudgetActivity().DeleteBudgetForProductByYearProductGroupCode(prodGroup.ProductGroupCode, this.year, sess.CompanyId, customerType);
                            else
                                result = ResultType.Ok; 
                            
                            

                            if (result == ResultType.Ok)
                            {
                                #region insert for each month
                                for (short j = 1; j <= 12; j++)
                                {
                                    BudgetForProduct budget;

                                    if (type == "Sales")
                                    {
                                        budget = new BudgetForProduct();
                                        budget.CoyID = sess.CompanyId;
                                        budget.BudgetYear = this.year;
                                        budget.BudgetMonth = j;
                                        budget.ProductGroupCode = prodGroup.ProductGroupCode;
                                        budget.CustomerType = customerType;
                                    }
                                    else
                                        budget = BudgetForProduct.RetrieveByKey(sess.CompanyId, this.year, j, prodGroup.ProductGroupCode, customerType); 

                                    Double budgetValue = 0; 

                                    #region switch j
                                    switch (j)
                                    {
                                        case 1:
                                            budgetValue = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Jan"].ToString()) * 1000;
                                            break;
                                        case 2:
                                            budgetValue = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Feb"].ToString()) * 1000;
                                            break;
                                        case 3:
                                            budgetValue = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Mar"].ToString()) * 1000;
                                            break;
                                        case 4:
                                            budgetValue = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Apr"].ToString()) * 1000;
                                            break;
                                        case 5:
                                            budgetValue = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["May"].ToString()) * 1000;
                                            break;
                                        case 6:
                                            budgetValue = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Jun"].ToString()) * 1000;
                                            break;
                                        case 7:
                                            budgetValue = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Jul"].ToString()) * 1000;
                                            break;
                                        case 8:
                                            budgetValue = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Aug"].ToString()) * 1000;
                                            break;
                                        case 9:
                                            budgetValue = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Sep"].ToString()) * 1000;
                                            break;
                                        case 10:
                                            budgetValue = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Oct"].ToString()) * 1000;
                                            break;
                                        case 11:
                                            budgetValue = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Nov"].ToString()) * 1000;
                                            break;
                                        case 12:
                                            budgetValue = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Dec"].ToString()) * 1000;
                                            break;
                                    }
                                    #endregion

                                    if (type == "Sales")
                                    {
                                        budget.SalesBudget = budgetValue;
                                        Response.Output.Write("Inserting Budget detail (Sales) for Product Group Name: '" + prodGroup.ProductGroupName + "' for month " + j + "...<br>");
                                    }
                                    else
                                    {
                                        budget.GPBudget = budgetValue / 1000; 
                                        Response.Output.Write("Updating Budget detail (GP%) for Product Group Name: '" + prodGroup.ProductGroupName + "' for month " + j + "...<br>");
                                    }

                                    
                                    Response.Flush();
                                    //if (budgetValue != 0)
                                    //{
                                        try
                                        {
                                            budget.CreatedBy = sess.UserId;
                                            budget.CreatedDate = DateTime.Now;
                                            ResultType create = new BudgetActivity().CreateUpdateBudgetForProduct(ref budget, sess);
                                            if (create == ResultType.Ok)
                                            {
                                                Response.Output.Write("Inserting/updating successful.<br>");
                                                Response.Flush();

                                                File.Delete(excelFilePath);
                                            }
                                            else
                                            {
                                                Response.Output.Write("<SPAN STYLE='color: red'>Processing error of type : " + result.ToString() + ".</SPAN><br>");
                                                Response.Flush();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Response.Output.Write("<SPAN STYLE='color: red'>Inserting/updating fail. Error:" + ex.Message + ".</SPAN><br>");
                                            Response.Flush();
                                        }
                                    //}
                                    //else
                                    //{
                                      //  Response.Output.Write("<SPAN STYLE='color: red'>Data not inserted/updated because value is 0.</SPAN><br>");
                                      //    Response.Flush();
                                    //}

                                }
                                #endregion
                            }
                            Response.Output.Write("<br>");

                        }
                        else
                        {
                            Response.Output.Write("<SPAN STYLE='color: red'>Product Group Name '" + dsExcel.Tables[0].Rows[i]["ProductGroupName"].ToString() + "' cannot be found at row " + (i + 1) + ".</SPAN><br><br>");
                            Response.Flush();
                        }
                        #endregion for each row
                    }//end for
                    #endregion upload budget for product
                }
                else if (dsExcel.Tables[0].Columns.Contains("CustomerCode"))
                {
                    #region upload budget for customer
                    

                    for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
                    {
                        #region for each row 
                        string customerCode = dsExcel.Tables[0].Rows[i]["CustomerCode"].ToString();
                        A21Account customer = A21Account.RetrieveByKey(sess.CompanyId, customerCode);

                        if (customer != null)
                        {
                            // delete from tbBudgetForCustomer records related to this customer and year before insertion
                            ResultType result = new BudgetActivity().DeleteBudgetForCustomerByYearAccountCode(customer.AccountCode, this.year, sess.CompanyId);

                            if (result == ResultType.Ok)
                            {
                                #region insert for each month
                                for (short j = 1; j <= 12; j++)
                                {
                                    BudgetForCustomer budget = new BudgetForCustomer();
                                    budget.CoyID = sess.CompanyId;
                                    budget.BudgetYear = this.year;
                                    budget.BudgetMonth = j;
                                    budget.AccountCode = customer.AccountCode; 
                                    
                                    #region switch j
                                    switch (j)
                                    {
                                        case 1:
                                            budget.Total = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Jan"].ToString()) * 1000;
                                            break;
                                        case 2:
                                            budget.Total = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Feb"].ToString()) * 1000;
                                            break;
                                        case 3:
                                            budget.Total = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Mar"].ToString()) * 1000;
                                            break;
                                        case 4:
                                            budget.Total = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Apr"].ToString()) * 1000;
                                            break;
                                        case 5:
                                            budget.Total = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["May"].ToString()) * 1000;
                                            break;
                                        case 6:
                                            budget.Total = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Jun"].ToString()) * 1000;
                                            break;
                                        case 7:
                                            budget.Total = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Jul"].ToString()) * 1000;
                                            break;
                                        case 8:
                                            budget.Total = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Aug"].ToString()) * 1000;
                                            break;
                                        case 9:
                                            budget.Total = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Sep"].ToString()) * 1000;
                                            break;
                                        case 10:
                                            budget.Total = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Oct"].ToString()) * 1000;
                                            break;
                                        case 11:
                                            budget.Total = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Nov"].ToString()) * 1000;
                                            break;
                                        case 12:
                                            budget.Total = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["Dec"].ToString()) * 1000;
                                            break;
                                    }
                                    #endregion

                                    Response.Output.Write("Inserting Budget detail for Customer Name: '" + customer.AccountName + "' for month " + j + "...<br>");
                                    Response.Flush();
                                    if (budget.Total != 0)
                                    {
                                        try
                                        {
                                            budget.CreatedBy = sess.UserId;
                                            budget.CreatedDate = DateTime.Now;
                                            ResultType create = new BudgetActivity().CreateBudgetForCustomer(ref budget, sess);
                                            if (create == ResultType.Ok)
                                            {
                                                Response.Output.Write("Inserting successful.<br>");
                                                Response.Flush();

                                                File.Delete(excelFilePath);
                                            }
                                            else
                                            {
                                                Response.Output.Write("<SPAN STYLE='color: red'>Processing error of type : " + result.ToString() + ".</SPAN><br>");
                                                Response.Flush();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error:" + ex.Message + ".</SPAN><br>");
                                            Response.Flush();
                                        }
                                    }
                                    else
                                    {
                                        Response.Output.Write("<SPAN STYLE='color: red'>Data not inserted because value is 0.</SPAN><br>");
                                        Response.Flush();
                                    }

                                }
                                #endregion
                            }
                            Response.Output.Write("<br>");
                                   
                        }
                        else
                        {
                            Response.Output.Write("<SPAN STYLE='color: red'>Customer Name '" + dsExcel.Tables[0].Rows[i]["CustomerName"].ToString() + "' cannot be found at row " + (i + 1) + ".</SPAN><br><br>");
                            Response.Flush();
                        }
                        #endregion for each row
                    }//end for
                    #endregion upload budget for customer 
                }
                else
                {
                    #region upload budget for finance 
                    parseFinanceExcelFile();
                    
                    #endregion
                }

                Response.Output.Write("<SPAN STYLE='color: red'>End of insertion.</SPAN><br><br>");
                Response.Flush();
                File.Delete(excelFilePath);
            }
            catch (Exception ex)
            {
                Response.Output.Write(("<SPAN STYLE='color: red'><B>Error:" + ex.Message.ToString() + ".</B></SPAN><br>"));
            }
            
        }
    }
}