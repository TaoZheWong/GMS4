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
    public partial class AccountSearch : GMSBasePage
    {
        string isLargeFont, isOptimizedTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"">
                    function SelectAccount(selectedObj,AccountName,Address1,Address2,Address3,Address4,AttentionTo,MobilePhone,OfficePhone,Fax,Email,SalesPersonID)
			        {
                        	
				        var accCode = selectedObj.firstChild.nodeValue;
                        accCode = accCode.replace(/^\s+|\s+$/g, '');
				        if(window.opener != null)
				        {
                            window.opener.SetSelectedAccountCode(accCode,AccountName,Address1,Address2,Address3,Address4,AttentionTo,MobilePhone,OfficePhone,Fax,Email,SalesPersonID);
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
            string accountName = string.Empty;

            switch (rblSearchOption.SelectedValue)
            {
                case "0":
                    accountCode = "%" + this.txtSearchAccountCode.Text + "%";
                    accountName = "%" + this.txtSearchAccountName.Text + "%";
                    break;
                case "1":
                    accountCode = this.txtSearchAccountCode.Text + "%";
                    accountName = txtSearchAccountName.Text + "%";
                    break;
                case "2":
                    accountCode = "%" + this.txtSearchAccountCode.Text;
                    accountName = "%" + txtSearchAccountName.Text;
                    break;
                case "3":
                    accountCode = this.txtSearchAccountCode.Text == string.Empty ? "%" : this.txtSearchAccountCode.Text;
                    accountName = txtSearchAccountName.Text == string.Empty ? "%" : txtSearchAccountName.Text;
                    break;
            }

            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            (new QuotationDataDALC()).GetAccountsByWildcardSelect(session.CompanyId, accountCode, accountName, session.UserId, ref ds);

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
