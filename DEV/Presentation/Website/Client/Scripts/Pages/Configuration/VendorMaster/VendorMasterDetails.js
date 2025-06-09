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