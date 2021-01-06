<%@ Page Language="C#" AutoEventWireup="true" Codebehind="sidebar.aspx.cs" Inherits="GMSWeb.Finance.sidebar" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Leeden Group Management System</title>
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="GroupSideBar">
            <div id="ContentBar">
                <asp:Label ID="lblFinance" Style="font-weight: bold; font-size: 120%; color: #0135ad"
                    runat="server" Text="Finance"></asp:Label>
            </div>
            <asp:TreeView ID="TreeView1" runat="server" ImageSet="Simple" ExpandDepth="0" ShowLines="true">
                <Nodes>
                    <asp:TreeNode Text="Bank Facilities" Value="BankFacilities" SelectAction="Expand">
                        <asp:TreeNode Text="Bank Info" Value="Bank Info" NavigateUrl="~/Finance/BankFacilities/BankInfo.aspx"
                            Target="main"></asp:TreeNode>
                        <asp:TreeNode Text="Bank Utilisation" Value="Bank Utilisation" NavigateUrl="~/Finance/BankFacilities/BankUtilisation.aspx"
                            Target="main"></asp:TreeNode>
                        <asp:TreeNode Text="Customer List" Value="Customer List" NavigateUrl="~/Finance/BankFacilities/CustomerList.aspx"
                            Target="main"></asp:TreeNode>
                    </asp:TreeNode>
                    <asp:TreeNode Text="Data" Value="Upload" SelectAction="Expand">
                        <asp:TreeNode Text="Budget" Value="Budget" NavigateUrl="~/Finance/Upload/UploadBudget.aspx"
                            Target="main"></asp:TreeNode>
                        <asp:TreeNode Text="Product Budget" Value="Product Budget" NavigateUrl="~/Finance/Upload/UploadProductBudget.aspx"
                            Target="main"></asp:TreeNode>
                        <asp:TreeNode Text="Audit" Value="Audit" NavigateUrl="~/Finance/Upload/UploadAudit.aspx"
                            Target="main"></asp:TreeNode>
                        <asp:TreeNode Text="Finance Data" Value="Finance Data" NavigateUrl="~/Finance/Upload/UploadFinanceData.aspx"
                            Target="main"></asp:TreeNode>
                        <asp:TreeNode Text="Special" Value="Special" NavigateUrl="~/Finance/Upload/UploadSpecial.aspx"
                            Target="main"></asp:TreeNode>
                        <asp:TreeNode Text="Audited Report" Value="Audited Report" NavigateUrl="~/Finance/Upload/UploadAuditedReports.aspx"
                            Target="main"></asp:TreeNode>
                        <asp:TreeNode Text="COA Mapping" Value="COA Mapping" NavigateUrl="~/Finance/Upload/COAMapping.aspx"
                            Target="main"></asp:TreeNode>
                    </asp:TreeNode>
                    <asp:TreeNode Text="Reports" Value="Reports" NavigateUrl="~/Finance/Reports/BankReport.aspx"
                        Target="main"></asp:TreeNode>
                </Nodes>
                <ParentNodeStyle Font-Bold="true" />
                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px"
                    ForeColor="#5555DD" />
                <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="0px"
                    NodeSpacing="0px" VerticalPadding="0px" />
            </asp:TreeView>
        </div>
    </form>
</body>
</html>
