<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="gmssidebar.aspx.cs" Inherits="GMSWeb.gmssidebar" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title>Ace Dynamics Group Management System</title>
	<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="GroupSideBar">
        <div id="ContentBar">
            <asp:Label ID="lblOrganization" style="FONT-WEIGHT: bold; FONT-SIZE: 120%; COLOR: #349a34" runat="server" Text="Organization"></asp:Label>
	    </div>
	    <asp:TreeView ID="TreeView1" runat="server" ImageSet="Simple">
            <Nodes>
                <asp:TreeNode Text="Employee" Value="Employee">
                    <asp:TreeNode Text="Staff Listing" Value="StaffListing"></asp:TreeNode>
                    <asp:TreeNode Text="Organization Chart" Value="OrgChart"></asp:TreeNode>
                    <asp:TreeNode Text="Job Specifications" Value="JobSpecs"></asp:TreeNode>
                    <asp:TreeNode Text="KPI" Value="KPI"></asp:TreeNode>
                    <asp:TreeNode Text="Staff Strength" Value="StaffStrength"></asp:TreeNode>
                </asp:TreeNode>
                <asp:TreeNode Text="Upload" Value="Upload">
                    <asp:TreeNode Text="Budget" Value="Budget" NavigateUrl="~/Organization/Upload/UploadBudget.aspx" Target="mainpage"></asp:TreeNode>
                    <asp:TreeNode Text="Audit" Value="Audit" NavigateUrl="~/Organization/Upload/UploadAudit.aspx" Target="mainpage"></asp:TreeNode>
                </asp:TreeNode>
            </Nodes>
            <ParentNodeStyle Font-Bold="true" />
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