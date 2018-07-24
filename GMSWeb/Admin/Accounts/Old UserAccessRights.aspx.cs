using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Admin.Accounts
{
    public partial class UserAccessRights : GMSBasePage
    {
        private short userNumId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.theBody.Attributes.Add("onload", "SafeAddOnload(SetBannerHighLight(" + ((byte)GMSCore.SystemType.Admin).ToString() + "));");

            this.userNumId = GMSUtil.ToShort(Request.QueryString["USERID"]);

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect("../../SessionTimeout.htm");
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            16);
            if (uAccess == null)
                Response.Redirect("../../Unauthorized.htm");

            if (!Page.IsPostBack && this.userNumId > 0)
            {
                LoadUserDetail();
                PopulateCompanyRepeater();
                PopulateModuleCategoryRepeater();
                PopulateModuleRepeater();
                PopulateReportsRepeater();
            }

        }
        #region LoadUserDetail
        protected void LoadUserDetail()
        {
            LogSession session = base.GetSessionInfo();
            GMSUserActivity userActivity = new GMSUserActivity();
            GMSUser user = null;

            try
            {
                user = userActivity.RetrieveUserById(this.userNumId, session);
            }
            catch (Exception ex)
            {
                this.PageMsgPanel.ShowMessage(ex.Message, MessagePanelControl.MessageEnumType.Alert);
                return;
            }

            Member memUser = Member.RetrieveByKey(user.UserId);

            this.lblUserRealName.Text = user.UserRealName.ToString();
            this.lblLoginID.Text = user.UserName.ToString();
            this.lblEmail.Text = user.UserEmail.ToString();
            this.lblActive.Text = (bool)memUser.IsApproved ? "Yes" : "No";
        }
        #endregion

        #region PopulateCompanyRepeater
        private void PopulateCompanyRepeater()
        {
            DataTable dttTable = new DataTable();

            dttTable.Columns.Add("CompanyTitle", typeof(string));
            DataRow drRow = dttTable.NewRow();

            drRow["CompanyTitle"] = "Company";
            dttTable.Rows.Add(drRow);

            this.rppCompanyList.DataSource = dttTable;
            this.rppCompanyList.DataBind();


            IList<Company> lstUserAccessCompany = null;
            lstUserAccessCompany = new SystemDataActivity().RetrieveAllCompanyList();

            if (lstUserAccessCompany != null && lstUserAccessCompany.Count > 0)
            {
                RepeaterItem item = this.rppCompanyList.Items[0];
                Repeater rppUserAccessCompany = (Repeater)item.FindControl("rppUserAccessCompany");

                // Bind Data to sub repeater
                rppUserAccessCompany.DataSource = lstUserAccessCompany;
                rppUserAccessCompany.DataBind();

                #region Get data from tbUserAccessCompany for comparison (checkbox)
                IList<UserAccessCompany> lstUCoyAccess = null;
                lstUCoyAccess = new GMSUserActivity().RetrieveUserAccessCompanyByUserId(this.userNumId);
                #endregion 

                for (int g = 0; g < rppUserAccessCompany.Items.Count; g++)
                {
                    Company company = (Company)lstUserAccessCompany[g];

                    HtmlTableRow hTR = rppUserAccessCompany.Items[g].FindControl("rppRow") as HtmlTableRow;

                    if (hTR != null)
                    {
                        HtmlTableCell hTC = new HtmlTableCell();

                        hTC.InnerHtml = @"<input type=""checkbox"" onclick=""RemoveCheckAll(this)"" id=""CoyID"" name=""CoyID"" value=""" +
                                GMSUtil.ToStr(company.Id) +
                                @""" title=""" +
                                GMSUtil.ToStr(company.Name) + @""" ";

                        foreach(UserAccessCompany uaCoy in lstUCoyAccess)
                        {
                            if (uaCoy.CoyID.ToString().Equals(company.Id.ToString()))
                            {
                                hTC.InnerHtml += @" checked=""checked"" ";
                                break;
                            }
                        }

                        hTC.InnerHtml += " />";
                        hTR.Cells.Add(hTC);
                    }
                }

            }
        }
        #endregion

        #region rppCompanyList_PreRender
        protected void rppCompanyList_PreRender(object sender, EventArgs e)
        {
            string strPrev = "";

            for (int m = 0; m < this.rppCompanyList.Controls.Count; m++)
            {
                RepeaterItem mainItem = (RepeaterItem)this.rppCompanyList.Controls[m];

                if (mainItem.ItemType == ListItemType.Item || mainItem.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater rppUserAccessCompany = (Repeater)mainItem.FindControl("rppUserAccessCompany");

                    if (rppUserAccessCompany != null)
                    {
                        for (int i = 0; i < rppUserAccessCompany.Controls.Count; i++)
                        {
                            RepeaterItem item = (RepeaterItem)rppUserAccessCompany.Controls[i];
                            HtmlInputHidden hidName = (HtmlInputHidden)item.FindControl("hidName");

                            if (hidName != null)
                            {
                                string nextrow = hidName.Value;

                                if (nextrow != strPrev)
                                {
                                    strPrev = nextrow;

                                    HtmlTableRow tr = new HtmlTableRow();
                                    HtmlTableCell tc = new HtmlTableCell();

                                    tc.InnerHtml = "<small><b>" + nextrow + "</b></small>";
                                    tc.Width = "50%";
                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    tc.Width = "50%";

                                    tr.Cells.Add(tc);

                                    tr.BgColor = "#E0EDEF";
                                    item.Controls.AddAt(0, tr);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion


        #region PopulateModuleCategoryRepeater
        private void PopulateModuleCategoryRepeater()
        {
            DataTable dttTable = new DataTable();

            dttTable.Columns.Add("ModCategoryTitle", typeof(string));
            DataRow drRow = dttTable.NewRow();

            drRow["ModCategoryTitle"] = "Module Category";
            dttTable.Rows.Add(drRow);

            this.rppModCategory.DataSource = dttTable;
            this.rppModCategory.DataBind();


            IList<ModuleCategory> lstUserAccessModuleCategory = null;
            lstUserAccessModuleCategory = new SystemDataActivity().RetrieveAllModuleCategoryList();

            if (lstUserAccessModuleCategory != null && lstUserAccessModuleCategory.Count > 0)
            {
                RepeaterItem item = this.rppModCategory.Items[0];
                Repeater rppUserAccessModCategory = (Repeater)item.FindControl("rppUserAccessModCategory");

                // Bind Data to sub repeater
                rppUserAccessModCategory.DataSource = lstUserAccessModuleCategory;
                rppUserAccessModCategory.DataBind();

                #region Get data from tbUserAccessModuleCategory for comparison (checkbox)
                IList<UserAccessModuleCategory> lstUCoyAccessModCategory = null;
                lstUCoyAccessModCategory = new GMSUserActivity().RetrieveUserAccessModuleCategoryByUserId(this.userNumId);
                #endregion 

                for (int g = 0; g < rppUserAccessModCategory.Items.Count; g++)
                {
                    ModuleCategory modCategory = (ModuleCategory)lstUserAccessModuleCategory[g];

                    HtmlTableRow hTR = rppUserAccessModCategory.Items[g].FindControl("rppRow") as HtmlTableRow;

                    if (hTR != null)
                    {
                        HtmlTableCell hTC = new HtmlTableCell();

                        hTC.InnerHtml = @"<input type=""checkbox"" onclick=""RemoveCheckAll(this)"" id=""modCategoryID"" name=""modCategoryID"" value=""" +
                                GMSUtil.ToStr(modCategory.ModuleCategoryID) +
                                @""" title=""" +
                                GMSUtil.ToStr(modCategory.Name) + @""" ";

                        foreach (UserAccessModuleCategory uaModCategory in lstUCoyAccessModCategory)
                        {
                            if (uaModCategory.ModuleCategoryID.ToString().Equals(modCategory.ModuleCategoryID.ToString()))
                            {
                                hTC.InnerHtml += @" checked=""checked"" ";
                                break;
                            }
                        }

                        hTC.InnerHtml += " />";
                        hTR.Cells.Add(hTC);
                    }
                }

            }
        }
        #endregion

        #region rppModCategory_PreRender
        protected void rppModCategory_PreRender(object sender, EventArgs e)
        {
            string strPrev = "";

            for (int m = 0; m < this.rppModCategory.Controls.Count; m++)
            {
                RepeaterItem mainItem = (RepeaterItem)this.rppModCategory.Controls[m];

                if (mainItem.ItemType == ListItemType.Item || mainItem.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater rppUserAccessModCategory = (Repeater)mainItem.FindControl("rppUserAccessModCategory");

                    if (rppUserAccessModCategory != null)
                    {
                        for (int i = 0; i < rppUserAccessModCategory.Controls.Count; i++)
                        {
                            RepeaterItem item = (RepeaterItem)rppUserAccessModCategory.Controls[i];
                            HtmlInputHidden hidName = (HtmlInputHidden)item.FindControl("hidName");

                            if (hidName != null)
                            {
                                string nextrow = hidName.Value;

                                if (nextrow != strPrev)
                                {
                                    strPrev = nextrow;

                                    HtmlTableRow tr = new HtmlTableRow();
                                    HtmlTableCell tc = new HtmlTableCell();

                                    tc.InnerHtml = "<small><b>" + nextrow + "</b></small>";
                                    tc.Width = "50%";
                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    tc.Width = "50%";

                                    tr.Cells.Add(tc);

                                    tr.BgColor = "#E0EDEF";
                                    item.Controls.AddAt(0, tr);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion


        #region PopulateModuleRepeater
        private void PopulateModuleRepeater()
        {
            IList<ModuleCategory> lstUserAccessModuleCategory = null;
            lstUserAccessModuleCategory = new SystemDataActivity().RetrieveAllModuleCategoryList();

            this.rppModule.DataSource = lstUserAccessModuleCategory;
            this.rppModule.DataBind();

            DataTable dttTable = null;

            for (int i = 0; i < this.rppModule.Items.Count; i++)
            {
                ModuleCategory modCategory = (ModuleCategory)lstUserAccessModuleCategory[i];

                #region Pull Data into DataTable
                dttTable = new DataTable();

                dttTable.Columns.Add("ModuleID", typeof(string));
                dttTable.Columns.Add("ParentModuleName", typeof(string));
                dttTable.Columns.Add("FunctionName", typeof(string));

                IList<VwModuleListing> lstModuleListingByParentModuleName = null;
                lstModuleListingByParentModuleName = new SystemDataActivity().RetrieveAllModuleListingByParentModuleName(modCategory.ModuleCategoryID);

                foreach (VwModuleListing module in lstModuleListingByParentModuleName)
                {
                    DataRow drRow = dttTable.NewRow();

                    drRow[0] = module.ModuleID.ToString();
                    drRow[1] = module.ParentModuleName.ToString();
                    drRow[2] = module.FunctionName.ToString();

                    dttTable.Rows.Add(drRow);
                }

                RepeaterItem item = this.rppModule.Items[i];
                Repeater rppUserAccessModule = (Repeater)item.FindControl("rppUserAccessModule");

                if (rppUserAccessModule != null)
                {
                    DataView dtvView = new DataView(dttTable);

                    rppUserAccessModule.DataSource = dtvView;
                    rppUserAccessModule.DataBind();

                    #region Get data from tbUserAccessModule for comparison (checkbox)
                    IList<UserAccessModule> lstUCoyAccessModule = null;
                    lstUCoyAccessModule = new GMSUserActivity().RetrieveUserAccessModuleByUserId(this.userNumId);
                    #endregion 

                    for (int g = 0; g < rppUserAccessModule.Items.Count; g++)
                    {
                        VwModuleListing module = (VwModuleListing)lstModuleListingByParentModuleName[g];

                        HtmlTableRow hTR = rppUserAccessModule.Items[g].FindControl("rppRow") as HtmlTableRow;

                        if (hTR != null)
                        {
                            HtmlTableCell hTC = new HtmlTableCell();

                            hTC.InnerHtml = @"<input type=""checkbox"" onclick=""RemoveCheckAll(this)"" id=""moduleID"" name=""moduleID"" value=""" +
                                    GMSUtil.ToStr(module.ModuleID) +
                                    @""" title=""" +
                                    GMSUtil.ToStr(module.FunctionName) + @""" ";

                            foreach (UserAccessModule uaModule in lstUCoyAccessModule)
                            {
                                if (uaModule.ModuleID.ToString().Equals(module.ModuleID.ToString()))
                                {
                                    hTC.InnerHtml += @" checked=""checked"" ";
                                    break;
                                }
                            }

                            hTC.InnerHtml += " />";
                            hTR.Cells.Add(hTC);
                        }
                    }
                }
                #endregion
            }
        }
        #endregion

        #region rppModule_PreRender
        protected void rppModule_PreRender(object sender, EventArgs e)
        {
            string strPrev = "";

            for (int m = 0; m < this.rppModule.Controls.Count; m++)
            {
                RepeaterItem mainItem = (RepeaterItem)this.rppModule.Controls[m];

                if (mainItem.ItemType == ListItemType.Item || mainItem.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater rppUserAccessModule = (Repeater)mainItem.FindControl("rppUserAccessModule");

                    if (rppUserAccessModule != null)
                    {
                        for (int i = 0; i < rppUserAccessModule.Controls.Count; i++)
                        {
                            RepeaterItem item = (RepeaterItem)rppUserAccessModule.Controls[i];
                            HtmlInputHidden hidName = (HtmlInputHidden)item.FindControl("hidName");

                            if (hidName != null)
                            {
                                string nextrow = hidName.Value;

                                if (nextrow != strPrev)
                                {
                                    strPrev = nextrow;

                                    HtmlTableRow tr = new HtmlTableRow();
                                    HtmlTableCell tc = new HtmlTableCell();

                                    tc.InnerHtml = "<small><b>" + nextrow + "</b></small>";
                                    tc.Width = "50%";
                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    tc.Width = "50%";

                                    tr.Cells.Add(tc);

                                    tr.BgColor = "#E0EDEF";
                                    item.Controls.AddAt(0, tr);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion


        #region PopulateReportsRepeater
        private void PopulateReportsRepeater()
        {
            DataTable dtt = new DataTable();

            dtt.Columns.Add("Reports", typeof(string));
            DataRow dr = dtt.NewRow();

            dr["Reports"] = "Reports";
            dtt.Rows.Add(dr);

            rppReportsFunctionList.DataSource = dtt;
            rppReportsFunctionList.DataBind();


            DataTable dttTable = null;

            for (int i = 0; i < this.rppReportsFunctionList.Items.Count; i++)
            {
                #region Pull Data into DataTable
                dttTable = new DataTable();

                dttTable.Columns.Add("ReportID", typeof(string));
                dttTable.Columns.Add("ReportCategory", typeof(string));
                dttTable.Columns.Add("ReportTitle", typeof(string));

                IList<Report> lstReportGroupByCategory = null;
                lstReportGroupByCategory = new ReportsActivity().RetrieveReportGroupByCategory();

                foreach (Report report in lstReportGroupByCategory)
                {
                    DataRow drRow = dttTable.NewRow();

                    drRow[0] = report.ReportID.ToString();
                    drRow[1] = report.ReportCategoryObject.Name.ToString();
                    drRow[2] = report.Description.ToString();

                    dttTable.Rows.Add(drRow);
                }

                RepeaterItem item = this.rppReportsFunctionList.Items[i];
                Repeater rppSystemTable = (Repeater)item.FindControl("rppReportsSystemFunctions");

                if (rppSystemTable != null)
                {
                    DataView dtvView = new DataView(dttTable);

                    rppSystemTable.DataSource = dtvView;
                    rppSystemTable.DataBind();

                    #region Get data from tbUserAccessReport for comparison (checkbox)
                    IList<UserAccessReport> lstUCoyAccessReport = null;
                    lstUCoyAccessReport = new GMSUserActivity().RetrieveUserAccessReportByUserId(this.userNumId);
                    #endregion 

                    for (int g = 0; g < rppSystemTable.Items.Count; g++)
                    {
                        HtmlTableRow hTR = rppSystemTable.Items[g].FindControl("rppRow") as HtmlTableRow;

                        if (hTR != null)
                        {
                            string roleAccessID = dtvView[g]["ReportID"].ToString();

                            HtmlTableCell hTC = new HtmlTableCell();

                            hTC.InnerHtml = @"<input type=""checkbox"" onclick=""RemoveCheckAll(this)"" id=""ReportID"" name=""ReportID"" value=""" +
                                GMSUtil.ToStr(dtvView[g]["ReportID"]) +
                                @""" title=""" +
                                GMSUtil.ToStr(dtvView[g]["ReportTitle"]) + @""" ";

                            foreach (UserAccessReport uaReport in lstUCoyAccessReport)
                            {
                                if (uaReport.ReportID.ToString().Equals(dtvView[g]["ReportID"].ToString()))
                                {
                                    hTC.InnerHtml += @" checked=""checked"" ";
                                    break;
                                }
                            }

                            hTC.InnerHtml += " />";
                            hTR.Cells.Add(hTC);
                        }
                    }
                }
                #endregion
            }

        }
        #endregion

        #region rppReportsFunctionList_PreRender
        protected void rppReportsFunctionList_PreRender(object sender, EventArgs e)
        {
            string strPrev = "";

            for (int m = 0; m < this.rppReportsFunctionList.Controls.Count; m++)
            {
                RepeaterItem mainItem = (RepeaterItem)this.rppReportsFunctionList.Controls[m];

                if (mainItem.ItemType == ListItemType.Item || mainItem.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater rppSystemFunctions = (Repeater)mainItem.FindControl("rppReportsSystemFunctions");

                    if (rppSystemFunctions != null)
                    {
                        for (int i = 0; i < rppSystemFunctions.Controls.Count; i++)
                        {
                            RepeaterItem item = (RepeaterItem)rppSystemFunctions.Controls[i];
                            HtmlInputHidden hidName = (HtmlInputHidden)item.FindControl("hidName");

                            if (hidName != null)
                            {
                                string nextrow = hidName.Value;

                                if (nextrow != strPrev)
                                {
                                    strPrev = nextrow;

                                    HtmlTableRow tr = new HtmlTableRow();
                                    HtmlTableCell tc = new HtmlTableCell();

                                    tc.InnerHtml = "<small><b>" + nextrow + "</b></small>";
                                    tc.Width = "50%";
                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    tc.Width = "50%";

                                    tr.Cells.Add(tc);

                                    tr.BgColor = "#E0EDEF";
                                    item.Controls.AddAt(0, tr);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ArrayList arlCoyId = new ArrayList();
            ArrayList arlModuleId = new ArrayList();
            ArrayList arlModCategoryId = new ArrayList();
            ArrayList arlReportId = new ArrayList();

            #region get CoyID access rights
            if (Utilities.Util.CheckStr(Request.Form["CoyID"]).Trim().Length > 0)
            {
                string[] strCoyIDs = null;
                if (Utilities.Util.inString(Request.Form["CoyID"], ',') == true)
                {
                    strCoyIDs = Request.Form["CoyID"].Trim().TrimEnd(',').Split(',');

                    if (strCoyIDs.Length > 0)
                    {
                        for (int i = 0; i < strCoyIDs.Length; i++)
                        {
                            arlCoyId.Add(strCoyIDs[i].ToString());
                        }
                    }
                }
                else
                    arlCoyId.Add(Request.Form["CoyID"].Trim().ToString());
            }
            #endregion

            #region get ModuleCategoryID access rights
            if (Utilities.Util.CheckStr(Request.Form["ModCategoryID"]).Trim().Length > 0)
            {
                string[] strModCategoryIDs = null;
                if (Utilities.Util.inString(Request.Form["ModCategoryID"], ',') == true)
                {
                    strModCategoryIDs = Request.Form["ModCategoryID"].Trim().TrimEnd(',').Split(',');

                    if (strModCategoryIDs.Length > 0)
                    {
                        for (int i = 0; i < strModCategoryIDs.Length; i++)
                        {
                            arlModCategoryId.Add(strModCategoryIDs[i].ToString());
                        }
                    }
                }
                else
                    arlModCategoryId.Add(Request.Form["ModCategoryID"].Trim().ToString());
            }
            #endregion

            #region get ModuleID access rights
            if (Utilities.Util.CheckStr(Request.Form["ModuleID"]).Trim().Length > 0)
            {
                string[] strModuleIDs = null;
                if (Utilities.Util.inString(Request.Form["ModuleID"], ',') == true)
                {
                    strModuleIDs = Request.Form["ModuleID"].Trim().TrimEnd(',').Split(',');

                    if (strModuleIDs.Length > 0)
                    {
                        for (int i = 0; i < strModuleIDs.Length; i++)
                        {
                            arlModuleId.Add(strModuleIDs[i].ToString());
                        }
                    }
                }
                else
                    arlModuleId.Add(Request.Form["ModuleID"].Trim().ToString());
            }
            #endregion

            #region get ReportID access rights
            if (Utilities.Util.CheckStr(Request.Form["ReportID"]).Trim().Length > 0)
            {
                string[] strReportIDs = null;
                if (Utilities.Util.inString(Request.Form["ReportID"], ',') == true)
                {
                    strReportIDs = Request.Form["ReportID"].Trim().TrimEnd(',').Split(',');

                    if (strReportIDs.Length > 0)
                    {
                        for (int i = 0; i < strReportIDs.Length; i++)
                        {
                            arlReportId.Add(strReportIDs[i].ToString());
                        }
                    }
                }
                else
                    arlReportId.Add(Request.Form["ReportID"].Trim().ToString());
            }
            #endregion


            if (this.userNumId > 0)
            {
                // update access rights'
                try
                {
                    ResultType result = new GMSUserActivity().UpdateUserAccessCompany(arlCoyId, arlModuleId, arlModCategoryId, arlReportId, this.userNumId);
                    switch (result)
                    {
                        case ResultType.Ok:
                            PopulateCompanyRepeater();
                            PopulateModuleCategoryRepeater();
                            PopulateModuleRepeater();
                            PopulateReportsRepeater();
                            this.PageMsgPanel.ShowMessage("User Access Rights successfully updated.", MessagePanelControl.MessageEnumType.Info);
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
}
