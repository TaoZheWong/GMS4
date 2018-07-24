<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadSpecial.aspx.cs" Inherits="GMSWeb.Organization.Upload.UploadSpecial" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Data - Special</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="GroupContentBar">
        <h3>Data &gt; Special</h3>
        
        Key in special data for the specified data.
        <br />
        <atlas:ScriptManager ID="scriptMgr" runat="server" />
        <table class="tGroupTable" style="width: 100%">
            <tr>
			    <td>
				    <br />
				    <asp:Label ID="lblParameter" runat="server" Text="Company Parameter"  />:
				    <asp:DropDownList ID="ddlCompanyParameter" runat="server" DataTextField="ParameterName" DataValueField="ParameterID" AutoPostBack="true" 
				            OnSelectedIndexChanged="ddlCompanyParameter_SelectedIndexChanged" />
				    <br /><br />
				    <div id="Resultslbl" style="text-align:right;width: 520px" visible="false" runat="server">
						        Results:
								<asp:Label ID="lblTotalRecordsFound" runat="server" Font-Bold="true" />
								</div>
				    <div class="tGroupTable">
				    <atlas:UpdatePanel ID="udpSpecialDataUpdater" runat="server" Mode="conditional">
						    <ContentTemplate>
							    <asp:DataGrid ID="dgSpecialData" runat="server" AutoGenerateColumns="False" ShowFooter="True"   OnItemDataBound="dgSpecialData_ItemDataBound"
								    OnCancelCommand="dgSpecialData_CancelCommand" OnEditCommand="dgSpecialData_EditCommand" OnUpdateCommand="dgSpecialData_UpdateCommand" 
								    GridLines="None"  OnItemCommand="dgSpecialData_CreateCommand"  OnDeleteCommand="dgSpecialData_DeleteCommand"
								    CellPadding="5" CssClass="tTable tBorder" AllowCustomPaging="True" AllowPaging="True" pagesize="10">
								    <Columns>
									     <asp:TemplateColumn HeaderText="CSDDate">
										    <ItemTemplate>
											    <asp:Label ID="lblValue" runat="server">
												    <%# Eval("CSDDate", "{0: dd-MMM-yyyy}")%>
											    </asp:Label>
											    <input type="hidden" id="hidCoyID" runat="server" value='<%# Eval("CoyID")%>' />
									            <input type="hidden" id="hidParameterID" runat="server" value='<%# Eval("ParameterID")%>' />
									            <input type="hidden" id="hidCSDDate" runat="server" value='<%# Eval("CSDDate", "{0: dd-MMM-yyyy}")%>' />
										    </ItemTemplate>
										    <FooterTemplate>
											    <uctrl:Calendar ID="calCSDDate" runat="server" InvalidDateMessage="Invalid Date"
									    IsValueRequired="false" />
										    </FooterTemplate>
                                             <ItemStyle HorizontalAlign="Center" Width="100px" />
                                             <FooterStyle HorizontalAlign="Center" />
                                             <HeaderStyle HorizontalAlign="Center" />
									    </asp:TemplateColumn>
									    <asp:TemplateColumn HeaderText="Value">
										    <ItemTemplate>
											    <asp:Label ID="lblParameter" runat="server">
												    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
												    CausesValidation="false"><span><%# Eval("ParameterValue")%></span></asp:LinkButton>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:TextBox ID="txtEditValue" runat="server" Columns="50" MaxLength="200" Text='<%# Eval("ParameterValue") %>' />
										        <asp:RequiredFieldValidator ID="rfvEditValue" runat="server" ControlToValidate="txtEditValue"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
										    <FooterTemplate>
											    <asp:TextBox ID="txtNewValue" runat="server" Columns="50" MaxLength="200" />
											    <asp:RequiredFieldValidator ID="rfvNewValue" runat="server" ControlToValidate="txtNewValue"
												    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
										    </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="260px" />
                                            <FooterStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
									    </asp:TemplateColumn>
									    
									    <asp:TemplateColumn HeaderText="Function">
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
                                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                                            <FooterStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
									    </asp:TemplateColumn>
								    </Columns>
								    <HeaderStyle CssClass="tGroupHeader" />
								    <AlternatingItemStyle CssClass="tGroupAltRow" />
								    <FooterStyle CssClass="tGroupFooter" />
                                    <PagerStyle Visible="False" />
							    </asp:DataGrid>
							     </ContentTemplate>
					    </atlas:UpdatePanel>
				    </div>
				    <div id="Pageslbl" style="text-align:center;width: 520px" visible="false" runat="server">
							        <asp:LinkButton ID="lnkFirst" runat="server" CommandName="First" OnCommand="SearchResults_PageNavigate">First</asp:LinkButton>
								    <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Previous" OnCommand="SearchResults_PageNavigate">Previous</asp:LinkButton>
								    <asp:Label ID="lblPage" runat="server" Font-Bold="true" />
								    <asp:LinkButton ID="lnkNext" runat="server" CommandName="Next" OnCommand="SearchResults_PageNavigate">Next</asp:LinkButton>
								    <asp:LinkButton ID="lnkLast" runat="server" CommandName="Last" OnCommand="SearchResults_PageNavigate">Last</asp:LinkButton>
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
