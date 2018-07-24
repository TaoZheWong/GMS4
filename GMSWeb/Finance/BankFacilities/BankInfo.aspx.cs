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

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Finance.BankFacilities
{
    public partial class BankInfo : GMSBasePage
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("CompanyFinance"); 
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            48);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyFinance"));

            
            Company coy = new SystemDataActivity().RetrieveCompanyById(session.CompanyId, session);
            
            
            GroupManagementUser gmu = GroupManagementUser.RetrieveByKey(GMSUtil.ToShort(session.UserId));
            if (coy.Is2011COA & coy.HQServerName != "")
            {
                btnImport.Visible = true;
                //btnImportGroup.Visible = true;
            }
            else
            {
                btnImport.Visible = false;
                //btnImportGroup.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                //preload
                dgData.CurrentPageIndex = 0;
                LoadData();

                
            }

            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>
              <script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/importing.js""></script>"; 

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            IList<GMSCore.Entity.BankAccount> lstBankAccount = null;
            try
            {
                lstBankAccount = new SystemDataActivity().RetrieveAllBankAccountListByCompanyIDSortByBankAccount(session.CompanyId);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lstBankAccount != null && lstBankAccount.Count > 0)
            {
                if (endIndex < lstBankAccount.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstBankAccount.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstBankAccount.Count.ToString() + " " + "of" + " " + lstBankAccount.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = lstBankAccount;
            this.dgData.DataBind();
        }
        #endregion

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
            Company coy = new SystemDataActivity().RetrieveCompanyById(session.CompanyId, session);

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                TextBox txtEditBalance = (TextBox)e.Item.FindControl("txtEditBalance");
                DropDownList ddlEditCurrency = (DropDownList)e.Item.FindControl("ddlEditCurrency");
                DropDownList ddlEditBankCode = (DropDownList)e.Item.FindControl("ddlEditBankCode");
                DropDownList ddlEditChequeFormatCode = (DropDownList)e.Item.FindControl("ddlEditChequeFormatCode");
                /*
                if (txtEditBalance != null)
                {                    
                    if (coy.Is2011COA)                    
                        txtEditBalance.Enabled = false;
                    else
                        txtEditBalance.Enabled = true;
                    
                }
                */

                if (ddlEditCurrency != null)
                {
                    GMSCore.Entity.BankAccount bankAccount = (GMSCore.Entity.BankAccount)e.Item.DataItem;
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    if (bankAccount != null)
                    {
                        // fill in Currency dropdown list
                        IList<GMSCore.Entity.Currency> lstCurrency = null;
                        lstCurrency = sDataActivity.RetrieveAllCurrencyListSortByCode();
                        ddlEditCurrency.DataSource = lstCurrency;
                        ddlEditCurrency.DataBind();
                        ddlEditCurrency.SelectedValue = bankAccount.Currency;
                    }
                }

                if (ddlEditBankCode != null)
                {
                    GMSCore.Entity.BankAccount bankAccount = (GMSCore.Entity.BankAccount)e.Item.DataItem;
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    if (bankAccount != null)
                    {
                        // fill in Bank Code dropdown list
                        IList<GMSCore.Entity.Bank> lstBank = null;
                        lstBank = sDataActivity.RetrieveAllBankListSortByBankCode();
                        ddlEditBankCode.DataSource = lstBank;
                        ddlEditBankCode.DataBind();
                        ddlEditBankCode.SelectedValue = bankAccount.BankID.ToString();
                    }
                }

                if (ddlEditChequeFormatCode != null)
                {
                    GMSCore.Entity.BankAccount bankAccount = (GMSCore.Entity.BankAccount)e.Item.DataItem;
                    ddlEditChequeFormatCode.SelectedValue = bankAccount.ChequeFormatCode;
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewCurrency = (DropDownList)e.Item.FindControl("ddlNewCurrency");
                DropDownList ddlNewBankCode = (DropDownList)e.Item.FindControl("ddlNewBankCode");
                TextBox txtNewBalance = (TextBox)e.Item.FindControl("txtNewBalance");
                /*
                if (txtNewBalance != null)
                {
                    if (coy.Is2011COA)
                        txtNewBalance.Enabled = false;
                    else
                        txtNewBalance.Enabled = true;
                }
                */

                if (ddlNewCurrency != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in currency dropdown list
                    IList<GMSCore.Entity.Currency> lstCurrency = null;
                    lstCurrency = sDataActivity.RetrieveAllCurrencyListSortByCode();
                    ddlNewCurrency.DataSource = lstCurrency;
                    ddlNewCurrency.DataBind();
                    ddlNewCurrency.SelectedValue = "SGD";
                }

                if (ddlNewBankCode != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in currency dropdown list
                    IList<GMSCore.Entity.Bank> lstBank = null;
                    lstBank = sDataActivity.RetrieveAllBankListSortByBankCode();
                    ddlNewBankCode.DataSource = lstBank;
                    ddlNewBankCode.DataBind();
                    ddlNewBankCode.SelectedValue = "DBS";
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlInputHidden hidBankCOA = (HtmlInputHidden)e.Item.FindControl("hidBankCOA");
                Label lblDelete = (Label)e.Item.FindControl("lblDelete");                
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

                /*

                if ((hidBankCOA.Value.Trim().Substring(0, 2) == "10") && (coy.Is2011COA))
                {
                    lblDelete.Text = "--";
                    lnkDelete.Visible = false;
                }
                else
                {
                    lblDelete.Text = "";
                    lnkDelete.Visible = true;
                }
                
                */


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
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            50);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyFinance"));

            this.dgData.EditItemIndex = e.Item.ItemIndex;
            LoadData();
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
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            50);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyFinance"));
            Company coy = new SystemDataActivity().RetrieveCompanyById(session.CompanyId, session);
            //TextBox txtEditBankCOA = (TextBox)e.Item.FindControl("txtEditBankCOA");
            TextBox txtEditBalance = (TextBox)e.Item.FindControl("txtEditBalance");
            //DropDownList ddlEditCurrency = (DropDownList)e.Item.FindControl("ddlEditCurrency");
            TextBox txtEditLimit = (TextBox)e.Item.FindControl("txtEditLimit");
            //TextBox txtEditInterestRate = (TextBox)e.Item.FindControl("txtEditInterestRate");
            DropDownList ddlEditBankCode = (DropDownList)e.Item.FindControl("ddlEditBankCode");
            //TextBox editMaturityDate = (TextBox)e.Item.FindControl("editMaturityDate");
            DropDownList ddlEditChequeFormatCode = (DropDownList)e.Item.FindControl("ddlEditChequeFormatCode");
            HtmlInputHidden hidCOAID = (HtmlInputHidden)e.Item.FindControl("hidCOAID");
            CheckBox chkIsMajorBank = (CheckBox)e.Item.FindControl("chkIsMajorBank");
            
            if (ddlEditBankCode != null && hidCOAID != null &&
                txtEditLimit != null && ddlEditChequeFormatCode != null)
            {

                GMSCore.Entity.BankAccount bankAccount = new BankActivity().RetrieveBankAccountByCOAID(GMSUtil.ToShort(hidCOAID.Value));

                DataSet majorBankAccount = new DataSet();
                new GMSGeneralDALC().GetDistinctMajorBankAccountSelect(session.CompanyId, GMSUtil.ToShort(ddlEditBankCode.SelectedValue), ref majorBankAccount);
                            
                int majorBankCount = 0;
                bool isMajorBank = chkIsMajorBank.Checked;

                if (majorBankAccount != null && majorBankAccount.Tables[0].Rows.Count > 0)
                {
                    majorBankCount = majorBankAccount.Tables[0].Rows.Count;
                }



                if (chkIsMajorBank.Checked && ddlEditBankCode.SelectedValue != "29")
                    majorBankCount = majorBankCount + 1;

                if (majorBankCount > 3 || ddlEditBankCode.SelectedValue == "29") 
                    isMajorBank = false; 
                        

                bankAccount.BankID = GMSUtil.ToShort(ddlEditBankCode.SelectedValue);
                //bankAccount.Currency = ddlEditCurrency.SelectedValue;
                bankAccount.ChequeFormatCode = ddlEditChequeFormatCode.SelectedValue;
                bankAccount.Limit = GMSUtil.ToDouble(txtEditLimit.Text);
                //bankAccount.InterestRate = txtEditInterestRate.Text.Trim();
                 
                // if (!coy.Is2011COA)
                // {
                     bankAccount.Balance = GMSUtil.ToDouble(txtEditBalance.Text);
                     bankAccount.DefaultCurrencyBalance = GMSUtil.ToDouble(txtEditBalance.Text);
                // }
                //bankAccount.MaturityDate = GMSUtil.ToDate(editMaturityDate.Text);
                bankAccount.IsMajorBank = isMajorBank;
                bankAccount.ModifiedBy = session.UserId;
                bankAccount.ModifiedDate = DateTime.Now;

               
                new GMSGeneralDALC().UpdateMajorBankAccount(session.CompanyId, GMSUtil.ToShort(ddlEditBankCode.SelectedValue), isMajorBank);


                if ((majorBankCount > 3) && chkIsMajorBank.Checked)
                    this.PageMsgPanel.ShowMessage("Only 3 Major Banks are Allowed!", MessagePanelControl.MessageEnumType.Alert);

                try
                {
                    ResultType result = new BankActivity().UpdateBankAccount(ref bankAccount, session);

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
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                50);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("CompanyFinance"));

                TextBox txtNewBankCOA = (TextBox)e.Item.FindControl("txtNewBankCOA");
                //Label lblNewCurrency = (Label)e.Item.FindControl("lblNewCurrency");
                DropDownList ddlNewBankCode = (DropDownList)e.Item.FindControl("ddlNewBankCode");
                TextBox txtNewBalance = (TextBox)e.Item.FindControl("txtNewBalance");
                //DropDownList ddlNewCurrency = (DropDownList)e.Item.FindControl("ddlNewCurrency");
                TextBox txtNewLimit = (TextBox)e.Item.FindControl("txtNewLimit");
                //TextBox txtNewInterestRate = (TextBox)e.Item.FindControl("txtNewInterestRate");
                //TextBox newMaturityDate = (TextBox)e.Item.FindControl("newMaturityDate");
                DropDownList ddlNewChequeFormatCode = (DropDownList)e.Item.FindControl("ddlNewChequeFormatCode");
                CheckBox chkIsMajorBank = (CheckBox)e.Item.FindControl("chkIsMajorBank");



                if (txtNewBankCOA != null && !string.IsNullOrEmpty(txtNewBankCOA.Text) &&
                    txtNewLimit != null && ddlNewChequeFormatCode != null)
                {
                    try
                    {
                        GMSGeneralDALC dacl = new GMSGeneralDALC();
                        DataSet dsBankAccountByCoyID = new DataSet();
                        dacl.GetListBankAccountSelect(session.CompanyId, txtNewBankCOA.Text.Trim(), ref dsBankAccountByCoyID);

                       

                        if (dsBankAccountByCoyID.Tables[0].Rows.Count > 0)
                        {
                            this.PageMsgPanel.ShowMessage("Processing error of type : This bank account already been used.", MessagePanelControl.MessageEnumType.Alert);
                            LoadData();                          

                        }
                        else
                        {
                            string strCurrency = "";

                            if (txtNewBankCOA.Text.Trim().Substring(4, 1) == "A")
                                strCurrency = "AUD";                           
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 1) == "C")
                                strCurrency = "CNY";
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 1) == "E")
                                strCurrency = "EUR";
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 1) == "B")
                                strCurrency = "GBP";
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 1) == "H")
                                strCurrency = "HKD";
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 1) == "I")
                                strCurrency = "IDR";
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 1) == "J")
                                strCurrency = "JPY";
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 1) == "M")
                                strCurrency = "MYR";
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 1) == "S")
                                strCurrency = "SGD";
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 1) == "T")
                                strCurrency = "THB";
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 1) == "U")
                                strCurrency = "USD";
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 1) == "P")
                                strCurrency = "PHP";
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 2) == "KW")
                                strCurrency = "NOK";
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 2) == "TW")
                                strCurrency = "NTD";                            
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 2) == "SK")
                                strCurrency = "SEK";
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 2) == "SF")
                                strCurrency = "SFR";                           
                            else if (txtNewBankCOA.Text.Trim().Substring(4, 2) == "CD")
                                strCurrency = "CAD";

                            DataSet dsBank = new DataSet();
                            new GMSGeneralDALC().GetBankID(txtNewBankCOA.Text.Trim().Substring(2, 2), ref dsBank);
                            short bankid = 29;                           

                            if (dsBank.Tables[0].Rows.Count > 0)
                            {
                                bankid = GMSUtil.ToShort(dsBank.Tables[0].Rows[0]["BankID"]);
                               
                            }


                            DataSet majorBankAccount = new DataSet();
                            new GMSGeneralDALC().GetDistinctMajorBankAccountSelect(session.CompanyId, bankid , ref majorBankAccount);

                            int majorBankCount = 0;
                            bool isMajorBank = chkIsMajorBank.Checked;

                            if (majorBankAccount != null && majorBankAccount.Tables[0].Rows.Count > 0)
                            {
                                majorBankCount = majorBankAccount.Tables[0].Rows.Count;
                            }

                            if (chkIsMajorBank.Checked && bankid.ToString() != "29")
                                majorBankCount = majorBankCount + 1;

                            if (majorBankCount > 3 || bankid.ToString() == "29")
                                isMajorBank = false;

                            
                            GMSCore.Entity.BankAccount bankAccount = new GMSCore.Entity.BankAccount();
                            bankAccount.BankCOA = txtNewBankCOA.Text.Trim();
                            bankAccount.BankID = GMSUtil.ToShort(bankid);                  

                                                        
                            bankAccount.Currency = strCurrency;
                            bankAccount.ChequeFormatCode = ddlNewChequeFormatCode.SelectedValue;
                            bankAccount.Limit = GMSUtil.ToDouble(txtNewLimit.Text);
                            //bankAccount.InterestRate = txtNewInterestRate.Text.Trim();
                            /*
                            Company coy = new SystemDataActivity().RetrieveCompanyById(session.CompanyId, session);
                            if (coy.Is2011COA)
                            {

                                DataSet dsAmount = new DataSet();
                                try
                                {
                                    GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                                    if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                                    {
                                        sc.Url = session.WebServiceAddress.Trim();
                                    }
                                    else
                                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";


                                    if (txtNewBankCOA.Text.Trim().Substring(0, 2) == "10")
                                    {
                                        dsAmount = sc.GetBankAccountCOA10BalanceFromA21(session.CompanyId, txtNewBankCOA.Text.Trim().ToString(), strCurrency);

                                    }
                                    else
                                    {
                                        dsAmount = sc.GetBankAccountCOAUtilisedBalanceFromA21(session.CompanyId, txtNewBankCOA.Text.Trim().ToString(), strCurrency);

                                    }

                                }
                                catch (Exception ex)
                                {
                                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                                }



                                if (dsAmount != null && dsAmount.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsAmount.Tables[0].Rows.Count; k++)
                                    {
                                        bankAccount.Balance = GMSUtil.ToDouble(dsAmount.Tables[0].Rows[k]["Amount"].ToString());
                                    }
                                }
                                else
                                {
                                    bankAccount.Balance = 0;
                                }

                            }
                            else
                            {
                                bankAccount.Balance = GMSUtil.ToDouble(txtNewBalance.Text);
                                bankAccount.DefaultCurrencyBalance = GMSUtil.ToDouble(txtNewBalance.Text);
                            }
                            */

                            bankAccount.Balance = GMSUtil.ToDouble(txtNewBalance.Text);
                            bankAccount.DefaultCurrencyBalance = GMSUtil.ToDouble(txtNewBalance.Text);

                            //bankAccount.MaturityDate = GMSUtil.ToDate(newMaturityDate.Text);

                            bankAccount.IsMajorBank = isMajorBank;
                            bankAccount.CreatedBy = session.UserId;
                            bankAccount.CreatedDate = DateTime.Now;
                            bankAccount.CoyID = session.CompanyId;

                            ResultType result = new BankActivity().CreateBankAccount(ref bankAccount, session);

                            switch (result)
                            {
                                case ResultType.Ok:
                                    LoadData();
                                    break;
                                default:
                                    this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                    return;
                            }

                            new GMSGeneralDALC().UpdateMajorBankAccount(session.CompanyId, GMSUtil.ToShort(bankid), isMajorBank);


                            if ((majorBankCount > 3) && chkIsMajorBank.Checked)
                                this.PageMsgPanel.ShowMessage("Only 3 Major Banks are Allowed!", MessagePanelControl.MessageEnumType.Alert);

                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgData_DeleteCommand
        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                50);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("CompanyFinance"));

                HtmlInputHidden hidCOAID = (HtmlInputHidden)e.Item.FindControl("hidCOAID");

                if (hidCOAID != null)
                {
                    BankActivity bActivity = new BankActivity();

                    try
                    {
                        ResultType result = bActivity.DeleteBankAccount(GMSUtil.ToShort(hidCOAID.Value), session);

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
                            this.PageMsgPanel.ShowMessage("This bank account cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
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

        private void RemoveBankAccountData(DataSet ds)
        {
            LogSession session = base.GetSessionInfo();

            string strBankListInA21 = "'0'".ToString();

            DataSet dsAmount = new DataSet();

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    strBankListInA21 += ",'" + ds.Tables[0].Rows[j]["bankcoa"].ToString() + "'";
                }

            }

            new GMSGeneralDALC().DeleteBankFacilityAndBankUtilisation(session.CompanyId, strBankListInA21);

        }

        #region btnImportGroup_Click
        protected void btnImportGroup_Click(object sender, EventArgs e)
        {

            LogSession session = base.GetSessionInfo();
            IList<Company> lstCompany = new SystemDataActivity().RetrieveAllAliveCompanyList();
            foreach (Company coy in lstCompany)
            {
                if ((coy.HQServerName.ToString() != null) && (coy.HQServerName.ToString() != "") && (coy.Is2011COA))
                {

                    DataSet ds = new DataSet();
                    try
                    {
                        GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                        sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                        ds = sc.GetBankAccountFromA21HQ(coy.CoyID);

                        RemoveBankAccountData(ds);

                        GMSGeneralDALC dacl = new GMSGeneralDALC();
                        DataSet dsBankAccountByCoyID = new DataSet();
                        DataSet dsBank = new DataSet();

                        string strCurrency = "SGD";
                        int bankid = 29;

                        DataSet dsAmount = new DataSet();

                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {

                                try
                                {

                                    if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "A")
                                        strCurrency = "AUD";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "P")
                                        strCurrency = "PHP";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "C")
                                        strCurrency = "CNY";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "E")
                                        strCurrency = "EUR";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "B")
                                        strCurrency = "GBP";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "H")
                                        strCurrency = "HKD";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "I")
                                        strCurrency = "IDR";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "J")
                                        strCurrency = "JPY";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "M")
                                        strCurrency = "MYR";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "S")
                                        strCurrency = "SGD";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "T")
                                        strCurrency = "THB";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "U")
                                        strCurrency = "USD";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "KW")
                                        strCurrency = "NOK";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "TW")
                                        strCurrency = "NTD";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "SK")
                                        strCurrency = "SEK";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "SF")
                                        strCurrency = "SFR";
                                    else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "CD")
                                        strCurrency = "CAD";

                                    dsBankAccountByCoyID.Clear();
                                    dacl.GetListBankAccountSelect(coy.CoyID, ds.Tables[0].Rows[j]["bankcoa"].ToString(), ref dsBankAccountByCoyID);

                                    dsBank.Clear();
                                    dacl.GetBankID(ds.Tables[0].Rows[j]["BankNumericCode"].ToString(), ref dsBank);


                                    if (dsBank.Tables[0].Rows.Count > 0)
                                    {
                                        bankid = GMSUtil.ToInt(dsBank.Tables[0].Rows[0]["BankID"]);
                                    }


                                    try
                                    {
                                        GMSWebService.GMSWebService sc1 = new GMSWebService.GMSWebService();
                                        sc1.Url = "http://localhost/GMSWebService/GMSWebService.asmx";


                                        dsAmount.Clear();
                                        if ((ds.Tables[0].Rows[j]["code"].ToString() == "10") || (ds.Tables[0].Rows[j]["code"].ToString() == "11"))
                                        {
                                            dsAmount = sc1.GetBankAccountCOA10BalanceFromA21HQ(coy.CoyID, ds.Tables[0].Rows[j]["bankcoa"].ToString(), strCurrency);
                                        }
                                        else
                                        {
                                            dsAmount = sc1.GetBankAccountCOAUtilisedBalanceFromA21HQ(coy.CoyID, ds.Tables[0].Rows[j]["bankcoa"].ToString(), strCurrency);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                                        this.PageMsgPanel.ShowMessage("The connection to the server has failed! <br />For more information, please contact your System Administrator. ", MessagePanelControl.MessageEnumType.Alert);
                                    }

                                    if (dsBankAccountByCoyID.Tables[0].Rows.Count == 0)
                                    {
                                        GMSCore.Entity.BankAccount bankAccount = new GMSCore.Entity.BankAccount();
                                        bankAccount.BankCOA = ds.Tables[0].Rows[j]["bankcoa"].ToString();
                                        bankAccount.BankID = GMSUtil.ToShort(bankid);
                                        bankAccount.Currency = strCurrency;
                                        bankAccount.IsMajorBank = false;
                                        bankAccount.ChequeFormatCode = "N.A";
                                        bankAccount.Limit = 0;
                                        bankAccount.InterestRate = "Nil";

                                        if (dsAmount != null && dsAmount.Tables[0].Rows.Count > 0)
                                        {
                                            for (int k = 0; k < dsAmount.Tables[0].Rows.Count; k++)
                                            {
                                                bankAccount.Balance = GMSUtil.ToDouble(dsAmount.Tables[0].Rows[k]["Amount"].ToString());
                                                bankAccount.DefaultCurrencyBalance = GMSUtil.ToDouble(dsAmount.Tables[0].Rows[k]["DefaultAmount"].ToString());
                                            }
                                        }
                                        else
                                        {
                                            bankAccount.Balance = 0;
                                            bankAccount.DefaultCurrencyBalance = 0;
                                        }


                                        bankAccount.MaturityDate = DateTime.Now;
                                        bankAccount.CreatedBy = session.UserId;
                                        bankAccount.CreatedDate = DateTime.Now;
                                        bankAccount.CoyID = coy.CoyID;

                                        ResultType result = new BankActivity().CreateBankAccount(ref bankAccount, session);

                                    }
                                    else
                                    {
                                        GMSCore.Entity.BankAccount bankAccount = new BankActivity().RetrieveBankAccountByCOAID(GMSUtil.ToShort(dsBankAccountByCoyID.Tables[0].Rows[0]["coaid"]));
                                        bankAccount.BankCOA = ds.Tables[0].Rows[j]["bankcoa"].ToString();
                                        bankAccount.BankID = GMSUtil.ToShort(bankid);
                                        bankAccount.Currency = strCurrency;
                                        if (dsAmount != null && dsAmount.Tables[0].Rows.Count > 0)
                                        {
                                            for (int k = 0; k < dsAmount.Tables[0].Rows.Count; k++)
                                            {
                                                bankAccount.Balance = GMSUtil.ToDouble(dsAmount.Tables[0].Rows[k]["Amount"].ToString());
                                                bankAccount.DefaultCurrencyBalance = GMSUtil.ToDouble(dsAmount.Tables[0].Rows[k]["DefaultAmount"].ToString());
                                            }
                                        }
                                        else
                                        {
                                            bankAccount.Balance = 0;
                                            bankAccount.DefaultCurrencyBalance = 0;
                                        }
                                        bankAccount.ModifiedBy = session.UserId;
                                        bankAccount.ModifiedDate = DateTime.Now;


                                        ResultType result = new BankActivity().UpdateBankAccount(ref bankAccount, session);
                                    }



                                }
                                catch (Exception ex)
                                {
                                    JScriptAlertMsg(ex.Message);
                                }

                            }
                        }
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "progress_stop", "progress_stop();", true);
                        JScriptAlertMsg("Finished importing.");
                        LoadData();



                    }
                    catch (Exception ex)
                    {
                        //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);

                        this.PageMsgPanel.ShowMessage("The connection to the server has failed! <br />For more information, please contact your System Administrator. " + (coy.Name.ToString()), MessagePanelControl.MessageEnumType.Alert);
                    }
                }
            }
           


        }
        #endregion

        #region btnImport_Click
        protected void btnImport_Click(object sender, EventArgs e)
        {

            LogSession session = base.GetSessionInfo();
            

            DataSet ds = new DataSet();
            try
            {
               
                GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                {
                    sc.Url = session.WebServiceAddress.Trim();
                }
                else
                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                ds = sc.GetBankAccountFromA21HQ(session.CompanyId);


                RemoveBankAccountData(ds);

                GMSGeneralDALC dacl = new GMSGeneralDALC();
                DataSet dsBankAccountByCoyID = new DataSet();
                DataSet dsBank = new DataSet();

                string strCurrency = "SGD";
                int bankid = 29;

                DataSet dsAmount = new DataSet();

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {

                        try
                        {

                            if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "A")
                                strCurrency = "AUD";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "P")
                                strCurrency = "PHP";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "C")
                                strCurrency = "CNY";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "E")
                                strCurrency = "EUR";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "B")
                                strCurrency = "GBP";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "H")
                                strCurrency = "HKD";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "I")
                                strCurrency = "IDR";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "J")
                                strCurrency = "JPY";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "M")
                                strCurrency = "MYR";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "S")
                                strCurrency = "SGD";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "T")
                                strCurrency = "THB";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "U")
                                strCurrency = "USD";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "KW")
                                strCurrency = "NOK";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "TW")
                                strCurrency = "NTD";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "SK")
                                strCurrency = "SEK";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "SF")
                                strCurrency = "SFR";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "CD")
                                strCurrency = "CAD";

                            dsBankAccountByCoyID.Clear();
                            dacl.GetListBankAccountSelect(session.CompanyId, ds.Tables[0].Rows[j]["bankcoa"].ToString(), ref dsBankAccountByCoyID);

                            dsBank.Clear();
                            dacl.GetBankID(ds.Tables[0].Rows[j]["BankNumericCode"].ToString(), ref dsBank);


                            if (dsBank.Tables[0].Rows.Count > 0)
                            {
                                bankid = GMSUtil.ToInt(dsBank.Tables[0].Rows[0]["BankID"]);
                            }


                            try
                            {
                                GMSWebService.GMSWebService sc1 = new GMSWebService.GMSWebService();
                                                              
                                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                                {
                                    sc1.Url = session.WebServiceAddress.Trim();
                                }
                                else
                                    sc1.Url = "http://localhost/GMSWebService/GMSWebService.asmx";


                                dsAmount.Clear();
                                if ((ds.Tables[0].Rows[j]["code"].ToString() == "10") || (ds.Tables[0].Rows[j]["code"].ToString() == "11"))
                                {
                                    dsAmount = sc1.GetBankAccountCOA10BalanceFromA21HQ(session.CompanyId, ds.Tables[0].Rows[j]["bankcoa"].ToString(), strCurrency);
                                }
                                else
                                {
                                    dsAmount = sc1.GetBankAccountCOAUtilisedBalanceFromA21HQ(session.CompanyId, ds.Tables[0].Rows[j]["bankcoa"].ToString(), strCurrency);
                                }

                            }
                            catch (Exception ex)
                            {
                                //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                                this.PageMsgPanel.ShowMessage("The connection to the server has failed! <br />For more information, please contact your System Administrator. ", MessagePanelControl.MessageEnumType.Alert);
                            }

                            if (dsBankAccountByCoyID.Tables[0].Rows.Count == 0)
                            {
                                GMSCore.Entity.BankAccount bankAccount = new GMSCore.Entity.BankAccount();
                                bankAccount.BankCOA = ds.Tables[0].Rows[j]["bankcoa"].ToString();
                                bankAccount.BankID = GMSUtil.ToShort(bankid);
                                bankAccount.Currency = strCurrency;
                                bankAccount.IsMajorBank = false;
                                bankAccount.ChequeFormatCode = "N.A";
                                bankAccount.Limit = 0;
                                bankAccount.InterestRate = "Nil";

                                if (dsAmount != null && dsAmount.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsAmount.Tables[0].Rows.Count; k++)
                                    {
                                        bankAccount.Balance = GMSUtil.ToDouble(dsAmount.Tables[0].Rows[k]["Amount"].ToString());
                                        bankAccount.DefaultCurrencyBalance = GMSUtil.ToDouble(dsAmount.Tables[0].Rows[k]["DefaultAmount"].ToString());
                                    }
                                }
                                else
                                {
                                    bankAccount.Balance = 0;
                                    bankAccount.DefaultCurrencyBalance = 0;
                                }


                                bankAccount.MaturityDate = DateTime.Now;
                                bankAccount.CreatedBy = session.UserId;
                                bankAccount.CreatedDate = DateTime.Now;
                                bankAccount.CoyID = session.CompanyId;

                                ResultType result = new BankActivity().CreateBankAccount(ref bankAccount, session);

                            }
                            else
                            {
                                GMSCore.Entity.BankAccount bankAccount = new BankActivity().RetrieveBankAccountByCOAID(GMSUtil.ToShort(dsBankAccountByCoyID.Tables[0].Rows[0]["coaid"]));
                                bankAccount.BankCOA = ds.Tables[0].Rows[j]["bankcoa"].ToString();
                                bankAccount.BankID = GMSUtil.ToShort(bankid);
                                bankAccount.Currency = strCurrency;
                                if (dsAmount != null && dsAmount.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsAmount.Tables[0].Rows.Count; k++)
                                    {
                                        bankAccount.Balance = GMSUtil.ToDouble(dsAmount.Tables[0].Rows[k]["Amount"].ToString());
                                        bankAccount.DefaultCurrencyBalance = GMSUtil.ToDouble(dsAmount.Tables[0].Rows[k]["DefaultAmount"].ToString());
                                    }
                                }
                                else
                                {
                                    bankAccount.Balance = 0;
                                    bankAccount.DefaultCurrencyBalance = 0;
                                }
                                bankAccount.ModifiedBy = session.UserId;
                                bankAccount.ModifiedDate = DateTime.Now;


                                ResultType result = new BankActivity().UpdateBankAccount(ref bankAccount, session);
                            }



                        }
                        catch (Exception ex)
                        {
                            JScriptAlertMsg(ex.Message);
                        }

                    }
                }
                ScriptManager.RegisterStartupScript(this, typeof(Page), "progress_stop", "progress_stop();", true);
                JScriptAlertMsg("Finished importing.");
                LoadData();



            }
            catch (Exception ex)
            {
                //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);

                this.PageMsgPanel.ShowMessage("The connection to the server has failed! <br />For more information, please contact your System Administrator. ", MessagePanelControl.MessageEnumType.Alert);
            }
            /*

            LogSession session = base.GetSessionInfo();

            IList<Company> lstCompany = new SystemDataActivity().RetrieveAllAliveCompanyList();
            foreach (Company coy in lstCompany)
            {
                if ((coy.HQServerName.ToString() != null) && (coy.HQServerName.ToString() != ""))
                {

                    DataSet ds = new DataSet();
            try
            {
                GMSGeneralDALC dacl = new GMSGeneralDALC();

                dacl.GetBankAccountFromA21(coy.CoyID, ref ds);

                RemoveBankAccountData(ds);

               
                DataSet dsBankAccountByCoyID = new DataSet();
                DataSet dsBank = new DataSet();

                string strCurrency = "SGD";
                int bankid = 29;

                DataSet dsAmount = new DataSet();

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {

                        try
                        {

                            if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "A")
                                strCurrency = "AUD";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "P")
                                strCurrency = "PHP";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "C")
                                strCurrency = "CNY";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "E")
                                strCurrency = "EUR";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "B")
                                strCurrency = "GBP";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "H")
                                strCurrency = "HKD";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "I")
                                strCurrency = "IDR";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "J")
                                strCurrency = "JPY";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "M")
                                strCurrency = "MYR";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "S")
                                strCurrency = "SGD";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "T")
                                strCurrency = "THB";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 1) == "U")
                                strCurrency = "USD";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "KW")
                                strCurrency = "NOK";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "TW")
                                strCurrency = "NTD";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "SK")
                                strCurrency = "SEK";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "SF")
                                strCurrency = "SFR";
                            else if (ds.Tables[0].Rows[j]["bankdenoted"].ToString().Substring(0, 2) == "CD")
                                strCurrency = "CAD";

                            dsBankAccountByCoyID.Clear();
                            dacl.GetListBankAccountSelect(coy.CoyID, ds.Tables[0].Rows[j]["bankcoa"].ToString(), ref dsBankAccountByCoyID);

                            dsBank.Clear();
                            dacl.GetBankID(ds.Tables[0].Rows[j]["BankNumericCode"].ToString(), ref dsBank);


                            if (dsBank.Tables[0].Rows.Count > 0)
                            {
                                bankid = GMSUtil.ToInt(dsBank.Tables[0].Rows[0]["BankID"]);
                            }


                            try
                            {
                                
                                dsAmount.Clear();
                                
                                if ((ds.Tables[0].Rows[j]["code"].ToString() == "10") || (ds.Tables[0].Rows[j]["code"].ToString() == "11"))
                                {
                                    dacl.GetBankAccountCOABalanceFromA21(coy.CoyID, ds.Tables[0].Rows[j]["bankcoa"].ToString(), strCurrency, "CASH", ref dsAmount);
                                   
                                }
                                else
                                {
                                    dacl.GetBankAccountCOABalanceFromA21(coy.CoyID, ds.Tables[0].Rows[j]["bankcoa"].ToString(), strCurrency, "", ref dsAmount);
                                }

                            }
                            catch (Exception ex)
                            {
                                //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                                this.PageMsgPanel.ShowMessage("The connection to the server has failed! <br />For more information, please contact your System Administrator. ", MessagePanelControl.MessageEnumType.Alert);
                            }

                            if (dsBankAccountByCoyID.Tables[0].Rows.Count == 0)
                            {
                                GMSCore.Entity.BankAccount bankAccount = new GMSCore.Entity.BankAccount();
                                bankAccount.BankCOA = ds.Tables[0].Rows[j]["bankcoa"].ToString();
                                bankAccount.BankID = GMSUtil.ToShort(bankid);
                                bankAccount.Currency = strCurrency;
                                bankAccount.IsMajorBank = false;
                                bankAccount.ChequeFormatCode = "N.A";
                                bankAccount.Limit = 0;
                                bankAccount.InterestRate = "Nil";

                                if (dsAmount != null && dsAmount.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsAmount.Tables[0].Rows.Count; k++)
                                    {
                                        bankAccount.Balance = GMSUtil.ToDouble(dsAmount.Tables[0].Rows[k]["Amount"].ToString());
                                        bankAccount.DefaultCurrencyBalance = GMSUtil.ToDouble(dsAmount.Tables[0].Rows[k]["DefaultAmount"].ToString());
                                    }
                                }
                                else
                                {
                                    bankAccount.Balance = 0;
                                    bankAccount.DefaultCurrencyBalance = 0;
                                }


                                bankAccount.MaturityDate = DateTime.Now;
                                bankAccount.CreatedBy = session.UserId;
                                bankAccount.CreatedDate = DateTime.Now;
                                bankAccount.CoyID = coy.CoyID;

                                ResultType result = new BankActivity().CreateBankAccount(ref bankAccount, session);

                            }
                            else
                            {
                                GMSCore.Entity.BankAccount bankAccount = new BankActivity().RetrieveBankAccountByCOAID(GMSUtil.ToShort(dsBankAccountByCoyID.Tables[0].Rows[0]["coaid"]));
                                bankAccount.BankCOA = ds.Tables[0].Rows[j]["bankcoa"].ToString();
                                bankAccount.BankID = GMSUtil.ToShort(bankid);
                                bankAccount.Currency = strCurrency;
                                if (dsAmount != null && dsAmount.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsAmount.Tables[0].Rows.Count; k++)
                                    {
                                        bankAccount.Balance = GMSUtil.ToDouble(dsAmount.Tables[0].Rows[k]["Amount"].ToString());
                                        bankAccount.DefaultCurrencyBalance = GMSUtil.ToDouble(dsAmount.Tables[0].Rows[k]["DefaultAmount"].ToString());
                                    }
                                }
                                else
                                {
                                    bankAccount.Balance = 0;
                                    bankAccount.DefaultCurrencyBalance = 0;
                                }
                                bankAccount.ModifiedBy = session.UserId;
                                bankAccount.ModifiedDate = DateTime.Now;


                                ResultType result = new BankActivity().UpdateBankAccount(ref bankAccount, session);
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
                //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);

                this.PageMsgPanel.ShowMessage("The connection to the server has failed! <br />For more information, please contact your System Administrator. ", MessagePanelControl.MessageEnumType.Alert);
            }


                }

            }

            

           LoadData();
           */

        
        }
        #endregion


        



        
        
    }
}
