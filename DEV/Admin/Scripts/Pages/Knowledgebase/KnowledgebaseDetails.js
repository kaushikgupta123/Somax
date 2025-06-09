var dtTable;
var _ObjectId;
var colorArray = ["#fe0000", "#ff7f00", "#fffe01", "#00bd3f", "#0068ff", "#7a01e6", "#d300c9", "#940100", "#066d7c", "#66cbff"];

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
function openCity(evt, cityName) {
    evt.preventDefault();
    $('#overviewcontainer').hide();
    $('#Equipmenttab').hide();
    switch (cityName) {
        case "Children":
            generateChildrenDataTable();
            hideAudit();
            break;
        case "Notes":
            generateNoteDataTable();
            hideAudit();
            break;
        case "Attachment":
            generateAttachmentDataTable();
            hideAudit();
            break;
        case "divSensors":
            generateSensorDataTable();
            hideAudit();
            break;
        case "TechSpecs":
            generateTechSpecsTable();
            hideAudit();
            break;
        case "PartsContainer":
            generatePartsDataTable();
            hideAudit();
            break;
        case "Downtime":
            generateDowntimeDataTable();
            hideAudit();
            break;
        case "PMList":
            generatePMDataTable();
            hideAudit();
            break;
        case "WOActive":
            generateWOActiveDataTable();
            $(document).find(".sidebar").mCustomScrollbar({
                theme: "minimal"
            });
            hideAudit();
            break;
        case "WOComplete":
            generateWOCompleteTable();
            $(document).find(".sidebar").mCustomScrollbar({
                theme: "minimal"
            });
            hideAudit();
            break;
        case "PartIssues":
            generatePartIssueTable();
            $(document).find(".sidebar").mCustomScrollbar({
                theme: "minimal"
            });
            hideAudit();
            break;
        case "Equipment":
            $('#overviewcontainer').show();
            $('#Equipmenttab').show();
            $('.tabcontent2').show();
            $('#btnIdentification').addClass('active');
            ShowAudit();
            break;
    }
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
    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
}
function hideAudit() {
    $('#btnnblock').removeClass('col-xl-6').addClass('col-xl-12');
    $('#auditlogcontainer').removeClass('col-xl-6').addClass('col-xl-12').hide();
}
function ShowAudit() {
    $('#btnnblock').removeClass('col-xl-12').addClass('col-xl-6');
    $('#auditlogcontainer').removeClass('col-xl-12').addClass('col-xl-6').show();
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
    var fontSize = 12;
    Chart.defaults.global.defaultFontColor = '#575962';
    Chart.defaults.global.defaultFontFamily = "Roboto";
  
    $(document).find(".tabsArea").hide();
    $(document).find("ul.vtabs li:first").addClass("active").show();
    $(document).find(".tabsArea:first").show();
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });

    $('#tabselector2').on('change', function (evt) {
        var cityName = $(this).val();
        openCity(evt, cityName);
    });
});




function clearDropzone() {
    deleteServer = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        Dropzone.forElement("div#dropzoneForm").destroy();
    }
}






