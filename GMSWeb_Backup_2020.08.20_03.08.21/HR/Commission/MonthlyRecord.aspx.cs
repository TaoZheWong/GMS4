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
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>
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

                if (ddlNewSalesPersonMasterName != null && ddlNewGPQ != null && ddlNewCommRate != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in currency dropdown list
                    IList<GMSCore.Entity.SalesPersonMaster> lstSalesPerson = null;
                    lstSalesPerson = sDataActivity.RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterName(session.CompanyId);
                    ddlNewSalesPersonMasterName.DataSource = lstSalesPerson;
                    ddlNewSalesPersonMasterName.DataBind();
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(ddlNewSalesPersonMasterName, ddlNewSalesPersonMasterName.GetType(), "script1", "<script type=\"text/javascript\"> var ddlNewSalesPersonMasterName = '" + ddlNewSalesPersonMasterName.ClientID + "';</script>", false);

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

                DropDownList ddlNewYear = (DropDownList)e.Item.FindControl("ddlNewYear");
                // load year ddl
                DataTable dtt1 = new DataTable();
                dtt1.Columns.Add("Year", typeof(string));

                for (int i = -4; i < 5; i++)
                {
                    DataRow dr1 = dtt1.NewRow();
                    dr1["Year"] = DateTime.Now.Year + i;

                    dtt1.Rows.Add(dr1);
                }
                ddlNewYear.DataSource = dtt1;
                ddlNewYear.DataBind();
                ddlNewYear.SelectedValue = DateTime.Now.Year.ToString();
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
    }
}
