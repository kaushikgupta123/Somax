$(document).on('click', "ul.vtabs li", function () {
    if ($(this).find('#drpDwnLink').length > 0) {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        return false;
    }
    else {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        $(".tabsArea").hide();
        var activeTab = $(this).find("a").attr("href");
        $(activeTab).fadeIn();
        return false;
    }
});
$(document).on('change', '#colorselector', function (evt) {
    $(document).find('.tabsArea').hide();
    openCity(evt, $(this).val());
    $('#' + $(this).val()).show();
});
function RedirectToPMDetail(PartMasterId, mode) {
    if (PartMasterId == 0) {
        window.location.href = "/PartMaster/index?page=Masters_Part_Part_Master";
    }
    else {
        $.ajax({
            url: "/PartMaster/PartMasterDetails",
            type: "GET",
            dataType: "html",
            data: { partMasterId: PartMasterId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#renderpartmaster').html(data);
            },
            complete: function () {
                CloseLoader();               
                SetPartMasterControl();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
}