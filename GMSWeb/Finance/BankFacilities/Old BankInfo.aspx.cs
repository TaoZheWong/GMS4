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

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Finance.BankFacility
{
    public partial class BankInfo : GMSBasePage
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
                                                                            48);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                //preload
                dgData.CurrentPageIndex = 0;
                LoadData();
            }
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
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddlEditCurrency = (DropDownList)e.Item.FindControl("ddlEditCurrency");
                DropDownList ddlEditBankCode = (DropDownList)e.Item.FindControl("ddlEditBankCode");
                DropDownList ddlEditChequeFormatCode = (DropDownList)e.Item.FindControl("ddlEditChequeFormatCode");

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
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            50);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

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
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            50);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            TextBox txtEditBankCOA = (TextBox)e.Item.FindControl("txtEditBankCOA");
            TextBox txtEditBalance = (TextBox)e.Item.FindControl("txtEditBalance");
            DropDownList ddlEditCurrency = (DropDownList)e.Item.FindControl("ddlEditCurrency");
            TextBox txtEditLimit = (TextBox)e.Item.FindControl("txtEditLimit");
            TextBox txtEditInterestRate = (TextBox)e.Item.FindControl("txtEditInterestRate");
            DropDownList ddlEditBankCode = (DropDownList) e.Item.FindControl("ddlEditBankCode");
            TextBox editMaturityDate = (TextBox)e.Item.FindControl("editMaturityDate");
            DropDownList ddlEditChequeFormatCode = (DropDownList)e.Item.FindControl("ddlEditChequeFormatCode");
            HtmlInputHidden hidCOAID = (HtmlInputHidden)e.Item.FindControl("hidCOAID");

            if (ddlEditBankCode != null && txtEditBalance != null && hidCOAID != null &&
                ddlEditCurrency != null && txtEditLimit != null && txtEditInterestRate != null && editMaturityDate != null && ddlEditChequeFormatCode != null)
            {

                GMSCore.Entity.BankAccount bankAccount = new BankActivity().RetrieveBankAccountByCOAID(GMSUtil.ToShort(hidCOAID.Value));
                bankAccount.BankID = GMSUtil.ToShort(ddlEditBankCode.SelectedValue);
                bankAccount.Currency = ddlEditCurrency.SelectedValue;
                bankAccount.ChequeFormatCode = ddlEditChequeFormatCode.SelectedValue;
                bankAccount.Limit = GMSUtil.ToDouble(txtEditLimit.Text);
                bankAccount.InterestRate = txtEditInterestRate.Text.Trim();
                bankAccount.Balance = GMSUtil.ToDouble(txtEditBalance.Text);
                bankAccount.MaturityDate = GMSUtil.ToDate(editMaturityDate.Text);
                bankAccount.ModifiedBy = session.UserId;
                bankAccount.ModifiedDate = DateTime.Now;

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
                    Response.Redirect("../../SessionTimeout.htm");
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                50);
                if (uAccess == null)
                    Response.Redirect("../../Unauthorized.htm");

                TextBox txtNewBankCOA = (TextBox)e.Item.FindControl("txtNewBankCOA");
                DropDownList ddlNewBankCode = (DropDownList)e.Item.FindControl("ddlNewBankCode");
                TextBox txtNewBalance = (TextBox)e.Item.FindControl("txtNewBalance");
                DropDownList ddlNewCurrency = (DropDownList)e.Item.FindControl("ddlNewCurrency");
                TextBox txtNewLimit = (TextBox)e.Item.FindControl("txtNewLimit");
                TextBox txtNewInterestRate = (TextBox)e.Item.FindControl("txtNewInterestRate");
                TextBox newMaturityDate = (TextBox)e.Item.FindControl("newMaturityDate");
                DropDownList ddlNewChequeFormatCode = (DropDownList)e.Item.FindControl("ddlNewChequeFormatCode");

                if (txtNewBankCOA != null && !string.IsNullOrEmpty(txtNewBankCOA.Text) && ddlNewBankCode != null && txtNewBalance != null &&
                    ddlNewCurrency != null && txtNewLimit != null && txtNewInterestRate != null && newMaturityDate != null && ddlNewChequeFormatCode != null)
                {
                    try
                    {
                        // check if newly inserted bank already exist
                        //GMSCore.Entity.BankAccount existingBankAccount = new BankActivity().RetrieveBankAccountByBankCOA(txtNewBankCOA.Text.Trim());

                        //if (existingBankAccount != null)
                        //{
                        //    this.PageMsgPanel.ShowMessage("Processing error of type : This bank account already been used.", MessagePanelControl.MessageEnumType.Alert);
                        //    return;
                        //}

                        GMSCore.Entity.BankAccount bankAccount = new GMSCore.Entity.BankAccount();
                        bankAccount.BankCOA = txtNewBankCOA.Text.Trim();
                        bankAccount.BankID = GMSUtil.ToShort(ddlNewBankCode.SelectedValue);
                        bankAccount.Currency = ddlNewCurrency.SelectedValue;
                        bankAccount.ChequeFormatCode = ddlNewChequeFormatCode.SelectedValue;
                        bankAccount.Limit = GMSUtil.ToDouble(txtNewLimit.Text);
                        bankAccount.InterestRate = txtNewInterestRate.Text.Trim();
                        bankAccount.Balance = GMSUtil.ToDouble(txtNewBalance.Text);
                        bankAccount.MaturityDate = GMSUtil.ToDate(newMaturityDate.Text);
                        bankAccount.OpenBalance = 0;
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
                    Response.Redirect("../../SessionTimeout.htm");
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                50);
                if (uAccess == null)
                    Response.Redirect("../../Unauthorized.htm");

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
    }
}
