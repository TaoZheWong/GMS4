

/* Edit Project JS Logs   */
/* 
Added Date      : 03/06/2016
Modified Date   :
Changes         :   
*/

$(document).ready(function() {
    $('#BillingAddressDropDown').click(function() {
       $("#txtBillingAddress").autocomplete( "search", "" );
    });
    
     $( "#PrintPC" ).on( "click dblclick", function() { 
           pdfprint("PC");
     });

     $("#PrintMR").on("click dblclick", function () {
         pdfprint("MR");
     });


    $("textarea[id*='txtBillingAddress']").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "PrjGeneralInfo.aspx/GetAccountBillingAddressList",
                data: "{ 'CompanyId': '" + getCoyID() + "', 'account': '" + $("#txtAccountCode").val() + "' }",
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
                    alert(textStatus);
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
    
    
    
    
    $('#btnSubmitInfo').click(function() {
        InsertUpdateProjectInfo();
    });

    if (getProjectNo() != "" || getProjectNo() != null) {
        GetProjectInfo();
    }
    
});

$(function () {
    // $('.date').datetimepicker({format: 'YYYY-MM-DD'});
    $('#datetimepicker1').datepicker({
        format: 'yyyy-mm-dd', autoclose: 1, ignoreReadonly: true
    });
    $('#txtContractDateTo').datepicker({
        format: 'yyyy-mm-dd', autoclose: 1, ignoreReadonly: true
    });
    $('#txtClosingDate').datepicker({
        format: 'yyyy-mm-dd', autoclose: 1, ignoreReadonly: true
    });
    
});

function GetProjectInfo() {
    $.ajax({
        url: "EditProject.aspx/GetProjectInformation",
        data: "{ 'CoyID': '" + getCoyID() + "', 'ProjectNo':'" + getProjectNo() + "' , 'UserId': '" + getUserID() + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            $.map(data, function (item) {
                //$('#txtProjectNo').attr("value", item.ProjectNo);
                //$('#txtPrevProjectNo').attr("value", item.PrevProjectNo);
                //$('#txtAccountCode').attr("value", item.AccountCode);
                //$('#txtAccountName').attr("value", item.AccountName);
                //$('#txtCEID').attr("value", item.CEID);
                //$('#txtCEStatusName').attr("value", item.CEStatusName);
                //$('#txtRefNo').attr("value", item.RefNo);
                //$('#txtStatusID option[value="' + item.StatusID + '"]').attr('selected', true);;
                //$('#txtSalesPersonID').attr("value", item.SalesPersonID);
                //$('#txtSalesPersonName').attr("value", item.SalesPersonName);
                //$('#txtEngineerID').attr("value", item.EngineerID);
                //$('#txtEngineerName').attr("value", item.EngineerName);
                //$('#txtIsBillable option[value="' + item.IsBillable + '"]').attr('selected', true);
                //$('#txtIsProgressiveClaim option[value="' + item.IsProgressiveClaim + '"]').attr('selected', true);
                $('#txtCurrencyCode option[value="'+item.CurrencyCode+'"]').attr('selected', true);
                $('#txtTotalBillableAmt').attr("value", item.TotalBillableAmt);
                $('#txtCECurrency').attr("value", item.CECurr);
                $('#txtTotalCE').attr("value", item.TotalCE);
                $('#txtTotalCurrentCE').attr("value", item.CurrCE);
                $('#txtTotalPrjCost').attr("value", item.TotalPrjCost);
                $('#txtTotalProfit').attr("value", item.TotalProfit);
                $('#txtClaimedToDate').attr("value", item.ClaimedToDate);
                $('#txtContractNo').attr("value", item.ContractNo);
                $('#txtContractDateFrom').attr("value", item.ContractDateFrom);
                $('#txtContractDateTo').attr("value", item.ContractDateTo);
                $('#txtCommencementDate').attr("value", item.CommencementDate);
                $('#txtCompletionDate').attr("value", item.CompletionDate);
                $('#txtClosingDate').attr("value", item.ClosingDate);
                $('#txtCustomerPO').attr("value", item.CustomerPO);
                $('#txtCustomerPIC').attr("value", item.CustomerPIC);
                $('#txtOfficePhone').attr("value", item.OfficePhone);
                $('#txtFaxNo').attr("value", item.Fax);
                $('#txtBillingAddress').val(item.BillingAddress).attr("value", item.BillingAddress);
                $('#txtOnsiteLocation').val(item.OnsiteLocation).attr("value", item.OnsiteLocation);
                $('#txtDescription').val(item.Description).attr("value", item.Description);
                $('#txtRemarks').val(item.Remarks).attr("value", item.Remarks);
            });
        },
        error: function (xhr, textstatus, error) {
            var r = jQuery.parseJSON(xhr.responseText);
            var text = r.Message;

            SetModalMessage(text, 'ProjectSearch.aspx?CoyID=' + getCoyID());

        }
    });
}

function InsertUpdateProjectInfo(){
    var method = "";
    var obj = {};
    obj['CompanyId'] = getCoyID();
    obj['UserID'] = getUserID();
    $( "input[id^='txt']" ).each(function() {
            if(this.id == 'txtContractDateFrom' && this.value == ""){
                obj[this.id]="1911-01-01";
            }
            else if(this.id == 'txtContractDateTo' && this.value == ""){
                obj[this.id]="1911-01-01";
            }
            else if(this.id == 'txtCommencementDate' && this.value == ""){
                obj[this.id]="1911-01-01";
            }
            else if(this.id == 'txtCompletionDate' && this.value == ""){
                obj[this.id]="1911-01-01";
            }
            else if(this.id == 'txtClosingDate' && this.value == ""){
                obj[this.id]="1911-01-01";
            }
            else
                obj[this.id]=this.value;
    });
    
    $( "textarea[id^='txt']" ).each(function() {
            obj[this.id]=this.value;
    });
    
    $( "select[id^='txt']" ).each(function() {
            obj[this.id]=this.value;
    });
    
    if($( "input[id^='txtProjectNo']" ).val()==null || $( "input[id^='txt']" ).val()=='')
    {
        method = 'InsertProjectInformation';
    }
    else
    {
        method = 'UpdateProjectInformation';
    }
    $.ajax({
        type: "POST",
        url: "PrjGeneralInfo.aspx/"+method,
        data: '{Info: ' + JSON.stringify(obj) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $.map(response, function(item) {
                var ID = item.ProjectNo;
                if (ID.indexOf("Prj") > 0 || ID.indexOf("Column1")> 0 ){
                    SetModalMessage('Unsuccessful', '');
                }
                else{
                    SetModalMessage('Success!', 'EditProject.aspx?CurrentLink='+getCurrentLink()+'&CoyID='+getCoyID()+'&ProjectNo='+item.ProjectNo);
                }
            });
        },
        error: function(xhr, status, error) {
            alert(error);
        }
    }); 
    
    
}

function pdfprint(type){
    var ProjectNo = getProjectNo();
    if (type == "PC")
        jsOpenOperationalReport('/GMS3/Finance/BankFacilities/PdfReportViewer.aspx?REPORT=ProjectCosting_'+getCoyID()+'&&TRNNO=' + ProjectNo.replace('#', '') + '&&REPORTID=-7');
    else if (type == "MR")
    jsOpenOperationalReport('/GMS3/Finance/BankFacilities/PdfReportViewer.aspx?REPORT=ProjectCostingMR_' + getCoyID() + '&&TRNNO=' + ProjectNo.replace('#', '') + '&&REPORTID=-8');
}

function jsOpenOperationalReport( url )
{
    jsWinOpen2( url, 795, 580, 'yes');
}
