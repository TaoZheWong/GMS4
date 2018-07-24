<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword1.aspx.cs" Inherits="GMSWeb.Admin.Accounts.ChangePassword" Title="System Admin - Change Password Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>
<h1>Accounts &gt; Change Password</h1>
        <p>Change your GMS password here. The new password must contain minimum 4 characters and maximum up to 8 characters.</p>
        <asp:ChangePassword ID="ChangePassword1" runat="server">
            <ChangePasswordTemplate>
                <table cellpadding="5" cellspacing="5" class="tblTable" style="margin-left: 8px">
                    <tr>
                        <td style="width: 382px">
                            <table border="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Password:</asp:Label></td>
                                    <td>
                                        <asp:TextBox class="textbox" ID="CurrentPassword" runat="server" TextMode="Password" MaxLength="8"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">New Password:</asp:Label></td>
                                    <td>
                                        <asp:TextBox class="textbox" ID="NewPassword" runat="server" TextMode="Password" MaxLength="8"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                            ErrorMessage="New Password is required." ToolTip="New Password is required."
                                            ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label></td>
                                    <td>
                                        <asp:TextBox class="textbox" ID="ConfirmNewPassword" runat="server" TextMode="Password" MaxLength="8"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                            ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
                                            ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                                            ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
                                            ValidationGroup="ChangePassword1"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="color: red">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="ChangePasswordPushButton" CssClass="button" runat="server" CommandName="ChangePassword"
                                            Text="Change Password" ValidationGroup="ChangePassword1" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ChangePasswordTemplate>
            <SuccessTemplate>
                <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0">
                                <tr>
                                    <td align="center" colspan="2">
                                        Change Password Complete</td>
                                </tr>
                                <tr>
                                    <td>
                                        Your password has been changed!</td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </SuccessTemplate>
        </asp:ChangePassword>
</asp:Content>
