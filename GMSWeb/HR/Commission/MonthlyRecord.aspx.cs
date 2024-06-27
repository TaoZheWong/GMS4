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
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Globalization;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.HR.Commission
{
    public partial class MonthlyRecord : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("CompanyHR"); 
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            72);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyHR"));

            if (!Page.IsPostBack)
            {
                //preload
                PopulateDDL();
                LoadData();
            }

            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>
            <script language=""javascript"" type=""text/javascript"">
            function ChangeSalesman(ctl)
            { 
                document.getElementById(ddlNewGPQ).value = ctl.value;
                if (document.getElementById(ddlNewGPQ).selectedIndex >= 0) 
                    document.getElementById(lblGPQ2).innerHTML = document.getElementById(ddlNewGPQ).options[document.getElementById(ddlNewGPQ).selectedIndex].text; 
                else 
                    document.getElementById(lblGPQ2).innerHTML = 'Nill'; 
                document.getElementById(ddlNewCommRate).value = ctl.value;
                if (document.getElementById(ddlNewCommRate).selectedIndex >= 0) 
                    document.getElementById(lblCommRate2).innerHTML = document.getElementById(ddlNewCommRate).options[document.getElementById(ddlNewCommRate).selectedIndex].text; 
                else 
                    document.getElementById(lblCommRate2).innerHTML = 'Nill';
            }
            </script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            short year = GMSUtil.ToShort(ddlYear.SelectedValue);
            short month = GMSUtil.ToShort(ddlMonth.SelectedValue);
            string GroupType = GMSUtil.ToStr(ddlGroupType.SelectedValue);
            IList<GMSCore.Entity.SalesPersonRecord> lstSalesPersonRecord = null;
            DataSet dsSalesRecord = new DataSet();

            // Only Allow SOX for SAP live run in 1 Apr 2024
            if ((session.CompanyId.ToString() == "98" && 
                    (Convert.ToInt32(year) > 2024 || (Convert.ToInt32(year) == 2024 && Convert.ToInt32(month) >= 4))
                ) || session.CompanyId.ToString() != "98")
            {
                string IssuesDetected = IsVerifiedSalesAmount(year, month);
                if (IssuesDetected != "")
                {
                    this.PageMsgPanel.ShowMessage("Alert! " + IssuesDetected + " for selected year and month does not tally.", MessagePanelControl.MessageEnumType.Alert);
                    this.lblSearchSummary.Visible = false;
                    this.dgData.DataSource = null;
                    this.dgData.DataBind();
                    return;
                }
            }          
            
            try
            {
                lstSalesPersonRecord = new SystemDataActivity().RetrieveAllSalesPersonRecordListByCompanyIDSortByYearMonthSalesPersonMasterID(session.CompanyId, year, month, GroupType);
                new GMSGeneralDALC().GetSalesPersonRecord(session.CompanyId, year, month, GroupType, ref dsSalesRecord);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lstSalesPersonRecord != null && lstSalesPersonRecord.Count > 0)
            {
                if (endIndex < lstSalesPersonRecord.Count)
                    this.lblSearchSummary.Text = "Result List" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstSalesPersonRecord.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Result List" + " " + startIndex.ToString() + " - " +
                        lstSalesPersonRecord.Count.ToString() + " " + "of" + " " + lstSalesPersonRecord.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = dsSalesRecord.Tables[0];
           //this.dgData.DataSource = lstSalesPersonRecord;
            this.dgData.DataBind();
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

        #region dgData_ItemDataBound
        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
                DropDownList ddlNewGPQ = (DropDownList)e.Item.FindControl("ddlNewGPQ");
                DropDownList ddlNewCommRate = (DropDownList)e.Item.FindControl("ddlNewCommRate");

                DropDownList ddlNewYear = (DropDownList)e.Item.FindControl("ddlNewYear");
                // load year ddl
                DataTable dtt1 = new DataTable();
                dtt1.Columns.Add("Year", typeof(string));

                if (GMSUtil.ToShort(ddlYear.SelectedValue) == 0)
                {
                    for (int i = -4; i < 5; i++)
                    {
                        DataRow dr1 = dtt1.NewRow();
                        dr1["Year"] = DateTime.Now.Year + i;
                        dtt1.Rows.Add(dr1);
                    }
                }
                else {
                    
                    DataRow dr1 = dtt1.NewRow();
                    dr1["Year"] = GMSUtil.ToShort(ddlYear.SelectedValue);
                    dtt1.Rows.Add(dr1);
                        
                }
                 

                ddlNewYear.DataSource = dtt1;
                ddlNewYear.DataBind();

                if (GMSUtil.ToShort(ddlYear.SelectedValue) == 0)
                    ddlNewYear.SelectedValue = DateTime.Now.Year.ToString();
                else
                    ddlNewYear.SelectedValue = ddlYear.SelectedValue;

                DropDownList ddlNewMonth = (DropDownList)e.Item.FindControl("ddlNewMonth");
                DataTable dtt2 = new DataTable();
                dtt2.Columns.Add("Month", typeof(string));

                if (GMSUtil.ToShort(ddlYear.SelectedValue) == 0)
                {
                    for (int i = 1; i < 13; i++)
                    {
                        DataRow dr2 = dtt2.NewRow();
                        dr2["Month"] = i;
                        dtt2.Rows.Add(dr2);
                    }
                }
                else
                {
                    DataRow dr2 = dtt2.NewRow();
                    dr2["Month"] = GMSUtil.ToShort(ddlMonth.SelectedValue);
                    dtt2.Rows.Add(dr2);

                }


                ddlNewMonth.DataSource = dtt2;
                ddlNewMonth.DataBind();

                if (GMSUtil.ToShort(ddlMonth.SelectedValue) == 0)
                    ddlNewMonth.SelectedValue = DateTime.Now.Month.ToString();
                else
                    ddlNewMonth.SelectedValue = ddlMonth.SelectedValue;


                if (ddlNewSalesPersonMasterName != null && ddlNewGPQ != null && ddlNewCommRate != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in currency dropdown list
                    IList<GMSCore.Entity.SalesPersonMaster> lstSalesPerson = null;
                    if (GMSUtil.ToShort(ddlYear.SelectedValue) == 0 || GMSUtil.ToShort(ddlMonth.SelectedValue) == 0)
                    {
                        lstSalesPerson = sDataActivity.RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterName(session.CompanyId);
                        ddlNewSalesPersonMasterName.DataSource = lstSalesPerson;
                        ddlNewSalesPersonMasterName.DataBind();
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlNewSalesPersonMasterName, ddlNewSalesPersonMasterName.GetType(), "script1", "<script type=\"text/javascript\"> var ddlNewSalesPersonMasterName = '" + ddlNewSalesPersonMasterName.ClientID + "';</script>", false);
                    }
                    else
                    {
                        DataSet dsGMS = new DataSet();
                        new GMSGeneralDALC().RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterName(session.CompanyId, GMSUtil.ToShort(ddlYear.SelectedValue), GMSUtil.ToShort(ddlMonth.SelectedValue), ref dsGMS);
                        ddlNewSalesPersonMasterName.DataSource = dsGMS;
                        ddlNewSalesPersonMasterName.DataBind();
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlNewSalesPersonMasterName, ddlNewSalesPersonMasterName.GetType(), "script1", "<script type=\"text/javascript\"> var ddlNewSalesPersonMasterName = '" + ddlNewSalesPersonMasterName.ClientID + "';</script>", false);
                    }


                    ddlNewCommRate.DataSource = lstSalesPerson;
                    ddlNewCommRate.DataBind();
                    ddlNewCommRate.SelectedValue = ddlNewSalesPersonMasterName.SelectedValue;
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlNewCommRate, ddlNewCommRate.GetType(), "script2", "<script type=\"text/javascript\"> var ddlNewCommRate = '" + ddlNewCommRate.ClientID + "';</script>", false);
                    Label lblCommRate2 = (Label)e.Item.FindControl("lblCommRate2");
                    if (ddlNewCommRate.SelectedIndex >= 0 && ddlNewCommRate.SelectedValue == ddlNewSalesPersonMasterName.SelectedValue)
                        lblCommRate2.Text = ddlNewCommRate.SelectedItem.Text;
                    else lblCommRate2.Text = "Nill";
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(lblCommRate2, lblCommRate2.GetType(), "script5", "<script type=\"text/javascript\"> var lblCommRate2 = '" + lblCommRate2.ClientID + "';</script>", false);

                    ddlNewGPQ.DataSource = lstSalesPerson;
                    ddlNewGPQ.DataBind();
                    ddlNewGPQ.SelectedValue = ddlNewSalesPersonMasterName.SelectedValue;
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlNewGPQ, ddlNewGPQ.GetType(), "script3", "<script type=\"text/javascript\"> var ddlNewGPQ = '" + ddlNewGPQ.ClientID + "';</script>", false);
                    Label lblGPQ2 = (Label)e.Item.FindControl("lblGPQ2");
                    if (ddlNewGPQ.SelectedIndex >= 0 && ddlNewGPQ.SelectedValue == ddlNewSalesPersonMasterName.SelectedValue)
                        lblGPQ2.Text = ddlNewGPQ.SelectedItem.Text;
                    else lblGPQ2.Text = "Nill";
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(lblGPQ2, lblGPQ2.GetType(), "script4", "<script type=\"text/javascript\"> var lblGPQ2 = '" + lblGPQ2.ClientID + "';</script>", false);
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region dgData_EditCommand
        protected void dgData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            73);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyHR"));

            this.dgData.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }
        #endregion

        #region dgData_CancelCommand
        protected void dgData_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgData_UpdateCommand
        protected void dgData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            73);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyHR"));

            TextBox txtEdit90Code = (TextBox)e.Item.FindControl("txtEdit90Code");
            TextBox txtEditExcess = (TextBox)e.Item.FindControl("txtEditExcess");
            TextBox txtEditCOG = (TextBox)e.Item.FindControl("txtEditCOG");
            //TextBox txtEditEntertainmentExpenses = (TextBox)e.Item.FindControl("txtEditEntertainmentExpenses");
            HtmlInputHidden hidSalesPersonMasterID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonMasterID");
            HtmlInputHidden hidSalesPersonMasterUserID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonMasterUserID");
            HtmlInputHidden hidYear = (HtmlInputHidden)e.Item.FindControl("hidYear");
            HtmlInputHidden hidMonth = (HtmlInputHidden)e.Item.FindControl("hidMonth");
            TextBox txtEditSalesAdjustment = (TextBox)e.Item.FindControl("txtEditSalesAdjustment");
            TextBox txtEditCommWithheld = (TextBox)e.Item.FindControl("txtEditCommWithheld");
            TextBox txtEditGPAdjustment = (TextBox)e.Item.FindControl("txtEditGPAdjustment");
            TextBox txtEditBadDebtWrittenOff = (TextBox)e.Item.FindControl("txtEditBadDebtWrittenOff");
            TextBox txtEditBadDebtRecovery = (TextBox)e.Item.FindControl("txtEditBadDebtRecovery");

            if (hidSalesPersonMasterID != null && hidYear != null && hidMonth != null &&
                txtEdit90Code != null && !string.IsNullOrEmpty(txtEdit90Code.Text) && txtEditExcess != null && !string.IsNullOrEmpty(txtEditExcess.Text) && txtEditCOG != null && !string.IsNullOrEmpty(txtEditCOG.Text) && 
                txtEditSalesAdjustment != null && !string.IsNullOrEmpty(txtEditSalesAdjustment.Text) && txtEditCommWithheld != null &&
                !string.IsNullOrEmpty(txtEditCommWithheld.Text) && txtEditGPAdjustment != null && !string.IsNullOrEmpty(txtEditGPAdjustment.Text) &&
                txtEditBadDebtWrittenOff != null && txtEditBadDebtRecovery != null)
            {

                GMSCore.Entity.SalesPersonRecord salesPersonRecord = new SalesPersonRecordActivity().RetrieveSalesPersonRecordBySalesPersonMasterIDYearMonth(short.Parse(hidSalesPersonMasterID.Value), short.Parse(hidSalesPersonMasterUserID.Value), GMSUtil.ToInt(hidYear.Value), GMSUtil.ToInt(hidMonth.Value));
                //salesPersonRecord.TotalEntertainmentExpenses = GMSUtil.ToDouble(txtEditEntertainmentExpenses.Text.Trim());
                salesPersonRecord.Excess = GMSUtil.ToDouble(txtEditExcess.Text.Trim());
                salesPersonRecord.CostOf90Code = GMSUtil.ToDouble(txtEdit90Code.Text.Trim());
                salesPersonRecord.CostOfCOG = GMSUtil.ToDouble(txtEditCOG.Text.Trim());
                salesPersonRecord.ModifiedBy = session.UserId;
                salesPersonRecord.ModifiedDate = DateTime.Now;
                salesPersonRecord.SalesAdjustment = GMSUtil.ToDouble(txtEditSalesAdjustment.Text.Trim());
                salesPersonRecord.CommWithheld = GMSUtil.ToDouble(txtEditCommWithheld.Text.Trim());
                salesPersonRecord.GPAdjustment = GMSUtil.ToDouble(txtEditGPAdjustment.Text.Trim());
                salesPersonRecord.BadDebtWrittenOff = GMSUtil.ToDouble(txtEditBadDebtWrittenOff.Text.Trim());
                salesPersonRecord.BadDebtRecovery = GMSUtil.ToDouble(txtEditBadDebtRecovery.Text.Trim());

                try
                {
                    ResultType result = new SalesPersonRecordActivity().UpdateSalesPersonRecord(ref salesPersonRecord, session);

                    switch (result)
                    {
                        case ResultType.Ok:
                            this.dgData.EditItemIndex = -1;
                            LoadData();
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

        #region dgData_CreateCommand
        protected void dgData_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                    return;
                }

                DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
                DropDownList ddlNewYear = (DropDownList)e.Item.FindControl("ddlNewYear");
                DropDownList ddlNewMonth = (DropDownList)e.Item.FindControl("ddlNewMonth");
                TextBox txtNew90Code = (TextBox)e.Item.FindControl("txtNew90Code");
                TextBox txtNewExcess = (TextBox)e.Item.FindControl("txtNewExcess");
                TextBox txtNewCOG = (TextBox)e.Item.FindControl("txtNewCOG");
                //TextBox txtNewEntertainmentExpenses = (TextBox)e.Item.FindControl("txtNewEntertainmentExpenses");
                TextBox txtNewSalesAdjustment = (TextBox)e.Item.FindControl("txtNewSalesAdjustment");
                TextBox txtNewCommWithheld = (TextBox)e.Item.FindControl("txtNewCommWithheld");
                TextBox txtNewGPAdjustment = (TextBox)e.Item.FindControl("txtNewGPAdjustment");
                TextBox txtNewBadDebtWrittenOff = (TextBox)e.Item.FindControl("txtNewBadDebtWrittenOff");
                TextBox txtNewBadDebtRecovery = (TextBox)e.Item.FindControl("txtNewBadDebtRecovery");

                if (ddlNewSalesPersonMasterName != null && ddlNewYear != null && ddlNewMonth != null &&
                    txtNew90Code != null && !string.IsNullOrEmpty(txtNew90Code.Text) && txtNewExcess != null && !string.IsNullOrEmpty(txtNewExcess.Text) && txtNewCOG != null && !string.IsNullOrEmpty(txtNewCOG.Text) &&
                    txtNewSalesAdjustment != null && !string.IsNullOrEmpty(txtNewSalesAdjustment.Text) && txtNewCommWithheld != null &&
                    !string.IsNullOrEmpty(txtNewCommWithheld.Text) && txtNewGPAdjustment != null && !string.IsNullOrEmpty(txtNewGPAdjustment.Text) &&
                    txtNewBadDebtWrittenOff != null && txtNewBadDebtRecovery != null)
                {
                    try
                    {
                        SalesPersonMaster sMaster = SalesPersonMaster.RetrieveByKey(short.Parse(ddlNewSalesPersonMasterName.SelectedValue));
                        if (sMaster == null || sMaster.CommissionRate == null || sMaster.CommissionRate <= 0)
                        {
                            this.PageMsgPanel.ShowMessage("Processing error of type : Commission Rate for this salesman has not been set yet.", MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }

                        //check if newly inserted SalesPersonRecord already exist
                        GMSCore.Entity.SalesPersonRecord existingSalesPersonRecord = new SalesPersonRecordActivity().RetrieveSalesPersonRecordByCoyIDSPYearMonth(short.Parse(ddlNewSalesPersonMasterName.SelectedValue), GMSUtil.ToInt(ddlNewYear.SelectedValue), GMSUtil.ToInt(ddlNewMonth.SelectedValue));
                        if (existingSalesPersonRecord != null)
                        {
                            this.PageMsgPanel.ShowMessage("Processing error of type : Record for this year month already exists.", MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }

                        if (sMaster == null || sMaster.GPQ == null || sMaster.GPQ < 0)
                        {
                            this.PageMsgPanel.ShowMessage("Processing error of type : GPQ for this salesman has not been set yet.", MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }

                        SalesPersonRecord salesPersonRecord = new SalesPersonRecord();
                        salesPersonRecord.CoyID = session.CompanyId;
                        salesPersonRecord.SalesPersonMasterID = short.Parse(ddlNewSalesPersonMasterName.SelectedValue);
                        salesPersonRecord.SalesPersonMasterUserID = short.Parse(ddlNewSalesPersonMasterName.SelectedValue);
                        salesPersonRecord.GroupType = "N/A";
                        salesPersonRecord.TbYear = GMSUtil.ToShort(ddlNewYear.SelectedValue);
                        salesPersonRecord.TbMonth = GMSUtil.ToShort(ddlNewMonth.SelectedValue);
                        salesPersonRecord.CostOf90Code = GMSUtil.ToDouble(txtNew90Code.Text.Trim());
                        salesPersonRecord.Excess = GMSUtil.ToDouble(txtNewExcess.Text.Trim());
                        salesPersonRecord.CostOfCOG = GMSUtil.ToDouble(txtNewCOG.Text.Trim());
                        salesPersonRecord.TotalEntertainmentExpenses = 0;
                        salesPersonRecord.GPQ = sMaster.GPQ;
                        salesPersonRecord.CommissionRate = sMaster.CommissionRate;
                        salesPersonRecord.TotalGP = 0;
                        salesPersonRecord.CreatedBy = session.UserId;
                        salesPersonRecord.CreatedDate = DateTime.Now;
                        salesPersonRecord.SalesAdjustment = GMSUtil.ToDouble(txtNewSalesAdjustment.Text.Trim());
                        salesPersonRecord.CommWithheld = GMSUtil.ToDouble(txtNewCommWithheld.Text.Trim());
                        salesPersonRecord.GPAdjustment = GMSUtil.ToDouble(txtNewGPAdjustment.Text.Trim());
                        salesPersonRecord.BadDebtWrittenOff = GMSUtil.ToDouble(txtNewBadDebtWrittenOff.Text.Trim());
                        salesPersonRecord.BadDebtRecovery = GMSUtil.ToDouble(txtNewBadDebtRecovery.Text.Trim());

                        ResultType result = new SalesPersonRecordActivity().CreateSalesPersonRecord(ref salesPersonRecord, session);
                        switch (result)
                        {
                            case ResultType.Ok:
                                LoadData();
                                break;
                            default:
                                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                return;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage("Processing error : " + ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgData_DeleteCommand
        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                73);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("CompanyHR"));

                HtmlInputHidden hidSalesPersonMasterID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonMasterID");
                HtmlInputHidden hidSalesPersonMasterUserID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonMasterUserID");
                HtmlInputHidden hidYear = (HtmlInputHidden)e.Item.FindControl("hidYear");
                HtmlInputHidden hidMonth = (HtmlInputHidden)e.Item.FindControl("hidMonth");

                if (hidSalesPersonMasterID != null && hidYear != null && hidMonth != null)
                {
                    SalesPersonRecordActivity sActivity = new SalesPersonRecordActivity();

                    try
                    {
                        ResultType result = sActivity.DeleteSalesPersonRecord(short.Parse(hidSalesPersonMasterID.Value), short.Parse(hidSalesPersonMasterUserID.Value), GMSUtil.ToInt(hidYear.Value), GMSUtil.ToInt(hidMonth.Value), session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgData.EditItemIndex = -1;
                                this.dgData.CurrentPageIndex = 0;
                                LoadData();
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
                            LoadData();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        LoadData();
                        return;
                    }
                }
            }
        }
        #endregion

        #region GenerateReport
        protected void GenerateReport(object sender, EventArgs e)
        {
            string selectedReport = ddlReport.SelectedValue;
            ClientScript.RegisterStartupScript(typeof(string), "Report1",
                string.Format("jsOpenOperationalReport('Reports/Report/SalesReportViewer.aspx?REPORT={0}&&TRNNO=1&&REPORTID=-2');",
                                    selectedReport)
                                    , true);

            LoadData();
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
        }
        #endregion

        #region PopulateDDL
        private void PopulateDDL()
        {
            ddlYear.ClearSelection();
            ddlYear.Items.Clear();
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 3; i <= currentYear + 1; i++)
            {
                ddlYear.Items.Add(i.ToString());
            }
            ddlYear.Items.Insert(0, new ListItem("All", "0"));
        }
        #endregion


        protected string IsVerifiedSalesAmount(short year, short month) {

            bool IsVerified = true;
            string IssuesDetected = "";

            if (year == 0 || month == 0)
            {
                return "";
                //year = short.Parse(DateTime.Now.Year.ToString());
                //month = short.Parse(DateTime.Now.Month.ToString());
            }
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //string datefrom = firstDayOfMonth.ToString();
            //string dateto = lastDayOfMonth.ToString();


            LogSession session = base.GetSessionInfo();
            DataSet ds = new DataSet();
            DataSet dsGMS = new DataSet();
            string accountingSystem = (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S") ? "SAP" : "A21";
            if (session.TBType == "N" || session.TBType == "P") // read from local A21 or remote A21
            {
                GMSWebService.GMSWebService ws = new GMSWebService.GMSWebService();
                if (session.WebServiceAddress != null && session.WebServiceAddress.Trim() != "")
                    ws.Url = session.WebServiceAddress.Trim();
                else
                    ws.Url = "http://localhost/GMSWebService/GMSWebService.asmx";

                new GMSGeneralDALC().RetrieveSalesDetailSummary(session.CompanyId, firstDayOfMonth, lastDayOfMonth, ref dsGMS);

                if (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S")
                {
                    SAPOperation sapOp = new SAPOperation(session.SAPURI.ToString(), session.SAPKEY.ToString(), session.SAPDB.ToString());

                    //string query = "SELECT YEAR(\"DocDate\"), MONTH(\"DocDate\"), ROUND(SUM(\"SUBTOTAL\"),0) FROM ( SELECT T0.\"DocDate\", CASE WHEN T0.\"DocRate\" != 1 THEN(T1.\"TotalFrgn\" * T0.\"DocRate\")  ELSE T1.\"LineTotal\" END AS \"SUBTOTAL\" FROM OINV T0 " +
                    //"INNER JOIN INV1 T1 ON TO_NVARCHAR(T0.\"DocEntry\") = TO_NVARCHAR(T1.\"DocEntry\") AND T0.\"CardCode\" = T1.\"BaseCard\" " +
                    //"INNER JOIN NNM1 T10 ON T0.\"Series\" = T10.\"Series\" AND T10.\"ObjectCode\" in ('13', '14') " +
                    //"WHERE T0.\"Series\" in ('4', '75') AND T0.\"CANCELED\" = 'N' " +
                    //"AND T0.\"DocType\" != 'S' " +
                    //"UNION ALL " +
                    //"SELECT T0.\"DocDate\", CASE WHEN T0.\"DocRate\" != 1 THEN((T1.\"TotalFrgn\" * T0.\"DocRate\") * -1) ELSE(T1.\"LineTotal\" * -1) END as \"SubTotal\" " +
                    //"FROM ORIN T0 " +
                    //"INNER JOIN RIN1 T1 ON TO_NVARCHAR(T0.\"DocEntry\") = TO_NVARCHAR(T1.\"DocEntry\") AND T0.\"CardCode\" = T1.\"BaseCard\" " +
                    //"INNER JOIN NNM1 T10 ON T0.\"Series\" = T10.\"Series\" AND T10.\"ObjectCode\" in ('13', '14') " +
                    //"WHERE T0.\"Series\" in ('5', '78') AND T0.\"CANCELED\" = 'N'  AND T0.\"DocType\" != 'S' )  " +
                    //"WHERE TO_VARCHAR(\"DocDate\", 'yyyymmdd') >= TO_VARCHAR('" + firstDayOfMonth.ToString("yyyy-MM-dd") + "', 'yyyymmdd') AND TO_VARCHAR(\"DocDate\", 'yyyymmdd') <= TO_VARCHAR('" + lastDayOfMonth.ToString("yyyy-MM-dd") + "', 'yyyymmdd') " +
                    //"GROUP BY YEAR(\"DocDate\"), MONTH(\"DocDate\") " +
                    //"ORDER BY YEAR(\"DocDate\"), MONTH(\"DocDate\");";


                    string query = "SELECT YEAR(\"Date\"), MONTH(\"Date\"), SUM(\"SUB\" * \"DocRate\"), SUM(\"SUB\" * \"Disc\"), SUM(\"TAX\") " +
                    "FROM( " +
                    "SELECT T0.\"DocDate\" as \"Date\", T0.\"Series\", T0.\"DocRate\", (1 - (T0.\"DiscPrcnt\" / 100)) as \"Disc\", " +
                    "CASE WHEN T0.\"DocRate\" != 1 THEN T1.\"TotalFrgn\" ELSE T1.\"LineTotal\" END AS \"SUB\", " +
                    "CASE WHEN T0.\"DocRate\" != 1 THEN T1.\"VatSumFrgn\" ELSE T1.\"VatSum\" END AS \"TAX\" " +
                    "FROM OINV T0 " +
                    "INNER JOIN INV1 T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" " +
                    "WHERE T0.\"CANCELED\" = 'N' AND T0.\"DocType\" != 'S' " +
                    "UNION ALL " +
                    "SELECT T0.\"DocDate\",T0.\"Series\", T0.\"DocRate\", (1 - (T0.\"DiscPrcnt\" / 100)), " +
                    "CASE WHEN T0.\"DocRate\" != 1 THEN T1.\"TotalFrgn\" * -1 ELSE T1.\"LineTotal\" * -1 END, " +
                    "CASE WHEN T0.\"DocRate\" != 1 THEN T1.\"VatSumFrgn\" ELSE T1.\"VatSum\" END " +
                    "FROM ORIN T0 INNER JOIN RIN1 T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" " +
                    "WHERE T0.\"CANCELED\" = 'N' AND T0.\"DocType\" != 'S' " +
                    ") T2 " +
                    "INNER JOIN NNM1 T10 ON T2.\"Series\" = T10.\"Series\" " +
                    "WHERE TO_VARCHAR(\"Date\", 'yyyymmdd') >= TO_VARCHAR('" + firstDayOfMonth.ToString("yyyy-MM-dd") + "', 'yyyymmdd') AND TO_VARCHAR(\"Date\", 'yyyymmdd') <= TO_VARCHAR('" + lastDayOfMonth.ToString("yyyy-MM-dd") + "', 'yyyymmdd') " +
                    "AND(T10.\"SeriesName\" LIKE 'INV%' OR T10.\"SeriesName\" LIKE 'CN%') AND T10.\"SeriesName\" NOT LIKE '%OB' AND T10.\"SeriesName\" NOT LIKE '%-NT' " +
                    "GROUP BY YEAR(\"Date\"), MONTH(\"Date\"); ";


                    ds = sapOp.GET_SAP_QueryData(session.CompanyId, query,
                        "tbYear", "tbMonth", "SubTotal", "HeaderSubTotal", "TaxAmount", "Field6", "Field7", "Field8", "Field9", "Field10", "Field11", "Field12", "Field13", "Field14", "Field15", "Field16", "Field17", "Field18", "Field19", "Field20",
                        "Field21", "Field22", "Field23", "Field24", "Field25", "Field26", "Field27", "Field28", "Field29", "Field30");
                }
                else
                {
                    ds = ws.GetInvoiceTotal(session.CompanyId, firstDayOfMonth, lastDayOfMonth);
                }

                foreach (DataRow dr in dsGMS.Tables[0].Rows)
                {
                    foreach (DataRow dr1 in ds.Tables[0].Rows)
                    {
                        if (int.Parse(dr1["tbYear"].ToString()) == int.Parse(dr["tbYear"].ToString()) && int.Parse(dr1["tbMonth"].ToString()) == int.Parse(dr["tbMonth"].ToString()))
                        {
                            if (decimal.Parse(dr1["SubTotal"].ToString()) != decimal.Parse(dr["SubTotal"].ToString()) && ((decimal.Parse(dr1["SubTotal"].ToString()) - decimal.Parse(dr["SubTotal"].ToString())) > 10 || (decimal.Parse(dr["SubTotal"].ToString()) - decimal.Parse(dr1["SubTotal"].ToString())) > 10))
                            {
                                return "Detail Amount";
                                
                            }

                            if (session.StatusType.ToString() == "L" || session.StatusType.ToString() == "S")
                            {
                                if (Math.Round(decimal.Parse(dr1["HeaderSubTotal"].ToString())) != decimal.Parse(dr["HeaderSubTotal"].ToString()) && ((Math.Round(decimal.Parse(dr1["HeaderSubTotal"].ToString())) - decimal.Parse(dr["HeaderSubTotal"].ToString())) > 10 || (decimal.Parse(dr["HeaderSubTotal"].ToString()) - Math.Round(decimal.Parse(dr1["HeaderSubTotal"].ToString()))) > 10))
                                {
                                    return "Sales Amount";
                                }

                                if (decimal.Parse(dr1["TaxAmount"].ToString()) != decimal.Parse(dr["TaxAmount"].ToString()) && ((decimal.Parse(dr1["TaxAmount"].ToString()) - decimal.Parse(dr["TaxAmount"].ToString())) > 5 || (decimal.Parse(dr["TaxAmount"].ToString()) - decimal.Parse(dr1["TaxAmount"].ToString())) > 5))
                                {
                                    return "Tax Amount";
                                }

                                if (decimal.Parse(dr1["TaxAmount"].ToString()) != decimal.Parse(dr["TaxTotal"].ToString()) && (Math.Abs(decimal.Parse(dr1["TaxAmount"].ToString()) - decimal.Parse(dr["TaxTotal"].ToString())) > 5))
                                {
                                    return "Tax Total";
                                }
                            }
                        }
                    }
                }
            }
            return IssuesDetected;
        }
    }
}
