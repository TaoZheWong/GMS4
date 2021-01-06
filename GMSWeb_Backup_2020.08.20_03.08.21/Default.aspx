<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GMSWeb.Default" %>

<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <title>GMS</title>
    <meta charset="utf-8" />
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport" />
    <meta name="theme-color" content="#007ee5">
    <link href="new_assets/css/animate.min.css" rel="stylesheet" />
    <link href="new_assets/plugins/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="icon" href="images/favicon.png" />
    <style>
        body {
            font-family: Roboto,"Helvetica Neue",Helvetica,Arial,sans-serif;
        }

        .login-form {
            width: 330px;
            margin-right: 130px;
            margin-top: 95px;
            float: right;
            background: #fff;
            position: relative;
            z-index:10;
        }

        .login-content {
            border-top: 4px solid #007ee5;
            z-index: 9;
            padding: 24px 24px 28px 24px;
            border-left: 1px solid #666872;
            -moz-box-shadow: 0 10px 10px 0 rgba(0, 0, 0, 0.30), 0 6px 3px 0 rgba(0, 0, 0, 0.23);
            box-shadow: 0 10px 10px 0 rgba(0, 0, 0, 0.30), 0 6px 3px 0 rgba(0, 0, 0, 0.23);
        }

        .login-logo {
            text-align: center;
            margin-bottom: 20px;
        }

        .btn-primary {
            background-color: #007ee5;
        }

        label {
            font-size: 14px;
            color: #4F5256;
        }

        .forgot-btn {
            margin-top: 20px;
        }

        .no-border {
            border-radius: 0;
        }


        .image {
            background-repeat: no-repeat;
            vertical-align: middle;
            background-size: cover;
            background-position: 50% 50%;
            position: fixed;
            height: 100%;
            width: 100%;
        }

        .Wallpaper1 {
            background-image: url(new_assets/images/background1.jpg);
        }
        .Wallpaper2 {
            background-image: url(new_assets/images/background2.jpg);
        }
        .Wallpaper3 {
            background-image: url(new_assets/images/background3.jpg);
        }
        .Wallpaper4 {
            background-image: url(new_assets/images/background4.jpg);
        }
        .Wallpaper5 {
            background-image: url(new_assets/images/background5.jpg);
        }
        .Wallpaper6 {
            background-image: url(new_assets/images/background6.jpg);
        }
        .Wallpaper7 {
            background-image: url(new_assets/images/background7.jpg);
        }
        .Wallpaper8 {
            background-image: url(new_assets/images/background8.jpg);
        }
        .Wallpaper9 {
            background-image: url(new_assets/images/background9.jpg);
        }
        .Wallpaper10 {
            background-image: url(new_assets/images/background10.jpg);
        }
        .Wallpaper11 {
            background-image: url(new_assets/images/background11.jpg);
        }
        .Wallpaper12 {
            background-image: url(new_assets/images/background12.jpg);
        }

        .gradient {
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            width: 100%;
            background: -moz-linear-gradient(top, rgba(0, 0, 0, 0.5) 0%, rgba(0, 0, 0, 0) 59%);
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%, rgba(0, 0, 0, 0.5)), color-stop(59%, rgba(0, 0, 0, 0)));
            background: -webkit-linear-gradient(top, rgba(0, 0, 0, 0.5) 0%, rgba(0, 0, 0, 0) 59%);
            background: -o-linear-gradient(top, rgba(0, 0, 0, 0.5) 0%, rgba(0, 0, 0, 0) 59%);
            background: -ms-linear-gradient(top, rgba(0, 0, 0, 0.5) 0%, rgba(0, 0, 0, 0) 59%);
            background: linear-gradient(to bottom, rgba(0, 0, 0, 0.5) 0%, rgba(0, 0, 0, 0) 59%);
            filter: progid: DXImageTransform.Microsoft.gradient(startColorstr='#80000000', endColorstr='#00000000', GradientType=0);
            z-index: 1;
        }

        @media (max-width: 990px) {
            .login-form {
                margin: 0 auto;
                float: none;
                top: 100px;
                width: 80%;
            }
        }
    </style>
</head>
<body onload="focusTxt()">

    <div class="image Wallpaper<%=getCurrentMonth %>"></div>
   
    <form id="form1" runat="server" class="container-fluid">
        <asp:Login ID="lgLoginControl" runat="server" DisplayRememberMe="False" DestinationPageUrl="~/main.aspx"
            TitleText="" OnAuthenticate="lgLoginControl_Authenticate" CssClass="login-form animated fadeInDown" EnableTheming="False">
            <LayoutTemplate>
                <div class="login-content">
                    <div class="login-logo">
                        <img src="new_assets/images/logo.png" />
                    </div>
                    <p class="text-danger">
                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></p>
                    <div class="form-group">
                        <asp:Label ID="UserNameLabel" CssClass="control-label" runat="server" AssociatedControlID="UserName">Login ID</asp:Label>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                            ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="lgLoginControl">*</asp:RequiredFieldValidator>
                        <asp:TextBox ID="UserName" Columns="20" runat="server" CssClass="form-control no-border"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password</asp:Label>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="lgLoginControl">*</asp:RequiredFieldValidator>
                        <asp:TextBox ID="Password" Columns="20" runat="server" TextMode="Password" CssClass="form-control no-border" MaxLength="8"></asp:TextBox>
                    </div>

                    <div class="login-buttons">
                        <asp:Button ID="LoginButton" CssClass="btn btn-primary btn-lg btn-block no-border" runat="server" CommandName="Login" Text="Log In" ValidationGroup="lgLoginControl" />
                        <asp:LinkButton runat="server" CssClass="pull-right forgot-btn" ID="lnkResetPassword" OnCommand="ResetPassword" OnClientClick="return confirm('Are you sure you want to reset your password?');">Reset Password</asp:LinkButton>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </LayoutTemplate>
        </asp:Login>
        <asp:Label ID="lblMessage" ForeColor="red" runat="server" />

    </form>
</body>
</html>
