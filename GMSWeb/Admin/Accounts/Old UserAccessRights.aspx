<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Old UserAccessRights.aspx.cs" Inherits="GMSWeb.Admin.Accounts.UserAccessRights" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title>Leeden Group Management System</title>
	<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
	<script type="text/javascript">
		//<![CDATA[
		function toggleCompanyAccessRow(n)
		{
			if( document.getElementById("rppToggleCompany_" + n) )
			{
				var current = document.getElementById("rppToggleCompany_" + n).style.display;
				document.getElementById("rppToggleCompany_" + n).style.display = (current == null || current == "none")?"":"none";
				document["imgAccessBoxCompany_" + n].src = (current == null || current == "none")? sDOMAIN+"/App_Themes/Default/images/checkCloseIcon.gif" : sDOMAIN+"/App_Themes/Default/images/checkOpenIcon.gif";
			}
		}
		
		function toggleModCategoryAccessRow(n)
		{
			if( document.getElementById("rppToggleModCategory_" + n) )
			{
				var current = document.getElementById("rppToggleModCategory_" + n).style.display;
				document.getElementById("rppToggleModCategory_" + n).style.display = (current == null || current == "none")?"":"none";
				document["imgAccessBoxModCategory_" + n].src = (current == null || current == "none")? sDOMAIN+"/App_Themes/Default/images/checkCloseIcon.gif" : sDOMAIN+"/App_Themes/Default/images/checkOpenIcon.gif";
			}
		}
		
		function toggleModuleAccessRow(n)
		{
			if( document.getElementById("rppToggleModule_" + n) )
			{
				var current = document.getElementById("rppToggleModule_" + n).style.display;
				document.getElementById("rppToggleModule_" + n).style.display = (current == null || current == "none")?"":"none";
				document["imgAccessBoxModule_" + n].src = (current == null || current == "none")? sDOMAIN+"/App_Themes/Default/images/checkCloseIcon.gif" : sDOMAIN+"/App_Themes/Default/images/checkOpenIcon.gif";
			}
		}
		
		function toggleReportAccessRow(n)
		{
			if( document.getElementById("rppToggleReport_" + n) )
			{
				var current = document.getElementById("rppToggleReport_" + n).style.display;
				document.getElementById("rppToggleReport_" + n).style.display = (current == null || current == "none")?"":"none";
				document["imgAccessBoxReport_" + n].src = (current == null || current == "none")? sDOMAIN+"/App_Themes/Default/images/checkCloseIcon.gif" : sDOMAIN+"/App_Themes/Default/images/checkOpenIcon.gif";
			}
		}
		
		function CheckAll( e )
		{	
			var p = e.parentElement.parentElement.parentElement;	
			var b = e.checked;
			if( p != null )
			{
				for(i=2; i<p.childNodes.length; i++)
				{
					var chk = p.childNodes[i].childNodes[1].childNodes[0];
					if( chk != null )
						chk.checked = b;					
				}
			}
		}
		function RemoveCheckAll( e )
		{
			var p = e.parentElement.parentElement.parentElement;
			if( p != null )
			{
				var unchk = p.childNodes[0].childNodes[1].childNodes[0];
				if( unchk != null )
					unchk.checked = false;
			}
		}
		//]]>
		</script>
</head>
<body ID="theBody" runat="server" style="background: none; text-align: left; font: 11px/1.6em  Verdana, Tahoma, Arial,sans-serif;">
    <form id="form1" runat="server">
        <div>
        <h1>System &gt; Access Rights</h1>
        <p>Modify user access rights.</p>
        <table>
	        <TR>
				<TD>
					<TABLE class="tTable1" width="550px">
						<TR>
							<TD class="tbLabel">User Name</TD>
							<TD class="tbColon">:</TD>
							<TD>    
							    <asp:Label ID="lblUserRealName" runat="server"></asp:Label>
							</TD>
						</TR>
						<TR>
							<TD class="tbLabel">Login ID</TD>
							<TD class="tbColon">:</TD>
							<TD>
							    <asp:Label ID="lblLoginID" runat="server"></asp:Label>
						    </TD>
						</TR>
						<TR>
							<TD class="tbLabel">Email</TD>
							<TD class="tbColon">:</TD>
							<TD>
							    <asp:Label ID="lblEmail" runat="server"></asp:Label>
						    </TD>
						</TR>
						<TR>
							<TD class="tbLabel" style="height: 21px">Active</TD>
							<TD class="tbColon" style="height: 21px">:</TD>
							<TD style="height: 21px">
							    <asp:Label ID="lblActive" runat="server"></asp:Label>
						    </TD>
						</TR>
					</TABLE>
				</TD>
			</TR>
		    <tr><td>&nbsp;</td></tr>
		    <tr valign="top">
			    <td style="width: 70%;">
			    <b>Company Access Rights</b>
    			<asp:Repeater id="rppCompanyList" Runat="server" OnPreRender="rppCompanyList_PreRender">
					<HeaderTemplate>
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
					</HeaderTemplate>
					<ItemTemplate>
						<tr class="tHeader">
							<td style="padding:4px;">
								<a href="javascript:toggleCompanyAccessRow(<%# Container.ItemIndex %>);" title="Display/Hide Access Functions">
									<img src="<%= Request.ApplicationPath %>/App_Themes/Default/images/checkOpenIcon.gif" alt="Expand/Hide" 
									    name="imgAccessBoxCompany_<%# Container.ItemIndex%>" /></a><%# DataBinder.Eval(Container.DataItem, "CompanyTitle")%>
							</td>
						</tr>
						<tr id="rppToggleCompany_<%# Container.ItemIndex %>" style="display:none">
							<td>
								<asp:repeater id="rppUserAccessCompany" runat="server">
									<headertemplate>
										<table cellpadding="2" cellspacing="1" border="0" width="100%">
											<tr bgcolor="#E0EDEF">
												<td align="right"><i>SelectAll</i></td>
												<td><input type="checkbox" id="chkAll" onclick="CheckAll(this)" name="chkAll" title="Select All"
														runat="server" /></td>
											</tr>
									</headertemplate>
									<itemtemplate>
										<tr id="rppRow" runat="server">
											<td style="padding-left:20px;">
												<input type="hidden" name="hidCoyId" id="hidCoyId" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Id")%>' />
												<input type="hidden" name="hidName" id="hidName" runat="server" value="Company List" />
												<small>
													<%# DataBinder.Eval(Container.DataItem,"Code") + " - " + DataBinder.Eval(Container.DataItem,"Name")%>
												</small>
											</td>
										</tr>
									</itemtemplate>
									<footertemplate>
									    </TABLE>
								    </footertemplate>
							    </asp:repeater>
					        </td>
				        </tr>
				    </ItemTemplate>
			
				    <FooterTemplate>
					    </table>
				    </FooterTemplate>
				</asp:Repeater>
			    
			    <br />
			    </td>
			    <td style="width: 30%;">
			    </td>
		    </tr>
		    
		    <tr valign="top">
			    <td style="width: 70%;">
			    <b>Module Category Access Rights</b>
    			<asp:Repeater id="rppModCategory" Runat="server" OnPreRender="rppModCategory_PreRender">
					<HeaderTemplate>
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
					</HeaderTemplate>
					<ItemTemplate>
						<tr class="tHeader">
							<td style="padding:4px;">
								<a href="javascript:toggleModCategoryAccessRow(<%# Container.ItemIndex %>);" title="Display/Hide Access Functions">
									<img src="<%= Request.ApplicationPath %>/App_Themes/Default/images/checkOpenIcon.gif" alt="Expand/Hide" 
									    name="imgAccessBoxModCategory_<%# Container.ItemIndex%>" /></a><%# DataBinder.Eval(Container.DataItem,"ModCategoryTitle")%>
							</td>
						</tr>
						<tr id="rppToggleModCategory_<%# Container.ItemIndex %>" style="display:none">
							<td>
								<asp:repeater id="rppUserAccessModCategory" runat="server">
									<headertemplate>
										<table cellpadding="2" cellspacing="1" border="0" width="100%">
											<tr bgcolor="#E0EDEF">
												<td align="right"><i>SelectAll</i></td>
												<td><input type="checkbox" id="chkAll" onclick="CheckAll(this)" name="chkAll" title="Select All"
														runat="server" /></td>
											</tr>
									</headertemplate>
									<itemtemplate>
										<tr id="rppRow" runat="server">
											<td style="padding-left:20px;">
												<input type="hidden" name="hidModCategoryId" id="hidModCategoryId" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ModuleCategoryID")%>' />
												<input type="hidden" name="hidName" id="hidName" runat="server" value="Module Category List" />
												<small>
													<%# DataBinder.Eval(Container.DataItem,"Name")%>
												</small>
											</td>
										</tr>
									</itemtemplate>
									<footertemplate>
									    </TABLE>
								    </footertemplate>
							    </asp:repeater>
					        </td>
				        </tr>
				    </ItemTemplate>
			
				    <FooterTemplate>
					    </table>
				    </FooterTemplate>
				</asp:Repeater>
			    
			    <br />
			    </td>
			    <td style="width: 30%;">
			    </td>
		    </tr>
		    
		    <tr valign="top">
			    <td style="width: 70%;">
			    <b>Module Access Rights</b>
    			<asp:Repeater id="rppModule" Runat="server" OnPreRender="rppModule_PreRender">
					<HeaderTemplate>
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
					</HeaderTemplate>
					<ItemTemplate>
						<tr class="tHeader">
							<td style="padding:4px;">
								<a href="javascript:toggleModuleAccessRow(<%# Container.ItemIndex %>);" title="Display/Hide Access Functions">
									<img src="<%= Request.ApplicationPath %>/App_Themes/Default/images/checkOpenIcon.gif" alt="Expand/Hide" 
									    name="imgAccessBoxModule_<%# Container.ItemIndex%>" /></a><%# DataBinder.Eval(Container.DataItem,"Name")%>
							</td>
						</tr>
						<tr id="rppToggleModule_<%# Container.ItemIndex %>" style="display:none">
							<td>
								<asp:repeater id="rppUserAccessModule" runat="server">
									<headertemplate>
										<table cellpadding="2" cellspacing="1" border="0" width="100%">
											<tr bgcolor="#E0EDEF">
												<td align="right"><i>SelectAll</i></td>
												<td><input type="checkbox" id="chkAll" onclick="CheckAll(this)" name="chkAll" title="Select All"
														runat="server" /></td>
											</tr>
									</headertemplate>
									<itemtemplate>
										<tr id="rppRow" runat="server">
											<td style="padding-left:20px;">
												<input type="hidden" name="hidName" id="hidName" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ParentModuleName")%>' />
												<small>
													<%# DataBinder.Eval(Container.DataItem, "FunctionName")%>
												</small>
											</td>
										</tr>
									</itemtemplate>
									<footertemplate>
									    </TABLE>
								    </footertemplate>
							    </asp:repeater>
					        </td>
				        </tr>
				    </ItemTemplate>
			
				    <FooterTemplate>
					    </table>
				    </FooterTemplate>
				</asp:Repeater>
			    
			    <br />
			    </td>
			    <td style="width: 30%;">
			    </td>
		    </tr>
	    
	        <tr valign="top">
			    <td style="width: 70%;">
			    <b>Reports Access Rights</b>
    			<asp:Repeater id="rppReportsFunctionList" Runat="server" OnPreRender="rppReportsFunctionList_PreRender">
					<HeaderTemplate>
						<table width="100%" cellpadding="0" cellspacing="0" border="0" class="tbBorder">
					</HeaderTemplate>
					<ItemTemplate>
						<tr class="tHeader">
							<td style="padding:4px;">
								<a href="javascript:toggleReportAccessRow(<%# Container.ItemIndex %>);" title="Display/Hide Access Functions">
									<img src="<%= Request.ApplicationPath %>/App_Themes/Default/images/checkOpenIcon.gif" alt="Expand/Hide" 
									    name="imgAccessBoxReport_<%# Container.ItemIndex%>" /></a><%# DataBinder.Eval(Container.DataItem,"Reports")%>
							</td>
						</tr>
						<tr id="rppToggleReport_<%# Container.ItemIndex %>" style="display:none">
							<td>
								<asp:repeater id="rppReportsSystemFunctions" runat="server">
									<headertemplate>
										<table cellpadding="2" cellspacing="1" border="0" width="100%">
											<tr bgcolor="#E0EDEF">
												<td align="right"><i>SelectAll</i></td>
												<td><input type="checkbox" id="chkAll" onclick="CheckAll(this)" name="chkAll" title="Select All"
														runat="server" /></td>
											</tr>
									</headertemplate>
									<itemtemplate>
										<tr id="rppRow" runat="server">
											<td style="padding-left:20px;">
												<input type="hidden" name="hidName" id="hidName" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ReportCategory")%>' />
												<small>
													<%# DataBinder.Eval(Container.DataItem,"ReportTitle")%>
												</small>
											</td>
										</tr>
									</itemtemplate>
									<footertemplate>
								</TABLE>
							</footertemplate>
						</asp:repeater>
					</td>
				</tr>
				
				</ItemTemplate>
			
				<FooterTemplate>
					</table>
				</FooterTemplate>
			</asp:Repeater>

			    <br />
			    </td>
			    <td style="width: 30%;">
			    </td>
		    </tr>
	    </table>
	    <TABLE>
			<TR>
				<TD><BR>
					<asp:Button id="btnSubmit" OnClick="btnSubmit_Click" Text="Update" EnableViewState="False" Runat="server" CssClass="button"></asp:Button>&nbsp;
				</TD>
			</TR>
		</TABLE>
		<div class="TABCOMMAND">
				    <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
        </div>
    </div>
 </form>
</body>
</html>
