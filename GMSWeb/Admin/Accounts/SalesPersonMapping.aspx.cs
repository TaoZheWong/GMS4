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

namespace GMSWeb.Admin.Accounts
{
    public partial class SalesPersonMapping1 : GMSBasePage
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Admin"); 
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Admin"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            77);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Admin"));

            if (!Page.IsPostBack)
            {
                this.dgResult.CurrentPageIndex = 0;
                this.dgResult2.CurrentPageIndex = 0;
                LoadUsernameDDL();
                //LoadReportList();
            }
        }
        #endregion

        #region LoadDDL
        private void LoadUsernameDDL()
        {
            
            IList<GMSUser> lstUsername = new GMSUserActivity().RetrieveAllUsersMember(new LogSession());            
            
            
            this.ddlUsername.DataSource = lstUsername;
            this.ddlUsername.DataBind();
            this.ddlUsername.Items.Insert(0, new ListItem("[SELECT]", "0"));
        }

        private void LoadCompanyDDL()
        {
            IList<Company> lstCompany = new SystemDataActivity().RetrieveAllAliveCompanyList();
            this.ddlCompany.DataSource = lstCompany;
            this.ddlCompany.DataBind();
            this.ddlCompany.Items.Insert(0, new ListItem("[SELECT]", "0"));
        }
        #endregion

        #region ddlUsername_SelectedIndexChanged
        protected void ddlUsername_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlUsername.SelectedValue != "0")
            {
                this.companyRow.Style.Add("display", "block");

                LoadCompanyDDL();

                this.dgResult.CurrentPageIndex = 0;
                this.dgResult.DataSource = null;
                this.dgResult.DataBind();
                this.dgResult2.CurrentPageIndex = 0;
                this.dgResult2.DataSource = null;
                this.dgResult2.DataBind();

               // this.groupRow.Visible = true;
                this.groupRow.Style.Add("display","block");

                GroupManagementUser gmu = GroupManagementUser.RetrieveByKey(GMSUtil.ToShort(ddlUsername.SelectedValue));
                if (gmu != null)
                {
                    rbIsGroupManagementUser.Checked = true;
                    rbIsNotGroupManagementUser.Checked = false;
                }
                else
                {
                    rbIsNotGroupManagementUser.Checked = true;
                    rbIsGroupManagementUser.Checked = false;
                }
            }
            else
            {
                //this.companyRow.Visible = false;
                this.companyRow.Style.Add("display", "none");
               // this.groupRow.Visible = false;
                this.groupRow.Style.Add("display", "none");

                this.dgResult.DataSource = null;
                this.dgResult.DataBind();
                this.dgResult2.DataSource = null;
                this.dgResult2.DataBind();

                this.PageMsgPanel.ShowMessage("You have not select a user.", MessagePanelControl.MessageEnumType.Alert);
            }
        }
        #endregion

        #region rbIsGroupManagementUser_CheckedChanged
        protected void rbIsGroupManagementUser_CheckedChanged(object sender, EventArgs e)
        {
            GroupManagementUser gmu = GroupManagementUser.RetrieveByKey(GMSUtil.ToShort(ddlUsername.SelectedValue));
            if (gmu != null)
            {
                gmu.Delete();
                gmu.Resync();
            }
            else
            {
                gmu = new GroupManagementUser();
                gmu.MGUserID = GMSUtil.ToShort(ddlUsername.SelectedValue);
                gmu.Save();
                gmu.Resync();
            }
        }
        #endregion

        #region ddlCompany_SelectedIndexChanged
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlCompany.SelectedValue != "0")
            {
                this.dgResult.CurrentPageIndex = 0;
                LoadData();

                this.dgResult2.CurrentPageIndex = 0;
                LoadData2();

                // this.companyManagementRow.Visible = true;
                this.companyManagementRow.Style.Add("display","block");

                CompanyManagementUser cmu = CompanyManagementUser.RetrieveByKey(GMSUtil.ToShort(ddlCompany.SelectedValue), GMSUtil.ToShort(ddlUsername.SelectedValue));
                if (cmu != null)
                {
                    rbIsCompanyManagementUser.Checked = true;
                    rbIsNotCompanyManagementUser.Checked = false;
                }
                else
                {
                    rbIsNotCompanyManagementUser.Checked = true;
                    rbIsCompanyManagementUser.Checked = false;
                }

                this.productManagementRow.Style.Add("display", "block");
                ProductManagementUser pmu = ProductManagementUser.RetrieveByKey(GMSUtil.ToShort(ddlCompany.SelectedValue), GMSUtil.ToShort(ddlUsername.SelectedValue));
                if (pmu != null)
                {
                    rbIsProductManagementUser.Checked = true;
                    rbIsNotProductManagementUser.Checked = false;
                }
                else
                {
                    rbIsNotProductManagementUser.Checked = true;
                    rbIsProductManagementUser.Checked = false;
                }

                this.divisionRow.Visible = true;
                DivisionUser du = DivisionUser.RetrieveByKey(GMSUtil.ToShort(ddlCompany.SelectedValue), GMSUtil.ToShort(ddlUsername.SelectedValue));
                if(du != null)
                {
                    if (du.DivisionID == "GAS")
                    {
                        rbGasDivisionUser.Checked = true;
                        rbWeldingDivisionUser.Checked = false;
                        rbAll.Checked = false;
                    }
                    else if (du.DivisionID == "WSD")
                    {
                        rbWeldingDivisionUser.Checked = true;
                        rbGasDivisionUser.Checked = false;
                        rbAll.Checked = false;
                    }
                }
                else
                {
                    rbGasDivisionUser.Checked = false;
                    rbWeldingDivisionUser.Checked = false;
                    rbAll.Checked = true;
                }
            }
            else
            {
                this.dgResult.DataSource = null;
                this.dgResult.DataBind();

                this.dgResult2.DataSource = null;
                this.dgResult2.DataBind();

                this.PageMsgPanel.ShowMessage("You have not select a company.", MessagePanelControl.MessageEnumType.Alert);
            }
        }
        #endregion

        #region rbIsCompanyManagementUser_CheckedChanged
        protected void rbIsCompanyManagementUser_CheckedChanged(object sender, EventArgs e)
        {
            CompanyManagementUser cmu = CompanyManagementUser.RetrieveByKey(GMSUtil.ToShort(ddlCompany.SelectedValue), GMSUtil.ToShort(ddlUsername.SelectedValue));
            if (cmu != null)
            {
                cmu.Delete();
                cmu.Resync();
            }
            else
            {
                cmu = new CompanyManagementUser();
                cmu.MGUserID = GMSUtil.ToShort(ddlUsername.SelectedValue);
                cmu.CoyID = GMSUtil.ToShort(ddlCompany.SelectedValue);
                cmu.Save();
                cmu.Resync();
            }
        }
        #endregion

        #region rbIsProductManagementUser_CheckedChanged
        protected void rbIsProductManagementUser_CheckedChanged(object sender, EventArgs e)
        {
            ProductManagementUser pmu = ProductManagementUser.RetrieveByKey(GMSUtil.ToShort(ddlCompany.SelectedValue), GMSUtil.ToShort(ddlUsername.SelectedValue));
            if (pmu != null)
            {
                pmu.Delete();
                pmu.Resync();
            }
            else
            {
                pmu = new ProductManagementUser();
                pmu.MGUserID = GMSUtil.ToShort(ddlUsername.SelectedValue);
                pmu.CoyID = GMSUtil.ToShort(ddlCompany.SelectedValue);
                pmu.Save();
                pmu.Resync();
            }
        }
        #endregion

        #region rbDivisionUser_CheckedChanged
        protected void rbDivisionUser_CheckedChanged(object sender, EventArgs e)
        {
            
            DivisionUser du = DivisionUser.RetrieveByKey(GMSUtil.ToShort(ddlCompany.SelectedValue), GMSUtil.ToShort(ddlUsername.SelectedValue));
            if (du != null)
            {
                if (rbAll.Checked)
                {
                    du.Delete();
                    du.Resync();
                }
                else if (rbGasDivisionUser.Checked)
                {
                    du.DivisionID = "GAS";
                    du.Save();
                    du.Resync();
                }
                else if (rbWeldingDivisionUser.Checked)
                {
                    du.DivisionID = "WSD";
                    du.Save();
                    du.Resync();
                }
                
            }
            else
            {
                if (rbGasDivisionUser.Checked || rbWeldingDivisionUser.Checked)
                {
                    du = new DivisionUser();
                    du.UserNumID = GMSUtil.ToShort(ddlUsername.SelectedValue);
                    du.CoyID = GMSUtil.ToShort(ddlCompany.SelectedValue);
                    if (rbGasDivisionUser.Checked)
                        du.DivisionID = "GAS";
                    else
                        du.DivisionID = "WSD";
                    du.Save();
                    du.Resync();
                }
            }
            
        }
        #endregion

        #region LoadData
        protected void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            short userNumId = GMSUtil.ToShort(ddlUsername.SelectedValue);
            short companyId = GMSUtil.ToShort(ddlCompany.SelectedValue);
            IList<SalesPersonUser> lstMapping = new SystemDataActivity().RetrieveAllSalesPersonUserByUserNumIDCoyID(userNumId, companyId);
            this.dgResult.DataSource = lstMapping;
            this.dgResult.DataBind();
        }
        #endregion

        #region dgResult_ItemDataBound
        protected void dgResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewSalesPersonID = (DropDownList)e.Item.FindControl("ddlNewSalesPersonID");

                if (ddlNewSalesPersonID != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in salespersonid dropdown list
                    IList<SalesPerson> lstSalesPerson = null;
                    lstSalesPerson = sDataActivity.RetrieveAllSalesPersonByCompanyIDSortBySalesPersonID(GMSUtil.ToShort(ddlCompany.SelectedValue));
                    ddlNewSalesPersonID.DataSource = lstSalesPerson;
                    ddlNewSalesPersonID.DataBind();
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

        #region dgResult datagrid PageIndexChanged event handling
        protected void dgResult_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();

        }
        #endregion

        #region dgResult_CreateCommand
        protected void dgResult_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();

                DropDownList ddlNewSalesPersonID = (DropDownList)e.Item.FindControl("ddlNewSalesPersonID");

                if (ddlNewSalesPersonID != null)
                {
                    try
                    {
                        // check if newly inserted mapping already exist
                        GMSCore.Entity.SalesPersonUser existingMapping = new UserSalesPersonActivity().RetrieveUserSalesPersonByUserNumIDCoyIDSalesPersonID(GMSUtil.ToShort(ddlUsername.SelectedValue), GMSUtil.ToShort(ddlCompany.SelectedValue), ddlNewSalesPersonID.SelectedValue);

                        if (existingMapping != null)
                        {
                            this.PageMsgPanel.ShowMessage("Processing error of type : This mapping already exists.", MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }

                        GMSCore.Entity.SalesPersonUser mapping = new GMSCore.Entity.SalesPersonUser();
                        mapping.UserID = GMSUtil.ToShort(ddlUsername.SelectedValue);
                        mapping.CoyID = GMSUtil.ToShort(ddlCompany.SelectedValue);
                        mapping.SalesPersonID = ddlNewSalesPersonID.SelectedValue;

                        ResultType result = new UserSalesPersonActivity().CreateUserSalesPerson(ref mapping, session);

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

        #region dgResult_DeleteCommand
        protected void dgResult_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                HtmlInputHidden hidSalesPersonID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonID");
                HtmlInputHidden hidUserNumID = (HtmlInputHidden)e.Item.FindControl("hidUserNumID");
                HtmlInputHidden hidCoyID = (HtmlInputHidden)e.Item.FindControl("hidCoyID");

                if (hidSalesPersonID != null && hidUserNumID != null && hidCoyID != null)
                {
                    LogSession session = base.GetSessionInfo();

                    UserSalesPersonActivity uActivity = new UserSalesPersonActivity();

                    try
                    {
                        ResultType result = uActivity.DeleteUserSalesPerson(GMSUtil.ToShort(hidUserNumID.Value), GMSUtil.ToShort(hidCoyID.Value), hidSalesPersonID.Value, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgResult.EditItemIndex = -1;
                                this.dgResult.CurrentPageIndex = 0;
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
                            this.PageMsgPanel.ShowMessage("This mapping cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
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

        #region LoadData2
        protected void LoadData2()
        {
            LogSession session = base.GetSessionInfo();
            short userNumId = GMSUtil.ToShort(ddlUsername.SelectedValue);
            short companyId = GMSUtil.ToShort(ddlCompany.SelectedValue);
            IList<ManagerSalesPerson> lstMapping2 = new SystemDataActivity().RetrieveAllManagerSalesPersonByManagerUserIDCoyID(userNumId, companyId);
            this.dgResult2.DataSource = lstMapping2;
            this.dgResult2.DataBind();
        }
        #endregion

        #region dgResult2_ItemDataBound
        protected void dgResult2_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewSalesPersonID = (DropDownList)e.Item.FindControl("ddlNewSalesPersonID");

                if (ddlNewSalesPersonID != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in salespersonid dropdown list
                    IList<SalesPerson> lstSalesPerson = null;
                    lstSalesPerson = sDataActivity.RetrieveAllSalesPersonByCompanyIDSortBySalesPersonID(GMSUtil.ToShort(ddlCompany.SelectedValue));
                    ddlNewSalesPersonID.DataSource = lstSalesPerson;
                    ddlNewSalesPersonID.DataBind();
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

        #region dgResult2 datagrid PageIndexChanged event handling
        protected void dgResult2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData2();

        }
        #endregion

        #region dgResult2_CreateCommand
        protected void dgResult2_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();

                DropDownList ddlNewSalesPersonID = (DropDownList)e.Item.FindControl("ddlNewSalesPersonID");

                if (ddlNewSalesPersonID != null)
                {
                    try
                    {
                        // check if newly inserted mapping already exist
                        GMSCore.Entity.ManagerSalesPerson existingMapping = ManagerSalesPerson.RetrieveByKey(GMSUtil.ToShort(ddlCompany.SelectedValue), GMSUtil.ToShort(ddlUsername.SelectedValue), ddlNewSalesPersonID.SelectedValue);

                        if (existingMapping != null)
                        {
                            this.PageMsgPanel.ShowMessage("Processing error of type : This mapping already exists.", MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }

                        GMSCore.Entity.ManagerSalesPerson mapping = new GMSCore.Entity.ManagerSalesPerson();
                        mapping.ManagerUserID = GMSUtil.ToShort(ddlUsername.SelectedValue);
                        mapping.CoyID = GMSUtil.ToShort(ddlCompany.SelectedValue);
                        mapping.SalesPersonID = ddlNewSalesPersonID.SelectedValue;

                        mapping.Save();
                        mapping.Resync();

                        LoadData2();
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

        #region dgResult2_DeleteCommand
        protected void dgResult2_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                HtmlInputHidden hidSalesPersonID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonID");
                HtmlInputHidden hidUserNumID = (HtmlInputHidden)e.Item.FindControl("hidManagerUserID");
                HtmlInputHidden hidCoyID = (HtmlInputHidden)e.Item.FindControl("hidCoyID");

                if (hidSalesPersonID != null && hidUserNumID != null && hidCoyID != null)
                {
                    LogSession session = base.GetSessionInfo();

                    UserSalesPersonActivity uActivity = new UserSalesPersonActivity();

                    try
                    {
                        GMSCore.Entity.ManagerSalesPerson existingMapping = ManagerSalesPerson.RetrieveByKey(GMSUtil.ToShort(hidCoyID.Value),
                            GMSUtil.ToShort(hidUserNumID.Value), hidSalesPersonID.Value);

                        if (existingMapping != null)
                        {
                            existingMapping.Delete();
                            existingMapping.Resync();
                        }

                        this.dgResult2.EditItemIndex = -1;
                        this.dgResult2.CurrentPageIndex = 0;
                        LoadData2();
                    }
                    catch (SqlException exSql)
                    {
                        if (exSql.Number == 547)
                        {
                            this.PageMsgPanel.ShowMessage("This mapping cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                            LoadData2();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        LoadData2();
                        return;
                    }
                }
            }
        }
        #endregion

    }
}
