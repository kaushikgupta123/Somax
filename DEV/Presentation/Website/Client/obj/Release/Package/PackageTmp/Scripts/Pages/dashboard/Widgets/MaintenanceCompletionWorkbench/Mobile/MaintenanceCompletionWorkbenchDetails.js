//#region Variables
var maintenanceAttachmentTable;
var PartsTable;
var dtXrefPartsTable;
var PrtClientLookupId = "";
var PrtDescription = "";
var PrtUPCcode = "";
var PrtManufacturer = "";
var PrtManufacturerId = "";
var PrtStockType = "";
var laborDtTable;
var laborSecurity;
var equipmentid = -1;
//#endregion 

//#region Details

$(document).on('click', '.wo-det-tab', function (e) {
    if ($(this).hasClass('active')) {
        return false;
    }
    ResetOtherTabs();
    var tab = $(this).data('tab');
    loadTabDetails(tab);
    SwitchTab(e, tab);
});
function loadTabDetails(tab) {
    switch (tab) {
        case "Labor":
            LoadLaborTab();
            break;
        case "Parts":
            LoadPartsListTab();
            break;
        case "Tasks":
            LoadTasksTab();
            break;
        case "Instructions":
            LoadInstructionsTab();
            break;
        case "Comments":
            LoadCommentsTab();
            break;
        case "Downtime":
            LoadDowntimeTab();
            break;
        case "Photos":
            LoadPhotosTab();
            break;
        case "Attachments":
            LoadAttachmentTab();
            break;
        case "MaterialRequests":
            LoadMaterialRequestTab();
            break;
    }
}
function SwitchTab(evt, tab) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(tab).style.display = "block";

    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
    $(document).find('.wo-det-tab').removeClass('mbsc-ms-item-sel');
    $(document).find('#' + tab).addClass('mbsc-ms-item-sel');
}
function ResetOtherTabs() {
    $(document).find('#Labor').html('');
    $(document).find('#Parts').html('');
    $(document).find('#Tasks').html('');
    $(document).find('#Instructions').html('');
    $(document).find('#Comments').html('');
    $(document).find('#Attachments').html('');
    $(document).find('#MaterialRequests').html('');
}
//#endregion 

//#region Action drop down

//#region AddOndemandWorkOrder
$(document).on('click', "#showUnplannedWoPopup_Mobile", function (e) {
    $('#UnplannedWo_Mobile').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', "#hideUnplannedWoPopup_Mobile", function (e) {
    $('#UnplannedWo_Mobile').removeClass('slide-active');
});
$(document).on('click', "#emergencyOnDemandPopUpPageID", function (e) {
    //var workOrderId = parseInt($(document).find('#WorkOrderId').val());
    //var clientLookupId = $(document).find('#WorkOrderClientLookupId').val();
    e.preventDefault();
    $('#UnplannedWo_Mobile').removeClass('slide-active');
    GoOnDemand();
    //$('#UnplannedWo_Mobile').modal('hide');
    //$('.modal-backdrop').remove();
});
function GoOnDemand() {
    $.ajax({
        url: "/Dashboard/AddOnDemandWO_Mobile",
        type: "POST",
        dataType: "html",
        //data: { WorkoderId: workOrderID, ClientLookupId: clientLookupId }, sweta
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddOnDemandWOModalpopup').html(data);
            //$('#AddOnDemandWOModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            //$("#imgChargeToTree").css("display", "none");
            $("#woEmergencyOnDemandModel_ChargeTo").attr('disabled', 'disabled');
            $('#AddOnDemandWOModalpopup').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', "#openemerwogrid", function () {
    generateEquipmentDataTable_Mobile();
});

$(document).on('click', '.btnCancelOnDemand', function () {
    //StatusArray = [];
    var areaChargeToId = "";
    $(document).find('#AddOnDemandWOModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
    $('#AddOnDemandWOModalpopup').removeClass('slide-active');
    $('#AddOnDemandWOModalpopup').html('');
});
//$(document).on('hidden.bs.modal', '#AddOnDemandWOModalpopup', function () {
//    $('#OnDemandWOPopUp').html('');
//});
//#endregion

//#region Add Describe Work order
$(document).on('click', "#emergencyDescribePopUpPageID", function (e) {
    //var workOrderId = parseInt($(document).find('#WorkOrderId').val());
    //var clientLookupId = $(document).find('#WorkOrderClientLookupId').val();
    e.preventDefault();
    $('#UnplannedWo_Mobile').removeClass('slide-active');
    GoEmergencyDescribe();
    //$('#UnplannedWo_Mobile').modal('hide');
    //$('.modal-backdrop').remove();

});
function GoEmergencyDescribe() {
    $.ajax({
        //url: "/Dashboard/AddDescribeWO_Mobile",
        url: "/Dashboard/AddDescribeWO_MobileDynamic",
        type: "POST",
        dataType: "html",
        //data: { WorkoderId: workOrderID, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            //$('#AddDescribeWOModalpopup').html(data);
            $('#AddDescribeWOModalpopupDynamic').html(data);
            //$('#AddDescribeWOModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            $('.dtpicker').mobiscroll().calendar({
                display: 'bottom',
                touchUi: true,
                weekDays: 'min',
                yearChange: false,
                min: new Date(),
                months: 1
            }).inputmask('mm/dd/yyyy');
            $('.decimal').mobiscroll().numpad({
                touchUi: true,
                //min: 0.01,
                //max: 99999999.99,
                scale: 2,
                preset: 'decimal',
                thousandsSeparator: ''
            });
            //$('#AddDescribeWOModalpopup').addClass('slide-active').trigger('mbsc-enhance');
            $('#AddUWOModalDialog').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#opendescribewogrid", function () {
    generateEquipmentDataTable_Mobile();
});


$(document).on('click', '.btnCancelDescribe', function () {
    //$("#AddDescribeWOModalpopup").modal('hide');
    //StatusArray = [];
    var areaChargeToId = "";
    $(document).find('#AddDescribeWOModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
    $('#AddDescribeWOModalpopup').removeClass('slide-active');
    $('#AddDescribeWOModalpopup').html('');
});

//$(document).on('hidden.bs.modal', '#AddDescribeWOModalpopup', function () {
//    $('#DescribeWOPopUp').html('');
//});
//#endregion

//#region AddFollowUpWorkOrder
$(document).on('click', "#actionAddFollowUpWO", function (e) {
    var workOrderId = parseInt($(document).find('#WorkOrderId').val());
    var clientLookupId = $(document).find('#WorkOrderClientLookupId').val();
    $.ajax({
        url: "/Dashboard/AddFollowUpWO_Mobile",
        type: "GET",
        dataType: 'html',
        data: {
            'WorkoderId': workOrderId, 'ClientLookupId': clientLookupId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#FollowupWOPopUp').html(data);
            //$('#AddFollowupWOModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            //$(document).find('.select2picker').select2({});
            //$.validator.setDefaults({ ignore: null });
            //$.validator.unobtrusive.parse(document);
            //$('input, form').blur(function () {
            //    $(this).valid();
            //});
            $('#AddFollowupWOModalpopup').parent().addClass('slide-active');
            $('#AddFollowupWOModalpopup').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', '#imgChargeToTree', function (e) {
    $(this).blur();
    generateEquipmentTree($(this).data('assignedid'));
});

function generateEquipmentTree(assignedid) {
    $.ajax({
        url: '/PlantLocationTree/EquipmentHierarchyTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        cache: false,
        beforeSend: function () {
            ShowLoader();
            $(document).find(".cntTree").html("<b>Processing...</b>");
        },
        success: function (data) {
            $(document).find(".cntTree").html('');
            $(document).find(".cntTree").html(data);
        },
        complete: function () {
            CloseLoader();
            treeTable($(document).find(".cntTree").find('#tblTree'));
            $(document).find('.radSelect').each(function () {
                //if ($(document).find('#hdnChargeTo').val() == '0' || $(document).find('#hdnChargeTo').val() == '') {

                //    if ($(this).data('equipmentid') === equipmentid) {
                //        $(this).attr('checked', true);
                //    }
                //}
                //else {
                var c_id = $(this).data('clientlookupid').split("(")[0];
                if (c_id != null && c_id.trim() == $(document).find('#' + assignedid).val()) {
                    $(this).attr('checked', true);
                }
                //}
            });
            $.each($(document).find(".cntTree").find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAAsTAAALEwEAmpwYAAAKT2lDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAHjanVNnVFPpFj333vRCS4iAlEtvUhUIIFJCi4AUkSYqIQkQSoghodkVUcERRUUEG8igiAOOjoCMFVEsDIoK2AfkIaKOg6OIisr74Xuja9a89+bN/rXXPues852zzwfACAyWSDNRNYAMqUIeEeCDx8TG4eQuQIEKJHAAEAizZCFz/SMBAPh+PDwrIsAHvgABeNMLCADATZvAMByH/w/qQplcAYCEAcB0kThLCIAUAEB6jkKmAEBGAYCdmCZTAKAEAGDLY2LjAFAtAGAnf+bTAICd+Jl7AQBblCEVAaCRACATZYhEAGg7AKzPVopFAFgwABRmS8Q5ANgtADBJV2ZIALC3AMDOEAuyAAgMADBRiIUpAAR7AGDIIyN4AISZABRG8lc88SuuEOcqAAB4mbI8uSQ5RYFbCC1xB1dXLh4ozkkXKxQ2YQJhmkAuwnmZGTKBNA/g88wAAKCRFRHgg/P9eM4Ors7ONo62Dl8t6r8G/yJiYuP+5c+rcEAAAOF0ftH+LC+zGoA7BoBt/qIl7gRoXgugdfeLZrIPQLUAoOnaV/Nw+H48PEWhkLnZ2eXk5NhKxEJbYcpXff5nwl/AV/1s+X48/Pf14L7iJIEyXYFHBPjgwsz0TKUcz5IJhGLc5o9H/LcL//wd0yLESWK5WCoU41EScY5EmozzMqUiiUKSKcUl0v9k4t8s+wM+3zUAsGo+AXuRLahdYwP2SycQWHTA4vcAAPK7b8HUKAgDgGiD4c93/+8//UegJQCAZkmScQAAXkQkLlTKsz/HCAAARKCBKrBBG/TBGCzABhzBBdzBC/xgNoRCJMTCQhBCCmSAHHJgKayCQiiGzbAdKmAv1EAdNMBRaIaTcA4uwlW4Dj1wD/phCJ7BKLyBCQRByAgTYSHaiAFiilgjjggXmYX4IcFIBBKLJCDJiBRRIkuRNUgxUopUIFVIHfI9cgI5h1xGupE7yAAygvyGvEcxlIGyUT3UDLVDuag3GoRGogvQZHQxmo8WoJvQcrQaPYw2oefQq2gP2o8+Q8cwwOgYBzPEbDAuxsNCsTgsCZNjy7EirAyrxhqwVqwDu4n1Y8+xdwQSgUXACTYEd0IgYR5BSFhMWE7YSKggHCQ0EdoJNwkDhFHCJyKTqEu0JroR+cQYYjIxh1hILCPWEo8TLxB7iEPENyQSiUMyJ7mQAkmxpFTSEtJG0m5SI+ksqZs0SBojk8naZGuyBzmULCAryIXkneTD5DPkG+Qh8lsKnWJAcaT4U+IoUspqShnlEOU05QZlmDJBVaOaUt2ooVQRNY9aQq2htlKvUYeoEzR1mjnNgxZJS6WtopXTGmgXaPdpr+h0uhHdlR5Ol9BX0svpR+iX6AP0dwwNhhWDx4hnKBmbGAcYZxl3GK+YTKYZ04sZx1QwNzHrmOeZD5lvVVgqtip8FZHKCpVKlSaVGyovVKmqpqreqgtV81XLVI+pXlN9rkZVM1PjqQnUlqtVqp1Q61MbU2epO6iHqmeob1Q/pH5Z/YkGWcNMw09DpFGgsV/jvMYgC2MZs3gsIWsNq4Z1gTXEJrHN2Xx2KruY/R27iz2qqaE5QzNKM1ezUvOUZj8H45hx+Jx0TgnnKKeX836K3hTvKeIpG6Y0TLkxZVxrqpaXllirSKtRq0frvTau7aedpr1Fu1n7gQ5Bx0onXCdHZ4/OBZ3nU9lT3acKpxZNPTr1ri6qa6UbobtEd79up+6Ynr5egJ5Mb6feeb3n+hx9L/1U/W36p/VHDFgGswwkBtsMzhg8xTVxbzwdL8fb8VFDXcNAQ6VhlWGX4YSRudE8o9VGjUYPjGnGXOMk423GbcajJgYmISZLTepN7ppSTbmmKaY7TDtMx83MzaLN1pk1mz0x1zLnm+eb15vft2BaeFostqi2uGVJsuRaplnutrxuhVo5WaVYVVpds0atna0l1rutu6cRp7lOk06rntZnw7Dxtsm2qbcZsOXYBtuutm22fWFnYhdnt8Wuw+6TvZN9un2N/T0HDYfZDqsdWh1+c7RyFDpWOt6azpzuP33F9JbpL2dYzxDP2DPjthPLKcRpnVOb00dnF2e5c4PziIuJS4LLLpc+Lpsbxt3IveRKdPVxXeF60vWdm7Obwu2o26/uNu5p7ofcn8w0nymeWTNz0MPIQ+BR5dE/C5+VMGvfrH5PQ0+BZ7XnIy9jL5FXrdewt6V3qvdh7xc+9j5yn+M+4zw33jLeWV/MN8C3yLfLT8Nvnl+F30N/I/9k/3r/0QCngCUBZwOJgUGBWwL7+Hp8Ib+OPzrbZfay2e1BjKC5QRVBj4KtguXBrSFoyOyQrSH355jOkc5pDoVQfujW0Adh5mGLw34MJ4WHhVeGP45wiFga0TGXNXfR3ENz30T6RJZE3ptnMU85ry1KNSo+qi5qPNo3ujS6P8YuZlnM1VidWElsSxw5LiquNm5svt/87fOH4p3iC+N7F5gvyF1weaHOwvSFpxapLhIsOpZATIhOOJTwQRAqqBaMJfITdyWOCnnCHcJnIi/RNtGI2ENcKh5O8kgqTXqS7JG8NXkkxTOlLOW5hCepkLxMDUzdmzqeFpp2IG0yPTq9MYOSkZBxQqohTZO2Z+pn5mZ2y6xlhbL+xW6Lty8elQfJa7OQrAVZLQq2QqboVFoo1yoHsmdlV2a/zYnKOZarnivN7cyzytuQN5zvn//tEsIS4ZK2pYZLVy0dWOa9rGo5sjxxedsK4xUFK4ZWBqw8uIq2Km3VT6vtV5eufr0mek1rgV7ByoLBtQFr6wtVCuWFfevc1+1dT1gvWd+1YfqGnRs+FYmKrhTbF5cVf9go3HjlG4dvyr+Z3JS0qavEuWTPZtJm6ebeLZ5bDpaql+aXDm4N2dq0Dd9WtO319kXbL5fNKNu7g7ZDuaO/PLi8ZafJzs07P1SkVPRU+lQ27tLdtWHX+G7R7ht7vPY07NXbW7z3/T7JvttVAVVN1WbVZftJ+7P3P66Jqun4lvttXa1ObXHtxwPSA/0HIw6217nU1R3SPVRSj9Yr60cOxx++/p3vdy0NNg1VjZzG4iNwRHnk6fcJ3/ceDTradox7rOEH0x92HWcdL2pCmvKaRptTmvtbYlu6T8w+0dbq3nr8R9sfD5w0PFl5SvNUyWna6YLTk2fyz4ydlZ19fi753GDborZ752PO32oPb++6EHTh0kX/i+c7vDvOXPK4dPKy2+UTV7hXmq86X23qdOo8/pPTT8e7nLuarrlca7nuer21e2b36RueN87d9L158Rb/1tWeOT3dvfN6b/fF9/XfFt1+cif9zsu72Xcn7q28T7xf9EDtQdlD3YfVP1v+3Njv3H9qwHeg89HcR/cGhYPP/pH1jw9DBY+Zj8uGDYbrnjg+OTniP3L96fynQ89kzyaeF/6i/suuFxYvfvjV69fO0ZjRoZfyl5O/bXyl/erA6xmv28bCxh6+yXgzMV70VvvtwXfcdx3vo98PT+R8IH8o/2j5sfVT0Kf7kxmTk/8EA5jz/GMzLdsAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAHUwAADqYAAAOpgAABdvkl/FRgAAAHFJREFUeNpi/P//PwMlgImBQsA44C6gvhfa29v3MzAwOODRc6CystIRbxi0t7fjDJjKykpGYrwwi1hxnLHQ3t7+jIGBQRJJ6HllZaUUKYEYRYBPOB0gBShKwKGA////48VtbW3/8clTnBIH3gCKkzJgAGvBX0dDm0sCAAAAAElFTkSuQmCC"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find(".cntTree").find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
            //$('#commonWOTreeModal').modal('show');
            $('#commonWOTreeModal').addClass('slide-active');//.trigger('mbsc-enhance');

            $('#tblTree tr td').removeAttr('style');// code to remove the style applied from treetable.js -- white-space: nowrap;
        },
        error: function (xhr) {
            alert('error');
        }
    });
}

$(document).on('change', '.radSelect', function () {
    $(document).find('#hdnChargeTo').val('0');
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid').split("(")[0];
    //$('#commonWOTreeModal').modal('hide');
    $('#commonWOTreeModal').removeClass('slide-active');
    $(document).find('#workOrderModel_ChargeToClientLookupId,#woOnDemandModel_ChargeToClientLookupId,#woDescriptionModel_ChargeToClientLookupId').val(equipmentid).trigger('change');
    $(document).find('#txtChargeTo').val(clientlookupid).trigger('mbsc-enhance');
    $(document).find('#txtChargeTo').closest('form').valid(); //.val(clientlookupid).removeClass('input-validation-error');
});
$(document).on('click', "#commonWOTreeModalHide", function () {
    $('#commonWOTreeModal').removeClass('slide-active');
});
$(document).on('click', "#openfollowwogrid", function () {
    generateEquipmentDataTable_Mobile();
});
function AddFollowUpOnSuccess(data) {
    if (data.Issuccess) {
        //localStorage.setItem("workorderstatus", '3');
        //localStorage.setItem("workorderstatustext", getResourceValue("spnOpenWorkOrder"));
        if (fileExtAddProccess != "") {
            var imgname = data.WorkOrderId + "_" + Math.floor((new Date()).getTime() / 1000);
            CompressImageAddProccess(FilesAddProccess[0], imgname + "." + fileExtAddProccess, data.WorkOrderId);
            fileExtAddProccess = "";
        }
        SuccessAlertSetting.text = getResourceValue("spnFollowUpAddedSuccessfully");
        swal(SuccessAlertSetting, function () {
            RedirectToMaintenanceWorkBenchDetail(data.WorkOrderId, '');
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#AddFollowupWOModalpopup');
    }
    CloseLoader();
}

$(document).on('click', '.btnCancelAddFollowUp', function () {
    //$("#AddFollowupWOModalpopup").modal('hide');
    $('#AddFollowupWOModalpopup').parent().removeClass('slide-active');
    $('#FollowupWOPopUp').html('');
    //StatusArray = [];
    var areaChargeToId = "";
    $(document).find('#AddFollowupWOModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
});
//$(document).on('hidden.bs.modal', '#AddFollowupWOModalpopup', function () {
//    $('#FollowupWOPopUp').html('');
//});
//#endregion 


function WorksEmergencyOnSuccess(data) {
    if (data.data == "success") {
        {
            //localStorage.setItem("workorderstatus", '1');
            //localStorage.setItem("workorderstatustext", (getStatusValue("Scheduled") + " " + getResourceValue("spnWorkOrder")));
            if (fileExtAddProccess != "") {
                var imgname = data.workOrderID + "_" + Math.floor((new Date()).getTime() / 1000);
                CompressImageAddProccess(FilesAddProccess[0], imgname + "." + fileExtAddProccess, data.workOrderID);
                fileExtAddProccess = "";
            }
            SuccessAlertSetting.text = getResourceValue("spnUnplanedWorkorderAddedSuccessfully");
            swal(SuccessAlertSetting, function () {
                if (data.IsToSanitationPage) {
                    var message = getResourceValue("DoesSanitationNeedToInspectAlert");
                    swal({
                        title: "",
                        text: message,
                        html: true,
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonClass: "btn-sm btn-primary",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("CancelAlertYes"),
                        cancelButtonText: getResourceValue("CancelAlertNo")
                    }, function (value) {
                        if (value == true) {
                            //GoSanitationDescribe(data.workOrderID); //need to check why this line exists
                        }
                        else {
                            RedirectToMaintenanceWorkBenchDetail(data.workOrderID, '');
                        }
                    });
                }
                else {
                    //V2-781
                    RedirectToMaintenanceWorkBenchDetail(data.workOrderID, '');
                    ////V2-735
                    //RedirectToWODetail(data.workOrderID);
                }
            });

        }
    }
    else {
        CloseLoader();
        ShowGenericErrorOnAddUpdate(data);
    }
}


//#endregion 

//#region Labor
function LoadLaborTab() {
    $.ajax({
        url: '/Dashboard/LoadLabor_Mobile',
        type: 'POST',
        //data: {
        //    'WorkOrderId': $(document).find('#WorkOrderId').val()
        //},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Labor').html(data);
        },
        complete: function () {
            CloseLoader();
            generateLaborGrid();

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            CloseLoader();
        }
    });
}
function generateLaborGrid() {

    var rCount = 0;
    var workOrderID = $(document).find('#WorkOrderId').val();
    var woCompletionWorkbenchSummary_Status = $(document).find('#woCompletionWorkbenchSummary_Status').val();
    if ($(document).find('#laborDtTable').hasClass('dataTable')) {
        laborDtTable.destroy();
    }
    laborDtTable = $("#laborDtTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Dashboard/PopulateLabor_Mobile?workOrderId=" + workOrderID,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                if (rCount > 0) {
                    $(document).find("#AddLaborbtn").hide();
                }
                else if (woCompletionWorkbenchSummary_Status != "Canceled" && woCompletionWorkbenchSummary_Status != "Denied") {
                    $(document).find("#AddLaborbtn").show();
                }

                laborSecurity = response.security.Access;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                /*visible: false,*/
                "className": "text-center",
                targets: [4], render: function (a, b, data, d) {
                    if (laborSecurity && woCompletionWorkbenchSummary_Status != "Canceled" && woCompletionWorkbenchSummary_Status != "Denied") {
                        return '<a class="btn btn-outline-primary addLaborBtn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editLaborBtn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delLaborBtn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return '';
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "StartDate", "type": "date" },
                { "data": "Hours", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Cost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                //{ "className": "text-center", "bSortable": false }
            ],
        initComplete: function () {
            CloseLoader();
            SetPageLengthMenu();

            if (laborSecurity && woCompletionWorkbenchSummary_Status != "Canceled" && woCompletionWorkbenchSummary_Status != "Denied")
                this.api().column(4).visible(true);
            else
                this.api().column(4).visible(false);
        }
    });
}

$(document).on('click', ".addLaborBtn, #AddLaborbtn", function (e) {
    var workOrderId = parseInt($(document).find('#WorkOrderId').val());
    $.ajax({
        url: "/Dashboard/AddEditLabor_Mobile",
        type: "GET",
        dataType: 'html',

        data: {
            'WorkOrderId': workOrderId,
            'TimeCardId': 0
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#LaborPopUp').html(data);
            //$('#AddLaborModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            BindMobiscrollControlForLaborTab();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//$(document).on('click', '#btnLaborSubmit', function () {
//$(document).on('click', 'button[type="submit"]', function () {
//    if ($(this).closest('form').valid() == false) {
//        $(this).closest('form').find('.input-validation-error').each(function () {
//            $(this).parents('label').eq(0).addClass('mbsc-err');
//        });
//    }
//});
$(document).on('click', '.editLaborBtn', function (e) {
    var row = $(this).parents('tr');
    var data = laborDtTable.row(row).data();
    var timeCardId = data.TimecardId;
    var workOrderId = parseInt($(document).find('#WorkOrderId').val());
    $.ajax({
        url: "/Dashboard/AddEditLabor_Mobile",
        type: "GET",
        dataType: 'html',
        data: {
            'WorkOrderId': workOrderId,
            'TimeCardId': timeCardId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#LaborPopUp').html(data);
            //$('#AddLaborModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            BindMobiscrollControlForLaborTab();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function BindMobiscrollControlForLaborTab() {
    $('#AddLaborModalpopup').parent().addClass("slide-active");
    $('#LaborModel_StartDate').mobiscroll().calendar({
        display: 'bottom',
        touchUi: true,
       /* weekDays: 'min',*/
        yearChange: false,
        max: new Date(),
        /* months: 1,*/
    }).inputmask('mm/dd/yyyy');
    $(document).on("change", "#LaborModel_StartDate", function (e) {
        ValidatePast30DaysDateFromDatePicker($(this).val(), getResourceValue("ValidatePast30DaysDateFromDatePickerAlert"));
    });
    $('#LaborModel_Hours').mobiscroll().numpad({
        touchUi: true,
        //min: 0.01,
       // max: 99999999.99,
        //scale: 2,
       // maxScale: 2,
        //entryMode: 'freeform',
       /* preset: 'decimal',*/
        thousandsSeparator: '',
        entryMode: 'freeform',

        preset: 'decimal',
        scale: 2,
        max: 23.99,
        min: 0.01
    });
    var x = parseFloat($('#LaborModel_Hours').val()) == 0 ? '' : $('#LaborModel_Hours').val();
    $('#LaborModel_Hours').mobiscroll('setVal', x);
    $('#AddLaborModalpopup').trigger('mbsc-enhance');
}
$(document).on('click', '.delLaborBtn', function () {
    var data = laborDtTable.row($(this).parents('tr')).data();
    var TimeCardId = data.TimecardId;

    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Dashboard/DeleteLabor_Mobile',
            data: {
                TimeCardId: TimeCardId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    laborDtTable.state.clear();
                    ShowDeleteAlert(getResourceValue("laborDeleteSuccessAlert"));
                }
            },
            complete: function () {
                swal(SuccessAlertSetting, function () {
                    ShowLoader();
                    laborDtTable.page('first').draw('page');
                    CloseLoader();
                });
                CloseLoader();
            }
        });
    });
});
function LaborAddOnSuccess(data) {
    if (data.Result == "success") {
        $('#AddLaborModalpopup').parent().removeClass("slide-active");
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("LaborAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("LaborUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            ShowLoader();
            laborDtTable.page('first').draw('page');
            CloseLoader();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

// if we are overriding highlight and unhighlight methods then
// adding an error class and valid class should be done manually
function SetControls() {
    var errClass = 'mobile-validation-error';
    CloseLoader();
    $.validator.setDefaults({
        ignore: null,
        //errorClass: "mobile-validation-error", // default values is input-validation-error
        //validClass: "valid", // default values is valid
        highlight: function (element, errorClass, validClass) { //for the elements having error
            $(element).addClass(errClass).removeClass(validClass);
            $(element.form).find("#" + element.id).parent().parent().addClass("mbsc-err");
            var elemName = $(element.form).find("#" + element.id).attr('name');
            $(element.form).find('[data-valmsg-for="' + elemName + '"]').addClass("mbsc-err-msg");
        },
        unhighlight: function (element, errorClass, validClass) { //for the elements having not any error
            $(element).removeClass(errClass).addClass(validClass);
            $(element.form).find("#" + element.id).parent().parent().removeClass("mbsc-err");
            var elemName = $(element.form).find("#" + element.id).attr('name');
            $(element.form).find('[data-valmsg-for="' + elemName + '"]').removeClass("mbsc-err-msg");
        },
    });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        if ($(this).closest('form').length > 0) {
            $(this).valid();
        }
    });
    //$('.mobiscrollselect, form').change(function () {
    //    debugger;
    //    $(this).find('label.mbsc-err').removeClass('mbsc-err');
    //    if ($(this).valid() == false) {
    //        $(this).find('.input-validation-error').each(function () {
    //            $(this).parents('label').eq(0).addClass('mbsc-err');
    //        });
    //    }
    //});
    //$('.select2picker, form').change(function () {
    //$('form').change(function () {
    //    $(this).valid();
    //});
    $(document).find('.select2picker').select2({});
    $(document).find('.mobiscrollselect:not(:disabled)').mobiscroll().select({
        display: 'bubble',
        //touchUi: false,
        /*data: remoteData,*/
        filter: true,
        group: {
            groupWheel: false,
            header: false
        },
        //width: 400,
        //placeholder: 'Please Select...'

    });
    $(document).find('.mobiscrollselect:disabled').mobiscroll().select({
        disabled: true
    });
    SetFixedHeadStyle();
}
$(document).on('click', '.clearstate', function () {
    //StatusArray = [];
    $('#AddLaborModalpopup').parent().removeClass('slide-active');
    var areaChargeToId = "";
    $(document).find('#AddLaborModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
});
//#endregion 

//#region Parts

function LoadPartsListTab() {
    $.ajax({
        url: '/Dashboard/PartsList_Mobile',
        type: 'POST',
        dataType: 'html',
        data: {},
        beforeSend: function () {
            ShowLoader();
        },
        //contentType: 'html',
        success: function (data) {
            if (data) {
                $(document).find('#Parts').html(data);
            }
        },
        complete: function () {
            CloseLoader();
            generatePartsListDataTable();
            $(document).find("#WOCompletionWorkbenchPartsListid").css("display", "block");
        },
        error: function (err) {
            CloseLoader();
        }
    });

}

function generatePartsListDataTable() {
    var workOrderID = parseInt($(document).find('#WorkOrderId').val());
    if ($(document).find('#partsListSearchTable').hasClass('dataTable')) {
        PartsTable.destroy();
    }
    PartsTable = $("#partsListSearchTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Dashboard/GetPartListGrid_Mobile?workOrderId=" + workOrderID,
            "type": "POST",
            "datatype": "json"
        },
        //V2-624
        columnDefs: [
            {
                "targets": [8],
                "visible": false,
                "searchable": false
            }
        ],
        //V2-624
        'rowCallback': function (row, data, dataIndex) {
            var found = SelectedActualPartId.some(function (el) {
                return el.PartHistoryId == data.PartHistoryId;
            });
            if (found) {
                $(row).find('input[type="checkbox"]').prop('checked', true);
            }
        },
        "columns":
            [
                {
                    "data": "PartHistoryId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    'render': function (data, type, full, meta) {
                        return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="chksearchdashboardpart"  value="'
                            + $('<div/>').text(data).html() + '">';
                    }
                },
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                },
                { "data": "TransactionQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Cost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "UnitofMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "TransactionDate",
                    "type": "date "

                },
                { "data": "UPCCode", "autoWidth": true, "bSearchable": false, "bSortable": false }, //V2-624
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
//#endregion




//$(document).on('click', '.openpartgrid', function () {  //Debashis Bose
//    $(".selectpart").addClass("slide-active");
//    $('.selectpart').trigger('mbsc-enhance');
//});

//$(document).on('click', '.selectpart-slide-back-btn', function () { //Debashis Bose
//    $(".selectpart").removeClass("slide-active");
//});


//$(document).on('click', '#AddPartsIssue', function () {  //Debashis Bose
//    $(".AddPartsIssue").addClass("slide-active");
//    $('.AddPartsIssue').trigger('mbsc-enhance');
//});

//$(document).on('click', '.AddPartsIssue-slide-back-btn', function () { //Debashis Bose
//    $(".AddPartsIssue").removeClass("slide-active");
//});


//#region Add Part Issue
$(document).on('change', '#StoreroomId', function () {
    if ($('#StoreroomId').val() == '') {
        $(document).find('#AddCompletionWorkbenchPartIssueModal #openpartgrid').closest('.mbsc-col-12').hide();
        $(document).find('#AddCompletionWorkbenchPartIssueModal #btnQrScanner').closest('.mbsc-col-12').hide();
    }
    else {
        $(document).find('#AddCompletionWorkbenchPartIssueModal #openpartgrid').closest('.mbsc-col-12').show();
        $(document).find('#AddCompletionWorkbenchPartIssueModal #btnQrScanner').closest('.mbsc-col-12').show();
    }
    $(document).find('#txtpartid,#PartIssueAddModel_PartId').val('');
});
$(document).on('click', '#AddPartsIssue', function () {
    $.ajax({
        url: '/Dashboard/AddPartIssue_Mobile',
        type: 'POST',
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data != '') {
                $('#divAddParts').html(data);
            }
        },
        complete: function () {
            if ($('#divAddParts').html() != '') {
                SetControls();
                BindMobiscrollControlForPartsTab();
            }
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
    //$(document).find('#AddCompletionWorkbenchPartIssueModal').modal({ backdrop: 'static', keyboard: false, show: true });
});
function BindMobiscrollControlForPartsTab() {
    $('#AddCompletionWorkbenchPartIssueModal').addClass("slide-active").trigger('mbsc-enhance');
    $('#PartIssueAddModel_Quantity').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 99999999.99,
        //fill: 'ltr',
        maxScale: 2,
        preset: 'decimal',
        thousandsSeparator: '',
        entryMode: 'freeform'
    });
    var x = parseFloat($('#PartIssueAddModel_Quantity').val()) == 0 ? '' : $('#PartIssueAddModel_Quantity').val();
    $('#PartIssueAddModel_Quantity').mobiscroll('setVal', x);
}
$(document).on('click', '#openpartgrid', function () {
    //generatePartsXrefDataTable();
    //$('#maintenancepartIdModal').addClass("slide-active");//.trigger('mbsc-enhance');
    generatePartScrollViewForMobileMobiscroll();
});
function generatePartScrollViewForMobileMobiscroll() {
    PartListlength = 0;
    //var SearchVal = $(document).find('#txtPartSearch_Mobile').val();
    $.ajax({
        "url": "/Dashboard/PartLookupListView_Mobile",
        type: "POST",
        dataType: "html",
        //data: {
        //    Search: SearchVal,
        //    //Description: SearchVal
        //    Storeroomid: ($('#UseMultiStoreroom').val() == 'True' ? $('#StoreroomId').val() : '')
        //},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DivPartSearchScrollViewModal').html(data);
        },
        complete: function () {
            //BindPartScrollViewOfMobiScroll();
            InitializePartListView_Mobile();
            //if (!$(document).find('#maintenancepartIdModal').hasClass('show')) {
            //    $(document).find('#maintenancepartIdModal').modal("show");
            //}
            //$(document).find('#txtPartSearch_Mobile').val(SearchVal);
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
var PartListView,
    PartListlength = 0,
    PartPageLength = 100;
function InitializePartListView_Mobile() {
    PartListView = $('#PartListViewForSearch').mobiscroll().listview({
        theme: 'ios',
        themeVariant: 'light',
        animateAddRemove: false,
        striped: true,
        swipe: false
    }).mobiscroll('getInst');
    BindPartDataForListView();
    $('#maintenancepartIdModal').addClass('slide-active');
    //$('#EquipmentWOModal').trigger('mbsc-enhance');
}
$(document).on('click', '#btnPartLoadMore', function () {
    $(this).hide();
    PartListlength += PartPageLength;
    InitializePartListView_Mobile();
});
function BindPartDataForListView() {
    var Search = $(document).find('#txtPartSearch_Mobile').val();
    $.ajax({
        "url": "/Dashboard/GetPartLookupListchunksearch_Mobile",
        data: {
            Search: Search,
            Start: PartListlength,
            Length: PartPageLength,
            Storeroomid: ($('#UseMultiStoreroom').val() == 'True' ? $('#StoreroomId').val() : '')
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
            ShowLoader();
            PartListView.showLoading();
        },
        success: function (data) {
            var i, item, lihtml;
            for (i = 0; i < data.data.length; ++i) {
                item = data.data[i];
                lihtml = '';
                lihtml = '<li class="scrollview-partsearch" data-id="' + item.PartId + '"; data-clientlookupid="' + item.ClientLookUpId + '">';
                lihtml = lihtml + "" + item.ClientLookUpId + ' (' + item.Description + ')</li>';
                PartListView.add(null, mobiscroll.$(lihtml));
            }
            if ((PartListlength + PartPageLength) < data.recordsTotal) {
                $('#btnPartLoadMore').show();
            }
            else {
                $('#btnPartLoadMore').hide();
            }
        },
        complete: function () {
            PartListView.hideLoading();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//function BindPartScrollViewOfMobiScroll() {
//    var $categoryNav,
//        $contentView

//    $categoryNav = $('#part-md-category').mobiscroll().nav({
//        type: 'tab',
//        onItemTap: function (event, inst) {
//            $contentView.mobiscroll('navigate', $('.' + $(event.target).data('page')));
//        }

//    });

//    $contentView = $('#part-md-content').mobiscroll().scrollview({
//        layout: 1,                                  // More info about layout: https://docs.mobiscroll.com/4-10-9/scrollview#opt-layout
//        paging: true,                               // More info about paging: https://docs.mobiscroll.com/4-10-9/scrollview#opt-paging
//        threshold: 15,                              // More info about threshold: https://docs.mobiscroll.com/4-10-9/scrollview#opt-threshold
//        cssClass: 'md-page',
//        onAnimationStart: function (event, inst) {  // More info about onAnimationStart: https://docs.mobiscroll.com/4-10-9/scrollview#event-onAnimationStart
//            var selectedIndex = parseInt((-(event.destinationX / inst.contWidth)).toString(), 10),
//                $selectedItem = $categoryNav.find('li').eq(selectedIndex);

//            if (!$selectedItem.hasClass('mbsc-ms-item-sel')) {
//                $categoryNav.mobiscroll('navigate', $selectedItem);
//            }
//        }
//    });

//    $('.part-md-list').mobiscroll().listview({
//        swipe: false,
//        striped: true,
//        enhance: true,
//    });
//    $('#maintenancepartIdModal').addClass("slide-active").trigger('mbsc-enhance');
//}
$(document).on('click', '#maintenancepartIdModalHide', function () {
    $(document).find('#maintenancepartIdModal').removeClass("slide-active"); //..modal("hide");
    $('#txtPartSearch_Mobile').val('');
    $('#DivPartSearchScrollViewModal').html('');
});
//$(document).on('hidden.bs.modal', '#maintenancepartIdModal', function () {
//    $('#txtPartSearch_Mobile').val('');
//    $('#DivPartSearchScrollViewModal').html('');
//});
$(document).on("keyup", '#txtPartSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        generatePartScrollViewForMobileMobiscroll();
    }
});
//function generatePartsXrefDataTable() {
//    var EquipmentId = $('#partsSessionData_EquipmentId').val();
//    var rCount = 0;
//    var visibility;
//    if ($(document).find('#XrefPartsTable').hasClass('dataTable')) {
//        dtXrefPartsTable.destroy();
//    }
//    mcxDialog.loading({ src: "../content/Images" });
//    dtXrefPartsTable = $("#XrefPartsTable").DataTable({
//        searching: true,
//        serverSide: true,
//        "pagingType": "full_numbers",
//        "bProcessing": true,
//        "bDeferRender": true,
//        "order": [[0, "asc"]],
//        "pageLength": 10,
//        stateSave: true,
//        language: {
//            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
//        },
//        sDom: 'Btlipr',
//        buttons: [],
//        "orderMulti": true,
//        "ajax": {
//            "url": "/Base/GetPartLookupListchunksearch",
//            data: function (d) {
//                d.ClientLookupId = PrtClientLookupId;
//                d.Description = PrtDescription;
//                d.UPCcode = PrtUPCcode;
//                d.Manufacturer = PrtManufacturer;
//                d.ManufacturerId = PrtManufacturerId;
//                d.StockType = PrtStockType;
//            },
//            "type": "POST",
//            "datatype": "json",
//            "dataSrc": function (json) {
//                rCount = json.data.length;
//                return json.data;
//            }
//        },
//        "columns":
//            [
//                {
//                    "data": "ClientLookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true,
//                    "mRender": function (data, type, row) {
//                        return '<a class=link_xrefpart_detail href="javascript:void(0)">' + data + '</a>'
//                    }
//                },
//                {
//                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
//                    mRender: function (data, type, full, meta) {
//                        return "<div class='text-wrap width-300'>" + data + "</div>";
//                    }
//                },
//                {
//                    "data": "UPCcode", "autoWidth": true, "bSearchable": true, "bSortable": true,
//                    "mRender": function (data, type, row) {
//                        return '<span class="m-badge--custom">' + data + '</span>';
//                    }
//                },
//                {
//                    "data": "Manufacturer", "autoWidth": true, "bSearchable": true, "bSortable": true,
//                    "mRender": function (data, type, row) {
//                        return '<span class="m-badge--dot-custom">' +
//                            '</span > &nbsp; <span class="m--font-custom" >' + data + '</span>';
//                    }
//                },
//                { "data": "ManufacturerID", "autoWidth": true, "bSearchable": true, "bSortable": true },
//                {
//                    "data": "StockType", "autoWidth": true, "bSearchable": true, "bSortable": true,
//                    "mRender": function (data, type, row) {
//                        return '<span class="m-badge--custom">' + data + '</span>'
//                    }
//                },
//            ],
//        "rowCallback": function (row, data, index, full) {
//            var colManufacturer = this.api().column(2).index('visible');
//            if (data.Manufacturer) {
//                var color = "#" + intToARGB(hashCode(LRTrim(data.Manufacturer)));
//                $('td', row).eq(colManufacturer).find('.m-badge--dot-custom').css('background-color', color).css('color', '#fff');
//                $('td', row).eq(colManufacturer).find('.m--font-custom').css('color', color);
//            }
//            var colStockType = this.api().column(4).index('visible');
//            if (data.StockType) {
//                var color = "#" + intToARGB(hashCode(LRTrim(data.StockType)));
//                $('td', row).eq(colStockType).find('.m-badge--custom').css('background-color', color).css('color', '#fff');
//            }
//        },
//        initComplete: function () {
//            $(document).find('#tblpartfooter').show();
//            mcxDialog.closeLoading();
//            SetPageLengthMenu();
//            if (!$(document).find('#maintenancepartIdModal').hasClass('show')) {
//                $(document).find('#maintenancepartIdModal').modal("show");
//            }
//            $(document).find('#XrefPartsTable tfoot th').each(function (i, v) {
//                var colIndex = i;
//                var title = $('#XrefPartsTable thead th').eq($(this).index()).text();
//                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="eqp_part_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
//                if (PrtClientLookupId) { $('#eqp_part_colindex_0').val(PrtClientLookupId); }
//                if (PrtDescription) { $('#eqp_part_colindex_1').val(PrtDescription); }
//                if (PrtUPCcode) { $('#eqp_part_colindex_2').val(PrtUPCcode); }
//                if (PrtManufacturer) { $('#eqp_part_colindex_3').val(PrtManufacturer); }
//                if (PrtManufacturerId) { $('#eqp_part_colindex_4').val(PrtManufacturerId); }
//                if (PrtStockType) { $('#eqp_part_colindex_5').val(PrtStockType); }

//            });

//            $('#XrefPartsTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
//                if (e.keyCode == 13) {
//                    var thisId = $(this).attr('id');
//                    var colIdx = thisId.split('_')[1];
//                    var searchText = LRTrim($(this).val());
//                    PrtClientLookupId = $('#eqp_part_colindex_0').val();
//                    PrtDescription = $('#eqp_part_colindex_1').val();
//                    PrtUPCcode = $('#eqp_part_colindex_2').val();
//                    PrtManufacturer = $('#eqp_part_colindex_3').val();
//                    PrtManufacturerId = $('#eqp_part_colindex_4').val();
//                    PrtStockType = $('#eqp_part_colindex_5').val();
//                    dtXrefPartsTable.page('first').draw('page');
//                }
//            });
//        }
//    });
//}

$(document).on('click', '.scrollview-partsearch', function (e) {
    var WOid = $(document).find('#WorkOrderId').val();
    var WOLookupid = $(document).find('#WorkOrderClientLookupId').val();
    //var index_row = $('#XrefPartsTable tr').index($(this).closest('tr')) - 1;
    //var row = $(this).parents('tr');
    //var td = $(this).parents('tr').find('td');
    //var data = dtXrefPartsTable.row(row).data();
    $(document).find('#txtpartid').val($(this).data('clientlookupid')).trigger('mbsc-enhance');//.removeClass('input-validation-error');
    $(document).find('#txtpartid').closest('form').valid();
    //$(document).find('#txtpartid').val(data.ClientLookUpId);
    $(document).find('#PartIssueAddModel_PartId').val($(this).data('id'));
    //$(document).find('#PartIssueAddModel_ClientLookUpId').val(data.ClientLookUpId);
    $(document).find('#PartIssueAddModel_WorkOrderId').val(WOid);
    $(document).find('#PartIssueAddModel_WorkOrderClientLookupId').val(WOLookupid);
    $(document).find('#maintenancepartIdModal').removeClass("slide-active");
});

//function SetControls() {
//    CloseLoader();
//    $.validator.setDefaults({ ignore: null });
//    $.validator.unobtrusive.parse(document);
//    $('input, form').blur(function () {
//        $(this).valid();
//    });
//    $('.select2picker, form').change(function () {
//        var areaddescribedby = $(this).attr('aria-describedby');
//        if ($(this).valid()) {
//            if (typeof areaddescribedby != 'undefined') {
//                $('#' + areaddescribedby).hide();
//            }
//        }
//        else {
//            if (typeof areaddescribedby != 'undefined') {
//                $('#' + areaddescribedby).show();
//            }
//        }
//    });
//    $(document).find('.select2picker').select2({});
//    SetFixedHeadStyle();
//}
function PartIssueAddOnSuccess(data) {
    $('#AddCompletionWorkbenchPartIssueModal').removeClass("slide-active");
    $("#AddCompletionWorkbenchPartIssueModal").find("input,select").val('').end();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("PartIssueAddAlert");
        swal(SuccessAlertSetting, function () {
            ShowLoader();
            //generatePartsListDataTable();
            PartsTable.page('first').draw('page');
            CloseLoader();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', "#btnPartIssuecancel,#closeprtissue", function (e) {
    $(document).find("#AddCompletionWorkbenchPartIssueModal").removeClass("slide-active");
    //$(document).find("#txtpartid").removeClass("input-validation-error");
    //$(document).find('form').find("#PartIssueAddModel_Quantity").removeClass("input-validation-error");
    $(document).find('.mbsc-err').removeClass('mbsc-err');
    $(document).find('.mbsc-err-msg').html('');
    //$("#AddCompletionWorkbenchPartIssueModal")
    //    .find("input,select")
    //    .val('')
    //    .end();//.modal("hide");
    $('#divAddParts').html('');
})
//#endregion

//#region Tasks

var Wotasktotalcount = 0;
function LoadTasksTab() {
    $.ajax({
        url: '/Dashboard/TaskList_Mobile',
        type: 'POST',
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data) {
                $(document).find('#Tasks').html(data);
            }
        },
        complete: function () {
            CloseLoader();
            generateWoTaskGrid();
        },
        error: function (err) {
            CloseLoader();
        }
    });
}

var woTaskTable;
$(document).on('click', "#brdwotask", function () {
    var workOrderID = $(this).attr('data-val');
    RedirectToPmDetail(workOrderID);
});
function generateWoTaskGrid() {
    var workOrderID = $(document).find('#WorkOrderId').val();
    var rwotaskCount = 0;
    $(document).find('#select-all-wotask').prop('checked', false);
    if ($(document).find('#woTaskTable').hasClass('dataTable')) {
        woTaskTable.destroy();
    }
    woTaskTable = $("#woTaskTable").DataTable({
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[1, "asc"]],
        stateSave: false,
        dom: 'Btlipr',
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        buttons: [],
        "filter": true,
        "orderMulti": true,
        "ajax": {
            "url": "/Dashboard/PopulateTask_Mobile?workOrderId=" + workOrderID,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rwotaskCount = response.data.length;
                Wotasktotalcount = response.recordsTotal;
                return response.data;
            }
        },
        columnDefs: [{
            "data": "WorkOrderTaskId",
            orderable: false,
            className: 'select-checkbox dt-body-center',
            targets: 0,
            'render': function (data, type, full, meta) {
                if ($('#select-all-wotask').is(':checked') && Wotasktotalcount == TaskIdsToupdate.length) {
                    return '<input type="checkbox"  checked="checked" name="id[]" data-eqid="' + data + '" class="isSelect" value="' + $('<div/>').text(data).html() + '">';
                }
                else {
                    if (TaskIdsToupdate.indexOf(data) != -1) {
                        return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="isSelect ' + data + '"  value="'
                            + $('<div/>').text(data).html() + '">';
                    }
                    else {
                        return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="isSelect" value="' + $('<div/>').text(data).html() + '">';
                    }
                }
            }
        },
        ],
        "columns":
            [
                {},
                { "data": "TaskNumber", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "0" },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-150'>" + data + "</div>";
                    }
                },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" }

            ],
        initComplete: function () {
            if (rwotaskCount > 0) {
                $("#btnwoCompletetask").show();
                $("#btnwoCanceltask").show();
            }
            else {
                $("#btnwoCompletetask").hide();
                $("#btnwoCanceltask").hide();
            }
            SetPageLengthMenu();
            CloseLoader();
        }
    });
}
var TaskIdsToupdate = [];
var equiToClientLookupIdbarcode = [];
$(document).on('change', '.isSelect', function () {
    var data = woTaskTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#select-all-wotask').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        var index = TaskIdsToupdate.indexOf(data.WorkOrderTaskId);
        TaskIdsToupdate.splice(index, 1);

    }
    else {
        TaskIdsToupdate.push(data.WorkOrderTaskId);
    }

    if (TaskIdsToupdate.length == Wotasktotalcount) {
        $(document).find('.dt-body-center').find('#select-all-wotask').prop('checked', 'checked');
    }
    else {
        $(document).find('.dt-body-center').find('#select-all-wotask').prop('checked', false);
    }
});
$(document).on('click', '#select-all-wotask', function () {
    var checked = this.checked;
    if (checked) {
        var workOrderID = $(document).find('#WorkOrderId').val();
        TaskIdsToupdate = [];
        $.ajax({
            "url": "/Dashboard/PopulateTaskIds_Mobile?workOrderId=" + workOrderID,
            async: true,
            type: "GET",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data) {
                    $.each(data, function (index, item) {
                        TaskIdsToupdate.push(item.WorkOrderTaskId);
                    });
                }
                else {
                    TaskIdsToupdate = [];
                }
            },
            complete: function () {
                woTaskTable.column(0).nodes().to$().each(function (index, item) {
                    $(this).find('.isSelect').prop('checked', 'checked');
                });
                CloseLoader();
            }
        });
    }
    else {
        $(document).find('.isSelect').prop('checked', false);
        TaskIdsToupdate = [];
    }
});
$(document).on('click', '#btnwoCompletetask', function () {
    var workOrderID = $(document).find('#WorkOrderId').val();
    var taskIds = null;
    taskIds = TaskIdsToupdate.join();
    if (taskIds !== "") {
        $.ajax({
            url: '/Dashboard/CompleteTask_Mobile',
            data: { taskList: taskIds, workOrderId: workOrderID },
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == "success" && data.successcount > 0) {
                    var msg = data.successcount + ' ' + getResourceValue("taskCompleteAlert");
                    ShowSuccessAlert(msg);
                    TaskIdsToupdate = [];

                    swal(SuccessAlertSetting, function () {
                        ShowLoader();
                        woTaskTable.page('first').draw('page');
                        $(document).find('.dt-body-center').find('#select-all-wotask').prop('checked', false);
                        CloseLoader();
                    });
                }
                else {
                    $(document).find('.isSelect').prop('checked', false);
                    $(document).find('#select-all-wotask').prop('checked', false);
                    var msg = getResourceValue("taskNotCompleteAlert");
                    swal({
                        title: getResourceValue("taskNotCompleteAlert"),
                        text: msg,
                        type: "error",
                        showCancelButton: false,
                        confirmButtonClass: "btn-sm btn-danger",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("SaveAlertOk"),
                        cancelButtonText: getResourceValue("CancelAlertNo")
                    });
                    TaskIdsToupdate = [];
                }
                $(document).find('.isSelect').prop('checked', false);
                $(document).find('#select-all-wotask').prop('checked', false);
            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        swal({
            title: getResourceValue("taskNotSelectedAlert"),
            text: getResourceValue("taskSelectAlert"),
            type: "warning",
            confirmButtonClass: "btn-sm btn-primary",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        });
        return false;
    }
});
var taskIDscancel;
$(document).on('click', '#btnwoCanceltask', function () {
    $(document).find('#dashboardWoTaskModel_CancelReason').val('').trigger('change');
    taskIDscancel = null;
    taskIDscancel = TaskIdsToupdate.join();
    if (taskIDscancel !== "") {
        //$.ajax({
        //    beforeSend: function () {
        //        ShowLoader();
        //    },
        //    success: function (data) {
        //        $('#cancelWOTaskModal').modal('show');

        //    },
        //    complete: function () {
        //        $(document).find('.select2picker').select2({});
        //        CloseLoader();
        //        $('#cancelWOTaskModal').modal('hide');
        //    },
        //    error: function () {
        //        CloseLoader();
        //    }
        //});
        //$(document).find('.select2picker').select2({});
        SetControls();
        //$('#cancelWOTaskModal').modal('show');
        $('#cancelWOTaskModal').addClass('slide-active').trigger('mbsc-enhance');
    }
    else {
        $(document).find('.isSelect').prop('checked', false);
        $(document).find('#select-all-wotask').prop('checked', false);
        //$('#cancelWOTaskModal').modal('hide');
        $('#cancelWOTaskModal').removeClass('slide-active');
        swal({
            title: getResourceValue("taskNotSelectedAlert"),
            text: getResourceValue("taskSelectAlert"),
            type: "warning",
            confirmButtonClass: "btn-sm btn-primary",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        });
        return false;
    }
});
$(document).on('click', '#btnWoTaskCancelOk', function () {
    var cancelreason = $(document).find('#dashboardWoTaskModel_CancelReason').val();
    $.ajax({
        url: '/Dashboard/CancelTask_Mobile',
        data: { taskList: taskIDscancel, cancelReason: cancelreason },
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            //$('#cancelWOTaskModal').modal('hide');
            $('#cancelWOTaskModal').removeClass('slide-active');
            if (data.data == "success") {
                var msg = data.successcount + ' ' + getResourceValue("taskCancelAlert");
                ShowSuccessAlert(msg);
                TaskIdsToupdate = [];

                swal(SuccessAlertSetting, function () {
                    ShowLoader();
                    woTaskTable.page('first').draw('page');
                    $(document).find('.dt-body-center').find('#select-all-wotask').prop('checked', false);
                    CloseLoader();
                });
            }
            else {
                $(document).find('.isSelect').prop('checked', false);
                $(document).find('#select-all-wotask').prop('checked', false);
                var msg = getResourceValue("taskNotCancelAlert");
                swal({
                    title: getResourceValue("taskCancelFailedAlert"),
                    text: msg,
                    type: "error",
                    showCancelButton: false,
                    confirmButtonClass: "btn-sm btn-danger",
                    cancelButtonClass: "btn-sm",
                    confirmButtonText: getResourceValue("SaveAlertOk"),
                    cancelButtonText: getResourceValue("CancelAlertNo")
                });
            }
            TaskIdsToupdate = [];
        },
        complete: function () {
            CloseLoader();
            //$('#cancelWOTaskModal').modal('hide');
            $('#cancelWOTaskModal').removeClass('slide-active');
        },
        error: function () {
            CloseLoader();
        }
    });
});
//$(document).on('click', '#btnWoTaskCancel,.cancel-modal-close', function () {
$(document).on('click', '.btnWoTaskCancel', function () {
    //$('#cancelWOTaskModal').modal('hide');
    $('#cancelWOTaskModal').removeClass('slide-active');
    TaskIdsToupdate = [];
    $(document).find('.isSelect').prop('checked', false);
    $(document).find('#select-all-wotask').prop('checked', false);
});


$(document).on("change", "#woTaskModel_ChargeType", function () {
    $(document).find('#txtChargeTo').val('');
    var textChargeToId = $("#woTaskModel_ChargeType option:selected").val();
});
//#endregion

//#region Instructions
function LoadInstructionsTab() {
    $.ajax({
        url: '/Dashboard/LoadIntructions_Mobile',
        type: 'POST',
        data: {
            'WorkOrderId': $(document).find('#WorkOrderId').val()
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Instructions').html(data);
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion 

//#region Comments
var colorarray = [];
function colorobject(string, color) {
    this.string = string;
    this.color = color;
}
function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}
function LoadCommentsTab() {
    var WorkOrderId = $(document).find('#WorkOrderId').val();
    $.ajax({
        url: '/Dashboard/GetCommentsDetails_Mobile',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Comments').html('');
            $(document).find('#Comments').html(data);
            LoadComments(WorkOrderId);
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

}
function LoadComments(WorkOrderId) {
    $.ajax({
        "url": "/Dashboard/LoadComments_Mobile",
        data: { WorkOrderId: WorkOrderId },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var getTexttoHtml = textToHTML(data);
            $(document).find('#commentstems').html(getTexttoHtml);
            $(document).find("#commentsList").mCustomScrollbar({
                theme: "minimal"
            });
        },
        complete: function () {
            var ftext = '';
            var bgcolor = '';
            $(document).find('#commentsdataloader').hide();
            $(document).find('#commentstems').find('.comment-header-item').each(function () {
                var thistext = LRTrim($(this).text());
                if (ftext == '' || ftext != thistext) {
                    var bgcolorarr = colorarray.filter(function (a) {
                        return a.string == thistext;
                    });
                    if (bgcolorarr.length == 0) {
                        bgcolor = getRandomColor();
                        var thisval = new colorobject(thistext, bgcolor);
                        colorarray.push(thisval);
                    }
                    else {
                        bgcolor = bgcolorarr[0].color;
                    }
                }
                $(this).attr('style', 'color:' + bgcolor + '!important;border:1px solid' + bgcolor + '!important;');
                ftext = LRTrim($(this).text());
            });
            var loggedinuserinitial = LRTrim($('#hdr-comments-add').text());
            var avlcolor = colorarray.filter(function (a) {
                return a.string == loggedinuserinitial;
            });
            if (avlcolor.length == 0) {
                $('#hdr-comments-add').attr('style', 'border:1px solid #264a7c !important;').show();
            }
            else {
                $('#hdr-comments-add').attr('style', 'color:' + avlcolor[0].color + ' !important;border:1px solid ' + avlcolor[0].color + '!important;').show();
            }
            $('.kt-notes__body a').attr('target', '_blank');
            CloseLoader();
        }
    });
}
$(document).on("focus", "#wotxtcommentsnew", function () {
    $(document).find('.ckeditorarea').show();
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.ckeditorarea').hide();
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    //ClearEditor();
    LoadCkEditor('wotxtcomments');
    $("#wotxtcommentsnew").hide();
    $(".ckeditorfield").show();
});
$(document).on('click', '#btnsavecommands', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    var workorderId = $(document).find('#WorkOrderId').val();
    var woClientLookupId = $(document).find('#WorkOrderClientLookupId').val();
    var noteId = 0;
    if (LRTrim(data) == "") {
        return false;
    }
    else {
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/Dashboard/AddComments_Mobile',
            type: 'POST',
            beforeSend: function () {
                ShowLoader();
            },
            data: {
                workOrderId: workorderId,
                content: data,
                woClientLookupId: woClientLookupId,
                userList: selectedUsers,
                noteId: noteId,
            },
            success: function (data) {
                if (data.Result == "success") {
                    var message;
                    if (data.mode == "add") {
                        SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                    }
                    else {
                        SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                    }
                    swal(SuccessAlertSetting, function () {
                        LoadCommentsTab();
                    });
                }
                else {
                    ShowGenericErrorOnAddUpdate(data);
                    CloseLoader();
                }
            },
            complete: function () {
                ClearEditor();
                $("#wotxtcommentsnew").show();
                $(".ckeditorfield").hide();
                selectedUsers = [];
                selectedUnames = [];
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', '#commandCancel', function () {
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    ClearEditor();
    $("#wotxtcommentsnew").show();
    $(".ckeditorfield").hide();
});
$(document).on('click', '.editcomments', function () {
    $(document).find(".ckeditorarea").each(function () {
        $(this).html('');
    });
    $("#wotxtcommentsnew").show();
    $(".ckeditorfield").hide();
    var notesitem = $(this).parents('.kt-notes__item').eq(0);
    notesitem.find('.ckeditorarea').html(CreateEditorHTML('wotxtcommentsEdit'));
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    var rawHTML = $.parseHTML($(this).parents('.kt-notes__item').find('.kt-notes__body').find('.originalContent').html());
    LoadCkEditorEdit('wotxtcommentsEdit', rawHTML);
    $(document).find('.ckeditorarea').hide();
    notesitem.find('.ckeditorarea').show();
    notesitem.find('.kt-notes__body').hide();
    notesitem.find('.commenteditdelearea').hide();
});

$(document).on('click', '.deletecomments', function () {
    DeleteWoNote($(this).attr('id'));
});
$(document).on('click', '.btneditcomments', function () {
    var data = getDataFromTheEditor();
    var workorderId = $(document).find('#WorkOrderId').val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    var woClientLookupId = $(document).find('#WorkOrderClientLookupId').val();
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/Dashboard/AddComments_Mobile',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { workOrderId: workorderId, content: LRTrim(data), noteId: noteId, updatedindex: updatedindex },
        success: function (data) {
            if (data.Result == "success") {
                var message;
                if (data.mode == "add") {
                    SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                }
                else {
                    SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                }
                swal(SuccessAlertSetting, function () {
                    LoadCommentsTab();
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
            }
        },
        complete: function () {
            // ClearEditorEdit();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

});
$(document).on('click', '.btncommandCancel', function () {
    ClearEditorEdit();
    $(document).find('.ckeditorarea').hide();
    $(this).parents('.kt-notes__item').find('.kt-notes__body').show();
    $(this).parents('.kt-notes__item').find('.commenteditdelearea').show();
});
function DeleteWoNote(notesId) {
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: '/Base/DeleteComment',
            data: {
                _notesId: notesId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SuccessAlertSetting.text = getResourceValue("CommentDeleteAlert");
                    swal(SuccessAlertSetting, function () {
                        LoadCommentsTab();
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}

//#endregion 

//#region Attachment
function LoadAttachmentTab() {
    $.ajax({
        url: '/Dashboard/LoadAttachment_Mobile',
        type: 'POST',
        data: {
            'WorkOrderId': $(document).find('#WorkOrderId').val()
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Attachments').html(data);
        },
        complete: function () {
            generateMaintenanceCompletionAttachmentGrid();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generateMaintenanceCompletionAttachmentGrid() {
    //var attchCount = 0;
    if ($(document).find('#MaintenanceAttachmentTable').hasClass('dataTable')) {
        maintenanceAttachmentTable.destroy();
    }
    var workOrderID = $(document).find('#WorkOrderId').val();
    maintenanceAttachmentTable = $("#MaintenanceAttachmentTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Dashboard/PopulateAttachment_Mobile?workOrderId=" + workOrderID,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                //attchCount = response.recordsTotal;
                //if (attchCount > 0) {
                //    $(document).find('#woAttachmentCount').show();
                //    $(document).find('#woAttachmentCount').html(attchCount);
                //}
                //else {
                //    $(document).find('#woAttachmentCount').hide();
                //}
                return response.data;
            }
        },
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "FileName",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_attachment_download href="javascript:void(0)"  target="_blank">' + row.FullName + '</a>';
                    }
                },
                {
                    "data": "CreateDate",
                    "type": "date "
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.lnk_attachment_download', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = maintenanceAttachmentTable.row(row).data();

    window.location = '/Dashboard/DownloadAttachment_Mobile?_fileinfoId=' + data.FileAttachmentId;
});
//#endregion 

//#region Action function
var currenttab;
function RedirectToMaintenanceWorkBenchDetail(WorkOrderId, ClientLookupId) {
    currenttab = $(document).find(".m-portlet").find("button.active").data('tab');
    $.ajax({
        url: "/DashBoard/CompletionWorkbench_Details_Mobile",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { WorkOrderId: WorkOrderId, ClientLookupId: ClientLookupId },
        success: function (data) {
            $('#mainwidget').html(data);

            //$(document).find('#spnlinkToSearch').text(titletext);
        },
        complete: function () {
            SetWOCompletionDetailEnvironment();
            SetFixedHeadStyle();
            CloseLoader();
            //restoring tab details page 
            if (currenttab == '' || currenttab == undefined) {
                if ($('#Labor').length > 0) {
                    currenttab = 'Labor';
                }
                else if ($('#Parts').length > 0) {
                    currenttab = 'Parts';
                }
            }
            $(document).find(".m-portlet").find("button.active").removeClass('active');
            $(document).find(".m-portlet").find("[data-tab='" + currenttab + "']").addClass('active');
            ResetOtherTabs();
            $(document).find("#Labor").removeAttr('style');
            $(document).find("#" + currenttab).css("display", "block");
            $('.modal-backdrop').remove();
            loadTabDetails(currenttab);
            InitWebCam();
        }
    });
}
$(document).on('click', '#actionCancelWO,.wobtngrdcancel', function () {
    $(document).find('#txtCancelReasonSelect').val("").trigger("change.select2");
    $(document).find('#txtcancelcomments').val("").trigger('change');
    //$(document).find('.select2picker').select2({});
    SetControls();
    if ($(document).find('#actionCancelWO').length > 0) {
        $(document).find('#cancelModalDetailsPage').addClass('slide-active').trigger('mbsc-enhance');
    }

});
$(document).on('click', '.cancelModalDetailsPageHide', function () {
    $('#cancelModalDetailsPage').removeClass('slide-active')
});
$(document).on('click', '#cancelWorkOrderJob', function () {
    var cancelreason = $(document).find('#txtCancelReasonSelect').val();
    var comments = $(document).find('#txtcancelcomments').val();
    var WorkOrderId = $(document).find('#WorkOrderId').val();
    $.ajax({
        url: '/Dashboard/CancelJob_Mobile',
        data: { WorkorderId: WorkOrderId, CancelReason: cancelreason, Comments: comments },
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#cancelModalDetailsPage').removeClass('slide-active');
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("WorkOrderCancelsuccessAlert");

                swal(SuccessAlertSetting, function () {

                    RedirectToMaintenanceWorkBenchDetail(WorkOrderId, "");

                });
            }
            else {

            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', '#actionCompleteWO,.wobtngrdcomplete', function () {
    var WorkOrderId = $(document).find('#WorkOrderId').val();

    var hdnIspopupShow = $(document).find('#hdnFoodSafetyPopup').val();
    var htmlMsg = '';
    if (hdnIspopupShow == 'FOOD SERVICES') {
        $.ajax({
            url: '/WorkOrder/GetFoodServicesMessage',
            type: "GET",
            datatype: "json",
            success: function (data) {
                htmlMsg = data.data;
                swal({
                    title: "Food Safety Conditions",
                    text: htmlMsg,
                    html: true,
                    type: "warning",
                    showCancelButton: true,
                    closeOnConfirm: false,
                    confirmButtonClass: "btn-sm btn-primary",
                    cancelButtonClass: "btn-sm",
                    confirmButtonText: getResourceValue("CancelAlertYes"),
                    cancelButtonText: getResourceValue("CancelAlertNo")
                }, function () {
                    CompleteWorkorderJob(WorkOrderId);
                });
            },
            complete: function () {
            },
            error: function () {
            }
        });
    }
    else {
        CompleteWorkorderJob(WorkOrderId);
    }

});

function CompleteWorkorderJob(WorkOrderId) {
    $.ajax({
        url: '/Dashboard/CompleteWorkOrder_Mobile',
        data: {
            workorderId: WorkOrderId
        },
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data == "success") {
                SuccessAlertSetting.text = getResourceValue("WorkOrderCompleteAlert");
                swal(SuccessAlertSetting, function () {
                    RedirectToMaintenanceWorkBenchDetail(WorkOrderId, "");
                });
            }
            else {

            }
        },
        error: function () {
            CloseLoader();
        },
        complete: function () {
            CloseLoader();
        }
    });
}

//#region Add Work Request
$(document).on('click', "#AddWorkRequestBtn_Mobile", function (e) {
    //var workOrderId = parseInt($(document).find('#WorkOrderId').val());
    $.ajax({
        url: "/Dashboard/AddWoRequestDynamicInMaintenanceCompletionWorkbench_Mobile",
        type: "GET",
        dataType: 'html',
        //data: {
        //    'WorkOrderId': workOrderId,
        //    'TimeCardId': 0
        //},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddWorkRequestDiv').html(data);
        },
        complete: function () {
            //$('#AddWorkRequestModalDialog').modal({ backdrop: 'static', keyboard: false, show: true });
            SetControls();
            //$(document).find('.dtpicker').datepicker({
            //    minDate: 0,
            //    changeMonth: true,
            //    changeYear: true,
            //    "dateFormat": "mm/dd/yy",
            //    autoclose: true
            //}).inputmask('mm/dd/yyyy');
            $('.dtpicker').mobiscroll().calendar({
                display: 'bottom',
                touchUi: true,
                weekDays: 'min',
                yearChange: false,
                min: new Date(),
                months: 1
            }).inputmask('mm/dd/yyyy');
            $('.decimal').mobiscroll().numpad({
                touchUi: true,
                //min: 0.01,
                //max: 99999999.99,
                scale: 2,
                preset: 'decimal',
                thousandsSeparator: ''
            });
            $('#AddWorkRequestModalDialog').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
});


//#region Equipment Modal
//var equipTable;
//var eqClientLookupId = "";
//var NameEq = "";
//var Model = "";
//var Type = "";
//var SerialNumber = "";
//var AssetGroup1ClientLookupId = "";
//var AssetGroup2ClientLookupId = "";
//var AssetGroup3ClientLookupId = "";
//var TextField = "";
//var ValueField = "";
//var Equipid = "";

$(document).on('click', '.OpenAssetModalPopupGridoverWorkReqModal', function () {
    //TextField = $(this).data('textfield');
    //ValueField = $(this).data('valuefield');
    //Equipid = $(this).data('equipmentid');
    //generateAssetDataTable();// not using this for mobile
    generateEquipmentDataTable_Mobile();
});

//function generateAssetDataTable() {
//    var rCount = 0;
//    var visibility;
//    if ($(document).find('#EquipmentWOTable').hasClass('dataTable')) {
//        equipTable.destroy();
//    }
//    mcxDialog.loading({ src: "../content/Images" });
//    equipTable = $("#EquipmentWOTable").DataTable({
//        searching: true,
//        serverSide: true,
//        "pagingType": "full_numbers",
//        "bProcessing": true,
//        "bDeferRender": true,
//        "order": [[0, "asc"]],
//        "pageLength": 10,
//        stateSave: true,
//        language: {
//            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
//        },
//        sDom: 'Btlipr',
//        buttons: [],
//        "orderMulti": true,
//        "ajax": {
//            "url": "/Base/GetEquipmentLookupListchunksearch",
//            data: function (d) {
//                d.ClientLookupId = eqClientLookupId;
//                d.Name = NameEq;
//                d.Model = Model;
//                d.Type = Type;
//                d.AssetGroup1ClientLookupId = AssetGroup1ClientLookupId;
//                d.AssetGroup2ClientLookupId = AssetGroup2ClientLookupId;
//                d.AssetGroup3ClientLookupId = AssetGroup3ClientLookupId;
//                d.SerialNumber = SerialNumber;
//                d.InactiveFlag = false;
//            },
//            "type": "POST",
//            "datatype": "json",
//            "dataSrc": function (json) {
//                rCount = json.data.length;
//                return json.data;
//            }
//        },
//        "columns":
//            [
//                {
//                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
//                    "mRender": function (data, type, row) {
//                        return '<a class=link_woeqp_detail_common href="javascript:void(0)">' + data + '</a>'
//                    }
//                },
//                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
//                { "data": "Model", "autoWidth": true, "bSearchable": true, "bSortable": true },
//                {
//                    "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true,
//                    "mRender": function (data, type, row) {
//                        return '<span class="m-badge--custom">' + data + '</span>'
//                    }
//                },

//                { "data": "AssetGroup1ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
//                { "data": "AssetGroup2ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
//                { "data": "AssetGroup3ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
//                { "data": "SerialNumber", "autoWidth": true, "bSearchable": true, "bSortable": true }
//            ],
//        "rowCallback": function (row, data, index, full) {
//            var colType = this.api().column(3).index('visible');
//            if (data.Type) {
//                var color = "#" + intToARGB(hashCode(LRTrim(data.Type)));
//                $('td', row).eq(colType).find('.m-badge--custom').css('background-color', color).css('color', '#fff');
//            }
//        },
//        initComplete: function () {
//            $(document).find('#tblEquipmentWOfooter').show();
//            mcxDialog.closeLoading();
//            SetPageLengthMenu();
//            if (!$(document).find('#EquipmentWOModal').hasClass('show')) {
//                $(document).find('#EquipmentWOModal').modal("show");
//            }
//            $('#EquipmentWOTable tfoot th').each(function (i, v) {
//                var colIndex = i;
//                var title = $('#EquipmentWOTable thead th').eq($(this).index()).text();
//                $(this).html('<input type="text" style="width:100%" class="woequipment tfootsearchtxt" id="woequipmentcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
//                if (eqClientLookupId) { $('#woequipmentcolindex_0').val(eqClientLookupId); }
//                if (NameEq) { $('#woequipmentcolindex_1').val(NameEq); }
//                if (Model) { $('#woequipmentcolindex_2').val(Model); }
//                if (Type) { $('#woequipmentcolindex_3').val(Type); }
//                if (AssetGroup1ClientLookupId) { $('#woequipmentcolindex_4').val(AssetGroup1ClientLookupId); }
//                if (AssetGroup2ClientLookupId) { $('#woequipmentcolindex_5').val(AssetGroup2ClientLookupId); }
//                if (AssetGroup3ClientLookupId) { $('#woequipmentcolindex_6').val(AssetGroup3ClientLookupId); }
//                if (SerialNumber) { $('#woequipmentcolindex_7').val(SerialNumber); }
//            });

//            $('#EquipmentWOTable tfoot th').find('.woequipment').on("keyup", function (e) {
//                if (e.keyCode == 13) {
//                    var thisId = $(this).attr('id');
//                    var colIdx = thisId.split('_')[1];
//                    var searchText = LRTrim($(this).val());
//                    eqClientLookupId = $('#woequipmentcolindex_0').val();
//                    NameEq = $('#woequipmentcolindex_1').val();
//                    Model = $('#woequipmentcolindex_2').val();
//                    Type = $('#woequipmentcolindex_3').val();
//                    AssetGroup1ClientLookupId = $('#woequipmentcolindex_4').val();
//                    AssetGroup2ClientLookupId = $('#woequipmentcolindex_5').val();
//                    AssetGroup3ClientLookupId = $('#woequipmentcolindex_6').val();
//                    SerialNumber = $('#woequipmentcolindex_7').val();
//                    equipTable.page('first').draw('page');
//                }
//            });
//        }
//    });
//}

//$(document).ready(function () {
//    $(window).keydown(function (event) {
//        if (event.keyCode == 13) {
//            event.preventDefault();
//            return false;
//        }
//    });
//});
//$(document).on('click', '.link_woeqp_detail_common', function (e) {
//    var index_row = $('#EquipmentWOTable tr').index($(this).closest('tr')) - 1;
//    var row = $(this).parents('tr');
//    var td = $(this).parents('tr').find('td');
//    var data = equipTable.row(row).data();
//    $(document).find('#' + TextField).val(data.ClientLookupId).css("display", "block");
//    $(document).find('#' + ValueField).val(data.EquipmentId).removeClass('input-validation-error').css("display", "none");
//    $(document).find('#' + ValueField).parent().find('div > button.ClearAssetModalPopupGridData').css('display', 'block');
//    $(document).find("#EquipmentWOModal").modal('hide');
//});
//$(document).on('hidden.bs.modal', '#EquipmentWOModal', function () {
//    TextField = "";
//    ValueField = "";
//});
//$(document).on('click', '.ClearAssetModalPopupGridData', function () {
//    $(document).find('#' + $(this).data('textfield')).val('').css("display", "none");
//    $(document).find('#' + $(this).data('valuefield')).val('').css("display", "block");
//    $(this).css('display', 'none');
//});

function WorkRequestDynamicAddOnSuccess(data) {
    if (data.data === "success") {
        if (data.Command === "save") {
            if (fileExtAddProccess != "") {
                var imgname = data.workOrderID + "_" + Math.floor((new Date()).getTime() / 1000);
                CompressImageAddProccess(FilesAddProccess[0], imgname + "." + fileExtAddProccess, data.workOrderID);
                fileExtAddProccess = "";
            }
           
            SuccessAlertSetting.text = getResourceValue("spnWoRequestAddSuccessfully");
            swal(SuccessAlertSetting, function () {
              
                //V2-735
                // RedirectToMaintenanceWorkBenchDetail(data.workOrderID, '');
                var DashboardId = $('#DashboardlistingId').val();
                if (DashboardId == null || DashboardId == "") {
                    DashboardId = 0;
                }
                window.location = '/Dashboard/RedirectfromDashboardChange?DashboardId=' + DashboardId;
            });
        }
        //V2-928
        //else {
        //    if (fileExtAddProccess != "") {
        //        var imgname = data.workOrderID + "_" + Math.floor((new Date()).getTime() / 1000);
        //        CompressImageAddProccess(FilesAddProccess[0], imgname + "." + fileExtAddProccess, data.workOrderID);
        //        fileExtAddProccess = "";
        //    }
        //    SuccessAlertSetting.text = getResourceValue("spnWoRequestAddSuccessfully");
        //    ResetErrorDiv();
        //    swal(SuccessAlertSetting, function () {
        //        $(document).find('form').trigger("reset");
        //        $(document).find('form').find("select").val("").trigger('change.select2');
        //        $('#wrEquipTreeModalHide').trigger('click');
        //        //$(document).find('form').find("select").removeClass("input-validation-error");
        //        //$(document).find('form').find("textarea").removeClass("input-validation-error");
        //        $(document).find('.mbsc-err').removeClass('mbsc-err');
        //        $(document).find('.mbsc-err-msg').html('');
        //    });
        //}
    }
    else {
        CloseLoader();
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '.btnCancelAddWorkRequest', function () {
    //$("#AddWorkRequestModalDialog").modal('hide');
    $('#AddWorkRequestModalDialog').removeClass('slide-active');
    fileExtAddProccess = "";
});
//$(document).on('hidden.bs.modal', '#AddWorkRequestModalDialog', function () {

//});
//#endregion

//#region Equipment Hierarchy ModalTree in Add Work Request Modal
var TextFieldId_ChargeTo = "";
var HdnfieldId_ChargeTo = "";
$(document).on('click', '#imgChargeToTreeGridOverWorkReqModal', function (e) {
    TextFieldId_ChargeTo = $(this).data('textfield');
    HdnfieldId_ChargeTo = $(this).data('valuefield');
    $(this).blur();
    generateWREquipmentTreeDynamic(-1);
});
function generateWREquipmentTreeDynamic(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/WorkOrderEquipmentHierarchyTreeDynamic',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find(".cntTreeWR").html('');
            $(document).find(".cntTreeWR").html(data);
        },
        complete: function () {
            CloseLoader();
            $('#wrEquipTreeModal').addClass('slide-active');
            treeTable($(document).find(".cntTreeWR").find('#tblTree'));
            $(document).find('.radSelectWoDynamic').each(function () {
                if ($(document).find('#' + HdnfieldId_ChargeTo).val() == '0' || $(document).find('#' + HdnfieldId_ChargeTo) == '') {

                    if ($(this).data('equipmentid') === equipmentid) {
                        $(this).attr('checked', true);
                    }

                }
                else {

                    if ($(this).data('equipmentid') == $(document).find('#' + HdnfieldId_ChargeTo).val()) {
                        $(this).attr('checked', true);
                    }

                }

            });
            //-- V2-518 collapse all element
            // looking for the collapse icon and triggered click to collapse
            $.each($(document).find(".cntTreeWR").find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAAsTAAALEwEAmpwYAAAKT2lDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAHjanVNnVFPpFj333vRCS4iAlEtvUhUIIFJCi4AUkSYqIQkQSoghodkVUcERRUUEG8igiAOOjoCMFVEsDIoK2AfkIaKOg6OIisr74Xuja9a89+bN/rXXPues852zzwfACAyWSDNRNYAMqUIeEeCDx8TG4eQuQIEKJHAAEAizZCFz/SMBAPh+PDwrIsAHvgABeNMLCADATZvAMByH/w/qQplcAYCEAcB0kThLCIAUAEB6jkKmAEBGAYCdmCZTAKAEAGDLY2LjAFAtAGAnf+bTAICd+Jl7AQBblCEVAaCRACATZYhEAGg7AKzPVopFAFgwABRmS8Q5ANgtADBJV2ZIALC3AMDOEAuyAAgMADBRiIUpAAR7AGDIIyN4AISZABRG8lc88SuuEOcqAAB4mbI8uSQ5RYFbCC1xB1dXLh4ozkkXKxQ2YQJhmkAuwnmZGTKBNA/g88wAAKCRFRHgg/P9eM4Ors7ONo62Dl8t6r8G/yJiYuP+5c+rcEAAAOF0ftH+LC+zGoA7BoBt/qIl7gRoXgugdfeLZrIPQLUAoOnaV/Nw+H48PEWhkLnZ2eXk5NhKxEJbYcpXff5nwl/AV/1s+X48/Pf14L7iJIEyXYFHBPjgwsz0TKUcz5IJhGLc5o9H/LcL//wd0yLESWK5WCoU41EScY5EmozzMqUiiUKSKcUl0v9k4t8s+wM+3zUAsGo+AXuRLahdYwP2SycQWHTA4vcAAPK7b8HUKAgDgGiD4c93/+8//UegJQCAZkmScQAAXkQkLlTKsz/HCAAARKCBKrBBG/TBGCzABhzBBdzBC/xgNoRCJMTCQhBCCmSAHHJgKayCQiiGzbAdKmAv1EAdNMBRaIaTcA4uwlW4Dj1wD/phCJ7BKLyBCQRByAgTYSHaiAFiilgjjggXmYX4IcFIBBKLJCDJiBRRIkuRNUgxUopUIFVIHfI9cgI5h1xGupE7yAAygvyGvEcxlIGyUT3UDLVDuag3GoRGogvQZHQxmo8WoJvQcrQaPYw2oefQq2gP2o8+Q8cwwOgYBzPEbDAuxsNCsTgsCZNjy7EirAyrxhqwVqwDu4n1Y8+xdwQSgUXACTYEd0IgYR5BSFhMWE7YSKggHCQ0EdoJNwkDhFHCJyKTqEu0JroR+cQYYjIxh1hILCPWEo8TLxB7iEPENyQSiUMyJ7mQAkmxpFTSEtJG0m5SI+ksqZs0SBojk8naZGuyBzmULCAryIXkneTD5DPkG+Qh8lsKnWJAcaT4U+IoUspqShnlEOU05QZlmDJBVaOaUt2ooVQRNY9aQq2htlKvUYeoEzR1mjnNgxZJS6WtopXTGmgXaPdpr+h0uhHdlR5Ol9BX0svpR+iX6AP0dwwNhhWDx4hnKBmbGAcYZxl3GK+YTKYZ04sZx1QwNzHrmOeZD5lvVVgqtip8FZHKCpVKlSaVGyovVKmqpqreqgtV81XLVI+pXlN9rkZVM1PjqQnUlqtVqp1Q61MbU2epO6iHqmeob1Q/pH5Z/YkGWcNMw09DpFGgsV/jvMYgC2MZs3gsIWsNq4Z1gTXEJrHN2Xx2KruY/R27iz2qqaE5QzNKM1ezUvOUZj8H45hx+Jx0TgnnKKeX836K3hTvKeIpG6Y0TLkxZVxrqpaXllirSKtRq0frvTau7aedpr1Fu1n7gQ5Bx0onXCdHZ4/OBZ3nU9lT3acKpxZNPTr1ri6qa6UbobtEd79up+6Ynr5egJ5Mb6feeb3n+hx9L/1U/W36p/VHDFgGswwkBtsMzhg8xTVxbzwdL8fb8VFDXcNAQ6VhlWGX4YSRudE8o9VGjUYPjGnGXOMk423GbcajJgYmISZLTepN7ppSTbmmKaY7TDtMx83MzaLN1pk1mz0x1zLnm+eb15vft2BaeFostqi2uGVJsuRaplnutrxuhVo5WaVYVVpds0atna0l1rutu6cRp7lOk06rntZnw7Dxtsm2qbcZsOXYBtuutm22fWFnYhdnt8Wuw+6TvZN9un2N/T0HDYfZDqsdWh1+c7RyFDpWOt6azpzuP33F9JbpL2dYzxDP2DPjthPLKcRpnVOb00dnF2e5c4PziIuJS4LLLpc+Lpsbxt3IveRKdPVxXeF60vWdm7Obwu2o26/uNu5p7ofcn8w0nymeWTNz0MPIQ+BR5dE/C5+VMGvfrH5PQ0+BZ7XnIy9jL5FXrdewt6V3qvdh7xc+9j5yn+M+4zw33jLeWV/MN8C3yLfLT8Nvnl+F30N/I/9k/3r/0QCngCUBZwOJgUGBWwL7+Hp8Ib+OPzrbZfay2e1BjKC5QRVBj4KtguXBrSFoyOyQrSH355jOkc5pDoVQfujW0Adh5mGLw34MJ4WHhVeGP45wiFga0TGXNXfR3ENz30T6RJZE3ptnMU85ry1KNSo+qi5qPNo3ujS6P8YuZlnM1VidWElsSxw5LiquNm5svt/87fOH4p3iC+N7F5gvyF1weaHOwvSFpxapLhIsOpZATIhOOJTwQRAqqBaMJfITdyWOCnnCHcJnIi/RNtGI2ENcKh5O8kgqTXqS7JG8NXkkxTOlLOW5hCepkLxMDUzdmzqeFpp2IG0yPTq9MYOSkZBxQqohTZO2Z+pn5mZ2y6xlhbL+xW6Lty8elQfJa7OQrAVZLQq2QqboVFoo1yoHsmdlV2a/zYnKOZarnivN7cyzytuQN5zvn//tEsIS4ZK2pYZLVy0dWOa9rGo5sjxxedsK4xUFK4ZWBqw8uIq2Km3VT6vtV5eufr0mek1rgV7ByoLBtQFr6wtVCuWFfevc1+1dT1gvWd+1YfqGnRs+FYmKrhTbF5cVf9go3HjlG4dvyr+Z3JS0qavEuWTPZtJm6ebeLZ5bDpaql+aXDm4N2dq0Dd9WtO319kXbL5fNKNu7g7ZDuaO/PLi8ZafJzs07P1SkVPRU+lQ27tLdtWHX+G7R7ht7vPY07NXbW7z3/T7JvttVAVVN1WbVZftJ+7P3P66Jqun4lvttXa1ObXHtxwPSA/0HIw6217nU1R3SPVRSj9Yr60cOxx++/p3vdy0NNg1VjZzG4iNwRHnk6fcJ3/ceDTradox7rOEH0x92HWcdL2pCmvKaRptTmvtbYlu6T8w+0dbq3nr8R9sfD5w0PFl5SvNUyWna6YLTk2fyz4ydlZ19fi753GDborZ752PO32oPb++6EHTh0kX/i+c7vDvOXPK4dPKy2+UTV7hXmq86X23qdOo8/pPTT8e7nLuarrlca7nuer21e2b36RueN87d9L158Rb/1tWeOT3dvfN6b/fF9/XfFt1+cif9zsu72Xcn7q28T7xf9EDtQdlD3YfVP1v+3Njv3H9qwHeg89HcR/cGhYPP/pH1jw9DBY+Zj8uGDYbrnjg+OTniP3L96fynQ89kzyaeF/6i/suuFxYvfvjV69fO0ZjRoZfyl5O/bXyl/erA6xmv28bCxh6+yXgzMV70VvvtwXfcdx3vo98PT+R8IH8o/2j5sfVT0Kf7kxmTk/8EA5jz/GMzLdsAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAHUwAADqYAAAOpgAABdvkl/FRgAAAHFJREFUeNpi/P//PwMlgImBQsA44C6gvhfa29v3MzAwOODRc6CystIRbxi0t7fjDJjKykpGYrwwi1hxnLHQ3t7+jIGBQRJJ6HllZaUUKYEYRYBPOB0gBShKwKGA////48VtbW3/8clTnBIH3gCKkzJgAGvBX0dDm0sCAAAAAElFTkSuQmCC"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find(".cntTreeWR").find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element

            $('#tblTree tr td').removeAttr('style');// code to remove the style applied from treetable.js -- white-space: nowrap;
        },
        error: function (xhr) {
            alert('error');
        }
    });
}


$(document).on('change', ".radSelectWoDynamic", function () {

    var s = $(this).data;
    $(document).find('#' + HdnfieldId_ChargeTo).val('0');
    equipmentid = $(this).data('equipmentid');
    var clientlookupid = $(this).data('clientlookupid');
    var chargetoname = $(this).data('itemname');
    chargetoname = chargetoname.substring(0, chargetoname.length - 1);

    //commented ajax call not sure why it is called
    //$.ajax({
    //    url: '/PlantLocationTree/MapEquipmentHierarchyTree',
    //    datatype: "json",
    //    type: "post",
    //    contenttype: 'application/json; charset=utf-8',
    //    async: false,
    //    cache: false,
    //    data: { _EquipmentId: equipmentid },
    //    success: function (data) {
    //        //$('#wrEquipTreeModal').modal('hide');
    //        //$(document).find('#' + TextFieldId_ChargeTo).val(clientlookupid).removeClass('input-validation-error');
    //        //$(document).find('#' + HdnfieldId_ChargeTo).val(equipmentid).trigger('change');

    $(document).find('#' + TextFieldId_ChargeTo).val(clientlookupid).trigger('mbsc-enhance');
    $(document).find('#' + TextFieldId_ChargeTo).parents('div').eq(0).css("display", "block");
    $(document).find('#' + HdnfieldId_ChargeTo).val(equipmentid);//.removeClass('input-validation-error').trigger('change');
    $(document).find('#' + HdnfieldId_ChargeTo).closest('form').valid();
    $(document).find('#' + HdnfieldId_ChargeTo).parents('div').eq(0).css("display", "none");
    $(document).find('#' + HdnfieldId_ChargeTo).parent().find('div > button.ClearAssetModalPopupGridData').css('display', 'block');
    $('#wrEquipTreeModalHide').trigger('click');
    //    }
    //});
    //commented ajax call not sure why it is called
    //V2-948
    var SourceAssetAccount = $(document).find("#SourceAssetAccount").val();
    if (SourceAssetAccount != undefined && SourceAssetAccount == "True") {
        getlaboraccount_mobile(equipmentid);

    }
            //V2-948
});
$(document).on('click', '#wrEquipTreeModalHide', function () {
    $('#wrEquipTreeModal').removeClass('slide-active');
    TextFieldId_ChargeTo = "";
    HdnfieldId_ChargeTo = "";
});
//#endregion

//#endregion
//#endregion
//#region add returnpart v2-624
var SelectedActualPartId = [];
function ActualPartSelectedItem(PartHistoryId, PartId, PartClientLookupId, Description, TransactionQuantity, TotalCost, UPCCode, StoreroomId) {
    //this.index = index;
    this.PartHistoryId = PartHistoryId;
    this.PartId = PartId;
    this.PartClientLookupId = PartClientLookupId;
    this.Description = Description;
    this.TransactionQuantity = TransactionQuantity;
    this.Cost = TotalCost;
    this.UPCCode = UPCCode;
    this.StoreroomId = StoreroomId;
};
$(document).on('change', '.chksearchdashboardpart', function () {
    var data = PartsTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        SelectedActualPartId = SelectedActualPartId.filter(function (el) {
            return el.PartHistoryId !== data.PartHistoryId;
        });
    }
    else {
        var item = new ActualPartSelectedItem(data.PartHistoryId, data.PartId, data.PartClientLookupId, data.Description,
            data.TransactionQuantity, data.TotalCost, data.UPCCode, data.StoreroomId);
        SelectedActualPartId.push(item);
    }
    if (SelectedActualPartId.length > 0) {
        $("#BtnReturnPartsDashboard").show();
    }
    else {
        $("#BtnReturnPartsDashboard").hide();
        SelectedActualPartId = []
    }
})

$(document).on('click', "#BtnReturnPartsDashboard", function () {
    var jsonResult = {
        "WoPart": SelectedActualPartId,
        "PartIssueAddModel.WorkOrderId": $(document).find("#WorkOrderId").val(),
        "PartIssueAddModel.WorkOrderClientLookupId": $(document).find("#WorkOrderClientLookupId").val()
    }
    $.ajax({
        url: '/Dashboard/ReturnPartSelectedList_Mobile',
        type: "POST",
        datatype: "json",
        data: JSON.stringify(jsonResult),
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("CheckinProcessCompleteSuccessAlert");
                swal(SuccessAlertSetting, function () {

                });
            }
            else {
            }
            $("#BtnReturnPartsDashboard").hide();
            $(document).find('.chksearchdashboardpart').prop('checked', false);
            SelectedActualPartId = [];
            pageno = PartsTable.page.info().page;
            PartsTable.page(pageno).draw('page');
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
})

//#endregion

//#region Add QR scanner

//#region Parts QR Reader
$(document).on('click', '#btnQrScanner', function () {
    $(document).find('#txtpartid').val('');
    $(document).find('#PartIssueAddModel_PartId').val('');
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').addClass("slide-active");
        StartQRReader_Mobile('');
    }
});
var Equipment_textFieldID_Mobile = '', Equipment_valueFieldID_Mobile = '';
function QrScannerEquipment_Mobile(txtID, ValID) {
    Equipment_textFieldID_Mobile = '#' + txtID;
    Equipment_valueFieldID_Mobile = '';
    if (ValID != '') {
        Equipment_valueFieldID_Mobile = '#' + ValID;
    }
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').addClass("slide-active");
        StartQRReader_Mobile('Equipment');
    }
}
$(document).on('click', '#closeQrScanner_Mobile', function () {
    if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').removeClass('slide-active');
    }
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').removeClass('slide-active');
        StopCamera(); // using same method from somax_main.js
    }
});
function StartQRReader_Mobile(Module) {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
                //aspectRatio: aspectratio //1.7777778
            }, success => {
                if (Module == '') {
                    onScanSuccessPartIssue_Mobile(success);
                }
                else if (Module == 'Equipment') {
                    onScanSuccessEquipment_Mobile(success);
                }
            }, error => {
            });
        } else {
            if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
                $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
            }
        }
    }).catch(e => {
        if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
            $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
        }
        if (e && e.startsWith('NotReadableError')) {
            ShowErrorAlert(getResourceValue("cameraIsBeingUsedByAnotherAppAlert"));
        }
        else if (e && e.startsWith('NotFoundError')) {
            ShowErrorAlert(getResourceValue("cameraDeviceNotFoundAlert"));
        }

    });
}

function onScanSuccessPartIssue_Mobile(decodedText) {
    var url = "/Dashboard/GetPartIdByClientLookUpId?clientLookUpId=" + decodedText;
    if ($(document).find('#UseMultiStoreroom').val() == "True") {
        var StoreroomId = $(document).find('#StoreroomId').val();
        url = "/Base/GetPartIdByStoreroomIdAndClientLookUpforMultiStoreroom?ClientLookupId=" + decodedText + "&StoreroomId=" + StoreroomId;
    }
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
                $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
            }
            if (data.PartId > 0) {
                var WOid = $(document).find('#WorkOrderId').val();
                var WOLookupid = $(document).find('#WorkOrderClientLookupId').val();
                $(document).find('#PartIssueAddModel_WorkOrderId').val(WOid);
                $(document).find('#PartIssueAddModel_WorkOrderClientLookupId').val(WOLookupid);
                $(document).find('#txtpartid').val(decodedText).trigger('mbsc-enhance'); //.removeClass('input-validation-error');
                $(document).find('#txtpartid').closest('form').valid();
                $(document).find('#PartIssueAddModel_PartId').val(data.PartId);
            }
            else if (data.MultiStoreroomError == true) {
                var msg = getResourceValue('spnInvalidQrCodeMsgforMultiStoreroom');
                ShowErrorAlert(msg.replace('${decodedText}', decodedText));
            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodeMsg').replace('${decodedText}', decodedText));
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

function detectMob() {
    var isMobile = false;
    if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
        || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
        return true;
    }
    return isMobile;
}

//#endregion

//#region Equipment qr scanner
function onScanSuccessEquipment_Mobile(decodedText) {
    $.ajax({
        url: "/WorkOrder/GetEquipmentIdByClientLookUpId?clientLookUpId=" + decodedText,
        type: "GET",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
                $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
            }
            if (data.EquipmentId > 0) {
                $(document).find(Equipment_textFieldID_Mobile).val('');
                $(document).find(Equipment_textFieldID_Mobile).val(decodedText).trigger('mbsc-enhance');//.removeClass('input-validation-error');
                $(document).find(Equipment_textFieldID_Mobile).closest('form').valid();
                $(document).find(Equipment_textFieldID_Mobile).parents('div').eq(0).css('display', 'block');
                if (Equipment_valueFieldID_Mobile != '') {
                    $(document).find(Equipment_valueFieldID_Mobile).val('');
                    $(document).find(Equipment_valueFieldID_Mobile).val(data.EquipmentId).trigger('change');
                    if ($(document).find(Equipment_valueFieldID_Mobile).css('display') == 'block') {
                        $(document).find(Equipment_valueFieldID_Mobile).parents('div').eq(0).css('display', 'none');
                    }
                    if ($(document).find(Equipment_valueFieldID_Mobile).parent().find('div > button.ClearAssetModalPopupGridData').length > 0) {
                        //Dynamic Pages Cross button to clear the textbox Value
                        $(document).find(Equipment_valueFieldID_Mobile).parent().find('div > button.ClearAssetModalPopupGridData').css('display', 'block');
                    }
                }

                //V2-948
                var SourceAssetAccount = $(document).find("#SourceAssetAccount").val();
                if (SourceAssetAccount != undefined && SourceAssetAccount == "True") {
                    getlaboraccount_mobile(data.EquipmentId);

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
//#endregion Add QR scanner

//#region Assign in details page
//$(document).on('mouseenter', '.assignedItemDetails', function (e) {
//    var thise = $(this);
//    if (LRTrim(thise.find('.tooltipcards').text()).length > 0) {
//        thise.find('.tooltipcards').attr('style', 'display :block !important;');
//        return;
//    }
//    WoId = $(this).attr('id');
//    var innerText = this.innerText.trim();
//    var waPersonnelId = $(this).attr('waPersonnelId');
//    if (waPersonnelId == -1) {
//        $.ajax({
//            "url": "/DashBoard/PopulateHover",
//            "data": {
//                workOrderId: WoId
//            },
//            "dataType": "json",
//            "type": "POST",
//            "beforeSend": function (data) {
//                thise.find('.loadingImg').show();
//            },
//            "success": function (data) {
//                if (data.personnelList != null) {
//                    $('#spn' + WoId).html(data.personnelList);
//                }
//            },
//            "complete": function () {
//                thise.find('.loadingImg').hide();
//                thise.find('.tooltipcards').attr('style', 'display :block !important;');
//            }
//        });
//    }
//});
//$(document).on('mouseleave', '.assignedItemDetails', function (e) {
//    var thise = $(this);
//    if (LRTrim(thise.find('.tooltipcards').text()).length > 0) {
//        thise.find('.tooltipcards').attr('style', 'display :none !important;');
//        return;
//    }
//});
//#endregion

//#region Common
$(document).on('click', '.clearTextBoxValue', function () {
    var id = $(this).data('txtboxid');
    $(document).find('#' + id).val('');
    if (id == 'txtPartSearch_Mobile') {
        generatePartScrollViewForMobileMobiscroll();
    }
    else if (id == 'txtEquipmentSearch_Mobile') {
        generateEquipmentDataTable_Mobile();
    }
    else if (id == 'txtAccountSearch_Mobile') {
        generateAccountDataTable_Mobile();
    }
});
//#endregion

//#region Show Qrcode
$(document).on('click', '#QrCodeDetail', function () {
    var workorderClientLookupId = $(document).find("#WorkOrderClientLookupId").val();
    $.ajax({
        url: '/Base/LoadQRCode',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: {
            WorkorderClientLookupId: workorderClientLookupId
        },
        success: function (data) {
            if (data) {
                DrawQRCode($('#divQrCodeDetail'), data);
                if (!$(document).find('#QrCodeDetailModal_Mobile').hasClass('slide-active')) {
                    $(document).find('#QrCodeDetailModal_Mobile').addClass("slide-active");
                }
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (xhr) {
            console.log(xhr)
        },
    });
});

$(document).on('click', '#closeQrDetails_Mobile', function () {
    if ($(document).find('#QrCodeDetailModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeDetailModal_Mobile').removeClass('slide-active');
    }
    if (!$(document).find('#QrCodeDetailModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeDetailModal_Mobile').removeClass('slide-active');
    }
});

//#endregion

//#region Show BarCode
$(document).on('click', '#BarCodeDetails', function () {
    var WOClientLookupId = $(document).find('#WorkOrderClientLookupId').val();
    DrawBarCode('#wocode39barcode', WOClientLookupId)
    if (!$(document).find('#barCodeDetailModal_Mobile').hasClass('slide-active')) {
        $(document).find('#barCodeDetailModal_Mobile').addClass("slide-active");
    }

})
$(document).on('click', '#closeBarDetails_Mobile', function () {
    if ($(document).find('#barCodeDetailModal_Mobile').hasClass('slide-active')) {
        $(document).find('#barCodeDetailModal_Mobile').removeClass('slide-active');
    }
    if (!$(document).find('#barCodeDetailModal_Mobile').hasClass('slide-active')) {
        $(document).find('#barCodeDetailModal_Mobile').removeClass('slide-active');
    }
});
//#endregion

//#region Material Request
$(document).on('click', '.MaterialRequestclearstate', function () {
    $('#AddMaterialRequestModalpopup').parent().removeClass('slide-active');

});
$(document).on('click', ".addWoEstimatesPartBttn,#btnAddParts", function (e) {
    $('#PartWo_Mobile').addClass('slide-active').trigger('mbsc-enhance');
});
function LoadMaterialRequestTab() {
    $.ajax({
        url: '/Dashboard/LoadMaterialRequest_Mobile',
        type: 'POST',
        data: {
            'WorkOrderId': $(document).find('#WorkOrderId').val()
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#MaterialRequests').html(data);
        },
        complete: function () {
            generateMaintenanceCompletionMaterialRequestGrid();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
var dtEstimatesPartTable;
var EstimatedCostsIdsToupdate = [];
function generateMaintenanceCompletionMaterialRequestGrid() {
    var rCount = 0;
    var initiated = false;
    if ($(document).find('#tblMaterialRequest').hasClass('dataTable')) {
        dtEstimatesPartTable.destroy();
    }
    var workOrderID = $(document).find('#WorkOrderId').val();
    var EstimatedCostsId = $(document).find('#workOrderModel_EstimatedCostsId').val();

    $(document).find('#EstimatedCostsGenerationSearch-select-all').prop('checked', false);

    dtEstimatesPartTable = $("#tblMaterialRequest").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Dashboard/PopulateMaterialRequest_Mobile?workOrderId=" + workOrderID,
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.EstimatedCostsId = EstimatedCostsId;
            },
            "dataSrc": function (response) {
                rCount = response.data.length;
                initiated = response.IsInitiated;
                //console.log(rCount + 'rcount');
                if (rCount > 0) {
                    $("#btnAddParts").hide();
                    if (initiated) {
                        $("#sendmaterialrequestitemsforapproval").show();
                    }
                    else {
                        $("#sendmaterialrequestitemsforapproval").hide();
                        $("#btnAddParts").show();
                    }
                    //$("#sendmaterialrequestitemsforapproval").show();
                }
                else {
                    if ($("#woCompletionWorkbenchSummary_Status").val() != "Complete" && $("#woCompletionWorkbenchSummary_Status").val() != "Denied") {
                        $("#btnAddParts").show();
                    }
                    else {
                        $("#btnAddParts").hide();
                    }
                    //$("#sendmaterialrequestitemsforapproval").hide();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                data: "EstimatedCostsId",
                orderable: false,
                className: 'select-checkbox dt-body-center',
                targets: 0,
                render: function (data, type, full, meta) {
                    if ($('#EstimatedCostsGenerationSearch-select-all').is(':checked')) {
                        return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="partisSelect" value="' + $('<div/>').text(data).html() + '">';
                    } else {
                        if (EstimatedCostsIdsToupdate.indexOf(data) != -1) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="partisSelect ' + data + '" value="' + $('<div/>').text(data).html() + '">';
                        } else {
                            return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="partisSelect" value="' + $('<div/>').text(data).html() + '">';
                        }
                    }
                }
            },
            {
                "data": null,
                targets: [8], render: function (a, b, data, d) {
                    var ismaterialreq = $("#ApprovalRouteModel_IsMaterialRequest").val();
                    if ($("#woCompletionWorkbenchSummary_Status").val() != "Complete" && $("#woCompletionWorkbenchSummary_Status").val() != "Denied") {
                        if (data.Status == "Initiated" && ismaterialreq == "True") {
                            if (data.PurchaseRequestId != 0) {
                                return '<a class="btn btn-outline-primary addWoEstimatesPartBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>';
                            }
                            else {
                                return '<a class="btn btn-outline-primary addWoEstimatesPartBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                                    '<a class="btn btn-outline-success editWoEstimatesPartBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                                    '<a class="btn btn-outline-danger delWoEstimatesPartBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            
                        }
                        else if (data.Status == "Approved" && ismaterialreq == "False") {
                            if (data.PurchaseRequestId != 0) {
                                return '<a class="btn btn-outline-primary addWoEstimatesPartBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>';
                            }
                            else {
                                return '<a class="btn btn-outline-primary addWoEstimatesPartBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                                    '<a class="btn btn-outline-success editWoEstimatesPartBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                                    '<a class="btn btn-outline-danger delWoEstimatesPartBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                            }
                            
                        }
                    }
                }
            }
        ],
        "columns":
            [
                {},
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Unit", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Status", autoWidth: true, bSearchable: true, bSortable: true, className: "text-left" },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
    $('#EstimatedCostsGenerationSearch-select-all').on('click', function () {
        $('.partisSelect').prop('checked', this.checked);
    });
}

$(document).on('click', "#selectidpartininventory", function (e) {
    e.preventDefault();
    var WorkOrderID = $(document).find('#WorkOrderId').val();
    $('.modal-backdrop').remove();
    if ($(document).find('#IsUseMultiStoreroom').val() == "True") {
        PopulateStorerooms();
    }
    else {
        GoToAddPartInInventory(WorkOrderID);
    }
});

function GoToAddPartInInventory(WorkOrderID) {
    var ClientLookupId = $(document).find('#WorkOrderClientLookupId').val();
    var vendorId = 0;
    var StoreroomId = 0;
    if ($(document).find('#StoreroomId').val() != "undefined" && $(document).find('#StoreroomId').val() > 0) {
        StoreroomId = $(document).find('#StoreroomId').val();
    }
    $.ajax({
        url: "/Dashboard/AddPartInInventory_Mobile",
        type: "POST",
        dataType: "html",
        data: { WorkOrderID: WorkOrderID, ClientLookupId: ClientLookupId, vendorId: vendorId, StoreroomId: StoreroomId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#mainwidget').html(data);
            SetFixedHeadStyle();
        },
        complete: function () {
            CloseLoader();
            ShowCardViewWO_FromDashboard(WorkOrderID);
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', "#selectidpartnotininventory", function (e) {
    AddEstimatePartNotInInventory();
});

function AddEstimatePartNotInInventory() {
    var workorderId = $(document).find('#WorkOrderId').val();
    var clientLookupId = $(document).find('#WorkOrderClientLookupId').val();
    $.ajax({
        url: "/Dashboard/AddEstimatesPartNotInInventory_Mobile",
        type: "GET",
        dataType: 'html',
        data: { WorkOrderId: workorderId, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#MaterialRequestPopUp').html(data);
        },
        complete: function () {
            CloseLoader();
            $('#PartWo_Mobile').removeClass("slide-active");
            $('.modal-backdrop').remove();
            SetControls();
            BindMobiscrollControlForMaterialRequestTab();
        },
        error: function () {
            CloseLoader();
        }
    });
}

function RedirectToMaintenanceWorkBenchDetailFromDashboard(WorkOrderId, ClientLookupId, CurrentTabForDashboard) {
    $.ajax({
        url: "/DashBoard/CompletionWorkbench_Details_Mobile",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { WorkOrderId: WorkOrderId, ClientLookupId: ClientLookupId },
        success: function (data) {
            $('#mainwidget').html(data);
        },
        complete: function () {
            SetWOCompletionDetailEnvironment();
            SetFixedHeadStyle();
            CloseLoader();
            //restoring tab details page 
            $(document).find(".m-portlet").find("button.active").removeClass('active');
            $(document).find(".m-portlet").find("[data-tab='" + CurrentTabForDashboard + "']").addClass('active');
            ResetOtherTabs();
            $(document).find("#Labor").removeAttr('style');
            $(document).find("#" + CurrentTabForDashboard).css("display", "block");
            $('.modal-backdrop').remove();
            loadTabDetails(CurrentTabForDashboard);
        }
    });
}


function EstimatePartNotInInventoryAddOnSuccess(data) {
    if (data.Result == "success") {
        $('#AddMaterialRequestModalpopup').parent().removeClass("slide-active");
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("MaterialRequestAddedAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("MaterialRequestUpdatedAlert");
        }
        swal(SuccessAlertSetting, function () {
            ShowLoader();
            dtEstimatesPartTable.page('first').draw('page');
            CloseLoader();
        });

    }
    else {
        ShowGenericErrorOnAddUpdate(data);

    }
}
$(document).on('click', '.addWoEstimatesPartBttn', function () {


});
$(document).on('click', '.editWoEstimatesPartBttn', function () {
    var workorderId = $(document).find('#WorkOrderId').val();
    var mainClientLookupId = $(document).find('#WorkOrderClientLookupId').val();
    var data = dtEstimatesPartTable.row($(this).parents('tr')).data();
    var PartclientLookupId = data.ClientLookupId;
    var EstimatedCostsId = data.EstimatedCostsId;
    var CategoryId = data.CategoryId;
    var Description = data.Description;
    var UnitCost = data.UnitCost;
    var Unit = data.Unit;
    var AccountId = data.AccountId;
    var AccountClientLookupId = data.AccountClientLookupId;
    var VendorId = data.VendorId;
    var VendorClientLookupId = data.VendorClientLookupId;
    var PartCategoryMasterId = data.PartCategoryMasterId;
    var PartCategoryClientLookupId = data.PartCategoryClientLookupId;
   /* var VendorId = data.VendorId;*/
    var Quantity = data.Quantity;
   
    var TotalCost = data.TotalCost;

    if (CategoryId > 0) {
        $.ajax({
            url: "/Dashboard/EditPartInInventory_Mobile",
            type: "GET",
            dataType: 'html',
            data: {
                EstimatedCostsId: EstimatedCostsId, WorkOrderId: workorderId
            },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#MaterialRequestPopUp').html(data);
            },
            complete: function () {
                CloseLoader();
                $('#PartWo_Mobile').removeClass("slide-active");
                $('.modal-backdrop').remove();
                SetControls();
                BindMobiscrollControlForMaterialRequestTab();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
    else {
        $.ajax({
            url: "/Dashboard/EditEstimatesPart_Mobile",
            type: "GET",
            dataType: 'html',
            data: { WorkOrderId: workorderId, MainClientLookupId: mainClientLookupId, PartclientLookupId: PartclientLookupId, EstimatedCostsId: EstimatedCostsId, Description: Description, UnitCost: UnitCost, Unit: Unit, AccountClientLookupId: AccountClientLookupId, AccountId: AccountId, VendorClientLookupId: VendorClientLookupId, VendorId: VendorId, PartCategoryClientLookupId: PartCategoryClientLookupId, PartCategoryMasterId: PartCategoryMasterId, Quantity: Quantity, TotalCost: TotalCost },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#MaterialRequestPopUp').html(data);
            },
            complete: function () {
                CloseLoader();
                $('#PartWo_Mobile').removeClass("slide-active");
                $('.modal-backdrop').remove();
                SetControls();
                BindMobiscrollControlForMaterialRequestTab();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});

$(document).on('click', '.delWoEstimatesPartBttn', function () {
    var data = dtEstimatesPartTable.row($(this).parents('tr')).data();
    var EstimatedCostsId = data.EstimatedCostsId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Dashboard/DeleteEstimatesPart_Mobile',
            data: {
                EstimatedCostsId: EstimatedCostsId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("MaterialRequestDeletedAlert"));
                    dtEstimatesPartTable.state.clear();
                }
            },
            complete: function () {
                loadTabDetails('MaterialRequests');
                CloseLoader();
            }
        });
    });
});

function BindMobiscrollControlForMaterialRequestTab() {
    $('#AddMaterialRequestModalpopup').parent().addClass("slide-active");
    $('#estimatePart_Quantity').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 99999999.99,
        //scale: 2,
        maxScale: 2,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    var x = parseFloat($('#estimatePart_Quantity').val()) == 0 ? '' : $('#estimatePart_Quantity').val();
    $('#estimatePart_Quantity').mobiscroll('setVal', x);
    $('#PartNotInInventoryModel_Quantity').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 99999999.99,
        //scale: 2,
        maxScale: 2,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    x = parseFloat($('#PartNotInInventoryModel_Quantity').val()) == 0 ? '' : $('#PartNotInInventoryModel_Quantity').val();
    $('#PartNotInInventoryModel_Quantity').mobiscroll('setVal', x);

    $('#estimatePart_UnitCost').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 9999999999.99999,
        //scale: 2,
        maxScale: 5,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    x = parseFloat($('#estimatePart_UnitCost').val()) == 0 ? '' : $('#estimatePart_UnitCost').val();
    $('#estimatePart_UnitCost').mobiscroll('setVal', x);

    $('#PartNotInInventoryModel_UnitCost').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 9999999999.99999,
        //scale: 2,
        maxScale: 5,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    x = parseFloat($('#PartNotInInventoryModel_UnitCost').val()) == 0 ? '' : $('#PartNotInInventoryModel_UnitCost').val();
    $('#PartNotInInventoryModel_UnitCost').mobiscroll('setVal', x);


    $('#PartNotInInventoryModel_UnitCostStockPart').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 9999999999.99999,
        //scale: 2,
        maxScale: 5,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    x = parseFloat($('#PartNotInInventoryModel_UnitCostStockPart').val()) == 0 ? '' : $('#PartNotInInventoryModel_UnitCostStockPart').val();
    $('#PartNotInInventoryModel_UnitCostStockPart').mobiscroll('setVal', x);

    $('#AddMaterialRequestModalpopup').trigger('mbsc-enhance');

    
    
}

$(document).on('click', "#btnWospartcancel", function () {
    var workorderId = $(document).find('#estimatePart_WorkOrderId').val();
    dtEstimatesPartTable = undefined;
    var CurrentTabForDashboard = 'MaterialRequests';
    RedirectToMaintenanceWorkBenchDetailFromDashboard(workorderId, '', CurrentTabForDashboard);
});

$(document).on('click', "#btnWopartcancel", function () {
    var workorderId = $(document).find('#PartNotInInventoryModel_ObjectId').val();
    dtEstimatesPartTable = undefined;
    var CurrentTabForDashboard = 'MaterialRequests';
    RedirectToMaintenanceWorkBenchDetailFromDashboard(workorderId, '', CurrentTabForDashboard);
});

function EditLineItemOnSuccess(data) {
    CloseLoader();
    var workOrderId = data.workOrderId;
    var CurrentTabForDashboard = 'MaterialRequests';
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("MaterialRequestUpdatedAlert");
        swal(SuccessAlertSetting, function () {
            $('.modal-backdrop').remove();
            RedirectToMaintenanceWorkBenchDetailFromDashboard(workOrderId, '', CurrentTabForDashboard);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '#lnk_psearchdtlsdashboard', function (e) {
    e.preventDefault();
    resetpage();
    var WorkOrderID = $('#WorkOrderID').val();
    var CurrentTabForDashboard = 'MaterialRequests';
    if (WorkOrderID != 0) {
        RedirectToMaintenanceWorkBenchDetailFromDashboard(WorkOrderID, '', CurrentTabForDashboard);
    }
});
$(document).on('click', '#hidePartsWoPopup_Mobile', function () {
    $('#PartWo_Mobile').removeClass('slide-active');

});
//#endregion

//#region Downtime
var DownCount = 0;
var downtimeSecurity;
var downtimeDtTable;
var downtimeSecurityEdit;
var downtimeSecurityCreate;
var downtimeSecurityDelete;
function BindMobiscrollControlForDowntimeTab() {
    $('#AddDowntimeModalpopup').parent().addClass("slide-active");
    $('#woDowntimeModel_Downdate').mobiscroll().calendar({
        display: 'bottom',
        touchUi: true,
        weekDays: 'min',
        yearChange: false,
        //min: new Date(),
        months: 1
    }).inputmask('mm/dd/yyyy');
    $('#woDowntimeModel_MinutesDown').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.0001,
        max: 99999999999.9999,
        //scale: 4,
        maxScale: 4,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    var x = parseFloat($('#woDowntimeModel_MinutesDown').val()) == 0 ? '' : $('#woDowntimeModel_MinutesDown').val();
    $('#woDowntimeModel_MinutesDown').mobiscroll('setVal', x);
    $('#AddDowntimeModalpopup').trigger('mbsc-enhance');
}
function LoadDowntimeTab() {
    $.ajax({
        url: '/Dashboard/LoadDowntime_Mobile',
        type: 'POST',
        //data: {
        //    'WorkOrderId': $(document).find('#WorkOrderId').val()
        //},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Downtime').html(data);
        },
        complete: function () {
            CloseLoader();
            generateDowntimeGrid();

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            CloseLoader();
        }
    });
}
function generateDowntimeGrid() {

    var DownCount = 0;
    var workOrderID = $(document).find('#WorkOrderId').val();
    if ($(document).find('#downtimeDtTable').hasClass('dataTable')) {
        downtimeDtTable.destroy();
    }
    downtimeDtTable = $("#downtimeDtTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Dashboard/PopulateDowntime_Mobile?workOrderId=" + workOrderID,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                DownCount = response.data.length;
                if (DownCount > 0) {
                    $(document).find("#AddDowntimebtn").hide();
                }
                else {
                    if (response.security.Create == true && response.security.Access == true) {
                        $(document).find("#AddDowntimebtn").show();
                    }
                }

                downtimeSecurity = response.security.Access;
                downtimeSecurityEdit = response.security.Edit;
                downtimeSecurityCreate = response.security.Create;
                downtimeSecurityDelete = response.security.Delete;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                /*visible: false,*/
                "className": "text-center",
                targets: [3], render: function (a, b, data, d) {
                    if (downtimeSecurity) {
                        var actionButtonhtml = "";
                        if (downtimeSecurityCreate == true) {
                            actionButtonhtml = '<a class="btn btn-outline-primary addDowntimeBtn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>';
                        }
                        if (downtimeSecurityEdit == true) {
                            actionButtonhtml = actionButtonhtml + '<a class="btn btn-outline-success editDowntimeBtn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                        }
                        if (downtimeSecurityDelete == true) {
                            actionButtonhtml = actionButtonhtml + '<a class="btn btn-outline-danger delDowntimeBtn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                        }
                        return actionButtonhtml;


                    }
                    else {
                        return '';
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "Downdate", "type": "date" },
                { "data": "MinutesDown", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right" },
                { "data": "ReasonForDownDescription", "autoWidth": true, "bSearchable": true, "bSortable": true }

                //{ "className": "text-center", "bSortable": false }
            ],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api();
            var rows = api.rows().nodes();
            var getData = api.rows({ page: 'current' }).data();
            // Total over all pages
            total = api.column(1).data().reduce(function (a, b) {
                return parseFloat(a) + parseFloat(b);
            }, 0);
            // Update footer
            // $(api.column(1).footer()).html(total.toFixed(4));
            $("#downtimeDtTablefoot").empty();
            if (data.length != 0) {
                var footer = "";
                //V2-775
                //if (end == getData[0].TotalCount) {
                //    footer = '<tr><th style="text-align: left!important; font-weight: 500!important; color:#0b0606!important">Total</th><th style = "text-align: right!important; font-weight: 500!important; color: #0b0606!important; padding: 0px 10px 0px 0px!important" >' + total.toFixed(4) + '</th><th></th><th></th></tr><tr><th style="text-align: left !important; font-weight: 500 !important; color: #0b0606 !important">Grand Total</th><th style="text-align: right !important; font-weight: 500 !important; color: #0b0606 !important; padding: 0px 10px 0px 0px !important">' + getData[0].TotalMinutesDown.toFixed(4) + '</th><th></th> <th></th></tr>'
                //    $("#downtimeDtTablefoot").empty().append(footer);
                //}
                //else {
                //    footer = '<tr><th style="text-align: left!important; font-weight: 500!important; color:#0b0606!important">Total</th><th style = "text-align: right!important; font-weight: 500!important; color: #0b0606!important; padding: 0px 10px 0px 0px!important" >' + total.toFixed(4) + '</th><th></th><th></th></tr>'
                //    $("#downtimeDtTablefoot").empty().append(footer);
                //}
                if (end == getData[0].TotalCount) {
                    footer = '<tr><th style="text-align: left !important; font-weight: 500 !important; color: #0b0606 !important">Grand Total</th><th style="text-align: right !important; font-weight: 500 !important; color: #0b0606 !important; padding: 0px 10px 0px 0px !important">' + getData[0].TotalMinutesDown.toFixed(4) + '</th><th></th> <th></th></tr>'
                    $("#downtimeDtTablefoot").empty().append(footer);
                }
            }
        },
        initComplete: function () {
            CloseLoader();
            SetPageLengthMenu();


            if (downtimeSecurity)
                this.api().column(3).visible(true);
            else
                this.api().column(3).visible(false);
        }
    });
}

$(document).on('click', ".addDowntimeBtn, #AddDowntimebtn", function (e) {
    var workOrderId = parseInt($(document).find('#WorkOrderId').val());
    $.ajax({
        url: "/Dashboard/AddEditDowntime_Mobile",
        type: "GET",
        dataType: 'html',
        data: {
            'WorkOrderId': workOrderId,
            'DowntimeId': 0
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DowntimePopUp').html(data);
            /* $('#AddDowntimeModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });*/
        },
        complete: function () {
            SetControls();
            BindMobiscrollControlForDowntimeTab();
            //$(document).find('form').find("#woDowntimeModel_ReasonForDown").select2({});
            //$(document).find('.dtpicker').datepicker({
            //    minDate: 0,
            //    changeMonth: true,
            //    changeYear: true,
            //    "dateFormat": "mm/dd/yy",
            //    autoclose: true
            //}).inputmask('mm/dd/yyyy');
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.editDowntimeBtn', function (e) {
    var row = $(this).parents('tr');
    var data = downtimeDtTable.row(row).data();
    var DowntimeId = data.DowntimeId;
    var workOrderId = parseInt($(document).find('#WorkOrderId').val());
    $.ajax({
        url: "/Dashboard/AddEditDowntime_Mobile",
        type: "GET",
        dataType: 'html',
        data: {
            'WorkOrderId': workOrderId,
            'DowntimeId': DowntimeId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DowntimePopUp').html(data);
            /*$('#AddDowntimeModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });*/
        },
        complete: function () {
            SetControls();
            BindMobiscrollControlForDowntimeTab();
            //$(document).find('form').find("#woDowntimeModel_ReasonForDown").select2({});
            //$(document).find('.dtpicker').datepicker({
            //    minDate: 0,
            //    changeMonth: true,
            //    changeYear: true,
            //    "dateFormat": "mm/dd/yy",
            //    autoclose: true
            //}).inputmask('mm/dd/yyyy');
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delDowntimeBtn', function () {
    var data = downtimeDtTable.row($(this).parents('tr')).data();
    var DowntimeId = data.DowntimeId;

    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Dashboard/DeleteDowntime_Mobile',
            data: {
                DowntimeId: DowntimeId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    downtimeDtTable.state.clear();
                    ShowDeleteAlert(getResourceValue("downtimeDeleteSuccessAlert"));
                }
            },
            complete: function () {
                swal(SuccessAlertSetting, function () {
                    ShowLoader();
                    downtimeDtTable.page('first').draw('page');
                    CloseLoader();
                });
                CloseLoader();
            }
        });
    });
});
function DowntimeAddOnSuccess(data) {
    if (data.Result == "success") {
        $('#AddDowntimeModalpopup').parent().removeClass("slide-active");
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AddDowntimeAlerts");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("spnUpdateSuccessDowntime");
        }
        swal(SuccessAlertSetting, function () {
            ShowLoader();
            downtimeDtTable.page('first').draw('page');
            CloseLoader();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '.clearstateDowntime', function () {
    //StatusArray = [];
    $('#AddDowntimeModalpopup').parent().removeClass('slide-active');
    var areadescribedby = "";
    $(document).find('#AddDowntimeModalpopup select').each(function (i, item) {
        areadescribedby = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areadescribedby).hide();
    });
});
//#endregion
//#region Photos
//#region Multiple Photo Upload
function CompressImage(files, imageName) {
    new Compressor(files, {
        quality: 0.6,
        convertTypes: ['image/png'],
        convertSize: 100000,
        // The compression process is asynchronous,
        // which means you have to access the `result` in the `success` hook function.
        success(result) {
            if (result.size < files.size) {
                SaveCompressedImage(result, imageName);
            }
            else {
                SaveCompressedImage(files, imageName);
            }
            console.log('file name ' + result.name + ' after compress ' + result.size);
        },
        error(err) {
            console.log(err.message);
        },
    });
}
function SaveCompressedImage(data, imageName) {
    var AddPhotoFileData = new FormData();
    AddPhotoFileData.append('file', data, imageName);

    $.ajax({
        url: '../base/SaveUploadedFile',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: AddPhotoFileData,
        success: function (result) {
            var WorkOrderId = $(document).find('#WorkOrderId').val();
            SaveUploadedFileToServer(WorkOrderId, imageName);

            $('#files').val('');
            $('#add_photos').val('');
        }
    });
}
$(document).on('change', '#files', function () {
    var val = $(this).val();
    var imageName = val.replace(/^.*[\\\/]/, '')
    var WorkOrderId1 = $(document).find('#WorkOrderId').val();
    var imgname = WorkOrderId1 + "_" + Math.floor((new Date()).getTime() / 1000);
    //console.log(imageName);
    var fileUpload = $("#files").get(0);
    var files = fileUpload.files;
    var fileExt = imageName.substr(imageName.lastIndexOf('.') + 1).toLowerCase();;
    if (fileExt != 'jpeg' && fileExt != 'jpg' && fileExt != 'png' && fileExt != 'JPEG' && fileExt != 'JPG' && fileExt != 'PNG') {
        ShowErrorAlert(getResourceValue("spnValidImage"));
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    else if (this.files[0].size > (1024 * 1024 * 10)) {
        ShowImageSizeExceedAlert();
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    //var duplicate_chk = SaveUploadedFileToServer(WorkOrderId, imageName);
    //if () { }
    else {
        //alert('Hello');
        swal(AddImageAlertSetting, function () {
            if (window.FormData !== undefined) {
                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    console.log('file name ' + files[i].name + ' before compress ' + files[i].size);
                    CompressImage(files[i], imgname + "." + fileExt);
                }
            }
            else {
                //alert("FormData is not supported.");
                //msg = "notanimage";
            }
            //if (msg != null && msg == "imagesizeexceed") {
            //    ShowImageSizeExceedAlert();
            //}
            //if (msg != null && msg == "notanimage") {
            //    ShowErrorAlert(getResourceValue("spnValidImage"));
            //}
        });
    }
});
function SaveUploadedFileToServer(WorkOrderId, imageName) {
    $.ajax({
        url: '../base/SaveMultipleUploadedFileToServer',
        type: 'POST',

        data: { 'fileName': imageName, objectId: WorkOrderId, TableName: "WorkOrder", AttachObjectName: "WorkOrder" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {
                //$('#EquipZoom').attr('src', data.imageurl);
                //$('.equipImg').attr('src', data.imageurl);
                //$(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>');
                //$('#EquipZoom').data('zoomImage', data.imageurl).elevateZoom(
                //    {
                //        zoomType: "window",
                //        lensShape: "round",
                //        lensSize: 1000,
                //        zoomWindowFadeIn: 500,
                //        zoomWindowFadeOut: 500,
                //        lensFadeIn: 100,
                //        lensFadeOut: 100,
                //        easing: true,
                //        scrollZoom: true,
                //        zoomWindowWidth: 450,
                //        zoomWindowHeight: 450
                //    });
                //$("#EquipZoom").on('load', function () {
                //    CloseLoader();
                //    ShowImageSaveSuccessAlert();
                //});
                CloseLoader();
                ShowImageSaveSuccessAlert();
            }
            else if (data.result == "1") {
                CloseLoader();
                var errorMessage = getResourceValue("ImageExistAlert");
                ShowErrorAlert(errorMessage);

            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
                ShowErrorAlert(errorMessage);

            }
        },
        complete: function () {
            //loadTabDetails('Photos');
            RedirectToMaintenanceWorkBenchDetailFromDashboard(WorkOrderId, '', 'Photos');
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion
//#region Show Images
var imgcardviewstartvalue = 0;
var imgcardviewlwngth = 10;
var imggrdcardcurrentpage = 1;
//var currentorderedcolumn = 1;
var imglayoutTypeWO = 1;
function LoadPhotosTab() {
    $.ajax({
        url: '/Dashboard/LoadPhotos_Mobile',
        type: 'POST',
        data: {
            'WorkOrderId': $(document).find('#WorkOrderId').val()
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Photos').html(data);
        },
        complete: function () {
            LoadImages();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function LoadImages() {
    WorkOrderId = $(document).find('#WorkOrderId').val();
    //if ($('#ModeForRedirect').val() == null || $('#ModeForRedirect').val() == undefined) {
    //    var ModeForRedirect = 'WorkOrder';
    //}
    //else {
    //    var ModeForRedirect = $('#ModeForRedirect').val();
    //}
    //if (!orderbycol) {
    //    orderbycol = 1;
    //}

    //if (!orderDir) {
    //    orderDir = 'asc';
    //}
    //var sorttext = '';
    $.ajax({
        url: '/Dashboard/GetImages_Mobile',
        type: 'POST',
        data: {
            currentpage: imggrdcardcurrentpage,
            start: imgcardviewstartvalue,
            length: imgcardviewlwngth,
            //currentorderedcolumn: currentorderedcolumn,
            //currentorder: currentorder,
            //searchString: LRTrim($(document).find('#txtsearch').val()),
            //VendorId: Vendorid,
            WorkOrderId: WorkOrderId
        },
        beforeSend: function () {
            $(document).find('#imagedataloader').show();
        },
        success: function (data) {
            /*if (data.TotalCount > 0) {*/
            $(document).find('#ImageGrid').show();
            $(document).find('#ObjectImages').html(data).show();
            $(document).find('#tblimages_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == imggrdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
            //}
            //else {
            //    $(document).find('#ImageGrid').hide();
            //}
        },
        complete: function () {
            $(document).find('#imagedataloader').hide();
            //switch (orderbycol) {
            //    case 1:
            //        $(document).find('.srtWOcolumn').eq(0).addClass('sort-active');
            //        sorttext = $(document).find('.srtWOcolumn').eq(0).text();
            //        break;
            //    case 2:
            //        $(document).find('.srtWOcolumn').eq(1).addClass('sort-active');
            //        sorttext = $(document).find('.srtWOcolumn').eq(1).text();
            //        break;
            //    case 3:
            //        $(document).find('.srtWOcolumn').eq(2).addClass('sort-active');
            //        sorttext = $(document).find('.srtWOcolumn').eq(2).text();
            //        break;
            //    case 5:
            //        $(document).find('.srtWOcolumn').eq(3).addClass('sort-active');
            //        sorttext = $(document).find('.srtWOcolumn').eq(3).text();
            //        break;
            //    case 6:
            //        $(document).find('.srtWOcolumn').eq(4).addClass('sort-active');
            //        sorttext = $(document).find('.srtWOcolumn').eq(4).text();
            //        break;
            //}
            //switch (currentorder) {
            //    case "asc":
            //        $(document).find('.srtWOorder').eq(0).addClass('sort-active');
            //        break;
            //    case "desc":
            //        $(document).find('.srtWOorder').eq(1).addClass('sort-active');
            //        break;
            //}
            $(document).find('#cardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(imgcardviewlwngth).trigger('change.select2');
            //HidebtnLoader("btnsortmenu");
            //HidebtnLoader("layoutsortmenuPartLookupWO");
            //$('#btnsortmenu').text(getResourceValue("spnSorting") + " : " + sorttext);
            //if ($('.sidebarCartList').length > 0) {
            //    FillDataFromLayoutView('CardView');
            //}
            //$(document).find('.dtpicker').datepicker({
            //    changeMonth: true,
            //    changeYear: true,
            //    beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
            //    "dateFormat": "mm/dd/yy",
            //    autoclose: true,
            //    minDate: new Date()
            //}).inputmask('mm/dd/yyyy');
            //CloseLoader();
            SetPageLengthMenu();
            InitWebCam();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('change', '#cardviewpagelengthdrp', function () {
    var WorkOrderId = $(document).find('#WorkOrderId').val();
    imgcardviewlwngth = $(this).val();
    imggrdcardcurrentpage = parseInt(imgcardviewstartvalue / imgcardviewlwngth) + 1;
    imgcardviewstartvalue = parseInt((imggrdcardcurrentpage - 1) * imgcardviewlwngth) + 1;
    //GetAndSaveState();
    LoadImages(WorkOrderId);

});
$(document).on('click', '#tblimages_paginate .paginate_button', function () {
    var WorkOrderId = $(document).find('#WorkOrderId').val();
    if (imglayoutTypeWO == 1) {
        var currentselectedpage = parseInt($(document).find('#tblimages_paginate .pagination').find('.active').text());
        imgcardviewlwngth = $(document).find('#cardviewpagelengthdrp').val();
        imgcardviewstartvalue = imgcardviewlwngth * (parseInt($(this).find('.page-link').text()) - 1);
        var lastpage = parseInt($(this).prev('li').data('currentpage'));

        if ($(this).attr('id') == 'tbl_previous') {
            if (currentselectedpage == 1) {
                return false;
            }
            imgcardviewstartvalue = imgcardviewlwngth * (currentselectedpage - 2);
            imggrdcardcurrentpage = imggrdcardcurrentpage - 1;
        }
        else if ($(this).attr('id') == 'tbl_next') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            imgcardviewstartvalue = imgcardviewlwngth * (currentselectedpage);
            imggrdcardcurrentpage = imggrdcardcurrentpage + 1;
        }
        else if ($(this).attr('id') == 'tbl_first') {
            if (currentselectedpage == 1) {
                return false;
            }
            imggrdcardcurrentpage = 1;
            imgcardviewstartvalue = 0;
        }
        else if ($(this).attr('id') == 'tbl_last') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            imggrdcardcurrentpage = parseInt($(this).prevAll('li').eq(1).text());
            imgcardviewstartvalue = imgcardviewlwngth * (imggrdcardcurrentpage - 1);
        }
        else {
            imggrdcardcurrentpage = $(this).data('currentpage');
        }
        //GetAndSaveState();
        LoadImages();

    }
    else {
        run = true;
    }
});
$(document).on('click', ".openPictureOptions", function (e) {
    var AttachmentId = $(this).attr('dataid');
    var AttachmentURL = $(this).attr('dataurl');
    $("#imgAttachmentId").val(AttachmentId);
    $("#imgAttachmentURL").val(AttachmentURL);
    $('#OpenImgActionPopup').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', ".actionpopupmobileback", function (e) {
    $('#OpenImgActionPopup').removeClass('slide-active');
});
$(document).on('click', ".selectidOpen", function (e) {
    var AttachmentURL = $("#imgAttachmentURL").val();
    $("#SelectedImg").attr('src', AttachmentURL);
    $('#ShowImgPopup').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', ".openimgback", function (e) {
    $("#SelectedImg").attr('src', '');
    $('#ShowImgPopup').removeClass('slide-active');
});
//#endregion
//#region Set As Default
$(document).on('click', '#selectidSetAsDefault', function () {
    var AttachmentId = $("#imgAttachmentId").val();
    $('#OpenImgActionPopup').removeClass('slide-active');
    var WorkOrderId = $(document).find('#WorkOrderId').val();
    $('.modal-backdrop').remove();
    $.ajax({
        url: '../base/SetImageAsDefault',
        type: 'POST',

        data: { AttachmentId: AttachmentId, objectId: WorkOrderId, TableName: "WorkOrder" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result === "success") {
                //$('.modal-backdrop').remove();
                //$('#' + AttachmentId).hide();
                $('#EquipZoom').attr('src', data.imageurl);
                $('#EquipZoom').attr('data-zoom-image', data.imageurl);
                $('.equipImg').attr('src', data.imageurl);
                //$(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>');
                $('#EquipZoom').data('zoomImage', data.imageurl).elevateZoom(
                    {
                        zoomType: "window",
                        lensShape: "round",
                        lensSize: 1000,
                        zoomWindowFadeIn: 500,
                        zoomWindowFadeOut: 500,
                        lensFadeIn: 100,
                        lensFadeOut: 100,
                        easing: true,
                        scrollZoom: true,
                        zoomWindowWidth: 450,
                        zoomWindowHeight: 450
                    });
                $("#EquipZoom").on('load', function () {
                    //LoadImages(EquimentId);
                    CloseLoader();
                    ShowImageSetSuccessAlert();
                });
                //RedirectToEquipmentDetail(EquimentId, "OnPremiseImageReload");
                //ShowImageSaveSuccessAlert();
            }
            else {
                CloseLoader();
                //var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
                //ShowErrorAlert(errorMessage);

            }
        },
        complete: function () {
            //CloseLoader();
            loadTabDetails('Photos');
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region Delete Image
$(document).on('click', '#selectidDelete', function () {
    var AttachmentId = $("#imgAttachmentId").val();
    var WorkOrderId = $(document).find('#WorkOrderId').val();
    var ClientOnPremise = $('#MCW_ClientOnPremise').val();
    $('.modal-backdrop').remove();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseMultipleImage(WorkOrderId, AttachmentId);
    }
    else {
        DeleteAzureMultipleImage(WorkOrderId, AttachmentId);
    }
});

function DeleteOnPremiseMultipleImage(WorkOrderId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $('#OpenImgActionPopup').removeClass('slide-active');
        $.ajax({
            url: '../base/DeleteMultipleImageFromOnPremise',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: WorkOrderId, TableName: "WorkOrder" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToMaintenanceWorkBenchDetail(WorkOrderId, '');
                    //loadTabDetails('Photos');
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function DeleteAzureMultipleImage(WorkOrderId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $('#OpenImgActionPopup').removeClass('slide-active');
        $.ajax({
            url: '../base/DeleteMultipleImageFromAzure',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: WorkOrderId, TableName: "WorkOrder" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    //LoadImages(EquimentId);
                    RedirectToMaintenanceWorkBenchDetail(WorkOrderId, '');
                    //loadTabDetails('Photos');
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion
//#region Webcam v2-555
$(document).delegate('[name="imageUploadRadio"]', "click", function () {
    if ($(this).val() == "false") {
        $('#CameraPopup').removeClass("slide-active").trigger('mbsc-enhance');
        $('#forDevice').show();
        $('#forWebcam').hide();
        Dropzone.forElement("div#dropzoneForm").removeAllFiles(true);
    }
    else {
        //$('#CameraPopup').modal('show');
        $('#CameraPopup').addClass("slide-active");
        $('#forDevice').hide();
        $('#forWebcam').show();

    }
});

function InitWebCam() {
    var webcamElement = $('#webcam');
    var canvasElement = $('#canvas');
    var snapSound = $('#snapSound');
    UseWebcam.InitWebcam(webcamElement, canvasElement, snapSound);
    //$('body').addClass('sudip');
}

$(document).delegate("#webcam-switch", "click", function () {
    if (this.checked) {
        UseWebcam.StartCamera(function () {
            cameraStarted();
            $('.md-modal').addClass('md-show');
            $('body').addClass('hideScroll');
        }, function () {
            displayError();
        });
    }
    else {
        cameraStopped();
        UseWebcam.StopCamera();
        $('body').removeClass('hideScroll');
    }
});

$(document).delegate("#cameraFlip", 'mousedown', function () {
    UseWebcam.FlipCamera();
});

$(document).delegate("#closeError", 'click', function () {
    $("#webcam-switch").click();
});

function displayError(err = '') {
    var msg = "Fail to start camera, please allow permision to access camera." +
        "If you are browsing through social media built in browsers, you would need to open the page in Sarafi(iPhone) / Chrome(Android)";
    ShowErrorAlert(msg);
    $('.offCamera').addClass("d-none");
    $('.onCamera').addClass("d-none");
}

function cameraStarted() {
    $("#errorMsg").addClass("d-none");
    $('.flash').hide();
    $('.onCamera').removeClass("d-none");
    $('.offCamera').addClass("d-none");
    //if (UseWebcam.CameraCounts() > 1) {
    //    $("#cameraFlip").removeClass('d-none');
    //} else {
    //    $("#cameraFlip").addClass('d-none');
    //}

}

function cameraStopped() {
    $("#errorMsg").addClass("d-none");
    $('.offCamera').removeClass("d-none");
    $('.onCamera').addClass("d-none");
    $('.md-modal').removeClass('md-show');
}

$(document).delegate("#take-photo", 'click', function () {
    beforeTakePhoto();
    let picture = UseWebcam.Snap();
    //document.querySelector('#download-photo').href = picture;
    document.querySelector('#imgDisply').src = picture;
    afterTakePhoto();
});

function beforeTakePhoto() {
    $('.flash')
        .show()
        .animate({ opacity: 0.3 }, 500)
        .fadeOut(500)
        .css({ 'opacity': 0.7 });
    window.scrollTo(0, 0);
}

function afterTakePhoto() {
    UseWebcam.StopCamera();
    $('.offCamera').removeClass("d-none");
    $(document).find('.onCamera').addClass("d-none");
}

function removeCapture() {
    $('.offCamera').addClass("d-none");
    $('.onCamera').removeClass("d-none");
    if (UseWebcam.CameraCounts() > 1) {
        $("#cameraFlip").removeClass('d-none');
    } else {
        $("#cameraFlip").addClass('d-none');
    }
}

$(document).delegate("#resume-camera", 'click', function () {
    UseWebcam.ResumeCamera(function () {
        removeCapture();
    });
});

$(document).delegate("#exit-app", 'click', function () {
    removeCapture();
    //UseWebcam.StopCamera();
    $("#webcam-switch").prop("checked", false).change();
    $('.md-modal').removeClass('md-show');
    $('body').removeClass('hideScroll');
    $('#CameraPopup').removeClass("slide-active");
});
function dataURLtoFile(dataurl, filename) {

    var arr = dataurl.split(','),
        mime = arr[0].match(/:(.*?);/)[1],
        bstr = atob(arr[1]),
        n = bstr.length,
        u8arr = new Uint8Array(n);

    while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
    }

    return new File([u8arr], filename, { type: mime });
}

$(document).delegate("#UploadWebCamImg", 'click', function () {
    $('body').removeClass('hideScroll');
    if ($('#imgDisply').attr('src') != '') {
        var ticks = ((new Date().getTime() * 10000) + 621355968000000000);
        var file = dataURLtoFile($('#imgDisply').attr('src'), 'imageFromWebCam_' + ticks + '.png');
        if (file.size > (1024 * 1024 * 10)) // not more than 10mb
        {
            ShowImageSizeExceedAlert();
            return false;
        }
        var formData = new FormData();
        formData.append(file.name, file);
        $.ajax({
            url: "../Base/SaveUploadedFile",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            beforeSend: function () {
                ShowLoader();
            },
            success: function (res) {
                removeCapture();
                UseWebcam.StopCamera();
                $("#webcam-switch").prop("checked", false).change();
                $('.md-modal').removeClass('md-show');
                $('body').removeClass('hideScroll');
                $('#CameraPopup').removeClass("slide-active");
                var WorkOrderId = $(document).find('#WorkOrderId').val();
                SaveUploadedFileToServer(WorkOrderId, file.name);
            },
            error: function (xhr) {
                CloseLoader();
            }
        });
    }
});

$(document).on('click', '.CloseCameraPopup', function () {
    $('#CameraPopup').removeClass('slide-active');

});
//#endregion
//#endregion
//#region V2-726

$(document).on('click', "#sendmaterialrequestitemsforapproval", function (e) {
    e.preventDefault();
    $('.modal-backdrop').remove();
    GoToSendMRForApproval();
});

function GoToSendMRForApproval() {
    var WorkOrderId = $(document).find('#WorkOrderId').val();
    $.ajax({
        url: "/Dashboard/SendMRForApproval_Mobile",
        type: "GET",
        dataType: 'html',
        data: { WorkOrderId: WorkOrderId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data != '') {
                $('#divSendForApproval').html(data);
            }
        },
        complete: function () {
            if ($('#divSendForApproval').html() != '') {
                SetControls();
                $('#SendForApprovalPopup').parent().addClass("slide-active");
                $('#SendForApprovalPopup').trigger('mbsc-enhance');
            }
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}

$(document).on('click', "#btncancelsendForApproval", function () {
    $('#SendForApprovalPopup').parent().removeClass('slide-active');
    //var workorderId = $(document).find('#ApprovalRouteModel_ObjectId').val();
    //dtEstimatesPartTable = undefined;
    //var CurrentTabForDashboard = 'MaterialRequests';
    //RedirectToMaintenanceWorkBenchDetailFromDashboard(workorderId, '', CurrentTabForDashboard);
});

function SendMRForApprovalOnSuccess(data) {
    $(document).find('#SendForApprovalModalPopup').modal('hide');
    var WorkOrderId = data.WorkOrderId;
    var ApprovalGroupId = data.ApprovalGroupId;
    if (data.data === "success") {
        if (data.ApprovalGroupId >= 0) {
            SuccessAlertSetting.text = getResourceValue("SendApprovalSuccessAlert");
            swal(SuccessAlertSetting, function () {
                CloseLoader();
                RedirectToMaintenanceWorkBenchDetail(WorkOrderId, '', 'MaterialRequests');
            });

        }
        //else {
        //    ErrorAlertSetting.text = "You have been not assigned any Approval Group";
        //    swal(ErrorAlertSetting, function () {
        //        CloseLoader();
        //    });
        //}
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
    //$('#Approver').val(null).trigger("change.select2");

}

$(document).on('click', '.SendMaterialRequestclearstate', function () {
    $('#SendForApprovalPopup').parent().removeClass('slide-active');

});
//#region V2-734
$(document).on('focus click', '.mbsc-control,.search', function () {
    var isAndroid = /(android)/i.test(navigator.userAgent);
    if (isAndroid) {
        var inputtype = $(this).attr("type");
        if ($(this).is("select")) {
            inputtype = "select";
        }
        else if ($(this).is("textarea")) {
            inputtype = "text";
        }
        //if (inputtype == "search") {
        //    inputtype = "text";
        //}
        var classtype = "string";
        if ($(this).hasClass("mbsc-select-input")) {
            classtype = "dropdown";
        }
        else if ($(this).hasClass("dropbtn")) {
            classtype = "dropdown";
        }
        else if ($(this).hasClass("dtpicker")) {
            classtype = "date";
        }
        else if ($(this).hasClass("decimalinput")) {
            classtype = "decimal";
        }
        else if ($(this).hasClass("mbsc-sel-filter-input")) {
            classtype = "select-filter-text";
        }
        if (inputtype.trim() == "text" && classtype == "string") {
            //$(this).parentsUntil("div").addClass("mobitxtfieldactive");
            $(this).parent().closest('div').addClass("mobitxtfieldactive");
        }
        if (inputtype.trim() == "search" && classtype == "string") {
            //$(this).parentsUntil("div").addClass("mobitxtfieldactive");
            $(document).find(".m-grid__item.m-header").hide();
            $(this).parent().closest('div').addClass("mobitxtfieldactive");
        }
        //if ($(window).width() <= 767) {
        //    $('html, body').animate({
        //        scrollTo: $('input').offset().top
        //    }, 'fast');
        //}
    }
});
$(document).on('focusout', '.mbsc-control,.search', function () {
    var isAndroid = /(android)/i.test(navigator.userAgent);
    if (isAndroid) {
        var inputtype = $(this).attr("type");
        if ($(this).is("select")) {
            inputtype = "select";
        }
        else if ($(this).is("textarea")) {
            inputtype = "text";
        }
        //if (inputtype == "search") {
        //    inputtype = "text";
        //}
        if (inputtype.trim() == "text") {
            //$(".mobitxtfieldactive").removeAttr("style");
            $(this).parent().closest('div').removeClass("mobitxtfieldactive");
        }
        if (inputtype.trim() == "search") {
            //$(this).parentsUntil("div").addClass("mobitxtfieldactive");
            //$(this).parent().closest('div').addClass("mobitxtfieldactive");
            $(document).find(".m-grid__item.m-header").show();
            //$(".mobitxtfieldactive").removeAttr("style");
            $(this).parent().closest('div').removeClass("mobitxtfieldactive");
        }
    }
});
//#endregion
//#endregion
//#endregion

//#region V2-735 redirect to WODetailspage ondemand success
function RedirectToWODetail(WOId) {

    localStorage.setItem("workorderstatus", '1');
    localStorage.setItem("workorderstatustext", ("Scheduled" + " " + getResourceValue("spnWorkOrder")));
    window.location.href = '../WorkOrder/DetailFromDashboard?workOrderId=' + WOId;

}

//#endregion

//#region V2-732
function PopulateStorerooms() {
    $(document).find('#AddEstimatedParts').modal('hide');
    $.ajax({
        url: "/Dashboard/PopulateStorerooms_Mobile",
        type: "GET",
        dataType: 'html',
        //data: { MaterialRequestId: MaterialRequestId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#StoreroomListPopUp').html(data);
            //$('#StoreroomListModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            if ($('#StoreroomListPopUp').html() != '') {
                SetControls();
                $('#StoreroomListModalpopup').parent().addClass("slide-active");
                $('#StoreroomListModalpopup').trigger('mbsc-enhance');
            }
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}

function SelctStoreroomOnSuccess_Mobile(data) {
    var WorkOrderId = $(document).find('#WorkOrderId').val();
    //$(document).find('#StoreroomListPopUp').modal('hide');
    if (data.data === "success") {
        $('#StoreroomListModalpopup').parent().removeClass('slide-active');
        $('#PartWo_Mobile').removeClass('slide-active');
        GoToAddPartInInventory(WorkOrderId);
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btnSubmitStoreroomcancel,.clearstate', function () {
    $('#StoreroomListModalpopup').parent().removeClass('slide-active');

});
//#endregion

//#region V2-791
$(document).on('change', '#add_photos', function () {
    var val = $(this).val();
    var imageName = val.replace(/^.*[\\\/]/, '');
    //image name set
    var WorkOrderId1 = $(document).find('#WorkOrderId').val();
    var imgname = WorkOrderId1 + "_" + Math.floor((new Date()).getTime() / 1000);
    //
    var fileUpload = $("#add_photos").get(0);
    var files = fileUpload.files;
    var fileExt = imageName.substr(imageName.lastIndexOf('.') + 1);
    if (fileExt != 'jpeg' && fileExt != 'jpg' && fileExt != 'png' && fileExt != 'JPEG' && fileExt != 'JPG' && fileExt != 'PNG') {
        ShowErrorAlert(getResourceValue("spnValidImage"));
        $('#add_photos').val('');
        //e.preventDefault();
        return false;
    }
    else if (this.files[0].size > (1024 * 1024 * 10)) {
        ShowImageSizeExceedAlert();
        $('#add_photos').val('');
        //e.preventDefault();
        return false;
    }
    //var duplicate_chk = SaveUploadedFileToServer(WorkOrderId, imageName);
    //if () { }
    else {
        //alert('Hello');
        swal(AddImageAlertSetting, function () {
            if (window.FormData !== undefined) {

                // Create FormData object  
               // var fileData = new FormData();

                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    //fileData.append(imgname + "." + fileExt, files[i]);
                    CompressImage(files[i], imgname + "." + fileExt);
                }

                // Adding one more key to FormData object  
                //fileData.append('username', ‘Manas’);

                //$.ajax({
                //    url: '../base/SaveUploadedFile_Mobile',
                //    type: "POST",
                //    contentType: false, // Not to set any content header  
                //    processData: false, // Not to process data  
                //    data: fileData,
                //    success: function (result) {
                //        var WorkOrderId = $(document).find('#WorkOrderId').val();
                //        SaveUploadedFileToServer(WorkOrderId, /*imageName*/imgname + "." + fileExt);

                //        $('#add_photos').val('');
                //    }
                //});

            }
            else {
                //alert("FormData is not supported.");
                //msg = "notanimage";
            }
            //if (msg != null && msg == "imagesizeexceed") {
            //    ShowImageSizeExceedAlert();
            //}
            //if (msg != null && msg == "notanimage") {
            //    ShowErrorAlert(getResourceValue("spnValidImage"));
            //}
        });
    }
});
//#endregion

//#region V2-865

var FilesAddProccess;
var fileExtAddProccess = "";
$(document).on('change', '.addphotoWorkorder', function () {
    
    var id = $(this).attr('id');
    var val = $(this).val();
   var previewid= $(this).closest(".takePic").find("img").attr('id');
    var imageName = val.replace(/^.*[\\\/]/, '');
    var fileUpload = $("#" + id).get(0);
    var fileExt = imageName.substr(imageName.lastIndexOf('.') + 1);
   
    if (fileExt != 'jpeg' && fileExt != 'jpg' && fileExt != 'png' && fileExt != 'JPEG' && fileExt != 'JPG' && fileExt != 'PNG') {
        ShowErrorAlert(getResourceValue("spnValidImage"));
        $("#" + id).val('');
        //e.preventDefault();
        return false;
    }
    else if (this.files[0].size > (1024 * 1024 * 10)) {
        ShowImageSizeExceedAlert();
        $("#" + id).val('');
        //e.preventDefault();
        return false;
    }
   
    else {
       
        if (window.FormData !== undefined) {
            var url = window.URL.createObjectURL(this.files[0]);
            $('#' + previewid).attr('src', url);
            FilesAddProccess = fileUpload.files;
            fileExtAddProccess = fileExt;
        }
        else {
            
        }
           
    }
});
function CompressImageAddProccess(files, imageName, WorkOrderId) {
    new Compressor(files, {
        quality: 0.6,
        convertTypes: ['image/png'],
        convertSize: 100000,
        // The compression process is asynchronous,
        // which means you have to access the `result` in the `success` hook function.
        success(result) {
            if (result.size < files.size) {
                SaveCompressedImageAddProccess(result, imageName, WorkOrderId);
            }
            else {
                SaveCompressedImageAddProccess(files, imageName, WorkOrderId);
            }
            console.log('file name ' + result.name + ' after compress ' + result.size);
        },
        error(err) {
            console.log(err.message);
        },
    });
}
function SaveCompressedImageAddProccess(data, imageName, WorkOrderId) {
    var AddPhotoFileData = new FormData();
    AddPhotoFileData.append('file', data, imageName);

    $.ajax({
        url: '../base/SaveUploadedFile',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: AddPhotoFileData,
        success: function (result) {
            SaveUploadedFileToServerAddProccess(WorkOrderId, imageName);
            $('#add_photosWR').val('');
        }
    });
}
function SaveUploadedFileToServerAddProccess(WorkOrderId, imageName) {
    $.ajax({
        url: '../base/SaveMultipleUploadedFileToServer',
        type: 'POST',

        data: { 'fileName': imageName, objectId: WorkOrderId, TableName: "WorkOrder", AttachObjectName: "WorkOrder" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {
                
                CloseLoader();
                /*ShowImageSaveSuccessAlert();*/
            }
            else if (data.result == "1") {
                CloseLoader();
                var errorMessage = getResourceValue("ImageExistAlert");
                ShowErrorAlert(errorMessage);

            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
                ShowErrorAlert(errorMessage);

            }
        },
        complete: function () {
            
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion



//#region V2-1056 AddSanitationRequestWO
$(document).on('click', "#AddSanitationRequestBtn_Mobile", function (e) {
    var workOrderId = parseInt($(document).find('#WorkOrderId').val());
    var clientLookupId = $(document).find('#WorkOrderClientLookupId').val();
    $.ajax({
        url: "/Dashboard/AddSanitationRequestWO_Mobile",
        type: "GET",
        dataType: 'html',
        data: {
            'WorkoderId': workOrderId, 'ClientLookupId': clientLookupId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddSanitationRequestWODashboardPopUp').html(data);
        },
        complete: function () {
            SetControls();
            $('#AddSanitationRequestWODashboardModalpopup').parent().addClass('slide-active');
            $('#AddSanitationRequestWODashboardModalpopup').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', '.btnCancelAddSanitationRequestWODashboard', function () {
    $('#AddSanitationRequestWODashboardModalpopup').parent().removeClass('slide-active');
    $('#AddSanitationRequestWODashboardPopUp').html('');
});
function AddSanitationRequestWODashboardOnSuccess(data) {
    if (data.Issuccess) {
        SuccessAlertSetting.text = getResourceValue("SanitationRequestAddAlert");
        swal(SuccessAlertSetting, function () {
            $('#AddSanitationRequestWODashboardModalpopup').parent().removeClass('slide-active');
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#AddSanitationRequestWODashboardModalpopup');
    }
    CloseLoader();
}
//#endregion
//#region V2-1067
$(document).on('click', '.btnCancelDescribeDynamic', function () {
    var areaChargeToId = "";
    $(document).find('#AddDescribeWOModalpopupDynamic select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
    $('#AddDescribeWOModalpopupDynamic').removeClass('slide-active');
    $('#AddDescribeWOModalpopupDynamic').html('');
});
//#endregion