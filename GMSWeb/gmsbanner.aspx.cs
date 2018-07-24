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
using GMSCore.Entity;
using GMSCore.Activity;

namespace GMSWeb
{
    public partial class gmsbanner : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LogSession session = base.GetSessionInfo();
                if (session == null)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                else
                {
                    LoadCountryList();
                    LoadDivisionList();
                }
            }
        }
        
        private void LoadCountryList()
        {
            IList<Country> lstCountry = null;
            try
            {
                lstCountry = new SystemDataActivity().RetrieveAllCountryListSortBySeqID();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            this.ddlCountry.DataSource = lstCountry;
            this.ddlCountry.DataBind();
        }

        private void LoadDivisionList()
        {
            IList<Division> lstDivision = null;
            try
            {
                lstDivision = new SystemDataActivity().RetrieveAllDivisionListSortBySeqID();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            this.ddlDivision.DataSource = lstDivision;
            this.ddlDivision.DataBind();
        }

        public void LoadCompanyList(object sender, EventArgs e)
        {
            IList<Company> lstCompany = null;
            try
            {
                lstCompany = new SystemDataActivity().RetrieveCompanyByCountryIdDivisionId(GMSUtil.ToShort(ddlCountry.SelectedValue),
                                                                                    GMSUtil.ToShort(ddlDivision.SelectedValue)
                                                                                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
            this.ddlCompany.DataSource = lstCompany;
            this.ddlCompany.DataBind();
        }

        

    }
}
