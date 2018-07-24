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

namespace GMSWeb.HR.Resources
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
                                                                            29);
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
            IList<ModuleReportCategory> lstCategory = null;
            try
            {
                lstCategory = new ReportsActivity().RetrieveAllModuleReportCategoryListByModuleCategoryIDSortBySeqID(new SystemDataActivity().RetrieveModuleCategoryByName("HR").ModuleCategoryID);
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

            if (txtEditName != null && !string.IsNullOrEmpty(txtEditName.Text))
            {
                short sCatId = GMSUtil.ToShort(this.dgCategory.DataKeys[e.Item.ItemIndex]);

                if (sCatId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    ReportsActivity categoryActivity = new ReportsActivity();
                    ModuleReportCategory category = null;

                    try
                    {
                        category = categoryActivity.RetrieveModuleReportCategoryById(sCatId, session);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    category.Name = txtEditName.Text.Trim();
                    category.ModuleCategoryID = new SystemDataActivity().RetrieveModuleCategoryByName("HR").ModuleCategoryID;
                    category.ModifiedBy = session.UserId;
                    category.ModifiedDate = DateTime.Now;

                    try
                    {
                        ResultType result = categoryActivity.UpdateModuleReportCategory(ref category, session);

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

                if (txtNewName != null && !string.IsNullOrEmpty(txtNewName.Text))
                {
                    LogSession session = base.GetSessionInfo();

                    ReportsActivity rCategoryActivity = new ReportsActivity();
                    ModuleReportCategory category = new ModuleReportCategory();

                    category.Name = txtNewName.Text.Trim();
                    int maxSeqID = new ReportsActivity().GetModuleReportCategoryMaxSeqID() + 1;
                    category.SeqID = GMSUtil.ToShort(maxSeqID);
                    category.ModuleCategoryID = new SystemDataActivity().RetrieveModuleCategoryByName("HR").ModuleCategoryID;
                    category.CreatedBy = session.UserId;
                    category.CreatedDate = DateTime.Now;

                    try
                    {
                        ResultType result = rCategoryActivity.CreateModuleReportCategory(ref category, session);

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
                        ResultType result = rActivity.DeleteModuleReportCategory(sCatId, session);

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
