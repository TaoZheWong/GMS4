$(document).ready(function() {
     
    if (getProjectNo() != "" || getProjectNo() != null)
    {
        MaterialRequisition();
        LaborCost();
        Miscellaneous();
        GetTotalInfo();
    }
    $('#MRTab').click(function () {
        MaterialRequisition();
    });
    $('#LaborTab').click(function () {
        LaborCost();
    });
    $('#MiscTab').click(function () {
        Miscellaneous();
    });

    $("#btnMRSubmit").button().on("click", function () {
        SaveItem(this);
    });

    $('#tblLaborCost tbody').on('click', 'button', function () {
        var action = "";
        var isactive = "false";
        var obj = {};
        obj["ProjectNo"] = $("#txtProjectNo").val();
        obj["LCID"] = this.value;
        obj["CompanyId"] = getCoyID();
        obj["UserID"] = getUserID();
        $("#" + this.value + " td").find("input").each(function () {
            obj[this.name] = this.value;
        });
        $(this).parents('tr').find("input").each(function () {
            obj[this.name] = this.value;
        });

        if (this.name == "Delete")
            DeleteLCDetail(obj["ProjectNo"], obj["LCID"]);
        else {
            var ID = this.value;
            if (ID.indexOf("New") >= 0) {
                action = "InsertLaborCost";
                isactive = "true";
            }
            else {
                action = "UpdateLaborCost"
                isactive = "true";
            }
        }

        if (isactive == "true") {
            $.ajax({
                type: "POST",
                url: "ProjectCost.aspx/" + action,
                data: '{Info: ' + JSON.stringify(obj) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    alert("Success");
                    LaborCost();
                    GetTotalInfo();
                }
            });
        }

    });

    $('#tblMiscellaneous tbody').on( 'click', 'button', function () {
        
        var action = "";
        var isactive = "false";
        var obj = {};
        obj["ProjectNo"] = $("#txtProjectNo").val();
        obj["OCID"] = this.value;
        obj["CompanyId"] = getCoyID();
        obj["UserID"] = getUserID();
        $("#"+this.value+" td").find("input").each(function() {
            obj[this.name]=this.value;
        });
        $(this).parents('tr').find("input").each(function() {
            obj[this.name]=this.value;
        });
        
       
        if (this.name=="Delete")
            DeleteOCDetail(obj["ProjectNo"], obj["OCID"]);
        else {
           var ID= this.value;
           if (ID.indexOf("New") >= 0){
                action = "InsertMiscCost";
                isactive = "true";
           }
           else
                action = "UpdateMiscCost"
                isactive = "true";
        }
        
        if(isactive == "true"){
            $.ajax({
                type: "POST",
                url: "ProjectCost.aspx/"+action,
                data: '{Info: ' + JSON.stringify(obj) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    alert("Success");
                    Miscellaneous();
                    GetTotalInfo();
                }
            }); 
        }
        
    });
});



function LaborCost(){
    var counter = 1;  
    var table = $('#tblLaborCost').DataTable( {
        "ajax": {
            "dataType": "json",
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "ProjectCost.aspx/GetLaborCostList",
            "data": function (d){
                return "{'CompanyId':'"+getCoyID()+"', 'ProjectNo': '" + $("#txtProjectNo").val() + "', 'UserID':'"+getUserID()+"' }";
            },
            "dataSrc": function (json) {
                return json;
            }   
        },
        "responsive": true,
        "bDestroy": true,
        "jQueryUI": true,
        "autoWidth": true,
        "dom": 'Bfrtip',
        "buttons": [
            {
                "text": 'Add',
                "action": function ( e, dt, node, config ) {
                    
                    table.row.add( {
                        "LCID": "New_"+counter,
                        "Period": "",
                        "PIC": "",
                        "Hour":"",
                        "Currency": "",
                        "Rate":"",
                        "Amount":"",
                        "Remarks":"",
                        "Action":"<button onclick=\"return false;\"  value='"+counter+"' name='Submit'>Submit</button>"
                    }).columns.adjust().draw();
                    table.columns.adjust().draw();
                    counter++;
                }
            }
        ],
        "language": {
            "emptyTable": "No results found!"
        },
        "rowId": "LCID",
        "columns": [
            {
                "data"          :   "LCID",
                "title"         :   "LCID",
                "visible"       :   false,
                'render'        :   function (data, type, row){
                                        return "<input type='text' class='form-control' id='LCID' name='LCID'  value='" + $('<div/>').text(data).html() + "' />";
                                    }
            },
            {
                "data"          :   "Period",
                "title"         :   "Period",
                'render'        :   function (data, type, row){
                    return "<div class='input-group date'><input type='text' class='form-control' id='Period' name='Period'  value='" + $('<div/>').text(data).html() + "' /></div>";
                }
            },
            {
                "data": "PIC",
                "title":"PIC",
                'render': function (data, type, row){
                     return "<input type='text' class='form-control' id='PIC' name='PIC' value='" + $('<div/>').text(data).html() + "'>";
                }
            },
            {
                "data": "CurrencyCode",
                "title": "Currency Code",
                'render': function (data, type, row){
                     return '<input type="text" class="form-control" onclick="GetCurrency(this);" id="CurrencyCode"  name="CurrencyCode" value="' + $('<div/>').text(data).html() + '" />';
                }
            },
            {
                "data": "Hour",
                "title": "Hour(s)",
                'render': function (data, type, row){
                     return "<input type='text' class='form-control'  id='Hour' name='Hour' style='text-align:right;' onchange=\"calculateLabor(this);\" value='" + $('<div/>').text(data).html() + "'>";
                }
            },
            {
                "data": "Rate",
                "title": "Rate",
                'render': function (data, type, row){
                    return "<input type='text' class='form-control' id='Rate' style='text-align:right;' onchange=\"calculateLabor(this);\" name='Rate' value='" + $('<div/>').text(data).html() + "'>";
                }
            },
            {
                "data": "Amount",
                "title": "Amount",
                'render': function (data, type, row){
                    return '<input type="text" class="form-control" id="Amount" style="text-align:right;" name="Amount" value="' + $('<div/>').text(data).html() + '" readonly="readonly"/>';
                }
            },
            {
                "data": "Remarks",
                "title": "Remarks",
                'render': function (data, type, row){
                    return "<input id='Remarks' class='form-control' name='Remarks' value='" + $('<div/>').text(data).html() + "' />";
                }
            },
            { 
                "data": null , 
                "title":"Action", 
                "render": function (data, type, row){
                    return "<button onclick=\"return false;\"  value='"+row.LCID+"' name='Update'>Submit</button>&nbsp;<button onclick=\"return false;\"  value='"+row.LCID+"' name='Delete'>Delete</button>";
                }
            }],
            "order":[1, "desc"]
        
    } );
 }
 
function DeleteLCDetail(ProjectNo, LCID){
$( "#dialog-confirm" ).dialog({
    resizable: false,
    height:140,
    modal: true,
    buttons: {
        "Confirm": function() {
            var obj = {};
            obj["ProjectNo"] = ProjectNo;
            obj["LCID"] = LCID;
            obj["CompanyId"] = getCoyID();
            obj["UserID"] = getUserID();
            $.ajax({
                type: "POST",
                url: "ProjectCost.aspx/DeleteLaborCost",
                data: '{Info: ' + JSON.stringify(obj) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    alert("Record have been deleted successfully.");
                    LaborCost();
                    GetTotalInfo();
                },
                 error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
             $( this ).dialog( "close" ); 
        },
        Cancel: function() {
            alert("Delete Record have been cancelled.");
            $( this ).dialog( "close" );
        }
    }
});
 }
 
 function DeleteOCDetail(ProjectNo, OCID){
    $( "#dialog-confirm" ).dialog({
        resizable: false,
        height:140,
        modal: true,
        buttons: {
            "Confirm": function() {
                var obj = {};
                obj["ProjectNo"] = ProjectNo;
                obj["OCID"] = OCID;
                obj["CompanyId"] = getCoyID();
                obj["UserID"] = getUserID();
                $.ajax({
                    type: "POST",
                    url: "ProjectCost.aspx/DeleteMiscCost",
                    data: '{Info: ' + JSON.stringify(obj) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Record have been deleted successfully.");
                        Miscellaneous();
                        GetTotalInfo();
                    },
                     error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
                 $( this ).dialog( "close" ); 
            },
            Cancel: function() {
                alert("Delete Record have been cancelled.");
                $( this ).dialog( "close" );
            }
        }
    });
 }
 
function GetCalendar(a){
    $(a).datetimepicker({format: 'YYYY-MM-DD'});
}

function calculateLabor(a){
    var Hour = 0;
    var Rate = 0;
    var Amount = 0;
    
    $(a).parents('tr').find("input[id='Hour']").each(function() {
        Hour = this.value;
    });
    $(a).parents('tr').find("input[id='Rate']").each(function() {
        Rate = this.value;
    });

    
    Amount = Hour * Rate;
    $(a).parents('tr').find("input[id='Amount']").val(Amount);
}

function Miscellaneous(){
    var counter = 1;  
    var selected = [];
    var table = $('#tblMiscellaneous').DataTable( {
        "ajax": {
            "dataType": "json",
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "ProjectCost.aspx/GetMiscCostList",
            "data": function (d){
                return "{ 'CompanyId':'" + getCoyID() + "', 'ProjectNo': '" + $("#txtProjectNo").val() + "', 'UserID':'" + getUserID() + "' }";
            },
            "dataSrc": function (json) {
                return json;
            }   
        },
        "responsive": true,
        "bDestroy": true,
        "jQueryUI": true,
        "autoWidth": true,
        "dom": 'Bfrtip',
        "buttons": [
            {
                "text": 'Add',
                "action": function ( e, dt, node, config ) {
                    table.row.add( {
                        "OCID": "New_"+counter,
                        "Description": "",
                        "CurrencyCode": "",
                        "Amount":"",
                        "Location": "",
                        "Remarks":"",
                        "Action":"<button onclick=\"return false;\"  value='"+counter+"' name='Submit'>Submit</button>"
                    }).columns.adjust().draw();
                    table.columns.adjust().draw();
                    counter++;
                }
            }
        ],
        "language": {
            "emptyTable": "No results found!"
        },
        "rowId": "OCID",
        "columns": [
          {
             "data": "OCID",
             "title": "OCID",
             "visible": false,
             'render': function (data, type, row){
                 return "<input type='text' class='form-control' id='OCID' name='OCID'  value='" + $('<div/>').text(data).html() + "' />";
             }
          },
          {
             "data": "Description",
             "title":"Description",
             'render': function (data, type, row){
                 return "<input type='text' class='form-control' id='Description' name='Description' value='" + $('<div/>').text(data).html() + "'>";
             }
          },
          {
             "data": "CurrencyCode",
             "title": "Currency Code",
             'render': function (data, type, row){
                 return '<input type="text" class="form-control" onclick="GetCurrency(this);" id="CurrencyCode"  name="CurrencyCode" value="' + $('<div/>').text(data).html() + '" />';
             }
          },
          {
             "data": "Amount",
             "title": "Amount",
             'render': function (data, type, row){
                 return "<input type='text' class='form-control'  id='Amount' name='Amount' style='text-align:right;' value='" + $('<div/>').text(data).html() + "'>";
             }
          },
          {
             "data": "Location",
             "title": "Location",
             'render': function (data, type, row){
                 return "<input type='text' class='form-control' id='Location' style='text-align:right;' name='Location' value='" + $('<div/>').text(data).html() + "'>";
             }
          },
          {
             "data": "Remarks",
             "title": "Remarks",
             'render': function (data, type, row){
                return "<input id='Remarks' class='form-control' name='Remarks' value='" + $('<div/>').text(data).html() + "' />";
             }
          },
          { 
            "data": null , 
            "title":"Action", 
            "render": function (data, type, row){
                return "<button onclick=\"return false;\"  value='"+row.OCID+"' name='Update'>Submit</button>&nbsp;<button onclick=\"return false;\"  value='"+row.OCID+"' name='Delete'>Delete</button>";
            }
          }],
          "rowCallback": function( row, data ) {
                if ( $.inArray(data.DT_RowId, selected) !== -1 ) {
                    $(row).addClass('selected');
                }
            },
          "order":[1, "desc"]
        
    } );
}

function MaterialRequisition() {
    var obj = {};
    obj["ProjectNo"] = $("#txtProjectNo").val();
    obj["PrevProjectNo"] = $("#txtPrevProjectNo").val();
    obj["CompanyId"] = getCoyID();
    obj["UserID"] = getUserID();
    var table = $('#tblMaterialRequisition').DataTable({
        "ajax": {
            "dataType": "json",
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "ProjectCost.aspx/GetMaterialRequisitionList",
            "data": function (d) {
                return JSON.stringify({ Info: obj });
            },
            "dataSrc": function (json) {
                return json;
            }
        },
        "responsive": true,
        "bDestroy": true,
        "jQueryUI": true,
        "autoWidth": true,
        "language": {
            "emptyTable": "No results found!"
        },
        "columns": [
                {
                    "data": "mrNo",
                    "title": "MR No"
                },
                {
                    "data": "MRDate",
                    "title": "MR Date"
                },
                {
                    "data": "vendorName",
                    "title": "Vendor"
                },
                {
                    "data": "requestor",
                    "title": "Requestor"
                },
                {
                    "data": "purchaser",
                    "title": "Purchaser"
                },
                {
                    "data": "purchaseCurrency",
                    "title": "Curr."
                },
                {
                    "data": "purchasePrice",
                    "title": "Amt."
                },
                {
                    "data": "InvoiceSum",
                    "title": "Inv. Paid"
                },
                {
                    "data": "remarks",
                    "title": "Remarks"
                },
                {
                    "data": "status",
                    "title": "Status"
                },
                {
                    "data": null,
                    "title": "Action",
                    "render": function (data, type, row) {
                        return "<button onclick=\"ViewMRDetails('" + row.mrNo + "');DisplayInvoiceList('" + row.mrNo + "');return false;\"  value='" + row.mrNo + "' name='Update' class='glyphicon glyphicon-pencil button-icon'></button>";
                    }
                }
        ]
    });
}

function ViewMRDetails(item) {
   
    var str = $("form").find("#ModelMR :input").serializeArray();
    var fields = new Object();
    fields["MRNo"] = item;
    fields["CompanyId"] = getCoyID();

    $.ajax({
        type        : "POST",
        url: "ProjectCost.aspx/GetMaterialRequisitionByMRNo",
        data        : '{Info: ' + JSON.stringify(fields) + '}',
        contentType : "application/json; charset=utf-8",
        dataType    : "json",
        success: function (data) {
           
            $.each(data, function (key, value) {
                $.each(value, function (i, item) {
                    $('#' + i).val(item);
                });
            });
            $('#MRNo').val(item);
            $('#btnMRSubmit').val(item);
            $('#ModelMR').modal({ backdrop: 'static', keyboard: false });
        },
        error       : function(xhr, textstatus, error){
            SetModalMessage(textstatus, '');
        }
    }); 
}

function DisplayInvoiceList(mrno) {
    var table = $('#tblMRInvoiceList').DataTable({
        "ajax": {
            "dataType": "json",
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "data": function (d) {
                return "{'CompanyId':'" + getCoyID() + "', 'MRNo': '" + mrno + "' }";
            },
            "url": "ProjectCost.aspx/GetMaterialRequisitionListByMRNo",
            "dataSrc": function (json) {
                return json;
            }
        },
        "responsive": true,
        "bDestroy": true,
        "jQueryUI": true,
        "language": {
            "emptyTable": "No results found!"
        },
        "rowId": "DetailNo",
        "columns": [
                                {
                                    "data": "InvoiceNo",
                                    "title": "Invoice No.",
                                },
                                {
                                    "data": "InvoiceAmount",
                                    "title": "InvoiceAmount"
                                },
                                {
                                    "data": "InvoiceRemarks",
                                    "title": "Invoice Remarks"
                                },
                                {
                                    "data": null,
                                    "title": "Action",
                                    "render": function (data, type, row) {
                                        return "<button onclick=\"deletemrinv(this, '" + mrno + "');return false;\" value='" + row.ItemID + "' id='btnDeleteInvoice' name='DeleteInvoice'>Delete</button>";
                                    }
                                }
        ],
        "order": [[1, "desc"]]
    });
}
function SaveItem(item)
{
    var str = $("form").find("#ModalMRPayment :input").serializeArray();
    var obj = {};
    obj["ProjectNo"] = getProjectNo();
    obj["MrNo"] = item.value;
    obj["CompanyId"] = getCoyID();
    obj["UserID"] = getUserID();

    jQuery.each(str, function (i, field) {
        obj[field.name] = field.value;
    });

    $.ajax({
        type: "POST",
        url: "ProjectCost.aspx/InsertProjectMR",
        data: '{Info: ' + JSON.stringify(obj) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            DisplayInvoiceList(item.value);
            SetModalMessage("Success!", '');
            ResetForm("ModalMRPayment");
        }
    });
}



function deletemrinv(item, mrno)
{
    $('#ModelMR').modal('hide');
    $("#dialog-confirm").dialog({
        resizable: false,
        height: 140,
        modal: true,
        buttons: {
            "Confirm": function () {
                var obj = {};
                obj["ProjectNo"] = getProjectNo();
                obj["ItemID"] = item.value;
                obj["CompanyId"] = getCoyID();
                obj["UserID"] = getUserID();
                obj["MRNo"] = mrno;
                $.ajax({
                    type: "POST",
                    url: "ProjectCost.aspx/DeleteMRInv",
                    data: '{Info: ' + JSON.stringify(obj) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Record have been deleted successfully.");
                        ViewMRDetails(mrno);
                        DisplayInvoiceList(mrno);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
                $(this).dialog("close");
            },
            Cancel: function () {
                alert("Delete Record have been cancelled.");
                $(this).dialog("close");
                ViewMRDetails(mrno);
                DisplayInvoiceList(mrno);
            }
        }
    });

}

function GetTotalInfo() {
    $.ajax({
        url: "ProjectCost.aspx/GetGrandTotalByProjectNo",
        data: "{ 'CompanyId': '" + getCoyID() + "', 'ProjectNo':'" + getProjectNo() + "' , 'UserId': '" + getUserID() + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            $.map(data, function (item) {
                $('#MRSubTotal').attr("value", item.MRGrandTotal);
                $('#LaborSubTotal').attr("value", item.LaborGrandTotal);
                $('#MiscSubTotal').attr("value", item.MiscGrandTotal);
                $('#InvGrandTotal').attr("value", item.ClaimGrandTotal);
                $('#PJTTotal').attr("value", parseFloat(item.LaborGrandTotal) + parseFloat(item.MiscGrandTotal) + parseFloat(item.MRGrandTotal));
            });
        },
        error: function (xhr, textstatus, error) {
            var r = jQuery.parseJSON(xhr.responseText);
            var text = r.Message;
            SetModalMessage(r.Message, 'ProjectSearch.aspx?CoyID=' + getCoyID());

        }
    });
}
