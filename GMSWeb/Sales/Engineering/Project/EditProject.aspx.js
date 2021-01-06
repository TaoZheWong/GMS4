
/* Edit Project JS Logs   */
/* 
Added Date      : 03/06/2016
Modified Date   :
Changes         :   
*/

$(document).ready(function () {

    
    /* Initialize tabs */
    $('a[data-toggle="tab"]').on( 'shown.bs.tab', function (e) {
            $.fn.dataTable.tables( {visible: true, api: true} ).columns.adjust();
    });
    
    //$('.date').datetimepicker({format: 'YYYY-MM-DD'});
    
    $('#tab1').load('PrjGeneralInfo.aspx');
    $('#tab2').load('CostEstimate.aspx');
    $('#tab3').load('ProjectCost.aspx');
    $('#tab5').load('Payment.aspx');
    $('#tab6').load('Attachment.aspx');

    $('#CE').click(function () {
        CEDetail();
    });

    $('#Inv').click(function () {
        Payment();
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
            $.ajax({
                url: "ProjectSearch.aspx/GetAccountAddressList",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: "{'CompanyId':'"+getCoyID()+"' ,'account': '"+ i.item.value +"' }",
                success: function (data) {
                    data = data.hasOwnProperty('d') ? data.d : data;
                    $.map(data, function(item) {
                        $("select[id*=txtCurrencyCode] option[value='"+item.CurrencyCode+"']").attr("selected",true);
                        $("input[id*=txtSalesPersonID]").val(item.SalesPersonID);
                        $("input[id*=txtSalesPersonName]").val(item.SalesPersonID + ' - ' + item.SalesPersonName) ;
                        action = data.Action;
                    })
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('Failed to retrieve detail.');
                }
            });
        },
        minLength: 2    // MINIMUM 1 CHARACTER TO START WITH.
    });
        
   $("input[id*='txtEngineerName']").autocomplete({
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
                                    label: item.EngineerID + ' - ' + item.EngineerName,
                                    value: item.EngineerName,
                                    text : item.EngineerID
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
            $("input[id*=txtEngineerID]").attr("value", i.item.text);
        },
        minLength: 2    // MINIMUM 1 CHARACTER TO START WITH.
    });
    
    $("input[id*='txtSalesPersonName']").autocomplete({
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
                                    value: item.SalesPersonName,
                                    text : item.SalesPersonID
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
            $("input[id*=txtSalesPersonName]").attr("value", i.item.value);
            $("input[id*=txtSalesPersonID]").attr("value", i.item.text);
        },
        minLength: 2    // MINIMUM 1 CHARACTER TO START WITH.
    });
    
    if (getProjectNo() != "" || getProjectNo() != null) {
        GetProjectInfo();
    }

    $("#btnClose").on("click", function () {
        $('#ModalMessage').modal('hide');
        if (this.value != '') {
            window.location.href = this.value;
        }
    });
    
    $(".tab-v2").find("li").on("click", function () {
        if (this.id == "CE")
            $("#DivTotal").css("display", '');
        else
            $("#DivTotal").css("display", 'none');
        if (this.id == "PT")
            $("#DivPJTTotal").css("display", '');
        else
            $("#DivPJTTotal").css("display", 'none');
        if (this.id == "Inv")
            $("#DivInvTotal").css("display", '');
        else
            $("#DivInvTotal").css("display", 'none');
    });

    AllowChanges();
});

function CurrencyDropdown(){
$.ajax({
        url: "ProjectSearch.aspx/GetCurrencyList",
        data: "{}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function(data) { return data; },
        success: function (data) {
            data = data.hasOwnProperty('d') ? data.d : data;
            $.each(data, function (key, value) {
                $("select[id*=txtCurrencyCode]").append($("<option data-tokens='"+value.CurrencyCode+"'></option>").val(value.CurrencyCode).html(value.CurrencyCode));
            });
        }
    });
}

function getUrlVars() 
{
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m,key,value) {
        vars[key] = value;
    });
    return vars;
}

function GetProjectInfo()
{
    $.ajax({
        url: "EditProject.aspx/GetProjectInformation",
        data: "{ 'CoyID': '" + getCoyID() + "', 'ProjectNo':'" + getProjectNo() + "' , 'UserId': '" + getUserID() +"'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function(data) { return data; },
        success: function (data) {
            data = data.hasOwnProperty('d') ? data.d : data;
            $.map(data, function(item) {
                    $('#txtProjectNo').attr("value", item.ProjectNo);
                    $('#txtPrevProjectNo').attr("value", item.PrevProjectNo);
                    $('#txtAccountCode').attr("value", item.AccountCode);
                    $('#txtAccountName').attr("value", item.AccountName);
                    $('#txtCEID').attr("value", item.CEID);
                    $('#txtCEStatusName').attr("value", item.CEStatusName);
                    $('#txtRefNo').attr("value", item.RefNo);
                    $('#txtStatusID option[value="'+item.StatusID+'"]').attr('selected', true);;
                    $('#txtSalesPersonID').attr("value", item.SalesPersonID);
                    $('#txtSalesPersonName').attr("value", item.SalesPersonName);
                    $('#txtEngineerID').attr("value", item.EngineerID);
                    $('#txtEngineerName').attr("value", item.EngineerName);
                    $('#txtIsBillable option[value="'+item.IsBillable+'"]').attr('selected', true);
                    $('#txtIsProgressiveClaim option[value="' + item.IsProgressiveClaim + '"]').attr('selected', true);
                    $('#GrandTotal').attr("value", item.CECurr + " " + item.TotalCE);
                    $('#CurrentGrandTotal').attr("value", item.CECurr + " " + item.CurrCE);
                    $('#txtTotalCurrentCE').attr("value", item.CurrCE);
                    $('#MRSubTotal').attr("value", item.CurrencyCode + " " + item.ClaimedToDate);
                    $('#LaborSubTotal').attr("value", item.CurrencyCode + " " + item.Labor);
                    $('#MiscSubTotal').attr("value", item.CurrencyCode + " " + item.Misc);
                    $('#PJTTotal').attr("value", item.CurrencyCode + " " + item.TotalPrjCost);
                    $('#InvGrandTotal').attr("value", item.CurrencyCode + " "+item.ClaimedToDate);
            });
        },
        error: function (xhr, textstatus, error) {
            var r = jQuery.parseJSON(xhr.responseText);
            var text = r.Message;

            SetModalMessage(r.Message, 'ProjectSearch.aspx?CoyID=' + getCoyID());
            
        }
    });
}

/*
-----------------------------------------------------------------------------------------
Function Name   : CheckAccess
Description     : Check for user access
Date Created    : 2016-06-20
Date Modified   : - 
-----------------------------------------------------------------------------------------
*/
function CheckAccess(callback) {
    var str = getProjectNo();
    $.ajax({
        type: "POST",
        url: "EditProject.aspx/CheckUserAccess",
        contentType: "application/json; charset=utf-8",
        data: '{"CompanyId":"' + getCoyID() + '", "DocNo": "' + str + '", "UserID":"' + getUserID() + '"}',
        dataType: "json",
        success: function (data) {
            data = data.hasOwnProperty('d') ? data.d : data;
            callback(data);
        },
        error: function (xhr, textstatus, error) {
            SetModalMessage(textstatus, '');
        }
    });
}

function AllowChanges()
{
    CheckAccess(function (data) {
        var AccessEngineeringAdmin = data[0].AccessEngineeringAdmin;
        var PIC = data[0].PIC;
        var AccessViewAll = data[0].AccessViewAll;
        if (AccessEngineeringAdmin == "0" && PIC=="0") {
            SetModalMessage("You do not have permission to view the information of " + getProjectNo(), 'ProjectSearch.aspx?CoyID=' + getCoyID());
        }
        else if (AccessEngineeringAdmin == "0" && PIC == "1" && AccessViewAll == "0") {
            $("#txtEngineerID").attr("readonly", true);
            $("#txtEngineerName").attr("readonly", true);
            //$("#btnSubmitInfo").css("display", 'none');
            //$("#Print").css("display", 'none');
            //$("#btnMRCancel").css("display", 'none');
            //$("#btnMRSubmit").css("display", 'none');
        }
        else if (AccessEngineeringAdmin == "1") {
            $("#txtEngineerID").attr("readonly", false);
            $("#txtEngineerName").attr("readonly", false);
            //$("#btnSubmitInfo").css("display", '');
            //$("#Print").css("display", '');
            //$("#btnMRSubmit").css("display", '');
            //$("#btnMRCancel").css("display", '');
            
        }
    });
}


function SetModalMessage(message, redirect) {
    $('#btnClose').val(redirect);
    $('#message').html(message);
    $('#ModalMessage').modal({ backdrop: 'static', keyboard: false });
}