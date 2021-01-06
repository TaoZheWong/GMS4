/*
=========================================================================================
Script          : MaterialSearch
Description     : Search function
Date Created    : 2016-06-20
Date Modified   : 2016-07-28 
=========================================================================================
*/
$(document).ready(function(){
    
    //Hide the div enclosing the datatable on page load
    $( "#tblSearch" ).hide(); 
    $( "#tblMaterial" ).hide(); 
    
    //Display Datatable 
    $( "#btnSearch" ).button().on( "click", function(event) {
         DisplayMaterialList();
    });
    
    //Display modal popup for new record
    $( "#btnAdd" ).button().on( "click", function() {
        $('#btnMaterialSave' ).val("");
        $('#checkbox' ).hide();
        ResetForm("SaveMaterialRecord");
    });
    
   //Display modal popup for existing record
    $( "#btnMaterialSave" ).button().on( "click", function() {
        SaveMaterialInfo(this);
    }); 
    
    //Display autocomplete for currencycode
    $( "#CurrencyCode" ).on( "mousedown",  function() {
        GetCurrency(this);        
    });
    
    $( "#NewCurrencyCode" ).on( "mousedown",  function() {
        GetCurrency(this);        
    });

    
    /*Datetimepicker*/
    $('#NewQuotationDate').datepicker({format: 'yyyy-mm-dd', autoclose:1, ignoreReadonly: true});
    var d = new Date();
    $("#NewQuotationDate").val(d);
    
    
    //Convert to uppercase while keying data
    $( "form" ).find("#SaveMaterialRecord :input").keyup(function(){
        $(this).val($(this).val().toUpperCase());
    });
    
    $( "#chkIsActive" ).on( "click",  function() {
        $('#SaveMaterialRecord').modal({backdrop: 'static', keyboard: false});     
    } );
    
    //Check user access
    AllowChanges();


    $("input[id*='ItemCategory']").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "MaterialSearch.aspx/GetMaterialCategory",
                data: "{ 'CompanyId': '" + getCoyID() + "', 'term': '" + request.term + "', 'UserID': " + getUserID() +" }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    data = data.hasOwnProperty('d') ? data.d : data;
                    if (!data) {
                        var result = [{
                            label: 'No matches found',
                            value: response.term
                        }];
                        response(result);
                    }
                    else {
                        response(
                            $.map(data, function (item) {
                                return {
                                    label   : item.ItemCategory,
                                    value   : item.ItemCategory,
                                    text    : item.ItemCategory
                                }
                            })
                        );
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    SetModalMessage(textStatus, "");
                }
            });
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
    }).click(function () {
        $(this).autocomplete('search', $("input[id*='ItemCategory']").val());
    });

    $("input[id*='ItemCategory']").focusout(function () {
        if ($(this).val() == "LABOR" || $(this).val() == "LABOUR") {
            $("input[id*='ItemMaterial']").val("").attr('disabled', true);
        }
        else {
            $("input[id*='ItemMaterial']").attr('disabled', false);
        }
    });


    $("#btnClose").on("click", function () {
        $('#ModalMessage').modal('hide');
        if (this.value != '') {
            window.location.href = this.value;
        }
    });

    //Set timeout
//    $("#dialog").dialog({
//	    autoOpen: false,
//	    modal: true,
//	    width: 400,
//	    height: 200,
//	    closeOnEscape: false,
//	    draggable: false,
//	    resizable: false,
//	    buttons: {
//		    'Yes, Keep Working': function(){
//			    $(this).dialog('close');
//		    },
//		    'No, Logoff': function(){
//			    // fire whatever the configured onTimeout callback is.
//			    // using .call(this) keeps the default behavior of "this" being the warning
//			    // element (the dialog in this case) inside the callback.
//			    $.idleTimeout.options.onTimeout.call(this);
//		    }
//	    }
//    });

//    // cache a reference to the countdown element so we don't have to query the DOM for it on each ping.
//    var $countdown = $("#dialog-countdown");

//    // start the idle timer plugin
//    $.idleTimeout('#dialog', 'div.ui-dialog-buttonpane button:first', {
//	    idleAfter: 3570,
//	    pollingInterval: 2,
//	   // keepAliveURL: 'http://localhost/GMSWeb/Sales/Engineering/Material/MaterialSearch.aspx',
//	    serverResponseEquals: 'OK',
//	    onTimeout: function(){
//		    window.location = "http://localhost/GMSWeb/SessionTimeout.aspx";
//	    },
//	    onIdle: function(){
//		    $(this).dialog("open");
//	    },
//	    onCountdown: function(counter){
//		    $countdown.html(counter); // update the counter
//	    }
    //    });

    
});




/*
-----------------------------------------------------------------------------------------
Function Name   : AllowChanges
Description     : Restrict 
Date Created    : 2016-07-29
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/
function AllowChanges()
{  
    var retval = "disabled";
    $("#btnAdd").attr("disabled", true);
    $( "form" ).find("#MaterialRecordDetail :input").attr("disabled", true);
    $( "form" ).find("#btnGroup :input").attr("disabled", true);
    CheckAccess(function(data){           
        var AllowChanges = data[0].AccessDRI;
        if(AllowChanges == "1"){
            retval = "";
            $("#btnAdd").attr("disabled", false);
            $( "form" ).find("#MaterialRecordDetail :input").attr("disabled", false);
            $( "form" ).find("#btnGroup :input").attr("disabled", false);
            $( "form" ).find("#view").css("display", "");
        }
    }); 
    $("#hidAllowChanges").val(retval);
} 

/*
-----------------------------------------------------------------------------------------
Function Name   : DisplayMaterialList
Description     : Get and display data from tbMaterial based on search input
Date Created    : 2016-06-20
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/
function DisplayMaterialList(){
    //String search input into an array

    var datastr = "{'CoyID': '" + getCoyID() + "'" +
                 ",'modelno':'%" + $('#txtModelNo').val() + "%'" +
                 ",'description':'%" + $('#txtDescription').val() + "%'" +
                 ",'itemcategory':'%" + $('#txtItemCategory').val() + "%'" +
                 ",'suppliername':'%" + $('#txtSupplierName').val() + "%'" +
                 ",'UserID': " + getUserID() +
                 "}";

    $('#tblSearch').show();
    $('#tblMaterial').show();
    // Initialize tblSearch Datatable
    var table   = $('#tblMaterial').DataTable( {
        "ajax": {
            "dataType"      : "json",
            "contentType"   : "application/json; charset=utf-8",
            "type"          : "POST",
            "url"           : "MaterialSearch.aspx/GetMaterialList",
            "data"          : function (data) { return datastr; },
            "dataSrc": function (json) {
                                json = json.hasOwnProperty('d') ? json.d : json;
                                return json;
                              }   
        },
        "responsive"        : true,
        "bDestroy"          : true,
        "jQueryUI"          : true,
        "language"          : {
                                "emptyTable":     "No results found!"
                              },
        "rowId"             : "ItemID",             
        "columns"           : [
                                {
                                    "className" : 'details-control',
                                    "orderable" : false,                    
                                    "data"      : null
                                },
                                {
                                    "data"      : "ItemDescription",
                                     "title"    : "Description"
                                },
                                { 
                                    "data"      : "ModelNo", 
                                    "title"     : "Model No",
                                },
                               
                                { 
                                    "data"      : "ItemCategory", 
                                    "title"     : "Category" 
                                },
                                { 
                                    "data"      : "ItemMaterial", 
                                    "title"     : "Material" 
                                },
                                { 
                                    "data"      : "ItemSize", 
                                    "title"     : "Size" 
                                },
                                { 
                                    "data"      : "SupplierName", 
                                    "title"     : "Supplier Name" 
                                },
                                { 
                                    "data"      : "CurrencyCode", 
                                    "title"     : "Currency" 
                                },
                                { 
                                    "data"      : "UnitPrice", 
                                    "title"     : "Unit Price" 
                                },
                                { 
                                    "data"      : "QuotationDate", 
                                    "title"     : "Quotation Date" 
                                },
                                { 
                                    "data"      : "IsActive", 
                                    "title"     : "Active" 
                                },
                                { 
                                    "data"      : null, 
                                    "title"     : "Action" ,
                                    "className" : "all",
                                    "render"    : function (data, type, row){
                                                    return "<button class='btn btn-primary btn-xs' id='Edit' value='"+row.ItemID+"' name='Edit' onclick=\"PopulateMaterialModal(this);DisplayMaterialPriceList(this);Attachment(this.value);return false;\">View</button>";
                                                  }
                                }
                              ], 
                            "columnDefs": [ {
                                "className": 'all',
                                "orderable": false,
                                "targets":   -1
                            } ],
                            "rowCallback": function( row, data, iDisplayIndex ) {
                                var info = table.page.info();
                                var page = info.iPage;
                                var length = info.length;
                                var index = (page * length + (iDisplayIndex +1));
                                $('td[class="details-control"]', row).html(index);
                            },   
                            "fnCreatedRow": function (row, data, index) {
                                $('td', row).eq(0).html(index + 1);
                            },   
        "order"             : [[ 0, "desc" ]]
    } );
} 

/*
-----------------------------------------------------------------------------------------
Function Name   : CheckAccess
Description     : Check if user have access to add or edit data
Date Created    : 2016-07-29
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/
function CheckAccess(callback) {
    var datastr = "{'CompanyId': '" + getCoyID() + "'" +
                 ",'DocNo':''" +
                 ",'UserID': " + getUserID() +
                 "}";

    $.ajax({
        async       : false,
        type        : "POST",
        url         : "MaterialSearch.aspx/CheckUserAccess",
        contentType : "application/json; charset=utf-8",
        dataType    : "json",
        data        : datastr,
        success: function (data) {
                        data = data.hasOwnProperty('d') ? data.d : data;
                        callback(data);
                      },
        error       : function (xhr, textstatus, error) {
                        SetModalMessage(textstatus, "");
                      }
    }); 
}

/*
-----------------------------------------------------------------------------------------
Function Name   : PopulateMaterialModal
Description     : Get existing record and populate modal dialog
Date Created    : 2016-06-20
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/
function PopulateMaterialModal(item) {
    var datastr = "{'CompanyId': '" + getCoyID() + "'" +
                 ",'ItemID':'" + item.value + "'" +
                 ",'UserID': " + getUserID() +
                 "}";


    $.ajax({
        type        : "POST",
        url         : "MaterialSearch.aspx/GetMaterialInformation",
        data        : datastr,
        contentType : "application/json; charset=utf-8",
        dataType    : "json",
        success: function (data) {
                        data = data.hasOwnProperty('d') ? data.d : data;
                        $('#ItemMaterial').val("").attr('disabled', false);
                        $.each(data, function (key, value) {
                            $.each(value, function (i, item) {
                                $('#'+i).val(item);
                                if(i == "CurrencyCode" || i == "UnitPrice" || i == "QuotationDate" || i == "QuotationValidity"){
                                    $('#New'+i).val(item);
                                }
                            });
                        });
                        if ($('#ItemCategory').val() == "LABOR" || $('#ItemCategory').val() == "LABOUR") {
                            $("#ItemMaterial").val("").attr('disabled', true);
                        }
                        $('#btnMaterialSave' ).val(item.value);
                        $('#checkbox' ).show();
                        $('#SaveMaterialRecord').modal({backdrop: 'static', keyboard: false});  
                      },
        error       : function(xhr, textstatus, error){
                        SetModalMessage(textstatus, "");
                      }
    }); 
}

function DisplayMaterialPriceList(item){

    var table   = $('#tblMaterialPriceList').DataTable( {
        "ajax": {
            "dataType"      : "json",
            "contentType"   : "application/json; charset=utf-8",
            "type"          : "POST",
            "data"          : function (d){
                            return "{'CompanyID':'" + getCoyID() + "', 'ItemID': '" + item.value + "' }";
                              },
            "url"           : "MaterialSearch.aspx/GetMaterialPriceList",
            "dataSrc": function (json) {
                                json = json.hasOwnProperty('d') ? json.d : json;
                                return json;
                              }   
        },
        "responsive"        : true,
        "bDestroy"          : true,
        "jQueryUI"          : true,
        "language"          : {
                                "emptyTable":     "No results found!"
                              },
        "rowId"             : "DetailNo",             
        "columns"           : [
                                {
                                    "className" : 'details-control',
                                    "orderable" : false,                    
                                    "data"      : null,
                                    "title"     : "S/N"
                                },     
                                { 
                                    "data"      : "QuotationDate", 
                                    "title"     : "Quotation Date",
                                },
                                { 
                                    "data"      : "QuotationValidity", 
                                    "title"     : "Quotation Validity" 
                                },
                                { 
                                    "data"      : "CurrencyCode", 
                                    "title"     : "Currency Code" 
                                },
                                { 
                                    "data"      : "UnitPrice", 
                                    "title"     : "Unit Price" 
                                },
                                { 
                                    "data"      : "IsInEffect", 
                                    "title"     : "Is InEffect" 
                                }
                              ], 
        "rowCallback"       : function( row, data, iDisplayIndex ) {
                                 var info = table.page.info();
                                 var page = info.iPage;
                                 var length = info.length;
                                 var index = (page * length + (iDisplayIndex +1));
                                 $('td[class="details-control"]', row).html(index);
                              },   
        "fnCreatedRow"      : function (row, data, index) {
                                $('td', row).eq(0).html(index + 1);
                              },
        "order"             : [[ 1, "desc" ]]
    } );
} 
/*
-----------------------------------------------------------------------------------------
Function Name   : SaveMaterialInfo
Description     : Save new record into tbMaterial
Date Created    : 2016-06-20
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/
function SaveMaterialInfo(item) {
    
    var cansave = "true";
    var test = "";
    var message = "";
    //serialize 
    var str = $( "form" ).find("#SaveMaterialRecord :input").serializeArray();
    var fields  = new Object();
    
    var method = "SaveMaterialInformation";
    fields["IsActive"] = "True";
    
    jQuery.each( str, function( i, field ) {
        if(field.name != "ItemLeadtime" || field.name != "NewQuotationValidity" || field.name != "NewQuotationDate")
        {
            if (field.name == "ItemCategory")
                test = field.value;
            if (field.name == "ItemMaterial")
            {
                if ((test != "LABOR" || test != "LABOUR") && field.value == "")
                {
                    cansave = "false";
                    message = message + "<li>Please fill in " + fieldnames(field.name) + "</li>"
                }
            }
            if (field.name != "ItemMaterial")
            {
                if (field.value == "") {
                    cansave = "false";
                    message = message + "<li>Please fill in " + fieldnames(field.name) + "</li>"
                }
            }
        }
    });

    jQuery.each( str, function( i, field ) {
        fields[field.name] = field.value;
    });
    
    if(item.value!=''){
        fields["ItemID"] = item.value;
        method = "EditMaterialInformation";
    }
    fields["CompanyId"] = getCoyID();
    fields["UserID"] = getUserID();
    if (message == "" && cansave == "true") {
        $.ajax({
            type: "POST",
            url: "MaterialSearch.aspx/" + method,
            data: '{Info: ' + JSON.stringify(fields) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                response = response.hasOwnProperty('d') ? response.d : response;
                SetModalMessage("Success!", "");
                $('#SaveMaterialRecord').modal('hide')
                DisplayMaterialList();
            },
            error: function (xhr, textstatus, error) {
                var r = jQuery.parseJSON(xhr.responseText);
                SetModalMessage(r.Message, "");
            }
        });
    }
    else {
        //  $('#SaveMaterialRecord').modal('hide');
        SetModalMessage(message, "");
    }
}

function SaveMaterialUpload(item, ItemID) {
    //serialize 
    var fields  = new Object();
    jQuery.each( item[0], function( i, field ) {
        fields[i] = field;
    });
    fields["CompanyId"] = getCoyID();
    fields["UserID"] = getUserID();
    fields["ItemID"] = ItemID;
    $.ajax({
        type        : "POST",
        url         : "MaterialSearch.aspx/MaterialUploadAttachment",
        data        : '{Info: ' + JSON.stringify(fields) + '}',
        contentType : "application/json; charset=utf-8",
        dataType    : "json",
        success: function (response) {
                        response = response.hasOwnProperty('d') ? response.d : response;
                        Attachment(ItemID);
                      },
        error       : function(xhr, textstatus, error){
                        var r = jQuery.parseJSON(xhr.responseText);
                        SetModalMessage(r.Message, "");
                      }
    });
}

function Attachment(item){

    var table   = $('#tblAttachment').DataTable( {
        "ajax": {
            "dataType"      : "json",
            "contentType"   : "application/json; charset=utf-8",
            "type"          : "POST",
            "data"          : function (d){
                                return "{'CompanyId':'"+getCoyID()+"', 'ItemID': '" + item + "' }";
                              },
            "url"           : "MaterialSearch.aspx/GetMaterialAttachment",
            "dataSrc": function (json) {
                                json = json.hasOwnProperty('d') ? json.d : json;
                                return json;
                              }   
        },
        "responsive"        : true,
        "bDestroy"          : true,
        "jQueryUI"          : true,
        "language"          : {
                                "emptyTable":     "No results found!"
                              },
        "rowId"             : "FileID",             
        "columns"           : [
                                {
                                    "className" : 'details-control',
                                    "orderable" : false,                    
                                    "data"      : null,
                                    "title"     : "S/N"
                                },     
                                { 
                                    "data"      : "FileDisplayName", 
                                    "title": "File Name",
                                    "render": function (data, type, row, meta) {
                                        var a = '<a onclick=\'Download("' + getCoyID() + '","' + row.FileName + '", "' + data + '");\'">' + data + '</a>'
                                        return a;
                                    }
                                }
                              ], 
        "rowCallback"       : function( row, data, iDisplayIndex ) {
                                 var info = table.page.info();
                                 var page = info.iPage;
                                 var length = info.length;
                                 var index = (page * length + (iDisplayIndex +1));
                                 $('td[class="details-control"]', row).html(index);
                              },   
        "fnCreatedRow"      : function (row, data, index) {
                                $('td', row).eq(0).html(index + 1);
                              },
        "order"             : [[ 1, "desc" ]]
    } );
} 


function GetCurrency(a){
    $(a).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "MaterialSearch.aspx/GetCurrencyList",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",              
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
                                    label: item.CurrencyCode,
                                    value: item.CurrencyCode,
                                    text : item.CurrencyCode
                                }
                            })
                        )
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    SetModalMessage(textStatus, "");
                }
            });
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
    }).click(function() {
        $(this).autocomplete('search', $(a).val());
    });
}

function SetModalMessage(message, redirect) {
    $('#btnClose').val(redirect);
    $('#message').html(message);
    $('#ModalMessage').modal({ backdrop: 'static', keyboard: false });
}


function Download(CoyID, fileName, orifilename) {
    window.location = "FileDownloadHandler.ashx?CompanyId=" + CoyID + "&FileName=" + fileName + "&OriFileName=" + orifilename;
}


function fieldnames(name)
{
    var field = "";
    if(name == "ItemDescription"){field = "Description";}
    else if (name == "ModelNo") { field = "Model"; }
    else if (name == "ItemCategory") { field = "Category"; }
    else if (name == "ItemMaterial") { field = "Material"; }
    else if (name == "ItemBrand") { field = "Brand"; }
    else if (name == "SupplierName") { field = "Supplier Name"; }
    else if (name == "ItemSize") { field = "Size"; }
    else if (name == "NewCurrencyCode") { field = "Currency"; }
    else if (name == "NewUnitPrice") { field = "Unit Price"; }
    
    return field;
                                  

}