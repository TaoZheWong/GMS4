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
using System.IO;
using GMSCore.Activity;

namespace GMSWeb.Sales.Sales
{
    public partial class ParsingCustomerlist : GMSBasePage
    {
        private string excelFilePath = "", excelFileName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.excelFileName = Request.Params["FILENAME"];

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

                DBManager db = DBManager.GetInstance();
                Wilson.ORMapper.QueryHelper helper = db.Engine.QueryHelper;
                System.Text.StringBuilder stb = new System.Text.StringBuilder(200);
                stb.AppendFormat(" {0} is NULL ", helper.GetFieldName("Company.TableSuffix"));
                stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Company.CoyID"),
                                        helper.CleanValue(sess.CompanyId));

                GMSCore.Entity.Company coy = GMSCore.Entity.Company.RetrieveFirst(stb.ToString());
                if (coy != null)
                {

                    ResultType result = new AccountInformationActivity().DeleteAccountForNonA21(sess.CompanyId);
                    if (result != ResultType.Ok)
                    {
                        Response.Output.Write("<SPAN STYLE='color: red'>Cannot delete old data : " + result.ToString() + ".</SPAN><br>");
                        Response.Flush();
                       
                    }



                    for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
                    {
                            GMSCore.Entity.A21Account acc = GMSCore.Entity.A21Account.RetrieveByKey(sess.CompanyId, dsExcel.Tables[0].Rows[i]["AccountCode"].ToString());
                            if (acc != null)
                            {
                                acc.CoyID = GMSUtil.ToShort(sess.CompanyId);
                                acc.AccountCode = dsExcel.Tables[0].Rows[i]["AccountCode"].ToString();
                                acc.SalesPersonID = " ";
                                acc.AccountType = "C";
                                acc.AccountName = dsExcel.Tables[0].Rows[i]["AccountName"].ToString();                              
                                acc.DefaultCurrency = coy.DefaultCurrencyCode;
                                acc.CreditLimit = 0;
                                acc.CreditTerm = 0;

                                Response.Output.Write("Inserting Customer : " + dsExcel.Tables[0].Rows[i]["AccountCode"].ToString() + "-" + dsExcel.Tables[0].Rows[i]["AccountName"].ToString() + "...<br>");
                                Response.Flush();

                                try
                                {
                                    acc.Save();
                                    acc.Resync();
                                    Response.Output.Write("Inserting successful.<br>");
                                    Response.Flush();
                                }
                                catch (Exception ex)
                                {
                                    Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error:" + ex.Message + ".</SPAN><br>");
                                    Response.Flush();
                                }

                                Response.Output.Write("<br>");
                            }
                            else
                            {
                                acc = new GMSCore.Entity.A21Account();
                                acc.CoyID = GMSUtil.ToShort(sess.CompanyId);
                                acc.AccountCode = dsExcel.Tables[0].Rows[i]["AccountCode"].ToString();
                                acc.SalesPersonID = " ";
                                acc.AccountType = "C";
                                acc.AccountName = dsExcel.Tables[0].Rows[i]["AccountName"].ToString();                               
                                acc.DefaultCurrency = coy.DefaultCurrencyCode;
                                acc.CreditLimit = 0;
                                acc.CreditTerm = 0;

                                Response.Output.Write("Inserting Customer : " + dsExcel.Tables[0].Rows[i]["AccountCode"].ToString() + "-" + dsExcel.Tables[0].Rows[i]["AccountName"].ToString() + "...<br>");
                                Response.Flush();

                                try
                                {
                                    acc.Save();
                                    acc.Resync();
                                    Response.Output.Write("Inserting successful.<br>");
                                    Response.Flush();
                                }
                                catch (Exception ex)
                                {
                                    Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error:" + ex.Message + ".</SPAN><br>");
                                    Response.Flush();
                                }

                                Response.Output.Write("<br>");
                            }
                        
                    }
                    Response.Output.Write("<SPAN STYLE='color: red'>End of insertion.</SPAN><br><br>");
                    Response.Flush();
                    sheetDataLoader_Finance.Dispose();
                }
                else
                {
                    Response.Output.Write("<SPAN STYLE='color: red'>Only Company that not using A21 allow to upload Customer List.</SPAN><br><br>");
                    Response.Flush();
                }
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
