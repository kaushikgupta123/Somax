//#region Loader
function ShowLoader() {
    mcxDialog.loading({ src: "../content/Images" });
}
function CloseLoader() {
    mcxDialog.closeLoading();
}
function onLoginBegin() {
    ShowLoader();
}
function onLoginFailure() {
    CloseLoader();
}
function AjaxBeginFormBegin() {
    ShowLoader();
}
function AjaxBeginFormComplete() {
    CloseLoader();
}
function AjaxBeginFormFaillure() {
    CloseLoader();
}
function ShowbtnLoader(btnid) {
    $('#' + btnid).addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
}
function HidebtnLoader(btnid) {
    $('#' + btnid).removeClass('m-loader m-loader--right m-loader--light').removeAttr('disabled');
}
function ShowbtnLoaderclass(btnid) {
    $('.newBtn-add').attr('disabled', true);
    $('.' + btnid).addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
}
function HidebtnLoaderclass(btnid) {
    $('.newBtn-add').removeAttr('disabled');
    $('.' + btnid).removeClass('m-loader m-loader--right m-loader--light').removeAttr('disabled');
}
//#endregion