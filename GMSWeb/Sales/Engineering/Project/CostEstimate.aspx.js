
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
                json = json.hasOwnProperty('d') ? json.d : json;
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
                    "data": "TotalAmount",
                    "title": "Amount"
                },
                { 
                    "data": "CurrencyRate", 
                    "title":"Ori Rate"
                },
                {
                    "data": "TotalAmountExchanged",
                    "title": "Total (*Ori Rate)"
                },
                {
                    "data": "CurrExchRate",
                    "title": "Curr Rate"
                },
                {
                    "data": "CurrExchAmt",
                    "title": "Total (*Curr Rate)"
                },
                { 
                    "data": "Remarks", 
                    "title":"Remarks"
                }
        ]    
    });
}
