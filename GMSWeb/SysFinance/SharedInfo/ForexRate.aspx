<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="ForexRate.aspx.cs" Inherits="GMSWeb.SysFinance.SharedInfo.ForexRate1" Title="Finance - Foreign Exchange Rate Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<a name="TemplateInfo"></a>
<h1>Shared Info &gt; Foreign Exchange Rate</h1>
<p>Monthly foreign exchange rates for Buy, Sell and Month-end.</p>
<asp:ScriptManager ID="sriptmgr1" runat="server" EnablePartialRendering="false"/>
<table style="margin-left: 8px">
    <tr>
	    <td>
		    <asp:Label ID="lblHomeCurrency" runat="server" Text="Home Currency"  />:
		    <asp:DropDownList CssClass="dropdownlist" ID="ddlHomeCurrency" runat="server" DataTextField="CurrencyName" DataValueField="CurrencyCode" AutoPostBack="true" 
		            OnSelectedIndexChanged="ddlHomeCurrency_SelectedIndexChanged" />
		    <br /><br />
		    <div class="tTable1">
					    <asp:DataGrid ID="dgForex" runat="server" AutoGenerateColumns="false" ShowFooter="false"
						    OnCancelCommand="dgForex_CancelCommand" OnEditCommand="dgForex_EditCommand" OnUpdateCommand="dgForex_UpdateCommand" 
						    GridLines="none" OnItemDataBound="dgForex_ItemDataBound" OnItemCommand="dgForex_CreateCommand"
						    CellPadding="5" CellSpacing="5" CssClass="tTable tBorder">
						    <Columns>
							     <asp:TemplateColumn HeaderText="Currency" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="center"
							            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center">
								    <ItemTemplate>
									    <asp:Label ID="lblCurrency" runat="server">
										    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
										    CausesValidation="false"><span><%# Eval("ForeignCurrencyCode")%></span></asp:LinkButton>
									    </asp:Label>
								    </ItemTemplate>
								    <FooterTemplate>
									    <asp:DropDownList ID="ddlNewCurrency" runat="server" DataValueField="CurrencyCode" DataTextField="CurrencyCode" />
								    </FooterTemplate>
							    </asp:TemplateColumn>
							    
							    <asp:TemplateColumn HeaderText="Buy" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="center" 
							            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center">
								    <ItemTemplate>
									    <asp:Label ID="lblBuy" runat="server">
										    <%# (Eval("ForeignCurrencyCode").ToString().Equals("IDR")) ? Eval("BuyRate", "{0:f7}") : Eval("BuyRate", "{0:f4}")%>
									    </asp:Label>
								    </ItemTemplate>
								    <EditItemTemplate>
									    <asp:TextBox ID="txtEditBuyRate" CssClass="textbox" runat="server" Columns="6" MaxLength="10" Text='<%# Eval("BuyRate") %>' />
								        <asp:RequiredFieldValidator ID="rfvEditBuyRate" runat="server" ControlToValidate="txtEditBuyRate"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEditBuyRate" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtEditBuyRate" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox ID="txtNewBuyRate" CssClass="textbox" runat="server" Columns="6" MaxLength="10" />
									    <asp:RequiredFieldValidator ID="rfvNewBuyRate" runat="server" ControlToValidate="txtNewBuyRate"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewBuyRate" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewBuyRate" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
								    </FooterTemplate>
							    </asp:TemplateColumn>
							    
							    <asp:TemplateColumn HeaderText="Sell" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="center" 
							            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center">
								    <ItemTemplate>
									    <asp:Label ID="lblSell" runat="server">
										    <%# (Eval("ForeignCurrencyCode").ToString().Equals("IDR")) ? Eval("SellRate", "{0:f7}") : Eval("SellRate", "{0:f4}")%>
									    </asp:Label>
								    </ItemTemplate>
								    <EditItemTemplate>
									    <asp:TextBox ID="txtEditSellRate" CssClass="textbox" runat="server" Columns="6" MaxLength="10" Text='<%# Eval("SellRate") %>' />
								        <asp:RequiredFieldValidator ID="rfvEditSellRate" runat="server" ControlToValidate="txtEditSellRate"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEditSellRate" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtEditSellRate" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox ID="txtNewSellRate" CssClass="textbox" runat="server" Columns="6" MaxLength="10" />
									    <asp:RequiredFieldValidator ID="rfvNewSellRate" runat="server" ControlToValidate="txtNewSellRate"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewSellRate" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewSellRate" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
								    </FooterTemplate>
							    </asp:TemplateColumn>
							    
							    <asp:TemplateColumn HeaderText="Month-End" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="center" 
							            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center">
								    <ItemTemplate>
									    <asp:Label ID="lblMonthEnd" runat="server">
										    <%# (Eval("ForeignCurrencyCode").ToString().Equals("IDR")) ? Eval("MonthEndRate", "{0:f7}") : Eval("MonthEndRate", "{0:f4}")%>
									    </asp:Label>
								    </ItemTemplate>
								    <EditItemTemplate>
									    <asp:TextBox ID="txtEditMonthEnd" CssClass="textbox" runat="server" Columns="6" MaxLength="10" Text='<%# Eval("MonthEndRate") %>' />
								        <asp:RequiredFieldValidator ID="rfvEditMonthEnd" runat="server" ControlToValidate="txtEditMonthEnd"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEditMonthEnd" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtEditMonthEnd" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox ID="txtNewMonthEnd" CssClass="textbox" runat="server" Columns="6" MaxLength="10" />
									    <asp:RequiredFieldValidator ID="rfvNewMonthEnd" runat="server" ControlToValidate="txtNewMonthEnd"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewMonthEnd" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewMonthEnd" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
								    </FooterTemplate>
							    </asp:TemplateColumn>
							    
							    <asp:TemplateColumn HeaderText="Date Created" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="center" 
							            ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center">
								    <ItemTemplate>
									    <asp:Label ID="lblCreatedDate" runat="server">
										    <%# Eval("CreatedDate", "{0: dd-MMM-yyyy}")%>
									    </asp:Label>
								    </ItemTemplate>
							    </asp:TemplateColumn>
							    
							    <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" 
							        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="View History">
							        <ItemTemplate>
							            <input type="hidden" id="hidForeignCurrencyCode" runat="server" value='<%# Eval("ForeignCurrencyCode")%>' />
							            <input type="hidden" id="hidCreatedDate" runat="server" value='<%# Eval("CreatedDate")%>' />
							            <asp:LinkButton ID="lnkViewHistory" OnClick="lnkViewHistory_Click" runat="server" EnableViewState="false">
							            <span>&nbsp;&nbsp;ViewHistory</span></asp:LinkButton>
							        </ItemTemplate>
							    </asp:TemplateColumn>
							    
							    <asp:TemplateColumn ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" 
							        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
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
							<PagerStyle Visible="false" />
						    <HeaderStyle CssClass="tHeader" />
						    <AlternatingItemStyle CssClass="tAltRow" />
						    <FooterStyle CssClass="tFooter" />
					    </asp:DataGrid>
		    </div>
	    </td>
    </tr>
</table>
<p>
<asp:LinkButton ID="lnkAddNewRate" runat="server"  CommandName="AddNewRateCommand" CssClass="NewButt" BackColor="#EDF3FF" Visible="false">Add New Rate</asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lnkAddNewRate" PopupControlID="PNL" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground"/>
                                                    <asp:Panel ID="PNL" runat="server" style="display:none; width:200px; background-color:White; border-width:2px; border-color:Black; border-style:solid; padding:20px;">
                                                    Home Currency : Singapore Dollar <br />
                                                    <table>
                                                    <tr><td>FOR :</td>
                                                    <td align="center"><asp:DropDownList ID="ddlNewYear" CssClass="dropdownlist" runat="server" DataTextField="Year" DataValueField="Year" /></td><td align="center"><asp:DropDownList CssClass="dropdownlist" ID="ddlNewMonth" runat="server">
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
                                            </asp:DropDownList></td>
                                                    </tr>
                                                    <tr><td></td>
                                                    <td align="center">Buy</td><td align="center">Sell</td>
                                                    </tr>
                                                    <tr><td>
                                                    AUD :
                                                    </td><td>
                                                    <asp:TextBox CssClass="textbox" runat="server" ID="txtNewAUD1" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewAUD1" runat="server" ControlToValidate="txtNewAUD1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewAUD1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewAUD1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                    <asp:TextBox CssClass="textbox" runat="server" ID="txtNewAUD2" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewAUD2" runat="server" ControlToValidate="txtNewAUD2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewAUD2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewAUD2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr><tr>
                                                    <td>
                                                    CAD :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewCAD1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewCAD1" runat="server" ControlToValidate="txtNewCAD1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewCAD1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewCAD1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewCAD2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewCAD2" runat="server" ControlToValidate="txtNewCAD2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewCAD2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewCAD2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr>
                                                    <tr><td>
                                                    CNY :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewCNY1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewCNY1" runat="server" ControlToValidate="txtNewCNY1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewCNY1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewCNY1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewCNY2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewCNY2" runat="server" ControlToValidate="txtNewCNY2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewCNY2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewCNY2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr><tr>
                                                    <td>
                                                    EUR :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewEUR1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewEUR1" runat="server" ControlToValidate="txtNewEUR1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewEUR1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewEUR1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewEUR2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewEUR2" runat="server" ControlToValidate="txtNewEUR2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewEUR2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewEUR2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr>
                                                    <tr><td>
                                                    GBP :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewGBP1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewGBP1" runat="server" ControlToValidate="txtNewGBP1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewGBP" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewGBP1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewGBP2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewGBP2" runat="server" ControlToValidate="txtNewGBP2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewGBP2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewGBP2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr><tr>
                                                    <td>
                                                    HKD :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewHKD1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewHKD1" runat="server" ControlToValidate="txtNewHKD1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewHKD1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewHKD1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewHKD2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewHKD2" runat="server" ControlToValidate="txtNewHKD2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewHKD2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewHKD2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr>
                                                    <tr><td>
                                                    IDR :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewIDR1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewIDR1" runat="server" ControlToValidate="txtNewIDR1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewIDR1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewIDR1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewIDR2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewIDR2" runat="server" ControlToValidate="txtNewIDR2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewIDR2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewIDR2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr><tr>
                                                    <td>
                                                    JPY :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewJPY1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewJPY1" runat="server" ControlToValidate="txtNewJPY1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewJPY1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewJPY1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewJPY2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewJPY2" runat="server" ControlToValidate="txtNewJPY2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewJPY2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewJPY2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr>
                                                    <tr><td>
                                                    MYR :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewMYR1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewMYR1" runat="server" ControlToValidate="txtNewMYR1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewMYR1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewMYR1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewMYR2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewMYR2" runat="server" ControlToValidate="txtNewMYR2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewMYR2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewMYR2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr><tr>
                                                    <td>
                                                    NOK :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewNOK1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewNOK1" runat="server" ControlToValidate="txtNewNOK1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewNOK1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewNOK1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewNOK2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewNOK2" runat="server" ControlToValidate="txtNewNOK2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewNOK2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewNOK2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr>
                                                    <tr><td>
                                                    NTD :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewNTD1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewNTD1" runat="server" ControlToValidate="txtNewNTD1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewNTD1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewNTD1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewNTD2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewNTD2" runat="server" ControlToValidate="txtNewNTD2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewNTD2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewNTD2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr><tr>
                                                    <td>
                                                    SEK :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewSEK1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewSEK1" runat="server" ControlToValidate="txtNewSEK1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewSEK1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewSEK1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewSEK2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewSEK2" runat="server" ControlToValidate="txtNewSEK2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewSEK2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewSEK2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr>
                                                    <tr><td>
                                                    SFR :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewSFR1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewSFR1" runat="server" ControlToValidate="txtNewSFR1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewSFR1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewSFR1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewSFR2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewSFR2" runat="server" ControlToValidate="txtNewSFR2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewSFR2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewSFR2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr><tr>
                                                    <td>
                                                    THB :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewTHB1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewTHB1" runat="server" ControlToValidate="txtNewTHB1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewTHB1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewTHB1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewTHB2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewTHB2" runat="server" ControlToValidate="txtNewTHB2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewTHB2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewTHB2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr>
                                                    <tr><td>
                                                    USD :
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewUSD1" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewUSD1" runat="server" ControlToValidate="txtNewUSD1"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewUSD1" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewUSD1" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td><td>
                                                        <asp:TextBox CssClass="textbox" ID="txtNewUSD2" runat="server" Columns="10"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNewUSD2" runat="server" ControlToValidate="txtNewUSD2"
										    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewUSD2" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewUSD2" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                                    </td>
                                                    </tr>
                                                    </table>
                                                       <br />
                                                        <div style="text-align:center;">
                                                            <asp:Button CssClass="button" ID="ButtonOk" runat="server" Text="Save" OnCommand="AddNewRateCommand" ValidationGroup="valGrpNewRow"/>
                                                            <asp:Button CssClass="button" ID="ButtonCancel" runat="server" Text="Cancel" />
                                                        </div>
                                                    </asp:Panel> 
                                                    </p>
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
