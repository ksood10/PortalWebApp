﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $("#Development").prop("checked", true);
    $('select>option:eq(3)').attr('selected', true);
    $("#ThrottleNum").val("2");
    $("#ThrottleDuration").val("1");

    $('#file1').change(function () {
        if ($(this).val().length > 0)
            $('#Submit').prop("disabled", false);
        else
            $('#Submit').prop("disabled", true);
    });

});