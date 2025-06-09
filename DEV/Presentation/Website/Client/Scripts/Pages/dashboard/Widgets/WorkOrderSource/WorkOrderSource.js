var noDataWO = true;
$(document).ready(function () {
    WorkOrderSourceLine_dbWidget();
});
$(document).on('change', '#WOSourceDropdown', function () {
    $('#workordersourcechartloader').show();
    $('#m_chart_WO_SRC').prev().hide();
    WorkOrderSourceLine_dbWidget();
});

function WorkOrderSourceLine_dbWidget() {
    $('#m_chart_WO_SRC-legend').html('');
    if (typeof WorkOrderSourceLineChart !== "undefined") {
        WorkOrderSourceLineChart.destroy();
    }
    var duration = parseInt($(document).find('#WOSourceDropdown').val());
    $.ajax({
        type: "GET",
        url: "/DashBoard/WOSource?duration=" + duration,
        dataType: "JSON",
        success: function (data) {
            if (data.WOSourceDbList.length == 0) {
                $('#m_chart_WO_SRC').parent().siblings().find(".cntNoData").show();
                return;
            }
            else {
                $('#m_chart_WO_SRC').prev().hide();
            }
            var options = {
                type: 'line',
                data: {
                    labels: data.SourceType,
                    datasets: data.WOSourceDbList
                },
                options: {
                    legend: {
                        display: false,
                    },
                    responsive: true,
                    maintainAspectRatio: false,
                    tooltips: {
                        mode: 'index',
                        intersect: true,
                        titleFontSize: 12,
                        bodyFontSize: 10,
                        titleFontStyle: 'normal'
                    },
                    hover: {
                        mode: 'nearest',
                        intersect: true
                    },
                    scales: {
                        xAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                fontSize: 12,
                                labelString: "Month",
                            },
                            ticks: {
                                fontSize: 9,
                                fontColor: "#575962",
                                fontStyle: "bold"
                            }
                        }],
                        yAxes: [{
                            display: true,
                            ticks: {
                                fontSize: 9,
                                fontColor: "#575962",
                                fontStyle: "bold",
                            },
                            scaleLabel: {
                                display: true,
                                fontSize: 12,
                                labelString: "Value",
                                padding: 5
                            }
                        }]
                    }
                }
            }
            var ctx = document.getElementById('m_chart_WO_SRC').getContext('2d');
            WorkOrderSourceLineChart = new Chart(ctx, options);
        },
        complete: function () {
            $('#workordersourcechartloader').hide();
            if (!noDataWO) {
                document.getElementById('m_chart_WO_SRC-legend').innerHTML = WorkOrderSourceLineChart.generateLegend();
            }
        }
    });
}