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
namespace GMSWeb.Products.Products
{
    public partial class ProductMovement : GMSBasePage
    {
        #region Page_Load
        protected DataSet ds = new DataSet();
        protected DataSet ds1 = new DataSet();
        protected short loginUserOrAlternateParty = 0;

        protected void Page_Load(object sender, EventArgs e)
        {     
            //lblPageHeader.Text = "Products";
            LogSession session = base.GetSessionInfo();
            string currentLink = "Products";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                //lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);

           
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }

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


            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
                                                                            143);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            143);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!Page.IsPostBack)
            {
                //preload

                ViewState["SortField"] = "ProductCode";
                ViewState["SortDirection"] = "ASC";
            }
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

           
            DataTable copyDataTable;
            copyDataTable = ds.Tables[0].Copy();           

            LogSession session = base.GetSessionInfo();

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
                                                                          142);

            if (copyDataTable.Columns.Contains("ExchangeRate"))
            {
                copyDataTable.Columns.Remove("ExchangeRate");
                //copyDataTable.Columns.RemoveAt(17);
            }

            if (copyDataTable.Columns.Contains("Currency"))
            {
                copyDataTable.Columns.Remove("Currency");
                //copyDataTable.Columns.RemoveAt(16);
            }

            if (copyDataTable.Columns.Contains("CostWT"))
            {
                copyDataTable.Columns.Remove("CostWT");
                //copyDataTable.Columns.RemoveAt(15);
            }

            if (copyDataTable.Columns.Contains("Cost"))
            {
                copyDataTable.Columns.Remove("Cost");
                //copyDataTable.Columns.RemoveAt(14);
            }

            if (copyDataTable.Columns.Contains("TransNum"))
            {
                copyDataTable.Columns.Remove("TransNum");
            }

            if (copyDataTable.Columns.Contains("Field22"))
            {
                copyDataTable.Columns.Remove("Field22");
            }

            if (copyDataTable.Columns.Contains("Field23"))
            {
                copyDataTable.Columns.Remove("Field23");
            }

            if (copyDataTable.Columns.Contains("Field24"))
            {
                copyDataTable.Columns.Remove("Field24");
            }

            if (copyDataTable.Columns.Contains("Field25"))
            {
                copyDataTable.Columns.Remove("Field25");
            }

            if (copyDataTable.Columns.Contains("Field26"))
            {
                copyDataTable.Columns.Remove("Field26");
            }

            if (copyDataTable.Columns.Contains("Field27"))
            {
                copyDataTable.Columns.Remove("Field27");
            }

            if (copyDataTable.Columns.Contains("Field28"))
            {
                copyDataTable.Columns.Remove("Field28");
            }

            if (copyDataTable.Columns.Contains("Field29"))
            {
                copyDataTable.Columns.Remove("Field29");
            }

            if (copyDataTable.Columns.Contains("Field30"))
            {
                copyDataTable.Columns.Remove("Field30");
            }
          
            if (uAccess == null)
            {
                copyDataTable.Columns.Remove("AccountName");
                copyDataTable.Columns.RemoveAt(6);
                copyDataTable.Columns.Remove("AccountCode");
                copyDataTable.Columns.RemoveAt(5);                
            }  

            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = copyDataTable;
            GridView1.DataBind();
            //Change the Header Row back to white color
            GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF");           

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

        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty,
                                                                           142);
            if (uAccess == null)
            {
                this.dgData.Columns[11].Visible = false;
            }
            else
            {
                this.dgData.Columns[11].Visible = true;
            }

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

            if (this.txtProductCode.Text.Trim() == "")
            {
                base.JScriptAlertMsg("Please input a product to search.");
                return;
            }

            string productCode = txtProductCode.Text.Trim();           
            string warehouse = "%" + txtWarehouse.Text.Trim() + "%";


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
                    ds = sc.GetProductStockMovementByWarehouse(session.CompanyId, productCode, warehouse);
                }

                if (session.StatusType.ToString() == "H")
                {
                    DateTime LMSParallelRunEndDate = GMSUtil.ToDate(session.LMSParallelRunEndDate);
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                    {
                        sc1.Url = session.CMSWebServiceAddress.Trim();
                    }
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    ds1 = sc1.GetProductStockMovementByWarehouse(productCode, LMSParallelRunEndDate.ToString("yyyy-MM-dd"), warehouse);
                }
                else if (session.StatusType.ToString() == "L")
                {
                    warehouse = warehouse.Replace("%", "");
                    string query = "CALL \"AF_API_GET_SAP_STOCK_MOVEMENT\" ('" + productCode + "', '" + productCode + "', '"+ warehouse + "', '"+ warehouse + "', '', '')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "TrnType", "TrnNo", "TrnDate", "RefNo", "AccountCode", "AccountName", "ProductCode", "ProductName", "ProductGroupCode", "ProductGroupName", "ReceivedQty", "IssuedQty", "BalanceQty", "Cost", "CostWT", "Currency", "ExchangeRate", "Narration", "DocNo", "Warehouse",
                    "TransNum", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    System.Data.DataColumn newColumn = new System.Data.DataColumn("DBVersion", typeof(int));
                    newColumn.DefaultValue = 0;
                    ds.Tables[0].Columns.Add(newColumn);
                }


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (session.StatusType.ToString() == "H" && ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Merge(ds1.Tables[0], true, System.Data.MissingSchemaAction.Ignore);
                    }
                }
                //dacl.GetProductStockStatus(session.CompanyId, productCode, ref ds2);

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
                    if (session.StatusType.ToString() == "L")
                        dv.Sort = "TrnDate desc, TransNum desc, DocNo desc";
                    else
                        dv.Sort = "DBVersion, TrnDate desc";                    

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

       
    }
}