<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="CostEstimateSearch.aspx.cs" Inherits="GMSWeb.Sales.Engineering.CostEstimate.CostEstimateSearch" Title="Cost Estimate Search" %>
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
    <li class="active">Cost Estimate</li>
</ul>
<h1 class="page-header">Cost Estimate
</h1>

<div class="panel panel-primary">
		<div class="panel-heading">
			<h1 class="panel-title">
                <i class="ti-search"></i>
                Cost Estimate Search</h1>
		</div>
		
		<div class="panel-body row">
           <div class="m-t-20">
                <input type="hidden" id="hidCoyID" runat="server" value="" />	
		        <input type="hidden" id="hidUserID" runat="server" value="" />
		        <input type="hidden" id="hidCurrentLink" runat="server" value="" />

                <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label" for="txtCEID">C/E No:</label>
                        <input type="text" class="form-control" id="txtCEID" tabindex="1" />
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12">
                    <label class="control-label" for="txtAccountCode">Customer Code:</label>
                        <input type="text" class="form-control" id="txtAccountCode" tabindex="2"/>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12">    
                    <label class="control-label" for="txtAccountName">Customer Name:</label>
                        <input type="text" class="form-control" id="txtAccountName" tabindex="3"/>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12">    
                    <label class="control-label" for="txtCEStatusID">Status:</label>
                        <input type="text" class="form-control" id="txtCEStatusName" tabindex="4"/>
                        <input type="hidden" class="form-control" id="txtCEStatusID" tabindex="4"/>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12" id="CDF">
                    <label class="control-label" for="txtCreatedDateFrom">Creation Date From:</label>
                        <div class='input-group date_yyyy_mm_dd' id='datetimepickerFrom'>
                            <input type='text' class="form-control" id="txtCreatedDateFrom" tabindex="5" readonly/>
                            <span class="input-group-addon">
                                <i class="ti-calendar"></i>
                            </span>
                        </div>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12" id="CDT">
                    <label class="control-label" for="txtCreatedDateTo">Creation Date To:</label>
                        <div class='input-group date_yyyy_mm_dd' id='datetimepickerTo'>
                            <input type='text' class="form-control" id="txtCreatedDateTo" tabindex="5" readonly/>
                            <span class="input-group-addon">
                                <i class="ti-calendar"></i>
                            </span>
                        </div>
                </div>
                <div class="form-group col-lg-3 col-md-6 col-sm-12" id="EngName">    
                    <label class="control-label" for="txtEngineerName">Engineer Name:</label>
                        <input type="text" class="form-control" id="txtEngineerName" tabindex="8"/>
                </div>
            </div> 
        </div>
        <div class="panel-footer clearfix">
            <a href="#" class="btn btn-primary pull-right m-l-10" id="btnSearch"  tabindex="9">Search</a>
            <a href="#" class="btn btn-default pull-right" id="btnAdd">Create New</a>
        </div>
    </div>
    <table id="tblCostEstimate" class="table table-striped table-bordered dt-responsive nowrap" width="100%"></table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".project-menu").addClass("active expand");
            $(".sub-cost-estimate").addClass("active");
        });
    </script>
    <script src="CostEstimateSearch.aspx.js" type="text/javascript"></script>
</asp:Content>
