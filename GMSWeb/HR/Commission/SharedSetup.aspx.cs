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

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
using System.Collections;

namespace GMSWeb.HR.Commission
{
    public partial class SharedSetup : GMSBasePage
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
                                                                            70);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyHR"));

            if (!Page.IsPostBack)
            {
                //preload
                LoadData();
            }
            
        }
        private void RatioCheck(DataTable dtSPS)
        {
            string problemteam = string.Empty;
            string preteamid = string.Empty, teamid = string.Empty;
            decimal ratioW = 0, ratioS = 0, ratioNA = 0;

            foreach (DataRow dr in dtSPS.Rows)
            {
                teamid = dr["SalesPersonTeamID"].ToString();
                if (preteamid != teamid)
                {
                    preteamid = teamid;
                    ratioW = 0; ratioS = 0; ratioNA = 0;

                    foreach (DataRow dr2 in dtSPS.Rows)
                    {
                        if (dr2["SalesPersonTeamID"].ToString() == teamid)
                        {
                            switch (dr2["GroupType"].ToString())
                            {
                                case "W":
                                    ratioW = ratioW + Convert.ToDecimal(dr2["Ratio"].ToString());
                                    break;
                                case "S":
                                    ratioS = ratioS + Convert.ToDecimal(dr2["Ratio"].ToString());
                                    break;
                                case "N/A":
                                    ratioNA = ratioNA + Convert.ToDecimal(dr2["Ratio"].ToString());
                                    break;
                            }
                        }
                    }

                    if ((ratioW != 1 && ratioW != 0)|| (ratioS != 1 && ratioS != 0) || (ratioNA != 1 && ratioNA != 0))
                    {
                        problemteam = problemteam + "[" + dr["Team"].ToString() + "] ";
                    }
                }
            }

            if (!string.IsNullOrEmpty(problemteam))
            {
                this.PageMsgPanel.ShowMessage("Please check " + problemteam + " ratio distibutions!", MessagePanelControl.MessageEnumType.Alert);
            }
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            DataSet dsSalesMaster = new DataSet();
            try
            {
                new GMSGeneralDALC().GetSalesPersonShared(session.CompanyId, ref dsSalesMaster);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (dsSalesMaster != null && dsSalesMaster.Tables[0].Rows.Count > 0)
            {
               

                if (endIndex < dsSalesMaster.Tables[0].Rows.Count)
                    this.lblSearchSummary.Text = "Joint Account Member List" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + dsSalesMaster.Tables[0].Rows.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Joint Account Member List" + " " + startIndex.ToString() + " - " +
                        dsSalesMaster.Tables[0].Rows.Count.ToString() + " " + "of" + " " + dsSalesMaster.Tables[0].Rows.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";
         
            this.lblSearchSummary.Visible = true;
            this.dgData.DataSource = dsSalesMaster.Tables[0];
            this.dgData.DataBind();

            RatioCheck(dsSalesMaster.Tables[0]);
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
                DropDownList ddlSalesPersonTeamID = (DropDownList)e.Item.FindControl("ddlSalesPersonTeamID");
                DropDownList ddlSalesPersonUserID = (DropDownList)e.Item.FindControl("ddlSalesPersonUserID");

                if (ddlSalesPersonTeamID != null && ddlSalesPersonUserID != null )
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill dropdown list
                    IList<GMSCore.Entity.SalesPersonMaster> lstSalesPerson = null;
                    lstSalesPerson = sDataActivity.RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterNameByMode(session.CompanyId, "Team");
                    ddlSalesPersonTeamID.DataSource = lstSalesPerson;
                    ddlSalesPersonTeamID.DataBind();

                    IList<GMSCore.Entity.SalesPersonMaster> lstSalesPerson2 = null;
                    lstSalesPerson2 = sDataActivity.RetrieveAllSalesPersonMasterListByCompanyIDSortBySalesPersonMasterNameByMode(session.CompanyId, "Member");
                    ddlSalesPersonUserID.DataSource = lstSalesPerson2;
                    ddlSalesPersonUserID.DataBind();
                }               
            }

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                HtmlInputHidden hidGroupType = (HtmlInputHidden)e.Item.FindControl("hidGroupType");
                DropDownList ddleditGroupType = (DropDownList)e.Item.FindControl("ddleditGroupType");
                ddleditGroupType.SelectedValue = hidGroupType.Value;
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
                                                                            71);
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
                                                                            71);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyHR"));


            HtmlInputHidden hidSalesPersonTeamID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonTeamID");
            HtmlInputHidden hidSalesPersonMasterID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonMasterID");
            DropDownList ddleditGroupType = (DropDownList)e.Item.FindControl("ddleditGroupType");
            TextBox txtEditRatio = (TextBox)e.Item.FindControl("txtEditRatio");

            if (ddleditGroupType != null && !string.IsNullOrEmpty(ddleditGroupType.SelectedValue) && hidSalesPersonTeamID != null &&
                txtEditRatio != null && !string.IsNullOrEmpty(txtEditRatio.Text) && hidSalesPersonMasterID != null )
            {
                try
                {
                    new GMSGeneralDALC().UpdateSalesPersonShared(session.CompanyId, GMSUtil.ToShort(hidSalesPersonTeamID.Value), GMSUtil.ToShort(hidSalesPersonMasterID.Value), ddleditGroupType.SelectedValue, GMSUtil.ToDecimal(txtEditRatio.Text));
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
                    Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                71);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("CompanyHR"));

                DropDownList ddlSalesPersonTeamID = (DropDownList)e.Item.FindControl("ddlSalesPersonTeamID");
                DropDownList ddlSalesPersonUserID = (DropDownList)e.Item.FindControl("ddlSalesPersonUserID");
                DropDownList ddlGroupType = (DropDownList)e.Item.FindControl("ddlGroupType");
                TextBox txtNewRatio = (TextBox)e.Item.FindControl("txtNewRatio");

                if (ddlSalesPersonTeamID != null && !string.IsNullOrEmpty(ddlSalesPersonTeamID.SelectedValue) &&
                    ddlSalesPersonUserID != null && !string.IsNullOrEmpty(ddlSalesPersonUserID.SelectedValue) &&
                    ddlGroupType != null && !string.IsNullOrEmpty(ddlGroupType.SelectedValue) &&
                    txtNewRatio != null && !string.IsNullOrEmpty(txtNewRatio.Text))
                {
                    try
                    {
                        new GMSGeneralDALC().InsertSalesPersonShared(session.CompanyId, 
                            GMSUtil.ToShort(ddlSalesPersonTeamID.SelectedValue), GMSUtil.ToShort(ddlSalesPersonUserID.SelectedValue), 
                            ddlGroupType.SelectedValue, GMSUtil.ToDecimal(txtNewRatio.Text));
                        
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
                    Response.Redirect(base.SessionTimeOutPage("CompanyHR"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                71);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("CompanyHR"));

                HtmlInputHidden hidSalesPersonTeamID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonTeamID");
                HtmlInputHidden hidSalesPersonMasterID = (HtmlInputHidden)e.Item.FindControl("hidSalesPersonMasterID");

                if ( hidSalesPersonTeamID != null && hidSalesPersonMasterID != null)
                {
                    try
                    {
                        new GMSGeneralDALC().DeleteSalesPersonShared(session.CompanyId, GMSUtil.ToShort(hidSalesPersonTeamID.Value), GMSUtil.ToShort(hidSalesPersonMasterID.Value));
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
