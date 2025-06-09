function openCity2(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent2");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks2");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}
$(document).ready(function () {
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
});
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $(document).find(".tabsArea").hide();
    $(document).find("ul.vtabs li:first").addClass("active").show();
    $(document).find(".tabsArea:first").show();
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity2(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
    //#region Client Edit

    $(document).on('click', "#btnCancelEditClient", function () {
        var clientid = $(document).find('#ClientModel_ClientId').val();
        swal(CancelAlertSetting, function () {
            RedirectToClientDetail(clientid, "client");
        });
    });
    //#endregion
});
$(document).on('click', '.lithisclient', function () {
    var ClientId = $(this).attr('data-val');
    RedirectToClientDetail(ClientId, "client");
});

















