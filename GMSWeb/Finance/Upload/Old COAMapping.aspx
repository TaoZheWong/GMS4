<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Old COAMapping.aspx.cs" Inherits="GMSWeb.Finance.Upload.COAMapping" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Data - COA Mapping</title>

    <script language="javascript" type="text/javascript" src="/GMS/scripts/popcalendar.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="GroupContentBar">
            <h3>
                Data &gt; COA Mapping</h3>
            For Users To Manage Chart Of Accounts Mapping
            <br />
            <br />
            <asp:ScriptManager ID="scriptMgr" runat="server" />
            <asp:UpdatePanel ID="udpBankUpdater" runat="server" UpdateMode="conditional">
                <ContentTemplate>
                    <table class="tTable" style="border-collapse: collapse" cellspacing="0" cellpadding="1"
                        border="1" width="500px">
                        <tr>
                            <td class="tbLabel">
                                Old COA</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="searchOldCOA" MaxLength="50" Columns="50" onfocus="select();"
                                    CssClass="textbox"></asp:TextBox></td>
                            <td style="width: 10%">
                                <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="button"
                                    OnClick="btnSearch_Click"></asp:Button></td>
                        </tr>
                    </table>
                    <table class="tGroupTable" >
                        <tr>
                            <td>
                                <br />
                                <div id="Div1" style="text-align: left;" runat="server">
                                    <asp:Label ID="lblSearchSummary" Visible="false" runat="server"></asp:Label>
                                </div>
                                <br />
                                <div class="tGroupTable">
                                    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                        CellPadding="5" CellSpacing="0" CssClass="tTable tBorder" OnItemCommand="dgData_CreateCommand"
                                        AllowPaging="true" PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged"
                                        OnEditCommand="dgData_EditCommand" OnCancelCommand="dgData_CancelCommand"
                                        OnUpdateCommand="dgData_UpdateCommand"
                                        OnDeleteCommand="dgData_DeleteCommand" OnItemDataBound="dgData_ItemDataBound" 
                                        EnableViewState="true" Width="600px">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                                    .</ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Old COA" HeaderStyle-Wrap="false" ItemStyle-Width="250px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOldCOAID" runat="server"><asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                                            CausesValidation="false"><span>
                                                        <%# Eval("OldCOAID")%>
                                                    </span></asp:LinkButton>
                                                        <input type="hidden" id="hidOldCOAID" runat="server" value='<%# Eval("OldCOAID")%>' />
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNewOldCOAID" runat="server" Columns="30" MaxLength="50" />
                                                    <asp:RequiredFieldValidator ID="rfvNewOldCOAID" runat="server" ControlToValidate="txtNewOldCOAID"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="New COA" HeaderStyle-Wrap="false" ItemStyle-Width="250px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNewCOAID" runat="server"><span>
                                                        <%# Eval("NewCOAID")%>
                                                    </span>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditNewCOAID" runat="server" Columns="30" MaxLength="50"  Text='<%# Eval("NewCOAID")%>'/>
                                                    <asp:RequiredFieldValidator ID="rfvEditNewCOAID" runat="server" ControlToValidate="txtEditNewCOAID"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNewNewCOAID" runat="server" Columns="30" MaxLength="50" />
                                                    <asp:RequiredFieldValidator ID="rfvNewNewCOAID" runat="server" ControlToValidate="txtNewNewCOAID"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center"
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
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
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
    </form>
</body>
</html>
