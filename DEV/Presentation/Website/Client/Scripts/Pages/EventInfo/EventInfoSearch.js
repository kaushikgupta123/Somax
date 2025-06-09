var DTeventInfoTable;
var argValStat;
var run = false;
//#region OnPageLoadJs
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
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });

    $(".actionBar").fadeIn();
    $("#eventInfoAction :input").attr("disabled", "disabled");

    var dropkey = localStorage.getItem("DROPKEYEVENT");
    if (dropkey) {
        generateEventInfoDataTable();
        $('#SearchTextDropID').val(dropkey).trigger('change.select2');
        argValStat = dropkey;

    }
    else {
        argValStat = 0;
        generateEventInfoDataTable();
    }

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

$(document).find('#sidebarCollapse').on('click', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('click', "ul.vtabs li", function () {
    $("ul.vtabs li").removeClass("active");
    $(this).addClass("active");
    $(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(activeTab).fadeIn();
    return false;
});
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "Notes":
            GenerateNoteGrid();
            break;
        case "Attachment":
            GenerateAttachmentGrid();
            break;
    }
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
    if ($(document).find('#eventInfoSearchTable').hasClass('dataTable')) {
        DTeventInfoTable.destroy();
    }
    DTeventInfoTable = $("#eventInfoSearchTable").DataTable({
        colReorder: {
            fixedColumnsLeft: 1
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        "stateSaveCallback": function (settings, data) {
            // Send an Ajax request to the server with the state object
            if (run == true) {
                var filterinfoarray = getfilterinfoarray($('#advsearchsidebar'));
                $.ajax({
                    "url": gridStateSaveUrl,//"/Base/CreateUpdateState",
                    "data": {
                        GridName: "EventInfo_Search",
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
            var o;
            $.ajax({
                "url": "/Base/GetLayout",
                "data": {
                    GridName: "EventInfo_Search"
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    selectCount = 0;
                    if (json.LayoutInfo !== '') {
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo !== '') {
                            setsearchui(JSON.parse(json.FilterInfo), $(".filteritemcount"), $("#advsearchfilteritems"));
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
            "url": "/EventInfo/GetEventInfoGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.SearchTextDropID = argValStat;
                d.EventInfoId = LRTrim($("#txteventId").val());
                d.SourceType = LRTrim($("#txtSource").val());
                d.EventType = LRTrim($("#txtType").val());
                d.Description = LRTrim($("#txtDescription").val());
                d.Status = LRTrim($("#txtStatus").val());
                d.Disposition = LRTrim($("#txtDisposition").val());
                d.WOClientLookupId = LRTrim($("#txtWorkOrder").val());
                d.FaultCode = LRTrim($("#txtFaultCode").val());
                d.CreateDate = LRTrim($("#txtCreated").val());
                d.SensorId = LRTrim($("#txtSensorID").val());
                d.ProcessDate = LRTrim($("#txtProcessed").val());
                d.ProcessBy_Personnel = LRTrim($("#txtProcessedBy").val());
                d.Comments = LRTrim($("#txtComments").val());
            },
            "dataSrc": function (result) {
                if (result.data.length < 1) {
                    $(document).find('.import-export').prop('disabled', true);
                }
                else {
                    $(document).find('.import-export').prop('disabled', false);
                }

                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "EventInfoId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_eventdetails href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "SourceType", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1", "className": "text-left", },
                { "data": "EventType", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", "className": "text-left", },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3", "className": "text-left",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4", "className": "text-left",
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
                { "data": "Disposition", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5", "className": "text-left", },
                { "data": "WOClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6", "className": "text-left", },
                { "data": "FaultCode", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7", "className": "text-left", },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8", "className": "text-left", },
                { "data": "SensorId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "9", "className": "text-center", },
                { "data": "ProcessDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "10", "className": "text-left", },
                { "data": "ProcessBy_Personnel", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "11", "className": "text-left", },
                {
                    "data": "Comments", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "12", "className": "text-left",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
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
            DisableExportButton($("#eventInfoSearchTable"), $(document).find('.import-export'));
        }
    });
};
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var jsonResult = $.ajax({
                "url": "/EventInfo/GetEventInfoPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    SearchTextDropID: argValStat,
                    EventInfoId: LRTrim($("#txteventId").val()),
                    SourceType: LRTrim($("#txtSource").val()),
                    EventType: LRTrim($("#txtType").val()),
                    Description: LRTrim($("#txtDescription").val()),
                    Status: LRTrim($("#txtStatus").val()),
                    Disposition: LRTrim($("#txtDisposition").val()),
                    WOClientLookupId: LRTrim($("#txtWorkOrder").val()),
                    FaultCode: LRTrim($("#txtFaultCode").val()),
                    CreateDate: LRTrim($("#txtCreated").val()),
                    SensorId: LRTrim($("#txtSensorID").val()),
                    ProcessDate: LRTrim($("#txtProcessed").val()),
                    ProcessBy_Personnel: LRTrim($("#txtProcessedBy").val()),
                    Comments: LRTrim($("#txtComments").val())
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#eventInfoSearchTable thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.EventInfoId != null) {
                    item.EventInfoId = item.EventInfoId;
                }
                else {
                    item.EventInfoId = "";
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
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
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
                if (item.SensorId != null) {
                    item.SensorId = item.SensorId;
                }
                else {
                    item.SensorId = "";
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
                header: $("#eventInfoSearchTable thead tr th div").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});
//#endregion
//#region Search
$(document).on('change', '#SearchTextDropID', function () {
    run = true;
    ShowbtnLoaderclass("LoaderDrop");
    var optionval = $('#SearchTextDropID option:selected').val();
    argValStat = optionval;
    localStorage.setItem("DROPKEYEVENT", optionval);
    if (optionval.length !== 0) {
        DTeventInfoTable.page('first').draw('page');
    }
});

$("#btnAdvanceSearch").on('click', function (e) {
    run = true;
    EventInfoAdvSearch();
    $('#sidebar').removeClass('active');
    $('#EventInfoadvsearchcontainer').find('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    DTeventInfoTable.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('');
    $(this).parent().remove();
    if (searchtxtId == "txtType") {
        $('#' + searchtxtId).val('').trigger('change');
    }
    if (searchtxtId == "txtStatus") {
        $('#' + searchtxtId).val('').trigger('change');
    }
    if (searchtxtId == "txtDisposition") {
        $('#' + searchtxtId).val('').trigger('change');
    }
    if (selectCount > 0) selectCount--;
    $(".filteritemcount").text(selectCount);
    DTeventInfoTable.page('first').draw('page');
});

$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $('#SearchTextDropID').val(0).trigger('change.select2');
    argValStat = 0;
    localStorage.removeItem("DROPKEYEVENT");
    clearAdvanceSearch();
    DTeventInfoTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    selectCount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $("#advsearchsidebar").find("textarea").val('');
    $(".filteritemcount").text(selectCount);
    $('#advsearchfilteritems').find('span').html('');
    clearAllAdvanceSearchField();
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}

function clearAllAdvanceSearchField() {
    $("#txtType").val("").trigger('change');
    $("#txtStatus").val("").trigger('change');
    $("#txtDisposition").val("").trigger('change');
}
function EventInfoAdvSearch() {
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#advsearchfilteritems").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
$(document).on('click', '.lnk_accountdetails', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = DTaccountSearchTable.row(row).data();
    var accountId = data.AccountId;
    $.ajax({
        url: "/Account/AccountDetail",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { AccountId: accountId },
        success: function (data) {
            $('#renderAccountPage').html(data);
        },
        complete: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region Details EventInfo
$(document).on('click', '.lnk_eventdetails', function (e) {
    var row = $(this).parents('tr');
    var data = DTeventInfoTable.row(row).data();
    $.ajax({
        url: "/EventInfo/EventInfoDetails",
        type: "POST",
        data: { EventId: data.EventInfoId },
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
});
$(document).on('click', '#lnkCreateWo', function () {
    $('#modalCreateWorkOrder').modal('show');
});
//#endregion
//#region addOnSuccess
function ModalDismissOnSuccess(data) {
    CloseLoader();
    if (data.Issuccess) {
        $('#modalDismiss').modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("EventInfoDismissAlert");
        swal(SuccessAlertSetting, function () {
            $(document).find("#eventInfoModel_Disposition").text(data.Type);
            $(document).find("#eventInfoModel_Comments").text(data.Comments);
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
        SuccessAlertSetting.text = getResourceValue("EventInfoAcknowledgeAlert");
        swal(SuccessAlertSetting, function () {
            $(document).find("#eventInfoModel_Disposition").text(data.Type);
            $(document).find("#eventInfoModel_FaultCode").text(data.DropdownValue);
            $(document).find("#eventInfoModel_Comments").text(data.Comments);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#modalAcknowledge');
    }
}
//#endregion
//#region Common
function SetEventEnvironmentPage() {
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $(document).find('.select2picker').select2({});
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
}
//#endregion

$(document).on('click', '#eventInfoSearchTable_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#eventInfoSearchTable_length .searchdt-menu', function () {
    run = true;
});
$(document).on('click', '#eventInfoSearchTable_wrapper th', function () {
    run = true;
});

//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(DTeventInfoTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(DTeventInfoTable, colOrder);
    run = true;
    DTeventInfoTable.state.save(run);
});
//#endregion

//#region V2-389
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
function getfilterinfoarray(advsearchcontainer) {
    var filterinfoarray = [];
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parent('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);

            if ($(this).parent('div').find('div').hasClass('range-timeperiod')) {
                if ($(this).parent('div').find('input').val() !== '' && $(this).val() == '10') {
                    f = new filterinfo('this-' + $(this).attr('id'), $(this).parent('div').find('input').val());
                    filterinfoarray.push(f);
                }
            }

        }
    });
    return filterinfoarray;
}
function setsearchui(data, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if ($('#' + item.key).parent('div').is(":visible")) {
            $('#' + item.key).val(item.value).trigger('change');
            if (item.value) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
        advcountercontainer.text(selectCount);
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}
//#endregion
