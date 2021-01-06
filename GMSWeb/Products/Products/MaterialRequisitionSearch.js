$(document).ready(function() { 
        $( "#Add" ).on( "click", function() {         
           window.location.href = "AddEditMaterialRequisition.aspx?CurrentLink="+getCurrentLink()+"&CoyID="+getCoyID()+"&MRNo=";
        });  
        
        $( "#Search" ).on( "click", function() {
             Search();
        }); 
                  
        
        var d = new Date();        
        d.setMonth("0");
        d.setDate("01");
        d = $.datepicker.formatDate('dd/mm/yy', d);
       
        var date = $.datepicker.formatDate('dd/mm/yy', new Date());
        $("#date-from").val(d);
        $("#date-to").val(date);        
        
        if (getCoyID() != "28" && getCoyID() != "57" && getCoyID() != "103" && getCoyID() != "81" && getCoyID() != "115" && getCoyID() != "116")
        {          
           $("#Add").css("display", '');            
        }
        
        $( "#Search" ).focus();
           
});

$(document).keypress(function (e) {
    if (e.which == 13) {
        $('#Search').click();
    }
});

function getMRScheme() {
    var hidMRScheme = $("input[id*=hidMRScheme]").val();    
    return hidMRScheme;
}


function Search()
{
    
    var UserID = getUserID(); 
    var datastr = "{'CoyID': " + getCoyID() +" "+
                  ",'DateFrom':'"+$('#date-from').val()+"'" +
                  ",'DateTo':'"+$('#date-to').val()+"'" +
                  ",'MRNo':'"+$('#mr-no').val()+"'" +
                  ",'Purchaser':'"+$('#purchaser').val()+"'" +
                  ",'CustomerCode':'"+$('#customer-code').val()+"'" +
                  ",'CustomerName':'"+$('#customer-name').val()+"'" +
                  ",'ProductCode':'"+$('#product-code').val()+"'" +
                  ",'ProductName':'"+$('#product-name').val()+"'" +
                  ",'ProductGroupCode':'"+$('#product-group-code').val()+"'" +
                  ",'ProductGroupName':'"+$('#product-group-name').val()+"'" +
                  ",'Vendor':'"+$('#vendor').val()+"'" +   
                  ",'PO':'"+$('#po').val()+"'" +                    
                  ",'Status':'"+$('#status').val()+"'" +  
                  ",'RequestorRemarks':'"+$('#requestor-remarks').val()+"'" +  
                  ",'Requestor':'"+$('#requestor').val()+"'" + 
                  ",'ProductManagerID':'"+$('#approver1-id').val()+"'" + 
                  ",'BudgetCode':'"+$('#budget-code').val()+"'" +
                  ",'RefNo':'"+$('#ref-no').val()+"'" +
                  ",'ProjectNo':'"+$('#project-no').val()+"'" +
                  ",'UserRole':'"+getUserRole()+"'" +                  
                  ",'UserID': " + getUserID() +
                  ",'MRScheme': '" + getMRScheme() + "'" +
                  "}";          
                
    
    var a = $('#tblSearch').DataTable( {
                "ajax": {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "MaterialRequisitionSearch.aspx/GetMR",
                "data": function (data) { return datastr;},  
                "dataSrc": function (json) { 
                    json = json.hasOwnProperty('d') ? json.d : json;
                     return json;                    
                }
            },
            "responsive": false,  
            "stateSave": false,         
            "bDestroy": true,
            "filter": false,
            "jQueryUI": true,
            "dom": 'Bfrtip',
            "buttons": [
               
            ],
            "language": {
                "emptyTable": "No results found!"
            },
            "rowId": "MRNo",
            "columns": [
                {
                    "className":      'details-control',
                    "orderable":      false,                    
                    "data":           null
                },                       
                { "data": "mrno",
                  "title": "MR No",
                  "width": "14%",
                  "render" : function(data, type, row, meta){
                       if(type === 'display'){
                           return $('<a>')
                              .attr('href', "AddEditMaterialRequisition.aspx?v1=9&CurrentLink=" + getCurrentLink() + "&CoyID=" + row.CoyID + "&MRNo=" + data)
                              .text(data)
                              .wrap('<div></div>')
                              .parent()
                              .html();

                        } else {
                           return data;
                        }
                     }   
                },
                { "data": "mrdate",
                  "title": "MR Date",
                  "width": "14%"                  
                },
                { "data": "statusname",
                  "title": "Status",
                  "width": "14%"
                },            
                { "data": "requestor",
                  "title": "Requestor",
                  "width": "14%"
                },
                { "data": "pmrealname",
                  "title": "Product Manager",
                  "width": "14%"
                },
                { "data": "vendorName",
                  "title": "Vendor",
                  "width": "14%",
                  "visible" : false
                },
                { "data": "Purchaser",
                  "title": "Purchaser",
                  "width": "14%"
                }             
            ],
            "columnDefs": [ {
                "className": 'all',
                "orderable": false,
                "targets":   -1
            } ],
            "rowCallback": function( row, data, iDisplayIndex ) {
                 var info = a.page.info();
                 var page = info.iPage;
                 var length = info.length;
                 var index = (page * length + (iDisplayIndex +1));
                 $('td[class="details-control"]', row).html(index);
            },   
            "fnCreatedRow": function (row, data, index) {
                $('td', row).eq(0).html(index + 1);
            },        
            "order":[[2, "desc"]]
        }); 
        
        if(getUserRole() == "Product Team" || getUserRole() == "Purchasing")
            a.columns([6]).visible(true);  
        
} 


           
        
       
    
        
        
 