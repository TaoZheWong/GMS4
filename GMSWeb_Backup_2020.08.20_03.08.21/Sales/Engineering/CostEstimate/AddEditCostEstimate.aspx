<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="AddEditCostEstimate.aspx.cs" Inherits="GMSWeb.Sales.Engineering.CostEstimate.AddEditCostEstimate" Title="Cost Estimate Detail" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

<style type="text/css">
    .ui-autocomplete {
        max-height: 150px;
        overflow-y: auto;
        /* prevent horizontal scrollbar */
        overflow-x: hidden;
    }
    /* IE 6 doesn't support max-height
    * we use height instead, but this forces the menu to always be this tall
    */
    * html .ui-autocomplete {
        height: 200px;
    }
    #resizable { width: 150px; height: 150px; padding: 0.5em; }
    #resizable h3 { text-align: center; margin: 0; }
    
</style>

<ul class="breadcrumb pull-right">
    <li><a href="#">Sales</a></li>
    <li class="active">Cost Estimate Requisition</li>
</ul>
<h1 class="page-header">Cost Estimate Requisition
</h1>
<div class="panel panel-primary" id="CEHeaderInfo">
		<div class="panel-heading">
			<h1 class="panel-title">
                <i class="ti-pencil"></i>
                Cost Estimate Requisition
			</h1>
		</div>
		
		<div class="panel-body">
            <div class="form-horizontal m-t-20">
                <input type="hidden" id="hidCoyID" runat="server" value="" />	
		        <input type="hidden" id="hidUserID" runat="server" value="" />
		        <input type="hidden" id="hidCurrentLink" runat="server" value="" />
                <!-- Row 1 -->
                <div class="row" id="Header-1">
                    <div class="col-lg-4 col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label text-left" for="txtCEID">C/E No:</label>
                            <div class="col-sm-6">
                                <input type="text" class="form-control" id="txtCEID" tabindex="1" name="CEID" readonly="readonly"/>
                            </div>
                        </div> 
                    </div>
                    
                    <div class="col-lg-4 col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label text-left" for="txtAccountCode"><sup><font color="red">*</font></sup>Customer Code
                                <a data-toggle="tooltip" class="tooltipLink" data-original-title="Temporary account code is D000000000">
                                    <span class="glyphicon glyphicon-info-sign"></span>
                                </a>:
                            </label>
                            <div class="col-sm-6">
                            <input type="text" class="form-control" id="txtAccountCode" name="AccountCode" tabindex="2" placeholder="e.g: D1LI720"/>
                                </div>
                        </div>
                    </div>
                    
                    <div class="col-lg-4 col-sm-6">    
                        <div class="form-group">
                            <label class="col-sm-6 control-label text-left" for="txtAccountName">Customer Name:</label>
                            <div class="col-sm-6">
                           <input type="text" class="form-control" id="txtAccountName" name="AccountName" tabindex="3" placeholder="e.g: Keppel" readonly="readonly" />
                            </div>
                        </div>    
                    </div>
                    
                    <div class="col-lg-4 col-sm-6">    
                        <div class="form-group">
                            <label class="col-sm-6 control-label text-left" for="txtCEStatusID">Status:</label>
                            <div class="col-sm-6">
                                <input type="text" class="form-control" id="txtCEStatusName" name="CEStatusName" tabindex="4" readonly="readonly"/>
                                <input type="hidden" class="form-control" id="txtCEStatusID" name="CECEStatusID"/>
                            </div>
                        </div>    
                    </div>
               
                    <div class="col-lg-4 col-sm-6">
                        <div class="form-group">  
                            <label class="col-sm-6 control-label text-left" for="txtSalesPersonID">Sales Person: </label>
                            <div class="col-sm-6">
                            <input type="text" class="form-control" id="txtSalesPersonName" name="SalesPersonName" tabindex="5" placeholder="e.g: John (Not compulsory)"/>
                            <input type="hidden" class="form-control" id="txtSalesPersonID" name="SalesPersonID" placeholder="e.g: John"/>
                            </div>
                        </div> 
                    </div>
                    <div class="col-lg-4 col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label text-left" for="txtEngineerID"><sup><font color="red">*</font></sup>Engineer: </label>
                            <div class="col-sm-6">
                            <input type="text" class="form-control" id="txtEngineerName" name="EngineerName"tabindex="6" placeholder="e.g: John"/>
                            <input type="hidden" class="form-control" id="txtEngineerID" name="EngineerID"/>
                            </div>
                        </div> 
                    </div>
                    <div class="col-lg-4 col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label text-left" for="txtIsBillable"><sup><font color="red">*</font></sup>Billable:</label>
                            <div class="col-sm-6">
                            <select type="text" class="form-control" id="txtIsBillable" tabindex="7" name="IsBillable">
                                <option value="False">No</option>
                                <option value="True">Yes</option>
                            </select>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4 col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label text-left" for="txtIsProgressiveClaim"><sup><font color="red">*</font></sup>Progressive Claim:</label>
                            <div class="col-sm-6">
                                <select class="form-control" id="txtIsProgressiveClaim" tabindex="8" name="IsProgressiveClaim">
                                    <option value="False">No</option>
                                    <option value="True">Yes</option>
                                </select>
                             </div>
                        </div>
                    </div>
         
                    <div class="col-lg-4 col-sm-6">
                        <div class="form-group">  
                            <label class="col-sm-6 control-label text-left" for="Revision">Revision: </label>
                            <div class="col-sm-6">
                                <select class="form-control" id="txtRevision" name="Revision" tabindex="9"></select>
                            </div>
                        </div> 
                    </div>
                    <div class="col-lg-4 col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-6 control-label text-left" for="txtEngineerID">Project No.: </label>
                            <div class="col-sm-6">
                                <input type="text" class="form-control" id="txtProjectNo" name="ProjectNo" tabindex="10" disabled="disabled"/>
                            </div>
                        </div> 
                    </div>
                </div>
            </div> 

           
        </div>
        <!-- Tabs -->
        <div class="tab-v2 panel-tab">
            <ul class="nav nav-tabs">
                <li class="active" id="t1"><a href="#tab1" data-toggle="tab">General</a></li>
                <li id="t2"><a href="#tab2" data-toggle="tab">List of Items</a></li>
            </ul>
	        <div class="tab-content form-horizontal" id="Info-1">
	            <!-- Child Tab 1 -->
		        <div class="tab-pane fade in active" id="tab1">
		                <!-- Row 1 -->
                        <div class="row"> 
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="txtCurrencyCode"><sup><font color="red">*</font></sup>Currency Code  
                                        <a data-toggle="tooltip" class="tooltipLink" data-original-title="Click on the textbox for the list of currency">
                                            <span class="glyphicon glyphicon-info-sign"></span>
                                        </a>:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtCurrencyCode" name="CurrencyCode" tabindex="9" readonly="readonly"/>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6">   
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="txtTotalCE">Total Cost Estimate:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtGrandTotal" name="TotalCE" placeholder="0.00" readonly="readonly" tabindex="10"/>
                                    </div>
                                </div> 
                            </div>
                            <div class="col-md-6 col-sm-6">   
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="txtTotalCE"><sup><font color="red">*</font></sup>Total Amount to be Quoted:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtTotalAmtQuoted" name="TotalAmtQuoted" placeholder="0.00" tabindex="11"/>
                                    </div>
                                </div> 
                            </div>
                       
                            <div class="col-md-6 col-sm-6">    
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="txtContractDateFrom"><sup><font color="red">*</font></sup>Contract Date From:</label>
                                    <div class="col-sm-6">
                                        <div class='input-group date_yyyy_mm_dd' id='datetimepicker1'>
                                            <input type='text' class="form-control" id="txtContractDateFrom" name="ContractDateFrom" placeholder="e.g: 2016-01-01" tabindex="13"/>
                                            <span class="input-group-addon">
                                                <i class="ti-calendar"></i>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="txtContractDateTo"><sup><font color="red">*</font></sup>Contract Date To:</label>    
                                    <div class="col-sm-6">
                                    <div class='input-group date_yyyy_mm_dd' id='datetimepicker2'>
                                        <input type='text' class="form-control" id="txtContractDateTo" name="ContractDateTo" placeholder="e.g: 2016-01-01"  tabindex="14"/>
                                        <span class="input-group-addon">
                                            <i class="ti-calendar"></i>
                                        </span>
                                    </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="txtCommencementDate">Commencement Date:</label>    
                                    <div class="col-sm-6">
                                        <div class='input-group date_yyyy_mm_dd'>
                                            <input type='text' class="form-control" id="txtCommencementDate" name="CommencementDate" placeholder="e.g: 2016-01-01" tabindex="15"/>
                                            <span class="input-group-addon">
                                                <i class="ti-calendar"></i>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                     
                            <div class="col-md-6 col-sm-6">    
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="txtCustomerPIC"><sup><font color="red">*</font></sup>Customer PIC:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtCustomerPIC" name="CustomerPIC" tabindex="16"/>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="txtTelNo">Tel No.:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtOfficePhone" name="OfficePhone" tabindex="17"/>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="txtFaxNo">Fax No.:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtFax" name="Fax" tabindex="18"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                      
                        <div class="row">
                            <div class="col-md-6 col-sm-6">    
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="txtBillingAddress"><sup><font color="red">*</font></sup>Billing Address:</label>
                                    <div class="col-sm-6">
                                        <div class='input-group' id='Div1'>
                                            <textarea class="form-control" rows="5" id="txtBillingAddress" name="BillingAddress" tabindex="19"></textarea>
                                            <span  id="BillingAddressDropDown" class="input-group-addon">
                                                <span class="glyphicon glyphicon-list-alt"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="txtOnsiteLocation">Onsite Location:</label>
                                    <div class="col-sm-6">
                                        <textarea class="form-control" rows="5" id="txtOnsiteLocation" name="OnsiteLocation" tabindex="20"></textarea>
                                    </div>
                                </div>
                            </div>
                       
                            <div class="col-sm-6">    
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="txtDescription"><sup><font color="red">*</font></sup>Description: </label>
                                    <div class="col-sm-6">
                                        <textarea class="form-control" rows="5" id="txtDescription" name="Description" data tabindex="21"></textarea>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="txtRemarks">Remarks:</label>
                                    <div class="col-sm-6">
                                        <textarea class="form-control" rows="5" id="txtRemarks" name="Remarks" tabindex="22"></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                       
		        </div>
		        <!-- End Child Tab 1 -->
		            
		        <!-- Child Tab 2 -->
		        <div class="tab-pane fade in" id="tab2">
		            <table id="tblCostEstimateDetail" class="table table-striped table-bordered dataTable no-footer" width="100%"></table>
		        </div>
		        <!-- End Child Tab 2 -->
	        </div>
        </div>
        <!-- End Tab v2 -->   
        <div class="panel-footer clearfix" id="displaybuttons">
                <button type="button" class="btn btn-default btn-block1 " id="btnPrintCE" title="Print Cost Estimation Form" style="display:none;"><span class="glyphicon glyphicon-print pull-left"></span>&nbsp; Print</button>
                <button type="button" class="btn btn-warning1 btn-block1  " id="btnConvertCE" name="Convert" title="Convert to Project & Quotation" data-toggle="modal" data-target="#ConvertCEForm" data-backdrop="static" data-keyboard="false" style="display:none;">Convert</button>
                <button type="button" class="btn btn-success btn-block1 " id="btnApproveCE"  name="Approve" title="Approve Cost Estimation Form" style="display:none;">Approve</button>
                <button type="button" class="btn btn-danger btn-block1 " id="btnRejectCE" name="Reject Reason" title="Reject Cost Estimation Form" style="display:none;" data-toggle="modal" data-target="#RejectCEForm" data-backdrop="static" data-keyboard="false">Reject</button>
                <button type="button" class="btn btn-default btn-block1 " id="btnCancelCE" name="Cancel Reason" title="Cancel Cost Estimation Form" data-toggle="modal" data-target="#CancelCEForm"  data-backdrop="static" data-keyboard="false" style="display:none;">Cancel</button>
                <button type="button" class="btn btn-default btn-block1 " id="btnAddToRevision" title="Add to Revision" style="display:none;" data-toggle="modal" data-target="#ReviseCEForm" data-backdrop="static" data-keyboard="false">Add To Revision</button>
                <button type="button" class="btn btn-primary btn-block1 " id="btnSubmitCEApproval" title="Submit Cost Estimation Form for Approval" data-toggle="modal" data-target="#SubmitCEForm"  data-backdrop="static" data-keyboard="false" style="display:none;">Submit for approval</button>
                <button type="button" class="btn btn-info btn-block1 pull-right" id="btnSubmitCEDraft" title="Save Cost Estimation Form as Draft" style="display:none;">Save</button>
        </div>  
    </div>
    <!-- Modal : Hidden Form Input-->
    <div class="modal fade" id="CancelCEForm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
        <div class="modal-dialog">
            
            <div class="modal-content" >
                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                           <span aria-hidden="true">&times;</span>
                           <span class="sr-only">Close</span>
                    </button>
                    <h4 class="modal-title" id="H4_1">Reason</h4>
                </div>
                
                <!-- Modal Body -->
                <div class="modal-body" id="CECancellationReason">
                   <form role="form">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group has-feedback">
                                <label class="col-sm-6 control-label text-left" for="MaterialName" class="control-label">Reason for cancellation:</label>
                                <input type="text" maxlength="100" class="form-control" id="CancelPurpose" name="CancelPurpose" placeholder="" required />
                                <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                                <div class="help-block with-errors"></div>
                              </div>
                            </div>
                        </div>
                   </form>
                </div>
                
                <!-- Modal Footer -->
                <div class="modal-footer">
                <div class="form-group">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-primary" id="btnSubmitCancellation" name="Cancel" data-dismiss="modal">Submit</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal : Hidden Form Input-->
    <div class="modal fade" id="RejectCEForm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
        <div class="modal-dialog">
            
            <div class="modal-content" >
                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                           <span aria-hidden="true">&times;</span>
                           <span class="sr-only">Close</span>
                    </button>
                    <h4 class="modal-title" id="H4_3">Reason</h4>
                </div>
                
                <!-- Modal Body -->
                <div class="modal-body" id="CECancellationReason">
                   <form role="form">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group has-feedback">
                                <label class="col-sm-6 control-label text-left" for="RejectPurpose" class="control-label">Reason for rejecting:</label>
                                <input type="text" maxlength="100" class="form-control" id="RejectPurpose" name="CancelPurpose" placeholder="" required />
                                <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                                <div class="help-block with-errors"></div>
                              </div>
                            </div>
                        </div>
                   </form>
                </div>
                
                <!-- Modal Footer -->
                <div class="modal-footer">
                <div class="form-group">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-primary" id="btnSubmitRejection" name="Reject" data-dismiss="modal">Submit</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal : Hidden Form Input-->
    <div class="modal fade" id="SubmitCEForm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
        <div class="modal-dialog">
            
            <div class="modal-content" >
                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                           <span aria-hidden="true">&times;</span>
                           <span class="sr-only">Close</span>
                    </button>
                    <h4 class="modal-title" id="H4_2">Confirmation</h4>
                </div>
                
                <!-- Modal Body -->
                <div class="modal-body" id="Div3">
                   <form role="form">
                        <div class="row">
                            <div class="col-sm-12">
                                <span>Confirm to submit form for approval?</span>
                            </div>
                        </div>
                   </form>
                </div>
                
                <!-- Modal Footer -->
                <div class="modal-footer">
                    <div class="form-group">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" id="btnSubmitforApproval1" name="Edit" data-dismiss="modal">Confirm</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    
    <!-- Modal : Hidden Form Input-->
    <div class="modal fade" id="ReviseCEForm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
        <div class="modal-dialog">
            
            <div class="modal-content" >
                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                           <span aria-hidden="true">&times;</span>
                           <span class="sr-only">Close</span>
                    </button>
                    <h4 class="modal-title" id="H4_7">Confirmation</h4>
                </div>
                
                <!-- Modal Body -->
                <div class="modal-body" id="Div8">
                   <form role="form">
                        <div class="row">
                            <div class="col-sm-12">
                                <span>Confirm to revise C/E form?</span>
                            </div>
                        </div>
                   </form>
                </div>
                
                <!-- Modal Footer -->
                <div class="modal-footer">
                    <div class="form-group">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" id="btnReviseCE1" name="Edit" data-dismiss="modal">Confirm</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <!-- Modal : Hidden Form Input-->
    <div class="modal fade" id="ConvertCEForm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
        <div class="modal-dialog">
            
            <div class="modal-content" >
                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                           <span aria-hidden="true">&times;</span>
                           <span class="sr-only">Close</span>
                    </button>
                    <h4 class="modal-title" id="H4_6">Confirmation</h4>
                </div>
                
                <!-- Modal Body -->
                <div class="modal-body" id="Div7">
                   <form role="form">
                        <div class="row">
                            <div class="col-sm-12">
                                <span>Confirm to approve and convert form to project?</span>
                            </div>
                        </div>
                   </form>
                </div>
                
                <!-- Modal Footer -->
                <div class="modal-footer">
                    <div class="form-group">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" id="btnConvertCE1" name="Edit" data-dismiss="modal">Confirm</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
     <!-- Modal : Hidden Form Input-->
    <div class="modal fade" id="DeleteCEItem" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            
            <div class="modal-content" >
                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                           <span aria-hidden="true">&times;</span>
                           <span class="sr-only">Close</span>
                    </button>
                    <h4 class="modal-title" id="H4_5">Confirmation</h4>
                </div>
                
                <!-- Modal Body -->
                <div class="modal-body" id="Div6">
                   <form role="form">
                        <div class="row">
                            <div class="col-sm-12">
                                <span>Confirm to delete item?</span>
                            </div>
                        </div>
                   </form>
                </div>
                
                <!-- Modal Footer -->
                <div class="modal-footer">
                    <div class="form-group">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" id="btnDeleteItem" name="Edit" data-dismiss="modal">Confirm</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <!-- Modal : Hidden Form Input-->
    <div class="modal fade" id="AddEditCostEstimate" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
        <div class="modal-dialog">
            
            <div class="modal-content" >
                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                           <span aria-hidden="true">&times;</span>
                           <span class="sr-only">Close</span>
                    </button>
                    <h4 class="modal-title" id="H4_4">Item</h4>
                </div>
                
                <!-- Modal Body -->
                <div class="modal-body" id="ModalItems">
                   <form role="form">
                       <div class="row">
                           <div class="col-sm-6" id="checkbox">
                                <div class="form-group">
                                    <div class="col-sm-12 columns table-bordered" style="padding: 10px 5px 10px 10px;">
                                    <label class="col-sm-6 control-label text-left" for="Checkboxes"><font color="red">Others</font></label> 
                                        <input type="checkbox" name="IsOthers" id="chkIsOthers" value="False" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="Category" class="control-label"><font color="red">*</font>Category</label>
                                    <input type="text" maxlength="100" class="form-control" id="ItemCategory" name="ItemCategory" placeholder="e.g: Item, Labor, Service"/>
                                </div>
                            </div>
                        </div>
                       <div class="row">
                           <div class="col-sm-6">
                                <div class="form-group">
                                <label class="col-sm-6 control-label text-left" for="ItemMaterial" class="control-label"><font color="red">*</font>Material</label>
                                <input type="text" maxlength="100" class="form-control" id="ItemMaterial" name="ItemMaterial" placeholder="" readonly="readonly"/>
                              </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                <label class="col-sm-6 control-label text-left" for="ItemBrand" class="control-label"><font color="red">*</font>Size</label>
                                <input type="text" maxlength="100" class="form-control" id="ItemSize" name="ItemSize" placeholder="" readonly="readonly"/>
                                </div>
                            </div>
                        </div>
                       <div class="row">
                           <div class="col-sm-6">
                                <div class="form-group">
                                <label class="col-sm-6 control-label text-left" for="ItemBrand" class="control-label"><font color="red">*</font>Supplier Name</label>
                                <input type="text" maxlength="100" class="form-control" id="SupplierName" name="SupplierName" placeholder="" readonly="readonly"/>
                                </div>
                            </div>
                           <div class="col-sm-6">
                                <div class="form-group">
                                <label class="col-sm-6 control-label text-left" for="ItemBrand" class="control-label">Brand</label>
                                <input type="text" maxlength="100" class="form-control" id="ItemBrand" name="ItemBrand" placeholder="" readonly="readonly"/>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group has-feedback">
                                <label class="col-sm-6 control-label text-left" for="ItemDescription" class="control-label"><font color="red">*</font>Description</label>
                                <textarea class="form-control" rows="5" id="ItemDescription" name="ItemDescription"  readonly="readonly"></textarea>
                              </div>
                            </div>
                        </div>
                        
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                <label class="col-sm-6 control-label text-left" for="UOM" class="control-label"><font color="red">*</font>UOM</label>
                                <input type="text" maxlength="100" class="form-control" id="UOM" name="UOM" placeholder="" readonly="readonly" />
                              </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                <label class="col-sm-6 control-label text-left" for="Quantity" class="control-label"><font color="red">*</font>Qty</label>
                                <input type="text" class="form-control" id="Quantity" name="Quantity" placeholder="" onkeyup="CalculateTotal();"/>
                              </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                <label class="col-sm-6 control-label text-left" for="CurrencyCode" class="control-label"><font color="red">*</font>Currency Code</label>
                                <input type="text" maxlength="100" class="form-control" id="CurrencyCode" name="CurrencyCode" placeholder="" required  readonly="readonly" />
                              </div>
                            </div>
                           <div class="col-sm-6">
                                <div class="form-group">
                                <label class="col-sm-6 control-label text-left" for="CurrencyRate" class="control-label"><font color="red">*</font>Currency Rate</label>
                                <input type="text" maxlength="100" class="form-control" id="CurrencyRate" name="CurrencyRate" placeholder="" readonly="readonly" onkeyup="CalculateTotal();"/>
                              </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                <label class="col-sm-6 control-label text-left" for="QuotedPrice" class="control-label"><font color="red">*</font>Unit Price</label>
                                <input type="text" maxlength="100" class="form-control" id="QuotedPrice" name="QuotedPrice" placeholder="" required readonly="readonly" onkeyup="CalculateTotal();"/>
                              </div>
                            </div>
                           <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="MarkUp" class="control-label">Mark Up Price</label>
                                    <input type="text" maxlength="100" class="form-control" id="MarkUpPrice" name="MarkUpPrice"  onkeyup="CalculateTotal();"/>
                                </div>
                            </div>
                        </div>   
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label text-left" for="TotalAmount" class="control-label">Total Amount</label>
                                    <input type="text" maxlength="100" class="form-control" id="TotalAmount" name="TotalAmount" placeholder="" readonly="readonly" onkeyup="CalculateTotal();"/>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row">
                         <div class="col-sm-12">
                                <div class="form-group">
                                <label class="col-sm-6 control-label text-left" for="Remarks" class="control-label">Remarks</label>
                                <textarea class="form-control" rows="5" id="Remarks" name="Remarks"></textarea>
                              </div>
                            </div>
                            </div>
                   </form>
                </div>
                
                <!-- Modal Footer -->
                <div class="modal-footer">
                <div class="form-group">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-primary" id="btnCEDetailSubmit" name="Reject">Submit</button>
                    </div>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".project-menu").addClass("active expand");
            $(".sub-cost-estimate").addClass("active");
        });
    </script>
    <script src="AddEditCostEstimate.aspx.js" type="text/javascript"></script>
</asp:Content>
