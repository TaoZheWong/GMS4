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

namespace GMSWeb.HR.Training
{
    public partial class Course : GMSBasePage
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
                                                                            38);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                //preload
                this.dgCourse.CurrentPageIndex = 0;
                LoadCourseData();
            }
        }

        #region LoadCourseData
        private void LoadCourseData()
        {
            LogSession session = base.GetSessionInfo();
            string courseTitle = "%";
            if (!string.IsNullOrEmpty(searchCourseTitle.Text))
                courseTitle = "%" + searchCourseTitle.Text.Trim() + "%";
            string type = ddlCourseType.SelectedValue;
            IList<GMSCore.Entity.Course> lstCourse = null;
            try
            {
                lstCourse = new SystemDataActivity().RetrieveAllCourseByCourseTitleCourseTypeListSortByCourseCode(courseTitle, type);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            //Update search result
            int startIndex = ((dgCourse.CurrentPageIndex + 1) * this.dgCourse.PageSize) - (this.dgCourse.PageSize - 1);
            int endIndex = (dgCourse.CurrentPageIndex + 1) * this.dgCourse.PageSize;

            if (lstCourse.Count > 0)
            {
                if (endIndex < lstCourse.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstCourse.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstCourse.Count.ToString() + " " + "of" + " " + lstCourse.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";
            this.lblSearchSummary.Visible = true;

            this.dgCourse.DataSource = lstCourse;
            this.dgCourse.DataBind();
            
        }
        #endregion

        #region dgCourse datagrid PageIndexChanged event handling
        protected void dgCourse_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            lblSearchSummary.Text = e.NewPageIndex.ToString();
            
            lblSearchSummary.Visible = true;
            LoadCourseData();
           
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgCourse.CurrentPageIndex = 0;
            LoadCourseData();
        }
        #endregion

        #region btnAdd_Click
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddEditCourse.aspx");
        }
        #endregion
    }
}
