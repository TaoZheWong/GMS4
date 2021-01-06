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

namespace GMSWeb.Admin.SystemData
{
    public partial class CompanyData : GMSBasePage
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
                                                                            6);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack)
            {
                LoadCompanyData();
            }
        }

        //Load Data
        #region LoadCompanyData
        private void LoadCompanyData()
        {
            LogSession session = base.GetSessionInfo();
            IList<VwCompanyListing> lstCompany = null;
            try
            {
                lstCompany = new SystemDataActivity().RetrieveCompanyListViewSortByCountryIdDivIdCoyName(session);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            }

            this.dgCompany.DataSource = lstCompany;
            this.dgCompany.DataBind();
        }
        #endregion

        #region dgCompany_ItemDataBound
        protected void dgCompany_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddlEditCountry = (DropDownList)e.Item.FindControl("ddlEditCountry");
                DropDownList ddlEditDivision = (DropDownList)e.Item.FindControl("ddlEditDivision");
                TextBox txtEditName = (TextBox)e.Item.FindControl("txtEditName");

                if (ddlEditCountry != null && ddlEditDivision != null && txtEditName != null)
                {
                    VwCompanyListing company = (VwCompanyListing)e.Item.DataItem;
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    if (company != null)
                    {
                        // fill in country dropdown list
                        IList<GMSCore.Entity.Country> lstCountry = null;
                        lstCountry = sDataActivity.RetrieveAllCountryListSortBySeqID();
                        ddlEditCountry.DataSource = lstCountry;
                        ddlEditCountry.DataBind();
                        ddlEditCountry.SelectedValue = company.CountryID.ToString();

                        // fill in division dropdown list
                        IList<Division> lstDivision = null;
                        lstDivision = sDataActivity.RetrieveAllDivisionListSortBySeqID();
                        ddlEditDivision.DataSource = lstDivision;
                        ddlEditDivision.DataBind();
                        ddlEditDivision.SelectedValue = company.DivisionID.ToString();
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewCountry = (DropDownList)e.Item.FindControl("ddlNewCountry");
                DropDownList ddlNewDivision = (DropDownList)e.Item.FindControl("ddlNewDivision");

                if (ddlNewCountry != null && ddlNewDivision!= null)
                {
                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    // fill in country dropdown list
                    IList<GMSCore.Entity.Country> lstCountry = null;
                    lstCountry = sDataActivity.RetrieveAllCountryListSortBySeqID();
                    ddlNewCountry.DataSource = lstCountry;
                    ddlNewCountry.DataBind();

                    // fill in division dropdown list
                    IList<Division> lstDivision = null;
                    lstDivision = sDataActivity.RetrieveAllDivisionListSortBySeqID();
                    ddlNewDivision.DataSource = lstDivision;
                    ddlNewDivision.DataBind();
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region dgCompany_EditCommand
        protected void dgCompany_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgCompany.EditItemIndex = e.Item.ItemIndex;
            LoadCompanyData();
        }
        #endregion

        #region dgCompany_CancelCommand
        protected void dgCompany_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgCompany.EditItemIndex = -1;
            LoadCompanyData();
        }
        #endregion

        #region dgCompany_UpdateCommand
        protected void dgCompany_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            DropDownList ddlEditCountry = (DropDownList)e.Item.FindControl("ddlEditCountry");
            DropDownList ddlEditDivision = (DropDownList)e.Item.FindControl("ddlEditDivision");
            TextBox txtEditName = (TextBox)e.Item.FindControl("txtEditName");
            TextBox txtEditDBName = (TextBox)e.Item.FindControl("txtEditDBName");

            if (txtEditName != null && ddlEditCountry != null && ddlEditDivision != null &&
                !string.IsNullOrEmpty(txtEditName.Text) && txtEditDBName != null && !string.IsNullOrEmpty(txtEditDBName.Text))
            {
                short sCoyId = GMSUtil.ToShort(this.dgCompany.DataKeys[e.Item.ItemIndex]);

                if (sCoyId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    Company company = null;

                    try
                    {
                        company = sDataActivity.RetrieveCompanyById(sCoyId, session);
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }

                    company.CountryID = GMSUtil.ToShort(ddlEditCountry.SelectedValue);
                    company.DivisionID = GMSUtil.ToShort(ddlEditDivision.SelectedValue);
                    company.Name = txtEditName.Text.Trim();
                    company.DBName = txtEditDBName.Text.Trim();
                    company.ModifiedBy = session.UserId;
                    company.ModifiedDate = DateTime.Now;

                    try
                    {
                        ResultType result = sDataActivity.UpdateCompany(ref company, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgCompany.EditItemIndex = -1;
                                LoadCompanyData();
                                break;
                            default:
                                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgCompany_CreateCommand
        protected void dgCompany_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                DropDownList ddlNewCountry = (DropDownList)e.Item.FindControl("ddlNewCountry");
                DropDownList ddlNewDivision = (DropDownList)e.Item.FindControl("ddlNewDivision");
                TextBox txtNewName = (TextBox)e.Item.FindControl("txtNewName");
                TextBox txtNewDBName = (TextBox)e.Item.FindControl("txtNewDBName");

                if (txtNewName != null && !string.IsNullOrEmpty(txtNewName.Text) &&
                    ddlNewCountry != null && ddlNewDivision != null && txtNewDBName != null && !string.IsNullOrEmpty(txtNewDBName.Text))
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();
                    Company company = new Company();

                    company.Name = txtNewName.Text.Trim();
                    company.CountryID = GMSUtil.ToShort(ddlNewCountry.SelectedValue);
                    company.DivisionID = GMSUtil.ToShort(ddlNewDivision.SelectedValue);
                    company.DBName = txtNewDBName.Text.Trim();
                    company.CreatedBy = session.UserId;
                    company.CreatedDate = DateTime.Now;

                    try
                    {
                        ResultType result = sDataActivity.CreateCompany(ref company, session);

                        switch (result)
                        {
                            case ResultType.Ok:

                                LoadCompanyData();
                                break;
                            default:
                                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgCompany_DeleteCommand
        protected void dgCompany_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                short sCoyId = GMSUtil.ToShort(this.dgCompany.DataKeys[e.Item.ItemIndex]);

                if (sCoyId > 0)
                {
                    LogSession session = base.GetSessionInfo();

                    SystemDataActivity sDataActivity = new SystemDataActivity();

                    try
                    {
                        ResultType result = sDataActivity.DeleteCompany(sCoyId, session);

                        switch (result)
                        {
                            case ResultType.Ok:
                                this.dgCompany.EditItemIndex = -1;
                                LoadCompanyData();
                                break;
                            default:
                                this.PageMsgPanel.ShowMessage("Processing error of type : " + result.ToString(), MessagePanelControl.MessageEnumType.Alert);
                                return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion
    }
}
