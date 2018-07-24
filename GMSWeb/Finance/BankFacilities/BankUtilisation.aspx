<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="BankUtilisation.aspx.cs" Inherits="GMSWeb.Finance.BankFacilities.BankUtilisation" Title="Finance - Bank Utilisation Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="outter" runat="server">
        <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
        <asp:ScriptManager ID="sriptmgr1" runat="server">

            <Services>
                <asp:ServiceReference Path="AutoComplete.asmx" />
            </Services>
        </asp:ScriptManager>

        <ul class="breadcrumb pull-right">
            <li><a href="#">Cash Flow/Loan</a></li>
            <li class="active">Bank Utilisation</li>
        </ul>
        <h1 class="page-header">Bank Utilisation <small>List of transactions for cash accounts. </small></h1>
        <div class="note note-info">
            <h4 class="block"><i class="ti-info-alt"></i>&nbsp;Info! </h4>
            <ul>
                <li>Click on the "Import" button to import/update data from A21 system.</li>
                <li>User can only edit "Received From/Pay To", "Cheque Date", "NN" and "NB" fields. Other fields are retrieved from A21 system and have to be modified in A21 system. </li>
                <li>The checkbox next to "NB" are applicable for "PV" type only and the cheque format must be set for the selected bank.</li>
            </ul>
        </div>

        <div class="panel panel-primary">
            <div class="panel-heading">
                <div class="panel-heading-btn">
                    <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                </div>
                <h4 class="panel-title">
                    <i class="ti-search"></i> 
                        Search filter
                </h4>
            </div>
            <div class="panel-body">
                <div class="m-t-20">
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Received From/Pay To</label>
                            <asp:TextBox ID="txtSearchName" runat="server" Columns="20" MaxLength="50" onfocus="select();" CssClass="form-control" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Mode</label>
                            <asp:TextBox ID="txtSearchMode" runat="server" Columns="10" MaxLength="10" onfocus="select();" CssClass="form-control" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Transaction No</label>
                            <asp:TextBox ID="txtSearchTrnNo" runat="server" Columns="10" MaxLength="10" onfocus="select();" CssClass="form-control" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Cheque No</label>
                            <asp:TextBox ID="txtSearchCHequeNo" runat="server" Columns="10" MaxLength="10" onfocus="select();" CssClass="form-control" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Transaction Date From</label>
                            <div class="input-group date">
                                <asp:TextBox runat="server" ID="trnDateFrom" MaxLength="10" Columns="10" onfocus="select();" CssClass="form-control datepicker"></asp:TextBox>
                                <span class="input-group-addon"><i class="ti-calendar"></i></span>
					        </div>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Transaction Date To</label>
                            <div class="input-group date">
                                <asp:TextBox runat="server" ID="trnDateTo" MaxLength="10" Columns="10" onfocus="select();" CssClass="form-control datepicker"></asp:TextBox>
                                <span class="input-group-addon"><i class="ti-calendar"></i></span>
					        </div>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Cheque Date From</label>
                            <div class="input-group date">
                                <asp:TextBox runat="server" ID="chequeDateFrom" MaxLength="10" Columns="10" onfocus="select();" CssClass="form-control datepicker"></asp:TextBox>
                                <span class="input-group-addon"><i class="ti-calendar"></i></span>
					        </div>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Cheque Date To</label>
                            <div class="input-group date">
                                <asp:TextBox runat="server" ID="chequeDateTo" MaxLength="10" Columns="10" onfocus="select();" CssClass="form-control datepicker"></asp:TextBox>
                                <span class="input-group-addon"><i class="ti-calendar"></i></span>
					        </div>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Bank Account</label>
                            <asp:DropDownList ID="ddlBankCOA" runat="Server" DataTextField="BankCodeCOA" DataValueField="COAID" CssClass="form-control" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-6">
                        <label class="control-label">Type</label>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                                <asp:ListItem Value="%">-All-</asp:ListItem>
                                <asp:ListItem Value="RR">RR</asp:ListItem>
                                <asp:ListItem Value="PV">PV</asp:ListItem>
                                <asp:ListItem Value="PV">GJ</asp:ListItem>
                            </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="panel-footer clearfix">
                <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="btn btn-default pull-right m-l-10" OnClick="btnSearch_Click"></asp:Button>
                <asp:Button ID="btnImport" Text="Import" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right" OnCommand="ImportCommand"></asp:Button>
            </div>
        </div>


        <span style="visibility: hidden">
            <asp:DropDownList ID="ddlBankAccount" runat="server" DataTextField="COAID" DataValueField="Currency" />
        </span>
        <br />
      
        <div class="panel panel-primary">
            <div class="panel-heading">
                <div class="panel-heading-btn">
                    <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                    </div>
                <h4 class="panel-title">
                    <i class="ti-align-justify"></i> 
                    <asp:Label ID="lblSearchSummary" Visible="false" runat="server" />
                </h4>
            </div>
            <div class="panel-body no-padding">
                <div class="table-responsive">
                    <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="udpBankUtilUpdater">
                        <ContentTemplate>
                            <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                DataKeyField="TransactionNo" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
                                OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" GridLines="none"
                                OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
                                CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                                PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="No">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Trans No" HeaderStyle-Wrap="false" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTrnNo" runat="server">
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                                    CausesValidation="false"><span><%# Eval( "TransactionNo" )%></span></asp:LinkButton>
                                                <input type="hidden" id="hidBUID" runat="server" value='<%# Eval("BUID")%>' />
                                                <input type="hidden" id="hidTransactionNo" runat="server" value='<%# Eval("TransactionNo")%>' />
                                                <input type="hidden" id="hidTransactionType" runat="server" value='<%# Eval("Type")%>' />
                                                <input type="hidden" id="hidBankCoaID" runat="server" value='<%# Eval("BankAccountObject.COAID")%>' />
                                                <input type="hidden" id="hidBankCOA" runat="server" value='<%# Eval("BankAccountObject.BankCOA")%>' />
                                                <input type="hidden" id="hidChequeFormatCode" runat="server" value='<%# Eval("BankAccountObject.ChequeFormatCode")%>' />
                                            </asp:Label>
                                        </ItemTemplate>
                                        <%-- <FooterTemplate>
								                <asp:TextBox ID="txtNewTransactionNo" runat="server" Columns="10" MaxLength="10" />
							                </FooterTemplate> --%>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Trans Date" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTrnDate" runat="server">
								                <%# Eval("TransactionDate", "{0: dd-MMM-yyyy}")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control" runat="server" ID="editTransactionDate" MaxLength="10" Columns="10" Text='<%# Eval("TransactionDate", "{0: dd/MM/yyyy}") %>' ReadOnly="true"></asp:TextBox>

                                        </EditItemTemplate>
                                        <%-- <FooterTemplate>
								                <asp:TextBox CssClass="form-control" runat="server" ID="newTransactionDate" MaxLength="10" Columns="10"></asp:TextBox>
								                <img id="imgCalendarNewTrnDate" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
									                height="20" width="17" alt="" align="absMiddle" border="0">
							                </FooterTemplate> --%>
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Type" HeaderStyle-Wrap="false" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server">
							                <%# Eval( "Type" )%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control" ID="ddlEditType" runat="server" Enabled="false">
                                                <asp:ListItem Value="RR">RR</asp:ListItem>
                                                <asp:ListItem Value="PV">PV</asp:ListItem>
                                                <asp:ListItem Value="PV">GJ</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <%--
							                <FooterTemplate>
								                <asp:DropDownList CssClass="form-control" ID="ddlNewType" runat="server">
									                <asp:ListItem Value="RR">RR</asp:ListItem>
									                <asp:ListItem Value="PV">PV</asp:ListItem>
									                <asp:ListItem Value="PV">GJ</asp:ListItem>
								                </asp:DropDownList>
							                </FooterTemplate>
                                        --%>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Received From/Pay To" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server">
								                <%# Eval( "Name" )%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control" ID="txtEditName" runat="server" Columns="50" MaxLength="75" autocomplete="off" onfocus="select();" Text='<%# Eval("Name") %>' />
                                            <ajaxToolkit:AutoCompleteExtender
                                                runat="server"
                                                BehaviorID="AutoCompleteEx"
                                                ID="autoComplete1"
                                                TargetControlID="txtEditName"
                                                ServicePath="AutoComplete.asmx"
                                                ServiceMethod="GetCompletionList"
                                                MinimumPrefixLength="1"
                                                CompletionInterval="100"
                                                EnableCaching="false"
                                                CompletionSetCount="25"
                                                DelimiterCharacters=",">
                                            </ajaxToolkit:AutoCompleteExtender>
                                            <asp:RequiredFieldValidator ID="rfvEditName" runat="server" ControlToValidate="txtEditName"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                        </EditItemTemplate>
                                        <%--
							                <FooterTemplate>
								                <asp:TextBox CssClass="form-control" ID="txtNewName" runat="server" Columns="50" MaxLength="50" autocomplete="off" onfocus="select();" />
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
							                </FooterTemplate> --%>

                                        <ItemStyle/>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Mode">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMode" runat="server">
								                <%# Eval("Mode")%> 
												   
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control" ID="txtEditMode" runat="server" Columns="10" ReadOnly="true" MaxLength="10" Text='<%# Eval("Mode") %>' />
                                            <asp:RequiredFieldValidator ID="rfvEditMode" runat="server" ControlToValidate="txtEditMode"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                        </EditItemTemplate>
                                        <%--
							                <FooterTemplate>
								                <asp:TextBox CssClass="form-control" ID="txtNewMode" runat="server" Columns="10" MaxLength="10" />
							                </FooterTemplate>
                                        --%>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Bank COA" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBankCOA" runat="server">
								                <%# Eval( "BankAccountObject.BankCOA" )%>										           
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control" ID="ddlEditBankCOAID" runat="Server" DataTextField="BankCodeCOABalance" DataValueField="COAID" Enabled="false" />
                                        </EditItemTemplate>
                                        <%--
						                <FooterTemplate>
							                <asp:DropDownList CssClass="form-control" ID="ddlNewBankCOAID" runat="Server" DataTextField="BankCodeCOABalance" DataValueField="COAID" OnChange="changeCurrency(this)"/>
						                </FooterTemplate>
                                        --%>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Cheque Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChequeDate" runat="server">
								                <%# Eval("ChequeDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "Nill" : Eval("ChequeDate", "{0: dd-MMM-yyyy}")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="input-group date">
                                                <asp:TextBox CssClass="form-control" runat="server" ID="editChequeDate" MaxLength="10" Columns="10" Text='<%# Eval("ChequeDate").ToString().Equals("1/01/1900 12:00:00 AM") ? "" : Eval("ChequeDate", "{0: dd/MM/yyyy}")%>'></asp:TextBox>
                                                <span class="input-group-addon">
                                                    <i class="ti-calendar"></i>
                                                </span>
                                            </div>
                                            <asp:CompareValidator
                                                ID="cvEditChequeDate" runat="server" ErrorMessage="Invalid Date" ControlToValidate="editChequeDate" Type="Date" Display="Dynamic" ValidationGroup="valGrpEditRow" Operator="DataTypeCheck"></asp:CompareValidator>
                                        </EditItemTemplate>
                                        <%--
							                <FooterTemplate>
								                <asp:TextBox CssClass="form-control" runat="server" ID="newChequeDate" MaxLength="10" Columns="10"></asp:TextBox>
								                <img id="imgCalendarNewChequeDate" src="../../images/imgCalendar.gif" onclick="showCalendar(this, this.parentElement.all(0), 'dd/mm/yyyy', null, 1);"
									                height="20" width="17" alt="" align="absMiddle" border="0">
							                </FooterTemplate>
                                        --%>
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Currency">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrency" runat="server">
								                <%# Eval("Currency")%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList CssClass="form-control" ID="ddlEditCurrency" runat="Server" DataTextField="CurrencyCode" DataValueField="CurrencyCode" Enabled="False" />
                                        </EditItemTemplate>
                                        <%--
						                <FooterTemplate>
							                <asp:DropDownList CssClass="form-control" ID="ddlNewCurrency" runat="Server" DataTextField="CurrencyCode" DataValueField="CurrencyCode" />
						                </FooterTemplate>
                                        --%>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Amount" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server">
								                <%# string.Format("{0:0,0.00}",Eval("Amount"))%>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox CssClass="form-control" ID="txtEditAmount" runat="server" Columns="15" MaxLength="15" ReadOnly="true" Text='<%# Eval("Amount") %>' />
                                            <asp:RequiredFieldValidator ID="rfvEditAmount" runat="server" ControlToValidate="txtEditAmount"
                                                ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:RangeValidator
                                                    ID="rgEditAmount" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditAmount" Type="Double" ValidationGroup="valGrpEditRow" />
                                        </EditItemTemplate>
                                        <%--
							                <FooterTemplate>
								                <asp:TextBox CssClass="form-control" ID="txtNewAmount" runat="server" Columns="15" MaxLength="15" />
							                </FooterTemplate>
                                        --%>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="NN" >
                                        <ItemTemplate>
                                            <%# ( (bool)Eval( "Marking1" ) ) ? "Yes" : "No"%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="checkbox input-sm no-margin">
                                                <asp:CheckBox ID="chkEditMarking1" runat="server" Checked='<%# Eval("Marking1") %>' Text=" "/>
                                            </div>
                                        </EditItemTemplate>
                                        <%-- <FooterTemplate>
								                <asp:CheckBox ID="chkNewMarking1" runat="server" Checked="true" />
							                </FooterTemplate> --%>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="NB">
                                        <ItemTemplate>
                                            <%# ( (bool)Eval( "Marking2" ) ) ? "Yes" : "No"%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="checkbox input-sm no-margin">
                                                <asp:CheckBox ID="chkEditMarking2" runat="server" Checked='<%# Eval("Marking2") %>' Text=" " />
                                            </div>
                                        </EditItemTemplate>
                                        <%-- <FooterTemplate>
								                <asp:CheckBox ID="chkNewMarking2" runat="server" Checked="true" />
							                </FooterTemplate>--%>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText=""  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div class="checkbox  input-sm no-margin">
                                                <asp:CheckBox ID="chkSelectAll" onclick="checkAll('chkPrint', this.checked);" Text=" "
                                                    runat="server"></asp:CheckBox>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate >
                                            <div class="checkbox  input-sm no-margin">
                                                <asp:CheckBox ID="chkPrint" Text=" " onclick="DeselectMainCheckbox(this);" runat="server" Enabled='<%# Eval("Type").ToString().Equals("PV") && Eval( "BankAccountObject.ChequeFormatCode").ToString() != "N.A" %>' />
                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">

                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                                CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i></asp:LinkButton>
                                        </ItemTemplate>

                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                                ValidationGroup="valGrpEditRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                                CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                        <%--
						                <FooterTemplate>
						                <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
								                CssClass="NewButt"><span>Add</span></asp:LinkButton>
						                </FooterTemplate>
                                        --%>
                                    </asp:TemplateColumn>
                                </Columns>
                                <HeaderStyle CssClass="tHeader" />
                                <FooterStyle CssClass="tFooter" />
                                <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                            </asp:DataGrid>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
                <div class="panel-footer clearfix">
                    <asp:LinkButton ID="LINKBUTTON1" OnClick="printReport" runat="server"
                        CssClass="btn btn-default pull-right" ToolTip="Please click to print selected cheques." CausesValidation="False">
                        <i class="ti-printer"></i> Print Selected Cheques
                    </asp:LinkButton>
                </div>
            </div>

        <div class="TABCOMMAND">
            <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>



    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnImport" PopupControlID="PNL" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
    <asp:Panel ID="PNL" runat="server" Style="display: none;" CssClass="popup-layer">
        <div class="modal-dialog">
	        <div class="modal-content">
	            <div class="modal-header">
	                <h4 class="modal-title">Import</h4>           
	            </div>
		        <div class="modal-body form-horizontal">
		                 <span style="color: Red">Note: Please select maximum of 5 days range to import.</span><br />
		            <div class="form-group">
                        <label class="col-sm-4 control-label text-left">
                             Transaction Date From            
                        </label>
                        <div class="col-sm-8">
                            <div class="input-group date">
                                <asp:TextBox runat="server" ID="tbxDateFrom" MaxLength="10" Columns="10" onfocus="select();" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-addon">
                                    <i class="ti-calendar"></i>
                                </span>
                            </div>            
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-4 control-label text-left">
                            Transaction Date To                   
                        </label>
                        <div class="col-sm-8">
                            <div class="input-group date">
                                <asp:TextBox runat="server" ID="tbxDateTo" MaxLength="10" Columns="10" onfocus="select();" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-addon">
                                    <i class="ti-calendar"></i>
                                </span>
                            </div>                 
                        </div>
                    </div>
                </div>
           
                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-default" ID="ButtonCancel" runat="server" Text="Cancel" />
                    <asp:Button CssClass="btn btn-primary" ID="ButtonOk" runat="server" Text="Import" OnCommand="ImportCommand" ValidationGroup="valGrpNewRow" OnClientClick="progress_update()" />
                </div>
            </div>
        </div>
               
                
                
                
                   
            
       


            <table align="center" style="margin-left: auto; margin-right: auto; margin-top: 0px; margin-bottom: 0px;">
                <tr align="center">
                    <td>
                        <div id="importing" style="visibility: hidden"><span style="font-style: italic;">Importing ...</span></div>
                    </td>
                </tr>
                <tr>
                    <td>
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
                    </td>
                </tr>
            </table>
    </asp:Panel>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".cash-flow-menu").addClass("active expand");
            $(".sub-bank-utilisation").addClass("active");
        });
    </script>
</asp:Content>
