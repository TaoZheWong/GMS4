<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sidebar.aspx.cs" Inherits="GMSWeb.Corporate.sidebar" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title>Leeden Group Management System</title>
	<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="SideBar">
        <div id="ContentBar">
            <asp:Label ID="lblCorporate" style="FONT-WEIGHT: bold; FONT-SIZE: 120%; COLOR: #0135ad" runat="server" Text="Corporate"></asp:Label>
	    </div>
	    <asp:TreeView ID="TreeView1" runat="server" ImageSet="Simple" ShowLines="true" ExpandDepth="0">
            <Nodes>
                <asp:TreeNode Text="Corporate" Value="Corporate" SelectAction="Expand">
                    <asp:TreeNode Text="News" Value="News"></asp:TreeNode>
                    <asp:TreeNode Text="Staff" Value="Staff"></asp:TreeNode>
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