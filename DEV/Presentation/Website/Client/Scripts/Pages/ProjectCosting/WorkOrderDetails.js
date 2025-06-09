var WoWorkplanPopupTable;
var WoClientLookupId = "";
var WoDescription = "";
var WoStatus = "";
var WoPlanner = "";
var WoCompleteDate = "";
var WoMaterialCost = 0;
var WoLaborCost = 0;
var WoTotalCost = 0;
var rCount = 0;

function LoadWorkOrdersTab() {
    ProjectId = $(document).find('#projectCostingModel_ProjectId').val();
    $.ajax({
        url: '/ProjectCosting/ProjectCostingWorkOrder',
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
                $(document).find('#WorkOrders').html(data);
            }
        },
        complete: function () {
            CloseLoader();
            generateWoWorkOrderPlanLookupTable();

        },
        error: function (err) {
            CloseLoader();
        }
    });

}
function generateWoWorkOrderPlanLookupTable() {
    if ($(document).find('#ProjectCostingWorkOrderLookupTable').hasClass('dataTable')) {
        WoWorkplanPopupTable.destroy();
    }
    var ProjectId = $(document).find("#projectCostingModel_ProjectId").val();
    WoWorkplanPopupTable = $("#ProjectCostingWorkOrderLookupTable").DataTable({
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
            "url": "/ProjectCosting/GetProjectCostingWorkOrderSearch",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.ProjectId = ProjectId;
                d.clientLookupId = WoClientLookupId;
                d.Description = WoDescription;
                d.Status = WoStatus;
                d.Planner = WoPlanner;
                d.CompleteDate = WoCompleteDate;
                d.MaterialCost = WoMaterialCost;
                d.LaborCost = WoLaborCost;
                d.TotalCost = WoTotalCost;
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
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true
                },
                { "data": "Planner", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "MaterialCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right"},
                { "data": "LaborCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right", },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right", }
            ],

        initComplete: function (settings, json) {

            $(document).find('#tbleqpfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();

            $('#ProjectCostingWorkOrderLookupTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#ProjectCostingWorkOrderLookupTable thead th').eq($(this).index()).text();
                if (i == 4) {
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt dtpicker_completeDate" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                } else {
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                }
                if (WoClientLookupId) { $('#colindex_0').val(WoClientLookupId); }
                if (WoDescription) { $('#colindex_1').val(WoDescription); }
                if (WoStatus) { $('#colindex_2').val(WoStatus); }
                if (WoPlanner) { $('#colindex_3').val(WoPlanner); }
                if (WoCompleteDate) { $('#colindex_4').val(WoCompleteDate); }
                if (WoMaterialCost) { $('#colindex_5').val(WoMaterialCost); }
                if (WoLaborCost) { $('#colindex_6').val(WoLaborCost); }
                if (WoTotalCost) { $('#colindex_7').val(WoTotalCost); }
            });

            $('#ProjectCostingWorkOrderLookupTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    WoClientLookupId = $('#colindex_0').val();
                    WoDescription = $('#colindex_1').val();
                    WoStatus = $('#colindex_2').val();
                    WoPlanner = $('#colindex_3').val();
                    WoCompleteDate = $('#colindex_4').val();
                    WoMaterialCost = $('#colindex_5').val();
                    WoLaborCost = $('#colindex_6').val();
                    WoTotalCost = $('#colindex_7').val();
                    WoWorkplanPopupTable.page('first').draw('page');
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