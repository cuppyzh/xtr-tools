// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    console.log("Test Javascript");

    $("#cover-spin").hide();

    $('#xtrtools-form').submit(function (event) {
        console.log("Test Click");

        $(".xtrtools-prcheckbox").hide();
        $("#cover-spin").show();

        event.preventDefault();

        $.ajax({
            type: 'POST',
            url: '/api/v1/pr-changes/get',
            data: $(this).serialize(),
            success: function (response) {
                PopulateFileList(response);
                $("#cover-spin").hide();
            },
            error: function () {
                console.log("Call failed");
            }
        });
    });

    $('#xtrtools-form-filelist').submit(function (event) {
        console.log("Test PR Checkbox button Click");

        //$("#cover-spin").show();

        event.preventDefault();

        //$.ajax({
        //    type: 'POST',
        //    url: '/api/v1/pr-changes/get',
        //    data: $(this).serialize(),
        //    success: function (response) {
        //        PopulateFileList(response);
        //        $("#cover-spin").hide();
        //    },
        //    error: function () {
        //        console.log("Call failed");
        //    }
        //});
    });
});

function PopulateFileList(response) {
    console.log(response["projectName"]);
    $('input[name="projectname"]').val(response["projectName"]);
    $('input[name="repositoryname"]').val(response["projectRepository"]);
    $('input[name="prid"]').val(response["prId"]);
    $('input[name="commitid"]').val(response["commitId"]);
    $('input[name="sincecommitid"]').val(response["sinceCommitId"]);

    let changes = response["changes"];

    $.each(changes, function (index, value) {
        var li = $('<li class="list-group-item"><input class="form-check-input me-1" type="checkbox" name="' + value.path + '" id="' + value.path + '" checked/>' +
            '<label for="' + value.path + '"></label></li>');
        li.find('label').text(value.path);
        $('#filelist').append(li);
    });



    $(".xtrtools-prcheckbox").show();
}

//let form = document.querySelector("#xtrtools-form");

//form.addEventListener("submit", function (event) {
//    event.preventDefault();

//    console.log("Test Click");

//    fetch(form.action, {
//        method: "post",
//        body: new URLSearchParams(new FormData(form)) // for application/x-www-form-urlencoded
//        // body: new FormData(form) // for multipart/form-data
//    });
//});
