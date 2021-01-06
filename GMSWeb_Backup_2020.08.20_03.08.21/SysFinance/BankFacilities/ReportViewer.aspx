<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="GMSWeb.SysFinance.BankFacilities.ReportViewer" %>

<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=11.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Report - Print</title>
</head>
<body style="background: none; text-align: left;">
    <div>
    <form id="form1" runat="server" style="margin: 0px; padding: 0px;">
        	<asp:Label ID="lblFeedback" runat="server" Text="" EnableViewState="false" />
	        
	         <CR:CrystalReportViewer ID="cyReportViewer" runat="server" AutoDataBind="true" DisplayGroupTree="False"
				EnableDatabaseLogonPrompt="False" EnableDrillDown="False" DisplayToolbar="true" 
				EnableTheming="False" HasSearchButton="true" HasToggleGroupTreeButton="False" 
				PrintMode="ActiveX" HasExportButton="true" />
    </form>
    </div>
</body>
</html>
