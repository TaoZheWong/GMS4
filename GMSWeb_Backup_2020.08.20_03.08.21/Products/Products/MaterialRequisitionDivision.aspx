<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="MaterialRequisitionDivision.aspx.cs" Inherits="GMSWeb.Products.Products.MaterialRequisitionDivision" %>
<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<ul class="breadcrumb pull-right">
    <li><a href="#">Product</a></li>
    <li class="active">Material Requisition Division</li>
</ul>
<h1 class="page-header">Material Requisition Division </h1>
<div class="panel panel-custom margin-bottom-40">
		<div id="FormSearch" class="panel-body">
		  <div class="btn-group">
              <asp:Button ID="btnGasDivision" Text="Gas" EnableViewState="False" runat="server" CssClass="btn btn-default" OnClick="btnGas_Click"></asp:Button>
              <asp:Button ID="btnWeldingDivision" Text="Welding" EnableViewState="False" runat="server" CssClass="btn btn-default" OnClick="btnWelding_Click"></asp:Button>
          </div>            
		</div>
</div>
</asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".products-menu").addClass("active expand");
            $(".sub-material-req").addClass("active");
        });
    </script>
    <script type="text/javascript" src="<%= Request.ApplicationPath %>/jqueryui/Common.js"></script>
    <script type="text/javascript" src="MaterialRequisitionSearch.js?v1=9"></script>
</asp:Content>
