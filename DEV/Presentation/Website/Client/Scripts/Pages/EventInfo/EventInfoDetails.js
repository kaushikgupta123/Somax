//#region Event Based Work Order
$(document).on('click', "#AddEventDemand", function (e) {
    var eventInfoId = $(document).find('#eventInfoModel_EventInfoId').val();
    e.preventDefault();
    AddEventWoOnDemand(eventInfoId);
    $('#modalCreateWorkOrder').modal('hide');
    $('.modal-backdrop').remove();
});
function AddEventWoOnDemand(eventInfoId) {
    $.ajax({
        url: "/EventInfo/AddWoOnDemand",
        type: "POST",
        dataType: "html",
        data: { eventInfoId: eventInfoId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendereventinfo').html(data);
        },
        complete: function () {
            CloseLoader();
            SetControls();
            $("#imgChargeToTree").css('visibility', 'hidden');
        },
        error: function () {
            CloseLoader();
        }
    });
}
function AddEventWoOnDemandOnSuccess(data) {
    if (data.data == "success") {
        {           
            SuccessAlertSetting.text = getResourceValue("WorkOrderAddAlert")
            swal(SuccessAlertSetting, function () {
                GoEventDescribe(data.eventInfoId);
            });

        }
    }
    else {
        CloseLoader();
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion Event Based Work Order

//#region Event Describe
$(document).on('click', "#AddEventDescribe", function (e) {
    var EventInfoId = $(document).find('#eventInfoModel_EventInfoId').val();
    e.preventDefault();
    AddEventDescribe(EventInfoId);
    $('#modalCreateWorkOrder').modal('hide');
    $('.modal-backdrop').remove();
});
function AddEventDescribe(EventInfoId) {
    $.ajax({
        url: "/EventInfo/AddEventInfoDescribe",
        type: "POST",
        dataType: "html",
        data: { EventInfoId: EventInfoId},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendereventinfo').html(data);
        },
        complete: function () {
            CloseLoader();
            SetControls();
            $("#imgChargeToTree").css('display', 'none');
        },
        error: function () {
            CloseLoader();
        }
    });
}
function EventDescribeAddOnSuccess(data) {
    if (data.data == "success") {
        {
            SuccessAlertSetting.text = getResourceValue("WorkOrderAddAlert")
            swal(SuccessAlertSetting, function () {
                GoEventDescribe(data.EventInfoId);
            });
        }
    }
    else {
        CloseLoader();
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '.btnCancelDataval', function (e) {
    var EventInfoId = $(this).attr('data-val');
    GoEventDescribe(EventInfoId)
});
function GoEventDescribe(EventInfoId) {
    $.ajax({
        url: "/EventInfo/EventInfoDetails",
        type: "POST",
        data: { EventId: EventInfoId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendereventinfo').html(data);
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

}
//#endregion Event Based Work Order
//#region common
$(document).on('click', "#brdevntinfo", function () {
    var evntInfoId = $(this).attr('data-val');
    GoEventDescribe(evntInfoId);
});
function SetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
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
    });
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
}
//#endregion common

