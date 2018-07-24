<%@ Page Language="C#" AutoEventWireup="true" Codebehind="PMCommission.aspx.cs" Inherits="GMSWeb.Sales.Commission.PMCommission" %>

<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Commission - Product Manager Commission</title>
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <script language="javascript" type="text/javascript" src="/GMS/scripts/popcalendar.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sriptmgr1" runat="server">
        </asp:ScriptManager>
        <div id="GroupContentBar">
            <h3>
                Commission &gt; Product Manager Commission</h3>
            Master record of commission rate for product manager.
            <br />
            <br />
            <div style="display: none">
                <asp:TextBox ID="txtBoxID" runat="server" Columns="100" MaxLength="100" Text="" />
                <asp:TextBox ID="txtHiddenID" runat="server" Columns="100" MaxLength="100" Text="" />
                <asp:TextBox ID="txtEditBoxID" runat="server" Columns="100" MaxLength="100" Text="" />
                <asp:TextBox ID="txtEditHiddenID" runat="server" Columns="100" MaxLength="100" Text="" />
            </div>
            <table class="tGroupTable" style="width: 100%">
                <tr>
                    <td>
                        <br />
                        <div id="Div1" style="text-align: left; width: 1000px" runat="server">
                            <asp:Label ID="lblSearchSummary" Visible="false" runat="server"></asp:Label>
                        </div>
                        <br />
                        <div class="tGroupTable">
                            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                DataKeyField="ProductGroupManagerID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
                                GridLines="none" OnItemDataBound="dgData_ItemDataBound" CellPadding="5" CellSpacing="0"
                                OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" OnDeleteCommand="dgData_DeleteCommand"
                                CssClass="tTable tBorder" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged"
                                EnableViewState="true">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                            <input type="hidden" id="hidProductGroupManagerID" runat="server" value='<%# Eval("ProductGroupManagerID")%>' />
                                            <input type="hidden" id="hidEfffectiveDate" runat="server" value='<%# Eval("EffectiveDate")%>' />
                                            .</ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="120px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductGroupManagerName" runat="server">
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                                    CausesValidation="false"><span><%# Eval("ProductGroupManagerObject.ProductGroupManagerName")%></span></asp:LinkButton>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlEditProductGroupManagerName" runat="Server" DataTextField="ProductGroupManagerName"
                                                DataValueField="ProductGroupManagerID" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlNewProductGroupManagerName" runat="Server" DataTextField="ProductGroupManagerName"
                                                DataValueField="ProductGroupManagerID" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Monthly Comm Rate" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Wrap="false" ItemStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMonthlyCommRate" runat="server">
										           <%# Eval("MonthlyCommissionRate")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditMonthlyCommissionRate" runat="server" Columns="10" MaxLength="10"
                                                Text='<%# Eval("MonthlyCommissionRate") %>' /><asp:CompareValidator ID="cvEditMonthlyCommissionRate"
                                                        runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtEditMonthlyCommissionRate"
                                                        Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewMonthlyCommissionRate" runat="server" Columns="10" MaxLength="10" /><asp:CompareValidator
                                                    ID="cvNewMonthlyCommissionRate" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                    ControlToValidate="txtNewMonthlyCommissionRate" Type="Double" Operator="DataTypeCheck"
                                                    ValidationGroup="valGrpNewRow" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Quarterly Comm Rate" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Wrap="false" ItemStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuarterlyCommRate" runat="server">
										           <%# Eval("QuarterlyCommissionRate")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditQuarterlyCommissionRate" runat="server" Columns="10" MaxLength="10"
                                                Text='<%# Eval("QuarterlyCommissionRate") %>' /><asp:CompareValidator ID="cvEditQuarterlyCommissionRate"
                                                        runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtEditQuarterlyCommissionRate"
                                                        Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtNewQuarterlyCommissionRate" runat="server" Columns="10" MaxLength="10" /><asp:CompareValidator
                                                    ID="cvNewQuarterlyCommissionRate" runat="server" ErrorMessage="Not a number"
                                                    Display="Dynamic" ControlToValidate="txtNewQuarterlyCommissionRate" Type="Double"
                                                    Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Effective Date" ItemStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEffectiveDate" runat="server">
												    <%# Eval("EffectiveDate", "{0: dd-MMM-yyyy}")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="txtEditEffectiveDate" MaxLength="10" Columns="10" Text='<%# Eval("EffectiveDate", "{0: dd/MM/yyyy}") %>'></asp:TextBox>
                                            <img id="imgCalendarEditEffectiveDate" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                height="20" width="17" alt="" align="absMiddle" border="0">
                                            <asp:RequiredFieldValidator ID="rfvEditEffectiveDate" runat="server" ControlToValidate="txtEditEffectiveDate"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator
                                                    ID="cvEditEffectiveDate" runat="server" ErrorMessage="Invalid Date" ControlToValidate="txtEditEffectiveDate"
                                                    Type="Date" Display="Dynamic" ValidationGroup="valGrpEditRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox runat="server" ID="txtNewEffectiveDate" MaxLength="10" Columns="10"></asp:TextBox>
                                            <img id="imgCalendarNewEffectiveDate" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                height="20" width="17" alt="" align="absmiddle" border="0">
                                            <asp:RequiredFieldValidator ID="rfvNewEffectiveDate" runat="server" ControlToValidate="txtNewEffectiveDate"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator
                                                    ID="cvNewEffectiveDate" runat="server" ErrorMessage="Invalid Date" ControlToValidate="txtNewEffectiveDate"
                                                    Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow" Operator="DataTypeCheck"></asp:CompareValidator>
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
                        </div>
                        <div style="text-align:left;width:88%">
				    <br />
                        <asp:DropDownList ID="ddlReport" runat="server" CssClass="dropdownlist">
                        <asp:ListItem Value="MonthlyCommissionPayableReport_ProductManager">Monthly Commission Payable Report (ProductManager)</asp:ListItem>
                        <asp:ListItem Value="QuarterlyCommissionPayableReport_ProductManager">Quarterly Commission Payable Report (ProductManager)</asp:ListItem>
                        <asp:ListItem Value="MonthlyCommissionPayableReport_Rental">Monthly Commission Payable Report (Rental)</asp:ListItem>
                        </asp:DropDownList>
													<ASP:LINKBUTTON id="LINKBUTTON1" onclick="GenerateReport" RUNAT="server" TEXT="Print"
														CSSCLASS="button" TOOLTIP="Please click to print report." CAUSESVALIDATION="False"><img id="img4" alt="" src="../../images/icons/printIcon.gif" align="top" border="0" /></ASP:LINKBUTTON>
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
