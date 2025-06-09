var dtmeter;
var run = false;
var activeStatus;

$(function () {
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    ShowbtnLoaderclass("LoaderDrop");
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', "ul.vtabs li", function () {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        $(".tabsArea").hide();
        var activeTab = $(this).find("a").attr("href");
        $(activeTab).fadeIn();
        return false;
    });
    $(document).on('click', '#sidebarCollapse', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    var displayState = localStorage.getItem("METERSEARCHGRIDDISPLAYSTATUS");
    if (displayState) {
        if (displayState == "false") {
            $('#meterstatusDropdown').val("1").trigger('change.select2');
        }
        else {
            $('#meterstatusDropdown').val("2").trigger('change.select2');
        }
        activeStatus = displayState;
    }
    else {
        activeStatus = false;
    }
    GenerateMeterGrid();
});

$(document).on('change', '#colorselector', function (evt) {
    $(document).find('.tabsArea').hide();
    var a = $(this).val();
    if ($(this).val() === 'MeterContainer') {
        opendiv(evt, 'Identification');
    }
    else {
        openCity(evt, $(this).val());
    }
    $('#' + $(this).val()).show();
});
function opendiv(evt, cityName) {
    $("#MeterContainer").find("#btnidentification").addClass('active');
    document.getElementById(cityName).style.display = "block";
}


//#region Search
function GenerateMeterGrid() {
    if ($(document).find('#meterSearch').hasClass('dataTable')) {
        dtmeter.destroy();
    }
    dtmeter = $("#meterSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 1
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
      //  setNumericRounding(0),
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "Meter_Search",
                        LayOutInfo: JSON.stringify(data)
                    },
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
            run = false;
        },
        "stateLoadCallback": function (settings, callback) {
            $.ajax({
                "url": gridStateLoadUrl,
                "data": {
                    GridName: "Meter_Search"
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json) {
                        callback(JSON.parse(json));
                    }
                    else {
                        callback(json);
                    }
                }
            });
        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 1
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Meter List',
                pageSize: 'A4'
            },
            {
                extend: 'print',
                title: 'Meter List',
                pageSize: 'A4'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Meter List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                orientation: 'landscape',
                pageSize: 'A2',
                title: 'Meter List'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Meter/GetMeterGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.inactiveFlag = activeStatus;
                d.MeterClientLookUpId = LRTrim($("#meterid").val());
                d.Name = LRTrim($("#meterName").val());
                d.ReadingCurrent = LRTrim($("#ReadingCurrent").val());
                d.ReadingDate = ValidateDate($("#ReadingDate").val());
                d.ReadingBy = LRTrim($("#ReadingBy").val());
                d.ReadingLife = LRTrim($("#ReadingLife").val());
                d.MaxReading = $("#MaxReading").val();
                d.ReadingUnits = $("#ReadingUnits").val();
            },
            "dataSrc": function (result) {
                if (result.data.length < 1) {
                    $(document).find('.import-export').prop('disabled', true);
                }
                else {
                    $(document).find('.import-export').prop('disabled', false);
                }
                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "MeterClientLookUpId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_meter href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "MeterName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                },
                 {
                     "data": "StringReadingCurrent", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", className: "text-right"
                     
                },              
                {
                    "data": "ReadingDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3", "type": "date",
                    render: function (data, type, row, meta) {
                        if (data == null) {
                            return '';
                        } else {
                            return data;
                        }
                    }
                },
                { "data": "PersonnelClientLookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                { "data": "StringReadingLife", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5", className: "text-right" },
                { "data": "StringMaxReading", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6", className: "text-right" },
                { "data": "ReadingUnits", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7" }
            ],
        columnDefs: [
            {
                
                //"mRender": function (data, type, row) {
                //    var tempNumber = parseFloat(data).toFixed(3);
                //    if (isNaN(tempNumber)) {
                //        return "";
                //    } else {
                //        return tempNumber;
                //    }
                //},
                //targets: [2,5,6]
            },
            {

                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#meterGridAction :input").removeAttr("disabled");
            $("#meterGridAction :button").removeClass("disabled");
            DisableExportButton($("#meterSearch"), $(document).find('.import-export'));
        }
    });
}
$(document).on('click', '#meterSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#meterSearch_length .searchdt-menu', function () {
    run = true;
});
$(document).on('change', '#meterstatusDropdown', function () {
    run = true;
    ShowbtnLoaderclass("LoaderDrop");
    var thisVal = $(this).val();
    if (thisVal == 1) {
        localStorage.setItem("METERSEARCHGRIDDISPLAYSTATUS", false);
        activeStatus = false;
    }
    else {
        localStorage.setItem("METERSEARCHGRIDDISPLAYSTATUS", true);
        activeStatus = true;
    }
    dtmeter.page('first').draw('page');

});
$("#btnMeterAdvSrch").on('click', function (e) {
    MeterAdvSearch();
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    dtmeter.page('first').draw('page');
});
function MeterAdvSearch() {
    var InactiveFlag = false;
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossmeter" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    var optionVal = $(document).find("#meterstatusDropdown").val();
    if (optionVal == "1") {
        InactiveFlag = true;
    }
    $("#dvFilterSearchSelect2").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
}
function clearAdvanceSearch() {
    $('#advsearchsidebar').find('input:text').val('');
    selectCount = 0;
    $(".filteritemcount").text(selectCount);
    $('#dvFilterSearchSelect2').find('span').html('');
    $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');
}
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    clearAdvanceSearch();
    dtmeter.page('first').draw('page');
});
$(document).on('click', '.btnCrossmeter', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('');
    $(this).parent().remove();
    selectCount--;
    MeterAdvSearch();
    dtmeter.page('first').draw('page');
});
$(document).on('click', '.lnk_meter', function (e) {
    e.preventDefault();
    var index_row = $('#meterSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtmeter.row(row).data();
    var MeterId = data.MeterId;
    $.ajax({
        url: "/Meter/MeterDetail",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { MeterId: MeterId },
        success: function (data) {
            $('#metercontainer').html(data);
        },
        complete: function () {
            CloseLoader();
        }
    });
});
//#endregion

//#region print
$(document).on('click', '#liPrint', function () {
    $(".buttons-print")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liPdf', function () {
    $(".buttons-pdf")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    funcCloseExportbtn();
});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            dtTable = $("#meterSearch").DataTable();
            var currestsortedcolumn = $('#meterSearch').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#meterSearch').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#meterSearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/Meter/GetMeterPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    inactiveFlag: activeStatus,
                    MeterClientLookUpId: LRTrim($("#meterid").val()),
                    Name: LRTrim($("#meterName").val()),
                    ReadingCurrent: LRTrim($("#ReadingCurrent").val()),
                    ReadingDate: ValidateDate($("#ReadingDate").val()),
                    ReadingBy: LRTrim($("#ReadingBy").val()),
                    ReadingLife: LRTrim($("#ReadingLife").val()),
                    MaxReading: $("#MaxReading").val(),
                    ReadingUnits: $("#ReadingUnits").val(),
                    colname: colname,
                    coldir: coldir
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#meterSearch thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.MeterClientLookUpId != null) {
                    item.MeterClientLookUpId = item.MeterClientLookUpId;
                }
                else {
                    item.MeterClientLookUpId = "";
                }
                if (item.MeterName != null) {
                    item.MeterName = item.MeterName;
                }
                else {
                    item.MeterName = "";
                }
                if (item.ReadingCurrent != null) {
                    item.ReadingCurrent = item.ReadingCurrent;
                }
                else {
                    item.ReadingCurrent = "";
                }
                if (item.ReadingDate != null) {
                    item.ReadingDate = item.ReadingDate;
                }
                else {
                    item.ReadingDate = "";
                }
                if (item.PersonnelClientLookUpId != null) {
                    item.PersonnelClientLookUpId = item.PersonnelClientLookUpId;
                }
                else {
                    item.PersonnelClientLookUpId = "";
                }
                if (item.ReadingLife != null) {
                    item.ReadingLife = item.ReadingLife;
                }
                else {
                    item.ReadingLife = "";
                }
                if (item.MaxReading != null) {
                    item.MaxReading = item.MaxReading;
                }
                else {
                    item.MaxReading = "";
                }
                if (item.ReadingUnits != null) {
                    item.ReadingUnits = item.ReadingUnits;
                }
                else {
                    item.ReadingUnits = "";
                }
                var fData = [];
                $.each(visiblecolumnsIndex, function (index, inneritem) {
                    var key = Object.keys(item)[inneritem];
                    var value = item[key]
                    fData.push(value);
                });
                d.push(fData);
            })
            return {
                body: d,
                header: $("#meterSearch thead tr th div").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});
//#endregion

//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtmeter, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(dtmeter, colOrder);
    run = true;
    dtmeter.state.save(run);
});
//#endregion

//#region Add or edit Meter
$(document).on('click', "#AddMeter", function () {
    $.ajax({
        url: "/Meter/AddMeter",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#metercontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            SetMeterControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#editmeter", function (e) {
    e.preventDefault();
    var meterId = $('#Meters_MeterId').val();
    $.ajax({
        url: "/Meter/EditMeter",
        type: "GET",
        dataType: 'html',
        data: { MeterId: meterId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#metercontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            SetMeterControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function MeterAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.Command == "save") {          
            SuccessAlertSetting.text = getResourceValue("MeterAddAlert");
            swal(SuccessAlertSetting, function () {
                RedirectToDetail(data.MeterId, "");
            });
        }
        else {
            SuccessAlertSetting.text = getResourceValue("MeterAddAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function MeterEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.Command == "save") {
            SuccessAlertSetting.text = getResourceValue("MeterUpdateAlert");
            swal(SuccessAlertSetting, function () {
                RedirectToDetail(data.MeterId, "");
            });
        }
       
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#brdmeter", function () {
    var meterid = $(this).attr('data-val');
    RedirectToDetail(meterid);
});
$(document).on('click', "#btneditcancel", function () {
    var meterid = $('#Meters_MeterId').val();
    swal(CancelAlertSetting, function () {
        RedirectToDetail(meterid);
    });
});
//#endregion

//#region Commom
function SetMeterControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $(document).find('.select2picker').select2({});
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if (areaddescribedby) {
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
        }
    });
}
$(document).on('click', '#btncancel', function () {
    swal(CancelAlertSetting, function () {
        RedirectToMeterSearch();
    });
});
function RedirectToMeterSearch() {
    window.location.href = "/Meter/Index";
}
function RedirectToDetail(MeterId, mode) {
    $.ajax({
        url: "/Meter/MeterDetail",
        type: "POST",
        dataType: 'html',
        data: { MeterId: MeterId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#metercontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            if (mode === "Reading") {
                $('#liReadings').trigger('click');
                $('#colorselector').val('ReadingsContainer');
                GenerateReadingsContainerGrid();
            }
           
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#limeter", function () {
    $(document).find('#btnidentification').addClass('active');
    $(document).find('#Identification').show();
});
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "MIdentification":
      break;
        case "ReadingsContainer":
            $(document).find("#sidebar").mCustomScrollbar({
                theme: "minimal"
            });
            GenerateReadingsContainerGrid();
            break;       
    };
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    document.getElementById(cityName).style.display = "block";
    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}
//#endregion
