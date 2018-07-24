<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Message.aspx.cs" Inherits="GMSWeb.Common.Message" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Message</title>
</head>
<body style="background-image: none; text-align: left">
    <form id="form1" runat="server">
    <br />
    <div>
    <h5 runat="server" id="lblMessgae"></h5>
    <br />
    </div>
    <asp:Button runat="server" Text="Close This Window" OnClientClick="javascript:window.close()" />
    </form>
</body>
</html>
