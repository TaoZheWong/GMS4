using System;
using System.Data;
using System.Configuration;
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
using System.Collections;

namespace GMSWeb.Sales.Commission
{
    public partial class EntertainmentExpenses : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            72);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                //preload
                LoadData();
            }
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            IList<GMSCore.Entity.SalesPersonRecord> lstSalesPersonRecord = null;
            try
            {
                lstSalesPersonRecord = new SystemDataActivity().RetrieveAllSalesPersonRecordListByCompanyIDSortByYearMonthSalesPersonMasterID(session.CompanyId);
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
            this.dgData.DataSource = lstSalesPersonRecord;
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

                    ddlNewGPQ.DataSource =lstSalesPerson;
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
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            73);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

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
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            73);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            TextBox txtEdit90Code = (TextBox)e.Item.FindControl("txtEdit90Code");
            TextBox txtEditCOG = (TextBox)e.Item.FindControl("txtEditCOG");
            TextBox txtEditEntertainmentExpenses = (TextBox)e.Item.FindControl("txtEditEntertainmentExpenses");
            HtmlInputHidden hidSalesPersonMasterID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonMasterID");
            HtmlInputHidden hidYear = (HtmlInputHidden)e.Item.FindControl("hidYear");
            HtmlInputHidden hidMonth = (HtmlInputHidden)e.Item.FindControl("hidMonth");

            if (txtEditEntertainmentExpenses != null && !string.IsNullOrEmpty(txtEditEntertainmentExpenses.Text) && hidSalesPersonMasterID != null && hidYear != null && hidMonth != null &&
                txtEdit90Code != null && !string.IsNullOrEmpty(txtEdit90Code.Text) && txtEditCOG != null && !string.IsNullOrEmpty(txtEditCOG.Text))
            {

                GMSCore.Entity.SalesPersonRecord salesPersonRecord = new SalesPersonRecordActivity().RetrieveSalesPersonRecordBySalesPersonMasterIDYearMonth(short.Parse(hidSalesPersonMasterID.Value), GMSUtil.ToInt(hidYear.Value), GMSUtil.ToInt(hidMonth.Value));
                salesPersonRecord.TotalEntertainmentExpenses = GMSUtil.ToDouble(txtEditEntertainmentExpenses.Text.Trim());
                salesPersonRecord.CostOf90Code = GMSUtil.ToDouble(txtEdit90Code.Text.Trim());
                salesPersonRecord.CostOfCOG = GMSUtil.ToDouble(txtEditCOG.Text.Trim());
                salesPersonRecord.ModifiedBy = session.UserId;
                salesPersonRecord.ModifiedDate = DateTime.Now;

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
                    Response.Redirect("../../SessionTimeout.htm");
                    return;
                }

                DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
                DropDownList ddlNewYear = (DropDownList)e.Item.FindControl("ddlNewYear");
                DropDownList ddlNewMonth = (DropDownList)e.Item.FindControl("ddlNewMonth");
                TextBox txtNew90Code = (TextBox)e.Item.FindControl("txtNew90Code");
                TextBox txtNewCOG = (TextBox)e.Item.FindControl("txtNewCOG");
                TextBox txtNewEntertainmentExpenses = (TextBox)e.Item.FindControl("txtNewEntertainmentExpenses");

                if (ddlNewSalesPersonMasterName != null && ddlNewYear != null && ddlNewMonth != null && txtNewEntertainmentExpenses != null && !string.IsNullOrEmpty(txtNewEntertainmentExpenses.Text) && 
                    txtNew90Code != null && !string.IsNullOrEmpty(txtNew90Code.Text) && txtNewCOG != null && !string.IsNullOrEmpty(txtNewCOG.Text))
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
                        GMSCore.Entity.SalesPersonRecord existingSalesPersonRecord = new SalesPersonRecordActivity().RetrieveSalesPersonRecordBySalesPersonMasterIDYearMonth(short.Parse(ddlNewSalesPersonMasterName.SelectedValue), GMSUtil.ToInt(ddlNewYear.SelectedValue), GMSUtil.ToInt(ddlNewMonth.SelectedValue));
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

                        GMSCore.Entity.SalesPersonRecord salesPersonRecord = new GMSCore.Entity.SalesPersonRecord();
                        salesPersonRecord.CoyID = session.CompanyId;
                        salesPersonRecord.SalesPersonMasterID = short.Parse(ddlNewSalesPersonMasterName.SelectedValue);
                        salesPersonRecord.TbYear = GMSUtil.ToShort(ddlNewYear.SelectedValue);
                        salesPersonRecord.TbMonth = GMSUtil.ToShort(ddlNewMonth.SelectedValue);
                        salesPersonRecord.CostOf90Code = GMSUtil.ToDouble(txtNew90Code.Text.Trim());
                        salesPersonRecord.CostOfCOG = GMSUtil.ToDouble(txtNewCOG.Text.Trim());
                        salesPersonRecord.TotalEntertainmentExpenses = GMSUtil.ToDouble(txtNewEntertainmentExpenses.Text.Trim());
                        salesPersonRecord.GPQ = sMaster.GPQ;
                        salesPersonRecord.CommissionRate = sMaster.CommissionRate;
                        salesPersonRecord.TotalGP = 0;
                        salesPersonRecord.CreatedBy = session.UserId;
                        salesPersonRecord.CreatedDate = DateTime.Now;

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
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
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
                    Response.Redirect("../../SessionTimeout.htm");
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                73);
                if (uAccess == null)
                    Response.Redirect("../../Unauthorized.htm");

                HtmlInputHidden hidSalesPersonMasterID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonMasterID");
                HtmlInputHidden hidYear = (HtmlInputHidden)e.Item.FindControl("hidYear");
                HtmlInputHidden hidMonth = (HtmlInputHidden)e.Item.FindControl("hidMonth");

                if (hidSalesPersonMasterID != null && hidYear != null && hidMonth != null)
                {
                    SalesPersonRecordActivity sActivity = new SalesPersonRecordActivity();

                    try
                    {
                        ResultType result = sActivity.DeleteSalesPersonRecord(short.Parse(hidSalesPersonMasterID.Value), GMSUtil.ToInt(hidYear.Value), GMSUtil.ToInt(hidMonth.Value), session);

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
                string.Format("jsOpenOperationalReport('Finance/BankFacilities/ReportViewer.aspx?REPORT={0}&&TRNNO=1&&REPORTID=-2');",
                                    selectedReport)
                                    , true);

            LoadData();
        }
        #endregion
    }
}
