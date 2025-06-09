$("#sidebar").mCustomScrollbar({
    theme: "minimal"
});
$('#dismiss, .overlay').on('click', function () {
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
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
$(document).ready(function () {
    $(".actionBar").fadeIn();
    $("#SensorAlertGridAction :input").attr("disabled", "disabled");
});
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "Tasks":
            GenerateSaTaskGrid();
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
$(function () {
    SetSaControl();
    generateSensorAlertTable();
});
//#region Search
var sensorAlertSearchTable;
var typeVal;

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
function generateSensorAlertTable() {
    if ($(document).find('#sensoralertSearch').hasClass('dataTable')) {
        sensorAlertSearchTable.destroy();
    }
    sensorAlertSearchTable = $("#sensoralertSearch").DataTable({
        rowGrouping: true,
        colReorder: true,
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
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'SensorAlert List'
            },
            {
                extend: 'print',
                title: 'SensorAlert List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'SensorAlert List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'SensorAlert List',
                orientation: 'landscape',
                pageSize: 'A3'
            }

        ],
        "orderMulti": true,
        "ajax": {
            "url": "/SensorAlert/GetSensorAlertGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.searchText = LRTrim($('#txtSAsearchbox').val());
                d.clientLookupId = LRTrim($("#txtSADemandId").val());
                d.description = LRTrim($("#txtSADescription").val());
                d.type = LRTrim($("#ddlSaType").val());
                d.createDate = $('#txtSACreated').val();
            },
            "dataSrc": function (result) {
                if (result.data.length < 1) {
                    $(document).find('.import-export').prop('disabled', true);
                }
                else {
                    $(document).find('.import-export').prop('disabled', false);
                }
                $("#ddlSaType").empty();
                $("#ddlSaType").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.typeList.length; i++) {
                    var id = result.typeList[i];
                    var name = result.typeList[i];
                    $("#ddlSaType").append("<option value='" + id + "'>" + name + "</option>");
                }
                if (typeVal) {
                    $("#ddlSaType").val(typeVal);
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
                        return '<a class=lnk_sensoralert href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
            ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#SensorAlertGridAction :input").removeAttr("disabled");
            $("#SensorAlertGridAction :button").removeClass("disabled");
            DisableExportButton($("#sensoralertSearch"), $(document).find('.import-export'));
        }
    });
};
//#endregion Search
//#region AdvSearch
var hGridfilteritemcount = 0;
$(document).on('click', "#btnSensorAlertAdvSrch", function (e) {
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#txtSAsearchbox').val('');
    typeVal = $("#ddlSaType").val();
    $('#advsearchsidebarSA').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossSA" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    run = true;
    sensorAlertSearchTable.page('first').draw('page');
    $('.filteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '.btnCrossSA', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    if (searchtxtId == "ddlSaType") {
        typeVal = null;
    }
    $(this).parent().remove();
    hGridfilteritemcount--;
    GridAdvanceSearch();
});
$(document).on('click', '#liClearAdvSearchFilterSA', function () {
    run = true;
    $("#txtSAsearchbox").val("");
    clearAdvanceSearch();
    sensorAlertSearchTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebarSA').find('input:text').val('');
    $(document).find('#txtSADescription').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
    $("#ddlSaType").val("").trigger('change');
    typeVal = $("#ddlSaType").val();
}
//#endregion AdvSearch
//#region Print
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var searchText = LRTrim($('#txtSAsearchbox').val());
            var clientLookupId = LRTrim($("#txtSADemandId").val());
            var description = LRTrim($("#txtSADescription").val());
            var type = LRTrim($("#ddlSaType").val());
            var createDate = $('#txtSACreated').val();
            //dtTable = $("#sensoralertSearch").DataTable();
            var currestsortedcolumn = $('#sensoralertSearch').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#sensoralertSearch').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#sensoralertSearch').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/SensorAlert/GetSensorAlertPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    clientLookupId: clientLookupId,
                    description: description,
                    type: type,
                    createDate: createDate,
                    searchText: searchText,
                    colname: colname,
                    coldir: coldir
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#sensoralertSearch thead tr th").map(function (key) {
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
                if (item.Type != null) {
                    item.Type = item.Type;
                }
                else {
                    item.Type = "";
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
                header: $("#sensoralertSearch thead tr th").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
})

//#endregion Print
//#region Details
$(document).on('click', '.lnk_sensoralert', function (e) {
    e.preventDefault();
    var index_row = $('#sensoralertSearch tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = sensorAlertSearchTable.row(row).data();
    var sensorAlertProcedureId = data.SensorAlertProcedureId;
    $.ajax({
        url: "/SensorAlert/SensorAlertDetails",
        type: "GET",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { sensorAlertProcedureId: sensorAlertProcedureId },
        success: function (data) {
            $('#renderSensorAlert').html(data);
        },
        complete: function (data) {

            CloseLoader();
        },
        error: function (xhr, error) {
        }
    });
});
$(document).on('click', ".addsensoralert", function (e) {
    $.ajax({
        url: "/SensorAlert/AddOrEditSensorAlert",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSensorAlert').html(data);
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
$(document).on('click', "#editSensorAlert", function (e) {
    e.preventDefault();
    var saId = $('#sensorAlertModel_SensorAlertProcedureId').val();
    $.ajax({
        url: "/SensorAlert/AddOrEditSensorAlert",
        type: "GET",
        dataType: 'html',
        data: { sensorAlertProcedureId: saId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSensorAlert').html(data);
        },
        complete: function () {
            SetSaControl();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function AddEditSAOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command === "save") {
            var message;
            if (data.mode === "add") {
                SuccessAlertSetting.text = getResourceValue("SensorAlertAdd");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("SensorAlertUpdate");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToSaDetail(data.SensorAlertProcedureId, "overview");
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
$(document).on('click', "#btnsacancel", function () {
    var saId = $('#sensorAlertModel_SensorAlertProcedureId').val();
    if (typeof saId != "undefined" && saId != 0) {
        RedirectToDetailOncancel(saId);
    }
    else {
        swal(CancelAlertSetting, function () {
            window.location.href = "../SensorAlert/Index?page=Sensors_Alert_Procedures";
        });
    }
});
//#endregion Details
//#region Common
function RedirectToDetailOncancel(saId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToSaDetail(saId, mode);
    });
}
function RedirectToSaDetail(saId, mode) {
    $.ajax({
        url: "/SensorAlert/SensorAlertDetails",
        type: "GET",
        dataType: 'html',
        data: { sensorAlertProcedureId: saId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderSensorAlert').html(data);
        },
        complete: function () {
            CloseLoader();
            if (mode === "task") {
                $(document).find('#litask').trigger('click');
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SetSaControl() {
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
$(document).on('click', '#brdsa', function () {
    var saId = $(this).attr('data-val');
    RedirectToSaDetail(saId);
});
function RedirectToSureOncancel(saId, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToSaDetail(saId, mode);
    });
}
//#endregion Common
//#region liCustomize1
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(sensorAlertSearchTable, true);
});
var colOrder = [0];
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(sensorAlertSearchTable, colOrder);
    run = true;
    sensorAlertSearchTable.state.save(run);
});
//#endregion