var dtXrefMmTable;
var ClientLookupId = "";
var Name = "";

$(document).on('click', '#opengrid', function (e) {
    generateMmXrefDataTable(); 
});
function generateMmXrefDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#XrefMmTable').hasClass('dataTable')) {
        dtXrefMmTable.destroy(); 
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtXrefMmTable = $("#XrefMmTable").DataTable({
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
                d.ClientLookupId = LRTrim(ClientLookupId);
                d.Name = LRTrim(Name);
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
                    return '<a class=link_xrefMM_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            {
                "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-400'>" + data + "</div>";
                }
            }
        ],
        "rowCallback": function (row, data, index, full) {
        },
        initComplete: function () {
            $(document).find('#tblmmfooter').show();
            $(document).find('.dataTables_length').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#mpModal').hasClass('show')) {
                $(document).find('#mpModal').modal("show");
                $(document).find("#tbldropdown").addClass('show');
            }
            $('#XrefMmTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#XrefMmTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (ClientLookupId) {
                    $('#colindex_0').val(ClientLookupId);
                }
                if (Name) {
                    $('#colindex_1').val(Name);
                }
            });
            $('#XrefMmTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    ClientLookupId = $('#colindex_0').val();
                    Name = $('#colindex_1').val();
                    dtXrefMmTable.page('first').draw('page');
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
    $(document).on('click', '.link_xrefMM_detail', function (e) {
        e.preventDefault();
        var index_row = $('#XrefMmTable tr').index($(this).closest('tr')) - 1;
        var row = $(this).parents('tr');
        var td = $(this).parents('tr').find('td');
        var data = dtXrefMmTable.row(row).data();
        $(document).find('#txtmmid').val(data.ClientLookupId).removeClass('input-validation-error');
        $(document).find('#PartMasterModel_Manufacturer').val(data.ClientLookupId);
       // $(document).find("#tbldropdown").removeClass('show');
        $(document).find('#mpModal').modal("hide");
    });
});

