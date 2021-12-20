<%@ Page Language="C#" MasterPageFile="~/Common/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GMSWeb.Claim.Default" Title="Claim Page" %>

<%@ MasterType VirtualPath="~/Common/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <input type="hidden" id="hidCoyID" runat="server" value="" />
    <input type="hidden" id="hidUserID" runat="server" value="" />
    <input type="hidden" id="hidCurrentLink" runat="server" value="" />

    <a name="TemplateInfo"></a>

    <div data-ng-controller="ClaimDefaultController as claim" data-ng-cloak="">
        <%--Create new claim model--%>
        <div id="claimForm" class="modal fade" role="dialog" data-backdrop="static">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">New Claim</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="control-label" for="feed_title">Claim Date</label>
                            <div class="input-group date">
                                <input type="text" class="form-control datepicker" autocomplete="off" id="claim_date" readonly required />
                                <span class="input-group-addon"><i class="ti-calendar"></i></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label" for="claim_desc">Claim Purpose</label>
                            <select class="form-control" name="claim_desc" id="claim_desc">
                                <option data-ng-repeat="claimpurpose in claim.typeList | filter: {EntertainmentType: 'Purpose'}" value="{{claimpurpose.EntertainmentOptions}}">{{claimpurpose.EntertainmentOptions}}</option>
                            </select>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-primary" id="create_claim_btn">Create</button>
                    </div>
                </div>
            </div>
        </div>


        <h1 class="page-header">Claim 
        <br />
            <small>Submit your claim here</small>
        </h1>

        <div class="panel panel-primary">
            <div class="panel-heading">
                <i class="ti-search"></i>
                Search Filter
            </div>
            <div class="panel-body row">
                <div class="m-t-20">

                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Status</label>
                        <select id="status_selected" class="form-control">
                            <option selected="selected" value="-1">All</option>
                            <option value="0">Open</option>
                            <option value="1">Pending</option>
                            <option value="2">Approved</option>
                            <option value="3">Rejected</option>
                        </select>
                    </div>

                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Claim Date From</label>
                        <div class="input-group date">
                            <input type="text" class="form-control datepicker" autocomplete="off" id="claim-date-from" value="" />
                            <span class="input-group-addon"><i class="ti-calendar"></i></span>
                        </div>
                    </div>

                    <div class="form-group col-lg-3 col-md-6 col-sm-12">
                        <label class="control-label">Claim Date To</label>
                        <div class="input-group date">
                            <input type="text" class="form-control datepicker" autocomplete="off" id="claim-date-to" />
                            <span class="input-group-addon"><i class="ti-calendar"></i></span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel-footer clearfix">
                <a href="#" class="btn btn-default pull-right m-r-10" onclick="javascript:printClaim()">Print Claim</a>
                <a href="#" class="btn btn-primary pull-right m-r-10" onclick="javascript:searchResult('0')">Search</a>
                <div id="btnAdd" runat="server">
                    <a href="#" class="btn btn-default pull-right m-r-10" data-toggle="modal" data-target="#claimForm">New Claim</a>
                </div>
            </div>
            <div data-ng-if="claim.allowApproveReject" class="panel-footer clearfix">
                <a href="#" class="btn btn-success pull-right m-r-10" data-ng-if="claim.allowApproveReject" onclick="javascript:searchResult('1')">Pending Claim</a>
                <a href="#" class="btn btn-default pull-right m-r-10" data-ng-if="claim.allowApproveReject" onclick="javascript:searchResult('2')">Search Claim (Finance)</a>
            </div>
        </div>

        <div class="panel panel-default hidden" id="claims-list-section">
            <div class="panel-heading">
                <div class="panel-heading-btn">
                </div>
                <h4 class="panel-title">List of claims
                </h4>
            </div>
            <div class="panel-body">
                <table id="tblClaims" class="table table-striped table-bordered dataTable no-footer" width="100%"></table>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        var CoyID = getCoyID();
        var UserID = $("#<%=hidUserID.ClientID%>").val();
        var CurrentLink = $("<%=hidCurrentLink.Value%>");

        function generateTable(elem, ajaxConfig, columns, order) {

            if ($.fn.dataTable.isDataTable(elem)) {
                elem.dataTable().fnDestroy();
            }


            var t = elem.DataTable({
                "ajax": ajaxConfig,
                "order": order,
                "pageLength": 20,
                "processing": true,
                "responsive": true,
                "bLengthChange": false,
                "bDestroy": false,
                "jQueryUI": true,
                "searching": false,
                "language": { "emptyTable": "No results found!" },
                "columns": columns
            });

            t.on('order.dt search.dt', function () {
                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        }

        //condition = 1 will get pending claims from all user, condition = 0 is default
        //condition = 2 will get approved claims from all user
        var searchResult = function (condition) {
            $("#claims-list-section").removeClass("hidden");

            var elem = $('#tblClaims');

            var ajaxConfig = {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "Default.aspx/GetClaims",
                "data": function (data) {
                    return JSON.stringify({
                        'CompanyID': CoyID,
                        'UserID': UserID,
                        'Status': $("#status_selected").val(),
                        'ClaimDateFrom': $("#claim-date-from").val(),
                        'ClaimDateTo': $("#claim-date-to").val(),
                        'Condition': condition
                    });
                },
                "error": function (XMLHttpRequest, textStatus, errorThrown) {
                    //console.log(textStatus);
                },
                "dataSrc": function (result) {
                    console.log(result);
                    result = result.hasOwnProperty('d') ? result.d : result;
                    return result.Params.data;
                }
            }

            var columns = [
                { "data": null, "title": "No." },
                { "data": "ClaimNo", "title": "Claim No." },
                { "data": "UserRealName", "title": "Employee Name" },
                { "data": "Description", "title": "Description" },
                { "data": "ConvDate", "title": "Claim Date" },
                { "data": "StatusName", "title": "Status" },
                {
                    "data": null,
                    "title": "Action",
                    "className": "all",
                    "render": function (data, type, row) {
                        var viewButton = "<a class='btn btn-primary btn-xs' href='Detail.aspx?CurrentLink=" + CurrentLink.selector + "#!?id=" + row.ClaimID + "' name='Edit' onclick=\"\">View</a>";
                        var viewButton2 = "<div class='btn-group'><button type='button' class='btn btn-primary btn-xs dropdown-toggle' data-toggle='dropdown'>Action</button><ul class='dropdown-menu' role='menu'><li><a class='btn-xs' href='Detail.aspx?CurrentLink=" + CurrentLink.selector + "#!?id=" + row.ClaimID + "' name='Edit' onclick=\"\">View</a></li><li ><a class='btn-xs' href='/GMS4/Finance/BankFacilities/PdfReportViewer.aspx?ISCLAIM=YES&ISTRAVEL=YES&DATE=" + row.ClaimDate + "&ID=" + row.ClaimID + "'  rel='noopener noreferrer' target='_blank'>Print</a></li></ul></div>";
                        var editButton = "<a class='btn btn-primary btn-xs' href='Detail.aspx?CurrentLink=" + CurrentLink.selector + "#!?id=" + row.ClaimID + "' name='Edit' onclick=\"\">Edit</a>";
                        if (row.Status > 0)
                            if (row.Description == "Travel") {
                                if (row.Status == 1) {
                                    return viewButton2;
                                } else {
                                    return viewButton;
                                }
                            } else {
                                return viewButton;
                            }
                        else
                            return editButton;
                    }
                }
            ]
            generateTable(elem, ajaxConfig, columns, [[1, "asc"]]);
        }

        $("#create_claim_btn").on("click", function () {

            var date = $("#claim_date").val();
            var desc = $("#claim_desc").val();

            if (!date || date == "") {
                alert("Please select the claim date");
                return;
            } else {

                var postObj = {
                    CompanyID: globalCoyID,
                    UserID: globalUserID,
                    ClaimDate: date,
                    Desc: desc
                }

                $.ajax({
                    url: "Default.aspx/CreateClaim",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(postObj),
                    success: function (data) {
                        data = data.hasOwnProperty('d') ? data.d : data;
                        //success return
                        $("#claim_desc").val("");
                        $("#claim_date").val("");
                        searchResult(0);
                        $("#claimForm").modal("hide");
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        //alert('Failed to retrieve detail.');
                    }
                });
            }

        });

        function printClaim() {
            // the true solutions :)
            var datefrom = $("#claim-date-from").val();
            var dateto = $("#claim-date-to").val()

            if (datefrom == "" || dateto == "" || !datefrom || !dateto) {
                alert("Please enter your date.");
            } else {
                jsOpenOperationalReport('/Finance/BankFacilities/PdfReportViewer.aspx?ISCLAIM=YES&DATEFROM=' + $("#claim-date-from").val() + '&DATETO=' + $("#claim-date-to").val());
            }
        }

        function printTravelClaim(id, date) {
            jsOpenOperationalReport('/Finance/BankFacilities/PdfReportViewer.aspx?ISCLAIM=YES&ISTRAVEL=YES&DATE=' + date + '&ID=' + id);
        }

        $(document).ready(function () {
            $(".administration-menu").addClass("active expand");
            $(".sub-claim").addClass("active");
        });
    </script>
    <script type="text/javascript" src="claim_detail.js"></script>
    <script src="../new_assets/js/angular/factory/claim.js"></script>
    <script src="../new_assets/js/angular/factory/commonUtil.js"></script>
    <script src="../new_assets/js/angular/directive/autocomplete.js"></script>
</asp:Content>

