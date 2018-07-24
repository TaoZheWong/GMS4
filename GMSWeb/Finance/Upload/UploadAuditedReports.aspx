<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadAuditedReports.aspx.cs" Inherits="GMSWeb.Organization.Upload.UploadAuditedReports" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Data - Audited Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="GroupContentBar">
        <h3>Data &gt; Audited Report</h3>
        
        Upload scanned audited report.
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
	    <table class="tGroupTable" style="width: 100%">
            <tr>
			    <td>
				    <br />
				    <div id="Resultslbl" style="text-align:right;width: 400px" visible="false" runat="server">
						        Results:
								<asp:Label ID="lblTotalRecordsFound" runat="server" Font-Bold="true" />
								</div>
				    <div class="tGroupTable">
							    <asp:DataGrid ID="dgModify" runat="server" AutoGenerateColumns="false" ShowFooter="false"
								    GridLines="none" DataKeyField="ReportId" 
								     OnItemDataBound="dgModify_ItemDataBound" OnDeleteCommand="dgModify_DeleteCommand" OnItemCommand="dgModify_ViewCommand"
								    CellPadding="4" CellSpacing="0" CssClass="tTable tBorder" AllowCustomPaging="True" AllowPaging="True" pagesize="10">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# (Container.ItemIndex + 1) + ( dgModify.PageSize * dgModify.CurrentPageIndex )%>
											    .</ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Report" ItemStyle-Width="250px">
										    <ItemTemplate>
                                                <asp:LinkButton ID="lnkReport" runat="server"  CommandName="View" EnableViewState="false"
												    CausesValidation="false"><span><%# Eval("FileName")%></span></asp:LinkButton>
												    <input type="hidden" id="hidFileName" runat="server" value='<%# Eval("FileName")%>' />
										    </ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" 
									        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
									        <ItemTemplate>
									            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
									                CausesValidation="false" CssClass="DeleteButt"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
									        </ItemTemplate>
									    </asp:TemplateColumn>
								    </Columns>
								    <HeaderStyle CssClass="tGroupHeader" />
								    <AlternatingItemStyle CssClass="tGroupAltRow" />
								    <FooterStyle CssClass="tGroupFooter" />
								    <PagerStyle Visible="False" />
							    </asp:DataGrid>
				    </div>
				    <div id="Pageslbl" style="text-align:center;width: 400px" visible="false" runat="server">
							        <asp:LinkButton ID="lnkFirst" runat="server" CommandName="First" OnCommand="SearchResults_PageNavigate">First</asp:LinkButton>
								    <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Previous" OnCommand="SearchResults_PageNavigate">Previous</asp:LinkButton>
								    <asp:Label ID="lblPage" runat="server" Font-Bold="true" />
								    <asp:LinkButton ID="lnkNext" runat="server" CommandName="Next" OnCommand="SearchResults_PageNavigate">Next</asp:LinkButton>
								    <asp:LinkButton ID="lnkLast" runat="server" CommandName="Last" OnCommand="SearchResults_PageNavigate">Last</asp:LinkButton>
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
