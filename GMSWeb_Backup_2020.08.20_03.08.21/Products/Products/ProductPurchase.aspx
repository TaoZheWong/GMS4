<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductPurchase.aspx.cs" Inherits="GMSWeb.Products.Products.ProductPurchase"
    Title="Inventory - Purchase Search" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="breadcrumb pull-right">
        <li><a href="#">Products</a></li>
        <li class="active">Supplier Invoice Detail</li>
    </ul>
    <h1 class="page-header">Supplier Invoice Detail <small>Search for purchase (i.e. supplier invoice) transactions.</small></h1>

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
            <div class="m-t-20">
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Trn Date From</label>
                        <div class="input-group date">
                            <asp:TextBox runat="server" ID="trnDateFrom" MaxLength="10" Columns="10" onfocus="select();"
                                CssClass="form-control datepicker"></asp:TextBox>
                            <span class="input-group-addon"><i class="ti-calendar"></i></span>
                        </div>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Trn Date To</label>
                        <div class="input-group date">
                            <asp:TextBox runat="server" ID="trnDateTo" MaxLength="10" Columns="10" onfocus="select();"
                                CssClass="form-control datepicker"></asp:TextBox>
                            <span class="input-group-addon"><i class="ti-calendar"></i></span>
                        </div>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Supplier Code</label>
                        <asp:TextBox runat="server" ID="txtSupplierAccountCode" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Supplier Name</label>
                        <asp:TextBox runat="server" ID="txtSupplierAccountName" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Product Code</label>
                        <asp:TextBox runat="server" ID="txtProductCode" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Product Name</label>
                        <asp:TextBox runat="server" ID="txtProductName" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Product Group (Brand)</label>
                        <asp:TextBox runat="server" ID="txtProductGroup" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Product Manager</label>
                        <asp:DropDownList ID="ddlProductManager" runat="Server" DataTextField="ProductManagerName" DataValueField="ProductManagerUserID" CssClass="form-control" />
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
                <asp:Label ID="lblSearchSummary" Visible="false" runat="server"></asp:Label>
            </h4>
        </div>
        <div class="table-responsive">
            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" GridLines="none"
                CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                <Columns>
                    <asp:TemplateColumn HeaderText="No">
                        <ItemTemplate>
                            <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Trn Date" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("TrnDate", "{0:dd/MM/yy}")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Ref No." HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("RefNo")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Prod Code" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label ID="lblProductCode" runat="server">
                                                                <%# Eval("ProductCode")%>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Prod Name" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label ID="lblProductName" runat="server">
                                                                <%# Eval("ProductName")%>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Brand" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label ID="lblProductGroup" runat="server">
                                                                <%# Eval("ProductGroupName")%>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Supplier" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("SupplierAccountCode")%> <%# Eval("SupplierAccountName")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Unit Price" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("ForeignCurrencySign") + Eval("UnitAmount", "{0:N}")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Qty" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("Qty")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Total" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("ForeignCurrencySign") + Eval("Total", "{0:N}")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("HomeCurrencySign") + Eval("HomeTotal", "{0:N}")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
                <HeaderStyle CssClass="tHeader" />
                <PagerStyle  Mode="NumericPages"  CssClass="grid_pagination" />
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
            $(".products-menu").addClass("active expand");
            $(".sub-sup-in-detail").addClass("active");
        });
    </script>
</asp:Content>
