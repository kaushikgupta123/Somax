var ServiceOrderBacklogWidgetChart;
$(document).ready(function () {
    ServiceOrderBacklogWidget_dbWidget();
});
//#region Service Order Backlog
var noData = true;
function ServiceOrderBacklogWidget_dbWidget() {
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
            if (!noData) {
                document.getElementById('m_chart_SO_BCK_LOG-legend').innerHTML = ServiceOrderBacklogWidgetChart.generateLegend();
            }
        }
    });
}
//#endregion