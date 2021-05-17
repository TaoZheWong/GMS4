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
using System.Text;
using System.Threading;
using System.Globalization;

//using DTS;
using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;


namespace GMSWeb.Finance.BankFacilities
{
    public partial class BankUtilisation : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string currentLink = "CompanyFinance";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();

            }

            Master.setCurrentLink(currentLink);
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            49);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                           49);


            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!Page.IsPostBack)
            {
                //preload
                PopulateBankCOAList();
                this.ddlBankCOA.Items.Insert(0, new ListItem("-All-", "-1"));

                trnDateFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
                trnDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");


                PopulateBankAccountList();
                dgData.CurrentPageIndex = 0;
                LoadData();
            }

            string javaScript =
@"
<script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>
<script type=""text/javascript"">
function checkAll(checkItem, checkVal)
		{
				var frm = document.aspnetForm;
				for(i = 1; i < frm.length; i++) 
				{
					var elm = frm.elements[i];
					if( elm.type == 'checkbox' && elm.name.indexOf(checkItem) != -1 && elm.disabled != true )
					{
						elm.checked = checkVal;
					}
				}
		} 
		function DeselectMainCheckbox(checkbox)
		{
				document.getElementById('"; javaScript += dgData.ClientID; javaScript += @"').rows[0].cells[12].childNodes[0].checked = false;
				
		}
		
function changeCurrency(ddl1)
{
    var ddl2 = document.getElementById("""; javaScript += ddlBankAccount.ClientID; javaScript += @""");
    for( var i=0; i<ddl2.options.length; i++){
    ddl2.options[i].selected=(ddl2.options[i].text==ddl1.value);
    }
    document.getElementById(ddlNewCurrency).value = ddl2.value;
}
</script>
";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        private void ImportBankAccountData()
        {
            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            try
            {
                if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                        sc.Url = session.WebServiceAddress.Trim();
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                    ds = sc.GetBankAccountFromA21(session.CompanyId);
                }
                else if (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S")
                {
                    string query = "CALL \"AF_API_GET_SAP_BANK_INFO\" ()";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "BankCode", "bankcoa", "Currency", "Limit", "InterestRate", "Balance", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                }

                GMSGeneralDALC dacl = new GMSGeneralDALC();
                string strCurrency = "SGD";
                int bankid = 29;

                DataSet dsAmount = new DataSet();

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        try
                        {
                            strCurrency = getCurrency(ds.Tables[0].Rows[j]["bankcoa"].ToString());

                            GMSCore.Entity.Bank bank = new BankActivity().RetrieveBankByNumericCode(ds.Tables[0].Rows[j]["bankcoa"].ToString().Substring(2, 2).ToString());
                            if (bank != null)
                                bankid = GMSUtil.ToInt(bank.BankID);

                            GMSCore.Entity.BankAccount bankAccount = new BankActivity().RetrieveBankAccountByBankCOAByCompanyId(session.CompanyId, ds.Tables[0].Rows[j]["bankcoa"].ToString());

                            if (bankAccount == null)
                            {
                                GMSCore.Entity.BankAccount newbankAccount = new GMSCore.Entity.BankAccount();
                                newbankAccount.BankCOA = ds.Tables[0].Rows[j]["bankcoa"].ToString();
                                newbankAccount.BankID = GMSUtil.ToShort(bankid);
                                newbankAccount.Currency = strCurrency;
                                newbankAccount.IsMajorBank = false;
                                newbankAccount.ChequeFormatCode = "N.A";
                                newbankAccount.Limit = 0;
                                newbankAccount.InterestRate = "Nil";
                                newbankAccount.Balance = 0;
                                newbankAccount.DefaultCurrencyBalance = 0;
                                newbankAccount.MaturityDate = DateTime.Now;
                                newbankAccount.CreatedBy = session.UserId;
                                newbankAccount.CreatedDate = DateTime.Now;
                                newbankAccount.CoyID = session.CompanyId;
                                ResultType result = new BankActivity().CreateBankAccount(ref newbankAccount, session);
                            }
                        }
                        catch (Exception ex)
                        {
                            JScriptAlertMsg(ex.Message);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage("The connection to the server has failed! <br />For more information, please contact your System Administrator. ", MessagePanelControl.MessageEnumType.Alert);
            }
        }

        private string getCurrency(string bankCOA)
        {
            string strCurrency = "";
            if (bankCOA.Substring(4, 1) == "A")
                strCurrency = "AUD";
            else if (bankCOA.Substring(4, 1) == "P")
                strCurrency = "PHP";
            else if (bankCOA.Substring(4, 1) == "C")
                strCurrency = "CNY";
            else if (bankCOA.Substring(4, 1) == "E")
                strCurrency = "EUR";
            else if (bankCOA.Substring(4, 1) == "B")
                strCurrency = "GBP";
            else if (bankCOA.Substring(4, 1) == "H")
                strCurrency = "HKD";
            else if (bankCOA.Substring(4, 1) == "I")
                strCurrency = "IDR";
            else if (bankCOA.Substring(4, 1) == "J")
                strCurrency = "JPY";
            else if (bankCOA.Substring(4, 1) == "M")
                strCurrency = "MYR";
            else if (bankCOA.Substring(4, 1) == "S")
                strCurrency = "SGD";
            else if (bankCOA.Substring(4, 1) == "T")
                strCurrency = "THB";
            else if (bankCOA.Substring(4, 1) == "U")
                strCurrency = "USD";
            else if (bankCOA.Substring(4, 2) == "KW")
                strCurrency = "NOK";
            else if (bankCOA.Substring(4, 2) == "TW")
                strCurrency = "NTD";
            else if (bankCOA.Substring(4, 2) == "SK")
                strCurrency = "SEK";
            else if (bankCOA.Substring(4, 2) == "SF")
                strCurrency = "SFR";
            else if (bankCOA.Substring(4, 2) == "CD")
                strCurrency = "CAD";

            return strCurrency;
        }

        private void ImportData(DateTime tDateFrom, DateTime tDateTo)
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC dacl = new GMSGeneralDALC();

            DataSet ds = new DataSet();
            try
            {
                if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                {
                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    {
                        sc.Url = session.WebServiceAddress.Trim();
                    }
                    else
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                    ds = sc.GetBankUtilisationFromA21(session.CompanyId, tDateFrom, tDateTo);
                }
                else if (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S")
                {
                    string from = tDateFrom.ToString("yyyy-MM-dd");
                    string to = tDateTo.ToString("yyyy-MM-dd");
                    string query = "CALL \"AF_API_GET_SAP_BANK_UTILISATION\" ('" + from + "','" + to + "')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "trntype", "cname", "mode", "trnno", "trndate", "bankcoa", "ChequeDate", "currency", "Amount", "acctcode", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                }

                string strCurrency = "";
                double debitamount = 0;
                double creditamount = 0;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                            strCurrency = getCurrency(ds.Tables[0].Rows[j]["bankcoa"].ToString());
                        else
                            strCurrency = ds.Tables[0].Rows[j]["currency"].ToString();
                        try
                        {
                            GMSCore.Entity.BankAccount bankAccount = new BankActivity().RetrieveBankAccountByBankCOAByCompanyId(session.CompanyId, ds.Tables[0].Rows[j]["bankcoa"].ToString());
                            if (bankAccount != null)
                            {
                                GMSCore.Entity.BankUtilisation existingBankUtilisation = new BankActivity().RetrieveBankUtilisationByCoyIDTransactionNo(session.CompanyId, ds.Tables[0].Rows[j]["trnno"].ToString(), ds.Tables[0].Rows[j]["trntype"].ToString(), (short)bankAccount.COAID);
                                if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                                {
                                    if (ds.Tables[0].Rows[j]["currency"].ToString() != strCurrency)
                                    {
                                        debitamount = (double)ds.Tables[0].Rows[j]["debitamount"] * (double)ds.Tables[0].Rows[j]["exchangerate"];
                                        creditamount = (double)ds.Tables[0].Rows[j]["creditamount"] * (double)ds.Tables[0].Rows[j]["exchangerate"];
                                    }
                                    else
                                    {
                                        debitamount = (double)ds.Tables[0].Rows[j]["debitamount"];
                                        creditamount = (double)ds.Tables[0].Rows[j]["creditamount"];
                                    }
                                }

                                if (existingBankUtilisation != null)
                                {
                                    if (ds.Tables[0].Rows[j]["mode"] == DBNull.Value || ds.Tables[0].Rows[j]["mode"].ToString() == "")
                                        existingBankUtilisation.Mode = "nil";
                                    else if (ds.Tables[0].Rows[j]["mode"].ToString().Length > 10)
                                        existingBankUtilisation.Mode = ds.Tables[0].Rows[j]["mode"].ToString().Substring(0, 9);
                                    else
                                        existingBankUtilisation.Mode = ds.Tables[0].Rows[j]["mode"].ToString();

                                    existingBankUtilisation.AccountCode = ds.Tables[0].Rows[j]["acctcode"].ToString();

                                    try
                                    {
                                        if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                                        {
                                            if (ds.Tables[0].Rows[j]["trntype"].ToString() == "PV")
                                            {
                                                if (creditamount > 0)
                                                    existingBankUtilisation.Amount = creditamount;
                                                else
                                                    existingBankUtilisation.Amount = debitamount * -1;
                                            }
                                            else if (ds.Tables[0].Rows[j]["trntype"].ToString() == "RR")
                                            {
                                                if (debitamount > 0)
                                                    existingBankUtilisation.Amount = debitamount;
                                                else
                                                    existingBankUtilisation.Amount = creditamount * -1;
                                            }
                                            else if (ds.Tables[0].Rows[j]["trntype"].ToString() == "GJ")
                                            {
                                                if (debitamount > 0)
                                                    existingBankUtilisation.Amount = debitamount;
                                                else
                                                    existingBankUtilisation.Amount = creditamount * -1;
                                            }
                                        }
                                        else if (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S")
                                        {
                                            double tmpAmt = 0;
                                            if (Double.TryParse(ds.Tables[0].Rows[j]["Amount"].ToString(), out tmpAmt))
                                                existingBankUtilisation.Amount = tmpAmt;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        this.PageMsgPanel.ShowMessage("Invalid amount!", MessagePanelControl.MessageEnumType.Alert);
                                        return;
                                    }

                                    try
                                    {
                                        existingBankUtilisation.TransactionDate = DateTime.Parse(ds.Tables[0].Rows[j]["trndate"].ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        this.PageMsgPanel.ShowMessage("Invalid Transaction Date!", MessagePanelControl.MessageEnumType.Alert);
                                        return;
                                    }

                                    existingBankUtilisation.Currency = strCurrency;
                                    existingBankUtilisation.BankCOAID = (short)bankAccount.COAID;

                                    try
                                    {
                                        ResultType result = new BankActivity().UpdateBankUtilisation(ref existingBankUtilisation, session);

                                    }
                                    catch (Exception ex)
                                    {
                                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                                        return;
                                    }

                                }
                                else if (existingBankUtilisation == null)
                                {

                                    GMSCore.Entity.BankUtilisation bu = new GMSCore.Entity.BankUtilisation();
                                    bu.Type = ds.Tables[0].Rows[j]["trntype"].ToString().Substring(0, 2);

                                    if (ds.Tables[0].Rows[j]["cname"].ToString().Length > 100)
                                        bu.Name = ds.Tables[0].Rows[j]["cname"].ToString().Substring(0, 99);
                                    else
                                        bu.Name = ds.Tables[0].Rows[j]["cname"].ToString();

                                    if (ds.Tables[0].Rows[j]["mode"] == DBNull.Value || ds.Tables[0].Rows[j]["mode"].ToString() == "")
                                        bu.Mode = "nil";
                                    else if (ds.Tables[0].Rows[j]["mode"].ToString().Length > 10)
                                        bu.Mode = ds.Tables[0].Rows[j]["mode"].ToString().Substring(0, 9);
                                    else
                                        bu.Mode = ds.Tables[0].Rows[j]["mode"].ToString();

                                    bu.BankCOAID = (short)bankAccount.COAID;

                                    try
                                    {
                                        if (DateTime.Parse(ds.Tables[0].Rows[j]["trndate"].ToString()) != GMSCoreBase.DEFAULT_NO_DATE)
                                            bu.ChequeDate = DateTime.Parse(ds.Tables[0].Rows[j]["trndate"].ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        this.PageMsgPanel.ShowMessage("Invalid Cheque Date!", MessagePanelControl.MessageEnumType.Alert);
                                        return;
                                    }

                                    bu.Currency = strCurrency;

                                    try
                                    {
                                        if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                                        {
                                            if (ds.Tables[0].Rows[j]["trntype"].ToString() == "PV")
                                            {
                                                if (creditamount > 0)
                                                    bu.Amount = creditamount;
                                                else
                                                    bu.Amount = debitamount * -1;
                                            }
                                            else if (ds.Tables[0].Rows[j]["trntype"].ToString() == "RR")
                                            {
                                                if (debitamount > 0)
                                                    bu.Amount = debitamount;
                                                else
                                                    bu.Amount = creditamount * -1;
                                            }
                                            else if (ds.Tables[0].Rows[j]["trntype"].ToString() == "GJ")
                                            {
                                                if (debitamount > 0)
                                                    bu.Amount = debitamount;
                                                else
                                                    bu.Amount = creditamount * -1;
                                            }
                                        }
                                        else if (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S")
                                        {
                                            double tmpAmt = 0;
                                            if (Double.TryParse(ds.Tables[0].Rows[j]["Amount"].ToString(), out tmpAmt))
                                                bu.Amount = tmpAmt;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        this.PageMsgPanel.ShowMessage("Invalid amount!", MessagePanelControl.MessageEnumType.Alert);
                                        return;
                                    }
                                    bu.Marking1 = true;
                                    bu.Marking2 = true;
                                    bu.TransactionNo = ds.Tables[0].Rows[j]["trnno"].ToString();
                                    bu.CreatedBy = session.UserId;
                                    bu.CreatedDate = DateTime.Now;
                                    bu.CoyID = session.CompanyId;
                                    bu.AccountCode = ds.Tables[0].Rows[j]["acctcode"].ToString();

                                    try
                                    {
                                        bu.TransactionDate = DateTime.Parse(ds.Tables[0].Rows[j]["trndate"].ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        this.PageMsgPanel.ShowMessage("Invalid Transaction Date!", MessagePanelControl.MessageEnumType.Alert);
                                        return;
                                    }

                                    if (bu.Name != "")
                                    {
                                        //update customer list
                                        IList<A21Account> lstBRP = new SystemDataActivity().RetrieveAllCustomerAccountsListByPrefixByCompanyIDSortByAccountName(bu.Name, session.CompanyId);
                                        if (lstBRP.Count == 0)
                                        {
                                            BankReceiverPayer bankReceiverPayer = new BankReceiverPayerActivity().RetrieveBankReceiverPayerByNameCompanyId(bu.Name, session.CompanyId);
                                            if (bankReceiverPayer == null)
                                            {
                                                bankReceiverPayer = new BankReceiverPayer();
                                                bankReceiverPayer.CoyID = bu.CoyID;
                                                bankReceiverPayer.Name = bu.Name;
                                                new BankReceiverPayerActivity().CreateBankReceiverPayer(ref bankReceiverPayer, session);
                                            }
                                        }
                                    }

                                    //create bank utilisation
                                    try
                                    {
                                        ResultType result = new BankActivity().CreateBankUtilisation(ref bu, session);
                                    }
                                    catch (Exception ex)
                                    {
                                        this.PageMsgPanel.ShowMessage("The connection to the server has failed! <br />For more information, please contact your System Administrator. ", MessagePanelControl.MessageEnumType.Alert);
                                        return;
                                    }


                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            JScriptAlertMsg(ex.Message);
                        }

                    }

                }

                //ModalPopupExtender1.Hide();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "progress_stop", "progress_stop();", true);
                JScriptAlertMsg("Finished importing.");
                LoadData();

            }
            catch (Exception ex)
            {
                //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                this.PageMsgPanel.ShowMessage("The connection to the server has failed! <br />For more information, please contact your System Administrator. ", MessagePanelControl.MessageEnumType.Alert);

            }


        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            IList<GMSCore.Entity.BankUtilisation> lstBU = null;
            short companyID = session.CompanyId;
            string name = string.IsNullOrEmpty(txtSearchName.Text) ? "%" : "%" + txtSearchName.Text.Trim() + "%";
            string mode = string.IsNullOrEmpty(txtSearchMode.Text) ? "%" : "%" + txtSearchMode.Text.Trim() + "%";
            string trnNo = string.IsNullOrEmpty(txtSearchTrnNo.Text) ? "%" : "%" + txtSearchTrnNo.Text.Trim() + "%";
            DateTime tDateFrom = (GMSUtil.ToDate(trnDateFrom.Text) == GMSCore.GMSCoreBase.DEFAULT_NO_DATE) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : GMSUtil.ToDate(trnDateFrom.Text.ToString());
            DateTime tDateTo = (GMSUtil.ToDate(trnDateTo.Text) == GMSCore.GMSCoreBase.DEFAULT_NO_DATE) ? DateTime.Now : GMSUtil.ToDate(trnDateTo.Text.ToString());
            if (string.IsNullOrEmpty(trnDateFrom.Text)) trnDateFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
            if (string.IsNullOrEmpty(trnDateTo.Text)) trnDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime cDateFrom = GMSCoreBase.DEFAULT_NO_DATE.AddYears(3);
            DateTime cDateTo = GMSCoreBase.DEFAULT_NO_DATE.AddYears(3);

            if (GMSUtil.ToDate(chequeDateTo.Text) != GMSCore.GMSCoreBase.DEFAULT_NO_DATE || GMSUtil.ToDate(chequeDateFrom.Text) != GMSCore.GMSCoreBase.DEFAULT_NO_DATE)
            {
                cDateFrom = GMSUtil.ToDate(chequeDateFrom.Text.ToString());
                cDateTo = (GMSUtil.ToDate(chequeDateTo.Text) == GMSCore.GMSCoreBase.DEFAULT_NO_DATE) ? DateTime.Now.AddYears(2) : GMSUtil.ToDate(chequeDateTo.Text.ToString());
            }
            short bankCOAID = GMSUtil.ToShort(ddlBankCOA.SelectedValue);
            string type = ddlType.SelectedValue;
            try
            {
                lstBU = new SystemDataActivity().RetrieveBankUtilisationListByCompanyIDByNameByModeByTransactionNoByTransactionDateByChequeDateByBankCOAIDByTypeSortByTransactionDateByTransactionNo(companyID, name, mode, trnNo, tDateFrom, tDateTo, cDateFrom, cDateTo, GMSUtil.ToShort(bankCOAID), type);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lstBU != null && lstBU.Count > 0)
            {
                if (endIndex < lstBU.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstBU.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstBU.Count.ToString() + " " + "of" + " " + lstBU.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = lstBU;
            this.dgData.DataBind();
        }
        #endregion

        private void PopulateBankCOAList()
        {
            SystemDataActivity sDataActivity = new SystemDataActivity();
            LogSession session = base.GetSessionInfo();

            // fill in course dropdown list
            IList<GMSCore.Entity.BankAccount> lstBankAccount = null;
            lstBankAccount = sDataActivity.RetrieveAllBankAccountListByCompanyIDSortByBankAccount(session.CompanyId);
            ddlBankCOA.DataSource = lstBankAccount;
            ddlBankCOA.DataBind();
        }

        private void PopulateBankAccountList()
        {
            SystemDataActivity sDataActivity = new SystemDataActivity();
            LogSession session = base.GetSessionInfo();

            // fill in course dropdown list
            IList<GMSCore.Entity.BankAccount> lstBankAccount = null;
            lstBankAccount = sDataActivity.RetrieveAllBankAccountListByCompanyIDSortByBankAccount(session.CompanyId);
            ddlBankAccount.DataSource = lstBankAccount;
            ddlBankAccount.DataBind();
        }

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        #region dgData_ItemDataBound
        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddlEditCurrency = (DropDownList)e.Item.FindControl("ddlEditCurrency");
                DropDownList ddlEditType = (DropDownList)e.Item.FindControl("ddlEditType");
                DropDownList ddlEditBankCOAID = (DropDownList)e.Item.FindControl("ddlEditBankCOAID");

                if (ddlEditCurrency != null)
                {
                    GMSCore.Entity.BankUtilisation bu = (GMSCore.Entity.BankUtilisation)e.Item.DataItem;
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    if (bu != null)
                    {
                        // fill in Currency dropdown list
                        IList<GMSCore.Entity.Currency> lstCurrency = null;
                        lstCurrency = sDataActivity.RetrieveAllCurrencyListSortByCode();
                        ddlEditCurrency.DataSource = lstCurrency;
                        ddlEditCurrency.DataBind();
                        ddlEditCurrency.SelectedValue = bu.Currency;
                    }
                }

                if (ddlEditType != null)
                {
                    GMSCore.Entity.BankUtilisation bu = (GMSCore.Entity.BankUtilisation)e.Item.DataItem;
                    if (bu != null)
                    {
                        ddlEditType.SelectedValue = bu.Type;
                    }
                }

                if (ddlEditBankCOAID != null)
                {
                    GMSCore.Entity.BankUtilisation bu = (GMSCore.Entity.BankUtilisation)e.Item.DataItem;
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    if (bu != null)
                    {
                        // fill in Bank COA dropdown list
                        IList<GMSCore.Entity.BankAccount> lstBankAccount = null;
                        lstBankAccount = sDataActivity.RetrieveAllBankAccountListByCompanyIDSortByBankAccount(session.CompanyId);
                        ddlEditBankCOAID.DataSource = lstBankAccount;
                        ddlEditBankCOAID.DataBind();
                        ddlEditBankCOAID.SelectedValue = bu.BankCOAID.ToString();
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewCurrency = (DropDownList)e.Item.FindControl("ddlNewCurrency");
                DropDownList ddlNewBankCOAID = (DropDownList)e.Item.FindControl("ddlNewBankCOAID");
                TextBox newTransactionDate = (TextBox)e.Item.FindControl("newTransactionDate");
                TextBox newChequeDate = (TextBox)e.Item.FindControl("newChequeDate");

                if (ddlNewCurrency != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in currency dropdown list
                    IList<GMSCore.Entity.Currency> lstCurrency = null;
                    lstCurrency = sDataActivity.RetrieveAllCurrencyListSortByCode();
                    ddlNewCurrency.DataSource = lstCurrency;
                    ddlNewCurrency.DataBind();
                    ddlNewCurrency.SelectedValue = "SGD";
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlNewCurrency, ddlNewCurrency.GetType(), "script1", "<script type=\"text/javascript\"> var ddlNewCurrency = '" + ddlNewCurrency.ClientID + "';</script>", false);
                }

                if (ddlNewBankCOAID != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in currency dropdown list
                    IList<GMSCore.Entity.BankAccount> lstBankAccount = null;
                    lstBankAccount = sDataActivity.RetrieveAllBankAccountListByCompanyIDSortByBankAccount(session.CompanyId);
                    ddlNewBankCOAID.DataSource = lstBankAccount;
                    ddlNewBankCOAID.DataBind();
                    ddlNewBankCOAID.SelectedValue = "1";
                }

                if (newTransactionDate != null)
                {
                    newTransactionDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }

                if (newChequeDate != null)
                {
                    newChequeDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");

            }
        }
        #endregion

        #region dgData_EditCommand
        protected void dgData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }

            HtmlInputHidden hidTransactionNo = (HtmlInputHidden)e.Item.FindControl("hidTransactionNo");
            HtmlInputHidden hidTransactionType = (HtmlInputHidden)e.Item.FindControl("hidTransactionType");
            HtmlInputHidden hidBankCoaID = (HtmlInputHidden)e.Item.FindControl("hidBankCoaID");

            GMSCore.Entity.BankUtilisation bUtilisation = new BankActivity().RetrieveBankUtilisationByCoyIDTransactionNo(session.CompanyId, hidTransactionNo.Value, hidTransactionType.Value, GMSUtil.ToShort(hidBankCoaID.Value));
            if (bUtilisation.CreatedDate.Date != DateTime.Now.Date)
            {
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                51);
                if (uAccess == null)
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("alert('You can only edit today\\'s transactions. Please ask your supervisor to eidt past transactions.');");
                    str.Append("</script>");
                    ClientScript.RegisterStartupScript(typeof(string), "Unauthorised",
                     str.ToString(), false);
                    this.dgData.EditItemIndex = -1;
                    LoadData();
                }
                else
                {
                    this.dgData.EditItemIndex = e.Item.ItemIndex;
                    LoadData();
                }
            }
            else
            {
                this.dgData.EditItemIndex = e.Item.ItemIndex;
                LoadData();
            }


        }
        #endregion

        #region dgData_CancelCommand
        protected void dgData_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgData_UpdateCommand
        protected void dgData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }

            HtmlInputHidden hidTransactionNo = (HtmlInputHidden)e.Item.FindControl("hidTransactionNo");
            HtmlInputHidden hidTransactionType = (HtmlInputHidden)e.Item.FindControl("hidTransactionType");
            HtmlInputHidden hidBankCoaID = (HtmlInputHidden)e.Item.FindControl("hidBankCoaID");

            GMSCore.Entity.BankUtilisation bUtilisation = new BankActivity().RetrieveBankUtilisationByCoyIDTransactionNo(session.CompanyId, hidTransactionNo.Value, hidTransactionType.Value, GMSUtil.ToShort(hidBankCoaID.Value));
            if (bUtilisation.CreatedDate.Date != DateTime.Now.Date)
            {
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                51);
                if (uAccess == null)
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("alert('You can only edit today\\'s transactions. Please ask your supervisor to eidt past transactions.');");
                    str.Append("</script>");
                    Response.Write(str.ToString());
                    Response.Redirect(base.UnauthorizedPage("CompanyFinance"));
                }
            }

            DropDownList ddlEditType = (DropDownList)e.Item.FindControl("ddlEditType");
            TextBox txtEditName = (TextBox)e.Item.FindControl("txtEditName");
            TextBox txtEditMode = (TextBox)e.Item.FindControl("txtEditMode");
            DropDownList ddlEditBankCOAID = (DropDownList)e.Item.FindControl("ddlEditBankCOAID");
            TextBox editChequeDate = (TextBox)e.Item.FindControl("editChequeDate");
            DropDownList ddlEditCurrency = (DropDownList)e.Item.FindControl("ddlEditCurrency");
            TextBox txtEditAmount = (TextBox)e.Item.FindControl("txtEditAmount");
            CheckBox chkEditMarking1 = (CheckBox)e.Item.FindControl("chkEditMarking1");
            CheckBox chkEditMarking2 = (CheckBox)e.Item.FindControl("chkEditMarking2");

            TextBox editTransactionDate = (TextBox)e.Item.FindControl("editTransactionDate");

            if (ddlEditType != null && txtEditName != null && !string.IsNullOrEmpty(txtEditName.Text) && txtEditMode != null && !string.IsNullOrEmpty(txtEditMode.Text) &&
                ddlEditBankCOAID != null && editChequeDate != null && ddlEditCurrency != null && txtEditAmount != null && !string.IsNullOrEmpty(txtEditAmount.Text) &&
                chkEditMarking1 != null && chkEditMarking2 != null && hidTransactionNo != null && editTransactionDate != null)
            {

                GMSCore.Entity.BankAccount bankAccount = new BankActivity().RetrieveBankAccountByCOAID(bUtilisation.BankCOAID);

                //When update, remove transaction amount from the bank balance first and then later add it back to the balance
                /*if (bUtilisation.Type == "PV")
                {
                    bankAccount.Balance += bUtilisation.Amount;
                }
                else if (bUtilisation.Type == "RR")
                {
                    bankAccount.Balance -= bUtilisation.Amount;
                }
                new BankActivity().UpdateBankAccount(ref bankAccount, session);
                */


                //bUtilisation.Type = ddlEditType.SelectedValue;
                bUtilisation.Name = txtEditName.Text.Trim();
                //bUtilisation.Mode = txtEditMode.Text.Trim();
                //bUtilisation.BankCOAID = GMSUtil.ToShort(ddlEditBankCOAID.SelectedValue);
                if (GMSUtil.ToDate(editChequeDate.Text.Trim()) != GMSCoreBase.DEFAULT_NO_DATE)
                    bUtilisation.ChequeDate = GMSUtil.ToDate(editChequeDate.Text.Trim());
                //bUtilisation.Currency = ddlEditCurrency.SelectedValue;
                //bUtilisation.Amount = GMSUtil.ToDouble(txtEditAmount.Text.Trim());
                bUtilisation.Marking1 = chkEditMarking1.Checked;
                bUtilisation.Marking2 = chkEditMarking2.Checked;
                bUtilisation.ModifiedBy = session.UserId;
                bUtilisation.ModifiedDate = DateTime.Now;
                //bUtilisation.TransactionDate = GMSUtil.ToDate(editTransactionDate.Text.Trim());

                //Add amount back to the bank balance
                /*
                bankAccount = new BankActivity().RetrieveBankAccountByCOAID(GMSUtil.ToShort(bUtilisation.BankCOAID));
                if (bUtilisation.Type == "PV")
                {
                    bankAccount.Balance -= bUtilisation.Amount;
                }
                else if (bUtilisation.Type == "RR")
                {
                    bankAccount.Balance += bUtilisation.Amount;
                }
                */

                //update customer list
                IList<A21Account> lstBRP = new SystemDataActivity().RetrieveAllCustomerAccountsListByPrefixByCompanyIDSortByAccountName(bUtilisation.Name, session.CompanyId);
                if (lstBRP.Count == 0)
                {
                    BankReceiverPayer bankReceiverPayer = new BankReceiverPayerActivity().RetrieveBankReceiverPayerByNameCompanyId(bUtilisation.Name, session.CompanyId);
                    if (bankReceiverPayer == null)
                    {
                        bankReceiverPayer = new BankReceiverPayer();
                        bankReceiverPayer.CoyID = bUtilisation.CoyID;
                        bankReceiverPayer.Name = bUtilisation.Name;
                        new BankReceiverPayerActivity().CreateBankReceiverPayer(ref bankReceiverPayer, session);
                    }
                }


                try
                {
                    //new BankActivity().UpdateBankAccount(ref bankAccount, session);
                    ResultType result = new BankActivity().UpdateBankUtilisation(ref bUtilisation, session);

                    switch (result)
                    {
                        case ResultType.Ok:
                            this.dgData.EditItemIndex = -1;
                            LoadData();
                            break;
                        default:
                            this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                            return;
                    }
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }
        }
        #endregion

        #region dgData_CreateCommand
        protected void dgData_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            /*
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();

                DropDownList ddlNewType = (DropDownList)e.Item.FindControl("ddlNewType");
                TextBox txtNewName = (TextBox)e.Item.FindControl("txtNewName");
                TextBox txtNewMode = (TextBox)e.Item.FindControl("txtNewMode");
                DropDownList ddlNewBankCOAID = (DropDownList)e.Item.FindControl("ddlNewBankCOAID");
                TextBox newChequeDate = (TextBox)e.Item.FindControl("newChequeDate");
                DropDownList ddlNewCurrency = (DropDownList)e.Item.FindControl("ddlNewCurrency");
                TextBox txtNewAmount = (TextBox)e.Item.FindControl("txtNewAmount");
                CheckBox chkNewMarking1 = (CheckBox)e.Item.FindControl("chkNewMarking1");
                CheckBox chkNewMarking2 = (CheckBox)e.Item.FindControl("chkNewMarking2");
                TextBox txtNewTransactionNo = (TextBox)e.Item.FindControl("txtNewTransactionNo");
                TextBox newTransactionDate = (TextBox)e.Item.FindControl("newTransactionDate");

                try
                {
                    // check if newly inserted transaction already exist
                    GMSCore.Entity.BankUtilisation existingBankUtilisation = new BankActivity().RetrieveBankUtilisationByCoyIDTransactionNo(session.CompanyId, txtNewTransactionNo.Text.Trim());

                    if (existingBankUtilisation != null)
                    {
                        this.PageMsgPanel.ShowMessage("Processing error of type : This transaction no already been used.", MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    if (txtNewTransactionNo.Text.Trim() == "")
                    {
                        this.PageMsgPanel.ShowMessage("Processing error of type : Please enter transaction number.", MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    if (newTransactionDate.Text.Trim() == "")
                    {
                        this.PageMsgPanel.ShowMessage("Processing error of type : Please enter transaction date.", MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    if (txtNewName.Text.Trim() == "")
                    {
                        this.PageMsgPanel.ShowMessage("Processing error of type : Please enter name.", MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    if (txtNewMode.Text.Trim() == "")
                    {
                        this.PageMsgPanel.ShowMessage("Processing error of type : Please enter mode.", MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    if (txtNewAmount.Text.Trim() == "")
                    {
                        this.PageMsgPanel.ShowMessage("Processing error of type : Please enter amount.", MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    GMSCore.Entity.BankUtilisation bu = new GMSCore.Entity.BankUtilisation();
                    bu.Type = ddlNewType.SelectedValue;
                    bu.Name = txtNewName.Text.Trim();
                    bu.Mode = txtNewMode.Text.Trim();
                    bu.BankCOAID = GMSUtil.ToShort(ddlNewBankCOAID.SelectedValue);
                    try
                    {
                        if (DateTime.Parse(newChequeDate.Text.Trim()) != GMSCoreBase.DEFAULT_NO_DATE)
                            bu.ChequeDate = DateTime.Parse(newChequeDate.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage("Invalid Cheque Date!", MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                    bu.Currency = ddlNewCurrency.SelectedValue;
                    try
                    {
                        bu.Amount = Double.Parse(txtNewAmount.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage("Invalid amount!", MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                    bu.Marking1 = chkNewMarking1.Checked;
                    bu.Marking2 = chkNewMarking2.Checked;
                    bu.TransactionNo = txtNewTransactionNo.Text.Trim();
                    bu.CreatedBy = session.UserId;
                    bu.CreatedDate = DateTime.Now;
                    bu.CoyID = session.CompanyId;
                    try
                    {
                        bu.TransactionDate = DateTime.Parse(newTransactionDate.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage("Invalid Transaction Date!", MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    //update customer list
                    IList<A21Account> lstBRP = new SystemDataActivity().RetrieveAllCustomerAccountsListByPrefixByCompanyIDSortByAccountName(bu.Name, session.CompanyId);
                    if (lstBRP.Count == 0)
                    {
                        BankReceiverPayer bankReceiverPayer = new BankReceiverPayerActivity().RetrieveBankReceiverPayerByNameCompanyId(bu.Name, session.CompanyId);
                        if (bankReceiverPayer == null)
                        {
                            bankReceiverPayer = new BankReceiverPayer();
                            bankReceiverPayer.CoyID = bu.CoyID;
                            bankReceiverPayer.Name = bu.Name;
                            new BankReceiverPayerActivity().CreateBankReceiverPayer(ref bankReceiverPayer, session);
                        }
                    }

                    //update bank balance
                    GMSCore.Entity.BankAccount bankAccount = new BankActivity().RetrieveBankAccountByCOAID(GMSUtil.ToShort(ddlNewBankCOAID.SelectedValue));
                    if (bu.Type == "R")
                    {
                        bankAccount.Balance = bankAccount.Balance + bu.Amount;
                    }
                    else if (bu.Type == "P")
                    {
                        bankAccount.Balance = bankAccount.Balance - bu.Amount;
                    }
                    new BankActivity().UpdateBankAccount(ref bankAccount, session);

                    //create bank utilisation
                    ResultType result = new BankActivity().CreateBankUtilisation(ref bu, session);

                    switch (result)
                    {
                        case ResultType.Ok:
                            LoadData();
                            break;
                        default:
                            this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                            return;
                    }
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }
             */
        }
        #endregion

        #region dgData_DeleteCommand
        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                51);
            if (uAccess == null)
            {
                HtmlInputHidden hidTransactionNo = (HtmlInputHidden)e.Item.FindControl("hidTransactionNo");
                HtmlInputHidden hidTransactionType = (HtmlInputHidden)e.Item.FindControl("hidTransactionType");
                HtmlInputHidden hidBankCoaID = (HtmlInputHidden)e.Item.FindControl("hidBankCoaID");

                GMSCore.Entity.BankUtilisation bUtilisation = new BankActivity().RetrieveBankUtilisationByCoyIDTransactionNo(session.CompanyId, hidTransactionNo.Value, hidTransactionType.Value, GMSUtil.ToShort(hidBankCoaID.Value));
                if (bUtilisation.CreatedDate.Date != DateTime.Now.Date)
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<script language='javascript'>");
                    str.Append("alert('You can only delete today\\'s transactions. Please ask your supervisor to delete past transactions.');");
                    str.Append("</script>");
                    ClientScript.RegisterStartupScript(typeof(string), "Unauthorised",
                     str.ToString(), false);
                    this.dgData.EditItemIndex = -1;
                    LoadData();
                }
                else
                {
                    if (e.CommandName == "Delete")
                    {
                        BankActivity bActivity = new BankActivity();
                        try
                        {
                            GMSCore.Entity.BankUtilisation bu = bActivity.RetrieveBankUtilisationByCoyIDTransactionNo(session.CompanyId, hidTransactionNo.Value, hidTransactionType.Value, GMSUtil.ToShort(hidBankCoaID.Value));
                            BankAccount bankAccount = bActivity.RetrieveBankAccountByCOAID(bu.BankCOAID);

                            if (bu.Type == "PV")
                            {
                                bankAccount.Balance += bu.Amount;
                            }
                            else if (bu.Type == "RR")
                            {
                                bankAccount.Balance -= bu.Amount;
                            }

                            bActivity.UpdateBankAccount(ref bankAccount, session);
                            ResultType result = bActivity.DeleteBankUtilisation(session.CompanyId, hidTransactionNo.Value, session, hidTransactionType.Value, GMSUtil.ToShort(hidBankCoaID.Value));

                            switch (result)
                            {
                                case ResultType.Ok:
                                    this.dgData.EditItemIndex = -1;
                                    this.dgData.CurrentPageIndex = 0;
                                    LoadData();
                                    break;
                                default:
                                    this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                    return;
                            }
                        }
                        catch (SqlException exSql)
                        {
                            if (exSql.Number == 547)
                            {
                                this.PageMsgPanel.ShowMessage("This transaction cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                                LoadData();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                            LoadData();
                            return;
                        }
                    }
                }
            }
            else
            {
                if (e.CommandName == "Delete")
                {
                    BankActivity bActivity = new BankActivity();
                    HtmlInputHidden hidTransactionNo = (HtmlInputHidden)e.Item.FindControl("hidTransactionNo");
                    HtmlInputHidden hidTransactionType = (HtmlInputHidden)e.Item.FindControl("hidTransactionType");
                    HtmlInputHidden hidBankCoaID = (HtmlInputHidden)e.Item.FindControl("hidBankCoaID");

                    try
                    {
                        GMSCore.Entity.BankUtilisation bu = bActivity.RetrieveBankUtilisationByCoyIDTransactionNo(session.CompanyId, hidTransactionNo.Value, hidTransactionType.Value, GMSUtil.ToShort(hidBankCoaID.Value));
                        BankAccount bankAccount = bActivity.RetrieveBankAccountByCOAID(bu.BankCOAID);

                        if (bu.Type == "PV")
                        {
                            bankAccount.Balance += bu.Amount;
                        }
                        else if (bu.Type == "RR")
                        {
                            bankAccount.Balance -= bu.Amount;
                        }

                        bActivity.UpdateBankAccount(ref bankAccount, session);
                        ResultType result = bActivity.DeleteBankUtilisation(session.CompanyId, hidTransactionNo.Value, session, hidTransactionType.Value, GMSUtil.ToShort(hidBankCoaID.Value));

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgData.EditItemIndex = -1;
                                this.dgData.CurrentPageIndex = 0;
                                LoadData();
                                break;
                            default:
                                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                return;
                        }
                    }
                    catch (SqlException exSql)
                    {
                        if (exSql.Number == 547)
                        {
                            this.PageMsgPanel.ShowMessage("This transaction cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                            LoadData();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        LoadData();
                        return;
                    }
                }
            }
        }
        #endregion

        #region printReport
        protected void printReport(object sender, EventArgs e)
        {
            LinkButton lnkPrintSelected = (LinkButton)sender;
            System.Collections.Hashtable hTable = new System.Collections.Hashtable();

            for (int i = 0; i < this.dgData.Items.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)this.dgData.Items[i].FindControl("chkPrint");
                HtmlInputHidden hidTrnNo = (HtmlInputHidden)this.dgData.Items[i].FindControl("hidBUID");
                HtmlInputHidden hidBankCOA = (HtmlInputHidden)this.dgData.Items[i].FindControl("hidBankCOA");
                HtmlInputHidden hidChequeFormatCode = (HtmlInputHidden)this.dgData.Items[i].FindControl("hidChequeFormatCode");

                if (chkSelect != null && hidTrnNo != null)
                {
                    if (chkSelect.Checked)
                    {
                        string bankCOA = hidBankCOA.Value;
                        int index = hidBankCOA.Value.IndexOf(" - ");
                        if (index != -1)
                            bankCOA = hidBankCOA.Value.Substring(index + 3);
                        index = bankCOA.IndexOf(" - ");
                        if (index != -1)
                            bankCOA = bankCOA.Substring(0, index + 1);
                        bankCOA = bankCOA + "_" + hidChequeFormatCode.Value;
                        if (hTable.ContainsKey(bankCOA))
                        {
                            string tempStr = (string)hTable[bankCOA];
                            tempStr += hidTrnNo.Value + ",";
                            hTable.Remove(bankCOA);
                            hTable.Add(bankCOA, tempStr);
                        }
                        else
                        {
                            hTable.Add(bankCOA, hidTrnNo.Value + ",");
                        }
                    }
                }
            }

            IDictionaryEnumerator en = hTable.GetEnumerator();
            while (en.MoveNext())
            {
                string trnNo = (string)en.Value;
                trnNo = trnNo.Trim().TrimEnd(',');
                ClientScript.RegisterStartupScript(typeof(string), "Report" + (string)en.Key,
                string.Format("jsOpenOperationalReport('Finance/BankFacilities/ReportViewer.aspx?REPORT={0}&&TRNNO={1}&&REPORTID=-1');",
                                    (string)en.Key, trnNo)
                                    , true);
            }

            LoadData();
        }
        #endregion


        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
        }
        #endregion

        #region btnImport_Click
        protected void btnImport_Click(object sender, EventArgs e)
        {

            ImportBankAccountData();
            DateTime dateFrom = GMSUtil.ToDate(trnDateFrom.Text.Trim());
            DateTime dateTo = GMSUtil.ToDate(trnDateTo.Text.Trim());
            ImportData(dateFrom, dateTo);

        }
        #endregion


        protected void ImportCommand(object sender, CommandEventArgs e)
        {

            DateTime tDateFrom = GMSUtil.ToDate(tbxDateFrom.Text.ToString());
            DateTime tDateTo = GMSUtil.ToDate(tbxDateTo.Text.ToString());

            TimeSpan difference = tDateTo - tDateFrom;
            int diff = GMSUtil.ToShort(difference.TotalDays);


            if (diff > 4)
            {
                JScriptAlertMsg("Please select maximum of 5 days range to import.");
            }
            else
            {
                ImportBankAccountData();
                ImportData(tDateFrom, tDateTo);
            }



        }
    }
}
