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


namespace GMSWeb.Products.Products
{
    public partial class PurchaseHistory : GMSBasePage
    {
        private short CoyID = 1;
        private string ProductCode = "";
        private short UserId = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.CoyID = short.Parse(Request.Params["CoyID"].ToString());
            this.ProductCode = Request.Params["ProductCode"];
            this.UserId = short.Parse(Request.Params["UserId"].ToString());
            LoadDataGrid();
        }

        protected void LoadDataGrid()
        {
            LogSession session = base.GetSessionInfo();            
            DataSet ds = new DataSet();
            new GMSGeneralDALC().GetProductPurchasePrice(session.CompanyId, ProductCode, UserId, ref ds);
                        
            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;
           

            if (ds != null && ds.Tables.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;

                if (endIndex < ds.Tables[0].Rows.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();


                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = dv;
                this.dgData.DataBind();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            

            this.lblTitle.Text = "<h3>Purchase Price History for Product <b>" + this.ProductCode + "</b></h3>";
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
