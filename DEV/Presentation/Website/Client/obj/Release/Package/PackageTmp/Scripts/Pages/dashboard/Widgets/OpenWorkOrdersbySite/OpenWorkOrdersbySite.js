var colorArray = ['#fe0000', '#ff7f00', '#fffe01', '#00bd3f', '#0068ff', '#7a01e6', '#d300c9', '#940100', '#066d7c', '#66cbff'];
$(document).ready(function () {
    OpenWorkOrderHrBar_dbWidget();
});

function OpenWorkOrderHrBar_dbWidget() {
    var count = 0;
    if (typeof OpenWorkOrderHrBarChart !== "undefined") {
        OpenWorkOrderHrBarChart.destroy();
    }
    var flag = "OpenWorkOrder";
    $.ajax({
        type: "GET",
        url: "/DashBoard/EnterpriseUserBarChart?flag=" + flag,
        dataType: "JSON",
        async: true,
        success: function (data) {
            count = data.length;
            if (data.length == 0) {
                $('#m_chart_OpenWorkorder_Hr').prev().show();
                return;
            }
            else {
                $('#m_chart_OpenWorkorder_Hr').prev().hide();
            }
            var chartKey = [], chartVal = [];
            $.each(data, function (index, item) {
                chartKey.push(item.Site);
                chartVal.push(item.Value);
            });
            var c = document.getElementById("m_chart_OpenWorkorder_Hr");
            var ctx = c.getContext("2d");
            OpenWorkOrderHrBarChart = new Chart(ctx, {
                type: 'horizontalBar',
                data: {
                    labels: chartKey,
                    datasets: [{
                        label: '',
                        data: chartVal,
                        //backgroundColor: 'rgba(255,99,132,1)',
                        backgroundColor: colorArray.slice(0, count),
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
                                labelString: "Value",
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
                                labelString: "Site",
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
            $('#openworkorderchartenterprise').hide();
        }
    });
}