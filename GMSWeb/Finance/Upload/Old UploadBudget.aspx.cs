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

namespace GMSWeb.Organization.Upload
{
    public partial class UploadBudget : GMSBasePage
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
                                                                            14);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");


            if (!Page.IsPostBack)
            {
                LoadDDLs();
            }
        }

        protected void LoadDDLs()
        {
            // load year ddl
            DataTable dtt1 = new DataTable();
            dtt1.Columns.Add("Year", typeof(string));

            for (int i = -1; i < 5; i++)
            {
                DataRow dr1 = dtt1.NewRow();
                dr1["Year"] = DateTime.Now.Year+i;

                dtt1.Rows.Add(dr1);
            }

            this.ddlYear.DataSource = dtt1;
            this.ddlYear.DataBind();

            // load purpose ddl
            IList<ItemPurpose> lstPurpose = null;
            lstPurpose = new SystemDataActivity().RetrieveAllItemPurposeListSortByName();

            this.ddlPurpose.DataSource = lstPurpose;
            this.ddlPurpose.DataBind();

            // load department ddl
            LogSession session = base.GetSessionInfo();
            IList<CompanyDepartment> lstDepartment = null;
            lstDepartment = new SystemDataActivity().RetrieveAllCompanyDepartmentListByCompanyIDSortByDepartmentID(session.CompanyId);

            this.ddlDepartment.DataSource = lstDepartment;
            this.ddlDepartment.DataBind();

            this.ddlDepartment.Items.Insert(0, new ListItem("Not Applicable", "-1"));
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                FileUpload1.SaveAs(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + "\\" + FileUpload1.FileName);

                this.IFrame1.Attributes["style"] = "";
                this.IFrame1.Attributes["src"] = String.Format("UploadParsing.aspx?FILENAME={0}&YEAR={1}&PURPOSEID={2}&DEPARTMENTID={3}",
                                                            Server.UrlEncode(FileUpload1.FileName),
                                                            this.ddlYear.SelectedValue,
                                                            this.ddlPurpose.SelectedValue,
                                                            this.ddlDepartment.SelectedValue);

                
            }
            else
            {
                lblMsg.Text = "You have not specified a file.";
            }
        }
    }
}
