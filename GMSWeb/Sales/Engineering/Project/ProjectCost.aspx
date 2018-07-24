<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectCost.aspx.cs" Inherits="GMSWeb.Sales.Engineering.Project.ProjectCost" %>


<br />
    <!-- Tab v2 -->

	<div class="tab-v2">
		<ul class="nav nav-tabs">
            <li class="active" id="MRTab"><a href="#Material" data-toggle="tab">Material Requisition</a></li>
            <li  id="LaborTab"><a href="#Labor" data-toggle="tab">Labor Cost</a></li>
            <li  id="MiscTab"><a href="#Misc" data-toggle="tab">Miscellaneous</a></li>
		</ul>
		<div class="tab-content">
			<div class="tab-pane fade in active" id="Material">
				<table id="tblMaterialRequisition" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%"></table>
			</div>
			<div class="tab-pane fade in" id="Labor">
				<table id="tblLaborCost" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%"></table>
			</div>
			<div class="tab-pane fade in" id="Misc">
				<table id="tblMiscellaneous" class="table table-striped table-bordered dt-responsive nowrap" cellspacing="0" width="100%"></table>
			</div>
				
		</div>
	</div>
	
    <!-- End Tab v2 -->
     <!-- Modal : Hidden Form Input-->
    <div class="modal fade" id="ModelMR" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
        <div class="modal-dialog">
            <div class="modal-content" >
                <!-- Modal Header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                           <span aria-hidden="true">&times;</span>
                           <span class="sr-only">Close</span>
                    </button>
                    <h4 class="modal-title" id="H4_4">MR Payment</h4>
                </div>
                
                <!-- Modal Body -->
                <div class="modal-body" id="ModalMRPayment">
                   <form role="form">
                       <div class="tab-v2">
                            <ul class="nav nav-tabs">
                                <li class="active" id="t1"><a href="#AddInvoiceListing" data-toggle="tab">Information</a></li>
                                <li id="t2"><a href="#InvoiceListing" data-toggle="tab">Invoices</a></li>
                            </ul>
                            <div class="tab-content">
	                            <!-- Child Tab 1 -->
		                        <div class="tab-pane fade in active" id="AddInvoiceListing">
		                            <div class="container">
                                       <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <label for="Category" class="control-label"><font color="red">*</font>MR No</label>
                                                    <input type="text" maxlength="100" class="form-control" id="MRNo" name="MRNo" placeholder="MR1700001" readonly="readonly" />
                                                </div>
                                            </div>
                                          <div class="col-sm-6">
                                                <div class="form-group">
                                                <label for="ItemBrand" class="control-label"><font color="red">*</font>Invoice No.</label>
                                                <input type="text" maxlength="100" class="form-control" id="InvoiceNo" name="InvoiceNo" placeholder=""/>
                                                </div>
                                            </div>
                                        </div>
                                       <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                <label for="ItemBrand" class="control-label"><font color="red">*</font>Invoice Amount.</label>
                                                <input type="text" maxlength="100" class="form-control" id="InvoiceAmount" name="InvoiceAmount" placeholder=""/>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                <label for="ItemBrand" class="control-label"><font color="red">*</font>Invoice Remarks</label>
                                                <textarea id="InvoiceRemarks" name="InvoiceRemarks" class="form-control" rows="5"></textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
		                        </div>
		                        <!-- End Child Tab 1 -->
            		            
		                        <!-- Child Tab 2 -->
		                        <div class="tab-pane fade in" id="InvoiceListing">
		                            <table id="tblMRInvoiceList" class="table table-striped table-bordered dataTable no-footer" width="100%"></table>
		                        </div>
                            </div>
                        </div>
                   </form>
                </div>
                
                <!-- Modal Footer -->
                <div class="modal-footer">
                <div class="form-group">
                    <button type="button" class="btn btn-default" id="btnMRCancel" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-primary" id="btnMRSubmit" name="Submit">Submit</button>
                    </div>
                </div>
            </div>
        </div>
        </div>
    
<script src="ProjectCost.aspx.js" type="text/javascript"></script>