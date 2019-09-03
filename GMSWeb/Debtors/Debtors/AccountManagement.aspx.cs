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
using System.IO;
using System.Data.SqlClient;

using GMSCore;
using GMSWeb.CustomCtrl;
using GMSCore.Entity;
using GMSCore.Activity;
using System.Web.Services;
using System.Text;
using AjaxControlToolkit;

namespace GMSWeb.Debtors.Debtors
{
	public partial class AccountManagement : GMSBasePage
	{

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
																			95);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            95);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Sales"));

			if (Request.Params["AccountCode"] != null)
			{
				DebtorCommentaryDALC dacl = new DebtorCommentaryDALC();
				DataSet ds = new DataSet();
				try
				{
					dacl.GetDebtors(session.CompanyId, Request.Params["AccountCode"].ToString().Trim(), "%%", loginUserOrAlternateParty, ref ds);
				}
				catch (Exception ex)
				{
					this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
				}

				if (ds.Tables[0].Rows.Count == 0)
				{
					Response.Redirect(base.UnauthorizedPage("Sales"));
				}
			}
			
			if (!Page.IsPostBack)
			{


				if (Request.Params["AccountCode"] != null)
				{                    

					hidAccountCode.Value = Request.Params["AccountCode"].ToString().Trim(); 
					lblAccountCode.Text = Request.Params["AccountCode"].ToString().Trim();
					ViewState["SortDirection"] = "ASC";

					LoadSalutationDDL();
					LoadIndustryDDL();

					btnLoadParticular_Click(btnLoadParticular, null);

					trnDateFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
					trnDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

					txtReceiptFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
					txtReceiptTo.Text = DateTime.Now.ToString("dd/MM/yyyy");




				}
			}



			Page.Form.Attributes.Add("enctype", "multipart/form-data");


			string javaScript = @"
			<script type=""text/javascript\"">

			function ViewInvoice(TrnNo,TrnType,DBVersion)
			{			
						
						var url = '/GMS3/Sales/Sales/ViewSalesOrderPopUp.aspx?TrnNo='+TrnNo+'&TrnType='+TrnType+'&DBVersion='+DBVersion;
						
						detailWindow = window.open(url,"""",""width="" + 650 + "",height="" + 600 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");	
						return false;
			}

			function dataSplit(ctr)
			{
				var addy_parts = ctr.value.split("":"");

				if (addy_parts[0].length < 1 && addy_parts[1].length < 1){

					alert(""Time format should be hh:mm"");
					ctr.value = '';
					return;
				}
				else
				{
					IntegerBoxControl_Validate(addy_parts[0],addy_parts[1], ctr);
				}
			}
			
			function IntegerBoxControl_Validate(data, data1, ctr)
			{
			// parse the input as an integer
			// var intValue = parseInt(document.getElementById('txtRefilledTime').value, 10);
			var intValue = parseInt(data, 10);
			var intValue1 = parseInt(data1, 10);

			// if this is not an integer
			if (isNaN(intValue))
			{
				// clear text box
				ctr.value = '';
				alert(""Time format should be hh:mm"");
				return;
			}
			// if this is an integer
			else
			{
	   
				switch (true)
				{  
					case (intValue == 0) :

					// clear text box
					ctr.value = ctr.value;
					break;
					case (intValue >= 0) :
						// put the parsed integer value in the text box
						ctr.value = ctr.value;
						break;
					case (intValue < 0) :
					{// put the positive parsed integer value in the text box
						alert(""Time format should be hh:mm"");
						ctr.value = '';
					}
					break;
				}
		  
			}

			// if this is not an integer
			if (isNaN(intValue1))
			{
				// clear text box
				ctr.value = '';
				alert(""Time format should be hh:mm"");
				return;
			}
			// if this is an integer
			else
			{
				switch (true)
				{
					case (intValue1 == 0) :
					// clear text box
					ctr.value =  ctr.value;
					break;
					case (intValue1 >= 0) :
					// put the parsed integer value in the text box
					ctr.value = ctr.value;
					break;
					case (intValue1 < 0) :
					{
						// put the positive parsed integer value in the text box
						alert(""Time format should be hh:mm"");
						ctr.value = '';
					}
					break;
				 }
		  
			}
			}
			</script>";
			Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);

			if (Page.IsPostBack)
			{

			}




		}

		#region LoadDDL
		private void LoadSalutationDDL()
		{
			DataSet dsSalutation = new DataSet();
			(new GMSGeneralDALC()).GetSalutationSelect(ref dsSalutation);
			ddlSalutation.DataSource = dsSalutation;
			ddlSalutation.DataBind();
		}
		#endregion


		#region LoadDDL
		private void LoadIndustryDDL()
		{
			LogSession session = base.GetSessionInfo();

			SystemDataActivity sDataActivity = new SystemDataActivity();
			// fill in currency dropdown list
			IList<GMSCore.Entity.AccountIndustry> lstAccIndustry = null;

			lstAccIndustry = sDataActivity.RetrieveAllIndustry();
			ddlPurchaseIndustry.DataSource = lstAccIndustry;
			ddlPurchaseIndustry.DataBind();


			DataSet dsTemp = new DataSet();
			(new QuotationDataDALC()).GetAllUOMByCoyIDSelect(session.CompanyId, ref dsTemp);
			ddlUOM.DataSource = dsTemp;
			ddlUOM.DataValueField = "UOM";
			ddlUOM.DataTextField = "UOM";
			ddlUOM.DataBind();


			IList<FinanceAttachmentType> lstFinanceAttachmentType = null;
			lstFinanceAttachmentType = new SystemDataActivity().RetrieveAllFinanceAttachmentTypeSortByID();
			this.ddlAttachmentType.DataSource = lstFinanceAttachmentType;
			this.ddlAttachmentType.DataBind();

			// load year ddl
			DataTable dtt1 = new DataTable();
			dtt1.Columns.Add("Year", typeof(string));

			for (int i = -5; i < 5; i++)
			{
				DataRow dr1 = dtt1.NewRow();
				dr1["Year"] = DateTime.Now.Year + i;

				dtt1.Rows.Add(dr1);
			}

			this.ddlPeriodYearFrom.DataSource = dtt1;
			this.ddlPeriodYearFrom.DataBind();

			this.ddlPeriodYearTo.DataSource = dtt1;
			this.ddlPeriodYearTo.DataBind();


			ddlPeriodYearFrom.SelectedValue = DateTime.Now.Year.ToString();
			ddlPeriodYearTo.SelectedValue = DateTime.Now.Year.ToString(); 

			// load month ddl
			DataTable dtt2 = new DataTable();
			dtt2.Columns.Add("Month", typeof(string));

			for (int i = 1; i < 13; i++)
			{
				DataRow dr2 = dtt2.NewRow();
				dr2["Month"] = i;

				dtt2.Rows.Add(dr2);
			}

			this.ddlPeriodMonthFrom.DataSource = dtt2;
			this.ddlPeriodMonthFrom.DataBind();

			this.ddlPeriodMonthTo.DataSource = dtt2;
			this.ddlPeriodMonthTo.DataBind();

			ddlPeriodMonthFrom.SelectedValue = DateTime.Now.Month.ToString();
			ddlPeriodMonthTo.SelectedValue = DateTime.Now.Month.ToString(); 

		}
		#endregion

		#region LoadContactsData
		private void LoadContactsData()
		{

			LogSession session = base.GetSessionInfo();

			GMSGeneralDALC dacl = new GMSGeneralDALC();
			DataSet ds = new DataSet();
			try
			{
				dacl.GetContactsSelect(session.CompanyId, this.hidAccountCode.Value.Trim(), ref ds);
			}
			catch (Exception ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}

			int startIndex = ((dgContacts.CurrentPageIndex + 1) * this.dgContacts.PageSize) - (this.dgContacts.PageSize - 1);
			int endIndex = (dgContacts.CurrentPageIndex + 1) * this.dgContacts.PageSize;

			DataView dv = ds.Tables[0].DefaultView;
			dv.Sort = ViewState["SortContactsField"].ToString() + " " + ViewState["SortDirection"].ToString();

			if (ds != null && ds.Tables[0].Rows.Count > 0)
			{
				if (endIndex < ds.Tables[0].Rows.Count)
					this.lblContactsSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
				else
					this.lblContactsSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();



				this.lblContactsSummary.Visible = true;

			}
			else
			{

				this.lblContactsSummary.Text = "No records.";
				this.lblContactsSummary.Visible = true;

			}
			this.dgContacts.DataSource = dv;
			this.dgContacts.DataBind();
			this.dgContacts.Visible = true;
		}
		#endregion

		#region LoadPurchasesData
		private void LoadPurchasesData()
		{
			LogSession session = base.GetSessionInfo();

			GMSGeneralDALC dacl = new GMSGeneralDALC();
			DataSet ds = new DataSet();
			try
			{
				dacl.GetPurchaseSelect(session.CompanyId, this.hidAccountCode.Value.Trim(), ref ds);
			}
			catch (Exception ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}

			int startIndex = ((dgPurchases.CurrentPageIndex + 1) * this.dgPurchases.PageSize) - (this.dgPurchases.PageSize - 1);
			int endIndex = (dgPurchases.CurrentPageIndex + 1) * this.dgPurchases.PageSize;

			DataView dv = ds.Tables[0].DefaultView;
			dv.Sort = ViewState["SortPurchasesField"].ToString() + " " + ViewState["SortDirection"].ToString();

			if (ds != null && ds.Tables[0].Rows.Count > 0)
			{
				if (endIndex < ds.Tables[0].Rows.Count)
					this.lblPurchasesSumary.Text = "Results" + " " + startIndex.ToString() + " - " +
						endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
				else
					this.lblPurchasesSumary.Text = "Results" + " " + startIndex.ToString() + " - " +
						ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();



				this.lblPurchasesSumary.Visible = true;

			}
			else
			{


				this.lblPurchasesSumary.Text = "No records.";
				this.lblPurchasesSumary.Visible = true;

			}
			this.dgPurchases.DataSource = dv;
			this.dgPurchases.DataBind();
			this.dgPurchases.Visible = true;

		}
		#endregion

		#region LoadCollectionsData
		private void LoadCollectionsData()
		{
			LogSession session = base.GetSessionInfo();

			DateTime dateFrom = GMSUtil.ToDate(txtReceiptFrom.Text.Trim());
			DateTime dateTo = GMSUtil.ToDate(txtReceiptTo.Text.Trim());
			string invoiceNo = "%" + txtInvoiceNo.Text.Trim() + "%";
			string receiptNo = "%" + txtReceiptNo.Text.Trim() + "%";


			GMSGeneralDALC dacl = new GMSGeneralDALC();
			DataSet ds = new DataSet();
			try
			{
				dacl.GetCollectionsSelect(session.CompanyId, loginUserOrAlternateParty, this.hidAccountCode.Value.Trim(), dateFrom, dateTo, invoiceNo, receiptNo, ref ds);
			}
			catch (Exception ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}

			int startIndex = ((dgCollections.CurrentPageIndex + 1) * this.dgCollections.PageSize) - (this.dgCollections.PageSize - 1);
			int endIndex = (dgCollections.CurrentPageIndex + 1) * this.dgCollections.PageSize;

			if (ds != null && ds.Tables[0].Rows.Count > 0)
			{
				if (endIndex < ds.Tables[0].Rows.Count)
					this.lblCollectionSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
				else
					this.lblCollectionSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

				DataView dv = ds.Tables[0].DefaultView;
				dv.Sort = ViewState["SortCollectionField"].ToString() + " " + ViewState["SortDirection"].ToString();

				this.lblCollectionSummary.Visible = true;
				this.dgCollections.DataSource = dv;
				this.dgCollections.DataBind();
				this.dgCollections.Visible = true;


			}
			else
			{
				this.lblCollectionSummary.Text = "No records.";
				this.lblCollectionSummary.Visible = true;
				this.dgCollections.DataSource = null;
				this.dgCollections.DataBind();
			}


		}
		#endregion

		#region LoadOutstandingPayementsData
		private void LoadOutstandingPayementsData(short days)
		{
			LogSession session = base.GetSessionInfo();

			GMSGeneralDALC dacl = new GMSGeneralDALC();
			DataSet ds = new DataSet();
			try
			{
				dacl.GetOutstandingPayementsSelect(session.CompanyId, days, loginUserOrAlternateParty, this.hidAccountCode.Value.Trim(), ref ds);
			}
			catch (Exception ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}

			int startIndex = ((dgOutstanding.CurrentPageIndex + 1) * this.dgOutstanding.PageSize) - (this.dgOutstanding.PageSize - 1);
			int endIndex = (dgOutstanding.CurrentPageIndex + 1) * this.dgOutstanding.PageSize;

			if (ds != null && ds.Tables[0].Rows.Count > 0)
			{
				if (endIndex < ds.Tables[0].Rows.Count)
					this.lblOutstandingPaymentsSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
				else
					this.lblOutstandingPaymentsSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

				DataView dv = ds.Tables[0].DefaultView;
				dv.Sort = ViewState["SortOutstandingField"].ToString() + " " + ViewState["SortDirection"].ToString();

				this.lblOutstandingPaymentsSummary.Visible = true;
				this.dgOutstanding.DataSource = dv;
				this.dgOutstanding.DataBind();
				this.dgOutstanding.Visible = true;
			}
			else
			{
				this.lblOutstandingPaymentsSummary.Text = "No records.";
				this.lblOutstandingPaymentsSummary.Visible = true;
				this.dgOutstanding.DataSource = null;
				this.dgOutstanding.DataBind();
			}


		}
		#endregion

		#region DisabledParticularFields
		private void DisabledParticularFields(string customerType)
		{
			LogSession session = base.GetSessionInfo();

			if (customerType == "C")
			{
				this.txtAccountCode.ReadOnly = true;
				this.txtAccountName.ReadOnly = true;
				this.ddlSalesman.Enabled = false;
				this.ddlTerritory.Enabled = false;
				this.txtCreditTerm.ReadOnly = true;
				this.txtCreditLimit.ReadOnly = true;
				this.ddlCurrency.Enabled = false;
				this.ddlIndustry.Enabled = false;
				this.txtAddress1.ReadOnly = true;
				this.txtAddress2.ReadOnly = true;
				this.txtAddress3.ReadOnly = true;
				this.txtAddress4.ReadOnly = true;
				this.txtPostalCode.ReadOnly = true;
			}
			else
			{
				UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 125);
				if (uAccess == null)                   
					this.ddlSalesman.Enabled = false;
			}

		}
		#endregion

		#region LoadAttachmentData
		private void LoadAttachmentData()
		{
			LogSession session = base.GetSessionInfo();

			GMSGeneralDALC dacl = new GMSGeneralDALC();
			DataSet ds = new DataSet();
			try
			{
				dacl.GetAccountAttachmentSelect(session.CompanyId, this.hidAccountCode.Value.Trim(), ref ds);
			}
			catch (Exception ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}

			int startIndex = ((dgAttachment.CurrentPageIndex + 1) * this.dgAttachment.PageSize) - (this.dgAttachment.PageSize - 1);
			int endIndex = (dgAttachment.CurrentPageIndex + 1) * this.dgAttachment.PageSize;

			if (ds != null && ds.Tables[0].Rows.Count > 0)
			{
				if (endIndex < ds.Tables[0].Rows.Count)
					this.lblAttachmentSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
				else
					this.lblAttachmentSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

				DataView dv = ds.Tables[0].DefaultView;
				dv.Sort = ViewState["SortAttachmentField"].ToString() + " " + ViewState["SortDirection"].ToString();

				this.lblAttachmentSummary.Visible = true;
				this.dgAttachment.DataSource = dv;
				this.dgAttachment.DataBind();
				this.dgAttachment.Visible = true;
			}
			else
			{
				this.lblAttachmentSummary.Text = "No records.";
				this.lblAttachmentSummary.Visible = true;
				this.dgAttachment.DataSource = null;
				this.dgAttachment.DataBind();
			}



		}
		#endregion

		#region LoadFinanceAttachmentData
		private void LoadFinanceAttachmentData()
		{
			LogSession session = base.GetSessionInfo();

			GMSGeneralDALC dacl = new GMSGeneralDALC();
			DataSet ds = new DataSet();
			try
			{
				string accountname = "%" + this.lblAccountName.Text.Trim() + "%";
				
				dacl.GetFinanceAttachmentSelect(accountname, ref ds);
			}
			catch (Exception ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}

			int startIndex = ((dgFinanceAttachment.CurrentPageIndex + 1) * this.dgFinanceAttachment.PageSize) - (this.dgFinanceAttachment.PageSize - 1);
			int endIndex = (dgFinanceAttachment.CurrentPageIndex + 1) * this.dgFinanceAttachment.PageSize;

			if (ds != null && ds.Tables[0].Rows.Count > 0)
			{
				if (endIndex < ds.Tables[0].Rows.Count)
					this.lblFinanceAttachmentSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
				else
					this.lblFinanceAttachmentSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

				DataView dv = ds.Tables[0].DefaultView;
				dv.Sort = ViewState["SortFinanceAttachmentField"].ToString() + " " + ViewState["SortDirection"].ToString();

				this.lblFinanceAttachmentSummary.Visible = true;
				this.dgFinanceAttachment.DataSource = dv;
				this.dgFinanceAttachment.DataBind();
				this.dgFinanceAttachment.Visible = true;
			}
			else
			{
				this.lblFinanceAttachmentSummary.Text = "No records.";
				this.lblFinanceAttachmentSummary.Visible = true;
				this.dgFinanceAttachment.DataSource = null;
				this.dgFinanceAttachment.DataBind();
			}



		}
		#endregion


		#region LoadParticularsData
		private void LoadParticularsData()
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

				if (accountCode.Substring(0, 1) == "P")
				{
					btnUpgradeToCustomer.Visible = false;
					dacl.GetProspectDetailByAccountCode(session.CompanyId, accountCode, ref ds);

				}
				else
					dacl.GetDebtorDetailByAccountCodeCRM(session.CompanyId, accountCode, ref ds);
			}
			catch (Exception ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				this.lblAccountName.Text = ds.Tables[0].Rows[0]["AccountName"].ToString();
				this.txtAccountCode.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString();
				this.txtAccountName.Text = ds.Tables[0].Rows[0]["AccountName"].ToString();

			   
				double CreditUsedOnDO = GMSUtil.ToDouble(ds.Tables[0].Rows[0]["CreditUsedOnDO"].ToString());
				if (CreditUsedOnDO > 0)
					this.lblCreditUsedOnDO.Text = String.Format("{0:0.00}", CreditUsedOnDO);
				else
					this.lblCreditUsedOnDO.Text = String.Format("{0:0.00}", "0");
				 

				double outstanding = GMSUtil.ToDouble(ds.Tables[0].Rows[0]["Outstanding"].ToString());

				if (outstanding > 0)
					this.lblOutstanding.Text = String.Format("{0:0.00}", outstanding);
				else
					this.lblOutstanding.Text = String.Format("{0:0.00}", "0");

				double excess = (GMSUtil.ToDouble(ds.Tables[0].Rows[0]["Outstanding"].ToString()) - GMSUtil.ToDouble(ds.Tables[0].Rows[0]["CreditLimit"].ToString()));


				if (excess > 0)
					this.lblExcess.Text = String.Format("{0:0.00}", excess);
				else
					this.lblExcess.Text = String.Format("{0:0.00}", "0");

				this.txtCreditTerm.Text = ds.Tables[0].Rows[0]["CreditTerm"].ToString();

				double originalCreditLimit = GMSUtil.ToDouble(ds.Tables[0].Rows[0]["OriginalCreditLimit"].ToString());


				if (originalCreditLimit > 0)
					this.txtOriginalCreditLimit.Text = String.Format("{0:0.00}", originalCreditLimit);
				else
					this.txtOriginalCreditLimit.Text = String.Format("{0:0.00}", "0");

				this.txtCreditTerm.Text = ds.Tables[0].Rows[0]["CreditTerm"].ToString();

				this.txtCreditLimit.Text = String.Format("{0:0.00}", ds.Tables[0].Rows[0]["CreditLimit"].ToString());
				this.txtAddress1.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
				this.txtAddress2.Text = ds.Tables[0].Rows[0]["Address2"].ToString();
				this.txtAddress3.Text = ds.Tables[0].Rows[0]["Address3"].ToString();
				this.txtAddress4.Text = ds.Tables[0].Rows[0]["Address4"].ToString();
				this.txtPostalCode.Text = ds.Tables[0].Rows[0]["PostalCode"].ToString();
				this.txtWebsite.Text = ds.Tables[0].Rows[0]["Website"].ToString();
				this.txtFacebook.Text = ds.Tables[0].Rows[0]["Facebook"].ToString();
				this.txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
				this.chkAcctActive.Checked = (bool)ds.Tables[0].Rows[0]["isActive"];

				SystemDataActivity sDataActivity = new SystemDataActivity();

				IList<GMSCore.Entity.AccountIndustry> lstAccIndustry = null;

				lstAccIndustry = sDataActivity.RetrieveAllIndustry();
				ddlIndustry.DataSource = lstAccIndustry;
				ddlIndustry.DataBind();

				GMSCore.Entity.AccountIndustry industry = sDataActivity.RetrieveIndustryByName(ds.Tables[0].Rows[0]["Industry"].ToString());

				if (industry != null)
					ddlIndustry.SelectedValue = industry.Name.ToString();
				else
					ddlIndustry.SelectedValue = "--";



				IList<GMSCore.Entity.AccountGrade> lstAccGrade = null;

				lstAccGrade = sDataActivity.RetrieveAllAccountGrade();
				ddlGrade.DataSource = lstAccGrade;
				ddlGrade.DataBind();
				if (ds.Tables[0].Rows[0]["GradeCode"].ToString() != "")
					ddlGrade.SelectedValue = ds.Tables[0].Rows[0]["GradeCode"].ToString();
				else
					ddlGrade.SelectedValue = "C";


				IList<GMSCore.Entity.AccountClass> lstAccClass = null;

				lstAccClass = sDataActivity.RetrieveAllAccountClass(session.CompanyId);
				ddlType.DataSource = lstAccClass;
				ddlType.DataBind();
				if (ds.Tables[0].Rows[0]["AccountClass"].ToString() != "")
					ddlType.SelectedValue = ds.Tables[0].Rows[0]["AccountClass"].ToString();
				else
					ddlType.SelectedValue = "-";

				IList<GMSCore.Entity.AccountGroup> lstAccGroup = null;

				lstAccGroup = sDataActivity.RetrieveAllAccountGroup();
				ddlGroup.DataSource = lstAccGroup;
				ddlGroup.DataBind();
				if (ds.Tables[0].Rows[0]["AccountGroup"].ToString() != "")
					ddlGroup.SelectedValue = ds.Tables[0].Rows[0]["AccountGroup"].ToString();
				else
					ddlGroup.SelectedValue = "-";

			   


				ProductsDataDALC pdacl = new ProductsDataDALC();
				DataSet dsSalesman = new DataSet();
				try
				{
					pdacl.GetSalesman(session.CompanyId, loginUserOrAlternateParty, ref dsSalesman);
				}
				catch (Exception ex)
				{
					JScriptAlertMsg(ex.Message);
				}

				ddlSalesman.DataSource = dsSalesman;
				ddlSalesman.DataValueField = "SalesPersonID";
				ddlSalesman.DataTextField = "SalesPersonName";
				ddlSalesman.DataBind();
				ddlSalesman.SelectedValue = ds.Tables[0].Rows[0]["SalesPersonID"].ToString();




				IList<Currency> cList = null;
				try
				{
					cList = sDataActivity.RetrieveAllCurrencyListSortByCode();
				}
				catch (Exception ex)
				{
					JScriptAlertMsg(ex.Message);
				}
				ddlCurrency.DataSource = cList;
				ddlCurrency.DataTextField = "CurrencyCode";
				ddlCurrency.DataValueField = "CurrencyCode";
				ddlCurrency.DataBind();
				ddlCurrency.SelectedValue = ds.Tables[0].Rows[0]["DefaultCurrency"].ToString();



				IList<AccountTerritory> countryList = (new SystemDataActivity()).RetrieveAllTerritoryList();
				ddlTerritory.DataSource = countryList;
				ddlTerritory.DataBind();
				ddlTerritory.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();



				if (accountCode.Substring(0, 1) != "P")
				{
					DisabledParticularFields("C");
				}
				else
				{
					if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CreatedBy"].ToString()))
						this.lblCreatedBy.Text = "Created By " + ds.Tables[0].Rows[0]["CreatedBy"].ToString() + " on " + ds.Tables[0].Rows[0]["CreatedDate"].ToString();
					if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ModifiedBy"].ToString()))
						this.lblModifiedBy.Text = "Modified By " + ds.Tables[0].Rows[0]["ModifiedBy"].ToString() + " on " + ds.Tables[0].Rows[0]["ModifiedDate"].ToString();
					DisabledParticularFields("P");
				}
			}
		}
		#endregion

		#region btnUpload_Click
		protected void btnUpload_Click(object sender, EventArgs e)
		{
			LogSession session = base.GetSessionInfo();

			FileUpload FileUpload1 = (FileUpload)upAttachment.FindControl("FileUpload1");
			if (FileUpload1.HasFile)
			{
				try
				{
					string folderPath = "C://GMS/CRM";
					if (!Directory.Exists(folderPath))
					{
						Directory.CreateDirectory(folderPath);
					}


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
						documentNumber.AttachmentNo = "0001";
						documentNumber.ProspectNo = "0001";
						documentNumber.ContactNo = "0001";
						documentNumber.CommNo = "0001";
						documentNumber.CommCommentNo = "0001";
						documentNumber.PurchaseNo = "0001";
                        documentNumber.MRNo = "00001";
                        documentNumber.DocumentNo = "00001";
                        documentNumber.DocumentNoForEmployee = "000001";
                        documentNumber.ProjectNo = "000001";
                        documentNumber.CEID = "0001";
                        documentNumber.CEDetailID = "00001";
                        documentNumber.ItemID = "1";
                        documentNumber.ClaimNo = "00001";
                    }

					FileUpload1.SaveAs(folderPath + "/" + (DateTime.Now.Year).ToString() + "-" + session.CompanyId + "-" + documentNumber.AttachmentNo + Path.GetExtension(this.FileUpload1.FileName));


					GMSCore.Entity.AccountAttachment accountAttachment = new GMSCore.Entity.AccountAttachment();

					accountAttachment.CoyID = session.CompanyId;
					accountAttachment.AccountCode = this.hidAccountCode.Value.Trim();
					accountAttachment.DocumentID = documentNumber.AttachmentNo;
					accountAttachment.DocumentName = txtFileName.Text.Trim();
					accountAttachment.FileName = (DateTime.Now.Year).ToString() + "-" + session.CompanyId + "-" + documentNumber.AttachmentNo + Path.GetExtension(this.FileUpload1.FileName);
					accountAttachment.IsActive = true;
					accountAttachment.CreatedBy = session.UserId;
					accountAttachment.CreatedDate = DateTime.Now;

					ResultType result = new AccountAttachmentActivity().CreateAccountAttachment(ref accountAttachment, session);

					switch (result)
					{
						case ResultType.Ok:
							LoadAttachmentData();
							txtFileName.Text = "";
							break;
						default:
							this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
							return;
					}

					string nxtStr = ((short)(short.Parse(documentNumber.AttachmentNo) + 1)).ToString();
					for (int i = nxtStr.Length; i < documentNumber.AttachmentNo.Length; i++)
					{
						nxtStr = "0" + nxtStr;
					}
					documentNumber.AttachmentNo = nxtStr;
					documentNumber.Save();


					ScriptManager.RegisterStartupScript(upAttachment, upAttachment.GetType(), "Report1", "alert('File has been uploaded!');", true);

				}
				catch (Exception ex)
				{
					JScriptAlertMsg(ex.Message);
				}
			}
		}
		#endregion

		#region btnUploadFinance_Click
		protected void btnUploadFinance_Click(object sender, EventArgs e)
		{
			LogSession session = base.GetSessionInfo();

			FileUpload FileUpload1 = (FileUpload)upAttachment.FindControl("FileUploadFinance");
			if (FileUpload1.HasFile)
			{
				try
				{
					string folderPath = "C://GMS/CRM";
					if (!Directory.Exists(folderPath))
					{
						Directory.CreateDirectory(folderPath);
					}


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
						documentNumber.AttachmentNo = "0001";
						documentNumber.ProspectNo = "0001";
						documentNumber.ContactNo = "0001";
						documentNumber.CommNo = "0001";
						documentNumber.CommCommentNo = "0001";
						documentNumber.PurchaseNo = "0001";
                        documentNumber.MRNo = "00001";
                        documentNumber.DocumentNo = "00001";
                        documentNumber.DocumentNoForEmployee = "000001";
                        documentNumber.ProjectNo = "000001";
                        documentNumber.CEID = "0001";
                        documentNumber.CEDetailID = "00001";
                        documentNumber.ItemID = "1";
                        documentNumber.ClaimNo = "00001";

                    }

					FileUpload1.SaveAs(folderPath + "/" + (DateTime.Now.Year).ToString() + "-" + session.CompanyId + "-" + documentNumber.AttachmentNo + Path.GetExtension(this.FileUpload1.FileName));


					GMSCore.Entity.FinanceAttachment financeAttachment = new GMSCore.Entity.FinanceAttachment();

					financeAttachment.CoyID = session.CompanyId;
					financeAttachment.AccountName = this.txtAccountName.Text.Trim();                   
					financeAttachment.DocumentName = (DateTime.Now.Year).ToString() + "-" + session.CompanyId + "-" + documentNumber.AttachmentNo + Path.GetExtension(this.FileUpload1.FileName);
					financeAttachment.Type = ddlAttachmentType.SelectedValue.ToString();

					if (ddlAttachmentType.SelectedValue == "Finance")
					{
						financeAttachment.PeriodYearFrom = GMSUtil.ToShort(ddlPeriodYearFrom.SelectedValue.ToString());
						financeAttachment.PeriodMonthFrom = GMSUtil.ToShort(ddlPeriodMonthFrom.SelectedValue.ToString());
						financeAttachment.PeriodYearTo = GMSUtil.ToShort(ddlPeriodYearTo.SelectedValue.ToString());
						financeAttachment.PeriodMonthTo = GMSUtil.ToShort(ddlPeriodMonthTo.SelectedValue.ToString());
					}

					financeAttachment.Territory = ddlTerritory.SelectedValue.ToString();                   
					financeAttachment.IsActive = true;
					financeAttachment.CreatedBy = session.UserId;
					financeAttachment.CreatedDate = DateTime.Now;

					ResultType result = new AccountAttachmentActivity().CreateFinanceAttachment(ref financeAttachment, session);

					switch (result)
					{
						case ResultType.Ok:
							//LoadAttachmentData();                            
							break;
						default:
							this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
							return;
					}

					string nxtStr = ((short)(short.Parse(documentNumber.AttachmentNo) + 1)).ToString();
					for (int i = nxtStr.Length; i < documentNumber.AttachmentNo.Length; i++)
					{
						nxtStr = "0" + nxtStr;
					}
					documentNumber.AttachmentNo = nxtStr;
					documentNumber.Save();


					ScriptManager.RegisterStartupScript(upAttachment, upAttachment.GetType(), "Report1", "alert('File has been uploaded!');", true);
					LoadFinanceAttachmentData();

				}
				catch (Exception ex)
				{
					JScriptAlertMsg(ex.Message);
				}
			}
		}
		#endregion

		#region btnCollections_Click
		protected void btnCollections_Click(object sender, EventArgs e)
		{
			this.dgCollections.CurrentPageIndex = 0;
            var javascript = @"
                     $(document).ready(function() {
                        $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
                    });
                ";

            ScriptManager.RegisterStartupScript(upCollection, GetType(), "Javascript", javascript, true);
			LoadCollectionsData();
		}
		#endregion

		#region btnPreviousMonthCollections_Click
		protected void btnPreviousMonthCollections_Click(object sender, EventArgs e)
		{
			txtReceiptFrom.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1).ToString("dd/MM/yyyy");
			txtReceiptTo.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1).ToString("dd/MM/yyyy");
			this.dgCollections.CurrentPageIndex = 0;
            var javascript = @"
                     $(document).ready(function() {
                        $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
                    });
                ";

            ScriptManager.RegisterStartupScript(upCollection, GetType(), "Javascript", javascript, true);
			LoadCollectionsData();
		}
		#endregion

		#region btnCurrentMonthCollections_Click
		protected void btnCurrentMonthCollections_Click(object sender, EventArgs e)
		{
			txtReceiptFrom.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("dd/MM/yyyy");
			txtReceiptTo.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");
			this.dgCollections.CurrentPageIndex = 0;
            var javascript = @"
                     $(document).ready(function() {
                        $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
                    });
                ";

            ScriptManager.RegisterStartupScript(upCollection, GetType(), "Javascript", javascript, true);
			LoadCollectionsData();
		}
		#endregion

		#region btnThisYearCollections_Click
		protected void btnThisYearCollections_Click(object sender, EventArgs e)
		{
			txtReceiptFrom.Text = new DateTime(DateTime.Today.Year, 1, 1).ToString("dd/MM/yyyy");
			txtReceiptTo.Text = new DateTime(DateTime.Today.Year, 12, 31).ToString("dd/MM/yyyy");
			this.dgCollections.CurrentPageIndex = 0;
            var javascript = @"
                     $(document).ready(function() {
                        $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
                    });
                ";

            ScriptManager.RegisterStartupScript(upCollection, GetType(), "Javascript", javascript, true);
			LoadCollectionsData();
		}
		#endregion


		#region btnSearch_Click
		protected void btnSearch_Click(object sender, EventArgs e)
		{
			this.dgData.CurrentPageIndex = 0;
            var javascript = @"
                     $(document).ready(function() {
                        $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
                    });
                ";

            ScriptManager.RegisterStartupScript(upSales, GetType(), "Javascript", javascript, true);
			LoadData();
		}
		#endregion

		#region btnPreviousMonthSales_Click
		protected void btnPreviousMonthSales_Click(object sender, EventArgs e)
		{

			trnDateFrom.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1).ToString("dd/MM/yyyy");
			trnDateTo.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1).ToString("dd/MM/yyyy");
			this.dgData.CurrentPageIndex = 0;
            var javascript = @"
                     $(document).ready(function() {
                        $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
                    });
                ";

            ScriptManager.RegisterStartupScript(upSales, GetType(), "Javascript", javascript, true);
			LoadData();
		}
		#endregion

		#region btnCurrentMonthSales_Click
		protected void btnCurrentMonthSales_Click(object sender, EventArgs e)
		{
			trnDateFrom.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("dd/MM/yyyy");
			trnDateTo.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");
			this.dgData.CurrentPageIndex = 0;
            var javascript = @"
                     $(document).ready(function() {
                        $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
                    });
                ";

            ScriptManager.RegisterStartupScript(upSales, GetType(), "Javascript", javascript, true);
			LoadData();
		}
		#endregion

		#region btnThisYearSales_Click
		protected void btnThisYearSales_Click(object sender, EventArgs e)
		{
			trnDateFrom.Text = new DateTime(DateTime.Today.Year, 1, 1).ToString("dd/MM/yyyy");
			trnDateTo.Text = new DateTime(DateTime.Today.Year, 12, 31).ToString("dd/MM/yyyy");
			this.dgData.CurrentPageIndex = 0;
            var javascript = @"
                     $(document).ready(function() {
                        $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
                    });
                ";

            ScriptManager.RegisterStartupScript(upSales, GetType(), "Javascript", javascript, true);
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

		#region dgAttachment dgAttachment_PageIndexChanged event handling
		protected void dgAttachment_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			DataGrid dtg = (DataGrid)source;
			dtg.CurrentPageIndex = e.NewPageIndex;
			LoadAttachmentData();


		}
		#endregion

		#region dgOutstanding dgOutstanding_PageIndexChanged event handling
		protected void dgOutstanding_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			DataGrid dtg = (DataGrid)source;
			dtg.CurrentPageIndex = e.NewPageIndex;

			if (ViewState["OutstandingRange"].ToString() == "0")
			{
				LoadOutstandingPayementsData(0);
			}
			else if (ViewState["OutstandingRange"].ToString() == "90")
			{
				LoadOutstandingPayementsData(90);
			}
			else if (ViewState["OutstandingRange"].ToString() == "180")
			{
				LoadOutstandingPayementsData(180);
			}

		}
		#endregion

		#region dgCollections dgCollections_PageIndexChanged event handling
		protected void dgCollections_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			DataGrid dtg = (DataGrid)source;
			dtg.CurrentPageIndex = e.NewPageIndex;
			LoadCollectionsData();

		}
		#endregion

		#region LoadData
		private void LoadData()
		{
			LogSession session = base.GetSessionInfo();


			string invoiceNo = "%" + txtInvoiceNumber.Text.Trim() + "%";
			string productCode = "%" + txtProductCode.Text.Trim() + "%";
			string productName = "%" + txtProductName.Text.Trim() + "%";
			string productGroupCode = "%" + txtProductGroup.Text.Trim() + "%";
			string productGroupName = "%" + txtProductGroupName.Text.Trim() + "%";

			DateTime dateFrom = GMSUtil.ToDate(trnDateFrom.Text.Trim());
			DateTime dateTo = GMSUtil.ToDate(trnDateTo.Text.Trim());
			string salesmanID = "'0'".ToString();

			GMSGeneralDALC dacl = new GMSGeneralDALC();

			DataSet dsSalesPerson = new DataSet();

			try
			{
				dacl.GetSalesPersonListSelect(session.CompanyId, loginUserOrAlternateParty, ref dsSalesPerson);
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


			try
			{
				GMSWebService.GMSWebService sc = new GMSWebService.GMSWebService();
				if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
				{
					sc.Url = session.WebServiceAddress.Trim();
				}
				else
					sc.Url = "http://localhost/GMSWebService/GMSWebService.asmx";

				ds = sc.GetInvoicelist(session.CompanyId, this.hidAccountCode.Value.Trim(), "%%", dateFrom, dateTo, salesmanID, productCode, productName, productGroupCode, productGroupName, invoiceNo, "%%");

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


			}
			catch (Exception ex)
			{
				this.PageMsgPanel.ShowMessage("The connection to the server has failed! <br />For more information, please contact your System Administrator. ", MessagePanelControl.MessageEnumType.Alert);

			}




		}
		#endregion


		#region SortContact
		protected void SortContact(object source, DataGridSortCommandEventArgs e)
		{
			if (e.SortExpression.ToString() == ViewState["SortContactsField"].ToString())
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
				ViewState["SortContactsField"] = e.SortExpression;
				ViewState["SortDirection"] = "ASC";
			}


			LoadContactsData();

		}
		#endregion

		#region SortPurchase
		protected void SortPurchase(object source, DataGridSortCommandEventArgs e)
		{
			if (e.SortExpression.ToString() == ViewState["SortPurchasesField"].ToString())
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
				ViewState["SortPurchasesField"] = e.SortExpression;
				ViewState["SortDirection"] = "ASC";
			}

			LoadPurchasesData();

		}
		#endregion

		#region SortCommunication
		protected void SortCommunication(object source, DataGridSortCommandEventArgs e)
		{
			if (e.SortExpression.ToString() == ViewState["SortCommRecordField"].ToString())
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
				ViewState["SortCommRecordField"] = e.SortExpression;
				ViewState["SortDirection"] = "ASC";
			}

			LoadCommunicationData();

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

		protected void SortFinanceAttachment(object source, DataGridSortCommandEventArgs e)
		{

			if (e.SortExpression.ToString() == ViewState["SortAttachmentField"].ToString())
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
				ViewState["SortAttachmentField"] = e.SortExpression;
				ViewState["SortDirection"] = "ASC";
			}
			LoadAttachmentData();

		}

		protected void SortAttachment(object source, DataGridSortCommandEventArgs e)
		{

			if (e.SortExpression.ToString() == ViewState["SortAttachmentField"].ToString())
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
				ViewState["SortAttachmentField"] = e.SortExpression;
				ViewState["SortDirection"] = "ASC";
			}
			LoadAttachmentData();

		}

		protected void SortOutstanding(object source, DataGridSortCommandEventArgs e)
		{

			if (e.SortExpression.ToString() == ViewState["SortOutstandingField"].ToString())
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
				ViewState["SortOutstandingField"] = e.SortExpression;
				ViewState["SortDirection"] = "ASC";
			}

			LoadOutstandingPayementsData(0);

		}

		protected void SortCollections(object source, DataGridSortCommandEventArgs e)
		{

			if (e.SortExpression.ToString() == ViewState["SortCollectionField"].ToString())
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
				ViewState["SortCollectionField"] = e.SortExpression;
				ViewState["SortDirection"] = "ASC";
			}

			LoadCollectionsData();

		}

		#region dgAttachment_DeleteCommand
		protected void dgAttachment_DeleteCommand(object sender, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				LogSession session = base.GetSessionInfo();
				if (session == null)
				{
					Response.Redirect(base.SessionTimeOutPage("Sales"));
					return;
				}

				HtmlInputHidden hidDocumentID = (HtmlInputHidden)e.Item.FindControl("hidDocumentID");

				if (hidDocumentID != null)
				{

					new GMSGeneralDALC().UpdateAccountAttachment(hidDocumentID.Value);
					this.dgAttachment.EditItemIndex = -1;
					lblAttachmentMsg.Text = "Record deleted successfully!";
					LoadAttachmentData();

					/*
					GMSCore.Entity.AccountAttachment accountAttachment = new AccountAttachmentActivity().RetrieveAccountAttachmentByDocumentID(session.CompanyId, GMSUtil.ToShort(hidDocumentID.Value));

					accountAttachment.IsActive = false;
					accountAttachment.ModifiedBy = session.UserId;
					accountAttachment.ModifiedDate = DateTime.Now;


					try
					{
						ResultType result = new AccountAttachmentActivity().UpdateBankAttachment(ref accountAttachment, session);

						switch (result)
						{
							case ResultType.Ok:
								this.dgAttachment.EditItemIndex = -1;
								lblAttachmentMsg.Text = "Record deleted successfully!";
								LoadAttachmentData();
								break;
							default:
								this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
								return;
						}
					}
					catch (Exception ex)
					{
						this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
						return;
					}
					*/

				}
			}
		}
		#endregion



		protected void dgAttachment_Command(Object sender, DataGridCommandEventArgs e)
		{
			string ext = Path.GetExtension(e.CommandArgument.ToString());
			string ContentType = "";

			switch (((LinkButton)e.CommandSource).CommandName)
			{

				case "Load":
					if (ext == ".asf")
						ContentType = "video/x-ms-asf";
					else if (ext == ".avi")
						ContentType = "video/avi";
					else if (ext == ".doc")
						ContentType = "application/msword";
					else if (ext == ".zip")
						ContentType = "application/zip";
					else if (ext == ".xls")
						ContentType = "application/vnd.ms-excel";
					else if (ext == ".gif")
						ContentType = "image/gif";
					else if (ext == ".jpg" || ext == "jpeg")
						ContentType = "image/jpeg";
					else if (ext == ".wav")
						ContentType = "audio/wav";
					else if (ext == ".mp3")
						ContentType = "audio/mpeg3";
					else if (ext == ".mpg" || ext == "mpeg")
						ContentType = "video/mpeg";
					else if (ext == ".mp3")
						ContentType = "audio/mpeg3";
					else if (ext == ".rtf")
						ContentType = "application/rtf";
					else if (ext == ".htm" || ext == "html")
						ContentType = "text/html";
					else if (ext == ".asp")
						ContentType = "text/asp";
					else
						ContentType = "application/octet-stream";

					Response.ContentType = ContentType.ToString();
					Response.AppendHeader("Content-Disposition", "attachment; filename=" + e.CommandArgument.ToString());
					Response.TransmitFile("C://GMS/CRM/" + e.CommandArgument.ToString());
					Response.End();
					break;

				// Add other cases here, if there are multiple ButtonColumns in 
				// the DataGrid control.

				default:
					// Do nothing.
					break;

			}

		}

		#region dgAttachment_ItemDataBound
		protected void dgAttachment_ItemDataBound(object sender, DataGridItemEventArgs e)
		{



			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{

				LinkButton ibtnDownload = (LinkButton)e.Item.FindControl("linkName");

				if (ibtnDownload != null)
				{

					//ScriptManager sm = (ScriptManager)this.Page.Master.FindControl("ScriptManager1");
					ScriptManager sm = ScriptManager.GetCurrent(this.Page);
					if (sm != null)
					{
						sm.RegisterPostBackControl(ibtnDownload);

					}
				}

				LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

				if (lnkDelete != null)
				{


					lnkDelete.Attributes.Add("onclick", "return confirm_delete();");


				}
			}
		}
		#endregion

		#region btnSearchAllOutstanding_Click
		protected void btnSearchAllOutstanding_Click(object sender, EventArgs e)
		{
			this.dgOutstanding.CurrentPageIndex = 0;
			btnAllOutstanding.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FAA634");
			LoadOutstandingPayementsData(0);
		}
		#endregion

		#region btnSearchGreater90DaysOutstanding_Click
		protected void btnSearchGreater90DaysOutstanding_Click(object sender, EventArgs e)
		{
			this.dgOutstanding.CurrentPageIndex = 0;
			Greater90DaysOutstanding.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FAA634");
			LoadOutstandingPayementsData(90);
			ViewState["OutstandingRange"] = "90";
		}
		#endregion

		#region btnSearchGreater180DaysOutstanding_Click
		protected void btnSearchGreater180DaysOutstanding_Click(object sender, EventArgs e)
		{
			this.dgOutstanding.CurrentPageIndex = 0;
			Greater180DaysOutstanding.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FAA634");
			LoadOutstandingPayementsData(180);
			ViewState["OutstandingRange"] = "180";
		}
		#endregion




		#region dgPurchases_ItemDataBound
		protected void dgPurchases_ItemDataBound(object sender, DataGridItemEventArgs e)
		{

			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
				if (lnkDelete != null)
				{
					lnkDelete.Attributes.Add("onclick", "return confirm_delete();");
				}
			}
		}
		#endregion

		#region dgPurchases_DeleteCommand
		protected void dgPurchases_DeleteCommand(object sender, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				LogSession session = base.GetSessionInfo();
				if (session == null)
				{
					Response.Redirect(base.SessionTimeOutPage("Sales"));
					return;
				}

				HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidID");

				if (hidID != null)
				{
					AccountPurchasesActivity apActivity = new AccountPurchasesActivity();


					try
					{
						ResultType result = apActivity.DeleteAccountPurchases(hidID.Value, session);

						switch (result)
						{
							case ResultType.Ok:
								this.dgPurchases.EditItemIndex = -1;
								this.dgPurchases.CurrentPageIndex = 0;
								LoadPurchasesData();
								lblPurchaseMsg.Text = "Record deleted successfully!<br /><br />";
								break;
							default:
								this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
								return;
						}
					}
					catch (SqlException exSql)
					{
						if (exSql.Number == 547)
						{
							this.PageMsgPanel.ShowMessage("This record cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
							LoadPurchasesData();
							return;
						}
					}
					catch (Exception ex)
					{
						this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
						LoadPurchasesData();
						return;
					}
				}
			}
		}
		#endregion

		#region dgPurchases datagrid PageIndexChanged event handling
		protected void dgPurchases_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			DataGrid dtg = (DataGrid)source;
			dtg.CurrentPageIndex = e.NewPageIndex;
			LoadPurchasesData();

		}
		#endregion

		#region btnUpdateParticular_Click
		protected void btnUpdateParticular_Click(object sender, EventArgs e)
		{

			LogSession session = base.GetSessionInfo();

			if (this.hidAccountCode.Value.Trim().Substring(0, 1) == "P")
			{
				GMSCore.Entity.AccountProspect accountProspect = new AccountProspectActivity().RetrieveAccountProspectByAccountCode(session.CompanyId, this.hidAccountCode.Value.Trim());
				if (accountProspect != null)
				{
					accountProspect.AccountName = txtAccountName.Text.Trim();
					accountProspect.SalesPersonID = this.ddlSalesman.SelectedValue;
					accountProspect.DefaultCurrency = this.ddlCurrency.SelectedValue;
					accountProspect.Country = this.ddlTerritory.SelectedValue;
					accountProspect.Industry = this.ddlIndustry.SelectedValue;

					if (!string.IsNullOrEmpty(txtCreditTerm.Text.Trim()))
						accountProspect.CreditTerm = GMSUtil.ToShort(txtCreditTerm.Text.Trim());
					else
						accountProspect.CreditTerm = 0;

					if (!string.IsNullOrEmpty(txtCreditLimit.Text.Trim()))
						accountProspect.CreditLimit = GMSUtil.ToDouble(txtCreditLimit.Text.Trim());
					else
						accountProspect.CreditLimit = 0;

					


					accountProspect.Address1 = this.txtAddress1.Text.Trim();
					accountProspect.Address2 = this.txtAddress2.Text.Trim();
					accountProspect.Address3 = this.txtAddress3.Text.Trim();
					accountProspect.Address4 = this.txtAddress4.Text.Trim();
					accountProspect.PostalCode = this.txtPostalCode.Text.Trim();


					accountProspect.ModifiedBy = session.UserId;
					accountProspect.ModifiedDate = DateTime.Now;

					try
					{
						ResultType result = new AccountProspectActivity().UpdateAccountProspect(ref accountProspect, session);

						switch (result)
						{
							case ResultType.Ok:
								break;
							default:
								this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
								return;
						}
					}
					catch (Exception ex)
					{
						this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
						return;
					}
				}

			}
            
			GMSCore.Entity.AccountInformation accountInfo = new AccountInformationActivity().RetrieveAccountInformationByAccountCode(session.CompanyId, this.hidAccountCode.Value.Trim());

			if (accountInfo == null)
			{
				GMSCore.Entity.AccountInformation newaccountInfo = new AccountInformation();
				newaccountInfo.CoyID = session.CompanyId;
				newaccountInfo.AccountCode = this.hidAccountCode.Value.Trim();
				newaccountInfo.GradeCode = this.ddlGrade.SelectedValue;
				newaccountInfo.AccountGroup = this.ddlGroup.SelectedValue;
				newaccountInfo.Website = txtWebsite.Text.Trim();
				newaccountInfo.Facebook = txtFacebook.Text.Trim();
				newaccountInfo.AccountClass = this.ddlType.SelectedValue;
				newaccountInfo.Remarks = this.txtRemarks.Text.Trim();
				newaccountInfo.IsActive = chkAcctActive.Checked;

				if (!string.IsNullOrEmpty(txtOriginalCreditLimit.Text.Trim()))
					newaccountInfo.OriginalCreditLimit = GMSUtil.ToShort(txtOriginalCreditLimit.Text.Trim());
				else
					newaccountInfo.OriginalCreditLimit = 0;

				
				newaccountInfo.CreatedBy = session.UserId;
				newaccountInfo.CreatedDate = DateTime.Now;

				try
				{
					ResultType result = new AccountInformationActivity().CreateAccountInformation(ref newaccountInfo, session);

					switch (result)
					{
						case ResultType.Ok:

							LoadParticularsData();
							lblParticularsMsg.Text = "Record modified successfully!";
							//ScriptManager.RegisterStartupScript(this, typeof(Page), "", "alert('Record modified successfully!')", true);
							break;
						default:
							this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
							return;
					}
				}
				catch (Exception ex)
				{
					this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
					return;
				}

			}
			else
			{
				accountInfo.GradeCode = this.ddlGrade.SelectedValue;
				accountInfo.AccountClass = this.ddlType.SelectedValue;
				accountInfo.AccountGroup = this.ddlGroup.SelectedValue;
				accountInfo.Website = txtWebsite.Text.Trim();
				accountInfo.Facebook = txtFacebook.Text.Trim();
				accountInfo.Remarks = txtRemarks.Text.Trim();
				accountInfo.IsActive = chkAcctActive.Checked;


				if (!string.IsNullOrEmpty(txtOriginalCreditLimit.Text.Trim()))
					accountInfo.OriginalCreditLimit = GMSUtil.ToShort(txtOriginalCreditLimit.Text.Trim());
				else
					accountInfo.OriginalCreditLimit = 0;

				accountInfo.ModifiedBy = session.UserId;
				accountInfo.ModifiedDate = DateTime.Now;

				try
				{
					ResultType result = new AccountInformationActivity().UpdateAccountInformation(ref accountInfo, session);

					switch (result)
					{
						case ResultType.Ok:

							LoadParticularsData();
							lblParticularsMsg.Text = "Record modified successfully!";
							//ScriptManager.RegisterStartupScript(this, typeof(Page), "", "alert('Record modified successfully!')", true);
							break;
						default:
							this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
							return;
					}
				}
				catch (Exception ex)
				{
					this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
					return;
				}
			}

		}
		#endregion

		#region btnUpgradeToCustomer_Click
		protected void btnUpgradeToCustomer_Click(object sender, EventArgs e)
		{

			LogSession session = base.GetSessionInfo();
			A21Account account = A21Account.RetrieveByKey(session.CompanyId, this.txtAccountCode.Text.Trim());

			if (account != null)
			{
				new GMSGeneralDALC().UpdateProspectAccountCode(session.CompanyId, this.hidAccountCode.Value.Trim(), this.txtAccountCode.Text.Trim());
				ScriptManager.RegisterStartupScript(this, typeof(Page), "", "alert('Prospect upgraded to customer successfully!')", true);
				Response.Redirect("../../Debtors/Debtors/CRM.aspx?AccountCode=" + this.txtAccountCode.Text.Trim());
			}


		}
		#endregion



		#region dgContacts_DeleteCommand
		protected void dgContacts_DeleteCommand(object sender, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				LogSession session = base.GetSessionInfo();
				if (session == null)
				{
					Response.Redirect(base.SessionTimeOutPage("Sales"));
					return;
				}

				HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidContactID");

				if (hidID != null)
				{
					AccountContactActivity apActivity = new AccountContactActivity();


					try
					{
						ResultType result = apActivity.DeleteAccountContact(hidID.Value, session);

						switch (result)
						{
							case ResultType.Ok:
								this.dgContacts.EditItemIndex = -1;
								this.dgContacts.CurrentPageIndex = 0;
								lblContactsMsg.Text = "Record deleted successfully!<br /><br />";
								LoadContactsData();
								break;
							default:
								this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
								return;
						}
					}
					catch (SqlException exSql)
					{
						if (exSql.Number == 547)
						{
							this.PageMsgPanel.ShowMessage("This record cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
							LoadPurchasesData();
							return;
						}
					}
					catch (Exception ex)
					{
						this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);

						return;
					}
				}
			}
		}
		#endregion



		#region dgContacts_ItemDataBound
		protected void dgContacts_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
				if (lnkDelete != null)
				{
					lnkDelete.Attributes.Add("onclick", "return confirm_delete();");
				}
			}
		}
		#endregion

		#region dgContacts_datagrid PageIndexChanged event handling
		protected void dgContacts_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			DataGrid dtg = (DataGrid)source;
			dtg.CurrentPageIndex = e.NewPageIndex;
			LoadContactsData();
		}
		#endregion








		#region AddUpdateContact
		protected void AddUpdateContact(object sender, CommandEventArgs e)
		{
			LogSession session = base.GetSessionInfo();

			if (string.IsNullOrEmpty(hidContactID1.Value.Trim()))
			{
				#region Add New Record.
				try
				{
					GMSCore.Entity.AccountContacts accountContacts = new GMSCore.Entity.AccountContacts();

					if (txtFirstName.Text.Trim() == "")
					{
						base.JScriptAlertMsg("First Name cannot be empty.");
						return;
					}

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
						documentNumber.AttachmentNo = "0001";
						documentNumber.ProspectNo = "0001";
						documentNumber.ContactNo = "0001";
						documentNumber.CommNo = "0001";
						documentNumber.CommCommentNo = "0001";
						documentNumber.PurchaseNo = "0001";
                        documentNumber.MRNo = "00001";
                        documentNumber.DocumentNo = "00001";
                        documentNumber.DocumentNoForEmployee = "000001";
                        documentNumber.ProjectNo = "000001";
                        documentNumber.CEID = "0001";
                        documentNumber.CEDetailID = "00001";
                        documentNumber.ItemID = "1";
                        documentNumber.ClaimNo = "00001";

                    }

					accountContacts.CoyID = session.CompanyId;
					accountContacts.ContactID = documentNumber.ContactNo;
					accountContacts.AccountCode = this.hidAccountCode.Value.Trim();
					accountContacts.FirstName = txtFirstName.Text.Trim();
					accountContacts.LastName = txtLastName.Text.Trim();
					accountContacts.Salutation = ddlSalutation.SelectedValue;
					accountContacts.Designation = txtDesignation.Text.Trim();
					accountContacts.OfficePhone = txtOfficePhone.Text.Trim();
					accountContacts.MobilePhone = txtMobilePhone.Text.Trim();
					accountContacts.Fax = txtFax.Text;
					accountContacts.Email = txtEmail.Text;
					accountContacts.Remarks = txtRemarks.Text.Trim();
					accountContacts.IsActive = chkIsActive.Checked;
					accountContacts.CreatedBy = session.UserId;
					accountContacts.CreatedDate = DateTime.Now;

					accountContacts.Save();
					accountContacts.Resync();

					string nxtStr = ((short)(short.Parse(documentNumber.ContactNo) + 1)).ToString();
					for (int i = nxtStr.Length; i < documentNumber.ContactNo.Length; i++)
					{
						nxtStr = "0" + nxtStr;
					}
					documentNumber.ContactNo = nxtStr;
					documentNumber.Save();


					lblContactsMsg.Text = "Record created successfully!<br /><br />";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "", "alert('Record created successfully!')", true);
                    
					LoadContactsData();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "click", "$('#contact-modal').modal('hide'); $('.modal-backdrop').fadeOut();$('body').removeClass('modal-open');", true);


				}
				catch (Exception ex)
				{
					this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
					return;
				}
				#endregion
			}
			else
			{
				#region Update Record.
				try
				{

					GMSCore.Entity.AccountContacts accountContacts = new AccountContactActivity().RetrieveAccountContactByID(session.CompanyId, hidContactID1.Value.Trim());

					if (accountContacts == null)
					{
						base.JScriptAlertMsg("This contact cannot be found in database.");
						return;
					}
					if (txtFirstName.Text.Trim() == "")
					{
						base.JScriptAlertMsg("First Name cannot be empty.");
						return;
					}

					accountContacts.FirstName = txtFirstName.Text.Trim();
					accountContacts.LastName = txtLastName.Text.Trim();
					accountContacts.Salutation = ddlSalutation.SelectedValue;
					accountContacts.Designation = txtDesignation.Text.Trim();
					accountContacts.OfficePhone = txtOfficePhone.Text.Trim();
					accountContacts.MobilePhone = txtMobilePhone.Text.Trim();
					accountContacts.Fax = txtFax.Text;
					accountContacts.Email = txtEmail.Text;
					accountContacts.Remarks = txtContactRemarks.Text.Trim();
					accountContacts.IsActive = chkIsActive.Checked;
					accountContacts.ModifiedBy = session.UserId;
					accountContacts.ModifiedDate = DateTime.Now;

					accountContacts.Save();
					accountContacts.Resync();
					lblContactsMsg.Text = "Record modified successfully!<br /><br />";
					LoadContactsData();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "click", "$('#contact-modal').modal('hide');$('.modal-backdrop').fadeOut();$('body').removeClass('modal-open');", true);

				}
				catch (Exception ex)
				{
					this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
					return;
				}
				#endregion
			}

		}
		#endregion

		#region dgContacts_EditCommand
		protected void dgContacts_EditCommand(object sender, DataGridCommandEventArgs e)
		{

			HtmlInputHidden hidContactID = (HtmlInputHidden)e.Item.FindControl("hidContactID");
			LoadSingleContactData(hidContactID.Value.Trim());
			hidContactID1.Value = hidContactID.Value.Trim();

			//ModalPopupExtender1.Show();


		}
		#endregion

		protected void dgContacts_Command(Object sender, DataGridCommandEventArgs e)
		{


			switch (((LinkButton)e.CommandSource).CommandName)
			{

				case "EditContact":
					HtmlInputHidden hidContactID = (HtmlInputHidden)e.Item.FindControl("hidContactID");
					LoadSingleContactData(hidContactID.Value.Trim());
					hidContactID1.Value = hidContactID.Value.Trim();
					lblContact.Text = "Edit Contact";
                    ScriptManager.RegisterClientScriptBlock(upContact, this.GetType(), "click", "$('#contact-modal').modal('show')", true); 
				//	ModalPopupExtender1.Show();
					break;

				default:
					// Do nothing.
					break;

			}
		}

		protected void dgPurchases_Command(Object sender, DataGridCommandEventArgs e)
		{


			switch (((LinkButton)e.CommandSource).CommandName)
			{

				case "EditPurchase":
					HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidID");
					LoadSinglePurchaseData(hidID.Value.Trim());
					hidPurchaseID.Value = hidID.Value.Trim();
					lblPurchase.Text = "Edit Purchase";
					//ModalPopupExtender2.Show();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "$('#purchase-modal').modal('show');$('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });", true); 
					break;;

				default:
					// Do nothing.
					break;

			}
		}

		#region LoadSingleCommunicationData
		private void LoadSingleCommunicationData(string hidCommID)
		{
			LogSession session = base.GetSessionInfo();
			if (hidCommID != "")
			{

				GMSCore.Entity.AccountCommRecord accountCommRecord = new AccountCommActivity().RetrieveAccountCommRecordByID(session.CompanyId, hidCommID);

				if (accountCommRecord != null)
				{
					hidCommID1.Value = hidCommID.ToString();
					txtDescription.Text = accountCommRecord.Description;
					txtDateFrom.Text = accountCommRecord.FromDateTime.ToString("dd/MM/yyyy");
					txtDateFromTime.Text = accountCommRecord.FromDateTime.ToString("HH:mm");
					txtDateTo.Text = accountCommRecord.ToDateTime.ToString("dd/MM/yyyy");
					txtDateToTime.Text = accountCommRecord.ToDateTime.ToString("HH:mm");


					ddlCommType.SelectedValue = accountCommRecord.Type;
					ddlCommStatus.Text = accountCommRecord.Status;

				}


			}
			else
			{

				txtDescription.Text = "";
				txtDateFrom.Text = "";
				txtDateFromTime.Text = "";
				txtDateTo.Text = "";
				txtDateToTime.Text = "";
				ddlCommType.SelectedIndex = -1;
				ddlCommStatus.SelectedIndex = -1;
				dgCommRecordComment.Visible = false;
				lblCommCommentRecordSummary.Visible = false;
			}

		}
		#endregion

		#region LoadSingleContactData
		private void LoadSingleContactData(string hidContactID)
		{

			if (hidContactID != "")
			{
				LogSession session = base.GetSessionInfo();

				GMSCore.Entity.AccountContacts accountContacts = new AccountContactActivity().RetrieveAccountContactByID(session.CompanyId, hidContactID);

				if (accountContacts != null)
				{
					hidContactID1.Value = hidContactID.ToString();
					txtFirstName.Text = accountContacts.FirstName;
					txtLastName.Text = accountContacts.LastName;
					ddlSalutation.SelectedValue = accountContacts.Salutation;
					txtDesignation.Text = accountContacts.Designation;
					txtOfficePhone.Text = accountContacts.OfficePhone;
					txtMobilePhone.Text = accountContacts.MobilePhone;
					txtFax.Text = accountContacts.Fax;
					txtEmail.Text = accountContacts.Email;
					if (accountContacts.Remarks != null)
						txtContactRemarks.Text = accountContacts.Remarks;
					chkIsActive.Checked = (bool)accountContacts.IsActive;

				}


			}
			else
			{

				txtFirstName.Text = "";
				txtLastName.Text = "";
				ddlSalutation.SelectedIndex = -1;
				txtDesignation.Text = "";
				txtOfficePhone.Text = "";
				txtMobilePhone.Text = "";
				txtFax.Text = "";
				txtEmail.Text = "";

				txtContactRemarks.Text = "";
				chkIsActive.Checked = true;
			}

		}
		#endregion


		#region btnAddNewContact_Click
		protected void btnAddNewContact_Click(object sender, EventArgs e)
		{
			//Response.Redirect("AddEditContact.aspx?AccountCode=" + this.hidAccountCode.Value.Trim());
			hidContactID1.Value = "";
			LoadSingleContactData("");
			lblContact.Text = "Add New Contact";
            ScriptManager.RegisterClientScriptBlock(upContact, this.GetType(), "click", "$('#contact-modal').modal('show')", true); 
		//	ModalPopupExtender1.Show();

		}
		#endregion

		#region AddCommunication_Click
		protected void AddCommunication_Click(object sender, EventArgs e)
		{
			hidCommID1.Value = "";
			LoadSingleCommunicationData("");
			lblCommunication.Text = "Add New Communication";
			lblMessage.Text = "";


            //init javascript when panel loaded
		    const string javascript = @"
                      $(document).ready(function() {
                          $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
                        });
                        
                        $('#communication-modal').modal('show')
                ";

            ScriptManager.RegisterClientScriptBlock(upCommunication, this.GetType(), "click", javascript, true); 
			//ModalPopupExtender3.Show();

		}
		#endregion

		#region AddCancelContact
		protected void AddCancelContact(object sender, CommandEventArgs e)
		{
			LoadContactsData();
		}
		#endregion

		#region AddCancelPurchase
		protected void AddCancelPurchase(object sender, CommandEventArgs e)
		{
			LoadPurchasesData();

		}
		#endregion

		#region btnAddPurchase_Click
		protected void btnAddPurchase_Click(object sender, EventArgs e)
		{
			hidPurchaseID.Value = "";
			LoadSinglePurchaseData("");
			lblPurchase.Text = "Add New Purchase";
//			ModalPopupExtender2.Show();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "$('#purchase-modal').modal('show'); $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });", true); 
		}
		#endregion

		#region btnAddNewPurchase_Click
		protected void btnAddNewPurchase_Click(object sender, EventArgs e)
		{

		}
		#endregion



		#region AddUpdatePurchase
		protected void AddUpdatePurchase(object sender, CommandEventArgs e)
		{
			LogSession session = base.GetSessionInfo();

			if (string.IsNullOrEmpty(hidPurchaseID.Value.Trim()))
			{
				#region Add New Record.
				try
				{
					GMSCore.Entity.AccountPurchases accountPurchase = new GMSCore.Entity.AccountPurchases();

					if (txtSupplier.Text.Trim() == "")
					{
						base.JScriptAlertMsg("Supplier Name cannot be empty.");
						return;
					}

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
						documentNumber.AttachmentNo = "0001";
						documentNumber.ProspectNo = "0001";
						documentNumber.ContactNo = "0001";
						documentNumber.CommNo = "0001";
						documentNumber.CommCommentNo = "0001";
						documentNumber.PurchaseNo = "0001";
                        documentNumber.MRNo = "00001";
                        documentNumber.DocumentNo = "00001";
                        documentNumber.DocumentNoForEmployee = "000001";
                        documentNumber.ProjectNo = "000001";
                        documentNumber.CEID = "0001";
                        documentNumber.CEDetailID = "00001";
                        documentNumber.ItemID = "1";
                        documentNumber.ClaimNo = "00001";

                    }

					accountPurchase.CoyID = session.CompanyId;
					accountPurchase.AccountCode = this.hidAccountCode.Value.Trim();
					accountPurchase.Supplier = txtSupplier.Text.Trim();
					accountPurchase.PurchaseID = documentNumber.PurchaseNo;
					accountPurchase.IndustryID = ddlPurchaseIndustry.SelectedValue;
					accountPurchase.ProductGroup = txtPG.Text.Trim();
					accountPurchase.ProductName = txtPN.Text.Trim();
					accountPurchase.UOM = ddlUOM.SelectedValue;
					accountPurchase.Qty = GMSUtil.ToShort(txtQuantity.Text);

					if (contractEndDate.Text.Trim() == "")
						accountPurchase.ContractEndDate = GMSCoreBase.DEFAULT_NO_DATE;

					else if (GMSUtil.ToDate(contractEndDate.Text.Trim()) != GMSCoreBase.DEFAULT_NO_DATE)
						accountPurchase.ContractEndDate = GMSUtil.ToDate(contractEndDate.Text.Trim());

					accountPurchase.Remarks = txtPurchaseRemarks.Text.Trim();
					accountPurchase.CreatedBy = session.UserId;
					accountPurchase.CreatedDate = DateTime.Now;

					accountPurchase.Save();
					accountPurchase.Resync();
					hidPurchaseID.Value = documentNumber.PurchaseNo.ToString();

					string nxtStr = ((short)(short.Parse(documentNumber.PurchaseNo) + 1)).ToString();
					for (int i = nxtStr.Length; i < documentNumber.PurchaseNo.Length; i++)
					{
						nxtStr = "0" + nxtStr;
					}
					documentNumber.PurchaseNo = nxtStr;
					documentNumber.Save();


					//ScriptManager.RegisterStartupScript(this, typeof(Page), "", "alert('Record created successfully!')", true);
					lblPurchaseMsg.Text = "Record created successfully!<br /><br />";
					LoadPurchasesData();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "$('#purchase-modal').modal('hide');$('.modal-backdrop').fadeOut();$('body').removeClass('modal-open');", true); 

				}
				catch (Exception ex)
				{
					this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
					return;
				}
				#endregion
			}
			else
			{
				#region Update Record.
				try
				{


					GMSCore.Entity.AccountPurchases accountPurchase = new AccountPurchasesActivity().RetrieveAccountPurchaseByID(session.CompanyId, hidPurchaseID.Value.Trim());
					if (accountPurchase == null)
					{
						base.JScriptAlertMsg("This purchase cannot be found in database.");
						return;
					}
					if (txtSupplier.Text.Trim() == "")
					{
						base.JScriptAlertMsg("Supplier Name cannot be empty.");
						return;
					}

					accountPurchase.Supplier = txtSupplier.Text.Trim();
					accountPurchase.IndustryID = ddlPurchaseIndustry.SelectedValue;
					accountPurchase.ProductGroup = txtPG.Text.Trim();
					accountPurchase.ProductName = txtPN.Text.Trim();
					accountPurchase.UOM = ddlUOM.SelectedValue;
					accountPurchase.Qty = GMSUtil.ToShort(txtQuantity.Text);

					if (contractEndDate.Text.Trim() == "")
						accountPurchase.ContractEndDate = GMSCoreBase.DEFAULT_NO_DATE;

					else if (GMSUtil.ToDate(contractEndDate.Text.Trim()) != GMSCoreBase.DEFAULT_NO_DATE)
						accountPurchase.ContractEndDate = GMSUtil.ToDate(contractEndDate.Text.Trim());
					accountPurchase.Remarks = txtPurchaseRemarks.Text.Trim();
					accountPurchase.ModifiedBy = session.UserId;
					accountPurchase.ModifiedDate = DateTime.Now;

					accountPurchase.Save();
					accountPurchase.Resync();
					hidPurchaseID.Value = accountPurchase.PurchaseID.ToString();
					lblPurchaseMsg.Text = "Record modified successfully!<br /><br />";
					//ScriptManager.RegisterStartupScript(this, typeof(Page), "", "alert('Record modified successfully!')", true);
					LoadPurchasesData();
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", "$('#purchase-modal').modal('hide');$('.modal-backdrop').fadeOut();$('body').removeClass('modal-open');", true); 

				}
				catch (Exception ex)
				{
					this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
					return;
				}
				#endregion


			}

            

		}
		#endregion

		#region dgPurchases_EditCommand
		protected void dgPurchases_EditCommand(object sender, DataGridCommandEventArgs e)
		{

			HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidID");
			LoadSinglePurchaseData(hidID.Value.Trim());
			hidPurchaseID.Value = hidID.Value.Trim();
			//ModalPopupExtender2.Show();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "$('#purchase-modal').modal('show'); $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });", true); 



		}
		#endregion

		#region LoadSinglePurchaseData
		private void LoadSinglePurchaseData(string hidPruchaseID)
		{

			if (hidPruchaseID != "")
			{
				LogSession session = base.GetSessionInfo();
				GMSCore.Entity.AccountPurchases accountPurchase = new AccountPurchasesActivity().RetrieveAccountPurchaseByID(session.CompanyId, hidPruchaseID);

				if (accountPurchase != null)
				{
					txtSupplier.Text = accountPurchase.Supplier;


					ddlPurchaseIndustry.SelectedValue = accountPurchase.IndustryID;
					txtPG.Text = accountPurchase.ProductGroup;
					txtPN.Text = accountPurchase.ProductName;
					ddlUOM.SelectedValue = accountPurchase.UOM.ToString();
					txtQuantity.Text = accountPurchase.Qty.ToString();
					if (accountPurchase.ContractEndDate != null)
						contractEndDate.Text = accountPurchase.ContractEndDate.ToString("dd/MM/yyyy");
					if (accountPurchase.Remarks != null)
						txtPurchaseRemarks.Text = accountPurchase.Remarks;

				}
			}
			else
			{
				txtSupplier.Text = "";
				ddlPurchaseIndustry.SelectedIndex = -1;
				txtPG.Text = "";
				txtPN.Text = "";
				ddlUOM.SelectedIndex = -1;
				txtQuantity.Text = "";
				contractEndDate.Text = "";
				txtPurchaseRemarks.Text = "";
			}

		}
		#endregion

		protected void resetButtonCSS()
		{
            btnLoadParticular.Attributes["class"] = "btn btn-default";
            btnTrigger.Attributes["class"] = "btn btn-default";
            btnLoadCommRecord.Attributes["class"] = "btn btn-default";
            btnLoadAttachment.Attributes["class"] = "btn btn-default";
            btnLoadFinance.Attributes["class"] = "btn btn-default";
            btnLoadSales.Attributes["class"] = "btn btn-default";
            btnLoadCollection.Attributes["class"] = "btn btn-default";
            btnLoadOutstandingPayment.Attributes["class"] = "btn btn-default";
            btnLoadPurchase.Attributes["class"] = "btn btn-default";
            btnLoadOthers.Attributes["class"] = "btn btn-default";

		}

		protected void btnTrigger_Click(object sender, EventArgs args)
		{
            upContact.Visible = true;
			resetButtonCSS();
            btnTrigger.Attributes["class"] = "btn btn-primary active";
			upParticular.Visible = false;
			
			upCommunication.Visible = false;
			upAttachment.Visible = false;
			upFinance.Visible = false;
			upSales.Visible = false;
			upCollection.Visible = false;
			upOutstanding.Visible = false;
			upPurchase.Visible = false;
			upOthers.Visible = false;

			lblContactsMsg.Text = "";
			ViewState["SortContactsField"] = "FirstName";
			ViewState["SortDirection"] = "ASC";
			LoadContactsData();

            //init javascript when panel loaded
            var javascript = @"
                      $(function () {
                        $(document).on('change', ':file', function () {
                            var input = $(this),
                                numFiles = input.get(0).files ? input.get(0).files.length : 1,
                                label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                            input.trigger('fileselect', [numFiles, label]);
                        });

                        $(document).ready(function () {
                            $(':file').on('fileselect', function (event, numFiles, label) {

                                var input = $(this).parents('.input-group').find(':text'),
                                    log = numFiles > 1 ? numFiles + ' files selected' : label;

                                if (input.length) {
                                    input.val(log);
                                } else {
                                    if (log) alert(log);
                                }

                            });
                        });

                    });
                ";

            ScriptManager.RegisterStartupScript(upSales, GetType(), "Javascript", javascript, true);

		}

		protected void btnLoadPurchase_Click(object sender, EventArgs args)
		{
			resetButtonCSS();
			btnLoadPurchase.Attributes["class"] = "btn btn-primary active";
			upParticular.Visible = false;
			upContact.Visible = false;
			upCommunication.Visible = false;
			upAttachment.Visible = false;
			upFinance.Visible = false;
			upSales.Visible = false;
			upCollection.Visible = false;
			upOutstanding.Visible = false;
			upPurchase.Visible = true;
			upOthers.Visible = false;

			lblPurchaseMsg.Text = "";
			ViewState["SortPurchasesField"] = "Supplier";
			LoadPurchasesData();

         
		}

		protected void btnLoadAttachment_Click(object sender, EventArgs args)
		{
			resetButtonCSS();
			btnLoadAttachment.Attributes["class"] = "btn btn-primary active";

			upParticular.Visible = false;
			upContact.Visible = false;
			upCommunication.Visible = false;
			upAttachment.Visible = true;
			upFinance.Visible = false;
			upSales.Visible = false;
			upCollection.Visible = false;
			upOutstanding.Visible = false;
			upPurchase.Visible = false;
			upOthers.Visible = false;

			lblAttachmentMsg.Text = "";
			ViewState["SortAttachmentField"] = "CreatedDate";
			LoadAttachmentData();

            //init javascript when panel loaded
            var javascript = @"
                      $(function () {
                        $(document).on('change', ':file', function () {
                            var input = $(this),
                                numFiles = input.get(0).files ? input.get(0).files.length : 1,
                                label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                            input.trigger('fileselect', [numFiles, label]);
                        });

                        $(document).ready(function () {
                            $(':file').on('fileselect', function (event, numFiles, label) {

                                var input = $(this).parents('.input-group').find(':text'),
                                    log = numFiles > 1 ? numFiles + ' files selected' : label;

                                if (input.length) {
                                    input.val(log);
                                } else {
                                    if (log) alert(log);
                                }

                            });
                        });

                    });
                ";

            ScriptManager.RegisterStartupScript(upSales, GetType(), "Javascript", javascript, true);

		}

		protected void btnLoadCommRecord_Click(object sender, EventArgs args)
		{
			resetButtonCSS();
			btnLoadCommRecord.Attributes["class"] = "btn btn-primary active";
			upParticular.Visible = false;
			upContact.Visible = false;
			upCommunication.Visible = true;
			upAttachment.Visible = false;
			upFinance.Visible = false;
			upSales.Visible = false;
			upCollection.Visible = false;
			upOutstanding.Visible = false;
			upPurchase.Visible = false;
			upOthers.Visible = false;

			lblCommunicationMsg.Text = "";
			ViewState["SortCommRecordField"] = "FromDateTime";
			LoadCommunicationData();

		}

		protected void btnLoadParticular_Click(object sender, EventArgs args)
		{
			resetButtonCSS();
			btnLoadParticular.Attributes["class"] = "btn btn-primary active";

			upParticular.Visible = true;
			upContact.Visible = false;
			upCommunication.Visible = false;
			upAttachment.Visible = false;
			upFinance.Visible = false;
			upSales.Visible = false;
			upCollection.Visible = false;
			upOutstanding.Visible = false;
			upPurchase.Visible = false;
			upOthers.Visible = false;
			lblParticularsMsg.Text = "";
			LoadParticularsData();

		}

		protected void btnLoadOutstandingPayment_Click(object sender, EventArgs args)
		{
			resetButtonCSS();
			btnLoadOutstandingPayment.Attributes["class"] = "btn btn-primary active";
			upParticular.Visible = false;
			upContact.Visible = false;
			upCommunication.Visible = false;
			upAttachment.Visible = false;
			upFinance.Visible = false;
			upSales.Visible = false;
			upCollection.Visible = false;
			upOutstanding.Visible = true;
			upPurchase.Visible = false;
			upOthers.Visible = false;



			ViewState["SortOutstandingField"] = "SALES_TrnDate";
			ViewState["OutstandingRange"] = "0";
			LoadOutstandingPayementsData(0);
			btnAllOutstanding.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FAA634");
			pnlOutstandingPaymentProgress.Visible = false;

		}

		protected void btnLoadFinance_Click(object sender, EventArgs args)
		{
			resetButtonCSS();
			btnLoadFinance.Attributes["class"] = "btn btn-primary active";
			upParticular.Visible = false;
			upContact.Visible = false;
			upCommunication.Visible = false;
			upAttachment.Visible = false;
			upFinance.Visible = true;
			upSales.Visible = false;
			upCollection.Visible = false;
			upOutstanding.Visible = false;
			upPurchase.Visible = false;
			upOthers.Visible = false;
			LogSession session = base.GetSessionInfo();

			UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(loginUserOrAlternateParty, 124);
			if (uAccess == null)
			{
				divFinanceAttachment.Visible = false;
				dgFinanceAttachment.Columns[6].Visible = false;
				
			}

			ViewState["SortFinanceAttachmentField"] = "Type";
			ViewState["SortDirection"] = "DESC";
			LoadFinanceAttachmentData();

            //init javascript when panel loaded
            var javascript = @"
                      $(function () {
                        $(document).on('change', ':file', function () {
                            var input = $(this),
                                numFiles = input.get(0).files ? input.get(0).files.length : 1,
                                label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                            input.trigger('fileselect', [numFiles, label]);
                        });

                        $(document).ready(function () {
                            $(':file').on('fileselect', function (event, numFiles, label) {

                                var input = $(this).parents('.input-group').find(':text'),
                                    log = numFiles > 1 ? numFiles + ' files selected' : label;

                                if (input.length) {
                                    input.val(log);
                                } else {
                                    if (log) alert(log);
                                }

                            });
                        });

                    });
                ";

            ScriptManager.RegisterStartupScript(upSales, GetType(), "Javascript", javascript, true);

		}

		protected void btnLoadSales_Click(object sender, EventArgs args)
		{
			resetButtonCSS();
			btnLoadSales.Attributes["class"] = "btn btn-primary active";
			upParticular.Visible = false;
			upContact.Visible = false;
			upCommunication.Visible = false;
			upAttachment.Visible = false;
			upFinance.Visible = false;
			upSales.Visible = true;
			upCollection.Visible = false;
			upOutstanding.Visible = false;
			upPurchase.Visible = false;
			upOthers.Visible = false;

			ViewState["SortField"] = "TrnNo";

            var javascript = @"
                     $(document).ready(function() {
                        $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
                    });
                ";

            ScriptManager.RegisterStartupScript(upSales, GetType(), "Javascript", javascript, true);
           
		}

		protected void btnLoadOthers_Click(object sender, EventArgs args)
		{
			resetButtonCSS();
			btnLoadOthers.Attributes["class"] = "btn btn-primary active";
			upParticular.Visible = false;
			upContact.Visible = false;
			upCommunication.Visible = false;
			upAttachment.Visible = false;
			upFinance.Visible = false;
			upSales.Visible = false;
			upCollection.Visible = false;
			upOutstanding.Visible = false;
			upPurchase.Visible = false;
			upOthers.Visible = true;


		}

		protected void btnLoadCollection_Click(object sender, EventArgs args)
		{
			resetButtonCSS();
			btnLoadCollection.Attributes["class"] = "btn btn-primary active";
			upParticular.Visible = false;
			upContact.Visible = false;
			upCommunication.Visible = false;
			upAttachment.Visible = false;
			upFinance.Visible = false;
			upSales.Visible = false;
			upCollection.Visible = true;
			upOutstanding.Visible = false;
			upPurchase.Visible = false;
			upOthers.Visible = false;

			ViewState["SortCollectionField"] = "RECEIPT_TrnDate";

            var javascript = @"
                     $(document).ready(function() {
                        $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
                    });
                ";

            ScriptManager.RegisterStartupScript(upSales, GetType(), "Javascript", javascript, true);

		}



		#region AddUpdateCommunication
		protected void AddUpdateCommunication(object sender, CommandEventArgs e)
		{
			LogSession session = base.GetSessionInfo();

            const string javascript = @"$('#communication-modal').modal('hide');$('.modal-backdrop').fadeOut();$('body').removeClass('modal-open');";

			if (string.IsNullOrEmpty(hidCommID1.Value.Trim()))
			{
				#region Add New Record.
				try
				{

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
						documentNumber.AttachmentNo = "0001";
						documentNumber.ProspectNo = "0001";
						documentNumber.ContactNo = "0001";
						documentNumber.CommNo = "0001";
						documentNumber.CommCommentNo = "0001";
						documentNumber.PurchaseNo = "0001";
                        documentNumber.MRNo = "00001";
                        documentNumber.DocumentNo = "00001";
                        documentNumber.DocumentNoForEmployee = "000001";
                        documentNumber.ProjectNo = "000001";
                        documentNumber.CEID = "0001";
                        documentNumber.CEDetailID = "00001";
                        documentNumber.ItemID = "1";
                        documentNumber.ClaimNo = "00001";

                    }

					GMSCore.Entity.AccountCommRecord accountCommRecord = new GMSCore.Entity.AccountCommRecord();

					accountCommRecord.CoyID = session.CompanyId;
					accountCommRecord.CommID = documentNumber.CommNo;
					accountCommRecord.AccountCode = this.hidAccountCode.Value.Trim();

					if (GMSUtil.ToDate(txtDateFrom.Text) != GMSCoreBase.DEFAULT_NO_DATE && txtDateFromTime.Text.Trim() != "")
						accountCommRecord.FromDateTime = GMSUtil.ToDate(txtDateFrom.Text + " " + txtDateFromTime.Text + ":00");
					if (GMSUtil.ToDate(txtDateTo.Text) != GMSCoreBase.DEFAULT_NO_DATE && txtDateToTime.Text.Trim() != "")
						accountCommRecord.ToDateTime = GMSUtil.ToDate(txtDateTo.Text + " " + txtDateToTime.Text + ":00");

					accountCommRecord.Description = txtDescription.Text.Trim();
					accountCommRecord.Status = ddlCommStatus.SelectedValue;
					accountCommRecord.Type = ddlCommType.SelectedValue;
					accountCommRecord.CreatedBy = session.UserId;
					accountCommRecord.CreatedDate = DateTime.Now;

					accountCommRecord.Save();
					accountCommRecord.Resync();


					if (txtComment.Text.Trim() != "")
					{
						GMSCore.Entity.AccountCommRecordComment accountCommRecordComment = new GMSCore.Entity.AccountCommRecordComment();

						accountCommRecordComment.CoyID = session.CompanyId;
						accountCommRecordComment.AccountCode = this.hidAccountCode.Value.Trim();
						accountCommRecordComment.CommID = accountCommRecord.CommID;
						accountCommRecordComment.CommentID = documentNumber.CommCommentNo;
						accountCommRecordComment.Comment = txtComment.Text.Trim();
						accountCommRecordComment.CreatedBy = session.UserId;
						accountCommRecordComment.CreatedDate = DateTime.Now;
						accountCommRecordComment.Save();
						accountCommRecordComment.Resync();
						txtComment.Text = "";


						string nxtCommCommentStr = ((short)(short.Parse(documentNumber.CommCommentNo) + 1)).ToString();
						for (int i = nxtCommCommentStr.Length; i < documentNumber.CommCommentNo.Length; i++)
						{
							nxtCommCommentStr = "0" + nxtCommCommentStr;
						}
						documentNumber.CommCommentNo = nxtCommCommentStr;


					}


					string nxtStr = ((short)(short.Parse(documentNumber.CommNo) + 1)).ToString();
					for (int i = nxtStr.Length; i < documentNumber.CommNo.Length; i++)
					{
						nxtStr = "0" + nxtStr;
					}
					documentNumber.CommNo = nxtStr;

					documentNumber.Save();


					//ScriptManager.RegisterStartupScript(this, typeof(Page), "", "alert('Record created successfully!')", true);
					LoadCommunicationData();
					lblCommunicationMsg.Text = "Record created successfully!<br /><br />";
					//LoadCommunicationCommentData((accountCommRecord.CommID).ToString());


				}
				catch (Exception ex)
				{
					this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
					return;
				}
				#endregion
			}
			else
			{
				#region Update Record.
				try
				{
					//GMSCore.Entity.AccountCommRecord accountCommRecord = GMSCore.Entity.AccountCommRecord.RetrieveByKey(hidCommID1.Value.Trim());

					GMSCore.Entity.AccountCommRecord accountCommRecord = new AccountCommActivity().RetrieveAccountCommRecordByID(session.CompanyId, hidCommID1.Value.Trim());
					if (accountCommRecord == null)
					{
						base.JScriptAlertMsg("This contact cannot be found in database.");
						return;
					}


					if (GMSUtil.ToDate(txtDateFrom.Text) != GMSCoreBase.DEFAULT_NO_DATE && txtDateFromTime.Text.Trim() != "")
						accountCommRecord.FromDateTime = GMSUtil.ToDate(txtDateFrom.Text + " " + txtDateFromTime.Text + ":00");
					if (GMSUtil.ToDate(txtDateTo.Text) != GMSCoreBase.DEFAULT_NO_DATE && txtDateToTime.Text.Trim() != "")
						accountCommRecord.ToDateTime = GMSUtil.ToDate(txtDateTo.Text + " " + txtDateToTime.Text + ":00");

					accountCommRecord.Description = txtDescription.Text.Trim();
					accountCommRecord.Status = ddlCommStatus.SelectedValue;
					accountCommRecord.Type = ddlCommType.SelectedValue;
					accountCommRecord.ModifiedBy = session.UserId;
					accountCommRecord.ModifiedDate = DateTime.Now;

					accountCommRecord.Save();
					accountCommRecord.Resync();

					if (txtComment.Text.Trim() != "")
					{
						DocumentNumber documentNumber = DocumentNumber.RetrieveByKey(session.CompanyId, (short)DateTime.Now.Year);

						GMSCore.Entity.AccountCommRecordComment accountCommRecordComment = new GMSCore.Entity.AccountCommRecordComment();

						accountCommRecordComment.CoyID = session.CompanyId;
						accountCommRecordComment.AccountCode = this.hidAccountCode.Value.Trim();
						accountCommRecordComment.CommID = hidCommID1.Value.Trim();
						accountCommRecordComment.CommentID = documentNumber.CommCommentNo;
						accountCommRecordComment.Comment = txtComment.Text.Trim();
						accountCommRecordComment.CreatedBy = session.UserId;
						accountCommRecordComment.CreatedDate = DateTime.Now;
						accountCommRecordComment.Save();
						accountCommRecordComment.Resync();
						txtComment.Text = "";

						string nxtCommCommentStr = ((short)(short.Parse(documentNumber.CommCommentNo) + 1)).ToString();
						for (int i = nxtCommCommentStr.Length; i < documentNumber.CommCommentNo.Length; i++)
						{
							nxtCommCommentStr = "0" + nxtCommCommentStr;
						}
						documentNumber.CommCommentNo = nxtCommCommentStr;

						documentNumber.Save();
					}

					//ScriptManager.RegisterStartupScript(this, typeof(Page), "", "alert('Record modified successfully!')", true);
					LoadCommunicationData();
					LoadCommunicationCommentData(hidCommID1.Value.Trim());
					lblMessage.Text = "Record modified successfully!";
					//ModalPopupExtender3.Show();
                    ScriptManager.RegisterClientScriptBlock(upCommunication, this.GetType(), "click", "$('#communication-modal').modal('hide')", true); 


				}
				catch (Exception ex)
				{
					this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
					return;
				}
				#endregion
			}

            
            ScriptManager.RegisterClientScriptBlock(upCommunication, this.GetType(), "Javascript", javascript, true); 

		}
		#endregion

		private void LoadCommunicationCommentData(string hidCommID)
		{
			LogSession session = base.GetSessionInfo();

			GMSGeneralDALC dacl = new GMSGeneralDALC();
			DataSet ds = new DataSet();
			try
			{
				dacl.GetCommunicationCommentSelect(session.CompanyId, this.hidAccountCode.Value.Trim(), GMSUtil.ToShort(hidCommID), ref ds);
			}
			catch (Exception ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}

			int startIndex = ((dgCommRecordComment.CurrentPageIndex + 1) * this.dgCommRecordComment.PageSize) - (this.dgCommRecordComment.PageSize - 1);
			int endIndex = (dgCommRecordComment.CurrentPageIndex + 1) * this.dgCommRecordComment.PageSize;

			DataView dv = ds.Tables[0].DefaultView;
			dv.Sort = ViewState["SortCommCommentRecordField"].ToString() + " " + ViewState["SortDirection"].ToString();

			if (ds != null && ds.Tables[0].Rows.Count > 0)
			{
				if (endIndex < ds.Tables[0].Rows.Count)
					this.lblCommCommentRecordSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
				else
					this.lblCommCommentRecordSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();



				this.lblCommCommentRecordSummary.Visible = true;

			}
			else
			{

				this.lblCommCommentRecordSummary.Text = "No records.";
				this.lblCommCommentRecordSummary.Visible = true;

			}
			this.dgCommRecordComment.DataSource = dv;
			this.dgCommRecordComment.DataBind();
			this.dgCommRecordComment.Visible = true;
		}

		private void LoadCommunicationData()
		{

			LogSession session = base.GetSessionInfo();

			GMSGeneralDALC dacl = new GMSGeneralDALC();
			DataSet ds = new DataSet();
			try
			{
				dacl.GetCommunicationSelect(session.CompanyId, this.hidAccountCode.Value.Trim(), ref ds);
			}
			catch (Exception ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}

			int startIndex = ((dgCommRecord.CurrentPageIndex + 1) * this.dgCommRecord.PageSize) - (this.dgCommRecord.PageSize - 1);
			int endIndex = (dgCommRecord.CurrentPageIndex + 1) * this.dgCommRecord.PageSize;

			DataView dv = ds.Tables[0].DefaultView;
			dv.Sort = ViewState["SortCommRecordField"].ToString() + " " + ViewState["SortDirection"].ToString();

			if (ds != null && ds.Tables[0].Rows.Count > 0)
			{
				if (endIndex < ds.Tables[0].Rows.Count)
					this.lblCommRecordSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
				else
					this.lblCommRecordSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();



				this.lblCommRecordSummary.Visible = true;

			}
			else
			{

				this.lblCommRecordSummary.Text = "No records.";
				this.lblCommRecordSummary.Visible = true;

			}
			this.dgCommRecord.DataSource = dv;
			this.dgCommRecord.DataBind();
			this.dgCommRecord.Visible = true;
		}

		protected void dgCommRecordComment_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

				if (lnkDelete != null)
				{
					lnkDelete.Attributes.Add("onclick", "return confirm_delete();");
				}
			}

		}

		protected void dgCommRecordComment_DeleteCommand(object sender, DataGridCommandEventArgs e)
		{

			if (e.CommandName == "Delete")
			{
				LogSession session = base.GetSessionInfo();
				if (session == null)
				{
					Response.Redirect(base.SessionTimeOutPage("Sales"));
					return;
				}

				HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("CommentID");

				if (hidID != null)
				{
					AccountCommActivity apActivity = new AccountCommActivity();


					try
					{
						ResultType result = apActivity.DeleteAccountCommComment(hidID.Value, session);

						switch (result)
						{
							case ResultType.Ok:
								this.dgCommRecordComment.EditItemIndex = -1;
								this.dgCommRecordComment.CurrentPageIndex = 0;
								LoadCommunicationCommentData(hidCommID1.Value.Trim());
								lblMessage.Text = "Record deleted successfully!";
                                ScriptManager.RegisterClientScriptBlock(upCommunication, this.GetType(), "click", "$('#communication-modal').modal('show')", true); 
								//ModalPopupExtender3.Show();
								break;
							default:
								this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
								return;
						}
					}
					catch (SqlException exSql)
					{
						if (exSql.Number == 547)
						{
							this.PageMsgPanel.ShowMessage("This record cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
							LoadCommunicationCommentData(hidCommID1.Value.Trim());
							return;
						}
					}
					catch (Exception ex)
					{
						this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
						LoadCommunicationCommentData(hidCommID1.Value.Trim());
						return;
					}
				}
			}
		}

		protected void dgCommRecordComment_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
		}


		protected void dgCommRecord_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LogSession session = base.GetSessionInfo();
				PopupControlExtender pce = e.Item.FindControl("PopupControlExtender1") as PopupControlExtender;

				string behaviorID = "pce_" + e.Item.ItemIndex;
				pce.BehaviorID = behaviorID;

				Image img = (Image)e.Item.FindControl("imgMagnify");

				string OnMouseOverScript = string.Format("$find('{0}').showPopup();", behaviorID);
				string OnMouseOutScript = string.Format("$find('{0}').hidePopup();", behaviorID);

				img.Attributes.Add("onmouseover", OnMouseOverScript);
				img.Attributes.Add("onmouseout", OnMouseOutScript);

				if (session.WebServiceAddress.Contains("gms.leedenlimited.com"))
				{
					img.Visible = true;
				}
				else
				{
					img.Visible = false;
				}

				LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

				if (lnkDelete != null)
				{


					lnkDelete.Attributes.Add("onclick", "return confirm_delete();");


				}
			}
		}



		protected void dgCommRecord_DeleteCommand(object sender, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				LogSession session = base.GetSessionInfo();
				if (session == null)
				{
					Response.Redirect(base.SessionTimeOutPage("Sales"));
					return;
				}

				HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidCommID");

				if (hidID != null)
				{
					AccountCommActivity apActivity = new AccountCommActivity();


					try
					{
						ResultType result = apActivity.DeleteAccountComm(hidID.Value, session);

						switch (result)
						{
							case ResultType.Ok:
								this.dgCommRecord.EditItemIndex = -1;
								this.dgCommRecord.CurrentPageIndex = 0;
								lblCommunicationMsg.Text = "Record deleted successfully!<br /><br />";
								LoadCommunicationData();
								break;
							default:
								this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
								return;
						}
					}
					catch (SqlException exSql)
					{
						if (exSql.Number == 547)
						{
							this.PageMsgPanel.ShowMessage("This record cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
							LoadCommunicationData();
							return;
						}
					}
					catch (Exception ex)
					{
						this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
						LoadCommunicationData();
						return;
					}
				}
			}
		}


		protected void dgCommRecordComment_Command(Object sender, DataGridCommandEventArgs e)
		{

		}

		#region dgCommRecordComment_EditCommand
		protected void dgCommRecordComment_EditCommand(object sender, DataGridCommandEventArgs e)
		{

			this.dgCommRecordComment.EditItemIndex = e.Item.ItemIndex;
			LoadCommunicationCommentData(hidCommID1.Value.Trim());
            ScriptManager.RegisterClientScriptBlock(upCommunication, this.GetType(), "click", "$('#communication-modal').modal('show')", true); 
			//ModalPopupExtender3.Show();


		}
		#endregion

		#region dgCommRecordComment_UpdateCommand
		protected void dgCommRecordComment_UpdateCommand(object sender, DataGridCommandEventArgs e)
		{
			LogSession session = base.GetSessionInfo();
			TextBox txtEditComment = (TextBox)e.Item.FindControl("txtEditComment");
			HtmlInputHidden hidCommentID = (HtmlInputHidden)e.Item.FindControl("CommentID");

			GMSCore.Entity.AccountCommRecordComment accountCommRecordComment = new AccountCommActivity().RetrieveAccountCommRecordCommentByID(session.CompanyId, hidCommentID.Value.Trim());

			if (accountCommRecordComment == null)
			{
				base.JScriptAlertMsg("This record cannot be found in database.");
				return;
			}

			accountCommRecordComment.Comment = txtEditComment.Text.Trim();
			accountCommRecordComment.ModifiedBy = session.UserId;
			accountCommRecordComment.ModifiedDate = DateTime.Now;
			accountCommRecordComment.Save();
			accountCommRecordComment.Resync();
			this.dgCommRecordComment.EditItemIndex = -1;
			LoadCommunicationCommentData(accountCommRecordComment.CommID.ToString());
			lblMessage.Text = "Record modified successfully!";
            ScriptManager.RegisterClientScriptBlock(upCommunication, this.GetType(), "click", "$('#communication-modal').modal('hide')", true); 
			//ModalPopupExtender3.Show();


		}
		#endregion

		protected void dgCommRecord_Command(Object sender, DataGridCommandEventArgs e)
		{
			switch (((LinkButton)e.CommandSource).CommandName)
			{

				case "EditCommunication":
					HtmlInputHidden hidCommID = (HtmlInputHidden)e.Item.FindControl("hidCommID");
					btnAddUpdateCommunication.Text = "Update";
					LoadSingleCommunicationData(hidCommID.Value.Trim());
					hidCommID1.Value = hidCommID.Value.Trim();
					lblCommunication.Text = "Edit Communication";
					txtComment.Text = "";
					lblMessage.Text = "";
					//ModalPopupExtender3.Show();
                     var javascript = @"
                             $(document).ready(function() {
                                $('.date').datepicker({ format: 'dd/mm/yyyy', autoclose: !0 });
                                $('#communication-modal').modal('show');
                            });
                        ";

                    ScriptManager.RegisterClientScriptBlock(upCommunication, this.GetType(), "click", javascript, true); 
					ViewState["SortCommCommentRecordField"] = "CommentID";
					ViewState["SortDirection"] = "DESC";

					LoadCommunicationCommentData(hidCommID.Value.Trim());

					break;
				case "AddEditComment":
					HtmlInputHidden hidCommID2 = (HtmlInputHidden)e.Item.FindControl("hidCommID");
					hidCommID1.Value = hidCommID2.Value.Trim();
					LoadCommunicationCommentData(hidCommID2.Value.Trim());
					//ModalPopupExtender4.Show();
					break;
				default:
					// Do nothing.
					break;

			}

		}

		protected void dgCommRecord_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			DataGrid dtg = (DataGrid)source;
			dtg.CurrentPageIndex = e.NewPageIndex;
			//LoadContactsData();
		}

		#region GetDynamicContent
		[WebMethod]
		public static string GetDynamicContent(string contextKey)
		{
			short coyid = 1;
			string accountcode = "";
			short commId = 1;
			string[] str = contextKey.Split(';');
			if (str != null && str.Length == 3)
			{
				coyid = GMSUtil.ToShort(str[0].Trim());
				accountcode = str[1].Trim();
				commId = GMSUtil.ToShort(str[2].Trim());
			}



			GMSGeneralDALC dacl = new GMSGeneralDALC();
			DataSet ds = new DataSet();
			try
			{
				dacl.GetCommunicationCommentSelect(coyid, accountcode, commId, ref ds);
			}
			catch (Exception ex)
			{
				//this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}


			StringBuilder b = new StringBuilder();


			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				b.Append("<table class='tTable1' width='500px'>");
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					b.Append("<tr>");
					b.Append("<td><p style='border-width: thin;border-style: solid;'><span style=\"color:Green; size:5px; font-style:italic;\">Comment posted by " + dr["CreatedByName"].ToString() + " on " + dr["CreatedDate"].ToString() + "</span>");
					b.Append("<br />" + dr["Comment"].ToString() + "</p></td>");



				}


			}
			else
			{
				b.Append("<table class='tTable1' width='100px'>");
				b.Append("<tr>");
				b.Append("<td><span style=\"color:Red; size:5px; font-style:italic;\">No comment.</span></td>");
				b.Append("</tr>");
			}

			b.Append("</table>");

			return b.ToString();
		}
		#endregion

		
		protected void dgFinanceAttachment_EditCommand(object sender, DataGridCommandEventArgs e)
		{
			this.dgFinanceAttachment.EditItemIndex = e.Item.ItemIndex;
			LoadFinanceAttachmentData();
		}

		#region dgFinanceAttachment_DeleteCommand
		protected void dgFinanceAttachment_DeleteCommand(object sender, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				LogSession session = base.GetSessionInfo();
				if (session == null)
				{
					Response.Redirect(base.SessionTimeOutPage("Sales"));
					return;
				}

				HtmlInputHidden hidDocumentID = (HtmlInputHidden)e.Item.FindControl("hidDocumentID");

				if (hidDocumentID != null)
				{
					GMSCore.Entity.FinanceAttachment financeAttachment = new AccountAttachmentActivity().RetrieveFinanceAttachmentByDocumentID(GMSUtil.ToShort(hidDocumentID.Value.Trim()));

					if (financeAttachment == null)
					{
						base.JScriptAlertMsg("This record cannot be found in database.");
						return;
					}

					financeAttachment.IsActive = false;
					financeAttachment.Save();                   
					this.dgFinanceAttachment.EditItemIndex = -1;
					lblFinanceAttachmentMsg.Text = "Record deleted successfully!";
					LoadFinanceAttachmentData();

				}
			}
		}
		#endregion
		
		
		protected void dgFinanceAttachment_CancelCommand(object sender, DataGridCommandEventArgs e)
		{            
			this.dgFinanceAttachment.EditItemIndex = -1;
			LoadFinanceAttachmentData();
		}

		protected void dgFinanceAttachment_UpdateCommand(object sender, DataGridCommandEventArgs e)
		{

			 HtmlInputHidden hidDocumentID = (HtmlInputHidden)e.Item.FindControl("hidDocumentID");

			 if (hidDocumentID != null)
			 {
				 DropDownList ddlEditType = (DropDownList)e.Item.FindControl("ddlEditType");


				 GMSCore.Entity.FinanceAttachment financeAttachment = new AccountAttachmentActivity().RetrieveFinanceAttachmentByDocumentID(GMSUtil.ToShort(hidDocumentID.Value.Trim()));

				 financeAttachment.Type = ddlEditType.SelectedValue.ToString();
				 if (financeAttachment.Type == "ROC")
				 {
					 financeAttachment.PeriodMonthFrom = null;
					 financeAttachment.PeriodYearFrom = null;
					 financeAttachment.PeriodMonthTo = null;
					 financeAttachment.PeriodYearTo = null;
				 }
				 else
				 {
					 DropDownList ddlEditYearFrom = (DropDownList)e.Item.FindControl("ddlEditYearFrom");
					 DropDownList ddlEditMonthFrom = (DropDownList)e.Item.FindControl("ddlEditMonthFrom");
					 DropDownList ddlEditYearTo = (DropDownList)e.Item.FindControl("ddlEditYearTo");
					 DropDownList ddlEditMonthTo = (DropDownList)e.Item.FindControl("ddlEditMonthTo");

					 financeAttachment.PeriodYearFrom = GMSUtil.ToShort(ddlEditYearFrom.SelectedValue.ToString());
					 financeAttachment.PeriodMonthFrom = GMSUtil.ToShort(ddlEditMonthFrom.SelectedValue.ToString());
					 financeAttachment.PeriodYearTo = GMSUtil.ToShort(ddlEditYearTo.SelectedValue.ToString());
					 financeAttachment.PeriodMonthTo = GMSUtil.ToShort(ddlEditMonthTo.SelectedValue.ToString());
				 }



				 financeAttachment.Save();
				 this.dgFinanceAttachment.EditItemIndex = -1;
				 lblFinanceAttachmentMsg.Text = "Record updated successfully!";
				 LoadFinanceAttachmentData();
			 }

		}

		protected void dgFinanceAttachment_Command(Object sender, DataGridCommandEventArgs e)
		{
			string ext = Path.GetExtension(e.CommandArgument.ToString());
			string ContentType = "";

			switch (((LinkButton)e.CommandSource).CommandName)
			{

				case "Load":
					if (ext == ".asf")
						ContentType = "video/x-ms-asf";
					else if (ext == ".avi")
						ContentType = "video/avi";
					else if (ext == ".doc")
						ContentType = "application/msword";
					else if (ext == ".zip")
						ContentType = "application/zip";
					else if (ext == ".xls")
						ContentType = "application/vnd.ms-excel";
					else if (ext == ".gif")
						ContentType = "image/gif";
					else if (ext == ".jpg" || ext == "jpeg")
						ContentType = "image/jpeg";
					else if (ext == ".wav")
						ContentType = "audio/wav";
					else if (ext == ".mp3")
						ContentType = "audio/mpeg3";
					else if (ext == ".mpg" || ext == "mpeg")
						ContentType = "video/mpeg";
					else if (ext == ".mp3")
						ContentType = "audio/mpeg3";
					else if (ext == ".rtf")
						ContentType = "application/rtf";
					else if (ext == ".htm" || ext == "html")
						ContentType = "text/html";
					else if (ext == ".asp")
						ContentType = "text/asp";
					else
						ContentType = "application/octet-stream";

					Response.ContentType = ContentType.ToString();
					Response.AppendHeader("Content-Disposition", "attachment; filename=" + e.CommandArgument.ToString());
					Response.TransmitFile("C://GMS/CRM/" + e.CommandArgument.ToString());
					Response.End();
					break;

				// Add other cases here, if there are multiple ButtonColumns in 
				// the DataGrid control.

				default:
					// Do nothing.
					break;

			}

		}

		#region dgFinanceAttachment_ItemDataBound
		protected void dgFinanceAttachment_ItemDataBound(object sender, DataGridItemEventArgs e)
		{

			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LinkButton ibtnDownload = (LinkButton)e.Item.FindControl("linkName");
							
				


				if (ibtnDownload != null)
				{                    
					ScriptManager sm = ScriptManager.GetCurrent(this.Page);
					if (sm != null)
					{
						sm.RegisterPostBackControl(ibtnDownload);
					}
				}

				LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

				if (lnkDelete != null)
				{
					lnkDelete.Attributes.Add("onclick", "return confirm_delete();");
				}
			}
			else if (e.Item.ItemType == ListItemType.EditItem)
			{
				DropDownList ddlEditType = (DropDownList)e.Item.FindControl("ddlEditType");
				HiddenField hidType = (HiddenField)e.Item.FindControl("hidType");

				if (ddlEditType != null)
				{  
						
						IList<FinanceAttachmentType> lstFinanceAttachmentType = null;
						lstFinanceAttachmentType = new SystemDataActivity().RetrieveAllFinanceAttachmentTypeSortByID();
						ddlEditType.DataSource = lstFinanceAttachmentType;
						ddlEditType.DataBind();
						ddlEditType.SelectedValue = hidType.Value.ToString();       
			

						
				}

				if (hidType.Value.ToString() == "ROC")
				{
					HtmlInputHidden hidDocumentID = (HtmlInputHidden)e.Item.FindControl("hidDocumentID");
					Panel pnl = (Panel)e.Item.FindControl("pnl");
					pnl.Visible = false;

				}

				DataTable dtt1 = new DataTable();
				dtt1.Columns.Add("Year", typeof(string));

				for (int i = -5; i < 5; i++)
				{
					DataRow dr1 = dtt1.NewRow();
					dr1["Year"] = DateTime.Now.Year + i;

					dtt1.Rows.Add(dr1);
				}

				DataTable dtt2 = new DataTable();
				dtt2.Columns.Add("Month", typeof(string));

				for (int i = 1; i < 13; i++)
				{
					DataRow dr2 = dtt2.NewRow();
					dr2["Month"] = i;

					dtt2.Rows.Add(dr2);
				}

				DropDownList ddlEditYearFrom = (DropDownList)e.Item.FindControl("ddlEditYearFrom");
				HiddenField hidYearFrom = (HiddenField)e.Item.FindControl("hidYearFrom");              

				if (ddlEditYearFrom != null)
				{
					ddlEditYearFrom.DataSource = dtt1;
					ddlEditYearFrom.DataBind();
					if (hidYearFrom != null)
						ddlEditYearFrom.SelectedValue = hidYearFrom.Value.ToString();
					else
					{
						ddlEditYearFrom.SelectedValue = DateTime.Now.Year.ToString();
						ddlEditYearFrom.Visible = false;
					}
				}

				DropDownList ddlEditYearTo = (DropDownList)e.Item.FindControl("ddlEditYearTo");
				HiddenField hidYearTo = (HiddenField)e.Item.FindControl("hidYearTo");

				if (ddlEditYearFrom != null)
				{
					ddlEditYearTo.DataSource = dtt1;
					ddlEditYearTo.DataBind();
					if (hidYearTo != null)
						ddlEditYearTo.SelectedValue = hidYearTo.Value.ToString();
					else
					{
						ddlEditYearTo.SelectedValue = DateTime.Now.Year.ToString();
						ddlEditYearTo.Visible = false;
					}
				}

				DropDownList ddlEditMonthFrom = (DropDownList)e.Item.FindControl("ddlEditMonthFrom");
				HiddenField hidMonthFrom = (HiddenField)e.Item.FindControl("hidMonthFrom");

				if (ddlEditMonthFrom != null)
				{
					ddlEditMonthFrom.DataSource = dtt2;
					ddlEditMonthFrom.DataBind();
					if (hidMonthFrom != null)
						ddlEditMonthFrom.SelectedValue = hidMonthFrom.Value.ToString();
					else
					{
						ddlEditMonthFrom.SelectedValue = DateTime.Now.Month.ToString();
						ddlEditMonthFrom.Visible = false;
					}
				}

				DropDownList ddlEditMonthTo = (DropDownList)e.Item.FindControl("ddlEditMonthTo");
				HiddenField hidMonthTo = (HiddenField)e.Item.FindControl("hidMonthTo");

				if (ddlEditMonthTo != null)
				{
					ddlEditMonthTo.DataSource = dtt2;
					ddlEditMonthTo.DataBind();
					if (hidMonthTo != null)
						ddlEditMonthTo.SelectedValue = hidMonthTo.Value.ToString();
					else
					{
						ddlEditMonthTo.SelectedValue = DateTime.Now.Month.ToString();
						ddlEditMonthTo.Visible = false;
					}
				}




			
					
			}

		}
		#endregion
	   
		protected void dgFinanceAttachment_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			DataGrid dtg = (DataGrid)source;
			dtg.CurrentPageIndex = e.NewPageIndex;
			LoadFinanceAttachmentData();


		}

		protected void ddlEditType_SelectedIndexChanged(object sender, EventArgs e)
		{
			LogSession session = base.GetSessionInfo();
			DropDownList ddlEditType = (DropDownList)sender;

			TableRow tr = (TableRow)ddlEditType.Parent.Parent;
			Panel pnl = (Panel)tr.FindControl("pnl");

			if (ddlEditType.SelectedValue.ToString() == "ROC")
				pnl.Visible = false;
			else
				pnl.Visible = true;
		   
		}

		protected void ddlAttachmentType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ddlAttachmentType.SelectedValue.ToString() == "Finance")
			{
				ddlPeriodYearFrom.Enabled = true;
				ddlPeriodMonthFrom.Enabled = true;
				ddlPeriodYearTo.Enabled = true;
				ddlPeriodMonthTo.Enabled = true;
			}
			else
			{
				ddlPeriodYearFrom.Enabled = false;
				ddlPeriodMonthFrom.Enabled = false;
				ddlPeriodYearTo.Enabled = false;
				ddlPeriodMonthTo.Enabled = false;   
			}

		}


	}
}
