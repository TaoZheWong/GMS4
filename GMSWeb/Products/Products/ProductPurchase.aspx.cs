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

namespace GMSWeb.Products.Products
{
    public partial class ProductPurchase : GMSBasePage
    {
        protected short loginUserOrAlternateParty = 0;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Products");

            LogSession session = base.GetSessionInfo();
                        
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Products"));
                return;
            }

            DataSet lstAlterParty = new DataSet();
            new GMSGeneralDALC().GetAlternatePartyByAction(session.CompanyId, session.UserId, "Supplier Invoice", ref lstAlterParty);
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
                                                                            90);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            90);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Products"));

            if (!Page.IsPostBack)
            {
                //preload
                trnDateFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
                trnDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
                PopulateProductManager();
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
            string supplierAccountCode = "%" + txtSupplierAccountCode.Text.Trim() + "%";
            string supplierAccountName = "%" + txtSupplierAccountName.Text.Trim() + "%";
            string productCode = "%" + txtProductCode.Text.Trim() + "%";
            string productName = "%" + txtProductName.Text.Trim() + "%";
            string productGroup = "%" + txtProductGroup.Text.Trim() + "%";
            string prouductManagerID = ddlProductManager.SelectedValue;

            this.dgData.Columns[10].HeaderText = "Total (" + session.DefaultCurrency + ")"; 

            ProductsDataDALC dacl = new ProductsDataDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetProductPurchase(session.CompanyId, dateFrom, dateTo, supplierAccountCode,
                                        supplierAccountName, productCode, productName, productGroup, 
                                        prouductManagerID, loginUserOrAlternateParty, ref ds);
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
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = ds;
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
    }
}
