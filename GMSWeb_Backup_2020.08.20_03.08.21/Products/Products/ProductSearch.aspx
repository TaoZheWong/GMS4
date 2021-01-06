<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductSearch.aspx.cs" Inherits="GMSWeb.Products.Products.ProductSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<base target="_self" />
    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE" />
    <meta http-equiv="PRAGMA" content="NO-CACHE" />
    <title>Product Search</title>
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
                                                    <asp:Label ID="lblProductCode" runat="server">Product Code</asp:Label>
                                                    <asp:HiddenField runat="server" ID="hidAccountCode" />
                                                    <asp:HiddenField runat="server" ID="hidPM" Value="0" />
                                                    <asp:HiddenField runat="server" ID="hidPH" Value="0" />
                                                    <asp:HiddenField runat="server" ID="hidPH3" Value="0" />                                                     
                                                    </td>
                                                <td class="tbColon">
                                                    :</td>
                                                <td>
                                                    <asp:TextBox ID="txtSearchProductCode" runat="server" CssClass="textbox"></asp:TextBox></td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tbLabel" nowrap>
                                                    <asp:Label ID="lblProductName" runat="server">Product Name</asp:Label></td>
                                                <td class="lbColon">
                                                    :</td>
                                                <td>
                                                    <asp:TextBox ID="txtSearchProductName" runat="server" CssClass="textbox"></asp:TextBox></td>
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
                                AllowPaging="True" OnPageIndexChanged="dtgResults_PageIndexChanged" OnItemDataBound="dtgResults_ItemDataBound"
                                  ItemStyle-VerticalAlign="Top" AlternatingItemStyle-VerticalAlign="Top">
                                <Columns>
                                    <asp:TemplateColumn SortExpression="ProductCode" HeaderText="Product Code">
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                        <ItemTemplate>
                                            <a onclick="return SelectProduct(this);" href="#">
                                                <%# DataBinder.Eval(Container, "DataItem.ProductCode")%>                                                 
                                            </a>
                                            <input id="hidProductCode" runat="server" type="hidden" value='<%# DataBinder.Eval(Container, "DataItem.ProductCode")%>' />                                            
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="ProductName" SortExpression="ProductName" ReadOnly="True"
                                        HeaderText="Product Name"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="LastUnitPrice" SortExpression="LastUnitPrice" ReadOnly="True"
                                        HeaderText="Last Selling Price" HeaderStyle-Width="10%"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Stock Status">
                                        <HeaderStyle Width="30%"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:label runat="server" ID="lblStockStatus"></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle NextPageText="Next &gt;&gt;" PrevPageText="&lt;&lt; Previous" HorizontalAlign="Center"
                                    ></PagerStyle>
                                <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                            <AlternatingItemStyle CssClass="tAltRow" />
                                            <FooterStyle CssClass="tFooter" />
                            </asp:DataGrid>
                            
                            <asp:DataGrid ID="dgPackage" runat="server" Width="100%" AutoGenerateColumns="False"
                                AllowPaging="True" OnPageIndexChanged="dgPackage_PageIndexChanged"
                                ItemStyle-VerticalAlign="Top" AlternatingItemStyle-VerticalAlign="Top">
                                <Columns>
                                    <asp:TemplateColumn SortExpression="ProductCode" HeaderText="Product Code">
                                        <HeaderStyle Width="20%"></HeaderStyle>
                                        <ItemTemplate>
                                            <a id="lnkSelectPackage" runat="server" onclick="return SelectPackage(this);" href="#">
                                                <%# DataBinder.Eval(Container, "DataItem.ProductCode")%>
                                            </a>
                                            <input type="hidden" id="hidPackage" runat="server" value='<%# Eval("PackageID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="ProductDescription" SortExpression="ProductDescription" ReadOnly="True"
                                        HeaderText="Product Description"></asp:BoundColumn>
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
