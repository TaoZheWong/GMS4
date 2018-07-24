<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="PMPHome.aspx.cs" Inherits="GMSWeb.HR.PMP.PMPHome"
    Title="HR - PMP Home" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <a name="TemplateInfo"></a>
    

    <ul class="breadcrumb pull-right">
        <li class="active"><asp:Label ID="lblPageHeader" runat="server"></asp:Label></li>
    </ul>
    <h1 class="page-header">PMP <br />
        <small>Click to view a document. You can also upload a document if you have access for uploading.</small>
    </h1>

    <input id="hidModuleCategoryID" runat="server" type="hidden" />
    <input id="hidModuleCategoryName" runat="server" type="hidden" />
    
    <asp:Repeater ID="rppCategoryList" runat="server">
        <ItemTemplate>
             <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-list-ol"></i> <%# DataBinder.Eval(Container.DataItem, "CategoryName")%>
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive">
                        <asp:Repeater ID="rppReportList" runat="server" OnItemCommand="rppReportList_ItemCommand" OnItemDataBound="rppReportList_ItemDataBound">
                        <HeaderTemplate>
                            <table class="table table-condensed">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id="rppRow" runat="server">
                                <td style="width: 5%">
                                    <%# Container.ItemIndex + 1 %>.</td>
                                <td style="padding-left: 10px; width: 60%">
                                    <asp:LinkButton ID="linkName" runat="server" Text='<%# Eval("DocumentName")%>' CommandName="Load" ForeColor="#005DAA" CommandArgument='<%#Eval("FileName")%>'></asp:LinkButton>
                                </td>
                                <td style="padding-left: 10px; width: 20%">
                                    <input type="hidden" id="hidDocumentID" runat="server" value='<%# Eval("DocumentID")%>' />
                                    <input type="hidden" id="hidNumOfDocs" runat="server" value='<%# Eval("NumOfDocs")%>' />
                                    <asp:LinkButton ID="lnkViewHistory" CommandName="ViewHistory" runat="server" EnableViewState="false" Visible="false">
							        <span>&nbsp;&nbsp;View History</span></asp:LinkButton>
                                </td>
                                <td class="text-right">
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"DocumentID")%>' EnableViewState="true"
                                        CausesValidation="false" CssClass="btn btn-default btn-xs" Visible="<%# CanDelete %>"
                                        OnClientClick="return confirm('Are you sure you want to delete this document?')" data-toggle="tooltip" data-placement="left" title="Delete">
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

    <asp:Repeater ID="rppYourList" runat="server">
        <ItemTemplate>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-list-ol"></i><%# Container.DataItem %>
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive">
                    <asp:Repeater ID="rppYourPMPList" runat="server" OnItemCommand="rppYourPMPList_ItemCommand" OnItemDataBound="rppYourPMPList_ItemDataBound">
                        <HeaderTemplate>
                            <table class="table table-condensed">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id="rppRow" runat="server">
                                <td style="width: 5%">
                                    <%# Container.ItemIndex + 1 %>.</td>
                                <td >
                                    <input type="hidden" id="hidNumOfPMP_1" runat="server" value='<%# Eval("NumOfPMP")%>' />
                                    <input type="hidden" id="hidEmployeeID_1" runat="server" value='<%# Eval("EmployeeID")%>' />
                                    <asp:LinkButton ID="lnkName_1" CommandName="LoadYourPMP" runat="server" EnableViewState="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"EmployeeID")%>' Text='<%# Eval("Name").ToString().ToUpper() %>' ForeColor="#005DAA"></asp:LinkButton>
                                </td>
                                <td class="text-right">
                                    <asp:LinkButton ID="lnkUpdateViewPMP" CommandName="UpdateViewPMP" runat="server" EnableViewState="false" CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="left" title="Update/View PMP">
							            <i class="ti-search"></i>
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

    <asp:Repeater ID="rppStaffList" runat="server">
        <ItemTemplate>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-list-ol"></i><%# Container.DataItem %>
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive">
                        <asp:Repeater ID="rppEmployeeList" runat="server" OnItemCommand="rppEmployeeList_ItemCommand" OnItemDataBound="rppEmployeeList_ItemDataBound">
                            <HeaderTemplate>
                                <table class="table table-condensed">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr id="rppRow" runat="server">
                                    <td style="width:5%">
                                        <%# Container.ItemIndex + 1 %>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hidNumOfPMP" runat="server" value='<%# Eval("NumOfPMP")%>' />
                                        <input type="hidden" id="hidEmployeeID" runat="server" value='<%# Eval("EmployeeID")%>' />
                                        <asp:LinkButton ID="lnkName" CommandName="LoadStaffPMP" runat="server" EnableViewState="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"EmployeeID")%>' Text='<%# Eval("Name").ToString().ToUpper() %>' ForeColor="#005DAA"></asp:LinkButton>
                                    </td>
                                    <td class="text-right">
                                        <asp:LinkButton ID="lnkUpdateViewPMP" CommandName="UpdateViewPMP" runat="server" EnableViewState="false" CssClass="btn btn-default btn-xs"  data-toggle="tooltip" data-placement="left" title="Update/View PMP">
							                <i class="ti-search"></i>
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
    <asp:Repeater ID="rppIndirectStaffList" runat="server">
        <ItemTemplate>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="panel-heading-btn">
                        <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                    </div>
                    <h4 class="panel-title">
                        <i class="ti-list-ol"></i><%# Container.DataItem %>
                    </h4>
                </div>
                <div class="panel-body no-padding">
                    <div class="table-responsive">
                    <asp:Repeater ID="rppIndirectStaffPMPList" runat="server" OnItemCommand="rppIndirectStaffPMPList_ItemCommand" OnItemDataBound="rppIndirectStaffPMPList_ItemDataBound">
                        <HeaderTemplate>
                            <table class="table table-condensed">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id="rppRow" runat="server">
                                <td style="width: 5%">
                                    <%# Container.ItemIndex + 1 %>.</td>
                                <td>
                                    <input type="hidden" id="hidNumOfPMP_3" runat="server" value='<%# Eval("NumOfPMP")%>' />
                                    <input type="hidden" id="hidEmployeeID_3" runat="server" value='<%# Eval("EmployeeID")%>' />
                                    <asp:LinkButton ID="lnkName_3" CommandName="LoadIndirectStaffPMP" runat="server" EnableViewState="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"EmployeeID")%>' Text='<%# Eval("Name").ToString().ToUpper() %>' ForeColor="#005DAA"></asp:LinkButton>
                                </td>
                                <td class="text-right">
                                    <asp:LinkButton ID="lnkUpdateViewPMP" CommandName="UpdateViewPMP" runat="server" EnableViewState="false" CssClass="btn btn-default btn-xs">
							            <i class="ti-search"></i>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
        </ItemTemplate>
    </asp:Repeater>
    
    <br />
    
    <asp:Label ID="lblMsg" runat="server" ForeColor="red" ></asp:Label>

    <br />
    
    <div class="panel panel-primary <%=CanDelete ? "": "hidden" %>" >
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
		    <h4 class="panel-title">
		        <i class="ti-pencil"></i>
                 Add/Edit a Document
		    </h4>
	    </div>
        <div class="panel-body">
            <div id="tbUpload" runat="server" class="form-horizontal form-group-sm row m-t-20">
                 <div class="form-group col-lg-6 col-sm-6">
			            <label class="col-sm-6 control-label text-left"><asp:Label CssClass="tbLabel" ID="lblYear" runat="server">Category</asp:Label></label>
			        <div class="col-sm-6">
				         <asp:DropDownList CssClass="form-control" ID="ddlDocumentCategory" runat="server"
                                DataTextField="CategoryName" DataValueField="DocumentCategoryID" AutoPostBack="true" OnSelectedIndexChanged="ddlDocumentCategory_SelectedIndexChanged" />
			        </div>
		        </div>
                <div class="form-group col-lg-6 col-sm-6">
			            <label class="col-sm-6 control-label text-left"><asp:Label CssClass="tbLabel" ID="lblDocumentName" runat="server">Document Name (Existing)</asp:Label></label>
			        <div class="col-sm-6">
				        <asp:DropDownList CssClass="form-control" ID="ddlDocument" runat="server"
                                DataTextField="DocumentName" DataValueField="DocumentID" AutoPostBack="true" OnSelectedIndexChanged="ddlDocument_SelectedIndexChanged" />
			        </div>
		        </div>
                <div class="form-group col-lg-6 col-sm-6">
			            <label class="col-sm-6 control-label text-left">
                            <asp:Label CssClass="tbLabel" ID="Label1" runat="server">Document Name (New)</asp:Label>
                            <span data-toggle="tooltip" data-placement="top" title="(If you select an existing document and key in a new document name, the name of the existing document will be updated.)">
                                <i class="glyphicon glyphicon-info-sign"></i>
                            </span>
			            </label>
			        <div class="col-sm-6">
				        <asp:TextBox CssClass="form-control" ID="txtDocumentName" runat="server" Style="text-transform: uppercase;" />
			        </div>
		        </div>
                <div class="form-group col-lg-6 col-sm-6">
			            <label class="col-sm-6 control-label text-left"><asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">File Location</asp:Label></label>
			        <div class="col-sm-6">
				        <div class="input-group">
                            <input type="text" class="form-control" readonly>
                                <label class="input-group-btn">
                                <span class="btn btn-primary btn-upload btn-sm">
                                    <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                    <asp:FileUpload CssClass="form-control hidden" ID="FileUpload1" runat="server" />
                                </span>
                            </label>
                        </div>
			        </div>
		        </div>
                <div class="form-group col-lg-6 col-sm-6">
			            <label class="col-sm-6 control-label text-left"><asp:Label CssClass="tbLabel" ID="lblSequence" runat="server">Sequence</asp:Label></label>
			        <div class="col-sm-6">
				        <asp:DropDownList CssClass="form-control" ID="ddlSequence" runat="server"
                                DataTextField="SeqID" DataValueField="SeqID" />
			        </div>
		        </div> 
                <div class="form-group col-lg-6 col-sm-6">
			            <label class="col-sm-6 control-label text-left">
			                <asp:Label CssClass="tbLabel" ID="lblOverwrite" runat="server">Overwrite Existing Document ?</asp:Label>
                            <span data-toggle="tooltip" data-placement="top" title="(Previous document will be arhived)">
                                <i class="glyphicon glyphicon-info-sign"></i>
                            </span>
			            </label>
			            
                    <div class="col-sm-6">
				        <div class="radio-inline m-b-3">
				            <asp:RadioButtonList ID="rblOverwriteDocument" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="1" Selected>Yes</asp:ListItem>
                                <asp:ListItem Value="0">No </asp:ListItem>
                            </asp:RadioButtonList>
				        </div>
			        </div>
		        </div>  
            </div>
        </div>
        <div class="panel-footer clearfix">
             <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload or Update" CssClass="btn btn-primary pull-right"
                                OnClick="btnUpload_Click" OnClientClick="this.disabled=true; this.value='Updating ...';" UseSubmitBehavior="false" />
        </div>
    </div>

        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".pmp-menu").addClass("active expand");
            $(".sub-pmp").addClass("active");
        });
    </script>
</asp:Content>
