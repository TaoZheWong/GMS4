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

namespace GMSWeb.Sales.Sales
{
    public partial class ParsingPricelist : GMSBasePage
    {
        private string excelFilePath = "", excelFileName = "";
        protected short loginUserOrAlternateParty = 0;

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

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(sess.CompanyId, sess.UserId, "Product Search", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {

                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = sess.UserId;

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
                    DBManager db = DBManager.GetInstance();
                    Wilson.ORMapper.QueryHelper helper = db.Engine.QueryHelper;
                    System.Text.StringBuilder stb = new System.Text.StringBuilder(200);
                    stb.AppendFormat(" {0} = {1} ", helper.GetFieldName("Product.ProductCode"),
                                        helper.CleanValue(dsExcel.Tables[0].Rows[i]["ProductCode"].ToString()));
                    stb.AppendFormat(" AND {0} = {1} ", helper.GetFieldName("Product.CoyID"),
                                        helper.CleanValue(sess.CompanyId));
                    GMSCore.Entity.Product pd = GMSCore.Entity.Product.RetrieveFirst(stb.ToString());
                    if (pd != null)
                    {
                        DataSet ds = new DataSet();
                        (new GMSGeneralDALC()).IsProductManager(sess.CompanyId, dsExcel.Tables[0].Rows[i]["ProductCode"].ToString(), loginUserOrAlternateParty, ref ds);
                        if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                        {
                            Response.Output.Write("<SPAN STYLE='color: red'>You do not have access to upload price for Product '" + dsExcel.Tables[0].Rows[i]["ProductCode"].ToString() + "'.</SPAN><br><br>");
                            Response.Flush();
                        }
                        else
                        {
                            GMSCore.Entity.PriceList pl = GMSCore.Entity.PriceList.RetrieveByKey(sess.CompanyId, dsExcel.Tables[0].Rows[i]["ProductCode"].ToString());
                            if (pl != null)
                            {
                                pl.ListPrice = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i]["ListPrice"].ToString());
                                pl.MinSellingPrice = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i]["UserMSP"].ToString());
                                pl.IntercoPrice = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i]["IntercoPrice"].ToString());
                                pl.DealerMSP = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i]["DealerMSP"].ToString());

                                Response.Output.Write("Inserting price for Product: " + dsExcel.Tables[0].Rows[i]["ProductCode"].ToString() + "-" + dsExcel.Tables[0].Rows[i]["ProductName"].ToString() + "...<br>");
                                Response.Flush();

                                try
                                {
                                    pl.CreatedBy = sess.UserId;
                                    pl.CreatedDate = DateTime.Now;
                                    pl.Save();
                                    pl.Resync();
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
                                pl = new GMSCore.Entity.PriceList();
                                pl.CoyID = sess.CompanyId;
                                pl.ProductCode = dsExcel.Tables[0].Rows[i]["ProductCode"].ToString();
                                pl.ListPrice = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i]["ListPrice"].ToString());
                                pl.MinSellingPrice = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i]["UserMSP"].ToString());
                                pl.IntercoPrice = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i]["IntercoPrice"].ToString());
                                pl.DealerMSP = GMSUtil.ToDecimal(dsExcel.Tables[0].Rows[i]["DealerMSP"].ToString());

                                Response.Output.Write("Inserting price for Product: " + dsExcel.Tables[0].Rows[i]["ProductCode"].ToString() + "-" + dsExcel.Tables[0].Rows[i]["ProductName"].ToString() + "...<br>");
                                Response.Flush();

                                try
                                {
                                    pl.CreatedBy = sess.UserId;
                                    pl.CreatedDate = DateTime.Now;
                                    pl.Save();
                                    pl.Resync();
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
                    }
                    else
                    {
                        Response.Output.Write("<SPAN STYLE='color: red'>Product '" + dsExcel.Tables[0].Rows[i]["ProductCode"].ToString() + "' cannot be found at row " + (i + 1) + ".</SPAN><br><br>");
                        Response.Flush();
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
