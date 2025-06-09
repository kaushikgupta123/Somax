var dtInvoiceVendorTable;
var InvClientLookupId = "";
var InvName = "";
$(document).on('click', '#invoicevendoropengrid', function () {
    generateVendorDataTable();
});
function generateVendorDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#InvoiceVendorsTable').hasClass('dataTable')) {
        dtInvoiceVendorTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtInvoiceVendorTable = $("#InvoiceVendorsTable").DataTable({
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
            "url": "/Base/GetVendorLookupListchunksearch",
            data: function (d) {
                d.ClientLookupId = InvClientLookupId;
                d.Name = InvName;
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
                        return '<a class=link_invoicevendor_detail href="javascript:void(0)">' + data + '</a>'
                    }
                },
                {
                    "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
                }
            ],
        initComplete: function () {
            $(document).find('#tblvendorfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#partModal').hasClass('show')) {
                $(document).find('#partModal').modal("show");
            }

            $('#InvoiceVendorsTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#InvoiceVendorsTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="invoicevcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (InvClientLookupId) { $('#invoicevcolindex_0').val(InvClientLookupId); }
                if (InvName) { $('#invoicevcolindex_1').val(InvName); }
            });


            $(document).ready(function () {
                $(window).keydown(function (event) {
                    if (event.keyCode === 13) {
                        event.preventDefault();
                        return false;
                    }
                });
                $('#InvoiceVendorsTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                    if (e.keyCode == 13) {
                        var thisId = $(this).attr('id');
                        var colIdx = thisId.split('_')[1];
                        var searchText = LRTrim($(this).val());
                        InvClientLookupId = $('#invoicevcolindex_0').val();
                        InvName = $('#invoicevcolindex_1').val();
                        dtInvoiceVendorTable.page('first').draw('page');
                    }
                });
            });
        }
    });
}

$(document).on('click', '.link_invoicevendor_detail', function (e) {
    var index_row = $('#InvoiceVendorsTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtInvoiceVendorTable.row(row).data();
    $(document).find('#InvoiceMatchHeaderModel_VendorClientLookupId').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#InvoiceMatchHeaderModel_VendorId').val(data.VendorID);
    if ($(document).find('#InvoiceMatchHeaderModel_VendorName').length != 0) {
        $(document).find('#InvoiceMatchHeaderModel_VendorName').val(data.Name).removeClass('input-validation-error');
    }
    $(document).find("#partModal").modal('hide');
    $(".InvoiceClearVendorModalPopupGridData").show();


});

$(document).on('click', '.InvoiceClearVendorModalPopupGridData', function () {
    $(document).find('#InvoiceMatchHeaderModel_VendorClientLookupId').val('');
    $(document).find('#InvoiceMatchHeaderModel_VendorId').val('');
    $(document).find('#InvoiceMatchHeaderModel_VendorName').val('');

    if ($(document).find('#InvoiceMatchHeaderModel_VendorName').css('display') == 'none') {
        $(document).find('#InvoiceMatchHeaderModel_VendorName').css('display', 'block');
    }
    if ($(document).find('#InvoiceMatchHeaderModel_VendorId').css('display') == 'none') {
        $(document).find('#InvoiceMatchHeaderModel_VendorId').css('display', 'block');
    }
    if ($(document).find('#InvoiceMatchHeaderModel_VendorClientLookupId').css('display') == 'none') {
        $(document).find('#InvoiceMatchHeaderModel_VendorClientLookupId').css('display', 'block');
    }
    $(this).css('display', 'none');
});