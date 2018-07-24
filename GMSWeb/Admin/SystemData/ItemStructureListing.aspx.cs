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

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Admin.SystemData
{
    public partial class ItemStructureListing : GMSBasePage
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
                                                                            23);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                LoadPurposeDDL();
            }
        }

        #region LoadPurposeDDL
        private void LoadPurposeDDL()
        {
            SystemDataActivity sDataActivity = new SystemDataActivity();
            IList<ItemPurpose> lstPurpose = sDataActivity.RetrieveAllItemPurposeListSortByName();
            this.ddlPurpose.DataSource = lstPurpose;
            this.ddlPurpose.DataBind();

            this.ddlPurpose.Items.Insert(0, new ListItem("[SELECT]", "0"));
        }
        #endregion

        #region LoadItemStructureData
        private void LoadItemStructureData()
        {
            IList<ItemStructure> lstItems = null;
            try
            {
                lstItems = new SystemDataActivity().RetrieveItemStructureByPurposeId(GMSUtil.ToShort(this.ddlPurpose.SelectedValue));
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                return;
            }

            this.dgItemStructure.DataSource = lstItems;
            this.dgItemStructure.DataBind();
        }
        #endregion

        #region ddlPurpose_SelectedIndexChanged
        protected void ddlPurpose_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlPurpose.SelectedValue != "0")
            {
                LoadItemStructureData();
                this.udpItemStructureUpdater.Visible = true;
            }
            else
            {
                this.PageMsgPanel.ShowMessage("You have not select a Purpose.", MessagePanelControl.MessageEnumType.Alert);
                this.udpItemStructureUpdater.Visible = false;
            }
        }
        #endregion

        #region dgItemStructure_ItemDataBound
        protected void dgItemStructure_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");

                LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");
                HtmlInputHidden hidItemId = (HtmlInputHidden)e.Item.FindControl("hidItemId");
                if (lnkEdit != null && hidItemId != null)
                {
                    Item item = new SystemDataActivity().RetrieveItemById(GMSUtil.ToShort(hidItemId.Value));
                    lnkEdit.Text = "<span>" + item.ItemName + "</span>";
                }

            }

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                HtmlInputHidden hidItemId = (HtmlInputHidden)e.Item.FindControl("hidItemId");
                DropDownList ddlEditName = (DropDownList)e.Item.FindControl("ddlEditName");

                if (hidItemId != null &&ddlEditName != null)
                {
                    IList<Item> lstItem = new SystemDataActivity().RetrieveAllItemList();
                    ddlEditName.DataSource = lstItem;
                    ddlEditName.DataBind();
                    ddlEditName.SelectedValue = hidItemId.Value;
                }
            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewName = (DropDownList)e.Item.FindControl("ddlNewName");
                if (ddlNewName != null)
                {
                    IList<Item> lstItem = new SystemDataActivity().RetrieveAllItemList();
                    ddlNewName.DataSource = lstItem;
                    ddlNewName.DataBind();
                }

            }
        }
        #endregion

        #region dgItemStructure_EditCommand
        protected void dgItemStructure_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgItemStructure.EditItemIndex = e.Item.ItemIndex;
            LoadItemStructureData();
        }
        #endregion

        #region dgItemStructure_CancelCommand
        protected void dgItemStructure_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgItemStructure.EditItemIndex = -1;
            LoadItemStructureData();
        }
        #endregion

        #region dgItemStructure_CreateCommand
        protected void dgItemStructure_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            DropDownList ddlNewName = (DropDownList)e.Item.FindControl("ddlNewName");

            if (ddlNewName!= null)
            {
                LogSession session = base.GetSessionInfo();
                SystemDataActivity sDataActivity = new SystemDataActivity();

                // check redundant item name/id
                IList<ItemStructure> lstItemRedundantCheck = sDataActivity.RetrieveItemStructureByItemIdPurposeId(GMSUtil.ToShort(ddlNewName.SelectedValue),
                                                                                            GMSUtil.ToShort(this.ddlPurpose.SelectedValue));

                if (lstItemRedundantCheck != null && lstItemRedundantCheck.Count > 0)
                {
                    base.JScriptAlertMsg("This item already exist. Please select another item.");
                    LoadItemStructureData();
                    return;
                }

                ItemStructure itemStructure = new ItemStructure();

                itemStructure.ItemID = GMSUtil.ToShort(ddlNewName.SelectedValue);
                itemStructure.ItemPurposeID = GMSUtil.ToShort(this.ddlPurpose.SelectedValue);
                itemStructure.CreatedBy = session.UserId;
                itemStructure.CreatedDate = DateTime.Now;

                try
                {
                    ResultType result = sDataActivity.CreateItemStructure(ref itemStructure, session);

                    switch (result)
                    {
                        case ResultType.Ok:
                            LoadItemStructureData();
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

        #region dgItemStructure_UpdateCommand
        protected void dgItemStructure_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            DropDownList ddlEditName = (DropDownList)e.Item.FindControl("ddlEditName");

            if (ddlEditName!=null)
            {
                short sItemStructureId = GMSUtil.ToShort(this.dgItemStructure.DataKeys[e.Item.ItemIndex]);

                if (sItemStructureId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // check redundant item name/id
                    IList<ItemStructure> lstItemRedundantCheck = sDataActivity.RetrieveItemStructureByItemIdPurposeId(GMSUtil.ToShort(ddlEditName.SelectedValue),
                                                                                                GMSUtil.ToShort(this.ddlPurpose.SelectedValue));

                    if (lstItemRedundantCheck != null && lstItemRedundantCheck.Count > 0)
                    {
                        base.JScriptAlertMsg("This item already exist. Please select another item.");
                        LoadItemStructureData();
                        return;
                    }

                    ItemStructure itemStructure = null;

                    try
                    {
                        itemStructure = sDataActivity.RetrieveItemStructureById(sItemStructureId, session);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    itemStructure.ItemID = GMSUtil.ToShort(ddlEditName.SelectedValue);
                    itemStructure.ItemPurposeID = GMSUtil.ToShort(this.ddlPurpose.SelectedValue);
                    itemStructure.ModifiedBy = session.UserId;
                    itemStructure.ModifiedDate = DateTime.Now;

                    try
                    {
                        ResultType result = sDataActivity.UpdateItemStructure(ref itemStructure, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgItemStructure.EditItemIndex = -1;
                                LoadItemStructureData();
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

        #region dgItemStructure_DeleteCommand
        protected void dgItemStructure_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                short sItemStructureId = GMSUtil.ToShort(this.dgItemStructure.DataKeys[e.Item.ItemIndex]);

                if (sItemStructureId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    try
                    {
                        ResultType result = sDataActivity.DeleteItemStructure(sItemStructureId, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgItemStructure.EditItemIndex = -1;
                                LoadItemStructureData();
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

    }
}
