var dtXreplPartsTable;
var partreplClientLookupId = "";
var Description = "";


function generatePMReplDataTable(RequestType) {
    var rCount = 0;
    var visibility;
    if ($(document).find('#TblReplParts').hasClass('dataTable')) {
        dtXreplPartsTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtXreplPartsTable = $("#TblReplParts").DataTable({
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
            "url": "/Base/GetPartReplaceLookupList",
            data: function (d) {
                d.ClientLookupId = partreplClientLookupId;
                d.Description = Description;
                d.RequestType = RequestType;//$(document).find('#replacePartModal_RequestType').val();
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
                    return '<a class=link_partrepl_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-300'>" + data + "</div>";
                }
            },
        ],
        initComplete: function () {
            $(document).find('#tblpartreplfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#partReplModal').hasClass('show')) {
                $(document).find('#partReplModal').modal("show");
            }
            $('#TblReplParts tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#TblReplParts thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="replcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (partreplClientLookupId) { $('#replcolindex_0').val(partreplClientLookupId); }
                if (Description) { $('#replcolindex_1').val(Description); }
            });
            $('#TblReplParts tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    partreplClientLookupId = $('#replcolindex_0').val();
                    Description = $('#replcolindex_1').val();
                    dtXreplPartsTable.page('first').draw('page');
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
$(document).on('click', '.link_partrepl_detail', function (e) {
    var index_row = $('#TblReplParts tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtXreplPartsTable.row(row).data();
    //$(document).find('#replacePartModal_ReplaceWith').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('.repltxt').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find("#partReplModal").modal('hide');
});