$(document).ready(function () {
    generateWOSourceTypeGraphWidget();
});

$(document).on('change', '#wosourcetypeDropdown', function () {
    $('#wosourcetypechartloader').show();
    $('#workordersourcechartdiv').prev().hide();
    generateWOSourceTypeGraphWidget();
});

function generateWOSourceTypeGraphWidget() {
    if (typeof WoSourceTypeChart !== "undefined") {
        WoSourceTypeChart.destroy();
    }
    var timeframe = parseInt($(document).find('#wosourcetypeDropdown').val());
    var c = document.getElementById("workordersourcechartdiv");
    var ctx = c.getContext("2d");
    $.ajax({
        type: "GET",
        url: "/Dashboard/WOSourceTypeData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (tData) {
            if (tData.labels.length == 0) {
                $('#workordersourcechartdiv').prev().show();
                return;
            }
            else {
                $('#workordersourcechartdiv').prev().hide();
                WoSourceTypeChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: tData.labels,
                        datasets: [
                            {
                                label: "All",
                                data: tData.dataPack1,
                                backgroundColor: "#34bfa3",
                            },
                            {
                                label: "Complete",
                                data: tData.dataPack2,
                                backgroundColor: 'red'// "#3442e2",
                            }
                        ]
                    },
                    options: {
                        title: {
                            display: false,
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
                        barRadius: 4,
                        scales: {
                            xAxes: [{
                                display: true,
                                gridLines: false,
                                stacked: true,
                                scaleLabel: {
                                    labelString: 'Work Order Source',
                                    display: true,
                                    fontSize: 12,
                                    padding: 8
                                },
                                ticks: {
                                    display: false
                                }
                            }],
                            yAxes: [{
                                display: true,
                                stacked: true,
                                gridLines: false,
                                ticks: {
                                    fontSize: 10,
                                    fontColor: "#575962",
                                    fontStyle: "bold"
                                },
                                scaleLabel: {
                                    labelString: 'All / Complete Count',
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
                }
                );
            }
        },
        async: true,
        data: {
            timeframe: timeframe
        },
        complete: function () {
            $('#wosourcetypechartloader').hide();
        }
    });
}