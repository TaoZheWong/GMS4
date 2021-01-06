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
using AjaxControlToolkit;

namespace GMSWeb
{
    public partial class Bulletin : GMSBasePage
    {
        protected int PageSize = 10;
        protected short userId = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            userId = session.UserId;

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                67);
            if (uAccess == null)
                lnkAddNews.Visible = false;

            if (!Page.IsPostBack)
            {
                ViewState["CurrentPageIndex"] = 0;
                if (Request.QueryString["PageIndex"] != null)
                    ViewState["CurrentPageIndex"] = GMSUtil.ToInt(Request.QueryString["PageIndex"]);
                LoadData();
            }
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            
            IList<GMSCore.Entity.Bulletin> lstMessages = null;

            try
            {
                lstMessages = new SystemDataActivity().RetrieveAllMessagesSortByDate();
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            PagedDataSource objPage = new PagedDataSource();
            objPage.AllowPaging = true;
            objPage.DataSource = lstMessages;
            objPage.PageSize = PageSize;
            objPage.CurrentPageIndex = (int)ViewState["CurrentPageIndex"];

            int startIndex = (((int)ViewState["CurrentPageIndex"] + 1) * PageSize) - (PageSize - 1);
            int endIndex = ((int)ViewState["CurrentPageIndex"] + 1) * PageSize;

            if (lstMessages != null && lstMessages.Count > 0)
            {
                if (endIndex < lstMessages.Count)
                    this.lblSearchSummary.Text = "News" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstMessages.Count.ToString();
                else
                    this.lblSearchSummary.Text = "News" + " " + startIndex.ToString() + " - " +
                        lstMessages.Count.ToString() + " " + "of" + " " + lstMessages.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.MyAccordion.DataSource = objPage;
            this.MyAccordion.DataBind();

            this.lnkPrevPage.Enabled = !objPage.IsFirstPage;
            this.lnkNextPage.Enabled = !objPage.IsLastPage;
        }
        #endregion

        #region Page Handler
        protected void ChangePageCommand(object source, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "ChangePage":
                    int i = int.Parse(e.CommandArgument.ToString());
                    ViewState["CurrentPageIndex"] = (int)ViewState["CurrentPageIndex"] + i;
                    Response.Redirect(Request.Url.AbsolutePath.ToString() + "?PageIndex=" + ViewState["CurrentPageIndex"].ToString());
                    break;
            }
        }
        #endregion

        #region AddNewsCommand
        protected void AddNewsCommand(object sender, CommandEventArgs e)
        {
                LogSession session = base.GetSessionInfo();

                if (!string.IsNullOrEmpty(txtNewTitle.Text) && !string.IsNullOrEmpty(txtNewMessage.Text))
                {
                    try
                    {
                        GMSCore.Entity.Bulletin bulletin = new GMSCore.Entity.Bulletin();
                        bulletin.Title = txtNewTitle.Text.ToString();
                        bulletin.Message = txtNewMessage.Text.ToString();
                        bulletin.CreatedBy = session.UserId;
                        bulletin.CreatedDate = DateTime.Now;

                        ResultType result = new BulletinActivity().CreateBulletin(ref bulletin, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                ViewState["CurrentPageIndex"] = 0;
                                Response.Redirect(Request.Url.AbsolutePath.ToString() + "?PageIndex=" + ViewState["CurrentPageIndex"].ToString());
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

        #region MyAccordion_ItemCommand
        protected void MyAccordion_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Button lnkEdit = (Button)sender;
                Panel panel = (Panel)lnkEdit.Parent;
                TextBox txtEditTitle = (TextBox)panel.FindControl("txtEditTitle");
                TextBox txtEditMessage = (TextBox)panel.FindControl("txtEditMessage");
                HtmlInputHidden hidMessageID = (HtmlInputHidden) panel.FindControl("hidMessageID");

                if (txtEditTitle != null && !string.IsNullOrEmpty(txtEditTitle.Text) && txtEditMessage != null && !string.IsNullOrEmpty(txtEditMessage.Text) && hidMessageID != null)
                {
                    LogSession session = base.GetSessionInfo();

                    GMSCore.Entity.Bulletin bulletin = GMSCore.Entity.Bulletin.RetrieveByKey(GMSUtil.ToShort(hidMessageID.Value));
                    bulletin.Title = txtEditTitle.Text.ToString();
                    bulletin.Message = txtEditMessage.Text.ToString();
                    bulletin.ModifiedBy = session.UserId;
                    bulletin.ModifiedDate = DateTime.Now;

                    try
                    {
                        ResultType result = new BulletinActivity().UpdateBulletin(ref bulletin, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                Response.Redirect(Request.Url.ToString());
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
            else if (e.CommandName == "Delete")
            {
                short messageId = GMSUtil.ToShort(e.CommandArgument);

                if (messageId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    try
                    {
                        ResultType result = new BulletinActivity().DeleteBulletin(messageId, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                ViewState["CurrentPageIndex"] = 0;
                                Response.Redirect(Request.Url.AbsolutePath.ToString() + "?PageIndex=" + ViewState["CurrentPageIndex"].ToString());
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
                            this.PageMsgPanel.ShowMessage("This message cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
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
