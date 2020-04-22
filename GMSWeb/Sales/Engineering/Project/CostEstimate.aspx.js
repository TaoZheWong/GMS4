
/* Edit Project JS Logs   */
/* 
Added Date      : 03/06/2016
Modified Date   :
Changes         :   
*/

$(document).ready(function() {
    CEDetail();
});

function CEDetail(){
var counter = 1;
    var table = $('#tblCostEstimate').DataTable({
        "ajax": {
            "dataType": "json",
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "CostEstimate.aspx/GetCostEstimateList",
            "data": function (d){
                return "{'CompanyId':'" + getCoyID() + "', 'CEID': '" + $("#txtCEID").val() + "', 'UserID':'" + getUserID() + "' }";
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
                    "data": "ItemName", 
                    "title":"Description"
                },
                {
                    "data": "Category",
                    "title": "Category"
                },
                { 
                    "data": "ItemBrand", 
                    "title":"Item Brand"
                },
                { 
                    "data": "ItemMaterial", 
                    "title":"Material"
                },
                { 
                    "data": "SupplierName", 
                    "title":"Supplier"
                },
                { 
                    "data": "UOM", 
                    "title":"UOM"
                },
                { 
                    "data": "Quantity" , 
                    "title":"Qty"
                },
                { 
                    "data": "CurrencyCode", 
                    "title":"Curr"
                },
                {
                    "data": "QuotedPrice",
                    "title": "Unit Price"
                },
                { 
                    "data": "CurrencyRate", 
                    "title":"Ori Rate"
                },
                {
                    "data": "TotalAmount",
                    "title": "Ori Amt"
                },
                {
                    "data": "CurrencyRate",
                    "title": "Curr Rate"
                },
                {
                    "data": "TotalAmountExchanged",
                    "title": "Curr Amt"
                },
                { 
                    "data": "Remarks", 
                    "title":"Remarks"
                }
        ]    
    });
}
