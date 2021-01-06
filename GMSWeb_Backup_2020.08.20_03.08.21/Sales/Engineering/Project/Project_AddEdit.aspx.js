
/* Project AddEdit JS    */
/*
Modified Date: 24/02/2016
Changes :   1. Remove "Insert new cost estimate" function
            2. Remove "Delete cost estimate record" function
            3. Remove "Add/Update cost estimate record" function
*/

    $(function() {
        //Initialize Ajax Tab
        $( "#tabs" ).tabs();
        
        //OnCheck Change Checkbox Value - Overide default value = "ON" 
        $('#IsBillable').on('click', function () {
            if($(this).val()=="1"){
                $(this).attr('value', "0");
            }
           else
                $(this).attr('value', "1");
        } );
        
        $('#IsProgressive').on('click', function () {
            if($(this).val()=="1"){
                $(this).attr('value', "0");
            }
            else
                $(this).attr('value', "1");
        });
        
        //Initialize Jquery DatePicker
        $( "#CommencementDate" ).datepicker({dateFormat: "yy-mm-dd"});
        
        $( "#CompletionDate" ).datepicker({dateFormat: "yy-mm-dd"});
        $( "#ClosingDate" ).datepicker({dateFormat: "yy-mm-dd"});
    });
    
    function date(){
        $( ".date" ).datepicker({dateFormat: "yy-mm-dd"});
        $( "input.date" ).datepicker({dateFormat: "yy-mm-dd"});
    }
    
    //Obtain ProjectID from page url
    function getUrlVars() 
    {
        var vars = {};
        var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m,key,value) {
            vars[key] = value;
        });
        return vars;
    }
    
    function money(rowid, field, numb)
    {
        
        var rd = parseFloat(Math.round(numb * 100) /100) || 0;
        var rdnum = rd.toFixed(2);
        
        if(rowid=='null'){
            $('#'+field).val(rdnum);
        }
        else{
            $('#'+field+'-'+rowid).val(rdnum);
        }
        
    }
    
    //Display Project Details if Project No is Provided
    function DisplayDetails(code){
                     
         $.ajax({
            url: "Project_AddEdit.aspx/ShowProjectDetail",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: "{ 'ProjectNo': '"+ code +"' }",

            success: function (data) {
                $.map(data, function(item) {
                    $('#AccountCode').val(item.AccountCode);
                    $('#AccountName').val(item.AccountName);
                    $('#txtBillingAddress').val(item.BillingAddress);
                    $('#OfficePhone').val(item.OfficePhone);
                    $('#OldProjectNo').val(item.OldProjectNo);
                    $("[id*=txtPrjNo]").val(item.ProjectNo);
                    $("#AccountName").val(item.AccountName);
                    $("#DONo").val(item.DONo);
                    $('#CustomerPIC').val(item.CustomerPIC);
                    $("#EngineerID").val(item.EngineerID);
                    $("#EngineerName").val(item.EngineerID + ' - '+ item.EngineerName);
                    $('#SalesPersonID').val(item.SalesPersonID);
                    $('#SalesPersonName').val(item.SalesPersonID + ' - '+ item.SalesPersonName);
                    $("#RefNo").val(item.RefNo);
                    $('#Default').val(item.DefaultCurrency);
                    $('#DefaultCurrency').val(item.DefaultCurrency);
                    $("#CostEstimate").val(item.CostEstimate);
                    $('#TotalCost').val(item.TotalCost);
                    $("#BillAmount").val(item.BillAmount);
                    $('#DONo').val(item.DONo);
                    if(item.DONo==null || item.DONo ==""){
                        $('#ConvertDO').show();
                    }
                    else{
                        $('#ConvertDO').hide()
                    }
                    if(item.IsBillable=="True"){
                        $("#IsBillable").attr("checked", true);
                        $("#IsBillable").val("1");
                    }
                    else{
                         $("#IsBillable").attr("checked", false);
                        $("#IsBillable").val("0");
                    }
                    if(item.IsProgressive=="True"){
                        $("#IsProgressive").attr("checked", true);
                        $('#IsProgressive').val("1");
                    }
                    else{
                        $("#IsProgressive").attr("checked", false);
                        $('#IsProgressive').val("0");
                    }
                    $("#CommencementDate").val(item.CommencementDate1);
                    $('#CompletionDate').val(item.CompletionDate);
                    $("#ClosingDate").val(item.ClosingDate);
                    $('#StatusID').val(item.StatusID);
                    $("#txtBillingAddress").val(item.BillingAddress);
                    $('#txtOnSiteLocation').val(item.OnSiteLocation);
                    $("#TelNo").val(item.OfficePhone);
                    $('#txtDescription').val(item.Description);
                    $("#txtContractNo").val(item.ContractNo);
                    $('#txtContractPeriodYear').val(item.ContractPeriodYear);
                    $("#txtContractPeriodMonth").val(item.ContractPeriodMonth);
                    $("#txtCustomerPO").val(item.CustomerPO);
                    
                    $("#txtProfit").val(item.Profit);
                    if(item.POAttachment=="1"){
                        $("[id*=lnkDownload]").show();
                    }
                    else{
                        $("[id*=lnkDownload]").hide();
                    }
                    if(item.WorkRequestForm=="1"){
                         $("[id*=lnkDll]").show();
                    }
                    else{
                        $("[id*=lnkDll]").hide();
                    }
                    $("#Fax").val(item.Fax);
                    $("#txtRemarks").val(item.Remarks);
                    action = data.Action;
                    
        
                })
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Failed to retrieve detail.');
            }
        });
        $("#ProjectNo").show();
    }
    
    //Display Account Details on Select AutoComplete
    function BindControls() {
        $("#AccountCode").autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: "Project_AddEdit.aspx/DisplayAccountCode",
                    data: "{ 'AccountCode': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        if(data==""){
                            alert("No data found");
                        }
                        response($.map(data, function(item) {
                        
                            return { 
                                label: item.AccountCode+ ' - ' + item.AccountName,
                                value: item.AccountCode 
                            }
                        }))
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            select: function (e, i) {
                 //fill selected customer details on form
                 $.ajax({
                    url: "Project_AddEdit.aspx/DisplayAccountDetail",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: "{ 'Code': '"+ i.item.value +"' }",

                    success: function (data) {
                        $.map(data, function(item) {
                            $('#AccountName').val(item.AccountName);
                            $('#DefaultCurrency').val(item.DefaultCurrency);
                            $("#txtBillingAddress").val(item.Address);
                            $("#OfficePhone").val(item.OfficePhone);
                            $("#Fax").val(item.Fax);
                            $("#SalesPersonID").val(item.SalesPersonID);
                            $("#SalesPersonName").val(item.SalesPersonName) ;

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
    }
    //AutoComplete
    function DisplayAutoComplete(){
        //SalesPerson Autocomplete
        $("#SalesPersonName").autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: "Project_AddEdit.aspx/DisplaySalesPersonList",
                    data: "{ 'SalesPerson': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        if(data==""){
                        alert("No data found");
                        }
                        response($.map(data, function(item) {
                            return { 
                            label: item.SalesPersonID + ' - ' + item.SalesPersonName,
                            value: item.SalesPersonID + ' - ' + item.SalesPersonName,
                            name : item.SalesPersonID
                            }
                        }))
                    },
                    
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
             select: function (event, ui) {
               $("#SalesPersonID").val(ui.item.name);
            },
            minLength: 2    // MINIMUM 1 CHARACTER TO START WITH.
        });
        
        //Engineer autocomplete
        $("#EngineerName").autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: "Project_AddEdit.aspx/DisplayEngineerList",
                    data: "{ 'Engineer': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        if(data==""){
                        alert("No data found");
                        }
                        response($.map(data, function(item) {
                            return { 
                            label: item.EngineerID +' - '+ item.EngineerName,
                            value: item.EngineerID +' - '+ item.EngineerName,
                            name : item.EngineerID
                            }
                        }))
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            select: function (event, ui) {
               $("#EngineerID").val(ui.item.name);
            },
            minLength: 2    // MINIMUM 1 CHARACTER TO START WITH.
        });
    }
    
    //Display DropDown
    function DisplayDropDown(){
        //Currency DropDownMenu
        $.ajax({
            url: "Project_AddEdit.aspx/DisplayCurrencyList",
            data: "{}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function(data) { return data; },
            success: function(data) {
                $.each(data, function (key, value) {
                    if(value.CurrencyCode==$("#Default").val()){
                        $("#DefaultCurrency").append($("<option selected='selected'></option>").val
                        (value.CurrencyCode).html(value.CurrencyCode));
                    }
                    else{
                        $("#DefaultCurrency").append($("<option></option>").val
                        (value.CurrencyCode).html(value.CurrencyCode));
                    }
                });
            }
        });
        
        //Status DropDownMenu
        $.ajax({
            url: "Project_AddEdit.aspx/ShowStatus",
            data: "{}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function(data) { return data; },
            success: function(data) {
                $.each(data, function (key, value) {
                    $("#StatusID").append($("<option></option>").val
                    (value.StatusID).html(value.StatusName));
                });
            }
        });
    }
    
    //Page Load Verification
    function PageLoad(){
        var first = getUrlVars()["ProjectNo"];
        var second = getUrlVars()["IsNew"];
        
        if(second=="True"){
         
            $("#ProjectNo").hide();
            $("#IsBillable").val("0");
            $("#IsProgressive").val("0");
            $("#StatusID").attr("disabled", "disabled");
//            $("#tabs" ).tabs({
//              disabled: [ 1, 2, 3, 4, 5 ]
//            });
            
        }
        
        else if(second=="False")
        {   
           $("#ProjectNo").hide();
            alert("Please select Project No");
            window.location="Project_Search.aspx";
        }   
        
        else if(first!=null){
          $("#ProjectNo").val(first);
          DisplayDetails(first);
          }
    }

  

/*Project AddEdit - Cost Estimate */     

    //Display UOM for existing CE Record
    function DisplayU() {
       $.ajax({
            url: "Project_AddEdit.aspx/DisplayUOMList",
            data: "{}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function(data) { return data; },
            success: function(data) {
                $.each(data, function (key, value) {
                    $('.UOM').append($("<option></option>").val
                    (value.UOM).html(value.UOM));
                });
            }
        });
     }
     
     //Initialize Datatable with Existing Data
    function TableRedraw(type){
        var table = $('#tblCostEstimate').DataTable( {
            "ajax": {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "Project_AddEdit.aspx/DisplayCostEstimate",
                "data": function(d){
                    return "{ 'ProjectNo': '" + $("#ProjectNo").val() + "' }";
                },
                "dataSrc": function (json) {   
                    
                    var prop;
                    var propCount = 0;

                    for (prop in json) {
                        propCount++;
                    }
                    if(propCount > 0){
                        $('#ImportCEForm').hide();
                    }
                    else{
                        $('#ImportCEForm').show();
                    }
                    return json; 
                 }
            },
            "bDestroy": true,
            "jQueryUI": true,
            "columns": [
                { "data": "Item", title:"Item Name"},
                { "data": "Material", title:"Material" },
                { "data": "Qty" , title:"Qty"},
                { "data": "UOM", title:"UOM" },
                { "data": "CurrencyCode" , title:"Currency"},
                { "data": "UnitPrice", title:"Unit Price" },
                { "data": "Amount", title:"Amount"},
                { "data": "Category", title:"Category" },
                { "data": "Remarks" , title:"Remarks"}
            ],
            "order":[0, 'desc']
        } );
     } 
     
    function alertvalue(a, b){
        $('#Cat-'+a).val(b);
    }
    
     function calculate(i, value) {
        var rdUP = parseFloat(Math.round(value * 100) /100) || 0;
        var rdnum = rdUP.toFixed(2);
        $('#ClaimAmount-'+i).val(rdnum);
    } 
    
    function hiddenRvalue(i) {
    alert('hello-' + $('#Remarks-'+i).val());
        var remarks = $('#Remarks-'+i).val();
        $('#Remark-'+i).val(remarks);
    } 
    
     //Initialize OtherClaims Table
     
 
    function OtherClaims(type){
    var i = 0;
        if(type == "add"){
            $('#tblAdditionalMR').empty();
        }
        var tableOC = $('#tblAdditionalMR').DataTable( {
            "ajax": {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "Project_AddEdit.aspx/GetOtherClaims",
                "data": function(d){
                    return "{ 'ProjectNo': '" + $("#ProjectNo").val() + "' }";
                },
                "dataSrc": function (json) {
                $.map(json, function(item) {
                       $("#OCCC").text($("#Default").val());
                       $("#OtherClaimsAmt").val(item.OCAmt);
                   });
                    return json;
                 }
            },
            "bDestroy": true,
            "jQueryUI": true,
            "columns": [
                { "data": "Description" },
                { "data": "Amount" },
                { "data": "Location" },
                { "data": "Remarks" },
                { "data": "OCId" }
            ],
            'columnDefs': [{
                 'targets': 0,
                 'searchable': true,
                 'orderable': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return "<textarea id='Description-"+ row.OCId +"' name='Description'  rows='4' cols='50' value='" + $('<div/>').text(data).html() + "' >" + $('<div/>').text(data).html() + "</textarea>";
                 }
              },
              {
                 'targets': 1,
                 'searchable': true,
                 'orderable': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return $('#DefaultCurrency').val()+"&emsp;<input type='text' onchange=\"money('"+ row.OCId +"', 'Amount', this.value);\" id='Amount-"+row.OCId+"' style='width:90px;text-align:right;' name='Amount' value='" + $('<div/>').text(data).html() + "'>";
                 }
              },
              {
                 'targets': 2,
                 'searchable': true,
                 'orderable': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return "<input type='text' id='Location-"+row.OCId+"' style='width:auto;' name='Location' value='" + $('<div/>').text(data).html() + "'>";
                 }
              },
              {
                 'targets': 3,
                 'searchable': true,
                 'orderable': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return '<pre><textarea  rows="4" cols="50" id="Remarks-'+row.OCId+'" style="width:auto;" name="Remarks" value="' + $('<div/>').text(data).html() + '">' + $('<div/>').text(data).html() + '</textarea></pre>';
                 }
              },
              {
                 'targets': 4,
                 'searchable': true,
                 'orderable': true,
                 'visible': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return "<button id='SubmitOC' label='Submit' onclick=\"EditOC('"+data+"');\"  value='" + data + "'>Submit</button>&nbsp&nbsp"+
                     "<button id='DeleteECE' label='Submit' onclick=\"DeleteOC('"+data+"');\"  value='" + data + "'>Delete</button>";
                 }
              }]
        } );
        
        if(type=="add"){
            tableOC.row.add( {
                "Description":"",
                "Amount":"",
                "Location":"",
                "Remarks":"",
                "OCId":i
            } ).draw();
           i++;
        }
     }
    
    function validateOC(a){
        var response="true";
        var up = $("#Amount-"+a).val();
        var rem = $("#Remarks-"+a).val();
        var desc = $("#Description-"+a).val();
        var countrem = $("#Remarks-"+a).val().length;
        var countdesc = $("#Description-"+a).val().length;
        if(!(up.match(/[0-9 -()+]+$/))){
            alert('Invalid Amount (Numbers/Decimals Only).');
            response = 'false';
        }
        if(rem!='' && countrem > 500){
            alert('Invalid Remarks. (0-500 characters only)');
            response = 'false';
        }
        if((desc!='' && countdesc > 500)  || desc=='' || desc==null){
            if(desc=='' || desc==null){
                alert('Description is Invalid. Cannot leave empty');
            }
            else if(desc!='' && countdesc > 500){
                alert('Invalid Description. (0-500 characters only)');
            }
            response = 'false';
        }
        return response;
    
    }
    //Edit OC Record
    function EditOC(a){
    
        var result = validateOC(a);
        if(a=='0'){
            if(result=='true'){
                var OCInfo = {};
                OCInfo.ProjectNo = $("#ProjectNo").val();
                OCInfo.Description = $("#Description-"+a).val();
                OCInfo.Amount = $("#Amount-"+a).val();
                OCInfo.Location = $("#Location-"+a).val();
                OCInfo.Remarks = $("#Remarks-"+a).val();
                
                $.ajax({
                    type: "POST",
                    url: "Project_AddEdit.aspx/InsertOtherClaims",
                    data: '{OCInfo: ' + JSON.stringify(OCInfo) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Project Info has been updated successfully.");
                        OtherClaims();
                        DisplayDetails($("#ProjectNo").val());
                    }
                }); 
            }
        }
        else{
        
            if(result=='true'){
                var OCInfo = {};
                OCInfo.ProjectNo = $("#ProjectNo").val();
                OCInfo.OCId = a;
                OCInfo.Description = $("#Description-"+a).val();
                OCInfo.Amount = $("#Amount-"+a).val();
                OCInfo.Location = $("#Location-"+a).val();
                OCInfo.Remarks = $("#Remarks-"+a).val();
                
                $.ajax({
                    type: "POST",
                    url: "Project_AddEdit.aspx/UpdateOCInfo",
                    data: '{OCInfo: ' + JSON.stringify(OCInfo) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Project Info has been updated successfully.");
                        OtherClaims();
                        DisplayDetails($("#ProjectNo").val());
                    }
                }); 
            }
        
        }
    }
    
    //Insert New Row
     function InsertNewOCRow(a){
        OtherClaims("add");

    }
    
    //Delete OC Record
    function DeleteOC(a){
        $( "#dialog-confirm1" ).dialog({
            resizable: false,
            height:140,
            modal: true,
            buttons: {
                "Delete all items": function() {
                    var OCInfo = {};
                    OCInfo.ProjectNo = $("#ProjectNo").val();
                    OCInfo.OCId = a;
               
                    $.ajax({
                        type: "POST",
                        url: "Project_AddEdit.aspx/DeleteOC",
                        data: '{OCInfo: ' + JSON.stringify(OCInfo) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            alert("Record have been deleted successfully.");
//                            $('#tblAdditionalMR').dataTable().empty();
                            OtherClaims();
                            DisplayDetails($("#ProjectNo").val());
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
     
     
     function LaborCost(type){
        var i = 0;
        
        $('#tblLaborCost').empty();
        var table = $('#tblLaborCost').DataTable( {
            "ajax": {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "Project_AddEdit.aspx/GetLaborCost",
                "data": function(d){
                    return "{ 'ProjectNo': '" + $("#ProjectNo").val() + "' }";
                },
                "dataSrc": function (json) {
                   $.map(json, function(item) {
                       $("#LCCC").text($("#Default").val());
                       $("#LaborCostAmt").val(item.LCAmt);
                   });
                    return json;
                 }
            },
            "bDestroy": true,
            "jQueryUI": true,
            "columns": [
                { "data": "Period" , "title": "Period"},
                { "data": "PIC" ,"title":"Person In Charge"},
                { "data": "Hour", "title": "Hour" },
                { "data": "Rate", "title": "Rate" },
                { "data": "Amount", "title": "Amount" },
                { "data": "Remarks", "title": "Remarks" },
                { "data": "LCId", "title": "Action" }
            ],
            'columnDefs': [
              {
                
                 'targets': 0,
                 'searchable': true,
                 'orderable': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return "<input type='text' class='date' id='Period-"+ row.LCId +"' onclick=\"$('#Period-"+row.LCId+"').datepicker({dateFormat: 'M-yy'});$('#Period-"+row.LCId+"').datepicker('show');\" name='Period'  value='" + $('<div/>').text(data).html() + "' />";
                 }
              },
              {
                 'targets': 1,
                 'searchable': true,
                 'orderable': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return "<input type='text' id='PIC-"+row.LCId+"' name='PIC' style='text-align:right;' value='" + $('<div/>').text(data).html() + "'>";
                 }
              },
              {
                 'targets': 2,
                 'searchable': true,
                 'orderable': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return "<input type='text' id='Hour-"+row.LCId+"' name='Hour' style='text-align:right;' onchange=\"calculateLabor('"+row.LCId+"');\" value='" + $('<div/>').text(data).html() + "'>";
                 }
              },
              {
                 'targets': 3,
                 'searchable': true,
                 'orderable': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return "<input type='text' id='Rate-"+row.LCId+"' style='width:auto;text-align:right;' onchange=\"calculateLabor('"+row.LCId+"');\" name='Rate' value='" + $('<div/>').text(data).html() + "'>";
                 }
              },
              {
                 'targets': 4,
                 'searchable': true,
                 'orderable': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return $('#DefaultCurrency').val()+'&emsp;<input type="text" id="Amount-'+row.LCId+'" style="width:auto;text-align:right;" name="Amount" value="' + $('<div/>').text(data).html() + '" readonly="readonly"/>';
                 }
              },
              {
                 'targets': 5,
                 'searchable': true,
                 'orderable': true,
                 'visible': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                    return "<textarea  rows='4' cols='50' id='Remarks-"+row.LCId+"' style='width:auto;' name='Remarks' value='" + $('<div/>').text(data).html() + "'>" + $('<div/>').text(data).html() + "</textarea>";
                 }
              },
              {
                 'targets': 6,
                 'searchable': false,
                 'orderable': false,
                 'visible': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return "<button id='SubmitLC' label='Submit' onclick=\"EditLC('"+data+"');\"  value='" + data + "'>Submit</button>&nbsp&nbsp"+
                     "<button id='DeleteLabor' label='Submit' onclick=\"DeleteLC('"+data+"');\"  value='" + data + "'>Delete</button>";
                 }
              }],
              "order":[1, "desc"]
            
        } );
        
        if(type=="add"){
            table.row.add( {
                "Period":       "",
                "Hour":   "",
                "Rate":     "",
                "Amount": "",
                "Remarks":     "",
                "LCId":       i,
                "LCAmt":       ""
            } ).draw(false);
           i++;
        }
     }
    
 
     function resize(a){
        $( "#Remarks-"+a ).resizable({
          handles: "se"
        });       
     } 
     function validateLC(a){
        var response="true";
        
        var rate = $("#Rate-"+a).val();
        var hrs = $("#Hour-"+a).val();
        var rem = $("#Remarks-"+a).val();
        
        if(!(rate.match(/[0-9 -()+]+$/))){
            alert('Invalid Rate (Numbers/Decimals Only).');
            response = 'false';
        }
        if(!(hrs.match(/[0-9 -()+]+$/))){
            alert('Invalid Hours (Numbers/Decimals Only).');
            response = 'false';
        }
        if(rem!='' && !(rem.match(/^.{0,100}$/))){
            alert('Invalid Remarks. (0-100 characters only)');
            response = 'false';
        }
        return response;
    
    }
    
    // Calculate Amount for each LC Record
    function calculateLabor(i) {
        var rdRt = $('#Rate-'+i).val();
        var rd = parseFloat(Math.round(rdRt * 100) /100) || 0;
        var rdnum = rd.toFixed(2);
        $('#Rate-'+i).val(rdnum);
        
        var sum = $('#Rate-'+i).val() * $('#Hour-'+i).val();
        var rds = parseFloat(Math.round(sum * 100) /100) || 0;
        var rdsum = rds.toFixed(2);
        $('#Amount-'+i).val(rdsum);
    } 
    
    //Edit LC Record
    function EditLC(a){
   var result = validateLC(a);
        if(a=="0"){
            
            if(result=='true'){
                var LCInfo = {};
                LCInfo.ProjectNo = $("#ProjectNo").val();
                LCInfo.Period = $("#Period-"+a).val();
                LCInfo.PIC = $("#PIC-"+a).val();
                LCInfo.Hour = $("#Hour-"+a).val();
                LCInfo.Rate = $("#Rate-"+a).val();
                LCInfo.Amount = $("#Amount-"+a).val();
                LCInfo.Remarks = $("#Remarks-"+a).val();
            
                $.ajax({
                    type: "POST",
                    url: "Project_AddEdit.aspx/InsertLaborCost",
                    data: '{LCInfo: ' + JSON.stringify(LCInfo) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Project Info has been updated successfully.");
                        DisplayDetails($("#ProjectNo").val());
                        //a.Destroy();
                        //$('#tblLaborCost').dataTable().empty();
                        LaborCost();
                       
                    }
                }); 
            }
        }
        else{
           
            if(result=='true'){
                var LCInfo = {};
                LCInfo.ProjectNo = $("#ProjectNo").val();
                LCInfo.LCId = a;
                LCInfo.Period = $("#Period-"+a).val();
                LCInfo.PIC = $("#PIC-"+a).val();
                LCInfo.Hour = $("#Hour-"+a).val();
                LCInfo.Rate = $("#Rate-"+a).val();
                LCInfo.Amount = $("#Amount-"+a).val();
                LCInfo.Remarks = $("#Remarks-"+a).val();
            
                $.ajax({
                    type: "POST",
                    url: "Project_AddEdit.aspx/UpdateLCInfo",
                    data: '{LCInfo: ' + JSON.stringify(LCInfo) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Project Info has been updated successfully.");
                        DisplayDetails($("#ProjectNo").val());
                        //a.Destroy();
                        //$('#tblLaborCost').dataTable().empty();
                        LaborCost();
                       
                    }
                }); 
            }
        }
    }
    
    //Insert New LC Row
    function InsertNewLCRow(a){
        LaborCost("add");
    }
    
    //Delete LC Record
    function DeleteLC(a){
        $( "#dialog-confirm2" ).dialog({
            resizable: false,
            height:140,
            modal: true,
            buttons: {
                "Delete all items": function() {
                    var LCInfo = {};
                    LCInfo.ProjectNo = $("#ProjectNo").val();
                    LCInfo.LCId = a;
               
                    $.ajax({
                        type: "POST",
                        url: "Project_AddEdit.aspx/DeleteLC",
                        data: '{LCInfo: ' + JSON.stringify(LCInfo) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            alert("Record have been deleted successfully.");
                            
                            //a.Destroy();
                            //$('#tblLaborCost').dataTable().empty();
                            LaborCost();
                            DisplayDetails($("#ProjectNo").val());
                            
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
     
     //Display SerialNo on Select AutoComplete

    
       
     function maintenanceRecord(type){
     var i = 0;
     if(type=="add"){
        $('#tblMaintenance').empty();
     }
     
        var table = $('#tblMaintenance').DataTable( {
            "ajax": {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "Project_AddEdit.aspx/GetMaintenance",
                "data": function(d){
                    return "{ 'ProjectNo': '" + $("#ProjectNo").val() + "' }";
                },
                "dataSrc": function (json) {
                    return json;
                 }
            },
            "bDestroy": true,
            "jQueryUI": true,
            "searchable": false,
            "columns": [
                { "data": "SerialNo", title: "SerialNo" },
                { title: "Extra Info"},
                { "data": "Location", title: "Location"},
                { "data": "InstallationDate", title: "Installation Date" },
                { "data": "ReturnDate", title: "Return Date" },
                { "data": "Period", title: "Period" },
                { "data": "VacuumReading", title: "Vacuum Reading" },
                { "data": "CheckedBy" , title: "Checked By"},
                { "data": "CheckedDate", title: "Checked Date" },
                { "data": "InspectionType", title: "Inspection Type" },
                { "data": "IsCOP", title: "Is COP" },
                { "data": "Remarks", title: "Remarks" },
                { "data": "MTId", title: "MTId" }
            ],
            'columnDefs': [{
                 'targets': 0,
                 'searchable': true,
                 'orderable': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return "<input type='text' id='SerialNo-"+ row.MTId +"' class='SerialN'  onkeyup='DisplaySerialNo();' value='" + $('<div/>').text(data).html() + "' /> "+
                     "<input type='hidden' id='CD-"+ row.MTId +"' value='" + row.CheckedDate + "' />"+
                     "<input type='hidden' id='CB-"+ row.MTId +"' value='" + row.CheckedBy + "' />"+
                     "<input type='hidden' id='IT-"+ row.MTId +"' value='" + row.InspectionType + "' />"+
                     "<input type='hidden' id='IC-"+ row.MTId +"' value='" + row.IsCOP + "' />"+
                     "<input type='hidden' id='Rem-"+ row.MTId +"' value='" + row.Remarks + "' />";
                 }
              },
              {
                 'targets': 1,
                 'searchable': false,
                 'orderable': false,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return "<span id='Extra-"+row.MTId+"' onclick=\"extraInfo('"+row.MTId+"');\"><b>+Info</b></span>";
                 }
              },
              {
                 'targets': 2,
                 'searchable': true,
                 'orderable': true,
                 'className': 'dt-body-center',
                 "sWidth": "50px",
                 'render': function (data, type, row){
                     return "<input type='text' id='Location-"+row.MTId+"' name='Hour' value='" + $('<div/>').text(data).html() + "'>";
                 }
              },
              {
                 'targets': 3,
                 'searchable': true,
                 'orderable': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return "<input type='text' class='date' id='InstallationDate-"+row.MTId+"'  onclick=\"$('#InstallationDate-"+row.MTId+"').datepicker({dateFormat: 'yy-mm-dd'});$('#InstallationDate-"+row.MTId+"').datepicker('show');\" style='width:90px;' value='" + $('<div/>').text(data).html() + "'>";
                 }
              },
              {
                 'targets': 4,
                 'searchable': true,
                 'orderable': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return "<input type='text' class='date' id='ReturnDate-"+row.MTId+"' onclick=\"$('#ReturnDate-"+row.MTId+"').datepicker({dateFormat: 'yy-mm-dd'});$('#ReturnDate-"+row.MTId+"').datepicker('show');\" style='width:90px;' value='" + $('<div/>').text(data).html() + "'/>";
                 }
              },
              
              {
                 'targets': 5,
                 'searchable': true,
                 'orderable': true,
                 'visible': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                    return '<input type="text" id="Period-'+row.MTId+'" style="width:90px;" value="' + $('<div/>').text(data).html() + '"/>';
                 }
              },
              {
                 'targets': 6,
                 'searchable': true,
                 'orderable': true,
                 'visible': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                    return '<input type="text" id="VacuumReading-'+row.MTId+'" style="width:50px;" value="' + $('<div/>').text(data).html() + '"/>';
                 }
              },
              {
                 'targets': 7,
                 'searchable': true,
                 'orderable': true,
                 'visible': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                    return "<input type='text'  id='CheckedBy-"+row.MTId+"' style='width:90px;'  onchange=\"alertval('Checkedby','"+row.MTId+"', this.value);\" value='" + $('<div/>').text(data).html() + "'/>";
                 }
              },
              {
                 'targets': 8,
                 'searchable': true,
                 'orderable': true,
                 'visible': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                    return "<input type='text' class='date' id='CheckedDate-"+row.MTId+"' style='width:90px;'   onclick=\"$('#CheckedDate-"+row.MTId+"').datepicker({dateFormat: 'yy-mm-dd'});$('#CheckedDate-"+row.MTId+"').datepicker('show');alertval('CheckedDate','"+row.MTId+"', this.value);\"  value='" + $('<div/>').text(data).html() + "'/>";
                 }
              },
              {
                 'targets': 9,
                 'searchable': true,
                 'orderable': true,
                 'visible': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                    return "<select id='InspectionType-"+row.MTId+"' style='width:auto;' value='" + $('<div/>').text(data).html() + "' onchange=\"alertval('InspectionType','"+row.MTId+"', this.value);\"><option value='Commission'>Commission</option><option value='Maintenance'>Maintenance</option></select>";
                 }
              },
              {
                 'targets': 10,
                 'searchable': true,
                 'orderable': true,
                 'visible': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                    return "<select id='IsCOP-"+row.MTId+"' style='width:auto;' value='" + $('<div/>').text(data).html() + "' onchange=\"alertval('IsCOP','"+row.MTId+"', this.value);\"><option value='1'>YES</option><option value='0'>NO</option></select>";
                 }
              },
              {
                 'targets': 11,
                 'searchable': true,
                 'orderable': true,
                 'visible': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                    return "<input type='text'  id='Remarks-"+row.MTId+"' style='width:auto;'   onchange=\"alertval('Remarks','"+row.MTId+"', this.value);\"  value='" + $('<div/>').text(data).html() + "'/>";
                 }
              },
              {
                 'targets': 12,
                 'searchable': false,
                 'orderable': false,
                 'visible': true,
                 'className': 'dt-body-center',
                 'render': function (data, type, row){
                     return "<button id='SubmitMaintenance' label='Submit' onclick=\"EditMT('"+data+"');\"  value='" + data + "'>Submit</button>&nbsp&nbsp"+
                     "<button id='DeleteMaintenance' label='Submit' onclick=\"DeleteMT('"+data+"');\"  value='" + data + "'>Delete</button>";
                 }
              }]
        } );
                
        date();
        
                
        if(type=="add"){
            table.row.add( {
                "SerialNo":       "",
                "null": "",
                "Location":     "",
                "InstallationDate": "",
                "ReturnDate":     "",
                "Period": "",
                "VacuumReading":     "",
                "CheckedBy": "",
                "CheckedDate":     "",
                "InspectionType": "",
                "IsCOP":     "",
                "Remarks": "",
                "MTId":    i
               
                
            } ).draw();
            i++;
        }
     }
     
    function alertval(a, b, c){
        if(a=='Checkedby'){
            $('#CB-'+b).val(c);
            
        }
        if(a=='CheckedDate'){
            $('#CD-'+b).val(c);
            $(this).val(c);
        }
        if(a=='InspectionType'){
            $('#IT-'+b).val(c);
            $(this).val(c);
        }
        if(a=='IsCOP'){
            $('#IC-'+b).val(c);
            $(this).val(c);
        }
        if(a=='Remarks'){
        
            $('#Rem-'+b).val(c);
            $(this).val(c);
        }
    }
    
    function date1(b){
        $('#CheckedDate-'+b).datepicker({dateFormat: "yy-mm-dd"});
    }
     //Maintenance
     function validateMT(a){
        var response="true";
        
        
        var sn = $("#SerialNo-"+a).val();
        var rem = $("#Remarks-"+a).val();
        
        if(sn==''){
            alert('Invalid Serial No.');
            response = 'false';
        }
        if(rem!='' && !(rem.match(/^.{0,100}$/))){
            alert('Invalid Remarks. (0-100 characters only)');
            response = 'false';
        }
        return response;
    
    }
    
    //Edit LC Record
    function EditMT(a){
        var result = validateMT(a);
        if(a=='0'){
            if(result=='true'){
                var MTInfo = {};
                MTInfo.ProjectNo = $("#ProjectNo").val();
                MTInfo.SerialNo = $("#SerialNo-"+a).val();
                MTInfo.Location = $("#Location-"+a).val();
                if($("#InstallationDate-"+a).val()==''){
                    MTInfo.InstallationDate = '1900-01-01';
                }
                else{
                    MTInfo.InstallationDate = $("#InstallationDate-"+a).val();
                }
                if($("#ReturnDate-"+a).val()==''){
                    MTInfo.ReturnDate = '1900-01-01';
                }
                else{
                    MTInfo.ReturnDate = $("#ReturnDate-"+a).val();
                }
                if($("#CD-"+a).val()==''){
                    MTInfo.CheckedDate = '1900-01-01';
                }
                else{
                    MTInfo.CheckedDate = $("#CD-"+a).val();
                }
               
                MTInfo.Remarks = $("#Rem-"+a).val();
                MTInfo.Period = $("#Period-"+a).val();
                MTInfo.VacuumReading = $("#VacuumReading-"+a).val();
                MTInfo.CheckedBy = $("#CB-"+a).val();
                MTInfo.InspectionType = $("#IT-"+a).val();
                MTInfo.IsCOP = $("#IC-"+a).val();
            
                $.ajax({
                    type: "POST",
                    url: "Project_AddEdit.aspx/InsertMaintenance",
                    data: '{MTInfo: ' + JSON.stringify(MTInfo) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Project Info has been updated successfully.");
                        $('#tblMaintenance').dataTable().empty();
                        maintenanceRecord();
                       
                    }
                }); 
            }
        }
        else{
        
            if(result=='true'){
                var MTInfo = {};
                MTInfo.ProjectNo = $("#ProjectNo").val();
                MTInfo.MTId = a;
                MTInfo.SerialNo = $("#SerialNo-"+a).val();
                MTInfo.Location = $("#Location-"+a).val();
                MTInfo.InstallationDate = $("#InstallationDate-"+a).val();
                MTInfo.ReturnDate = $("#ReturnDate-"+a).val();
                MTInfo.Remarks = $("#Rem-"+a).val();
                MTInfo.Period = $("#Period-"+a).val();
                MTInfo.VacuumReading = $("#VacuumReading-"+a).val();
                MTInfo.CheckedBy = $("#CB-"+a).val();
                MTInfo.CheckedDate = $("#CD-"+a).val();
                MTInfo.InspectionType = $("#IT-"+a).val();
                MTInfo.IsCOP = $("#IC-"+a).val();
            
                $.ajax({
                    type: "POST",
                    url: "Project_AddEdit.aspx/UpdateMTInfo",
                    data: '{MTInfo: ' + JSON.stringify(MTInfo) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Project Info has been updated successfully.");
                        $('#tblMaintenance').dataTable().empty();
                        maintenanceRecord();
                       
                    }
                }); 
            }
        
        }
    }
    
    //Insert New LC Row
    function InsertNewMTRow(a){
        maintenanceRecord('add');
    }
    
    //Delete LC Record
    function DeleteMT(a){
        $( "#dialog-confirm3" ).dialog({
            resizable: false,
            height:140,
            modal: true,
            buttons: {
                "Delete all items": function() {
                    var MTInfo = {};
                    MTInfo.ProjectNo = $("#ProjectNo").val();
                    MTInfo.MTId = a;
               
                    $.ajax({
                        type: "POST",
                        url: "Project_AddEdit.aspx/DeleteMT",
                        data: '{MTInfo: ' + JSON.stringify(MTInfo) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            alert("Record have been deleted successfully.");
                            $('#tblMaintenance').dataTable().empty();
                            maintenanceRecord();
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
     
     //Display SerialNo on Select AutoComplete
    function DisplaySerialNo() {
        $("input.SerialN").autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: "Project_AddEdit.aspx/DisplaySerialNo",
                    data: "{ 'SerialNo': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        if(data==""){
                            alert("No data found");
                        }
                        response($.map(data, function(item) {
                        
                            return { 
                                label: item.SerialNo+ ' - ' + item.GasTypeName,
                                value: item.SerialNo 
                            }
                        }))
                    },
                    
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            minLength: 2    // MINIMUM 1 CHARACTER TO START WITH.
        });
    }
    
    
    function ProgressiveClaim(type){

         $('#tblProgressiveClaim').empty();
            var table = $('#tblProgressiveClaim').DataTable( {
                "ajax": {
                    "dataType": "json",
                    "contentType": "application/json; charset=utf-8",
                    "type": "POST",
                    "url": "Project_AddEdit.aspx/GetProgressiveClaim",
                    "data": function(d){
                        return "{ 'ProjectNo': '" + $("#ProjectNo").val() + "' }";
                    },
                    "dataSrc": function (json) {
                        return json;
                     }
                },
                "bDestroy": true,
                "jQueryUI": true,
                "responsive":true,
                "columns": [
                    { "data": "ClaimOrder",  title: "Claim Order" },
                    { "data": "ClaimDate", title: "Claim Date" },
                    { "data": "ClaimAmount", title: "Claim Amount"},
                    { "data": "Ref", title: "Ref"},
                    { "data": "DONo", title: "DO No."},
                    { "data": "DODate", title: "DO Date"},
                    { "data": "Retention", title: "Retention"},
                    { "data": "Remarks", title: "Remarks"},
                    
                    { "data": "PCId", title: "Action"}
                ],
                'columnDefs': [{
                     'targets': 0,
                     'searchable': true,
                     'orderable': true,
                     'className': 'dt-body-center',
                     'render': function (data, type, row){
                         return "<input type='text' id='ClaimOrder-"+ row.PCId +"' name='ClaimOrder'  value='" + $('<div/>').text(data).html() + "' />";
                     }
                  },
                  {
                     'targets': 1,
                     'searchable': true,
                     'orderable': true,
                     'className': 'dt-body-center',
                     'render': function (data, type, row){
                         return "<input type='text' class='date' id='ClaimDate-"+row.PCId+"' onclick=\"$('#ClaimDate-"+row.PCId+"').datepicker({dateFormat: 'yy-mm-dd'});$('#ClaimDate-"+row.PCId+"').datepicker('show');\"  ClaimDate='Amount' value='" + $('<div/>').text(data).html() + "' style='text-align:right;'>";
                     }
                  },
                  {
                     'targets': 2,
                     'searchable': true,
                     'orderable': true,
                     'className': 'dt-body-center',
                     'render': function (data, type, row){
                         return $('#DefaultCurrency').val()+" <input type='text' style='text-align:right;' id='ClaimAmount-"+row.PCId+"' style='width:auto;' name='ClaimAmount' onchange=\"calculate('"+row.PCId+"', this.value);\" value='" + $('<div/>').text(data).html() + "'/>";
                     }
                  },
                  
                  {
                     'targets': 3,
                     'searchable': true,
                     'orderable': true,
                     'className': 'dt-body-center',
                     'render': function (data, type, row){
                         return '<input type="text" id="Ref-'+row.PCId+'" style="width:auto;" name="Ref" value="' + $('<div/>').text(data).html() + '"/>';
                     }
                  },
                  {
                     'targets': 4,
                     'className': 'dt-body-center'
                     
                  },
                  {
                     'targets': 5,
                     'className': 'dt-body-center'
                    
                  },
                  {
                     'targets': 6,
                     'searchable': true,
                     'orderable': true,
                     'className': 'dt-body-center',
                     'render': function (data, type, row){
                         return '<input type="text" id="Retention-'+row.PCId+'" style="width:auto;" name="Retention" value="' + $('<div/>').text(data).html() + '"/>';
                     }
                  },
                  {
                     'targets': 7,
                     'searchable': true,
                     'orderable': true,
                     'className': 'dt-body-center',
                     'render': function (data, type, row){
                         return '<input type="text" id="Remarks-'+row.PCId+'" style="width:auto;" name="Remarks" value="' + $('<div/>').text(data).html() + '"/>';
                     }
                  },
                  {
                     'targets': 8,
                     'searchable': true,
                     'orderable': true,
                     'visible': true,
                     'className': 'dt-body-center',
                     'render': function (data, type, row){
                         return "<button id='SubmitPCe' label='Submit' onclick=\"EditPC('"+data+"');\"  value='" + data + "'>Submit</button>&nbsp&nbsp"+
                         "<button id='DeletePCe' label='Submit' onclick=\"DeletePC('"+data+"');\"  value='" + data + "'>Delete</button>&emsp;"+
                         "<input type='button' id='Convert' label='Submit' onclick=\"ConvertToDO('"+$("#ProjectNo").val()+"', '"+data+"');\"  value='Create DO' />&emsp;";
                     }
                  }
                  ]
            } );
            date(); 
               var i= 0;
            
            if(type=="add"){
                table.row.add( {
                    "ClaimOrder":       " ",
                    "ClaimDate": " ",
                    "CurrencyCode":     " ",
                    "ClaimAmount": " ",
                    "DONo": " - ",
                    "DODate": " - ",
                    "Ref":     " ",
                    "Retention": " ",
                    "Remarks":     " ",
                    "PCId": i
                    
                } ).draw();
                i++;
            }
            
     }
    
    function validatePC(a){
        var response="true";
        
        var up = $("#ClaimAmount-"+a).val();
        var rem = $("#Remarks-"+a).val();
        
        if(!(up.match(/[0-9 -()+]+$/))){
            alert('Invalid Claim Amount (Numbers/Decimals Only).');
            response = 'false';
        }
        if(rem!='' && !(rem.match(/^.{0,100}$/))){
            alert('Invalid Remarks. (0-100 characters only)');
            response = 'false';
        }
        return response;
    
    }
    //Edit PC Record
    function EditPC(a){
        var result = validatePC(a);
        if(a=="0"){
            if(result=='true'){
                var PCInfo = {};
                PCInfo.ProjectNo = $("#ProjectNo").val();
                PCInfo.ClaimOrder = $("#ClaimOrder-"+a).val();
                PCInfo.ClaimAmount = $("#ClaimAmount-"+a).val();
                PCInfo.Ref = $("#Ref-"+a).val();
                PCInfo.Retention = $("#Retention-"+a).val();
                PCInfo.Remarks = $("#Remarks-"+a).val();
                PCInfo.ClaimDate = $("#ClaimDate-"+a).val();
                PCInfo.CurrencyCode = $("#DefaultCurrency").val();
                
                $.ajax({
                    type: "POST",
                    url: "Project_AddEdit.aspx/InsertPCInfo",
                    data: '{PCInfo: ' + JSON.stringify(PCInfo) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Project Info has been updated successfully.");
        //                 var a = $('#tblAdditionalMR').DataTable();
        //                //a.Destroy();
                        //$('#tblProgressiveClaim').dataTable().empty();
                        ProgressiveClaim();
                       
                    }
                }); 
            }
        }
        else{
            if(result=='true'){
                var PCInfo = {};
                PCInfo.ProjectNo = $("#ProjectNo").val();
                PCInfo.PCId = a;
                PCInfo.ClaimOrder = $("#ClaimOrder-"+a).val();
                PCInfo.ClaimAmount = $("#ClaimAmount-"+a).val();
                PCInfo.Ref = $("#Ref-"+a).val();
                PCInfo.Retention = $("#Retention-"+a).val();
                PCInfo.Remarks = $("#Remarks-"+a).val();
                //PCInfo.ClaimDate = $("#ClaimDate-"+a).val();
                if($("#ClaimDate-"+a).val()==''){
                    PCInfo.ClaimDate = '1900-01-01';
                }
                else{
                    PCInfo.ClaimDate = $("#ClaimDate-"+a).val();
                }
               
                PCInfo.CurrencyCode = $("#DefaultCurrency").val();
                
                $.ajax({
                    type: "POST",
                    url: "Project_AddEdit.aspx/UpdatePCInfo",
                    data: '{PCInfo: ' + JSON.stringify(PCInfo) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Project Info has been updated successfully.");
        //                 var a = $('#tblAdditionalMR').DataTable();
        //                //a.Destroy();
                       
                        ProgressiveClaim();
                       
                    }
                }); 
            }
        }
    }
    

    
    //Delete PC Record
    function DeletePC(a){
        $( "#dialog-confirm4" ).dialog({
            resizable: false,
            height:140,
            modal: true,
            buttons: {
                "Delete all items": function() {
                    var PCInfo = {};
                    PCInfo.ProjectNo = $("#ProjectNo").val();
                    PCInfo.PCId = a;
               
                    $.ajax({
                        type: "POST",
                        url: "Project_AddEdit.aspx/DeletePCInfo",
                        data: '{PCInfo: ' + JSON.stringify(PCInfo) + '}',
                        contentType: "application/json; charset=utf-8",   <div id="dialog-confirm" title="Delete record?" >
  <p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>These items will be permanently deleted and cannot be recovered. Are you sure?</p>
  </div>
                        dataType: "json",
                        success: function (response) {
                            alert("Record have been deleted successfully.");
                            //$('#tblProgressiveClaim').dataTable().empty();
                            ProgressiveClaim();
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
    
    
    function InsertPC(){
        ProgressiveClaim("add");
    }
    function ImportMRItems(a){
    var ProjectNo = a;
        if($("#OldProjectNo").val()!= null || $("#OldProjectNo").val()!= '' || $("#OldProjectNo").val()!= ' ')
        {
            ProjectNo = $("#OldProjectNo").val();
        }
        $( "#confirm-import" ).dialog({
            resizable: false,
            height:140,
            modal: true,
            buttons: {
                "Import MR Items": function() {
                    $.ajax({
                        type: "POST",
                        url: "Project_AddEdit.aspx/ImportMRItems",
                        data: "{ 'ProjectNo': '" + ProjectNo + "' }",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                       
                            alert(response);
                            //$('#tblImportedMRs').dataTable().empty();
                            MRRecord();
                            DisplayDetails($("#ProjectNo").val());
                        }
                    });
                     $( this ).dialog( "close" ); 
                },
                Cancel: function() {
                    alert("Import have been cancelled.");
                    $( this ).dialog( "close" );
                }
            }
        });
        
    }
    
    function MRRecord(){
        var a = $('#tblImportedMRs').DataTable( {
            "ajax": {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "Project_AddEdit.aspx/GetMR",
                "data": function(d){
                    return "{ 'ProjectNo': '" + $("#ProjectNo").val() + "' }";
                },
                "dataSrc": function (json) {
                    return json;
                    
                 }
            },
            "bDestroy": true,
            "jQueryUI": true,
            "columns": [
                { "data": "mrNo" },
                { "data": "MRDate" },
                { "data": "vendorName" },
                { "data": "requestor" },
                { "data": "purchaser" },
                { "data": "purchaseCurrency" },
                { "data": "purchasePrice" },
                { "data": "remarks" },
                { "data": "status" }
            ],
            'columnDefs': [{
                 'targets': 0,
                 'className': 'dt-body-center'
              },
              {
                 'targets': 1,
                 'className': 'dt-body-center'
              },
              {
                 'targets': 2,
                 'className': 'dt-body-center'
              },
              {
                 'targets': 3,
                 'className': 'dt-body-center'
              },
              {
                 'targets': 4,
                 'className': 'dt-body-center'
              },
              {
                 'targets': 5,
                 'className': 'dt-body-center'
              },
              {
                 'targets': 6,
                 'className': 'dt-body-center'
              },
              {
                 'targets': 7,
                 'className': 'dt-body-center'
              },
              {
                 'targets': 8,
                 'className': 'dt-body-center'
              }],
            "order":[0, "desc"]
        });
                       
     }
    
     function extraInfo(a){
       
        var SN = $("#SerialNo-"+a).val();
        if (SN!=null || SN!=''){
            
            $.ajax({
                    url: "Project_AddEdit.aspx/DisplaySNExtra",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: "{ 'SerialNo': '"+ SN +"' }",

                    success: function (data) {
                        $.map(data, function(item) {
                            $('#Manufacturer').html(item.Manufacturer);
                            $('#ModelNo').html(item.ModelNo);
                            $("#CESize").html(item.CESize);
                            $("#MfgDate").html(item.MfgDate);
                            $("#GasTypeName").html(item.GasTypeName);
                            $("#ContainerStatus").html(item.ContainerStatus);
                            OpenMsg();
                            action = data.Action;
                        })
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert('Failed to retrieve detail.');
                    }
                });
        }
     }
     
     function OpenMsg(){
        $( "#dialog-message" ).dialog({
                              modal: true,
                              buttons: {
                                Ok: function() {
                                  $( this ).dialog( "close" );
                                }
                              }
                            });
     
     }
     
     function CreateInvoice(a, b){
      var PCInfo = {};
                PCInfo.ProjectNo = a;
                PCInfo.PCId = b;
        $.ajax({
            type: "POST",
            url: "Project_AddEdit.aspx/ConvertCEToInvoice",
             data: '{PCInfo: ' + JSON.stringify(PCInfo) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if(response == '1'){
                    alert("Project No."+ $("#ProjectNo").val()+" already have a Delivery Order No."+ $("#DONo").val());
                }else{
                    alert("Invoice Number "+response+" have been created from Project No."+ $("#ProjectNo").val());
                }
            }
        });
}



  function ConvertToDO(a,b){
                var PCInfo = {};
                PCInfo.ProjectNo = a;
                PCInfo.PCId = b;
        $.ajax({
            type: "POST",
            url: "Project_AddEdit.aspx/ConvertPrjToDO",
            data: '{PCInfo: ' + JSON.stringify(PCInfo) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                    alert (response);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                            var err = eval("(" + xhr.responseText + ")");
                            var text = err.Message;
                            var arr = text.split(',');
                            var arr1 = arr[1].split('.');
                            alert(arr1[5]);
                        }
        });
       
     }
     
     function validateProject(){
        var PN =  $('#AccountCode').val().length;
        var AC =  $('#txtCustomerPO').val().length;
        var VPInfo = {};
        VPInfo.AccountCode=$('#AccountCode').val();
        VPInfo.ProjectNo=$('#ProjectNo').val();
        VPInfo.CustomerPO =  $("[id*=txtCustomerPO]").val();
        var res;
        if(PN > 0 && AC > 0){
            var c = [];
            i = 0;
            $.ajax({
                type: "POST",
                async: false,
                url: "Project_AddEdit.aspx/ValidateProject",
                data:'{VPInfo: ' + JSON.stringify(VPInfo) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                     $.each(data, function(index) {
                        c[i++] = data[index].ProjectNo;
                    });
                    res = c.join();
                    if(res.length >0){
                        alert("Duplicate Account Code and Customer PO for Project No. "+ res);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                            var err = eval("(" + xhr.responseText + ")");
                            var text = err.Message;
                            alert(text);
                       }
            });
        }
        return res;
     }
     
     function LoadAttachments(prjno){
            var i = 1;
//            $.ajax({
//                type: "POST",
//                async: false,
//                url: "Project_AddEdit.aspx/LoadAttachment",
//                data:"{ 'ProjectNo': '" + $("#ProjectNo").val() + "' }",
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                success: function (data) {
//                     $.each(data, function(index) {
//                        $('#tblAttachmentItems').append('<tr><td>'+data[index].CreatedDate+'</td><td><asp:LinkButton id="LinkButton'+i+'" Text="'+data[index].FileName+'" CommandName="Download" CommandArgument="'+data[index].FileName+'" OnCommand="LinkButton_Command" runat="server">'+data[index].FileName+'</asp:LinkButton></td><td>'+data[index].Extension+'</td></tr>');
//                    });
//                },
//                error: function (xhr, ajaxOptions, thrownError) {
//                            var err = eval("(" + xhr.responseText + ")");
//                            var text = err.Message;
//                            alert(text);
//                       }
//            });
       
//        }
            
            var table = $('#tblAttachmentItems').DataTable( {
                "ajax": {
                    "dataType": "json",
                    "contentType": "application/json; charset=utf-8",
                    "type": "POST",
                    "url": "Project_AddEdit.aspx/LoadAttachment",
                    "data": function(d){
                        return "{ 'ProjectNo': '" + $("#ProjectNo").val() + "' }";
                    },
                    "dataSrc": function (json) {
                        return json;
                     }
                },
                "bDestroy": true,
                "jQueryUI": true,
                "responsive":true,
                "columns": [
                    { "data": "FileID", title: "ID"},
                    { "data": "CreatedDate", title: "Uploaded Date"},
                    { "data": "FileName", title: "File Name" },
                    { "data": "Extension", title: "Extension"},
                    { "data": "CreatedBy", title: "Uploaded By"}
                ],
                'columnDefs': [{
                     'targets': 0,
                     'searchable': true,
                     'orderable': true,
                     'className': 'dt-body-center',
                     'visible': false,
                     'render': function (data, type, row){
                         return data;
                     }
                  },
                {
                     'targets': 2,
                     'searchable': true,
                     'orderable': true,
                     'className': 'dt-body-left',
                     'render': function (data, type, row, meta){
                         return "<a href='#' id='lnkButton' onclick=\"DownloadAttachment('"+row.FileID+"');\" >"+row.FileName+"</a>";
                     }
                  }],
                  "order": [0, "DESC"]
            } );

     i++;
     }
     
     function DownloadAttachment(id){
        $("[id*=fileid]").val(id);
        $("[id*=DownloadAF]").trigger('click');
     }