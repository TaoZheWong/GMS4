<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarControl.ascx.cs" Inherits="GMSWeb.CustomCtrl.CalendarControl" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<table cellpadding="0" cellspacing="0" border="0">
	<tr>
		<td>
			<asp:TextBox CssClass="textbox" ID="txtCalendarField" runat="server" Columns="10" MaxLength="15"/>
		</td>
		<td style="padding-left:1px;">
			<rjs:PopCalendar ID="calCalendarCtrl" runat="server" TextMessage="Invalid Date."
				RequiredDate="False" Buttons="[<][>][m][y]" BorderWidth="1px" Culture="en-AU English (Australia)"
				RequiredDateMessage="date is required" Fade="0.1" Format="dd MMM yyyy" Move="false"
				ShowWeekend="True" Shadow="false" Control="txtCalendarField" InvalidDateMessage="The Date is invalid!"
				BorderStyle="Solid" BorderColor="Black" BlankFieldText="Select a Date" BackColor="Yellow"
				From-Message="" Separator="-" ShowGoodFriday="true"></rjs:PopCalendar>
		</td>
	</tr>
</table>