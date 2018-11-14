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
        protected DataSet ds = new DataSet();
        protected DataSet ds1 = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            string currentLink = "Products";
           
            //lblPageHeader.Text = "Products";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                //lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
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

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!Page.IsPostBack)
            {
                //preload

                ViewState["SortField"] = "ProductCode";
                ViewState["SortDirection"] = "ASC";
            }

            hidCoyID.Value = session.CompanyId.ToString();
            hidUserID.Value = session.UserId.ToString();
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
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

            int columnIndex = 10;//this will remove the second column
            DataTable copyDataTable;
            copyDataTable = ds.Tables[0].Copy();
            copyDataTable.DefaultView.Sort = "ProductCode ASC";
            copyDataTable.Columns.RemoveAt(columnIndex);

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
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            
            if (this.txtProductCode.Text.Trim() == "" && this.txtProductName.Text.Trim() == "" && 
                this.txtProductGroup.Text.Trim() == "" && this.txtProductGroupCode.Text.Trim() == "")
            {
                base.JScriptAlertMsg("Please input a product to search.");
                return;
            }

            string productCode = "%" + txtProductCode.Text.Trim() + "%";
            string productName = "%" + txtProductName.Text.Trim() + "%";
            string productGroupCode = "%" + txtProductGroupCode.Text.Trim() + "%";
            string productGroup = "%" + txtProductGroup.Text.Trim() + "%";
            string productForeignName = "%" + txtProductForeignName.Text.Trim() + "%";

            ProductsDataDALC dacl = new ProductsDataDALC();

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

                    if(txtWarehouse.Text.Trim() == "")
                        ds = sc.GetProductFullDetail(session.CompanyId, productCode, productName, productGroupCode, productGroup);
                    else
                        ds = sc.GetProductFullDetailByWarehouse(session.CompanyId, productCode, productName, productGroupCode, productGroup, txtWarehouse.Text.Trim());
                }

                if (session.StatusType.ToString() == "H")
                {
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        sc1.Url = session.CMSWebServiceAddress.Trim();
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                    if (session.StatusType.ToString() == "H" && txtWarehouse.Text.Trim() == "")
                        ds1 = sc1.GetUnpostedProductFullDetail(productCode, productName, productGroupCode, productGroup, productForeignName);
                    else if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() == "")
                        ds = sc1.GetProductFullDetail(productCode, productName, productGroupCode, productGroup, productForeignName);
                    else if (session.StatusType.ToString() == "H" && txtWarehouse.Text.Trim() != "")
                        ds1 = sc1.GetUnpostedProductFullDetailByWarehouse(productCode, productName, productGroupCode, productGroup, txtWarehouse.Text.Trim(), productForeignName);
                    else if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() != "")
                        ds = sc1.GetProductFullDetailByWarehouse(productCode, productName, productGroupCode, productGroup, txtWarehouse.Text.Trim(), productForeignName);
                }
                else if (session.StatusType.ToString() == "L")
                {
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        sc1.Url = session.CMSWebServiceAddress.Trim();
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() == "")
                        ds = sc1.GetProductFullDetail(productCode, productName, productGroupCode, productGroup, productForeignName);
                    else if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() != "")
                        ds = sc1.GetProductFullDetailByWarehouse(productCode, productName, productGroupCode, productGroup, txtWarehouse.Text.Trim(), productForeignName);

                    if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                        sc1.Url = session.GASLMSWebServiceAddress.Trim();
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() == "" && session.GASLMSWebServiceAddress.Trim() != "")
                        ds = sc1.GetProductFullDetail(productCode, productName, productGroupCode, productGroup, productForeignName);
                    else if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() != "" && session.GASLMSWebServiceAddress.Trim() != "")
                        ds = sc1.GetProductFullDetailByWarehouse(productCode, productName, productGroupCode, productGroup, txtWarehouse.Text.Trim(), productForeignName);

                    if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                        sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() == "" && session.WSDLMSWebServiceAddress.Trim() != "")
                        ds1 = sc1.GetProductFullDetail(productCode, productName, productGroupCode, productGroup, productForeignName);
                    else if (session.StatusType.ToString() == "L" && txtWarehouse.Text.Trim() != "" && session.WSDLMSWebServiceAddress.Trim() != "")
                        ds1 = sc1.GetProductFullDetailByWarehouse(productCode, productName, productGroupCode, productGroup, txtWarehouse.Text.Trim(), productForeignName);
                }
                else if (session.StatusType.ToString() == "S")
                {
                    string query = "CALL \"AF_API_GET_SAP_ITEMMASTERINFO\" ('" + productCode.Replace("%", "") + "', '" + productName.Replace("%", "") + "', '" + productGroupCode.Replace("%", "") + "', '" + productGroup.Replace("%", "") + "', '" + productForeignName.Replace("%", "") + "', '" + txtWarehouse.Text.Trim() + "')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "ProductCode", "ProductName", "ProductGroupCode", "Volume", "UOM", "WeightedCost", "OnOrderQuantity", "OnPOQuantity", "OnBOQuantity", "AvailableQuantity", "IsGasDivision", "IsWeldingDivision", "ProdForeignName", "TrackedByBatch", "TrackedBySerial", "ProductNotes", "IsActive", "ItemType", "ProductGroupName", "OnHandQuantity",
                    "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
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
                                ds.Tables[0].Rows[j][8] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][4]) - GMSUtil.ToDecimal(ds.Tables[0].Rows[j][5]);
                                ds.Tables[0].Rows[j][10] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][10].ToString()) + GMSUtil.ToDecimal(ds1.Tables[0].Rows[i][10].ToString());
                                break;
                            }
                        }
                    }
                }

                if ((session.StatusType.ToString() == "L") && ds1 != null && ds1.Tables.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            if (ds1.Tables[0].Rows[i][0].ToString() == ds.Tables[0].Rows[j][0].ToString())
                            {                               
                                ds.Tables[0].Rows[j][5] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][5].ToString()) + GMSUtil.ToDecimal(ds1.Tables[0].Rows[i][5].ToString()); // SO
                                ds.Tables[0].Rows[j][6] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][6].ToString()) + GMSUtil.ToDecimal(ds1.Tables[0].Rows[i][6].ToString()); // BO                               
                                ds.Tables[0].Rows[j][8] = GMSUtil.ToDecimal(ds.Tables[0].Rows[j][4]) - GMSUtil.ToDecimal(ds.Tables[0].Rows[j][5]); // OnHand - SO                                
                                break;
                            }
                        }
                    }

                }

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
                    dv.Sort = ViewState["SortField"].ToString() + " " + ViewState["SortDirection"].ToString();

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
            catch (Exception ex)
            {
                //this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                this.PageMsgPanel.ShowMessage("The connection to the server has failed! <br />For more information, please contact your System Administrator. ", MessagePanelControl.MessageEnumType.Alert);
            }          
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
