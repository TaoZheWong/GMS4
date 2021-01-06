<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductSearch.aspx.cs" Inherits="GMSWeb.Sales.Sales.ProductSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<base target="_self" />
    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE" />
    <meta http-equiv="PRAGMA" content="NO-CACHE" />
    <title>Product Search</title>
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
<body style="background-image:none; text-align:left">
    <form id="form1" runat="server">
    <div class="container">
       
            <div class="panel panel-primary m-t-20">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <i class="ti-search"></i>
                        Search Product
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                        <div class="row">
                            <div class="form-group col-lg-6 col-md-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                    <asp:Label ID="lblProductCode" runat="server">Product Code</asp:Label>
                                </label>
                                <asp:HiddenField runat="server" ID="hidAccountCode" />
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtSearchProductCode" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group col-lg-6 col-md-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left"><asp:Label ID="lblProductName" runat="server">Product Name</asp:Label></label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtSearchProductName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-lg-12 col-md-12 col-sm-12">
                            <div>
                                <label class="control-label text-left"><asp:Label ID="lblSearchOption" runat="server">Search Option</asp:Label></label>
                            </div>
                            <div class="radio-inline">
                                <asp:RadioButtonList ID="rblSearchOption" runat="server" RepeatLayout="Flow"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Value="0" Selected="True">Anywhere</asp:ListItem>
                                    <asp:ListItem Value="1">Start with</asp:ListItem>
                                    <asp:ListItem Value="2">End with</asp:ListItem>
                                    <asp:ListItem Value="3">Exact Word</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div> 
                    </div>    
                                 
                </div>
                <div class="panel-footer clearfix">
                     <asp:Button ID="btnSearch" OnClick="btnSearch_Click" AccessKey="S" runat="server"
                            CssClass="btn btn-default pull-right" Text="Search"></asp:Button>
                </div>
                 
            </div> 
		   
	    
	
	                             
                                   
                                       
                             
                                <asp:Label ID="lblResultSummary" Visible="false" runat="server"></asp:Label>
                     
                            <asp:DataGrid ID="dtgResults" runat="server" Width="100%" AutoGenerateColumns="False"
                                AllowPaging="True" OnPageIndexChanged="dtgResults_PageIndexChanged" OnItemDataBound="dtgResults_ItemDataBound"
                                  ItemStyle-VerticalAlign="Top" AlternatingItemStyle-VerticalAlign="Top" CssClass="table table-striped table-bordered">
                                <Columns>
                                    <asp:TemplateColumn SortExpression="ProductCode" HeaderText="Product Code">
                                        <ItemTemplate>
                                            <a onclick="return SelectProduct(this,'<%# ((DataBinder.Eval(Container, "DataItem.ProductName").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'")%>','<%# DataBinder.Eval(Container, "DataItem.UOM")%>','<%# DataBinder.Eval(Container, "DataItem.WeightedCost")%>','<%# string.Format("{0:0.00}",DataBinder.Eval(Container, "DataItem.ListPrice"))%>','<%# string.Format("{0:0.00}",DataBinder.Eval(Container, "DataItem.MinSellingPrice"))%>','<%# string.Format("{0:0.00}",DataBinder.Eval(Container, "DataItem.IntercoPrice"))%>');" href="#">
                                                <%# DataBinder.Eval(Container, "DataItem.ProductCode")%> 
                                            </a>
                                            <input id="hidProductCode" runat="server" type="hidden" value='<%# DataBinder.Eval(Container, "DataItem.ProductCode")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="ProductName" SortExpression="ProductName" ReadOnly="True"
                                        HeaderText="Product Name"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="LastUnitPrice" SortExpression="LastUnitPrice" ReadOnly="True"
                                        HeaderText="Last Selling Price"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Stock Status">
                                        <ItemTemplate>
                                            <asp:label runat="server" ID="lblStockStatus"></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle NextPageText="Next &gt;&gt;" PrevPageText="&lt;&lt; Previous" CssClass="grid_pagination"
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
                                <PagerStyle NextPageText="Next &gt;&gt;" PrevPageText="&lt;&lt; Previous" HorizontalAlign="Center" CssClass="grid_pagination"
                                    ></PagerStyle>
                            </asp:DataGrid>
                       
    </div>
    </form>
</body>
</html>
