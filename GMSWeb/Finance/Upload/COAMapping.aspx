<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="COAMapping.aspx.cs" Inherits="GMSWeb.Finance.Upload.COAMapping" Title="Finance - COA Mapping Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="breadcrumb pull-right">
        <li><a href="#">Administration</a></li>
        <li class="active">COA Mapping</li>
    </ul>
    <h1 class="page-header">COA Mapping <br />
        <small>
            For companies which are not using A21 systems, their Chart Of Accounts can be mapped to Leeden's Chart Of Accounts in order to view the companies' financials using Leeden's standard financial reports.
       </small>
    </h1>
    <asp:ScriptManager ID="scriptMgr" runat="server" />
    <asp:UpdatePanel ID="udpBankUpdater" runat="server" UpdateMode="conditional">
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
                            <label class="col-sm-6 control-label text-left">Old COA</label>
                            <div class="col-sm-6">
                                <asp:TextBox runat="server" ID="searchOldCOA" MaxLength="50" Columns="50" onfocus="select();"
                                    CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group col-lg-4 col-sm-6">
                            <label class="col-sm-6 control-label text-left">New COA</label>
                            <div class="col-sm-6">
                                <asp:TextBox runat="server" ID="searchNewCOA" MaxLength="50" Columns="50" onfocus="select();"
                                    CssClass="form-control"></asp:TextBox>
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
            <asp:Label ID="lblSearchSummary" Visible="false" runat="server"></asp:Label>
        </h4>
    </div>
    <div class="table-responsive">
        <asp:DataGrid ID="dgData" runat="server" AutoGenerateColumns="false" ShowFooter="true"
        CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" OnItemCommand="dgData_CreateCommand"
        AllowPaging="true" PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged"
        OnEditCommand="dgData_EditCommand" OnCancelCommand="dgData_CancelCommand" GridLines="none"
        OnUpdateCommand="dgData_UpdateCommand"
        OnDeleteCommand="dgData_DeleteCommand" OnItemDataBound="dgData_ItemDataBound"
        EnableViewState="true" >
        <Columns>
            <asp:TemplateColumn HeaderText="No" >
                <ItemTemplate>
                    <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Old COA" HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <asp:Label ID="lblOldCOAID" runat="server">
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                            CausesValidation="false"><span>
								<%# Eval("OldCOAID")%>
							</span></asp:LinkButton>
                        <input type="hidden" id="hidOldCOAID" runat="server" value='<%# Eval("OldCOAID")%>' />
                    </asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox CssClass="form-control input-sm" ID="txtNewOldCOAID" runat="server" Columns="30" MaxLength="50" />
                    <asp:RequiredFieldValidator ID="rfvNewOldCOAID" runat="server" ControlToValidate="txtNewOldCOAID"
                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="New COA" HeaderStyle-Wrap="false" >
                <ItemTemplate>
                    <asp:Label ID="lblNewCOAID" runat="server"><span>
								<%# Eval("NewCOAID")%>
							</span>
                    </asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox CssClass="form-control input-sm" ID="txtEditNewCOAID" runat="server" Columns="30" MaxLength="50" Text='<%# Eval("NewCOAID")%>' />
                    <asp:RequiredFieldValidator ID="rfvEditNewCOAID" runat="server" ControlToValidate="txtEditNewCOAID"
                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditRow" />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox CssClass="form-control input-sm" ID="txtNewNewCOAID" runat="server" Columns="30" MaxLength="50" />
                    <asp:RequiredFieldValidator ID="rfvNewNewCOAID" runat="server" ControlToValidate="txtNewNewCOAID"
                        ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewRow" />
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="New 2016 COA" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lblIs2016COA" runat="server">
                        <%# ( (bool)Eval( "Is2016COA" ) ) ? "Yes" : "No"%>
                        <input type="hidden" id="hidIs2016COA" runat="server" value='<%# Eval("Is2016COA")%>' />
                    </asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <div class="checkbox input-sm no-margin">
                        <asp:CheckBox ID="chkNewIs2016COA" runat="server" Checked="false" Text=" "/>
                    </div>
                </FooterTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn  HeaderStyle-HorizontalAlign="Center"
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
        <AlternatingItemStyle CssClass="tAltRow" />
        <PagerStyle Mode="NumericPages" CssClass="grid_pagination" />
    </asp:DataGrid>
    </div>
</div>                        
        </ContentTemplate>
    </asp:UpdatePanel>
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
            $(".administration-menu").addClass("active expand");
            $(".sub-coa-mapping").addClass("active");
        });
    </script>
</asp:Content>
