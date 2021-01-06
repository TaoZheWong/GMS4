
$(document).ready(function () {

    tbIssue();

    btn_EditIssue();

    btn_NewIssue();
});

//Bind DataTable Issue
function tbIssue() {
    var system = getUrlVars()["System"];
    var type = getUrlVars()["Type"];
    var name = $('#ctl00_lblWelcome').text();
    var datastr = "{'system':'" + system + "'" +
                  ",'type':'" + type + "'" +
                  ",'IssueID':''" +
                  "}";
    var a = $('#tblIssue').DataTable({
        "ajax": {
            "dataType": "json",
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "Issue.aspx/GetIssue",
            "data": function (data) { return datastr; },
            "dataSrc": function (json) {
                return json;
            }
        },
        "responsive": false,
        "stateSave": false,
        "bDestroy": true,
        "filter": false,
        "jQueryUI": true,
        "dom": 'Bfrtip',
        "buttons": [
                {
                    "text": 'Add',
                    "className": 'btn btn-primary btn-md',
                    "enabled": true,
                    "action": function (e, dt, node, config) {
                        ResetValidation();
                        ResetForm('ModalIssue');
                        $('#input-Status option[value="N"]').prop('selected', true);
                        $('#input-Status').prop('disabled', true);
                        $("#input-System").prop('disabled', false);
                        $('#input-System option[value="' + system + '"]').prop('selected', true);
                        $("#input-Type").prop('disabled', false);
                        $('#input-Type option[value="' + type + '"]').prop('selected', true);
                        $('#input-ReportedBy').val(name);
                        $("#input-ReportedBy").prop('disabled', false);
                        $('#input-Description').prop('disabled', false);
                        $('#input-Description').val("");
                        $('#modaltitle').text("New Issue");
                        $('#DivRowRemarks').css("display", 'None');
                        $('#DivRowDates').css("display", 'None');
                        $("#save-Issue").val("");
                        $('#ModalIssue').modal({ backdrop: 'static', keyboard: false });

                    }
                },
        ],
        "language": {
            "emptyTable": "No results found!"
        },
        "rowId": "IssueID",
        "autoWidth": true,
        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null
            },
            {
                "data": "IssueID",
                "title": "Issue ID",
                "orderable": false,
                "width": "13%"
            },
            {
                "data": "Description",
                "title": "Description",
                "orderable": false,
                "width": "40%"
            },
            {
                "data": "ReportedBy",
                "title": "Reported By",
                "width": "13%"
            },
            {
                "data": "CreatedBy",
                "title": "Created By",
                "width": "13%"
            },
            {
                "data": "ReportedDate",
                "title": "Reported Date"
            },
            {
                "data": "CompletedDate",
                "title": "Completed Date"
            },
            {
                "data": "Status_Name",
                "title": "Status"
            },
            {
                "data": null,
                "title": "Action",
                "orderable": false,
                "render": function (data, type, row) {
                    return "<button type='button' onclick=\"return false;\"  value='" + row.IssueID + "' name='Edit' class='glyphicon glyphicon-pencil button-icon'></button>";
                }
            },

        ],

        "rowCallback": function (row, data, iDisplayIndex) {
            var info = a.page.info();
            var page = info.iPage;
            var length = info.length;
            var index = (page * length + (iDisplayIndex + 1));
            $('td[class="details-control"]', row).html(index);
        },
        "fnCreatedRow": function (row, data, index) {
            $('td', row).eq(0).html(index + 1);
        }
    });
}

//Save Button for New
function btn_NewIssue() {
    var errorMessage = "";
    $("#save-Issue").click(function () {

        var str = $("form").find("#ModalIssue :input").serializeArray();
        jQuery.each(str, function (i, field) {
            if ($('#' + field.name).val().length == 0 && field.name != "input-Remarks") {
                $('#Div' + field.name).addClass("has-error has-danger");
                $('#Div' + field.name).removeClass("has-success");
                $('#Div' + field.name).find('span').removeClass('glyphicon-ok');
                $('#Div' + field.name).find('span').addClass('glyphicon-remove');
                errorMessage = "Invalid " + field.name;
            }
        });
        if (errorMessage == "") {

            //New Issue Save Button
            var myStr = $('#input-Description').val();
            myStr = myStr.replace("'","`");

            if ($("#save-Issue").val() == "") {
                $.ajax({
                    type: "POST",
                    url: "Issue.aspx/InsertIssue",
                    data: "{'CoyID':'" + getCoyID() + "'" +
                      ",'System':'" + $('#input-System').val() + "'" +
                      ",'Type':'" + $('#input-Type').val() + "'" +
                      ",'ReportedBy':'" + $('#input-ReportedBy').val() + "'" +
                      ",'Description':'" + myStr + "'" +
                      ",'CreatedBy':'" + getUserID() + "'" +
                      "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Issue has been submitted!");
                        tbIssue();
                    },
                    error: function (xhr, text, status) {
                        var r = jQuery.parseJSON(xhr.responseText);
                        alert("Message: " + r.Message);
                    }
                });
            }
            //Edit Issue Save Button
            else {
                $.ajax({
                    type: "POST",
                    url: "Issue.aspx/UpdateIssue",
                    data: "{'IssueID':'" + $("#save-Issue").val() + "'" +
                      ",'System':'" + $('#input-System').val() + "'" +
                      ",'Type':'" + $('#input-Type').val() + "'" +
                      ",'ReportedBy':'" + $('#input-ReportedBy').val() + "'" +
                      ",'Description':'" + myStr + "'" +
                      ",'Remarks':'" + $('#input-Remarks').val() + "'" +
                      ",'Status':'" + $('#input-Status').val() + "'" +
                      ",'ModifiedBy':'" + getUserID() + "'" +
                      "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Issue has been saved!");
                        tbIssue();
                    },
                    error: function (xhr, text, status) {
                        var r = jQuery.parseJSON(xhr.responseText);
                        alert("Message: " + r.Message);
                    }
                });
            }
            $('#ModalIssue').modal('hide');
        }
        else {
            alert(errorMessage);
            errorMessage = "";
        }

    });
   
}

//Edit Button on Grid
function btn_EditIssue() {
    var system = getUrlVars()["System"];
    var type = getUrlVars()["Type"];

    var record_user;
    var record_status;
   
    $('#tblIssue').on('click', 'button', function (e) {
        //Bind value to edit form
        if (this.value != '') {
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                url: "Issue.aspx/GetIssue",
                data: "{'system':'" + system + "'" +
                      ",'type':'" + type + "'" +
                      ",'IssueID':'"+ this.value +"'" +
                      "}",
                success: function (data) {
                    $.each(data, function (key, value) {
                        $.each(value, function (i, item) {
                            $('#input-' + i).val(item);
                            if (i.indexOf("Date") >= 0) {
                                $('#input-' + i).text(item);
                            }
                            if (i == "Creator") {
                                record_user = item;
                            }
                            if (i == "Status") {
                                record_status = item;
                            }
                        });
                    });


                    //ReadOnly All (Not Admin)
                    if (getUserID() != 1) {
                        var str = $("form").find("#ModalIssue :input").serializeArray();
                        jQuery.each(str, function (i, field) {
                            $('#' + field.name).attr("disabled", "disabled");
                        });
                    }
                    //Creator Right to Edit
                    if (record_user == getUserID() && record_status == "N") {
                        $("#input-ReportedBy").prop('disabled', false);
                        $('#input-Description').prop('disabled', false);
                    }


                }
            });
        }

        
        $("#save-Issue").val(this.value);
        $('#modaltitle').text("Edit Issue");
        $('#DivRowRemarks').css("display", '');
        $('#DivRowDates').css("display", '');


        $('#ModalIssue').modal({ backdrop: 'static', keyboard: false });

        
    });
}
 
      
      
    
        
        
 