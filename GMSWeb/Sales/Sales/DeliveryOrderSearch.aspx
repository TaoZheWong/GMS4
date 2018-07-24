<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true"
    CodeBehind="DeliveryOrderSearch.aspx.cs" Inherits="GMSWeb.Sales.Sales.DeliveryOrderSearch" Title="Delivery Order - Search" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="breadcrumb pull-right">
        <li><a href="#">Sales</a></li>
        <li class="active">Delivery Order</li>
    </ul>
    <h1 class="page-header">Delivery Order <br /><small>Search customers' Delivery Order in <b>real time</b>. This module directly linked to A21 system.</small></h1>

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
        <div class="panel-body">
            <div class=" m-t-20">
                <div class="row">
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Trn Date From</label>
                            <div class="input-group date">
                                <asp:TextBox runat="server" ID="trnDateFrom" MaxLength="10" Columns="10" onfocus="select();"
                                    CssClass="form-control datepicker"></asp:TextBox>
                                <span class="input-group-addon"><i class="ti-calendar"></i></span>
                            </div>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Trn Date Tp</label>
                            <div class="input-group date">
                                <asp:TextBox runat="server" ID="trnDateTo" MaxLength="10" Columns="10" onfocus="select();"
                                    CssClass="form-control datepicker"></asp:TextBox>
                                <span class="input-group-addon"><i class="ti-calendar"></i></span>
                            </div>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Customer Code</label>
                            <asp:TextBox runat="server" ID="txtAccountCode" MaxLength="20" Columns="20" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. DLK690"></asp:TextBox>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Customer Name</label>
                            <asp:TextBox runat="server" ID="txtAccountName" MaxLength="50" Columns="20" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. Keppel"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Product Code</label>
                            <asp:TextBox runat="server" ID="txtProductCode" MaxLength="20" Columns="20" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. B1110535616"></asp:TextBox>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Product Name</label>
                            <asp:TextBox runat="server" ID="txtProductName" MaxLength="50" Columns="20" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. BLUE-TIG 5356"></asp:TextBox>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Product Detail Description</label>
                            <asp:TextBox runat="server" ID="txtProductDescription" MaxLength="50" Columns="20" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. U10053MF108"></asp:TextBox>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Product Group Code</label>
                            <asp:TextBox runat="server" ID="txtProductGroup" MaxLength="50" Columns="20" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. B11"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Product Group Name</label>
                            <asp:TextBox runat="server" ID="txtProductGroupName" MaxLength="50" Columns="20" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. BLUEMETALS"></asp:TextBox>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Customer PO No</label>
                            <asp:TextBox runat="server" ID="txtCustomerPONo" MaxLength="50" Columns="20" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. PO123456"></asp:TextBox>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                    <label class="control-label">DO No</label>
                        <asp:TextBox runat="server" ID="txtDONumber" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="form-control" placeholder="e.g. 023456"></asp:TextBox>
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
                    <asp:TemplateColumn HeaderText="Trn Date" SortExpression="TrnDate" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("TrnDate", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Ref No" SortExpression="TrnNo" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "ViewDeliveryOrder.aspx?TrnNo="+Eval("TrnNo")+"&TrnType="+Eval("TrnType")+"&DBVersion="+Eval("DBVersion")+"&StatusType="+Eval("StatusType")%>'><%# Eval("TrnNo")%></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="DO No" SortExpression="DONo" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label ID="lblDONo" runat="server" Width="70px">
                                                                        <%# Eval("DONo")%>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Customer Code" SortExpression="AccountCode" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label ID="lblProductName" runat="server">
                                                        <%# Eval("AccountCode")%>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Customer Name" SortExpression="AccountName" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label ID="lblProductName" runat="server">
                                                        <%# Eval("AccountName")%>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Customer PO No" SortExpression="Custponumber" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label ID="lblCustomerPO" runat="server" Width="90px">
                                                        <%# Eval("Custponumber")%>            
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="DB Version" SortExpression="DBVersion" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label ID="lblDBVersion" runat="server">
                                                        <%# Eval("DBVersion")%>
                            </asp:Label>
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
            $(".sale-menu").addClass("active expand");
            $(".sub-delivery-order").addClass("active");
        });
    </script>
</asp:Content>
