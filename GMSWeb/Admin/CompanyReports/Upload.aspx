<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="GMSWeb.Reports.Setup.Upload" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Company Reports - Upload</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ContentBar">
        <h3>Company Reports &gt; Upload</h3>
        
        Upload a new report to the system
        <br /><br />
        <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
        
        <table class="tblTable" style="width: 90%">
            <tr>
			    <td>
				    <br />
				    <div class="tblTable">
				        <table>
				            <tr>
				                <td>
					                <asp:Label ID="lblCategoryName" runat="server">Category</asp:Label>
				                </td>
				                <td>
				                    <asp:DropDownList ID="ddlCategoryName" runat="server" DataTextField="Name" DataValueField="ReportCategoryId" />
				                </td>
				            </tr>
				            
				            <tr>
				                <td>
					                <asp:Label ID="lblReportName" runat="server">Report Name</asp:Label>
				                </td>
				                <td>
				                    <asp:TextBox ID="txtReportName" runat="server" Columns="20" MaxLength="50" />
									<asp:RequiredFieldValidator ID="rfvReportName" runat="server" ControlToValidate="txtReportName"
										    ErrorMessage="*" Display="dynamic" />
				                </td>
				            </tr>
				            
				            <tr>
				                <td>
                                    <asp:Label ID="lblLocation" runat="server">Location</asp:Label></td>
				                <td>
                                    <asp:FileUpload ID="FileUpload1" runat="server" /></td>
				            </tr>
				            <tr><td></td>
				                <td>
				                    <asp:Button ID="btnUpload" CssClass="button" runat="server" CausesValidation="true" Text="Upload" OnClick="btnUpload_Click" />
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
