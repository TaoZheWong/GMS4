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
using System.IO;


using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Reports.Setup
{
    public partial class ModuleModify : GMSBasePage
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
                                                                            19);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");


            if (!Page.IsPostBack)
            {
                LoadReportList();
            }
        }

        private void LoadReportList()
        {
            IList<GMSCore.Entity.ModuleReport> lstReport = null;
            try
            {
                lstReport = new ReportsActivity().RetrieveAllModuleReportSortBySeqIDName();
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            this.dgModify.DataSource = lstReport;
            this.dgModify.DataBind();
        }

        #region dgModify_ItemDataBound
        protected void dgModify_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region dgModify_EditCommand
        protected void dgModify_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgModify.EditItemIndex = e.Item.ItemIndex;
            LoadReportList();
        }
        #endregion

        #region dgModify_CancelCommand
        protected void dgModify_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgModify.EditItemIndex = -1;
            LoadReportList();
        }
        #endregion

        #region dgModify_UpdateCommand
        protected void dgModify_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            TextBox txtEditName = (TextBox)e.Item.FindControl("txtEditName");

            if (txtEditName != null && !string.IsNullOrEmpty(txtEditName.Text))
            {
                short sReportId = GMSUtil.ToShort(this.dgModify.DataKeys[e.Item.ItemIndex]);

                if (sReportId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    ReportsActivity rActivity = new ReportsActivity();
                    GMSCore.Entity.ModuleReport report = null;

                    try
                    {
                        report = rActivity.RetrieveModuleReportById(sReportId);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    report.Description = txtEditName.Text.Trim();
                    report.ModifiedBy = session.UserId;
                    report.ModifiedDate = DateTime.Now;

                    try
                    {
                        ResultType result = rActivity.UpdateModuleReport(ref report, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgModify.EditItemIndex = -1;
                                LoadReportList();
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

        #region dgModify_DeleteCommand
        protected void dgModify_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                short sReportId = GMSUtil.ToShort(this.dgModify.DataKeys[e.Item.ItemIndex]);

                if (sReportId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    ReportsActivity rActivity = new ReportsActivity();
                    GMSCore.Entity.ModuleReport report = rActivity.RetrieveModuleReportById(sReportId);
                    string reportPath = AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.DOC_PATH + Path.DirectorySeparatorChar + report.FileName;

                    try
                    {
                        ResultType result = rActivity.DeleteModuleReport(sReportId, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                File.Delete(reportPath);
                                this.dgModify.EditItemIndex = -1;
                                LoadReportList();
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
