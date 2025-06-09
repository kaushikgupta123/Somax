var dtAccountTable;
var accClientLookupId = "";
var accName = "";
function generateAccountDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#AccountPRTable').hasClass('dataTable')) {
        dtAccountTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtAccountTable = $("#AccountPRTable").DataTable({
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
            "url": "/Base/GetAccountLookupList",
            data: function (d) {
                d.ClientLookupId = accClientLookupId;
                d.Name = accName;
               
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
                    return '<a class=link_PRAccount href="javascript:void(0)">' + data + '</a>'
                }
            },
          
            {
                "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<span class="m-badge--custom">' + data + '</span>'
                }
            },
        ],
        "rowCallback": function (row, data, index, full) {
        },
        initComplete: function () {
            $(document).find('#tblAccountPRfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#AccountPRModal').hasClass('show')) {
                $(document).find('#AccountPRModal').modal("show");
            }

            $(document).find('#AccountPRTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#AccountPRTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="praccount tfootsearchtxt" id="praccountcolindex_' + colIndex + '"  /><i class="fa fa-search dropSearchIcon"></i>');
                if (accClientLookupId) { $('#praccountcolindex_0').val(accClientLookupId); }
                if (accName) { $('#praccountcolindex_1').val(accName); }

            });

            $('#AccountPRTable tfoot th').find('.praccount').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    accClientLookupId = $('#praccountcolindex_0').val();
                    accName = $('#praccountcolindex_1').val();
                    dtAccountTable.page('first').draw('page');
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
$(document).on('click', '.link_PRAccount', function (e) {
    var index_row = $('#AccountPRTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtAccountTable.row(row).data();
    $(document).find('#txtChargeToId').val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#hdnChargeToId').val(data.AccountId); //---change ID
    $(document).find("#AccountPRModal").modal('hide');
});