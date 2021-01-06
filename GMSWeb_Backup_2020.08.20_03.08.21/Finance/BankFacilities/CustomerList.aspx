<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="GMSWeb.Finance.BankFacilities.CustomerList" Title="Finance - Customer List Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<h1>Administration &gt; Receiver & Payer List</h1>
        <p>List of bank transaction's receiver or payer. </p>
        <asp:ScriptManager ID="scriptMgr" runat="server" />
        <asp:UpdatePanel ID="udpBankUpdater" runat="server" UpdateMode="conditional" >
						    <ContentTemplate>
						    <table class="tTable1" style="BORDER-COLLAPSE: collapse; margin-left: 8px" cellspacing="5" cellpadding="5" 
				border="0" width="500px">
        <tr>
        <td class="tbLabel">Name</td><td>:</td>
        <td colspan="2">
        <asp:TextBox runat="server" ID="searchName" MaxLength="50" Columns="50" onfocus="select();" CssClass="textbox"></asp:TextBox></td>
		<td style="width:10%"><asp:Button id="btnSearch" Text="Search" EnableViewState="False" Runat="server" CssClass="button" OnClick="btnSearch_Click"></asp:Button></td>
        </tr>
        </table>
        <table class="table1" style="margin-left: 8px">
            <tr>
			    <td>
				    <br />
				    <div id="Div1" style="text-align:left;" runat="server">
					<asp:label id="lblSearchSummary" Visible="false" Runat="server"></asp:label>
				</div>
				<br />
				    <div>
							    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="Name"  OnItemCommand="dgData_CreateCommand" 
								    GridLines="none" OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
								    CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
											    .</ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Name" HeaderStyle-Wrap="false" ItemStyle-Width="330px">
										    <ItemTemplate>
										        <asp:Label ID="lblName" runat="server">
                                                <span><%# Eval( "Name" )%></span>
										           <input type="hidden" id="hidName" runat="server" value='<%# Eval("Name")%>' />
											    </asp:Label>
										    </ItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewName" runat="server" Columns="50" MaxLength="50" />
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
										    <FooterTemplate>
											<asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
												ValidationGroup="valGrpNewRow" CssClass="NewButt"><span>Add</span></asp:LinkButton>
										</FooterTemplate>
									    </asp:TemplateColumn>
								    </Columns>
								    <HeaderStyle CssClass="tHeader" />
								    <AlternatingItemStyle CssClass="tAltRow" />
								    <FooterStyle CssClass="tFooter" />
								    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
							    </asp:DataGrid>
				    </div>
			    </td>
		    </tr>
	    </table>
	    </ContentTemplate>
					    </asp:UpdatePanel>
	    <br />   
	    <div class="TABCOMMAND">
		    <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
			    <ContentTemplate>
				    <ul>
					    <li>&nbsp;</li>
				    </ul>
				    <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
			    </ContentTemplate>
		    </asp:UpdatePanel>
        </div>   
</asp:Content>
