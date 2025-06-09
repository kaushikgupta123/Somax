$(document).ready(function () {
    GetOpenJobsCountsWidget();
    GetRequestsCountsWidget();
    GetOverdueJobsCountsWidget();
});
function GetOpenJobsCountsWidget() {
    $.ajax({
        url: '/Dashboard/GetOpenJobsCounts',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#OpenJobsCount').text(data);
        }
    });
}
function GetRequestsCountsWidget() {
    $.ajax({
        url: '/Dashboard/GetRequestsCounts',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#RequestCount').text(data);
        }
    });
}
function GetOverdueJobsCountsWidget() {
    $.ajax({
        url: '/Dashboard/GetOverdueJobsCounts',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#OverDueJobsCount').text(data);
        }
    });
}