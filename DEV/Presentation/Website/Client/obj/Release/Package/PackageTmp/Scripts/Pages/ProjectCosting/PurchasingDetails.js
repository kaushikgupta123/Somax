var PurchasingDataTable;
var PrClientLookupId = "";
var PrLine = 0;
var PrPartID = "";
var PrDescription = "";
var PrQuantity = 0;
var PrUnitCost = 0;
var PrTotalCost = 0;
var PrStatus = "";
var PrBuyer = "";
var PrCompleteDate = "";
var rCount = 0;

function LoadPurchasingTab() {
    ProjectId = $(document).find('#projectCostingModel_ProjectId').val();
    $.ajax({
        url: '/ProjectCosting/ProjectCostingPurchasing',
        type: 'POST',
        data: {
            'ProjectId': ProjectId
        },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data) {
                $(document).find('#Purchasing').html(data);
            }
        },
        complete: function () {
            CloseLoader();
            generatePurchasingLookupTable();
        },
        error: function (err) {
            CloseLoader();
        }
    });

}
function generatePurchasingLookupTable() {
    if ($(document).find('#ProjectCostingPurchasingTable').hasClass('dataTable')) {
        PurchasingDataTable.destroy();
    }
    var ProjectId = $(document).find("#projectCostingModel_ProjectId").val();
    PurchasingDataTable = $("#ProjectCostingPurchasingTable").DataTable({
        colReorder: false,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "bProcessing": true,
        "order": [[0, "asc"]],
        stateSave: false,
        "pagingType": "full_numbers",
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/ProjectCosting/GetProjectCostingPurchesingSearch",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.ProjectId = ProjectId;
                d.clientLookupId = PrClientLookupId;
                d.Line = PrLine;
                d.PartID = PrPartID;
                d.Description = PrDescription;
                d.Quantity = PrQuantity;
                d.UnitCost = PrUnitCost;
                d.TotalCost = PrTotalCost;
                d.Status = PrStatus;
                d.Buyer = PrBuyer;
                d.CompleteDate = PrCompleteDate;
            },
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true
                },
                { "data": "Line", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": " text-right" },
                { "data": "PartID", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": " text-right" },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": " text-right" },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": " text-right" },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true
                },
                { "data": "Buyer", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],

        initComplete: function (settings, json) {

            $(document).find('#tbleqpfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();

            $('#ProjectCostingPurchasingTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#ProjectCostingPurchasingTable thead th').eq($(this).index()).text();
                if (i == 9) {
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt dtpicker_completeDate" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                } else {
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                }
                if (PrClientLookupId) { $('#colindex_0').val(PrClientLookupId); }
                if (PrLine) { $('#colindex_1').val(PrLine); }
                if (PrPartID) { $('#colindex_2').val(PrPartID); }
                if (PrDescription) { $('#colindex_3').val(PrDescription); }
                if (PrQuantity) { $('#colindex_4').val(PrQuantity); }
                if (PrUnitCost) { $('#colindex_5').val(PrUnitCost); }
                if (PrTotalCost) { $('#colindex_6').val(PrTotalCost); }
                if (PrStatus) { $('#colindex_7').val(PrStatus); }
                if (PrBuyer) { $('#colindex_8').val(PrBuyer); }
                if (PrCompleteDate) { $('#colindex_9').val(PrCompleteDate); }
                
            });

            $('#ProjectCostingPurchasingTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    PrClientLookupId = $('#colindex_0').val();
                    PrLine = $('#colindex_1').val();
                    PrPartID = $('#colindex_2').val();
                    PrDescription = $('#colindex_3').val();
                    PrQuantity = $('#colindex_4').val();
                    PrUnitCost = $('#colindex_5').val();
                    PrTotalCost = $('#colindex_6').val();
                    PrStatus = $('#colindex_7').val();
                    PrBuyer = $('#colindex_8').val();
                    PrCompleteDate = $('#colindex_9').val();
                    PurchasingDataTable.page('first').draw('page');
                }
            });

            $(document).find('.dtpicker_completeDate').datepicker({
                /*minDate: new Date(),*/
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            }).inputmask('mm/dd/yyyy');

        }
    });
}

$(window).keydown(function (event) {
    if (event.keyCode == 13) {
        event.preventDefault();
        return false;
    }
});








