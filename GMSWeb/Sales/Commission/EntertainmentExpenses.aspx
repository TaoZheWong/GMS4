<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EntertainmentExpenses.aspx.cs" Inherits="GMSWeb.Sales.Commission.EntertainmentExpenses" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Commission - Entertainment Expenses</title>
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <script language="javascript" type="text/javascript" src="/GMS/scripts/popcalendar.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    
    <asp:ScriptManager ID="sriptmgr1" runat="server">
         </asp:ScriptManager>               
        <div id="GroupContentBar">
            <h3>
                Commission &gt; Entertainment Expenses</h3>
            List of monthly entertainment expenses by salesman.
            <br />
            <br />
            <table class="tGroupTable" style="width: 100%" >
            <tr>
			    <td>
				    <br />
				    <div id="Div1" style="text-align:left;width:1150px" runat="server">
					<asp:label id="lblSearchSummary" Visible="false" Runat="server"></asp:label>
				</div>
				<br />
				    <div class="tGroupTable">
							    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="SalesPersonMasterID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
								    OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" 
								    GridLines="none" OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
								    CellPadding="5" CellSpacing="0" CssClass="tTable tBorder" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
											    .</ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Salesman Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="140px" >
										    <ItemTemplate>
										        <asp:Label ID="lblSalesPersonMasterName" runat="server">
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
												    CausesValidation="false"><span><%# Eval("SalesPersonMasterObject.SalesPersonMasterName")%></span></asp:LinkButton>
										           <input type="hidden" id="hidSalesPersonMasterID" runat="server" value='<%# Eval("SalesPersonMasterID")%>' />
										           <input type="hidden" id="hidYear" runat="server" value='<%# Eval("tbYear")%>' />
										           <input type="hidden" id="hidMonth" runat="server" value='<%# Eval("tbMonth")%>' />
											    </asp:Label>
										    </ItemTemplate>
										    <FooterTemplate>
											    <asp:DropDownList ID="ddlNewSalesPersonMasterName" runat="Server" DataTextField="SalesPersonMasterName" DataValueField="SalesPersonMasterID" OnChange="document.getElementById(ddlNewGPQ).value = this.value;if (document.getElementById(ddlNewGPQ).selectedIndex >= 0) document.getElementById(lblGPQ2).innerText = document.getElementById(ddlNewGPQ).options[document.getElementById(ddlNewGPQ).selectedIndex].text; else document.getElementById(lblGPQ2).innerText = 'Nill'; document.getElementById(ddlNewCommRate).value = this.value;if (document.getElementById(ddlNewCommRate).selectedIndex >= 0) document.getElementById(lblCommRate2).innerText = document.getElementById(ddlNewCommRate).options[document.getElementById(ddlNewCommRate).selectedIndex].text; else document.getElementById(lblCommRate2).innerText = 'Nill';" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Year" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
										    <ItemTemplate>
										        <asp:Label ID="lblYear" runat="server">
										           <%# Eval( "tbYear" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <FooterTemplate>
											    <asp:DropDownList ID="ddlNewYear" runat="server" DataTextField="Year" DataValueField="Year" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Month" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
										    <ItemTemplate>
										        <asp:Label ID="lblMonth" runat="server">
										           <%# Eval( "tbMonth" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <FooterTemplate>
											    <asp:DropDownList ID="ddlNewMonth" runat="server">
                                                        <asp:ListItem>1</asp:ListItem>
                                                        <asp:ListItem>2</asp:ListItem>
                                                        <asp:ListItem>3</asp:ListItem>
                                                        <asp:ListItem>4</asp:ListItem>
                                                        <asp:ListItem>5</asp:ListItem>
                                                        <asp:ListItem>6</asp:ListItem>
                                                        <asp:ListItem>7</asp:ListItem>
                                                        <asp:ListItem>8</asp:ListItem>
                                                        <asp:ListItem>9</asp:ListItem>
                                                        <asp:ListItem>10</asp:ListItem>
                                                        <asp:ListItem>11</asp:ListItem>
                                                        <asp:ListItem>12</asp:ListItem>
                                                    </asp:DropDownList>
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Comm Rate" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
										    <ItemTemplate>
										        <asp:Label ID="lblCommRate" runat="server">
										           <%# Eval( "CommissionRate" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <FooterTemplate>
										    <asp:Label ID="lblCommRate2" runat="server">
											    </asp:Label>
											    <div style="display:none">
											    <asp:DropDownList ID="ddlNewCommRate" runat="Server" DataTextField="CommissionRate" DataValueField="SalesPersonMasterID"/></div>
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="GPQ" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
										    <ItemTemplate>
										        <asp:Label ID="lblGPQ" runat="server">
										           <%# Eval( "GPQ" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <FooterTemplate>
										    <asp:Label ID="lblGPQ2" runat="server">
											    </asp:Label>
											    <div style="display:none">
											    <asp:DropDownList ID="ddlNewGPQ" runat="Server" DataTextField="GPQ" DataValueField="SalesPersonMasterID"/></div>
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="90 Code" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Width="70px">
										    <ItemTemplate>
										        <asp:Label ID="lbl90Code" runat="server">
										           <%# Eval( "CostOf90Code" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEdit90Code" runat="server" Columns="10" MaxLength="10" Text='<%# Eval("CostOf90Code") %>'/><asp:RequiredFieldValidator ID="rfvEdit90Code" runat="server" ControlToValidate="txtEdit90Code"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEdit90Code" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                        ControlToValidate="txtEdit90Code" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNew90Code" runat="server" Columns="10" MaxLength="10" /><asp:RequiredFieldValidator ID="rfvNew90Code" runat="server" ControlToValidate="txtNew90Code"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNew90Code" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                        ControlToValidate="txtNew90Code" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="C.O.G" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Width="70px">
										    <ItemTemplate>
										        <asp:Label ID="lblCOG" runat="server">
										           <%# Eval( "CostOfCOG" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditCOG" runat="server" Columns="10" MaxLength="10" Text='<%# Eval("CostOfCOG") %>'/><asp:RequiredFieldValidator ID="rfvEditCOG" runat="server" ControlToValidate="txtEditCOG"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEditCOG" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                        ControlToValidate="txtEditCOG" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewCOG" runat="server" Columns="10" MaxLength="10" /><asp:RequiredFieldValidator ID="rfvNewCOG" runat="server" ControlToValidate="txtNewCOG"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewCOG" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                        ControlToValidate="txtNewCOG" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Entertainment Exp" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Width="70px">
										    <ItemTemplate>
										        <asp:Label ID="lblEntertainmentExpense" runat="server">
										           <%# Eval( "TotalEntertainmentExpenses" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditEntertainmentExpenses" runat="server" Columns="10" MaxLength="10" Text='<%# Eval("TotalEntertainmentExpenses") %>'/><asp:RequiredFieldValidator ID="rfvEditEntertainmentExpenses" runat="server" ControlToValidate="txtEditEntertainmentExpenses"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEditEntertainmentExpenses" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                        ControlToValidate="txtEditEntertainmentExpenses" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewEntertainmentExpenses" runat="server" Columns="10" MaxLength="10" /><asp:RequiredFieldValidator ID="rfvNewEntertainmentExpenses" runat="server" ControlToValidate="txtNewEntertainmentExpenses"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewEntertainmentExpenses" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                        ControlToValidate="txtNewEntertainmentExpenses" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
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
								    <HeaderStyle CssClass="tGroupHeader"  />
								    <AlternatingItemStyle CssClass="tGroupAltRow" />
								    <FooterStyle CssClass="tGroupFooter" />
								    <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
							    </asp:DataGrid>
				    </div>
				    <div style="text-align:left;width:88%">
				    <br />
                        <asp:DropDownList ID="ddlReport" runat="server" CssClass="dropdownlist">
                        <asp:ListItem Value="SalesCommissionsEntitlement">Commission Entitlement Report</asp:ListItem>
                        <asp:ListItem Value="SalesCommissions">Commission Payable Report</asp:ListItem>
                        <asp:ListItem Value="SalesCommissions_Forfeited">Commissions Forfeited Report</asp:ListItem>
                        <asp:ListItem Value="SalesSpecialCommissions">Special Continuous Commissions Report</asp:ListItem>
                        </asp:DropDownList>
													<ASP:LINKBUTTON id="LINKBUTTON1" onclick="GenerateReport" RUNAT="server" TEXT="Print"
														CSSCLASS="button" TOOLTIP="Please click to print report." CAUSESVALIDATION="False"><img id="img4" alt="" src="../../images/icons/printIcon.gif" align="top" border="0" /></ASP:LINKBUTTON>
														</div>
			    </td>
		    </tr>
	    </table>
	    <br />
            <div class="TABCOMMAND">
                <asp:UpdatePanel ID="udpMsgUpdater" runat="server"  UpdateMode="Always">
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
