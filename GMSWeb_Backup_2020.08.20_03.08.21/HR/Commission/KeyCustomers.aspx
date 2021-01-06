<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true"
    Codebehind="KeyCustomers.aspx.cs" Inherits="GMSWeb.HR.Commission.KeyCustomers"
    Title="HR - Key Customers" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <asp:ScriptManager ID="sriptmgr1" runat="server">
        <Services>
            <asp:ServiceReference Path="AutoCompletionAccount.asmx" />
        </Services>
    </asp:ScriptManager>
        <ul class="breadcrumb pull-right">
        <li><a href="#">Incentive</a></li>
        <li class="active">Key Customers</li>
    </ul>
    <h1 class="page-header">Key Customers 
     <br />    
    <small>List of key customers by salesman.</small></h1>
        <div class="panel panel-primary">
            <div class="panel-heading">
                <div class="panel-heading-btn">
                    <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                </div>
                <h4 class="panel-title">
                    <i class="ti-align-justify"></i> <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                </h4>
            </div>
            <div class="panel-body no-padding">
                <div class="table-responsive">                
                    <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="udpBankUtilUpdater">
                                <ContentTemplate>
                    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                        OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
                        OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" GridLines="none"
                        OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
                        CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                        PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                        <Columns>
                            <asp:TemplateColumn HeaderText="No">
                                <ItemTemplate>
                                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                    <input type="hidden" id="hidSalesPersonMasterID" runat="server" value='<%# Eval("SalesPersonMasterID")%>' />
                                    <input type="hidden" id="hidAccountCode" runat="server" value='<%# Eval("AccountCode")%>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Customer">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                                CausesValidation="false"><%# Eval("AccountObject.AccountCode").ToString() + " " + Eval("AccountObject.AccountName").ToString()%></asp:LinkButton>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNewAccountCode" runat="server" Columns="30" MaxLength="60" CssClass="form-control input-sm"
                                        onfocus="select();" onchange="this.value = this.value.toUpperCase()" />
                                    <asp:RequiredFieldValidator ID="rfvNewAccountCode" runat="server" ControlToValidate="txtNewAccountCode"
                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                    <ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx" ID="autoComplete1"
                                        TargetControlID="txtNewAccountCode" ServicePath="AutoCompletionAccount.asmx"
                                        ServiceMethod="GetCompletionListByName" MinimumPrefixLength="1" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" DelimiterCharacters=",">
                                    </ajaxToolkit:AutoCompleteExtender>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Salesman" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server">
                                                        <%# Eval("SalesPersonMasterObject.SalesPersonMasterName")%>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList CssClass="form-control input-sm" ID="ddlEditSalesPersonMasterName" runat="Server"
                                            DataTextField="SalesPersonMasterName" DataValueField="SalesPersonMasterID" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList CssClass="form-control input-sm" ID="ddlNewSalesPersonMasterName" runat="Server"
                                            DataTextField="SalesPersonMasterName" DataValueField="SalesPersonMasterID" />
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Average GP">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server">
												   <%# Eval("AverageGP", "{0:f2}")%>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtEditAverageGP" runat="server" Columns="15" MaxLength="15"
                                        Text='<%# Eval("AverageGP", "{0:f2}") %>' />
                                    <asp:RequiredFieldValidator ID="rfvEditAverageGP" runat="server" ControlToValidate="txtEditAverageGP"
                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:RangeValidator
                                            ID="rgEditAverageGP" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditAverageGP"
                                            Type="Double" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtNewAverageGP" runat="server" Columns="15" MaxLength="15" />
                                    <asp:RequiredFieldValidator ID="rfvNewAverageGP" runat="server" ControlToValidate="txtNewAverageGP"
                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:RangeValidator
                                            ID="rgNewAverageGP" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewAverageGP"
                                            Type="Double" ValidationGroup="valGrpNewRow" />
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                        CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete">
                                <i class="ti-trash"></i> </asp:LinkButton>
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
                        <HeaderStyle CssClass="tHeader"/>
                        <AlternatingItemStyle CssClass="tAltRow" />
                        <FooterStyle CssClass="tFooter" />
                        <PagerStyle Mode="NumericPages" CssClass="grid_pagination"/>
                    </asp:DataGrid>
                    </ContentTemplate></asp:UpdatePanel>
                </div>
            </div>
        </div>
    <br />
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
            $(".incentive-menu").addClass("active expand");
            $(".sub-key-customer").addClass("active");
        });
    </script>
</asp:Content>
