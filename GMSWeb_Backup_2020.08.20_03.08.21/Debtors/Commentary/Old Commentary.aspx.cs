using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Debtors.Commentary
{
    public partial class Commentary : GMSBasePage
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
                                                                            84);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                //preload
                txtAsOfDate.Text = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddDays(-1).ToString("dd/MM/yyyy");
                PopulateSalesperson();
                //dgData.CurrentPageIndex = 0;
                //LoadData();
            }
        }
        

        #region PopulateSalesperson
        private void PopulateSalesperson()
        {
            LogSession session = base.GetSessionInfo();
            DataSet lstSalesPerson = new DataSet();
            new DebtorCommentaryDALC().GetSalesPersonRecords(session.CompanyId, session.UserId, ref lstSalesPerson);

            if (lstSalesPerson != null && lstSalesPerson.Tables[0].Rows.Count > 0)
            {
                this.ddlSalesperson.DataSource = lstSalesPerson;
                this.ddlSalesperson.DataValueField = "SalesPersonID";
                this.ddlSalesperson.DataTextField = "SalesPersonName";
                this.ddlSalesperson.DataBind();
            }
        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            DateTime asOfDate = GMSUtil.ToDate(txtAsOfDate.Text.ToString());
            string salesPersonID = ddlSalesperson.SelectedValue.Trim();
            DebtorCommentaryDALC dcDALC = new DebtorCommentaryDALC();
            DataSet ds = new DataSet();
            try
            {
                dcDALC.GetDebtorsRecords(session.CompanyId, asOfDate, salesPersonID, session.UserId, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (endIndex < ds.Tables[0].Rows.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = ds;
            this.dgData.DataBind();
            hidAsOfDate.Value = txtAsOfDate.Text.ToString();
            hidSalesperson.Value = ddlSalesperson.SelectedValue.Trim();
            hidCoyID.Value = session.CompanyId.ToString();
        }
        #endregion

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
        }
        #endregion

        #region EditCommentCommand
        protected void EditCommentCommand(object sender, CommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            short companyId = GMSUtil.ToShort(hidCoyID.Value.Trim());
            string accountCode = hidAccountCode.Value.Trim();
            string currency = hidCurrency.Value.Trim();
            DateTime commentDate = GMSUtil.ToDate(hidCommentDate.Value.Trim());
            string comment = txtComment.Text.Trim();

            DebtorCommentary dc = DebtorCommentary.RetrieveByKey(companyId, accountCode, currency, commentDate, session.UserId);
            if (dc != null)
            {
                //if (dc.CommentDate < (new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1)))
                //{
                    //base.JScriptAlertMsg("You are not allowed to edit previous month's comments.");
                //}
                //else
                //{
                    dc.Comment = comment;
                    dc.ModifiedBy = session.UserId;
                    dc.ModifiedDate = DateTime.Now;
                    dc.Save();
                    dc.Resync();
                //}
            }
            else
            {
                //if (commentDate < new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1))
                //{
                //    base.JScriptAlertMsg("You are not allowed to edit previous month's comments.");
                //}
                //else
                //{
                    dc = new DebtorCommentary();
                    dc.AccountCode = accountCode;
                    dc.CoyID = companyId;
                    dc.CurrencyCode = currency;
                    dc.CommentDate = commentDate;
                    dc.Comment = comment;
                    dc.CreatedBy = session.UserId;
                    dc.CreatedDate = DateTime.Now;
                    dc.Save();
                    dc.Resync();
                //}
            }
            LoadData();
        }
        #endregion

        protected string FixCrLf(string value)
        {
   
            if (String.IsNullOrEmpty(value))
            { 
                return string.Empty;
            }
            else
            {
                return value.Replace(Environment.NewLine, "<br />");
            }
        }
    }
}
