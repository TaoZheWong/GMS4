<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddressSearch.aspx.cs" Inherits="GMSWeb.Sales.Sales.AddressSearch" %>

<!DOCTYPE html>

<html >
<head id="Head1" runat="server">
<base target="_self" />
    <title>Address Search</title>
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
        <asp:HiddenField runat="server" ID="hidAccountCode" />
    <div class="container">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <i class="ti-agenda"></i>
                    <asp:Label ID="lblResultSummary" Visible="false" runat="server"></asp:Label>
                </h4>
            </div>
            <div class="panel-body no-padding">
                <div class="table-responsive">
                    <asp:DataGrid ID="dtgResults" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="table table-condensed table-striped table-hover"
                        AllowPaging="True" OnPageIndexChanged="dtgResults_PageIndexChanged">
                        <Columns>   
                            <asp:TemplateColumn SortExpression="AddressCode" HeaderText="AddressCode">
                                <HeaderStyle></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label id="lblAddressCode" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.AddressCode").ToString()) %>'>
						            </ASP:LABEL>
                                </ItemTemplate>
                            </asp:TemplateColumn>                                                                      
                            <asp:TemplateColumn SortExpression="Address1" HeaderText="Address">
                                <HeaderStyle></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label id="lblAddress" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.Address1").ToString() + " " + DataBinder.Eval(Container, "DataItem.Address2").ToString() + " " + DataBinder.Eval(Container, "DataItem.Address3").ToString() + " " + DataBinder.Eval(Container, "DataItem.Address4").ToString()) %>'>
						            </ASP:LABEL>
                                </ItemTemplate>
                            </asp:TemplateColumn>  
                            <asp:TemplateColumn SortExpression="AddressID" HeaderText="Select">
                                <HeaderStyle></HeaderStyle>
                                <ItemTemplate>
                                    <a onclick="return SelectAccount(this,'<%# ((DataBinder.Eval(Container, "DataItem.Address1").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.Address2").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.Address3").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.Address4").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.AddressCode").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>','<%# ((DataBinder.Eval(Container, "DataItem.AddressID").ToString()).Replace("\'","\\'")).Replace("\"","\\'\\'") %>');" href="#">
                                        Select<br />                                                
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle NextPageText="Next &gt;&gt;" PrevPageText="&lt;&lt; Previous" HorizontalAlign="Center"
                            ></PagerStyle>
                        <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                                    <AlternatingItemStyle CssClass="tAltRow" />
                                    <FooterStyle CssClass="tFooter" />
                    </asp:DataGrid>
                </div>
            </div>
        </div>
                      
    </div>
    </form>
</body>
</html>
