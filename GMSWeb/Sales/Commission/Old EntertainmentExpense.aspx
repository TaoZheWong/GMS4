<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Old EntertainmentExpense.aspx.cs"
    Inherits="GMSWeb.Sales.Commission.EntertainmentExpense" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Commission - Entertainment Expenses</title>
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />

    <script language="javascript" type="text/javascript" src="/GMS/scripts/popcalendar.js"></script>

    <script type="text/javascript">
    function SelectOthers(chkbox)
	{
	    var prefix = chkbox.id.substring(0,chkbox.id.lastIndexOf("_")+1);
	    if (chkbox.checked)
	    {
	        document.getElementById(prefix+"spanSalesPersonMasterName").style.visibility = 'hidden';
	        document.getElementById(prefix+"spanArea").style.visibility = 'hidden';
	    } else
	    {
	        document.getElementById(prefix+"spanSalesPersonMasterName").style.visibility = 'visible';
	        document.getElementById(prefix+"spanArea").style.visibility = 'visible';
	    }
	} 
	 
    </script>

</head>
<body onload="javascript:document.getElementById('txtSearchName').focus();">
    <form id="form1" runat="server" defaultbutton="btnSearch">
        <asp:ScriptManager ID="sriptmgr1" runat="server">
        </asp:ScriptManager>
        <div id="GroupContentBar">
            <h3>
                Commission &gt; Entertainment Expenses</h3>
            List of entertainment expenses by salesman.
            <br />
            <br />
            <table class="tGroupTable" style="border-collapse: collapse" cellspacing="0" cellpadding="1"
                border="1" width="800px">
                <tr>
                    <td class="tbLabel" style="width: 20%">
                        Payee</td>
                    <td style="width: 5%">
                        :</td>
                    <td style="width: 20%">
                        <asp:TextBox ID="txtSearchName" runat="server" Columns="20" MaxLength="50" onfocus="select();"
                            CssClass="textbox" />
                    </td>
                    <td class="tbLabel" style="width: 20%">
                        Local/Export</td>
                    <td style="width: 5%">
                        :</td>
                    <td style="width: 20%">
                        <asp:DropDownList ID="ddlSearchArea" runat="server" CssClass="dropdownlist">
                            <asp:ListItem Value="%%">All</asp:ListItem>
                            <asp:ListItem Value="1">Local</asp:ListItem>
                            <asp:ListItem Value="2">Export</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Payment Date From</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtSearchDateFrom" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="textbox"></asp:TextBox>
                        <img id="imgCalendarEditFrom" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                            height="20" width="17" alt="" align="absMiddle" border="0" /></td>
                    <td class="tbLabel">
                        Payment Date To</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtSearchDateTo" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="textbox"></asp:TextBox>
                        <img id="img1" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                            height="20" width="17" alt="" align="absMiddle" border="0" /></td>
                </tr>
                <tr>
                    <td class="tbLabel">
                        Others</td>
                    <td>
                        :</td>
                    <td>
                        <asp:CheckBox ID="chkSearchOthers" runat="server" /></td>
                    <td class="tbLabel">
                    </td>
                    <td>
                        :</td>
                    <td>
                    </td>
                    <td style="width: 10%">
                        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="button"
                            OnClick="btnSearch_Click"></asp:Button></td>
                </tr>
            </table>
            <br />
            <table class="tGroupTable">
                <tr>
                    <td>
                        <div class="tGroupTable">
                            <div id="Div1" style="text-align: left; width: 1500px" runat="server">
                                <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                            </div>
                            <br />
                            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                DataKeyField="TransactionID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
                                OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" GridLines="none"
                                OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
                                CellPadding="5" CellSpacing="0" CssClass="tTable tBorder" AllowPaging="true"
                                PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                            <input type="hidden" id="hidTransactionID" runat="server" value='<%# Eval("TransactionID")%>' />
                                            .</ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Others" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                                CausesValidation="false"><span><%# ( (bool)Eval( "Others" ) ) ? "Yes" : "No"%></span></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkEditOthers" runat="server" Checked='<%# Eval("Others") %>' onclick="SelectOthers(this);" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="chkNewOthers" runat="server" onclick="SelectOthers(this);" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Payee" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server">
                                                        <%# Eval("SalesPersonMasterObject.SalesPersonMasterName")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <span id="spanSalesPersonMasterName" runat="server" style='<%# ((bool)Eval("others"))?"visibility:hidden": "visibility:visible" %>'>
                                                <asp:DropDownList ID="ddlEditSalesPersonMasterName" runat="Server" DataTextField="SalesPersonMasterName"
                                                    DataValueField="SalesPersonMasterID" />
                                            </span>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <span id="spanSalesPersonMasterName" runat="server">
                                                <asp:DropDownList ID="ddlNewSalesPersonMasterName" runat="Server" DataTextField="SalesPersonMasterName"
                                                    DataValueField="SalesPersonMasterID" /></span>
                                        </FooterTemplate>
                                        <ItemStyle Width="200px" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Payment Date" ItemStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTrnDate" runat="server">
												    <%# Eval("Date", "{0: dd-MMM-yyyy}")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="editPaymentDate" MaxLength="10" Columns="10" Text='<%# Eval("Date", "{0: dd/MM/yyyy}") %>'></asp:TextBox>
                                            <img id="imgCalendarEditFrom" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                height="20" width="17" alt="" align="absMiddle" border="0">
                                            <asp:RequiredFieldValidator ID="rfvEditPaymentDate" runat="server" ControlToValidate="editPaymentDate"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator
                                                    ID="cvEditPaymentDate" runat="server" ErrorMessage="Invalid Date" ControlToValidate="editPaymentDate"
                                                    Type="Date" Display="Dynamic" ValidationGroup="valGrpEditRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox runat="server" ID="newPaymentDate" MaxLength="10" Columns="10"></asp:TextBox>
                                            <img id="imgCalendarNewTrnDate" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                height="20" width="17" alt="" align="absmiddle" border="0">
                                            <asp:RequiredFieldValidator ID="rfvNewPaymentDate" runat="server" ControlToValidate="newPaymentDate"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator
                                                    ID="cvNewPaymentDate" runat="server" ErrorMessage="Invalid Date" ControlToValidate="newPaymentDate"
                                                    Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Area" HeaderStyle-Wrap="false" ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server">
										        <%# ((bool)Eval("Others"))?"":((Eval("Area").ToString() == "1") ? "Local" : "Export")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <span id="spanArea" runat="server" style='<%# ((bool)Eval("others"))?"visibility:hidden": "visibility:visible" %>'>
                                                <asp:DropDownList ID="ddlEditArea" runat="server">
                                                    <asp:ListItem Value="1">Local</asp:ListItem>
                                                    <asp:ListItem Value="2">Export</asp:ListItem>
                                                </asp:DropDownList>
                                            </span>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <span id="spanArea" runat="server">
                                                <asp:DropDownList ID="ddlNewArea" runat="server">
                                                    <asp:ListItem Value="1">Local</asp:ListItem>
                                                    <asp:ListItem Value="2">Export</asp:ListItem>
                                                </asp:DropDownList>
                                            </span>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Amount" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server">
												   <%# Eval("Amount","{0:f2}")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditAmount" runat="server" Columns="15" MaxLength="15" Text='<%# Eval("Amount", "{0:f2}") %>' />
                                            <asp:RequiredFieldValidator ID="rfvEditAmount" runat="server" ControlToValidate="txtEditAmount"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:RangeValidator
                                                    ID="rgEditAmount" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditAmount"
                                                    Type="Double" ValidationGroup="valGrpEditRow" /></EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewAmount" runat="server" Columns="15" MaxLength="15" />
                                            <asp:RequiredFieldValidator ID="rfvNewAmount" runat="server" ControlToValidate="txtNewAmount"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:RangeValidator
                                                    ID="rgNewAmount" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewAmount"
                                                    Type="Double" ValidationGroup="valGrpNewRow" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Remark" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemark" runat="server">
												   <%# Eval("Remark")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditRemark" runat="server" Columns="15" MaxLength="50" Text='<%# Eval("Remark") %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewRemark" runat="server" Columns="15" MaxLength="50" />
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
                                <HeaderStyle CssClass="tGroupHeader" HorizontalAlign="Center" />
                                <AlternatingItemStyle CssClass="tGroupAltRow" />
                                <FooterStyle CssClass="tGroupFooter" />
                                <PagerStyle Font-Bold="true" HorizontalAlign="Center" Mode="NumericPages" />
                            </asp:DataGrid>
                        </div>
                        <div style="text-align: left; width: 88%">
                            <br />
                            <asp:DropDownList ID="ddlReport" runat="server" CssClass="dropdownlist">
                                <asp:ListItem Value="EntertainmentExpensesReport">Entertainment Expenses Report</asp:ListItem>
                            </asp:DropDownList>
                            <asp:LinkButton ID="LINKBUTTON1" OnClick="GenerateReport" runat="server" Text="Print"
                                CssClass="button" ToolTip="Please click to print report." CausesValidation="False"><img id="img4" alt="" src="../../images/icons/printIcon.gif" align="top" border="0" /></asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </table>
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
