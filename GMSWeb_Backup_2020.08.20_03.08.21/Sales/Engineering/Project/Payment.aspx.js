$(document).ready(function() {
    
    Payment();
    $( "#dialog-confirm" ).hide();
    $('#tblPayment tbody').on( 'click', 'button', function () {
        
        var action = "";
        var isactive = "false";
        var obj = {};
        obj["ProjectNo"] = $("#txtProjectNo").val();
        obj["PCId"] = this.value;
        $("#"+this.value+" td").find("input").each(function() {
            obj[this.name]=this.value;
        });
        $(this).parents('tr').find("input").each(function() {
            obj[this.name]=this.value;
        });
        
      
        if (this.name=="Delete")
            DeletePCDetail(obj["ProjectNo"], this.value);
        else {
            action = "InsertPayment";
            var ID = this.value;
            if (ID.toLowerCase().indexOf("new") >= 0){
                obj['IsNew']='Yes';
            }
            else{
                obj['IsNew']='No';
            }
            isactive = "true";
        }
        
        if(isactive == "true"){
            $.ajax({
                type: "POST",
                url: "Payment.aspx/"+action,
                data: '{Info: ' + JSON.stringify(obj) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    alert("Success");
                    Payment();
                }
            }); 
        }
        
    });
});

function Payment(){
    var counter = 1;  
    var table = $('#tblPayment').DataTable({
        "ajax": {
            "dataType": "json",
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "Payment.aspx/GetPaymentList",
            "data": function (d){
                return "{ 'ProjectNo': '" + $("#txtProjectNo").val() + "' }";
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
                        "PCId": "New_"+counter,
                        "ClaimOrder": "",
                        "ClaimDate": "",
                        "CurrencyCode":"",
                        "ClaimAmount":"",
                        "Balance":"",
                        "Retention":"",
                        "Ref":"",
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
        "rowId": "PCId",
        "columns": [
                { 
                    "data": "PCId", 
                    "title":"Detail ID", 
                    "visible": false,
                    "render": function (data, type, row){
                     return "<input type='text' class='form-control' id='PCId' name='PCId' value='" + $('<div/>').text(data).html() + "'>";
                    }
                },
                { 
                    "data": "ClaimOrder", 
                    "title":"Payment Order",
                    "render": function (data, type, row){
                        return "<input type='number' min='1' class='form-control' id='ClaimOrder' name='ClaimOrder' value='" + $('<div/>').text(data).html() + "'>";
                    }
                },
                { 
                    "data": "ClaimDate" , 
                    "title":"Payment Date",
                    "render": function (data, type, row){
                        return "<div class='input-group date'><input type='text' class='form-control' id='ClaimDate' name='ClaimDate' onmousedown ='GetCalendar(this);' value='" + $('<div/>').text(data).html() + "'></div>";
                    }
                },
                { 
                    "data": "CurrencyCode", 
                    "title":"Currency Code",
                    "render": function (data, type, row){
                        return "<input type='text' class='form-control' id='CurrencyCode' onmousedown='GetCurrency(this);' name='CurrencyCode' value='" + $('<div/>').text(data).html() + "'>";
                    }
                },
                { 
                    "data": "ClaimAmount", 
                    "title":"Amount",
                    "render": function (data, type, row){
                        return "<input type='text' class='form-control' id='ClaimAmount' name='ClaimAmount' value='" + $('<div/>').text(data).html() + "'>";
                    }
                },
                { 
                    "data": "Balance", 
                    "title":"Balance",
                    "render": function (data, type, row){
                        return "<input type='text' class='form-control' id='Balance' name='Balance' value='" + $('<div/>').text(data).html() + "'>";
                    }
                },
                { 
                    "data": "Retention", 
                    "title":"Retention",
                    "render": function (data, type, row){
                        return "<input class='form-control select' id='Retention' name='Retention' onmousedown='GetRetention(this);' value='" + $('<div/>').text(data).html() + "'/>";
                    }
                },
                { 
                    "data": "Ref", 
                    "title":"Ref",
                    "render": function (data, type, row){
                        return "<input type='text' class='form-control' id='Ref' name='Ref' value='" + $('<div/>').text(data).html() + "'>";
                    }
                },
                { 
                    "data": "Remarks", 
                    "title":"Remarks",
                    "render": function (data, type, row){
                        return "<input type='text' class='form-control' id='Remarks' name='Remarks' value='" + $('<div/>').text(data).html() + "'>";
                    }
                },
                
                { 
                    "data": null , 
                    "title":"Action", 
                    "render": function (data, type, row){
                        return "<button onclick=\"return false;\"  value='"+row.PCId+"' name='Update'>Submit</button>&nbsp;<button onclick=\"return false;\"  value='"+row.PCId+"' name='Delete'>Delete</button>";
                    }
                }
        ]
    });
}

function GetCalendar(a){
        $(a).datetimepicker({format: 'YYYY-MM-DD'})
}

//function GetCurrency(a){
//    var obj = {};
//    obj['ProjectNo'] = $( "#txtProjectNo" ).val();
//      
//    $(a).autocomplete({
//    
//        source: function(request, response) {
//            $.ajax({
//                url: "ProjectSearch.aspx/GetCurrencyList",
//                dataType: "json",
//                type: "POST",
//                contentType: "application/json; charset=utf-8",
//                dataFilter: function(data) { return data; },
//                success: function(data) {
//                    if(!data){
//                        var result = [{
//                            label: 'No matches found', 
//                            value: response.term
//                        }];
//                        response(result);
//                    }
//                    else{
//                        response(
//                            $.map(data, function(item) {
//                                return { 
//                                    label: item.CurrencyCode,
//                                    value: item.CurrencyCode,
//                                    text : item.CurrencyCode
//                                }
//                            })
//                        )
//                    }
//                },
//                error: function(XMLHttpRequest, textStatus, errorThrown) {
//                    alert(textStatus);
//                }
//            });
//        },
//        select: function (e, i) {
//            
//            obj['CurrencyCode'] = i.item.value;
//             $.ajax({
//                url: "CostEstimate.aspx/GetCurrencyRate",
//                dataType: "json",
//                type: "POST",
//                contentType: "application/json; charset=utf-8",
//                data: '{Info: ' + JSON.stringify(obj) + '}',
//                success: function (data) { 
//                    $.map(data, function(item) {
//                        $(a).closest("tr").find("#CurrencyRate").val(item.MonthEndRate);
//                        action = data.Action;
//                    });             
//                    CalculateAmount(a); 
//                },
//                error: function (xhr, ajaxOptions, thrownError) {
//                    alert('Failed to retrieve detail.');
//                }
//            });
//        },
//        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
//    }).click(function() {
//        $(this).autocomplete('search', $(this).val())
//    });;
//}

function GetRetention(a){
    
    $(a).autocomplete({
        source: ["Yes", "No"],
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
    }).click(function() {
        $(this).autocomplete('search', "");
    });
}

function DeletePCDetail(ProjectNo, PCId) {
    $( "#dialog-confirm" ).dialog({
        resizable: false,
        height:140,
        modal: true,
        buttons: {
            "Confirm": function() {
                var obj = {};
                obj["ProjectNo"] = ProjectNo;
                obj["PCId"] = PCId;
                
                $.ajax({
                    type: "POST",
                    url: "Payment.aspx/DeletePayment",
                    data: '{Info: ' + JSON.stringify(obj) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Record have been deleted successfully.");
                        Payment();
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