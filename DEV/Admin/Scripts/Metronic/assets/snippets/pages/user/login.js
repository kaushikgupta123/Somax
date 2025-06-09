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
        $(document).on('click', '#m_login_forget_password_cancel', function (e) {
            e.preventDefault();
            $('input').removeClass('errorClass');
            $('#error_UserNameEMail').text('');

            var id = $(this).attr('id');
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
        $(document).on('click', '#m_login_forget_password_submit', function (e) {
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
jQuery(document).ready(function () {
    SnippetLogin.init();
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
            if (thiselement.attr('id') == "m_login_forget_password_submit") {
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
            if (thiselement.attr('id') == "m_login_forget_password_submit") {
                btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
            else {
                $(document).find('#resendMailLoader').css('display', 'none');
            }
        }
    });
}
//#endregion
