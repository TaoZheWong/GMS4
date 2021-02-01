<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="HRBudget.aspx.cs" Inherits="GMSWeb.Budget.HRBudget" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <%--  <telerik:AjaxSetting AjaxControlID="Contentpanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Contentpanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnVariance">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>

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

        .rddlPopup .rddlItem.A {
            color: black;
            background: #bdd7ee;
        }

        .rddlPopup .rddlItem.B {
            color: black;
            background: #ffcc66;
        }

        .rddlPopup .rddlItem.C {
            color: black;
            background: #ffccff;
        }

        .rddlPopup .rddlItem.D {
            color: black;
            background: #c6e0b4;
        }

        .rddlPopup .rddlItem.E {
            color: black;
            background: #ffff00;
        }
    </style>

    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">
            <asp:Label ID="lblPageHeader" runat="server" /></a></li>
        <li class="active">HR Budget</li>
    </ul>
    <h1 class="page-header">HR Budget<small> </small></h1>
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
                        <label class="control-label">Year</label>
                        <telerik:RadDropDownList runat="server" ID="ddlYear" AutoPostBack="true" CssClass="btn-group-lg largeFont"
                            OnSelectedIndexChanged="ddlYearMonth_SelectedIndexChanged" RenderMode="Lightweight" Skin="Bootstrap"
                            Width="100%">
                        </telerik:RadDropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Dim 1</label>
                        <telerik:RadDropDownList runat="server" ID="ddlSearchDim1" AutoPostBack="true" CssClass="btn-group-lg largeFont"
                            OnSelectedIndexChanged="ddlDim1_SelectedIndexChanged" RenderMode="Lightweight" Skin="Bootstrap"
                            Width="100%">
                        </telerik:RadDropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Dim 2</label>
                        <telerik:RadDropDownList runat="server" ID="ddlSearchDim2" AutoPostBack="true" CssClass="btn-group-lg largeFont"
                            OnSelectedIndexChanged="ddlDim2_SelectedIndexChanged" Enabled="false" RenderMode="Lightweight" Skin="Bootstrap"
                            Width="100%">
                        </telerik:RadDropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6 ">
                        <label class="control-label">Dim 3</label>
                        <telerik:RadDropDownList runat="server" ID="ddlSearchDim3" AutoPostBack="true" CssClass="btn-group-lg largeFont"
                            OnSelectedIndexChanged="ddlDim3_SelectedIndexChanged" Enabled="false" RenderMode="Lightweight" Skin="Bootstrap"
                            Width="100%">
                        </telerik:RadDropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Dim 4</label>
                        <telerik:RadDropDownList runat="server" ID="ddlSearchDim4" Enabled="false" CssClass="btn-group-lg largeFont"
                            RenderMode="Lightweight" Skin="Bootstrap" Width="100%">
                        </telerik:RadDropDownList>
                    </div>

                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Type</label>
                        <telerik:RadDropDownList runat="server" ID="ddlType" CssClass="btn-group-lg largeFont"
                            RenderMode="Lightweight" Skin="Bootstrap" Width="100%">
                            <Items>
                                <telerik:DropDownListItem Text="Direct Labour" Value="A" />
                                <telerik:DropDownListItem Text="Indirect Labour" Value="B" />
                                <telerik:DropDownListItem Text="Fixed Overhead" Value="C" />
                                <telerik:DropDownListItem Text="Selling & Distribution" Value="D" />
                                <telerik:DropDownListItem Text="Admin & General Expenses" Value="E" />
                            </Items>
                        </telerik:RadDropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel-footer clearfix">
        <asp:Label runat="server" ID="lblmessage"></asp:Label>
        <telerik:RadButton runat="server" ID="RadButton1" Text="Search" CssClass="pull-right btn btn-lg m-l-5 bg-primary largeFont" Skin="Bootstrap"
            OnClick="btnSearch_Click" SingleClick="true" SingleClickText="Retrieving Data..." Width="150px">
        </telerik:RadButton>
        <telerik:RadButton runat="server" ID="btnDelete" Text="Reset" CssClass="pull-right btn btn-lg m-l-5 largeFont" Skin="Bootstrap"
            OnClick="btnDelete_Click" SingleClick="true" SingleClickText="Deleting..." Width="150px">
            <ConfirmSettings ConfirmText="Caution: This action will delete the selected year data!!!
Click 'Ok' to Proceed." />
        </telerik:RadButton>
        <asp:HiddenField runat="server" ID="hidYear" />
        <asp:HiddenField runat="server" ID="hidBudgetYear" />
        <asp:HiddenField runat="server" ID="hidType" />
    </div>

    <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" Visible="false" OnClientTabSelected="OnClientTabSelected" SelectedIndex="0" runat="server" MultiPageID="RadMultiPage1" Skin="Bootstrap" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="B52A" PageViewID="PageView4">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="B52F" PageViewID="PageView2">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="B52B" PageViewID="PageView3">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Data Processing" PageViewID="PageView1" Visible="false">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="100%" Visible="false">
        <telerik:RadPageView ID="PageView4" runat="server" CssClass="p-1">
            <div class="panel panel-primary" id="Div2" runat="server">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                        <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-align-justify"></i>
                        <asp:Label ID="Label2" runat="server" Text="" />
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive" style="overflow: auto">
                        <asp:HiddenField ID="HiddenField4" runat="server" />
                        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet4" RenderMode="Lightweight" Skin="Bootstrap">
                            <Toolbar>
                                <telerik:SpreadsheetToolbarTab Text="Tool">
                                    <telerik:SpreadsheetToolbarGroup>
                                        <telerik:SpreadsheetTool Name="ExportAs" />
                                        <telerik:SpreadsheetTool Name="FontSize" />
                                        <telerik:SpreadsheetTool Name="Undo" />
                                        <telerik:SpreadsheetTool Name="Redo" />
                                    </telerik:SpreadsheetToolbarGroup>
                                </telerik:SpreadsheetToolbarTab>
                            </Toolbar>
                        </telerik:RadSpreadsheet>
                    </div>
                </div>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="PageView2" runat="server">
            <div class="panel panel-primary" id="Div1" runat="server">
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
                        <asp:HiddenField runat="server" ID="hidRowChangedB52F" ClientIDMode="Static" />
                        <asp:HiddenField ID="HiddenField3" runat="server" />
                        <telerik:RadButton runat="server" ID="btnSubmitB52F" Text="Submit" CssClass="pull-right btn-lg m-l-5 bg-danger largeFont" Skin="Bootstrap"
                            OnClick="btnSubmitB52F_Click" OnClientClicked="OnClientClicked3" SingleClick="true" SingleClickText="Updating..." Width="150px">
                        </telerik:RadButton>
                        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet3" OnClientChange="OnClientChangeB52F">
                            <Toolbar>
                                <telerik:SpreadsheetToolbarTab Text="Tool">
                                    <telerik:SpreadsheetToolbarGroup>
                                        <telerik:SpreadsheetTool Name="ExportAs" />
                                        <telerik:SpreadsheetTool Name="FontSize" />
                                        <telerik:SpreadsheetTool Name="Undo" />
                                        <telerik:SpreadsheetTool Name="Redo" />
                                    </telerik:SpreadsheetToolbarGroup>
                                </telerik:SpreadsheetToolbarTab>
                            </Toolbar>
                        </telerik:RadSpreadsheet>
                    </div>
                </div>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="PageView3" runat="server">
            <div class="panel panel-primary" id="resultList2" runat="server">
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
                        <asp:HiddenField runat="server" ID="hidRowChangedB52B" ClientIDMode="Static" />
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                        <telerik:RadButton runat="server" ID="btnSubmitB52B" Text="Submit" CssClass="pull-right btn-lg m-l-5 bg-danger largeFont" Skin="Bootstrap"
                            OnClick="btnSubmitB52B_Click" OnClientClicked="OnClientClicked2" SingleClick="true" SingleClickText="Updating..." Width="150px">
                        </telerik:RadButton>
                        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet2" OnClientChange="OnClientChangeB52B">
                            <Toolbar>
                                <telerik:SpreadsheetToolbarTab Text="Tool">
                                    <telerik:SpreadsheetToolbarGroup>
                                        <telerik:SpreadsheetTool Name="ExportAs" />
                                        <telerik:SpreadsheetTool Name="FontSize" />
                                        <telerik:SpreadsheetTool Name="Undo" />
                                        <telerik:SpreadsheetTool Name="Redo" />
                                    </telerik:SpreadsheetToolbarGroup>
                                </telerik:SpreadsheetToolbarTab>
                            </Toolbar>
                        </telerik:RadSpreadsheet>
                    </div>
                </div>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="PageView1" runat="server" CssClass="p-1">
            <div class="panel panel-primary" id="resultList" runat="server">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                        <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-align-justify"></i>
                        <asp:Label ID="lblSearchSummary" runat="server" Text="" />
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive" style="overflow: auto">
                        <asp:HiddenField ID="HiddenField1" runat="server" />

                        <telerik:RadButton runat="server" ID="btnSubmit" Text="Submit" CssClass="pull-right btn-lg m-l-5 bg-danger largeFont" Width="17%" Skin="Bootstrap"
                            OnClick="btnSubmitBudget_Click" OnClientClicked="OnClientClicked1" SingleClick="true" SingleClickText="Submitting..." RenderMode="Lightweight" Height="5%">
                            <ConfirmSettings ConfirmText="Confirm to submit? Proceed to overwrite." />
                        </telerik:RadButton>
                        <telerik:RadButton runat="server" ID="btnSubmitForecast" Text="Submit" CssClass="pull-right btn-lg m-l-5 bg largeFont" Width="17%" Skin="Bootstrap" Visible="false"
                            OnClick="btnSubmitForecast_Click" OnClientClicked="OnClientClicked1" SingleClick="true" SingleClickText="Submitting..." RenderMode="Lightweight" Height="5%">
                            <ConfirmSettings ConfirmText="Confirm to submit? Proceed to overwrite." />
                        </telerik:RadButton>

                        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet1" RenderMode="Lightweight" Skin="Bootstrap">
                            <Toolbar>
                                <telerik:SpreadsheetToolbarTab Text="Tool">
                                    <telerik:SpreadsheetToolbarGroup>
                                        <telerik:SpreadsheetTool Name="ExportAs" />
                                        <telerik:SpreadsheetTool Name="FontSize" />
                                        <telerik:SpreadsheetTool Name="Undo" />
                                        <telerik:SpreadsheetTool Name="Redo" />
                                    </telerik:SpreadsheetToolbarGroup>
                                </telerik:SpreadsheetToolbarTab>
                            </Toolbar>
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
            $(".hrbudget-menu").addClass("active");

            hideReferenceColumn();
        });

        function hideReferenceColumn() {
            var spreadsheet = $find("<%= RadSpreadsheet2.ClientID %>");
            var activeSheet = spreadsheet.get_activeSheet();
            activeSheet.hideColumn(18);
            activeSheet.hideColumn(19);
            activeSheet.hideColumn(20);

            var spreadsheet2 = $find("<%= RadSpreadsheet3.ClientID %>");
            var activeSheet2 = spreadsheet2.get_activeSheet();
            activeSheet2.hideColumn(18);
            activeSheet2.hideColumn(19);
            activeSheet2.hideColumn(20);

            var spreadsheet3 = $find("<%= RadSpreadsheet4.ClientID %>");
            var activeSheet3 = spreadsheet3.get_activeSheet();
            activeSheet3.hideColumn(18);
            activeSheet3.hideColumn(19);
            activeSheet3.hideColumn(20);
        }

        function OnClientTabSelected(sender, args) {
            var $pageView = $telerik.$(args.get_tab().get_pageView().get_element())
            $pageView.find(".RadSpreadsheet").each(function (ind, elem) {
                elem.control.get_kendoWidget().refresh();
            })
        }
        //save on row changed event
        var changedRowsB52B = {};
        var changedRowsIndicesB52B = [];
        function OnClientChangeB52B(sender, args) {
            args.get_range()._range._ref.forEachRowIndex(function (rowIndex) {
                changedRowsB52B[rowIndex] = true;//save row index into list
            });
        }
        function getChangeRowsAsArrayB52B(sender, args) {
            for (var r in changedRowsB52B) {
                changedRowsIndicesB52B.push(r);
            }
            $("#hidRowChangedB52B").val(changedRowsIndicesB52B);
            changedRowsIndicesB52B = [];
        }

        var changedRowsB52F = {};
        var changedRowsIndicesB52F = [];
        function OnClientChangeB52F(sender, args) {
            args.get_range()._range._ref.forEachRowIndex(function (rowIndex) {
                changedRowsB52F[rowIndex] = true;//save row index into list
            });
        }
        function getChangeRowsAsArrayB52F(sender, args) {
            for (var r in changedRowsB52F) {
                changedRowsIndicesB52F.push(r);
            }
            $("#hidRowChangedB52F").val(changedRowsIndicesB52F);
            changedRowsIndicesB52F = [];
        }

        function pageLoadHandler() {
            try {
                var spreadsheet1 = $find("<%= RadSpreadsheet1.ClientID %>");
                var value1 = $get("<%= HiddenField1.ClientID %>").value;
                var valueAsJSON1 = JSON.parse(value1);
                spreadsheet1.get_kendoWidget().fromJSON(valueAsJSON1);
            } catch (err) { }

            try {
                var spreadsheet2 = $find("<%= RadSpreadsheet2.ClientID %>");
                var value2 = $get("<%= HiddenField2.ClientID %>").value;
                var valueAsJSON2 = JSON.parse(value2);
                spreadsheet2.get_kendoWidget().fromJSON(valueAsJSON2);
            } catch (err) { }

            try {
                var spreadsheet3 = $find("<%= RadSpreadsheet3.ClientID %>");
                var value3 = $get("<%= HiddenField3.ClientID %>").value;
                var valueAsJSON3 = JSON.parse(value3);
                spreadsheet3.get_kendoWidget().fromJSON(valueAsJSON3);
            } catch (err) { }
        }

        Sys.Application.add_load(pageLoadHandler);//load spreadsheet data after postback //default spreadsheet data will not reload after postback

        function OnClientClicked1(sender, args) {
            var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
             var jsonstring = JSON.stringify(spreadsheet.get_kendoWidget().toJSON());
             $get("<%= HiddenField1.ClientID %>").value = jsonstring;
          }

          function OnClientClicked2(sender, args) {
              var spreadsheet = $find("<%= RadSpreadsheet2.ClientID %>");
            var jsonstring = JSON.stringify(spreadsheet.get_kendoWidget().toJSON());
            $get("<%= HiddenField2.ClientID %>").value = jsonstring;
            getChangeRowsAsArrayB52B();
        }

        function OnClientClicked3(sender, args) {
            var spreadsheet = $find("<%= RadSpreadsheet3.ClientID %>");
             var jsonstring = JSON.stringify(spreadsheet.get_kendoWidget().toJSON());
             $get("<%= HiddenField3.ClientID %>").value = jsonstring;
            getChangeRowsAsArrayB52F();
        }
    </script>
</asp:Content>
