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

namespace GMSWeb.HR.Resources
{
    public partial class View : GMSBasePage
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
                                                                            30);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["URL"] != null && !string.IsNullOrEmpty(Request.QueryString["URL"]))
                {
                    transmitFile(Request.QueryString["URL"]);
                }
                    this.dgModify.CurrentPageIndex = 0;
                    LoadReportCategoryDDL();
                    //LoadReportList();
            }
        }

        #region btnUpload_Click
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    LogSession session = base.GetSessionInfo();
                    string folderPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\Resources\\" + ddlReportCategory.SelectedItem.Text;
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    FileUpload1.SaveAs(folderPath + "\\" + FileUpload1.FileName);

                    lblMsg.Text = "File uploaded successfully. <br>" +
                                    "File Name: " + FileUpload1.PostedFile.FileName + "<br>" +
                                    "File Length: " + FileUpload1.PostedFile.ContentLength + " kb<br>" +
                                    "File Type: " + FileUpload1.PostedFile.ContentType;
                    LoadReportList();

                }
                catch (Exception ex)
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

        private void LoadReportList()
        {
            int iTotalRecords = 0;
            List<GMSCore.Entity.Report> lstReport = new List<GMSCore.Entity.Report>();
            int i = 1;
            LogSession session = base.GetSessionInfo();
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\Resources\\" + ddlReportCategory.SelectedItem.Text;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            foreach (string sFile in
                System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\Resources\\" + ddlReportCategory.SelectedItem.Text + "\\"))
            {
                Report tReport = new Report();
                tReport.ReportID = (short)i;
                tReport.FileName = new System.IO.FileInfo(sFile).Name;
                lstReport.Add(tReport);
                i++;
            }
            iTotalRecords = lstReport.Count;
            Report[] rList = new Report[10];
            int count = 10;
            if (lstReport.Count <= 10 * (this.dgModify.CurrentPageIndex + 1))
                count = lstReport.Count - (10 * this.dgModify.CurrentPageIndex);
            lstReport.CopyTo(0 + this.dgModify.CurrentPageIndex * 10, rList, 0, count);
            lstReport.Clear();
            for (int j = 0; j < 10; j++)
            {
                if ((rList[j] as GMSCore.Entity.Report) != null)
                    lstReport.Add(rList[j]);
            }
            this.dgModify.DataSource = lstReport;
            this.dgModify.DataBind();

            if (iTotalRecords > 0)
            {
                Resultslbl.Visible = true;
                Pageslbl.Visible = true;
                this.dgModify.VirtualItemCount = iTotalRecords;
                this.dgModify.DataSource = lstReport;
                this.dgModify.DataBind();

                this.lblPage.Text = string.Format("Page {0} of {1}", this.dgModify.CurrentPageIndex + 1, this.dgModify.PageCount);
                if (this.dgModify.PageCount == 1)
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

                int iMaxRange = ((this.dgModify.CurrentPageIndex + 1) * this.dgModify.PageSize);
                if (iMaxRange > iTotalRecords)
                    iMaxRange = iTotalRecords;

                if (iMaxRange > 0)
                    this.lblTotalRecordsFound.Text = string.Format("{0} - {1} of {2}",
                                                                    (this.dgModify.CurrentPageIndex * this.dgModify.PageSize) + 1,
                                                                    iMaxRange,
                                                                     iTotalRecords);
                else
                    this.lblTotalRecordsFound.Text = "0";
            }
            else
            {
                Resultslbl.Visible = false;
                Pageslbl.Visible = false;
                this.lblTotalRecordsFound.Text = "0";
            }
        }

        #region SearchResults_PageNavigate
        protected void SearchResults_PageNavigate(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "First":
                    this.dgModify.CurrentPageIndex = 0;
                    LoadReportList();
                    break;

                case "Previous":
                    if (this.dgModify.CurrentPageIndex > 0)
                    {
                        this.dgModify.CurrentPageIndex--;
                        LoadReportList();
                    }
                    break;

                case "Next":
                    if (this.dgModify.CurrentPageIndex < this.dgModify.PageCount - 1)
                    {
                        this.dgModify.CurrentPageIndex++;
                        LoadReportList();
                    }
                    break;

                case "Last":
                    int pageMax = this.dgModify.VirtualItemCount / this.dgModify.PageSize;
                    if (this.dgModify.VirtualItemCount % this.dgModify.PageSize > 0)
                        pageMax++;
                    this.dgModify.CurrentPageIndex = this.dgModify.PageCount - 1;
                    LoadReportList();
                    break;


            }
        }
        #endregion

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

        #region dgModify_DeleteCommand
        protected void dgModify_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                short sReportId = GMSUtil.ToShort(this.dgModify.DataKeys[e.Item.ItemIndex]);
                HtmlInputHidden hidFileName = (HtmlInputHidden)e.Item.FindControl("hidFileName");

                if (sReportId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    string reportPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\Resources\\" + ddlReportCategory.SelectedItem.Text + "\\" + hidFileName.Value;

                    try
                    {
                        File.Delete(reportPath);
                        this.dgModify.EditItemIndex = -1;
                        LoadReportList();
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

        #region dgModify_ViewCommand
        protected void dgModify_ViewCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                LogSession session = base.GetSessionInfo();
                HtmlInputHidden hidFileName = (HtmlInputHidden)e.Item.FindControl("hidFileName");
                string url = AppDomain.CurrentDomain.BaseDirectory + "Data\\HR\\Resources\\" + ddlReportCategory.SelectedItem.Text + "\\" + hidFileName.Value;
                try
                {

                    Response.Redirect("View.aspx?URL=" + url);
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }
        }
        #endregion

        protected void transmitFile(String url)
        {
            if (File.Exists(url))
            {
                FileInfo downloadFile = new FileInfo(url);
                Response.Clear();
                Response.ClearHeaders();
                Response.Buffer = false;
                Response.ContentType = "application/octect-stream";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + downloadFile.Name);
                Response.AppendHeader("Content-Length", downloadFile.Length.ToString());
                Response.WriteFile(downloadFile.FullName);
                Response.Flush();
                Response.End();
            }
            else
            {
                Response.Write("<script>alert('Requested file is not found !'); window.close();window.location = '../../blank.htm';</script>");
                Response.Flush();
                Response.End();
            }
        }

        #region ddlReportCategory_SelectedIndexChanged
        protected void ddlReportCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlReportCategory.SelectedValue != "0")
            {
                this.dgModify.CurrentPageIndex = 0;
                this.MainPanel.Visible = true;
                LoadReportList();
            }
            else
            {
                this.MainPanel.Visible = false;
                this.PageMsgPanel.ShowMessage("You have not select a category.", MessagePanelControl.MessageEnumType.Alert);
            }
        }
        #endregion

        #region LoadDDL
        private void LoadReportCategoryDDL()
        {
            IList<ModuleReportCategory> lstCategory = new ReportsActivity().RetrieveAllModuleReportCategoryListByModuleCategoryIDSortBySeqID(new SystemDataActivity().RetrieveModuleCategoryByName("HR").ModuleCategoryID);
            this.ddlReportCategory.DataSource = lstCategory;
            this.ddlReportCategory.DataBind();
            this.ddlReportCategory.Items.Insert(0, new ListItem("[SELECT]", "0"));
        }
        #endregion
    }
}
