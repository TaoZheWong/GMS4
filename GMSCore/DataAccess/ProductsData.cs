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
    public class ProductsDataDALC
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
        public ProductsDataDALC()
        {
            cm = new ConnectionManager();
        }	//	end of ContainerDALC()

        #region Methods for ProductsData
        /// <summary>
        /// Retrieve list of products.
        /// </summary>
        public void GetProducts(short companyId, string productCode, string productName, string productGroupCode, string productGroup, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            command.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = productGroupCode;
            command.Parameters.Add("@ProductGroupName", SqlDbType.NVarChar).Value = productGroup;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a particular product.
        /// </summary>
        public void GetProductDetail(short companyId, string productCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductDetailSelectByProductCode", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a particular product.
        /// </summary>
        public void GetProductPrice(short companyId, string productCode, int userID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductPriceSelectByProductCode", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a particular product.
        /// </summary>
        public void GetProductParticular(short companyId, string productCode, int userID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductParticularSelectByProductCode", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            command.Parameters.Add("@UserNumID", SqlDbType.Int).Value = userID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of products for auto complete
        /// </summary>
        public void GetProductDetailForAutoComplete(short companyId, string productCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductDetailSelectByProductCodeForAutoComplete", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of products for auto complete
        /// </summary>
        public void GetProductDetailByNameForAutoComplete(short companyId, string productName, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductDetailSelectByProductNameForAutoComplete", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = "%" + productName + "%";
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of stock status of a product.
        /// </summary>
        public void GetProductStockStatus(short companyId, string productCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductStockStatusSelectByProductCode", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of product managers.
        /// </summary>
        public void GetProductManagers(short companyId, short userNumId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductManagerWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userNumId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// Retrieve list of product managers for MR.
        /// </summary>
        public void GetProductManagersForMR(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductManagerWildcardSelectForMR", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;           
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of product purchase.
        /// </summary>
        public void GetProductPurchase(short companyId, DateTime dateFrom, DateTime dateTo, string supplierAccountCode, string supplierAccountName, 
                                    string productCode, string productName, string productGroup, string PMUserID, short userNumId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductPurchaseWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@TrnDateFrom", SqlDbType.DateTime).Value = dateFrom;
            command.Parameters.Add("@TrnDateTo", SqlDbType.DateTime).Value = dateTo;
            command.Parameters.Add("@SupplierAccountCode", SqlDbType.NVarChar).Value = supplierAccountCode;
            command.Parameters.Add("@SupplierAccountName", SqlDbType.NVarChar).Value = supplierAccountName;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            command.Parameters.Add("@ProductGroupName", SqlDbType.NVarChar).Value = productGroup;
            command.Parameters.Add("@PMUserID", SqlDbType.NVarChar).Value = PMUserID;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userNumId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of stock movements of a product.
        /// </summary>
        public void GetProductStockMovement(short companyId, string productCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductStockMovementSelectByProductCode", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of salesman.
        /// </summary>
        public void GetSalesman(short companyId, short userNumId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSalesmanWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userNumId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of salesman.
        /// </summary>
        public void GetSalesmanByUserID(short companyId, short userNumId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSalesmanByUserIDWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userNumId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve list of sales.
        /// </summary>
        public void GetSalesByWildcardSelect(short companyId, DateTime dateFrom, DateTime dateTo, string supplierAccountCode, string supplierAccountName,
                                    string productCode, string productName, string productGroup, string productGroupCode, string PMUserID, string salesmanID, short userNumId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSalesWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@TrnDateFrom", SqlDbType.DateTime).Value = dateFrom;
            command.Parameters.Add("@TrnDateTo", SqlDbType.DateTime).Value = dateTo;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = supplierAccountCode;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = supplierAccountName;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            command.Parameters.Add("@ProductGroupName", SqlDbType.NVarChar).Value = productGroup;
            command.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = productGroupCode;
            command.Parameters.Add("@PMUserID", SqlDbType.NVarChar).Value = PMUserID;
            command.Parameters.Add("@SalesmanID", SqlDbType.NVarChar).Value = salesmanID;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userNumId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a sales transaction detail base on primary keys.
        /// </summary>
        public void GetSalesTransactionByPrimanyKey(short companyId, string trnType, int trnNo, short srNo, string prodCode, string accountCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppSalesTransactionSelectByKey", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@TrnType", SqlDbType.NVarChar).Value = trnType;
            command.Parameters.Add("@TrnNo", SqlDbType.Int).Value = trnNo;
            command.Parameters.Add("@SrNo", SqlDbType.SmallInt).Value = srNo;
            command.Parameters.Add("@ProdCode", SqlDbType.NVarChar).Value = prodCode;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of products for auto complete
        /// </summary>
        public void GetProductPackageByNameForAutoComplete(short companyId, string productName, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductPackageSelectByProductNameForAutoComplete", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = "%" + productName + "%";
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
       

        /// <summary>
        /// Retrieve a list of products group
        /// </summary>
        public void GetProductGroup(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductGroupSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;           
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a particular receipe Gas Price.
        /// </summary>
        public void GetRecipeGasPrice(short companyId, string recipeNo, string accountCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppRecipeGasPriceSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@RecipeNo", SqlDbType.NVarChar).Value = recipeNo;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;             
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        #endregion

        /// <summary>
        /// Retrieve a list of products group without short name
        /// </summary>
        public void GetProductGroupWithNoShortName(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductGroupWithNoShortNameSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of products group short name
        /// </summary>
        public void GetProductGroupWithShortName(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductGroupWithShortNameSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of products short name
        /// </summary>
        public void GetProductWithShortName(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductWithShortNameSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of products without short name
        /// </summary>
        public void GetProductWithNoShortName(short companyId, string productGroupCode, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductWithNoShortNameSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = productGroupCode;        
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of products group in Product
        /// </summary>
        public void GetProductGroupInProduct(short companyId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductGroupInProductSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        /// <summary>
        /// Retrieve a list of products code, short name for search
        /// </summary>
        public void GetProductCodeWithShortName(short companyId, string ProductCode, string ProductName, string ShortName, string Brand, ref DataSet dsProducts)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductCodeWithShortNameSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = ProductCode;
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = ProductName;
            command.Parameters.Add("@ShortName", SqlDbType.NVarChar).Value = ShortName;
            command.Parameters.Add("@Brand", SqlDbType.NVarChar).Value = Brand;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dsProducts);
            return;
        }

        /// <summary>
        /// Retrieve a list of products group code, short name for search
        /// </summary>
        public void GetProductGroupCodeWithShortName(short companyId, string ProductGroupCode, string ProductGroupName, string ShortName, ref DataSet dsProducts)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductGroupCodeWithShortNameSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = ProductGroupCode;
            command.Parameters.Add("@ProductGroupName", SqlDbType.NVarChar).Value = ProductGroupName;
            command.Parameters.Add("@ShortName", SqlDbType.NVarChar).Value = ShortName;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dsProducts);
            return;
        }

        public void GetProductByBrand(short companyId, int brand, ref DataSet dsProducts)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppProductCodeNameSelectByBrand", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyId;
            command.Parameters.Add("@Brand", SqlDbType.SmallInt).Value = brand;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dsProducts);
            return;
        }
    }
}
