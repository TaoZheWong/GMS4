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
    public class EngineeringDataDALC
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
        public EngineeringDataDALC()
        {
            cm = new ConnectionManager();
        }	//	end of ContainerDALC()

         //<summary>
         //Retrieve list of ProjectNo.
         //</summary>
        public void GetProjectInformation(short companyid, string projectno, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetProjectInformation", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = projectno;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        public void GetProjectNo(short companyid, string projectno, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetProjectNo", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = projectno;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetAttachmentList(short companyid, string projectno, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetProjectAttachment", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = projectno;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetPrevProjectNo(short companyid, string prevprojectno, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetPrevProjectNo", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = prevprojectno;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetAccountList(short companyid, string account, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetAccountList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@Account", SqlDbType.NVarChar).Value = account;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        
        public void GetAccountAddressList(short companyid, string account, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetAccountAddressList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@Account", SqlDbType.NVarChar).Value = account;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetAccountBillingAddressList(short companyid, string account, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetAccountBillingAddressList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@Account", SqlDbType.NVarChar).Value = account;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetEngineerList(short companyid, string engineer, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetEngineerList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@Engineer", SqlDbType.NVarChar).Value = engineer;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetSalesPersonList(short companyid, string salesperson, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetSalesPersonList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@SalesPerson", SqlDbType.NVarChar).Value = salesperson;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetStatusList(ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetStatusList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCurrencyList(ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetCurrencyList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCurrencyRate(string projectno, string currencycode, string type, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetCurrencyRate", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = projectno;
            command.Parameters.Add("@ExchangeCurrencyCode", SqlDbType.NVarChar).Value = currencycode;
            command.Parameters.Add("@Type", SqlDbType.NVarChar).Value = type;
            adapter.Fill(ds);
            return;
        }

        public void GetProjectList(short companyid, string projectno, string prevprojectno, string accountcode, string accountname, string customerpo, string engineerid,
            string salespersonid, string isbillable, string isprogressiveclaim, string statusid, DateTime createddatefrom, DateTime createddateto,
            DateTime commencementdatefrom, DateTime commencementdateto, DateTime closingdatefrom, DateTime closingdateto, short usernumid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection(); 
            SqlCommand command = new SqlCommand("procEngineeringGetProjectList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = projectno;
            command.Parameters.Add("@PrevProjectNo", SqlDbType.NVarChar).Value = prevprojectno;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountcode;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountname;
            command.Parameters.Add("@CustomerPO", SqlDbType.NVarChar).Value = customerpo;
            command.Parameters.Add("@EngineerID", SqlDbType.NVarChar).Value = engineerid;
            command.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = salespersonid;
            command.Parameters.Add("@IsBillable", SqlDbType.NVarChar).Value = isbillable;
            command.Parameters.Add("@IsProgressiveClaim", SqlDbType.NVarChar).Value = isprogressiveclaim;
            command.Parameters.Add("@StatusID", SqlDbType.NVarChar).Value = statusid;
            command.Parameters.Add("@CreatedDateFrom", SqlDbType.DateTime).Value = createddatefrom;
            command.Parameters.Add("@CreatedDateTo", SqlDbType.DateTime).Value = createddateto;
            command.Parameters.Add("@CommencementDateFrom", SqlDbType.DateTime).Value = commencementdatefrom;
            command.Parameters.Add("@CommencementDateTo", SqlDbType.DateTime).Value = commencementdateto;
            command.Parameters.Add("@ClosingDateFrom", SqlDbType.DateTime).Value = closingdatefrom;
            command.Parameters.Add("@ClosingDateTo", SqlDbType.DateTime).Value = closingdateto;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = usernumid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void InsertProjectInformation(short companyid, string PrevProjectNo, string AccountCode, string AccountName, string QuotationNo, string RefNo, string StatusID,
            string SalesPersonID, string EngineerID, Nullable<bool> IsBillable, Nullable<bool> IsProgressiveClaim, string CurrencyCode, decimal TotalBillableAmount,
            string ContractNo, DateTime ContractDateFrom, DateTime ContractDateTo, DateTime CommencementDate, DateTime CompletionDate, 
            DateTime ClosingDate, string CustomerPO, string CustomerPIC, string OfficePhone, string Fax, string BillingAddress, string OnsiteLocation, string Description, 
            string Remarks, short UserID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringInsertProjectInformation", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@PrevProjectNo", SqlDbType.NVarChar).Value = PrevProjectNo;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = AccountCode;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = AccountName;
            command.Parameters.Add("@QuotationNo", SqlDbType.NVarChar).Value = QuotationNo;
            command.Parameters.Add("@RefNo", SqlDbType.NVarChar).Value = RefNo;
            command.Parameters.Add("@StatusID", SqlDbType.NVarChar).Value = StatusID;
            command.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = SalesPersonID;
            command.Parameters.Add("@EngineerID", SqlDbType.NVarChar).Value = EngineerID;
            command.Parameters.Add("@IsBillable", SqlDbType.Bit).Value = IsBillable;
            command.Parameters.Add("@IsProgressiveClaim", SqlDbType.Bit).Value = IsProgressiveClaim;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@TotalBillableAmount", SqlDbType.Money).Value = TotalBillableAmount;
            command.Parameters.Add("@ContractNo", SqlDbType.NVarChar).Value = ContractNo;
            command.Parameters.Add("@ContractDateFrom", SqlDbType.DateTime).Value = ContractDateFrom;
            command.Parameters.Add("@ContractDateTo", SqlDbType.DateTime).Value = ContractDateTo;
            command.Parameters.Add("@CommencementDate", SqlDbType.DateTime).Value = CommencementDate;
            command.Parameters.Add("@CompletionDate", SqlDbType.DateTime).Value = CompletionDate;
            command.Parameters.Add("@ClosingDate", SqlDbType.DateTime).Value = ClosingDate;
            command.Parameters.Add("@CustomerPO", SqlDbType.NVarChar).Value = CustomerPO;
            command.Parameters.Add("@CustomerPIC", SqlDbType.NVarChar).Value = CustomerPIC;
            command.Parameters.Add("@OfficePhone", SqlDbType.NVarChar).Value = OfficePhone;
            command.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = Fax;
            command.Parameters.Add("@BillingAddress", SqlDbType.NVarChar).Value = BillingAddress;
            command.Parameters.Add("@OnsiteLocation", SqlDbType.NVarChar).Value = OnsiteLocation;
            command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = UserID;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void UpdateProjectInformation(short companyid, string ProjectNo, string PrevProjectNo, string AccountCode, string AccountName, string QuotationNo, string RefNo, string StatusID,
            string SalesPersonID, string EngineerID, Nullable<bool> IsBillable, Nullable<bool> IsProgressiveClaim, string CurrencyCode, decimal TotalBillableAmt,
            string ContractNo, DateTime ContractDateFrom, DateTime ContractDateTo, DateTime CommencementDate, DateTime CompletionDate,
            DateTime ClosingDate, string CustomerPO, string CustomerPIC, string OfficePhone, string Fax, string BillingAddress, string OnsiteLocation, string Description,
            string Remarks, short UserID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringUpdateProjectInformation", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@PrevProjectNo", SqlDbType.NVarChar).Value = PrevProjectNo;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = AccountCode;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = AccountName;
            command.Parameters.Add("@QuotationNo", SqlDbType.NVarChar).Value = QuotationNo;
            command.Parameters.Add("@RefNo", SqlDbType.NVarChar).Value = RefNo;
            command.Parameters.Add("@StatusID", SqlDbType.NVarChar).Value = StatusID;
            command.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = SalesPersonID;
            command.Parameters.Add("@EngineerID", SqlDbType.NVarChar).Value = EngineerID;
            command.Parameters.Add("@IsBillable", SqlDbType.Bit).Value = IsBillable;
            command.Parameters.Add("@IsProgressiveClaim", SqlDbType.Bit).Value = IsProgressiveClaim;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@TotalBillableAmt", SqlDbType.Money).Value = TotalBillableAmt;
            command.Parameters.Add("@ContractNo", SqlDbType.NVarChar).Value = ContractNo;
            command.Parameters.Add("@ContractDateFrom", SqlDbType.DateTime).Value = ContractDateFrom;
            command.Parameters.Add("@ContractDateTo", SqlDbType.DateTime).Value = ContractDateTo;
            command.Parameters.Add("@CommencementDate", SqlDbType.DateTime).Value = CommencementDate;
            command.Parameters.Add("@CompletionDate", SqlDbType.DateTime).Value = CompletionDate;
            command.Parameters.Add("@ClosingDate", SqlDbType.DateTime).Value = ClosingDate;
            command.Parameters.Add("@CustomerPO", SqlDbType.NVarChar).Value = CustomerPO;
            command.Parameters.Add("@CustomerPIC", SqlDbType.NVarChar).Value = CustomerPIC;
            command.Parameters.Add("@OfficePhone", SqlDbType.NVarChar).Value = OfficePhone;
            command.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = Fax;
            command.Parameters.Add("@BillingAddress", SqlDbType.NVarChar).Value = BillingAddress;
            command.Parameters.Add("@OnsiteLocation", SqlDbType.NVarChar).Value = OnsiteLocation;
            command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = UserID;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCostEstimateList(short companyid, string ceid, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetCostEstimateList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = ceid;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetUserAccessList(short companyid, string DocNo, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCheckUserAccess", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@DocNo", SqlDbType.NVarChar).Value = DocNo;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCostEstimateListByCEID(short companyid, string ceid, int revision, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetCostEstimateListByCEID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = ceid;
            command.Parameters.Add("@Revision", SqlDbType.Int).Value = revision;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void InsertCEDetails(short companyid, string ProjectNo, string CEID, string ItemName, string ItemBrand, string ItemMaterial, string SupplierCode, string SupplierName, string UOM, decimal Quantity, 
            string CurrencyCode, decimal CurrencyRate, decimal QuotedPrice, decimal TotalAmount, string Category, string Remarks, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCostEstimateDetailInsert", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = ItemName;
            command.Parameters.Add("@ItemBrand", SqlDbType.NVarChar).Value = ItemBrand;
            command.Parameters.Add("@ItemMaterial", SqlDbType.NVarChar).Value = ItemMaterial;
            command.Parameters.Add("@SupplierCode", SqlDbType.NVarChar).Value = SupplierCode;
            command.Parameters.Add("@SupplierName", SqlDbType.NVarChar).Value = SupplierName;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@CurrencyRate", SqlDbType.Money).Value = CurrencyRate;
            command.Parameters.Add("@QuotedPrice", SqlDbType.Money).Value = QuotedPrice;
            command.Parameters.Add("@UOM", SqlDbType.NVarChar).Value = UOM;
            command.Parameters.Add("@Quantity", SqlDbType.Float).Value = Quantity;
            command.Parameters.Add("@TotalAmount", SqlDbType.Money).Value = TotalAmount;
            command.Parameters.Add("@Category", SqlDbType.NVarChar).Value = Category;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void UpdateCEDetails(short companyid, string ProjectNo, string CEID, string CEDetailID, string ItemName, string ItemBrand, string ItemMaterial, string SupplierCode, string SupplierName, string UOM, decimal Quantity,
            string CurrencyCode, decimal CurrencyRate, decimal QuotedPrice, decimal TotalAmount, string Category, string Remarks, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCostEstimateDetailUpdate", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@CEDetailID", SqlDbType.NVarChar).Value = CEDetailID;
            command.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = ItemName;
            command.Parameters.Add("@ItemBrand", SqlDbType.NVarChar).Value = ItemBrand;
            command.Parameters.Add("@ItemMaterial", SqlDbType.NVarChar).Value = ItemMaterial;
            command.Parameters.Add("@SupplierCode", SqlDbType.NVarChar).Value = SupplierCode;
            command.Parameters.Add("@SupplierName", SqlDbType.NVarChar).Value = SupplierName;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@CurrencyRate", SqlDbType.Money).Value = CurrencyRate;
            command.Parameters.Add("@QuotedPrice", SqlDbType.Money).Value = QuotedPrice;
            command.Parameters.Add("@UOM", SqlDbType.NVarChar).Value = UOM;
            command.Parameters.Add("@Quantity", SqlDbType.Float).Value = Quantity;
            command.Parameters.Add("@TotalAmount", SqlDbType.Money).Value = TotalAmount;
            command.Parameters.Add("@Category", SqlDbType.NVarChar).Value = Category;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        public void DeleteCostEstimateDetail(short companyid, string ProjectNo, string CEID, string CEDetailID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCostEstimateDetailDelete", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@CEDetailID", SqlDbType.NVarChar).Value = CEDetailID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetMaterialRequisitionList(short companyid, string ProjectNo, string PrevProjectNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procAppMRDetailByProjectNoSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@PrevProjectNo", SqlDbType.NVarChar).Value = PrevProjectNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetLaborCostList(short companyid, string ProjectNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetLaborCostList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void InsertLaborCost(short companyid, string ProjectNo, string Period, string PIC, string Hour, string CurrencyCode, decimal Rate, decimal Amount, string Remarks, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringLaborCostInsert", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@PIC", SqlDbType.NVarChar).Value = PIC;
            command.Parameters.Add("@Period", SqlDbType.NVarChar).Value = Period;
            command.Parameters.Add("@Hour", SqlDbType.NVarChar).Value = Hour;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@Rate", SqlDbType.Money).Value = Rate;
            command.Parameters.Add("@Amount", SqlDbType.Money).Value = Amount;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void UpdateLaborCost(short companyid, string ProjectNo, short LCID, string Period, string PIC, string Hour, string CurrencyCode, decimal Rate, decimal Amount, string Remarks, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringLaborCostUpdate", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@LCID", SqlDbType.SmallInt).Value = LCID;
            command.Parameters.Add("@PIC", SqlDbType.NVarChar).Value = PIC;
            command.Parameters.Add("@Period", SqlDbType.NVarChar).Value = Period;
            command.Parameters.Add("@Hour", SqlDbType.NVarChar).Value = Hour;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@Rate", SqlDbType.Money).Value = Rate;
            command.Parameters.Add("@Amount", SqlDbType.Money).Value = Amount;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void DeleteLaborCost(short companyid, string ProjectNo, short LCID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringLaborCostDelete", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@LCID", SqlDbType.NVarChar).Value = LCID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetMiscCostList(short companyid, string ProjectNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetMiscCostList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void InsertMiscCost(short companyid, string ProjectNo, string Description, string CurrencyCode, decimal Amount, string Location, string Remarks, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringMiscCostInsert", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@Amount", SqlDbType.Money).Value = Amount;
            command.Parameters.Add("@Location", SqlDbType.NVarChar).Value = Location;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
       
        public void UpdateMiscCost(short companyid, string ProjectNo, short OCID, string Description, string CurrencyCode, decimal Amount, string Location, string Remarks, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringMiscCostUpdate", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@OCID", SqlDbType.SmallInt).Value = OCID;
            command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@Amount", SqlDbType.Money).Value = Amount;
            command.Parameters.Add("@Location", SqlDbType.NVarChar).Value = Location;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void DeleteMiscCost(short companyid, string ProjectNo, short OCID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringMiscCostDelete", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@OCID", SqlDbType.NVarChar).Value = OCID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetPaymentList(short companyid, string ProjectNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetPaymentList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void InsertPaymentList(short companyid, string ProjectNo, string ClaimOrder, Nullable<DateTime> ClaimDate, string CurrencyCode, decimal ClaimAmount, decimal Balance, 
            string Retention, string Ref, string Remarks, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringPaymentListInsert", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@ClaimOrder", SqlDbType.NVarChar).Value = ClaimOrder;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@ClaimDate", SqlDbType.DateTime).Value = ClaimDate;
            command.Parameters.Add("@ClaimAmount", SqlDbType.Money).Value = ClaimAmount;
            command.Parameters.Add("@Balance", SqlDbType.Money).Value = Balance;
            command.Parameters.Add("@Retention", SqlDbType.NVarChar).Value = Retention;
            command.Parameters.Add("@Ref", SqlDbType.NVarChar).Value = Ref;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void UpdatePaymentList(short companyid, string ProjectNo, short PCId, string ClaimOrder, Nullable<DateTime> ClaimDate, string CurrencyCode, decimal ClaimAmount, decimal Balance,
            string Retention, string Ref, string Remarks, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringPaymentListUpdate", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@PCId", SqlDbType.SmallInt).Value = PCId;
            command.Parameters.Add("@ClaimOrder", SqlDbType.NVarChar).Value = ClaimOrder;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@ClaimDate", SqlDbType.DateTime).Value = ClaimDate;
            command.Parameters.Add("@ClaimAmount", SqlDbType.Money).Value = ClaimAmount;
            command.Parameters.Add("@Balance", SqlDbType.Money).Value = Balance;
            command.Parameters.Add("@Retention", SqlDbType.NVarChar).Value = Retention;
            command.Parameters.Add("@Ref", SqlDbType.NVarChar).Value = Ref;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void DeletePaymentList(short companyid, string ProjectNo, short PCId, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringPaymentListDelete", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@PCID", SqlDbType.NVarChar).Value = PCId;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void InsertAttachmentList(short companyid, string ProjectNo, string filename, string filetype, Byte[] size, string extension, string type, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringInsertFileAttachment", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = filename;
            command.Parameters.Add("@Type", SqlDbType.NVarChar).Value = filetype;
            command.Parameters.Add("@Data", SqlDbType.Binary).Value = size;
            command.Parameters.Add("@Extension", SqlDbType.NVarChar).Value = extension;
            command.Parameters.Add("@Category", SqlDbType.NVarChar).Value = type;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

       

        public void GetMaterialList(short companyid, string modelno, string description, string itemcategory, string suppliername, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringMaterialListWildcardSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = modelno;
            command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = description;
            command.Parameters.Add("@ItemCategory", SqlDbType.NVarChar).Value = itemcategory;
            command.Parameters.Add("@SupplierName", SqlDbType.NVarChar).Value = suppliername;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void SaveMaterialInformation(short companyid, string ModelNo, string ItemCategory, string ItemMaterial, 
             string SupplierName, string ItemSize, string ItemBrand, string CurrencyCode, decimal UnitPrice, Nullable<DateTime> QuotationDate, decimal QuotationValidity, string ItemDescription,
             decimal ItemLeadtime, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringMaterialInsertInformation", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = ModelNo;
            //command.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = ItemName;
            command.Parameters.Add("@ItemCategory", SqlDbType.NVarChar).Value = ItemCategory;
            //command.Parameters.Add("@ItemType", SqlDbType.NVarChar).Value = ItemType;
            command.Parameters.Add("@ItemMaterial", SqlDbType.NVarChar).Value = ItemMaterial;
            //command.Parameters.Add("@SupplierCode", SqlDbType.NVarChar).Value = SupplierCode;
            command.Parameters.Add("@SupplierName", SqlDbType.NVarChar).Value = SupplierName;
            command.Parameters.Add("@ItemSize", SqlDbType.NVarChar).Value = ItemSize;
            command.Parameters.Add("@ItemBrand", SqlDbType.NVarChar).Value = ItemBrand;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = UnitPrice;
            command.Parameters.Add("@QuotationDate", SqlDbType.DateTime).Value = QuotationDate;
            command.Parameters.Add("@QuotationValidity", SqlDbType.NVarChar).Value = QuotationValidity;
            command.Parameters.Add("@ItemDescription", SqlDbType.NVarChar).Value = ItemDescription;
            command.Parameters.Add("@ItemLeadtime", SqlDbType.NVarChar).Value = ItemLeadtime;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void EditMaterialInformation(short companyid, string ItemID,string ModelNo, string ItemCategory, string ItemMaterial, string SupplierName, string ItemSize, string ItemBrand, 
            string CurrencyCode, string NewCurrencyCode, decimal UnitPrice, decimal NewUnitPrice, Nullable<DateTime> QuotationDate, Nullable<DateTime> NewQuotationDate, decimal QuotationValidity, 
            decimal NewQuotationValidity, string ItemDescription, decimal ItemLeadtime,  Nullable<Boolean> IsActive, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringMaterialInformationUpdate", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ItemID", SqlDbType.NVarChar).Value = ItemID;
            command.Parameters.Add("@ModelNo", SqlDbType.NVarChar).Value = ModelNo;
            command.Parameters.Add("@ItemCategory", SqlDbType.NVarChar).Value = ItemCategory;
            command.Parameters.Add("@ItemMaterial", SqlDbType.NVarChar).Value = ItemMaterial;
            command.Parameters.Add("@SupplierName", SqlDbType.NVarChar).Value = SupplierName;
            command.Parameters.Add("@ItemSize", SqlDbType.NVarChar).Value = ItemSize;
            command.Parameters.Add("@ItemBrand", SqlDbType.NVarChar).Value = ItemBrand;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@NewCurrencyCode", SqlDbType.NVarChar).Value = NewCurrencyCode;
            command.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = UnitPrice;
            command.Parameters.Add("@NewUnitPrice", SqlDbType.Money).Value = NewUnitPrice;
            command.Parameters.Add("@QuotationDate", SqlDbType.DateTime).Value = QuotationDate;
            command.Parameters.Add("@NewQuotationDate", SqlDbType.DateTime).Value = NewQuotationDate;
            command.Parameters.Add("@QuotationValidity", SqlDbType.NVarChar).Value = QuotationValidity;
            command.Parameters.Add("@NewQuotationValidity", SqlDbType.NVarChar).Value = NewQuotationValidity;
            command.Parameters.Add("@ItemDescription", SqlDbType.NVarChar).Value = ItemDescription;
            command.Parameters.Add("@ItemLeadtime", SqlDbType.NVarChar).Value = ItemLeadtime;
            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetMaterialInformation(short companyid, string ItemID, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringMaterialInformationByMaterialCode", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ItemID", SqlDbType.NVarChar).Value = ItemID;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void MaterialUploadAttachment(short companyid, string ItemID,string FileID,string FileName,string FileDisplayName, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringMaterialUploadAttachment", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ItemID", SqlDbType.NVarChar).Value = ItemID;
            command.Parameters.Add("@FileID", SqlDbType.NVarChar).Value = FileID;
            command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = FileName;
            command.Parameters.Add("@FileDisplayName", SqlDbType.NVarChar).Value = FileDisplayName;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetMaterialAttachment(short companyid, string ItemID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringMaterialGetAttachment", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ItemID", SqlDbType.NVarChar).Value = ItemID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        
        public void GetCostEstimate(short companyid, string CEID, string AccountCode, string AccountName, string CEStatusID, Nullable<DateTime> CreatedDateFrom, Nullable<DateTime> CreatedDateTo, string EngineerName, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetCostEstimate", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = AccountCode;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = AccountName;
            command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = CEStatusID;
            command.Parameters.Add("@CreatedDateFrom", SqlDbType.DateTime).Value = CreatedDateFrom;
            command.Parameters.Add("@CreatedDateTo", SqlDbType.DateTime).Value = CreatedDateTo;
            command.Parameters.Add("@EngineerName", SqlDbType.NVarChar).Value = EngineerName;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCostEstimateInfo(short companyid, string CEID, int Revision, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetCostEstimateInfo", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@Revision", SqlDbType.Int).Value = Revision;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void InsertCostEstimateHeaderInfo(short companyid, string AccountCode, string AccountName, string SalesPersonID, string EngineerID, Nullable<bool> IsBillable, 
            Nullable<bool> IsProgressiveClaim, string CurrencyCode, decimal TotalAmtQuoted, Nullable<DateTime> ContractDateFrom, Nullable<DateTime> ContractDateTo, 
            Nullable<DateTime> CommencementDate, string CustomerPIC, string OfficePhone, string Fax, string BillingAddress, string OnsiteLocation, string Description, string Remarks, 
            int Revision, short UserID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCostEstimateHeaderInsert", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = AccountCode;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = AccountName;
            command.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = SalesPersonID;
            command.Parameters.Add("@EngineerID", SqlDbType.NVarChar).Value = EngineerID;
            command.Parameters.Add("@IsBillable", SqlDbType.Bit).Value = IsBillable;
            command.Parameters.Add("@IsProgressiveClaim", SqlDbType.Bit).Value = IsProgressiveClaim;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@TotalAmtQuoted", SqlDbType.Money).Value = TotalAmtQuoted;
            command.Parameters.Add("@ContractDateFrom", SqlDbType.DateTime).Value = ContractDateFrom;
            command.Parameters.Add("@ContractDateTo", SqlDbType.DateTime).Value = ContractDateTo;
            command.Parameters.Add("@CommencementDate", SqlDbType.DateTime).Value = CommencementDate;
            command.Parameters.Add("@CustomerPIC", SqlDbType.NVarChar).Value = CustomerPIC;
            command.Parameters.Add("@OfficePhone", SqlDbType.NVarChar).Value = OfficePhone;
            command.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = Fax;
            command.Parameters.Add("@BillingAddress", SqlDbType.NVarChar).Value = BillingAddress;
            command.Parameters.Add("@OnsiteLocation", SqlDbType.NVarChar).Value = OnsiteLocation;
            command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@Revision", SqlDbType.Int).Value = Revision;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = UserID;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void UpdateCostEstimateHeaderInfo(short companyid, string CEID, string AccountCode, string AccountName, string CEStatusID,
            string SalesPersonID, string EngineerID, Nullable<bool> IsBillable, Nullable<bool> IsProgressiveClaim, string CurrencyCode, decimal TotalAmtQuoted,
            Nullable<DateTime> ContractDateFrom, Nullable<DateTime> ContractDateTo, Nullable<DateTime> CommencementDate, string CustomerPIC,
            string OfficePhone, string Fax, string BillingAddress, string OnsiteLocation, string Description, string Remarks, int Revision, short UserID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCostEstimateUpdate", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = AccountCode;
            command.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = AccountName;
            command.Parameters.Add("@CEStatusID", SqlDbType.NVarChar).Value = CEStatusID;
            command.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = SalesPersonID;
            command.Parameters.Add("@EngineerID", SqlDbType.NVarChar).Value = EngineerID;
            command.Parameters.Add("@IsBillable", SqlDbType.Bit).Value = IsBillable;
            command.Parameters.Add("@IsProgressiveClaim", SqlDbType.Bit).Value = IsProgressiveClaim;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@TotalAmtQuoted", SqlDbType.Money).Value = TotalAmtQuoted;
            command.Parameters.Add("@ContractDateFrom", SqlDbType.DateTime).Value = ContractDateFrom;
            command.Parameters.Add("@ContractDateTo", SqlDbType.DateTime).Value = ContractDateTo;
            command.Parameters.Add("@CommencementDate", SqlDbType.DateTime).Value = CommencementDate;
            command.Parameters.Add("@CustomerPIC", SqlDbType.NVarChar).Value = CustomerPIC;
            command.Parameters.Add("@OfficePhone", SqlDbType.NVarChar).Value = OfficePhone;
            command.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = Fax;
            command.Parameters.Add("@BillingAddress", SqlDbType.NVarChar).Value = BillingAddress;
            command.Parameters.Add("@OnsiteLocation", SqlDbType.NVarChar).Value = OnsiteLocation;
            command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@Revision", SqlDbType.Int).Value = Revision;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = UserID;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        
        public void GetEngineerInfo(short companyid, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetEngineerInfoByUserID", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void CancelCostEstimate(short companyid, string CEID, string CancelPurpose, string Type, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCostEstimateCancel", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@CancelPurpose", SqlDbType.NVarChar).Value = CancelPurpose;
            command.Parameters.Add("@Type", SqlDbType.NVarChar).Value = Type;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }


        public void ApproveCostEstimate(short companyid, string CEID, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCostEstimateApprove", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void SubmitCostEstimate(short companyid, string CEID, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCostEstimateApprovalRequisition", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void SubmitCostEstimateItem(short companyid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetMaterialItemList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetMaterialItemList(short companyid, string item, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetMaterialItemList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@Item", SqlDbType.NVarChar).Value = item;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetMaterialPriceList(short companyid, string ItemID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetMaterialPriceHistory ", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ItemID", SqlDbType.NVarChar).Value = ItemID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        public void SaveCostEstimateItem(short companyid, string CEID, string ItemName, byte chkIsOthers, string ItemBrand, string ItemMaterial, string SupplierName, string UOM, decimal Quantity,
            string CurrencyCode, decimal CurrencyRate, decimal QuotedPrice, decimal MarkUpPrice, decimal TotalAmount, string Category, string Remarks, string ItemSize, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCostEstimateItemInsert", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = ItemName;
            command.Parameters.Add("@chkIsOthers", SqlDbType.Bit).Value = chkIsOthers;
            command.Parameters.Add("@ItemBrand", SqlDbType.NVarChar).Value = ItemBrand;
            command.Parameters.Add("@ItemMaterial", SqlDbType.NVarChar).Value = ItemMaterial;
            command.Parameters.Add("@SupplierName", SqlDbType.NVarChar).Value = SupplierName;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@CurrencyRate", SqlDbType.Money).Value = CurrencyRate;
            command.Parameters.Add("@QuotedPrice", SqlDbType.Money).Value = QuotedPrice;
            command.Parameters.Add("@MarkUpPrice", SqlDbType.Money).Value = MarkUpPrice;
            command.Parameters.Add("@UOM", SqlDbType.NVarChar).Value = UOM;
            command.Parameters.Add("@Quantity", SqlDbType.Float).Value = Quantity;
            command.Parameters.Add("@TotalAmount", SqlDbType.Money).Value = TotalAmount;
            command.Parameters.Add("@Category", SqlDbType.NVarChar).Value = Category;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@ItemSize", SqlDbType.NVarChar).Value = ItemSize;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void EditCostEstimateItem(short companyid, string CEID, string CEDetailID, string ItemName, byte chkIsOthers, string ItemBrand, string ItemMaterial, string SupplierName, string UOM, decimal Quantity,
            string CurrencyCode, decimal CurrencyRate, decimal QuotedPrice, decimal MarkUpPrice, decimal TotalAmount, string Category, string Remarks, string ItemSize, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCostEstimateItemUpdate", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@CEDetailID", SqlDbType.NVarChar).Value = CEDetailID;
            command.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = ItemName;
            command.Parameters.Add("@chkIsOthers", SqlDbType.Bit).Value = chkIsOthers;
            command.Parameters.Add("@ItemBrand", SqlDbType.NVarChar).Value = ItemBrand;
            command.Parameters.Add("@ItemMaterial", SqlDbType.NVarChar).Value = ItemMaterial;
            command.Parameters.Add("@SupplierName", SqlDbType.NVarChar).Value = SupplierName;
            command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = CurrencyCode;
            command.Parameters.Add("@CurrencyRate", SqlDbType.Money).Value = CurrencyRate;
            command.Parameters.Add("@QuotedPrice", SqlDbType.Money).Value = QuotedPrice;
            command.Parameters.Add("@MarkUpPrice", SqlDbType.Money).Value = MarkUpPrice;
            command.Parameters.Add("@UOM", SqlDbType.NVarChar).Value = UOM;
            command.Parameters.Add("@Quantity", SqlDbType.Float).Value = Quantity;
            command.Parameters.Add("@TotalAmount", SqlDbType.Money).Value = TotalAmount;
            command.Parameters.Add("@Category", SqlDbType.NVarChar).Value = Category;
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
            command.Parameters.Add("@ItemSize", SqlDbType.NVarChar).Value = ItemSize;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetCostEstimateItemDetail(short companyid, string CEID, string CEDetailID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetCostEstimateItemDetail", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@CEDetailID", SqlDbType.NVarChar).Value = CEDetailID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void DeleteCostEstimateItem(short companyid, string CEID, string CEDetailID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCostEstimateItemDelete", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@CEDetailID", SqlDbType.NVarChar).Value = CEDetailID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void ConvertCostEstimate(short companyid, string CEID, short UserID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCostEstimateConvert", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = UserID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        
        public void ReviseCostEstimate(short companyid, string CEID, short UserID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringCostEstimateRevision", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@CEID", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = UserID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetRevisionList(short companyid, string CEID, short UserID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetRevisionList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@DocNo", SqlDbType.NVarChar).Value = CEID;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = UserID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void InsertProjectMR(short companyid, string ProjectNo, string MRNo, string InvoiceNo, decimal InvoiceAmount, string InvoiceRemarks, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringProjectMRInsert", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = MRNo;
            command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
            command.Parameters.Add("@InvoiceAmount", SqlDbType.Money).Value = InvoiceAmount;
            command.Parameters.Add("@InvoiceRemarks", SqlDbType.NVarChar).Value = InvoiceRemarks;
            command.Parameters.Add("@UserID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetMaterialCatList(short companyid, string term, short userid, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetCategoryList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@Term", SqlDbType.NVarChar).Value = term;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = userid;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetMaterial(short CompanyId, string category, string material, string size, string suppliername, string brand, string description, string field, short UserID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetMaterial", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CompanyId;
            command.Parameters.Add("@Category", SqlDbType.NVarChar).Value = category;
            command.Parameters.Add("@Material", SqlDbType.NVarChar).Value = material;
            command.Parameters.Add("@Size", SqlDbType.NVarChar).Value = size;
            command.Parameters.Add("@Suppliername", SqlDbType.NVarChar).Value = suppliername;
            command.Parameters.Add("@Brand", SqlDbType.NVarChar).Value = brand;
            command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = description;
            command.Parameters.Add("@Field", SqlDbType.NVarChar).Value = field;
            command.Parameters.Add("@UserNumID", SqlDbType.SmallInt).Value = UserID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        public void GetMRInfoByMRNo(short companyid, string MRNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringMRDetailByMRNoSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = MRNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
        public void GetMaterialRequisitionListByMRNo(short companyid, string MRNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringMRListByMRNoSelect", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = MRNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void DeleteMRInv(short companyid, string ProjectNo, string MRNo, int ItemID, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringMRInvoiceDelete", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            command.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = MRNo;
            command.Parameters.Add("@ItemID", SqlDbType.Int).Value = ItemID;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }

        public void GetGrandTotalByProjectNo(short companyid, string ProjectNo, ref DataSet ds)
        {
            IDbConnection conn = cm.GetConnection();
            SqlCommand command = new SqlCommand("procEngineeringGetTotalList", (SqlConnection)conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = companyid;
            command.Parameters.Add("@ProjectNo", SqlDbType.NVarChar).Value = ProjectNo;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);
            return;
        }
    }
}	
