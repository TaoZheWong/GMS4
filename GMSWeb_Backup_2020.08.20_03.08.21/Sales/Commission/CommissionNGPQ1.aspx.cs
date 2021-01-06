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
using System.Collections;

namespace GMSWeb.Sales.Commission
{
    public partial class CommissionNGPQ : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Sales"); 
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            70);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                //preload
                LoadData();
            }
            
            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS/scripts/popcalendar.js""></script>
            <script language=""javascript"" type=""text/javascript"">
            var mode = ""add"";
            function FillData() 
            {
                var selObj = document.getElementById('"; 
                
                javaScript += lsbAccounts.ClientID + "')"; 
                javaScript += @"
                if (mode == ""add"")
                {
                    var str = """";
                    for (var i = 0; i < selObj.options.length; i++) 
                    {
                        if (selObj.options[i].selected) 
                        {
                           str += selObj.options[i].value + "", ""; 
                        }
                    }
                    
                    if (str.length > 0)
                    {
                        str = str.substr(0, str.length - 2);
                    }"; 
                    
                    javaScript += "document.getElementById(document.getElementById('" + txtBoxID.ClientID + "').value).value = str;"; 
                    javaScript += "document.getElementById(document.getElementById('" + txtHiddenID.ClientID + "').value).value = str;"; 
                    
                    javaScript += @"
                    return;
                }
                if (mode == ""edit"")
                {
                    var str = """";
                    for (var i = 0; i < selObj.options.length; i++) 
                    {
                        if (selObj.options[i].selected) 
                        {
                           str += selObj.options[i].value + "", ""; 
                        }
                    }
                    
                    if (str.length > 0)
                    {
                        str = str.substr(0, str.length - 2);
                    }"; 
                    
                    javaScript += "document.getElementById(document.getElementById('" + txtEditBoxID.ClientID + "').value).value = str;"; 
                    javaScript += "document.getElementById(document.getElementById('" + txtEditHiddenID.ClientID + "').value).value = str;";
                    javaScript += @"
                    
                    return;
                }
            }
            
            function selectAccounts(obj)
            {
                var selObj = "; javaScript += "document.getElementById('"+ lsbAccounts.ClientID + "');";
                javaScript += @"
                for (var i = 0; i < selObj.options.length; i++) 
                {
                    if (selObj.options[i].selected) 
                    {
                       selObj.options[i].selected = false;
                    }
                }
                if (mode == ""add"")
                {
                    var hidAccountListID = obj.id.replace(""lnkAddAccount"", ""hidAccountList"");
                    var hidAccountList = document.getElementById(hidAccountListID).value;
                    var accountList = hidAccountList.split("", "");
                    for (var i = 0; i < accountList.length; i++) 
                    {
                        for (var j = 0; j < selObj.options.length; j++) 
                        {
                            if (selObj.options[j].value == accountList[i]) 
                            {
                               selObj.options[j].selected = true;
                            }
                        }
                    }
                }
                if (mode == ""edit"")
                {
                    var hidEditAccountListID = obj.id.replace(""lnkEditAccount"", ""hidEditAccountList"");
                    var hidEditAccountList = document.getElementById(hidEditAccountListID).value;
                    var accountList = hidEditAccountList.split("", "");
                    for (var i = 0; i < accountList.length; i++) 
                    {
                        for (var j = 0; j < selObj.options.length; j++) 
                        {
                            if (selObj.options[j].value == accountList[i]) 
                            {
                               selObj.options[j].selected = true;
                            }
                        }
                    }
                }
            }
            </script>"; 

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);

            PopulateAccountListRepeater();
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            IList<GMSCore.Entity.SalesPersonMaster> lstSalesMaster = null;
            try
            {
                lstSalesMaster = new SystemDataActivity().RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterName(session.CompanyId);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lstSalesMaster != null && lstSalesMaster.Count > 0)
            {
                foreach (SalesPersonMaster sMaster in lstSalesMaster)
                {
                    IList<SalesPersonMapping> lstSalesMapping = new SystemDataActivity().RetrieveAllSalesPersonMappingListBySalesPersonMasterIDSortBySalesPersonID(sMaster.SalesPersonMasterID);
                    foreach (SalesPersonMapping sMapping in lstSalesMapping)
                    {
                        sMaster.AccountList += sMapping.SalesPersonID + ", ";
                    }
                    if (sMaster.AccountList.Length > 0)
                    {
                        sMaster.AccountList = sMaster.AccountList.TrimEnd(' ');
                        sMaster.AccountList = sMaster.AccountList.TrimEnd(',');
                    }

                }

                if (endIndex < lstSalesMaster.Count)
                    this.lblSearchSummary.Text = "Salesman List" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstSalesMaster.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Salesman List" + " " + startIndex.ToString() + " - " +
                        lstSalesMaster.Count.ToString() + " " + "of" + " " + lstSalesMaster.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = lstSalesMaster;
            this.dgData.DataBind();
        }
        #endregion

        #region PopulateAccountListRepeater
        private void PopulateAccountListRepeater()
        {
            LogSession session = base.GetSessionInfo();
            IList<SalesPerson> lstSalesPerson = null;
            lstSalesPerson = new SystemDataActivity().RetrieveAllSalesPersonListByCompanyIDSortBySalesPersonName(session.CompanyId);

            if (lstSalesPerson != null && lstSalesPerson.Count > 0)
            {
                this.lsbAccounts.DataSource = lstSalesPerson;
                this.lsbAccounts.DataValueField = "SalesPersonID";
                this.lsbAccounts.DataTextField = "SalesIDName";
                this.lsbAccounts.DataBind();
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

        #region dgData_ItemDataBound
        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                TextBox txtNewAccountList = (TextBox)e.Item.FindControl("txtNewAccountList");
                if (txtNewAccountList != null)
                {
                    txtBoxID.Text = txtNewAccountList.ClientID;
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(txtNewAccountList, txtNewAccountList.GetType(), "script1", "<script type=\"text/javascript\"> var txtNewAccountList = '" + txtNewAccountList.ClientID + "';</script>", false);
                }

                HtmlInputHidden hidAccountList = (HtmlInputHidden)e.Item.FindControl("hidAccountList");
                if (hidAccountList != null)
                {
                    txtHiddenID.Text = hidAccountList.ClientID;
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(txtNewAccountList, txtNewAccountList.GetType(), "script1", "<script type=\"text/javascript\"> var txtNewAccountList = '" + txtNewAccountList.ClientID + "';</script>", false);
                }
            }

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                TextBox txtEditAccountList = (TextBox)e.Item.FindControl("txtEditAccountList");
                if (txtEditAccountList != null)
                {
                    txtEditBoxID.Text = txtEditAccountList.ClientID;
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(txtNewAccountList, txtNewAccountList.GetType(), "script1", "<script type=\"text/javascript\"> var txtNewAccountList = '" + txtNewAccountList.ClientID + "';</script>", false);
                }

                HtmlInputHidden hidEditAccountList = (HtmlInputHidden)e.Item.FindControl("hidEditAccountList");
                if (hidEditAccountList != null)
                {
                    txtEditHiddenID.Text = hidEditAccountList.ClientID;
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(txtNewAccountList, txtNewAccountList.GetType(), "script1", "<script type=\"text/javascript\"> var txtNewAccountList = '" + txtNewAccountList.ClientID + "';</script>", false);
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
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            71);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

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
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            71);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            TextBox txtEditGPQ = (TextBox)e.Item.FindControl("txtEditGPQ");
            TextBox txtEditCommissionRate = (TextBox)e.Item.FindControl("txtEditCommissionRate");
            TextBox txtEditSalesPersonMasterName = (TextBox)e.Item.FindControl("txtEditSalesPersonMasterName");
            HtmlInputHidden hidSalesPersonMasterID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonMasterID");
            HtmlInputHidden hidEditAccountList = (HtmlInputHidden)e.Item.FindControl("hidEditAccountList");

            if (txtEditGPQ != null && !string.IsNullOrEmpty(txtEditGPQ.Text) && hidSalesPersonMasterID != null &&
                txtEditCommissionRate != null && !string.IsNullOrEmpty(txtEditCommissionRate.Text) && hidEditAccountList != null && !string.IsNullOrEmpty(hidEditAccountList.Value) &&
                txtEditSalesPersonMasterName != null && !string.IsNullOrEmpty(txtEditSalesPersonMasterName.Text))
            {

                GMSCore.Entity.SalesPersonMaster sMaster = SalesPersonMaster.RetrieveByKey(short.Parse(hidSalesPersonMasterID.Value));
                sMaster.GPQ = GMSUtil.ToDouble(txtEditGPQ.Text.Trim());
                sMaster.CommissionRate = GMSUtil.ToDouble(txtEditCommissionRate.Text.Trim());
                sMaster.SalesPersonMasterName = txtEditSalesPersonMasterName.Text.Trim().ToUpper();
                sMaster.ModifiedBY = session.UserId;
                sMaster.ModifiedDate = DateTime.Now;

                try
                {
                    ResultType result = new SalesPersonMasterActivity().UpdateSalesPersonMaster(ref sMaster, session);

                    switch (result)
                    {
                        case ResultType.Ok:
                            try
                            {
                                new SalesPersonMappingActivity().DeleteSalesPersonMappingBySalesPersonMasterID(sMaster.SalesPersonMasterID, session);
                                string[] AccountList = hidEditAccountList.Value.Split(',');
                                foreach (string account in AccountList)
                                {
                                    SalesPersonMapping sMapping = new SalesPersonMapping();
                                    sMapping.CoyID = session.CompanyId;
                                    sMapping.SalesPersonMasterID = sMaster.SalesPersonMasterID;
                                    sMapping.SalesPersonID = account.Trim();
                                    new SalesPersonMappingActivity().CreateSalesPersonMapping(ref sMapping, session);
                                }
                            }
                            catch (Exception ex)
                            {
                                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                                return;
                            }
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
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                71);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                TextBox txtNewSalesPersonMasterName = (TextBox)e.Item.FindControl("txtNewSalesPersonMasterName");
                TextBox txtNewGPQ = (TextBox)e.Item.FindControl("txtNewGPQ");
                TextBox txtNewCommissionRate = (TextBox)e.Item.FindControl("txtNewCommissionRate");
                HtmlInputHidden hidAccountList = (HtmlInputHidden)e.Item.FindControl("hidAccountList");

                if (txtNewSalesPersonMasterName != null && !string.IsNullOrEmpty(txtNewSalesPersonMasterName.Text) && txtNewGPQ != null && !string.IsNullOrEmpty(txtNewGPQ.Text) &&
                    txtNewCommissionRate != null && !string.IsNullOrEmpty(txtNewCommissionRate.Text) && hidAccountList != null && !string.IsNullOrEmpty(hidAccountList.Value))
                {
                    try
                    {
                        //check if newly inserted GPQ already exist
                        GMSCore.Entity.SalesPersonMaster existingSMaster = new SalesPersonMasterActivity().RetrieveSalesPersonMasterByCoyIDSalesPersonMasterName(session.CompanyId, txtNewSalesPersonMasterName.Text.Trim());
                        if (existingSMaster != null)
                        {
                            this.PageMsgPanel.ShowMessage("Processing error of type : Record for this salesman already exists.", MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }

                        GMSCore.Entity.SalesPersonMaster sMaster = new GMSCore.Entity.SalesPersonMaster();
                        sMaster.CoyID = session.CompanyId;
                        sMaster.SalesPersonMasterName = txtNewSalesPersonMasterName.Text.Trim();
                        sMaster.GPQ = GMSUtil.ToDouble(txtNewGPQ.Text.Trim());
                        sMaster.CommissionRate = GMSUtil.ToDouble(txtNewCommissionRate.Text.Trim());
                        sMaster.CreatedBy = session.UserId;
                        sMaster.CreatedDate = DateTime.Now;

                        ResultType result = new SalesPersonMasterActivity().CreateSalesPersonMaster(ref sMaster, session);
                        switch (result)
                        {
                            case ResultType.Ok:
                                string[] AccountList = hidAccountList.Value.Split(',');
                                foreach (string account in AccountList)
                                {
                                    try
                                    {
                                        SalesPersonMapping sMapping = new SalesPersonMapping();
                                        sMapping.CoyID = session.CompanyId;
                                        sMapping.SalesPersonMasterID = sMaster.SalesPersonMasterID;
                                        sMapping.SalesPersonID = account.Trim();
                                        new SalesPersonMappingActivity().CreateSalesPersonMapping(ref sMapping, session);
                                    }
                                    catch (Exception ex)
                                    {
                                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                                        return;
                                    }
                                }
                                LoadData();
                                PopulateAccountListRepeater();
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
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                71);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                HtmlInputHidden hidSalesPersonMasterID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonMasterID");

                if (hidSalesPersonMasterID != null)
                {
                    SalesPersonMasterActivity gActivity = new SalesPersonMasterActivity();

                    try
                    {
                        ResultType result = gActivity.DeleteSalesPersonMaster(short.Parse(hidSalesPersonMasterID.Value), session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                new SalesPersonMappingActivity().DeleteSalesPersonMappingBySalesPersonMasterID(short.Parse(hidSalesPersonMasterID.Value), session);
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
    }
}
