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
using System.Data.SqlClient;

using GMSCore;
using GMSCore.Activity;
using System.Collections.Generic;
using GMSWeb.CustomCtrl;
using GMSCore.Entity;

namespace GMSWeb.Finance.Upload
{
    public partial class COAMapping : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentLink = "CompanyFinance";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();

            }

            Master.setCurrentLink(currentLink);
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            89);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                           89);


            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            if (!Page.IsPostBack)
            {
                //preload
                dgData.CurrentPageIndex = 0;
                LoadData();
            }

            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>"; 

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            string oldCOA = "%";
            string newCOA = "%";

            if (!string.IsNullOrEmpty(searchOldCOA.Text))
                oldCOA = "%" + searchOldCOA.Text.Trim() + "%";

            if (!string.IsNullOrEmpty(searchNewCOA.Text))
                newCOA = "%" + searchNewCOA.Text.Trim() + "%";
            
            IList<GMSCore.Entity.ChartOfAccountsMapping> lstCOA = null;
            try
            {
                lstCOA = new SystemDataActivity().RetrieveAllCOAMappingByCOAIDSortByOldCOAID(oldCOA,newCOA, session.CompanyId);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lstCOA != null && lstCOA.Count > 0)
            {
                if (endIndex < lstCOA.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstCOA.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstCOA.Count.ToString() + " " + "of" + " " + lstCOA.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = lstCOA;
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
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
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

        #region dgData_CreateCommand
        protected void dgData_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                    return;
                }

                TextBox txtNewOldCOAID = (TextBox)e.Item.FindControl("txtNewOldCOAID");
                TextBox txtNewNewCOAID = (TextBox)e.Item.FindControl("txtNewNewCOAID");
                CheckBox chkNewIs2016COA = (CheckBox)e.Item.FindControl("chkNewIs2016COA");
                if (txtNewOldCOAID != null && !string.IsNullOrEmpty(txtNewOldCOAID.Text) && txtNewNewCOAID != null && !string.IsNullOrEmpty(txtNewNewCOAID.Text))
                {
                    try
                    {
                        // check if newly inserted name already exist
                        GMSCore.Entity.ChartOfAccountsMapping existingCOA = ChartOfAccountsMapping.RetrieveByKey(session.CompanyId, txtNewOldCOAID.Text.Trim(), chkNewIs2016COA.Checked.ToString());

                        if (existingCOA != null)
                        {
                            this.PageMsgPanel.ShowMessage("Processing error of type : This COA already been used.", MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }

                        GMSCore.Entity.ChartOfAccountsMapping coa = new GMSCore.Entity.ChartOfAccountsMapping();
                        coa.CoyID = session.CompanyId;
                        coa.OldCOAID = txtNewOldCOAID.Text.Trim();
                        coa.NewCOAID = txtNewNewCOAID.Text.Trim();
                        coa.Is2016COA = chkNewIs2016COA.Checked;
                        coa.Save();
                        //coa.Resync();
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
                    Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                    return;
                }

                HtmlInputHidden hidOldCOAID = (HtmlInputHidden)e.Item.FindControl("hidOldCOAID");
                HtmlInputHidden hidIs2016COA = (HtmlInputHidden)e.Item.FindControl("hidIs2016COA");

                if (hidOldCOAID != null && hidIs2016COA != null)
                {
                    try
                    {
                        GMSCore.Entity.ChartOfAccountsMapping existingCOA = ChartOfAccountsMapping.RetrieveByKey(session.CompanyId, hidOldCOAID.Value.Trim(), hidIs2016COA.Value);
                        existingCOA.Delete();
                        existingCOA.Resync();
                        LoadData();
                    }
                    catch (SqlException exSql)
                    {
                        if (exSql.Number == 547)
                        {
                            this.PageMsgPanel.ShowMessage("This COA cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                            LoadData();
                            return;
                        }
                        else
                        {
                            this.PageMsgPanel.ShowMessage(exSql.Message, MessagePanelControl.MessageEnumType.Alert);
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

        #region dgData_EditCommand
        protected void dgData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }

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
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }

            TextBox txtEditNewCOAID = (TextBox)e.Item.FindControl("txtEditNewCOAID");
            HtmlInputHidden hidOldCOAID = (HtmlInputHidden)e.Item.FindControl("hidOldCOAID");
            HtmlInputHidden hidIs2016COA = (HtmlInputHidden)e.Item.FindControl("hidIs2016COA");
            if (txtEditNewCOAID != null && !string.IsNullOrEmpty(txtEditNewCOAID.Text) && hidOldCOAID != null && hidIs2016COA != null)
            {

                GMSCore.Entity.ChartOfAccountsMapping coa = ChartOfAccountsMapping.RetrieveByKey(session.CompanyId, hidOldCOAID.Value.Trim(), hidIs2016COA.Value);
                if (coa != null)
                {
                    try
                    {
                        coa.NewCOAID = txtEditNewCOAID.Text.Trim();
                        coa.Save();
                        //coa.Resync();
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
    }
}
