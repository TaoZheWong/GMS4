<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="TeamSetup.aspx.cs" Inherits="GMSWeb.Sales.Sales.TeamSetup" %>
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
    <li class="active">Team Setup</li>
</ul>
<h1 class="page-header">Team Setup <small>Setup of sales group and team.</small></h1>
   <%--Search--%>
    <div class="panel panel-primary" >
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-search"></i>
                Search filter
            </h4>
        </div>
        <div class="panel-body row">
            <div class="form-horizontal m-t-20">
                <div class="form-horizontal m-t-20">
               <div class="form-group col-lg-4 col-md-6 col-sm-6">
                    <label class="control-label">Team</label>
                        <asp:TextBox runat="server" ID="txtTeam" MaxLength="50" Columns="20" onfocus="select();" CssClass="form-control" placeholder="e.g. GAS"></asp:TextBox>
                </div>
                <div class="form-group col-lg-4 col-md-6 col-sm-6">
                    <label class="control-label">Sales Person Name</label>
                        <asp:TextBox runat="server" ID="txtSalesPersonName" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="form-control" placeholder="e.g. Raymond"></asp:TextBox>
                </div>
                <div class="form-group col-lg-4 col-md-6 col-sm-6">
                    <label class="control-label">Sales Person Short Name</label>
                        <asp:TextBox runat="server" ID="txtShortName" MaxLength="50" Columns="20" onfocus="select();"
                                    CssClass="form-control" ></asp:TextBox>
                </div>
               
            </div>
        </div>
       </div>
        </div>
    <div class="panel-footer clearfix">
        <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="pull-right btn btn-primary m-l-5" OnClick="btnSearch_Click"></asp:Button> 
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="MsgPanel2" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
          
   <%--Group Setup --%>   
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
                    DataKeyField="TeamID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
                    OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" GridLines="none"
                    OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
                    CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                <input type="hidden" id="hidTeamID" runat="server" value='<%# Eval("TeamID")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Group" HeaderStyle-Wrap="false">
                            <ItemTemplate>                               
                                <%# Eval( "SalesGroupObject.GroupName")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList CssClass="form-control input-sm" ID="ddlEditGroupName" runat="Server" DataTextField="GroupName"
                                    DataValueField="GroupID" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList CssClass="form-control input-sm" ID="ddlNewGroupName" runat="Server" DataTextField="GroupName"
                                    DataValueField="GroupID" />
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Team Name">
                            <ItemTemplate>
                                <asp:Label ID="lblTeamName" runat="server">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true"
                                            CausesValidation="false"><span><%# Eval("TeamName")%></span></asp:LinkButton>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditTeamName" runat="server" Columns="15" MaxLength="15" Text='<%# Eval("TeamName") %>' />
                                 <asp:RequiredFieldValidator
									ID="rfvEditTeamName" runat="server" ControlToValidate="txtEditTeamName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditTeamName" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewTeamName" runat="server" Columns="15" MaxLength="15" />
                                <asp:RequiredFieldValidator
									ID="rfvNewTeamName" runat="server" ControlToValidate="txtNewTeamName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewTeamName" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                       
                         <asp:TemplateColumn HeaderText="Team Short Name">
                            <ItemTemplate>
                                <asp:Label ID="lblRemark" runat="server">
												       <%# Eval("TeamShortName")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditTeamShortName" runat="server" Columns="15" MaxLength="50" Text='<%# Eval("TeamShortName") %>' />
                                  <asp:RequiredFieldValidator
									ID="rfvEditTeamShortName" runat="server" ControlToValidate="txtEditTeamShortName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditTeamName" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewTeamShortName" runat="server" Columns="15" MaxLength="50" />
                                 <asp:RequiredFieldValidator
									ID="rfvNewTeamShortName" runat="server" ControlToValidate="txtNewTeamShortName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewTeamName" />
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
                                    ValidationGroup="valGrpEditTeamName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                    ValidationGroup="valGrpNewTeamName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="tHeader" />
                    <FooterStyle CssClass="tFooter" />
                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                </asp:DataGrid>
            </div>
        </div>
         <div class="TABCOMMAND">
    <asp:UpdatePanel ID="udpMsgUpdater1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="MsgPanel1" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div> 
    </div>

    

     <%--Team Setup --%>  
     <div class="panel panel-primary" id="resultList" runat="server" visible="true">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-align-justify"></i>
                <asp:Label ID="Label1" Visible="false" runat="server" />
            </h4>
        </div>
        <div class="panel-body no-padding">
            <div class="table-responsive">
                <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    DataKeyField="SalesPersonID" OnCancelCommand="DataGrid1_CancelCommand" OnEditCommand="DataGrid1_EditCommand"
                    OnUpdateCommand="DataGrid1_UpdateCommand" OnItemCommand="DataGrid1_CreateCommand" GridLines="none"
                    OnItemDataBound="DataGrid1_ItemDataBound" OnDeleteCommand="DataGrid1_DeleteCommand"
                    CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    PageSize="20" OnPageIndexChanged="DataGrid1_PageIndexChanged" EnableViewState="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                <input type="hidden" id="hidSalesPersonID" runat="server" value='<%# Eval("SalesPersonID")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Team" HeaderStyle-Wrap="false">
                            <ItemTemplate>                             
                                <%# Eval( "TeamName")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <!--<asp:DropDownList CssClass="form-control input-sm" ID="ddlEditTeamName" runat="Server" DataTextField="TeamName"
                                    DataValueField="TeamID" />-->
                                 <%# Eval( "TeamName")%>
                            </EditItemTemplate>
                            <FooterTemplate>                              
                                <asp:DropDownList CssClass="form-control input-sm" ID="ddlNewTeamName" runat="Server" DataTextField="TeamName"
                                    DataValueField="TeamID" AutoPostBack="true" OnSelectedIndexChanged="ddlNewTeamName_SelectedIndexChanged"/>            
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Sales Person Name" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                     <asp:Label ID="lblTeamName" runat="server">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true"
                                                CausesValidation="false"><span><%# Eval( "SalesPersonName")%></span></asp:LinkButton>                            
                                     </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%# Eval( "SalesPersonName")%>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList CssClass="form-control input-sm" ID="ddlNewSalesPersonMasterName" runat="Server"  DataTextField="SalesPersonName"
                                    DataValueField="SalesPersonID" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                       
                         <asp:TemplateColumn HeaderText="Sales Person Short Name">
                            <ItemTemplate>
                                <asp:Label ID="lblSalesPersonShortName" runat="server">
												       <%# Eval("SalesPersonShortName")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditSalesPersonShortName" runat="server" Columns="15" MaxLength="50" Text='<%# Eval("SalesPersonShortName")%>' />
                                 <asp:RequiredFieldValidator
									ID="rfvEditSalesPersonShortName" runat="server" ControlToValidate="txtEditSalesPersonShortName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditSalesPersonShortName" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewSalesPersonShortName" runat="server" Columns="15" MaxLength="50" />
                                   <asp:RequiredFieldValidator
									ID="rfvNewSalesPersonShortName" runat="server" ControlToValidate="txtNewSalesPersonShortName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewSalesPersonShortName" />
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
                                    ValidationGroup="valGrpEditSalesPersonShortName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                    ValidationGroup="valGrpNewSalesPersonShortName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <HeaderStyle CssClass="tHeader" />
                    <FooterStyle CssClass="tFooter" />
                    <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                </asp:DataGrid>
            </div>
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
            $(".sub-team-setup").addClass("active");
        });
    </script>
</asp:Content>
