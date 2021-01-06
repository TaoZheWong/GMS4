<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productbanner.aspx.cs" Inherits="GMSWeb.Products.productbanner" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title>Leeden Group Management System</title>
	<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
</head>
<body>	
        <form id="form1" runat="server">
			<div id="header" valign="bottom">
			    <div id="headersub" style=" height:36%; width:100%; ">
			    <table><tr><td style="width:830px">
			        <ul>
			            <li>
				            <asp:Label ID="lblCountry" runat="server" ForeColor="#0135ad" Font-Bold="true" Text="Country: "></asp:Label>
                            <asp:DropDownList ID="ddlCountry" runat="server" DataTextField="Name" DataValueField="CountryId" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                            </asp:DropDownList>
					    </li>
					    <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</li>
					    <li>
				            <asp:Label ID="lblDivision" runat="server" ForeColor="#0135ad" Font-Bold="true" Text="Division: "></asp:Label>
                            <asp:DropDownList ID="ddlDivision" runat="server" DataTextField="Name" DataValueField="DivisionId" AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                            </asp:DropDownList>
					    </li>
					    <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</li>
			            <li>
				            <asp:Label ID="lblCompany" runat="server" ForeColor="#0135ad" Font-Bold="true" Text="Company : "></asp:Label>
                            <asp:DropDownList ID="ddlCompany" runat="server" DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                            </asp:DropDownList>
					    </li>
			        </ul>
			    </td><td style="  vertical-align:text-bottom; text-align:right;"><a  href="../Admin/Accounts/ChangePassword.aspx" target="_blank" title="Change password" style="font-weight:bold; color:#0135ad">Change Your Password Here</a></td></tr>
			    </table></div>
				<ul runat="server" id="linkTag">				
				</ul>
			</div>
			<hr style="margin-top: -8px; " color="#0135ad" size="1px" />
		</form>
	</body>
</html>
