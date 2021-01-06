<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="SalesPersonMapping.aspx.cs" Inherits="GMSWeb.Admin.Accounts.SalesPersonMapping1" Title="System Admin - Sales Person Mapping Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
		<li><a href="#">Accounts </a></li>
		<li class="active">Sales Person Mapping</li>
	</ul>
    <h1 class="page-header">
		Sales Mapping 
        <br />
        <small>Maintain a list of mappings from A21 accounts to GMS accounts .</small>
	</h1>
    
    <div class="panel panel-primary">
        <div class="panel-heading">
            <h4 class="panel-title"><i class="ti-link"></i> Mapping</h4>
        </div>
        <div class="panel-body">
            <div class="form-horizontal row m-t-20">
                <div class="form-group col-lg-12 col-sm-12">
                    <label class="col-sm-6 control-label text-left">Username</label>
                    <div class="col-sm-6">
                        <asp:DropDownList ID="ddlUsername" runat="Server" DataTextField="UserName" DataValueField="Id"
                            CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUsername_SelectedIndexChanged" />
                    </div>
                </div>
                <div class="form-group col-lg-12 col-sm-12"  ID="groupRow" runat="server" style="display:none">
                    <label class="col-sm-6 control-label text-left">Group Management User</label>
                    <div class="col-sm-6 radio-inline m-b-3">
                        <asp:RadioButton ID="rbIsGroupManagementUser" runat="server" Text="Yes" GroupName="GroupManagementUser"
                        OnCheckedChanged="rbIsGroupManagementUser_CheckedChanged" AutoPostBack="true" />
                        <asp:RadioButton ID="rbIsNotGroupManagementUser" runat="server" Text="No" GroupName="GroupManagementUser"
                            OnCheckedChanged="rbIsGroupManagementUser_CheckedChanged" AutoPostBack="true" />
                    </div>
                </div>
                <div class="form-group col-lg-12 col-sm-12"  ID="companyRow" runat="server" style="display:none">
                    <label class="col-sm-6 control-label text-left">Company</label>
                    <div class="col-sm-6">
                        <asp:DropDownList ID="ddlCompany" runat="Server" DataTextField="Name" DataValueField="CoyID"
                            CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" />
                    </div>
                </div>
                <div class="form-group col-lg-12 col-sm-12"  runat="server" ID="companyManagementRow" style="display:none">
                    <label class="col-sm-6 control-label text-left">Company Management User</label>
                    <div class="col-sm-6 radio-inline m-b-3">
                        <asp:RadioButton ID="rbIsCompanyManagementUser" runat="server" Text="Yes" GroupName="CompanyManagementUser"
                            OnCheckedChanged="rbIsCompanyManagementUser_CheckedChanged" AutoPostBack="true" />
                        <asp:RadioButton ID="rbIsNotCompanyManagementUser" runat="server" Text="No" GroupName="CompanyManagementUser"
                            OnCheckedChanged="rbIsCompanyManagementUser_CheckedChanged" AutoPostBack="true" />
                    </div>
                </div>
                <div class="form-group col-lg-12 col-sm-12"  runat="server" ID="productManagementRow" style="display:none">
                    <label class="col-sm-6 control-label text-left">Product Management User</label>
                    <div class="col-sm-6 radio-inline m-b-3">
                        <asp:RadioButton ID="rbIsProductManagementUser" runat="server" Text="Yes" GroupName="ProductManagementUser"
                            OnCheckedChanged="rbIsProductManagementUser_CheckedChanged" AutoPostBack="true" />
                        <asp:RadioButton ID="rbIsNotProductManagementUser" runat="server" Text="No" GroupName="ProductManagementUser"
                            OnCheckedChanged="rbIsProductManagementUser_CheckedChanged" AutoPostBack="true" />
                    </div>
                </div>
                <div runat="server" id="divisionRow" visible="false" class="form-group col-lg-12 col-sm-12">
                    <label class="col-sm-6 control-label text-left">
                        Division User
                    </label>
                    <div class="col-sm-6 radio-inline m-b-3">
                        <asp:RadioButton ID="rbGasDivisionUser" runat="server" Text="Gas" GroupName="DivisionUser"
                            OnCheckedChanged="rbDivisionUser_CheckedChanged" AutoPostBack="true" />
                        <asp:RadioButton ID="rbWeldingDivisionUser" runat="server" Text="Welding" GroupName="DivisionUser"
                            OnCheckedChanged="rbDivisionUser_CheckedChanged" AutoPostBack="true" />
                        <asp:RadioButton ID="rbAll" runat="server" Text="All" GroupName="DivisionUser"
                            OnCheckedChanged="rbDivisionUser_CheckedChanged" AutoPostBack="true" />
                    </div>
                </div>
            </div>
        </div>
    </div>
            <asp:ScriptManager ID="scriptMgr" runat="server" />
            <asp:UpdatePanel ID="udpMappingUpdater" runat="server" UpdateMode="conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-6 col-md-6">
                                <asp:DataGrid ID="dgResult" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                    DataKeyField="SalesPersonID" OnItemCommand="dgResult_CreateCommand" OnPageIndexChanged="dgResult_PageIndexChanged"
                                    GridLines="None" OnItemDataBound="dgResult_ItemDataBound" OnDeleteCommand="dgResult_DeleteCommand"
                                    CellPadding="5" CssClass="table table-bordered" AllowPaging="False" PageSize="30">
                                    <Columns>
                                <asp:TemplateColumn HeaderText="No">
                                    <ItemTemplate>
                                        <%# (Container.ItemIndex + 1) + ((dgResult.CurrentPageIndex) * dgResult.PageSize)%></ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Own SalesPersonID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSalesPersonID" runat="server">
                                            <%# Eval( "SalesPersonID")  + " - " + Eval( "SalesPersonObject.SalesPersonName")%>
                                            <input type="hidden" id="hidSalesPersonID" runat="server" value='<%# Eval("SalesPersonID")%>' />
                                            <input type="hidden" id="hidUserNumID" runat="server" value='<%# Eval("UserID")%>' />
                                            <input type="hidden" id="hidCoyID" runat="server" value='<%# Eval("CoyID")%>' />
                                        </asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlNewSalesPersonID" runat="Server" DataTextField="SalesPersonNameNID"
                                            DataValueField="SalesPersonID" CssClass="form-control" />
                                    </FooterTemplate>
                                    <HeaderStyle Wrap="False" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Function">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                            CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                            ValidationGroup="valGrpEditRow"  CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                            CausesValidation="false"  CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                            ValidationGroup="valGrpNewRow"  CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <FooterStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateColumn>
                            </Columns>
                            <HeaderStyle CssClass="tHeader" />
                            <AlternatingItemStyle CssClass="tAltRow" />
                            <FooterStyle CssClass="tFooter" />
                            <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                        </asp:DataGrid>
                        </div>
                        <div class="col-lg-6 col-md-6">
                                     <asp:DataGrid ID="dgResult2" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                            DataKeyField="SalesPersonID" OnItemCommand="dgResult2_CreateCommand" OnPageIndexChanged="dgResult2_PageIndexChanged"
                                            GridLines="None" OnItemDataBound="dgResult2_ItemDataBound" OnDeleteCommand="dgResult2_DeleteCommand"
                                            CellPadding="5" CssClass="table table-bordered" AllowPaging="False" PageSize="30">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="No">
                                                    <ItemTemplate>
                                                        <%# (Container.ItemIndex + 1) + ((dgResult2.CurrentPageIndex) * dgResult2.PageSize)%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Managing SalesPersonID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSalesPersonID" runat="server">
                                                            <%# Eval( "SalesPersonID") + " - " + Eval( "SalesPersonObject.SalesPersonName" )%>
                                                            <input type="hidden" id="hidSalesPersonID" runat="server" value='<%# Eval("SalesPersonID")%>' />
                                                            <input type="hidden" id="hidManagerUserID" runat="server" value='<%# Eval("ManagerUserID")%>' />
                                                            <input type="hidden" id="hidCoyID" runat="server" value='<%# Eval("CoyID")%>' />
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlNewSalesPersonID" runat="Server" DataTextField="SalesPersonNameNID"
                                                            DataValueField="SalesPersonID" CssClass="form-control" />
                                                    </FooterTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Function">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                                            CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkSave" runat="server" CommandName="Update" EnableViewState="false"
                                                            ValidationGroup="valGrpEditRow" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                                            CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                                            ValidationGroup="valGrpNewRow" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <FooterStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle CssClass="tHeader" />
                                            <FooterStyle CssClass="tFooter" />
                                            <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
                                        </asp:DataGrid>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
    

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
            $(".account-menu").addClass("active expand");
            $(".sub-salesman-map").addClass("active");
        });
    </script>
</asp:Content>