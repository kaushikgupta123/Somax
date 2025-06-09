var doughnut2dConfig = {
    renderAt: "",
    type: "doughnut2d",
    width: "100%",
    height: "100%",
    dataFormat: "json",
    dataSource: {
        "chart": '',
        "data": ''
    }
}
$(function () {
    $(document).find('.select2picker').select2({});
    SetFunsionChartGlobalSettings();
    GenerateSeatAvailability();
    GenerateActivityCharts();
    if (SiteControl === 'False' && SuperUser === 'True')
        GenerateEngagementsCharts();
});

$("#siteselector").change(function () {
    GenerateSeatAvailability();
    GenerateActivityCharts();
    if (SiteControl === 'False' && SuperUser === 'True')
        GenerateEngagementsCharts();
});
//#region SeatAvailability
function GenerateSeatAvailability() {
    $('#seatavailablechart').html('');
    $('#totalseatsCount').text('');
    $('#seatsAvailableCount').text('');
    $('#activeUserCount').text('');
    $('#workRequestUserCount').text('');
    $('.available-user-loader').show();
    var siteid = $("#siteselector").val();
    $.ajax({
        type: "GET",
        url: "/Configuration/GetAvailabilityChartData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { SiteId: siteid },
        success: function (data) {
            SetUserSeatsCount(data);
            if (data.data.length > 0) {
                GenerateDoughnutChart(data, 'seatavailablechart');
            }
            else {
                $('#availabilitynodata').show();
            }
        },
        complete: function () {
            $('.available-user-loader').hide();
        }
    });
}
function SetUserSeatsCount(data) {
    $('#totalseatsCount').text(data.TotalSeats);
    $('#seatsAvailableCount').text(data.SeatsAvailable);
    $('#activeUserCount').text(data.ActiveUser);
    $('#workRequestUserCount').text(data.WorkRequestUser);
}
//#endregion

//#region Activity
function GenerateActivityCharts() {
    $('#activitylinechart,#activitydoughnut2dchart').html('');
    $('.activity-chart-loader').show();
    $('#activitydoughnut2dnodata').hide();
    $('#activitylinechartnodata').hide();
    var siteid = $("#siteselector").val();
    var caseno = $('#activity-daterangeselector').val();
    $.ajax({
        type: "GET",
        url: "/Configuration/GetActivityMultiSeriesData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { SiteId: siteid, CaseNo: caseno },
        success: function (data) {
            var seriesdata = data.seriesdata;
            var doughnutdata = data.doughnutdata;

            if (seriesdata.categories != null && seriesdata.categories.length > 0) {
                if (seriesdata.categories[0].category != null && seriesdata.categories[0].category.length > 0) {
                    GenerateSeriecChart(seriesdata);
                }
                else {
                    $('#activitylinechartnodata').show();
                }
            }
            else {
                $('#activitylinechartnodata').show();
            }

            if (doughnutdata.data.length > 0) {
                var checkVal = doughnutdata.data.filter(function (x) { return x.value > 0 }).length;
                if (checkVal > 0) {
                    GenerateDoughnutChart(doughnutdata, 'activitydoughnut2dchart');
                }
                else {
                    $('#activitydoughnut2dnodata').show();
                }
            }
            else {
                $('#activitydoughnut2dnodata').show();
            }
        },
        complete: function () {
            $('.activity-chart-loader').hide();
        }
    });
}
function GenerateSeriecChart(data) {
    const chartConfigs = {
        renderAt: "activitylinechart",
        type: "msline",
        width: "100%",
        height: "100%",
        dataFormat: "json",
        dataSource: {
            "chart": data.chart,
            "categories": data.categories,
            "dataset": data.dataset,
        }
    }
    new FusionCharts(chartConfigs).render();
}
function GenerateDoughnutChart(data, renderAt) {
    doughnut2dConfig.renderAt = renderAt;
    doughnut2dConfig.dataSource.chart = data.info;
    doughnut2dConfig.dataSource.data = data.data;
    const chartConfigs = doughnut2dConfig;
    new FusionCharts(chartConfigs).render();
}
$('#activity-daterangeselector').change(function () {
    GenerateActivityCharts();
});
//#endreion

//#region Engagement
function GenerateEngagementsCharts() {
    $('#engagementchart').html('');
    $('#engagement-chart-loader').show();
    var siteid = $("#siteselector").val();
    var caseno = $('#engagement-daterangeselector').val();
    $.ajax({
        type: "GET",
        url: "/Configuration/GetActivityEngagementData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { SiteId: siteid, CaseNo: caseno },
        success: function (data) {
            if (data.dataset.length > 0) {
                const chartConfigs = {
                    renderAt: "engagementchart",
                    type: "scrollbar2d",
                    width: "100%",
                    height: "100%",
                    dataFormat: "json",
                    dataSource: {
                        "chart": data.chart,
                        "categories": data.categories,
                        "dataset": data.dataset
                    }
                }
                new FusionCharts(chartConfigs).render();
            }
            else {
                $('#engagementchartnodata').show();
            }
        },
        complete: function () {
            $('#engagement-chart-loader').hide();
        }
    });
}
$('#engagement-daterangeselector').change(function () {
    GenerateEngagementsCharts();
});
//#endregion
