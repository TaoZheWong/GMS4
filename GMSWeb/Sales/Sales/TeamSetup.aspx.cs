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
    public partial class TeamSetup : GMSBasePage
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
                                                                            152);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!Page.IsPostBack)
            {
                //preload
                dgData.CurrentPageIndex = 0;
                DataGrid1.CurrentPageIndex = 0;
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

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            IList<GMSCore.Entity.SalesGroupTeam> lstEE = null;
            lstEE = new SystemDataActivity().RetrieveTeamSetupSalesGroupTeam(session.CompanyId);
            this.dgData.DataSource = lstEE;
            this.dgData.DataBind();

            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            ggdal.RetrieveTeamSetupSalesTeamSalesPerson(session.CompanyId, ref ds);
            this.DataGrid1.DataSource = ds.Tables[0];
            this.DataGrid1.DataBind();

            SystemDataActivity sDataActivity = new SystemDataActivity();
            IList<GMSCore.Entity.SalesGroupTeam> lstSalesGroupTeam = null;
            lstSalesGroupTeam = sDataActivity.RetrieveTeamSetupSalesGroupTeam(session.CompanyId);
            this.ddlSearchTeamName.DataSource = lstSalesGroupTeam;
            this.ddlSearchTeamName.DataBind();
        }
        #endregion

        #region LoadTeamData
        private void LoadTeamData()
        {
            LogSession session = base.GetSessionInfo();
            IList<GMSCore.Entity.SalesGroupTeam> lstEE = null;
            lstEE = new SystemDataActivity().RetrieveTeamSetupSalesGroupTeam(session.CompanyId);
            this.dgData.DataSource = lstEE;
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

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                GMSCore.Entity.SalesGroupTeam ee = (GMSCore.Entity.SalesGroupTeam)e.Item.DataItem;
                DropDownList ddlEditGroupName = (DropDownList)e.Item.FindControl("ddlEditGroupName");                

                if (ddlEditGroupName != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in sales group dropdown list
                    IList<GMSCore.Entity.SalesGroup> lstSalesGroup = null;

                    lstSalesGroup = sDataActivity.RetrieveTeamSetupSalesGroup(session.CompanyId);
                    /*
                    SalesGroup sg = new SalesGroup();
                    sg.GroupID = 0;
                    sg.GroupName = "N/A";
                    sg.GroupShortName = "N/A";
                    lstSalesGroup.Add(sg);
                    */
                    ddlEditGroupName.DataSource = lstSalesGroup;
                    ddlEditGroupName.DataBind();
                    ddlEditGroupName.SelectedValue = ee.GroupID.ToString();
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewGroupName = (DropDownList)e.Item.FindControl("ddlNewGroupName");
                
                if (ddlNewGroupName != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in currency dropdown list
                    IList<GMSCore.Entity.SalesGroup> lstSalesGroup = null;
                    lstSalesGroup = sDataActivity.RetrieveTeamSetupSalesGroup(session.CompanyId);
                    /*
                    SalesGroup sg = new SalesGroup();
                    sg.GroupID = 0;
                    sg.GroupName = "N/A";
                    sg.GroupShortName = "N/A";
                    lstSalesGroup.Add(sg);
                    */
                    ddlNewGroupName.DataSource = lstSalesGroup;
                    ddlNewGroupName.DataBind();
                  
                }

                /*DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
                if (ddlNewSalesPersonMasterName != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();                

                    // fill in salespersonid dropdown list
                    IList<SalesPerson> lstSalesPerson = null;
                    lstSalesPerson = sDataActivity.RetrieveAllSalesPersonByCompanyIDSortBySalesPersonID(GMSUtil.ToShort(session.CompanyId));
                    ddlNewSalesPersonMasterName.DataSource = lstSalesPerson;
                    ddlNewSalesPersonMasterName.DataBind();
                }*/

                DropDownList ddlNewTeamName = (DropDownList)e.Item.FindControl("ddlNewTeamName");
                if (ddlNewTeamName != null)
                {
                    IList<GMSCore.Entity.SalesGroupTeam> lstSalesGroupTeam = null;
                    lstSalesGroupTeam = SalesGroupTeam.RetrieveAll();
                    ddlNewTeamName.DataSource = lstSalesGroupTeam;
                    ddlNewTeamName.DataBind();
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
                                                                                153);
                if (uAccess == null)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "You don't have access." + "\");</script>", false);
                    return;
                }

                DropDownList ddlNewGroupName = (DropDownList)e.Item.FindControl("ddlNewGroupName");
                TextBox txtNewTeamName = (TextBox)e.Item.FindControl("txtNewTeamName");
                TextBox txtNewTeamShortName = (TextBox)e.Item.FindControl("txtNewTeamShortName");


                if (ddlNewGroupName != null && !string.IsNullOrEmpty(ddlNewGroupName.SelectedValue) &&
                   txtNewTeamName != null && !string.IsNullOrEmpty(txtNewTeamName.Text) &&
                     txtNewTeamShortName != null && !string.IsNullOrEmpty(txtNewTeamShortName.Text))
                { 
                    try
                    {

                        GMSCore.Entity.SalesGroupTeam sgt = new GMSCore.Entity.SalesGroupTeam();
                        sgt.CoyID = session.CompanyId;
                        sgt.GroupID = short.Parse(ddlNewGroupName.SelectedValue);
                        sgt.TeamName = txtNewTeamName.Text.Trim();
                        sgt.TeamShortName = txtNewTeamShortName.Text.Trim();
                        sgt.Save();
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
                                                                            153);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            DropDownList ddlEditGroupName = (DropDownList)e.Item.FindControl("ddlEditGroupName");
            TextBox txtEditTeamName = (TextBox)e.Item.FindControl("txtEditTeamName");
            TextBox txtEditTeamShortName = (TextBox)e.Item.FindControl("txtEditTeamShortName");
            HtmlInputHidden hidTeamID = (HtmlInputHidden)e.Item.FindControl("hidTeamID");

            if (txtEditTeamName != null && !string.IsNullOrEmpty(txtEditTeamName.Text) && hidTeamID != null &&
                txtEditTeamShortName != null && !string.IsNullOrEmpty(txtEditTeamShortName.Text) && ddlEditGroupName != null )
            {

                GMSCore.Entity.SalesGroupTeam ee = GMSCore.Entity.SalesGroupTeam.RetrieveByKey(int.Parse(hidTeamID.Value));
                ee.GroupID = int.Parse(ddlEditGroupName.SelectedValue);
                ee.TeamName = txtEditTeamName.Text.Trim();
                ee.TeamShortName = txtEditTeamShortName.Text.Trim();
                try
                {
                    ee.Save();
                    this.dgData.EditItemIndex = -1;
                    //chkSearchOthers.Checked = chkEditOthers.Checked;
                    LoadData();
                }
                catch (Exception ex)
                {
                    this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
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
                                                                                152);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                HtmlInputHidden hidTeamID = (HtmlInputHidden)e.Item.FindControl("hidTeamID");

                if (hidTeamID != null)
                {
                    try
                    {
                        SystemDataActivity sDataActivity = new SystemDataActivity();
                        GMSCore.Entity.SalesGroupTeam ee = sDataActivity.RetrieveTeamSetupSalesGroupTeamByTeamID(session.CompanyId, short.Parse(hidTeamID.Value));
                        ee.Delete();
                        ee.Resync();
                        this.dgData.EditItemIndex = -1;
                        this.dgData.CurrentPageIndex = 0;
                        LoadData();
                    }
                    catch (SqlException exSql)
                    {
                        if (exSql.Number == 547)
                        {
                            this.PageMsgPanel.ShowMessage("This record cannot be deleted because sales person has been assigned to this team.", MessagePanelControl.MessageEnumType.Alert);
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
            this.DataGrid1.CurrentPageIndex = 0;
            LoadTeamData();
            RetrieveSalesPerson();
        }
        #endregion

        #region DataGrid1 PageIndexChanged event handling
        protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        #region DataGrid1_ItemDataBound
        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            /*if (e.Item.ItemType == ListItemType.EditItem)
            {
                GMSCore.Entity.SalesTeamSalesPerson ee = (GMSCore.Entity.SalesTeamSalesPerson)e.Item.DataItem;
                DropDownList ddlEditTeamName = (DropDownList)e.Item.FindControl("ddlEditTeamName");
                if (ddlEditTeamName != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in sales group dropdown list
                    IList<GMSCore.Entity.SalesGroupTeam> lsSalesGroupTeam = null;
                    lsSalesGroupTeam = sDataActivity.RetrieveTeamSetupSalesGroupTeam(session.CompanyId);
                    ddlEditTeamName.DataSource = lsSalesGroupTeam;
                    ddlEditTeamName.DataBind();
                    ddlEditTeamName.SelectedValue = ee.TeamID.ToString();
                }
            }
            else*/ if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewTeamName = (DropDownList)e.Item.FindControl("ddlNewTeamName");
                if (ddlNewTeamName != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    IList<GMSCore.Entity.SalesGroupTeam> lstSalesGroupTeam = null;
                    lstSalesGroupTeam = sDataActivity.RetrieveTeamSetupSalesGroupTeam(session.CompanyId);
                    if(lstSalesGroupTeam.Count == 0)
                    {
                        DataGrid1.ShowFooter = false;
                    }else
                    {
                        DataGrid1.ShowFooter = true;
                    }
                    ddlNewTeamName.DataSource = lstSalesGroupTeam;
                    ddlNewTeamName.DataBind();
                    //ddlNewTeamName.SelectedValue = lstSalesGroupTeam.
                }

                /*DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
                if (ddlNewSalesPersonMasterName != null)
                {
                    //SystemDataActivity sDataActivity = new SystemDataActivity();
                    // fill in salespersonid dropdown list
                    IList<SalesPerson> lstSalesPerson = null;
                    lstSalesPerson = sDataActivity.RetrieveAllSalesPersonByCompanyIDSortBySalesPersonID(GMSUtil.ToShort(session.CompanyId));
                    ddlNewSalesPersonMasterName.DataSource = lstSalesPerson;

                    GMSGeneralDALC ggdal = new GMSGeneralDALC();
                    DataSet dsTeam = new DataSet();
                    ggdal.RetrieveTeamSetupSalesGroupWithNoShortName(session.CompanyId, 
                        ref dsTeam);
                    ddlNewSalesPersonMasterName.DataSource = dsTeam.Tables[0];
                    ddlNewSalesPersonMasterName.DataBind();
                }*/
            }
           
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region DataGrid1_EditCommand
        protected void DataGrid1_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.DataGrid1.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }
        #endregion

        #region DataGrid1_CancelCommand
        protected void DataGrid1_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.DataGrid1.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region DataGrid1_CreateCommand
        protected void DataGrid1_CreateCommand(object sender, DataGridCommandEventArgs e)
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
                                                                                153);
                if (uAccess == null)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "You don't have access." + "\");</script>", false);
                    return;
                }

                DropDownList ddlNewTeamName = (DropDownList)e.Item.FindControl("ddlNewTeamName");
                DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
                TextBox txtNewSalesPersonShortName = (TextBox)e.Item.FindControl("txtNewSalesPersonShortName");

                if (ddlNewTeamName != null && !string.IsNullOrEmpty(ddlNewTeamName.SelectedValue) &&
                   ddlNewSalesPersonMasterName != null && !string.IsNullOrEmpty(ddlNewSalesPersonMasterName.SelectedValue) &&
                     txtNewSalesPersonShortName != null && !string.IsNullOrEmpty(txtNewSalesPersonShortName.Text))
                { 
                    try
                    {
                        /*GMSGeneralDALC ggdal = new GMSGeneralDALC();
                        DataSet ds = new DataSet();
                        DataSet ds1 = new DataSet();
                        ggdal.RetrieveSalesPersonMasterNameByID(session.CompanyId, ddlNewSalesPersonMasterName.SelectedValue,
                            ref ds1);
                        string teamName = ds1.Tables[0].Rows[0]["DivisionID"].ToString();
                        ggdal.RetrieveTeamSetupIDByTeamName(session.CompanyId, teamName, ref ds);
                        short teamID = short.Parse(ds.Tables[0].Rows[0]["TeamID"].ToString());*/ 
                        GMSCore.Entity.SalesTeamSalesPerson sgt = new GMSCore.Entity.SalesTeamSalesPerson();
                        sgt.CoyID = session.CompanyId;
                        sgt.TeamID = int.Parse(ddlNewTeamName.SelectedValue);
                        sgt.SalesPersonID = ddlNewSalesPersonMasterName.SelectedValue;
                        sgt.SalesPersonShortName = txtNewSalesPersonShortName.Text.Trim();
                        sgt.Save();
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

        #region DataGrid1_UpdateCommand
        protected void DataGrid1_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            153);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            DropDownList ddlEditTeamName = (DropDownList)e.Item.FindControl("ddlEditTeamName");
            TextBox txtEditSalesPersonShortName = (TextBox)e.Item.FindControl("txtEditSalesPersonShortName");
            HtmlInputHidden hidSalesPersonID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonID");

            if (hidSalesPersonID != null && txtEditSalesPersonShortName != null && !string.IsNullOrEmpty(txtEditSalesPersonShortName.Text) 
                && ddlEditTeamName != null)
            {

                GMSCore.Entity.SalesTeamSalesPerson ee = GMSCore.Entity.SalesTeamSalesPerson.RetrieveByKey(session.CompanyId , hidSalesPersonID.Value.ToString());
                //ee.TeamID = int.Parse(ddlEditTeamName.SelectedValue);
                ee.SalesPersonShortName = txtEditSalesPersonShortName.Text.Trim();
                try
                {
                    ee.Save();
                    this.DataGrid1.EditItemIndex = -1;
                    //chkSearchOthers.Checked = chkEditOthers.Checked;
                    LoadData();
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }
        }
        #endregion

        #region DataGrid1_DeleteCommand
        protected void DataGrid1_DeleteCommand(object sender, DataGridCommandEventArgs e)
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
                                                                                153);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                HtmlInputHidden hidSalesPersonID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonID");

                if (hidSalesPersonID != null)
                {
                    try
                    {
                        SystemDataActivity sDataActivity = new SystemDataActivity();
                        GMSCore.Entity.SalesTeamSalesPerson ee = sDataActivity.RetrieveTeamSetupSalesTeamSalesPersonByTeamID(session.CompanyId, hidSalesPersonID.Value);
                        ee.Delete();
                        ee.Resync();
                        this.DataGrid1.EditItemIndex = -1;
                        this.DataGrid1.CurrentPageIndex = 0;
                        LoadData();
                    }
                    catch (SqlException exSql)
                    {
                        if (exSql.Number == 547)
                        {
                            this.PageMsgPanel.ShowMessage("This record cannot be deleted because sales person has been assgined in this group.", MessagePanelControl.MessageEnumType.Alert);
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

        #region Populate Sales Person DDL
        protected void PopulateSalesPersonddl(short companyId)
        {
            ContentPlaceHolder MainContent = Page.Master.FindControl("ContentPlaceHolderMain") as ContentPlaceHolder;
            DropDownList ddlNewSalesPersonMasterName = (DropDownList)MainContent.FindControl("ddlNewSalesPersonMasterName");
            //DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
            if (ddlNewSalesPersonMasterName != null)
            {
                SystemDataActivity sDataActivity = new SystemDataActivity();

                // fill in currency dropdown list
                IList<GMSCore.Entity.SalesPersonMaster> lstSalesPerson = null;
                lstSalesPerson = sDataActivity.RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterName(companyId);
                ddlNewSalesPersonMasterName.DataSource = lstSalesPerson;
                ddlNewSalesPersonMasterName.DataBind();
            }
        }
        #endregion

        #region Populate Sales Group
        protected void PopulateSalesGroup(short companyId)
        {
            ContentPlaceHolder MainContent = Page.Master.FindControl("ContentPlaceHolderMain") as ContentPlaceHolder;
            DropDownList ddlNewGroupName = (DropDownList)MainContent.FindControl("ddlNewGroupName");
            //DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
            if (ddlNewGroupName != null)
            {
                SystemDataActivity sDataActivity = new SystemDataActivity();

                // fill in currency dropdown list
                IList<GMSCore.Entity.SalesGroup> lstSalesGroup = null;
                lstSalesGroup = sDataActivity.RetrieveTeamSetupSalesGroup(companyId);
                ddlNewGroupName.DataSource = lstSalesGroup;
                ddlNewGroupName.DataBind();
            }
        }
        #endregion

        #region Populate Sales Group Team
        protected void PopulateSalesGroupTeam(short companyId)
        {
            ContentPlaceHolder MainContent = Page.Master.FindControl("ContentPlaceHolderMain") as ContentPlaceHolder;
            DropDownList ddlNewTeamName = (DropDownList)MainContent.FindControl("ddlNewTeamName ");
            //DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
            if (ddlNewTeamName != null)
            {
                SystemDataActivity sDataActivity = new SystemDataActivity();

                // fill in currency dropdown list
                IList<GMSCore.Entity.SalesGroupTeam> lstSalesGroupTeam = null;
                lstSalesGroupTeam = sDataActivity.RetrieveTeamSetupSalesGroupTeam(companyId);
                ddlNewTeamName.DataSource = lstSalesGroupTeam;
                ddlNewTeamName.DataBind();
            }
        }
        #endregion
        
        #region RetrieveSalesPerson
        private void RetrieveSalesPerson()
        {

            LogSession session = base.GetSessionInfo();
            GMSGeneralDALC ggdal = new GMSGeneralDALC();
            DataSet ds = new DataSet();

            string TeamCode = "";
            string SalesPersonName = "";
            string SalesPersonShortName = "";
            if (string.IsNullOrEmpty(ddlSearchTeamName.SelectedValue.Trim()) && string.IsNullOrEmpty(txtSalesPersonName.Text.Trim()) && string.IsNullOrEmpty(txtShortName.Text.Trim()))
            {
                this.MsgPanel2.ShowMessage("Please input data to search", MessagePanelControl.MessageEnumType.Alert);
                resultList.Visible = false;

            }
            else
            {
                TeamCode = "%" + ddlSearchTeamName.SelectedValue.Trim() + "%";
                SalesPersonName = "%" + txtSalesPersonName.Text.Trim() + "%";
                SalesPersonShortName = "%" + txtShortName.Text.Trim() + "%";
                resultList.Visible = true;
            }
            try
            {
                ggdal.GetSalesPersonWithShortName(session.CompanyId, TeamCode, SalesPersonName, SalesPersonShortName, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (endIndex < ds.Tables[0].Rows.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

                this.lblSearchSummary.Visible = true;
                this.DataGrid1.DataSource = ds;
                this.DataGrid1.DataBind();

            }
            else
            {
                this.lblSearchSummary.Text = "No records.";
                this.lblSearchSummary.Visible = true;
                this.DataGrid1.DataSource = null;
                this.DataGrid1.DataBind();
                return;

            }
        }
        #endregion

        protected void ddlNewTeamName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            DropDownList ddlNewTeamName = (DropDownList)sender;
            string teamName = ddlNewTeamName.SelectedValue;

            GMSGeneralDALC ggdac = new GMSGeneralDALC();
            TableRow tr = (TableRow)ddlNewTeamName.Parent.Parent;
            DropDownList ddlNewSalesPersonMasterName = (DropDownList)tr.FindControl("ddlNewSalesPersonMasterName");
            if (ddlNewSalesPersonMasterName != null)
            {
                DataSet dsProducts = new DataSet();
                ggdac.GetSalesPersonWithNoShortName(session.CompanyId, teamName, ref dsProducts);
                ddlNewSalesPersonMasterName.DataSource = dsProducts.Tables[0];
                ddlNewSalesPersonMasterName.DataBind();
            }
        }

        
    }
}
