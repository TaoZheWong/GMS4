using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using System.Text;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Debtors.Debtors
{
    public partial class AddProspect : GMSBasePage
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
                                                                            94);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            PopulateDDL();


        }

        #region PopulateDDL
        private void PopulateDDL()
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

            SystemDataActivity dalc = new SystemDataActivity();
            IList<Currency> cList = null;
            try
            {
                cList = dalc.RetrieveAllCurrencyListSortByCode();
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
            }
            ddlCurrency.DataSource = cList;
            ddlCurrency.DataTextField = "CurrencyCode";
            ddlCurrency.DataValueField = "CurrencyCode";
            ddlCurrency.DataBind();

            SystemDataActivity sDataActivity = new SystemDataActivity();
            // fill in currency dropdown list
            IList<GMSCore.Entity.AccountIndustry> lstAccIndustry = null;
            try
            {
                lstAccIndustry = sDataActivity.RetrieveAllIndustry();
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
            }
            ddlIndustry.DataSource = lstAccIndustry;
            ddlIndustry.DataBind();


            
            Company coy = null;
            try
            {
                coy = dalc.RetrieveCompanyById(session.CompanyId, session);
            }
            catch (Exception ex)
            {
                JScriptAlertMsg(ex.Message);
            }

            ddlCurrency.SelectedValue = coy.DefaultCurrencyCode;

            IList<AccountTerritory> countryList = (new SystemDataActivity()).RetrieveAllTerritoryList();
            ddlCountry.DataSource = countryList;
            ddlCountry.DataBind();

        }
        #endregion

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        { 
            #region Add New Record.
            try
            {
                LogSession session = base.GetSessionInfo();
                DocumentNumber documentNumber = DocumentNumber.RetrieveByKey(session.CompanyId, (short)DateTime.Now.Year);
                if (documentNumber == null) //If tbDocumentNumber does not exist
                {
                    documentNumber = new DocumentNumber();
                    documentNumber.CoyID = session.CompanyId;
                    documentNumber.Year = (short)DateTime.Now.Year;
                    documentNumber.QuotationNo = "0001";
                    documentNumber.ExternalCourseCodePrefix = "E";
                    documentNumber.ExternalCourseCodeNumber = "001";
                    documentNumber.InternalCourseCodePrefix = "I";
                    documentNumber.InternalCourseCodeNumber = "001";
                    documentNumber.OrganizerID = 0;
                    documentNumber.EmployeeCourseRowID = 0;
                    documentNumber.EmployeeID = 0;
                    documentNumber.ProspectNo = "0001";
                }

                GMSCore.Entity.AccountProspect prospect = new GMSCore.Entity.AccountProspect();

                prospect.CoyID = session.CompanyId;
                prospect.AccountCode = "P" + DateTime.Now.ToString("yy") + "-" + documentNumber.ProspectNo;
                prospect.AccountName = txtCustomerName.Text.Trim();
                prospect.SalesPersonID = this.ddlSalesman.SelectedValue;
                prospect.DefaultCurrency = this.ddlCurrency.SelectedValue;
                prospect.Industry = this.ddlIndustry.SelectedValue;
                prospect.Country = this.ddlCountry.SelectedValue;
                if (!string.IsNullOrEmpty(txtCreditTerm.Text.Trim()))
                    prospect.CreditTerm = GMSUtil.ToByte(txtCreditTerm.Text.Trim());
                else
                    prospect.CreditTerm = 0;

                if (!string.IsNullOrEmpty(txtCreditLimit.Text.Trim()))
                    prospect.CreditLimit= GMSUtil.ToByte(txtCreditLimit.Text.Trim());
                else
                    prospect.CreditLimit = 0;

                prospect.Address1 = this.txtAddress1.Text.Trim();
                prospect.Address2 = this.txtAddress2.Text.Trim();
                prospect.Address3 = this.txtAddress3.Text.Trim();
                prospect.Address4 = this.txtAddress4.Text.Trim();
                prospect.PostalCode = this.txtPostalCode.Text.Trim();

                prospect.IsActive = true;
                prospect.CreatedBy = session.UserId;
                prospect.CreatedDate = DateTime.Now;
                prospect.Save();
                prospect.Resync();


                string nxtStr = ((short)(short.Parse(documentNumber.ProspectNo) + 1)).ToString();
                for (int i = nxtStr.Length; i < documentNumber.ProspectNo.Length; i++)
                {
                    nxtStr = "0" + nxtStr;
                }
                documentNumber.ProspectNo = nxtStr;
                documentNumber.Save();
                documentNumber.Resync();
               
                
                StringBuilder str = new StringBuilder();
                str.Append("<script language='javascript'>");
                str.Append("var result = confirm('Record added successfully! Add another one?'); if (result) {window.location.href = \"../../Debtors/Debtors/AddProspect.aspx\";}");
                str.Append("</script>");
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "add", str.ToString(), false);
                


            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                return;
            }
            #endregion



        }
        #endregion
    }
}
