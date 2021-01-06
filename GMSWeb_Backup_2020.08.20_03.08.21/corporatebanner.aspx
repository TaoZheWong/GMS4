<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="corporatebanner.aspx.cs" Inherits="GMSWeb.corporatebanner" %>

<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
			<div id="corporateheader" valign="bottom">
				<ul>				
					<li><a href="main.aspx" runat="server"  id="lnkHOME" class="Current" target="_parent"><span>Home</span></a></li>
					<li><a href="Corporate/" runat="server" id="lnkCORPORATE" target="main"><span>Corporate</span></a></li>
					<li><a href="Finance/" runat="server" id="lnkFINANCE" target="main"><span>Finance</span></a></li>
					<li><a href="HR/" runat="server" id="lnkHR" target="main"><span>Human Resource</span></a></li>
					<li><a href="MIS/" runat="server" id="lnkMIS" target="main"><span>MIS</span></a></li>
					<li><a href="Admin/" runat="server" id="lnkADMIN" target="main"><span>Admin</span></a></li>
					<li><a href="Logout.aspx" id="lnkLogout" target="_top"><span>Logout</span></a></li>
				</ul>
			</div>
			<hr style="margin-top: -8px; " color="#0f04b8" size="1px" />
			
		</form>
	</body>
</html>
