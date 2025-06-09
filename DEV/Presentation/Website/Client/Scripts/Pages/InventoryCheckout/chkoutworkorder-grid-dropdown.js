var dtWorkOrderTable;
var woClientLookupId = "";
var woDescription = "";
var woChargeTo = "";
var woWorkAssigned = "";
var woRequestor = "";
var woStatus = "";
function generateWorkOrderDataTable() {
     
    var rCount = 0;
    var visibility;
    if ($(document).find('#WorkOrderCHKTable').hasClass('dataTable')) {
        dtWorkOrderTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtWorkOrderTable = $("#WorkOrderCHKTable").DataTable({
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
            "url": "/Base/GetWorkOrderLookupListChunkSearch",
            data: function (d) {
                d.ClientLookupId = woClientLookupId;
                d.Description = woDescription;
                d.ChargeTo = woChargeTo;
                d.WorkAssigned = woWorkAssigned;
                d.Requestor = woRequestor;
                d.Status = woStatus;
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
                        return '<a class=link_CHKWorkOrder href="javascript:void(0)">' + data + '</a>'
                    }
                },
                {
                    "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                },
                {
                    "data": "ChargeTo", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
                {
                    "data": "WorkAssigned", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
                {
                    "data": "Requestor", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
            ],
        "rowCallback": function (row, data, index, full) {
        },
        initComplete: function () {
            $(document).find('#tblWorkOrderCHKfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#WorkOrderCHKModal').hasClass('show')) {
                $(document).find('#WorkOrderCHKModal').modal("show");
            }

            $(document).find('#WorkOrderCHKTable tfoot th').each(function (i, v) {
                var colIndex = i;
                if (colIndex !== 3 && colIndex !== 4) {
                    $(this).html('<input type="text" style="width:100%" class="chkwo tfootsearchtxt" id="chkwocolindex_' + colIndex + '"  /><i class="fa fa-search dropSearchIcon"></i>');
                    if (woClientLookupId) { $('#chkwocolindex_0').val(woClientLookupId); }
                    if (woDescription) { $('#chkwocolindex_1').val(woDescription); }
                    if (woChargeTo) { $('#chkwocolindex_2').val(woChargeTo); }
                    if (woWorkAssigned) { $('#chkwocolindex_3').val(woWorkAssigned); }
                    if (woRequestor) { $('#chkwocolindex_4').val(woRequestor); }
                    if (woStatus) { $('#chkwocolindex_5').val(woStatus); }
                }
                else {
                    $(this).html('');
                }
            });

            $('#WorkOrderCHKTable tfoot th').find('.chkwo').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    woClientLookupId = $('#chkwocolindex_0').val();
                    woDescription = $('#chkwocolindex_1').val();
                    woChargeTo = $('#chkwocolindex_2').val();
                    woWorkAssigned = $('#chkwocolindex_3').val();
                    woRequestor = $('#chkwocolindex_4').val();
                    woStatus = $('#chkwocolindex_5').val();
                    dtWorkOrderTable.page('first').draw('page');
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
$(document).on('click', '.link_CHKWorkOrder', function (e) {
    var index_row = $('#WorkOrderCHKTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtWorkOrderTable.row(row).data();
    if (typeof activeTab !== 'undefined' && activeTab == "#Returnpart")
    {
        $(document).find('#txtReturnChargeToId').val(data.ClientLookupId).removeClass('input-validation-error');
        $(document).find('#hdnReturnChargeToId').val(data.WorkOrderId);
    }
    else
    {
        $(document).find('#txtChargeToId').val(data.ClientLookupId).removeClass('input-validation-error');
        $(document).find('#hdnChargeToId').val(data.WorkOrderId);
    }
    
    $(document).find("#WorkOrderCHKModal").modal('hide');
});