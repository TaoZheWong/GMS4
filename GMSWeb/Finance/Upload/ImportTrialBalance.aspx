<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="ImportTrialBalance.aspx.cs" Inherits="GMSWeb.Finance.Upload.ImportTrialBalance" Title="Finance - Upload Finance Data Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
    <ul class="breadcrumb pull-right">
        <li><a href="#">Administration</a></li>
        <li class="active">Importer/Exporter</li>
    </ul>
    <h1 class="page-header">Importer/Exporter
        <br />
        <small>Import data into GMS or export data from GMS.
        </small>
    </h1>
    <atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />

    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                1. Import Trial Balance
                <small>Click <b>Import</b> button below to import trial balance data into from A21 to GMS.</small>
            </h4>
        </div>
        <div class="panel-body row">
            <div class="form-horizontal m-t-20">
                <div class="form-group col-lg-4 col-sm-6">
                    <label class="col-sm-6 control-label text-left"><asp:Label CssClass="tbLabel" ID="lblYear" runat="server">Year</asp:Label></label>
                    <div class="col-sm-6">
                        <asp:DropDownList CssClass="form-control" ID="ddlYear" runat="server" DataTextField="Year" DataValueField="Year" />
                    </div>
                </div>
                <div class="form-group col-lg-4 col-sm-6">
                    <label class="col-sm-6 control-label text-left"><asp:Label CssClass="tbLabel" ID="lblMonth" runat="server" Style="margin-left: 10px">Month</asp:Label></label>
                    <div class="col-sm-6">
                        <asp:DropDownList CssClass="form-control" ID="ddlMonth" runat="server" DataTextField="Month" DataValueField="Month" />
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer clearfix">
            <asp:Button ID="btnImport" runat="server" Style="margin-left: 60px" CausesValidation="true" Text="Import" CssClass="btn btn-primary pull-right" OnClick="btnImport_Click" />
        </div>
    </div>                   
    <iframe id="IFrame1" frameborder="0" scrolling="YES" runat="Server" width="100%" style="display: none"></iframe>
       
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
            $(".administration-menu").addClass("active expand");
            $(".sub-import-export").addClass("active");
        });
    </script>
</asp:Content>
