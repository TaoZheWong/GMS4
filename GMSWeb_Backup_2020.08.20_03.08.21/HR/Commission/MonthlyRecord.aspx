<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true"
	Codebehind="MonthlyRecord.aspx.cs" Inherits="GMSWeb.HR.Commission.MonthlyRecord"
	Title="HR - Monthly Records Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
	<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
	<asp:ScriptManager ID="sriptmgr1" runat="server">
	</asp:ScriptManager>
	<a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">Incentive</a></li>
        <li class="active">Records</li>
    </ul>
    <h1 class="page-header">Records 
        <small>
            <br/>List of monthly incentive records by salesman. 
        </small>
    </h1>

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
                    <label class="control-label">
                        Year
                    </label>
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
                            <asp:ListItem Value="%%">All</asp:ListItem>
                            <asp:ListItem Value="1">Local</asp:ListItem>
                            <asp:ListItem Value="2">Export</asp:ListItem>
                        </asp:DropDownList>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">
                        Month
                    </label>
                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                        </asp:DropDownList>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">
                        Group
                    </label>
                        <asp:DropDownList ID="ddlGroupType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="All">All</asp:ListItem>
                            <asp:ListItem Value="N/A">N/A</asp:ListItem>
                            <asp:ListItem Value="W">W - Welding</asp:ListItem>
                            <asp:ListItem Value="S">S - Safety</asp:ListItem>
                        </asp:DropDownList>
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
                <asp:Label ID="lblSearchSummary" Visible="false" runat="server"></asp:Label>
            </h4>
        </div>
        <div class="panel-content">
            <div class="table-responsive">
		        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
						DataKeyField="SalesPersonMasterUserID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
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
							<asp:TemplateColumn HeaderText="Sales Team Name">
								<ItemTemplate>
									<asp:Label ID="lblSalesPersonMasterName" runat="server">
										<asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
											CausesValidation="false">
											<%--<span><%# Eval("SalesPersonMasterObject.SalesPersonMasterName")%>
											</span>--%>
											<span><%# Eval("SalesPersonMasterTeamName")%>
											</span>
											<%--<span><%# (Eval("SalesPersonMasterObject.SalesPersonMasterName") == Eval("SalesPersonMasterUserObject.SalesPersonMasterName")) 
														  ? Eval("SalesPersonMasterObject.SalesPersonMasterName") 
														  : String.Format("{0} - <b>{1}<b/>", Eval("SalesPersonMasterObject.SalesPersonMasterName"), Eval("SalesPersonMasterUserObject.SalesPersonMasterName"))%>
											</span>--%></asp:LinkButton>
										<input type="hidden" id="hidSalesPersonMasterID" runat="server" value='<%# Eval("SalesPersonMasterID")%>' />
										<input type="hidden" id="hidSalesPersonMasterUserID" runat="server" value='<%# Eval("SalesPersonMasterUserID")%>' />
										<input type="hidden" id="hidYear" runat="server" value='<%# Eval("tbYear")%>' />
										<input type="hidden" id="hidMonth" runat="server" value='<%# Eval("tbMonth")%>' />
									</asp:Label>
								</ItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="form-control input-sm" ID="ddlNewSalesPersonMasterName" runat="Server"
										DataTextField="SalesPersonMasterName" DataValueField="SalesPersonMasterID" OnChange="ChangeSalesman(this)" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Salesman Name">
								<ItemTemplate>
									<asp:Label ID="lblSalesPersonMasterUserName" runat="server">
										<asp:LinkButton ID="lnkEdit2" runat="server" CommandName="Edit" EnableViewState="false"
											CausesValidation="false">
											<%--<span><%# Eval("SalesPersonMasterUserObject.SalesPersonMasterName")%>
											</span>--%>
											<span><%# Eval("SalesPersonMasterUserName")%>
											</span>
										</asp:LinkButton>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Group"  HeaderStyle-Wrap="false">
								<ItemTemplate>
									<asp:Label ID="lblGroupType" runat="server">
												   <%# Eval( "GroupType" )%>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Year"  HeaderStyle-Wrap="false">
								<ItemTemplate>
									<asp:Label ID="lblYear" runat="server">
												   <%# Eval( "tbYear" )%>
									</asp:Label>
								</ItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="form-control input-sm" ID="ddlNewYear" runat="server" DataTextField="Year"
										DataValueField="Year" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Month"  HeaderStyle-Wrap="false">
								<ItemTemplate>
									<asp:Label ID="lblMonth" runat="server">
												   <%# Eval( "tbMonth" )%>
									</asp:Label>
								</ItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="form-control input-sm" ID="ddlNewMonth" runat="server">
										<asp:ListItem>1</asp:ListItem>
										<asp:ListItem>2</asp:ListItem>
										<asp:ListItem>3</asp:ListItem>
										<asp:ListItem>4</asp:ListItem>
										<asp:ListItem>5</asp:ListItem>
										<asp:ListItem>6</asp:ListItem>
										<asp:ListItem>7</asp:ListItem>
										<asp:ListItem>8</asp:ListItem>
										<asp:ListItem>9</asp:ListItem>
										<asp:ListItem>10</asp:ListItem>
										<asp:ListItem>11</asp:ListItem>
										<asp:ListItem>12</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Comm Rate"  HeaderStyle-Wrap="false">
								<ItemTemplate>
									<asp:Label ID="lblCommRate" runat="server">
												   <%# Eval( "CommissionRate" )%>
									</asp:Label>
								</ItemTemplate>
								<FooterTemplate>
									<asp:Label ID="lblCommRate2" runat="server">
									</asp:Label>
									<div style="display: none">
										<asp:DropDownList ID="ddlNewCommRate" runat="Server" DataTextField="CommissionRate"
											DataValueField="SalesPersonMasterID" /></div>
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="GPQ"  HeaderStyle-Wrap="false">
								<ItemTemplate>
									<asp:Label ID="lblGPQ" runat="server">
												   <%# Eval( "GPQ" )%>
									</asp:Label>
								</ItemTemplate>
								<FooterTemplate>
									<asp:Label ID="lblGPQ2" runat="server">
									</asp:Label>
									<div style="display: none">
										<asp:DropDownList ID="ddlNewGPQ" runat="Server" DataTextField="GPQ" DataValueField="SalesPersonMasterID" /></div>
								</FooterTemplate>
							</asp:TemplateColumn>
						   <asp:TemplateColumn HeaderText="Excess"  HeaderStyle-Wrap="false">
								<ItemTemplate>
									<asp:Label ID="lblExcess" runat="server">
												   <%# Eval( "Excess" )%>
									</asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtEditExcess" runat="server" Columns="10" MaxLength="15"
										Text='<%# Eval("Excess") %>' /><asp:RequiredFieldValidator ID="rfvEditExcess"
											runat="server" ControlToValidate="txtEditExcess" ErrorMessage="*" Display="dynamic"
											ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEditExcess" runat="server"
												ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtEditExcess"
												Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtNewExcess" runat="server" Columns="10" MaxLength="15" /><asp:RequiredFieldValidator
										ID="rfvNewExcess" runat="server" ControlToValidate="txtNewExcess" ErrorMessage="*"
										Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewExcess"
											runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtNewExcess"
											Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
								</FooterTemplate>
							</asp:TemplateColumn> 
							<asp:TemplateColumn HeaderText="90 Code"  HeaderStyle-Wrap="false">
								<ItemTemplate>
									<asp:Label ID="lbl90Code" runat="server">
												   <%# Eval( "CostOf90Code" )%>
									</asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtEdit90Code" runat="server" Columns="10" MaxLength="15"
										Text='<%# Eval("CostOf90Code") %>' /><asp:RequiredFieldValidator ID="rfvEdit90Code"
											runat="server" ControlToValidate="txtEdit90Code" ErrorMessage="*" Display="dynamic"
											ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEdit90Code" runat="server"
												ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtEdit90Code"
												Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtNew90Code" runat="server" Columns="10" MaxLength="15" /><asp:RequiredFieldValidator
										ID="rfvNew90Code" runat="server" ControlToValidate="txtNew90Code" ErrorMessage="*"
										Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNew90Code"
											runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtNew90Code"
											Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="C.O.G"  HeaderStyle-Wrap="false">
								<ItemTemplate>
									<asp:Label ID="lblCOG" runat="server">
												   <%# Eval( "CostOfCOG" )%>
									</asp:Label>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtEditCOG" runat="server" Columns="10" MaxLength="15"
										Text='<%# Eval("CostOfCOG") %>' /><asp:RequiredFieldValidator ID="rfvEditCOG" runat="server"
											ControlToValidate="txtEditCOG" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator
												ID="cvEditCOG" runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtEditCOG"
												Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtNewCOG" runat="server" Columns="10" MaxLength="15" /><asp:RequiredFieldValidator
										ID="rfvNewCOG" runat="server" ControlToValidate="txtNewCOG" ErrorMessage="*"
										Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewCOG"
											runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtNewCOG"
											Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Sales Adj"  HeaderStyle-Wrap="false">
								<ItemTemplate>
									<%# Eval( "SalesAdjustment" )%>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtEditSalesAdjustment" runat="server" Columns="10"
										MaxLength="15" Text='<%# Eval("SalesAdjustment") %>' /><asp:RequiredFieldValidator
											ID="rfvEditSalesAdjustment" runat="server" ControlToValidate="txtEditSalesAdjustment"
											ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator
												ID="cvEditSalesAdjustment" runat="server" ErrorMessage="Not a number" Display="Dynamic"
												ControlToValidate="txtEditSalesAdjustment" Type="Double" Operator="DataTypeCheck"
												ValidationGroup="valGrpEditRow" />
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtNewSalesAdjustment" runat="server" Columns="10"
										MaxLength="15" /><asp:RequiredFieldValidator ID="rfvNewSalesAdjustment" runat="server"
											ControlToValidate="txtNewSalesAdjustment" ErrorMessage="*" Display="dynamic"
											ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewSalesAdjustment"
												runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtNewSalesAdjustment"
												Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="GP Adj"  HeaderStyle-Wrap="false">
								<ItemTemplate>
									<%# Eval( "GPAdjustment" )%>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtEditGPAdjustment" runat="server" Columns="10"
										MaxLength="15" Text='<%# Eval("GPAdjustment") %>' /><asp:RequiredFieldValidator ID="rfvEditGPAdjustment"
											runat="server" ControlToValidate="txtEditGPAdjustment" ErrorMessage="*" Display="dynamic"
											ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEditGPAdjustment" runat="server"
												ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtEditGPAdjustment"
												Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtNewGPAdjustment" runat="server" Columns="10"
										MaxLength="15" /><asp:RequiredFieldValidator ID="rfvNewGPAdjustment" runat="server"
											ControlToValidate="txtNewGPAdjustment" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator
												ID="cvNewGPAdjustment" runat="server" ErrorMessage="Not a number" Display="Dynamic"
												ControlToValidate="txtNewGPAdjustment" Type="Double" Operator="DataTypeCheck"
												ValidationGroup="valGrpNewRow" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Comm W/H"  HeaderStyle-Wrap="false">
								<ItemTemplate>
									<%# Eval( "CommWithheld" )%>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtEditCommWithheld" runat="server" Columns="10"
										MaxLength="15" Text='<%# Eval("CommWithheld") %>' /><asp:RequiredFieldValidator ID="rfvEditCommWithheld"
											runat="server" ControlToValidate="txtEditCommWithheld" ErrorMessage="*" Display="dynamic"
											ValidationGroup="valGrpEditRow" /><asp:CompareValidator ID="cvEditCommWithheld" runat="server"
												ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtEditCommWithheld"
												Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpEditRow" />
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtNewCommWithheld" runat="server" Columns="10"
										MaxLength="15" /><asp:RequiredFieldValidator ID="rfvNewCommWithheld" runat="server"
											ControlToValidate="txtNewCommWithheld" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" /><asp:CompareValidator
												ID="cvNewCommWithheld" runat="server" ErrorMessage="Not a number" Display="Dynamic"
												ControlToValidate="txtNewCommWithheld" Type="Double" Operator="DataTypeCheck"
												ValidationGroup="valGrpNewRow" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Bad Debt Written Off" >
								<ItemTemplate>
									<%# Eval( "BadDebtWrittenOff" )%>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtEditBadDebtWrittenOff" runat="server" Columns="10"
										MaxLength="15" Text='<%# Eval("BadDebtWrittenOff") %>' /><asp:RequiredFieldValidator
											ID="rfvEditBadDebtWrittenOff" runat="server" ControlToValidate="txtEditBadDebtWrittenOff"
											ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator
												ID="cvEditBadDebtWrittenOff" runat="server" ErrorMessage="Not a number" Display="Dynamic"
												ControlToValidate="txtEditBadDebtWrittenOff" Type="Double" Operator="DataTypeCheck"
												ValidationGroup="valGrpEditRow" />
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtNewBadDebtWrittenOff" runat="server" Columns="10"
										MaxLength="15" /><asp:RequiredFieldValidator ID="rfvNewBadDebtWrittenOff" runat="server"
											ControlToValidate="txtNewBadDebtWrittenOff" ErrorMessage="*" Display="dynamic"
											ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewBadDebtWrittenOff"
												runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtNewBadDebtWrittenOff"
												Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Bad Debt Recovery" 
								HeaderStyle-Wrap="false" >
								<ItemTemplate>
									<%# Eval("BadDebtRecovery")%>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtEditBadDebtRecovery" runat="server" Columns="10"
										MaxLength="15" Text='<%# Eval("BadDebtRecovery") %>' /><asp:RequiredFieldValidator
											ID="rfvEditBadDebtRecovery" runat="server" ControlToValidate="txtEditBadDebtRecovery"
											ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" /><asp:CompareValidator
												ID="cvEditBadDebtRecovery" runat="server" ErrorMessage="Not a number" Display="Dynamic"
												ControlToValidate="txtEditBadDebtRecovery" Type="Double" Operator="DataTypeCheck"
												ValidationGroup="valGrpEditRow" />
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox CssClass="form-control input-sm" ID="txtNewBadDebtRecovery" runat="server" Columns="10"
										MaxLength="15" /><asp:RequiredFieldValidator ID="rfvNewBadDebtRecovery" runat="server"
											ControlToValidate="txtNewBadDebtRecovery" ErrorMessage="*" Display="dynamic"
											ValidationGroup="valGrpNewRow" /><asp:CompareValidator ID="cvNewBadDebtRecovery"
												runat="server" ErrorMessage="Not a number" Display="Dynamic" ControlToValidate="txtNewBadDebtRecovery"
												Type="Double" Operator="DataTypeCheck" ValidationGroup="valGrpNewRow" />
								</FooterTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Entertainment Exp" 
								HeaderStyle-Wrap="false" >
								<ItemTemplate>
									<asp:Label ID="lblEntertainmentExpense" runat="server">
												   <%# Eval( "TotalEntertainmentExpenses" )%>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn 
								ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
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
						<FooterStyle CssClass="tFooter" />
						<PagerStyle  Mode="NumericPages" CssClass="grid_pagination"/>
					</asp:DataGrid>
			</div>
        </div>
        <div class="panel-footer clearfix">
            <asp:LinkButton ID="LINKBUTTON1" OnClick="GenerateReport" runat="server" Text="Print"
				CssClass="btn btn-default pull-right  m-l-10" ToolTip="Please click to print report." CausesValidation="False"><i class="ti-printer"></i></asp:LinkButton>
            <asp:DropDownList ID="ddlReport" runat="server" CssClass="form-control no-full-width pull-right ">
						<asp:ListItem Value="SalesCommissionsEntitlement">Commission Entitlement Report</asp:ListItem>
				<asp:ListItem Value="SalesCommissionsEntitlementBySalesman">Commission Entitlement Report By Salesman</asp:ListItem>
				<asp:ListItem Value="SalesCommissionsEntitlement_WS">Commission Entitlement Report By Team</asp:ListItem>
				<asp:ListItem Value="SalesCommissionsEntitlement_WS_SalesGP">Sales & GP Report By Team</asp:ListItem>
				<%--<asp:ListItem Value="SalesCommissions_New">Commission Payable & Forfeited  Report</asp:ListItem>--%>
				<asp:ListItem Value="SalesCommissions_2016">Commission Payable & Forfeited Report</asp:ListItem>
				<asp:ListItem Value="SalesCommissions_2016_HR">Commission Payable & Forfeited Report[HR]</asp:ListItem>
				<%-- <asp:ListItem Value="SalesSpecialCommissions">Sales Special Continuous Commissions</asp:ListItem>--%>
				<asp:ListItem Value="C06 - Yearly GP For Salesman">Yearly GP For Salesman</asp:ListItem>
				<asp:ListItem Value="C07 - Monthly GP For Salesman">Monthly GP For Salesman</asp:ListItem>
                <asp:ListItem Value="C08 - CommissionEntitlementReportforSGC">Commission Entitlement Report for SGC</asp:ListItem>
			</asp:DropDownList>
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

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".incentive-menu").addClass("active expand");
            $(".sub-monthly").addClass("active");
        });
    </script>
</asp:Content>