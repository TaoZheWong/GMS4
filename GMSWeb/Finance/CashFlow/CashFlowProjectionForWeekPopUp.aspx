<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CashFlowProjectionForWeekPopUp.aspx.cs" Inherits="GMSWeb.Finance.CashFlow.CashFlowProjectionForWeekPopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>Cash Flow Projections</title>   
    <link href="../../App_Themes/default/style%20-%20Copy.css" type="text/css" rel="stylesheet">
    <link href="../../App_Themes/default/style.css" type="text/css" rel="stylesheet">
    <link href="../../App_Themes/default/style2011.css" type="text/css" rel="stylesheet">
    <script type='text/javascript' src='/GMS3/scripts/popcalendar.js'></script>
    <script type='text/javascript' src='/GMS3/scripts/date.js'></script> 
    <script type='text/javascript'>
	function btnSubmit_OnClick()
	{
		var YY = document.getElementById('txtAsOfDate').value.substring(6); 
		if (document.getElementById('txtAsOfDate').value.substring(3,4) == '0') 
			var MM = parseInt(document.getElementById('txtAsOfDate').value.substring(4,5))-1; 
		else 
			var MM = parseInt(document.getElementById('txtAsOfDate').value.substring(3,5))-1; 
		var DD = parseInt(document.getElementById('txtAsOfDate').value.substring(0,2)); 
		var selectedDate = new Date(YY,MM,DD); 
		var tempDate = selectedDate.addMonths(1); 
		var tempDate1 = new Date(tempDate.getYear(),tempDate.getMonth(),1); 
		tempDate1.setDate(tempDate1.getDate() - 1); 
		var lastDay = tempDate1.getDate(); 
		if (DD != lastDay) 
		{
			alert('You must select the last day of the month.'); 
			return false; 
		}
		else 
			return true; 
	}
	
</script> 
    
       
</head>
<body style="background-image:none; text-align: left" >
    <form id="form1" runat="server">
       <div>
       
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
      
<asp:UpdatePanel runat="server" id="upCashFlowData" updatemode="Conditional">      
<ContentTemplate>
<table class="tTable1" style="margin-left: 8px;" cellspacing="5" cellpadding="5" border="0" width="99%">
                 <tr>
                    <td class="tbLabel">
                        As Of Date</td>
                    <td style="width:5%">
                        :</td>
                    <td style="width:10%">
                        <asp:TextBox runat="server" ID="txtAsOfDate" MaxLength="10" Columns="10" onfocus="select();"
                                    CssClass="textbox"></asp:TextBox>
                                <img id="imgCalendarAsOfDate" src="../../images/imgCalendar.gif" onclick="showCalendar(this, document.getElementById('txtAsOfDate'), 'dd/mm/yyyy', null, 1);"
                                    height="20" width="17" alt="" align="absMiddle" border="0">
                                    <input type="hidden" id="hidAsOfDate" runat="server"  />
                                    <input type="hidden" id="hidCoyID" runat="server"  />
                    </td>
                    <td>
	                    <asp:Button runat="server" ID="btnSubmit" CssClass="button" Text="Submit" OnClick="btnSubmit_Click" OnClientClick="return btnSubmit_OnClick();" />
	                </td>
                </tr>    
                
	
    </table>
    <br />
<!--    
<table class="tTable tBorder" style="margin-left: 8px" cellspacing="2" cellpadding="2" border="0" width="98%"> 
<tr class="tHeader">
    <td class="tbLabel" align="center" >CASH BALANCE & UTILISATION <br />银行现金应用报告</td>
<tr/>
<tr>   
    <td align="left">Petty Cash 零用现金</td>
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtPC1" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtPC2" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td> 
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtPC3" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>  
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtPC4" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td> 
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtPC5" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td> 
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtPC6" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>  
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtPC7" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td> 
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtPC8" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtPC9" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtPC10" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>     
<tr>
<tr>   
    <td align="left">Cash in Bank 银行存款</td>
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtCIB1" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtCIB2" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td> 
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtCIB3" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>  
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtCIB4" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td> 
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtCIB5" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td> 
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtCIB6" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>  
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtCIB7" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td> 
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtCIB8" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtCIB9" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtCIB10" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>     
<tr>
<tr>   
    <td align="left">Fixed Deposit 定期存款</td>
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtFD1" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtFD2" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td> 
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtFD3" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>  
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtFD4" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td> 
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtFD5" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td> 
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtFD6" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>  
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtFD7" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td> 
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtFD8" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtFD9" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>
    <td align="left" style="padding-left:5px;"><asp:TextBox ID="txtFD10" runat="server" CssClass="textbox" Columns="8"></asp:TextBox></td>     
<tr>
</table>
-->
<table class="tTable tBorder" style="margin-left: 8px" cellspacing="2" cellpadding="2" border="0" width="98%">
    <tr class="tHeader">
    <td class="tbLabel" rowspan= "2" align="center" >CASH FLOW PROJECTION  <br />现金流报告</td>
    <td align="center" class="tbLabel" colspan= "5"><asp:Label ID="lblMonth1" runat="server"></asp:Label></td>    
    <td align="center" rowspan = "2">
            <asp:Label ID="lblMonth2" runat="server"></asp:Label>            
        </td>
        <td align="center"  rowspan = "2">
            <asp:Label ID="lblMonth3" runat="server"></asp:Label>         
        </td>
        <td align="center"  rowspan = "2">
            <asp:Label ID="lblMonth4" runat="server"></asp:Label>       
        </td> 
    </tr>  
    <tr class="tHeader">
   
    <td align="center">    
     
       <asp:Label ID="lblWeek1" runat="server">Week 1</asp:Label> 
    </td>
    <td align="center">       
      <asp:Label ID="lblWeek2" runat="server">Week 2</asp:Label>   
    </td>
    <td align="center">
       <asp:Label ID="lblWeek3" runat="server">Week 3</asp:Label>         
    </td>
    <td align="center">     
       <asp:Label ID="lblWeek4" runat="server">Week 4</asp:Label>      
    </td>
    <td align="center">      
       <asp:Label ID="lblWeek5" runat="server" >Week 5</asp:Label>              
    </td>
    
    </tr>
    
    <tr class="tAltRow" style="background-color:#E9EAEB;">
    <td colspan="9"><strong><u>Cash Inflow from Operating Activities</u></strong></td>
    </tr>
    
    <tr>
    <td align="left">Collection from Sales 货款回笼</td>
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
    <td align="left">Other Income 其他收入</td>
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
    <td colspan="9"><strong><u>Cash Outflow from Operating Activities</u></strong></td>
    </tr>
    
    <tr>
    <td align="left">Payment to Overseas Suppliers 支付海外供应商</td>
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
    <td align="left">Payment to Local Suppliers 支付本地供应商</td>
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
    <td align="left">Personnel Expenses 人事相关费用</td>
    <td align="center">
        <asp:TextBox ID="txtPE1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPE2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPE3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPE4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPE5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPE6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPE7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPE8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
    
    <tr>
    <td align="left">Carriage/Transportation 货代/交通</td>
    <td align="center">
        <asp:TextBox ID="txtCT1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCT2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCT3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCT4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCT5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCT6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCT7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtCT8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr>
   
    <tr>
    <td align="left">Property/ Equipment Expenses 房产/设备费用</td>
    <td align="center">
        <asp:TextBox ID="txtPEE1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPEE2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPEE3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPEE4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPEE5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPEE6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPEE7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtPEE8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    </tr> 
    
    <tr>
    <td align="left">Other Expenses 其他费用</td>
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
    <td align="left">Taxes payment (GST) (增值税)</td>
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
    <td align="left">Taxes payment (Corporate) (企业所得税)</td>
    <td align="center">
        <asp:TextBox ID="txtTPC1" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTPC2" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate2_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTPC3" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate3_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTPC4" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate4_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTPC5" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate5_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTPC6" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate6_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTPC7" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate7_OnTextChanged" AutoPostBack="true"></asp:TextBox>
    </td>
    <td align="center">
        <asp:TextBox ID="txtTPC8" runat="server" CssClass="textbox" Columns="12" OnTextChanged="Calculate8_OnTextChanged" AutoPostBack="true"></asp:TextBox>
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
    <td colspan="9"><strong><u>Cash Flow from Investing Activities</u></strong></td>
    </tr>
    
    <tr>
    <td align="left">Purchase of Fixed Assets 购买固定资产</td>
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
    <td align="left">Investments/Other Assets 投资/购买其他资产</td>
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
    <td align="left">Disposal of Fixed Assets 出售固定资产	</td>
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
    <td align="left">Disposal of Investments/Others 出售投资/其他资产	</td>
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
    <td align="left">Loan To Intercompany 关联公司贷款</td>
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
    <td align="left">Interest Received 已收利息</td>
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
    <td align="left">Dividends Received 已收股息</td>
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
    <td align="left">Dividends Paid 已付股息</td>
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
    <td align="left" class="tbLabel"><i>Net Cash Flow From Investing</i></td>
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
    <td colspan="9"><strong><u>Cash Flow from Financing Activities</u></strong></td>
    </tr>
    
    <tr>
    <td align="left">Proceeds of Bank Loans 新银行貸款	</td>
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
    <td align="left">Repayment of Bank Loans 偿还银行貸款</td>
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
    <td align="left">Repayment of Trade Financing 偿还贸易融资</td>
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
    <td align="left">Payment of Interests 支付利息</td>
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
    <td align="left">New Capital/Convertible Loan 新资本/可换股贷款</td>
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
    <td align="left">Loan From Intercompany 关联公司贷款</td>
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
    <td align="left">Repayment of Intercompany Loan 偿还关联公司貸款</td>
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
    <!--
    <tr>
    <td align="left" class="tbLabel"><i>Net Cash Flow 净现金流</i></td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblNCF1" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblNCF2" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCF3" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCF4" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCF5" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCF6" runat="server" Font-Bold="true"></asp:Label>
    </td>   
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCF7" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNCF8" runat="server" Font-Bold="true"></asp:Label>
    </td>
    </tr>  
   
   <tr>
    <td align="left" class="tbLabel"><i>Add: Total Available Fund 可用资金</i></td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblTAF1" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblTAF2" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTAF3" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTAF4" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTAF5" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTAF6" runat="server" Font-Bold="true"></asp:Label>
    </td>   
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTAF7" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblTAF8" runat="server" Font-Bold="true"></asp:Label>
    </td>
    </tr>   
   
   <tr>
    <td align="left" class="tbLabel"><i>Net Surplus (Deficit) 净可用资金(不足额)</i></td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblNSD1" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
        <asp:Label ID="lblNSD2" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNSD3" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNSD4" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNSD5" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNSD6" runat="server" Font-Bold="true"></asp:Label>
    </td>   
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNSD7" runat="server" Font-Bold="true"></asp:Label>
    </td>
    <td align="left" style="padding-left:5px;">
       <asp:Label ID="lblNSD8" runat="server" Font-Bold="true"></asp:Label>
    </td>
    </tr>    
    -->
    
    <tr>
    <td colspan="8">Prepared By: <asp:Label ID="lblPreparedBy" runat="server"></asp:Label></td>
    <td align="right">
    <asp:Button runat="server" ID="btnSubmitData" CssClass="button" Text="Save" OnClick="btnSubmitData_Click" ValidationGroup="Value" />

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

