//#region Set Fixed Head Style
function SetFixedHeadStyle() {
    if ($(document).find('.m-content').hasClass('openMenu')) {
        if ($(document).find('.fixedDiv1').hasClass('fixupperpannel2')) {
            $(document).find('.fixedDiv1').removeClass('fixupperpannel2');
        }
        if ($(document).find('.fixedDiv2').hasClass('fixupperpannel')) {
            $(document).find('.fixedDiv2').removeClass('fixupperpannel');
        }
    }
    else if ($('.m-content').hasClass('closeMenu')) {
        if (!$(document).find('.fixedDiv1').hasClass('fixupperpannel2')) {
            $(document).find('.fixedDiv1').addClass('fixupperpannel2');
        }
        if (!$(document).find('.fixedDiv2').hasClass('fixupperpannel')) {
            $(document).find('.fixedDiv2').addClass('fixupperpannel');
        }
    }
}
//#endregion


//#region Menu
if ($(window).width() < 1024) {
    $('.m-content').removeClass('openMenu');
    $('.m-content').removeClass('closeMenu');
    $(document).find('.fixedDiv1').removeClass('fixupperpannel2');
    $(document).find('.fixedDiv2').removeClass('fixupperpannel');
}
function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}
$(function () {
    //#region Menu-Open
    var page = getUrlVars()["page"];
    var $currentli = $(document).find('#' + page);
    $currentli.addClass('m-menu__item--active');
    $currentli.parents('li.rootli').addClass('m-menu__item--open');
    $currentli.parents('div').show();
    //#endregion

    //#region Menu-Toggle
    $('#m_aside_left_minimize_toggle').click(function () {
        var prevMenuOpenState = localStorage.getItem("USERMENUOPENCLOSESTATE");
        if ($(document).find('.m-content').hasClass('openMenu')) {
            SetMenuBarOpenCloseState("close");
            $(document).find('.SmallLogo').show();
            $(document).find('.m-content').removeClass('openMenu');
            $(document).find('.m-content').addClass('closeMenu');
            if (!$(document).find('.fixedDiv1').hasClass('fixupperpannel2')) {
                $(document).find('.fixedDiv1').addClass('fixupperpannel2');
            }
            if (!$(document).find('.fixedDiv2').hasClass('fixupperpannel')) {
                $(document).find('.fixedDiv2').addClass('fixupperpannel');
            }
            $($.fn.dataTable.tables(true)).DataTable().columns.adjust().fixedColumns().relayout();
            $(document).find('#searcharea').css({ 'left': '81px' });
        }
        else if ($('.m-content').hasClass('closeMenu')) {
            SetMenuBarOpenCloseState("open");
            $(document).find('.SmallLogo').hide();
            $(document).find('.m-content').removeClass('closeMenu');
            $(document).find('.m-content').addClass('openMenu');


            if ($(document).find('.fixedDiv1').hasClass('fixupperpannel2')) {
                $(document).find('.fixedDiv1').removeClass('fixupperpannel2');
            }
            if ($(document).find('.fixedDiv2').hasClass('fixupperpannel')) {
                $(document).find('.fixedDiv2').removeClass('fixupperpannel');
            }
            $(document).find('#searcharea').css({ 'left': '256px' });
        }

    });
    //#endregion
});
//#endregion
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
//#region Scroll
function ScrollToClass(Class) {
    $('html, body').animate({
        scrollTop: $("." + Class).offset().top
    }, 2000);
}
function ScrollToId(Id) {
    $('html, body').animate({
        scrollTop: $("#" + Id).offset().top
    }, 2000);
}
//#endregion
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
//#endregion

//#region validation
$.validator.setDefaults({ ignore: null });
function ValidateEmail(email) {
    var expr = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    return expr.test(email);
};
function ValidateDate(date) {
    if (date) {
        var splitteddate = date.split('/');
        var month = $.isNumeric(splitteddate[0]);
        if (!month) { return ""; }
        var day = $.isNumeric(splitteddate[1]);
        if (!day) { return ""; }
        var year = $.isNumeric(splitteddate[2]);
        if (!year) { return ""; }
        return date;
    }
    else {
        return "";
    }
}
//#endregion

function LRTrim(value) {
    if (value) {
        return value.trim();
    }
    else {
        return "";
    }
}

//#region Datatable
function SetPageLengthMenu() {
    var pagelengthmenu = $(document).find('.searchdt-menu');
    pagelengthmenu.select2({
        minimumResultsForSearch: -1
    });
    $(document).find('.dataTables_length').show();
}
function GoToFirstPage(dtTable) {
    dtTable.page('first').draw('page');
}
$(document).on({
    mouseenter: function () {
        if ($(this).parents('table').hasClass('haschild')) {
            return;
        }

        if ($(this).parents('tbody').siblings('tfoot').length > 0) {
            trIndex = $(this).index() + 2;
        }
        else {
            trIndex = $(this).index() + 1;
        }
        $("table.dataTable").each(function (index) {
            $(this).find("tr:eq(" + trIndex + ")").addClass("tbl-row-hover");
        });
    },
    mouseleave: function () {
        if ($(this).parents('table').hasClass('haschild')) {
            return;
        }
        if ($(this).parents('tbody').siblings('tfoot').length > 0) {
            trIndex = $(this).index() + 2;
        }
        else {
            trIndex = $(this).index() + 1;
        }
        $("table.dataTable").each(function (index) {
            $(this).find("tr:eq(" + trIndex + ")").removeClass("tbl-row-hover");
        });
    }
}, ".dataTables_wrapper tr");
$(function () {
    $(document).find('.select2-not-search').select2({
        minimumResultsForSearch: -1
    });
    if ($.fn.dataTable) {
        $.fn.dataTable.ext.errMode = 'none';
    }
});
function DisableExportButton(dataTbl, btnExort) {
    if (dataTbl.dataTable().fnGetData().length < 1)
        btnExort.prop('disabled', true);
    else
        btnExort.prop('disabled', false);
}
//#endregion
//#region Export Button Show Hide
function funcShowExportbtn() {
    $(document).find("#popupExport").show();
    $(document).find("#mask").show();
}
function funcCloseExportbtn() {
    $(document).find("#popupExport").hide();
    $(document).find("#mask").hide();
}
//#endregion


//#region liCustomize1
var selectedCol = [];
var nselectedCol = [];
var oCols = [];
function funCustomizeBtnClick(dtTable, customsort, titleArray) {
    selectedCol = [];
    nselectedCol = [];
    oCols = [];
    colOrder = [0, 1];
    $('#StaffList').multiselect('destroy');
    var vCols = [];
    var mCols = [];
    $('#StaffList option').each(function () { $(this).remove(); });
    $('#PresenterList li').remove();
    $.each(dtTable.settings()[0].aoColumns, function (c) {
        var KeyValuePair = {};
        if (customsort) {
            KeyValuePair = {
                Id: dtTable.colReorder.order()[c],
                Idx: dtTable.settings()[0].aoColumns[c].idx,
                Value: dtTable.settings()[0].aoColumns[c].sTitle,
                Selected: dtTable.settings()[0].aoColumns[c].bVisible,
                Order: c
            };
        }
        else {
            KeyValuePair = {
                Id: dtTable.colReorder.order()[c],
                Idx: dtTable.settings()[0].aoColumns[c].idx,
                Value: dtTable.settings()[0].aoColumns[c].sTitle,
                Disabled: dtTable.settings()[0].aoColumns[c].bSortable,
                Selected: dtTable.settings()[0].aoColumns[c].bVisible,
                Order: c
            };
        }
        ////------------for uiconfig------------------------

        if (titleArray != null && titleArray.length > 0) {
            if (!(titleArray.includes(KeyValuePair.Value))) {
                oCols.push(KeyValuePair);
                if (!($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
                    vCols.push(KeyValuePair);
                }
                if (dtTable.settings()[0].aoColumns[c].bVisible == true && !($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
                    mCols.push(dtTable.colReorder.order()[c]);
                }
            }
        }
        else {
            oCols.push(KeyValuePair);
            if (!($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
                vCols.push(KeyValuePair);
            }
            if (dtTable.settings()[0].aoColumns[c].bVisible == true && !($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
                mCols.push(dtTable.colReorder.order()[c]);
            }
        }
        ////------------------------------------
        //oCols.push(KeyValuePair);
        //if (!($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
        //    vCols.push(KeyValuePair);
        //}
        //if (dtTable.settings()[0].aoColumns[c].bVisible == true && !($(dtTable.settings()[0].aoColumns[c].nTh).hasClass('noVis'))) {
        //    mCols.push(dtTable.colReorder.order()[c]);
        //}

    });
    vCols = vCols.sort(function (ob1, ob2) {
        return ob1.Id - ob2.Id;
    });
    oCols = oCols.sort(function (ob1, ob2) {
        return ob1.Id - ob2.Id;
    });
    var options = [];
    $.each(vCols, function (i, v) {
        if (customsort) {
            options[i] = {
                label: v.Value,
                title: v.Value,
                value: v.Id,
                selected: v.Selected,

                attributes: {
                    'order': v.Order
                }
            };
        }
        else {
            options[i] = {
                label: v.Value,
                title: v.Value,
                value: v.Id,
                selected: v.Selected,
                disabled: v.Disabled,
                attributes: {
                    'order': v.Order
                }
            };
        }
    });
    $('#StaffList').multiselect({
        buttonContainer: '<div class="col-md-12 btn-group" />',
        templates: {
            button: '<button type="button" style="display:none;" class="multiselect dropdown-toggle" data-toggle="dropdown"><span class="multiselect-selected-text" ></span> <b class="caret"></b></button>',
            ul: '<ul class="multiselect-container dropdown-menu"></ul>',
            filter: '<li class="multiselect-item filter"><div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-search"></i></span><input class="form- control multiselect-search" type="text"></div></li>',
            filterClearBtn: '<span class="input-group-btn"><button class="btn btn-default multiselect-clear-filter" type="button"><i class="glyphicon glyphicon-remove-circle"></i></button></span>',
            li: '<li><a tabindex="0"><label class="m-checkbox m-checkbox--bold m-checkbox--state-success"></label></a></li>',
            divider: '<li class="multiselect-item divider"></li>',
            liGroup: '<li class="multiselect-item multiselect-group"><label></label></li>'
        },
        onChange: function (optiona, checked) {
            if (checked) {
                $('ul').moveToList('#StaffList', '#PresenterList', optiona);
            }
            else {
                $('#PresenterList li').each(function () {
                    if ($(this).data('val').toString() === $(optiona).val())
                        $(this).remove();
                });
            }
            selectedCol = [];
            nselectedCol = [];
            $('#StaffList option').each(function () {
                if ($(this).is(":selected"))
                    selectedCol.push({
                        Id: parseInt($(this).val()),
                        Name: $(this).text()
                    });
                else
                    nselectedCol.push({
                        Id: parseInt($(this).val()),
                        Name: $(this).text()
                    });
            });
            $(document).find('#lblCounter').text(selectedCol.length > 0 ? selectedCol.length : "None");
        }
    });

    $('#StaffList').multiselect("dataprovider", options, true);
    $('#StaffList').parent().find('ul li label').each(function () {
        $(this).append("<span></span>");
    });

    $.each(mCols, function (x, val) {
        $('#StaffList').multiselect('select', val, true);
    });
}

$(document).on('click', '#PresenterList li:not(.disabled)', function () {
    $('#PresenterList li').removeClass('activeCol');
    $(this).addClass('activeCol');
});
$('#btnRemoveAvenger').click(function (e) {
    var opts = $('#PresenterList li.activeCol');
    if (opts.length > 0)
        $('#StaffList').multiselect('deselect', $(opts).data('val').toString(), true);
    e.preventDefault();
});
$('#btnAvengerUp').click(function (e) {
    $('ul').moveUpDown('#PresenterList', true, false);
    e.preventDefault();
});
$('#btnAvengerDown').click(function (e) {
    $('ul').moveUpDown('#PresenterList', false, true);
    e.preventDefault();
});
function funCustozeSaveBtn(dtTable, colOrder) {
    //Maintaing colspan when columns gets added or removed in no data available state in grid
    $('.dataTables_empty').attr('colspan', '100%');
    $("#PresenterList li").each(function () {
        var name = $(this).find('span').text();
        $.each(oCols, function (k, l) {
            if (name === l.Value) {
                colOrder.push(l.Id);
                //dtTable.columns(l.Id).visible(true);
                dtTable.columns(l.Idx).visible(true);
            }
        });
    });
    $.each(nselectedCol, function (o, g) {
        $.each(oCols, function (k, l) {
            if (g.Name === l.Value) {
                colOrder.push(l.Id);
                console.log(dtTable.settings()[0].aoColumns[l.Idx].sTitle);
                dtTable.columns(l.Idx).visible(false);
            }
        });
    });
    dtTable.colReorder.reset();
    dtTable.colReorder.order(colOrder);
}
//#endregion

//#region UserProfile
function ToogleUserProfileTab(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("profile-tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("profile-tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}
$('#showUserProfile').on('click', function () {
    $.ajax({
        url: '/Admin/UserProfile/GetUserProfile',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        beforeSend: function () {
            ShowLoader();
        },
        type: "GET",
        success: function (data) {
            ResetErrorDiv($(document).find('#frmuserprofile'));
            $('#profileModal').modal('show');
            $(document).find('#UserProfileFirstName').val(data.UserProfileFirstName);
            $(document).find('#UserProfileMiddleName').val(data.UserProfileMiddleName);
            $(document).find('#UserProfileLastName').val(data.UserProfileLastName);
            $(document).find('#UserProfileEmailAddress').val(data.UserProfileEmailAddress);
            $(document).find('#UserProfileSiteName').val(data.UserProfileSiteName);
            $(document).find('#UserProfileSecurityQuestion').val(data.UserProfileSecurityQuestion);
            $(document).find('#UserProfileSecurityAnswer').val(data.UserProfileSecurityAnswer);
            $(document).find('#UserInfoUpdateIndex').val(data.UserInfoUpdateIndex);
            $(document).find('#LoginInfoUpdateIndex').val(data.LoginInfoUpdateIndex);
        },
        complete: function () {
            $.validator.unobtrusive.parse(document);
            $('input, form').blur(function () {
                $(this).valid();
            });
            CloseLoader();
        }
    });
});
function UpdateUserProfileOnSuccess(data) {
    if (data.Result == "success") {
        $('#profileModal').modal('hide');
        //var msgText = "Profile successfully updated.";
        var msgText = getResourceValue('ProfileUpdate');
        $('#layoutusercompletename').text(data.userFirstName + " " + data.userLastName);
        $('.nameinitials').text(data.userFirstName[0] + data.userLastName[0]);
        ShowSuccessAlert(msgText);
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#profileModal');
    }
}
$(document).on('click', "#btnUpdateUserProfile", function () {
    if ($(document).find("#frmuserprofile").valid()) {
        return;
    }
    else {
        var errorTab = $(document).find('#frmuserprofile').find(".input-validation-error").closest('.profile-tabcontent').attr('id');
        if (errorTab === 'EditLogin') {
            $(document).find('#Editlogintabbutton').trigger('click');
        }
        else {
            $(document).find('#UpdatePasswordtabbutton').trigger('click');
        }
    }
});
//#endregion

//#region System Unavailable Message
function GetMaintenanceMessage() {
    $.ajax({
        url: "/Admin/Base/GetSiteMaintenanceMessage",
        type: "GET",
        datatype: "json",
        success: function (data) {
            if (data != '') {
                $.notify({
                    // options
                    icon: 'glyphicon glyphicon-warning-sign',
                    //title: 'System Under Maintenance',
                    title: '',
                    message: data,
                    url: '#'
                   /*url: '/SiteMaintenance/UnderConstruction'*/,
                    target: '_blank'
                }, {
                    // settings
                    element: 'body',
                    position: null,
                    //type: "info",
                    type: "danger",
                    allow_dismiss: true,
                    newest_on_top: false,
                    showProgressbar: false,
                    placement: {
                        from: "top",
                        align: "right"
                    },
                    offset: 20,
                    spacing: 10,
                    z_index: 1031,
                    delay: 5000,
                    timer: 5000,
                    url_target: '_blank',
                    mouse_over: null,
                    animate: {
                        enter: 'animated fadeInDown',
                        exit: 'animated fadeOutUp'
                    },
                    onShow: null,
                    onShown: null,
                    onClose: null,
                    onClosed: null,
                    icon_type: 'class',
                    template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                        '<button type="button" aria-hidden="true" class="close" data-notify="dismiss"></button>' +
                        '<span data-notify="icon"></span> ' +
                        '<span data-notify="title">{1}</span> ' +
                        '<span data-notify="message"><i class="fa fa-exclamation-circle" style="font-size:13px"></i>&nbsp; {2}</span>' +
                        '<div class="progress" data-notify="progressbar">' +
                        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                        '</div>' +
                        '<a href="{3}" target="{4}" data-notify="url"></a>' +
                        '</div>'

                });
            }

        },
        complete: function () {

        }
    });
}

$(function () {
    GetMaintenanceMessage();
});
$(document).on('keydown keyup', '.decimalinput', function (el) {
    var ex = /^[0-9]*\.?[0-9]*$/;
    if (ex.test(el.target.value) === false) {
        el.target.value = el.target.value.substring(0, el.target.value.length - 1);
    }
});

$(document).on('keypress keyup', '.integerinput', function (el) {
    var ex = /^[0-9]*$/;
    if (ex.test(el.target.value) === false) {
        el.target.value = el.target.value.substring(0, el.target.value.length - 1);
    }
});
//#endregion

//#region Menu-State
function SetMenuBarOpenCloseState(state) {
    $.ajax({
        url: '/Admin/Base/SetMenuOpenState',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: {
            state: state
        },
        beforeSend: function () {
            $('#procnotificationbody').html('');
            $('#notificationloader').show();
        },
        type: "GET",
        success: function (data) { },
        complete: function () {
        },
        error: function (xhr) {
        }
    });
}
//#endregion
