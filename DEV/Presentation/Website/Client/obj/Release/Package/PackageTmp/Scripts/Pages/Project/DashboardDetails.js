$(function () {
    SetFunsionChartGlobalSettings();
});

function LoadDashboardTab() {
    ProjectId = $(document).find('#projectModel_ProjectId').val();
    $.ajax({
        url: '/Project/ProjectTaskDashboard',
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
            GenerateProjectTaskDashboardStatuses()
            GenerateProjectTaskDashboardScheduleComplience();
        },
        error: function (err) {
            CloseLoader();
        }
    });

}

function GenerateProjectTaskDashboardStatuses() {
    ProjectId = $(document).find('#projectModel_ProjectId').val();
    $('.ScheduleComplienceCountLoader').show();
    $.ajax({
        type: "POST",
        url: '/Project/ProjectTaskDashboardStatuses',        
        data: {
            ProjectId: ProjectId
        },
        dataType: "json",
        success: function (tData) {
            if (tData.length > 0) {
                for (var i = 0; i < tData.length; i++) {
                    if (tData[i].Key === "Planned") { $("#ProjectTaskPlannedCount").html(tData[i].Value); continue; }
                    if (tData[i].Key === "Incomplete") { $("#ProjectTaskInCompleteCount").html(tData[i].Value); continue; }
                    if (tData[i].Key === "Completed") { $("#ProjectTaskCompleteCount").html(tData[i].Value); continue; }
                }
            }
        },
        complete: function () {
            $('.ScheduleComplienceCountLoader').hide();
        }
    });
}

function GenerateProjectTaskDashboardScheduleComplience() {
    ProjectId = $(document).find('#projectModel_ProjectId').val();
    $('.ScheduleComplienceLoader').show();
    $.ajax({
        type: "POST",
        url: "/Project/ProjectTaskDashboardScheduleComplianceStatuses",        
        data: {
            ProjectId: ProjectId
        },
        dataType: "json",
        success: function (tData) {
            const chartConfigs = {
                renderAt: "ProjectTaskScheduleComplience",
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
                    "data": tData.chartdata
                }
            }
            new FusionCharts(chartConfigs).render();
        },
        complete: function () {
            $('.ScheduleComplienceLoader').hide();
        }
    });
}






