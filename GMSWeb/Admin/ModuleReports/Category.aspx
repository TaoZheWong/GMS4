<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="GMSWeb.Reports.Setup.ModuleCategory" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Module Reports - Category</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ContentBar">
        <h3>Module Reports &gt; Category</h3>
        Add new category to the report or click on a category to modify.
        <br />
        <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
        <table class="tTable" style="width: 100%">
            <tr>
			    <td>
				    <br />
				    <div class="tTable">
					    <atlas:UpdatePanel ID="udpCategoryUpdater" runat="server" Mode="conditional">
						    <ContentTemplate>
							    <asp:DataGrid ID="dgCategory" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="ReportCategoryId" OnCancelCommand="dgCategory_CancelCommand" OnEditCommand="dgCategory_EditCommand"
								    OnUpdateCommand="dgCategory_UpdateCommand" OnItemCommand="dgCategory_CreateCommand"
								    GridLines="none" OnItemDataBound="dgCategory_ItemDataBound" OnDeleteCommand="dgCategory_DeleteCommand"
								    CellPadding="4" CellSpacing="0" CssClass="tTable tBorder">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# Container.ItemIndex + 1%>
											    .</ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Category" ItemStyle-Width="80px">
										    <ItemTemplate>
											    <asp:Label ID="lblName" runat="server">
												    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
												    CausesValidation="false"><span><%# Eval("Name")%></span></asp:LinkButton>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditName" runat="server" Columns="20" MaxLength="50" Text='<%# Eval("Name") %>' />
										        <asp:RequiredFieldValidator ID="rfvEditName" runat="server" ControlToValidate="txtEditName"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewName" runat="server" Columns="20" MaxLength="50" />
											    <asp:RequiredFieldValidator ID="rfvNewName" runat="server" ControlToValidate="txtNewName"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Module Category" ItemStyle-Width="80px">
										    <ItemTemplate>
											    <asp:Label ID="lblModuleCat" runat="server">
												    <%# Eval("ModuleCategoryObject.Name")%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:DropDownList ID="ddlEditModuleCategory" runat="Server" DataTextField="Name" DataValueField="ModuleCategoryID" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:DropDownList ID="ddlNewModuleCategory" runat="Server" DataTextField="Name" DataValueField="ModuleCategoryID" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Display Sequence" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
										    <ItemTemplate>
										        <asp:Label ID="lblSeqID" runat="server">
										           <%# Eval( "SeqID" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:DropDownList ID="ddlEditSeqID" runat="Server" DataTextField="SeqID" DataValueField="SeqID" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:DropDownList ID="ddlNewSeqID" runat="Server" DataTextField="SeqID" DataValueField="SeqID" />
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
