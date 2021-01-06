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

namespace GMSWeb.Reports.Setup
{
    public partial class Upload : GMSBasePage
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
                                                                            20);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");


            if (!IsPostBack)
            {
                LoadDDL();
            }
        }

        #region btnUpload_Click
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                { 
                    FileUpload1.SaveAs(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.DOC_PATH + "\\" + FileUpload1.FileName);

                    #region  Save Report detail into tbReport
                    LogSession session = base.GetSessionInfo();
                    ReportsActivity rActivity = new ReportsActivity();
                    GMSCore.Entity.Report report = new GMSCore.Entity.Report();

                    report.Description = txtReportName.Text.Trim();
                    report.FileName = FileUpload1.FileName;
                    report.ReportCategoryID = GMSUtil.ToShort(ddlCategoryName.SelectedItem.Value);
                    report.IsActive = true;
                    report.CreatedBy = session.UserId;
                    report.CreatedDate = DateTime.Now;

                    try
                    {
                        ResultType result = rActivity.CreateReport(ref report, session);

                        switch (result)
                        {
                            case ResultType.Ok:
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
                    #endregion

                    lblMsg.Text =  "File uploaded successfully. <br>" +
                                    "File Name: " + FileUpload1.PostedFile.FileName + "<br>" +
                                    "File Length: " + FileUpload1.PostedFile.ContentLength + " kb<br>" +
                                    "File Type: " + FileUpload1.PostedFile.ContentType;

                    this.txtReportName.Text = "";
                }
                catch(Exception ex)
                {
                    lblMsg.Text = "Error: " + ex.Message.ToString();
                }
            }
            else
            {
                lblMsg.Text = "You have not specified a file.";
            }
        }
        #endregion

        #region LoadDDL
        private void LoadDDL()
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

            this.ddlCategoryName.DataSource = lstCategory;
            this.ddlCategoryName.DataBind();
        }
        #endregion
    }
}
