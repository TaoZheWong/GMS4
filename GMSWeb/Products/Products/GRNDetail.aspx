<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="GRNDetail.aspx.cs" Inherits="GMSWeb.Products.Products.GRNDetail" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="breadcrumb pull-right">
        <li><a href="#">Products</a></li>
        <li class="active">GRN Detail</li> 
    </ul>
    <h1 class="page-header">GRN Detail </h1>

	<asp:ScriptManager ID="ScriptManager1" runat="server">
	</asp:ScriptManager>
	<input type="hidden" id="hidTrnNo" runat="server" />
	
    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                GRN Information
            </h4>
        </div>
        <div class="panel-body row">
            <div class="form-horizontal m-t-20">
                <div class="form-group col-lg-6 col-sm-12 col-md-12">
                    <label class="col-sm-6 control-label text-left">
                        GRN No:
                    </label>
                    <label class="col-sm-6 control-label text-left">
                       <asp:Label runat="server" ID="lblGRN"></asp:Label>
                    </label>
                </div>
                <div class="form-group col-lg-6 col-sm-12 col-md-12">
                    <label class="col-sm-6 control-label text-left">
                        GRN Date:
                    </label>
                    <label class="col-sm-6 control-label text-left">
                       <asp:Label runat="server" ID="lblGRNDate"></asp:Label>
                    </label>
                </div>
                <div class="form-group col-lg-6 col-sm-12 col-md-12">
                    <label class="col-sm-6 control-label text-left">
                        Vendor Code:
                    </label>
                    <label class="col-sm-6 control-label text-left">
                       <asp:Label runat="server" ID="lblVendorCode"></asp:Label>
                    </label>
                </div>
                <div class="form-group col-lg-6 col-sm-12 col-md-12">
                    <label class="col-sm-6 control-label text-left">
                        Vendor Name:
                    </label>
                    <label class="col-sm-6 control-label text-left">
                       <asp:Label runat="server" ID="lblVendorName"></asp:Label>
                    </label>
                </div>
            </div>
        </div>
        <div class="panel-footer clearfix">       
        </div>
    </div>
	
    <div runat="server" id="resultList"  visible="false" class="panel panel-primary">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-align-justify"></i> 
                        <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive">
                        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" GridLines="none"
                            CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" AllowPaging="true"
                            PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" OnItemDataBound="dgData_ItemDataBound" >
                            <Columns>
                                <asp:TemplateColumn HeaderText="No" >
                                    <ItemTemplate>
                                        <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Trn Date" HeaderStyle-Wrap="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblTrnDate" runat="server">
														<%# Eval("TrnDate", "{0:dd/MM/yyyy}")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Detail No" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDetailNo" runat="server" Text='<%# Eval("DetailNo")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="PO No." HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPONo" runat="server">
														<%# Eval("PONo")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="PO Date" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPODate" runat="server">
														<%# Eval("PODate")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Item Code" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductCode" runat="server">
														<%# Eval("ProductCode")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Item Description" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductDescription" runat="server">
														<%# Eval("ProductDescription")%>
                                        </asp:Label>          
                                        <div id="divBatchSeial" runat="server" visible="false">
										    <a id="lnkViewTNDetail" onclick="return ViewBatchSerial('<%# Eval("TrnNo")%>','<%# Eval("DetailNo")%>')" href="#">
										    View Batch / Serial</a>
                                        </div>                             
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Brand/Product Code" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductGroupCode" runat="server">
														<%# Eval("ProductGroupCode")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="Brand/Product Name" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductGroupName" runat="server">
														<%# Eval("ProductGroupName")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="UOM" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUOM" runat="server">
														<%# Eval("UOM")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="Quantity" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" runat="server">
														<%# Eval("Quantity") %>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="Warehouse" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWH" runat="server">
														<%# Eval("WH")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>   
                            </Columns>
                            <HeaderStyle CssClass="tHeader" />
                            <AlternatingItemStyle CssClass="tAltRow" />
                            <FooterStyle CssClass="tFooter" />
                            <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                        </asp:DataGrid>
                    </div>
                </div>
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
            $(".sub-GRNSearch").addClass("active");
        });
    </script>
</asp:Content>
