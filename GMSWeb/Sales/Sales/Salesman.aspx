<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true"
    CodeBehind="Salesman.aspx.cs" Inherits="GMSWeb.Sales.Sales.Salesman" Title="Salesmen" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">Administration</a></li>
        <li class="active">Salesman Detail</li>
    </ul>
    <h1 class="page-header">Salesman Detail <br /><small>Salesperson can update his/her contact information, which will be used in the quotation printout for customer. </small></h1>

    <asp:ScriptManager ID="scriptMgr" runat="server" />
    <asp:UpdatePanel ID="udpMappingUpdater" runat="server" UpdateMode="conditional">
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
                <div class="table-responsive">
                    <asp:DataGrid ID="dgResult" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                        DataKeyField="SalesPersonID" OnPageIndexChanged="dgResult_PageIndexChanged" GridLines="None"
                        OnEditCommand="dgResult_EditCommand" CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover"
                        OnCancelCommand="dgResult_CancelCommand" OnUpdateCommand="dgResult_UpdateCommand"
                        AllowPaging="False">
                        <Columns>
                            <asp:TemplateColumn HeaderText="No" HeaderStyle-Width="1%">
                                <ItemTemplate>
                                    <%# (Container.ItemIndex + 1) + ((dgResult.CurrentPageIndex) * dgResult.PageSize)%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Salesman" HeaderStyle-Width="29%">
                                <ItemTemplate>
                                    <asp:Label ID="lblSalesPersonID" runat="server">
                                        <%# Eval( "SalesPersonID")  + " - " + Eval( "SalesPersonName")%>
                                        <input type="hidden" id="hidSalesPersonID" runat="server" value='<%# Eval("SalesPersonID")%>' />
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Designation" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <%# Eval("Designation")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtEditDesignation" runat="server" Columns="20"
                                        MaxLength="50" Text='<%# Eval("Designation") %>' />
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Mobile Phone" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <%# Eval("MobilePhone")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtEditMobilePhone" runat="server" Columns="20" MaxLength="50"
                                        Text='<%# Eval("MobilePhone") %>' />
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="DID" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <%# Eval("DID")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtEditDID" runat="server" Columns="10" MaxLength="20"
                                        Text='<%# Eval("DID") %>' />
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Fax">
                                <ItemTemplate>
                                    <%# Eval("Fax")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtEditFax" runat="server" Columns="10" MaxLength="20"
                                        Text='<%# Eval("Fax") %>' />
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Email" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <span class="break_long_word"><%# Eval("Email")%></span>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtEditEmail" runat="server" Columns="20" MaxLength="60"
                                        Text='<%# Eval("Email") %>' />
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Function"  HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                        CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Edit"><i class="ti-pencil"></i></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                        ValidationGroup="valGrpEditRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                        CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                                </EditItemTemplate>

                                <FooterStyle HorizontalAlign="Center" />
                                <HeaderStyle />
                            </asp:TemplateColumn>
                        </Columns>
                        <HeaderStyle CssClass="tHeader" />
                        <AlternatingItemStyle CssClass="tAltRow" />
                        <FooterStyle CssClass="tFooter" />
                        <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                    </asp:DataGrid>
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
            $(".sub-salesman-detail").addClass("active");
        });
    </script>
</asp:Content>
