<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="BudgetExercise.aspx.cs" Inherits="GMSWeb.Budget.BudgetExercise" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="Contentpanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Contentpanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnVariance">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <!--Modal popup for variance-->
    <telerik:RadWindowManager ID="RadWindowManager2" runat="server">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" AutoSize="true" Skin="Bootstrap" Modal="true">
                <ContentTemplate>
                    <asp:Panel ID="Contentpanel" runat="server">
                        <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip2" Visible="true" runat="server" MultiPageID="RadMultiPage2" Skin="Bootstrap" Width="100%">
                            <Tabs>
                                <telerik:RadTab runat="server" Text="B12F & B15F" PageViewID="RadPageView3" Selected="true">
                                </telerik:RadTab>
                                <telerik:RadTab runat="server" Text="B13B & B16B" PageViewID="RadPageView4">
                                </telerik:RadTab>
                            </Tabs>
                        </telerik:RadTabStrip>
                        <telerik:RadMultiPage ID="RadMultiPage2" runat="server" SelectedIndex="0" Width="100%">
                            <telerik:RadPageView ID="RadPageView3" runat="server" CssClass="p-1">
                                <telerik:RadGrid ID="RadGrid1" runat="server" Visible="True"
                                    AllowPaging="False" AutoGenerateColumns="False" Skin="Bootstrap" AllowFilteringByColumn="true"
                                    OnNeedDataSource="RadGrid1_NeedDataSource" OnItemCommand="RadGrid1_ItemCommand"
                                    AllowSorting="True" OnFilterCheckListItemsRequested="RadGrid1_FilterCheckListItemsRequested"
                                    FilterType="HeaderContext" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true">
                                    <MasterTableView DataKeyNames="AccountCode,SPShortName" CommandItemDisplay="Top" CommandItemSettings-ShowAddNewRecordButton="false">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="S/N" HeaderButtonType="TextButton" HeaderStyle-Width="3%" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn SortExpression="AccountCode" FilterCheckListEnableLoadOnDemand="true" HeaderText="Account Code" HeaderButtonType="TextButton"
                                                DataField="AccountCode" UniqueName="AccountCode" GroupByExpression="AccountCode group by AccountCode">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="AccountName" FilterCheckListEnableLoadOnDemand="true" HeaderText="Account Name" HeaderButtonType="TextButton"
                                                DataField="AccountName" UniqueName="AccountName" GroupByExpression="AccountName group by AccountName">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="SPShortName" FilterCheckListEnableLoadOnDemand="true" HeaderText="Salesperson" HeaderButtonType="TextButton"
                                                DataField="SPShortName" UniqueName="SPShortName" GroupByExpression="SPShortName group by SPShortName">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="AccountClass" FilterCheckListEnableLoadOnDemand="true" HeaderText="Account Class" HeaderButtonType="TextButton"
                                                DataField="AccountClass" UniqueName="AccountClass" GroupByExpression="AccountClass group by AccountClass">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView4" runat="server" CssClass="p-1">
                                <telerik:RadGrid ID="RadGrid2" runat="server" Visible="True"
                                    AllowPaging="False" AutoGenerateColumns="False" Skin="Bootstrap" AllowFilteringByColumn="true"
                                    OnNeedDataSource="RadGrid2_NeedDataSource" OnItemCommand="RadGrid2_ItemCommand"
                                    AllowSorting="True" OnFilterCheckListItemsRequested="RadGrid2_FilterCheckListItemsRequested"
                                    FilterType="HeaderContext" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true">
                                    <MasterTableView DataKeyNames="AccountCode,SPShortName"  CommandItemDisplay="Top" CommandItemSettings-ShowAddNewRecordButton="false">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="S/N" HeaderButtonType="TextButton" HeaderStyle-Width="3%" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((RadGrid2.CurrentPageIndex) * RadGrid2.PageSize)  %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn SortExpression="AccountCode" FilterCheckListEnableLoadOnDemand="true" HeaderText="Account Code" HeaderButtonType="TextButton"
                                                DataField="AccountCode" UniqueName="AccountCode" GroupByExpression="AccountCode group by AccountCode">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="AccountName" FilterCheckListEnableLoadOnDemand="true" HeaderText="Account Name" HeaderButtonType="TextButton"
                                                DataField="AccountName" UniqueName="AccountName" GroupByExpression="AccountName group by AccountName">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="SPShortName" FilterCheckListEnableLoadOnDemand="true" HeaderText="Salesperson" HeaderButtonType="TextButton"
                                                DataField="SPShortName" UniqueName="SPShortName" GroupByExpression="SPShortName group by SPShortName">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="AccountClass" FilterCheckListEnableLoadOnDemand="true" HeaderText="Account Class" HeaderButtonType="TextButton"
                                                DataField="AccountClass" UniqueName="AccountClass" GroupByExpression="AccountClass group by AccountClass">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </telerik:RadPageView>
                        </telerik:RadMultiPage>
                    </asp:Panel>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <style>
        .RadSpreadsheet .rssSheetsbar a.t-spreadsheet-sheets-bar-add {
            display: none;
        }

        .rssPopup .rssCollapsibleList .rssDetails {
            height: 150px;
        }

        html body .RadMenu.RadMenu_Context.GridContextMenu {
            height: 500px !important;
            overflow: scroll !important;
        }
    </style>

    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">
            <asp:Label ID="lblPageHeader" runat="server" /></a></li>
        <li class="active">Budget Input</li>
    </ul>
    <h1 class="page-header">Budget Input<small> </small></h1>
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
                        <label class="control-label">Dim 1</label>
                        <asp:DropDownList runat="server" ID="ddlSearchDim1" CssClass="form-control input-sm" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlDim1_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Dim 2</label>
                        <asp:DropDownList runat="server" ID="ddlSearchDim2" CssClass="form-control input-sm" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlDim2_SelectedIndexChanged" Enabled="false">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6 ">
                        <label class="control-label">Dim 3</label>
                        <asp:DropDownList runat="server" ID="ddlSearchDim3" CssClass="form-control input-sm" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlDim3_SelectedIndexChanged" Enabled="false">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Dim 4</label>
                        <asp:DropDownList runat="server" ID="ddlSearchDim4" CssClass="form-control input-sm" Enabled="false"></asp:DropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Year</label>
                        <asp:DropDownList runat="server" ID="ddlYear" CssClass="form-control input-sm" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlYearMonth_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Customer Type</label>
                        <asp:DropDownList runat="server" ID="ddlCustomerType" CssClass="form-control input-sm">
                            <asp:ListItem Value="All" Text="All" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="External" Text="External"></asp:ListItem>
                            <asp:ListItem Value="Internal" Text="Interco"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Type</label>
                        <asp:DropDownList runat="server" ID="ddlType" CssClass="form-control input-sm">
                            <asp:ListItem Value="Amount" Text="Amount" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Quantity" Text="Quantity"></asp:ListItem>
                        </asp:DropDownList>
                        <input type="hidden" id="hidType" runat="server" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Class Type</label>
                        <asp:DropDownList runat="server" ID="ddlClassType" CssClass="form-control input-sm">
                            <asp:ListItem Value="ALL" Text="All"></asp:ListItem>
                            <asp:ListItem Value="Key & Mgd" Text="Key & Mgd" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Sales Person</label>
                        <asp:DropDownList runat="server" ID="ddlSalesperson" AutoPostBack="true" OnSelectedIndexChanged="ddlSalesperson_SelectedIndexChanged" CssClass="form-control input-sm">
                            <asp:ListItem Value="" Text="All" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6" runat="server" id="ddlAccField" visible="false">
                        <label class="control-label">Customer</label>
                        <asp:DropDownList runat="server" ID="ddlAccount" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel-footer clearfix">
        <asp:Label runat="server" ID="lblmessage"></asp:Label>
        <telerik:RadButton runat="server" ID="RadButton1" Text="Search" CssClass="pull-right btn btn-lg m-l-5 bg-primary largeFont" Skin="Bootstrap"
            OnClick="btnSearch_Click" SingleClick="true" SingleClickText="Retrieving Data...">
        </telerik:RadButton>
        <telerik:RadButton runat="server" ID="btnReset" Text="Reset" CssClass="pull-right btn btn-lg m-l-5 bg-danger largeFont" Skin="Bootstrap"
            OnClick="btnReset_Click" SingleClick="true" SingleClickText="Reseting...">
            <ConfirmSettings ConfirmText="Confirm to Reset? This action cannot be undone." />
        </telerik:RadButton>

        <asp:Button ID="btnStart" Text="Start Budgeting Exercise" EnableViewState="False" runat="server" CssClass="pull-right btn btn-primary m-l-5" OnClick="btnStart_Click" Visible="false"></asp:Button>

        <telerik:RadButton runat="server" ID="btnVariance" Text="Check Variance" CssClass="pull-right btn btn-lg m-l-5 largeFont" Skin="Bootstrap"
            OnClientClicked="OnClientClicked" OnClick="btnVariance_Click" AutoPostBack="true">
        </telerik:RadButton>
    </div>

    <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" Visible="true" OnClientTabSelected="OnClientTabSelected" SelectedIndex="0" runat="server" MultiPageID="RadMultiPage1" Skin="Bootstrap" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="B11A" PageViewID="PageView1" Selected="true">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="B12F" PageViewID="PageView2">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="B13B" PageViewID="PageView3">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="B14A" PageViewID="PageView4">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="B15F" PageViewID="PageView5">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="B16B" PageViewID="PageView6">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="100%">

        <telerik:RadPageView ID="PageView1" runat="server" CssClass="p-1">
            <div class="panel panel-primary" id="resultList" runat="server" visible="false">
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
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet1">
                        </telerik:RadSpreadsheet>
                    </div>
                </div>
            </div>
        </telerik:RadPageView>

        <telerik:RadPageView ID="PageView2" runat="server">
            <div class="panel panel-primary" id="resultList2" runat="server" visible="false">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                        <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-align-justify"></i>
                        <asp:Label ID="lblSearchSummary2" Visible="false" runat="server" />
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive" style="overflow: auto">
                        <asp:HiddenField runat="server" ID="hidRowChangedB12" ClientIDMode="Static" />
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                        <telerik:RadButton runat="server" ID="btnB2F" Text="Submit" CssClass="pull-right btn-lg m-l-5 bg-danger largeFont" Skin="Bootstrap"
                            OnClick="btnB2F_Click" OnClientClicked="OnClientClicked2" SingleClick="true" SingleClickText="Updating...">
                        </telerik:RadButton>
                        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet2" OnClientChange="OnClientChangeB12">
                        </telerik:RadSpreadsheet>
                    </div>
                </div>
            </div>
        </telerik:RadPageView>

        <telerik:RadPageView ID="PageView3" runat="server">
            <div class="panel panel-primary" id="resultList3" runat="server" visible="false">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                        <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-align-justify"></i>
                        <asp:Label ID="lblSearchSummary3" Visible="false" runat="server" />
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive" style="overflow: auto">
                        <asp:HiddenField runat="server" ID="hidRowChangedB13" ClientIDMode="Static" />
                        <asp:HiddenField ID="HiddenField3" runat="server" />
                        <telerik:RadButton runat="server" ID="btnB3B" Text="Submit" CssClass="pull-right btn-lg m-l-5 bg-danger largeFont" Skin="Bootstrap"
                            OnClick="btnB3B_Click" OnClientClicked="OnClientClicked3" SingleClick="true" SingleClickText="Updating..." />
                        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet3" OnClientChange="OnClientChangeB13">
                        </telerik:RadSpreadsheet>
                    </div>
                </div>
            </div>
        </telerik:RadPageView>

        <telerik:RadPageView ID="PageView4" runat="server" CssClass="p-1">
            <div class="panel panel-primary" id="resultList4" runat="server" visible="false">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                        <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-align-justify"></i>
                        <asp:Label ID="Label1" Visible="false" runat="server" />
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive" style="overflow: auto">
                        <asp:HiddenField ID="HiddenField4" runat="server" />
                        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet4">
                        </telerik:RadSpreadsheet>
                    </div>
                </div>
            </div>
        </telerik:RadPageView>

        <telerik:RadPageView ID="PageView5" runat="server">
            <div class="panel panel-primary" id="resultList5" runat="server" visible="false">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                        <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-align-justify"></i>
                        <asp:Label ID="Label2" Visible="false" runat="server" />
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive" style="overflow: auto">
                        <asp:HiddenField runat="server" ID="hidRowChangedB15" ClientIDMode="Static" />
                        <asp:HiddenField ID="HiddenField5" runat="server" />
                        <telerik:RadButton runat="server" ID="btnB5F" Text="Submit" CssClass="pull-right btn-lg m-l-5 bg-danger largeFont" Skin="Bootstrap"
                            OnClick="btnB5F_Click" OnClientClicked="OnClientClicked5" SingleClick="true" SingleClickText="Updating..." />
                        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet5" OnClientChange="OnClientChangeB15">
                        </telerik:RadSpreadsheet>
                    </div>
                </div>
            </div>
        </telerik:RadPageView>

        <telerik:RadPageView ID="PageView6" runat="server">
            <div class="panel panel-primary" id="resultList6" runat="server" visible="false">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                        <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-align-justify"></i>
                        <asp:Label ID="Label3" Visible="false" runat="server" />
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive" style="overflow: auto">
                        <asp:HiddenField runat="server" ID="hidRowChangedB16" ClientIDMode="Static" />
                        <asp:HiddenField ID="HiddenField6" runat="server" />
                        <telerik:RadButton runat="server" ID="btnB6B" Text="Submit" CssClass="pull-right btn-lg m-l-5 bg-danger largeFont" Skin="Bootstrap"
                            OnClick="btnB6B_Click" OnClientClicked="OnClientClicked6" SingleClick="true" SingleClickText="Updating..." />
                        <telerik:RadButton runat="server" ID="btnGPB16" Text="Update GP" CssClass="pull-right btn-lg m-l-5 bg-success largeFont" Skin="Bootstrap"
                            OnClick="btnGPB16_Click" OnClientClicked="OnClientClicked6" SingleClick="true" SingleClickText="GP Updating..." />
                        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet6" OnClientChange="OnClientChangeB16">
                        </telerik:RadSpreadsheet>
                    </div>
                </div>
            </div>
        </telerik:RadPageView>
    </telerik:RadMultiPage>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".budget-menu").addClass("active expand");
            $(".sub-budgetExercise").addClass("active");

            hideIDColumn();
        });

        function OnClientClicked(sender, args) {
            GetRadWindowManager().open(null, "RadWindow1");
        }

        function OnClientTabSelected(sender, args) {
            var $pageView = $telerik.$(args.get_tab().get_pageView().get_element())
            $pageView.find(".RadSpreadsheet").each(function (ind, elem) {
                elem.control.get_kendoWidget().refresh();
            })
        }
        //save on row changed event
        var changedRowsB12 = {};
        var changedRowsIndicesB12 = [];
        function OnClientChangeB12(sender, args) {
            args.get_range()._range._ref.forEachRowIndex(function (rowIndex) {
                changedRowsB12[rowIndex] = true;//save row index into list
            });
        }
        function getChangeRowsAsArrayB12(sender, args) {
            for (var r in changedRowsB12) {
                changedRowsIndicesB12.push(r);
            }
            $("#hidRowChangedB12").val(changedRowsIndicesB12);
            changedRowsIndicesB12 = [];
        }
        var changedRowsB13 = {};
        var changedRowsIndicesB13 = [];
        function OnClientChangeB13(sender, args) {
            args.get_range()._range._ref.forEachRowIndex(function (rowIndex) {
                changedRowsB13[rowIndex] = true;//save row index into list
            });
        }
        function getChangeRowsAsArrayB13(sender, args) {
            for (var r in changedRowsB13) {
                changedRowsIndicesB13.push(r);
            }
            $("#hidRowChangedB13").val(changedRowsIndicesB13);
            changedRowsIndicesB13 = [];
        }
        var changedRowsB15 = {};
        var changedRowsIndicesB15 = [];
        function OnClientChangeB15(sender, args) {
            args.get_range()._range._ref.forEachRowIndex(function (rowIndex) {
                changedRowsB15[rowIndex] = true;//save row index into list
            });
        }
        function getChangeRowsAsArrayB15(sender, args) {
            for (var r in changedRowsB15) {
                changedRowsIndicesB15.push(r);
            }
            $("#hidRowChangedB15").val(changedRowsIndicesB15);
            changedRowsIndicesB15 = [];
        }
        var changedRowsB16 = {};
        var changedRowsIndicesB16 = [];
        function OnClientChangeB16(sender, args) {
            args.get_range()._range._ref.forEachRowIndex(function (rowIndex) {
                changedRowsB16[rowIndex] = true;//save row index into list
            });
        }
        function getChangeRowsAsArrayB16(sender, args) {
            for (var r in changedRowsB16) {
                changedRowsIndicesB16.push(r);
            }
            $("#hidRowChangedB16").val(changedRowsIndicesB16);
            changedRowsIndicesB16 = [];
        }


        function pageLoadHandler() {
            try {
                var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
                var value = $get("<%= HiddenField1.ClientID %>").value;
                var valueAsJSON = JSON.parse(value);

                spreadsheet.get_kendoWidget().fromJSON(valueAsJSON);

                var spreadsheet2 = $find("<%= RadSpreadsheet2.ClientID %>");
                var value2 = $get("<%= HiddenField2.ClientID %>").value;
                var valueAsJSON2 = JSON.parse(value2);

                spreadsheet2.get_kendoWidget().fromJSON(valueAsJSON2);

                var spreadsheet3 = $find("<%= RadSpreadsheet3.ClientID %>");
                var value3 = $get("<%= HiddenField3.ClientID %>").value;
                var valueAsJSON3 = JSON.parse(value3);

                spreadsheet3.get_kendoWidget().fromJSON(valueAsJSON3);

                var spreadsheet4 = $find("<%= RadSpreadsheet4.ClientID %>");
                var value4 = $get("<%= HiddenField4.ClientID %>").value;
                var valueAsJSON4 = JSON.parse(value4);

                spreadsheet4.get_kendoWidget().fromJSON(valueAsJSON4);

                var spreadsheet5 = $find("<%= RadSpreadsheet5.ClientID %>");
                var value5 = $get("<%= HiddenField5.ClientID %>").value;
                var valueAsJSON5 = JSON.parse(value5);

                spreadsheet5.get_kendoWidget().fromJSON(valueAsJSON5);

                var spreadsheet6 = $find("<%= RadSpreadsheet6.ClientID %>");
                var value6 = $get("<%= HiddenField6.ClientID %>").value;
                var valueAsJSON6 = JSON.parse(value6);

                spreadsheet6.get_kendoWidget().fromJSON(valueAsJSON6);
            } catch (err) { }
        }

        Sys.Application.add_load(pageLoadHandler);//load spreadshhet data after postback //default spreadsheet data no reload after postback

        function OnClientClicked2(sender, args) {
            var spreadsheet = $find("<%= RadSpreadsheet2.ClientID %>");
            var jsonstring = JSON.stringify(spreadsheet.get_kendoWidget().toJSON());
            $get("<%= HiddenField2.ClientID %>").value = jsonstring;
            getChangeRowsAsArrayB12();
        }

        function OnClientClicked3(sender, args) {
            var spreadsheet = $find("<%= RadSpreadsheet3.ClientID %>");
            var jsonstring = JSON.stringify(spreadsheet.get_kendoWidget().toJSON());
            $get("<%= HiddenField3.ClientID %>").value = jsonstring;
            getChangeRowsAsArrayB13();
        }

        function OnClientClicked5(sender, args) {
            var spreadsheet = $find("<%= RadSpreadsheet5.ClientID %>");
            var jsonstring = JSON.stringify(spreadsheet.get_kendoWidget().toJSON());
            $get("<%= HiddenField5.ClientID %>").value = jsonstring;
            getChangeRowsAsArrayB15();
        }

        function OnClientClicked6(sender, args) {
            var spreadsheet = $find("<%= RadSpreadsheet6.ClientID %>");
            var jsonstring = JSON.stringify(spreadsheet.get_kendoWidget().toJSON());
            $get("<%= HiddenField6.ClientID %>").value = jsonstring;
            getChangeRowsAsArrayB16();
        }

        function hideIDColumn() {
            var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
            var activeSheet = spreadsheet.get_activeSheet();
            for (var i = 20; i <= 40; i++) {
                activeSheet.hideColumn(i);
            }

            var spreadsheet2 = $find("<%= RadSpreadsheet2.ClientID %>");
            var activeSheet2 = spreadsheet2.get_activeSheet();
            for (var i = 20; i <= 40; i++) {
                activeSheet2.hideColumn(i);
            }

            var spreadsheet3 = $find("<%= RadSpreadsheet3.ClientID %>");
            var activeSheet3 = spreadsheet3.get_activeSheet();
            for (var i = 21; i <= 40; i++) {
                activeSheet3.hideColumn(i);
            }

            var spreadsheet4 = $find("<%= RadSpreadsheet4.ClientID %>");
            var activeSheet4 = spreadsheet4.get_activeSheet();
            for (var i = 34; i <= 40; i++) {
                activeSheet4.hideColumn(i);
            }

            var spreadsheet5 = $find("<%= RadSpreadsheet5.ClientID %>");
            var activeSheet5 = spreadsheet5.get_activeSheet();
            for (var i = 34; i <= 40; i++) {
                activeSheet5.hideColumn(i);
            }

            var spreadsheet6 = $find("<%= RadSpreadsheet6.ClientID %>");
            var activeSheet6 = spreadsheet6.get_activeSheet();
            for (var i = 34; i <= 59; i++) {
                activeSheet6.hideColumn(i);
            }
        }
    </script>
</asp:Content>
