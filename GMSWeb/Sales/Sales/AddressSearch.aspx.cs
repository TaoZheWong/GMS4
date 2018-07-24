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

namespace GMSWeb.Sales.Sales
{
    public partial class AddressSearch : GMSBasePage
    {
        string isLargeFont, isOptimizedTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"">
                    function SelectAccount(selectedObj,Address1,Address2,Address3,Address4,AddressCode,AddressID)
			        {                       	
				       
				        if(window.opener != null)
				        {
                            window.opener.SetSelectedAccountAddress(Address1,Address2,Address3,Address4,AddressCode,AddressID);
				        }
				        window.close();
				        return false;
			        }	
              </script>
                ";

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

            if (Request.Params["AccountCode"] != null)
            {
                hidAccountCode.Value = Request.Params["AccountCode"].ToString();
            }

            RetrieveCustomer();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);

            
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            dtgResults.CurrentPageIndex = 0;
            RetrieveCustomer();
        }

        private void RetrieveCustomer()
        {

            string accountCode = string.Empty;

            accountCode = hidAccountCode.Value;

            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            (new QuotationDataDALC()).GetAccountAddressByAccountCodeSelect(session.CompanyId, accountCode, session.UserId, ref ds);

            int startIndex = ((dtgResults.CurrentPageIndex + 1) * this.dtgResults.PageSize) - (this.dtgResults.PageSize - 1);
            int endIndex = (dtgResults.CurrentPageIndex + 1) * this.dtgResults.PageSize;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (endIndex < ds.Tables[0].Rows.Count)
                    this.lblResultSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                else
                    this.lblResultSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

                this.lblResultSummary.Visible = true;
                this.dtgResults.DataSource = ds;
                this.dtgResults.DataBind();
            }
            else
            {
                this.lblResultSummary.Text = "No records.";
                this.lblResultSummary.Visible = true;
                this.dtgResults.DataSource = null;
                this.dtgResults.DataBind();
            }
        }

        #region dtgResults datagrid PageIndexChanged event handling
        protected void dtgResults_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            RetrieveCustomer();
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
