var sparklineOptions = {
    responsive: true,
    legend: {
        display: false
    },
    elements: {
        line: {
            borderColor: '#fff',
            borderWidth: 3
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
var figridurl = '../FleetIssue/Index?page=Fleet_Issues';
var figridName = 'FleetIssue_Search';
var fsgridurl = '../FleetService/Index?page=Fleet_Service';
var fsgridName = 'FleetService_Search';
var partgridName = 'Part_Search';
var pardgridurl = '../Parts/Index?page=Inventory_Part';
$(document).ready(function () {
    GetOpenServiceOrdersCountsWidget();
    GetOpenFleetIssuesCountsWidget();
    GetPastDueServiceCountsWidget();
    LowParts_SparklineChartWidget();
});
//#region Open Service Order Count
function GetOpenServiceOrdersCountsWidget() {
    $.ajax({
        url: '/Dashboard/GetOpenServiceOrdersCounts',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#OpenServiceOrderCount').text(data);
        },
        complete: function () {
            $('#openserviceordspchartloader').hide();
        }
    });
}
//#endregion

//#region Open Fleet Issues Count
function GetOpenFleetIssuesCountsWidget() {
    $.ajax({
        url: '/Dashboard/GetOpenFleetIssuesCounts',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#OpenFleetIssuesCount').text(data);
        },
        complete: function () {
            $('#openissuesspchartloader').hide();
        }
    });
}
//#endregion
//#region Past Due Service Count
function GetPastDueServiceCountsWidget() {
    $.ajax({
        url: '/Dashboard/GetPastDueServiceCounts',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#PastDueServiceCount').text(data);
        },
        complete: function () {
            $('#openpastduespchartloader').hide();
        }
    });
}
//#endregion

//#region Low Parts Count
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
//#endregion

//#region Redirect To Specific Grid
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

$(document).on('click', '#OpenServiceOrderCount,#openServiceOrderCountText', function () {
    localStorage.setItem("ServiceOrderstatus", 3);
    setGridStateRedirectToGrid(fsgridName, fsgridurl);
});

$(document).on('click', '#OpenFleetIssuesCount,#openFleetIssuesCountText', function () {
    localStorage.setItem("FLEETISSUESEARCHGRIDDISPLAYSTATUS", 1);
    setGridStateRedirectToGrid(figridName, figridurl);
});

$(document).on('click', '#lowPartsCount,#lowPartstxt', function () {
    localStorage.setItem("CURRENTTABSTATUS", "4");
    setGridStateRedirectToGrid(partgridName, pardgridurl);
});

//#endregion
