$(document).ready(function () {
    generateMainDataTableWidget();
});
$(document).on('change', '#MaintenanceDropdown', function () {
    generateMainDataTableWidget();
});
function generateMainDataTableWidget() {
    var EquipmentId = 0;//$('#FleetAssetModel_EquID').val();
    var rCount = 0;
    var visibility;
    var duration = parseInt($(document).find('#MaintenanceDropdown').val());
    if ($(document).find('#MaintenanceTable').hasClass('dataTable')) {
        dtMaintenanceTable.destroy();
    }
    dtMaintenanceTable = $("#MaintenanceTable").DataTable({
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
            "url": "/DashBoard/GetMetrics_Maintenance",
            data: { duration: duration },
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
                { "data": "WorkOrdersCreated", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "WorkOrdersCompleted", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "LaborHours", "autoWidth": true, "bSearchable": true, "bSortable": false }

            ],
        initComplete: function () {

            SetPageLengthMenu();
        }
    });
}