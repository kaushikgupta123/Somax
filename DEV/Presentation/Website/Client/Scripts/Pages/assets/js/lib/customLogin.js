//JQuery fallback
var pathToJQuery;
//Global Variables
var Access;
var ApplicationName;
var SaveContactJson;
var adminPassVal;
var adminUserName;
var apiUrl;
var changeSecGroup = false;
var changeTimezone = false;
var company;
var contactId = "";
var createUser = false;
var dashUrl;
var descrip;
var enterpriseUser = 0;
var facilities = 0;
var firstName;
var foodServices = 0;
var groupName = "";
var hdnVar;
var jsonCurrentContact = [];
var jsonLoginResponse = {};
var jsonSecGroups = {};
var lastName;
var libUrl;
var numCurrentAssignedGroup = 0;
var roleName;
var serverUrl;
var smxTimeZone = 0;
var smxTimeZoneName = "";
var standard = 0;
var today = new Date();
var userEmail = "";
//After the UserInfo.ContactId column is added, and a SOMAX API is built to assign the Insights Contact ID, a function will be built to 
//update the UserInfo record with the Yurbi ContactId value
var userInfoId;
var userName = "";
var userType = "";
var userPassVal = "";
if ("querySelector" in document
    && "localStorage" in window
    && "addEventListener" in window) {
    pathToJQuery = "../lib/vendors/jquery-2.1.0";
} else {
    pathToJQuery = "../lib/vendors/jquery-1.11.0";
}

//Paths
requirejs.config({
    paths: {
        custom: "custom",
        amplify: "../lib/vendors/amplify.min",
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
var language = window.navigator.userLanguage || window.navigator.language;
language = String(language).substring(0, 2).toUpperCase();
//Actions
require(["utilities/localStorage", "jquery"], function (local) {
 //  ShowLoader();
    local.setSession("sessionToken", null);
    local.setSession("sessionUser", null);
    local.setSession("isGuestSession", false);
    local.setSession("isFirstRun", false);
    local.setSession("isExpired", false);
    local.setSession('ylang', language);
    //ApiUrl = local.getSession("ApiUrl");
    $(".login").hide();
    //You must repleace your Yurbi Server name/URL here
    $(document).ready(function () {

 if (typeof hdnVar === "undefined") {  // V2 SOMAX
      Access = document.getElementById("Access").value;
    } else {
      Access = hdnVar.Get("Access");
    }
             if (Access == "true") {
           
 //*** Testing **************
      var bTesting = false;
      //**************************
 if (typeof hdnVar === "undefined") {  // V2 SOMAX
        apiUrl = document.getElementById("ReportApiUrl").value;
        serverUrl = document.getElementById("ReportServerUrl").value;
        libUrl = document.getElementById("ReportServerLibUrl").value;
        dashUrl = document.getElementById("ReportServerDashBoard").value;
        adminUserName = document.getElementById("AdminUserNameval").value;
        userName = document.getElementById("UserNameVal").value;
        adminPassVal = document.getElementById("AdminPassword").value;
        userPassVal = document.getElementById("UserPassword").value;
        userEmail = document.getElementById("UserEmail").value;
        firstName = document.getElementById("FirstName").value;
        lastName = document.getElementById("LastName").value;
        company = document.getElementById("Company").value;
        descrip = document.getElementById("Description").value;
        userInfoId = document.getElementById("UserInfoId").value;
        // group set to the Client/Site data group name e.g. 0004-00025
        groupName = document.getElementById("Group").value;
        userType = document.getElementById("UserType").value;  //Full, Reference, WorkRequest, Inventory, and Enterprise
        if (typeof userType === "undefined" || userType === null) {
          userType = "Full";
        }
        if (userType == "Enterprise") {
          enterpriseUser = 1
        }
        //contactId = document.getElementById("ContactId").value;
        //if (typeof contactId === "undefined" || contactId === null) {
        //  contactId = "";
        //}
        standard = document.getElementById("Standard").value;
        if (typeof standard === "undefined" || standard === null) {
          standard = 0;
        }
        foodServices = document.getElementById("FoodServices").value;
        if (typeof foodServices === "undefined" || foodServices === null) {
          foodServices = 0;
        }
        facilities = document.getElementById("Facilities").value;
        if (typeof facilities === "undefined" || facilities === null) {
          facilities = 0;
        }
        smxTimeZoneName = document.getElementById("TimeZone").value;
        if (typeof smxTimeZoneName === "undefined" || smxTimeZoneName === null) {
          smxTimeZoneName = 'Eastern Standard Time';
        }
      } else { //V1 SOMAX
        apiUrl = hdnVar.Get("ReportApiUrl");
        serverUrl = hdnVar.Get("ReportServerUrl");
        libUrl = hdnVar.Get("ReportServerLibUrl");
        dashUrl = hdnVar.Get("ReportServerDashBoard");
        adminUserName = hdnVar.Get("AdminUserNameval");
        userName = hdnVar.Get("UserNameVal");
        adminPassVal = hdnVar.Get("AdminPassword");
        userPassVal = hdnVar.Get("UserPassword");
        userEmail = hdnVar.Get("UserEmail");
        firstName = hdnVar.Get("FirstName");
        lastName = hdnVar.Get("LastName");
        company = hdnVar.Get("Company");
        descrip = hdnVar.Get("Description");
        userInfoId = hdnVar.Get("UserInfoId");
        // group set to the Client/Site data group name e.g. 0004-00025
        groupName = hdnVar.Get("Group");

        userType = hdnVar.Get("UserType");  //Full, Reference, WorkRequest, Inventory, and Enterprise?
        if (typeof userType === "undefined" || userType === null) {
          userType = "Full";
        }
        if (userType == "Enterprise") {
          enterpriseUser = 1
        }
        contactId = hdnVar.Get("ContactId");
        if (typeof contactId === "undefined" || contactId === null) {
          contactId = "";
        }
        standard = hdnVar.Get("Standard");
        if (typeof standard === "undefined" || standard === null) {
          standard = 0;
        }
        foodServices = hdnVar.Get("FoodServices");
        if (typeof foodServices === "undefined" || foodServices === null) {
          foodServices = 0;
        }
        facilities = hdnVar.Get("Facilities");
        if (typeof facilities === "undefined" || facilities === null) {
          facilities = 0;
        }
        smxTimeZoneName = hdnVar.Get("TimeZone");
        if (typeof smxTimeZoneName === "undefined" || smxTimeZoneName === null) {
          smxTimeZoneName = 'Eastern Standard Time';
        }
      }
      //*** Testing **************
      if (bTesting === true) {
        //contactId = "636900687815715380";
        contactId = "";
        standard = 0;
        enterpriseUser = 0;
        foodServices = 1;
        facilities = 0;
        smxTimeZoneName = "Eastern Standard Time";
      }
      //**************************
      switch (smxTimeZoneName) {
        case 'US Eastern Standard Time':
          smxTimeZone = -5;
          break;
        case 'Eastern Standard Time':
          smxTimeZone = -5;
          break;
        case 'Central Standard Time':
          smxTimeZone = -6;
          break;
        case 'Mountain Standard Time':
          smxTimeZone = -7;
          break;
        case 'Pacific Standard Time':
          smxTimeZone = -8;
          break;
        default:
          smxTimeZone = -5;
      }
      roleName = "View";
      ApplicationName = "Report Data";
      //if (contactId.length !== 0) {
      //log in the current user
      loginAction(userName, userPassVal, false, true);
      jsonCurrentContact = jsonLoginResponse.LoginUser;
      if (jsonCurrentContact === null) {
        //user does not exist in Insights
        createUser = true;
      } else {
        //Different Data Security group?
        var jsonCurrentSecGroups = jsonCurrentContact.SecurityGroups;
        for (var i = 0; i < jsonCurrentSecGroups.length; i++) {   //Search the users sec groups for data sec group
          var obj = jsonCurrentSecGroups[i];
          //console.log(obj.GroupName.substring(0,4));
          if (Number(obj.GroupName.substring(0, 4)) > 0) {
            numCurrentAssignedGroup = i;
            break;
          }
        }
        //console.log("SOMAX Data Security Group: " + groupName);
        //console.log("Contact Data Security Group: " + jsonCurrentSecGroups[numCurrentAssignedGroup].GroupName);

        if (jsonCurrentSecGroups[numCurrentAssignedGroup].GroupName.substring(5, 10) === "00000") {
          enterpriseUser = 1
          //console.log("Enterprise User");
        }
        else {
          if (jsonCurrentSecGroups[numCurrentAssignedGroup].GroupName !== groupName) {
            changeSecGroup = true;
          }
        }
        //Different timezone?
        if (jsonCurrentContact.timezonename !== smxTimeZoneName || jsonCurrentContact.timezone !== smxTimeZone) {
          //console.log("Contact TimeZoneName: " + jsonCurrentContact.timezonename);
          //console.log("SOMAX TimeZoneName: " + smxTimeZoneName);
          changeTimezone = true;
        }
        DoLogout(jsonLoginResponse.LoginSession.SessionToken);
      }
      //console.log("Admin Username: " + adminUserName);
      //console.log("Admin Password: " + adminPassVal);

      if (changeTimezone === true || changeSecGroup === true || createUser === true) {
        loginAction(adminUserName, adminPassVal, false);
      } else {
        loginAction(userName, userPassVal, false, false);
      }
    }
        else {
            alert("Access Denied. Please report the issue to the SOMAX Administrator", "center", "error");
            $(document).find('.rep-loader').css('background', 'none');
        }
    });

  function setContact(token) {

    var SaveContactJ;
    var frsBusinessLevel = "";
    var frsProductLevel = "FRS-SOMAX-ProfEnt";
    var frsGroup = "FRS-" + groupName.substring(0, 4) + '-Sites';
    if (enterpriseUser != 1) {  // NOT Enterprise User
      // Package Level
      if (standard === 1) {
        frsProductLevel = "FRS-SOMAX-Standard";
      }
      // Business Type
      if (facilities == 1) {
        frsBusinessLevel = "FRS-SOMAX-Facilities";
      }
      if (foodServices == 1) {
        frsBusinessLevel = "FRS-SOMAX-Food Services";
      }
      if (frsProductLevel === "FRS-SOMAX-ProfEnt") {
        if (foodServices == 1 || facilities == 1) {
          SaveContactJ = {
            "sessionToken": token,
            "user": {
              "AllApplications": [],
              "AllGroups": [],
              "AllRoles": [],
              "ApplicationConstraints": [],
              "AuthType": "PIN",
              "ComboName": null,
              "CreateDate": today,
              "Description": descrip,
              "EmailAddress": userEmail,
              "ErrorCode": 0,
              "ErrorMessage": null,
              "FirstName": firstName,
              "FullName": "",
              "ID": null,
              "LastName": lastName,
              "LoginDate": today,
              "LoginName": userName,
              "ModifyDate": today,
              "Pin": userPassVal,
              "Preferences": [],
              "SecurityGroups": [{
                "GroupName": groupName,
                "GroupRoles": [{
                  "RoleId": 2,
                  "RoleName": roleName
                }]
              }, {
                "GroupName": frsBusinessLevel,
                "GroupRoles": [{
                  "RoleId": 2,
                  "RoleName": roleName
                }]
              }, {
                "GroupName": frsProductLevel,
                "GroupRoles": [{
                  "RoleId": 2,
                  "RoleName": roleName
                }]
              }, {
                "GroupName": frsGroup,
                "GroupRoles": [{
                  "RoleId": 2,
                  "RoleName": roleName
                }]
              }],
              "UserApplications": [{
                "ApplicationID": 1003,
                "ApplicationName": ApplicationName,
                "ApplicationRoleID": 6,
                "ApplicationRoleName": "Agent",
                "ApplicationRoleType": "0",
              }],
              "timezone": smxTimeZone,
              "timezonename": smxTimeZoneName,
              "UserState": 0,
              "isAdmin": false,
              "isAgent": false,
              "isArchitect": false,
              "isBuilder": false,
              "isFirstRun": false,
              "isSuperAdmin": false,
              "PIN": userPassVal
            },
            "withpin": true
          };

        } else {
          SaveContactJ = {
            "sessionToken": token,
            "user": {
              "AllApplications": [],
              "AllGroups": [],
              "AllRoles": [],
              "ApplicationConstraints": [],
              "AuthType": "PIN",
              "ComboName": null,
              "CreateDate": today,
              "Description": descrip,
              "EmailAddress": userEmail,
              "ErrorCode": 0,
              "ErrorMessage": null,
              "FirstName": firstName,
              "FullName": "",
              "ID": null,
              "LastName": lastName,
              "LoginDate": today,
              "LoginName": userName,
              "ModifyDate": today,
              "Pin": userPassVal,
              "Preferences": [],
              "SecurityGroups": [{
                "GroupName": groupName,
                "GroupRoles": [{
                  "RoleId": 2,
                  "RoleName": roleName
                }]
              }, {
                "GroupName": frsProductLevel,
                "GroupRoles": [{
                  "RoleId": 2,
                  "RoleName": roleName
                }]
              }, {
                "GroupName": frsGroup,
                "GroupRoles": [{
                  "RoleId": 2,
                  "RoleName": roleName
                }]
              }],
              "UserApplications": [{
                "ApplicationID": 1003,
                "ApplicationName": ApplicationName,
                "ApplicationRoleID": 6,
                "ApplicationRoleName": "Agent",
                "ApplicationRoleType": "0",
              }],
              "timezone": smxTimeZone,
              "timezonename": smxTimeZoneName,
              "UserState": 0,
              "isAdmin": false,
              "isAgent": false,
              "isArchitect": false,
              "isBuilder": false,
              "isFirstRun": false,
              "isSuperAdmin": false,
              "PIN": userPassVal
            },
            "withpin": true
          };
        }
      } else { //Standard - NOT Enterprise, so do NOT assign an FRS group (e.g. FRS-0004-Sites)
        if (foodServices == 1 || facilities == 1) {
          SaveContactJ = {
            "sessionToken": token,
            "user": {
              "AllApplications": [],
              "AllGroups": [],
              "AllRoles": [],
              "ApplicationConstraints": [],
              "AuthType": "PIN",
              "ComboName": null,
              "CreateDate": today,
              "Description": descrip,
              "EmailAddress": userEmail,
              "ErrorCode": 0,
              "ErrorMessage": null,
              "FirstName": firstName,
              "FullName": "",
              "ID": null,
              "LastName": lastName,
              "LoginDate": today,
              "LoginName": userName,
              "ModifyDate": today,
              "Pin": userPassVal,
              "Preferences": [],
              "SecurityGroups": [{
                "GroupName": groupName,
                "GroupRoles": [{
                  "RoleId": 2,
                  "RoleName": roleName
                }]
              }, {
                "GroupName": frsBusinessLevel,
                "GroupRoles": [{
                  "RoleId": 2,
                  "RoleName": roleName
                }]
              }, {
                "GroupName": frsProductLevel,
                "GroupRoles": [{
                  "RoleId": 2,
                  "RoleName": roleName
                }]
              }],
              "UserApplications": [{
                "ApplicationID": 1003,
                "ApplicationName": ApplicationName,
                "ApplicationRoleID": 6,
                "ApplicationRoleName": "Agent",
                "ApplicationRoleType": "0",
              }],
              "timezone": smxTimeZone,
              "timezonename": smxTimeZoneName,
              "UserState": 0,
              "isAdmin": false,
              "isAgent": false,
              "isArchitect": false,
              "isBuilder": false,
              "isFirstRun": false,
              "isSuperAdmin": false,
              "PIN": userPassVal
            },
            "withpin": true
          };

        } else {
          SaveContactJ = {
            "sessionToken": token,
            "user": {
              "AllApplications": [],
              "AllGroups": [],
              "AllRoles": [],
              "ApplicationConstraints": [],
              "AuthType": "PIN",
              "ComboName": null,
              "CreateDate": today,
              "Description": descrip,
              "EmailAddress": userEmail,
              "ErrorCode": 0,
              "ErrorMessage": null,
              "FirstName": firstName,
              "FullName": "",
              "ID": null,
              "LastName": lastName,
              "LoginDate": today,
              "LoginName": userName,
              "ModifyDate": today,
              "Pin": userPassVal,
              "Preferences": [],
              "SecurityGroups": [{
                "GroupName": groupName,
                "GroupRoles": [{
                  "RoleId": 2,
                  "RoleName": roleName
                }]
              }, {
                "GroupName": frsProductLevel,
                "GroupRoles": [{
                  "RoleId": 2,
                  "RoleName": roleName
                }]
              }],
              "UserApplications": [{
                "ApplicationID": 1003,
                "ApplicationName": ApplicationName,
                "ApplicationRoleID": 6,
                "ApplicationRoleName": "Agent",
                "ApplicationRoleType": "0",
              }],
              "timezone": smxTimeZone,
              "timezonename": smxTimeZoneName,
              "UserState": 0,
              "isAdmin": false,
              "isAgent": false,
              "isArchitect": false,
              "isBuilder": false,
              "isFirstRun": false,
              "isSuperAdmin": false,
              "PIN": userPassVal
            },
            "withpin": true
          };
        }
      }
    } else { //Enterprise User
      frsBusinessLevel = "FRS-SOMAX-Enterprise";
      //also assign the Enterprise FRS group (e.g. FRS-0004-Enterprise)
      frsGroup = "FRS-" + groupName.substring(0, 4) + '-Enterprise';
      SaveContactJ = {
        "sessionToken": token,
        "user": {
          "AllApplications": [],
          "AllGroups": [],
          "AllRoles": [],
          "ApplicationConstraints": [],
          "AuthType": "PIN",
          "ComboName": null,
          "CreateDate": today,
          "Description": descrip,
          "EmailAddress": userEmail,
          "ErrorCode": 0,
          "ErrorMessage": null,
          "FirstName": firstName,
          "FullName": "",
          "ID": null,
          "LastName": lastName,
          "LoginDate": today,
          "LoginName": userName,
          "ModifyDate": today,
          "Pin": userPassVal,
          "Preferences": [],
          "SecurityGroups": [{
            "GroupName": groupName,
            "GroupRoles": [{
              "RoleId": 2,
              "RoleName": roleName
            }]
          }, {
            "GroupName": frsBusinessLevel,
            "GroupRoles": [{
              "RoleId": 2,
              "RoleName": roleName
            }]
          }, {
            "GroupName": frsGroup,
            "GroupRoles": [{
              "RoleId": 2,
              "RoleName": roleName
            }]
          }],
          "UserApplications": [{
            "ApplicationID": 1003,
            "ApplicationName": ApplicationName,
            "ApplicationRoleID": 6,
            "ApplicationRoleName": "Agent",
            "ApplicationRoleType": "0",
          }

          ],
          "timezone": smxTimeZone,
          "timezonename": smxTimeZoneName,
          "UserState": 0,
          "isAdmin": false,
          "isAgent": false,
          "isArchitect": false,
          "isBuilder": false,
          "isFirstRun": false,
          "isSuperAdmin": false,
          "PIN": userPassVal
        },
        "withpin": true
      };
    }
    return SaveContactJ;
  }

  
    function loginAction(user, pass, isGuest, noResponse) {

    var userVal = user;
    var passVal = pass;
    if (typeof noResponse === "undefined" || noResponse === null) {
      noResponse = false;
    } else { noResponse = true; }
    //Fields not empty
    if (userVal !== null && userVal !== "" && passVal !== null && passVal !== "") {
      var loginJSON = {
        "bolForceLogin": true,
        "UserId": userVal,
        "UserPassword": passVal,
        "isGuest": isGuest
      };
      $.ajax({
        url: apiUrl + '/api/login/DoLogin',
        type: 'POST',
        data: JSON.stringify(loginJSON),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        // setting async to true causes the browser to not wait and then this script fails
        error: function (msg, ajaxOptions, thrownError) {
          //console.log("loginAction() - Logged in User: " + user );
          //console.log("alertResponse being called from loginAction() - error" );
          alertResponse(thrownError);
        },
        success: function (msg) {
          //console.log("Logged In User: " + msg.LoginUser.FullName );
          if (noResponse === false) {
            //console.log("alertResponse being called from loginAction() - success when noResponse is 'false'");
            alertResponse(msg, isGuest);
          } else {
            jsonLoginResponse = msg;
            jsonCurrentContact = msg.LoginUser;
          }
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
      alert("Oops... There was an error with the login response. Please report the issue to the SOMAX Administrator", "top", "error");
      return;
    }
    var errorCode = msg.ErrorCode;
    if (errorCode != null && errorCode == 101) {
      loginAction(adminUserName, adminPassVal, false);
    }
    //Success
    if (errorCode == 0) {
      var isAdmin = false;
      var isGuestSession = false;
      var isExpired = msg.isPassportExpiredBase;
      var isFirstRun = msg.isPassportFirstRun;
      var Session_Token = msg.LoginSession.SessionToken;
      if (msg.LoginUser != null) {
        isAdmin = msg.LoginUser.isAdmin || msg.LoginUser.isSuperAdmin;
      } else {
        jsonCurrentContact = msg.LoginUser;
      }
      if (msg.LoginSession != null) {
        isGuestSession = msg.LoginSession.isGuestSession;
      }
      //Save session and user name in localstorage
      local.setSession("embedDashboard", null);
      local.setSession("sessionToken", Session_Token);
      local.setSession("sessionUser", msg.LoginUser);
      local.setSession("isFirstRun", isFirstRun);
      local.setSession("isExpired", isExpired);
      local.setSession("isGuestSession", msg.LoginSession.isGuestSession);
      //alert("Login successful.", "top", "success");

      if (isAdmin === false) {
        setTimeout(function () {
          document.getElementById("myframe").setAttribute("src", serverUrl + "/val_login.html?h=1&s=" + Session_Token);
          $("#login-panel-lightbox").hide();
        }, 2000);

        //window.location.href = dashboard;
      } else if (isAdmin === true) {
        if (createUser === true) {
          CreateUser(Session_Token);
          SaveContactJson = setContact(Session_Token);
        } else {
          if (changeSecGroup === true) {
            GetSecurityGroups(Session_Token);   //sets the jsonSecGroups variable with the current groups
            var numSecGroup = 0;
            for (var sg = 0; sg < jsonSecGroups.length; sg++) {   //Search sec groups for the assigned site in SOMAX
              var secgrpobj = jsonSecGroups[sg];
              if (secgrpobj.GroupName === groupName) {
                numSecGroup = sg;
                break;
              }
            }
            //console.log("********** Data Group Change *************************************************");
            //console.log("Current Data Group ID: " + jsonCurrentContact.SecurityGroups[numCurrentAssignedGroup].GroupId);
            //console.log("New Data Group ID: " + jsonSecGroups[numSecGroup].GroupId);
            //console.log("******************************************************************************");
            jsonCurrentContact.SecurityGroups[numCurrentAssignedGroup].GroupId = jsonSecGroups[numSecGroup].GroupId;
          }
          if (changeTimezone === true) {
            //console.log("********** TimeZone Change ***************************************************");
            //console.log("Current Timezone Name: " + jsonCurrentContact.timezonename);
            //console.log("New Timezone Name: " + smxTimeZoneName);
            //console.log("******************************************************************************");
            //set the timezone to the SOMAX timezone
            jsonCurrentContact.timezonename = smxTimeZoneName;
            jsonCurrentContact.timezone = smxTimeZone;
          }
          if (changeTimezone === true || changeSecGroup === true || createUser === true) {
            //console.log("New User, or Timezone change, or a Security Group change....");
            //Save changes must include the Pin
            jsonCurrentContact.Pin = userPassVal;
            //Save the contact changes
            SaveContactJson = {
              "sessionToken": Session_Token,
              "user": jsonCurrentContact,
              "withpin": true
            };
            //console.log("..about to call SaveUser() method from AlertResponse()");

            SaveUser(Session_Token); //also logs out Admin
          }
          //loginAction(userName, userPassVal, false);

        }
      }
    }
      //Error
    else {
      //alert(msg.ErrorMessage);
    }
  }
  
    function CreateUser(session_tkn) {

        var sessiontkn = { "sessionToken": session_tkn };
        $.ajax({
            url: apiUrl + '/api/Contact/NewContact',
            type: 'POST',
            data: JSON.stringify(sessiontkn),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: true,
            success: function (msg) {
                SaveUser(session_tkn);
            },
            error: function (msg, ajaxOptions, thrownError) {
                alertResponse(thrownError);
            }
        });
    }

  function SaveUser(session_tkn) {
    if (typeof SaveContactJson === "undefined" || SaveContactJson === null) {
      //create new user
      SaveContactJson = setContact(session_tkn);
    }
    $.ajax({
      url: apiUrl + '/api/Contact/SaveContact',
      type: 'POST',
      data: JSON.stringify(SaveContactJson),
      contentType: 'application/json; charset=utf-8',
      dataType: 'json',
      async: false,
      success: function (msg) {
        var objNewContact = msg;
        if (objNewContact.ErrorCode == 0) {
          //save action did not return an error
          //console.log("Save User Completed for: " + objNewContact.FullName +" (" + objNewContact.LoginName + ")");
          DoLogout(session_tkn);
        } else if (objNewContact.ErrorCode == 8101) {
          //Login name already in use 
          //console.log("Login name already in use : " + objNewContact.FullName +" (" + objNewContact.LoginName + ")");
          DoLogout(session_tkn);
        } else {
          //show error
          //console.log(objNewContact.ErrorMessage);
        }
      },
      error: function (msg, ajaxOptions, thrownError) {
        //console.log("alertResponse being called from SaveUser() - error" );
        alertResponse(thrownError);
      }
    });
  }

    function DoLogout(token) {

        var sessiontkn = { "sessionToken": token };
        $.ajax({

            url: apiUrl + '/api/Login/DoLogout',
            type: 'POST',
            data: JSON.stringify(sessiontkn),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: true,
            success: function (msg) {

                loginAction(userName, userPassVal, false);
            },
            error: function (msg, ajaxOptions, thrownError) {
                alertResponse(thrownError);
            }
        });
    }


  function GetSecurityGroups(session_tkn) {

    var sessiontkn = {
      "sessionToken": session_tkn
    };
    $.ajax({
      url: apiUrl + '/api/Group/GetAllSecurityGroups',
      type: 'POST',
      data: JSON.stringify(sessiontkn),
      contentType: 'application/json; charset=utf-8',
      dataType: 'json',
      async: false,
      success: function (secgroups) {
        jsonSecGroups = secgroups;
      },
      error: function (msg, ajaxOptions, thrownError) {
        //console.log("alertResponse being called from GetSecurityGroups()" );
        alertResponse(thrownError);
      }
    });
  }
});
