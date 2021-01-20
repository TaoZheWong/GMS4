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
    public partial class CustomerList : GMSBasePage
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
                                                                            56);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyFinance"));

            if (!Page.IsPostBack)
            {
                //preload
                dgData.CurrentPageIndex = 0;
                LoadData();
            }
            string javaScript =
@"
<script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>
"; 
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            string name = "%";
            if (!string.IsNullOrEmpty(searchName.Text))
                name = "%" + searchName.Text.Trim() + "%";
            IList<GMSCore.Entity.BankReceiverPayer> lstBRP = null;
            try
            {
                lstBRP = new SystemDataActivity().RetrieveAllBankReceiverPayerListByPrefixByCompanyIDSortByName(name, session.CompanyId);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lstBRP != null && lstBRP.Count > 0)
            {
                if (endIndex < lstBRP.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstBRP.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstBRP.Count.ToString() + " " + "of" + " " + lstBRP.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = lstBRP;
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

                TextBox txtNewName = (TextBox)e.Item.FindControl("txtNewName");

                if (txtNewName != null && !string.IsNullOrEmpty(txtNewName.Text))
                {
                    try
                    {
                        // check if newly inserted name already exist
                        GMSCore.Entity.BankReceiverPayer existingBRP = new BankReceiverPayerActivity().RetrieveBankReceiverPayerByNameCompanyId(txtNewName.Text.Trim(), session.CompanyId);

                        if (existingBRP != null)
                        {
                            this.PageMsgPanel.ShowMessage("Processing error of type : This name already been used.", MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }

                        GMSCore.Entity.BankReceiverPayer bankRP = new GMSCore.Entity.BankReceiverPayer();
                        bankRP.CoyID = session.CompanyId;
                        bankRP.Name = txtNewName.Text.Trim();
                        ResultType result = new BankReceiverPayerActivity().CreateBankReceiverPayer(ref bankRP, session);
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
                    Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                    return;
                }

                HtmlInputHidden hidName = (HtmlInputHidden)e.Item.FindControl("hidName");

                if (hidName != null)
                {
                    BankReceiverPayerActivity bActivity = new BankReceiverPayerActivity();

                    try
                    {
                        ResultType result = bActivity.DeleteBankReceiverPayer(hidName.Value, session);

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
                            this.PageMsgPanel.ShowMessage("This name cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                            LoadData();
                            return;
                        }
                        else
                        {
                            this.PageMsgPanel.ShowMessage(exSql.Message, MessagePanelControl.MessageEnumType.Alert);
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

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
        }
        #endregion
    }
}
