<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="CustomerTypeSetup.aspx.cs" Inherits="GMSWeb.Sales.Sales.CustomerTypeSetup" %>
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
    <li class="active">Customer Type Setup</li>
</ul>
<h1 class="page-header">Customer Type Setup <br /><small>Setup of Customer Type.</small></h1>
         
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
                    DataKeyField="ClassID" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
                    OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" GridLines="none"
                    OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
                    CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>
                                <input type="hidden" id="hidClassID" runat="server" value='<%# Eval("ClassID")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Class" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblClassName" runat="server">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="true"
                                            CausesValidation="false"><span><%# Eval("ClassName")%></span></asp:LinkButton>
                                </asp:Label>
                            </ItemTemplate>
                        <EditItemTemplate>
                              <%# Eval("ClassName")%>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList CssClass="form-control input-sm" ID="ddlNewClassName" runat="Server" DataTextField="ClassName"
                                    DataValueField="ClassID" />
                            </FooterTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Short Name">
                             <ItemTemplate>                               
                                <%# Eval( "ShortName")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditShortName" runat="server" Columns="15" MaxLength="3" Text='<%# Eval("ShortName") %>' />
                                 <asp:RequiredFieldValidator
									ID="rfvEditShortName" runat="server" ControlToValidate="txtEditShortName" ErrorMessage="*" Display="dynamic" MaxLength="3" ValidationGroup="valGrpEditShortName" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewShortName" runat="server" Columns="15" MaxLength="3" />
                                <asp:RequiredFieldValidator
									ID="rfvNewShortName" runat="server" ControlToValidate="txtNewShortName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewShortName" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                       
                         <asp:TemplateColumn HeaderText="Value">
                            <ItemTemplate>
                                <asp:Label ID="lblValue" runat="server">
												       <%# Eval("Value")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm custom-number" ID="txtEditValue" runat="server" Columns="15" MaxLength="50" Text='<%# Eval("Value") %>' />
                                  <asp:RequiredFieldValidator
									ID="rfvEditValue" runat="server" ControlToValidate="txtEditValue" ErrorMessage="*"  Display="dynamic" ValidationGroup="valGrpEditShortName" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm custom-number" ID="txtNewValue" runat="server" Columns="15" MaxLength="50" />
                                 <asp:RequiredFieldValidator
									ID="rfvNewValue" runat="server" ControlToValidate="txtNewValue" ErrorMessage="*"  Display="dynamic" ValidationGroup="valGrpNewShortName" />
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
                                    ValidationGroup="valGrpEditShortName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Save"><i class="ti-check"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" EnableViewState="false"
                                    CausesValidation="false" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Cancel"><i class="ti-close"></i></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="lnkCreate" runat="server" CommandName="Create" EnableViewState="false"
                                    ValidationGroup="valGrpNewShortName" CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="top" title="Add"><i class="ti-plus"></i></asp:LinkButton>
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
            $(".sub-customertype-setup").addClass("active");
        });

        var numberElements = document.getElementsByClassName("custom-number");
        // Loop through each one
        for (var i = 0; i < numberElements.length; i++) {
            // Get your current element
            numberElements[i].type = 'number';
        }

    </script>
</asp:Content>
