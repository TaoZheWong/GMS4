<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="SharedSetup.aspx.cs" Inherits="GMSWeb.HR.Commission.SharedSetup" Title="HR - Joint Account Member Setup" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<asp:ScriptManager ID="sriptmgr1" runat="server"></asp:ScriptManager>

<ul class="breadcrumb pull-right">
    <li><a href="#">Incentive</a></li>
    <li><asp:HyperLink id="hyperlink1" NavigateUrl="CommissionNGPQ.aspx" Text="Setup" runat="server"/></li>
    <li class="active">Joint Account Member</li>
</ul>
<h1 class="page-header">Joint Account Member <br />
    <small>Master record of Joint Account Member.</small>
</h1>

<div style="display:none">
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
					DataKeyField="SalesPersonTeamID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
					OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" GridLines="none"
					OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
					CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
					PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
					<Columns>
						<asp:TemplateColumn HeaderText="No" ItemStyle-Width="15px">
							<ItemTemplate>
								<%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Sales Person Team">
								<ItemTemplate>
									<asp:Label ID="lblSalesPersonTeamID" runat="server">
									   <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
											CausesValidation="false">
											<span><%# (Eval("Team"))%></span></asp:LinkButton>
										<input type="hidden" id="hidSalesPersonTeamID" runat="server" value='<%# Eval("SalesPersonTeamID")%>' />
										<input type="hidden" id="hidSalesPersonMasterID" runat="server" value='<%# Eval("SalesPersonMasterID")%>' />
									</asp:Label>
								</ItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="form-control input-sm" ID="ddlSalesPersonTeamID" runat="Server"
										DataTextField="SalesPersonMasterName" DataValueField="SalesPersonMasterID"/>
								</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Team Member" >
								<ItemTemplate>
									<asp:Label ID="lblSalesPersonMasterID" runat="server">
									   <%# Eval( "Member" )%>
									</asp:Label>
								</ItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="form-control input-sm" ID="ddlSalesPersonUserID" runat="Server"
										DataTextField="SalesPersonMasterName" DataValueField="SalesPersonMasterID"/>
								</FooterTemplate>
						</asp:TemplateColumn>
						 <asp:TemplateColumn HeaderText="Group">
								<ItemTemplate>
									<asp:Label ID="lblGroup" runat="server">
									   <%# Eval( "GroupType" )%>
									</asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="ddleditGroupType" runat="server" CssClass="form-control input-sm" >
										<asp:ListItem Value="N/A">N/A</asp:ListItem>
										<asp:ListItem Value="W">W - Welding</asp:ListItem>
										<asp:ListItem Value="S">S - Safety</asp:ListItem>
									</asp:DropDownList>
									 <input type="hidden" id="hidGroupType" runat="server" value='<%# Eval("GroupType")%>' />
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ddlGroupType" runat="server" CssClass="form-control input-sm">
										<asp:ListItem Value="N/A">N/A</asp:ListItem>
										<asp:ListItem Value="W">W - Welding</asp:ListItem>
										<asp:ListItem Value="S">S - Safety</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Ratio" HeaderStyle-Wrap="false" >
							<ItemTemplate>
								<asp:Label ID="lblRatio" runat="server">
									   <%# Eval( "Ratio" )%>
								</asp:Label>
							</ItemTemplate>
							<EditItemTemplate>
								<asp:TextBox CssClass="form-control input-sm" ID="txtEditRatio" runat="server" Columns="10" MaxLength="10"
									Text='<%# Eval("Ratio") %>' /><asp:RequiredFieldValidator ID="rfvEditRatio"
										runat="server" ControlToValidate="txtEditRatio" ErrorMessage="*" Display="dynamic"
										ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEditRatio"
											runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtEditRatio"
											Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
							</EditItemTemplate>
							<FooterTemplate>
								<asp:TextBox CssClass="form-control input-sm" ID="txtNewRatio" runat="server" Columns="10" MaxLength="10" Text="1"/><asp:RequiredFieldValidator
									ID="rfvNewRatio" runat="server" ControlToValidate="txtNewRatio"
									ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator
										ID="cvNewRatio" runat="server" ErrorMessage="Not a number" Display="Dynamic"
										ControlToValidate="txtNewRatio" Type="Double" Operator="DataTypeCheck"
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
<div class="TABCOMMAND">
	<asp:UpdatePanel ID="udpMsgUpdater" runat="server" UpdateMode="Always">
		<ContentTemplate>
			<uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
		</ContentTemplate>
	</asp:UpdatePanel>
</div>
</asp:Content>
