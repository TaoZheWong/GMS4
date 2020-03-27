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
using System.IO;


using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Collections.Generic;
using System.Text;
using System.Web.Services;

namespace GMSWeb.Products.Products
{
    public partial class ProductsSearch : GMSBasePage
    {
        #region Page_Load
        protected short loginUserOrAlternateParty = 0;
        protected string userDivision;
        protected DataSet ds = new DataSet();
        protected DataSet ds1 = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            string currentLink = "Products";
           
            lblPageHeader.Text = "Products";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }

            if(session.IsOffline.ToString() == "True")
                Response.Redirect(base.OfflinePage(currentLink));

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            88);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            88);

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Product Search", ref lstAlterParty);
            if ((lstAlterParty != null) && (lstAlterParty.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < lstAlterParty.Tables[0].Rows.Count; i++)
                {
                    loginUserOrAlternateParty = GMSUtil.ToShort(lstAlterParty.Tables[0].Rows[i]["OnBehalfUserNumID"].ToString());
                }
            }
            else
                loginUserOrAlternateParty = session.UserId;

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!Page.IsPostBack)
            {
                //preload
                ViewState["SortField"] = "ProductCode";
                ViewState["SortDirection"] = "ASC";
            }

            getUserDivision(session,loginUserOrAlternateParty);//get user's division
            hidCoyID.Value = session.CompanyId.ToString();
            hidUserID.Value = session.UserId.ToString();
        }
        #endregion

        #region getUserDivision
        protected void getUserDivision(LogSession session,short usernumid)
        {
            DivisionUser du = DivisionUser.RetrieveByKey(session.CompanyId, usernumid);
            if (du != null)
            {
                if (du.DivisionID == "GAS")
                {
                    userDivision = "GAS";
                }
                else if (du.DivisionID == "WSD")
                {
                    userDivision = "WSD";
                }
            }
            else
                userDivision = "ALL";
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
            /*
            LogSession session = base.GetSessionInfo();
            
            GMSCore.Entity.BudgetSalesTeam bs = new GMSCore.Entity.BudgetSalesTeam();
            bs.CoyID = session.CompanyId;
            bs.TeamName = "Gas";
            bs.IsActive = true;

            ResultType result = new BudgetActivity().CreateBudgetSalesTeam(ref bs, session);
            switch (result)
            {
                case ResultType.Ok:
                    //LoadData();
                    break;
                default:
                    lblSearchSummary.Text = "Processing error of type : " + result.ToString();                        
                    return;
            }

            GMSCore.Entity.BudgetSalesTeam bse = new BudgetActivity().RetrieveBudgetSalesTeamByName(session.CompanyId, "Safety");
            bse.IsActive = false;
            bse.Save();
            
            GMSCore.Entity.BudgetSalesTeam bsd = new BudgetActivity().RetrieveBudgetSalesTeamByName(session.CompanyId, "Gas");
            bsd.Delete();*/           
        }
        #endregion

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            LoadData();
            Response.Clear();
            Response.Buffer = true;

            Response.AddHeader("content-disposition",
            "attachment;filename=Products.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            int columnIndex = 9;//this will remove the second column
            DataTable copyDataTable;
            copyDataTable = ds.Tables[0].Copy();
            copyDataTable.DefaultView.Sort = "ProductCode ASC";
            //copyDataTable.Columns.RemoveAt(columnIndex);
            copyDataTable.Columns.Remove("WeightedCost");
            copyDataTable.Columns.Remove("IsActive");

            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;            
            GridView1.DataSource = copyDataTable;
            GridView1.DataBind();

            //Change the Header Row back to white color
            GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF");

            //Apply style to Individual Cells
            GridView1.HeaderRow.Cells[0].Style.Add("background-color", "green");
            GridView1.HeaderRow.Cells[1].Style.Add("background-color", "green");
            GridView1.HeaderRow.Cells[2].Style.Add("background-color", "green");
            GridView1.HeaderRow.Cells[3].Style.Add("background-color", "green");
            GridView1.HeaderRow.Cells[4].Style.Add("background-color", "green");
            GridView1.HeaderRow.Cells[5].Style.Add("background-color", "green");
            GridView1.HeaderRow.Cells[6].Style.Add("background-color", "green");
            GridView1.HeaderRow.Cells[7].Style.Add("background-color", "green");
            GridView1.HeaderRow.Cells[8].Style.Add("background-color", "green");
            GridView1.HeaderRow.Cells[9].Style.Add("background-color", "green");

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridViewRow row = GridView1.Rows[i];
                //Change Color back to white
                row.BackColor = System.Drawing.Color.White;
                //Apply text style to each Row
                row.Attributes.Add("class", "textmode");                   
            }
            GridView1.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion
        /*
        #region dgData datagrid ItemDataBound event handling
        protected void dgData_ItemDataBound(object source, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                double AvailabeQty = 0;
                Label lblAvailable = (Label)e.Item.FindControl("lblAvailable");
                Label lblOnHandQty = (Label)e.Item.FindControl("lblOnHand");
                Label lblOnSOQty = (Label)e.Item.FindControl("lblOnSO");
                AvailabeQty = Convert.ToDouble(lblOnHandQty.Text) - Convert.ToDouble(lblOnSOQty.Text);
                lblAvailable.Text = AvailabeQty.ToString();
            }
        }
        #endregion
        */
        #region LoadData
        private void LoadData() {
            LogSession session = base.GetSessionInfo();
            DataSet dsWarehouseSearch = new DataSet();
            List<string> warehouseArray = new List<string>();
            UserAccessModule accessSearchWarehouse = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            151);
            if (accessSearchWarehouse != null) {
                new GMSGeneralDALC().GetWarehouseSearch(session.CompanyId, ref dsWarehouseSearch);
                if (dsWarehouseSearch.Tables[0].Rows.Count > 0) {
                    for (int i = 0; i < dsWarehouseSearch.Tables[0].Rows.Count; i++) {
                        warehouseArray.Add(dsWarehouseSearch.Tables[0].Rows[i]["Warehouse"].ToString());
                    }
                }
                if (this.txtProductCode.Text.Trim() == "" && this.txtProductName.Text.Trim() == "" &&
                this.txtProductGroup.Text.Trim() == "" && this.txtProductGroupCode.Text.Trim() == ""&&this.txtWarehouse.Text.Trim()==""&&this.txtProductManager.Text.Trim()=="") {
                    if (this.txtWarehouse.Text.Trim() == ""){
                        base.JScriptAlertMsg("Please input a product or warehouse to search.");
                        return;
                    }else if (!warehouseArray.Contains(this.txtWarehouse.Text.Trim().ToUpper())) {
                        base.JScriptAlertMsg("Please input a product to search for this warehouse.");
                        return;
                    }
                }
            }
            else {
                if (this.txtProductCode.Text.Trim() == "" && this.txtProductName.Text.Trim() == "" &&
                this.txtProductGroup.Text.Trim() == "" && this.txtProductGroupCode.Text.Trim() == "" && this.txtWarehouse.Text.Trim() == "" && this.txtProductManager.Text.Trim() == "") {
                    base.JScriptAlertMsg("Please input a product to search.");
                    return;
                }
            }

            string productCode = "%" + txtProductCode.Text.Trim() + "%";
            string productName = "%" + txtProductName.Text.Trim() + "%";
            string productGroupCode = "%" + txtProductGroupCode.Text.Trim() + "%";
            string productGroup = "%" + txtProductGroup.Text.Trim() + "%";
            string productForeignName = "%" + txtProductForeignName.Text.Trim() + "%";
            string productManager = "%" + txtProductManager.Text.Trim() + "%";

            string prodNameSQL = "";
            string tempproductName = productName.Substring(1, productName.Length - 2);
            string[] words = tempproductName.Split(' ');
            foreach (string word in words)
            {
                //construct productName conditions 
                prodNameSQL += " and p.ProductName like '%" + word + "%'";
            }

            ProductsDataDALC dacl = new ProductsDataDALC();
            
                getProduct(session, productCode, productName, productGroupCode, productGroup, productForeignName, prodNameSQL);

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

                DataView dv = ds.Tables[0].DefaultView;
                if (session.StatusType == "S")
                {
                    dv.RowFilter = "IsActive = 'true'";
                }else
                {
                    dv.RowFilter = "IsActive = 1";
                }
                
                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = dv;
                this.dgData.DataBind();
                this.btnExportToExcel.Visible = true;
            }
            else
            {
                this.lblSearchSummary.Text = "No records.";
                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = null;
                this.dgData.DataBind();
                this.btnExportToExcel.Visible = false;
            }
            resultList.Visible = true;
        }
        #endregion

        #region getProduct
        private void getProduct(LogSession session,string productCode, string productName,string productGroupCode, string productGroupName,string productForeignName, string prodNameSQL)
        {
            #region getProductData
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

                    if (txtWarehouse.Text.Trim() == "")
                        ds = sc.GetProductFullDetail(session.CompanyId, productCode, productName, productGroupCode, productGroupName);
                    else
                        ds = sc.GetProductFullDetailByWarehouse(session.CompanyId, productCode, productName, productGroupCode, productGroupName, txtWarehouse.Text.Trim());
                }

                if (session.StatusType.ToString() == "H")
                {
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        sc1.Url = session.CMSWebServiceAddress.Trim();
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    if (session.StatusType.ToString() == "H" && txtWarehouse.Text.Trim() == "")
                        ds1 = sc1.GetUnpostedProductFullDetail(productCode, productName, productGroupCode, productGroupName, productForeignName);
                    else if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() == "")
                        ds = sc1.GetProductFullDetail(productCode, productName, productGroupCode, productGroupName, productForeignName);
                    else if (session.StatusType.ToString() == "H" && txtWarehouse.Text.Trim() != "")
                        ds1 = sc1.GetUnpostedProductFullDetailByWarehouse(productCode, productName, productGroupCode, productGroupName, txtWarehouse.Text.Trim(), productForeignName);
                    else if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() != "")
                        ds = sc1.GetProductFullDetailByWarehouse(productCode, productName, productGroupCode, productGroupName, txtWarehouse.Text.Trim(), productForeignName);
                }
                else if (session.StatusType.ToString() == "L")
                {
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if(session.CompanyId != 120 && userDivision =="ALL")
                    {
                        if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                            sc1.Url = session.CMSWebServiceAddress.Trim();
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                        if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() == "")
                            ds = sc1.GetProductFullDetail(productCode, productName, productGroupCode, productGroupName, productForeignName);
                        else if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() != "")
                            ds = sc1.GetProductFullDetailByWarehouse(productCode, productName, productGroupCode, productGroupName, txtWarehouse.Text.Trim(), productForeignName);

                      
                    }
                    if(session.CompanyId == 120 && userDivision == "GAS")
                    {
                        if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                            sc1.Url = session.GASLMSWebServiceAddress.Trim();
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                        if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() == "" && session.GASLMSWebServiceAddress.Trim() != "")
                            ds = sc1.GetProductFullDetail(productCode, productName, productGroupCode, productGroupName, productForeignName);
                        else if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() != "" && session.GASLMSWebServiceAddress.Trim() != "")
                            ds = sc1.GetProductFullDetailByWarehouse(productCode, productName, productGroupCode, productGroupName, txtWarehouse.Text.Trim(), productForeignName);
                    }

                    if (session.CompanyId == 120 && userDivision == "WSD")
                    {
                        if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                            sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                        if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() == "" && session.WSDLMSWebServiceAddress.Trim() != "")
                            ds = sc1.GetProductFullDetail(productCode, productName, productGroupCode, productGroupName, productForeignName);
                        else if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() != "" && session.WSDLMSWebServiceAddress.Trim() != "")
                            ds = sc1.GetProductFullDetailByWarehouse(productCode, productName, productGroupCode, productGroupName, txtWarehouse.Text.Trim(), productForeignName);
                    }

                    if(session.CompanyId == 120 && userDivision == "ALL")
                    {
                        if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                            sc1.Url = session.GASLMSWebServiceAddress.Trim();
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                        if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() == "" && session.GASLMSWebServiceAddress.Trim() != "")
                            ds = sc1.GetProductFullDetail(productCode, productName, productGroupCode, productGroupName, productForeignName);
                        else if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() != "" && session.GASLMSWebServiceAddress.Trim() != "")
                            ds = sc1.GetProductFullDetailByWarehouse(productCode, productName, productGroupCode, productGroupName, txtWarehouse.Text.Trim(), productForeignName);

                        if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                            sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                        else
                            sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                        if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() == "" && session.WSDLMSWebServiceAddress.Trim() != "")
                            ds1 = sc1.GetProductFullDetail(productCode, productName, productGroupCode, productGroupName, productForeignName);
                        else if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() != "" && session.WSDLMSWebServiceAddress.Trim() != "")
                            ds1 = sc1.GetProductFullDetailByWarehouse(productCode, productName, productGroupCode, productGroupName, txtWarehouse.Text.Trim(), productForeignName);
                    }
                }
                else if (session.StatusType.ToString() == "S")
                {
                    string query = "CALL \"AF_API_GET_SAP_ITEMMASTERINFO\" ('" + productCode.Replace("%", "") + "', '" + productName.Replace("%", "") + "', '" + productGroupCode.Replace("%", "") + "', '" + productGroupName.Replace("%", "") + "', '" + productForeignName.Replace("%", "") + "', '" + txtWarehouse.Text.Trim() + "')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "ProductCode", "ProductName", "ProductGroupCode", "Volume", "UOM", "WeightedCost", "OnOrderQuantity", "OnPOQuantity", "OnBOQuantity", "AvailableQuantity", "IsGasDivision", "IsWeldingDivision", "ProdForeignName", "TrackedByBatch", "TrackedBySerial", "ProductNotes", "IsActive", "ItemType", "ProductGroupName", "OnHandQuantity",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    DataView dvOri = new DataView(ds.Tables[0]);
                    dvOri.RowFilter = "ProductName NOT LIKE '%DO NOT USE%'";
                    DataTable dtOri = dvOri.ToTable();
                    ds.Reset();
                    ds.Tables.Add(dtOri);
                }
                string dealerPrice = "0.00";
                string userPrice = "0.00";
                string retailPrice = "0.00";
                ds.Tables[0].Columns.Add("DealerPrice", typeof(string));
                ds.Tables[0].Columns.Add("UserPrice", typeof(string));
                ds.Tables[0].Columns.Add("RetailPrice", typeof(string));
                

                if (session.StatusType.ToString()=="S")
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        ds.Tables[0].Rows[j]["OnHandQuantity"] = Math.Round(GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["OnHandQuantity"].ToString()), 0);
                        ds.Tables[0].Rows[j]["AvailableQuantity"] = Math.Round(GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["AvailableQuantity"].ToString()), 0);
                        ds.Tables[0].Rows[j]["OnOrderQuantity"] = Math.Round(GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["OnOrderQuantity"].ToString()), 0); // SO
                        ds.Tables[0].Rows[j]["OnPOQuantity"] = Math.Round(GMSUtil.ToDecimal(ds.Tables[0].Rows[j]["OnPOQuantity"].ToString()), 0); // BO 
                        ds.Tables[0].Rows[j]["DealerPrice"] = dealerPrice;
                        ds.Tables[0].Rows[j]["UserPrice"] = userPrice;
                        ds.Tables[0].Rows[j]["RetailPrice"] = retailPrice;
                    }
     
                    DataSet ds2 = new DataSet();
                    new GMSGeneralDALC().SelectProductPriceByProductCode(session.CompanyId, productCode, productName, productGroupCode, productGroupName, prodNameSQL, ref ds2);

                    if (ds2.Tables[0].Rows.Count != 0)
                    {
                        for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                if (ds2.Tables[0].Rows[i][0].ToString() == ds.Tables[0].Rows[j][0].ToString())
                                {
                                    ds.Tables[0].Rows[j]["DealerPrice"] = ds2.Tables[0].Rows[i]["DealerPrice"].ToString();
                                    ds.Tables[0].Rows[j]["UserPrice"] = ds2.Tables[0].Rows[i]["UserPrice"].ToString();
                                    ds.Tables[0].Rows[j]["RetailPrice"] = ds2.Tables[0].Rows[i]["RetailPrice"].ToString();
                                    break;
                                }
                            }
                        }
                    }
                }

                if ((session.StatusType.ToString() == "H") && ds1 != null && ds1.Tables.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            if (ds1.Tables[0].Rows[i][0].ToString() == ds.Tables[0].Rows[j][0].ToString())
                            {
                                ds.Tables[0].Rows[j][4] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][4].ToString()) + GMSUtil.ToDecimal(ds1.Tables[0].Rows[i][4].ToString());
                                ds.Tables[0].Rows[j][5] = GMSUtil.ToDecimal(ds1.Tables[0].Rows[i][5].ToString());
                                ds.Tables[0].Rows[j][6] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][6].ToString()) + GMSUtil.ToDecimal(ds1.Tables[0].Rows[i][6].ToString());
                                ds.Tables[0].Rows[j][7] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][7].ToString()) + GMSUtil.ToDecimal(ds1.Tables[0].Rows[i][7].ToString());
                                ds.Tables[0].Rows[j][8] = Math.Round(GMSUtil.ToDecimal(ds.Tables[0].Rows[j][4]) - GMSUtil.ToDecimal(ds.Tables[0].Rows[j][5]), 0);
                                ds.Tables[0].Rows[j][10] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][10].ToString()) + GMSUtil.ToDecimal(ds1.Tables[0].Rows[i][10].ToString());
                                break;
                            }
                        }
                    }
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        ds.Tables[0].Rows[j][5] = Math.Round(GMSUtil.ToDecimal(ds.Tables[0].Rows[j][5].ToString()), 0); // SO
                        ds.Tables[0].Rows[j][6] = Math.Round(GMSUtil.ToDecimal(ds.Tables[0].Rows[j][6].ToString()), 0); // BO 
                        ds.Tables[0].Rows[j]["DealerPrice"] = dealerPrice;
                        ds.Tables[0].Rows[j]["UserPrice"] = userPrice;
                        ds.Tables[0].Rows[j]["RetailPrice"] = retailPrice;
                    }

                    DataSet ds2 = new DataSet();
                    new GMSGeneralDALC().SelectProductPriceByProductCode(session.CompanyId, productCode, productName, productGroupCode, productGroupName, prodNameSQL, ref ds2);

                    if (ds2.Tables[0].Rows.Count != 0)
                    {
                        for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                if (ds2.Tables[0].Rows[i][0].ToString() == ds.Tables[0].Rows[j][0].ToString())
                                {
                                    ds.Tables[0].Rows[j]["DealerPrice"] = ds2.Tables[0].Rows[i]["DealerPrice"].ToString();
                                    ds.Tables[0].Rows[j]["UserPrice"] = ds2.Tables[0].Rows[i]["UserPrice"].ToString();
                                    ds.Tables[0].Rows[j]["RetailPrice"] = ds2.Tables[0].Rows[i]["RetailPrice"].ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
                if (session.StatusType.ToString() == "L" && userDivision !="ALL")
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        ds.Tables[0].Rows[j][5] = Math.Round(GMSUtil.ToDecimal(ds.Tables[0].Rows[j][5].ToString()), 0); // SO
                        ds.Tables[0].Rows[j][6] = Math.Round(GMSUtil.ToDecimal(ds.Tables[0].Rows[j][6].ToString()), 0); // BO                             
                        ds.Tables[0].Rows[j][8] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][4]) - GMSUtil.ToDecimal(ds.Tables[0].Rows[j][5]); // OnHand - SO
                        ds.Tables[0].Rows[j]["DealerPrice"] = dealerPrice;
                        ds.Tables[0].Rows[j]["UserPrice"] = userPrice;
                        ds.Tables[0].Rows[j]["RetailPrice"] = retailPrice;
                    }
                   
                    DataSet ds2 = new DataSet();
                    new GMSGeneralDALC().SelectProductPriceByProductCode(session.CompanyId, productCode, productName, productGroupCode, productGroupName, prodNameSQL, ref ds2);

                    if (ds2.Tables[0].Rows.Count != 0)
                    {
                        for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                if (ds2.Tables[0].Rows[i][0].ToString() == ds.Tables[0].Rows[j][0].ToString())
                                {
                                    ds.Tables[0].Rows[j]["DealerPrice"] = ds2.Tables[0].Rows[i]["DealerPrice"].ToString();
                                    ds.Tables[0].Rows[j]["UserPrice"] = ds2.Tables[0].Rows[i]["UserPrice"].ToString();
                                    ds.Tables[0].Rows[j]["RetailPrice"] = ds2.Tables[0].Rows[i]["RetailPrice"].ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
                if (session.StatusType.ToString() == "L"  && userDivision == "ALL")
                {
                    if(ds1 != null && ds1.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                            {
                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    if (ds1.Tables[0].Rows[i][0].ToString() == ds.Tables[0].Rows[j][0].ToString())
                                    {
                                        ds.Tables[0].Rows[j][5] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][5].ToString()) + GMSUtil.ToDecimal(ds1.Tables[0].Rows[i][5].ToString()); // SO
                                        ds.Tables[0].Rows[j][6] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][6].ToString()) + GMSUtil.ToDecimal(ds1.Tables[0].Rows[i][6].ToString()); // BO                               
                               
                                        /*string x = "";
                                        foreach (DataColumn clmn in ds.Tables[0].Columns)
                                        {
                                            x = x + "," + clmn.ColumnName;
                                        }
                                        foreach (DataRow row in ds.Tables[0].Rows)
                                        {
                                            foreach (var item in row.ItemArray)
                                            {
                                                x = x + "," + item;
                                            }
                                            break;
                                        }
                                        this.PageMsgPanel.ShowMessage(x, MessagePanelControl.MessageEnumType.Alert);*/
                                        break;
                                    }
                                }

                            }
                    }
                    
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        ds.Tables[0].Rows[j][5] = Math.Round(GMSUtil.ToDecimal(ds.Tables[0].Rows[j][5].ToString()), 0); // SO
                        ds.Tables[0].Rows[j][6] = Math.Round(GMSUtil.ToDecimal(ds.Tables[0].Rows[j][6].ToString()), 0); // BO 
                        ds.Tables[0].Rows[j][8] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][4]) - GMSUtil.ToDecimal(ds.Tables[0].Rows[j][5]); // OnHand - SO
                        ds.Tables[0].Rows[j]["DealerPrice"] = dealerPrice;
                        ds.Tables[0].Rows[j]["UserPrice"] = userPrice;
                        ds.Tables[0].Rows[j]["RetailPrice"] = retailPrice;
                    }
                    DataSet ds2 = new DataSet();
                    new GMSGeneralDALC().SelectProductPriceByProductCode(session.CompanyId, productCode, productName, productGroupCode, productGroupName, prodNameSQL, ref ds2);

                    if (ds2.Tables[0].Rows.Count != 0)
                    {
                        for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                if (ds2.Tables[0].Rows[i][0].ToString() == ds.Tables[0].Rows[j][0].ToString())
                                {
                                    ds.Tables[0].Rows[j]["DealerPrice"] = ds2.Tables[0].Rows[i]["DealerPrice"].ToString();
                                    ds.Tables[0].Rows[j]["UserPrice"] = ds2.Tables[0].Rows[i]["UserPrice"].ToString();
                                    ds.Tables[0].Rows[j]["RetailPrice"] = ds2.Tables[0].Rows[i]["RetailPrice"].ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }
            #endregion
        }
        #endregion

        #region View SO
        public void lnkViewSO_Click(object sender, CommandEventArgs e)
        {
            string productCode = e.CommandArgument.ToString();

            string strPopup = "<script language='javascript' ID='script1'>"
           + "window.open('ProductOrderDetail.aspx?PRODUCTCODE="+ productCode + "&TYPE=SO"
           + "','new window', 'width=900,height=400 ,resizable=yes,status=yes,menubar=no,scrollbars=yes')"
           + "</script>";
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
        }
        #endregion

        #region View BO
        public void lnkViewBO_Click(object sender, CommandEventArgs e)
        {
            string productCode = e.CommandArgument.ToString();

            string strPopup = "<script language='javascript' ID='script1'>"
           + "window.open('ProductOrderDetail.aspx?PRODUCTCODE=" + productCode + "&TYPE=BO"
           + "','new window', 'width=900,height=400 ,resizable=yes,status=yes,menubar=no,scrollbars=yes')"
           + "</script>";
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
        }
        #endregion

        #region View PO
        public void lnkViewPO_Click(object sender, CommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string productCode = e.CommandArgument.ToString();
            string productGroupCode = "";
            #region getproductgroupcode
            DataSet ds = new DataSet();
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
                    ds = sc.GetProductDetail(session.CompanyId, productCode);
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
                    ds = sc1.GetProductDetail(productCode);

                }
                else if (session.StatusType.ToString() == "S")
                {
                    string query = "CALL \"AF_API_GET_SAP_ITEMMASTERINFO\" ('" + productCode + "', '', '', '', '', '')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "ProductCode", "ProductName", "ProductGroupCode", "Volume", "UOM", "WeightedCost", "OnOrderQuantity", "OnPOQuantity", "OnBOQuantity", "AvailableQuantity", "IsGasDivision", "IsWeldingDivision", "ProdForeignName", "TrackedByBatch", "TrackedBySerial", "ProductNotes", "IsActive", "ItemType", "ProductGroupName", "OnHandQuantity",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                }
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                productGroupCode = ds.Tables[0].Rows[0]["ProductGroupCode"].ToString();
            }
            #endregion
                string strPopup = "<script language='javascript' ID='script1'>"
           + "window.open('ProductOrderDetail.aspx?PRODUCTGROUPCODE=" + productGroupCode + "&PRODUCTCODE=" + productCode + "&TYPE=PO"
           + "','new window', 'width=900,height=400 ,resizable=yes,status=yes,menubar=no,scrollbars=yes')"
           + "</script>";
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
        }
        #endregion

        #region SortData
        protected void SortData(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["SortField"].ToString())
            {
                switch (ViewState["SortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["SortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["SortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["SortField"] = e.SortExpression;
                ViewState["SortDirection"] = "ASC";
            }
            LoadData();
        }
        #endregion

        #region GetDynamicContent
        [WebMethod]
        public static string GetDynamicContent(string contextKey)
        {
            short coyId = 1;
            string prodCode = "";
            bool canAccessProductStatus = false;
            bool isGasDivision = false;
            bool isWeldingDivision = false;
            short UserId = 0;

            string[] str = contextKey.Split(';');
            if (str != null && str.Length == 3)
            {
                coyId = GMSUtil.ToShort(str[0].Trim());
                prodCode = str[1].Trim();
                UserId = GMSUtil.ToShort(str[2].Trim());

            }

            DataSet ds = new DataSet();
            DataSet ds_lms = new DataSet();
            Company coy = Company.RetrieveByKey(coyId);
            DataSet ds2 = new DataSet();

            GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
            if (coy.StatusType.ToString() == "H" || coy.StatusType.ToString() == "A")
            {
                if (coy.WebServiceAddress != null && coy.WebServiceAddress.Trim() != "")
                {
                    sc.Url = coy.WebServiceAddress.Trim();
                }
                else
                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                ds = sc.GetProductStockStatus(coyId, prodCode);
            }

            // Get ProductStatus From LMS
            if (coy.StatusType.ToString() == "H")
            {
                CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                if (coy.CMSWebServiceAddress != null && coy.CMSWebServiceAddress.Trim() != "")
                {
                    sc1.Url = coy.CMSWebServiceAddress.Trim();
                }
                else
                    sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                ds_lms = sc1.GetProductStockStatus(prodCode);
                ds2 = sc.GetProductDetailByProductCode(coyId, prodCode);

            }
            else if (coy.StatusType.ToString() == "L")
            {
                CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                if (coy.CMSWebServiceAddress != null && coy.CMSWebServiceAddress.Trim() != "")
                {
                    sc1.Url = coy.CMSWebServiceAddress.Trim();
                }
                else
                    sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                ds = sc1.GetProductWarehouse(prodCode);
                ds2 = sc1.GetProductDetailByProductCode(prodCode);
                if (ds2 != null && ds.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    isGasDivision = Convert.ToBoolean(ds2.Tables[0].Rows[0]["IsGasDivision"].ToString());
                    isWeldingDivision = Convert.ToBoolean(ds2.Tables[0].Rows[0]["IsWeldingDivision"].ToString());
                }
            }
            else if (coy.StatusType.ToString() == "S")
            {
                string query = "CALL \"AF_API_GET_SAP_STOCK_STATUS\" ('" + prodCode + "', '', '', '', '', '2099-12-31', 'Y')";
                SAPOperation sop = new SAPOperation(coy.SAPURI.ToString(), coy.SAPKEY.ToString(), coy.SAPDB.ToString());
                ds = sop.GET_SAP_QueryData(coy.CoyID, query,
                "ItemCode", "Warehouse", "OnHand", "Committed", "Quantity", "WarehouseName", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
            }

            DivisionUser du = DivisionUser.RetrieveByKey(coyId, UserId);
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


            StringBuilder b = new StringBuilder();
            //padding:5px; margin:5px; background-color: #FEF6E8; border: 1px solid #969696;

            //b.Append("<table style='background-color:#f3f3f3; border: #336699 3px solid; ");
            b.Append("<table>");
            //b.Append("width:350px; font-size:10pt; font-family:Verdana;' cellspacing='0' cellpadding='3'>");
            //b.Append("<tr><td colspan='2'>");
            //b.Append("<b><i>Stock Status For Product " + prodCode + "</i></b>");
            //b.Append("</td></tr>");

            if (canAccessProductStatus && ((ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) || (ds2 != null && ds.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)))
            {
                if (coy.StatusType.ToString() == "H")
                {
                    for (int i = 0; i < ds_lms.Tables[0].Rows.Count; i++)
                    {
                        bool isDupe = false;
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            if (ds_lms.Tables[0].Rows[i][0].ToString() == ds.Tables[0].Rows[j][0].ToString())
                            {
                                ds.Tables[0].Rows[j][2] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][2].ToString()) + GMSUtil.ToDecimal(ds_lms.Tables[0].Rows[i][2].ToString());
                                isDupe = true;
                                break;
                            }
                        }
                        if (!isDupe)
                        {
                            ds.Tables[0].ImportRow(ds_lms.Tables[0].Rows[i]);
                        }
                    }
                }
                b.Append("<tr><td style='width:250px;'><b>Warehouse</b></td>");
                b.Append("<td><b>Quantity</b></td></tr>");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    b.Append("<tr>");
                    b.Append("<td>" + dr["Warehouse"].ToString() + " - " + dr["WarehouseName"].ToString() + "</td>");
                    b.Append("<td>" + dr["Quantity"].ToString() + "</td>");
                    b.Append("</tr>");
                }
            }
            else
            {
                b.Append("<tr>");
                b.Append("<td colspan='2'><i>Not Available.</i></td>");
                b.Append("</tr>");
            }

            b.Append("</table>");

            return b.ToString();
        }
        #endregion

    }
}
