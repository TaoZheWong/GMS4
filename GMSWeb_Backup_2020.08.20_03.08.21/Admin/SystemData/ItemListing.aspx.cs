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
    public partial class ItemListing : GMSBasePage
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
                                                                            8);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                //preload
                this.dgItem.CurrentPageIndex = 0;
                LoadItemData();
            }
        }

        #region LoadItemData
        private void LoadItemData()
        {
            int iTotalRecords = 0;
            
            IList<Item> lstItems = null;
            try
            {
                lstItems = new SystemDataActivity().SearchItem(this.dgItem.PageSize,
                                                                    this.dgItem.CurrentPageIndex + 1,
                                                                    out iTotalRecords,
                                                                    null);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                return;
            }

            if (iTotalRecords > 0)
            {
                this.dgItem.VirtualItemCount = iTotalRecords;
                this.dgItem.DataSource = lstItems;
                this.dgItem.DataBind();

                this.lblPage.Text = string.Format("Page {0} of {1}", this.dgItem.CurrentPageIndex + 1, this.dgItem.PageCount);
                if (this.dgItem.PageCount == 1)
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

                int iMaxRange = ((this.dgItem.CurrentPageIndex + 1) * this.dgItem.PageSize);
                if (iMaxRange > iTotalRecords)
                    iMaxRange = iTotalRecords;

                if (iMaxRange > 0)
                    this.lblTotalRecordsFound.Text = string.Format("{0} - {1} of {2}",
                                                                    (this.dgItem.CurrentPageIndex * this.dgItem.PageSize) + 1,
                                                                    iMaxRange,
                                                                     iTotalRecords);
                else
                    this.lblTotalRecordsFound.Text = "0";
            }
            else
            {
                this.lblTotalRecordsFound.Text = "0";
            }


        }
        #endregion

        #region SearchResults_PageNavigate
        protected void SearchResults_PageNavigate(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "First":
                    this.dgItem.CurrentPageIndex = 0;
                    LoadItemData();
                    break;

                case "Previous":
                    if (this.dgItem.CurrentPageIndex > 0)
                    {
                        this.dgItem.CurrentPageIndex--;
                        LoadItemData();
                    }
                    break;

                case "Next":
                    if (this.dgItem.CurrentPageIndex < this.dgItem.PageCount - 1)
                    {
                        this.dgItem.CurrentPageIndex++;
                        LoadItemData();
                    }
                    break;

                case "Last":
                    int pageMax = this.dgItem.VirtualItemCount / this.dgItem.PageSize;
                    if (this.dgItem.VirtualItemCount % this.dgItem.PageSize > 0)
                        pageMax++;
                    this.dgItem.CurrentPageIndex = this.dgItem.PageCount - 1;
                    LoadItemData();
                    break;


            }
        }
        #endregion

        #region dgItem_ItemDataBound
        protected void dgItem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region dgItem_EditCommand
        protected void dgItem_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgItem.EditItemIndex = e.Item.ItemIndex;
            LoadItemData();
        }
        #endregion

        #region dgItem_CancelCommand
        protected void dgItem_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgItem.EditItemIndex = -1;
            LoadItemData();
        }
        #endregion

        #region dgItem_UpdateCommand
        protected void dgItem_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            TextBox txtEditName = (TextBox)e.Item.FindControl("txtEditName");

            if (txtEditName != null && !string.IsNullOrEmpty(txtEditName.Text))
            {
                short sItemId = GMSUtil.ToShort(this.dgItem.DataKeys[e.Item.ItemIndex]);

                if (sItemId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    Item item = null;

                    try
                    {
                        item = sDataActivity.RetrieveItemById(sItemId, session);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    item.ItemName = txtEditName.Text.Trim();
                    item.ModifiedBy = session.UserId;
                    item.ModifiedDate = DateTime.Now;

                    try
                    {
                        ResultType result = sDataActivity.UpdateItem(ref item, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgItem.EditItemIndex = -1;
                                LoadItemData();
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

        #region dgItem_CreateCommand
        protected void dgItem_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                TextBox txtNewName = (TextBox)e.Item.FindControl("txtNewName");

                if (txtNewName != null && !string.IsNullOrEmpty(txtNewName.Text))
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    Item item = new Item();

                    item.ItemName = txtNewName.Text.Trim();
                    item.CreatedBy = session.UserId;
                    item.CreatedDate = DateTime.Now;

                    try
                    {
                        ResultType result = sDataActivity.CreateItem(ref item, session);

                        switch (result)
                        {
                            case ResultType.Ok:

                                LoadItemData();
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

        #region dgItem_DeleteCommand
        protected void dgItem_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                short sItemId = GMSUtil.ToShort(this.dgItem.DataKeys[e.Item.ItemIndex]);

                if (sItemId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    try
                    {
                        ResultType result = sDataActivity.DeleteItem(sItemId, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgItem.EditItemIndex = -1;
                                LoadItemData();
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
                            this.PageMsgPanel.ShowMessage("This Item cannot be deleted because it has been referenced by Item Structure.", MessagePanelControl.MessageEnumType.Alert);
                            LoadItemData();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        LoadItemData();
                        return;
                    }
                }
            }
        }
        #endregion

    }
}
