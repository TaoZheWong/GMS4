<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="ProductMovement.aspx.cs" Inherits="GMSWeb.Products.Products.ProductMovement" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <ul class="breadcrumb pull-right">
        <li><a href="#">Products</a></li>
        <li class="active">Product Movement</li>
    </ul>
    <h1 class="page-header">Product Movement <br />
        <small>Search existing product movement in real time. This module directly linked to A21 system.</small>
    </h1>

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
                <div class="form-group col-lg-6 col-sm-6">
                    <label class="col-sm-4 control-label text-left">Product Code</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="txtProductCode" MaxLength="20" Columns="20" onfocus="select();"
                            CssClass="form-control" placeholder="e.g. B1110535616"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group col-lg-6 col-sm-6">
                    <label class="col-sm-4 control-label text-left">Warehouse</label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="txtWarehouse" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="form-control" placeholder="e.g. 02"></asp:TextBox>
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
        <div class="panel-body no-padding">
            <div class="table-responsive">
                <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false"
                    GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    PageSize="200" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true" OnSortCommand="SortData" AllowSorting="true" OnItemDataBound="dgData_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Product Code" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblProductCode" runat="server">
														    <%# Eval("ProductCode")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Product Name" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblProdName" runat="server">
														    <%# Eval("ProductName")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Trn Date" HeaderStyle-Wrap="false" ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:Label ID="lblTrnDate" runat="server">
														    <%# Eval("TrnDate", "{0:dd/MM/yyyy}")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Trn Type" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblTrnType" runat="server">
														    <%# Eval("TrnType")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Trn No." HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblTrnNo" runat="server">
														    <%# Eval("TrnNo")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Doc No." HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDocNo" runat="server">
														    <%# Eval("DocNo")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Received Qty" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblReceivedQty" runat="server">
														    <%# Eval("ReceivedQty")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Issued Qty" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblIssuedQty" runat="server">
														    <%# Eval("IssuedQty")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Warehouse" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblWarehouse" runat="server">
														    <%# Eval("Warehouse")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Balance Qty" HeaderStyle-Wrap="false">
							<ItemTemplate>
                                <asp:Label ID="lblBalanceQty" runat="server">
                                                <%# Eval("BalanceQty")%>
                                </asp:Label>
                            </ItemTemplate>
						</asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Remarks" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <%--<a href="#" title="View" onclick='viewCommentHistory("<%#  Eval("AccountCode")%>","<%# Eval("SALES_Currency")%>");return false;'>View</a>--%>
                                <%# Eval("Narration")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Account Name" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAccountName" runat="server">
														    <%# Eval("AccountName")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="DB Version" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDBVersion" runat="server">
														    <%# Eval("DBVersion")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="tHeader" HorizontalAlign="Center" />
                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                </asp:DataGrid>
            </div>
        </div>
        <div class="panel-footer clearfix">
            <asp:Button ID="btnExportToExcel" Text="Export To Excel" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
                OnClick="btnExportToExcel_Click" Visible="false"></asp:Button>
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
            $(".sub-product-movement").addClass("active");
        });
    </script>
</asp:Content>
