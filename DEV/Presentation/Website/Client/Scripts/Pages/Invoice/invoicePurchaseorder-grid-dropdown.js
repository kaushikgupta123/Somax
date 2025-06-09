var dtInvoicePurchaseOrderTable;
var InvPOClientLookupId = "";
var InvPOStatus = "";
var InvPOVendorClientLookupId = "";
var InvPOVendorName = "";
$(document).on('click', '#purchaseorderopengrid', function () {
    POClientLookupId = "";
    Status = "";
    VendorClientLookupId = "";
    VendorName = "";
    generatePurchaseOrderDataTable();
});
function generatePurchaseOrderDataTable() {
    if ($(document).find('#InvoicePurchaseOrdersTable').hasClass('dataTable')) {
        dtInvoicePurchaseOrderTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtInvoicePurchaseOrderTable = $("#InvoicePurchaseOrdersTable").DataTable({
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
            "url": "/Base/GetPurchaseOrderLookupListchunksearch",
            data: function (d) {
                d.POClientLookupId = InvPOClientLookupId;
                d.Status = InvPOStatus;
                d.VendorClientLookupId = InvPOVendorClientLookupId;
                d.VendorName = InvPOVendorName;
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
                    "data": "POClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=link_PurchaseOrder_detail href="javascript:void(0)">' + data + '</a>'
                    }
                },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true,
                },
                {
                    "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                },
                {
                    "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true,
                }
            ],
        initComplete: function () {
            $(document).find('#tblPurchaseOrderfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#purchasepartModal').hasClass('show')) {
                $(document).find('#purchasepartModal').modal("show");
            }



            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });

            $('#InvoicePurchaseOrdersTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#InvoicePurchaseOrdersTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="invoicepocolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (InvPOClientLookupId) { $('#invoicepocolindex_0').val(InvPOClientLookupId); }
                if (InvPOStatus) { $('#invoicepocolindex_1').val(InvPOStatus); }
                if (InvPOVendorClientLookupId) { $('#invoicepocolindex_2').val(InvPOVendorClientLookupId); }
                if (InvPOVendorName) { $('#invoicepocolindex_3').val(InvPOVendorName); }
            });
            $('#InvoicePurchaseOrdersTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    InvPOClientLookupId = $('#invoicepocolindex_0').val();
                    InvPOStatus = $('#invoicepocolindex_1').val();
                    InvPOVendorClientLookupId = $('#invoicepocolindex_2').val();
                    InvPOVendorName = $('#invoicepocolindex_3').val();
                    dtInvoicePurchaseOrderTable.page('first').draw('page');
                }
            });
        }
    });
}

$(document).on('click', '.link_PurchaseOrder_detail', function (e) {
    var index_row = $('#InvoicePurchaseOrdersTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtInvoicePurchaseOrderTable.row(row).data();

    $(document).find('#InvoiceMatchHeaderModel_POClientLookUpId').val(data.POClientLookupId).removeClass('input-validation-error');
    $(document).find('#InvoiceMatchHeaderModel_PurchaseOrderId').val(data.PurchaseOrderId);
    if ($(document).find('#InvoiceMatchHeaderModel_Status').length != 0) {
        $(document).find('#InvoiceMatchHeaderModel_Status').val(data.Status).removeClass('input-validation-error');
    }

    $(document).find("#purchasepartModal").modal('hide');
    $(".InvoiceClearPOModalPopupGridData").show();
});

$(document).on('click', '.InvoiceClearPOModalPopupGridData', function () {
    $(document).find('#InvoiceMatchHeaderModel_POClientLookUpId').val('');
    $(document).find('#InvoiceMatchHeaderModel_PurchaseOrderId').val('');

    if ($(document).find('#InvoiceMatchHeaderModel_PurchaseOrderId').css('display') == 'none') {
        $(document).find('#InvoiceMatchHeaderModel_PurchaseOrderId').css('display', 'block');
    }
    if ($(document).find('#InvoiceMatchHeaderModel_POClientLookUpId').css('display') == 'none') {
        $(document).find('#InvoiceMatchHeaderModel_POClientLookUpId').css('display', 'block');
    }
    $(this).css('display', 'none');
});