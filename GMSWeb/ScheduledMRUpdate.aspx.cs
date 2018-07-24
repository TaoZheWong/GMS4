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
    public partial class ScheduledMRUpdate : GMSBasePage
    {
        private short coyid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["CoyID"] != null && Request.Params["CoyID"].ToString() != "")
            {
                coyid = GMSUtil.ToByte(Request.Params["CoyID"].ToString());
                Company coy = new SystemDataActivity().RetrieveCompanyByCoyId(coyid);
                if (coy != null)
                {                   
                    // Send Email to respective Party
                    //EmailNotification(coy);
                    //5. Close MR
                    CloseMR(coy);
                    //Close Browser
                    CloseBrowser();
                }
            }
        }

        protected void CloseBrowser()
        {
            string close = @"<script type='text/javascript'>
                                var windowObject = window.self; windowObject.opener = window.self; windowObject.close();
                                </script>";
            base.Response.Write(close);
        }

        protected void EmailNotification(Company coy)
        {
            string subject = "[GMS - Goods Received (GRN) for Purchased Products]";
            //if (coy.MRScheme.ToString() == "Product")
            //{
                System.Net.Mail.MailAddressCollection TO_addressList = new System.Net.Mail.MailAddressCollection();
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
                    
                    TO_addressList.Clear();
                    if(userEmail != "")
                    {
                        System.Net.Mail.MailAddress mytoAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
                        //System.Net.Mail.MailAddress mytoAddress = new System.Net.Mail.MailAddress("keith.wong@leedennox.com", "KimYoong");
                        TO_addressList.Add(mytoAddress);
                    }
                    
                    if(TO_addressList.Count > 0)
                    {
                        SendNotificationEmail(TO_addressList, subject, msgBody);                         
                        //SendNotificationEmail(TO_addressList, subject + userEmail, msgBody);
                    }                      
                    
                    
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
                    
                    TO_addressList.Clear();
                    if(userEmail != "")
                    {
                        System.Net.Mail.MailAddress mytoAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
                        //System.Net.Mail.MailAddress mytoAddress = new System.Net.Mail.MailAddress("keith.wong@leedennox.com", "KimYoong");
                        TO_addressList.Add(mytoAddress);
                    }
                    
                    if(TO_addressList.Count > 0)
                    {
                        SendNotificationEmail(TO_addressList, subject, msgBody);                         
                        //SendNotificationEmail(TO_addressList, subject + userEmail, msgBody);
                    }     

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
                    string pauserEmail = "";
                    foreach (DataRow row in resultToEmail)
                    {
                        msgBody += "<tr>";
                        msgBody += "<td>" + row["coyName"] + "</td><td>" + row["mrno"] + "</td><td>" + row["prodcode"] + "</td><td>" + row["prodname"] + "</td><td>" + row["pono"] + "</td><td>" + row["podate"] + "</td><td>" + row["grntrnno"] + "</td><td>" + row["grndate"] + "</td><td>" + row["requestor"] + "</td><td>" + row["pmuserid"] + "</td><td>" + row["phuserid"] + "</td><td>" + row["customer"] + "</td>";
                        msgBody += "</tr>";
                        userEmail = row["phuseremail"].ToString();
                        userRealName = row["phuserid"].ToString();
                        pauserEmail = row["pauseremail"].ToString();                        
                    }
                    msgBody = msgBodyHeader + msgBody + "</table><br /><br />***** This is a computer-generated email. Please do not reply.*****";

                    TO_addressList.Clear();
                    if(userEmail != "")
                    {
                        System.Net.Mail.MailAddress mytoAddress = new System.Net.Mail.MailAddress(userEmail, userRealName);
                        //System.Net.Mail.MailAddress mytoAddress = new System.Net.Mail.MailAddress("keith.wong@leedennox.com", "KimYoong");
                        TO_addressList.Add(mytoAddress);
                    }

                    if (pauserEmail != "")
                    {
                        System.Net.Mail.MailAddress mytoAddress = new System.Net.Mail.MailAddress(pauserEmail, pauserEmail);
                        //System.Net.Mail.MailAddress mytoAddress = new System.Net.Mail.MailAddress("keith.wong@leedennox.com", "KimYoong");
                        TO_addressList.Add(mytoAddress);
                    }
                    
                    if(TO_addressList.Count > 0)
                    {
                        SendNotificationEmail(TO_addressList, subject, msgBody);                         
                       //SendNotificationEmail(TO_addressList, subject + userEmail + pauserEmail, msgBody);
                    }
                                       
                    msgBody = "";

                    // Reset Email Flag for PH
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
                bool hasRecord = false;
                new GMSGeneralDALC().GetCSOEmailByCoyID(coy.CoyID, ref dsCSO);
                if ((dsCSO != null) && (dsCSO.Tables[0].Rows.Count > 0))
                    CSOEmail = dsCSO.Tables[0].Rows[0]["CSEmail"].ToString();

                if (CSOEmail != "")
                {
                    DataRow[] resultToEmail = ds.Tables[0].Select("SendCSO = 0 AND (IntendedUsage LIKE '%Sales%' OR IntendedUsage LIKE '%Maintenance%')");


                    foreach (DataRow row in resultToEmail)
                    {
                        hasRecord = true;
                        msgBody += "<tr>";
                        msgBody += "<td>" + row["coyName"] + "</td><td>" + row["mrno"] + "</td><td>" + row["prodcode"] + "</td><td>" + row["prodname"] + "</td><td>" + row["pono"] + "</td><td>" + row["podate"] + "</td><td>" + row["grntrnno"] + "</td><td>" + row["grndate"] + "</td><td>" + row["requestor"] + "</td><td>" + row["pmuserid"] + "</td><td>" + row["phuserid"] + "</td><td>" + row["customer"] + "</td>";
                        msgBody += "</tr>";

                    }
                    msgBody = msgBodyHeader + msgBody + "</table><br /><br />***** This is a computer-generated email. Please do not reply.*****";

                    TO_addressList.Clear();
                    string[] split = CSOEmail.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string s in split)
                    {
                        System.Net.Mail.MailAddress mytoAddress = new System.Net.Mail.MailAddress(s, s);
                        //System.Net.Mail.MailAddress mytoAddress = new System.Net.Mail.MailAddress("keith.wong@leedennox.com", "KimYoong");
                        TO_addressList.Add(mytoAddress);  
                    }

                    if (TO_addressList.Count > 0 && hasRecord)
                    {
                        SendNotificationEmail(TO_addressList, subject, msgBody);                         
                        //SendNotificationEmail(TO_addressList, subject + CSOEmail, msgBody);
                    }                    
                   
                    msgBody = "";

                    // Reset Email Flag for CSO
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
                string PurchasingName = "";
                hasRecord = false;
                new GMSGeneralDALC().GetPurchasingEmailByCoyID(coy.CoyID, ref dsPurchasing);
                if ((dsPurchasing != null) && (dsPurchasing.Tables[0].Rows.Count > 0))
                {
                    PurchasingEmail = dsPurchasing.Tables[0].Rows[0]["UserEmail"].ToString();
                    PurchasingName = dsPurchasing.Tables[0].Rows[0]["UserRealName"].ToString();
                }
                if (PurchasingEmail != "")
                {
                    DataRow[] resultToEmail = ds.Tables[0].Select("SendPurchasing = 0");

                    foreach (DataRow row in resultToEmail)
                    {
                        hasRecord = true;
                        msgBody += "<tr>";
                        msgBody += "<td>" + row["coyName"] + "</td><td>" + row["mrno"] + "</td><td>" + row["prodcode"] + "</td><td>" + row["prodname"] + "</td><td>" + row["pono"] + "</td><td>" + row["podate"] + "</td><td>" + row["grntrnno"] + "</td><td>" + row["grndate"] + "</td><td>" + row["requestor"] + "</td><td>" + row["pmuserid"] + "</td><td>" + row["phuserid"] + "</td><td>" + row["customer"] + "</td>";
                        msgBody += "</tr>";

                    }
                    msgBody = msgBodyHeader + msgBody + "</table><br /><br />***** This is a computer-generated email. Please do not reply.*****";

                    TO_addressList.Clear();
                    if (PurchasingEmail != "")
                    {
                        System.Net.Mail.MailAddress mytoAddress = new System.Net.Mail.MailAddress(PurchasingEmail, PurchasingName);
                        //System.Net.Mail.MailAddress mytoAddress = new System.Net.Mail.MailAddress("keith.wong@leedennox.com", "KimYoong");
                        TO_addressList.Add(mytoAddress);
                    }

                    if (TO_addressList.Count > 0 && hasRecord)
                    {
                        SendNotificationEmail(TO_addressList, subject, msgBody);                         
                        //SendNotificationEmail(TO_addressList, subject + PurchasingEmail, msgBody);
                    } 
                   
                    msgBody = "";

                    // Reset Email Flag for Purchasing
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
           // }

        }

        private void SendNotificationEmail(System.Net.Mail.MailAddressCollection TO_addressList, string subject, string msgBody)
        {
            System.Net.Mail.MailAddress adminEmailAddress = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Administrator");
            string smtpServer = "smtp.leedenlimited.com";
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();  

            mail.From = new System.Net.Mail.MailAddress("gmsadmin@leedenlimited.com", "GMS Admin");
            mail.To.Add(TO_addressList.ToString());
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

        protected void CloseMR(Company coy)
        {
            //if (coy.MRScheme.ToString() == "Product")
            //{
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
         
            //}
           
        }       
        
    }
}
