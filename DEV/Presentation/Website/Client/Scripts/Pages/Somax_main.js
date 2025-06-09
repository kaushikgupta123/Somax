var _isLoggedInFromMobile = undefined;
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
function CheckLoggedInFromMob() {
    var isMobile = false;
    if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
        || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
        return true;
    }
    return isMobile;
}
$(function () {
    if (_isLoggedInFromMobile == undefined) {
        _isLoggedInFromMobile = CheckLoggedInFromMob();
        if (_isLoggedInFromMobile == true) {
            console.log('mobile / ipad');
        }
        else {
            console.log('desktop');
        }
    }
    GetUnreadNotificationCount("onload");
    if ($('#spnopenevent').length > 0 || $('#spnopenwo').length > 0) {
        GetMenuItemsCount();
    }
    //#region Menu-Open
    var page = getUrlVars()["page"];
    var $currentli = $(document).find('#' + page);
    $currentli.addClass('m-menu__item--active');
    $currentli.parents('li.rootli').addClass('m-menu__item--open');
    $currentli.parents('div').show();
    //#endregion
    //#region common
    $(document).ajaxError(function (event, xhr, options, exc) {
        if (xhr.status === 401) {
            window.location.href = "/";
        }
        if (xhr.status === 999) {
            window.location.href = "/Error/SessionExpired";
        }
        if (xhr.status === 888) {
            window.location.href = "/Error/Index";
        }
        if (xhr.status === 302) {
            swal({
                title: getResourceValue("CommonErrorAlert"),
                text: "Your session has expired. Please log in again to continue.",
                type: "error",
                showCancelButton: false,
                confirmButtonClass: "btn-sm btn-danger",
                cancelButtonClass: "btn-sm",
                confirmButtonText: getResourceValue("SaveAlertOk"),
                cancelButtonText: getResourceValue("CancelAlertNo")

            }, function () {
                document.location.href = '/LogIn/SomaxLogIn';
            });
        }
        mcxDialog.closeLoading();
    });
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
            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            $(document).find('#searcharea').css({ 'left': '256px' });
        }

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

    $(document).on('keyup keydown', '.decimalinputupto2places', function () {

        var num = $(this).attr("maskedFormat").toString().split(',');
        var regex = new RegExp("^\\d{0," + num[0] + "}(\\.\\d{0," + num[1] + "})?$");
        if (!regex.test(this.value)) {
            this.value = this.value.substring(0, this.value.length - 1);
        }
    });
    $(document).on('keyup keydown', '.longinput', function (el) {
        //var ex = /^\d+$/;
        var ex = /^\d{1,19}$/;
        if (ex.test(el.target.value) === false) {
            el.target.value = el.target.value.substring(0, el.target.value.length - 1);
        }
    });

    $(document).on("cut copy paste", '.decimalinput,.longinput,.decimalinputupto2places', function (e) {
        e.preventDefault();
    });
    $(document).find('.dtpicker').datepicker({
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        changeMonth: true,
        changeYear: true
    }).inputmask('mm/dd/yyyy');
    if ($.fn.dataTable) {
        $.fn.dataTable.ext.errMode = 'none';
    }
    $(document).on('keyup', '.textcountmsg', function () {
        if ($(this).hasClass('multilineedit')) {
            var totalCount = $(this).val().length;
            $(this).parent().find('.textcountlabel').text("Total characters count: " + totalCount);
        }
        else if ($(this).data('val-length-max')) {
            var totalCount = $(this).val().length;
            var maxlen = parseInt($(this).data('val-length-max'));
            $(this).parent().find('.textcountlabel').text("Total characters count: " + totalCount + " Characters left: " + (maxlen - totalCount));
        }
    });
    $(document).on('keyup', '.textcountmsg_mobile', function () {
        if ($(this).hasClass('multilineedit')) {
            var totalCount = $(this).val().length;
            $(this).parents('div').eq(0).find('.textcountlabel_mobile').text("Total characters count: " + totalCount);
        }
        else if ($(this).data('val-length-max')) {
            var totalCount = $(this).val().length;
            var maxlen = parseInt($(this).data('val-length-max'));
            $(this).parents('div').eq(0).find('.textcountlabel_mobile')
                .text("Total characters count: " + totalCount + " Characters left: " + (maxlen - totalCount));
        }
    });
    //#region V2-1044
    $(document).on('keyup', '.multilineedit', function (event) {
        if (event.key === 'Enter') {
            let start = this.selectionStart;
            let end = this.selectionEnd;
            let value = $(this).val();
            $(this).val(value.substring(0, start) + '\n' + value.substring(end));
            this.selectionStart = this.selectionEnd = start + 1;
            event.preventDefault();
        }
    });
    //#endregion
    //#endregion

    $(function () {
        function setTotalNotification(notification) {
            if (notification) {
                GetUnreadNotificationCount();
                $('#m_topbar_notification_icon .m-nav__link-icon').addClass('m-animate-shake');
                $('#m_topbar_notification_icon .m-nav__link-badge').addClass('m-animate-blink');
                $('#notification-dot').addClass('m-nav__link-badge m-badge m-badge--dot m-badge--dot-small m-badge--danger m-animate-blink');
            }
        }
        var hub = $.connection.notificationHub;
        hub.client.broadcaastNotif = function (notification) {
            setTotalNotification(notification);
        };
        $.connection.hub.start()
            .done(function () {
                console.log("Connected!");
                hub.server.getNotification();
            })
            .fail(function () {
                console.log("Could not Connect!");
            });

    });
    $.validator.addMethod("date", function (value, element) {
        var ok = true;
        try {
            $.datepicker.parseDate('mm/dd/yy', value);
        }
        catch (err) {
            ok = false;
        }
        return ok;
    });

    //#region custom-drop-down
    var x, i, j, selElmnt, a, b, c;
    x = document.getElementsByClassName("custom-selected");
    for (i = 0; i < x.length; i++) {
        selElmnt = x[i].getElementsByTagName("select")[0];
        a = document.createElement("DIV");
        a.setAttribute("class", "select-selected");
        a.innerHTML = selElmnt.options[selElmnt.selectedIndex].innerHTML;
        x[i].appendChild(a);
        b = document.createElement("DIV");
        b.setAttribute("class", "select-items select-hide");
        for (j = 0; j < selElmnt.length; j++) {
            c = document.createElement("DIV");
            c.innerHTML = selElmnt.options[j].innerHTML;
            c.addEventListener("click", function (e) {
                var y, i, k, s, h;
                s = this.parentNode.parentNode.getElementsByTagName("select")[0];
                h = this.parentNode.previousSibling;
                for (i = 0; i < s.length; i++) {
                    if (s.options[i].innerHTML === this.innerHTML) {
                        s.selectedIndex = i;
                        h.innerHTML = this.innerHTML;
                        y = this.parentNode.getElementsByClassName("same-as-selected");
                        for (k = 0; k < y.length; k++) {
                            y[k].removeAttribute("class");
                        }
                        this.setAttribute("class", "same-as-selected");
                        break;
                    }
                }
                h.click();
            });
            b.appendChild(c);
        }
        x[i].appendChild(b);
        a.addEventListener("click", function (e) {
            e.stopPropagation();
            closeAllSelect(this);
            this.nextSibling.classList.toggle("select-hide");
            this.classList.toggle("select-arrow-active");
        });
    }
    function closeAllSelect(elmnt) {
        var x, y, i, arrNo = [];
        x = document.getElementsByClassName("select-items");
        y = document.getElementsByClassName("select-selected");
        for (i = 0; i < y.length; i++) {
            if (elmnt == y[i]) {
                arrNo.push(i)
            } else {
                y[i].classList.remove("select-arrow-active");
            }
        }
        for (i = 0; i < x.length; i++) {
            if (arrNo.indexOf(i)) {
                x[i].classList.add("select-hide");
            }
        }
    }
    document.addEventListener("click", closeAllSelect);
    //#endregion
    $(document).on('click', '.upArrow', function () {
        $(document).find(".errorList").slideUp();
        $(document).find(".downArrow").show();
        $(document).find(".upArrow").hide();
    });
    $(document).on('click', '.downArrow', function () {
        $(document).find(".errorList").slideDown();
        $(document).find(".downArrow").hide();
        $(document).find(".upArrow").show();
    });
});

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

$(function () {
    $(document).on('change', 'input.attfile[type="file"]', function () {
        if ($(this).val() != "") {
            var areaddescribedby = $(this).attr('aria-describedby');
            if ($(this).valid()) {
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
});
//#region URL-Parameter-Encryption
function EncryptionUrlParams(param) {
    if (param && param.length > 0) {
        var encKey = makeid(16);
        var key = CryptoJS.enc.Utf8.parse(encKey);
        var iv = CryptoJS.enc.Utf8.parse(encKey);
        var Encript_Param = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(param), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });
        key = CryptoJS.enc.Utf8.parse($('#encKeyV').val());
        iv = CryptoJS.enc.Utf8.parse($('#encKeyV').val());
        var Dicript_Key = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(encKey + Encript_Param), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });
        return new ValueSetInObject(Encript_Param, Dicript_Key);
    }
    return null;
}
function makeid(length) {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    for (var i = 0; i < length; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));
    return text;
}
function ValueSetInObject(Encrptparam, Dicriptkey) {
    this.id = Encrptparam;
    this.key = Dicriptkey;
}
//#endregion
//#region Formatting
function FormatJavascriptDate(value) {
    if (value === null) return "";
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    var date = dt.getDate();
    var month = dt.getMonth() + 1;
    var year = dt.getFullYear();
    if (year < 1900) {
        return "";
    }
    if (date < 10) {
        date = "0" + date;
    }
    if (month < 10) {
        month = "0" + month;
    }
    var dtDown = date + "-" + month + "-" + year;
    return dtDown;
}
function arrayContainsArray(superset, subset) {
    if (0 === subset.length || superset.length < subset.length) {
        return false;
    }
    for (var i = 0; i < subset.length; i++) {
        if (superset.indexOf(subset[i]) === -1)
            return false;
    }
    return true;
}
function LRTrim(value) {
    if (value) {
        return value.trim();
    }
    else {
        return "";
    }
}
var daterangepickersetting = {
    format: 'MM/DD/YYYY',
    opens: 'left',
    drops: 'up',
    "locale": {
        "applyLabel": getResourceValue("JsApply"),
        "cancelLabel": getResourceValue("CancelAlert")
    }
};
function PreviousDateByDay(day) {
    var today = new Date();
    var date = new Date(today.setDate(today.getDate() - day));
    return $.datepicker.formatDate('mm/dd/yy', date);
}
//#endregion

//#region hashcolorcodefromword
function hashCode(str) {
    var hash = 0;
    for (var i = 0; i < str.length; i++) {
        hash = str.charCodeAt(i) + ((hash << 5) - hash);
    }
    return hash;
}
function intToARGB(i) {
    var hex = ((i >> 24) & 0xFF).toString(16) +
        ((i >> 16) & 0xFF).toString(16) +
        ((i >> 8) & 0xFF).toString(16) +
        (i & 0xFF).toString(16);
    hex += '000000';
    return hex.substring(0, 6);
}
String.prototype.toHexColour = function () {
    return intToARGB(hashCode(this));
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
        url: '/UserProfile/GetUserProfile',
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

            //V2-491
            $(document).find('#PasswordReqMinLength').val(data.PasswordReqMinLength);
            $(document).find('#PasswordMinLength').val(data.PasswordMinLength);
            $(document).find('#PasswordRequireNumber').val(data.PasswordRequireNumber);
            $(document).find('#PasswordRequireAlpha').val(data.PasswordRequireAlpha);
            $(document).find('#PasswordRequireMixedCase').val(data.PasswordRequireMixedCase);
            $(document).find('#PasswordRequireSpecialChar').val(data.PasswordRequireSpecialChar);
            $(document).find('#PasswordNoRepeatChar').val(data.PasswordNoRepeatChar);
            $(document).find('#PasswordNotEqualUserName').val(data.PasswordNotEqualUserName);
            $(document).find('#UserProfileUserName').val(data.UserProfileUserName);
            //V2-491
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
        var msgText = "Profile successfully updated.";
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
//#region Change Active Site
function UpdateChangeActiveSiteOnSuccess(data) {
    if (data.Result == "success" && data.UpdateIndex > 0) {
        $('#activityModal').modal('hide');
        SuccessAlertSetting.text = "Site changed successfully.";
        var message = getResourceValue("ReassignAlert");
        swal(SuccessAlertSetting, function () {
            location.reload();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$('#showchangeactivesite').on('click', function () {
    $.ajax({
        url: '/ChangeActiveSite/GetActiveSites',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        beforeSend: function () {
            ShowLoader();
        },
        type: "GET",
        success: function (data) {
            var option = "";
            var statusList = data.SiteList;
            if (statusList) {
                option += '<option value="">--Select--</option>';
                for (var i = 0; i < statusList.length; i++) {
                    option += '<option value="' + statusList[i].Value + '">' + statusList[i].Text + '</option>';
                }
            }
            $(document).find('#ChangeSiteSiteId').empty().html(option);
            $(document).find('#ChangeSiteSiteId').val(data.ChangeSiteSiteId).trigger('change');
            ResetErrorDiv();
            $('#activityModal').modal('show');
            $(document).find('#ActiveSiteUpdateIndex').val(data.ActiveSiteUpdateIndex);
        },
        complete: function () {
            $(document).find('.select2picker').select2({});
            $.validator.unobtrusive.parse(document);
            $('input, form').blur(function () {
                $(this).valid();
            });
            CloseLoader();
        }
    });
});

$(document).on('click', '#showchangeversion', function () {

    $.ajax({
        url: '/Login/ChangeToVersion1',
        mode: 'cors',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        beforeSend: function () {
            ShowLoader();
        },
        type: "POST",
        success: function (data) {
            window.location.href = data.redirecturl;
        },
        complete: function () {
            CloseLoader();
        }
    });
});
//#endregion

//#region VendorMasterAdd
var dtVMAddGridTable;
var selectedVendorsArray = [];
var selectAllChecked = false;
var VMGridTotalItemCount = 0;
$(document).on('click', '#SelectFromVenMastUM', function () {
    $(document).find('#AddVendorPopupModal').modal('hide');
    VMPopupGrid();
});
//$(function () {
function VMPopupGrid() {
    selectedVendorsArray = [];
    $('#VMAddGrid-select-all').prop('checked', false);
    generateVMGridAddTable();
}
$(document).on('click', '#spnvendormasteradd', function () {
    selectedVendorsArray = [];
    $('#VMAddGrid-select-all').prop('checked', false);
    generateVMGridAddTable();
});
function generateVMGridAddTable() {
    if ($(document).find('#tblVMAddGrid').hasClass('dataTable')) {
        dtVMAddGridTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtVMAddGridTable = $("#tblVMAddGrid").DataTable({
        order: [[1, 'asc']],
        colReorder: true,
        rowGrouping: true,
        searching: true,
        'bPaginate': true,
        "bProcessing": true,
        dom: 'Btlipr',
        "pagingType": "full_numbers",
        serverSide: true,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
        },
        buttons: [],
        "filter": true,
        "orderMulti": true,
        "ajax": {
            "url": "/VendorMaster/GetVendorMasterAddPopupGrid",
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (result) {
                VMGridTotalItemCount = result.recordsTotal;
                return result.data;
            }
        },
        "columns":
            [
                {
                    "data": "VendorMasterId",
                    orderable: false,
                    "bSortable": false,
                    className: 'dt-center dt-body-center',
                    "name": "0",
                    'render': function (data, type, full, meta) {
                        if (selectAllChecked && VMGridTotalItemCount == selectedVendorsArray.length) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="VM-chksearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                        else {
                            if (selectedVendorsArray.indexOf(data) != -1) {
                                return '<input type="checkbox" name="id[]" data-eqid="' + data + '" checked="checked" class="VM-chksearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="VM-chksearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    }
                },
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "AddressCity", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                { "data": "AddressState", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
                {
                    "data": "Inactive", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center",
                    "mRender": function (data, type, row) {
                        if (data === true) {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" checked="checked" class="status"><span></span></label>';
                        }
                        else {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" class="status"><span></span></label>';
                        }
                    }

                }
            ],
        initComplete: function () {
            $("#VMAddGridModal").modal('show');
            $(document).on('click', '.status', function (e) {
                e.preventDefault();
            });
            mcxDialog.closeLoading();
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '#VMAddGrid-select-all', function () {
    selectedVendorsArray = [];
    var checked = this.checked;
    $.ajax({
        url: "/VendorMaster/GetVendorMasterData",
        async: true,
        type: "GET",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            $.each(data, function (index, item) {
                if (checked) {
                    if (selectedVendorsArray.indexOf(item.VendorMasterId) === -1) {
                        selectedVendorsArray.push(item.VendorMasterId);
                    }
                    selectAllChecked = true;
                }
                else {
                    var i = selectedVendorsArray.indexOf(item.VendorMasterId);
                    selectedVendorsArray.splice(i, 1);
                    selectAllChecked = false;
                }
            });
        },
        complete: function () {
            if (checked) {
                $(document).find('#tblVMAddGrid').find('.VM-chksearch').prop('checked', 'checked');
            } else {
                $(document).find('#tblVMAddGrid').find('.VM-chksearch').prop('checked', false);
            }
            CloseLoader();
        }
    });
});
$(document).on('change', '.VM-chksearch', function () {
    var thisrowdata = dtVMAddGridTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        $('#VMAddGrid-select-all').prop('checked', false);
        var index = selectedVendorsArray.indexOf(thisrowdata.VendorMasterId);
        selectedVendorsArray.splice(index, 1);
    }
    else {
        selectedVendorsArray.push(thisrowdata.VendorMasterId);
        if (VMGridTotalItemCount === selectedVendorsArray.length) {
            $('#VMAddGrid-select-all').prop('checked', 'checked');
        }
    }
});
$(document).on('click', '#btnAddVendorFromVM', function () {
    if (selectedVendorsArray.length === 0) {
        ShowGridItemSelectionAlert();
        return false;
    }
    $.ajax({
        url: "/VendorMaster/AddVendorFromVMGrid",
        async: true,
        type: "POST",
        data: { VendorsIds: selectedVendorsArray },
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data.Result === "success") {
                SuccessAlertSetting.text = getResourceValue("VendorAddAlert");
                swal(SuccessAlertSetting, function () {
                    dtVMAddGridTable.page('first').draw('page');
                    if ($(document).find('#vendorSearch').length > 0) {
                        dtVendorSearch.page('first').draw('page');
                    }
                });
            }
            else {
                var htmlerrortext = "<ul style='list-style: none;padding: 0;margin: 0;'>";
                $.each(data.errorList, function (index, item) {
                    var thiserror = "<li style='color: #d43f3a;'><i class='fa fa-circle bull'></i>" + item.ClinetLookUpId + " - " + item.ErrorMessage + "</li>";
                    htmlerrortext = htmlerrortext + thiserror;
                });
                htmlerrortext = htmlerrortext + "</ul>";
                HtmlAlertSettings.text = htmlerrortext;
                swal(HtmlAlertSettings);
            }
        },
        complete: function () {
            selectedVendorsArray = [];
            $('#VMAddGrid-select-all').prop('checked', false);
            CloseLoader();
        }
    });
});
//});
//#endregion

//#region UiConfig Custom Validator
function UIConfigVal(ViewName, ColumnName, Required, Hide, Disable) {
    this.ViewName = ViewName;
    this.ColumnName = ColumnName;
    this.Required = Required;
    this.Hide = Hide;
    this.Disable = Disable;
}
var validateControls = [];
function getAllViews(viewname, isextern) {
    $.ajax({
        url: "/Base/UiConfigAllColumns",
        type: "POST",
        dataType: "json",
        async: false,
        beforeSend: function () {
            ShowLoader();
        },
        data: { viewName: viewname, isExternal: isextern },
        success: function (data) {
            $.each(data.vList, function (index, value) {
                var UIConfigValobj = new UIConfigVal(value.ViewName, value.ColumnName, value.Required, value.Hide, value.Disable);
                validateControls.push(UIConfigValobj);
            });
        },
        complete: function () {
            CloseLoader();
        }
    });
}
//#endregion

//#region Fusion Chart
function SetFunsionChartGlobalSettings() {
    // Global Configuration
    FusionCharts.options.license({
        key: 'mB-16A7C-13f1nA3H2C2C4C3C6B6B4G5F2B1H2bB-16uD2E6zA-8rleA5C7pg1qD4F1H3A2B2C2C1C5F1A1F1A3A10A4B2C2dH-9uD1H4B3B-16D1D3D1psfA32B2B9d1dB-11G4F4D3j1xxA5C5E3F5E1H4E1A9A4D4E2F-11wqH1B3DB8gapB4A3H4aC-22C6WE6F4hjH-8F2C8D5D7B5E5G5B1C3C2C4A6E5E1c==',
        creditLabel: false,
    });
}
//#endregion

//#region PunchoutPurchaserequestaddFromMenu 

function clearDropzone() {
    deleteServer = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        Dropzone.forElement("div#dropzoneForm").destroy();
    }
}

$(document).on('click', '.AddPrequestPlus', function () {
    swal({
        title: getResourceValue("makePunchoutPurchaseAlert"),
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "confirm btn btn-lg btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    },
        function (isConfirm) {
            if (isConfirm == true) {
                generatePunchOutVendorDataTableForPlusMenu();
            }
            else {
                window.location.href = '/PurchaseRequest/Add';
            }
        });

});

//#endregion

//#region QR Scanner Reader
//#region  Common 
var scanner;
function StopCamera() {
    if (scanner) {
        scanner.stop().then((ignore) => {

        }).catch((err) => {

        });
    }
}

$(document).on('hidden.bs.modal', '#QrCodeReaderModal', function () {
    if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
        $(document).find('#QrCodeReaderModal').modal("hide");
    }
    if (!$(document).find('#QrCodeReaderModal').hasClass('show')) {
        $(document).find('#QrCodeReaderModal').modal("hide");
        StopCamera();
    }
});
//#endregion

//#region Equipment QR Reader
var Equipment_textFieldID = "";
var Equipment_valueFieldID = "";
function QrScannerEquipment(txtID, ValID) {
    Equipment_textFieldID = "";
    Equipment_valueFieldID = "";
    Equipment_textFieldID = '#' + txtID;
    if (ValID != '') {
        Equipment_valueFieldID = '#' + ValID;
    }
    if (!$(document).find('#QrCodeReaderModal').hasClass('show')) {
        $(document).find('#QrCodeReaderModal').modal("show");
        StartQRReaderEquipment();
    }

}
function StartQRReaderEquipment() {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
                //aspectRatio: aspectratio //1.7777778
            }, success => {
                onScanSuccessEquipment(success);
            }, error => {
            });
        } else {
            if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
                $(document).find('#QrCodeReaderModal').modal("hide");
            }
        }
    }).catch(e => {
        if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
            $(document).find('#QrCodeReaderModal').modal("hide");
        }
        if (e && e.startsWith('NotReadableError')) {
            ShowErrorAlert(getResourceValue("cameraIsBeingUsedByAnotherAppAlert"));
        }
        else if (e && e.startsWith('NotFoundError')) {
            ShowErrorAlert(getResourceValue("cameraDeviceNotFoundAlert"));
        }

    });
}
function onScanSuccessEquipment(decodedText) {
    $.ajax({
        url: "/WorkOrder/GetEquipmentIdByClientLookUpId?clientLookUpId=" + decodedText,
        type: "GET",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
                $(document).find('#QrCodeReaderModal').modal("hide");
            }
            if (data.EquipmentId > 0) {
                $(document).find(Equipment_textFieldID).val('');
                $(document).find(Equipment_textFieldID).val(decodedText).removeClass('input-validation-error').css("display", "block");
                if (Equipment_valueFieldID != '') {
                    $(document).find(Equipment_valueFieldID).val('');
                    $(document).find(Equipment_valueFieldID).val(data.EquipmentId).removeClass('input-validation-error').css("display", "none").trigger('change');;
                    if ($(document).find(Equipment_valueFieldID).parent().find('div > button.ClearAssetModalPopupGridData').length > 0) {
                        //Dynamic Pages Cross button to clear the textbox Value
                        $(document).find(Equipment_valueFieldID).parent().find('div > button.ClearAssetModalPopupGridData').css('display', 'block');
                    }

                }
                //V2-948
                var SourceAssetAccount = $(document).find("#SourceAssetAccount").val();
                if (SourceAssetAccount != undefined && SourceAssetAccount == "True") {
                    getlaboraccount(data.EquipmentId);

                }
                //V2-948
            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodeMsgforEquipment').replace('${decodedText}', decodedText));
            }

        },
        complete: function () {
            StopCamera();
            CloseLoader();
        },
        error: function (xhr) {
            ShowErrorAlert(getResourceValue("somethingWentWrongAlert"));
            CloseLoader();
        }
    });
}
//#endregion
//#endregion

//#region ProgressBar In Printing V2-663
var PrintingCountConnectionID = "";
function ProgressbarDynamicStatus() {
    // initialize the connection to the server
    var progressNotifier = $.connection.notificationHub;

    // client-side ProgressBarCurrentStatus function that will be called from the server-side
    progressNotifier.client.ProgressBarCurrentStatus = function (TotalWo, count) {
        // update progress Bar
        UpdateProgress(TotalWo, count);
    };

    //// establish the connection to the server and start server-side operation
    $.connection.hub.start().done(function () {
        PrintingCountConnectionID = $.connection.hub.id;
        /*   console.log("PrintingCountConnectionID -- " + PrintingCountConnectionID );*/
        // call the method CallLongOperation defined in the Hub
        // progressNotifier.server.callLongOperation();
    });
}
function UpdateProgress(TotalWo, count) {
    $("#PogressBarStatus").html("");
    $('.progress-bar').html("");
    var widths = parseFloat(100 / TotalWo);
    $('.progress-bar').width(widths * count + '%');
    var Status = count + ' of ' + TotalWo + ' work order printed.';
    /*  $('.progress-bar').html(Status);*/
    $("#PogressBarStatus").html(Status);
}
function ShowProgressBar() {
    CloseLoader();
    $("#PogressBarStatus").html("");
    $('.progress-bar').width('0%');
    $("#ProgressPopUp").modal("show");
}
function CloseProgressBar() {
    $("#ProgressPopUp").modal("hide");
}
//#endregion

//#region QRCode
function DrawQRCode(element, data) {
    if (data) {
        element.html('').prepend('<img src="' + data + '" class="somax-qr" style="max-width:100%;" " />');
    }
    else {
        ShowErrorAlert("Data selected for the QR code is not in proper format.");
    }
}
//#endregion

//#region BarCode
function DrawBarCode(element, data) {
    JsBarcode(element, data, {
        format: "CODE39"
    });
}
//#endregion

//#region V2-763 QRCode scanning for Search
var searchfield = "";
function QrScannerSearch(txtID) {
    searchfield = "";
    searchfield = '#' + txtID;
    if (!$(document).find('#QrCodeReaderModal').hasClass('show')) {
        $(document).find('#QrCodeReaderModal').modal("show");
        StartQRScanning();
    }

}
function StartQRScanning() {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
            }, success => {
                $(document).find(searchfield).val(success)
                if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
                    $(document).find('#QrCodeReaderModal').modal("hide");
                }
            }, error => {
            });
        } else {
            if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
                $(document).find('#QrCodeReaderModal').modal("hide");
            }
        }
    }).catch(e => {
        if ($(document).find('#QrCodeReaderModal').hasClass('show')) {
            $(document).find('#QrCodeReaderModal').modal("hide");
        }
        if (e && e.startsWith('NotReadableError')) {
            ShowErrorAlert(getResourceValue("cameraIsBeingUsedByAnotherAppAlert"));
        }
        else if (e && e.startsWith('NotFoundError')) {
            ShowErrorAlert(getResourceValue("cameraDeviceNotFoundAlert"));
        }

    });
}
//#endregion