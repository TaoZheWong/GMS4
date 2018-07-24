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
    /// Summary description for GMSGeneralDALC.
    /// </summary>
    public class FinanceDALC
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
        public FinanceDALC()
        {
            cm = new ConnectionManager();
        }

        #region ProcessTrialBalance
        public void ProcessTrialBalance()
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procFinanceTBProcessAll", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 5000; 
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
        }
        #endregion

        #region ConstructTempTransferSQL
        public string ConstructTempTransferSQL(short coyId, short year, short month, System.Data.DataRowCollection ttRows)
        {
            string SQL = "DELETE FROM tbTempTransfer;";
            foreach (DataRow dr in ttRows)
            {
                SQL += "INSERT INTO tbTempTransfer VALUES (" + coyId + ", " +
                       dr["Project"].ToString() + ", " + dr["Department"].ToString() + ", " + dr["Section"].ToString() + ", " +
                       year.ToString() + ", " + month.ToString() +
                       ", '" + dr["AccountCode"].ToString() + "', " +
                       Convert.ToString(Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])) + ", " +
                       Convert.ToString(Convert.ToDecimal(dr["PrevBalance"]) + Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])) + ", " +
                       "1, '" + String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now) + "');";
            }
            return SQL;
        }
        #endregion 

        #region ConstructTempTransferSQLForPDS
        public string ConstructTempTransferSQLForPDS(short coyId, short year, short month, System.Data.DataRowCollection ttRows)
        {
            string SQL = "DELETE FROM tbTempTransfer2;"; 
            foreach (DataRow dr in ttRows)
            {
                SQL += "INSERT INTO tbTempTransfer2 (CoyID, Project, Department, Section, tbYear, tbMonth, COAID, MTDTotal, YTDTotal, CreatedBy, CreatedDate) VALUES (" + coyId + ", '" +
                           dr["Project"].ToString() + "', '" + dr["Department"].ToString() + "', '" + dr["Section"].ToString() + "', " + 
                           year.ToString() + ", " + month.ToString() +
                           ", '" + dr["AccountCode"].ToString() + "', " +
                           Convert.ToString(Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])) + ", " +
                           Convert.ToString(Convert.ToDecimal(dr["PrevBalance"]) + Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])) + ", " +
                           "1, '" + String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now) + "');";
            }
            SQL += "DELETE FROM tbTempTransfer;"; 
            return SQL; 
        }
        #endregion

        #region ConstructTempTransferSQLForPDSSAP
        public string ConstructTempTransferSQLForPDSSAP(short coyId, short year, short month, System.Data.DataRowCollection ttRows)
        {
            string SQL = "DELETE FROM tbTempTransfer4;";
            foreach (DataRow dr in ttRows)
            {
                SQL += "INSERT INTO tbTempTransfer4 (CoyID, Project, Department, Section, Unit, tbYear, tbMonth, COAID, MTDTotal, YTDTotal, CreatedBy, CreatedDate) VALUES (" + coyId + ", '" +
                           dr["Project"].ToString() + "', '" + dr["Department"].ToString() + "', '" + dr["Section"].ToString() + "', '" + dr["Unit"].ToString() + "', " +
                           year.ToString() + ", " + month.ToString() +
                           ", '" + dr["AccountCode"].ToString() + "', " +
                           Convert.ToString(Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])) + ", " +
                           Convert.ToString(Convert.ToDecimal(dr["PrevBalance"]) + Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])) + ", " +
                           "1, '" + String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now) + "');";                
            }
            SQL += "DELETE FROM tbTempTransfer4a;";
            return SQL;
        }
        #endregion 

        #region ConstructTempTransferSQLForMapping
        public string ConstructTempTransferSQLForMapping(short coyId, short year, short month, System.Data.DataRowCollection ttRows)
        {
            string SQL = "DELETE FROM tbTempTransfer;";
            foreach (DataRow dr in ttRows)
            {
                try
                {
                    if (dr["Year"].ToString() == "") break;
                    SQL += "INSERT INTO tbTempTransfer VALUES (" + coyId + ", -1, -1, -1, " + dr["Year"].ToString() + ", " + dr["Month"].ToString() +
                           ", '" + dr["AccountCode"].ToString() + "', " +
                           Convert.ToString(Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])) + ", " +
                           Convert.ToString(Convert.ToDecimal(dr["PrevBalance"]) + Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])) + ", " +
                           "1, '" + String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now) + "');";
                }
                catch (Exception ex)
                {
                    throw new Exception("Trial Balance data " + dr["AccountCode"].ToString()); 
                }
            }
            return SQL;
        }
        #endregion 

        #region InsertTempTransfer
        public void InsertTempTransfer(short coyId, short year, short month, System.Data.DataRowCollection ttRows, string TBType, bool isMapping, string StatusType)
        {
            string SQL = "";
            if (isMapping)
                SQL = ConstructTempTransferSQLForMapping(coyId, year, month, ttRows);
            else
            {
                if (StatusType == "L" || StatusType == "S")
                    SQL = ConstructTempTransferSQLForPDSSAP(coyId, year, month, ttRows);
                else
                { 
                    if (TBType == "N") 
                        SQL = ConstructTempTransferSQL(coyId, year, month, ttRows);
                    else if (TBType == "P")
                        SQL = ConstructTempTransferSQLForPDS(coyId, year, month, ttRows);
                }
            }
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand(SQL, (SqlConnection)conn);
                command.CommandType = CommandType.Text; 
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
        }
        #endregion 

        public void GetFinanceItemByName(string itemName ,short companyId, short startYear, short endYear, short startMonth, short endMonth, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procReportFinanceChartByName", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@itemName", SqlDbType.NVarChar).Value = itemName;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@StartYear", SqlDbType.NVarChar).Value = startYear;
            command.Parameters.Add("@EndYear", SqlDbType.NVarChar).Value = endYear;
            command.Parameters.Add("@StartMonth", SqlDbType.NVarChar).Value = startMonth;
            command.Parameters.Add("@EndMonth", SqlDbType.NVarChar).Value = endMonth;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetFinanceBudget(string itemName, short companyId, short startYear, short endYear, short startMonth, short endMonth, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procReportFinanceChartBudget", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@itemName", SqlDbType.NVarChar).Value = itemName;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@StartYear", SqlDbType.NVarChar).Value = startYear;
            command.Parameters.Add("@EndYear", SqlDbType.NVarChar).Value = endYear;
            command.Parameters.Add("@StartMonth", SqlDbType.NVarChar).Value = startMonth;
            command.Parameters.Add("@EndMonth", SqlDbType.NVarChar).Value = endMonth;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
    }
}
