<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="CashFlowProjections.aspx.cs" Inherits="GMSWeb.Finance.CashFlow.CashFlowProjections" Title="Finance - Cash Flow Projections Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<link href="<%= Request.ApplicationPath %>/greybox/gb_styles.css" rel="stylesheet" type="text/css" />

<a name="TemplateInfo"></a>
<ul class="breadcrumb pull-right">
    <li><a href="#">Cash Flow/Loan</a></li>
    <li class="active">Cash Flow Projections</li>
</ul>
<h1 class="page-header">Cash Flow Projections </h1>

<asp:ScriptManager ID="sriptmgr1" runat="server">
    <Services>
        
    </Services>
</asp:ScriptManager>

 
<a href="#formModal" data-toggle="modal" class="btn btn-primary" >Start Cash Flow Projections</a>
    
<div id="formModal" class="modal fade" role="dialog">
    <div class="modal-dialog modal-full">
        <div class="modal-content ">
            <div class="modal-header">
                <h4 class="modal-title">Cash Flow Projection</h4>
                <button type="button" class="close" data-dismiss="modal"><span>×</span></button>
            </div>
            <div class="modal-body p-0">
                <iframe name="" class="" frameborder="0" style="width: 100%;height:100%" src="/GMS3/Finance/CashFlow/CashFlowProjectionForWeekPopUp.aspx"></iframe>
            </div>
        </div>
    </div>
</div>
    		   
<div class="TABCOMMAND">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uctrl:MsgPanel ID="PageMsgPanel" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>     

    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        //<!--
        var GB_ROOT_DIR = "/GMS3/greybox/";


        //-->
</script>

<script type="text/javascript">
    $(document).ready(function () {
        $(".cash-flow-menu").addClass("active expand");
        $(".sub-cash-flow").addClass("active");
    });

    $('#formModal').on('show.bs.modal', function () {
        $('.modal-body').css('height', $(window).height() * 0.8);
    });
</script>
</asp:Content>
