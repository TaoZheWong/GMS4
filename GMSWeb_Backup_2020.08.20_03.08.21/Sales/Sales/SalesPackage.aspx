<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true"
    Codebehind="SalesPackage.aspx.cs" Inherits="GMSWeb.Sales.Sales.SalesPackage"
    Title="Sales - Package" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">Administration</a></li>
        <li class="active">Sales Package</li>
    </ul>
    <h1 class="page-header">Sales Package <br /><small>Product Manager can key in the sales packages, which can be selected by salesperson in the quotation. </small></h1>

    <asp:ScriptManager ID="scriptMgr" runat="server" />
    <asp:UpdatePanel ID="udpPackageUpdater" runat="server" UpdateMode="conditional">
        <ContentTemplate>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-align-justify"></i> 
                        <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive">   
                            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                OnPageIndexChanged="dgData_PageIndexChanged" GridLines="None" CellPadding="5"
                                CellSpacing="5" CssClass="table table-condensed table-striped table-hover" AllowPaging="True" PageSize="20"
                                OnItemCommand="dgData_CreateCommand" OnEditCommand="dgData_EditCommand"
                                OnDeleteCommand="dgData_DeleteCommand" OnItemDataBound="dgData_ItemDataBound"
                                OnCancelCommand="dgData_CancelCommand" OnUpdateCommand="dgData_UpdateCommand">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="No">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)%>
                                            <input type="hidden" id="hidPackageID" runat="server" value='<%# Eval("PackageID")%>' />
                                            <input type="hidden" id="hidProductCode" runat="server" value='<%# Eval("ProductCode")%>' />
                                            </ItemTemplate>
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
                                                        CssClass="btn btn-primary btn-sm right-rounded-border" OnClientClick="return SearchProduct(this.parentElement.all(0));">
                                                        <i class="ti-search"></i>
                                                    </asp:LinkButton>
                                                </span>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvNewProductCode" runat="server" ControlToValidate="txtNewProductCode"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
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
                                    <asp:TemplateColumn HeaderText="UOM" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <%# Eval("UOM")%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlEditUOM" CssClass="form-control input-sm">
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlNewUOM" CssClass="form-control input-sm">
                                                    </asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                        <ItemTemplate>
                                             <asp:LinkButton ID="lnkEditDetail" runat="server" EnableViewState="true"
                                                OnClientClick='<%# "viewDetail("+Eval("PackageID") + ");"%>' CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="View Detail"><i class="ti-search"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                                CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                                ValidationGroup="valGrpEditRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                                CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                                ValidationGroup="valGrpNewRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <HeaderStyle CssClass="tHeader" />
                                <PagerStyle Mode="NumericPages" CssClass="grid_pagination"/>
                            </asp:DataGrid>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="TABCOMMAND">
        <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".administration-menu").addClass("active expand");
            $(".sub-sales-package").addClass("active");
        });
    </script>
</asp:Content>