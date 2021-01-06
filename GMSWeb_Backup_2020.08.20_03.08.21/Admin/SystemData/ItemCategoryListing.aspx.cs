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
    public partial class ItemCategoryListing : GMSBasePage
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
                                                                            7);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                LoadItemCategoryData();
            }
        }

        //Load Data
        #region LoadItemCategoryData
        private void LoadItemCategoryData()
        {
            LogSession session = base.GetSessionInfo();
            IList<ItemCategory> lstItemCategory = null;
            try
            {
                lstItemCategory = new SystemDataActivity().RetrieveAllItemCategoryListSortByName(session);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            this.dgItemCategory.DataSource = lstItemCategory;
            this.dgItemCategory.DataBind();
        }
        #endregion

        #region dgItemCategory_ItemDataBound
        protected void dgItemCategory_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region dgItemCategory_EditCommand
        protected void dgItemCategory_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgItemCategory.EditItemIndex = e.Item.ItemIndex;
            LoadItemCategoryData();
        }
        #endregion

        #region dgItemCategory_CancelCommand
        protected void dgItemCategory_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgItemCategory.EditItemIndex = -1;
            LoadItemCategoryData();
        }
        #endregion

        #region dgItemCategory_UpdateCommand
        protected void dgItemCategory_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            TextBox txtEditName = (TextBox)e.Item.FindControl("txtEditName");

            if (txtEditName != null && !string.IsNullOrEmpty(txtEditName.Text))
            {
                short sItemCategoryId = GMSUtil.ToShort(this.dgItemCategory.DataKeys[e.Item.ItemIndex]);

                if (sItemCategoryId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    ItemCategory itemCategory = null;

                    try
                    {
                        itemCategory = sDataActivity.RetrieveItemCategoryById(sItemCategoryId, session);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    itemCategory.ItemCategoryName = txtEditName.Text.Trim();
                    itemCategory.ModifiedBy = session.UserId;
                    itemCategory.ModifiedDate = DateTime.Now;

                    try
                    {
                        ResultType result = sDataActivity.UpdateItemCategory(ref itemCategory, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgItemCategory.EditItemIndex = -1;
                                LoadItemCategoryData();
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

        #region dgItemCategory_CreateCommand
        protected void dgItemCategory_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                TextBox txtNewName = (TextBox)e.Item.FindControl("txtNewName");

                if (txtNewName != null && !string.IsNullOrEmpty(txtNewName.Text))
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    ItemCategory itemCategory = new ItemCategory();

                    itemCategory.ItemCategoryName = txtNewName.Text.Trim();
                    itemCategory.CreatedBy = session.UserId;
                    itemCategory.CreatedDate = DateTime.Now;

                    try
                    {
                        ResultType result = sDataActivity.CreateItemCategory(ref itemCategory, session);

                        switch (result)
                        {
                            case ResultType.Ok:

                                LoadItemCategoryData();
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

        #region dgItemCategory_DeleteCommand
        protected void dgItemCategory_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                short sItemCategoryId = GMSUtil.ToShort(this.dgItemCategory.DataKeys[e.Item.ItemIndex]);

                if (sItemCategoryId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    try
                    {
                        ResultType result = sDataActivity.DeleteItemCategory(sItemCategoryId, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgItemCategory.EditItemIndex = -1;
                                LoadItemCategoryData();
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
                            this.PageMsgPanel.ShowMessage("This Category cannot be deleted because it has been referenced by other Items.", MessagePanelControl.MessageEnumType.Alert);
                            LoadItemCategoryData();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        LoadItemCategoryData();
                        return;
                    }
                }
            }
        }
        #endregion
    }
}
