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
    public partial class DebtorDetails : GMSBasePage
    {
        private short CoyID = 1;
        private string AccountCode = "";
        private string CurrencyCode = "";
        private short Type = 1;
        private DateTime AsOfDate = new DateTime();
        private short dayRangeFrom = 0;
        private short dayRangeTo = 365;
        private string SalesPersonID = "";
        private string SalesPersonType = "";
        protected short loginUserOrAlternateParty = 0;

        string isLargeFont, isOptimizedTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.CoyID = short.Parse(Request.Params["CoyID"].ToString());
            this.AccountCode = Request.Params["AccountCode"];
            this.CurrencyCode = Request.Params["CurrencyCode"];
            this.Type = short.Parse(Request.Params["Type"].ToString());
            this.AsOfDate = GMSUtil.ToDate(Request.Params["AsOfDate"].ToString());
            this.SalesPersonID = Request.Params["SalesPersonID"];
            this.SalesPersonType = Request.Params["SalesPersonType"];
           
            switch (Type)
            {
                case 1: dayRangeFrom = 0; dayRangeTo = 30; break;
                case 2: dayRangeFrom = 30; dayRangeTo = 60; break;
                case 3: dayRangeFrom = 60; dayRangeTo = 90; break;
                case 4: dayRangeFrom = 90; dayRangeTo = 120; break;
                case 5: dayRangeFrom = 120; dayRangeTo = 180; break;
                case 6: dayRangeFrom = 180; dayRangeTo = 365; break;
                case 7: dayRangeFrom = 365; dayRangeTo = 9999; break;

            }

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
            dcDALC.GetDebtorsDetails(session.CompanyId, AsOfDate, AccountCode, CurrencyCode, dayRangeFrom, dayRangeTo, SalesPersonID, SalesPersonType, ref ds);

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

            this.lblTitle.Text = "<h3 class='page-header'>Outstanding invoices for <b>" + this.AccountCode + "</b> with Currency: <b>" + this.CurrencyCode + "</b></h3>";
        }

        public string getIsOptimizedTable
        {
            get { return isOptimizedTable; }
        }

        public string getIsLargeFont
        {
            get { return isLargeFont; }
        }

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadDataGrid();
        }
        #endregion
    }
}
