var dtAccountTable;
var gAccountClientLookupId = "";
var gAccountName = "";
var TextField = "";
var ValueField = "";

$(document).on('click', '.OpenAccountModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    generateAccountDataTable();
});
$(document).on('click', '.ClearAccountModalPopupGridData', function () {
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
function generateAccountDataTable() {
    if ($(document).find('#tblAccountModalPopup').hasClass('dataTable')) {
        dtAccountTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtAccountTable = $("#tblAccountModalPopup").DataTable({
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
            "url": "/Base/GetAccountLookupList",
            data: function (d) {
                d.ClientLookupId = gAccountClientLookupId;
                d.Name = gAccountName;
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
                        return '<a class=link_Account_detail href="javascript:void(0)">' + data + '</a>'
                    }
                },
                {
                    "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
                }
            ],
        initComplete: function () {
            $(document).find('#tblAccountModalPopupFooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#AccountTableModalPopup').hasClass('show')) {
                $(document).find('#AccountTableModalPopup').modal("show");
            }
            $('#tblAccountModalPopup tfoot th').each(function (i, v) {
                var colIndex = i;
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="account_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (gAccountClientLookupId) { $('#account_colindex_0').val(gAccountClientLookupId); }
                if (gAccountName) { $('#account_colindex_1').val(gAccountName); }
            });
            $(document).ready(function () {
                $(window).keydown(function (event) {
                    if (event.keyCode === 13) {
                        event.preventDefault();
                        return false;
                    }
                });
                $('#tblAccountModalPopup tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                    if (e.keyCode === 13) {
                        gAccountClientLookupId = $('#account_colindex_0').val();
                        gAccountName = $('#account_colindex_1').val();
                        dtAccountTable.page('first').draw('page');
                    }
                });
            });
        }
    });
}

$(document).on('click', '.link_Account_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtAccountTable.row(row).data();
    $(document).find('#' + TextField).val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#' + ValueField).val(data.AccountId).removeClass('input-validation-error');
    $(document).find("#AccountTableModalPopup").modal('hide');
    $(document).find('#' + ValueField).parent().find('div > button.ClearAccountModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).css('display') == 'none') {
        $(document).find('#' + TextField).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).css('display') == 'block') {
        $(document).find('#' + ValueField).css('display', 'none');
    }
});

$(document).on('hidden.bs.modal', '#AccountTableModalPopup', function () {
    TextField = "";
    ValueField = "";
});
