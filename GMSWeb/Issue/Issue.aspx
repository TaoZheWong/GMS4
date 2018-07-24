<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Issue.aspx.cs" Inherits="GMSWeb.Issue.Issue" MaintainScrollPositionOnPostback="true" Title="Issue" %>
<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<%@ Register TagPrefix="uctrl" TagName="Header1" Src="~/SiteHeader1.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
	<uctrl:Header1 ID="MySiteHeader" runat="server" EnableViewState="true" />

	<script type="text/javascript" src="Issue.js"></script>

	<input type="hidden" id="hidCoyID" runat="server" value="" />	
	<input type="hidden" id="hidUserID" runat="server" value="" />
	<input type="hidden" id="hidSystem" runat="server" value="G" />	
	<input type="hidden" id="hidType" runat="server" value="B" />
	
	<div class="panel panel-custom margin-bottom-40">
		<div class="panel-heading">
		<h1 class="panel-title"><asp:Label ID="lblTitle" runat="server"></asp:Label></h1>
		</div>
		<div id="main" class="panel-body">
			<div id="content">
			<table id="tblIssue" class="table table-striped table-bordered dt-responsive" cellspacing="0" width="100%"></table>
			
			<div class="modal fade" id="ModalIssue" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-toggle="validator">
			<div class="modal-dialog">
				<div class="modal-content">
					<!-- Modal Header -->
					<div class="modal-header">
						<button type="button" class="close" data-dismiss="modal">
							   <span aria-hidden="true">&times;</span>
							   <span class="sr-only">Close</span>
						</button>
						<h4 class="modal-title" id="modaltitle">New Issue</h4>
					</div>                
					<!-- Modal Body -->
					<div class="modal-body">
						<form role="form">
							<div class="row">
								<div class="col-sm-6">
								  <div class="form-group">
									<label for="lblReportedBy">Report By</label>
									  <input type="text" class="form-control" name="input-ReportedBy" id="input-ReportedBy" required/>
									  <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
										<div class="help-block with-errors"></div>
								  </div>
								</div> 
								<div class="col-sm-6">
								  <div class="form-group">
									<label for="lblStatus">Status</label>
									<select class="form-control" name="input-Status" id="input-Status">
									  <option value="N">New</option>
									  <option value="P">Pending</option>
									  <option value="C">Closed</option>
									  <option value="X">Cancelled</option>
									  <option value="K">KIV</option>
									</select>
								  </div>
								</div>                                  
							</div>
							<div class="row">
								<div class="col-sm-6">
								  <div class="form-group">
									<label for="lblSystem">System</label>
									<select class="form-control" name="input-System" id="input-System">
									  <option value="G">GMS</option>
									  <option value="L">LMS</option>
									</select>
								  </div>
								</div> 
								<div class="col-sm-6">
								  <div class="form-group">
									<label for="lblType">Type</label>
									<select class="form-control" name="input-Type" id="input-Type">
									  <option value="B">Errors(Bugs)</option>
									  <option value="C">Change</option>
									</select>
								  </div>
								</div>                                  
							</div>
							<div class="row">
								<div class="col-sm-12">
									<div id="DivDescription" class="form-group has-feedback" >
										<label for="txtDescription">Description</label>
										<textarea rows="3" class="form-control" id="input-Description" name='input-Description' required></textarea>
										<span class="glyphicon form-control-feedback" aria-hidden="true"></span>
										<div class="help-block with-errors"></div>
									</div>
								</div>                            
							</div>
							<div class="row" id="DivRowRemarks" style="display:none;">
								<div class="col-sm-12">
									<div class="form-group has-feedback">
										<label for="txtRemarks">Remarks</label>
										<textarea rows="3" class="form-control" id="input-Remarks" name='input-Remarks'></textarea>
									</div>
								</div>                            
							</div>
							<div class="row" id="DivRowDates" style="display:none;">
								<div class="col-sm-6">
								  <div class="form-group">
									<label for="lblRemarksDate">Remarks Date : </label><br />
									<label id="input-RemarksDate">N/A</label>
								  </div>
								</div> 
								<div class="col-sm-6">
								  <div class="form-group">
									<label for="lblCompletedDate">Completed Date : </label><br />
									<label id="input-CompletedDate">N/A</label>
								  </div>
								</div>                                  
							</div>
						</form>
					</div>                
					<!-- Modal Footer -->
					<div class="modal-footer">
						<button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
						<button type="button" class="btn btn-primary" id="save-Issue" name="Edit">Save</button>
					</div>
				</div>
			</div>
		</div>
			</div>
		</div>
	</div>





</asp:Content> 
