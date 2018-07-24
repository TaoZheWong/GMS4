<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Old BankReport.aspx.cs" Inherits="GMSWeb.SysFinance.BankFacilities.BankReport" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title>Bank Facilities - Bank Report</title>
	<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
	<script type="text/javascript">
		//<![CDATA[
		function toggleAccessRow(n)
		{
			if( document.getElementById("rppToggle_" + n) )
			{
				var current = document.getElementById("rppToggle_" + n).style.display;
				document.getElementById("rppToggle_" + n).style.display = (current == null || current == "none")?"":"none";
				document["imgAccessBox_" + n].src = (current == null || current == "none")? sDOMAIN+"/App_Themes/Default/images/checkCloseIcon.gif" : sDOMAIN+"/App_Themes/Default/images/checkOpenIcon.gif";
			}
		}
		//]]>
		</script>
</head>
<body id="theBody" runat="server">
    <form id="form1" runat="server">
    <div id="ContentBar">
        <h3>Bank Facilities &gt; Bank Report</h3>
        Click a report to view.
        <br /><br />    
	    <table class="tTable" style="width: 99%;">
		    <tr valign="top">
			    <td style="width: 70%;">
    			<asp:Repeater id="rppCategoryList" Runat="server">
					<HeaderTemplate>
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
					</HeaderTemplate>
					<ItemTemplate>
						<tr class="tHeader">
							<td style="padding:4px;">
								<a href="javascript:toggleAccessRow(<%# Container.ItemIndex %>);" title="Display/Hide Access Functions">
									<img src="<%= Request.ApplicationPath %>/App_Themes/Default/images/checkCloseIcon.gif" alt="Expand/Hide" 
									    name="imgAccessBox_<%# Container.ItemIndex%>" /></a><%# DataBinder.Eval(Container.DataItem,"Name")%>
							</td>
						</tr>
						<tr id="rppToggle_<%# Container.ItemIndex %>">
							<td>
								<asp:repeater id="rppReportList" runat="server" OnItemCommand="rppReportList_ItemCommand">
									<headertemplate>
										<table cellpadding="2" cellspacing="1" border="0" width="100%">
									</headertemplate>
									<itemtemplate>
										<tr id="rppRow" runat="server">
											<td style="padding-left:30px;">
													<%# Container.ItemIndex + 1 %>. &nbsp;
													<asp:LinkButton ID="lnkPrintReport" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ReportId")%>' EnableViewState="false"
												    CausesValidation="false"><span><%# Eval("Description")%></span></asp:LinkButton>
											</td>
										</tr>
									</itemtemplate>
									<footertemplate>
								        </table>
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
    </div>
 </form>
</body>
</html>
