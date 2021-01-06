<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysBanner.aspx.cs" Inherits="GMSWeb.Common.SysBanner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="SharpPieces.Web.Controls.ExtendedDropDownList" Namespace="SharpPieces.Web.Controls"
    TagPrefix="piece" %>
    <%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Leeden Group Management System</title>
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
</head>
<body id="body1">
    <form id="form1" runat="server">
        <div id="header" style="background: url(../App_Themes/Default/images/body_bg3.jpg)">
               <br>
               <table width="100%">
               <tr>
               <td align="left">
                <asp:Label runat="server" ID="lblWelcome" Font-Bold="true" ForeColor="#666362" style="font-size:8pt"></asp:Label>
                            <a href="../Admin/Accounts/ChangePassword.aspx" target="_blank" title="Change password"
                                style="font-weight: bold; color: #666362; font-size:7pt">Change Your Password <u>Here</u></a>
                </td>
               <td align="left">
                        <asp:Label ID="lblCompany" runat="server" ForeColor="#666362" Font-Bold="true" Text="Company :" Width="80px"></asp:Label>
                        <piece:ExtendedDropDownList ID="ddlCompany" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" >
                        </piece:ExtendedDropDownList></li>
               </td></tr>
               </table>
              
          </div>    
          
          <div id="header" valign="bottom" >
            <ul runat="server" id="linkTag"></ul>
          </div>
    </form>
</body>
</html>
