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
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.XPath;

using GMSCore;
using GMSCore.Activity;
using GMSCore.Entity;
using GMSWeb.CustomCtrl;

namespace GMSWeb.Admin.Accounts
{
    public partial class UserAccessRights1 : GMSBasePage
    {
        private short userNumId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.setCurrentLink("Admin"); 
            //this.theBody.Attributes.Add("onload", "SafeAddOnload(SetBannerHighLight(" + ((byte)GMSCore.SystemType.Admin).ToString() + "));");

            this.userNumId = GMSUtil.ToShort(Request.QueryString["USERID"]);

            LogSession session = base.GetSessionInfo();
            if (session == null)
            {
                Response.Redirect(base.SessionTimeOutPage("Admin"));
                return;
            }
            UserAccessModule uAccess = new GMSUserActivity().RetrieveUserAccessModuleByUserIdModuleId(session.UserId,
                                                                            16);
            if (uAccess == null)
                Response.Redirect(base.UnauthorizedPage("Admin"));

            if (!Page.IsPostBack && this.userNumId > 0)
            {
                LoadUserDetail();
                PopulateCompanyRepeater();
                PopulateModuleCategoryRepeater();
                PopulateModuleRepeater();
                PopulateReportsRepeater();
                PopulateDocumentsRepeater();
                PopulateDocumentOperationModuleCategoryRepeater();
                LoadCompanyDDL();
            }

            string javaScript = @"
            <script type=""text/javascript"">
	            function toggleCompanyAccessRow(n)
	            {
		            if( document.getElementById(""rppToggleCompany_"" + n) )
		            {
			            var current = document.getElementById(""rppToggleCompany_"" + n).style.display;
			            document.getElementById(""rppToggleCompany_"" + n).style.display = (current == null || current == ""none"")?"""":""none"";
			            document[""imgAccessBoxCompany_"" + n].src = (current == null || current == ""none"")? sDOMAIN+""/App_Themes/Default/images/checkCloseIcon.gif"" : sDOMAIN+""/App_Themes/Default/images/checkOpenIcon.gif"";
		            }
	            }
        		
	            function toggleModCategoryAccessRow(n)
	            {
		            if( document.getElementById(""rppToggleModCategory_"" + n) )
		            {
			            var current = document.getElementById(""rppToggleModCategory_"" + n).style.display;
			            document.getElementById(""rppToggleModCategory_"" + n).style.display = (current == null || current == ""none"")?"""":""none"";
			            document[""imgAccessBoxModCategory_"" + n].src = (current == null || current == ""none"")? sDOMAIN+""/App_Themes/Default/images/checkCloseIcon.gif"" : sDOMAIN+""/App_Themes/Default/images/checkOpenIcon.gif"";
		            }
	            }
        		
	            function toggleModuleAccessRow(n)
	            {
		            if( document.getElementById(""rppToggleModule_"" + n) )
		            {
			            var current = document.getElementById(""rppToggleModule_"" + n).style.display;
			            document.getElementById(""rppToggleModule_"" + n).style.display = (current == null || current == ""none"")?"""":""none"";
			            document[""imgAccessBoxModule_"" + n].src = (current == null || current == ""none"")? sDOMAIN+""/App_Themes/Default/images/checkCloseIcon.gif"" : sDOMAIN+""/App_Themes/Default/images/checkOpenIcon.gif"";
		            }
	            }
        		
	            function toggleReportAccessRow(n)
	            {
		            if( document.getElementById(""rppToggleReport_"" + n) )
		            {
			            var current = document.getElementById(""rppToggleReport_"" + n).style.display;
			            document.getElementById(""rppToggleReport_"" + n).style.display = (current == null || current == ""none"")?"""":""none"";
			            document[""imgAccessBoxReport_"" + n].src = (current == null || current == ""none"")? sDOMAIN+""/App_Themes/Default/images/checkCloseIcon.gif"" : sDOMAIN+""/App_Themes/Default/images/checkOpenIcon.gif"";
		            }
	            }

                function toggleDocumentAccessRow(n)
	            {
		            if( document.getElementById(""rppToggleDocument_"" + n) )
		            {
			            var current = document.getElementById(""rppToggleDocument_"" + n).style.display;
			            document.getElementById(""rppToggleDocument_"" + n).style.display = (current == null || current == ""none"")?"""":""none"";
			            document[""imgAccessBoxDocument_"" + n].src = (current == null || current == ""none"")? sDOMAIN+""/App_Themes/Default/images/checkCloseIcon.gif"" : sDOMAIN+""/App_Themes/Default/images/checkOpenIcon.gif"";
		            }
	            }

                function toggleDocumentOperationAccessRow(n)
	            {
		            if( document.getElementById(""rppToggleDocumentOperationAccess_"" + n) )
		            {
			            var current = document.getElementById(""rppToggleDocumentOperationAccess_"" + n).style.display;
			            document.getElementById(""rppToggleDocumentOperationAccess_"" + n).style.display = (current == null || current == ""none"")?"""":""none"";
			            document[""imgAccessBoxDocumentOperation_"" + n].src = (current == null || current == ""none"")? sDOMAIN+""/App_Themes/Default/images/checkCloseIcon.gif"" : sDOMAIN+""/App_Themes/Default/images/checkOpenIcon.gif"";
		            }
	            }
        		
	            function CheckAll( e , checkListName )
	            {	
		            var b = e.checked;
                    var checkList = $('.'+ checkListName +' input:checkbox');
			        for(i=0; i< checkList.length; i++)
			        {
				        var chk = checkList[i];
				        if( chk )
					        chk.checked = b;					
			       }         
	            }
         
	            function RemoveCheckAll( e , checkAllName)
	            {
		            var unchk = $('#'+checkAllName);
		            if(unchk !=null)
		            {
				        unchk.attr('checked',false);
		            }
	            }

                   function CheckAllEdit( e )
	            {	
		            var p = e.parentElement.parentElement.parentElement;	
		            var b = e.checked;
		            if( p != null )
		            {
			            for(i=2; i<p.childNodes.length; i++)
			            {
				            var chk = p.childNodes[i].childNodes[3].childNodes[0];
				            if( chk != null )
					            chk.checked = b;					
			            }
		            }
	            }


                function RemoveCheckAllEdit( e )
	            {
		            var p = e.parentElement.parentElement.parentElement;
		            if( p != null )
		            {
			            var unchk = p.childNodes[0].childNodes[3].childNodes[0];
			            if( unchk != null )
				            unchk.checked = false;
		            }
	            }
             
	             function CheckOne( e , checkListName )
	            {	
		            var b = e.checked;
                    var checkList = $('.'+ checkListName +' input:checkbox');
			       
				        var chk = checkList;
				        if( chk )
					        chk.checked = b;					
			           
	            }
 
                 function RemoveCheckOne( e )
	            {
		           var unchk = $('#'+checkAllName);
		            if(unchk !=null)
		            {
				        unchk.attr('checked',false);
		            }
	            }

	            </script>"; 

                Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", javaScript);
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
                        hTC.Attributes.Add("class", "rppUserAccessCompany");

                        hTC.InnerHtml = @"<div class=""checkbox""> <input type=""checkbox"" onclick=""RemoveCheckAll(this, 'chkAllCompany')"" id='CoyID_" + GMSUtil.ToStr(company.CoyID) + 
                                @"' name=""CoyID"" value=""" +
                                GMSUtil.ToStr(company.CoyID) +
                                @""" title=""" +
                                GMSUtil.ToStr(company.Name) + @""" ";

                        foreach (UserAccessCompany uaCoy in lstUCoyAccess)
                        {
                            if (uaCoy.CoyID.ToString().Equals(company.CoyID.ToString()))
                            {
                                hTC.InnerHtml += @" checked=""checked"" ";
                                break;
                            }
                        }

                        hTC.InnerHtml += " /><label for='CoyID_"+ GMSUtil.ToStr(company.CoyID) +"'></label></div>";
                        hTR.Cells.Add(hTC);


                        HtmlTableCell hTC3 = new HtmlTableCell();
                        hTR.Cells.Add(hTC3);

                        HtmlTableCell hTC2 = new HtmlTableCell();
                        hTC2.Attributes.Add("class", "rppUserAccessDefaultCompany");
                        hTC2.InnerHtml = @"<div class=""checkbox""> <input type=""checkbox"" onclick=""RemoveCheckOne(this, 'chkDefaultCompany')"" id='DefaultCoyID_" + GMSUtil.ToStr(company.CoyID) +
                                @"' name=""DefaultCoyID"" value=""" +
                                GMSUtil.ToStr(company.CoyID) +
                                @""" title=""" +
                                GMSUtil.ToStr(company.Name) + @""" ";

                        foreach (UserAccessCompany uaCoy in lstUCoyAccess)
                        {
                            if (uaCoy.CoyID.ToString().Equals(company.CoyID.ToString()) && uaCoy.IsDefault)
                            {
                                hTC2.InnerHtml += @" checked=""checked"" ";
                                break;
                            }
                        }

                        hTC2.InnerHtml += " /><label for='DefaultCoyID_" + GMSUtil.ToStr(company.CoyID) + "'></label></div>";
                        hTR.Cells.Add(hTC2);
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
                            HtmlInputHidden hidDefault = (HtmlInputHidden)item.FindControl("hidDefault");


                            if (hidName != null)
                            {
                                string nextrow = hidName.Value;

                                if (nextrow != strPrev)
                                {
                                    strPrev = nextrow;

                                    HtmlTableRow tr = new HtmlTableRow();
                                    HtmlTableCell tc = new HtmlTableCell();

                                    tc.InnerHtml = "<small><b>" + nextrow + "</b></small>";
                                    //tc.Width = "80%";
                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    //tc.Width = "20%";
                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    //tc.Width = "20%";
                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    //tc.Width = "20%";
                                    tr.Cells.Add(tc);

                                    tr.BgColor = "#F5F6F7";
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

                        hTC.InnerHtml = @"<div class='checkbox'><input type=""checkbox"" onclick=""RemoveCheckAll(this, 'chkAllModule')"" id='modCategoryID_" + GMSUtil.ToStr(modCategory.ModuleCategoryID) + 
                                @"' name=""modCategoryID"" value=""" +
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

                        hTC.InnerHtml += " /><label for='modCategoryID_" + GMSUtil.ToStr(modCategory.ModuleCategoryID) + "'></label></div>";
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
                                    tc.Width = "80%";
                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    tc.Width = "20%";

                                    tr.Cells.Add(tc);

                                    tr.BgColor = "#F5F6F7";
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

                            hTC.InnerHtml = @"<div class='checkbox'><input type=""checkbox"" onclick=""RemoveCheckAll(this,'chkAll_" + GMSUtil.ToStr(modCategory.Name).Trim() +
                                    @"')"" id='moduleID_" + GMSUtil.ToStr(module.ModuleID) + 
                                    @"' name=""moduleID"" value=""" +
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

                            hTC.InnerHtml += " /><label for='moduleID_" + GMSUtil.ToStr(module.ModuleID) + "'></label></div>";
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
                                    tc.Width = "80%";
                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    tc.Width = "20%";

                                    tr.Cells.Add(tc);

                                    tr.BgColor = "#F5F6F7";
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

                            hTC.InnerHtml = @"<div class='checkbox'><input type=""checkbox"" onclick=""RemoveCheckAll(this,'chkAll_report')"" id='ReportID_" + GMSUtil.ToStr(dtvView[g]["ReportID"]) +
                                @"' name=""ReportID"" value=""" +
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

                            hTC.InnerHtml += " /><label for='ReportID_" + GMSUtil.ToStr(dtvView[g]["ReportID"])+"'></label></div>";
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
                                    tc.Width = "80%";
                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    tc.Width = "20%";
                                    tr.Cells.Add(tc);

                                    tr.BgColor = "#F5F6F7";
                                    item.Controls.AddAt(0, tr);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region PopulateDocumentsRepeater
        private void PopulateDocumentsRepeater()
        {
            DataTable dtt = new DataTable();

            dtt.Columns.Add("Documents", typeof(string));
            DataRow dr = dtt.NewRow();

            dr["Documents"] = "Documents";
            dtt.Rows.Add(dr);

            rppDocumentsFunctionList.DataSource = dtt;
            rppDocumentsFunctionList.DataBind();


            DataTable dttTable = null;

            for (int i = 0; i < this.rppDocumentsFunctionList.Items.Count; i++)
            {
                #region Pull Data into DataTable
                dttTable = new DataTable();

                dttTable.Columns.Add("DocumentID", typeof(string));
                dttTable.Columns.Add("DocumentCategory", typeof(string));
                dttTable.Columns.Add("DocumentName", typeof(string));

                IList<Document> lstDocumentGroupByCategory = null;
                lstDocumentGroupByCategory = new SystemDataActivity().RetrieveAllDocuments();

                foreach (Document document in lstDocumentGroupByCategory)
                {
                    DataRow drRow = dttTable.NewRow();

                    drRow[0] = document.DocumentID.ToString();
                    drRow[1] = document.DocumentCategoryObject.ModuleCategoryObject.Name.ToString() + " - " + document.DocumentCategoryObject.CategoryName.ToString();
                    drRow[2] = document.DocumentName.ToString();

                    dttTable.Rows.Add(drRow);
                }

                RepeaterItem item = this.rppDocumentsFunctionList.Items[i];
                Repeater rppSystemTable = (Repeater)item.FindControl("rppDocumentsSystemFunctions");

                if (rppSystemTable != null)
                {
                    DataView dtvView = new DataView(dttTable);

                    rppSystemTable.DataSource = dtvView;
                    rppSystemTable.DataBind();

                    #region Get data from tbUserAccessDoument for comparison (checkbox)
                    IList<UserAccessDocument> lstUCoyAccessDocument = null;
                    lstUCoyAccessDocument = new GMSUserActivity().RetrieveUserAccessDocumentByUserId(this.userNumId);
                    #endregion

                    for (int g = 0; g < rppSystemTable.Items.Count; g++)
                    {
                        HtmlTableRow hTR = rppSystemTable.Items[g].FindControl("rppRow") as HtmlTableRow;

                        if (hTR != null)
                        {
                            string roleAccessID = dtvView[g]["DocumentID"].ToString();

                            HtmlTableCell hTC = new HtmlTableCell();

                            hTC.InnerHtml = @"<div class='checkbox'> <input type=""checkbox"" onclick=""RemoveCheckAll(this,'chkAllDocument')"" id='DocumentID_" + GMSUtil.ToStr(dtvView[g]["DocumentID"]) +
                                @"' name=""DocumentID"" value=""" +
                                GMSUtil.ToStr(dtvView[g]["DocumentID"]) +
                                @""" title=""" +
                                GMSUtil.ToStr(dtvView[g]["DocumentName"]) + @""" ";

                            foreach (UserAccessDocument uaDocument in lstUCoyAccessDocument)
                            {
                                if (uaDocument.DocumentID.ToString().Equals(dtvView[g]["DocumentID"].ToString()))
                                {
                                    hTC.InnerHtml += @" checked=""checked"" ";
                                    break;
                                }
                            }

                            hTC.InnerHtml += " /><label for='DocumentID_" + GMSUtil.ToStr(dtvView[g]["DocumentID"]) + "'></label></div>";
                            hTR.Cells.Add(hTC);
                        }
                    }
                }
                #endregion
            }

        }
        #endregion

        #region rppDocumentsFunctionList_PreRender
        protected void rppDocumentsFunctionList_PreRender(object sender, EventArgs e)
        {
            string strPrev = "";

            for (int m = 0; m < this.rppDocumentsFunctionList.Controls.Count; m++)
            {
                RepeaterItem mainItem = (RepeaterItem)this.rppDocumentsFunctionList.Controls[m];

                if (mainItem.ItemType == ListItemType.Item || mainItem.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater rppSystemFunctions = (Repeater)mainItem.FindControl("rppDocumentsSystemFunctions");

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
                                    tc.Width = "80%";
                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    tc.Width = "20%";

                                    tr.Cells.Add(tc);

                                    tr.BgColor = "#F5F6F7";
                                    item.Controls.AddAt(0, tr);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region PopulateDocumentOperationModuleCategoryRepeater
        private void PopulateDocumentOperationModuleCategoryRepeater()
        {
            DataTable dttTable = new DataTable();

            dttTable.Columns.Add("DocumentsOperation", typeof(string));
            DataRow drRow = dttTable.NewRow();

            drRow["DocumentsOperation"] = "Document Operations";
            dttTable.Rows.Add(drRow);

            this.rppDocumentOperationAccessModuleCategoryList.DataSource = dttTable;
            this.rppDocumentOperationAccessModuleCategoryList.DataBind();


            IList<ModuleCategory> lstUserAccessModuleCategory = null;
            lstUserAccessModuleCategory = new SystemDataActivity().RetrieveAllModuleCategoryList();

            if (lstUserAccessModuleCategory != null && lstUserAccessModuleCategory.Count > 0)
            {
                RepeaterItem item = this.rppDocumentOperationAccessModuleCategoryList.Items[0];
                Repeater rppDocumentOperationModuleCategory = (Repeater)item.FindControl("rppDocumentOperationModuleCategory");

                // Bind Data to sub repeater
                rppDocumentOperationModuleCategory.DataSource = lstUserAccessModuleCategory;
                rppDocumentOperationModuleCategory.DataBind();

                #region Get data from tbUserAccessDocumentOperation for comparison (checkbox)
                IList<UserAccessDocumentOperation> lstUCoyAccessDocumentOperation = null;
                lstUCoyAccessDocumentOperation = new GMSUserActivity().RetrieveUserAccessDocumentOperationByUserId(this.userNumId);
                #endregion

                for (int g = 0; g < rppDocumentOperationModuleCategory.Items.Count; g++)
                {
                    ModuleCategory modCategory = (ModuleCategory)lstUserAccessModuleCategory[g];

                    HtmlTableRow hTR = rppDocumentOperationModuleCategory.Items[g].FindControl("rppRow") as HtmlTableRow;

                    if (hTR != null)
                    {
                        HtmlTableCell hTC = new HtmlTableCell();
                        hTC.Attributes.Add("class", "rppDocumentOperationModuleCategory");

                        hTC.InnerHtml = @"<div class='checkbox'><input type=""checkbox"" onclick=""RemoveCheckAll(this,'chkAll_documentOperation')"" id='DocumentOperationViewID_" + GMSUtil.ToStr(modCategory.ModuleCategoryID) +
                                @"' name=""DocumentOperationViewID"" value=""" +
                                GMSUtil.ToStr(modCategory.ModuleCategoryID) +
                                @""" title=""" +
                                GMSUtil.ToStr(modCategory.Name) + @""" ";

                        foreach (UserAccessDocumentOperation uaDocumentOperation in lstUCoyAccessDocumentOperation)
                        {
                            if (uaDocumentOperation.ModuleCategoryID.ToString().Equals(modCategory.ModuleCategoryID.ToString()) && uaDocumentOperation.Operation == "V")
                            {
                                hTC.InnerHtml += @" checked=""checked"" ";
                                break;
                            }
                        }

                        hTC.InnerHtml += " /><label for='DocumentOperationViewID_"+ GMSUtil.ToStr(modCategory.ModuleCategoryID) +"'></label></div>";
                        hTR.Cells.Add(hTC);

                        HtmlTableCell hTC3 = new HtmlTableCell();
                        hTR.Cells.Add(hTC3);

                        HtmlTableCell hTC2 = new HtmlTableCell();
                        hTC2.Attributes.Add("class", "rppDocumentOperationModuleCategoryEdit");
                        hTC2.InnerHtml = @"<div class='checkbox'><input type=""checkbox"" onclick=""RemoveCheckAll(this,'chkAllEdit__documentOperation')"" id='DocumentOperationEditID_" + GMSUtil.ToStr(modCategory.ModuleCategoryID) +
                                @"' name=""DocumentOperationEditID"" value=""" +
                                GMSUtil.ToStr(modCategory.ModuleCategoryID) +
                                @""" title=""" +
                                GMSUtil.ToStr(modCategory.Name) + @""" ";

                        foreach (UserAccessDocumentOperation uaDocumentOperation in lstUCoyAccessDocumentOperation)
                        {
                            if (uaDocumentOperation.ModuleCategoryID.ToString().Equals(modCategory.ModuleCategoryID.ToString()) && uaDocumentOperation.Operation == "E")
                            {
                                hTC2.InnerHtml += @" checked=""checked"" ";
                                break;
                            }
                        }

                        hTC2.InnerHtml += " /><label for='DocumentOperationEditID_"+ GMSUtil.ToStr(modCategory.ModuleCategoryID) +"'></label></div>";
                        hTR.Cells.Add(hTC2);
                    }
                }

            }
        }
        #endregion

        #region rppDocumentOperation_PreRender
        protected void rppDocumentOperation_PreRender(object sender, EventArgs e)
        {
            string strPrev = "";

            for (int m = 0; m < this.rppDocumentOperationAccessModuleCategoryList.Controls.Count; m++)
            {
                RepeaterItem mainItem = (RepeaterItem)this.rppDocumentOperationAccessModuleCategoryList.Controls[m];

                if (mainItem.ItemType == ListItemType.Item || mainItem.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater rppDocumentOperationModuleCategory = (Repeater)mainItem.FindControl("rppDocumentOperationModuleCategory");

                    if (rppDocumentOperationModuleCategory != null)
                    {
                        for (int i = 0; i < rppDocumentOperationModuleCategory.Controls.Count; i++)
                        {
                            RepeaterItem item = (RepeaterItem)rppDocumentOperationModuleCategory.Controls[i];
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
                                    //tc.Width = "50%";
                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    //tc.Width = "50%";

                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    //tc.Width = "50%";

                                    tr.Cells.Add(tc);

                                    tc = new HtmlTableCell();
                                    tc.InnerHtml = "";
                                    //tc.Width = "50%";

                                    tr.Cells.Add(tc);

                                    tr.BgColor = "#F5F6F7";
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
            if (this.ddlCompany.SelectedIndex == 0)
            {
                #region For All
                ArrayList arlCoyId = new ArrayList();
                ArrayList arlIsDefault = new ArrayList();
                ArrayList arlModuleId = new ArrayList();
                ArrayList arlModCategoryId = new ArrayList();
                ArrayList arlReportId = new ArrayList();
                ArrayList arlDocumentId = new ArrayList();
                ArrayList arlOpeModuleCategoryViewId = new ArrayList();
                ArrayList arlOpeModuleCategoryEditId = new ArrayList();
                short DefaultCoyID = 0; 

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
                 
                if (Utilities.Util.CheckStr(Request.Form["IsDefault"]).Trim().Length > 0)
                {
                    string[] strIsDefaults = null;
                    if (Utilities.Util.inString(Request.Form["IsDefault"], ',') == true)
                    {
                        strIsDefaults = Request.Form["IsDefault"].Trim().TrimEnd(',').Split(',');

                        if (strIsDefaults.Length > 0)
                        {
                            for (int i = 0; i < strIsDefaults.Length; i++)
                            {
                                arlIsDefault.Add(strIsDefaults[i].ToString());
                            }
                        }
                    }
                    else
                        arlIsDefault.Add(Request.Form["IsDefault"].Trim().ToString());
                }

                if (Utilities.Util.CheckStr(Request.Form["DefaultCoyID"]).Trim().Length > 0)
                {
                    string[] strDefaultCoyIDs = null;
                    if (Utilities.Util.inString(Request.Form["DefaultCoyID"], ',') == true)
                    {
                        JScriptAlertMsg("You can only select one default! Please reset again");
                        //return;
                    }
                    else
                        DefaultCoyID = GMSUtil.ToShort(Request.Form["DefaultCoyID"].Trim().ToString());
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

                #region get DocumentID access rights
                if (Utilities.Util.CheckStr(Request.Form["DocumentID"]).Trim().Length > 0)
                {
                    string[] strDocumentIDs = null;
                    if (Utilities.Util.inString(Request.Form["DocumentID"], ',') == true)
                    {
                        strDocumentIDs = Request.Form["DocumentID"].Trim().TrimEnd(',').Split(',');

                        if (strDocumentIDs.Length > 0)
                        {
                            for (int i = 0; i < strDocumentIDs.Length; i++)
                            {
                                arlDocumentId.Add(strDocumentIDs[i].ToString());
                            }
                        }
                    }
                    else
                        arlDocumentId.Add(Request.Form["DocumentID"].Trim().ToString());
                }
                #endregion

                #region get document operation access rights
                if (Utilities.Util.CheckStr(Request.Form["DocumentOperationEditID"]).Trim().Length > 0)
                {
                    string[] strModCategoryIDs = null;
                    if (Utilities.Util.inString(Request.Form["DocumentOperationEditID"], ',') == true)
                    {
                        strModCategoryIDs = Request.Form["DocumentOperationEditID"].Trim().TrimEnd(',').Split(',');

                        if (strModCategoryIDs.Length > 0)
                        {
                            for (int i = 0; i < strModCategoryIDs.Length; i++)
                            {
                                arlOpeModuleCategoryEditId.Add(strModCategoryIDs[i].ToString());
                            }
                        }
                    }
                    else
                    {
                        arlOpeModuleCategoryEditId.Add(Request.Form["DocumentOperationEditID"].Trim().ToString());
                    }
                }

                if (Utilities.Util.CheckStr(Request.Form["DocumentOperationViewID"]).Trim().Length > 0)
                {
                    string[] strModCategoryIDs = null;
                    if (Utilities.Util.inString(Request.Form["DocumentOperationViewID"], ',') == true)
                    {
                        strModCategoryIDs = Request.Form["DocumentOperationViewID"].Trim().TrimEnd(',').Split(',');

                        if (strModCategoryIDs.Length > 0)
                        {
                            for (int i = 0; i < strModCategoryIDs.Length; i++)
                            {
                                if (!arlOpeModuleCategoryEditId.Contains(strModCategoryIDs[i].ToString()))
                                    arlOpeModuleCategoryViewId.Add(strModCategoryIDs[i].ToString());
                            }
                        }
                    }
                    else
                    {
                        if (!arlOpeModuleCategoryEditId.Contains(Request.Form["DocumentOperationViewID"].Trim().ToString()))
                            arlOpeModuleCategoryViewId.Add(Request.Form["DocumentOperationViewID"].Trim().ToString());
                    }
                }
                #endregion


                if (this.userNumId > 0)
                {
                    // update access rights'
                    try
                    {
                        ResultType result = new GMSUserActivity().UpdateUserAccessCompany(arlCoyId, arlModuleId, arlModCategoryId, arlReportId, arlDocumentId, arlOpeModuleCategoryViewId, arlOpeModuleCategoryEditId, this.userNumId, DefaultCoyID);
                        switch (result)
                        {
                            case ResultType.Ok:
                                PopulateCompanyRepeater();
                                PopulateModuleCategoryRepeater();
                                PopulateModuleRepeater();
                                PopulateReportsRepeater();
                                PopulateDocumentsRepeater();
                                PopulateDocumentOperationModuleCategoryRepeater();
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
                #endregion
            }
            else
            {
                ArrayList arlModuleId = new ArrayList();
                ArrayList arlReportId = new ArrayList();

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
                        ResultType result = new GMSUserActivity().UpdateUserAccessRightsForCompany(arlModuleId, arlReportId, this.userNumId, GMSUtil.ToShort(ddlCompany.SelectedValue));
                        switch (result)
                        {
                            case ResultType.Ok:
                                PopulateModuleRepeater(GMSUtil.ToShort(this.ddlCompany.SelectedValue));
                                PopulateReportsRepeater(GMSUtil.ToShort(this.ddlCompany.SelectedValue));
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

        #region LoadDDL
        private void LoadCompanyDDL()
        {
            IList<Company> lstCompany = new SystemDataActivity().RetrieveAllAliveCompanyList();
            this.ddlCompany.DataSource = lstCompany;
            this.ddlCompany.DataBind();
            this.ddlCompany.Items.Insert(0, new ListItem("-All-", "0"));
        }
        #endregion

        #region ddlCompany_SelectedIndexChanged
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlCompany.SelectedIndex == 0)
            {
                trCompany.Visible = true;
                trModCategory.Visible = true;
                trDocument.Visible = true;
                trDocumentOperation.Visible = true;

                PopulateCompanyRepeater();
                PopulateModuleCategoryRepeater();
                PopulateModuleRepeater();
                PopulateReportsRepeater();
                PopulateDocumentsRepeater();
                PopulateDocumentOperationModuleCategoryRepeater();
            }
            else
            {
                trCompany.Visible = false;
                trModCategory.Visible = false;
                trDocument.Visible = false;
                trDocumentOperation.Visible = false;

                PopulateModuleRepeater(GMSUtil.ToShort(this.ddlCompany.SelectedValue));
                PopulateReportsRepeater(GMSUtil.ToShort(this.ddlCompany.SelectedValue));
            }
        }
        #endregion

        #region PopulateModuleRepeater
        private void PopulateModuleRepeater(short coyID)
        {
            IList<ModuleCategory> lstUserAccessModuleCategory = null;
            lstUserAccessModuleCategory = new SystemDataActivity().RetrieveAllModuleCategoryList();

            this.rppModule.DataSource = lstUserAccessModuleCategory;
            //this.rppModule.DataBind();

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

                    #region Get data from tbUserAccessModuleForCompany for comparison (checkbox)
                    IList<UserAccessModuleForCompany> lstUCoyAccessModule = null;
                    lstUCoyAccessModule = new GMSUserActivity().RetrieveUserAccessModuleForCompanyByCoyIDUserId(coyID, this.userNumId);
                    #endregion

                    for (int g = 0; g < rppUserAccessModule.Items.Count; g++)
                    {
                        VwModuleListing module = (VwModuleListing)lstModuleListingByParentModuleName[g];

                        HtmlTableRow hTR = rppUserAccessModule.Items[g].FindControl("rppRow") as HtmlTableRow;

                        if (hTR != null)
                        {
                            HtmlTableCell hTC = new HtmlTableCell();

                            hTC.InnerHtml = @"<div class='checkbox'><input type=""checkbox"" onclick=""RemoveCheckAll(this,'chkAll_" + GMSUtil.ToStr(modCategory.Name).Trim() +
                                    @"')"" id='moduleID_" + GMSUtil.ToStr(module.ModuleID) +
                                    @"' name=""moduleID"" value=""" +
                                    GMSUtil.ToStr(module.ModuleID) +
                                    @""" title=""" +
                                    GMSUtil.ToStr(module.FunctionName) 
                                    + @""" ";

                            foreach (UserAccessModuleForCompany uaModule in lstUCoyAccessModule)
                            {
                                if (uaModule.ModuleID.ToString().Equals(module.ModuleID.ToString()))
                                {
                                    hTC.InnerHtml += @" checked=""checked"" ";
                                    break;
                                }
                            }

                            hTC.InnerHtml += " /><label for='moduleID_" + GMSUtil.ToStr(module.ModuleID) + "'></label></div>";
                            hTR.Cells.Add(hTC);
                        }
                    }
                }
                #endregion
            }
       
        }
        #endregion

        #region PopulateReportsRepeater
        private void PopulateReportsRepeater(short coyID)
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
                    IList<UserAccessReportForCompany> lstUCoyAccessReport = null;
                    lstUCoyAccessReport = new GMSUserActivity().RetrieveUserAccessReportForCompanyByCoyIDUserId(coyID, this.userNumId);
                    #endregion

                    for (int g = 0; g < rppSystemTable.Items.Count; g++)
                    {
                        HtmlTableRow hTR = rppSystemTable.Items[g].FindControl("rppRow") as HtmlTableRow;

                        if (hTR != null)
                        {
                            string roleAccessID = dtvView[g]["ReportID"].ToString();

                            HtmlTableCell hTC = new HtmlTableCell();

                            hTC.InnerHtml = @"<div class='checkbox'><input type=""checkbox"" onclick=""RemoveCheckAll(this,'chkAll_report')"" id='ReportID_" + GMSUtil.ToStr(dtvView[g]["ReportID"]) +
                                @"' name=""ReportID"" value=""" +
                                GMSUtil.ToStr(dtvView[g]["ReportID"]) +
                                @""" title=""" +
                                GMSUtil.ToStr(dtvView[g]["ReportTitle"]) + @""" ";

                            foreach (UserAccessReportForCompany uaReport in lstUCoyAccessReport)
                            {
                                if (uaReport.ReportID.ToString().Equals(dtvView[g]["ReportID"].ToString()))
                                {
                                    hTC.InnerHtml += @" checked=""checked"" ";
                                    break;
                                }
                            }

                            hTC.InnerHtml += " /><label for='ReportID_" + GMSUtil.ToStr(dtvView[g]["ReportID"]) +"'></label></div>";
                            hTR.Cells.Add(hTC);
                        }
                    }
                }
                #endregion
            }

        }
        #endregion
    }
}
