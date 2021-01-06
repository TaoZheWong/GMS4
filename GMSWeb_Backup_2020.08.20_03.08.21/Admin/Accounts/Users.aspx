<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="GMSWeb.Admin.Accounts.Users1" Title="System Admin - Users Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <a name="TemplateInfo"></a>

    <ul class="breadcrumb pull-right">
        <li><a href="#">Accounts</a></li>
        <li class="active">Users</li>
    </ul>
    <h1 class="page-header">Users <br />
        <small>Create new user account or click on an acount to modify.</small>
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
            <div class="form-horizontal form-group-sm row m-t-20">
                <div class="form-group col-lg-4 col-sm-6">
                    <label class="col-sm-6 control-label text-left">User Name</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="searchUserName" MaxLength="50" Columns="50" onfocus="select();" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer clearfix">
            <asp:Button ID="btnSearch" Text="Search" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right" OnClick="btnSearch_Click"></asp:Button>
        </div>
    </div>

    <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
    <br />
    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-align-justify"></i>
                User List
            </h4>
        </div>
        <div class="table-responsive">
            <atlas:UpdatePanel ID="udpUsersUpdater" runat="server" Mode="conditional">
                <ContentTemplate>
                    <asp:DataGrid ID="dgUsers" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                        DataKeyField="Id" OnCancelCommand="dgUsers_CancelCommand" OnEditCommand="dgUsers_EditCommand"
                        OnUpdateCommand="dgUsers_UpdateCommand" OnItemCommand="dgUsers_ItemCommand"
                        GridLines="none" OnItemDataBound="dgUsers_ItemDataBound" OnPageIndexChanged="dgUsers_PageIndexChanged"
                        CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" AllowPaging="True" PageSize="50">
                        <Columns>
                            <asp:TemplateColumn HeaderText="No">
                                <ItemTemplate>
                                    <input type="hidden" id="hidUserID" runat="server" value='<%# Eval("Id")%>' />
                                    <%# (Container.ItemIndex + 1) + ((dgUsers.CurrentPageIndex) * dgUsers.PageSize)%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="User Name" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblUserRealName" runat="server">
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                            CausesValidation="false"><span><%# Eval( "UserRealName" )%></span></asp:LinkButton>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtEditUserRealName" runat="Server" Columns="20" MaxLength="50" Text='<%# Eval("UserRealName") %>' />
                                    <asp:RequiredFieldValidator ID="rfvEditUserRealName" runat="server" ControlToValidate="txtEditUserRealName"
                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtNewUserRealName" runat="Server" Columns="20" MaxLength="50" />
                                    <asp:RequiredFieldValidator ID="rfvNewUserRealName" runat="server" ControlToValidate="txtNewUserRealName"
                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Login ID" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblLoginID" runat="server">
								           <%# Eval( "UserName" )%>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtEditLoginID" runat="Server" Columns="10" MaxLength="50" Text='<%# Eval("UserName") %>' />
                                    <asp:RequiredFieldValidator ID="rfvEditLoginID" runat="server" ControlToValidate="txtEditLoginID"
                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtNewLoginID" runat="Server" Columns="10" MaxLength="50" />
                                    <asp:RequiredFieldValidator ID="rfvNewLoginID" runat="server" ControlToValidate="txtNewLoginID"
                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Email">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmail" runat="server">
										    <%# Eval("MemberObject.Email")%>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtEditEmail" runat="server" Columns="30" MaxLength="50" Text='<%# Eval("MemberObject.Email") %>' />
                                    <asp:RequiredFieldValidator ID="rfvEditEmail" runat="server" ControlToValidate="txtEditEmail"
                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox CssClass="form-control input-sm" ID="txtNewEmail" runat="server" Columns="30" MaxLength="50" />
                                    <asp:RequiredFieldValidator ID="rfvNewEmail" runat="server" ControlToValidate="txtNewEmail"
                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Active">
                                <ItemTemplate>
                                    <%# ( (bool)Eval( "MemberObject.IsApproved" ) ) ? "Yes" : "No"%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="checkbox input-sm no-margin">
                                        <asp:CheckBox ID="chkActive" runat="server" Text=" "/>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Remote Access">
                                <ItemTemplate>
                                    <%# ( (bool)Eval( "AllowRemoteAccess" ) ) ? "Yes" : "No"%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="checkbox input-sm no-margin">
                                        <asp:CheckBox ID="chkEditAllowRemoteAccess" runat="server" Text=" " />
                                    </div>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <div class="checkbox input-sm no-margin">
                                        <asp:CheckBox ID="chkNewAllowRemoteAccess" runat="server" Checked="false" Text=" "/>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hypEditAccessRights" runat="server" EnableViewState="false"
                                         CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Edit Access Rights"><i class="ti-pencil"></i></asp:HyperLink>
                                    <asp:LinkButton ID="hypResetPassword" runat="server" CommandName="ResetPassword" EnableViewState="false"
                                        CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Reset Password"><i class="ti-key"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                        CausesValidation="false"  CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i></asp:LinkButton>
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
            $(".account-menu").addClass("active expand");
            $(".sub-users").addClass("active");
        });
    </script>
</asp:Content>