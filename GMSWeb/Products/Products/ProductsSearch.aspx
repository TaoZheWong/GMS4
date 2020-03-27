<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="ProductsSearch.aspx.cs" Inherits="GMSWeb.Products.Products.ProductsSearch" Title="Products - Search" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <ul class="breadcrumb pull-right">
        <li><a href="#"><asp:Label ID="lblPageHeader" runat="server" /></a></li>
        <li class="active">Product Search</li>
    </ul>
    <h1 class="page-header">Product Search <br />
        <small>Search existing product status in real time. This module directly linked to SAP/A21 system.</small></h1>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="note note-info">
        <h4 class="block"><i class="ti-info-alt"></i> Info! </h4>
        <p>Available = On Hand - On SO</p>
        <input type='hidden' id='hidCoyID' runat="server" />
        <input type='hidden' id='hidUserID' runat="server" />
    </div>
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
                <div class="form-group col-lg-4 col-md-6 col-sm-6 hidden">
                    <label class="control-label">Category</label>
                        <asp:TextBox runat="server" ID="TextBox1" MaxLength="20" Columns="20" onfocus="select();" CssClass="form-control" placeholder=""></asp:TextBox>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Brand/Product Code</label>
                        <asp:TextBox runat="server" ID="txtProductGroupCode" MaxLength="4" Columns="20" onfocus="select();"
                            CssClass="form-control" placeholder="e.g. B11"></asp:TextBox>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Brand/Product Name</label>
                        <asp:TextBox runat="server" ID="txtProductGroup" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="form-control" placeholder="e.g. BLUEMETALS"></asp:TextBox>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Item Code</label>
                        <asp:TextBox runat="server" ID="txtProductCode" MaxLength="20" Columns="20" onfocus="select();" CssClass="form-control" placeholder="e.g. B1110535616"></asp:TextBox>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Item Name</label>
                        <asp:TextBox runat="server" ID="txtProductName" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="form-control" placeholder="e.g. BLUE-TIG 5356"></asp:TextBox>
                </div>
                <div class="form-group col-lg-4 col-md-6 col-sm-6 hidden">
                    <label class="control-label">Product Manager</label>
                        <asp:TextBox runat="server" ID="txtProductManager" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6" hidden>
                    <label class="control-label">Product Foreign Name</label>
                        <asp:TextBox runat="server" ID="txtProductForeignName" MaxLength="50" Columns="20" onfocus="select();"
                                    CssClass="form-control" placeholder="e.g. BLUE-TIG 5356"></asp:TextBox>
                </div>
                
                <div class="form-group col-lg-3 col-md-6 col-sm-6 hidden">
                    <label class="control-label">Warehouse</label>
                        <asp:TextBox runat="server" ID="txtWarehouse" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="form-control" placeholder="e.g. 02"></asp:TextBox>
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
                                <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Item Code" SortExpression="ProductCode" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "ProductDetail.aspx?ProductCode="+Eval("ProductCode")+"&CurrentLink="+Request.Params["CurrentLink"].ToString().Trim()%>'><%# Eval("ProductCode")%></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Item Name" SortExpression="ProductName" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblProductName" runat="server">
                                                        <%# Eval("ProductName")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Brand/Product" SortExpression="ProductGroupName" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblProductGroup" runat="server">
                                                        <%# Eval("ProductGroupName")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="UOM" HeaderStyle-Wrap="false" SortExpression="UOM">
                            <ItemTemplate>
                                <asp:Label ID="lblUOM" runat="server">
                                                        <%# Eval("UOM")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="On Hand" HeaderStyle-Wrap="false" SortExpression="OnHandQuantity">
                            <ItemTemplate>
                                <asp:Label ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true" CausesValidation="false">
                                    <input type='hidden' value='<%# hidCoyID.Value + ";" + Eval("ProductCode").ToString() + ";" + hidUserID.Value%>' />            
                                    <a data-toggle='popover' data-trigger='hover' title='<%# "Stock Status For "+Eval("ProductCode")%>' data-content='' data-html='true' onMouseOver="getProductDetails(this)"><%# Eval("OnHandQuantity")%></a>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="On SO" HeaderStyle-Wrap="false" SortExpression="OnOrderQuantity">
                            <ItemTemplate>
                                <asp:Label ID="lblOnSO" runat="server">
                                    <asp:LinkButton ID="lnkViewSO" runat="server" OnCommand="lnkViewSO_Click" CommandArgument='<%#Eval("ProductCode")%>'>
                                        <%# Eval("OnOrderQuantity")%>
                                    </asp:LinkButton>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="On BO" HeaderStyle-Wrap="false" SortExpression="OnBOQuantity">
                            <ItemTemplate>
                               <asp:Label ID="lblOnBO" runat="server">
                                    <asp:LinkButton ID="lnkViewBO" runat="server" OnCommand="lnkViewBO_Click" CommandArgument='<%#Eval("ProductCode")%>' CssClass="btn-link btn-xs no-padding">
                                        <%# Eval("OnBOQuantity")%>
                                    </asp:LinkButton>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="On PO" HeaderStyle-Wrap="false" SortExpression="OnPOQuantity">
                            <ItemTemplate>
                                <asp:Label ID="lblOnPO" runat="server">
                                    <asp:LinkButton ID="lnkViewPO" runat="server" OnCommand="lnkViewPO_Click" CommandArgument='<%#Eval("ProductCode")%>' CssClass="btn-link btn-xs no-padding">
                                        <%# Eval("OnPOQuantity")%>
                                    </asp:LinkButton>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Available" HeaderStyle-Wrap="false" SortExpression="AvailableQuantity">
                            <ItemTemplate>
                                <asp:Label ID="lblAvailable" runat="server">
                                    <%# Eval("AvailableQuantity")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Dealer Price" HeaderStyle-Wrap="true" SortExpression="DealerPrice">
                            <ItemTemplate>
                                <asp:Label ID="lblDealerPrice" runat="server">
                                    <%# Eval("DealerPrice")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="User Price" HeaderStyle-Wrap="true" SortExpression="UserPrice">
                            <ItemTemplate>
                                <asp:Label ID="lblUserPrice" runat="server">
                                    <%# Eval("UserPrice")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Retail Price" HeaderStyle-Wrap="true" SortExpression="RetailPrice">
                            <ItemTemplate>
                                <asp:Label ID="lblRetailPrice" runat="server">
                                    <%# Eval("RetailPrice")%>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <%--<asp:TemplateColumn HeaderText="Active" HeaderStyle-Wrap="false" SortExpression="IsActive">
                            <ItemTemplate>                                
                                <asp:CheckBox Enabled="false" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsActive"))%>'></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>--%>
                    </Columns>
                    <HeaderStyle CssClass="tHeader" />
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
            $(".sub-product-search-sale").addClass("active");
        });

        function getProductDetails(element) {
            var context = $(element).prev().val();

            $.ajax({
                async: true,
                url: "ProductsSearch.aspx/GetDynamicContent",
                data: JSON.stringify({ "contextKey": context }),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $(element).attr('data-content', data);
                    $('[data-toggle="popover"]').popover();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Product Detail Error");
                }
            });
        }
    </script>
</asp:Content>
