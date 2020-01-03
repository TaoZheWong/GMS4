<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" MaintainScrollPositionOnPostback="true"
    AutoEventWireup="true" CodeBehind="Recruitment.aspx.cs" Inherits="GMSWeb.HR.Recruitment.Recruitment" Title =""%>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <a name="TemplateInfo"></a>
    <ul class="breadcrumb pull-right">
        <li><a href="#">HR</a></li>
        <li class="active">Recruitment</li>
    </ul>
    <h1 class="page-header">Recruitment<br />
        <small>Upload Resume to the server. Maximum 5 Pages accepted.</small></h1>

    <asp:ScriptManager ID="scriptMgr" runat="server" />
    <asp:UpdatePanel ID="udpMappingUpdater" runat="server" UpdateMode="conditional">
        <ContentTemplate>
            <asp:UpdatePanel ID="upAttachment" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <p style="color: Red; size: 7px; font-style: italic;">
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    </p>
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="panel-heading-btn">
                                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
                            </div>
                            <h4 class="panel-title">
                                <i class="ti-upload"></i> Recruitment
                            </h4>
                        </div>
                        <div class="panel-body row">
                            <div class="m-t-20">	
                                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                                    <label class="control-label">
                                        <asp:Label CssClass="tbLabel" ID="lblFileName" runat="server">Candidate Name</asp:Label>
                                        <asp:RequiredFieldValidator ID="rfvFileName" runat="server" ControlToValidate="txtName" ErrorMessage="*" Display="dynamic" ValidationGroup="attachment" />
                                    </label>
                                        <asp:TextBox runat="server" ID="txtName" MaxLength="100" Columns="45" onfocus="select();" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-6 col-md-6 col-sm-6">
                                    <label class="control-label">
                                        <asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">Resume File</asp:Label>
                                        <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="FileUpload"
                                            ErrorMessage="*" Display="dynamic" ValidationGroup="attachment" />
                                    </label>
                                        <div class="input-group">
                                            <input type="text" class="form-control" readonly>
                                             <label class="input-group-btn">
                                                <span class="btn btn-primary btn-upload">
                                                    <i class="ti-files" data-toggle="tooltip" data-placement="top" title="Upload"></i>
                                                    <asp:FileUpload CssClass="form-control hidden" ID="FileUpload" runat="server" />
                                                </span>
                                            </label>    
                                        </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer clearfix">
                            <asp:Button ID="btnUpload" Text="Upload" EnableViewState="False" runat="server" CssClass="btn btn-primary pull-right"
                                    OnClick="btnUpload_Click" ValidationGroup="attachment"></asp:Button>
                        </div>
                    </div>
                                
                      
                <asp:Label ID="lblAttachmentMsg" runat="server"></asp:Label></span>
                
                   <div class="panel panel-primary">
                    <div class="panel-heading">
                        <div class="panel-heading-btn">
                            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
                        </div>
                        <h4 class="panel-title">
                            <i class="ti-align-justify"></i> 
                            <asp:Label ID="lblAttachmentSummary" Visible="false" runat="server" />
                        </h4>
                    </div>
                    <div class="panel-body no-padding">
                        <div class="table-responsive">               
                            <asp:DataGrid ID="dgAttachment" runat="server" AutoGenerateColumns="false"
                                GridLines="none" CellPadding="5" CellSpacing="5" CssClass="table table-condensed table-striped table-hover" AllowPaging="true"
                                PageSize="20" OnPageIndexChanged="dgAttachment_PageIndexChanged" EnableViewState="true" OnSortCommand="SortAttachment" AllowSorting="true"
                                OnItemDataBound="dgAttachment_ItemDataBound" OnDeleteCommand="dgAttachment_DeleteCommand" OnItemCommand="dgAttachment_Command">
                                <Columns >
                                    <asp:TemplateColumn HeaderText="No" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# (Container.ItemIndex + 1) + ((dgAttachment.CurrentPageIndex) * dgAttachment.PageSize)%>
                                            <input type="hidden" id="hidFileID" runat="server" value='<%# Eval("FileID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                    <asp:TemplateColumn HeaderText="File Name" SortExpression="FileName" HeaderStyle-Wrap="false" 
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" OnCommand="lnkView_Click" CommandArgument='<%#Eval("FileName")%>'><span><%# Eval("FileName")%></span></asp:LinkButton>
                                            <input type="hidden" id="hidFileName" runat="server" value='<%#Eval("FileName")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Uploaded Date" SortExpression="CreatedDate" HeaderStyle-Wrap="false" 
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Eval("CreatedDate", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Uploaded By" SortExpression="userrealname" HeaderStyle-Wrap="false" 
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Eval("userrealname")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center"  HeaderText="Function">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSend" runat="server" OnCommand="lnkSend_Click" CommandArgument='<%#Eval("FileName")%>'  CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Send Mail">
                                                <i class="ti-email"></i> 
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" HeaderText="Function">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" EnableViewState="false" CausesValidation="false"  CssClass="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Delete">
                                                <i class="ti-trash"></i> 
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <HeaderStyle CssClass="tHeader" />
                                <AlternatingItemStyle CssClass="tAltRow" />
                                <FooterStyle CssClass="tFooter" />
                                <PagerStyle Font-Bold="true" Mode="NumericPages" CssClass="grid_pagination"/>
                            </asp:DataGrid>
                        </div>
                    </div>
                </div>                    
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnUpload" />
                </Triggers>
            </asp:UpdatePanel>
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
            $(".recruitment-menu").addClass("active expand");
            $(".sub-recruitment").addClass("active");
        });
    </script>
</asp:Content>
