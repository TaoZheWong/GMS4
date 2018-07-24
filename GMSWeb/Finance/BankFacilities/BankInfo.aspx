<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="BankInfo.aspx.cs" Inherits="GMSWeb.Finance.BankFacilities.BankInfo" Title="Finance - Bank Info Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">Cash Flow/Loan</a></li>
        <li class="active">Bank Facility</li>
    </ul>
    <h1 class="page-header">Bank Facility <small>List of current bank facilities.</small></h1>

    <div class="note note-info">
        <h4 class="block"><i class="ti-info-alt"></i>Info! </h4>
        <ul>
            <li>The following bank facilities' balances are updated based on trial balance data. </li>
            <li>If the Bank Code's value is N.A, user has to update it to the correct Bank Code.</li>
            <li>For cash accounts, Facility Limit and Utilised values are not applicable.</li>
            <li>For non-cash accounts, <b>Utilised = Facility Limit - Balance.</b></li>
            <li>Values for Facility Limit, Utilised and Balance are based on company default currency. </li>
        </ul>

        <div class="row">
            <div class="col-sm-4">
                <ul>
                    <li>10XX - Cash at Bank  </li>
                    <li>30XX - Bank Overdraft </li>
                    <li>31XX - Revolving Credit </li>
                </ul>
            </div>
            <div class="col-sm-4">
                <ul>
                    <li>32XX - Factoring/Bills Payable/TR </li>
                    <li>33XX - Short-Term Loan </li>
                    <li>34XX - Term Loan - Current </li>
                </ul>
            </div>
            <div class="col-sm-4">
                <ul>
                    <li>35XX - HP Creditors - Current </li>
                    <li>41XX - Long-Term Loan </li>
                    <li>42XX - HP Creditors</li>
                </ul>
            </div>
        </div>
    </div>


    <atlas:ScriptManager ID="scriptMgr" runat="server" />

        <div class="panel panel-primary">
            <div class="panel-heading">
                <div class="panel-heading-btn">
                    <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                    </div>
                <h4 class="panel-title">
                    <i class="ti-align-justify"></i> 
                    <asp:Label ID="lblSearchSummary" Visible="false" runat="server"></asp:Label>
                </h4>
            </div>
            <div class="table-responsive">
                    <atlas:UpdatePanel ID="udpBankUpdater" runat="server" Mode="conditional">
                        <ContentTemplate>
                            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                DataKeyField="COAID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
                                OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand"
                                GridLines="none" OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
                                CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="No">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Bank COA" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBankCOA" runat="server">
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                                    CausesValidation="false"><span><%# Eval( "BankCOA" )%></span></asp:LinkButton>
                                                <input type="hidden" id="hidCOAID" runat="server" value='<%# Eval("COAID")%>' />
                                                <input type="hidden" id="hidBankCOA" runat="server" value='<%# Eval("BankCOA")%>' />
                                            </asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtNewBankCOA" runat="server" Columns="8" MaxLength="10" />
                                            <asp:RequiredFieldValidator ID="rfvNewBankCOA" runat="server" ControlToValidate="txtNewBankCOA"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Bank Code" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBankCode" runat="server">
                                                <%# Eval( "BankObject.BankCode" )%>
                                                <input type="hidden" id="hidBankCode" runat="server" value='<%# Eval("BankObject.BankCode")%>' />
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control input-sm" ID="ddlEditBankCode" runat="Server" DataTextField="BankCode" DataValueField="BankID" Enabled="False" />
                                        </EditItemTemplate>

                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Currency">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrency" runat="server">
												    <%# Eval("Currency")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control input-sm" ID="ddlEditCurrency" runat="Server" DataTextField="CurrencyCode" DataValueField="CurrencyCode" Enabled="False" />
                                        </EditItemTemplate>



                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Cheque Format" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChequeFormat" runat="server">
										        <%# Eval( "ChequeFormatCode" )%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control input-sm" ID="ddlEditChequeFormatCode" runat="server">
                                                <asp:ListItem Value="N.A">N.A</asp:ListItem>
                                                <asp:ListItem Value="2007SG">2007SG</asp:ListItem>
                                                <asp:ListItem Value="2007ID">2007ID</asp:ListItem>
                                                <asp:ListItem Value="2007TH">2007TH</asp:ListItem>
                                                <asp:ListItem Value="2010ID">2010ID</asp:ListItem>
                                                <asp:ListItem Value="2011ID">2011ID</asp:ListItem>
                                                <asp:ListItem Value="2014ID">2014ID</asp:ListItem>
                                                <asp:ListItem Value="2012PH">2012PH</asp:ListItem>
                                                <asp:ListItem Value="2017SG">2017SG</asp:ListItem>
                                                <asp:ListItem Value="2017ID">2017ID</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>

                                        <FooterTemplate>
                                            <asp:DropDownList CssClass="form-control input-sm" ID="ddlNewChequeFormatCode" runat="server">
                                                <asp:ListItem Value="N.A">N.A</asp:ListItem>
                                                <asp:ListItem Value="2007SG">2007SG</asp:ListItem>
                                                <asp:ListItem Value="2007ID">2007ID</asp:ListItem>
                                                <asp:ListItem Value="2007TH">2007TH</asp:ListItem>
                                                <asp:ListItem Value="2010ID">2010ID</asp:ListItem>
                                                <asp:ListItem Value="2011ID">2011ID</asp:ListItem>
                                                <asp:ListItem Value="2014ID">2014ID</asp:ListItem>
                                                <asp:ListItem Value="2012PH">2012PH</asp:ListItem>
                                                <asp:ListItem Value="2017SG">2017SG</asp:ListItem>
                                                <asp:ListItem Value="2017ID">2017ID</asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>

                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Facility Limit" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLimit" runat="server">										          
										            <%# (((Eval("BankCOA").ToString().Substring(0, 2) == "10") && Convert.ToDecimal(Eval("DefaultCurrencyBalance")) >= 0) || (Eval("BankCOA").ToString().Substring(0, 2) == "11") || (Eval("BankCOA").ToString().Substring(0, 2) == "34") || (Eval("BankCOA").ToString().Substring(0, 2) == "35") || (Eval("BankCOA").ToString().Substring(0, 2) == "41") || (Eval("BankCOA").ToString().Substring(0, 2) == "42")) ? "N.A" : Eval("Limit", "{0:n}")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtEditLimit" runat="server" Columns="10" MaxLength="15" Text='<%# Eval("Limit") %>' Enabled='<%# (((Eval("BankCOA").ToString().Substring(0, 2) == "10") && Convert.ToDecimal(Eval("DefaultCurrencyBalance")) >= 0) || (Eval("BankCOA").ToString().Substring(0, 2) == "11") ||  (Eval("BankCOA").ToString().Substring(0, 2) == "34") || (Eval("BankCOA").ToString().Substring(0, 2) == "35") || (Eval("BankCOA").ToString().Substring(0, 2) == "41") || (Eval("BankCOA").ToString().Substring(0, 2) == "42")) ? Convert.ToBoolean(0) : Convert.ToBoolean(1) %>' />
                                            <asp:CompareValidator ID="cvEditLimit" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtEditLimit" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
                                        </EditItemTemplate>

                                        <FooterTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtNewLimit" runat="server" Columns="10" MaxLength="15" /><asp:CompareValidator ID="cvNewLimit" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtNewLimit" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
                                        </FooterTemplate>

                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Int. Rate" HeaderStyle-Wrap="false" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInterestRate" runat="server">
										        <%# Eval( "InterestRate" )%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtEditInterestRate" runat="server" Columns="10" MaxLength="20" Text='<%# Eval("InterestRate") %>' />
                                            <asp:RequiredFieldValidator ID="rfvEditInterestRate" runat="server" ControlToValidate="txtEditInterestRate"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                        </EditItemTemplate>

                                        <FooterTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtNewInterestRate" runat="server" Columns="10" MaxLength="20" />
                                            <asp:RequiredFieldValidator ID="rfvNewInterestRate" runat="server" ControlToValidate="txtNewInterestRate"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                        </FooterTemplate>

                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Utilised" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBalance" runat="server">
										            <%# ((Eval("BankCOA").ToString().Substring(0, 2) == "10" || Eval("BankCOA").ToString().Substring(0, 2) == "11") && Convert.ToDecimal(Eval("DefaultCurrencyBalance")) >= 0)  ? "N.A" : string.Format("{0:0,0.00}", Eval("DefaultCurrencyBalance"))%>										           
                                            </asp:Label>
                                        </ItemTemplate>

                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtEditBalance" runat="server" Columns="10" MaxLength="15" Text='<%# Eval("DefaultCurrencyBalance") %>' /><asp:CompareValidator ID="cvEditBalance" runat="server" ErrorMessage="Not a number" Display="Dynamic"
                                                ControlToValidate="txtEditBalance" Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" ID="txtNewBalance" runat="server" Columns="10" MaxLength="20" />

                                        </FooterTemplate>

                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Balance" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFinalBalance" runat="server">
										            <%# ((Eval("BankCOA").ToString().Substring(0, 2) == "10" && Convert.ToDecimal(Eval("DefaultCurrencyBalance")) < 0) || Eval("BankCOA").ToString().Substring(0, 2) == "30" || Eval("BankCOA").ToString().Substring(0, 2) == "31" || Eval("BankCOA").ToString().Substring(0, 2) == "32" || Eval("BankCOA").ToString().Substring(0, 2) == "33") ? string.Format("{0:0,0.00}", Convert.ToDecimal(Eval("Limit")) + Convert.ToDecimal(Eval("DefaultCurrencyBalance"))) : string.Format("{0:0,0.00}", Convert.ToDecimal(Eval("DefaultCurrencyBalance")))%>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Major Bank">
                                        <ItemTemplate>
                                            <%# ( (bool)Eval( "isMajorBank" ) ) ? "Yes" : "No"%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="checkbox custom-for-click input-sm no-margin">
                                                <asp:CheckBox ID="chkIsMajorBank" runat="server" Checked='<%# Eval("isMajorBank") %>' />
                                                <label></label>
                                            </div>
                                        </EditItemTemplate>

                                        <FooterTemplate>
                                            <div class="checkbox custom-for-click input-sm no-margin">
                                                <asp:CheckBox ID="chkIsMajorBank" runat="server" Checked="false" />
                                                <label></label>
                                            </div>
                                        </FooterTemplate>

                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Maturity Date" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMaturityDate" runat="server">
												    <%# Eval("MaturityDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("MaturityDate", "{0: dd-MMM-yyyy}")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" runat="server" ID="editMaturityDate" MaxLength="10" Columns="10" Text='<%# Eval("MaturityDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "" : Eval("MaturityDate", "{0: dd/MM/yyyy}")%>'></asp:TextBox>
                                            <img id="imgCalendarEditMaturityDate" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                height="20" width="17" alt="" align="absMiddle" border="0"><asp:CompareValidator
                                                    ID="cvEditMaturityDate" runat="server" ErrorMessage="Invalid Date" ControlToValidate="editMaturityDate" Type="Date" Display="Dynamic" ValidationGroup="valGrpEditRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                        </EditItemTemplate>

                                        <FooterTemplate>
                                            <asp:TextBox CssClass="form-control input-sm" runat="server" ID="newMaturityDate" MaxLength="10" Columns="10"></asp:TextBox>
                                            <img id="imgCalendarNewMaturityDate" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
                                                height="20" width="17" alt="" align="absMiddle" border="0"><asp:CompareValidator
                                                    ID="cvNewMaturityDate" runat="server" ErrorMessage="Invalid Date" ControlToValidate="newMaturityDate" Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                        </FooterTemplate>

                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn  HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDelete" runat="server" Text=""></asp:Label>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false" CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete">
                                <i class="ti-trash"></i> </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                                ValidationGroup="valGrpEditRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                                CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                                        </EditItemTemplate>

                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                                ValidationGroup="valGrpNewRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                                        </FooterTemplate>

                                    </asp:TemplateColumn>
                                </Columns>
                                <HeaderStyle CssClass="tHeader" />
                                <FooterStyle CssClass="tFooter" />
                                <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                            </asp:DataGrid>
                        </ContentTemplate>
                    </atlas:UpdatePanel>
            </div>
        </div> 


        <!--To do : Update to see -->
        <asp:Button ID="btnImportGroup" Text="Import (Group)" EnableViewState="False" runat="server" CssClass="button" OnClick="btnImportGroup_Click" OnClientClick="progress_update()" Visible="false"></asp:Button>
        <asp:Button ID="btnImport" Text="Import" EnableViewState="False" runat="server" CssClass="btn btn-primary" OnClick="btnImport_Click" OnClientClick="progress_update()" Visible="false" disabled="true"></asp:Button>
          
        <div id="importing" style="visibility: hidden"><span style="font-style: italic;">Importing ...</span></div>
           
        <div id="showbar" style="font-size: 8pt; padding: 2px; border: solid #D8D8D8 1px; visibility: hidden">
            <span id="progress1">&nbsp; &nbsp;</span>
            <span id="progress2">&nbsp; &nbsp;</span>
            <span id="progress3">&nbsp; &nbsp;</span>
            <span id="progress4">&nbsp; &nbsp;</span>
            <span id="progress5">&nbsp; &nbsp;</span>
            <span id="progress6">&nbsp; &nbsp;</span>
            <span id="progress7">&nbsp; &nbsp;</span>
            <span id="progress8">&nbsp; &nbsp;</span>
            <span id="progress9">&nbsp; &nbsp;</span>
        </div>
         
    <br />
    <div class="TABCOMMAND">
        <atlas:UpdatePanel ID="udpMsgUpdater" runat="server" Mode="Always">
            <ContentTemplate>
                <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </atlas:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".cash-flow-menu").addClass("active expand");
            $(".sub-bank-info").addClass("active");
        });
    </script>
</asp:Content>
