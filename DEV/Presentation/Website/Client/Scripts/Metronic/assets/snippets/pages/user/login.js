var SnippetLogin = function () {
    var login = $('#m_login');
    var showErrorMsg = function (form, type, msg) {
        var alert = $('<div class="m-alert m-alert--outline alert alert-' + type + ' alert-dismissible" role="alert">\
			<button type="button" class="close" data-dismiss="alert" aria-label="Close"></button>\
			<span></span>\
		</div>');
        form.find('.alert').remove();
        alert.prependTo(form);
        alert.animateClass('fadeIn animated');
        alert.find('span').html(msg);
    }
    var displaySignUpForm = function () {
        login.removeClass('m-login--forget-password');
        login.removeClass('m-login--signin');
        login.addClass('m-login--signup');
        login.find('.m-login__signup').animateClass('flipInX animated');
    }
    var displaySignInForm = function () {
        login.removeClass('m-login--forget-password');
        login.removeClass('m-login--signup');

        login.addClass('m-login--signin');
        login.find('.m-login__signin').animateClass('flipInX animated');
    }
    var displayForgetPasswordForm = function () {
        login.removeClass('m-login--signin');
        login.removeClass('m-login--signup');
        login.addClass('m-login--forget-password');
        $.ajax({
            url: '/LogIn/ForgetPasswordPatial',
            type: "GET",
            success: function (response, status, jqXHR) {
                if (jqXHR.getResponseHeader('content-type').indexOf('text/html') >= 0) {

                    $(document).find('#forgetPasswordContainer').html(response);
                    login.find('.m-login__forget-password').animateClass('flipInX animated');
                }
            },
            error: function (xhr, errorType, exception) {
                console.log('error');
                console.log(xhr);
                console.log(errorType);
                console.log(exception);
            },
            complete: function (response) {
            }
        });
    }
    var handleFormSwitch = function () {
        $(document).on('click', '#m_login_forget_password', function (e) {
            e.preventDefault();
            $('#Password').val('');
            $('#UserName').val('');
            $('.m-checkbox input').prop('checked', false);
            $('#errormsg').text("");
            $('#mobileView').css('display', 'none');
            displayForgetPasswordForm();
        });
        $(document).on('click', '#m_login_forget_passwordFirst', function (e) {
            e.preventDefault();
            $('#errorMessage').text("");
            $('#m_login_forget_passwordFirst').text("");
            $('#UserName').val('');
            $('#PasswordId').val('');
            displayForgetPasswordForm();
        });
        $(document).on('click', '.m_login_forget_password_cancel', function (e) {
            e.preventDefault();
            $('input').removeClass('errorClass');
            $('#error_UserNameEMail').text('');

            //var id = $(this).attr('id');
            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                $('#mobileView').css('display', 'block');
            }
            displaySignInForm();
        });
        $(document).on('click', '#m_login_page', function (e) {
            e.preventDefault();
            $("#m_user").val('');
            $("#m_email").val('');
            $("#m_user").css({ 'border-color': '', 'opacity': '' });
            $("#m_email").css({ 'border-color': '', 'opacity': '' });
            $('#myModal').modal('toggle');
            displaySignInForm();
        });
        $(document).on('click', '#m_login_signup', function (e) {
            e.preventDefault();
            displaySignUpForm();
        });
        $(document).on('click', '#m_login_signup_cancel', function (e) {
            e.preventDefault();
            displaySignInForm();
        });
    }
    var handleSignInFormSubmit = function () {
        $(document).on('click', '#m_login_signin_submit', function (e) {
            e.preventDefault();
            $('input').removeClass('errorClass');
            $('#errormsg').text("");
            var btn = $(this);
            var form = $(this).closest('form');
            form.validate({
                rules: {
                    UserName: {
                        required: true,
                    },
                    Password: {
                        required: true
                    }
                },
                errorPlacement: function (error, element) {
                    if (element.val() == "") {
                        element.addClass('errorClass');
                    }
                },
            });
            if (!form.valid()) {
                return;
            }
            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            form.submit();
        });
    }
    var handleSignUpFormSubmit = function () {
        $('#m_login_signup_submit').click(function (e) {
            e.preventDefault();
            var btn = $(this);
            var form = $(this).closest('form');
            form.validate({
                rules: {
                    fullname: {
                        required: true
                    },
                    email: {
                        required: true,
                        email: true
                    },
                    password: {
                        required: true
                    },
                    rpassword: {
                        required: true
                    },
                    agree: {
                        required: true
                    }
                }
            });
            if (!form.valid()) {
                return;
            }
            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            form.ajaxSubmit({
                url: '',
                success: function (response, status, xhr, $form) {
                    setTimeout(function () {
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                        form.clearForm();
                        form.validate().resetForm();
                        displaySignInForm();
                        var signInForm = login.find('.m-login__signin form');
                        signInForm.clearForm();
                        signInForm.validate().resetForm();
                        showErrorMsg(signInForm, 'success', 'Thank you. To complete your registration please check your email.');
                    }, 2000);
                }
            });
        });
    }
    var handleForgetPasswordFormSubmit = function () {
        $(document).on('click', '.m_login_forget_password_submit', function (e) {
            e.preventDefault();
            SendMail($(this));
        });
    }
    return {
        init: function () {
            handleFormSwitch();
            handleSignInFormSubmit();
            handleSignUpFormSubmit();
            handleForgetPasswordFormSubmit();
        }
    };
}();
function CheckLoggedInFromMob() {
    var isMobile = false;
    if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
        || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
        return true;
    }
    return isMobile;
}
jQuery(document).ready(function () {
    SnippetLogin.init();
     
    if ($("#IsSOMAXAdmin").val() == "True") {
        $("#m_login_signin_submit").addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
        getClientUserInfoListDataTable();
    }
    $(document).on('focus', '#m_email', function () {
        $('#m_user').val('');
        $("#m_user").css({ 'border-color': '', 'opacity': '' });
        $("#m_email").css({ 'border-color': '', 'opacity': '' });
    });
    $(document).on('focus', '#m_user', function () {
        $('#m_email').val('');
        $("#m_email").css({ 'border-color': '', 'opacity': '' });
        $("#m_user").css({ 'border-color': '', 'opacity': '' });
    });
    var _isLoggedInFromMobile = CheckLoggedInFromMob();
    // sweta
    if (_isLoggedInFromMobile === true) {
        console.log('Mobile / ipad');
    }
    else {
        console.log('desktop');
    }
    //sweta
    $('#ExternalIsLoggedInFromMobile').val(_isLoggedInFromMobile);
    $('#IsLoggedInFromMobile').val(_isLoggedInFromMobile);
});
function ValidateMail(mailId) {
    //var re = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(mailId);
}

$('#m_popup_close').click(function (e) {
    $('#myModal').modal('toggle');
    $('#m_user').val('');
    $('#m_email').val('');
});

$(document).on('change', '#IsSelected', function () {
    var selectedText = $(this).find('option:selected').val();
    $.ajax({
        url: '/LogIn/SomaxLogIn',
        type: "POST",
        data: { "Language": selectedText },
        success: function (response, status, jqXHR) {
            console.log(response);
            if (jqXHR.getResponseHeader('content-type').indexOf('text/html') >= 0) {
                $(document).find('#loginForm').html(response);
            }
        },
        error: function (xhr, errorType, exception) {
            console.log('error');
            console.log(xhr);
            console.log(errorType);
            console.log(exception);
        },
        complete: function (response) {
            $(document).find("#IsSelected option").each(function () {
                if ($(this).text() == selectedText) {
                    $(this).attr('selected', 1);
                }
            });
            $(document).find('.selectpicker').selectpicker();
            $(document).find('.selectpicker').selectpicker('refresh');
        }
    });
});
//#region Show_Hide Controls
function show1() {
    $('.errMsg').text('');
    $(document).find('#m_email').val('');
    $(document).find('#m_user').val('');
    $(document).find('#divCancelBtn').hide();
    $(document).find('#divUserID').css('display', 'block');
    $(document).find('#divPassword').css('display', 'none');
}
function show2() {
    $('.errMsg').text('');
    $(document).find('#m_email').val('');
    $(document).find('#m_user').val('');
    $(document).find('#divCancelBtn').hide();
    $(document).find('#divUserID').css('display', 'none');
    $(document).find('#divPassword').css('display', 'block');
}
//#endregion
//#region Resend Mail
$(document).on('click', '#m_login_popupsignup', function (e) {
    e.preventDefault();
    SendMail($(this));
});
//#endregion
//#region Common
$(document).find('#signInForm').find('input[type="text"],input[type="password"]').blur(function () {
    if ($(this).val()) {
        $(this).removeClass('errorClass');
    }
});
function SendMail(thiselement) {
    var btn = thiselement;
    var form = thiselement.closest('form');
    var email = $('#m_email').val();
    if (email) {
        if (!ValidateMail(email.trim())) {
            $('#m_email').css('border-color', 'red');
            return;
        }
    }
    var user = $('#m_user').val();
    if (email.trim() == "" && (user.trim() == "" || user.trim() == undefined)) {
        $('#m_email').css('border-color', 'red');
        $('#m_user').css('border-color', 'red');
        return;
    }
    $.ajax({
        url: "/LogIn/ForgotPasswordSendEMail",
        type: "POST",
        data: JSON.stringify({ 'email': email, 'user': user }),
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            //if (thiselement.attr('id') == "m_login_forget_password_submit") {
            if (thiselement.hasClass('m_login_forget_password_submit')) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            }
            else {
                $(document).find('#resendMailLoader').css('display', 'block');
            }
        },
        success: function (data, status, jqXHR) {
            if (jqXHR.getResponseHeader('content-type').indexOf('text/html') >= 0) {
                $('#confirmationPopUp').html(data);
                $('#myModal').modal('show');
            } else {
                $('.errMsg').text('Email or userName not found');
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("An error has occured!!!");
        },
        complete: function () {
            //if (thiselement.attr('id') == "m_login_forget_password_submit") {
            if (thiselement.hasClass('m_login_forget_password_submit')) {
                btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
            else {
                $(document).find('#resendMailLoader').css('display', 'none');
            }
        }
    });
}
//#endregion
//#region Show Hide Password
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
//#region V2-911
var dtClientUserInfoList;
var ClientUserInfoListId = "";
var ClientName = "";
var SiteName = "";
var ClientId = "";
var DefaultSiteId = "";

function getClientUserInfoListDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#tblClientUserInfoListTableModalPopup').hasClass('dataTable')) {
        dtClientUserInfoList.destroy();
    }
    /*mcxDialog.loading({ src: "../content/Images" });*/
    dtClientUserInfoList = $("#tblClientUserInfoListTableModalPopup").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        "pageLength": 10,
        stateSave: false,
        language: {
            url: "/LogIn/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/LogIn/RetrieveByUserInfoIdChunkSearchLookupList",
            data: function (d) {
                d.UserInfoId = $('#UserInfoId').val();
                d.UserName = $('#UserName').val();
                d.ClientName = ClientName;
                d.SiteName = SiteName;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
            [
                {
                    "data": "ClientUserInfoListID", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=link_ClientUserInfoList_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "ClientName", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                },
                {
                    "data": "SiteName", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
                {
                    "data": "ClientId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
                {
                    "data": "DefaultSiteId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
               
            ],
        "rowCallback": function (row, data, index, full) {
        },
        initComplete: function () {
            $(document).find('#tblClientUserInfoListTableModalPopupfooter').show();
            /*mcxDialog.closeLoading();*/
            SetPageLengthMenuLogin();
            $(document).find('#tblClientUserInfoListTableModalPopup tfoot th').each(function (i, v) {
                var colIndex = i;
                if (colIndex !== 3 && colIndex !== 4 && colIndex !== 0) {
                    $(this).html('<input type="text" style="width:100%" class="clsClientUserInfoList tfootsearchtxt" id="ClientUserInfoListindex_' + colIndex + '"  /><i class="fa fa-search dropSearchIcon"></i>');
                    if (ClientUserInfoListId) { $('#ClientUserInfoListindex_0').val(ClientUserInfoListId); }
                    if (ClientName) { $('#ClientUserInfoListindex_1').val(ClientName); }
                    if (SiteName) { $('#ClientUserInfoListindex_2').val(SiteName); }
                    if (ClientId) { $('#ClientUserInfoListindex_3').val(ClientId); }
                    if (DefaultSiteId) { $('#ClientUserInfoListindex_4').val(DefaultSiteId); }
               

                }
                else {
                    $(this).html('');
                }
            });
            $(document).ready(function () {
                $(window).keydown(function (event) {
                    if (event.keyCode === 13) {
                        event.preventDefault();
                        return false;
                    }
                });
                $('#tblClientUserInfoListTableModalPopup tfoot th').find('.clsClientUserInfoList').on("keyup", function (e) {
                    if (e.keyCode == 13) {
                        ClientUserInfoListId = $('#ClientUserInfoListindex_0').val();
                        ClientName = $('#ClientUserInfoListindex_1').val();
                        SiteName = $('#ClientUserInfoListindex_2').val();
                        ClientId = $('#ClientUserInfoListindex_3').val();
                        DefaultSiteId = $('#ClientUserInfoListindex_4').val();
                        dtClientUserInfoList.page('first').draw('page');
                    }
                });
            });
            $("#m_login_signin_submit").removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            if (!$(document).find('#ClientUserInfoListTableModalPopup').hasClass('show')) {
                $(document).find('#ClientUserInfoListTableModalPopup').modal("show");
            }
        }
    });
}
$(document).on('click', '.link_ClientUserInfoList_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtClientUserInfoList.row(row).data();
    $('#ClientUserInfoListId').val(data.ClientUserInfoListID);
    $("#m_login_signin_submit").trigger('click');
    if ($(document).find('#ClientUserInfoListTableModalPopup').hasClass('show')) {
        $(document).find('#ClientUserInfoListTableModalPopup').modal("hide");
    }
});


function SetPageLengthMenuLogin() {
    var pagelengthmenu = $(document).find('.searchdt-menu');
    pagelengthmenu.select2({
        minimumResultsForSearch: -1
    });
    $(document).find('.dataTables_length').show();
}
//#endregion