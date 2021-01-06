<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="BankInfo.aspx.cs" Inherits="GMSWeb.Sales.Sales.SalesBreakdown" Title="Sales -Sales Breakdown Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<a name="TemplateInfo"></a>
<h1>Administration &gt; Package Costing</h1>
        <p>Recommend unit price for each product based on total selling price.</p>
        <asp:ScriptManager ID="sriptmgr" runat="server">
        <Services>
            <asp:ServiceReference Path="AutoCompletionProduct.asmx" />
        </Services>
        </asp:ScriptManager>

        <table style="margin-left: 8px; margin-right: 8px">
            <tr>
			    <td>
			        <td>
				        
					    <asp:label id="lblSearchSummary" Visible="false" Runat="server"></asp:label><br/>
				    
				    <br />
				    
						    <ContentTemplate>
							    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
								    DataKeyField="ProdCode" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
								    OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" 
								    GridLines="none" OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
								    CellPadding="5" CellSpacing="5" CssClass="tTable tBorder" AllowPaging="true" PageSize="50" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
								    <Columns>
									    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
										    <ItemTemplate>
											    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
											    .</ItemTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Product Code & Desctiptions" HeaderStyle-Wrap="false" ItemStyle-Width="500px">
										    <ItemTemplate>
										        <asp:Label ID="lblProdCode" runat="server">
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true"
												    CausesValidation="false"><span><%# Eval( "ProdCode" )%></span></asp:LinkButton>
										           <input type="hidden" id="hidProdCode" runat="server" value='<%# Eval("ProdCode")%>' />
											    </asp:Label>
										    </ItemTemplate>
										    <FooterTemplate>
										        <asp:TextBox ID="txtNewProdCode" runat="server" Columns="60" MaxLength="60" CssClass="textbox"
                                                    onfocus="select();" onchange="this.value = this.value.toUpperCase()" AutoPostBack="false"
                                                     />
                                                <asp:RequiredFieldValidator ID="rfvNewProdCode" runat="server"
                                                        ControlToValidate="txtNewProdCode" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                                        
										        <ajaxToolkit:AutoCompleteExtender
                                                            runat="server" 
                                                            BehaviorID="AutoCompleteEx"
                                                            ID="autoComplete1" 
                                                            TargetControlID="txtNewProdCode"
                                                            ServicePath="AutoCompletionProduct.asmx" 
                                                            ServiceMethod="GetCompletionList"
                                                            MinimumPrefixLength="1" 
                                                            CompletionInterval="100"
                                                            EnableCaching="false"
                                                            CompletionSetCount="10"
                                                            DelimiterCharacters=",">
                                                        </ajaxToolkit:AutoCompleteExtender>
										        
                                             </FooterTemplate>
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Quantity" HeaderStyle-Wrap="false" ItemStyle-Width="70px">
										    <ItemTemplate>
										        <asp:Label ID="lblQuantity" runat="server">
										           <%# Eval( "Quantity" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox CssClass="textbox" ID="txtEditQuantity" runat="server" Columns="10" MaxLength="15" Text='<%# Eval("Quantity") %>'/><asp:CompareValidator ID="cvEditQuantity" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                        ControlToValidate="txtEditQuantity" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox CssClass="textbox" ID="txtNewQuantity" runat="server" Columns="10" MaxLength="15" /><asp:CompareValidator ID="cvNewQuantity" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                        ControlToValidate="txtNewQuantity" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
									    </asp:TemplateColumn>
									     <asp:TemplateColumn HeaderText="Unit Price" HeaderStyle-Wrap="false" ItemStyle-Width="70px">
										    <ItemTemplate>
										        <asp:Label ID="lblSellingPrice" runat="server">
										           <%# Eval( "SellingPrice" )%>
											    </asp:Label>
										    </ItemTemplate>
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
								    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
							    </asp:DataGrid>
						    </ContentTemplate>
					  
			    </td>
			 </tr>
		</table>
	    <br />  
	    <table>
	    <tr>
	    <td class="tbLabel">Total Price:&nbsp;&nbsp;</td>
	    <td><asp:TextBox runat="server" ID="txtTotalPrice" MaxLength="20" Columns="20" onfocus="select();"
                    CssClass="textbox"></asp:TextBox></td>
        <td> <asp:Button ID="btnGenerate" Text="Generate" EnableViewState="False" runat="server" CssClass="button"
                    OnClick="btnGenerate_Click" ></asp:Button></td>
	    </tr>
	    </table>
	    
	    </table> 
	    <div class="TABCOMMAND">
	    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <ul>
                    <li></li>
                </ul>
                <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>   
</asp:Content>
