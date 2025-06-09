var dtPOVendorTable;
var VenClientLookupId = "";
var VenName = "";
$(document).on('click', '#povendoropengrid', function () {
    ClientLookupId = "";
    Name = "";
    generateVendorDataTable();
});
function generateVendorDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#POVendorsTable').hasClass('dataTable')) {
        dtPOVendorTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtPOVendorTable = $("#POVendorsTable").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        "pageLength": 10,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            url:'/Base/GetVendorLookupListchunksearch',
            data: function (d) {
                d.ClientLookupId = VenClientLookupId;
                d.Name = VenName;
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
                    return '<a class=link_povendor_detail href="javascript:void(0)">' + data + '</a>'
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

            $('#POVendorsTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#POVendorsTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="povcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (VenClientLookupId) { $('#povcolindex_0').val(VenClientLookupId); }
                if (VenName) { $('#povcolindex_1').val(VenName); }
            });

            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });
            $('#POVendorsTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    VenClientLookupId = $('#povcolindex_0').val();
                    VenName = $('#povcolindex_1').val();
                    dtPOVendorTable.page('first').draw('page');
                }
            });
        }
    });
}

$(document).on('click', '.link_povendor_detail', function (e) {
    var index_row = $('#POVendorsTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtPOVendorTable.row(row).data();
    $(document).find('#PurchaseOrderModel_VendorClientLookupId').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#PurchaseOrderModel_VendorId').val(data.VendorID);
    $(document).find('#PurchaseOrderModel_VendorName').val(data.Name);
    $(document).find("#partModal").modal('hide');
});