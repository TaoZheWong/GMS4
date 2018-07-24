<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="FinanceAuditReport.aspx.cs" Inherits="GMSWeb.Finance.Reports.FinanceAuditReport" Title="Finance - Reports Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<a name="TemplateInfo"></a>
<h1>Audit &gt; Reports</h1>
        <p>Click a report to view.</p>
          <table class="tTable1" style="width: 620px;margin-left: 8px">
		    <tr valign="top">
			    <td style="width: 620px;">
    			<asp:Repeater id="rppCategoryList" Runat="server">
					<HeaderTemplate>
						<table width="620px" cellpadding="5" cellspacing="5" border="0">
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
</asp:Content>
