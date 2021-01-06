using GMSCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GMSWeb.SysHR.Training
{
    public partial class TEFsignin : GMSBasePage
    {
        string courseSessionID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Request.Params["COURSESESSIONID"] != null)
            {
                courseSessionID = Request.Params["COURSESESSIONID"].ToString().Trim();
            }
            if(IsPostBack)
                this.lblMessage.InnerText = "";
        }

        protected void btnSignIn_onClick(object sender, EventArgs e)
        {
            string NRIC = "";
            string EmployeeCourseID = "";
            if (this.txtNRIC.Value!=null)
                NRIC = this.txtNRIC.Value.Trim();

            DataSet ds = new DataSet();
            EmployeeDataDALC employeeData = new EmployeeDataDALC();
            try
            {
                employeeData.GetEmployeeDataByNRIC(NRIC, int.Parse(courseSessionID), ref ds);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        if(this.ddlCompany.Items.Count == 0)
                        {
                            this.multi.Visible = true;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                this.ddlCompany.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CompanyName"].ToString(), ds.Tables[0].Rows[i]["EmployeeCourseID"].ToString()));
                            }
                        }else
                        {
                            Response.Redirect("AddEditTEF.aspx?EMPLOYEECOURSEID=" + this.ddlCompany.SelectedValue);
                        }
                    }
                    else
                    {
                        EmployeeCourseID = ds.Tables[0].Rows[0]["EmployeeCourseID"].ToString();
                        Response.Redirect("AddEditTEF.aspx?EMPLOYEECOURSEID=" + EmployeeCourseID);
                    }
                }else
                {
                    this.multi.Visible = false;
                    this.ddlCompany.Items.Clear();
                    this.lblMessage.InnerText = "You have not registered in this course.";
                }
            }
            catch (Exception ex)
            {
                this.lblMessage.InnerText = ex.Message;
            }
        }

        public string getCurrentMonth
        {
            get { return DateTime.Now.Month.ToString(); }
        }
    }
}