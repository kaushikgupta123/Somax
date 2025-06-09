$(function () {
    SetFunsionChartGlobalSettings();
});

function LoadDashboardTab() {
    ProjectId = $(document).find('#projectCostingModel_ProjectId').val();
    $.ajax({
        url: '/ProjectCosting/ProjectCostingDashboard',
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
                $(document).find('#Dashboard').html(data);
            }
        },
        complete: function () {
            CloseLoader();
            GenerateProjectCostGrid();
            GenerateProjectCostingWorkOrderStatuses();
        },
        error: function (err) {
            CloseLoader();
        }
    });

}

function GenerateProjectCostingWorkOrderStatuses() {
    ProjectId = $(document).find('#projectCostingModel_ProjectId').val();
    $('.ScheduleComplienceCountLoader').show();
    $.ajax({
        type: "POST",
        url: '/ProjectCosting/ProjectCostingWorkOrderStatuses',
        data: {
            ProjectId: ProjectId
        },
        dataType: "json",
        success: function (tData) {
            var statusCount = tData.StatusCount;
            if (statusCount.length > 0) {
                for (var i = 0; i < statusCount.length; i++) {
                    if (statusCount[i].Key === "Complete") { $("#WorkOrderCompleteCount").html(statusCount[i].Value); continue; }
                    if (statusCount[i].Key === "InComplete") { $("#WorkOrderInCompleteCount").html(statusCount[i].Value); continue; }
                    if (statusCount[i].Key === "Total") { $("#WorkOrderTotalCount").html(statusCount[i].Value); continue; }
                }
            }
            PlotScheduleComplienceChart(tData.ChartData);
        },
        complete: function () {
            $('.ScheduleComplienceCountLoader').hide();
        }
    });
}

function PlotScheduleComplienceChart(data) {
    $('.ScheduleComplienceLoader').show();
    const chartConfigs = {
        renderAt: "WorkOrderScheduleComplience",
        type: "doughnut2d",
        width: "100%",
        height: "100%",
        dataFormat: "json",
        dataSource: {
            "chart": {
                "theme": "fusion",
                "showLabels": "0",
                "toolTipBgColor": "#000000",
                "toolTipColor": "#FFFFFF",
                "plotToolText": "$label  <br> $dataValue",
                "tooltipborderradius": "4",
                "centerLabel": "$value",
                "numberSuffix": "%",
                "showPercentValues": "0",
                "showTooltip": "0",
            },
            "data": data
        }
    }
    new FusionCharts(chartConfigs).render();
    $('.ScheduleComplienceLoader').hide();
}

function GenerateProjectCostGrid() {
    ProjectId = $(document).find('#projectCostingModel_ProjectId').val();
    $('.SpendingAndRemainingLoader').show();
    $.ajax({
        type: "POST",
        url: "/ProjectCosting/ProjectCostingDashboardGrid",
        data: {
            ProjectId: ProjectId
        },
        dataType: "json",
        success: function (data) {
            PlotSpendingAndRemainingChart(data.spendingChartData);
            PlotGridData(data);
        },
        complete: function () {

        }

    });
}

function PlotSpendingAndRemainingChart(data) {
    $('.SpendingAndRemainingLoader').show();
    const chartConfigs = {
        renderAt: "ProjectSpendingAndRemaining",
        type: "doughnut2d",
        width: "100%",
        height: "100%",
        dataFormat: "json",
        dataSource: {
            "chart": {
                "theme": "fusion",
                "showLabels": "0",
                "toolTipBgColor": "#000000",
                "toolTipColor": "#FFFFFF",
                "plotToolText": "$label  <br> $dataValue",
                "tooltipborderradius": "4",
                //"defaultCenterLabel": tData.Percent,
                "centerLabel": "$value",
                "numberSuffix": "%",
                "showPercentValues": "0",
                "showTooltip": "0",
            },
            "data": data
        }
    }
    new FusionCharts(chartConfigs).render();

    $('.SpendingAndRemainingLoader').hide();
}

function PlotGridData(data) {
    var $tableBody = $(document).find('#table-widget .widget-table tbody');
    $.each(data.gridData, function (index, item) {
        var $row = $('<tr>');
        $row.append($('<td>').text(item.Key));
        $row.append($('<td>').text("$ " + (item.Value).toFixed(2)).css('text-align', 'right'));
        $tableBody.append($row);
    });
}






