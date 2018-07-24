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

namespace GMSWeb.HR.Staff
{
    public partial class Staff : GMSBasePage
    {
        protected int PageSize = 8;

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            43);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                ViewState["CurrentPageIndex"] = 0;
              //  LoadData();
                if (session.UserId == 1)
                showDOB();
            }
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            string employeeNo = "%";
            if (!string.IsNullOrEmpty(txtSearchEmployeeNo.Text))
                employeeNo = "%" + txtSearchEmployeeNo.Text.Trim() + "%";
            string name = "%";
            if (!string.IsNullOrEmpty(txtSearchName.Text))
            name = "%"+txtSearchName.Text.Trim()+"%";
            string designation = "%";
            if (!string.IsNullOrEmpty(txtSearchDesignation.Text))
            designation = "%"+txtSearchDesignation.Text.Trim()+"%";
            string grade = "%";
            if (!string.IsNullOrEmpty(txtSearchGrade.Text))
            grade = "%" + txtSearchGrade.Text.Trim() + "%";
            bool isActive = rbIsActive.Checked;
            string nric = "%";
            if (!string.IsNullOrEmpty(txtSearchNRIC.Text))
                nric = "%" + txtSearchNRIC.Text.Trim() + "%";
            IList<GMSCore.Entity.Employee> lstEmployee = null;
            short coyId = session.CompanyId;
            if (ViewState["TYPE"].ToString() == "HR")
                coyId = 61;

            try
            {
                lstEmployee = new SystemDataActivity().RetrieveEmployeeListByEmployeeNoByNameByDesignationByNRICByGradeByActiveSortByEmployeeName(employeeNo, name, designation, nric, grade, isActive, coyId);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            PagedDataSource objPage = new PagedDataSource();
            objPage.AllowPaging = true;
            objPage.DataSource = lstEmployee;
            objPage.PageSize = PageSize;
            objPage.CurrentPageIndex = (int)ViewState["CurrentPageIndex"];

            int startIndex = (((int)ViewState["CurrentPageIndex"] + 1) * PageSize) - (PageSize - 1);
            int endIndex = ((int)ViewState["CurrentPageIndex"] + 1) * PageSize;

            if (lstEmployee != null && lstEmployee.Count > 0)
            {
                if (endIndex < lstEmployee.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + lstEmployee.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        lstEmployee.Count.ToString() + " " + "of" + " " + lstEmployee.Count.ToString();
            }
            else
                this.lblSearchSummary.Text = "No records.";

            this.lblSearchSummary.Visible = true;
            this.dlData.DataSource = objPage;
            this.dlData.DataBind();

            if (dlData.Controls.Count > 0)
            {
                ((LinkButton)dlData.Controls[dlData.Controls.Count - 1].FindControl("lnkPrevPage")).Enabled = !objPage.IsFirstPage;
                ((LinkButton)dlData.Controls[dlData.Controls.Count - 1].FindControl("lnkNextPage")).Enabled = !objPage.IsLastPage;
            }
        }
        #endregion

        protected void dlData_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "ChangePage":
                    int i = int.Parse(e.CommandArgument.ToString());
                    ViewState["CurrentPageIndex"] = (int)ViewState["CurrentPageIndex"] + i;
                    dlData.EditItemIndex = -1;
                    LoadData();
                    break;
            }
        }

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["CurrentPageIndex"] = 0;
            LoadData();
        }
        #endregion

        #region showDOB
        private void showDOB()
        {
            IList<Employee> lstEmployee = new SystemDataActivity().RetrieveEmployeeListByDOBSortByEmployeeName(DateTime.Today);
            if (lstEmployee.Count > 0)
            {
                string script = "alert('Today is ";
                foreach (Employee employee in lstEmployee)
                {
                    script = script + employee.Name + ", ";
                }
                script = script.TrimEnd(',',' ') + "\\'s";
                script = script + " birthday!');";
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dobscript1", script, true);
            }
        }
        #endregion
    }
}
