<%@ Page Language="C#" AutoEventWireup="true" Codebehind="banner.aspx.cs" Inherits="GMSWeb.Common.banner" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<%@ Register Assembly="SharpPieces.Web.Controls.ExtendedDropDownList" Namespace="SharpPieces.Web.Controls"
    TagPrefix="piece" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Leeden Group Management System</title>
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
</head>
<body id="body1">
    <form id="form1" runat="server">
        <div id="header" valign="bottom">
            <div id="headersub" style="height: 36%; width: 100%;">
                <table style="vertical-align:middle">
                    <tr>
                        <td style="width: 830px">
                            <ul>
                                <li>
                                    <asp:Label ID="lblCompany" runat="server" ForeColor="#0135ad" Font-Bold="true" Text="Company :" Width="80px"></asp:Label>
                                    <piece:ExtendedDropDownList ID="ddlCompany" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" >
                                    </piece:ExtendedDropDownList></li>
                            </ul>
                        </td>
                        <td style="vertical-align: text-bottom; text-align: right;">
                        <asp:Label runat="server" ID="lblWelcome" Font-Bold="true" ForeColor="#0135ad"></asp:Label>&nbsp&nbsp&nbsp
                            <a href="../Admin/Accounts/ChangePassword.aspx" target="_blank" title="Change password"
                                style="font-weight: bold; color: #0135ad">Change Your Password Here</a></td>
                    </tr>
                </table>
            </div>
            <ul runat="server" id="linkTag">
            </ul>
        </div>
        <hr style="margin-top: -8px;" color="#0135ad" size="1px" />
    </form>
</body>
</html>
