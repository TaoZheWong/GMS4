<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Budget.aspx.cs" Inherits="GMSWeb.Budget.Budget" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <style>
        .RadSpreadsheet .rssSheetsbar a.t-spreadsheet-sheets-bar-add {
            display: none;
        }

        .rssPopup .rssCollapsibleList .rssDetails {
            height: 150px;
        }
    </style>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <%--<telerik:AjaxSetting AjaxControlID="RadMultiPage1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">
            <asp:Label ID="lblPageHeader" runat="server" /></a></li>
        <li class="active">Budget Sales Exercise</li>
    </ul>
    <h1 class="page-header">Budget Sales Exercise<small> </small></h1>
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
                            <asp:ListItem Value="Interco" Text="Interco"></asp:ListItem>
                        </asp:DropDownList>
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
                        <asp:DropDownList runat="server" ID="ddlSalesperson" CssClass="form-control input-sm">
                            <asp:ListItem Value="" Text="All" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel-footer clearfix">
        <asp:Button ID="btnStart" Text="Start Budgeting Exercise" EnableViewState="False" runat="server" CssClass="pull-right btn btn-primary m-l-5" OnClick="btnStart_Click"></asp:Button>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="MsgPanel2" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" Visible="true" OnClientTabSelected="OnClientTabSelected" SelectedIndex="0" runat="server" MultiPageID="RadMultiPage1" Skin="Bootstrap" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="B11A" PageViewID="PageView1">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="B11F" PageViewID="PageView2">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="B11B" PageViewID="PageView3">
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
                <div class="TABCOMMAND">
                    <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                        <telerik:RadButton runat="server" ID="RadButton2" Text="Submit" CssClass="pull-right btn m-l-5" Skin="Bootstrap"
                            OnClick="RadButton2_Click" OnClientClicked="OnClientClicked2" />
                        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet2" OnClientChange="OnClientChange">
                        </telerik:RadSpreadsheet>
                    </div>
                </div>
                <div class="TABCOMMAND">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <uctrl:MsgPanel ID="MsgPanel1" runat="server" EnableViewState="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
                        <asp:HiddenField ID="HiddenField3" runat="server" />
                        <telerik:RadButton runat="server" ID="RadButton3" Text="Submit" CssClass="pull-right btn m-l-5" Skin="Bootstrap"
                            OnClick="RadButton3_Click" OnClientClicked="OnClientClicked3" />
                        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet3" OnClientChange="OnClientChange2">
                            <ContextMenus>
                                <CellContextMenu OnClientItemClicked="CellContextMenuItemClicked">
                                    <Items>
                                        <telerik:RadMenuItem Text="Copy" Value="CommandCopy"></telerik:RadMenuItem>
                                        <telerik:RadMenuItem Text="Paste" Value="CommandPaste"></telerik:RadMenuItem>
                                        <telerik:RadMenuItem Text="HideRow" Value="CommandHideRow"></telerik:RadMenuItem>
                                        <telerik:RadMenuItem Text="HideColumn" Value="CommandHideColumn"></telerik:RadMenuItem>
                                        <telerik:RadMenuItem Text="Bold" Value="CustomSetBold"></telerik:RadMenuItem>
                                    </Items>
                                </CellContextMenu>
                                <RowHeaderContextMenu>
                                    <Items>
                                        <telerik:RadMenuItem Text="HideRow" Value="CommandHideRow"></telerik:RadMenuItem>
                                        <telerik:RadMenuItem Text="DeleteRow" Value="CommandDeleteRow"></telerik:RadMenuItem>
                                    </Items>
                                </RowHeaderContextMenu>
                                <ColumnHeaderContextMenu>
                                    <Items>
                                        <telerik:RadMenuItem Text="HideColumn" Value="CommandHideColumn"></telerik:RadMenuItem>
                                        <telerik:RadMenuItem Text="DeleteColumn" Value="CommandDeleteColumn"></telerik:RadMenuItem>
                                    </Items>
                                </ColumnHeaderContextMenu>
                            </ContextMenus>
                        </telerik:RadSpreadsheet>
                    </div>
                </div>
                <div class="TABCOMMAND">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <uctrl:MsgPanel ID="MsgPanel3" runat="server" EnableViewState="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </telerik:RadPageView>
    </telerik:RadMultiPage>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".budget-menu").addClass("active expand");
            $(".sub-budgetSales").addClass("active");

            hideIDColumn();
        });

        function OnClientTabSelected(sender, args) {
            var $pageView = $telerik.$(args.get_tab().get_pageView().get_element())
            $pageView.find(".RadSpreadsheet").each(function (ind, elem) {
                elem.control.get_kendoWidget().refresh();
            })
        }

        function pageLoadHandler() {
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
        }

        Sys.Application.add_load(pageLoadHandler);//load spreadshhet data after postback //default spreadsheet data no reload after postback

        function OnClientClicked(sender, args) {
            var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
            var jsonstring = JSON.stringify(spreadsheet.get_kendoWidget().toJSON());
            $get("<%= HiddenField1.ClientID %>").value = jsonstring;
           }

           function OnClientClicked2(sender, args) {

               var spreadsheet = $find("<%= RadSpreadsheet2.ClientID %>");
            var jsonstring = JSON.stringify(spreadsheet.get_kendoWidget().toJSON());
            $get("<%= HiddenField2.ClientID %>").value = jsonstring;
        }

        function OnClientClicked3(sender, args) {

            var spreadsheet = $find("<%= RadSpreadsheet3.ClientID %>");
            var jsonstring = JSON.stringify(spreadsheet.get_kendoWidget().toJSON());
            $get("<%= HiddenField3.ClientID %>").value = jsonstring;

        }

        function OnClientChange(sender, eventArgs) {
            eventArgs.get_range().set_color("#000000"); //returns the new color of text in the cell
        }

        function OnClientChange2(sender, eventArgs) {
            eventArgs.get_range().set_color("#000000"); //returns the new color of text in the cell
        }

        function CellContextMenuItemClicked(sender, args) {
            if (args.get_item().get_value() == "CustomSetBold") {
                var range = $find("RadSpreadsheet3").get_activeSheet().get_selection();
                range.set_bold(true);
            }
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
        }
    </script>
</asp:Content>
