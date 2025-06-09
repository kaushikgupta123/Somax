var CustomQueryDisplayId = 2;
var DTioTEventTable;
var ioTEventIdVal;
var ioTEventSourceVal;
var ioTEventTypeVal;
var ioTEventAssetIDVal;
var ioTEventStatusVal;
var ioTEventDispositionVal;
var ioTEventWorkOrderVal;
var ioTEventFaultCodeVal;
var ioTEventCreatedVal;
var ioTEventIoTDeviceIDVal;
var selectCount = 0;
var run = false;
var titleText = '';
var order = '0';
var orderDir = 'asc';
var gridname = "IoTEvent_Search";

function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
$(function () {
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    var ioteventstatus = localStorage.getItem("ioteventstatus");
    if (ioteventstatus) {
        CustomQueryDisplayId = ioteventstatus;
        $('#iotEventsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId && $(this).attr('id') != '0') {
                $('#iotEventsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else {
        localStorage.setItem("ioteventstatus", "2");
        ioteventstatus = localStorage.getItem("ioteventstatus");
        $('#iotEventsearchListul').children('li').eq(1).addClass('activeState');
        $('#iotEventsearchtitle').text($('#iotEventsearchListul').children('li').eq(1).text());
    }
    var titletext = $('#iotEventsearchtitle').text();
    localStorage.setItem("iotEventstatustext", titletext);
    generateEventInfoDataTable();
});

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

//Search
$(document).on('click', "#SrchBttnEvent", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'Event' },
        beforeSend: function () {
            ShowbtnLoader("SrchBttnEvent");
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
            HidebtnLoader("SrchBttnEvent");
        },
        error: function () {
            HidebtnLoader("SrchBttnEvent");
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
        data: { tableName: 'Event', searchText: txtSearchval, isClear: isClear },
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
                DTioTEventTable.page('first').draw('page');
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
$(document).on('click', '.txtSearchClick', function () {
    TextSearch();
});
function TextSearch() {
    run = true;
    GridclearAdvanceSearch();
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossIoTEvent" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        DTioTEventTable.page('first').draw('page');
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}

function GridclearAdvanceSearch() {
    $('#advsearchsidebar').find('input:text').val('');
    $('#advsearchsidebar').find("select").val("").trigger('change.select2');
    Gridfilteritemcount = 0;
    $(".filteritemcount").text(Gridfilteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
    ioTEventSourceVal = LRTrim($("#txtSource").val());
    ioTEventTypeValtxtType = LRTrim($("#txtType").val());
    ioTEventAssetIDVal = LRTrim($("#txtAssetId").val());
    ioTEventStatusVal = LRTrim($("#txtStatus").val());
    ioTEventDispositionVal = LRTrim($("#txtDisposition").val());
    ioTEventWorkOrderVal = $("#txtWorkOrder").val();
    ioTEventFaultCodeVal = $("#txtFaultCode").val();
    ioTEventCreatedVal = $("#txtCreated").val();
    ioTEventIoTDeviceIDVal = $("#txtIoTDeviceID").val();
}
$(document).on('click', '#UlSearchList li', function () {
    var v = LRTrim($(this).text());
    $(document).find('#txtColumnSearch').val(v);
    TextSearch();
});
//Search

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
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossIoTEvent" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parent('div').is(":visible")) {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    Gridfilteritemcount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossIoTEvent" aria-hidden="true"></a></span>';
                }
            }
            advcountercontainer.text(Gridfilteritemcount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);

}

$(document).on('keyup', '#iotEventsearctxtbox', function (e) {
    var tagElems = $(document).find('#iotEventsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('keyup', '#txtColumnSearch', function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == 13) {
        TextSearch();
    }
    else {
        event.preventDefault();
    }
});
$(document).ready(function () {
    $(document).find('.select2picker').select2({});
    ShowbtnLoaderclass("LoaderDrop");

    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $(document).find("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#drpDwnLink', function (e) {
        e.preventDefault();
        $(document).find("#drpDwn").slideToggle();
    });
    $(document).on('change', '#acknowledgeModel_FaultCode', function () {
        var tlen = $(document).find("#acknowledgeModel_FaultCode").val();
        if (tlen.length > 0) {
            var areaddescribedby = $(document).find("#acknowledgeModel_FaultCode").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#acknowledgeModel_FaultCode").removeClass("input-validation-error");
        }
        else {
            var areaddescribedby = $(document).find("#acknowledgeModel_FaultCode").attr('aria-describedby');
            $('#' + areaddescribedby).show();
            $(document).find('form').find("#acknowledgeModel_FaultCode").addClass("input-validation-error");
        }
    });
    $(document).on('change', '#dismissModel_DismissReason', function () {
        var tlen = $(document).find("#dismissModel_DismissReason").val();
        if (tlen.length > 0) {
            var areaddescribedby = $(document).find("#dismissModel_DismissReason").attr('aria-describedby');
            $('#' + areaddescribedby).hide();
            $(document).find('form').find("#dismissModel_DismissReason").removeClass("input-validation-error");
        }
        else {
            var areaddescribedby = $(document).find("#dismissModel_DismissReason").attr('aria-describedby');
            $('#' + areaddescribedby).show();
            $(document).find('form').find("#dismissModel_DismissReason").addClass("input-validation-error");
        }
    });

});

$(document).on('click', "#sidebarCollapse", function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$("#btnDataIoTAdvSearchiotEvent").on('click', function (e) {
    run = true;
    EventInfoAdvSearch();
    $('#sidebar').removeClass('active');
    $('#iotEventadvsearchcontainer').find('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    DTioTEventTable.page('first').draw('page');
});

function EventInfoAdvSearch() {
    $('#txtColumnSearch').val('');
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossIoTEvent" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#advsearchfilteritems").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
    $('.overlay').fadeOut();
    ioTEventSourceVal = LRTrim($("#txtSource").val());
    ioTEventTypeValtxtType = LRTrim($("#txtType").val());
    ioTEventAssetIDVal = LRTrim($("#txtAssetId").val());
    ioTEventStatusVal = LRTrim($("#txtStatus").val());
    ioTEventDispositionVal = LRTrim($("#txtDisposition").val());
    ioTEventWorkOrderVal = $("#txtWorkOrder").val();
    ioTEventFaultCodeVal = $("#txtFaultCode").val();
    ioTEventCreatedVal = $("#txtCreated").val();
    ioTEventIoTDeviceIDVal = $("#txtIoTDeviceID").val();
    DTioTEventTable.page('first').draw('page');
}
$(document).on('click', "ul.vtabs li", function () {
    $("ul.vtabs li").removeClass("active");
    $(this).addClass("active");
    $(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(activeTab).fadeIn();
    return false;
});


//#region Generate EventInfo func()
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


function generateEventInfoDataTable() {
    if ($(document).find('#iotEventSearchTable').hasClass('dataTable')) {
        DTioTEventTable.destroy();
    }
    DTioTEventTable = $("#iotEventSearchTable").DataTable({
        colReorder: {
            fixedColumnsLeft: 1
        },
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
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: gridname,
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
                    GridName: gridname
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    Gridfilteritemcount = 0;
                    if (json.LayoutInfo !== '') {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo !== '') {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
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
            leftColumns: 1,
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'EventInfo List'
            },
            {
                extend: 'print',
                title: 'EventInfo List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'EventInfo List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'EventInfo List',
                orientation: 'landscape',
                pageSize: 'A3'
            }

        ],
        "orderMulti": true,
        "ajax": {
            "url": "/IoTEvent/GetIotEventChunkSearch",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.customQueryDisplayId = CustomQueryDisplayId;
                d.ioTEventSource = LRTrim($("#txtSource").val());
                d.ioTEventType = LRTrim($("#txtType").val());
                d.ioTStatus = LRTrim($("#txtStatus").val());
                d.iotDisposition = LRTrim($("#txtDisposition").val());
                d.iotWorkOrderId = LRTrim($("#txtWorkOrder").val());
                d.iotFaultCode = LRTrim($("#txtFaultCode").val());
                d.iotCreateDate = LRTrim($("#txtCreated").val());
                d.assetClentLookupId = LRTrim($("#txtAssetId").val());
                d.IoTDeviceID = LRTrim($("#txtIoTDeviceID").val());
                d.txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
            },
            "dataSrc": function (result) {
                let colOrder = DTioTEventTable.order();
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
                    "data": "IoTEventId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_ioteventdetails href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "SourceType", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1", "className": "text-left", },
                { "data": "EventType", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", "className": "text-left", },
                { "data": "AssetID", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3", "className": "text-left", },
                { "data": "AssetName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4", "className": "text-left", },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5", "className": "text-left",
                    render: function (data, type, row, meta) {
                        if (data == statusCode.Processed) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Open) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "Disposition", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6", "className": "text-left", },
                { "data": "WOClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7", "className": "text-left", },
                { "data": "FaultCode", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8", "className": "text-left", },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "9", "className": "text-left", },
                { "data": "IoTDeviceClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "10", "className": "text-center", },
                { "data": "ProcessDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "11", "className": "text-left", },
                {
                    "data": "ProcessBy_Personnel", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "12", "className": "text-left",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-100'>" + data + "</div>";
                    }
                },

                {
                    "data": "Comments", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "13", "className": "text-left",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                }
            ],
        "columnDefs": [
            {
                targets: [0],
                className: 'noVis'
            }

        ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#eventInfoAction :input").removeAttr("disabled");
            $("#eventInfoAction :button").removeClass("disabled");
            DisableExportButton($("#iotEventSearchTable"), $(document).find('.import-export'));
        }
    });
};
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var colname = order;//WO Sorting
            var coldir = orderDir;//WO Sorting
            var jsonResult = $.ajax({
                "url": "/IoTEvent/GetIoTEventPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    colname: colname,//WO Sorting
                    coldir: coldir,//WO Sorting
                    customQueryDisplayId: CustomQueryDisplayId,
                    iotEventId: LRTrim($("#txteventId").val()),
                    iotEventSource : LRTrim($("#txtSource").val()),
                    iotEventType : LRTrim($("#txtType").val()),
                    assetClentLookupId : LRTrim($("#txtAssetId").val()),
                    iotStatus : LRTrim($("#txtStatus").val()),
                    iotDisposition : LRTrim($("#txtDisposition").val()),
                    iotWorkOrderId: LRTrim($("#txtWorkOrder").val()),
                    iotFaultCode: LRTrim($("#txtFaultCode").val()),
                    iotCreateDate: LRTrim($("#txtCreated").val()),
                    iotDeviceID: LRTrim($("#txtIoTDeviceID").val()),
                    txtSearchval : LRTrim($(document).find('#txtColumnSearch').val())
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#iotEventSearchTable thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.IoTEventId != null) {
                    item.IoTEventId = item.IoTEventId;
                }
                else {
                    item.IoTEventId = "";
                }
                if (item.SourceType != null) {
                    item.SourceType = item.SourceType;
                }
                else {
                    item.SourceType = "";
                }
                if (item.EventType != null) {
                    item.EventType = item.EventType;
                }
                else {
                    item.EventType = "";
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
                if (item.Status != null) {
                    item.Status = item.Status;
                }
                else {
                    item.Status = "";
                }
                if (item.Disposition != null) {
                    item.Disposition = item.Disposition;
                }
                else {
                    item.Disposition = "";
                }
                if (item.WOClientLookupId != null) {
                    item.WOClientLookupId = item.WOClientLookupId;
                }
                else {
                    item.WOClientLookupId = "";
                }
                if (item.FaultCode != null) {
                    item.FaultCode = item.FaultCode;
                }
                else {
                    item.FaultCode = "";
                }
                if (item.CreateDate != null) {
                    item.CreateDate = item.CreateDate;
                }
                else {
                    item.CreateDate = "";
                }
                if (item.iotDeviceID != null) {
                    item.iotDeviceID = item.iotDeviceID;
                }
                else {
                    item.iotDeviceID = "";
                }
                if (item.ProcessDate != null) {
                    item.ProcessDate = item.ProcessDate;
                }
                else {
                    item.ProcessDate = "";
                }
                if (item.ProcessBy_Personnel != null) {
                    item.ProcessBy_Personnel = item.ProcessBy_Personnel;
                }
                else {
                    item.ProcessBy_Personnel = "";
                }
                if (item.Comments != null) {
                    item.Comments = item.Comments;
                }
                else {
                    item.Comments = "";
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
                header: $("#iotEventSearchTable thead tr th div").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});

//#endregion

$(document).on('click', '#iotEventSearchTable_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#iotEventSearchTable_length .searchdt-menu', function () {
    run = true;
});
$('#iotEventSearchTable').find('th').click(function () {
    if ($(this).data('col') !== undefined && $(this).data('col') !== '') {
        run = true;
        order = $(this).data('col');
    }
   
});


$(document).on('click', '.iotEventsearchdrpbox', function (e) {
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#advsearchfilteritems").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    if ($(this).attr('id') != '0') {
        $('#iotEventsearchtitle').text($(this).text());
    }
    else {
        $('#iotEventsearchtitle').text("IoTEvent");
    }
    var titletext = $('#iotEventsearchtitle').text();
    localStorage.setItem("iotEventstatustext", titletext);
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("ioteventstatus", optionval);
    CustomQueryDisplayId = optionval;
    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        DTioTEventTable.page('first').draw('page');
    }
});

$(document).on('click', '.btnCrossIoTEvent', function () {
    run = true;
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
   
    if (Gridfilteritemcount > 0) Gridfilteritemcount--;
    ioTEventSourceVal = LRTrim($("#txtSource").val());
    ioTEventTypeValtxtType = LRTrim($("#txtType").val());
    ioTEventAssetIDVal = LRTrim($("#txtAssetId").val());
    ioTEventStatusVal = LRTrim($("#txtStatus").val());
    ioTEventDispositionVal = LRTrim($("#txtDisposition").val());
    ioTEventWorkOrderVal = $("#txtWorkOrder").val();
    ioTEventFaultCodeVal = $("#txtFaultCode").val();
    ioTEventCreatedVal = $("#txtCreated").val();
    ioTEventIoTDeviceIDVal = $("#txtIoTDeviceID").val();
    GridAdvanceSearch();
    DTioTEventTable.page('first').draw('page');
});

$(document).on('click', "#btnDataAdvSrchiotEvent", function (e) {
    run = true;
    $(document).find('#txtColumnSearch').val('');
    var searchitemhtml = "";
    Gridfilteritemcount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            Gridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossIoTEvent" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#renderdevices').find('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    ioTEventSourceVal = LRTrim($("#txtSource").val());
    ioTEventTypeValtxtType = LRTrim($("#txtType").val());
    ioTEventAssetIDVal = LRTrim($("#txtAssetId").val());
    ioTEventStatusVal = LRTrim($("#txtStatus").val());
    ioTEventDispositionVal = LRTrim($("#txtDisposition").val());
    ioTEventWorkOrderVal = $("#txtWorkOrder").val();
    ioTEventFaultCodeVal = $("#txtFaultCode").val();
    ioTEventCreatedVal = $("#txtCreated").val();
    ioTEventIoTDeviceIDVal = $("#txtIoTDeviceID").val();
    GridAdvanceSearch();
    DTioTEventTable.page('first').draw('page');
});
function GridAdvanceSearch() {
    $('.filteritemcount').text(Gridfilteritemcount);
}

$(document).on('click', '#lnkAcknowledge', function () {
    $('#modalAcknowledge').modal('show');
});
$(document).on('click', '#lnkDismiss', function () {
    $('#modalDismiss').modal('show');
});
//#region Details IotEventInfo
$(document).on('click', '.lnk_ioteventdetails', function (e) {
    
    var eventId;
    var row = $(this).parents('tr');
    var data = DTioTEventTable.row(row).data();
    eventId = data.IoTEventId;
    var titletext = $('#iotEventsearchtitle').text();
    localStorage.setItem("iotEventstatustext", titletext);
    $.ajax({
        url: "/IoTEvent/IoTEventDetails",
        type: "POST",
        data: { ioTEventId: eventId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendereventinfo').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../IoTEvent/Index?page=Monitoring_Events";
});
//#endregion

//#region addOnSuccess
function ModalDismissOnSuccess(data) {
    CloseLoader();
    if (data.Issuccess) {
        $('#modalDismiss').modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("IoTEventInfoDismissAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToDetail($(document).find("#dismissModel_IoTEventId").val());
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#modalDismiss');
    }
}
function ModalAcknowledgeOnSuccess(data) {
    CloseLoader();
    if (data.Issuccess) {
        $('#modalAcknowledge').modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("IoTEventAcknowledgeAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToDetail($(document).find("#acknowledgeModel_IoTEventId").val());
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#modalAcknowledge');
    }
}
//#endregion
$(document).on('click', '#dismiss, .overlay', function () {
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
});


//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(DTioTEventTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(DTioTEventTable, colOrder);
    run = true;
    DTioTEventTable.state.save(run);
});
//#endregion

//#region Reset Grid
$('#liResetGridClearBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("ioteventstatus");
        localstorageKeys.push("iotEventstatustext");
        DeleteGridLayout(gridname, DTioTEventTable, localstorageKeys);
        GenerateSearchList('', true);
        window.location.href = "../IoTEvent/Index?page=Monitoring_Events";
    });
});
//#endregion

//#region Reload the details page 
function RedirectToDetail(ioTEventId) {
    var titletext = $('#iotEventsearchtitle').text();
    localStorage.setItem("iotEventstatustext", titletext);
    $.ajax({
        url: "/IoTEvent/IoTEventDetails",
        type: "POST",
        data: { ioTEventId: ioTEventId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendereventinfo').html(data);
            $(document).find('#spnlinkToSearch').text(titletext);
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