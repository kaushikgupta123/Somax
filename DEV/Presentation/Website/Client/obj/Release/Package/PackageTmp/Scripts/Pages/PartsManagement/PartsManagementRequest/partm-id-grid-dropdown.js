var dtXrefPartsTable;
var partmClientLookupId = "";
var LongDescription = "";


function generatePMDataTable(RequestType) {
    var rCount = 0;
    var visibility;
    if ($(document).find('#TblXrefParts').hasClass('dataTable')) {
        dtXrefPartsTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtXrefPartsTable = $("#TblXrefParts").DataTable({
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
            "url": "/Base/GetPartMasterLookupList",
            data: function (d) {
                d.ClientLookupId = partmClientLookupId;
                d.LongDescription = LongDescription;
                d.RequestType = RequestType;
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
                    return '<a class=link_partmast_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            {
                "data": "LongDescription", "autoWidth": true, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-400'>" + data + "</div>";
                }
            },
        ],
        initComplete: function () {
            $(document).find('#tblpartmastfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#partMasterModal').hasClass('show')) {
                $(document).find('#partMasterModal').modal("show");
            }
            $('#TblXrefParts tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#TblXrefParts thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (partmClientLookupId) { $('#colindex_0').val(partmClientLookupId);}
                if (LongDescription) { $('#colindex_1').val(LongDescription);}
            });
            $('#TblXrefParts tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    partmClientLookupId = $('#colindex_0').val();
                    LongDescription = $('#colindex_1').val();
                    dtXrefPartsTable.page('first').draw('page');
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
$(document).on('click', '.link_partmast_detail', function (e) {
    var index_row = $('#TblXrefParts tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtXrefPartsTable.row(row).data();
    //  $(document).find('#assignPartMastertoIndusnetBakeryModel_PartMaster_ClientLookupId').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('.mastidtxt').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find("#partMasterModal").modal('hide');
});