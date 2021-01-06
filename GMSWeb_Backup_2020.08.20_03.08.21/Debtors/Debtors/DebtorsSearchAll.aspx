<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="DebtorsSearchAll.aspx.cs" Inherits="GMSWeb.Debtors.Debtors.DebtorsSearchAll" Title="Debtors - Search All" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="breadcrumb pull-right">
        <li><a href="#">Customer Info</a></li>
        <li class="active">Search</li>
    </ul>
    <h1 class="page-header">Search  <br />
        <small>
       Search for company's customers by keying in partial customer name. Search result includes prospects from the Quotation. </small></h1>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:Panel runat="server" ID="pnl1" DefaultButton="btnSearch">
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
                <div class="m-t-20">
                    <div class="form-group col-lg-6  col-md-6 col-sm-6">
                        <label class="col-sm-4 control-label text-left"> Customer Name</label>
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
    </asp:Panel>

    <div class="panel panel-primary" id="resultList" runat="server" visible="false">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-align-justify"></i>
                <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
            </h4>
        </div>
        <div class="table-responsive">
            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false"
                GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover"
                PageSize="20" EnableViewState="true">
                <Columns>
                    <asp:TemplateColumn HeaderText="No">
                        <ItemTemplate>
                            <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Account Code" SortExpression="AccountCode" HeaderStyle-Wrap="false" >
                        <ItemTemplate>
                            <%# Eval("AccountCode")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Account Name" SortExpression="AccountName" HeaderStyle-Wrap="false" >
                        <ItemTemplate>
                            <%# Eval("AccountName")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Sales Person" SortExpression="SalesPerson" HeaderStyle-Wrap="false" >
                        <ItemTemplate>
                            <%# Eval("SalesPerson")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
                <HeaderStyle CssClass="tHeader" />
                <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
            </asp:DataGrid>
        </div>
    </div>

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
            $(".sub-debtors-search-all").addClass("active");
        });
    </script>
</asp:Content>
