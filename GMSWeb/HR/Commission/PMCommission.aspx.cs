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

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System.Data.SqlClient;

namespace GMSWeb.Sales.Commission
{
    public partial class PMCommission : GMSBasePage
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
                                                                            81);
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
            IList<GMSCore.Entity.ProductManagerMaster> lstProductManagerMaster = null;
            try
            {
                lstProductManagerMaster = new SystemDataActivity().RetrieveAllProductManagerMasterListByCompanyIDSortByEffectiveDate(session.CompanyId);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lstProductManagerMaster != null && lstProductManagerMaster.Count > 0)
            {
                if (endIndex < lstProductManagerMaster.Count)
                    this.lblSearchSummary.Text = "Result List" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstProductManagerMaster.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Result List" + " " + startIndex.ToString() + " - " +
                        lstProductManagerMaster.Count.ToString() + " " + "of" + " " + lstProductManagerMaster.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = lstProductManagerMaster;
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
                GMSCore.Entity.ProductManagerMaster ee = (GMSCore.Entity.ProductManagerMaster)e.Item.DataItem;
                DropDownList ddlEditProductGroupManagerName = (DropDownList)e.Item.FindControl("ddlEditProductGroupManagerName");
                if (ddlEditProductGroupManagerName != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in currency dropdown list
                    IList<GMSCore.Entity.ProductGroupManager> lstProductGroupManager = null;
                    lstProductGroupManager = sDataActivity.RetrieveAllProductGroupManagerListByCompanyIDSortByProductGroupManagerName(session.CompanyId);
                    ddlEditProductGroupManagerName.DataSource = lstProductGroupManager;
                    ddlEditProductGroupManagerName.DataBind();
                    ddlEditProductGroupManagerName.SelectedValue = ee.ProductGroupManagerID.ToString();
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewProductGroupManagerName = (DropDownList)e.Item.FindControl("ddlNewProductGroupManagerName");
                if (ddlNewProductGroupManagerName != null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in currency dropdown list
                    IList<GMSCore.Entity.ProductGroupManager> lstProductGroupManager = null;
                    lstProductGroupManager = sDataActivity.RetrieveAllProductGroupManagerListByCompanyIDSortByProductGroupManagerName(session.CompanyId);
                    ddlNewProductGroupManagerName.DataSource = lstProductGroupManager;
                    ddlNewProductGroupManagerName.DataBind();
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
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            82);
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
                                                                            82);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            DropDownList ddlEditProductGroupManagerName = (DropDownList)e.Item.FindControl("ddlEditProductGroupManagerName");
            TextBox txtEditMonthlyCommissionRate = (TextBox)e.Item.FindControl("txtEditMonthlyCommissionRate");
            TextBox txtEditQuarterlyCommissionRate = (TextBox)e.Item.FindControl("txtEditQuarterlyCommissionRate");
            TextBox txtEditEffectiveDate = (TextBox)e.Item.FindControl("txtEditEffectiveDate");
            HtmlInputHidden hidProductGroupManagerID = (HtmlInputHidden)e.Item.FindControl("hidProductGroupManagerID");
            HtmlInputHidden hidEfffectiveDate = (HtmlInputHidden)e.Item.FindControl("hidEfffectiveDate");

            if (txtEditMonthlyCommissionRate != null && !string.IsNullOrEmpty(txtEditMonthlyCommissionRate.Text) && hidProductGroupManagerID != null &&
                txtEditQuarterlyCommissionRate != null && !string.IsNullOrEmpty(txtEditQuarterlyCommissionRate.Text) && ddlEditProductGroupManagerName != null &&
                hidEfffectiveDate != null && txtEditEffectiveDate != null && !string.IsNullOrEmpty(txtEditEffectiveDate.Text))
            {
                try
                {
                    GMSCore.Entity.ProductManagerMaster pmm = GMSCore.Entity.ProductManagerMaster.RetrieveByKey(session.CompanyId, short.Parse(hidProductGroupManagerID.Value), GMSUtil.ToDate(hidEfffectiveDate.Value));
                    short createdBy = pmm.CreatedBy;
                    DateTime createdDate = pmm.CreatedDate;
                    pmm.Delete();
                    pmm.Resync();

                    pmm = new ProductManagerMaster();
                    pmm.CoyID = session.CompanyId;
                    pmm.ProductGroupManagerID = short.Parse(ddlEditProductGroupManagerName.SelectedValue);
                    pmm.MonthlyCommissionRate = decimal.Parse(txtEditMonthlyCommissionRate.Text);
                    pmm.QuarterlyCommissionRate = decimal.Parse(txtEditQuarterlyCommissionRate.Text);
                    pmm.EffectiveDate = GMSUtil.ToDate(txtEditEffectiveDate.Text);
                    pmm.CreatedBy = createdBy;
                    pmm.CreatedDate = createdDate;
                    pmm.ModifiedBy = session.UserId;
                    pmm.ModifiedDate = DateTime.Now;

                    pmm.Save();
                    this.dgData.EditItemIndex = -1;
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
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                82);
                if (uAccess == null)
                    Response.Redirect("../../Unauthorized.htm");

                DropDownList ddlNewProductGroupManagerName = (DropDownList)e.Item.FindControl("ddlNewProductGroupManagerName");
                TextBox txtNewMonthlyCommissionRate = (TextBox)e.Item.FindControl("txtNewMonthlyCommissionRate");
                TextBox txtNewQuarterlyCommissionRate = (TextBox)e.Item.FindControl("txtNewQuarterlyCommissionRate");
                TextBox txtNewEffectiveDate = (TextBox)e.Item.FindControl("txtNewEffectiveDate");

                if (txtNewMonthlyCommissionRate != null && !string.IsNullOrEmpty(txtNewMonthlyCommissionRate.Text) &&
                    txtNewQuarterlyCommissionRate != null && !string.IsNullOrEmpty(txtNewQuarterlyCommissionRate.Text) && ddlNewProductGroupManagerName != null &&
                    txtNewEffectiveDate != null && !string.IsNullOrEmpty(txtNewEffectiveDate.Text))
                {
                    try
                    {
                        GMSCore.Entity.ProductManagerMaster existingProductManagerMaster = GMSCore.Entity.ProductManagerMaster.RetrieveByKey(
                            session.CompanyId, short.Parse(ddlNewProductGroupManagerName.SelectedValue), GMSUtil.ToDate(txtNewEffectiveDate.Text));
                        if (existingProductManagerMaster != null)
                        {
                            this.PageMsgPanel.ShowMessage("Processing error of type : Record already exists.", MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }

                        GMSCore.Entity.ProductManagerMaster newProductManagerMaster = new ProductManagerMaster();
                        newProductManagerMaster.CoyID = session.CompanyId;
                        newProductManagerMaster.ProductGroupManagerID = short.Parse(ddlNewProductGroupManagerName.SelectedValue);
                        newProductManagerMaster.MonthlyCommissionRate = decimal.Parse(txtNewMonthlyCommissionRate.Text);
                        newProductManagerMaster.QuarterlyCommissionRate = decimal.Parse(txtNewQuarterlyCommissionRate.Text);
                        newProductManagerMaster.EffectiveDate = GMSUtil.ToDate(txtNewEffectiveDate.Text);
                        newProductManagerMaster.CreatedBy = session.UserId;
                        newProductManagerMaster.CreatedDate = DateTime.Now;


                        newProductManagerMaster.Save();
                        this.dgData.EditItemIndex = -1;
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
                                                                                82);
                if (uAccess == null)
                    Response.Redirect("../../Unauthorized.htm");

                HtmlInputHidden hidProductGroupManagerID = (HtmlInputHidden)e.Item.FindControl("hidProductGroupManagerID");
                HtmlInputHidden hidEfffectiveDate = (HtmlInputHidden)e.Item.FindControl("hidEfffectiveDate");

                if (hidProductGroupManagerID != null && hidEfffectiveDate != null)
                {
                    SalesPersonRecordActivity sActivity = new SalesPersonRecordActivity();

                    try
                    {
                        GMSCore.Entity.ProductManagerMaster pmm = GMSCore.Entity.ProductManagerMaster.RetrieveByKey(session.CompanyId, short.Parse(hidProductGroupManagerID.Value), GMSUtil.ToDate(hidEfffectiveDate.Value));
                        pmm.Delete();
                        pmm.Resync();

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
