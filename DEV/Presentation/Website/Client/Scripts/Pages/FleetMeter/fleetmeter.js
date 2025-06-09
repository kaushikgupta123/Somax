var btnActiveStatus = false;
var selectCount = 0;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var StartReadingDate = '';
var EndReadingDate = '';
var today = $.datepicker.formatDate('mm/dd/yy', new Date());

function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}

$.validator.setDefaults({ ignore: null });
$(document).ready(function () {
    $('#sampleDatepicker').datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: "-90:+00"
    });
    ShowbtnLoaderclass("LoaderDrop");
    $("#fleetmeterGridAction :input").attr("disabled", "disabled");
   // ShowbtnLoader("btnsortmenu");
    $(document).find('.select2picker').select2({});

    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    GenerateFleetMeterGrid();
    $("#btnFleetMeterAdvSrch").on('click', function (e) {
        run = true;
        $(document).find('#txtColumnSearch').val('');
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
        FleetMeterAdvSearch();
        fleetMtrTable.page('first').draw('page');
    });
});

//#region Dropdown toggle   
$(document).mouseup(function (e) {
    var container = $(document).find('#searchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});
//#endregion

//#region Search
var run = false;
var fleetMtrTable;
var currestsortedcolumn = 0;
var currestsortedorder = 'asc';
var dtmeter = '<div class="symbol"><img src="#val0" /></div><div class="dtlsBox"><h2>#val1</h2><p>#val2</p></div>';
var colstr = '<div class="dtlsBox"><h2>#val1</h2><p class="#class-2">#val2</p></div>';
var actionvoid = '<button type="button" class="btn btn-danger btn-cart" title="Void"><i class="fa fa-toggle-on"></i></button>';
var actionunvoid = '<button type="button" class="btn btn-danger btn-cart" title="UnVoid"><i class="fa fa-toggle-off"></i></button>';
var GridName = "FleetMeter_Search";

$(document).on('click', '#liPdf', function () {
    var params = {
        colname: currestsortedcolumn,
        coldir: currestsortedorder,
        ClientLookupId: LRTrim($("#EquipmentID").val()),
        Name: LRTrim($("#Name").val()),
        Make: LRTrim($("#Make").val()),
        Model: LRTrim($("#ModelNumber").val()),
        VIN: LRTrim($("#VIN").val()),
        StartReadingDate: ValidateDate(StartReadingDate),
        EndReadingDate: ValidateDate(EndReadingDate),
        SearchText: LRTrim($(document).find('#txtColumnSearch').val())
    };
    fleetMeterPrintParams = JSON.stringify({ 'fleetMeterPrintParams': params });
    $.ajax({
        "url": "/FleetMeter/SetPrintData",
        "data": fleetMeterPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            window.open('/FleetMeter/PrintASPDF', '_self');
            return;
        }
    });
    funcCloseExportbtn();
});

$(document).on('click', '#liPrint', function () {
    var params = {
        colname: currestsortedcolumn,
        coldir: currestsortedorder,
        ClientLookupId: LRTrim($("#EquipmentID").val()),
        Name: LRTrim($("#Name").val()),
        Make: LRTrim($("#Make").val()),
        Model: LRTrim($("#ModelNumber").val()),
        VIN: LRTrim($("#VIN").val()),
        StartReadingDate: ValidateDate(StartReadingDate),
        EndReadingDate: ValidateDate(EndReadingDate),
        SearchText: LRTrim($(document).find('#txtColumnSearch').val())
    };
    fleetMeterPrintParams = JSON.stringify({ 'fleetMeterPrintParams': params });
    $.ajax({
        "url": "/FleetMeter/SetPrintData",
        "data": fleetMeterPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            window.open('/FleetMeter/ExportASPDF', '_blank');
            return;
        }
    });
    funcCloseExportbtn();
});

function GenerateFleetMeterGrid() {
    var printCounter = 0;
    if ($(document).find('#tblfleetmeter').hasClass('dataTable')) {
        fleetMtrTable.destroy();
    }
    fleetMtrTable = $("#tblfleetmeter").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                //Search Retention
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": "/Base/CreateUpdateState",// gridStateSaveUrl,
                    "data": {
                        GridName: GridName,
                        LayOutInfo: JSON.stringify(data),
                        FilterInfo: JSON.stringify(filterinfoarray)
                    },
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
            run = false;
            //Search Retention
        },
        "stateLoadCallback": function (settings, callback) {
            //Search Retention
            $.ajax({
                "url": "/Base/GetLayout",//gridStateLoadUrl,
                "data": {
                    GridName: GridName
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json.LayoutInfo) {
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#spnControlCounter"), $("#dvFilterSearchSelect2"));
                        }
                    }
                    else {
                        callback(json);
                    }
                }
            });
            //Search Retention
        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 2
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'print',
                title: 'Fleet Meter List'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Fleet Meter List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/FleetMeter/FleetMeterGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.ClientLookupId = LRTrim($("#EquipmentID").val());
                d.Name = LRTrim($("#Name").val());
                d.Make = LRTrim($("#Make").val());
                d.Model = LRTrim($("#ModelNumber").val());
                d.VIN = LRTrim($("#VIN").val());
                d.StartReadingDate = ValidateDate(StartReadingDate);
                d.EndReadingDate = ValidateDate(EndReadingDate);
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
            },
            "dataSrc": function (result) {
               // HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                searchcount = result.recordsTotal;
                if (result.data.length < 1) {
                    $(document).find('#btnMeterHistoryExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnMeterHistoryExport').prop('disabled', false);
                }
                if (totalcount < result.recordsTotal)
                    totalcount = result.recordsTotal;
                if (totalcount != result.recordsTotal)
                    selectedcount = result.recordsTotal;

                //HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            },
            global: true
        },
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        "columns":
        [
            {
                "data": "ClientLookupId",
                "autoWidth": false,
                "bSearchable": true,
                "bSortable": true,
                "name": "0",
                "mRender": function (data, type, full) {
                    return dtmeter.replace('#val0', full.EquipmentImage).replace('#val1', full.ClientLookupId).replace('#val2', full.Name);
                }
            },
            {
                "data": "ReadingDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                mRender: function (data, type, full, meta) {
                    return colstr.replace('#val1', full.ReadingDate).replace('#val2', full.NoOfDays + "  " + getResourceValue("DaysAgoAlert"));
                }
            },
            {
                "data": "ReadingLine1", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", className: 'text-right',
                mRender: function (data, type, full, meta) {
                    if (full.Meter2Indicator == true) {
                        if (full.ReadingLine2 == "CR") {
                            return colstr.replace('#val1', full.ReadingLine1 + "  " + '<span class="fmbadgelabel">' + getResourceValue("SecondaryMeterAlert") + '</span>').replace('#class-2', 'm-text-green').replace('#val2', getResourceValue("CurrentReadingAlert"));
                        }
                        else {
                            return colstr.replace('#val1', full.ReadingLine1 + "  " + '<span class="fmbadgelabel">' + getResourceValue("SecondaryMeterAlert") + '</span>').replace('#class-2', 'm-text-grey').replace('#val2', full.ReadingLine2);
                        }

                    }
                    else {
                        if (full.ReadingLine2 == "CR") {
                            return colstr.replace('#val1', full.ReadingLine1).replace('#class-2', 'm-text-green').replace('#val2', getResourceValue("CurrentReadingAlert"));
                        }
                        else { return colstr.replace('#val1', full.ReadingLine1).replace('#class-2', 'm-text-grey').replace('#val2', full.ReadingLine2); }
                    }
                }
            },
            {
                "data": "SourceType", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3", className: 'text-center'
            },
            { "data": "FleetMeterReadingId", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "4", className: 'text-center' }
        ],
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    if (data.Void == false) {
                        return '<button type="button" class="btn btn-outline-primary gridinnerbutton btnvoidfm" title="Void" data-fmrid="' + data.FleetMeterReadingId + '"><i class="fa fa-toggle-on"></i></button>'
                    }
                    else {
                        return '<button type="button" class="btn btn-outline-primary gridinnerbutton btnunvoidfm" title="UnVoid" data-fmrid="' + data.FleetMeterReadingId + '"><i class="fa fa-toggle-off"></i></button>'
                    }
                }
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();         
            $("#fleetmeterGridAction :input").removeAttr("disabled");
            $("#fleetmeterGridAction :button").removeClass("disabled");
            DisableExportButton($("#tblfleetmeter"), $(document).find('.import-export'));
        }
    });
}
$(document).on('click', '#tblfleetmeter_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#tblfleetmeter_length .searchdt-menu', function () {
    run = true;
});
//#endregion
//#region AdvancedSearch
function FleetMeterAdvSearch() {
    var InactiveFlag = false;
    $('#txtColumnSearch').val('');
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#dvFilterSearchSelect2").html(searchitemhtml);
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $('#txtColumnSearch').val('');
    clearAdvanceSearch();
    fleetMtrTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    $('#advsearchsidebar').find('input:text').val('');
    $('#advsearchsidebar').find("select").val("").trigger('change.select2');
    $('.adv-item').val("");
    selectCount = 0;
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
    newEle = "";
    $('#dvFilterSearchSelect2').find('span').html('');
    $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');
    StartReadingDate = '';
    EndReadingDate = '';
}
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    FleetMeterAdvSearch();
    fleetMtrTable.page('first').draw('page');
});
$(document).on('click', '#sidebarCollapse', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('change', '#dtgridadvsearchReadingDate', function () {
    var thisval = $(this).val();
    switch (thisval) {
        case '2':
            $('#ReadingDatetimeperiodcontainer').hide();
            StartReadingDate = today;
            EndReadingDate = today;
            break;
        case '3':
            $('#ReadingDatetimeperiodcontainer').hide();
            StartReadingDate = PreviousDateByDay(7);
            EndReadingDate = today;
            break;
        case '4':
            $('#ReadingDatetimeperiodcontainer').hide();
            StartReadingDate = PreviousDateByDay(30);
            EndReadingDate = today;
            break;
        case '5':
            $('#ReadingDatetimeperiodcontainer').hide();
            StartReadingDate = PreviousDateByDay(60);
            EndReadingDate = today;
            break;
        case '6':
            $('#ReadingDatetimeperiodcontainer').hide();
            StartReadingDate = PreviousDateByDay(90);
            EndReadingDate = today;
            break;
        case '10':
            $('#ReadingDatetimeperiodcontainer').show();
            StartReadingDate = today;
            EndReadingDate = today;
            $('#advreadingDatedaterange').val(StartReadingDate + ' - ' + EndReadingDate);
            $(document).find('#advreadingDatedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                StartReadingDate = start.format('MM/DD/YYYY');
                EndReadingDate = end.format('MM/DD/YYYY');
            });
            break;
        default:
            $('#ReadingDatetimeperiodcontainer').hide();
            $(document).find('#advreadingDatedaterange').daterangepicker({
                format: 'MM/DD/YYYY'
            });
            StartReadingDate = '';
            EndReadingDate = '';
            break;
    }
});
//#endregion

//#region New Search button
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'FleetMeterReading' },
        beforeSend: function () {
            ShowbtnLoader("SrchBttnNew");
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)" id= "mem_' + i + '"' + '><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchList.innerHTML = str;
            $(document).find('#searchBttnNewDrop').show("slideToggle");
        },
        complete: function () {
            HidebtnLoader("SrchBttnNew");
        },
        error: function () {
            HidebtnLoader("SrchBttnNew");
        }
    });
});
function GenerateSearchList(txtSearchval, isClear) {
    run = true;
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'FleetMeterReading', searchText: txtSearchval, isClear: isClear },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)"><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchList.innerHTML = str;
        },
        complete: function () {
            if (isClear == false) {
                fleetMtrTable.page('first').draw('page');
                CloseLoader();
            }
            else {
                CloseLoader();
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('keyup', '#txtColumnSearch', function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == 13) {
        TextSearch();
    }
    else {
        event.preventDefault();
    }
});
$(document).on('click', '.txtSearchClick', function () {
    TextSearch();
});
function TextSearch() {
    clearAdvanceSearch();
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#dvFilterSearchSelect2").html(searchitemhtml);
    }
    else {
        run = true;
        GenerateFleetMeterGrid();
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}
$(document).on('click', '#UlSearchList li', function () {
    var v = LRTrim($(this).text());
    $(document).find('#txtColumnSearch').val(v);
    TextSearch();
});
$(document).on('click', '#cancelText', function () {
    $(document).find('#txtColumnSearch').val('');
});
$(document).on('click', '#clearText', function () {
    GenerateSearchList('', true);
});
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../FleetMeter/Index?page=Fleet_Meter";
});
//#endregion

function SetFleetFuelControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
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
    SetFixedHeadStyle();
}
function FleetMeterAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("MeterReadingAddAlert");
        swal(SuccessAlertSetting, function () {
            $("#readingMeterModal").modal('hide');
            titleText = getResourceValue("AlertActive");
            window.location.href = '/FleetMeter/Index?page=Fleet_Meter';
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        if ($(document).find("#fleetMeterModel_Meter1Void").prop("checked") == false && parseFloat($(document).find("#FM1CurrentReading").val()) >= parseFloat($(document).find("#fleetMeterModel_Meter1CurrentReading").val())) {
            $(document).find('#spnMeter1dayDiff').show();
            $(document).find("#fleetMeterModel_Meter1CurrentReading").addClass("input-validation-error");
        }
        else {
            $(document).find('#spnMeter1dayDiff').hide();
        }
        if ($(document).find("#fleetMeterModel_Meter2Void").prop("checked") == false && parseFloat($(document).find("#FM2CurrentReading").val()) >= parseFloat($(document).find("#fleetMeterModel_Meter2CurrentReading").val())) {
            $(document).find('#spnMeter2dayDiff').show();
            $(document).find("#fleetMeterModel_Meter2CurrentReading").addClass("input-validation-error");
        }
        else {
            $(document).find('#spnMeter2dayDiff').hide();
        }


    }
}

$(document).on('click', '#AddNewMeterReading', function () {
    ResetErrorDiv($(document).find('#frmrecordmeterreading'));
    $('#readingMeterModal').find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy').datepicker("setDate", new Date()).removeClass('input-validation-error');
    $(document).find('#Readings_Reading').val('0').removeClass('input-validation-error');
    $.validator.unobtrusive.parse(document);
    $('#readingMeterModal').modal('show');
    $(document).find(".dtpickerNew").datepicker({
        showOn: 'button',
        buttonImageOnly: true,
        maxDate: new Date(),
        buttonImage: '/Images/calender.png'
    });
    $('.dtpickerNew').datepicker('setDate', new Date());
    var timerVal = moment().format('hh:mm A');
    $(document).find('.timerId').timepicker(
        {
            template: 'dropdown',
            minuteStep: 1,
            showMeridian: true,
            defaultTime: timerVal
        });
    $(document).find('#fleetMeterModel_Meter1CurrentReading').val('0').trigger('change');
    $(document).find('#fleetMeterModel_Meter2CurrentReading').val('0').trigger('change');
    $(document).find('#fleetMeterModel_Meter1Void').prop('checked', false);
    $(document).find('#fleetMeterModel_Meter2Void').prop('checked', false);
    $(document).find('#spnMeter1dayDiff').hide();
    $(document).find('#spnMeter2dayDiff').hide();
});

function getCurrentTime() {
    var timerVal = moment().format('hh:mm A');
    $(document).find('.timerId').timepicker(
        {
            template: 'dropdown',
            minuteStep: 1,
            showMeridian: true,
            defaultTime: timerVal
        });
}

$(document).on('click', "#dismiss, .overlay", function () {
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
});

//#endregion

//#region VoidUnVoid
$(document).on('click', ".btnvoidfm", function () {
    var fmReadingId = $(this).data("fmrid");
    $(document).find('.fleeterrormessage').css('display', 'none');
    $.ajax({
        url: "/FleetMeter/VoidFleetMeter",
        type: "POST",
        dataType: 'JSON',
        data: { fleetMeterReadingId: fmReadingId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var message = "";
            if (data == "success") {
                SuccessAlertSetting.text = getResourceValue("FleetMeterVoidAlert");
                swal(SuccessAlertSetting, function () {
                    if (data == "success") {
                        fleetMtrTable.destroy();
                        GenerateFleetMeterGrid();
                    }
                });
            }
            else {
                message = getResourceValue("FleetMeterVoidFailedAlert");
                GenericSweetAlertMethod(message);
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
$(document).on('click', ".btnunvoidfm", function () {
    var fmReadingId = $(this).data("fmrid");
    $(document).find('.fleeterrormessage').css('display', 'none');
    $.ajax({
        url: "/FleetMeter/UnvoidFleetMeter",
        type: "POST",
        dataType: 'JSON',
        data: { fleetMeterReadingId: fmReadingId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var message = "";
            if (data == "success") {
                SuccessAlertSetting.text = getResourceValue("FleetMeterUnVoidAlert");
                swal(SuccessAlertSetting, function () {
                    if (data == "success") {
                        fleetMtrTable.destroy();
                        GenerateFleetMeterGrid();
                    }
                });
            }
            else {
                ShowFleetUnvoidError(data);
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
//#endregion

//#region //Search Retention
function getfilterinfoarray(txtsearchelement, advsearchcontainer) {
    var filterinfoarray = [];
    var f = new filterinfo('searchstring', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parent('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);
        }
        if ($(this).parent('div').find('div').hasClass('range-timeperiod')) {
            if ($(this).parent('div').find('input').val() !== '' && $(this).val() == '10') {
                f = new filterinfo('this-' + $(this).attr('id'), $(this).parent('div').find('input').val());
                filterinfoarray.push(f);
            }
        }
    });
    return filterinfoarray;
}
function setsearchui(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if (item.key == 'searchstring' && item.value) {
            var txtSearchval = item.value;
            if (item.value) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parent('div').is(":visible")) {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
                if ($('#' + item.key).hasClass('has-dtrangepicker') && item.value !== '') {
                    $('#' + item.key).val(item.value).trigger('change');
                    var datarangeval = data.filter(function (val) { return val.key === 'this-' + item.key; });
                    if (datarangeval.length > 0) {
                        if (datarangeval[0].value) {
                            var rangeid = $('#' + item.key).parent('div').find('input').attr('id');
                            $('#' + rangeid).css('display', 'block');
                            $('#' + rangeid).val(datarangeval[0].value);
                            if (item.key === 'dtgridadvsearchReadingDate') {
                                StartReadingDate = datarangeval[0].value.split(' - ')[0];
                                EndReadingDate = datarangeval[0].value.split(' - ')[1];
                                $(document).find('#advreadingDatedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
                                    StartReadingDate = start.format('MM/DD/YYYY');
                                    EndReadingDate = end.format('MM/DD/YYYY');
                                });
                            }
                        }
                    }
                }
                else {
                    $('#' + item.key).val(item.value);
                }
            }
            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}
//#endregion

$(document).on('click', '#btnFleetMetercancel', function () {
    swal(CancelAlertSetting, function () {
        $("#readingMeterModal").modal('hide');
    });
});
function ShowFleetUnvoidError(errorObject, thisformId) {
    var errorMessageContainer;
    var errorString = "";
    errorMessageContainer = $(document).find('.fleeterrormessage');
    if (typeof errorObject !== "string") {
        $.each(errorObject, function (index, item) {
            errorString = errorString + '<div class="m-alert m-alert--icon m-alert--icon-solid m-alert--outline alert alert-danger alert-dismissible fade show" role="alert"><div class="m-alert__icon"><i class="flaticon-danger"></i></div>' +
                '<div class="m-alert__text">' + item + '</div><div class="m-alert__close">' +
                '<button type="button" class="close" data-dismiss="alert" aria-label="Close"></button></div></div>';
        });
    }
    else {
        errorString = errorString + '<div class="m-alert m-alert--icon m-alert--icon-solid m-alert--outline alert alert-danger alert-dismissible fade show" role="alert"><div class="m-alert__icon"><i class="flaticon-danger"></i></div>' +
            '<div class="m-alert__text">' + errorObject + '</div><div class="m-alert__close">' +
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close"></button></div></div>';
    }
    errorMessageContainer.html(errorString).show();
    window.scrollTo(0, 0);
}

$(document).on('click', "#btnFleetMetercancel", function () {
    swal(CancelAlertSetting, function () {
        $("#readingMeterModal").modal('hide');
        $(document).find('.errormessage').css("display", "none");

        ClearAllData();
    });
});

function ClearAllData() {

    $(document).find('form').trigger("reset");
    $(document).find("#fleetMeterModel_Meter1CurrentReading").prop("disabled", true);
    $(document).find("#fleetMeterModel_Meter1Void").prop("disabled", true);
    $(document).find("#fleetMeterModel_CurrentReadingDate").prop("disabled", true);
    $(document).find("#fleetMeterModel_CurrentReadingTime").prop("disabled", true);
    $(document).find("#btnAddMeterRecord").prop("disabled", true);
    $(document).find("#fleetMeterModel_Meter2CurrentReading").prop("disabled", true);
    $(document).find("#fleetMeterModel_Meter2Void").prop("disabled", true);
    $(document).find('#liOdometer').show();
    $(document).find('#liOdometerVoid').show();
    $(document).find('#liHour').show();
    $(document).find('#liHourVoid').show();
    var timerVal = moment().format('hh:mm A');
    $('.timerId').val(timerVal);
    $("#errmsg").css("display", "none");
}

$(document).on('click', '.ResetallData', function () {
    ClearAllData();
});

$(document).on('click', '.ResetAllContent', function () {
    ClearAllData();
});

$(function () {
    $('.restkey').bind('keyup keydown keypress', function (evt) {
        return false;
    });
});