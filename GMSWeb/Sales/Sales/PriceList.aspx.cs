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
using GMSCore.Entity;
using GMSCore.Activity;

using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using GMSWeb.CustomCtrl;
using System.IO;
using System.Collections.Generic;

namespace GMSWeb.Sales.Sales
{
    public partial class PriceList : GMSBasePage
    {
        private HSSFWorkbook hssfworkbook;
        protected short loginUserOrAlternateParty = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentLink = "Products";
            short moduleID = 0;

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();

            }

            Master.setCurrentLink(currentLink);
 
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }

            if (currentLink == "Products") moduleID = 62;
            if (currentLink == "Sales") moduleID = 60;

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Product Search", ref lstAlterParty);
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
                                                                            moduleID);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            moduleID);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string fileName = "PriceList_"+DateTime.Now.Date.ToShortDateString()+".xls";
            hssfworkbook = new HSSFWorkbook();
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");
            IRow row;

            row = sheet1.CreateRow(0);
            row.CreateCell(0).SetCellValue("SN");
            row.CreateCell(1).SetCellValue("Brand Name");
            row.CreateCell(2).SetCellValue("Brand Code");
            row.CreateCell(3).SetCellValue("ProductCode");
            row.CreateCell(4).SetCellValue("ProductName");
            row.CreateCell(5).SetCellValue("UOM");
            row.CreateCell(6).SetCellValue("WeightedCost");
            row.CreateCell(7).SetCellValue("Country");
            row.CreateCell(8).SetCellValue("DealerPrice");
            row.CreateCell(9).SetCellValue("DPercent");
            row.CreateCell(10).SetCellValue("UserPrice");
            row.CreateCell(11).SetCellValue("UPercent");
            row.CreateCell(12).SetCellValue("RetailPrice");
            row.CreateCell(13).SetCellValue("RPercent");
            row.CreateCell(14).SetCellValue("Remarks");

            //Additional
            row.CreateCell(15).SetCellValue("Stock");
            row.CreateCell(16).SetCellValue("Last 4Q");
            row.CreateCell(17).SetCellValue("Last 3Q");
            row.CreateCell(18).SetCellValue("Last 2Q");
            row.CreateCell(19).SetCellValue("Last Q");
            row.CreateCell(20).SetCellValue("Last 6 months latest PO Prices");
            //row.CreateCell(21).SetCellValue("DPerNew");
            //row.CreateCell(22).SetCellValue("UPerNew");
            //row.CreateCell(23).SetCellValue("RPerNew");




            DataSet dsProductPrice = new DataSet();
            try
            {
                new GMSGeneralDALC().GetProductPriceByUser(session.CompanyId, loginUserOrAlternateParty, ref dsProductPrice);
                if ((dsProductPrice != null))
                {
                    int i = 1;
                    foreach (DataRow dr in dsProductPrice.Tables[0].Rows)
                    {
                        row = sheet1.CreateRow(i);
                        string si = (i + 1).ToString();

                        row.CreateCell(0).SetCellValue(i);
                        row.CreateCell(1).SetCellValue(dr["ProductGroupName"].ToString());
                        row.CreateCell(2).SetCellValue(dr["ProductGroupCode"].ToString());
                        row.CreateCell(3).SetCellValue(dr["ProductCode"].ToString());
                        row.CreateCell(4).SetCellValue(dr["ProductName"].ToString());
                        row.CreateCell(5).SetCellValue(dr["UOM"].ToString());
                        row.CreateCell(6).SetCellValue(dr["WeightedCost"].ToString() == "" ? 0 : Double.Parse(dr["WeightedCost"].ToString()));
                        row.CreateCell(7).SetCellValue(dr["Country"].ToString());
                        row.CreateCell(8).SetCellValue(dr["DealerPrice"].ToString() == "" ? 0 : Double.Parse(dr["DealerPrice"].ToString()));
                        //row.CreateCell(9).SetCellValue(dr["DPercent"].ToString() == "" ? 0 : Double.Parse(dr["DPercent"].ToString()));
                        row.CreateCell(9).SetCellValue(dr["DPernew"].ToString() == "" ? 0 : Double.Parse(dr["DPerNew"].ToString()));
                        row.CreateCell(10).SetCellValue(dr["UserPrice"].ToString() == "" ? 0 : Double.Parse(dr["UserPrice"].ToString()));
                        //row.CreateCell(11).SetCellValue(dr["UPercent"].ToString() == "" ? 0 : Double.Parse(dr["UPercent"].ToString()));
                        row.CreateCell(11).SetCellValue(dr["UPerNew"].ToString() == "" ? 0 : Double.Parse(dr["UPerNew"].ToString()));
                        row.CreateCell(12).SetCellValue(dr["RetailPrice"].ToString() == "" ? 0 : Double.Parse(dr["RetailPrice"].ToString()));
                        //row.CreateCell(13).SetCellValue(dr["RPercent"].ToString() == "" ? 0 : Double.Parse(dr["RPercent"].ToString()));
                        row.CreateCell(13).SetCellValue(dr["RPerNew"].ToString() == "" ? 0 : Double.Parse(dr["RPerNew"].ToString()));
                        row.CreateCell(14).SetCellValue(dr["Remarks"].ToString());                  

                        //Additional
                        row.CreateCell(15).SetCellValue(dr["BalanceQuantity"].ToString());
                        row.CreateCell(16).SetCellValue(dr["L4Q"].ToString());
                        row.CreateCell(17).SetCellValue(dr["L3Q"].ToString());
                        row.CreateCell(18).SetCellValue(dr["L2Q"].ToString());
                        row.CreateCell(19).SetCellValue(dr["LQ"].ToString());
                        row.CreateCell(20).SetCellValue(dr["Currency"].ToString()+ dr["UnitAmount"].ToString());
                        //row.CreateCell(21).SetCellValue(dr["DPernew"].ToString() == "" ? 0 : Double.Parse(dr["DPerNew"].ToString()));
                        //row.CreateCell(22).SetCellValue(dr["UPerNew"].ToString() == "" ? 0 : Double.Parse(dr["UPerNew"].ToString()));
                        //row.CreateCell(23).SetCellValue(dr["RPerNew"].ToString() == "" ? 0 : Double.Parse(dr["RPerNew"].ToString()));

                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/vnd.ms-excel";
            GetExcelStream().WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();

        }
        MemoryStream GetExcelStream()
        {
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            return file;
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                FileUpload1.SaveAs(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + "\\" + FileUpload1.FileName);

                this.IFrame1.Attributes["style"] = "";
                this.IFrame1.Attributes["src"] = String.Format("ParsingProductPrice.aspx?FILENAME={0}",
                                                            Server.UrlEncode(FileUpload1.FileName));
            }
            else
            {
                lblMsg.Text = "You have not specified a file.";
            }
        }

        #region GenerateReport
        protected void GenerateReport(object sender, EventArgs e)
        {
            string selectedReport = ddlReport.SelectedValue;
            ClientScript.RegisterStartupScript(typeof(string), "Report1",
                string.Format("jsOpenOperationalReport('Finance/BankFacilities/ReportViewer.aspx?REPORT={0}&TRNNO=0&REPORTID=-2');",
                                    selectedReport)
                                    , true);
        }
        #endregion
    }
}
