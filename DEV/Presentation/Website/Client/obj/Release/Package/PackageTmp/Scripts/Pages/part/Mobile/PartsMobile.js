var order = '1';
var orderDir = 'asc';
var gridName = "Part_Mobile_Search";
var titleText = "";
var DefaultLayoutInfo = '{"time":currentTime,"start":0,"length":10,"order":[[1,"asc"]],"search":{"search":"","smart":true,"regex":false,"caseInsensitive":true},"columns":[],"ColReorder":[]}';
var ZoomConfig = { zoomType: "window", lensShape: "round", lensSize: 1000, zoomWindowFadeIn: 500, zoomWindowFadeOut: 500, lensFadeIn: 100, lensFadeOut: 100, easing: true, scrollZoom: true, zoomWindowWidth: 450, zoomWindowHeight: 450 };
var run = false;

var cardviewstartvalue = 0;
var cardviewlength = 10;
var grdcardcurrentpage = 1;
var CustomQueryDisplayId;

var SortByDropdown;
var CustomQueryDropdown;
var dtpNotesTable_Mobile;
var dtpAttachmentTable_Mobile;
var dtpVendorTable_Mobile;
var dtpEquipmentTable_Mobile;
var dtpReviewSiteTable_Mobile;
var selectCount = 0;
$(document).ready(function () {
    var MaintCompstatus = localStorage.getItem("PARTMOBILESTATUS");
    if (MaintCompstatus) {
        CustomQueryDisplayId = MaintCompstatus;
    }
    else {
        MaintCompstatus = "1";
        CustomQueryDisplayId = MaintCompstatus;
        localStorage.setItem("PARTMOBILESTATUS", "1");
    }
    $("#CustomQueryDropdown li").removeClass('active');
    $("#CustomQueryDropdown li[data-value='" + CustomQueryDisplayId + "']").addClass('active');
    titleText = $("#CustomQueryDropdown li[data-value='" + CustomQueryDisplayId + "']").text();
    $('#partsearchtitle').text(titleText);
    GetDatatableLayout();
    ShowCardView();

    ////--- debashis
    mobiscroll.settings = {
        lang: 'en',                                       // Specify language like: lang: 'pl' or omit setting to use default
        theme: 'ios',                                     // Specify theme like: theme: 'ios' or omit setting to use default
        themeVariant: 'light'                             // More info about themeVariant: https://docs.mobiscroll.com/4-10-9/javascript/popup#opt-themeVariant
    };

    CustomQueryDropdown = $('#CustomQueryDropdownDiv').mobiscroll().popup({
        display: 'bubble',
        anchor: '#ShowCustomQueryDropdown',
        buttons: [],
        cssClass: 'mbsc-no-padding md-vertical-list'
    }).mobiscroll('getInst');
    SortByDropdown = $('#SortByDropdownDiv').mobiscroll().popup({
        display: 'bubble',
        anchor: '#ShowSortByDropdown',
        buttons: [],
        cssClass: 'mbsc-no-padding md-vertical-list'
    }).mobiscroll('getInst');

    $('#CustomQueryDropdown').mobiscroll().listview({
        enhance: true,
        swipe: false,
    });
    $('#SortByDropdown').mobiscroll().listview({
        enhance: true,
        swipe: false
    });

    $('#ShowCustomQueryDropdown').click(function () {
        CustomQueryDropdown.show();
        return false;
    });
    $('#ShowSortByDropdown').click(function () {
        SortByDropdown.show();
        return false;
    });

    ////--- debashis
    $(document).find('.select2picker').select2({});
    $(document).find('.mobiscrollselect:not(:disabled)').mobiscroll().select({
        display: 'bubble',
        //touchUi: false,
        /*data: remoteData,*/
        filter: true,
        group: {
            groupWheel: false,
            header: false
        },
        //width: 400,
        //placeholder: 'Please Select...'

    });
    $(document).find('.mobiscrollselect:disabled').mobiscroll().select({
        disabled: true
    });
    SetFixedHeadStyle();
});

function ShowCardView() {
    $.ajax({
        "url": "/Parts/GetPartsMainGridMobile",
        type: 'POST',
        dataType: 'html',
        //contentType: "application/json; charset=utf-8",
        data: {
            currentpage: grdcardcurrentpage,
            start: cardviewstartvalue,
            length: cardviewlength,
            CustomQueryDisplayId: CustomQueryDisplayId,
            SearchText: LRTrim($(document).find('#txtColumnSearch').val()),
            PartID: LRTrim($("#PartID").val()),
            Description: LRTrim($("#Description").val()),
            Manufacturer: LRTrim($("#Manufacturer").val()),
            ManufacturerID: LRTrim($('#ManufacturerID').val()),
            Section: LRTrim($("#Section").val()),
            Row: LRTrim($('#Row').val()),
            Shelf: LRTrim($('#Shelf').val()),
            Bin: LRTrim($('#Bin').val()),
            PlaceArea: LRTrim($('#PlaceArea').val()),
            StockType: LRTrim($('#StockType').val()),
            Order: order,
            orderDir: orderDir
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#ActiveCard').html(data).show();
            $(document).find('#tblparts_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == grdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
        },
        complete: function () {
            var sortClass = '';
            if (orderDir == 'asc') {
                sortClass = 'sorting_asc_mobile';
            }
            else if (orderDir == 'desc') {
                sortClass = 'sorting_desc_mobile';
            }
            $(document).find('#SortByDropdown li').removeClass('active sorting_asc_mobile sorting_desc_mobile');
            $(document).find('#SortByDropdown li[data-value="' + order + '"]').addClass('active').addClass(sortClass);
            $(document).find('#searchcardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(cardviewlength).trigger('change.select2');
            HidebtnLoader("SrchBttnNew");
            HidebtnLoader("txtColumnSearch");

            //#region V2-886 Page Navigation Show Hide
            if ($(document).find('#spntotalpages').text() <= 1) {
                $(document).find('.pagenavdiv').hide();
            }
            else {
                $(document).find('.pagenavdiv').show();
            }
            //#endregion
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', '#tblparts_paginate .paginate_button', function () {
    //var currentselectedpage = parseInt($(document).find('#tblparts_paginate .pagination').find('.active').text());
    //cardviewlength = $(document).find('#searchcardviewpagelengthdrp').val();
    //cardviewstartvalue = cardviewlength * (parseInt($(this).find('.page-link').text()) - 1);
    //var lastpage = parseInt($(this).prev('li').data('currentpage'));
    //if ($(this).attr('id') == 'tbl_previous') {
    //    if (currentselectedpage == 1) {
    //        return false;
    //    }
    //    cardviewstartvalue = cardviewlength * (currentselectedpage - 1);
    //    grdcardcurrentpage = grdcardcurrentpage - 1;
    //}
    //else if ($(this).attr('id') == 'tbl_next') {
    //    if (currentselectedpage == lastpage) {
    //        return false;
    //    }
    //    cardviewstartvalue = cardviewlength * (currentselectedpage - 1);
    //    grdcardcurrentpage = grdcardcurrentpage + 1;
    //}
    //else if ($(this).attr('id') == 'tbl_first') {
    //    if (currentselectedpage == 1) {
    //        return false;
    //    }
    //    grdcardcurrentpage = 1;
    //    cardviewstartvalue = 0;
    //}
    //else if ($(this).attr('id') == 'tbl_last') {
    //    if (currentselectedpage == lastpage) {
    //        return false;
    //    }
    //    grdcardcurrentpage = parseInt($(this).prevAll('li').eq(1).text());
    //    cardviewstartvalue = cardviewlength * (grdcardcurrentpage - 1);
    //}
    //else {
    //    grdcardcurrentpage = $(this).data('currentpage');
    //}
    //#region 886
    var currentselectedpage = parseInt($(document).find('#txtcurrentpage').val());
    cardviewlength = $(document).find('#searchcardviewpagelengthdrp').val();
    cardviewstartvalue = cardviewlength * (currentselectedpage - 1);
    var lastpage = parseInt($(document).find('#spntotalpages').text());

    if ($(this).attr('id') == 'tbl_previous') {
        if (currentselectedpage == 1) {
            return false;
        }
        grdcardcurrentpage = grdcardcurrentpage - 1;
        cardviewstartvalue = cardviewlength * (grdcardcurrentpage - 1);
    }
    else if ($(this).attr('id') == 'tbl_next') {
        if (currentselectedpage == lastpage) {
            return false;
        }
        grdcardcurrentpage = grdcardcurrentpage + 1;
        cardviewstartvalue = cardviewlength * (grdcardcurrentpage - 1);
    }
    else if ($(this).attr('id') == 'tbl_first') {
        if (currentselectedpage == 1) {
            return false;
        }
        grdcardcurrentpage = 1;
        cardviewstartvalue = 0;
    }
    else if ($(this).attr('id') == 'tbl_last') {
        if (currentselectedpage == lastpage) {
            return false;
        }
        grdcardcurrentpage = lastpage;
        cardviewstartvalue = cardviewlength * (grdcardcurrentpage - 1);
    }
    //#endregion
    LayoutUpdate('pagination');
    ShowCardView();
});
$(document).on('change', '#searchcardviewpagelengthdrp', function () {
    cardviewlength = $(this).val();
    grdcardcurrentpage = parseInt(cardviewstartvalue / cardviewlength) + 1;
    cardviewstartvalue = parseInt((grdcardcurrentpage - 1) * cardviewlength);

    LayoutUpdate('pagelength');
    ShowCardView();
});
$(document).find('#SortByDropdown li').on('click', function () {
    $(this).addClass('active').siblings().removeClass('active');
    SortByDropdown.hide();

    if (order == $(this).data('value')) {
        if (orderDir == 'asc') {
            orderDir = 'desc';
        }
        else if (orderDir == 'desc') {
            orderDir = 'asc';
        }
    }
    else {
        orderDir = 'asc';
    }

    order = $(this).data('value');
    grdcardcurrentpage = 1;
    cardviewstartvalue = 0;

    LayoutUpdate('column');
    ShowCardView();
});
//#region Search
$(document).mouseup(function (e) {
    var container = $(document).find('#searchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: gridName },
        beforeSend: function () {
            ShowbtnLoader("SrchBttnNew");
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)" id= "mem_' + i + '"' + '><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchList.innerHTML = str;
            $(document).find('#searchBttnNewDrop').show("slideToggle");
        },
        complete: function () {
            HidebtnLoader("SrchBttnNew");
        },
        error: function () {
            HidebtnLoader("SrchBttnNew");
        }
    });
});
function GenerateSearchList(txtSearchval, isClear) {
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: gridName, searchText: txtSearchval, isClear: isClear },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)"><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchList.innerHTML = str;
        }
        ,
        complete: function () {
            if (isClear == false) {
                cardviewstartvalue = 0;
                grdcardcurrentpage = 1;

                LayoutFilterinfoUpdate();
                ShowCardView();
            }
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('keyup', '#txtColumnSearch', function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == 13) {
        TextSearch();
    }
    else {
        event.preventDefault();
    }
});
function TextSearch() {
    run = true;
    clearAdvanceSearch();
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        cardviewstartvalue = 0;
        cardviewlength = 10;
        grdcardcurrentpage = 1;
        ShowCardView();
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    if (searchtxtId == 'StockType') {
        stockType = null;
    }
    if (selectCount > 0) selectCount--;
    AdvanceSearch();
    cardviewstartvalue = 0;
    grdcardcurrentpage = 1;

    LayoutFilterinfoUpdate();
    ShowCardView();
});
$(document).on('click', '#UlSearchList li', function () {
    var v = LRTrim($(this).text());
    $(document).find('#txtColumnSearch').val(v);
    TextSearch();
});
$(document).on('click', '#cancelText', function () {
    $(document).find('#txtColumnSearch').val('');
});
$(document).on('click', '#btnQrScanner', function () {
    $(document).find('#txtPartId').val('');
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').addClass("slide-active");
        QrScannerSearch_Mobile('txtColumnSearch');
    }
});

//#region V2-763 QRCode scanning for Search
var searchfield = "";
function QrScannerSearch_Mobile(txtID) {
    searchfield = "";
    searchfield = '#' + txtID;
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').addClass("slide-active");
        StartQRScanning_Mobile();
    }

}
function StartQRScanning_Mobile() {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
            }, success => {
                $(document).find(searchfield).val(success);
                TextSearch();
                if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
                    $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
                }
            }, error => {
            });
        } else {
            if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
                $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
            }
        }
    }).catch(e => {
        if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
            $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
        }
        if (e && e.startsWith('NotReadableError')) {
            ShowErrorAlert(getResourceValue("cameraIsBeingUsedByAnotherAppAlert"));
        }
        else if (e && e.startsWith('NotFoundError')) {
            ShowErrorAlert(getResourceValue("cameraDeviceNotFoundAlert"));
        }

    });
}
//#endregion
$(document).on('click', '#closeQrScanner_Mobile', function () {
    if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').removeClass('slide-active');
    }
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').removeClass('slide-active');
        StopCamera(); // using same method from somax_main.js
    }
});
$(document).on('click', '#clearText', function () {
    GenerateSearchList('', true);
});
$(document).on('click', '.txtSearchClickComp', function () {
    TextSearch();
});
//#endregion
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
function getfilterinfoarray(txtsearchelement, advsearchcontainer) {
    var filterinfoarray = [];
    var f = new filterinfo('searchstring', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parents('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);

            if ($(this).parent('div').find('div').hasClass('range-timeperiod')) {
                if ($(this).parent('div').find('input').val() !== '' && $(this).val() == '10') {
                    f = new filterinfo('this-' + $(this).attr('id'), $(this).parent('div').find('input').val());
                    filterinfoarray.push(f);
                }
            }

        }
    });
    return filterinfoarray;
}
function setsearchui(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if (item.key == 'searchstring' && item.value) {
            var txtSearchval = item.value;
            if (item.value) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parents('div').is(":visible")) {
                $('#' + item.key).val(item.value).trigger('change');
                if (item.value) {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("#" + item.key + "").parents('label').children('span').eq(0).text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}

//#region Card view griddatalayout update
//For column , order , page and page length change
function LayoutUpdate(area) {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: gridName
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            if (json.LayoutInfo == '') {
                json.LayoutInfo = DefaultLayoutInfo;
            }
            if (json.LayoutInfo !== '') {
                var gridstate = JSON.parse(json.LayoutInfo);
                gridstate.start = cardviewstartvalue;
                if (area === 'column' || area === 'order') {
                    gridstate.order[0] = [order, orderDir];
                }
                else if (area === 'pagination') {//
                }
                else if (area === 'pagelength' || area === 'optionDropdownChange') {
                    gridstate.length = cardviewlength;
                }

                if (json.FilterInfo !== '') {
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                }

                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: gridName,
                        LayOutInfo: JSON.stringify(gridstate)
                    },
                    "async": false,
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
        }
    });
}
function LayoutFilterinfoUpdate() {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: gridName
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            if (json.LayoutInfo !== '') {
                var gridstate = JSON.parse(json.LayoutInfo);
                gridstate.start = cardviewstartvalue;
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: gridName,
                        LayOutInfo: JSON.stringify(gridstate),
                        FilterInfo: JSON.stringify(filterinfoarray)
                    },
                    "async": false,
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
        }
    });
}
function GetDatatableLayout() {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: gridName
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            if (json.LayoutInfo !== '') {
                var LayoutInfo = JSON.parse(json.LayoutInfo);
                var pageclicked = (LayoutInfo.start / LayoutInfo.length);
                cardviewlength = LayoutInfo.length;
                cardviewstartvalue = cardviewlength * pageclicked;
                grdcardcurrentpage = pageclicked + 1;
                order = LayoutInfo.order[0][0];
                orderDir = LayoutInfo.order[0][1];

                if (json.FilterInfo !== '') {
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                }
            }
            else {
                DefaultLayoutInfo = DefaultLayoutInfo.replace('currentTime', new Date().getTime());
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: gridName,
                        LayOutInfo: (DefaultLayoutInfo),
                        FilterInfo: JSON.stringify(filterinfoarray)
                    },
                    "async": false,
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
        }
    });
}
$(document).on('click', '.lnk_part_mobile', function (e) {
    e.preventDefault();
    var PartId = $(this).attr('id');
    //var ClientLookupId = $(this).attr('clientlookupid');
    $.ajax({
        url: "/Parts/PartDetailsDynamic_Mobile",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { PartId: PartId },
        success: function (data) {
            $('#renderparts').html(data);
            $(document).find('#spnlinkToSearch').text(titleText);
        },
        complete: function () {
            SetPartDetailEnvironment();
            SetFixedHeadStyle();
        }
    });

});
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../Parts/Index?page=Inventory_Part";
});
//#endregion
//#region View dropdown change
$(document).on('click', '#CustomQueryDropdown li', function (e) {
    if ($(this).hasClass('active') == true) {
        CustomQueryDropdown.hide();
        return false;
    }

    $(this).addClass('active').siblings().removeClass('active');
    CustomQueryDropdown.hide();

    $(".updateArea").hide();
    $(".actionBar").fadeIn();

    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).data('value');
    localStorage.setItem("PARTMOBILESTATUS", optionval);
    CustomQueryDisplayId = optionval;
    titleText = $("#CustomQueryDropdown li[data-value='" + CustomQueryDisplayId + "']").text();
    $('#partsearchtitle').text(titleText);

    if ($(document).find('#txtColumnSearch').val() !== '') {
        $('#advsearchfilteritems').find('span').html('');
        $('#advsearchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnSearch').val('');


    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        cardviewstartvalue = 0;
        grdcardcurrentpage = 1;
        LayoutFilterinfoUpdate();
        ShowCardView();

    }
});
//#endregion
//#region Common
function SetControls() {
    var errClass = 'mobile-validation-error';
    CloseLoader();
    $.validator.setDefaults({
        ignore: null,
        //errorClass: "mobile-validation-error", // default values is input-validation-error
        //validClass: "valid", // default values is valid
        highlight: function (element, errorClass, validClass) { //for the elements having error
            $(element).addClass(errClass).removeClass(validClass);
            $(element.form).find("#" + element.id).parent().parent().addClass("mbsc-err");
            var elemName = $(element.form).find("#" + element.id).attr('name');
            $(element.form).find('[data-valmsg-for="' + elemName + '"]').addClass("mbsc-err-msg");
        },
        unhighlight: function (element, errorClass, validClass) { //for the elements having not any error
            $(element).removeClass(errClass).addClass(validClass);
            if (element.id != "") {
                $(element.form).find("#" + element.id).parent().parent().removeClass("mbsc-err");
                var elemName = $(element.form).find("#" + element.id).attr('name');
                $(element.form).find('[data-valmsg-for="' + elemName + '"]').removeClass("mbsc-err-msg");
            }

        },
    });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        if ($(this).closest('form').length > 0) {
            $(this).valid();
        }
    });
    //$('.mobiscrollselect, form').change(function () {
    //    debugger;
    //    $(this).find('label.mbsc-err').removeClass('mbsc-err');
    //    if ($(this).valid() == false) {
    //        $(this).find('.input-validation-error').each(function () {
    //            $(this).parents('label').eq(0).addClass('mbsc-err');
    //        });
    //    }
    //});
    //$('.select2picker, form').change(function () {
    //$('form').change(function () {
    //    $(this).valid();
    //});
    $(document).find('.select2picker').select2({});
    $(document).find('.mobiscrollselect:not(:disabled)').mobiscroll().select({
        display: 'bubble',
        //touchUi: false,
        /*data: remoteData,*/
        filter: true,
        group: {
            groupWheel: false,
            header: false
        },
        //width: 400,
        //placeholder: 'Please Select...'

    });
    $(document).find('.mobiscrollselect:disabled').mobiscroll().select({
        disabled: true
    });
    SetFixedHeadStyle();
}
function ZoomImage(element) {
    element.elevateZoom(ZoomConfig);
}
function SetPartDetailEnvironment() {
    CloseLoader();
    ZoomImage($(document).find('#EquipZoom'));

    SetFixedHeadStyle();
    $('#tabscroll').mobiscroll().nav({
        type: 'tab'
    });
}
//#endregion
//#region Add
$(document).on('click', "#AddPartBtn", function (e) {
    $.ajax({
        url: "/Parts/AddPartsDynamic_Mobile",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddPartDiv').html(data);
        },
        complete: function () {
            SetControls();
            SetControlsForAddPart();
            $('#AddPartModalDialog').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
});
function SetControlsForAddPart() {
    $('.dtpicker').mobiscroll().calendar({
        display: 'bottom',
        touchUi: true,
        weekDays: 'min',
        yearChange: false,
        min: new Date(),
        months: 1
    }).inputmask('mm/dd/yyyy');
    $('.decimal').mobiscroll().numpad({
        touchUi: true,
        //min: 0.01,
        //max: 99999999.99,
        scale: 2,
        preset: 'decimal',
        thousandsSeparator: ''
    });
}
$(document).on('click', '.btnCancelAddPart', function () {
    window.location.href = "../Parts/Index?page=Inventory_Part";
});
function PartsAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command == "save") {
            var message;
            if (data.mode == "add") {
                localStorage.setItem("PARTMOBILESTATUS", '1');
                SuccessAlertSetting.text = getResourceValue("AddPartsAlerts");
            }
            swal(SuccessAlertSetting, function () {
                titleText = getResourceValue("AlertActive");
                $('#AddPartModalDialog').removeClass('slide-active');
                RedirectToPartDetail(data.PartId);
            });
        }
        else {
            ResetErrorDiv();
            SuccessAlertSetting.text = getResourceValue("AddPartsAlerts");
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change.select2');
                $(document).find('.mbsc-err').removeClass('mbsc-err');
                $(document).find('.mbsc-err-msg').html('');
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region Edit
$(document).on('click', "#editparts", function (e) {
    e.preventDefault();
    var partId = $('#PartModel_PartId').val();
    $.ajax({
        url: "/Parts/EditPartsDynamic_Mobile",
        type: "GET",
        dataType: 'html',
        data: { PartId: partId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#EditPartDiv').html(data);
        },
        complete: function () {
            SetControls();
            $('.dtpicker').mobiscroll().calendar({
                display: 'bottom',
                touchUi: true,
                weekDays: 'min',
                yearChange: false,
                min: new Date(),
                months: 1
            }).inputmask('mm/dd/yyyy');
            $('.decimal').mobiscroll().numpad({
                touchUi: true,
                //min: 0.01,
                //max: 99999999.99,
                scale: 2,
                preset: 'decimal',
                thousandsSeparator: ''
            });
            $('#EditPartModalDialog').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.btnCancelEditPart', function () {
    $('#EditPartModalDialog').removeClass('slide-active');
});
function PartsEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        SuccessAlertSetting.text = getResourceValue("UpdatePartsAlerts");
        swal(SuccessAlertSetting, function () {
            $('#EditPartModalDialog').removeClass('slide-active');
            RedirectToPartDetail(data.partid);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region Details
var tab = "Overview";
function RedirectToPartDetail(PartId, mode) {
    $.ajax({
        url: "/Parts/PartDetailsDynamic_Mobile",
        type: "POST",
        dataType: 'html',
        data: { PartId: PartId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderparts').html(data);
            $(document).find('#spnlinkToSearch').text(titleText);
        },
        complete: function () {
            CloseLoader();

            $(document).find(".m-portlet").find("button.active").removeClass('active');
            $(document).find(".m-portlet").find("[data-tab='" + tab + "']").addClass('active');
            $(document).find("#" + tab).css("display", "block");
            if (tab != 'Overview') {
                ResetOtherTabs();
                loadTabDetails(tab);
            }
            SetPartDetailEnvironment();
            SetFixedHeadStyle();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.wo-det-tab', function (e) {
    if ($(this).hasClass('active')) {
        return false;
    }
    ResetOtherTabs();
    tab = $(this).data('tab');
    loadTabDetails(tab);
    SwitchTab(e, tab);
});
function loadTabDetails(tab) {
    switch (tab) {
        case "Overview":
            LoadOverviewTab();
            break;
        case "Comments":
            LoadCommentsTab();
            break;
        case "Photos":
            LoadPhotosTab();
            break;
        case "Attachments":
            LoadAttachmentsTab();
            break;
        case "Vendors":
            LoadVendorsTab();
            break;
        case "Equipments":
            LoadEquipmentsTab();
            break;
        case "History":
            LoadHistoryTab();
            break;
        case "Receipt":
            LoadReceiptTab();
            break;
        case "Review":
            LoadReviewTab();
            break;
    }
}
function SwitchTab(evt, tab) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(tab).style.display = "block";

    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
    $(document).find('.wo-det-tab').removeClass('mbsc-ms-item-sel');
    $(document).find('#' + tab).addClass('mbsc-ms-item-sel');
}
function ResetOtherTabs() {
    $(document).find('#Overview').html('');
    $(document).find('#Photos').html('');
    $(document).find('#Comments').html('');
}
//#region Overview
function LoadOverviewTab() {
    var PartId = $(document).find('#PartModel_PartId').val();
    $.ajax({
        url: "/Parts/PartDetailsDynamic_Mobile",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { PartId: PartId },
        success: function (data) {
            $('#renderparts').html(data);
            $(document).find('#spnlinkToSearch').text(titleText);
        },
        complete: function () {
            SetPartDetailEnvironment();
            SetFixedHeadStyle();
        }
    });
}
//#endregion
//#region CKEditor
$(document).on("focus", "#parttxtcommentsnew", function () {
    $(document).find('.ckeditorarea').show();
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.ckeditorarea').hide();
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    //ClearEditor();
    LoadCkEditor('parttxtcomments');
    $("#parttxtcommentsnew").hide();
    $(".ckeditorfield").show();
});

$(document).on('click', '#btnsavecommands', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    var partid = $(document).find('#PartModel_PartId').val();
    var PartClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    var noteId = 0;
    if (LRTrim(data) == "") {
        return false;
    }
    else {
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/Parts/AddOrUpdateComment',
            type: 'POST',
            beforeSend: function () {
                ShowLoader();
            },
            data: {
                partid: partid,
                content: data,
                PartClientLookupId: PartClientLookupId,
                userList: selectedUsers,
                noteId: noteId,
            },
            success: function (data) {
                if (data.Result == "success") {
                    var message;
                    if (data.mode == "add") {
                        SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                    }
                    else {
                        SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                    }
                    swal(SuccessAlertSetting, function () {
                        LoadCommentsTab();
                    });
                }
                else {
                    ShowGenericErrorOnAddUpdate(data);
                    CloseLoader();
                }
            },
            complete: function () {
                ClearEditor();
                $("#parttxtcommentsnew").show();
                $(".ckeditorfield").hide();
                selectedUsers = [];
                selectedUnames = [];
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', '#commandCancel', function () {
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    ClearEditor();
    $("#parttxtcommentsnew").show();
    $(".ckeditorfield").hide();
});
$(document).on('click', '.editcomments', function () {
    $(document).find(".ckeditorarea").each(function () {
        $(this).html('');
    });
    $("#parttxtcommentsnew").show();
    $(".ckeditorfield").hide();
    var notesitem = $(this).parents('.kt-notes__item').eq(0);
    notesitem.find('.ckeditorarea').html(CreateEditorHTML('parttxtcommentsEdit'));
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();

    var rawHTML = $.parseHTML($(this).parents('.kt-notes__item').find('.kt-notes__body').find('.originalContent').html());
    LoadCkEditorEdit('parttxtcommentsEdit', rawHTML);
    $(document).find('.ckeditorarea').hide();
    notesitem.find('.ckeditorarea').show();
    notesitem.find('.kt-notes__body').hide();
    notesitem.find('.commenteditdelearea').hide();
});

$(document).on('click', '.deletecomments', function () {
    DeletePartNote($(this).attr('id'));
});
$(document).on('click', '.btneditcomments', function () {
    var data = getDataFromTheEditor();
    var partid = $(document).find('#PartModel_PartId').val();
    var ClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/Parts/AddOrUpdateComment',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { partid: partid, content: LRTrim(data), noteId: noteId, updatedindex: updatedindex },
        success: function (data) {
            if (data.Result == "success") {
                var message;
                if (data.mode == "add") {
                    SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                }
                else {
                    SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                }
                swal(SuccessAlertSetting, function () {
                    //RedirectToPartDetail(partid);
                    LoadCommentsTab();

                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
            }
        },
        complete: function () {
            //ClearEditorEdit();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

});
$(document).on('click', '.btncommandCancel', function () {
    ClearEditorEdit();
    $(document).find('.ckeditorarea').hide();
    $(this).parents('.kt-notes__item').find('.kt-notes__body').show();
    $(this).parents('.kt-notes__item').find('.commenteditdelearea').show();
});
function DeletePartNote(notesId) {
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: '/Base/DeleteComment',
            data: {
                _notesId: notesId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SuccessAlertSetting.text = getResourceValue("CommentDeleteAlert");
                    swal(SuccessAlertSetting, function () {
                        //RedirectToPartDetail($(document).find('#PartModel_PartId').val());
                        LoadCommentsTab();
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion
//#region Comment
var colorarray = [];
function colorobject(string, color) {
    this.string = string;
    this.color = color;
}
function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}
function LoadCommentsTab() {
    var PartId = $(document).find('#PartModel_PartId').val();
    $.ajax({
        url: '/Parts/GetCommentsDetails_Mobile',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Comments').html('');
            $(document).find('#Comments').html(data);
            LoadComments(PartId);
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

}
function LoadComments(PartID) {
    $.ajax({
        "url": "/Parts/LoadComments",
        data: { PartID: PartID },
        type: "POST",
        datatype: "json",
        success: function (data) {
            var getTexttoHtml = textToHTML(data);
            $(document).find('#commentstems').html(getTexttoHtml);
            $(document).find("#commentsList").mCustomScrollbar({
                theme: "minimal"
            });
        },
        complete: function () {
            var ftext = '';
            var bgcolor = '';
            $(document).find('#commentsdataloader').hide();
            $(document).find('#commentstems').find('.comment-header-item').each(function () {
                var thistext = LRTrim($(this).text());
                if (ftext == '' || ftext != thistext) {
                    var bgcolorarr = colorarray.filter(function (a) {
                        return a.string == thistext;
                    });
                    if (bgcolorarr.length == 0) {
                        bgcolor = getRandomColor();
                        var thisval = new colorobject(thistext, bgcolor);
                        colorarray.push(thisval);
                    }
                    else {
                        bgcolor = bgcolorarr[0].color;
                    }
                }
                $(this).attr('style', 'color:' + bgcolor + '!important;border:1px solid' + bgcolor + '!important;');
                ftext = LRTrim($(this).text());
            });
            var loggedinuserinitial = LRTrim($('#hdr-comments-add').text());
            var avlcolor = colorarray.filter(function (a) {
                return a.string == loggedinuserinitial;
            });
            if (avlcolor.length == 0) {
                $('#hdr-comments-add').attr('style', 'border:1px solid #264a7c !important;').show();
            }
            else {
                $('#hdr-comments-add').attr('style', 'color:' + avlcolor[0].color + ' !important;border:1px solid ' + avlcolor[0].color + '!important;').show();
            }
            $('.kt-notes__body a').attr('target', '_blank');
        }
    });
}
//#endregion
//#region Photo
//#region Multiple Photo Upload
function CompressImage(files, imageName) {
    new Compressor(files, {
        quality: 0.6,
        convertTypes: ['image/png'],
        convertSize: 100000,
        // The compression process is asynchronous,
        // which means you have to access the `result` in the `success` hook function.
        success(result) {
            if (result.size < files.size) {
                SaveCompressedImage(result, imageName);
            }
            else {
                SaveCompressedImage(files, imageName);
            }
            console.log('file name ' + result.name + ' after compress ' + result.size);
        },
        error(err) {
            console.log(err.message);
        },
    });
}
function SaveCompressedImage(data, imageName) {
    var AddPhotoFileData = new FormData();
    AddPhotoFileData.append('file', data, imageName);
    $.ajax({
        url: '../base/SaveUploadedFile',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: AddPhotoFileData,
        success: function (result) {
            SaveMultipleUploadedFileToServer(imageName);

            $('#files').val('');
            $('#add_photos').val('');
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}
$(document).on('change', '#files', function () {
    var val = $(this).val();
    var _isMobile = CheckLoggedInFromMob();
    var imageName = val.replace(/^.*[\\\/]/, '')
    var partId = LRTrim($('#PartModel_PartId').val());
    var imgname = partId + "_" + Math.floor((new Date()).getTime() / 1000);
    var fileUpload = $("#files").get(0);
    var files = fileUpload.files;
    var fileExt = imageName.substr(imageName.lastIndexOf('.') + 1).toLowerCase();
    if (fileExt != 'jpeg' && fileExt != 'jpg' && fileExt != 'png' && fileExt != 'JPEG' && fileExt != 'JPG' && fileExt != 'PNG') {
        ShowErrorAlert(getResourceValue("spnValidImage"));
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    else if (this.files[0].size > (1024 * 1024 * 10)) {
        ShowImageSizeExceedAlert();
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    else {
        swal(AddImageAlertSetting, function () {
            if (window.FormData !== undefined) {
                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    console.log('file name ' + files[i].name + ' before compress ' + files[i].size);
                    if (_isMobile == true) {
                        CompressImage(files[i], imgname + "." + fileExt);
                    }
                    else {
                        CompressImage(files[i], imageName);
                    }

                }
            } else {
                //alert("FormData is not supported.");
            }

        });
    }
});

$(document).on('change', '#add_photos', function () {
    var val = $(this).val();
    var imageName = val.replace(/^.*[\\\/]/, '');
    //image name set
    var PartId = $(document).find('#PartModel_PartId').val();
    var imgname = PartId + "_" + Math.floor((new Date()).getTime() / 1000);
    //
    var fileUpload = $("#add_photos").get(0);
    var files = fileUpload.files;
    var fileExt = imageName.substr(imageName.lastIndexOf('.') + 1);
    if (fileExt != 'jpeg' && fileExt != 'jpg' && fileExt != 'png' && fileExt != 'JPEG' && fileExt != 'JPG' && fileExt != 'PNG') {
        ShowErrorAlert(getResourceValue("spnValidImage"));
        $('#add_photos').val('');
        //e.preventDefault();
        return false;
    }
    else if (this.files[0].size > (1024 * 1024 * 10)) {
        ShowImageSizeExceedAlert();
        $('#add_photos').val('');
        //e.preventDefault();
        return false;
    }
    //var duplicate_chk = SaveUploadedFileToServer(WorkOrderId, imageName);
    //if () { }
    else {
        //alert('Hello');
        swal(AddImageAlertSetting, function () {
            if (window.FormData !== undefined) {

                // Create FormData object  
                // var fileData = new FormData();

                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    //fileData.append(imgname + "." + fileExt, files[i]);
                    CompressImage(files[i], imgname + "." + fileExt);
                }
            }
            else {

            }
        });
    }
});
//#endregion

//#region Show Images
var imgcardviewstartvalue = 0;
var imgcardviewlwngth = 10;
var imggrdcardcurrentpage = 1;
var imgcurrentorderedcolumn = 1;
var layoutTypeWO = 1;
function LoadPhotosTab() {
    var PartId = $(document).find('#PartModel_PartId').val();
    $.ajax({
        url: '/Parts/LoadPhotos_Mobile',
        type: 'POST',
        data: {
            'PartId': PartId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Photos').html(data);
        },
        complete: function () {
            LoadImages(PartId);
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function LoadImages(PartId) {
    $.ajax({
        url: '/Parts/GetImages_Mobile',
        type: 'POST',
        data: {
            currentpage: imggrdcardcurrentpage,
            start: imgcardviewstartvalue,
            length: imgcardviewlwngth,
            PartId: PartId
        },
        beforeSend: function () {
            $(document).find('#imagedataloader').show();
        },
        success: function (data) {
            $(document).find('#ImageGrid').show();
            $(document).find('#ObjectImages').html(data).show();
            $(document).find('#tblimages_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == imggrdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
        },
        complete: function () {
            $(document).find('#imagedataloader').hide();
            $(document).find('#cardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(imgcardviewlwngth).trigger('change.select2');
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('change', '#cardviewpagelengthdrp', function () {
    var PartId = $('#PartModel_PartId').val();
    imgcardviewlwngth = $(this).val();
    imggrdcardcurrentpage = parseInt(imgcardviewstartvalue / imgcardviewlwngth) + 1;
    imgcardviewstartvalue = parseInt((imggrdcardcurrentpage - 1) * imgcardviewlwngth) + 1;
    LoadImages(PartId);

});
$(document).on('click', '#tblimages_paginate .paginate_button', function () {
    var PartId = $('#PartModel_PartId').val();
    if (layoutTypeWO == 1) {
        var currentselectedpage = parseInt($(document).find('#tblimages_paginate .pagination').find('.active').text());
        imgcardviewlwngth = $(document).find('#cardviewpagelengthdrp').val();
        imgcardviewstartvalue = imgcardviewlwngth * (parseInt($(this).find('.page-link').text()) - 1);
        var lastpage = parseInt($(this).prev('li').data('currentpage'));

        if ($(this).attr('id') == 'tbl_previous') {
            if (currentselectedpage == 1) {
                return false;
            }
            imgcardviewstartvalue = imgcardviewlwngth * (currentselectedpage - 1);
            imggrdcardcurrentpage = imggrdcardcurrentpage - 1;
        }
        else if ($(this).attr('id') == 'tbl_next') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            imgcardviewstartvalue = imgcardviewlwngth * (currentselectedpage - 1);
            imggrdcardcurrentpage = imggrdcardcurrentpage + 1;
        }
        else if ($(this).attr('id') == 'tbl_first') {
            if (currentselectedpage == 1) {
                return false;
            }
            imggrdcardcurrentpage = 1;
            imgcardviewstartvalue = 0;
        }
        else if ($(this).attr('id') == 'tbl_last') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            imggrdcardcurrentpage = parseInt($(this).prevAll('li').eq(1).text());
            imgcardviewstartvalue = imgcardviewlwngth * (imggrdcardcurrentpage - 1);
        }
        else {
            imggrdcardcurrentpage = $(this).data('currentpage');
        }
        LoadImages(PartId);

    }
    else {
        run = true;
    }
});
$(document).on('click', ".openPictureOptions", function (e) {
    var AttachmentId = $(this).attr('dataid');
    var AttachmentURL = $(this).attr('dataurl');
    $("#imgAttachmentId").val(AttachmentId);
    $("#imgAttachmentURL").val(AttachmentURL);
    $('#OpenImgActionPopup').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', ".actionpopupmobileback", function (e) {
    $('#OpenImgActionPopup').removeClass('slide-active');
});
$(document).on('click', ".selectidOpen", function (e) {
    var AttachmentURL = $("#imgAttachmentURL").val();
    $("#SelectedImg").attr('src', AttachmentURL);
    $('#ShowImgPopup').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', ".openimgback", function (e) {
    $("#SelectedImg").attr('src', '');
    $('#ShowImgPopup').removeClass('slide-active');
});
//#endregion
//#region Set As Default
$(document).on('click', '#selectidSetAsDefault', function () {
    var AttachmentId = $("#imgAttachmentId").val();
    $('#OpenImgActionPopup').removeClass('slide-active');
    var PartId = $('#PartModel_PartId').val();
    $.ajax({
        url: '../base/SetImageAsDefault',
        type: 'POST',

        data: { AttachmentId: AttachmentId, objectId: PartId, TableName: "Part" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result === "success") {
                $('#EquipZoom').attr('src', data.imageurl);
                $('#EquipZoom').attr('data-zoom-image', data.imageurl);
                $('.equipImg').attr('src', data.imageurl);
                $('#EquipZoom').data('zoomImage', data.imageurl).elevateZoom(
                    {
                        zoomType: "window",
                        lensShape: "round",
                        lensSize: 1000,
                        zoomWindowFadeIn: 500,
                        zoomWindowFadeOut: 500,
                        lensFadeIn: 100,
                        lensFadeOut: 100,
                        easing: true,
                        scrollZoom: true,
                        zoomWindowWidth: 450,
                        zoomWindowHeight: 450
                    });
                $("#EquipZoom").on('load', function () {
                    CloseLoader();
                    ShowImageSetSuccessAlert();
                });
            }
        },
        complete: function () {
            //CloseLoader();
            LoadImages(PartId);
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region Delete Image
$(document).on('click', '#selectidDelete', function () {
    var AttachmentId = $("#imgAttachmentId").val();
    $('#' + AttachmentId).hide();
    var PartId = $('#PartModel_PartId').val();
    var ClientOnPremise = $('#PartModel_ClientOnPremise').val();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseMultipleImage(PartId, AttachmentId);
    }
    else {
        DeleteAzureMultipleImage(PartId, AttachmentId);
    }
});

function DeleteOnPremiseMultipleImage(PartId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromOnPremise',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: PartId, TableName: "Part" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    $('#OpenImgActionPopup').removeClass('slide-active');
                    //LoadImages(PartId);
                    RedirectToPartDetail(PartId);
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function DeleteAzureMultipleImage(PartId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromAzure',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: PartId, TableName: "Part" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToPartDetail(PartId);
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion
//#region Save Multiple Image
function SaveMultipleUploadedFileToServer(imageName) {
    var PartId = $('#PartModel_PartId').val();
    $.ajax({
        url: '../base/SaveMultipleUploadedFileToServer',
        type: 'POST',

        data: { 'fileName': imageName, objectId: PartId, TableName: "Part", AttachObjectName: "Part" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {

                CloseLoader();
                ShowImageSaveSuccessAlert();
            }
            else if (data.result == "1") {
                CloseLoader();
                var errorMessage = getResourceValue("ImageExistAlert");
                ShowErrorAlert(errorMessage);

            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
                ShowErrorAlert(errorMessage);

            }
        },
        complete: function () {
            LoadImages(PartId);
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion
//#endregion
//#region QR Code
function PartsQROnSuccess(data) {
    CloseLoader();
    if (data.success === 0) {
        var smallLabel = $('#SmallLabel').prop('checked');
        //window.open('/Parts/QRCodeGenerationUsingRotativa?SmallLabel=' + encodeURI(smallLabel), '_blank');
        window.open('/Parts/QRCodeGenerationUsingDevExpress?SmallLabel=' + encodeURI(smallLabel), '_blank');

        $('#printDetailsPartQrCodeModal').modal('hide');
        partToQR = [];
        PartToClientLookupIdQRcode = [];
        //-- when called from grid         
        if ($(document).find('#tblparts').find('.chksearch').length > 0) {
            $('#printQrcode').prop("disabled", "disabled");
            $('.itemQRcount').text(0);
            $('.itemcount').text(0);
            $(document).find('.DTFC_LeftBodyLiner').find('.chksearch').prop('checked', false);
            $(document).find(".updateArea").hide();
            $(document).find(".actionBar").fadeIn();
        }
        //--
    }
}
$(document).on('click', '#printDetailsPartQrcode', function () {
    var ClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    var Description = $(document).find('#PartModel_Description').val();
    var Location = '';
    var PlaceArea = $(document).find('#PartModel_PlaceArea').val();
    var Section = $(document).find('#PartModel_Section').val();
    var Row = $(document).find('#PartModel_Row').val();
    var Shelf = $(document).find('#PartModel_Shelf').val();
    var Bin = $(document).find('#PartModel_Bin').val();

    var IssueUnit = $(document).find('#PartModel_IssueUnit').val();
    var QtyMinimum = $(document).find('#PartModel_Minimum').val();
    var QtyMaximum = $(document).find('#PartModel_Maximum').val();
    var Manufacturer = $(document).find('#PartModel_Manufacturer').val();
    var ManufacturerID = $(document).find('#PartModel_ManufacturerID').val();
    PartToClientLookupIdQRcode = [];
    if (PlaceArea != "") {
        Location = PlaceArea;
    }
    if (Section != "") {
        if (Location != "") {
            Location = Location + "-" + Section;
        }
        else {
            Location = Section;
        }
    }
    if (Row != "") {
        if (Location != "") {
            Location = Location + "-" + Row;
        }
        else {
            Location = Row;
        }
    }
    if (Shelf != "") {
        if (Location != "") {
            Location = Location + "-" + Shelf;
        }
        else {
            Location = Shelf;
        }
    }
    if (Bin != "") {
        if (Location != "") {
            Location = Location + "-" + Bin;
        }
        else {
            Location = Bin;
        }
    }
    //var Consignment = $(document).find('#PartModel_Consignment').val();
    //var RepairablePart = $(document).find('#PartModel_RepairablePart').val();
    PartToClientLookupIdQRcode.push(ClientLookupId + '][' + Description + '][' + (Location == null ? " " : Location) + '][' + IssueUnit + '][' + QtyMinimum + '][' + QtyMaximum + '][' + Manufacturer + '][' + ManufacturerID);
    var partClientLookups = PartToClientLookupIdQRcode;
    $.ajax({
        url: "/Parts/PartDetailsQRcode_Mobile",
        data: {
            PartClientLookups: partClientLookups
        },
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "html",
        success: function (data) {
            $('#printPartDetailsqrcodemodalcontainer').html(data);
        },
        complete: function () {
            SetControls();
            $('#printDetailsPartQrCodeModal').addClass('slide-active').trigger('mbsc-enhance');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', ".closePrintQRModalCancel", function (e) {
    $('#printDetailsPartQrCodeModal').removeClass('slide-active');
});
//#endregion
//#region Created Last Updated
$(document).on('click', ".closeCreatedLastUpdatedModalPart", function (e) {
    $('#CreatedLastUpdatedModalPart').removeClass('slide-active');
});
$(document).on('click', "#CreatedLastUpdatedModalPartid", function (e) {
    $('#CreatedLastUpdatedModalPart').addClass('slide-active').trigger('mbsc-enhance');
});
//#endregion
//#region Change PartId
function ChangePartIDOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $('#menuPartIdChange').removeClass('slide-active');
        SuccessAlertSetting.text = getResourceValue("spnChangePartSuccess");
        swal(SuccessAlertSetting, function () {
            RedirectToPartDetail(data.partid);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', ".closemenuPartIdChange", function (e) {
    $('#menuPartIdChange').removeClass('slide-active');
});
$(document).on('click', "#liChangePartId", function (e) {
    $('#menuPartIdChange').addClass('slide-active').trigger('mbsc-enhance');
    SetControls();
});
//#endregion
//#region ActivateInActivate
$(document).on('click', '#actinctivatepart', function () {
    var partId = $('#PartModel_PartId').val();
    var clientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    var InActiveFlag = $(document).find('#parthiddeninactiveflag').val();
    $.ajax({
        url: "/Parts/ValidateForActiveInactive",
        type: "POST",
        dataType: "json",
        data: { InActiveFlag: InActiveFlag, PartId: partId, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.validationStatus == true) {
                if (InActiveFlag == "True") {
                    CancelAlertSetting.text = getResourceValue("ActivatePartAlert");
                }
                else {
                    CancelAlertSetting.text = getResourceValue("InActivatePartAlert");
                }
                swal(CancelAlertSetting, function (isConfirm) {
                    if (isConfirm == true) {
                        ActiveInactive(InActiveFlag, partId);
                    }
                });
            }
            else {
                GenericSweetAlertMethod(data);
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (jqxhr) {
            CloseLoader();
        }
    });
});
function ActiveInactive(InActiveFlag, partId) {
    $.ajax({
        url: "/Parts/MakeActiveInactive",
        type: "POST",
        dataType: "json",
        data: { InActiveFlag: InActiveFlag, partId: partId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {

            if (data.Result == 'success') {
                if (InActiveFlag == "True") {
                    SuccessAlertSetting.text = getResourceValue("PartActiveSuccessAlert");
                    localStorage.setItem("PARTMOBILESTATUS", "1");
                    titleText = getResourceValue("AlertActive");
                    CustomQueryDisplayId = "1";

                }
                else {
                    SuccessAlertSetting.text = getResourceValue("PartInActiveSuccessAlert");
                    localStorage.setItem("PARTMOBILESTATUS", "2");
                    titleText = getResourceValue("AlertInactive");
                    CustomQueryDisplayId = "2";
                }
                swal(SuccessAlertSetting, function () {
                    RedirectToPartDetail(partId);
                    $(document).find('#spnlinkToSearch').text(titleText);
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion
//#region Change Hands On Qty
function ShowAdjustHandQty() {
    $('#changeHandsOnQtyPopUp').addClass('slide-active').trigger('mbsc-enhance');
    var partId = LRTrim($('#PartModel_PartId').val());
    var _PartClientLookupId = LRTrim($("#PartModel_ClientLookupId").val());
    var Description = LRTrim($("#PartModel_Description").val());
    var UPCCode = LRTrim($("#PartModel_UPCCode").val());
    $('#inventoryModel_PartId').val(partId);
    $('#inventoryModel_PartClientLookupId').val(_PartClientLookupId);
    $('#inventoryModel_Description').val(Description);
    $('#inventoryModel_PartUPCCode').val(UPCCode);
    $('#inventoryModel_ReceiptQuantity').val('');
    SetControls();
}
function ValidationHandsQtyOnSuccess(data) {
    if (data.Result == "success") {
        $('#changeHandsOnQtyPopUp').removeClass('slide-active');
        RedirectToPartDetail(data.data.PartId);
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
$(document).on('click', ".closechangeHandsOnQtyPopUp", function (e) {
    $('#changeHandsOnQtyPopUp').removeClass('slide-active');
});
//#endregion
//#region Part Checkout
function ShowPartCheckout() {
    $('#partCheckOutPopUp').addClass('slide-active').trigger('mbsc-enhance');
    var partId = LRTrim($('#PartModel_PartId').val());
    var _PartClientLookupId = LRTrim($("#PartModel_ClientLookupId").val());
    var Description = LRTrim($("#PartModel_Description").val());
    var UPCCode = LRTrim($("#PartModel_UPCCode").val());
    $(document).find("#inventoryCheckoutModel_PartId").val(partId);
    $(document).find("#inventoryCheckoutModel_PartClientLookupId").val(_PartClientLookupId);
    $(document).find("#inventoryCheckoutModel_PartDescription").val(Description);
    $(document).find("#inventoryCheckoutModel_UPCCode").val(UPCCode);
    $(document).find("#imgChargeToTree").hide();
    $("#inventoryCheckoutModel_ChargeToId").attr('disabled', 'disabled');
    SetControls();
}
function ValidateParCheckOutOnSuccess(data) {
    if (data.Result == "success") {
        $('#partCheckOutPopUp').removeClass('slide-active');
        RedirectToPartDetail(data.data.PartId);
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
$(document).on('click', "#openpartChkoutgrid", function () {
    //$('.overlay').fadeOut();
    var textChargeToId = $("#inventoryCheckoutModel_ChargeType option:selected").val();
    if (textChargeToId == "WorkOrder") {
        $('#partcheckoutWorkOrderModal').addClass('slide-active');
        $(document).find("#WOListViewForSearch").html("");
        InitializeWOListView_Mobile();
    }
    else if (textChargeToId == "Equipment") {
        $('#partcheckoutEquipmentModal').addClass('slide-active');
        $(document).find("#AssetListViewForSearch").html("");
        InitializeAssetListView_Mobile();
    }
    else {
        $('#partcheckoutLocationModal').addClass('slide-active');
        $(document).find("#LocListViewForSearch").html("");
        InitializeLocListView_Mobile();
    }
});
$(function () {
    //$(document).find('.select2picker').select2({});
    $(document).on("change", "#inventoryCheckoutModel_ChargeType", function () {
        var option = '';
        //chargeTypeText = $('option:selected', this).text();
        var type = $(this).val();
        //chargeTypeSelected = type;
        if (type == "") {
            $(document).find("#imgChargeToTree").hide();
            option = "--Select--";
            $(document).find("#inventoryCheckoutModel_ChargeToId").val("").trigger('change');
            $("#inventoryCheckoutModel_ChargeToId,#openpartChkoutgrid").attr('disabled', 'disabled');
        }
        else {
            if (type == "Equipment") {
                $(document).find("#imgChargeToTree").show();
            }
            else {
                $(document).find("#imgChargeToTree").hide();
            }
            $("#inventoryCheckoutModel_ChargeToId,#openpartChkoutgrid").removeAttr('disabled');

        }
        $(document).find("#txtChargeToId").val("");
    });
});
function resetValidation() {
    $(document).find("#inventoryCheckoutModel_ChargeType").val("").trigger('change');
    $(document).find("#inventoryCheckoutModel_ChargeToId").val("").trigger('change');
    $(document).find("#inventoryCheckoutModel_Quantity").val("1");
}
$(document).on('click', ".partcheckoutlookuplistModalHide", function (e) {
    $('.partcheckoutlookuplistModal').removeClass('slide-active');
});
$(document).on('click', ".partCheckOutPopUpHide", function (e) {
    $('#partCheckOutPopUp').removeClass('slide-active');
});
//#endregion
//#endregion
//#region Description
var summarydescriptionmodaltitle = '';
$(document).on('click', '.partCardViewDescription', function () {
    $(document).find('#partdetaildesmodaltext').text($(this).find("span").text());
    $(document).find('#partdetaildesmodal').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', '#partdetaildesmodalHide', function () {
    if (summarydescriptionmodaltitle != "") {
        $(document).find('.summarydescriptionmodaltitle').text(summarydescriptionmodaltitle);
    }
    $(document).find('#partdetaildesmodal').removeClass('slide-active');
});
$(document).on('click', '.partCardViewMorebtn', function () {
    summarydescriptionmodaltitle = $(document).find('.summarydescriptionmodaltitle').text();
    var modaltitle = $(this).closest('.gridviewcardspan').text();
    modaltitle = modaltitle.split(':')[0];
    $(document).find('.summarydescriptionmodaltitle').text(modaltitle);
    $(document).find('#partdetaildesmodaltext').text($(this).find("span").text());
    $(document).find('#partdetaildesmodal').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', '.partmoreaddescription', function () {
    $(document).find('#partdetaildesmodaltext').text($(this).data("des"));
    $(document).find('#partdetaildesmodal').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', '.partSummaryMorebtn', function () {
    summarydescriptionmodaltitle = $(document).find('.summarydescriptionmodaltitle').text();
    var modaltitle = $(this).closest('.summaryviewcardspan').text();
    modaltitle = modaltitle.split(':')[0];
    $(document).find('.summarydescriptionmodaltitle').text(modaltitle);
    $(document).find('#partdetaildesmodaltext').text($(this).data("des"));
    $(document).find('#partdetaildesmodal').addClass('slide-active').trigger('mbsc-enhance');
});
//#endregion
//#region V2-853 Reset Grid
$('#ResetGridBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("PARTMOBILESTATUS");
        DeleteGridLayout_Mobile('Part_Mobile_Search', localstorageKeys);
        GenerateSearchList('', true);
        window.location.href = "../Parts/Index?page=Inventory_Parts";
    });
});
//#endregion

//#region V2-886
$(document).on('keyup', '#tblparts_paginate .paginate_input', function () {
    var currentselectedpage = parseInt($(document).find('#txtcurrentpage').val());
    var lastpage = parseInt($(document).find('#spntotalpages').text());
    if (currentselectedpage > lastpage) {
        currentselectedpage = lastpage;
    }
    if (currentselectedpage < 1) {
        currentselectedpage = 1;
    }
    cardviewlwngth = $(document).find('#searchcardviewpagelengthdrp').val();
    cardviewstartvalue = cardviewlwngth * (currentselectedpage - 1);
    grdcardcurrentpage = currentselectedpage;

    LayoutUpdate('pagination');
    ShowCardView();
});
//#endregion

//#region V2-918
function SetPartControls() {
    var errClass = 'mobile-validation-error';
    CloseLoader();
    $.validator.setDefaults({
        ignore: null,
        //errorClass: "mobile-validation-error", // default values is input-validation-error
        //validClass: "valid", // default values is valid
        highlight: function (element, errorClass, validClass) { //for the elements having error
            $(element).addClass(errClass).removeClass(validClass);
            $(element.form).find("#" + element.id).parent().parent().addClass("mbsc-err");
            var elemName = $(element.form).find("#" + element.id).attr('name');
            $(element.form).find('[data-valmsg-for="' + elemName + '"]').addClass("mbsc-err-msg");
        },
        unhighlight: function (element, errorClass, validClass) { //for the elements having not any error
            $(element).removeClass(errClass).addClass(validClass);
            $(element.form).find("#" + element.id).parent().parent().removeClass("mbsc-err");
            var elemName = $(element.form).find("#" + element.id).attr('name');
            $(element.form).find('[data-valmsg-for="' + elemName + '"]').removeClass("mbsc-err-msg");
        },
    });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        if ($(this).closest('form').length > 0) {
            $(this).valid();
        }
    });
    $('.orderquantityinput').mobiscroll().numpad({
        touchUi: true,
        preset: 'decimal',
        thousandsSeparator: '',
        entryMode: 'freeform',
        preset: 'decimal',
        scale: 0,
        max: 2147483647,
        min: 0
    });
    $('.decimalinput').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 99999999.99999,
        //fill: 'ltr',
        maxScale: 5,
        preset: 'decimal',
        thousandsSeparator: '',
        entryMode: 'freeform'
    });
    $('.issueorderdecimalinput').mobiscroll().numpad({
        //touchUi: true,
        min: 1.00,
        max: 99999999.99999,
        //fill: 'ltr',
        maxScale: 5,
        preset: 'decimal',
        thousandsSeparator: '',
        entryMode: 'freeform'
    });
    $(document).find('.select2picker').select2({});
    $(document).find('.mobiscrollselect:not(:disabled)').mobiscroll().select({
        display: 'bubble',
        filter: true,
        group: {
            groupWheel: false,
            header: false
        },
    });
    $(document).find('.mobiscrollselect:disabled').mobiscroll().select({
        disabled: true
    });
    SetFixedHeadStyle();
}
//#region Attachment
function LoadAttachmentsTab() {
    var PartId = $(document).find('#PartModel_PartId').val();
    $.ajax({
        url: '/Parts/LoadAttachments_Mobile',
        type: 'POST',
        data: {
            'PartId': PartId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Attachments').html(data);
        },
        complete: function () {
            GeneratePAttachmentGrid();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

function GeneratePAttachmentGrid() {
    if ($(document).find('#pattachmentTable_Mobile').hasClass('dataTable')) {
        dtpAttachmentTable_Mobile.destroy();
    }
    var partid = $(document).find('#PartModel_PartId').val();
    var visibility;
    var attchCount = 0;
    dtpAttachmentTable_Mobile = $("#pattachmentTable_Mobile").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Parts/PopulateAttachment?_partId=" + partid,
            "type": "post",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                if (attchCount > 0) {
                    $(document).find('#partAttachmentCount').show();
                    $(document).find('#partAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#partAttachmentCount').hide();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delPartAttchBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "FileName",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_sensor_1 href="javascript:void(0)"  target="_blank">' + row.FullName + '</a>'
                    }
                },
                { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "CreateDate",
                    "type": "date "
                },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            if (visibility == "false") {
                var column = this.api().column(5);
                column.visible(false);
            }
            else {
                var column = this.api().column(5);
                column.visible(true);
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.lnk_sensor_1', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtpAttachmentTable_Mobile.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/Parts/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });
});
$(document).on('click', '.delPartAttchBttn', function () {
    var data = dtpAttachmentTable_Mobile.row($(this).parents('tr')).data();
    DeletePartAttachment(data.FileAttachmentId);
});
function DeletePartAttachment(fileAttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Parts/DeletePAttachment',
            data: {
                _fileAttachmentId: fileAttachmentId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    dtpAttachmentTable_Mobile.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                GeneratePAttachmentGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnAddpAttachment_Mobile", function () {
    var ClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    var partId = $(document).find('#PartModel_PartId').val();
    $.ajax({
        url: "/Parts/AddPAttachment_Mobile",
        type: "GET",
        dataType: 'html',
        data: { partId: partId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddAttachmentModal_Mobile').html(data);
        },
        complete: function () {
            SetPartControls();
            $('#AddAttachmentModal_Mobile').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
});
function AttachmentPAddOnSuccess_Mobile(data) {
    CloseLoader();
    var partid = data.partid;
    if (data.Result == "success") {
        if (data.IsduplicateAttachmentFileExist) {
            ShowErrorAlert(getResourceValue("AttachmentFileExistAlerts"));
        }
        else {
            SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
            swal(SuccessAlertSetting, function () {
                RedirectToPartDetail(data.partid, "attachment");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }

}
$(document).on('click', ".btnCancelAddAttachment_Mobile", function () {
    $('#AddAttachmentModal_Mobile').removeClass('slide-active')
    var partId = $(document).find('#attachmentModel_PartId').val();
    //swal(CancelAlertSetting, function () {
    //    RedirectToPartDetail(partId, "attachment");
    //});
});
//#endregion
//#region Vendors
function LoadVendorsTab() {
    var PartId = $(document).find('#PartModel_PartId').val();
    $.ajax({
        url: '/Parts/LoadVendor_Mobile',
        type: 'POST',
        data: {
            'PartId': PartId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Vendors').html(data);
        },
        complete: function () {
            GeneratePVendorGrid_Mobile();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', '.addBtnPartsVendor', function () {
    var data = dtpVendorTable_Mobile.row($(this).parents('tr')).data();
    AddVendor_Mobile(data.PartId);
});
$(document).on('click', '.editBttnPartsVendor', function () {
    var data = dtpVendorTable_Mobile.row($(this).parents('tr')).data();
    EditPartVendor(data.PartId, data.Part_Vendor_XrefId, data.UpdateIndex, "update");
});
$(document).on('click', '.delBtnPartsVendor', function () {
    var data = dtpVendorTable_Mobile.row($(this).parents('tr')).data();
    dtpVendorTable_Mobile.state.clear();
    DeletePartVendor(data.Part_Vendor_XrefId);
});
function GeneratePVendorGrid_Mobile() {
    var visibility;
    var rCount = 0;
    var partid = $(document).find('#PartModel_PartId').val();
    if ($(document).find('#pvendorTable_Mobile').hasClass('dataTable')) {
        dtpVendorTable_Mobile.destroy();
    }
    dtpVendorTable_Mobile = $("#pvendorTable_Mobile").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Parts/PopulateVendor",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d._partId = partid;
            },
            "dataSrc": function (json) {
                visibility = json.partVendorSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [10], render: function (a, b, data, d) {
                    if (visibility == "true") {
                        return '<a class="btn btn-outline-primary addBtnPartsVendor gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editBttnPartsVendor gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delBtnPartsVendor gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "Vendor_ClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Vendor_Name", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "CatalogNumber", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Manufacturer", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "ManufacturerId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "OrderQuantity", "autoWidth": false, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "OrderUnit", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Price", "autoWidth": false, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "IssueOrder", "autoWidth": false, "bSearchable": true, "bSortable": true, "className": "text-right" },
                {
                    "data": "UOMConvRequired", "autoWidth": false, "bSearchable": true, "bSortable": true, "className": "text-center",
                    'render': function (data, type, full, meta) {
                        if (data === true) {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" checked="checked" class="status" onclick="return false"><span></span></label>';
                        }
                        else {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" class="status" onclick="return false"><span></span></label>';
                        }
                    }
                },
               
                {
                    "data": "VendorId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            var column = this.api().column(10);
            if (visibility == "false") {
                column.visible(false);
            }
            else {
                column.visible(true);
            }
            if (rCount > 0) {
                $("#btnAddpVendor_Mobile").hide();
            }
            else {
                if (visibility == "true") {
                    $("#btnAddpVendor_Mobile").show();
                }
                else {
                    $("#btnAddpVendor_Mobile").hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}
function DeletePartVendor(PartVendorXrefId) {
    var partid = $(document).find('#PartModel_PartId').val();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Parts/PartsVendorDelete',
            data: {
                _PartVendorXrefId: PartVendorXrefId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    dtpVendorTable_Mobile.state.clear();
                    ShowDeleteAlert(getResourceValue("vendorDeleteSuccessAlert"));
                }
            },
            complete: function () {
                GeneratePVendorGrid_Mobile();
                CloseLoader();
            }
        });
    });
}
function AddPartsVendor(PartId) {
    AddVendor_Mobile(PartId);
}
function AddVendor_Mobile(PartId) {
    ClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    $.ajax({
        url: "/Parts/PartsVedndorAdd_Mobile",
        type: "GET",
        dataType: 'html',
        data: { _partId: PartId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddVendorModal_Mobile').html(data);
        },
        complete: function () {
            SetPartControls();
            $('#AddVendorModal_Mobile').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#btnAddpVendor_Mobile", function () {
    var partId = $(document).find('#PartModel_PartId').val();
    AddVendor_Mobile(partId);
});
function PartsVendorAddOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("VendorAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("VendorUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPartDetail(data.partid, "vendor");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditPartVendor(part_Id, part_Vendor_XrefId, updatedIndex) {
    ClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    $.ajax({
        url: "/Parts/PartsVedndorEdit_Mobile",
        type: "GET",
        dataType: 'html',
        data: { partId: part_Id, _part_Vendor_XrefId: part_Vendor_XrefId, updatedIndex: updatedIndex, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            $('#AddVendorModal_Mobile').html(data);
        },
        complete: function () {
            SetPartControls();
            $('#AddVendorModal_Mobile').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', ".btnpvendorcancel_Mobile", function () {
    $('#AddVendorModal_Mobile').removeClass('slide-active');
    var partId = $(document).find('#partsVendorModel_PartId').val();
    RedirectToPartDetail(partId, "vendor");
    //swal(CancelAlertSetting, function () {
    //    RedirectToPartDetail(partId, "vendor");
    //});
});
$(document).on('change', "#partsVendorModel_VendorClientLookupId", function () {
    var tlen = $(document).find("#partsVendorModel_VendorClientLookupId").val();
    var areaddescribedby = $(document).find("#partsVendorModel_VendorClientLookupId").attr('aria-describedby');
    if (tlen.length > 0) {
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#partsVendorModel_VendorClientLookupId").removeClass("input-validation-error");
    }
    else {
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#partsVendorModel_VendorClientLookupId").addClass("input-validation-error");
    }
});
//#endregion
//#region Equipments
function LoadEquipmentsTab() {
    var PartId = $(document).find('#PartModel_PartId').val();
    $.ajax({
        url: '/Parts/LoadEquipment_Mobile',
        type: 'POST',
        data: {
            'PartId': PartId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Equipments').html(data);
        },
        complete: function () {
            GeneratePEquipmentGrid_Mobile();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function GeneratePEquipmentGrid_Mobile() {
    var rCount = 0;
    var visibility;
    var partid = $(document).find('#PartModel_PartId').val();
    if ($(document).find('#pequipmentTable_Mobile').hasClass('dataTable')) {
        dtpEquipmentTable.destroy();
    }
    dtpEquipmentTable = $("#pequipmentTable_Mobile").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "bProcessing": true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Parts/PopulateEquipment?_partId=" + partid,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                visibility = json.partEquipmentSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    if (visibility == "true") {
                        return '<a class="btn btn-outline-primary addBtnPartsEquipment gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editBttnPartsEquipment gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delBtnPartsEquipment gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "Equipment_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Equipment_Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "QuantityNeeded", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "QuantityUsed", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Comment", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "EquipmentId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            var column = this.api().column(5);
            if (visibility != "true") {
                column.visible(false);
            }
            else {
                column.visible(true);
            }
            if (rCount > 0) {
                $("#btnAddpEquipment_Mobile").hide();
            }
            else {
                if (visibility == "true") {
                    $("#btnAddpEquipment_Mobile").show();
                }
                else {
                    $("#btnAddpEquipment_Mobile").hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}

$(document).on('click', "#btnAddpEquipment_Mobile,.addBtnPartsEquipment", function () {
    var partId = $(document).find('#PartModel_PartId').val();
    var clientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    AddEquipment_Mobile(partId, clientLookupId);
});
$(document).on('click', '.editBttnPartsEquipment', function () {
    var data = dtpEquipmentTable.row($(this).parents('tr')).data();
    EditPartEquipment(data.PartId, data.Equipment_Parts_XrefId, data.UpdateIndex, "update");
});
$(document).on('click', '.delBtnPartsEquipment', function () {
    var data = dtpEquipmentTable.row($(this).parents('tr')).data();
    DeletePartEquipment(data.Equipment_Parts_XrefId);
});
function AddPartsEquipment(PartId) {
    AddEquipment_Mobile(PartId);
}
function AddEquipment_Mobile(partId, clientLookupId) {
    $.ajax({
        url: "/Parts/PartsEquipmentAdd_Mobile",
        type: "GET",
        dataType: 'html',
        data: { partId: partId, clientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            $('#AddEquipmentModal_Mobile').html(data);
        },
        complete: function () {
            SetPartControls();
            $('#AddEquipmentModal_Mobile').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
}
function PartsEquipmentAddOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("EquipmentSaveAlerts");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("EquipmentUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPartDetail(data.partid, "equipment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditPartEquipment(part_Id, part_Equipment_XrefId, updatedIndex) {
    var ClientLookupId = $(document).find('#PartModel_ClientLookupId').val();
    $.ajax({
        url: "/Parts/PartsEquipmentEdit_Mobile",
        type: "GET",
        dataType: 'html',
        data: { partId: part_Id, _equipment_Parts_XrefId: part_Equipment_XrefId, updatedIndex: updatedIndex, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            $('#AddEquipmentModal_Mobile').html(data);
        },
        complete: function () {
            SetPartControls();
            $('#AddEquipmentModal_Mobile').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
}
function DeletePartEquipment(Equipment_Parts_XrefId) {
    var partid = $(document).find('#PartModel_PartId').val();
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Parts/DeleteEquipmentPartXref',
            data: {
                _equipment_Parts_XrefId: Equipment_Parts_XrefId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    dtpEquipmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("equipmentDeleteSuccessAlert"));
                }
            },
            complete: function () {
                GeneratePEquipmentGrid_Mobile();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', ".btnpequipmentcancel_Mobile", function () {
    $('#AddEquipmentModal_Mobile').removeClass('slide-active');
    var partId = $(document).find('#equipmentPartXrefModel_PartId').val();
    RedirectToPartDetail(partId, "equipment");

});
//#endregion
//#region History
var typeValTransactionType;
var typeValChargeType;
var typeValVendor;
var dtpHistoryTable_Mobile;
var hGridfilteritemcount = 0;
var daterange = "0";
function LoadHistoryTab() {
    var PartId = $(document).find('#PartModel_PartId').val();
    $.ajax({
        url: '/Parts/LoadHistory_Mobile',
        type: 'POST',
        data: {
            'PartId': PartId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#History').html(data);
        },
        complete: function () {
            GenerateHistoryGrid_Mobile();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}


function GenerateHistoryGrid_Mobile() {
    var daterange = $('#partsHistoryModel_HistoryDaterange').val();
    var partid = $(document).find('#PartModel_PartId').val();
    var partclientlookup = $(document).find('#partclientlookupid').text();
    var TransactionType = LRTrim($("#rgridadvsearchTransactionType").val());
    var Requestor_Name = LRTrim($("#rgridadvsearchRequestorName").val());
    var PerformBy_Name = LRTrim($("#rgridadvsearchPerformByName").val());
    var TransactionDate = LRTrim($("#rgridadvsearchTransactionDate").val());
    var TransactionQuantity = LRTrim($('#rgridadvsearchTransactionQuantity').val());
    var Cost = $('#rgridadvsearchCost').val();
    var ChargeType_Primary = LRTrim($("#rgridadvsearchChargeTypePrimary").val());
    var ChargeTo_ClientLookupId = LRTrim($("#rgridadvsearchChargeToName").val());
    var Account_ClientLookupId = LRTrim($("#rgridadvsearchAccountId").val());
    var PurchaseOrder_ClientLookupId = LRTrim($("#rgridadvsearchPurchaseOrderClientLookupId").val());
    var Vendor_ClientLookupId = LRTrim($('#rgridadvsearchVendorClientLookupId').val());
    var Vendor_Name = $('#rgridadvsearchVendorName').val();
    if ($(document).find('#phistoryTable_Mobile').hasClass('dataTable')) {
        dtpHistoryTable_Mobile.destroy();
    }
    dtpHistoryTable_Mobile = $("#phistoryTable_Mobile").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[3, "desc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons:
            [
                {
                    extend: 'excel',
                    filename: partclientlookup + '_History',
                    extension: '.xlsx',
                    className: "datatable-btn-export historygridexport"
                }
            ],
        "orderMulti": true,
        "ajax":
        {
            "url": "/Parts/PopulatePartHistoryReview",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.partid = partid;
                d.daterange = daterange;
                d.TransactionType = TransactionType;
                d.Requestor_Name = Requestor_Name;
                d.PerformBy_Name = PerformBy_Name;
                d.TransactionDate = TransactionDate;
                d.TransactionQuantity = TransactionQuantity;
                d.Cost = Cost;
                d.ChargeType_Primary = ChargeType_Primary;
                d.ChargeTo_ClientLookupId = ChargeTo_ClientLookupId;
                d.Account_ClientLookupId = Account_ClientLookupId;
                d.PurchaseOrder_ClientLookupId = PurchaseOrder_ClientLookupId;
                d.Vendor_ClientLookupId = Vendor_ClientLookupId;
                d.Vendor_Name = Vendor_Name;
            },
            "dataSrc": function (json) {
                var myDataSource = json;
                var TransTyp = [];
                var ChrgTyp = [];
                var Vendor = [];
                var VendorObj;
                for (var i = 0; i < myDataSource.dataAll.length; ++i) {
                    TransTyp.push(myDataSource.dataAll[i].TransactionType);
                    ChrgTyp.push(myDataSource.dataAll[i].ChargeType_Primary);
                    VendorObj = new VendorsObjArray(myDataSource.dataAll[i].Vendor_ClientLookupId, myDataSource.dataAll[i].Vendor_ClientLookupId + '-' + myDataSource.dataAll[i].Vendor_Name);
                    Vendor.push(VendorObj);
                }
                TransTyp = TransTyp.filter(function (v) { return v !== '' });
                ChrgTyp = ChrgTyp.filter(function (v) { return v !== '' });
                Vendor = Vendor.filter(function (v) { return v.id !== '' });
                var result = [];
                $.each(Vendor, function (i, e) {
                    var matchingItems = $.grep(result, function (item) {
                        return item.id === e.id && item.name === e.name;
                    });
                    if (matchingItems.length === 0) result.push(e);
                });


                TransTyp = TransTyp.filter(onlyUnique);
                ChrgTyp = ChrgTyp.filter(onlyUnique);

                var TTlen = TransTyp.length;
                $("#rgridadvsearchTransactionType").empty();
                $("#rgridadvsearchTransactionType").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < TTlen; i++) {
                    var id = TransTyp[i];
                    var name = TransTyp[i];
                    $("#rgridadvsearchTransactionType").append("<option value='" + id + "'>" + name + "</option>");
                }
                var CTlen = ChrgTyp.length;
                $("#rgridadvsearchChargeTypePrimary").empty();
                $("#rgridadvsearchChargeTypePrimary").append("<option value='" + 0 + "'>" + "--Select--" + "</option>");
                for (var i = 0; i < CTlen; i++) {
                    var id = ChrgTyp[i];
                    var name = ChrgTyp[i];
                    $("#rgridadvsearchChargeTypePrimary").append("<option value='" + id + "'>" + name + "</option>");
                }
                var VenLen = result.length;
                $("#rgridadvsearchVendorClientLookupId").empty();
                $("#rgridadvsearchVendorClientLookupId").append("<option value='" + 0 + "'>" + "--Select--" + "</option>");
                for (var i = 0; i < VenLen; i++) {
                    var id = result[i].id;
                    var name = result[i].name;
                    $("#rgridadvsearchVendorClientLookupId").append("<option value='" + id + "'>" + name + "</option>");
                }
                if (typeValTransactionType) {
                    $("#rgridadvsearchTransactionType").val(typeValTransactionType);
                }
                if (typeValChargeType) {
                    $("#rgridadvsearchChargeTypePrimary").val(typeValChargeType);
                }
                if (typeValVendor) {
                    $("#rgridadvsearchVendorClientLookupId").val(typeValVendor);
                }
                return json.data;
            }
        },
        "columns":
            [
                { "data": "TransactionType", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Requestor_Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PerformBy_Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "TransactionDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "TransactionQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Cost", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ChargeType_Primary", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ChargeTo_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Account_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PurchaseOrder_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Vendor_ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Vendor_Name", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('change', '#partsHistoryModel_HistoryDaterange', function () {
    dtpHistoryTable_Mobile.state.clear();
    GenerateHistoryGrid_Mobile();
});

function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
}
//#endregion
//#region Receipt

var rgridfilteritemcount = 0;
var dtReceipt_Mobile;
var typeValVendor;
function LoadReceiptTab() {
    var PartId = $(document).find('#PartModel_PartId').val();
    $.ajax({
        url: '/Parts/LoadReceipt_Mobile',
        type: 'POST',
        data: {
            'PartId': PartId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Receipt').html(data);
        },
        complete: function () {
            generateReceiptDataTable_Mobile();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}


function generateReceiptDataTable_Mobile() {
    var partid = $(document).find('#PartModel_PartId').val();
    var daterange = $(document).find('#partsReceiptModel_receiptdtselector').val();
    var partclientlookup = $(document).find('#partclientlookupid').text();
    var POClientLookupId = LRTrim($("#PurchaseOrder").val());
    var ReceivedDate = LRTrim($("#ReceiptDate").val());
    var VendorClientLookupId = LRTrim($("#rgridReceiptadvsearchVendorClientLookupId").val());
    var VendorName = LRTrim($("#VendorName").val());
    var OrderQuantity = LRTrim($("#Quantity").val());
    var UnitCost = LRTrim($('#UnitCost').val());
    if (typeof dtReceipt_Mobile !== "undefined") {
        dtReceipt_Mobile.destroy();
    }
    dtReceipt_Mobile = $("#receiptsTable_Mobile").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[0, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excel',
                filename: partclientlookup + '_Receipts',
                extension: '.xlsx',
                className: "datatable-btn-export receiptgridexport"
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Parts/populatePartReceipt",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.partid = partid;
                d.daterange = daterange;
                d.POClientLookupId = POClientLookupId;
                d.ReceivedDate = ReceivedDate;
                d.VendorClientLookupId = VendorClientLookupId;
                d.VendorName = VendorName;
                d.OrderQuantity = OrderQuantity;
                d.UnitCost = UnitCost;
            },
            "dataSrc": function (json) {
                var myDataSource = json;
                var Vendor = [];
                var VendorObj;
                for (var i = 0; i < myDataSource.dataAll.length; ++i) {
                    VendorObj = new VendorsObjArray(myDataSource.dataAll[i].VendorClientLookupId, myDataSource.dataAll[i].VendorClientLookupId + '-' + myDataSource.dataAll[i].VendorName);
                    Vendor.push(VendorObj);
                }
                Vendor = Vendor.filter(function (v) { return v.id !== '' });
                var result = [];
                $.each(Vendor, function (i, e) {
                    var matchingItems = $.grep(result, function (item) {
                        return item.id === e.id && item.name === e.name;
                    });
                    if (matchingItems.length === 0) result.push(e);
                });
                var VenLen = result.length;
                $("#rgridReceiptadvsearchVendorClientLookupId").empty();
                $("#rgridReceiptadvsearchVendorClientLookupId").append("<option value='" + 0 + "'>" + "--Select--" + "</option>");
                for (var i = 0; i < VenLen; i++) {
                    var id = result[i].id;
                    var name = result[i].name;
                    $("#rgridReceiptadvsearchVendorClientLookupId").append("<option value='" + id + "'>" + name + "</option>");
                }
                if (typeValVendor) {
                    $("#rgridReceiptadvsearchVendorClientLookupId").val(typeValVendor);
                }
                return json.data;
            }
        },
        "columns":
            [
                { "data": "POClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ReceivedDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OrderQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('change', '#partsReceiptModel_receiptdtselector', function () {
    dtReceipt_Mobile.state.clear();
    generateReceiptDataTable_Mobile();
});

function VendorsObjArray(id, name) {
    this.id = id;
    this.name = name;
}


//#endregion
//#region Review
function LoadReviewTab() {
    var PartId = $(document).find('#PartModel_PartId').val();
    $.ajax({
        url: '/Parts/LoadReviewt_Mobile',
        type: 'POST',
        data: {
            'PartId': PartId
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#Review').html(data);
        },
        complete: function () {
            GenerateReviewSiteGrid_Mobile();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

function GenerateReviewSiteGrid_Mobile() {
    var visibility;
    var rCount = 0;
    var PartMasterId = $(document).find('#partMasterModel_PartMasterId').val();
    if ($(document).find('#reviewSiteTable_Mobile').hasClass('dataTable')) {
        dtpReviewSiteTable_Mobile.destroy();
    }
    dtpReviewSiteTable_Mobile = $("#reviewSiteTable_Mobile").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Parts/PopulateReviewSite",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.PartMasterId = PartMasterId;
                d.SiteName = LRTrim($('#RSSiteName').val());
                d.ClientLookupId = LRTrim($("#RSClientLookupId").val());
                d.Description = LRTrim($("#RSDescription").val());
                d.QtyOnHand = LRTrim($('#RSQtyOnHand').val());
                d.QtyOnOrder = LRTrim($('#RSQtyOnOrder').val());
                d.LastPurchaseDate = ValidateDate($("#RSLastPurchaseDate").val());
                d.LastPurchaseCost = LRTrim($('#RSLastPurchaseCost').val());
                d.LastPurchaseVendor = LRTrim($('#RSLastPurchaseVendor').val());
                d.InactiveFlag = LRTrim($('#RSisInactive').val());
            },
            "dataSrc": function (json) {
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [9], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-primary btnRequestTransfer gridinnerbutton" title="Request Transfer">Request Transfer</a>';
                }
            }
        ],
        "columns":
            [
                { "data": "SiteName", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "ClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "QtyOnHand", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "QtyOnOrder", "autoWidth": false, "bSearchable": true, "bSortable": true },

                {
                    "data": "LastPurchaseDate", "autoWidth": true, "bSearchable": true, "bSortable": false, "type": "date",
                    render: function (data, type, row, meta) {
                        if (data == null) {
                            return '';
                        } else {
                            return data;
                        }
                    }
                },
                { "data": "LastPurchaseCost", "autoWidth": false, "bSearchable": true, "bSortable": true },

                { "data": "LastPurchaseVendor", "autoWidth": false, "bSearchable": true, "bSortable": true },
                {
                    "data": "InactiveFlag", "autoWidth": true, "bSearchable": true, "bSortable": true, className: 'text-center', "name": "2",
                    "mRender": function (data, type, row) {
                        if (data == true) {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" checked="checked" class="status" ><span></span></label>';
                        }
                        else {

                            return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox"  class="status"><span></span></label>';
                        }

                    }
                },
                {
                    "data": "PartId",
                    "autoWidth": true,
                    "bSearchable": false,
                    "bSortable": false,
                    className: 'text-left',
                    "mRender": function (data, type, row) {
                        //console.log(row);
                        if (row.RequestTransferStatus == true)
                            return '<a class="btn btn-outline-primary btnRequestTransfer gridinnerbutton" title="Request Transfer" >Request Transfer</a>';
                    }
                },
            ],
        initComplete: function () {
            $(document).on('click', '.status', function (e) {
                e.preventDefault();
            });
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.btnRequestTransfer', function () {
    var RequestPartId = $(document).find('#PartModel_PartId').val();

    var data = dtpReviewSiteTable_Mobile.row($(this).parents('tr')).data();
    RequestTransfer(data.PartId, data.Description, data.ClientLookupId, data.SiteName, RequestPartId);
});
function RequestTransfer(IssuePartId, IssueDescription, IssueClientLookupId, SiteName, RequestPartId) {
    $.ajax({
        url: "/Parts/AddPartTransfer",
        type: "GET",
        dataType: 'html',
        data: { IssuePartId: IssuePartId, IssueDescription: IssueDescription, IssueClientLookupId: IssueClientLookupId, SiteName: SiteName, RequestPartId: RequestPartId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderparts').html(data);
        },
        complete: function () {
            SetPartControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function PartTransferAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        SuccessAlertSetting.text = getResourceValue("PartTransferCreateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPartDetail(data.PartId, "ReviewSite")
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};
$(document).on('click', "#btnPartTransfercancel", function () {
    var partid = $(document).find('#partTransferModel_PartId').val();
    swal(CancelAlertSetting, function () {
        RedirectToPartDetail(partid, "ReviewSite");
    });
});
//#endregion


//#endregion

//#region V2-919
var IsPartDetailsFromEquipmentstatus = $('#IsPartDetailsFromEquipment').val();
if (IsPartDetailsFromEquipmentstatus == 'True') {
    var stexts = $(document).find('#PartModel_partStatusForRedirection').val();
    $(document).find('#spnlinkToSearch').text(stexts);
    CloseLoader();
    SetFixedHeadStyle();
    $('#tabscroll').mobiscroll().nav({
        type: 'tab'
    });
}
//#endregion
//#region V2-958
$(document).on('click', '#sidebarCollapse', function () {
    $('#sidebar').addClass('slide-active').trigger('mbsc-enhance');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('click', '.PartsDataAdvSrchclearstate, .overlay', function () {
    $('#sidebar').removeClass('slide-active');
    $('.overlay').fadeOut();
});
$("#btnPartsDataAdvSrch").on('click', function (e) {
    $(document).find('#txtColumnSearch').val('');
    $('#sidebar').removeClass('slide-active');
    $('.overlay').fadeOut();
    AdvanceSearch();
    cardviewstartvalue = 0;
    grdcardcurrentpage = 1;
    LayoutFilterinfoUpdate();
    ShowCardView();
});
function AdvanceSearch() {
    var InactiveFlag = false;
    $('#txtColumnSearch').val('');
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("#" + this.id + "").parents('label').children('span').eq(0).text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
        if ($(this).attr('id') == "ddlassetAvailability") {
            if ($(this).val() == null && AssetAvailability != null) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("#" + this.id + "").parents('label').children('span').eq(0).text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#advsearchfilteritems").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
}
function clearAdvanceSearch() {
    selectCount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $(document).find("#StockType").val("").trigger('change');
    $(document).find("#Consignment").val("").trigger('change');
    $('.filteritemcount').text(selectCount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion
//#region V2-1007
$(document).on('click', '#linkToEquipment', function (e) {
    clearDropzone();
    var EquipmentId = $(document).find('#EquipmentId').val();
    window.location.href = "../Equipment/DetailFromWorkOrder?EquipmentId=" + EquipmentId;
});
//#endregion

//#region V2-1196
function BindMobiscrollControlForPartsConfigureAutoPurchasing() {
 
    $('#QtyMaximum').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 99999999.99,
        //scale: 2,
        maxScale: 2,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    var x = parseFloat($('#QtyMaximum').val()) == 0 ? '' : $('#QtyMaximum').val();
    $('#QtyMaximum').mobiscroll('setVal', x);
   
    $('#QtyReorderLevel').mobiscroll().numpad({
        //touchUi: true,
        //min: 0.01,
        max: 9999999999.99999,
        //scale: 2,
        maxScale: 5,
        entryMode: 'freeform',
        preset: 'decimal',
        thousandsSeparator: ''
    });
    x = parseFloat($('#QtyReorderLevel').val()) == 0 ? '' : $('#QtyReorderLevel').val();
    $('#QtyReorderLevel').mobiscroll('setVal', x);

    $('#UpdatePartsConfigureAutoPurchasingPopUp').trigger('mbsc-enhance');


}
function resetValidationConfigureAutoPurchasing() {
    $(document).find("#partsConfigureAutoPurchasingModel_IsAutoPurchase").prop("checked", false);
    $(document).find("#partsConfigureAutoPurchasingModel_PartVendorId").val("").trigger('change');
    $(document).find("#QtyReorderLevel").val("");
    $(document).find("#QtyMaximum").val("");
    var partsConfigureAutoPurchasingModel_PartVendorId = $(document).find("#partsConfigureAutoPurchasingModel_PartVendorId").attr('aria-describedby');
    $('#' + partsConfigureAutoPurchasingModel_PartVendorId).hide();
    $(document).find('form').find("#partsConfigureAutoPurchasingModel_PartVendorId").removeClass("input-validation-error");
    $(document).find('form').find("#QtyReorderLevel").removeClass("input-validation-error");
    $(document).find('form').find("#QtyMaximum").removeClass("input-validation-error");

}
function ShowConfigureAutoPurchasing() {
    
    $('#UpdatePartsConfigureAutoPurchasingPopUp').addClass('slide-active');
    SetControls();
    BindMobiscrollControlForPartsConfigureAutoPurchasing();
}
function UpdatePartsConfigureAutoPurchasingOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("AutoPurchasingsetupupdatedsuccessfullyAlert");
        $('#UpdatePartsConfigureAutoPurchasingPopUp').removeClass('slide-active');
        swal(SuccessAlertSetting, function () {
            RedirectToPartDetail(data.PartId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);

    }
}
$(document).on('click', '.UpdatePartsConfigureAutoPurchasingPopUpClose', function (e) {
    $('#UpdatePartsConfigureAutoPurchasingPopUp').removeClass('slide-active');
    resetValidationConfigureAutoPurchasing();
});
$(document).on("change", "#partsConfigureAutoPurchasingModel_PartVendorId", function () {

    ConfigureAutoPurchasingVendorError();
});
function ConfigureAutoPurchasingVendorError() {
    var tlen = $(document).find("#partsConfigureAutoPurchasingModel_PartVendorId").val();
    if (tlen.length > 0) {
        var areaddescribedby = $(document).find("#partsConfigureAutoPurchasingModel_PartVendorId").attr('aria-describedby');
        $('#' + areaddescribedby).hide();
        $(document).find('form').find("#partsConfigureAutoPurchasingModel_PartVendorId").removeClass("input-validation-error");
    }
    else {
        var areaddescribedby = $(document).find("#partsConfigureAutoPurchasingModel_PartVendorId").attr('aria-describedby');
        $('#' + areaddescribedby).show();
        $(document).find('form').find("#partsConfigureAutoPurchasingModel_PartVendorId").addClass("input-validation-error");
    }
}
$(document).on("change", ".partsConfigureQty", function () {
    var $document = $(document);
    var $autoPurchase = $document.find("#partsConfigureAutoPurchasingModel_IsAutoPurchase");
    var $qtyMax = $document.find("#QtyMaximum");
    var $qtyReorder = $document.find("#QtyReorderLevel");
    var $qtyMaxlbl = $document.find("#spnMaximum");
    var $qtyReorderlbl = $document.find("#spnMinimum");
    if ($autoPurchase.prop("checked")) {
        if (parseFloat($qtyMax.val()) > parseFloat($qtyReorder.val())) {
            $qtyReorderlbl.removeClass("mbsc-err");
            $qtyMaxlbl.removeClass("mbsc-err");
            var $outerSpanMax = $('span[data-valmsg-for="partsConfigureAutoPurchasingModel.QtyMaximum"]');
            if ($outerSpanMax.length > 0) {
                $outerSpanMax.find('span[for="QtyMaximum"]').remove();
            }
            var $outerSpanMin = $('span[data-valmsg-for="partsConfigureAutoPurchasingModel.QtyReorderLevel"]');
            if ($outerSpanMin.length > 0) {
                $outerSpanMin.find('span[for="QtyReorderLevel"]').remove();
            }
        }
    }
});
//#endregion