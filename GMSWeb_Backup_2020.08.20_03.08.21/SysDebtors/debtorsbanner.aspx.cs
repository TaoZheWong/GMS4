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

namespace GMSWeb.SysDebtors
{
    public partial class debtorsbanner : GMSBasePage
    {
        PageHandler pageHandler = new PageHandler();

        protected void Page_Load(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../SessionTimeout.htm");
                return;
            }

            if (!Page.IsPostBack)
            {
                LoadCountryList();
                LoadDivisionList();
                LoadCompanyList();
            }

            string inner = pageHandler.LoadTabs();
            linkTag.InnerHtml = inner;
        }

        private void LoadCountryList()
        {
            pageHandler.LoadCountryList(this.ddlCountry);
        }

        private void LoadDivisionList()
        {
            pageHandler.LoadDivisionList(this.ddlDivision);
        }

        public void LoadCompanyList()
        {
            pageHandler.LoadCompanyList(this.ddlCompany);
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            PageHandler pageHandler = new PageHandler();
            pageHandler.ddlCountry_SelectedIndexChanged(GMSUtil.ToShort(ddlCountry.SelectedValue),
                                                                                    GMSUtil.ToShort(ddlDivision.SelectedValue), sender);
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            PageHandler pageHandler = new PageHandler();
            pageHandler.ddlDivision_SelectedIndexChanged(GMSUtil.ToShort(ddlCountry.SelectedValue),
                                                                                    GMSUtil.ToShort(ddlDivision.SelectedValue), sender);
        }
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            PageHandler pageHandler = new PageHandler();
            pageHandler.ddlCompany_SelectedIndexChanged(GMSUtil.ToShort(this.ddlCompany.SelectedValue), sender);
        }
    }
}
