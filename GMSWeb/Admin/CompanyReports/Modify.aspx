<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Modify.aspx.cs" Inherits="GMSWeb.Reports.Setup.Modify" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Company Reports - Modify</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ContentBar">
        <h3>Company Reports &gt; Modify</h3>
        Click on a report to modify.
        <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
        <table class="tTable" style="width: 100%">
            <tr>
			    <td>
				    <br />
				    <div class="tTable">
					    <atlas:UpdatePanel ID="udpModifyUpdater" runat="server" Mode="conditional">
						    <ContentTemplate>
							    <asp:DataGrid ID="dgModify" runat="server" AutoGenerateColumns="false" ShowFooter="false"
								    GridLines="none" DataKeyField="ReportId" OnCancelCommand="dgModify_CancelCommand" OnEditCommand="dgModify_EditCommand"
								    OnUpdateCommand="dgModify_UpdateCommand" OnItemDataBound="dgModify_ItemDataBound" OnDeleteCommand="dgModify_DeleteCommand"
								    CellPadding="4" CellSpacing="0" CssClass="tTable tBorder">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# Container.ItemIndex + 1%>
											    .</ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Category" ItemStyle-Width="100px">
										    <ItemTemplate>
											    <asp:Label ID="lblName" runat="server">
												    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
												    CausesValidation="false"><span><%# Eval("ReportCategoryObject.Name")%></span></asp:LinkButton>
											    </asp:Label>
										    </ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Report" ItemStyle-Width="250px">
										    <ItemTemplate>
											    <asp:Label ID="lblReport" runat="server">
												    <%# Eval("Description")%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditName" runat="server" Columns="20" MaxLength="50" Text='<%# Eval("Description") %>' />
											    <asp:RequiredFieldValidator ID="rfvEditName" runat="server" ControlToValidate="txtEditName"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
										    </EditItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Active" ItemStyle-Width="50px">
										    <ItemTemplate>
											     <%# ( (bool)Eval( "IsActive" ) ) ? "Yes" : "No"%>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:CheckBox ID="chkEditIsActive" runat="server" Checked='<%# Eval( "IsActive" )%>' />
										    </EditItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" 
									        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
									        <ItemTemplate>
									            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
									                CausesValidation="false" CssClass="DeleteButt"><span>&nbsp;&nbsp;Delete</span></asp:LinkButton>
									        </ItemTemplate>
										    <EditItemTemplate>
											    <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
												    ValidationGroup="valGrpEditRow" CssClass="SaveButt"><span>Save</span></asp:LinkButton>
											    <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
												    CausesValidation="false" CssClass="CancelButt"><span>Cancel</span></asp:LinkButton>
										    </EditItemTemplate>
									    </asp:TemplateColumn>
								    </Columns>
								    <HeaderStyle CssClass="tGroupHeader" />
								    <AlternatingItemStyle CssClass="tGroupAltRow" />
								    <FooterStyle CssClass="tGroupFooter" />
							    </asp:DataGrid>
						    </ContentTemplate>
					    </atlas:UpdatePanel>
				    </div>
			    </td>
		    </tr>
	    </table>
        <br />
        <div class="TABCOMMAND">
            <atlas:UpdatePanel ID="udpMsgUpdater" runat="server" Mode="Always">
	            <ContentTemplate>
		            <ul>
			            <li>&nbsp;</li>
		            </ul>
		            <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
	            </ContentTemplate>
            </atlas:UpdatePanel>
        </div>   
    </div>
    </form>
</body>
</html>
