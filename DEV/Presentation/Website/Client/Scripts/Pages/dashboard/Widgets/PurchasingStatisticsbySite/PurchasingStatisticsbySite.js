$(document).ready(function () {
    generatePurDataTableWidget();
});

$(document).on('change', '#PurchasingDropdown', function () {
    generatePurDataTableWidget();
});
function generatePurDataTableWidget() {
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