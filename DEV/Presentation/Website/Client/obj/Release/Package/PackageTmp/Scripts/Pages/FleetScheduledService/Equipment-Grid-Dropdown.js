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
    //var EquipmentId = $('#partsSessionData_EquipmentId').val();
    var EquipmentId = $('#partsSessionData_EquipmentId').val();
    ScheduledServiceModel_ClientLookupId
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
                    //"mRender": function (data, type, row) {
                    //    return '<a class=link_fueleqp_detail href="javascript:void(0)">' + data + '</a>';
                    //}
                    "mRender": function (data, type, full) {
                        var Meter1Type = "Odometer&nbsp;";
                        var Inerval = "Interval";
                        var M1Type = Meter1Type + Inerval;
                        if (full.Meter1Type) {
                            M1Type = full.Meter1Type + "&nbsp;Interval:";
                        }

                        var Meter2Type = "Hour";
                        var M2Type = Meter2Type + Inerval;
                        if (full.Meter2Type) {
                            M2Type = full.Meter2Type + "&nbsp;Interval:";
                        }

                        var Meter1Units = "Kilometers";
                        if (full.Meter1Units) {
                            Meter1Units = full.Meter1Units;
                        }

                        var Meter2Units = "Hours";
                        if (full.Meter2Units) {
                            Meter2Units = full.Meter2Units;
                        }

                        return '<a class=link_fueleqp_detail href="javascript:void(0)" data-meter1type=' + M1Type + ' data-meter2type=' + M2Type + ' data-meter1units=' + Meter1Units + ' data-meter2units=' + Meter2Units + '>' + data + '</a>';
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
            if (!$(document).find('#ScheduleServiceModal').hasClass('show')) {
                $(document).find('#ScheduleServiceModal').modal("show");
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
    $(".hiddenInitial").hide();
    $(".hiddenInitialMeter2").hide();
    var index_row = $('#EquipPopupTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = equipTable.row(row).data();
    var Meter1Type = "Odometer Interval";
    var Meter2Type = "Hour Interval";
    var Meter1Unit = "Miles";
    var Meter2Unit = "Hours";
    var Meter1Threshold = "Odometer Threshold";
    var Meter2Threshold = "Hour Threshold";
    if (data.Meter1Type) {
        Meter1Type = data.Meter1Type + ' Interval:';
        Meter1Threshold = data.Meter1Type + ' Threshold:';
        $(".hiddenInitialMeter1").show();
        $("#spnMeter1Interval").text(Meter1Type);
        $("#spnMeter1Threshold").text(Meter1Threshold);
    }
    if (data.Meter2Type) {
        Meter2Type = data.Meter2Type + ' Interval:';
        Meter2Threshold = data.Meter2Type + ' Threshold:';
        $(".hiddenInitialMeter2").show();        
        $("#spnMeter2Interval").text(Meter2Type);
        $("#spnMeter2Threshold").text(Meter2Threshold);
    }
    if (data.Meter1Unit) {
        Meter1Unit = data.Meter1Unit;
        $("#spnMeter1IntervalType").text(Meter1Unit);
        $("#spnMeter1ThresholdType").text(Meter1Unit);
    }
    if (data.Meter2Unit) {
        Meter2Unit = data.Meter2Unit;
        $("#spnMeter2IntervalType").text(Meter2Unit);
        $("#spnMeter2ThresholdType").text(Meter2Unit);
    }

    $(".hiddenInitialDueSoonSettings").show();
    $(".hiddenRepairReason").show();
    $(".hiddenSystem").show();
    $(".hiddenAssembly").show();
    $(".hiddenInitialServiceTask").show();
    $(".hiddenInitialTimeInterval").show();
    $(".hiddenInitialbtnSave").show();
    $(".hiddenInitialTimeThreshold").show();
    $(document).find('#ScheduledServiceModel_ClientLookupId').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find("#ScheduleServiceModal").modal('hide');
    $(document).find('.resetText').val("0");
    $(document).find(".Chckerror").css("display", "none");
    $(document).find('.errormessage').css("display", "none");

    CloseLoader();
});


