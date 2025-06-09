var dtAccountTable;
var accClientLookupId = "";
var accName = "";
function generateAccountDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#AccountPOTable').hasClass('dataTable')) {
        dtAccountTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtAccountTable = $("#AccountPOTable").DataTable({
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
                    return '<a class=link_POAccount href="javascript:void(0)">' + data + '</a>'
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
            $(document).find('#tblAccountPOfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#AccountPOModal').hasClass('show')) {
                $(document).find('#AccountPOModal').modal("show");
            }

            $(document).find('#AccountPOTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#AccountPOTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="poaccount tfootsearchtxt" id="poaccountcolindex_' + colIndex + '"  /><i class="fa fa-search dropSearchIcon"></i>');
                if (accClientLookupId) { $('#poaccountcolindex_0').val(accClientLookupId); }
                if (accName) { $('#poaccountcolindex_1').val(accName); }
            });

            $('#AccountPOTable tfoot th').find('.poaccount').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    accClientLookupId = $('#poaccountcolindex_0').val();
                    accName = $('#poaccountcolindex_1').val();
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
//$(document).on('click', '.link_POAccount', function (e) {   
//    var index_row = $('#AccountPOTable tr').index($(this).closest('tr')) - 1;
//    var row = $(this).parents('tr');
//    var td = $(this).parents('tr').find('td');
//    var data = dtAccountTable.row(row).data();
//    $(document).find('#txtChargeToId').val(data.ClientLookupId).removeClass('input-validation-error');
//    $(document).find('#hdnChargeToId').val(data.AccountId);
//    $(document).find("#AccountPOModal").modal('hide');
//});
$(document).on('click', '.link_POAccount', function (e) {
    var index_row = $('#AccountPOTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtAccountTable.row(row).data();
    $(document).find('#ChargeToClientLookupId').val(data.ClientLookupId).css("display", "block");
    $(document).find('#ChargeToId').val(data.AccountId).removeClass('input-validation-error').css("display", "none");
    $(document).find('#ChargeToId').parent().find('div > button.ClearChargeToModalPopupGridData').css('display', 'block');
    //$(document).find('#ChargeTo_Name').val(data.Name);
    $(document).find("#AccountPOModal").modal('hide');
});