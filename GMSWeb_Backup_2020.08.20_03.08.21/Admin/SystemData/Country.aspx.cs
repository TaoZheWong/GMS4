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
    public partial class Country : GMSBasePage
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
                                                                            4);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                LoadCountry();
            }
        }

        //Load Data
        #region LoadCountry
        private void LoadCountry()
        {
            LogSession session = base.GetSessionInfo();
            IList<GMSCore.Entity.Country> lstCountry = null;
            try
            {
                lstCountry = new SystemDataActivity().RetrieveAllCountryListSortBySeqID(session);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            this.dgCountry.DataSource = lstCountry;
            this.dgCountry.DataBind();
        }
        #endregion

        #region dgCountry_ItemDataBound
        protected void dgCountry_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddlEditSeqID = (DropDownList)e.Item.FindControl("ddlEditSeqID");
                if (ddlEditSeqID != null)
                {
                    GMSCore.Entity.Country country = (GMSCore.Entity.Country)e.Item.DataItem;
                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    if (country != null)
                    {
                        // fill in drop down list
                        IList<GMSCore.Entity.Country> lstCountry = null;
                        lstCountry = sDataActivity.RetrieveAllCountryListSortBySeqID();
                        ddlEditSeqID.DataSource = lstCountry;
                        ddlEditSeqID.DataBind();

                        int maxSeqID = sDataActivity.GetCountryMaxSeqID();
                        int newMaxSeqID = maxSeqID + 1;
                        ddlEditSeqID.Items.Add(new ListItem(newMaxSeqID.ToString(), newMaxSeqID.ToString()));

                        ddlEditSeqID.SelectedValue = country.SeqID.ToString();
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewSeqID = (DropDownList)e.Item.FindControl("ddlNewSeqID");
                if (ddlNewSeqID != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in drop down list
                    IList<GMSCore.Entity.Country> lstCountry = null;
                    lstCountry = sDataActivity.RetrieveAllCountryListSortBySeqID();
                    ddlNewSeqID.DataSource = lstCountry;
                    ddlNewSeqID.DataBind();

                    int maxSeqID = sDataActivity.GetCountryMaxSeqID();
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

        #region dgCountry_EditCommand
        protected void dgCountry_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgCountry.EditItemIndex = e.Item.ItemIndex;
            LoadCountry();
        }
        #endregion

        #region dgCountry_CancelCommand
        protected void dgCountry_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgCountry.EditItemIndex = -1;
            LoadCountry();
        }
        #endregion

        #region dgCountry_UpdateCommand
        protected void dgCountry_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            TextBox txtEditName = (TextBox)e.Item.FindControl("txtEditName");
            DropDownList ddlEditSeqID = (DropDownList)e.Item.FindControl("ddlEditSeqID");

            if (txtEditName != null && ddlEditSeqID != null && !string.IsNullOrEmpty(txtEditName.Text))
            {
                short sCountryId = GMSUtil.ToShort(this.dgCountry.DataKeys[e.Item.ItemIndex]);

                if (sCountryId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    GMSCore.Entity.Country country = null;

                    try
                    {
                        country = sDataActivity.RetrieveCountryById(sCountryId, session);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    country.Name = txtEditName.Text.Trim();
                    country.SeqID = GMSUtil.ToShort(ddlEditSeqID.SelectedValue);
                    country.ModifiedBy = session.UserId;
                    country.ModifiedDate = DateTime.Now;

                    try
                    {
                        ResultType result = sDataActivity.UpdateCountry(ref country, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgCountry.EditItemIndex = -1;
                                LoadCountry();
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

        #region dgCountry_CreateCommand
        protected void dgCountry_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                TextBox txtNewName = (TextBox)e.Item.FindControl("txtNewName");
                DropDownList ddlNewSeqID = (DropDownList)e.Item.FindControl("ddlNewSeqID");

                if (txtNewName != null && !string.IsNullOrEmpty(txtNewName.Text) && ddlNewSeqID != null)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    GMSCore.Entity.Country country = new GMSCore.Entity.Country();

                    country.Name = txtNewName.Text.Trim();
                    country.SeqID = GMSUtil.ToShort(ddlNewSeqID.SelectedValue);
                    country.CreatedBy = session.UserId;
                    country.CreatedDate = DateTime.Now;

                    try
                    {
                        ResultType result = sDataActivity.CreateCountry(ref country, session);

                        switch (result)
                        {
                            case ResultType.Ok:

                                LoadCountry();
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

        #region dgCountry_DeleteCommand
        protected void dgCountry_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                short sCountryId = GMSUtil.ToShort(this.dgCountry.DataKeys[e.Item.ItemIndex]);

                if (sCountryId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    try
                    {
                        ResultType result = sDataActivity.DeleteCountry(sCountryId, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgCountry.EditItemIndex = -1;
                                LoadCountry();
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
