<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="ImporterExporter.aspx.cs" Inherits="GMSWeb.SysHR.Upload.ImporterExporter" Title="HR - Importer / Exporter Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<h1>Administration &gt; Importer</h1>
        <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
        <p>Import data into GMS.</p>      
        <table style="margin-left: 8px" cellpadding="5" cellspacing="5" border=0 class="tTable">
        <tr class="tHeader">
        <td style="padding:4px;">
        1. Import Cost
        </td>
        </tr>
        <tr>
        <td>
        <p>Click <b>Import</b> button below to import cost from HR into GMS.</p>
        </td>
        </tr>
            <tr>
			    <td>
				   <div class="tTable">
				        <table style="margin-left: 6px" cellpadding="5" cellspacing="5" border=0>
				            <tr>
				                <td>
		                            <asp:Label CssClass="tbLabel" ID="lblYear" runat="server">Year</asp:Label>
	                            </td>
	                            <td style="width: 5%">:</td>
	                            <td>
	                                <asp:DropDownList CssClass="dropdownlist" ID="ddlYear" runat="server" DataTextField="Year" DataValueField="Year" />
	                            </td>
	                            <td>
		                            <asp:Label CssClass="tbLabel" ID="lblMonth" runat="server" style="margin-left:10px">Month</asp:Label>
	                            </td>
	                            <td style="width: 5%">:</td>
	                            <td>
	                                <asp:DropDownList CssClass="dropdownlist" ID="ddlMonth" runat="server" DataTextField="Month" DataValueField="Month" />
	                            </td>
				                <td>
				                <asp:Button ID="btnImport" runat="server" style="margin-left: 60px" CausesValidation="true" Text="Import" CssClass="button" OnClick="btnImport_Click" />
				                </td>
				            </tr>
				            <tr></tr>
				            <tr>
				                 <td colspan="7">
						            <IFRAME ID=IFrame1 FRAMEBORDER=0 SCROLLING=YES Runat="Server" width=100% Style="display:none"></IFRAME>	
					            </td>
				            </tr>
				        </table>				        				       
				    </div>
			    </td>
		    </tr>
		    
	    </table>
	    <div class="TABCOMMAND">
		    <atlas:UpdatePanel ID="udpMsgUpdater" runat="server" Mode="Always">
			    <ContentTemplate>
				    <ul>
					    <li></li>
				    </ul>
				    <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
			    </ContentTemplate>
		    </atlas:UpdatePanel>
        </div>
</asp:Content>
