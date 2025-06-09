//#region Create New Password V2-332
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
function CreatePassBeginForm() {
    $('#error_Message').text('');
    $('#error_Message').css('display', 'none');
    $('#Create_Password').addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
}
function CreatePassFormComplete() {
    $('#Create_Password').removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
}
function CreatePassFormFaillure() {
    $('#Create_Password').removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
}
function PasswordCreateOnSuccess(data) {
    if (data.result === 'success') {
        SuccessAlertSetting.text = data.message;
        swal(SuccessAlertSetting, function () {
            window.location.href = "../LogIn/LogOut";
        });
    }
    else {
        $('#error_Message').text(data.message);
        $('#error_Message').css('display', 'block');
    }
}
$('#return_to_signin').click(function () {
    window.location.href = "../LogIn/LogOut";
});
$(document).on('click', '#NewPasswordEye', function () {
    ShowHidePassword('NewPassword', 'NewPasswordEye');
});
$(document).on('click', '#ConfirmPasswordEye', function () {
    ShowHidePassword('ConfirmPassword', 'ConfirmPasswordEye');
});
function ShowHidePassword(PasswordId, EyeId) {
    if ($('#' + EyeId).hasClass('fa-eye-slash')) {
        $('#' + EyeId).removeClass('fa-eye-slash');
        $('#' + EyeId).addClass('fa-eye');
        $('#' + PasswordId).attr('type', 'text');
    } else {
        $('#' + EyeId).removeClass('fa-eye');
        $('#' + EyeId).addClass('fa-eye-slash');
        $('#' + PasswordId).attr('type', 'password');
    }
}
//#endregion