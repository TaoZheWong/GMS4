<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveMR.aspx.cs" Inherits="GMSWeb.ApproveMR" %>
<%@ Register TagPrefix="uctrl" TagName="Calendar" Src="CustomCtrl/CalendarControl.ascx" %>
<%@ Register TagPrefix="uctrl" TagName="MsgPanel" Src="~/CustomCtrl/MessagePanelControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Material Requisition</title> 

<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/jQueryUI-1.11.4/jquery-ui.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/jQueryUI-1.11.4/jquery-ui.min.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/css/bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/css/bootstrap-datetimepicker.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/css/fileinput.min.css" media="screen"/>

<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/DataTables-1.10.12/css/dataTables.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/AutoFill-2.1.2/css/autoFill.bootstrap.min.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/css/buttons.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/ColReorder-1.3.2/css/colReorder.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/FixedColumns-3.2.2/css/fixedColumns.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/FixedHeader-3.1.2/css/fixedHeader.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/KeyTable-2.1.2/css/keyTable.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Responsive-2.1.0/css/responsive.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/RowReorder-1.1.2/css/rowReorder.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Scroller-1.4.2/css/scroller.bootstrap.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Select-1.2.0/css/select.bootstrap.css" media="screen"/>

<script type="text/javascript">
        var ua = window.navigator.userAgent;       
        var msie = ua.indexOf("MSIE ");        
        if (msie > 0) // If Internet Explorer, return version number
        {
            alert("You are using an incompatible web browse. Please use Google Chrome.");            
        }
</script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/jQuery-2.2.3/jquery-2.2.3.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/jQueryUI-1.11.4/jquery-ui.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/bootstrap.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/moment.min.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/bootstrap-datetimepicker.min.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/plugins/canvas-to-blob.min.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/plugins/sortable.min.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/plugins/purify.min.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/fileinput.min.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/plugins/canvas-to-blob.min.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Bootstrap-3.3.6/js/validator.js"></script>

<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/JSZip-2.5.0/jszip.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/pdfmake-0.1.18/build/pdfmake.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/pdfmake-0.1.18/build/vfs_fonts.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/DataTables-1.10.12/js/jquery.dataTables.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/DataTables-1.10.12/js/dataTables.bootstrap.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/AutoFill-2.1.2/js/dataTables.autoFill.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/AutoFill-2.1.2/js/autoFill.bootstrap.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/js/dataTables.buttons.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/js/buttons.bootstrap.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/js/buttons.colVis.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/js/buttons.flash.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/js/buttons.html5.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Buttons-1.2.1/js/buttons.print.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/ColReorder-1.3.2/js/dataTables.colReorder.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/FixedColumns-3.2.2/js/dataTables.fixedColumns.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/FixedHeader-3.1.2/js/dataTables.fixedHeader.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/KeyTable-2.1.2/js/dataTables.keyTable.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Responsive-2.1.0/js/dataTables.responsive.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/RowReorder-1.1.2/js/dataTables.rowReorder.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Scroller-1.4.2/js/dataTables.scroller.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/DataTables-Bootstrap/Select-1.2.0/js/dataTables.select.js"></script>
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/jqueryui/Common.js"></script>    
<script type="text/javascript" src="https://gms.leedenlimited.com/GMS4/ApproveMR.js"></script>
</head>
<body style="background-image:none; text-align: left" >
<form id="form1">
<div class="panel panel-custom margin-bottom-40">
	    <div class="panel-heading">
		<h1 class="panel-title">Material Requisition</h1>
		</div>
	    <div id="FormSearch" class="panel-body">
		    <div id="content">
		    <input type="hidden" id="hidUserID" runat="server" value="" />
		    <input type='hidden' id='hidUserRole' runat="server" name='hidUserRole' value="" />
		    <input type="hidden" id="hidCurrentLink" runat="server" value="" />
		    <input type='hidden' id='hidMainPurchaserUserID' name='hidMainPurchaserUserID' runat="server" value="" />		
		    <input type='hidden' id='hidMRScheme' name='hidMRScheme'>
		    <input type='hidden' id='hidAllowChanges' name='hidAllowChanges' value="disabled">  
		    <input type='hidden' id='hidViewPurchaseInfo' name='hidViewPurchaseInfo'>
		    <input type='hidden' id='hidApproverUserID' name='hidApproverUserID'>
		    <input type='hidden' id='hidIsMainPurchaser' name='hidIsMainPurchaser'>
		    <input type='hidden' id='hidMRRole' name='hidMRRole'>
		    <input type='hidden' id='hiddenPageName' name='hiddenPageName' value="ApproveMR.aspx" runat="server" />		    
		    <input type='hidden' id='hidCustomerCode' name='hidCustomerCode'>
		    <div id="MRHeader">     
		    <div class="row">
		        <div class="col-sm-3">
                <div class="form-group">
                  <label for="status">Source:</label>
                  <select class="form-control" id="source" name="source" disabled>
                    <option value="Local">Local</option>
                    <option value="Overseas">Overseas</option>
                  </select>
                </div>
                </div>
                <div class="col-sm-3">
                <div class="form-group">
                  <label for="freight-mode">* Ship Via:</label>
                  <select class="form-control" id="freight-mode" name="freight-mode" disabled>
                    <option value="">-</option>
                    <option value="Air">Air</option>
                    <option value="Sea">Sea</option>
                    <option value="Courier">Courier</option>
                    <option value="Land">Land</option>
                  </select>
                </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                    <label for="mr-date">MR Date:</label>
                    <div class='input-group date' id='mrdate'>
                    <input type='text' class="form-control" id='mr-date' name="mr-date" disabled>
                    <span class="input-group-addon">
                    <span class="glyphicon glyphicon-calendar">
                    </span>
                    </span>
                    </div>
                    </div> 
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                      <label for="mr-no">MR No:</label>
                      <input type="text" class="form-control" id="mr-no" disabled>
                    </div>
                </div>
		    </div>
		    
		    <div class="row">
		        <div class="col-sm-12">
		        <div class="form-group">
                <label for="Checkboxes">* Intended Use:</label>
                <div class="col-sm-12 columns table-bordered" style="padding: 10px 5px 10px 10px;"> 
                    <label class="checkbox-inline" for="Stock">
                      <input type="checkbox" name="Stock" id="Stock" value="Stock" disabled>
                      Stock
                    </label> 
                    <label class="checkbox-inline" for="Sales">
                      <input type="checkbox" name="Sales" id="Sales" value="Sales" disabled>
                      Sales
                    </label> 
                    <label class="checkbox-inline" for="Repair-Maintenance">
                      <input type="checkbox" name="Repair-Maintenance" id="Repair-Maintenance" value="Repair & Maintenance" disabled>
                      Repair & Maintenance
                    </label>
                    <label class="checkbox-inline" for="Sample">
                      <input type="checkbox" name="Sample" id="Sample" value="Sample" disabled>
                      Sample
                    </label> 
                    <label class="checkbox-inline" for="Workshop / Others">
                      <input type="checkbox" name="Workshop" id="Workshop" value="Workshop" disabled>
                      Workshop / Others
                    </label>
                    <label class="checkbox-inline" for="Project">
                      <input type="checkbox" name="Project" id="Project" value="Project" disabled>
                      Project
                    </label>
                    <label class="checkbox-inline" for="Raw Material">
                      <input type="checkbox" name="Raw-Material" id="Raw-Material" value="Raw Material" disabled>
                      Raw Material
                    </label>
                    <label class="checkbox-inline" for="Staff Welfare">
                      <input type="checkbox" name="Staff-Welfare" id="Staff-Welfare" value="Staff Welfare" disabled>
                      Staff Welfare
                    </label> 
                    <span class="additional-info-wrap">
                        <label class="checkbox-inline" for="Asset">
                          <input type="checkbox" name="Asset" id="Asset" value="Asset" disabled>
                          Asset, GL Code
                        </label>
                        <div class="additional-info hide">
                              <input type="text" id="GLCode" name="GLCode" placeholder="GL Code" class="form-control" disabled>
                        </div>
                    </span>
                </div>
            </div>
            </div>
            </div>
            
            <div class="row">
                <div class="col-sm-3">
                <div class="form-group">
                  <label for="mr-no">Budget Code:</label>
                  <input type="text" class="form-control" id="budget-code" maxlength="50" name="budget-code" disabled>
                </div>
                </div>
                <div class="col-sm-3">
                   <div class="form-group">
                      <label for="ref-no">Ref No:</label>
                      <input type="text" class="form-control" id="ref-no" maxlength="50" name="ref-no" disabled>
                    </div>
                </div>                
                <div class="col-sm-3">
                    <div class="form-group">
                      <label for="project-no">Project No:</label>
                      <input type="text" class="form-control" id="project-no" maxlength="50" name="project-no" disabled>
                    </div>
                </div>
                <div class="col-sm-3" id="displayStatus">
                <div class="form-group">
                  <label for="status">Status:</label>
                  <input type="text" class="form-control" id="status" name="status" onmousedown="GetMRSpecialStatus(this);" disabled>
                  <input type="hidden" class="form-control" id="status-id" name="status-id">
                </div>
                </div>
                
            </div>
		    
		    <div class="row">
                <div class="col-sm-3">
                <div class="form-group">
                  <label for="approver1-name">Approver (1):</label>
                  <input type="hidden" id="approver1-id">
                  <input type="text" class="form-control" id="approver1-name" onmousedown="GetApprover1(this,'approver1-id','approver2-id','approver2-name','approver3-id','approver3-name','approver4-id','approver4-name');" disabled>
                </div>
                </div>
                <div class="col-sm-3">
                   <div class="form-group">
                      <label for="approver2-name">Approver (2):</label>
                      <input type="hidden" id="approver2-id">
                      <input type="text" class="form-control" id="approver2-name" onmousedown="GetApprover2(this,'approver2-id','approver3-id', 'approver3-name');" disabled>
                    </div>
                </div>                
                <div class="col-sm-3">
                    <div class="form-group">
                      <label for="approver3-name">Approver (3):</label>
                      <input type="hidden" id="approver3-id">
                      <input type="text" class="form-control" id="approver3-name" onmousedown="GetApprover3(this,'approver3-id', 'approver4-id','approver4-name');" disabled>
                    </div>
                </div>
                <div class="col-sm-3">
                <div class="form-group">
                  <label for="approver1-name">Approver (4):</label>
                  <input type="hidden" id="approver4-id">
                  <input type="text" class="form-control" id="approver4-name" onmousedown="GetApprover4(this,'approver4-id');" disabled>
                </div>
                </div>
            </div>
		    
		    <div class="row">
		        <div class="col-sm-3">
                    <div class="form-group">
                      <label for="requestor-name">Requestor:</label>
                      <input type="hidden" id="requestor-id">
                      <input type="text" class="form-control" id="requestor-name" onmousedown="GetRequestor(this);" disabled>
                    </div>
                </div>
		        <div class="col-sm-3">
                    <div class="form-group">
                      <label for="purchaser">Purchaser:</label>
                      <input type="text" class="form-control" id="purchaser" onmousedown="GetPurchaser(this);" disabled>
                    </div>
                </div>
                <div class="col-sm-3">                
                                <div class="form-group">
    					        <span class="additional-info-wrap">
                                    <label class="checkbox-inline" for="Console">
                                      <input type="checkbox" name="Console" id="Console" value="Yes" disabled>
                                      Console: 
                                    </label>
                                    <div class="additional-info hide">
                                          <div class='input-group date' id='consoledate'>
                                            <input type='text' class="form-control" id='console-date' disabled>
                                            <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar">
                                            </span>
                                            </span>
                                          </div>
                                    </div>
                                </span>
                                </div>
    			</div>
    			<div class="col-sm-3">
    			    <div class="form-group">
				        <span class="additional-info-wrap">
                            <label class="checkbox-inline" for="mov">
                              <input type="checkbox" name="ismov" id="ismov" value="Yes" disabled>
                              MOV: 
                            </label>        
				            <div class="additional-info hide">
				                <input type="text" name="mov" class="form-control" id="mov" disabled>
                            </div>
                        </span>
                        </div>
                    </div>            
		        </div>
		   </div>
		    <p></p>	
			<div class="tab-v2">
			        <ul class="nav nav-tabs"> 
                    <li class="active"><a href="#salestab" data-toggle="tab">Confirmed Sales</a></li>
                    <li><a href="#vendor" data-toggle="tab">Vendor</a></li>
                    <li id="productTab"><a href="#product" data-toggle="tab">Product</a></li>
                    <li><a href="#delivery" data-toggle="tab">Delivery</a></li>
                    <li><a href="#attachment" data-toggle="tab">Attachment</a></li>
                    <li><a href="#information" data-toggle="tab">More Information</a></li>
			        </ul>
				    <div class="tab-content">
					    
    					<div class="tab-pane fade in" id="information">
    					    
    					    <div class="row">
		                        <div class="col-sm-12">
                                <div class="form-group">
                                  <label for="requestor-remarks">Requestor Remarks:</label>
                                  <textarea class="form-control" rows="5" id="requestor-remarks" name="requestor-remarks" disabled></textarea>
                                </div>
                                </div>
		                    </div>
		                    <div class="row">
		                        <div class="col-sm-12">
                                <div class="form-group">
                                  <label for="purchase-reasons">Other Purchase Reasons:</label>
                                  <textarea class="form-control" rows="5" id="purchase-reasons" name="purchase-reasons" disabled></textarea>
                                </div>
                                </div>
		                    </div> 
		                    <div class="row">
		                        <div class="col-sm-12">
                                <div class="form-group">
                                  <label for="cancelled-reasons">Cancelled Reasons:</label>
                                  <textarea class="form-control" rows="5" id="cancelled-reason" disabled></textarea>
                                </div>
                                </div>
		                    </div>
		                    
    					</div>
    					<div class="tab-pane fade in active" id="salestab">
					        <table id="tblSales" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%"></table>
    					</div>
					    <div class="tab-pane fade in" id="vendor" style="display:none">
					        <div class="form-group">
                              <label for="vendor-remarks">Vendor Remarks:</label>
                              <textarea class="form-control" rows="5" id="vendor-remarks" name="vendor-remarks" disabled></textarea>
                            </div>    
					        <table id="tblVendor" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%"></table>
					    </div>
					    <div class="tab-pane fade in" id="product">
					    
					        <div class="bs-callout bs-callout-info">
					         <div class="row">
		                        <div class="col-sm-4">
		                           <b>S.Curr. = Selling Currency</b>
		                        </div>
		                        <div class="col-sm-4">
		                           <b>P.Curr. = Purchase Currency</b>
		                        </div>
		                        <div class="col-sm-4">
		                           <b>Total P.P = Total Purchase Price</b>
		                        </div>
					         </div>
					         <div class="row">
		                        <div class="col-sm-4">
		                           <b>S.Price = Selling Price</b>
		                        </div>
		                        <div class="col-sm-4">
		                           <b>P.Price = Purchase Price</b>
		                        </div>
		                        <div class="col-sm-4">
		                         
		                        </div>
					         </div>
					        </div>			        
					        
					        
					      
					        <table id="tblProduct" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%"></table>
					        <br />					        
					        <div class="container" style="width: 50%; float:right; clear: both; display: inline-block;">
                             
                            </div>
					    </div>
					    <div class="tab-pane fade in" id="delivery" style="display:none">
					        <div class="form-group">
                              <label for="delivery-remarks">Delivery Remarks:</label>
                              <textarea class="form-control" rows="5" id="delivery-remarks"></textarea>
                            </div>  
					        <table id="tblDelivery" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%"></table>
					    </div>
    					<div class="tab-pane fade in" id="attachment">
    					    <div id="drag" style="display:none">
    					    <input id="input-706" name="kartik-input-706[]" type="file" multiple=true class="file-loading">
                            
                            <script>
                                var $el2 = $("#input-706");
                                var CoyID = getCoyID();
                                 
                                // custom footer template for the scenario
                                // the custom tags are in braces
                                var footerTemplate = '<div class="file-thumbnail-footer">\n' +
                                '   <div style="margin:5px 0">\n' +
                                '       <input class="kv-input kv-new form-control input-sm text-center {TAG_CSS_NEW}" value="{caption}" placeholder="Enter caption...">\n' +
                                '       <input class="kv-input kv-init form-control input-sm text-center {TAG_CSS_INIT}" value="{TAG_VALUE}" placeholder="Enter caption...">\n' +    
                                '       <select class="kv-input kv-category form-control input-sm text-center {TAG_CSS_NEW}">' +
                                '           <option value="PO">PO</option>' +
                                '           <option value="Service Job Sheet">Service Job Sheet</option>' +
                                '           <option value="End User Form">End User Form</option>' +
                                '           <option value="Others">Others</option>' +
                                '       </select>\n' +
                                '       <input class="kv-input kv-coyid form-control input-sm text-center {TAG_CSS_NEW}" value="'+ CoyID +'" placeholder="Enter caption..." readonly>\n' +    
                                '   </div>\n' +
                                '   {size}\n' +
                                '   {actions}\n' +
                                '</div>';
                                 
                                $el2.fileinput({
                                    uploadUrl: 'Products/Products/FileUploadHandler.ashx',
                                    uploadAsync: true,
                                    maxFileCount: 5,
                                    overwriteInitial: false,
                                    layoutTemplates: {footer: footerTemplate, size: '<samp><small>({sizeText})</small></samp>'},
                                    previewThumbTags: {
                                        '{TAG_VALUE}': '',        // no value
                                        '{TAG_CSS_NEW}': '',      // new thumbnail input
                                        '{TAG_CSS_INIT}': 'hide'  // hide the initial input                                       
                                    },
                                    showBrowse: false,
                                    showPreview: true,
                                    browseOnZoneClick: true,
                                    uploadExtraData: function() {  // callback example
                                        var out = {}, key, i = 0;
                                        $('.kv-input:visible').each(function() {
                                            $el = $(this);
                                            if($el.hasClass('kv-new'))
                                                key = 'new_' + i;
                                            else if($el.hasClass('kv-category'))
                                                key = 'category_' + i;
                                            else if($el.hasClass('kv-init'))
                                                key = 'init_' + i;
                                            else if($el.hasClass('kv-coyid'))
                                                key = 'coyid_' + i;
                                            //key = $el.hasClass('kv-new') ? 'new_' + i : 'init_' + i;
                                            out[key] = $el.val();
                                            i++;
                                        });
                                        return out;
                                    }
                                });
                                
                                $('#input-706').on('fileuploaded', function(event, data, previewId, index, jqXHR) {
                                    var temp = data["response"];
                                                                       
                                    var tblAttachment = JSON.parse(localStorage.getItem('DataTables_tblAttachment_'+$('#mr-no').val())); 
                                    if(tblAttachment == null)
                                        tblAttachment = [temp[0]];
                                    else
                                        tblAttachment.push(temp[0]);
                                    localStorage.setItem( 'DataTables_tblAttachment_'+$('#mr-no').val(), JSON.stringify(tblAttachment)); 
                                    if($('#mr-no').val() != '') 
                                        SaveRecord('tblAttachment', 'SaveAttachment', 'AttachmentInfo');    
                                    Attachment();
                                    //alert(JSON.stringify(temp));
                                    
                                });
                                </script>
                            </div>
                            <p></p>
    					    <table id="tblAttachment" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%"></table>
					    </div>
				    </div>
			</div>
			<label for="Created"></label><br />
			<label for="Modified"></label>
			<input type="hidden" id="CreatedBy-id">
			<div class="row" id="DivTotal" style="display:none">
			    <table class="table-condensed" width="50%" align="right">
                    <tbody>
                      <tr>                                    
                        <td>Sub Total</td>
                        <td>:</td>
                        <td></td>
                       <td><input type="text" class="form-control" id="SubTotal" name="SubTotal" readonly></td>
                      </tr>
                      <tr>                                   
                        <td>Discount</td>
                        <td>:</td>
                        <td></td>
                        <td><input type="text" class="form-control" id="Discount" name="Discount" onkeyup="ReCalculate();"></td>
                      </tr>
                      <tr>                                    
                        <td>Tax Type <input type="text" id="TaxTypeName" name="TaxTypeName" onmousedown="GetTaxType(this);" readonly> 
                        <input type="hidden" id="TaxRate" name="TaxRate">
                        <input type="hidden" id="TaxTypeID" name="TaxTypeID">
                        </td>
                        <td>:</td>
                        <td></td>
                        <td><input type="text" class="form-control" id="TaxAmount" name="TaxAmount" readonly></td>
                      </tr>
                      <tr>                                   
                        <td>Grand Total</td>
                        <td>:</td>
                        <td></td>
                        <td><input type="text" class="form-control" id="GrandTotal" name="GrandTotal" readonly></td>
                      </tr>
                    </tbody>
              </table>
			</div>
			            
			
			<p></p>
			<table id="tblRouting" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%"></table>
			
		<div class="row" id="displayButton">		    
		    <div class="col-sm-12" align="right">
		    <p>
		        <div class="btn-group">
                  <div class="btn-group" id="PrintReport">
                    <button type="button" class="btn btn-default btn-sm active dropdown-toggle" data-toggle="dropdown" id="Print" style="display:none">
                    Print <span class="caret"></span></button>
                    <ul class="dropdown-menu" role="menu">
                      <li><a href="#" id="PrintMR">Material Requisition</a></li>
                      <li id="li-PO"><a href="#" id="PrintPO">PO</a></li>
                    </ul>
                  </div>
                </div>                
		      <a href="#" class="btn btn-warning btn-sm btn-primary" id="ConfirmVendor" style="display:none">Confirm Vendor</a> 
		      <a href="#" class="btn btn-info btn-sm active" id="SubmitForApproval" style="display:none">SubmitForApproval</a>  
		      <a href="#" class="btn btn-success btn-sm active" id="Approve" style="display:none">Approve</a> 
		      <a href="#" class="btn btn-danger btn-sm active" id="Reject" style="display:none">Reject</a>
		      <a href="#" class="btn btn-primary btn-sm active" id="Duplicate" style="display:none">Duplicate</a> 
		      <a href="#" class="btn btn-warning btn-sm active" id="Cancel" style="display:none">Cancel</a> 
		      <a href="#" class="btn btn-danger btn-sm active" id="UndoCancel" style="display:none">Undo Cancellation</a>
              <a href="#" class="btn btn-sm btn-primary" id="Save" style="display:none">Save</a>
            </p>
        </div>
        </div>  		
        <!-- Modal Confirmed Sales -->
        <div class="modal fade" id="ModalConfirmSales" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
            <div class="modal-dialog">
                <div class="modal-content">
                    <!-- Modal Header -->
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">
                               <span aria-hidden="true">&times;</span>
                               <span class="sr-only">Close</span>
                        </button>
                        <h4 class="modal-title" id="H4_1">Confirmed Sales</h4>
                    </div>                
                    <!-- Modal Body -->
                    <div class="modal-body">
                        <form role="form">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div id="DivCustomerCode" class="form-group has-feedback" >
                                        <label for="txtCustomerCode">Customer Code</label>
                                        <input type="text" class="form-control" id="CustomerAccountCode" name='CustomerAccountCode' maxlength="12" onkeypress="GetAccount(this,'tblSales','C');" required />
                                        <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                                        <div class="help-block with-errors"></div>
                                    </div>
                                </div>                            
                            </div>
                            <div class="row">
                            <div class="col-sm-12">
                                  <div id="DivCustomerName" class="form-group has-feedback">
                                    <label for="txtCustomerName">Customer Name</label>
                                      <input type="text" class="form-control" id="CustomerAccountName" name='CustomerAccountName' maxlength="100" readonly />  
                                      <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                                      <div class="help-block with-errors"></div>   
                                  </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                  <div id="DivSONo" class="form-group has-feedback">
                                    <label for="lblSONo">Customer PO No.</label>
                                      <input type="text" class="form-control" id="SONo" placeholder="" name='SONo' maxlength="50" required/>
                                      <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                                      <div class="help-block with-errors"></div>  
                                  </div>
                                </div>                            
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                  <div id="DivSODate" class="form-group has-feedback">
                                    <label for="lblSupplierName">Customer PO Date</label>
                                      <div class='input-group date' id='SO-Date'>
                                        <input type='text' class="form-control" id='SODate' name='SODate' required/>
                                        <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                                        <div class="help-block with-errors"></div> 
                                      </div>
                                  </div>
                                </div>
                                <div class="col-sm-6">
                                  <div id="DivRequiredDate" class="form-group has-feedback">
                                    <label for="lblRequiredDate">Required Date</label>
                                      <div class='input-group date' id='required-date'>
                                        <input type='text' class="form-control" id='RequiredDate' name='RequiredDate' required/>
                                        <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                                        <div class="help-block with-errors"></div> 
                                      </div>
                                  </div>
                                </div>                            
                            </div>
                        </form>
                    </div>                
                    <!-- Modal Footer -->
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" id="save-confirmedSales" name="Edit">Save</button>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- ModalVendor -->
        <div class="modal fade" id="ModalVendor" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
            <div class="modal-dialog">
                <div class="modal-content">
                    <!-- Modal Header -->
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">
                               <span aria-hidden="true">&times;</span>
                               <span class="sr-only">Close</span>
                        </button>
                        <h4 class="modal-title" id="H4_2">Vendor</h4>
                    </div>                
                    <!-- Modal Body -->
                    <div class="modal-body">
                        <form role="form">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <label for="lblVendorName">Vendor Name</label>
                                        <input type="text" class="form-control" id="VendorName" name='VendorName' maxlength="50" onkeypress="GetAccount(this,'tblVendor','S');" required/>
                                    </div>
                                </div>                            
                            </div>
                            <div class="row">
                            <div class="col-sm-12">
                                  <div class="form-group">
                                    <label for="lblVendorContact">Vendor Contact</label>
                                      <input type="text" class="form-control" id="VendorContact" placeholder="" name='VendorContact' maxlength="100" required/>
                                  </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                  <div class="form-group">
                                    <label for="lblVendorTel">Vendor Tel.</label>
                                      <input type="text" class="form-control" id="VendorTel" placeholder="" name='VendorTel' maxlength="50" required/>
                                  </div>
                                </div>                            
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                  <div class="form-group">
                                    <label for="lblVendorFax">Vendor Fax</label>
                                      <input type="text" class="form-control" id="VendorFax" placeholder="" name='VendorFax' maxlength="50"/>
                                  </div>
                                </div>
                            </div>                       
                            <div class="row">
                                <div class="col-sm-12">
                                  <div class="form-group">
                                    <label for="lblVendorEmail">Vendor Email</label>
                                      <input type="text" class="form-control" id="VendorEmail" placeholder="" name='VendorEmail' maxlength="100" required />
                                  </div>
                                </div>                            
                            </div>
                        </form>
                    </div>                
                    <!-- Modal Footer -->
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" id="save-vendor" name="Edit" data-dismiss="modal">Save</button>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- ModalDelivery -->
        <div class="modal fade" id="ModalDelivery" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
            <div class="modal-dialog">
                <div class="modal-content">
                    <!-- Modal Header -->
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">
                               <span aria-hidden="true">&times;</span>
                               <span class="sr-only">Close</span>
                        </button>
                        <h4 class="modal-title" id="H4_3">Delivery</h4>
                    </div>                
                    <!-- Modal Body -->
                    <div class="modal-body">
                        <form role="form">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <label for="lblPONo">PO No</label>
                                        <input type="text" class="form-control" id="PONo" name='PONo' maxlength="20" Required/>
                                        <input type="hidden" class="form-control" id="Hidden1" name='Purchaser'/>
                                        <input type="hidden" class="form-control" id="PODate" name='PODate'/>
                                    </div>
                                </div>                            
                            </div>                            
                            <div class="row">
                                <div class="col-sm-12">
                                  <div class="form-group">
                                    <label for="lblCRDDate">CRD Date</label>
                                      <div class='input-group date' id='crd-date'>
                                        <input type='text' class="form-control" id='CRD' name='CRD'/>
                                        <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar">
                                        </span>
                                        </span>
                                      </div>
                                  </div>
                                </div>
                            </div>                       
                            <div class="row">
                                <div class="col-sm-12">
                                  <div class="form-group">
                                    <label for="lblETDDate">ETD Date</label>
                                      <div class='input-group date' id='etd-date'>
                                        <input type='text' class="form-control" id='ETD' name='ETD'/>
                                        <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar">
                                        </span>
                                        </span>
                                      </div>
                                  </div>
                                </div>                            
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                  <div class="form-group">
                                    <label for="lblETADate">ETA Date</label>
                                      <div class='input-group date' id='eta-date'>
                                        <input type='text' class="form-control" id='ETA' name='ETA'/>
                                        <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar">
                                        </span>
                                        </span>
                                      </div>
                                  </div>
                                </div>                            
                            </div>
                        </form>
                    </div>                
                    <!-- Modal Footer -->
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" id="save-delivery" name="Edit" data-dismiss="modal">Save</button>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- ModalAttachment -->
        <div class="modal fade" id="ModalAttachment" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <!-- Modal Header -->
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">
                               <span aria-hidden="true">&times;</span>
                               <span class="sr-only">Close</span>
                        </button>
                        <h4 class="modal-title" id="H4_4">Attachment</h4>
                    </div>                
                    <!-- Modal Body -->
                    <div class="modal-body">
                        <form role="form">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <label for="lblFileDisplayName">Display Name</label>
                                        <input type="text" class="form-control" id="FileDisplayName" name='FileDisplayName' maxlength="100" Required/>
                                    </div>
                                </div>                            
                            </div>
                            <div class="row">
                            <div class="col-sm-12">
                                  <div class="form-group">
                                    <label for="lblFileName">File Name</label>
                                      <input type="text" class="form-control" id="FileName" placeholder="" name='FileName' readonly/>
                                  </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                  <div class="form-group">
                                    <label for="lblDocumentCategory">Category</label>
                                      <input type="text" class="form-control" id="DocumentCategory" placeholder="" name='DocumentCategory' onmousedown="GetDocumentCategory(this);" maxlength="100" readonly/>
                                  </div>
                                </div>                            
                            </div>                            
                        </form>
                    </div>                
                    <!-- Modal Footer -->
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" id="save-attachment" name="Edit" data-dismiss="modal">Save</button>
                    </div>
                </div>
            </div>
        </div>
	
	    <!-- Modal Product -->
        <div class="modal fade" id="ModalProduct" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
            <div class="modal-dialog">
                <div class="modal-content">
                    <!-- Modal Header -->
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">
                               <span aria-hidden="true">&times;</span>
                               <span class="sr-only">Close</span>
                        </button>
                        <h4 class="modal-title" id="H4_5">Product</h4>
                    </div>                
                    <!-- Modal Body -->
                    <div class="modal-body">
                        <form role="form">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div id="DivProdCode" class="form-group has-feedback" >
                                        <label for="lblProdCode">ProdCode / ProdName (min. 3 char)</label>
                                        <input type="text" class="form-control" id="ProdCode" name='ProdCode' tablename="tblProduct" maxlength="50" required/>
                                        <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                                        <div class="help-block with-errors"></div>
                                    </div>
                                </div> 
                                <div class="col-sm-6">
                                  <div class="form-group">
                                    <label for="lblNewProdCode">New Product Code</label>
                                      <input type="text" class="form-control" id="NewProdCode" placeholder="" name='NewProdCode' maxlength="11" />
                                  </div>
                                </div>                            
                            </div> 
                            <div class="row">
                            <div class="col-sm-12">
                                  <div class="form-group">
                                    <label for="lblProdName">Product Name</label>
                                      <input type="text" class="form-control" id="ProdName" placeholder="" name='ProdName' maxlength="50"/>
                                      <input type='hidden' class='form-control' id='ProductGroupCode' name='ProductGroupCode'>
                                      <input type='hidden' class='form-control' id='KeyToSplit' name='KeyToSplit'>
                                  </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                  <div class="form-group">
                                    <label for="lblUOM">UOM</label>
                                      <input type="text" class="form-control" id="UOM" placeholder="" name='UOM' onkeypress="GetUOM(this,'tblProduct');" maxlength="20" />
                                  </div>
                                </div> 
                                <div class="col-sm-6">
                                  <div class="form-group">
                                    <label for="lblConfirmedOrderQty">Sales Qty</label>
                                      <input type="text" class="form-control" id="ConfirmedOrderQty" placeholder="" name='ConfirmedOrderQty' pattern="^[0-9]+(\.[0-9]{1,2})?$" />
                                  </div>
                                </div>                            
                            </div> 
                            <div class="row">
                                <div class="col-sm-6">
                                  <div class="form-group">
                                    <label for="lblForStockingQty">Stk Qty</label>
                                      <input type="text" class="form-control" id="ForStockingQty" placeholder="" name='ForStockingQty' pattern="^[0-9]+(\.[0-9]{1,2})?$" />
                                  </div>
                                </div>
                                <div class="col-sm-6">
                                  <div id="DivOrderQty" class="form-group has-feedback">
                                    <label for="lblOrderQty">Order Qty</label>
                                      <input type="text" class="form-control" id="OrderQty" placeholder="" name='OrderQty' pattern="^[0-9]+(\.[0-9]{1,2})?$" onkeyup="CalculatePurchasePrice();" />
                                      <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                                      <div class="help-block with-errors"></div>
                                  </div>
                                </div>                            
                            </div> 
                            <div class="row">
                                <div class="col-sm-6">
                                  <div class="form-group">
                                    <label for="lblSellingCurrency">Selling Currency</label>
                                      <input type="text" class="form-control" id="SellingCurrency" placeholder="" name='SellingCurrency' onmousedown="GetCurrency(this);" maxlength="10" required readonly />
                                  </div>
                                </div> 
                                <div class="col-sm-6">
                                  <div class="form-group">
                                    <label for="lblUnitSellingPrice">Selling Price</label>&nbsp;<a data-toggle="popover" data-placement="left" data-trigger='hover' title='Stock Status' data-content='' data-html='true' onMouseOver="GetProductInfo1(this);"><span class="glyphicon glyphicon-info-sign"></span></a>
                                      <input type="text" class="form-control" id="UnitSellingPrice" placeholder="" name='UnitSellingPrice' pattern="(^(\+|\-)(0|([1-9][0-9]*))(\.[0-9]{1,4})?$)|(^(0{0,1}|([1-9][0-9]*))(\.[0-9]{1,4})?$)" />
                                  </div>
                                </div>                            
                            </div>  
                            <div class="row" id="PurchaseInfo" style="display:none">
                                <div class="col-sm-6">
                                  <div class="form-group">
                                    <label for="lblPurchaseCurrency">Purchase Currency</label>
                                      <input type="text" class="form-control" id="PurchaseCurrency" placeholder="" name='PurchaseCurrency' onmousedown="GetCurrency(this);" maxlength="10" required readonly />
                                  </div>
                                </div> 
                                <div class="col-sm-6">
                                  <div class="form-group">
                                    <label for="lblUnitPurchasePrice">Purchase Price</label>&nbsp;<a data-toggle="popover" data-placement="left" data-trigger='hover' title='Stock Status' data-content='' data-html='true' onMouseOver="GetProductInfo1(this);"><span class="glyphicon glyphicon-info-sign"></span></a>
                                      <input type="text" class="form-control" id="UnitPurchasePrice" placeholder="" name='UnitPurchasePrice' pattern="(^(\+|\-)(0|([1-9][0-9]*))(\.[0-9]{1,4})?$)|(^(0{0,1}|([1-9][0-9]*))(\.[0-9]{1,4})?$)" onkeyup="CalculatePurchasePrice();" />
                                  </div>
                                </div>                            
                            </div>
                            <div class="row" id="PurchaseInfoTotal" style="display:none"> 
                                <div class="col-sm-12">
                                  <div class="form-group">
                                    <label for="lblTotalPurchasePrice">Total Purchase Price</label>
                                      <input type="text" class="form-control" id="TotalPurchasePrice" placeholder="" name='TotalPurchasePrice' />
                                      <input type='hidden' class='form-control' id='PriceStatus' name='PriceStatus'>
                                  </div>
                                </div>                            
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                  <div class="form-group">
                                    <label for="lblApprover1">Approver (1)</label>
                                      <input type='hidden' id='Approver1ID' class='form-control' name='Approver1ID' value='0'>  
                                      <input type="text" class="form-control" id="Approver1" placeholder="" name='Approver1' onmousedown="GetApprover1(this,'Approver1ID','Approver2ID','Approver2', 'Approver3ID','Approver3', 'Approver4ID', 'Approver4');" readonly />
                                  </div>
                                </div> 
                                <div class="col-sm-6">
                                  <div class="form-group">
                                    <label for="lblApprover2">Approver (2)</label>
                                      <input type='hidden' id='Approver2ID' class='form-control' name='Approver2ID' value='0'>  
                                      <input type="text" class="form-control" id="Approver2" placeholder="" name='Approver2' onmousedown="GetApprover2(this,'Approver2ID', 'Approver3ID','Approver3');" readonly />
                                  </div>
                                </div>                            
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                  <div class="form-group">
                                    <label for="lblApprover2">Approver (3)</label>
                                      <input type='hidden' id='Approver3ID' class='form-control' name='Approver3ID' value='0'>  
                                      <input type="text" class="form-control" id="Approver3" placeholder="" name='Approver3' onmousedown="GetApprover3(this,'Approver3ID', 'Approver4ID', 'Approver4');" readonly />
                                  </div>
                                </div>
                                <div class="col-sm-6">
                                  <div class="form-group">
                                    <label for="lblApprover2">Approver (4)</label>
                                      <input type='hidden' id='Approver4ID' class='form-control' name='Approver4ID' value='0'>  
                                      <input type="text" class="form-control" id="Approver4" placeholder="" name='Approver4' onmousedown="GetApprover4(this,'Approver4ID');" readonly />
                                  </div>
                                </div>                            
                            </div> 
                            <div class="row">
                                <div class="col-sm-12">
                                  <div id="DivProductReason" class="form-group has-feedback" >
                                    <label for="lblReason">Reason</label>
                                      <textarea class="form-control" rows="5" id="ProductReason" name='ProductReason' maxlength="100"></textarea>
                                      <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                                      <div class="help-block with-errors"></div>
                                  </div>
                                </div>                            
                            </div>                           
                        </form>
                    </div>                
                    <!-- Modal Footer -->
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" id="save-product" name="Edit">Save</button>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="modal fade" id="ModalReason" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <!-- Modal Header -->
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">
                               <span aria-hidden="true">&times;</span>
                               <span class="sr-only">Close</span>
                        </button>
                        <h4 class="modal-title" id="H4_7">Reason</h4>
                    </div>  
                    <!-- Modal Body -->
                    <div class="modal-body">
                        <form role="form">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div id="DivReason" class="form-group has-feedback" >
                                        <textarea class="form-control" rows="5" id="Reason" name='Reason' maxlength="500" Required></textarea>
                                        <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                                        <div class="help-block with-errors"></div>
                                    </div>
                                </div>                            
                            </div>                     
                        </form>
                    </div>                
                    <!-- Modal Footer -->
                    <div class="modal-footer">                        
                        <button type="button" class="btn btn-primary" id="cancel-reject" name="Edit">Save</button>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="modal fade" id="ModalMessage" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    
                    <!-- Modal Body -->
                    <div class="modal-body">
                        <span id="message" name="message"></span>
                    </div> 
                    <!-- Modal Footer -->
                    <div class="modal-footer"> 
                        <button type="button" class="btn btn-primary" id="btnClose" name="Close">Close</button>
                    </div>
                </div>
            </div>
        </div> 
        
        <div class="modal fade" id="confirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
          <div class="modal-dialog">
                <div class="modal-content">
                    
                    <!-- Modal Body -->
                    <div class="modal-body">
                        Are you sure?
                    </div> 
                    <!-- Modal Footer -->
                    <div class="modal-footer"> 
                        <button type="button" data-dismiss="modal" class="btn btn-primary" id="delete">Delete</button>
                        <button type="button" data-dismiss="modal" class="btn">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
        
        </div>
       </div>	
    </div>
</form>
</body>
</html>