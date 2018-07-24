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

namespace GMSWeb.Finance.Upload
{
    public partial class UploadProductBudget : GMSBasePage
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
                                                                            74);
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
                dr1["Year"] = DateTime.Now.Year + i;

                dtt1.Rows.Add(dr1);
            }

            this.ddlYear.DataSource = dtt1;
            this.ddlYear.DataBind();

            // load division ddl
            DataTable dtt = new DataTable();
            dtt.Columns.Add("DivisionName", typeof(string));
            dtt.Columns.Add("DivisionID", typeof(char));

            DataRow dr = dtt.NewRow();
            dr["DivisionName"] = "Local";
            dr["DivisionID"] = 'L';
            dtt.Rows.Add(dr);

            dr = dtt.NewRow();
            dr["DivisionName"] = "Export";
            dr["DivisionID"] = 'E';
            dtt.Rows.Add(dr);

            dr = dtt.NewRow();
            dr["DivisionName"] = "Inter-divison";
            dr["DivisionID"] = 'I';
            dtt.Rows.Add(dr);

            dr = dtt.NewRow();
            dr["DivisionName"] = "Industrial";
            dr["DivisionID"] = 'D';
            dtt.Rows.Add(dr);

            dr = dtt.NewRow();
            dr["DivisionName"] = "Wholesale";
            dr["DivisionID"] = 'W';
            dtt.Rows.Add(dr);

            dr = dtt.NewRow();
            dr["DivisionName"] = "Suntec City";
            dr["DivisionID"] = 'S';
            dtt.Rows.Add(dr);

            dr = dtt.NewRow();
            dr["DivisionName"] = "Illuma";
            dr["DivisionID"] = 'M';
            dtt.Rows.Add(dr);

            this.ddlDivision.DataSource = dtt;
            this.ddlDivision.DataBind();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                FileUpload1.SaveAs(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + "\\" + FileUpload1.FileName);

                this.IFrame1.Attributes["style"] = "";
                this.IFrame1.Attributes["src"] = String.Format("ParsingProductBudget.aspx?FILENAME={0}&YEAR={1}&DIVISIONID={2}",
                                                            Server.UrlEncode(FileUpload1.FileName),
                                                            this.ddlYear.SelectedValue,
                                                            this.ddlDivision.SelectedValue);


            }
            else
            {
                lblMsg.Text = "You have not specified a file.";
            }
        }
    }
}
