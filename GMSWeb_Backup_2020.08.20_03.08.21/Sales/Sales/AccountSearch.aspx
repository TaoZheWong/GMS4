<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountSearch.aspx.cs" Inherits="GMSWeb.Sales.Sales.AccountSearch" %>

<!DOCTYPE html >

<html>
<head runat="server">
<base target="_self" />
    <title>Account Search</title>
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
    <div class="container">
         <div class="panel panel-primary m-t-20">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <i class="ti-search"></i>
                        Search Account 
                    </h4>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal m-t-20">
                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                        <asp:Label ID="lblAccountCode" runat="server">Account Code</asp:Label>
                                </label>
                                <div class="col-sm-6">
                                        <asp:TextBox ID="txtSearchAccountCode" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                        <asp:Label ID="lblAccountName" runat="server">Account Name</asp:Label>
                                </label>
                                <div class="col-sm-6">
                                        <asp:TextBox ID="txtSearchAccountName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group col-lg-6 col-sm-6">
                                <label class="col-sm-6 control-label text-left">
                                        <asp:Label ID="lblSearchOption" runat="server">Search Option</asp:Label>
                                </label>
                                <div class="col-sm-6">
                                    <div class="radio">
                                       <asp:RadioButtonList ID="rblSearchOption" runat="server" RepeatColumns="2" RepeatLayout="Flow"
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
                </div>
                <div class="panel-footer clearfix">
                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click" AccessKey="S" runat="server"
                            CssClass="btn btn-primary pull-right" Text="Search"></asp:Button>
                </div> 
            </div>

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <i class="ti-agenda"></i>
                        <asp:Label ID="lblResultSummary" Visible="false" runat="server"></asp:Label>
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive">
                        <asp:DataGrid ID="dtgResults" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-striped table-hover"
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
                                <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="grid_pagination"></PagerStyle>
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
