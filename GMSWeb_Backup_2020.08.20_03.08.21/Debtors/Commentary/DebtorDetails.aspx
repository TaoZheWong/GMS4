<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DebtorDetails.aspx.cs" Inherits="GMSWeb.Debtors.Commentary.DebtorDetails" %>

<!DOCTYPE>

<html>
<head id="Head1" runat="server">
    <title>Detail</title>
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
            <asp:Label ID="lblTitle" runat="server" />
            <div class="panel panel-primary">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                </h4>
            </div>
            <div class="panel-body no-padding">
                <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" OnPageIndexChanged="dgData_PageIndexChanged"
                    GridLines="none" CellPadding="5" CellSpacing="0" CssClass="table table-condensed table-striped table-hover" AllowPaging="true" PageSize="10">
                    <Columns>
                        <asp:TemplateColumn HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedDate" runat="server">
							        <%# Eval("SALES_TrnDate", "{0: dd-MMM-yyyy}")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Ref No.">
                            <ItemTemplate>
                                <asp:Label ID="lblComment2" runat="server">
														        <%# Eval("DocNo").ToString()%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="PO No.">
                            <ItemTemplate>
                                <asp:Label ID="lblComment3" runat="server">
														        <%# Eval("PONo").ToString()%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Receipt Amt" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFr91" runat="server">
														        <%# Eval("Receipt_Amount", "{0:C}")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Outstanding Amt" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFr90" runat="server">
														        <%# Eval("Amount", "{0:C}")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="tHeader" />
                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                </asp:DataGrid>
            </div>
        </div>
        </div>
    </form>
</body>
</html>
