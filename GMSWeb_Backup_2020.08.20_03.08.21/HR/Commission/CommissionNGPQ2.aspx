<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="CommissionNGPQ2.aspx.cs" Inherits="GMSWeb.Sales.Commission.CommissionNGPQ" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<asp:ScriptManager ID="sriptmgr1" runat="server">
        </asp:ScriptManager>
        <div id="GroupContentBar">
            <h3>
                Commission &gt; Commission Rate & GPQ</h3>
            Master record of commission rate and GPQ.
            <br />
            <br />
            <div style="display:none">
            <asp:TextBox ID="txtBoxID" runat="server" Columns="100" MaxLength="100" Text="" />
            <asp:TextBox ID="txtHiddenID" runat="server" Columns="100" MaxLength="100" Text="" />
            <asp:TextBox ID="txtEditBoxID" runat="server" Columns="100" MaxLength="100" Text="" />
            <asp:TextBox ID="txtEditHiddenID" runat="server" Columns="100" MaxLength="100" Text="" />
            </div>
            <table class="tGroupTable" style="width: 100%">
                <tr>
                    <td>
                        <br />
                        <div id="Div1" style="text-align: left; width: 1000px" runat="server">
                            <asp:Label ID="lblSearchSummary" Visible="false" runat="server"></asp:Label>
                        </div>
                        <br />
                        <div class="tGroupTable">
                            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                DataKeyField="SalesPersonMasterID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
                                OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" GridLines="none"
                                OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
                                CellPadding="5" CellSpacing="0" CssClass="tTable tBorder" AllowPaging="true"
                                PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                            <input type="hidden" id="hidSalesPersonMasterID" runat="server" value='<%# Eval("SalesPersonMasterID")%>' />
                                            .</ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Salesman Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="120px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalesPersonMasterName" runat="server">
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                                    CausesValidation="false"><span><%# Eval( "SalesPersonMasterName" )%></span></asp:LinkButton>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditSalesPersonMasterName" runat="server" Columns="20" MaxLength="50" onchange="this.value = this.value.toUpperCase()" Text='<%# Eval( "SalesPersonMasterName" )%>' /><asp:RequiredFieldValidator
                                                ID="rfvEditSalesPersonMasterName" runat="server" ControlToValidate="txtEditSalesPersonMasterName"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewSalesPersonMasterName" runat="server" Columns="20" MaxLength="50" onchange="this.value = this.value.toUpperCase()" /><asp:RequiredFieldValidator
                                                ID="rfvNewSalesPersonMasterName" runat="server" ControlToValidate="txtNewSalesPersonMasterName"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Accounts List" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="320px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccountList" runat="server">
                                                <%# Eval( "AccountList" )%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditAccountList" runat="server" Columns="55" MaxLength="100" ReadOnly="true" Text='<%# Eval( "AccountList" )%>'/><asp:ImageButton ID="lnkEditAccount"
                                                    runat="server" ImageUrl="~/images/Icons/ModifyItem.gif" OnClientClick="mode = 'edit'; selectAccounts(this)" />
                                                    <input type="hidden" id="hidEditAccountList" runat="server" value='<%# Eval( "AccountList" )%>' />
                                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lnkEditAccount"
                                                PopupControlID="PNL" OkControlID="ButtonOK" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewAccountList" runat="server" Columns="55" MaxLength="100" ReadOnly="true"/><asp:ImageButton ID="lnkAddAccount"
                                                    runat="server" ImageUrl="../../images/Icons/ModifyItem.gif" OnClientClick="mode = 'add';selectAccounts(this)" />
                                                    <input type="hidden" id="hidAccountList" runat="server" value="" />
                                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lnkAddAccount"
                                                PopupControlID="PNL" OkControlID="ButtonOK" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="GPQ" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false"
                                        ItemStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGPQ" runat="server">
										           <%# Eval( "GPQ" )%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditGPQ" runat="server" Columns="10" MaxLength="10" Text='<%# Eval("GPQ") %>' /><asp:RequiredFieldValidator
                                                ID="rfvEditGPQ" runat="server" ControlToValidate="txtEditGPQ" ErrorMessage="*"
                                                Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEditGPQ"
                                                    runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtEditGPQ"
                                                    Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewGPQ" runat="server" Columns="10" MaxLength="10" /><asp:RequiredFieldValidator
                                                ID="rfvNewGPQ" runat="server" ControlToValidate="txtNewGPQ" ErrorMessage="*"
                                                Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewGPQ"
                                                    runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtNewGPQ"
                                                    Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Comm Rate" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false"
                                        ItemStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCommRate" runat="server">
										           <%# Eval( "CommissionRate" )%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditCommissionRate" runat="server" Columns="10" MaxLength="10"
                                                Text='<%# Eval("CommissionRate") %>' /><asp:RequiredFieldValidator ID="rfvEditCommissionRate"
                                                    runat="server" ControlToValidate="txtEditCommissionRate" ErrorMessage="*" Display="dynamic"
                                                    ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEditCommissionRate"
                                                        runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtEditCommissionRate"
                                                        Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewCommissionRate" runat="server" Columns="10" MaxLength="10" /><asp:RequiredFieldValidator
                                                ID="rfvNewCommissionRate" runat="server" ControlToValidate="txtNewCommissionRate"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator
                                                    ID="cvNewCommissionRate" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                    ControlToValidate="txtNewCommissionRate" Type="Double" Operator="DataTypeCheck"
                                                    ValidationGroup="valGrpNewRow" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                                CausesValidation="false" CssClass="DeleteButt"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                                ValidationGroup="valGrpEditRow" CssClass="SaveButt"><span>Save</span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                                CausesValidation="false" CssClass="CancelButt"><span>Cancel</span></asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                                ValidationGroup="valGrpNewRow" CssClass="NewButt"><span>Add</span></asp:LinkButton>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <HeaderStyle CssClass="tGroupHeader" />
                                <AlternatingItemStyle CssClass="tGroupAltRow" />
                                <FooterStyle CssClass="tGroupFooter" />
                                <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                            </asp:DataGrid>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Panel ID="PNL" runat="server" Style="display: none; width: 500px; background-color: White;
                border-width: 2px; border-color: Black; border-style: solid; padding: 10px;">
                <div style="text-align: center;">
                    <table>
                        <tr>
                            <td style="width: 296px">
                                <b>Select Accounts
                                    <br />
                                    (Press CTRL to select multiple accounts) </b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 296px">
                                <asp:ListBox ID="lsbAccounts" runat="server" SelectionMode="Multiple" Rows="30" Width="334px"
                                    Font-Size="XX-Small"></asp:ListBox>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="ButtonOk" runat="server" Text="Save" OnClientClick="FillData()" />
                    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" />
                </div>
            </asp:Panel>
            <div class="TABCOMMAND">
                <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <ul>
                            <li>&nbsp;</li>
                        </ul>
                        <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
</asp:Content>
