<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CashFlowProjPopUp.aspx.cs" Inherits="GMSWeb.Finance.CashFlow.CashFlowProjPopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cash Flow Projections</title>   
    
</head>
<body style="background-image:none; text-align: left" >
    <form id="form1" runat="server">
       <div>
       
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <table style="margin-left: 8px;" class="tInfoTable" width="100%" >
            
            <tr>
            <td style="color:#3C3C3C; font-weight: bold; font-size:10px; padding: 5px;">                       
                Net Operating Cash Flow = Total Cash Inflow - Total Operating Expenses            
            </td>            
            </tr>
            <tr>
            <td style="color:#3C3C3C; font-weight: bold; font-size:10px; padding: 5px;">                       
               Net Cash Flow Surplus/(Deficit) = Net Operating Cash Flow + Net Cash flow from Investing + Net Cash Flow From Financing            
            </td>            
            </tr>           
            
       </table>
       
       
       
       
       <p style="padding-top:10px"></p>
<asp:UpdatePanel runat="server" id="upCashFlowData" updatemode="Conditional">      
<ContentTemplate>


<table class="tTable1" style="margin-left: 8px;" cellspacing="5" cellpadding="5" border="0" width="99%">
    <tr>
        <td class="tbLabel" style="width: 15%">
        <asp:Label CssClass="tbLabel" ID="lblYear" runat="server">Year</asp:Label>
        </td>
		<td style="width: 2%">:</td>
		<td>
		<asp:DropDownList CssClass="dropdownlist" ID="ddlYear" runat="server" DataTextField="Year" DataValueField="Year" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_OnSelectedIndexChanged" />
		</td>
		
		<td class="tbLabel" style="width: 15%">
		<asp:Label CssClass="tbLabel" ID="lblWeek" runat="server">Week</asp:Label>
		</td>
		<td style="width: 2%">:</td>
		<td>
		<asp:DropDownList CssClass="dropdownlist" ID="ddlWeek" runat="server" DataTextField="WeekRange" DataValueField="Week" Width="250px" />
		</td>
		
		<td>
	    <asp:Button runat="server" ID="btnSubmit" CssClass="button" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="Value" />
	    </td>
	    
	    <td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
	</tr>	            
	
    </table>
    <br />

<table class="tTable tBorder" style="margin-left: 8px" cellspacing="2" cellpadding="2" border="0" width="97%">
    <tr class="tHeader">
    <td align="left" class="tbLabel">Month<br />Week<br />Date Range</td>
    <td align="center">
        <asp:Label ID="lblMonth1" runat="server"></asp:Label>-<asp:Label ID="lblYear1" runat="server"></asp:Label>
        <br />Week <asp:Label ID="lblWeek1" runat="server"></asp:Label>
        <br /><asp:Label ID="lblRange1" runat="server" Font-Size="X-Small" Font-Bold="false"></asp:Label>
        <asp:HiddenField ID="hidMonth1" runat="server" />
        <asp:HiddenField ID="hidYear1" runat="server" />
    </td>
    <td align="center">
        <asp:Label ID="lblMonth2" runat="server"></asp:Label>-<asp:Label ID="lblYear2" runat="server"></asp:Label>
        <br />Week <asp:Label ID="lblWeek2" runat="server"></asp:Label>
        <br /><asp:Label ID="lblRange2" runat="server" Font-Size="X-Small" Font-Bold="false"></asp:Label>
        <asp:HiddenField ID="hidMonth2" runat="server" />
        <asp:HiddenField ID="hidYear2" runat="server" />
    </td>
    <td align="center">
        <asp:Label ID="lblMonth3" runat="server"></asp:Label>-<asp:Label ID="lblYear3" runat="server"></asp:Label>
        <br />Week <asp:Label ID="lblWeek3" runat="server"></asp:Label>
        <br /><asp:Label ID="lblRange3" runat="server" Font-Size="X-Small" Font-Bold="false"></asp:Label>
        <asp:HiddenField ID="hidMonth3" runat="server" />
        <asp:HiddenField ID="hidYear3" runat="server" />
    </td>
    <td align="center">
        <asp:Label ID="lblMonth4" runat="server"></asp:Label>-<asp:Label ID="lblYear4" runat="server"></asp:Label>
        <br />Week <asp:Label ID="lblWeek4" runat="server"></asp:Label>
        <br /><asp:Label ID="lblRange4" runat="server" Font-Size="X-Small" Font-Bold="false"></asp:Label>
        <asp:HiddenField ID="hidMonth4" runat="server" />
        <asp:HiddenField ID="hidYear4" runat="server" />
    </td>
    <td align="center">
        <asp:Label ID="lblMonth5" runat="server"></asp:Label>-<asp:Label ID="lblYear5" runat="server"></asp:Label>
        <br />Week <asp:Label ID="lblWeek5" runat="server" ></asp:Label>
        <br /><asp:Label ID="lblRange5" runat="server" Font-Size="X-Small" Font-Bold="false"></asp:Label>
        <asp:HiddenField ID="hidMonth5" runat="server" />
        <asp:HiddenField ID="hidYear5" runat="server" />
    </td>
    <td align="center">
        <asp:Label ID="lblMonth6" runat="server"></asp:Label>-<asp:Label ID="lblYear6" runat="server"></asp:Label>
        <br />Week <asp:Label ID="lblWeek6" runat="server"></asp:Label>
        <br /><asp:Label ID="lblRange6" runat="server" Font-Size="X-Small" Font-Bold="false"></asp:Label>
        <asp:HiddenField ID="hidMonth6" runat="server" />
        <asp:HiddenField ID="hidYear6" runat="server" />
    </td>
    <td align="center">
        <asp:Label ID="lblMonth7" runat="server"></asp:Label>-<asp:Label ID="lblYear7" runat="server"></asp:Label>
        <br />Week <asp:Label ID="lblWeek7" runat="server"></asp:Label>
        <br /><asp:Label ID="lblRange7" runat="server" Font-Size="X-Small" Font-Bold="false"></asp:Label>
        <asp:HiddenField ID="hidMonth7" runat="server" />
        <asp:HiddenField ID="hidYear7" runat="server" />
    </td>
    <td align="center">
        <asp:Label ID="lblMonth8" runat="server"></asp:Label>-<asp:Label ID="lblYear8" runat="server"></asp:Label>
        <br />Week <asp:Label ID="lblWeek8" runat="server"></asp:Label>
        <br /><asp:Label ID="lblRange8" runat="server" Font-Size="X-Small" Font-Bold="false"></asp:Label>
        <asp:HiddenField ID="hidMonth8" runat="server" />
        <asp:HiddenField ID="hidYear8" runat="server" />
    </td>
    </tr>
    
    <tr class="tAltRow" style="background-color:#E9EAEB;">
    <td colspan="9"><strong><u>Cash Inflow from Operating Activities</u></strong></td>
    </tr>
    
    <tr>
    <td align="left">Collection from Sales</td>
    <td align="center">
        <asp:TextBox ID="txtCFS1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCFS2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCFS3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCFS4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCFS5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCFS6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCFS7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCFS8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Other Income</td>
    <td align="center">
        <asp:TextBox ID="txtOI1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOI2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOI3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOI4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOI5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOI6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOI7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOI8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left" class="tbLabel"><i>Total Cash Inflow</i></td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblTCI1" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblTCI2" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTCI3" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTCI4" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTCI5" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTCI6" runat="server" Font-Bold="true"></asp:Label>
    </td>   
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTCI7" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTCI8" runat="server" Font-Bold="true"></asp:Label>
    </td>
    </tr>
    
    <tr class="tAltRow" style="background-color:#E9EAEB;">
    <td colspan="9"><strong><u>Less Cash Outflow from Operating Activities</u></strong></td>
    </tr>
    
    <tr>
    <td align="left">Payment to Overseas Supplier</td>
    <td align="center">
        <asp:TextBox ID="txtPTOS1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTOS2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTOS3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTOS4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTOS5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTOS6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTOS7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTOS8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Payment to Local Supplier</td>
    <td align="center">
        <asp:TextBox ID="txtPTLS1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTLS2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTLS3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTLS4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTLS5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTLS6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTLS7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPTLS8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Salesman Claim</td>
    <td align="center">
        <asp:TextBox ID="txtSC1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSC2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSC3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSC4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSC5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSC6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSC7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSC8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Salary Payment</td>
    <td align="center">
        <asp:TextBox ID="txtSP1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSP2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSP3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSP4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSP5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSP6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSP7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtSP8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Other Payment</td>
    <td align="center">
        <asp:TextBox ID="txtOP1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOP2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOP3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOP4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOP5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOP6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOP7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtOP8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td align="left">Taxs Payment (GST + corp)</td>
    <td align="center">
        <asp:TextBox ID="txtTP1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTP2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTP3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTP4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTP5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTP6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTP7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTP8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left" class="tbLabel"><i>Total Operating Expenses</i></td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblTOE1" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblTOE2" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTOE3" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTOE4" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTOE5" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTOE6" runat="server" Font-Bold="true"></asp:Label>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTOE7" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTOE8" runat="server" Font-Bold="true"></asp:Label>
    </td>
    </tr>
    
    <tr>
    <td align="left" class="tbLabel"><i>Net Operating Cash Flow</i></td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblNPCF1" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblNPCF2" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNPCF3" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNPCF4" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNPCF5" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNPCF6" runat="server" Font-Bold="true"></asp:Label>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNPCF7" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNPCF8" runat="server" Font-Bold="true"></asp:Label>
    </td>
    </tr>
    
    <tr class="tAltRow" style="background-color:#E9EAEB;">
    <td colspan="9"><strong><u>Less/Add Cash Flow from Investing Activities</u></strong></td>
    </tr>
    
    <tr>
    <td align="left">Purchase of Fixed Assets</td>
    <td align="center">
        <asp:TextBox ID="txtPOFA1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOFA2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOFA3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOFA4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOFA5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOFA6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOFA7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOFA8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Investments/Others</td>
    <td align="center">
        <asp:TextBox ID="txtIO1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIO2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIO3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIO4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIO5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIO6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIO7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIO8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Disposal of Fixed Assets</td>
    <td align="center">
        <asp:TextBox ID="txtDOFA1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOFA2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOFA3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOFA4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOFA5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOFA6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOFA7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOFA8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Disposal of Investments/Others</td>
    <td align="center">
        <asp:TextBox ID="txtDOIO1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOIO2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOIO3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOIO4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOIO5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOIO6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOIO7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDOIO8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Loan To Intercompany</td>
    <td align="center">
        <asp:TextBox ID="txtLTI1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLTI2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLTI3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLTI4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLTI5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLTI6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLTI7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLTI8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Interest Received</td>
    <td align="center">
        <asp:TextBox ID="txtIR1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIR2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIR3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIR4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIR5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIR6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIR7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtIR8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Dividend Received</td>
    <td align="center">
        <asp:TextBox ID="txtDR1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDR2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDR3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDR4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDR5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDR6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDR7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDR8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Dividend Paid</td>
    <td align="center">
        <asp:TextBox ID="txtDP1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDP2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDP3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDP4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDP5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDP6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDP7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtDP8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left" class="tbLabel"><i>Net Cash flow from Investing</i></td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblNCFFI1" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblNCFFI2" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFFI3" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFFI4" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFFI5" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFFI6" runat="server" Font-Bold="true"></asp:Label>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFFI7" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFFI8" runat="server" Font-Bold="true"></asp:Label>
    </td>
    </tr>
    
    <tr class="tAltRow" style="background-color:#E9EAEB;">
    <td colspan="9"><strong><u>Less/Add Financing Activities</u></strong></td>
    </tr>
    
    <tr>
    <td align="left">Proceeds of Bank Loans</td>
    <td align="center">
        <asp:TextBox ID="txtPOBL1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOBL2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOBL3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOBL4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOBL5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOBL6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOBL7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOBL8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Repayment of Bank Loans</td>
    <td align="center">
        <asp:TextBox ID="txtROBL1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROBL2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROBL3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROBL4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROBL5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROBL6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROBL7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROBL8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Repayment of Trade Financing</td>
    <td align="center">
        <asp:TextBox ID="txtROTF1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROTF2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROTF3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROTF4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROTF5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROTF6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROTF7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROTF8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Payment of Interests</td>
    <td align="center">
        <asp:TextBox ID="txtPOI1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOI2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOI3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOI4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOI5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOI6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOI7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPOI8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">New Capital/Convertible Loan</td>
    <td align="center">
        <asp:TextBox ID="txtNCCL1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtNCCL2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtNCCL3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtNCCL4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtNCCL5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtNCCL6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtNCCL7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtNCCL8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Loan From Intercompany</td>
    <td align="center">
        <asp:TextBox ID="txtLFI1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLFI2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLFI3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLFI4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLFI5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLFI6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLFI7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtLFI8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Repayment of Intercompany Loan</td>
    <td align="center">
        <asp:TextBox ID="txtROIL1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROIL2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROIL3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROIL4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROIL5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROIL6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROIL7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtROIL8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left" class="tbLabel"><i>Net Cash Flow From Financing</i></td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblNCFFF1" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblNCFFF2" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFFF3" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFFF4" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFFF5" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFFF6" runat="server" Font-Bold="true"></asp:Label>
    </td>   
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFFF7" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFFF8" runat="server" Font-Bold="true"></asp:Label>
    </td>
    </tr>
    
    <tr>
    <td align="left" class="tbLabel"><i>Net Cash Flow Surplus/(Deficit)</i></td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblNCFSD1" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblNCFSD2" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFSD3" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFSD4" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFSD5" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFSD6" runat="server" Font-Bold="true"></asp:Label>
    </td>   
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFSD7" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCFSD8" runat="server" Font-Bold="true"></asp:Label>
    </td>
    </tr>  
    
    
    <tr>
    <td colspan="8">Prepared By: <asp:Label ID="lblPreparedBy" runat="server"></asp:Label></td>
    <td align="right">
    <asp:Button runat="server" ID="btnSubmitData" CssClass="button" Text="Submit" OnClick="btnSubmitData_Click" ValidationGroup="Value" />

    </td>
    </tr> 
    
    
    
</table>

<asp:Button runat="server" ID="HiddenForModalSave" style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="HiddenForModalSave" PopupControlID="PNL" BackgroundCssClass="modalBackground" BehaviorID="btnOkPopupBehavior" />
    <asp:Panel ID="PNL" runat="server" style="display:none; width:100px; background-color:White; border-width:2px; border-color:#005DAA; border-style: solid; padding:20px;">
    <table align="center" style="margin-left: auto; margin-right: auto; margin-top: 0px; margin-bottom: 0px;" >
        <tr align="center">             
        <td>
        <img src = "../../images/loading.gif" />                                                         
        </td>
        </tr>
    </table>                  
    </asp:Panel> 

</ContentTemplate>
    <Triggers>
             <asp:AsyncPostBackTrigger controlid="btnSubmit" EventName="Click" />
            <asp:AsyncPostBackTrigger controlid="btnSubmitData" EventName="Click" />
    </Triggers> 
</asp:UpdatePanel>
				   


       </div>
       <br />
    </form>
</body>
</html>
