$(document).ready(function () {
     var pDropdownDef = 3;
    $(document).find('#woPriorityDropdown').val(pDropdownDef).trigger('change');
});
$(document).on('change', '#woPriorityDropdown', function () {
    $('#workorderbyprioritychartloader').show();
    $('#workorderchartdivPriority').prev().hide();
    wobyPriorityGraphDataWidget();
});

function wobyPriorityGraphDataWidget() {
    var count = 0;
    $('#js-legendPrio').html('');
    if (typeof WObyPriorityChart !== "undefined") {
        WObyPriorityChart.destroy();
    }
    var wotimeframe1 = $(document).find('#woPriorityDropdown').val();
    $.ajax({
        type: "GET",
        url: "/Dashboard/WObyPriorityData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccess,
        async: true,
        data: {
            wotimeframe1: wotimeframe1
        },
        complete: function () {
            $('#workorderbyprioritychartloader').hide();
        }
    });
    function OnSuccess(response) {
        count = response.labels.length;
        if (response.series.length > 0) {
            $('#workorderchartdivPriority').prev().hide();
            $('#js-legendPrio').show();
            if (response.length == 0) {
                $('#workorderchartdivPriority').prev().show();
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
            WObyPriorityChart = new Chart(document.getElementById('workorderchartdivPriority').getContext("2d"), {
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
            document.getElementById('js-legendPrio').innerHTML = WObyPriorityChart.generateLegend();
        }
        else {
            $('#js-legendPrio').hide();
            $('#workorderchartdivPriority').prev().show();
        }
    }
}