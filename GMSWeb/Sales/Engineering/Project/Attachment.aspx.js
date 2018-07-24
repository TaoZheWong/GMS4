$(document).ready(function() {
    Attachment();
    $("#btnSubmit").hide();
});
$(function () {
    $("#upload").hide();
    var PrjNo = $("#txtProjectNo").attr("value");
    $("#dialog-confirm1").hide();
    $("#FileUpload").change(function(){
    
         $( "#dialog-confirm1" ).dialog({
            resizable: false,
            height:140,
            modal: true,
            buttons: {
                "Confirm": function() {
                     $("#btnSubmit_ServerClick").trigger('click');   
                     $( this ).dialog( "close" ); 
                },
                Cancel: function() {
                    alert("Upload have been cancelled.");
                    $( this ).dialog( "close" );
                }
            }
        });  
    });
});
function Attachment(){
    var PrjNo = $("#txtProjectNo").attr("value");
    var table = $('#tblAttachment').DataTable( {
        "ajax": {
            "dataType": "json",
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "Attachment.aspx/GetAttachmentList",
            "data": function (d){
                 return "{ 'ProjectNo': '" + PrjNo + "' }";
            },
            "dataSrc": function (json) {
                return json;
            }   
        },
        "responsive": true,
        "bDestroy": true,
        "jQueryUI": true,
        "dom": 'Bfrtip',
        "buttons": [
            {
                "text": 'Add',
                "action": function ( e, dt, node, config ) {
                    $("#FileUpload").click();
                    $("#txtPrjNo").val(PrjNo);
                    $("#btnSubmit").show();
                }
            }
        ],
        "language": {
                "emptyTable":     "No results found!"
            },
        "rowId": "FileId",
        "columns": [
            { "data": "FileName", "title": "File Name" },
            { "data": "CreatedBy", "title": "UploadedBy"}
        ]
    } );
}

