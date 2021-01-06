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

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System.Data.SqlClient;

namespace GMSWeb.Sales.Commission
{
    public partial class EntertainmentExpense : GMSBasePage
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
                                                                            79);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            if (!Page.IsPostBack)
            {
                //preload
                dgData.CurrentPageIndex = 0;
                LoadData();
            }

            string javaScript =
            @"
            <script language=""javascript"" type=""text/javascript"" src=""/GMS/scripts/popcalendar.js""></script>

            <script type=""text/javascript"">
            function SelectOthers(chkbox)
	        {
	            var prefix = chkbox.id.substring(0,chkbox.id.lastIndexOf(""_"")+1);
	            if (chkbox.checked)
	            {
	                document.getElementById(prefix+""spanSalesPersonMasterName"").style.visibility = 'hidden';
	                document.getElementById(prefix+""spanArea"").style.visibility = 'hidden';
	            } else
	            {
	                document.getElementById(prefix+""spanSalesPersonMasterName"").style.visibility = 'visible';
	                document.getElementById(prefix+""spanArea"").style.visibility = 'visible';
	            }
	        } 
        	 
            </script>
            "; 
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            IList<GMSCore.Entity.EntertainmentExpense> lstEE = null;
            short companyID = session.CompanyId;
            string name = string.IsNullOrEmpty(txtSearchName.Text) ? "%" : "%" + txtSearchName.Text.Trim() + "%";
            DateTime tDateFrom = (GMSUtil.ToDate(txtSearchDateFrom.Text) == GMSCore.GMSCoreBase.DEFAULT_NO_DATE) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : GMSUtil.ToDate(txtSearchDateFrom.Text.ToString());
            DateTime tDateTo = (GMSUtil.ToDate(txtSearchDateTo.Text) == GMSCore.GMSCoreBase.DEFAULT_NO_DATE) ? DateTime.Now : GMSUtil.ToDate(txtSearchDateTo.Text.ToString());
            if (string.IsNullOrEmpty(txtSearchDateFrom.Text)) txtSearchDateFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
            if (string.IsNullOrEmpty(txtSearchDateTo.Text)) txtSearchDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            string area = ddlSearchArea.SelectedValue;
            bool others = chkSearchOthers.Checked;
            try
            {
                lstEE = new SystemDataActivity().RetrieveEntertainmentExpenseListByNameByAreaByPaymentDateByOthers(companyID, name, area, tDateFrom, tDateTo, others);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lstEE != null && lstEE.Count > 0)
            {
                if (endIndex < lstEE.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstEE.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstEE.Count.ToString() + " " + "of" + " " + lstEE.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = lstEE;
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

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                GMSCore.Entity.EntertainmentExpense ee = (GMSCore.Entity.EntertainmentExpense)e.Item.DataItem;
                DropDownList ddlEditSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlEditSalesPersonMasterName");
                if (ddlEditSalesPersonMasterName != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in currency dropdown list
                    IList<GMSCore.Entity.SalesPersonMaster> lstSalesPerson = null;
                    lstSalesPerson = sDataActivity.RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterName(session.CompanyId);
                    ddlEditSalesPersonMasterName.DataSource = lstSalesPerson;
                    ddlEditSalesPersonMasterName.DataBind();
                    ddlEditSalesPersonMasterName.SelectedValue = ee.SalesPersonMasterID.ToString();
                }

                DropDownList ddlEditArea = (DropDownList)e.Item.FindControl("ddlEditArea");
                if (ddlEditArea != null)
                {
                    ddlEditArea.SelectedValue = ee.Area;
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
                if (ddlNewSalesPersonMasterName != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in currency dropdown list
                    IList<GMSCore.Entity.SalesPersonMaster> lstSalesPerson = null;
                    lstSalesPerson = sDataActivity.RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterName(session.CompanyId);
                    ddlNewSalesPersonMasterName.DataSource = lstSalesPerson;
                    ddlNewSalesPersonMasterName.DataBind();
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

        #region dgData_CreateCommand
        protected void dgData_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                80);
                if (uAccess == null)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertmsg", "<script type=\"text/javascript\"> alert(\"" + "You don't have access." + "\");</script>", false);
                    return;
                }

                DropDownList ddlNewSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlNewSalesPersonMasterName");
                DropDownList ddlNewArea = (DropDownList)e.Item.FindControl("ddlNewArea");
                TextBox newPaymentDate = (TextBox)e.Item.FindControl("newPaymentDate");
                TextBox txtNewAmount = (TextBox)e.Item.FindControl("txtNewAmount");
                TextBox txtNewRemark = (TextBox)e.Item.FindControl("txtNewRemark");
                CheckBox chkNewOthers = (CheckBox)e.Item.FindControl("chkNewOthers");

                if (chkNewOthers != null && ddlNewArea != null && ddlNewSalesPersonMasterName != null && newPaymentDate != null && !string.IsNullOrEmpty(newPaymentDate.Text) &&
                    txtNewAmount != null && !string.IsNullOrEmpty(txtNewAmount.Text) && txtNewRemark != null)
                {
                    try
                    {
                        GMSCore.Entity.EntertainmentExpense ee = new GMSCore.Entity.EntertainmentExpense();
                        ee.CoyID = session.CompanyId;
                        ee.Others = chkNewOthers.Checked;
                        if (!chkNewOthers.Checked)
                        {
                            ee.SalesPersonMasterID = short.Parse(ddlNewSalesPersonMasterName.SelectedValue);
                            ee.Area = ddlNewArea.SelectedValue;
                        }
                        ee.Date = GMSUtil.ToDate(newPaymentDate.Text.Trim());
                        ee.Amount = decimal.Parse(txtNewAmount.Text.Trim());
                        ee.Remark = txtNewRemark.Text.Trim();
                        ee.CreatedBy = session.UserId;
                        ee.CreatedDate = DateTime.Now;

                        ee.Save();
                        chkSearchOthers.Checked = chkNewOthers.Checked;
                        LoadData();
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

        #region dgData_UpdateCommand
        protected void dgData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Sales"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            80);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Sales"));

            DropDownList ddlEditSalesPersonMasterName = (DropDownList)e.Item.FindControl("ddlEditSalesPersonMasterName");
            TextBox editPaymentDate = (TextBox)e.Item.FindControl("editPaymentDate");
            TextBox txtEditAmount = (TextBox)e.Item.FindControl("txtEditAmount");
            TextBox txtEditRemark = (TextBox)e.Item.FindControl("txtEditRemark");
            DropDownList ddlEditArea = (DropDownList)e.Item.FindControl("ddlEditArea");
            CheckBox chkEditOthers = (CheckBox)e.Item.FindControl("chkEditOthers");
            HtmlInputHidden hidTransactionID = (HtmlInputHidden)e.Item.FindControl("hidTransactionID");

            if (editPaymentDate != null && !string.IsNullOrEmpty(editPaymentDate.Text) && hidTransactionID != null &&
                txtEditAmount != null && !string.IsNullOrEmpty(txtEditAmount.Text) && ddlEditSalesPersonMasterName != null && ddlEditArea != null && chkEditOthers != null && txtEditRemark != null)
            {

                GMSCore.Entity.EntertainmentExpense ee = GMSCore.Entity.EntertainmentExpense.RetrieveByKey(int.Parse(hidTransactionID.Value));
                if (chkEditOthers.Checked)
                {
                    ee.SalesPersonMasterID = 0;
                    ee.Area = "";
                }
                else
                {
                    ee.SalesPersonMasterID = short.Parse(ddlEditSalesPersonMasterName.SelectedValue);
                    ee.Area = ddlEditArea.SelectedValue;
                }
                ee.Date = GMSUtil.ToDate(editPaymentDate.Text.Trim());
                ee.Amount = decimal.Parse(txtEditAmount.Text.Trim());
                ee.Remark = txtEditRemark.Text.Trim();
                ee.Others = chkEditOthers.Checked;
                ee.ModifiedBy = session.UserId;
                ee.ModifiedDate = DateTime.Now;

                try
                {
                    ee.Save();
                    this.dgData.EditItemIndex = -1;
                    chkSearchOthers.Checked = chkEditOthers.Checked;
                    LoadData();
                }
                catch (Exception ex)
                {
                    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
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
                    Response.Redirect(base.SessionTimeOutPage("Sales"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                80);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("Sales"));

                HtmlInputHidden hidTransactionID = (HtmlInputHidden)e.Item.FindControl("hidTransactionID");

                if (hidTransactionID != null)
                {
                    try
                    {
                        GMSCore.Entity.EntertainmentExpense ee = GMSCore.Entity.EntertainmentExpense.RetrieveByKey(int.Parse(hidTransactionID.Value));
                        ee.Delete();
                        ee.Resync();
                        this.dgData.EditItemIndex = -1;
                        this.dgData.CurrentPageIndex = 0;
                        LoadData();
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

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgData.CurrentPageIndex = 0;
            LoadData();
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
