// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('Environment').prop("checked", true);
    //$('#btnRun').click(function () {

    //    if ($('#file1').val() == "") {
    //        $('#btnRun').prop("disabled", true);
    //        return false;
    //    }
    //    else if ($('#file').val() != "") {
    //        $('#btnRun').prop("disabled", false);
    //        return true;
    //    }
    //});
    $("input[name='Environment'][value='Development']").prop("checked", true);
    $('select>option:eq(3)').attr('selected', true);
    $("#ThrottleNum").val("2");
    $("#ThrottleDuration").val("1");
    $('#file1').change(function () {
        if ($(this).val().length > 0)
            $('#Submit').prop("disabled", false);
        else
            $('#Submit').prop("disabled", true);
    });


    $("#success-alert").hide();
    $("#myWish").click(function showAlert() {
        $("#success-alert").fadeTo(2000, 500).slideUp(500, function () {
            $("#success-alert").slideUp(500);
        });
    });


});