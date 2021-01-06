<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sidebar.aspx.cs" Inherits="GMSWeb.Admin.sidebar" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title>Leeden Group Management System</title>
	<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
</head>
<body bgcolor="#F5F5F5">
    <form id="form1" runat="server">
    <div id="SideBar">
        <div id="ContentBar">
            <asp:Label ID="lblAdmin" style="FONT-WEIGHT: bold; FONT-SIZE: 120%; COLOR: #0135ad" runat="server" Text="Admin"></asp:Label>
	    </div>
	    <asp:TreeView ID="TreeView1" runat="server" ImageSet="Simple" ShowLines="true" ExpandDepth="0">
            <Nodes>
                <asp:TreeNode Text="System" Value="System" SelectAction="Expand">
                    <asp:TreeNode Text="Country" Value="Country" NavigateUrl="~/Admin/SystemData/Country.aspx" Target="main"></asp:TreeNode>
                    <asp:TreeNode Text="Division" Value="Division" NavigateUrl="~/Admin/SystemData/DivisionData.aspx" Target="main"></asp:TreeNode>
                    <asp:TreeNode Text="Company" Value="Company" NavigateUrl="~/Admin/SystemData/CompanyData.aspx" Target="main"></asp:TreeNode>
                    <asp:TreeNode Text="Item" Value="Item" NavigateUrl="~/Admin/SystemData/ItemListing.aspx" Target="main"></asp:TreeNode>
                    <asp:TreeNode Text="Item Structure" Value="Item" NavigateUrl="~/Admin/SystemData/ItemStructureListing.aspx" Target="main"></asp:TreeNode>
                    <asp:TreeNode Text="Currency" Value="Currency" NavigateUrl="~/Admin/SystemData/CurrencyListing.aspx" Target="main"></asp:TreeNode>
                </asp:TreeNode>
                <asp:TreeNode Text="Company Reports" Value="CompanyReports" Expanded="false" SelectAction="Expand">
                    <asp:TreeNode Text="Category" Value="Category" NavigateUrl="CompanyReports/Category.aspx" Target="main"></asp:TreeNode>
                    <asp:TreeNode Text="Modify" Value="Modify" NavigateUrl="CompanyReports/Modify.aspx" Target="main"></asp:TreeNode>
                    <asp:TreeNode Text="Upload" Value="Upload" NavigateUrl="CompanyReports/Upload.aspx" Target="main"></asp:TreeNode>
                </asp:TreeNode>
                <asp:TreeNode Text="Module Reports" Value="ModuleReports" Expanded="false" SelectAction="Expand">
                    <asp:TreeNode Text="Category" Value="ModuleCategory" NavigateUrl="ModuleReports/Category.aspx" Target="main"></asp:TreeNode>
                </asp:TreeNode>
                <asp:TreeNode Text="Accounts" Value="Accounts" SelectAction="Expand">
                    <asp:TreeNode Text="Users" Value="Users" NavigateUrl="~/Admin/Accounts/Users.aspx" Target="main"></asp:TreeNode>
                    <asp:TreeNode Text="Change Password" Value="CPassword" NavigateUrl="~/Admin/Accounts/ChangePassword.aspx" Target="main"></asp:TreeNode>
                    <asp:TreeNode Text="Sales Person Mapping" Value="Sales Person Mapping" NavigateUrl="~/Admin/Accounts/SalesPersonMapping.aspx" Target="main"></asp:TreeNode>
                </asp:TreeNode>
            </Nodes>
            <ParentNodeStyle Font-Bold="true" />
            <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
            <SelectedNodeStyle
                Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px" ForeColor="#5555DD" />
            <NodeStyle Font-Names="Tahoma" Font-Size="9pt" ForeColor="Black" HorizontalPadding="0px"
                NodeSpacing="0px" VerticalPadding="0px" />
        </asp:TreeView>
	</div>
    </form>
	
  </body>
</HTML>