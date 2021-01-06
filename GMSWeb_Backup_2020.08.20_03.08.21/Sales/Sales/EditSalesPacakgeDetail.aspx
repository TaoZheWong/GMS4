<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditSalesPacakgeDetail.aspx.cs" Inherits="GMSWeb.Sales.Sales.EditSalesPacakgeDetail"
    Title="Sales - Edit Package Detail" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-timepicker/bootstrap-timepicker.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/style.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/overwrite_app.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptMgr" runat="server" />
            <div class="container">
                <div class="panel panel-primary m-t-20">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            Package Info
                        </h4>
                    </div>
                    <div class="panel-body">
                        <div class="m-t-20">
                            <div class="form-group col-lg-6 col-sm-12">
                                <label class="col-sm-4 control-label text-left">Package Product Code</label>
                                <label class="col-sm-8 control-label">
                                    : <asp:Label runat="server" ID="lblPackageProductCode"></asp:Label>
                                </label>
                            </div>
                            <div class="form-group col-lg-6 col-sm-12">
                                <label class="col-sm-4 control-label text-left"> Package Product Description</label>
                                <label class="col-sm-8 control-label">
                                     : <asp:Label runat="server" ID="lblPackageProductDescription"></asp:Label>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-primary m-t-20">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            Package Descriptions
                        </h4>
                    </div>
                    <div class="panel-body no-padding">
                        <asp:DataGrid ID="dgDetail" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                GridLines="None" CellPadding="5" OnPageIndexChanged="dgDetail_PageIndexChanged"
                                OnItemCommand="dgDetail_ItemCommand" OnEditCommand="dgDetail_EditCommand"
                                OnCancelCommand="dgDetail_CancelCommand" OnUpdateCommand="dgDetail_UpdateCommand"
                                OnDeleteCommand="dgDetail_DeleteCommand"
                                CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="True" PageSize="20" Width="100%">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((dgDetail.CurrentPageIndex) * dgDetail.PageSize)%>
                                    <input type="hidden" id="hidPackageID" runat="server" value='<%# Eval("PackageID")%>' />
                                            <input type="hidden" id="hidDetailID" runat="server" value='<%# Eval("DetailID")%>' />
                                        </ItemTemplate>

                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Package Description">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true"
                                                CausesValidation="false"><span><%# Eval("Description")%></span></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditProductDescription" runat="server" Columns="80" MaxLength="80" CssClass="form-control input-sm"
                                                Text='<%# Eval("Description") %>' />
                                            <asp:RequiredFieldValidator ID="rfvEditProductDescription" runat="server" ControlToValidate="txtEditProductDescription"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valDetailEditRow" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewProductDescription" runat="server" Columns="80" MaxLength="80" CssClass="form-control input-sm" />
                                            <asp:RequiredFieldValidator ID="rfvNewProductDescription" runat="server" ControlToValidate="txtNewProductDescription"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valDetailNewRow" />
                                        </FooterTemplate>
                                        <HeaderStyle Wrap="False" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Order">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkGoUp" runat="server" CommandName="GoUp" EnableViewState="true" CssClass="btn btn-default btn-sm"><i class="ti-arrow-up"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkGoDown" runat="server" CommandName="GoDown" EnableViewState="true" CssClass="btn btn-default btn-sm"><i class="ti-arrow-down"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle />
                                        <HeaderStyle Wrap="False" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                                CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                                ValidationGroup="valDetailEditRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i> </asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                                CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i> </asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                                ValidationGroup="valDetailNewRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i> </asp:LinkButton>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <HeaderStyle CssClass="tHeader" />
                                <AlternatingItemStyle CssClass="tAltRow" />
                                <FooterStyle CssClass="tFooter" />
                                <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                            </asp:DataGrid>
                    </div>
                </div>
                <div class="panel panel-primary m-t-20">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            List of Products
                        </h4>
                    </div>
                    <div class="panel-body no-padding">
                        <asp:DataGrid ID="dgProduct" runat="server" AutoGenerateColumns="False" ShowFooter="True"
            GridLines="None" CellPadding="5" OnPageIndexChanged="dgProduct_PageIndexChanged"
            OnItemCommand="dgProduct_CreateCommand" OnEditCommand="dgProduct_EditCommand"
            OnCancelCommand="dgProduct_CancelCommand" OnUpdateCommand="dgProduct_UpdateCommand"
            OnDeleteCommand="dgProduct_DeleteCommand"
            CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="True" PageSize="20">
            <Columns>
                <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6">
                    <ItemTemplate>
                        <%# (Container.ItemIndex + 1) + ((dgProduct.CurrentPageIndex) * dgProduct.PageSize)%>
                <input type="hidden" id="hidPackageID" runat="server" value='<%# Eval("PackageID")%>' />
                        <input type="hidden" id="hidProductCode" runat="server" value='<%# Eval("ProductCode")%>' />
                    </ItemTemplate>
                    <ItemStyle Width="15px" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Product Code">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true"
                            CausesValidation="false"><span><%# Eval("ProductCode")%></span></asp:LinkButton>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div class="input-group">
                             <asp:TextBox ID="txtNewProductCode" runat="server" Columns="10" MaxLength="15" CssClass="form-control input-sm"
                                onfocus="select();" onchange="this.value = this.value.toUpperCase()" OnTextChanged="txtNewProductCode_OnTextChanged"
                                AutoPostBack="true" />
                            <span class="input-group-btn">
                                <asp:LinkButton ID="lnkFindProduct" runat="server" CommandName="FindProduct" EnableViewState="true"
                            CssClass="btn btn-primary right-rounded-border" OnClientClick="return SearchProduct(this.parentElement.all(0));"><i class="ti-search"></i></asp:LinkButton>
                            </span>
                        </div>
                       
                        <asp:RequiredFieldValidator ID="rfvNewProductCode" runat="server" ControlToValidate="txtNewProductCode"
                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                        <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx" ID="autoComplete1"
                            TargetControlID="txtNewProductCode" ServicePath="AutoCompletionProduct.asmx"
                            ServiceMethod="GetCompletionListByName" MinimumPrefixLength="1" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=",">
                        </ajaxToolkit:AutoCompleteExtender>
                    </FooterTemplate>

                    <HeaderStyle Wrap="False" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Product Description">
                    <ItemTemplate>
                        <%# Eval("ProductDescription")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditProductDescription" runat="server" Columns="50" MaxLength="80" CssClass="form-control input-sm"
                            Text='<%# Eval("ProductDescription") %>' />
                        <asp:RequiredFieldValidator ID="rfvEditProductDescription" runat="server" ControlToValidate="txtEditProductDescription"
                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtNewProductDescription" runat="server" Columns="50" MaxLength="80" CssClass="form-control input-sm"/>
                        <asp:RequiredFieldValidator ID="rfvNewProductDescription" runat="server" ControlToValidate="txtNewProductDescription"
                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                    </FooterTemplate>

                    <HeaderStyle Wrap="False" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Qty">
                    <ItemTemplate>
                        <%# Eval("Qty", "{0:f2}")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox CssClass="form-control input-sm" ID="txtEditQty" runat="server" Columns="5"
                            MaxLength="10" Text='<%# Eval("Qty", "{0:f2}") %>' />
                        <asp:RequiredFieldValidator ID="rfvEditQty" runat="server" ControlToValidate="txtEditQty"
                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:RangeValidator
                                ID="rgEditQty" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditQty"
                                Type="Double" ValidationGroup="valGrpEditRow" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox CssClass="form-control input-sm" ID="txtNewQty" runat="server" Columns="5" MaxLength="10" />
                        <asp:RequiredFieldValidator ID="rfvNewQty" runat="server" ControlToValidate="txtNewQty"
                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:RangeValidator
                                ID="rgNewQty" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewQty"
                                Type="Double" ValidationGroup="valGrpNewRow" />
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                            CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                            ValidationGroup="valGrpEditRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i> </asp:LinkButton>
                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                            CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i> </asp:LinkButton>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                            ValidationGroup="valGrpNewRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i> </asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateColumn>
            </Columns>
            <HeaderStyle CssClass="tHeader" />
            <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
        </asp:DataGrid>
                    </div>
                </div>
        </div>     
        <div class="TABCOMMAND">
            <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
