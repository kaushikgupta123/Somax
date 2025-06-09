var dvStatus;
var devicedt;
var run = false;

var deviceIdVal;
var deviceNameVal;
var assetIdVal;
var assetCategoryVal;
var sensorTypeVal;

var deviceAssetNameVal;
var deviceLastReadingVal;
var filterinfoarray = [];
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
$(function () {
    if ($(document).find('#DeviceDetails').length == 1) {
        localStorage.setItem("devicestatustext", "Devices");       
        $(document).find('#spnlinkToSearch').text(localStorage.getItem("devicestatustext"));
    }
    //Required for Fusion Chart.
    SetFunsionChartGlobalSettings();

    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', "#sidebarCollapse", function () {
        $('#renderdevices').find('.sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
        $(document).find('.select2picker').select2({});
    });
    var devicestatus = localStorage.getItem("devicestatus");
    if (devicestatus) {
        dvStatus = devicestatus;
        $('#devicesearchListul li').each(function (index, value) {
            if ($(this).attr('id') == dvStatus && $(this).attr('id') != '0') {
                $('#devicesearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    var titletext = $('#devicesearchtitle').text();
    if (titletext == "") { titletext = "Devices" }

    localStorage.setItem("devicestatustext", titletext);    
    generateDeviceTable();
    $(document).on('click', '#liPdf', function () {
        $(".buttons-pdf")[0].click();
        funcCloseExportbtn();
    });
    $(document).on('click', '#liCsv', function () {
        $(".buttons-csv")[0].click();
        funcCloseExportbtn();
    });
    $(document).on('click', "#liExcel", function () {
        $(".buttons-excel")[0].click();
        funcCloseExportbtn();
    });
    $(document).on('click', '#liPrint', function () {
        $(".buttons-print")[0].click();
        funcCloseExportbtn();
    });
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
});
//#region Search
$(document).on('keyup', '#devicesearctxtbox', function (e) {
    var tagElems = $(document).find('#devicesearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.dvsearchdrpbox', function (e) {
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#advsearchfilteritems").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    if ($(this).attr('id') != '0') {
        $('#devicesearchtitle').text($(this).text());
    }
    else {
        $('#devicesearchtitle').text("Devices");
    }
    var titletext = $('#devicesearchtitle').text();
    localStorage.setItem("devicestatustext", titletext);
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("devicestatus", optionval);
    dvStatus = optionval;
    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        devicedt.page('first').draw('page');
    }
});

$(document).on('click', "#SrchBttnDevice", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'Device' },
        beforeSend: function () {
            ShowbtnLoader("SrchBttnDevice");
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
            HidebtnLoader("SrchBttnDevice");
        },
        error: function () {
            HidebtnLoader("SrchBttnDevice");
        }
    });
});
$(document).on('click', '#cancelText', function () {
    $(document).find('#txtColumnSearch').val('');
});
$(document).on('click', '#clearText', function () {
    GenerateSearchList('', true);
});
function GenerateSearchList(txtSearchval, isClear) {
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'Device', searchText: txtSearchval, isClear: isClear },
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
            CloseLoader();
            if (isClear == false) {
                devicedt.page('first').draw('page');
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
    run = true;
    hGridclearAdvanceSearch();    
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossDevice" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        devicedt.page('first').draw('page');
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
//#region 
function getfilterinfoarray(txtsearchelement, advsearchcontainer) {
    var filterinfoarray = [];
    var f = new filterinfo('searchstring', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parent('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);
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
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossDevice" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parent('div').is(":visible")) {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    hGridfilteritemcount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossDevice" aria-hidden="true"></a></span>';
                }
            }
            advcountercontainer.text(hGridfilteritemcount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);

}
//#endregion
var order = '0';
var orderDir = 'asc';
function generateDeviceTable() {
    var printCounter = 0;
    if ($(document).find('#tblDevices').hasClass('dataTable')) {
        devicedt.destroy();
    }
    devicedt = $("#tblDevices").DataTable({
        colReorder: {
            fixedColumnsLeft: 1
        },
        /*  rowGrouping: true,*/
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebarDevice'));
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: "Device_Search",
                        LayOutInfo: JSON.stringify(data),
                        FilterInfo: JSON.stringify(filterinfoarray)
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
                "url": "/Base/GetLayout",
                "data": {
                    GridName: "Device_Search"
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    hGridfilteritemcount = 0;
                    if (json.LayoutInfo !== '') {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo !== '') {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#filteritemcount"), $("#advsearchfilteritems"));
                        }
                    }
                    else {
                        callback(json.LayoutInfo);
                    }

                }
            });
        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 1
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Device List'
            },
            {
                extend: 'print',
                title: 'Device List'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Device List',
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
                title: 'Device List',
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Devices/GetDeviceMainGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.customQueryDisplayId = dvStatus;
                d.deviceClientLookupId = LRTrim($("#txtDeviceId").val());
                d.name = LRTrim($("#txtName").val());
                d.assetClentLookupId = LRTrim($("#txtAssetId").val());
                d.assetCategory = $("#advancedSearchAssetCategory option:selected").val();
                d.sensorType = $("#advancedSearchSensorType option:selected").val();
                d.txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
            },
            "dataSrc": function (result) {
                let colOrder = devicedt.order();
                orderDir = colOrder[0][1];
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
                    "data": "ClientLookupID",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_device href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                },
                { "data": "SensorType", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "AssetID", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                { "data": "AssetName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
                { "data": "LastReading", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5", "className": "text-right" },
                { "data": "LastReadingDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6", "className": "text-right" },
                { "data": "InactiveFlag", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7", "className": "text-right" }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            var currestsortedcolumn = $('#tblDevices').dataTable().fnSettings().aaSorting[0][0];
            var column = this.api().column(currestsortedcolumn);
            var columnId = $(column.header()).attr('id');           
            DisableExportButton($("#tblDevices"), $(document).find('.import-export'));
        }
    });
}
$(document).on('click', '#tblDevices_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#tblDevices_length .searchdt-menu', function () {
    run = true;
});
$('#tblDevices').find('th').click(function () {
    if ($(this).data('col') !== undefined && $(this).data('col') !== '') {
        run = true;
        order = $(this).data('col');
    }
});

$(document).on('click', "#btnDataAdvSrchdevice", function (e) {
    run = true;
    $(document).find('#txtColumnSearch').val('');
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#advsearchsidebarDevice').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossDevice" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#renderdevices').find('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    deviceIdVal = LRTrim($("#txtDeviceId").val());
    deviceNameVal = LRTrim($("#txtName").val());
    assetIdVal = LRTrim($("#txtAssetId").val());
    assetCategoryVal = $("#advancedSearchAssetCategory option:selected").val();
    sensorTypeVal = $("#advancedSearchSensorType option:selected").val();
    hGridAdvanceSearch();
    devicedt.page('first').draw('page');
});
function hGridAdvanceSearch() {
    $('.filteritemcount').text(hGridfilteritemcount);
}

function hGridclearAdvanceSearch() {
    $('#advsearchsidebarDevice').find('input:text').val('');
    $('#advsearchsidebarDevice').find("select").val("").trigger('change.select2');
    hGridfilteritemcount = 0;
    $(".filteritemcount").text(hGridfilteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
    deviceIdVal = LRTrim($("#txtDeviceId").val());
    deviceNameVal = LRTrim($("#txtName").val());
    deviceAssetNameVal = LRTrim($("#txtAssetName").val());
    deviceLastReadingVal = LRTrim($("#txtLastReading").val());
    assetIdVal = LRTrim($("#txtAssetId").val());
    assetCategoryVal = $("#advancedSearchAssetCategory").val();
    sensorTypeVal = $("#advancedSearchSensorType").val();
}
$(document).on('click', '.btnCrossDevice', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    run = true;
    if (hGridfilteritemcount > 0) hGridfilteritemcount--;
    if (searchtxtId == "txtDeviceId") {
        deviceIdVal = null;
    }
    if (searchtxtId == "txtName") {
        deviceNameVal = null;
    }
    if (searchtxtId == "txtAssetId") {
        assetIdVal = null;
    }

    if (searchtxtId == "txtAssetName") {
        deviceAssetNameVal = null;
    }
    if (searchtxtId == "txtLastReading") {
        deviceLastReadingVal = null;
    }

    if (searchtxtId == "advancedSearchAssetCategory") {
        assetCategoryVal = null;
    }
    if (searchtxtId == "advancedSearchSensorType") {
        sensorTypeVal = null;
    }
    hGridAdvanceSearch();
    devicedt.page('first').draw('page');
});
$(document).on('click', '.lnk_device', function (e) {
    var deviceId;
    var titletext = $('#devicesearchtitle').text();
    localStorage.setItem("devicestatustext", titletext);
    var row = $(this).parents('tr');
    var data = devicedt.row(row).data();
    deviceId = data.IoTDeviceId;
    var sensorid = data.SensorId;
    var IoTDeviceCategory = data.IoTDeviceCategory;
    var ClientLookupID = data.ClientLookupID;
    $.ajax({
        url: "/Devices/DeviceDetails",
        type: "POST",
        data: { ioTDeviceId: deviceId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderdevices').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);
            if (IoTDeviceCategory == 'MonnitSensor' || IoTDeviceCategory == 'Sensor') {
                GenerateGaugeData();
            }
        },
        complete: function () {
            if (IoTDeviceCategory == 'MonnitSensor' || IoTDeviceCategory == 'Sensor') {
              CreateTimeSeriesChart(deviceId);
            }
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../Devices/Index?page=Monitoring_Device_Search";
});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var customQueryDisplayId = dvStatus;
            var clientLookupID = LRTrim($("#txtDeviceId").val());
            var name = LRTrim($("#txtName").val());
            var ioTDeviceCategory = $("#advancedSearchAssetCategory option:selected").val();
            var sensorType = $("#advancedSearchSensorType option:selected").val();
            var assetID = LRTrim($("#txtAssetId").val());
            var currestsortedcolumn = $('#tblDevices').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#tblDevices').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#tblDevices').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/Devices/DevicePrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    customQueryDisplayId: customQueryDisplayId,
                    deviceClientLookupId: clientLookupID,
                    name: name,
                    ioTDeviceCategory: ioTDeviceCategory,
                    sensorType: sensorType,
                    assetClentLookupId: assetID,
                    assetCategory: ioTDeviceCategory,                    
                    colname: colname,
                    coldir: coldir,
                    txtSearchval: LRTrim($(document).find('#txtColumnSearch').val())
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#tblDevices thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.ClientLookupID != null) {
                    item.ClientLookupID = item.ClientLookupID;
                }
                else {
                    item.ClientLookupId = "";
                }
                if (item.Name != null) {
                    item.Name = item.Name;
                }
                else {
                    item.Name = "";
                }
                if (item.AssetCategory != null) {
                    item.AssetCategory = item.AssetCategory;
                }
                else {
                    item.AssetCategory = "";
                }
                if (item.SensorType != null) {
                    item.SensorType = item.SensorType;
                }
                else {
                    item.SensorType = "";
                }
                if (item.AssetID != null) {
                    item.AssetID = item.AssetID;
                }
                else {
                    item.AssetID = "";
                }
                if (item.AssetName != null) {
                    item.AssetName = item.AssetName;
                }
                else {
                    item.AssetName = "";
                }
                if (item.LastReading != null) {
                    item.LastReading = item.LastReading;
                }
                else {
                    item.LastReading = "";
                }
                if (item.LastReadingDate != null) {
                    item.LastReadingDate = item.LastReadingDate;
                }
                else {
                    item.LastReadingDate = "";
                }
                if (item.InactiveFlag != null) {
                    item.InactiveFlag = item.InactiveFlag;
                }
                else {
                    item.InactiveFlag = false;
                }
                var fData = [];
                $.each(visiblecolumnsIndex, function (index, inneritem) {
                    var key = Object.keys(item)[inneritem];
                    var value = item[key];
                    fData.push(value);
                });
                d.push(fData);
            });
            return {
                body: d,
                header: $("#tblDevices thead tr th").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});
//#endregion

//#region Dropdown toggle
$(document).on('click', "#spnDropToggle", function () {
    $(document).find('#searcharea').show("slide");
});
$(document).mouseup(function (e) {
    var container = $(document).find('#searcharea');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slide");
    }
});
$(document).mouseup(function (e) {
    var container = $(document).find('#searchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});
//#endregion
$(document).on('click', "ul.vtabs li", function () {
    if ($(this).find('#drpDwnLink').length > 0 || $(this).find('#drpDwnLink2').length > 0) {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        return false;
    }
    else {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        $(".tabsArea").hide();
        var activeTab = $(this).find("a").attr("href");
        $(activeTab).fadeIn();
        return false;
    }
});

//#region Time Series Chart
function CreateTimeSeriesChart(sensorid) {
    $.ajax({
        url: '/Devices/GetSensorRedingData',
        type: 'GET',
        data: { SensorId: sensorid },
        beforeSend: function () {
            ShowbtnLoader("SrchBttnDevice");
        },
        success: function (data) {
            if (data && data.arrayLists.length > 0) {
                const plotData = data.arrayLists;
                var schema = [];
                //const schema = JSON.parse('[{"name": "Time","type": "date","format": "%d-%b-%y %-I:%-M:%-S"}, {"name": " Anadi","type": "number"}]');// data1.schemas;
                var schema1 = { "name": "Time", "type": "date", "format": "%d-%b-%y %-I:%-M:%-S" };
                schema.push(schema1);
                var schema2 = { "name": data.plotLevel, "type": "number" };
                schema.push(schema2);
                // First we are creating a DataStore
                const fusionDataStore = new FusionCharts.DataStore();
                // After that we are creating a DataTable by passing our data and schema as arguments
                const fusionTable = fusionDataStore.createDataTable(plotData, schema);

                const chartConfig = {
                    renderAt: "device-time-series",
                    type: "timeseries",
                    width: "100%",
                    height: "400",
                    dataFormat: "json",
                    dataSource: {
                        data: fusionTable,
                        chart: {
                            "theme": "fusion",
                            "toolTipBgColor": "#000000",
                            "toolTipColor": "#FFFFFF",
                            "tooltipborderradius": "4"
                        },
                        yaxis: [],
                        tooltip: {
                            style: {
                                container: {
                                    //"border-radius": "4px",
                                    //"color": "#FFFFFF",
                                    //"background- color": "#000000"
                                }
                            }
                        }
                    }
                };
                new FusionCharts(chartConfig).render();
            }
            else {
                $('#time-seriesNoData').show();
            }

        },
        complete: function () {
            $('#devtimeserieschartloader').hide();
        },
        error: function () {
            $('#devtimeserieschartloader').hide();
        }
    });
}

//#endregion

//#region V2-536
$(document).on('click', ".AddDevice", function (e) {
    e.preventDefault();
    PopulateDeviceCatagory();
});
function PopulateDeviceCatagory() {
    $(document).find('#AddLineItems').modal('hide');
    $.ajax({
        url: "/Devices/PopulateDeviceCatagory",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DeviceCatagoryListPopUp').html(data);
            $('#DeviceCatagoryListModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
$(document).on('click', '.clearstate', function () {
    var areaChargeToId = "";
    $(document).find('#DeviceCatagoryListModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
});
function SelctDeviceCatagoryOnSuccess(data) {
    $('.modal-backdrop').remove();
    if (data.data === "success") {
        var Category = data.Result;
        $(document).find('#DeviceCatagoryListModalpopup').hide();
        ShowLoader();
        AddDevice(Category);
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function AddDevice(Category) {
    $.ajax({
        url: "/Devices/AddDevice",
        type: "POST",
        dataType: "html",
        data: { Category: Category },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderdevices').html(data);
            SetFixedHeadStyle();
        },
        complete: function () {
            SetControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function DeviceAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command == "save") {
            if (data.mode == "add") {
                SuccessAlertSetting.text = getResourceValue("DeviceAddAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("DeviceUpdateAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToDeviceDetail(data.IoTDeviceId, "dvoverview", data.SensorId);
            });
        }
        else {
            SuccessAlertSetting.text = getResourceValue("DeviceAddAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};
$(document).on('click', "#brdDevice,#btnCancelAddDevices", function (e) {
    var IoTDeviceId = $(document).find('#IoTDeviceId').val();
    var SensorId = $(document).find('#SensorId').val();
    CancelAlertSetting.text = getResourceValue("CancelAlertLostMsg");
    swal(CancelAlertSetting, function () {
        if (IoTDeviceId == 0) {
            window.location.href = "../Devices/Index?page=Monitoring_Device_Search";
        }
        else {
            RedirectToDeviceDetail(IoTDeviceId, "dvoverview", SensorId);
        }
    });
});
//#endregion

