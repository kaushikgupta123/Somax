var colorArray = ['#fe0000', '#ff7f00', '#fffe01', '#00bd3f', '#0068ff', '#7a01e6', '#d300c9', '#940100', '#066d7c', '#66cbff'];
var wogridurl = '../WorkOrder/Index?page=Maintenance_Work_Order_Search';
var pardgridurl = '../Parts/Index?page=Inventory_Part';
var wogridName = 'WorkOrder_Search';
var partgridName = 'Part_Search';

$(document).ready(function () {
    OpenWO_SparklineChartWidget();
    WorkRequest_SparklineChartWidget();
    OverDuePM_SparklineChartWidget();
    LowParts_SparklineChartWidget();
});

$(document).on('click', '#openWoCount,#openWoCountText', function () {
    localStorage.setItem("workorderstatus", 3);
    setGridStateRedirectToGrid(wogridName, wogridurl);
});
function OpenWO_SparklineChartWidget() {
    $.ajax({
        type: "GET",
        url: "/DashBoard/LoadingOpenWoSpChart",
        dataType: "JSON",
        success: function (data) {
            if ($('#openWoCount').length > 0) {
                $('#openWoCount').text(data.OpenWoCount);
            }
            else if ($('#openWoCountInactive').length > 0) {
                $('#openWoCountInactive').text(data.OpenWoCount);
            }
            if ($('#wrCount').length > 0) {
                $('#wrCount').text(data.wrCount);
            }
            else if ($('#wrCountInactive').length > 0) {
                $('#wrCountInactive').text(data.wrCount);
            }
            if (data.metricsValueList.length > 0) {
                var ctx = document.getElementById('myChart_OpenWo').getContext('2d');
                var chart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: data.dataDateList,
                        datasets: [{
                            data: data.metricsValueList,
                            label: "",
                            fill: false,
                            borderColor: "#fff"
                        }]
                    },
                    options: sparklineOptions
                });
            }
            //else {
            //    $('#myChart_OpenWo').prev().show();
            //}
        },
        error: function (event, xhr, options, exc) {
            $('#openwospchartloader').hide();
            if (xhr.status === 401) {
                window.location.href = "/";
            }
        },
        complete: function () {
            $('#openwospchartloader').hide();
        }
    });
}

$(document).on('click', '#wrCount,#wrCountText', function () {
    localStorage.setItem('workorderstatus', 2);
    setGridStateRedirectToGrid(wogridName, wogridurl);
});
function WorkRequest_SparklineChartWidget() {
    $.ajax({
        type: "GET",
        url: "/DashBoard/LoadingWorkRequestSpChart",
        dataType: "JSON",
        success: function (data) {
            if (data.metricsValueList.length > 0) {
                var ctx = document.getElementById('myChart_WR').getContext('2d');
                var chart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: data.dataDateList,
                        datasets: [{
                            data: data.metricsValueList,
                            label: "",
                            fill: false,
                            borderColor: "#fff",
                        }]
                    },
                    options: sparklineOptions
                });
            }
            //else {
            //    $('#myChart_WR').prev().show();
            //}
        },
        error: function (a, b, c) {
            $('#workreqspchartloader').hide();
            if (xhr.status === 401) {
                window.location.href = "/";
            }
        },
        complete: function () {
            $('#workreqspchartloader').hide();
        }
    });
}

$(document).on('click', '#overDuePmCount,#overDuePmCountCountText', function () {
    localStorage.setItem("workorderstatus", 24);
    setGridStateRedirectToGrid(wogridName, wogridurl);
});
function OverDuePM_SparklineChartWidget() {
    $.ajax({
        type: "GET",
        url: "/DashBoard/LoadingOverDuePmSpChart",
        dataType: "JSON",
        success: function (data) {
            $('#overDuePmCount').append(data.overDuePmCount);
            if (data.metricsValueList.length > 0) {
                var ctx = document.getElementById('myChart_OverDuePm').getContext('2d');
                var chart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: data.dataDateList,
                        datasets: [{
                            data: data.metricsValueList,
                            label: "",
                            fill: false,
                            borderColor: "#fff",
                        }]
                    },
                    options: sparklineOptions
                });
            }
            //else {
            //    $('#myChart_OverDuePm').prev().show();
            //}
        },
        error: function (a, b, c) {
            $('#overduepmspchartloader').hide();
            if (xhr.status === 401) {
                window.location.href = "/";
            }
        },
        complete: function () {
            $('#overduepmspchartloader').hide();
        }
    });
}

$(document).on('click', '#lowPartsCount,#lowPartstxt', function () {
    localStorage.setItem("CURRENTTABSTATUS", "4");
    setGridStateRedirectToGrid(partgridName, pardgridurl);
});
function LowParts_SparklineChartWidget() {
    $.ajax({
        type: "GET",
        url: "/DashBoard/LoadingLowPartsSpChart",
        dataType: "JSON",
        success: function (data) {
            if ($('#lowPartsCount').length > 0) {
                $('#lowPartsCount').append(data.lowPartsCount);
            }
            else if ($('#lowPartsCountInactive').length > 0) {
                $('#lowPartsCountInactive').append(data.lowPartsCount);
            }
            if (data.metricsValueList.length > 0) {
                var ctx = document.getElementById('myChart_LowParts').getContext('2d');
                var chart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: data.dataDateList,
                        datasets: [{
                            data: data.metricsValueList,
                            label: "",
                            fill: false,
                            borderColor: "#fff"
                        }]
                    },
                    options: sparklineOptions
                });
            }
            //else {
            //    $('#myChart_LowParts').prev().show();
            //}
        },
        error: function (a, b, c) {
            $('#lowpartspchartloader').hide();
            if (xhr.status === 401) {
                window.location.href = "/";
            }
        },
        complete: function () {
            $('#lowpartspchartloader').hide();
        }
    });
}

var sparklineOptions = {
    responsive: true,
    legend: {
        display: false
    },
    elements: {
        line: {
            borderColor: '#fff',
            borderWidth: 3,
        },
        point: {
            radius: 0
        }
    },
    scales: {
        yAxes: [
            {
                display: false
            }
        ],
        xAxes: [
            {
                display: false
            }
        ]
    }
};

function setGridStateRedirectToGrid(GridName, url) {
    $.ajax({
        "url": gridStateLoadUrl,
        "data": { GridName: GridName },
        "dataType": "json",
        "success": function (json) {
            if (json && json != null && json.length > 0) {
                var gridstate = JSON.parse(json);
                gridstate.start = 0;
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": { GridName: GridName, LayOutInfo: JSON.stringify(gridstate) },
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { window.location.href = url; }
                });
            }
            else {
                window.location.href = url;
            }            
        }
    });
}