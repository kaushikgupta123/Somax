var dtSAPTable;
var SensorAlertProcClientLookupId = "";
var SensorAlertProcDescription = "";
var SensorAlertProcType = "";
var TextField = "";
var ValueField = "";

$(document).on('click', '.OpenSAPModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    getSAProcDataTable();
});
$(document).on('click', '.ClearSAProcModalPopupGridData', function () {
    $(document).find('#' + $(this).data('textfield')).val('');
    $(document).find('#' + $(this).data('valuefield')).val('');

    if ($(document).find('#' + $(this).data('textfield')).css('display') == 'block') {
        $(document).find('#' + $(this).data('textfield')).css('display', 'none');
    }
    if ($(document).find('#' + $(this).data('valuefield')).css('display') == 'none') {
        $(document).find('#' + $(this).data('valuefield')).css('display', 'block');
    }
    $(this).css('display', 'none');
});
function getSAProcDataTable() {
    if ($(document).find('#tblSAProcModalPopup').hasClass('dataTable')) {
        dtSAPTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtSAPTable = $("#tblSAProcModalPopup").DataTable({
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
            "url": "/Base/GetActiveSensorProcedureAlertLookupListGridData",
            data: function (d) {
                d.ClientLookupId = SensorAlertProcClientLookupId;
                d.Description = SensorAlertProcDescription;
                d.Type = SensorAlertProcType;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                //rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
            [
                {
                    "data": "ClientLookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=link_SAProc_detail href="javascript:void(0)">' + data + '</a>'
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                },
                {
                    "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true,
                }
            ],
        initComplete: function () {
            $(document).find('#tblSAProcModalPopupFooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#SAProcTableModalPopup').hasClass('show')) {
                $(document).find('#SAProcTableModalPopup').modal("show");
            }
            $('#tblSAProcModalPopup tfoot th').each(function (i, v) {
                var colIndex = i;
                //var title = $('#tblSAProcModalPopup thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="SAProc_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (SensorAlertProcClientLookupId) { $('#SAProc_colindex_0').val(SensorAlertProcClientLookupId); }
                if (SensorAlertProcDescription) { $('#SAProc_colindex_1').val(SensorAlertProcDescription); }
                if (SensorAlertProcType) { $('#SAProc_colindex_2').val(SensorAlertProcType); }
            });
            $(document).ready(function () {
                $(window).keydown(function (event) {
                    if (event.keyCode === 13) {
                        event.preventDefault();
                        return false;
                    }
                });
                $('#tblSAProcModalPopup tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                    if (e.keyCode === 13) {
                        SensorAlertProcClientLookupId = $('#SAProc_colindex_0').val();
                        SensorAlertProcDescription = $('#SAProc_colindex_1').val();
                        SensorAlertProcType = $('#SAProc_colindex_2').val();
                        dtSAPTable.page('first').draw('page');
                    }
                });
            });
        }
    });
}

$(document).on('click', '.link_SAProc_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtSAPTable.row(row).data();
    $(document).find('#' + TextField).val(data.ClientLookUpId).removeClass('input-validation-error');
    $(document).find('#' + ValueField).val(data.SensorAlertProcedureId).removeClass('input-validation-error');
    $(document).find("#SAProcTableModalPopup").modal('hide');
    $(document).find('#' + ValueField).parent().find('div > button.ClearSAProcModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).css('display') == 'none') {
        $(document).find('#' + TextField).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).css('display') == 'block') {
        $(document).find('#' + ValueField).css('display', 'none');
    }
});

$(document).on('hidden.bs.modal', '#SAProcTableModalPopup', function () {
    TextField = "";
    ValueField = "";
});
//#region CriticalAlertProcedure
var dtCAPTable;
var CriticalAlertProcClientLookupId = "";
var CriticalAlertProcDescription = "";
var CriticalAlertProcType = "";
//var TextField = "";
//var ValueField = "";

$(document).on('click', '.OpenCriticalProcedureModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    getCAProcDataTable();
});
$(document).on('click', '.ClearCriticalProcedureModalPopupGridData', function () {
    $(document).find('#' + $(this).data('textfield')).val('');
    $(document).find('#' + $(this).data('valuefield')).val('');

    if ($(document).find('#' + $(this).data('textfield')).css('display') == 'block') {
        $(document).find('#' + $(this).data('textfield')).css('display', 'none');
    }
    if ($(document).find('#' + $(this).data('valuefield')).css('display') == 'none') {
        $(document).find('#' + $(this).data('valuefield')).css('display', 'block');
    }
    $(this).css('display', 'none');
});
function getCAProcDataTable() {
    if ($(document).find('#tblCAProcModalPopup').hasClass('dataTable')) {
        dtCAPTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtCAPTable = $("#tblCAProcModalPopup").DataTable({
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
            "url": "/Base/GetActiveSensorProcedureAlertLookupListGridData",
            data: function (d) {
                d.ClientLookupId = CriticalAlertProcClientLookupId;
                d.Description = CriticalAlertProcDescription;
                d.Type = CriticalAlertProcType;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                //rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
            [
                {
                    "data": "ClientLookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=link_CAProc_detail href="javascript:void(0)">' + data + '</a>'
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                },
                {
                    "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true,
                }
            ],
        initComplete: function () {
            $(document).find('#tblCAProcModalPopupFooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#CAProcTableModalPopup').hasClass('show')) {
                $(document).find('#CAProcTableModalPopup').modal("show");
            }
            $('#tblCAProcModalPopup tfoot th').each(function (i, v) {
                var colIndex = i;
                //var title = $('#tblCAProcModalPopup thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="CAProc_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (CriticalAlertProcClientLookupId) { $('#CAProc_colindex_0').val(CriticalAlertProcClientLookupId); }
                if (CriticalAlertProcDescription) { $('#CAProc_colindex_1').val(CriticalAlertProcDescription); }
                if (CriticalAlertProcType) { $('#CAProc_colindex_2').val(CriticalAlertProcType); }
            });
            $(document).ready(function () {
                $(window).keydown(function (event) {
                    if (event.keyCode === 13) {
                        event.preventDefault();
                        return false;
                    }
                });
                $('#tblCAProcModalPopup tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                    if (e.keyCode === 13) {
                        CriticalAlertProcClientLookupId = $('#CAProc_colindex_0').val();
                        CriticalAlertProcDescription = $('#CAProc_colindex_1').val();
                        CriticalAlertProcType = $('#CAProc_colindex_2').val();
                        dtCAPTable.page('first').draw('page');
                    }
                });
            });
        }
    });
}

$(document).on('click', '.link_CAProc_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtCAPTable.row(row).data();
    $(document).find('#' + TextField).val(data.ClientLookUpId).removeClass('input-validation-error');
    $(document).find('#' + ValueField).val(data.SensorAlertProcedureId).removeClass('input-validation-error');
    $(document).find("#CAProcTableModalPopup").modal('hide');
    $(document).find('#' + ValueField).parent().find('div > button.ClearCriticalProcedureModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).css('display') == 'none') {
        $(document).find('#' + TextField).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).css('display') == 'block') {
        $(document).find('#' + ValueField).css('display', 'none');
    }
});
$(document).on('hidden.bs.modal', '#CAProcTableModalPopup', function () {
    TextField = "";
    ValueField = "";
});
//#endregion