//#region Constant Values
var BarChartofJobsbyStatus;
var colorArray = ["#fe0000",
    "#ff7f00",
    "#fffe01",
    "#00bd3f",
    "#0068ff",
    "#7a01e6",
    "#d300c9",
    "#940100",
    "#066d7c",
    "#66cbff"];
var figridurl = '../FleetIssue/Index?page=Fleet_Issues';
var figridName = 'FleetIssue_Search';
var fsgridurl = '../FleetService/Index?page=Fleet_Service';
var fsgridName = 'FleetService_Search';
var partgridName = 'Part_Search';
var pardgridurl = '../Parts/Index?page=Inventory_Part';
var sparklineOptions = {
    responsive: true,
    legend: {
        display: false
    },
    elements: {
        line: {
            borderColor: '#fff',
            borderWidth: 3
        },
        point: {
            radius: 0
        }
    },
    scales: {
        yAxes: [
            {
                display: false
            }
        ],
        xAxes: [
            {
                display: false
            }
        ]
    }
};
//#endregion
//#region Ready Function
$(function () {
    var ServiceOrderBacklogWidgetChart;

    GetOpenServiceOrdersCounts();
    GetOpenFleetIssuesCounts();
    GetPastDueServiceCounts();
    LowParts_SparklineChart();
    LaborHrBar_db();
    ServiceOrderBacklogWidget_db();
});
//#endregion
//#region Open Service Order Count
function GetOpenServiceOrdersCounts() {
    $.ajax({
        url: '/Dashboard/GetOpenServiceOrdersCounts',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#OpenServiceOrderCount').text(data);
        },
        complete: function () {
            $('#openserviceordspchartloader').hide();
        }
    });
}
//#endregion
//#region Open Fleet Issues Count
function GetOpenFleetIssuesCounts() {
    $.ajax({
        url: '/Dashboard/GetOpenFleetIssuesCounts',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#OpenFleetIssuesCount').text(data);
        },
        complete: function () {
            $('#openissuesspchartloader').hide();
        }
    });
}
//#endregion
//#region Past Due Service Count
function GetPastDueServiceCounts() {
    $.ajax({
        url: '/Dashboard/GetPastDueServiceCounts',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#PastDueServiceCount').text(data);
        },
        complete: function () {
            $('#openpastduespchartloader').hide();
        }
    });
}
//#endregion
//#region Low Parts Count
function LowParts_SparklineChart() {
    $.ajax({
        type: "GET",
        url: "/DashBoard/LoadingLowPartsSpChart",
        dataType: "JSON",
        success: function (data) {
            if ($('#lowPartsCount').length > 0) {
                $('#lowPartsCount').append(data.lowPartsCount);
            }
            else if ($('#lowPartsCountInactive').length > 0) {
                $('#lowPartsCountInactive').append(data.lowPartsCount);
            }
            if (data.metricsValueList.length > 0) {
                const ctx = document.getElementById('myChart_LowParts').getContext('2d');
                const chart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: data.dataDateList,
                        datasets: [{
                            data: data.metricsValueList,
                            label: "",
                            fill: false,
                            borderColor: "#fff"
                        }]
                    },
                    options: sparklineOptions
                });
            }
            //else {
            //    $('#myChart_LowParts').prev().show();
            //}
        },
        error: function (a, b, c) {
            $('#lowpartspchartloader').hide();
            if (xhr.status === 401) {
                window.location.href = "/";
            }
        },
        complete: function () {
            $('#lowpartspchartloader').hide();
        }
    });
}
//#endregion
//#region of Service Order Labor Hours
$(document).on('change', '#LaborHrsDropdown', function () {
    $('#m_chart_Labor_Hr').prev().hide();
    $('#solaborhourchartloader').show();
    LaborHrBar_db();
});
function LaborHrBar_db() {
    if (typeof LaborHrBarChart !== "undefined") {
        LaborHrBarChart.destroy();
    }
    var duration = parseInt($(document).find('#LaborHrsDropdown').val());
    $.ajax({
        type: "GET",
        url: "/DashBoard/ServiceorderLaborHr?duration=" + duration,
        dataType: "JSON",
        async: true,
        success: function (data) {
            if (data.length == 0) {
                $('#m_chart_Labor_Hr').prev().show();
                return;
            }
            else {
                $('#m_chart_Labor_Hr').prev().hide();
            }
            var chartKey = [], chartVal = [];
            $.each(data, function (index, item) {
                chartKey.push(item.PID);
                chartVal.push(item.Hrs);
            });
            var c = document.getElementById("m_chart_Labor_Hr");
            var ctx = c.getContext("2d");
            LaborHrBarChart = new Chart(ctx, {
                type: 'horizontalBar',
                data: {
                    labels: chartKey,
                    datasets: [{
                        label: '',
                        data: chartVal,
                        backgroundColor: 'rgba(255,99,132,1)',
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
                                labelString: "Hrs",
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
                                labelString: "Personnel",
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
            $('#solaborhourchartloader').hide();
        }
    });
}
//#endregion
//#region Service Order Backlog
var noData = true;
function ServiceOrderBacklogWidget_db() {
    if (typeof ServiceOrderBacklogWidgetChart !== "undefined") {
        ServiceOrderBacklogWidgetChart.destroy();
    }
    $.ajax({
        type: "GET",
        url: "/DashBoard/ServiceOrderBacklogWidget",
        dataType: "JSON",
        catche: false,
        success: function (data) {
            $.each(data, function (index, item) {
                if (item.SoCount > 0) {
                    noData = false;
                }
            });
            if (noData) {
                $(document).find('.cntNoData').show();
                $('#m_chart_SO_BCK_LOG-legend').hide();
                $('#m_chart_SO_BCK_LOG').prev().show();
            }
            else {
                $(document).find('.cntNoData').hide();
                var chartKey = [], chartVal = [];
                $.each(data, function (index, item) {
                    chartKey.push(item.SoCount);
                    chartVal.push(item.DateRange);
                });
                var barOptions_stacked = {
                    events: [],
                    responsive: true,
                    maintainAspectRatio: false,
                    tooltips: {
                        enabled: true,
                        titleFontSize: 12,
                        bodyFontSize: 10,
                        titleFontStyle: 'normal'
                    },
                    legend: {
                        display: false,
                        position: 'right',
                        boxWidth: 5,
                        fontSize: 8,
                        padding: 5,
                    },
                    scales: {
                        xAxes: [{
                            display: false,
                            ticks: {
                                beginAtZero: true,
                                fontSize: 9,
                                fontColor: "#575962",
                                fontStyle: "bold"
                            },
                            scaleLabel: {
                                display: false
                            },
                            gridLines: {
                                display: false,
                                color: "grey"
                            },
                            stacked: true
                        }],
                        yAxes: [{
                            gridLines: {
                                display: false,
                                color: "#fff",
                                zeroLineColor: "#fff",
                                zeroLineWidth: 0
                            },
                            ticks: {
                                fontSize: 9,
                                fontColor: "#575962",
                                fontStyle: "bold"
                            },
                            stacked: true,
                            barThickness: 40
                        }]
                    },
                    animation: {
                        onComplete: function () {
                            var currentModel = 0;

                            var chartInstance = this.chart;
                            var ctx = chartInstance.ctx;
                            ctx.textAlign = "center";
                            ctx.font = "14px Roboto";
                            Chart.helpers.each(this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                Chart.helpers.each(meta.data.forEach(function (bar, index) {
                                    data = dataset.data[index];
                                    var xTotalVal = bar._model.x;
                                    var thisXval = xTotalVal - currentModel;

                                    if (i == 0) {
                                        currentModel = bar._model.x;
                                        ctx.fillText(data, currentModel / 2, bar._model.y + 5);
                                    } else {
                                        currentModel = currentModel + thisXval;
                                        ctx.fillText(data, currentModel - (thisXval / 2), bar._model.y + 5);
                                    }
                                }), this)
                            }), this);
                        }
                    },
                };
                var ctx = document.getElementById("m_chart_SO_BCK_LOG");
                ServiceOrderBacklogWidgetChart = new Chart(ctx, {
                    type: 'horizontalBar',
                    data: {
                        datasets: [{
                            data: [chartKey[0]],
                            label: chartVal[0],
                            backgroundColor: "#50C750",
                        }, {
                            data: [chartKey[1]],
                            label: chartVal[1],
                            backgroundColor: "#EFD081",
                        }, {
                            data: [chartKey[2]],
                            label: chartVal[2],
                            backgroundColor: "#FF0000",
                        }]
                    },
                    options: barOptions_stacked
                });
            }
        },
        complete: function () {
            $('#serviceorderbacklogchartloader').hide();
            if (!noData)
            {
                document.getElementById('m_chart_SO_BCK_LOG-legend').innerHTML = ServiceOrderBacklogWidgetChart.generateLegend();
            }
        }
    });
}
//#endregion
//#region Redirect To Specific Grid
function setGridStateRedirectToGrid(GridName, url) {
    $.ajax({
        "url": gridStateLoadUrl,
        "data": { GridName: GridName },
        "dataType": "json",
        "success": function (json) {
            var gridstate = JSON.parse(json);
            gridstate.start = 0;
            $.ajax({
                "url": gridStateSaveUrl,
                "data": { GridName: GridName, LayOutInfo: JSON.stringify(gridstate) },
                "dataType": "json",
                "type": "POST",
                "success": function () { window.location.href = url; }
            });
        }
    });
}

$(document).on('click', '#OpenServiceOrderCount,#openServiceOrderCountText', function () {
    localStorage.setItem("ServiceOrderstatus", 3);
    setGridStateRedirectToGrid(fsgridName, fsgridurl);
});

$(document).on('click', '#OpenFleetIssuesCount,#openFleetIssuesCountText', function () {
    localStorage.setItem("FLEETISSUESEARCHGRIDDISPLAYSTATUS", 1);
    setGridStateRedirectToGrid(figridName, figridurl);
});

$(document).on('click', '#lowPartsCount,#lowPartstxt', function () {
    localStorage.setItem("CURRENTTABSTATUS", "4");
    setGridStateRedirectToGrid(partgridName, pardgridurl);
});

//#endregion

