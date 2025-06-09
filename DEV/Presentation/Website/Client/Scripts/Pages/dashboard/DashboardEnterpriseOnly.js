var colorArray = ['#fe0000', '#ff7f00', '#fffe01', '#00bd3f', '#0068ff', '#7a01e6', '#d300c9', '#940100', '#066d7c', '#66cbff'];

$(function () {
   
    var OpenWorkOrderHrBarChart;
    var OverDuePms;
    Chart.defaults.global.defaultFontColor = '#575962';
    Chart.defaults.global.defaultFontFamily = "Roboto";
    var pDropdownDef = 3;
    
    OpenWorkOrderHrBar_db();
    OverduePMsBar_db();
    generateMainDataTable();
    generateInvDataTable();
    generatePurDataTable();
});

//#region Open Work Order by Site
function OpenWorkOrderHrBar_db() {
    var count = 0;
    if (typeof OpenWorkOrderHrBarChart !== "undefined") {
        OpenWorkOrderHrBarChart.destroy();
    }
    var flag = "OpenWorkOrder";
    $.ajax({
        type: "GET",
        url: "/DashBoard/EnterpriseUserBarChart?flag=" + flag,
        dataType: "JSON",
        async: true,
        success: function (data) {
            count = data.length;
            if (data.length == 0) {
                $('#m_chart_OpenWorkorder_Hr').prev().show();
                return;
            }
            else {
                $('#m_chart_OpenWorkorder_Hr').prev().hide();
            }
            var chartKey = [], chartVal = [];
            $.each(data, function (index, item) {
                chartKey.push(item.Site);
                chartVal.push(item.Value);
            });
            var c = document.getElementById("m_chart_OpenWorkorder_Hr");
            var ctx = c.getContext("2d");
            OpenWorkOrderHrBarChart = new Chart(ctx, {
                type: 'horizontalBar',
                data: {
                    labels: chartKey,
                    datasets: [{
                        label: '',
                        data: chartVal,
                        //backgroundColor: 'rgba(255,99,132,1)',
                        backgroundColor: colorArray.slice(0, count),
                        borderWidth: 1
                    }
                    ]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        display: false
                    },
                    tooltips: {
                        mode: 'nearest',
                        intersect: false,
                        position: 'nearest',
                        xPadding: 10,
                        yPadding: 10,
                        caretPadding: 10,
                        titleFontSize: 12,
                        bodyFontSize: 10,
                        titleFontStyle: 'normal'
                    },
                    scales: {
                        xAxes: [{
                            ticks: {
                                beginAtZero: false,
                                steps: 100,
                                stepValue: 500,
                                fontSize: 9,
                                fontColor: "#575962",
                                fontStyle: "bold"
                            },
                            scaleLabel: {
                                display: true,
                                labelString: "Value",
                                fontSize: 12,
                            }
                        }
                        ],
                        yAxes: [{
                            barThickness: 15,
                            ticks: {
                                beginAtZero: true,
                                fontSize: 9,
                                fontStyle: "bold"
                            },
                            scaleLabel: {
                                display: true,
                                labelString: "Site",
                                fontSize: 12,
                                padding: 5
                            }
                        },
                        ]
                    }
                }
            });
        },
        complete: function () {
            $('#openworkorderchartenterprise').hide();
        }
    });
}
//#endregion

//#region Overdue PMs by Site
function OverduePMsBar_db() {
    var count = 0;
    if (typeof OverDuePms !== "undefined") {
        OverDuePms.destroy();
    }
    var flag = "OverDuePMs";
    $.ajax({
        type: "GET",
        url: "/DashBoard/EnterpriseUserBarChart?flag=" + flag,
        dataType: "JSON",
        async: true,
        success: function (data) {
            count = data.length;
            if (data.length == 0) {
                $('#m_chart_OverDueWorkorder_Hr').prev().show();
                return;
            }
            else {
                $('#m_chart_OverDueWorkorder_Hr').prev().hide();
            }
            var chartKey = [], chartVal = [];
            $.each(data, function (index, item) {
                chartKey.push(item.Site);
                chartVal.push(item.Value);
            });
            var c = document.getElementById("m_chart_OverDueWorkorder_Hr");
            var ctx = c.getContext("2d");
            OverDuePms = new Chart(ctx, {
                type: 'horizontalBar',
                data: {
                    labels: chartKey,
                    datasets: [{
                        label: '',
                        data: chartVal,
                        backgroundColor: colorArray.slice(0, count),
                        borderWidth: 1
                    }
                    ]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        display: false
                    },
                    tooltips: {
                        mode: 'nearest',
                        intersect: false,
                        position: 'nearest',
                        xPadding: 10,
                        yPadding: 10,
                        caretPadding: 10,
                        titleFontSize: 12,
                        bodyFontSize: 10,
                        titleFontStyle: 'normal'
                    },
                    scales: {
                        xAxes: [{
                            ticks: {
                                beginAtZero: false,
                                steps: 100,
                                stepValue: 500,
                                fontSize: 9,
                                fontColor: "#575962",
                                fontStyle: "bold"
                            },
                            scaleLabel: {
                                display: true,
                                labelString: "Value",
                                fontSize: 12,
                            }
                        }
                        ],
                        yAxes: [{
                            barThickness: 15,
                            ticks: {
                                beginAtZero: true,
                                fontSize: 9,
                                fontStyle: "bold"
                            },
                            scaleLabel: {
                                display: true,
                                labelString: "Site",
                                fontSize: 12,
                                padding: 5
                            }
                        },
                        ]
                    }
                }
            });
        },
        complete: function () {
            $('#overduepmschartloader').hide();
        }
    });
}
//#endregion

//#region For Maintenance Grid
function generateMainDataTable() {
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
//#endregion

//#region For Inventory Grid
function generateInvDataTable() {
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
//#endregion

//#region For Purchasing Grid
function generatePurDataTable() {
    var EquipmentId = 0;//$('#FleetAssetModel_EquID').val();
    var rCount = 0;
    var visibility;
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
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/DashBoard/GetMetrics_Purchasing",
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
                targets: [3],
                className: 'noVis text-right'
                
            }
        ],
        "columns":
            [

                { "data": "SiteName", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "PurchaseOrdersCreated", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "PurchaseOrdersCompleted", "autoWidth": true, "bSearchable": true, "bSortable": false },
                { "data": "ReceivedAmount", "autoWidth": true, "bSearchable": true, "bSortable": false }


            ],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(),
                // Total over all pages
                total = api.column(3).data().reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b);
                }, 0);
            // Update footer
            $(api.column(3).footer()).html(total.toFixed(2));
        },
        initComplete: function () {

            SetPageLengthMenu();
        }
    });
}
//#endregion
$(document).on('change', '#PurchasingDropdown', function () {
    generatePurDataTable();
});
$(document).on('change', '#MaintenanceDropdown', function () {
    generateMainDataTable();
});

