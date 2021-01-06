<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadProductBudget.aspx.cs" Inherits="GMSWeb.Finance.Upload.UploadProductBudget" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Data - Product Budget</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="GroupContentBar">
        <h3>Data &gt; Product Budget</h3>
        
        Upload yearly product sales budget data using Excel files. 
        If there is data already existed for the year specified below, it will be overwritten. 
        <br /><br />
        <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
        
        <table class="tblGroupTable" style="width: 90%">
            <tr>
			    <td>
				    <br />
				    <div class="tTable">
				        <table>
				            <tr>
				                <td>
					                <asp:Label ID="lblYear" runat="server">Year</asp:Label>
				                </td>
				                <td style="width: 470px">
				                    <asp:DropDownList ID="ddlYear" runat="server" DataTextField="Year" DataValueField="Year" />
				                </td>
				            </tr>
				            <tr>
				                <td>
					                <asp:Label ID="lblDepartment" runat="server">Division</asp:Label>
				                </td>
				                <td style="width: 470px">
				                    <asp:DropDownList ID="ddlDivision" runat="server" DataTextField="DivisionName" DataValueField="DivisionID" />
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
				                    <asp:Button ID="btnUpload" CssClass="button" runat="server" CausesValidation="true" Text="Upload" OnClick="btnUpload_Click" />
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
