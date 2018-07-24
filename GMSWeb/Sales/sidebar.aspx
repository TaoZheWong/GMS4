<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sidebar.aspx.cs" Inherits="GMSWeb.Sales.sidebar" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title>Leeden Group Management System</title>
	<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
</head>
<body>
    <form id="form1" runat="server">
		<div id="GroupSideBar">
        <div id="ContentBar">
            <asp:Label ID="lblSales" style="FONT-WEIGHT: bold; FONT-SIZE: 120%; COLOR: #0135ad" runat="server" Text="Sales"></asp:Label>
	    </div>
	    <asp:TreeView ID="TreeView1" runat="server" ImageSet="Simple" ExpandDepth="0" ShowLines="true">
            <Nodes>
            <asp:TreeNode Text="Commission" Value="Commission" SelectAction="Expand">
                    <asp:TreeNode Text="CommissionNGPQ" Value="CommissionNGPQ" NavigateUrl="~/Sales/Commission/CommissionNGPQ.aspx" Target="main">
                    </asp:TreeNode>
                    <asp:TreeNode Text="Monthly Records" Value="MonthlyRecords" NavigateUrl="~/Sales/Commission/MonthlyRecord.aspx" Target="main">
                    </asp:TreeNode>
                    <asp:TreeNode Text="Entertainment Exp" Value="EntertainmentExpenses" NavigateUrl="~/Sales/Commission/EntertainmentExpense.aspx" Target="main">
                    </asp:TreeNode>
                    <asp:TreeNode Text="PMCommission" Value="PMCommission" NavigateUrl="~/Sales/Commission/PMCommission.aspx" Target="main">
                    </asp:TreeNode>
            </asp:TreeNode>
            <asp:TreeNode Text="Reports" Value="Reports" NavigateUrl="~/Sales/Reports/View.aspx" Target="main">
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