$(document).ready(function () {
    InventoryValuation_dbWidget();
});
function InventoryValuation_dbWidget() {
    $.ajax({
        type: "GET",
        url: "/DashBoard/InventoryValuation",
        dataType: "JSON",
        success: function (data) {
            if (data.metricsValueList.length > 0) {
                var speedCanvas = document.getElementById("m_chart_Inv_Val");
                var chartData = {
                    labels: data.dataDateList,
                    datasets: data.metricsValueList
                };
                var chartOptions = {
                    responsive: true,
                    maintainAspectRatio: false,
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
                    legend: {
                        display: false,
                        position: 'top',
                        labels: {
                            boxWidth: 0,
                            fontColor: 'black'
                        }
                    },
                    scales: {
                        xAxes: [{
                            gridLines: {
                                display: true,
                                color: "#d4d6d8"
                            },
                            scaleLabel: {
                                display: true,
                                labelString: "Days",
                                fontSize: 12,
                            },
                            ticks: {
                                beginAtZero: true,
                                fontSize: 9,
                                fontColor: "#575962",
                                fontStyle: "bold"
                            }
                        }],
                        yAxes: [{
                            gridLines: {
                                color: "#d4d6d8"
                            },
                            scaleLabel: {
                                display: true,
                                labelString: "Value",
                                fontSize: 12,
                                padding: 5
                            },
                            ticks: {
                                min: data.minValue,
                                max: data.maxValue,
                                beginAtZero: false,
                                fontSize: 9,
                                fontColor: "#575962",
                                fontStyle: "bold"
                            }
                        }]
                    }
                };
                var InventoryValuationChart = new Chart(speedCanvas, {
                    type: 'line',
                    data: chartData,
                    options: chartOptions
                });
            }
            else {
                $('#m_chart_Inv_Val').prev().show();
            }
        },
        complete: function () {
            $('#inventoryvaluationchartloader').hide();
        }
    });
}