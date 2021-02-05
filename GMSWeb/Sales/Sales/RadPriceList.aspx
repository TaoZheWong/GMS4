<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="RadPriceList.aspx.cs" Inherits="GMSWeb.Sales.Sales.RadPriceList" Title="Price List"%>

<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
             <telerik:AjaxSetting AjaxControlID="RadSpreadsheet1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadSpreadsheet1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkDelete2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <style>
        .RadSpreadsheet .rssSheetsbar a.t-spreadsheet-sheets-bar-add
        {
             display: none;
        }

        .rssPopup .rssCollapsibleList .rssDetails {
            height: 150px;
        }
    </style>
<a name="TemplateInfo"></a>
<ul class="breadcrumb pull-right">
    <li><a href="#"><asp:Label ID="lblPageHeader" runat="server" /></a></li>
    <li class="active">Price & Reorder Level Input</li>
</ul>
<h1 class="page-header">Price & Reorder Level Input<small> Setup of product price & reorder level.</small></h1>
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
                        <label class="control-label">Brand/Product Code</label>
                            <asp:TextBox runat="server" ID="txtProductGroupCode" MaxLength="4" Columns="20" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. B11"></asp:TextBox>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Brand/Product Name</label>
                            <asp:TextBox runat="server" ID="txtProductGroup" MaxLength="50" Columns="20" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. BLUEMETALS"></asp:TextBox>
                    </div>
                   <div class="form-group col-lg-3 col-md-6 col-sm-6 " hidden>
                        <label class="control-label">Item Code</label>
                            <asp:TextBox runat="server" ID="txtProductCode" MaxLength="50" Columns="20" onfocus="select();" CssClass="form-control" placeholder="e.g. B1110535616"></asp:TextBox>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6 " hidden>
                        <label class="control-label">Item Description</label>
                            <asp:TextBox runat="server" ID="txtProductName" MaxLength="50" Columns="20" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. BLUE-TIG 5356"></asp:TextBox>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Status</label>
                        <asp:DropDownList CssClass="form-control input-sm" ID="ddlSearchStatus" runat="Server">
                            <asp:ListItem Text="Current" Value="" Selected/>
                            <asp:ListItem Text="Pending Approval" Value="Pending"/>
                            <asp:ListItem Text="Rejected" Value="Rejected"/>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel-footer clearfix">
        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="pull-right btn btn-primary m-l-5" OnClick="btnSearch_Click"></asp:Button> 
    </div>

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
            <div class="table-responsive" style="overflow:auto">
                <!--Current price list-->
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:HiddenField ID="hidEmail" runat="server"/>
                <telerik:RadButton runat="server" ID="RadButton1" Text="Submit" CssClass="pull-right btn m-l-5" Skin="Bootstrap"
                    OnClick="RadButton1_Click" OnClientClicked="OnClientClicked" visible="false" SingleClick="true"  SingleClickText="Submiting..."/>
                <asp:HiddenField runat="server" ID="hidRowChanged" ClientIDMode="Static" />
                <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet1" Visible="false" OnClientChange="OnClientChange">
                </telerik:RadSpreadsheet>
                 <telerik:RadGrid ID="RadGrid1" runat="server" Visible="false" OnPageIndexChanged="radGrid_OnPageIndexChanged" OnPageSizeChanged="radGrid_OnPageSizeChanged"
                   AllowPaging="True" AutoGenerateColumns="False" Skin="Bootstrap" AllowFilteringByColumn="true" PageSize="20" OnCancelCommand="radGrid_OnCancel" 
                    OnNeedDataSource="RadGrid1_NeedDataSource" OnUpdateCommand="RadGrid1_OnUpdateCommand">

                    <MasterTableView CommandItemDisplay="Top" InsertItemDisplay="Top" EditMode="Batch" CommandItemSettings-ShowSaveChangesButton="true"
                        CommandItemSettings-ShowRefreshButton="false" DataKeyNames="ProductCode" CommandItemSettings-ShowAddNewRecordButton="false">
                        <BatchEditingSettings EditType="Row" SaveAllHierarchyLevels="true" OpenEditingEvent="MouseOver" HighlightDeletedRows="true"/>
                
                        <Columns>
                            <telerik:GridTemplateColumn  HeaderText="S/N" AllowFiltering="false" headerstyle-Width="5%">  
                                <ItemTemplate>
                                     <%# (Container.ItemIndex + 1) + ((RadGrid1.CurrentPageIndex) * RadGrid1.PageSize)  %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Item Code column" HeaderText="Item Code" DataField="ProductCode" ReadOnly="true" FilterControlWidth="70%" headerstyle-Width="12%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Item Description column" HeaderText="Item Description" DataField="ProductName" ReadOnly="true" FilterControlWidth="70%" HeaderStyle-Width="20%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Weighted Cost column" HeaderText="Weighted Cost" HeaderStyle-Wrap="true" DataField="WeightedCost" ReadOnly="true" FilterControlWidth="70%">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter Dealer Price column" HeaderText="Dealer Price" UniqueName="DealerPrice" FilterControlWidth="70%">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtDealerPrice" Text='<%#Eval( "DealerPrice")%>' BorderStyle="None"></asp:TextBox>
                                    </br>
                                    <asp:Label runat="server" ID="lblDPercent"><i><%#Eval( "DPercent")%>%</i></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" InputType="Number" ID="txtRadDealerPrice" runat="server" Width="100px"></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn   FilterControlAltText="Filter User Price column" HeaderText="User Price" FilterControlWidth="70%" UniqueName="UserPrice">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtUserPrice" Text='<%#Eval( "UserPrice")%>' BorderStyle="None" Enabled="true"></asp:TextBox>
                                    </br>
                                    <asp:Label runat="server"><i><%#Eval("UPercent")%>%</i></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <telerik:RadTextBox RenderMode="Lightweight" InputType="Number" ID="txtRadUserPrice" runat="server" Width="100px"></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn   FilterControlAltText="Filter Retail Price column" HeaderText="Retail Price" FilterControlWidth="70%" UniqueName="RetailPrice">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtRetailPrice" Text='<%#Eval( "RetailPrice")%>' BorderStyle="None" Enabled="true"></asp:TextBox>
                                    </br>
                                    <asp:Label runat="server"><i><%#Eval("RPercent")%>%</i></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <telerik:RadTextBox RenderMode="Lightweight" InputType="Number" ID="txtRadRetailPrice" runat="server" Width="100px"></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="ReorderLevel" FilterControlAltText="Filter Reorder Level column" HeaderText="Reorder Level" FilterControlWidth="70%">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtReorderLevel" Text='<%#Eval( "ReorderLevel")%>' BorderStyle="None" Enabled="true"></asp:TextBox>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <telerik:RadTextBox RenderMode="Lightweight" InputType="Number" ID="txtRadReorderLevel" runat="server" Width="100px"></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter Clearing Stock column" HeaderText="Clearing Stock" FilterControlWidth="70%" UniqueName="TradingStock"
                                ItemStyle-HorizontalAlign="Center" AllowFiltering="false" headerstyle-Width="8%">
                                 <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkbxTradingStock" Checked='<%#Convert.ToBoolean(Eval( "TradingStock"))%>' ></asp:CheckBox>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:CheckBox runat="server" BorderStyle="None"></asp:CheckBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn UniqueName="EffectiveDate" FilterControlAltText="Filter Effective Date column" HeaderText="Effective Date" FilterControlWidth="70%">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtEffectiveDate" Text='<%#Eval( "EffectiveDate")%>' BorderStyle="None" Enabled="true"></asp:TextBox>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadDatePicker ID="RadDatePicker1" runat="server" DateInput-DateFormat="dd/MM/yyyy"></telerik:RadDatePicker>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <!--Pending price list-->
               <telerik:RadGrid ID="RadGrid2" runat="server" Visible="false" OnPageIndexChanged="radGrid2_OnPageIndexChanged" OnPageSizeChanged="radGrid2_OnPageSizeChanged"
                   AllowPaging="True" AutoGenerateColumns="False" Skin="Bootstrap" AllowFilteringByColumn="true" PageSize="20" OnCancelCommand="radGrid2_OnCancel" 
                    OnNeedDataSource="RadGrid2_NeedDataSource" OnDeleteCommand="RadGrid2_OnDeleteCommand" OnItemDataBound="radGrid2_OnItemDataBound">

                    <MasterTableView CommandItemDisplay="None"
                        CommandItemSettings-ShowRefreshButton="false" DataKeyNames="ProductCode">
                
                        <Columns>
                            <telerik:GridTemplateColumn  HeaderText="S/N" AllowFiltering="false" headerstyle-Width="5%">  
                                <ItemTemplate>
                                     <%# (Container.ItemIndex + 1) + ((RadGrid2.CurrentPageIndex) * RadGrid2.PageSize)  %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Item Code column" HeaderText="Item Code" DataField="ProductCode" ReadOnly="true" FilterControlWidth="70%" headerstyle-Width="10%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Item Description column" HeaderText="Item Description" DataField="ProductName" ReadOnly="true" FilterControlWidth="70%" HeaderStyle-Width="17%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Weighted Cost column" HeaderText="Weighted Cost" HeaderStyle-Wrap="true" DataField="WeightedCost" ReadOnly="true" AllowFiltering="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn AllowFiltering="false"  FilterControlAltText="Filter Dealer Price column" HeaderText="Dealer Price" UniqueName="DealerPrice">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="txtDealerPrice" Text='<%#Eval( "DealerPrice")%>' Font-Bold="true"></asp:Label>
                                    </br>
                                    <asp:Label runat="server" ID="lblDPercent"><i><%#Eval( "DPercent")%>%</i></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowFiltering="false"  FilterControlAltText="Filter User Price column" HeaderText="User Price" UniqueName="UserPrice">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="txtUserPrice" Text='<%#Eval( "UserPrice")%>' Font-Bold="true"></asp:Label>
                                    </br>
                                    <asp:Label runat="server"><i><%#Eval("UPercent")%>%</i></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowFiltering="false"  FilterControlAltText="Filter Retail Price column" HeaderText="Retail Price" UniqueName="RetailPrice">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="txtRetailPrice" Text='<%#Eval( "RetailPrice")%>' Font-Bold="true"></asp:Label>
                                    </br>
                                    <asp:Label runat="server"><i><%#Eval("RPercent")%>%</i></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Reorder Level column" HeaderText="Reorder Level" HeaderStyle-Wrap="true" DataField="ReorderLevel" ReadOnly="true" AllowFiltering="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridCheckBoxColumn FilterControlAltText="Filter Clearing Stock column" HeaderText="Clearing Stock" FilterControlWidth="50%" UniqueName="TradingStock"
                                ItemStyle-HorizontalAlign="Center" AllowFiltering="false" headerstyle-Width="7%" DataField="TradingStock">
                            </telerik:GridCheckBoxColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Remarks column" HeaderText="Remarks" HeaderStyle-Wrap="true" DataField="Remarks" ReadOnly="true" AllowFiltering="false" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Country column" HeaderText="Country" HeaderStyle-Wrap="true" DataField="Country" ReadOnly="true" AllowFiltering="false" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Last 6 Mths Sales" HeaderStyle-Wrap="true" DataField="Amount" ReadOnly="true" AllowFiltering="false" headerstyle-Width="9%">
                            </telerik:GridBoundColumn>
                             <telerik:GridTemplateColumn HeaderText="Current Inventory Value" UniqueName="StockValue" AllowFiltering="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbl1" Text='<%#"<1 yr: "+Eval( "LessThanEqual1YearCost")%> '></asp:Label>
                                    </br>
                                    <asp:Label runat="server" ID="lbl2" Text='<%#">1 yr: "+Eval("MoreThan1YearCost")%>'></asp:Label>
                                    </br>
                                    <asp:Label runat="server" ID="lbl3" Text='<%#">2 yr: "+Eval( "MoreThan2YearCost")%>'></asp:Label>
                                    </br>
                                    <asp:Label runat="server" ID="lbl4" Text='<%#">3 yr: "+Eval( "MoreThan3YearCost")%>'></asp:Label>
                                    </br>
                                    <asp:Label runat="server" ID="lbl5" Text='<%#"Total: "+Eval( "Total")%>'></asp:Label>
                                    </br>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridBoundColumn FilterControlAltText="Filter Effective Date column" HeaderText="Effective Date" HeaderStyle-Wrap="true" DataField="EffectiveDate" ReadOnly="true" AllowFiltering="false" headerstyle-Width="9%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Submit Date column" HeaderText="Submission Date" HeaderStyle-Wrap="true" DataField="UpdatedDate" ReadOnly="true" AllowFiltering="false" HeaderStyle-Width="9%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Status column" HeaderText="Status" HeaderStyle-Wrap="true" DataField="Status" ReadOnly="true" AllowFiltering="false">
                            </telerik:GridBoundColumn>
                             <telerik:GridTemplateColumn UniqueName="Function" AllowFiltering="false" HeaderText="Function" headerstyle-Width="10%">
                                <ItemTemplate>
                                     <div class="btn-group-vertical">
                                   <asp:LinkButton ID="lnkApprove" runat="server" Visible="false" OnClick="lnkApprove_Click" CommandArgument='<%# Eval("Email")+","+Eval("ProductGroupName")+","+Eval("ProductCode")+","+Eval("PMName")%>' 
                                             CssClass="btn btn-default btn-sm btn-success" data-toggle="tooltip" data-placement="top" title="Approve">Approve</asp:LinkButton>
                                     <asp:LinkButton ID="lnkReject" runat="server" Visible="false" CommandArgument='<%# Eval("Email")+","+Eval("ProductGroupName")+","+Eval("ProductCode")+","+Eval("PMName")%>' OnClick="lnkReject_Click" 
                                         CssClass="btn btn-default btn-sm btn-danger" data-toggle="tooltip" data-placement="top" title="Reject">Reject</asp:LinkButton>
                                    <asp:LinkButton ID="lnkDelete2" runat="server" EnableViewState="true" CommandName="Delete"
                                        CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                                </div>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%-- <telerik:GridButtonColumn ConfirmText="Delete this data?" 
                                        CommandName="Delete" Text="Delete" UniqueName="DeleteColumn1" Visible="true">
                                        <ItemStyle CssClass="MyImageButton"></ItemStyle>
                                    </telerik:GridButtonColumn>--%>
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
            $(".pricing-menu").addClass("active expand");
            $(".sub-radpricelist").addClass("active");
        });

        function OnClientClicked(sender, args) {
            var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
            var jsonstring = JSON.stringify(spreadsheet.get_kendoWidget().toJSON());
            $get("<%= HiddenField1.ClientID %>").value = jsonstring;
            getChangeRowsAsArray();
        }

        //save on row changed event
        var changedRows = {};
        var changedRowsIndices = [];
        function OnClientChange(sender, args) {
            args.get_range()._range._ref.forEachRowIndex(function (rowIndex) {
                changedRows[rowIndex] = true;//save row index into list
            });
        }
        function getChangeRowsAsArray(sender, args) {
            for (var r in changedRows) {
                changedRowsIndices.push(r);
            }
            $("#hidRowChanged").val(changedRowsIndices);
            changedRowsIndices = [];
        }

        if (Telerik.Web.UI.Spreadsheet && Telerik.Web.UI.Spreadsheet.FilterMenuView) {

            Telerik.Web.UI.Spreadsheet.FilterMenuView.prototype.filterValueListBox = function (value) {
                var listBox = this.controls.valueListBox;
                var items = listBox.get_items();
                var count = items.get_count();
                var item;
                var isVisible;
                var hasVisibleItems = false;

                for (var i = 0; i < count; i++) {
                    item = items.getItem(i);

                    isVisible = item.get_text().toLowerCase().indexOf(value.toLowerCase()) > -1;

                    item.set_visible(isVisible);

                    hasVisibleItems = hasVisibleItems || isVisible;
                }

                $telerik.$(listBox.get_element()).find(".rlbCheckAllItems").toggle(hasVisibleItems);
            }
        }
    </script>                  
</asp:Content>