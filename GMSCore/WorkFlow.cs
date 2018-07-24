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
    public class WorkFlow
    {
        private ConnectionManager cm;

        public WorkFlow()
        {
            cm = new ConnectionManager();
        }

        #region InitiateRouteGroup
        private string InitiateRouteGroup(int PUser, string PApplication, DataTable WFRouting)
        {
            //Return RGID - Success
            //Return String.Empty - Failed
            string RGID = string.Empty;
            
            if (WFRouting != null)
            {
                if (WFRouting.Rows.Count > 0)
                {
                    //SP Create New Route Grouping and return RGID
                    try
                    {
                        RGID = PApplication + "_" + PUser + "_" + DateTime.Now.ToString("yyMMddhhmmss");
                        foreach (DataRow dr in WFRouting.Rows)
                        {
                            IDbConnection conn = cm.GetConnection();
                            SqlCommand command = new SqlCommand("procAppWFRouting_Insert", (SqlConnection)conn);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add("@RGID", SqlDbType.NVarChar).Value = RGID;
                            command.Parameters.Add("@RLevel", SqlDbType.Int).Value = Convert.ToInt32(dr["Level"].ToString());
                            command.Parameters.Add("@RRole", SqlDbType.NVarChar).Value = dr["Role"].ToString();
                            command.Parameters.Add("@RUser", SqlDbType.Int).Value = Convert.ToInt32(dr["User"].ToString());
                            command.Parameters.Add("@RApproved", SqlDbType.NVarChar).Value = dr["Approved"].ToString();
                            command.Parameters.Add("@RRejected", SqlDbType.NVarChar).Value = dr["Rejected"].ToString();

                            conn.Open();
                            command.ExecuteNonQuery();

                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    catch
                    {
                        RGID = string.Empty;
                    }
                }
            }
            return RGID;
        }
        #endregion

        #region InitiateProcess
        public Guid InitiateProcessInstance(int User, string Application, DataTable RoutingTable)
        {
            Guid PIID = Guid.Empty;
            string RGID = string.Empty;

            //Generate RouteGroup
            RGID = InitiateRouteGroup(User, Application, RoutingTable);

            if (!string.IsNullOrEmpty(RGID))
            {
                //SP create new process and return PIID
                try
                {
                    IDbConnection conn = cm.GetConnection();
                    SqlCommand command = new SqlCommand("procAppWFProcessInstance_Insert", (SqlConnection)conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@RGID", SqlDbType.NVarChar).Value = RGID;
                    command.Parameters.Add("@PName", SqlDbType.NVarChar).Value = Application + "_" + User + "_" + DateTime.Now.ToString("yyMMddhhmmss");
                    command.Parameters.Add("@PUser", SqlDbType.Int).Value = User;
                    command.Parameters.Add("@PApplication", SqlDbType.NVarChar).Value = Application;
                    command.Parameters.Add("@PIID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;

                    conn.Open();
                    command.ExecuteScalar();
                    PIID = (Guid)command.Parameters["@PIID"].Value;
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception ex)
                {
                    PIID = Guid.Empty;
                }
            }

            return PIID;
        }
        #endregion

        #region CompleteWorkItem
        public int CompleteWorkItem(Guid WIID, int User, int OnBehalfUser, string Action, string Remark, bool IsApproved, bool IsWithdrawn, bool IsByPassMode)
        {
            //MSG >= 1 MEANS NUMBER OF USER
            //MSG = 0 MEANS NO MORE APPROVAL
            //MSG =-1 MEANS NO PIID FOUND
            //MSG =-2 MEANS calculation error
            //MSG =-3 MEANS NO WIID FOUND
            //MSG =-4 MEANS SQL ERROR

            int MSG = -3;
            if (WIID!=Guid.Empty)
            {
                
                string WStatus = string.Empty;
                if (!IsWithdrawn)
                {
                    WStatus = "COMPLETED";
                }
                else
                {
                    WStatus = "CANCELLED";
                }

                try
                {
                    IDbConnection conn = cm.GetConnection();
                    SqlCommand command = new SqlCommand("procAppWFWorkItem_Update", (SqlConnection)conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@WIID", SqlDbType.UniqueIdentifier).Value = WIID;
                    command.Parameters.Add("@WPerformedUser", SqlDbType.Int).Value = User;
                    command.Parameters.Add("@OnBehalfUser", SqlDbType.Int).Value = OnBehalfUser;
                    command.Parameters.Add("@WAction", SqlDbType.NVarChar).Value = Action;
                    command.Parameters.Add("@WRemark", SqlDbType.NVarChar).Value = Remark;
                    command.Parameters.Add("@WStatus", SqlDbType.NVarChar).Value = WStatus;
                    command.Parameters.Add("@WIsApproved", SqlDbType.Bit).Value = IsApproved;
                    command.Parameters.Add("@IsByPassMode", SqlDbType.Bit).Value = IsByPassMode;
                    command.Parameters.Add("@NumApproval", SqlDbType.Int).Direction = ParameterDirection.Output;

                    conn.Open();
                    command.ExecuteScalar();
                    MSG = Convert.ToInt32 (command.Parameters["@NumApproval"].Value);
                    conn.Close();
                    conn.Dispose();
                }
                catch
                {
                    MSG = -4;
                }
            }
            return MSG;
        }
        #endregion

        public Guid ResetCurrentApproval(Guid PIID, int usernum)
        {
            Guid WIID = Guid.Empty;
            try
            {
                IDbConnection conn = cm.GetConnection();
                SqlCommand command = new SqlCommand("procAppWFWorkItem_Reset", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@PIID", SqlDbType.UniqueIdentifier).Value = PIID;
                command.Parameters.Add("@usernum", SqlDbType.Int).Value = usernum;
                command.Parameters.Add("@WIID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                conn.Open();
                command.ExecuteScalar();
                WIID = (Guid)command.Parameters["@WIID"].Value;
                conn.Close();
                conn.Dispose();
            }
            catch
            {
                WIID = Guid.Empty;
            }
            return WIID;
        }

        #region GetInfo
        public Guid GetPendingWorkItem(Guid PIID)
        {
            Guid WIID = Guid.Empty;
            if (PIID != Guid.Empty)
            {
                try
                {
                    IDbConnection conn = cm.GetConnection();
                    SqlCommand command = new SqlCommand("procAppWFWorkItem_Select", (SqlConnection)conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@PIID", SqlDbType.UniqueIdentifier).Value = PIID;
                    command.Parameters.Add("@WIID", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    conn.Open();
                    command.ExecuteScalar();
                    WIID = (Guid)command.Parameters["@WIID"].Value;
                    conn.Close();
                    conn.Dispose();
                }
                catch
                {
                    WIID = Guid.Empty;
                }
            }
            return WIID;
        }
 
        public DataTable GetWorkItemByProcess(Guid PIID)
        {
            DataTable dt = new DataTable();
            if (PIID != Guid.Empty)
            {
                try
                {
                    IDbConnection conn = cm.GetConnection();
                    SqlCommand command = new SqlCommand("procAppWFWorkItemByProcess_Select", (SqlConnection)conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@PIID", SqlDbType.UniqueIdentifier).Value = PIID;
                    conn.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        dt.Load(dr);
                        dr.Dispose();
                    }
                    conn.Close();
                    conn.Dispose();
                }
                catch
                {
                    dt = null;
                }
            }
            return dt;
        }

        public DataTable GetWorkItemInfo(Guid WIID)
        {
            DataTable dt = new DataTable();
            if (WIID != Guid.Empty)
            {
                try
                {
                    IDbConnection conn = cm.GetConnection();
                    SqlCommand command = new SqlCommand("procAppWFWorkItemInfo_Select", (SqlConnection)conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@WIID", SqlDbType.UniqueIdentifier).Value = WIID;
                    conn.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        dt.Load(dr);
                        dr.Dispose();
                    }
                    conn.Close();
                    conn.Dispose();
                }
                catch
                {
                    dt = null;
                }
            }
            return dt;
        }
        #endregion
    }
}
