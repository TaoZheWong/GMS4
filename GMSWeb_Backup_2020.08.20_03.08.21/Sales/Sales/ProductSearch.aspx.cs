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
using GMSCore.Entity;
using GMSCore.Activity;

namespace GMSWeb.Sales.Sales
{
    public partial class ProductSearch : GMSBasePage
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Params["Package"] != null)
                {
                    lblProductCode.Text = "Package Product Code";
                    lblProductName.Text = "Package Product Description";
                }

                if (Request.Params["AccountCode"] != null)
                {
                    hidAccountCode.Value = Request.Params["AccountCode"].ToString();
                }
            }

            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"">

                    function SelectProduct(selectedObj,prdName,uom,weightedCost,listPrice,minSellingPrice,intercoPrice)
			        {				
				        var prdCode = selectedObj.firstChild.nodeValue.replace(/^\s+|\s+$/g, ''); 
                        
    
				        if(window.opener != null)
				        {
					        window.opener.SetSelectedProductCode(prdCode,prdName,uom,weightedCost,listPrice,minSellingPrice,intercoPrice);
				        }
				        window.close();
				        return false;
			        }	

                    function SelectPackage(selectedObj)
			        {	
                        var id = selectedObj.id.replace('lnkSelectPackage','hidPackage')
                        var packageID = document.getElementById(id).value.replace(/^\s+|\s+$/g, '');
				        if(window.opener != null)
				        {
					        window.opener.SetSelectedPackageID(packageID);
				        }
				        window.close();
				        return false;
			        }	
              </script>
                ";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (lblProductCode.Text != "Package Product Code")
            {
                dtgResults.CurrentPageIndex = 0;
                RetrieveProduct();
                dgPackage.Visible = false;
                dtgResults.Visible = true;
            }
            else
            {
                dgPackage.CurrentPageIndex = 0;
                RetrievePackage();
                dgPackage.Visible = true;
                dtgResults.Visible = false;
            }
        }
        #endregion

        #region RetrieveProduct
        private void RetrieveProduct()
        {

            string productCode = string.Empty;
            string productName = string.Empty;

            switch (rblSearchOption.SelectedValue)
            {
                case "0":
                    productCode = "%" + this.txtSearchProductCode.Text.Trim() + "%";
                    productName = "%" + this.txtSearchProductName.Text.Trim() + "%";
                    break;
                case "1":
                    productCode = this.txtSearchProductCode.Text.Trim() + "%";
                    productName = txtSearchProductName.Text.Trim() + "%";
                    break;
                case "2":
                    productCode = "%" + this.txtSearchProductCode.Text.Trim();
                    productName = "%" + txtSearchProductName.Text.Trim();
                    break;
                case "3":
                    productCode = this.txtSearchProductCode.Text.Trim() == string.Empty ? "%" : this.txtSearchProductCode.Text.Trim();
                    productName = txtSearchProductName.Text.Trim() == string.Empty ? "%" : txtSearchProductName.Text.Trim();
                    break;
            }

            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();

            if (hidAccountCode.Value.Trim() != "")

                (new QuotationDataDALC()).GetProductsByWildcardSelectByAccountCode(session.CompanyId, productCode, productName, hidAccountCode.Value, ref ds);
            else
                (new QuotationDataDALC()).GetProductsByWildcardSelect(session.CompanyId, productCode, productName, ref ds);

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
        #endregion

        #region dtgResults datagrid PageIndexChanged event handling
        protected void dtgResults_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            RetrieveProduct();
        }
        #endregion

        #region dtgResults_ItemDataBound
        protected void dtgResults_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblStockStatus = (Label)e.Item.FindControl("lblStockStatus");
                HtmlInputHidden hidProductCode = (HtmlInputHidden)e.Item.FindControl("hidProductCode");
                bool isGasDivision = false;
                bool isWeldingDivision = false;
                bool canAccessProductStatus = false;
                DataSet ds_lms = new DataSet();
                DataSet ds = new DataSet();
                DataSet ds2 = new DataSet();
                try
                {
                    if (session.StatusType.ToString() == "H" || session.StatusType.ToString() == "A")
                    {
                        GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                        if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                        {
                            sc.Url = session.WebServiceAddress.Trim();
                        }
                        else
                            sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                        ds = sc.GetProductStockStatus(session.CompanyId, hidProductCode.Value.Trim());
                        ds2 = sc.GetProductDetailByProductCode(session.CompanyId, hidProductCode.Value.Trim());
                    }
                    else if (session.StatusType.ToString() == "L")
                    {
                        CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        {
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        }
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                        ds = sc1.GetProductWarehouse(hidProductCode.Value.Trim());
                        ds2 = sc1.GetProductDetailByProductCode(hidProductCode.Value.Trim());
                        if (ds2 != null && ds.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                        {
                            isGasDivision = Convert.ToBoolean(ds2.Tables[0].Rows[0]["IsGasDivision"].ToString());
                            isWeldingDivision = Convert.ToBoolean(ds2.Tables[0].Rows[0]["IsWeldingDivision"].ToString());
                        }
                    }
                    else if (session.StatusType.ToString() == "S")
                    {

                        string query = "CALL \"AF_API_GET_SAP_STOCK_STATUS\" ('" + hidProductCode.Value.Trim() + "', '', '', '', '', '2099-12-31', 'Y')";
                        SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                        ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                        "ItemCode", "Warehouse", "OnHand", "Committed", "Quantity", "WarehouseName", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                        query = "CALL \"AF_API_GET_SAP_ITEMMASTERINFO\" ('" + hidProductCode.Value.Trim().Replace("%", "") + "', '', '', '', '', '')";
                        sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                        ds2 = sop.GET_SAP_QueryData(session.CompanyId, query,
                        "ProductCode", "ProductName", "ProductGroupCode", "Volume", "UOM", "WeightedCost", "OnOrderQuantity", "OnPOQuantity", "OnBOQuantity", "AvailableQuantity", "IsGasDivision", "IsWeldingDivision", "ProdForeignName", "TrackedByBatch", "TrackedBySerial", "ProductNotes", "IsActive", "ItemType", "ProductGroupName", "OnHandQuantity",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                    }
                }
                catch (Exception ex)
                {
                    this.JScriptAlertMsg(ex.Message);
                }
                DivisionUser du = DivisionUser.RetrieveByKey(session.CompanyId, session.UserId);
                if (du != null)
                {
                    if (du.DivisionID == "GAS" && isGasDivision)
                    {
                        canAccessProductStatus = true;
                    }
                    else if (du.DivisionID == "WSD" && isWeldingDivision)
                    {
                        canAccessProductStatus = true;
                    }
                }
                else
                    canAccessProductStatus = true;

                if (canAccessProductStatus && ((ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) || (ds2 != null && ds.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)))
                {

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["Quantity"].ToString() != "0" && Decimal.Round(Convert.ToDecimal(dr["Quantity"]), 6).ToString() != "0.000000")
                                lblStockStatus.Text = lblStockStatus.Text + "<span style=\"font-weight:bolder\">" + dr["Warehouse"].ToString() + "</span>-" + dr["WarehouseName"].ToString() +
                                                        ":" + Decimal.Round(Convert.ToDecimal(dr["Quantity"]), 6).ToString() + "<br>";
                        }
                    }
                    if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        if (ds2.Tables[0].Rows[0]["OnOrderQuantity"].ToString() != "0" && ds2.Tables[0].Rows[0]["OnOrderQuantity"].ToString() != "0.000000")
                        {
                            lblStockStatus.Text = lblStockStatus.Text + "<span style=\"font-weight:bolder\">On SO</span>" + ":" + Decimal.Round(Convert.ToDecimal(ds2.Tables[0].Rows[0]["OnOrderQuantity"]), 6).ToString() + "<br>";
                        }
                        if (ds2.Tables[0].Rows[0]["OnPOQuantity"].ToString() != "0" && ds2.Tables[0].Rows[0]["OnPOQuantity"].ToString() != "0.000000")
                        {
                            lblStockStatus.Text = lblStockStatus.Text + "<span style=\"font-weight:bolder\">On PO</span>" + ":" + Decimal.Round(Convert.ToDecimal(ds2.Tables[0].Rows[0]["OnPOQuantity"].ToString()), 6).ToString() + "<br>";
                        }
                    }
                    if (lblStockStatus.Text.Length >= 4)
                        lblStockStatus.Text = lblStockStatus.Text.Substring(0, lblStockStatus.Text.Length - 4);

                }
            }
        }
        #endregion

        #region RetrievePackage
        private void RetrievePackage()
        {

            string productCode = string.Empty;
            string productName = string.Empty;

            switch (rblSearchOption.SelectedValue)
            {
                case "0":
                    productCode = "%" + this.txtSearchProductCode.Text.Trim() + "%";
                    productName = "%" + this.txtSearchProductName.Text.Trim() + "%";
                    break;
                case "1":
                    productCode = this.txtSearchProductCode.Text.Trim() + "%";
                    productName = txtSearchProductName.Text.Trim() + "%";
                    break;
                case "2":
                    productCode = "%" + this.txtSearchProductCode.Text.Trim();
                    productName = "%" + txtSearchProductName.Text.Trim();
                    break;
                case "3":
                    productCode = this.txtSearchProductCode.Text.Trim() == string.Empty ? "%" : this.txtSearchProductCode.Text.Trim();
                    productName = txtSearchProductName.Text.Trim() == string.Empty ? "%" : txtSearchProductName.Text.Trim();
                    break;
            }

            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            (new QuotationDataDALC()).GetProductPackageByWildcardSelect(session.CompanyId, productCode, productName, ref ds);

            int startIndex = ((dgPackage.CurrentPageIndex + 1) * this.dgPackage.PageSize) - (this.dgPackage.PageSize - 1);
            int endIndex = (dgPackage.CurrentPageIndex + 1) * this.dgPackage.PageSize;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (endIndex < ds.Tables[0].Rows.Count)
                    this.lblResultSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                else
                    this.lblResultSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

                this.lblResultSummary.Visible = true;
                this.dgPackage.DataSource = ds;
                this.dgPackage.DataBind();
            }
            else
            {
                this.lblResultSummary.Text = "No records.";
                this.lblResultSummary.Visible = true;
                this.dgPackage.DataSource = null;
                this.dgPackage.DataBind();
            }
        }
        #endregion

        #region dgPackage datagrid PageIndexChanged event handling
        protected void dgPackage_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            RetrievePackage();
        }
        #endregion
    }
}
