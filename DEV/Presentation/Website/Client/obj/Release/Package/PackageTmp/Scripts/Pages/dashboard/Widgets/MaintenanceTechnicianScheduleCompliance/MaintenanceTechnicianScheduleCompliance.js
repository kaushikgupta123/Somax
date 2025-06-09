$(function () {
    let colorArray = ['#fe0000', '#ff7f00', '#fffe01', '#00bd3f', '#0068ff', '#7a01e6', '#d300c9', '#940100', '#066d7c', '#66cbff'];
    let doughnut2dConfig = {
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
    $("#activity-daterangeselector").css('visibility', 'visible');
    function GenerateDoughnutChart(data, renderAt) {
        doughnut2dConfig.renderAt = renderAt;
        doughnut2dConfig.dataSource.chart = data.info;
        doughnut2dConfig.dataSource.data = data.data;
        const chartConfigs = doughnut2dConfig;
        new FusionCharts(chartConfigs).render();
    }

    $('#activity-daterangeselector').change(function () {
        $('#MaintenanceScheduleComplience').html('');
        GenerateMaintenanceScheduleComplience();
    });

    function GenerateMaintenanceScheduleComplience() {
        $('#scheduleComplienceloader').show();
        var caseno = $('#activity-daterangeselector').val();
        $.ajax({
            type: "POST",
            url: "/DashBoard/GetWorkOrderScheduleComplianceChartData",
            dataType: "json",
            data: { CaseNo: caseno },
            success: function (data) {
                GenerateDoughnutChart(data, 'MaintenanceScheduleComplience');
            },
            complete: function () {
                $('#scheduleComplienceloader').hide();
            }
        });
    }
    var MaintenanceLaborHour;
    function GenerateMaintenanceLaborHourBarWidget() {
        if (typeof LaborHrBarChart !== "undefined") {
            MaintenanceLaborHour.destroy();
        }
        $.ajax({
            type: "GET",
            url: "/DashBoard/GetTimeCardLabourHoursChartData",
            dataType: "JSON",
            async: true,
            success: function (data) {
                if (data.length == 0) {
                    $('#maintenancelaborhourchart').css('visibility', 'hidden');
                    $('#maintenancelaborhourchart').prev().show();
                }
                else {
                    $('#maintenancelaborhourchart').css('visibility', 'visible');
                    $('#maintenancelaborhourchart').prev().hide();
                    //var formatDataLabels = Array.from(data).map(function (val, idx) { return { label: val.label }; });
                    //var formatDataValues = Array.from(data).map(function (val, idx) { return { value: val.value, color: colorArray[+idx.toString().charAt(idx.toString().length - 1)] }; });


                    const chartConfigs = {
                        type: "scrollbar2d",
                        renderAt: "maintenancelaborhourchart",
                        width: "100%",
                        height: "100%",
                        dataFormat: "json",
                        dataSource: {
                            "chart": data.chart,
                            "categories": data.categories,
                            "dataset": data.dataset
                        }
                        //dataSource: {
                        //    "chart": {
                        //        "xAxisName": "Personnel",
                        //        "yAxisName": "Hrs",
                        //        "theme": "fusion",
                        //        "exportEnabled": "1",
                        //        "toolTipBgColor": "#000000",
                        //        "toolTipColor": "#FFFFFF",
                        //        "plotToolText": "$label  <br> $dataValue",
                        //        "flatScrollBars": "1",
                        //        "scrollheight": "6",
                        //        "scrollPadding": "5",
                        //        "linethickness": "3",
                        //        "scrollShowButtons": "0",
                        //        "showHoverEffect": "1",
                        //        "exportFileName": "WO_Labor_Hours",
                        //        "tooltipborderradius": "4",
                        //    },
                        //    "categories": [{
                        //        category: formatDataLabels,
                        //    }],
                        //    "dataset": [{
                        //        data: formatDataValues
                        //    }]
                        //}
                    }
                    new FusionCharts(chartConfigs).render();
                }
            },
            complete: function () {
                $('#maintenancelaborhourchartloader').hide();
            }
        });
    }
    SetFunsionChartGlobalSettings();
    if ($('#MaintenanceScheduleComplience').length > 0) {
        GenerateMaintenanceScheduleComplience();
    }
    if ($('#maintenancelaborhourchart').length > 0) {
        GenerateMaintenanceLaborHourBarWidget();
    } 
});