var colorArray = ['#fe0000', '#ff7f00', '#fffe01', '#00bd3f', '#0068ff', '#7a01e6', '#d300c9', '#940100', '#066d7c', '#66cbff'];
$(document).ready(function () {   
    generateEquipGraphWidget();
});

$(document).on('change', '#equipDropdown', function (e) {
    $('#downtimechartloader').show();
    $("#chartdiv").css('visibility', 'hidden');
    generateEquipGraphWidget();
});

function generateEquipGraphWidget() {
    var timeframe = parseInt($(document).find('#equipDropdown').val());
    $.ajax({
        type: "GET",
        url: "/Dashboard/EquipMentChartDataNew",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (tData) {
            if (tData.length > 0) {
                $("#chartdiv").css('visibility', 'visible');
                $("#chartdiv").prev().hide();
                const chartConfigs = {
                    renderAt: "chartdiv",
                    type: "column2d",
                    width: "100%",
                    height: "100%",
                    dataFormat: "json",
                    dataSource: {
                        "chart": {
                            "xAxisName": "Asset",
                            "yAxisName": "Down Time In Minutes",
                            "theme": "fusion",
                            "exportEnabled": "1",
                            "palettecolors": colorArray.map(function (color) { return color.replace('#', ''); }).join(','),
                            "showLabels": "0",
                            "toolTipBgColor": "#000000",
                            "toolTipColor": "#FFFFFF",
                            "plotToolText": "$label  <br> $dataValue",
                            "showHoverEffect": "1",
                            "exportFileName": "Asset_Down_Time",
                            "tooltipborderradius": "4",
                        },
                        "data": tData
                    }
                }
                new FusionCharts(chartConfigs).render();

                //  $('#chartdiv').insertFusionCharts(chartConfigs);
            }
            else {
                $("#chartdiv").prev().show();
            }
        },
        data: {
            timeframe: timeframe
        },
        complete: function () {
            $('#downtimechartloader').hide();
        }
    });
}