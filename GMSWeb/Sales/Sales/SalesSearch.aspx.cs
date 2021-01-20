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
    public partial class SalesSearch : GMSBasePage
    {
        protected short loginUserOrAlternateParty = 0;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

            string currentLink = "Sales";


            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();

            }

            Master.setCurrentLink(currentLink); 
           

            LogSession session = base.GetSessionInfo();

            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Sales Detail", ref lstAlterParty);
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
                                                                            92);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            92);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!Page.IsPostBack)
            {
                //preload
                trnDateFrom.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("dd/MM/yyyy");
                trnDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
                PopulateProductManager();
                PopulateSalesman();
            }

            string javaScript =
@"
<script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>
";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }
        #endregion

        #region PopulateProductManager
        private void PopulateProductManager()
        {
            LogSession session = base.GetSessionInfo();

            ProductsDataDALC dacl = new ProductsDataDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetProductManagers(session.CompanyId, loginUserOrAlternateParty, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            ddlProductManager.DataSource = ds;
            ddlProductManager.DataBind();

            ddlProductManager.Items.Insert(0, new ListItem("-All-", "%%"));
        }
        #endregion

        #region PopulateSalesman
        private void PopulateSalesman()
        {
            LogSession session = base.GetSessionInfo();

            ProductsDataDALC dacl = new ProductsDataDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetSalesman(session.CompanyId, loginUserOrAlternateParty, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            ddlSalesman.DataSource = ds;
            ddlSalesman.DataBind();

            ddlSalesman.Items.Insert(0, new ListItem("-All-", "%%"));
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

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            DateTime dateFrom = GMSUtil.ToDate(trnDateFrom.Text.Trim());
            DateTime dateTo = GMSUtil.ToDate(trnDateTo.Text.Trim());
            string accountCode = "%" + txtCustomerAccountCode.Text.Trim() + "%";
            string accountName = "%" + txtCustomerAccountName.Text.Trim() + "%";
            string productCode = "%" + txtProductCode.Text.Trim() + "%";
            string productName = "%" + txtProductName.Text.Trim() + "%";
            string productGroup = "%" + txtProductGroup.Text.Trim() + "%";
            string productGroupCode = "%" + txtProductGroupCode.Text.Trim() + "%";
            string prouductManagerID = ddlProductManager.SelectedValue;
            string salesmanID = ddlSalesman.SelectedValue;

            ProductsDataDALC dacl = new ProductsDataDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetSalesByWildcardSelect(session.CompanyId, dateFrom, dateTo, accountCode,
                                        accountName, productCode, productName, productGroup, productGroupCode,
                                        prouductManagerID, salesmanID, loginUserOrAlternateParty, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (endIndex < ds.Tables[0].Rows.Count)
                    this.lblSearchSummary.Text = "Showing" + " " + startIndex.ToString() + " to " +
                        endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Showing" + " " + startIndex.ToString() + " to " +
                        ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = ds;
                this.dgData.DataBind();
            }
            else
            {
                this.lblSearchSummary.Text = "Showing 0 to 0 of 0 entries";
                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = null;
                this.dgData.DataBind();
            }

            resultList.Visible = true;
           
        }
        #endregion

        protected void lnkViewDetail_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            LinkButton lnkViewDetail = (LinkButton)sender;
            TableCell td = (TableCell)lnkViewDetail.Parent;

            string TrnType = ((HtmlInputHidden)td.FindControl("hidTrnType")).Value;
            string TrnNo = ((HtmlInputHidden)td.FindControl("hidTrnNo")).Value;
            string SrNo = ((HtmlInputHidden)td.FindControl("hidSrNo")).Value;
            string ProductCode = ((HtmlInputHidden)td.FindControl("hidProductCode")).Value;
            string AccountCode = ((HtmlInputHidden)td.FindControl("hidAccountCode")).Value;
            DataSet ds1 = new DataSet();
            (new GMSGeneralDALC()).IsProductManager(session.CompanyId, ProductCode, loginUserOrAlternateParty, ref ds1);
            if (ds1 == null || ds1.Tables.Count <= 0 || ds1.Tables[0].Rows.Count <= 0)
            {
                JScriptAlertMsg("You do not have access to this data.");
                return;
            }

            ClientScript.RegisterStartupScript(typeof(string), "",
                         string.Format("jsOpenReport('Sales/Sales/SalesDetail.aspx?TrnType={0}&TrnNnumber={1}&SrNo={2}&ProdCode={3}&AccountCode={4}');",
                                        TrnType,
                                         TrnNo, SrNo, ProductCode, AccountCode),
                                        true);

            //ClientScript.RegisterStartupScript(typeof(string), "CurrencyCode",
            //             string.Format("jsOpenReport('Finance/SharedInfo/WebForm1.aspx');"),
            //                            true);
            LoadData();
            //ClientScript.RegisterStartupScript(typeof(string), "CurrencyCode",
            //             "<script>jsWinOpen('ForexRateHistory.aspx?FOREIGNCODE=" + foreignCurrencyCode + "&HOMECODE=" + this.ddlHomeCurrency.SelectedValue + "','795','580','yes')</script>",
            //                            true);

        }
    }
}
