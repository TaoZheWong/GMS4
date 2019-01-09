<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="ViewResources.aspx.cs" Inherits="GMSWeb.UsefulResources.Resources.ViewResources"
    Title="" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <a name="TemplateInfo"></a>

    <ul class="breadcrumb pull-right">
        <li class="active">
            <asp:Label ID="lblPageHeader" runat="server"></asp:Label></li>
    </ul>
    <h1 class="page-header">Upload
    <br />
        <small>Click to view a document. You can also upload a document if you have access for uploading.
        </small>
    </h1>
    
    <input id="hidModuleCategoryID" runat="server" type="hidden" />
    <input id="hidModuleCategoryName" runat="server" type="hidden" />
    <input id="hidDocumentCateogryID" runat="server" type="hidden" value="" />
    
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
                            <asp:Repeater ID="rppReportList" runat="server" OnItemCommand="rppReportList_ItemCommand" OnItemDataBound="rppReportList_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="table table-condensed table-striped table-hover">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr id="rppRow" runat="server">
                                        <td style="width:2%;">
                                            <%# Container.ItemIndex + 1 %>
                                        </td>
                                        <td style="width:83%;">
                                            <asp:LinkButton ID="linkName" runat="server" Text='<%# Eval("DocumentName")%>' CommandName="Load" ForeColor="#005DAA" CommandArgument='<%#Eval("FileName")%>'></asp:LinkButton>
                                        </td>
                                        <td style="width:10%;">
                                            <input type="hidden" id="hidDocumentID" runat="server" value='<%# Eval("DocumentID")%>' />
                                            <input type="hidden" id="hidNumOfDocs" runat="server" value='<%# Eval("NumOfDocs")%>' />
                                            <asp:LinkButton ID="lnkViewHistory" CommandName="ViewHistory" runat="server" EnableViewState="false" Visible="false">
							                <span>&nbsp;&nbsp;View History</span></asp:LinkButton>
                                        </td>
                                        <td style="width:5%;" class="text-right">
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"DocumentID")%>' EnableViewState="true"
                                                CausesValidation="false" Visible='<%# CanDelete %>' OnClientClick="return confirm('Are you sure you want to delete this document?')"
                                                CssClass="btn btn-default btn-sm" data-toggle="tooltip" data-placement="left" title="Delete">
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
    <div id="tbUpload" runat="server">
        <div class="note note-info">
            <h4 class="block"><i class="ti-info-alt"></i>&nbsp;Info! </h4>
            <p>
                <asp:Label CssClass="tbLabelInfo" ID="Label2" runat="server"></asp:Label>
                (If you select an existing document and key in a new document name, the name of the existing document will be updated.)
            </p>
        </div>
        <div class="panel panel-primary">
            <div class="panel-heading">
                <div class="panel-heading-btn">
                    <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                </div>
                <h4 class="panel-title">
                    <i class="ti-pencil"></i>Add/Edit a Document  
                </h4>
            </div>
            <div class="panel-body row">
                <div class="form-horizontal m-t-20">
                    <div class="form-group col-lg-4 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="lblYear" runat="server">Category</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <asp:DropDownList CssClass="form-control" ID="ddlDocumentCategory" runat="server" DataTextField="CategoryName" DataValueField="DocumentCategoryID" AutoPostBack="true" OnSelectedIndexChanged="ddlDocumentCategory_SelectedIndexChanged" />
                        </div>
                    </div>
                    <div class="form-group col-lg-4 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label ID="lblDocumentName" runat="server">Document Name (Existing)</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <asp:DropDownList CssClass="form-control" ID="ddlDocument" runat="server"
                                DataTextField="DocumentName" DataValueField="DocumentID" AutoPostBack="true" OnSelectedIndexChanged="ddlDocument_SelectedIndexChanged" />
                        </div>
                    </div>
                    <div class="form-group col-lg-4 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="Label1" runat="server">Document Name (New)</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control" ID="txtDocumentName" runat="server" Style="text-transform: uppercase;" />
                        </div>
                    </div>
                    <div class="form-group col-lg-4 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">File Location</asp:Label>
                        </label>
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
                    <div class="form-group col-lg-4 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="lblSequence" runat="server">Sequence</asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <asp:DropDownList CssClass="form-control" ID="ddlSequence" runat="server"
                                DataTextField="SeqID" DataValueField="SeqID" />
                        </div>
                    </div>
                    <div class="form-group col-lg-4 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="lblOverwrite" runat="server">Overwrite Existing Document ?
                            <span data-toggle="tooltip" data-placement="top" title="(Previous document will be arhived)">
                                <i class="glyphicon glyphicon-info-sign"></i>
                            </span>
                            </asp:Label>
                        </label>
                        <div class="col-sm-6">
                            <div class="radio-inline m-b-3">
                                <asp:RadioButtonList ID="rblOverwriteDocument" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Selected>Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel-footer clearfix">
                <small class="pull-left">
                    <asp:Label ID="lblMsg" runat="server" ForeColor="red"></asp:Label></small>
                <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload or Update" CssClass="btn btn-primary pull-right"
                    OnClick="btnUpload_Click" OnClientClick="this.disabled=true; this.value='Updating ...';" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">

            $(document).ready(function () {

            //if (getParameterByName("ModuleCategoryID") != "18") {
            //    if (getParameterByName("ModuleCategoryID") == "8") {
            //        $(".administration-menu").addClass("active expand");
            //        $(".sub-forms").addClass("active");
                    
            //    } else {
            //        $(".resources-menu").addClass("active expand");
            //        $(".sub-resource-accounting-regulations").addClass("active");
            //    }

                if (getParameterByName("ModuleCategoryID") == "10") {
                    if (getParameterByName("DocumentCategoryID") == "31") {
                        $(".resources-menu").addClass("active expand");
                        $(".sub-resource-accounting-regulations").addClass("active");
                    
                    } else if (getParameterByName("DocumentCategoryID") == "32") {
                        $(".resources-menu").addClass("active expand");
                        $(".sub-resource-company-information").addClass("active");

                    } else if (getParameterByName("DocumentCategoryID") == "14") {
                        $(".resources-menu").addClass("active expand");
                        $(".sub-resource-listing-of-codes").addClass("active");
                    }

                    else if (getParameterByName("DocumentCategoryID") == "15") {
                        $(".resources-menu").addClass("active expand");
                        $(".sub-resource-forms").addClass("active");
                    }
                    else if (getParameterByName("DocumentCategoryID") == "35,29") {
                        $(".resources-menu").addClass("active expand");
                        $(".sub-resource-finance-seminar").addClass("active");
                    }
                    else if (getParameterByName("DocumentCategoryID") == "33,28") {
                        $(".resources-menu").addClass("active expand");
                        $(".sub-resource-guides").addClass("active");
                    }
                    else if (getParameterByName("DocumentCategoryID") == "34") {
                        $(".resources-menu").addClass("active expand");
                        $(".sub-resource-currency-exchange-rate").addClass("active");
                    }
                    else {
                        
                    }
              

                
            } else {
                $(".hr-menu").addClass("active expand");
                $(".sub-hr").addClass("active");
            }
        });
    </script>
</asp:Content>