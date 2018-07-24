<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YesNo_PopUp.aspx.cs" Inherits="GMSWeb.Common.YesNo_PopUp" %>
<!DOCTYPE html 
     PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
     "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Confirmation</title>
</head>
	<BODY style="background-image: none; text-align: left" >
		<FORM ID="Form1" RUNAT="server">	<br>	
			<table width="100%" height="100%">
				<tr>
					<td align="center">
						<asp:Label ID="lblMsg" Runat="server"></asp:Label><br><br>
					</td>
				</tr>
				<tr>
				<td align="center">
						<input type=button tabindex="2" class=button id=btnYes value=Yes onclick="window.returnValue=true;window.close();">
						&nbsp;
						<input type=button tabindex="1" class=button id=btnNo value=No onclick="window.returnValue=false;window.close();">
				</td>
				</tr>
			</table>
			
		</FORM>
	</BODY>
</HTML>
