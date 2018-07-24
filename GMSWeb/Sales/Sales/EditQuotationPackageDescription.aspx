<%@ Page Language="C#" AutoEventWireup="true" Codebehind="EditQuotationPackageDescription.aspx.cs"
    Inherits="GMSWeb.Sales.Sales.EditQuotationPackageDescription" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<body style="background: none">
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
            <table class="tTable1" cellspacing="5" cellpadding="5" border="0">
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
            </table>
        </p>
        <p>
            <table>
                <tr>
                    <td>
                        <div style="font-weight: bolder">
                            Package Description Listing :
                        </div><br />
                        <i>Characters typed :
                            <asp:TextBox runat="server" ID="txtCounter" CssClass="textbox" ReadOnly="true" Columns="3"></asp:TextBox>
                            (Limit:80)</i><br /><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="tTable">
                            <asp:DataGrid ID="dgDetail" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                GridLines="None" CellPadding="5" OnItemCommand="dgDetail_ItemCommand" OnEditCommand="dgDetail_EditCommand"
                                OnCancelCommand="dgDetail_CancelCommand" OnUpdateCommand="dgDetail_UpdateCommand"
                                OnDeleteCommand="dgDetail_DeleteCommand" CellSpacing="5" CssClass="tTable tBorder">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="No" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#0039A6">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((dgDetail.CurrentPageIndex) * dgDetail.PageSize)%>
                                            .
                                            <input type="hidden" id="hidDetailID" runat="server" value='<%# Eval("DetailID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Package Description">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true"
                                                CausesValidation="false"><span><%# Eval("Description")%></span></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditProductDescription" runat="server" Columns="50" MaxLength="80"
                                                Text='<%# Eval("Description") %>' CssClass="textbox" />
                                            <asp:RequiredFieldValidator ID="rfvEditProductDescription" runat="server" ControlToValidate="txtEditProductDescription"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valDetailEditRow" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewProductDescription" runat="server" Columns="50" MaxLength="80"
                                                CssClass="textbox" onkeyup="update(this);" />
                                            <asp:RequiredFieldValidator ID="rfvNewProductDescription" runat="server" ControlToValidate="txtNewProductDescription"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valDetailNewRow" />
                                        </FooterTemplate>
                                        <HeaderStyle Wrap="False" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Order">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkGoUp" runat="server" CommandName="GoUp" EnableViewState="true"><IMG height="16" src="../../images/icons/UpArrow.png" align="absMiddle"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkGoDown" runat="server" CommandName="GoDown" EnableViewState="true"><IMG height="16" src="../../images/icons/DownArrow.png" align="absMiddle"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle />
                                        <HeaderStyle Wrap="False" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                                CausesValidation="false" CssClass="DeleteButt"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                                ValidationGroup="valDetailEditRow" CssClass="SaveButt"><span>Save</span></asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                                CausesValidation="false" CssClass="CancelButt"><span>Cancel</span></asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                                ValidationGroup="valDetailNewRow" CssClass="NewButt"><span>Add</span></asp:LinkButton>
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
    </form>
</body>
