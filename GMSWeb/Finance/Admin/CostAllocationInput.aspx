<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="CostAllocationInput.aspx.cs" Inherits="GMSWeb.Finance.Admin.CostAllocationInput" %>


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
                    <telerik:AjaxUpdatedControl ControlID="RadPdfViewer1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <style>
        div.RadUpload .ruBrowse {
            background-position: 0 -23px;
            width: 75px;
            height: 10%;
        }

        div.RadUpload .ruFakeInput {
            height: 10%;
        }
    </style>
    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">
            <asp:Label ID="lblPageHeader" runat="server" /></a></li>
        <li class="active">Cost Allocation</li>
    </ul>
    <h1 class="page-header">Cost Allocation<small></small></h1>
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
        <%--<asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="pull-right btn btn-primary m-l-5" OnClick="btnSearch_Click"></asp:Button>--%>
        <telerik:RadButton ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="pull-right btn btn-lg bg-primary m-l-5 largeFont" OnClick="btnSearch_Click"
             SingleClick="true" SingleClickText="Retrieving"></telerik:RadButton>
    </div>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="MsgPanel2" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" SelectedIndex="0" runat="server" MultiPageID="RadMultiPage1" Skin="Bootstrap" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="Input Screen" PageViewID="PageView1">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Upload Document" PageViewID="PageView2">
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

                <telerik:RadWindowManager runat="server">
                    <Windows>
                        <telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" AutoSize="true" Skin="Bootstrap"
                            Modal="true" Style="z-index: 100001;">
                            <Localization PinOn="Pin on"/>
                            <ContentTemplate>
                                <telerik:RadPdfViewer runat="server" ID="RadPdfViewer1" Height="550px" Width="100%" Scale="0.9" Skin="Bootstrap" EnableViewState="true">
                                     <ToolBarSettings Items="pager, spacer, zoom, toggleSelection, spacer, search, open, download" />
                                    <PdfjsProcessingSettings></PdfjsProcessingSettings>
                                </telerik:RadPdfViewer>
                            </ContentTemplate>
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>

                <div class="panel-body no-padding">
                    <div class="table-responsive" style="overflow: auto">
                        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AutoGenerateColumns="False" Skin="Bootstrap"
                            PageSize="20" AllowSorting="True" AllowFilteringByColumn="true" AllowPaging="True" ShowStatusBar="true"
                            OnNeedDataSource="RadGrid1_NeedDataSource" OnDetailTableDataBind="RadGrid1_DetailTableDataBind"
                            OnPageIndexChanged="RadGrid1_OnPageIndexChanged" OnPageSizeChanged="RadGrid1_OnPageSizeChanged"
                            OnInsertCommand="RadGrid1_InsertCommand" OnItemCreated="RadGrid1_OnItemCreated" OnUpdateCommand="RadGrid1_UpdateCommand"
                            OnPreRender="RadGrid1_PreRender" OnDeleteCommand="RadGrid1_DeleteCommand" OnItemDataBound="RadGrid1_ItemDataBound">
                            <MasterTableView DataKeyNames="ID" AllowMultiColumnSorting="True" CommandItemDisplay="Top" Name="Parent">
                                <DetailTables>
                                    <telerik:GridTableView DataKeyNames="ParentID,DetailItemNo" Name="ChildItem" Width="100%"
                                        CommandItemDisplay="Top" HierarchyLoadMode="ServerOnDemand">

                                        <ParentTableRelation>
                                            <telerik:GridRelationFields DetailKeyField="ParentID" MasterKeyField="ID"></telerik:GridRelationFields>
                                        </ParentTableRelation>
                                        <Columns>
                                            <telerik:GridEditCommandColumn UniqueName="EditCommandColumn1">
                                                <HeaderStyle Width="20px"></HeaderStyle>
                                                <ItemStyle CssClass="MyImageButton"></ItemStyle>
                                            </telerik:GridEditCommandColumn>
                                            <telerik:GridTemplateColumn SortExpression="DetailItemNo" AllowFiltering="false" HeaderText="S/N" HeaderButtonType="TextButton" HeaderStyle-Width="5%"
                                                DataField="DetailItemNo">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn SortExpression="tbYear" AllowFiltering="false" HeaderText="Year" HeaderStyle-Width="5%"
                                                HeaderButtonType="TextButton" DataField="tbYear" UniqueName="tbYear">
                                                <ItemTemplate>
                                                    <%# Eval("tbYear") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn SortExpression="tbMonth" AllowFiltering="false" HeaderText="Month" HeaderStyle-Width="5%"
                                                HeaderButtonType="TextButton" DataField="tbMonth" UniqueName="tbMonth">
                                                <ItemTemplate>
                                                    <%# Eval("tbMonth") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn SortExpression="Division" AllowFiltering="false" HeaderText="D1"
                                                HeaderButtonType="TextButton" DataField="Division" UniqueName="Division">
                                                <ItemTemplate>
                                                    <%# Eval("Division") %>
                                                    <input type="hidden" id="hidDetailItemNo" runat="server" value='<%# Eval("DetailItemNo")%>' />
                                                    <input type="hidden" id="hidAllocatedCost" runat="server" value='<%# Eval("AllocatedCost")%>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlDim1" AutoPostBack="true" CssClass="form-control input-sm"
                                                        OnSelectedIndexChanged="ddlProjectID_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <input type="hidden" id="hidDivision" runat="server" value='<%# Eval("Dim1")%>' />
                                                    <input type="hidden" id="hidTotal" runat="server" value='<%# Eval("Total")%>' />
                                                    <input type="hidden" id="hidDetailItemNo" runat="server" value='<%# Eval("DetailItemNo")%>' />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlDim1" AutoPostBack="true" CssClass="form-control input-sm"
                                                        OnSelectedIndexChanged="ddlProjectID_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </InsertItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn SortExpression="Department" AllowFiltering="false" HeaderText="D2"
                                                HeaderButtonType="TextButton" DataField="Department" UniqueName="Department">
                                                <ItemTemplate>
                                                    <%# Eval("Department") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlDim2" AutoPostBack="true" CssClass="form-control input-sm"
                                                        OnSelectedIndexChanged="ddlDepartmentID_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <input type="hidden" id="hidDepartment" runat="server" value='<%# Eval("Dim2")%>' />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlDim2" Enabled="false" AutoPostBack="true" CssClass="form-control input-sm"
                                                        OnSelectedIndexChanged="ddlDepartmentID_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </InsertItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn SortExpression="Section" AllowFiltering="false" HeaderText="D3"
                                                HeaderButtonType="TextButton" DataField="Section" UniqueName="Section">
                                                <ItemTemplate>
                                                    <%# Eval("Section") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlDim3" AutoPostBack="true" CssClass="form-control input-sm"
                                                        OnSelectedIndexChanged="ddlSectionID_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <input type="hidden" id="hidSection" runat="server" value='<%# Eval("Dim3")%>' />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlDim3" Enabled="false" AutoPostBack="true" CssClass="form-control input-sm"
                                                        OnSelectedIndexChanged="ddlSectionID_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </InsertItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn SortExpression="Unit" AllowFiltering="false" HeaderText="D4"
                                                HeaderButtonType="TextButton" DataField="Unit" UniqueName="Unit">
                                                <ItemTemplate>
                                                    <%# Eval("Unit") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlDim4" CssClass="form-control input-sm"></asp:DropDownList>
                                                    <input type="hidden" id="hidUnit" runat="server" value='<%# Eval("Dim4")%>' />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlDim4" Enabled="false" CssClass="form-control input-sm"></asp:DropDownList>
                                                </InsertItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn SortExpression="ItemName" AllowFiltering="false" HeaderText="Item Name" HeaderButtonType="TextButton" HeaderStyle-Width="5%"
                                                DataField="ItemName" ItemStyle-Wrap="false" EditFormHeaderTextFormat="<b>Item Name:</b>" UniqueName="ItemName">
                                                <ItemTemplate>
                                                    <%# Eval("ItemName")  %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <telerik:RadAutoCompleteBox RenderMode="Lightweight" runat="server" ID="txtItemName" SelectMethod=""
                                                        InputType="Text" DropDownWidth="40%" Width="100%" DataValueField="ItemID" DataTextField="ItemName">
                                                    </telerik:RadAutoCompleteBox>
                                                    <input type="hidden" id="hidItemName" runat="server" value='<%# Eval("ItemName")%>' />
                                                    <input type="hidden" id="hidChildID" runat="server" value='<%# Eval("ChildID")%>' />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <telerik:RadAutoCompleteBox RenderMode="Lightweight" runat="server" ID="txtItemName"
                                                        InputType="Text" DropDownWidth="40%" Width="100%" DataValueField="ItemID" DataTextField="ItemName">
                                                    </telerik:RadAutoCompleteBox>
                                                </InsertItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn SortExpression="Percentage" AllowFiltering="false" HeaderText="Percentage"
                                                HeaderButtonType="TextButton" DataField="Percentage" UniqueName="Percentage">
                                                <ItemTemplate>
                                                    <%# Eval("Percentage") +" %" %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtPercentage" placeholder="e.g. 50" CssClass="form-control input-sm" Text=' <%# Eval("Percentage")%> '></asp:TextBox>
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtPercentage" placeholder="e.g. 50" CssClass="form-control input-sm"></asp:TextBox>
                                                </InsertItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn SortExpression="UOM" AllowFiltering="false" HeaderText="UOM"
                                                HeaderButtonType="TextButton" DataField="UOM" UniqueName="UOM">
                                                <ItemTemplate>
                                                    <%# Eval("UOM") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlUOM" CssClass="form-control input-sm"></asp:DropDownList>
                                                    <input type="hidden" id="hidUOM" runat="server" value='<%# Eval("UOM")%>' />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlUOM" CssClass="form-control input-sm"></asp:DropDownList>
                                                </InsertItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn SortExpression="Quantity" AllowFiltering="false" HeaderText="Quantity" HeaderButtonType="TextButton"
                                                DataField="Quantity" UniqueName="Quantity">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="UnitCost" AllowFiltering="false" HeaderText="UnitCost" HeaderButtonType="TextButton"
                                                DataField="UnitCost" UniqueName="UnitCost">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="AllocatedCost" AllowFiltering="false" HeaderText="AllocatedCost" HeaderButtonType="TextButton"
                                                DataField="AllocatedCost" UniqueName="AllocatedCost">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn SortExpression="FileName" AllowFiltering="false" HeaderText="Files"
                                                HeaderButtonType="TextButton" DataField="FileName" UniqueName="FileName" EditFormHeaderTextFormat="<b>File Upload:</b>" InsertVisiblityMode="AlwaysHidden">
                                                <ItemTemplate>
                                                    <telerik:RadButton runat="server" ID="btnViewFile" ButtonType="ToggleButton" Text="View File"
                                                        Width="100%" Height="100%" OnClientClicked="openwin" OnClick="btnViewFile_Click">
                                                    </telerik:RadButton>
                                                    <input type="hidden" id="hidFileName" runat="server" value='<%# Eval("FileName")%>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="RadAsycnUpload1" OnClientValidationFailed="OnClientValidationFailed"
                                                        OnFileUploaded="RadAsycnUpload1_onFileUploaded" Width="100%" MultipleFileSelection="Disabled" MaxFileInputsCount="1"
                                                        AllowedFileExtensions="pdf" >
                                                    </telerik:RadAsyncUpload>
                                                </EditItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridButtonColumn ConfirmText="Delete this data?"
                                                CommandName="Delete" Text="Delete" UniqueName="DeleteColumn1" Visible="true">
                                                <ItemStyle CssClass="MyImageButton"></ItemStyle>
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </telerik:GridTableView>
                                </DetailTables>
                                <Columns>
                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="S/N" HeaderButtonType="TextButton" HeaderStyle-Width="5%" InsertVisiblityMode="AlwaysHidden">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn SortExpression="ItemNo" Visible="false" HeaderText="Item Code" HeaderButtonType="TextButton" HeaderStyle-Width="10%" UniqueName="ItemNo" FilterControlWidth="10%"
                                        DataField="ItemNo" InsertVisiblityMode="AlwaysHidden">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn SortExpression="tbYear" AllowFiltering="false" HeaderText="Year" HeaderStyle-Width="5%"
                                        HeaderButtonType="TextButton" DataField="tbYear" UniqueName="tbYear">
                                        <ItemTemplate>
                                            <%# Eval("tbYear") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlYear" AutoPostBack="true" CssClass="form-control input-sm"
                                                OnSelectedIndexChanged="ddlGridYearMonth_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn SortExpression="tbMonth" AllowFiltering="false" HeaderText="Month" HeaderStyle-Width="5%"
                                        HeaderButtonType="TextButton" DataField="tbMonth" UniqueName="tbMonth">
                                        <ItemTemplate>
                                            <%# Eval("tbMonth") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlMonth" AutoPostBack="true" CssClass="form-control input-sm"
                                                OnSelectedIndexChanged="ddlGridYearMonth_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn SortExpression="Division" AllowFiltering="false" HeaderText="D1" HeaderStyle-Width="10%"
                                        HeaderButtonType="TextButton" DataField="Division" UniqueName="Division">
                                        <ItemTemplate>
                                            <%# Eval("Division") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlDim1" AutoPostBack="true" CssClass="form-control input-sm"
                                                OnSelectedIndexChanged="ddlProjectID_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn SortExpression="Department" AllowFiltering="false" HeaderText="D2"
                                        HeaderButtonType="TextButton" DataField="Department" UniqueName="Department" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <%# Eval("Department") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlDim2" AutoPostBack="true" Enabled="false" CssClass="form-control input-sm"
                                                OnSelectedIndexChanged="ddlDepartmentID_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn SortExpression="Section" AllowFiltering="false" HeaderText="D3" HeaderStyle-Width="10%"
                                        HeaderButtonType="TextButton" DataField="Section" UniqueName="Section">
                                        <ItemTemplate>
                                            <%# Eval("Section") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlDim3" AutoPostBack="true" Enabled="false" CssClass="form-control input-sm"
                                                OnSelectedIndexChanged="ddlSectionID_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn SortExpression="Unit" AllowFiltering="false" HeaderText="D4"
                                        HeaderButtonType="TextButton" DataField="Unit" UniqueName="Unit">
                                        <ItemTemplate>
                                            <%# Eval("Unit") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlDim4" Enabled="false" CssClass="form-control input-sm"></asp:DropDownList>
                                        </InsertItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn SortExpression="ItemName" HeaderText="Item Name" HeaderButtonType="TextButton"
                                        DataField="ItemName" UniqueName="ItemName" EditFormHeaderTextFormat="<b>Item Name:</b>">
                                        <ItemTemplate>
                                            <%# Eval("ItemName")%>
                                            <input type="hidden" id="hidID" runat="server" value='<%# Eval("ID")%>' />
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <telerik:RadAutoCompleteBox RenderMode="Lightweight" runat="server" ID="txtItemName"
                                                InputType="Text" DropDownWidth="40%" Width="100%" DataValueField="ItemID" DataTextField="ItemName">
                                            </telerik:RadAutoCompleteBox>
                                        </InsertItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn SortExpression="Cost" AllowFiltering="false" HeaderText="Total Cost" HeaderButtonType="TextButton"
                                        DataField="Cost" UniqueName="TOTAL" InsertVisiblityMode="AlwaysHidden">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn SortExpression="Method" AllowFiltering="false" HeaderText="Allocation Method"
                                        HeaderButtonType="TextButton" DataField="Method" UniqueName="Method">
                                        <ItemTemplate>
                                            <%# Eval("Method") %>
                                            <input type="hidden" id="hidMethod" runat="server" value='<%# Eval("Method")%>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlAllocationMethod" CssClass="form-control input-sm">
                                                <asp:ListItem Text="%" Value="Percent" Selected />
                                                <asp:ListItem Text="Amount" Value="Amount" />
                                                <asp:ListItem Text="UOM" Value="UOM" />
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn ConfirmText="Delete this data?(**Child Data will be deleted too!)"
                                        CommandName="Delete" Text="Delete" UniqueName="DeleteColumn1" Visible="true">
                                        <ItemStyle CssClass="MyImageButton"></ItemStyle>
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
        </telerik:RadPageView>
        <telerik:RadPageView ID="PageView2" runat="server">
            <br>
            <table border="0" width="100%" height="100%" align="center" id="Table1">
                <tr>
                    <td valign="middle" align="center">
                        <div>
                            This page is still under construction. Stay tuned for the upcoming new feature in GMS!
						        <br>
                            For more information, please contact your System Administrator.
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
    </telerik:RadMultiPage>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".setting-menu").addClass("active expand");
            $(".sub-costallocation-input").addClass("active");
        });

        window.pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.worker.js';

        function openwin() {
            window.radopen(null, "RadWindow1");
        }

        function OnClientValidationFailed(sender, args) {
            var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);
            if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
                if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {
                    alert("Only PDF files allowed!");
                }
                else {
                    alert("Wrong file size!");
                }
            }
            else {
                alert("Only PDF files allowed!");
            }
        }
    </script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.js"></script><!--For Telerik PDF Viewer-->
</asp:Content>
