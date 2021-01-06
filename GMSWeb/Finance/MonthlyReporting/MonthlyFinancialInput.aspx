<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="MonthlyFinancialInput.aspx.cs" Inherits="GMSWeb.Finance.MonthlyReporting.MonthlyFinancialPerformance" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <!--prevent back to top in auto postback in radGrid-->
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid4">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid4" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">
            <asp:Label ID="lblPageHeader" runat="server" /></a></li>
        <li class="active">Monthly Reporting Input</li>
    </ul>
    <h1 class="page-header">Monthly Reporting Input<small></small></h1>
    <%--Search--%>
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
                        <asp:DropDownList runat="server" ID="ddlYear" CssClass="form-control input-sm" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlYearMonth_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Month</label>
                        <asp:DropDownList runat="server" ID="ddlMonth" CssClass="form-control input-sm" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlYearMonth_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
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
                </div>
            </div>
        </div>
    </div>
    <div class="panel-footer clearfix">
        <telerik:RadButton runat="server" ID="btnSearch" Text="Search" CssClass="pull-right btn btn-lg m-l-5 bg-primary largeFont" Skin="Bootstrap"
            OnClick="btnSearch_Click" SingleClick="true" SingleClickText="Retrieving Data...">
        </telerik:RadButton>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="MsgPanel2" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" SelectedIndex="0" runat="server" MultiPageID="RadMultiPage1" Skin="Bootstrap" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="By Month Analysis" PageViewID="PageView1">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="YTD By Month Analysis" PageViewID="PageView2">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Next 3 Months Forecast" PageViewID="PageView3">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Financial Year Forecast" PageViewID="PageView4">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="100%">
        <telerik:RadPageView ID="PageView1" runat="server" CssClass="p-1">
            <div class="panel panel-primary" id="resultList" runat="server">
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
                        <telerik:RadGrid ID="RadGrid1" runat="server" Visible="True"
                            AllowPaging="True" Skin="Bootstrap" AllowFilteringByColumn="true" AutoGenerateColumns="false"
                            OnNeedDataSource="RadGrid1_NeedDataSource" AllowSorting="True" OnItemCommand="RadGrid1_ItemCommand">
                            <MasterTableView CommandItemDisplay="None" DataKeyNames="ID" AllowFilteringByColumn="false"
                                CommandItemSettings-ShowAddNewRecordButton="false" CommandItemSettings-ShowRefreshButton="false">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="S/N" HeaderButtonType="TextButton" HeaderStyle-Width="3%" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn SortExpression="ItemName" FilterCheckListEnableLoadOnDemand="true" HeaderText="Item" HeaderButtonType="TextButton"
                                        DataField="ItemName" UniqueName="ItemName" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="ItemName group by ItemName">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn SortExpression="Actual" FilterCheckListEnableLoadOnDemand="true" HeaderText="Actual" HeaderButtonType="TextButton"
                                        DataField="Actual" UniqueName="Actual" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="Actual group by Actual">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn SortExpression="Budget" FilterCheckListEnableLoadOnDemand="true" HeaderText="Budget" HeaderButtonType="TextButton"
                                        DataField="Budget" UniqueName="Budget" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="Budget group by Budget">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn SortExpression="Variance" FilterCheckListEnableLoadOnDemand="true" HeaderText="Variance" HeaderButtonType="TextButton"
                                        DataField="Variance" UniqueName="Variance" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="Variance group by Variance">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn SortExpression="Reason" FilterCheckListEnableLoadOnDemand="true" HeaderText="Shortfall/Surplus Reason" HeaderButtonType="TextButton"
                                        DataField="Reason" UniqueName="Reason" HeaderStyle-Width="50%" FilterControlWidth="70%" GroupByExpression="Reason group by Reason">
                                        <ItemTemplate>
                                            <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtReason" runat="server" Text='<%#Eval( "Reason")%>' TextMode="MultiLine" EmptyMessage="type here">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn CommandName="Save" Text="Save" UniqueName="SaveColumn" Visible="true">
                                    </telerik:GridButtonColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
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
                        <telerik:RadGrid ID="RadGrid2" runat="server" Visible="True"
                            AllowPaging="True" Skin="Bootstrap" AllowFilteringByColumn="true" AutoGenerateColumns="false"
                            OnNeedDataSource="RadGrid1_NeedDataSource" AllowSorting="True" OnItemCommand="RadGrid1_ItemCommand">
                            <MasterTableView CommandItemDisplay="None" DataKeyNames="ID" AllowFilteringByColumn="false"
                                CommandItemSettings-ShowAddNewRecordButton="false" CommandItemSettings-ShowRefreshButton="false">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="S/N" HeaderButtonType="TextButton" HeaderStyle-Width="3%" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn SortExpression="ItemName" FilterCheckListEnableLoadOnDemand="true" HeaderText="Item" HeaderButtonType="TextButton"
                                        DataField="ItemName" UniqueName="ItemName" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="ItemName group by ItemName">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn SortExpression="Actual" FilterCheckListEnableLoadOnDemand="true" HeaderText="Actual" HeaderButtonType="TextButton"
                                        DataField="Actual" UniqueName="Actual" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="Actual group by Actual">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn SortExpression="Budget" FilterCheckListEnableLoadOnDemand="true" HeaderText="Budget" HeaderButtonType="TextButton"
                                        DataField="Budget" UniqueName="Budget" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="Budget group by Budget">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn SortExpression="Variance" FilterCheckListEnableLoadOnDemand="true" HeaderText="Variance" HeaderButtonType="TextButton"
                                        DataField="Variance" UniqueName="Variance" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="Variance group by Variance">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn SortExpression="Reason" FilterCheckListEnableLoadOnDemand="true" HeaderText="Comment/Recovery Plan" HeaderButtonType="TextButton"
                                        DataField="Reason" UniqueName="Reason" HeaderStyle-Width="50%" FilterControlWidth="70%" GroupByExpression="Reason group by Reason">
                                        <ItemTemplate>
                                            <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtReason" runat="server" Text='<%#Eval( "Reason")%>' TextMode="MultiLine" EmptyMessage="type here">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn CommandName="Save" Text="Save" UniqueName="SaveColumn" Visible="true">
                                    </telerik:GridButtonColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView3" runat="server" CssClass="p-1">
            <div class="panel panel-primary" id="Div2" runat="server">
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
                        <telerik:RadGrid ID="RadGrid3" runat="server" Visible="True"
                            AllowPaging="True" Skin="Bootstrap" AllowFilteringByColumn="true" AutoGenerateColumns="false" OnItemDataBound="RadGrid_ItemDataBound"
                            OnNeedDataSource="RadGrid1_NeedDataSource" AllowSorting="True" OnItemCommand="RadGridForecast_ItemCommand">
                            <MasterTableView CommandItemDisplay="None" DataKeyNames="ID" AllowFilteringByColumn="false"
                                CommandItemSettings-ShowAddNewRecordButton="false" CommandItemSettings-ShowRefreshButton="false">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="S/N" HeaderButtonType="TextButton" HeaderStyle-Width="3%" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
       
                                    <telerik:GridBoundColumn SortExpression="ItemName" FilterCheckListEnableLoadOnDemand="true" HeaderText="Item" HeaderButtonType="TextButton"
                                        DataField="ItemName" UniqueName="ItemName" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="ItemName group by ItemName">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn SortExpression="Actual" FilterCheckListEnableLoadOnDemand="true" HeaderText="Forecast" HeaderButtonType="TextButton"
                                        DataField="Actual" UniqueName="Actual" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="Actual group by Actual">
                                        <ItemTemplate>
                                            <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtActual" runat="server" Text='<%#Eval( "Actual")%>'
                                                TextMode="SingleLine" EmptyMessage="type here" InputType="Number">
                                            </telerik:RadTextBox>
                                            <input type="hidden" id="hidItemName" runat="server" Value='<%#Eval( "ItemName")%>'/>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn SortExpression="Budget" FilterCheckListEnableLoadOnDemand="true" HeaderText="Budget" HeaderButtonType="TextButton"
                                        DataField="Budget" UniqueName="Budget" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="Budget group by Budget">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn SortExpression="Variance" FilterCheckListEnableLoadOnDemand="true" HeaderText="Variance" HeaderButtonType="TextButton"
                                        DataField="Variance" UniqueName="Variance" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="Variance group by Variance">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn SortExpression="Reason" FilterCheckListEnableLoadOnDemand="true" HeaderText="Comment" HeaderButtonType="TextButton"
                                        DataField="Reason" UniqueName="Reason" HeaderStyle-Width="100%" FilterControlWidth="70%" GroupByExpression="Reason group by Reason">
                                        <ItemTemplate>
                                            <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtReason" runat="server" Text='<%#Eval( "Reason")%>' TextMode="MultiLine" EmptyMessage="type here">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn CommandName="Save" Text="Save" UniqueName="SaveColumn" Visible="true">
                                    </telerik:GridButtonColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView4" runat="server">
            <div class="panel panel-primary" id="Div3" runat="server">
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
                        <telerik:RadGrid ID="RadGrid4" runat="server" Visible="True"
                            AllowPaging="True" Skin="Bootstrap" AllowFilteringByColumn="true" AutoGenerateColumns="false"
                            OnNeedDataSource="RadGrid1_NeedDataSource" AllowSorting="True" OnItemCommand="RadGridForecast_ItemCommand">
                            <MasterTableView CommandItemDisplay="None" DataKeyNames="ID" AllowFilteringByColumn="false"
                                CommandItemSettings-ShowAddNewRecordButton="false" CommandItemSettings-ShowRefreshButton="false">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="S/N" HeaderButtonType="TextButton" HeaderStyle-Width="3%" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn SortExpression="ItemName" FilterCheckListEnableLoadOnDemand="true" HeaderText="Item" HeaderButtonType="TextButton"
                                        DataField="ItemName" UniqueName="ItemName" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="ItemName group by ItemName">
                                    </telerik:GridBoundColumn>
                                     <telerik:GridTemplateColumn SortExpression="Actual" FilterCheckListEnableLoadOnDemand="true" HeaderText="Actual" HeaderButtonType="TextButton"
                                        DataField="Actual" UniqueName="Actual" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="Actual group by Actual">
                                        <ItemTemplate>
                                            <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtActual" runat="server" Text='<%#Eval( "Actual")%>'
                                                TextMode="SingleLine" EmptyMessage="type here" InputType="Number">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn SortExpression="Budget" FilterCheckListEnableLoadOnDemand="true" HeaderText="Budget" HeaderButtonType="TextButton"
                                        DataField="Budget" UniqueName="Budget" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="Budget group by Budget">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn SortExpression="Variance" FilterCheckListEnableLoadOnDemand="true" HeaderText="Variance" HeaderButtonType="TextButton"
                                        DataField="Variance" UniqueName="Variance" HeaderStyle-Width="12%" FilterControlWidth="70%" GroupByExpression="Variance group by Variance">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn SortExpression="Reason" FilterCheckListEnableLoadOnDemand="true" HeaderText="Comment" HeaderButtonType="TextButton"
                                        DataField="Reason" UniqueName="Reason" HeaderStyle-Width="50%" FilterControlWidth="70%" GroupByExpression="Reason group by Reason">
                                        <ItemTemplate>
                                            <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtReason" runat="server" Text='<%#Eval( "Reason")%>' TextMode="MultiLine" EmptyMessage="type here">
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn CommandName="Save" Text="Save" UniqueName="SaveColumn" Visible="true">
                                    </telerik:GridButtonColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>
            </div>
        </telerik:RadPageView>
    </telerik:RadMultiPage>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".monthlyReport-menu").addClass("active expand");
            $(".sub-MonthlyReport-input").addClass("active");
        });

    </script>
</asp:Content>
