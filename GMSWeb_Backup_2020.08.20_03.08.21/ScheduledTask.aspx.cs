using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using System.Collections.Generic;



namespace GMSWeb
{
    public partial class ScheduledTask : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.AppendHeader("Refresh", "1800");
            //1. Update Product
            ImportProduct();
            //2. Import GRN Detail From A21
            ImportGRNDetail();
            //3. Update MR Details Zero Product Code From New Product Code
            UpdateMRProductDetail();
            //4. Send Email
            //EmailNotification();
            //5. Close MR
            //CloseMR();
            
                    
        }

        protected void EmailNotification()
        {
            IList<Company> lstCompany = null;
            lstCompany = new SystemDataActivity().RetrieveAllCompanyListWithMR();

            string subject = "[GMS - Goods Received (GRN) for Purchased Products]";
            foreach (Company coy in lstCompany)
            {
                if (coy.MRScheme.ToString() == "Product" && coy.Code == "LDJ")
                {
                    string msgBodyHeader = "Dear All, <br /><br />"; 
	                msgBodyHeader += "GRN has been created for the following list of products that were requested by the respective parties:  <br /><br />"; 			
	                msgBodyHeader += "<table width=100% border=1 cellpadding=4 cellspacing=0 bordercolor=#eeeeee>";
	                msgBodyHeader += "<tr><th>Company</th><th>MR No</th><th>Product Code</th><th>Product Name</th><th>PO No</th><th>PO Date</th><th>GRN No</th><th>GRN Date</th><th>Requestor</th><th>Product Manager</th><th>Product Head</th><th>Customer Name</th></tr>";

                    string msgBody = "";

                    DataSet ds = new DataSet();
                    new GMSGeneralDALC().GetGRNForEmailNotification(coy.CoyID, ref ds);

                    //1. Email Requestor                     
                    DataTable dtTemp = new DataTable();
                    dtTemp = ds.Tables[0].Clone();
                    DataRow[] result = ds.Tables[0].Select("SendRequestor = 0");

                    foreach (DataRow dr in result)
                    {
                        object[] row = dr.ItemArray;
                        dtTemp.Rows.Add(row);
                    }
                   
                    DataTable distinctDT = dtTemp.DefaultView.ToTable(true, new string[] { "requestoremail" });

                    foreach (DataRow distinctRow in distinctDT.Rows)
                    {
                        DataRow[] resultToEmail = ds.Tables[0].Select("SendRequestor = 0 AND requestoremail = '" + distinctRow["requestoremail"].ToString() + "'");
                        string userEmail = "";
                        string userRealName = "";
                        foreach (DataRow row in resultToEmail)
                        {
                            msgBody += "<tr>";
                            msgBody += "<td>" + row["coyName"] + "</td><td>" + row["mrno"] + "</td><td>" + row["prodcode"] + "</td><td>" + row["prodname"] + "</td><td>" + row["pono"] + "</td><td>" + row["podate"] + "</td><td>" + row["grntrnno"] + "</td><td>" + row["grndate"] + "</td><td>" + row["requestor"] + "</td><td>" + row["pmuserid"] + "</td><td>" + row["phuserid"] + "</td><td>" + row["customer"] + "</td>";
                            msgBody += "</tr>";
                            userEmail = row["requestoremail"].ToString();
                            userRealName = row["requestor"].ToString();
                        }
                        msgBody = msgBodyHeader + msgBody + "</table><br /><br />***** This is a computer-generated email. Please do not reply.*****";
                        //SendNotificationEmail(userEmail, userRealName, subject, msgBody);
                        SendNotificationEmail("keith.wong@leedennox.com", "Kim Yoong", subject + userEmail, msgBody);
                        msgBody = "";
                        // Reset Email Flag for Requestor
                        foreach (DataRow row in resultToEmail)
                        {
                            MRGRNDetail mrGRN = new MRActivity().RetriveMRGRNDetail(coy.CoyID, row["prodcode"].ToString(), row["grnno"].ToString().ToString(), row["pono"].ToString());
                            if (mrGRN != null)
                            {
                                mrGRN.SendRequestor = true;
                                mrGRN.Save();
                                mrGRN.Resync();
                            }    
                        }
                    }            
                                        
                    //2. Email Product Manager
                    dtTemp.Clear();
                    distinctDT.Clear();
                                       
                    result = ds.Tables[0].Select("SendPM = 0");

                    foreach (DataRow dr in result)
                    {
                        object[] row = dr.ItemArray;
                        dtTemp.Rows.Add(row);
                    }

                    distinctDT = dtTemp.DefaultView.ToTable(true, new string[] { "pmuseremail" });

                    foreach (DataRow distinctRow in distinctDT.Rows)
                    {
                        DataRow[] resultToEmail = ds.Tables[0].Select("SendPM = 0 AND pmuseremail = '" + distinctRow["pmuseremail"].ToString() + "'");
                        string userEmail = "";
                        string userRealName = "";
                        foreach (DataRow row in resultToEmail)
                        {
                            msgBody += "<tr>";
                            msgBody += "<td>" + row["coyName"] + "</td><td>" + row["mrno"] + "</td><td>" + row["prodcode"] + "</td><td>" + row["prodname"] + "</td><td>" + row["pono"] + "</td><td>" + row["podate"] + "</td><td>" + row["grntrnno"] + "</td><td>" + row["grndate"] + "</td><td>" + row["requestor"] + "</td><td>" + row["pmuserid"] + "</td><td>" + row["phuserid"] + "</td><td>" + row["customer"] + "</td>";
                            msgBody += "</tr>";
                            userEmail = row["pmuseremail"].ToString();
                            userRealName = row["pmuserid"].ToString();
                        }
                        msgBody = msgBodyHeader + msgBody + "</table><br /><br />***** This is a computer-generated email. Please do not reply.*****";
                        //SendNotificationEmail(userEmail, userRealName, subject, msgBody);
                        SendNotificationEmail("keith.wong@leedennox.com", "Kim Yoong", subject + userEmail, msgBody);
                        msgBody = "";

                        // Reset Email Flag for PM
                        foreach (DataRow row in resultToEmail)
                        {
                            MRGRNDetail mrGRN = new MRActivity().RetriveMRGRNDetail(coy.CoyID, row["prodcode"].ToString(), row["grnno"].ToString().ToString(), row["pono"].ToString());
                            if (mrGRN != null)
                            {
                                mrGRN.SendPM = true;
                                mrGRN.Save();
                                mrGRN.Resync();
                            }
                        }

                    }  

                    //3. Email Product Head
                   
                    dtTemp.Clear();
                    distinctDT.Clear();

                    result = ds.Tables[0].Select("SendPH = 0");

                    foreach (DataRow dr in result)
                    {
                        object[] row = dr.ItemArray;
                        dtTemp.Rows.Add(row);
                    }

                    distinctDT = dtTemp.DefaultView.ToTable(true, new string[] { "phuseremail" });

                    foreach (DataRow distinctRow in distinctDT.Rows)
                    {
                        DataRow[] resultToEmail = ds.Tables[0].Select("SendPH = 0 AND phuseremail = '" + distinctRow["phuseremail"].ToString() + "'");
                        string userEmail = "";
                        string userRealName = "";
                        foreach (DataRow row in resultToEmail)
                        {
                            msgBody += "<tr>";
                            msgBody += "<td>" + row["coyName"] + "</td><td>" + row["mrno"] + "</td><td>" + row["prodcode"] + "</td><td>" + row["prodname"] + "</td><td>" + row["pono"] + "</td><td>" + row["podate"] + "</td><td>" + row["grntrnno"] + "</td><td>" + row["grndate"] + "</td><td>" + row["requestor"] + "</td><td>" + row["pmuserid"] + "</td><td>" + row["phuserid"] + "</td><td>" + row["customer"] + "</td>";
                            msgBody += "</tr>";
                            userEmail = row["phuseremail"].ToString();
                            userRealName = row["phuserid"].ToString();
                        }
                        msgBody = msgBodyHeader + msgBody + "</table><br /><br />***** This is a computer-generated email. Please do not reply.*****";
                        //SendNotificationEmail(userEmail, userRealName, subject, msgBody);
                        SendNotificationEmail("keith.wong@leedennox.com", "Kim Yoong", subject + userEmail, msgBody);
                        msgBody = "";

                        // Reset Email Flag for PM
                        foreach (DataRow row in resultToEmail)
                        {
                            MRGRNDetail mrGRN = new MRActivity().RetriveMRGRNDetail(coy.CoyID, row["prodcode"].ToString(), row["grnno"].ToString().ToString(), row["pono"].ToString());
                            if (mrGRN != null)
                            {
                                mrGRN.SendPH = true;
                                mrGRN.Save();
                                mrGRN.Resync();
                            }
                        }

                    }     
                    //4. Email CSO 

                    DataSet dsCSO = new DataSet();
                    string CSOEmail = "";
                    new GMSGeneralDALC().GetCSOEmailByCoyID(coy.CoyID, ref dsCSO);
                    if ((dsCSO != null) && (dsCSO.Tables[0].Rows.Count > 0))
                        CSOEmail = dsCSO.Tables[0].Rows[0]["CSEmail"].ToString();

                    if (CSOEmail != "")
                    {
                        DataRow[] resultToEmail = ds.Tables[0].Select("SendCSO = 0 AND (IntendedUsage LIKE '%Sales%' OR IntendedUsage LIKE '%Maintenance%')");

                        
                        foreach (DataRow row in resultToEmail)
                        {
                            msgBody += "<tr>";
                            msgBody += "<td>" + row["coyName"] + "</td><td>" + row["mrno"] + "</td><td>" + row["prodcode"] + "</td><td>" + row["prodname"] + "</td><td>" + row["pono"] + "</td><td>" + row["podate"] + "</td><td>" + row["grntrnno"] + "</td><td>" + row["grndate"] + "</td><td>" + row["requestor"] + "</td><td>" + row["pmuserid"] + "</td><td>" + row["phuserid"] + "</td><td>" + row["customer"] + "</td>";
                            msgBody += "</tr>";

                        }
                        msgBody = msgBodyHeader + msgBody + "</table><br /><br />***** This is a computer-generated email. Please do not reply.*****";
                        //SendNotificationEmail(CSOEmail, "CSO", subject, msgBody);
                        SendNotificationEmail("keith.wong@leedennox.com", "Kim Yoong", subject + CSOEmail, msgBody);
                        msgBody = "";

                        // Reset Email Flag for PM
                        foreach (DataRow row in resultToEmail)
                        {
                            MRGRNDetail mrGRN = new MRActivity().RetriveMRGRNDetail(coy.CoyID, row["prodcode"].ToString(), row["grnno"].ToString().ToString(), row["pono"].ToString());
                            if (mrGRN != null)
                            {
                                mrGRN.SendCSO = true;
                                mrGRN.Save();
                                mrGRN.Resync();
                            }
                        }
                    }



                    //5. Email Purchasing 

                    DataSet dsPurchasing = new DataSet();
                    string PurchasingEmail = "";
                    new GMSGeneralDALC().GetPurchasingEmailByCoyID(coy.CoyID, ref dsPurchasing);
                    if ((dsPurchasing != null) && (dsPurchasing.Tables[0].Rows.Count > 0))
                        PurchasingEmail = dsPurchasing.Tables[0].Rows[0]["UserEmail"].ToString();
                    if (PurchasingEmail != "")
                    {
                        DataRow[] resultToEmail = ds.Tables[0].Select("SendPurchasing = 0");

                        foreach (DataRow row in resultToEmail)
                        {
                            msgBody += "<tr>";
                            msgBody += "<td>" + row["coyName"] + "</td><td>" + row["mrno"] + "</td><td>" + row["prodcode"] + "</td><td>" + row["prodname"] + "</td><td>" + row["pono"] + "</td><td>" + row["podate"] + "</td><td>" + row["grntrnno"] + "</td><td>" + row["grndate"] + "</td><td>" + row["requestor"] + "</td><td>" + row["pmuserid"] + "</td><td>" + row["phuserid"] + "</td><td>" + row["customer"] + "</td>";
                            msgBody += "</tr>";

                        }
                        msgBody = msgBodyHeader + msgBody + "</table><br /><br />***** This is a computer-generated email. Please do not reply.*****";
                        //SendNotificationEmail(PurchasingEmail, "Purchasing", subject, msgBody);
                        SendNotificationEmail("keith.wong@leedennox.com", "Kim Yoong", subject + PurchasingEmail, msgBody);
                        msgBody = "";

                        // Reset Email Flag for PM
                        foreach (DataRow row in resultToEmail)
                        {
                            MRGRNDetail mrGRN = new MRActivity().RetriveMRGRNDetail(coy.CoyID, row["prodcode"].ToString(), row["grnno"].ToString().ToString(), row["pono"].ToString());
                            if (mrGRN != null)
                            {
                                mrGRN.SendPurchasing = true;
                                mrGRN.Save();
                                mrGRN.Resync();
                            }
                        }
                    }
                   
                }
            }


        }

        private void SendNotificationEmail(string userEmail, string userRealName, string subject, string msgBody)
        {
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            System.Net.Mail.MailAddress userEmailAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
            string smtpServer = "smtp.leedenlimited.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(adminEmailAddress, userEmailAddress);

            mail.ReplyTo = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Admin");
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = msgBody;

            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.Host = smtpServer;
                mailClient.Port = 25;
                mailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential authentication = new System.Net.NetworkCredential("gmsadmin@leedenlimited.com", "admin2008");
                mailClient.Credentials = authentication;
                mailClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void UpdateMRProductDetail()
        {
            IList<Company> lstCompany = null;
            lstCompany = new SystemDataActivity().RetrieveAllCompanyListWithMR();

            foreach (Company coy in lstCompany)
            {
                if (coy.MRScheme.ToString() == "Product" && coy.Code == "LDJ")
                {
                    new GMSGeneralDALC().UpdateMRDetailProdCode(coy.CoyID);
                }
            }


        }


        protected void CloseMR()
        {
            IList<Company> lstCompany = null;
            lstCompany = new SystemDataActivity().RetrieveAllCompanyListWithMR();

            foreach (Company coy in lstCompany)
            {
                if (coy.MRScheme.ToString() == "Product" && coy.Code == "LDJ")
                {
                    IList<MR> lstMR = null;
                    lstMR = new MRActivity().RetrieveMRByStatus(coy.CoyID, "P");
                    foreach (MR mr in lstMR)
                    {
                        IList<MRDelivery> lstMRDelivery = null;
                        lstMRDelivery = new MRActivity().RetrieveMRDeliveryByMRNo(coy.CoyID, mr.MRNo.ToString());

                        bool isComplete = true;
                        IList<MRDetail> lstMRDetail = null;
                        lstMRDetail = new MRActivity().RetrieveMRProductByMRNo(coy.CoyID, mr.MRNo.ToString());
                        foreach (MRDetail mrDetail in lstMRDetail)
                        {
                            double quantity = 0;

                            foreach (MRDelivery mrDelivery in lstMRDelivery)
                            {
                                if (mrDelivery.PONo.ToString() != "")
                                {
                                    IList<MRGRNDetail> lstGRNDetail = new MRActivity().RetriveMRGRNDetailByPOAndProduct(coy.CoyID, mrDetail.ProdCode.ToString(), mrDelivery.PONo.ToString());
                                    foreach (MRGRNDetail grnDetail in lstGRNDetail)
                                    {
                                        quantity += grnDetail.Quantity;
                                    }
                                }
                            }

                            if (mrDetail.OrderQty > quantity)
                            {
                                isComplete = false;
                                break;
                            }

                        }

                        if (isComplete == true)
                        {
                            mr.StatusID = "C";
                            mr.ModifiedBy = 1;
                            mr.ModifiedDate = DateTime.Now;
                            mr.Save();
                            mr.Resync();
                        }
                            
                    }

                }
            }

        }

        protected void ImportProduct()
        {
            IList<Company> lstCompany = null;
            lstCompany = new SystemDataActivity().RetrieveAllCompanyListWithMR();

            foreach (Company coy in lstCompany)
            {
                if (coy.MRScheme.ToString() != "" && coy.MRScheme != null)
                {
                    DataSet ds = new DataSet();                    
                    try
                    {
                        GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                        if (coy.WebServiceAddress != null && coy.WebServiceAddress.Trim() != "")
                        {
                            sc.Url = coy.WebServiceAddress.Trim();
                        }
                        else
                            sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                        ds = sc.GetAllProduct(coy.CoyID);                       

                                               

                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            IList<ScheduledTaskProduct> lstScheduledTaskProduct = new SystemDataActivity().RetrieveAllScheduledTaskProductByCompany(coy.CoyID);

                            foreach (ScheduledTaskProduct stp in lstScheduledTaskProduct)
                            {
                                new SystemDataActivity().DeleteScheduledTaskProduct(stp);
                                stp.Resync();
                            }

                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                ScheduledTaskProduct product = new ScheduledTaskProduct();
                                product.CoyID = GMSUtil.ToByte(coy.CoyID);
                                product.ProductCode = ds.Tables[0].Rows[j]["ProductCode"].ToString();
                                product.ProductName = ds.Tables[0].Rows[j]["ProductName"].ToString();
                                product.ProductGroupCode = ds.Tables[0].Rows[j]["ProductGroupCode"].ToString();
                                product.Volume = GMSUtil.ToDouble(ds.Tables[0].Rows[j]["Volume"].ToString());
                                product.UOM = ds.Tables[0].Rows[j]["UOM"].ToString();
                                product.WeightedCost = GMSUtil.ToDouble(ds.Tables[0].Rows[j]["WeightedCost"].ToString());
                                product.WarehouseHQ = GMSUtil.ToDouble(ds.Tables[0].Rows[j]["WarehouseHQ"].ToString());
                                product.Warehouse77 = GMSUtil.ToDouble(ds.Tables[0].Rows[j]["Warehouse77"].ToString());
                                product.WarehouseOthers = GMSUtil.ToDouble(ds.Tables[0].Rows[j]["WarehouseOthers"].ToString());
                                product.OnSOQty = GMSUtil.ToDouble(ds.Tables[0].Rows[j]["OnSOQty"].ToString());
                                product.OnPOQty = GMSUtil.ToDouble(ds.Tables[0].Rows[j]["OnPOQty"].ToString());
                                product.UpdatedDate = DateTime.Now;
                                product.Save();
                                product.Resync(); 
                            }  
                        }
                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    }
                }
            }
            // Update and Insert New Product
            new GMSGeneralDALC().UpdateProductFromScheduledTaskProduct();

        }

        protected void ImportGRNDetail()
        {
            IList<Company> lstCompany = null;
            lstCompany = new SystemDataActivity().RetrieveAllCompanyListWithMR();

            foreach (Company coy in lstCompany)
            {
                if (coy.MRScheme.ToString() != "" && coy.MRScheme != null)
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                        if (coy.WebServiceAddress != null && coy.WebServiceAddress.Trim() != "")
                        {
                            sc.Url = coy.WebServiceAddress.Trim();
                        }
                        else
                            sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                        ds = sc.GetMRGRNDetailFromA21(coy.CoyID);
                        
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {

                            IList<ScheduledTaskGRNDetail> lstScheduledTaskGRNDetail = new SystemDataActivity().RetrieveAllScheduledTaskGRNDetailByCompany(coy.CoyID);

                            foreach (ScheduledTaskGRNDetail grn in lstScheduledTaskGRNDetail)
                            {
                                new SystemDataActivity().DeleteScheduledTaskGRNDetail(grn);
                                grn.Resync();
                            }

                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                ScheduledTaskGRNDetail mrGRN = new ScheduledTaskGRNDetail();                                
                                mrGRN.CoyID = GMSUtil.ToByte(coy.CoyID);
                                mrGRN.ProductCode = ds.Tables[0].Rows[j]["Product"].ToString();
                                mrGRN.Purchaser = ds.Tables[0].Rows[j]["Purchaser"].ToString();
                                mrGRN.PONo = ds.Tables[0].Rows[j]["PONo"].ToString();
                                mrGRN.PODate = GMSUtil.ToDate(ds.Tables[0].Rows[j]["PODate"].ToString());
                                mrGRN.GRNNo = ds.Tables[0].Rows[j]["GRNNo"].ToString();
                                mrGRN.GRNTrnNo = ds.Tables[0].Rows[j]["trntype"].ToString() + ds.Tables[0].Rows[j]["trnno"].ToString();
                                mrGRN.GRNDate = GMSUtil.ToDate(ds.Tables[0].Rows[j]["GRNDate"].ToString());
                                mrGRN.Quantity = GMSUtil.ToDouble(ds.Tables[0].Rows[j]["Quantity"].ToString());
                                mrGRN.UpdatedDate = DateTime.Now;
                                mrGRN.Save();
                                mrGRN.Resync();
                            }  

                           /* 
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                MRGRNDetail mrGRN = null;

                                if (ds.Tables[0].Rows[j]["Product"].ToString() != "" && ds.Tables[0].Rows[j]["GRNNo"].ToString() != "" && ds.Tables[0].Rows[j]["PONo"].ToString() != "")
                                {
                                    mrGRN = new MRActivity().RetriveMRGRNDetail(coy.CoyID, ds.Tables[0].Rows[j]["Product"].ToString(), ds.Tables[0].Rows[j]["GRNNo"].ToString(), ds.Tables[0].Rows[j]["PONo"].ToString());
                                    //mrGRN.Delete();
                                    //mrGRN.Resync();
                                    
                                    if (mrGRN != null)
                                    {

                                        if (mrGRN.Purchaser != ds.Tables[0].Rows[j]["Purchaser"].ToString() ||
                                            mrGRN.Quantity != GMSUtil.ToDouble(ds.Tables[0].Rows[j]["Quantity"].ToString()) ||
                                            mrGRN.PODate != GMSUtil.ToDate(ds.Tables[0].Rows[j]["PODate"].ToString()) ||
                                            mrGRN.GRNDate != GMSUtil.ToDate(ds.Tables[0].Rows[j]["GRNDate"].ToString()))
                                        {
                                            mrGRN.Purchaser = ds.Tables[0].Rows[j]["Purchaser"].ToString();
                                            mrGRN.Quantity = GMSUtil.ToDouble(ds.Tables[0].Rows[j]["Quantity"].ToString());
                                            mrGRN.PODate = GMSUtil.ToDate(ds.Tables[0].Rows[j]["PODate"].ToString());
                                            mrGRN.GRNDate = GMSUtil.ToDate(ds.Tables[0].Rows[j]["GRNDate"].ToString());
                                            mrGRN.ModifiedDate = DateTime.Now;
                                            mrGRN.Save();
                                            mrGRN.Resync();
                                        }
                                    }
                                    else
                                    {
                                        mrGRN = new MRGRNDetail();
                                        mrGRN.CoyID = GMSUtil.ToByte(coy.CoyID);
                                        mrGRN.ProductCode = ds.Tables[0].Rows[j]["Product"].ToString();
                                        mrGRN.Purchaser = ds.Tables[0].Rows[j]["Purchaser"].ToString();
                                        mrGRN.PONo = ds.Tables[0].Rows[j]["PONo"].ToString();
                                        mrGRN.PODate = GMSUtil.ToDate(ds.Tables[0].Rows[j]["PODate"].ToString());
                                        mrGRN.GRNNo = ds.Tables[0].Rows[j]["GRNNo"].ToString();
                                        mrGRN.GRNTrnNo = ds.Tables[0].Rows[j]["trntype"].ToString() + ds.Tables[0].Rows[j]["trnno"].ToString();
                                        mrGRN.GRNDate = GMSUtil.ToDate(ds.Tables[0].Rows[j]["GRNDate"].ToString());
                                        mrGRN.Quantity = GMSUtil.ToDouble(ds.Tables[0].Rows[j]["Quantity"].ToString());
                                        mrGRN.ModifiedDate = DateTime.Now;
                                        mrGRN.Save();
                                        mrGRN.Resync();
                                    }
                                    

                                }
                               
                            }
                            */
                        }


                    }
                    catch (Exception ex)
                    {
                        JScriptAlertMsg(ex.Message);
                    } 
                }
            }

            // Update and Insert GRN Detail
            new GMSGeneralDALC().UpdateGRNDetailFromScheduledTaskGRNDetail();
            
        }
    }
}
