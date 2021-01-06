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
    public partial class ParsingFinanceData : GMSBasePage
    {
        private string excelFilePath = "", excelFileName = "";
        private short year = 0;
        private short month = 0;
        private short itemPurposeId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.excelFileName = Request.Params["FILENAME"];
            year = GMSUtil.ToShort(Request.Params["YEAR"]);
            month = GMSUtil.ToShort(Request.Params["MONTH"]);
            itemPurposeId = GMSUtil.ToShort(Request.Params["PURPOSEID"]);

            excelFilePath = AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + Path.DirectorySeparatorChar + excelFileName;

            Response.ContentType = "text/html";
            ParseExcelFile();
        }

        protected void ParseExcelFile()
        {
            DataSet dsExcel = new DataSet();

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
                    IList<FinanceItem> lstItem = new SystemDataActivity().RetrieveFinanceItemByName(dsExcel.Tables[0].Rows[i]["Item"].ToString());
                    if (lstItem != null)
                    {
                        if (lstItem.Count > 0)
                        {
                            FinanceItem item = (FinanceItem)lstItem[0];

                            LogSession sess = base.GetSessionInfo();

                            // delete from tbFinanceData records according to year and month pecified by user, then insert new record
                            ResultType result = new FinanceDataActivity().DeleteFinanceDataByYearMonthItemID(this.year, this.month, item.ItemID, sess.CompanyId);
                            if (result == ResultType.Ok)
                            {
                                #region insert for each row
                                FinanceData financeData = new FinanceData(); 
                                financeData.CoyID = sess.CompanyId;
                                financeData.ProjectID = -1;
                                financeData.DepartmentID = -1;
                                financeData.SectionID = -1;
                                financeData.TbYear = this.year;
                                financeData.TbMonth = this.month;
                                financeData.ItemID = item.ItemID;
                                financeData.MTD = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["MTD"].ToString());
                                financeData.YTD = GMSUtil.ToDouble(dsExcel.Tables[0].Rows[i]["YTD"].ToString());
                                
                                Response.Output.Write("Inserting finance data detail for Item Name: '" + item.ItemName + "...<br>");
                                Response.Flush();

                                if (financeData.MTD != 0 || financeData.YTD != 0)
                                {
                                    try
                                    {
                                        if (item.ItemName == "Gross Profits Margin %" ||
                                                    item.ItemName == "- External Sales GP%" ||
                                                    item.ItemName == "- Interco Sales GP%" ||
                                                    item.ItemName == "Selling & Dist Margin %" ||
                                                    item.ItemName == "Admin & General Margin %" ||
                                                    item.ItemName == "Operating Profit Margin %" ||
                                                    item.ItemName == "Profits Before Taxation %" ||
                                                    item.ItemName == "Return on Equity %" ||
                                                    item.ItemName == "P&L PU Product Support Margin %" ||
                                                    item.ItemName == "P&L PU Operating Profit Margin %" ||
                                                    item.ItemName == "P&L PU Profits Before Taxation %" ||
                                                    item.ItemName == "P&L PU Stocks Turnover Days" ||
                                                    item.ItemName == "P&L PU % of Stocks Exceeding 1 Year" ||
                                                    item.ItemName == "P&L WS Selling & Dist Margin %" ||
                                                    item.ItemName == "P&L WS Operating Profit Margin %" ||
                                                    item.ItemName == "P&L WS Profits Before Taxation %" ||
                                                    item.ItemName == "P&L WS Debtors Turnover Days" ||
                                                    item.ItemName == "P&L WS % of Debtors Exceeding 1 Year")
                                        {
                                            financeData.MTD = financeData.MTD * 100; 
                                            financeData.YTD = financeData.YTD * 100; 
                                        }
                                        financeData.CreatedDate = DateTime.Now;
                                        ResultType create = new FinanceDataActivity().CreateFinanceData(ref financeData, sess); 
                                        
                                        if (create == ResultType.Ok)
                                        {
                                            Response.Output.Write("Inserting successful.<br>");
                                            Response.Flush();

                                            File.Delete(excelFilePath);
                                        }
                                        else
                                        {
                                            Response.Output.Write("<SPAN STYLE='color: red'>Processing error of type : " + create.ToString() + ".</SPAN><br>");
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
                                Response.Output.Write("<br>");
                                #endregion
                            }
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
                sheetDataLoader_Finance.Dispose();
            }
            catch (Exception ex)
            {
                Response.Output.Write(("<SPAN STYLE='color: red'><B>Error:" + ex.Message.ToString() + ".</B></SPAN><br>"));
            }
            finally
            {
                File.Delete(excelFilePath);
            }
        }
    }
}
