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
using GMSCore.Activity;
using GMSCore.Entity;
using System.Collections.Generic;
using GMSWeb.CustomCtrl;
using System.Data.SqlClient;

namespace GMSWeb.HR.Commission
{
    public partial class KeyCustomers : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("CompanyHR");
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            109);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyHR"));

            if (!Page.IsPostBack)
            {
                //preload
                dgData.CurrentPageIndex = 0;
                LoadData();
            }

            string javaScript =
            @"
            <script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>
            ";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            IList<GMSCore.Entity.KeyCustomerAverageGP> lst = null;
            short companyID = session.CompanyId;
            try
            {
                lst = new SystemDataActivity().RetrieveKeyCustomersByCoyID(companyID);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lst != null && lst.Count > 0)
            {
                if (endIndex < lst.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lst.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lst.Count.ToString() + " " + "of" + " " + lst.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = lst;
            this.dgData.DataBind();
        }
        #endregion

        #region dgData_ItemDataBound
        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                GMSCore.Entity.KeyCustomerAverageGP kc = (GMSCore.Entity.KeyCustomerAverageGP)e.Item.DataItem;
                DropDownList ddlEditSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlEditSalesPersonMasterName");
                if (ddlEditSalesPersonMasterName != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in currency dropdown list
                    IList<GMSCore.Entity.SalesPersonMaster> lstSalesPerson = null;
                    lstSalesPerson = sDataActivity.RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterName(session.CompanyId);
                    ddlEditSalesPersonMasterName.DataSource = lstSalesPerson;
                    ddlEditSalesPersonMasterName.DataBind();
                    ddlEditSalesPersonMasterName.SelectedValue = kc.SalesPersonMasterID.ToString();
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
                if (ddlNewSalesPersonMasterName != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in currency dropdown list
                    IList<GMSCore.Entity.SalesPersonMaster> lstSalesPerson = null;
                    lstSalesPerson = sDataActivity.RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterName(session.CompanyId);
                    ddlNewSalesPersonMasterName.DataSource = lstSalesPerson;
                    ddlNewSalesPersonMasterName.DataBind();
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

        #region dgData_CreateCommand
        protected void dgData_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                    return;
                }

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                110);
                if (uAccess == null)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "You don't have access." + "\");</script>", false);
                    return;
                }

                DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
                TextBox txtNewAccountCode = (TextBox)e.Item.FindControl("txtNewAccountCode");
                TextBox txtNewAverageGP = (TextBox)e.Item.FindControl("txtNewAverageGP");

                if (ddlNewSalesPersonMasterName != null && txtNewAccountCode != null && txtNewAverageGP != null)
                {
                    try
                    {
                        string accountCode = "";

                        if (txtNewAccountCode.Text.Trim() == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('Please input customer!')", true);
                            txtNewAccountCode.Text = "";
                            return;
                        }
                        else
                        {
                            accountCode = txtNewAccountCode.Text.Trim().Split(' ')[0];
                        }

                        if (ddlNewSalesPersonMasterName.SelectedValue.Trim() == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('No Salesman has been chosen!')", true);
                            return;
                        }

                        if (txtNewAverageGP.Text.Trim() == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('lease input Average  GP!')", true);
                            return;
                        }

                        DataSet ds = new DataSet();
                        A21Account acct = A21Account.RetrieveByKey(session.CompanyId, accountCode.Trim());
                        if (acct != null && acct.AccountType == "C")
                        {
                            GMSCore.Entity.KeyCustomerAverageGP keyCust = new GMSCore.Entity.KeyCustomerAverageGP();
                            keyCust.CoyID = session.CompanyId;
                            keyCust.AccountCode = accountCode.Trim();
                            keyCust.SalesPersonMasterID = GMSUtil.ToShort(ddlNewSalesPersonMasterName.SelectedValue.Trim());
                            keyCust.AverageGP = GMSUtil.ToDouble(txtNewAverageGP.Text.Trim());

                            keyCust.Save();
                            LoadData();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('Customer is not found!')", true);
                            txtNewAccountCode.Text = "";
                            LoadData();
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

        #region dgData_UpdateCommand
        protected void dgData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            110);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyHR"));

            DropDownList ddlEditSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlEditSalesPersonMasterName");
            TextBox txtEditAverageGP = (TextBox)e.Item.FindControl("txtEditAverageGP");
            HtmlInputHidden hidSalesPersonMasterID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonMasterID");
            HtmlInputHidden hidAccountCode = (HtmlInputHidden)e.Item.FindControl("hidAccountCode");

            if (ddlEditSalesPersonMasterName != null && txtEditAverageGP != null && hidSalesPersonMasterID != null && hidAccountCode != null)
            {
                GMSCore.Entity.KeyCustomerAverageGP kc = GMSCore.Entity.KeyCustomerAverageGP.RetrieveByKey(session.CompanyId,
                                                                                                    hidAccountCode.Value.Trim());

                if (kc != null)
                {
                    kc.SalesPersonMasterID = GMSUtil.ToShort(ddlEditSalesPersonMasterName.SelectedValue.Trim());
                    kc.AverageGP = GMSUtil.ToDouble(txtEditAverageGP.Text.Trim());

                    try
                    {
                        kc.Save();
                        this.dgData.EditItemIndex = -1;
                        LoadData();
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
                    Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                110);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("CompanyHR"));

                HtmlInputHidden hidAccountCode = (HtmlInputHidden)e.Item.FindControl("hidAccountCode");

                if (hidAccountCode != null)
                {
                    try
                    {
                        GMSCore.Entity.KeyCustomerAverageGP kc = GMSCore.Entity.KeyCustomerAverageGP.RetrieveByKey(session.CompanyId, hidAccountCode.Value.Trim());
                        kc.Delete();
                        kc.Resync();
                        this.dgData.EditItemIndex = -1;
                        this.dgData.CurrentPageIndex = 0;
                        LoadData();
                    }
                    catch (SqlException exSql)
                    {
                        if (exSql.Number == 547)
                        {
                            this.PageMsgPanel.ShowMessage("This record cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
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

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion
    }
}
