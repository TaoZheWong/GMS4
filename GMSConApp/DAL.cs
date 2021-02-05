using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace GMSConApp
{
    class DAL
    {
        public string strcon;
        public SqlConnection cn;
        public SqlCommand cm;
        public DataTable dt;
        public string ex;

        #region SQLConnection
        public DAL(string conn)
        {
            strcon = conn;
        }
        public void CNCMGenerator()
        {
            cn = new SqlConnection(strcon);
            cm = new SqlCommand();
        }
        public void SPexecutor(string strSPName)
        {
            //Store Procedual executor
            cm.CommandTimeout = 90;
            cm.CommandText = strSPName;
            cm.CommandType = CommandType.StoredProcedure;
            cn.Open();
            cm.Connection = cn;
            try
            {
                this.ex = string.Empty;
                cm.ExecuteScalar();
            }
            catch (Exception e)
            {
                this.ex = e.Message;
            }
            finally
            {
                cn.Dispose();
                cm.Dispose();
            }
        }
        public void SPexecutor_dt(string strSPName)
        {
            dt = new DataTable();
            cm.CommandTimeout = 90;
            cm.CommandText = strSPName;
            cm.CommandType = CommandType.StoredProcedure;
            cn.Open();
            cm.Connection = cn;
            try
            {
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    dt.Load(dr);
                    dr.Dispose();
                }
            }
            catch (Exception e)
            {
                this.ex = e.Message;
            }
            finally
            {
                cn.Dispose();
                cm.Dispose();
            }
        }
        #endregion

        #region Product
        public void GMS_Update_ProductFromScheduledTaskProduct()
        {
            CNCMGenerator();
            SPexecutor("procAppProductFromScheduledTaskProductUpdate");
        }
        public DataTable GMS_Get_CompanyByCoyID(short CoyID)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            SPexecutor_dt("procAppGetCompanyByCoyID");
            return dt;
        }
        public void GMS_Delete_ScheduledTaskProductByCoyID(short CoyID)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            SPexecutor("procAppDeleteScheduledTaskProductByCoyID");
        }
        public void GMS_Delete_ScheduledTaskProductDescriptionByCoyID(short CoyID)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            SPexecutor("procAppDeleteScheduledTaskProductDescription");
        }
        public void GMS_Insert_ScheduledTaskProductDescription(short CoyID, string ProductCode, short SrNo, string ProductDescription)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = ProductCode;
            cm.Parameters.Add("@SrNo", SqlDbType.Int).Value = SrNo;
            cm.Parameters.Add("@ProductDescription", SqlDbType.NVarChar).Value = ProductDescription;
            SPexecutor("procAppScheduledTaskProductDescriptionInsert");
        }
        public void GMS_Insert_ScheduledTaskProduct(short CoyID, string ProductCode, string ProductName, string ProductGroupCode, float Volume,
            string UOM, float WeightedCost, float WarehouseHQ, float Warehouse77, float WarehouseOthers,
            float OnSOQty, float OnPOQty)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = ProductCode;
            cm.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = ProductName;
            cm.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = ProductGroupCode;
            cm.Parameters.Add("@Volume", SqlDbType.Float).Value = Volume;
            cm.Parameters.Add("@UOM", SqlDbType.NVarChar).Value = UOM;
            cm.Parameters.Add("@WeightedCost", SqlDbType.Float).Value = WeightedCost;
            cm.Parameters.Add("@WarehouseHQ", SqlDbType.Float).Value = WarehouseHQ;
            cm.Parameters.Add("@Warehouse77", SqlDbType.Float).Value = Warehouse77;
            cm.Parameters.Add("@WarehouseOthers", SqlDbType.Float).Value = WarehouseOthers;
            cm.Parameters.Add("@OnSOQty", SqlDbType.Float).Value = OnSOQty;
            cm.Parameters.Add("@OnPOQty", SqlDbType.Float).Value = OnPOQty;
            SPexecutor("procAppScheduledTaskProductInsert");

        }
        #endregion

        #region GRNDetail
        public void GMS_Delete_ScheduledTaskGRNDetailByCoyID(short CoyID)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            SPexecutor("procAppDeleteScheduledTaskGRNDetailByCoyID");
        }
        public void GMS_Insert_ScheduledTaskGRNDetail(short CoyID, string PONo, DateTime PODate, string GRNNo, DateTime GRNDate,
            string ProductCode, string Purchaser, float Quantity, string GRNTrnNo)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@PONo", SqlDbType.NVarChar).Value = PONo;
            cm.Parameters.Add("@PODate", SqlDbType.DateTime).Value = PODate;
            cm.Parameters.Add("@GRNNo", SqlDbType.NVarChar).Value = GRNNo;
            cm.Parameters.Add("@GRNDate", SqlDbType.DateTime).Value = GRNDate;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = ProductCode;
            cm.Parameters.Add("@Purchaser", SqlDbType.NVarChar).Value = Purchaser;
            cm.Parameters.Add("@Quantity", SqlDbType.Float).Value = Quantity;
            cm.Parameters.Add("@GRNTrnNo", SqlDbType.NVarChar).Value = GRNTrnNo;
            SPexecutor("procAppScheduledTaskGRNDetailInsert");

        }

        public void GRN_Update_GRNDetailFromScheduledTaskGRNDetail()
        {
            CNCMGenerator();
            SPexecutor("procAppGRNDetailFromScheduledTaskGRNDetailUpdate");
        }


        public void GMS_Update_MRDetailProdCodeByCoyID(short CoyID)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            SPexecutor("procAppMRDetailProdCodeUpdate");
        }
        #endregion

        #region MR
        public DataTable GMS_Get_MRByCoyIDStatusID(short CoyID, string StatusID)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@StatusID", SqlDbType.NVarChar).Value = StatusID;
            SPexecutor_dt("procAppGetMRByCoyIDStatusID");
            return dt;
        }
        public DataTable GMS_Get_MRDetailByCoyIDMRNo(short CoyID, string MRNo)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = MRNo;
            SPexecutor_dt("procAppGetMRDetailByCoyIDMRNo");
            return dt;
        }
        public DataTable GMS_Get_MRDeliveryByCoyIDMRNo(short CoyID, string MRNo)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = MRNo;
            SPexecutor_dt("procAppGetMRDeliveryByCoyIDMRNo");
            return dt;
        }
        public DataTable GMS_Get_MRGRNDetailByCoyIDProductPO(short CoyID, string ProductCode, string PONo)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = ProductCode;
            cm.Parameters.Add("@PONo", SqlDbType.NVarChar).Value = PONo;
            SPexecutor_dt("procAppGetMRGRNDetailByCoyIDProductPO");
            return dt;
        }
        public void GMS_Update_MR(short CoyID, string MRNo)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = MRNo;
            SPexecutor("procAppUpdateMR");
        }

        public void GMS_Update_MR_SAP(short CoyID)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            SPexecutor("procAppUpdateMRSAP");
        }
        #endregion



        #region TB
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
        public string ConstructTempTransferSQLForPDS(short coyId, short year, short month, System.Data.DataRowCollection ttRows)
        {
            string SQL = "DELETE FROM tbTempTransfer2;";
            foreach (DataRow dr in ttRows)
            {
                SQL += "INSERT INTO tbTempTransfer2 VALUES (" + coyId + ", '" +
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

        public void InsertTempTransfer(short coyId, short year, short month, System.Data.DataRowCollection ttRows, string TBType)
        {
            string SQL = "";

            if (TBType == "N")
                SQL = ConstructTempTransferSQL(coyId, year, month, ttRows);
            else if (TBType == "P")
                SQL = ConstructTempTransferSQLForPDS(coyId, year, month, ttRows);
            CNCMGenerator();
            cm.CommandText = SQL;
            cm.CommandType = CommandType.Text;
            cn.Open();
            cm.Connection = cn;
            SqlDataReader rdr = null;
            try
            {
                rdr = cm.ExecuteReader();
            }
            catch (Exception e)
            {
                this.ex = e.Message;
            }
            finally
            {
                rdr.Close();
                cn.Dispose();
                cm.Dispose();
            }
        }
        public void ProcessTrialBalance()
        {
            CNCMGenerator();
            SPexecutor("procFinanceTBProcessAll");
        }
        #endregion

        #region CMSAccountAddress
        public void GMS_Delete_AccountAddress(short CoyID)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            SPexecutor("procAppDeleteAccountAddress");
        }
        public void GMS_Insert_AccountAddress(short CoyID, string AccountCode, string AddressType, int AddressID, string ContactPerson,
            string Address1, string Address2, string Address3, string Address4, string PostalCode,
            string TransportZone, string OfficePhone, string MobilePhone, string Fax, string Email,
            string SpecialInstruction, bool IsActive, short CreatedBy, DateTime CreatedDate, string OrderedBy,
            string AddressCode)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = AccountCode;
            cm.Parameters.Add("@AddressType", SqlDbType.NVarChar).Value = AddressType;
            cm.Parameters.Add("@AddressID", SqlDbType.Int).Value = AddressID;
            cm.Parameters.Add("@ContactPerson", SqlDbType.NVarChar).Value = ContactPerson;

            cm.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = Address1;
            cm.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = Address2;
            cm.Parameters.Add("@Address3", SqlDbType.NVarChar).Value = Address3;
            cm.Parameters.Add("@Address4", SqlDbType.NVarChar).Value = Address4;
            cm.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = PostalCode;

            cm.Parameters.Add("@TransportZone", SqlDbType.NVarChar).Value = TransportZone;
            cm.Parameters.Add("@OfficePhone", SqlDbType.NVarChar).Value = OfficePhone;
            cm.Parameters.Add("@MobilePhone", SqlDbType.NVarChar).Value = MobilePhone;
            cm.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = Fax;
            cm.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;

            cm.Parameters.Add("@SpecialInstruction", SqlDbType.NVarChar).Value = SpecialInstruction;
            cm.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
            cm.Parameters.Add("@CreatedBy", SqlDbType.SmallInt).Value = CreatedBy;
            cm.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = CreatedDate;
            cm.Parameters.Add("@OrderedBy", SqlDbType.NVarChar).Value = OrderedBy;

            cm.Parameters.Add("@AddressCode", SqlDbType.NVarChar).Value = AddressCode;
            SPexecutor("procAppAccountAddressInsert");
        }

        #endregion

        public void GMS_Insert_Product(short CoyID, string productCode, string productName, string productGroupCode, string uom)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            cm.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            cm.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = productGroupCode;
            cm.Parameters.Add("@UOM", SqlDbType.NVarChar).Value = uom;
            SPexecutor("procAppWSProductInsert");
        }

        public void GMS_Insert_Product2(short CoyID, string productCode, string productName, string productGroupCode, double volume, string uom,
            double weightedCost, double onSOQty, double onPOQty, double onBOQty, double onHandQty, bool isGasDivision, bool isWeldingDivision,
            string prodForeignName, bool trackedByBatch, bool trackedBySerial, int version)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            cm.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            cm.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = productGroupCode;
            cm.Parameters.Add("@Volume", SqlDbType.Float).Value = volume;
            cm.Parameters.Add("@UOM", SqlDbType.NVarChar).Value = uom;
            cm.Parameters.Add("@WeightedCost", SqlDbType.Float).Value = weightedCost;
            cm.Parameters.Add("@OnSOQty", SqlDbType.Float).Value = onSOQty;
            cm.Parameters.Add("@OnPOQty", SqlDbType.Float).Value = onPOQty;
            cm.Parameters.Add("@OnBOQty", SqlDbType.Float).Value = onBOQty;
            cm.Parameters.Add("@OnHandQty", SqlDbType.Float).Value = onHandQty;
            cm.Parameters.Add("@IsGasDivision", SqlDbType.Bit).Value = isGasDivision;
            cm.Parameters.Add("@IsWeldingDivision", SqlDbType.Bit).Value = isWeldingDivision;
            cm.Parameters.Add("@ProdForeignName", SqlDbType.NVarChar).Value = prodForeignName;
            cm.Parameters.Add("@TrackedByBatch", SqlDbType.Bit).Value = trackedByBatch;
            cm.Parameters.Add("@TrackedBySerial", SqlDbType.Bit).Value = trackedBySerial;
            cm.Parameters.Add("@Version", SqlDbType.Int).Value = version;
            SPexecutor("procAppWSProduct2Insert");
        }


        public void GMS_Insert_ProductGroup(short CoyID, string productName, string productGroupCode, string productGroupCategory, string team, string abcGrouping)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = productName;
            cm.Parameters.Add("@ProductGroupName", SqlDbType.NVarChar).Value = productGroupCode;
            cm.Parameters.Add("@ProductGroupCategory", SqlDbType.NVarChar).Value = productGroupCategory;
            cm.Parameters.Add("@Team", SqlDbType.NVarChar).Value = team;
            cm.Parameters.Add("@ABCGrouping", SqlDbType.NVarChar).Value = abcGrouping;
            SPexecutor("procAppWSProductGroupInsert");
        }

        public void GMS_Insert_Account(short CoyID, string accountCode, string accountType, string accountName, string salesPersonID, string defaultCurrency, string gstType,
            short creditTerm, float creditLimit, string address1, string address2, string address3, string address4, string postalCode, string contactPerson, string officePhone,
            string mobilePhone, string fax, string email, string industry, string country, DateTime createdDate, string accountGroupName, string accountClass, string gradeCode, float outstanding)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            cm.Parameters.Add("@AccountType", SqlDbType.NVarChar).Value = accountType;
            cm.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            cm.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = salesPersonID;
            cm.Parameters.Add("@DefaultCurrency", SqlDbType.NVarChar).Value = defaultCurrency;
            cm.Parameters.Add("@GSTType", SqlDbType.NVarChar).Value = gstType;
            cm.Parameters.Add("@CreditTerm", SqlDbType.SmallInt).Value = creditTerm;
            cm.Parameters.Add("@CreditLimit", SqlDbType.Float).Value = creditLimit;
            cm.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = address1;
            cm.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = address2;
            cm.Parameters.Add("@Address3", SqlDbType.NVarChar).Value = address3;
            cm.Parameters.Add("@Address4", SqlDbType.NVarChar).Value = address4;
            cm.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = postalCode;
            cm.Parameters.Add("@ContactPerson", SqlDbType.NVarChar).Value = contactPerson;
            cm.Parameters.Add("@OfficePhone", SqlDbType.NVarChar).Value = officePhone;
            cm.Parameters.Add("@MobilePhone", SqlDbType.NVarChar).Value = mobilePhone;
            cm.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = fax;
            cm.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;
            cm.Parameters.Add("@Industry", SqlDbType.NVarChar).Value = industry;
            cm.Parameters.Add("@Country", SqlDbType.NVarChar).Value = country;
            cm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = createdDate;
            cm.Parameters.Add("@AccountGroupName", SqlDbType.NVarChar).Value = accountGroupName;
            cm.Parameters.Add("@AccountClass", SqlDbType.NVarChar).Value = accountClass;
            cm.Parameters.Add("@GradeCode", SqlDbType.NVarChar).Value = gradeCode;
            cm.Parameters.Add("@Outstanding", SqlDbType.NVarChar).Value = outstanding;
            SPexecutor("procAppWSAccountInsert");
        }

        public void GMS_Insert_SalesPerson(short CoyID, string salesPersonID, string salesPersonName, string divisionID, string shortName, string team,string active)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@SalesPersonID", SqlDbType.NVarChar).Value = salesPersonID;
            cm.Parameters.Add("@SalesPersonName", SqlDbType.NVarChar).Value = salesPersonName;
            cm.Parameters.Add("@DivisionID", SqlDbType.NVarChar).Value = divisionID;
            cm.Parameters.Add("@ShortName", SqlDbType.NVarChar).Value = shortName;
            cm.Parameters.Add("@Team", SqlDbType.NVarChar).Value = team;
            cm.Parameters.Add("@Active", SqlDbType.NVarChar).Value = active;

            SPexecutor("procAppWSSalesPersonInsert");
        }

        public void GMS_Insert_Purchaser(short CoyID, string purchaserID, string purchaserName, string purchaserEmail)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@PurchaserID", SqlDbType.NVarChar).Value = purchaserID;
            cm.Parameters.Add("@PurchaserName", SqlDbType.NVarChar).Value = purchaserName;
            cm.Parameters.Add("@PurchaserEmail", SqlDbType.NVarChar).Value = purchaserEmail;

            SPexecutor("procAppWSSPurchaserInsert");
        }

        public void GMS_Insert_Sales(short CoyID, string trnType, string trnNo, DateTime trnDate, string accountCode, string accountName, string docNo, string poNo, double amount,
            string currency, double exchangeRate, double taxAmount, string customerSalesPersonID, string trnSalesPersonID, DateTime docdate, DateTime duedate)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@TrnType", SqlDbType.NVarChar).Value = trnType;
            cm.Parameters.Add("@TrnNo", SqlDbType.NVarChar).Value = trnNo;
            cm.Parameters.Add("@TrnDate", SqlDbType.DateTime).Value = trnDate;
            cm.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            cm.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            cm.Parameters.Add("@DocNo", SqlDbType.NVarChar).Value = docNo;
            cm.Parameters.Add("@PONo", SqlDbType.NVarChar).Value = poNo;
            cm.Parameters.Add("@Amount", SqlDbType.Float).Value = amount;
            cm.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = currency;
            cm.Parameters.Add("@ExchangeRate", SqlDbType.Float).Value = exchangeRate;
            cm.Parameters.Add("@TaxAmount", SqlDbType.Float).Value = taxAmount;
            cm.Parameters.Add("@CustomerSalesPersonID", SqlDbType.NVarChar).Value = customerSalesPersonID;
            cm.Parameters.Add("@TrnSalesPersonID", SqlDbType.NVarChar).Value = trnSalesPersonID;
            cm.Parameters.Add("@DocDate", SqlDbType.DateTime).Value = docdate;
            cm.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = duedate;
            SPexecutor("procAppWSSalesInsert");
        }

        public void GMS_Insert_SalesDetail(short CoyID, string salesTrnType, string salesTrnNo, DateTime salesTrnDate, short srNo, string accountCode, string accountName,
                                        string productCode, string productName, string productGroupCode, string productGroupName, double quantity, double unitCost, double unitAmount,
                                        double cost, double amount, double gpAmount, string currency, double exchangeRate, double taxRate, string doNo, string location, string customerSalesPersonID,
                                        string trnSalesPersonID, DateTime salesdocdate, DateTime duedate, string glaccount)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@SalesTrnType", SqlDbType.NVarChar).Value = salesTrnType;
            cm.Parameters.Add("@SalesTrnNo", SqlDbType.NVarChar).Value = salesTrnNo;
            cm.Parameters.Add("@SalesTrnDate", SqlDbType.DateTime).Value = salesTrnDate;
            cm.Parameters.Add("@SrNo", SqlDbType.SmallInt).Value = srNo;
            cm.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            cm.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            cm.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            cm.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = productGroupCode;
            cm.Parameters.Add("@ProductGroupName", SqlDbType.NVarChar).Value = productGroupName;
            cm.Parameters.Add("@Quantity", SqlDbType.Float).Value = quantity;
            cm.Parameters.Add("@UnitCost", SqlDbType.Float).Value = unitCost;
            cm.Parameters.Add("@UnitAmount", SqlDbType.Float).Value = unitAmount;
            cm.Parameters.Add("@Cost", SqlDbType.Float).Value = cost;
            cm.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
            cm.Parameters.Add("@GPAmount", SqlDbType.Float).Value = gpAmount;
            cm.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = currency;
            cm.Parameters.Add("@ExchangeRate", SqlDbType.Float).Value = exchangeRate;
            cm.Parameters.Add("@TaxRate", SqlDbType.Float).Value = taxRate;
            cm.Parameters.Add("@DONo", SqlDbType.NVarChar).Value = doNo;
            cm.Parameters.Add("@Location", SqlDbType.NVarChar).Value = location;
            cm.Parameters.Add("@CustomerSalesPersonID", SqlDbType.NVarChar).Value = customerSalesPersonID;
            cm.Parameters.Add("@TrnSalesPersonID", SqlDbType.NVarChar).Value = trnSalesPersonID;
            cm.Parameters.Add("@SalesDocDate", SqlDbType.DateTime).Value = salesdocdate;
            cm.Parameters.Add("@Duedate", SqlDbType.DateTime).Value = duedate;
            cm.Parameters.Add("@GLAccount", SqlDbType.NVarChar).Value = glaccount;
            SPexecutor("procAppWSSalesDetailInsert");
        }

        public void GMS_Insert_Receipt(short CoyID, string trnType, string trnNo, DateTime trnDate, string accountCode, string accountName, string salesTrnType,
                        string salesTrnNo, string docNo, string allcDocNo, double amount, string currency, double exchangeRate)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@TrnType", SqlDbType.NVarChar).Value = trnType;
            cm.Parameters.Add("@TrnNo", SqlDbType.NVarChar).Value = trnNo;
            cm.Parameters.Add("@TrnDate", SqlDbType.DateTime).Value = trnDate;
            cm.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            cm.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            cm.Parameters.Add("@SalesTrnType", SqlDbType.NVarChar).Value = salesTrnType;
            cm.Parameters.Add("@SalesTrnNo", SqlDbType.NVarChar).Value = salesTrnNo;
            cm.Parameters.Add("@DocNo", SqlDbType.NVarChar).Value = docNo;
            cm.Parameters.Add("@AllcDocNo", SqlDbType.NVarChar).Value = allcDocNo;
            cm.Parameters.Add("@Amount", SqlDbType.Float).Value = amount;
            cm.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = currency;
            cm.Parameters.Add("@ExchangeRate", SqlDbType.Float).Value = exchangeRate;
            SPexecutor("procAppWSReceiptInsert");
        }

        public void GMS_Insert_GRN(short CoyID, string trnNo, DateTime trnDate, string poNo, DateTime poDate, string productCode,
                        string productName, string productGroupCode, string productGroupName,
                        double quantity, double unitPrice, double landedCostUnitPrice, double cost, string currency, double exchangeRate)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@TrnNo", SqlDbType.NVarChar).Value = trnNo;
            cm.Parameters.Add("@TrnDate", SqlDbType.DateTime).Value = trnDate;
            cm.Parameters.Add("@PONo", SqlDbType.NVarChar).Value = poNo;
            cm.Parameters.Add("@PODate", SqlDbType.DateTime).Value = poDate;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            cm.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            cm.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = productGroupCode;
            cm.Parameters.Add("@ProductGroupName", SqlDbType.NVarChar).Value = productGroupName;
            cm.Parameters.Add("@Quantity", SqlDbType.Float).Value = quantity;
            cm.Parameters.Add("@UnitPrice", SqlDbType.Float).Value = unitPrice;
            cm.Parameters.Add("@LandedCostUnitPrice", SqlDbType.Float).Value = landedCostUnitPrice;
            cm.Parameters.Add("@Cost", SqlDbType.Float).Value = cost;
            cm.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = currency;
            cm.Parameters.Add("@ExchangeRate", SqlDbType.Float).Value = exchangeRate;
            SPexecutor("procAppWSGRNInsert");
        }

        public void GMS_Insert_StockMovement(short CoyID, string trnType, int trnNo, DateTime trnDate, string refNo, string accountCode, string accountName,
                    string productCode, string productName, string productGroupCode, string productGroupName, double receivedQuantity, double issuedQuantity,
                    double balanceQuantity, double cost, double costWT, string currency, double exchangeRate, string narration)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@TrnType", SqlDbType.NVarChar).Value = trnType;
            cm.Parameters.Add("@TrnNo", SqlDbType.Int).Value = trnNo;
            cm.Parameters.Add("@TrnDate", SqlDbType.DateTime).Value = trnDate;
            cm.Parameters.Add("@RefNo", SqlDbType.NVarChar).Value = refNo;
            cm.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            cm.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            cm.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            cm.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = productGroupCode;
            cm.Parameters.Add("@ProductGroupName", SqlDbType.NVarChar).Value = productGroupName;
            cm.Parameters.Add("@ReceivedQuantity", SqlDbType.Float).Value = receivedQuantity;
            cm.Parameters.Add("@IssuedQuantity", SqlDbType.Float).Value = issuedQuantity;
            cm.Parameters.Add("@BalanceQuantity", SqlDbType.Float).Value = balanceQuantity;
            cm.Parameters.Add("@Cost", SqlDbType.Float).Value = cost;
            cm.Parameters.Add("@CostWT", SqlDbType.Float).Value = costWT;
            cm.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = currency;
            cm.Parameters.Add("@ExchangeRate", SqlDbType.Float).Value = exchangeRate;
            cm.Parameters.Add("@Narration", SqlDbType.NVarChar).Value = narration;
            SPexecutor("procAppWSStockMovementInsert");
        }

        public void GMS_ImportUpdateDataByAction(short CoyID, string action)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@ActionType", SqlDbType.NVarChar).Value = action;
            SPexecutor("procAppImportUpdateDataByAction");
        }

        public void GMS_Insert_Warehouse(short CoyID, string warehouse, string warehouseName)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@Warehouse", SqlDbType.NVarChar).Value = warehouse;
            cm.Parameters.Add("@Name", SqlDbType.NVarChar).Value = warehouseName;

            SPexecutor("procAppWSWarehouseInsert");
        }

        public void LogException(short CoyID, string method, string json, string exceptionMessage)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@Method", SqlDbType.NVarChar).Value = method;
            cm.Parameters.Add("@JSON", SqlDbType.NVarChar).Value = json;
            cm.Parameters.Add("@ExceptionMessage", SqlDbType.NVarChar).Value = exceptionMessage;

            SPexecutor("procAppSAPLogInsert");
        }

        public void GMS_Insert_ForeignExchange(string currencyCode, decimal buyrate, decimal sellrate, decimal monthend)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = currencyCode;
            cm.Parameters.Add("@ExchRate1", SqlDbType.Decimal).Value = buyrate;
            cm.Parameters.Add("@ExchRate2", SqlDbType.Decimal).Value = sellrate;
            cm.Parameters.Add("@RevRate", SqlDbType.Decimal).Value = monthend;
            SPexecutor("procAppWSExchangeRateInsert");
        }

        public void GMS_Update_ForeignExchange()
        {
            CNCMGenerator();
            SPexecutor("procA21ImportForeignExchangeRateSAP");
        }


        public void GMS_Insert_PurchaseDetail(short CoyID, DateTime trnDate, string refNo, string accountCode, string accountName,
                                        string productCode, string productName, string productGroupCode, string productGroupName, double quantity, double unitAmount,
                                        double amount, string currency, double exchangeRate, double taxRate, string transactionID)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@TrnDate", SqlDbType.DateTime).Value = trnDate;
            cm.Parameters.Add("@RefNo", SqlDbType.NVarChar).Value = refNo;
            cm.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            cm.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            cm.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            cm.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = productGroupCode;
            cm.Parameters.Add("@ProductGroupName", SqlDbType.NVarChar).Value = productGroupName;
            cm.Parameters.Add("@Quantity", SqlDbType.Float).Value = quantity;
            cm.Parameters.Add("@UnitAmount", SqlDbType.Float).Value = unitAmount;
            cm.Parameters.Add("@Amount", SqlDbType.Float).Value = amount;
            cm.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = currency;
            cm.Parameters.Add("@ExchangeRate", SqlDbType.Float).Value = exchangeRate;
            cm.Parameters.Add("@TaxRate", SqlDbType.Float).Value = taxRate;
            cm.Parameters.Add("@TransactionID", SqlDbType.NVarChar).Value = transactionID;
            SPexecutor("procAppWSPurchaseDetailInsert");
        }

        public void GMS_Insert_PurchaseOrder(short CoyID, int trnNo, DateTime trnDate, string accountCode, string accountName,
                                        string productCode, string productName, string productGroupCode, string productGroupName, double quantity, string uom, double unitAmount,
                                        double discount, double amountBeforeDiscount, double amountAfterDiscount, string currency, double exchangeRate, double taxRate, string docNo)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@TrnNo", SqlDbType.Int).Value = trnNo;
            cm.Parameters.Add("@TrnDate", SqlDbType.DateTime).Value = trnDate;
            cm.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            cm.Parameters.Add("@AccountName", SqlDbType.NVarChar).Value = accountName;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            cm.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = productName;
            cm.Parameters.Add("@ProductGroupCode", SqlDbType.NVarChar).Value = productGroupCode;
            cm.Parameters.Add("@ProductGroupName", SqlDbType.NVarChar).Value = productGroupName;
            cm.Parameters.Add("@Quantity", SqlDbType.Float).Value = quantity;
            cm.Parameters.Add("@UOM", SqlDbType.NVarChar).Value = uom;
            cm.Parameters.Add("@UnitAmount", SqlDbType.Float).Value = unitAmount;
            cm.Parameters.Add("@Discount", SqlDbType.Float).Value = discount;
            cm.Parameters.Add("@AmountBeforeDiscount", SqlDbType.Float).Value = amountBeforeDiscount;
            cm.Parameters.Add("@AmountAfterDiscount", SqlDbType.Float).Value = amountAfterDiscount;
            cm.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = currency;
            cm.Parameters.Add("@ExchangeRate", SqlDbType.Float).Value = exchangeRate;
            cm.Parameters.Add("@TaxRate", SqlDbType.Float).Value = taxRate;
            cm.Parameters.Add("@DocNo", SqlDbType.NVarChar).Value = docNo;
            SPexecutor("procAppWSPurchaseOrderInsert");
        }

        public void GMS_Insert_ProductUOM(short CoyID, string productCode, string uom, float conversionFactor, float baseQty, string baseUOM, float convertedQty, string convertedUOM)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            cm.Parameters.Add("@UOM", SqlDbType.NVarChar).Value = uom;
            cm.Parameters.Add("@ConversionFactor", SqlDbType.Float).Value = conversionFactor;
            cm.Parameters.Add("@BaseQty", SqlDbType.Float).Value = baseQty;
            cm.Parameters.Add("@BaseUOM", SqlDbType.NVarChar).Value = baseUOM;
            cm.Parameters.Add("@ConvertedQty", SqlDbType.Float).Value = convertedQty;
            cm.Parameters.Add("@ConvertedUOM", SqlDbType.NVarChar).Value = convertedUOM;
            SPexecutor("procAppWSProductUOMInsert");
        }

        public void GMS_Insert_ProductNotes(short CoyID, string productCode, int SrNo, string description)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            cm.Parameters.Add("@SrNo", SqlDbType.Int).Value = SrNo;
            cm.Parameters.Add("@Description", SqlDbType.NVarChar).Value = description;
            SPexecutor("procAppWSProductNotesInsert");
        }

        public void GMS_Delete_ProductNotes(short CoyID, string productCode)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@ProductCode", SqlDbType.NVarChar).Value = productCode;
            SPexecutor("procAppWSProductNotesDelete");
        }

        public void GMS_Insert_ContactPerson(short CoyID, string accountCode, string name, string position, string address,
            string tel1, string tel2, string mobile, string fax, string email, string firstName, string middleName,
            string lastName, string blockComm)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@AccountCode", SqlDbType.NVarChar).Value = accountCode;
            cm.Parameters.Add("@Name", SqlDbType.NVarChar).Value = name;
            cm.Parameters.Add("@Position", SqlDbType.NVarChar).Value = position;
            cm.Parameters.Add("@Address", SqlDbType.NVarChar).Value = address;
            cm.Parameters.Add("@Tel1", SqlDbType.NVarChar).Value = tel1;
            cm.Parameters.Add("@Tel2", SqlDbType.NVarChar).Value = tel2;
            cm.Parameters.Add("@Mobile", SqlDbType.NVarChar).Value = mobile;
            cm.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = fax;
            cm.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;
            cm.Parameters.Add("@Firstname", SqlDbType.NVarChar).Value = firstName;
            cm.Parameters.Add("@MiddleName", SqlDbType.NVarChar).Value = middleName;
            cm.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = lastName;
            cm.Parameters.Add("@BlockComm", SqlDbType.NVarChar).Value = blockComm;
            SPexecutor("procAppContactPersonInsert");
        }

        public void GMS_Insert_TaxCode(short CoyID, string taxcode, string taxName, decimal taxRate, string category)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@TaxTypeID", SqlDbType.NVarChar).Value = taxcode;
            cm.Parameters.Add("@TaxName", SqlDbType.NVarChar).Value = taxName;
            cm.Parameters.Add("@TaxRate", SqlDbType.Decimal).Value = taxRate;
            cm.Parameters.Add("@Category", SqlDbType.NVarChar).Value = category;
            SPexecutor("procAppWSTaxTypeInsert");
        }

        //DateTime trnDate, 
        public void GMS_Insert_JobTraveller(short CoyID, DateTime trnDate, int jobTravellerNo, string productionOrderNo, string finalFG, string finalFGDescription, string bomTemplate,
            int bomLevel, string bomParent, int completionQty, string category, string childCode, string childDescription, double baseQuantity, string uom,
            double quantity, string glCode, double amount, DateTime lastProductionIssueDate, DateTime lastProductionReceiptDate, double plannedQty, string jobTravellerStatus)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@TrnDate", SqlDbType.DateTime).Value = trnDate;
            cm.Parameters.Add("@JobTravellerNo", SqlDbType.Int).Value = jobTravellerNo;
            cm.Parameters.Add("@ProductionOrderNo", SqlDbType.NVarChar).Value = productionOrderNo;
            cm.Parameters.Add("@FinalFG", SqlDbType.NVarChar).Value = finalFG;
            cm.Parameters.Add("@FinalFGDescription", SqlDbType.NVarChar).Value = finalFGDescription;
            cm.Parameters.Add("@BOMTemplate", SqlDbType.NVarChar).Value = bomTemplate;
            cm.Parameters.Add("@BOMLevel", SqlDbType.Int).Value = bomLevel;
            cm.Parameters.Add("@BOMParent", SqlDbType.NVarChar).Value = bomParent;
            cm.Parameters.Add("@CompletionQty", SqlDbType.Int).Value = completionQty;
            cm.Parameters.Add("@Category", SqlDbType.NVarChar).Value = category;
            cm.Parameters.Add("@ChildCode", SqlDbType.NVarChar).Value = childCode;
            cm.Parameters.Add("@ChildDescription", SqlDbType.NVarChar).Value = childDescription;
            cm.Parameters.Add("@BaseQuantity", SqlDbType.Float).Value = baseQuantity;
            cm.Parameters.Add("@UOM", SqlDbType.NVarChar).Value = uom;
            cm.Parameters.Add("@Quantity", SqlDbType.Float).Value = quantity;
            cm.Parameters.Add("@GLCode", SqlDbType.NVarChar).Value = glCode;
            cm.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
            cm.Parameters.Add("@LastProductionIssueDate", SqlDbType.DateTime).Value = lastProductionIssueDate;
            cm.Parameters.Add("@LastProductionReceiptDate", SqlDbType.DateTime).Value = lastProductionReceiptDate;
            cm.Parameters.Add("@PlannedQty", SqlDbType.Float).Value = plannedQty;
            cm.Parameters.Add("@JobTravellerStatus", SqlDbType.NVarChar).Value = jobTravellerStatus;

            SPexecutor("procAppWSJobTravellerInsert");
        }

        public void GMS_Insert_Country(string country, string shortName)
        {
            CNCMGenerator();
            cm.Parameters.Add("@Name", SqlDbType.NVarChar).Value = country;
            cm.Parameters.Add("@ShortName", SqlDbType.NVarChar).Value = shortName;
            SPexecutor("procAppWSCountryInsert");
        }

        public void GMS_Update_CloseMRStatus(short CoyID, string mrno)
        {
            CNCMGenerator();
            cm.Parameters.Add("@CoyID", SqlDbType.SmallInt).Value = CoyID;
            cm.Parameters.Add("@MRNo", SqlDbType.NVarChar).Value = mrno;
            SPexecutor("procAppWSUpdateMRStatus");
        }
    }
}
