<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="EntertainmentExpense.aspx.cs" Inherits="GMSWeb.HR.Commission.EntertainmentExpense" Title="HR - Entertainment Expenses Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<asp:ScriptManager ID="sriptmgr1" runat="server"></asp:ScriptManager>
<a name="TemplateInfo"></a>
<ul class="breadcrumb pull-right">
    <li><a href="#"><asp:Label ID="lblPageHeader" runat="server" /></a></li>
    <li class="active">Entertainment</li>
</ul>
<h1 class="page-header">Entertainment <small>List of entertainment expenses by salesman.</small></h1>

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
                    <label class="control-label">Payee</label>
                       <asp:TextBox ID="txtSearchName" runat="server" Columns="20" MaxLength="50" onfocus="select();"
                            CssClass="form-control" />
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Local/Export</label>
                       <asp:DropDownList ID="ddlSearchArea" runat="server" CssClass="form-control">
                            <asp:ListItem Value="%%">All</asp:ListItem>
                            <asp:ListItem Value="1">Local</asp:ListItem>
                            <asp:ListItem Value="2">Export</asp:ListItem>
                        </asp:DropDownList>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Payment Date From</label>
                        <div class="input-group date">
                            <asp:TextBox runat="server" ID="txtSearchDateFrom" MaxLength="10" Columns="10" onfocus="select();"
                                CssClass="form-control datepicker"></asp:TextBox>
                            <span class="input-group-addon"><i class="ti-calendar"></i></span>
					    </div>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Payment Date To</label>
                        <div class="input-group date">
                            <asp:TextBox runat="server" ID="txtSearchDateTo" MaxLength="10" Columns="10" onfocus="select();"
                            CssClass="form-control datepicker"></asp:TextBox>
                             <span class="input-group-addon"><i class="ti-calendar"></i></span>
					    </div>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">Others</label>
                        <div class="checkbox">
                            <asp:CheckBox ID="chkSearchOthers" runat="server" />
                            <label for="<%= chkSearchOthers.ClientID %>"></label>
                        </div>
                </div>
            </div>
        </div>
        <div class="panel-footer clearfix">
            <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
                OnClick="btnSearch_Click"></asp:Button>
        </div>
    </div>
        
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
                <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    DataKeyField="TransactionID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
                    OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" GridLines="none"
                    OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
                    CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                <input type="hidden" id="hidTransactionID" runat="server" value='<%# Eval("TransactionID")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Expense Type" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("EntertainmentExpenseTypeObject.EntertainmentExpenseTypeName")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList CssClass="form-control input-sm" ID="ddlEditExpenseType" runat="Server" DataTextField="EntertainmentExpenseTypeName"
                                    DataValueField="EntertainmentExpenseTypeID" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList CssClass="form-control input-sm" ID="ddlNewExpenseType" runat="Server" DataTextField="EntertainmentExpenseTypeName"
                                    DataValueField="EntertainmentExpenseTypeID" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Others">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                    CausesValidation="false"><span><%# ( (bool)Eval( "Others" ) ) ? "Yes" : "No"%></span></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="checkbox custom-for-click input-sm no-margin">
                                    <asp:CheckBox ID="chkEditOthers" runat="server" Checked='<%# Eval("Others") %>' onclick="SelectOthers(this);" />
                                    <label></label>
                                </div>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <div class="checkbox custom-for-click input-sm no-margin">
                                    <asp:CheckBox ID="chkNewOthers" runat="server" onclick="SelectOthers(this);" />
                                    <label></label>
                                </div>
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
                                    <asp:DropDownList CssClass="form-control input-sm" ID="ddlEditSalesPersonMasterName" runat="Server" DataTextField="SalesPersonMasterName"
                                        DataValueField="SalesPersonMasterID" />
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spanSalesPersonMasterName" runat="server">
                                    <asp:DropDownList CssClass="form-control input-sm" ID="ddlNewSalesPersonMasterName" runat="Server" DataTextField="SalesPersonMasterName"
                                        DataValueField="SalesPersonMasterID" /></span>
                            </FooterTemplate>
                            <ItemStyle />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Payment Date">
                            <ItemTemplate>
                                <asp:Label ID="lblTrnDate" runat="server">
									<%# Eval("Date", "{0: dd-MMM-yyyy}")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="input-group date">
                                    <asp:TextBox CssClass="form-control input-sm datepicker" runat="server" ID="editPaymentDate" MaxLength="10" Columns="10" Text='<%# Eval("Date", "{0: dd/MM/yyyy}") %>'></asp:TextBox>
                                     <span class="input-group-addon"><i class="ti-calendar"></i></span>
					            </div>
                                <asp:RequiredFieldValidator ID="rfvEditPaymentDate" runat="server" ControlToValidate="editPaymentDate"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator
                                        ID="cvEditPaymentDate" runat="server" ErrorMessage="Invalid Date" ControlToValidate="editPaymentDate"
                                        Type="Date" Display="Dynamic" ValidationGroup="valGrpEditRow" Operator="DataTypeCheck"></asp:CompareValidator>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <div class="input-group date">
                                     <asp:TextBox CssClass="form-control datepicker input-sm" runat="server" ID="newPaymentDate" MaxLength="10" Columns="10"></asp:TextBox>
                                     <span class="input-group-addon"><i class="ti-calendar"></i></span>
					            </div>
                               
                                <asp:RequiredFieldValidator ID="rfvNewPaymentDate" runat="server" ControlToValidate="newPaymentDate"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator
                                        ID="cvNewPaymentDate" runat="server" ErrorMessage="Invalid Date" ControlToValidate="newPaymentDate"
                                        Type="Date" Display="Dynamic" ValidationGroup="valGrpNewRow" Operator="DataTypeCheck"></asp:CompareValidator>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                            <FooterStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Area" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server">
										            <%# ((bool)Eval("Others"))?"":((Eval("Area").ToString() == "1") ? "Local" : "Export")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spanArea" runat="server" style='<%# ((bool)Eval("others"))?"visibility:hidden": "visibility:visible" %>'>
                                    <asp:DropDownList CssClass="form-control input-sm" ID="ddlEditArea" runat="server">
                                        <asp:ListItem Value="1">Local</asp:ListItem>
                                        <asp:ListItem Value="2">Export</asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spanArea" runat="server">
                                    <asp:DropDownList CssClass="form-control input-sm" ID="ddlNewArea" runat="server">
                                        <asp:ListItem Value="1">Local</asp:ListItem>
                                        <asp:ListItem Value="2">Export</asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server">
												       <%# Eval("Amount","{0:f2}")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditAmount" runat="server" Columns="15" MaxLength="15" Text='<%# Eval("Amount", "{0:f2}") %>' />
                                <asp:RequiredFieldValidator ID="rfvEditAmount" runat="server" ControlToValidate="txtEditAmount"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:RangeValidator
                                        ID="rgEditAmount" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtEditAmount"
                                        Type="Double" ValidationGroup="valGrpEditRow" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewAmount" runat="server" Columns="15" MaxLength="15" />
                                <asp:RequiredFieldValidator ID="rfvNewAmount" runat="server" ControlToValidate="txtNewAmount"
                                    ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:RangeValidator
                                        ID="rgNewAmount" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtNewAmount"
                                        Type="Double" ValidationGroup="valGrpNewRow" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Remark">
                            <ItemTemplate>
                                <asp:Label ID="lblRemark" runat="server">
												       <%# Eval("Remark")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditRemark" runat="server" Columns="15" MaxLength="50" Text='<%# Eval("Remark") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewRemark" runat="server" Columns="15" MaxLength="50" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
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
            </div>
        </div>
        <div class="panel-footer clearfix">
            <asp:LinkButton ID="LINKBUTTON1" OnClick="GenerateReport" runat="server" Text="Print"
                CssClass="btn btn-default pull-right m-l-10" ToolTip="Please click to print report." CausesValidation="False"><i class="ti-printer"></i></asp:LinkButton>
            <asp:DropDownList ID="ddlReport" runat="server" CssClass="form-control no-full-width pull-right">
                <asp:ListItem Value="EntertainmentExpensesReport">Entertainment Expenses Report</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
                
                           
<div class="TABCOMMAND">
    <asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".administration-menu").addClass("active expand");
            $(".incentive-menu").addClass("active expand");
            $(".sub-entertainment-expense").addClass("active");
        });
    </script>
</asp:Content>
