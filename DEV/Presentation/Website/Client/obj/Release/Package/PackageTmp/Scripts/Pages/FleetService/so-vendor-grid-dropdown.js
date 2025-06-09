var dtWoVendorTable;
var VendorClientLookupId = "";
var VendorName = "";




function generateSoVendorDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#WoVendorsTable').hasClass('dataTable')) {
        dtWoVendorTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtWoVendorTable = $("#WoVendorsTable").DataTable({
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
                d.ClientLookupId = VendorClientLookupId;
                d.Name = VendorName;
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
                    return '<a class=link_wovendor_detail href="javascript:void(0)">' + data + '</a>'
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
            if (!$(document).find('#SO_VendorModal').hasClass('show')) {
                $(document).find('#SO_VendorModal').modal("show");
            }
            $('#WoVendorsTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#WoVendorsTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="wocolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (VendorClientLookupId) { $('#wocolindex_0').val(VendorClientLookupId); }
                if (VendorName) { $('#wocolindex_1').val(VendorName); }

            });
            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });
            $('#WoVendorsTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    VendorClientLookupId = $('#wocolindex_0').val();
                    VendorName = $('#wocolindex_1').val();
                    dtWoVendorTable.page('first').draw('page');
                }
            });
        }
    });
}
$(document).on('click', '.link_wovendor_detail', function (e) {
    var index_row = $('#WoVendorsTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtWoVendorTable.row(row).data();
    $(document).find('#txtSoVendorId').val(data.ClientLookupId);
    $(document).find('#hdnSoVendorId').val(data.VendorID);
    $(document).find("#SO_VendorModal").modal('hide');
});