var dtVendorTable;
var gVendorClientLookupId = "";
var gVendorName = "";
var TextField = "";
var ValueField = "";

$(document).on('click', '.OpenVendorModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    generateVendorDataTable();
});
$(document).on('click', '.ClearVendorModalPopupGridData', function () {
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

function generateVendorDataTable() {
    if ($(document).find('#tblVendorModalPopup').hasClass('dataTable')) {
        dtVendorTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtVendorTable = $("#tblVendorModalPopup").DataTable({
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
            "url": "/Base/GetVendorLookupList",
            data: function (d) {
                d.ClientLookupId = gVendorClientLookupId;
                d.Name = gVendorName;
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
                        return '<a class=link_vendor_detail href="javascript:void(0)">' + data + '</a>'
                    }
                },
                {
                    "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
                }
            ],
        initComplete: function () {
            $(document).find('#tblVendorModalPopupFooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#VendorTableModalPopup').hasClass('show')) {
                $(document).find('#VendorTableModalPopup').modal("show");
            }
            $('#tblVendorModalPopup tfoot th').each(function (i, v) {
                var colIndex = i;
                //var title = $('#tblVendorModalPopup thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="vendor_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (gVendorClientLookupId) { $('#vendor_colindex_0').val(gVendorClientLookupId); }
                if (gVendorName) { $('#vendor_colindex_1').val(gVendorName); }
            });
            $(document).ready(function () {
                $(window).keydown(function (event) {
                    if (event.keyCode === 13) {
                        event.preventDefault();
                        return false;
                    }
                });
                $('#tblVendorModalPopup tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                    if (e.keyCode === 13) {
                        gVendorClientLookupId = $('#vendor_colindex_0').val();
                        gVendorName = $('#vendor_colindex_1').val();
                        dtVendorTable.page('first').draw('page');
                    }
                });
            });
        }
    });
}


$(document).on('click', '.link_vendor_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtVendorTable.row(row).data();
    $(document).find('#' + TextField).val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#' + ValueField).val(data.VendorID).removeClass('input-validation-error');
    $(document).find("#VendorTableModalPopup").modal('hide');    
    $(document).find('#' + ValueField).parent().find('div > button.ClearVendorModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).css('display')=='none') {
        $(document).find('#' + TextField).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).css('display') == 'block') {
        $(document).find('#' + ValueField).css('display', 'none');
    }
});

$(document).on('hidden.bs.modal', '#VendorTableModalPopup', function () {
    TextField = "";
    ValueField = "";
});
