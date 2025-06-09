var dtPMAPersonnelTable;
var PersonnelClientLookupId = "";
var PersonnelNameFirst = "";
var PersonnelNameLast = "";
var TextField = "";
var ValueField = "";

$(document).on('click', '.ClearPMPersonnelModalGridData', function () {
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

$(document).on('click', ".OpenPMAssignPersonnelModalGrid", function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    generatePrePersonnelDataTable()
});

function generatePrePersonnelDataTable() {
    var rCount = 0;    
    if ($(document).find('#PrevAssignPersonnelTable').hasClass('dataTable')) {
        dtPMAPersonnelTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtPMAPersonnelTable = $("#PrevAssignPersonnelTable").DataTable({
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
            "url": "/PreventiveMaintenance/GetPMAssignPersonnelLookupListGridData",
            data: function (d) {
                d.ClientLookupId = PersonnelClientLookupId;
                d.NameFirst = PersonnelNameFirst;
                d.NameLast = PersonnelNameLast;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
            [
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=link_pmassignpersonnel_detail href="javascript:void(0)">' + data + '</a>'
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
            $(document).find('#tblpmpersonnelfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#pmpersonnelModal').hasClass('show')) {
                $(document).find('#pmpersonnelModal').modal("show");
            }
            $('#PrevAssignPersonnelTable tfoot th').each(function (i, v) {
                var colIndex = i;
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="Personnel_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (PersonnelClientLookupId) { $('#Personnel_colindex_0').val(PersonnelClientLookupId); }
                if (PersonnelNameFirst) { $('#Personnel_colindex_1').val(PersonnelNameFirst); }
                if (PersonnelNameLast) { $('#Personnel_colindex_2').val(PersonnelNameLast); }
            });
            $(document).ready(function () {
            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });
            $('#PrevAssignPersonnelTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    PersonnelClientLookupId = $('#Personnel_colindex_0').val();
                    PersonnelNameFirst = $('#Personnel_colindex_1').val();
                    PersonnelNameLast = $('#Personnel_colindex_2').val();
                    dtPMAPersonnelTable.page('first').draw('page');
                }
            });
            });
        }
    });
}

$(document).on('click', '.link_pmassignpersonnel_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtPMAPersonnelTable.row(row).data();   
    $(document).find('#' + TextField).val(data.NameFirst + ' ' + data.NameLast).removeClass('input-validation-error');
    $(document).find('#' + ValueField).val(data.PersonnelId);
    $(document).find("#pmpersonnelModal").modal('hide');
    $(document).find('#' + ValueField).parent().find('div > button.ClearPMPersonnelModalGridData').css('display', 'block');
    if ($(document).find('#' + TextField).css('display') == 'none') {
        $(document).find('#' + TextField).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).css('display') == 'block') {
        $(document).find('#' + ValueField).css('display', 'none');
    }
});