var dtMeterTable;
var gMeterClientLookupId = "";
var gMeterName = "";
var TextField = "";
var ValueField = "";

$(document).on('click', '.OpenMeterModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    getMeterDataTable();
});
$(document).on('click', '.ClearMeterModalPopupGridData', function () {
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
function getMeterDataTable() {
    if ($(document).find('#tblMeterModalPopup').hasClass('dataTable')) {
        dtMeterTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtMeterTable = $("#tblMeterModalPopup").DataTable({
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
            "url": "/Base/GetMeterLookupListGridData",
            data: function (d) {
                d.ClientLookupId = gMeterClientLookupId;
                d.Name = gMeterName;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {               
                return json.data;
            }
        },
        "columns":
            [
                {
                    "data": "MeterClientLookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=link_Meter_detail href="javascript:void(0)">' + data + '</a>'
                    }
                },
                {
                    "data": "MeterName", "autoWidth": true, "bSearchable": true, "bSortable": true,
                }
            ],
        initComplete: function () {
            $(document).find('#tblMeterModalPopupFooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#MeterTableModalPopup').hasClass('show')) {
                $(document).find('#MeterTableModalPopup').modal("show");
            }
            $('#tblMeterModalPopup tfoot th').each(function (i, v) {
                var colIndex = i;                
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="Meter_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (gMeterClientLookupId) { $('#Meter_colindex_0').val(gMeterClientLookupId); }
                if (gMeterName) { $('#Meter_colindex_1').val(gMeterName); }
            });
            $(document).ready(function () {
                $(window).keydown(function (event) {
                    if (event.keyCode === 13) {
                        event.preventDefault();
                        return false;
                    }
                });
                $('#tblMeterModalPopup tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                    if (e.keyCode === 13) {
                        gMeterClientLookupId = $('#Meter_colindex_0').val();
                        gMeterName = $('#Meter_colindex_1').val();
                        dtMeterTable.page('first').draw('page');
                    }
                });
            });
        }
    });
}

$(document).on('click', '.link_Meter_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtMeterTable.row(row).data();
    $(document).find('#' + TextField).val(data.MeterClientLookUpId).removeClass('input-validation-error');
    $(document).find('#' + ValueField).val(data.MeterId).removeClass('input-validation-error');
    $(document).find("#MeterTableModalPopup").modal('hide');
    $(document).find('#' + ValueField).parent().find('div > button.ClearMeterModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).css('display') == 'none') {
        $(document).find('#' + TextField).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).css('display') == 'block') {
        $(document).find('#' + ValueField).css('display', 'none');
    }
});

$(document).on('hidden.bs.modal', '#MeterTableModalPopup', function () {
    TextField = "";
    ValueField = "";
});
