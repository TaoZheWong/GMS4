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
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.SysHR.Training
{
    public partial class TrainingAttendee : GMSBasePage
    {
        private string CourseSessionID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.CourseSessionID = Request.Params["CourseSessionID"];

            if (!Page.IsPostBack)
                LoadDataGrid();
        }

        protected void LoadDataGrid()
        {
            CourseSession cs = CourseSession.RetrieveByKey(int.Parse(this.CourseSessionID));
            if (cs != null && cs.EmployeeCourseList != null && cs.EmployeeCourseList.Count > 0)
            {
                cs.Resync();
                this.dgData.DataSource = cs.EmployeeCourseList;
                this.dgData.DataBind();

                //this.lblTitle.Text = "<h3>History Rates for Foreign Currency: <b>" + this.foreignCurrencyCode + "</b></h3>";
            }
        }

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            foreach (DataGridItem item in this.dgData.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl("chkAttended");
                HtmlInputHidden hidEmployeeCourseID = (HtmlInputHidden)item.FindControl("hidEmployeeCourseID");
                if (cb != null && hidEmployeeCourseID != null)
                {
                    GMSCore.Entity.EmployeeCourse ec = GMSCore.Entity.EmployeeCourse.RetrieveByKey(int.Parse(hidEmployeeCourseID.Value.Trim()));
                    ec.Attended = cb.Checked;
                    ec.Save();
                    ec.Resync();
                }
            }
            lblMsg.Text = "Record has been saved.";
            LoadDataGrid();
        }
        #endregion
    }
}
