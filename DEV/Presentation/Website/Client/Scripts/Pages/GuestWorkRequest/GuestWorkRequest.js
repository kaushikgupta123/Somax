$(document).ready(function () {
    LoadNewCaptcha();
    SetControls();
});
function SetControls() {
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var validstate = "";
        try {
            validstate = $(this).valid();
        } catch (e) {
            validstate = undefined;
        }
        var areaddescribedby = $(this).attr('aria-describedby');
        if (areaddescribedby) {
            if (validstate) {
                if (typeof areaddescribedby != 'undefined') {
                    $('#' + areaddescribedby).hide();
                }
            }
            else {
                if (typeof areaddescribedby != 'undefined') {
                    $('#' + areaddescribedby).show();
                }
            }
        }
    });
    $(document).find('.select2picker').select2({});
}
function WorkRequestAddOnSuccess(data) {
    if (data.data === "success") {
        if (data.Command === "save") {
            SuccessAlertSetting.text = getResourceValue("spnWoRequestAddSuccessfully");
            swal(SuccessAlertSetting, function () {
                ResetErrorDiv();
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change.select2');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else if (data === "failed") {
        var msgtext = getResourceValue("InvalidCaptchaAlert");
        ShowErrorAlert(msgtext);
    }
    else {
        CloseLoader();
        ShowGenericErrorOnAddUpdate(data);
    }
    LoadNewCaptcha();
}
function LoadNewCaptcha() {
    $.ajax({
        url: '/GuestWorkRequest/LoadCaptcha',
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#captchaRegion').html(data);
        },
        complete: function () {
            CloseLoader();
        },
        error: function (jqXHR, exception) {
        }
    });
}
$(document).on('click', '#btnCancelGuestWorkRequest', function () {
    ResetErrorDiv();
    $(document).find('form').trigger("reset");
    $(document).find('form').find("select").val("").trigger('change.select2');
    $(document).find('form').find("select").removeClass("input-validation-error");
    $(document).find('form').find("textarea").removeClass("input-validation-error");
});