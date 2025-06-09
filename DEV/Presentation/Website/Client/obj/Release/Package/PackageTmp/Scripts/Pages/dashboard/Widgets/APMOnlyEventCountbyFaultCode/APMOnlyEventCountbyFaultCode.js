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
$(document).ready(function () {
    generateFaultCodeChartWidget();
});

$(document).on('change', '#FCCDropdown', function () {
    $('#EventCountbyFaultCodechartloader').show();
    $("#EventCountbyFaultCode").prev().hide();
    generateFaultCodeChartWidget();
});

function generateFaultCodeChartWidget() {
    if (typeof FaultCodeChart !== "undefined") {
        FaultCodeChart.destroy();
    }
    var timeframe = parseInt($(document).find('#FCCDropdown').val());
    var c = document.getElementById("EventCountbyFaultCode");
    var ctx = c.getContext("2d");
    $.ajax({
        type: "GET",
        url: "/Dashboard/FaultCodeChartData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (tData) {
            if (tData.datasets != null && tData.datasets.length > 0) {
                var count = tData.labels.length;
                tData.datasets[0].backgroundColor = colorArray.slice(0, count);
                $("#EventCountbyFaultCode").show();
                $("#EventCountbyFaultCode").prev().hide();
                FaultCodeChart = new Chart(ctx, {
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
                                    labelString: 'Fault Code',
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
                                    fontSize: 9,
                                    fontColor: "#575962",
                                    fontStyle: "bold"
                                },
                                scaleLabel: {
                                    labelString: 'Events Count',
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