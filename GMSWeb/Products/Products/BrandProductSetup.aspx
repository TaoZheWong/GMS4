<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="BrandProductSetup.aspx.cs" Inherits="GMSWeb.Products.Products.BrandProductSetup" %>

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
        <li class="active">Brand/Product Set Up</li>
    </ul>
    <h1 class="page-header">Brand/Product  Set Up<small> Setup of Brand/Product detail.</small></h1>

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
                    OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand" AllowSorting="True" OnFilterCheckListItemsRequested="RadGrid1_FilterCheckListItemsRequested" 
                    FilterType="HeaderContext"  EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true">

                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="ProductGroupCode"
                        CommandItemSettings-ShowAddNewRecordButton="false" CommandItemSettings-ShowRefreshButton="true">
                        <BatchEditingSettings EditType="Row" />

                        <Columns>
                            <telerik:GridEditCommandColumn UniqueName="EditCommandColumn1">
                                <HeaderStyle Width="10px"></HeaderStyle>
                                <ItemStyle CssClass="MyImageButton"></ItemStyle>
                            </telerik:GridEditCommandColumn>
                            <telerik:GridTemplateColumn  HeaderText="S/N" HeaderButtonType="TextButton" HeaderStyle-Width="3%" AllowFiltering="false">
                                <ItemTemplate>
                                    <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn SortExpression="d1" FilterCheckListEnableLoadOnDemand="true"  HeaderText="D1" HeaderStyle-Width="10%" FilterControlWidth="70%"
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
                             <telerik:GridTemplateColumn SortExpression="d2" FilterCheckListEnableLoadOnDemand="true"  HeaderText="D2" HeaderStyle-Width="10%" FilterControlWidth="70%"
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
                            <telerik:GridTemplateColumn SortExpression="d3" FilterCheckListEnableLoadOnDemand="true"  HeaderText="D3" HeaderStyle-Width="10%" FilterControlWidth="70%"
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
                            <telerik:GridTemplateColumn SortExpression="d4" FilterCheckListEnableLoadOnDemand="true" HeaderText="D4" HeaderStyle-Width="10%" FilterControlWidth="70%"
                                HeaderButtonType="TextButton" DataField="d4" UniqueName="Dim4" GroupByExpression="d4 group by d4">
                                <ItemTemplate>
                                    <%# Eval("d4") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlDim4" CssClass="form-control input-sm"></asp:DropDownList>
                                    <input type="hidden" id="hidDim4" runat="server" value='<%# Eval("Dim4")%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridBoundColumn SortExpression="ProductCategory" FilterCheckListEnableLoadOnDemand="true"  HeaderText="Product Category" HeaderButtonType="TextButton"
                                DataField="ProductCategory" UniqueName="ProductCategory" HeaderStyle-Width="20%" FilterControlWidth="70%" GroupByExpression="ProductCategory group by ProductCategory">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn SortExpression="ProductGroupName" FilterCheckListEnableLoadOnDemand="true"  HeaderText="Brand/Product Name" 
                                HeaderButtonType="TextButton" DataField="ProductGroupName" UniqueName="ProductGroupName"  GroupByExpression="ProductGroupName group by ProductGroupName">
                                <ItemTemplate>
                                    <%# Eval("ProductGroupName") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Eval("ProductGroupName") %>' Enabled="false"></asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn SortExpression="PMName" FilterCheckListEnableLoadOnDemand="true" HeaderText="PMName" 
                                HeaderButtonType="TextButton" DataField="PMName" UniqueName="PMName" GroupByExpression="PMName group by PMName">
                                <ItemTemplate>
                                    <%# Eval("PMName") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn SortExpression="ShortName" FilterCheckListEnableLoadOnDemand="true"  HeaderText="Short Name" HeaderButtonType="TextButton"
                                DataField="ShortName" UniqueName="ShortName" HeaderStyle-Width="20%" FilterControlWidth="70%" GroupByExpression="ShortName group by ShortName">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn SortExpression="MainCategory" FilterCheckListEnableLoadOnDemand="true" HeaderText="Budget Product" HeaderButtonType="TextButton"
                                DataField="MainCategory" UniqueName="MainCategory" HeaderStyle-Width="20%" FilterControlWidth="70%" GroupByExpression="MainCategory group by MainCategory">
                                  <ItemTemplate>
                                    <%# Eval("MainCategory") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlMainCategory" CssClass="form-control input-sm"></asp:DropDownList>
                                    <input type="hidden" id="hidMainCategory" runat="server" value='<%# Eval("BudgetCode")%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridCheckBoxColumn SortExpression="IsBudget" FilterCheckListEnableLoadOnDemand="true" FilterControlAltText="Filter isBudget column" HeaderText="Is Budget" FilterControlWidth="50%" UniqueName="IsBudget"
                                ItemStyle-HorizontalAlign="Center" AllowFiltering="false" headerstyle-Width="7%" DataField="IsBudget" GroupByExpression="IsBudget group by IsBudget" Visible="false">
                            </telerik:GridCheckBoxColumn>
                            <telerik:GridCheckBoxColumn SortExpression="IsActive"  HeaderText="Active" UniqueName="IsActive"
                                ItemStyle-HorizontalAlign="Center" AllowFiltering="false" headerstyle-Width="7%" DataField="IsActive" Display="false">
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
            $(".sub-brandProductSetup").addClass("active");
        });
    </script>
</asp:Content>
