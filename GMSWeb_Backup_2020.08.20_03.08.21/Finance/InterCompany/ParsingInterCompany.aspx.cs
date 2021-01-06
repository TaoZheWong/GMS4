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
using System.Transactions;
using System.Data.SqlClient;
using System.Text;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Finance.InterCompany
{
    public partial class ParsingInterCompany : GMSBasePage
    {
        private string excelFilePath = "", excelFileName="";
        private short year = 0;
        private short month = 0;
        private short coyId = 0;
        private bool isTBAvailable = true; 
        
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            coyId = session.CompanyId;
            Company company = new SystemDataActivity().RetrieveCompanyById(coyId, session);

            if (company != null)
            {
                isTBAvailable = company.IsTBAvailable; 
            }
            
            this.excelFileName = Request.Params["FILENAME"];
            this.year = GMSUtil.ToShort(Request.Params["YEAR"]);
            this.month = GMSUtil.ToShort(Request.Params["MONTH"]);
            
            excelFilePath = AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + Path.DirectorySeparatorChar + excelFileName;

            Response.ContentType = "text/html";
            if (excelFileName.Contains("COA2016"))
            {
                ParseExcelFile_newCOA();
            }
            else
            {
                ParseExcelFile_oldCOA();
            }      
        }

        protected void ParseExcelFile_oldCOA()
        {
            DataSet dsExcel = new DataSet();

            try
            {
                Response.Output.Write("Parsing Excel file...<br>");
                Response.Flush();

                Asiasoft.MSExcelFileReader.SheetDataLoader sheetDataLoader_InterCompany = new Asiasoft.MSExcelFileReader.SheetDataLoader();
                sheetDataLoader_InterCompany.ExcelFilePath = this.excelFilePath;
                sheetDataLoader_InterCompany.IsHeaderIncludedInExcelFile = true;
                sheetDataLoader_InterCompany.SheetName = "Sheet1";
                sheetDataLoader_InterCompany.LoadExcelData();

                dsExcel = sheetDataLoader_InterCompany.ExcelData;

                Double BSAssetCurrentTrade = 0;
                Double BSAssetCurrentLoan = 0;
                Double BSAssetCurrentNonTrade = 0;
                Double BSAssetNonCurrentNonTrade = 0;
                Double BSAssetNonCurrentLoan = 0;
                Double BSLiabilitiesAccruedPurchase = 0;
                Double BSLiabilitiesCurrentTrade = 0;
                Double BSLiabilitiesCurrentNonTrade = 0;
                Double BSLiabilitiesCurrentLoan = 0;
                Double BSLiabilitiesNonCurrentNonTrade = 0;
                Double BSLiabilitiesNonCurrentLoan = 0;
                Double BSEquityQuasi = 0;
                Double PNLSalesTrade = 0;
                Double PNLSalesManu = 0;
                Double PNLSalesRental = 0;
                Double PNLSalesOthers = 0;
                Double PNLInterestIncome = 0;
                Double PNLInterestExpense = 0;
                Double PNLCommissionIncome = 0;
                Double PNLPropertyRentalIncome = 0;
                Double PNLAdminMgtFee = 0;
                Double PNLOtherIncome = 0;
                Double PNLOtherExpense = 0;
                Double PNLSNDRentalExpense = 0;
                Double PNLANGRentalExpense = 0;
                Double PNLSNDRecovery = 0;
                Double PNLANGRecovery = 0;
                Double PNLDividendIncome = 0;
                Double PNLDividendExpense = 0;

                Double CTIBSAssetCurrentTrade = 0;
                Double CTIBSAssetCurrentLoan = 0;
                Double CTIBSAssetCurrentNonTrade = 0;
                Double CTIBSAssetNonCurrentNonTrade = 0;
                Double CTIBSAssetNonCurrentLoan = 0;
                Double CTIBSLiabilitiesAccruedPurchase = 0;
                Double CTIBSLiabilitiesCurrentTrade = 0;
                Double CTIBSLiabilitiesCurrentNonTrade = 0;
                Double CTIBSLiabilitiesCurrentLoan = 0;
                Double CTIBSLiabilitiesNonCurrentNonTrade = 0;
                Double CTIBSLiabilitiesNonCurrentLoan = 0;
                Double CTIBSEquityQuasi = 0;
                Double CTIPNLSalesTrade = 0;
                Double CTIPNLSalesManu = 0;
                Double CTIPNLSalesRental = 0;
                Double CTIPNLSalesOthers = 0;
                Double CTIPNLInterestIncome = 0;
                Double CTIPNLInterestExpense = 0;
                Double CTIPNLCommissionIncome = 0;
                Double CTIPNLPropertyRentalIncome = 0;
                Double CTIPNLAdminMgtFee = 0;
                Double CTIPNLOtherIncome = 0;
                Double CTIPNLOtherExpense = 0;
                Double CTIPNLSNDRentalExpense = 0;
                Double CTIPNLANGRentalExpense = 0;
                Double CTIPNLSNDRecovery = 0;
                Double CTIPNLANGRecovery = 0;
                Double CTIPNLDividendIncome = 0;
                Double CTIPNLDividendExpense = 0;

                using (TransactionScope tran = new TransactionScope())
                {
                    bool hasError = false;
                    string hasErrorMessage = ""; 

                    for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
                    {
                        if (i == 63) 
                            continue;
                        else if (i >= 65) 
                            break;

                        if (i == 0)
                        {
                            //retrieve from system for Check Againsts values
                            IDbConnection conn = new ConnectionManager().GetConnection(); 
                            SqlDataReader rdr = null;
                            StringBuilder sb = new StringBuilder();

                            try
                            {
                                conn.Open();
                                SqlCommand command = new SqlCommand("procFinanceInterCompanyTransactionVerify", (SqlConnection)conn);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyId;
                                command.Parameters.Add("@Year", SqlDbType.SmallInt).Value = this.year;
                                command.Parameters.Add("@Month", SqlDbType.SmallInt).Value = this.month; 
                                rdr = command.ExecuteReader();

                                 while (rdr.Read())
                                 {
                                     if (rdr["ItemName"].ToString() == "Check Against GMS (Total InterCompany)")
                                     {
                                         CTIBSAssetCurrentTrade = Double.Parse(rdr["BSAssetCurrentTrade"].ToString());
                                         CTIBSAssetCurrentLoan = Double.Parse(rdr["BSAssetCurrentLoan"].ToString());
                                         CTIBSAssetCurrentNonTrade = Double.Parse(rdr["BSAssetCurrentNonTrade"].ToString());
                                         CTIBSAssetNonCurrentNonTrade = Double.Parse(rdr["BSAssetNonCurrentNonTrade"].ToString());
                                         CTIBSAssetNonCurrentLoan = Double.Parse(rdr["BSAssetNonCurrentLoan"].ToString());
                                         CTIBSLiabilitiesAccruedPurchase = Double.Parse(rdr["BSLiabilitiesAccruedPurchase"].ToString());
                                         CTIBSLiabilitiesCurrentTrade = Double.Parse(rdr["BSLiabilitiesCurrentTrade"].ToString());
                                         CTIBSLiabilitiesCurrentNonTrade = Double.Parse(rdr["BSLiabilitiesCurrentNonTrade"].ToString());
                                         CTIBSLiabilitiesCurrentLoan = Double.Parse(rdr["BSLiabilitiesCurrentLoan"].ToString());
                                         CTIBSLiabilitiesNonCurrentNonTrade = Double.Parse(rdr["BSLiabilitiesNonCurrentNonTrade"].ToString());
                                         CTIBSLiabilitiesNonCurrentLoan = Double.Parse(rdr["BSLiabilitiesNonCurrentLoan"].ToString());
                                         CTIBSEquityQuasi = Double.Parse(rdr["BSEquityQuasi"].ToString());
                                         CTIPNLSalesTrade = Double.Parse(rdr["PNLSalesTrade"].ToString());
                                         CTIPNLSalesManu = Double.Parse(rdr["PNLSalesManu"].ToString());
                                         CTIPNLSalesRental = Double.Parse(rdr["PNLSalesRental"].ToString());
                                         CTIPNLSalesOthers = Double.Parse(rdr["PNLSalesOthers"].ToString());
                                         CTIPNLInterestIncome = Double.Parse(rdr["PNLInterestIncome"].ToString());
                                         CTIPNLInterestExpense = Double.Parse(rdr["PNLInterestExpense"].ToString());
                                         CTIPNLCommissionIncome = Double.Parse(rdr["PNLCommissionIncome"].ToString());
                                         CTIPNLPropertyRentalIncome = Double.Parse(rdr["PNLPropertyRentalIncome"].ToString());
                                         CTIPNLAdminMgtFee = Double.Parse(rdr["PNLAdminMgtFee"].ToString());
                                         //CTIPNLOtherIncome = Double.Parse(rdr["PNLOtherIncome"].ToString());
                                         //CTIPNLOtherExpense = Double.Parse(rdr["PNLOtherExpense"].ToString());
                                         CTIPNLSNDRentalExpense = Double.Parse(rdr["PNLSNDRentalExpense"].ToString());
                                         CTIPNLANGRentalExpense = Double.Parse(rdr["PNLANGRentalExpense"].ToString());
                                         CTIPNLSNDRecovery = Double.Parse(rdr["PNLSNDRecovery"].ToString());
                                         CTIPNLANGRecovery = Double.Parse(rdr["PNLANGRecovery"].ToString());
                                         CTIPNLDividendIncome = Double.Parse(rdr["PNLDividendIncome"].ToString());
                                         CTIPNLDividendExpense = Double.Parse(rdr["PNLDividendExpense"].ToString());
                                     }
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
                             continue; 
                        }//end if i == 0

                        if (i < 64) 
                        {
                            BSAssetCurrentTrade += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetCurrentTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetCurrentTrade"].ToString());
                            BSAssetCurrentLoan += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetCurrentLoan"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetCurrentLoan"].ToString());
                            BSAssetCurrentNonTrade += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetCurrentNonTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetCurrentNonTrade"].ToString());
                            BSAssetNonCurrentNonTrade += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrentNonTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrentNonTrade"].ToString());
                            BSAssetNonCurrentLoan += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrentLoan"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrentLoan"].ToString());
                            BSLiabilitiesAccruedPurchase += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesAccruedPurchase"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesAccruedPurchase"].ToString());
                            BSLiabilitiesCurrentTrade += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentTrade"].ToString());
                            BSLiabilitiesCurrentNonTrade += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentNonTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentNonTrade"].ToString());
                            BSLiabilitiesCurrentLoan += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentLoan"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentLoan"].ToString());
                            BSLiabilitiesNonCurrentNonTrade += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesNonCurrentNonTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesNonCurrentNonTrade"].ToString());
                            BSLiabilitiesNonCurrentLoan += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesNonCurrentLoan"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesNonCurrentLoan"].ToString());
                            BSEquityQuasi += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSEquityQuasi"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSEquityQuasi"].ToString());
                            PNLSalesTrade += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesTrade"].ToString());
                            PNLSalesManu += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesManu"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesManu"].ToString());
                            PNLSalesRental += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesRental"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesRental"].ToString());
                            PNLSalesOthers += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesOthers"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesOthers"].ToString());
                            PNLInterestIncome += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLInterestIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLInterestIncome"].ToString());
                            PNLInterestExpense += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLInterestExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLInterestExpense"].ToString());
                            PNLCommissionIncome += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLCommissionIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLCommissionIncome"].ToString());
                            PNLPropertyRentalIncome += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLPropertyRentalIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLPropertyRentalIncome"].ToString());
                            PNLAdminMgtFee += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLAdminMgtFee"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLAdminMgtFee"].ToString());
                            //PNLOtherIncome += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLOtherIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLOtherIncome"].ToString());
                            //PNLOtherExpense += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLOtherExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLOtherExpense"].ToString());
                            PNLSNDRentalExpense += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSNDRentalExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSNDRentalExpense"].ToString());
                            PNLANGRentalExpense += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLANGRentalExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLANGRentalExpense"].ToString());
                            PNLSNDRecovery += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSNDRecovery"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSNDRecovery"].ToString());
                            PNLANGRecovery += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLANGRecovery"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLANGRecovery"].ToString());
                            PNLDividendIncome += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLDividendIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLDividendIncome"].ToString());
                            PNLDividendExpense += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLDividendExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLDividendExpense"].ToString());
                        }

                        if (i == 64) 
                        {
                            if (isTBAvailable)
                            {
                                //if (Math.Round(BSAssetCurrentTrade) != Math.Round(CTIBSAssetCurrentTrade) ||
                                //    Math.Round(BSAssetCurrentLoan) != Math.Round(CTIBSAssetCurrentLoan) ||
                                //    Math.Round(BSAssetCurrentNonTrade) != Math.Round(CTIBSAssetCurrentNonTrade) ||
                                //    Math.Round(BSAssetNonCurrentNonTrade) != Math.Round(CTIBSAssetNonCurrentNonTrade) ||
                                //    Math.Round(BSAssetNonCurrentLoan) != Math.Round(CTIBSAssetNonCurrentLoan) ||
                                //    Math.Round(BSLiabilitiesAccruedPurchase) != Math.Round(CTIBSLiabilitiesAccruedPurchase) ||
                                //    Math.Round(BSLiabilitiesCurrentTrade) != Math.Round(CTIBSLiabilitiesCurrentTrade) ||
                                //    Math.Round(BSLiabilitiesCurrentNonTrade) != Math.Round(CTIBSLiabilitiesCurrentNonTrade) ||
                                //    Math.Round(BSLiabilitiesCurrentLoan) != Math.Round(CTIBSLiabilitiesCurrentLoan) ||
                                //    Math.Round(BSLiabilitiesNonCurrentNonTrade) != Math.Round(CTIBSLiabilitiesNonCurrentNonTrade) ||
                                //    Math.Round(BSLiabilitiesNonCurrentLoan) != Math.Round(CTIBSLiabilitiesNonCurrentLoan) ||
                                //    Math.Round(BSEquityQuasi) != Math.Round(CTIBSEquityQuasi) ||
                                //    Math.Round(PNLSalesTrade) != Math.Round(CTIPNLSalesTrade) ||
                                //    Math.Round(PNLSalesManu) != Math.Round(CTIPNLSalesManu) ||
                                //    Math.Round(PNLSalesRental) != Math.Round(CTIPNLSalesRental) ||
                                //    Math.Round(PNLSalesOthers) != Math.Round(CTIPNLSalesOthers) ||
                                //    Math.Round(PNLInterestIncome) != Math.Round(CTIPNLInterestIncome) ||
                                //    Math.Round(PNLInterestExpense) != Math.Round(CTIPNLInterestExpense) ||
                                //    Math.Round(PNLCommissionIncome) != Math.Round(CTIPNLCommissionIncome) ||
                                //    Math.Round(PNLPropertyRentalIncome) != Math.Round(CTIPNLPropertyRentalIncome) ||
                                //    Math.Round(PNLAdminMgtFee) != Math.Round(CTIPNLAdminMgtFee) ||
                                //    Math.Round(PNLOtherIncome) != Math.Round(CTIPNLOtherIncome) ||
                                //    Math.Round(PNLOtherExpense) != Math.Round(CTIPNLOtherExpense) ||
                                //    Math.Round(PNLSNDRentalExpense) != Math.Round(CTIPNLSNDRentalExpense) ||
                                //    Math.Round(PNLANGRentalExpense) != Math.Round(CTIPNLANGRentalExpense) ||
                                //    Math.Round(PNLSNDRecovery) != Math.Round(CTIPNLSNDRecovery) ||
                                //    Math.Round(PNLANGRecovery) != Math.Round(CTIPNLANGRecovery) ||
                                //    Math.Round(PNLDividendIncome) != Math.Round(CTIPNLDividendIncome) ||
                                //    Math.Round(PNLDividendExpense) != Math.Round(CTIPNLDividendExpense))
                                //{
                                if (Math.Round(BSAssetCurrentTrade) != Math.Round(CTIBSAssetCurrentTrade)) 
                                    hasErrorMessage = "BSAssetCurrentTrade"; 
                                if (Math.Round(BSAssetCurrentLoan) != Math.Round(CTIBSAssetCurrentLoan)) 
                                    hasErrorMessage = "BSAssetCurrentLoan";
                                if (Math.Round(BSAssetCurrentNonTrade) != Math.Round(CTIBSAssetCurrentNonTrade))
                                    hasErrorMessage = "BSAssetCurrentNonTrade"; 
                                if (Math.Round(BSAssetNonCurrentNonTrade) != Math.Round(CTIBSAssetNonCurrentNonTrade))
                                    hasErrorMessage = "BSAssetNonCurrentNonTrade"; 
                                if (Math.Round(BSAssetNonCurrentLoan) != Math.Round(CTIBSAssetNonCurrentLoan))
                                    hasErrorMessage = "BSAssetNonCurrentLoan"; 
                                if (Math.Round(BSLiabilitiesAccruedPurchase) != Math.Round(CTIBSLiabilitiesAccruedPurchase))
                                    hasErrorMessage = "BSLiabilitiesAccruedPurchase"; 
                                if (Math.Round(BSLiabilitiesCurrentTrade) != Math.Round(CTIBSLiabilitiesCurrentTrade))
                                    hasErrorMessage = "BSLiabilitiesCurrentTrade"; 
                                if (Math.Round(BSLiabilitiesCurrentNonTrade) != Math.Round(CTIBSLiabilitiesCurrentNonTrade))
                                    hasErrorMessage = "BSLiabilitiesCurrentNonTrade"; 
                                if (Math.Round(BSLiabilitiesCurrentLoan) != Math.Round(CTIBSLiabilitiesCurrentLoan))
                                    hasErrorMessage = "BSLiabilitiesCurrentLoan"; 
                                if (Math.Round(BSLiabilitiesNonCurrentNonTrade) != Math.Round(CTIBSLiabilitiesNonCurrentNonTrade))
                                    hasErrorMessage = "BSLiabilitiesNonCurrentNonTrade"; 
                                if (Math.Round(BSLiabilitiesNonCurrentLoan) != Math.Round(CTIBSLiabilitiesNonCurrentLoan))
                                    hasErrorMessage = "BSLiabilitiesNonCurrentLoan"; 
                                if (Math.Round(BSEquityQuasi) != Math.Round(CTIBSEquityQuasi))
                                    hasErrorMessage = "BSEquityQuasi"; 
                                if (Math.Round(PNLSalesTrade) != Math.Round(CTIPNLSalesTrade))
                                    hasErrorMessage = "PNLSalesTrade"; 
                                if (Math.Round(PNLSalesManu) != Math.Round(CTIPNLSalesManu))
                                    hasErrorMessage = "PNLSalesManu"; 
                                if (Math.Round(PNLSalesRental) != Math.Round(CTIPNLSalesRental))
                                    hasErrorMessage = "PNLSalesRental"; 
                                if (Math.Round(PNLSalesOthers) != Math.Round(CTIPNLSalesOthers))
                                    hasErrorMessage = "PNLSalesOthers"; 
                                if (Math.Round(PNLInterestIncome) != Math.Round(CTIPNLInterestIncome))
                                    hasErrorMessage = "PNLInterestIncome"; 
                                if (Math.Round(PNLInterestExpense) != Math.Round(CTIPNLInterestExpense))
                                    hasErrorMessage = "PNLInterestExpense"; 
                                if (Math.Round(PNLCommissionIncome) != Math.Round(CTIPNLCommissionIncome))
                                    hasErrorMessage = "PNLCommissionIncome"; 
                                if (Math.Round(PNLPropertyRentalIncome) != Math.Round(CTIPNLPropertyRentalIncome))
                                    hasErrorMessage = "PNLPropertyRentalIncome"; 
                                if (Math.Round(PNLAdminMgtFee) != Math.Round(CTIPNLAdminMgtFee))
                                    hasErrorMessage = "PNLAdminMgtFee"; 
                                //if (Math.Round(PNLOtherIncome) != Math.Round(CTIPNLOtherIncome))
                                //    hasErrorMessage = "PNLOtherIncome"; 
                                //if (Math.Round(PNLOtherExpense) != Math.Round(CTIPNLOtherExpense))
                                //    hasErrorMessage = "PNLOtherExpense"; 
                                if (Math.Round(PNLSNDRentalExpense) != Math.Round(CTIPNLSNDRentalExpense))
                                    hasErrorMessage = "PNLSNDRentalExpense"; 
                                if (Math.Round(PNLANGRentalExpense) != Math.Round(CTIPNLANGRentalExpense))
                                    hasErrorMessage = "PNLANGRentalExpense"; 
                                if (Math.Round(PNLSNDRecovery) != Math.Round(CTIPNLSNDRecovery))
                                    hasErrorMessage = "PNLSNDRecovery"; 
                                if (Math.Round(PNLANGRecovery) != Math.Round(CTIPNLANGRecovery))
                                    hasErrorMessage = "PNLANGRecovery"; 
                                if (Math.Round(PNLDividendIncome) != Math.Round(CTIPNLDividendIncome))
                                    hasErrorMessage = "PNLDividendIncome"; 
                                if (Math.Round(PNLDividendExpense) != Math.Round(CTIPNLDividendExpense))
                                    hasErrorMessage = "PNLDividendExpense"; 

                                if (hasErrorMessage != "") 
                                {
                                    hasError = true;
                                    break;
                                }
                            }
                            continue; 
                        }                    

                        IList<Company> lstItem = new SystemDataActivity().RetrieveCompanyByCode(dsExcel.Tables[0].Rows[i]["Company"].ToString().Substring(0, 3));

                        if (lstItem != null)
                        {
                            if (lstItem.Count > 0)
                            {
                                Company company = (Company)lstItem[0];
                                LogSession sess = base.GetSessionInfo();

                                if (i == 1)
                                {
                                    // delete tbFinanceInterCompanyData records according to year and month specified by user, then insert new record
                                    ResultType result = new FinanceInterCompanyDataActivity().DeleteFinanceInterCompanyDataByYearMonth(this.year, this.month, sess.CompanyId);
                                    if (result != ResultType.Ok)
                                    {
                                        Response.Output.Write("<SPAN STYLE='color: red'>Cannot delete old data : " + result.ToString() + ".</SPAN><br>");
                                        Response.Flush();
                                        break;
                                    }
                                }

                                #region insert for each row
                                FinanceInterCompanyData interCompany = new FinanceInterCompanyData();
                                interCompany.CoyID = sess.CompanyId;
                                interCompany.TbYear = this.year;
                                interCompany.TbMonth = this.month;
                                interCompany.CounterCoyID = company.CoyID;

                                interCompany.BSAssetCurrentTrade = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetCurrentTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetCurrentTrade"].ToString());
                                interCompany.BSAssetCurrentLoan = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetCurrentLoan"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetCurrentLoan"].ToString());
                                interCompany.BSAssetCurrentNonTrade = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetCurrentNonTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetCurrentNonTrade"].ToString());
                                interCompany.BSAssetNonCurrentNonTrade = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrentNonTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrentNonTrade"].ToString());
                                interCompany.BSAssetNonCurrentLoan = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrentLoan"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrentLoan"].ToString());
                                interCompany.BSLiabilitiesAccruedPurchase = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesAccruedPurchase"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesAccruedPurchase"].ToString());
                                interCompany.BSLiabilitiesCurrentTrade = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentTrade"].ToString());
                                interCompany.BSLiabilitiesCurrentNonTrade = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentNonTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentNonTrade"].ToString());
                                interCompany.BSLiabilitiesCurrentLoan = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentLoan"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentLoan"].ToString());
                                interCompany.BSLiabilitiesNonCurrentNonTrade = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesNonCurrentNonTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesNonCurrentNonTrade"].ToString());
                                interCompany.BSLiabilitiesNonCurrentLoan = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesNonCurrentLoan"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesNonCurrentLoan"].ToString());
                                interCompany.BSEquityQuasi = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSEquityQuasi"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSEquityQuasi"].ToString());
                                interCompany.PNLSalesTrade = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesTrade"].ToString());
                                interCompany.PNLSalesManu = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesManu"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesManu"].ToString());
                                interCompany.PNLSalesEquipRental = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesRental"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesRental"].ToString());
                                interCompany.PNLSalesOthers = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesOthers"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesOthers"].ToString());
                                interCompany.PNLInterestIncome = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLInterestIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLInterestIncome"].ToString());
                                interCompany.PNLInterestExpense = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLInterestExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLInterestExpense"].ToString());
                                interCompany.PNLCommissionIncome = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLCommissionIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLCommissionIncome"].ToString());
                                interCompany.PNLPropertyRentalIncome = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLPropertyRentalIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLPropertyRentalIncome"].ToString());
                                interCompany.PNLAdminMgtFee = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLAdminMgtFee"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLAdminMgtFee"].ToString());
                                //interCompany.PNLOtherIncome = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLOtherIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLOtherIncome"].ToString());
                                //interCompany.PNLOtherExpense = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLOtherExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLOtherExpense"].ToString());
                                interCompany.PNLSNDRentalExpense = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSNDRentalExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSNDRentalExpense"].ToString());
                                interCompany.PNLANGRentalExpense = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLANGRentalExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLANGRentalExpense"].ToString());
                                interCompany.PNLSNDRecovery = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSNDRecovery"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSNDRecovery"].ToString());
                                interCompany.PNLANGRecovery = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLANGRecovery"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLANGRecovery"].ToString());
                                interCompany.PNLDividendIncome = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLDividendIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLDividendIncome"].ToString());
                                interCompany.PNLDividendExpense = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLDividendExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLDividendExpense"].ToString());
                                //interCompany.BSAssetTrade = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetTrade"].ToString());
                                interCompany.CreatedBy = sess.UserId;
                                interCompany.CreatedDate = DateTime.Now;

                                Response.Output.Write("Inserting intercompany transactions for " + company.Code + "' ...<br>");
                                Response.Flush();

                                if (interCompany.CoyID == interCompany.CounterCoyID &&
                                      !(interCompany.BSAssetCurrentTrade == 0 &&
                                        interCompany.BSAssetCurrentLoan == 0 &&
                                        interCompany.BSAssetCurrentNonTrade == 0 &&
                                        interCompany.BSAssetNonCurrentNonTrade == 0 &&
                                        interCompany.BSAssetNonCurrentLoan == 0 &&
                                        interCompany.BSLiabilitiesAccruedPurchase == 0 &&
                                        interCompany.BSLiabilitiesCurrentTrade == 0 &&
                                        interCompany.BSLiabilitiesCurrentNonTrade == 0 &&
                                        interCompany.BSLiabilitiesCurrentLoan == 0 &&
                                        interCompany.BSLiabilitiesNonCurrentNonTrade == 0 &&
                                        interCompany.BSLiabilitiesNonCurrentLoan == 0 &&
                                        interCompany.BSEquityQuasi == 0 &&
                                        interCompany.PNLSalesTrade == 0 &&
                                        interCompany.PNLSalesManu == 0 &&
                                        interCompany.PNLSalesEquipRental == 0 &&
                                        interCompany.PNLSalesOthers == 0 &&
                                        interCompany.PNLInterestIncome == 0 &&
                                        interCompany.PNLInterestExpense == 0 &&
                                        interCompany.PNLCommissionIncome == 0 &&
                                        interCompany.PNLPropertyRentalIncome == 0 &&
                                        interCompany.PNLAdminMgtFee == 0 &&
                                        //interCompany.PNLOtherIncome == 0 &&
                                        //interCompany.PNLOtherExpense == 0 &&
                                        interCompany.PNLSNDRentalExpense == 0 &&
                                        interCompany.PNLANGRentalExpense == 0 &&
                                        interCompany.PNLSNDRecovery == 0 &&
                                        interCompany.PNLANGRecovery == 0 &&
                                        interCompany.PNLDividendIncome == 0 &&
                                        interCompany.PNLDividendExpense == 0))
                                {
                                    Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error: Cannot insert self-company at row " + (i + 1) + ".</SPAN><br>");
                                    Response.Flush();
                                    Response.Output.Write("<br>");
                                }
                                else
                                {
                                    if (interCompany.CoyID == interCompany.CounterCoyID)
                                    {
                                        Response.Output.Write("Bypassing insertion for self-company.<br><br>");
                                        continue;
                                    }
                                    try
                                    {
                                        ResultType create = new FinanceInterCompanyDataActivity().CreateFinanceInterCompanyData(ref interCompany, sess);
                                        if (create == ResultType.Ok)
                                        {
                                            Response.Output.Write("Inserting successful.<br>");
                                            Response.Flush();
                                        }
                                        else
                                        {
                                            Response.Output.Write("<SPAN STYLE='color: red'>Processing error of type : " + create.ToString() + "at row " + (i + 1) + ".</SPAN><br>");
                                            Response.Flush();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error:" + ex.Message + "at row " + (i + 1) + ".</SPAN><br>");
                                        Response.Flush();
                                    }
                                    Response.Output.Write("<br>");
                                }
                                #endregion
                            }
                            else
                            {
                                Response.Output.Write("<SPAN STYLE='color: red'>Company '" + dsExcel.Tables[0].Rows[i]["Company"].ToString() + "' cannot be found at row " + (i + 1) + ".</SPAN><br><br>");
                                Response.Flush();
                            }
                        }
                    }//end for
                    if (!hasError) tran.Complete();
                    else
                    {
                        Response.Output.Write("<SPAN STYLE='color: red'>Data is not tallied for column <b>" + hasErrorMessage + "</b>. Please check your data again. Rolling back to previous state ...</SPAN><br><br>");
                        Response.Flush();
                    }
                }//end transaction
                //delete Excel file
                File.Delete(excelFilePath);
                Response.Output.Write("<SPAN STYLE='color: red'>End of insertion.</SPAN><br><br>");
                Response.Flush();
            }
            catch (Exception ex)
            {
                Response.Output.Write(("<SPAN STYLE='color: red'><B>Error:" + ex.Message.ToString() + ".</B></SPAN><br>"));
            }
        }
        protected void ParseExcelFile_newCOA()
        {
            DataSet dsExcel = new DataSet();

            try
            {
                Response.Output.Write("Parsing Excel file...<br>");
                Response.Flush();

                Asiasoft.MSExcelFileReader.SheetDataLoader sheetDataLoader_InterCompany = new Asiasoft.MSExcelFileReader.SheetDataLoader();
                sheetDataLoader_InterCompany.ExcelFilePath = this.excelFilePath;
                sheetDataLoader_InterCompany.IsHeaderIncludedInExcelFile = true;
                sheetDataLoader_InterCompany.SheetName = "Sheet1";
                sheetDataLoader_InterCompany.LoadExcelData();

                dsExcel = sheetDataLoader_InterCompany.ExcelData;

                Double BSAssetCurrentTrade = 0;
                Double BSAssetCurrentLoan = 0;
                Double BSAssetCurrentNonTrade = 0;
                Double BSAssetNonCurrent = 0;
                Double BSAssetNonCurrentExclude = 0;
                Double BSLiabilitiesAccruedPurchase = 0;
                Double BSLiabilitiesCurrentTrade = 0;
                Double BSLiabilitiesCurrentNonTrade = 0;
                Double BSLiabilitiesCurrentLoan = 0;
                Double BSLiabilitiesNonCurrent = 0;
                Double BSEquityQuasi = 0;
                Double PNLSalesGoods = 0;
                Double PNLSalesCylinderRental = 0;
                Double PNLSalesRental = 0;
                Double PNLSalesPropertyRental = 0;
                Double PNLSalesOthers = 0;
                Double PNLInterestIncome = 0;
                Double PNLInterestExpense = 0;
                Double PNLCommissionIncome = 0;
                Double PNLPropertyRentalIncome = 0;
                Double PNLAdminMgmtFee = 0;
                Double PNLSNDRentalExpense = 0;
                Double PNLANGRentalExpense = 0;
                Double PNLSNDRecovery = 0;
                Double PNLANGRecovery = 0;
                Double PNLDividendIncome = 0;
                Double PNLDividendExpense = 0;
                Double DividendDeclared = 0; 

                Double CTIBSAssetCurrentTrade = 0;
                Double CTIBSAssetCurrentLoan = 0;
                Double CTIBSAssetCurrentNonTrade = 0;
                Double CTIBSAssetNonCurrent = 0;
                Double CTIBSAssetNonCurrentExclude = 0;
                Double CTIBSLiabilitiesAccruedPurchase = 0;
                Double CTIBSLiabilitiesCurrentTrade = 0;
                Double CTIBSLiabilitiesCurrentNonTrade = 0;
                Double CTIBSLiabilitiesCurrentLoan = 0;
                Double CTIBSLiabilitiesNonCurrent = 0;
                Double CTIBSEquityQuasi = 0;
                Double CTIPNLSalesGoods = 0;
                Double CTIPNLSalesCylinderRental = 0;
                Double CTIPNLSalesRental = 0;
                Double CTIPNLSalesPropertyRental = 0;
                Double CTIPNLSalesOthers = 0;
                Double CTIPNLInterestIncome = 0;
                Double CTIPNLInterestExpense = 0;
                Double CTIPNLCommissionIncome = 0;
                Double CTIPNLPropertyRentalIncome = 0;
                Double CTIPNLAdminMgmtFee = 0;
                Double CTIPNLSNDRentalExpense = 0;
                Double CTIPNLANGRentalExpense = 0;
                Double CTIPNLSNDRecovery = 0;
                Double CTIPNLANGRecovery = 0;
                Double CTIPNLDividendIncome = 0;
                Double CTIPNLDividendExpense = 0;
                Double CTIDividendDeclared = 0; 

                using (TransactionScope tran = new TransactionScope())
                {
                    bool hasError = false;
                    string hasErrorMessage = "";

                    for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
                    {
                        if (i >= 61) 
                            break;

                        if (i == 0)
                        {
                            //retrieve from system for Check Againsts values
                            IDbConnection conn = new ConnectionManager().GetConnection();
                            SqlDataReader rdr = null;
                            StringBuilder sb = new StringBuilder();

                            try
                            {
                                conn.Open();
                                SqlCommand command = new SqlCommand("procFinanceInterCompanyTransactionVerify_COA2016", (SqlConnection)conn);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyId;
                                command.Parameters.Add("@Year", SqlDbType.SmallInt).Value = this.year;
                                command.Parameters.Add("@Month", SqlDbType.SmallInt).Value = this.month;
                                rdr = command.ExecuteReader();

                                while (rdr.Read())
                                {
                                    if (rdr["ItemName"].ToString() == "Check Against GMS (Total InterCompany)")
                                    {
                                        CTIBSAssetCurrentTrade = Double.Parse(rdr["BSAssetCurrentTrade"].ToString());
                                        CTIBSAssetCurrentLoan = Double.Parse(rdr["BSAssetCurrentLoan"].ToString());
                                        CTIBSAssetCurrentNonTrade = Double.Parse(rdr["BSAssetCurrentNonTrade"].ToString());
                                        CTIBSAssetNonCurrent = Double.Parse(rdr["BSAssetNonCurrent"].ToString());
                                        CTIBSAssetNonCurrentExclude = Double.Parse(rdr["BSAssetNonCurrentExclude"].ToString());
                                        CTIBSLiabilitiesAccruedPurchase = Double.Parse(rdr["BSLiabilitiesAccruedPurchase"].ToString());

                                        CTIBSLiabilitiesCurrentTrade = Double.Parse(rdr["BSLiabilitiesCurrentTrade"].ToString());
                                        CTIBSLiabilitiesCurrentNonTrade = Double.Parse(rdr["BSLiabilitiesCurrentNonTrade"].ToString());
                                        CTIBSLiabilitiesCurrentLoan = Double.Parse(rdr["BSLiabilitiesCurrentLoan"].ToString());
                                        CTIBSLiabilitiesNonCurrent = Double.Parse(rdr["BSLiabilitiesNonCurrent"].ToString());
                                        CTIBSEquityQuasi = Double.Parse(rdr["BSEquityQuasi"].ToString());

                                        CTIPNLSalesGoods = Double.Parse(rdr["PNLSalesGoods"].ToString());
                                        CTIPNLSalesCylinderRental = Double.Parse(rdr["PNLSalesCylinderRental"].ToString());
                                        CTIPNLSalesRental = Double.Parse(rdr["PNLSalesEquipRental"].ToString());
                                        CTIPNLSalesPropertyRental = Double.Parse(rdr["PNLSalesPropertyRental"].ToString());
                                        CTIPNLSalesOthers = Double.Parse(rdr["PNLSalesOthers"].ToString());
                                        CTIPNLInterestIncome = Double.Parse(rdr["PNLInterestIncome"].ToString());

                                        CTIPNLInterestExpense = Double.Parse(rdr["PNLInterestExpense"].ToString());
                                        CTIPNLCommissionIncome = Double.Parse(rdr["PNLCommissionIncome"].ToString());
                                        CTIPNLPropertyRentalIncome = Double.Parse(rdr["PNLPropertyRentalIncome"].ToString());
                                        CTIPNLAdminMgmtFee = Double.Parse(rdr["PNLAdminMgmtFee"].ToString());
                                        CTIPNLSNDRentalExpense = Double.Parse(rdr["PNLSNDRentalExpense"].ToString());

                                        CTIPNLANGRentalExpense = Double.Parse(rdr["PNLANGRentalExpense"].ToString());
                                        CTIPNLSNDRecovery = Double.Parse(rdr["PNLSNDRecovery"].ToString());
                                        CTIPNLANGRecovery = Double.Parse(rdr["PNLANGRecovery"].ToString());
                                        CTIPNLDividendIncome = Double.Parse(rdr["PNLDividendIncome"].ToString());
                                        CTIPNLDividendExpense = Double.Parse(rdr["PNLDividendExpense"].ToString());
                                        CTIDividendDeclared = Double.Parse(rdr["DividendDeclared"].ToString());
                                    }
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
                            continue;
                        }

                        if (i <= 59) 
                        {
                            BSAssetCurrentTrade += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetCurrentTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetCurrentTrade"].ToString());
                            BSAssetCurrentLoan += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetCurrentLoan"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetCurrentLoan"].ToString());
                            BSAssetCurrentNonTrade += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetCurrentNonTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetCurrentNonTrade"].ToString());
                            BSAssetNonCurrent += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrent"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrent"].ToString());
                            BSAssetNonCurrentExclude += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrentExclude"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrentExclude"].ToString());
                            BSLiabilitiesAccruedPurchase += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesAccruedPurchase"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesAccruedPurchase"].ToString());
                            BSLiabilitiesCurrentTrade += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentTrade"].ToString());
                            BSLiabilitiesCurrentNonTrade += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentNonTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentNonTrade"].ToString());
                            BSLiabilitiesCurrentLoan += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentLoan"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentLoan"].ToString());
                            BSLiabilitiesNonCurrent += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesNonCurrent"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesNonCurrent"].ToString());
                            BSEquityQuasi += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSEquityQuasi"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSEquityQuasi"].ToString());
                            PNLSalesGoods += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesGoods"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesGoods"].ToString());
                            PNLSalesCylinderRental += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesCylinderRental"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesCylinderRental"].ToString());
                            PNLSalesRental += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesEquipRental"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesEquipRental"].ToString());
                            PNLSalesPropertyRental += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesPropertyRental"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesPropertyRental"].ToString());
                            PNLSalesOthers += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesOthers"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesOthers"].ToString());
                            PNLInterestIncome += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLInterestIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLInterestIncome"].ToString());
                            PNLInterestExpense += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLInterestExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLInterestExpense"].ToString());
                            PNLCommissionIncome += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLCommissionIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLCommissionIncome"].ToString());
                            PNLPropertyRentalIncome += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLPropertyRentalIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLPropertyRentalIncome"].ToString());
                            PNLAdminMgmtFee += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLAdminMgmtFee"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLAdminMgmtFee"].ToString());
                            PNLSNDRentalExpense += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSNDRentalExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSNDRentalExpense"].ToString());
                            PNLANGRentalExpense += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLANGRentalExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLANGRentalExpense"].ToString());
                            PNLSNDRecovery += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSNDRecovery"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSNDRecovery"].ToString());
                            PNLANGRecovery += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLANGRecovery"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLANGRecovery"].ToString());
                            PNLDividendIncome += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLDividendIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLDividendIncome"].ToString());
                            PNLDividendExpense += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLDividendExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLDividendExpense"].ToString());
                            DividendDeclared += String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["DividendDeclared"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["DividendDeclared"].ToString());
                        }

                        if (i == 60) 
                        {
                            if (isTBAvailable)
                            {
                                BSAssetCurrentTrade = Math.Round(BSAssetCurrentTrade,2);
                                BSAssetCurrentLoan = Math.Round(BSAssetCurrentLoan,2);                                 
                                BSAssetCurrentNonTrade = Math.Round(BSAssetCurrentNonTrade,2);
                                BSAssetNonCurrent = Math.Round(BSAssetNonCurrent,2);
                                BSAssetNonCurrentExclude = Math.Round(BSAssetNonCurrentExclude, 2);
                                BSLiabilitiesAccruedPurchase = Math.Round(BSLiabilitiesAccruedPurchase,2);
                                BSLiabilitiesCurrentTrade = Math.Round(BSLiabilitiesCurrentTrade,2);
                                BSLiabilitiesCurrentNonTrade = Math.Round(BSLiabilitiesCurrentNonTrade,2);
                                BSLiabilitiesCurrentLoan = Math.Round(BSLiabilitiesCurrentLoan,2);
                                BSLiabilitiesNonCurrent = Math.Round(BSLiabilitiesNonCurrent,2);
                                BSEquityQuasi = Math.Round(BSEquityQuasi,2);
                                PNLSalesGoods = Math.Round(PNLSalesGoods,2);
                                PNLSalesCylinderRental = Math.Round(PNLSalesCylinderRental,2);
                                PNLSalesRental = Math.Round(PNLSalesRental,2);
                                PNLSalesPropertyRental = Math.Round(PNLSalesPropertyRental,2);
                                PNLSalesOthers = Math.Round(PNLSalesOthers,2);
                                PNLInterestIncome = Math.Round(PNLInterestIncome,2);
                                PNLInterestExpense = Math.Round(PNLInterestExpense,2);
                                PNLCommissionIncome = Math.Round(PNLCommissionIncome,2);
                                PNLPropertyRentalIncome = Math.Round(PNLPropertyRentalIncome,2);
                                PNLAdminMgmtFee = Math.Round(PNLAdminMgmtFee,2);
                                PNLSNDRentalExpense = Math.Round(PNLSNDRentalExpense,2);
                                PNLANGRentalExpense = Math.Round(PNLANGRentalExpense,2);
                                PNLSNDRecovery = Math.Round(PNLSNDRecovery,2);
                                PNLANGRecovery = Math.Round(PNLANGRecovery,2);
                                PNLDividendIncome = Math.Round(PNLDividendIncome,2);
                                PNLDividendExpense = Math.Round(PNLDividendExpense,2);
                                DividendDeclared = Math.Round(DividendDeclared, 2);

                                if (Math.Round(BSAssetCurrentTrade) != Math.Round(CTIBSAssetCurrentTrade))
                                    hasErrorMessage = "BSAssetCurrentTrade";
                                if (Math.Round(BSAssetCurrentLoan) != Math.Round(CTIBSAssetCurrentLoan))
                                    hasErrorMessage = "BSAssetCurrentLoan";
                                if (Math.Round(BSAssetCurrentNonTrade) != Math.Round(CTIBSAssetCurrentNonTrade))
                                    hasErrorMessage = "BSAssetCurrentNonTrade";
                                if (Math.Round(BSAssetNonCurrent) != Math.Round(CTIBSAssetNonCurrent))
                                    hasErrorMessage = "BSAssetNonCurrent";
                                if (Math.Round(BSAssetNonCurrentExclude) != Math.Round(CTIBSAssetNonCurrentExclude))
                                    hasErrorMessage = "BSAssetNonCurrentExclude";
                                if (Math.Round(BSLiabilitiesAccruedPurchase) != Math.Round(CTIBSLiabilitiesAccruedPurchase))
                                    hasErrorMessage = "BSLiabilitiesAccruedPurchase";
                                if (Math.Round(BSLiabilitiesCurrentTrade) != Math.Round(CTIBSLiabilitiesCurrentTrade))
                                    hasErrorMessage = "BSLiabilitiesCurrentTrade";
                                if (Math.Round(BSLiabilitiesCurrentNonTrade) != Math.Round(CTIBSLiabilitiesCurrentNonTrade))
                                    hasErrorMessage = "BSLiabilitiesCurrentNonTrade";
                                if (Math.Round(BSLiabilitiesCurrentLoan) != Math.Round(CTIBSLiabilitiesCurrentLoan))
                                    hasErrorMessage = "BSLiabilitiesCurrentLoan";
                                if (Math.Round(BSLiabilitiesNonCurrent) != Math.Round(CTIBSLiabilitiesNonCurrent))
                                    hasErrorMessage = "BSLiabilitiesNonCurrent";
                                if (Math.Round(BSEquityQuasi) != Math.Round(CTIBSEquityQuasi))
                                    hasErrorMessage = "BSEquityQuasi";
                                if (Math.Round(PNLSalesGoods) != Math.Round(CTIPNLSalesGoods))
                                    hasErrorMessage = "PNLSalesGoods";
                                if (Math.Round(PNLSalesCylinderRental) != Math.Round(CTIPNLSalesCylinderRental))
                                    hasErrorMessage = "PNLSalesCylinderRental";
                                if (Math.Round(PNLSalesRental) != Math.Round(CTIPNLSalesRental))
                                    hasErrorMessage = "PNLSalesRental";
                                if (Math.Round(PNLSalesPropertyRental) != Math.Round(CTIPNLSalesPropertyRental))
                                    hasErrorMessage = "PNLSalesPropertyRental";                                
                                if (Math.Round(PNLSalesOthers) != Math.Round(CTIPNLSalesOthers))
                                    hasErrorMessage = "PNLSalesOthers";
                                if (Math.Round(PNLInterestIncome) != Math.Round(CTIPNLInterestIncome))
                                    hasErrorMessage = "PNLInterestIncome";
                                if (Math.Round(PNLInterestExpense) != Math.Round(CTIPNLInterestExpense))
                                    hasErrorMessage = "PNLInterestExpense";
                                if (Math.Round(PNLCommissionIncome) != Math.Round(CTIPNLCommissionIncome))
                                    hasErrorMessage = "PNLCommissionIncome";
                                if (Math.Round(PNLPropertyRentalIncome) != Math.Round(CTIPNLPropertyRentalIncome))
                                    hasErrorMessage = "PNLPropertyRentalIncome";
                                if (Math.Round(PNLAdminMgmtFee) != Math.Round(CTIPNLAdminMgmtFee))
                                    hasErrorMessage = "PNLAdminMgmtFee";
                                if (Math.Round(PNLSNDRentalExpense) != Math.Round(CTIPNLSNDRentalExpense))
                                    hasErrorMessage = "PNLSNDRentalExpense";
                                if (Math.Round(PNLANGRentalExpense) != Math.Round(CTIPNLANGRentalExpense))
                                    hasErrorMessage = "PNLANGRentalExpense";
                                if (Math.Round(PNLSNDRecovery) != Math.Round(CTIPNLSNDRecovery))
                                    hasErrorMessage = "PNLSNDRecovery";
                                if (Math.Round(PNLANGRecovery) != Math.Round(CTIPNLANGRecovery))
                                    hasErrorMessage = "PNLANGRecovery";
                                if (Math.Round(PNLDividendIncome) != Math.Round(CTIPNLDividendIncome))
                                    hasErrorMessage = "PNLDividendIncome";
                                if (Math.Round(PNLDividendExpense) != Math.Round(CTIPNLDividendExpense))
                                    hasErrorMessage = "PNLDividendExpense";
                                if (Math.Round(DividendDeclared) != Math.Round(CTIDividendDeclared))
                                    hasErrorMessage = "DividendDeclared";

                                if (hasErrorMessage != "")
                                {
                                    hasError = true;
                                    break;
                                }
                            }
                            continue;
                        }

                        IList<Company> lstItem = new SystemDataActivity().RetrieveCompanyByCode(dsExcel.Tables[0].Rows[i]["Company"].ToString().Substring(0, 3));

                        if (lstItem != null)
                        {
                            if (lstItem.Count > 0)
                            {
                                Company company = (Company)lstItem[0];
                                LogSession sess = base.GetSessionInfo();

                                if (i == 1)
                                {
                                    // delete tbFinanceInterCompanyData_COA2016 DATA
                                    IDbConnection conn = new ConnectionManager().GetConnection();
                                    SqlDataReader rdr = null;
                                    StringBuilder sb = new StringBuilder();
                                    try
                                    {
                                        conn.Open();
                                        SqlCommand command = new SqlCommand("procAppDeleteInterCompanyData_COA2016", (SqlConnection)conn);
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = sess.CompanyId;
                                        command.Parameters.Add("@tbYear", SqlDbType.SmallInt).Value = this.year;
                                        command.Parameters.Add("@tbMonth", SqlDbType.SmallInt).Value = this.month;
                                        rdr = command.ExecuteReader();
                                    }
                                    catch (Exception ex)
                                    {
                                        Response.Output.Write("<SPAN STYLE='color: red'>Cannot delete old data. Error:" + ex.Message + ".</SPAN><br>");
                                        Response.Flush();
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
                                }

                                #region insert for each row
                                Double ExcelBSAssetCurrentTrade = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetCurrentTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetCurrentTrade"].ToString());
                                Double ExcelBSAssetCurrentLoan = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetCurrentLoan"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetCurrentLoan"].ToString());
                                Double ExcelBSAssetCurrentNonTrade = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetCurrentNonTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetCurrentNonTrade"].ToString());
                                Double ExcelBSAssetNonCurrent = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrent"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrent"].ToString());
                                Double ExcelBSAssetNonCurrentExclude = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrentExclude"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSAssetNonCurrentExclude"].ToString());
                                Double ExcelBSLiabilitiesAccruedPurchase = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesAccruedPurchase"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesAccruedPurchase"].ToString());
                                Double ExcelBSLiabilitiesCurrentTrade = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentTrade"].ToString());
                                Double ExcelBSLiabilitiesCurrentNonTrade = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentNonTrade"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentNonTrade"].ToString());
                                Double ExcelBSLiabilitiesCurrentLoan = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentLoan"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesCurrentLoan"].ToString());
                                Double ExcelBSLiabilitiesNonCurrent = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSLiabilitiesNonCurrent"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSLiabilitiesNonCurrent"].ToString());
                                Double ExcelBSEquityQuasi = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["BSEquityQuasi"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["BSEquityQuasi"].ToString());
                                Double ExcelPNLSalesGoods = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesGoods"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesGoods"].ToString());
                                Double ExcelPNLSalesCylinderRental = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesCylinderRental"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesCylinderRental"].ToString());
                                Double ExcelPNLSalesRental = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesEquipRental"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesEquipRental"].ToString());
                                Double ExcelPNLSalesPropertyRental = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesPropertyRental"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesPropertyRental"].ToString());
                                Double ExcelPNLSalesOthers = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSalesOthers"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSalesOthers"].ToString());
                                Double ExcelPNLInterestIncome = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLInterestIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLInterestIncome"].ToString());
                                Double ExcelPNLInterestExpense = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLInterestExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLInterestExpense"].ToString());
                                Double ExcelPNLCommissionIncome = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLCommissionIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLCommissionIncome"].ToString());
                                Double ExcelPNLPropertyRentalIncome = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLPropertyRentalIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLPropertyRentalIncome"].ToString());
                                Double ExcelPNLAdminMgmtFee = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLAdminMgmtFee"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLAdminMgmtFee"].ToString());
                                Double ExcelPNLSNDRentalExpense = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSNDRentalExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSNDRentalExpense"].ToString());
                                Double ExcelPNLANGRentalExpense = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLANGRentalExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLANGRentalExpense"].ToString());
                                Double ExcelPNLSNDRecovery = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLSNDRecovery"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLSNDRecovery"].ToString());
                                Double ExcelPNLANGRecovery = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLANGRecovery"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLANGRecovery"].ToString());
                                Double ExcelPNLDividendIncome = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLDividendIncome"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLDividendIncome"].ToString());
                                Double ExcelPNLDividendExpense = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["PNLDividendExpense"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["PNLDividendExpense"].ToString());
                                Double ExcelDividendDeclared = String.IsNullOrEmpty(dsExcel.Tables[0].Rows[i]["DividendDeclared"].ToString()) ? 0 : Double.Parse(dsExcel.Tables[0].Rows[i]["DividendDeclared"].ToString());

                                Response.Output.Write("Inserting intercompany transactions for " + company.Code + "' ...<br>");
                                Response.Flush();

                                if (sess.CompanyId == sess.UserId &&
                                      !(ExcelBSAssetCurrentTrade == 0 &&
                                        ExcelBSAssetCurrentLoan == 0 &&
                                        ExcelBSAssetCurrentNonTrade == 0 &&
                                        ExcelBSAssetNonCurrent == 0 &&
                                        ExcelBSAssetNonCurrentExclude == 0 &&
                                        ExcelBSLiabilitiesAccruedPurchase == 0 &&
                                        ExcelBSLiabilitiesCurrentTrade == 0 &&
                                        ExcelBSLiabilitiesCurrentNonTrade == 0 &&
                                        ExcelBSLiabilitiesCurrentLoan == 0 &&
                                        ExcelBSLiabilitiesNonCurrent == 0 &&
                                        ExcelBSEquityQuasi == 0 &&
                                        ExcelPNLSalesGoods == 0 &&
                                        ExcelPNLSalesCylinderRental == 0 &&
                                        ExcelPNLSalesRental == 0 &&
                                        ExcelPNLSalesPropertyRental == 0 &&
                                        ExcelPNLSalesOthers == 0 &&
                                        ExcelPNLInterestIncome == 0 &&
                                        ExcelPNLInterestExpense == 0 &&
                                        ExcelPNLCommissionIncome == 0 &&
                                        ExcelPNLPropertyRentalIncome == 0 &&
                                        ExcelPNLAdminMgmtFee == 0 &&
                                        ExcelPNLSNDRentalExpense == 0 &&
                                        ExcelPNLANGRentalExpense == 0 &&
                                        ExcelPNLSNDRecovery == 0 &&
                                        ExcelPNLANGRecovery == 0 &&
                                        ExcelPNLDividendIncome == 0 &&
                                        ExcelPNLDividendExpense == 0 &&
                                        ExcelDividendDeclared == 0))
                                {
                                    Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error: Cannot insert self-company at row " + (i + 1) + ".</SPAN><br>");
                                    Response.Flush();
                                    Response.Output.Write("<br>");
                                }
                                else
                                {
                                    if (sess.CompanyId == sess.UserId)
                                    {
                                        Response.Output.Write("Bypassing insertion for self-company.<br><br>");
                                        continue;
                                    }

                                    IDbConnection conn = new ConnectionManager().GetConnection();
                                    SqlDataReader rdr = null;
                                    StringBuilder sb = new StringBuilder();
                                    try
                                    {
                                        conn.Open();
                                        SqlCommand command = new SqlCommand("procAppInsertInterCompanyData_COA2016", (SqlConnection)conn);
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = sess.CompanyId;
                                        command.Parameters.Add("@tbYear", SqlDbType.SmallInt).Value = this.year;
                                        command.Parameters.Add("@tbMonth", SqlDbType.SmallInt).Value = this.month;
                                        command.Parameters.Add("@CounterCoyID", SqlDbType.SmallInt).Value = company.CoyID;
                                        command.Parameters.Add("@BSAssetCurrentTrade", SqlDbType.Float).Value = ExcelBSAssetCurrentTrade;
                                        command.Parameters.Add("@BSAssetCurrentLoan", SqlDbType.Float).Value = ExcelBSAssetCurrentLoan;
                                        command.Parameters.Add("@BSAssetCurrentNonTrade", SqlDbType.Float).Value = ExcelBSAssetCurrentNonTrade;
                                        command.Parameters.Add("@BSAssetNonCurrent", SqlDbType.Float).Value = ExcelBSAssetNonCurrent;
                                        command.Parameters.Add("@BSAssetNonCurrentExclude", SqlDbType.Float).Value = ExcelBSAssetNonCurrentExclude;
                                        command.Parameters.Add("@BSLiabilitiesAccruedPurchase", SqlDbType.Float).Value = ExcelBSLiabilitiesAccruedPurchase;
                                        command.Parameters.Add("@BSLiabilitiesCurrentTrade", SqlDbType.Float).Value = ExcelBSLiabilitiesCurrentTrade;
                                        command.Parameters.Add("@BSLiabilitiesCurrentNonTrade", SqlDbType.Float).Value = ExcelBSLiabilitiesCurrentNonTrade;
                                        command.Parameters.Add("@BSLiabilitiesCurrentLoan", SqlDbType.Float).Value = ExcelBSLiabilitiesCurrentLoan;
                                        command.Parameters.Add("@BSLiabilitiesNonCurrent", SqlDbType.Float).Value = ExcelBSLiabilitiesNonCurrent;
                                        command.Parameters.Add("@BSEquityQuasi", SqlDbType.Float).Value = ExcelBSEquityQuasi;
                                        command.Parameters.Add("@PNLSalesGoods", SqlDbType.Float).Value = ExcelPNLSalesGoods;
                                        command.Parameters.Add("@PNLSalesCylinderRental", SqlDbType.Float).Value = ExcelPNLSalesCylinderRental;
                                        command.Parameters.Add("@PNLSalesEquipRental", SqlDbType.Float).Value = ExcelPNLSalesRental;
                                        command.Parameters.Add("@PNLSalesPropertyRental", SqlDbType.Float).Value = ExcelPNLSalesPropertyRental;
                                        command.Parameters.Add("@PNLSalesOthers", SqlDbType.Float).Value = ExcelPNLSalesOthers;
                                        command.Parameters.Add("@PNLInterestIncome", SqlDbType.Float).Value = ExcelPNLInterestIncome;
                                        command.Parameters.Add("@PNLInterestExpense", SqlDbType.Float).Value = ExcelPNLInterestExpense;
                                        command.Parameters.Add("@PNLCommissionIncome", SqlDbType.Float).Value = ExcelPNLCommissionIncome;
                                        command.Parameters.Add("@PNLPropertyRentalIncome", SqlDbType.Float).Value = ExcelPNLPropertyRentalIncome;
                                        command.Parameters.Add("@PNLAdminMgmtFee", SqlDbType.Float).Value = ExcelPNLAdminMgmtFee;
                                        command.Parameters.Add("@PNLSNDRentalExpense", SqlDbType.Float).Value = ExcelPNLSNDRentalExpense;
                                        command.Parameters.Add("@PNLANGRentalExpense", SqlDbType.Float).Value = ExcelPNLANGRentalExpense;
                                        command.Parameters.Add("@PNLSNDRecovery", SqlDbType.Float).Value = ExcelPNLSNDRecovery;
                                        command.Parameters.Add("@PNLANGRecovery", SqlDbType.Float).Value = ExcelPNLANGRecovery;
                                        command.Parameters.Add("@PNLDividendIncome", SqlDbType.Float).Value = ExcelPNLDividendIncome;
                                        command.Parameters.Add("@PNLDividendExpense", SqlDbType.Float).Value = ExcelPNLDividendExpense;
                                        command.Parameters.Add("@DividendDeclared", SqlDbType.Float).Value = ExcelDividendDeclared;
                                        command.Parameters.Add("@CreatedBy", SqlDbType.SmallInt).Value = sess.UserId;
                                        rdr = command.ExecuteReader();

                                        Response.Output.Write("Inserting successful.<br>");
                                        Response.Flush();

                                    }
                                    catch (Exception ex)
                                    {
                                        Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error:" + ex.Message + "at row " + (i + 1) + ".</SPAN><br>");
                                        Response.Flush();
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
                                    Response.Output.Write("<br>");
                                }
                                #endregion
                            }
                            else
                            {
                                Response.Output.Write("<SPAN STYLE='color: red'>Company '" + dsExcel.Tables[0].Rows[i]["Company"].ToString() + "' cannot be found at row " + (i + 1) + ".</SPAN><br><br>");
                                Response.Flush();
                            }
                        }
                    }//end for
                    if (!hasError) tran.Complete();
                    else
                    {
                        Response.Output.Write("<SPAN STYLE='color: red'>Data is not tallied for column <b>" + hasErrorMessage + "</b>. Please check your data again. Rolling back to previous state ...</SPAN><br><br>");
                        Response.Flush();
                    }
                }//end transaction
                //delete Excel file
                File.Delete(excelFilePath);
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
