<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="CustomerGroupTypeSetup.aspx.cs" Inherits="GMSWeb.Sales.Sales.CustomerGroupTypeSetup" %>

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
        <li class="active">Customer Set Up</li>
    </ul>
    <h1 class="page-header">Customer Group & Type Update<small> Setup of Customer Group & Type.</small></h1>

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
                    AllowPaging="True" AutoGenerateColumns="False" Skin="Bootstrap" AllowFilteringByColumn="true" PageSize="20" OnCancelCommand="radGrid_OnCancel"
                    OnNeedDataSource="RadGrid1_NeedDataSource" OnUpdateCommand="RadGrid1_OnUpdateCommand" OnItemCreated="RadGrid1_OnItemCreated"
                    OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand" OnFilterCheckListItemsRequested="RadGrid1_FilterCheckListItemsRequested" 
                    FilterType="HeaderContext"  EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true">
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="AccountCode" CommandItemSettings-ShowRefreshButton="true" CommandItemSettings-ShowAddNewRecordButton="false" EditMode="InPlace">

                        <Columns>
                            <telerik:GridEditCommandColumn UniqueName="EditCommandColumn1">
                                <HeaderStyle Width="10px"></HeaderStyle>
                                <ItemStyle CssClass="MyImageButton"></ItemStyle>
                            </telerik:GridEditCommandColumn>
                            <telerik:GridTemplateColumn SortExpression="DetailItemNo" AllowFiltering="false" HeaderText="S/N" HeaderButtonType="TextButton" HeaderStyle-Width="8%"
                                DataField="DetailItemNo">
                                <ItemTemplate>
                                    <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn SortExpression="Division" FilterCheckListEnableLoadOnDemand="true" AllowFiltering="true" HeaderText="D1" HeaderButtonType="TextButton"
                                DataField="Division" UniqueName="Division" HeaderStyle-Width="12%" FilterControlWidth="70%" ReadOnly="true" GroupByExpression="Division group by Division">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="AccountCode" FilterCheckListEnableLoadOnDemand="true" AllowFiltering="true" HeaderText="Code" HeaderButtonType="TextButton"
                                DataField="AccountCode" UniqueName="AccountCode" HeaderStyle-Width="12%" FilterControlWidth="70%" ReadOnly="true" GroupByExpression="AccountCode group by AccountCode">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="AccountName" FilterCheckListEnableLoadOnDemand="true" AllowFiltering="true" HeaderText="Name" HeaderButtonType="TextButton"
                                DataField="AccountName" UniqueName="AccountName" HeaderStyle-Width="50%" FilterControlWidth="70%" ReadOnly="true" GroupByExpression="AccountName group by AccountName">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn SortExpression="Type" FilterCheckListEnableLoadOnDemand="true" AllowFiltering="true" HeaderText="Type"
                                HeaderButtonType="TextButton" DataField="Type" UniqueName="Type" HeaderStyle-Width="15%" FilterControlWidth="70%" GroupByExpression="Type group by Type">
                                <ItemTemplate>
                                    <%# Eval("ClassName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlType" CssClass="form-control input-sm"
                                         DataTextField="ClassName" DataValueField="ClassID" ></asp:DropDownList>
                                    <input type="hidden" id="hidType" runat="server" value='<%# Eval("Type")%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn SortExpression="Group" FilterCheckListEnableLoadOnDemand="true"  HeaderText="Group" HeaderButtonType="TextButton"
                                DataField="Group" UniqueName="Group" HeaderStyle-Width="15%" FilterControlWidth="70%" GroupByExpression="Group group by Group">
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
            $(".sub-customerSetup").addClass("active");
        });
    </script>
</asp:Content>
