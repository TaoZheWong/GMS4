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
using System.Collections.Generic;

namespace GMSWeb.Sales.Sales
{
    public partial class QuotationSearch : GMSBasePage
    {
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
                                                                           101);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            101);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Sales"));


            if (!Page.IsPostBack)
            {
                //preload
                trnDateFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
                trnDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
                PopulateSalesman();
                PopulateStatus();              

                ViewState["SortField"] = "QuotationNo";
                ViewState["SortDirection"] = "DESC";
            }

            
            if (session.CompanyId.ToString() == "115" || session.CompanyId.ToString() == "116")
                btnAdd.Visible = false;
            

            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region PopulateSalesman
        private void PopulateSalesman()
        {
            LogSession session = base.GetSessionInfo();

            ProductsDataDALC dacl = new ProductsDataDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetSalesman(session.CompanyId, session.UserId, ref ds);
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
            }

            ddlSalesman.DataSource = ds;
            ddlSalesman.DataBind();

            ddlSalesman.Items.Insert(0, new ListItem("-All-", "%%"));
        }
        #endregion

        #region PopulateStatus
        private void PopulateStatus()
        {
            LogSession session = base.GetSessionInfo();

            IList<QuotationStatus> lstQuotationStatus = QuotationStatus.RetrieveAll();

            ddlQuotationStatus.DataSource = lstQuotationStatus;
            ddlQuotationStatus.DataBind();

            ddlQuotationStatus.Items.Insert(0, new ListItem("-All-", "%%"));
        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            /* Integration with A21
            try
            {
                GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                {
                    sc.Url = session.WebServiceAddress.Trim();
                }
                else
                    sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";
                DataTable dt = sc.A21QuotationUpdateStatus(session.CompanyId);
                IList<QuotationHeader> qhList = (IList<QuotationHeader>)QuotationHeader.RetrieveAll();
                foreach (QuotationHeader qh in qhList)
                {
                    if (qh.QuotationStatusID == "1" || qh.QuotationStatusID == "4" || qh.QuotationStatusID == "6")
                    {
                        DataRow[] dr = dt.Select("TrnNo = '" + qh.A21QuotationNo.ToString() + "'");
                        if (dr.Length > 0)
                        {
                            if (dr[0]["Completed"].ToString() == "0")
                            {
                                if (qh.QuotationStatusID != "6")
                                {
                                    qh.QuotationStatusID = "6";
                                    qh.Save();
                                }
                            }
                            else
                            {
                                if (qh.QuotationStatusID != "4")
                                {
                                    qh.QuotationStatusID = "4";
                                    qh.Save();
                                }
                            }
                        }
                        else
                        {
                            if (qh.QuotationStatusID != "1")
                            {
                                qh.QuotationStatusID = "1";
                                qh.Save();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ex", "<script language='javascript'>alert('" + ex.Message + "');</script>", false);
                //return;
            }*/

            DateTime dateFrom = GMSUtil.ToDate(trnDateFrom.Text.Trim());
            DateTime dateTo = GMSUtil.ToDate(trnDateTo.Text.Trim());
            string accountCode = "%" + txtCustomerAccountCode.Text.Trim() + "%";
            string accountName = "%" + txtCustomerAccountName.Text.Trim() + "%";
            string salesmanID = ddlSalesman.SelectedValue;
            string productCode = "%" + txtProductCode.Text.Trim() + "%";
            string productName = "%" + txtProductName.Text.Trim() + "%";
            string quotationStatusID = ddlQuotationStatus.SelectedValue;
            string quotationNo = "%" + txtQuotationNo.Text.Trim() + "%";
            string acknowledge = ddlAcknowledge.SelectedValue;

            QuotationDataDALC dacl = new QuotationDataDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetQuotationByWildcardSelect(session.CompanyId, dateFrom, dateTo, accountCode,
                                        accountName, productCode, productName, salesmanID, session.UserId, quotationStatusID, quotationNo, acknowledge, ref ds);
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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

        #region btnAdd_Click
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddEditQuotation.aspx");
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
    }
}
