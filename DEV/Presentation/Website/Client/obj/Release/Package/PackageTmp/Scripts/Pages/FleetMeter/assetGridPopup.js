var equipTable;
var EqClientLookupId = "";
var EqName = "";
var EqModel = "";
var EqMake = "";
var EqVIN = "";
$(document).on('click', '#opengrid', function () {
    generateFleetMeterAssetTable();
});
function generateFleetMeterAssetTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#EquipPopupTable').hasClass('dataTable')) {
        equipTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    equipTable = $("#EquipPopupTable").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        "pageLength": 10,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Base/FleetAssetLookupList",
            data: function (d) {
                d.clientLookupId = EqClientLookupId;
                d.name = EqName;
                d.model = EqModel;
                d.make = EqMake;
                d.vin = EqVIN;
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
                "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<a class=link_fueleqp_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Make", "autoWidth": true, "bSearchable": true, "bSortable": true
            },
            { "data": "Model", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "VIN", "autoWidth": true, "bSearchable": true, "bSortable": true },
        ],
        "rowCallback": function (row, data, index, full) {
            var colType = this.api().column(3).index('visible');
        },
        initComplete: function () {
            $(document).find('#tbleqpfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#FMassetModal').hasClass('show')) {
                $(document).find('#FMassetModal').modal("show");
            }
            $('#EquipPopupTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#EquipPopupTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (EqClientLookupId) { $('#colindex_0').val(EqClientLookupId); }
                if (EqName) { $('#colindex_1').val(EqName); }
                if (EqModel) { $('#colindex_3').val(EqModel); }
                if (EqMake) { $('#colindex_2').val(EqMake); }
                if (EqVIN) { $('#colindex_4').val(EqVIN); }
            });
            $('#EquipPopupTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    EqClientLookupId = $('#colindex_0').val();
                    EqName = $('#colindex_1').val();
                    EqModel = $('#colindex_3').val();
                    EqMake = $('#colindex_2').val();
                    EqVIN = $('#colindex_4').val();
                    equipTable.page('first').draw('page');
                }
            });
        }
    });
}
$(document).ready(function () {
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
});
$(document).on('click', '.link_fueleqp_detail', function (e) {
    ResetAllContent();
   
    var index_row = $('#EquipPopupTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = equipTable.row(row).data();
    $(document).find('#fleetMeterModel_ClientLookupId').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#fleetMeterModel_Meter1CurrentReading').removeClass('input-validation-error');
    $(document).find('#fleetMeterModel_Meter2CurrentReading').removeClass('input-validation-error');
    $(document).find('#fleetMeterModel_EquipmentId').val(parseInt(data.EquipmentId));
    $(document).find('#fleetMeterModel_Meter1Type').val(data.Meter1Type);
    $(document).find('#fleetMeterModel_Meter2Type').val(data.Meter2Type);
    

    $(document).find('#FM1CurrentReading').val(data.Meter1CurrentReading);
    $(document).find('#FM2CurrentReading').val(data.Meter2CurrentReading);
    var M1unit = data.Meter1Unit;
    if (M1unit != "") {
        M1unit = M1unit.substring(0, 2);
    }
    var M2unit = data.Meter2Unit;
    if (M2unit != "") {
        M2unit = M2unit.substring(0, 2);
    }
    $(document).find('#spnMeter1dayDiff').text(getResourceValue("LastReadingAlert")+': ' + data.Meter1CurrentReading + ' ' + M1unit + ' (' + data.Meter1DayDiff + '   ' + getResourceValue("DaysAgoAlert")+')');
    $(document).find('#spnMeter2dayDiff').text(getResourceValue("LastReadingAlert") +': ' + data.Meter2CurrentReading + ' ' + M2unit + ' (' + data.Meter2DayDiff + '   ' + getResourceValue("DaysAgoAlert") + ')');
    enableDisableFleetControls(data.Meter1CurrentReading, data.Meter2CurrentReading, data.Meter1Type, data.Meter2Type);
    hideFleetControls(data.Meter1Type, data.Meter2Type);
    $(document).find("#FMassetModal").modal('hide');
    $(document).find('#spnMeter1dayDiff').hide();
    $(document).find('#spnMeter2dayDiff').hide();
});

function hideFleetControls(meter1Type, meter2Type)
{
    if (meter1Type != null && meter1Type != '')
    {
        $(document).find('#liOdometer').show();
        $(document).find('#liOdometerVoid').show();
        $(document).find('#fleetMeterModel_MetersAssociated').val('single');
    }
    else {
        $(document).find('#liOdometer').hide();
        $(document).find('#liOdometerVoid').hide();
    }
    if (meter2Type != null && meter2Type != '') {
        $(document).find('#liHour').show();
        $(document).find('#liHourVoid').show();
        $(document).find('#fleetMeterModel_MetersAssociated').val('both');
    }
    else {
        $(document).find('#liHour').hide();
        $(document).find('#liHourVoid').hide();
        $(document).find('#fleetMeterModel_Meter2CurrentReading').val("1");
        $(document).find('#fleetMeterModel_Meter2CurrentReading').removeAttr('disabled');
    }
    if (meter1Type != null && meter1Type != '' || meter2Type != null && meter2Type != '') {
        $("#errmsg").css("display", "none");
    }
    else {
        $("#errmsg").css("display", "block");
    }
}

function enableDisableFleetControls(meter1CurrentReading, meter2CurrentReading, Meter1Type, Meter2Type)
{
    
    if (meter1CurrentReading != "0.0") {
        $(document).find("#fleetMeterModel_Meter1CurrentReading").removeAttr('disabled');
        $(document).find("#fleetMeterModel_Meter1Void").removeAttr('disabled');
        $(document).find("#fleetMeterModel_CurrentReadingDate").removeAttr('disabled');
        $(document).find("#fleetMeterModel_CurrentReadingTime").removeAttr('disabled');
        $(document).find("#btnAddMeterRecord").removeAttr('disabled');
    }
    else {
        if (Meter1Type != null && Meter1Type != '') {
            $(document).find("#fleetMeterModel_Meter1CurrentReading").removeAttr('disabled');
            $(document).find("#fleetMeterModel_Meter1Void").removeAttr('disabled');
            $(document).find("#fleetMeterModel_CurrentReadingDate").removeAttr('disabled');
            $(document).find("#fleetMeterModel_CurrentReadingTime").removeAttr('disabled');
            $(document).find("#btnAddMeterRecord").removeAttr('disabled');
        }
        else {
            $(document).find("#fleetMeterModel_Meter1CurrentReading").prop("disabled", true);
            $(document).find("#fleetMeterModel_Meter1Void").prop("disabled", true);
            $(document).find("#fleetMeterModel_CurrentReadingDate").prop("disabled", true);
            $(document).find("#fleetMeterModel_CurrentReadingTime").prop("disabled", true);
            $(document).find("#btnAddMeterRecord").prop("disabled", true);
        }
       
    }
    if (meter2CurrentReading != "0.0") {
        $(document).find("#fleetMeterModel_Meter2CurrentReading").removeAttr('disabled');
        $(document).find("#fleetMeterModel_Meter2Void").removeAttr('disabled');
    }
    else {
        if (Meter2Type != null && Meter2Type != '') {
            $(document).find("#fleetMeterModel_Meter2CurrentReading").removeAttr('disabled');
            $(document).find("#fleetMeterModel_Meter2Void").removeAttr('disabled');
        }
        else {
            $(document).find("#fleetMeterModel_Meter2CurrentReading").prop("disabled", true);
            $(document).find("#fleetMeterModel_Meter2Void").prop("disabled", true);
        }
       
    }
}

function ResetAllContent() {
    $(document).find("#fleetMeterModel_Meter1CurrentReading").val("0");
    $(document).find("#fleetMeterModel_Meter1Void").prop("checked", false);
    $(document).find(".dtpickerNew").val(GetCurrentDate());
       
    var timerVal = moment().format('hh:mm A');
    $('.timerId').val(timerVal);    
    $(document).find("#fleetMeterModel_Meter2CurrentReading").val("0");
    $(document).find("#fleetMeterModel_Meter2Void").prop("checked", false);
    $("#errmsg").css("display", "none");
    $(".errormessage").css("display", "none");
}

function GetCurrentDate() {
    var d = new Date();

    var month = d.getMonth() + 1;
    var day = d.getDate();

    var output = (('' + month).length < 2 ? '0' : '') + month + '/' +
        (('' + day).length < 2 ? '0' : '') + day + '/' + d.getFullYear();

    return output;
    
}



