<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Old BankInfo.aspx.cs" Inherits="GMSWeb.SysFinance.BankFacility.BankInfo" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Bank Facilities - Bank Info</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ContentBar">
        <h3>Bank Facilities &gt; Bank Info</h3>
        List of current bank information.
        <br />
        <atlas:ScriptManager ID="scriptMgr" runat="server" />
        <table class="tTable" style="width: 100%" >
            <tr>
			    <td>
				    <br />
				    <div id="Div1" style="text-align:left;width:1500px" runat="server">
					<asp:label id="lblSearchSummary" Visible="false" Runat="server"></asp:label>
				</div>
				<br />
				    <div class="tTable">
					    <atlas:UpdatePanel ID="udpBankUpdater" runat="server" Mode="conditional" >
						    <ContentTemplate>
							    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="BankID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
								    OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" 
								    GridLines="none" OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
								    CellPadding="5" CellSpacing="0" CssClass="tTable tBorder" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
											    .</ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Bank Code" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
										    <ItemTemplate>
										        <asp:Label ID="lblBankCode" runat="server">
										        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
												    CausesValidation="false"><span><%# Eval( "BankCode" )%></span></asp:LinkButton>
										           <input type="hidden" id="hidBankCode" runat="server" value='<%# Eval("BankCode")%>' />
											    </asp:Label>
										    </ItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewBankCode" runat="server" Columns="5" MaxLength="5" />
											    <asp:RequiredFieldValidator ID="rfvNewBankCode" runat="server" ControlToValidate="txtNewBankCode"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Bank Name" HeaderStyle-Wrap="false" ItemStyle-Width="250px">
										    <ItemTemplate>
										        <asp:Label ID="lblBankName" runat="server">
										        <%# Eval( "BankName" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditBankName" runat="server" Columns="50" MaxLength="50" Text='<%# Eval("BankName") %>' />
											    <asp:RequiredFieldValidator ID="rfvEditBankName" runat="server" ControlToValidate="txtEditBankName"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewBankName" runat="server" Columns="50" MaxLength="50" />
											    <asp:RequiredFieldValidator ID="rfvNewBankName" runat="server" ControlToValidate="txtNewBankName"
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
								    <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
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
