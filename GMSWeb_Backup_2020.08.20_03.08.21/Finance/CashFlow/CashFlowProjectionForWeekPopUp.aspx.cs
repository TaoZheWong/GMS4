using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Globalization;



using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Finance.CashFlow
{
    public partial class CashFlowProjectionForWeekPopUp : GMSBasePage
    {
        private const string SCRIPT_DOFOCUS =
        @"window.setTimeout('DoFocus()', 1);
        function DoFocus()
        {
            try {
                
                document.getElementById('REQUEST_LASTFOCUS').focus();
                document.getElementById('REQUEST_LASTFOCUS').select();
                
            } catch (ex) {}
        }";



        protected void Page_Load(object sender, EventArgs e)
        {

            string currentLink = "CompanyFinance";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();

            }


            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            117);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                           117);


            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!IsPostBack)
            {
                txtAsOfDate.Text = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(0).AddDays(-1).ToString("dd/MM/yyyy");

                LoadCashFlowProjectionData();


                HookOnFocus(this.Page as Control);
            }

            ScriptManager.RegisterStartupScript(this, typeof(Page), "ScriptDoFocus", SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]), true);


        }

        private void HookOnFocus(Control CurrentControl)
        {
            //checks if control is one of TextBox, DropDownList, ListBox or Button
            if ((CurrentControl is TextBox) ||
                (CurrentControl is DropDownList) ||
                (CurrentControl is ListBox))
                //adds a script which saves active control on receiving focus 
                //in the hidden field __LASTFOCUS.
                (CurrentControl as WebControl).Attributes.Add(
                   "onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
            //checks if the control has children
            if (CurrentControl.HasControls())
                //if yes do them all recursively
                foreach (Control CurrentChildControl in CurrentControl.Controls)
                    HookOnFocus(CurrentChildControl);
        }

        protected void setTableHeader()
        {
            DateTime asOfDate = GMSUtil.ToDate(txtAsOfDate.Text.ToString());

            lblMonth1.Text = (asOfDate.AddMonths(1)).ToString("MMM");
            lblMonth2.Text = (asOfDate.AddMonths(2)).ToString("MMM");
            lblMonth3.Text = (asOfDate.AddMonths(3)).ToString("MMM");
            lblMonth4.Text = (asOfDate.AddMonths(4)).ToString("MMM");

        }

        protected void LoadCashFlowProjectionData()
        {
            setTableHeader();

            LogSession session = base.GetSessionInfo();
            DateTime asOfDate = GMSUtil.ToDate(txtAsOfDate.Text.ToString());
            short asOfYear = (short)asOfDate.Year;
            short asOfMonth = (short)asOfDate.Month;

            //Reset All Value to Zero
            ResetAllToZero();

            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsCashFlowProjections = new DataSet();

            dacl.GetCashFlowProjectionAsOf(session.CompanyId, asOfYear, asOfMonth, ref dsCashFlowProjections);

            if (dsCashFlowProjections.Tables[0].Rows.Count > 0)
            {
                LoadCashFlowProjectionItemData(dsCashFlowProjections, false);
            }
            else
            {
                PopuldateAndResetCashFlowProjectionItemData();
            }

        }

        protected void LoadCashFlowProjectionItemData(DataSet dsCashFlowProjections, bool setMonthOnly)
        {
            LogSession session = base.GetSessionInfo();
            short CashFlowItemID = 0;

            for (int i = 0; i < dsCashFlowProjections.Tables[0].Rows.Count; i++)
            {
                CashFlowItemID = GMSUtil.ToShort(dsCashFlowProjections.Tables[0].Rows[i]["CashFlowItemID"].ToString());

                //1	Cash Inflow from Operating Activities
                //2	Collection from Sales  货款回笼
                if (CashFlowItemID.ToString() == "2")
                    SetCashFlowProjectionItem(2, "txtCFS", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //3	Other Income 其他收入
                if (CashFlowItemID.ToString() == "3")
                    SetCashFlowProjectionItem(3, "txtOI", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //4	Total Cash Inflow 
                if (CashFlowItemID.ToString() == "4")
                    SetCashFlowProjectionItem(4, "lblTCI", session, "Label", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //5	Cash Outflow from Operating Activities
                //6	Payment to Overseas Suppliers 支付海外供应商
                if (CashFlowItemID.ToString() == "6")
                    SetCashFlowProjectionItem(6, "txtPTOS", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //7	Payment to Local Suppliers 支付本地供应商
                if (CashFlowItemID.ToString() == "7")
                    SetCashFlowProjectionItem(7, "txtPTLS", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //8	Personnel Expenses 人事相关费用   
                if (CashFlowItemID.ToString() == "8")
                    SetCashFlowProjectionItem(8, "txtPE", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //9	Carriage/Transportation 货代/交通
                if (CashFlowItemID.ToString() == "9")
                    SetCashFlowProjectionItem(9, "txtCT", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //10	Property/ Equipment Expenses 房产/设备费用    
                if (CashFlowItemID.ToString() == "10")
                    SetCashFlowProjectionItem(10, "txtPEE", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //11	Other Expenses 其他费用 
                if (CashFlowItemID.ToString() == "11")
                    SetCashFlowProjectionItem(11, "txtOP", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //12	Taxes payment (GST) (增值税)
                if (CashFlowItemID.ToString() == "12")
                    SetCashFlowProjectionItem(12, "txtTP", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //13	Taxes payment (Corporate) (企业所得税)
                if (CashFlowItemID.ToString() == "13")
                    SetCashFlowProjectionItem(13, "txtTPC", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //14	Total Operating Expenses
                if (CashFlowItemID.ToString() == "14")
                    SetCashFlowProjectionItem(14, "lblTOE", session, "Label", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //15	Net Operating Cash Flow 
                if (CashFlowItemID.ToString() == "15")
                    SetCashFlowProjectionItem(15, "lblNPCF", session, "Label", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //16	Cash Flow from Investing Activities
                //17	Purchase of Fixed Assets 购买固定资产
                if (CashFlowItemID.ToString() == "17")
                    SetCashFlowProjectionItem(17, "txtPOFA", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //18	Investments/Other Assets 投资/购买其他资产
                if (CashFlowItemID.ToString() == "18")
                    SetCashFlowProjectionItem(18, "txtIO", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //19	Disposal of Fixed Assets 出售固定资产
                if (CashFlowItemID.ToString() == "19")
                    SetCashFlowProjectionItem(19, "txtDOFA", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //20	Disposal of Investments/Others 出售投资/其他资产
                if (CashFlowItemID.ToString() == "20")
                    SetCashFlowProjectionItem(20, "txtDOIO", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //21	Loan To Intercompany 关联公司贷款
                if (CashFlowItemID.ToString() == "21")
                    SetCashFlowProjectionItem(21, "txtLTI", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //22	Interest Received 已收利息
                if (CashFlowItemID.ToString() == "22")
                    SetCashFlowProjectionItem(22, "txtIR", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //23	Dividends Received 已收股息
                if (CashFlowItemID.ToString() == "23")
                    SetCashFlowProjectionItem(23, "txtDR", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //24	Dividends Paid 已付股息
                if (CashFlowItemID.ToString() == "24")
                    SetCashFlowProjectionItem(24, "txtDP", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //25	Net Cash Flow From Investing
                if (CashFlowItemID.ToString() == "25")
                    SetCashFlowProjectionItem(25, "lblNCFFI", session, "Label", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //26	Cash Flow from Financing Activities
                //27	Proceeds of Bank Loans 新银行貸款
                if (CashFlowItemID.ToString() == "27")
                    SetCashFlowProjectionItem(27, "txtPOBL", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //28	Repayment of Bank Loans 偿还银行貸款 
                if (CashFlowItemID.ToString() == "28")
                    SetCashFlowProjectionItem(28, "txtROBL", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //29	Repayment of Trade Financing 偿还贸易融资
                if (CashFlowItemID.ToString() == "29")
                    SetCashFlowProjectionItem(29, "txtROTF", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //30	Payment of Interests 支付利息
                if (CashFlowItemID.ToString() == "30")
                    SetCashFlowProjectionItem(30, "txtPOI", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //31	New Capital/Convertible Loan 新资本/可换股贷款
                if (CashFlowItemID.ToString() == "31")
                    SetCashFlowProjectionItem(31, "txtNCCL", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //32	Loan From Intercompany 关联公司贷款
                if (CashFlowItemID.ToString() == "32")
                    SetCashFlowProjectionItem(32, "txtLFI", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //33	Repayment of Intercompany Loan 偿还关联公司貸款
                if (CashFlowItemID.ToString() == "33")
                    SetCashFlowProjectionItem(33, "txtROIL", session, "TextBox", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //34	Net Cash Flow From Financing
                if (CashFlowItemID.ToString() == "34")
                    SetCashFlowProjectionItem(34, "lblNCFFF", session, "Label", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //35	Net Cash Flow 净现金流
                if (CashFlowItemID.ToString() == "35")
                    SetCashFlowProjectionItem(35, "lblNCF", session, "Label", dsCashFlowProjections.Tables[0], i, setMonthOnly);
                //36	Add: Total Available Fund 可用资金
                //37	Net Surplus (Deficit) 净可用资金(不足额)
            }

        }

        protected void ResetAllToZero()
        {

            for (int i = 1; i < 9; i++)
            {
                //1	Cash Inflow from Operating Activities
                //2	Collection from Sales  货款回笼           
                SetPreCashFlowProjectionItem("txtCFS", "TextBox", 0, i);
                //3	Other Income 其他收入
                SetPreCashFlowProjectionItem("txtOI", "TextBox", 0, i);
                //4	Total Cash Inflow            
                SetPreCashFlowProjectionItem("lblTCI", "Label", 0, i);
                //5	Cash Outflow from Operating Activities
                //6	Payment to Overseas Suppliers 支付海外供应商           
                SetPreCashFlowProjectionItem("txtPTOS", "TextBox", 0, i);
                //7	Payment to Local Suppliers 支付本地供应商            
                SetPreCashFlowProjectionItem("txtPTLS", "TextBox", 0, i);
                //8	Personnel Expenses 人事相关费用              
                SetPreCashFlowProjectionItem("txtPE", "TextBox", 0, i);
                //9	Carriage/Transportation 货代/交通           
                SetPreCashFlowProjectionItem("txtCT", "TextBox", 0, i);
                //10	Property/ Equipment Expenses 房产/设备费用 
                SetPreCashFlowProjectionItem("txtPEE", "TextBox", 0, i);
                //11	Other Expenses 其他费用             
                SetPreCashFlowProjectionItem("txtOP", "TextBox", 0, i);
                //12	Taxes payment (GST) (增值税)            
                SetPreCashFlowProjectionItem("txtTP", "TextBox", 0, i);
                //13	Taxes payment (Corporate) (企业所得税)            
                SetPreCashFlowProjectionItem("txtTPC", "TextBox", 0, i);
                //14	Total Operating Expenses           
                SetPreCashFlowProjectionItem("lblTOE", "Label", 0, i);
                //15	Net Operating Cash Flow             
                SetPreCashFlowProjectionItem("lblNPCF", "Label", 0, i);
                //16	Cash Flow from Investing Activities
                //17	Purchase of Fixed Assets 购买固定资产            
                SetPreCashFlowProjectionItem("txtPOFA", "TextBox", 0, i);
                //18	Investments/Other Assets 投资/购买其他资产            
                SetPreCashFlowProjectionItem("txtIO", "TextBox", 0, i);
                //19	Disposal of Fixed Assets 出售固定资产            
                SetPreCashFlowProjectionItem("txtDOFA", "TextBox", 0, i);
                //20	Disposal of Investments/Others 出售投资/其他资产           
                SetPreCashFlowProjectionItem("txtDOIO", "TextBox", 0, i);
                //21	Loan To Intercompany 关联公司贷款           
                SetPreCashFlowProjectionItem("txtLTI", "TextBox", 0, i);
                //22	Interest Received 已收利息            
                SetPreCashFlowProjectionItem("txtIR", "TextBox", 0, i);
                //23	Dividends Received 已收股息           
                SetPreCashFlowProjectionItem("txtDR", "TextBox", 0, i);
                //24	Dividends Paid 已付股息           
                SetPreCashFlowProjectionItem("txtDP", "TextBox", 0, i);
                //25	Net Cash Flow From Investing            
                SetPreCashFlowProjectionItem("lblNCFFI", "Label", 0, i);
                //26	Cash Flow from Financing Activities
                //27	Proceeds of Bank Loans 新银行貸款           
                SetPreCashFlowProjectionItem("txtPOBL", "TextBox", 0, i);
                //28	Repayment of Bank Loans 偿还银行貸款             
                SetPreCashFlowProjectionItem("txtROBL", "TextBox", 0, i);
                //29	Repayment of Trade Financing 偿还贸易融资           
                SetPreCashFlowProjectionItem("txtROTF", "TextBox", 0, i);
                //30	Payment of Interests 支付利息            
                SetPreCashFlowProjectionItem("txtPOI", "TextBox", 0, i);
                //31	New Capital/Convertible Loan 新资本/可换股贷款           
                SetPreCashFlowProjectionItem("txtNCCL", "TextBox", 0, i);
                //32	Loan From Intercompany 关联公司贷款            
                SetPreCashFlowProjectionItem("txtLFI", "TextBox", 0, i);
                //33	Repayment of Intercompany Loan 偿还关联公司貸款            
                SetPreCashFlowProjectionItem("txtROIL", "TextBox", 0, i);
                //34	Net Cash Flow From Financing            
                SetPreCashFlowProjectionItem("lblNCFFF", "Label", 0, i);
                //35	Net Cash Flow 净现金流            
                SetPreCashFlowProjectionItem("lblNCF", "Label", 0, i);
                //36	Add: Total Available Fund 可用资金
                //37	Net Surplus (Deficit) 净可用资金(不足额)
            }
        }

        protected void PopuldateAndResetCashFlowProjectionItemDataForMonth(short year, short month, int columnId)
        {
            LogSession session = base.GetSessionInfo();
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCostOfSales = 0;
            double SDPersonalRelatedCost = 0;
            double AGPersonalRelatedCost = 0;
            double PersonalExpenses = 0;
            double SDCarriageTransportation = 0;
            double SDRentalPropertyRelated = 0;
            double AGRentalPropertyRelated = 0;
            double SDEquipmentExpenses = 0;
            double AGEquipmentExpenses = 0;
            double PropertyEquipmentExpenses = 0;
            double Advertising = 0;
            double Entertaiment = 0;
            double OverseasTravelling = 0;
            double SDStationeryPrintingTelepone = 0;
            double OtherSDExpAllocation = 0;
            double TravellingEntertaiment = 0;
            double LegalProfFeesInsurance = 0;
            double Transportation = 0;
            double AGStationeryPrintingTelepone = 0;
            double AGOtherExp = 0;
            double AGAllocationRecovery = 0;
            double otherExpenses = 0;

            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsCompany = new DataSet();

            dacl.GetCompany(session.CompanyId, ref dsCompany);
            short financeitem = 0;

            DateTime COA2016 = GMSUtil.ToDate(dsCompany.Tables[0].Rows[0]["Is2016COA"].ToString());

        
            bool isCOA2016 = false;

            if (COA2016.Year == year)
            {
                if (COA2016.Month <= month)
                {
                    isCOA2016 = true;
                }
            }
            else if (COA2016.Year < year)
            {
                isCOA2016 = true;
            }

            //Total Sales
            if (isCOA2016)
            {
                financeitem = 1083;
            }
            else
            {
                financeitem = 3;
            }
            FinanceData itemdata = new FinanceDataActivity().RetrieveFinanceDataByItemID(year, month, financeitem, session.CompanyId);

            if (itemdata != null)
                CollectionFromSales = GMSUtil.ToDouble(itemdata.MTD);
            //Set Week Data
            SetPreCashFlowProjectionItem("txtCFS", "TextBox", Math.Round(CollectionFromSales, 0), columnId);

            //retrieve Total Other Operationg Income
            if (isCOA2016)
            {
                financeitem = 858;
            }
            else
            {
                financeitem = 10;
            }
            itemdata = new FinanceDataActivity().RetrieveFinanceDataByItemID(year, month, financeitem, session.CompanyId);

            if (itemdata != null)
                OtherIncome = GMSUtil.ToDouble(itemdata.MTD);
            SetPreCashFlowProjectionItem("txtOI", "TextBox", Math.Round(OtherIncome, 0), columnId);

            //retrieve Cost of Sales
            if (isCOA2016)
            {
                financeitem = 1086;
            }
            else
            {
                financeitem = 6;
            }
            itemdata = new FinanceDataActivity().RetrieveFinanceDataByItemID(year, month, financeitem, session.CompanyId);
            if (itemdata != null)
                TotalCostOfSales = GMSUtil.ToDouble(itemdata.MTD);
            SetPreCashFlowProjectionItem("txtPTLS", "TextBox", Math.Round(TotalCostOfSales, 0), columnId);

            //retrieve S&D Personnel Related Cost
            if (isCOA2016)
            {
                financeitem = 1092;
            }
            else
            {
                financeitem = 146;
            }
            BudgetForFinance budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                SDPersonalRelatedCost = GMSUtil.ToDouble(budgetItemdata.Total);

            //retrieve A&G Personnel Related Cost
            if (isCOA2016)
            {
                financeitem = 1103;
            }
            else
            {
                financeitem = 157;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                AGPersonalRelatedCost = GMSUtil.ToDouble(budgetItemdata.Total);

            PersonalExpenses = (SDPersonalRelatedCost + AGPersonalRelatedCost);
            SetPreCashFlowProjectionItem("txtPE", "TextBox", Math.Round(PersonalExpenses, 0), columnId);

            //retrieve S&D Carriage/Transportation
            if (isCOA2016)
            {
                financeitem = 1093;
            }
            else
            {
                financeitem = 147;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                SDCarriageTransportation = GMSUtil.ToDouble(budgetItemdata.Total);
            SetPreCashFlowProjectionItem("txtCT", "TextBox", Math.Round(SDCarriageTransportation, 0), columnId);

            //retrieve S&D Rental/Property Related
            if (isCOA2016)
            {
                financeitem = 1098;
            }
            else
            {
                financeitem = 153;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                SDRentalPropertyRelated = GMSUtil.ToDouble(budgetItemdata.Total);

            //retrieve A&G Rental/Property Related
            if (isCOA2016)
            {
                financeitem = 1108;
            }
            else
            {
                financeitem = 160;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                AGRentalPropertyRelated = GMSUtil.ToDouble(budgetItemdata.Total);

            //retrieve S&D Equipment Expenses
            if (isCOA2016)
            {
                financeitem = 1097;
            }
            else
            {
                financeitem = 152;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                SDEquipmentExpenses = GMSUtil.ToDouble(budgetItemdata.Total);

            //retrieve A&G Equipment Expenses
            if (isCOA2016)
            {
                financeitem = 1107;
            }
            else
            {
                financeitem = 159;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                AGEquipmentExpenses = GMSUtil.ToDouble(budgetItemdata.Total);

            PropertyEquipmentExpenses = (SDRentalPropertyRelated + AGRentalPropertyRelated + SDEquipmentExpenses + AGEquipmentExpenses);
            SetPreCashFlowProjectionItem("txtPEE", "TextBox", Math.Round(PropertyEquipmentExpenses, 0), columnId);

            //retrieve Advertising/Promotion
            if (isCOA2016)
            {
                financeitem = 1094;
            }
            else
            {
                financeitem = 148;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                Advertising = GMSUtil.ToDouble(budgetItemdata.Total);

            //retrieve Entertainment
            if (isCOA2016)
            {
                financeitem = 1096;
            }
            else
            {
                financeitem = 149;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                Entertaiment = GMSUtil.ToDouble(budgetItemdata.Total);

            //retrieve Overseas Travelling
            if (isCOA2016)
            {
                financeitem = 1095;
            }
            else
            {
                financeitem = 150;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                OverseasTravelling = GMSUtil.ToDouble(budgetItemdata.Total);

            //retrieve SD Stationery/Printing/Telephone
            if (isCOA2016)
            {
                financeitem = 1100;
            }
            else
            {
                financeitem = 154;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                SDStationeryPrintingTelepone = GMSUtil.ToDouble(budgetItemdata.Total);

            //retrieve Other S&D ExpAllocation
            if (isCOA2016)
            {
                financeitem = 1101;
            }
            else
            {
                financeitem = 155;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                OtherSDExpAllocation = GMSUtil.ToDouble(budgetItemdata.Total);

            //retrieve Legal/Prof Fees/Insurance
            if (isCOA2016)
            {
                financeitem = 1111;
            }
            else
            {
                financeitem = 158;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                LegalProfFeesInsurance = GMSUtil.ToDouble(budgetItemdata.Total);

            //retrieve Travelling/Entertaiment
            if (isCOA2016)
            {
                financeitem = 1104;
            }
            else
            {
                financeitem = 162;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                TravellingEntertaiment = GMSUtil.ToDouble(budgetItemdata.Total);

            //retrieve Transportation
            if (isCOA2016)
            {
                financeitem = 1105;
            }
            else
            {
                financeitem = 163;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                Transportation = GMSUtil.ToDouble(budgetItemdata.Total);

            //AG Stationery/Printing/Telephone
            if (isCOA2016)
            {
                financeitem = 1110;
            }
            else
            {
                financeitem = 164;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                AGStationeryPrintingTelepone = GMSUtil.ToDouble(budgetItemdata.Total);

            //AGOtherExp
            if (isCOA2016)
            {
                financeitem = 1118;
            }
            else
            {
                financeitem = 165;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                AGOtherExp = GMSUtil.ToDouble(budgetItemdata.Total);

            //AGAllocationRecovery
            if (isCOA2016)
            {
                financeitem = 1031;
            }
            else
            {
                financeitem = 166;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId(year, month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                AGAllocationRecovery = GMSUtil.ToDouble(budgetItemdata.Total);

            otherExpenses = (Advertising + Entertaiment + OverseasTravelling + SDStationeryPrintingTelepone + OtherSDExpAllocation +
                LegalProfFeesInsurance + TravellingEntertaiment + Transportation + AGStationeryPrintingTelepone + AGOtherExp + AGAllocationRecovery);
            SetPreCashFlowProjectionItem("txtOP", "TextBox", Math.Round(otherExpenses, 0), columnId);
        }


        protected void PopuldateAndResetCashFlowProjectionItemData()
        {
            LogSession session = base.GetSessionInfo();
            DateTime asOfDate = GMSUtil.ToDate(txtAsOfDate.Text.ToString());

            DateTime lastmonth = asOfDate.AddMonths(-1);
            //Date for Weekly Data
            DateTime nextmonth = asOfDate.AddMonths(1);

            //Date for Month Column
            DateTime month1 = asOfDate.AddMonths(2);
            DateTime month2 = asOfDate.AddMonths(3);
            DateTime month3 = asOfDate.AddMonths(4);

            //retrieve Total Sales from 3 months before & Change to first day of the month
            DateTime forecast = asOfDate.AddMonths(-3);
            forecast = new DateTime(forecast.Year, forecast.Month, 1);

            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCostOfSales = 0;
            double SDPersonalRelatedCost = 0;
            double AGPersonalRelatedCost = 0;
            double PersonalExpenses = 0;
            double SDCarriageTransportation = 0;
            double SDRentalPropertyRelated = 0;
            double AGRentalPropertyRelated = 0;
            double SDEquipmentExpenses = 0;
            double AGEquipmentExpenses = 0;
            double PropertyEquipmentExpenses = 0;
            double Advertising = 0;
            double Entertaiment = 0;
            double OverseasTravelling = 0;
            double SDStationeryPrintingTelepone = 0;
            double OtherSDExpAllocation = 0;
            double LegalProfFeesInsurance = 0;
            double TravellingEntertaiment = 0;
            double Transportation = 0;
            double AGStationeryPrintingTelepone = 0;
            double AGOtherExp = 0;
            double AGAllocationRecovery = 0;

            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet dsCompany = new DataSet();

            dacl.GetCompany(session.CompanyId, ref dsCompany);
            short financeitem = 0;

            DateTime COA2016 = GMSUtil.ToDate(dsCompany.Tables[0].Rows[0]["Is2016COA"].ToString());

            //Total Sales
            if (COA2016 >= forecast)
            {
                financeitem = 1083;
            }
            else
            {
                financeitem = 3;
            }
            FinanceData itemdata = new FinanceDataActivity().RetrieveFinanceDataByItemID((short)asOfDate.AddMonths(-3).Year, (short)asOfDate.AddMonths(-3).Month, financeitem, session.CompanyId);
            if (itemdata != null)
                CollectionFromSales = GMSUtil.ToDouble(itemdata.MTD) / 5;
            //Set Week Data
            SetPreCashFlowProjectionItem("txtCFS", "TextBox", Math.Round(CollectionFromSales, 0), 1);
            SetPreCashFlowProjectionItem("txtCFS", "TextBox", Math.Round(CollectionFromSales, 0), 2);
            SetPreCashFlowProjectionItem("txtCFS", "TextBox", Math.Round(CollectionFromSales, 0), 3);
            SetPreCashFlowProjectionItem("txtCFS", "TextBox", Math.Round(CollectionFromSales, 0), 4);
            SetPreCashFlowProjectionItem("txtCFS", "TextBox", Math.Round(CollectionFromSales, 0), 5);

            //retrieve Total Other Operationg Income
            if (COA2016 >= forecast)
            {
                financeitem = 858;
            }
            else
            {
                financeitem = 10;
            }
            itemdata = new FinanceDataActivity().RetrieveFinanceDataByItemID((short)asOfDate.AddMonths(-3).Year, (short)asOfDate.AddMonths(-3).Month, financeitem, session.CompanyId);
            if (itemdata != null)
                OtherIncome = GMSUtil.ToDouble(itemdata.MTD) / 5;
            SetPreCashFlowProjectionItem("txtOI", "TextBox", Math.Round(OtherIncome, 0), 1);
            SetPreCashFlowProjectionItem("txtOI", "TextBox", Math.Round(OtherIncome, 0), 2);
            SetPreCashFlowProjectionItem("txtOI", "TextBox", Math.Round(OtherIncome, 0), 3);
            SetPreCashFlowProjectionItem("txtOI", "TextBox", Math.Round(OtherIncome, 0), 4);
            SetPreCashFlowProjectionItem("txtOI", "TextBox", Math.Round(OtherIncome, 0), 5);


            //retrieve Cost of Sales
            if (COA2016 >= forecast)
            {
                financeitem = 1086;
            }
            else
            {
                financeitem = 6;
            }
            itemdata = new FinanceDataActivity().RetrieveFinanceDataByItemID((short)asOfDate.AddMonths(-3).Year, (short)asOfDate.AddMonths(-3).Month, financeitem, session.CompanyId);
            if (itemdata != null)
                TotalCostOfSales = GMSUtil.ToDouble(itemdata.MTD) / 5;
            SetPreCashFlowProjectionItem("txtPTLS", "TextBox", Math.Round(TotalCostOfSales, 0), 1);
            SetPreCashFlowProjectionItem("txtPTLS", "TextBox", Math.Round(TotalCostOfSales, 0), 2);
            SetPreCashFlowProjectionItem("txtPTLS", "TextBox", Math.Round(TotalCostOfSales, 0), 3);
            SetPreCashFlowProjectionItem("txtPTLS", "TextBox", Math.Round(TotalCostOfSales, 0), 4);
            SetPreCashFlowProjectionItem("txtPTLS", "TextBox", Math.Round(TotalCostOfSales, 0), 5);

            //retrieve S&D Personnel Related Cost
            if (COA2016 >= forecast)
            {
                financeitem = 1092;
            }
            else
            {
                financeitem = 146;
            }
            BudgetForFinance budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                SDPersonalRelatedCost = GMSUtil.ToDouble(budgetItemdata.Total);
            //retrieve A&G Personnel Related Cost
            if (COA2016 >= forecast)
            {
                financeitem = 1103;
            }
            else
            {
                financeitem = 157;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                AGPersonalRelatedCost = GMSUtil.ToDouble(budgetItemdata.Total);

            PersonalExpenses = (SDPersonalRelatedCost + AGPersonalRelatedCost) / 5;
            SetPreCashFlowProjectionItem("txtPE", "TextBox", Math.Round(PersonalExpenses, 0), 1);
            SetPreCashFlowProjectionItem("txtPE", "TextBox", Math.Round(PersonalExpenses, 0), 2);
            SetPreCashFlowProjectionItem("txtPE", "TextBox", Math.Round(PersonalExpenses, 0), 3);
            SetPreCashFlowProjectionItem("txtPE", "TextBox", Math.Round(PersonalExpenses, 0), 4);
            SetPreCashFlowProjectionItem("txtPE", "TextBox", Math.Round(PersonalExpenses, 0), 5);

            //retrieve S&D Carriage/Transportation
            if (COA2016 >= forecast)
            {
                financeitem = 1093;
            }
            else
            {
                financeitem = 147;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                SDCarriageTransportation = GMSUtil.ToDouble(budgetItemdata.Total) / 5;
            SetPreCashFlowProjectionItem("txtCT", "TextBox", Math.Round(SDCarriageTransportation, 0), 1);
            SetPreCashFlowProjectionItem("txtCT", "TextBox", Math.Round(SDCarriageTransportation, 0), 2);
            SetPreCashFlowProjectionItem("txtCT", "TextBox", Math.Round(SDCarriageTransportation, 0), 3);
            SetPreCashFlowProjectionItem("txtCT", "TextBox", Math.Round(SDCarriageTransportation, 0), 4);
            SetPreCashFlowProjectionItem("txtCT", "TextBox", Math.Round(SDCarriageTransportation, 0), 5);


            //retrieve S&D Rental/Property Related
            if (COA2016 >= forecast)
            {
                financeitem = 1098;
            }
            else
            {
                financeitem = 153;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                SDRentalPropertyRelated = GMSUtil.ToDouble(budgetItemdata.Total);
            //retrieve A&G Rental/Property Related
            if (COA2016 >= forecast)
            {
                financeitem = 1108;
            }
            else
            {
                financeitem = 160;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                AGRentalPropertyRelated = GMSUtil.ToDouble(budgetItemdata.Total);
            //retrieve S&D Equipment Expenses
            if (COA2016 >= forecast)
            {
                financeitem = 1097;
            }
            else
            {
                financeitem = 152;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                SDEquipmentExpenses = GMSUtil.ToDouble(budgetItemdata.Total);
            //retrieve A&G Equipment Expenses
            if (COA2016 >= forecast)
            {
                financeitem = 1107;
            }
            else
            {
                financeitem = 159;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                AGEquipmentExpenses = GMSUtil.ToDouble(budgetItemdata.Total);

            PropertyEquipmentExpenses = (SDRentalPropertyRelated + AGRentalPropertyRelated + SDEquipmentExpenses + AGEquipmentExpenses) / 5;
            SetPreCashFlowProjectionItem("txtPEE", "TextBox", Math.Round(PropertyEquipmentExpenses, 0), 1);
            SetPreCashFlowProjectionItem("txtPEE", "TextBox", Math.Round(PropertyEquipmentExpenses, 0), 2);
            SetPreCashFlowProjectionItem("txtPEE", "TextBox", Math.Round(PropertyEquipmentExpenses, 0), 3);
            SetPreCashFlowProjectionItem("txtPEE", "TextBox", Math.Round(PropertyEquipmentExpenses, 0), 4);
            SetPreCashFlowProjectionItem("txtPEE", "TextBox", Math.Round(PropertyEquipmentExpenses, 0), 5);

            //retrieve Advertising/Promotion
            if (COA2016 >= forecast)
            {
                financeitem = 1094;
            }
            else
            {
                financeitem = 148;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                Advertising = GMSUtil.ToDouble(budgetItemdata.Total);
            //retrieve Entertainment
            if (COA2016 >= forecast)
            {
                financeitem = 1096;
            }
            else
            {
                financeitem = 149;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                Entertaiment = GMSUtil.ToDouble(budgetItemdata.Total);
            //retrieve Overseas Travelling
            if (COA2016 >= forecast)
            {
                financeitem = 1095;
            }
            else
            {
                financeitem = 150;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                OverseasTravelling = GMSUtil.ToDouble(budgetItemdata.Total);
            //retrieve SD Stationery/Printing/Telephone
            if (COA2016 >= forecast)
            {
                financeitem = 1100;
            }
            else
            {
                financeitem = 154;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                SDStationeryPrintingTelepone = GMSUtil.ToDouble(budgetItemdata.Total);
            //retrieve Other S&D ExpAllocation
            if (COA2016 >= forecast)
            {
                financeitem = 1101;
            }
            else
            {
                financeitem = 155;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                OtherSDExpAllocation = GMSUtil.ToDouble(budgetItemdata.Total);
            //retrieve Legal/Prof Fees/Insurance
            if (COA2016 >= forecast)
            {
                financeitem = 1111;
            }
            else
            {
                financeitem = 158;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                LegalProfFeesInsurance = GMSUtil.ToDouble(budgetItemdata.Total);
            //retrieve Travelling/Entertaiment
            if (COA2016 >= forecast)
            {
                financeitem = 1104;
            }
            else
            {
                financeitem = 162;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                TravellingEntertaiment = GMSUtil.ToDouble(budgetItemdata.Total);
            if (COA2016 >= forecast)
            {
                financeitem = 1106;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                TravellingEntertaiment = TravellingEntertaiment + GMSUtil.ToDouble(budgetItemdata.Total);
            //retrieve Transportation
            if (COA2016 >= forecast)
            {
                financeitem = 1105;
            }
            else
            {
                financeitem = 163;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                Transportation = GMSUtil.ToDouble(budgetItemdata.Total);
            //AG Stationery/Printing/Telephone
            if (COA2016 >= forecast)
            {
                financeitem = 1110;
            }
            else
            {
                financeitem = 164;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                AGStationeryPrintingTelepone = GMSUtil.ToDouble(budgetItemdata.Total);
            //AGOtherExp
            if (COA2016 >= forecast)
            {
                financeitem = 1118;
            }
            else
            {
                financeitem = 165;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                AGOtherExp = GMSUtil.ToDouble(budgetItemdata.Total);
            //AGAllocationRecovery
            if (COA2016 >= forecast)
            {
                financeitem = 1031;
            }
            else
            {
                financeitem = 166;
            }
            budgetItemdata = new BudgetActivity().RetrieveBudgetForFinanceByItemId((short)nextmonth.Year, (short)nextmonth.Month, financeitem, session.CompanyId);
            if (budgetItemdata != null)
                AGAllocationRecovery = GMSUtil.ToDouble(budgetItemdata.Total);

            double otherExpenses = (Advertising + Entertaiment + OverseasTravelling + SDStationeryPrintingTelepone + OtherSDExpAllocation +
                LegalProfFeesInsurance + TravellingEntertaiment + Transportation + AGStationeryPrintingTelepone + AGOtherExp + AGAllocationRecovery) / 5;
            SetPreCashFlowProjectionItem("txtOP", "TextBox", Math.Round(otherExpenses, 0), 1);
            SetPreCashFlowProjectionItem("txtOP", "TextBox", Math.Round(otherExpenses, 0), 2);
            SetPreCashFlowProjectionItem("txtOP", "TextBox", Math.Round(otherExpenses, 0), 3);
            SetPreCashFlowProjectionItem("txtOP", "TextBox", Math.Round(otherExpenses, 0), 4);
            SetPreCashFlowProjectionItem("txtOP", "TextBox", Math.Round(otherExpenses, 0), 5);



            DataSet dsCashFlowProjections = new DataSet();
            dacl.GetCashFlowProjectionAsOf(session.CompanyId, (short)lastmonth.Year, (short)lastmonth.Month, ref dsCashFlowProjections);

            // has last month projection data, pre-populate month1 and month2 records, then populate month3 from P&L and Budget Data  
            // else Populate month1, month2 and month3 data from P&L and Budget Data
            if (dsCashFlowProjections.Tables[0].Rows.Count > 0)
            {
                LoadCashFlowProjectionItemData(dsCashFlowProjections, true);
                PopuldateAndResetCashFlowProjectionItemDataForMonth((short)asOfDate.Year, (short)asOfDate.Month, 8);
            }
            else
            {
                PopuldateAndResetCashFlowProjectionItemDataForMonth((short)asOfDate.AddMonths(-2).Year, (short)asOfDate.AddMonths(-2).Month, 6);
                PopuldateAndResetCashFlowProjectionItemDataForMonth((short)asOfDate.AddMonths(-1).Year, (short)asOfDate.AddMonths(-1).Month, 7);
                PopuldateAndResetCashFlowProjectionItemDataForMonth((short)asOfDate.Year, (short)asOfDate.Month, 8);
            }

            CalculateTextChanged(1);
            CalculateTextChanged(2);
            CalculateTextChanged(3);
            CalculateTextChanged(4);
            CalculateTextChanged(5);
            CalculateTextChanged(6);
            CalculateTextChanged(7);
            CalculateTextChanged(8);



            /*
            //1	Cash Inflow from Operating Activities
            //2	Collection from Sales  货款回笼
            InsertCashFlowProjectionItem(2, asOfYear, asOfMonth, "txtCFS", session, "TextBox");
            //3	Other Income 其他收入
            InsertCashFlowProjectionItem(3, asOfYear, asOfMonth, "txtOI", session, "TextBox");
            //4	Total Cash Inflow 
            InsertCashFlowProjectionItem(4, asOfYear, asOfMonth, "lblTCI", session, "Label");
            //5	Cash Outflow from Operating Activities
            //6	Payment to Overseas Suppliers 支付海外供应商
            InsertCashFlowProjectionItem(6, asOfYear, asOfMonth, "txtPTOS", session, "TextBox");
            //7	Payment to Local Suppliers 支付本地供应商
            InsertCashFlowProjectionItem(7, asOfYear, asOfMonth, "txtPTLS", session, "TextBox");
            //8	Personnel Expenses 人事相关费用   
            InsertCashFlowProjectionItem(8, asOfYear, asOfMonth, "txtPE", session, "TextBox");
            //9	Carriage/Transportation 货代/交通
            InsertCashFlowProjectionItem(9, asOfYear, asOfMonth, "txtCT", session, "TextBox");
            //10	Property/ Equipment Expenses 房产/设备费用    
            InsertCashFlowProjectionItem(10, asOfYear, asOfMonth, "txtPEE", session, "TextBox");
            //11	Other Expenses 其他费用 
            InsertCashFlowProjectionItem(11, asOfYear, asOfMonth, "txtOP", session, "TextBox");
            //12	Taxes payment (GST) (增值税)
            InsertCashFlowProjectionItem(12, asOfYear, asOfMonth, "txtTP", session, "TextBox");
            //13	Taxes payment (Corporate) (企业所得税)
            InsertCashFlowProjectionItem(13, asOfYear, asOfMonth, "txTPC", session, "TextBox");
            //14	Total Operating Expenses
            InsertCashFlowProjectionItem(14, asOfYear, asOfMonth, "lblTOE", session, "Label");
            //15	Net Operating Cash Flow 
            InsertCashFlowProjectionItem(15, asOfYear, asOfMonth, "lblNPCF", session, "Label");
            //16	Cash Flow from Investing Activities
            //17	Purchase of Fixed Assets 购买固定资产
            InsertCashFlowProjectionItem(17, asOfYear, asOfMonth, "txtPOFA", session, "TextBox");
            //18	Investments/Other Assets 投资/购买其他资产
            InsertCashFlowProjectionItem(18, asOfYear, asOfMonth, "txtIO", session, "TextBox");
            //19	Disposal of Fixed Assets 出售固定资产
            InsertCashFlowProjectionItem(19, asOfYear, asOfMonth, "txtDOFA", session, "TextBox");
            //20	Disposal of Investments/Others 出售投资/其他资产
            InsertCashFlowProjectionItem(20, asOfYear, asOfMonth, "txtDOIO", session, "TextBox");
            //21	Loan To Intercompany 关联公司贷款
            InsertCashFlowProjectionItem(21, asOfYear, asOfMonth, "txtLTI", session, "TextBox");
            //22	Interest Received 已收利息
            InsertCashFlowProjectionItem(22, asOfYear, asOfMonth, "txtIR", session, "TextBox");
            //23	Dividends Received 已收股息
            InsertCashFlowProjectionItem(23, asOfYear, asOfMonth, "txtDR", session, "TextBox");
            //24	Dividends Paid 已付股息
            InsertCashFlowProjectionItem(24, asOfYear, asOfMonth, "txtDP", session, "TextBox");
            //25	Net Cash Flow From Investing
            InsertCashFlowProjectionItem(25, asOfYear, asOfMonth, "lblNCFFI", session, "Label");
            //26	Cash Flow from Financing Activities
            //27	Proceeds of Bank Loans 新银行貸款
            InsertCashFlowProjectionItem(27, asOfYear, asOfMonth, "txtPOBL", session, "TextBox");
            //28	Repayment of Bank Loans 偿还银行貸款 
            InsertCashFlowProjectionItem(28, asOfYear, asOfMonth, "txtROBL", session, "TextBox");
            //29	Repayment of Trade Financing 偿还贸易融资
            InsertCashFlowProjectionItem(29, asOfYear, asOfMonth, "txtROTF", session, "TextBox");
            //30	Payment of Interests 支付利息
            InsertCashFlowProjectionItem(30, asOfYear, asOfMonth, "txtPOI", session, "TextBox");
            //31	New Capital/Convertible Loan 新资本/可换股贷款
            InsertCashFlowProjectionItem(31, asOfYear, asOfMonth, "txtNCCL", session, "TextBox");
            //32	Loan From Intercompany 关联公司贷款
            InsertCashFlowProjectionItem(32, asOfYear, asOfMonth, "txtLFI", session, "TextBox");
            //33	Repayment of Intercompany Loan 偿还关联公司貸款
            InsertCashFlowProjectionItem(33, asOfYear, asOfMonth, "txtROIL", session, "TextBox");
            //34	Net Cash Flow From Financing
            InsertCashFlowProjectionItem(34, asOfYear, asOfMonth, "lblNCFFF", session, "Label");
            //35	Net Cash Flow 净现金流
            InsertCashFlowProjectionItem(35, asOfYear, asOfMonth, "lblNCF", session, "Label");
            //36	Add: Total Available Fund 可用资金
            //37	Net Surplus (Deficit) 净可用资金(不足额)
            */

        }

        public void btnSubmit_Click(object sender, EventArgs e)
        {

            LoadCashFlowProjectionData();

        }

        protected void SetPreCashFlowProjectionItem(string ControlName, string ControlType, double total, int ControlId)
        {
            if (ControlType == "TextBox")
                ((TextBox)upCashFlowData.FindControl(ControlName + ControlId)).Text = total.ToString();
            else if (ControlType == "Label")
                ((Label)upCashFlowData.FindControl(ControlName + 1)).Text = total.ToString();
        }


        protected void SetCashFlowProjectionItem(short CashFlowItemID, string ControlName, LogSession session, string ControlType, DataTable dt, int RowNumber, bool setMonthOnly)
        {

            if (ControlType == "TextBox")
            {
                if (setMonthOnly == true)
                {
                    ((TextBox)upCashFlowData.FindControl(ControlName + 6)).Text = dt.Rows[RowNumber]["tbMonth2"].ToString();
                    ((TextBox)upCashFlowData.FindControl(ControlName + 7)).Text = dt.Rows[RowNumber]["tbMonth3"].ToString();
                    ((TextBox)upCashFlowData.FindControl(ControlName + 8)).Text = "0".ToString();
                }
                else
                {
                    ((TextBox)upCashFlowData.FindControl(ControlName + 1)).Text = dt.Rows[RowNumber]["tbWeek1"].ToString();
                    ((TextBox)upCashFlowData.FindControl(ControlName + 2)).Text = dt.Rows[RowNumber]["tbWeek2"].ToString();
                    ((TextBox)upCashFlowData.FindControl(ControlName + 3)).Text = dt.Rows[RowNumber]["tbWeek3"].ToString();
                    ((TextBox)upCashFlowData.FindControl(ControlName + 4)).Text = dt.Rows[RowNumber]["tbWeek4"].ToString();
                    ((TextBox)upCashFlowData.FindControl(ControlName + 5)).Text = dt.Rows[RowNumber]["tbWeek5"].ToString();
                    ((TextBox)upCashFlowData.FindControl(ControlName + 6)).Text = dt.Rows[RowNumber]["tbMonth1"].ToString();
                    ((TextBox)upCashFlowData.FindControl(ControlName + 7)).Text = dt.Rows[RowNumber]["tbMonth2"].ToString();
                    ((TextBox)upCashFlowData.FindControl(ControlName + 8)).Text = dt.Rows[RowNumber]["tbMonth3"].ToString();

                }

            }
            else if (ControlType == "Label")
            {
                if (setMonthOnly == true)
                {
                    ((Label)upCashFlowData.FindControl(ControlName + 6)).Text = dt.Rows[RowNumber]["tbMonth2"].ToString();
                    ((Label)upCashFlowData.FindControl(ControlName + 7)).Text = dt.Rows[RowNumber]["tbMonth3"].ToString();
                    ((Label)upCashFlowData.FindControl(ControlName + 7)).Text = "0".ToString();
                }
                else
                {
                    ((Label)upCashFlowData.FindControl(ControlName + 1)).Text = dt.Rows[RowNumber]["tbWeek1"].ToString();
                    ((Label)upCashFlowData.FindControl(ControlName + 2)).Text = dt.Rows[RowNumber]["tbWeek2"].ToString();
                    ((Label)upCashFlowData.FindControl(ControlName + 3)).Text = dt.Rows[RowNumber]["tbWeek3"].ToString();
                    ((Label)upCashFlowData.FindControl(ControlName + 4)).Text = dt.Rows[RowNumber]["tbWeek4"].ToString();
                    ((Label)upCashFlowData.FindControl(ControlName + 5)).Text = dt.Rows[RowNumber]["tbWeek5"].ToString();
                    ((Label)upCashFlowData.FindControl(ControlName + 6)).Text = dt.Rows[RowNumber]["tbMonth1"].ToString();
                    ((Label)upCashFlowData.FindControl(ControlName + 7)).Text = dt.Rows[RowNumber]["tbMonth2"].ToString();
                    ((Label)upCashFlowData.FindControl(ControlName + 8)).Text = dt.Rows[RowNumber]["tbMonth3"].ToString();

                }

            }

        }

        protected void InsertCashFlowProjectionItem(short CashFlowItemID, short asOfYear, short asOfMonth, string ControlName, LogSession session, string ControlType)
        {
            GMSCore.Entity.CashFlowProjectionForWeek cashflowproj = new GMSCore.Entity.CashFlowProjectionForWeek();
            cashflowproj.CoyID = GMSUtil.ToShort(session.CompanyId.ToString());
            cashflowproj.CashFlowItemID = GMSUtil.ToShort(CashFlowItemID.ToString());
            cashflowproj.TbAsOfYear = GMSUtil.ToShort(asOfYear);
            cashflowproj.TbAsOfMonth = GMSUtil.ToShort(asOfMonth);

            if (ControlType == "TextBox")
            {
                cashflowproj.TbWeek1 = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl(ControlName + 1)).Text);
                cashflowproj.TbWeek2 = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl(ControlName + 2)).Text);
                cashflowproj.TbWeek3 = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl(ControlName + 3)).Text);
                cashflowproj.TbWeek4 = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl(ControlName + 4)).Text);
                cashflowproj.TbWeek5 = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl(ControlName + 5)).Text);
                cashflowproj.TbMonth1 = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl(ControlName + 6)).Text);
                cashflowproj.TbMonth2 = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl(ControlName + 7)).Text);
                cashflowproj.TbMonth3 = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl(ControlName + 8)).Text);
            }
            else if (ControlType == "Label")
            {
                cashflowproj.TbWeek1 = GMSUtil.ToDouble(((Label)upCashFlowData.FindControl(ControlName + 1)).Text);
                cashflowproj.TbWeek2 = GMSUtil.ToDouble(((Label)upCashFlowData.FindControl(ControlName + 2)).Text);
                cashflowproj.TbWeek3 = GMSUtil.ToDouble(((Label)upCashFlowData.FindControl(ControlName + 3)).Text);
                cashflowproj.TbWeek4 = GMSUtil.ToDouble(((Label)upCashFlowData.FindControl(ControlName + 4)).Text);
                cashflowproj.TbWeek5 = GMSUtil.ToDouble(((Label)upCashFlowData.FindControl(ControlName + 5)).Text);
                cashflowproj.TbMonth1 = GMSUtil.ToDouble(((Label)upCashFlowData.FindControl(ControlName + 6)).Text);
                cashflowproj.TbMonth2 = GMSUtil.ToDouble(((Label)upCashFlowData.FindControl(ControlName + 7)).Text);
                cashflowproj.TbMonth3 = GMSUtil.ToDouble(((Label)upCashFlowData.FindControl(ControlName + 8)).Text);
            }
            cashflowproj.PreparedBy = GMSUtil.ToShort(session.UserId.ToString());
            cashflowproj.Save();
            cashflowproj.Resync();

        }



        protected void InsertCashFlowProjectionData(short asOfYear, short asOfMonth)
        {
            LogSession session = base.GetSessionInfo();

            //1	Cash Inflow from Operating Activities
            //2	Collection from Sales  货款回笼
            InsertCashFlowProjectionItem(2, asOfYear, asOfMonth, "txtCFS", session, "TextBox");
            //3	Other Income 其他收入
            InsertCashFlowProjectionItem(3, asOfYear, asOfMonth, "txtOI", session, "TextBox");
            //4	Total Cash Inflow 
            InsertCashFlowProjectionItem(4, asOfYear, asOfMonth, "lblTCI", session, "Label");
            //5	Cash Outflow from Operating Activities
            //6	Payment to Overseas Suppliers 支付海外供应商
            InsertCashFlowProjectionItem(6, asOfYear, asOfMonth, "txtPTOS", session, "TextBox");
            //7	Payment to Local Suppliers 支付本地供应商
            InsertCashFlowProjectionItem(7, asOfYear, asOfMonth, "txtPTLS", session, "TextBox");
            //8	Personnel Expenses 人事相关费用   
            InsertCashFlowProjectionItem(8, asOfYear, asOfMonth, "txtPE", session, "TextBox");
            //9	Carriage/Transportation 货代/交通
            InsertCashFlowProjectionItem(9, asOfYear, asOfMonth, "txtCT", session, "TextBox");
            //10	Property/ Equipment Expenses 房产/设备费用    
            InsertCashFlowProjectionItem(10, asOfYear, asOfMonth, "txtPEE", session, "TextBox");
            //11	Other Expenses 其他费用 
            InsertCashFlowProjectionItem(11, asOfYear, asOfMonth, "txtOP", session, "TextBox");
            //12	Taxes payment (GST) (增值税)
            InsertCashFlowProjectionItem(12, asOfYear, asOfMonth, "txtTP", session, "TextBox");
            //13	Taxes payment (Corporate) (企业所得税)
            InsertCashFlowProjectionItem(13, asOfYear, asOfMonth, "txtTPC", session, "TextBox");
            //14	Total Operating Expenses
            InsertCashFlowProjectionItem(14, asOfYear, asOfMonth, "lblTOE", session, "Label");
            //15	Net Operating Cash Flow 
            InsertCashFlowProjectionItem(15, asOfYear, asOfMonth, "lblNPCF", session, "Label");
            //16	Cash Flow from Investing Activities
            //17	Purchase of Fixed Assets 购买固定资产
            InsertCashFlowProjectionItem(17, asOfYear, asOfMonth, "txtPOFA", session, "TextBox");
            //18	Investments/Other Assets 投资/购买其他资产
            InsertCashFlowProjectionItem(18, asOfYear, asOfMonth, "txtIO", session, "TextBox");
            //19	Disposal of Fixed Assets 出售固定资产
            InsertCashFlowProjectionItem(19, asOfYear, asOfMonth, "txtDOFA", session, "TextBox");
            //20	Disposal of Investments/Others 出售投资/其他资产
            InsertCashFlowProjectionItem(20, asOfYear, asOfMonth, "txtDOIO", session, "TextBox");
            //21	Loan To Intercompany 关联公司贷款
            InsertCashFlowProjectionItem(21, asOfYear, asOfMonth, "txtLTI", session, "TextBox");
            //22	Interest Received 已收利息
            InsertCashFlowProjectionItem(22, asOfYear, asOfMonth, "txtIR", session, "TextBox");
            //23	Dividends Received 已收股息
            InsertCashFlowProjectionItem(23, asOfYear, asOfMonth, "txtDR", session, "TextBox");
            //24	Dividends Paid 已付股息
            InsertCashFlowProjectionItem(24, asOfYear, asOfMonth, "txtDP", session, "TextBox");
            //25	Net Cash Flow From Investing
            InsertCashFlowProjectionItem(25, asOfYear, asOfMonth, "lblNCFFI", session, "Label");
            //26	Cash Flow from Financing Activities
            //27	Proceeds of Bank Loans 新银行貸款
            InsertCashFlowProjectionItem(27, asOfYear, asOfMonth, "txtPOBL", session, "TextBox");
            //28	Repayment of Bank Loans 偿还银行貸款 
            InsertCashFlowProjectionItem(28, asOfYear, asOfMonth, "txtROBL", session, "TextBox");
            //29	Repayment of Trade Financing 偿还贸易融资
            InsertCashFlowProjectionItem(29, asOfYear, asOfMonth, "txtROTF", session, "TextBox");
            //30	Payment of Interests 支付利息
            InsertCashFlowProjectionItem(30, asOfYear, asOfMonth, "txtPOI", session, "TextBox");
            //31	New Capital/Convertible Loan 新资本/可换股贷款
            InsertCashFlowProjectionItem(31, asOfYear, asOfMonth, "txtNCCL", session, "TextBox");
            //32	Loan From Intercompany 关联公司贷款
            InsertCashFlowProjectionItem(32, asOfYear, asOfMonth, "txtLFI", session, "TextBox");
            //33	Repayment of Intercompany Loan 偿还关联公司貸款
            InsertCashFlowProjectionItem(33, asOfYear, asOfMonth, "txtROIL", session, "TextBox");
            //34	Net Cash Flow From Financing
            InsertCashFlowProjectionItem(34, asOfYear, asOfMonth, "lblNCFFF", session, "Label");
            //35	Net Cash Flow 净现金流
            InsertCashFlowProjectionItem(35, asOfYear, asOfMonth, "lblNCF", session, "Label");
            //36	Add: Total Available Fund 可用资金
            //37	Net Surplus (Deficit) 净可用资金(不足额)






            /*

            short year = 0;
            short month = 0;
            short week = 0;
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double SalesmanClaim = 0;
            double SalaryPayment = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            for (int i = 1; i < 9; i++)
            {

                HiddenField hidYear1 = (HiddenField)upCashFlowData.FindControl("hidYear" + i);
                year = GMSUtil.ToShort(hidYear1.Value);
                HiddenField hidMonth1 = (HiddenField)upCashFlowData.FindControl("hidMonth" + i);
                month = GMSUtil.ToShort(hidMonth1.Value);
                Label lblWeek1 = (Label)upCashFlowData.FindControl("lblWeek" + i);
                week = GMSUtil.ToShort(lblWeek1.Text);
                               
                //Cash Inflow From Operating Activities
                TextBox txtCFS1 = (TextBox)upCashFlowData.FindControl("txtCFS" + i);
                CollectionFromSales = GMSUtil.ToDouble(txtCFS1.Text);
                TextBox txtOI1 = (TextBox)upCashFlowData.FindControl("txtOI" + i);
                OtherIncome = GMSUtil.ToDouble(txtOI1.Text);
                TotalCashInflow = CollectionFromSales + OtherIncome;


                //Less Cash Outflow from Operating Activities
                TextBox txtPTOS1 = (TextBox)upCashFlowData.FindControl("txtPTOS" + i);
                PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS1.Text);
                TextBox txtPTLS1 = (TextBox)upCashFlowData.FindControl("txtPTLS" + i);
                PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS1.Text);
                TextBox txtSC1 = (TextBox)upCashFlowData.FindControl("txtSC" + i);
                SalesmanClaim = GMSUtil.ToDouble(txtSC1.Text);
                TextBox txtSP1 = (TextBox)upCashFlowData.FindControl("txtSP" + i);
                SalaryPayment = GMSUtil.ToDouble(txtSP1.Text);
                TextBox txtOP1 = (TextBox)upCashFlowData.FindControl("txtOP" + i);
                OtherPayment = GMSUtil.ToDouble(txtOP1.Text);
                TextBox txtTP1 = (TextBox)upCashFlowData.FindControl("txtTP" + i);
                TaxsPayment = GMSUtil.ToDouble(txtTP1.Text);
                TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + SalesmanClaim + SalaryPayment + OtherPayment + TaxsPayment;
                NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;

                //Less/Add Cash Flow from Investing Activities
                TextBox txtPOFA1 = (TextBox)upCashFlowData.FindControl("txtPOFA" + i);
                PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA1.Text);
                TextBox txtIO1 = (TextBox)upCashFlowData.FindControl("txtIO" + i);
                Investments = GMSUtil.ToDouble(txtIO1.Text);
                TextBox txtDOFA = (TextBox)upCashFlowData.FindControl("txtDOFA" + i);
                DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA1.Text);
                TextBox txtDOIO1 = (TextBox)upCashFlowData.FindControl("txtDOIO" + i);
                DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO1.Text);
                TextBox txtLTI1 = (TextBox)upCashFlowData.FindControl("txtLTI" + i);
                LoanToIntercompany = GMSUtil.ToDouble(txtLTI1.Text);
                TextBox txtIR1 = (TextBox)upCashFlowData.FindControl("txtIR" + i);
                InterestReceived = GMSUtil.ToDouble(txtIR1.Text);
                TextBox txtDR1 = (TextBox)upCashFlowData.FindControl("txtDR" + i);
                DividendReceived = GMSUtil.ToDouble(txtDR1.Text);
                TextBox txtDP1 = (TextBox)upCashFlowData.FindControl("txtDP" + i);
                DividendPaid = GMSUtil.ToDouble(txtDP1.Text);
                NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;

                //Less/Add Financing Activities
                TextBox txtPOBL1 = (TextBox)upCashFlowData.FindControl("txtPOBL" + i);
                ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL1.Text);
                TextBox txtROBL1 = (TextBox)upCashFlowData.FindControl("txtROBL" + i);
                RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL1.Text);
                TextBox txtROTF1 = (TextBox)upCashFlowData.FindControl("txtROTF" + i);
                RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF1.Text);
                TextBox txtPOI1 = (TextBox)upCashFlowData.FindControl("txtPOI" + i);
                PaymentOfInterests = GMSUtil.ToDouble(txtPOI1.Text);
                TextBox txtNCCL1 = (TextBox)upCashFlowData.FindControl("txtNCCL" + i);
                NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL1.Text);
                TextBox txtLFI1 = (TextBox)upCashFlowData.FindControl("txtLFI" + i);
                LoanFromIntercompany = GMSUtil.ToDouble(txtLFI1.Text);
                TextBox txtROIL1 = (TextBox)upCashFlowData.FindControl("txtROIL" + i);
                RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL1.Text);

                NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
                NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;


                dacl.InsertCashFlowProjection(session.CompanyId, year, month, week,
                    CollectionFromSales, OtherIncome, TotalCashInflow, PaymentToOverseasSupplier, PaymentToLocalSupplier,
                    SalesmanClaim, SalaryPayment, OtherPayment, TaxsPayment, TotalOperatingExpenses,
                    NetOperatingCashFlow, PurchaseofFixedAssets, Investments, DisposalOfFixedAssets, DisposalOfInvestmentsOthers,
                    LoanToIntercompany, InterestReceived, DividendReceived, DividendPaid, NetCashFlowFromInvesting,
                    ProceedsOfBankLoans, RepaymentOfBankLoans, RepaymentOfTradeFinancing, PaymentOfInterests, NewCapitalConvertibleLoan,
                    LoanFromIntercompany, RepaymentOfIntercompanyLoan, NetCashFlowFromFinancing, NetCashFlowSurplusDeficit,
                    session.UserId, ref dsCashFlowProjectionData);

            }

            */

        }

        protected void btnSubmitData_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            DateTime asOfDate = GMSUtil.ToDate(txtAsOfDate.Text.ToString());
            short asOfYear = (short)asOfDate.Year;
            short asOfMonth = (short)asOfDate.Month;
            //Remove Cash Flow Projection
            new GMSGeneralDALC().DeleteCashFlowProjAsOf(session.CompanyId, asOfYear, asOfMonth);

            InsertCashFlowProjectionData(asOfYear, asOfMonth);

            ModalPopupExtender2.Hide();
            //LoadCashFlowProjectionData();
            ScriptManager.RegisterStartupScript(this, typeof(Page), "", "alert('Data has been successfully saved!')", true);

        }


        protected void CalculateTextChanged(short controlIndex)
        {
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double PersonalExpenses = 0;
            double CarriageTransportation = 0;
            double PropertyEquipmentExpenses = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TaxsPaymentCorp = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;



            CollectionFromSales = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtCFS" + controlIndex)).Text);
            OtherIncome = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtOI" + controlIndex)).Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            Label lblTCI = (Label)upCashFlowData.FindControl("lblTCI" + controlIndex);
            lblTCI.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtPTOS" + controlIndex)).Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtPTLS" + controlIndex)).Text);
            PersonalExpenses = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtPE" + controlIndex)).Text);
            CarriageTransportation = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtCT" + controlIndex)).Text);
            PropertyEquipmentExpenses = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtPEE" + controlIndex)).Text);
            OtherPayment = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtOP" + controlIndex)).Text);
            TaxsPayment = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtTP" + controlIndex)).Text);
            TaxsPaymentCorp = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtTPC" + controlIndex)).Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + PersonalExpenses + CarriageTransportation + PropertyEquipmentExpenses + OtherPayment + TaxsPayment + TaxsPaymentCorp;
            NetOperatingCashFlow = TotalCashInflow + TotalOperatingExpenses;

            Label lblTOE = (Label)upCashFlowData.FindControl("lblTOE" + controlIndex);
            lblTOE.Text = TotalOperatingExpenses.ToString();

            Label lblNPCF = (Label)upCashFlowData.FindControl("lblNPCF" + controlIndex);
            lblNPCF.Text = NetOperatingCashFlow.ToString();


            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtPOFA" + controlIndex)).Text);
            Investments = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtIO" + controlIndex)).Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtDOFA" + controlIndex)).Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtDOIO" + controlIndex)).Text);
            LoanToIntercompany = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtLTI" + controlIndex)).Text);
            InterestReceived = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtIR" + controlIndex)).Text);
            DividendReceived = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtDR" + controlIndex)).Text);
            DividendPaid = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtDP" + controlIndex)).Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            Label lblNCFFI = (Label)upCashFlowData.FindControl("lblNCFFI" + controlIndex);
            lblNCFFI.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtPOBL" + controlIndex)).Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtROBL" + controlIndex)).Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtROTF" + controlIndex)).Text);
            PaymentOfInterests = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtPOI" + controlIndex)).Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtNCCL" + controlIndex)).Text);
            LoanFromIntercompany = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtLFI" + controlIndex)).Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(((TextBox)upCashFlowData.FindControl("txtROIL" + controlIndex)).Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            Label lblNCFFF = (Label)upCashFlowData.FindControl("lblNCFFF" + controlIndex);
            lblNCFFF.Text = NetCashFlowFromFinancing.ToString();
            Label lblNCF = (Label)upCashFlowData.FindControl("lblNCF" + controlIndex);
            lblNCF.Text = NetCashFlowSurplusDeficit.ToString();

        }


        protected void Calculate_OnTextChanged(object sender, EventArgs e)
        {
            CalculateTextChanged(1);
            /*
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double PersonalExpenses = 0;
            double CarriageTransportation = 0;
            double PropertyEquipmentExpenses = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS1.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI1.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI1.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS1.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS1.Text);
            PersonalExpenses = GMSUtil.ToDouble(txtPE1.Text);
            CarriageTransportation = GMSUtil.ToDouble(txtCT1.Text);
            PropertyEquipmentExpenses = GMSUtil.ToDouble(txtPEE1.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP1.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP1.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + PersonalExpenses + CarriageTransportation + PropertyEquipmentExpenses + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE1.Text = TotalOperatingExpenses.ToString();
            lblNPCF1.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA1.Text);
            Investments = GMSUtil.ToDouble(txtIO1.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA1.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO1.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI1.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR1.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR1.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP1.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI1.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL1.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL1.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF1.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI1.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL1.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI1.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL1.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF1.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD1.Text = NetCashFlowSurplusDeficit.ToString();
            */


        }

        protected void Calculate2_OnTextChanged(object sender, EventArgs e)
        {
            CalculateTextChanged(2);
            /*
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double PersonalExpenses = 0;
            double CarriageTransportation = 0;
            double PropertyEquipmentExpenses = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS2.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI2.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI2.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS2.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS2.Text);
            PersonalExpenses = GMSUtil.ToDouble(txtPE2.Text);
            CarriageTransportation = GMSUtil.ToDouble(txtCT2.Text);
            PropertyEquipmentExpenses = GMSUtil.ToDouble(txtPEE2.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP2.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP2.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + PersonalExpenses + CarriageTransportation + PropertyEquipmentExpenses + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE2.Text = TotalOperatingExpenses.ToString();
            lblNPCF2.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA2.Text);
            Investments = GMSUtil.ToDouble(txtIO2.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA2.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO2.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI2.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR2.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR2.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP2.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI2.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL2.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL2.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF2.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI2.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL2.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI2.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL2.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF2.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD2.Text = NetCashFlowSurplusDeficit.ToString();
            */


        }

        protected void Calculate3_OnTextChanged(object sender, EventArgs e)
        {
            CalculateTextChanged(3);
            /*
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double PersonalExpenses = 0;
            double CarriageTransportation = 0;
            double PropertyEquipmentExpenses = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS3.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI3.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI3.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS3.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS3.Text);
            PersonalExpenses = GMSUtil.ToDouble(txtPE3.Text);
            CarriageTransportation = GMSUtil.ToDouble(txtCT3.Text);
            PropertyEquipmentExpenses = GMSUtil.ToDouble(txtPEE3.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP3.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP3.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + PersonalExpenses + CarriageTransportation + PropertyEquipmentExpenses + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE3.Text = TotalOperatingExpenses.ToString();
            lblNPCF3.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA3.Text);
            Investments = GMSUtil.ToDouble(txtIO3.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA3.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO3.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI3.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR3.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR3.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP3.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI3.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL3.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL3.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF3.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI3.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL3.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI3.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL3.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF3.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD3.Text = NetCashFlowSurplusDeficit.ToString();
             */


        }

        protected void Calculate4_OnTextChanged(object sender, EventArgs e)
        {
            CalculateTextChanged(4);
            /*
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double PersonalExpenses = 0;
            double CarriageTransportation = 0;
            double PropertyEquipmentExpenses = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS4.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI4.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI4.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS4.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS4.Text);
            PersonalExpenses = GMSUtil.ToDouble(txtPE4.Text);
            CarriageTransportation = GMSUtil.ToDouble(txtCT4.Text);
            PropertyEquipmentExpenses = GMSUtil.ToDouble(txtPEE4.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP4.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP4.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + PersonalExpenses + CarriageTransportation + PropertyEquipmentExpenses + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE4.Text = TotalOperatingExpenses.ToString();
            lblNPCF4.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA4.Text);
            Investments = GMSUtil.ToDouble(txtIO4.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA4.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO4.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI4.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR4.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR4.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP4.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI4.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL4.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL4.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF4.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI4.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL4.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI4.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL4.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF4.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD4.Text = NetCashFlowSurplusDeficit.ToString();
            */


        }

        protected void Calculate5_OnTextChanged(object sender, EventArgs e)
        {
            CalculateTextChanged(5);
            /*
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double PersonalExpenses = 0;
            double CarriageTransportation = 0;
            double PropertyEquipmentExpenses = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS5.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI5.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI5.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS5.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS5.Text);
            PersonalExpenses = GMSUtil.ToDouble(txtPE5.Text);
            CarriageTransportation = GMSUtil.ToDouble(txtCT5.Text);
            PropertyEquipmentExpenses = GMSUtil.ToDouble(txtPEE5.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP5.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP5.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + PersonalExpenses + CarriageTransportation + PropertyEquipmentExpenses + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE5.Text = TotalOperatingExpenses.ToString();
            lblNPCF5.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA5.Text);
            Investments = GMSUtil.ToDouble(txtIO5.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA5.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO5.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI5.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR5.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR5.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP5.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI5.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL5.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL5.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF5.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI5.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL5.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI5.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL5.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF5.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD5.Text = NetCashFlowSurplusDeficit.ToString();
            */


        }

        protected void Calculate6_OnTextChanged(object sender, EventArgs e)
        {
            CalculateTextChanged(6);
            /*
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double PersonalExpenses = 0;
            double CarriageTransportation = 0;
            double PropertyEquipmentExpenses = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS6.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI6.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI6.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS6.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS6.Text);
            PersonalExpenses = GMSUtil.ToDouble(txtPE6.Text);
            CarriageTransportation = GMSUtil.ToDouble(txtCT6.Text);
            PropertyEquipmentExpenses = GMSUtil.ToDouble(txtPEE6.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP6.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP6.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + PersonalExpenses + CarriageTransportation + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE6.Text = TotalOperatingExpenses.ToString();
            lblNPCF6.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA6.Text);
            Investments = GMSUtil.ToDouble(txtIO6.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA6.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO6.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI6.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR6.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR6.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP6.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI6.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL6.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL6.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF6.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI6.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL6.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI6.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL6.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF6.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD6.Text = NetCashFlowSurplusDeficit.ToString();
             */


        }

        protected void Calculate7_OnTextChanged(object sender, EventArgs e)
        {
            CalculateTextChanged(7);
            /*
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double PersonalExpenses = 0;
            double CarriageTransportation = 0;
            double PropertyEquipmentExpenses = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS7.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI7.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI7.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS7.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS7.Text);
            PersonalExpenses = GMSUtil.ToDouble(txtPE7.Text);
            CarriageTransportation = GMSUtil.ToDouble(txtCT7.Text);
            PropertyEquipmentExpenses = GMSUtil.ToDouble(txtPEE7.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP7.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP7.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + PersonalExpenses + CarriageTransportation + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE7.Text = TotalOperatingExpenses.ToString();
            lblNPCF7.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA7.Text);
            Investments = GMSUtil.ToDouble(txtIO7.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA7.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO7.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI7.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR7.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR7.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP7.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI7.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL7.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL7.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF7.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI7.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL7.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI7.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL7.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF7.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD7.Text = NetCashFlowSurplusDeficit.ToString();
             */


        }

        protected void Calculate8_OnTextChanged(object sender, EventArgs e)
        {
            CalculateTextChanged(8);
            /*
            double CollectionFromSales = 0;
            double OtherIncome = 0;
            double TotalCashInflow = 0;
            double PaymentToOverseasSupplier = 0;
            double PaymentToLocalSupplier = 0;
            double PersonalExpenses = 0;
            double CarriageTransportation = 0;
            double PropertyEquipmentExpenses = 0;
            double OtherPayment = 0;
            double TaxsPayment = 0;
            double TotalOperatingExpenses = 0;
            double NetOperatingCashFlow = 0;
            double PurchaseofFixedAssets = 0;
            double Investments = 0;
            double DisposalOfFixedAssets = 0;
            double DisposalOfInvestmentsOthers = 0;
            double LoanToIntercompany = 0;
            double InterestReceived = 0;
            double DividendReceived = 0;
            double DividendPaid = 0;
            double NetCashFlowFromInvesting = 0;
            double ProceedsOfBankLoans = 0;
            double RepaymentOfBankLoans = 0;
            double RepaymentOfTradeFinancing = 0;
            double PaymentOfInterests = 0;
            double NewCapitalConvertibleLoan = 0;
            double LoanFromIntercompany = 0;
            double RepaymentOfIntercompanyLoan = 0;
            double NetCashFlowFromFinancing = 0;
            double NetCashFlowSurplusDeficit = 0;

            //Cash Inflow From Operating Activities

            CollectionFromSales = GMSUtil.ToDouble(txtCFS8.Text);
            OtherIncome = GMSUtil.ToDouble(txtOI8.Text);
            TotalCashInflow = CollectionFromSales + OtherIncome;
            lblTCI8.Text = TotalCashInflow.ToString();

            //Less Cash Outflow from Operating Activities

            PaymentToOverseasSupplier = GMSUtil.ToDouble(txtPTOS8.Text);
            PaymentToLocalSupplier = GMSUtil.ToDouble(txtPTLS8.Text);
            PersonalExpenses = GMSUtil.ToDouble(txtPE8.Text);
            CarriageTransportation = GMSUtil.ToDouble(txtCT8.Text);
            PropertyEquipmentExpenses = GMSUtil.ToDouble(txtPEE8.Text);
            OtherPayment = GMSUtil.ToDouble(txtOP8.Text);
            TaxsPayment = GMSUtil.ToDouble(txtTP8.Text);
            TotalOperatingExpenses = PaymentToOverseasSupplier + PaymentToLocalSupplier + PersonalExpenses + CarriageTransportation + PropertyEquipmentExpenses + OtherPayment + TaxsPayment;
            NetOperatingCashFlow = TotalCashInflow - TotalOperatingExpenses;
            lblTOE8.Text = TotalOperatingExpenses.ToString();
            lblNPCF8.Text = NetOperatingCashFlow.ToString();

            //Less/Add Cash Flow from Investing Activities

            PurchaseofFixedAssets = GMSUtil.ToDouble(txtPOFA8.Text);
            Investments = GMSUtil.ToDouble(txtIO8.Text);
            DisposalOfFixedAssets = GMSUtil.ToDouble(txtDOFA8.Text);
            DisposalOfInvestmentsOthers = GMSUtil.ToDouble(txtDOIO8.Text);
            LoanToIntercompany = GMSUtil.ToDouble(txtLTI8.Text);
            InterestReceived = GMSUtil.ToDouble(txtIR8.Text);
            DividendReceived = GMSUtil.ToDouble(txtDR8.Text);
            DividendPaid = GMSUtil.ToDouble(txtDP8.Text);

            NetCashFlowFromInvesting = PurchaseofFixedAssets + Investments + DisposalOfFixedAssets + DisposalOfInvestmentsOthers + LoanToIntercompany + InterestReceived + DividendReceived + DividendPaid;
            lblNCFFI8.Text = NetCashFlowFromInvesting.ToString();

            //Less/Add Financing Activities
            ProceedsOfBankLoans = GMSUtil.ToDouble(txtPOBL8.Text);
            RepaymentOfBankLoans = GMSUtil.ToDouble(txtROBL8.Text);
            RepaymentOfTradeFinancing = GMSUtil.ToDouble(txtROTF8.Text);
            PaymentOfInterests = GMSUtil.ToDouble(txtPOI8.Text);
            NewCapitalConvertibleLoan = GMSUtil.ToDouble(txtNCCL8.Text);
            LoanFromIntercompany = GMSUtil.ToDouble(txtLFI8.Text);
            RepaymentOfIntercompanyLoan = GMSUtil.ToDouble(txtROIL8.Text);

            NetCashFlowFromFinancing = ProceedsOfBankLoans + RepaymentOfBankLoans + RepaymentOfTradeFinancing + PaymentOfInterests + NewCapitalConvertibleLoan + LoanFromIntercompany + RepaymentOfIntercompanyLoan;
            NetCashFlowSurplusDeficit = NetOperatingCashFlow + NetCashFlowFromInvesting + NetCashFlowFromFinancing;
            lblNCFFF8.Text = NetCashFlowFromFinancing.ToString();
            lblNCFSD8.Text = NetCashFlowSurplusDeficit.ToString();
            */


        }

    }
}
