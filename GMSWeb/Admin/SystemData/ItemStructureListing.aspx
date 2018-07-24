<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemStructureListing.aspx.cs" Inherits="GMSWeb.Admin.SystemData.ItemStructureListing" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>System - Item Structure</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ContentBar">
        <h3>System &gt; Item Structure</h3>
        Maintain a list of item structure for financial data, used for different purposes, e.g. budget for P&amp;L,
        budget for Listing of Expenses etc.
        <br />
        <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
        <table class="tTable" style="width: 100%">
            <tr>
			    <td>
				    <br />
				    <asp:Label ID="lblPurpose" runat="server" Text="Purpose"  />:
				    <asp:DropDownList ID="ddlPurpose" runat="server" DataTextField="ItemPurposeName" DataValueField="ItemPurposeID" AutoPostBack="true" 
				            OnSelectedIndexChanged="ddlPurpose_SelectedIndexChanged" />
				            <br /><br />
				    <div class="tTable">
					    <atlas:UpdatePanel ID="udpItemStructureUpdater" Visible="false" runat="server" Mode="conditional">
						    <ContentTemplate>
							    <asp:DataGrid ID="dgItemStructure" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="ItemStructureID" OnCancelCommand="dgItemStructure_CancelCommand" OnEditCommand="dgItemStructure_EditCommand"
								    OnItemDataBound="dgItemStructure_ItemDataBound" OnUpdateCommand="dgItemStructure_UpdateCommand" 
								    OnItemCommand="dgItemStructure_CreateCommand" OnDeleteCommand="dgItemStructure_DeleteCommand"
								    GridLines="none"
								    CellPadding="5" CellSpacing="0" CssClass="tTable tBorder">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%#  (Container.ItemIndex + 1) + (dgItemStructure.PageSize * dgItemStructure.CurrentPageIndex)%>
												.</ItemTemplate>
									    </asp:TemplateColumn>
									    
									    <asp:TemplateColumn HeaderText="Item" ItemStyle-Width="200px">
										    <ItemTemplate>
											    <asp:Label ID="lblItem" runat="server">
											        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
												    CausesValidation="false"></asp:LinkButton>
											    </asp:Label>
											    <input type="hidden" runat="server" id="hidItemId" value='<%# Eval("ItemID") %>' />
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:DropDownList ID="ddlEditName" runat="server" DataTextField="ItemName" DataValueField="ItemID" />
											    <input type="hidden" runat="server" id="hidItemId" value='<%# Eval("ItemID") %>' />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:DropDownList ID="ddlNewName" runat="server" DataTextField="ItemName" DataValueField="ItemID" />
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
		    <tr>
		        <td>
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
		        </td>
		    </tr>
	    </table>
	    <br />   
	     
    </div>
    </form>
</body>
</html>
