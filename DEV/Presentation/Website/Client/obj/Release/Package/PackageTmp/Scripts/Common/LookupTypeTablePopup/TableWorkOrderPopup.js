var dtWorkOrderTable;
var woClientLookupId = "";
var woDescription = "";
var woChargeTo = "";
var woWorkAssigned = "";
var woRequestor = "";
var woStatus = "";
var TextField = "";
var ValueField = "";

$(document).on('click', '.OpenWorkOrderModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    getWorkOrderDataTable();
});
$(document).on('click', '.ClearWorkOrderModalPopupGridData', function () {
    $(document).find('#' + $(this).data('textfield')).val('');
    $(document).find('#' + $(this).data('valuefield')).val('');

    if ($(document).find('#' + $(this).data('textfield')).css('display') == 'block') {
        $(document).find('#' + $(this).data('textfield')).css('display', 'none');
    }
    if ($(document).find('#' + $(this).data('valuefield')).css('display') == 'none') {
        $(document).find('#' + $(this).data('valuefield')).css('display', 'block');
    }
    $(this).css('display', 'none');
});

function getWorkOrderDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#tblWorkOrderModalPopup').hasClass('dataTable')) {
        dtWorkOrderTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtWorkOrderTable = $("#tblWorkOrderModalPopup").DataTable({
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
            "url": "/Base/GetWorkOrderLookupList",
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
                        return '<a class=link_WorkOrder_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
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
            $(document).find('#tblWorkOrderModalPopupFooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#WorkOrderTableModalPopup').hasClass('show')) {
                $(document).find('#WorkOrderTableModalPopup').modal("show");
            }

            $(document).find('#tblWorkOrderModalPopup tfoot th').each(function (i, v) {
                var colIndex = i;
                if (colIndex !== 3 && colIndex !== 4) {
                    $(this).html('<input type="text" style="width:100%" class="prwo tfootsearchtxt" id="prwocolindex_' + colIndex + '"  /><i class="fa fa-search dropSearchIcon"></i>');
                    if (woClientLookupId) { $('#prwocolindex_0').val(woClientLookupId); }
                    if (woDescription) { $('#prwocolindex_1').val(woDescription); }
                    if (woChargeTo) { $('#prwocolindex_2').val(woChargeTo); }
                    if (woWorkAssigned) { $('#prwocolindex_3').val(woWorkAssigned); }
                    if (woRequestor) { $('#prwocolindex_4').val(woRequestor); }
                    if (woStatus) { $('#prwocolindex_5').val(woStatus); }

                }
                else {
                    $(this).html('');
                }
            });
            $(document).ready(function () {
                $(window).keydown(function (event) {
                    if (event.keyCode === 13) {
                        event.preventDefault();
                        return false;
                    }
                });
                $('#tblWorkOrderModalPopup tfoot th').find('.prwo').on("keyup", function (e) {
                    if (e.keyCode == 13) {
                        var thisId = $(this).attr('id');
                        var colIdx = thisId.split('_')[1];
                        var searchText = LRTrim($(this).val());
                        woClientLookupId = $('#prwocolindex_0').val();
                        woDescription = $('#prwocolindex_1').val();
                        woChargeTo = $('#prwocolindex_2').val();
                        woWorkAssigned = $('#prwocolindex_3').val();
                        woRequestor = $('#prwocolindex_4').val();
                        woStatus = $('#prwocolindex_5').val();
                        dtWorkOrderTable.page('first').draw('page');
                    }
                });
            });
        }
    });
}
//$(document).on('click', '.link_WorkOrder_detail', function (e) {
//    var index_row = $('#tblWorkOrderModalPopup tr').index($(this).closest('tr')) - 1;
//    var row = $(this).parents('tr');
//    var td = $(this).parents('tr').find('td');
//    var data = dtWorkOrderTable.row(row).data();
//    $(document).find('#txtChargeToId').val(data.ClientLookupId).removeClass('input-validation-error');
//    $(document).find('#hdnChargeToId').val(data.WorkOrderId);
//    $(document).find("#WorkOrderTableModalPopup").modal('hide');
//});
$(document).on('click', '.link_WorkOrder_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtWorkOrderTable.row(row).data();
    $(document).find('#' + TextField).val(data.ClientLookupId).removeClass('input-validation-error');
    $(document).find('#' + ValueField).val(data.WorkOrderId).removeClass('input-validation-error');
    $(document).find("#WorkOrderTableModalPopup").modal('hide');
    $(document).find('#' + ValueField).parent().find('div > button.ClearWorkOrderModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).css('display') == 'none') {
        $(document).find('#' + TextField).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).css('display') == 'block') {
        $(document).find('#' + ValueField).css('display', 'none');
    }
});

$(document).on('hidden.bs.modal', '#WorkOrderTableModalPopup', function () {
    TextField = "";
    ValueField = "";
});
