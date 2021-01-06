<%@ Page Language="C#" AutoEventWireup="true" Codebehind="EditQuotationPackageDetail.aspx.cs"
    Inherits="GMSWeb.Sales.Sales.EditQuotationPackageDetail" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<body onbeforeunload="if (document.all) window.opener.location.href = window.opener.location.href; else window.opener.location.reload();"
    style="background: none">
    <form id="form1" runat="server" style="text-align: left">
        <asp:ScriptManager ID="scriptMgr" runat="server" />
        <p>
            <table>
                <tr>
                    <td>
                        <div style="font-weight: bolder">
                            Package Info :
                        </div>
                        <br />
                    </td>
                </tr>
            </table>
            <table class="tTable1" cellspacing="5" cellpadding="5" border="0" width="100%">
                <tr>
                    <td class="tbLabel" style="width: 45%">
                        Package Product Code</td>
                    <td>
                        :</td>
                    <td>
                        <asp:Label runat="server" ID="lblPackageProductCode"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Package Product Description</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:Label runat="server" ID="lblPackageProductDescription"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Unit Price</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:TextBox CssClass="textbox" ID="txtEditUnitPrice" runat="server" Columns="10"
                            MaxLength="10" OnTextChanged="txtEditUnitPrice_OnTextChanged" AutoPostBack="true" />
                        <asp:RequiredFieldValidator ID="rfvEditUnitPrice" runat="server" ControlToValidate="txtEditUnitPrice"
                            ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditPackageRow" /><asp:RangeValidator
                                ID="rgEditUnitPrice" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditUnitPrice"
                                Type="Double" ValidationGroup="valGrpEditPackageRow" />
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Unit Package Price</td>
                    <td style="width: 5%;">
                        :</td>
                    <td style="color: Red">
                        <asp:Label runat="server" ID="lblUnitPackagePrice"></asp:Label>
                        <asp:HiddenField runat="server" ID="hidCurrencyRate" />
                        <asp:HiddenField runat="server" ID="hidAccountCode" />
                    </td>
                </tr>
            </table>
        </p>
        <p>
            <table>
                <tr>
                    <td>
                        <div style="font-weight: bolder">
                            Package Product Listing :
                        </div>
                        <br />
                        <div class="tTable">
                            <asp:DataGrid ID="dgProduct" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                GridLines="None" CellPadding="5" OnUpdateCommand="dgProduct_UpdateCommand" OnItemCommand="dgProduct_CreateCommand"
                                OnDeleteCommand="dgProduct_DeleteCommand" OnItemDataBound="dgProduct_ItemDataBound"
                                CellSpacing="5" CssClass="tTable tBorder" AllowPaging="false">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((dgProduct.CurrentPageIndex) * dgProduct.PageSize)%>
                                            .
                                            <input type="hidden" id="hidProductCode" runat="server" value='<%# Eval("ProductCode")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Prod Code">
                                        <ItemTemplate>
                                            <%# Eval("ProductCode")%>
                                            <asp:Image ID="imgMagnify" runat="server" ImageUrl="../../images/icons/box_closed.png" />
                                            <ajaxToolkit:PopupControlExtender id="PopupControlExtender1" runat="server" popupcontrolid="Panel1"
                                                targetcontrolid="imgMagnify" dynamiccontextkey='<%# Eval("CoyID").ToString() + ";" + Eval("ProductCode").ToString() %>' dynamiccontrolid="Panel1"
                                                dynamicservicemethod="GetDynamicContent" position="Right">
                                            </ajaxToolkit:PopupControlExtender>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewProductCode" runat="server" Columns="12" MaxLength="15" CssClass="textbox"
                                                onfocus="select();" onchange="this.value = this.value.toUpperCase()" OnTextChanged="txtNewProductCode_OnTextChanged"
                                                AutoPostBack="true" />
                                            <asp:RequiredFieldValidator ID="rfvNewProductCode" runat="server" ControlToValidate="txtNewProductCode"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                            <asp:LinkButton ID="lnkFindProduct" runat="server" CommandName="FindProduct" EnableViewState="true"
                                                CssClass="FindButt" OnClientClick="return SearchProduct(this);"><IMG height="16" src="../../images/icons/FindItem.gif" align="absMiddle"></asp:LinkButton>
                                        </FooterTemplate>
                                        <ItemStyle Width="50px" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Product Description">
                                        <ItemTemplate>
                                            <%# Eval("ProductDescription")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditProductDescription" runat="server" Columns="20" MaxLength="80"
                                                Text='<%# Eval("ProductDescription") %>' CssClass="textbox" />
                                            <asp:RequiredFieldValidator ID="rfvEditProductDescription" runat="server" ControlToValidate="txtEditProductDescription"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewProductDescription" runat="server" Columns="20" MaxLength="80"
                                                CssClass="textbox" />
                                            <asp:RequiredFieldValidator ID="rfvNewProductDescription" runat="server" ControlToValidate="txtNewProductDescription"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                        </FooterTemplate>
                                        <ItemStyle Width="250px" />
                                        <HeaderStyle Wrap="False" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Unit Qty">
                                        <ItemTemplate>
                                            <asp:TextBox CssClass="textbox" ID="txtEditQty" runat="server" Columns="5" MaxLength="10"
                                                Text='<%# Eval("Qty", "{0:f2}") %>' />
                                            <asp:RequiredFieldValidator ID="rfvEditQty" runat="server" ControlToValidate="txtEditQty"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:RangeValidator
                                                    ID="rgEditQty" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditQty"
                                                    Type="Double" ValidationGroup="valGrpEditRow" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox CssClass="textbox" ID="txtNewQty" runat="server" Columns="5" MaxLength="10" />
                                            <asp:RequiredFieldValidator ID="rfvNewQty" runat="server" ControlToValidate="txtNewQty"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:RangeValidator
                                                    ID="rgNewQty" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewQty"
                                                    Type="Double" ValidationGroup="valGrpNewRow" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Unit Price">
                                        <ItemTemplate>
                                            <asp:TextBox CssClass="textbox" ID="txtEditUnitPrice" runat="server" Columns="5"
                                                MaxLength="10" Text='<%# Eval("UnitPrice", "{0:f2}") %>' />
                                            <asp:RequiredFieldValidator ID="rfvEditUnitPrice" runat="server" ControlToValidate="txtEditUnitPrice"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:RangeValidator
                                                    ID="rgEditUnitPrice" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditUnitPrice"
                                                    Type="Double" ValidationGroup="valGrpEditRow" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox CssClass="textbox" ID="txtNewUnitPrice" runat="server" Columns="5" MaxLength="10" />
                                            <asp:RequiredFieldValidator ID="rfvNewUnitPrice" runat="server" ControlToValidate="txtNewUnitPrice"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:RangeValidator
                                                    ID="rgNewUnitPrice" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewUnitPrice"
                                                    Type="Double" ValidationGroup="valGrpNewRow" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                                CssClass="SaveButt"><span>Save</span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                                CausesValidation="false" CssClass="DeleteButt"><span>Delete</span></asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                                ValidationGroup="valGrpNewRow" CssClass="NewButt"><span>Add</span></asp:LinkButton>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <HeaderStyle CssClass="tHeader" />
                                <AlternatingItemStyle CssClass="tAltRow" />
                                <FooterStyle CssClass="tFooter" />
                                <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                            </asp:DataGrid>
                        </div>
                    </td>
                </tr>
            </table>
        </p>
        <p>
            <table>
            <tr><td>
             <div style="font-weight: bolder">
                            For Even Cost Distribution :
                        </div>
            </td>         
            </tr>
            <tr>
            <td>Key in the Proposed Package Price and system will distribute cost evenly for all products in this package. </td>
            </tr>
            </table><br />
            <table class="tTable1" cellspacing="5" cellpadding="5" border="0" width="100%">
            <tr>
                    <td class="tbLabel" style="width: 40%; color:#0039A6;">
                        Proposed Package Price:&nbsp;</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtTotalPackagePrice" MaxLength="20" Columns="20"
                            onfocus="select();" CssClass="textbox"></asp:TextBox>&nbsp;&nbsp;</td>
                    <td>
                        <asp:Button ID="btnGenerate" Text="Generate Now" EnableViewState="False" runat="server"
                            CssClass="button" OnClick="btnGenerate_Click"></asp:Button></td>
                </tr>
            </table>
        </p>
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
        <asp:Panel ID="Panel1" runat="server"> </asp:Panel>
    </form>
</body>
