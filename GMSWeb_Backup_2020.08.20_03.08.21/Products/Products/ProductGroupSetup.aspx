<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="ProductGroupSetup.aspx.cs" Inherits="GMSWeb.Products.Products.ProductGroupSetup" %>

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
    <li class="active">Product Group Setup</li>
</ul>
<h1 class="page-header">Product Group Setup <br /><small>Setup of product group short name.</small></h1>

    <%--Search--%>
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
               <div class="form-group col-lg-4 col-md-6 col-sm-6">
                    <label class="control-label">Product Group Code</label>
                        <asp:TextBox runat="server" ID="txtProductGroupCode" MaxLength="4" Columns="20" onfocus="select();" CssClass="form-control" placeholder="e.g. A21"></asp:TextBox>
                </div>
                <div class="form-group col-lg-4 col-md-6 col-sm-6">
                    <label class="control-label">Product Group Name</label>
                        <asp:TextBox runat="server" ID="txtProductGroupName" MaxLength="50" Columns="20" onfocus="select();"
                            CssClass="form-control" placeholder="e.g. AUWELD FILLER METALS"></asp:TextBox>
                </div>
                <div class="form-group col-lg-4 col-md-6 col-sm-6">
                    <label class="control-label">Short Name</label>
                        <asp:TextBox runat="server" ID="txtShortName" MaxLength="50" Columns="20" onfocus="select();"
                                    CssClass="form-control" ></asp:TextBox>
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
     <div class="panel panel-primary" id="resultList" runat="server" visible="true">
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
                    DataKeyField="ProductGroupCode" OnCancelCommand="dgData_CancelCommand" OnEditCommand="dgData_EditCommand"
                    OnUpdateCommand="dgData_UpdateCommand" OnItemCommand="dgData_CreateCommand" GridLines="none"
                    OnItemDataBound="dgData_ItemDataBound" OnDeleteCommand="dgData_DeleteCommand"
                    CellPadding="5" CellSpacing="5" CssClass="table table-striped table-condensed table-hover" AllowPaging="true"
                    PageSize="20" OnPageIndexChanged="dgData_PageIndexChanged" EnableViewState="true">
                    <Columns>
                        <asp:TemplateColumn HeaderText="No">
                            <ItemTemplate>
                                <%# (Container.ItemIndex + 1) + ((dgData.CurrentPageIndex) * dgData.PageSize)  %>   
                                <input type="hidden" id="hidProductGroupCode" runat="server" value='<%# Eval("ProductGroupCode")%>' />                            
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Product Group Name" HeaderStyle-Wrap="false"  SortExpression="ProductGroupName">
                            <ItemTemplate>  
                                <asp:Label ID="lblProductGroupName" runat="server">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" EnableViewState="false"
                                            CausesValidation="false"><span><%# Eval("ProductGroupCodeName")%></span></asp:LinkButton>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%# Eval("ProductGroupCodeName")%>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList CssClass="form-control input-sm" ID="ddlNewProductGroup" runat="Server" DataTextField="ProductGroupCodeName" DataValueField="ProductGroupCode" Width="100%" />
                            </FooterTemplate>
                        </asp:TemplateColumn>                        
                       
                         <asp:TemplateColumn HeaderText="Short Name" HeaderStyle-Wrap="false"  SortExpression="ShortName">
                            <ItemTemplate>
                                <asp:Label ID="lblShortName" runat="server">
												       <%# Eval("ShortName")%>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtEditShortName" runat="server" Columns="40" MaxLength="20" Text='<%# Eval("ShortName") %>' />
                                  <asp:RequiredFieldValidator
									ID="rfvEditShortName" runat="server" ControlToValidate="txtEditShortName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpEditTeamName" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox CssClass="form-control input-sm" ID="txtNewShortName" runat="server" Columns="40" MaxLength="20" />
                                 <asp:RequiredFieldValidator
									ID="rfvNewEditShortName" runat="server" ControlToValidate="txtNewShortName" ErrorMessage="*" Display="dynamic" ValidationGroup="valGrpNewTeamName" />
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Is Budget" HeaderStyle-Wrap="false"  SortExpression="IsBudget">
                                        <ItemTemplate>
                                            <%# ( (bool)Eval( "IsBudget" ) ) ? "Yes" : "No"%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="checkbox input-sm no-margin">
                                                <asp:CheckBox ID="chkEditIsBudget" runat="server" Checked='<%# Eval("IsBudget") %>' Text=" "/>
                                            </div>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                             <div class="checkbox input-sm no-margin">
								                <asp:CheckBox ID="chkNewIsBudget" runat="server" Checked="false" Text=" "/>
							                </div>
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
            $(".sub-productgroup-setup").addClass("active");
        });
    </script>
</asp:Content>
