var dtXrefPmTable;
var ClientLookupId = "";
var Description = "";

$(document).on('click', '#opengrid', function (e) {
    generatePmXrefDataTable();
});
function generatePmXrefDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#XrefPmTable').hasClass('dataTable')) {
        dtXrefPmTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtXrefPmTable = $("#XrefPmTable").DataTable({
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
            "url": "/EquipmentMaster/GetPmLookupList",
            data: function (d) {
                d.ClientLookupId = LRTrim(ClientLookupId);
                d.Description = LRTrim(Description);
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
                    return '<a class=link_xrefPM_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            }
        ],
        "rowCallback": function (row, data, index, full) {
        },
        initComplete: function () {
            $(document).find('#tblpmfooter').show();
            $(document).find('.dataTables_length').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#partModal').hasClass('show')) {
                $(document).find('#partModal').modal("show");
            }

            $('#XrefPmTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#XrefPmTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (ClientLookupId) { $('#colindex_0').val(ClientLookupId); }
                if (Description) { $('#colindex_1').val(Description); }
            });
            $('#XrefPmTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    ClientLookupId = $('#colindex_0').val();
                    Description = $('#colindex_1').val();
                    dtXrefPmTable.page('first').draw('page');
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
    $(document).on('click', '.link_xrefPM_detail', function (e) {
        e.preventDefault();
        var index_row = $('#XrefPmTable tr').index($(this).closest('tr')) - 1;
        var row = $(this).parents('tr');
        var td = $(this).parents('tr').find('td');
        var data = dtXrefPmTable.row(row).data();
        $(document).find('#txtpmid').val(data.ClientLookupId).removeClass('input-validation-error');
        $(document).find('#EquipmentMasterPmModel_ClientLookupId').val(data.ClientLookupId);
        $(document).find('#partModal').modal("hide");
    });
});
