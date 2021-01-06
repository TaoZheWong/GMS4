<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="DebtorsSearch.aspx.cs" Inherits="GMSWeb.Debtors.Debtors.DebtorsSearch" Title="Debtors - Search" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <ul class="breadcrumb pull-right">
        <li><a href="#">Customer Info</a></li>
        <li class="active">Customer Detail</li>
    </ul>
    <h1 class="page-header">Customer Detail <br /><small>Search for customer's details or add new prospect. The customer listing also includes prospects from Quotation module.</small></h1>

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
                <div class="form-group col-lg-4 col-sm-6">
                    <label class="col-sm-6 control-label text-left">Customer Code</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtAccountCode" MaxLength="10" Columns="20" onfocus="select();"
                            CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group col-lg-8 col-sm-6">
                    <label class="col-sm-3 control-label text-left">Customer Name</label>
                    <div class="col-sm-9">
                        <asp:TextBox runat="server" ID="txtAccountName" MaxLength="50" Columns="50" onfocus="select();"
                            CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
             </div>
        </div>
          <div class="panel-footer clearfix">
              <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
                    OnClick="btnSearch_Click"></asp:Button>&nbsp;
            <asp:Button ID="btnAddProspect" Text="Add Prospect" EnableViewState="False" runat="server" CssClass="btn btn-default pull-right m-r-10"
                OnClick="btnAddProspect_Click"></asp:Button>
        </div>
    </div>  
          
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
                        GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" AllowPaging="true"
                        PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true" OnSortCommand="SortData" AllowSorting="true">
                        <Columns>
                            <asp:TemplateColumn HeaderText="No">
                                <ItemTemplate>
                                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Customer Code" SortExpression="AccountCode" HeaderStyle-Wrap="false" >
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "AccountManagement.aspx?AccountCode="+Eval("AccountCode")%>'><%# Eval("AccountCode")%></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Customer Name" SortExpression="AccountName" HeaderStyle-Wrap="false" >
                                <ItemTemplate>
                                    <%# Eval("AccountName")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Sales Person" SortExpression="SalesPerson" HeaderStyle-Wrap="false" >
                                <ItemTemplate>
                                    <%# Eval("SalesPerson")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Credit Term" SortExpression="CreditTerm" HeaderStyle-Wrap="false" >
                                <ItemTemplate>
                                    <%# Eval("CreditTerm")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Credit Limit" SortExpression="CreditLimit" HeaderStyle-Wrap="false" >
                                <ItemTemplate>
                                    <%# Eval("CreditLimit")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Default Currency" SortExpression="DefaultCurrency" HeaderStyle-Wrap="false" >
                                <ItemTemplate>
                                    <%# Eval("DefaultCurrency")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                              <asp:TemplateColumn HeaderText="Office Phone" SortExpression="OfficePhone" HeaderStyle-Wrap="false" >
                                <ItemTemplate>
                                    <%# Eval("OfficePhone")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Fax" SortExpression="Fax" HeaderStyle-Wrap="false" >
                                <ItemTemplate>
                                    <%# Eval("Fax")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Industry" SortExpression="Industry" HeaderStyle-Wrap="false" >
                                <ItemTemplate>
                                    <%# Eval("Industry")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Country" SortExpression="Country" HeaderStyle-Wrap="false" >
                                <ItemTemplate>
                                    <%# Eval("Country")%>
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
            $(".sub-debtors-search").addClass("active");
        });
    </script>
</asp:Content>
