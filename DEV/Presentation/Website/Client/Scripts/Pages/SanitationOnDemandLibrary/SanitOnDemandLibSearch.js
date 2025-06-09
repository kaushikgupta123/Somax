var run = false;
$(function () {
    generateSanitOnDemandLibTable();
    $(document).on('click', '#SODLsidebarCollapse', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });

    $(document).find('.select2picker').select2({});
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
});
$(document).ready(function () {
    $(".actionBar").fadeIn();
    $("#SanitOnDemandGridAction :input").attr("disabled", "disabled");
});
//#region Search
var sanitOnDemandLibSearchTable;
function generateSanitOnDemandLibTable() {
    var printCounter = 0;
    if ($(document).find('#sanitOnDemandLibSearchTable').hasClass('dataTable')) {
        sanitOnDemandLibSearchTable.destroy();
    }
    sanitOnDemandLibSearchTable = $("#sanitOnDemandLibSearchTable").DataTable({
        colReorder: {
            fixedColumnsLeft: 0
        },
        rowGrouping: true,
        colReorder: true,
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
                        GridName: "SanOnDemandMasterSearch",
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
                "url": "/Base/GetState",
                "data": {
                    GridName: gridStateLoadUrl,//"/Base/GetState",
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
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Sanitation On-Demand Library'
            },
            {
                extend: 'print',
                title: 'Sanitation On-Demand Library'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Sanitation On-Demand Library',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                css: 'display:none',
                title: 'Sanitation On-Demand Library',
                orientation: 'landscape',
                pageSize: 'A2'
            }

        ],
        "orderMulti": true,
        "ajax": {
            "url": "/SanitationOnDemandLibrary/GetSanitOnDemandLibraryGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.SearchText = LRTrim($('#txtSODLsearchbox').val());
                d.ClientLookupId = LRTrim($("#SODLOnDemandId").val());
                d.Description = LRTrim($("#SODLDescription").val());
                d.CreateDate = $('#SODLCreated').val();
            },
            "dataSrc": function (result) {
                if (result.data.length < 1) {
                    $(document).find('.import-export').prop('disabled', true);
                }
                else {
                    $(document).find('.import-export').prop('disabled', false);
                }               
                return result.data;
            },
            global: true
        },
        "columns":
        [
            {
                "data": "ClientLookUpId",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "className": "text-left",
                "name": "0",
                "mRender": function (data, type, row) {
                    return '<a class=lnk_sanitondemand href="javascript:void(0)">' + data + '</a>';
                }
            },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-400'>" + data + "</div>";
                }
            },
            { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2", },
        ],
        "columnDefs": [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#SanitOnDemandGridAction :input").removeAttr("disabled");
            $("#SanitOnDemandGridAction :button").removeClass("disabled");
            DisableExportButton($("#sanitOnDemandLibSearchTable"), $(document).find('.import-export'));
        }
    });
};
$('#btnmainSODLsearch').on('click', function () {
    clearAdvanceSearch();
    sanitOnDemandLibSearchTable.page('first').draw('page');
    //sanitOnDemandLibSearchTable.state.clear();
    //generateSanitOnDemandLibTable();
});
//#endregion Search
//#region Adv Search
var hGridfilteritemcount = 0;
$(document).on('click', "#btnSODLibraryDataAdvSrch", function (e) {
    //sanitOnDemandLibSearchTable.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#txtSODLsearchbox').val('');
    $('#advsearchsidebarSODLibrary').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossSODL" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#sidebar').removeClass('active');
    $('#SODLibraryadvsearchcontainer').find('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
    sanitOnDemandLibSearchTable.page('first').draw('page');
});
function GridAdvanceSearch() {   
    //generateSanitOnDemandLibTable();
    $('.filteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '.btnCrossSODL', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    GridAdvanceSearch();
    sanitOnDemandLibSearchTable.page('first').draw('page');
});
$(document).on('click', '#liClearAdvSearchFilterSODL', function () {
    run = true;
    $("#txtSODLsearchbox").val("");
    clearAdvanceSearch();
    //generateSanitOnDemandLibTable();
    sanitOnDemandLibSearchTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebarSODLibrary').find('input:text').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion Adv search
//#region Print
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var searchText = LRTrim($('#txtSODLsearchbox').val());
            var clientLookupId = LRTrim($("#SODLOnDemandId").val());
            var description = LRTrim($("#SODLDescription").val());
            var createDate = $('#SODLCreated').val();
            dtTable = $("#sanitOnDemandLibSearchTable").DataTable();
            var currestsortedcolumn = $('#sanitOnDemandLibSearchTable').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#sanitOnDemandLibSearchTable').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#sanitOnDemandLibSearchTable').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/SanitationOnDemandLibrary/GetSanitOnDemandLibraryPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    ClientLookupId: clientLookupId,
                    Description: description,
                    CreateDate: createDate,
                    SearchText: searchText,
                    colname: colname,
                    coldir:coldir
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#sanitOnDemandLibSearchTable thead tr th").map(function (key) {
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
                header: $("#sanitOnDemandLibSearchTable thead tr th").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
})
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
//#endregion Print
//#region Sanit OnDemand Details
$(document).on('click', '.lnk_sanitondemand', function (e) {
    e.preventDefault();
    var index_row = $('#sanitOnDemandLibSearchTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = sanitOnDemandLibSearchTable.row(row).data();
    var SanOnDemandMasterId = data.SanOnDemandMasterId;
    $.ajax({
        url: "/SanitationOnDemandLibrary/SanitOnDemandLibDetails",
        type: "GET",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { sanOnDemandMasterId: SanOnDemandMasterId },
        success: function (data) {
            $('#rendersanitationondemandLibrary').html(data);
        },
        complete: function (data) {
            
            CloseLoader();
        },
        error: function (xhr, error) {
        }
    });
});
$(document).on('click', ".addSanitOnDemandLibrary", function (e) {
    $.ajax({
        url: "/SanitationOnDemandLibrary/AddOrEditSanitOnDemandLib",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitationondemandLibrary').html(data);
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $(document).find('.select2picker').select2({});
            $('input, form').blur(function () {
                $(this).valid();
            });
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#editSODL", function (e) {
    e.preventDefault();
    var sanId = $('#sanitationOnDemandLibModel_SanOnDemandMasterId').val();
    $.ajax({
        url: "/SanitationOnDemandLibrary/AddOrEditSanitOnDemandLib",
        type: "GET",
        dataType: 'html',
        data: { sanOnDemandMasterId: sanId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitationondemandLibrary').html(data);
        },
        complete: function () {
            SetOnDemmandControl();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function SODLibraryAddEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command === "save") {
            var message;
            if (data.mode === "add") {
                SuccessAlertSetting.text = getResourceValue("SanitationOnDemandLibraryaddedsuccessfullyAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("SanitationOnDemandLibraryupdatedsuccessfullyAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToSodlDetail(data.SanOnDemandMasterId, "overview");
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
$(document).on('click', "#btnsSODLibrarycancel", function () {
    var sanId = $('#sanitationOnDemandLibModel_SanOnDemandMasterId').val();
    if (typeof sanId != "undefined" && sanId != 0) {
        RedirectToDetailOncancel(sanId);
    }
    else {
        swal(CancelAlertSetting, function () {
            window.location.href = "../SanitationOnDemandLibrary/Index?page=Sanitation_Jobs_Sanitation_-_On_Demand";
        });
    }
});
//#endregion
//#region Common
function RedirectToSodlDetail(SanOnDemandMasterId, mode) {
    $.ajax({
        url: "/SanitationOnDemandLibrary/SanitOnDemandLibDetails",
        type: "POST",
        dataType: 'html',
        data: { sanOnDemandMasterId: SanOnDemandMasterId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#rendersanitationondemandLibrary').html(data);
        },
        complete: function () {
            CloseLoader();
            if (mode === "task") {
                $('#litask').trigger('click');
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
function RedirectToDetailOncancel(sanId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToSodlDetail(sanId, mode);
    });
}
$(document).on('click', '#brdondemand', function () {
    var sanitId = $(this).attr('data-val'); 
    RedirectToonDemandDetail(sanitId,"");
   
});
function RedirectToSureOncancel(masterId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToonDemandDetail(masterId, mode);
    });
}
function SetOnDemmandControl() {
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
//#endregion Common

$(document).on('click', '#sanitOnDemandLibSearchTable_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#sanitOnDemandLibSearchTable_length .searchdt-menu', function () {
    run = true;
});
$(document).on('click', '#sanitOnDemandLibSearchTable_wrapper th', function () {
    run = true;
});
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(sanitOnDemandLibSearchTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(sanitOnDemandLibSearchTable, colOrder);
    run = true;
    sanitOnDemandLibSearchTable.state.save(run);
});
//#endregion