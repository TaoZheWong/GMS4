using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Organization.Upload
{
    public partial class ParsingAudit : GMSBasePage
    {
        private string excelFilePath = "", excelFileName="";
        private short year = 0;
        private short itemPurposeId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.excelFileName = Request.Params["FILENAME"];
            this.year = GMSUtil.ToShort(Request.Params["YEAR"]);
            itemPurposeId = GMSUtil.ToShort(Request.Params["PURPOSEID"]);

            excelFilePath = AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + Path.DirectorySeparatorChar + excelFileName;

            Response.ContentType = "text/html";
            ParseExcelFile();
            //UpdateSummary();
            
        }

        protected void UpdateSummary()
        {
            LogSession sess = base.GetSessionInfo();
            IList<FinanceItem> lstItem = null;
            IList<FinanceAuditData> lstAuditItem  = null;
            FinanceItem item = null;

            double GrossProfits = 0;
            double TotalSales = 0;
            double ExternalSales = 0;
            double CostofExternalSales = 0;
            double IntercoSales = 0;
            double CostofIntercoSales = 0;
            double PLTotalSDExp = 0;
            double PLTotalAGExp = 0;
            double ProfitsfromOperations = 0;
            double ProfitBeforeTaxation = 0;
            double ProfitAfterTaxation = 0;
            double TotalEquity = 0;
            double TradeDebtors = 0;
            double Stocks = 0;
            double TotalLiabilities = 0;
            double ODRevolvingCredit = 0;
            double TrustReceiptsNotesPayableFactoring = 0;
            double ShortTermLoans = 0;
            double TermLoansCurrentPortion = 0;
            double HirePurchaseCreditorsCurrent = 0;
            double LongTermLoans = 0;
            double HirePurchaseCreditors = 0;
            double ProvisionForStockObsolescence = 0;
            double TradeCreditors = 0;
            double TotalCostOfSales = 0;
            

            //GrossProfits       
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Gross Profits");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            GrossProfits = (Double)((FinanceAuditData)lstAuditItem[0]).Total;  
                          
                        }

                    }
                }
            }

            //Total Sales       
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Total Sales");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            TotalSales = (Double)((FinanceAuditData)lstAuditItem[0]).Total;                            
                        }

                    }
                }
            }
            

            //GrossProfitsMargin        
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Gross Profits Margin %");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        Response.Output.Write("Updating Audit detail for Item Name: 'Gross Profits Margin' ...<br>");
                        Response.Flush();

                        if (lstAuditItem.Count > 0)
                        {                                                       
                            FinanceAuditData GrossProfitsMargin = (FinanceAuditData)lstAuditItem[0];
                            GrossProfitsMargin.Total = (GrossProfits / TotalSales) * 100;
                            GrossProfitsMargin.Save();

                            Response.Output.Write("Updating successful.<br>");
                            Response.Flush();
                        }

                    }
                }
            }

            //External Sales     
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("External Sales");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            ExternalSales = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Cost of External Sales      
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Cost of External Sales");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            CostofExternalSales = (Double)((FinanceAuditData)lstAuditItem[0]).Total;
                            CostofExternalSales = CostofExternalSales * -1;
                        }

                    }
                }
            }

            //- External Sales GP%

            Response.Output.Write("<br>");

            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("- External Sales GP%");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        Response.Output.Write("Updating Audit detail for Item Name: 'External Sales GP%' ...<br>");
                        Response.Flush();

                        if (lstAuditItem.Count > 0)
                        {                                                       
                            FinanceAuditData ExternalSalesGP = (FinanceAuditData)lstAuditItem[0];
                            ExternalSalesGP.Total = ((ExternalSales - CostofExternalSales) / ExternalSales) * 100;
                            ExternalSalesGP.Save();

                            Response.Output.Write("Updating successful.<br>");
                            Response.Flush();
                        }

                    }
                }
            }

            //Interco Sales    
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Interco Sales");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            IntercoSales = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Cost of Interco Sales   
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Cost of Interco Sales");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            CostofIntercoSales = (Double)((FinanceAuditData)lstAuditItem[0]).Total;
                            CostofIntercoSales = CostofIntercoSales * -1;
                        }

                    }
                }
            }

            //- Interco Sales GP%

            Response.Output.Write("<br>");

            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("- Interco Sales GP%");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        Response.Output.Write("Updating Audit detail for Item Name: '- Interco Sales GP%' ...<br>");
                        Response.Flush();

                        if (lstAuditItem.Count > 0)
                        {
                            FinanceAuditData IntercoSalesGP = (FinanceAuditData)lstAuditItem[0];
                            IntercoSalesGP.Total = ((IntercoSales - CostofIntercoSales) / IntercoSales) * 100;
                            IntercoSalesGP.Save();

                            Response.Output.Write("Updating successful.<br>");
                            Response.Flush();
                        }

                    }
                }
            }

            //P&L Total S&D Exp 
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("P&L Total S&D Exp");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            PLTotalSDExp = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }


            //Selling & Dist Margin %

            Response.Output.Write("<br>");

            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Selling & Dist Margin %");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        Response.Output.Write("Updating Audit detail for Item Name: 'Selling & Dist Margin %' ...<br>");
                        Response.Flush();

                        if (lstAuditItem.Count > 0)
                        {
                            FinanceAuditData SellingDistMargin = (FinanceAuditData)lstAuditItem[0];
                            SellingDistMargin.Total = (PLTotalSDExp / TotalSales) * -100;
                            SellingDistMargin.Save();

                            Response.Output.Write("Updating successful.<br>");
                            Response.Flush();
                        }

                    }
                }
            }

            //P&L Total A&G Exp
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("P&L Total A&G Exp");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            PLTotalAGExp = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Admin & General Margin %

            Response.Output.Write("<br>");

            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Admin & General Margin %");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        Response.Output.Write("Updating Audit detail for Item Name: 'Admin & General Margin %' ...<br>");
                        Response.Flush();

                        if (lstAuditItem.Count > 0)
                        {
                            FinanceAuditData AdminGeneralMargin = (FinanceAuditData)lstAuditItem[0];
                            AdminGeneralMargin.Total = (PLTotalAGExp / TotalSales) * -100;
                            AdminGeneralMargin.Save();

                            Response.Output.Write("Updating successful.<br>");
                            Response.Flush();
                        }

                    }
                }
            }

            //Profits from Operations
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Profits from Operations");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            ProfitsfromOperations = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Operating Profit Margin %

            Response.Output.Write("<br>");

            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Operating Profit Margin %");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        Response.Output.Write("Updating Audit detail for Item Name: 'Operating Profit Margin %' ...<br>");
                        Response.Flush();

                        if (lstAuditItem.Count > 0)
                        {
                            FinanceAuditData OperatingProfitMargin = (FinanceAuditData)lstAuditItem[0];
                            OperatingProfitMargin.Total = (ProfitsfromOperations / TotalSales) * 100;
                            OperatingProfitMargin.Save();

                            Response.Output.Write("Updating successful.<br>");
                            Response.Flush();
                        }

                    }
                }
            }

            //Profit Before Taxation
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Profit Before Taxation");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            ProfitBeforeTaxation = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Profits Before Taxation %

            Response.Output.Write("<br>");

            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Profits Before Taxation %");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        Response.Output.Write("Updating Audit detail for Item Name: 'Profits Before Taxation %' ...<br>");
                        Response.Flush();

                        if (lstAuditItem.Count > 0)
                        {
                            FinanceAuditData ProfitsBeforeTaxationPer = (FinanceAuditData)lstAuditItem[0];
                            ProfitsBeforeTaxationPer.Total = (ProfitBeforeTaxation / TotalSales) * 100;
                            ProfitsBeforeTaxationPer.Save();

                            Response.Output.Write("Updating successful.<br>");
                            Response.Flush();
                        }

                    }
                }
            }

            //Profit After Taxation
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Profit After Taxation");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            ProfitAfterTaxation = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Total Equity
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Total Equity");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            TotalEquity = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Return on Equity %

            Response.Output.Write("<br>");

            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Return on Equity %");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        Response.Output.Write("Updating Audit detail for Item Name: 'Return on Equity %' ...<br>");
                        Response.Flush();

                        if (lstAuditItem.Count > 0)
                        {
                            FinanceAuditData ReturnonEquity = (FinanceAuditData)lstAuditItem[0];
                            ReturnonEquity.Total = (ProfitAfterTaxation / TotalEquity) * 100;
                            ReturnonEquity.Save();

                            Response.Output.Write("Updating successful.<br>");
                            Response.Flush();
                        }

                    }
                }
            }

            //Trade Debtors
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Trade Debtors");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            TradeDebtors = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }


            //Debtors Turnover (Days)

            Response.Output.Write("<br>");

            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Debtors Turnover (Days)");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        Response.Output.Write("Updating Audit detail for Item Name: 'Debtors Turnover (Days)' ...<br>");
                        Response.Flush();

                        if (lstAuditItem.Count > 0)
                        {
                            FinanceAuditData DebtorsTurnoverDays = (FinanceAuditData)lstAuditItem[0];
                            DebtorsTurnoverDays.Total = (365 / (ExternalSales / TradeDebtors));
                            DebtorsTurnoverDays.Save();

                            Response.Output.Write("Updating successful.<br>");
                            Response.Flush();
                        }

                    }
                }
            }

            //Stocks
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Stocks");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            Stocks = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Stocks Turnover (Days)

            Response.Output.Write("<br>");

            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Stocks Turnover (Days)");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        Response.Output.Write("Updating Audit detail for Item Name: 'Stocks Turnover (Days)' ...<br>");
                        Response.Flush();

                        if (lstAuditItem.Count > 0)
                        {
                            FinanceAuditData StocksTurnoverDays = (FinanceAuditData)lstAuditItem[0];
                            StocksTurnoverDays.Total = (365 / (CostofExternalSales / Stocks));
                            StocksTurnoverDays.Save();

                            Response.Output.Write("Updating successful.<br>");
                            Response.Flush();
                        }

                    }
                }
            }

            //Total Liabilities
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Total Liabilities");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            TotalLiabilities = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Liabilities To Equity (Leverage)

            Response.Output.Write("<br>");

            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Liabilities To Equity (Leverage)");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        Response.Output.Write("Updating Audit detail for Item Name: 'Liabilities To Equity (Leverage)' ...<br>");
                        Response.Flush();

                        if (lstAuditItem.Count > 0)
                        {
                            FinanceAuditData LiabilitiesToEquityLeverage = (FinanceAuditData)lstAuditItem[0];
                            LiabilitiesToEquityLeverage.Total = (TotalLiabilities / TotalEquity);
                            LiabilitiesToEquityLeverage.Save();

                            Response.Output.Write("Updating successful.<br>");
                            Response.Flush();
                        }

                    }
                }
            }

            //OD/Revolving Credit
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("OD/Revolving Credit");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            ODRevolvingCredit = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Trust Receipts/Notes Payable/Factoring
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Trust Receipts/Notes Payable/Factoring");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            TrustReceiptsNotesPayableFactoring = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Short Term Loans
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Short Term Loans");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            ShortTermLoans = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Term Loans - Current Portion
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Term Loans - Current Portion");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            TermLoansCurrentPortion = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Hire Purchase Creditors - Current
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Hire Purchase Creditors - Current");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            HirePurchaseCreditorsCurrent = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Long Term Loans
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Long Term Loans");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            LongTermLoans = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Hire Purchase Creditors
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Hire Purchase Creditors");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            HirePurchaseCreditors = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }


            //Debt To Equity

            Response.Output.Write("<br>");

            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Debt To Equity");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        Response.Output.Write("Updating Audit detail for Item Name: 'Debt To Equity' ...<br>");
                        Response.Flush();

                        if (lstAuditItem.Count > 0)
                        {
                            FinanceAuditData DebtToEquity = (FinanceAuditData)lstAuditItem[0];
                            DebtToEquity.Total = (ODRevolvingCredit + TrustReceiptsNotesPayableFactoring + ShortTermLoans + TermLoansCurrentPortion + HirePurchaseCreditorsCurrent + LongTermLoans + HirePurchaseCreditors) / TotalEquity;
                            DebtToEquity.Save();

                            Response.Output.Write("Updating successful.<br>");
                            Response.Flush();
                        }

                    }
                }
            }

            //Provision For Stock Obsolescence
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Provision For Stock Obsolescence");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            ProvisionForStockObsolescence = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Trade Creditors
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Trade Creditors");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            TradeCreditors = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Total Cost Of Sales
            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Total Cost Of Sales");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        if (lstAuditItem.Count > 0)
                        {
                            TotalCostOfSales = (Double)((FinanceAuditData)lstAuditItem[0]).Total;

                        }

                    }
                }
            }

            //Creditors Turnover (Days)

            Response.Output.Write("<br>");

            lstItem = new SystemDataActivity().RetrieveFinanceItemByName("Creditors Turnover (Days)");
            if (lstItem != null)
            {
                if (lstItem.Count > 0)
                {
                    item = (FinanceItem)lstItem[0];
                    lstAuditItem = new AuditActivity().RetrieveAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                    if (lstAuditItem != null)
                    {
                        Response.Output.Write("Updating Audit detail for Item Name: 'Creditors Turnover (Days)' ...<br>");
                        Response.Flush();

                        if (lstAuditItem.Count > 0)
                        {
                            FinanceAuditData CreditorsTurnoverDays = (FinanceAuditData)lstAuditItem[0];
                            CreditorsTurnoverDays.Total = 365/(((TotalCostOfSales*-1) + ProvisionForStockObsolescence + Stocks) / TradeCreditors);
                            CreditorsTurnoverDays.Save();

                            Response.Output.Write("Updating successful.<br>");
                            Response.Flush();
                        }

                    }
                }
            }


            Response.Output.Write("<SPAN STYLE='color: red'>End of insertion.</SPAN><br><br>");

        }


        protected void ParseExcelFile()
        {
            DataSet dsExcel = new DataSet();

            try
            {
                Response.Output.Write("Parsing excel file...<br>");
                Response.Flush();

                Asiasoft.MSExcelFileReader.SheetDataLoader sheetDataLoader_Audit = new Asiasoft.MSExcelFileReader.SheetDataLoader();
                sheetDataLoader_Audit.ExcelFilePath = this.excelFilePath;
                sheetDataLoader_Audit.IsHeaderIncludedInExcelFile = true;
                sheetDataLoader_Audit.SheetName = "Sheet1";
                sheetDataLoader_Audit.LoadExcelData();

                dsExcel = sheetDataLoader_Audit.ExcelData;

                //Modified by OSS on 16 May 2012
                for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++)
                {
                    IList<FinanceItem> lstItem = new SystemDataActivity().RetrieveFinanceItemByName(dsExcel.Tables[0].Rows[i]["Item"].ToString());
                    if (lstItem != null)
                    {
                        if (lstItem.Count > 0)
                        {
                            FinanceItem item = (FinanceItem)lstItem[0];

                            LogSession sess = base.GetSessionInfo();
                           
                            // delete from tbAudit records according to year specified by user, then insert new record
                            ResultType result = new AuditActivity().DeleteAuditByYearItemID(this.year, item.ItemID, sess.CompanyId);
                            if (result == ResultType.Ok)
                            {
                                #region insert for each row
                                FinanceAuditData audit = new FinanceAuditData();
                                audit.CoyID = sess.CompanyId;
                                audit.TbYear = this.year; 
                                audit.ItemID = item.ItemID; 
                                audit.Total = GMSUtil.ToFloat(dsExcel.Tables[0].Rows[i]["Total"].ToString());

                                Response.Output.Write("Inserting Audit detail for Item Name: '" + item.ItemName + "' ...<br>");
                                Response.Flush();

                                if (audit.Total != 0)
                                {
                                    try
                                    {
                                        ResultType create = new AuditActivity().CreateAudit(ref audit, sess);
                                        if (create == ResultType.Ok)
                                        {
                                            Response.Output.Write("Inserting successful.<br>");
                                            Response.Flush();

                                            File.Delete(excelFilePath);
                                        }
                                        else
                                        {
                                            Response.Output.Write("<SPAN STYLE='color: red'>Processing error of type : " + result.ToString() + ".</SPAN><br>");
                                            Response.Flush();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Response.Output.Write("<SPAN STYLE='color: red'>Inserting fail. Error:" + ex.Message + ".</SPAN><br>");
                                        Response.Flush();
                                    }
                                }
                                else
                                {
                                    Response.Output.Write("<SPAN STYLE='color: red'>Data not inserted because value is 0.</SPAN><br>");
                                    Response.Flush();
                                }
                                #endregion
                            }
                            Response.Output.Write("<br>");
                        }
                        else
                        {
                            Response.Output.Write("<SPAN STYLE='color: red'>Item Name '" + dsExcel.Tables[0].Rows[i]["Item"].ToString() + "' cannot be found at row " + (i + 1) + ".</SPAN><br><br>");
                            Response.Flush();
                        }
                    }
                }
                Response.Output.Write("<SPAN STYLE='color: red'>End of insertion.</SPAN><br><br>");
                Response.Flush();
            }
            catch (Exception ex)
            {
                Response.Output.Write(("<SPAN STYLE='color: red'><B>Error:" + ex.Message.ToString() + ".</B></SPAN><br>"));
            }
        }
    }
}
