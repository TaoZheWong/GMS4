/*
=========================================================================================
Script          : MaterialSearch
Description     : Search function
Date Created    : 2016-06-20
Date Modified   : - 
=========================================================================================
*/

$(function() {
    
    $( "#tblSearch" ).hide(); 
   
    $( "#btnSearch" ).button().on( "click", function() {
         DisplayCostEstimateList();
    });
    
    $( "#btnAdd" ).button().on( "click", function() {
        window.location.href = 'AddEditCostEstimate.aspx?CurrentLink=' + getCurrentLink() + '&CoyID=' + getCoyID();
    });
   
    
    $( "#btnMaterialSave" ).button().on( "click", function() {
        SaveMaterialInfo(this);
    }); 
    
   
    $('#txtCreatedDateFrom').datepicker({
        format: 'yyyy-mm-dd', autoclose: 1, ignoreReadonly: true
    });
    $('#txtCreatedDateTo').datepicker({
        format: 'yyyy-mm-dd', autoclose: 1, ignoreReadonly: true
    });
   
    var d = new Date();
    d.setMonth(d.getMonth() - 1);
    d = $.datepicker.formatDate('yy-mm-dd', d);
   
    var date = $.datepicker.formatDate('yy-mm-dd', new Date());
    $("#txtCreatedDateFrom").val(d);
    $("#txtCreatedDateTo").val(date);
    
    $("#txtAccountCode").on('keyup', function(){
        GetFullAccountList(this);
    });
    
    AllowChanges();
    
    $("#txtCEStatusName").on('mousedown', function(){
    var statusid = [
        {
            key: 1,
            sid: "1",
            label: 'Draft',
            value: 'Draft'
        },
        {
            key: 1,
            sid: "2",
            label: 'Pending for Approval',
            value: 'Pending for Approval'
        },
        {
            key: 1,
            sid: "3",
            label: 'Approved',
            value: 'Approved'
        },
        {
            key: 1,
            sid: "4",
            label: 'Converted',
            value: 'Converted'
        },
        {
            key: 1,
            sid: "5",
            label: 'Rejected',
            value: 'Rejected'
        },
        {
            key: 1,
            sid: "X",
            label: 'Cancelled',
            value: 'Cancelled'
        }];
        
         $("#txtCEStatusName").autocomplete({
            source: statusid,
            select: function (e, i) {
                $('#txtCEStatusID').val(i.item.sid);    
                
            },
            minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
        }).click(function() {
            $(this).autocomplete('search', '')
        });
        
    });
    
    $("input[id*='txtEngineerName']").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "CostEstimateSearch.aspx/GetEngineerList",
                data: "{ 'engineer': '" + request.term + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function(data) {
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
                                    label: item.EngineerID + ' - ' + item.EngineerName,
                                    value: item.EngineerName,
                                    text: item.EngineerName
                                }
                            })
                        )
                    }
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        select: function (e, i) {
            $("input[id*=txtEngineerName]").attr("value", i.item.value);
            //$("input[id*=txtEngineerID]").attr("value", i.item.text);
        },
        minLength: 2    // MINIMUM 1 CHARACTER TO START WITH.
    });
});

function GetFullAccountList(item)
{
    $(item).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "CostEstimateSearch.aspx/GetAccountList",
                data: "{'CompanyID':'"+getCoyID()+"', 'account': '" + item.value + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function(data) {
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
            $('#txtAccountName').val(i.item.text);    
        },
        minLength: 3    // MINIMUM 1 CHARACTER TO START WITH.
    });
     
}

function AllowChanges() {
    var retval = "disabled";
    $("#btnAdd").attr("disabled", true);
    $("form").find("#MaterialRecordDetail :input").attr("disabled", true);
    $("form").find("#btnGroup :input").attr("disabled", true);
    CheckAccess(function (data) {
        var AccessViewCE = data[0].AccessViewCE;
        var AccessAddEditCE = data[0].AccessAddEditCE;
        var AccessEngineeringAdmin = data[0].AccessEngineeringAdmin;
        var EngineerName = data[0].EngineerName;

        if (AccessEngineeringAdmin == "0" &&  EngineerName !="") {
            retval = "";
            $("#txtEngineerName").val(EngineerName).attr("disabled",true);
        }

        if (AccessAddEditCE == "1" || AccessEngineeringAdmin == "1") {
            $("#btnAdd").attr("disabled", false);
        }
    });
}
/*
-----------------------------------------------------------------------------------------
Function Name   : CheckAccess
Description     : Check for user access
Date Created    : 2016-06-20
Date Modified   : - 

            list2.Add("AccessAddEdit", AccessAddEdit.ToString());
            list2.Add("AccessViewAll", AccessViewAll.ToString());
            list2.Add("AccessAdmin", AccessAdmin.ToString());
            list2.Add("UserID", UserID.ToString());
-----------------------------------------------------------------------------------------
*/
function CheckAccess(callback){
    
    var datastr = "{'CompanyId': '" + getCoyID() + "'" +
                 ",'DocNo':''" +
                 ",'UserID': " + getUserID() +
                 "}";

    $.ajax({
        async: false,
        type: "POST",
        url: "CostEstimateSearch.aspx/CheckUserAccess",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: datastr,
        success: function (data) {
            callback(data);
        },
        error: function (xhr, textstatus, error) {
            SetModalMessage(textstatus, "");
        }
    });
}

/*
-----------------------------------------------------------------------------------------
Function Name   : DisplayCostEstimateList
Description     : Get and display data from tbCostEstimate based on search input
Date Created    : 2016-06-20
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/

function DisplayCostEstimateList(){
    //String search input into an array
    var datastr = "{'CompanyId': '" + getCoyID() + "'" +
                  ",'ceid':'%" + $("#txtCEID").val() + "%'" +
                  ",'accountcode':'%" + $("#txtAccountCode").val() + "%'" +
                  ",'accountname':'%" + $("#txtAccountName").val() + "%'" +
                  ",'cestatusid':'" + $("#txtCEStatusID").val() + "'" +
                  ",'createddatefrom':'" + $("#txtCreatedDateFrom").val() + "'" +
                  ",'createddateto':'" + $("#txtCreatedDateTo").val() + "'" +
                  ",'engineername':'%" + $("#txtEngineerName").val() + "%'" +
                  ",'UserID': " + getUserID() +
                 "}";

    $('#tblSearch').show();
    
    // Initialize tblSearch Datatable
    var table   = $('#tblCostEstimate').DataTable( {
        "ajax": {
            "dataType"      : "json",
            "contentType"   : "application/json; charset=utf-8",
            "type"          : "POST",
            "url"           : "CostEstimateSearch.aspx/GetCostEstimate",
            "data"          : function (d){  return datastr;  },
            "dataSrc"       : function (json) {
                                return json;
                              }   
        },
        "responsive"        : true,
        "bDestroy"          : true,
        "jQueryUI"          : true,
        "language"          : {
                                "emptyTable":     "No results found!"
                              },
        "rowId"             : "CEID",             
        "columns"           : [
                                { 
                                    "data"      : "CEID", 
                                    "title"     : "C/E No.",
                                    "render"    : function(data, type, row, meta){
                                                    if(type === 'display'){
                                                       return $('<a>')
                                                          .attr('href', "AddEditCostEstimate.aspx?CurrentLink="+getCurrentLink()+"&CoyID="+row.CoyID+"&&CEID="+data)
                                                          .text(data)
                                                          .wrap('<div></div>')
                                                          .parent()
                                                          .html();
                                                    } else {
                                                       return data;
                                                    }
                                                 }
                                },
                                { 
                                    "data"      : "AccountCode", 
                                    "title"     : "Customer Code"
                                },
                                { 
                                    "data"      : "AccountName", 
                                    "title"     : "Customer Name" 
                                },
                                { 
                                    "data"      : "EngineerName", 
                                    "title"     : "Engineer Name" 
                                },
                                { 
                                    "data"      : "CurrencyCode", 
                                    "title"     : "Currency" 
                                },
                                { 
                                    "data"      : "GrandTotal", 
                                    "title"     : "Total Amount" 
                                },
                                { 
                                    "data"      : "CEStatusName", 
                                    "title"     : "Status" 
                                }
                              ], 
        "columnDefs"        : [ {
                                    "className" : 'all',
                                    "orderable" : false,
                                    "targets"   :   -1
                              } ],
        "order"             : [[ 0, "desc" ]]
    } );
}

