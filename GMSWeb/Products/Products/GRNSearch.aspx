<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="GRNSearch.aspx.cs" Inherits="GMSWeb.Products.Products.GRNSearch" Title="GRN - Search" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <ul class="breadcrumb pull-right">
        <li><a href="#"><asp:Label ID="lblPageHeader" runat="server" /></a></li>
        <li class="active">GRN Search</li>
    </ul>
    <h1 class="page-header">GRN Search <br />
        <small>Search existing GRN in real time. This module directly linked to SAP system.</small></h1>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--<div class="note note-info">
        <h4 class="block"><i class="ti-info-alt"></i> Info! </h4>
        <p>Available = On Hand - On SO</p>
        <input type='hidden' id='hidCoyID' runat="server" />
        <input type='hidden' id='hidUserID' runat="server" />
    </div>--%>
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
                <div class="form-group col-lg-3 col-md-6 col-sm-12">
                    <label class="control-label">GRN Date From</label>
                        <div class="input-group date">
                            <asp:TextBox runat="server" ID="txtGRNDateFrom" MaxLength="10" Columns="10" onfocus="select();"
                                CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon"><i class="ti-calendar"></i></span>
                        </div>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12">
                    <label class="control-label">GRN Date To</label>
                        <div class="input-group date">
                            <asp:TextBox runat="server" ID="txtGRNDateTo" MaxLength="10" Columns="10" onfocus="select();"
                                CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon"><i class="ti-calendar"></i></span>
                        </div>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">PO No</label>
                        <asp:TextBox runat="server" ID="txtPoNo" MaxLength="20" Columns="20" onfocus="select();"
                            CssClass="form-control" ></asp:TextBox>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">GRN No</label>
                        <asp:TextBox runat="server" ID="txtGRNNo" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Item Code</label>
                        <asp:TextBox runat="server" ID="txtProductCode" MaxLength="20" Columns="20" onfocus="select();" CssClass="form-control" placeholder="e.g. B1110535616"></asp:TextBox>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Item Description</label>
                        <asp:TextBox runat="server" ID="txtProductName" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="form-control" placeholder="e.g. BLUE-TIG 5356"></asp:TextBox>
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
                <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
            </h4>
        </div>
        <div class="panel-body no-padding">
            <div class="table-responsive">
                <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false"
                    GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true" OnSortCommand="SortData" AllowSorting="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSN" EnableViewState="true" Text='<%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>'>
                                    </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="GRN No.">
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "GRNDetail.aspx?TrnNo="+Eval("DocNum")+"&CurrentLink="+Request.Params["CurrentLink"].ToString().Trim()%>'><%# Eval("DocNum")%></asp:HyperLink>
                            </ItemTemplate>                                                    
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="GRN Date">
                            <ItemTemplate>
                                <%# Eval("DocDate")%>
                            </ItemTemplate>                                                    
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="PO No.">
                            <ItemTemplate>
                                <%# Eval("BaseRef")%>
                            </ItemTemplate>                                                    
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Vendor Code">
                            <ItemTemplate>
                                <%# Eval("CardCode")%>
                            </ItemTemplate>                                                    
                        </asp:TemplateColumn> 
                        <asp:TemplateColumn HeaderText="Vendor Name">
                            <ItemTemplate>
                                <%# Eval("CardName")%>
                            </ItemTemplate>                                                    
                        </asp:TemplateColumn>  
                        <asp:TemplateColumn HeaderText="Created By">
                            <ItemTemplate>
                                <%# Eval("U_AF_CREATEDBY")%>                                
                            </ItemTemplate>                                                    
                        </asp:TemplateColumn>                                            
                    </Columns>
                    <HeaderStyle CssClass="tHeader" />
                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                </asp:DataGrid>
            </div>
        </div>
        <div class="panel-footer clearfix">
            
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
            $(".products-menu").addClass("active expand");
            $(".sub-GRNSearch").addClass("active");
        });

    </script>
</asp:Content>
