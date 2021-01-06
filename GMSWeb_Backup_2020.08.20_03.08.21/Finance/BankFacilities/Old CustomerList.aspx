<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Old CustomerList.aspx.cs" Inherits="GMSWeb.Finance.BankFacilities.CustomerList" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Bank Facilities - Receiver Payer List</title>
    <script language="javascript" type="text/javascript" src="/GMS/scripts/popcalendar.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="GroupContentBar">
        <h3>Bank Facilities &gt; Receiver Payer List</h3>
        List of historical bank transaction receiver or payer
        <br /><br />
        <asp:ScriptManager ID="scriptMgr" runat="server" />
        <asp:UpdatePanel ID="udpBankUpdater" runat="server" UpdateMode="conditional" >
						    <ContentTemplate>
						    <table class="tTable"style="BORDER-COLLAPSE: collapse" cellspacing="0" cellpadding="1" 
				border="1" width="500px">
        <tr>
        <td class="tbLabel">Name</td><td>:</td>
        <td colspan="2">
        <asp:TextBox runat="server" ID="searchName" MaxLength="50" Columns="50" onfocus="select();" CssClass="textbox"></asp:TextBox></td>
		<td style="width:10%"><asp:Button id="btnSearch" Text="Search" EnableViewState="False" Runat="server" CssClass="button" OnClick="btnSearch_Click"></asp:Button></td>
        </tr>
        </table>
        <table class="tGroupTable" style="width: 100%" >
            <tr>
			    <td>
				    <br />
				    <div id="Div1" style="text-align:left;width:1500px" runat="server">
					<asp:label id="lblSearchSummary" Visible="false" Runat="server"></asp:label>
				</div>
				<br />
				    <div class="tGroupTable">
							    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="Name"  OnItemCommand="dgData_CreateCommand" 
								    GridLines="none" OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
								    CellPadding="5" CellSpacing="0" CssClass="tTable tBorder" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
											    .</ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Name" HeaderStyle-Wrap="false" ItemStyle-Width="250px">
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
								    <HeaderStyle CssClass="tGroupHeader" />
								    <AlternatingItemStyle CssClass="tGroupAltRow" />
								    <FooterStyle CssClass="tGroupFooter" />
								    <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
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
    </div>
    </form>
</body>
</html>
