var dtXmfPartsTable;
var mnfClientLookupId = "";
var mnfName = "";
function generatePMManufacDataTable(RequestType) {
    var rCount = 0;
    var visibility;
    if ($(document).find('#TblXrefPartsMf').hasClass('dataTable')) {
        dtXmfPartsTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtXmfPartsTable = $("#TblXrefPartsMf").DataTable({
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
            "url": "/PartMaster/GetPartManufacturerLookupList",
            data: function (d) {
                d.ClientLookupId = mnfClientLookupId;
                d.Name = mnfName;
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
                    return '<a class=link_partmf_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
        ],
        initComplete: function () {
            $(document).find('#tblpartmfacfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#partManufacturerModal').hasClass('show')) {
                $(document).find('#partManufacturerModal').modal("show");
            }
            $('#TblXrefPartsMf tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#TblXrefPartsMf thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="mfcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (mnfClientLookupId) { $('#mfcolindex_0').val(mnfClientLookupId); }
                if (mnfName) { $('#mfcolindex_1').val(mnfName); }
            });
            $('#TblXrefPartsMf tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    mnfClientLookupId = $('#mfcolindex_0').val();
                    mnfName = $('#mfcolindex_1').val();
                    dtXmfPartsTable.page('first').draw('page');
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
$(document).on('click', '.link_partmf_detail', function (e) {
    var index_row = $('#TblXrefPartsMf tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtXmfPartsTable.row(row).data();
    $(document).find('.mfnametxt').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find("#partManufacturerModal").modal('hide');
});