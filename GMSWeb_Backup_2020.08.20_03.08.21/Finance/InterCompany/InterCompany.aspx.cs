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

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;

namespace GMSWeb.Finance.InterCompany
{
    public partial class InterCompany : GMSBasePage
    {
        private int coyId;
        private string companyCode;
        private HSSFWorkbook hssfworkbook;
        private bool isnewCOA;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("CompanyFinance"); 
            LogSession session = base.GetSessionInfo();
            Company coy = new SystemDataActivity().RetrieveCompanyById(session.CompanyId, session);
            
           

            companyCode = coy.Code; 

            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }
            coyId = session.CompanyId; 

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            68);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyFinance"));

            if (!Page.IsPostBack)
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

                DateTime lastMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1).AddDays(-1);
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
        }


        #region btnExport_Click
        protected void btnExport_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            GMSGeneralDALC DALC = new GMSGeneralDALC();
            DateTime coadate = DALC.GetNewCOADate(session.CompanyId);
            if (((coadate.Year * 100) + coadate.Month) <= ((Convert.ToInt16(ddlYear.SelectedValue) * 100) + Convert.ToInt16(ddlMonth.SelectedValue)))
            {
                newCOA();
            }
            else
            {
                oldCOA();
            }
        }

        protected void oldCOA()
        {
            

            string fileName = "InterCompany_" + companyCode + "_" + ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString().PadLeft(2, '0') + ".xls"; 
                       
            IDbConnection conn = new ConnectionManager().GetConnection(); 
            SqlDataReader rdr = null;
            StringBuilder sb = new StringBuilder();

            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procFinanceInterCompanyTransactionExport", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyId; 
                command.Parameters.Add("@Year", SqlDbType.SmallInt).Value = ddlYear.SelectedValue;
                command.Parameters.Add("@Month", SqlDbType.SmallInt).Value = ddlMonth.SelectedValue; 
                rdr = command.ExecuteReader();

                hssfworkbook = new HSSFWorkbook();
                ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");
                IRow row;

                row = sheet1.CreateRow(0);
                row.CreateCell(0).SetCellValue("SN");
                row.CreateCell(1).SetCellValue("Company");
                row.CreateCell(2).SetCellValue("BSAssetCurrentTrade");
                row.CreateCell(3).SetCellValue("BSAssetCurrentLoan");
                row.CreateCell(4).SetCellValue("BSAssetCurrentNonTrade");
                row.CreateCell(5).SetCellValue("BSAssetNonCurrentNonTrade");
                row.CreateCell(6).SetCellValue("BSAssetNonCurrentLoan");
                row.CreateCell(7).SetCellValue("BSLiabilitiesAccruedPurchase");
                row.CreateCell(8).SetCellValue("BSLiabilitiesCurrentTrade");
                row.CreateCell(9).SetCellValue("BSLiabilitiesCurrentNonTrade");
                row.CreateCell(10).SetCellValue("BSLiabilitiesCurrentLoan");
                row.CreateCell(11).SetCellValue("BSLiabilitiesNonCurrentNonTrade");
                row.CreateCell(12).SetCellValue("BSLiabilitiesNonCurrentLoan");
                row.CreateCell(13).SetCellValue("BSEquityQuasi");
                row.CreateCell(14).SetCellValue("PNLSalesTrade");
                row.CreateCell(15).SetCellValue("PNLSalesManu");
                row.CreateCell(16).SetCellValue("PNLSalesRental");
                row.CreateCell(17).SetCellValue("PNLSalesOthers");
                row.CreateCell(18).SetCellValue("PNLInterestIncome");
                row.CreateCell(19).SetCellValue("PNLInterestExpense");
                row.CreateCell(20).SetCellValue("PNLCommissionIncome");
                row.CreateCell(21).SetCellValue("PNLPropertyRentalIncome");
                row.CreateCell(22).SetCellValue("PNLAdminMgtFee");
                //row.CreateCell(23).SetCellValue("PNLOtherIncome");
                //row.CreateCell(24).SetCellValue("PNLOtherExpense");
                row.CreateCell(23).SetCellValue("PNLSNDRentalExpense");
                row.CreateCell(24).SetCellValue("PNLANGRentalExpense");
                row.CreateCell(25).SetCellValue("PNLSNDRecovery");
                row.CreateCell(26).SetCellValue("PNLANGRecovery");
                row.CreateCell(27).SetCellValue("PNLDividendIncome");
                row.CreateCell(28).SetCellValue("PNLDividendExpense");

                //A21 formulaes
                row = sheet1.CreateRow(1);
                row.CreateCell(0).SetCellValue("0");
                row.CreateCell(1).SetCellValue("GL Code");
                row.CreateCell(2).SetCellValue("1400");
                row.CreateCell(3).SetCellValue("1408");
                row.CreateCell(4).SetCellValue("1410");
                row.CreateCell(5).SetCellValue("2650");
                row.CreateCell(6).SetCellValue("2658");
                row.CreateCell(7).SetCellValue("3610");
                row.CreateCell(8).SetCellValue("3700");
                row.CreateCell(9).SetCellValue("3701");
                row.CreateCell(10).SetCellValue("3702");
                row.CreateCell(11).SetCellValue("4300");
                row.CreateCell(12).SetCellValue("4308");
                row.CreateCell(13).SetCellValue("4602");
                row.CreateCell(14).SetCellValue("5300");
                row.CreateCell(15).SetCellValue("5310");
                row.CreateCell(16).SetCellValue("5320,5340,5360");
                row.CreateCell(17).SetCellValue("5330,5350");
                row.CreateCell(18).SetCellValue("5502");
                row.CreateCell(19).SetCellValue("9050");
                row.CreateCell(20).SetCellValue("5400");
                row.CreateCell(21).SetCellValue("5506");
                row.CreateCell(22).SetCellValue("5404");
                //row.CreateCell(23).SetCellValue("5499");
                //row.CreateCell(24).SetCellValue("5498");
                row.CreateCell(23).SetCellValue("7721,7750,7751");
                row.CreateCell(24).SetCellValue("8621,8650,8651");
                row.CreateCell(25).SetCellValue("7990,7999");
                row.CreateCell(26).SetCellValue("8990,8999");
                row.CreateCell(27).SetCellValue("5500");
                row.CreateCell(28).SetCellValue("9300");

                int i = 1; 
                while (rdr.Read())
                {
                    row = sheet1.CreateRow(i+1);
                    row.CreateCell(0).SetCellValue(i);
                    
                    if (i == 64) // prev is 61 
                    {
                        row.CreateCell(1).SetCellValue("Total InterCompany");
                        row.CreateCell(2).SetCellFormula("SUM(C3:C65)"); //prev is 62, need to change all below 
                        row.CreateCell(3).SetCellFormula("SUM(D3:D65)");
                        row.CreateCell(4).SetCellFormula("SUM(E3:E65)");
                        row.CreateCell(5).SetCellFormula("SUM(F3:F65)");
                        row.CreateCell(6).SetCellFormula("SUM(G3:G65)");
                        row.CreateCell(7).SetCellFormula("SUM(H3:H65)");
                        row.CreateCell(8).SetCellFormula("SUM(I3:I65)");
                        row.CreateCell(9).SetCellFormula("SUM(J3:J65)");
                        row.CreateCell(10).SetCellFormula("SUM(K3:K65)");
                        row.CreateCell(11).SetCellFormula("SUM(L3:L65)");
                        row.CreateCell(12).SetCellFormula("SUM(M3:M65)");
                        row.CreateCell(13).SetCellFormula("SUM(N3:N65)");
                        row.CreateCell(14).SetCellFormula("SUM(O3:O65)");
                        row.CreateCell(15).SetCellFormula("SUM(P3:P65)");
                        row.CreateCell(16).SetCellFormula("SUM(Q3:Q65)");
                        row.CreateCell(17).SetCellFormula("SUM(R3:R65)");
                        row.CreateCell(18).SetCellFormula("SUM(S3:S65)");
                        row.CreateCell(19).SetCellFormula("SUM(T3:T65)");
                        row.CreateCell(20).SetCellFormula("SUM(U3:U65)");
                        row.CreateCell(21).SetCellFormula("SUM(V3:V65)");
                        row.CreateCell(22).SetCellFormula("SUM(W3:W65)");
                        row.CreateCell(23).SetCellFormula("SUM(X3:X65)");
                        row.CreateCell(24).SetCellFormula("SUM(Y3:Y65)");
                        row.CreateCell(25).SetCellFormula("SUM(Z3:Z65)");
                        row.CreateCell(26).SetCellFormula("SUM(AA3:AA65)");
                        row.CreateCell(27).SetCellFormula("SUM(AB3:AB65)");
                        row.CreateCell(28).SetCellFormula("SUM(AC3:AC65)");
                        //row.CreateCell(29).SetCellFormula("SUM(AD3:AD65)");
                        //row.CreateCell(30).SetCellFormula("SUM(AE3:AE65)");
                        i++; 
                        continue;
                    }

                    #region associate rows 
                    /*
                    if (i == 66)
                    {
                        row.CreateCell(1).SetCellValue("Total Related");
                        row.CreateCell(2).SetCellFormula("SUM(C60:C66)");
                        row.CreateCell(3).SetCellFormula("SUM(D60:D66)");
                        row.CreateCell(4).SetCellFormula("SUM(E60:E66)");
                        row.CreateCell(5).SetCellFormula("SUM(F60:F66)");
                        row.CreateCell(6).SetCellFormula("SUM(G60:G66)");
                        row.CreateCell(7).SetCellFormula("SUM(H60:H66)");
                        row.CreateCell(8).SetCellFormula("SUM(I60:I66)");
                        row.CreateCell(9).SetCellFormula("SUM(J60:J66)");
                        row.CreateCell(10).SetCellFormula("SUM(K60:K66)");
                        row.CreateCell(11).SetCellFormula("SUM(L60:L66)");
                        row.CreateCell(12).SetCellFormula("SUM(M60:M66)");
                        row.CreateCell(13).SetCellFormula("SUM(N60:N66)");
                        row.CreateCell(14).SetCellFormula("SUM(O60:O66)");
                        row.CreateCell(15).SetCellFormula("SUM(P60:P66)");
                        row.CreateCell(16).SetCellFormula("SUM(Q60:Q66)");
                        row.CreateCell(17).SetCellFormula("SUM(R60:R66)");
                        row.CreateCell(18).SetCellFormula("SUM(S60:S66)");
                        row.CreateCell(19).SetCellFormula("SUM(T60:T66)");
                        row.CreateCell(20).SetCellFormula("SUM(U60:U66)");
                        i++;
                        continue;
                    }
                    */
                    #endregion

                    row.CreateCell(1).SetCellValue(rdr["ItemName"].ToString());
                    row.CreateCell(2).SetCellValue(rdr["BSAssetCurrentTrade"].ToString() == "" ? 0 : Double.Parse(rdr["BSAssetCurrentTrade"].ToString()));
                    row.CreateCell(3).SetCellValue(rdr["BSAssetCurrentLoan"].ToString() == "" ? 0 : Double.Parse(rdr["BSAssetCurrentLoan"].ToString()));
                    row.CreateCell(4).SetCellValue(rdr["BSAssetCurrentNonTrade"].ToString() == "" ? 0 : Double.Parse(rdr["BSAssetCurrentNonTrade"].ToString()));
                    row.CreateCell(5).SetCellValue(rdr["BSAssetNonCurrentNonTrade"].ToString() == "" ? 0 : Double.Parse(rdr["BSAssetNonCurrentNonTrade"].ToString()));
                    row.CreateCell(6).SetCellValue(rdr["BSAssetNonCurrentLoan"].ToString() == "" ? 0 : Double.Parse(rdr["BSAssetNonCurrentLoan"].ToString()));
                    row.CreateCell(7).SetCellValue(rdr["BSLiabilitiesAccruedPurchase"].ToString() == "" ? 0 : Double.Parse(rdr["BSLiabilitiesAccruedPurchase"].ToString()));
                    row.CreateCell(8).SetCellValue(rdr["BSLiabilitiesCurrentTrade"].ToString() == "" ? 0 : Double.Parse(rdr["BSLiabilitiesCurrentTrade"].ToString()));
                    row.CreateCell(9).SetCellValue(rdr["BSLiabilitiesCurrentNonTrade"].ToString() == "" ? 0 : Double.Parse(rdr["BSLiabilitiesCurrentNonTrade"].ToString()));
                    row.CreateCell(10).SetCellValue(rdr["BSLiabilitiesCurrentLoan"].ToString() == "" ? 0 : Double.Parse(rdr["BSLiabilitiesCurrentLoan"].ToString()));
                    row.CreateCell(11).SetCellValue(rdr["BSLiabilitiesNonCurrentNonTrade"].ToString() == "" ? 0 : Double.Parse(rdr["BSLiabilitiesNonCurrentNonTrade"].ToString()));
                    row.CreateCell(12).SetCellValue(rdr["BSLiabilitiesNonCurrentLoan"].ToString() == "" ? 0 : Double.Parse(rdr["BSLiabilitiesNonCurrentLoan"].ToString()));
                    row.CreateCell(13).SetCellValue(rdr["BSEquityQuasi"].ToString() == "" ? 0 : Double.Parse(rdr["BSEquityQuasi"].ToString()));
                    row.CreateCell(14).SetCellValue(rdr["PNLSalesTrade"].ToString() == "" ? 0 : Double.Parse(rdr["PNLSalesTrade"].ToString()));
                    row.CreateCell(15).SetCellValue(rdr["PNLSalesManu"].ToString() == "" ? 0 : Double.Parse(rdr["PNLSalesManu"].ToString()));
                    row.CreateCell(16).SetCellValue(rdr["PNLSalesEquipRental"].ToString() == "" ? 0 : Double.Parse(rdr["PNLSalesEquipRental"].ToString()));
                    row.CreateCell(17).SetCellValue(rdr["PNLSalesOthers"].ToString() == "" ? 0 : Double.Parse(rdr["PNLSalesOthers"].ToString()));
                    row.CreateCell(18).SetCellValue(rdr["PNLInterestIncome"].ToString() == "" ? 0 : Double.Parse(rdr["PNLInterestIncome"].ToString()));
                    row.CreateCell(19).SetCellValue(rdr["PNLInterestExpense"].ToString() == "" ? 0 : Double.Parse(rdr["PNLInterestExpense"].ToString()));
                    row.CreateCell(20).SetCellValue(rdr["PNLCommissionIncome"].ToString() == "" ? 0 : Double.Parse(rdr["PNLCommissionIncome"].ToString()));
                    row.CreateCell(21).SetCellValue(rdr["PNLPropertyRentalIncome"].ToString() == "" ? 0 : Double.Parse(rdr["PNLPropertyRentalIncome"].ToString()));
                    row.CreateCell(22).SetCellValue(rdr["PNLAdminMgtFee"].ToString() == "" ? 0 : Double.Parse(rdr["PNLAdminMgtFee"].ToString()));
                    //row.CreateCell(23).SetCellValue(rdr["PNLOtherIncome"].ToString() == "" ? 0 : Double.Parse(rdr["PNLOtherIncome"].ToString()));
                    //row.CreateCell(24).SetCellValue(rdr["PNLOtherExpense"].ToString() == "" ? 0 : Double.Parse(rdr["PNLOtherExpense"].ToString()));
                    row.CreateCell(23).SetCellValue(rdr["PNLSNDRentalExpense"].ToString() == "" ? 0 : Double.Parse(rdr["PNLSNDRentalExpense"].ToString()));
                    row.CreateCell(24).SetCellValue(rdr["PNLANGRentalExpense"].ToString() == "" ? 0 : Double.Parse(rdr["PNLANGRentalExpense"].ToString()));
                    row.CreateCell(25).SetCellValue(rdr["PNLSNDRecovery"].ToString() == "" ? 0 : Double.Parse(rdr["PNLSNDRecovery"].ToString()));
                    row.CreateCell(26).SetCellValue(rdr["PNLANGRecovery"].ToString() == "" ? 0 : Double.Parse(rdr["PNLANGRecovery"].ToString()));
                    row.CreateCell(27).SetCellValue(rdr["PNLDividendIncome"].ToString() == "" ? 0 : Double.Parse(rdr["PNLDividendIncome"].ToString()));
                    row.CreateCell(28).SetCellValue(rdr["PNLDividendExpense"].ToString() == "" ? 0 : Double.Parse(rdr["PNLDividendExpense"].ToString()));

                    i++; 
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/vnd.ms-excel";
            GetExcelStream().WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();
        }

        protected void newCOA()
        {
            string fileName = "InterCompany_" + companyCode + "_" + ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString().PadLeft(2, '0') + "_COA2016.xls";

            IDbConnection conn = new ConnectionManager().GetConnection();
            SqlDataReader rdr = null;
            StringBuilder sb = new StringBuilder();

            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procFinanceInterCompanyTransactionExport_COA2016", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyId;
                command.Parameters.Add("@Year", SqlDbType.SmallInt).Value = ddlYear.SelectedValue;
                command.Parameters.Add("@Month", SqlDbType.SmallInt).Value = ddlMonth.SelectedValue;
                rdr = command.ExecuteReader();

                hssfworkbook = new HSSFWorkbook();
                ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");
                IRow row;

                row = sheet1.CreateRow(0);
                row.CreateCell(0).SetCellValue("SN");
                row.CreateCell(1).SetCellValue("Company");
                row.CreateCell(2).SetCellValue("BSAssetCurrentTrade");
                row.CreateCell(3).SetCellValue("BSAssetCurrentLoan");
                row.CreateCell(4).SetCellValue("BSAssetCurrentNonTrade");
                row.CreateCell(5).SetCellValue("BSAssetNonCurrent");
                row.CreateCell(6).SetCellValue("BSAssetNonCurrentExclude");
                row.CreateCell(7).SetCellValue("BSLiabilitiesAccruedPurchase");
                row.CreateCell(8).SetCellValue("BSLiabilitiesCurrentTrade");
                row.CreateCell(9).SetCellValue("BSLiabilitiesCurrentNonTrade");
                row.CreateCell(10).SetCellValue("BSLiabilitiesCurrentLoan");
                row.CreateCell(11).SetCellValue("BSLiabilitiesNonCurrent");
                row.CreateCell(12).SetCellValue("BSEquityQuasi");
                row.CreateCell(13).SetCellValue("PNLSalesGoods");
                row.CreateCell(14).SetCellValue("PNLSalesCylinderRental");
                row.CreateCell(15).SetCellValue("PNLSalesEquipRental");
                row.CreateCell(16).SetCellValue("PNLSalesPropertyRental");
                row.CreateCell(17).SetCellValue("PNLSalesOthers");
                row.CreateCell(18).SetCellValue("PNLInterestIncome");
                row.CreateCell(19).SetCellValue("PNLInterestExpense");
                row.CreateCell(20).SetCellValue("PNLCommissionIncome");
                row.CreateCell(21).SetCellValue("PNLPropertyRentalIncome");
                row.CreateCell(22).SetCellValue("PNLAdminMgmtFee");
                row.CreateCell(23).SetCellValue("PNLSNDRentalExpense");
                row.CreateCell(24).SetCellValue("PNLANGRentalExpense");
                row.CreateCell(25).SetCellValue("PNLSNDRecovery");
                row.CreateCell(26).SetCellValue("PNLANGRecovery");
                row.CreateCell(27).SetCellValue("PNLDividendIncome");
                row.CreateCell(28).SetCellValue("PNLDividendExpense");
                row.CreateCell(29).SetCellValue("DividendDeclared");
                //A21 formulaes
                row = sheet1.CreateRow(1);
                row.CreateCell(0).SetCellValue("0");
                row.CreateCell(1).SetCellValue("GL Code");
                row.CreateCell(2).SetCellValue("1401");
                row.CreateCell(3).SetCellValue("1403");
                row.CreateCell(4).SetCellValue("1402");
                row.CreateCell(5).SetCellValue("2721");
                row.CreateCell(6).SetCellValue("2721 Exclude 2721Z");
                row.CreateCell(7).SetCellValue("3612");
                row.CreateCell(8).SetCellValue("3701");
                row.CreateCell(9).SetCellValue("3702");
                row.CreateCell(10).SetCellValue("3703");
                row.CreateCell(11).SetCellValue("4301");
                row.CreateCell(12).SetCellValue("4602");
                row.CreateCell(13).SetCellValue("5031");
                row.CreateCell(14).SetCellValue("5032");
                row.CreateCell(15).SetCellValue("5033");
                row.CreateCell(16).SetCellValue("5041,5042");
                row.CreateCell(17).SetCellValue("5034,5035,5036,5037");
                row.CreateCell(18).SetCellValue("9023");
                row.CreateCell(19).SetCellValue("8806");
                row.CreateCell(20).SetCellValue("5501");
                row.CreateCell(21).SetCellValue("5502");
                row.CreateCell(22).SetCellValue("5503");
                row.CreateCell(23).SetCellValue("7501,7502");
                row.CreateCell(24).SetCellValue("8301,8302");
                row.CreateCell(25).SetCellValue("7901,7903,7910,7920,7998,7999");
                row.CreateCell(26).SetCellValue("8610,8651,8653,8798.8799");
                row.CreateCell(27).SetCellValue("9021");
                row.CreateCell(28).SetCellValue("9022");
                row.CreateCell(29).SetCellValue("4902");
                int i = 1;
                while (rdr.Read())
                {
                    row = sheet1.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(i);

                    if (i == 62) // prev is 61 
                    {
                        row.CreateCell(1).SetCellValue("Total InterCompany");
                        row.CreateCell(2).SetCellFormula("SUM(C3:C63)"); //prev is 62, need to change all below 
                        row.CreateCell(3).SetCellFormula("SUM(D3:D63)");
                        row.CreateCell(4).SetCellFormula("SUM(E3:E63)");
                        row.CreateCell(5).SetCellFormula("SUM(F3:F63)");
                        row.CreateCell(6).SetCellFormula("SUM(G3:G63)");
                        row.CreateCell(7).SetCellFormula("SUM(H3:H63)");
                        row.CreateCell(8).SetCellFormula("SUM(I3:I63)");
                        row.CreateCell(9).SetCellFormula("SUM(J3:J63)");
                        row.CreateCell(10).SetCellFormula("SUM(K3:K63)");
                        row.CreateCell(11).SetCellFormula("SUM(L3:L63)");
                        row.CreateCell(12).SetCellFormula("SUM(M3:M63)");
                        row.CreateCell(13).SetCellFormula("SUM(N3:N63)");
                        row.CreateCell(14).SetCellFormula("SUM(O3:O63)");
                        row.CreateCell(15).SetCellFormula("SUM(P3:P63)");
                        row.CreateCell(16).SetCellFormula("SUM(Q3:Q63)");
                        row.CreateCell(17).SetCellFormula("SUM(R3:R63)");
                        row.CreateCell(18).SetCellFormula("SUM(S3:S63)");
                        row.CreateCell(19).SetCellFormula("SUM(T3:T63)");
                        row.CreateCell(20).SetCellFormula("SUM(U3:U63)");
                        row.CreateCell(21).SetCellFormula("SUM(V3:V63)");
                        row.CreateCell(22).SetCellFormula("SUM(W3:W63)");
                        row.CreateCell(23).SetCellFormula("SUM(X3:X63)");
                        row.CreateCell(24).SetCellFormula("SUM(Y3:Y63)");
                        row.CreateCell(25).SetCellFormula("SUM(Z3:Z63)");
                        row.CreateCell(26).SetCellFormula("SUM(AA3:AA63)");
                        row.CreateCell(27).SetCellFormula("SUM(AB3:AB63)");
                        row.CreateCell(28).SetCellFormula("SUM(AC3:AC63)");
                        row.CreateCell(29).SetCellFormula("SUM(AD3:AD63)");
                        i++;
                        continue;
                    }

                    row.CreateCell(1).SetCellValue(rdr["ItemName"].ToString());
                    row.CreateCell(2).SetCellValue(rdr["BSAssetCurrentTrade"].ToString() == "" ? 0 : Double.Parse(rdr["BSAssetCurrentTrade"].ToString()));
                    row.CreateCell(3).SetCellValue(rdr["BSAssetCurrentLoan"].ToString() == "" ? 0 : Double.Parse(rdr["BSAssetCurrentLoan"].ToString()));
                    row.CreateCell(4).SetCellValue(rdr["BSAssetCurrentNonTrade"].ToString() == "" ? 0 : Double.Parse(rdr["BSAssetCurrentNonTrade"].ToString()));
                    row.CreateCell(5).SetCellValue(rdr["BSAssetNonCurrent"].ToString() == "" ? 0 : Double.Parse(rdr["BSAssetNonCurrent"].ToString()));
                    row.CreateCell(6).SetCellValue(rdr["BSAssetNonCurrentExclude"].ToString() == "" ? 0 : Double.Parse(rdr["BSAssetNonCurrentExclude"].ToString()));
                    row.CreateCell(7).SetCellValue(rdr["BSLiabilitiesAccruedPurchase"].ToString() == "" ? 0 : Double.Parse(rdr["BSLiabilitiesAccruedPurchase"].ToString()));
                    row.CreateCell(8).SetCellValue(rdr["BSLiabilitiesCurrentTrade"].ToString() == "" ? 0 : Double.Parse(rdr["BSLiabilitiesCurrentTrade"].ToString()));
                    row.CreateCell(9).SetCellValue(rdr["BSLiabilitiesCurrentNonTrade"].ToString() == "" ? 0 : Double.Parse(rdr["BSLiabilitiesCurrentNonTrade"].ToString()));
                    row.CreateCell(10).SetCellValue(rdr["BSLiabilitiesCurrentLoan"].ToString() == "" ? 0 : Double.Parse(rdr["BSLiabilitiesCurrentLoan"].ToString()));
                    row.CreateCell(11).SetCellValue(rdr["BSLiabilitiesNonCurrent"].ToString() == "" ? 0 : Double.Parse(rdr["BSLiabilitiesNonCurrent"].ToString()));
                    row.CreateCell(12).SetCellValue(rdr["BSEquityQuasi"].ToString() == "" ? 0 : Double.Parse(rdr["BSEquityQuasi"].ToString()));
                    row.CreateCell(13).SetCellValue(rdr["PNLSalesGoods"].ToString() == "" ? 0 : Double.Parse(rdr["PNLSalesGoods"].ToString()));
                    row.CreateCell(14).SetCellValue(rdr["PNLSalesCylinderRental"].ToString() == "" ? 0 : Double.Parse(rdr["PNLSalesCylinderRental"].ToString()));
                    row.CreateCell(15).SetCellValue(rdr["PNLSalesEquipRental"].ToString() == "" ? 0 : Double.Parse(rdr["PNLSalesEquipRental"].ToString()));
                    row.CreateCell(16).SetCellValue(rdr["PNLSalesPropertyRental"].ToString() == "" ? 0 : Double.Parse(rdr["PNLSalesPropertyRental"].ToString()));
                    row.CreateCell(17).SetCellValue(rdr["PNLSalesOthers"].ToString() == "" ? 0 : Double.Parse(rdr["PNLSalesOthers"].ToString()));
                    row.CreateCell(18).SetCellValue(rdr["PNLInterestIncome"].ToString() == "" ? 0 : Double.Parse(rdr["PNLInterestIncome"].ToString()));
                    row.CreateCell(19).SetCellValue(rdr["PNLInterestExpense"].ToString() == "" ? 0 : Double.Parse(rdr["PNLInterestExpense"].ToString()));
                    row.CreateCell(20).SetCellValue(rdr["PNLCommissionIncome"].ToString() == "" ? 0 : Double.Parse(rdr["PNLCommissionIncome"].ToString()));
                    row.CreateCell(21).SetCellValue(rdr["PNLPropertyRentalIncome"].ToString() == "" ? 0 : Double.Parse(rdr["PNLPropertyRentalIncome"].ToString()));
                    row.CreateCell(22).SetCellValue(rdr["PNLAdminMgmtFee"].ToString() == "" ? 0 : Double.Parse(rdr["PNLAdminMgmtFee"].ToString()));
                    row.CreateCell(23).SetCellValue(rdr["PNLSNDRentalExpense"].ToString() == "" ? 0 : Double.Parse(rdr["PNLSNDRentalExpense"].ToString()));
                    row.CreateCell(24).SetCellValue(rdr["PNLANGRentalExpense"].ToString() == "" ? 0 : Double.Parse(rdr["PNLANGRentalExpense"].ToString()));
                    row.CreateCell(25).SetCellValue(rdr["PNLSNDRecovery"].ToString() == "" ? 0 : Double.Parse(rdr["PNLSNDRecovery"].ToString()));
                    row.CreateCell(26).SetCellValue(rdr["PNLANGRecovery"].ToString() == "" ? 0 : Double.Parse(rdr["PNLANGRecovery"].ToString()));
                    row.CreateCell(27).SetCellValue(rdr["PNLDividendIncome"].ToString() == "" ? 0 : Double.Parse(rdr["PNLDividendIncome"].ToString()));
                    row.CreateCell(28).SetCellValue(rdr["PNLDividendExpense"].ToString() == "" ? 0 : Double.Parse(rdr["PNLDividendExpense"].ToString()));
                    row.CreateCell(29).SetCellValue(rdr["DividendDeclared"].ToString() == "" ? 0 : Double.Parse(rdr["DividendDeclared"].ToString()));
                    i++;
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
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
        #endregion

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload.HasFile)
            {
                FileUpload.SaveAs(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + "\\" + FileUpload.FileName);

                this.IFrame.Attributes["style"] = "";
                this.IFrame.Attributes["src"] = String.Format("ParsingInterCompany.aspx?FILENAME={0}&YEAR={1}&MONTH={2}",
                                                            Server.UrlEncode(FileUpload.FileName),
                                                            this.ddlYear.SelectedValue, this.ddlMonth.SelectedValue);
            }
            else
            {
                lblMsg.Text = "You have not specified a file.";
            }
        }
    }
}
