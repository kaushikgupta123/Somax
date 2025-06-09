$(function () {
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
    $(document).on('click','#forget_password_submit,#m_login_popupsignup',function (e) {
        e.preventDefault(); 
        var btn = $(this);
        var email = $('#m_email').val();
        if (email != "") {
            if (!ValidateMail(email)) {
                $('#m_email').css('border-color', 'red');
                return;
            }
        }
        var user = $('#m_user').val();
        $('#required_text').text("");
        if (email == "" && (user == "" || user == undefined)) {
            $('#m_email').css('border-color', 'red');
            $('#m_user').css('border-color', 'red');
            return;
        }
        btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
        $.ajax({
            url: "/LogIn/ForgotPasswordSendEMail",
            type: "POST",
            data: JSON.stringify({ 'email': email, 'user': user }),
            contentType: "application/json; charset=utf-8",
            success: function (data, status, jqXHR) {
                if (jqXHR.getResponseHeader('content-type').indexOf('text/html') >= 0) {
                    console.log(data);
                    $('#confirmationPopUp').html(data);
                    $('#myModal').modal('show');
                    btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                } else {
                    $('#error_UserNameEMail').text("Email or userName not found");
                    btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false); // remove 
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(XMLHttpRequest);
                console.log(textStatus);
                console.log(errorThrown);
                alert("An error has occured!!!");
            }
        });
    });

    $('#m_popup_close').click(function (e) {
        $('#myModal').modal('toggle');
        $('#m_user').val('');
        $('#m_email').val('');
    });

    function ValidateMail(mailId) {
        var re = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
        return re.test(mailId);
    }

    $("input").focus(function () {
        $(this).removeClass("vlaidationError");
    });
   
    $('#reset_Password').click(function (e) {
        e.preventDefault();
        $('#error_Message').text("");
        ValidateUser()
        if (!ValidateUser()) {
            return false;
        }
        else
        {
            var resetPassword = {
                NewPassword : "",
                ConfirmPassword : "",
                TempPassword : "",
                SecurityAnswer : "",
                PasswordCode : "",
                UserName : ""
            };

                resetPassword.NewPassword = $("#new_Password").val();
                resetPassword.ConfirmPassword = $("#confirm_Password").val();
                resetPassword.TempPassword = $("#temporary_Password").val();
                resetPassword.SecurityAnswer = $("#security_Response").val();
                resetPassword.PasswordCode = $('#answer_Type :selected').text();
                resetPassword.UserName = $("#user_Id").val();
                $.ajax({
                    url: "/LogIn/ResetPassword",
                    type: "POST",
                    data: JSON.stringify({ 'resetPassword': resetPassword }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            alert("Sccessfully changed");
                        } else {
                            $('#error_Message').text(data.message)
                        }
                    },
                    error: function () {
                        alert("An error has occured!!!");
                    }
                });
        }
    
    });

function ValidateUser() {
    isValid = true;
    $("input, select").each(function () {
        var element = $(this);
        element.removeClass("vlaidationError");
        if (element.is("select")) {
            if (element.prop("selectedIndex") == 0) {
                element.addClass("vlaidationError").selectpicker('setStyle');;
                isValid = false;
                return;
            }
        }
        else {
            if (element.val() == "") {
                element.addClass("vlaidationError");
                isValid = false;
                return;
            }
        }
    });
    return isValid;
}

function getLanguage(s) {
   
}
    $(document).on('change', '#IsSelected', function () {
    var selectedText = $(this).find('option:selected').val();
    $.ajax({
        url: '/LogIn/SomaxLogIn',
        type: "POST",
        data: { "Language": selectedText },
        success: function (response, status, jqXHR) {
            console.log(response);
            if (jqXHR.getResponseHeader('content-type').indexOf('text/html') >= 0 ) {
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
            $(document).find("#IsSelected option").each(function(){
                if($(this).text() == selectedText) {
                    $(this).attr('selected', 1);
                }
            });
            $(document).find('.selectpicker').selectpicker();
            $(document).find('.selectpicker').selectpicker('refresh');
        }
    });
});
});