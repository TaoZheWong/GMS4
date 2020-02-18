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
    /// Summary description for DocumentDataDALC.
    /// </summary>
    public class EmployeeDataDALC
    {
        private ConnectionManager cm;

        public EmployeeDataDALC()
        {
            cm = new ConnectionManager();
        }	

        #region Methods for EmployeeData
        public void GetEmployeeListByCoyID(short coyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppEmployeeSelectByCoyID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetEmployeeListByCoyIDAndUserName(short coyId, string username, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppEmployeeSelectByCoyIDAndUserName", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyId;
            command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetEmployeeListByCoyIDAndUserID(short coyId, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppEmployeeSelectByCoyIDAndUserID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyId;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetEmployeeIndirectListByCoyIDAndUserName(short coyId, string username, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppEmployeeIndirectSelectByCoyIDAndUserName", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyId;
            command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetEmployeeIndirectListByCoyIDAndUserID(short coyId, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppEmployeeIndirectSelectByCoyIDAndUserID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyId;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetEmployeeSelfByCoyIDAndUserName(short coyId, string username, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppEmployeeSelfSelectByCoyIDAndUserName", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyId;
            command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetEmployeeSelfByCoyIDAndUserID(short coyId, short userId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppEmployeeSelfSelectByCoyIDAndUserID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = coyId;
            command.Parameters.Add("@UserNumId", SqlDbType.SmallInt).Value = userId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetEmployeeDataByNRIC(string nric,int courseSessionID ,ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppEmployeeSelectByNRIC", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@NRIC", SqlDbType.NVarChar).Value = nric;
            command.Parameters.Add("@CourseSessionID", SqlDbType.SmallInt).Value = courseSessionID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        #endregion
    }
}
