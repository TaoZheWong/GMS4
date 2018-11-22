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
using GMSWeb.CustomCtrl;
using System.Collections.Generic;

namespace GMSWeb.Sales.Sales
{
    public partial class Invoice : GMSBasePage
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Sales");

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            116);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            116);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                //preload

                ViewState["SortField"] = "TrnNo";
                ViewState["SortDirection"] = "ASC";
            }

            if (!Page.IsPostBack)
            {
                //preload
                trnDateFrom.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("dd/MM/yyyy");
                trnDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            }

            string javaScript =
@"
<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>
";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
        }
        #endregion

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

            //if (this.txtProductCode.Text.Trim() == "" && this.txtProductName.Text.Trim() == "" && 
            //    this.txtProductGroup.Text.Trim() == "")
            //{
            //    base.JScriptAlertMsg("Please input a product to search.");
            //    return;
            //}

            string accountCode = "%" + txtAccountCode.Text.Trim() + "%";
            string accountName = "%" + txtAccountName.Text.Trim() + "%";
            DateTime dateFrom = GMSUtil.ToDate(trnDateFrom.Text.Trim());
            DateTime dateTo = GMSUtil.ToDate(trnDateTo.Text.Trim());


            string productCode = "%" + txtProductCode.Text.Trim() + "%";
            string productName = "%" + txtProductName.Text.Trim() + "%";
            string productGroupCode = "%" + txtProductGroup.Text.Trim() + "%";
            string productGroupName = "%" + txtProductGroupName.Text.Trim() + "%";
            string invoiceNo = "%" + txtInvoiceNo.Text.Trim() + "%";
            string productDetailDesc = "%" + txtProductDescription.Text.Trim() + "%";


            string salesmanID = "'0'".ToString();

            GMSGeneralDALC dacl = new GMSGeneralDALC();

            DataSet dsSalesPerson = new DataSet();

            try
            {
                dacl.GetSalesPersonListSelect(session.CompanyId, session.UserId, ref dsSalesPerson);
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
            }

            if (dsSalesPerson != null && dsSalesPerson.Tables.Count > 0 && dsSalesPerson.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsSalesPerson.Tables[0].Rows)
                {
                    salesmanID += ",'" + dr["salespersonid"].ToString() + "'";
                }
            }



            DataSet ds = new DataSet();
            DataSet ds_lms = new DataSet();
            DataSet ds_cn = new DataSet();

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
                    ds = sc.GetInvoicelist(session.CompanyId, accountCode, accountName, dateFrom, dateTo, salesmanID, productCode, productName, productGroupCode, productGroupName, invoiceNo, productDetailDesc);
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
                    ds_lms = sc1.GetInvoicelist(accountCode, accountName, dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), salesmanID, productCode, productName, productGroupCode, productGroupName, invoiceNo, productDetailDesc, LMSParallelRunEndDate.ToString("yyyy-MM-dd"));

                }
                else if (session.StatusType.ToString() == "L")
                {
                    DateTime LMSParallelRunEndDate = GMSUtil.ToDate(session.LMSParallelRunEndDate);
                    CMSWebService.CMSWebService sc1 = new CMSWebService.CMSWebService();
                    if (session.CMSWebServiceAddress != null && session.CMSWebServiceAddress.Trim() != "")
                        sc1.Url = session.CMSWebServiceAddress.Trim();
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";
                    ds = sc1.GetInvoicelist(accountCode, accountName, dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), salesmanID, productCode, productName, productGroupCode, productGroupName, invoiceNo, productDetailDesc, LMSParallelRunEndDate.ToString("yyyy-MM-dd"));

                    if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                        sc1.Url = session.GASLMSWebServiceAddress.Trim();
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                    if (session.GASLMSWebServiceAddress != null && session.GASLMSWebServiceAddress.Trim() != "")
                        ds = sc1.GetInvoicelist(accountCode, accountName, dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), salesmanID, productCode, productName, productGroupCode, productGroupName, invoiceNo, productDetailDesc, LMSParallelRunEndDate.ToString("yyyy-MM-dd"));

                    if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                        sc1.Url = session.WSDLMSWebServiceAddress.Trim();
                    else
                        sc1.Url = "http://localhost/CMS.WebServices/CMSWebService.asmx";

                    if (session.WSDLMSWebServiceAddress != null && session.WSDLMSWebServiceAddress.Trim() != "")
                        ds_lms = sc1.GetInvoicelist(accountCode, accountName, dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), salesmanID, productCode, productName, productGroupCode, productGroupName, invoiceNo, productDetailDesc, LMSParallelRunEndDate.ToString("yyyy-MM-dd"));

                }
                else if (session.StatusType.ToString() == "S")
                {
                    string query = "CALL \"AF_API_GET_SAP_GMS_IN_HEADER\" ('" + accountCode.Replace("%", "") + "', '" + accountName.Replace("%", "") + "', '" + dateFrom.ToString("yyyy-MM-dd") + "', '" + dateTo.ToString("yyyy-MM-dd") + "', '" + salesmanID.Replace("'", "") + "', '" + productCode.Replace("%", "") + "', '" + productName.Replace("%", "") + "', '" + productGroupCode.Replace("%", "") + "', '" + productGroupName.Replace("%", "") + "', '" + invoiceNo.Replace("%", "") + "','" + productDetailDesc.Replace("%", "") + "')";
                    SAPOperation sop = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());
                    ds = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "TrnNo", "DONo", "trntype", "trndate", "AccountName", "AccountCode", "Custponumber", "salespersonname", "salesperson", "narration", "DeliveryAddress1", "Currency", "RefNo1", "RefNo2", "DocNo", "gst", "Discount", "SubTotal", "billamt", "mobile",
                    "OfficePhone", "fax", "email", "contact", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");

                    query = "CALL \"AF_API_GET_SAP_GMS_CN_HEADER\" ('" + accountCode.Replace("%", "") + "', '" + accountName.Replace("%", "") + "', '" + dateFrom.ToString("yyyy-MM-dd") + "', '" + dateTo.ToString("yyyy-MM-dd") + "', '" + salesmanID.Replace("'", "") + "', '" + productCode.Replace("%", "") + "', '" + productName.Replace("%", "") + "', '" + productGroupCode.Replace("%", "") + "', '" + productGroupName.Replace("%", "") + "', '" + invoiceNo.Replace("%", "") + "','" + productDetailDesc.Replace("%", "") + "')";
                    ds_cn = sop.GET_SAP_QueryData(session.CompanyId, query,
                    "TrnNo", "DONo", "trntype", "trndate", "AccountName", "AccountCode", "Custponumber", "salespersonname", "salesperson", "narration", "DeliveryAddress1", "Currency", "RefNo1", "RefNo2", "DocNo", "gst", "Discount", "SubTotal", "billamt", "mobile",
                    "OfficePhone", "fax", "email", "contact", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                    
                    foreach (DataRow datRow in ds_cn.Tables[0].Rows)
                    {
                        ds.Tables[0].ImportRow(datRow);                        
                    }

                    System.Data.DataColumn newColumn = new System.Data.DataColumn("DBVersion", typeof(int));
                    newColumn.DefaultValue = 0;
                    ds.Tables[0].Columns.Add(newColumn);

                    System.Data.DataColumn newColumn1 = new System.Data.DataColumn("StatusType", typeof(string));
                    newColumn1.DefaultValue = "S";
                    ds.Tables[0].Columns.Add(newColumn1);
                   
                }

                if (ds_lms != null && ds_lms.Tables.Count > 0 && ds_lms.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables.Count == 0)
                        ds = ds_lms;
                    else
                    {
                        for (int i = 0; i < ds_lms.Tables[0].Rows.Count; i++)
                        {
                            ds.Tables[0].ImportRow(ds_lms.Tables[0].Rows[i]);
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
                }
                else
                {
                    this.lblSearchSummary.Text = "No records.";

                    this.lblSearchSummary.Visible = true;
                    this.dgData.DataSource = null;
                    this.dgData.DataBind();
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
