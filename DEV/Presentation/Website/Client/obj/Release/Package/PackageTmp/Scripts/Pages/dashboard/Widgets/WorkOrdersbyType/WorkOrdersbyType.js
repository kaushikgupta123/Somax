$(document).ready(function () {
    wobyTypeGraphDataWidget();
});

$(document).on('change', '#wotypeDropdown', function () {
    $('#workorderbytypechartloader').show();
    $('#workorderchartdiv').prev().hide();
    wobyTypeGraphDataWidget();
});

function wobyTypeGraphDataWidget() {
    $('#js-legend').html('');
    var count = 0;
    if (typeof WObyTypeChart !== "undefined") {
        WObyTypeChart.destroy();
    }
    var wotimeframe = {
        wotimeframe: parseInt($(document).find('#wotypeDropdown').val())
    };
    $.ajax({
        type: "GET",
        url: "/Dashboard/WObyTypeData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccess,
        async: true,
        data: {
            wotimeframe: wotimeframe.wotimeframe
        },
        complete: function () {
            $('#workorderbytypechartloader').hide();
        }
    });
    function OnSuccess(response) {
        count = response.labels.length;
        if (response.series.length > 0) {
            $('#workorderchartdiv').prev().hide();
            $('#js-legend').show();
            if (response.length == 0) {
                $('#workorderchartdiv').prev().show();
                return;
            }
            var data = {
                labels: response.labels,
                datasets:
                    [
                        {
                            data: response.series,
                            backgroundColor: colorArray.slice(0, count)//response.backgroundColor,
                        }]
            };

            WObyTypeChart = new Chart(document.getElementById('workorderchartdiv').getContext("2d"), {
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
            document.getElementById('js-legend').innerHTML = WObyTypeChart.generateLegend();
        }
        else {
            $('#js-legend').hide();
            $('#workorderchartdiv').prev().show();
        }
    }
}