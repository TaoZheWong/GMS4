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
using System.Data.SqlClient;

namespace GMSWeb.Finance.Upload
{
    public partial class DimensionSetup : GMSBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentLink = "CompanyFinance";
            lblPageHeader.Text = "CompanyFinance";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                if (Request.Params["CurrentLink"].ToString().Trim() == "CompanyFinance")
                    lblPageHeader.Text = "Administration";
                else
                    lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);
            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage(currentLink));
                return;
            }
            if (session.StatusType != "")
            {
                dgData.ShowFooter = false;
                dgData2.ShowFooter = false;
                dgData3.ShowFooter = false;
                dgData4.ShowFooter = false;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            160);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                                160);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));


            if (!Page.IsPostBack)
            {
                //preload
                dgData.CurrentPageIndex = 0;
                LoadData();
            }

            string javaScript =
            @"
            <script language=""javascript"" type=""text/javascript"" src=""/GMS4/scripts/popcalendar.js""></script>

            <script type=""text/javascript"">
            function SelectOthers(chkbox)
	        {
	            var prefix = chkbox.id.substring(0,chkbox.id.lastIndexOf(""_"")+1);
	            if (chkbox.checked)
	            {
	                document.getElementById(prefix+""spanSalesPersonMasterName"").style.visibility = 'hidden';
	                document.getElementById(prefix+""spanArea"").style.visibility = 'hidden';
	            } else
	            {
	                document.getElementById(prefix+""spanSalesPersonMasterName"").style.visibility = 'visible';
	                document.getElementById(prefix+""spanArea"").style.visibility = 'visible';
	            }
	        } 
        	 
            </script>
            ";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
        }

        #region LoadData
        private void LoadData()
        {
            LogSession session = base.GetSessionInfo();
            FinanceDALC proddacl = new FinanceDALC();
            DataSet dsProducts = new DataSet();
            DataSet dsDepartment = new DataSet();
            DataSet dsSection = new DataSet();
            DataSet dsUnit = new DataSet();
            proddacl.GetCompanyProject(session.CompanyId, ref dsProducts);
            proddacl.GetCompanyDepartment(session.CompanyId, ref dsDepartment);
            proddacl.GetCompanySection(session.CompanyId, ref dsSection);
            proddacl.GetCompanyUnit(session.CompanyId, ref dsUnit);
            this.dgData.DataSource = dsProducts.Tables[0];
            this.dgData.DataBind(); 
            this.dgData2.DataSource = dsDepartment.Tables[0];
            this.dgData2.DataBind();
            this.dgData3.DataSource = dsSection.Tables[0];
            this.dgData3.DataBind();
            this.dgData4.DataSource = dsUnit.Tables[0];
            this.dgData4.DataBind();
            #region comment
            //string ProductGroupCode = "";
            //string ShortName = "";
            //if ((!string.IsNullOrEmpty(txtProductGroupCode.Text)) || (!string.IsNullOrEmpty(txtShortName.Text)))
            //{
            //    ProductGroupCode = "%" + txtProductGroupCode.Text.Trim() + "%";
            //    ShortName = "%" + txtShortName.Text.Trim() + "%";

            //}
            //else {
            //    base.JScriptAlertMsg("Please input product code or short name to search.");
            //    return;
            //}
            //try
            //{
            //    proddacl.GetProductGroupCodeWithShortName(session.CompanyId, ProductGroupCode, ShortName, ref dsProducts);
            //}
            //catch (Exception ex)
            //{
            //    this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
            //}

            //int startIndex = ((dgData.CurrentPageIndex + 1) * this.dgData.PageSize) - (this.dgData.PageSize - 1);
            //int endIndex = (dgData.CurrentPageIndex + 1) * this.dgData.PageSize;

            //if (dsProducts != null && dsProducts.Tables[0].Rows.Count > 0)
            //{
            //    if (endIndex < dsProducts.Tables[0].Rows.Count)
            //        this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
            //            endIndex.ToString() + " " + "of" + " " + dsProducts.Tables[0].Rows.Count.ToString();
            //    else
            //        this.lblSearchSummary.Text = "Results" + " " + startIndex.ToString() + " - " +
            //            dsProducts.Tables[0].Rows.Count.ToString() + " " + "of" + " " + dsProducts.Tables[0].Rows.Count.ToString();

            //    this.lblSearchSummary.Visible = true;
            //    this.dgData.DataSource = dsProducts;
            //    this.dgData.DataBind();

            //}
            //else
            //{
            //    this.lblSearchSummary.Text = "No records.";
            //    this.lblSearchSummary.Visible = true;
            //    this.dgData.DataSource = null;
            //    this.dgData.DataBind();
            //    return;

            //}
            #endregion
        }
        #endregion

        #region dgData datagrid PageIndexChanged event handling
        protected void dgData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        #region dgData2 datagrid PageIndexChanged event handling
        protected void dgData2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        #region dgData3 datagrid PageIndexChanged event handling
        protected void dgData3_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        #region dgData4 datagrid PageIndexChanged event handling
        protected void dgData4_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid dtg = (DataGrid)source;
            dtg.CurrentPageIndex = e.NewPageIndex;
            LoadData();
        }
        #endregion

        #region dgData_ItemDataBound
        protected void dgData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewGroupName = (DropDownList)e.Item.FindControl("ddlNewProjectGroup");
                if (ddlNewGroupName != null)
                {
                    FinanceDALC proddacl = new FinanceDALC();
                    DataSet dsProject = new DataSet();
                    proddacl.GetCompanyProject(session.CompanyId, ref dsProject);
                    ddlNewGroupName.DataSource = dsProject.Tables[0];
                    ddlNewGroupName.DataBind();
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

        #region dgData2_ItemDataBound
        protected void dgData2_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewGroupName = (DropDownList)e.Item.FindControl("ddlNewDepartmentGroup");
                if (ddlNewGroupName != null)
                {
                    FinanceDALC proddacl = new FinanceDALC();
                    DataSet dsDepartment = new DataSet();
                    proddacl.GetCompanyDepartment(session.CompanyId, ref dsDepartment);
                    ddlNewGroupName.DataSource = dsDepartment.Tables[0];
                    ddlNewGroupName.DataBind();
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete2");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region dgData3_ItemDataBound
        protected void dgData3_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewGroupName = (DropDownList)e.Item.FindControl("ddlNewSectionGroup");
                if (ddlNewGroupName != null)
                {
                    FinanceDALC proddacl = new FinanceDALC();
                    DataSet dsSection = new DataSet();
                    proddacl.GetCompanySection(session.CompanyId, ref dsSection);
                    ddlNewGroupName.DataSource = dsSection.Tables[0];
                    ddlNewGroupName.DataBind();
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete3");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region dgData4_ItemDataBound
        protected void dgData4_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            LogSession session = base.GetSessionInfo();

            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlNewGroupName = (DropDownList)e.Item.FindControl("ddlNewUnitGroup");
                if (ddlNewGroupName != null)
                {
                    FinanceDALC proddacl = new FinanceDALC();
                    DataSet dsUnit = new DataSet();
                    proddacl.GetCompanyUnit(session.CompanyId, ref dsUnit);
                    ddlNewGroupName.DataSource = dsUnit.Tables[0];
                    ddlNewGroupName.DataBind();
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete4");
                if (lnkDelete != null)
                    lnkDelete.Attributes.Add("onclick", "return confirm('Confirm deletion of this record?')");
            }
        }
        #endregion

        #region dgData_EditCommand
        protected void dgData_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }
        #endregion

        #region dgData2_EditCommand
        protected void dgData2_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData2.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }
        #endregion

        #region dgData3_EditCommand3
        protected void dgData3_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData3.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }
        #endregion

        #region dgData4_EditCommand4
        protected void dgData4_EditCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData4.EditItemIndex = e.Item.ItemIndex;
            LoadData();
        }
        #endregion

        #region dgData_CancelCommand
        protected void dgData_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgData2_CancelCommand
        protected void dgData2_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData2.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgData3_CancelCommand
        protected void dgData3_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData3.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgData4_CancelCommand
        protected void dgData4_CancelCommand(object sender, DataGridCommandEventArgs e)
        {
            this.dgData4.EditItemIndex = -1;
            LoadData();
        }
        #endregion

        #region dgData_CreateCommand
        protected void dgData_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
           
                string currentLink = "CompanyFinance";
                lblPageHeader.Text = "CompanyFinance";

                if (Request.Params["CurrentLink"] != null)
                {
                    currentLink = Request.Params["CurrentLink"].ToString().Trim();
                    if (Request.Params["CurrentLink"].ToString().Trim() == "CompanyFinance")
                        lblPageHeader.Text = "Administration";
                    else
                        lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
                }
                Master.setCurrentLink(currentLink);


                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                161);

                IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                               161);

                if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                    Response.Redirect(base.UnauthorizedPage(currentLink));

                TextBox txtNewProjectCode = (TextBox)e.Item.FindControl("txtNewProjectCode");
                TextBox txtNewProjectName = (TextBox)e.Item.FindControl("txtNewProjectName");
                TextBox txtNewShortName = (TextBox)e.Item.FindControl("txtNewShortName");

                if (txtNewProjectCode != null && !string.IsNullOrEmpty(txtNewProjectCode.Text) &&
                    txtNewProjectName != null && !string.IsNullOrEmpty(txtNewProjectName.Text) &&
                   txtNewShortName != null && !string.IsNullOrEmpty(txtNewShortName.Text))
                {
                    try
                    {
                        new FinanceDALC().AddNewCompanyProject(session.CompanyId, txtNewProjectCode.Text.Trim(), 
                            txtNewProjectName.Text.Trim(), txtNewShortName.Text.Trim());
                        this.dgData.EditItemIndex = -1;
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgData2_CreateCommand
        protected void dgData2_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
                string currentLink = "CompanyFinance";
                lblPageHeader.Text = "CompanyFinance";

                if (Request.Params["CurrentLink"] != null)
                {
                    currentLink = Request.Params["CurrentLink"].ToString().Trim();
                    if (Request.Params["CurrentLink"].ToString().Trim() == "CompanyFinance")
                        lblPageHeader.Text = "Administration";
                    else
                        lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
                }
                Master.setCurrentLink(currentLink);

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                161);

                IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                               161);

                if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                    Response.Redirect(base.UnauthorizedPage(currentLink));

                TextBox txtNewDepartmentCode = (TextBox)e.Item.FindControl("txtNewDepartmentCode");
                TextBox txtNewDepartmentName = (TextBox)e.Item.FindControl("txtNewDepartmentName");
                TextBox txtNewShortName = (TextBox)e.Item.FindControl("txtNewShortName2");

                if (txtNewDepartmentCode != null && !string.IsNullOrEmpty(txtNewDepartmentCode.Text) &&
                    txtNewDepartmentName != null && !string.IsNullOrEmpty(txtNewDepartmentName.Text) &&
                   txtNewShortName != null && !string.IsNullOrEmpty(txtNewShortName.Text))
                {
                    try
                    {
                        new FinanceDALC().AddNewCompanyDepartment(session.CompanyId, txtNewDepartmentCode.Text.Trim(),
                            txtNewDepartmentName.Text.Trim(), txtNewShortName.Text.Trim());
                        this.dgData2.EditItemIndex = -1;
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel2.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgData3_CreateCommand
        protected void dgData3_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
                string currentLink = "CompanyFinance";
                lblPageHeader.Text = "CompanyFinance";

                if (Request.Params["CurrentLink"] != null)
                {
                    currentLink = Request.Params["CurrentLink"].ToString().Trim();
                    if (Request.Params["CurrentLink"].ToString().Trim() == "CompanyFinance")
                        lblPageHeader.Text = "Administration";
                    else
                        lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
                }
                Master.setCurrentLink(currentLink);

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                161);

                IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                               161);

                if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                    Response.Redirect(base.UnauthorizedPage(currentLink));

                TextBox txtNewSectionCode = (TextBox)e.Item.FindControl("txtNewSectionCode");
                TextBox txtNewSectionName = (TextBox)e.Item.FindControl("txtNewSectionName");
                TextBox txtNewShortName = (TextBox)e.Item.FindControl("txtNewShortName3");

                if (txtNewSectionCode != null && !string.IsNullOrEmpty(txtNewSectionCode.Text) &&
                    txtNewSectionName != null && !string.IsNullOrEmpty(txtNewSectionName.Text) &&
                   txtNewShortName != null && !string.IsNullOrEmpty(txtNewShortName.Text))
                {

                    try
                    {
                        new FinanceDALC().AddNewCompanySection(session.CompanyId, txtNewSectionCode.Text.Trim(),
                            txtNewSectionName.Text.Trim(), txtNewShortName.Text.Trim());
                        this.dgData3.EditItemIndex = -1;
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel3.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgData4_CreateCommand
        protected void dgData4_CreateCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Create")
            {
                LogSession session = base.GetSessionInfo();
                string currentLink = "CompanyFinance";
                lblPageHeader.Text = "CompanyFinance";

                if (Request.Params["CurrentLink"] != null)
                {
                    currentLink = Request.Params["CurrentLink"].ToString().Trim();
                    if (Request.Params["CurrentLink"].ToString().Trim() == "CompanyFinance")
                        lblPageHeader.Text = "Administration";
                    else
                        lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
                }
                Master.setCurrentLink(currentLink);

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                161);

                IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                               161);

                if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                    Response.Redirect(base.UnauthorizedPage(currentLink));

                TextBox txtNewUnitCode = (TextBox)e.Item.FindControl("txtNewUnitCode");
                TextBox txtNewUnitName = (TextBox)e.Item.FindControl("txtNewUnitName");
                TextBox txtNewShortName = (TextBox)e.Item.FindControl("txtNewShortName4");

                if (txtNewUnitCode != null && !string.IsNullOrEmpty(txtNewUnitCode.Text) &&
                    txtNewUnitName != null && !string.IsNullOrEmpty(txtNewUnitName.Text) &&
                   txtNewShortName != null && !string.IsNullOrEmpty(txtNewShortName.Text))
                {

                    try
                    {
                        new FinanceDALC().AddNewCompanyUnit(session.CompanyId, txtNewUnitCode.Text.Trim(),
                            txtNewUnitName.Text.Trim(), txtNewShortName.Text.Trim());
                        this.dgData4.EditItemIndex = -1;
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel4.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgData_UpdateCommand
        protected void dgData_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            
             LogSession session = base.GetSessionInfo();
            string currentLink = "CompanyFinance";
            lblPageHeader.Text = "CompanyFinance";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                if (Request.Params["CurrentLink"].ToString().Trim() == "CompanyFinance")
                    lblPageHeader.Text = "Administration";
                else
                    lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            161);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                           161);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            HtmlInputHidden hidProjectID = (HtmlInputHidden)e.Item.FindControl("hidProjectID");
                TextBox txtEditShortName = (TextBox)e.Item.FindControl("txtEditShortName");

                if (hidProjectID != null &&
                    txtEditShortName != null && !string.IsNullOrEmpty(txtEditShortName.Text))
                {
                    try
                    {
                        new FinanceDALC().UpdateCompanyProjectShortName(session.CompanyId, hidProjectID.Value, txtEditShortName.Text.Trim());
                        this.dgData.EditItemIndex = -1;
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            
        }
        #endregion

        #region dgData2_UpdateCommand
        protected void dgData2_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string currentLink = "CompanyFinance";
            lblPageHeader.Text = "CompanyFinance";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                if (Request.Params["CurrentLink"].ToString().Trim() == "CompanyFinance")
                    lblPageHeader.Text = "Administration";
                else
                    lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            161);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                           161);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            HtmlInputHidden hidProjectID = (HtmlInputHidden)e.Item.FindControl("hidDepartmentID");
            TextBox txtEditShortName = (TextBox)e.Item.FindControl("txtEditShortName2");

            if (hidProjectID != null &&
                txtEditShortName != null && !string.IsNullOrEmpty(txtEditShortName.Text))
            {
                try
                {
                    new FinanceDALC().UpdateCompanyDepartmentShortName(session.CompanyId, hidProjectID.Value, txtEditShortName.Text.Trim());
                    this.dgData2.EditItemIndex = -1;
                    LoadData();
                }
                catch (Exception ex)
                {
                    this.MsgPanel2.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }
        }
        #endregion

        #region dgData3_UpdateCommand
        protected void dgData3_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string currentLink = "CompanyFinance";
            lblPageHeader.Text = "CompanyFinance";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                if (Request.Params["CurrentLink"].ToString().Trim() == "CompanyFinance")
                    lblPageHeader.Text = "Administration";
                else
                    lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            161);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                           161);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            HtmlInputHidden hidProjectID = (HtmlInputHidden)e.Item.FindControl("hidSectionID");
            TextBox txtEditShortName = (TextBox)e.Item.FindControl("txtEditShortName3");

            if (hidProjectID != null &&
                txtEditShortName != null && !string.IsNullOrEmpty(txtEditShortName.Text))
            {
                try
                {
                    new FinanceDALC().UpdateCompanySectionShortName(session.CompanyId, hidProjectID.Value, txtEditShortName.Text.Trim());
                    this.dgData3.EditItemIndex = -1;
                    LoadData();
                }
                catch (Exception ex)
                {
                    this.MsgPanel3.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }
        }
        #endregion

        #region dgData4_UpdateCommand
        protected void dgData4_UpdateCommand(object sender, DataGridCommandEventArgs e)
        {
            LogSession session = base.GetSessionInfo();
            string currentLink = "CompanyFinance";
            lblPageHeader.Text = "CompanyFinance";

            if (Request.Params["CurrentLink"] != null)
            {
                currentLink = Request.Params["CurrentLink"].ToString().Trim();
                if (Request.Params["CurrentLink"].ToString().Trim() == "CompanyFinance")
                    lblPageHeader.Text = "Administration";
                else
                    lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
            }
            Master.setCurrentLink(currentLink);

            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            161);

            IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                           161);

            if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                Response.Redirect(base.UnauthorizedPage(currentLink));

            HtmlInputHidden hidUnitID = (HtmlInputHidden)e.Item.FindControl("hidUnitID");
            TextBox txtEditShortName = (TextBox)e.Item.FindControl("txtEditShortName4");

            if (hidUnitID != null &&
                txtEditShortName != null && !string.IsNullOrEmpty(txtEditShortName.Text))
            {
                try
                {
                    new FinanceDALC().UpdateCompanyUnitShortName(session.CompanyId, hidUnitID.Value, txtEditShortName.Text.Trim());
                    this.dgData4.EditItemIndex = -1;
                    LoadData();
                }
                catch (Exception ex)
                {
                    this.MsgPanel4.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                    return;
                }
            }
        }
        #endregion

        #region dgData_DeleteCommand
        protected void dgData_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                string currentLink = "CompanyFinance";
                lblPageHeader.Text = "CompanyFinance";

                if (Request.Params["CurrentLink"] != null)
                {
                    currentLink = Request.Params["CurrentLink"].ToString().Trim();
                    if (Request.Params["CurrentLink"].ToString().Trim() == "CompanyFinance")
                        lblPageHeader.Text = "Administration";
                    else
                        lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
                }
                Master.setCurrentLink(currentLink);
              
                if (session.StatusType != "")
                {
                    this.MsgPanel1.ShowMessage("This record cannot be deleted.", MessagePanelControl.MessageEnumType.Alert);
                    LoadData();
                    return;
                }

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                161);

                IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                               161);

                if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                    Response.Redirect(base.UnauthorizedPage(currentLink));

                HtmlInputHidden hidProjectID = (HtmlInputHidden)e.Item.FindControl("hidProjectID");

                if (hidProjectID != null)
                {
                    try
                    {
                        if (session.StatusType != "")
                        {
                            new FinanceDALC().UpdateCompanyProjectShortName(session.CompanyId, hidProjectID.Value, "");
                        }else
                        {
                            new FinanceDALC().DeleteCompanyProject(session.CompanyId, hidProjectID.Value);
                        }
                        
                        this.dgData.EditItemIndex = -1;
                        this.dgData.CurrentPageIndex = 0;
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel1.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgData2_DeleteCommand
        protected void dgData2_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                string currentLink = "CompanyFinance";
                lblPageHeader.Text = "CompanyFinance";

                if (Request.Params["CurrentLink"] != null)
                {
                    currentLink = Request.Params["CurrentLink"].ToString().Trim();
                    if (Request.Params["CurrentLink"].ToString().Trim() == "CompanyFinance")
                        lblPageHeader.Text = "Administration";
                    else
                        lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
                }
                Master.setCurrentLink(currentLink);

                if (session.StatusType != "")
                {
                    this.MsgPanel1.ShowMessage("This record cannot be deleted.", MessagePanelControl.MessageEnumType.Alert);
                    LoadData();
                    return;
                }

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                161);

                IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                               161);

                if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                    Response.Redirect(base.UnauthorizedPage(currentLink));

                HtmlInputHidden hidProjectID = (HtmlInputHidden)e.Item.FindControl("hidDepartmentID");

                if (hidProjectID != null)
                {
                    try
                    {
                        if (session.StatusType != "")
                        {
                            new FinanceDALC().UpdateCompanyDepartmentShortName(session.CompanyId, hidProjectID.Value, "");
                        }
                        else
                        {
                            new FinanceDALC().DeleteCompanyDepartment(session.CompanyId, hidProjectID.Value);
                        }
                        this.dgData2.EditItemIndex = -1;
                        this.dgData2.CurrentPageIndex = 0;
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel2.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgData3_DeleteCommand
        protected void dgData3_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                string currentLink = "CompanyFinance";
                lblPageHeader.Text = "CompanyFinance";

                if (Request.Params["CurrentLink"] != null)
                {
                    currentLink = Request.Params["CurrentLink"].ToString().Trim();
                    if (Request.Params["CurrentLink"].ToString().Trim() == "CompanyFinance")
                        lblPageHeader.Text = "Administration";
                    else
                        lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
                }
                Master.setCurrentLink(currentLink);

                if (session.StatusType != "")
                {
                    this.MsgPanel1.ShowMessage("This record cannot be deleted.", MessagePanelControl.MessageEnumType.Alert);
                    LoadData();
                    return;
                }

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                161);

                IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                               161);

                if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                    Response.Redirect(base.UnauthorizedPage(currentLink));

                HtmlInputHidden hidProjectID = (HtmlInputHidden)e.Item.FindControl("hidSectionID");

                if (hidProjectID != null)
                {
                    try
                    {
                        if (session.StatusType != "")
                        {
                            new FinanceDALC().UpdateCompanySectionShortName(session.CompanyId, hidProjectID.Value, "");
                        }
                        else
                        {
                            new FinanceDALC().DeleteCompanySection(session.CompanyId, hidProjectID.Value);
                        }
                        this.dgData3.EditItemIndex = -1;
                        this.dgData3.CurrentPageIndex = 0;
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel3.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion

        #region dgData4_DeleteCommand
        protected void dgData4_DeleteCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                LogSession session = base.GetSessionInfo();
                string currentLink = "CompanyFinance";
                lblPageHeader.Text = "CompanyFinance";

                if (Request.Params["CurrentLink"] != null)
                {
                    currentLink = Request.Params["CurrentLink"].ToString().Trim();
                    if (Request.Params["CurrentLink"].ToString().Trim() == "CompanyFinance")
                        lblPageHeader.Text = "Administration";
                    else
                        lblPageHeader.Text = Request.Params["CurrentLink"].ToString().Trim();
                }
                Master.setCurrentLink(currentLink);

                if (session.StatusType != "")
                {
                    this.MsgPanel1.ShowMessage("This record cannot be deleted.", MessagePanelControl.MessageEnumType.Alert);
                    LoadData();
                    return;
                }

                UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                                161);

                IList<UserAccessModuleForCompany> uAccessForCompanyList = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByUserIdModuleId(session.CompanyId, session.UserId,
                                                               161);

                if (uAccess == null && (uAccessForCompanyList != null && uAccessForCompanyList.Count == 0))
                    Response.Redirect(base.UnauthorizedPage(currentLink));

                HtmlInputHidden hidUnitID = (HtmlInputHidden)e.Item.FindControl("hidUnitID");

                if (hidUnitID != null)
                {
                    try
                    {
                        if (session.StatusType != "")
                        {
                            new FinanceDALC().UpdateCompanyUnitShortName(session.CompanyId, hidUnitID.Value, "");
                        }
                        else
                        {
                            new FinanceDALC().DeleteCompanyUnit(session.CompanyId, hidUnitID.Value);
                        }
                        this.dgData4.EditItemIndex = -1;
                        this.dgData4.CurrentPageIndex = 0;
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        this.MsgPanel4.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                        return;
                    }
                }
            }
        }
        #endregion
    }
}