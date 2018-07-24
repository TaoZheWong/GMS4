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
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Products");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Products"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            107);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            107);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Products"));

            if (!Page.IsPostBack)
            {
            }
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

            DataSet dsProductPrice = new DataSet();
            try
            {
                new GMSGeneralDALC().GetProductPriceByUser(session.CompanyId, session.UserId, ref dsProductPrice);
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
                        row.CreateCell(9).SetCellFormula("IFERROR(((I"+ si + " - G" + si + ") / I" + si +  "),0)");
                        row.CreateCell(10).SetCellValue(dr["UserPrice"].ToString() == "" ? 0 : Double.Parse(dr["UserPrice"].ToString()));
                        row.CreateCell(11).SetCellFormula("IFERROR(((K" + si + " - G" + si + ")/ K" + si + "),0)");
                        row.CreateCell(12).SetCellValue(dr["RetailPrice"].ToString() == "" ? 0 : Double.Parse(dr["RetailPrice"].ToString()));
                        row.CreateCell(13).SetCellFormula("IFERROR(((M" + si + " - G" + si + ") / M" + si + "),0)");
                        row.CreateCell(14).SetCellValue(dr["Remarks"].ToString());
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
