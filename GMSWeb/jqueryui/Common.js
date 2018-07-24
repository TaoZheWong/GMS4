/*
 *----------------------------------------------------------------------
 *   DESCRIPTION 	:   Add button and Edit Button attribute name must be "Edit"  
 *                      - new row id must append "New-" 
 *   AUTHOR      	:   KimYoong
 *   DATE        	:   14/06/2016    
 *   MODIFICATION LOG	:    
 *----------------------------------------------------------------------
 */

 
function jsOpenOperationalReport(url)
{
    if (url.indexOf('GMS3') < 0)
        url = sDOMAIN + "/" + url;
	jsWinOpen2( url, 795, 580, 'yes');
}  

function jsWinOpen2(x,w,h, haveScroll){
	var winLeft = (screen.width - w) / 2;
	var winUp = (screen.height - h) / 2;
	if (! window.focus)
		return true;
		
	haveScroll = 'yes';
		
	window.open(x,"","width=" + w + ",height=" + h + ",top="+ winUp+",left="+ winLeft +",resizable=yes,status=yes,menubar=no,scrollbars=" + haveScroll);
}

                    
function ResetForm(modal)
{
    var str = $( "form" ).find("#"+modal+" :input").serializeArray();
    var fields  = new Object();
    jQuery.each( str, function( i, field ) { 
        
        $('#' + field.name).val("");
    });  

}
 
function PopulateForm(item, tablename, modal, itemkey, itemno, functionname)
{
    var MRNo = getMRNo(); 
    var CoyID = getCoyID();
    var urlink = get_hostname(window.location.href);
    ResetValidation();     
    
    if(itemno != '') 
    {
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/" + functionname,  
            data: "{'CompanyId' : "+CoyID + ",'MRNo': '" + MRNo + "'}",
            success: function(json) {               
                localStorage.setItem( 'DataTables_'+ tablename +'_'+itemno, JSON.stringify(json));
                SetFormValue(item, tablename, modal, itemkey, itemno);  
                                    
            }
        });
    }
    else 
        SetFormValue(item, tablename, modal, itemkey, itemno);
} 

function SetFormValue(item, tablename, modal, itemkey, itemno)
{
    var search_obj = {};
    search_obj[itemkey] = item.value;

    var tbl = JSON.parse(localStorage.getItem('DataTables_'+ tablename + '_' + itemno)); 
    var data = _.findWhere(tbl, search_obj);  

    var str = $( "form" ).find("#"+modal+" :input").serializeArray();   
        
    var fields  = new Object();
    jQuery.each( str, function( i, field ) {            
        $('#' + field.name).val(data[field.name]);
    }); 
    
    
    if(modal == "ModalProduct") 
    {
        if($('#ProdCode').val() != "00000000000")
            $('#ProdName').prop("readonly", true);
        else
            $('#ProdName').prop("readonly", false);    
    } 
    else if(modal == "ModalConfirmSales") 
    {
        if($('#CustomerAccountCode').val() != "D00000")
            $('#CustomerAccountName').attr("readonly", true);
        else
            $('#CustomerAccountName').attr("readonly", false);
    }   

}

function ResetValidation()
{
    $('.form-group').removeClass( "has-error has-danger" );
    $('.form-group').removeClass( "has-success" ); 
    $('.form-group').find('span').removeClass('glyphicon-ok'); 
    $('.form-group').find('span').removeClass('glyphicon-remove');
}

function CRUD(item, tablename, modal, itemkey, itemno)
{ 
    
    var search_obj = {};
    search_obj[itemkey] = item.value;    
      
    var obj = {};  
    var tbl = JSON.parse(localStorage.getItem('DataTables_'+ tablename + '_' + itemno));                                   
    var i = _.indexOf(tbl, _.findWhere(tbl, search_obj));
    var tbl1 = $('#'+tablename).DataTable();
    if (item.name == "Delete") {
        if(item.value.substring(0,4) != "New-")
        {  
            var tbl_remove = JSON.parse(localStorage.getItem('DataTables_'+ tablename + '_' + itemno +'_Delete'));
            var remove_obj = {}; 
            remove_obj[itemkey] = item.value;           
            if(tbl_remove == null)
                 tbl_remove = [remove_obj];  
            else             
                tbl_remove.push(remove_obj);                  
            localStorage.setItem( 'DataTables_'+tablename + '_' + itemno +'_Delete', JSON.stringify(tbl_remove));
        }        
        
        if(itemno == '') 
        {
            tbl.splice(i, 1);         
            tbl1.row( item ).remove().draw(false);
        }
    }   
    else if (item.name == "Edit")
    {         
        $('#'+ modal).find('input, textarea, select').each(function() {                       
            obj[this.name]= this.value;             
        }); 
        
        obj[itemkey] = item.value;  
                        
        if(i == -1) //insert
        {
            obj["RowStatus"]= "Insert"; 
            if(tbl == null)
                tbl = [obj];
            else
                tbl.push(obj);
        }
        else // delete and insert for update
        {            
            var modify = _.findWhere(tbl, search_obj); 
            // update row status to modify with status original and modify            
            if (modify["RowStatus"] == "Insert")
                obj["RowStatus"] = "Insert"; 
            else 
                obj["RowStatus"] = "Modify";
            
            tbl.splice(i, 1, obj);  
        }
    }                
    localStorage.setItem( 'DataTables_'+tablename+ '_' + itemno, JSON.stringify(tbl)); 
}




function CRUD2 (item, tablename, itemkey, itemno)
{
    
    var search_obj = {};
    search_obj[itemkey] = item.value;
    
    var obj = {};  
    var tbl = JSON.parse(localStorage.getItem('DataTables_'+ tablename + '_' + itemno));                                   
    var i = _.indexOf(tbl, _.findWhere(tbl, search_obj));
    var tbl1 = $('#'+tablename).DataTable();
    if (item.name == "Delete") {
        if(item.value.substring(0,4) != "New-")
        {  
            var tbl_remove = JSON.parse(localStorage.getItem('DataTables_'+ tablename + '_' + itemno +'_Delete'));
            var remove_obj = {}; 
            remove_obj[itemkey] = item.value;           
            if(tbl_remove == null)
                 tbl_remove = [remove_obj];  
            else             
                tbl_remove.push(remove_obj);                  
            localStorage.setItem( 'DataTables_'+tablename + '_' + itemno +'_Delete', JSON.stringify(tbl_remove));
        }         
        tbl.splice(i, 1);         
        tbl1.row( item ).remove().draw(false);
        
    }   
    else if (item.name == "Edit")
    {   
        obj[itemkey] = item.value; 
                
        $("#"+item.value+" td").find("input,select").each(function() {
            obj[this.name]= this.value;  
        });  
        
        $("#"+item.value+" td").find("label").each(function() {
            obj[$(this).attr("name")]= $(this).html();  
        }); 
        
        $(item).parents('tr').find("input,select").each(function() {
            obj[this.name]= this.value;
        }); 
        
        $(item).parents('tr').find("label").each(function() {
            obj[$(this).attr("name")]= $(this).html();
        }); 
                
        if(i == -1) //insert
        {            
            obj["RowStatus"]= "Insert"; 
            tbl.push(obj);
        }
        else // delete and insert for update
        {
            var modify = _.findWhere(tbl, search_obj)
            // update row status to modify with status original and modify
            if (modify["RowStatus"] == "Insert")
                obj["RowStatus"] = "Insert"; 
            else 
                obj["RowStatus"] = "Modify";
            tbl.splice(i, 1, obj);  
        }
        
        
    }                
    localStorage.setItem( 'DataTables_'+tablename+ '_' + itemno, JSON.stringify(tbl)); 
        
    $("#success-alert").attr("class","alert alert-success");
    
    $("#success-alert").fadeTo(1000, 100).slideUp(100, function(){
    
    });
    
}

function CheckAccount(item, callback)
{ 
    var urlink = get_hostname(window.location.href);
    var UserId = getUserID();
 
    $.ajax({
                async       : true,
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/CheckAccountCode",
                data: "{'CompanyId' : "+CoyID + ",UserId: "+ UserId+", 'AccountCode': '" + item + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function(data) {
                    if(JSON.stringify(data) == "[]")
                        callback(false);
                    else
                        callback(true);                      
                         
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
}

 
function get_hostname(url) {
    var m = url.match(/^(http|https):\/\/[^/]+/);
    return m ? m[0] : null;
}

function GetCompany(callback)
{  
    
    var urlink = get_hostname(window.location.href);
    
    $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetCompany",
                data: "{'CompanyId' : "+CoyID + "}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function(data) {
                     callback(data);   
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
    
}

function CheckProduct(item, callback)
{ 
    var urlink = get_hostname(window.location.href);
    var CoyID = getCoyID();  
    $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/CheckProductCode",
                data: "{'CompanyId' : "+CoyID + ",'ProdCode': '" + item + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function(data) {
                    if(JSON.stringify(data) == "[]")
                        callback(false);
                    else
                        callback(true);            
                         
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
}

function CheckPrice(unitsellingPrice, sellingcurrency, unitpurchasePrice, purchasecurrency, callback)
{ 
    var urlink = get_hostname(window.location.href);
    var CoyID = getCoyID();  
    $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetConvertedSellingAndPurchasePrice",
                data: "{'CompanyId' : "+CoyID + ",'SellingPrice': '" + unitsellingPrice + "','SellingCurrency': '" + sellingcurrency + "','PurchasePrice': '" + unitpurchasePrice + "','PurchaseCurrency': '" + purchasecurrency + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function(data) {
                   
                    if(parseFloat(data[0].SellingPrice) < parseFloat(data[0].PurchasePrice))
                        callback(false);
                    else
                        callback(true);  
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
}

function GetAccount(item, tablename, accounttype, isAccountCode)
{ 
var CoyID = getCoyID(); 
var UserId = getUserID();
var urlink = get_hostname(window.location.href);

$(item).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetAccountList",
                data: "{'CompanyId' : "+CoyID + ", 'UserId':"+UserId+", 'account': '" + item.value + "', 'exact': false, 'accounttype':'"+ accounttype +"'}",
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
                            
                            
                                    if(accounttype == 'C')
                                    {
                                        return {                                                                
                               
                                        label: item.AccountCode + ' - ' + item.AccountName, 
                                        value: item.AccountCode,
                                        text : item.AccountName,
                                        ContactPerson: item.ContactPerson,
                                        OfficePhone:item.OfficePhone,
                                        Fax:item.Fax,
                                        Email:item.Email  
                                        } 
                                    } 
                                    else if (!isAccountCode && accounttype == 'S')
                                    {
                                        return {
                                        label: item.AccountCode + ' - ' + item.AccountName, 
                                        value: item.AccountName,
                                        text : item.AccountName,
                                        ContactPerson: item.ContactPerson,
                                        OfficePhone:item.OfficePhone,
                                        Fax:item.Fax,
                                        Email: item.Email                                      
                                        } 
                                    }
                                    else if (isAccountCode && accounttype == 'S') {
                                        return {
                                            label: item.AccountCode + ' - ' + item.AccountName,
                                            value: item.AccountCode,
                                            text: item.AccountName,
                                            ContactPerson: item.ContactPerson,
                                            OfficePhone: item.OfficePhone,
                                            Fax: item.Fax,
                                            Email: item.Email
                                        }
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
            if(tablename == "tblSales")
            {
                $('#CustomerAccountName').val(i.item.text);
                if(i.item.value != "D00000")
                    $('#CustomerAccountName').attr("readonly", true);
                else
                    $('#CustomerAccountName').attr("readonly", false);
            }
            else 
            {
                if (isAccountCode) {
                    $('#VendorName').val(i.item.text);
                    $('#VendorCode').val(i.item.AccountCode);
                }
                $('#VendorContact').val(i.item.ContactPerson);
                $('#VendorTel').val(i.item.OfficePhone);
                $('#VendorFax').val(i.item.Fax);
                $('#VendorEmail').val(i.item.Email);
                $('#VendorName').attr(i.item.text);
            }
                         
        },
        minLength: 3    // MINIMUM 1 CHARACTER TO START WITH.
    });
    
}



function GetProduct(item, tablename)
{

var MRScheme = getMRScheme();
var urlink = get_hostname(window.location.href);
$(item).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetProductList",
                data: "{'CompanyId' : "+CoyID + ", 'product': '" + item.value + "', 'approver1': '" + $('#approver1-id').val() + "', 'approver2': '" + $('#approver2-id').val() + "', 'approver3': '" + $('#approver3-id').val() + "', 'approver4': '" + $('#approver4-id').val() + "', 'exact': false }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function(data) {
                   
                    if(JSON.stringify(data) == "[]")
                    {    
                        $('#ProdCode').val("");                      
                        $('#ProdName').val("");
                        $('#UOM').val("");
                        $('#ProductGroupCode').val("");
                        $('#Approver1').val("");
                        $('#Approver2').val("");
                        $('#Approver3').val("");
                        $('#Approver4').val("");
                        $('#Approver1ID').val("");
                        $('#Approver2ID').val("");
                        $('#Approver3ID').val("");
                        $('#Approver4ID').val("");
                        alert('No matches found');                        
                    }
                    else{
                        response(
                            $.map(data, function(item) {
                                return { 
                                    label: item.ProductCode + ' - ' + item.ProductName, 
                                    value: item.ProductCode,
                                    text : item.ProductName,
                                    textUOM : item.UOM,
                                    textProductGroupCode : item.ProductGroupCode,
                                    textApprover1 : item.pmrealname,
                                    textApprover2 : item.phrealname,
                                    textApprover3 : item.ph3realname,
                                    textApprover4 : item.ph5realname,
                                    textApprover1ID : item.pmuserid,
                                    textApprover2ID : item.phuserid,
                                    textApprover3ID : item.ph3userid,
                                    textApprover4ID : item.ph5userid
                                    
                                }
                            })
                        );
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                  
                    alert(textStatus);
                }
            });
        },
        
        select: function (e, i) {   
            var KeyToSplit = "";
            $('#ProdCode').val(i.item.value);
            $('#ProdName').val(i.item.text);
            $('#UOM').val(i.item.textUOM);
            $('#ProductGroupCode').val(i.item.textProductGroupCode);
            
            if(MRScheme == "Product")
            {                
                KeyToSplit = i.item.textProductGroupCode + i.item.textApprover1ID + i.item.textApprover2ID + i.item.textApprover3ID + i.item.textApprover4ID;
                $('#Approver1').val(i.item.textApprover1);
                $('#Approver2').val(i.item.textApprover2);
                $('#Approver3').val(i.item.textApprover3);
                $('#Approver4').val(i.item.textApprover4);
                $('#Approver1ID').val(i.item.textApprover1ID);
                $('#Approver2ID').val(i.item.textApprover2ID);
                $('#Approver3ID').val(i.item.textApprover3ID);
                $('#Approver4ID').val(i.item.textApprover4ID);
            }
            
            $('#KeyToSplit').val(KeyToSplit);
            
            if(i.item.value != "00000000000")
            {
                $('#ProdName').prop("readonly", true);
            }
            else
            {
                $('#ProdName').prop("readonly", false);
            }       
                        
            if((i.item.textApprover1ID == "" || i.item.textApprover1ID == "0") &&
               (i.item.textApprover2ID == "" || i.item.textApprover2ID == "0") &&
               (i.item.textApprover3ID == "" || i.item.textApprover3ID == "0") &&
               (i.item.textApprover4ID == "" || i.item.textApprover4ID == "0") && MRScheme == "Product")
            {                
                $('#Approver1').attr("readonly", false); 
                $('#Approver2').attr("readonly", false);   
                $('#Approver3').attr("readonly", false);   
                $('#Approver4').attr("readonly", false);     
            }
            else
            {                               
                $('#Approver1').attr("readonly", true); 
                $('#Approver2').attr("readonly", true);   
                $('#Approver3').attr("readonly", true);   
                $('#Approver4').attr("readonly", true);                
            }          
        },
        minLength: 3    // MINIMUM 1 CHARACTER TO START WITH.
    });    
}


function GetUOM(item, tablename)
{
var CoyID = getCoyID(); 
var urlink = get_hostname(window.location.href);
$(item).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetUOMList",
                data: "{'CompanyId' : "+CoyID + ", 'uom': '" + item.value + "' }",
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
                                    value: item.UOM                                 
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
           
            $(item).parents('tr').find("input[id*=UOM]").val(i.item.value); 
            var dt = $('#'+tablename).DataTable();
            var tr = $(item).closest('tr');
            var rowIndex = dt.row( tr ).index();
            /* Can be used to find column number*/
            //* var colIndex = $(a).parent().parent().children().index($(a).parent());
            $( 'li[data-dt-row='+rowIndex+']' ).find("input[id*=UOM]").attr('value', i.item.value);
                         
        },
        minLength: 1    // MINIMUM 1 CHARACTER TO START WITH.
    });
    
}



function GetProductGroupCode(item, tablename)
{
var CoyID = getCoyID(); 
var urlink = get_hostname(window.location.href);
$(item).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetProductGroupList",
                data: "{'CompanyId' : "+CoyID + ", 'productgroupcode': '" + item.value + "' }",
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
                                    label: item.ProductGroupCode + ' - ' + item.ProductGroupName,                                
                                    value: item.ProductGroupCode                                 
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
           
            $(item).parents('tr').find("input[id*=ProductGroupCode]").val(i.item.value); 
            var dt = $('#'+tablename).DataTable();
            var tr = $(item).closest('tr');
            var rowIndex = dt.row( tr ).index();
            /* Can be used to find column number*/
            //* var colIndex = $(a).parent().parent().children().index($(a).parent());
            $( 'li[data-dt-row='+rowIndex+']' ).find("input[id*=ProductGroupCode]").attr('value', i.item.value);
        },
        minLength: 1    // MINIMUM 1 CHARACTER TO START WITH.
    });
    
}



function PopulateApproverTeam(item, productgroupcode,  tablename)
{
var CoyID = getCoyID(); 
var urlink = get_hostname(window.location.href);
$(item).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetProductTeamByProductGroup",
                data: "{ 'CompanyId' : "+CoyID + ", 'productgroupcode': '" + productgroupcode + "' }",
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
                                    textProductGroupCode : item.ProductGroupCode,
                                    textApprover1 : item.pmrealname,
                                    textApprover2 : item.phrealname,
                                    textApprover3 : item.ph3realname,
                                    textApprover4 : item.ph5realname                               
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
        
           
           
            $(item).parents('tr').find("input[id*=Approver1]").val(i.item.textApprover1); 
            $(item).parents('tr').find("input[id*=Approver2]").val(i.item.textApprover2); 
            $(item).parents('tr').find("input[id*=Approver3]").val(i.item.textApprover3); 
            $(item).parents('tr').find("input[id*=Approver4]").val(i.item.textApprover4); 
           
                                 
            var dt = $('#'+tablename).DataTable();
            var tr = $(item).closest('tr');
            var rowIndex = dt.row( tr ).index();
            
            
            /* Can be used to find column number*/
            //* var colIndex = $(a).parent().parent().children().index($(a).parent());  
           
            $( 'li[data-dt-row='+rowIndex+']' ).find("input[id*=Approver1]").attr('value', i.item.textApprover1);
            $( 'li[data-dt-row='+rowIndex+']' ).find("input[id*=Approver2]").attr('value', i.item.textApprover2);
            $( 'li[data-dt-row='+rowIndex+']' ).find("input[id*=Approver3]").attr('value', i.item.textApprover3);
            $( 'li[data-dt-row='+rowIndex+']' ).find("input[id*=Approver4]").attr('value', i.item.textApprover4);
                         
        },
        minLength: 3    // MINIMUM 1 CHARACTER TO START WITH.
    });
    
}


function GetCurrency(a){
var urlink = get_hostname(window.location.href);
   
    $(a).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetCurrencyList",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",              
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
                    alert(textStatus);
                }
            });
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
    }).val("SGD").click(function() {
        $(this).autocomplete('search', $(this).val());
    });
}


function GetRequestor(a){

    var CoyID = getCoyID();
    var UserId = getUserID();
    var urlink = get_hostname(window.location.href);
   
    $(a).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetRequestorList",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",    
                data: "{CompanyId : "+CoyID+",UserId: "+ UserId+", 'MRScheme': '" + getMRScheme() + "'}",          
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
                                    label: item.RequestorRealName,
                                    value: item.RequestorRealName,
                                    text: item.Requestor
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
            $('#requestor-id').val(i.item.text);
            if(getMRScheme() != "Product")
            {
                // Get Approver 1
                $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/CheckApproverUser",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",  
                data: "{'CompanyId' : " + CoyID + ",'UserId': " + i.item.text + ", 'Seq': 1, 'MRScheme': '" + getMRScheme() + "' }",
                success: function (data) {
                    $.map(data, function(item) {  
                                      
                     $('#approver1-id').val(item.UserNumId);                 
                     $('#approver1-name').val(item.UserRealName);                     
                     action = data.Action;        
                    })               
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
                });
                
                // Get Approver 2
                $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/CheckApproverUser",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",  
                data: "{'CompanyId' : " + CoyID + ",'UserId': " + i.item.text + ", 'Seq': 2, 'MRScheme': '" + getMRScheme() + "' }",
                success: function (data) {
                    $.map(data, function(item) {  
                                     
                     $('#approver2-id').val(item.UserNumId);                 
                     $('#approver2-name').val(item.UserRealName);                     
                     action = data.Action;        
                    })               
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
                });
                
                // Get Approver 3
                $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/CheckApproverUser",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",  
                data: "{'CompanyId' : " + CoyID + ",'UserId': " + i.item.text + ", 'Seq': 3, 'MRScheme': '" + getMRScheme() + "' }",
                success: function (data) {
                    $.map(data, function(item) {  
                                   
                     $('#approver3-id').val(item.UserNumId);                 
                     $('#approver3-name').val(item.UserRealName);                     
                     action = data.Action;        
                    })               
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
                });
                
                // Get Approver 4
                $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/CheckApproverUser",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",  
                data: "{'CompanyId' : " + CoyID + ",'UserId': " + i.item.text + ", 'Seq': 4, 'MRScheme': '" + getMRScheme() + "' }",
                success: function (data) {
                    $.map(data, function(item) {  
                                  
                     $('#approver4-id').val(item.UserNumId);                 
                     $('#approver4-name').val(item.UserRealName);                     
                     action = data.Action;        
                    })               
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
                });
                
                
            
            }
             
            
            
            
            
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
        
    }).click(function() {
        $(this).autocomplete('search', $(a).val()); 
    });
}



function GetApprover1(a, fill, Approver2ID, Approver2, Approver3ID, Approver3, Approver4ID, Approver4){

    var CoyID = getCoyID();
    var UserId = getUserID();
    var urlink = get_hostname(window.location.href);
    var MRScheme = getMRScheme();  
   
    $(a).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetApprover1List",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",    
                data: "{'CompanyId' : " + CoyID + ",'UserId': " + UserId + ", 'UserRealName': '" + a.value + "', 'MRScheme': '" + MRScheme + "'}",
                success: function (data) {
                   
                    if (!data) {
                        alert('a');
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
                                    
                                    label: item.Approver1RealName,
                                    value: item.Approver1RealName,
                                    text: item.Approver1UserID
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
            
            $('#'+fill).val(i.item.text); 
            
            if (MRScheme == "Product")
            {
                // Set Approver 2
                $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetNextApprover",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",  
                data: "{'CompanyId' : " + CoyID + ",'UserId': " + i.item.text + ", 'Seq': 0, 'MRScheme': '" + getMRScheme() + "' }",
                success: function (data) {
                    
                    $.map(data, function (item) {
                       
                     $('#' + Approver2ID).val(item.UserNumId);
                     $('#' + Approver2).val(item.UserRealName);
                     $('#' + Approver3ID).val("");
                     $('#' + Approver3).val("");
                     SetApprover3(item.UserNumId, Approver3ID, Approver3);  
                    
                     action = data.Action;        
                    })               
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
                });
               
                
                
                 
            } 
            
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
        
    }).click(function () {
        
        $(this).autocomplete('search', $(a).val()); 
    });
}

function SetApprover3(Approver2ID, Approver3ID, Approver3)
{
    var CoyID = getCoyID();
    var UserId = getUserID();
    var urlink = get_hostname(window.location.href);
    
    if(getMRScheme() == "Product")
            {
                // Set Approver 3
                $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetNextApprover",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",  
                data: "{'CompanyId' : " + CoyID + ",'UserId': " + Approver2ID + ", 'Seq': 1, 'MRScheme': '" + getMRScheme() + "' }",
                success: function (data) {
                    $.map(data, function (item) {
                        if (Approver2ID != item.UserNumId)
                        {
                            $('#' + Approver3ID).val(item.UserNumId);
                            $('#' + Approver3).val(item.UserRealName);
                            action = data.Action;
                        }
                             
                    }) ;
                               
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
                });
            }   
}



function GetApprover2(a, fill, fillNextID, fillNextText){
   
    var CoyID = getCoyID();
    var UserId = getUserID();
    var urlink = get_hostname(window.location.href);
   
    $(a).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetApprover2List",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",    
                data: "{'CompanyId' : " + CoyID + ",'UserId': " + UserId + ", 'UserRealName': '" + a.value + "', 'MRScheme': '" + getMRScheme() + "' }",
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
                                    label: item.Approver2RealName,
                                    value: item.Approver2RealName,
                                    text: item.Approver2UserID
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
            $('#'+fill).val(i.item.text);       
            if(getMRScheme() == "Product")
            {
                // Set Approver 3
                $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetNextApprover",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",  
                data: "{'CompanyId' : " + CoyID + ",'UserId': " + i.item.text + ", 'Seq': 1, 'MRScheme': '" + getMRScheme() + "' }",
                success: function (data) {
                    $.map(data, function(item) {  
                     $('#'+fillNextID).val(item.UserNumId);
                     $('#'+fillNextText).val(item.UserRealName);   
                     action = data.Action;        
                    }) ;
                               
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
                });
            }   
            
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
        
    }).click(function() {
        $(this).autocomplete('search', $(a).val()); 
    });
}


function GetApprover3(a, fill, fillNextID, fillNextText){

    var CoyID = getCoyID();
    var UserId = getUserID();
    var urlink = get_hostname(window.location.href);
   
    $(a).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetApprover3List",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",    
                data: "{'CompanyId' : " + CoyID + ",'UserId': " + UserId + ", 'UserRealName': '" + a.value + "', 'MRScheme': '" + getMRScheme() + "' }",
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
                                    label: item.Approver3RealName,
                                    value: item.Approver3RealName,
                                    text: item.Approver3UserID
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
             $('#'+fill).val(i.item.text); 
             // Set Approver 4
                $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetNextApprover",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",  
                data: "{'CompanyId' : " + CoyID + ",'UserId': " + i.item.text + ", 'Seq': 2, 'MRScheme': '" + getMRScheme() + "' }",
                success: function (data) {
                    $.map(data, function(item) {  
                     $('#'+fillNextID).val(item.UserNumId);
                     $('#'+fillNextText).val(item.UserRealName);   
                     action = data.Action;        
                    })               
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
                });
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
        
    }).click(function() {
        $(this).autocomplete('search', $(a).val()); 
    });
}



function GetApprover4(a, fill){

    var CoyID = getCoyID();
    var UserId = getUserID();
    var urlink = get_hostname(window.location.href);
   
    $(a).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetApprover4List",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",    
                data: "{'CompanyId' : " + CoyID + ",'UserId': " + UserId + ", 'UserRealName': '" + a.value + "', 'MRScheme': '" + getMRScheme() + "' }",
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
                                    label: item.Approver4RealName,
                                    value: item.Approver4RealName,
                                    text: item.Approver4UserID
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
            $('#'+fill).val(i.item.text); 
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
        
    }).click(function() {
        $(this).autocomplete('search', $(a).val()); 
    });
}

function GetPurchaser(a){

    var CoyID = getCoyID();   
    var urlink = get_hostname(window.location.href);
    $(a).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetPurchaserList",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",    
                data: "{CompanyId : "+CoyID + "}",          
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
                                    label: item.PurchaserName,
                                    value: item.PurchaserName,
                                    text: item.PurchaserID
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
            $('#purchaser-id').val(i.item.text);
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
        
    }).click(function() {
        $(this).autocomplete('search', $(a).val()); 
    });
}

function GetWarehouse(a) {

    var CoyID = getCoyID();
    var urlink = get_hostname(window.location.href);
    $(a).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetWarehouseList",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: "{CompanyId : " + CoyID + "}",
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
                                    label: item.Warehouse
                                }
                            })
                        )
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        select: function (e, i) {
            $('#warehouse').val(i.item.label);
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.

    }).click(function () {
        $(this).autocomplete('search', $(a).val());
    });
}



function GetDocumentCategory(a){

    var arrLinks = [
    {
        key: 1,
        value: "PO",
        label: 'PO'},
    {
        key: 2,
        value: "Service Job Sheet",        
        label: 'Service Job Sheet'},
    {
        key: 3,
        value: "End User Form",
        label: 'End User Form'},
    {
        key: 4,
        value: "Others",
        label: 'Others'}
    ]
   
    $(a).autocomplete({
        source: arrLinks,
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
    }).click(function() {
        
        $(this).autocomplete('search', "");
    });
}


function GetProductInfo1(a)
{
    var item = $("#ProdCode").val();   
    var AccountCode = $("#hidCustomerCode").val();
    
    var urlink = get_hostname(window.location.href);
     $.ajax({
                async       : true,
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetDynamicContent",
                data: "{'CompanyId' : "+CoyID + ",'ProdCode': '" + item + "', 'UserId':"+getUserID()+",'AccountCode': '" + AccountCode + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function(data) {
                    $(a).attr('data-content', data);
                    $('[data-toggle="popover"]').popover();                                        
                         
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
}


function GetProductInfo(item, a, UserId, AccountCode)
{
    
    var urlink = get_hostname(window.location.href);
     $.ajax({
                async       : true,
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetDynamicContent",
                data: "{'CompanyId' : "+CoyID + ",'ProdCode': '" + item + "', 'UserId':"+UserId+",'AccountCode': '" + AccountCode + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function(data) {
                    $(a).attr('data-content', data);
                    $('[data-toggle="popover"]').popover();                                        
                         
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
}

function GetGRNInfo(item, a, mrno, UserId)
{
     var urlink = get_hostname(window.location.href);
     $.ajax({
                async       : true,
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetGRNInfo",
                data: "{'CompanyId' : "+CoyID + ",'PONo': '" + item + "','MRNo': '" + mrno + "', 'UserId':"+UserId+"}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function(data) {                    
                    $(a).attr('data-content', data);
                    $('[data-toggle="popover"]').popover();                                        
                         
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
}


function GetCalendar(a){
        $(a).datetimepicker({format: 'DD/MM/YYYY'});

}


function GetMRSpecialStatus(a){
    
    var arrLinks = [];
    if(IsMainPurchaser() == "1")
    { 
        arrLinks = [
        {
            key: 1,
            value: "In Progress",
            label: "In Progress",
            text: "P"},
        {
            key: 2,
            value: "Closed",        
            label: "Closed",
            text: "C"},
        {
            key: 3,
            value: "Approved",
            label: "Approved",
            text: "A"}
        ]       
        
    }
    else {
        arrLinks = [
        {
            key: 1,
             value: "Closed",        
            label: "Closed",
            text: "C"}
        ] 
        
    }
   
    $(a).autocomplete({
        source: arrLinks,
        select: function (e, i) {
            $('#status-id').val(i.item.text); 
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
    }).click(function() {
        
        $(this).autocomplete('search', "");
    });

}

function GetMRStatus(a){

    var CoyID = getCoyID();
    var UserId = getUserID();
    var urlink = get_hostname(window.location.href);
    
    
    $(a).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetMRStatusList",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",    
                data: "{'CompanyId' : "+CoyID+",'Status': '"+ a.value +"' }",          
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
                                    label: item.StatusName,
                                    value: item.StatusName,
                                    text: item.StatusID
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
            $('#status-id').val(i.item.text); 
        },       
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
        
    }).click(function() {
        $(this).autocomplete('search', $(a).val()); 
    });
}


function GetTaxType(a){

    var CoyID = getCoyID();
    var UserId = getUserID();
    var urlink = get_hostname(window.location.href);
   
    $(a).autocomplete({
        source: function(request, response) {
            $.ajax({
                url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetTaxType",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",    
                data: "{CompanyId : "+CoyID+"}",          
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
                                    label: item.TaxTypeID + ' - ' + item.TaxName, 
                                    value: item.TaxName,
                                    text: item.TaxTypeID,
                                    rate: item.TaxRate
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
            $('#TaxTypeID').val(i.item.text); 
            $('#TaxRate').val(i.item.rate); 
            ReCalculate(); 
            
        },
        minLength: 0    // MINIMUM 1 CHARACTER TO START WITH.
        
    }).click(function() {
        $(this).autocomplete('search', $(a).val()); 
    });
}

function PopulateProductItem(item)
{    
     var CoyID = getCoyID();       
     var urlink = get_hostname(window.location.href);
     var MRScheme = getMRScheme();
     var KeyToSplit = "";
   
        $.ajax({
            async       : true,
            type        : "POST",
            url: urlink + "/GMS3/Products/Products/AddEditMaterialRequisition.aspx/GetProductList",
            data: "{'CompanyId' : "+CoyID + ", 'product': '" + item.value + "', 'approver1': '" + $('#approver1-id').val() + "', 'approver2': '" + $('#approver2-id').val() + "', 'approver3': '" + $('#approver3-id').val() + "', 'approver4': '" + $('#approver4-id').val() + "','exact': true }",
            contentType : "application/json; charset=utf-8",
            dataType    : "json",
            success: function (data) {

                if (JSON.stringify(data) == "[]") {
                    $('#ProdCode').val("");
                    $('#ProdName').val("");
                    $('#UOM').val("");
                    $('#ProductGroupCode').val("");
                    $('#Approver1').val("");
                    $('#Approver2').val("");
                    $('#Approver3').val("");
                    $('#Approver4').val("");
                    $('#Approver1ID').val("");
                    $('#Approver2ID').val("");
                    $('#Approver3ID').val("");
                    $('#Approver4ID').val("");
                    alert('No matches found');
                }
                else {
                   
                    $.map(data, function (item) {
                        $('#ProdCode').val(item.ProductCode);
                        $('#ProdName').val(item.ProductName);
                        $('#UOM').val(item.UOM);
                        $('#ProductGroupCode').val(item.ProductGroupCode);
                        if (MRScheme == "Product") {
                            KeyToSplit = item.ProductGroupCode + item.pmuserid + item.phuserid + item.ph3userid + item.ph5userid;

                            $('#Approver1').val(item.pmrealname);
                            $('#Approver2').val(item.phrealname);
                            $('#Approver3').val(item.ph3realname);
                            $('#Approver4').val(item.ph5realname);
                            $('#Approver1ID').val(item.pmuserid);
                            $('#Approver2ID').val(item.phuserid);
                            $('#Approver3ID').val(item.ph3userid);
                            $('#Approver4ID').val(item.ph5userid);
                        }
                        $('#KeyToSplit').val(KeyToSplit);

                        if (item.ProductCode != "00000000000") {
                            $('#ProdName').prop("readonly", true);
                        }
                        else {
                            $('#ProdName').prop("readonly", false);
                        }

                        if ((item.pmuserid == "" || item.pmuserid == "0") &&
                           (item.phuserid == "" || item.phuserid == "0") &&
                           (item.ph3userid == "" || item.ph3userid == "0") &&
                           (item.ph5realname == "" || item.ph5realname == "0") && MRScheme == "Product") {
                            $('#Approver1').attr("readonly", false);
                            $('#Approver2').attr("readonly", false);
                            $('#Approver3').attr("readonly", false);
                            $('#Approver4').attr("readonly", false);
                        }
                        else {
                            $('#Approver1').attr("readonly", true);
                            $('#Approver2').attr("readonly", true);
                            $('#Approver3').attr("readonly", true);
                            $('#Approver4').attr("readonly", true);
                        }


                    })

                }
                           
                          },
            error       : function(xhr, textstatus, error){
                            alert(textstatus);
                          }
        }); 
}


function getCurrentLink(){
    var hidCurrentLink = $("input[id*=hidCurrentLink]").val();
    return hidCurrentLink;
}

function getMainPurchaserUserID(){
    var hidMainPurchaserUserID = $("input[id*=hidMainPurchaserUserID]").val();
    return hidMainPurchaserUserID;
}
        
function getUserID(){
    var hidUserID = $("input[id*=hidUserID]").val();
    return hidUserID;
}


function getCoyID() {
    var CoyID = getUrlVars()["CoyID"];
    if(CoyID == undefined)
    {
        CoyID = $("input[id*=hidCoyID]").val();
    }
    return CoyID;
}

function getViewPurchaseInfo()
{
    var hidViewPurchaseInfo = $("input[id*=hidViewPurchaseInfo]").val();
    return hidViewPurchaseInfo;
}

function getMRScheme()
{
    var hidMRScheme = $("input[id*=hidMRScheme]").val();
    return hidMRScheme;           
}


function getPageName()
{
    var hiddenPageName = $("input[id*=hiddenPageName]").val();
    return hiddenPageName;
}

function getMRRole()
{
    var hidMRRole = $("input[id*=hidMRRole]").val();
    return hidMRRole;
}

function getUserRole() { 
        var UserRole = $("input[id*=hidUserRole]").val();            
        return UserRole;
}

function getMRNo() {
    var MRNo = getUrlVars()["MRNo"];
    
    if(MRNo == undefined)
        MRNo = '';
    
    return MRNo;
}

function getUrlVars() 
{
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&#]*)/gi, function(m,key,value) {
        vars[key] = value;
    });
    return vars;
} 

function getApproverUser()
{
    var hidApproverUserID = $("input[id*=hidApproverUserID]").val();            
    return hidApproverUserID;
}

function getCurrentMRStatus()
{
    return $('#status-id').val();
}


function IsMainPurchaser()
{
    return $('#hidIsMainPurchaser').val();
}

function getProjectNo() {
    var ProjectNo = getUrlVars()["ProjectNo"];

    if (ProjectNo == undefined)
        ProjectNo = '';

    return ProjectNo;
}