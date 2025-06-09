//TemporaryPassword,NewPassword,ConfirmPassword,SecurityResponse - Mandatory

var SuccessAlertSetting = {
    title: 'Success',
    text: "",
    type: "success",
    showCancelButton: false,
    closeOnConfirm: false,
    confirmButtonClass: "btn-sm btn-success",
    cancelButtonClass: "btn-sm",
    confirmButtonText: 'Ok'
};
function ResetPasswordOnBegin() {
    $('#error_Message').text('');
    $('#reset_Password').addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
}
function ResetPasswordOnSuccess(data) {
    var btn = $('#reset_Password');
    if (data.result)
    {
        SuccessAlertSetting.text = data.message;
        swal(SuccessAlertSetting, function () {
            window.location.href = "../LogIn/SomaxLogIn";
        });
        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
    } else {
        $('#error_Message').text(data.message)
        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
    }
}
function ResetPasswordOnFailure()
{
    $('#error_Message').text("An error has occured!!!");
}
$(document).on('click', '#TempPasswordEye', function () {
    ShowHidePassword('TempPassword', 'TempPasswordEye');
});
$(document).on('click', '#NewPasswordEye', function () {
    ShowHidePassword('NewPassword', 'NewPasswordEye');
});
$(document).on('click', '#ConfirmPasswordEye', function () {
    ShowHidePassword('ConfirmPassword', 'ConfirmPasswordEye');
});
