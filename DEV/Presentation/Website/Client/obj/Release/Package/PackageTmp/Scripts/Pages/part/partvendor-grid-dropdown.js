var dtVendorTable;
var VenClientLookupId = "";
var VenName = "";
$(document).on('click', '#opengrid', function () {
    generateVendorDataTable();
});
function generateVendorDataTable() {
    // var EquipmentId = $('#partsSessionData_EquipmentId').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#VendorsTable').hasClass('dataTable')) {
        dtVendorTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtVendorTable = $("#VendorsTable").DataTable({
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
                    return '<a class=link_partvendor_detail href="javascript:void(0)">' + data + '</a>'
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
            $('#VendorsTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#VendorsTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (VenClientLookupId) { $('#colindex_0').val(VenClientLookupId); }
                if (VenName) { $('#colindex_1').val(VenName); }

            });

            $('#VendorsTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    VenClientLookupId = $('#colindex_0').val();
                    VenName = $('#colindex_1').val();
                    dtVendorTable.page('first').draw('page');
                }
            });
        }
    });
}

$(document).ready(function () {
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });

});
$(document).on('click', '.link_partvendor_detail', function (e) {
    var index_row = $('#VendorsTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtVendorTable.row(row).data();
    // $(document).find('#txtvendorid').val(data.ClientLookupId).removeClass('input-validation-error');  
    $(document).find('#partsVendorModel_VendorClientLookupId').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#vendorSessionData_Part').val(data.ClientLookUpId);
    $(document).find("#partModal").modal('hide');
});