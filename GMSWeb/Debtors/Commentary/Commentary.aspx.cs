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
using System.Globalization;

using GMSCore;
using GMSCore.Entity;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Collections.Generic;

namespace GMSWeb.Debtors.Commentary
{
	public partial class Commentary : GMSBasePage
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
																			84);
            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, loginUserOrAlternateParty,
                                                                            84);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage("Sales")); 

			if (!Page.IsPostBack)
			{
				//preload
				txtAsOfDate.Text = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddDays(-1).ToString("dd/MM/yyyy");
				PopulateSalesperson();
				//dgData.CurrentPageIndex = 0;
				//LoadData();
			}

			string javaScript =
			@"
<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>
<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/date.js""></script>
<uctrl:Header ID=""MySiteHeader"" runat=""server"" EnableViewState=""true"" />
<script language=""javascript"" type=""text/javascript"">
	function btnSearch_OnClick()
	{
		var YY = document.getElementById('"; javaScript += txtAsOfDate.ClientID; javaScript += @"').value.substring(6); 
		if (document.getElementById('"; javaScript += txtAsOfDate.ClientID; javaScript += @"').value.substring(3,4) == '0') 
			var MM = parseInt(document.getElementById('"; javaScript += txtAsOfDate.ClientID; javaScript += @"').value.substring(4,5))-1; 
		else 
			var MM = parseInt(document.getElementById('"; javaScript += txtAsOfDate.ClientID; javaScript += @"').value.substring(3,5))-1; 
		var DD = parseInt(document.getElementById('"; javaScript += txtAsOfDate.ClientID; javaScript += @"').value.substring(0,2)); 
		var selectedDate = new Date(YY,MM,DD); 
		var tempDate = selectedDate.addMonths(1); 
		var tempDate1 = new Date(tempDate.getYear(),tempDate.getMonth(),1); 
		tempDate1.setDate(tempDate1.getDate() - 1); 
		var lastDay = tempDate1.getDate(); 
		if (DD != lastDay) 
		{
			alert('You must select the last day of the month.'); 
			return false; 
		}
		else 
			return true; 
	}

	function EditComment(obj)
	{
		 var hidCommentID = obj.id;
		 hidCommentID = hidCommentID.replace(""lnkEditComment1"", ""hidOwnComment1"");
		 hidCommentID = hidCommentID.replace(""lnkEditComment2"", ""hidOwnComment2"");
		 var textarea=document.getElementsByTagName(""textarea"");
		 
		 for(var i=0;i<textarea.length;i++)
		 {
			if(textarea[i].id.indexOf(""txtComment"")!=-1)
			{
				textarea[i].value = document.getElementById(hidCommentID).value;
			}
			update();
		 }
		 var hidAccountCodeID = obj.id;
		 hidAccountCodeID = hidAccountCodeID.replace(""lnkEditComment1"", ""hidAccountCode"");
		 hidAccountCodeID = hidAccountCodeID.replace(""lnkEditComment2"", ""hidAccountCode"");
		 document.getElementById("""; javaScript += hidAccountCode.ClientID; javaScript += @""").value = document.getElementById(hidAccountCodeID).value;
		 
		 var hidCurrencyID = obj.id;
		 hidCurrencyID = hidCurrencyID.replace(""lnkEditComment1"", ""hidCurrency"");
		 hidCurrencyID = hidCurrencyID.replace(""lnkEditComment2"", ""hidCurrency"");
		 document.getElementById("""; javaScript += hidCurrency.ClientID; javaScript += @""").value = document.getElementById(hidCurrencyID).value;
		 
		 var hidCommentDateID = obj.id;
		 hidCommentDateID = hidCommentDateID.replace(""lnkEditComment1"", ""hidComment1Date"");
		 hidCommentDateID = hidCommentDateID.replace(""lnkEditComment2"", ""hidComment2Date"");
		 document.getElementById("""; javaScript += hidCommentDate.ClientID; javaScript += @""").value = document.getElementById(hidCommentDateID).value;
	}
	
	function viewCommentHistory(AccountCode, Currency)
	{			
		var CoyID = document.getElementById('"; javaScript += hidCoyID.ClientID; javaScript += @"').value;					
		var url = ""CommentaryHistory.aspx?CoyID="" + CoyID + ""&AccountCode="" + AccountCode + ""&CurrencyCode="" + Currency; 
		window.open(url,"""",""width="" + 800 + "",height="" + 600 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");			
		return false;
	}	
	
	function viewDetail(Type, AccountCode, Currency)
	{			
		var CoyID = document.getElementById('"; javaScript += hidCoyID.ClientID; javaScript += @"').value;	
		var AsOfDate = document.getElementById('"; javaScript += hidAsOfDate.ClientID; javaScript += @"').value;	
        var SalesPersonType = document.getElementById('"; javaScript += ddlSalesPersonType.ClientID; javaScript += @"').options[document.getElementById('"; javaScript += ddlSalesPersonType.ClientID; javaScript += @"').selectedIndex].value;
        //"" is removed	as comment Eason 2020/04/20 
        var SalesPersonID = document.getElementById('; javaScript += ddlSalesperson.ClientID; javaScript += @').options[document.getElementById('; javaScript += ddlSalesperson.ClientID; javaScript += @').selectedIndex].value;	
       	
		var url = ""DebtorDetails.aspx?CoyID="" + CoyID + ""&AccountCode="" + 
					AccountCode + ""&CurrencyCode="" + Currency + ""&Type="" + Type + ""&AsOfDate="" + AsOfDate + ""&SalesPersonType=""+ SalesPersonType + ""&SalesPersonID=""+ SalesPersonID ; 
		//window.open(url,window,""dialogWidth:35;dialogHeight:40"");	
		window.open(url,"""",""width="" + 600 + "",height="" + 600 +"",resizable=yes,status=yes,menubar=no,scrollbars=no"");			
		return false;
	}	

	function viewPaymentDetail(Type, AccountCode, Currency, PaymentRefNo)
	{			
		var CoyID = document.getElementById('"; javaScript += hidCoyID.ClientID; javaScript += @"').value;	
		var AsOfDate = document.getElementById('"; javaScript += hidAsOfDate.ClientID; javaScript += @"').value;					
		var SalesPersonType = document.getElementById('"; javaScript += ddlSalesPersonType.ClientID; javaScript += @"').options[document.getElementById('"; javaScript += ddlSalesPersonType.ClientID; javaScript += @"').selectedIndex].value;	
        //"" is removed	to ignore error - Eason 2020/04/20 
        var SalesPersonID = document.getElementById('; javaScript += ddlSalesperson.ClientID; javaScript += @').options[document.getElementById('; javaScript += ddlSalesperson.ClientID; javaScript += @').selectedIndex].value;       	
        var url = ""DebtorPaymentDetails.aspx?CoyID="" + CoyID + ""&AccountCode="" + 
					AccountCode + ""&CurrencyCode="" + Currency + ""&Type="" + Type + ""&AsOfDate="" + AsOfDate + ""&PaymentRefNo=""+PaymentRefNo+ ""&SalesPersonType=""+ SalesPersonType + ""&SalesPersonID=""+ SalesPersonID ; ; 
		//window.open(url,window,""dialogWidth:35;dialogHeight:40"");	
		window.open(url,"""",""width="" + 700 + "",height="" + 600 +"",resizable=yes,status=yes,menubar=no,scrollbars=no"");			
		return false;
	}	
	
	function viewLastPaymentDetail(AccountCode, DocNo)
	{			
		var CoyID = document.getElementById('"; javaScript += hidCoyID.ClientID; javaScript += @"').value;	
		var AsOfDate = document.getElementById('"; javaScript += hidAsOfDate.ClientID; javaScript += @"').value;					
		var SalesPersonType = document.getElementById('"; javaScript += ddlSalesPersonType.ClientID; javaScript += @"').options[document.getElementById('"; javaScript += ddlSalesPersonType.ClientID; javaScript += @"').selectedIndex].value;	
       //"" is removed	to ignore error - Eason 2020/04/20 
        var SalesPersonID = document.getElementById('; javaScript += ddlSalesperson.ClientID; javaScript += @').options[document.getElementById('; javaScript += ddlSalesperson.ClientID; javaScript += @').selectedIndex].value;	
        var url = ""DebtorLastPaymentDetails.aspx?CoyID="" + CoyID + ""&AccountCode="" + 
					AccountCode + ""&DocNo="" + DocNo+ ""&SalesPersonType=""+ SalesPersonType + ""&SalesPersonID=""+ SalesPersonID ; ; 
		//window.open(url,window,""dialogWidth:35;dialogHeight:40"");	
		window.open(url,"""",""width="" + 700 + "",height="" + 600 +"",resizable=yes,status=yes,menubar=no,scrollbars=no"");			
		return false;
	}	

	function print()
	{
		var reportID = document.getElementById('"; javaScript += ddlReport.ClientID; javaScript += @"').options[document.getElementById('"; javaScript += ddlReport.ClientID; javaScript += @"').selectedIndex].value;
		jsOpenOperationalReport('Reports/Report/SalesReportViewer.aspx?REPORTID=' + reportID);
	}
	
	function update() {
	   
	   var textarea=document.getElementsByTagName(""textarea"");
		 for(var i=0;i<textarea.length;i++)
		 {
			if(textarea[i].id.indexOf(""txtComment"")!=-1)
			{
			document.getElementById('"; javaScript += txtCounter.ClientID; javaScript += @"').value = textarea[i].value.length;
			}
		 }
	 }
</script>
";
			Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
		}
        #endregion

        public void lnkViewDetail_Click(object sender, CommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string[] arg = new string[4];
            arg = e.CommandArgument.ToString().Split(';');
            string type = arg[0];
            string accountCode = arg[1];
            string salesCurrency = arg[2];
            string salespersonID = arg[3];
            string coyID = session.CompanyId.ToString();
            string asOfDate = txtAsOfDate.Text.Trim();
            string salespersonType = ddlSalesPersonType.SelectedValue.Trim();

            string url = "DebtorDetails.aspx?CoyID=" + coyID + "&AccountCode=" + accountCode + "&CurrencyCode=" + salesCurrency
           + "&Type=" + type + "&AsOfDate=" + asOfDate + "&SalesPersonType=" + salespersonType + "&SalesPersonID=" + salespersonID;

            string strPopup = "<script language='javascript' ID='script1'>"
           + "window.open('"+url
          + "','new window', ' width=600, height=600, dependant=no, location=0,  resizeable=no, scrollbars=no, toolbar=no, status=yes')"
           + "</script>";
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
        }

        public void lnkViewPaymentDetail_Click(object sender, CommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string[] arg = new string[5];
            arg = e.CommandArgument.ToString().Split(';');
            string type = arg[0];
            string accountCode = arg[1];
            string salesCurrency = arg[2];
            string paymentRefNo = arg[3];
            string salespersonID = arg[4];
            string coyID = session.CompanyId.ToString();
            string asOfDate = txtAsOfDate.Text.Trim();
            string salespersonType = ddlSalesPersonType.SelectedValue.Trim();

            string url = "DebtorPaymentDetails.aspx?CoyID=" + coyID + "&AccountCode=" + accountCode + "&CurrencyCode=" + salesCurrency
           + "&Type=" + type + "&AsOfDate=" + asOfDate + "&PaymentRefNo=" + paymentRefNo +"&SalesPersonType=" + salespersonType + "&SalesPersonID=" + salespersonID;

            string strPopup = "<script language='javascript' ID='script1'>"
           + "window.open('" + url
          + "','new window', ' width=600, height=600, dependant=no, location=0,  resizeable=no, scrollbars=no, toolbar=no, status=yes')"
           + "</script>";
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
        }

        public void lnkViewLastPaymentDetail_Click(object sender, CommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string[] arg = new string[3];
            arg = e.CommandArgument.ToString().Split(';');
            string accountCode = arg[0];
            string docNo = arg[1];
            string salespersonID = arg[2];
            string coyID = session.CompanyId.ToString();
            string asOfDate = txtAsOfDate.Text.Trim();
            string salespersonType = ddlSalesPersonType.SelectedValue.Trim();

            string url = "DebtorLastPaymentDetails.aspx?CoyID=" + coyID + "&AccountCode=" + accountCode + "&DocNo=" + docNo
          + "&SalesPersonType=" + salespersonType + "&SalesPersonID=" + salespersonID;

            string strPopup = "<script language='javascript' ID='script1'>"
           + "window.open('" + url
          + "','new window', ' width=600, height=600, dependant=no, location=0,  resizeable=no, scrollbars=no, toolbar=no, status=yes')"
           + "</script>";
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
        }

        #region PopulateSalesperson
        private void PopulateSalesperson()
		{
			LogSession session = base.GetSessionInfo();
			DataSet lstSalesPerson = new DataSet();
			new DebtorCommentaryDALC().GetSalesPersonRecords(session.CompanyId, loginUserOrAlternateParty, ref lstSalesPerson);

			if (lstSalesPerson != null && lstSalesPerson.Tables[0].Rows.Count > 0)
			{
				//this.ddlSalesperson.DataSource = lstSalesPerson;
				//this.ddlSalesperson.DataValueField = "SalesPersonID";
				//this.ddlSalesperson.DataTextField = "SalesPersonNameID";
				//this.ddlSalesperson.DataBind();
			}
		}
		#endregion

		#region LoadData
		private void LoadData()
		{
			LogSession session = base.GetSessionInfo();
			DateTime asOfDate = GMSUtil.ToDate(txtAsOfDate.Text.ToString());
			string salesPersonID = "%" + txtSalespersonID.Text.Trim() + "%";
            string salespersonName = "%" + txtSalesPersonName.Text.Trim() + "%";
            string salesPersonType = ddlSalesPersonType.SelectedValue.Trim();
            short days = GMSUtil.ToShort(ddlPeriod.SelectedValue.Trim());
            string accountCode = "%" + txtAccountCode.Text.Trim() + "%";
            string accountName = "%" + txtAccountName.Text.Trim() + "%";

            DebtorCommentaryDALC dcDALC = new DebtorCommentaryDALC();
			DataSet ds = new DataSet();
			try
			{
				dcDALC.GetDebtorsRecordsWithDays(session.CompanyId, days, asOfDate, salesPersonID, loginUserOrAlternateParty, salesPersonType , accountCode, accountName,salespersonName, ref ds);
			}
			catch (Exception ex)
			{
				this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
			}

			int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
			int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				dgData.Columns[5].ItemStyle.CssClass = "apacity_100";
				dgData.Columns[6].ItemStyle.CssClass = "apacity_100";
				dgData.Columns[7].ItemStyle.CssClass = "apacity_100";
				dgData.Columns[8].ItemStyle.CssClass = "apacity_100";
				dgData.Columns[9].ItemStyle.CssClass = "apacity_100";
				dgData.Columns[10].ItemStyle.CssClass = "apacity_100";
				dgData.Columns[11].ItemStyle.CssClass = "apacity_100";

				if (days == 90)
				{
					dgData.Columns[5].ItemStyle.CssClass = "apacity_100";
					dgData.Columns[6].ItemStyle.CssClass = "apacity_100";
					dgData.Columns[7].ItemStyle.CssClass = "apacity_100";
				}  
				else if (days == 120)
				{
					dgData.Columns[5].ItemStyle.CssClass = "apacity_100";
					dgData.Columns[6].ItemStyle.CssClass = "apacity_100";
					dgData.Columns[7].ItemStyle.CssClass = "apacity_100";
					dgData.Columns[8].ItemStyle.CssClass = "apacity_100";
				}
				else if (days == 180)
				{
					dgData.Columns[5].ItemStyle.CssClass = "apacity_100";
					dgData.Columns[6].ItemStyle.CssClass = "apacity_100";
					dgData.Columns[7].ItemStyle.CssClass = "apacity_100";
					dgData.Columns[8].ItemStyle.CssClass = "apacity_100";
					dgData.Columns[9].ItemStyle.CssClass = "apacity_100";
				}  
			   
				
				if (endIndex < ds.Tables[0].Rows.Count)
					this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
				else
					this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
						ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();

                this.lblSearchSummary.Visible = true;
                this.dgData.DataSource = ds;
                this.dgData.DataBind();
                this.dgData.Visible = true;

            }
            else
            {
                this.lblSearchSummary.Text = "No records.";
                this.dgData.Visible = false;
            }
				

            hidAsOfDate.Value = txtAsOfDate.Text.ToString();
            //hidSalesperson.Value = txtSalespersonID.Text.Trim();
            //hidSalesperson.Value = ddlSalesperson.SelectedValue.Trim();
            hidCoyID.Value = session.CompanyId.ToString();

            this.lblSearchSummary.Visible = true;
			
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

		#region btnSearch_Click
		protected void btnSearch_Click(object sender, EventArgs e)
		{
			this.dgData.CurrentPageIndex = 0;
			btnSearch.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FAA634");
            //if(txtAccountCode.Text.Trim() != ""|| txtAccountName.Text.Trim() != ""|| txtSalespersonID.Text.Trim() != "" || txtSalesPersonName.Text.Trim() != "")
			    LoadData();
            //else
            //{
            //    this.MsgPanel2.ShowMessage("Please input Customer Info or Salesperson Info.", MessagePanelControl.MessageEnumType.Alert);
            //    this.dgData.Visible = false;
            //    this.lblSearchSummary.Visible = false;
            //}
                
        }
		#endregion
	
		#region EditCommentCommand
		protected void EditCommentCommand(object sender, CommandEventArgs e)
		{
			LogSession session = base.GetSessionInfo();
			short companyId = GMSUtil.ToShort(hidCoyID.Value.Trim());
			string accountCode = hidAccountCode.Value.Trim();
			string currency = hidCurrency.Value.Trim();
			DateTime commentDate = GMSUtil.ToDate(hidCommentDate.Value.Trim());
			string comment = txtComment.Text.Trim();
			if (comment.Length > 600)
				comment = comment.Substring(0, 600);


			DebtorCommentary dc = DebtorCommentary.RetrieveByKey(companyId, accountCode, currency, commentDate, loginUserOrAlternateParty);
			if (dc != null)
			{
				//if (dc.CommentDate < (new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1)))
				//{
				//base.JScriptAlertMsg("You are not allowed to edit previous month's comments.");
				//}
				//else
				//{
				if (comment.Trim() == "")
				{
					dc.Delete();
					dc.Resync();
				}
				else
				{
					dc.Comment = comment;
					dc.ModifiedBy = session.UserId;
					dc.ModifiedDate = DateTime.Now;
					dc.Save();
					dc.Resync();
				}
				//}
			}
			else
			{
				//if (commentDate < new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1))
				//{
				//    base.JScriptAlertMsg("You are not allowed to edit previous month's comments.");
				//}
				//else
				//{
				dc = new DebtorCommentary();
				dc.AccountCode = accountCode;
				dc.CoyID = companyId;
				dc.CurrencyCode = currency;
				dc.CommentDate = commentDate;
				dc.Comment = comment;
				dc.CreatedBy = session.UserId;
				dc.CreatedDate = DateTime.Now;
				dc.Save();
				dc.Resync();
				//}
			}
			LoadData();
		}
		#endregion

		protected string FixCrLf(string value)
		{

			if (String.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			else
			{
				return value.Replace(Environment.NewLine, "<br />");
			}
		}
	}
}
