<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewResourcesHistory.aspx.cs" Inherits="GMSWeb.UsefulResources.Resources.ViewResourcesHistory" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Archived Documents</title>
    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-timepicker/bootstrap-timepicker.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/style.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/overwrite_app.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
    <style>
        body {
            overflow: auto;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
        <h1 class="page-header">
        <asp:Label ID="lblTitle" runat="server" />
        </h1>

        <asp:DataGrid ID="dgDocument" OnItemCommand="dgDocument_ItemCommand" OnPageIndexChanged="dgDocument_PageIndexChanged" runat="server" 
            AutoGenerateColumns="false" GridLines="none" 
            CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="True" PageSize="10">
		    <Columns>
		         <asp:TemplateColumn HeaderText="S/N" ItemStyle-Width="10px" HeaderStyle-HorizontalAlign="center" 
			            ItemStyle-HorizontalAlign="center">
				    <ItemTemplate>
				        <%# (Container.ItemIndex + 1) + ((dgDocument.CurrentPageIndex) * dgDocument.PageSize)%>
				    </ItemTemplate>
			    </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="Date Uploaded" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="left" 
			            ItemStyle-HorizontalAlign="left">
				    <ItemTemplate>
				        <asp:LinkButton ID="linkName" runat="server" Text='<%# Eval("DateUploaded")%>' CommandName="Load" ForeColor="#005DAA" CommandArgument='<%#Eval("FileName")%>'></asp:LinkButton> 
				    </ItemTemplate>
			    </asp:TemplateColumn>
			     </Columns>
		    <HeaderStyle CssClass="tHeader" />
		    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
	    </asp:DataGrid>
    </div>
    </form>
</body>
</html>
