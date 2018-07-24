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
    public partial class UploadAudit : GMSBasePage
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
                                                                            15);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");


            if (!Page.IsPostBack)
            {
                this.calAuditDate.SelectedDate = DateTime.Now;
                LoadDDLs();
            }
        }

        protected void LoadDDLs()
        {
            // load purpose ddl
            IList<ItemPurpose> lstPurpose = null;
            lstPurpose = new SystemDataActivity().RetrieveAllItemPurposeListSortByName();

            this.ddlPurpose.DataSource = lstPurpose;
            this.ddlPurpose.DataBind();

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                FileUpload1.SaveAs(AppDomain.CurrentDomain.BaseDirectory + GMSCoreBase.TEMP_DOC_PATH + "\\" + FileUpload1.FileName);
                
                this.IFrame1.Attributes["style"] = "";
                this.IFrame1.Attributes["src"] = String.Format("ParsingAudit.aspx?FILENAME={0}&DATE={1}&PURPOSEID={2}",
                                                            Server.UrlEncode(FileUpload1.FileName),
                                                            this.calAuditDate.SelectedDate.ToString(), this.ddlPurpose.SelectedValue);
            }
            else
            {
                lblMsg.Text = "You have not specified a file.";
            }
        }
    }
}
