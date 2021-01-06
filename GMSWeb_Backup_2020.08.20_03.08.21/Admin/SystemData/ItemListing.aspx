<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemListing.aspx.cs" Inherits="GMSWeb.Admin.SystemData.ItemListing" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>System - Item</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ContentBar">
        <h3>System &gt; Item</h3>
        Maintain a list of items for financial data.
        <br />
        <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
        <table class="tTable" style="width: 100%">
            <tr>
			    <td>
				    <br />
				    <div class="tTable">
					    <atlas:UpdatePanel ID="udpItemUpdater" runat="server" Mode="conditional">
						    <ContentTemplate>
						        <div align="right" style="width: 360px;">
						        Results:
								<asp:Label ID="lblTotalRecordsFound" runat="server" Font-Bold="true" />
								</div>
							    <asp:DataGrid ID="dgItem" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="ItemId" OnCancelCommand="dgItem_CancelCommand" OnEditCommand="dgItem_EditCommand"
								    OnUpdateCommand="dgItem_UpdateCommand" OnItemCommand="dgItem_CreateCommand" AllowPaging="true"
								    AllowCustomPaging="true" PageSize="10"
								    GridLines="none" OnItemDataBound="dgItem_ItemDataBound" OnDeleteCommand="dgItem_DeleteCommand"
								    CellPadding="5" CellSpacing="0" CssClass="tTable tBorder">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%#  ( Container.ItemIndex + 1 ) + ( dgItem.PageSize * dgItem.CurrentPageIndex )%>
												.</ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Item" ItemStyle-Width="200px">
										    <ItemTemplate>
											    <asp:Label ID="lblItem" runat="server">
												    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
												    CausesValidation="false"><span><%# Eval("ItemName")%></span></asp:LinkButton>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditName" runat="server" Columns="20" MaxLength="50" Text='<%# Eval("ItemName") %>' />
										        <asp:RequiredFieldValidator ID="rfvEditName" runat="server" ControlToValidate="txtEditName"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewName" runat="server" Columns="20" MaxLength="50" />
											    <asp:RequiredFieldValidator ID="rfvNewName" runat="server" ControlToValidate="txtNewName"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
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
										    <FooterTemplate>
											<asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
												ValidationGroup="valGrpNewRow" CssClass="NewButt"><span>Add</span></asp:LinkButton>
										</FooterTemplate>
									    </asp:TemplateColumn>
								    </Columns>
									<PagerStyle Visible="false" />
								    <HeaderStyle CssClass="tHeader" />
								    <AlternatingItemStyle CssClass="tAltRow" />
								    <FooterStyle CssClass="tFooter" />
							    </asp:DataGrid>
							    <div align="center" style="width: 360px;">
							        <asp:LinkButton ID="lnkFirst" runat="server" CommandName="First" OnCommand="SearchResults_PageNavigate">First</asp:LinkButton>
								    <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Previous" OnCommand="SearchResults_PageNavigate">Previous</asp:LinkButton>
								    <asp:Label ID="lblPage" runat="server" Font-Bold="true" />
								    <asp:LinkButton ID="lnkNext" runat="server" CommandName="Next" OnCommand="SearchResults_PageNavigate">Next</asp:LinkButton>
								    <asp:LinkButton ID="lnkLast" runat="server" CommandName="Last" OnCommand="SearchResults_PageNavigate">Last</asp:LinkButton>
						        </div>
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
