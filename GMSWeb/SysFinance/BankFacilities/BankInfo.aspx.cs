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

namespace GMSWeb.SysFinance.BankFacilities
{
    public partial class BankInfo : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Finance"); 
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Finance"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            54);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Finance"));

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
            IList<GMSCore.Entity.Bank> lstBank = null;
            try
            {
                lstBank = new SystemDataActivity().RetrieveAllBankListSortByBankName();
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lstBank != null && lstBank.Count > 0)
            {
                if (endIndex < lstBank.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstBank.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstBank.Count.ToString() + " " + "of" + " " + lstBank.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = lstBank;
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
                Response.Redirect(base.SessionTimeOutPage("Finance"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            54);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Finance"));

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
                Response.Redirect(base.SessionTimeOutPage("Finance"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            54);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Finance"));

            TextBox txtEditBankName = (TextBox)e.Item.FindControl("txtEditBankName");
            HtmlInputHidden hidBankCode = (HtmlInputHidden)e.Item.FindControl("hidBankCode");

            if (txtEditBankName != null && !string.IsNullOrEmpty(txtEditBankName.Text) && hidBankCode != null)
            {

                GMSCore.Entity.Bank bank = new BankActivity().RetrieveBankByBankCode(hidBankCode.Value);
                bank.BankName = txtEditBankName.Text.Trim();
                bank.ModifiedBy = session.UserId;
                bank.ModifiedDate = DateTime.Now;

                try
                {
                    ResultType result = new BankActivity().UpdateBank(ref bank, session);

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
                    Response.Redirect(base.SessionTimeOutPage("Finance"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                54);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Finance"));

                TextBox txtNewBankCode = (TextBox)e.Item.FindControl("txtNewBankCode");
                TextBox txtNewBankName = (TextBox)e.Item.FindControl("txtNewBankName");

                if (txtNewBankCode != null && !string.IsNullOrEmpty(txtNewBankCode.Text) && txtNewBankName != null && !string.IsNullOrEmpty(txtNewBankName.Text))
                {
                    try
                    {
                        // check if newly inserted bank already exist
                        GMSCore.Entity.Bank existingBank = new BankActivity().RetrieveBankByBankCode(txtNewBankCode.Text.Trim());

                        if (existingBank != null)
                        {
                            this.PageMsgPanel.ShowMessage("Processing error of type : This bank code already been used.", MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }

                        GMSCore.Entity.Bank bank = new GMSCore.Entity.Bank();
                        bank.BankCode = txtNewBankCode.Text.Trim();
                        bank.BankName = txtNewBankName.Text.Trim();
                        bank.CreatedBy = session.UserId;
                        bank.CreatedDate = DateTime.Now;

                        ResultType result = new BankActivity().CreateBank(ref bank, session);

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
                    Response.Redirect(base.SessionTimeOutPage("Finance"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                54);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Finance"));

                HtmlInputHidden hidBankCode = (HtmlInputHidden)e.Item.FindControl("hidBankCode");

                if (hidBankCode != null)
                {
                    BankActivity bActivity = new BankActivity();

                    try
                    {
                        ResultType result = bActivity.DeleteBank(hidBankCode.Value, session);

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
                            this.PageMsgPanel.ShowMessage("This bank cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
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
