<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="MaterialRequisitionSearch.aspx.cs" Inherits="GMSWeb.Products.Products.MaterialRequisitionSearch" Title="Material Requisition Search" %>
<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

        <ul class="breadcrumb pull-right">
            <li><a href="#">Products</a></li>
            <li class="active">Material Requisition Search</li>
        </ul>
        <h1 class="page-header">Material Requisition Search</h1>

		
		<input type="hidden" id="hidCoyID" runat="server" value="" />	
		<input type="hidden" id="hidUserID" runat="server" value="" />
		<input type='hidden' id='hidUserRole' runat="server" name='hidUserRole' value="" />
		<input type="hidden" id="hidCurrentLink" runat="server" value="" />
        <input type='hidden' id='hidMRScheme' name='hidMRScheme' runat="server" />

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
             <div class="panel-body">
                 <div class="m-t-20">
                     <div class="row">
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="date-from">Date From:</label>
                                 <div class='input-group date' id='datefrom'>
                                     <input type='text' class="form-control" id='date-from' />
                                     <span class="input-group-addon">
                                         <i class="ti-calendar"></i>
                                     </span>
                                 </div>
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="date-to">Date To:</label>
                                 <div class='input-group date' id='dateto'>
                                     <input type='text' class="form-control" id='date-to' />
                                     <span class="input-group-addon">
                                         <i class="ti-calendar"></i>
                                     </span>
                                 </div>
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="customer-code">Customer Code:</label>
                                 <input type="text" class="form-control" id="customer-code">
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                         <label class="control-label" for="customer-name">Customer Name:</label>
                             <input type="text" class="form-control" id="customer-name">
                     </div>
                     </div>
                     <div class="row">
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="product-code">Product Code:</label>
                                 <input type="text" class="form-control" id="product-code">
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="product-name">Product Name:</label>
                                 <input type="text" class="form-control" id="product-name">
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="product-group-code">Product Group Code:</label>
                                 <input type="text" class="form-control" id="product-group-code">
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                         <label class="control-label" for="product-group-name">Product Group Name:</label>
                             <input type="text" class="form-control" id="product-group-name">
                     </div>
                     </div>
                     <div class="row">
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="vendor">Vendor:</label>
                                 <input type="text" class="form-control" id="vendor">
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="po">PO:</label>
                                 <input type="text" class="form-control" id="po">
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="purchaser">Purchaser:</label>
                                 <input type="text" class="form-control" id="purchaser" onmousedown="GetPurchaser(this);">
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="mr-no">MR No:</label>
                                 <input type="text" class="form-control" id="mr-no">
                         </div>
                     </div>
                     <div class="row">
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="requestor">Requestor</label>
                                 <input type="text" class="form-control" id="requestor">
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="approver1-name">Product Manager:</label>
                                 <input type="hidden" id="approver1-id">
                                 <input type="text" class="form-control" id="approver1-name" onmousedown="GetApprover1(this,'approver1-id','','','','','','');">
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="status">Status:</label>
                                 <input type="text" class="form-control" id="status" onmousedown="GetMRStatus(this);">
                                 <input type="hidden" class="form-control" id="status-id" name="status-id">
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="mr-no">Budget Code:</label>
                                 <input type="text" class="form-control" id="budget-code" maxlength="50">
                         </div>
                     </div>
                     <div class="row">
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="ref-no">Ref No:</label>
                                 <input type="text" class="form-control" id="ref-no" maxlength="50">
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                             <label class="control-label" for="project-no">Project No:</label>
                                 <input type="text" class="form-control" id="project-no" maxlength="50">
                         </div>
                         <div class="form-group col-lg-3 col-md-6 col-sm-6">
                         <label class="control-label" for="requestor-remarks">Requestor Remarks:</label>
                             <input type="text" class="form-control" id="requestor-remarks">
                     </div>
                    </div>
                 </div>
             </div>
            <div class="panel-footer clearfix">
			    <a href="#" class="btn btn-primary pull-right" id="Search">Search</a>       
                <a href="#" class="btn btn-default m-r-10 pull-right " id="Add" style="display:none">Add</a>
            </div>
        </div>


        <div class="panel margin-bottom-40">
		    <div id="FormSearch" class="panel-body no-padding">
		            <div class="table-responsive">
		            <table id="tblSearch" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%"></table>
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
    <script type="text/javascript" src="MaterialRequisitionSearch.js?v1=<%=DateTime.Now.Millisecond %>"></script>
</asp:Content>


