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
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Globalization;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System.Collections;

namespace GMSWeb.Finance.BankFacilities
{
    public partial class BankUtilisation : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            49);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                //preload
                PopulateBankCOAList();
                this.ddlBankCOA.Items.Insert(0, new ListItem("-All-", "-1"));
                PopulateBankAccountList();
                dgData.CurrentPageIndex = 0;
                LoadData();
            }
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            IList<GMSCore.Entity.BankUtilisation> lstBU = null;
            short companyID = session.CompanyId;
            string name = string.IsNullOrEmpty(txtSearchName.Text) ? "%" : "%"+txtSearchName.Text.Trim()+"%";
            string mode = string.IsNullOrEmpty(txtSearchMode.Text) ? "%" : "%" + txtSearchMode.Text.Trim() + "%";
            string trnNo = string.IsNullOrEmpty(txtSearchTrnNo.Text) ? "%" : "%"+txtSearchTrnNo.Text.Trim()+"%";
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
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }

            HtmlInputHidden hidTransactionNo = (HtmlInputHidden)e.Item.FindControl("hidTransactionNo");
            GMSCore.Entity.BankUtilisation bUtilisation = new BankActivity().RetrieveBankUtilisationByCoyIDTransactionNo(session.CompanyId, hidTransactionNo.Value);
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
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }

            HtmlInputHidden hidTransactionNo = (HtmlInputHidden)e.Item.FindControl("hidTransactionNo");
            GMSCore.Entity.BankUtilisation bUtilisation = new BankActivity().RetrieveBankUtilisationByCoyIDTransactionNo(session.CompanyId, hidTransactionNo.Value);
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
                    Response.Redirect("../../Unauthorized.htm");
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
                if (bUtilisation.Type == "P")
                {
                    bankAccount.Balance += bUtilisation.Amount;
                }
                else if (bUtilisation.Type == "R")
                {
                    bankAccount.Balance -= bUtilisation.Amount;
                }
                new BankActivity().UpdateBankAccount(ref bankAccount, session);

                bUtilisation.Type = ddlEditType.SelectedValue;
                bUtilisation.Name = txtEditName.Text.Trim();
                bUtilisation.Mode = txtEditMode.Text.Trim();
                bUtilisation.BankCOAID = GMSUtil.ToShort(ddlEditBankCOAID.SelectedValue);
                if (GMSUtil.ToDate(editChequeDate.Text.Trim()) != GMSCoreBase.DEFAULT_NO_DATE)
                bUtilisation.ChequeDate = GMSUtil.ToDate(editChequeDate.Text.Trim());
                bUtilisation.Currency = ddlEditCurrency.SelectedValue;
                bUtilisation.Amount = GMSUtil.ToDouble(txtEditAmount.Text.Trim());
                bUtilisation.Marking1 = chkEditMarking1.Checked;
                bUtilisation.Marking2 = chkEditMarking2.Checked;
                bUtilisation.ModifiedBy = session.UserId;
                bUtilisation.ModifiedDate = DateTime.Now;
                bUtilisation.TransactionDate = GMSUtil.ToDate(editTransactionDate.Text.Trim());

                //Add amount back to the bank balance
                bankAccount = new BankActivity().RetrieveBankAccountByCOAID(GMSUtil.ToShort(bUtilisation.BankCOAID));
                if (bUtilisation.Type == "P")
                {
                    bankAccount.Balance -= bUtilisation.Amount;
                }
                else if (bUtilisation.Type == "R")
                {
                    bankAccount.Balance += bUtilisation.Amount;
                }

                try
                {
                    new BankActivity().UpdateBankAccount(ref bankAccount, session);
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
        }
        #endregion

        #region dgData_DeleteCommand
        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                51);
            if (uAccess == null)
            {
                HtmlInputHidden hidTransactionNo = (HtmlInputHidden)e.Item.FindControl("hidTransactionNo");
                GMSCore.Entity.BankUtilisation bUtilisation = new BankActivity().RetrieveBankUtilisationByCoyIDTransactionNo(session.CompanyId, hidTransactionNo.Value);
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
                                GMSCore.Entity.BankUtilisation bu = bActivity.RetrieveBankUtilisationByCoyIDTransactionNo(session.CompanyId, hidTransactionNo.Value);
                                BankAccount bankAccount = bActivity.RetrieveBankAccountByCOAID(bu.BankCOAID);

                                if (bu.Type == "P")
                                {
                                    bankAccount.Balance += bu.Amount;
                                }
                                else if (bu.Type == "R")
                                {
                                    bankAccount.Balance -= bu.Amount;
                                }

                                bActivity.UpdateBankAccount(ref bankAccount, session);
                                ResultType result = bActivity.DeleteBankUtilisation(session.CompanyId, hidTransactionNo.Value, session);

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
                        try
                        {
                            GMSCore.Entity.BankUtilisation bu = bActivity.RetrieveBankUtilisationByCoyIDTransactionNo(session.CompanyId, hidTransactionNo.Value);
                            BankAccount bankAccount = bActivity.RetrieveBankAccountByCOAID(bu.BankCOAID);

                            if (bu.Type == "P")
                            {
                                bankAccount.Balance += bu.Amount;
                            }
                            else if (bu.Type == "R")
                            {
                                bankAccount.Balance -= bu.Amount;
                            }

                            bActivity.UpdateBankAccount(ref bankAccount, session);
                            ResultType result = bActivity.DeleteBankUtilisation(session.CompanyId, hidTransactionNo.Value, session);

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
                HtmlInputHidden hidTrnNo = (HtmlInputHidden)this.dgData.Items[i].FindControl("hidTransactionNo");
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
                ClientScript.RegisterStartupScript(typeof(string), "Report"+(string)en.Key,
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
    }
}
