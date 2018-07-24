<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="BankInfo.aspx.cs" Inherits="GMSWeb.SysFinance.BankFacilities.BankInfo" Title="Finance - Bank Info Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
		<li><a href="#">Bank Facilities</a></li>
		<li class="active"> Info</li>
	</ul>
    <h1 class="page-header">
		Bank Info <small>List of current bank information.</small>
	</h1>
<atlas:ScriptManager ID="scriptMgr" runat="server" />
	<atlas:UpdatePanel ID="udpBankUpdater" runat="server" Mode="conditional" >
		<ContentTemplate>
		    <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-align-justify"></i>
                        <asp:label id="lblSearchSummary" Visible="false" Runat="server"></asp:label>
                    </h4>
                </div>
                <div class="table-responsive">
                    <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
				        DataKeyField="BankID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
				        OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" 
				        GridLines="none" OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
				        CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" AllowPaging="true" PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
				        <Columns>
					        <asp:TemplateColumn HeaderText="No">
						        <ItemTemplate>
							        <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
						        </ItemTemplate>
					        </asp:TemplateColumn>
					        <asp:TemplateColumn HeaderText="Bank Code">
						        <ItemTemplate>
							        <asp:Label ID="lblBankCode" runat="server">
							        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
								        CausesValidation="false"><span><%# Eval( "BankCode" )%></span></asp:LinkButton>
								        <input type="hidden" id="hidBankCode" runat="server" value='<%# Eval("BankCode")%>' />
							        </asp:Label>
						        </ItemTemplate>
						        <FooterTemplate>
							        <asp:TextBox ID="txtNewBankCode" CssClass="form-control input-sm" runat="server" Columns="5" MaxLength="5" />
							        <asp:RequiredFieldValidator ID="rfvNewBankCode" runat="server" ControlToValidate="txtNewBankCode"
								        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
						        </FooterTemplate>
					        </asp:TemplateColumn>
					        <asp:TemplateColumn HeaderText="Bank Name" >
						        <ItemTemplate>
							        <asp:Label ID="lblBankName" runat="server">
							        <%# Eval( "BankName" )%>
							        </asp:Label>
						        </ItemTemplate>
						        <EditItemTemplate>
							        <asp:TextBox ID="txtEditBankName"  CssClass="form-control input-sm" runat="server" Columns="50" MaxLength="50" Text='<%# Eval("BankName") %>' />
							        <asp:RequiredFieldValidator ID="rfvEditBankName" runat="server" ControlToValidate="txtEditBankName"
								        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
						        </EditItemTemplate>
						        <FooterTemplate>
							        <asp:TextBox ID="txtNewBankName" CssClass="form-control input-sm" runat="server" Columns="50" MaxLength="50" />
							        <asp:RequiredFieldValidator ID="rfvNewBankName" runat="server" ControlToValidate="txtNewBankName"
								        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
						        </FooterTemplate>
					        </asp:TemplateColumn>
					        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" 
						        ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
						        <ItemTemplate>
							        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
							            CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete">
                                        <i class="ti-trash"></i> 
							        </asp:LinkButton>
						        </ItemTemplate>
						        <EditItemTemplate>
							        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
								        ValidationGroup="valGrpEditRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
							        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
								        CausesValidation="false"  CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
						        </EditItemTemplate>
						        <FooterTemplate>
						            <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
							            ValidationGroup="valGrpNewRow" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
					            </FooterTemplate>
					        </asp:TemplateColumn>
				        </Columns>
				        <HeaderStyle Font-Bold="true" CssClass="tHeader"/>
				        <FooterStyle/>
				        <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
			        </asp:DataGrid>
                </div>
            </div>
			    
		</ContentTemplate>
	</atlas:UpdatePanel>

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
            $(".administration-menu").addClass("active expand");
            $(".sub-bank-info").addClass("active");
        });
    </script>
</asp:Content>