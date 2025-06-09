var doughnut2dConfig = {
    renderAt: "",
    type: "doughnut2d",
    width: "100%",
    height: "100%",
    dataFormat: "json",
    dataSource: {
        "chart": '',
        "data": ''
    }
}
var overlappedbar2dConfig = {
    renderAt: "",
    type: "overlappedbar2d",
    width: "100%",
    height: "100%",
    dataFormat: "json",
    dataSource: {
        "chart": '',
        "categories": '',
        "dataset": ''
    }
}

function LoadDashboardTab() {
    $.ajax({
        url: '/WorkOrderPlanning/WorkOrderPlannigDashboard',
        type: 'POST',
        dataType: 'html',
        data: { WorkOrderPlanID: WorkOrderPlanId },
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
            GenerateWOPlanLineItemStatusesData()
            GenerateScheduleComplience();
            GenerateWorkOrderByType();
            GenerateWOPlanEstimateHours();
            GeneratePlannedWorkOrderByAssigned();
            GeneratePlannedWorkOrderByHours();
        },
        error: function (err) {
            CloseLoader();
        }
    });

}
$(function () {
    SetFunsionChartGlobalSettings();
});

function GenerateWOPlanLineItemStatusesData() {

    $.ajax({
        type: "POST",
        url: "/WorkOrderPlanning/PlannedWorkOrderLineItemStatuses",
        dataType: "json",
        data: { WorkOrderPlanID: WorkOrderPlanId},
        success: function (tData) {
            if (tData.length > 0) {
                for (var i = 0; i < tData.length; i++) {
                    if (tData[i].Key === "Planned") { $("#WOLineItemPlannedCount").html(tData[i].Value); continue; }
                    if (tData[i].Key === "Incomplete") { $("#WOLineItemInCompleteCount").html(tData[i].Value); continue; }
                    if (tData[i].Key === "Completed") { $("#WOLineItemCompleteCount").html(tData[i].Value); continue; }
                    if (tData[i].Key === "Break-In") { $("#WOLineItemBreakInCount").html(tData[i].Value); continue; }
                }
            }
        },
        complete: function () {
            $(document).find('.Schedule-Compliance-loader').hide();
        }
    });
}

function GenerateWOPlanEstimateHours() {
    $.ajax({
        type: "POST",
        url: "/WorkOrderPlanning/PlannedWorkOrderPlanEstimateHours",
        dataType: "json",
        data: { WorkOrderPlanID: WorkOrderPlanId},
        success: function (tData) {
            if (tData.length > 0) {
                for (var i = 0; i < tData.length; i++) {
                    if (tData[i].Key === "EstimatedHours") { $("#WOLineItemPlannedEstimatedHours").html(tData[i].Value); continue; }
                    if (tData[i].Key === "ActualHours") { $("#WOLineItemPlannedActualHours").html(tData[i].Value); continue; }
                }
            }
        },
        complete: function () {
            $(document).find('.hours-count-loader').hide();
        }
    });
}

function GenerateScheduleComplience() {
    $.ajax({
        type: "POST",
        url: "/WorkOrderPlanning/GetScheduleComplianceChartData",
        dataType: "json",
        data: { WorkOrderPlanID: WorkOrderPlanId },
        success: function (data) {
            GenerateDoughnutChart(data, 'scheduleComplience');
        },
        complete: function () {
            $('#scheduleComplienceloader').hide();
        }
    });
}

function GenerateWorkOrderByType() {

    $.ajax({
        type: "POST",
        url: "/WorkOrderPlanning/PlannedWorkOrderByType",
        dataType: "json",
        data: { WorkOrderPlanID: WorkOrderPlanId },
        success: function (data) {
            const chartConfigs = {
                renderAt: "WorkOrderByType",
                type: data.chart.chartType,
                width: "100%",
                height: "100%",
                dataFormat: "json",
                dataSource: {
                    "chart": data.chart,
                    "dataset": data.dataset,
                    "categories": data.categories
                }
            }
            new FusionCharts(chartConfigs).render();
        },
        complete: function () {
            $('#WorkOrderByTypeloader').hide();
        }
    });
}

function GeneratePlannedWorkOrderByAssigned() {
    $.ajax({
        type: "POST",
        url: "/WorkOrderPlanning/PlannedWorkOrderByAssigned",
        dataType: "json",
        data: { WorkOrderPlanID: WorkOrderPlanId },
        success: function (data) {
            Generateoverlappedbar2dChart(data, "WorkOrderByAssigned");
        },
        complete: function () {
            $('#WorkOrderByAssignedloader').hide();
        }
    });
}

function GeneratePlannedWorkOrderByHours() {
    $.ajax({
        type: "POST",
        url: "/WorkOrderPlanning/PlannedWorkOrderByHours",
        dataType: "json",
        data: { WorkOrderPlanID: WorkOrderPlanId},
        success: function (data) {
            Generateoverlappedbar2dChart(data, "workOrderByHours");
        },
        complete: function () {
            $('#workOrderByHoursloader').hide();
        }
    });
}


//#region common
function GenerateDoughnutChart(data, renderAt) {
    doughnut2dConfig.renderAt = renderAt;
    doughnut2dConfig.dataSource.chart = data.info;
    doughnut2dConfig.dataSource.data = data.data;
    const chartConfigs = doughnut2dConfig;
    new FusionCharts(chartConfigs).render();
}
function Generateoverlappedbar2dChart(data, renderAt) {
    overlappedbar2dConfig.renderAt = renderAt;
    overlappedbar2dConfig.dataSource.chart = data.chart;
    overlappedbar2dConfig.dataSource.categories = data.categories;
    overlappedbar2dConfig.dataSource.dataset = data.dataset;
    const chartConfigs = overlappedbar2dConfig;
    new FusionCharts(chartConfigs).render();
}
//#endregion


