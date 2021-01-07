<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GMSWeb.Sales.Default" Title="Sales Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <a name="TemplateInfo"></a>

    <input type="hidden" id="hidCoyID" runat="server" value="" />	
	<input type="hidden" id="hidUserID" runat="server" value="" />

    <ul class="breadcrumb pull-right">
        <li class="active">Sales</li>
    </ul>
    <h1 class="page-header">Sales <br />
        <small>Main page for Sales.</small></h1>


    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
            <h4 class="panel-title">
                List of MRs requires your approval / action
            </h4>
        </div>
        <div class="panel-body">
            <table id="tblMRApproval" class="table table-striped table-bordered dataTable no-footer" width="100%"></table>
        </div>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
            <h4 class="panel-title">
                List of MRs that were submitted by you but have been rejected / cancelled for the past 7 days
            </h4>
        </div>
        <div class="panel-body">
            <table id="tblRejected" class="table table-striped table-bordered dataTable no-footer" width="100%"></table>
        </div>
    </div>
    
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-heading-btn">
                <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
            </div>
            <h4 class="panel-title">
                List of MRs that were submitted by you but have been rejected / cancelled for the past 7 days
            </h4>
        </div>
        <div class="panel-body">
            <table id="tblFailed" class="table table-striped table-bordered dataTable no-footer" width="100%"></table>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        var CoyID = getCoyID();
        var UserID = $("#<%=hidUserID.ClientID%>").val();

        function generateTable(elem,ajaxConfig,columns,order) {
            var t = elem.DataTable({
                "ajax": ajaxConfig,
                "order": order,
                "pageLength": 5,
                "processing": true,
                "pageLength": 5,
                "aLengthMenu": [[5, 10, 15, 25, 35, 50, 100, -1], [5, 10, 15, 25, 35, 50, 100, "All"]],
                "responsive": true,
                "bDestroy": false,
                "jQueryUI": true,
                "language": { "emptyTable": "No results found!" },
                "columns": columns
            });

            t.on('order.dt search.dt', function () {
                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        }
        
        $(document).ready(function () {
            //List of MRs requires your approval / action
            var MrApprovalElem = $('#tblMRApproval');
            var mrAjaxConfig = {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "Default.aspx/GetListOfMRRequiresApprovalByUserNumId",
                "data": function (data) { return JSON.stringify({ 'CompanyID': CoyID, 'UserID': UserID }); },
                "error": function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(textStatus);
                },
                "dataSrc": function (json) {
                    json = json.hasOwnProperty('d') ? json.d : json;
                    return json;
                }
            }
            mrColumnsOrder = [[2, "desc"]];
            var mrColumns = [
                { "data": null, "title": "No." },
                {
                    "data": "mrno",
                    "title": "MR No.",
                    "render": function (data, type, row) {
                        return "<a name='Edit' href=\"../Products/Products/AddEditMaterialRequisition.aspx?CurrentLink=Sales&CoyID="+CoyID+"&MRNo="+data+"\" >" + data + "</a>";
                    }
                },
                { "data": "mrdate", "title": "MR Date" },
                { "data": "requestor", "title": "Requestor" },
                { "data": "routeddate", "title": "Date Routed" },
                { "data": "diff", "title": "Days" },
                { "data": "VendorName", "title": "VendorName" },
                { "data": "Purchaser", "title": "Purchaser" },
            ]
            generateTable(MrApprovalElem, mrAjaxConfig, mrColumns, mrColumnsOrder);


            //List of MRs that were submitted by you but have been rejected / cancelled for the past 7 days
            var rejectedElem = $('#tblRejected');
            var rejectedAjaxConfig = {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "error": function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(textStatus);
                },
                "url": "Default.aspx/GetListOfRejectedOrCancelledMRByUserNumId",
                "data": function (data) { return JSON.stringify({ 'CompanyID': CoyID, 'UserID': UserID }); },
                "dataSrc": function (json) {
                    json = json.hasOwnProperty('d') ? json.d : json;
                    return json;
                }
            };
            var rejectedColumnsOrder = [[2, "desc"]];
            var rejectedColumns = [
                { "data": null, "title": "No." },
                {
                    "data": "mrno",
                    "title": "MR No.",
                    "render": function (data, type, row) {
                        return "<a name='Edit' href=\"../Products/Products/AddEditMaterialRequisition.aspx?CurrentLink=Sales&CoyID=" + CoyID + "&MRNo=" + data + "\" >" + data + "</a>";
                    }
                },
                { "data": "mrdate", "title": "MR Date" },
                { "data": "requestor", "title": "Requestor" },
                { "data": "pmname", "title": "Product Manager" },
                { "data": "actiondate", "title": "Date Action" },
                { "data": "diff", "title": "Days" },
            ];
            generateTable(rejectedElem, rejectedAjaxConfig, rejectedColumns, rejectedColumnsOrder);



            //List of MRs that has failed customer's Required Date
            var FailReuiredElem = $('#tblFailed');
            var failedAjaxConfig = {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "error": function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(textStatus);
                },
                "url": "Default.aspx/GetListOfMRFailedDeliveryDateByUserNumId",
                "data": function (data) { return JSON.stringify({ 'CompanyID': CoyID, 'UserID': UserID }); },
                "dataSrc": function (json) {
                    json = json.hasOwnProperty('d') ? json.d : json;
                    return json;
                }
            };
            var failedColumnsOrder = [[2, "desc"]];
            var failedColumns = [
                { "data": null, "title": "No." },
                {
                    "data": "mrno",
                    "title": "MR No.",
                    "render": function (data, type, row) {
                        return "<a name='Edit' href=\"../Products/Products/AddEditMaterialRequisition.aspx?CurrentLink=Sales&CoyID=" + CoyID + "&MRNo=" + data + "\" >" + data + "</a>";
                    }
                },
                { "data": "mrdate", "title": "MR Date" },
                { "data": "requestor", "title": "Requestor" },
                { "data": "customeraccountcode", "title": "Account Code" },
                { "data": "customername", "title": "Account Name" },
                { "data": "requireddate", "title": "Required Date" },
                { "data": "diff", "title": "Days" },
            ];
            generateTable(FailReuiredElem, failedAjaxConfig, failedColumns, failedColumnsOrder);


            $(window).bind('beforeunload', function () {
                console.log("aborting");
                $("#tblMRApproval").DataTable().context[0].jqXHR.abort();
                $("#tblRejected").DataTable().context[0].jqXHR.abort();
                $("#tblFailed").DataTable().context[0].jqXHR.abort();
                return
            });
        });
    </script>
</asp:Content>