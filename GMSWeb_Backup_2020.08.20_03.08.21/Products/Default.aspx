<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GMSWeb.Products.Default" Title="Products Page" %>
<%@ MasterType VirtualPath="~/Common/Site.Master"%> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<a name="TemplateInfo"></a>

<input type="hidden" id="hidCoyID" runat="server" value="" />	
<input type="hidden" id="hidUserID" runat="server" value="" />

<ul class="breadcrumb pull-right">
    <li class="active">Product</li>
</ul>
<h1 class="page-header">Product <br /><small>Main page for Products.</small></h1>

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
            List of MRs requires your Product Managers' approval
        </h4>
    </div>
    <div class="panel-body">
        <table id="tblPmApproval" class="table table-striped table-bordered dataTable no-footer" width="100%"></table>
    </div>
</div>

<div class="panel panel-default">
    <div class="panel-heading">
        <div class="panel-heading-btn">
            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
        </div>
        <h4 class="panel-title">
            List of MRs without ETD Infomation
        </h4>
    </div>
    <div class="panel-body">
        <table id="tblWithoutEtd" class="table table-striped table-bordered dataTable no-footer" width="100%"></table>
    </div>
</div>

<div class="panel panel-default">
    <div class="panel-heading">
        <div class="panel-heading-btn">
            <a href="javascript:;" class="btn" data-toggle="panel-expand"><i class="glyphicon glyphicon-resize-full"></i></a>
        </div>
        <h4 class="panel-title">
            List of Rejected MRs 
        </h4>
    </div>
    <div class="panel-body">
        <table id="tblRejectedMR" class="table table-striped table-bordered dataTable no-footer" width="100%"></table>
    </div>
</div>
    
                                
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        var CoyID = getCoyID();
        var UserID = $("#<%=hidUserID.ClientID%>").val();

        function generateTable(elem, ajaxConfig, columns,order) {
            var t = elem.DataTable({
                "ajax": ajaxConfig,
                "pageLength": 5,
                "order": order,
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
                "error": function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(textStatus);
                },
                "url": "Default.aspx/GetListOfMRRequiresApprovalByUserNumId",
                "data": function (data) { return JSON.stringify({ 'CompanyID': CoyID, 'UserID': UserID }); },
                "dataSrc": function (json) {
                    return json;
                }
            }
            var mrColumnsOrder = [[2, "desc"]];
            var mrColumns = [
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
                { "data": "routeddate", "title": "Date Routed" },
                { "data": "diff", "title": "Days" },
                { "data": "VendorName", "title": "VendorName" },
                { "data": "Purchaser", "title": "Purchaser" },
            ]
            generateTable(MrApprovalElem, mrAjaxConfig, mrColumns, mrColumnsOrder);


            //List of MRs requires your Product Managers' approval
            var pmApprovalElem = $('#tblPmApproval');
            var pmApprovalAjaxConfig = {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "error": function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(textStatus);
                },
                "url": "Default.aspx/GetListOfMRRequiresProductManagerApprovalByUserNumId",
                "data": function (data) { return JSON.stringify({ 'CompanyID': CoyID, 'UserID': UserID }); },
                "dataSrc": function (json) {
                    return json;
                }
            };
            var pmApprovalColumnssOrder = [[2, "desc"]];
            var pmApprovalColumns = [
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
                { "data": "routeddate", "title": "Date Action" },
                { "data": "diff", "title": "Days" },
            ];
            generateTable(pmApprovalElem, pmApprovalAjaxConfig, pmApprovalColumns, pmApprovalColumnssOrder);



            //List of MRs that has failed customer's Required Date
            var tblWithoutEtdElem = $('#tblWithoutEtd');
            var tblWithoutEtdAjaxConfig = {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "error": function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(textStatus);
                },
                "url": "Default.aspx/GetListOfMRWithoutETDInfoByUserNumId",
                "data": function (data) { return JSON.stringify({ 'CompanyID': CoyID, 'UserID': UserID }); },
                "dataSrc": function (json) {
                    return json;
                }
            };
            var tblWithoutEtdColumnsOrder = [[2, "desc"]];
            var tblWithoutEtdColumns = [
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
                { "data": "routeddate", "title": "Routed" },
                { "data": "diff", "title": "Days" },
                { "data": "VendorName", "title": "Vendor Name" },
                { "data": "Purchaser", "title": "Purchaser" },
            ];
            generateTable(tblWithoutEtdElem, tblWithoutEtdAjaxConfig, tblWithoutEtdColumns, tblWithoutEtdColumnsOrder);

            //List of rejected MRs 
            var rejectedMRElem = $('#tblRejectedMR');
            var rejectedMRAjaxConfig = {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "error": function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(textStatus);
                },
                "url": "Default.aspx/GetListOfRejectedMRByUserNumId",
                "data": function (data) { return JSON.stringify({ 'CompanyID': CoyID, 'UserID': UserID }); },
                "dataSrc": function (json) {
                    return json;
                }
            };
            var rejectedMRColumnssOrder = [[2, "desc"]];
            var rejectedMRColumns = [
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
                { "data": "diff", "title": "Days" },
            ];
            generateTable(rejectedMRElem, rejectedMRAjaxConfig, rejectedMRColumns, rejectedMRColumnssOrder);




            $(window).bind('beforeunload', function () {
                console.log("aborting");
                $("#tblMRApproval").DataTable().context[0].jqXHR.abort();
                $("#tblPmApproval").DataTable().context[0].jqXHR.abort();
                $("#tblWithoutEtd").DataTable().context[0].jqXHR.abort();
                return
            });
        });
    </script>
</asp:Content>