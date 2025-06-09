$(document).ready(function () {
    GetHozBarCountWidget();
});
function GetHozBarCountWidget() {
    $.ajax({
        url: '/Dashboard/GetHozBarCount',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (data) {
            $('#OWOCount').text(data.TotalOpenCount);
            $('#WRCount').text(data.OpenAssetCount);
            $('#OPMCount').text(data.MonitoredAssetCount);
        }
    });
}