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

namespace GMSWeb.Admin.SystemData
{
    public partial class CurrencyListing : GMSBasePage
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
                                                                            9);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                LoadCurrencyData();
            }
        }

        //Load Data
        #region LoadCurrencyData
        private void LoadCurrencyData()
        {
            LogSession session = base.GetSessionInfo();
            IList<Currency> lstCurrency = null;
            try
            {
                lstCurrency = new SystemDataActivity().RetrieveAllCurrencyListSortByCode(session);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            this.dgCurrency.DataSource = lstCurrency;
            this.dgCurrency.DataBind();
        }
        #endregion

        #region dgCurrency_ItemDataBound
        protected void dgCurrency_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region dgCurrency_EditCommand
        protected void dgCurrency_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgCurrency.EditItemIndex = e.Item.ItemIndex;
            LoadCurrencyData();
        }
        #endregion

        #region dgCurrency_CancelCommand
        protected void dgCurrency_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgCurrency.EditItemIndex = -1;
            LoadCurrencyData();
        }
        #endregion

        #region dgCurrency_UpdateCommand
        protected void dgCurrency_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            TextBox txtEditCode = (TextBox)e.Item.FindControl("txtEditCode");
            TextBox txtEditName = (TextBox)e.Item.FindControl("txtEditName");
            TextBox txtEditSign = (TextBox)e.Item.FindControl("txtEditSign");

            if (txtEditCode != null && txtEditName != null && !string.IsNullOrEmpty(txtEditName.Text) &&
                !string.IsNullOrEmpty(txtEditCode.Text) && txtEditSign != null && !string.IsNullOrEmpty(txtEditSign.Text))
            {
                string sCurrencyCode = this.dgCurrency.DataKeys[e.Item.ItemIndex].ToString();

                if (sCurrencyCode != null)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    Currency currency = null;

                    try
                    {
                        currency = sDataActivity.RetrieveCurrencyById(sCurrencyCode, session);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    currency.CurrencyCode = txtEditCode.Text.Trim();
                    currency.CurrencyName = txtEditName.Text.Trim();
                    currency.CurrencySign = txtEditSign.Text.Trim();
                    currency.ModifiedBy = session.UserId;
                    currency.ModifiedDate = DateTime.Now;

                    try
                    {
                        ResultType result = sDataActivity.UpdateCurrency(ref currency, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgCurrency.EditItemIndex = -1;
                                LoadCurrencyData();
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

        #region dgCurrency_CreateCommand
        protected void dgCurrency_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                TextBox txtNewCode = (TextBox)e.Item.FindControl("txtNewCode");
                TextBox txtNewName = (TextBox)e.Item.FindControl("txtNewName");
                TextBox txtNewSign = (TextBox)e.Item.FindControl("txtNewSign");

                if (txtNewName != null && txtNewCode != null && !string.IsNullOrEmpty(txtNewName.Text) &&
                    !string.IsNullOrEmpty(txtNewCode.Text) && txtNewSign != null && !string.IsNullOrEmpty(txtNewSign.Text))
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    Currency currency = new Currency();

                    currency.CurrencyCode = txtNewCode.Text.Trim();
                    currency.CurrencyName = txtNewName.Text.Trim();
                    currency.CurrencySign = txtNewSign.Text.Trim();
                    currency.CreatedBy = session.UserId;
                    currency.CreatedDate = DateTime.Now;

                    try
                    {
                        ResultType result = sDataActivity.CreateCurrency(ref currency, session);

                        switch (result)
                        {
                            case ResultType.Ok:

                                LoadCurrencyData();
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

        #region dgCurrency_DeleteCommand
        protected void dgCurrency_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string sCurrencyCode = this.dgCurrency.DataKeys[e.Item.ItemIndex].ToString();

                if (sCurrencyCode != null)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    try
                    {
                        ResultType result = sDataActivity.DeleteCurrency(sCurrencyCode, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgCurrency.EditItemIndex = -1;
                                LoadCurrencyData();
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
                            this.PageMsgPanel.ShowMessage("This Currency cannot be deleted because it has been referenced by Foreign Exchange Rate.", MessagePanelControl.MessageEnumType.Alert);
                            LoadCurrencyData();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        LoadCurrencyData(); 
                        return;
                    }
                }
            }
        }
        #endregion
    }
}
