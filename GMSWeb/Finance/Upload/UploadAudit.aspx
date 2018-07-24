<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="UploadAudit.aspx.cs" Inherits="GMSWeb.Finance.Upload.UploadAudit" Title="Finance - Upload Audit Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <ul class="breadcrumb pull-right">
        <li><a href="#">Accounting/Tax</a></li>
        <li class="active">Audit Upload</li>
    </ul>
    <h1 class="page-header">Audit Upload
    <br />
        <small>Upload yearly audited data for different purposes using Excel files. Figures should be keyed in 000s.
        </small>
    </h1>

    <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />

    <div class="note note-info">
        <h4 class="block"><i class="ti-info-alt"></i> Info! </h4>
        <p>If data has been existed for the year specified below, it will be overwritten. See the following files for examples:</p>
        <br />
        <ul>
            <li><a href="<%= Request.ApplicationPath %>/Documents/Audit_BalanceSheet.xls">Audit_BalanceSheet.xls</a></li>
            <li><a href="<%= Request.ApplicationPath %>/Documents/Audit_P&L_Company.xls">Audit_P&L_Company.xls</a></li>
        </ul>
    </div>
    
    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                <i class="ti-upload"></i> Upload
            </h4>
        </div>
        <div class="panel-body row">
            <div class="m-t-20">
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">
		                Year
                    </label>
	                    <asp:DropDownList CssClass="form-control" ID="ddlYear" runat="server" DataTextField="Year" DataValueField="Year" />
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">
		                <asp:Label CssClass="tbLabel" ID="lblPurpose" runat="server">Purpose</asp:Label>
                    </label>
	                    <asp:DropDownList CssClass="form-control" ID="ddlPurpose" runat="server" DataTextField="ItemPurposeName" DataValueField="ItemPurposeID" />
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-6">
                    <label class="control-label">
		                 <asp:Label CssClass="tbLabel" ID="lblLocation" runat="server">Location</asp:Label>
                    </label>
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
        <div class="panel-footer clearfix">
             <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload" CssClass="btn btn-primary pull-right" OnClick="btnUpload_Click" />
        </div>
    </div>
    
    <iframe id="IFrame1" frameborder="0" scrolling="YES" runat="Server" width="100%" style="display: none"></iframe>
    <asp:Label ID="lblMsg" runat="server"></asp:Label>    
                           
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
            $(".accounting-menu").addClass("active expand");
            $(".sub-upload-audit").addClass("active");
        });
    </script>
</asp:Content>