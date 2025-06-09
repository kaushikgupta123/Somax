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
$(function () {
    GetOpenJobsCounts();
    GetRequestsCounts();
    GetOverdueJobsCounts();
    generateBarChartOfJobsByStatusChart();
    DoughnutbyPassFailGraphData();
});
function GetOpenJobsCounts() {
    $.ajax({
        url: '/Dashboard/GetOpenJobsCounts',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#OpenJobsCount').text(data);
        }
    });
}
function GetRequestsCounts() {
    $.ajax({
        url: '/Dashboard/GetRequestsCounts',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#RequestCount').text(data);
        }
    });
}
function GetOverdueJobsCounts() {
    $.ajax({
        url: '/Dashboard/GetOverdueJobsCounts',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#OverDueJobsCount').text(data);
        }
    });
}

function generateBarChartOfJobsByStatusChart() {
    if (typeof BarChartofJobsbyStatus !== "undefined") {
        BarChartofJobsbyStatus.destroy();
    }
    var timeframe = parseInt($(document).find('#ByStatusDropdown').val());
    var c = document.getElementById("EventCountbyFaultCode");
    var ctx = c.getContext("2d");
    $.ajax({
        type: "GET",
        url: "/Dashboard/GetBarChartOfJobsByStatusData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (tData) {
            if (tData.datasets != null && tData.datasets.length > 0) {
                var count = tData.labels.length;
                tData.datasets[0].backgroundColor = colorArray.slice(0, count);
                $("#EventCountbyFaultCode").show();
                $("#EventCountbyFaultCode").prev().hide();
                BarChartofJobsbyStatus = new Chart(ctx, {
                    type: 'bar',
                    data: tData,
                    options: {
                        hover: {
                            mode: false
                        },
                        title: {
                            display: false
                        },
                        tooltips: {
                            intersect: false,
                            mode: 'nearest',
                            xPadding: 10,
                            yPadding: 10,
                            caretPadding: 10,
                            titleFontSize: 12,
                            bodyFontSize: 10,
                            titleFontStyle: 'normal'
                        },
                        legend: {
                            display: false
                        },
                        responsive: true,
                        maintainAspectRatio: false,
                        barRadius: 4,
                        scales: {
                            xAxes: [{
                                display: true,
                                gridLines: true,
                                scaleLabel: {
                                    labelString: 'Status',
                                    display: true,
                                    fontSize: 12,
                                    padding: 8,

                                },
                                ticks: {
                                    display: false
                                }
                            }],
                            yAxes: [{
                                display: true,
                                gridLines: true,
                                ticks: {
                                    beginAtZero: true,
                                    fontSize: 9,
                                    fontColor: "#575962",
                                    fontStyle: "bold"
                                },
                                scaleLabel: {
                                    labelString: 'Count',
                                    display: true,
                                    fontSize: 12,
                                    padding: 5
                                }
                            }]
                        },
                        layout: {
                            padding: {
                                left: 0,
                                right: 0,
                                top: 0,
                                bottom: 0
                            }
                        }
                    }
                });
            }
            else {
                $("#EventCountbyFaultCode").prev().show();
            }
        },
        data: {
            timeframe: timeframe
        },
        error: function () {

        },
        complete: function () {
            $('#EventCountbyFaultCodechartloader').hide();
        }
    });
}

$(document).on('change', '#ByStatusDropdown', function () {
    $('#EventCountbyFaultCodechartloader').show();
    $("#EventCountbyFaultCode").prev().hide();
    generateBarChartOfJobsByStatusChart();
});

var DoughnutbyPassChart;
function DoughnutbyPassFailGraphData() {
    $('#js-legend').html('');
    var count = 0;
    if (typeof DoughnutbyPassChart !== "undefined") {
        DoughnutbyPassChart.destroy();
    }
    var timeframe = parseInt($(document).find('#bypassfail').val());
    $.ajax({
        type: "GET",
        url: "/Dashboard/GetDoughnutbyPassFailChart",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccess,
        async: true,
        data: {
            timeframe: timeframe
        },
        error: function () {
        },
        complete: function () {
            $('#DoughnutbyEventDisChartloader').hide();
        }
    });
    function OnSuccess(response) {
        count = response.labels.length;
        if (response.series.length > 0) {
            $('#DoughnutbyEventDisChart').prev().hide();
            $('#js-legend').show();
            if (response.length == 0) {
                $('#DoughnutbyEventDisChart').prev().show();
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

            DoughnutbyPassChart = new Chart(document.getElementById('DoughnutbyEventDisChart').getContext("2d"), {
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
                        display: false,
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
            document.getElementById('js-legend').innerHTML = DoughnutbyPassChart.generateLegend();
        }
        else {
            $('#js-legend').hide();
            $('#DoughnutbyEventDisChart').prev().show();
        }
    }
}
$(document).on('change', '#bypassfail', function () {
    $('#DoughnutbyEventDisChartloader').show();
    $("#DoughnutbyEventDisChart").prev().hide();
    DoughnutbyPassFailGraphData();
});