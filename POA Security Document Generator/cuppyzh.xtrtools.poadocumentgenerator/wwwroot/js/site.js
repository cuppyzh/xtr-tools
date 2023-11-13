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
            url: '/api/v1/pr-changes/get',
            data: $(this).serialize(),
            success: function (response) {
                console.log("Call success");
                $('#result').html(response);
                PopulateFileList(response);
            },
            error: function () {
                console.log("Call failed");
            }
        });
    });
});

function PopulateFileList(response) {
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
