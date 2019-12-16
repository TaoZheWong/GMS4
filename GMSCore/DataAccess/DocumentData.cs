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
    public class DocumentDataDALC
    {
        private ConnectionManager cm;

        public DocumentDataDALC()
        {
            cm = new ConnectionManager();
        }	

        #region Methods for DocumentData
        /// <summary>
        /// Retrieve active documents
        /// </summary>
        public void GetActiveDocuments(short documentCategoryID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppDocumentSelectByDocumentCategoryID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@DocumentCategoryID", SqlDbType.SmallInt).Value = documentCategoryID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        #endregion

        public void GetActivePriceList(short documentCategoryID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppPriceListDocumentSelectByDocumentCategoryID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@DocumentCategoryID", SqlDbType.SmallInt).Value = documentCategoryID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        #region
        /// <summary>
        /// Retrieve archived documents
        /// </summary>
        public void GetArchivedDocuments(short documentID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppArchivedDocumentSelectByDocumentID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@DocumentID", SqlDbType.SmallInt).Value = documentID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        #endregion
    }
}
