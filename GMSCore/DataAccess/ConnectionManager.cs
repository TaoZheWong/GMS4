using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web;
using System.Diagnostics;

namespace GMSCore
{
    /// <summary>
    /// Summary description for ConnectionManager.
    /// </summary>
    public class ConnectionManager
    {
        private static string m_ConnectionString = null;
        private IDbConnection m_objConnection = null;
        private IDbTransaction m_objTransaction = null;

        public static string ConnectionString
        {
            get { return m_ConnectionString; }
            set { m_ConnectionString = value; }
        }

        public ConnectionManager()
        {
            GetConnectionString();
        }

        public ConnectionManager(string connectionString)
        {
            ConnectionString = connectionString;
        }


        //-------------------------------------------------------------------
        // public GetConnectionString()
        //------------------------------------------------------------------- 
        /// <summary>
        /// This function will return Database Connection string.
        /// 
        /// </summary>
        public string GetConnectionString()
        {
            ConnectionString =
                ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

            return ConnectionString;
        }


        //-------------------------------------------------------------------
        // public GetConnection()
        //------------------------------------------------------------------- 
        /// <summary>
        /// This function will Create Connection and return.
        /// 
        /// </summary>
        public IDbConnection GetConnection()
        {
            try
            {
                SqlConnection sqlConnection = null;

                if (m_ConnectionString != null)
                {
                    sqlConnection = new SqlConnection(ConnectionString);
                    //sqlConnection.Open();
                    // To Check

                    m_objConnection = (IDbConnection)sqlConnection;
                }
                else
                {
                    //throw no connection string
                }

                return sqlConnection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IDbConnection GetConnection(string connectionString)
        {
            ConnectionManager.ConnectionString = connectionString;

            return this.GetConnection();
        }

        //-------------------------------------------------------------------
        // public BeginTransaction()
        //------------------------------------------------------------------- 
        /// <summary>
        /// This function will Start Transaction.
        /// 
        /// </summary>
        public IDbTransaction BeginTransaction()
        {
            try
            {
                SqlTransaction sqlTransaction = null;

                if (m_objConnection != null)
                {
                    sqlTransaction = (SqlTransaction)
                        m_objConnection.BeginTransaction();
                    //To check
                    m_objTransaction = (IDbTransaction)sqlTransaction;
                }
                else
                {
                    SqlConnection sqlConnection = null;
                    sqlConnection = new SqlConnection(ConnectionString);
                    sqlConnection.Open();
                    // To Check
                    m_objConnection = (IDbConnection)sqlConnection;
                    sqlTransaction = (SqlTransaction)
                        m_objConnection.BeginTransaction();
                    //To check
                    m_objTransaction = (IDbTransaction)sqlTransaction;

                }
                return sqlTransaction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //-------------------------------------------------------------------
        // public BeginTransaction()
        //------------------------------------------------------------------- 
        /// <summary>
        /// This function will Commit Transaction.
        /// 
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                if (m_objTransaction != null)
                {
                    m_objTransaction.Commit();
                }
                else
                {
                    m_objConnection.BeginTransaction();
                    m_objTransaction.Commit();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (m_objConnection != null)
                {
                    m_objConnection.Close();
                    m_objConnection = null;
                }
            }
        }


        //-------------------------------------------------------------------
        // public RollBackTransaction()
        //------------------------------------------------------------------- 
        /// <summary>
        /// This function will Roll Back Transaction.
        /// 
        /// </summary>
        public void RollBackTransaction()
        {
            try
            {
                m_objTransaction.Rollback();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (m_objConnection != null)
                {
                    m_objConnection.Close();
                    m_objConnection = null;
                }
            }
        }
    }
}
