<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true"
    CodeBehind="ViewCompanyResources.aspx.cs" Inherits="GMSWeb.UsefulResources.Resources.ViewCompanyResources"
    Title="" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <h1 class="page-header">
        <asp:Label ID="lblPageHeader" runat="server"></asp:Label>
        <br />
        <small>Click to view a document. You can also upload a document if you have access for uploading.</small></h1>

    <a name="TemplateInfo"></a>
    <input id="hidModuleCategoryID" runat="server" type="hidden" />
    <input id="hidModuleCategoryName" runat="server" type="hidden" />


                <asp:Repeater ID="rppCategoryList" runat="server">
                    <ItemTemplate>
                        <div class="panel panel-primary">
                            <div class="panel-heading">
                                <div class="panel-heading-btn">
                                    <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                                    <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                                </div>
                                <h4 class="panel-title">
                                    <i class="ti-list-ol"></i><%# DataBinder.Eval(Container.DataItem, "CategoryName")%>
                                </h4>
                            </div>
                            <div class="panel-body no-padding">
                                <div class="table-responsive">
                                        <asp:Repeater ID="rppReportList" runat="server" OnItemCommand="rppReportList_ItemCommand">
                                            <HeaderTemplate>
                                                <table class="table table-condensed table-striped table-hover">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr id="rppRow" runat="server">
                                                    <td style="width:95%">
                                                        <%# Container.ItemIndex + 1 %>
                                                        . &nbsp; <a href='<%# Request.ApplicationPath + "/Data/Company Resources/" + Eval("coyid") + "/" + Eval("FileName")%>'>
                                                            <%# Eval("DocumentName")%>
                                                            </span></a>
                                                    </td>
                                                    <td style="width:5%">
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"DocumentID")%>' EnableViewState="true"
                                                            CausesValidation="false" Visible="<%# CanDelete %>" OnClientClick="return confirm('Are you sure you want to delete this document?')"
                                                            CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete">
                                                            <i class="ti-trash"></i> 
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                   </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

    <br />

    <div id="tbUpload" runat="server" class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-upload"></i>
                Upload
            </h4>
        </div>
        <div class="panel-body row">
            <div class="form-horizontal m-t-20">

                <div class="form-horizontal m-t-20">
                    <div class="form-group col-lg-4 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="lblYear" runat="server">Category</asp:Label></label>
                        <div class="col-sm-6">
                            <asp:DropDownList CssClass="form-control" ID="ddlDocumentCategory" runat="server"
                                DataTextField="CategoryName" DataValueField="DocumentCategoryID" />
                        </div>
                    </div>
                </div>
                <div class="form-horizontal m-t-20">
                    <div class="form-group col-lg-4 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="lblDocumentName" runat="server">Name</asp:Label></label>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control" ID="txtDocumentName" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="form-horizontal m-t-20">
                    <div class="form-group col-lg-4 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">Location</asp:Label></label>
                        <div class="col-sm-6">
                            <div class="input-group">
                                <input type="text" class="form-control" readonly>
                                 <label class="input-group-btn">
                                    <span class="btn btn-primary btn-upload">
                                        <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                        <asp:FileUpload CssClass="form-control hidden" ID="FileUpload1" runat="server" />
                                    </span>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer clearfix">
            <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload" CssClass="btn btn-primary pull-right"
                OnClick="btnUpload_Click" />
        </div>
    </div>
    <p>
        <asp:Label ID="lblMsg" runat="server"></asp:Label></p>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".accounting-menu").addClass("active expand");
            $(".sub-company-resource").addClass("active");
        });
    </script>
</asp:Content>