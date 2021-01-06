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

namespace GMSWeb.Reports.Setup
{
    public partial class Category : GMSBasePage
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
                                                                            18);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");


            if (!Page.IsPostBack)
            {
                LoadCategorySetup();
            }
        }

        //Load Data
        #region LoadCategorySetup
        private void LoadCategorySetup()
        {
            LogSession session = base.GetSessionInfo();
            IList<ReportCategory> lstCategory = null;
            try
            {
                lstCategory = new ReportsActivity().RetrieveAllReportCategoryListSortBySeqID(session);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            this.dgCategory.DataSource = lstCategory;
            this.dgCategory.DataBind();
        }
        #endregion

        #region dgCategory_ItemDataBound
        protected void dgCategory_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddlEditSeqID = (DropDownList)e.Item.FindControl("ddlEditSeqID");
                if (ddlEditSeqID != null)
                {
                    ReportCategory rCategory = (ReportCategory)e.Item.DataItem;
                    ReportsActivity rCategoryActivity = new ReportsActivity();
                    if (rCategory != null)
                    {
                        // fill in drop down list
                        IList<ReportCategory> lstAllCategory = null;
                        lstAllCategory = rCategoryActivity.RetrieveAllReportCategoryListSortBySeqID();
                        ddlEditSeqID.DataSource = lstAllCategory;
                        ddlEditSeqID.DataBind();

                        int maxSeqID = rCategoryActivity.GetReportCategoryMaxSeqID();
                        int newMaxSeqID = maxSeqID+1;
                        ddlEditSeqID.Items.Add(new ListItem(newMaxSeqID.ToString(), newMaxSeqID.ToString()));

                        ddlEditSeqID.SelectedValue = rCategory.SeqID.ToString();
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewSeqID = (DropDownList)e.Item.FindControl("ddlNewSeqID");
                if (ddlNewSeqID != null)
                {
                    ReportsActivity rCategoryActivity = new ReportsActivity();
                    
                    // fill in drop down list
                    IList<ReportCategory> lstAllCategory = null;
                    lstAllCategory = rCategoryActivity.RetrieveAllReportCategoryListSortBySeqID();
                    ddlNewSeqID.DataSource = lstAllCategory;
                    ddlNewSeqID.DataBind();

                    int maxSeqID = rCategoryActivity.GetReportCategoryMaxSeqID();
                    int newMaxSeqID = maxSeqID + 1;
                    ddlNewSeqID.Items.Add(new ListItem(newMaxSeqID.ToString(), newMaxSeqID.ToString()));
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

        #region dgCategory_EditCommand
        protected void dgCategory_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgCategory.EditItemIndex = e.Item.ItemIndex;
            LoadCategorySetup();
        }
        #endregion

        #region dgCategory_CancelCommand
        protected void dgCategory_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgCategory.EditItemIndex = -1;
            LoadCategorySetup();
        }
        #endregion

        #region dgCategory_UpdateCommand
        protected void dgCategory_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            TextBox txtEditName = (TextBox)e.Item.FindControl("txtEditName");
            DropDownList ddlEditSeqID = (DropDownList)e.Item.FindControl("ddlEditSeqID");

            if (txtEditName != null && ddlEditSeqID != null && !string.IsNullOrEmpty(txtEditName.Text))
            {
                short sCatId = GMSUtil.ToShort(this.dgCategory.DataKeys[e.Item.ItemIndex]);

                if (sCatId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    ReportsActivity categoryActivity = new ReportsActivity();
                    ReportCategory category = null;

                    try
                    {
                        category = categoryActivity.RetrieveReportCategoryById(sCatId, session);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    category.Name = txtEditName.Text.Trim();
                    category.SeqID = GMSUtil.ToShort(ddlEditSeqID.SelectedValue);
                    category.ModifiedBy = session.UserId;
                    category.ModifiedDate = DateTime.Now;

                    try
                    {
                        ResultType result = categoryActivity.UpdateReportCategory(ref category, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgCategory.EditItemIndex = -1;
                                LoadCategorySetup();
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

        #region dgCategory_CreateCommand
        protected void dgCategory_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                TextBox txtNewName = (TextBox)e.Item.FindControl("txtNewName");
                DropDownList ddlNewSeqID = (DropDownList)e.Item.FindControl("ddlNewSeqID");

                if (txtNewName != null && !string.IsNullOrEmpty(txtNewName.Text) && ddlNewSeqID != null )
                {
                    LogSession session = base.GetSessionInfo();

                    ReportsActivity rCategoryActivity = new ReportsActivity();
                    ReportCategory category = new ReportCategory();

                    category.Name = txtNewName.Text.Trim();
                    category.SeqID = GMSUtil.ToShort(ddlNewSeqID.SelectedValue);
                    category.CreatedBy = session.UserId;
                    category.CreatedDate = DateTime.Now;

                    try
                    {
                        ResultType result = rCategoryActivity.CreateReportCategory(ref category, session);

                        switch (result)
                        {
                            case ResultType.Ok:

                                LoadCategorySetup();
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

        #region dgCategory_DeleteCommand
        protected void dgCategory_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                short sCatId = GMSUtil.ToShort(this.dgCategory.DataKeys[e.Item.ItemIndex]);

                if (sCatId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    ReportsActivity rActivity = new ReportsActivity();

                    try
                    {
                        ResultType result = rActivity.DeleteReportCategory(sCatId, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgCategory.EditItemIndex = -1;
                                LoadCategorySetup();
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
                            this.PageMsgPanel.ShowMessage("This Category cannot be deleted because it has been referenced by system reports.", MessagePanelControl.MessageEnumType.Alert);
                            LoadCategorySetup();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        LoadCategorySetup();
                        return;
                    }
                }
            }
        }
        #endregion
    }
}
