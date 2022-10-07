$(document).ready(function () {
    $("#Development").prop("checked", true);
    $("#ThrottleNum").val("2");
    $("#ThrottleDuration").val("1");

    $("#ThrottleNum").change(function () {
        if ($('#file1').val().length > 0 && $("#ThrottleNum").val() >= 0 && $("#ThrottleNum").val() <= 100 && $("#ThrottleDuration").val() >= 0 && $("#ThrottleDuration").val() <= 100)
            $('#ImportExcel').prop("disabled", false);
        else
            $('#ImportExcel').prop("disabled", true);
    });
    $("#ThrottleDuration").change(function () {
        if ($('#file1').val().length > 0 && $("#ThrottleNum").val() >= 0 && $("#ThrottleNum").val() <= 100 && $("#ThrottleDuration").val() >= 0 && $("#ThrottleDuration").val() <= 100)
            $('#ImportExcel').prop("disabled", false);
        else
            $('#ImportExcel').prop("disabled", true);
    });
    $('#file1').change(function () {
        if ($(this).val().length > 0 && $("#ThrottleNum").val() >= 0 && $("#ThrottleNum").val() <= 100 && $("#ThrottleDuration").val() >= 0 && $("#ThrottleDuration").val() <=100  ) 
            $('#ImportExcel').prop("disabled", false);
        else 
            $('#ImportExcel').prop("disabled", true);
    });

});