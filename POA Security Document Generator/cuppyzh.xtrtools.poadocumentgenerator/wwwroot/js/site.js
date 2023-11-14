// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ajaxStart(function () {
    console.log("ajaxStart");
    $("#cover-spin").show();
});

$(document).ajaxStop(function () {
    console.log("ajaxStop");
    $("#cover-spin").hide();
});
$(document).ready(function () {
    console.log("Test Javascript");

    $("#cover-spin").hide();


    $('#xtrtools-form').submit(function (event) {
        console.log("Test Click");

        $("#cover-spin").show();
        $(".xtrtools-prcheckbox").hide();

        event.preventDefault();

        $.ajax({
            type: 'POST',
            url: '/api/v1/pr-changes/get',
            data: $(this).serialize(),
            beforeSend: function (xhr) {
                $("#cover-spin").show();
            },
            success: function (response) {
                PopulateFileList(response);
                $("#cover-spin").show();
            },
            error: function () {
                console.log("Call failed");
            }
        });
    });

    $('#xtrtools-form-filelist').submit(function (event) {
        console.log("Test PR Checkbox button Click");

        event.preventDefault();

        const checkboxInput = new Array();

        $("#filelist .form-check-input:checked").each(function () {
            checkboxInput.push(this.name);
        });

        var request = {
            "ProjectName": $('input[name="projectname"]').val(),
            "ProjectRepository": $('input[name="repositoryname"]').val(),
            "PRId": $('input[name="prid"]').val(),
            "CommitId": $('input[name="commitid"]').val(),
            "Files": checkboxInput
        };

        $.ajax({
            type: 'POST',
            url: '/api/v1/pr-changes/export',
            data: JSON.stringify(request),
            contentType: "application/json; charset=utf-8",
            responseType: 'blob',
            //dataType: "json",
            success: function (response, status, xhr) {
                var filename = GetFileNameFromXhr(xhr);

                var blob = response;
                if (window.navigator.msSaveOrOpenBlob) {
                    window.navigator.msSaveBlob(blob, filename);
                }
                else {
                    var downloadLink = window.document.createElement('a');
                    downloadLink.href = window.URL.createObjectURL(new Blob([blob], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }));
                    downloadLink.download = filename;
                    document.body.appendChild(downloadLink);
                    downloadLink.click();
                    document.body.removeChild(downloadLink);
                }

            },
            error: function () {
                console.log("Call failed");
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

        var li = $('<li class="list-group-item"><input class="form-check-input me-1" type="checkbox" name="' + value.path + '" id="' + value.path + '" checked/>' +
            '<label for="' + value.path + '"></label>' + status + '</li>');
        li.find('label').text(value.path);
        $('#filelist').append(li);
    });

    $(".xtrtools-prcheckbox").show();
}