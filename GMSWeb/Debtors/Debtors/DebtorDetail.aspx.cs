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
using GMSCore.Entity;
using GMSCore.Activity;


namespace GMSWeb.Debtors.Debtors
{
    public partial class DebtorDetail : GMSBasePage
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
                                                                            95);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                //preload
                if (Request.Params["AccountCode"] != null)
                {
                    hidAccountCode.Value = Request.Params["AccountCode"].ToString().Trim();
                    LoadData();
                }
            }
        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();

            if (this.hidAccountCode.Value.Trim() == "")
            {
                base.JScriptAlertMsg("Please input a customer to view.");
                return;
            }

            string accountCode = this.hidAccountCode.Value.Trim();

            DebtorCommentaryDALC dacl = new DebtorCommentaryDALC();
            DataSet ds = new DataSet();
            try
            {
                dacl.GetDebtorDetailByAccountCode(session.CompanyId, accountCode, ref ds);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                this.lblAccountCode.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString();
                this.lblAccountName.Text = ds.Tables[0].Rows[0]["AccountName"].ToString();
                this.lblSalesPerson.Text = ds.Tables[0].Rows[0]["SalesPersonID"].ToString() + " - " + ds.Tables[0].Rows[0]["SalesPersonName"].ToString();
                this.lblCreditTerm.Text = ds.Tables[0].Rows[0]["CreditTerm"].ToString();
                this.lblCreditLimit.Text = ds.Tables[0].Rows[0]["CreditLimit"].ToString();
                this.lblDefaultCurrency.Text = ds.Tables[0].Rows[0]["DefaultCurrency"].ToString();
                this.lblIndustry.Text = ds.Tables[0].Rows[0]["Industry"].ToString();
                this.lblAddress1.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
                this.lblAddress2.Text = ds.Tables[0].Rows[0]["Address2"].ToString();
                this.lblAddress3.Text = ds.Tables[0].Rows[0]["Address3"].ToString();
                this.lblAddress4.Text = ds.Tables[0].Rows[0]["Address4"].ToString();
                this.lblPostalCode.Text = ds.Tables[0].Rows[0]["PostalCode"].ToString();
                this.lblContactPerson.Text = ds.Tables[0].Rows[0]["ContactPerson"].ToString();
                this.lblOfficePhone.Text = ds.Tables[0].Rows[0]["OfficePhone"].ToString();
                this.lblMobilePhone.Text = ds.Tables[0].Rows[0]["MobilePhone"].ToString();
                this.lblFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
                this.lblEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
            }
        }
        #endregion
    }
}
