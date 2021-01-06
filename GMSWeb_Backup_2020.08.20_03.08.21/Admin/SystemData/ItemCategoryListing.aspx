<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemCategoryListing.aspx.cs" Inherits="GMSWeb.Admin.SystemData.ItemCategoryListing" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>System - Item Category</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ContentBar">
        <h3>System &gt; Item Category</h3>
        Maintain a list of item categories for financial data.
        <br />
        <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
        <table class="tTable" style="width: 100%">
            <tr>
			    <td>
				    <br />
				    <div class="tTable">
					    <atlas:UpdatePanel ID="udpItemCategoryUpdater" runat="server" Mode="conditional">
						    <ContentTemplate>
							    <asp:DataGrid ID="dgItemCategory" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="ItemCategoryId" OnCancelCommand="dgItemCategory_CancelCommand" OnEditCommand="dgItemCategory_EditCommand"
								    OnUpdateCommand="dgItemCategory_UpdateCommand" OnItemCommand="dgItemCategory_CreateCommand"
								    GridLines="none" OnItemDataBound="dgItemCategory_ItemDataBound" OnDeleteCommand="dgItemCategory_DeleteCommand"
								    CellPadding="5" CellSpacing="0" CssClass="tTable tBorder">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# Container.ItemIndex + 1%>
											    .</ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Item Category" ItemStyle-Width="200px">
										    <ItemTemplate>
											    <asp:Label ID="lblItemCategory" runat="server">
												    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
												    CausesValidation="false"><span><%# Eval("ItemCategoryName")%></span></asp:LinkButton>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditName" runat="server" Columns="20" MaxLength="50" Text='<%# Eval("ItemCategoryName") %>' />
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
								    <HeaderStyle CssClass="tHeader" />
								    <AlternatingItemStyle CssClass="tAltRow" />
								    <FooterStyle CssClass="tFooter" />
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
