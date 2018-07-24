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

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;
namespace GMSWeb.SysHR.Training
{
    public partial class Organizer : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("HR"); 
            if (!Page.IsPostBack)
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    Response.Redirect(base.SessionTimeOutPage("HR"));
                    return;
                }
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                37);
                if (uAccess == null)
                    Response.Redirect(base.UnauthorizedPage("HR"));

                //preload
                this.dgOrganizer.CurrentPageIndex = 0;
                LoadOrganizerData();
            }
        }

        #region LoadOrganizerData
        private void LoadOrganizerData()
        {
            LogSession session = base.GetSessionInfo();
            string organizerName = "%";
            if (!string.IsNullOrEmpty(searchOrganizerName.Text))
                organizerName = "%" + searchOrganizerName.Text.Trim() + "%";
            IList<CourseOrganizer> lstOrganizer = null;
            try
            {
                lstOrganizer = new SystemDataActivity().RetrieveAllOrganizerListByOrganizerNameSortByOrganizerName(organizerName);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            //Update search result
            int startIndex = ((dgOrganizer.CurrentPageIndex + 1) * this.dgOrganizer.PageSize) - (this.dgOrganizer.PageSize - 1);
            int endIndex = (dgOrganizer.CurrentPageIndex + 1) * this.dgOrganizer.PageSize;

            if (lstOrganizer.Count > 0)
            {
                if (endIndex < lstOrganizer.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstOrganizer.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstOrganizer.Count.ToString() + " " + "of" + " " + lstOrganizer.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";
            this.lblSearchSummary.Visible = true;

            this.dgOrganizer.DataSource = lstOrganizer;
            this.dgOrganizer.DataBind();
        }
        #endregion

        #region dgOrganizer datagrid PageIndexChanged event handling
        protected void dgOrganizer_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadOrganizerData();
        }
        #endregion

        #region dgOrganizer_ItemDataBound
        protected void dgOrganizer_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region dgOrganizer_EditCommand
        protected void dgOrganizer_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgOrganizer.EditItemIndex = e.Item.ItemIndex;
            LoadOrganizerData();
        }
        #endregion

        #region dgOrganizer_CancelCommand
        protected void dgOrganizer_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgOrganizer.EditItemIndex = -1;
            LoadOrganizerData();
        }
        #endregion

        #region dgOrganizer_UpdateCommand
        protected void dgOrganizer_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            TextBox txtEditName = (TextBox)e.Item.FindControl("txtEditName");
            HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidID");

            if (txtEditName != null && !string.IsNullOrEmpty(txtEditName.Text) && hidID != null)
            {
                LogSession session = base.GetSessionInfo();

                CourseOrganizer organizer = new CourseOrganizerActivity().RetrieveOrganizerByOrganizerID(short.Parse(hidID.Value));

                organizer.OrganizerName = txtEditName.Text.Trim().ToUpper();

                try
                {
                    ResultType result = new CourseOrganizerActivity().UpdateOrganizer(ref organizer, session);

                    switch (result)
                    {
                        case ResultType.Ok:
                            this.dgOrganizer.EditItemIndex = -1;
                            LoadOrganizerData();
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

        #region dgOrganizer_CreateCommand
        protected void dgOrganizer_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();

                TextBox txtNewName = (TextBox)e.Item.FindControl("txtNewName");

                if (txtNewName != null && !string.IsNullOrEmpty(txtNewName.Text))
                {
                    try
                    {
                        // check if newly inserted parameter value already exist
                        CourseOrganizer existingOrganizer = new CourseOrganizerActivity().RetrieveOrganizerByOrganizerName(txtNewName.Text.Trim());

                        if (existingOrganizer != null)
                        {
                            this.PageMsgPanel.ShowMessage("Processing error of type : This Organizer already exists.", MessagePanelControl.MessageEnumType.Alert);
                            return;
                        }

                        CourseOrganizer organizer = new CourseOrganizer();
                        organizer.OrganizerName = txtNewName.Text.Trim().ToUpper();

                        //GMSCore.Entity.DocumentNumber documentNumber = GMSCore.Entity.DocumentNumber.RetrieveByKey(1, (short)DateTime.Now.Year);
                        //organizer.OrganizerID = documentNumber.OrganizerID;
                        //documentNumber.OrganizerID++;

                        ResultType result = new CourseOrganizerActivity().CreateOrganizer(ref organizer, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                //documentNumber.Save();
                                LoadOrganizerData();
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

        #region dgOrganizer_DeleteCommand
        protected void dgOrganizer_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                HtmlInputHidden hidID = (HtmlInputHidden)e.Item.FindControl("hidID");

                if (hidID != null)
                {
                    LogSession session = base.GetSessionInfo();

                    CourseOrganizerActivity oActivity = new CourseOrganizerActivity();

                    try
                    {
                        ResultType result = oActivity.DeleteOrganizer(short.Parse(hidID.Value), session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgOrganizer.EditItemIndex = -1;
                                this.dgOrganizer.CurrentPageIndex = 0;
                                LoadOrganizerData();
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
                            this.PageMsgPanel.ShowMessage("This organizer cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                            LoadOrganizerData();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        LoadOrganizerData();
                        return;
                    }
                }
            }
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgOrganizer.CurrentPageIndex = 0;
            LoadOrganizerData();
        }
        #endregion
    }
}
