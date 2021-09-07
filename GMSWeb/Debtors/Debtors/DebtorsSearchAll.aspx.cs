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
using GMSWeb.CustomCtrl;
using GMSCore.Activity;
using GMSCore.Entity;
using System.Collections.Generic;

namespace GMSWeb.Debtors.Debtors
{
    public partial class DebtorsSearchAll : GMSBasePage
    {
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
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            113);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            113);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!Page.IsPostBack)
            {
                Page.Form.DefaultFocus = this.txtAccountName.ClientID;
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

            if (this.txtAccountName.Text.Trim() == "")
            {
                base.JScriptAlertMsg("Please input a customer to search.");
                return;
                this.txtAccountName.Focus();
            }

            string accountName = "%" + txtAccountName.Text.Trim() + "%";

            DebtorCommentaryDALC dacl = new DebtorCommentaryDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetDebtorsForAll(session.CompanyId, accountName, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count <= 60)
                {
                    int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
                    int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

                    if (endIndex < ds.Tables[0].Rows.Count)
                        this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                            endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                    else
                        this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                            ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

                    this.lblSearchSummary.Visible = true;
                    this.dgData.DataSource = ds.Tables[0];
                    this.dgData.DataBind();
                    this.dgData.Visible = true;
                }
                else
                {
                    base.JScriptAlertMsg("There are too many results. Please be more specific with your search term!");
                    this.dgData.DataSource = null;
                    this.dgData.DataBind();
                }
            }
            else
            {
                this.lblSearchSummary.Text = "No records.";
                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = null;
                this.dgData.DataBind();
            }
            this.txtAccountName.Focus();
            resultList.Visible = true;
        }
        #endregion
    }
}
