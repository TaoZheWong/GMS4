<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Old BankUtilisation.aspx.cs"
    Inherits="GMSWeb.Finance.BankFacilities.BankUtilisation" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bank Facilities - Bank Utilisation</title>
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <script language="javascript" type="text/javascript" src="/GMS/scripts/popcalendar.js"></script>
    <script type="text/javascript">
    function checkAll(checkItem, checkVal)
			{
					var frm = document.form1;
					for(i = 1; i < frm.length; i++) 
					{
						var elm = frm.elements[i];
						if( elm.type == 'checkbox' && elm.name.indexOf(checkItem) != -1 && elm.disabled != true )
						{
							elm.checked = checkVal;
						}
					}
			} 
			function DeselectMainCheckbox(checkbox)
			{
					document.all('dgData').rows[0].cells[12].childNodes[0].checked = false;
					
			}
			
    function changeCurrency(ddl1)
    {
        var ddl2 = document.getElementById("ddlBankAccount");
        for( var i=0; i<ddl2.options.length; i++){
        ddl2.options[i].selected=(ddl2.options[i].text==ddl1.value);
        }
        document.getElementById(ddlNewCurrency).value = ddl2.value;
    }
    </script>
</head>
<body onload="javascript:document.getElementById('txtSearchName').focus();">
    <form id="form1" runat="server" defaultbutton="btnSearch">
    
    <asp:ScriptManager ID="sriptmgr1" runat="server">
                        <Services>
        <asp:ServiceReference Path="AutoComplete.asmx" />
    </Services>
         </asp:ScriptManager>               
        <div id="GroupContentBar">
            <h3>
                Bank Facilities &gt; Bank Utilisation</h3>
            List of records of bank transaction.
            <br />
            <br />
            <table class="tGroupTable"style="BORDER-COLLAPSE: collapse" cellspacing="0" cellpadding="1" 
				border="1" width="800px">
        <tr>
        <td class="tbLabel" style="width:20%">Received From/Pay To</td><td style="width:5%">:</td>
        <td style="width:20%">
        <asp:TextBox ID="txtSearchName" runat="server" Columns="20" MaxLength="50" onfocus="select();"  CssClass="textbox" />
        </td>
        <td class="tbLabel" style="width:20%">Mode</td><td style="width:5%">:</td>
        <td style="width:20%">
        <asp:TextBox ID="txtSearchMode" runat="server" Columns="10" MaxLength="10" onfocus="select();"  CssClass="textbox"/>
        </td>
        </tr>
        <tr>
        <td class="tbLabel">Transaction No</td><td>:</td>
        <td>
        <asp:TextBox ID="txtSearchTrnNo" runat="server" Columns="10" MaxLength="10" onfocus="select();" CssClass="textbox"/>
        </td>
        <td class="tbLabel">Cheuqe No</td><td>:</td>
        <td>
        <asp:TextBox ID="txtSearchCHequeNo" runat="server" Columns="10" MaxLength="10" onfocus="select();" CssClass="textbox"/>
        </td>
        </tr>
        <tr>
        <td class="tbLabel">Transaction Date From</td><td>:</td>
        <td>
        <asp:TextBox runat="server" ID="trnDateFrom" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox"></asp:TextBox>
                                                    <img id="imgCalendarEditFrom" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                        height="20" width="17" alt="" align="absMiddle"  border="0"></td>
        <td class="tbLabel">Transaction Date To</td><td>:</td>
        <td>
        <asp:TextBox runat="server" ID="trnDateTo" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox"></asp:TextBox>
                                                    <img id="img1" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                        height="20" width="17" alt="" align="absMiddle"  border="0"></td>
        </tr>
        <tr>
        <td class="tbLabel">Cheque Date From</td><td>:</td>
        <td>
        <asp:TextBox runat="server" ID="chequeDateFrom" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox"></asp:TextBox>
                                                    <img id="img2" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                        height="20" width="17" alt="" align="absMiddle"  border="0"></td>
        <td class="tbLabel">Cheque Date To</td><td>:</td>
        <td>
        <asp:TextBox runat="server" ID="chequeDateTo" MaxLength="10" Columns="10" onfocus="select();" CssClass="textbox"></asp:TextBox>
                                                    <img id="img3" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                        height="20" width="17" alt="" align="absMiddle"  border="0"></td>
        </tr>
        <tr>
        <td class="tbLabel">Bank Account</td><td>:</td>
        <td>
        <asp:DropDownList ID="ddlBankCOA" runat="Server" DataTextField="BankCodeCOA" DataValueField="COAID" CssClass="dropdownlist"/></td>
        <td class="tbLabel">Type</td><td>:</td>
        <td>
        <asp:DropDownList ID="ddlType" runat="server" CssClass="dropdownlist">
        <asp:ListItem Value="%">-All-</asp:ListItem>
                                                        <asp:ListItem Value="R">R</asp:ListItem>
                                                        <asp:ListItem Value="P">P</asp:ListItem>
                                                    </asp:DropDownList>
        </td>
        <td style="width:10%"><asp:Button id="btnSearch" Text="Search" EnableViewState="False" Runat="server" CssClass="button"  OnClick="btnSearch_Click"></asp:Button></td>
        </tr>
        </table>
        <span style="visibility:hidden">
        <asp:DropDownList ID="ddlBankAccount" runat="server" DataTextField="COAID" DataValueField="Currency"/> 
            </span>
            <br />
            <table class="tGroupTable" >
                <tr>
                    <td>
                        <div class="tGroupTable">
                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="udpBankUtilUpdater">
                                <ContentTemplate>
                                <div id="Div1" style="text-align: left; width: 1500px" runat="server">
                            <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                        </div>
                            <br />
                                    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                        DataKeyField="TransactionNo" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
                                        OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" GridLines="none"
                                        OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
                                        CellPadding="5" CellSpacing="0" CssClass="tTable tBorder" AllowPaging="true"
                                        PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                                        <Columns>
                                        <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                                                <ItemTemplate>
                                                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                                    .</ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Trans No" HeaderStyle-Wrap="false" ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTrnNo" runat="server">
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                                            CausesValidation="false"><span><%# Eval( "TransactionNo" )%></span></asp:LinkButton>
                                                        <input type="hidden" id="hidTransactionNo" runat="server" value='<%# Eval("TransactionNo")%>' />
                                                        <input type="hidden" id="hidBankCOA" runat="server" value='<%# Eval("BankAccountObject.BankCOA")%>' />
                                                        <input type="hidden" id="hidChequeFormatCode" runat="server" value='<%# Eval("BankAccountObject.ChequeFormatCode")%>' />
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNewTransactionNo" runat="server" Columns="10" MaxLength="10" />
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Trans Date" ItemStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTrnDate" runat="server">
												    <%# Eval("TransactionDate", "{0: dd-MMM-yyyy}")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" ID="editTransactionDate" MaxLength="10" Columns="10" Text='<%# Eval("TransactionDate", "{0: dd/MM/yyyy}") %>'></asp:TextBox>
                                                    <img id="imgCalendarEditFrom" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                        height="20" width="17" alt="" align="absMiddle" border="0">
                                                         <asp:RequiredFieldValidator ID="rfvEditTransactionDate" runat="server" ControlToValidate="editTransactionDate"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator
                                                            ID="cvEditTransactionDate" runat="server" ErrorMessage="Invalid Date" ControlToValidate="editTransactionDate" Type="Date" Display="Dynamic" ValidationGroup="valGrpEditRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="newTransactionDate" MaxLength="10" Columns="10"></asp:TextBox>
                                                    <img id="imgCalendarNewTrnDate" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                        height="20" width="17" alt="" align="absMiddle" border="0">
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                <FooterStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Type" HeaderStyle-Wrap="false" ItemStyle-Width="15px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblType" runat="server">
										        <%# Eval( "Type" )%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlEditType" runat="server">
                                                        <asp:ListItem>R</asp:ListItem>
                                                        <asp:ListItem>P</asp:ListItem>
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlNewType" runat="server">
                                                        <asp:ListItem>R</asp:ListItem>
                                                        <asp:ListItem>P</asp:ListItem>
                                                    </asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Received From/Pay To" HeaderStyle-Wrap="false" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" >
										           <%# Eval( "Name" )%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditName" runat="server" Columns="50" MaxLength="50" Text='<%# Eval("Name") %>' />
                                                    <asp:RequiredFieldValidator ID="rfvEditName" runat="server" ControlToValidate="txtEditName"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNewName" runat="server" Columns="50" MaxLength="50" autocomplete="off" onfocus="select();" />
                                                        <ajaxToolkit:AutoCompleteExtender
                                                            runat="server" 
                                                            BehaviorID="AutoCompleteEx"
                                                            ID="autoComplete1" 
                                                            TargetControlID="txtNewName"
                                                            ServicePath="AutoComplete.asmx" 
                                                            ServiceMethod="GetCompletionList"
                                                            MinimumPrefixLength="1" 
                                                            CompletionInterval="100"
                                                            EnableCaching="false"
                                                            CompletionSetCount="25"
                                                            DelimiterCharacters=",">
                                                        </ajaxToolkit:AutoCompleteExtender>
                                                </FooterTemplate>
                                                <ItemStyle Width="200px" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Mode" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMode" runat="server">
												   <%# Eval("Mode")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditMode" runat="server" Columns="10" MaxLength="10" Text='<%# Eval("Mode") %>' />
                                                    <asp:RequiredFieldValidator ID="rfvEditMode" runat="server" ControlToValidate="txtEditMode"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNewMode" runat="server" Columns="10" MaxLength="10" />
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Bank COA" HeaderStyle-Wrap="false" ItemStyle-Width="180px">
										    <ItemTemplate>
										        <asp:Label ID="lblBankCOA" runat="server">
										           <%# Eval( "BankAccountObject.BankCOA" )%>
											    </asp:Label>
										    </ItemTemplate>
										    <EditItemTemplate>
											    <asp:DropDownList ID="ddlEditBankCOAID" runat="Server" DataTextField="BankCodeCOABalance" DataValueField="COAID" />
										    </EditItemTemplate>
										    <FooterTemplate>
											    <asp:DropDownList ID="ddlNewBankCOAID" runat="Server" DataTextField="BankCodeCOABalance" DataValueField="COAID" OnChange="changeCurrency(this)"/>
										    </FooterTemplate>
									    </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Cheque Date" ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChequeDate" runat="server">
												    <%# Eval("ChequeDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("ChequeDate", "{0: dd-MMM-yyyy}")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" ID="editChequeDate" MaxLength="10" Columns="10" Text='<%# Eval("ChequeDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "" : Eval("ChequeDate", "{0: dd/MM/yyyy}")%>'></asp:TextBox>
                                                    <img id="imgCalendarEditFrom" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                        height="20" width="17" alt="" align="absMiddle" border="0"><asp:CompareValidator
                                                            ID="cvEditChequeDate" runat="server" ErrorMessage="Invalid Date" ControlToValidate="editChequeDate" Type="Date" Display="Dynamic" ValidationGroup="valGrpEditRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="newChequeDate" MaxLength="10" Columns="10"></asp:TextBox>
                                                    <img id="imgCalendarNewChequeDate" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                        height="20" width="17" alt="" align="absMiddle" border="0">
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                <FooterStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
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
                                            <asp:TemplateColumn HeaderText="Amount" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server">
												   <%# Eval("Amount")%>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditAmount" runat="server" Columns="15" MaxLength="15" Text='<%# Eval("Amount") %>' />
                                                    <asp:RequiredFieldValidator ID="rfvEditAmount" runat="server" ControlToValidate="txtEditAmount"
                                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:RangeValidator
                                                            ID="rgEditAmount" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditAmount" Type="Double" ValidationGroup="valGrpEditRow"  /></EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNewAmount" runat="server" Columns="15" MaxLength="15" />
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="NN" ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%# ( (bool)Eval( "Marking1" ) ) ? "Yes" : "No"%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkEditMarking1" runat="server" Checked='<%# Eval("Marking1") %>' />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:CheckBox ID="chkNewMarking1" runat="server" Checked="true" />
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="NB" ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%# ( (bool)Eval( "Marking2" ) ) ? "Yes" : "No"%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkEditMarking2" runat="server" Checked='<%# Eval("Marking2") %>' />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:CheckBox ID="chkNewMarking2" runat="server" Checked="true" />
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="" ItemStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                            <ASP:CHECKBOX ID="chkSelectAll" onclick="checkAll('chkPrint', this.checked);" Text=""
														RUNAT="server"></ASP:CHECKBOX>
                                            </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkPrint" onclick="DeselectMainCheckbox(this);" runat="server" Enabled='<%# Eval("Type").ToString().Equals("P") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                </EditItemTemplate>
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
												 CssClass="NewButt"><span>Add</span></asp:LinkButton>
										</FooterTemplate>
									    </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle CssClass="tGroupHeader" HorizontalAlign="Center" />
                                        <AlternatingItemStyle CssClass="tGroupAltRow" />
                                        <FooterStyle CssClass="tGroupFooter" />
                                        <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                                    </asp:DataGrid>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <br />
                        <div style="text-align:right;width:88%">
                        <IMG ID="img4" ALT="" SRC="../../images/icons/printIcon.gif" align="top" border="0">
													<ASP:LINKBUTTON id="LINKBUTTON1" onclick="printReport" RUNAT="server" TEXT="Print Selected Cheques"
														CSSCLASS="button" TOOLTIP="Please click to print selected cheuqes." CAUSESVALIDATION="False"></ASP:LINKBUTTON>
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
                        <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false"  />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        
    </form>
</body>
</html>
