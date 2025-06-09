//#region Search
var prevMaintLibrarySearchTable;
var FrequencytypeVal;
var run = false;
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
function generatePrevMaintLibraryDataTable() {
    var printCounter = 0;
    if ($(document).find('#prevMaintLibrarySearchTable').hasClass('dataTable')) {
        prevMaintLibrarySearchTable.destroy();
    }
    prevMaintLibrarySearchTable = $("#prevMaintLibrarySearchTable").DataTable({
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
        "stateSaveCallback": function (settings, data) {
            // Send an Ajax request to the server with the state object
            if (run == true) {
                $.ajax({
                    "url": gridStateSaveUrl,//"/Base/CreateUpdateState",
                    "data": {
                        GridName: "PrevMaintLibrary_Search",
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
            var o;
            $.ajax({
                "url": gridStateLoadUrl,
                "data": {
                    GridName: "PrevMaintLibrary_Search"
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
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [

            {
                extend: 'excelHtml5',
                title: 'Preventive Maintenance Library'
            },
            {
                extend: 'print',
                title: 'Preventive Maintenance Library'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Preventive Maintenance Library',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                css: 'display:none',
                title: 'Preventive Maintenance Library',
                orientation: 'landscape',
                pageSize: 'A2'
            }
      
       ],
        "orderMulti": true,
        "ajax": {
            "url": "/PreventiveMaintenanceLibrary/GetPreventiveMaintenanceLibraryGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.SearchText = LRTrim($('#txtPMLsearchbox').val());
                d.ClientLookupId = LRTrim($("#PMLPmMasterId").val());
                d.Description = LRTrim($("#PMLDescription").val());
                d.JobDuration = LRTrim($("#PMLDuration").val());
                d.FrequencyType = $('#PMLFrequencyType').val();
                d.Frequency = LRTrim($("#PMLFrequency").val());
                d.CreateDate = $('#PMLCreated').val();
            },
            "dataSrc": function (result) {
                if (result.data.length == "0") {
                    $(document).find('.import-export').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }
                //#region type-drop-down
                $("#PMLFrequencyType").empty();
                $("#PMLFrequencyType").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.FrequencyTypeList.length; i++) {
                    var id = result.FrequencyTypeList[i];
                    var name = result.FrequencyTypeList[i];
                    $("#PMLFrequencyType").append("<option value='" + id + "'>" + name + "</option>");
                }
                if (FrequencytypeVal) {
                    $("#PMLFrequencyType").val(FrequencytypeVal);
                }
                //#endregion
              

                return result.data;
            },
            global: true
        },
        "columns":
        [
            {
                "data": "ClientLookupId",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "className": "text-left",
                "name": "0",
                "mRender": function (data, type, row) {
                    return '<a class=lnk_prevmaintanance href="javascript:void(0)">' + data + '</a>';
                }
            },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-400'>" + data + "</div>";
                }
            },
            { "data": "JobDuration", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
            { "data": "FrequencyType", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
            { "data": "Frequency", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
            { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
        ],
        "columnDefs": [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#PMlibraryGridAction :input").not('.import-export').removeAttr("disabled");
            $("#PMlibraryGridAction :button").not('.import-export').removeClass("disabled");
        }
    });
};
$('#btnmainPMLsearch').on('click', function () {
    run = true;
    clearAdvanceSearch();
    prevMaintLibrarySearchTable.page('first').draw('page');
    //prevMaintLibrarySearchTable.state.clear();
    //generatePrevMaintLibraryDataTable();
});
$(document).on('click', '.lnk_prevmaintanance', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = prevMaintLibrarySearchTable.row(row).data();
    $.ajax({
        url: "/PreventiveMaintenanceLibrary/PreventiveMaintenanceLibraryDetail",
        type: "POST",
        dataType: 'html',
        data: { PrevMaintLibraryId: data.PrevMaintLibraryId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventiveLibrary').html(data);
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
//#region Advance Search
$(document).on('click', "#btnPMLibraryDataAdvSrch", function (e) {
   // prevMaintLibrarySearchTable.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#txtPMLsearchbox').val('');
    $('#advsearchsidebarPMLibrary').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossPML" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);   
    $('#PMLibraryadvsearchcontainer').find('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    FrequencytypeVal = $("#PMLFrequencyType").val();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    run=true;
    prevMaintLibrarySearchTable.page('first').draw('page');
   // generatePrevMaintLibraryDataTable();
    $('.filteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '.btnCrossPML', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    if (searchtxtId == "PMLFrequencyType") {
        FrequencytypeVal = null;
    }
    GridAdvanceSearch();
});
$(document).on('click', '#liClearAdvSearchFilterPML', function () {
    run = true;
    $("#txtPMLsearchbox").val("");
    clearAdvanceSearch();
    prevMaintLibrarySearchTable.page('first').draw('page');
   // generatePrevMaintLibraryDataTable();
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebarPMLibrary').find('input:text').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $("#PMLFrequencyType").val("").trigger('change');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
    FrequencytypeVal = $("#PMLFrequencyType").val();
}
//#endregion
//#region print
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var searchText = LRTrim($('#txtPMLsearchbox').val());
            var clientLookupId = LRTrim($("#PMLPmMasterId").val());
            var description = LRTrim($("#PMLDescription").val());
            var jobDuration = LRTrim($("#PMLDuration").val());
            var frequencyType = $('#PMLFrequencyType').val();
            if (!frequencyType) {
                frequencyType = "";
            }
            var frequency = LRTrim($("#PMLFrequency").val());
            var createDate = $('#PMLCreated').val();
            dtTable = $("#prevMaintLibrarySearchTable").DataTable();
            var currestsortedcolumn = $('#prevMaintLibrarySearchTable').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#prevMaintLibrarySearchTable').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#prevMaintLibrarySearchTable').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/PreventiveMaintenanceLibrary/GetPreventiveMaintenanceLibraryPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    ClientLookupId: clientLookupId,
                    Description: description,
                    JobDuration: jobDuration,
                    FrequencyType: frequencyType,
                    Frequency: frequency,
                    CreateDate: createDate,
                    SearchText: searchText,
                    colname: colname,
                    coldir: coldir
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#prevMaintLibrarySearchTable thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.ClientLookupId != null) {
                    item.ClientLookupId = item.ClientLookupId;
                }
                else {
                    item.ClientLookupId = "";
                }
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
                }
                if (item.JobDuration != null) {
                    item.JobDuration = item.JobDuration;
                }
                else {
                    item.JobDuration = "";
                }
                if (item.FrequencyType != null) {
                    item.FrequencyType = item.FrequencyType;
                }
                else {
                    item.FrequencyType = "";
                }
                if (item.Frequency != null) {
                    item.Frequency = item.Frequency;
                }
                else {
                    item.Frequency = "";
                }
                if (item.CreateDate != null) {
                    item.CreateDate = item.CreateDate;
                }
                else {
                    item.CreateDate = "";
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
                header: $("#prevMaintLibrarySearchTable thead tr th div").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});

//#endregion
//#region Common
$(document).ready(function () {
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
    $(document).find('.select2picker').select2({});

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
    $("#PMlibraryGridAction :input").attr("disabled", "disabled");
});
$(document).on('click', '#PMLsidebarCollapse', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "Tasks":
            GenerateTaskGrid();
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
$(document).on('click', "ul.vtabs li", function () {
    if ($(this).find('#drpDwnLink').length > 0) {
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
$(function () {
    generatePrevMaintLibraryDataTable();   
});
function RedirectToDetailOncancel(pmlId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToPmlDetail(pmlId, mode);
    });
}
function RedirectToPmlDetail(pmlId, mode) {
    $.ajax({
        url: "/PreventiveMaintenanceLibrary/PreventiveMaintenanceLibraryDetail",
        type: "POST",
        dataType: 'html',
        data: { PrevMaintLibraryId: pmlId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventiveLibrary').html(data);
        },
        complete: function () {
            CloseLoader();
            if (mode === "Tasks") {
                $('#litask').trigger('click');
                $('#colorselector').val('Tasks');
            }
            SetPMLControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SetPMLControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
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
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    });
}
//#endregion
//#region Preventive Maintenance Library Add/Edit
function PMLibraryAddEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command === "save") {
            var message;
            if (data.mode === "add") {
                SuccessAlertSetting.text = getResourceValue("PMLibraryAddAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("PMLibraryUpdateAlert");
            }
            swal(SuccessAlertSetting, function () {

                RedirectToPmlDetail(data.PrevMaintLibraryId, "overview");
            });
        }
        else {
            ResetErrorDiv();
            $('#identificationtab').addClass('active').trigger('click');
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
}
$(document).on('click', ".addPrevMaintLibrary,.addPreventiveMaintenanceLibrary", function (e) {
    $.ajax({
        url: "/PreventiveMaintenanceLibrary/AddEditPMLibrary",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventiveLibrary').html(data);
        },
        complete: function () {
            SetPMLControls();
        },
        error: function () {
            CloseLoader();
        }
    });
})
$(document).on('click', "#editpreventiveMaintenanceLibrary", function (e) {
    e.preventDefault();
    var pmlId = $('#preventiveMaintenanceLibraryModel_PrevMaintLibraryId').val();
    $.ajax({
        url: "/PreventiveMaintenanceLibrary/AddEditPMLibrary",
        type: "GET",
        dataType: 'html',
        data: { PrevMaintLibraryId: pmlId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventiveLibrary').html(data);
        },
        complete: function () {
            SetPMLControls();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnsPMLibrarycancel", function () {
    var pmlId = $('#preventiveMaintenanceLibraryModel_PrevMaintLibraryId').val();
    if (typeof pmlId != "undefined" && pmlId != 0) {
        RedirectToDetailOncancel(pmlId);
    }
    else {
        swal(CancelAlertSetting, function () {
            window.location.href = "../PreventiveMaintenanceLibrary/Index?page=Libraries_Preventive_Maintenance";
        });
    }
});
//#endregion

$(document).on('click', '#prevMaintLibrarySearchTable_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#prevMaintLibrarySearchTable_length .searchdt-menu', function () {
    run = true;
});
$(document).on('click', '#prevMaintLibrarySearchTable_wrapper th', function () {
    run = true;
});


//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(prevMaintLibrarySearchTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(prevMaintLibrarySearchTable, colOrder);
    run = true;
    prevMaintLibrarySearchTable.state.save(run);
});
//#endregion