$(document).ready(function () {
    $('#btnRun').css("display", "none");
    $('#Submit').css("display", "none");
    $("#Development").prop("checked", true);
    $('select>option:eq(3)').attr('selected', true);
    $("#ThrottleNum").val("2");
    $("#ThrottleDuration").val("1");

    $('#file1').change(function () {
        if ($(this).val().length > 0 && document.getElementById("UserID").options.length >1) {
            $('#UploadBtn').prop("disabled", false);
        }
        else {
            $('#UploadBtn').prop("disabled", true);

        }
    });
});