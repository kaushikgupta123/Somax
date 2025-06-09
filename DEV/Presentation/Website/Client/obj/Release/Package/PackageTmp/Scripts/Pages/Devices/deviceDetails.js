var dvEventTable;
$(function () {

    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        $('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
});

function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "DVEvents":
            generateEventGrid();
            break;
    }
    var i, tabcontent;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    document.getElementById(cityName).style.display = "block";
    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}
$(document).on('click', "#brddevice", function () {
    var ioTDeviceId = $(this).attr('data-val');
    var sensorid = $('#deviceModel_SensorId').val();
    RedirectToDeviceDetail(ioTDeviceId, "dvoverview", sensorid);
});
function RedirectToDeviceDetail(ioTDeviceId, mode, sensorid) {
    $.ajax({
        url: "/Devices/DeviceDetails",
        type: "POST",
        dataType: 'html',
        data: { ioTDeviceId: ioTDeviceId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderdevices').html(data);
            if (IsFromEquipment == true) {
                $(document).find("#linkToEquipment").text(EquipmentClientLookupId);
                $(document).find("#EquipmentId").val(RedirectEquipmentId);
            }
            $(document).find('#spnlinkToSearch').text(localStorage.getItem("devicestatustext"));
        },
        complete: function () {
            colorarray = [];
            GenerateGaugeData();
            CreateTimeSeriesChart(sensorid);
            if (mode === "dvoverview") {
                $('#dvoverview').trigger('click');
                $('#colorselector').val('DVOverview');
            }
            if (mode === "dvevents") {
                $('#dvevents').trigger('click');
                $('#colorselector').val('DVEvents');
            }
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#region V2-1105
var IsFromEquipment = false;
var RedirectEquipmentId;
var EquipmentClientLookupId;
$(document).ready(function () {
    if ($(document).find("#IsFromEquipment").val() == "True") {
        IsFromEquipment = true;
        EquipmentClientLookupId = $(document).find("#linkToEquipment").text();
        RedirectEquipmentId = $(document).find("#EquipmentId").val();
    }
});
$(document).on('click', '#linkToEquipment', function (e) {
    clearDropzone();
    var EquipmentId = $(document).find('#EquipmentId').val();
    window.location.href = "../Equipment/DetailFromWorkOrder?EquipmentId=" + EquipmentId;
});
//#endregion
function RedirectToDetailOncancel(deviceId, mode, sensorid) {
    swal(CancelAlertSetting, function () {
        RedirectToDeviceDetail(deviceId, mode, sensorid);
    });
}
//#region Event
function generateEventGrid() {
    var ioTDeviceId = $(document).find('#deviceModel_IoTDeviceId').val();
    var deviceClientLookupId = $(document).find('#deviceModel_ClientLookupID').val();
    if ($(document).find('#dvEventTable').hasClass('dataTable')) {
        dvEventTable.destroy();
    }
    dvEventTable = $("#dvEventTable").DataTable({
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
        "order": [[0, "asc"]],
        stateSave: false,
        dom: 'Btlipr',
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        buttons: [],
        "filter": true,
        "orderMulti": true,
        "ajax": {
            "url": "/Devices/PopulateEvent?deviceClientLookupId=" + deviceClientLookupId,
            "type": "POST",
            data: { deviceClientLookupId: deviceClientLookupId, ioTDeviceId: ioTDeviceId },
            "datatype": "json",
            "dataSrc": function (response) {
                return response.data;
            }
        },
        "columns":
            [
                { "data": "EventID", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-150'>" + data + "</div>";
                    }
                },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Disposition", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Created", defaultContent: "", "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            CloseLoader();
        }
    });
}
//#endregion

//#region Gauge
function GenerateGaugeData() {
    var ioTDeviceId = $(document).find('#deviceModel_IoTDeviceId').val();
    var deviceClientLookupId = $(document).find('#deviceModel_ClientLookupID').val();
    $.ajax({
        type: "GET",
        url: "/Devices/GenerateArcGauge",
        beforeSend: function () {
            ShowbtnLoader("SrchBttnDevice");
        },
        data: { ioTDeviceId: ioTDeviceId, deviceClientLookupId: deviceClientLookupId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            CreateArc(response);
        },
        complete: function () {
            $('#gaugeloader').hide();
        }
    });
}
function CreateArc(response) {
    var opts = {
        lines: 1,
        angle: 0.2,
        lineWidth: 0.1,
        radiusScale: 1,
        pointer: {
            length: 0.5,
            strokeWidth: 0,
            color: '#0caff0'
        },
        limitMax: 'false',
        //percentColors: [[0.0, "#FF4B57"], [0.40, "#EBEB3A"], [0.48, "#5ACF40"], [0.481, '#FFF'], [0.52, "#EBEB3A"], [0.60, "#FF4B57"]],
        strokeColor: '#EEEEEE',
        generateGradient: false,
        highDpiSupport: true
    };
    var target = document.getElementById('cnvArc');
    var gauge = new Donut(target).setOptions(opts);
    gauge.maxValue = 3000;
    gauge.setMinValue(0);
    gauge.animationSpeed = 38;
    var s = response.LastReading;
    var v = 0;
    gauge.set(s + v);
    $(document).find('#spnTextVal').text(response.ReadingWithUnit);
}
//#endregion

//#region Edit
$(document).on('click', '#btnEditDevice', function (e) {
    e.preventDefault();
    var ioTDeviceId = $(document).find('#deviceModel_IoTDeviceId').val();
    $.ajax({
        url: "/Devices/EditDeviceCategory",
        type: "GET",
        dataType: 'html',
        data: { IoTDeviceId: ioTDeviceId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderdevices').html(data);
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function EditDeviceOnSuccess(data) {
    CloseLoader();
    if (data.data === "success") {
        if (data.Command === "add" || data.Command === "update") {
            if (data.mode === "add") {
                localStorage.setItem("workorderstatus", '3');
                localStorage.setItem("workorderstatustext", getResourceValue("spnOpenWorkOrder"));
                SuccessAlertSetting.text = getResourceValue("DeviceAddAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("DeviceUpdateAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToDeviceDetail(data.IotDeviceId, "dvoverview", data.SensorId);
            });
        }
        else {
            SuccessAlertSetting.text = "Updation failed";
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('#requesttab').addClass('active').trigger('click');
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").not("#colorselector").val("").trigger('change');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnCancelAddDevice", function () {
    var ioTDeviceId = $(document).find('#deviceModel_IoTDeviceId').val();
    if (ioTDeviceId == 0) {
        swal(CancelAlertSetting, function () {
            window.location.href = "../Devices/Index?page=Monitoring_Device_Search";
        });
    } else {
        var sensorid = $(document).find('#deviceModel_SensorId').val();
        RedirectToDetailOncancel(ioTDeviceId, "dvoverview", sensorid);
    }
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
            if (typeof areaddescribedby !== 'undefined') {
                $('#' + areaddescribedby).hide();
            }
        }
        else {
            if (typeof areaddescribedby !== 'undefined') {
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
    SetFixedHeadStyle();
};
//#endregion

//#region Asset popup
$(document).on('click', "#openAssetGrid", function () {
    var textAssetID = $("#deviceModel_IoTDeviceId").val();
    generateAssetDataTable();
});
//#endregion


//#region Options
$(document).on('click', '#activeInactiveDevice', function () {
    var deviceId = $(document).find('#deviceModel_IoTDeviceId').val();
    var inactiveFlag = $(document).find('#InactiveFlagId').val();
    if (inactiveFlag == "True") {
        CancelAlertSetting.text = getResourceValue("ActivateDeviceAlert");
    }
    else {
        CancelAlertSetting.text = getResourceValue("InActivateDeviceAlert");
    }
    swal(CancelAlertSetting, function (isConfirm) {
        if (isConfirm == true) {
            $.ajax({
                url: "/Devices/UpdateDeviceStatus",
                type: "POST",
                dataType: "json",
                data: {
                    deviceId: deviceId,
                    inactiveFlag: inactiveFlag
                },
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (data) {
                    if (data.result == 'success') {
                        if (inactiveFlag == "True") {
                            SuccessAlertSetting.text = getResourceValue("DeviceActiveSuccessAlert");
                        }
                        else {
                            SuccessAlertSetting.text = getResourceValue("DeviceInActiveSuccessAlert");
                        }
                        swal(SuccessAlertSetting, function () {
                            var sensorid = $('#deviceModel_SensorId').val();
                            RedirectToDeviceDetail(deviceId, "dvoverview", sensorid);
                        });
                    }
                    else {
                        ShowGenericErrorOnAddUpdate(data);
                    }
                },
                complete: function () {
                    CloseLoader();
                },
                error: function () {
                    CloseLoader();
                }
            });
        }
    });

});

$(document).on('click', '#optchangedeviceid', function (e) {
    var clientlookupid = $(document).find('#OldClientLookupId').val();
    $(document).find('#txtIoTDeviceId').val(clientlookupid).removeClass('input-validation-error');
    $('#changeDeviceIDModalDetailsPage').modal('show');
    $.validator.unobtrusive.parse(document);
    $(this).blur();
});

function ChangeIoTDeviceIdOnSuccess(data) {
    $('#changeDeviceIDModalDetailsPage').modal('hide');
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("DeviceIdUpdateAlert");
        swal(SuccessAlertSetting, function () {
            var sensorid = $('#deviceModel_SensorId').val();
            var deviceId = $(document).find('#deviceModel_IoTDeviceId').val();
            RedirectToDeviceDetail(deviceId, "dvoverview", sensorid);
        });
    }
    else {
        GenericSweetAlertMethod(data);
    }
}
//#endregion

//#region V2-1103
$(document).on('click', '#optAddRecordReading', function (e) {
    $('#addRecordReadingModalDetailsPage').modal('show');
    AddReadingRecordDatePicker();
    var timerVal = moment().format('hh:mm A');
    $('.timerId').val(timerVal)
    $.validator.unobtrusive.parse(document);
    $(this).blur();
});
function AddRecordReadingOnSuccess(data) {
    $('#addRecordReadingModalDetailsPage').modal('hide'); 
    var sensorid = $('#deviceModel_SensorId').val();
    var deviceId = $(document).find('#deviceModel_IoTDeviceId').val();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("IoTReadingRecordAddAlert");
        swal(SuccessAlertSetting, function () {
           RedirectToDeviceDetail(deviceId, "dvoverview", sensorid);
        });
    }
    else {
        GenericSweetAlertMethod(data);
    }
}
function AddReadingRecordDatePicker() {
    $(".dtpickerNew").datepicker({
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        changeMonth: true,
        changeYear: true,
        showOn: 'button',
        buttonImageOnly: true,
        buttonImage: '/Images/calender.png'
    }).inputmask('mm/dd/yyyy');
    $('.dtpickerNew').datepicker('setDate', new Date());
    var timerVal = moment().format('hh:mm A');
    $('.timerId').timepicker(
        {
            template: 'dropdown',
            minuteStep: 1,
            showMeridian: true,
            defaultTime: timerVal
        });
}
//#endregion