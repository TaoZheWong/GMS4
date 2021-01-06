<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessagePanelControl.ascx.cs" Inherits="GMSWeb.CustomCtrl.MessagePanelControl" %>
<asp:Panel ID="pnlMessagePanel" runat="server" Visible="false" CssClass="P_MsgBox" EnableViewState="false">
	<div class="alert alert-danger m-t-10 INFOIMG">
		<asp:Label ID="lblMessagePanelMsg" runat="server" />
	</div>
</asp:Panel>