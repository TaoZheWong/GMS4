using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web;
using System.Diagnostics;

using GMSCore;

namespace GMSCore
{
    /// <summary>
    /// Summary description for DebtorCommentaryDALC.
    /// </summary>
    public class DebtorCommentaryDALC
    {
        private ConnectionManager cm;

        //---------------------------------------------------------------------
        // public DebtorCommentaryDALC()
        //---------------------------------------------------------------------
        /// <summary>
        /// Default constructor. If the object is instantiated using the 
        /// default constructor, create a new connection. UI Components will 
        /// always use this constructor.
        /// </summary>
        //---------------------------------------------------------------------
        public DebtorCommentaryDALC()
        {
            cm = new ConnectionManager();
        }	//	end of ContainerDALC()

        #region Methods for DebtorCommentary
        /// <summary>
        /// Retrieve list of debtors' records.
        /// </summary>
        //public void GetDebtorsRecords(short companyId, DateTime asOfDate, string salesPersonID, short userId, ref DataSet ds)
        //{
        //    IDbConnection conn = cm.GetConnection();
        //    SqlCommand command = new SqlCommand("procAppDebtorRecordsSelect", (SqlConnection)conn);
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
        //    command.Parameters.Add("@AsOfDate", SqlDbType.SmallDateTime).Value = asOfDate;
        //    command.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = salesPersonID;
        //    command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userId;
        //    SqlDataAdapter adapter = new SqlDataAdapter(command);
        //    adapter.Fill(ds);
        //    return;
        //}

        public void GetDebtorsRecordsWithDays(short companyId, short days, DateTime asOfDate, string salesPersonID, short userId, string salesPersonType, string accountCode, string accountName, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppDebtorRecordsWithDaysSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@Days", SqlDbType.SmallInt).Value = days;
            command.Parameters.Add("@AsOfDate", SqlDbType.SmallDateTime).Value = asOfDate;
            command.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = salesPersonID;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userId;
            command.Parameters.Add("@SalesPersonType", SqlDbType.NVarChar).Value = salesPersonType;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of Salesperson records by usernumid.
        /// </summary>
        public void GetSalesPersonRecords(short companyId, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSalesPersonSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        //Added By Kim on 13 July 2012
        /// <summary>
        /// Retrieve list of ALL Salesperson records by usernumid.
        /// </summary>
        public void GetSalesPersonRecordsByUserNumIDCoyID(short companyId, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSalesPersonByUserNumIDCoyIDSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        
        /// <summary>
        /// Retrieve list of debtor's details base on day range.
        /// </summary>
        public void GetDebtorsDetails(short companyId, DateTime asOfDate, string accountCode, string currency,
                                        short dayRangeFrom, short dayRangeTo, string salesPersonID, string salesPersonType,  ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppDebtorDetailsSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AsOfDate", SqlDbType.SmallDateTime).Value = asOfDate;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = currency;
            command.Parameters.Add("@DayRangeFrom", SqlDbType.SmallInt).Value = dayRangeFrom;
            command.Parameters.Add("@DayRangeTo", SqlDbType.SmallInt).Value = dayRangeTo;
            command.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = salesPersonID;           
            command.Parameters.Add("@SalesPersonType", SqlDbType.NVarChar).Value = salesPersonType;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of debtor's payment details base on day range.
        /// </summary>
        public void GetDebtorsPaymentDetails(short companyId, string accountCode, DateTime receiptDateFrom, DateTime receiptDateTo, short userId, string paymentRefNo, string salesPersonID, string salesPersonType, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppDebtorPaymentDetailsSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            command.Parameters.Add("@ReceiptDateFrom", SqlDbType.SmallDateTime).Value = receiptDateFrom;
            command.Parameters.Add("@ReceiptDateTo", SqlDbType.SmallDateTime).Value = receiptDateTo;            
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@PaymentRefNo", SqlDbType.NVarChar).Value = paymentRefNo;
            command.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = salesPersonID;
            command.Parameters.Add("@SalesPersonType", SqlDbType.NVarChar).Value = salesPersonType;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of debtor's last payment details base on day range.
        /// </summary>
        public void GetDebtorsLastPaymentDetails(short companyId, string accountCode, string docno, short userId, string salesPersonID, string salesPersonType, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppDebtorLastPaymentDetailsSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            command.Parameters.Add("@DocNo", SqlDbType.NVarChar).Value = docno;  
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = salesPersonID;
            command.Parameters.Add("@SalesPersonType", SqlDbType.NVarChar).Value = salesPersonType;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of debtors.
        /// </summary>
        public void GetDebtors(short companyId, string accountCode, string accountName, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppDebtorsWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of debtors.
        /// </summary>
        public void GetCustomerWithoutFilterByCompany(string accountName, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCustomerWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;           
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve debtor's detail info by Account Code.
        /// </summary>
        public void GetDebtorDetailByAccountCode(short companyId, string accountCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppDebtorDetailSelectByAccountCode", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve customer's detail info by Account Code.
        /// </summary>
        public void GetDebtorDetailByAccountCodeCRM(short companyId, string accountCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppDebtorDetailSelectByAccountCodeCRM", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve debtor's detail info by Account Code.
        /// </summary>
        public void GetProspectDetailByAccountCode(short companyId, string accountCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProspectDetailSelectByAccountCode", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of debtors for Search All.
        /// </summary>
        public void GetDebtorsForAll(short companyId, string accountName, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppDebtorsWildcardSelectForAll", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        #endregion
    }
}
