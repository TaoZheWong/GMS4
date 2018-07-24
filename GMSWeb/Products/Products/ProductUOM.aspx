<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductUOM.aspx.cs" Inherits="GMSWeb.Products.Products.ProductUOM" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>Product UOM</title>
    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-timepicker/bootstrap-timepicker.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/style.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/overwrite_app.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
    <style type="text/css">
        body {
            overflow: auto;
        }
    </style>
</head>
<body class="<%=getIsLargeFont%> <%=getIsOptimizedTable%>">
    <form id="form1" runat="server">
    <div class="container m-t-20">
        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false"
		    GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover">
		    <Columns>
                <asp:TemplateColumn HeaderText="UOM">
				    <ItemTemplate>
					    <asp:Label ID="lblBatchNo" runat="server">
						    <%#Eval("UOM")%>
					    </asp:Label>
				    </ItemTemplate>
			    </asp:TemplateColumn>		        
			    <asp:TemplateColumn HeaderText="ConversionFactor">
				    <ItemTemplate>
					    <%#Eval("ConversionFactor")%>
				    </ItemTemplate>
			    </asp:TemplateColumn>
		    </Columns>
		    <HeaderStyle CssClass="tHeader" />
		    <AlternatingItemStyle CssClass="tAltRow" />
		    <FooterStyle CssClass="tFooter" />
	    </asp:DataGrid>	    
    </div>
    <asp:Label ID="lblMsg" runat="server" />
    </form>
</body>
</html>