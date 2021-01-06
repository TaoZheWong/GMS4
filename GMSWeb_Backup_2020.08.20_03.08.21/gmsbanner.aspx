<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="gmsbanner.aspx.cs" Inherits="GMSWeb.gmsbanner" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<title>Ace Dynamics Group Management System</title>
	<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
	<script type="text/javascript">
		
			function SetHighLight( n ) {
				ClearAllHighLight();
				
				switch( n ) {
					case 1:
						document.getElementById("lnkHOME").className = "Current";
						break;
					case 2:
						document.getElementById("lnkHR").className = "Current";
						break;
					case 3:
						document.getElementById("lnkPRODUCTS").className = "Current";
						break;
					case 4:
						document.getElementById("lnkSALES").className = "Current";
						break;
					case 5:
						document.getElementById("lnkREPORTS").className = "Current";
						break;
					case 6:
						document.getElementById("lnkCORPORATE").className = "Current";
						break;
					case 7:
						document.getElementById("lnkFINANCE").className = "Current";
						break;
					case 8:
						document.getElementById("lnkMIS").className = "Current";
						break;
					case 9:
						document.getElementById("lnkADMIN").className = "Current";
						break;
					case 10:
						document.getElementById("lnkORGANIZATION").className = "Current";
						break;
				}							
			}
			
			function ClearAllHighLight() {
				var x = document.getElementsByTagName('a');			
				for (var i=0;i<x.length;i++) {					
					if (x[i].className == 'Current') {					
							x[i].className = '';
					}			
				}
			}
		
		</script>
</head>
<body>	
        <form id="form1" runat="server">
			<div id="header" valign="bottom">
			    <div id="headersub">
			        <ul>
			            <li>
				            <asp:Label ID="lblCountry" runat="server" ForeColor="#349a34" Font-Bold="true" Text="Country: "></asp:Label>
                            <asp:DropDownList ID="ddlCountry" runat="server" DataTextField="Name" DataValueField="CountryId" AutoPostBack="true" OnLoad="LoadCompanyList" OnSelectedIndexChanged="LoadCompanyList">
                            </asp:DropDownList>
					    </li>
					    <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</li>
					    <li>
				            <asp:Label ID="lblDivision" runat="server" ForeColor="#349a34" Font-Bold="true" Text="Division: "></asp:Label>
                            <asp:DropDownList ID="ddlDivision" runat="server" DataTextField="Name" DataValueField="DivisionId" AutoPostBack="true" OnLoad="LoadCompanyList" OnSelectedIndexChanged="LoadCompanyList">
                            </asp:DropDownList>
					    </li>
					    <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</li>
			            <li>
				            <asp:Label ID="lblCompany" runat="server" ForeColor="#349a34" Font-Bold="true" Text="Company : "></asp:Label>
                            <asp:DropDownList ID="ddlCompany" runat="server" DataTextField="Name" DataValueField="Id">
                            </asp:DropDownList>
					    </li>
			        </ul>
			    </div>
			    <br /><br />
				<ul>				
					<li><a href="main.aspx" id="lnkHOME" class="Current" target="_parent"><span>Home</span></a></li>
					<li><a href="gmscontent.aspx" id="lnkORGANIZATION" target="main"><span>Organization</span></a></li>
					<li><a href="Products/" id="lnkPRODUCTS" target="main"><span>Products</span></a></li>
					<li><a href="Sales/" id="lnkSALES" target="main"><span>Sales</span></a></li>
					<li><a href="Reports/" id="lnkREPORTS" target="main"><span>Reports</span></a></li>
					<li><a href="Logout.aspx?LOGOUT" id="lnkLogout" target="_top"><span>Logout</span></a></li>
				</ul>
			</div>
			<hr style="margin-top: -8px; " color="#349a34" size="1px" />
		</form>
	</body>
</html>
