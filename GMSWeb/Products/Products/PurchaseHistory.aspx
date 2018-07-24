<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseHistory.aspx.cs" Inherits="GMSWeb.Products.Products.PurchaseHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Detail</title>
</head>
<body style="background: none">
    <form id="form1" runat="server">
    
    <div align="center">
        <asp:Label ID="lblTitle" runat="server" /><br />
        <div id="Div1" style="text-align: left;width:90%" runat="server">
                            <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                        </div>
                        <br />
        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" OnPageIndexChanged="dgData_PageIndexChanged"
		    GridLines="none" CellPadding="5" CellSpacing="0" CssClass="tTable tBorder" Width="90%" AllowPaging="true" PageSize="10">
		    <Columns>
		        <asp:TemplateColumn HeaderText="Trn Date" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="center" 
			            ItemStyle-HorizontalAlign="center">
				    <ItemTemplate>
					    <asp:Label ID="lblTrnDate" runat="server">
						    <%# Eval("TrnDate", "{0: dd-MMM-yyyy}")%>
					    </asp:Label>
				    </ItemTemplate>
			    </asp:TemplateColumn>	    
			    <asp:TemplateColumn HeaderText="Currency" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="center" 
			            ItemStyle-HorizontalAlign="center">
				<ItemTemplate>
				<asp:Label ID="lblCurrency" runat="server">
                                                        <%# Eval("Currency").ToString()%>
                                                    </asp:Label>
				</ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="Unit Price" ItemStyle-Width="100px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="center">
                    <ItemTemplate>
                       <asp:Label ID="lblUnitPrice" runat="server">
                                                        <%# Eval("UnitAmount")%>
                       </asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>                			    
		    </Columns>
		    <HeaderStyle CssClass="tHeader" />
		    <AlternatingItemStyle CssClass="tAltRow" />
		     <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
		    <FooterStyle CssClass="tFooter" />
	    </asp:DataGrid>
    </div>
    </form>
</body>
</html>

