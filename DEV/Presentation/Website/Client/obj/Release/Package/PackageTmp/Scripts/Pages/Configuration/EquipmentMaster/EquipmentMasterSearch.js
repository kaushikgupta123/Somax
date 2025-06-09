var run = false;
//#region Common
var activeStatus = false;
$(document).on('click', '#emsidebarCollapse', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(function () {
    ShowbtnLoaderclass("LoaderDrop");
    $(".actionBar").fadeIn();
    $("#equipmentGridAction :input").attr("disabled", "disabled");


    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    //#region Load Grid With Status
    var displayState = localStorage.getItem("EQUIPMENTMASTERGRIDDISPLAYSTATUS");
    if (displayState) {
        if (displayState == "false") {
            $('#equipDropdown').val("1").trigger('change.select2');
        }
        else {
            $('#equipDropdown').val("2").trigger('change.select2');
        }
        activeStatus = displayState;
    }
    else {
        activeStatus = false;
    }
    generateEqpMasterTable();
    //#endregion
});
function SetEqpMasterControl() {
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
function RedirectToEMDetail(EquipmentMasterId, mode) {
    if (EquipmentMasterId == 0) {
        window.location.href = "/EquipmentMaster/index?page=Masters_Assets";
    }
    else {
        $.ajax({
            url: "/EquipmentMaster/EquipmentMasterDetails",
            type: "GET",
            dataType: "html",
            data: { equipmentMasterId: EquipmentMasterId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#renderequipmentmaster').html(data);
            },
            complete: function () {
                CloseLoader();
                if (mode == "Preventive") {
                    $('#PM').trigger('click');
                    $('#colorselector').val('Preventive');
                }
                SetEqpMasterControl();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
}
function RedirectToSureOncancel(MasterId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToEMDetail(MasterId, mode);
    });
}
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "Preventive":
            GeneratePMGrid();
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
//#endregion Common
//#region Search
$(document).on('change', '#equipDropdown', function () {
    ShowbtnLoaderclass("LoaderDrop");
    run = true;
    var optionVal = $(document).find("#equipDropdown").val();
    if (optionVal == "1") {
        searchcount = 0;
        localStorage.setItem("EQUIPMENTMASTERGRIDDISPLAYSTATUS", false);
        activeStatus = false;
       // eqpMasterTable.page('first').draw('page');
    }
    else {
        localStorage.setItem("EQUIPMENTMASTERGRIDDISPLAYSTATUS", true);
        activeStatus = true;
       // eqpMasterTable.page('first').draw('page');
    }
    eqpMasterTable.page('first').draw('page');
});
var eqpMasterTable;
function generateEqpMasterTable() {
    var printCounter = 0;
    if ($(document).find('#euipmentMasterSearch').hasClass('dataTable')) {
        eqpMasterTable.destroy();
    }
    eqpMasterTable = $("#euipmentMasterSearch").DataTable({
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
            if (run == true) {
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "EquipmentMaster_Search",
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
                    GridName: "EquipmentMaster_Search",
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
                title: 'Asset Master'
            },
            {
                extend: 'print',
                title: 'Asset Master'
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Asset Master',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                css: 'display:none',
                title: 'Asset Master',
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/EquipmentMaster/GetEquipmentMasterGrid",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.name = LRTrim($('#Name').val());
                d.make = LRTrim($("#Make").val());
                d.model = LRTrim($("#Model").val());
                d.type = LRTrim($('#Type').val());
                d.inactiveFlag = activeStatus;
            },
            "dataSrc": function (result) {   
                HidebtnLoaderclass("LoaderDrop");
                if (result.data.length == "0") {
                    $(document).find('.import-export').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }

                return result.data;
            },
            global: true
        },
        "columns":
        [
            {
                "data": "Name",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "className": "text-left",
                "name": "0",
                "mRender": function (data, type, row) {
                    return '<a class=lnk_eqpmaster href="javascript:void(0)">' + data + '</a>';
                }
            },
            { "data": "Make", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
            { "data": "Model", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
            { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" }
        ],
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#equipmentGridAction :input").not('.import-export').removeAttr("disabled");
            $("#equipmentGridAction :button").not('.import-export').removeClass("disabled");
        }
    });
};

$(document).on('click', '.lnk_eqpmaster', function (e) {
    e.preventDefault();
    // var index_row = $('#euipmentMasterSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    //  var td = $(this).parents('tr').find('td');
    var data = eqpMasterTable.row(row).data();
    //  var EquipmentMasterId = data.EquipmentMasterId;
    $.ajax({
        url: "/EquipmentMaster/EquipmentMasterDetails",
        type: "GET",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { equipmentMasterId: data.EquipmentMasterId },
        success: function (data) {
            $('#renderequipmentmaster').html(data);
        },
        complete: function (data) {
            CloseLoader();
        },
        error: function (xhr, error) {
        }
    });
});
//#endregion Search
//#region AdvSearch
var hGridfilteritemcount = 0;
$(document).on('click', "#btnemAdvSrch", function (e) {
   // eqpMasterTable.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val() && $(this).val() != "0") {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossSODL" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    run =true;
   // generateEqpMasterTable();
    eqpMasterTable.page('first').draw('page');
    $('.filteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '.btnCrossSODL', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    GridAdvanceSearch();
});
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    clearAdvanceSearch();
    eqpMasterTable.page('first').draw('page');
   // generateEqpMasterTable();
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion AdvSearch
//#region Print
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var name = LRTrim($('#Name').val());
            var make = LRTrim($("#Make").val());
            var model = LRTrim($("#Model").val());
            var type = $('#Type').val();
            var inactiveFlag = false;
            var optionVal = $(document).find("#equipDropdown").val();
            if (optionVal == "1") {
                inactiveFlag = false;
            }
            else {
                inactiveFlag = true;
            }
            dtTable = $("#euipmentMasterSearch").DataTable();
            var currestsortedcolumn = $('#euipmentMasterSearch').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#euipmentMasterSearch').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#euipmentMasterSearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/EquipmentMaster/GetEqpMasterPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    name: name,
                    make: make,
                    model: model,
                    type: type,
                    inactiveFlag: inactiveFlag,
                    colname: colname,
                    coldir: coldir
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#euipmentMasterSearch thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.Name != null) {
                    item.Name = item.Name;
                }
                else {
                    item.Name = "";
                }
                if (item.Make != null) {
                    item.Make = item.Make;
                }
                else {
                    item.Make = "";
                }
                if (item.Model != null) {
                    item.Model = item.Model;
                }
                else {
                    item.Model = "";
                }
                if (item.Type != null) {
                    item.Type = item.Type;
                }
                else {
                    item.Type = "";
                }
                if (item.InactiveFlag != null) {
                    item.InactiveFlag = item.InactiveFlag;
                }
                else {
                    item.InactiveFlag = "";
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
                header: $("#euipmentMasterSearch thead tr th").map(function (key) {
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
//#region Add_Edit Eqp Master
$(document).on('click', ".AddEqpMaster", function () {
    $.ajax({
        url: "/EquipmentMaster/AddEquipmentMaster",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderequipmentmaster').html(data);
        },
        complete: function () {
            SetEqpMasterControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function EqpMasterAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.Command == "save") {
            if (data.mode == "add") {
                SuccessAlertSetting.text = getResourceValue("EquipmentMasterAddAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("EquipmentMasterUpdateAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToEMDetail(data.EquipmentMasterId, "");
            });
        }
        else {
            message = getResourceValue("EquipmentMasterAddAlert");
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                $(document).find('form').find("#purchaseRequestModel_VendorId").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};
$(document).on('click', "#btnCancelEqpMaster,#brdEMdetail", function () {
    var MasterId = $(document).find('#EquipmentMasterModel_EquipmentMasterId').val();
    if (!MasterId) {
        MasterId = 0;// $(document).find('#masterSanitationModel_MasterIdForCancel').val();
    }
    RedirectToEMDetail(MasterId, "");
});
$(document).on('click', "#btnEditEqMaster", function () {
    var EquipmentMasterId = $(document).find('#EquipmentMasterModel_EquipmentMasterId').val();
    var Name = $(document).find('#EquipmentMasterModel_Name').val();
    var Type = $(document).find('#EquipmentMasterModel_Type').val();
    var Model = $(document).find('#EquipmentMasterModel_Model').val();
    var Make = $(document).find('#EquipmentMasterModel_Make').val();
    var InactiveFlag = $(document).find('#EquipmentMasterModel_InactiveFlag').val();
    $.ajax({
        url: "/EquipmentMaster/EditEquipmentMaster",
        type: "GET",
        dataType: 'html',
        data: { equipmentMasterId: EquipmentMasterId, name: Name, type: Type, model: Model, make: Make, inactiveFlag: InactiveFlag },
        // data: { equipmentMasterId: EquipmentMasterId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderequipmentmaster').html(data);
        },
        complete: function () {
            SetEqpMasterControl();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion Add_Edit Eqp Master
$(document).on('click', '#euipmentMasterSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#euipmentMasterSearch_length .searchdt-menu', function () {
    run = true;
});
$(document).on('click', '#euipmentMasterSearch_wrapper th', function () {
    run = true;
});


//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(eqpMasterTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(eqpMasterTable, colOrder);
    run = true;
    eqpMasterTable.state.save(run);
});
//#endregion
