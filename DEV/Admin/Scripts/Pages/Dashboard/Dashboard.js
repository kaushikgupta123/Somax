$(document).ready(function () {
    generateMainDataTableForAdmin();
    generateInvDataTableForAdmin();
    generatePurDataTableForAdmin();
});

//#region For Maintenance Grid
function generateMainDataTableForAdmin() {
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
            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/Dashboard/GetMetrics_Maintenance",
            data: { duration: duration },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [2, 3, 4],
                className: 'text-right'
            }
        ],
        "columns":
            [
                {
                    "data": "ClientName", "autoWidth": true, "bSearchable": true, "bSortable": false,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "SiteName", "autoWidth": true, "bSearchable": true, "bSortable": false,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "WorkOrdersCreated", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "WorkOrdersCompleted", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "LaborHours", "autoWidth": true, "bSearchable": true, "bSortable": false }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
//#endregion

//#region For Inventory Grid
function generateInvDataTableForAdmin() {
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
            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/Dashboard/GetMetrics_Inventory",
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [2, 3],
                className: 'text-right'
            }
        ],
        "columns":
            [
                {
                    "data": "ClientName", "autoWidth": true, "bSearchable": true, "bSortable": false,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "SiteName", "autoWidth": true, "bSearchable": true, "bSortable": false,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "Valuation", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "LowParts", "autoWidth": true, "bSearchable": true, "bSortable": false }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
//#endregion

//#region For Purchasing Grid
function generatePurDataTableForAdmin() {
    var duration = parseInt($(document).find('#PurchasingDropdown').val());
    if ($(document).find('#PurchasingTable').hasClass('dataTable')) {
        dtPurchasingTable.destroy();
    }
    dtPurchasingTable = $("#PurchasingTable").DataTable({
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
            url: "/Admin/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Admin/Dashboard/GetMetrics_Purchasing",
            data: { duration: duration },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [2, 3, 4],
                className: 'text-right'
            }
        ],
        "columns":
            [
                {
                    "data": "ClientName", "autoWidth": true, "bSearchable": true, "bSortable": false,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "SiteName", "autoWidth": true, "bSearchable": true, "bSortable": false,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "PurchaseOrdersCreated", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "PurchaseOrdersCompleted", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "ReceivedAmount", "autoWidth": true, "bSearchable": true, "bSortable": false }
            ],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(),
                // Total over all pages
                total = api.column(4).data().reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b);
                }, 0);
            // Update footer
            $(api.column(4).footer()).html(total.toFixed(2));
        },
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
//#endregion
$(document).on('change', '#PurchasingDropdown', function () {
    generatePurDataTableForAdmin();
});
$(document).on('change', '#MaintenanceDropdown', function () {
    generateMainDataTableForAdmin();
});