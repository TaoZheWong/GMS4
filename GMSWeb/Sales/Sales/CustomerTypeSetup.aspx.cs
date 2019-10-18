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

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System.Data.SqlClient;

namespace GMSWeb.Sales.Sales
{
    public partial class CustomerTypeSetup : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentLink = "Sales";
            lblPageHeader.Text = "Sales";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                if (Request.Params["CurrentLink"].ToString().Trim() == "Sales")
                    lblPageHeader.Text = "Administration";
                else
                    lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            158);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!Page.IsPostBack)
            {
                //preload
                dgData.CurrentPageIndex = 0;
                LoadData();
            }

            string javaScript =
            @"
            <script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>

            <script type=""text/javascript"">
            function SelectOthers(chkbox)
	        {
	            var prefix = chkbox.id.substring(0,chkbox.id.lastIndexOf(""_"")+1);
	            if (chkbox.checked)
	            {
	                document.getElementById(prefix+""spanSalesPersonMasterName"").style.visibility = 'hidden';
	                document.getElementById(prefix+""spanArea"").style.visibility = 'hidden';
	            } else
	            {
	                document.getElementById(prefix+""spanSalesPersonMasterName"").style.visibility = 'visible';
	                document.getElementById(prefix+""spanArea"").style.visibility = 'visible';
	            }
	        } 
        	 
            </script>
            ";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        //#region LoadData
        //private void LoadData()
        //{
        //    LogSession session = base.GetSessionInfo();
        //    IList<GMSCore.Entity.AccountClass> lstEE = null;
        //    lstEE = new SystemDataActivity().RetrieveAccountClass(session.CompanyId);
        //    this.dgData.DataSource = lstEE;
        //    this.dgData.DataBind();

        //}
        //#endregion

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC classdacl = new GMSGeneralDALC();
            DataSet dsClass = new DataSet();
            classdacl.GetClassNameWithShortName(session.CompanyId, ref dsClass);
            this.dgData.DataSource = dsClass.Tables[0];
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

            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewClassName = (DropDownList)e.Item.FindControl("ddlNewClassName");
                //DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
                if (ddlNewClassName != null)
                {

                    GMSGeneralDALC classdacl = new GMSGeneralDALC();
                    DataSet dsClass = new DataSet();
                    classdacl.GetClassNameWithNoShortName(session.CompanyId, ref dsClass);
                    ddlNewClassName.DataSource = dsClass.Tables[0];
                    ddlNewClassName.DataBind();

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
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                159);
                if (uAccess == null)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "You don't have access." + "\");</script>", false);
                    return;
                }

                DropDownList ddlNewClassName = (DropDownList)e.Item.FindControl("ddlNewClassName");
                TextBox txtNewShortName = (TextBox)e.Item.FindControl("txtNewShortName");
                TextBox txtNewValue = (TextBox)e.Item.FindControl("txtNewValue");


                if (ddlNewClassName != null && !string.IsNullOrEmpty(ddlNewClassName.SelectedValue) &&
                   txtNewShortName != null && !string.IsNullOrEmpty(txtNewShortName.Text) &&
                   txtNewValue != null && !string.IsNullOrEmpty(txtNewValue.Text))
                {
                    IList<GMSCore.Entity.AccountClass> lstData = null;
                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    lstData = sDataActivity.RetrieveAccountClass(session.CompanyId, txtNewShortName.Text.Trim(), int.Parse(txtNewValue.Text.Trim()));
                    if (lstData.Count <= 0)
                    {

                        GMSCore.Entity.AccountClass ac = GMSCore.Entity.AccountClass.RetrieveByKey(session.CompanyId, ddlNewClassName.SelectedValue);
                        ac.ShortName = txtNewShortName.Text.Trim();
                        ac.Value = int.Parse(txtNewValue.Text.Trim());

                    try
                    {
                        ac.Save();
                        this.dgData.EditItemIndex = -1;
                        this.PageMsgPanel.ShowMessage("Customer Type has been created successfully.", MessagePanelControl.MessageEnumType.Info);
                            LoadData();

                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
                else
                {
                        this.PageMsgPanel.ShowMessage("Short Name or Value already exists.", MessagePanelControl.MessageEnumType.Info);
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
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            159);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            //DropDownList ddlEditClassName = (DropDownList)e.Item.FindControl("ddlEditClassName");
            TextBox txtEditShortName = (TextBox)e.Item.FindControl("txtEditShortName");
            TextBox txtEditValue = (TextBox)e.Item.FindControl("txtEditValue");
            HtmlInputHidden hidClassID = (HtmlInputHidden)e.Item.FindControl("hidClassID");

            if (txtEditShortName != null && !string.IsNullOrEmpty(txtEditShortName.Text) && hidClassID != null &&
                txtEditValue != null && !string.IsNullOrEmpty(txtEditValue.Text))
            {
                IList<GMSCore.Entity.AccountClass> lstData = null;
                SystemDataActivity sDataActivity = new SystemDataActivity();
                lstData = sDataActivity.RetrieveAccountClassByClassID(session.CompanyId, hidClassID.Value, txtEditShortName.Text.Trim(), int.Parse(txtEditValue.Text.Trim()));
                if (lstData.Count <= 0)
                {

                GMSCore.Entity.AccountClass ac = GMSCore.Entity.AccountClass.RetrieveByKey(session.CompanyId, hidClassID.Value);
                ac.ShortName = txtEditShortName.Text.Trim();
                ac.Value = int.Parse(txtEditValue.Text.Trim());
                try
                {
                    ac.Save();
                    this.dgData.EditItemIndex = -1;
                    this.PageMsgPanel.ShowMessage("Customer Type has been updated successfully.", MessagePanelControl.MessageEnumType.Info);
                        LoadData();

                        GMSGeneralDALC gg = new GMSGeneralDALC();
                        gg.UpdateCustomerType(session.CompanyId, 2019);
                    }
                catch (Exception ex)
                {
                    this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
                }
                else
                {
                    this.PageMsgPanel.ShowMessage("Short Name or Value already exists.", MessagePanelControl.MessageEnumType.Info);
                    return;
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
                                                                                159);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                HtmlInputHidden hidClassID = (HtmlInputHidden)e.Item.FindControl("hidClassID");

                if (hidClassID != null)
                {

                        GMSCore.Entity.AccountClass ac = GMSCore.Entity.AccountClass.RetrieveByKey(session.CompanyId, hidClassID.Value);
                        ac.ShortName = "";
                        ac.Value = 0;
                        try
                        {
                            ac.Save();
                            this.dgData.EditItemIndex = -1;
                            this.dgData.CurrentPageIndex = 0;
                            LoadData();
                        }
                        catch (Exception ex)
                        {
                           
                           this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
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
