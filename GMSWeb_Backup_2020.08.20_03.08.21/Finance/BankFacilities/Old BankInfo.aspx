<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Old BankInfo.aspx.cs" Inherits="GMSWeb.Finance.BankFacility.BankInfo" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Bank Facilities - Bank Info</title>
    <script language="javascript" type="text/javascript" src="/GMS/scripts/popcalendar.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="GroupContentBar">
        <h3>Bank Facilities &gt; Bank Info</h3>
        List of current bank information.
        <br />
        <atlas:ScriptManager ID="scriptMgr" runat="server" />
        <table class="tGroupTable" style="width: 100%" >
            <tr>
			    <td>
				    <br />
				    <div id="Div1" style="text-align:left;width:1500px" runat="server">
					<asp:label id="lblSearchSummary" Visible="false" Runat="server"></asp:label>
				</div>
				<br />
				    <div class="tGroupTable">
					    <atlas:UpdatePanel ID="udpBankUpdater" runat="server" Mode="conditional" >
						    <ContentTemplate>
							    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="COAID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
								    OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" 
								    GridLines="none" OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
								    CellPadding="5" CellSpacing="0" CssClass="tTable tBorder" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
											    .</ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Bank COA" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
										    <ItemTemplate>
										        <asp:Label ID="lblBankCOA" runat="server">
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
												    CausesValidation="false"><span><%# Eval( "BankCOA" )%></span></asp:LinkButton>
										           <input type="hidden" id="hidCOAID" runat="server" value='<%# Eval("COAID")%>' />
											    </asp:Label>
										    </ItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewBankCOA" runat="server" Columns="8" MaxLength="10" />
											    <asp:RequiredFieldValidator ID="rfvNewBankCOA" runat="server" ControlToValidate="txtNewBankCOA"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Bank Code" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
										    <ItemTemplate>
										        <asp:Label ID="lblBankCode" runat="server">
										        <%# Eval( "BankObject.BankCode" )%>
										           <input type="hidden" id="hidBankCode" runat="server" value='<%# Eval("BankObject.BankCode")%>' />
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:DropDownList ID="ddlEditBankCode" runat="Server" DataTextField="BankCode" DataValueField="BankID" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:DropDownList ID="ddlNewBankCode" runat="Server" DataTextField="BankCode" DataValueField="BankID" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Currency" ItemStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrency" runat="server">
												   <%# Eval("Currency")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
											    <asp:DropDownList ID="ddlEditCurrency" runat="Server" DataTextField="CurrencyCode" DataValueField="CurrencyCode" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:DropDownList ID="ddlNewCurrency" runat="Server" DataTextField="CurrencyCode" DataValueField="CurrencyCode" />
										    </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Cheque Format" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
										    <ItemTemplate>
										        <asp:Label ID="lblChequeFormat" runat="server">
										        <%# Eval( "ChequeFormatCode" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:DropDownList ID="ddlEditChequeFormatCode" runat="server">
                                                        <asp:ListItem Value="N.A">N.A</asp:ListItem>
                                                        <asp:ListItem Value="2007SG">2007SG</asp:ListItem>
                                                        <asp:ListItem Value="2007ID">2007ID</asp:ListItem>
                                                        <asp:ListItem Value="2007TH">2007TH</asp:ListItem>
                                                        <asp:ListItem Value="2010ID">2010ID</asp:ListItem>
                                                    </asp:DropDownList>
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:DropDownList ID="ddlNewChequeFormatCode" runat="server">
                                                        <asp:ListItem Value="N.A">N.A</asp:ListItem>
                                                        <asp:ListItem Value="2007SG">2007SG</asp:ListItem>
                                                        <asp:ListItem Value="2007ID">2007ID</asp:ListItem>
                                                        <asp:ListItem Value="2007TH">2007TH</asp:ListItem>
                                                        <asp:ListItem Value="2010ID">2010ID</asp:ListItem>
                                                    </asp:DropDownList>
										    </FooterTemplate>
									    </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Facility Limit" HeaderStyle-Wrap="false" ItemStyle-Width="70px">
										    <ItemTemplate>
										        <asp:Label ID="lblLimit" runat="server">
										           <%# Eval( "Limit" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditLimit" runat="server" Columns="10" MaxLength="15" Text='<%# Eval("Limit") %>'/><asp:CompareValidator ID="cvEditLimit" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                        ControlToValidate="txtEditLimit" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewLimit" runat="server" Columns="10" MaxLength="15" /><asp:CompareValidator ID="cvNewLimit" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                        ControlToValidate="txtNewLimit" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Int. Rate" HeaderStyle-Wrap="false" ItemStyle-Width="70px">
										    <ItemTemplate>
										        <asp:Label ID="lblInterestRate" runat="server">
										        <%# Eval( "InterestRate" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditInterestRate" runat="server" Columns="10" MaxLength="20" Text='<%# Eval("InterestRate") %>' />
											    <asp:RequiredFieldValidator ID="rfvEditInterestRate" runat="server" ControlToValidate="txtEditInterestRate"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewInterestRate" runat="server" Columns="10" MaxLength="20" />
											    <asp:RequiredFieldValidator ID="rfvNewInterestRate" runat="server" ControlToValidate="txtNewInterestRate"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Balance" HeaderStyle-Wrap="false" ItemStyle-Width="70px">
										    <ItemTemplate>
										        <asp:Label ID="lblBalance" runat="server">
										           <%# Eval( "Balance" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditBalance" runat="server" Columns="10" MaxLength="15" Text='<%# Eval("Balance") %>'/><asp:CompareValidator ID="cvEditBalance" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                        ControlToValidate="txtEditBalance" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewBalance" runat="server" Columns="10" MaxLength="15" /><asp:CompareValidator ID="cvNewBalance" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                        ControlToValidate="txtNewBalance" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Maturity Date" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaturityDate" runat="server">
												    <%# Eval("MaturityDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("MaturityDate", "{0: dd-MMM-yyyy}")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" ID="editMaturityDate" MaxLength="10" Columns="10" Text='<%# Eval("MaturityDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "" : Eval("MaturityDate", "{0: dd/MM/yyyy}")%>'></asp:TextBox>
                                                    <img id="imgCalendarEditMaturityDate" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                        height="20" width="17" alt="" align="absMiddle" border="0"><asp:CompareValidator
                                                            ID="cvEditMaturityDate" runat="server" ErrorMessage="Invalid Date" ControlToValidate="editMaturityDate" Type="Date" Display="Dynamic" ValidationGroup="valGrpEditRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="newMaturityDate" MaxLength="10" Columns="10"></asp:TextBox>
                                                    <img id="imgCalendarNewMaturityDate" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                        height="20" width="17" alt="" align="absMiddle" border="0"><asp:CompareValidator
                                                            ID="cvNewMaturityDate" runat="server" ErrorMessage="Invalid Date" ControlToValidate="newMaturityDate" Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                <FooterStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
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
