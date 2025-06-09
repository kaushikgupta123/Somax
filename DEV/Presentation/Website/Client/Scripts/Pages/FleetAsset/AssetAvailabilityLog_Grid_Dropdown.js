var dtXrefAssetAvailabilityTable;
var AstTransactionDate = "";
var AstEvent = "";
var AstReturnToService = "";
var AstReason = "";
//var PrtManufacturerId = "";
//var PrtStockType = "";
$(document).on('click', '#btnViewLog', function () {
    
    generateAstLogXrefDataTable();
});
function generateAstLogXrefDataTable() {
    var EquipmentId = $('#FleetAssetModel_EquID').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#XrefAstLogTable').hasClass('dataTable')) {
        dtXrefAssetAvailabilityTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtXrefAssetAvailabilityTable = $("#XrefAstLogTable").DataTable({
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
                    "data": "Reason", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
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
            $(document).find('#XrefAstLogTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#XrefAstLogTable thead th').eq($(this).index()).text();
                if (colIndex == 0 || colIndex== 2){
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt dtpicker" id="eqp_part_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
            }
                else{
            $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="eqp_part_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
        }
                if (AstTransactionDate) { $('#eqp_part_colindex_0').val(AstTransactionDate); }
                if (AstEvent) { $('#eqp_part_colindex_1').val(AstEvent); }
                if (AstReturnToService) { $('#eqp_part_colindex_2').val(AstReturnToService); }
                if (AstReason) { $('#eqp_part_colindex_3').val(AstReason); }
                
            });
            $(document).find('.dtpicker').datepicker({
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                //onSelect: function () {
                //    this.focus();
                //}
                 onClose: function () {
                    this.focus();
                }
            }).inputmask('mm/dd/yyyy');
            
            $('#XrefAstLogTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    AstTransactionDate = $('#eqp_part_colindex_0').val();
                    AstEvent = $('#eqp_part_colindex_1').val();
                    AstReturnToService = $('#eqp_part_colindex_2').val();
                    AstReason = $('#eqp_part_colindex_3').val();
                    $(document).find('.dtpicker').blur();
                    dtXrefAssetAvailabilityTable.page('first').draw('page');
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
//$(document).on('click', '.link_xrefpart_detail', function (e) {
//    var index_row = $('#XrefPartsTable tr').index($(this).closest('tr')) - 1;
//    var row = $(this).parents('tr');
//    var td = $(this).parents('tr').find('td');
//    var data = dtXrefPartsTable.row(row).data();
//    $(document).find('#txtpartid').val(data.ClientLookUpId).removeClass('input-validation-error');
//    $(document).find('#partsSessionData_Part').val(data.ClientLookUpId);
//    $(document).find("#partModal").modal('hide');
//});

