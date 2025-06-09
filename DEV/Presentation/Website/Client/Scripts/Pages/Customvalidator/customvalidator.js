(function ($) {
    var matched, browser;
    jQuery.uaMatch = function (ua) {
        ua = ua.toLowerCase();

        var match = /(chrome)[ \/]([\w.]+)/.exec(ua) ||
            /(webkit)[ \/]([\w.]+)/.exec(ua) ||
            /(opera)(?:.*version|)[ \/]([\w.]+)/.exec(ua) ||
            /(msie) ([\w.]+)/.exec(ua) ||
            ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(ua) ||
            [];
        return {
            browser: match[1] || "",
            version: match[2] || "0"
        };
    };
    matched = jQuery.uaMatch(navigator.userAgent);
    browser = {};

    if (matched.browser) {
        browser[matched.browser] = true;
        browser.version = matched.version;
    }

    if (browser.chrome) {
        browser.webkit = true;
    } else if (browser.webkit) {
        browser.safari = true;
    }
    jQuery.browser = browser;
    $.validator.addMethod("filesize", function (value, element, params) {
        var retVal = true;
        if (value != null) {
            var file = getNameFromPath(value);
            if (file != null) {
                var size = GetFileSize(element.id);
                if (size > params.maxsize) {
                    retVal = false;
                }
                else {
                    retVal = true;
                }
            }
        }
        return retVal;
    });
    $.validator.unobtrusive.adapters.add("Filesize", ['maxsize'], function (options) {
        options.rules['filesize'] = {
            maxsize: options.params.maxsize
        };
        options.messages['filesize'] = options.message;
    }
    );
    $.validator.unobtrusive.adapters.add('requiredif', ['dependentproperty', 'desiredvalue'], function (options) {
        options.rules['requiredif'] = options.params;
        options.messages['requiredif'] = options.message;
    });
    $.validator.addMethod('requiredif', function (value, element, parameters) {
        var desiredvalue = parameters.desiredvalue;
        desiredvalue = (desiredvalue == null ? '' : desiredvalue).toString();
        var controlType = $("input[id$='" + parameters.dependentproperty + "']").attr("type");
        var actualvalue = {}
        if (controlType == "checkbox" || controlType == "radio") {
            var control = $("input[id$='" + parameters.dependentproperty + "']:checked");
            actualvalue = control.val();
        } else {
            actualvalue = $("#" + parameters.dependentproperty).val();
        }
        if ($.trim(desiredvalue).toLowerCase() === $.trim(actualvalue).toLocaleLowerCase()) {
            var isValid = $.validator.methods.required.call(this, value, element, parameters);
            return isValid;
        }
        return true;
    });
    $.validator.unobtrusive.adapters.add('requiredifvalueexist', ['dependentproperty'], function (options) {
        options.rules['requiredifvalueexist'] = options.params;
        options.messages['requiredifvalueexist'] = options.message;
    });
    $.validator.addMethod('requiredifvalueexist', function (value, element, parameters) {
        var controlType = $("input[id$='" + parameters.dependentproperty + "']").attr("type");
        var actualvalue = {}
        if (controlType == "checkbox" || controlType == "radio") {
            var control = $("input[id$='" + parameters.dependentproperty + "']:checked");
            actualvalue = control.val();
        } else {
            actualvalue = $("#" + parameters.dependentproperty).val();
        }
        if ($.trim(actualvalue).toLocaleLowerCase() !== "") {
            var isValid = $.validator.methods.required.call(this, value, element, parameters);
            return isValid;
        }
        return true;
    });

    $.validator.unobtrusive.adapters.add('greaterthanorequalto', ['dependentproperty'], function (options) {
        options.rules['greaterthanorequalto'] = options.params;
        options.messages['greaterthanorequalto'] = options.message;
    });
    $.validator.addMethod('greaterthanorequalto', function (value, element, parameters) {
        var dependentproperty = $("#" + parameters.dependentproperty).val();
        var retVal = true;
        if (!value) value = 0;
        if (dependentproperty) {
            if (parseFloat(value) < parseFloat(dependentproperty)) {
                retVal = false;
            }
            else {
                retVal = true;
            }
        }
        return retVal;
    });

    $.validator.unobtrusive.adapters.add('requiredifvalueexistthengreaterthanzeroattribute', ['dependentproperty'], function (options) {
        options.rules['requiredifvalueexistthengreaterthanzeroattribute'] = options.params;
        options.messages['requiredifvalueexistthengreaterthanzeroattribute'] = options.message;
    });
    $.validator.addMethod('requiredifvalueexistthengreaterthanzeroattribute', function (value, element, parameters) {
        var dependentproperty = $("#" + parameters.dependentproperty).val();
        var retVal = true;
        if (dependentproperty) {
            if (value) {
                if (parseFloat(value) < 1) {
                    retVal = false;
                }
                else {
                    retVal = true;
                }
            }
            else {
                retVal = false;
            }
        }
        return retVal;
    });
    $.validator.unobtrusive.adapters.add('notrequiredifvalueexists', ['otherfield'], function (options) {
        options.rules['notrequiredifvalueexists'] = options.params;
        options.messages['notrequiredifvalueexists'] = options.message;
    });
    $.validator.addMethod('notrequiredifvalueexists', function (value, element, parameters) {
        var otherval = $("#" + parameters.otherfield).val();
        if (value != 0 && otherval != 0) {
            return false;
        }
        return true;
    });

    $.validator.unobtrusive.adapters.add('validatepinforusa', ['dependentproperty'], function (options) {
        options.rules['validatepinforusa'] = options.params;
        options.messages['validatepinforusa'] = options.message;
    });
    $.validator.addMethod('validatepinforusa', function (value, element, parameters) {
        var dependentproperty = $("#" + parameters.dependentproperty).val();
        var retVal = true;
        if (dependentproperty && dependentproperty == "USA") {
            if (value) {
                var pattern = /^\d{5}$|^\d{5}-\d{4}$/;
                retVal = $.trim(value).match(pattern) ? true : false;
                return retVal;
            }
            else {
                retVal = false;
            }
        }
        return retVal;
    });

    $.validator.unobtrusive.adapters.add('requiredsecurityprofileforuser', ['dependentproperty'], function (options) {
        options.rules['requiredsecurityprofileforuser'] = options.params;
        options.messages['requiredsecurityprofileforuser'] = options.message;
    });
    $.validator.addMethod('requiredsecurityprofileforuser', function (value, element, parameters) {
        var dependentproperty = $("#" + parameters.dependentproperty).val();
        var retVal = true;
        if (dependentproperty && dependentproperty == "Full") {
            if (value) {
                retVal = true;
                return retVal;
            }
            else {
                retVal = false;
            }
        }
        return retVal;
    });



    $.validator.unobtrusive.adapters.add('futuredatevalidationattribute', function (options) {
        options.rules['futuredatevalidationattribute'] = options.params;
        options.messages['futuredatevalidationattribute'] = options.message;
    });
    $.validator.addMethod('futuredatevalidationattribute', function (value, element, parameters) {
        var retVal = true;
        var CurrentDate = new Date().setHours(0, 0, 0, 0);
        var GivenDate = new Date(value).setHours(0, 0, 0, 0);

        if (CurrentDate > GivenDate) {
            retVal = false;
        }
        else {
            retVal = true;
        }
        return retVal;
    });


    // The unlike function

    $.validator.unobtrusive.adapters.add(
        'unlike', ['property1', 'property2'], function (options) {
            var params = {
                property1: options.params.property1,
                property2: options.params.property2
            };
            options.rules['unlike'] = params;
            options.messages['unlike'] = options.message;
        });
    $.validator.addMethod(
        'unlike',
        function (value, element, params) {
            var retVal = true;
            if (!this.optional(element)) {
                var property1 = $('#' + params.property1);
                if (params.property2) {
                    var property2 = $('#' + params.property2);
                    if (property1.val() == value || property2.val() == value) { retVal = false; }
                }
                else {
                    if (property1.val() == value) { retVal = false; }
                }
            }
            return retVal;
        });


    // The minlengthpassword function
    $.validator.unobtrusive.adapters.add(
        'minlengthpassword', ['passwordreqminlengthproperty', 'passwordminlengthproperty'], function (options) {
            var params = {
                passwordreqminlengthproperty: options.params.passwordreqminlengthproperty,
                passwordminlengthproperty: options.params.passwordminlengthproperty
            };
            options.rules['minlengthpassword'] = params;
            options.messages['minlengthpassword'] = options.message;
        });
    $.validator.addMethod(
        'minlengthpassword',
        function (value, element, params) {
            var retVal = true;
            
            var passwordreqminlength = $('#' + params.passwordreqminlengthproperty).val();
            var passwordminlength = $('#' + params.passwordminlengthproperty).val();
            
            if (passwordreqminlength && passwordminlength && passwordreqminlength.toLowerCase() === "true"
                && parseInt(passwordminlength) > 0 && value) {
                var valLength = value.length;
                if (valLength < parseInt(passwordminlength)) {
                    var msg = this.settings.messages[element.name]['minlengthpassword'];
                    if (msg && msg.indexOf("{0}") > -1) {
                        this.settings.messages[element.name]['minlengthpassword'] = msg.replace("{0}", passwordminlength);                      
                    }                    
                    retVal = false;
                }
            }
            return retVal;
        });

    // The RequireNumberAttribute function
    $.validator.unobtrusive.adapters.add(
        'requirenumber', ['requirenumberproperty'], function (options) {
            options.rules['requirenumber'] = options.params;
            options.messages['requirenumber'] = options.message;
        });
    $.validator.addMethod(
        'requirenumber',
        function (value, element, params) {
            var retVal = true;

            var requirenumber = $('#' + params.requirenumberproperty).val();

            if (requirenumber && requirenumber.toLowerCase() === "true" && value) {
                var hasNumber = /\d/;
                if (!hasNumber.test(value)) {
                    retVal = false;
                }
            }
            return retVal;
        });

    // The RequireAlphaCharacterAttribute function
    $.validator.unobtrusive.adapters.add(
        'requirealphacharacter', ['requirealphacharacterproperty'], function (options) {
            options.rules['requirealphacharacter'] = options.params;
            options.messages['requirealphacharacter'] = options.message;
        });
    $.validator.addMethod(
        'requirealphacharacter',
        function (value, element, params) {
            var retVal = true;

            var requirealphacharacter = $('#' + params.requirealphacharacterproperty).val();

            if (requirealphacharacter && requirealphacharacter.toLowerCase() === "true" && value) {
                var hasCharacter = /[a-zA-Z]/;
                if (!hasCharacter.test(value)) {
                    retVal = false;
                }
            }
            return retVal;
        });

    // The RequireMixedcaseCharacterAttribute function
    $.validator.unobtrusive.adapters.add(
        'requiremixedcasecharacter', ['requiremixedcasecharacterproperty'], function (options) {
            options.rules['requiremixedcasecharacter'] = options.params;
            options.messages['requiremixedcasecharacter'] = options.message;
        });
    $.validator.addMethod(
        'requiremixedcasecharacter',
        function (value, element, params) {
            var retVal = true;

            var requiremixedcasecharacter = $('#' + params.requiremixedcasecharacterproperty).val();

            if (requiremixedcasecharacter && requiremixedcasecharacter.toLowerCase() === "true" && value) {
                var lowerCharacter = /[a-z]/;
                var upperCharacter = /[A-Z]/;
                if (!lowerCharacter.test(value) || !upperCharacter.test(value)) {
                    retVal = false;
                }
            }
            return retVal;
        });

    // The RequireSpecialcaseCharacterAttribute function
    $.validator.unobtrusive.adapters.add(
        'requirespecialcasecharacter', ['requireppecialcasepharacterproperty'], function (options) {
            options.rules['requirespecialcasecharacter'] = options.params;
            options.messages['requirespecialcasecharacter'] = options.message;
        });
    $.validator.addMethod(
        'requirespecialcasecharacter',
        function (value, element, params) {
            var retVal = true;

            var requirespecialcasecharacter = $('#' + params.requireppecialcasepharacterproperty).val();

            if (requirespecialcasecharacter && requirespecialcasecharacter.toLowerCase() === "true" && value) {
                //var hasCharacter = new Array('~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '{', '}', '|', '[', ']', '\\', ':', ';', '<', '>', '?', ',', '.', '/');
                //for (var i = 0; i < hasCharacter.length; i++) {
                //    if (value.indexOf(hasCharacter[i]) >= 0) {
                //        return true;
                //    }
                //}
                var hasCharacter = /[!@#$%^&*()`~+{}|:;<>?,./\[\]\\]/;
                if (!hasCharacter.test(value)) {
                    var msg = this.settings.messages[element.name]['requirespecialcasecharacter'];
                    this.settings.messages[element.name]['requirespecialcasecharacter'] = msg.replace("pipesymbol", "|");
                    retVal = false;
                }                
            }
            return retVal;
        });

    // The NoRepeatedCharactersAttribute function
    $.validator.unobtrusive.adapters.add(
        'norepeatedcharacters', ['repeatedcharactersproperty'], function (options) {
            options.rules['norepeatedcharacters'] = options.params;
            options.messages['norepeatedcharacters'] = options.message;
        });
    $.validator.addMethod(
        'norepeatedcharacters',
        function (value, element, params) {
            var retVal = true;

            var norepeatedcharacters = $('#' + params.repeatedcharactersproperty).val();

            if (norepeatedcharacters && norepeatedcharacters.toLowerCase() === "true" && value) {
                //var valLength = value.length;
                //var distinctValue = "";
                //for (var i = 0; i < valLength; i++) {
                //    if (distinctValue.indexOf(value[i]) === -1) {
                //        distinctValue = distinctValue + "" + value[i];
                //    }
                //}
                //if (valLength !== distinctValue.length) {
                //    return false;
                //} 
                //var hasRepeatCharacter = /([\w\W]).*?\1/;
                var repeatChar = /([\w\W])\1\1/; // 3 or more then 3 times
                if (repeatChar.test(value.toLowerCase())) {
                    retVal = false;
                }    
            }
            return retVal;
        });

    // The PasswordNotEqualUsernameAttribute function
    $.validator.unobtrusive.adapters.add(
        'passwordnotequalusername', ['passwordnotequalusernameproperty', 'usernameproperty'], function (options) {
            options.rules['passwordnotequalusername'] = options.params;
            options.messages['passwordnotequalusername'] = options.message;
        });
    $.validator.addMethod(
        'passwordnotequalusername',
        function (value, element, params) {
            var retVal = true;

            var passwordnotequalusername = $('#' + params.passwordnotequalusernameproperty).val();
            var username = $('#' + params.usernameproperty).val();

            if (passwordnotequalusername && username && passwordnotequalusername.toLowerCase() === "true" && value) {
                if (value.toLowerCase() === username.toLowerCase()) {
                    retVal = false;
                }
            }
            return retVal;
        });

}(jQuery));
function GetFileSize(fileid) {
    try {
        var fileSize = 0;
        //for IE
        if ($.browser.msie) {
            var objFSO = new ActiveXObject("Scripting.FileSystemObject"); var filePath = $("#" + fileid)[0].value;
            var objFile = objFSO.getFile(filePath);
            var fileSize = objFile.size; //size in kb
            fileSize = fileSize / 1048576; //size in mb
        }
        //for FF, Safari, Opeara and Others
        else {
            fileSize = $("#" + fileid)[0].files[0].size //size in kb
            fileSize = fileSize / 1048576; //size in mb
        }
        return fileSize;
    }
    catch (e) {
        alert("Error is :" + e);
    }
}
function getNameFromPath(strFilepath) {
    var objRE = new RegExp(/([^\/\\]+)$/);
    var strName = objRE.exec(strFilepath);
    if (strName == null) {
        return null;
    }
    else {
        return strName[0];
    }
}

//---RequiredAsPerUI---
$.validator.unobtrusive.adapters.add("requiredasperui", ["propname", "viewname", "isexternal"], function (options) {
    options.rules["requiredasperui"] = options.params;
    options.messages["requiredasperui"] = options.message;
});
var vunameold;
$.validator.addMethod("requiredasperui", function (value, element, params) {
    var prpname = params.propname;
    var retVal = true;
    var vuname = $(document).find("#ViewName").val();
    var isextern = "";
    if ($(document).find("#IsExternal").length > 0) {
        isextern = $(document).find("#IsExternal").val();
    }
    if (vunameold != vuname) {
        vunameold = vuname;
        validateControls = [];
    }
    if (validateControls.length < 1) {
        getAllViews(vuname, isextern);
    }
    var isdisable = "";
    if ($(document).find("#DisabledVal").length > 0) {
        isdisable = $(document).find("#DisabledVal").val();
    }
    if (isdisable !== "") {
        $.each(validateControls, function (i, j) {
            if (j.ColumnName == prpname) {
                if (j.Required == true && j.Hide == false) {
                    if (value == null || value == "") {
                        retVal = false;
                    }
                }
            }
        })
    }
    else {
        $.each(validateControls, function (i, j) {
            if (j.ColumnName == prpname) {
                if (j.Required == true && j.Hide == false && j.Disable == false) {
                    if (value == null || value == "") {
                        retVal = false;
                    }
                }
            }
        })
    }
    return retVal;
});

//#region End date greater than Start date
$.validator.unobtrusive.adapters.add('isgreater', ['otherproperty'], function (options) {
    options.rules['isgreater'] = { otherproperty: options.params.otherproperty };
    options.messages['isgreater'] = options.message;
});

$.validator.addMethod("isgreater", function (value, element, param) {
    var otherProp = $('#' + param.otherproperty);
    if (otherProp.val() != '') {
        var StartDate = new Date(moment(otherProp.val(), 'MM/DD/YYYY'));

        var Enddate = new Date(value);
        if (value != "") {
            if (StartDate != '') {
                return Enddate > StartDate;
            }
        }
        
    }
    return true;
});
//#endregion

//#region date  between start date aand end date
$.validator.unobtrusive.adapters.add('betweenstartdateandenddate', ['otherproperty1','otherproperty2'], function (options) {
    options.rules['betweenstartdateandenddate'] = options.params;
    options.messages['betweenstartdateandenddate'] = options.message;
});

$.validator.addMethod("betweenstartdateandenddate", function (value, element, param) {
    var retVal = true;
    var otherProp1 = $('#' + param.otherproperty1);
    var otherProp2 = $('#' + param.otherproperty2);
    if (otherProp1.val() != '' && otherProp2.val() != '') {
        var StartDate = new Date(moment(otherProp1.val(), 'MM/DD/YYYY'));
        var EndDate = new Date(moment(otherProp2.val(), 'MM/DD/YYYY'));

        var Validdate = new Date(moment(value, 'MM/DD/YYYY')); 
        if ((StartDate != '' && Validdate >= StartDate) && (EndDate != '' && Validdate <= EndDate) ) {
            retVal = true;
        }
        else
        {
            retVal = false;
        }
    }
    return retVal;
});
//#endregion

//#region greater than validation V2-798
$.validator.unobtrusive.adapters.add('greaterthan', ['dependentproperty'], function (options) {
    options.rules['greaterthan'] = options.params;
    options.messages['greaterthan'] = options.message;
});
$.validator.addMethod('greaterthan', function (value, element, parameters) {
    var dependentproperty = $("#" + parameters.dependentproperty).val();
    var retVal = true;
    if (!value) value = 0;
    if (dependentproperty) {
        if (parseFloat(value) <= parseFloat(dependentproperty)) {
            retVal = false;
        }
        else {
            retVal = true;
        }
    }
    return retVal;
});
//#endregion

//#region greater than validation V2-1196
$.validator.unobtrusive.adapters.add('greaterthanforpartautopurchasingconfiguration', ['dependentproperty'], function (options) {
    options.rules['greaterthanforpartautopurchasingconfiguration'] = options.params;
    options.messages['greaterthanforpartautopurchasingconfiguration'] = options.message;
});
$.validator.addMethod('greaterthanforpartautopurchasingconfiguration', function (value, element, parameters) {
    let dependentproperty = $("#" + parameters.dependentproperty).val();
    let isChecked = $(document).find("#partsConfigureAutoPurchasingModel_IsAutoPurchase").prop("checked");
 
    let retVal = true;
    if (isChecked) {
        if (!value) value = 0;
        if (dependentproperty) {
                
                if (parseFloat(value) <= parseFloat(dependentproperty)) {
                    retVal = false;
                }
        }
    }
    return retVal;
});
$.validator.unobtrusive.adapters.add('lessthanforpartautopurchasingconfiguration', ['dependentproperty'], function (options) {
    options.rules['lessthanforpartautopurchasingconfiguration'] = options.params;
    options.messages['lessthanforpartautopurchasingconfiguration'] = options.message;
});
$.validator.addMethod('lessthanforpartautopurchasingconfiguration', function (value, element, parameters) {
    let dependentproperty = $("#" + parameters.dependentproperty).val();
    let isChecked = $(document).find("#partsConfigureAutoPurchasingModel_IsAutoPurchase").prop("checked");

    let retVal = true;
    if (isChecked) {
        if (!value) value = 0;
        if (dependentproperty) {
         
            if (parseFloat(value) >= parseFloat(dependentproperty)) {
                retVal = false;
            }
        }
    }
    return retVal;
});
//#endregion