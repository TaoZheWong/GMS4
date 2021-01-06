       
/* Search Project JS Logs   */
/* 
Added Date      : 03/06/2016
Modified Date   :
Changes         :   
*/


//DatePickerFunction
$(function() {
    $( "#btnSearch" ).button().on( "click", function() {
         DisplayProjectList();
    }); 
    $( "#btnAdd" ).button().on( "click", function() {
        window.location.href = "EditProject.aspx?CurrentLink=" + getCurrentLink() + "&CoyID=" + getCoyID();
    }); 
});

$(document).ready(function() {
   $( "#tblSearch" ).hide(); 
    
   $('#txtCreatedDateFrom').datepicker({ format: 'yyyy-mm-dd', autoclose: 1 });
   $('#txtCreatedDateTo').datepicker({ format: 'yyyy-mm-dd', autoclose: 1 });
   $('#txtCommencementDateFrom').datepicker({ format: 'yyyy-mm-dd', autoclose: 1 });
   $('#txtCommencementDateTo').datepicker({ format: 'yyyy-mm-dd', autoclose: 1 }); 
   $('#txtClosingDateFrom').datepicker({ format: 'yyyy-mm-dd', autoclose: 1 });
   $('#txtClosingDateTo').datepicker({ format: 'yyyy-mm-dd', autoclose: 1 });
   
   $.fn.dataTable.tables( {visible: true, api: true} ).columns.adjust();
   
   var d = new Date();
   d.setMonth(d.getMonth() - 1);
   d = $.datepicker.formatDate('yy-mm-dd', d);
   
   var date = $.datepicker.formatDate('yy-mm-dd', new Date());
   $("#txtCreatedDateFrom").val(d);
   $("#txtCreatedDateTo").val(date);
   AllowChanges();
    //Autocomplete
    $("input[id*='txtProjectNo']").autocomplete({
      source: function(request, response) {
            $.ajax({
                url: "ProjectSearch.aspx/GetProjectNo",
                data: "{'CompanyId' : '" + getCoyID() + "' , 'projectno': '" + request.term + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function (data) {
                    data = data.hasOwnProperty('d') ? data.d : data;
                    if(!data){
                        var result = [{
                            label: 'No matches found', 
                            value: response.term
                        }];
                        response(result);
                    }
                    else{
                        response(
                            $.map(data, function(item) {
                                return { 
                                    label: item.ProjectNo,
                                    value: item.ProjectNo
                                }
                            })
                        );
                    }
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        minLength: 2   // MINIMUM 8 CHARACTER TO START WITH.
    });
    
    $("input[id*='txtPrevProjectNo']").autocomplete({
      source: function(request, response) {
            $.ajax({
                url: "ProjectSearch.aspx/GetPrevProjectNo",
                data: "{ 'CompanyId' : '" + getCoyID() + "' , 'prevprojectno': '" + request.term + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function (data) {
                    data = data.hasOwnProperty('d') ? data.d : data;
                    if(!data){
                        var result = [{
                            label: 'No matches found', 
                            value: response.term
                        }];
                        response(result);
                    }
                    else{
                        response(
                            $.map(data, function(item) {
                                return { 
                                    label: item.PrevProjectNo,
                                    value: item.PrevProjectNo
                                }
                            })
                        );
                    }
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        minLength: 2   // MINIMUM 8 CHARACTER TO START WITH.
    });
    
    $("input[id*='txtAccountCode']").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "ProjectSearch.aspx/GetAccountList",
                data: "{ 'CompanyId' : '" + getCoyID() + "' , 'account': '" + request.term + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function (data) {
                    data = data.hasOwnProperty('d') ? data.d : data;
                    if(!data){
                        var result = [{
                            label: 'No matches found', 
                            value: response.term
                        }];
                        response(result);
                    }
                    else{
                        response(
                            $.map(data, function(item) {
                                return { 
                                    label: item.AccountCode + ' - ' + item.AccountName, 
                                    value: item.AccountCode,
                                    text : item.AccountName
                                }
                            })
                        );
                    }
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        select: function (e, i) {
            $("input[id*='txtAccountName']").val(i.item.text);
        },
        minLength: 2    // MINIMUM 1 CHARACTER TO START WITH.
    });
    
    $("input[id*='txtAccountName']").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "ProjectSearch.aspx/GetAccountList",
                data: "{ 'CompanyId' : '" + getCoyID() + "' , 'account': '" + request.term + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function (data) {
                    data = data.hasOwnProperty('d') ? data.d : data;
                    if(!data){
                        var result = [{
                            label: 'No matches found', 
                            value: response.term
                        }];
                        response(result);
                    }
                    else{
                        response(
                            $.map(data, function(item) {
                                return { 
                                    value: item.AccountName,
                                    text : item.AccountCode
                                }
                            })
                        );
                    }
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        select: function (e, i) {
            $("input[id*='txtAccountCode']").val(i.item.text);
        },
        minLength: 2    // MINIMUM 1 CHARACTER TO START WITH.
    });
    
    $("input[id*='txtEngineer']").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "ProjectSearch.aspx/GetEngineerList",
                data: "{'CompanyId' : '" + getCoyID() + "' ,  'engineer': '" + request.term + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function (data) {
                    data = data.hasOwnProperty('d') ? data.d : data;
                    if(!data){
                        var result = [{
                            label: 'No matches found', 
                            value: response.term
                        }];
                        response(result);
                    }
                    else{
                        response(
                            $.map(data, function(item) {
                                return { 
                                    label: item.EngineerName,
                                    value: item.EngineerName
                                }
                            })
                        );
                    }
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        minLength: 2    // MINIMUM 1 CHARACTER TO START WITH.
    });
    
    $("input[id*='txtSalesPerson']").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "ProjectSearch.aspx/GetSalesPersonList",
                data: "{'CompanyId' : '" + getCoyID() + "' ,  'salesperson': '" + request.term + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function (data) {
                    data = data.hasOwnProperty('d') ? data.d : data;
                    if(!data){
                        var result = [{
                            label: 'No matches found', 
                            value: response.term
                        }];
                        response(result);
                    }
                    else{
                        response(
                            $.map(data, function(item) {
                                return { 
                                    label: item.SalesPersonID +' - '+ item.SalesPersonName,
                                    value: item.SalesPersonID
                                }
                            })
                        );
                    }
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        minLength: 2    // MINIMUM 1 CHARACTER TO START WITH.
    });

    $.ajax({
        url: "ProjectSearch.aspx/GetStatusList",
        data: "{}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function(data) { return data; },
        success: function (data) {
            data = data.hasOwnProperty('d') ? data.d : data;
            $.each(data, function (key, value) {
                $("select[id*='txtStatusID']").append($("<option></option>").val(value.StatusID).html(value.StatusName));
            });
            $("select[id*='txtStatusID'] option[value='']").attr("selected",true);
        }
    });
}); 

function DisplayProjectList() {
    var UserID = getUserID();
    var datastr = "{'CoyID': '" + getCoyID() + "'" +
                  ",'ProjectNo':'" + $('#txtProjectNo').val() + "'" +
                  ",'PrevProjectNo':'" + $('#txtPrevProjectNo').val() + "'" +
                  ",'AccountCode':'" + $('#txtAccountCode').val() + "'" +
                  ",'AccountName':'" + $('#txtAccountName').val() + "'" +
                  ",'CreatedDateFrom':'" + $('#txtCreatedDateFrom').val() + "'" +
                  ",'CreatedDateTo':'" + $('#txtCreatedDateTo').val() + "'" +
                  ",'ClosingDateFrom':'" + $('#txtClosingDateFrom').val() + "'" +
                  ",'ClosingDateTo':'" + $('#txtClosingDateTo').val() + "'" +
                  ",'CommencementDateFrom':'" + $('#txtCommencementDateFrom').val() + "'" +
                  ",'CommencementDateTo':'" + $('#txtCommencementDateTo').val() + "'" +
                  ",'CustomerPo':'" + $('#txtCustomerPO').val() + "'" +
                  ",'EngineerId':'" + $('#txtEngineer').val() + "'" +
                  ",'SalespersonId':'" + $('#txtSalesPerson').val() + "'" +
                  ",'IsBillable':'" + $('#txtIsBillable').val() + "'" +
                  ",'IsProgressiveClaim':'" + $('#txtIsProgressiveClaim').val() + "'" +
                  ",'StatusId':'" + $('#txtStatusID').val() + "'" +
                  ",'UserID': " + getUserID() +
                  "}";


    var a = $('#tblSearch').DataTable({
        "ajax": {
            "dataType": "json",
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "ProjectSearch.aspx/GetProjectList",
            "data": function (data) { return datastr; },
            "dataSrc": function (json) {
                json = json.hasOwnProperty('d') ? json.d : json;
                return json;
            }
        },
        "responsive": true,
        "stateSave": false,
        "bDestroy": true,
        "filter": false,
        "jQueryUI": true,
        "dom": 'Bfrtip',
        "buttons": [],
        "language": {
            "emptyTable": "No results found!"
        },
        "rowId": "MRNo",
        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null
            },
            {
                "data": "ProjectNo",
                "title": "Project No.",
                "render": function (data, type, row, meta) {
                    if (type === 'display') {
                        return $('<a>')
                           .attr('href', "EditProject.aspx?CurrentLink=" + getCurrentLink() + "&CoyID=" + getCoyID() + "&ProjectNo=" + data)
                           .text(data)
                           .wrap('<div></div>')
                           .parent()
                           .html();

                    } else {
                        return data;
                    }
                }
            },
            { "data": "PrevProjectNo", "title": "Prev Project No" },
            { "data": "AccountCode", "title": "Account Code" },
            { "data": "AccountName", "title": "Account Name" },
            { "data": "CustomerPO", "title": "Customer PO" },
            { "data": "IsBillable", "title": "Billable" },
            { "data": "IsProgressive", "title": "Progressive Claim" },
            { "data": "CurrencyCode", "title": "Currency Code" },
            { "data": "TotalBillableAmt", "title": "Billable Amt"},
            { "data": "TotalCE", "title": "Estimated Cost" },
            { "data": "TotalPrjCost", "title": "Project Cost" },
            { "data": "TotalOwnCost", "title": "Own Cost" },
            { "data": "EngineerName", "title": "Engineer" },
            { "data": "StatusName", "title": "Status" },
            { "data": "ClosingDate", "title": "Closing Date" }
        ],
        "columnDefs": [{
            "className": 'all',
            "orderable": false,
            "targets": -1
        }],
        "rowCallback": function (row, data, iDisplayIndex) {
            var info = a.page.info();
            var page = info.iPage;
            var length = info.length;
            var index = (page * length + (iDisplayIndex + 1));
            $('td[class="details-control"]', row).html(index);
        },
        "fnCreatedRow": function (row, data, index) {
            $('td', row).eq(0).html(index + 1);
        },
        "order": [[1, "desc"]]
    });


    $('#tblSearch').show();

}


function GetEngineerDetail() {

    $.ajax({
        type: "POST",
        url: "AddEditCostEstimate.aspx/GetEngineerInfo",
        contentType: "application/json; charset=utf-8",
        data: '{"CompanyId": "' + getCoyID() + '", "UserID":"' + getUserID() + '"}',
        dataType: "json",
        success: function (data) {
            data = data.hasOwnProperty('d') ? data.d : data;
            $.map(data, function (item) {
                $("#txtEngineerID").val(item.EngineerID);
                $("#txtEngineerName").val(item.EngineerName);
            });
        },
        error: function (xhr, textstatus, error) {
            var r = jQuery.parseJSON(xhr.responseText);
            SetModalMessage(r.Message, '');
        }
    });
}


function AllowChanges() {
    CheckAccess(function (data) {
        var AccessEngineering = data[0].AccessEngineering;
        var AccessEngineeringAdmin = data[0].AccessEngineeringAdmin;
        var EngineerName = data[0].EngineerName;

        if (AccessEngineeringAdmin == "0" && EngineerName != "") {
            $("#txtEngineer").val(EngineerName).attr("disabled", true);
        }

        else if (AccessEngineeringAdmin == "0" && EngineerName == "") {
            $("#btnSearch").val("").attr("disabled", true);
        }

        if (AccessEngineeringAdmin == "1")
        {
            $("#txtEngineer").val("").attr("disabled", false);
            $("#btnSearch").val("").attr("disabled", false);
        }
    });
}

function CheckAccess(callback) {

    var datastr = "{'CompanyId': '" + getCoyID() + "'" +
                 ",'DocNo':''" +
                 ",'UserID': " + getUserID() +
                 "}";

    $.ajax({
        async: false,
        type: "POST",
        url: "ProjectSearch.aspx/CheckUserAccess",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: datastr,
        success: function (data) {
            data = data.hasOwnProperty('d') ? data.d : data;
            callback(data);
        },
        error: function (xhr, textstatus, error) {
            SetModalMessage(textstatus, "");
        }
    });
}