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

namespace GMSWeb.SysFinance.SharedInfo
{
    public partial class ForexRateHistory : GMSBasePage
    {
        private string homeCurrencyCode = "", foreignCurrencyCode = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.homeCurrencyCode = Request.Params["HOMECODE"];
            this.foreignCurrencyCode = Request.Params["FOREIGNCODE"];

            LoadDataGrid();
        }

        protected void LoadDataGrid()
        {

            IList<ForeignExchangeRate> lstForex = new ForexActivity().RetrieveForexHistorySortByCreatedDate(this.homeCurrencyCode,
                                                                                                        this.foreignCurrencyCode);

            this.dgForexHistory.DataSource = lstForex;
            this.dgForexHistory.DataBind();

            this.lblTitle.Text = "<h3>History Rates for Foreign Currency: <b>" + this.foreignCurrencyCode + "</b></h3>";
        }
    }
}
