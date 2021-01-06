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

namespace GMSWeb.Finance.Forex
{
    public partial class ForexRateHistory : GMSBasePage
    {
        private string homeCurrencyCode = "", foreignCurrencyCode = "";

        string isLargeFont, isOptimizedTable;


        protected void Page_Load(object sender, EventArgs e)
        {
            this.homeCurrencyCode = Request.Params["HOMECODE"];
            this.foreignCurrencyCode = Request.Params["FOREIGNCODE"];

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

            IList<ForeignExchangeRate> lstForex = new ForexActivity().RetrieveForexHistorySortByCreatedDate(this.homeCurrencyCode,
                                                                                                        this.foreignCurrencyCode);

            this.dgForexHistory.DataSource = lstForex;
            this.dgForexHistory.DataBind();

            this.lblTitle.Text = "History Rates for Foreign Currency: <b>" + this.foreignCurrencyCode + "</b>";
        }

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
