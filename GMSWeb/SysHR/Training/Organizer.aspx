<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Organizer.aspx.cs" Inherits="GMSWeb.SysHR.Training.Organizer" Title="Training - Organizer Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">Training</a></li>
        <li class="active">Organizer</li>
    </ul>
    <h1 class="page-header">Organizer <br />
        <small>List of training course organizers.</small></h1>
    <asp:ScriptManager ID="scriptMgr" runat="server" />
    <asp:UpdatePanel ID="udpOrganizerUpdater" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
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
                <div class="panel-body row">
                    <div class="form-horizontal m-t-20">
                        <div class="form-group col-lg-4 col-sm-6">
                            <label class="col-sm-6 control-label text-left">Organizer Name</label>
                            <div class="col-sm-6">
                                <asp:TextBox runat="server" ID="searchOrganizerName" MaxLength="50" Columns="50"
                                    onfocus="select();" CssClass="form-control"></asp:TextBox>
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
                <div class="table-responsive">
                    <asp:DataGrid ID="dgOrganizer" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                        DataKeyField="OrganizerName" OnCancelCommand="dgOrganizer_CancelCommand" OnEditCommand="dgOrganizer_EditCommand"
                        OnUpdateCommand="dgOrganizer_UpdateCommand" OnItemCommand="dgOrganizer_CreateCommand"
                        GridLines="none" OnItemDataBound="dgOrganizer_ItemDataBound" OnDeleteCommand="dgOrganizer_DeleteCommand"
                        CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                        PageSize="20" OnPageIndexChanged="dgOrganizer_PageIndexChanged" >
                        <Columns>
                            <asp:TemplateColumn HeaderText="No">
                                <ItemTemplate>
                                    <%# (Container.ItemIndex + 1) + ((dgOrganizer.CurrentPageIndex) * dgOrganizer.PageSize)  %>
                                    <input type="hidden" id="hidID" runat="server" value='<%# Eval("OrganizerID")%>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Organizer Name" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server">
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                            CausesValidation="false"><span><%# Eval( "OrganizerName" )%></span></asp:LinkButton>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditName" runat="server" Columns="50" MaxLength="80" Text='<%# Eval("OrganizerName") %>' CssClass="form-control input-sm"/>
                                    <asp:RequiredFieldValidator ID="rfvEditName" runat="server" ControlToValidate="txtEditName"
                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtNewName" runat="server" Columns="50" MaxLength="80" CssClass="form-control input-sm" />
                                    <asp:RequiredFieldValidator ID="rfvNewName" runat="server" ControlToValidate="txtNewName"
                                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderText="Function">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false"
                                        CausesValidation="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete"><i class="ti-trash"></i> </asp:LinkButton>
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
            $(".training-menu").addClass("active expand");
            $(".sub-organizer").addClass("active");
        });
    </script>
</asp:Content>