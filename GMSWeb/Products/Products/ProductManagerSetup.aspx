<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="ProductManagerSetup.aspx.cs" Inherits="GMSWeb.Products.Products.ProductMangerSetup" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <style>
        html body .RadMenu.RadMenu_Context.GridContextMenu{
            height:500px !important;
            overflow:scroll !important;
        }
    </style>
    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">
            <asp:Label ID="lblPageHeader" runat="server" /></a></li>
        <li class="active">Product Manager Set Up</li>
    </ul>
    <h1 class="page-header">Product Manager Set Up<small> Setup of Product Manager.</small></h1>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="MsgPanel2" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="panel panel-primary" id="resultList" runat="server" visible="true">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-align-justify"></i>
                <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
            </h4>
        </div>
        <div class="panel-body no-padding">
            <div class="table-responsive" style="overflow: auto">
                <telerik:RadGrid ID="RadGrid1" runat="server" Visible="True" OnPageIndexChanged="radGrid_OnPageIndexChanged" OnPageSizeChanged="radGrid_OnPageSizeChanged"
                    AllowPaging="True" Skin="Bootstrap" AllowFilteringByColumn="true" AutoGenerateColumns="false" PageSize="20" OnCancelCommand="radGrid_OnCancel" OnItemCommand="RadGrid1_ItemCommand"
                    OnNeedDataSource="RadGrid1_NeedDataSource" OnUpdateCommand="RadGrid1_OnUpdateCommand" AllowSorting="True" OnItemCreated="RadGrid1_OnItemCreated" OnItemDataBound="RadGrid1_ItemDataBound"
                    OnFilterCheckListItemsRequested="RadGrid1_FilterCheckListItemsRequested" 
                    FilterType="HeaderContext"  EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true">
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="ProductGroupCode" AllowMultiColumnSorting="true"
                        CommandItemSettings-ShowAddNewRecordButton="false" CommandItemSettings-ShowRefreshButton="true">
                        <Columns>
                            <telerik:GridTemplateColumn SortExpression="DetailItemNo" AllowFiltering="false" HeaderText="S/N" HeaderButtonType="TextButton" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn SortExpression="ProductGroupName" FilterCheckListEnableLoadOnDemand="true" AllowFiltering="true" HeaderText="Brand/Product Name"
                                HeaderButtonType="TextButton" DataField="ProductGroupName" UniqueName="ProductGroupName" GroupByExpression="ProductGroupName group by ProductGroupName">
                                <ItemTemplate>
                                    <%# Eval("ProductGroupName") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn SortExpression="PMName" FilterCheckListEnableLoadOnDemand="true" AllowFiltering="true" HeaderText="PM Name"
                                HeaderButtonType="TextButton" DataField="PMName" UniqueName="PMName" GroupByExpression="PMName group by PMName">
                                <ItemTemplate>
                                    <telerik:RadDropDownList runat="server" ID="ddlPM" DataValueField="MGUserID" AutoPostBack="True" Skin="Bootstrap" Width="100%"
                                        OnClientSelectedIndexChanged="onClientSelectedIndexChanging" DataTextField="PMName" OnSelectedIndexChanged="ddlPM_onSelectedIndexChanged" />
                                    <input type="hidden" id="hidPM" runat="server" value='<%# Eval("PMUserID")%>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
        <div class="TABCOMMAND">
            <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".setting-menu").addClass("active expand");
            $(".sub-productManagerSetup").addClass("active");
        });

        var showConfirm = 1;

        function onClientSelectedIndexChanging(item) {
            if (!confirm('Confirm to update PM?')) {
                args.set_cancel(true);
                item.selectedIndex = item.oldIndex;
            }
        }
    </script>
</asp:Content>
