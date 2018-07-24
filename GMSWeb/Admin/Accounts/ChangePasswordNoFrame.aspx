<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordNoFrame.aspx.cs" Inherits="GMSWeb.Admin.Accounts.ChangePasswordNoFrame" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Change Password Page</title>
    <link href="../../new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/style.min.css" rel="stylesheet" />
    <link href="../../new_assets/css/overwrite_app.css" rel="stylesheet" />
    <link href="../../new_assets/plugins/icon/themify-icons/css/themify-icons.css" rel="stylesheet" />
    <style>
        body {
            overflow: auto;
        }
        #ChangePassword1 {
            width:100%
        }
    </style>
</head>
<body class="inverse-mode">
    <form id="form1" runat="server">
    <div id="page-container">
        <div class="register">
            <div class="register-cover"></div>
            <div class="register-content">
            <div class="register-brand">
				<a href="#"><span class="logo"><i class="ti-lock"></i></span> GMS</a>
			</div>
            <h3 class="m-b-20"><span>Change Password</span></h3>
             <asp:ChangePassword ID="ChangePassword1" runat="server">
            <ChangePasswordTemplate>
                <p class="m-b-20">The new password must contain minimum 4 characters and maximum up to 8 characters.</p>
               <div class="row row-space-2">
					<div class="col-lg-12">
						<div class="form-group">
							<label class="control-label">
                                <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Password:</asp:Label> 
                                <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword" CssClass="text-danger"
                                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
							</label>
							    <asp:TextBox class="form-control" ID="CurrentPassword" runat="server" TextMode="Password" MaxLength="8"></asp:TextBox>
						</div>
                        <div class="form-group">
							<label class="control-label">
                                <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">New Password:</asp:Label>
                                    <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword" CssClass="text-danger"
                                            ErrorMessage="New Password is required." ToolTip="New Password is required."
                                            ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
							</label>
							<asp:TextBox class="form-control" ID="NewPassword" runat="server" TextMode="Password" MaxLength="8"></asp:TextBox>
						</div>
                        <div class="form-group">
							<label class="control-label">
                                <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                                    <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                    ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
                                    ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                                    ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
                                    ValidationGroup="ChangePassword1"></asp:CompareValidator>
							</label>
							<asp:TextBox class="form-control" ID="ConfirmNewPassword" runat="server" TextMode="Password" MaxLength="8"></asp:TextBox>
						</div>
                               
					</div>
				</div>
               <asp:Button ID="ChangePasswordPushButton" CssClass="btn btn-primary" runat="server" CommandName="ChangePassword" Text="Change Password" ValidationGroup="ChangePassword1" />
            </ChangePasswordTemplate>
            <SuccessTemplate>
                <p class="m-b-20">Change Password Complete</p>
                <p class="m-b-20">Your password has been changed!</p>
                <a class="btn btn-primary" href="../../Default.aspx">Go To GMS Homepage</a>
            </SuccessTemplate>
        </asp:ChangePassword>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
