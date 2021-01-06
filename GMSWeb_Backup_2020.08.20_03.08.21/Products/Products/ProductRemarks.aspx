<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductRemarks.aspx.cs" Inherits="GMSWeb.Products.Products.ProductRemarks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Remarks</title>
</head>
<body style="background: none">
    <form id="form1" runat="server">
    
    <div align="center">
        <asp:Label ID="lblTitle" runat="server" /><br />
        <div id="Div1" style="text-align: left;width:90%" runat="server">
                            <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                        </div>
                        <br />
        <asp:DataGrid ID="dgRemarks" runat="server" AutoGenerateColumns="false"
		    GridLines="none" CellPadding="5" CellSpacing="0" CssClass="tTable tBorder" AllowPaging="true"
                             PageSize="10" OnPageIndexChanged="dgRemarks_PageIndexChanged" Width="90%"> 
		    <Columns>
			    <asp:TemplateColumn HeaderText="Comment" ItemStyle-Width="400px" HeaderStyle-HorizontalAlign="center" 
			            ItemStyle-HorizontalAlign="Left">
				    <ItemTemplate>
					     <asp:Label ID="lblComment2" runat="server">
                                                        <%# FixCrLf(Eval("Comment").ToString())%>
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