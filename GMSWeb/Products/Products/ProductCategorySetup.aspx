<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="ProductCategorySetup.aspx.cs" Inherits="GMSWeb.Products.Products.ProductCategorySetup" %>

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
        <li class="active">Product Category Set Up</li>
    </ul>
    <h1 class="page-header">Product Category Set Up<small> Setup of Product Category detail.</small></h1>

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
                    AllowPaging="True" Skin="Bootstrap" AllowFilteringByColumn="true" AutoGenerateColumns="false" PageSize="20" OnCancelCommand="radGrid_OnCancel"
                    OnNeedDataSource="RadGrid1_NeedDataSource" OnUpdateCommand="RadGrid1_OnUpdateCommand" OnItemCommand="RadGrid1_ItemCommand" AllowSorting="True" 
                    OnFilterCheckListItemsRequested="RadGrid1_FilterCheckListItemsRequested" 
                    FilterType="HeaderContext"  EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true">

                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="CategoryID" AllowMultiColumnSorting="true" EditMode="InPlace"
                        CommandItemSettings-ShowAddNewRecordButton="false" CommandItemSettings-ShowRefreshButton="true">
                        <Columns>
                            <telerik:GridEditCommandColumn UniqueName="EditCommandColumn1">
                                <HeaderStyle Width="10px"></HeaderStyle>
                                <ItemStyle CssClass="MyImageButton"></ItemStyle>
                            </telerik:GridEditCommandColumn>
                            <telerik:GridTemplateColumn   HeaderText="S/N" AllowFiltering="false" HeaderButtonType="TextButton" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn SortExpression="CategoryName" FilterCheckListEnableLoadOnDemand="true" HeaderText="Product Category" HeaderButtonType="TextButton"
                                DataField="CategoryName" UniqueName="CategoryName" HeaderStyle-Width="50%" FilterControlWidth="70%" ReadOnly="true" GroupByExpression="CategoryName group by CategoryName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="ShortName" FilterCheckListEnableLoadOnDemand="true"  HeaderText="Short Name" HeaderButtonType="TextButton"
                                DataField="ShortName" UniqueName="ShortName" HeaderStyle-Width="30%" FilterControlWidth="70%" GroupByExpression="ShortName group by ShortName">
                            </telerik:GridBoundColumn>
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
            $(".sub-productCategorySetup").addClass("active");
        });
    </script>
</asp:Content>
