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

namespace GMSWeb.Finance.Upload
{
    public partial class ParsingProductBudget : GMSBasePage
    {
        private string excelFilePath = "", excelFileName = "";
        private short year = 0;
        private string division = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.excelFileName = Request.Params["FILENAME"];
            year = GMSUtil.ToShort(Request.Params["YEAR"]);
            division = Request.Params["DIVISIONID"];

            excelFilePath = AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + Path.DirectorySeparatorChar + excelFileName;

            Response.ContentType = "text/html";
            ParseExcelFile();
        }

        protected void ParseExcelFile()
        {
            DataSet dsExcel = new DataSet();
            LogSession sess = base.GetSessionInfo();

            try
            {
                Response.Output.Write("Parsing excel file...<br>");
                Response.Flush();

                Asiasoft.MSExcelFileReader.SheetDataLoader sheetDataLoader_Finance = new Asiasoft.MSExcelFileReader.SheetDataLoader();
                sheetDataLoader_Finance.ExcelFilePath = this.excelFilePath;
                sheetDataLoader_Finance.IsHeaderIncludedInExcelFile = true;
                sheetDataLoader_Finance.SheetName = "Sheet1";
                sheetDataLoader_Finance.LoadExcelData();

                dsExcel = sheetDataLoader_Finance.ExcelData;

                for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
                {
                    IList<ProductGroup> lstProductGroup = new SystemDataActivity().RetrieveProductGroupByCoyIDName(sess.CompanyId, dsExcel.Tables[0].Rows[i]["Item"].ToString());
                    if (lstProductGroup != null)
                    {
                        if (lstProductGroup.Count > 0)
                        {
                            ResultType result = new ProductBudgetActivity().DeleteBudgetByCoyIDYearDivisionIDProductGroupCode(sess.CompanyId, this.year, this.division, lstProductGroup[0].ProductGroupCode);

                            if (result == ResultType.Ok)
                            {
                                #region insert for each month
                                for (short j = 1; j <= 12; j++)
                                {
                                    ProductBudget budget = new ProductBudget();
                                    budget.CoyID = sess.CompanyId;
                                    budget.BudgetYear = this.year;
                                    budget.BudgetMonth = j;
                                    budget.Division = division;
                                    budget.ProductGroupCode = lstProductGroup[0].ProductGroupCode;

                                    #region switch j
                                    switch (j)
                                    {
                                        case 1:
                                            budget.Sales = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["JAN SALES"].ToString());
                                            budget.Gp = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["JAN GP"].ToString());
                                            break;
                                        case 2:
                                            budget.Sales = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["FEB SALES"].ToString());
                                            budget.Gp = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["FEB GP"].ToString());
                                            break;
                                        case 3:
                                            budget.Sales = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["MAR SALES"].ToString());
                                            budget.Gp = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["MAR GP"].ToString());
                                            break;
                                        case 4:
                                            budget.Sales = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["APR SALES"].ToString());
                                            budget.Gp = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["APR GP"].ToString());
                                            break;
                                        case 5:
                                            budget.Sales = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["MAY SALES"].ToString());
                                            budget.Gp = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["MAY GP"].ToString());
                                            break;
                                        case 6:
                                            budget.Sales = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["JUN SALES"].ToString());
                                            budget.Gp = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["JUN GP"].ToString());
                                            break;
                                        case 7:
                                            budget.Sales = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["JUL SALES"].ToString());
                                            budget.Gp = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["JUL GP"].ToString());
                                            break;
                                        case 8:
                                            budget.Sales = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["AUG SALES"].ToString());
                                            budget.Gp = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["AUG GP"].ToString());
                                            break;
                                        case 9:
                                            budget.Sales = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["SEP SALES"].ToString());
                                            budget.Gp = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["SEP GP"].ToString());
                                            break;
                                        case 10:
                                            budget.Sales = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["OCT SALES"].ToString());
                                            budget.Gp = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["OCT GP"].ToString());
                                            break;
                                        case 11:
                                            budget.Sales = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["NOV SALES"].ToString());
                                            budget.Gp = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["NOV GP"].ToString());
                                            break;
                                        case 12:
                                            budget.Sales = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["DEC SALES"].ToString());
                                            budget.Gp = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["DEC GP"].ToString());
                                            break;
                                    }
                                    #endregion

                                    Response.Output.Write("Inserting Budget detail for Item Name: '" + lstProductGroup[0].ProductGroupName + "' for month " + j + "...<br>");
                                    Response.Flush();
                                    if (budget.Sales != 0 || budget.Gp != 0)
                                    {
                                        try
                                        {
                                            budget.CreatedBy = sess.UserId;
                                            budget.CreatedDate = DateTime.Now;
                                            ResultType create = new ProductBudgetActivity().CreateProductBudget(ref budget, sess);
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
                            Response.Output.Write("<SPAN STYLE='color: red'>Item Name '" + dsExcel.Tables[0].Rows[i]["Item"].ToString() + "' cannot be found at row " + (i + 1) + ".</SPAN><br><br>");
                            Response.Flush();
                        }
                    }
                }
                Response.Output.Write("<SPAN STYLE='color: red'>End of insertion.</SPAN><br><br>");
                Response.Flush();
            }
            catch (Exception ex)
            {
                Response.Output.Write(("<SPAN STYLE='color: red'><B>Error:" + ex.Message.ToString() + ".</B></SPAN><br>"));
            }
        }
    }
}
