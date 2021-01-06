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

namespace GMSWeb.Organization.Upload
{
    public partial class UploadSpecial : GMSBasePage
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
                                                                            25);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                //preload
                this.dgSpecialData.CurrentPageIndex = 0;
                LoadParameterDDL();
            }

        }

        #region LoadParameterDDL
        private void LoadParameterDDL()
        {
            SystemDataActivity sDataActivity = new SystemDataActivity();
            IList<CompanyParameter> lstPurpose = sDataActivity.RetrieveAllParameterListSortById();
            this.ddlCompanyParameter.DataSource = lstPurpose;
            this.ddlCompanyParameter.DataBind();

            this.ddlCompanyParameter.Items.Insert(0, new ListItem("[SELECT]", "0"));
        }
        #endregion

        #region ddlCompanyParameter_SelectedIndexChanged
        protected void ddlCompanyParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlCompanyParameter.SelectedValue != "0")
            {
                this.dgSpecialData.CurrentPageIndex = 0;
                LoadSpecialDataGrid();
            }
            else
                this.PageMsgPanel.ShowMessage("You have not select a parameter.", MessagePanelControl.MessageEnumType.Alert);
        }
        #endregion

        #region LoadSpecialDataGrid
        protected void LoadSpecialDataGrid()
        {
            int iTotalRecords = 0;

            LogSession session = base.GetSessionInfo();
            IList<CompanySpecialData> lstSpecialData = null;
            try
            {
                lstSpecialData = new SystemDataActivity().SearchSpecialData(session.CompanyId, GMSUtil.ToShort(this.ddlCompanyParameter.SelectedValue),this.dgSpecialData.PageSize, this.dgSpecialData.CurrentPageIndex + 1,out iTotalRecords);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            if (iTotalRecords > 0)
            {
                Resultslbl.Visible = true;
                Pageslbl.Visible = true;
                this.dgSpecialData.VirtualItemCount = iTotalRecords;
                this.dgSpecialData.DataSource = lstSpecialData;
                this.dgSpecialData.DataBind();

                this.lblPage.Text = string.Format("Page {0} of {1}", this.dgSpecialData.CurrentPageIndex + 1, this.dgSpecialData.PageCount);
                if (this.dgSpecialData.PageCount == 1)
                {
                    this.lnkFirst.Enabled = false;
                    this.lnkLast.Enabled = false;
                    this.lnkNext.Enabled = false;
                    this.lnkPrev.Enabled = false;
                }
                else
                {
                    this.lnkFirst.Enabled = true;
                    this.lnkLast.Enabled = true;
                    this.lnkNext.Enabled = true;
                    this.lnkPrev.Enabled = true;
                }

                int iMaxRange = ((this.dgSpecialData.CurrentPageIndex + 1) * this.dgSpecialData.PageSize);
                if (iMaxRange > iTotalRecords)
                    iMaxRange = iTotalRecords;

                if (iMaxRange > 0)
                    this.lblTotalRecordsFound.Text = string.Format("{0} - {1} of {2}",
                                                                    (this.dgSpecialData.CurrentPageIndex * this.dgSpecialData.PageSize) + 1,
                                                                    iMaxRange,
                                                                     iTotalRecords);
                else
                    this.lblTotalRecordsFound.Text = "0";
            }
            else
            {
                Resultslbl.Visible = false;
                Pageslbl.Visible = false;
                this.lblTotalRecordsFound.Text = "0";
                this.dgSpecialData.DataSource = lstSpecialData;
                this.dgSpecialData.DataBind();
            }
        }
        #endregion

        #region SearchResults_PageNavigate
        protected void SearchResults_PageNavigate(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "First":
                    this.dgSpecialData.CurrentPageIndex = 0;
                    LoadSpecialDataGrid();
                    break;

                case "Previous":
                    if (this.dgSpecialData.CurrentPageIndex > 0)
                    {
                        this.dgSpecialData.CurrentPageIndex--;
                        LoadSpecialDataGrid();
                    }
                    break;

                case "Next":
                    if (this.dgSpecialData.CurrentPageIndex < this.dgSpecialData.PageCount - 1)
                    {
                        this.dgSpecialData.CurrentPageIndex++;
                        LoadSpecialDataGrid();
                    }
                    break;

                case "Last":
                    int pageMax = this.dgSpecialData.VirtualItemCount / this.dgSpecialData.PageSize;
                    if (this.dgSpecialData.VirtualItemCount % this.dgSpecialData.PageSize > 0)
                        pageMax++;
                    this.dgSpecialData.CurrentPageIndex = this.dgSpecialData.PageCount - 1;
                    LoadSpecialDataGrid();
                    break;


            }
        }
        #endregion

        #region dgSpecialData_ItemDataBound
        protected void dgSpecialData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region dgSpecialData_EditCommand
        protected void dgSpecialData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                        26);
            if (uAccess == null)
            {
                base.JScriptAlertMsg("You are not authorized to update company special data.");
                LoadSpecialDataGrid();
                return;
            }
            else
            {
                this.dgSpecialData.EditItemIndex = e.Item.ItemIndex;
                LoadSpecialDataGrid();
            }
        }
        #endregion

        #region dgSpecialData_CancelCommand
        protected void dgSpecialData_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgSpecialData.EditItemIndex = -1;
            LoadSpecialDataGrid();
        }
        #endregion

        #region dgSpecialData_UpdateCommand
        protected void dgSpecialData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            TextBox txtEditValue = (TextBox)e.Item.FindControl("txtEditValue");
            HtmlInputHidden hidCoyID = (HtmlInputHidden)e.Item.FindControl("hidCoyID");
            HtmlInputHidden hidParameterID = (HtmlInputHidden)e.Item.FindControl("hidParameterID");
            HtmlInputHidden hidCSDDate = (HtmlInputHidden)e.Item.FindControl("hidCSDDate");

            if (txtEditValue != null && !string.IsNullOrEmpty(txtEditValue.Text) && hidCoyID != null && hidParameterID != null && hidCSDDate != null)
            {
                LogSession session = base.GetSessionInfo();

                CompanySpecialData specialData = new ParameterActivity().RetrieveParameterByCSDDate(session.CompanyId, GMSUtil.ToShort(this.ddlCompanyParameter.SelectedValue),
                                                                                                                        GMSUtil.ToDate(hidCSDDate.Value)
                                                                                                                        );

                specialData.ParameterValue = txtEditValue.Text.Trim();
                try
                {
                    ResultType result = new ParameterActivity().UpdateCompanyParameter(ref specialData, session);

                    switch (result)
                    {
                        case ResultType.Ok:
                            this.dgSpecialData.EditItemIndex = -1;
                            LoadSpecialDataGrid();
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

        #region dgSpecialData_CreateCommand
        protected void dgSpecialData_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            26);
                if (uAccess == null)
                {
                    base.JScriptAlertMsg("You are not authorized to update Company Parameter.");
                    LoadSpecialDataGrid();
                    return;
                }

                TextBox txtNewValue = (TextBox)e.Item.FindControl("txtNewValue");
                CalendarControl calCSDDate = (CalendarControl)e.Item.FindControl("calCSDDate");

                if (txtNewValue != null && !string.IsNullOrEmpty(txtNewValue.Text) && calCSDDate != null && !string.IsNullOrEmpty(calCSDDate.SelectedDate.ToString()))
                {
                    try
                    {
                        // check if newly inserted parameter value already exist
                        CompanySpecialData existingSpecialData = new ParameterActivity().RetrieveParameterByCSDDate(session.CompanyId, GMSUtil.ToShort(this.ddlCompanyParameter.SelectedValue),
                                                                                                                       GMSUtil.ToDate(calCSDDate.SelectedDate.ToString()));

                        if (existingSpecialData != null)
                        {
                            this.PageMsgPanel.ShowMessage("Processing error of type : Parameter value already exists.", MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }

                        CompanySpecialData specialData = new CompanySpecialData();
                        specialData.CoyID = session.CompanyId;
                        specialData.ParameterID = GMSUtil.ToShort(this.ddlCompanyParameter.SelectedValue);
                        specialData.CSDDate = GMSUtil.ToDate(calCSDDate.SelectedDate.ToString());
                        specialData.ParameterValue = txtNewValue.Text.Trim();

                        ResultType result = new ParameterActivity().CreateCompanySpecialData(ref specialData, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                LoadSpecialDataGrid();
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

        #region dgSpecialData_DeleteCommand
        protected void dgSpecialData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                HtmlInputHidden hidCSDDate = (HtmlInputHidden)e.Item.FindControl("hidCSDDate");

                if (hidCSDDate != null)
                {
                    LogSession session = base.GetSessionInfo();

                    ParameterActivity pActivity = new ParameterActivity();

                    try
                    {
                        ResultType result = pActivity.DeleteSpecialData(GMSUtil.ToDate(hidCSDDate.Value), GMSUtil.ToShort(this.ddlCompanyParameter.SelectedValue), session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgSpecialData.EditItemIndex = -1;
                                LoadSpecialDataGrid();
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
                            this.PageMsgPanel.ShowMessage("This parameter value cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                            LoadSpecialDataGrid();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        LoadSpecialDataGrid();
                        return;
                    }
                }
            }
        }
        #endregion
    }
}
