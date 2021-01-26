
$(document).ready(function() { 

    var MRNo = getMRNo();
     
      $("#exchange-rate").focusout(function () {

          var intRegex = /^\d+(\.\d{1,5})?$/;
          var original = $('#exchange-rate').val();

         
          if (original <= 0) {
              alert('The amount should be bigger than 0');
              return false;
          }
          if (!original.match(intRegex)) {
              alert('Please enter a valid number');
              $("#exchange-rate").val('1.00000');
              return false;
          }
          else {
              return true;
          }

      });
       
       $('#FormSearch input[type=text],textarea').focusout(function(event){
           $(this).val($(this).val().toUpperCase()); 
       });  
                   
       $('#FormSearch :input').keypress(function(event){
           var target = $( event.target ); 
           if($('#Save').is(':visible'))
           {
              if(event.keyCode == 13)
                if ( !target.is( "textarea" ) ) {
                    $('#Save').focus(); 
                }                          
           }
           else 
           {
              if(event.keyCode == 13)
                if ( !target.is( "textarea" ) ) {
                    $('#Print').click();  
                }
           }
        });
                   
        //var position = $(window).scrollTop();
        
        $('input#ProdCode').bind("keypress change input", _.debounce(function () {
             GetProduct(this, $(this).attr("tablename"));
        }, 600, {
            'leading': true,
            'trailing': false
        })).focusout(function () {
            PopulateProductItem(this)
            //$("#ProdName").val(,); 
        });

        $('input#RecipeNo').focusout(function () {
            ImportRecipe(this);
        });

       
        $("#success-alert").hide();
        $('a[data-toggle="tab"]').on( 'shown.bs.tab', function (e) {
                $.fn.dataTable.tables( {visible: true, api: true} ).columns.adjust();
        });
        
        $( "#Save" ).on( "click dblclick", function() {
           
           SaveMRHeader('tblMRHeader_'+$('#mr-no').val()); 
           SaveALL();   
           //$( "#Save" ).scrollTop(position);   
        });  
        
        $( "#Duplicate" ).on( "click dblclick", function() {
            Duplicate(); 
        });

        $("#CreatePO").on("click dblclick", function () {
            var MRNo = getMRNo();            
            CreatePO();            
        });
        
        $( "#SubmitForApproval" ).on( "click dblclick", function() {
            SubmitForApproval(); 
        }); 
        
        $( "#Approve" ).on( "click dblclick", function() {
            var MRNo = getMRNo();             
            
            if ($("#ConfirmVendor" ).val() == "0")
            {            
                CheckAccess(function(data){           
                        var ApproverUserID = data[0].ApproverUserID;
                        var IsCurrentLevel = data[0].IsCurrentLevel;
                        var InvalidOrderQty = data[0].InvalidOrderQty;
                        var InvalidPurchasePrice = data[0].InvalidPurchasePrice;
                                               
                        if(InvalidOrderQty == "1")
                        {
                             SetModalMessage("Please ensure the Order Qty is greater than zero.", "");     
                        }
                        else if (InvalidPurchasePrice == "1")
                        {
                             SetModalMessage("Please enter the reason for purchase price lower than selling price.", "");
                        }
                        else {                        
                            if(ApproverUserID != "0" && IsCurrentLevel == "Y")  
                            {              
                                Approve();
                            }
                            else 
                            {
                                $('#cancel-reject').val("approve"); 
                                $('#ModalReason').modal({backdrop: 'static', keyboard: false});
                            }   
                        }              
               }); 
           }
           else 
           {
                SetModalMessage("Please confirm Vendor Info.", "");  
           }
        });
        
        $( "#ConfirmVendor" ).on( "click dblclick", function() {
            ConfirmVendor(); 
        });
        
        $( ".tab-v2" ).find("li").on( "click", function() {            
            if(getViewPurchaseInfo() == "1" && this.id == "productTab")           
                $("#DivTotal").css("display", '');
            else
                $("#DivTotal").css("display", 'none');
        });
        
        
        $( "#Cancel" ).on( "click dblclick", function() {
            $('#cancel-reject').val("cancel"); 
            $('#ModalReason').modal({backdrop: 'static', keyboard: false});
        });
        
        $( "#Reject" ).on( "click dblclick", function() {
            
            $('#cancel-reject').val("reject"); 
            $('#ModalReason').modal({backdrop: 'static', keyboard: false}); 
        });
        
        $( "#btnClose" ).on( "click", function() {
            $('#ModalMessage').modal('hide');    
                 
            
            if(this.value != '')
            {               
                window.location.href = this.value;
            }
            
            
        });
        
        $( "#btnConfirmDelete" ).on( "click dblclick", function() {  
               
                CRUD($('#info-7').val(), $('#info-1').val(),  $('#info-2').val(), $('#info-3').val(), $('#info-4').val());    
                if($('#info-4').val() != '')
                {
                    DeleteRecord($('#info-1').val(), $('#info-5').val(), $('#info-6').val());
                }  
                    
                $('#ModalConfirmDelete').modal('hide'); 
                ResetForm("ModalConfirmDelete");
                $("#btnClose").css("display", '');
        });
        
        $( "#cancel-reject" ).on( "click dblclick", function() { 
            
            if($('#Reason').val().length > 0)
            {       
                if(this.value == "cancel")
                    Cancel();
                else if(this.value == "reject")
                    Reject();
                else if(this.value == "approve")
                    Approve();                
                $('#ModalReason').modal('hide');
            }
            else 
            {
                $('#DivReason').addClass( "has-error has-danger" );
                $('#DivReason').removeClass( "has-success" ); 
                $('#DivReason').find('span').removeClass('glyphicon-ok'); 
                $('#DivReason').find('span').addClass('glyphicon-remove'); 
                
            }
            
        });
        
        $( "#PrintMR" ).on( "click dblclick", function() {  
            if(getViewPurchaseInfo() == "1")                
                jsOpenOperationalReport('/Finance/BankFacilities/PdfReportViewer.aspx?REPORT=MaterialRequisitionWithLetterHead_'+getCoyID()+'&TRNNO='+$('#mr-no').val() + '&REPORTID=-4');
            else
                jsOpenOperationalReport('/Finance/BankFacilities/PdfReportViewer.aspx?REPORT=MaterialRequisitionWithLetterHeadEmptyVendor_'+getCoyID()+'&TRNNO='+$('#mr-no').val() + '&REPORTID=-4');
        });     
        
        $( "#PrintPO" ).on( "click dblclick", function() {            
            jsOpenOperationalReport('/Finance/BankFacilities/PdfReportViewer.aspx?REPORT=PurchaseOrder_'+getCoyID()+'&TRNNO='+$('#mr-no').val() + '&REPORTID=-4');
        });
                       
        //When checkboxes/radios checked/unchecked, toggle background color
        $('.form-group').on('click','input[type=radio]',function() {
            $(this).closest('.form-group').find('.radio-inline, .radio').removeClass('checked');
            $(this).closest('.radio-inline, .radio').addClass('checked');
        });
        $('.form-group').on('click','input[type=checkbox]',function() {
            $(this).closest('.checkbox-inline, .checkbox').toggleClass('checked');
        });

        //Show additional info text box when relevant checkbox checked
        $('.additional-info-wrap input[type=checkbox]').click(function() {
            if($(this).is(':checked')) {
                $(this).closest('.additional-info-wrap').find('.additional-info').removeClass('hide').find('input,select').removeAttr('disabled');
            }
            else {
                $(this).closest('.additional-info-wrap').find('.additional-info').addClass('hide').find('input,select').val('').attr('disabled','disabled');
            }
        });

        //Show additional info text box when relevant radio checked
        $('input[type=radio]').click(function() {
            $(this).closest('.form-group').find('.additional-info-wrap .additional-info').addClass('hide').find('input,select').val('').attr('disabled','disabled');
            if($(this).closest('.additional-info-wrap').length > 0) {
                $(this).closest('.additional-info-wrap').find('.additional-info').removeClass('hide').find('input,select').removeAttr('disabled');
            }        
        });
        
               
        // tblSales
        $( "#save-confirmedSales" ).on( "click dblclick", function() {  
            if ($('#hidAllowChanges').val() != "") {
                return;
            }
            var item = this;    
            var errorMessage = "";
            CheckAccount($('#CustomerAccountCode').val(),function(data){
                   if(data == false)
                   {
                        $('#DivCustomerCode').addClass( "has-error has-danger" );
                        $('#DivCustomerCode').removeClass( "has-success" ); 
                        $('#DivCustomerCode').find('span').removeClass('glyphicon-ok'); 
                        $('#DivCustomerCode').find('span').addClass('glyphicon-remove');                       
                        errorMessage = "Invalid Customer Code.";
                   }
                   else
                   {
                        $('#DivCustomerCode').removeClass( "has-error has-danger" );
                        errorMessage = "";
                   }
                   
                    var str = $( "form" ).find("#ModalConfirmSales :input").serializeArray();
                    jQuery.each( str, function( i, field ) {  
                        if($('#' + field.name).val().length == 0)
                        {
                            $('#Div'+ field.name).addClass( "has-error has-danger" );
                            $('#Div'+ field.name).removeClass( "has-success" ); 
                            $('#Div'+ field.name).find('span').removeClass('glyphicon-ok'); 
                            $('#Div'+ field.name).find('span').addClass('glyphicon-remove');                  
                            errorMessage = "Invalid "+ field.name;
                        }
                    }); 
                   
                   if(errorMessage == "")
                   {            
                        CRUD(item, 'tblSales', 'ModalConfirmSales', 'FileID', $('#mr-no').val());                       
                        if(MRNo != '') 
                            SaveRecord('tblSales', 'SaveConfirmedSales', 'ConfirmedSalesInfo');          
                        ConfirmedSales();
                        $('#ModalConfirmSales').modal('hide')
                   }                  
           }); 
        });  
             
        // tblSales
        var objSales = {};        
        $('#tblSales').on( 'click dblclick', 'button', function (e) {
                var temp = this;
                if(this.name == "Delete") {  
                    $('#confirm').modal({ backdrop: 'static', keyboard: false })
                        .one('click', '#delete', function (e) {
                            CRUD(temp, 'tblSales', 'ModalConfirmSales', 'FileID', $('#mr-no').val());  
                            if(MRNo != '')
                                DeleteRecord('tblSales', 'DeleteConfirmedSales', 'ConfirmedSalesInfo');    
                            ConfirmedSales();  
                    });
                }
                else {                     
                    PopulateForm(this, 'tblSales', 'ModalConfirmSales', 'FileID', $('#mr-no').val(), 'GetConfirmedSales'); 
                    
                    $('#save-confirmedSales').val(this.value); 
                    $('#ModalConfirmSales').modal({backdrop: 'static', keyboard: false});
                }                      
                ConfirmedSales();  
          
        });  
        
        // tblVendor
        $("#save-vendor").on("click dblclick", function () {
            if ($('#hidAllowChanges').val() != "") {
                return;
            }
            CRUD(this, 'tblVendor', 'ModalVendor', 'VendorID', $('#mr-no').val());  
            if(MRNo != '') 
                SaveRecord('tblVendor', 'SaveVendor', 'VendorInfo');    
            Vendor();            
        });         
                
        $('#tblVendor').on( 'click dblclick', 'button', function () { 
            var temp = this;
            if(this.name == "Delete") { 
                $('#confirm').modal({ backdrop: 'static', keyboard: false })
                    .one('click', '#delete', function (e) {
                        CRUD(temp, 'tblVendor', 'ModalVendor', 'VendorID', $('#mr-no').val());   
                        if(MRNo != '')                 
                            DeleteRecord('tblVendor', 'DeleteVendor','VendorInfo');    
                        Vendor();  
                });           
            }
            else {                           
                PopulateForm(this, 'tblVendor', 'ModalVendor', 'VendorID', $('#mr-no').val(), 'GetVendor');                                                    
                $('#save-vendor').val(this.value); 
                $('#ModalVendor').modal({backdrop: 'static', keyboard: false});
                
            }       
            Vendor(); 
        });
        
        // tblDelivery       
        $("#save-delivery").on("click dblclick", function () {
            if ($('#hidAllowChanges').val() != "") {
                return;
            }
            CRUD(this, 'tblDelivery', 'ModalDelivery', 'DeliveryNo', $('#mr-no').val());  
            if(MRNo != '') 
                SaveRecord('tblDelivery', 'SaveDelivery','DeliveryInfo'); 
            Delivery();  
        }); 
        
        $('#tblDelivery').on( 'click dblclick', 'button', function () { 
            var temp = this;
            if(this.name == "Delete") { 
                 $('#confirm').modal({ backdrop: 'static', keyboard: false })
                    .one('click', '#delete', function (e) {
                       CRUD(temp, 'tblDelivery', 'ModalDelivery', 'DeliveryNo', $('#mr-no').val());  
                         if(MRNo != '')
                            DeleteRecord('tblDelivery', 'DeleteDelivery','DeliveryInfo');  
                         Delivery();  
                });
            }
            else {                           
                PopulateForm(this, 'tblDelivery', 'ModalDelivery', 'DeliveryNo', $('#mr-no').val(), 'GetDelivery');                                     
                $('#save-delivery').val(this.value); 
                $('#ModalDelivery').modal({backdrop: 'static', keyboard: false});
            }       
            Delivery(); 
        });
        
        //  tblProduct 
        $("#save-product").on("click dblclick", function () {
                if ($('#hidAllowChanges').val() != "") {
                    return;
                }
               var item = this;
               var errorMessage = "";
               var MRScheme = getMRScheme();
               if (MRScheme == "Product" && MRNo == "" && $('#ProdCode').val() == "00000000000")
               {
                    var  KeyToSplit1 = $('#ProductGroupCode').val() + (parseInt($('#Approver1ID').val()) || 0) + (parseInt($('#Approver2ID').val()) || 0) + (parseInt($('#Approver3ID').val()) || 0)+ (parseInt($('#Approver4ID').val()) || 0);  
                    $('#KeyToSplit').val(KeyToSplit1);
               } 
               
               
               CheckProduct($('#ProdCode').val(),function(data){
               
               if(data == false)
               {
                    $('#DivProdCode').addClass( "has-error has-danger" );
                    $('#DivProdCode').removeClass( "has-success" ); 
                    $('#DivProdCode').find('span').removeClass('glyphicon-ok'); 
                    $('#DivProdCode').find('span').addClass('glyphicon-remove');                                             
                    errorMessage = "Invalid Product Code.";
               }
               else
               {
                    $('#DivProdCode').removeClass( "has-error has-danger" );
                    errorMessage = "";
               }           
                                            
               if (MRScheme == "Product" && MRNo != "" && (
               (parseInt($('#approver1-id').val()) || 0) != (parseInt($('#Approver1ID').val()) || 0) ||
               (parseInt($('#approver2-id').val()) || 0) != (parseInt($('#Approver2ID').val()) || 0) ||
               (parseInt($('#approver3-id').val()) || 0) != (parseInt($('#Approver3ID').val()) || 0) ||
               (parseInt($('#approver4-id').val()) || 0) != (parseInt($('#Approver4ID').val()) || 0)
               ))
               {
                    errorMessage = "Selected Product does not belong to Product Team.";
               }               
               
               if(((parseFloat($('#OrderQty').val()) || 0) <= 0) && (getUserRole() == "Product Team" || getUserRole() == "Purchasing"))
               {
                     errorMessage = "Please ensure order qty > 0.";
                     $('#DivOrderQty').addClass( "has-error has-danger" );
                     $('#DivOrderQty').removeClass( "has-success" ); 
                     $('#DivOrderQty').find('span').removeClass('glyphicon-ok'); 
                     $('#DivOrderQty').find('span').addClass('glyphicon-remove'); 
               }
               else 
               {
                    $('#DivOrderQty').removeClass( "has-error has-danger" );
                    errorMessage = "";
               }
                              
               CheckPrice($('#UnitSellingPrice').val(), $('#SellingCurrency').val(), $('#UnitPurchasePrice').val(), $('#PurchaseCurrency').val(),function(data) {
                     
                    
                     if(data == false && $('#ProductReason').val() == "" && getIntendedUse().indexOf("Sales") >= 0)
                     {
                        $('#DivProductReason').addClass( "has-error has-danger" );
                        $('#DivProductReason').removeClass( "has-success" ); 
                        $('#DivProductReason').find('span').removeClass('glyphicon-ok'); 
                        $('#DivProductReason').find('span').addClass('glyphicon-remove');
                        $('#DivProductReason').find('.help-block').append("<p>Please Enter Reason for Selling Price Less Than Purchase Price!</p>");
                        
                        errorMessage = "Please Enter Reason.";
                        
                     }  
                     
                     if(errorMessage == "")
                     {                                           
                        CRUD(item, 'tblProduct', 'ModalProduct', 'DetailNo', $('#mr-no').val()); 
                        if(MRNo != '') 
                            SaveRecord('tblProduct', 'SaveProduct', 'ProductInfo');
                        Product();  
                        
                        $('#ModalProduct').modal('hide');
                    } 
              });
           });
        });
        
        
                 
        $('#tblProduct').on('click dblclick', 'button', function (e) {
            //if ($('#hidAllowChanges').val() == "") {
                var temp = this;
                if (this.name == "Delete") {
                    $('#confirm').modal({ backdrop: 'static', keyboard: false })
                        .one('click', '#delete', function (e) {
                            CRUD(temp, 'tblProduct', 'ModalProduct', 'DetailNo', $('#mr-no').val());
                            if ($('#mr-no').val() != '')
                                DeleteRecord('tblProduct', 'DeleteProduct', 'ProductInfo');
                            Product();
                        });

                }
                else {
                    $('#DivProdCode').removeClass("has-error has-danger");
                    $('#DivProdCode').find('span').removeClass('glyphicon-remove');

                    PopulateForm(this, 'tblProduct', 'ModalProduct', 'DetailNo', $('#mr-no').val(), 'GetProduct');
                    $('#save-product').val(this.value);
                    $('#ModalProduct').modal({ backdrop: 'static', keyboard: false });
                }
                Product();
           // }
        });
        
        // tblAttachement  
        
        $( "#save-attachment" ).on( "click dblclick", function() { 
            CRUD(this, 'tblAttachment', 'ModalAttachment', 'FileID', $('#mr-no').val());   
            if(MRNo != '') 
                SaveRecord('tblAttachment', 'SaveAttachment', 'AttachmentInfo'); 
            Attachment();  
        });
                 
        $('#tblAttachment').on( 'click dblclick', 'button', function () { 
            var temp = this;   
            if(this.name == "Delete") {  
                $('#confirm').modal({ backdrop: 'static', keyboard: false })
                    .one('click', '#delete', function (e) {
                        CRUD(temp, 'tblAttachment', 'ModalAttachment', 'FileID', $('#mr-no').val()); 
                         if(MRNo != '')
                            DeleteRecord('tblAttachment', 'DeleteAttachment', 'AttachmentInfo');
                         Attachment();
                });
            }
            else { 
                                      
                PopulateForm(this, 'tblAttachment', 'ModalAttachment', 'FileID', $('#mr-no').val(), 'GetAttachment');                                     
                $('#save-attachment').val(this.value); 
                $('#ModalAttachment').modal({backdrop: 'static', keyboard: false});
            }         
            Attachment(); 
        });
                
        /*        
        GetCompany(function(data){  
            $('#hidMRScheme').val(data[0].MRScheme);  
        });    
        */
        EnabledAccess();
        
        $('[data-toggle="popover"]').popover();
        
        
        // Display Child Row for Product
        $('#tblProduct tbody').on('click', 'td.details-control', function () {
            var tr = $(this).closest('tr');
            var row = table.row( tr );
     
            if ( row.child.isShown() ) {
                // This row is already open - close it
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                // Open this row
                row.child( format(row.data()) ).show();
                tr.addClass('shown');
            }
        });
                     
        }); 
        
        function DisplayButtons(ApproverUserID, MRRole, RequiredVendorConfirmation, ActedDate, IsPosted)
        { 
          
           if($('#status-id').val() == "D")    
           { 
                
                //if(getUserID() == $('#requestor-id').val() || getUserID() == $('#CreatedBy-id').val())
                //{
                   $("#SubmitForApproval").css("display", '');  //ok 
                   $('#requestor-name').removeAttr('disabled'); //ok
                   $("#Save").css("display", ''); //ok
                   $("#Cancel").css("display", ''); //ok  
                //}            
                $("#Print").css("display", '');  
                              
                $('#li-PO').remove();                
                
           } 
           else if ($('#status-id').val() == "F")
           {    
                $("#SubmitForApproval").css("display", 'none'); //ok
                $("#Duplicate").css("display", ''); // ok
                $("#Cancel").css("display", ''); //ok                     
                $("#Save").css("display", '');
                                            
                if(ApproverUserID != "0" && ActedDate == "")
                {
                    $("#Approve").css("display", ''); //ok
                    $("#Reject").css("display", '');  // ok
                    $("#Cancel").css("display", ''); //ok
                    if(RequiredVendorConfirmation == "1")
                    {
                        $("#ConfirmVendor").val("1").css("display", '');
                    }
                    else
                    {
                        $("#ConfirmVendor").val("0").css("display", 'none');  
                    }                      
                    
                }
                $("#Print").css("display", '');   
                if($('#hidIsMainPurchaser').val() == "1")
                    $('#purchaser').attr("disabled", false); // ok
                $('#li-PO').remove();             
           }
           else if ($('#status-id').val() == "R")
           {
                $("#Duplicate").css("display", ''); //ok
                $("#Cancel").css("display", ''); //ok  
                $("#Save").css("display", '');                             
                $("#SubmitForApproval").css("display", ''); //ok                
                $('#li-PO').remove();                
           }
           else if ($('#status-id').val() == "X")
           {
                $("#Duplicate").css("display", '');  // ok               
                $('#li-PO').remove();
           }
           else if ($('#status-id').val() == "A")
           {              
                $("#Duplicate").css("display", '');  // ok    
                $("#delivery").css("display", '');                              
                if(getUserRole() == "Purchasing")
                {
                    $("#Save").css("display", ''); //ok
                    $("#Cancel").css("display", ''); //ok  
                    $('#status').attr("disabled", false); //ok 
                    if(IsPosted == "1")
                        $("#CreatePO").css("display", '');
                }
                $("#Print").css("display", '');
                if($('#hidIsMainPurchaser').val() == "1")
                    $('#purchaser').attr("disabled", false); // ok
               
           }
           else if ($('#status-id').val() == "P")
           {
                $("#Duplicate").css("display", ''); //ok
                $("#delivery").css("display", '');                 
                if(getUserRole() == "Purchasing")
                    $("#Save").css("display", ''); 
                $("#Print").css("display", '');  
                if(getUserRole() == "Purchasing")
                {
                    $('#status').attr("disabled", false); //ok
                    if (IsPosted == "1")
                        $("#CreatePO").css("display", '');
                }
                if($('#hidIsMainPurchaser').val() == "1")
                    $('#purchaser').attr("disabled", false); // ok
           }
           else if ($('#status-id').val() == "C")
           {
                $("#Duplicate").css("display", ''); //ok
                $("#delivery").css("display", '');                
                $("#Print").css("display", '');  
                if(getUserRole() == "Purchasing")
                {
                     $('#status').removeAttr('disabled');
                     $("#Save").css("display", '');
                }                
           }  
           /*
           if($('#Save').is(':visible'))
           {
                $( "#Save" ).focus();
           }
           */
                 
           
            
        
        }
        
        function UpdateStatusAndButton()
        {
           CheckAccess(function(data){          
           var ApproverUserID = data[0].ApproverUserID;
           var MRStatusID = data[0].MRStatusID;
           var MRStatusName = data[0].MRStatusName;
           var MRRole = data[0].MRRole;
           var RequiredVendorConfirmation = data[0].RequiredVendorConfirmation;
           var ActedDate = data[0].ActedDate;
           var IsPosted = data[0].IsPosted;
           
           
           $('#status').val(MRStatusName);
           $('#status-id').val(MRStatusID);    
            DisplayButtons(ApproverUserID, MRRole, RequiredVendorConfirmation, ActedDate, IsPosted);
           });
        }
         
                
        function EnabledAccess()
        {
            var MRNo = getMRNo();
            var MRScheme = getMRScheme();
           
           LoadHeader();
           
           CheckAccess(function(data){
           var result = data[0].result;
           var AllowToChangeApprover = data[0].AllowToChangeApprover;
           var ApproverUserID = data[0].ApproverUserID;
           var AllowChanges = data[0].AllowChanges;
           var ViewPurchaseInfo = data[0].ViewPurchaseInfo;
           var IsMainPurchaser = data[0].IsMainPurchaser;
           var MRRole = data[0].MRRole;
           var RequiredVendorConfirmation = data[0].RequiredVendorConfirmation;
           var ActedDate = data[0].ActedDate;
           var IsPosted = data[0].IsPosted;
          
           $('#hidAllowChanges').val("");
           if(AllowChanges == "0")
                $('#hidAllowChanges').val("disabled");
           
           $('#hidViewPurchaseInfo').val("0");
           if(ViewPurchaseInfo == "1")
                $('#hidViewPurchaseInfo').val("1");
                
           $('#hidIsMainPurchaser').val("0");  
           if(IsMainPurchaser == "1")
                $('#hidIsMainPurchaser').val("1");                 
           $('#hidMRRole').val(MRRole); 
           
           $('#hidApproverUserID').val(ApproverUserID);
           
           
           if(result == "1")
                $("#vendor").css("display", '');
                      
           
           
           LoadTabData(); 
                      
           var tblSales = $('#tblSales').DataTable();
           var tblVendor = $('#tblVendor').DataTable();
           var tblProduct = $('#tblProduct').DataTable();  
           var tblDelivery = $('#tblDelivery').DataTable();
           var tblAttachment = $('#tblAttachment').DataTable();  
                     
           if(ViewPurchaseInfo == "1")
           {
                $("#PurchaseInfo").css("display", '');
                $("#PurchaseInfoTotal").css("display", '');   
                if($("#productTab").attr("aria-expanded") == "true")  
                    $("#DivTotal").css("display", '');
                else 
                    $("#DivTotal").css("display", 'none');
           } 
           if (getCoyID()=="120" && MRScheme == "Department") {
               $("#RecipeInfo").css("display", '');
           }
          

           if (($('#status-id').val() == "D" || $('#status-id').val() == "R" || $('#status-id').val() == "F") && MRScheme == "Product" && AllowToChangeApprover == '1')
           {                  
                $('#approver1-name').removeAttr('disabled'); //ok
                $('#approver2-name').removeAttr('disabled'); //ok
                $('#approver3-name').removeAttr('disabled'); //ok
                $('#approver4-name').removeAttr('disabled'); //ok
           }          
           
           if(AllowChanges == "1" || getUserRole() == "Purchasing")
           {
                tblSales.buttons().enable();
                $('#tblSales').find("button").attr('disabled',false);
                tblVendor.buttons().enable();
                $('#tblVendor').find("button").attr('disabled',false);
                tblProduct.buttons().enable();
                $('#tblProduct').find("button").attr('disabled',false);
                tblDelivery.buttons().enable();
                $('#tblDelivery').find("button").attr('disabled',false);
                $('#tblAttachment').find("button").attr('disabled',false);   
                
                $('#MRHeader').find('input, textarea, select').each(function() { 
                    if (this.name != "status")
                        $('#' + this.name).attr("disabled", false);            
                });
               
                $('#information').find('input, textarea, select').each(function() {   
                    $('#' + this.name).attr("disabled", false);            
                });   
                
                $('#vendor').find('input, textarea, select').each(function() {   
                    $('#' + this.name).attr("disabled", false);            
                }); 
                
                $('#DivTotal').find('input, textarea, select').each(function() {   
                    $('#' + this.name).attr("disabled", false);            
                });  
                
                $('#drag').css("display", '');
                //$('#dim1').attr("disabled", '');
           } 

           
           if(MRNo == undefined || MRNo == '')
           {                 
                $("#Save").css("display", '');
                
           }
           else
           {                
               DisplayButtons(ApproverUserID, MRRole, RequiredVendorConfirmation, ActedDate, IsPosted);
           }  
           
        });
            
        }
        
        function LoadHeader()
        {
            var MRNo = getUrlVars()["MRNo"];
            MRGasDimension();
            if(MRNo == undefined || MRNo == '')
            {                
                InitialiseMR();
            }
            else 
            { 
                GetMRHeader();
            }
            
        }       
        
        function LoadTabData()
        {
            
            var MRNo = getUrlVars()["MRNo"]; 
            if(MRNo == undefined || MRNo == '')
            {
                ConfirmedSales();
                Vendor();
                Product();
                Delivery();
                Attachment();
                RoutingInfo();
            }
            else 
            {
                
                ConfirmedSales();
                Vendor();
                Product();
                Delivery();
                Attachment();
                RoutingInfo();
                UpdateStatusAndButton();
            }
           
        } 
        
        function InitialiseMR()
        {
            var CoyID = getCoyID();
            var UserId = getUserID();    
            var UserRole = getUserRole();
            var hidWarehouse = $("input[id*=hidWarehouse]").val();
            
        
            $.ajax({
            url: "AddEditMaterialRequisition.aspx/GetRequestorAndApproverList",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: "{CompanyId : "+CoyID+",UserId: "+ UserId+" }",
            success: function (data) {
                data = data.hasOwnProperty('d') ? data.d : data;
                $.map(data, function(item) {
                 $('#requestor-name').val(item.RequestorRealName);
                 $('#requestor-id').val(item.RequestorUserNumID);  
                 $('#approver1-id').val(item.Approver1UserNumID);                 
                 $('#approver1-name').val(item.Approver1RealName);
                 $('#approver2-id').val(item.Approver2UserNumID);
                 $('#approver2-name').val(item.Approver2RealName);
                 $('#approver3-id').val(item.Approver3UserNumID);  
                 $('#approver3-name').val(item.Approver3RealName);
                 $('#approver4-id').val(item.Approver4UserNumID); 
                 $('#approver4-name').val(item.Approver4RealName);      
                 $('#status').val("Draft");
                 $('#status-id').val("D"); 
                 $('#mr-date').val($.datepicker.formatDate('dd/mm/yy', new Date()));
                 $('#warehouse').val(hidWarehouse);
                 
                 if(UserRole = "CS" || getMRScheme() == "Product")  
                    $('#requestor-name').removeAttr('disabled'); //ok
                 
                 action = data.Action;        
                })               
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Failed to retrieve detail.');
            }
            });  
            
            
            $.ajax({
            url: "AddEditMaterialRequisition.aspx/GetTaxType",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: "{CompanyId : "+CoyID+"}",
            success: function (data) {
                data = data.hasOwnProperty('d') ? data.d : data;
                $.map(data, function(item) {
                 $('#TaxTypeID').val(item.TaxTypeID);
                 $('#TaxRate').val(item.TaxRate);  
                 $('#TaxTypeName').val(item.TaxTypeID + ' - ' + item.TaxName);    
                 $('#Discount').val("0");
                 action = data.Action;        
                })               
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Failed to retrieve detail.');
            }
            });   
        }
        
        function Cancel()
        {   
                  
            var CoyID = getCoyID(); 
            $.ajax({
                type: "POST",
                url: "AddEditMaterialRequisition.aspx/Cancel",
                data: "{ 'CompanyId' : "+CoyID+", 'MRNo': '" +$('#mr-no').val() + "', 'Reason' :'" + $('#Reason').val().replace("'", "\\'") + "' ,'PageName':'" +  getPageName() + "' ,'UserID': " + getUserID() + ",'CurrentLink':'" +  getCurrentLink() + "','IsCurrentLevel':'" +  getApproverUser() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    response = response.hasOwnProperty('d') ? response.d : response;
                        SetModalMessage("MR approval request has been cancelled!", response);    
                        UpdateStatusAndButton();                                    
                },
                error: function (xhr,text,status) {
                   var r = jQuery.parseJSON(xhr.responseText);
                   alert("Message: " + r.Message);
                }
            }); 
        
        }
        
        function Reject()
        {
            var CoyID = getCoyID(); 
           
            $.ajax({
                type: "POST",
                url: "AddEditMaterialRequisition.aspx/Reject",
                data: "{ 'CompanyId' : "+CoyID+", 'MRNo': '" +$('#mr-no').val() + "', 'Reason' :'" + $('#Reason').val().replace("'", "\\'") + "' ,'PageName':'" +  getPageName() + "','UserID': " + getUserID() + ",'CurrentLink':'" +  getCurrentLink() + "','IsCurrentLevel':'" +  getApproverUser() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    response = response.hasOwnProperty('d') ? response.d : response;
                        SetModalMessage("MR approval request has been rejected!", response);
                        UpdateStatusAndButton();
                },
                error: function (xhr,text,status) {
                   var r = jQuery.parseJSON(xhr.responseText);
                   alert("Message: " + r.Message);
                }
                
            }); 
        
        }
        
        function ConfirmVendor()
        {
            var CoyID = getCoyID(); 
                $.ajax({
                    type: "POST",
                    url: "AddEditMaterialRequisition.aspx/ConfirmVendor",
                    data: "{ 'CompanyId' : "+CoyID+", 'MRNo': '" +$('#mr-no').val() + "', 'PageName':'" +  getPageName() + "','CurrentLink':'" +  getCurrentLink() + "','UserID': " + getUserID() + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        response = response.hasOwnProperty('d') ? response.d : response;
                            SetModalMessage("Vendor information has been confirmed!", response);
                            UpdateStatusAndButton();
                                            
                    },
                    error: function (xhr,text,status) {
                       var r = jQuery.parseJSON(xhr.responseText);
                       alert("Message: " + r.Message);
                    }
                    
                }); 
        
        }     
        
        function Approve()
        {
           
            if(Validation('Approve') == true)
            { 
                var CoyID = getCoyID(); 
                $.ajax({
                    type: "POST",
                    url: "AddEditMaterialRequisition.aspx/Approve",
                    data: "{ 'CompanyId' : " + CoyID + ", 'MRNo': '" + $('#mr-no').val() + "', 'Purchaser' :'" + $('#purchaser-id').val() + "', 'Reason' :'" + $('#Reason').val().replace("'", "\\'") + "' ,'PageName':'" + getPageName() + "','UserID': " + getUserID() + ",'CurrentLink':'" + getCurrentLink() + "','IsCurrentLevel':'" + getApproverUser() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        response = response.hasOwnProperty('d') ? response.d : response;
                            SetModalMessage("MR approval request has been approved!", response);
                            UpdateStatusAndButton();
                                            
                    },
                    error: function (xhr,text,status) {
                       var r = jQuery.parseJSON(xhr.responseText);
                       alert("Message: " + r.Message);
                    }
                    
                }); 
            }
        
        }
        
        function SubmitForApproval()
        { 
            
            if(Validation('SubmitForApproval') == true)
            { 
                
                var CoyID = getCoyID(); 
                $.ajax({
                    type: "POST",
                    url: "AddEditMaterialRequisition.aspx/SubmitForApproval",
                    data: "{ 'CompanyId' : "+CoyID+", 'MRNo': '" +$('#mr-no').val() + "','PageName':'" +  getPageName() + "','UserID': " + getUserID() + ",'MainPurchaserUserID': " + getMainPurchaserUserID() + ",'CurrentLink':'" +  getCurrentLink() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        response = response.hasOwnProperty('d') ? response.d : response;
                            SetModalMessage("MR has been submitted for approval!", response); 
                            UpdateStatusAndButton();                                           
                    },
                    error: function (xhr,text,status) {
                       var r = jQuery.parseJSON(xhr.responseText);
                       alert("Message: " + r.Message);
                    }
                }); 
            }
        }
        
        function Duplicate()
        {  
            var UserId = getUserID();
            $.ajax({
                type: "POST",
                url: "AddEditMaterialRequisition.aspx/DuplicateMR",
                data: "{ 'CompanyId' : "+CoyID+", 'oldMRNo': '" +$('#mr-no').val() + "','PageName':'" +  getPageName() + "','UserID': " + getUserID() + ",'CurrentLink':'" +  getCurrentLink() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    response = response.hasOwnProperty('d') ? response.d : response;
                        SetModalMessage("MR created successfully!", response);                                        
                },
                error: function (xhr,text,status) {
                   var r = jQuery.parseJSON(xhr.responseText);
                   alert("Message: " + r.Message);
                }                
            });         
        }

        function CreatePO() {
            
            $.ajax({
                type: "POST",
                url: "AddEditMaterialRequisition.aspx/CreatePO",
                data: "{ 'companyId' : " + CoyID + ", 'mrNo': '" + $('#mr-no').val() + "','PageName':'" + getPageName() + "','CurrentLink':'" + getCurrentLink() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    response = response.hasOwnProperty('d') ? response.d : response;
                    SetModalMessage("PO updated successfully!", response);
                },
                error: function (xhr, text, status) {
                    var r = jQuery.parseJSON(xhr.responseText);
                    alert("Message: " + r.Message);
                }
            });
        }
        
                
        function DeleteRecord(tablename, functionname, info)
        { 
            
            $.ajax({
                type: "POST",
                url: "AddEditMaterialRequisition.aspx/"+functionname,
                data: "{'MRNo':'" + $('#mr-no').val() + "', "+ info +": " + (localStorage.getItem('DataTables_'+ tablename +'_' + $('#mr-no').val() + '_Delete')) + ",'PageName':'" +  getPageName() + "','MRRole':'" +  getMRRole() + "', 'CompanyId': " + getCoyID() + ",'UserID': " + getUserID() + ",'CurrentLink':'" +  getCurrentLink() + "','CurrentMRStatus':'" +  getCurrentMRStatus() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    response = response.hasOwnProperty('d') ? response.d : response;
                    localStorage.removeItem('DataTables_'+ tablename +'_' + $('#mr-no').val() + '_Delete');
                                           
                    if(tablename == 'tblProduct')    
                    {
                        var tblProduct = $('#tblProduct').DataTable(); 
                        if( !tblProduct.data().count())
                        {
                           $('#Discount').val("0.0000");
                           $('#SubTotal').val("0.0000")
                           $('#TaxAmount').val("0.0000")
                           $('#GrandTotal').val("0.0000"); 
                        }
                    }    
                        
                    if(response != 'success')   
                        SetModalMessage("Record have been deleted successfully.", response);  
                    else 
                    {
                        SetModalMessage("Record have been deleted successfully.", ""); 
                        UpdateStatusAndButton(); 
                    }  
                }
            }); 
        }          
                
        function SaveRecord(tablename, functionname, info)
        {
            
            var datastr = "";
            if(tablename == 'tblProduct')
                datastr = "{'MRNo':'" + $('#mr-no').val() + "', 'uniqueProductGroup':'' , "+ info +": " + (localStorage.getItem('DataTables_'+ tablename +'_' + $('#mr-no').val())) + ",'IsNew': false, 'mr': {}, 'Source': '" + $('#source').val() + "','PageName':'" + getPageName() + "', 'CompanyId': " + getCoyID() + ",'MRRole':'" + getMRRole() + "','UserID': " + getUserID() + ",'CurrentLink':'" +  getCurrentLink() + "','CurrentMRStatus':'" +  getCurrentMRStatus() + "','MRScheme':'" +  getMRScheme() + "'}";
            else if(tablename == 'tblSales' || tablename == 'tblVendor')
                datastr = "{'MRNo':'" + $('#mr-no').val() + "', "+ info +": " + (localStorage.getItem('DataTables_'+ tablename +'_' + $('#mr-no').val())) + ", 'IsNew': false,'PageName':'" + getPageName() + "', 'CompanyId': " + getCoyID() + ",'MRRole':'" + getMRRole() + "','UserID': " + getUserID() + ",'CurrentLink':'" +  getCurrentLink() + "','CurrentMRStatus':'" +  getCurrentMRStatus() + "','MRScheme':'" +  getMRScheme() + "'}";
            else
                datastr = "{'MRNo':'" + $('#mr-no').val() + "', "+ info +": " + (localStorage.getItem('DataTables_'+ tablename +'_' + $('#mr-no').val())) + ",'PageName':'" + getPageName() + "', 'CompanyId': " + getCoyID() + ",'MRRole':'" + getMRRole() + "','UserID': " + getUserID() + ",'CurrentLink':'" +  getCurrentLink() + "','CurrentMRStatus':'" +  getCurrentMRStatus() + "','MRScheme':'" +  getMRScheme() + "'}";
            $.ajax({
                type: "POST",
                url: "AddEditMaterialRequisition.aspx/"+functionname,
                data: datastr,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    response = response.hasOwnProperty('d') ? response.d : response;
                    if($('#mr-no').val() != '') 
                    {                        
                        localStorage.removeItem('DataTables_'+ tablename +'_' + $('#mr-no').val()); 
                        RoutingInfo();
                        if (tablename == 'tblVendor')
                            setTaxInfo();
                    } 
                    if(response != 'success')   
                        SetModalMessage("Failed updated.", response);  
                    else 
                    {
                        SetModalMessage("Record have been saved successfully.", "");  
                        UpdateStatusAndButton();
                    }   
                }
            }); 
        }     
        
        function getIntendedUse()
        {
            var intendedusage = [];
            if($("#Stock").prop('checked') == true)
                intendedusage.push('Stock');
            if($("#Sales").prop('checked') == true)
                intendedusage.push('Sales');
            if($("#Repair-Maintenance").prop('checked') == true)
                intendedusage.push('Repair & Maintenance'); 
            if($("#Sample").prop('checked') == true)
                intendedusage.push('Sample');     
            if($("#Workshop").prop('checked') == true)
                intendedusage.push('Workshop');
            if($("#Project").prop('checked') == true)
                intendedusage.push('Project');    
            if($("#Raw-Material").prop('checked') == true)
                intendedusage.push('Raw Material');     
            if($("#Staff-Welfare").prop('checked') == true)
                intendedusage.push('Staff Welfare'); 
            if($("#Asset").prop('checked') == true)
                intendedusage.push('Asset');
            if ($("#Others").prop('checked') == true)
                intendedusage.push('Others');
                
            return intendedusage.toString();
        }
        
                
        function Validation(action)
        {
           
            var tbl = $('#tblSales').DataTable();
            var tblAttachment = $('#tblAttachment').DataTable();
            var tblProduct = $('#tblProduct').DataTable();     
            
            var message = "";

            var previous = "";

            for (i = 0; i < tblProduct.rows().data().length; i++) {
                var current = tblProduct.rows(i).data()[0]["PurchaseCurrency"];
                if (previous != "") {
                    if (previous != current) {
                        message = message + "<li>Please select one Purchase Currency only.</li>";
                        break;
                    }
                }
                previous = current;
            }
            
            if($('#requestor-id').val() == "" || $('#requestor-name').val() == "")  
                 message = message + "<li>Please select requestor name.</li>";    
            
            if($('#source').val() == "Local" && $('#freight-mode').val() != "")  
                message = message + "<li>There is no freigh mode for local purchase.</li>";      
                        
            if($('#source').val() != "Local" && $('#freight-mode').val() == "")
                message = message + "<li>Please select freigh mode for overseas purchase.</li>";
                        
            if(getIntendedUse() == "")
                message = message + "<li>Please select at least one Intended Use.</li>";
            
            if (getIntendedUse().indexOf("Sales") >= 0 && !tbl.data().count())
                message = message +  "<li>Please fill in Confirmed Sales Info.</li>";
            
            if (getIntendedUse().indexOf("Sales") >= 0 && !tblAttachment.data().count())
                message = message +  "<li>Please upload PO in the attachment.</li>";
                
            if(($("#Console").prop('checked') == true) && ($('#console-date').val() == "")) 
                message = message + "<li>Please select Console Date.</li>";
                
            if(($("#ismov").prop('checked') == true) && ($('#mov').val() == "")) 
                message = message + "<li>Please indicate MOV.</li>";            
              
            if (getMRScheme() == "Product" && !tblProduct.data().count())
            {
                message = message +  "<li>Please input at least one product to raise a MR.</li>";
            }
                        
            if(action == "Approve")
            {
                var tblProduct = $('#tblProduct').DataTable();
                if (!tblProduct.data().count())
                    message = message +  "<li>Please fill in Product Info.</li>";
                    
                var tblVendor = $('#tblVendor').DataTable();
                if (!tblVendor.data().count() && getMRScheme() == "Product" )
                    message = message +  "<li>Please fill in Vendor Info.</li>"; 
                    
                if($('#hidIsMainPurchaser').val() == "1" && $('#purchaser').val() == "")
                    message = message +  "<li>Please select Purchaser.</li>"; 
            
            }
            
            if(action == "SubmitForApproval")
            {
                var tblProduct = $('#tblProduct').DataTable();
                if (!tblProduct.data().count() )
                    message = message +  "<li>Please fill in Product Info.</li>";
            }
            
            
            GetMRHeaderByMRNo(function(data){                           
               if(JSON.stringify(data) != '[]')  
               {       
                   var StatusID  = data[0].StatusID;
                   var ModifiedDate = data[0].ModifiedDate;
                   
                    if($('#status-id').val() != StatusID || $('#modifieddate').val() != ModifiedDate)
                        message = message +  "<li>Data inconsistency found in the page. Please refresh the page.</li>";
               }
            }); 
            
            if(message == "")
                    return true;
            else 
            {                 
                SetModalMessage(message,'');
                return false;
            }            
        }
             
        function SaveALL()
        { 
            if(Validation('') == true)
            { 
                var ProductInfo = (localStorage.getItem('DataTables_tblProduct_'+$('#mr-no').val()));
                if(ProductInfo == null)
                    ProductInfo = '{}';
                var ConfirmedSalesInfo = (localStorage.getItem('DataTables_tblSales_'+$('#mr-no').val()));
                if(ConfirmedSalesInfo == null)
                    ConfirmedSalesInfo = '{}';
                var VendorInfo = (localStorage.getItem('DataTables_tblVendor_'+$('#mr-no').val()));
                if(VendorInfo == null)
                    VendorInfo = '{}';
                var DeliveryInfo = (localStorage.getItem('DataTables_tblDelivery_'+$('#mr-no').val()));
                if(DeliveryInfo == null)
                    DeliveryInfo = '{}';
                var AttachmentInfo = (localStorage.getItem('DataTables_tblAttachment_'+$('#mr-no').val()));
                if(AttachmentInfo == null)
                    AttachmentInfo = '{}';                   
               
                var PageName = "'"+ getPageName() + "'";
                var MRRole = "'"+ getMRRole() + "'";
                var CurrentLink = "'"+ getCurrentLink() + "'";
                var CurrentMRStatus = "'"+ getCurrentMRStatus() + "'";
                var MRScheme = "'"+ getMRScheme() + "'";
                
                var datastr = '{MRInfo: ' + (localStorage.getItem('DataTables_tblMRHeader_'+$('#mr-no').val())) + ', ' + 
                              'ProductInfo: ' + ProductInfo +', ' + 
                              'ConfirmedSalesInfo: ' + ConfirmedSalesInfo + ', ' + 
                              'VendorInfo: ' + VendorInfo + ', ' + 
                              'DeliveryInfo: ' + DeliveryInfo + ', ' +  
                              'AttachmentInfo: ' + AttachmentInfo + ', ' +  
                              'PageName: ' + PageName + ', ' +                               
                              'CompanyId: ' + getCoyID() + ', ' +
                              'UserID: ' + getUserID() + ', ' +
                              'CurrentLink: ' + CurrentLink+ ', ' +
                              'CurrentMRStatus: ' + CurrentMRStatus + ', ' +
                              'MRScheme: ' + MRScheme + ', ' +
                              'MRRole: ' + MRRole +                             
                              '}';
                                                     
                $.ajax({
                    type: "POST",
                    url: "AddEditMaterialRequisition.aspx/SaveMR",
                    data: datastr,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        response = response.hasOwnProperty('d') ? response.d : response;
                        if(ProductInfo != null) 
                            localStorage.removeItem('DataTables_tblProduct_' + $('#mr-no').val()); 
                        if(ConfirmedSalesInfo != null)
                            localStorage.removeItem('DataTables_tblSales_' + $('#mr-no').val()); 
                        if(VendorInfo != null)
                            localStorage.removeItem('DataTables_tblVendor_' + $('#mr-no').val()); 
                        if(AttachmentInfo != null)
                            localStorage.removeItem('DataTables_tblAttachment_' + $('#mr-no').val()); 
                        
                        if(response != 'success')   
                            SetModalMessage("Record have been saved successfully.", response);  
                        else 
                        {
                            SetModalMessage("Record have been saved successfully.", "");  
                            UpdateStatusAndButton();
                        }   
                    }
                }); 
            }              
        }        
       
        
        
        
        function SaveMRHeader(tablename)
        {           
            var tbl = JSON.parse(localStorage.getItem('DataTables_'+ tablename));
            var objHeader = {};             
            objHeader['mrno'] = $('#mr-no').val();
            objHeader['purchaser'] = $('#purchaser').val();           
            objHeader['purchaserid'] = $('#purchaser-id').val();   
            objHeader['BudgetCode'] = $('#budget-code').val();
            objHeader['RefNo'] = $('#ref-no').val();
            objHeader['ProjectNo'] = $('#project-no').val();
            objHeader['sourceid'] = $('#source').val();
            objHeader['freightmode'] = $('#freight-mode').val();
            objHeader['statusname'] = $('#status').val();
            objHeader['statusid'] = $('#status-id').val();
            if($("#Console").prop('checked') == true) 
            {           
                objHeader['isconsole'] = "Yes";
                objHeader['consoleDate'] = $('#console-date').val();
            }
            else
            {
                objHeader['isconsole'] = "No";
                objHeader['consoleDate'] = "";
            }
            
            if($("#ismov").prop('checked') == true) 
            {                
                objHeader['ismov'] = "Yes";
                objHeader['mov'] = $('#mov').val();  
            }  
            else 
            {                
                objHeader['ismov'] = "No";
                objHeader['mov'] = ""; 
            }
            
            objHeader['mrdate'] = $('#mr-date').val();
            objHeader['requestorremarks'] = $('#requestor-remarks').val();
            objHeader['orderreason'] = $('#purchase-reasons').val();
            objHeader['vendorremarks'] = $('#vendor-remarks').val();
            objHeader['purchasingremarks'] = $('#delivery-remarks').val();
            objHeader['cancelledreason'] = $('#cancelled-reason').val();
            objHeader['glcode'] = $('#GLCode').val();
            objHeader['requestorname'] = $('#requestor-name').val();
            objHeader['requestor'] = $('#requestor-id').val();
            objHeader['pmname'] = $('#approver1-name').val();
            objHeader['pmuserid'] = $('#approver1-id').val();
            objHeader['phname'] = $('#approvismover2-name').val();
            objHeader['phuserid'] = $('#approver2-id').val();
            objHeader['ph3name'] = $('#approver3-name').val();
            objHeader['ph3userid'] = $('#approver3-id').val();
            objHeader['ph5name'] = $('#approver4-name').val();
            objHeader['ph5userid'] = $('#approver4-id').val();  
            objHeader['intendedusage'] = getIntendedUse();           
            objHeader['mrscheme'] = getMRScheme();
            objHeader['Discount'] = $('#Discount').val();  
            objHeader['TaxRate'] = $('#TaxRate').val()*100; 
            objHeader['TaxTypeID'] = $('#TaxTypeID').val();
            objHeader['ExchangeRate'] = $('#exchange-rate').val();            
            objHeader['DimensionL1'] = $("input[id*=hidDimensionL1]").val();
            objHeader['Warehouse'] = $("input[id*=hidWarehouse]").val();
            objHeader['othersRemarks'] = $('#OthersRemarks').val();
            objHeader['dim1'] = $('#dim1').val();
            objHeader['dim2'] = $('#dim2').val();
            objHeader['dim3'] = $('#dim3').val();
            objHeader['dim4'] = $('#dim4').val();
            localStorage.setItem( 'DataTables_'+tablename, JSON.stringify(objHeader));
        }   
                 
                  
        function ConfirmedSales(){
        var CoyID = getUrlVars()["CoyID"];         
        var MRNo = getUrlVars()["MRNo"]; 
         
        var a = $('#tblSales').DataTable( {
                "ajax": {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "AddEditMaterialRequisition.aspx/GetConfirmedSales",
                "data": function (data) { return "{'CompanyId':"+CoyID+",'MRNo':'"+MRNo+"'}";},  
                "dataSrc": function (json) {  
                    json = json.hasOwnProperty('d') ? json.d : json;
                    if(MRNo == undefined || MRNo == '')  
                    {   
                        if(JSON.stringify(localStorage.getItem('DataTables_tblSales_'+$('#mr-no').val())) == "null")
                            return json;                        
                        else                
                            return JSON.parse(localStorage.getItem('DataTables_tblSales_'+$('#mr-no').val()));  
                    }                  
                    else
                        return json;                    
                 }
            },
            "responsive": false,  
            "stateSave": true,   
            "filter" : false, 
            "ordering": false,     
            "bDestroy": true,
            "jQueryUI": true,
            "dom": 'Bfrtip',
            "buttons": [
                {
                    "text": 'Add',
                    "className" : 'btn btn-primary btn-md',
                    "enabled" : false,  
                    "value" : "New-" + $.now(),                  
                    "action": function ( e, dt, node, config ) { 
                        ResetForm('ModalConfirmSales');       
                        $('#CustomerName').attr("disabled", true);                
                        $('#save-confirmedSales').val("New-" + $.now());                   
                        $('#ModalConfirmSales').modal({backdrop: 'static', keyboard: false});  
                        
                    }
                },
            ],
            "language": {
                "emptyTable": "No results found!"
            },
            "rowId": "FileID",
            "columns": [
                {
                    "className":      'details-control',
                    "orderable":      false,                    
                    "data":           null
                },                       
                { "data": "CustomerAccountCode",
                  "title": "Customer Code",
                  "width": "10%"
                },
                { "data": "CustomerAccountName",
                  "title": "Customer Name"                  
                },
                { "data": "SONo",
                  "title": "Customer PO No.",
                  "width": "16%"
                },            
                { "data": "SODate",
                  "title": "Customer PO Date",
                  "width": "11%"
                },
                { "data": "RequiredDate",
                  "title": "Required Date",
                  "width": "11%"
                },                
                { 
                    "data": null ,
                    "className": 'all action', 
                    "title":"Action",                                     
                    "render": function (data, type, row){
                        return "<button type='button' id='confirmedSalesEdit' onclick=\"return false;\" value='"+row.FileID+"' name='Edit' class='btn btn-xs btn-default "+ $('#hidAllowChanges').val()+ "' "+$('#hidAllowChanges').val()+"><i class='ti-pencil'></i></button>&nbsp;<button type='button' id='confirmedSalesDelete' onclick=\"return false;\"  value='"+row.FileID+"' name='Delete' class='btn btn-xs btn-default delete "+ $('#hidAllowChanges').val()+ "' "+$('#hidAllowChanges').val()+"><i class='ti-trash'></i></button>";
                    }
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
            "fnRowCallback": function( nRow, aData, iDisplayIndex, iDisplayIndexFull ) {
                $('#hidCustomerCode').val(aData['CustomerAccountCode']);
            }
        }); 
                
        if($('#hidAllowChanges').val() == "")
            a.buttons().enable();                
                         
      }  
     
     
      function Vendor(){
        var MRNo = getMRNo(); 
        var CoyID = getCoyID();  
        var a = $('#tblVendor').DataTable( {
            "ajax": {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "AddEditMaterialRequisition.aspx/GetVendor",               
                "data": function (data) { return "{'CompanyId':"+CoyID+",'MRNo':'"+MRNo+"'}";},
                "dataSrc": function (json) {
                    json = json.hasOwnProperty('d') ? json.d : json;
                    if(MRNo == undefined || MRNo == '')      
                    {
                        if(JSON.stringify(localStorage.getItem('DataTables_tblVendor_'+$('#mr-no').val())) == "null")
                            return json;
                        else            
                            return JSON.parse(localStorage.getItem('DataTables_tblVendor_'+$('#mr-no').val()));   
                    }              
                    else
                        return json;
                 }
            },  
            "responsive": false,  
            "stateSave": true,   
            "filter" : false,  
            "ordering": false,    
            "bDestroy": true,
            "jQueryUI": true,
            "dom": 'Bfrtip',
            "buttons": [
                {
                    "text": 'Add',
                    "className" : 'btn btn-primary btn-md',
                    "enabled" : false,
                    "action": function ( e, dt, node, config ) {
                        ResetForm('ModalVendor');                     
                        $('#save-vendor').val("New-" + $.now());                   
                        $('#ModalVendor').modal({backdrop: 'static', keyboard: false}); 
                    }
                },
            ],
            "language": {
                "emptyTable": "No results found!"
            },
            "rowId": "VendorID",
            "columns": [     
                {
                    "className":      'details-control',
                    "orderable":      false,
                    "data":           null
                },
                {
                    "data": "VendorCode",
                    "title": "Vendor Code"
                },
                { "data": "VendorName",
                  "title": "Name"
                },
                { "data": "VendorContact",
                  "title": "Contact"
                },
                { "data": "VendorTel", 
                  "title": "Tel"
                },            
                { "data": "VendorFax",
                  "title": "Fax"
                },
                { "data": "VendorEmail",
                  "title": "Email"
                },                
                { 
                    "data": null , 
                    "title":"Action", 
                    "render": function (data, type, row){
                        return "<button type='button' onclick=\"return false;\"  value='"+row.VendorID+"' name='Edit' class='btn btn-xs btn-default "+ $('#hidAllowChanges').val()+ "' "+$('#hidAllowChanges').val()+"><i class='ti-pencil'></i></button>&nbsp;<button type='button' onclick=\"return false;\"  value='"+row.VendorID+"' name='Delete' class='btn btn-xs btn-default delete "+ $('#hidAllowChanges').val()+ "' "+$('#hidAllowChanges').val()+"><i class='ti-trash'></i></button>";
                    }
                }                
            ],
            "columnDefs": [{
                "className": 'all',
                "orderable": false,
                "targets":   -1
            }],
            "rowCallback": function( row, data, iDisplayIndex ) {
                 var info = a.page.info();
                 var page = info.iPage;
                 var length = info.length;
                 var index = (page * length + (iDisplayIndex +1));
                 $('td[class="details-control"]', row).html(index);
            },   
            "fnCreatedRow": function (row, data, index) {
                $('td', row).eq(0).html(index + 1);
            }
        });   
        
        if($('#hidAllowChanges').val() == "")
            a.buttons().enable();     
        
    } 
     
       
    function Product(){
        var MRNo = getMRNo(); 
        var CoyID = getCoyID();  
        var UserId = getUserID(); 
        var SubTotal = 0;         
        var GrandTotal = 0;     
        var a = $('#tblProduct').DataTable( {
            "ajax": {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "AddEditMaterialRequisition.aspx/GetProduct",               
                "data": function (data) { return "{'CompanyId':"+CoyID+",'MRNo':'"+MRNo+"'}";},
                "dataSrc": function (json) {
                    json = json.hasOwnProperty('d') ? json.d : json;
                    if(MRNo == undefined || MRNo == '')  
                    { 
                        if(JSON.stringify(localStorage.getItem('DataTables_tblProduct_'+$('#mr-no').val())) == "null")
                            return json;
                        else                     
                            return JSON.parse(localStorage.getItem('DataTables_tblProduct_'+$('#mr-no').val())); 
                    }                
                    else
                        return json;                                    
                 }
            },           
            "responsive": false,
            "stateSave": false, // disable memeory caching for purchase related information      
            "bDestroy": true,
            "jQueryUI": true,
            "ordering": false,
            "paging": false,
            "dom": 'Bfrtip',
            "buttons": [
                {
                    "text": 'Add',
                    "className" : 'btn btn-primary btn-md',
                    "enabled" : false,
                    "action": function ( e, dt, node, config ) {
                        ResetValidation();                    
                        ResetForm('ModalProduct');   
                        $('#ProdName').prop("readonly", true);               
                        $('#save-product').val("New-" + $.now());
                        GetCompany(function(data){   
                            $('#SellingCurrency').val(data[0].DefaultCurrencyCode);
                            $('#PurchaseCurrency').val(data[0].DefaultCurrencyCode);                         
                        });        
                        $('#ModalProduct').modal({backdrop: 'static', keyboard: false}); 
                    }
                },
            ],
            "language": {
                "emptyTable": "No results found!"
            },
            "rowId": "DetailNo",
            "columns": [   
                {
                    "className":      'details-control',
                    "orderable":      false,
                    "data":           null,
                    "defaultContent": ''
                },                
                { "data": "ProdCode" ,
                  "title": "Product Code",
                  "render": function (data, type, row){  
                        return "<a data-toggle='popover' data-trigger='hover' title='Stock Status for "+data+"' data-content='' data-html='true' onMouseOver=\"GetProductInfo('"+ data +"', this, '"+ UserId +"', '"+ $('#hidCustomerCode').val() +"');\"><span>" + data + "</span></a>" + 
                        "<br />" + row.NewProdCode;
                    }                  
                },
                {
                    "data": "RecipeNo",
                    "title": "Recipe No",
                    "visible": false
                },
                { 
                  "data": "ProdName" , 
                  "title": "Product Name"
                },    
                { "data": "UOM", 
                  "title": "UOM"
                },            
                { "data": "ConfirmedOrderQty", 
                  "title": "Sales Qty"
                },
                { "data": "ForStockingQty",
                  "title": "Stk Qty"
                },
                { "data": "OrderQty",
                  "title": "Order Qty",
                  "className": "datatable_bold"
                },                
                { "data": "SellingCurrency",
                  "title": "S.Curr."
                },
                { "data": "UnitSellingPrice",
                  "title": "S.Price"
                },                              
                { "data": "PurchaseCurrency",
                  "title": "P.Curr.",
                  "visible" : false,
                  "className": "datatable_bold"                   
                },
                { "data": "UnitPurchasePrice",
                  "title": "P.Price" ,
                  "visible" : false,
                  "className": "datatable_bold"                  
                },   
                { "data": "TotalPurchasePrice",
                  "title": "Total P.P",
                  "visible" : false
                },
                { 
                    "data": null , 
                    "title":"Action",                                         
                    "render": function (data, type, row){
                        return "<button type='button' onclick=\"return false;\"  value='" + row.DetailNo + "' name='Edit' class='btn btn-xs btn-default button-icon " + $('#hidAllowChanges').val() + "' " + $('#hidAllowChanges').val() + "><i class='ti-pencil'></i></button><button type='button' onclick=\"return false;\"  value='" + row.DetailNo + "' name='Delete' class='btn btn-xs btn-default button-icon delete " + $('#hidAllowChanges').val() + "' " + $('#hidAllowChanges').val() + "><i class='ti-trash'></i></button>";
                    }
                } ,
                /*, 
                { "data": "Approver1" ,
                  "title": "Approver (1)"
                },
                { "data": "Approver2" ,
                  "title": "Approver (2)"
                },
                 { "data": "Approver3" ,
                  "title": "Approver (3)"
                },
                 { "data": "Approver4" ,
                  "title": "Approver (4)"
                }           
                */                         
            ],
            "columnDefs": [ {
                "className": 'all',
                "orderable": false,
                "targets":   -1
                }
             ],
            "rowCallback": function( row, data, iDisplayIndex ) {
                 var info = a.page.info();
                 var page = info.iPage;
                 var length = info.length;
                 var index = (page * length + (iDisplayIndex +1));
                 $('td[class="details-control"]', row).html(index);
            }, 
            "fnDrawCallback": function (aData, type, full, meta) {
                   
            },  
            "fnCreatedRow": function (row, data, index) {
                $('td', row).eq(0).html(index + 1);
            },                      
            "fnRowCallback": function( nRow, aData, iDisplayIndex, iDisplayIndexFull ) {
                    var api = this.api(), data;  
                    // Remove the formatting to get integer data for summation
                    var intVal = function ( i ) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '')*1 : typeof i === 'number' ?  i : 0;
                    };

                    // total_salary over all pages
                    SubTotal = api.column( 12 ).data().reduce( function (a, b) {
                       return intVal(a) + intVal(b);
                    },0 );
                    
                   
//                    SubTotal = SubTotal + (aData['UnitPurchasePrice']*aData['OrderQty']); 
                    $('#SubTotal').val(SubTotal.toFixed(2));  
                    ReCalculate();
                    
            }
            
        }); 
        
        if($('#hidAllowChanges').val() == "")
            a.buttons().enable();
        
        if($('#hidViewPurchaseInfo').val() == "1")
            a.columns([10, 11, 12]).visible(true);

        if (getMRScheme() == "Department" && CoyID == "120")
            a.columns([2]).visible(true);
    } 
    
    function ReCalculate()
    {
         var SubTotal = $('#SubTotal').val() || 0; 
         var Discount = $('#Discount').val() || 0;  
         var TaxRate = $('#TaxRate').val() || 0;             
         var TaxAmount = (SubTotal - Discount) * (TaxRate); 
         TaxAmount = TaxAmount.toFixed(4);  
         $('#TaxAmount').val(TaxAmount); 
         GrandTotal = parseFloat(SubTotal) - parseFloat(Discount) + parseFloat(TaxAmount);        
         GrandTotal = GrandTotal.toFixed(4);  
         $('#GrandTotal').val(GrandTotal);
        
    }
    
    function Delivery(){
        var MRNo = getMRNo(); 
        var CoyID = getCoyID();    
        var UserId = getUserID(); 
        var a = $('#tblDelivery').DataTable( {  
            "ajax": {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "AddEditMaterialRequisition.aspx/GetDelivery",               
                "data": function (data) { return "{'CompanyId':"+CoyID+",'MRNo':'"+MRNo+"'}";},
                "dataSrc": function (json) {
                    json = json.hasOwnProperty('d') ? json.d : json;
                    if(MRNo == undefined || MRNo == '')
                    {
                        if(JSON.stringify(localStorage.getItem('DataTables_tblDelivery_'+$('#mr-no').val())) == "null")
                            return json;
                        else 
                            return JSON.parse(localStorage.getItem('DataTables_tblDelivery_'+$('#mr-no').val()));      
                    }           
                    else
                        return json;                   
                 }
            },
            "responsive": false,  
            "stateSave": true, 
            "filter" : false,   
            "ordering": false,  
            "bDestroy": true,
            "jQueryUI": true,
            "dom": 'Bfrtip',
            "buttons": [
                {
                    "text": 'Add',
                    "className" : 'btn btn-primary btn-md',
                    "enabled" : false,
                    "action": function ( e, dt, node, config ) {
                        ResetForm('ModalDelivery');                     
                        $('#save-delivery').val("New-" + $.now());                   
                        $('#ModalDelivery').modal({backdrop: 'static', keyboard: false});
                        
                    }
                },
            ],
            "language": {
                "emptyTable": "No results found!"
            },
            "rowId": "DeliveryNo",
            "columns": [ 
                {
                    "className":      'details-control',
                    "orderable":      false,
                    "data":           null                    
                }, 
                { 
                  "data": "PONo" , 
                  "title": "PO No.",                  
                  "render" : function(data, type, row, meta){
                    var a = '<a onclick=\'OpenPopUp("'+MRNo+'","'+data+'");\'">'+data+'</a>'
                    return a;     
                  } 
                },
                { "data": "Purchaser" , 
                  "title": "Purchaser"
                },
                { "data": "PODate", 
                  "title": "PODate"
                },            
                { "data": "CRD", 
                  "title": "CRD"
                },
                { "data": "ETD", 
                  "title": "ETD"
                },
                { "data": "ETA", 
                  "title": "ETA"
                },
                { "data": "null",
                  "title": "GRN Info" ,
                  "render" : function(data, type, row, meta){
                    return "<a data-toggle='popover' data-trigger='click' title='GRN Info' data-content='' data-html='true' onMouseDown=\"GetGRNInfo('"+ row.PONo +"', this,'"+ MRNo +"','"+ UserId +"');\"><span>Click To View</span></a>"
                  }                
                },
                { 
                    "data": null , 
                    "title":"Action", 
                    "render": function (data, type, row){
                        return "<button type='button' onclick=\"return false;\"  value='" + row.DeliveryNo + "' name='Edit' class='btn btn-xs btn-default " + $('#hidAllowChanges').val() + "' " + $('#hidAllowChanges').val() + "><i class='ti-pencil'></i></button><button type='button' onclick=\"return false;\"  value='" + row.DeliveryNo + "' name='Delete' class='btn btn-xs btn-default button-icon delete " + $('#hidAllowChanges').val() + "' " + $('#hidAllowChanges').val() + "><i class='ti-trash'></i></button>";
                    }
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
            }
        });   
        

        //$('.date').datetimepicker({
        //        format: 'DD/MM/YYYY'
        //}); 
        
        if($('#hidAllowChanges').val() == "")
            a.buttons().enable();
    } 
    
    
    function Attachment(){
        var MRNo = getMRNo(); 
        var CoyID = getCoyID();  
        var a = $('#tblAttachment').DataTable( {
            "ajax": {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "AddEditMaterialRequisition.aspx/GetAttachment",               
                "data": function (data) { return "{'CompanyId':"+CoyID+",'MRNo':'"+MRNo+"'}";},
                "dataSrc": function (json) {
                    json = json.hasOwnProperty('d') ? json.d : json;
                    if(MRNo == undefined || MRNo == '')  
                    {   
                        if(JSON.stringify(localStorage.getItem('DataTables_tblAttachment_'+$('#mr-no').val())) == "null")
                            return json;
                        else              
                            return JSON.parse(localStorage.getItem('DataTables_tblAttachment_'+$('#mr-no').val()));   
                    }              
                    else
                        return json;                   
                 }
            },
            "responsive": false,  
            "stateSave": true,   
            "filter" : false,   
            "ordering": false,
            "bDestroy": true,
            "jQueryUI": true,    
            "rowId": "FileID",       
            "columns": [  
                {
                    "className":      'details-control',
                    "orderable":      false,
                    "data":           null
                },              
                { "data": "FileDisplayName" ,
                  "title": "Display Name"
                },
                { "data": "FileName" ,
                  "title": "File Name" ,
                  "render" : function(data, type, row, meta){
                    var a = '<a onclick=\'Download("'+CoyID+'","'+row.FileNameEncrypted+'");\'">'+data+'</a>'
                    return a;     
                  }                
                },
                { "data": "DocumentCategory" ,
                  "title": "Category"
                },
                { 
                    "data": null , 
                    "title":"Action", 
                    "render": function (data, type, row){
                        return "<button type='button' onclick=\"return false;\"  value='"+row.FileID+"' name='Edit' class='btn btn-xs btn-default"+ $('#hidAllowChanges').val()+ "' "+$('#hidAllowChanges').val()+"><i class='ti-pencil'></i></button>&nbsp;<button type='button' onclick=\"return false;\"  value='"+row.FileID+"' name='Delete' class='btn btn-xs btn-default delete "+ $('#hidAllowChanges').val()+ "' "+$('#hidAllowChanges').val()+"><i class='ti-trash'></i></button>";
                    }
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
            }
        });  
        
        if($('#hidAllowChanges').val() == "")
            a.buttons().enable(); 
    } 
    
    
     function RoutingInfo(){
        var MRNo = getMRNo(); 
        var CoyID = getCoyID();  
        var a = $('#tblRouting').DataTable( {
            "ajax": {
                "dataType": "json",
                "contentType": "application/json; charset=utf-8",
                "type": "POST",
                "url": "AddEditMaterialRequisition.aspx/GetRoutingInfo",               
                "data": function (data) { return "{'CompanyId':"+CoyID+",'MRNo':'"+MRNo+"'}";},
                "dataSrc": function (json) {
                    json = json.hasOwnProperty('d') ? json.d : json;
                    return json;
                 }
            },
            "responsive": false,  
            "stateSave": true,         
            "bDestroy": true,
            "filter": false,
            "jQueryUI": true,
            "ordering": false,
            "columns": [     
                {
                    "className":      'details-control',
                    "orderable":      false,
                    "data":           null
                },           
                { "data": "statusname" , "title": "Status Name"},
                { "data": "approver" , "title": "Approver / Person In Charge" },
                { "data": "routeddate" , "title": "Date Route"} ,
                { "data": "actiondate" , "title": "Date Action"},
                { "data": "reason" , "title": "Reason"}                                        
            ],
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
            "fnRowCallback": function( nRow, aData, iDisplayIndex, iDisplayIndexFull ) {
                    if ( aData['iscurrentlevel'] == "Y")
                    {
                        $('td', nRow).css('background-color', '#266c8e');
                        $('td', nRow).css('color', '#fff');
                        
                    }
            }
        });   
     }

     function setTaxInfo()
     {
         var MRNo = getMRNo();
         var CoyID = getCoyID();
         

         $.ajax({
             url: "AddEditMaterialRequisition.aspx/GetMRHeader",
             dataType: "json",
             type: "POST",
             contentType: "application/json; charset=utf-8",
             data: "{'CompanyId' : " + CoyID + ",'MRNo': '" + MRNo + "'}",
             success: function (data) {
                 data = data.hasOwnProperty('d') ? data.d : data;
                 $.map(data, function (item) {
                     $('#TaxRate').val(item.TaxRate);                    
                     $('#TaxTypeID').val(item.TaxTypeID);                    
                     $('#TaxTypeName').val(item.TaxTypeID + '-' + item.TaxName);
                     $('#Discount').val(item.Discount);
                     ReCalculate();
                     action = data.Action;
                 })
             },
             error: function (xhr, ajaxOptions, thrownError) {
                 alert('Failed to retrieve detail.');
             }
         });

     }
    
    function GetMRHeader(){   
        var MRNo = getMRNo(); 
        var CoyID = getCoyID();       
                         
         $.ajax({
            url: "AddEditMaterialRequisition.aspx/GetMRHeader",
            dataType: "json",            
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: "{'CompanyId' : "+CoyID + ",'MRNo': '" + MRNo + "'}",
            success: function (data) {
                data = data.hasOwnProperty('d') ? data.d : data;
                $.map(data, function (item) {                    
                    $('#mr-no').val(item.mrno);
                    $('#purchaser').val(item.PurchaserName);
                    $('#purchaser-id').val(item.purchaser);
                    $('#budget-code').val(item.BudgetCode); 
                    $('#ref-no').val(item.RefNo);                     
                    $("select[id*='source'] option[value='"+item.sourceid+"']").attr("selected",true);  
                    $('#status').val(item.statusname);
                    $('#status-id').val(item.statusid); 
                    if(item.sourceid=="Overseas")                     
                        $("select[id*='freight-mode'] option[value='"+item.freightmode+"']").attr("selected",true); 
                    else 
                        $("select[id*='freight-mode'] option[value='-']").attr("selected",true); 
                                       
                    if(item.isconsole =="True")
                    {
                       $("#Console").closest('.checkbox-inline, .checkbox').toggleClass('checked');
                       $("#Console").prop('checked', true);  
                       $("#Console").closest('.additional-info-wrap').find('.additional-info').removeClass('hide').find('input,select').removeAttr('disabled');  
                    }   
                    
                    if(item.IsMOV == "True")
                    {
                       $("#ismov").closest('.checkbox-inline, .checkbox').toggleClass('checked');
                       $("#ismov").prop('checked', true);  
                       $("#ismov").closest('.additional-info-wrap').find('.additional-info').removeClass('hide').find('input,select').removeAttr('disabled');  
                    }
                    $('#mov').val(item.MOV); 
                    
                    $('#mr-date').val(item.mrdate);    
                    $('#console-date').val(item.consoleDate); 
                    $('#requestor-remarks').val(item.requestorremarks)                    
                    $('#purchase-reasons').val(item.orderreason); 
                    $('#vendor-remarks').val(item.vendorremarks);      
                    $('#delivery-remarks').val(item.purchasingremarks);                  
                    $('#cancelled-reason').val(item.cancelledreason);                     
                    $('#project-no').val(item.ProjectNo); 
                    $('#requestor-name').val(item.requestorname);
                    $('#requestor-id').val(item.requestor);
                    $('#approver1-name').val(item.pmname);
                    $('#approver1-id').val(item.pmuserid);
                    $('#approver2-name').val(item.phname);
                    $('#approver2-id').val(item.phuserid);
                    $('#approver3-name').val(item.ph3name);
                    $('#approver3-id').val(item.ph3userid);                    
                    $('#GLCode').val(item.glcode);
                    $('#OthersRemarks').val(item.OthersRemarks);                    
                    $('#Discount').val(item.Discount);  
                    $('#TaxRate').val(item.TaxRate); 
                    $('#TaxTypeID').val(item.TaxTypeID); 
                    $('#TaxTypeName').val(item.TaxTypeID + '-' + item.TaxName);
                    $('#exchange-rate').val(item.ExchangeRate);
                    $('#warehouse').val(item.Warehouse);
                    GetDim1(function (data) {
                        $('#dim1').val(item.Dim1);
                    });
                    GetDim2(item.Dim1, function (data) {
                        $('#dim2').val(item.Dim2);
                    });
                    GetDim3(item.Dim2, function (data) {
                        $('#dim3').val(item.Dim3);
                    });
                    GetDim4(item.Dim3, function (data) {
                        $('#dim4').val(item.Dim4);
                    });
                    
                    if(item.createdbyname != "")
                        $("label[for='Created']").html("Created By " + item.createdbyname + " On " + item.createddate);
                    if(item.modifiedbyname != "")
                        $("label[for='Modified']").html("Modified By " + item.modifiedbyname + " On " + item.modifieddate);
                    $('#CreatedBy-id').val(item.createdby);
                    
                    var array = item.intendedusage.split(",");
                    $.each(array,function(i){ 
                       if(array[i] == "Repair & Maintenance") 
                       {                            
                            $("#Repair-Maintenance").closest('.checkbox-inline, .checkbox').toggleClass('checked');
                            $("#Repair-Maintenance").prop('checked', true); 
                       }
                       else if(array[i] == "Staff Welfare") 
                       {
                            $("#Staff-Welfare").closest('.checkbox-inline, .checkbox').toggleClass('checked'); 
                            $("#Staff-Welfare").prop('checked', true); 
                       }
                       else if(array[i] == "Raw Material")  
                       {
                            $("#Raw-Material").closest('.checkbox-inline, .checkbox').toggleClass('checked');
                            $("#Raw-Material").prop('checked', true); 
                       } 
                       else if(array[i] == "Asset")  
                       {
                            $("#" + array[i]).closest('.checkbox-inline, .checkbox').toggleClass('checked');   
                            $("#" + array[i]).prop('checked', true);                        
                            $("#" + array[i]).closest('.additional-info-wrap').find('.additional-info').removeClass('hide').find('input,select').removeAttr('disabled');  
                       }
                       else if (array[i] == "Others") {
                           $("#" + array[i]).closest('.checkbox-inline, .checkbox').toggleClass('checked');
                           $("#" + array[i]).prop('checked', true);
                           $("#" + array[i]).closest('.additional-info-wrap').find('.additional-info').removeClass('hide').find('input,select').removeAttr('disabled');
                       }
                       else
                       {
                            $("#" + array[i]).closest('.checkbox-inline, .checkbox').toggleClass('checked');  
                            $("#" + array[i]).prop('checked', true);
                       }                       
                    });
                    action = data.Action;
        
                })
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Failed to retrieve detail.');
            }
        });
        
    }  
    
    function CalculatePurchasePrice()
    {
        var total =  ($('#OrderQty').val() * $('#UnitPurchasePrice').val());
        $('#TotalPurchasePrice').val(total.toFixed(2));
    }
    
    function SetModalMessage(message, redirect)
    {
        $('#btnClose').val(redirect);        
        $('#message').html(message);
        $('#ModalMessage').modal({backdrop: 'static', keyboard: false});
    }

    function CheckData(callback) {
        var CoyID = getCoyID();
        var UserId = getUserID();        
        var MRNo = getMRNo();
        $.ajax({
            async: true,
            type: "POST",
            url: "AddEditMaterialRequisition.aspx/CheckData",
            data: "{'CompanyId' : " + CoyID + ",'UserId': " + UserId + ", 'MRNo': '" + MRNo + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                data = data.hasOwnProperty('d') ? data.d : data;
                callback(data);
            },
            error: function (xhr, textstatus, error) {
                alert(textstatus);
            }
        });
    }
    
    
    function CheckAccess(callback){
       var CoyID = getCoyID();
       var UserId = getUserID();
       var MRNo = getMRNo();              
       
        $.ajax({
            async       : true,
            type        : "POST",
            url         : "AddEditMaterialRequisition.aspx/CheckUserAccess",
            data: "{'CompanyId' : " + CoyID + ",'UserId': " + UserId + ", 'MRNo': '" + MRNo + "', 'MRScheme': '" + getMRScheme() + "'}",
            contentType : "application/json; charset=utf-8",
            dataType    : "json",
            success: function (data) {
                            data = data.hasOwnProperty('d') ? data.d : data;
                            callback(data);
                          },
            error       : function(xhr, textstatus, error){
                            alert(textstatus);
                          }
        }); 
    }
    
    
    function GetMRHeaderByMRNo(callback){
       var CoyID = getCoyID();       
       var MRNo = getMRNo();     
   
        $.ajax({
            async       : true,
            type        : "POST",
            url         : "AddEditMaterialRequisition.aspx/GetMRHeaderByMRNo",
            data        : "{'CompanyId' : "+CoyID+",'MRNo': '" + MRNo +"'}",
            contentType : "application/json; charset=utf-8",
            dataType    : "json",
            success     : function (data) {
                            data = data.hasOwnProperty('d') ? data.d : data;
                            callback(data);
                          },
            error       : function(xhr, textstatus, error){
                            alert(textstatus);
                          }
        }); 
}
     
function OpenPopUp(mrno, po)
{   
    var url = "";
    if($('#hidViewPurchaseInfo').val() == "1")
        url = "ViewPO.aspx?MRNo=" + mrno + "&TrnNo="+ po;
    else
        url = "ViewPOWithoutSupplierInfo.aspx?MRNo=" + mrno + "&TrnNo="+ po;
        
   window.open(url,"","width=750,height=500,resizable=yes,status=yes,menubar=no,scrollbars=yes");	
}   

function Download(CoyID, fileName)
{  
    window.location = "FileDownloadHandler.ashx?CompanyId="+CoyID+"&FileName="+fileName;
} 

function format ( d ) {
    // `d` is the original data object for the row
    return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">'+
        '<tr>'+
            '<td>Full name:</td>'+
            '<td>'+d.Approver1+'</td>'+
        '</tr>'+
        '<tr>'+
            '<td>Extension number:</td>'+
            '<td>'+d.Approver2+'</td>'+
        '</tr>'+
        '<tr>'+
            '<td>Extra info:</td>'+
            '<td>And any further details here (images etc)...</td>'+
        '</tr>'+
    '</table>';
} 
      
$("#ImportRecipe").on("click dblclick", function () {
    ImportRecipe();
});

        
function ImportRecipe(item) {
    var CoyID = getCoyID();
    var urlink = get_hostname(window.location.href);
    var MRScheme = getMRScheme();
    var KeyToSplit = "";
    if (MRScheme=="Product" || item.value == "") {
        return;
    }
    else {
        $.ajax({
            async: true,
            type: "POST",
            url: urlink + "/GMS4/Products/Products/AddEditMaterialRequisition.aspx/ImportRecipeInfo",
            data: "{'CompanyId' : " + CoyID + ", 'recipeno': '" + item.value + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                data = data.hasOwnProperty('d') ? data.d : data;
                //alert("Message: Valid recipe no.");
                return;
            },
            error: function (xhr, text, status) {
                item.value = "";
                var r = jQuery.parseJSON(xhr.responseText);
                alert("Message: " + r.Message);
            }
        });
    }
}

function MRGasDimension() {
    var CoyID = getCoyID();
    var Scheme = getMRScheme();
    var $dim1 = $('#dim1');
    var $dim2 = $('#dim2');
    var $dim3 = $('#dim3');
    var $dim4 = $('#dim4');

    if(CoyID == 120 && Scheme == 'Department'){
        $("#rowgasdivision").css("display", '');
            GetDim1(function (data) { });
    }

    $('#dim1').change(function () {
        GetDim2($(this).val(), function () {
            $('#dim3').empty().append(function () {
                return '<option value="-1">L3NA</option>'
            });

            $('#dim4').empty().append(function () {
                return '<option value="-1">L4NA</option>'
            });
        });
    });

    $('#dim2').change(function () {
        GetDim3($(this).val(), function () {
            $('#dim4').empty().append(function () {
                return '<option value="-1">L4NA</option>'
            });
        });
    });

    $('#dim3').change(function () {
        GetDim4($(this).val(), function () { });
    });
}

function GetDim1(callback) {
    var CoyID = getCoyID();
    var Scheme = getMRScheme();
    var UserId = getUserID();
    var urlink = get_hostname(window.location.href);
    var $dim1 = $('#dim1')
    var $dim2 = $('#dim2')

    $.ajax({
        async: true,
        type: "POST",
        url: urlink + "/GMS4/Products/Products/AddEditMaterialRequisition.aspx/GetDim1",
        data: "{'CompanyId' : '" + CoyID + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            data = data.hasOwnProperty('d') ? data.d : data;
            $dim1.empty().append(function () {
                var output = '';
                $.each(data, function (key, value) {
                    output += '<option value="' + value.ProjectID + '">' + value.ProjectName + '</option>';
                });
                return output;
            });
            callback()
            return;
        },
        error: function (xhr, text, status) {
            var r = jQuery.parseJSON(xhr.responseText);
            alert("Message: " + r.Message);
        }
    });
}

function GetDim2(projectid, callback) {
    var CoyID = getCoyID();
    var Scheme = getMRScheme();
    var UserId = getUserID();
    var urlink = get_hostname(window.location.href);
    var $dim2 = $('#dim2')

    $.ajax({
        async: true,
        type: "POST",
        url: urlink + "/GMS4/Products/Products/AddEditMaterialRequisition.aspx/GetDim2",
        data: "{'CompanyId' : '" + CoyID + "', 'ProjectId' : '" + projectid + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            data = data.hasOwnProperty('d') ? data.d : data;
            $dim2.empty().append(function () {
                var output = '';
                $.each(data, function (key, value) {
                    output += '<option value="' + value.DepartmentID + '">' + value.DepartmentName + '</option>';
                });
                return output;
            });
            callback()
        },
        error: function (xhr, text, status) {
            var r = jQuery.parseJSON(xhr.responseText);
            alert("Message: " + r.Message);
        }
    });
}

function GetDim3(departmentid, callback) {
    var CoyID = getCoyID();
    var MRNo = getMRNo();
    var Scheme = getMRScheme();
    var UserId = getUserID();
    var urlink = get_hostname(window.location.href);
    var $dim3 = $('#dim3')
    var $dim4 = $('#dim4')

    $.ajax({
        async: true,
        type: "POST",
        url: urlink + "/GMS4/Products/Products/AddEditMaterialRequisition.aspx/GetDim3",
        data: "{'CompanyId' : '" + CoyID + "', 'DepartmentId' : '" + departmentid + "','MrDate' :'" + $('#mr-date').val() + "','Userid':'" + UserId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            data = data.hasOwnProperty('d') ? data.d : data;
            $dim3.empty().append(function () {
                var output = '';
                $.each(data, function (key, value) {
                    output += '<option value="' + value.SectionID + '">' + value.SectionName + '</option>';
                });
                return output;
            });

            callback();
        },
        error: function (xhr, text, status) {
            var r = jQuery.parseJSON(xhr.responseText);
            alert("Message: " + r.Message);
        }
    });
}

function GetDim4(sectionid, callback) {
    var CoyID = getCoyID();
    var Scheme = getMRScheme();
    var UserId = getUserID();
    var urlink = get_hostname(window.location.href);
    var $dim4 = $('#dim4')

    $.ajax({
        async: true,
        type: "POST",
        url: urlink + "/GMS4/Products/Products/AddEditMaterialRequisition.aspx/GetDim4",
        data: "{'CompanyId' : '" + CoyID + "', 'SectionId' : '" + sectionid + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            data = data.hasOwnProperty('d') ? data.d : data;
            $dim4.empty().append(function () {
                var output = '';
                $.each(data, function (key, value) {
                    output += '<option value="' + value.UnitID + '">' + value.UnitName + '</option>';
                });
                return output;
            });

            callback();
        },
        error: function (xhr, text, status) {
            var r = jQuery.parseJSON(xhr.responseText);
            alert("Message: " + r.Message);
        }
    });
}