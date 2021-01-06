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
    public partial class DivisionData : GMSBasePage
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
                                                                            5);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                LoadDivisionData();
            }
        }

        //Load Data
        #region LoadDivisionData
        private void LoadDivisionData()
        {
            LogSession session = base.GetSessionInfo();
            IList<Division> lstDivision = null;
            try
            {
                lstDivision = new SystemDataActivity().RetrieveAllDivisionListSortBySeqID(session);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            this.dgDivision.DataSource = lstDivision;
            this.dgDivision.DataBind();
        }
        #endregion

        #region dgDivision_ItemDataBound
        protected void dgDivision_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddlEditSeqID = (DropDownList)e.Item.FindControl("ddlEditSeqID");
                if (ddlEditSeqID != null)
                {
                    Division division = (Division)e.Item.DataItem;
                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    if (division != null)
                    {
                        // fill in drop down list
                        IList<Division> lstDivision = null;
                        lstDivision = sDataActivity.RetrieveAllDivisionListSortBySeqID();
                        ddlEditSeqID.DataSource = lstDivision;
                        ddlEditSeqID.DataBind();

                        int maxSeqID = sDataActivity.GetDivisionMaxSeqID();
                        int newMaxSeqID = maxSeqID + 1;
                        ddlEditSeqID.Items.Add(new ListItem(newMaxSeqID.ToString(), newMaxSeqID.ToString()));

                        ddlEditSeqID.SelectedValue = division.SeqID.ToString();
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
                    IList<Division> lstDivision = null;
                    lstDivision = sDataActivity.RetrieveAllDivisionListSortBySeqID();
                    ddlNewSeqID.DataSource = lstDivision;
                    ddlNewSeqID.DataBind();

                    int maxSeqID = sDataActivity.GetDivisionMaxSeqID();
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

        #region dgDivision_EditCommand
        protected void dgDivision_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgDivision.EditItemIndex = e.Item.ItemIndex;
            LoadDivisionData();
        }
        #endregion

        #region dgDivision_CancelCommand
        protected void dgDivision_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgDivision.EditItemIndex = -1;
            LoadDivisionData();
        }
        #endregion

        #region dgDivision_UpdateCommand
        protected void dgDivision_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            TextBox txtEditName = (TextBox)e.Item.FindControl("txtEditName");
            DropDownList ddlEditSeqID = (DropDownList)e.Item.FindControl("ddlEditSeqID");

            if (txtEditName != null && ddlEditSeqID != null && !string.IsNullOrEmpty(txtEditName.Text))
            {
                short sDivisionId = GMSUtil.ToShort(this.dgDivision.DataKeys[e.Item.ItemIndex]);

                if (sDivisionId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    Division division = null;

                    try
                    {
                        division = sDataActivity.RetrieveDivisionById(sDivisionId, session);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    division.Name = txtEditName.Text.Trim();
                    division.SeqID = GMSUtil.ToShort(ddlEditSeqID.SelectedValue);
                    division.ModifiedBy = session.UserId;
                    division.ModifiedDate = DateTime.Now;

                    try
                    {
                        ResultType result = sDataActivity.UpdateDivision(ref division, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgDivision.EditItemIndex = -1;
                                LoadDivisionData();
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

        #region dgDivision_CreateCommand
        protected void dgDivision_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                TextBox txtNewName = (TextBox)e.Item.FindControl("txtNewName");
                DropDownList ddlNewSeqID = (DropDownList)e.Item.FindControl("ddlNewSeqID");

                if (txtNewName != null && !string.IsNullOrEmpty(txtNewName.Text) && ddlNewSeqID != null)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    Division division = new Division();

                    division.Name = txtNewName.Text.Trim();
                    division.SeqID = GMSUtil.ToShort(ddlNewSeqID.SelectedValue);
                    division.CreatedBy = session.UserId;
                    division.CreatedDate = DateTime.Now;

                    try
                    {
                        ResultType result = sDataActivity.CreateDivision(ref division, session);

                        switch (result)
                        {
                            case ResultType.Ok:

                                LoadDivisionData();
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

        #region dgDivision_DeleteCommand
        protected void dgDivision_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                short sDivisionId = GMSUtil.ToShort(this.dgDivision.DataKeys[e.Item.ItemIndex]);

                if (sDivisionId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    try
                    {
                        ResultType result = sDataActivity.DeleteDivision(sDivisionId, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgDivision.EditItemIndex = -1;
                                LoadDivisionData();
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
