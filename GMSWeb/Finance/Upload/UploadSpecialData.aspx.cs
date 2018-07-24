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
using System.Collections.Generic;

namespace GMSWeb.Finance.Upload
{
    public partial class UploadSpecialData : GMSBasePage
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
                                                                            26);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyFinance"));

            if (!Page.IsPostBack)
            {
                LoadDDLs();
                ddl_SelectedIndexChanged(null, null);
            }
        }

        protected void LoadDDLs()
        {
            LogSession session = base.GetSessionInfo();
            // load year ddl
            DataTable dtt1 = new DataTable();
            dtt1.Columns.Add("Year", typeof(string));

            for (int i = -11; i < 10; i++)
            {
                DataRow dr1 = dtt1.NewRow();
                dr1["Year"] = DateTime.Now.Year + i;

                dtt1.Rows.Add(dr1);
            }

            this.ddlYear.DataSource = dtt1;
            this.ddlYear.DataBind();
            this.ddlYear.SelectedValue = DateTime.Now.Year.ToString();

            // load month ddl
            DataTable dtt2 = new DataTable();
            dtt2.Columns.Add("Month", typeof(string));

            for (int i = 1; i < 13; i++)
            {
                DataRow dr2 = dtt2.NewRow();
                dr2["Month"] = i;

                dtt2.Rows.Add(dr2);
            }

            this.ddlMonth.DataSource = dtt2;
            this.ddlMonth.DataBind();
            this.ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

            // load purpose ddl
            IList<CompanySpecialDataPurpose> lstPurpose = (new SystemDataActivity()).RetrieveAllCompanySpecialDataPurposeByCoyID(session.CompanyId);

            this.ddlPurpose.DataSource = lstPurpose;
            this.ddlPurpose.DataBind();

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            CompanySpecialData sd = CompanySpecialData.RetrieveByKey(session.CompanyId, GMSUtil.ToShort(ddlYear.SelectedValue),
                                                GMSUtil.ToShort(ddlMonth.SelectedValue), GMSUtil.ToByte(ddlPurpose.SelectedValue));
            if (sd != null)
            {
                sd.Value = GMSUtil.ToDouble(txtValue.Text.Trim());
                sd.Save();
                this.JScriptAlertMsg("Value updated successfully!");
                txtValue.Text = "";
                txtValue.Focus();
            }
            else
            {
                sd = new CompanySpecialData();
                sd.CoyID = session.CompanyId;
                sd.TbYear = GMSUtil.ToShort(ddlYear.SelectedValue);
                sd.TbMonth = GMSUtil.ToShort(ddlMonth.SelectedValue);
                sd.SpecialDataPurposeID = GMSUtil.ToByte(ddlPurpose.SelectedValue);
                sd.Value = GMSUtil.ToDouble(txtValue.Text.Trim());
                sd.Save();
            }
            this.JScriptAlertMsg("Value inserted successfully!");
            ddl_SelectedIndexChanged(null, null);
            txtValue.Text = "";
            txtValue.Focus();
        }

        #region ddl_SelectedIndexChanged
        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            CompanySpecialData sd = CompanySpecialData.RetrieveByKey(session.CompanyId, GMSUtil.ToShort(ddlYear.SelectedValue),
                                                GMSUtil.ToShort(ddlMonth.SelectedValue), GMSUtil.ToByte(ddlPurpose.SelectedValue));
            if (sd != null)
            {
                lblCurrentValue.Text = sd.Value.ToString();
                txtValue.Focus();
            }
            else
            {
                lblCurrentValue.Text = "";
                txtValue.Focus();
            }
        }
        #endregion
    }
}
