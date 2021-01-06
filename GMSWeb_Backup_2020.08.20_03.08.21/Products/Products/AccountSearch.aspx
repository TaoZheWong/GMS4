<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountSearch.aspx.cs" Inherits="GMSWeb.Products.Products.AccountSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<base target="_self" />
    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE" />
    <meta http-equiv="PRAGMA" content="NO-CACHE" />
    <title>Untitled Page</title>
</head>
<body style="background-image:none; text-align:left">
    <form id="form1" runat="server">
    <div>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td nowrap>
                            <table class="tblTable" width="100%">
                                <tr>
                                    <td>
                                        <table id="tblSearch" width="336" style="width: 336px; height: 58px">
                                            <tr>
                                                <td class="tbLabel" nowrap>
                                                    <asp:HiddenField runat="server" ID="hidAccountType" />
                                                    <asp:Label ID="lblAccountCode" runat="server">Account Code</asp:Label></td>
                                                <td class="tbColon">
                                                    :</td>
                                                <td>
                                                    <asp:TextBox ID="txtSearchAccountCode" runat="server" CssClass="textbox"></asp:TextBox></td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tbLabel" nowrap>
                                                    <asp:Label ID="lblAccountName" runat="server">Account Name</asp:Label></td>
                                                <td class="lbColon">
                                                    :</td>
                                                <td>
                                                    <asp:TextBox ID="txtSearchAccountName" runat="server" CssClass="textbox"></asp:TextBox></td>
                                                <td>
                                                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click" AccessKey="S" runat="server"
                                                        CssClass="button" Text="Search"></asp:Button></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="tbLabel" nowrap>
                                        <div>
                                            <asp:Label ID="lblSearchOption" runat="server">Search Option</asp:Label></div>
                                        <asp:RadioButtonList ID="rblSearchOption" runat="server" RepeatColumns="2" RepeatLayout="Flow"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0" Selected="True">Anywhere</asp:ListItem>
                                            <asp:ListItem Value="1">Start with</asp:ListItem>
                                            <asp:ListItem Value="2">End with</asp:ListItem>
                                            <asp:ListItem Value="3">Exact Word</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 100%" align="right">
                                <asp:Label ID="lblResultSummary" Visible="false" runat="server"></asp:Label></div>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>
                            <asp:DataGrid ID="dtgResults" runat="server" Width="100%" AutoGenerateColumns="False"
                                AllowPaging="True" OnPageIndexChanged="dtgResults_PageIndexChanged">
                                <Columns>
                                    <asp:TemplateColumn SortExpression="AccountCode" HeaderText="Account Code">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                        <ItemTemplate>
                                            <a onclick="return SelectAccount(this,'<%# ((DataBinder.Eval(Container, "DataItem.AccountName").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.Address1").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.Address2").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.Address3").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.Address4").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.ContactPerson").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.MobilePhone").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.OfficePhone").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.Fax").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.Email").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.SalesPersonID").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>');" href="#">
                                                <%# DataBinder.Eval(Container,"DataItem.AccountCode") %><br />
                                                
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="AccountName" SortExpression="AccountName" ReadOnly="True"
                                        HeaderText="Customer Name"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Address1" SortExpression="Address1" ReadOnly="True" HeaderText="Default Address">
                                    </asp:BoundColumn>
                                </Columns>
                                <PagerStyle NextPageText="Next &gt;&gt;" PrevPageText="&lt;&lt; Previous" HorizontalAlign="Center"
                                    ></PagerStyle>
                                <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
    </div>
    </form>
</body>
</html>
