using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;

using GMSCore;
using GMSWeb.CustomCtrl;
using GMSCore.Entity;
using GMSCore.Activity;
using System.Web.Services;
using System.Text;
using AjaxControlToolkit;
using System.Globalization;



namespace GMSWeb.Debtors.Debtors
{
    public partial class FinanceSearch : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Sales");

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId, 124);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            124);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                //preload
                ViewState["SortField"] = "Type";
                ViewState["SortDirection"] = "DESC";               
            }

        }

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgFinanceAttachment.CurrentPageIndex = 0;
            LoadData();
        }
        #endregion



        protected void dgFinanceAttachment_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }


         protected void dgFinanceAttachment_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton ibtnDownload = (LinkButton)e.Item.FindControl("linkName");
                if (ibtnDownload != null)
                {                    
                    ScriptManager sm = ScriptManager.GetCurrent(this.Page);
                    if (sm != null)
                    {
                        sm.RegisterPostBackControl(ibtnDownload);
                    }
                }

                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

                if (lnkDelete != null)
                {
                    lnkDelete.Attributes.Add("onclick", "return confirm_delete();");
                }
            }   

        }
       



        protected void SortFinanceAttachment(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["SortField"].ToString())
            {
                switch (ViewState["SortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["SortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["SortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["SortField"] = e.SortExpression;
                ViewState["SortDirection"] = "ASC";
            }
            LoadData();
        }


        protected void dgFinanceAttachment_Command(Object sender, DataGridCommandEventArgs e)
        {
            string ext = Path.GetExtension(e.CommandArgument.ToString());
            string ContentType = "";

            switch (((LinkButton)e.CommandSource).CommandName)
            {

                case "Load":
                    if (ext == ".asf")
                        ContentType = "video/x-ms-asf";
                    else if (ext == ".avi")
                        ContentType = "video/avi";
                    else if (ext == ".doc")
                        ContentType = "application/msword";
                    else if (ext == ".zip")
                        ContentType = "application/zip";
                    else if (ext == ".xls")
                        ContentType = "application/vnd.ms-excel";
                    else if (ext == ".gif")
                        ContentType = "image/gif";
                    else if (ext == ".jpg" || ext == "jpeg")
                        ContentType = "image/jpeg";
                    else if (ext == ".wav")
                        ContentType = "audio/wav";
                    else if (ext == ".mp3")
                        ContentType = "audio/mpeg3";
                    else if (ext == ".mpg" || ext == "mpeg")
                        ContentType = "video/mpeg";
                    else if (ext == ".mp3")
                        ContentType = "audio/mpeg3";
                    else if (ext == ".rtf")
                        ContentType = "application/rtf";
                    else if (ext == ".htm" || ext == "html")
                        ContentType = "text/html";
                    else if (ext == ".asp")
                        ContentType = "text/asp";
                    else
                        ContentType = "application/octet-stream";

                    Response.ContentType = ContentType.ToString();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + e.CommandArgument.ToString());
                    Response.TransmitFile("C://GMS/CRM/" + e.CommandArgument.ToString());
                    Response.End();
                    break;

                // Add other cases here, if there are multiple ButtonColumns in 
                // the DataGrid control.

                default:
                    // Do nothing.
                    break;

            }

        }


        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            try
            {
                string accountname = "%" + txtAccountName.Text.Trim() + "%";
               
                dacl.GetFinanceAttachmentSelect(accountname, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgFinanceAttachment.CurrentPageIndex + 1) * this.dgFinanceAttachment.PageSize) - (this.dgFinanceAttachment.PageSize - 1);
            int endIndex = (dgFinanceAttachment.CurrentPageIndex + 1) * this.dgFinanceAttachment.PageSize;

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (endIndex < ds.Tables[0].Rows.Count)
                    this.lblFinanceAttachmentSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                else
                    this.lblFinanceAttachmentSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();

                this.lblFinanceAttachmentSummary.Visible = true;
                this.dgFinanceAttachment.DataSource = dv;
                this.dgFinanceAttachment.DataBind();
                this.dgFinanceAttachment.Visible = true;
            }
            else
            {
                this.lblFinanceAttachmentSummary.Text = "No records.";
                this.lblFinanceAttachmentSummary.Visible = true;
                this.dgFinanceAttachment.DataSource = null;
                this.dgFinanceAttachment.DataBind();
            }

            resultList.Visible = true;

        }
        


    }
}
