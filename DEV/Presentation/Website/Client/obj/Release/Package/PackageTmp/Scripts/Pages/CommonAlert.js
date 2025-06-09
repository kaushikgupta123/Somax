//#region Alert
var SuccessAlertSetting = {
    title: getResourceValue("SaveAlertSuccess"),
    text: "",
    type: "success",
    showCancelButton: false,
    confirmButtonClass: "btn-sm btn-success",
    cancelButtonClass: "btn-sm",
    confirmButtonText: getResourceValue("SaveAlertOk"),
    cancelButtonText: getResourceValue("CancelAlertNo")
};
var CancelAlertSettingForCallback = {
    title: getResourceValue("CancelAlertSure"),
    text: getResourceValue("CancelAlertLostMsg"),
    type: "warning",
    showCancelButton: true,
    closeOnConfirm: false,
    confirmButtonClass: "btn-sm btn-primary",
    cancelButtonClass: "btn-sm",
    confirmButtonText: getResourceValue("CancelAlertYes"),
    cancelButtonText: getResourceValue("CancelAlertNo")
};
var CancelAlertSetting = {
    title: getResourceValue("CancelAlertSure"),
    text: getResourceValue("CancelAlertLostMsg"),
    type: "warning",
    showCancelButton: true,
    confirmButtonClass: "btn-sm btn-primary",
    cancelButtonClass: "btn-sm",
    confirmButtonText: getResourceValue("CancelAlertYes"),
    cancelButtonText: getResourceValue("CancelAlertNo")
};
var ErrorAlertSetting = {
    title: getResourceValue("CommonErrorAlert"),
    text: "",
    type: "error",
    showCancelButton: false,
    confirmButtonClass: "btn-sm btn-danger",
    cancelButtonClass: "btn-sm",
    confirmButtonText: getResourceValue("SaveAlertOk"),
    cancelButtonText: getResourceValue("CancelAlertNo")
};
var HtmlAlertSettings = {
    title: getResourceValue("CommonErrorAlert"),
    text: '',
    html: true,
    type: 'error',
    showCancelButton: false,
    confirmButtonClass: "btn-sm btn-danger",
    cancelButtonClass: "btn-sm",
    confirmButtonText: getResourceValue("SaveAlertOk"),
    cancelButtonText: getResourceValue("CancelAlertNo")
};
function ShowGenericErrorOnAddUpdate(errorObject, thisformId) {
    var errorMessageContainer;
    var errorString = "";
    if (!thisformId) {
        errorMessageContainer = $(document).find('.errormessage');
    }
    else {
        errorMessageContainer = $(thisformId).find('.errormessage');
    }
    if (typeof errorObject !== "string") {
        $.each(errorObject, function (index, item) {
            errorString = errorString + '<div class="m-alert m-alert--icon m-alert--icon-solid m-alert--outline alert alert-danger alert-dismissible fade show" role="alert"><div class="m-alert__icon"><i class="flaticon-danger"></i></div>' +
                '<div class="m-alert__text">' + item + '</div><div class="m-alert__close">' +
                '<button type="button" class="close" data-dismiss="alert" aria-label="Close"></button></div></div>';
        });
    }
    else {
        errorString = errorString + '<div class="m-alert m-alert--icon m-alert--icon-solid m-alert--outline alert alert-danger alert-dismissible fade show" role="alert"><div class="m-alert__icon"><i class="flaticon-danger"></i></div>' +
            '<div class="m-alert__text">' + errorObject + '</div><div class="m-alert__close">' +
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close"></button></div></div>';
    }
    errorMessageContainer.html(errorString).show();
    window.scrollTo(0, 0);
}
function ResetErrorDiv(element) {
    var errorMessageContainer;
    if (!element) {
        errorMessageContainer = $(document).find('.errormessage');
    }
    else {
        errorMessageContainer = element.siblings('.errormessage');
    }
    errorMessageContainer.html('').hide();
}
function ShowImageSaveSuccessAlert() {
    var msgText = getResourceValue("ImageUploadAlert");
    ShowSuccessAlert(msgText);
}
function ShowImageDeleteSuccessAlert() {
    var msgText = "Image deleted successfully.";
    ShowSuccessAlert(msgText);
}
function ShowImageSizeExceedAlert() {
    swal({
        title: getResourceValue("CommonErrorAlert"),
        text: getResourceValue("ImageFileSizeAlert"),
        type: "error",
        showCancelButton: false,
        confirmButtonClass: "btn-sm btn-danger",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("SaveAlertOk"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () { });
}
function ShowTextMissingCommonAlert(message) {
    swal({
        title: getResourceValue("CommonErrorAlert"),
        text: message,
        type: "error",
        showCancelButton: false,
        confirmButtonClass: "btn-sm btn-danger",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("SaveAlertOk"),
        cancelButtonText: getResourceValue("CancelAlertNo")

    }, function () { });
    return false;
}
function GenericSweetAlertMethod(ErrorMsg) {
    var htmltext = "<ul style='list-style: none;padding: 0;margin: 0;text-align:left;'>";
    if (typeof ErrorMsg !== "string") {
        $.each(ErrorMsg, function (index, value) {
            htmltext = htmltext + "<li style='color: #d43f3a;'><i class='fa fa-circle bull'></i>" + value + "</li>";
        });
    }
    else {
        var thiserror = "<li style='color: #d43f3a;'><i class='fa fa-circle bull'></i>" + ErrorMsg + "</li>";
        htmltext = htmltext + thiserror;
    }
    htmltext = htmltext + "</ul>";
    HtmlAlertSettings.text = htmltext;
    swal(HtmlAlertSettings);
}
function ShowDeleteAlert(message) {
    ShowSuccessAlert(message);
}
function ShowGridItemSelectionAlert() {
    swal({
        title: getResourceValue("CommonErrorAlert"),
        text: getResourceValue("PleaseSelectRecordAlert"),
        type: "error",
        showCancelButton: false,
        confirmButtonClass: "btn-sm btn-danger",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("SaveAlertOk"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () { });
}
function ShowSuccessAlert(msgText) {
    SuccessAlertSetting.text = msgText;
    swal(SuccessAlertSetting, function () { });
};
function ShowErrorAlert(msgText) {
    ErrorAlertSetting.text = msgText;
    swal(ErrorAlertSetting, function () { });
};
function ResetModal(modalid) {
    $(modalid).find('select').val('').trigger('change.select2').removeClass('input-validation-error');
    $(modalid).find('input[type=text]').val('').removeClass('input-validation-error');
    $(modalid).find('.errormessage').html('').hide();
};
function hidemodal(modal) {
    modal.modal('hide');
}
function showmodal(modal) {
    modal.modal('show');
}
//#region V2-716
var AddImageAlertSetting = {
    title: getResourceValue("CancelAlertSure"),
    text: getResourceValue("AddImageAlertUploadMsg"),
    type: "warning",
    showCancelButton: true,
    confirmButtonClass: "btn-sm btn-primary",
    cancelButtonClass: "btn-sm",
    confirmButtonText: getResourceValue("CancelAlertYes"),
    cancelButtonText: getResourceValue("CancelAlertNo")
};
function ShowImageSetSuccessAlert() {
    var msgText = getResourceValue("ImageSetAlert");
    ShowSuccessAlert(msgText);
}
//#endregion
//#region  V2-1010
var WarningAlertSetting = {
    title: "Warning",
    text: "",
    type: "warning",
    showCancelButton: false,
    confirmButtonClass: "btn-sm btn-primary",
    cancelButtonClass: "btn-sm",
    confirmButtonText: getResourceValue("SaveAlertOk")
};
function ShowWarningAlert(msgText) {
    WarningAlertSetting.text = msgText;
    swal(WarningAlertSetting, function () { });
};
function ValidatePast30DaysDateFromDatePicker(seletedDate, msgText) {
    var today = new Date();
    var Targetdate = new Date(new Date().setDate(today.getDate() - 30));
    if (Date.parse(seletedDate) < Date.parse(Targetdate)) {
        ShowWarningAlert(msgText);
    }
}
//#endregion
