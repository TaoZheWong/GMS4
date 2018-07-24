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
using GMSCore;
using GMSCore.Activity;
using GMSWeb.CustomCtrl;
using System.Collections.Generic;
using GMSCore.Entity;
using System.Data.SqlClient;

namespace GMSWeb.SysHR.Training
{
    public partial class CourseSession : GMSBasePage
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
                                                                            39);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                //preload
                this.dgData.CurrentPageIndex = 0;
                LoadData();
            }
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            string courseTitle = "%";
            if (!string.IsNullOrEmpty(searchCourseTitle.Text))
                courseTitle = "%" + searchCourseTitle.Text.Trim() + "%";
            DateTime dFrom = GMSCoreBase.DEFAULT_NO_DATE.AddYears(3);
            DateTime dTo = GMSCoreBase.DEFAULT_NO_DATE.AddYears(3);
            if (GMSUtil.ToDate(dateFrom.Text) != GMSCore.GMSCoreBase.DEFAULT_NO_DATE || GMSUtil.ToDate(dateTo.Text) != GMSCore.GMSCoreBase.DEFAULT_NO_DATE)
            {
                dFrom = GMSUtil.ToDate(dateFrom.Text);
                dTo = (GMSUtil.ToDate(dateTo.Text) == GMSCore.GMSCoreBase.DEFAULT_NO_DATE) ? DateTime.Now.AddYears(2) : GMSUtil.ToDate(dateTo.Text);
            }
            IList<GMSCore.Entity.CourseSession> lstData = null;
            try
            {
                lstData = new SystemDataActivity().RetrieveCourseSessionListByCourseTitleByDateSortByCourseTitleDate(courseTitle, dFrom, dTo);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            //Update search result
            int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            if (lstData.Count > 0)
            {
                if (endIndex < lstData.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstData.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstData.Count.ToString() + " " + "of" + " " + lstData.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";
            this.lblSearchSummary.Visible = true;

            this.dgData.DataSource = lstData;
            this.dgData.DataBind();
        }
        #endregion

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            lblSearchSummary.Text = e.NewPageIndex.ToString();

            lblSearchSummary.Visible = true;
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

        #region btnAdd_Click
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddEditSession.aspx");
        }
        #endregion

        #region dgData_DeleteCommand
        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                HtmlInputHidden hidCourseSessionID = (HtmlInputHidden)e.Item.FindControl("hidCourseSessionID");

                if (hidCourseSessionID != null)
                {
                    LogSession session = base.GetSessionInfo();
                    try
                    {
                        GMSCore.Entity.CourseSession cs = GMSCore.Entity.CourseSession.RetrieveByKey(GMSUtil.ToInt(hidCourseSessionID.Value));
                        if (cs != null)
                        {
                            if (cs.EmployeeCourseList != null && cs.EmployeeCourseList.Count > 0)
                            {
                                base.JScriptAlertMsg("This course session cannot be deleted because it has been referenced by other value.");
                                this.PageMsgPanel.ShowMessage("This course session cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
                                LoadData();
                                return;
                            }
                            else
                            {
                                cs.Delete();
                                LoadData();
                            }
                        }
                    }
                    catch (SqlException exSql)
                    {
                        if (exSql.Number == 547)
                        {
                            this.PageMsgPanel.ShowMessage("This course session cannot be deleted because it has been referenced by other value.", MessagePanelControl.MessageEnumType.Alert);
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
    }
}
