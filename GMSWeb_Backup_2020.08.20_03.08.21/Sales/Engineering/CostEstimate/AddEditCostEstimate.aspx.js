/*
=========================================================================================
Script          : AddEditCostEstimate.js
Description     : AddEditDelete function
Date Created    : 2016-06-20
Date Modified   : - 
=========================================================================================
*/

$(function() {

    $("a.tooltipLink").tooltip();

    /*Datetimepicker*/
    $('#txtContractDateFrom').datepicker({ format: 'yyyy-mm-dd', ignoreReadonly: true });
    $('#txtContractDateTo').datepicker({ format: 'yyyy-mm-dd', ignoreReadonly: true });
    $('#txtCommencementDate').datepicker({ format: 'yyyy-mm-dd', ignoreReadonly: true });

    var d = new Date();
    d.setMonth(d.getMonth() - 1);
    d = $.datepicker.formatDate('yy-mm-dd', d);
   
    var date = $.datepicker.formatDate('yy-mm-dd', new Date());
    $("#txtCreatedDateFrom").val(d);
    $("#txtCreatedDateTo").val(date);
    
    /*Check User Access*/
    CheckAccess(function(data){
        if(data[0].AccessEngineeringAdmin == "0"){
            $("#txtEngineerID").attr("readonly", "readonly");
            $("#txtEngineerName").attr("readonly", "readonly");
            GetEngineerDetail();
        }
        else{
            $("#txtEngineerID").attr("readonly", false);
            $("#txtEngineerName").attr("readonly", false);
        }
    });
    
    /*Check for CEID*/
    var first = getUrlVars()["CEID"];
    if((first!="" || first!=null) && first!== undefined && first.toLowerCase().indexOf("ce") >= 0)
    {
        DisplayCostEstimate(0);
        RevisionList();
    }
    else {
        $("#txtCEStatusName").val("Draft"); 
        $("#txtCEStatusID").val("1");  
        showButton("null");
        $("#t2").addClass("disabled").find("a").removeAttr("data-toggle").attr("title", "Please click 'Save' button first.");
    }
    
    $("#txtRevision").on('change', function(){
        DisplayCostEstimate(this.value);
        $("#displaybuttons").load();
    });
    /*Autocomplete*/
    $("#txtAccountCode").on('keyup', function(){
        GetFullAccountList(this);
    });
    
    $('#BillingAddressDropDown').click(function() {
       $("#txtBillingAddress").autocomplete( "search", "" );
    });
    
    $("textarea[id*='txtBillingAddress']").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "AddEditCostEstimate.aspx/GetAccountBillingAddressList",
                data: "{'CompanyId':'" + getCoyID() + "', 'account': '" + $("#txtAccountCode").val() + "' }",
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
                                    label: item.AddressCode + ' - ' + item.BillingAddress,
                                    value: item.BillingAddress,
                                    text : item.BillingAddress,
                                    phone: item.OfficePhone,
                                    fax  : item.Fax 
                                }
                            })
                        );
                    }    
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    SetModalMessage(textstatus, '');
                }
            });
        },
        select: function (e, i) {
            $("input[id*='txtOfficePhone']").val(i.item.phone);
            $("input[id*='txtFax']").val(i.item.fax);
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
    }).click(function() {
        $(this).autocomplete('search', $(this).val())
    });
    
    $("#txtCurrencyCode").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "AddEditCostEStimate.aspx/GetCurrencyList",
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
                                    value: item.CurrencyCode,
                                    text : item.CurrencyCode,
                                }
                            })
                        );
                    }    
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    SetModalMessage(textstatus, '');
                }
            });
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
    }).click(function() {
        $(this).autocomplete('search', '')
    });
    
    $("input[id*='txtEngineerName']").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "AddEditCostEstimate.aspx/GetEngineerList",
                data: "{'CompanyId':'" + getCoyID() + "', 'engineer': '" + request.term + "' }",
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
                                    text : item.EngineerID
                                }
                            })
                        )
                    }
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    SetModalMessage(textstatus, '');
                }
            });
        },
        select: function (e, i) {
            $("input[id*=txtEngineerName]").attr("value", i.item.value);
            $("input[id*=txtEngineerID]").attr("value", i.item.text);
        },
        minLength: 2    // MINIMUM 1 CHARACTER TO START WITH.
    });
    
    $("#CurrencyCode").on("mousedown", function(){
        CurrencyRate(this);
    });

    $("input[id*='txtSalesPersonName']").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "AddEditCostEstimate.aspx/GetSalesPersonList",
                data: "{'CompanyId':'" + getCoyID() + "', 'salesperson': '" + request.term + "' }",
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
                                    label: item.SalesPersonID +' - '+ item.SalesPersonName,
                                    value: item.SalesPersonName,
                                    text : item.SalesPersonID
                                }
                            })
                        )
                    }
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    SetModalMessage(textstatus, '');
                }
            });
        },
        select: function (e, i) {
            $("input[id*=txtSalesPersonName]").val(i.item.value);
            $("input[id*=txtSalesPersonID]").val(i.item.text);
        },
        minLength: 2    // MINIMUM 1 CHARACTER TO START WITH.
    })
  
    
    
       
    $("#UOM").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "AddEditCostEStimate.aspx/GetUOMList",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: "{ 'CompanyId': '" + getCoyID() + "'}",
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
                                   
                                    value: item.UOM,
                                    text : item.UOM,
                                }
                            })
                        );
                    }    
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    SetModalMessage(textstatus, '');
                }
            });
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
    }).click(function() {
        $(this).autocomplete('search', '')
    });

    //Convert to uppercase while keying data
    $("form").find("#ModalItems :input").keyup(function () {
        $(this).val($(this).val().toUpperCase());
    });

    $("#ItemCategory").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "AddEditCostEstimate.aspx/GetMaterialCategory",
                data: "{ 'CompanyId': '" + getCoyID() + "', 'term': '" + request.term + "', 'UserID': " + getUserID() + " }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
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
                                    label: item.ItemCategory,
                                    value: item.ItemCategory,
                                    text: item.ItemCategory
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
        $(this).autocomplete('search', '')
    });
    
    $("#ItemMaterial").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "AddEditCostEstimate.aspx/GetMaterial",
                data: "{ 'CompanyId': '" + getCoyID() + "', 'category': '%" + $("#ItemCategory").val() + "%', 'material': '%" + request.term + "', 'size': '%" + $("#ItemSize").val() + "%', 'suppliername': '%" + $("#SupplierName").val() + "%', 'brand': '%" + $("#ItemBrand").val() + "%', 'description': '%" + $("#ItemDescription").val() + "%', 'field': 'Material' , 'UserID': " + getUserID() + " }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
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
                                    label: item.ItemMaterial,
                                    value: item.ItemMaterial,
                                    text: item.ItemMaterial
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
        $(this).autocomplete('search', '')
    });

    $("#ItemSize").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "AddEditCostEstimate.aspx/GetMaterial",
                data: "{ 'CompanyId': '" + getCoyID() + "', 'category': '%" + $("#ItemCategory").val() + "%', 'material': '%" + request.term + "', 'size': '%" + $("#ItemSize").val() + "%', 'suppliername': '%" + $("#SupplierName").val() + "%', 'brand': '%" + $("#ItemBrand").val() + "%', 'description': '%" + $("#ItemDescription").val() + "%', 'field': 'Size' , 'UserID': " + getUserID() + " }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
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
                                    label: item.ItemSize,
                                    value: item.ItemSize,
                                    text: item.ItemSize
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
        $(this).autocomplete('search', '')
    });

    $("#SupplierName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "AddEditCostEstimate.aspx/GetMaterial",
                data: "{ 'CompanyId': '" + getCoyID() + "', 'category': '%" + $("#ItemCategory").val() + "%', 'material': '%" + request.term + "', 'size': '%" + $("#ItemSize").val() + "%', 'suppliername': '%" + $("#SupplierName").val() + "%', 'brand': '%" + $("#ItemBrand").val() + "%', 'description': '%" + $("#ItemDescription").val() + "%', 'field': 'SupplierName' , 'UserID': " + getUserID() + " }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
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
                                    label: item.SupplierName,
                                    value: item.SupplierName,
                                    text: item.SupplierName
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
        $(this).autocomplete('search', '')
    });

    $("#ItemBrand").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "AddEditCostEstimate.aspx/GetMaterial",
                data: "{ 'CompanyId': '" + getCoyID() + "', 'category': '%" + $("#ItemCategory").val() + "%', 'material': '%" + request.term + "', 'size': '%" + $("#ItemSize").val() + "%', 'suppliername': '%" + $("#SupplierName").val() + "%', 'brand': '%" + $("#ItemBrand").val() + "%', 'description': '%" + $("#ItemDescription").val() + "%', 'field': 'Brand' , 'UserID': " + getUserID() + " }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
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
                                    label: item.ItemBrand,
                                    value: item.ItemBrand,
                                    text: item.ItemBrand
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
        $(this).autocomplete('search', '')
    });

    $("#ItemDescription").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "AddEditCostEstimate.aspx/GetMaterial",
                data: "{ 'CompanyId': '" + getCoyID() + "', 'category': '%" + $("#ItemCategory").val() + "%', 'material': '%" + request.term + "', 'size': '%" + $("#ItemSize").val() + "%', 'suppliername': '%" + $("#SupplierName").val() + "%', 'brand': '%" + $("#ItemBrand").val() + "%', 'description': '%" + $("#ItemDescription").val() + "%', 'field': 'Description' , 'UserID': " + getUserID() + " }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
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
                                    label: item.ItemDescription,
                                    value: item.ItemDescription,
                                    text: item.ItemDescription
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
        select: function (e, i) {
       $("#ItemDescription").val(i.item.value);
        //if (i.item.label == 'Others') {
        //    $("#CurrencyCode").attr("readonly", true);
        //    $("#UOM").attr("readonly", false);
        //    $("#QuotedPrice").attr("readonly", false);
        //    $("#CurrencyRate").val("");
        //}
        //else {
        //    $("#CurrencyCode").attr("readonly", true);
        //    $("#UOM").attr("readonly", true);
        //    $("#QuotedPrice").attr("readonly", true);
        //}

        $.ajax({
            url: "AddEditCostEstimate.aspx/GetMaterial",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: "{ 'CompanyId': '" + getCoyID() + "', 'category': '%" + $("#ItemCategory").val() + "%', 'material': '%" + $("#ItemMaterial").val() + "', 'size': '%" + $("#ItemSize").val() + "%', 'suppliername': '%" + $("#SupplierName").val() + "%', 'brand': '%" + $("#ItemBrand").val() + "%', 'description': '%" + $("#ItemDescription").val() + "%', 'field': 'All' , 'UserID': " + getUserID() + " }",
            success: function (data) {
                $.map(data, function (item) {
                    $("#CurrencyCode").val(item.CurrencyCode);
                    $("#QuotedPrice").val(item.UnitPrice);
                    getRate();
                });
                //CalculateAmount(a, rowid ,rowIndex); 
            },
            error: function (xhr, ajaxOptions, thrownError) {
                SetModalMessage('Failed to retrieve detail.', '');
            }
        });

       
    },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
    }).click(function () {
        $(this).autocomplete('search', '')
    });

    $("#chkIsOthers").on("click", function () {
        if ($(this).is(':checked')) {
            //$("#ItemCategory").val("").attr('readonly', false);
            $("#ItemMaterial").val("").attr('readonly', false);
            $("#ItemSize").val("").attr('readonly', false);
            $("#SupplierName").val("").attr('readonly', false);
            $("#ItemBrand").val("").attr('readonly', false);
            $("#ItemDescription").val("").attr('readonly', false);
            $("#QuotedPrice").val("").attr('readonly', false);
        } else {
            //$("#ItemCategory").val("").attr('readonly', true);
            $("#ItemMaterial").val("").attr('readonly', true);
            $("#ItemSize").val("").attr('readonly', true);
            $("#SupplierName").val("").attr('readonly', true);
            $("#ItemBrand").val("").attr('readonly', true);
            $("#ItemDescription").val("").attr('readonly', true);
            $("#QuotedPrice").val("").attr('readonly', true);
        }
    });

    $("#ItemCategory").focusout(function () {
        if ($(this).val() == "LABOR" || $(this).val() == "LABOUR") {
            $("#ItemMaterial").val("").attr('readonly', true);
            $("#ItemSize").val("").attr('readonly', true);
        }
        else {
            if ($("#chkIsOthers").is(':checked')) {
                $("#ItemMaterial").val("").attr('readonly', false);
                $("#ItemSize").val("").attr('readonly', false);
            }

        }
    });

    /*Save as Draft*/
    $("#btnSubmitCEDraft").button().on('click', function(){
        SaveCostEstimateInfo();
    });
    
    /*Cancel C/E Form*/
    $("#btnSubmitCancellation").button().on("click", function(){
        SubmitCancellation(first, 'cancelled');
    });
    
     /*Approve C/E Form*/
    $("#btnApproveCE").button().on("click", function(){
        SubmitApproval(first);
    });
    
    /*Submit C/E Form for approval after confirm*/
    $("#btnSubmitforApproval1").button().on("click", function(){
        SubmitforApproval(first);
    });
    
    
    $("#btnConvertCE1").button().on("click", function(){
        ConvertCEForm(first);
        $("#displaybuttons").load();
    });
    
    $("#btnReviseCE1").button().on("click", function(){
        ReviseCEForm(first);
        RevisionList();
    });
    
    /*Save C/E Item Detail*/
    $("#btnCEDetailSubmit").button().on("click", function(){
        SaveItem(this);
    });
    
    /*Delete C/E Item Detail*/
    $("#btnDeleteItem").button().on("click", function(){
        DeleteCEItem(this.value);
    });

    $("#btnPrintCE").button().on("click", function(){
        pdfprint();
    });

    $("#btnClose").on("click", function () {
        $('#ModalMessage').modal('hide');
        if (this.value != '') {
            window.location.href = this.value;
        }
    });
});

function getRate(){
    var obj = {};
    obj['CEID'] = $( "#txtCEID" ).val();
    obj['Type'] = "CEID";
    obj['CurrencyCode'] = $("#CurrencyCode").val();
        $.ajax({
            url: "AddEditCostEstimate.aspx/GetCurrencyRate",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: '{Info: ' + JSON.stringify(obj) + '}',
            success: function (data) { 
                $.map(data, function(item) {
                   $("#CurrencyRate").val(item.MonthEndRate);
                    action = data.Action;
                });  
                //CalculateAmount(a, rowid ,rowIndex); 
            },
            error: function (xhr, ajaxOptions, thrownError) {
                SetModalMessage('Failed to retrieve detail.', '');
            }
        });
}

function CurrencyRate(item){
    var obj = {};
    obj['CEID'] = $( "#txtCEID" ).val();
    obj['Type'] = "CEID";
    $(item).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "AddEditCostEstimate.aspx/GetCurrencyList",
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
                                    label: item.CurrencyCode,
                                    value: item.CurrencyCode,
                                    text : item.CurrencyCode
                                }
                            })
                        )
                    }
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    SetModalMessage(textstatus, '');
                }
            });
        },
        select: function (e, i) {
            obj['CurrencyCode'] = i.item.value;
            $.ajax({
                url: "AddEditCostEstimate.aspx/GetCurrencyRate",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: '{Info: ' + JSON.stringify(obj) + '}',
                success: function (data) { 
                    $.map(data, function(item) {
                       $("#CurrencyRate").val(item.MonthEndRate);
                        action = data.Action;
                    });  
                    //CalculateAmount(a, rowid ,rowIndex); 
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    SetModalMessage('Failed to retrieve detail.', '');
                }
            });
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
    }).click(function() {
        $(this).autocomplete('search', '');
    });
}

/*
-----------------------------------------------------------------------------------------
Function Name   : showButton
Description     : display button based on status
Date Created    : 2016-06-20
Date Modified   : - 
Additional Info :   1	Draft
                    2	Pending for Approval
                    3	Approved
                    4	Converted -> Removed
                    5	Rejected
                    X	Cancelled

                    *btnCancelCE - shown when draft, pending, approved and ceid <> ''
                    *btnSubmitCEApproval - shown when draft and ceid <> ''
                    *btnPrintCE - shown pending for approval  and ceid <> ''
                    *btnConvertCE - shown approved  and ceid <> ''
                    *btnApproveCE - shown pending for approval  and ceid <> ''
                    *btnRejectCE - shown pending for approval  and ceid <> ''
                    *btnSubmitCEDraft shown when draft
-----------------------------------------------------------------------------------------
*/
function showButton(CEID, Revision){
    if(CEID!='null' ){
        if(Revision == 0){
            if($("#txtCEStatusName").val()=="Draft" || $("#txtCEStatusName").val()=="Pending for Approval" || $("#txtCEStatusName").val()=="Approved"){
                if($("#txtCEStatusName").val()=="Draft"){
                    CheckAccess(function(data){
                        if(data[0].AccessAddEditCE == "1"){
                            $("#btnCancelCE").css("display", '');
                            $("#btnSubmitCEApproval").css("display", '');
                            $("#btnSubmitCEDraft").css("display", ''); 
                            $("#btnPrintCE").css("display", 'none');
                            $("#btnApproveCE").css("display", 'none');
                            $("#btnConvertCE").css("display", 'none');
                            $("#btnAddToRevision").css("display", 'none');
                            $("#btnRejectCE").css("display", 'none');
                        }
                        else{
                            $("#btnCancelCE").css("display", 'none');
                            $("#btnSubmitCEApproval").css("display", 'none');
                            $("#btnSubmitCEDraft").css("display", 'none'); 
                            $("#btnPrintCE").css("display", 'none');
                            $("#btnApproveCE").css("display", 'none');
                            $("#btnConvertCE").css("display", 'none');
                            $("#btnAddToRevision").css("display", 'none');
                            $("#btnRejectCE").css("display", 'none');
                        }
                    }); 
                }
                else{
                    CheckAccess(function(data){
                        var AccessAdmin = data[0].AccessEngineeringAdmin;
                        var Converted = data[0].Converted;
                       
                        if($("#txtCEStatusName").val()=="Pending for Approval"){
                            if(AccessAdmin == "1"){
                                $("#btnApproveCE").css("display", '');
                                $("#btnRejectCE").css("display", '');
                            }
                            else{
                                $("#btnApproveCE").css("display", 'none');
                                $("#btnRejectCE").css("display", 'none');
                            }
                            
                            if(data[0].AccessAddEditCE == "1"){
                                $("#btnPrintCE").css("display", '');
                                $("#btnSubmitCEDraft").css("display", '');
                            }
                            else{
                                $("#btnPrintCE").css("display", 'none');
                                $("#btnSubmitCEDraft").css("display", 'none');
                            }
                        }
                        else if ($("#txtCEStatusName").val()=="Approved"){
                            if(AccessAdmin == "1"){
                                if(Converted == '0'){
                                    $("#btnConvertCE").css("display", '');
                                }
                                else{
                                    $("#btnConvertCE").css("display", 'none');
                                }
                            }
                            else{
                                $("#btnConvertCE").css("display", 'none');
                            }
                            
                            if(data[0].AccessAddEditCE == "1"){
                                $("#btnAddToRevision").css("display", 'none');
                                $("#btnSubmitCEDraft").css("display", 'none');
                                $("#btnPrintCE").css("display", 'none');
                            }
                            else{
                                $("#btnPrintCE").css("display", 'none');
                                $("#btnAddToRevision").css("display", 'none');
                                $("#btnSubmitCEDraft").css("display", 'none');
                            }

                            if (AccessAdmin == "1") {
                                $("#btnPrintCE").css("display", '');
                                $("#btnAddToRevision").css("display", '');
                            }
                        }
                    });
                    
                }
            }
            else{
                $("#btnAddToRevision").css("display", '');
            }
        }
        else{
            $("#CEHeaderInfo").find("button[id^='btn']").css("display", 'none');
        }
    }
    else{
        if($("#txtCEStatusName").val()=="Draft"){
            $("#btnSubmitCEDraft").css("display", '');
        }
        
    }
}

/*
-----------------------------------------------------------------------------------------
Function Name   : SubmitCancellation
Description     : Submit CE for cancellation
Date Created    : 2016-06-20
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/
function SubmitCancellation(first, type){
    if(type == 'rejected'){
        var str = $( "form" ).find("#RejectCEForm :input").serializeArray();
    }
    else{
        var str = $( "form" ).find("#CancelCEForm :input").serializeArray();
    }
    var fields = new Object();
    fields["CompanyId"] = getCoyID();
    fields["CEID"] = first;
    fields["Type"] = type;
    fields["UserID"] = getUserID();
    jQuery.each( str, function( i, field ) {
        fields[field.name] = field.value;
    });
    $.ajax({
        type        : "POST",
        url         : "AddEditCostEstimate.aspx/CancelCostEstimate",
        data        : '{Info: ' + JSON.stringify(fields) + '}',
        contentType : "application/json; charset=utf-8",
        dataType    : "json",
        success     : function (response) {
                        SetModalMessage("C/E form " + first + " has been " + type + "!", '');
                        location.reload();
                      },
        error       : function(xhr, textstatus, error){
                        SetModalMessage(textstatus, '');
                      }
    });
    
}

function SubmitApproval(first) {
    var message = "";
    var cansave = "true";
    var str = $("form").find("#CEHeaderInfo :input").serializeArray();
    jQuery.each(str, function (i, field) {
        if (field.name == 'AccountCode' || field.name == 'AccountName' || field.name == 'EngineerName' || field.name == 'IsBillable' || field.name == 'IsProgressiveClaim' || field.name == 'CurrencyCode' || field.name == 'TotalAmtQuoted' || field.name == 'ContractDateFrom' || field.name == 'ContractDateTo' ||
            field.name == 'CustomerPIC' || field.name == 'OfficePhone' || field.name == 'BillingAddress' || field.name == 'Description') {
            if (field.value == "") {
                cansave = "false";
                message = message + "<li>Please fill in " + fieldnames(field.name) + "</li>"
            }
        }
    });
    if (message == "" && cansave == "true") {
        $.ajax({
            type        : "POST",
            url         : "AddEditCostEstimate.aspx/ApproveCostEstimate",
            data        : '{"CompanyId":"'+getCoyID()+'", "CEID": "' + first + '", "UserID":"'+getUserID()+'"}',
            contentType : "application/json; charset=utf-8",
            dataType    : "json",
            success     : function (response) {
                            SetModalMessage("C/E form " + first + " has been approved!", '');
                            location.reload();
                          },
            error       : function (xhr, textstatus, error) {
                            SetModalMessage(textstatus, '');
                          }
        });
    }
    else {
        SetModalMessage(message, "");
    }
}


/*
-----------------------------------------------------------------------------------------
Function Name   : SubmitforApproval
Description     : Submit CE for approval
Date Created    : 2016-06-20
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/
function SubmitforApproval(first){
    var message = "";
    var cansave = "true";
    var str = $("form").find("#CEHeaderInfo :input").serializeArray();
    jQuery.each(str, function (i, field) {
        if (field.name == 'AccountCode' || field.name == 'AccountName' || field.name == 'EngineerName' || field.name == 'IsBillable' || field.name == 'IsProgressiveClaim' || field.name == 'CurrencyCode' || field.name == 'TotalAmtQuoted' || field.name == 'ContractDateFrom' || field.name == 'ContractDateTo' ||
            field.name == 'CustomerPIC' || field.name == 'OfficePhone' || field.name == 'BillingAddress' || field.name == 'Description') {
            if (field.value == "") {
                cansave = "false";
                message = message + "<li>Please fill in " + fieldnames(field.name) + "</li>"
            }
        }
    });

    if (message == "" && cansave == "true") {
        $.ajax({
            type: "POST",
            url: "AddEditCostEstimate.aspx/SubmitCostEstimate",
            data: '{"CompanyId":"' + getCoyID() + '", "CEID": "' + first + '", "UserID":"' + getUserID() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                SetModalMessage("C/E form " + first + " has been sent for approval!", location.reload());
                
            },
            error: function (xhr, textstatus, error) {
                SetModalMessage(textStatus, '');
            }
        });
    }
    else {
        SetModalMessage(message, "");
    }
}
/*
-----------------------------------------------------------------------------------------
Function Name   : CheckAccess
Description     : Check for user access
Date Created    : 2016-06-20
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/
function CheckAccess(callback){
    var str = getUrlVars()["CEID"];
    $.ajax({
        type        : "POST",
        url         : "AddEditCostEstimate.aspx/CheckUserAccess",
        contentType : "application/json; charset=utf-8",
        data        : '{"CompanyId":"'+getCoyID()+'", "DocNo": "' + str + '", "UserID":"'+getUserID()+'"}',
        dataType    : "json",
        success     : function (data) {
                        callback(data);
                      },
        error       : function(xhr, textstatus, error){
                        SetModalMessage(textstatus, '');
                      }
    }); 
}

function RevisionList(){
    var str = getUrlVars()["CEID"];
    var coyid = getCoyID();
    $.ajax({
        type        : "POST",
        url         : "AddEditCostEstimate.aspx/GetRevisionList",
        contentType : "application/json; charset=utf-8",
        data        : '{"DocNo": "' + str + '", "CompanyId": "'+getCoyID()+'", "UserID":"'+getUserID()+'"}',
        dataType    : "json",
        success     : function (data) {
                        $('#txtRevision').children().remove().end();
                        $.each(data, function (key, value) {
                            $('#txtRevision').append($("<option></option>").attr("value",value.Revision).text(value.RevisionName)); 
                        });
                      },
        error       : function(xhr, textstatus, error){
            SetModalMessage(textstatus, '');
                      }
    }); 
}

/*
-----------------------------------------------------------------------------------------
Function Name   : getUrlVars
Description     : Get and display CEID from page url
Date Created    : 2016-06-20
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/
function getUrlVars() 
{
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m,key,value) {
        vars[key] = value;
        
    });
    return vars;
}

/*
-----------------------------------------------------------------------------------------
Function Name   : GetFullAccountList
Description     : Get and display data from tbAccount based on min 3 chars
Date Created    : 2016-06-20
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/

function GetFullAccountList(item)
{
    $(item).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "AddEditCostEstimate.aspx/GetAccountList",
                data: "{'CompanyId': '"+getCoyID()+"', 'account': '" + item.value + "' }",
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
                    SetModalMessage(textStatus, '');
                }
            });
        },
        select: function (e, i) {
            if(i.item.value == "D000000000"){
                $('#txtAccountName').val("").attr("readonly", false);
            }
            else{
                $('#txtAccountName').val(i.item.text).attr("readonly", true); 
            }
               
        },
        minLength: 3    // MINIMUM 1 CHARACTER TO START WITH.
    });
}

function GetEngineerDetail(){
   
    $.ajax({
        type        : "POST",
        url         : "AddEditCostEstimate.aspx/GetEngineerInfo",
        contentType : "application/json; charset=utf-8",
        data        : '{"CompanyId": "' + getCoyID() + '", "UserID":"' + getUserID() + '"}',
        dataType    : "json",
        success     : function (data) {
                        $.map(data, function(item) { 
                            $("#txtEngineerID").val(item.EngineerID);
                            $("#txtEngineerName").val(item.EngineerName);
                        });
                      },
        error       : function(xhr, textstatus, error){
                        var r = jQuery.parseJSON(xhr.responseText);
                        SetModalMessage(r.Message, '');
                      }
    }); 
}

function DisplayCostEstimate(revision){
   
    var str = getUrlVars()["CEID"];
    var fields  = new Object();
    
    $.ajax({
        type        : "POST",
        url         : "AddEditCostEstimate.aspx/GetCostEstimateInfo",
        data        : '{"CompanyId":"'+getCoyID()+'","CEID": "' + str + '", "Revision": "'+revision+'", "UserID":"'+getUserID()+'"}',
        contentType : "application/json; charset=utf-8",
        dataType    : "json",
        success     : function (data) {
                        $.each(data, function (key, value) {
                            $.each(value, function (i, item) {
                                $('#txt'+i).val(item);
                            });
                        });
                        if($('#txtAccountCode').val() == "D000000000" ){
                            $('#txtAccountName').attr("readonly", false); 
                        }else{
                            $('#txtAccountName').attr("readonly", true); 
                        }
                        showButton(str, revision);
                        CEDetail(str, data[0].CEStatusName, revision);
                        
                      },
        error       : function(xhr, textstatus, error){
                        var r = jQuery.parseJSON(xhr.responseText);
                        var text = r.Message;
                        
                        if (text.toLowerCase().indexOf("permitted") >= 0) {
                            SetModalMessage(r.Message, 'CostEstimateSearch.aspx?CoyID='+getCoyID());
                            //window.location.href='CostEstimateSearch.aspx?CoyID='+getCoyID();
                        }
                      }
    }); 
     
}


/*
-----------------------------------------------------------------------------------------
Function Name   : SaveCostEstimateHeaderInfo
Description     : Save new record into tbProjectCostEstimate
Date Created    : 2016-06-20
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/
function SaveCostEstimateInfo() {
    //serialize 

    var str = $( "form" ).find("#CEHeaderInfo :input").serializeArray();
    var fields  = new Object();
    var message = "";
    var cansave = "true";

    jQuery.each(str, function (i, field) {
        if (field.name == 'AccountCode' || field.name == 'AccountName' || field.name == 'EngineerName' || field.name == 'IsBillable' || field.name == 'IsProgressiveClaim' || field.name == 'CurrencyCode' || field.name == 'TotalAmtQuoted' || field.name == 'ContractDateFrom' || field.name == 'ContractDateTo' ||
            field.name == 'CustomerPIC' || field.name == 'OfficePhone' || field.name == 'BillingAddress' || field.name == 'Description') {
            if (field.value == "")
            {
                cansave = "false";
                message = message + "<li>Please fill in " + fieldnames(field.name) + "</li>"
            }
        }
    });

    if (cansave == "true" && message == "") {
        var method = "SaveCostEstimateHeaderInfo";
        fields["IsActive"] = "True";
        fields["CompanyId"] = getCoyID();
        fields["UserID"] = getUserID();

        jQuery.each(str, function (i, field) {
            fields[field.name] = field.value;
        });

        if (fields["CEID"] != '') {
            method = "EditCostEstimateHeaderInfo";
        }

        $.ajax({
            type: "POST",
            url: "AddEditCostEstimate.aspx/" + method,
            data: '{Info: ' + JSON.stringify(fields) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $.map(response, function (item) {
                    if (item.CEID == fields["CEID"]) {
                        SetModalMessage("Success!", 'AddEditCostEstimate.aspx?CurrentLink=' + getCurrentLink() + '&CoyID=' + getCoyID() + '&&CEID=' + item.CEID);
                    }
                    else if (fields["CEID"] == '' && method == "SaveCostEstimateHeaderInfo") {
                        SetModalMessage("Success!", 'AddEditCostEstimate.aspx?CurrentLink=' + getCurrentLink() + '&CoyID=' + getCoyID() + '&&CEID=' + item.CEID);
                    }
                });
            },
            error: function (xhr, textstatus, error) {
                var r = jQuery.parseJSON(xhr.responseText);
                SetModalMessage(r.Message, '');
            }
        });
    }
    else
    {
        SetModalMessage(message, "");
    }
}


function CEDetail(first, status, revision){
    var table = $('#tblCostEstimateDetail').DataTable({
        "ajax": {
            "dataType": "json",
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "AddEditCostEstimate.aspx/GetCostEstimateList",
            "data": function (d){
                return "{ 'CEID': '" + first + "', 'Revision': '" + revision + "', 'CompanyId': '" + getCoyID() + "', 'UserID': '"+getUserID()+"' }";
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
                            ResetForm("AddEditCostEstimate");
                            if ($('#chkIsOthers').is(":checked")) {
                                $("#chkIsOthers").click();
                            }
                            $("#btnCEDetailSubmit").val("");
                            $('#AddEditCostEstimate').modal({backdrop: 'static', keyboard: false}) 
                }
            }
        ],
        "language": {
            "emptyTable": "No results found!"
        },
        "rowId": "CEDetailID",
        "columns": [
                { 
                    "data": "ItemDescription", 
                    "title":"Description"
                },
                {
                    "data": "Category",
                    "title": "Category"
                },
                {
                    "data": "ItemMaterial",
                    "title": "Material"
                },
                {
                     "data": "SupplierName",
                     "title": "Supplier Name"
                 },
                { 
                    "data": "ItemBrand", 
                    "title":"Item Brand"
                },
                {
                    "data": "ItemSize",
                    "title": "Item Size"
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
                    "title":"Currency Code"
                },
                { 
                    "data": "CurrencyRate", 
                    "title":"Currency Rate"
                },
                { 
                    "data": "QuotedPrice", 
                    "title":"Unit Price"
                },
                { 
                    "data": "MarkUpPrice", 
                    "title":"Mark-Up Price"
                },
                { 
                    "data": "TotalAmount", 
                    "title":"Total Amount"
                },
                {
                    "data": "TotalAmountInDocCurr",
                    "title": "Total Amount (*Rate)"
                },
                { 
                    "data": "Remarks", 
                    "title":"Remarks"
                },
                { 
                    "data": null , 
                    "title":"Action", 
                    "class":"action",
                    "render": function (data, type, row){
                            return "<button class='btn btn-primary btn-xs' onclick=\"EditCostEstimateItem('"+row.CEDetailID+"');return false;\" id='Edit' value='"+row.CEDetailID+"' name='Edit'>Edit</button>&nbsp;<button onclick=\"DeleteCostEstimateItem('"+row.CEDetailID+"');return false;\"  value='"+row.CEDetailID+"' name='Delete' class='btn btn-primary btn-xs'>Delete</button>";
                    },
                    
                }
        ]     
    });
      
    
    CheckAccess(function(data){
       if(status =='Draft' || status =='Pending for Approval'){    
            if(data[0].AccessAddEditCE=="1"){
                table.columns( '.action' ).visible( true );
                table.buttons().enable();
            }
            else{
                table.columns( '.action' ).visible( false );
                table.buttons().disable();
            }
        }
        else{
            table.columns( '.action' ).visible( false );
            table.buttons().disable();
        }
    });
}

function CalculateTotal(){
     var qty = $("#Quantity").val();
     var rate = $("#CurrencyRate").val();
     var unitprice = $("#QuotedPrice").val();
     var markup = $("#MarkUpPrice").val();
     
     if (markup > 0){
        var total = qty*markup;
     }
     else{
        var total = qty*unitprice;
     }
     $("#TotalAmount").val(total);
}

/*
-----------------------------------------------------------------------------------------
Function Name   : SaveCostEstimateItem
Description     : Save new record into tbProjectCostEstimateDetails
Date Created    : 2016-07-15
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/
function SaveItem(item) {
    //serialize 
    var str = $( "form" ).find("#AddEditCostEstimate :input").serializeArray();
    var fields  = new Object();
    var cansave = "true";
    var test = "";
    var message = "";
    var method = "SaveCostEstimateItem";

    fields["CompanyId"] = getCoyID();
    fields["CEID"] = $("#txtCEID").val();
    fields["Revision"] = $("#txtRevision").val();
    fields["IsActive"] = "True";
    fields["UserID"] = getUserID();
    
    jQuery.each(str, function (i, field) {
        if (field.name != "MarkUpPrice" && field.name != "ItemSize" && field.name != "Remarks" && field.name != "TotalAmount") {
            
            if (field.name == "ItemCategory")
                test = field.value;
            if (field.name == "ItemMaterial") {
                if ((test != "LABOR" && test != "LABOUR") && field.value == "") {
                    cansave = "false";
                    message = message + "<li>Please fill in " + field.name + "</li>";
                }
            }
            if (field.name != "ItemMaterial") {
                if (field.value == "") {
                    cansave = "false";
                    message = message + "<li>Please fill in " + field.name + "</li>";
                }
            }
        }
    });

    jQuery.each( str, function( i, field ) {
        fields[field.name] = field.value;
        if (field.name == "IsOthers")
        {
            fields["chkIsOthers"] = $("#chkIsOthers").checked ? "0" : "1";
        }
    });

    if(item.value!=''){
        fields["CEDetailID"] = item.value;
        method = "EditCostEstimateItem";
    }

    if (message == "" && cansave == "true") {
        $.ajax({
            type        : "POST",
            url         : "AddEditCostEstimate.aspx/"+method,
            data        : '{Info: ' + JSON.stringify(fields) + '}',
            contentType : "application/json; charset=utf-8",
            dataType    : "json",
            success     : function (response) {
                            SetModalMessage("Success", '');
                            CEDetail(fields["CEID"], $("#txtCEStatusName").val(), $("#txtRevision").val());
                            $('#AddEditCostEstimate').modal('hide');
                          },
            error       : function(xhr, textstatus, error){
                            SetModalMessage(textstatus, '');
                          }
        });
    }
    else {
        //  $('#SaveMaterialRecord').modal('hide');
        SetModalMessage(message, "");
    }
}

/*
-----------------------------------------------------------------------------------------
Function Name   : EditCostEstimateItem
Description     : Get existing record and populate modal dialog
Date Created    : 2016-06-20
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/
function EditCostEstimateItem(item){
   
    var str = $( "form" ).find("#AddEditCostEstimate :input").serializeArray();
    var fields = new Object();
    fields["CompanyId"] = getCoyID();
    fields["UserID"] = getUserID();
    fields["CEID"] = $("#txtCEID").val();
    fields["CEDetailID"] = item;
    
    $.ajax({
        type        : "POST",
        url         : "AddEditCostEstimate.aspx/GetCostEstimateItemDetail",
        data        : '{Info: ' + JSON.stringify(fields) + '}',
        contentType : "application/json; charset=utf-8",
        dataType    : "json",
        success     : function (data) {
                        if ($('#chkIsOthers').is(":checked")) {
                            $("#chkIsOthers").click();
                        }
                        $.each(data, function (key, value) {
                            $.each(value, function (i, item) {
                                $('#'+i).val(item);
                            });
                        });
                        
                        $('#btnCEDetailSubmit' ).val(item);
                        $('#AddEditCostEstimate').modal({backdrop: 'static', keyboard: false});  
                      },
        error       : function(xhr, textstatus, error){
                            SetModalMessage(textstatus, '');
                      }
    }); 
}

function DeleteCostEstimateItem(item){
    $('#btnDeleteItem' ).val(item);
    $('#DeleteCEItem').modal({backdrop: 'static', keyboard: false});  
}

function DeleteCEItem(item){
    var fields = new Object();
    fields["CompanyId"] = getCoyID();
    fields["CEID"] = $("#txtCEID").val();
    fields["CEDetailID"] = item;
    
    $.ajax({
        type        : "POST",
        url         : "AddEditCostEstimate.aspx/DeleteCEItem",
        data        : '{Info: ' + JSON.stringify(fields) + '}',
        contentType : "application/json; charset=utf-8",
        dataType    : "json",
        success     : function (data) {
                         SetModalMessage("Deleted successfully", '');
                         CEDetail(fields["CEID"], $("#txtCEStatusName").val(), $("#txtRevision").val());
                      },
        error       : function(xhr, textstatus, error){
                        SetModalMessage(textstatus, '');
                      }
    });   
}

function ConvertCEForm(item){
    var str = getUrlVars()["CEID"];
    $.ajax({
        type        : "POST",
        url         : "AddEditCostEstimate.aspx/ConvertCE",
        data        : '{"CompanyId":"'+getCoyID()+'","CEID": "' + item + '", "UserID":"'+getUserID()+'"}',
        contentType : "application/json; charset=utf-8",
        dataType    : "json",
        success     : function (data) {
                        SetModalMessage("Converted successfully. New project no. is " + data[0].ProjectNo, ''); 
                        $("#txtProjectNo").val(data[0].ProjectNo);
                        showButton(str, "0");
                      },
        error       : function(xhr, textstatus, error){
                            SetModalMessage(textstatus, '');
                      }
    });   
}

function ReviseCEForm(item){
    
    $.ajax({
        type        : "POST",
        url         : "AddEditCostEstimate.aspx/ReviseCE",
        data        : '{"CompanyId":"' + getCoyID() + '","CEID": "' + item + '", "UserID":"' + getUserID() + '"}',
        contentType : "application/json; charset=utf-8",
        dataType    : "json",
        success     : function (data) {
                        SetModalMessage("Revised successfully.", '');
                        DisplayCostEstimate(0);
                      },
        error       : function(xhr, textstatus, error){
                        SetModalMessage(textstatus,'');
                      }
    });   
}

function jsOpenOperationalReport( url )
{
    jsWinOpen2( url, 795, 580, 'yes');
}

function pdfprint(){
    var CEID = $("#txtCEID").val();
    jsOpenOperationalReport('/GMS3/Finance/BankFacilities/PdfReportViewer.aspx?REPORT=EngineeringCostEstimateFormWithLetterHead&&TRNNO=' + CEID + '&&REPORTID=-6');
}

function SetModalMessage(message, redirect) {
    $('#btnClose').val(redirect);
    $('#message').html(message);
    $('#ModalMessage').modal({ backdrop: 'static', keyboard: false });
}


function fieldnames(name){ 

    var field = "";

    if (name == 'AccountCode') { field = "Account Code"; }
    else if (name == 'AccountName') { field = "Account Name"; }
    else if (name == 'EngineerName') { field = "Engineer"; }
    else if (name == 'IsBillable') { field = "Billable"; }
    else if (name == 'IsProgressiveClaim') { field = "Progressive Claim"; }
    else if (name == 'CurrencyCode') { field = "Currency Code"; }
    else if (name == 'TotalAmtQuoted') { field = "Total Amt Quoted"; }
    else if (name == 'ContractDateFrom') { field = "Contract Date From"; }
    else if (name == 'ContractDateTo') { field = "Contract Date To"; }
    else if (name == 'CustomerPIC') { field = "Customer PIC"; }
    else if (name == 'OfficePhone') { field = "Office Phone"; }
    else if (name == 'BillingAddress') { field = "Billing Address"; }
    else if (name == 'Description') { field = "Description"; }
    
    return field;
}