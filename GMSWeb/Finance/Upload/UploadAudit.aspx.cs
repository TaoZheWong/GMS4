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

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Finance.Upload
{
    public partial class UploadAudit : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("CompanyFinance"); 
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            15);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyFinance"));


            if (!Page.IsPostBack)
            {
                //this.calAuditDate.SelectedDate = DateTime.Now;
                LoadDDLs();
            }
        }

        protected void LoadDDLs()
        {
            // load year ddl
            DataTable dtt1 = new DataTable();
            dtt1.Columns.Add("Year", typeof(string));

            for (int i = -2; i < 5; i++)
            {
                DataRow dr1 = dtt1.NewRow();
                dr1["Year"] = DateTime.Now.Year + i;

                dtt1.Rows.Add(dr1);
            }

            this.ddlYear.DataSource = dtt1;
            this.ddlYear.DataBind();

            // load purpose ddl
            IList<ItemPurpose> lstPurpose = new List<ItemPurpose>();  
            //lstPurpose = new SystemDataActivity().RetrieveAllItemPurposeListSortByID();
                       
            this.ddlPurpose.DataSource = lstPurpose;
            this.ddlPurpose.DataBind();
            this.ddlPurpose.Items.Insert(0, new ListItem("Balance Sheet Detailed", "4"));
            this.ddlPurpose.Items.Insert(0, new ListItem("Balance Sheet", "3")); 
            this.ddlPurpose.Items.Insert(0, new ListItem("P & L Detailed", "2"));
            this.ddlPurpose.Items.Insert(0, new ListItem("P & L", "1"));                 

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                FileUpload1.SaveAs(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + "\\" + FileUpload1.FileName);

                this.IFrame1.Attributes["style"] = "";
                this.IFrame1.Attributes["src"] = String.Format("ParsingAudit.aspx?FILENAME={0}&YEAR={1}&PURPOSEID={2}",
                                                            Server.UrlEncode(FileUpload1.FileName),
                                                            this.ddlYear.SelectedValue, this.ddlPurpose.SelectedValue);
            }
            else
            {
                lblMsg.Text = "You have not specified a file.";
            }
        }
    }
}
