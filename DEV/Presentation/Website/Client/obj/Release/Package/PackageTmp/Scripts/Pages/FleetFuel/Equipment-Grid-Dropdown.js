var equipTable;
var EqClientLookupId = "";
var EqName = "";
var EqModel = "";
var EqMake = "";
var EqVIN = "";
$(document).on('click', '#opengrid', function () {
    generateFleetFuelTable();
});
function generateFleetFuelTable() {
    var EquipmentId = $('#partsSessionData_EquipmentId').val();
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
            if (!$(document).find('#FleetFuelModal').hasClass('show')) {
                $(document).find('#FleetFuelModal').modal("show");
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
    var index_row = $('#EquipPopupTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = equipTable.row(row).data();
    $(document).find('#FleetFuelModel_ClientLookupId').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find("#FleetFuelModal").modal('hide');
    $(document).find('.resetText').val("0");
    $(document).find('form').find("select").not("#colorselector").val("").trigger('change.select2');
    $(document).find('form').find("select").removeClass("input-validation-error");
    $(document).find('form').find("input").removeClass("input-validation-error");
    $(document).find('form').find("textarea").removeClass("input-validation-error");
    $(document).find('.reset').prop("checked", false);
    $(document).find(".Chckerror").css("display", "none");
    $(document).find('.errormessage').css("display", "none");
    $(document).find(".dtpickerNew").val(GetCurrentDate());
    var timerVal = moment().format('hh:mm A');
    $('.timerId').val(timerVal);

    if (data.Meter1Type != "") {
        $("#errmsg").css("display", "none");
        $(document).find("#FleetFuelModel_MtrCurrentReadingDate").removeAttr('disabled');
        $(document).find("#FleetFuelModel_StartTimeValue").removeAttr('disabled');
        $(document).find("#FleetFuelModel_Void").removeAttr('disabled');
        $(document).find("#FleetFuelModel_UnitCost").removeAttr('disabled');
        $(document).find("#FleetFuelModel_FuelType").removeAttr('disabled');
        $(document).find("#FleetFuelModel_FuelAmount").removeAttr('disabled');
        $(document).find("#FleetFuelModel_Reading").removeAttr('disabled');
        $(document).find("#btnFleetFuelRecord").removeAttr('disabled');
        $(document).find("#FleetFuelModel_PrevMeter1Reading").val(data.Meter1CurrentReading);
        //for set meter reading value
        var getdiff = GetDateDiff(data.Meter1CurrentReadingDate);
        var Munit = data.Meter1Unit;
        if (Munit != "") {
            Munit = Munit.substring(0, 2);
        }
        var chkerr = getResourceValue("LastReadingAlert") + ': '  + data.Meter1CurrentReading + " " + Munit + " (" + getdiff + ")";
        $(document).find(".Chckerror").text(chkerr);
        if (data.FuelUnits != "") {
            if (data.FuelUnits == "Liters")
            {
                $(document).find(".spnFuelUnit label").text(getResourceValue("LitersAlert")); 
                $(document).find("#FleetFuelModel_FuelUnit").val(getResourceValue("LitersAlert"));
            }
            else
            {
                $(document).find(".spnFuelUnit label").text(data.FuelUnits);  
                $(document).find("#FleetFuelModel_FuelUnit").val(data.FuelUnits);
            }           
            
        }
        else {
            $(document).find(".spnFuelUnit label").text(getResourceValue("UnitAlert"));
            $(document).find("#FleetFuelModel_FuelUnit").val(getResourceValue("UnitAlert"));
          
        }
    }
    else {
        $(document).find("#FleetFuelModel_MtrCurrentReadingDate").prop("disabled", true);
        $(document).find("#FleetFuelModel_StartTimeValue").prop("disabled", true);
        $(document).find("#FleetFuelModel_Void").prop("disabled", true);
        $(document).find("#FleetFuelModel_UnitCost").prop("disabled", true);
        $(document).find("#FleetFuelModel_FuelType").prop("disabled", true);
        $(document).find("#btnFleetFuelRecord").prop("disabled", true);
        $(document).find("#FleetFuelModel_FuelAmount").prop("disabled", true);
        $(document).find("#FleetFuelModel_Reading").prop("disabled", true);
        $("#errmsg").css("display", "block");
    }

    CloseLoader();  

});

function GetDateDiff(fromdate) {
    var Fdate = new Date(parseInt(fromdate.slice(6, -2)));
    var today = new Date();
    var diffMs = (today - Fdate); // milliseconds between now & Christmas
    var diffDays = Math.floor(diffMs / 86400000); // days
    var diffHrs = Math.floor((diffMs % 86400000) / 3600000); // hours
    var diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); // minutes    
    if (parseInt(diffDays) > 0) {
        return diffDays + "  " + getResourceValue("DaysAgoAlert");
    }
    else {
        diffDays = 0;
        return diffDays + "  " + getResourceValue("DaysAgoAlert");
    }
    //else if (parseInt(diffHrs) > 0) {
    //    return diffHrs + "  " + getResourceValue("HoursAgoAlert");
    //}
    //else {
    //    return diffMins + "  " + getResourceValue("MinutesAgoAlert");
    //}   
}

function GetCurrentDate() {
    var d = new Date();

    var month = d.getMonth() + 1;
    var day = d.getDate();

    var output = (('' + month).length < 2 ? '0' : '') + month + '/' +
        (('' + day).length < 2 ? '0' : '') + day + '/' + d.getFullYear();

    return output;

}