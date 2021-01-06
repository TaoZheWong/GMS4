<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GMSWeb.Default" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Leeden Group Management System</title>
    <uctrl:Header ID="MyHeader" runat="server" EnableViewState="True"></uctrl:Header>
    <style type="text/css">
            #divLogin {
				border: 1px solid #0135ad;
                margin-top: -2px;
                padding-bottom: 0px;
                width: 800px;
                margin-left: -400px;
                font-size: 12px;
                left: 50%;
                position: absolute;

            }
	</style>

	<script type="text/javascript">
		//<![CDATA[
			window.onload = function() { 
				if( window.top.location.href != document.location.href )
					window.top.location.href = document.location.href;
			}
		//]]>
	</script>

</head>
<body onload="focusTxt()">
	<form id="form1" runat="server">
	    <br /><br /><br /><br /><br />
	    <div id="divLogin">
	        <div class="PanelBar" style="margin-top: -8px;">
	            <img src="images/topbar.jpg" alt="Leeden" />
            </div>
            <br /><br /><br /><br />
	        <div class="PanelBar" style="margin-left:8px;">
	            <img src="images/leedenlogo2.jpg" alt="Leeden" />
            </div>
            <br /><br /><br />
	        <div class="PanelBar">
                <hr color="#0135ad" size="1px" />
            </div>
			<div class="PanelBar" style="margin-top: -15px; margin-left:8px;">
                 <b>Group Management System (GMS)</b>
            </div>
            <div class="PanelBar" style="margin-top: -15px;">
                <hr color="#0135ad" size="1px" />
            </div>
            <div class="PanelBar" style="margin-top: -15px; margin-left:8px; color: Black;">
                 Group Management System (GMS) is for the management for easy viewing for updated
                information and reports. HODs are able to keep the information for reference.
            </div>
                <br />
                <br />
				<asp:Login ID="lgLoginControl" runat="server" DisplayRememberMe="False" DestinationPageUrl="~/main.aspx"
					TitleText="" OnAuthenticate="lgLoginControl_Authenticate" Width="333px" style="margin-left: 85px;">
                    <LayoutTemplate>
                        <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse; width: 326px;">
                            <tr>
                                <td style="width: 511px">
                                    <table border="0" cellpadding="0" style="width: 332px">
                                        <tr>
                                            <td align="left" style="width: 91px">
                                                <asp:Label ID="UserNameLabel" runat="server" Font-Size="12px" AssociatedControlID="UserName">Login ID:</asp:Label></td>
                                            <td style="width: 298px">
                                                <asp:TextBox ID="UserName" Columns="20" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                    ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="lgLoginControl">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 211px">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 91px">
                                                <asp:Label ID="PasswordLabel" runat="server" Font-Size="12px" AssociatedControlID="Password">Password:</asp:Label></td>
                                            <td style="width: 298px">
                                                <asp:TextBox ID="Password" Columns="20" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                    ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="lgLoginControl">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 211px">
                                                <asp:Button ID="LoginButton" CssClass="button" runat="server" CommandName="Login" Text="Log In" ValidationGroup="lgLoginControl" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" style="color: red">
                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="2">
                                                </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
				</asp:Login>
				<asp:Label ID="lblMessage" ForeColor="red" runat="server" /><br />
                <br /><br />
		</div>
	    <br /><br /><br /><br /><br />
	    <br /><br /><br /><br /><br />
	    <br /><br /><br /><br /><br />
	    <br /><br /><br /><br /><br />
	    <br /><br /><br /><br /><br />
	    <br /><br /><br /><br /><br />
		<DIV ID="footer">
				Copyright ?2007, Leeden Limited.
		</DIV>
	</form>
</body>
</html>
