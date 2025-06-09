$(document).ready(function () {
    LaborHrBar_dbWidget();
});
$(document).on('change', '#LaborHrsDropdown', function () {
    $('#m_chart_Labor_Hr').prev().hide();
    $('#wolaborhourchartloader').show();
    $('#m_chart_Labor_Hr').css('visibility', 'hidden');
    LaborHrBar_dbWidget();
});

function LaborHrBar_dbWidget() {
    if (typeof LaborHrBarChart !== "undefined") {
        LaborHrBarChart.destroy();
    }
    var duration = parseInt($(document).find('#LaborHrsDropdown').val());
    $.ajax({
        type: "GET",
        url: "/DashBoard/WoLaborHrNew?duration=" + duration,
        dataType: "JSON",
        async: true,
        success: function (data) {
            if (data.length == 0) {
                $('#m_chart_Labor_Hr').css('visibility', 'hidden');
                $('#m_chart_Labor_Hr').prev().show();
            }
            else {
                $('#m_chart_Labor_Hr').css('visibility', 'visible');
                $('#m_chart_Labor_Hr').prev().hide();
                var formatDataLabels = Array.from(data).map(function (val, idx) { return { label: val.label }; });
                var formatDataValues = Array.from(data).map(function (val, idx) { return { value: val.value, color: colorArray[+idx.toString().charAt(idx.toString().length - 1)] }; });


                const chartConfigs = {
                    type: "scrollbar2d",
                    renderAt: "m_chart_Labor_Hr",
                    width: "100%",
                    height: "100%",
                    dataFormat: "json",
                    dataSource: {
                        "chart": {
                            "xAxisName": "Personnel",
                            "yAxisName": "Hrs",
                            "theme": "fusion",
                            "exportEnabled": "1",
                            "toolTipBgColor": "#000000",
                            "toolTipColor": "#FFFFFF",
                            "plotToolText": "$label  <br> $dataValue",
                            "flatScrollBars": "1",
                            "scrollheight": "6",
                            "scrollPadding": "5",
                            "linethickness": "3",
                            "scrollShowButtons": "0",
                            "showHoverEffect": "1",
                            "exportFileName": "WO_Labor_Hours",
                            "tooltipborderradius": "4",
                        },
                        "categories": [{
                            category: formatDataLabels,
                        }],
                        "dataset": [{
                            data: formatDataValues
                        }]
                    }
                }
                new FusionCharts(chartConfigs).render();
            }
        },
        complete: function () {
            $('#wolaborhourchartloader').hide();
        }
    });
}