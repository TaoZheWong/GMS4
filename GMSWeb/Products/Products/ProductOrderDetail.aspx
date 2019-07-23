<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductOrderDetail.aspx.cs" Inherits="GMSWeb.Products.Products.ProductOrderDetail" %>

<!DOCTYPE html>

<html>
<head id="Head1" runat="server">
    <title>Product Order Detail</title>
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
<body class="<%=getIsLargeFont%> <%=getIsOptimizedTable%>">
    <form id="form1" runat="server">
    <div class="container m-t-20">
          <div class="form-group col-lg-12 col-sm-12">           
                <div class="col-sm-4">
                    <input type="hidden" id="hidStatusType" runat="server" />
                </div>
            </div>
        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false"
		    GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" OnItemDataBound="dgData_ItemDataBound">
             <Columns>
		        <asp:TemplateColumn HeaderText="Trn Date">
				    <ItemTemplate>       
                   <%# hidStatusType.Value == "S" ? DateTime.ParseExact(Eval("TrnDate").ToString(), "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy"):DateTime.Parse(Eval("TrnDate").ToString()).ToString("dd-MMM-yyyy")%> 
			    </ItemTemplate>
			    </asp:TemplateColumn>
			    
			    <asp:TemplateColumn HeaderText="Trn No." >
				    <ItemTemplate>
					    <asp:Label ID="lblTrnNo" runat="server">
						    <%#Eval("TrnNo")%>
					    </asp:Label>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    
			    <asp:TemplateColumn HeaderText="Customer" >
				    <ItemTemplate>
					    <%#Eval("AccountName")%>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    
			    <asp:TemplateColumn HeaderText="Qty" >
				    <ItemTemplate>
					    <%#Eval("Qty")%>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="Narration">
				    <ItemTemplate>
					    <%#Eval("Narration")%>
				    </ItemTemplate>
			    </asp:TemplateColumn>
		    </Columns>
		    <HeaderStyle CssClass="tHeader" />
		    <AlternatingItemStyle CssClass="tAltRow" />
		    <FooterStyle CssClass="tFooter" />
	    </asp:DataGrid>
	    <asp:DataGrid ID="dgPO" runat="server" AutoGenerateColumns="false"
		    GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" OnItemDataBound="dgPO_ItemDataBound">
		    <Columns>
		        <asp:TemplateColumn HeaderText="Trn Date">
				    <ItemTemplate>    
                        <%# DateTime.Parse(Eval("TrnDate").ToString()).ToString("dd-MMM-yyyy") %>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="PO No.">
				    <ItemTemplate>
					    <asp:Label ID="lblPONo" runat="server" Text='<%#Eval("PONo")%>'></asp:Label>
				    </ItemTemplate>
			    </asp:TemplateColumn>			    
			    <asp:TemplateColumn HeaderText="Trn No.">
				    <ItemTemplate>
					    <asp:Label ID="lblTrnNo" runat="server">
						    <%#Eval("TrnNo")%></asp:Label>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    
			    <asp:TemplateColumn HeaderText="Supplier" Visible="false">
				    <ItemTemplate>
					    <%#Eval("AccountName")%>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    
			    <asp:TemplateColumn HeaderText="Qty">
				    <ItemTemplate>
					    <%#Eval("Qty")%>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    
			    <asp:TemplateColumn HeaderText="CRD" >
				    <ItemTemplate>
                        <asp:Label ID="lblCRD" runat="server"></asp:Label>
					    
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    
			     <asp:TemplateColumn HeaderText="ETD" >
				    <ItemTemplate>
                        <asp:Label ID="lblETD" runat="server"></asp:Label>
					    
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    
			    <asp:TemplateColumn HeaderText="ETA" >
				    <ItemTemplate>
                        <asp:Label ID="lblETA" runat="server"></asp:Label>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="Remarks">
				    <ItemTemplate>
                        <asp:Label ID="lblRemarks" runat="server"></asp:Label>					    
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="Mode">
				    <ItemTemplate>
					    <%#Eval("DelMode")%>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    
			    <asp:TemplateColumn HeaderText="Purchaser">
				    <ItemTemplate>
					    <%#Eval("CrtUser")%>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn HeaderText="Narration">
				    <ItemTemplate>
					    <%#Eval("Narration")%>
				    </ItemTemplate>
			    </asp:TemplateColumn>
		    </Columns>
		    <HeaderStyle CssClass="tHeader" />
		    <AlternatingItemStyle CssClass="tAltRow" />
		    <FooterStyle CssClass="tFooter" />
	    </asp:DataGrid>
        <asp:DataGrid ID="dgDetail" runat="server" AutoGenerateColumns="false"
            GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" OnItemDataBound="dgDetail_ItemDataBound">
            <Columns>
                <asp:TemplateColumn HeaderText="Customer">
                    <ItemTemplate>
                        <%# Eval("Customer")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Customer Name">
                    <ItemTemplate>
                        <%# Eval("CustomerName")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Account Group">
                    <ItemTemplate>
                        <%# Eval("AccountGroup")%>
                    </ItemTemplate>
                </asp:TemplateColumn>                
                <asp:TemplateColumn HeaderText="Quantity">
                    <ItemTemplate>
                        <%#Eval("Quantity")%>
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