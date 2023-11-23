// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    console.log("Test Javascript");


    $('#xtrtools-form').submit(function (event) {
        console.log("Test Click");

        $(".xtrtools-prcheckbox").hide();

        event.preventDefault();

        $.ajax({
            type: 'POST',
            url: '/api/v1/poadocument/pr-changes/get',
            data: $(this).serialize(),
            beforeSend: function (xhr) {
            },
            success: function (response) {
                PopulateFileList(response);
            },
            error: function (xhr, status, error) {
                if (xhr.responseText == null || xhr.responseText == "") {
                    SetErrorMessage(xhr.status);
                } else {
                    SetErrorMessage(xhr.responseText);
                }
            }
        });
    });

    $('#xtrtools-form-filelist').submit(function (event) {
        console.log("Test PR Checkbox button Click");

        event.preventDefault();

        const checkboxInput = new Array();

        $("#filelist .form-check-input:checked").each(function () {
            checkboxInput.push({ File: this.name, Context: this.attributes["context"].value });
        });

        var request = {
            "ProjectName": $('input[name="projectname"]').val(),
            "ProjectRepository": $('input[name="repositoryname"]').val(),
            "PRId": $('input[name="prid"]').val(),
            "CommitId": $('input[name="commitid"]').val(),
            "SinceCommitId": $('input[name="sincecommitid"]').val(),
            "Files": checkboxInput
        };

        $.ajax({
            type: 'POST',
            url: '/api/v1/pr-changes/export',
            data: JSON.stringify(request),
            contentType: "application/json; charset=utf-8",
            responseType: 'blob',
            xhrFields: {
                responseType: 'blob'
            },
            //dataType: "json",
            success: function (response, status, xhr) {
                var filename = GetFileNameFromXhr(xhr);
                var tempDom = document.createElement('a');
                var url = window.URL.createObjectURL(response);
                tempDom.href = url;
                tempDom.download = filename;
                document.body.append(tempDom);
                tempDom.click();
                tempDom.remove();
                window.URL.revokeObjectURL(url);
            },
            error: function (xhr, status, error) {
                if (xhr.responseText == null || xhr.responseText == "") {
                    SetErrorMessage(xhr.status);
                } else {
                    SetErrorMessage(xhr.responseText);
                }
            }
        });

    });
});

function GetFileNameFromXhr(xhr) {
    var filename = "";
    var disposition = xhr.getResponseHeader('Content-Disposition');
    if (disposition && disposition.indexOf('attachment') !== -1) {
        var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
        var matches = filenameRegex.exec(disposition);
        if (matches != null && matches[1]) {
            filename = matches[1].replace(/['"]/g, '');
        }
    }
    return filename;
}

function PopulateFileList(response) {
    $("#filelist").empty();

    $('input[name="projectname"]').val(response["projectName"]);
    $('input[name="repositoryname"]').val(response["projectRepository"]);
    $('input[name="prid"]').val(response["prId"]);
    $('input[name="commitid"]').val(response["commitId"]);
    $('input[name="sincecommitid"]').val(response["sinceCommitId"]);

    let changes = response["changes"];

    $.each(changes, function (index, value) {
        var status = "";

        if (value.type == "MODIFY") {
            status = '<span class="filelist-modified"><span class="filelist-text" style="max-width: 200px;">MODIFIED</span></span>';
        }

        if (value.type == "ADD") {
            status = '<span class="filelist-added"><span class="filelist-text" style="max-width: 200px;">ADDED</span></span>';
        }

        if (value.type == "DELETE") {
            status = '<span class="filelist-deleted"><span class="filelist-text" style="max-width: 200px;">DELETED</span></span>';
        }

        var li = $('<li class="list-group-item"><input class="form-check-input me-1" type="checkbox" name="' + value.path + '" id="' + value.path + '" context="' + value.type + '" checked/>' +
            '<label for="' + value.path + '"></label>' + status + '</li>');
        li.find('label').text(value.path);
        $('#filelist').append(li);
    });

    $(".xtrtools-prcheckbox").show();
}

function SetErrorMessage(message) {
    $(".alert").show();
    $("#error-message").text(message)
}