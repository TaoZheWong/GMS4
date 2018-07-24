<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Old Organizer.aspx.cs" Inherits="GMSWeb.HR.Training.Organizer" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Training - Organizer</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="ContentBar">
            <h3>
                Training &gt; Organizer</h3>
            List of training course organizers.
            <br />
            <br />
            <asp:ScriptManager ID="scriptMgr" runat="server" />
            <asp:UpdatePanel ID="udpOrganizerUpdater" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="tTable" style="border-collapse: collapse" cellspacing="0" cellpadding="1"
                        border="1" width="450px">
                        <tr>
                            <td class="tbLabel">
                                Organizer Name</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="searchOrganizerName" MaxLength="50" Columns="50"
                                    onfocus="select();" CssClass="textbox"></asp:TextBox></td>
                            <td style="width: 10%">
                                <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="button"
                                    OnClick="btnSearch_Click"></asp:Button></td>
                        </tr>
                    </table>
                    <br />
                    <table class="tTable" style="width: 100%">
                        <tr>
                            <td>
                                <br />
                                <div id="Div1" style="text-align: left; width: 850px" runat="server">
                                    <asp:Label ID="lblSearchSummary" Visible="false" runat="server"></asp:Label>
                                </div>
                                <br />
                                <div class="tTable">
                                    <asp:DataGrid ID="dgOrganizer" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                        DataKeyField="OrganizerName" OnCancelCommand="dgOrganizer_CancelCommand" OnEditCommand="dgOrganizer_EditCommand"
                                        OnUpdateCommand="dgOrganizer_UpdateCommand" OnItemCommand="dgOrganizer_CreateCommand"
                                        GridLines="none" OnItemDataBound="dgOrganizer_ItemDataBound" OnDeleteCommand="dgOrganizer_DeleteCommand"
                                        CellPadding="5" CellSpacing="0" CssClass="tTable tBorder" AllowPaging="true"
                                        PageSize="20" OnPageIndexChanged="dgOrganizer_PageIndexChanged">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgOrganizer.CurrentPageIndex) * dgOrganizer.PageSize)  %>
                                                    <input type="hidden" id="hidID" runat="server" value='<%# Eval("OrganizerID")%>' />
                                                    .</ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Organizer Name" HeaderStyle-Wrap="false" ItemStyle-Width="280px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server">
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                                            CausesValidation="false"><span><%# Eval( "OrganizerName" )%></span></asp:LinkButton>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditName" runat="server" Columns="50" MaxLength="80" Text='<%# Eval("OrganizerName") %>' />
                                                    <asp:RequiredFieldValidator ID="rfvEditName" runat="server" ControlToValidate="txtEditName"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNewName" runat="server" Columns="50" MaxLength="80" />
                                                    <asp:RequiredFieldValidator ID="rfvNewName" runat="server" ControlToValidate="txtNewName"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
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
                                        <HeaderStyle CssClass="tHeader" />
                                        <AlternatingItemStyle CssClass="tAltRow" />
                                        <FooterStyle CssClass="tFooter" />
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
