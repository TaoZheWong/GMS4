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
using System.IO;
using System.Data.SqlClient;
using System.Text; 

//using DTS;
using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Finance.Upload
{
    public partial class ImportTrialBalance : GMSBasePage
    {
        private int coyId;
        private string companyCode;
        LogSession session;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("CompanyFinance"); 
            session = base.GetSessionInfo();
            Company coy = new SystemDataActivity().RetrieveCompanyById(session.CompanyId, session);
            companyCode = coy.Code; 

            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("CompanyFinance"));
                return;
            }
            coyId = session.CompanyId; 

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            68);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("CompanyFinance"));

            if (!IsPostBack)
            {
                // load year ddl
                DataTable dtt1 = new DataTable();
                dtt1.Columns.Add("Year", typeof(string));

                for (int i = -2; i < 1; i++)
                {
                    DataRow dr1 = dtt1.NewRow();
                    dr1["Year"] = DateTime.Now.Year + i;

                    dtt1.Rows.Add(dr1);
                }

                this.ddlYear.DataSource = dtt1;
                this.ddlYear.DataBind();
                
                DateTime lastMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                this.ddlYear.SelectedValue = lastMonthDate.Year.ToString();  

                // load month ddl
                DataTable dtt2 = new DataTable();
                dtt2.Columns.Add("Month", typeof(string));

                for (int i = 1; i <= 12; i++)
                {
                    DataRow dr1 = dtt2.NewRow();
                    dr1["Month"] = i;
                    dtt2.Rows.Add(dr1);
                }

                this.ddlMonth.DataSource = dtt2;
                this.ddlMonth.DataBind();               
            }
            //btnImport.Attributes.Add("onclick", "javascript:" + btnImport.ClientID + ".disabled=true;" + this.GetPostBackEventReference(btnImport));
        }
        #endregion

        #region btnImport_Click
        //Revamped by Siew Siew Ong on 2015-10-14 
        protected void btnImport_Click(object sender, EventArgs e)
        {
            this.IFrame1.Attributes["style"] = "";
            this.IFrame1.Attributes["src"] = String.Format("ProcessTrialBalance.aspx?CoyID={0}&Year={1}&Month={2}",
                                                        coyId,ddlYear.SelectedValue,ddlMonth.SelectedValue);
        }
        #endregion
                
    }
}
