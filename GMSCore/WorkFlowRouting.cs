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
    public class WorkFlowRouting
    {
        private ConnectionManager cm;
        public WorkFlowRouting()
        {
            cm = new ConnectionManager();
        }

        private DataTable ConstructRoutingTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Level");
            dt.Columns.Add("Role");
            dt.Columns.Add("User");
            dt.Columns.Add("Approved");
            dt.Columns.Add("Rejected");
            return dt;
        }


        public DataTable GenerateRoutingTable_Default(Array Users)
        {
            DataTable dt = ConstructRoutingTable();
            foreach (int User in Users)
            {
                if (User != 0)
                {
                    DataRow Row = dt.NewRow();

                    Row["User"] = User;
                    Row["Approved"] = "NEXT";
                    Row["Rejected"] = "END";

                    if (dt.Rows.Count == 0)
                    {
                        Row["Level"] = 0;
                        Row["Role"] = "Submitter";
                    }
                    else
                    {
                        Row["Level"] = dt.Rows.Count;
                        Row["Role"] = "Approval " + dt.Rows.Count;
                    }
                    dt.Rows.Add(Row);
                }
            }

            return dt;
        }

        public DataTable GenerateRoutingTable_MR(Array Users)
        {
            DataTable dt = ConstructRoutingTable();
            foreach (int User in Users)
            {
                if (User != 0)
                {
                    DataRow Row = dt.NewRow();

                    Row["User"] = User;
                    Row["Approved"] = "NEXT";
                    Row["Rejected"] = "RESTART";

                    if (dt.Rows.Count == 0)
                    {
                        Row["Level"] = 0;
                        Row["Role"] = "Submitter";
                    }
                    else
                    {
                        Row["Level"] = dt.Rows.Count;
                        Row["Role"] = "Approval " + dt.Rows.Count;
                    }
                    dt.Rows.Add(Row);
                }
            }

            return dt;
        }
    }
}
