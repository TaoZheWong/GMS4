<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="CommissionNGPQ1.aspx.cs" Inherits="GMSWeb.HR.Commission.CommissionNGPQ" Title="HR - Commission and GPQ Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<asp:ScriptManager ID="sriptmgr1" runat="server"></asp:ScriptManager>

<ul class="breadcrumb pull-right">
    <li><a href="#">Incentive</a></li>
    <li class="active">Setup</li>
</ul>
<h1 class="page-header">Setup <br />
    <small>Master record of commission rate and GPQ.</small>
</h1>

<div class="note note-info">
    <h4 class="block"><i class="ti-info-alt"></i> Info! </h4>
    <p>To setup Joint Account member, please access 
	<asp:HyperLink id="hyperlink1" NavigateUrl="SharedSetup.aspx" Text="Team Setup" runat="server"/>.</p>
</div>

<div style="display: none">
    <asp:TextBox ID="txtBoxID" runat="server" Columns="100" MaxLength="100" Text="" />
    <asp:TextBox ID="txtHiddenID" runat="server" Columns="100" MaxLength="100" Text="" />
    <asp:TextBox ID="txtEditBoxID" runat="server" Columns="100" MaxLength="100" Text="" />
    <asp:TextBox ID="txtEditHiddenID" runat="server" Columns="100" MaxLength="100" Text="" />
</div>
			
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
			<asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
				DataKeyField="SalesPersonMasterID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
				OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" GridLines="none"
				OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
				CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
				PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
				<Columns>
					<asp:TemplateColumn HeaderText="No">
						<ItemTemplate>
							<%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
							<input type="hidden" id="hidSalesPersonMasterID" runat="server" value='<%# Eval("SalesPersonMasterID")%>' />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Salesman Name">
						<ItemTemplate>
							<asp:Label ID="lblSalesPersonMasterName" runat="server">
								<asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
									CausesValidation="false"><span><%# Eval( "SalesPersonMasterName" )%></span></asp:LinkButton>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox CssClass="form-control input-sm" ID="txtEditSalesPersonMasterName" runat="server" Columns="20" MaxLength="50" onchange="this.value = this.value.toUpperCase()" Text='<%# Eval( "SalesPersonMasterName" )%>' /><asp:RequiredFieldValidator
								ID="rfvEditSalesPersonMasterName" runat="server" ControlToValidate="txtEditSalesPersonMasterName"
								ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
						</EditItemTemplate>
						<FooterTemplate>
							<asp:TextBox CssClass="form-control input-sm" ID="txtNewSalesPersonMasterName" runat="server" Columns="20" MaxLength="50" onchange="this.value = this.value.toUpperCase()" /><asp:RequiredFieldValidator
								ID="rfvNewSalesPersonMasterName" runat="server" ControlToValidate="txtNewSalesPersonMasterName"
								ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Accounts List">
						<ItemTemplate>
							<asp:Label ID="lblAccountList" runat="server">
								<%# Eval( "AccountList" )%>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
						    <div class="input-group">
							    <asp:TextBox CssClass="form-control input-sm" ID="txtEditAccountList" runat="server" Columns="30" MaxLength="100" ReadOnly="true" Text='<%# Eval( "AccountList" )%>'/>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="lnkEditAccount" runat="server" OnClientClick="mode = 'edit'; selectAccounts(this);" CssClass="btn btn-primary btn-sm right-rounded-border">
                                        <i class="ti-user"></i>
                                    </asp:LinkButton>
                                </span>
							</div>
                            <input type="hidden" id="hidEditAccountList" runat="server" value='<%# Eval( "AccountList" )%>' />
							<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="lnkEditAccount"
								PopupControlID="PNL" OkControlID="ButtonOK" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
						</EditItemTemplate>
						<FooterTemplate>
						    <div class="input-group">
							    <asp:TextBox CssClass="form-control input-sm" ID="txtNewAccountList" runat="server" Columns="30" MaxLength="100" ReadOnly="true"/>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="lnkAddAccount" runat="server" OnClientClick="mode = 'add';selectAccounts(this);" CssClass="btn btn-primary btn-sm right-rounded-border">
                                        <i class="ti-user"></i>
                                    </asp:LinkButton>
                                </span>
                            </div>
							<input type="hidden" id="hidAccountList" runat="server" value="" />
							<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lnkAddAccount"
								PopupControlID="PNL" OkControlID="ButtonOK" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="GPQ" HeaderStyle-Wrap="false" >
						<ItemTemplate>
							<asp:Label ID="lblGPQ" runat="server">
									<%# Eval( "GPQ" )%>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox CssClass="form-control input-sm" ID="txtEditGPQ" runat="server" Columns="10" MaxLength="10" Text='<%# Eval("GPQ") %>' /><asp:RequiredFieldValidator
								ID="rfvEditGPQ" runat="server" ControlToValidate="txtEditGPQ" ErrorMessage="*"
								Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEditGPQ"
									runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtEditGPQ"
									Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
						</EditItemTemplate>
						<FooterTemplate>
							<asp:TextBox CssClass="form-control input-sm" ID="txtNewGPQ" runat="server" Columns="10" MaxLength="10" /><asp:RequiredFieldValidator
								ID="rfvNewGPQ" runat="server" ControlToValidate="txtNewGPQ" ErrorMessage="*"
								Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewGPQ"
									runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtNewGPQ"
									Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Comm Rate" HeaderStyle-Wrap="false" >
						<ItemTemplate>
							<asp:Label ID="lblCommRate" runat="server">
									<%# Eval( "CommissionRate" )%>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox CssClass="form-control input-sm" ID="txtEditCommissionRate" runat="server" Columns="10" MaxLength="10"
								Text='<%# Eval("CommissionRate") %>' /><asp:RequiredFieldValidator ID="rfvEditCommissionRate"
									runat="server" ControlToValidate="txtEditCommissionRate" ErrorMessage="*" Display="dynamic"
									ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEditCommissionRate"
										runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtEditCommissionRate"
										Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
						</EditItemTemplate>
						<FooterTemplate>
							<asp:TextBox CssClass="form-control input-sm" ID="txtNewCommissionRate" runat="server" Columns="10" MaxLength="10" /><asp:RequiredFieldValidator
								ID="rfvNewCommissionRate" runat="server" ControlToValidate="txtNewCommissionRate"
								ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator
									ID="cvNewCommissionRate" runat="server" ErrorMessage="Not a number" Display="Dynamic"
									ControlToValidate="txtNewCommissionRate" Type="Double" Operator="DataTypeCheck"
									ValidationGroup="valGrpNewRow" />
						</FooterTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Function">
						<ItemTemplate>
							<asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
								CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Delete">
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
				<AlternatingItemStyle CssClass="tAltRow" />
				<FooterStyle CssClass="tFooter" />
				<PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
			</asp:DataGrid>
    </div>
</div>


<br />
<asp:Panel ID="PNL" runat="server" CssClass="popup-layer" Style="display: none;">
    <div class="modal-dialog">
	    <div class="modal-content">
	        <div class="modal-header">
	            <h4 class="modal-title">Select Accounts</h4>
	        </div>
		    <div class="modal-body">
		        <p>(Press CTRL to select multiple accounts) </p>
		        <asp:ListBox ID="lsbAccounts" runat="server" SelectionMode="Multiple" Width="100%" Rows="30"></asp:ListBox>
		    </div>	
		    <div class="modal-footer">
		        <asp:Button ID="ButtonOk" runat="server" Text="Save" OnClientClick="FillData()" CssClass="btn btn-primary"/>
		        <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" CssClass="btn btn-default" />
		    </div>
        </div>
    </div>
</asp:Panel>

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
            $(".incentive-menu").addClass("active expand");
            $(".sub-commission").addClass("active");
        });
    </script>
</asp:Content>