<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="FinanceSearch.aspx.cs" Inherits="GMSWeb.Debtors.Debtors.FinanceSearch" Title="Finance Search" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="breadcrumb pull-right">
        <li><a href="#">Customer Info</a></li>
        <li class="active">Finance Search</li>
    </ul>
    <h1 class="page-header">Finance Search <br />
        <small>Search for customer's ROC / Finance attachment. </small></h1>


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-search"></i>
                Search filter
            </h4>
        </div>
        <div class="panel-body row">
            <div class="form-horizontal m-t-20">
                <div class="form-group col-lg-6 col-md-6 col-sm-6">
                    <label class="col-sm-4 control-label text-left">Customer Name</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="txtAccountName" MaxLength="50" Columns="50" onfocus="select();"
                            CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer clearfix">
            <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
                OnClick="btnSearch_Click"></asp:Button>
        </div>
    </div>


    <div class="panel panel-primary" id="resultList" runat="server" visible="false">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-align-justify"></i>
                <asp:Label ID="lblFinanceAttachmentSummary" Visible="false" runat="server" />
            </h4>
        </div>
        <div class="table-responsive">
            <asp:DataGrid ID="dgFinanceAttachment" runat="server" AutoGenerateColumns="false"
                GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" AllowPaging="true"
                PageSize="20" OnPageIndexChanged="dgFinanceAttachment_PageIndexChanged" EnableViewState="true" OnSortCommand="SortFinanceAttachment" AllowSorting="true"
                OnItemDataBound="dgFinanceAttachment_ItemDataBound" OnItemCommand="dgFinanceAttachment_Command">
                <Columns>
                    <asp:TemplateColumn HeaderText="No">
                        <ItemTemplate>
                            <%# (Container.ItemIndex + 1) + ((dgFinanceAttachment.CurrentPageIndex) * dgFinanceAttachment.PageSize) %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Type" SortExpression="Type" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("Type")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Country" SortExpression="Territory" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("Territory")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Account Name" SortExpression="AccountName" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("AccountName")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Period From : To" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# string.IsNullOrEmpty(Eval("PeriodYearFrom").ToString())  ? "NIL" : new DateTime(Convert.ToInt32(Eval("PeriodYearFrom")), Convert.ToInt32(Eval("PeriodMonthFrom")), 1).ToString("MMM-yyyy")+ " : "  + new DateTime(Convert.ToInt32(Eval("PeriodYearTo")), Convert.ToInt32(Eval("PeriodMonthTo")), 1).ToString("MMM-yyyy")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Attachment" SortExpression="DocumentName" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="linkName" runat="server" Text='<%# Eval("DocumentName")%>' CommandName="Load" ForeColor="#005DAA" CommandArgument='<%#Eval("DocumentName")%>'></asp:LinkButton>

                            <input type="hidden" id="hidDocumentID" runat="server" value='<%# Eval("ID")%>' />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Uploaded Date" SortExpression="CreatedDate" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("CreatedDate", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
                <HeaderStyle CssClass="tHeader" />
                <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
            </asp:DataGrid>
        </div>
    </div>
    <br />
    <div class="TABCOMMAND">
        <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".customer-info-menu").addClass("active expand");
            $(".sub-finance-search").addClass("active");
        });
    </script>
</asp:Content>