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

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System.Collections.Generic;

namespace GMSWeb.Debtors.Commentary
{
    public partial class CommentaryHistory : GMSBasePage
    {
        private short CoyID = 1;
        private string AccountCode = "";
        private string CurrencyCode = "";
        string isLargeFont, isOptimizedTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.CoyID = short.Parse(Request.Params["CoyID"].ToString());
            this.AccountCode = Request.Params["AccountCode"];
            this.CurrencyCode = Request.Params["CurrencyCode"];

            //Getting LargerFont Cookies
            HttpCookie isLargeFontCookie = Request.Cookies["isLargeFont"];
            if (null == isLargeFontCookie)
                isLargeFont = "";
            else
                isLargeFont = isLargeFontCookie.Value == "true" ? "largeFont" : "";

            //Getting optimizedtable Cookies
            HttpCookie isOptimizedTableCookie = Request.Cookies["isOptimizedTable"];
            if (null == isOptimizedTableCookie)
                isOptimizedTable = "";
            else
                isOptimizedTable = isOptimizedTableCookie.Value == "true" ? "optimizedTable" : "";

            LoadDataGrid();
        }

        protected void LoadDataGrid()
        {

            IList<DebtorCommentary> lstComments = new SystemDataActivity().RetrieveCommentHistorySortByCommentDate(this.CoyID, this.AccountCode, this.CurrencyCode);

            int startIndex = ((dgCommentHistory.CurrentPageIndex + 1) * this.dgCommentHistory.PageSize) - (this.dgCommentHistory.PageSize - 1);
            int endIndex = (dgCommentHistory.CurrentPageIndex + 1) * this.dgCommentHistory.PageSize;

            if (lstComments != null && lstComments.Count > 0)
            {
                if (endIndex < lstComments.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstComments.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstComments.Count.ToString() + " " + "of" + " " + lstComments.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgCommentHistory.DataSource = lstComments;
            this.dgCommentHistory.DataBind();

            this.lblTitle.Text = "<h3 class='page-header'>Comment History for <b>" + this.AccountCode + "</b> with Currency: <b>" + this.CurrencyCode + "</b></h3>";
        }

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

        #region dgCommentHistory datagrid PageIndexChanged event handling
        protected void dgCommentHistory_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadDataGrid();
        }
        #endregion

        public string getIsOptimizedTable
        {
            get { return isOptimizedTable; }
        }

        public string getIsLargeFont
        {
            get { return isLargeFont; }
        }
    }
}
