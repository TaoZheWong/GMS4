<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="NewPriceList.aspx.cs" Inherits="GMSWeb.Sales.Sales.NewPriceList" Title="Price List"%>

<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<asp:ScriptManager ID="sriptmgr1" runat="server"></asp:ScriptManager>
<a name="TemplateInfo"></a>
<ul class="breadcrumb pull-right">
    <li><a href="#"><asp:Label ID="lblPageHeader" runat="server" /></a></li>
    <li class="active">Price & Reorder Level Input</li>
</ul>
<h1 class="page-header">Price & Reorder Level Input<small> Setup of item price & reorder level.</small></h1>
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
                    <div class="form-group col-lg-3 col-md-6 col-sm-6 hidden">
                        <label class="control-label">Brand</label>
                        <asp:DropDownList CssClass="form-control input-sm" ID="ddlSearchBrandName" runat="Server" DataTextField="Brand" DataValueField="BrandID"
                             AutoPostBack="true"/>
                    </div>
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
                    <div id="hiddenLabel" runat="server" class="form-group col-lg-3 col-md-6 col-sm-6" visible="false">
                        <label class="control-label">Product Group</label>
                        <asp:DropDownList CssClass="form-control input-sm" ID="ddlSearchProductGroup" runat="Server" DataTextField="ProductGroupCodeName" DataValueField="ProductGroupCode"/>
                    </div>
                   <div class="form-group col-lg-3 col-md-6 col-sm-6 ">
                        <label class="control-label">Item Code</label>
                            <asp:TextBox runat="server" ID="txtProductCode" MaxLength="50" Columns="20" onfocus="select();" CssClass="form-control" placeholder="e.g. B1110535616"></asp:TextBox>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6 ">
                        <label class="control-label">Item Name</label>
                            <asp:TextBox runat="server" ID="txtProductName" MaxLength="50" Columns="20" onfocus="select();"
                                CssClass="form-control" placeholder="e.g. BLUE-TIG 5356"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel-footer clearfix">
        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="pull-right btn btn-primary m-l-5" OnClick="btnSearch_Click"></asp:Button> 
        <asp:Button ID="btnExport" Text="Export To Excel" EnableViewState="False" runat="server" CssClass="pull-right btn btn-success m-l-5" OnClick="btnExport_Click" Visible="false"></asp:Button> 
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
                <asp:DataGrid ID="DataGrid1" runat="server" Visible="false" AutoGenerateColumns="false" ShowFooter="true"
                    DataKeyField="ProductCode" OnCancelCommand="DataGrid1_CancelCommand" OnEditCommand="DataGrid1_EditCommand"
                    OnUpdateCommand="DataGrid1_UpdateCommand" OnItemCommand="DataGrid1_CreateCommand" GridLines="none"
                    OnItemDataBound="DataGrid1_ItemDataBound" OnDeleteCommand="DataGrid1_DeleteCommand" PageSize="20" 
                    CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    EnableViewState="true" OnPageIndexChanged="DataGrid1_PageIndexChanged">
                    <Columns>
                        <asp:TemplateColumn HeaderText="S/N" >
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((DataGrid1.CurrentPageIndex) * DataGrid1.PageSize)  %>
                                <input type="hidden" Name="hidProductCode" id="hidProductCode" runat="server" value='<%#Eval("ProductCode")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Item Code" HeaderStyle-Wrap="true" FooterStyle-Wrap="false" FooterStyle-Width="50px">
                            <ItemTemplate>
                                     <asp:Label ID="lblProductCode" runat="server">
                                        <span><%# Eval( "ProductCode")%></span>                       
                                     </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblProductCode2" runat="server">
                                        <span><%# Eval( "ProductCode")%></span>                       
                                     </asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate>                              
                                <asp:DropDownList CssClass="form-control input-sm" ID="ddlNewProductGroup" runat="Server" DataTextField="ProductGroupName"
                                    DataValueField="productgroupcode" AutoPostBack="true" OnSelectedIndexChanged="ddlNewProductGroup_SelectedIndexChanged"/>
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Item Description" HeaderStyle-Wrap="false" ItemStyle-Width="200px" ItemStyle-Wrap="true" FooterStyle-Wrap="true">
                            <ItemTemplate>
                                     <asp:Label ID="Label1" runat="server">
                                    <asp:LinkButton ID="LinkButtonProductName" runat="server" CommandName="Edit" EnableViewState="true"
                                                CausesValidation="false"><span><%# Eval( "ProductName")%></span></asp:LinkButton>                            
                                     </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="Label3" runat="server"><%# Eval( "ProductName")%></asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList CssClass="form-control input-sm" ID="ddlNewProduct" runat="Server"  DataTextField="ProductCodeName"
                                    DataValueField="ProductCode" />
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Model No" HeaderStyle-Wrap="true" Visible="false">
                            <ItemTemplate>                             
                                <%# Eval( "ModelNo")%>
                            </ItemTemplate>
                             <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditModelNo" runat="server" Columns="15" MaxLength="50" Text='NA'/>
                                 <%--<asp:RequiredFieldValidator
									ID="rfvEditModelNo" runat="server" ControlToValidate="txtEditModelNo" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditProductPrice" />--%>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewModelNo" runat="server" Columns="15" MaxLength="50" Text='NA'/>
                                   <%--<asp:RequiredFieldValidator
									ID="rfvNewModelNo" runat="server" ControlToValidate="txtNewModelNo" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewProductPrice" />--%>
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Item Type" HeaderStyle-Wrap="true" Visible="false">
                            <ItemTemplate>
                                 <%# Eval( "ProductType")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditProductType" runat="server" Columns="15" MaxLength="50"  Text='NA' />
                                <%-- <asp:RequiredFieldValidator
									ID="rfvEditProductType" runat="server" ControlToValidate="txtEditProductType" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditProductPrice" />--%>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewProductType" runat="server" Columns="15" MaxLength="50"  Text='NA'/>
                                  <%-- <asp:RequiredFieldValidator
									ID="rfvNewProductType" runat="server" ControlToValidate="txtNewProductType" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewProductPrice" />--%>
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Dealer Price" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                 <%# Eval( "DealerPrice")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditDealerPrice" runat="server" Columns="15" MaxLength="50" Text='<%# Eval("DealerPrice")%>' />
                                 <asp:RequiredFieldValidator
									ID="rfvEditDealerPrice" runat="server" ControlToValidate="txtEditDealerPrice" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditProductPrice" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewDealerPrice" runat="server" Columns="15" MaxLength="50" />
                                   <asp:RequiredFieldValidator
									ID="rfvNewDealerPrice" runat="server" ControlToValidate="txtNewDealerPrice" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewProductPrice" />
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="User Price" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                 <%# Eval( "UserPrice")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditUserPrice" runat="server" Columns="15" MaxLength="50" Text='<%# Eval("UserPrice")%>' />
                                 <asp:RequiredFieldValidator
									ID="rfvEditUserPrice" runat="server" ControlToValidate="txtEditUserPrice" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditProductPrice" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewUserPrice" runat="server" Columns="15" MaxLength="50" />
                                   <asp:RequiredFieldValidator
									ID="rfvNewUserPrice" runat="server" ControlToValidate="txtNewUserPrice" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewProductPrice" />
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Retail Price" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                 <%# Eval( "RetailPrice")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm " ID="txtEditRetailPrice" runat="server" Columns="15" MaxLength="50" Text='<%# Eval("RetailPrice")%>' />
                                 <asp:RequiredFieldValidator
									ID="rfvEditRetailPrice" runat="server" ControlToValidate="txtEditRetailPrice" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditProductPrice" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewRetailPrice" runat="server" Columns="15" MaxLength="50" />
                                   <asp:RequiredFieldValidator
									ID="rfvNewRetailPrice" runat="server" ControlToValidate="txtNewRetailPrice" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewProductPrice" />
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Reorder Level" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                  <%# Eval( "ReorderLevel")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditReorderLevel" runat="server" Columns="15" MaxLength="50" Text='<%# Eval("ReorderLevel")%>' />
                                 <asp:RequiredFieldValidator
									ID="rfvEditReorderLevel" runat="server" ControlToValidate="txtEditReorderLevel" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditProductPrice" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewReorderLevel" runat="server" Columns="15" MaxLength="50" />
                                   <asp:RequiredFieldValidator
									ID="rfvNewReorderLevel" runat="server" ControlToValidate="txtNewReorderLevel" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewProductPrice" />
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Clearing Stock" HeaderStyle-Wrap="true" >
                            <ItemTemplate>                                
                                <asp:CheckBox Enabled="false" runat="server" Checked='<%# Convert.ToBoolean(Eval("TradingStock"))%>'></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox Enabled="true" runat="server" id="chkbxEditTradingStock" Checked='<%# Convert.ToBoolean(Eval("TradingStock"))%>'></asp:CheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox Enabled="true" runat="server" ID="chkbxNewTradingStock"></asp:CheckBox>
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Inactive" HeaderStyle-Wrap="true" >
                            <ItemTemplate>                                
                                <asp:CheckBox Enabled="false" runat="server" Checked='<%# Convert.ToBoolean(Eval("Inactive"))%>'></asp:CheckBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox Enabled="true" runat="server" id="chkbxEditInactive" Checked='<%# Convert.ToBoolean(Eval("Inactive"))%>'></asp:CheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox Enabled="true" runat="server" ID="chkbxNewInactive"></asp:CheckBox>
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Effective Date">
                            <ItemTemplate>
                                 <asp:label ID="lblUpdatedDate" runat="server">
                                    <%# Eval( "EffectiveDate")%>
                                </asp:label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="input-group date">
                                    <asp:TextBox CssClass="form-control  input-sm datepicker" ID="txtEditEffectiveDate" runat="server" Text='<%# Eval("EffectiveDate")%>'/>
                                    <span class="input-group-addon"><i class="ti-calendar"></i></span>
					            </div>
                                 <asp:RequiredFieldValidator
									ID="rfvEditEffectiveDate" runat="server" ControlToValidate="txtEditEffectiveDate" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditProductPrice" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <div class="input-group date">
                                    <asp:TextBox CssClass="form-control  input-sm datepicker" ID="txtNewEffectiveDate" runat="server"/>
                                    <span class="input-group-addon"><i class="ti-calendar"></i></span>
					            </div>
                                   <asp:RequiredFieldValidator
									ID="rfvNewEffectiveDate" runat="server" ControlToValidate="txtNewEffectiveDate" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewProductPrice" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                      
                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete2" runat="server" CommandName="Delete" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Update" EnableViewState="false"
                                    ValidationGroup="valGrpEditProductPrice" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Cancel" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Create" EnableViewState="false"
                                    ValidationGroup="valGrpNewProductPrice" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="tHeader" />
                    <FooterStyle CssClass="tFooter" />
                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                </asp:DataGrid>
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
    <br>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".administration-menu").addClass("active expand");
            $(".sub-newpricelist").addClass("active");
        });
    </script>

    <!--<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>-->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/TableDnD/0.9.1/jquery.tablednd.js" integrity="sha256-d3rtug+Hg1GZPB7Y/yTcRixO/wlI78+2m08tosoRn7A=" crossorigin="anonymous"></script>
    <script type="text/javascript">
        //drag and drop using tableDnD & call ajax web method onDrop to save 
        var strorder;

        $(document).ready(function() {
            $('.DragRow').tableDnD(
                {
                    onDrop: function(table, row) {
                        reorder();
                        $.ajax({
                                 type: "POST",
                                 url: "NewPriceList.aspx/GridViewReorders",
                                 data: '{"Reorder":"'+strorder+'"}',
                                 contentType: "application/json; charset=utf-8",
                                 dataType: "json",
                                 async: true,
                                 cache: false,
                                 success: function (msg) {
                                    alert("Successfully Save Product Order");
                                 }
                        })
                    }
                }
            );
        });
        function  reorder()
        {
            strorder="";
            var totalid = $('.DragRow tr td input').length;
            for(var i=0;i<totalid;i++)
            {
                strorder = strorder + $('.DragRow tr td input')[i].getAttribute("value") + "|";
            }
        }
    </script>
    
                           
</asp:Content>