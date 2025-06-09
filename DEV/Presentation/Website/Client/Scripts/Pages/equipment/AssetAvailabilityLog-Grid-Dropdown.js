var dtAssetAvailabilityTable;
var AstTransactionDate = "";
var AstEvent = "";
var AstReturnToService = "";
var AstReason = "";
var AstReasonCode = "";
var AstPersonnelName = "";
//var PrtManufacturerId = "";
//var PrtStockType = "";
$(document).on('click', '#btnAstViewLog', function () {
    generateAssetLogDataTable();
});
function generateAssetLogDataTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#AssetLogTable').hasClass('dataTable')) {
        dtAssetAvailabilityTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtAssetAvailabilityTable = $("#AssetLogTable").DataTable({
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
            "url": "/Base/AssetAvailabilityLogLookupList",
            data: function (d) {
                d.ObjectId = EquipmentId;
                d.TransactionDate = AstTransactionDate;
                d.Event = AstEvent;
                d.ReturnToService = AstReturnToService;
                d.Reason = AstReason;
                d.ReasonCode = AstReasonCode;
                d.PersonnelName = AstPersonnelName;
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
                    "data": "TransactionDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date",
                    "mRender": function (data, type, row) {
                        return "<div>" + data + "</div>";
                    }
                },
                {
                    "data": "Event", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return '<div class="m - badge--custom">' + data + '</div>';
                    }
                },
                {
                    "data": "ReturnToService", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
                {
                    "data": "ReasonCode", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                },
                {
                    "data": "Reason", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                {
                    "data": "PersonnelName", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<span class="m-badge--custom">' + data + '</span>';
                    }
                }
                
            ],

        initComplete: function () {
            $(document).find('#tblAstLogfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#AstAvailabilityModal').hasClass('show')) {
                $(document).find('#AstAvailabilityModal').modal("show");
            }
            $(document).find('#AssetLogTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#AssetLogTable thead th').eq($(this).index()).text();
                if (colIndex == 0 || colIndex == 2) {
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt dtpicker" id="eqp_part_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                }
                else {
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="eqp_part_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                }
                if (AstTransactionDate) { $('#eqp_part_colindex_0').val(AstTransactionDate); }
                if (AstEvent) { $('#eqp_part_colindex_1').val(AstEvent); }
                if (AstReturnToService) { $('#eqp_part_colindex_2').val(AstReturnToService); }
                if (AstReasonCode) { $('#eqp_part_colindex_3').val(AstReasonCode); }
                if (AstReason) { $('#eqp_part_colindex_4').val(AstReason); }
                if (AstPersonnelName) { $('#eqp_part_colindex_5').val(AstPersonnelName); }

            });
            $(document).find('.dtpicker').datepicker({
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                onSelect: function () {
                    this.focus();
                }
                //onClose: function () {
                //    this.focus();
                //}
            }).inputmask('mm/dd/yyyy');

            $('#AssetLogTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    AstTransactionDate = $('#eqp_part_colindex_0').val();
                    AstEvent = $('#eqp_part_colindex_1').val();
                    AstReturnToService = $('#eqp_part_colindex_2').val();
                    AstReasonCode = $('#eqp_part_colindex_3').val();
                    AstReason = $('#eqp_part_colindex_4').val();
                    AstPersonnelName = $('#eqp_part_colindex_5').val();
                    dtAssetAvailabilityTable.page('first').draw('page');
                    this.blur();
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
