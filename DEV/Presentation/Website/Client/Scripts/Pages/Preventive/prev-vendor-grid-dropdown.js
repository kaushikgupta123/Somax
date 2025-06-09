var dtPREVendorTable;
var VenClientLookupId = "";
var VenName = "";
function generatePreVendorDataTable() {
    var rCount = 0;   
    if ($(document).find('#PrevVendorsTable').hasClass('dataTable')) {
        dtPREVendorTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtPREVendorTable = $("#PrevVendorsTable").DataTable({
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
                    return '<a class=link_prevendor_detail href="javascript:void(0)">' + data + '</a>'
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
            $('#PrevVendorsTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#PrevVendorsTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="prvcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (VenClientLookupId) { $('#prvcolindex_0').val(VenClientLookupId); }
                if (VenName) { $('#prvcolindex_1').val(VenName); }
            });
            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });
            $('#PrevVendorsTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    VenClientLookupId = $('#prvcolindex_0').val();
                    VenName = $('#prvcolindex_1').val();
                    dtPREVendorTable.page('first').draw('page');
                }
            });
        }
    });
}

$(document).on('click', '.link_prevendor_detail', function (e) {
    var index_row = $('#PrevVendorsTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtPREVendorTable.row(row).data();   
    $(document).find('#txtVendorId').val(data.ClientLookupId);
    $(document).find('#hdnVendorId').val(data.VendorID);
    $(document).find("#partModal").modal('hide');
});