<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sidebar.aspx.cs" Inherits="GMSWeb.SysHR.sidebar" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title>Leeden Group Management System</title>
	<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="SideBar" style="height:2000px" >
        <div id="ContentBar">
            <asp:Label ID="lblHR" style="FONT-WEIGHT: bold; FONT-SIZE: 120%; COLOR: #0135ad" runat="server" Text="Human Resource"></asp:Label>
	    </div>
	    <asp:TreeView ID="TreeView1" runat="server" ImageSet="Simple" ExpandDepth="0" ShowLines="true">
            <Nodes>
                <asp:TreeNode Text="List of People" Value="List of People" SelectAction="Expand">
                    <asp:TreeNode Text="Staff" Value="Staff" NavigateUrl="~/SysHR/Staff/Staff.aspx" Target="main"></asp:TreeNode>
                     <asp:TreeNode Text="Add New Staff" Value="Add New Staff" NavigateUrl="~/SysHR/Staff/AddNewStaff.aspx" Target="main"></asp:TreeNode>
                </asp:TreeNode>
            </Nodes>
            <Nodes>
                <asp:TreeNode Text="Training" Value="Training" SelectAction="Expand">
                    <asp:TreeNode Text="Organizer" Value="Organizer" NavigateUrl="~/SysHR/Training/Organizer.aspx" Target="main"></asp:TreeNode>
                    <asp:TreeNode Text="Course" Value="Course" NavigateUrl="~/SysHR/Training/Course.aspx" Target="main"></asp:TreeNode>
                    <asp:TreeNode Text="Session" Value="Session" NavigateUrl="~/SysHR/Training/CourseSession.aspx" Target="main"></asp:TreeNode>
                    <asp:TreeNode Text="Records" Value="Records" NavigateUrl="~/SysHR/Training/EmployeeCourse.aspx" Target="main"></asp:TreeNode>
                </asp:TreeNode>
            </Nodes>
            <Nodes>
                <asp:TreeNode Text="Reports" Value="Reports" NavigateUrl="~/SysHR/Reports/SystemGenerated.aspx" Target="main">
                </asp:TreeNode>
            </Nodes>
            <Nodes>
                <asp:TreeNode Text="Resources" Value="Resources" SelectAction="Expand">
                <asp:TreeNode Text="Category" Value="Category" NavigateUrl="~/SysHR/Resources/Category.aspx" Target="main"></asp:TreeNode>
                    <asp:TreeNode Text="View" Value="View" NavigateUrl="~/SysHR/Resources/View.aspx" Target="main"></asp:TreeNode>
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
</html>