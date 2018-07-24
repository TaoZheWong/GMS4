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

namespace GMSWeb.Debtors.Debtors
{
    public partial class DebtorsSearch : GMSBasePage
    {
        #region Page_Load
        protected short loginUserOrAlternateParty = 0;
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
                                                                            94);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                             94);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                //preload
                ViewState["SortField"] = "AccountName";
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

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            //if (this.txtAccountCode.Text.Trim() == "" && this.txtAccountName.Text.Trim() == "")
            //{
            //    base.JScriptAlertMsg("Please input a customer to search.");
            //    return;
            //}

            string accountCode = "%" + txtAccountCode.Text.Trim() + "%";
            string accountName = "%" + txtAccountName.Text.Trim() + "%";

            DebtorCommentaryDALC dacl = new DebtorCommentaryDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetDebtors(session.CompanyId, accountCode, accountName, loginUserOrAlternateParty, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
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
                this.dgData.Visible = true;
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

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
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

        #region btnAddProspect_Click
        protected void btnAddProspect_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddEditProspect.aspx");
        }
        #endregion
    }
}
