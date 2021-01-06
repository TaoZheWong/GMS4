<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="ProductionVolumeInput.aspx.cs" Inherits="GMSWeb.Finance.ProductVolume.RefillingProductVolumeInput" %>

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
        html body .RadMenu.RadMenu_Context.GridContextMenu {
            height: 500px !important;
            overflow: scroll !important;
        }
    </style>
    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">
            <asp:Label ID="lblPageHeader" runat="server" /></a></li>
        <li class="active">Production Volume Budget</li>
    </ul>
    <h1 class="page-header">Production Volume Budget<small></small></h1>

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
                <div class="form-horizontal m-t-20">
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Date From</label><br />
                        <telerik:RadDatePicker runat="server" ID="txtDateFrom" RenderMode="Lightweight" Width="100%" Font-Size="Small">
                        </telerik:RadDatePicker>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Date To</label><br />
                        <telerik:RadDatePicker runat="server" ID="txtDateTo" RenderMode="Lightweight" Width="100%" Font-Size="Small">
                        </telerik:RadDatePicker>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer clearfix">
            <telerik:RadButton runat="server" ID="btnSearch" Text="Search" CssClass="pull-right btn btn-lg m-l-5 bg-primary largeFont" Skin="Bootstrap"
                OnClick="btnSearch_Click" SingleClick="true" SingleClickText="Retrieving Data..." RenderMode="Lightweight">
            </telerik:RadButton>
        </div>
    </div>


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
                    AllowPaging="True" AutoGenerateColumns="False" Skin="Bootstrap" AllowFilteringByColumn="true" PageSize="50" OnCancelCommand="radGrid_OnCancel"
                    OnNeedDataSource="RadGrid1_NeedDataSource" OnInsertCommand="RadGrid1_InsertCommand" OnItemCreated="RadGrid1_OnItemCreated"
                    OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand" AllowSorting="True" OnFilterCheckListItemsRequested="RadGrid1_FilterCheckListItemsRequested"
                    FilterType="HeaderContext" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true">

                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="ID" AllowFilteringByColumn="true"
                        CommandItemSettings-ShowAddNewRecordButton="true" CommandItemSettings-ShowRefreshButton="true">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="S/N" HeaderButtonType="TextButton" HeaderStyle-Width="3%" AllowFiltering="false">
                                <ItemTemplate>
                                    <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn SortExpression="ProductDate" FilterCheckListEnableLoadOnDemand="true" HeaderText="Date" HeaderButtonType="TextButton"
                                DataField="ProductDate" UniqueName="ProductDate" HeaderStyle-Width="20%" FilterControlWidth="70%" GroupByExpression="ProductDate group by ProductDate">
                                <ItemTemplate>
                                    <telerik:RadDatePicker runat="server" ID="txtDate" RenderMode="Lightweight" SelectedDate ='<%#Eval("ProductDate") %>' Width="100%">
                                    </telerik:RadDatePicker>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <telerik:RadDatePicker runat="server" ID="txtDate" RenderMode="Lightweight" Width="100%">
                                    </telerik:RadDatePicker>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn SortExpression="Product" FilterCheckListEnableLoadOnDemand="true" HeaderText="Product" HeaderButtonType="TextButton"
                                DataField="Product" UniqueName="Product" HeaderStyle-Width="20%" FilterControlWidth="70%" GroupByExpression="Product group by Product"
                                EditFormHeaderTextFormat="<b>Product:</b>">
                                <ItemTemplate>
                                    <telerik:RadDropDownList runat="server" ID="ddlProduct" RenderMode="Lightweight"
                                        DataValueField="ID" DataTextField="DisplayName" Width="100%">
                                    </telerik:RadDropDownList>
                                    <input type="hidden" runat="server" id="hidProduct" value='<%#Eval("ProductID") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadDropDownList runat="server" ID="ddlNewProduct" RenderMode="Lightweight" DataValueField="ID" DataTextField="DisplayName" Width="100%">
                                    </telerik:RadDropDownList>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn SortExpression="UOM" FilterCheckListEnableLoadOnDemand="true" HeaderText="Production UOM" HeaderButtonType="TextButton"
                                DataField="UOM" UniqueName="UOM" HeaderStyle-Width="13%" FilterControlWidth="70%" GroupByExpression="UOM group by UOM" ReadOnly="true">
                            </telerik:GridBoundColumn>

                            <telerik:GridTemplateColumn SortExpression="Volume" FilterCheckListEnableLoadOnDemand="true" HeaderText="Volume" HeaderButtonType="TextButton"
                                DataField="Volume" UniqueName="Volume" HeaderStyle-Width="20%" FilterControlWidth="70%" GroupByExpression="Volume group by Volume">
                                <ItemTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtVolume" runat="server" Text='<%# Eval("Volume") %>' TextMode="SingleLine"
                                        InputType="Number" EmptyMessage="type here">
                                    </telerik:RadTextBox>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtVolume" runat="server" InputType="Number" TextMode="SingleLine" EmptyMessage="type here">
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridButtonColumn CommandName="Save" Text="Save" UniqueName="SaveColumn" Visible="true" HeaderStyle-Width="5%">
                            </telerik:GridButtonColumn>
                            <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" Visible="true" HeaderStyle-Width="5%"
                                ConfirmText="Confirm to delete this data?">
                            </telerik:GridButtonColumn>
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
            $(".manufacture-menu").addClass("active expand");
            $(".sub-ProductionVolume-input").addClass("active");
        });
    </script>
</asp:Content>
