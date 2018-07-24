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

namespace GMSWeb.SysHR.Staff
{
    public partial class Staff : GMSBasePage
    {
        protected int PageSize = 8;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["CurrentPageIndex"] = 0;
                //  LoadData();
                //if (session.UserId == 1)
                //showDOB();
                ViewState["TYPE"] = Request.Params["TYPE"];
            }

            if (ViewState["TYPE"] == null || ViewState["TYPE"].ToString() == "")
                ViewState["TYPE"] = "CompanyHR";

            Master.setCurrentLink(ViewState["TYPE"].ToString());

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(ViewState["TYPE"].ToString()));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            43);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                            43);
            if (uAccess == null && (uAccessForCompanyList == null || uAccessForCompanyList.Count <=0))
                Response.Redirect(base.UnauthorizedPage(ViewState["TYPE"].ToString()));

            string javaScript =
            @"<script language=""javascript"" type=""text/javascript"" src=""/GMS3/scripts/popcalendar.js""></script>

            <script language=""javascript"" type=""text/javascript"">
            function viewEmployeeDetail(EmployeeID)
	        {					
		        var url = ""StaffDetail.aspx?EmployeeID="" + EmployeeID; 	
                window.open(url,"""",""width="" + 800 + "",height="" + 900 +"",resizable=yes,status=yes,menubar=no,scrollbars=yes"");			
		        return false;
	        }	
	        function resetList()
	        {
	            document.getElementById('ddlEmployeeNameList').value = '%';
	            document.getElementById('ddlEmployeeNoList').value = '%';
	            return false;
	        }
            </script>"; 

            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
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
                name = "%" + txtSearchName.Text.Trim() + "%";
            string designation = "%";
            if (!string.IsNullOrEmpty(txtSearchDesignation.Text))
                designation = "%" + txtSearchDesignation.Text.Trim() + "%";
            string grade = "%";
            if (!string.IsNullOrEmpty(txtSearchGrade.Text))
                grade = "%" + txtSearchGrade.Text.Trim() + "%";
            bool isActive = rbIsActive.Checked;
            string nric = "%";
            if (!string.IsNullOrEmpty(txtSearchNRIC.Text))
                nric = "%" + txtSearchNRIC.Text.Trim() + "%";
            //IList<GMSCore.Entity.Employee> lstEmployee = null;
            short coyId = session.CompanyId;
            if (ViewState["TYPE"].ToString() == "HR")
                coyId = 61;

            //try
            //{
            //    lstEmployee = new SystemDataActivity().RetrieveEmployeeListByEmployeeNoByNameByDesignationByNRICByGradeByActiveSortByEmployeeName(employeeNo, name, designation, nric, grade, isActive, coyId);
            //}
            //catch (Exception ex)
            //{
            //    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            //}

            GMSGeneralDALC dacl = new GMSGeneralDALC();
            DataSet ds = new DataSet();
            try
            {
                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,45);
                if (uAccess == null)
                    dacl.GetEmployeeListByDepartmentWildcardSelect(session.CompanyId, employeeNo, name, designation, nric, grade, isActive, session.UserId, session.UserName, ref ds);
                else 
                    dacl.GetEmployeeListWildcardSelect(session.CompanyId, employeeNo, name, designation, nric, grade, isActive, session.UserId, ref ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            PagedDataSource objPage = new PagedDataSource();
            objPage.AllowPaging = true;
            objPage.DataSource = ds.Tables[0].DefaultView;
            objPage.PageSize = PageSize;
            objPage.CurrentPageIndex = (int)ViewState["CurrentPageIndex"];

            int startIndex = (((int)ViewState["CurrentPageIndex"] + 1) * PageSize) - (PageSize - 1);
            int endIndex = ((int)ViewState["CurrentPageIndex"] + 1) * PageSize;

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                if (endIndex < ds.Tables[0].Rows.Count)
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        endIndex.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
                else
                    this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
                        ds.Tables[0].Rows.Count.ToString() + " " + "of" + " " + ds.Tables[0].Rows.Count.ToString();
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

            resultList.Visible = true;;
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

        //#region showDOB
        //private void showDOB()
        //{
        //    IList<Employee> lstEmployee = new SystemDataActivity().RetrieveEmployeeListByDOBSortByEmployeeName(DateTime.Today);
        //    if (lstEmployee.Count > 0)
        //    {
        //        string script = "alert('Today is ";
        //        foreach (Employee employee in lstEmployee)
        //        {
        //            script = script + employee.Name + ", ";
        //        }
        //        script = script.TrimEnd(',', ' ') + "\\'s";
        //        script = script + " birthday!');";
        //        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dobscript1", script, true);
        //    }
        //}
        //#endregion
    }
}
