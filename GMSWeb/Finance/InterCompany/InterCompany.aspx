<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="InterCompany.aspx.cs" Inherits="GMSWeb.Finance.InterCompany.InterCompany" Title="Finance - InterCompany Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="~/CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="Header" Src="~/SiteHeader.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<uctrl:Header ID="MySiteHeader" runat="server" EnableViewState="true" />
<ul class="breadcrumb pull-right">
    <li><a href="#">Accounting/Tax</a></li>
    <li class="active">Inter-Company</li>
</ul>
<h1 class="page-header">Inter-Company
    <br />
    <small>Export inter-company transactions into Excel file, modify the data and upload the final figures into the system.
    </small>
</h1>

<atlas:ScriptManager ID="scriptMgr" runat="server" EnablePartialRendering="true" />
    <div class="note note-info">
        <h4 class="block"><i class="ti-info-alt"></i>&nbsp;Info! </h4>
        <p>Export inter-company transactions into Excel file for data input.</p>
    </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                1. Export Inter-Company Transactions
            </h4>
        </div>
        <div class="panel-body">
            <div class="row m-t-20">
                <div class="form-horizontal">
                    <div class="form-group col-lg-4 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
		                    <asp:Label CssClass="tbLabel" ID="lblYear" runat="server">Year</asp:Label>
                        </label>
                        <div class="col-sm-6">
	                        <asp:DropDownList CssClass="form-control" ID="ddlYear" runat="server" DataTextField="Year" DataValueField="Year" />
	                    </div>
                    </div>
                    <div class="form-group col-lg-4 col-sm-6">
                        <label class="col-sm-6 control-label text-left">
		                    <asp:Label CssClass="tbLabel" ID="lblMonth" runat="server" style="margin-left:10px">Month</asp:Label>
                        </label>
                        <div class="col-sm-6">
	                        <asp:DropDownList CssClass="form-control" ID="ddlMonth" runat="server" DataTextField="Month" DataValueField="Month" />
	                    </div>
                    </div>
		        </div>
            </div>
            
        </div>
        <div class="panel-footer clearfix">
            <asp:Button ID="btnExport" runat="server" style="margin-left:30px" CausesValidation="true" Text="Export" CssClass="btn btn-primary pull-right" OnClick="btnExport_Click"/>
        </div>
    </div>

    <div class="note note-info">
        <h4 class="block"><i class="ti-info-alt"></i>&nbsp;Info! </h4>
        <p>Upload final inter-company transactions into the system, using the Excel file you've exported and modified from the above step.</p>
    </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                2. Upload Inter-Company Transactions
            </h4>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="form-horizontal m-t-20">
                    <div class="col-lg-6 col-sm-6">
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
        </div>
        <div class="panel-footer clearfix">
            <asp:Button ID="btnUpload" runat="server" CausesValidation="true" Text="Upload" CssClass="btn btn-primary pull-right" OnClick="btnUpload_Click" />
        </div>
    </div>

      
    <IFRAME ID=IFrame FRAMEBORDER=0 SCROLLING=YES Runat="Server" width=100% Style="display:none"></IFRAME>	
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
		
    <div class="note note-info">
        <h4 class="block"><i class="ti-info-alt"></i>&nbsp;Info! </h4>
        <p>You can print the final inter-company transactions reports (see the following) at <b>Accounting/Tax > Reports</b>.</p>
    </div>
    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a data-init="true" title="" data-original-title="" href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            <h4 class="panel-title">
                3. Print Inter-Company Transactions Report
            </h4>
        </div>
        <div class="panel-body">
            <ul class="m-t-20">
                <li><b>F91P1 - Inter-Company Transactions (Balance Sheet)</b></li>
                <li><b>F91P2 - Inter-Company Transactions (P&L)</b></li>
            </ul>
        </div>
    </div>		             

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
            $(".sub-intercompany").addClass("active");
        });
    </script>
</asp:Content>