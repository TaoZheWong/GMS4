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

namespace GMSWeb.Debtors.Commentary
{
    public partial class DebtorPaymentDetails : GMSBasePage
    {
        private short CoyID = 1;
        private string AccountCode = "";
        private string CurrencyCode = "";
        private short Type = 1;
        private DateTime receiptDateFrom = new DateTime();
        private DateTime receiptDateTo = new DateTime();
        private string paymentRefNo = "";
        private string SalesPersonID = "";
        private string SalesPersonType = "";
        protected short loginUserOrAlternateParty = 0;

        string isLargeFont, isOptimizedTable;


        protected void Page_Load(object sender, EventArgs e)
        {
            this.CoyID = short.Parse(Request.Params["CoyID"].ToString());
            this.AccountCode = Request.Params["AccountCode"];
             
            this.receiptDateTo = GMSUtil.ToDate(Request.Params["AsOfDate"].ToString());
            this.receiptDateFrom = GMSUtil.ToDate(new DateTime(receiptDateTo.Year, receiptDateTo.Month, 1).ToString("dd/MM/yyyy"));
            this.paymentRefNo = Request.Params["PaymentRefNo"];
            this.SalesPersonID = Request.Params["SalesPersonID"];
            this.SalesPersonType = Request.Params["SalesPersonType"];

            LogSession session = base.GetSessionInfo();

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Sales Detail", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {

                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;

            DataSet ds = new DataSet();
            new GMSGeneralDALC().CanUserAccessDocument(session.CompanyId, "ACCOUNT", this.AccountCode, loginUserOrAlternateParty, ref ds);

            if (!(Convert.ToBoolean(ds.Tables[0].Rows[0]["result"])))
            {
                JScriptAlertMsg("You do not have access to this data.");
                return;
            }

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

            LogSession session = base.GetSessionInfo();

            DebtorCommentaryDALC dcDALC = new DebtorCommentaryDALC();
            DataSet ds = new DataSet();

            
            dcDALC.GetDebtorsPaymentDetails(session.CompanyId, AccountCode, receiptDateFrom, receiptDateTo, loginUserOrAlternateParty, paymentRefNo, SalesPersonID, SalesPersonType, ref ds);

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

            this.lblTitle.Text = "<h3 class='page-header'>Debtor Collection Detail As Of " + Request.Params["AsOfDate"].ToString() + " for <b>" + this.AccountCode + "</b></h3>";
        }

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
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
