<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="GMSWeb.Claim.Detail"  Title="Claim Detail Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <style>
        .table-input {
            border-radius: 0;
            border-color: #fff;
            background-color: transparent;
        }

        input.ng-invalid.ng-touched, input.ng-invalid-required.ng-touched {
            border: 1px solid red;
        }

        .ui-autocomplete-loading {
            background: transparent url("../new_assets/img/ui-anim_basic_16x16.gif") right center no-repeat;
            transition:none !important;
        }
    </style>
    
    <a name="TemplateInfo"></a>
    <h1 class="page-header">Entertainment Claim Detail
        <br />
    </h1>

    <div data-ng-controller="ClaimDetailController as claim" data-ng-cloak="">
        <input type="hidden" id="hidCurrentLink" data-ng-bind="currentlink" runat="server" value="" />

        <!-- Reject Modal used to keep track the reason to reject-->
        <div id="rejectModal" class="modal fade" role="dialog" data-backdrop="static" >
            <div class="modal-dialog" >
                <div class="modal-content">
                    <div class="modal-header">
                         <h4 class="modal-title">Reject Claim Remark</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <textarea class="form-control" id="reject_remark" data-ng-model="claim.claimInfo.rejectremark"></textarea>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-dismiss="modal" data-ng-click="claim.submitClaim(claim.claimInfo,'Reject')">Confirm Reject</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
       
        <!--Upload Attachment Modal-->
        <div id="uploadModal" class="modal fade" role="dialog" data-backdrop="static" >
            <div class="modal-dialog" >
                <div class="modal-content">
                    <div class="modal-header">
                         <h4 class="modal-title">{{claim.selectedAttachmentID != 0 ? 'Update' : 'New'}} Receipt </h4>
                    </div>
                    <div class="modal-body">
                        <table class="table table-bordered table-condesed" data-ng-if="claim.attachmentList.length > 0">
                            <tr>
                                <td>No</td>
                                <td>Action</td>
                            </tr>
                            <tr data-ng-repeat="attachment in claim.attachmentList track by $index">
                                <td>{{'Receipt' +' '+($index + 1)}}</td>
                                <td class="btn-col" style="white-space: nowrap">
                                    <a href="javascript:void(0)" class="btn btn-xs btn-default" data-ng-click="claim.previewAttachment(attachment)"><i class="ti-search"></i></a>
                                    <a href="javascript:void(0)" class="btn btn-xs btn-default" data-ng-if="claim.claimInfo.Status == '0'" data-ng-click="claim.deleteAttachment(attachment)"><i class="ti-trash"></i></a>
                                </td>
                            </tr>
                        </table>
                        <div class="form-group " data-ng-if="claim.claimInfo.Status == '0'">
                            <input type="file" id="attachmentFile" accept="image/*" data-ng-file-select="onFileSelect($files)" data-ng-model="claim.attachmentSrc" class="form-control">
                        </div>
                        <img data-ng-src="{{claim.attachmentSrc}}" class="img-responsive img-thumbnail" data-ng-if="claim.attachmentSrc != ''"/>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-ng-click="claim.resetSelectedAttachment()"  data-ng-if="claim.claimInfo.Status == '0' && claim.selectedAttachmentID != 0">New Receipt</button>
                        <button type="button" class="btn btn-primary" data-ng-click="claim.attachmentSrc == '' || claim.saveAttachment()"  data-ng-disabled="claim.attachmentSrc == ''"  data-ng-if="claim.claimInfo.Status == '0'">{{claim.selectedAttachmentID != 0 ? 'Update' : 'Submit'}}</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Approve Modal used to key in the payment voucher-->
        <div id="approveModal" class="modal fade" role="dialog" data-backdrop="static" >
            <div class="modal-dialog" >
                <div class="modal-content">
                    <div class="modal-header">
                         <h4 class="modal-title">Payment Voucher Number</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <textarea class="form-control" id="approve_paymentvoucher" data-ng-model="claim.claimInfo.paymentvoucher"></textarea>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" data-dismiss="modal" data-ng-click="claim.submitClaim(claim.claimInfo,'Approve')">Confirm Approve</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="panel panel-primary panel-loading">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-collapse"><i class="glyphicon glyphicon-chevron-up"></i></a>
            </div>
            Claimant Detail
        </div>
        <div class="panel-loading" style="z-index:1"><div class="spinner"></div></div>
        <div class="panel-body ">
            <div class="m-t-20">
                <div class="row">
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Claim No</label>
                        <input type="text" class="form-control" autocomplete="off" data-ng-model="claim.claimInfo.ClaimNo" id="Text7" disabled/>
                    </div>
                     <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Description!='Travel'">
                        <label class="control-label">Claim Date</label>
                        <input type="text" class="form-control" datepicker autocomplete="off" data-ng-model="claim.claimInfo.ClaimDate" data-ng-disabled="claim.claimInfo.Status != '0'"/>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Description=='Travel'">
                        <label class="control-label">Departure Date</label>
                        <input type="text" class="form-control" datepicker autocomplete="off" data-ng-model="claim.claimInfo.DepartureDate" data-ng-disabled="claim.claimInfo.Status != '0'"/>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Description=='Travel'">
                        <label class="control-label">Return Date</label>
                        <input type="text" class="form-control" datepicker autocomplete="off" data-ng-model="claim.claimInfo.ReturnDate" data-ng-disabled="claim.claimInfo.Status != '0'"/>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Claim Status</label>
                        <input type="text" class="form-control" autocomplete="off" data-ng-model="claim.claimInfo.StatusName" disabled />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Description!='Travel'">
                        <label class="control-label">Claim Purpose</label>
                        <select class="form-control" name="claim_desc" id="claim_desc" data-ng-model="claim.claimInfo.Description" data-ng-disabled="claim.claimInfo.Status != '0'">
                          <option data-ng-repeat="claimpurpose in claim.typeList | filter: {EntertainmentType: 'Purpose'}" value="{{claimpurpose.EntertainmentOptions}}">{{claimpurpose.EntertainmentOptions}}</option>
                        </select>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Company Name</label>
                        <input type="text" class="form-control" autocomplete="off" id="Text3" data-ng-model="claim.claimInfo.CompanyName" disabled/>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Employee Name</label>
                        <input type="text" class="form-control" autocomplete="off" data-ng-model="claim.claimInfo.UserRealName" disabled/>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Employee No</label>
                        <input type="text" class="form-control" autocomplete="off" id="Text2" data-ng-model="claim.claimInfo.EmployeeNo" disabled/>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Sales Person ID</label>
                        <select class="form-control" name="salespersonid" data-ng-model="claim.claimInfo.SalesPersonID" data-ng-disabled="claim.claimInfo.Status != '0'||claim.claimInfo.Description=='Travel'">
                          <option value="">--</option>
                          <option data-ng-repeat="salespersonid in claim.salesPersonID" value="{{salespersonid.salespersonid}}">{{salespersonid.salespersonid}}</option>
                        </select>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Description=='Travel'">
                        <label class="control-label">Claim Purpose</label>
                        <select class="form-control" name="claim_desc" id="claim_desc" data-ng-model="claim.claimInfo.Description" data-ng-disabled="claim.claimInfo.Status != '0'">
                          <option data-ng-repeat="claimpurpose in claim.typeList | filter: {EntertainmentType: 'Purpose'}" value="{{claimpurpose.EntertainmentOptions}}" >{{claimpurpose.EntertainmentOptions}}</option>
                        </select>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.allowCreateOnbehalf">
                        <label class="control-label">Create On Behalf</label>
                        <select class="form-control" name="createonbehalf" data-ng-model="claim.claimInfo.CreateOnBehalf" data-ng-disabled="claim.claimInfo.Status != '0'">
                          <option value="">--</option>
                          <option data-ng-repeat="onbehalfuser in claim.onbehalfuser" value="{{onbehalfuser.CreateOnBehalf}}">{{onbehalfuser.UserRealName}}</option>
                        </select>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Designation</label>
                        <input type="text" class="form-control" autocomplete="off" id="Text4" data-ng-model="claim.claimInfo.ClaimantDesig" data-ng-disabled="claim.claimInfo.Status != '0'"/>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Description!='Travel'">
                        <label class="control-label">No of Customers</label>
                        <input type="text" class="form-control" autocomplete="off" id="Text5" data-ng-model="claim.claimInfo.NumPplEntertained" data-ng-disabled="claim.claimInfo.Status != '0'"/>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Description=='Travel'">
                        <label class="control-label">Destination</label>
                        <input type="text" class="form-control" autocomplete="off" id="TextDestination" data-ng-model="claim.claimInfo.Destination" data-ng-disabled="claim.claimInfo.Status != '0'"/>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Status=='2' && claim.claimInfo.Description != 'Travel'">
                        <label class="control-label">Payment Voucher</label>
                        <input type="text" class="form-control" autocomplete="off" id="Text6" data-ng-model="claim.claimInfo.PaymentVoucher"  disabled/>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">DIM 1 (DIVISION)</label>
                        <select class="form-control" name="dim1" data-ng-model="claim.claimInfo.dim1" data-ng-disabled="(claim.claimInfo.Status != '0' && claim.access) || (claim.claimInfo.Status > '1' && !claim.allowApproveReject)">
                          <option data-ng-repeat="dim1 in claim.dim1List" value="{{dim1.ProjectID}}">{{dim1.ProjectName}}</option>
                        </select>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">DIM 2 (DEPARTMENT)</label>
                        <select class="form-control" name="dim2" data-ng-model="claim.claimInfo.dim2" data-ng-disabled="(claim.claimInfo.Status != '0' && claim.access) || (claim.claimInfo.Status > '1' && !claim.allowApproveReject)">
                            <option data-ng-repeat="dim2 in claim.dim2List" value="{{dim2.DepartmentID}}">{{dim2.DepartmentName}}</option>
                        </select>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">DIM 3 (SECTION)</label>
                        <select class="form-control" name="dim3" data-ng-model="claim.claimInfo.dim3" data-ng-disabled="(claim.claimInfo.Status != '0' && claim.access) || (claim.claimInfo.Status > '1' && !claim.allowApproveReject)">
                            <option data-ng-repeat="dim3 in claim.dim3List" value="{{dim3.SectionID}}">{{dim3.SectionName}}</option>
                        </select>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">DIM 4 (UNIT)</label>
                        <select class="form-control" name="dim4" data-ng-model="claim.claimInfo.dim4" data-ng-disabled="(claim.claimInfo.Status != '0' && claim.access) || (claim.claimInfo.Status > '1' && !claim.allowApproveReject)">
                          <option data-ng-repeat="dim4 in claim.dim4List" value="{{dim4.UnitID}}">{{dim4.UnitName}}</option>
                        </select>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Description!='Travel'">
                        <label class="control-label">Customer Company Name</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.Cust1"  data-ng-disabled="claim.claimInfo.Status != '0'"
                            custom-auto-complete data-src="claim.customerListSrc" data-keyword-length="3" 
                            data-search-object="{'CompanyID':claim.companyID,'Name':claim.claimInfo.Cust1}" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Description!='Travel'">
                        <label class="control-label">Name Of Person</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.Person1" data-ng-disabled="claim.claimInfo.Status != '0'" placeholder="Mr. Wong/Mr. Lim/..."/>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Description!='Travel'">
                        <label class="control-label">Designation</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.Desig1" data-ng-disabled="claim.claimInfo.Status != '0'" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Description=='Travel'">
                        <label class="control-label">Project No./Name</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.ProjectName" data-ng-disabled="claim.claimInfo.Status != '0'"/>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Description=='Travel'">
                        <label class="control-label">Travel With</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.TravelWith" data-ng-disabled="claim.claimInfo.Status != '0'"/>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12" data-ng-if="claim.claimInfo.Description=='Travel'">
                        <label class="control-label">Purpose Of Travel</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.PurposeOfTravel" data-ng-disabled="claim.claimInfo.Status != '0'"/>
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Phone</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.Phone1" maxlength="20" data-ng-disabled="claim.claimInfo.Status != '0'"/>
                    </div>
                </div>
                <%--<div class="row">
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Customer Company Name 2</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.Cust2" data-ng-disabled="claim.claimInfo.Status != '0'" 
                            custom-auto-complete data-src="claim.customerListSrc" data-keyword-length="3" 
                            data-search-object="{'CompanyID':claim.companyID,'Name':claim.claimInfo.Cust2}" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Name Of Person 2</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.Person2" data-ng-disabled="claim.claimInfo.Status != '0'" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Designation 2</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.Desig2" data-ng-disabled="claim.claimInfo.Status != '0'" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Phone 2</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.Phone2" maxlength="20" data-ng-disabled="claim.claimInfo.Status != '0'" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Customer Company Name 3</label>
                       <input type="text" class="form-control" data-ng-model="claim.claimInfo.Cust3" data-ng-disabled="claim.claimInfo.Status != '0'" 
                            custom-auto-complete data-src="claim.customerListSrc" data-keyword-length="3" 
                            data-search-object="{'CompanyID':claim.companyID,'Name':claim.claimInfo.Cust3}" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Name Of Person 3</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.Person3" data-ng-disabled="claim.claimInfo.Status != '0'" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Designation 3</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.Desig3" data-ng-disabled="claim.claimInfo.Status != '0'" />
                    </div>
                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Phone 3</label>
                        <input type="text" class="form-control" data-ng-model="claim.claimInfo.Phone3" maxlength="20" data-ng-disabled="claim.claimInfo.Status != '0'" />
                    </div>
                </div>--%>
            </div>
        </div>
            <div class="panel-footer clearfix" data-ng-if="claim.access == true">
                <a href="javascript:void(0)" class="btn btn-default pull-right m-r-10" data-ng-click="claim_detail_form.$invalid || claim.detail.length == 0 || claims.detailsSaved == false || claim.submitClaim(claim.claimInfo,'Submit')" data-ng-disabled="claim_detail_form.$invalid || claim.detail.length == 0" data-ng-if="claim.claimInfo.Status == '0' && claim.access == true">Submit For Approval</a>
                <a href="javascript:void(0)" class="btn btn-primary pull-right m-r-10" data-ng-click="claim_detail_form.$invalid || claim.updateClaimInfo()" data-ng-disabled="claim_detail_form.$invalid" data-ng-if="claim.claimInfo.Status == 0 && claim.access == true">Update Claim</a>
                <a href="javascript:void(0)" class="btn btn-primary pull-right m-r-10" data-ng-click="claim.submitClaim(claim.claimInfo,'Revise')" data-ng-if="(claim.claimInfo.Status == 3 || claim.claimInfo.Status == 1) && claim.access == true">Revise Claim</a>
                <a href="javascript:void(0)" class="btn btn-danger pull-left m-r-10" data-ng-click="claim.deleteClaim(claim.claimInfo)" data-ng-if="claim.claimInfo.Status == 0 && claim.access == true">Delete Claim</a>
            </div>
            <div class="panel-footer clearfix" data-ng-if="claim.claimInfo.Status == 1 && claim.allowApproveReject">
                <a href="javascript:void(0)" class="btn btn-success pull-right m-r-10" data-ng-click="claim.approveClaim()" data-ng-if="claim.claimInfo.Status == 1 && claim.allowApproveReject">Approve</a>
                <a href="javascript:void(0)" class="btn btn-danger pull-right m-r-10" data-ng-click="claim.rejectClaim()" data-ng-if="claim.claimInfo.Status == 1 && claim.allowApproveReject">Reject</a>
                <a href="javascript:void(0)" class="btn btn-primary pull-right m-r-10" data-ng-click="claim.updateClaimInfo()" data-ng-if="claim.claimInfo.Status == 1 && claim.allowApproveReject">Update Dimension</a>
            </div>
    </div>

    

    <div class="btn-group pull-right" role="group" aria-label="...">
        <a href="javascript:void(0)" class="btn btn-default pull-right m-r-10" data-ng-click="claim.saveDetails()" data-ng-if="claim.claimInfo.Status >= 1 && claim.allowApproveReject">
                Save GST Detail
        </a>
        <a href="javascript:void(0)" class="btn btn-default pull-right m-r-10" data-ng-click="claim.addDetail()" data-ng-if="claim.claimInfo.Status == '0' && claim.access">
                Add Detail
        </a>
    </div>
    <div class="clearfix"></div>
    <br />
    <div class="alert alert-danger m-b-10" data-ng-show="claim_detail_form.$invalid">
		<strong>Please fill up your form.</strong> The claim sheet is invalid or have empty column.
	</div>
    <div class="table-responsive" ng-form name="claim_detail_form" data-ng-show="claim.detail.length > 0">
        <table class="table table-bordered custom-excel-table" data-ng-class="{'table-striped':claim.claimInfo.status > 0,'table-hover':claim.claimInfo.Status == '0'}" width="100%">
        <thead>
            <tr>
                <th>#</th>
                <th class="text-center" style="width:50px;">Receipt</th>
                <th>Date                        
                    <i class="ti-calendar pull-right hidden-xs hidden-sm" data-toggle="tooltip" data-placement="top" title="" data-original-title="Datepicker" data-ng-if="claim.claimInfo.Status == '0'"></i>
                </th>
                <th>Destination                       
                    <i class="ti-text pull-right hidden-xs hidden-sm" data-toggle="tooltip" data-placement="top" title="" data-original-title="Text Input" data-ng-if="claim.claimInfo.Status == '0'"></i>
                </th>
                <th>Receipt Number                        
                    <i class="ti-text pull-right hidden-xs hidden-sm" data-toggle="tooltip" data-placement="top" title="" data-original-title="Text Input" data-ng-if="claim.claimInfo.Status == '0'"></i>
                </th>
                <th>Type
                    <i class="ti-text pull-right hidden-xs hidden-sm" data-toggle="tooltip" data-placement="top" title="" data-original-title="Text Input" data-ng-if="claim.claimInfo.Status == '0'"></i>
                </th>
                <th style="width:150px;">Charge to
                    <i class="ti-text pull-right hidden-xs hidden-sm" data-toggle="tooltip" data-placement="top" title="" data-original-title="Text Input" data-ng-if="claim.claimInfo.Status == '0'"></i>
                </th>
                <th>Remarks                       
                    <i class="ti-text pull-right hidden-xs hidden-sm" data-toggle="tooltip" data-placement="top" title="" data-original-title="Text Input" data-ng-if="claim.claimInfo.Status == '0'"></i>
                </th>
                <th style="width:100px;">Currency
                    <i class="ti-search pull-right hidden-xs hidden-sm" data-toggle="tooltip" data-placement="top" title="" data-original-title="Search" data-ng-if="claim.claimInfo.Status == '0'"></i>
                </th>
                <th style="width:100px;">Rate
                    <i class="ti-text pull-right hidden-xs hidden-sm" data-toggle="tooltip" data-placement="top" title="" data-original-title="Text Input" data-ng-if="claim.claimInfo.Status == '0'"></i>
                </th>
                <th style="width:100px;">Amount
                    <i class="ti-text pull-right hidden-xs hidden-sm" data-toggle="tooltip" data-placement="top" title="" data-original-title="Integer" data-ng-if="claim.claimInfo.Status == '0'"></i>
                </th>
                <th style="width:70px;" data-ng-if="claim.allowApproveReject || claim.claimInfo.Status > 1">GST ({{claim.defaultCurrency}})
                    <i class="ti-text pull-right hidden-xs hidden-sm" data-toggle="tooltip" data-placement="top" title="" data-original-title="Integer" data-ng-if="claim.claimInfo.Status == '0'"></i>
                </th>
                <th style="width:100px;">Amount ({{claim.defaultCurrency}})</th>
                <th style="width:100px;"></th>
            </tr>
        </thead>
        <tbody data-ng-if="claim.claimInfo.Status != '0'">
            <tr data-ng-repeat="detail in claim.detail | orderBy : 'id' track by $index">
                <td class="p-l-10">{{$index + 1}}</td>
                <td class="btn-col text-center">
                    <a href="javascript:void(0)" class="btn btn-xs btn-default" disabled data-ng-if="detail.Count != '0'"><i class="ti-check"></i></a>
                    <a href="javascript:void(0)" class="btn btn-xs btn-default" disabled data-ng-if="detail.Count == '0'"><i class="ti-close"></i></a>
                </td>
                <td>{{detail.date}}</td>
                <td>{{detail.Destination}}</td>
                <td>{{detail.ReceiptNum}}</td>
                <td>{{detail.type}}</td>
                <td>{{detail.chargeto}}</td>
                <td>{{detail.remark}}</td>
                <td>{{detail.currencyCode}}</td>
                <td>{{detail.currencyRate | number : 4}}</td>
                <td class="text-right">{{detail.amount | number : 2}}</td>
                <td class="p-0" data-ng-if="claim.allowApproveReject || claim.claimInfo.Status > 1"><input type="number" data-input-field="GST" data-ng-model="detail.GST" class="form-control table-input text-right" data-ng-disabled="!claim.allowApproveReject || claim.claimInfo.Status < 1" /></td>
                <td class="text-right">{{detail.amountSGD | number : 2}}</td>
                <td class="btn-col">
                    <a href="javascript:void(0)" class="btn btn-xs btn-default" data-ng-click="claim.uploadReceipt(detail)"><i class="ti-files"></i></a>
                </td>
            </tr>
        </tbody>
        <tbody data-ng-if="claim.claimInfo.Status == '0'" >
            <tr data-ng-repeat="detail in claim.detail | orderBy : 'id' track by $index ">
                <td class="p-l-10">{{$index + 1}}</td>
                <td class="btn-col text-center">
                    <a href="javascript:void(0)" class="btn btn-xs btn-default" disabled data-ng-if="detail.Count && detail.Count != '0'"><i class="ti-check"></i></a>
                    <a href="javascript:void(0)" class="btn btn-xs btn-default" disabled data-ng-if="detail.Count == '0'"><i class="ti-close"></i></a>
                </td>
                <td class="p-0"><input type="text" data-input-field="date" data-ng-model="detail.date" datepicker class="form-control table-input" readonly required/></td>
                <td class="p-0"><input type="text" data-input-field="destination" data-ng-model="detail.Destination" class="form-control table-input text-right" /></td>
                <td class="p-0"><input type="text" data-input-field="receiptnumber" data-ng-model="detail.ReceiptNum" class="form-control table-input text-right" /></td>
                <td class="p-0">
                    <select class="form-control table-input" name="typeList"  data-ng-model="detail.type" required custom-auto-complete">
                          <option data-ng-repeat="receipttype in claim.typeList | filter: {EntertainmentType: 'ReceiptType'}" value="{{receipttype.EntertainmentOptions}}">{{receipttype.EntertainmentOptions}}</option>
                    </select>
                </td>
                <td class="p-0"><input type="text" data-input-field="chargeto" data-ng-model="detail.chargeto" class="form-control table-input text-right" required /></td>
                <td class="p-0"><input type="text" data-input-field="remarks" data-ng-model="detail.remark" class="form-control table-input text-right" /></td>
                <td class="p-0"><input type="text" data-input-field="currencyCode" class="form-control table-input"
                    custom-auto-complete data-hidden-value="detail.currencyRate"  data-src="claim.currencyListSrc"
                    data-ng-model="detail.currencyCode" data-search-object="{'DefaultCurrency':claim.defaultCurrency,'CompanyID':claim.companyID,'Currency':detail.currencyCode ? detail.currencyCode : '','Date':detail.date}" required />
                </td>
                <td class="p-0"><input type="text" data-input-field="currencyRate" data-ng-model="detail.currencyRate" class="form-control table-input text-right" required /></td>
                <td class="p-0"><input type="number" data-input-field="amount" data-ng-model="detail.amount" class="form-control table-input text-right" required /></td>
                <td class="text-right" data-ng-model="detail.GST" data-ng-if="claim.allowApproveReject || claim.claimInfo.Status > 1">{{detail.GST | number : 2}}</td> 
                <td class="text-right">{{detail.amountSGD | number : 2}}</td>
                <td class="btn-col" style="white-space: nowrap" > 
                    <div data-ng-if="claim.access">
                        <a href="javascript:void(0)" class="btn btn-default btn-xs" data-ng-click="claim.saveDetails()" data-ng-if="detail.id != null" data-ng-disabled="claim_detail_form.$invalid"><i class="ti-save"></i></a>           
                        <a href="javascript:void(0)" class="btn btn-warning btn-xs" data-ng-click="claim.saveDetails()" data-ng-if="detail.id == null" data-ng-disabled="claim_detail_form.$invalid"><i class="ti-plus"></i></a>
					    <a href="javascript:void(0)" class="btn btn-xs btn-default" data-ng-class="{'btn-danger btn-outline-secondary mr-2' : detail.Count == '0'}" data-ng-click="claim.uploadReceipt(detail)" data-ng-if="detail.id != null" data-toggle="tooltip" title="Please upload your receipt."><i class="ti-files"></i></a>
                        <a href="javascript:void(0)" class="btn btn-xs btn-default" data-ng-click="claim.deleteDetail(detail,$index)" ><i class="ti-trash"></i></a>
                    </div>
                    <div data-ng-if="claim.allowApproveReject && !claim.access">
                        <a href="javascript:void(0)" class="btn btn-xs btn-default" data-ng-click="claim.uploadReceipt(detail)"><i class="ti-files"></i></a>
                    </div>        
                </td>
            </tr>
        </tbody>
        <tfoot>
            <tr >
                <td colspan="11" class="text-right"><b>Total</b></td>
                <td class="text-right" data-ng-if="claim.allowApproveReject || claim.claimInfo.Status > 1">{{claim.totalGST | number : 2}}</td>
                <td class="text-right">{{claim.totalSGD | number : 2}}</td>
                <td></td>
            </tr>
        </tfoot>
    </table>
    </div>
    
    </div>


   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">

    </script>
    <script src="../new_assets/js/angular/factory/claim.js"></script>
    <script src="../new_assets/js/angular/factory/commonUtil.js"></script>
    <script src="../new_assets/js/angular/directive/autocomplete.js"></script>
    <script type="text/javascript" src="claim_detail.js"></script>
    <script>
        $(document).ready(function () {
            $(".administration-menu").addClass("active expand");
            $(".sub-claim").addClass("active");
        });
    </script>
</asp:Content>

