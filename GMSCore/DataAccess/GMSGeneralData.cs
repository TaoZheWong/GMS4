using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web;
using System.Diagnostics;

using GMSCore;
using System.Globalization;

namespace GMSCore
{
    /// <summary>
    /// Summary description for GMSGeneralDALC.
    /// </summary>
    public class GMSGeneralDALC
    {
        private ConnectionManager cm;

        //---------------------------------------------------------------------
        // public GMSGeneralDALC()
        //---------------------------------------------------------------------
        /// <summary>
        /// Default constructor. If the object is instantiated using the 
        /// default constructor, create a new connection. UI Components will 
        /// always use this constructor.
        /// </summary>
        //---------------------------------------------------------------------
        public GMSGeneralDALC()
        {
            cm = new ConnectionManager();
        }	//	end of GMSGeneralDALC()

        #region Methods for GMSGeneral
        /// <summary>
        /// Update All Employee To Inactive.
        /// </summary>
        public void UpdateAllEmployeeToInacive()
        {
        }

        /// <summary>
        /// Retrieve list of manager level access of a product.
        /// </summary>
        public void IsProductManager(short companyId, string productCode, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductManagerSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void IsProductManagerByProductGroupCode(short companyId, string productGroupCode, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductManagerSelectByProductGroupCode", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userId;
            command.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = productGroupCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a user's role.
        /// </summary>
        public void GetUserRoleByUserID(short companyId, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppUserRoleSelectByNumID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of products for auto complete
        /// </summary>
        public void GetAccountByNameForAutoComplete(short companyId, string accountName, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppAccountSelectByAccountNameForAutoComplete", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = "%" + accountName + "%";
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetEmployeeListWildcardSelect(short coyid, string employeeNo, 
            string name, string designation, string nric, string grade, bool isActive, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppEmployeeListWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyid;
            command.Parameters.Add("@EmployeeNo", SqlDbType.NVarChar).Value = employeeNo;
            command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = name;
            command.Parameters.Add("@Designation", SqlDbType.NVarChar).Value = designation;
            command.Parameters.Add("@NRIC", SqlDbType.NVarChar).Value = nric;
            command.Parameters.Add("@Grade", SqlDbType.NVarChar).Value = grade;
            command.Parameters.Add("@IsInactive", SqlDbType.Bit).Value = !isActive;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetEmployeeListByDepartmentWildcardSelect(short coyid, string employeeNo,
            string name, string designation, string nric, string grade, bool isActive, short userId, string userName, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppEmployeeListByDepartmentWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyid;
            command.Parameters.Add("@EmployeeNo", SqlDbType.NVarChar).Value = employeeNo;
            command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = name;
            command.Parameters.Add("@Designation", SqlDbType.NVarChar).Value = designation;
            command.Parameters.Add("@NRIC", SqlDbType.NVarChar).Value = nric;
            command.Parameters.Add("@Grade", SqlDbType.NVarChar).Value = grade;
            command.Parameters.Add("@IsInactive", SqlDbType.Bit).Value = !isActive;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userId;
            command.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = userName; 
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
       
        public void GetCountryListSelectUserNumID(short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCountryListSelectUserNumID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of products for auto complete
        /// </summary>
        public void GetCompanyListSelectUserNumID(short userId, short countryId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCompanyListSelectUserNumID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userId;
            command.Parameters.Add("@CountryID", SqlDbType.SmallInt).Value = countryId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        #endregion


        /// <summary>
        /// Retrieve a list of products for auto complete
        /// </summary>
        public void GetSalesPersonListSelect(short companyId, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectSalesPersonID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        public void GetSalesPersonRecord(short companyId, short Year, short Month, string GroupType, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectSalesPersonRecord", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@Year", SqlDbType.SmallInt).Value = Year;
            command.Parameters.Add("@Month", SqlDbType.SmallInt).Value = Month;
            command.Parameters.Add("@GroupType", SqlDbType.NVarChar).Value = GroupType;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        public void GetSalesPersonShared(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectSalesPersonShared", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        public void UpdateSalesPersonShared(short companyId, short SalesPersonTeamID, short SalesPersonMasterID, string GroupType, Decimal Ratio)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppUpdateSalesPersonShared", (SqlConnection)conn);


                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@SalesPersonTeamID", SqlDbType.SmallInt).Value = SalesPersonTeamID;
                command.Parameters.Add("@SalesPersonMasterID", SqlDbType.SmallInt).Value = SalesPersonMasterID;
                command.Parameters.Add("@GroupType", SqlDbType.NVarChar).Value = GroupType;
                command.Parameters.Add("@Ratio", SqlDbType.Decimal).Value = Ratio;
                rdr = command.ExecuteReader();
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

            return;
        }
        public void InsertSalesPersonShared(short companyId, short SalesPersonTeamID, short SalesPersonMasterID, string GroupType, Decimal Ratio)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppInsertSalesPersonShared", (SqlConnection)conn);


                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@SalesPersonTeamID", SqlDbType.SmallInt).Value = SalesPersonTeamID;
                command.Parameters.Add("@SalesPersonMasterID", SqlDbType.SmallInt).Value = SalesPersonMasterID;
                command.Parameters.Add("@GroupType", SqlDbType.NVarChar).Value = GroupType;
                command.Parameters.Add("@Ratio", SqlDbType.Decimal).Value = Ratio;
                rdr = command.ExecuteReader();
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

            return;
        }
        public void DeleteSalesPersonShared(short companyId, short SalesPersonTeamID, short SalesPersonMasterID)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppDeleteSalesPersonShared", (SqlConnection)conn);


                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@SalesPersonTeamID", SqlDbType.SmallInt).Value = SalesPersonTeamID;
                command.Parameters.Add("@SalesPersonMasterID", SqlDbType.SmallInt).Value = SalesPersonMasterID;
                rdr = command.ExecuteReader();
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

            return;
        }
        /// <summary>
        /// Retrieve a list of Distinct MajorBankAccountExcludeCurrentBank
        /// </summary>
        public void GetDistinctMajorBankAccountSelect(short companyId, short bankId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectDistinctMajorBank", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@BankID", SqlDbType.SmallInt).Value = bankId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of Distinct MajorBankAccountExcludeCurrentBank
        /// </summary>
        public void GetBankSelectByFirstTwoDigitsCOA(string bankcoa, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectBankByFirstTwoDigitsCOA", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@BankCOA", SqlDbType.NVarChar).Value = bankcoa;            
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        /// <summary>
        /// Retrieve a list of Distinct MajorBankAccountExcludeCurrentBank
        /// </summary>
        public void UpdateMajorBankAccount(short companyId, short bankId, bool isMajorBank)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppBankAccountIsMajorBankUpdate", (SqlConnection)conn);
                

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@BankID", SqlDbType.SmallInt).Value = bankId;
                command.Parameters.Add("@isMajorBank", SqlDbType.Bit).Value = isMajorBank;
                rdr = command.ExecuteReader();
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
            
            return;
        }

        public void ImportBankUtilisationData(short companyId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppImportBankUtilisationData", (SqlConnection)conn);         
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;                
                rdr = command.ExecuteReader();
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
            
            return;
        }

        /// <summary>
        /// Retrieve a list of Bank Account
        /// </summary>
        public void GetListBankAccountSelect(short companyId,string bankcoa, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectListBankAccount", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@BankCOA", SqlDbType.NVarChar).Value = bankcoa;
            //command.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = currency;             
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of Bank Account
        /// </summary>
        public void GetBankID(string bankNumericCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectBank", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@bankNumericCode", SqlDbType.NVarChar).Value = bankNumericCode;           
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        /// <summary>
        /// Remove BankFacility And BankUtilisation DeleteBankFacilityAndBankUtilisation
        /// </summary>
        public void DeleteBankFacilityAndBankUtilisation(short companyId, string strBankListInA21)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppBankFacilityAndBankUtilisationDelete", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@strBankListInA21", SqlDbType.NVarChar).Value = strBankListInA21;                
                rdr = command.ExecuteReader();
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

            return;
        }

        /// <summary>
        /// Retrieve a list of 
        /// </summary>
        public void GetCashFlowProjectionAsOf(short companyId, short year, short month, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCashFlowProjectionForWeekSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@Year", SqlDbType.NVarChar).Value = year;
            command.Parameters.Add("@Month", SqlDbType.NVarChar).Value = month;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

       
        /// <summary>
        /// Retrieve a list of CashFlowProjection
        /// </summary>
        public void GetCashFlowProjection(short companyId, short year, short week, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectCashFlowProjection", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;            
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@Year", SqlDbType.NVarChar).Value = year;
            command.Parameters.Add("@Week", SqlDbType.NVarChar).Value = week;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Delete Cash Flow Proj As Of
        /// </summary>
        public void DeleteCashFlowProjAsOf(short companyId, short year, short month)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppDeleteCashFlowProjAsOf", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@Year", SqlDbType.NVarChar).Value = year;
                command.Parameters.Add("@Month", SqlDbType.NVarChar).Value = month;
                rdr = command.ExecuteReader();

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

            return;
        }



        /// <summary>
        /// Delete CashFlowProjData
        /// </summary>
        public void DeleteCashFlowProj(short companyId, short year, short week)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppCashFlowProjDelete", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@Year", SqlDbType.NVarChar).Value = year;
                command.Parameters.Add("@Week", SqlDbType.NVarChar).Value = week;
                rdr = command.ExecuteReader();
               
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

            return;
        }

        

        /// <summary>
        /// Insert CashFlowProjection
        /// </summary>
        public void InsertCashFlowProjection(short companyId, short year, short month, short week, 
            double CollectionFromSales, double OtherIncome,double TotalCashInflow,double PaymentToOverseasSupplier,double PaymentToLocalSupplier,
            double SalesmanClaim, double SalaryPayment, double OtherPayment, double TaxsPayment, double TotalOperatingExpenses,
            double NetOperatingCashFlow, double PurchaseofFixedAssets, double Investments, double DisposalOfFixedAssets, double DisposalOfInvestmentsOthers,
            double LoanToIntercompany, double InterestReceived, double DividendReceived, double DividendPaid, double NetCashFlowFromInvesting,
            double ProceedsOfBankLoans, double RepaymentOfBankLoans, double RepaymentOfTradeFinancing, double PaymentOfInterests, double NewCapitalConvertibleLoan,
            double LoanFromIntercompany, double RepaymentOfIntercompanyLoan, double NetCashFlowFromFinancing, double NetCashFlowSurplusDeficit, 
            int userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppInsertCashFlowProjection", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@Year", SqlDbType.NVarChar).Value = year;
            command.Parameters.Add("@Month", SqlDbType.NVarChar).Value = month;
            command.Parameters.Add("@Week", SqlDbType.NVarChar).Value = week;
            command.Parameters.Add("@CollectionFromSales", SqlDbType.NVarChar).Value = CollectionFromSales;
            command.Parameters.Add("@OtherIncome", SqlDbType.NVarChar).Value = OtherIncome;
            command.Parameters.Add("@TotalCashInflow", SqlDbType.NVarChar).Value = TotalCashInflow;
            command.Parameters.Add("@PaymentToOverseasSupplier", SqlDbType.NVarChar).Value = PaymentToOverseasSupplier;
            command.Parameters.Add("@PaymentToLocalSupplier", SqlDbType.NVarChar).Value = PaymentToLocalSupplier;
            command.Parameters.Add("@SalesmanClaim", SqlDbType.NVarChar).Value = SalesmanClaim;
            command.Parameters.Add("@SalaryPayment", SqlDbType.NVarChar).Value = SalaryPayment;
            command.Parameters.Add("@OtherPayment", SqlDbType.NVarChar).Value = OtherPayment;
            command.Parameters.Add("@TaxsPayment", SqlDbType.NVarChar).Value = TaxsPayment;
            command.Parameters.Add("@TotalOperatingExpenses", SqlDbType.NVarChar).Value = TotalOperatingExpenses;
            command.Parameters.Add("@NetOperatingCashFlow", SqlDbType.NVarChar).Value = NetOperatingCashFlow;
            command.Parameters.Add("@PurchaseofFixedAssets", SqlDbType.NVarChar).Value = PurchaseofFixedAssets;
            command.Parameters.Add("@Investments", SqlDbType.NVarChar).Value = Investments;
            command.Parameters.Add("@DisposalOfFixedAssets", SqlDbType.NVarChar).Value = DisposalOfFixedAssets;
            command.Parameters.Add("@DisposalOfInvestmentsOthers", SqlDbType.NVarChar).Value = DisposalOfInvestmentsOthers;
            command.Parameters.Add("@LoanToIntercompany", SqlDbType.NVarChar).Value = LoanToIntercompany;
            command.Parameters.Add("@InterestReceived", SqlDbType.NVarChar).Value = InterestReceived;
            command.Parameters.Add("@DividendReceived", SqlDbType.NVarChar).Value = DividendReceived;
            command.Parameters.Add("@DividendPaid", SqlDbType.NVarChar).Value = DividendPaid;
            command.Parameters.Add("@NetCashFlowFromInvesting", SqlDbType.NVarChar).Value = NetCashFlowFromInvesting;
            command.Parameters.Add("@ProceedsOfBankLoans", SqlDbType.NVarChar).Value = ProceedsOfBankLoans;
            command.Parameters.Add("@RepaymentOfBankLoans", SqlDbType.NVarChar).Value = RepaymentOfBankLoans;
            command.Parameters.Add("@RepaymentOfTradeFinancing", SqlDbType.NVarChar).Value = RepaymentOfTradeFinancing;
            command.Parameters.Add("@PaymentOfInterests", SqlDbType.NVarChar).Value = PaymentOfInterests;
            command.Parameters.Add("@NewCapitalConvertibleLoan", SqlDbType.NVarChar).Value = NewCapitalConvertibleLoan;
            command.Parameters.Add("@LoanFromIntercompany", SqlDbType.NVarChar).Value = LoanFromIntercompany;
            command.Parameters.Add("@RepaymentOfIntercompanyLoan", SqlDbType.NVarChar).Value = RepaymentOfIntercompanyLoan;
            command.Parameters.Add("@NetCashFlowFromFinancing", SqlDbType.NVarChar).Value = NetCashFlowFromFinancing;
            command.Parameters.Add("@NetCashFlowSurplusDeficit", SqlDbType.NVarChar).Value = NetCashFlowSurplusDeficit;
            command.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = userid;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of ProjectID
        /// </summary>
        public void GetCompanyProject(short companyId, short userid, short reportid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectCompanyProject", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumId", SqlDbType.NVarChar).Value = userid;
            command.Parameters.Add("@ReportId", SqlDbType.NVarChar).Value = reportid;
            
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        public void GetCompanyProject(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectCompanyProjectByCompanyID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCompanyDepartment(short companyId, short ProjectID, short ReportID, short UserID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectCompanyDepartment", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProjectID", SqlDbType.SmallInt).Value = ProjectID;
            command.Parameters.Add("@ReportID", SqlDbType.SmallInt).Value = ReportID;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = UserID;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCompanyDepartment(short companyId, short ProjectID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectCompanyDepartmentByCoyIDAndProjectID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProjectID", SqlDbType.SmallInt).Value = ProjectID;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetDepartmentByDivision(short companyId, short ProjectID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectDepartmentByDivision", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProjectID", SqlDbType.SmallInt).Value = ProjectID;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        public void GetCompanySection(short companyId, short DepartmentID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectCompanySection", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@DepartmentID", SqlDbType.SmallInt).Value = DepartmentID;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        public void GetCompanyUnit(short companyId, short SectionID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectCompanyUnit", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@SectionID", SqlDbType.SmallInt).Value = SectionID;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        public void GetSalesPersonDetailsSelect(short companyId, short UserId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppQuotationEmailSalesPersonSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.NVarChar).Value = UserId;                       
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetAccountAttachmentSelect(short companyId, string accountcode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppAccountAttachmentSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountcode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetFinanceAttachmentSelect(string accountname, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppFinanceAttachmentSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountname;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetSalesmanAttachmentSelect(short companyId, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSalesmanAttachmentSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.NVarChar).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetOutstandingPayementsSelect(short companyId, short days, short userId, string accountcode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppOutstandingPayments", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@Days", SqlDbType.SmallInt).Value = days;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountcode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCollectionsSelect(short companyId, short userId, string accountcode, DateTime dateFrom, DateTime dateTo, string invoiceNo, string receiptNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCollections", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;            
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountcode;
            command.Parameters.Add("@ReceiptDateFrom", SqlDbType.DateTime).Value = dateFrom;
            command.Parameters.Add("@ReceiptDateTo", SqlDbType.DateTime).Value = dateTo;
            command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = invoiceNo;
            command.Parameters.Add("@ReceiptNo", SqlDbType.NVarChar).Value = receiptNo;            

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetPurchaseSelect(short companyId, string accountcode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppAccountPurchasesSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;            
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountcode;            

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetContactsSelect(short companyId, string accountcode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppAccountContactsSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountcode;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCommunicationSelect(short companyId, string accountcode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppAccountCommunicationSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountcode;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCommunicationCommentSelect(short companyId, string accountcode, short commID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppAccountCommunicationCommentSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountcode;
            command.Parameters.Add("@CommID", SqlDbType.SmallInt).Value = commID;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void UpdateProspectAccountCode(short companyId, string oldAccountCode, string newAccountCode)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppProspectAccountCodeUpdate", (SqlConnection)conn);


                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@OldAccountCode", SqlDbType.NChar).Value = oldAccountCode;
                command.Parameters.Add("@NewAccountCode", SqlDbType.NChar).Value = newAccountCode;
                rdr = command.ExecuteReader();
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

            return;
        }



        /// <summary>
        /// Retrieve a list of products for auto complete
        /// </summary>
        public void GetSalutationSelect(ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSalutationSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;            
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        /// <summary>
        /// Retrieve current year rate created date.
        /// </summary>
        public void GetAllCurrentYearCreatedDateSelect(ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCurrentYearCreatedDateSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;            
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCurrentYearMonthCreatedDateSelect(short year, short month ,ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCurrentYearMonthCreatedDateSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Year", SqlDbType.NVarChar).Value = year;
            command.Parameters.Add("@Month", SqlDbType.NVarChar).Value = month;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void UpdateIDRRate(short year, short month, double rate, short modifiedby)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppIDRRateUpdate", (SqlConnection)conn);


                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Year", SqlDbType.NVarChar).Value = year;
                command.Parameters.Add("@Month", SqlDbType.NVarChar).Value = month;
                command.Parameters.Add("@IDRRate", SqlDbType.NVarChar).Value = rate;
                command.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar).Value = modifiedby;
                rdr = command.ExecuteReader();
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

            return;
        }

        /// <summary>
        /// Retrieve a list of Company Name
        /// </summary>
        public void GetCompanyName(ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectCompanyName", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;          

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCompany(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectCompany", (SqlConnection)conn);
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        //Added By Kim on 15 Apr 2013
        /// <summary>
        /// Retrieve list of ALL SalesPersonAndProductManager by usernumid.
        /// </summary>
        public void GetSalesPersonAndProductManagerRecordsByUserNumIDCoyID(short companyId, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSalesPersonAndProductManagerByUserNumIDCoyIDSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        //Added By Kim on 15 Apr 2013
        /// <summary>
        /// Retrieve list of ALL ProductHeadAndProductManager by usernumid.
        /// </summary>
        public void GetProductHeadAndProductManagerRecordsByUserNumIDCoyID(short companyId, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductHeadAndProductManagerByUserNumIDCoyIDSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        //Added By Kim on 15 Apr 2013
        /// <summary>
        /// Retrieve list of ALL GetMRUserAccessByUserNumIDCoyID by usernumid.
        /// </summary>
        public void GetMRUserAccessByUserNumIDCoyID(short companyId, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRUserAccessByUserNumIDCoyIDSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        //Added By Kim on 15 Apr 2013
        /// <summary>
        /// Retrieve MR UserRole
        /// </summary>
        public void GetMRUserRoleByUserNumIDCoyID(short companyId, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRUserRoleByUserNumIDCoyIDSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        //Added By Kim on 15 Apr 2013
        /// <summary>
        /// Retrieve MR UserRole
        /// </summary>
        public void GetAlternatePartyByAction(short companyId, short userId, string action ,ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppUserAlternatePartySelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@Action", SqlDbType.NChar).Value = action;
            
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        //Added By Kim on 15 Apr 2013
        /// <summary>
        /// Retrieve MR UserRole
        /// </summary>
        public void GetMRApprovalUserByCoyID(short companyId, string userName, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRApprovalUserSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserName", SqlDbType.NChar).Value = userName;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }



        //Added By Kim on 09 July 2013
        /// <summary>
        /// Retrieve Access To View PO or GRN
        /// </summary>
        public void GetAccessToViewPOAndGRN(short companyId, string mrNo, short userId, string supplierInfo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppAccessToViewPOAndGRNSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrNo;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@SupplierInfo", SqlDbType.NVarChar).Value = supplierInfo;
            
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        //Added By Kim on 07 Jun 2013
        /// <summary>
        /// Retrieve list of Warehouse by CompanyID.
        /// </summary>
        public void GetMRWareHouseByCoyID(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRWareHouseByCoyIDSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;           
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        //Added By Kim on 07 Jun 2013
        /// <summary>
        /// Retrieve Non-Zero product by mrno.
        /// </summary>
        public void GetNonZeroProductByMRNo(short companyId, string mrNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRNonPMProductByMRNoSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        //Added By Kim on 07 Jun 2013
        /// <summary>
        /// Retrieve ProductGroupCode by mrno.
        /// </summary>
        public void GetProductGroupCodeByMRNo(short companyId, string mrNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRProductGroupCodeByMRNoSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }



        /// <summary>
        /// Retrieve list of ALL MaterialRequisition requestor by usernumid.
        /// </summary>
        public void GetMaterialRequisitionRequestorByUserNumIDCoyID(short companyId, short userId, string MRScheme ,ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMaterialRequisitionRequestorByUserNumIDCoyIDSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@MRScheme", SqlDbType.NVarChar).Value = MRScheme;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of ALL MaterialRequisition PM PH and requestor by coyid.
        /// </summary>
        public void GetMaterialRequisitionPMPHRequestorByCoyID(short companyId, short userId, string MRScheme , ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMaterialRequisitionPMPHRequestorByCoyIDSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@MRScheme", SqlDbType.NVarChar).Value = MRScheme;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of ALL MaterialRequisition PM PH and requestor by coyid.
        /// </summary>
        public void GetMaterialRequisitionPMPHPH3ByCoyID(short companyId, short pm, short ph, short ph3, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMaterialRequisitionPMPHPH3ByCoyIDSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@PM", SqlDbType.Int).Value = pm;
            command.Parameters.Add("@PH", SqlDbType.Int).Value = ph;
            command.Parameters.Add("@PH3", SqlDbType.Int).Value = ph3;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetNextApproverByLevel(short companyId, short userId, string MRScheme, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMaterialRequisitionNextApproverSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@MRScheme", SqlDbType.NVarChar).Value = MRScheme;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Material Requisition select
        /// </summary>
        public void GetMaterialRequisition(short companyId, short userId, DateTime dateFrom, DateTime dateTo, string status, string accountCode,
                                        string accountName, string productCode, string productName, string productGroup, string productGroupName, string PMUserID, string requestor, string role, string mrNo, string vendor, string poNo, string purchaser, string refNo, string budgetCode, string projectNo, string requestorRemarks, string MRScheme ,ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMaterialRequisitionsWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = dateFrom;
            command.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = dateTo;
            command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = status;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            command.Parameters.Add("@ProductGroup", SqlDbType.NVarChar).Value = productGroup;
            command.Parameters.Add("@ProductGroupName", SqlDbType.NVarChar).Value = productGroupName;
            command.Parameters.Add("@PMUserID", SqlDbType.NVarChar).Value = PMUserID;
            command.Parameters.Add("@Requestor", SqlDbType.NVarChar).Value = requestor;
            command.Parameters.Add("@Role", SqlDbType.NVarChar).Value = role;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrNo;
            command.Parameters.Add("@Vendor", SqlDbType.NVarChar).Value = vendor;
            command.Parameters.Add("@PONo", SqlDbType.NVarChar).Value = poNo;
            command.Parameters.Add("@Purchaser", SqlDbType.NVarChar).Value = purchaser;
            command.Parameters.Add("@BudgetCode", SqlDbType.NVarChar).Value = budgetCode;
            command.Parameters.Add("@RefNo", SqlDbType.NVarChar).Value = refNo;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = projectNo;
            command.Parameters.Add("@RequestorRemarks", SqlDbType.NVarChar).Value = requestorRemarks;
            command.Parameters.Add("@MRScheme", SqlDbType.NVarChar).Value = MRScheme;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Material Requisition By MR No. select
        /// </summary>
        public void GetMaterialRequisitionByMRNo(short companyId, string mrNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMaterialRequisitionsByMRNoSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrNo;            
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// CanUserAccessDocument
        /// </summary>
        public void CanUserAccessDocument(short companyId, string documentType, string documentNo, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCheckUserAccessDocument", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@DocumentType", SqlDbType.NVarChar).Value = documentType;
            command.Parameters.Add("@DocumentNo", SqlDbType.NVarChar).Value = documentNo;
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
            
        }

        /// <summary>
        /// Retrieve list of GetExistPMPHByPHPMIDCoyID.
        /// </summary>
        public void GetExistPMPHByPHPMCoyID(short companyId, short pm, short ph, short ph3, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMaterialRequisitionExistPMPHByPHPMCoyIDSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@PM", SqlDbType.SmallInt).Value = pm;
            command.Parameters.Add("@PH", SqlDbType.SmallInt).Value = ph;
            command.Parameters.Add("@PH3", SqlDbType.SmallInt).Value = ph3;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void ImportNewProdFromA21(short companyId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppImportNewProdCode", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                rdr = command.ExecuteReader();
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

            return;
        }

        public void RemoveMSTProduct(short companyId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppRemoveMSTProduct", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                rdr = command.ExecuteReader();
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

            return;
        }

        public void procMapNewProdCode(short companyId, string mrNo)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppMapNewProdCode", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrNo;
                rdr = command.ExecuteReader();
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

            return;
        }


        public void GetCheckExistNewProdCodeForMapping(short companyId, string mrNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppExistNewProdCodeForMappingSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCheckExistCustomerPO(short companyId, string mrNo, string custpo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppExistCustomerPOSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrNo;
            command.Parameters.Add("@CustomerPONo", SqlDbType.NVarChar).Value = custpo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void ConvertSellingPriceAndPurchasePrice(short companyId, double sellingPrice, double purchasePrice, string sellingPriceCurrency, string purchasePriceCurrency, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppConvertPurchasePriceAndSellingPrice", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@SellingPrice", SqlDbType.Float).Value = sellingPrice;
            command.Parameters.Add("@PurchasePrice", SqlDbType.Float).Value = purchasePrice;
            command.Parameters.Add("@SellingPriceCurrency", SqlDbType.NVarChar).Value = sellingPriceCurrency;
            command.Parameters.Add("@PurchasePriceCurrency", SqlDbType.NVarChar).Value = purchasePriceCurrency;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetProjectPurchasePrice(short companyId, string projectNo, string purchasePriceCurrency, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppPurchasePriceByProjectNo", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = projectNo;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = purchasePriceCurrency;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        public void GetCountryByCoy(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCountryByCoySelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;          
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        //Added By Kim on 15 July 2013
        /// <summary>
        /// Retrieve Product Purchase Price
        /// </summary>
        public void GetProductPurchasePrice(short companyId, string prodCode, short UserId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductPurchasePriceSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = prodCode;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = UserId;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve ETA ETD CRD for Product
        public void GetProductDeliveryInfo(short companyId, string poNo, string prodCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductDeliveryInfoSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@PONo", SqlDbType.NVarChar).Value = poNo;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = prodCode;
           
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void UpdateAccountAttachment(string fileName)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppAccountAttachmentUpdate", (SqlConnection)conn);


                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@FileName", SqlDbType.NChar).Value = fileName;
                rdr = command.ExecuteReader();
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

            return;
        }

        public void procUpdateBudgetSummary(short companyId, int FYE, short projectId, short departmentId, short sectionId, short unitId, short year)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppUpdateBudgetSummary", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@FYE", SqlDbType.Int).Value = FYE;
                command.Parameters.Add("@ProjectID", SqlDbType.SmallInt).Value = projectId;
                command.Parameters.Add("@DepartmentID", SqlDbType.SmallInt).Value = departmentId;
                command.Parameters.Add("@SectionID", SqlDbType.SmallInt).Value = sectionId;
                command.Parameters.Add("@UnitID", SqlDbType.SmallInt).Value = unitId;
                command.Parameters.Add("@BudgetYear", SqlDbType.SmallInt).Value = year;
                command.CommandTimeout = 10000;
                rdr = command.ExecuteReader();
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

            return;
        }



        //Added By Kim on 19 Sept 2013
        /// <summary>
        /// ComputePerformanceIndicator
        /// </summary>
        public void procUpdateBudgetPerformanceIndicators(short companyId, int FYE, short projectId, short departmentId, short sectionId, short unitId ,short year)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppUpdateBudgetPerformanceIndicators", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@FYE", SqlDbType.Int).Value = FYE;
                command.Parameters.Add("@ProjectID", SqlDbType.SmallInt).Value = projectId;
                command.Parameters.Add("@DepartmentID", SqlDbType.SmallInt).Value = departmentId;
                command.Parameters.Add("@SectionID", SqlDbType.SmallInt).Value = sectionId;
                command.Parameters.Add("@UnitID", SqlDbType.SmallInt).Value = unitId;
                command.Parameters.Add("@BudgetYear", SqlDbType.SmallInt).Value = year;
                command.CommandTimeout = 10000;
                rdr = command.ExecuteReader();
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

            return;
        }

        ///Added By Kim on 23 Sept 2013
        /// <summary>
        /// Remove ApprovalForm records
        /// </summary>
        public void DeleteMRFormApprovalByLevel(short companyId, string mrno, short level, short levelId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppMRFormApprovalByLevelDelete", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrno;
                command.Parameters.Add("@Level", SqlDbType.SmallInt).Value = level;
                command.Parameters.Add("@LevelID", SqlDbType.SmallInt).Value = levelId;        
                rdr = command.ExecuteReader();
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

            return;
        }

        ///Added By Kim on 23 Sept 2013
        /// <summary>
        /// UpdateByPassMRFormApprovalByLevel
        /// </summary>
        public void UpdateByPassMRFormApprovalByLevel(short companyId, string mrno, short level, short levelId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppByPassMRFormApprovalByLevelUpdate", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrno;
                command.Parameters.Add("@Level", SqlDbType.SmallInt).Value = level;
                command.Parameters.Add("@LevelID", SqlDbType.SmallInt).Value = levelId;
                rdr = command.ExecuteReader();
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

            return;
        }

        /// <summary>
        /// Retrieve a list of Bank Account
        /// </summary>
        public void GetBankAccountFromA21(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppGetBankAccountFromA21", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;                        
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of Bank Account
        /// </summary>
        public void GetBankAccountCOABalanceFromA21(short companyId, string bankCOA, string currency, string type, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppGetBankAccountCOABalanceFromA21", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@BankCOA", SqlDbType.NVarChar).Value = bankCOA;
            command.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = currency;
            command.Parameters.Add("@Type", SqlDbType.NVarChar).Value = type;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }



        public void InsertMREmail(short companyId, string mrno, string subject, string purpose, string content)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppMREmailInsert", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrno;
                command.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = subject;
                command.Parameters.Add("@Purpose", SqlDbType.NVarChar).Value = purpose;
                command.Parameters.Add("@Content", SqlDbType.NVarChar).Value = content;
                //command.Parameters.Add("@ApproverUserID", SqlDbType.SmallInt).Value = approverUserId;
                rdr = command.ExecuteReader();
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

            return;
        }

        /// <summary>
        /// GetListOfMRRequiresApprovalByUserNumId
        /// </summary>
        public void GetListOfMRRequiresApprovalByUserNumId(short companyId, short userNumId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppListOfMRRequiresApprovalByUserIdSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userNumId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        /// <summary>
        /// GetListOfMRRequiresProductManagerApprovalByUserNumId
        /// </summary>
        public void GetListOfMRRequiresProductManagerApprovalByUserNumId(short companyId, short userNumId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppListOfMRRequiresProductManagersApprovalByUserIdSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userNumId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// GetListOfMRRequiresToFillInETDByUserNumId
        /// </summary>
        public void GetListOfMRWithoutETDInfoByUserNumId(short companyId, short userNumId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppListOfMRWithoutETDInfoByUserIdSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userNumId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        /// <summary>
        /// GetListOfMRRequiresApprovalByUserNumId
        /// </summary>
        public void GetListOfRejectedOrCancelledMRByUserNumId(short companyId, short userNumId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppListOfRejectedOrCancelledMRByUserIdSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userNumId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// GetListOfMRFailedDeliveryDateByUserNumId
        /// </summary>
        public void GetListOfMRFailedDeliveryDateByUserNumId(short companyId, short userNumId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppListOfMRFailedDeliveryDateByUserIdSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userNumId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// GetMRSupplierCodeByCoyIDAndProductCode
        /// </summary>
        public void GetMRSupplierCodeByCoyIDAndProductCode(short companyId, string productCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRSupplierCodeByCoyIDAndProductCodeSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        //Added By Kim on 12 5 2015
        /// <summary>
        /// Retrieve Product Info
        /// </summary>
        public void GetProductInfoByProductCode(short companyId, string productCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductInfoByProductCodeSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void UpdateProductInfoByProductCode(short companyId, string productCode, string remarks, short userNumId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppProductInfoByProductCodeUpdate", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
                command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = remarks;
                command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userNumId;
               
                rdr = command.ExecuteReader();
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

            return;
        }

        public void InsertProductPrice(short companyId, string ProductCode, string Country, float WeigthedCost, float DealerPrice,
            float UserPrice, float RetailPrice, int userid, string Remarks)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppProductPriceInsert", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = ProductCode;
                command.Parameters.Add("@Country", SqlDbType.NVarChar).Value = Country;
                command.Parameters.Add("@WeigthedCost", SqlDbType.Float).Value = WeigthedCost;
                command.Parameters.Add("@DealerPrice", SqlDbType.Float).Value = DealerPrice;
                command.Parameters.Add("@UserPrice", SqlDbType.Float).Value = UserPrice;
                command.Parameters.Add("@RetailPrice", SqlDbType.Float).Value = RetailPrice;
                command.Parameters.Add("@UpdatedBy", SqlDbType.SmallInt).Value = userid;
                command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;

                rdr = command.ExecuteReader();
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

            return;
        }
        public void GetProductPriceByProductCode(short companyId, string productCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductPriceByProductCodeSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        public void GetProductPriceByUser(short companyId, short UserNumID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductPriceByUser", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumId", SqlDbType.NVarChar).Value = UserNumID;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        public void GetProductLocationByProductCode(short companyId, string productCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductLocationByProductCodeSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void InsertProductLocationByProductCode(short companyId, string productCode, int PLID, string Location, short userNumId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppProductLocationByProductCodeInsert", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
                command.Parameters.Add("@PLID", SqlDbType.Int).Value = PLID;
                command.Parameters.Add("@Location", SqlDbType.NVarChar).Value = Location;
                command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userNumId;

                rdr = command.ExecuteReader();
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

            return;
        }
        public void DeleteProductLocationByProductCode(short companyId, string productCode, int PLID)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppProductLocationByProductCodeDelete", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
                command.Parameters.Add("@PLID", SqlDbType.Int).Value = PLID;

                rdr = command.ExecuteReader();
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

            return;
        }
        public void UpdateMRDetailProdCode(short companyId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppMRDetailProdCodeUpdate", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                rdr = command.ExecuteReader();
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

            return;
        }

        public void GetGRNForEmailNotification(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppGRNForEmailNotificationSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
           
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void UpdateProductFromScheduledTaskProduct()
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppProductFromScheduledTaskProductUpdate", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure; 
                rdr = command.ExecuteReader();
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

            return;
        }

        public void DeleteProductDescFromScheduledTaskProductDescription(short companyId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppDeleteScheduledTaskProductDescription", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                rdr = command.ExecuteReader();

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

            return;
        }

        public void UpdateGRNDetailFromScheduledTaskGRNDetail()
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppGRNDetailFromScheduledTaskGRNDetailUpdate", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                rdr = command.ExecuteReader();
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

            return;
        }

        public void DeleteCostCenterForMonth(short companyId, short year, short month)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppCostCenterForMonthDelete", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@Year", SqlDbType.SmallInt).Value = year;
                command.Parameters.Add("@Month", SqlDbType.SmallInt).Value = month;            
                rdr = command.ExecuteReader();


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

            return;

        }


        public void InsertCostCenterForMonth(short companyId, short year, short month, string costCentre, string itemName, decimal amount)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppCostCenterForMonthInsert", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@Year", SqlDbType.SmallInt).Value = year;
                command.Parameters.Add("@Month", SqlDbType.SmallInt).Value = month;
                command.Parameters.Add("@CostCentre", SqlDbType.NVarChar).Value = costCentre;
                command.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = itemName;               
                command.Parameters.Add("@Amount", SqlDbType.Float).Value = amount;
                rdr = command.ExecuteReader();


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

            return;

        }

        public DateTime GetNewCOADate(short companyId)
        {
            DateTime dt;
                try
                {
                    IDbConnection conn = cm.GetConnection();
                    SqlCommand command = new SqlCommand("procAppGetNewCOADate", (SqlConnection)conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                    command.Parameters.Add("@dt", SqlDbType.DateTime).Direction = ParameterDirection.Output;
                    conn.Open();
                    command.ExecuteScalar();
                    dt = Convert.ToDateTime(command.Parameters["@dt"].Value);
                    conn.Close();
                    conn.Dispose();
                }
                catch
                {
                    return DateTime.Now;
                }
            return dt;
        }

        public void GetHRFinanceItem(short companyId, string type, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppHRFinanceItemSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@Type", SqlDbType.NVarChar).Value = type;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetHRFinanceMapping(short companyId, string type, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppHRFinanceMappingSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@Type", SqlDbType.NVarChar).Value = type;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        public void InsertRecipe(short companyId, string RecipeNo, string AccountCode, DateTime RecipeDate, string MixtureType, string MolecularUnit, decimal CylinderCapacity,
            decimal Temperature, decimal RequiredPressure, bool IsStandardLiquidContent, decimal LiquidContent, string TopPressure, string CertificationType, string ValveConnection,
            string ValveConnectionType, short ShelfLife, string SpecialRequirement, decimal GasContent, decimal Pressure, decimal GasPrice, short MinLeadTime, short MaxLeadTime, short TotalComponent,
            string CylinderTypeID, string Remarks)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppRecipeInsert", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@RecipeNo", SqlDbType.NVarChar).Value = RecipeNo;
                command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = AccountCode;
                command.Parameters.Add("@RecipeDate", SqlDbType.SmallDateTime).Value = RecipeDate;
                command.Parameters.Add("@MixtureType", SqlDbType.NVarChar).Value = MixtureType;
                command.Parameters.Add("@MolecularUnit", SqlDbType.NVarChar).Value = MolecularUnit;
                command.Parameters.Add("@CylinderCapacity", SqlDbType.Float).Value = CylinderCapacity;
                command.Parameters.Add("@Temperature", SqlDbType.Float).Value = Temperature;
                command.Parameters.Add("@RequiredPressure", SqlDbType.Float).Value = RequiredPressure;
                command.Parameters.Add("@IsStandardLiquidContent", SqlDbType.Bit).Value = IsStandardLiquidContent;
                command.Parameters.Add("@LiquidContent", SqlDbType.Float).Value = LiquidContent;
                command.Parameters.Add("@TopPressure", SqlDbType.NVarChar).Value = TopPressure;
                command.Parameters.Add("@CertificationType", SqlDbType.NVarChar).Value = CertificationType;
                command.Parameters.Add("@ValveConnection", SqlDbType.NVarChar).Value = ValveConnection;
                command.Parameters.Add("@ValveConnectionType", SqlDbType.NVarChar).Value = ValveConnectionType;
                command.Parameters.Add("@ShelfLife", SqlDbType.SmallInt).Value = ShelfLife;
                command.Parameters.Add("@SpecialRequirement", SqlDbType.NVarChar).Value = SpecialRequirement;
                command.Parameters.Add("@GasContent", SqlDbType.Float).Value = GasContent;
                command.Parameters.Add("@Pressure", SqlDbType.Float).Value = Pressure;
                command.Parameters.Add("@GasPrice", SqlDbType.Float).Value = GasPrice;
                command.Parameters.Add("@MinLeadTime", SqlDbType.SmallInt).Value = MinLeadTime;
                command.Parameters.Add("@MaxLeadTime", SqlDbType.SmallInt).Value = MaxLeadTime;
                command.Parameters.Add("@TotalComponent", SqlDbType.SmallInt).Value = TotalComponent;
                command.Parameters.Add("@CylinderTypeID", SqlDbType.NVarChar).Value = CylinderTypeID;
                command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;

                rdr = command.ExecuteReader();               


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

            return;
        }

        public void InsertRecipeDetail(short companyId, string RecipeNo, short DetailNo, short ComponentID, short ConcentrationUnitID, float RequestedConcentration, string RequestedConcentrationUnit,
            float IdealWeight, bool IsBaseGas, bool RequiredSpecification, float BlendTolerance, float CertificationAccuracy)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppRecipeDetailInsert", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@RecipeNo", SqlDbType.NVarChar).Value = RecipeNo;
                command.Parameters.Add("@DetailNo", SqlDbType.SmallInt).Value = DetailNo;
                command.Parameters.Add("@ComponentID", SqlDbType.SmallInt).Value = ComponentID;
                command.Parameters.Add("@ConcentrationUnitID", SqlDbType.SmallInt).Value = ConcentrationUnitID;
                command.Parameters.Add("@RequestedConcentration", SqlDbType.Float).Value = RequestedConcentration;
                command.Parameters.Add("@RequestedConcentrationUnit", SqlDbType.NVarChar).Value = RequestedConcentrationUnit;
                command.Parameters.Add("@IdealWeight", SqlDbType.Float).Value = IdealWeight;
                command.Parameters.Add("@IsBaseGas", SqlDbType.Bit).Value = IsBaseGas;
                command.Parameters.Add("@RequiredSpecification", SqlDbType.Bit).Value = RequiredSpecification;
                command.Parameters.Add("@BlendTolerance", SqlDbType.Float).Value = BlendTolerance;
                command.Parameters.Add("@CertificationAccuracy", SqlDbType.Float).Value = CertificationAccuracy;
                 
                rdr = command.ExecuteReader();

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

            return;
        }

        public void DuplicateMR(short companyId, string oldMRNo, string newMRNo, short userNumId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppMRDuplicate", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@OldMRNo", SqlDbType.NVarChar).Value = oldMRNo;
                command.Parameters.Add("@NewMRNo", SqlDbType.NVarChar).Value = newMRNo;
                command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userNumId;
                rdr = command.ExecuteReader();

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

            return;
        }


        /// <summary>
        /// Retrieve Purchasing Email
        /// </summary>
        public void GetPurchasingEmailByCoyID(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppPurchasingEmailByCoyIDSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;           
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve CSO Email
        /// </summary>
        public void GetCSOEmailByCoyID(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCSOEmailByCoyIDSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void InsertScheduledTaskProductDescription(short companyId, string ProductCode, short SrNo, string ProductDescription)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppScheduledTaskProductDescriptionInsert", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = ProductCode;
                command.Parameters.Add("@SrNo", SqlDbType.Int).Value = SrNo;
                command.Parameters.Add("@ProductDescription", SqlDbType.NVarChar).Value = ProductDescription;

                rdr = command.ExecuteReader();

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

            return;
        }


        public void GetMRDetailByProjectNo(short companyId, string projectNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRDetailByProjectNoSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = projectNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        //Added By Kim on 03 Aug 2016
        /// <summary>
        /// 
        /// </summary>
        public void procDeleteEmployeeEducationalQualificationByCompany(short companyId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppDeleteEmployeeEducationalQualificationByCompany", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                rdr = command.ExecuteReader();
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

            return;
        }

        //Added By Kim on 03 Aug 2016
        /// <summary>
        /// 
        /// </summary>
        public void procDeleteEmployeeHistoryByCompany(short companyId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppDeleteEmployeeHistoryByCompany", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                rdr = command.ExecuteReader();
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

            return;
        }


        //Added By Kim on 03 Aug 2016
        /// <summary>
        /// 
        /// </summary>
        public void procDeleteEmployeeCareerProgressionByCompany(short companyId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppDeleteEmployeeCareerProgressionByCompany", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                rdr = command.ExecuteReader();
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

            return;
        }

        
        public void GetAccountList(short companyid, string account, short userNumId, bool exact, string accounttype, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppAccountsListSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@Account", SqlDbType.NVarChar).Value = account;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userNumId;
            command.Parameters.Add("@Exact", SqlDbType.Bit).Value = exact;
            command.Parameters.Add("@AccountType", SqlDbType.NVarChar).Value = accounttype;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetProductList(short companyid, string product, short pm, short ph, short ph3, short ph5, bool exact, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductListSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@Product", SqlDbType.NVarChar).Value = product;
            command.Parameters.Add("@PM", SqlDbType.SmallInt).Value = pm;
            command.Parameters.Add("@PH", SqlDbType.SmallInt).Value = ph;
            command.Parameters.Add("@PH3", SqlDbType.SmallInt).Value = ph3;
            command.Parameters.Add("@PH5", SqlDbType.SmallInt).Value = ph5;
            command.Parameters.Add("@Exact", SqlDbType.Bit).Value = exact;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetUOMList(short companyid, string uom, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductUOMSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@UOM", SqlDbType.NVarChar).Value = uom;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetProductGroupList(short companyid, string productgroupcode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductGroupListSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProductGroup", SqlDbType.NVarChar).Value = productgroupcode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetProductTeamByProductGroup(short companyid, string productgroupcode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductTeamByProductGroupSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProductGroup", SqlDbType.NVarChar).Value = productgroupcode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetProductTeamByProductCode(short companyid, string productcode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductTeamByProductCodeSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productcode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetRequestorAndApproverList(short companyId, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRRequestorAndApproverSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetRequestorList(short companyId, short userId, string MRScheme, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRRequestorListSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@MRScheme", SqlDbType.NVarChar).Value = MRScheme;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetApprover1List(short companyId, short userId, string userRealName, string MRScheme ,ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRApprover1ListSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@UserRealName", SqlDbType.NVarChar).Value = userRealName;
            command.Parameters.Add("@MRScheme", SqlDbType.NVarChar).Value = MRScheme;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetApprover2List(short companyId, short userId, string userRealName, string MRScheme, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRApprover2ListSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@UserRealName", SqlDbType.NVarChar).Value = userRealName;
            command.Parameters.Add("@MRScheme", SqlDbType.NVarChar).Value = MRScheme;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetApprover3List(short companyId, short userId, string userRealName, string MRScheme, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRApprover3ListSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@UserRealName", SqlDbType.NVarChar).Value = userRealName;
            command.Parameters.Add("@MRScheme", SqlDbType.NVarChar).Value = MRScheme;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetApprover4List(short companyId, short userId, string userRealName, string MRScheme, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRApprover4ListSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@UserRealName", SqlDbType.NVarChar).Value = userRealName;
            command.Parameters.Add("@MRScheme", SqlDbType.NVarChar).Value = MRScheme;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetPurchaserList(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRPurchaserListSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetWarehouseList(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppWarehouseListSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// CanUserAccessDocument
        /// </summary>
        public void CanUserAccessForMR(short companyId, string documentType, string documentNo, short userId, string MRScheme, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCheckUserAccessForMR", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@DocumentType", SqlDbType.NVarChar).Value = documentType;
            command.Parameters.Add("@DocumentNo", SqlDbType.NVarChar).Value = documentNo;
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@MRScheme", SqlDbType.NVarChar).Value = MRScheme;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;

        }

        /// <summary>
        /// Product Code
        /// </summary>
        public void CheckProductCode(short companyId, string ProdCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductCodeCheck", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = ProdCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;

        }

        /// <summary>
        /// Product Code
        /// </summary>
        public void CheckAccountCode(short companyId, string AccountCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppAccountCodeCheck", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@Account", SqlDbType.NVarChar).Value = AccountCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;

        }

        public void GetMRStatusList(short companyId, string Status, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRStatusSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetTaxType(short companyId, bool IsCustomerSales ,ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppTaxTypeSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@IsCustomerSales", SqlDbType.Bit).Value = IsCustomerSales;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetPurchaseInfoForProductTeam(short companyId, string ProdCode, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppPurchaseInfoForProductTeamSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProdCode", SqlDbType.NVarChar).Value = ProdCode;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetSalesInfoForProductTeam(short companyId, string ProdCode, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSalesInfoForProductTeamSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProdCode", SqlDbType.NVarChar).Value = ProdCode;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetSalesInfo(short companyId, string ProdCode, string accountCode, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSalesInfoSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProdCode", SqlDbType.NVarChar).Value = ProdCode;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetAutoInsertedVendorByMRNo(short companyId, string mrno, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRAutoInsertedVendorByMRNoSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrno;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        //Added By Kim on 15 Apr 2013
        /// <summary>
        /// Retrieve MR UserRole
        /// </summary>
        public void GetMRHeaderByMRNo(short companyId, string mrno, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRHeaderByMRNoSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrno;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        //Added By Kim on 15 Apr 2013
        /// <summary>
        /// Retrieve MR UserRole
        /// </summary>
        public void GetMRHeaderByPIID(Guid piid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRHeaderByPIIDSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;           
            command.Parameters.Add("@PIID", SqlDbType.UniqueIdentifier).Value = piid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        //Added By Kim on 15 Apr 2013
        /// <summary>
        /// Retrieve MR UserRole
        /// </summary>
        public void SendEmailNotificationForMR(string action, Guid piid)
        { 
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procEmailNotification_MR", (SqlConnection)conn);


                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Action", SqlDbType.NVarChar).Value = action;
                command.Parameters.Add("@PIID", SqlDbType.UniqueIdentifier).Value = piid;
                rdr = command.ExecuteReader();
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

            return;
        }


        public void GetGRNInfoByPO(short companyId, string PONo, short userId, string mrno, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppGRNInfoByPOSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@PONo", SqlDbType.NVarChar).Value = PONo;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@DocumentNo", SqlDbType.NVarChar).Value = mrno;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;            
        }


        public void InsertMRLog(short companyId, string mrno, int userId, int onbehalf, string type)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppInsertLogForMR", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrno;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                command.Parameters.Add("@OnBehalfUserID", SqlDbType.Int).Value = onbehalf;
                command.Parameters.Add("@Type", SqlDbType.NVarChar).Value = type;                
                rdr = command.ExecuteReader();
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

            return;
        }

        //Added By Adam [Issue Module]
        #region Issue Module
        public void GetIssue(string System, string Type, string IssueID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppIssue_Select", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@System", SqlDbType.NVarChar).Value = System;
            command.Parameters.Add("@Type", SqlDbType.NVarChar).Value = Type;
            command.Parameters.Add("@IssueID", SqlDbType.NVarChar).Value = IssueID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        public void InsertIssue(short CoyID, string System, string Type, string ReportedBy, string Description, short CreatedBy)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppIssue_Insert", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
                command.Parameters.Add("@System", SqlDbType.NVarChar).Value = System;
                command.Parameters.Add("@Type", SqlDbType.NVarChar).Value = Type;
                command.Parameters.Add("@ReportedBy", SqlDbType.NVarChar).Value = ReportedBy;
                command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
                command.Parameters.Add("@CreatedBy", SqlDbType.SmallInt).Value = CreatedBy;
                rdr = command.ExecuteReader();
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
            return;
        }
        public void UpdateIssue(string IssueID, string System, string Type, string ReportedBy, string Description, string Remarks, string Status, short ModifiedBy)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppIssue_Update", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@IssueID", SqlDbType.NVarChar).Value = IssueID;
                command.Parameters.Add("@System", SqlDbType.NVarChar).Value = System;
                command.Parameters.Add("@Type", SqlDbType.NVarChar).Value = Type;
                command.Parameters.Add("@ReportedBy", SqlDbType.NVarChar).Value = ReportedBy;
                command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
                command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
                command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;
                command.Parameters.Add("@ModifiedBy", SqlDbType.SmallInt).Value = ModifiedBy;
                rdr = command.ExecuteReader();
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
            return;
        }
        #endregion

        public void GetAccountAgeing(short companyId, string accountCode, string creditTermValidation, DateTime trnDate, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procWSAccountAgeingSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            command.Parameters.Add("@CreditTermValidation", SqlDbType.NVarChar).Value = creditTermValidation;
            command.Parameters.Add("@Date", SqlDbType.DateTime).Value = trnDate;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public Boolean GetPostedTransaction(short coyID, string documentNo, string module)
        {
            Boolean result;
            try
            {
                IDbConnection conn = cm.GetConnection();
                SqlCommand command = new SqlCommand("procAppCommonPostedTransactionSelect", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyID;
                command.Parameters.Add("@DocumentNo", SqlDbType.NVarChar).Value = documentNo;
                command.Parameters.Add("@Module", SqlDbType.NVarChar).Value = module;                
                command.Parameters.Add("@Count", SqlDbType.Int).Direction = ParameterDirection.Output;
                conn.Open();
                command.ExecuteScalar();
                result = Convert.ToBoolean(command.Parameters["@Count"].Value);
                conn.Close();
                conn.Dispose();
            }
            catch
            {
                return false;
            }
            return result;
        }

        public void UpdatePostedBeforeTransaction(short coyID, string documentNo, string module)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppCommonPostedTransactionBeforeUpdate", (SqlConnection)conn);


                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyID;
                command.Parameters.Add("@DocumentNo", SqlDbType.NVarChar).Value = documentNo;
                command.Parameters.Add("@Module", SqlDbType.NVarChar).Value = module;
                rdr = command.ExecuteReader();
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

            return;
        }


        public void UpdatePostedTransaction(short coyID, string documentNo, string module, int SAPNo)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppCommonPostedTransactionUpdate", (SqlConnection)conn);


                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyID;
                command.Parameters.Add("@DocumentNo", SqlDbType.NVarChar).Value = documentNo;
                command.Parameters.Add("@Module", SqlDbType.NVarChar).Value = module;
                command.Parameters.Add("@SAPNo", SqlDbType.Int).Value = SAPNo;
                rdr = command.ExecuteReader();
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

            return;
        }

        public void UpdatePostedErrorTransaction(short coyID, string documentNo, string module, bool isGLInfo)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppCommonPostedTransactionErrorUpdate", (SqlConnection)conn);


                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyID;
                command.Parameters.Add("@DocumentNo", SqlDbType.NVarChar).Value = documentNo;
                command.Parameters.Add("@Module", SqlDbType.NVarChar).Value = module;
                command.Parameters.Add("@IsGLInfo", SqlDbType.Bit).Value = isGLInfo;
                rdr = command.ExecuteReader();
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

            return;
        }

        public void CancelTransaction(short coyID, string documentNo, string module, string cancelledReason, int SAPNo)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppCommonCancelTransactionUpdate", (SqlConnection)conn);


                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyID;
                command.Parameters.Add("@DocumentNo", SqlDbType.NVarChar).Value = documentNo;
                command.Parameters.Add("@Module", SqlDbType.NVarChar).Value = module;
                command.Parameters.Add("@CancelledReason", SqlDbType.NVarChar).Value = cancelledReason;
                command.Parameters.Add("@SAPNo", SqlDbType.Int).Value = SAPNo;                
                rdr = command.ExecuteReader();
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

            return;
        }

        public void InsertSAPLog(short companyId, string method, string json, string exceptionMessage)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppSAPLogInsert", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@Method", SqlDbType.NVarChar).Value = method;
                command.Parameters.Add("@JSON", SqlDbType.NVarChar).Value = json;
                command.Parameters.Add("@ExceptionMessage", SqlDbType.NVarChar).Value = exceptionMessage;               
                rdr = command.ExecuteReader();
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

            return;
        }

        public void GetMaterialRequisitionByMRNoForSAP(short companyId, string mrNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRByMRNoForSAPSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void CanUserAccessCost(short companyId, string prodCode, short userId, bool isGasDivision, bool isWeldingDivision, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCheckUserAccessCost", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProdCode", SqlDbType.NVarChar).Value = prodCode;            
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@IsGasDivision", SqlDbType.Bit).Value = isGasDivision;
            command.Parameters.Add("@IsWeldingDivision", SqlDbType.Bit).Value = isWeldingDivision;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;

        }

        /// <summary>
        /// CanUserAccessDocument
        /// </summary>
        public void CheckDataForMR(short companyId, string documentType, string documentNo, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCheckDataForMR", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@DocumentType", SqlDbType.NVarChar).Value = documentType;
            command.Parameters.Add("@DocumentNo", SqlDbType.NVarChar).Value = documentNo;
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;           
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;

        }

        public void GetActionAccessRight(int userId, int action, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppGetAccessRight", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@Action", SqlDbType.Int).Value = action;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;

        }
        public void GetClaims(short companyId, short userId, short status, string claimFrom, string claimTo, short condition, ref DataSet ds)
        {
            DateTime claimFromDate, claimToDate;
            //date 
            if (claimFrom != "")
                claimFromDate = DateTime.ParseExact(claimFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            else
                claimFromDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (claimTo != "")
                claimToDate = DateTime.ParseExact(claimTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            else
                claimToDate = DateTime.Now;

            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectEntertainmentClaim", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userId;
            command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = status;
            command.Parameters.Add("@ClaimFrom", SqlDbType.DateTime).Value = claimFromDate;
            command.Parameters.Add("@ClaimTo", SqlDbType.DateTime).Value = claimToDate;
            command.Parameters.Add("@Condition", SqlDbType.SmallInt).Value = condition;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;

        }


        public void GetClaimDetails(int claimID, int CompanyID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectEntertainmentClaimDetailByClaimID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ID", SqlDbType.Int).Value = claimID;
            command.Parameters.Add("@CoyID", SqlDbType.Int).Value = CompanyID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;

        }
        public void GetClaimAttachment(int claimDetailID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectEntertainmentClaimDetailAttachmentByClaimID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ClaimDetailID", SqlDbType.Int).Value = claimDetailID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;

        }
        public void SaveClaimAttachment(int ClaimAttachmentID, int claimDetailID, string data)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;

            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppSaveEntertainmentClaimDetailAttachment", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ClaimAttachmentID", SqlDbType.Int).Value = ClaimAttachmentID;
                command.Parameters.Add("@ClaimDetailID", SqlDbType.Int).Value = claimDetailID;
                command.Parameters.Add("@Attachment", SqlDbType.NVarChar).Value = data;

                rdr = command.ExecuteReader();
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

            return;

        }
        public void DeleteClaimDetailAttachment(int ClaimAttachmentID)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;

            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppDeleteEntertainmentClaimDetailAttachment", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ClaimAttachmentID", SqlDbType.Int).Value = ClaimAttachmentID;

                rdr = command.ExecuteReader();
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

            return;

        }
        public void GetClaimByID(int ClaimID, ref DataSet ds)
        {

            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectEntertainmentClaimByID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ClaimID", SqlDbType.Int).Value = ClaimID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void InsertNewClaim(string ClaimNo, short CoyID, short UserID, string ClaimDate, string Desc)
        {
            DateTime claimDateTime = DateTime.ParseExact(ClaimDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppInsertClaim", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ClaimNo", SqlDbType.NVarChar).Value = ClaimNo;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
                command.Parameters.Add("@ClaimDate", SqlDbType.DateTime).Value = claimDateTime;
                command.Parameters.Add("@CreatedBy", SqlDbType.SmallInt).Value = UserID;
                command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Desc;

                rdr = command.ExecuteReader();
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

            return;
        }

        public void UpdateClaim(int ClaimID, string Description, string ClaimDate, string ClaimantDesig,
            string SalesPersonID, string NumPplEntertained, string CreateOnBehalf,
            short dim1, short dim2, short dim3, short dim4,
            string Cust1, string Cust2, string Cust3,
            string Desig1, string Desig2, string Desig3,
            string Person1, string Person2, string Person3,
            string Phone1, string Phone2, string Phone3)
        {

            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppUpdateClaimByID", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ClaimID", SqlDbType.Int).Value = ClaimID;
                command.Parameters.Add("@ClaimDate", SqlDbType.NVarChar).Value = ClaimDate;
                command.Parameters.Add("@dim1", SqlDbType.SmallInt).Value = dim1;
                command.Parameters.Add("@dim2", SqlDbType.SmallInt).Value = dim2;
                command.Parameters.Add("@dim3", SqlDbType.SmallInt).Value = dim3;
                command.Parameters.Add("@dim4", SqlDbType.SmallInt).Value = dim4;
                command.Parameters.Add("@Cust1", SqlDbType.NVarChar).Value = Cust1;
                command.Parameters.Add("@Cust2", SqlDbType.NVarChar).Value = Cust2;
                command.Parameters.Add("@Cust3", SqlDbType.NVarChar).Value = Cust3;
                command.Parameters.Add("@Person1", SqlDbType.NVarChar).Value = Person1;
                command.Parameters.Add("@Person2", SqlDbType.NVarChar).Value = Person2;
                command.Parameters.Add("@Person3", SqlDbType.NVarChar).Value = Person3;
                command.Parameters.Add("@Phone1", SqlDbType.NVarChar).Value = Phone1;
                command.Parameters.Add("@Phone2", SqlDbType.NVarChar).Value = Phone2;
                command.Parameters.Add("@Phone3", SqlDbType.NVarChar).Value = Phone3;
                command.Parameters.Add("@Desig1", SqlDbType.NVarChar).Value = Desig1;
                command.Parameters.Add("@Desig2", SqlDbType.NVarChar).Value = Desig2;
                command.Parameters.Add("@Desig3", SqlDbType.NVarChar).Value = Desig3;
                command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
                command.Parameters.Add("@ClaimantDesig", SqlDbType.NVarChar).Value = ClaimantDesig;
                command.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = SalesPersonID;
                command.Parameters.Add("@NumPplEntertained", SqlDbType.NVarChar).Value = NumPplEntertained;
                command.Parameters.Add("@CreateOnBehalf", SqlDbType.NVarChar).Value = CreateOnBehalf;

                rdr = command.ExecuteReader();
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

            return;
        }

        public void InsertNewClaimDetail(short companyID, short ClaimID, string type, string date, string remark, string currencyCode,
            float currencyRate, float amount, string chargeto, float GST, string receiptNum)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppInsertClaimDetail", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyID;
                command.Parameters.Add("@ClaimID", SqlDbType.SmallInt).Value = ClaimID;
                command.Parameters.Add("@type", SqlDbType.NVarChar).Value = type;
                command.Parameters.Add("@claimDate", SqlDbType.NVarChar).Value = date;
                command.Parameters.Add("@remark", SqlDbType.NVarChar).Value = remark == null ? "" : remark;
                command.Parameters.Add("@currencyCode", SqlDbType.NVarChar).Value = currencyCode;
                command.Parameters.Add("@currencyRate", SqlDbType.Decimal).Value = currencyRate;
                command.Parameters.Add("@amount", SqlDbType.Decimal).Value = amount;
                command.Parameters.Add("@chargeto", SqlDbType.NVarChar).Value = chargeto;
                command.Parameters.Add("@gst", SqlDbType.Decimal).Value = GST;
                command.Parameters.Add("@receiptNum", SqlDbType.NVarChar).Value = receiptNum;

                rdr = command.ExecuteReader();
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

            return;
        }

        public void UpdateClaimDetail(int ClaimDetailID, string type, string date, 
            string remark, string currencyCode, float currencyRate, float amount, string chargeto,
            float GST, string receiptNum)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppUpdateClaimDetail", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ClaimDetailID", SqlDbType.Int).Value = ClaimDetailID;
                command.Parameters.Add("@type", SqlDbType.NVarChar).Value = type;
                command.Parameters.Add("@claimDate", SqlDbType.NVarChar).Value = date;
                command.Parameters.Add("@remark", SqlDbType.NVarChar).Value = remark;
                command.Parameters.Add("@currencyCode", SqlDbType.NVarChar).Value = currencyCode;
                command.Parameters.Add("@currencyRate", SqlDbType.Decimal).Value = currencyRate;
                command.Parameters.Add("@amount", SqlDbType.Decimal).Value = amount;
                command.Parameters.Add("@chargeto", SqlDbType.NVarChar).Value = chargeto;
                command.Parameters.Add("@gst", SqlDbType.Decimal).Value = GST;
                command.Parameters.Add("@receiptNum", SqlDbType.NVarChar).Value = receiptNum;

                rdr = command.ExecuteReader();
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

            return;
        }
        public void DeleteClaimDetail(int ClaimDetailID)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppDeleteClaimDetailByID", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ClaimDetailID", SqlDbType.Int).Value = ClaimDetailID;

                rdr = command.ExecuteReader();
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

            return;
        }


        public void UpdateClaimStatus(int ClaimID, int UserID, short Status, string RejectRemark, string ApprovePaymentVoucher)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppUpdateClaimStatus", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ClaimID", SqlDbType.Int).Value = ClaimID;
                command.Parameters.Add("@Status", SqlDbType.Int).Value = Status;
                command.Parameters.Add("@AppOrRejby", SqlDbType.Int).Value = UserID;
                command.Parameters.Add("@RejRemark", SqlDbType.NVarChar).Value = RejectRemark;
                command.Parameters.Add("@AppPayVou", SqlDbType.NVarChar).Value = ApprovePaymentVoucher;

                rdr = command.ExecuteReader();
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

            return;
        }


        public void GetNewFeeds(short companyId, short userId, string status, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppGetFeed", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userId;
            command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = status;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;

        }

        public void ReadNewFeed(short feedId, short userId)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppReadFeed", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@FeedID", SqlDbType.SmallInt).Value = feedId;
                command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userId;

                rdr = command.ExecuteReader();
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

            return;
        }

        public void InsertNewFeed(short CoyID, short UserID, string Title, string Desc, string Content)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppInsertFeed", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
                command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = UserID;
                command.Parameters.Add("@Title", SqlDbType.NVarChar).Value = Title;
                command.Parameters.Add("@Desc", SqlDbType.NVarChar).Value = Desc;
                command.Parameters.Add("@Content", SqlDbType.NVarChar).Value = Content;

                rdr = command.ExecuteReader();
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

            return;
        }

        public void UpdateNewFeed(short ID, short CoyID, short UserID, string Title, string Desc, string Content)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppUpdateFeed", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ID", SqlDbType.SmallInt).Value = ID;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
                command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = UserID;
                command.Parameters.Add("@Title", SqlDbType.NVarChar).Value = Title;
                command.Parameters.Add("@Desc", SqlDbType.NVarChar).Value = Desc;
                command.Parameters.Add("@Content", SqlDbType.NVarChar).Value = Content;

                rdr = command.ExecuteReader();
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

            return;
        }

        public void GetSelectionCustomerList(string name, short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectionGetCustomerListByID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = '%'+name+'%';
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetSelectionCurrencyList(string defaultCurrency, string currency, short companyId, short year, short month, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectionGetCurrencyRateListByCodeInSGD", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@Year", SqlDbType.SmallInt).Value = year;
            command.Parameters.Add("@Month", SqlDbType.SmallInt).Value = month;
            command.Parameters.Add("@DefaultCurrency", SqlDbType.NVarChar).Value = defaultCurrency;
            command.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = '%' + currency + '%';
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetFinanceItemByReport(string Report, ref DataSet ds)
        {

            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectFinanceItemByReport", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Report", SqlDbType.NVarChar).Value = Report;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        
        public void GetClaimSalesPersonID(int companyId, int claimantId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectClaimSalesPersonID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = claimantId;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetClaimEntertainmentReceiptType(ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand(@"Select * from tbEntertainmentType", (SqlConnection)conn);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void DeleteClaim(int ClaimID, int CoyID)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppDeleteClaimByID", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ClaimID", SqlDbType.Int).Value = ClaimID;
                command.Parameters.Add("@CoyID", SqlDbType.Int).Value = CoyID;

                rdr = command.ExecuteReader();
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

            return;
        }

        public void IsLatestTransaction(int CoyID, int DocumentNo, string ModifiedDate, string Module, ref Boolean trans)
        {
            //this function return only 1 or 0

            Int32 result = 0;
            DateTime NewModifiedDate;
            NewModifiedDate = DateTime.ParseExact(ModifiedDate, "d/MM/yyyy H:mm:ss tt", CultureInfo.InvariantCulture);

            IDbConnection conn = cm.GetConnection();
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procCommonLatestTransaction", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.Int).Value = CoyID;
                command.Parameters.Add("@DocumentNo", SqlDbType.Int).Value = DocumentNo;
                command.Parameters.Add("@ModifiedDate", SqlDbType.DateTime).Value = ModifiedDate;
                command.Parameters.Add("@Module", SqlDbType.NVarChar).Value = Module;
                result = (Int32)command.ExecuteScalar();
                trans = Convert.ToBoolean(result);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return;
        }

        public void EntertainmentClaimCreateOnBehalf(int CoyID, int UserID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppEntertainmentGetCreateOnBehalf", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.Int).Value = CoyID;
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// GetListOfMRRequiresProductManagerApprovalByUserNumId
        /// </summary>
        public void GetListOfRejectedMRByUserNumId(short companyId, short userNumId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppListOfRejectedMRByUserIdSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userNumId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetSalesExecForReport(short companyId, short userid, int year, int month, string department,ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSalesPersonSelectForReport", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumId", SqlDbType.Int).Value = userid;
            command.Parameters.Add("@Year", SqlDbType.Int).Value = year;
            command.Parameters.Add("@Month", SqlDbType.Int).Value = month;
            command.Parameters.Add("@Department", SqlDbType.NVarChar).Value = department;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetDepartmentForReport(short companyId, short reportid, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSelectCompanyDivisionForSalesReport", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;            
            command.Parameters.Add("@ReportID", SqlDbType.SmallInt).Value = reportid;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userid;   

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetWarehouseSearch(short companyId, ref DataSet ds) {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand(@"Select * from tbWarehouseSearch where CoyID = @p1", (SqlConnection)conn);
            command.Parameters.AddWithValue("@p1", companyId);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void DeleteProductCostForYearMonth(short companyId, short year, short month)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppProductCostByYearAndMonthDelete", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@tbYear", SqlDbType.SmallInt).Value = year;
                command.Parameters.Add("@tbMonth", SqlDbType.SmallInt).Value = month;
                rdr = command.ExecuteReader();


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

            return;

        }

        public void InsertProductCostForYearMonth(short companyId, short year, short month, string product, string uom, string dimensionL1, string dimensionL2, string dimensionL3, string dimensionL4, Decimal totalProduction, Decimal totalCost)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppProductCostByYearAndMonthInsert", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@tbYear", SqlDbType.SmallInt).Value = year;
                command.Parameters.Add("@tbMonth", SqlDbType.SmallInt).Value = month;
                command.Parameters.Add("@Product", SqlDbType.NVarChar).Value = product;
                command.Parameters.Add("@UOM", SqlDbType.NVarChar).Value = uom;
                command.Parameters.Add("@DimensionL1", SqlDbType.NVarChar).Value = dimensionL1;
                command.Parameters.Add("@DimensionL2", SqlDbType.NVarChar).Value = dimensionL2;
                command.Parameters.Add("@DimensionL3", SqlDbType.NVarChar).Value = dimensionL3;
                command.Parameters.Add("@DimensionL4", SqlDbType.NVarChar).Value = dimensionL4;
                command.Parameters.Add("@TotalProduction", SqlDbType.Decimal).Value = totalProduction;
                command.Parameters.Add("@TotalCost", SqlDbType.Decimal).Value = totalCost;
                rdr = command.ExecuteReader();
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

            return;
        }

        public void SendVendorEmail(short companyID, string companyName, string email, string link)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null; 
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procVendorEmailGenerator", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@coyid", SqlDbType.SmallInt).Value = companyID;
                command.Parameters.Add("@companyName", SqlDbType.NVarChar).Value = companyName;
                command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
                command.Parameters.Add("@linktopass", SqlDbType.NVarChar).Value = link;
                rdr = command.ExecuteReader();
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

            return;
        }

        public void GetProductCategoryForReport(ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductCategoryForReportSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void UpdateProductShortName(short companyId, string productCode, string shortName)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppProductShortNameUpdate", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
                command.Parameters.Add("@ShortName", SqlDbType.NVarChar).Value = shortName;  
                rdr = command.ExecuteReader();
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

            return;
        }
    }
}
