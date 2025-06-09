//JQuery fallback
var pathToJQuery;
if ("querySelector" in document
    && "localStorage" in window
    && "addEventListener" in window) {
    pathToJQuery = "vendors/jquery-2.1.0";
} else {
    pathToJQuery = "vendors/jquery-1.11.0";
}

//Paths
requirejs.config({
    paths: {
        custom: "custom",
        amplify: "vendors/amplify.min",
        jquery: pathToJQuery
    },
    waitSeconds: 200,
    shim: {
        "custom": ["jquery"],
        "amplify": {
            deps: ["jquery"],
            exports: "amplify"
        }
    }
});

//Actions
require(["utilities/localStorage", "jquery"], function (local) {

    local.setSession("sessionToken", null);
    local.setSession("sessionUser", null);
    local.setSession("isGuestSession", false);
    local.setSession("isFirstRun", false);
    local.setSession("isExpired", false);
    var hiddenvar1;
    var hiddenvar2;

    //You must repleace your Yurbi Server name/URL here
    $(document).ready(function () {
        
        hiddenvar1 = hdnurlval.Get("ApiUrl");
        hiddenvar2 = hdnrdurlval.Get("ReportServerUrl");
        var userVal = hdnUserName.Get("UserNameValue");
        var passVal = hdnPasswordval.Get("passwordValue");
        //var userVal = "BBU_User";
        //var passVal = "user123";
        loginAction(userVal, passVal, false);
    });

    ////Login click
    //$("#login").click(function () {

    //});

    function loginAction(user, pass, isGuest) {
        
        var userVal = user;
        var passVal = pass;
        //Fields not empty
        if (userVal != null && userVal != "" && passVal != null && passVal != "") {
            var loginJSON = { "bolForceLogin": true, "UserId": userVal, "UserPassword": passVal, "isGuest": isGuest };
            $.ajax({
                url: hiddenvar1 + '/api/login/DoLogin',
                type: 'POST',
                data: JSON.stringify(loginJSON),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: true,
                success: function (msg) {
                    alertResponse(msg, isGuest);
                },
                error: function (msg, ajaxOptions, thrownError) {
                    alertResponse(thrownError);
                }
            });
        } else {
            //Fields empty
            alert("User Name, Password must be provided.");
        }
    }

    //Responses
    function alertResponse(msg) {
        
        if (msg.ErrorCode == null) {
            alert("Oops... There was an error with the login response. Please report the issue to the Yurbi Administrator", "top", "error");
            return;
        }
        var errorCode = msg.ErrorCode;
        //Succes
        if (errorCode == 0) {
            var isAdmin = false;
            var isGuestSession = false;
            var isExpired = msg.isPassportExpiredBase;
            var isFirstRun = msg.isPassportFirstRun;
            if (msg.LoginUser != null) {
                isAdmin = msg.LoginUser.isAdmin || msg.LoginUser.isSuperAdmin;
            }
            if (msg.LoginSession != null) {
                isGuestSession = msg.LoginSession.isGuestSession;
            }
            //Save session and user name in localstorage
            local.setSession("embedDashboard", null);
            local.setSession("sessionToken", msg.LoginSession.SessionToken);
            local.setSession("sessionUser", msg.LoginUser);
            local.setSession("isFirstRun", isFirstRun);
            local.setSession("isExpired", isExpired);
            local.setSession("isGuestSession", msg.LoginSession.isGuestSession);
            alert("Login successful.", "top", "success");
            document.getElementById("myframe").setAttribute("src", hiddenvar2);
            //window.location.href = hiddenvar2;
        }
            //Error
        else {
            alert(msg.ErrorMessage);
        }
    };
});
