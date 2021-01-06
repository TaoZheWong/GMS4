<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="GMSWeb.HR.Reports.ReportViewer" %>
<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=11.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>HR & Training - View Reports</title>
    <script language="javascript" >
     function right(e) 
     { 
        if (navigator.appName == 'Netscape' && (e.which == 3 || e.which 
            == 2)) 
            return false; 
        else if (navigator.appName == 'Microsoft Internet Explorer' 
            && (event.button == 2 || event.button == 3)) 
        { 
            alert('The function is diabled!'); 
            return false; 
        } return true; 
    } 
    document.onmousedown=right; 
    if (document.layers) 
        window.captureEvents(Event.MOUSEDOWN); 
    window.onmousedown=right;
    </script>
</head>
<body ondragstart="return false" onselectstart="return false" style="margin: 0; padding: 0; background: none; text-align:left">
    <form id="form1" runat="server" style="margin: 0px; padding: 0px;">       
			<asp:Label ID="lblFeedback" runat="server" Text="" EnableViewState="false" />
		    <CR:CrystalReportViewer ID="cyReportViewer" runat="server" AutoDataBind="true" DisplayGroupTree="False"
				EnableDatabaseLogonPrompt="False" EnableDrillDown="False" DisplayToolbar="true" 
				EnableTheming="False" HasSearchButton="true" HasToggleGroupTreeButton="False" 
				PrintMode="ActiveX" HasExportButton="true" CssClass="crinput"/>
   
    </form>
</body>
</html>
