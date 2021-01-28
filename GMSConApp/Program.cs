using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
namespace GMSConApp
{
    class Program
    {
        static string _DBConn = "server=192.168.1.236\\gms;database=gms;user=sa;password=gms$628128lnox";
        //static string _DBConn = "server=ldlnb17;database=GMS_20200204;user=sa;password=eason";
        static string _GMSDefaultURL = "https://gms.leedenlimited.com/GMSWebService/GMSWebService.asmx";
        static string _CMSDefaultURL = "http://10.1.1.21/CMS.WebServices/Recipe.asmx";
        static DAL oDAL = null;

        // GMSConApp Tasks Menu: 
        // 1. MR with COYID         : MR
        // 2. TB                    : Trial Balance
        // 3. CMSAA with COYID      : Account Address        

        static void Main(string[] args)
        {
            try
            {
                string TaskName = string.Empty;
                short CoyID = 0;
                oDAL = new DAL(_DBConn);
                if (args[0] == null)
                {
                    Console.WriteLine("Task Name is null"); // Check for null array
                }
                else
                {
                    TaskName = args[0];
                    switch (TaskName)
                    {
                        case "MR":
                            CoyID = Convert.ToInt16(args[1]);
                            if (args[1] == null)
                            {
                                Console.WriteLine("CoyID is null"); // Check for null array
                            }
                            else
                            {
                                Task_MR(CoyID);
                            }
                            break;

                        case "TB":
                            Task_TB();
                            break;
                        case "CMSAA":
                            CoyID = Convert.ToInt16(args[1]);
                            if (args[1] == null)
                            {
                                Console.WriteLine("CoyID is null"); // Check for null array
                            }
                            else
                            {
                                Task_CMSAccountAddress(CoyID);
                            }
                            break;
                        case "LHMImport":
                            CoyID = Convert.ToInt16(args[1]);
                            if (args[1] == null)
                            {
                                Console.WriteLine("CoyID is null"); // Check for null array
                            }
                            else
                            {
                                Task_LHMImport(CoyID);
                            }
                            break;
                        case "LMSImport":
                            CoyID = Convert.ToInt16(args[1]);
                            if (args[1] == null)
                            {
                                Console.WriteLine("CoyID is null"); // Check for null array
                            }
                            else
                            {
                                Task_LMSImport(CoyID);
                            }
                            break;
                        case "LMSImportProduct":
                            CoyID = Convert.ToInt16(args[1]);
                            if (args[1] == null)
                            {
                                Console.WriteLine("CoyID is null"); // Check for null array
                            }
                            else
                            {
                                Task_ImportProduct(CoyID);
                            }
                            break;
                        case "LMSImportSalesPerson":
                            CoyID = Convert.ToInt16(args[1]);
                            if (args[1] == null)
                            {
                                Console.WriteLine("CoyID is null"); // Check for null array
                            }
                            else
                            {
                                Task_LMSImportSalesPerson(CoyID);
                            }
                            break;
                        default:
                            Console.WriteLine("End with no task excuted");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString() + " -- Scheduled Task Error: " + ex.ToString());
                //Console.ReadLine();
            }
        }

        static void Task_MR(short CoyID)
        {
            Console.WriteLine(DateTime.Now.ToString() + " -- Start Task : MR");
            Console.WriteLine(DateTime.Now.ToString() + " -- CoyID = " + CoyID);

            DataTable dtCompany = oDAL.GMS_Get_CompanyByCoyID(CoyID);
            if (dtCompany.Rows.Count > 0)
            {
                //Company Found
                Console.WriteLine(DateTime.Now.ToString() + " -- Scheduled Task for " + dtCompany.Rows[0]["Code"].ToString() + " started!");

                //1. Update Product
                Console.WriteLine(DateTime.Now.ToString() + " -- Start Import Product for " + dtCompany.Rows[0]["Code"].ToString());
                ImportProduct(CoyID, dtCompany.Rows[0]["WebServiceAddress"].ToString().Trim());

                //2. Import GRN Detail From A21
                Console.WriteLine(DateTime.Now.ToString() + " -- Start Import GRNDetail for " + dtCompany.Rows[0]["Code"].ToString());
                ImportGRNDetail(CoyID, dtCompany.Rows[0]["WebServiceAddress"].ToString().Trim());

                //3. Update MR Details Zero Product Code From New Product Code
                Console.WriteLine(DateTime.Now.ToString() + " -- Start Update GRNDetail for " + dtCompany.Rows[0]["Code"].ToString());
                oDAL.GMS_Update_MRDetailProdCodeByCoyID(CoyID);
                Console.WriteLine(DateTime.Now.ToString() + " -- Update GRNDetail Successfully!");


                //4. Close MR
                Console.WriteLine(DateTime.Now.ToString() + " -- Start Close MR for " + dtCompany.Rows[0]["Code"].ToString());
                CloseMR(CoyID);
            }
            else
            {
                //No company found
            }
            dtCompany.Dispose();
            Console.WriteLine(DateTime.Now.ToString() + " -- Scheduled Task for " + dtCompany.Rows[0]["Code"].ToString() + " ended!");
        }

        static void Task_TB()
        {
            Console.WriteLine(DateTime.Now.ToString() + " -- Start Task : TB");

            DataTable dtCompany = oDAL.GMS_Get_CompanyByCoyID(0); //Return All Company
            short financialYear = 0;
            short financialMonth = 0;
            short CoyID = 0;
            short Year = Convert.ToInt16(DateTime.Now.AddMonths(-1).Year.ToString());
            short Month = Convert.ToInt16(DateTime.Now.AddMonths(-1).Month.ToString());
            short FYE = 0;
            string TBType = "";

            foreach (DataRow dr in dtCompany.Rows)
            {
                try
                {
                    TBType = dr["TBType"].ToString().Trim();
                    if (TBType != "M" && !string.IsNullOrEmpty(TBType))
                    {
                        CoyID = Convert.ToInt16(dr["CoyID"].ToString());
                        FYE = Convert.ToInt16(dr["FYE"].ToString());
                        Console.WriteLine(DateTime.Now.ToString() + " -- Start " + CoyID.ToString() +
                            " for " + Month.ToString() + "/" + Year.ToString());

                        //Financial Year and Month Conversion
                        if (FYE == 12)
                        {
                            financialYear = Year;
                            financialMonth = Month;
                        }
                        else if (FYE == 3)
                        {
                            if (Month >= 4 && Month <= 12)
                            {
                                financialMonth = Convert.ToInt16(Month - 3);
                            }
                            else
                            {
                                financialYear = Convert.ToInt16(Year - 1);
                                financialMonth = Convert.ToInt16(Month + 9);
                            }
                        }
                        else if (FYE == 6)
                        {
                            if (Month >= 7 && Month <= 12)
                            {
                                financialMonth = Convert.ToInt16(Month - 6);
                            }
                            else
                            {
                                financialYear = Convert.ToInt16(Year - 1);
                                financialMonth = Convert.ToInt16(Month + 6);
                            }
                        }

                        //Connect to WebService
                        //Console.WriteLine(DateTime.Now.ToString() + " -- Connecting WebServer...");
                        GMSWebService.GMSWebService ws = new GMSWebService.GMSWebService();
                        DataSet ds = new DataSet();
                        if (!string.IsNullOrEmpty(dr["WebServiceAddress"].ToString().Trim()))
                        {
                            ws.Url = dr["WebServiceAddress"].ToString().Trim();
                        }
                        else
                        {
                            ws.Url = _GMSDefaultURL;
                        }


                        //Retrieve Trial Balance data
                        //Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Trial Balance data...");
                        if (TBType == "N")
                        {
                            ds = ws.GetTrialBalance(CoyID, financialYear, financialMonth, FYE);
                        }
                        else if (TBType == "P")
                        {
                            ds = ws.GetTrialBalanceForPDS(CoyID, financialYear, financialMonth, FYE);
                        }
                        ws.Dispose();

                        //Insert Trial Balance data into GMS
                        //Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Trial Balance data into GMS...");
                        oDAL.InsertTempTransfer(CoyID, Year, Month, ds.Tables[0].Rows, TBType);


                        //Processing Trial Balance data in GMS
                        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            //Console.WriteLine(DateTime.Now.ToString() + " -- No Trial Balance data");

                        }
                        else
                        {
                            Console.WriteLine(DateTime.Now.ToString() + " -- Processing Trial Balance data in GMS...");
                            oDAL.ProcessTrialBalance();
                        }

                        //Console.WriteLine(DateTime.Now.ToString() + " -- End Trial Balance data insertion for " + CoyID.ToString());
                        ds.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(DateTime.Now.ToString() + " -- Scheduled Task Error: " + ex.ToString());
                }
            }
        }

        static void Task_CMSAccountAddress(short CoyID)
        {
            Console.WriteLine(DateTime.Now.ToString() + " -- Start Task : CMSAA");
            Console.WriteLine(DateTime.Now.ToString() + " -- CoyID = " + CoyID);

            DataTable dtCompany = oDAL.GMS_Get_CompanyByCoyID(CoyID);
            if (dtCompany.Rows.Count > 0)
            {
                //Connect to WebService
                Console.WriteLine(DateTime.Now.ToString() + " -- Connecting WebServer...");
                CMSWebService.CMSWebService ws = new CMSWebService.CMSWebService();
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(dtCompany.Rows[0]["CMSWebServiceAddress"].ToString().Trim()))
                {
                    ws.Url = dtCompany.Rows[0]["CMSWebServiceAddress"].ToString().Trim();
                }
                else
                {
                    ws.Url = _CMSDefaultURL;
                }

                //Retrieve Account Address data
                Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Account Address data...");
                ds = ws.GetAccountAddress();

                //Clean Account Address data in GMS
                Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Account Address data in GMS...");
                oDAL.GMS_Delete_AccountAddress(CoyID);

                //Insert Account Address data into GMS
                Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Account Address data into GMS...");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    oDAL.GMS_Insert_AccountAddress(CoyID, dr["AccountCode"].ToString(), dr["AddressType"].ToString(), int.Parse(!string.IsNullOrEmpty(dr["AddressID"].ToString()) ? dr["AddressID"].ToString() : "0"), dr["ContactPerson"].ToString(),
                        dr["Address1"].ToString(), dr["Address2"].ToString(), dr["Address3"].ToString(), dr["Address4"].ToString(), dr["PostalCode"].ToString(),
                        dr["TransportZone"].ToString(), dr["OfficePhone"].ToString(), dr["MobilePhone"].ToString(), dr["Fax"].ToString(), dr["Email"].ToString(),
                        dr["SpecialInstruction"].ToString(), bool.Parse(dr["IsActive"].ToString()), short.Parse(!string.IsNullOrEmpty(dr["CreatedBy"].ToString()) ? dr["CreatedBy"].ToString() : "1"), DateTime.Parse(!string.IsNullOrEmpty(dr["CreatedDate"].ToString()) ? dr["CreatedDate"].ToString() : DateTime.Now.ToString()), dr["OrderedBy"].ToString(),
                        dr["AddressCode"].ToString());
                }
                Console.WriteLine(DateTime.Now.ToString() + " -- End Account Address data insertion for " + CoyID.ToString());
                ds.Dispose();
            }
        }

        #region Function for Task_MR
        static void ImportProduct(short CoyID, string URL)
        {
            //Step 1 : Retrieve Company Product from WebService
            GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
            if (!string.IsNullOrEmpty(URL))
            {
                sc.Url = URL;
            }
            else
            {
                sc.Url = _GMSDefaultURL;
            }
            DataTable dtProduct = sc.GetAllProduct(CoyID).Tables[0];
            int i = 0;
            Console.WriteLine(DateTime.Now.ToString() + " -- Total: " + dtProduct.Rows.Count + " Product records found!");
            if (dtProduct.Rows.Count > 0)
            {

                //Step 2 : Delete Comapany Product from ScheduledTaskProduct
                oDAL.GMS_Delete_ScheduledTaskProductByCoyID(CoyID);

                //Step 3 : Delete Comapany Product Description from ScheduledTaskProductDescription
                oDAL.GMS_Delete_ScheduledTaskProductDescriptionByCoyID(CoyID);

                //Step 4 : Insert Product  ScheduledTaskProductDescription
                foreach (DataRow drP in dtProduct.Rows)
                {
                    i++;
                    DataTable dtProductED = sc.GetProductExtendedDescriptionByProductCode(CoyID, drP["ProductCode"].ToString()).Tables[0];
                    foreach (DataRow drPED in dtProductED.Rows)
                    {
                        oDAL.GMS_Insert_ScheduledTaskProductDescription(CoyID, drP["ProductCode"].ToString(),
                            Convert.ToInt16(drPED["DescNo"].ToString()), drPED["ProdDetail"].ToString());
                    }

                    oDAL.GMS_Insert_ScheduledTaskProduct(CoyID, drP["ProductCode"].ToString(), drP["ProductName"].ToString(), drP["ProductGroupCode"].ToString(),
                        float.Parse(!string.IsNullOrEmpty(drP["Volume"].ToString()) ? drP["Volume"].ToString() : "0"), drP["UOM"].ToString(), float.Parse(drP["WeightedCost"].ToString()),
                        float.Parse(drP["WarehouseHQ"].ToString()), float.Parse(drP["Warehouse77"].ToString()), float.Parse(drP["WarehouseOthers"].ToString()),
                        float.Parse(drP["OnSOQty"].ToString()), float.Parse(drP["OnPOQty"].ToString()));

                    Console.Write("\r{0}/{1} Inserted...", i.ToString(), dtProduct.Rows.Count.ToString());
                }
                Console.WriteLine("");
                Console.WriteLine(DateTime.Now.ToString() + " -- Inserting/Updating Product..");
                oDAL.GMS_Update_ProductFromScheduledTaskProduct();
                if (!string.IsNullOrEmpty(oDAL.ex))
                {
                    Console.WriteLine(DateTime.Now.ToString() + " -- " + oDAL.ex);
                }
            }
            else
            {

            }
            Console.WriteLine(DateTime.Now.ToString() + " -- Import Product Successfully!");
            dtProduct.Dispose();
        }

        static void ImportGRNDetail(short CoyID, string URL)
        {
            //Step 1 : Retrieve GRN Detail from WebService
            GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
            if (!string.IsNullOrEmpty(URL))
            {
                sc.Url = URL;
            }
            else
            {
                sc.Url = _GMSDefaultURL;
            }
            DataTable dtGRNDetail = sc.GetMRGRNDetailFromA21(CoyID).Tables[0];
            int i = 0;
            Console.WriteLine(DateTime.Now.ToString() + " -- Total: " + dtGRNDetail.Rows.Count + " GRNDetail records found!");
            if (dtGRNDetail.Rows.Count > 0)
            {

                //Step 2 : Delete GRN Detail from ScheduledTaskGRNDetail
                oDAL.GMS_Delete_ScheduledTaskGRNDetailByCoyID(CoyID);

                //Step 3 : Insert ScheduledTaskProductGRNDetail 
                foreach (DataRow dr in dtGRNDetail.Rows)
                {
                    i++;
                    oDAL.GMS_Insert_ScheduledTaskGRNDetail(CoyID, dr["PONo"].ToString(), Convert.ToDateTime(dr["PODate"].ToString()),
                        dr["GRNNo"].ToString(), Convert.ToDateTime(dr["GRNDate"].ToString()), dr["Product"].ToString(), dr["Purchaser"].ToString(),
                        float.Parse(dr["Quantity"].ToString()), dr["trntype"].ToString() + dr["trnno"].ToString());

                    Console.Write("\r{0}/{1} Inserted...", i.ToString(), dtGRNDetail.Rows.Count.ToString());
                }
                Console.WriteLine("");
                Console.WriteLine(DateTime.Now.ToString() + " -- Inserting/Updating GRN Detail..");
                oDAL.GRN_Update_GRNDetailFromScheduledTaskGRNDetail();
                if (!string.IsNullOrEmpty(oDAL.ex))
                {
                    Console.WriteLine(DateTime.Now.ToString() + " -- " + oDAL.ex);
                }
            }
            else
            {

            }
            Console.WriteLine(DateTime.Now.ToString() + " -- Import GRNDetail Successfully!");
            dtGRNDetail.Dispose();
        }

        static void CloseMR(short CoyID)
        {
            DataTable dtMR = oDAL.GMS_Get_MRByCoyIDStatusID(CoyID, "P");
            int i = 0;
            foreach (DataRow drMR in dtMR.Rows)
            {
                i++;
                bool isComplete = true;
                DataTable dtMRDetail = oDAL.GMS_Get_MRDetailByCoyIDMRNo(CoyID, drMR["MRNo"].ToString());
                DataTable dtMRDelivery = oDAL.GMS_Get_MRDeliveryByCoyIDMRNo(CoyID, drMR["MRNo"].ToString());
                foreach (DataRow drMRDetail in dtMRDetail.Rows)
                {
                    double quantity = 0;
                    foreach (DataRow drMRDelivery in dtMRDelivery.Rows)
                    {
                        if (!string.IsNullOrEmpty(drMRDelivery["PONo"].ToString()))
                        {
                            DataTable dtMRGRNDetail = oDAL.GMS_Get_MRGRNDetailByCoyIDProductPO(CoyID, drMRDetail["ProdCode"].ToString(), drMRDelivery["PONo"].ToString());
                            foreach (DataRow drMRGRNDetail in dtMRGRNDetail.Rows)
                            {
                                quantity += float.Parse(drMRGRNDetail["Quantity"].ToString());
                            }
                        }
                    }

                    if (float.Parse(drMRDetail["OrderQty"].ToString()) > quantity)
                    {
                        isComplete = false;
                        break;
                    }
                }

                if (isComplete)
                {
                    oDAL.GMS_Update_MR(CoyID, drMR["MRNo"].ToString());
                    //Console.WriteLine("Closed MR " + drMR["MRNo"].ToString());  
                }
                Console.Write("\r{0}/{1} Checked...", i.ToString(), dtMR.Rows.Count.ToString());
            }

            Console.WriteLine("");
            Console.WriteLine(DateTime.Now.ToString() + " -- Close MR Successfully!");
            dtMR.Dispose();
        }
        #endregion

        static void Task_LHMImport(short CoyID)
        {
            Console.WriteLine(DateTime.Now.ToString() + " -- Start Task : LHMImport");
            Console.WriteLine(DateTime.Now.ToString() + " -- CoyID = " + CoyID);

            DataTable dtCompany = oDAL.GMS_Get_CompanyByCoyID(CoyID);
            if (dtCompany.Rows.Count > 0)
            {
                //Connect to WebService
                Console.WriteLine(DateTime.Now.ToString() + " -- Connecting WebServer...");
                CMSWebService.CMSWebService ws = new CMSWebService.CMSWebService();
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(dtCompany.Rows[0]["CMSWebServiceAddress"].ToString().Trim()))
                {
                    ws.Url = dtCompany.Rows[0]["CMSWebServiceAddress"].ToString().Trim();
                }
                else
                {
                    ws.Url = _CMSDefaultURL;
                }

                // Delete Sales Person               
                Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Sales Person data in GMS...");
                oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteSalesPerson");

                // Delete Product
                Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Product data in GMS...");
                oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteProduct");
                //Retrieve Product data
                Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Product data...");
                ds = ws.GetProduct();
                //Insert Product data into GMS
                Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Product data into GMS...");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    oDAL.GMS_Insert_Product(CoyID,
                        dr["productcode"].ToString(),
                        dr["productname"].ToString(),
                        dr["ProductGroupCode"].ToString(),
                        dr["uom"].ToString()
                        );
                }
                Console.WriteLine(DateTime.Now.ToString() + " -- End Product data insertion for " + CoyID.ToString());
                ds.Dispose();

                // Delete Product 
                Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Duplicate Product data in GMS...");
                oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteDuplicateProduct");

                // Delete Product Group
                Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Product Group data in GMS...");
                oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteProductGroup");

                //Retrieve Product Group data
                Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Product Group data...");
                ds = ws.GetProductGroup();
                //Insert Product Group data into GMS
                Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Product Group data into GMS...");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    oDAL.GMS_Insert_ProductGroup(CoyID,
                        dr["ProductGroupCode"].ToString(),
                        dr["ProductGroupName"].ToString(),"","",""
                        );
                }
                Console.WriteLine(DateTime.Now.ToString() + " -- End Product Group data insertion for " + CoyID.ToString());
                ds.Dispose();

                // Delete Account
                Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Account data in GMS...");
                oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteAccount");
                // Delete Sales
                Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Sales data in GMS...");
                oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteSales");
                // Delete Sales Detail
                Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Sales Detail data in GMS...");
                oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteSalesDetail");
                // Delete Receipt
                Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Receipt data in GMS...");
                oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteReceipt");
                // Delete GRN
                Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete GRN data in GMS...");
                oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteGRN");
                // Delete Stock Movement
                Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Product Movement data in GMS...");
                oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteStockMovement");

                //Retrieve Last Month GRN
                Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving GRN data...");
                ds = ws.GetGRN();
                //Insert GRN data into GMS
                Console.WriteLine(DateTime.Now.ToString() + " -- Inserting GRN data into GMS...");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    oDAL.GMS_Insert_GRN(CoyID,
                     dr["trnno"].ToString(),
                     GMSUtil.ToDate(dr["trndate"].ToString()),
                     dr["pono"].ToString(),
                     GMSUtil.ToDate(dr["podate"].ToString()),
                     dr["productcode"].ToString(),
                     dr["ProductName"].ToString(),
                     dr["ProductGroupCode"].ToString(),
                     dr["ProductGroupName"].ToString(),
                     GMSUtil.ToFloat(dr["Quantity"].ToString()),
                     GMSUtil.ToFloat(dr["unitprice"].ToString()),
                     GMSUtil.ToFloat(dr["landercostunitprice"].ToString()),
                     GMSUtil.ToFloat(dr["cost"].ToString()),
                     dr["currency"].ToString(),
                     GMSUtil.ToFloat(dr["exchangerate"].ToString())
                    );
                }
                Console.WriteLine(DateTime.Now.ToString() + " -- End GRN data insertion");
                ds.Dispose();


                //Retrieve StockMovement
                Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Stock Movement data...");
                ds = ws.GetStockMovement();
                //Insert StockMovement data into GMS
                Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Stock Movement data into GMS...");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    oDAL.GMS_Insert_StockMovement(CoyID,
                     dr["TrnType"].ToString(),
                     GMSUtil.ToInt(dr["TrnNo"].ToString()),
                     GMSUtil.ToDate(dr["TrnDate"].ToString()),
                     dr["RefNo"].ToString(),
                     dr["accountcode"].ToString(),
                     dr["AccountName"].ToString(),
                     dr["ProductCode"].ToString(),
                     dr["ProductName"].ToString(),
                     dr["ProductGroupCode"].ToString(),
                     dr["ProductGroupName"].ToString(),
                     GMSUtil.ToFloat(dr["receivedquantity"].ToString()),
                     GMSUtil.ToFloat(dr["issuedquantity"].ToString()),
                     GMSUtil.ToFloat(dr["balancequantity"].ToString()),
                     GMSUtil.ToFloat(dr["cost"].ToString()),
                     GMSUtil.ToFloat(dr["CostWT"].ToString()),
                     dr["currency"].ToString(),
                     GMSUtil.ToFloat(dr["exchangerate"].ToString()),
                     dr["narration"].ToString()
                    );

                }
                Console.WriteLine(DateTime.Now.ToString() + " -- End Stock Movement data insertion");
                ds.Dispose();


                string tmpURL = ws.Url;

                for (int i = 1; i < 11; i++)
                {
                    ws.Url = tmpURL.Replace("3", i.ToString());

                    //Retrieve Sales Person
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Sales Person data...");
                    ds = ws.GetSalesPerson(i.ToString());
                    //Insert Sales Person data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Sales Person data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_SalesPerson(CoyID,
                        dr["salespersonid"].ToString(),
                        dr["salespersonname"].ToString(),
                        "", "", "",""
                        );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Sales Person data insertion for " + i.ToString());
                    ds.Dispose();

                    //Retrieve Account
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Account data...");
                    ds = ws.GetAccount(i.ToString());
                    //Insert Account data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Account data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_Account(CoyID,
                        dr["AccountCode"].ToString(),
                        dr["AccountType"].ToString(),
                        dr["AccountName"].ToString(),
                        dr["SalesPersonID"].ToString(),
                        dr["DefaultCurrency"].ToString(),
                        dr["TaxType"].ToString(),
                        GMSUtil.ToShort(dr["CreditTerm"].ToString()),
                        GMSUtil.ToFloat(dr["CreditLimit"].ToString()),
                        dr["Address1"].ToString(),
                        dr["Address2"].ToString(),
                        dr["Address3"].ToString(),
                        dr["Address4"].ToString(),
                        dr["PostalCode"].ToString(),
                        dr["ContactPerson"].ToString(),
                        dr["OfficePhone"].ToString(),
                        dr["MobilePhone"].ToString(),
                        dr["Fax"].ToString(),
                        dr["Email"].ToString(),
                        dr["Industry"].ToString(),
                        dr["Country"].ToString(),
                        GMSUtil.ToDate(dr["CreatedDate"].ToString()),
                        "",
                        "",
                        "",
                        0
                        );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Account data insertion for " + i.ToString());
                    ds.Dispose();

                    //Retrieve Sales I
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Sales I data...");
                    ds = ws.GetSales1(i.ToString());
                    //Insert Sales I data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Sales I data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        oDAL.GMS_Insert_Sales(CoyID,
                        dr["trntype"].ToString(),
                        dr["trnno"].ToString(),
                        GMSUtil.ToDate(dr["trndate"].ToString()),
                        dr["accountcode"].ToString(),
                        dr["accountname"].ToString(),
                        dr["docno"].ToString(),
                        dr["pono"].ToString(),
                        GMSUtil.ToFloat(dr["amount"].ToString()),
                        dr["currency"].ToString(),
                        GMSUtil.ToFloat(dr["exchangerate"].ToString()),
                        GMSUtil.ToFloat(dr["taxrate"].ToString()),
                        dr["customersalespersonid"].ToString(),
                        dr["transactionsalespersonid"].ToString(),
                        GMSUtil.ToDate(""), GMSUtil.ToDate("")
                        );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Sales I data insertion for " + i.ToString());
                    ds.Dispose();

                    //Retrieve Sales II
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Sales II data...");
                    ds = ws.GetSales2(i.ToString());
                    //Insert Sales II data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Sales II data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_Sales(CoyID,
                        dr["trntype"].ToString(),
                        dr["trnno"].ToString(),
                        GMSUtil.ToDate(dr["trndate"].ToString()),
                        dr["accountcode"].ToString(),
                        dr["accountname"].ToString(),
                        dr["docno"].ToString(),
                        dr["pono"].ToString(),
                        GMSUtil.ToFloat(dr["amount"].ToString()),
                        dr["currency"].ToString(),
                        GMSUtil.ToFloat(dr["exchangerate"].ToString()),
                        GMSUtil.ToFloat(dr["taxrate"].ToString()),
                        dr["customersalespersonid"].ToString(),
                        dr["transactionsalespersonid"].ToString(),
                        GMSUtil.ToDate(""), GMSUtil.ToDate("")
                        );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Sales II data insertion for " + i.ToString());
                    ds.Dispose();

                    //Retrieve Sales Detail
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Sales Detail data...");
                    ds = ws.GetSalesDetail(i.ToString());
                    //Insert Sales Detail data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Sales Detail data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_SalesDetail(CoyID,
                        dr["trntype"].ToString(),
                        dr["trnno"].ToString(),
                        GMSUtil.ToDate(dr["trndate"].ToString()),
                        GMSUtil.ToShort(dr["srno"].ToString()),
                        dr["accountcode"].ToString(),
                        dr["accountname"].ToString(),
                        dr["productcode"].ToString(),
                        dr["ProdName"].ToString(),
                        dr["productgroupcode"].ToString(),
                        dr["productgroupname"].ToString(),
                        GMSUtil.ToFloat(dr["qty"].ToString()),
                        GMSUtil.ToFloat(dr["UnitCost"].ToString()),
                        GMSUtil.ToFloat(dr["UnitAmount"].ToString()),
                        GMSUtil.ToFloat(dr["cost"].ToString()),
                        GMSUtil.ToFloat(dr["amount"].ToString()),
                        GMSUtil.ToFloat(dr["gpamount"].ToString()),
                        dr["CurrencyCode"].ToString(),
                        GMSUtil.ToFloat(dr["CurrencyRate"].ToString()),
                        GMSUtil.ToFloat(dr["TaxRate"].ToString()),
                        dr["dono"].ToString(),
                        dr["location"].ToString(),
                        dr["customersalespersonid"].ToString(),
                        dr["transactionsalespersonid"].ToString(),
                        GMSUtil.ToDate(""), GMSUtil.ToDate("")
                        );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Sales Detail data insertion for " + i.ToString());
                    ds.Dispose();


                    //Retrieve Receipt I
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Receipt I data...");
                    ds = ws.GetReceipt1(i.ToString());
                    //Insert Receipt I data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Receipt I data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_Receipt(CoyID,
                     dr["trntype"].ToString(),
                     dr["trnno"].ToString(),
                     GMSUtil.ToDate(dr["TrnDate"].ToString()),
                     dr["accountcode"].ToString(),
                     dr["accountname"].ToString(),
                     dr["salestrntype"].ToString(),
                     dr["salestrnno"].ToString(),
                     dr["docno"].ToString(),
                     dr["allcdocno"].ToString(),
                     GMSUtil.ToFloat(dr["amount"].ToString()),
                     dr["currency"].ToString(),
                     GMSUtil.ToFloat(dr["exchangerate"].ToString())
                     );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Receipt I data insertion for " + i.ToString());
                    ds.Dispose();

                    //Retrieve Receipt II
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Receipt II data...");
                    ds = ws.GetReceipt2(i.ToString());
                    //Insert Receipt II data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Receipt II data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_Receipt(CoyID,
                      dr["trntype"].ToString(),
                      dr["trnno"].ToString(),
                      GMSUtil.ToDate(dr["TrnDate"].ToString()),
                      dr["accountcode"].ToString(),
                      dr["accountname"].ToString(),
                      dr["salestrntype"].ToString(),
                      dr["salestrnno"].ToString(),
                      dr["docno"].ToString(),
                      dr["allcdocno"].ToString(),
                      GMSUtil.ToFloat(dr["amount"].ToString()),
                      dr["currency"].ToString(),
                      GMSUtil.ToFloat(dr["exchangerate"].ToString())
                      );

                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Receipt II data insertion for " + i.ToString());
                    ds.Dispose();

                    //Retrieve Receipt III
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Receipt III data...");
                    ds = ws.GetReceipt3(i.ToString());
                    //Insert Receipt III data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Receipt III data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_Receipt(CoyID,
                      dr["trntype"].ToString(),
                      dr["trnno"].ToString(),
                      GMSUtil.ToDate(dr["TrnDate"].ToString()),
                      dr["accountcode"].ToString(),
                      dr["accountname"].ToString(),
                      dr["salestrntype"].ToString(),
                      dr["salestrnno"].ToString(),
                      dr["docno"].ToString(),
                      dr["allcdocno"].ToString(),
                      GMSUtil.ToFloat(dr["amount"].ToString()),
                      dr["currency"].ToString(),
                      GMSUtil.ToFloat(dr["exchangerate"].ToString())
                      );

                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Receipt III data insertion for " + i.ToString());
                    ds.Dispose();
                }

                // Delete Duplicate Account
                Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Duplicate Account data in GMS...");
                oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteDuplicateAccount");

                // Update Sales Trn Type CCAmount
                Console.WriteLine(DateTime.Now.ToString() + " -- Update Sales Trn Type CC Amount in GMS...");
                oDAL.GMS_ImportUpdateDataByAction(CoyID, "UpdateSalesTrnTypeCCAmount");
            }
        }

        static void Task_LMSImportProduct(short CoyID, string url, bool execute, string from, string to, string SAPURI, string SAPKEY, string SAPDB, bool executeFX)
        {
            string tempProductName = "";

            DataTable dtCompany = oDAL.GMS_Get_CompanyByCoyID(CoyID);
            if (dtCompany.Rows.Count > 0)
            {
                //Connect to WebService
                Console.WriteLine(DateTime.Now.ToString() + " -- Connecting WebServer...");
                CMSWebService.CMSWebService ws = new CMSWebService.CMSWebService();
                ws.Url = url;
                DataSet ds = new DataSet();
                string query = "";

                SAPOperation sop = new SAPOperation();
                sop.BaseAddress = SAPURI;
                sop.SAPKey = SAPKEY;
                sop.SAPDB = SAPDB;

                if (execute)
                {

                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Product UOM data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteProductUOM");

                    //ProductUOM
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Product UOM data...");
                    //ds = ws.GetProductUOM("");
                    query = "SELECT * FROM \"@AF_UOM1\"";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                        "field1", "field2", "field3", "field4", "field5", "field6", "field7", "field8", "field9", "field10", "field11", "field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                    //Insert Product UOM data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Product UOM data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_ProductUOM(CoyID,
                            dr["field1"].ToString(),
                            dr["field6"].ToString(),
                            GMSUtil.ToFloat(dr["field7"].ToString()) / GMSUtil.ToFloat(dr["field5"].ToString()),
                            GMSUtil.ToFloat(dr["field5"].ToString()),
                            dr["field6"].ToString(),
                            GMSUtil.ToFloat(dr["field7"].ToString()),
                            dr["field8"].ToString()
                            );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Product UOM data insertion for " + CoyID.ToString());
                    ds.Dispose();
                }

                if (execute)
                {
                    //Retrieve Product data
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Product data...");
                    //ds = ws.GetProduct();

                    query = "CALL \"AF_API_GET_SAP_ITEMMASTERINFO\" ('','','','','','')";

                    ds = sop.GET_SAP_QueryData(CoyID, query,
                    "ProductCode", "ProductName", "ProductGroupCode", "Volume", "UOM", "WeightedCost", "OnSOQty", "OnPOQty", "OnBOQty", "OnHandQty", "IsGasDivision", "IsWeldingDivision", "ProdForeignName", "TrackedByBatch", "TrackedBySerial", "ProductNotes", "IsActive", "ItemType", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    bool IsGasDivision = false;
                    bool IsWeldingDivision = false;
                    bool TrackedByBatch = false;
                    bool TrackedBySerial = false;
                    string ProductNotes = "";
                    int VersionNo = 0;

                    tempProductName = "";
                    string[] stringSeparators = new string[] { "\r\n", "\r", "\n" };
                    //Insert Product data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Product data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tempProductName = dr["ProductName"].ToString().Replace("'", "''");

                        if (dr["IsGasDivision"].ToString() == "1")
                            IsGasDivision = true;
                        else
                            IsGasDivision = false;

                        if (dr["IsWeldingDivision"].ToString() == "1")
                            IsWeldingDivision = true;
                        else
                            IsWeldingDivision = false;

                        if (dr["TrackedByBatch"].ToString() == "1")
                            TrackedByBatch = true;
                        else
                            TrackedByBatch = false;

                        if (dr["TrackedBySerial"].ToString() == "1")
                            TrackedBySerial = true;
                        else
                            TrackedBySerial = false;

                        if (dr["IsActive"].ToString() == "true")
                            VersionNo = 0;
                        else
                            VersionNo = 1;

                        ProductNotes = dr["ProductNotes"].ToString();

                        if (dr["ItemType"].ToString() != "F")
                        {
                            oDAL.GMS_Insert_Product2(CoyID,
                                dr["ProductCode"].ToString(),
                                tempProductName,
                                dr["ProductGroupCode"].ToString(),
                                GMSUtil.ToDouble(dr["Volume"].ToString()),
                                dr["UOM"].ToString(),
                                GMSUtil.ToDouble(dr["WeightedCost"].ToString()),
                                GMSUtil.ToDouble(dr["OnSOQty"].ToString()),
                                GMSUtil.ToDouble(dr["OnPOQty"].ToString()),
                                GMSUtil.ToDouble(dr["OnBOQty"].ToString()),
                                GMSUtil.ToDouble(dr["OnHandQty"].ToString()),
                                IsGasDivision,
                                IsWeldingDivision,
                                dr["ProdForeignName"].ToString(),
                                TrackedByBatch,
                                TrackedBySerial,
                                VersionNo
                                );

                            if (ProductNotes != "")
                            {
                                string[] arrProductNotes = ProductNotes.Split(stringSeparators, StringSplitOptions.None);

                                if (arrProductNotes.Length > 0)
                                {
                                    oDAL.GMS_Delete_ProductNotes(CoyID,
                                        dr["ProductCode"].ToString()
                                    );

                                    for (int i = 0; i < arrProductNotes.Length; i++)
                                    {
                                        oDAL.GMS_Insert_ProductNotes(CoyID,
                                        dr["ProductCode"].ToString(),
                                        i + 1,
                                        arrProductNotes[i].ToString()
                                        );
                                    }
                                }
                            }
                        }

                    }

                    Console.WriteLine(DateTime.Now.ToString() + " -- End Product data insertion for " + CoyID.ToString());
                    ds.Dispose();

                    // Delete Product 
                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Duplicate Product data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteDuplicateProduct");
                }

            }
        }

        static void Task_LMSImport(short CoyID)
        {

            string from = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1).ToString("yyyy-MM-dd");
            string to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("yyyy-MM-dd");

            DataTable dtCompany = oDAL.GMS_Get_CompanyByCoyID(CoyID);
            if (dtCompany.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dtCompany.Rows[0]["GASLMSWebServiceAddress"].ToString().Trim()) && !string.IsNullOrEmpty(dtCompany.Rows[0]["WSDLMSWebServiceAddress"].ToString().Trim()))
                {
                    Task_LMSImportData(CoyID, dtCompany.Rows[0]["GASLMSWebServiceAddress"].ToString().Trim(), true, from, to, dtCompany.Rows[0]["SAPURI"].ToString().Trim(), dtCompany.Rows[0]["SAPKEY"].ToString().Trim(), dtCompany.Rows[0]["SAPDB"].ToString().Trim(), true, Convert.ToBoolean(dtCompany.Rows[0]["ImplementedJobTraveller"].ToString()));
                    //Task_LMSImportData(CoyID, dtCompany.Rows[0]["WSDLMSWebServiceAddress"].ToString().Trim(), false, from, to);
                }
                else
                {
                    Task_LMSImportData(CoyID, dtCompany.Rows[0]["CMSWebServiceAddress"].ToString().Trim(), true, from, to, dtCompany.Rows[0]["SAPURI"].ToString().Trim(), dtCompany.Rows[0]["SAPKEY"].ToString().Trim(), dtCompany.Rows[0]["SAPDB"].ToString().Trim(), false, Convert.ToBoolean(dtCompany.Rows[0]["ImplementedJobTraveller"].ToString()));
                }
            }
        }

        static void Task_ImportProduct(short CoyID)
        {

            string from = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1).ToString("yyyy-MM-dd");
            string to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("yyyy-MM-dd");

            DataTable dtCompany = oDAL.GMS_Get_CompanyByCoyID(CoyID);
            if (dtCompany.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dtCompany.Rows[0]["GASLMSWebServiceAddress"].ToString().Trim()) && !string.IsNullOrEmpty(dtCompany.Rows[0]["WSDLMSWebServiceAddress"].ToString().Trim()))
                {
                    Task_LMSImportProduct(CoyID, dtCompany.Rows[0]["GASLMSWebServiceAddress"].ToString().Trim(), true, from, to, dtCompany.Rows[0]["SAPURI"].ToString().Trim(), dtCompany.Rows[0]["SAPKEY"].ToString().Trim(), dtCompany.Rows[0]["SAPDB"].ToString().Trim(), true);
                    //Task_LMSImportData(CoyID, dtCompany.Rows[0]["WSDLMSWebServiceAddress"].ToString().Trim(), false, from, to);
                }
                else
                {
                    Task_LMSImportProduct(CoyID, dtCompany.Rows[0]["CMSWebServiceAddress"].ToString().Trim(), true, from, to, dtCompany.Rows[0]["SAPURI"].ToString().Trim(), dtCompany.Rows[0]["SAPKEY"].ToString().Trim(), dtCompany.Rows[0]["SAPDB"].ToString().Trim(), false);
                }
            }
        }


        static void Task_LMSImportSalesPerson(short CoyID)
        {

            string from = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1).ToString("yyyy-MM-dd");
            string to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("yyyy-MM-dd");

            DataTable dtCompany = oDAL.GMS_Get_CompanyByCoyID(CoyID);
            if (dtCompany.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dtCompany.Rows[0]["GASLMSWebServiceAddress"].ToString().Trim()) && !string.IsNullOrEmpty(dtCompany.Rows[0]["WSDLMSWebServiceAddress"].ToString().Trim()))
                {
                    Task_LMSImportDataSalesPerson(CoyID, dtCompany.Rows[0]["GASLMSWebServiceAddress"].ToString().Trim(), true, from, to, dtCompany.Rows[0]["SAPURI"].ToString().Trim(), dtCompany.Rows[0]["SAPKEY"].ToString().Trim(), dtCompany.Rows[0]["SAPDB"].ToString().Trim(), true);
                    //Task_LMSImportData(CoyID, dtCompany.Rows[0]["WSDLMSWebServiceAddress"].ToString().Trim(), false, from, to);
                }
                else
                {
                    Task_LMSImportDataSalesPerson(CoyID, dtCompany.Rows[0]["CMSWebServiceAddress"].ToString().Trim(), true, from, to, dtCompany.Rows[0]["SAPURI"].ToString().Trim(), dtCompany.Rows[0]["SAPKEY"].ToString().Trim(), dtCompany.Rows[0]["SAPDB"].ToString().Trim(), false);
                }
            }
        }

        static void Task_LMSImportDataSalesPerson(short CoyID, string url, bool execute, string from, string to, string SAPURI, string SAPKEY, string SAPDB, bool executeFX)
        {

            Console.WriteLine(DateTime.Now.ToString() + " -- Start Task : LHMImport");
            Console.WriteLine(DateTime.Now.ToString() + " -- CoyID = " + CoyID);

            DataTable dtCompany = oDAL.GMS_Get_CompanyByCoyID(CoyID);
            if (dtCompany.Rows.Count > 0)
            {
                DataSet ds = new DataSet();
                string query = "";

                SAPOperation sop = new SAPOperation();
                sop.BaseAddress = SAPURI;
                sop.SAPKey = SAPKEY;
                sop.SAPDB = SAPDB;

                if (execute)
                {


                    //Retrieve Sales Person
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Sales Person data...");
                    query = "SELECT * FROM OSLP";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                        "field1", "SalespersonName", "SalespersonID", "field4", "field5", "field6", "field7", "field8", "field9", "Active", "field11", "field12", "Field13", "Field14", "Field15", "Division", "Field17", "ShortName", "Team", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    //Insert Sales Person data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Sales Person data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["field15"].ToString() == "S")
                            oDAL.GMS_Insert_SalesPerson(CoyID, dr["SalespersonID"].ToString(), dr["SalespersonName"].ToString(), dr["Division"].ToString(), dr["ShortName"].ToString(), dr["Team"].ToString(), dr["Active"].ToString());
                        else if (dr["field15"].ToString() == "B")
                            oDAL.GMS_Insert_Purchaser(CoyID, dr["SalespersonID"].ToString(), dr["SalespersonName"].ToString(), "");
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Sales Person & Purchaser data insertion");
                    ds.Dispose();
                    /*
                    string from4 = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-3).ToString("yyyy-MM-dd");
                    //string to4 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).AddMonths(-3).ToString("yyyy-MM-dd");
                    string to4 = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-2).AddDays(-1).ToString("yyyy-MM-dd");

                    string from3 = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-2).ToString("yyyy-MM-dd");
                    //string to3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).AddMonths(-2).ToString("yyyy-MM-dd");
                    string to3 = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");

                    string from1 = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1).ToString("yyyy-MM-dd");
                    //string to1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).AddMonths(-1).ToString("yyyy-MM-dd");
                    string to1 = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddDays(-1).ToString("yyyy-MM-dd");

                    string from2 = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
                    string to2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("yyyy-MM-dd");

                    Console.WriteLine(from4 + " " + to4);
                    Task_ImportDataJobTraveller(CoyID, from4, to4, sop);
                    Console.WriteLine(from3 + " " + to3);
                    Task_ImportDataJobTraveller(CoyID, from3, to3, sop);
                    Console.WriteLine(from1 + " " + to1);
                    Task_ImportDataJobTraveller(CoyID, from1, to1, sop);
                    Console.WriteLine(from2 + " " + to2);
                    Task_ImportDataJobTraveller(CoyID, from2, to2, sop);
                    */
                }

                if (execute)
                {
                    //Retrieve Closed MRNo from PO
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Closed MR Data...");
                    query = "SELECT \"U_AF_DOCNUM\" FROM OPOR WHERE \"DocStatus\" = 'C' AND  \"U_AF_DOCNUM\" <> ''";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                        "MRNo", "Field2", "Field3", "Field4", "Field5", "Field6", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    Console.WriteLine(DateTime.Now.ToString() + " -- Updating Closed MR status in GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Update_CloseMRStatus(CoyID, dr["MRNo"].ToString());
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Closed MR status update");
                    ds.Dispose();
                }
            }
        }

        static void Task_ImportDataJobTraveller(short CoyID, string from, string to, SAPOperation sop)
        {
            DataSet ds = new DataSet();
            string query = "";

            Console.WriteLine(from + " -- " + to);

            Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Job Traveller data...");
            query = "CALL \"AF_MFG_QRY_MFGCOST_WIP\" ('" + from + "' , '" + to + "', '', '' )";
            ds = sop.GET_SAP_QueryData(CoyID, query,
            "JobTravellerNo", "ProductionOrderNo", "FinalFG", "FinalFGDescription", "BOMTemplate", "BOMLevel", "BOMParent", "CompletionQty", "Category", "ChildCode", "ChildDescription", "BaseQuantity", "UOM", "Quantity", "GLCode", "Amount", "LastProductionIssueDate", "LastProductionReceiptDate", "PlannedQty", "JobTravellerStatus",
            "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

            //Insert JobTraveller data into GMS
            Console.WriteLine(DateTime.Now.ToString() + " -- Inserting JobTraveller data into GMS...");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                oDAL.GMS_Insert_JobTraveller(CoyID,
                 GMSUtil.ToDate(to),
                 GMSUtil.ToInt(dr["JobTravellerNo"].ToString()),
                 dr["ProductionOrderNo"].ToString(),
                 dr["FinalFG"].ToString(),
                 dr["FinalFGDescription"].ToString(),
                 dr["BOMTemplate"].ToString(),
                 GMSUtil.ToInt(dr["BOMLevel"].ToString()),
                 dr["BOMParent"].ToString(),
                 GMSUtil.ToInt(dr["CompletionQty"].ToString()),
                 dr["Category"].ToString(),
                 dr["ChildCode"].ToString(),
                 dr["ChildDescription"].ToString(),
                 GMSUtil.ToDouble(dr["BaseQuantity"].ToString()),
                 dr["UOM"].ToString(),
                 GMSUtil.ToDouble(dr["Quantity"].ToString()),
                 dr["GLCode"].ToString(),
                 GMSUtil.ToDouble(dr["Amount"].ToString()),
                 GMSUtil.ToDate(dr["LastProductionIssueDate"].ToString()),
                 GMSUtil.ToDate(dr["LastProductionReceiptDate"].ToString()),
                 GMSUtil.ToDouble(dr["PlannedQty"].ToString()),
                 dr["JobTravellerStatus"].ToString()
                );
            }
            Console.WriteLine(DateTime.Now.ToString() + " -- End Job Traveller data insertion");
            ds.Dispose();
        }

        static void Task_LMSImportData(short CoyID, string url, bool execute, string from, string to, string SAPURI, string SAPKEY, string SAPDB, bool executeFX, bool implementedJobTraveller)
        {
            Console.WriteLine(DateTime.Now.ToString() + " -- Start Task : Close MR");
            oDAL.GMS_Update_MR_SAP(CoyID);
            Console.WriteLine(DateTime.Now.ToString() + " -- End Update MR Status");

            Console.WriteLine(DateTime.Now.ToString() + " -- Start Task : LHMImport");
            Console.WriteLine(DateTime.Now.ToString() + " -- CoyID = " + CoyID);
            string tempProductName = "";

            DataTable dtCompany = oDAL.GMS_Get_CompanyByCoyID(CoyID);
            if (dtCompany.Rows.Count > 0)
            {
                if (url != "")
                {
                    //Connect to WebService               
                    Console.WriteLine(DateTime.Now.ToString() + " -- Connecting WebServer...");
                    CMSWebService.CMSWebService ws = new CMSWebService.CMSWebService();
                    ws.Url = url;
                }
                DataSet ds = new DataSet();
                string query = "";

                SAPOperation sop = new SAPOperation();
                sop.BaseAddress = SAPURI;
                sop.SAPKey = SAPKEY;
                sop.SAPDB = SAPDB;
                Console.WriteLine(SAPURI);
                Console.WriteLine(SAPKEY);
                Console.WriteLine(SAPDB);

                if (executeFX)
                {
                    // Delete Exchange Rate              
                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Exchange Rate data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteExchangeRate");

                    string LastMonthLastDay = DateTime.Today.AddDays(0 - DateTime.Today.Day).ToString("yyyy-MM-dd");
                    Console.WriteLine("Last Month Last Day : " + LastMonthLastDay);
                    //Retrieve Exchange Rate
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Exchange Rate data...");
                    query = "CALL \"AF_API_GET_SAP_FOREX\" ('', '','" + LastMonthLastDay + "')";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                    "CurrencyCode", "BuyRate", "SellRate", "MonthEndRate", "Field5", "Field6", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    //Insert Exchange Rate data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Exchange Rate data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_ForeignExchange(
                           dr["CurrencyCode"].ToString(),
                           GMSUtil.ToDecimal(dr["BuyRate"].ToString()),
                           GMSUtil.ToDecimal(dr["SellRate"].ToString()),
                           GMSUtil.ToDecimal(dr["MonthEndRate"].ToString())
                         );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Exchange Rate data insertion");
                    Console.WriteLine(DateTime.Now.ToString() + " -- Update Exchange Rate.");
                    oDAL.GMS_Update_ForeignExchange();
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Update Exchange Rate.");
                    ds.Dispose();

                    //get country shortname 
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Country data...");
                    query = "SELECT * FROM \"@AF_COUNTRY\"";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                        "ID", "Name", "field3", "field4", "field5", "field6", "field7", "field8", "field9", "field10", "field11", "field12", "Field13", "Field14", "ShortName", "Field16", "Field17", "Field18", "Field19", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    //Insert Country data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Country data into GMS...");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["ShortName"].ToString() != "")
                        {
                            oDAL.GMS_Insert_Country(dr["Name"].ToString(), dr["ShortName"].ToString());
                        }
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Country data insertion");
                    ds.Dispose();
                }

                if (execute)
                {
                    //Retrieve Sales Person
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Sales Person data...");
                    query = "SELECT * FROM OSLP";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                        "field1", "SalespersonName", "SalespersonID", "field4", "field5", "field6", "field7", "field8", "field9", "Active", "field11", "field12", "Field13", "Field14", "Field15", "Division", "Field17", "ShortName", "Team", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    //Insert Sales Person data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Sales Person data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["field15"].ToString() == "S")
                            oDAL.GMS_Insert_SalesPerson(CoyID, dr["SalespersonID"].ToString(), dr["SalespersonName"].ToString(), dr["Division"].ToString(), dr["ShortName"].ToString(), dr["Team"].ToString(), dr["Active"].ToString());
                        else if (dr["field15"].ToString() == "B")
                            oDAL.GMS_Insert_Purchaser(CoyID, dr["SalespersonID"].ToString(), dr["SalespersonName"].ToString(), "");
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Sales Person & Purchaser data insertion");
                    ds.Dispose();
                }

                if (execute)
                {

                    //Retrieve Product data
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Product data...");
                    //ds = ws.GetProduct();

                    query = "CALL \"AF_API_GET_SAP_ITEMMASTERINFO\" ('','','','','','')";

                    ds = sop.GET_SAP_QueryData(CoyID, query,
                    "ProductCode", "ProductName", "ProductGroupCode", "Volume", "UOM", "WeightedCost", "OnSOQty", "OnPOQty", "OnBOQty", "OnHandQty", "IsGasDivision", "IsWeldingDivision", "ProdForeignName", "TrackedByBatch", "TrackedBySerial", "ProductNotes", "IsActive", "ItemType", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    bool IsGasDivision = false;
                    bool IsWeldingDivision = false;
                    bool TrackedByBatch = false;
                    bool TrackedBySerial = false;
                    string ProductNotes = "";
                    int VersionNo = 0;

                    tempProductName = "";
                    string[] stringSeparators = new string[] { "\r\n", "\r", "\n" };
                    //Insert Product data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Product data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tempProductName = dr["ProductName"].ToString().Replace("'", "''");

                        if (dr["IsGasDivision"].ToString() == "1")
                            IsGasDivision = true;
                        else
                            IsGasDivision = false;

                        if (dr["IsWeldingDivision"].ToString() == "1")
                            IsWeldingDivision = true;
                        else
                            IsWeldingDivision = false;

                        if (dr["TrackedByBatch"].ToString() == "1")
                            TrackedByBatch = true;
                        else
                            TrackedByBatch = false;

                        if (dr["TrackedBySerial"].ToString() == "1")
                            TrackedBySerial = true;
                        else
                            TrackedBySerial = false;

                        if (dr["IsActive"].ToString() == "true")
                            VersionNo = 0;
                        else
                            VersionNo = 1;

                        ProductNotes = dr["ProductNotes"].ToString();

                        if (dr["ItemType"].ToString() != "F")
                        {
                            oDAL.GMS_Insert_Product2(CoyID,
                                dr["ProductCode"].ToString(),
                                tempProductName,
                                dr["ProductGroupCode"].ToString(),
                                GMSUtil.ToDouble(dr["Volume"].ToString()),
                                dr["UOM"].ToString(),
                                GMSUtil.ToDouble(dr["WeightedCost"].ToString()),
                                GMSUtil.ToDouble(dr["OnSOQty"].ToString()),
                                GMSUtil.ToDouble(dr["OnPOQty"].ToString()),
                                GMSUtil.ToDouble(dr["OnBOQty"].ToString()),
                                GMSUtil.ToDouble(dr["OnHandQty"].ToString()),
                                IsGasDivision,
                                IsWeldingDivision,
                                dr["ProdForeignName"].ToString(),
                                TrackedByBatch,
                                TrackedBySerial,
                                VersionNo
                                );

                            if (ProductNotes != "")
                            {
                                string[] arrProductNotes = ProductNotes.Split(stringSeparators, StringSplitOptions.None);

                                if (arrProductNotes.Length > 0)
                                {
                                    oDAL.GMS_Delete_ProductNotes(CoyID,
                                        dr["ProductCode"].ToString()
                                    );

                                    for (int i = 0; i < arrProductNotes.Length; i++)
                                    {
                                        oDAL.GMS_Insert_ProductNotes(CoyID,
                                        dr["ProductCode"].ToString(),
                                        i + 1,
                                        arrProductNotes[i].ToString()
                                        );
                                    }
                                }
                            }
                        }

                    }

                    Console.WriteLine(DateTime.Now.ToString() + " -- End Product data insertion for " + CoyID.ToString());
                    ds.Dispose();

                    // Delete Product 
                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Duplicate Product data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteDuplicateProduct");
                }

                if (execute)
                {

                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Product UOM data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteProductUOM");

                    //ProductUOM
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Product UOM data...");
                    //ds = ws.GetProductUOM("");
                    query = "SELECT * FROM \"@AF_UOM1\"";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                        "field1", "field2", "field3", "field4", "field5", "field6", "field7", "field8", "field9", "field10", "field11", "field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                    //Insert Product UOM data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Product UOM data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_ProductUOM(CoyID,
                            dr["field1"].ToString(),
                            dr["field6"].ToString(),
                            GMSUtil.ToFloat(dr["field7"].ToString()) / GMSUtil.ToFloat(dr["field5"].ToString()),
                            GMSUtil.ToFloat(dr["field5"].ToString()),
                            dr["field6"].ToString(),
                            GMSUtil.ToFloat(dr["field7"].ToString()),
                            dr["field8"].ToString()
                            );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Product UOM data insertion for " + CoyID.ToString());
                    ds.Dispose();
                }

                if (execute)
                {
                    //Retrieve Product Group data
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Product Group data...");
                    //ds = ws.GetProductGroup();
                    //query = "CALL \"AF_API_GET_SAP_PRODGROUP\" ()";
                    query = "SELECT * FROM \"@AF_PRODGRP\"";

                    ds = sop.GET_SAP_QueryData(CoyID, query,
                    "ProductGroupCode", "ProductGroupName", "Field3", "Field4", "Field5", "Field6", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "ABCGrouping", "Team", "ProductCategory", "Field17", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    //Insert Product Group data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Product Group data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_ProductGroup(CoyID,
                            dr["ProductGroupCode"].ToString(),
                            dr["ProductGroupName"].ToString().Replace("'", "''"),
                            dr["ProductCategory"].ToString(),
                            dr["Team"].ToString(),
                            dr["ABCGrouping"].ToString()
                            );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Product Group data insertion for " + CoyID.ToString());
                    ds.Dispose();
                }

                if (execute)
                {
                    // Delete Warehouse
                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Warehouse data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteWarehouse");

                    //Retrieve Warehouse
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Warehouse data...");
                    query = "SELECT * FROM OWHS";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                        "field1", "field2", "field3", "field4", "field5", "field6", "field7", "field8", "field9", "field10", "field11", "field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    //ds = ws.GetWarehouse();
                    //Insert Warehouse data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Warehouse data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_Warehouse(CoyID,
                        dr["field1"].ToString(),
                        dr["field2"].ToString()
                        );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Warehouse data insertion");
                    ds.Dispose();
                }

                if (execute)
                {
                    //Retrieve Account
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Account data...");
                    //ds = ws.GetAccount("''");
                    query = "CALL \"AF_API_GET_SAP_CUSTOMERSUPPLIERINFO\" ()";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                    "AccountCode", "AccountType", "AccountName", "DefaultCurrency", "SalesPersonID", "TaxType", "CreditTerm", "CreditLimit", "Outstanding", "Address1", "Address2", "Address3", "Address4", "PostalCode", "ContactPerson", "OfficePhone", "MobilePhone", "Fax", "Email", "Industry",
                    "Country", "CreatedDate", "AccountGroupName", "AccountClass", "GradeCode", "Field26", "Field27", "Field28", "Field29", "Field30");

                    //Insert Account data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Account data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_Account(CoyID,
                        dr["AccountCode"].ToString(),
                        dr["AccountType"].ToString(),
                        dr["AccountName"].ToString(),
                        dr["SalesPersonID"].ToString(),
                        dr["DefaultCurrency"].ToString(),
                        dr["TaxType"].ToString(),
                        GMSUtil.ToShort(dr["CreditTerm"].ToString()),
                        GMSUtil.ToFloat(dr["CreditLimit"].ToString()),
                        dr["Address1"].ToString(),
                        dr["Address2"].ToString(),
                        dr["Address3"].ToString(),
                        dr["Address4"].ToString(),
                        dr["PostalCode"].ToString(),
                        dr["ContactPerson"].ToString(),
                        dr["OfficePhone"].ToString(),
                        dr["MobilePhone"].ToString(),
                        dr["Fax"].ToString(),
                        dr["Email"].ToString(),
                        dr["Industry"].ToString(),
                        dr["Country"].ToString(),
                        GMSUtil.ToDate(dr["CreatedDate"].ToString()),
                        dr["AccountGroupName"].ToString(),
                        dr["AccountClass"].ToString(),
                        dr["GradeCode"].ToString(),
                        GMSUtil.ToFloat(dr["Outstanding"])
                        );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Account data insertion");
                    ds.Dispose();

                }

                // Delete Duplicate Account
                Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Duplicate Account data in GMS...");
                oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteDuplicateAccount");

                if (implementedJobTraveller)
                {
                    // Start
                    //Delete JobTraveller
                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete JobTraveller data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteJobTraveller");
                    string from1 = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1).ToString("yyyy-MM-dd");
                    string to1 = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddDays(-1).ToString("yyyy-MM-dd");
                    string from2 = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
                    string to2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("yyyy-MM-dd");

                    Task_ImportDataJobTraveller(CoyID, from1, to1, sop);
                    Task_ImportDataJobTraveller(CoyID, from2, to2, sop);
                }

                if (execute)
                {
                    // Delete GRN
                    //Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete GRN data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteGRN");

                    //Retrieve Last Month GRN
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving GRN data...");
                    //query = "CALL \"AF_API_GET_SAP_GRN_DETAIL\" ('', '', '2019-01-01' , '2019-12-31')";
                    query = "CALL \"AF_API_GET_SAP_GRN_DETAIL\" ('', '', '" + from + "' , '" + to + "')";
                    //query = "CALL \"AF_API_GET_SAP_GRN_DETAIL\" ('', '', '' , '')";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                    "TrnNo", "TrnDate", "DetailNo", "PONo", "PODate", "ProductCode", "ProductDescription", "ProductGroupCode", "ProductGroupName", "UOM", "Quantity", "UnitPrice", "LandedCostUnitPrice", "Cost", "Currency", "WH", "ExchangeRate", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    tempProductName = "";
                    //Insert GRN data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting GRN data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tempProductName = dr["ProductDescription"].ToString().Replace("'", "''");
                        oDAL.GMS_Insert_GRN(CoyID,
                         dr["TrnNo"].ToString(),
                         GMSUtil.ToDate(dr["TrnDate"].ToString()),
                         dr["PONo"].ToString(),
                         GMSUtil.ToDate(dr["PODate"].ToString()),
                         dr["ProductCode"].ToString(),
                         tempProductName,
                         dr["ProductGroupCode"].ToString(),
                         dr["ProductGroupName"].ToString().Replace("'", "''"),
                         GMSUtil.ToDouble(dr["Quantity"].ToString()),
                         GMSUtil.ToDouble(dr["UnitPrice"].ToString()),
                         GMSUtil.ToDouble(dr["LandedCostUnitPrice"].ToString()) == 0 ? GMSUtil.ToDouble(dr["LandedCostUnitPrice"].ToString()) : GMSUtil.ToDouble(GMSUtil.ToDecimal(dr["LandedCostUnitPrice"].ToString()) / GMSUtil.ToDecimal(dr["ExchangeRate"].ToString())),
                         GMSUtil.ToDouble(dr["Cost"].ToString()) == 0 ? GMSUtil.ToDouble(dr["Cost"].ToString()) : GMSUtil.ToDouble(GMSUtil.ToDecimal(dr["Cost"].ToString()) / GMSUtil.ToDecimal(dr["ExchangeRate"].ToString())),
                         dr["Currency"].ToString(),
                         GMSUtil.ToDouble(dr["ExchangeRate"].ToString())
                        );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End GRN data insertion");
                    ds.Dispose();
                }

                // Tax Code
                if (execute)
                {
                    //Retrieve Tax
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Tax Data...");
                    query = "SELECT \"Code\", \"Name\", \"Rate\", \"Category\",  \"Inactive\" FROM OVTG WHERE \"Inactive\" = 'N'";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                        "TaxTypeID", "TaxName", "TaxRate", "Category", "Inactive", "Field6", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    //Insert Tax data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Tax data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_TaxCode(
                            CoyID,
                        dr["TaxTypeID"].ToString(),
                        dr["TaxName"].ToString(),
                        GMSUtil.ToDecimal(dr["TaxRate"].ToString()),
                        dr["Category"].ToString()
                        );

                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Tax data insertion");
                    ds.Dispose();

                }

                // Contact Person

                if (execute)
                {
                    // Delete Contact Person
                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Contact Person data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteContactPerson");

                    //Retrieve Contact Person
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Contact Person Data...");
                    query = "SELECT \"CardCode\", \"Name\", \"Position\", \"Address\",  \"Tel1\", \"Tel2\", \"Cellolar\", \"Fax\", \"E_MailL\", \"FirstName\", \"MiddleName\", \"LastName\",  \"BlockComm\" FROM OCPR WHERE \"Active\" = 'Y'";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                        "AccountCode", "Name", "Position", "Address", "Tel1", "Tel2", "Mobile", "Fax", "Email", "FirstName", "MiddleName", "LastName", "BlockComm", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");


                    //Insert Contact Person data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Contact Person data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        oDAL.GMS_Insert_ContactPerson(
                            CoyID,
                        dr["AccountCode"].ToString(),
                        dr["Name"].ToString(),
                        dr["Position"].ToString(),
                        dr["Address"].ToString(),
                        dr["Tel1"].ToString(),
                        dr["Tel2"].ToString(),
                        dr["Mobile"].ToString(),
                        dr["Fax"].ToString(),
                        dr["Email"].ToString(),
                        dr["FirstName"].ToString(),
                        dr["MiddleName"].ToString(),
                        dr["LastName"].ToString(),
                        dr["BlockComm"].ToString()
                        );

                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Contact Person data insertion");
                    ds.Dispose();
                }


                if (execute)
                {
                    // Delete Stock Movement
                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Product Movement data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteStockMovement");

                    //Retrieve StockMovement
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Stock Movement data...");
                    query = "CALL \"AF_API_GET_SAP_STOCK_MOVEMENT\" ('', '', '', '', '" + from + "', '" + to + "')";
                    //query = "CALL \"AF_API_GET_SAP_STOCK_MOVEMENT\" ('', '', '', '', '', '')";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                    "TrnType", "TrnNo", "TrnDate", "RefNo", "AccountCode", "AccountName", "ProductCode", "ProductName", "ProductGroupCode", "ProductGroupName", "ReceivedQty", "IssuedQty", "BalanceQty", "Cost", "CostWT", "Currency", "ExchangeRate", "Narration", "DocNo", "Warehouse",
                    "TransNum", "UnitPrice", "WarehouseName", "FromWarehouse", "FromWarehouseName", "ToWarehouse", "ToWarehouseName", "DueDate", "DocumentDate", "Field30");
                    tempProductName = "";
                    string tempNarration = "";
                    //Insert StockMovement data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Stock Movement data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tempProductName = dr["ProductName"].ToString().Replace("'", "''");
                        tempNarration = dr["Narration"].ToString().Replace("'", "''");
                        oDAL.GMS_Insert_StockMovement(CoyID,
                         dr["TrnType"].ToString(),
                         GMSUtil.ToInt(dr["TrnNo"].ToString()),
                         GMSUtil.ToDate(dr["TrnDate"].ToString()),
                         dr["RefNo"].ToString().Replace("'", "''"),
                         dr["AccountCode"].ToString(),
                         dr["AccountName"].ToString(),
                         dr["ProductCode"].ToString(),
                         tempProductName,
                         dr["ProductGroupCode"].ToString(),
                         dr["ProductGroupName"].ToString().Replace("'", "''"),
                         GMSUtil.ToDouble(dr["ReceivedQty"].ToString()),
                         GMSUtil.ToDouble(dr["IssuedQty"].ToString()),
                         GMSUtil.ToDouble(dr["BalanceQty"].ToString()),
                         GMSUtil.ToDouble(dr["Cost"].ToString()),
                         GMSUtil.ToDouble(dr["CostWT"].ToString()),
                         dr["Currency"].ToString(),
                         GMSUtil.ToDouble(dr["ExchangeRate"].ToString()),
                        tempNarration
                        );

                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Stock Movement data insertion");
                    ds.Dispose();
                }

                if (execute)
                {
                    //Delete Sales Detail
                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Sales Detail data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteSalesDetail");

                    //Retrieve Sales Detail
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Sales Detail data...");
                    query = "CALL \"AF_API_GET_SAP_SALES_DETAIL\" ('', '', '" + from + "', '" + to + "')";
                    //query = "CALL \"AF_API_GET_SAP_SALES_DETAIL\" ('', '', '', '')";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                    "SalesTrnType", "SalesTrnNo", "SalesTrnDate", "SrNo", "AccountCode", "AccountName", "ProductCode", "ProductName", "ProductGroupCode", "ProductGroupName", "Quantity", "UnitCost", "UnitAmount", "Cost", "Amount", "GPAmount", "Currency", "ExchangeRate", "TaxRate", "LMSDONo",
                    "Warehouse", "CustomerSalesPersonID", "TrnSalesPersonID", "SalesDocDate", "DueDate", "Field26", "Field27", "Field28", "Field29", "Field30");

                    tempProductName = "";
                    //Insert Sales Detail data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Sales Detail data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tempProductName = dr["ProductName"].ToString().Replace("'", "''");

                        oDAL.GMS_Insert_SalesDetail(CoyID,
                        dr["SalesTrnType"].ToString(),
                        dr["SalesTrnNo"].ToString(),
                        GMSUtil.ToDate(dr["SalesTrnDate"].ToString()),
                        GMSUtil.ToShort(dr["SrNo"].ToString()),
                        dr["AccountCode"].ToString(),
                        dr["AccountName"].ToString(),
                        dr["ProductCode"].ToString(),
                        tempProductName,
                        dr["ProductGroupCode"].ToString(),
                        dr["ProductGroupName"].ToString().Replace("'", "''"),
                        GMSUtil.ToDouble(dr["Quantity"].ToString()),
                        GMSUtil.ToDouble(dr["UnitCost"].ToString()),
                        GMSUtil.ToDouble(dr["UnitAmount"].ToString()) * GMSUtil.ToDouble(dr["ExchangeRate"].ToString()),
                        GMSUtil.ToDouble(dr["Cost"].ToString()),
                        GMSUtil.ToDouble(dr["Amount"].ToString()) * GMSUtil.ToDouble(dr["ExchangeRate"].ToString()),
                        GMSUtil.ToDouble(dr["GPAmount"].ToString()),
                        dr["Currency"].ToString(),
                        GMSUtil.ToDouble(dr["ExchangeRate"].ToString()),
                        GMSUtil.ToDouble(dr["TaxRate"].ToString()) / 100,
                        dr["LMSDONo"].ToString().Replace("'", "''"),
                        dr["Warehouse"].ToString(),
                        dr["CustomerSalesPersonID"].ToString(),
                        dr["TrnSalesPersonID"].ToString(),
                        GMSUtil.ToDate(dr["SalesDocDate"].ToString()),
                        GMSUtil.ToDate(dr["DueDate"].ToString())
                        );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Sales Detail data insertion");
                    ds.Dispose();

                    // Update Sales Trn Type CCAmount
                    Console.WriteLine(DateTime.Now.ToString() + " -- Update Sales Trn Type CC Amount in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "UpdateSalesTrnTypeCCAmount");
                }

                if (execute)
                {
                    //Delete Sales
                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Sales data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteSales");

                    //Retrieve Sales I
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Sales I data...");
                    query = "CALL \"AF_API_GET_SAP_SALES_HEADER\" ('', '', '" + from + "', '" + to + "')";
                    //query = "CALL \"AF_API_GET_SAP_SALES_HEADER\" ('', '', '', '')";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                    "TrnType", "TrnNo", "TrnDate", "AccountCode", "AccountName", "LMSDocNo", "PONo", "Amount", "Currency", "ExchangeRate", "TaxAmount", "CustomerSalesPersonID", "TrnSalesPersonID", "DocDate", "DueDate", "Field16", "Field17", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    string lmsDoc = "";
                    //Insert Sales I data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Sales I data into GMS...");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lmsDoc = dr["LMSDocNo"].ToString().Replace("'", "''");
                        oDAL.GMS_Insert_Sales(CoyID,
                        dr["TrnType"].ToString(),
                        dr["TrnNo"].ToString(),
                        GMSUtil.ToDate(dr["TrnDate"].ToString()),
                        dr["AccountCode"].ToString(),
                        dr["AccountName"].ToString(),
                        lmsDoc,
                        dr["PONo"].ToString(),
                        GMSUtil.ToDouble(dr["Amount"].ToString()),
                        dr["Currency"].ToString(),
                        GMSUtil.ToDouble(dr["ExchangeRate"].ToString()),
                        GMSUtil.ToDouble(dr["TaxAmount"].ToString()),
                        dr["CustomerSalesPersonID"].ToString(),
                        dr["TrnSalesPersonID"].ToString(),
                        GMSUtil.ToDate(dr["DocDate"].ToString()),
                        GMSUtil.ToDate(dr["DueDate"].ToString())
                        );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Sales I data insertion.");
                    ds.Dispose();

                }

                if (execute)
                {
                    string lmsDoc = "";
                    // Delete Receipt
                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Receipt data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeleteReceipt");

                    //Retrieve Receipt I
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Receipt I data...");
                    query = "CALL \"AF_API_GET_SAP_RECEIPT\" ('', '', '" + from + "', '" + to + "')";
                    //query = "CALL \"AF_API_GET_SAP_RECEIPT\" ('', '', '', '')";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                    "TrnType", "TrnNo", "TrnDate", "AccountCode", "AccountName", "SalesTrnType", "SalesTrnNo", "LMSDocNo", "AllocateDocNo", "Amount", "Currency", "ExchangeRate", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    //Insert Receipt I data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Receipt I data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lmsDoc = dr["LMSDocNo"].ToString().Replace("'", "''");

                        oDAL.GMS_Insert_Receipt(CoyID,
                         dr["TrnType"].ToString(),
                         dr["TrnNo"].ToString(),
                         GMSUtil.ToDate(dr["TrnDate"].ToString()),
                         dr["AccountCode"].ToString(),
                         dr["AccountName"].ToString(),
                         dr["SalesTrnType"].ToString(),
                         dr["SalesTrnNo"].ToString(),
                         lmsDoc,
                         dr["AllocateDocNo"].ToString().Replace("'", "''"),
                         GMSUtil.ToDouble(dr["Amount"].ToString()),
                         dr["Currency"].ToString(),
                         GMSUtil.ToDouble(dr["ExchangeRate"].ToString())
                     );
                    }

                    Console.WriteLine(DateTime.Now.ToString() + " -- End Receipt I data insertion.");
                    ds.Dispose();


                    // Retrieve Sales II - Unreconciled based on trn no
                    // Parameter : doc no from, doc no to, date from, date to

                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Sales II data...");
                    query = "CALL \"AF_API_GET_SAP_OVERPAYMENT\" ('', '', '" + from + "', '" + to + "')";
                    //query = "CALL \"AF_API_GET_SAP_OVERPAYMENT\" ('', '', '', '')";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                    "TrnType", "TrnNo", "TrnDate", "AccountCode", "AccountName", "LMSDocNo", "PONo", "Amount", "Currency", "ExchangeRate", "TaxRate", "CustomerSalesPersonID", "Field13", "ReconNum", "ReconDate", "ReconAmount", "TotalPaymentAmount", "DocDate", "DueDate", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    //Insert Sales II data into GMS (Overpayment)
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Sales II (Overpayment) data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        double TotalPaymentAmount = 0;
                        if (dr["TrnType"].ToString() == "JE")
                            TotalPaymentAmount = GMSUtil.ToDouble(dr["TotalPaymentAmount"].ToString());
                        else
                            TotalPaymentAmount = GMSUtil.ToDouble(dr["TotalPaymentAmount"].ToString()) * -1;

                        oDAL.GMS_Insert_Sales(CoyID,
                            dr["TrnType"].ToString(),
                            dr["TrnNo"].ToString(),
                            GMSUtil.ToDate(dr["TrnDate"].ToString()),
                            dr["AccountCode"].ToString(),
                            dr["AccountName"].ToString(),
                            "",
                            dr["PONo"].ToString(),
                           TotalPaymentAmount,
                            dr["Currency"].ToString(),
                            GMSUtil.ToDouble(dr["ExchangeRate"].ToString()),
                            GMSUtil.ToDouble(dr["TaxRate"].ToString()) / 100,
                            dr["CustomerSalesPersonID"].ToString(),
                            "",
                            GMSUtil.ToDate(dr["DocDate"].ToString()), GMSUtil.ToDate(dr["DueDate"].ToString())
                        );


                        if (dr["ReconNum"].ToString() != "")
                        {
                            double TotalAppliedAmount = 0;
                            if (dr["TrnType"].ToString() == "JE")
                                TotalAppliedAmount = GMSUtil.ToDouble(dr["ReconAmount"].ToString()) * -1;
                            else
                                TotalAppliedAmount = GMSUtil.ToDouble(dr["ReconAmount"].ToString());

                            //Insert Receipt II data into GMS (Overpayment)                      
                            Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Receipt II data into GMS...");
                            oDAL.GMS_Insert_Receipt(CoyID,
                                    dr["TrnType"].ToString(),
                                    dr["TrnNo"].ToString(),
                                    GMSUtil.ToDate(dr["ReconDate"].ToString()),
                                    dr["AccountCode"].ToString(),
                                    dr["AccountName"].ToString(),
                                    dr["TrnType"].ToString(),
                                    dr["TrnNo"].ToString(),
                                    "",
                                    dr["ReconNum"].ToString(),
                                    TotalAppliedAmount,
                                    dr["Currency"].ToString(),
                                    GMSUtil.ToDouble(dr["ExchangeRate"].ToString())
                            );
                        }
                    }

                    if (execute)
                    {
                        Console.WriteLine(DateTime.Now.ToString() + " -- End Receipt II data insertion.");
                        ds.Dispose();

                        // Retrieve Sales III   : Partially or fully reconciled based on recon date
                        // Parameter            : doc no from, doc no to, date from, date to
                        // Added                : 2018-08-14 Annie

                        Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Sales II data...");
                        query = "CALL \"AF_API_GET_SAP_OVERPAYMENT_RECON\" ('', '', '" + from + "', '" + to + "')";
                        //query = "CALL \"AF_API_GET_SAP_OVERPAYMENT_RECON\" ('', '', '', '')";
                        ds = sop.GET_SAP_QueryData(CoyID, query,
                        "TrnType", "TrnNo", "TrnDate", "AccountCode", "AccountName", "LMSDocNo", "PONo", "Amount", "Currency", "ExchangeRate", "TaxRate", "CustomerSalesPersonID", "Field13", "ReconNum", "ReconDate", "ReconAmount", "TotalPaymentAmount", "DocDate", "DueDate", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                        //Insert Sales III data into GMS (Overpayment)
                        Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Sales III (Overpayment) data into GMS...");
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            double TotalPaymentAmount = 0;
                            if (dr["TrnType"].ToString() == "JE")
                                TotalPaymentAmount = GMSUtil.ToDouble(dr["TotalPaymentAmount"].ToString());
                            else
                                TotalPaymentAmount = GMSUtil.ToDouble(dr["TotalPaymentAmount"].ToString()) * -1;

                            oDAL.GMS_Insert_Sales(CoyID,
                                dr["TrnType"].ToString(),
                                dr["TrnNo"].ToString(),
                                GMSUtil.ToDate(dr["TrnDate"].ToString()),
                                dr["AccountCode"].ToString(),
                                dr["AccountName"].ToString(),
                                "",
                                dr["PONo"].ToString(),
                               TotalPaymentAmount,
                                dr["Currency"].ToString(),
                                GMSUtil.ToDouble(dr["ExchangeRate"].ToString()),
                                GMSUtil.ToDouble(dr["TaxRate"].ToString()) / 100,
                                dr["CustomerSalesPersonID"].ToString(),
                                "",
                                GMSUtil.ToDate(dr["DocDate"].ToString()), GMSUtil.ToDate(dr["DueDate"].ToString())
                            );

                            if (dr["ReconNum"].ToString() != "")
                            {
                                double TotalAppliedAmount = 0;
                                if (dr["TrnType"].ToString() == "JE")
                                    TotalAppliedAmount = GMSUtil.ToDouble(dr["ReconAmount"].ToString()) * -1;
                                else
                                    TotalAppliedAmount = GMSUtil.ToDouble(dr["ReconAmount"].ToString());

                                //Insert Receipt III data into GMS (Overpayment)                      
                                Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Receipt III data into GMS...");
                                oDAL.GMS_Insert_Receipt(CoyID,
                                        dr["TrnType"].ToString(),
                                        dr["TrnNo"].ToString(),
                                        GMSUtil.ToDate(dr["ReconDate"].ToString()),
                                        dr["AccountCode"].ToString(),
                                        dr["AccountName"].ToString(),
                                        dr["TrnType"].ToString(),
                                        dr["TrnNo"].ToString(),
                                        "",
                                        dr["ReconNum"].ToString(),
                                        TotalAppliedAmount,
                                        dr["Currency"].ToString(),
                                        GMSUtil.ToDouble(dr["ExchangeRate"].ToString())
                                );
                            }
                        }
                        Console.WriteLine(DateTime.Now.ToString() + " -- End Sales II data insertion.");
                        ds.Dispose();
                    }
                }

                if (execute)
                {
                    // Update Customer SalesPersonID to Sales Detail, Sales
                    Console.WriteLine(DateTime.Now.ToString() + " -- Update Customer Sales Person in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "UpdateCustomerSalesPerson");

                    // Update ProductGroup Info to Sales Detail
                    Console.WriteLine(DateTime.Now.ToString() + " -- Update Product Group Information in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "UpdateSalesDetailProduct");

                }

                if (execute)
                {
                    // Delete Purchase Detail
                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Purchase Detail data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeletePurchaseDetail");

                    //Retrieve Purchase Detail
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Purchase Detail data...");
                    query = "CALL \"AF_API_GET_SAP_PURCHASEDETAILINFO\" ('', '','" + from + "', '" + to + "')";
                    //query = "CALL \"AF_API_GET_SAP_PURCHASEDETAILINFO\" ('', '','', '')";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                    "TransactionID", "TrnDate", "RefNo", "AccountCode", "AccountName", "ProductCode", "ProductName", "ProductGroupCode", "ProductGroupName", "Quantity", "UnitAmount", "Amount", "Currency", "ExchangeRate", "TaxRate", "Field16", "Field17", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    tempProductName = "";
                    //Insert Purchase Detail data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Purchase Detail data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tempProductName = dr["ProductName"].ToString().Replace("'", "''");

                        oDAL.GMS_Insert_PurchaseDetail(CoyID,
                        DateTime.ParseExact(dr["TrnDate"].ToString(), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        dr["RefNo"].ToString().Replace("'", "''"),
                        dr["AccountCode"].ToString(),
                        dr["AccountName"].ToString(),
                        dr["ProductCode"].ToString(),
                        tempProductName,
                        dr["ProductGroupCode"].ToString(),
                        dr["ProductGroupName"].ToString().Replace("'", "''"),
                        GMSUtil.ToDouble(dr["Quantity"].ToString()),
                        GMSUtil.ToDouble(dr["UnitAmount"].ToString()),
                        GMSUtil.ToDouble(dr["Amount"].ToString()),
                        dr["Currency"].ToString(),
                        GMSUtil.ToDouble(dr["ExchangeRate"].ToString()),
                        GMSUtil.ToDouble(dr["TaxRate"].ToString()) / 100,
                        dr["TransactionID"].ToString()
                        );
                    }

                    Console.WriteLine(DateTime.Now.ToString() + " -- End Purchase Detail data insertion.");
                    ds.Dispose();
                }

                if (execute)
                {
                    // Delete Purchase Order
                    Console.WriteLine(DateTime.Now.ToString() + " -- Clean up Delete Purchase Order data in GMS...");
                    oDAL.GMS_ImportUpdateDataByAction(CoyID, "DeletePurchaseOrder");

                    //Retrieve Purchase Order
                    Console.WriteLine(DateTime.Now.ToString() + " -- Retrieving Purchase Detail data...");
                    query = "CALL \"AF_API_GET_SAP_PO_DETAIL\" ('', '', '" + from + "', '" + to + "')";
                    //query = "CALL \"AF_API_GET_SAP_PO_DETAIL\" ('', '', '', '')";
                    ds = sop.GET_SAP_QueryData(CoyID, query,
                    "TrnNo", "TrnDate", "AccountCode", "AccountName", "ProductCode", "ProductName", "ProductGroupCode", "ProductGroupName", "Quantity", "UOM", "UnitAmount", "Discount", "AmountBeforeDiscount", "AmountAfterDiscount", "Currency", "ExchangeRate", "TaxRate", "DocNo", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    tempProductName = "";
                    //Insert Purchase Order data into GMS
                    Console.WriteLine(DateTime.Now.ToString() + " -- Inserting Purchase Order data into GMS...");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tempProductName = dr["ProductName"].ToString().Replace("'", "''");

                        oDAL.GMS_Insert_PurchaseOrder(CoyID,
                        GMSUtil.ToInt(dr["TrnNo"].ToString()),
                        GMSUtil.ToDate(dr["TrnDate"].ToString()),
                        dr["AccountCode"].ToString(),
                        dr["AccountName"].ToString(),
                        dr["ProductCode"].ToString(),
                        tempProductName,
                        dr["ProductGroupCode"].ToString(),
                        dr["ProductGroupName"].ToString().Replace("'", "''"),
                        GMSUtil.ToDouble(dr["Quantity"].ToString()),
                        dr["UOM"].ToString(),
                        GMSUtil.ToDouble(dr["UnitAmount"].ToString()),
                        GMSUtil.ToDouble(dr["Discount"].ToString()),
                        GMSUtil.ToDouble(dr["AmountBeforeDiscount"].ToString()),
                        GMSUtil.ToDouble(dr["AmountAfterDiscount"].ToString()),
                        dr["Currency"].ToString(),
                        GMSUtil.ToDouble(dr["ExchangeRate"].ToString()),
                        GMSUtil.ToDouble(dr["TaxRate"].ToString()) / 100,
                        dr["DocNo"].ToString().Replace("'", "''")
                        );
                    }
                    Console.WriteLine(DateTime.Now.ToString() + " -- End Purchase Order data insertion.");
                    ds.Dispose();
                }
            }
        }
    }
}
