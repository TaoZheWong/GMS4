<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocViewer.aspx.cs" Inherits="GMSWeb.Reports.DocViewer" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
	Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
	
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Report - View</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
	    
			<asp:Label ID="lblFeedback" runat="server" Text="" EnableViewState="false" />
	        
		    <CR:CrystalReportViewer ID="cyReportViewer" runat="server" AutoDataBind="true" DisplayGroupTree="False"
				EnableDatabaseLogonPrompt="False" EnableDrillDown="False" EnableParameterPrompt="False"
				EnableTheming="False" HasSearchButton="False" HasToggleGroupTreeButton="False"
				Height="520px" PrintMode="ActiveX" Width="790px" BestFitPage="false" HasExportButton="False" />
    </div>
    </form>
</body>
</html>
