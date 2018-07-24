<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Old UploadAudit.aspx.cs" Inherits="GMSWeb.Organization.Upload.UploadAudit" %>

<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Data - Audit</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="GroupContentBar">
        <h3>Data &gt; Audit</h3>
        
        Upload yearly audited data for different purposes using Excel files. If there is data already existed for the date specified below, data will be overwritten.
        <br />See the following files for examples: 
        <br /><br />
        <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
        
        <table class="tblGroupTable" style="width: 90%">
            <tr>
			    <td>
				    <br />
				    <div class="tTable">
				        <table>
				            <tr>
				                <td colspan="2"><A href="<%= Request.ApplicationPath %>/Documents/Audit.xls">
							        Audit_BalanceSheet.xls</A>
							    </td>
				            </tr>
				            <tr>
				                <td colspan="2"><A href="<%= Request.ApplicationPath %>/Documents/Audit_P&L.xls">
							        Audit_P & L.xls</A>
							    </td>
				            </tr>
				            <tr>
				                <td>&nbsp;
							    </td>
				            </tr>
				            <tr id="trReportDateTo" runat="server">
							    <td>Audit Date</td>
							    <td colspan="2">
								    <uctrl:Calendar ID="calAuditDate" runat="server" InvalidDateMessage="Invalid Date"
									    IsValueRequired="false" /></td>
						    </tr>
				            <tr>
				                <td>
					                <asp:Label ID="lblPurpose" runat="server">Purpose</asp:Label>
				                </td>
				                <td style="width: 470px">
				                    <asp:DropDownList ID="ddlPurpose" runat="server" DataTextField="ItemPurposeName" DataValueField="ItemPurposeID" />
				                </td>
				            </tr>
				            <tr>
				                <td>
                                    <asp:Label ID="lblLocation" runat="server">Location</asp:Label></td>
				                <td style="width: 470px">
                                    <asp:FileUpload ID="FileUpload1" runat="server" Width="355px" /></td>
				            </tr>
				            <tr><td></td>
				                <td>
				                    <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload" CssClass="button" OnClick="btnUpload_Click" />
				                </td>
				            </tr>
				            <tr>
					            <td colspan="2">
						            <IFRAME ID=IFrame1 FRAMEBORDER=0 SCROLLING=YES Runat="Server" width=100% Style="display:none"></IFRAME>	
					            </td>
				            </tr>
				            <tr>
				                <td colspan="2">
				                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
				                </td>
				            </tr>
				        </table>
				    </div>
			    </td>
		    </tr>
	    </table>
	    <div class="TABCOMMAND">
		    <atlas:UpdatePanel ID="udpMsgUpdater" runat="server" Mode="Always">
			    <ContentTemplate>
				    <ul>
					    <li></li>
				    </ul>
				    <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
			    </ContentTemplate>
		    </atlas:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
