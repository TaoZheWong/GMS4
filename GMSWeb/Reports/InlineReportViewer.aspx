<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InlineReportViewer.aspx.cs" Inherits="GMSWeb.Reports.InlineReportViewer" %>

<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=11.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Report - Print</title>
</head>
<body style="margin: 8px; padding: 0; background: none; text-align:left">
    <form id="form1" runat="server">
   
    <h1 style=" width: 702px">Report Parameters</h1>
    <p>Select or enter the report parameters in below.</p>
        
            <asp:Panel ID="pnlParameter" runat="server"></asp:Panel>            
           
    <br />
    
    
    
    <div>
        <asp:Label ID="lblFeedback" runat="server" Text="" EnableViewState="false" />
	    <CR:CrystalReportViewer ID="cyReportViewer" runat="server" AutoDataBind="true" DisplayGroupTree="false"
		EnableDatabaseLogonPrompt="False" EnableDrillDown="false" DisplayToolbar="true" 
		EnableTheming="False" HasSearchButton="true" HasToggleGroupTreeButton="true" 
		PrintMode="ActiveX" HasExportButton="true"/>
    </div>
    </form>
</body>
</html>
