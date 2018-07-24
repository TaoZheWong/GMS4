<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForexRateHistory.aspx.cs" Inherits="GMSWeb.SysFinance.SharedInfo.ForexRateHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>History Rates for Foreign Currency</title>
</head>
<body style="background:none">
    <form id="form1" runat="server">
    <div align="center">
        <asp:Label ID="lblTitle" runat="server" />
        <asp:DataGrid ID="dgForexHistory" runat="server" AutoGenerateColumns="false"
		    GridLines="none" CellPadding="5" CellSpacing="5" CssClass="tTable tBorder">
		    <Columns>
		        <asp:TemplateColumn HeaderText="Date Created" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="center" 
			            ItemStyle-HorizontalAlign="center">
				    <ItemTemplate>
					    <asp:Label ID="lblCreatedDate" runat="server">
						    <%# Eval("CreatedDate", "{0: dd-MMM-yyyy}")%>
					    </asp:Label>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    
			    <asp:TemplateColumn HeaderText="Buy" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="center" 
			            ItemStyle-HorizontalAlign="center">
				    <ItemTemplate>
					    <asp:Label ID="lblBuy" runat="server">
						    <%#(Eval("ForeignCurrencyCode").ToString().Equals("IDR")) ? Eval("BuyRate", "{0:f7}") : Eval("BuyRate", "{0:f4}")%>
					    </asp:Label>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    
			    <asp:TemplateColumn HeaderText="Sell" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="center" 
			            ItemStyle-HorizontalAlign="center">
				    <ItemTemplate>
					    <asp:Label ID="lblBuy" runat="server">
						    <%#(Eval("ForeignCurrencyCode").ToString().Equals("IDR")) ? Eval("SellRate", "{0:f7}") : Eval("SellRate", "{0:f4}")%>
					    </asp:Label>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    
			    <asp:TemplateColumn HeaderText="Month-End" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="center" 
			            ItemStyle-HorizontalAlign="center">
				    <ItemTemplate>
					    <asp:Label ID="lblBuy" runat="server">
						    <%#(Eval("ForeignCurrencyCode").ToString().Equals("IDR")) ? Eval("MonthEndRate", "{0:f7}") : Eval("MonthEndRate", "{0:f4}")%>
					    </asp:Label>
				    </ItemTemplate>
			    </asp:TemplateColumn>
		    </Columns>
		    <HeaderStyle CssClass="tHeader" />
		    <AlternatingItemStyle CssClass="tAltRow" />
		    <FooterStyle CssClass="tFooter" />
	    </asp:DataGrid>
    </div>
    </form>
</body>
</html>
