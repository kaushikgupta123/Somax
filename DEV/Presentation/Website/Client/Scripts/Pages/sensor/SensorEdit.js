$(document).ready(function () {
    amGaugeChart();
    var retDate;
    $("#readings").dataTable({
        colReorder: true,
        dom: 'Bfrtip',
        "orderMulti": true,
        "ajax": {
            "url": "/SensorEdit/Resultgrid",
            "type": "GET",
            "datatype": "json",
        },
        "aoColumnDefs":
            [
                {
                    "targets": [0], 
                    "render": function (data, type, row) {                       
                        retDate = ConvertJsonDateString(data);
                        return (retDate);
                    },
                },
            ],

        "columns":
        [
            { "data": "MessageDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "PlotValues", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "PlotLabels", "autoWidth": true, "bSearchable": true, "bSortable": true },
        ]

    }); 
    $(document).on('change', '#drpGroup', function () {
        var selectedIndex = $(this).prop('selectedIndex');
        var selectedText = $(this).find('option:selected').text();
        renderDataTable(selectedIndex, selectedText);
    });   
});
function amGaugeChart()
{
    $.ajax({
        dataType: 'json',
        url: "/SensorEdit/GaugeChart",
        type: 'GET',
        success: function (data, status, xhr) {
            if (data.LastReading != null) {
                var chart = AmCharts.makeChart("chartdiv", {
                    "type": "gauge",
                    "arrows": [
                        {
                            "value": data.LastReading,
                            "balloonText": "Average of Value",
                        }
                    ],
                    "titles": [
                        {
                            "text": "Last Reading",
                            "size": 15
                        }
                    ],
                    "axes": [
                        {
                            "endValue": 100,
                            "valueInterval": 10,
                            "bands": [
                                {
                                    "color": "#0070cc",
                                    "endValue": 10,
                                    "startValue": 0,
                                    "innerRadius": "95%",
                                },
                                {
                                    "color": "#e1f22b",
                                    "endValue": 20,
                                    "startValue": 10,
                                    "innerRadius": "95%",
                                },
                                {
                                    "color": "#36f21d",
                                    "endValue": 30,
                                    "startValue": 20,
                                    "innerRadius": "95%",
                                },
                                {
                                    "color": "#e1f22b",
                                    "endValue": 40,
                                    "startValue": 30,
                                    "innerRadius": "95%",
                                },
                                {
                                    "color": "#36f21d",
                                    "endValue": 50,
                                    "startValue": 40,
                                    "innerRadius": "95%",
                                },
                                {
                                    "color": "#c9cec8",
                                    "endValue": 60,
                                    "startValue": 50,
                                    "innerRadius": "95%",
                                },
                                {
                                    "color": "#e1f22b",
                                    "endValue": 70,
                                    "startValue": 60,
                                    "innerRadius": "95%",
                                },
                                {
                                    "color": "#ea3838",
                                    "endValue": 80,
                                    "startValue": 70,
                                    "innerRadius": "95%",
                                },
                                {
                                    "color": "#e1f22b",
                                    "endValue": 90,
                                    "startValue": 80,
                                    "innerRadius": "95%",
                                },
                                {
                                    "color": "#ea3838",
                                    "endValue": 100,
                                    "startValue": 90,
                                    "innerRadius": "95%",
                                }
                            ]
                        }
                    ],
                    "balloon": {
                        "adjustBorderColor": true,
                        "color": "#000000",
                        "cornerRadius": 5,
                        "fillColor": "#FFFFFF"
                    },
                    "listeners": [{
                        "event": "rendered",
                        "method": function (event) {
                            var chart = event.chart;
                            var text = "";
                            for (var i = 0; i < chart.arrows.length; i++) {
                                var arrow = chart.arrows[i];
                                text += arrow.title + ": " + arrow.value + "<br />";
                            }
                            for (var i = 0; i < chart.axes[0].bands.length; i++) {
                                chart.axes[0].bands[i].balloonText = text;
                                chart.arrows[0].balloonText = text;
                            }
                        }
                    }]
                });

            } 
        },
        error: function () {
            $('#info').html('<p>An error has occurred</p>');
        },
    });
}
function ConvertJsonDateString(jsonDate) {
    var shortDate = null;
    if (jsonDate) {
        var regex = /-?\d+/;
        var matches = regex.exec(jsonDate);
        var dt = new Date(parseInt(matches[0]));
        var month = dt.getMonth() + 1;
        var monthString = month > 9 ? month : '0' + month;
        var day = dt.getDate();
        var dayString = day > 9 ? day : '0' + day;
        var year = dt.getFullYear();
        shortDate = monthString + '-' + dayString + '-' + year;
    }
    return shortDate;
};
function zoomChart(data, chart) {
    chart.zoomToIndexes(data.length - 40, data.length - 1);
}

$("#btnDatepicker").on('click', function (e) {  
    $.ajax({
        url: "/SensorEdit/GetDateRange",
        type: "POST",
        dataType: "json",               
        data: { _startDate: $("#eventdate_start").val(), _endDate: $("#eventdate_end").val() },
        success: function (data) {         
            if (data != null) {
                $.each(data, function (i) {                    
                    data[i].MessageDate = ConvertJsonDateString(data[i].MessageDate);
                });
                var chart = AmCharts.makeChart("linechartdiv", {
                    "type": "serial",
                    "theme": "none",
                    "marginRight": 40,
                    "marginLeft": 40,
                    "autoMarginOffset": 20,
                    "mouseWheelZoomEnabled": true,
                    "dataDateFormat": "MM/DD/YYYY",
                    "dataProvider": data,
                    "valueAxes": [{
                        "id": "v1",
                        "axisAlpha": 0,
                        "position": "left",
                        "ignoreAxisWidth": true
                    }],
                    "balloon": {
                        "borderThickness": 1,
                        "shadowAlpha": 0
                    },
                    "graphs": [{
                        "id": "g1",
                        "balloon": {
                            "drop": true,
                            "adjustBorderColor": false,
                            "color": "#ffffff"
                        },
                        "bullet": "round",
                        "bulletBorderAlpha": 1,
                        "bulletColor": "#FFFFFF",
                        "bulletSize": 5,
                        "hideBulletsCount": 50,
                        "lineThickness": 2,
                        "title": "red line",
                        "useLineColorForBulletBorder": true,
                        "valueField": "PlotValues",
                        "balloonText": "<span style='font-size:18px;'>[[value]]</span>"
                    }],
                    "chartScrollbar": {
                        "oppositeAxis": false,
                        "offset": 30,
                        "scrollbarHeight": 30,
                        "backgroundAlpha": 0,
                        "selectedBackgroundAlpha": 0.1,
                        "selectedBackgroundColor": "#888888",
                        "graphFillAlpha": 0,
                        "graphLineAlpha": 0.5,
                        "selectedGraphFillAlpha": 0,
                        "selectedGraphLineAlpha": 1,
                        "autoGridCount": true,
                        "color": "#AAAAAA"
                    },
                    "chartCursor": {
                        "pan": true,
                        "valueLineEnabled": true,
                        "valueLineBalloonEnabled": true,
                        "cursorAlpha": 1,
                        "cursorColor": "#258cbb",
                        "limitToGraph": "g1",
                        "valueLineAlpha": 0.2,
                        "valueZoomable": true
                    },
                    "valueScrollbar": {
                        "oppositeAxis": false,
                        "offset": 50,
                        "scrollbarHeight": 10
                    },
                    "categoryField": "MessageDate",
                    "categoryAxis": {
                        "parseDates": false,
                        "dashLength": 1,
                        "minorGridEnabled": false
                    },
                    "export": {
                        "enabled": true
                    },
                });
                chart.addListener("rendered", zoomChart);
                chart.zoomToIndexes(data.length - 40, data.length - 1);                
            }
        }
    });
});
function renderDataTable(selectedIndex, selectedText) {
    $("#readings").DataTable().destroy();
    if (selectedIndex == 0) {
        $("#readings").dataTable({
            colReorder: true,
            dom: 'Bfrtip',
            "orderMulti": true, 
            "ajax": {
                "url": "/SensorEdit/Resultgrid",
                "type": "GET",
                "datatype": "json",
            },
            "aoColumnDefs":
                [
                    {
                        "targets": [0],   
                        "render": function (data, type, row) {                     
                            retDate = ConvertJsonDateString(data);
                            return (retDate);
                        },
                    },
                ],

            "columns":
            [
                { "data": "MessageDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PlotValues", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PlotLabels", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ]

        });
    }
    else {
        selectedIndex = selectedIndex - 1;
        $("#readings").dataTable({
            colReorder: true,
            dom: 'Bfrtip',
            "orderMulti": true, 
            "ajax": {
                "url": "/SensorEdit/Resultgrid",
                "type": "GET",
                "datatype": "json",
            },
            "columnDefs": [
         { "visible": false, "targets": selectedIndex }
            ],
            "order": [[selectedIndex, 'asc']],
            "displayLength": 25,
            "drawCallback": function (settings) {
                var api = this.api();
                var rows = api.rows({ page: 'current' }).nodes();
                var last = null;

                api.column(selectedIndex, { page: 'current' }).data().each(function (group, i) {
                    var toShow = selectedText + " : " + ConvertJsonDateString(group);
                    if (last !== group) {
                        $(rows).eq(i).before(
                            '<tr class="group"><td colspan="5">' + toShow + '</td></tr>'
                        );

                        last = group;
                    }
                });
            },          
            "aoColumnDefs":
                [
                    {
                        "targets": [0],  
                        "render": function (data, type, row) {                    
                            retDate = ConvertJsonDateString(data);
                            return (retDate);
                        },
                    },
                ],

            "columns":
            [
                { "data": "MessageDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PlotValues", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PlotLabels", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ]

        });
    }

}