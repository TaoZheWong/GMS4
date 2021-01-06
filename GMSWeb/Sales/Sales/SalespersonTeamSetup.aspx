<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="SalespersonTeamSetup.aspx.cs" Inherits="GMSWeb.Sales.Sales.SalespersonTeamSetup" Title="Sales Person Team Setup" %>

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
        <li class="active">Sales Exec Team Set Up</li>
    </ul>
    <h1 class="page-header">Sales Exec Team Set Up<small> Setup of sales exec & team.</small></h1>

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

                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="SalespersonID"
                        CommandItemSettings-ShowAddNewRecordButton="false" CommandItemSettings-ShowRefreshButton="true">
                        <BatchEditingSettings EditType="Row" />

                        <Columns>
                            <telerik:GridEditCommandColumn UniqueName="EditCommandColumn1">
                                <HeaderStyle Width="10px"></HeaderStyle>
                                <ItemStyle CssClass="MyImageButton"></ItemStyle>
                            </telerik:GridEditCommandColumn>
                            <telerik:GridTemplateColumn SortExpression="DetailItemNo" AllowFiltering="false" HeaderText="S/N" HeaderButtonType="TextButton" HeaderStyle-Width="3%"
                                DataField="DetailItemNo">
                                <ItemTemplate>
                                    <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn SortExpression="d1" FilterCheckListEnableLoadOnDemand="true" AllowFiltering="true" HeaderText="D1" HeaderStyle-Width="10%" FilterControlWidth="70%"
                                HeaderButtonType="TextButton" DataField="d1" UniqueName="Dim1" GroupByExpression="d1 group by d1">
                                <ItemTemplate>
                                    <%# Eval("d1") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlDim1" AutoPostBack="true" CssClass="form-control input-sm"
                                        OnSelectedIndexChanged="ddlProjectID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <input type="hidden" id="hidDim1" runat="server" value='<%# Eval("Dim1")%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn SortExpression="d2" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true" HeaderText="D2" HeaderStyle-Width="10%" FilterControlWidth="70%"
                                HeaderButtonType="TextButton" DataField="d2" UniqueName="Dim2" GroupByExpression="d2 group by d2">
                                <ItemTemplate>
                                    <%# Eval("d2") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlDim2" AutoPostBack="true" CssClass="form-control input-sm"
                                        OnSelectedIndexChanged="ddlDepartmentID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <input type="hidden" id="hidDim2" runat="server" value='<%# Eval("Dim2")%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn SortExpression="d3" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true" HeaderText="D3" HeaderStyle-Width="10%" FilterControlWidth="70%"
                                HeaderButtonType="TextButton" DataField="d3" UniqueName="Dim3" GroupByExpression="d3 group by d3">
                                <ItemTemplate>
                                    <%# Eval("d3") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlDim3" AutoPostBack="true" CssClass="form-control input-sm"
                                        OnSelectedIndexChanged="ddlSectionID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <input type="hidden" id="hidDim3" runat="server" value='<%# Eval("Dim3")%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn SortExpression="d4" AllowFiltering="true" FilterCheckListEnableLoadOnDemand="true" HeaderText="D4" HeaderStyle-Width="10%" FilterControlWidth="70%"
                                HeaderButtonType="TextButton" DataField="d4" UniqueName="Dim4" GroupByExpression="d4 group by d4">
                                <ItemTemplate>
                                    <%# Eval("d4") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlDim4" CssClass="form-control input-sm"></asp:DropDownList>
                                    <input type="hidden" id="hidDim4" runat="server" value='<%# Eval("Dim4")%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn SortExpression="SalespersonID" FilterCheckListEnableLoadOnDemand="true"  AllowFiltering="true" HeaderText="Code"
                                HeaderButtonType="TextButton" DataField="SalespersonID" UniqueName="SalespersonID" GroupByExpression="SalespersonID group by SalespersonID">
                                <ItemTemplate>
                                    <%# Eval("SalespersonID") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Eval("SalespersonID") %>' Enabled="false"></asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn SortExpression="SalespersonName" FilterCheckListEnableLoadOnDemand="true"  AllowFiltering="true" HeaderText="Name"
                                HeaderButtonType="TextButton" DataField="SalespersonName" UniqueName="SalespersonName" GroupByExpression="SalespersonName group by SalespersonName">
                                <ItemTemplate>
                                    <%# Eval("SalespersonName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Eval("SalespersonName") %>' Enabled="false"></asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn SortExpression="Shortname" HeaderText="Short Name" FilterCheckListEnableLoadOnDemand="true"  HeaderButtonType="TextButton"
                                DataField="Shortname" UniqueName="Shortname" GroupByExpression="Shortname group by Shortname">
                            </telerik:GridBoundColumn>
                              <telerik:GridCheckBoxColumn SortExpression="IsBudget" HeaderText="Is Budget" UniqueName="IsBudget"
                                ItemStyle-HorizontalAlign="Center" AllowFiltering="false" headerstyle-Width="7%" DataField="IsBudget">
                            </telerik:GridCheckBoxColumn>
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
            $(".sub-teamSetup").addClass("active");
        });
    </script>
</asp:Content>
