
var colorArray = ['#fe0000', '#ff7f00', '#fffe01', '#00bd3f', '#0068ff', '#7a01e6', '#d300c9', '#940100', '#066d7c', '#66cbff'];
var wogridurl = '../WorkOrder/Index?page=Maintenance_Work_Order_Search';
var pardgridurl = '../Parts/Index?page=Inventory_Part';
var wogridName = 'WorkOrder_Search';
var partgridName = 'Part_Search';
$(function () {
    //Required for Fusion Chart.
    SetFunsionChartGlobalSettings();

    var EquipmentDownTimeChart;
    var WObyTypeChart;
    var WObyPriorityChart;
    var WorkOrderBacklogWidgetChart;
    var WoSourceTypeChart;
    var WorkOrderSourceLineChart;
    var LaborHrBarChart;
    Chart.defaults.global.defaultFontColor = '#575962';
    Chart.defaults.global.defaultFontFamily = "Roboto";
    //var pDropdownDef = 3;
    //$(document).find('#woPriorityDropdown').val(pDropdownDef).trigger('change');
    //generateEquipGraph();
   // generateWOSourceTypeGraph();
    //wobyTypeGraphData();
    //OpenWO_SparklineChart();
    //WorkRequest_SparklineChart();
    //OverDuePM_SparklineChart();
    //LowParts_SparklineChart();
    //LaborHrBar_db();
    //InventoryValuation_db();
   // WorkOrderSourceLine_db();
    //WorkOrderBacklogWidget_db();
   
});


//#region events

//#region Comment for V2-552
//$(document).on('change', '#equipDropdown', function (e) {
//    $('#downtimechartloader').show();
//    $("#chartdiv").css('visibility', 'hidden');
//    generateEquipGraph();
//});
//$(document).on('change', '#wosourcetypeDropdown', function () {
//    $('#wosourcetypechartloader').show();
//    $('#workordersourcechartdiv').prev().hide();
//    generateWOSourceTypeGraph();
//});
//$(document).on('change', '#wotypeDropdown', function () {
//    $('#workorderbytypechartloader').show();
//    $('#workorderchartdiv').prev().hide();
//    wobyTypeGraphData();
//});
//$(document).on('change', '#woPriorityDropdown', function () {
//    $('#workorderbyprioritychartloader').show();
//    $('#workorderchartdivPriority').prev().hide();
//    wobyPriorityGraphData();
//});

$(document).on('click', '#btnWOCompSchesearch', function () {
    woCompletevsSche();
});
//$(document).on('change', '#LaborHrsDropdown', function () {
//    $('#m_chart_Labor_Hr').prev().hide();
//    $('#wolaborhourchartloader').show();
//    $('#m_chart_Labor_Hr').css('visibility', 'hidden');
//    LaborHrBar_db();
//});
//$(document).on('change', '#WOSourceDropdown', function () {
//    $('#workordersourcechartloader').show();
//    $('#m_chart_WO_SRC').prev().hide();
//    WorkOrderSourceLine_db();
//});
//$(document).on('click', '#btnInventory', function () {
//    $('#inventoryvaluationchartloader').show();
//    $('#m_chart_Inv_Val').prev().hide();
//    InventoryValuation_db();
//});
//#endregion

//#endregion
//#region DashBoard Spark Line Charts
//#region  Comment for V2-552
//$(document).on('click', '#openWoCount,#openWoCountText', function () {
//    localStorage.setItem("workorderstatus", 3);
//    setGridStateRedirectToGrid(wogridName, wogridurl);
//});
//function OpenWO_SparklineChart() {
//    $.ajax({
//        type: "GET",
//        url: "/DashBoard/LoadingOpenWoSpChart",
//        dataType: "JSON",
//        success: function (data) {
//            if ($('#openWoCount').length > 0) {
//                $('#openWoCount').text(data.OpenWoCount);
//            }
//            else if ($('#openWoCountInactive').length > 0) {
//                $('#openWoCountInactive').text(data.OpenWoCount);
//            }
//            if ($('#wrCount').length > 0) {
//                $('#wrCount').text(data.wrCount);
//            }
//            else if ($('#wrCountInactive').length > 0) {
//                $('#wrCountInactive').text(data.wrCount);
//            }
//            if (data.metricsValueList.length > 0) {
//                const ctx = document.getElementById('myChart_OpenWo').getContext('2d');
//                const chart = new Chart(ctx, {
//                    type: 'line',
//                    data: {
//                        labels: data.dataDateList,
//                        datasets: [{
//                            data: data.metricsValueList,
//                            label: "",
//                            fill: false,
//                            borderColor: "#fff"
//                        }]
//                    },
//                    options: sparklineOptions
//                });
//            }
//            //else {
//            //    $('#myChart_OpenWo').prev().show();
//            //}
//        },
//        error: function (event, xhr, options, exc) {
//            $('#openwospchartloader').hide();
//            if (xhr.status === 401) {
//                window.location.href = "/";
//            }
//        },
//        complete: function () {
//            $('#openwospchartloader').hide();
//        }
//    });
//}
//$(document).on('click', '#wrCount,#wrCountText', function () {
//    localStorage.setItem('workorderstatus', 2);
//    setGridStateRedirectToGrid(wogridName, wogridurl);
//});
//function WorkRequest_SparklineChart() {
//    $.ajax({
//        type: "GET",
//        url: "/DashBoard/LoadingWorkRequestSpChart",
//        dataType: "JSON",
//        success: function (data) {
//            if (data.metricsValueList.length > 0) {
//                const ctx = document.getElementById('myChart_WR').getContext('2d');
//                const chart = new Chart(ctx, {
//                    type: 'line',
//                    data: {
//                        labels: data.dataDateList,
//                        datasets: [{
//                            data: data.metricsValueList,
//                            label: "",
//                            fill: false,
//                            borderColor: "#fff",
//                        }]
//                    },
//                    options: sparklineOptions
//                });
//            }
//            //else {
//            //    $('#myChart_WR').prev().show();
//            //}
//        },
//        error: function (a, b, c) {
//            $('#workreqspchartloader').hide();
//            if (xhr.status === 401) {
//                window.location.href = "/";
//            }
//        },
//        complete: function () {
//            $('#workreqspchartloader').hide();
//        }
//    });
//}
//$(document).on('click', '#overDuePmCount,#overDuePmCountCountText', function () {
//    localStorage.setItem("workorderstatus", 24);
//    setGridStateRedirectToGrid(wogridName, wogridurl);
//});
//function OverDuePM_SparklineChart() {
//    $.ajax({
//        type: "GET",
//        url: "/DashBoard/LoadingOverDuePmSpChart",
//        dataType: "JSON",
//        success: function (data) {
//            $('#overDuePmCount').append(data.overDuePmCount);
//            if (data.metricsValueList.length > 0) {
//                const ctx = document.getElementById('myChart_OverDuePm').getContext('2d');
//                const chart = new Chart(ctx, {
//                    type: 'line',
//                    data: {
//                        labels: data.dataDateList,
//                        datasets: [{
//                            data: data.metricsValueList,
//                            label: "",
//                            fill: false,
//                            borderColor: "#fff",
//                        }]
//                    },
//                    options: sparklineOptions
//                });
//            }
//            //else {
//            //    $('#myChart_OverDuePm').prev().show();
//            //}
//        },
//        error: function (a, b, c) {
//            $('#overduepmspchartloader').hide();
//            if (xhr.status === 401) {
//                window.location.href = "/";
//            }
//        },
//        complete: function () {
//            $('#overduepmspchartloader').hide();
//        }
//    });
//}
//$(document).on('click', '#lowPartsCount,#lowPartstxt', function () {
//    localStorage.setItem("CURRENTTABSTATUS", "4");
//    setGridStateRedirectToGrid(partgridName, pardgridurl);
//});
//function LowParts_SparklineChart() {
//    $.ajax({
//        type: "GET",
//        url: "/DashBoard/LoadingLowPartsSpChart",
//        dataType: "JSON",
//        success: function (data) {
//            if ($('#lowPartsCount').length > 0) {
//                $('#lowPartsCount').append(data.lowPartsCount);
//            }
//            else if ($('#lowPartsCountInactive').length > 0) {
//                $('#lowPartsCountInactive').append(data.lowPartsCount);
//            }
//            if (data.metricsValueList.length > 0) {
//                const ctx = document.getElementById('myChart_LowParts').getContext('2d');
//                const chart = new Chart(ctx, {
//                    type: 'line',
//                    data: {
//                        labels: data.dataDateList,
//                        datasets: [{
//                            data: data.metricsValueList,
//                            label: "",
//                            fill: false,
//                            borderColor: "#fff"
//                        }]
//                    },
//                    options: sparklineOptions
//                });
//            }
//            //else {
//            //    $('#myChart_LowParts').prev().show();
//            //}
//        },
//        error: function (a, b, c) {
//            $('#lowpartspchartloader').hide();
//            if (xhr.status === 401) {
//                window.location.href = "/";
//            }
//        },
//        complete: function () {
//            $('#lowpartspchartloader').hide();
//        }
//    });
//}
//var sparklineOptions = {
//    responsive: true,
//    legend: {
//        display: false
//    },
//    elements: {
//        line: {
//            borderColor: '#fff',
//            borderWidth: 3,
//        },
//        point: {
//            radius: 0
//        }
//    },
//    scales: {
//        yAxes: [
//            {
//                display: false
//            }
//        ],
//        xAxes: [
//            {
//                display: false
//            }
//        ]
//    }
//};
//function setGridStateRedirectToGrid(GridName, url) {
//    $.ajax({
//        "url": gridStateLoadUrl,
//        "data": { GridName: GridName },
//        "dataType": "json",
//        "success": function (json) {
//            var gridstate = JSON.parse(json);
//            gridstate.start = 0;
//            $.ajax({
//                "url": gridStateSaveUrl,
//                "data": { GridName: GridName, LayOutInfo: JSON.stringify(gridstate) },
//                "dataType": "json",
//                "type": "POST",
//                "success": function () { window.location.href = url; }
//            });
//        }
//    });
//}
//#endregion
//#endregion 
//#region Charts
//function generateEquipGraph() {
//    if (typeof EquipmentDownTimeChart !== "undefined") {
//        EquipmentDownTimeChart.destroy();
//    }
//    var timeframe = parseInt($(document).find('#equipDropdown').val());
//    var c = document.getElementById("chartdiv");
//    var ctx = c.getContext("2d");
//    $.ajax({
//        type: "GET",
//        url: "/Dashboard/EquipMentChartData",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (tData) {
//            if (tData.datasets != null && tData.datasets.length > 0) {
//                var count = tData.datasets[0].data.length;
//                tData.datasets[0].backgroundColor = colorArray.slice(0, count);
//                tData.datasets[0].borderColor = colorArray.slice(0, count);
//                $("#chartdiv").show();
//                $("#chartdiv").prev().hide();
//                EquipmentDownTimeChart = new Chart(ctx, {
//                    type: 'bar',
//                    data: tData,
//                    options: {
//                        hover: {
//                            mode: false
//                        },
//                        title: {
//                            display: false
//                        },
//                        tooltips: {
//                            intersect: false,
//                            mode: 'nearest',
//                            xPadding: 10,
//                            yPadding: 10,
//                            caretPadding: 10,
//                            titleFontSize: 12,
//                            bodyFontSize: 10,
//                            titleFontStyle: 'normal'
//                        },
//                        legend: {
//                            display: false
//                        },
//                        responsive: true,
//                        maintainAspectRatio: false,
//                        barRadius: 4,
//                        scales: {
//                            xAxes: [{
//                                display: true,
//                                gridLines: true,
//                                scaleLabel: {
//                                    labelString: 'Asset',
//                                    display: true,
//                                    fontSize: 12,
//                                    padding: 8,

//                                },
//                                ticks: {
//                                    display: false
//                                }
//                            }],
//                            yAxes: [{
//                                display: true,
//                                gridLines: true,
//                                ticks: {
//                                    beginAtZero: true,
//                                    fontSize: 9,
//                                    fontColor: "#575962",
//                                    fontStyle: "bold"
//                                },
//                                scaleLabel: {
//                                    labelString: 'Down Time In Minutes',
//                                    display: true,
//                                    fontSize: 12,
//                                    padding: 5
//                                }
//                            }]
//                        },
//                        layout: {
//                            padding: {
//                                left: 0,
//                                right: 0,
//                                top: 0,
//                                bottom: 0
//                            }
//                        }
//                    }
//                });
//            }
//            else {
//                $("#chartdiv").prev().show();
//            }
//        },
//        data: {
//            timeframe: timeframe
//        },
//        complete: function () {
//            $('#downtimechartloader').hide();
//        }
//    });
//}

//#region All Comment for V2-552
//function generateEquipGraph() {
//    var timeframe = parseInt($(document).find('#equipDropdown').val());
//    $.ajax({
//        type: "GET",
//        url: "/Dashboard/EquipMentChartDataNew",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (tData) {
//            if (tData.length > 0) {
//                $("#chartdiv").css('visibility', 'visible');
//                $("#chartdiv").prev().hide();
//                const chartConfigs = {
//                    renderAt:"chartdiv",
//                    type: "column2d",
//                    width: "100%",
//                    height: "100%",
//                    dataFormat: "json",
//                    dataSource: {
//                        "chart": {
//                            "xAxisName": "Asset",
//                            "yAxisName": "Down Time In Minutes",
//                            "theme": "fusion",
//                            "palettecolors": colorArray.map(function (color) { return color.replace('#', ''); }).join(','),
//                            "showLabels": "0",
//                            "toolTipBgColor": "#000000",
//                            "toolTipColor": "#FFFFFF",
//                            "plotToolText": "$label  <br> $dataValue",
//                            "tooltipborderradius": "4",
//                        },
//                        "data": tData
//                    }
//                }
//                new FusionCharts(chartConfigs).render();

//              //  $('#chartdiv').insertFusionCharts(chartConfigs);
//            }
//            else {
//                $("#chartdiv").prev().show();
//            }
//        },
//        data: {
//            timeframe: timeframe
//        },
//        complete: function () {
//            $('#downtimechartloader').hide();
//        }
//    });
//}


//function generateWOSourceTypeGraph() {
//    if (typeof WoSourceTypeChart !== "undefined") {
//        WoSourceTypeChart.destroy();
//    }
//    var timeframe = parseInt($(document).find('#wosourcetypeDropdown').val());
//    var c = document.getElementById("workordersourcechartdiv");
//    var ctx = c.getContext("2d");
//    $.ajax({
//        type: "GET",
//        url: "/Dashboard/WOSourceTypeData",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (tData) {
//            if (tData.labels.length == 0) {
//                $('#workordersourcechartdiv').prev().show();
//                return;
//            }
//            else {
//                $('#workordersourcechartdiv').prev().hide();
//                WoSourceTypeChart = new Chart(ctx, {
//                    type: 'bar',
//                    data: {
//                        labels: tData.labels,
//                        datasets: [
//                            {
//                                label: "All",
//                                data: tData.dataPack1,
//                                backgroundColor: "#34bfa3",
//                            },
//                            {
//                                label: "Complete",
//                                data: tData.dataPack2,
//                                backgroundColor: 'red'// "#3442e2",
//                            }
//                        ]
//                    },
//                    options: {
//                        title: {
//                            display: false,
//                        },
//                        tooltips: {
//                            intersect: false,
//                            mode: 'nearest',
//                            xPadding: 10,
//                            yPadding: 10,
//                            caretPadding: 10,
//                            titleFontSize: 12,
//                            bodyFontSize: 10,
//                            titleFontStyle: 'normal'
//                        },
//                        legend: {
//                            display: false
//                        },
//                        responsive: true,
//                        barRadius: 4,
//                        scales: {
//                            xAxes: [{
//                                display: true,
//                                gridLines: false,
//                                stacked: true,
//                                scaleLabel: {
//                                    labelString: 'Work Order Source',
//                                    display: true,
//                                    fontSize: 12,
//                                    padding: 8
//                                },
//                                ticks: {
//                                    display: false
//                                }
//                            }],
//                            yAxes: [{
//                                display: true,
//                                stacked: true,
//                                gridLines: false,
//                                ticks: {
//                                    fontSize: 10,
//                                    fontColor: "#575962",
//                                    fontStyle: "bold"
//                                },
//                                scaleLabel: {
//                                    labelString: 'All / Complete Count',
//                                    display: true,
//                                    fontSize: 12,
//                                    padding: 5

//                                }
//                            }]
//                        },
//                        layout: {
//                            padding: {
//                                left: 0,
//                                right: 0,
//                                top: 0,
//                                bottom: 0
//                            }
//                        }
//                    }
//                }
//                );
//            }
//        },
//        async: true,
//        data: {
//            timeframe: timeframe
//        },
//        complete: function () {
//            $('#wosourcetypechartloader').hide();
//        }
//    });
//}

//#endregion
function wobyPriorityGraphData() {
    var count = 0;
    $('#js-legendPrio').html('');
    if (typeof WObyPriorityChart !== "undefined") {
        WObyPriorityChart.destroy();
    }
    var wotimeframe1 = $(document).find('#woPriorityDropdown').val();
    $.ajax({
        type: "GET",
        url: "/Dashboard/WObyPriorityData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccess,
        async: true,
        data: {
            wotimeframe1: wotimeframe1
        },
        complete: function () {
            $('#workorderbyprioritychartloader').hide();
        }
    });
    function OnSuccess(response) {
        count = response.labels.length;
        if (response.series.length > 0) {
            $('#workorderchartdivPriority').prev().hide();
            $('#js-legendPrio').show();
            if (response.length == 0) {
                $('#workorderchartdivPriority').prev().show();
                return;
            }
            var data = {
                labels: response.labels,
                datasets:
                    [
                        {
                            data: response.series,
                            backgroundColor: colorArray.slice(0, count)
                        }]
            };
            WObyPriorityChart = new Chart(document.getElementById('workorderchartdivPriority').getContext("2d"), {
                plugins: [{
                    beforeDraw: function (chart, options) {
                        var width = chart.chart.width,
                            height = chart.chart.height,
                            ctx = chart.chart.ctx;
                        ctx.restore();
                        var fontSize = (height / 8).toFixed(2);
                        ctx.font = fontSize + "px 'Roboto', sans-serif";
                        ctx.textBaseline = "middle";
                        ctx.fillStyle = "grey";
                        var text = count,
                            textX = Math.round((width - ctx.measureText(text).width) / 2),
                            textY = height / 2;
                        ctx.fillText(text, textX, textY);
                        ctx.save();
                    }
                }],
                type: 'doughnut',
                data: data,
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
                    cutoutPercentage: 75
                }
            });
            document.getElementById('js-legendPrio').innerHTML = WObyPriorityChart.generateLegend();
        }
        else {
            $('#js-legendPrio').hide();
            $('#workorderchartdivPriority').prev().show();
        }
    }
}
//#region Comment for V2-552
//function wobyTypeGraphData() {
//    $('#js-legend').html('');
//    var count = 0;
//    if (typeof WObyTypeChart !== "undefined") {
//        WObyTypeChart.destroy();
//    }
//    var wotimeframe = {
//        wotimeframe: parseInt($(document).find('#wotypeDropdown').val())
//    };
//    $.ajax({
//        type: "GET",
//        url: "/Dashboard/WObyTypeData",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: OnSuccess,
//        async: true,
//        data: {
//            wotimeframe: wotimeframe.wotimeframe
//        },
//        complete: function () {
//            $('#workorderbytypechartloader').hide();
//        }
//    });
//    function OnSuccess(response) {
//        count = response.labels.length;
//        if (response.series.length > 0) {
//            $('#workorderchartdiv').prev().hide();
//            $('#js-legend').show();
//            if (response.length == 0) {
//                $('#workorderchartdiv').prev().show();
//                return;
//            }
//            var data = {
//                labels: response.labels,
//                datasets:
//                    [
//                        {
//                            data: response.series,
//                            backgroundColor: colorArray.slice(0, count)//response.backgroundColor,
//                        }]
//            };

//            WObyTypeChart = new Chart(document.getElementById('workorderchartdiv').getContext("2d"), {
//                plugins: [{
//                    beforeDraw: function (chart, options) {
//                        var width = chart.chart.width,
//                            height = chart.chart.height,
//                            ctx = chart.chart.ctx;
//                        ctx.restore();
//                        var fontSize = (height / 8).toFixed(2);
//                        ctx.font = fontSize + "px 'Roboto', sans-serif";
//                        ctx.textBaseline = "middle";
//                        ctx.fillStyle = "grey";
//                        var text = count,
//                            textX = Math.round((width - ctx.measureText(text).width) / 2),
//                            textY = height / 2;
//                        ctx.fillText(text, textX, textY);
//                        ctx.save();
//                    }
//                }],
//                type: 'doughnut',
//                data: data,
//                options: {
//                    responsive: true,
//                    maintainAspectRatio: false,
//                    legend: {
//                        display: false
//                    },
//                    tooltips: {
//                        mode: 'nearest',
//                        intersect: false,
//                        position: 'nearest',
//                        xPadding: 10,
//                        yPadding: 10,
//                        caretPadding: 10,
//                        titleFontSize: 12,
//                        bodyFontSize: 10,
//                        titleFontStyle: 'normal'
//                    },
//                    cutoutPercentage: 75
//                }
//            });
//            document.getElementById('js-legend').innerHTML = WObyTypeChart.generateLegend();
//        }
//        else {
//            $('#js-legend').hide();
//            $('#workorderchartdiv').prev().show();
//        }
//    }
//}
//#endregion
//function LaborHrBar_db() {
//    if (typeof LaborHrBarChart !== "undefined") {
//        LaborHrBarChart.destroy();
//    }
//    var duration = parseInt($(document).find('#LaborHrsDropdown').val());
//    $.ajax({
//        type: "GET",
//        url: "/DashBoard/WoLaborHr?duration=" + duration,
//        dataType: "JSON",
//        async: true,
//        success: function (data) {
//            if (data.length == 0) {
//                $('#m_chart_Labor_Hr').prev().show();
//                return;
//            }
//            else {
//                $('#m_chart_Labor_Hr').prev().hide();
//            }
//            var chartKey = [], chartVal = [];
//            $.each(data, function (index, item) {
//                chartKey.push(item.PID);
//                chartVal.push(item.Hrs);
//            });
//            var c = document.getElementById("m_chart_Labor_Hr");
//            var ctx = c.getContext("2d");
//            LaborHrBarChart = new Chart(ctx, {
//                type: 'horizontalBar',
//                data: {
//                    labels: chartKey,
//                    datasets: [{
//                        label: '',
//                        data: chartVal,
//                        backgroundColor: 'rgba(255,99,132,1)',
//                        borderWidth: 1
//                    }
//                    ]
//                },
//                options: {
//                    responsive: true,
//                    maintainAspectRatio: false,
//                    legend: {
//                        display: false
//                    },
//                    tooltips: {
//                        mode: 'nearest',
//                        intersect: false,
//                        position: 'nearest',
//                        xPadding: 10,
//                        yPadding: 10,
//                        caretPadding: 10,
//                        titleFontSize: 12,
//                        bodyFontSize: 10,
//                        titleFontStyle: 'normal'
//                    },
//                    scales: {
//                        xAxes: [{
//                            ticks: {
//                                beginAtZero: false,
//                                steps: 100,
//                                stepValue: 500,
//                                fontSize: 9,
//                                fontColor: "#575962",
//                                fontStyle: "bold"
//                            },
//                            scaleLabel: {
//                                display: true,
//                                labelString: "Hrs",
//                                fontSize: 12,
//                            }
//                        }
//                        ],
//                        yAxes: [{
//                            barThickness: 15,
//                            ticks: {
//                                beginAtZero: true,
//                                fontSize: 9,
//                                fontStyle: "bold"
//                            },
//                            scaleLabel: {
//                                display: true,
//                                labelString: "Personnel",
//                                fontSize: 12,
//                                padding: 5
//                            }
//                        },
//                        ]
//                    }
//                }
//            });
//        },
//        complete: function () {
//            $('#wolaborhourchartloader').hide();
//        }
//    });
//}

//#region Comment for V2-552
//function LaborHrBar_db() {
//    if (typeof LaborHrBarChart !== "undefined") {
//        LaborHrBarChart.destroy();
//    }
//    var duration = parseInt($(document).find('#LaborHrsDropdown').val());
//    $.ajax({
//        type: "GET",
//        url: "/DashBoard/WoLaborHrNew?duration=" + duration,
//        dataType: "JSON",
//        async: true,
//        success: function (data) {
//            if (data.length == 0) {
//                $('#m_chart_Labor_Hr').css('visibility', 'hidden');
//                $('#m_chart_Labor_Hr').prev().show();
//            }
//            else {
//                $('#m_chart_Labor_Hr').css('visibility', 'visible');
//                $('#m_chart_Labor_Hr').prev().hide();
//                var formatDataLabels = Array.from(data).map(function (val, idx) { return { label: val.label }; });
//                var formatDataValues = Array.from(data).map(function (val, idx) { return { value: val.value, color: colorArray[+idx.toString().charAt(idx.toString().length - 1)] }; });

              
//                const chartConfigs = {
//                    type: "scrollbar2d",
//                    renderAt: "m_chart_Labor_Hr",
//                    width: "100%",
//                    height: "100%",
//                    dataFormat: "json",
//                    dataSource: {
//                        "chart": {
//                            "xAxisName": "Personnel",
//                            "yAxisName": "Hrs",
//                            "theme": "fusion",
//                            "exportEnabled": "1",
//                            "toolTipBgColor": "#000000",
//                            "toolTipColor": "#FFFFFF",
//                            "plotToolText": "$label  <br> $dataValue",
//                            "flatScrollBars": "1",
//                            "scrollheight": "6",
//                            "scrollPadding": "5",
//                            "linethickness": "3",
//                            "scrollShowButtons": "0",
//                            "showHoverEffect": "1",
//                            "exportFileName": "WO_Labor_Hours",
//                            "tooltipborderradius": "4",
//                        },
//                        "categories": [{
//                            category: formatDataLabels,
//                        }],
//                        "dataset": [{
//                            data: formatDataValues
//                        }]
//                    }
//                }
//                new FusionCharts(chartConfigs).render();
//            }
//        },
//        complete: function () {
//            $('#wolaborhourchartloader').hide();
//        }
//    });
//}


//function WorkOrderSourceLine_db() {
//    $('#m_chart_WO_SRC-legend').html('');
//    if (typeof WorkOrderSourceLineChart !== "undefined") {
//        WorkOrderSourceLineChart.destroy();
//    }
//    var duration = parseInt($(document).find('#WOSourceDropdown').val());
//    $.ajax({
//        type: "GET",
//        url: "/DashBoard/WOSource?duration=" + duration,
//        dataType: "JSON",
//        success: function (data) {
//            if (data.WOSourceDbList.length == 0) {
//                $('#m_chart_WO_SRC').parent().siblings().find(".cntNoData").show();
//                return;
//            }
//            else {
//                $('#m_chart_WO_SRC').prev().hide();
//            }
//            var options = {
//                type: 'line',
//                data: {
//                    labels: data.SourceType,
//                    datasets: data.WOSourceDbList
//                },
//                options: {
//                    legend: {
//                        display: false,
//                    },
//                    responsive: true,
//                    maintainAspectRatio: false,
//                    tooltips: {
//                        mode: 'index',
//                        intersect: true,
//                        titleFontSize: 12,
//                        bodyFontSize: 10,
//                        titleFontStyle: 'normal'
//                    },
//                    hover: {
//                        mode: 'nearest',
//                        intersect: true
//                    },
//                    scales: {
//                        xAxes: [{
//                            display: true,
//                            scaleLabel: {
//                                display: true,
//                                fontSize: 12,
//                                labelString: "Month",
//                            },
//                            ticks: {
//                                fontSize: 9,
//                                fontColor: "#575962",
//                                fontStyle: "bold"
//                            }
//                        }],
//                        yAxes: [{
//                            display: true,
//                            ticks: {
//                                fontSize: 9,
//                                fontColor: "#575962",
//                                fontStyle: "bold",
//                            },
//                            scaleLabel: {
//                                display: true,
//                                fontSize: 12,
//                                labelString: "Value",
//                                padding: 5
//                            }
//                        }]
//                    }
//                }
//            }
//            var ctx = document.getElementById('m_chart_WO_SRC').getContext('2d');
//            WorkOrderSourceLineChart = new Chart(ctx, options);
//        },
//        complete: function () {
//            $('#workordersourcechartloader').hide();
//            if (!noDataWO) {
//                document.getElementById('m_chart_WO_SRC-legend').innerHTML = WorkOrderSourceLineChart.generateLegend();
//            }
//        }
//    });
//}

//function InventoryValuation_db() {
//    $.ajax({
//        type: "GET",
//        url: "/DashBoard/InventoryValuation",
//        dataType: "JSON",
//        success: function (data) {
//            if (data.metricsValueList.length > 0) {
//                var speedCanvas = document.getElementById("m_chart_Inv_Val");
//                var chartData = {
//                    labels: data.dataDateList,
//                    datasets: data.metricsValueList
//                };
//                var chartOptions = {
//                    responsive: true,
//                    maintainAspectRatio: false,
//                    tooltips: {
//                        mode: 'nearest',
//                        intersect: false,
//                        position: 'nearest',
//                        xPadding: 10,
//                        yPadding: 10,
//                        caretPadding: 10,
//                        titleFontSize: 12,
//                        bodyFontSize: 10,
//                        titleFontStyle: 'normal'
//                    },
//                    legend: {
//                        display: false,
//                        position: 'top',
//                        labels: {
//                            boxWidth: 0,
//                            fontColor: 'black'
//                        }
//                    },
//                    scales: {
//                        xAxes: [{
//                            gridLines: {
//                                display: true,
//                                color: "#d4d6d8"
//                            },
//                            scaleLabel: {
//                                display: true,
//                                labelString: "Days",
//                                fontSize: 12,
//                            },
//                            ticks: {
//                                beginAtZero: true,
//                                fontSize: 9,
//                                fontColor: "#575962",
//                                fontStyle: "bold"
//                            }
//                        }],
//                        yAxes: [{
//                            gridLines: {
//                                color: "#d4d6d8"
//                            },
//                            scaleLabel: {
//                                display: true,
//                                labelString: "Value",
//                                fontSize: 12,
//                                padding: 5
//                            },
//                            ticks: {
//                                min: data.minValue,
//                                max: data.maxValue,
//                                beginAtZero: false,
//                                fontSize: 9,
//                                fontColor: "#575962",
//                                fontStyle: "bold"
//                            }
//                        }]
//                    }
//                };
//                var InventoryValuationChart = new Chart(speedCanvas, {
//                    type: 'line',
//                    data: chartData,
//                    options: chartOptions
//                });
//            }
//            else {
//                $('#m_chart_Inv_Val').prev().show();
//            }
//        },
//        complete: function () {
//            $('#inventoryvaluationchartloader').hide();
//        }
//    });
//}

//var noDataWO = true;
//function WorkOrderBacklogWidget_db() {
//    if (typeof WorkOrderBacklogWidgetChart !== "undefined") {
//        WorkOrderBacklogWidgetChart.destroy();
//    }
//    $.ajax({
//        type: "GET",
//        url: "/DashBoard/WorkOrderBacklogWidget",
//        dataType: "JSON",
//        catche: false,
//        success: function (data) {
//            $.each(data, function (index, item) {
//                if (item.WoCount > 0) {
//                    noDataWO = false;
//                }
//            });
//            if (noDataWO) {
//                $('#m_chart_WO_BCK_LOG-legend').hide();
//                $('#m_chart_WO_BCK_LOG').prev().show();
//            }
//            else {
//                var chartKey = [], chartVal = [];
//                $.each(data, function (index, item) {
//                    chartKey.push(item.WoCount);
//                    chartVal.push(item.DateRange);
//                });
//                var barOptions_stacked = {
//                    events: [],
//                    responsive: true,
//                    maintainAspectRatio: false,
//                    tooltips: {
//                        enabled: true,
//                        titleFontSize: 12,
//                        bodyFontSize: 10,
//                        titleFontStyle: 'normal'
//                    },
//                    legend: {
//                        display: false,
//                        position: 'right',
//                        boxWidth: 5,
//                        fontSize: 8,
//                        padding: 5,
//                    },
//                    scales: {
//                        xAxes: [{
//                            display: false,
//                            ticks: {
//                                beginAtZero: true,
//                                fontSize: 9,
//                                fontColor: "#575962",
//                                fontStyle: "bold"
//                            },
//                            scaleLabel: {
//                                display: false
//                            },
//                            gridLines: {
//                                display: false,
//                                color: "grey"
//                            },
//                            stacked: true
//                        }],
//                        yAxes: [{
//                            gridLines: {
//                                display: false,
//                                color: "#fff",
//                                zeroLineColor: "#fff",
//                                zeroLineWidth: 0
//                            },
//                            ticks: {
//                                fontSize: 9,
//                                fontColor: "#575962",
//                                fontStyle: "bold"
//                            },
//                            stacked: true,
//                            barThickness: 40
//                        }]
//                    },
//                    animation: {
//                        onComplete: function () {
//                            var currentModel = 0;

//                            var chartInstance = this.chart;
//                            var ctx = chartInstance.ctx;
//                            ctx.textAlign = "center";
//                            ctx.font = "14px Roboto";
//                            Chart.helpers.each(this.data.datasets.forEach(function (dataset, i) {
//                                var meta = chartInstance.controller.getDatasetMeta(i);
//                                Chart.helpers.each(meta.data.forEach(function (bar, index) {
//                                    data = dataset.data[index];
//                                    var xTotalVal = bar._model.x;
//                                    var thisXval = xTotalVal - currentModel;

//                                    if (i == 0) {
//                                        currentModel = bar._model.x;
//                                        ctx.fillText(data, currentModel / 2, bar._model.y + 5);
//                                    } else {
//                                        currentModel = currentModel + thisXval;
//                                        ctx.fillText(data, currentModel - (thisXval / 2), bar._model.y + 5);
//                                    }
//                                }), this)
//                            }), this);
//                        }
//                    },
//                };
//                var ctx = document.getElementById("m_chart_WO_BCK_LOG");
//                WorkOrderBacklogWidgetChart = new Chart(ctx, {
//                    type: 'horizontalBar',
//                    data: {
//                        datasets: [{
//                            data: [chartKey[0]],
//                            label: chartVal[0],
//                            backgroundColor: "#50C750",
//                        }, {
//                            data: [chartKey[1]],
//                            label: chartVal[1],
//                            backgroundColor: "#EFD081",
//                        }, {
//                            data: [chartKey[2]],
//                            label: chartVal[2],
//                            backgroundColor: "#FF0000",
//                        }]
//                    },
//                    options: barOptions_stacked
//                });
//            }
//        },
//        complete: function () {
//            $('#workorderbacklogchartloader').hide();
//            if (!noDataWO) {
//                document.getElementById('m_chart_WO_BCK_LOG-legend').innerHTML = WorkOrderBacklogWidgetChart.generateLegend();
//            }
//        }
//    });
//}
//#endregion
//#endregion
