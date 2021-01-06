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
using System.Collections.Generic;
using GMSCore.Activity;

namespace GMSWeb.Sales.Sales
{
    public partial class Salesman : GMSBasePage
    {
        protected short loginUserOrAlternateParty = 0;
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
                                                                            108);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            108);
            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                this.dgResult.CurrentPageIndex = 0;
                LoadData();
                //LoadReportList();
            }
        }
        #endregion

        #region LoadData
        protected void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            //IList<SalesPerson> lstSalesPerson = null;
            //lstSalesPerson = (new SystemDataActivity()).RetrieveAllSalesPersonByCompanyIDSortBySalesPersonID(GMSUtil.ToShort(session.CompanyId));

            DataSet lstSalesPerson = new DataSet();
            new DebtorCommentaryDALC().GetSalesPersonRecordsByUserNumIDCoyID(session.CompanyId, loginUserOrAlternateParty, ref lstSalesPerson);

            
            int startIndex = ((dgResult.CurrentPageIndex + 1) * this.dgResult.PageSize) - (this.dgResult.PageSize - 1);
            int endIndex = (dgResult.CurrentPageIndex + 1) * this.dgResult.PageSize;

            if (lstSalesPerson != null && lstSalesPerson.Tables[0].Rows.Count > 0)
            {
                if (endIndex < lstSalesPerson.Tables[0].Rows.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstSalesPerson.Tables[0].Rows.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstSalesPerson.Tables[0].Rows.Count.ToString() + " " + "of" + " " + lstSalesPerson.Tables[0].Rows.Count.ToString();

                this.lblSearchSummary.Visible = true;
                this.dgResult.DataSource = lstSalesPerson;
                this.dgResult.DataBind();
            }
            else
            {
                this.lblSearchSummary.Text = "No records.";
                this.lblSearchSummary.Visible = true;
                this.dgResult.DataSource = null;
                this.dgResult.DataBind();
            }
        }
        #endregion

        #region dgResult datagrid PageIndexChanged event handling
        protected void dgResult_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();

        }
        #endregion

        #region dgResult_EditCommand
        protected void dgResult_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            this.dgResult.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }
        #endregion

        #region dgResult_CancelCommand
        protected void dgResult_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgResult.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgResult_UpdateCommand
        protected void dgResult_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }

            TextBox txtEditDesignation = (TextBox)e.Item.FindControl("txtEditDesignation");
            TextBox txtEditMobilePhone = (TextBox)e.Item.FindControl("txtEditMobilePhone");
            TextBox txtEditDID = (TextBox)e.Item.FindControl("txtEditDID");
            TextBox txtEditFax = (TextBox)e.Item.FindControl("txtEditFax");
            TextBox txtEditEmail = (TextBox)e.Item.FindControl("txtEditEmail");
            HtmlInputHidden hidSalesPersonID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonID");

            SalesPerson sp = SalesPerson.RetrieveByKey(session.CompanyId, hidSalesPersonID.Value.Trim());
            if (sp != null)
            {
                sp.Designation = txtEditDesignation.Text.Trim();
                sp.MobilePhone = txtEditMobilePhone.Text.Trim();
                sp.DID = txtEditDID.Text.Trim();
                sp.Fax = txtEditFax.Text.Trim();
                sp.Email = txtEditEmail.Text.Trim();
                sp.Save();
                sp.Resync();
                dgResult.EditItemIndex = -1;
                LoadData();
            }

        }
        #endregion
    }
}
