$(document).ready(function () {
    generateInvDataTableWidget();
});
function generateInvDataTableWidget() {
    var EquipmentId = 0;//$('#FleetAssetModel_EquID').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#InventoryTable').hasClass('dataTable')) {
        dtInventoryTable.destroy();
    }
    dtInventoryTable = $("#InventoryTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/DashBoard/GetMetrics_Inventory",
            //data: { EquipmentId: EquipmentId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                visibility = json.partSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        "columns":
            [

                { "data": "SiteName", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "Valuation", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "LowParts", "autoWidth": true, "bSearchable": true, "bSortable": false }


            ],
        initComplete: function () {

            SetPageLengthMenu();
        }
    });
}