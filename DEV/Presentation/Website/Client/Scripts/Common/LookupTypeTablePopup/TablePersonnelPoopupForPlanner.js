var dtPersonnelTable;
var gPersonnelClientLookupId = "";
var gPersonnelNameFirst = "";
var gPersonnelNameLast = "";
var TextField = "";
var ValueField = "";

$(document).on('click', '.OpenPlannerPersonnelModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    getPersonnelDataTable();
});
$(document).on('click', '.ClearPlannerPersonnelModalPopupGridData', function () {
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
function getPersonnelDataTable() {
    if ($(document).find('#tblPersonnelModalPopup').hasClass('dataTable')) {
        dtPersonnelTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtPersonnelTable = $("#tblPersonnelModalPopup").DataTable({
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
            "url": "/Base/GetActiveAdminOrFullPlannerPersonnelLookupListGridData",
            data: function (d) {
                d.ClientLookupId = gPersonnelClientLookupId;
                d.NameFirst = gPersonnelNameFirst;
                d.NameLast = gPersonnelNameLast;
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
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=link_Personnel_detail href="javascript:void(0)">' + data + '</a>'
                    }
                },
                {
                    "data": "NameFirst", "autoWidth": true, "bSearchable": true, "bSortable": true,
                },
                {
                    "data": "NameLast", "autoWidth": true, "bSearchable": true, "bSortable": true,
                }
            ],
        initComplete: function () {
            $(document).find('#tblPersonnelModalPopupFooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#PersonnelTableModalPopup').hasClass('show')) {
                $(document).find('#PersonnelTableModalPopup').modal("show");
            }
            $('#tblPersonnelModalPopup tfoot th').each(function (i, v) {
                var colIndex = i;
                //var title = $('#tblPersonnelModalPopup thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="Personnel_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (gPersonnelClientLookupId) { $('#Personnel_colindex_0').val(gPersonnelClientLookupId); }
                if (gPersonnelNameFirst) { $('#Personnel_colindex_1').val(gPersonnelNameFirst); }
                if (gPersonnelNameLast) { $('#Personnel_colindex_2').val(gPersonnelNameLast); }
            });
            $(document).ready(function () {
                $(window).keydown(function (event) {
                    if (event.keyCode === 13) {
                        event.preventDefault();
                        return false;
                    }
                });
                $('#tblPersonnelModalPopup tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                    if (e.keyCode === 13) {
                        gPersonnelClientLookupId = $('#Personnel_colindex_0').val();
                        gPersonnelNameFirst = $('#Personnel_colindex_1').val();
                        gPersonnelNameLast = $('#Personnel_colindex_2').val();
                        dtPersonnelTable.page('first').draw('page');
                    }
                });
            });
        }
    });
}

$(document).on('click', '.link_Personnel_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtPersonnelTable.row(row).data();
    $(document).find('#' + TextField).val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#' + ValueField).val(data.PersonnelId).removeClass('input-validation-error');
    $(document).find("#PersonnelTableModalPopup").modal('hide');
    $(document).find('#' + ValueField).parent().find('div > button.ClearPlannerPersonnelModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).css('display') == 'none') {
        $(document).find('#' + TextField).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).css('display') == 'block') {
        $(document).find('#' + ValueField).css('display', 'none');
    }
});

$(document).on('hidden.bs.modal', '#PersonnelTableModalPopup', function () {
    TextField = "";
    ValueField = "";
});
