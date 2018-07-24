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
    /// Summary description for ProductsDataDALC.
    /// </summary>
    public class QuotationDataDALC
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
        public QuotationDataDALC()
        {
            cm = new ConnectionManager();
        }	//	end of ContainerDALC()

        /// <summary>
        /// Retrieve list of quotations.
        /// </summary>
        public void GetQuotationByWildcardSelect(short companyId, DateTime dateFrom, DateTime dateTo, string accountCode, string accountName,
                                    string prodCode, string prodName, string salesmanID, short userNumId, string quotationStatusID, string quotationNo, string acknowledge, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppQuotationWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@TrnDateFrom", SqlDbType.DateTime).Value = dateFrom;
            command.Parameters.Add("@TrnDateTo", SqlDbType.DateTime).Value = dateTo;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            command.Parameters.Add("@SalesmanID", SqlDbType.NVarChar).Value = salesmanID;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userNumId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = prodCode;
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = prodName;
            command.Parameters.Add("@QuotationStatusID", SqlDbType.NVarChar).Value = quotationStatusID;
            command.Parameters.Add("@QuotationNo", SqlDbType.NVarChar).Value = quotationNo;
            command.Parameters.Add("@Acknowledge", SqlDbType.NVarChar).Value = acknowledge;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve quotation detail by quotation number.
        /// </summary>
        public void GetQuotationDetailByQuotationNoSelect(short companyId, string quotationNo, int userID, byte revisionNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppQuotationDetailSelectByQuotationNo", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@QuotationNo", SqlDbType.NVarChar).Value = quotationNo;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userID;
            command.Parameters.Add("@RevisionNo", SqlDbType.TinyInt).Value = revisionNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve quotation detail by quotation number.
        /// </summary>
        public void GetAccountsByWildcardSelect(short companyId, string accountcode, string accountName, int userID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppAccountsWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountcode;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userID; 
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        /// <summary>
        /// Retrieve quotation detail by quotation number.
        /// </summary>
        public void GetSupplierByWildcardSelect(short companyId, string accountcode, string accountName, int userID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSupplierWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountcode;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        

        /// <summary>
        /// Retrieve a list of T&C for auto complete
        /// </summary>
        public void GetTNCByNameForAutoComplete(string name, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppTNCSelectByNameForAutoComplete", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = "%" + name + "%";
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve quotation detail by quotation number.
        /// </summary>
        public void GetQuotationTNCByQuotationNoSelect(short companyId, string quotationNo, byte revisionNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppQuotationTNCSelectByQuotationNo", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@QuotationNo", SqlDbType.NVarChar).Value = quotationNo;
            command.Parameters.Add("@RevisionNo", SqlDbType.TinyInt).Value = revisionNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve quotation detail by quotation number.
        /// </summary>
        public void GetQuotationAttachmentByQuotationNoSelect(short companyId, string quotationNo, byte revisionNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppQuotationAttachmentSelectByQuotationNo", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@QuotationNo", SqlDbType.NVarChar).Value = quotationNo;
            command.Parameters.Add("@RevisionNo", SqlDbType.TinyInt).Value = revisionNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve quotation detail by quotation number.
        /// </summary>
        //public void GetRecipeDataSelect(short companyId, string recipeNo, ref DataSet ds)
        //{
        //    IDbConnection conn = cm.GetConnection();
        //    SqlCommand command = new SqlCommand("procAppRecipeDataSelect", (SqlConnection)conn);
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
        //    command.Parameters.Add("@RecipeNo", SqlDbType.NVarChar).Value = recipeNo;            
        //    SqlDataAdapter adapter = new SqlDataAdapter(command);
        //    adapter.Fill(ds);
        //    return;
        //}

        /// <summary>
        /// Retrieve Special Condition.
        /// </summary>
        public void GetQuotationSpecialConditionsByQuotationNoSelect(short companyId, string quotationNo,ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppQuotationSpecialConditionsSelectByQuotationNo", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@QuotationNo", SqlDbType.NVarChar).Value = quotationNo;           
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }



        /// <summary>
        /// Retrieve Product Head ID by product code.
        /// </summary>
        public void GetPHUserIDByProductCodeSelect(short companyId, string productCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppPHUserIDSelectByProductCode", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve transaction approval by quotation number.
        /// </summary>
        public void GetTransactionApprovalByQuotationNoSelect(short companyId, string quotationNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppTransactionApprovalSelectByQuotationNo", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@QuotationNo", SqlDbType.NVarChar).Value = quotationNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve transaction approval by quotation number.
        /// </summary>
        public void GetTransactionApprovalWithLimitByQuotationNoSelect(short companyId, string quotationNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppTransactionApprovalWithLimitSelectByQuotationNo", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@QuotationNo", SqlDbType.NVarChar).Value = quotationNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve approval party by quotation amount and salesman ID.
        /// </summary>
        public void GetQuotationApprovalParty(short companyId, decimal quotationTotal, string salesPersonID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppQuotationApprovalPartySelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@QuotationTotal", SqlDbType.Decimal).Value = quotationTotal;
            command.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = salesPersonID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve quotation detail by quotation number.
        /// </summary>
        public void GetProductsByWildcardSelect(short companyId, string productcode, string productName, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductsWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productcode;
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve quotation detail by quotation number.
        /// </summary>
        public void GetProductsByWildcardSelectByAccountCode(short companyId, string productcode, string productName, string accountCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductsWildcardSelectByAccountCode", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productcode;
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve quotation detail by quotation number.
        /// </summary>
        public void GetProductsByWildcardSelectForMR(short companyId, string productcode, string productName, string accountCode, short pm, short ph, short ph3,ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductsWildcardSelectForMR", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productcode;
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            command.Parameters.Add("@PM", SqlDbType.SmallInt).Value = pm;
            command.Parameters.Add("@PH", SqlDbType.SmallInt).Value = ph;
            command.Parameters.Add("@PH3", SqlDbType.SmallInt).Value = ph3;
           
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve all revisions by quotation number.
        /// </summary>
        public void GetAllRevisionsByQuotationNoSelect(short companyId, string quotationNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppQuotationRevisionSelectByQuotationNo", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@QuotationNo", SqlDbType.NVarChar).Value = quotationNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve quotation detail by quotation number.
        /// </summary>
        public void GetProductPackageByWildcardSelect(short companyId, string productcode, string productName, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductPackageWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productcode;
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve customer info by AccountCode.
        /// </summary>
        public void GetCustomerInfoByAccountCode(short companyId, string accountCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppCustomerInfoSelectByAccountCode", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve quotation detail by new customer name by salespersonid.
        /// </summary>
        public void GetCustomerInfoByNewCustomerNameBySalesPersonID(short companyId, string salespersonId, string accountName, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppQuotationByNewCustomerNameBySalesPersonID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = salespersonId;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve quotation detail by quotation number.
        /// </summary>
        public void GetAllUOMByCoyIDSelect(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppUOMSelectByCoyID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        ///Added By Kim on 23 Sept 2013
        /// <summary>
        /// UpdateByPassMRFormApprovalByLevel
        /// </summary>
        public void UpdateQuotationSpecialConditionsByQuotationNoAndConditionName(short companyId, string quotationNo, string conditionName, string conditionValue, int seq)
        {
            IDbConnection conn = cm.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("procAppQuotationSpecialConditionsByQuotationNoAndConditionNameUpdate", (SqlConnection)conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
                command.Parameters.Add("@QuotationNo", SqlDbType.NVarChar).Value = quotationNo;
                command.Parameters.Add("@ConditionName", SqlDbType.NVarChar).Value = conditionName;
                command.Parameters.Add("@ConditionValue", SqlDbType.NVarChar).Value = conditionValue;
                command.Parameters.Add("@Seq", SqlDbType.Int).Value = seq;
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
        /// DeleteRecipeDataByRecipeNo
        /// </summary>
        //public void DeleteRecipeDataByRecipeNo(short companyId, string recipeNo)
        //{
        //    IDbConnection conn = cm.GetConnection();
        //    SqlDataReader rdr = null;
        //    try
        //    {
        //        conn.Open();
        //        SqlCommand command = new SqlCommand("procAppRecipeDataDelete", (SqlConnection)conn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
        //        command.Parameters.Add("@RecipeNo", SqlDbType.NVarChar).Value = recipeNo;                
        //        rdr = command.ExecuteReader();
        //    }
        //    finally
        //    {
        //        if (conn != null)
        //        {
        //            conn.Close();
        //        }
        //        if (rdr != null)
        //        {
        //            rdr.Close();
        //        }
        //    }

        //    return;
        //}

        /// <summary>
        /// Retrieve Account Address .
        /// </summary>
        public void GetAccountAddressByAccountCodeSelect(short companyId, string accountcode, int userID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppAccountAddressByAccountCodeSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountcode;           
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve quotation detail by quotation number.
        /// </summary>
        public void GetProductExtendedDescriptionSelect(short companyId, string prodCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductExtendedDescSelectByCoyIDAndProdCode", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = prodCode; 
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        
        
    }
}
