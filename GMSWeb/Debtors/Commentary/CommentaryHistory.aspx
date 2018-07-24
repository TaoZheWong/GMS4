<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommentaryHistory.aspx.cs" Inherits="GMSWeb.Debtors.Commentary.CommentaryHistory" %>

<!DOCTYPE html>

<html>
<head id="Head1" runat="server">
    <title>Commentary History</title>
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
            <asp:Label ID="lblTitle" runat="server" />
             <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <asp:DataGrid ID="dgCommentHistory" runat="server" AutoGenerateColumns="false"
                        GridLines="none" CellPadding="5" CellSpacing="0" CssClass="table table-condensed table-striped table-hover" AllowPaging="true"
                        PageSize="10" OnPageIndexChanged="dgCommentHistory_PageIndexChanged">
                        <Columns>
                            <asp:TemplateColumn HeaderText="Comment Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedDate" runat="server">
						            <%# Eval("ModifiedDate") == null ? Eval("CreatedDate", "{0: dd-MMM-yyyy}") : Eval("CreatedDate", "{0: dd-MMM-yyyy}")%>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>

                            <asp:TemplateColumn HeaderText="Comment">
                                <ItemTemplate>
                                    <asp:Label ID="lblComment2" runat="server">
                                                                <%# FixCrLf(Eval("Comment").ToString())%>
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
