$(document).ready(function () {
    LaborHrBar_dbWidget();
});
//#region of Service Order Labor Hours
$(document).on('change', '#LaborHrsDropdown', function () {
    $('#m_chart_Labor_Hr').prev().hide();
    $('#solaborhourchartloader').show();
    LaborHrBar_dbWidget();
});
function LaborHrBar_dbWidget() {
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