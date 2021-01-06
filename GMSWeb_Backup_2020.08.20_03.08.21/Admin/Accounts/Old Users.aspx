<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Old Users.aspx.cs" Inherits="GMSWeb.Admin.Accounts.Users" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Accounts - Users</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ContentBar">
        <h3>Accounts &gt; Users</h3>
        Create new user account or click on an acount to modify.
        <br />
        <br />
        <table class="tTable"style="BORDER-COLLAPSE: collapse" cellspacing="0" cellpadding="1" 
				border="1" width="80%">
        <tr>
        <td class="tbLabel">User Name</td><td>:</td>
        <td colspan="2">
        <asp:TextBox runat="server" ID="searchUserName" MaxLength="50" Columns="50" onfocus="select();" CssClass="textbox"></asp:TextBox></td>
		<td style="width:10%"><asp:Button id="btnSearch" Text="Search" EnableViewState="False" Runat="server" CssClass="button" OnClick="btnSearch_Click"></asp:Button></td>
        </tr>
        </table>
        <br />
        <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
        <table class="tTable" style="width: 1300px">
            <tr>
			    <td>
				    <br />
				    <div class="tTable">
					    <atlas:UpdatePanel ID="udpUsersUpdater" runat="server" Mode="conditional">
						    <ContentTemplate>
							    <asp:DataGrid ID="dgUsers" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="Id" OnCancelCommand="dgUsers_CancelCommand" OnEditCommand="dgUsers_EditCommand"
								    OnUpdateCommand="dgUsers_UpdateCommand" OnItemCommand="dgUsers_ItemCommand" 
								    GridLines="none" OnItemDataBound="dgUsers_ItemDataBound" OnPageIndexChanged="dgUsers_PageIndexChanged"
								    CellPadding="5" CellSpacing="0" CssClass="tTable tBorder" AllowPaging ="True" PageSize ="50">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
										    <input type="hidden" id="hidUserID" runat="server" value='<%# Eval("Id")%>' />
											    <%# (Container.ItemIndex + 1) + ((dgUsers.CurrentPageIndex) * dgUsers.PageSize)%>
											    .</ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="User Name" HeaderStyle-Wrap="false" ItemStyle-Width="100px">
										    <ItemTemplate>
										        <asp:Label ID="lblUserRealName" runat="server">
										           <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
												    CausesValidation="false"><span><%# Eval( "UserRealName" )%></span></asp:LinkButton>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditUserRealName" runat="Server" Columns="20" MaxLength="50" Text='<%# Eval("UserRealName") %>' />
										        <asp:RequiredFieldValidator ID="rfvEditUserRealName" runat="server" ControlToValidate="txtEditUserRealName"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewUserRealName" runat="Server" Columns="20" MaxLength="50" />
											    <asp:RequiredFieldValidator ID="rfvNewUserRealName" runat="server" ControlToValidate="txtNewUserRealName"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Login ID" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
										    <ItemTemplate>
										        <asp:Label ID="lblLoginID" runat="server">
										           <%# Eval( "UserName" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditLoginID" runat="Server" Columns="10" MaxLength="50" Text='<%# Eval("UserName") %>' />
										        <asp:RequiredFieldValidator ID="rfvEditLoginID" runat="server" ControlToValidate="txtEditLoginID"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewLoginID" runat="Server" Columns="10" MaxLength="50" />
											    <asp:RequiredFieldValidator ID="rfvNewLoginID" runat="server" ControlToValidate="txtNewLoginID"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Email" ItemStyle-Width="170px">
										    <ItemTemplate>
											    <asp:Label ID="lblEmail" runat="server">
												    <%# Eval("MemberObject.Email")%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditEmail" runat="server" Columns="30" MaxLength="50" Text='<%# Eval("MemberObject.Email") %>' />
										        <asp:RequiredFieldValidator ID="rfvEditEmail" runat="server" ControlToValidate="txtEditEmail"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewEmail" runat="server" Columns="30" MaxLength="50" />
											    <asp:RequiredFieldValidator ID="rfvNewEmail" runat="server" ControlToValidate="txtNewEmail"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Active">
										    <ItemTemplate>
											    <%# ( (bool)Eval( "MemberObject.IsApproved" ) ) ? "Yes" : "No"%>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:CheckBox ID="chkActive" runat="server" />
										    </EditItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Allow Remote Access">
										    <ItemTemplate>
											    <%# ( (bool)Eval( "AllowRemoteAccess" ) ) ? "Yes" : "No"%>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:CheckBox ID="chkEditAllowRemoteAccess" runat="server" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:CheckBox ID="chkNewAllowRemoteAccess" runat="server" Checked="false" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" 
									        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="User Access Rights">
									        <ItemTemplate>
									            <asp:HyperLink ID="hypEditAccessRights" runat="server" EnableViewState="false"
									                CssClass="EditButt"><span>&nbsp;&nbsp;Edit Access Rights</span></asp:HyperLink>
									        </ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Center" 
									        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Reset Password">
									        <ItemTemplate>
									            <asp:LinkButton ID="hypResetPassword" runat="server" CommandName="ResetPassword" EnableViewState="false"
									                CssClass="EditButt"><span>Reset Password</span></asp:LinkButton>
									        </ItemTemplate>
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
						    </ContentTemplate>
					    </atlas:UpdatePanel>
				    </div>
			    </td>
		    </tr>
	    </table>
	    <br />   
	    <div class="TABCOMMAND">
		    <atlas:UpdatePanel ID="udpMsgUpdater" runat="server" Mode="Always">
			    <ContentTemplate>
				    <ul>
					    <li>&nbsp;</li>
				    </ul>
				    <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
			    </ContentTemplate>
		    </atlas:UpdatePanel>
        </div>   
    </div>
    </form>
</body>
</html>
