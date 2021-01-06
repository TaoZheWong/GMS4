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

namespace GMSWeb.Reports
{
    public partial class reportbanner : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadCountryList();
                LoadDivisionList();
                LoadCompanyList();
            }
        }

        private void LoadCountryList()
        {
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../blank.htm");
                return;
            }

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

            this.ddlCountry.SelectedValue = session.CountryId.ToString();
        }

        private void LoadDivisionList()
        {
            LogSession session = base.GetSessionInfo();
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

            this.ddlDivision.SelectedValue = session.DivisionId.ToString();
        }

        public void LoadCompanyList()
        {
            LogSession session = base.GetSessionInfo();
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
            if (lstCompany != null)
            {
                this.ddlCompany.DataSource = lstCompany;
                this.ddlCompany.DataBind();

                if (lstCompany.Count <= 0)
                    this.ddlCompany.Items.Insert(0, new ListItem("", "0"));

                this.ddlCompany.SelectedValue = session.CompanyId.ToString();
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            IList<Company> lstCompany = null;
            lstCompany = new SystemDataActivity().RetrieveCompanyByCountryIdDivisionId(GMSUtil.ToShort(ddlCountry.SelectedValue),
                                                                                    GMSUtil.ToShort(ddlDivision.SelectedValue)
                                                                                    );

            if (lstCompany != null && lstCompany.Count > 0)
            {
                foreach (Company coy in lstCompany)
                {
                    UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
                                                                           coy.Id);
                    if (uAccess != null)
                    {
                        session.CompanyId = coy.Id;
                        session.CountryId = coy.CountryID;
                        session.DivisionId = coy.DivisionID;
                        LoadCompanyList();
                        return;
                    }
                }
                this.ddlCountry.SelectedValue = session.CountryId.ToString();
                base.JScriptAlertMsg("You are not authorized to view this country data.");
                return;
            }
            else
                session.CompanyId = 0;
            session.CountryId = GMSUtil.ToShort(this.ddlCountry.SelectedValue);
            LoadCompanyList();
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            IList<Company> lstCompany = null;
            lstCompany = new SystemDataActivity().RetrieveCompanyByCountryIdDivisionId(GMSUtil.ToShort(ddlCountry.SelectedValue),
                                                                                    GMSUtil.ToShort(ddlDivision.SelectedValue)
                                                                                    );

            if (lstCompany != null && lstCompany.Count > 0)
            {
                foreach (Company coy in lstCompany)
                {
                    UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
                                                                           coy.Id);
                    if (uAccess != null)
                    {
                        session.CompanyId = coy.Id;
                        session.CountryId = coy.CountryID;
                        session.DivisionId = coy.DivisionID;
                        LoadCompanyList();
                        return;
                    }
                }
                this.ddlDivision.SelectedValue = session.DivisionId.ToString();
                base.JScriptAlertMsg("You are not authorized to view this division data.");
                return;
            }
            else
                session.CompanyId = 0;
            session.DivisionId = GMSUtil.ToShort(this.ddlDivision.SelectedValue);
            LoadCompanyList();
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            UserAccessCompany uAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserIdCoyId(session.UserId,
                                                                            GMSUtil.ToShort(this.ddlCompany.SelectedValue));
            if (uAccess == null)
            {
                this.ddlCompany.SelectedValue = session.CompanyId.ToString();
                base.JScriptAlertMsg("You are not authorized to view this company data.");
                return;
            }
            else
                session.CompanyId = GMSUtil.ToShort(this.ddlCompany.SelectedValue);
        }
    }
}
