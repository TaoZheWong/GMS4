<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sidebar.aspx.cs" Inherits="GMSWeb.CorporateServices.sidebar" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title>Leeden Group Management System</title>
	<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
</head>
<body>
    <form id="form1" runat="server">
	<div id="SideBar" style="height:2000px">
        <div id="ContentBar">
            <asp:Label ID="lblFinance" style="FONT-WEIGHT: bold; FONT-SIZE: 120%; COLOR: #0135ad" runat="server" Text="Corporate Services"></asp:Label>
	    </div>
        	<asp:TreeView ID="TreeView1" runat="server" ImageSet="Simple" ExpandDepth="0" ShowLines="true">
            <Nodes>
                <asp:TreeNode Text="Corporate Services" Value="Corporate Services" SelectAction="Expand">
                    </asp:TreeNode>
            </Nodes>
            <ParentNodeStyle Font-Bold="False" />
            <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
            <SelectedNodeStyle
                Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px" ForeColor="#5555DD" />
            <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="0px"
                NodeSpacing="0px" VerticalPadding="0px" />
        </asp:TreeView>
	</div>
    </form>
  </body>
</HTML>