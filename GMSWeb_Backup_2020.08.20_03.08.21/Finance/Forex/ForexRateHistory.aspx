<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForexRateHistory.aspx.cs" Inherits="GMSWeb.Finance.Forex.ForexRateHistory" %>

<!DOCTYPE html>

<html>
<head id="Head1" runat="server">
    <title>History Rates for Foreign Currency</title>
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
                        <asp:Label ID="lblTitle" runat="server" />
                    </h4>
                </div>
                <div class="panel-table">
                    <asp:DataGrid ID="dgForexHistory" runat="server" AutoGenerateColumns="false"
                        GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover">
                        <Columns>
                            <asp:TemplateColumn HeaderText="Date Created">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedDate" runat="server">
						    <%# Eval("CreatedDate", "{0: dd-MMM-yyyy}")%>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>

                            <asp:TemplateColumn HeaderText="Buy">
                                <ItemTemplate>
                                    <asp:Label ID="lblBuy" runat="server">
						    <%#(Eval("ForeignCurrencyCode").ToString().Equals("IDR")) ? Eval("BuyRate", "{0:f7}") : Eval("BuyRate", "{0:f4}")%>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>

                            <asp:TemplateColumn HeaderText="Sell" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblBuy" runat="server">
						    <%#(Eval("ForeignCurrencyCode").ToString().Equals("IDR")) ? Eval("SellRate", "{0:f7}") : Eval("SellRate", "{0:f4}")%>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>

                            <asp:TemplateColumn HeaderText="Month-End">
                                <ItemTemplate>
                                    <asp:Label ID="lblBuy" runat="server">
						    <%#(Eval("ForeignCurrencyCode").ToString().Equals("IDR")) ? Eval("MonthEndRate", "{0:f7}") : Eval("MonthEndRate", "{0:f4}")%>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <HeaderStyle CssClass="tHeader" />
                    </asp:DataGrid>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
