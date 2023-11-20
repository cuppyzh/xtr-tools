// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $("#cover-spin").hide();
});

$(document).ajaxStart(function () {
    console.log("ajaxStart");
    $("#cover-spin").show();
});

$(document).ajaxStop(function () {
    console.log("ajaxStop");
    $("#cover-spin").hide();
});