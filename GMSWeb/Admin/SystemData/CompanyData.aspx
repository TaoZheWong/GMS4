<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyData.aspx.cs" Inherits="GMSWeb.Admin.SystemData.CompanyData" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>System - Company</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ContentBar">
        <h3>System &gt; Company</h3>
        Maintain a list of companies for the group.
        <br />
        <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
        <table class="tTable" style="width: 100%">
            <tr>
			    <td>
				    <br />
				    <div class="tTable">
					    <atlas:UpdatePanel ID="udpCompanyUpdater" runat="server" Mode="conditional">
						    <ContentTemplate>
							    <asp:DataGrid ID="dgCompany" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="Id" OnCancelCommand="dgCompany_CancelCommand" OnEditCommand="dgCompany_EditCommand"
								    OnUpdateCommand="dgCompany_UpdateCommand" OnItemCommand="dgCompany_CreateCommand"
								    GridLines="none" OnItemDataBound="dgCompany_ItemDataBound" OnDeleteCommand="dgCompany_DeleteCommand"
								    CellPadding="5" CellSpacing="0" CssClass="tTable tBorder">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# Container.ItemIndex + 1%>
											    .</ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Country" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
										    <ItemTemplate>
										        <asp:Label ID="lblCountry" runat="server">
										           <%# Eval( "CountryName" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:DropDownList ID="ddlEditCountry" runat="Server" DataTextField="Name" DataValueField="CountryID" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:DropDownList ID="ddlNewCountry" runat="Server" DataTextField="Name" DataValueField="CountryID" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Division" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
										    <ItemTemplate>
										        <asp:Label ID="lblDivision" runat="server">
										           <%# Eval( "DivisionName" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:DropDownList ID="ddlEditDivision" runat="Server" DataTextField="Name" DataValueField="DivisionID" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:DropDownList ID="ddlNewDivision" runat="Server" DataTextField="Name" DataValueField="DivisionID" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Company" ItemStyle-Width="200px">
										    <ItemTemplate>
											    <asp:Label ID="lblName" runat="server">
												    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
												    CausesValidation="false"><span><%# Eval("CompanyName")%></span></asp:LinkButton>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditName" runat="server" Columns="20" MaxLength="50" Text='<%# Eval("CompanyName") %>' />
										        <asp:RequiredFieldValidator ID="rfvEditName" runat="server" ControlToValidate="txtEditName"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewName" runat="server" Columns="20" MaxLength="50" />
											    <asp:RequiredFieldValidator ID="rfvNewName" runat="server" ControlToValidate="txtNewName"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="DB Name" ItemStyle-Width="200px">
										    <ItemTemplate>
											    <asp:Label ID="lblDBName" runat="server">
												    <%# Eval("DBName")%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditDBName" runat="server" Columns="20" MaxLength="50" Text='<%# Eval("DBName") %>' />
										        <asp:RequiredFieldValidator ID="rfvEditDBName" runat="server" ControlToValidate="txtEditDBName"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewDBName" runat="server" Columns="20" MaxLength="50" />
											    <asp:RequiredFieldValidator ID="rfvNewDBName" runat="server" ControlToValidate="txtNewDBName"
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
