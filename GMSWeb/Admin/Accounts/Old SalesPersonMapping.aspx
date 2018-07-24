<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Old SalesPersonMapping.aspx.cs"
    Inherits="GMSWeb.Admin.Accounts.SalesPersonMapping" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Accounts - Sales Person Mapping</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="ContentBar">
            <h3>
                Accounts &gt; Sales Person Mapping</h3>
            Maintein a list of mappings from A21 accounts to GMS accounts .
            <br />
            <br />
            <table class="tTable" style="border-collapse: collapse" cellspacing="0" cellpadding="1"
                border="1" width="80%">
                <tr>
                    <td class="tbLabel">
                        Username</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlUsername" runat="Server" DataTextField="UserName" DataValueField="Id"
                            CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlUsername_SelectedIndexChanged" /></td>
                </tr>
                <tr runat="server" id="groupRow" visible="false">
                    <td class="tbLabel">
                        Group Management User</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:RadioButton ID="rbIsGroupManagementUser" runat="server" Text="Yes" GroupName="GroupManagementUser"
                            OnCheckedChanged="rbIsGroupManagementUser_CheckedChanged" AutoPostBack="true" />
                        <asp:RadioButton ID="rbIsNotGroupManagementUser" runat="server" Text="No" GroupName="GroupManagementUser"
                            OnCheckedChanged="rbIsGroupManagementUser_CheckedChanged" AutoPostBack="true" />
                    </td>
                </tr>
                <tr runat="server" id="companyRow" visible="false">
                    <td class="tbLabel">
                        Company</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlCompany" runat="Server" DataTextField="Name" DataValueField="Id"
                            CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" /></td>
                </tr>
                <tr runat="server" id="companyManagementRow" visible="false">
                    <td class="tbLabel">
                        Company Management User</td>
                    <td style="width: 5%">
                        :</td>
                    <td>
                        <asp:RadioButton ID="rbIsCompanyManagementUser" runat="server" Text="Yes" GroupName="CompanyManagementUser"
                            OnCheckedChanged="rbIsCompanyManagementUser_CheckedChanged" AutoPostBack="true" />
                        <asp:RadioButton ID="rbIsNotCompanyManagementUser" runat="server" Text="No" GroupName="CompanyManagementUser"
                            OnCheckedChanged="rbIsCompanyManagementUser_CheckedChanged" AutoPostBack="true" />
                    </td>
                </tr>
            </table>
            <asp:ScriptManager ID="scriptMgr" runat="server" />
            <asp:UpdatePanel ID="udpMappingUpdater" runat="server" UpdateMode="conditional">
                <ContentTemplate>
                    <table class="tTable" style="width: 100%">
                        <tr>
                            <td>
                                <br />
                                <div class="tTable">
                                    <asp:DataGrid ID="dgResult" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                        DataKeyField="SalesPersonID" OnItemCommand="dgResult_CreateCommand" OnPageIndexChanged="dgResult_PageIndexChanged"
                                        GridLines="None" OnItemDataBound="dgResult_ItemDataBound" OnDeleteCommand="dgResult_DeleteCommand"
                                        CellPadding="5" CssClass="tTable tBorder" AllowPaging="False" PageSize="30">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="No">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgResult.CurrentPageIndex) * dgResult.PageSize)%>
                                                    .</ItemTemplate>
                                                <ItemStyle Width="15px" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Own SalesPersonID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSalesPersonID" runat="server">
                                                        <%# Eval( "SalesPersonID" )%>
                                                        <input type="hidden" id="hidSalesPersonID" runat="server" value='<%# Eval("SalesPersonID")%>' />
                                                        <input type="hidden" id="hidUserNumID" runat="server" value='<%# Eval("UserID")%>' />
                                                        <input type="hidden" id="hidCoyID" runat="server" value='<%# Eval("CoyID")%>' />
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlNewSalesPersonID" runat="Server" DataTextField="SalesPersonNameNID"
                                                        DataValueField="SalesPersonID" />
                                                </FooterTemplate>
                                                <ItemStyle Width="80px" />
                                                <HeaderStyle Wrap="False" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Function">
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
                                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                <FooterStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle CssClass="tHeader" />
                                        <AlternatingItemStyle CssClass="tAltRow" />
                                        <FooterStyle CssClass="tFooter" />
                                        <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                                    </asp:DataGrid>
                                </div>
                                <br /><br />
                                <div class="tTable">
                                    <asp:DataGrid ID="dgResult2" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                        DataKeyField="SalesPersonID" OnItemCommand="dgResult2_CreateCommand" OnPageIndexChanged="dgResult2_PageIndexChanged"
                                        GridLines="None" OnItemDataBound="dgResult2_ItemDataBound" OnDeleteCommand="dgResult2_DeleteCommand"
                                        CellPadding="5" CssClass="tTable tBorder" AllowPaging="False" PageSize="30">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="No">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgResult2.CurrentPageIndex) * dgResult2.PageSize)%>
                                                    .</ItemTemplate>
                                                <ItemStyle Width="15px" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Managing SalesPersonID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSalesPersonID" runat="server">
                                                        <%# Eval( "SalesPersonID" )%>
                                                        <input type="hidden" id="hidSalesPersonID" runat="server" value='<%# Eval("SalesPersonID")%>' />
                                                        <input type="hidden" id="hidManagerUserID" runat="server" value='<%# Eval("ManagerUserID")%>' />
                                                        <input type="hidden" id="hidCoyID" runat="server" value='<%# Eval("CoyID")%>' />
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlNewSalesPersonID" runat="Server" DataTextField="SalesPersonNameNID"
                                                        DataValueField="SalesPersonID" />
                                                </FooterTemplate>
                                                <ItemStyle Width="80px" />
                                                <HeaderStyle Wrap="False" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Function">
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
                                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                <FooterStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
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
            <div class="TABCOMMAND">
                <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <ul>
                            <li></li>
                        </ul>
                        <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
