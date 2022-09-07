$(document).ready(function () {
    $('#btnRun').css("display", "none");
    $("#Development").prop("checked", true);
    $('select>option:eq(3)').attr('selected', true);
    $("#ThrottleNum").val("2");
    $("#ThrottleDuration").val("1");

    $('#file1').change(function () {
        if ($(this).val().length > 0) {
            $('#Submit').prop("disabled", false);
            $('#UploadBtn').prop("disabled", false);
        }
        else {
            $('#Submit').prop("disabled", true);
            $('#UploadBtn').prop("disabled", true);

        }
    });
});