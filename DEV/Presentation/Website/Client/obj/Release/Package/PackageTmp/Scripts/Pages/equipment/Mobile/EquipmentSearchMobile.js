var equipOrder = '1';
var equipDir = 'asc';
var gridName = "Equipment_Mobile_Search";
var DefaultLayoutInfo = '{"time":currentTime,"start":0,"length":10,"order":[[0,"asc"]],"search":{"search":"","smart":true,"regex":false,"caseInsensitive":true},"columns":[],"ColReorder":[]}';
var dtEquipmentMobileSearch;
var ZoomConfig = { zoomType: "window", lensShape: "round", lensSize: 1000, zoomWindowFadeIn: 500, zoomWindowFadeOut: 500, lensFadeIn: 100, lensFadeOut: 100, easing: true, scrollZoom: true, zoomWindowWidth: 450, zoomWindowHeight: 450 };
var titleText = "";
var run = false;
var cardviewstartvalue = 0;
var cardviewlength = 10;
var grdcardcurrentpage = 1; 
//var currentorderedcolumn = 1;
//var currentorder = 'asc';
var CustomQueryDisplayId;
var SortByDropdown;
var EquipmentViewDropdown;
var AssetAvailability;
var selectCount = 0;
$.validator.setDefaults({ ignore: null });
$(document).ready(function () {
    if (IsAdd == "True") {
        addAssetMobile();
    }   
    var EquipmentViewStatus = localStorage.getItem("EquipmentViewstatusMobile");
    if (EquipmentViewStatus) {
        CustomQueryDisplayId = EquipmentViewStatus;
    }
    else {
        EquipmentViewStatus = "1";
        CustomQueryDisplayId = EquipmentViewStatus;
        localStorage.setItem("EquipmentViewstatusMobile", "1");
    }
    $("#EquipmentViewDropdown li").removeClass('active');
    $("#EquipmentViewDropdown li[data-value='" + CustomQueryDisplayId + "']").addClass('active');
    titleText = $("#EquipmentViewDropdown li[data-value='" + CustomQueryDisplayId + "']").text();
    $('#Equipmentmobilesearchtitle').text(titleText);

    GetDatatableLayout();
    ShowCardView();

    ////--- debashis
    mobiscroll.settings = {
        lang: 'en',                                       // Specify language like: lang: 'pl' or omit setting to use default
        theme: 'ios',                                     // Specify theme like: theme: 'ios' or omit setting to use default
        themeVariant: 'light'                             // More info about themeVariant: https://docs.mobiscroll.com/4-10-9/javascript/popup#opt-themeVariant
    };

    EquipmentViewDropdown = $('#EquipmentViewDropdownDiv').mobiscroll().popup({
        display: 'bubble',
        anchor: '#ShowEquipmentViewDropdown',
        buttons: [],
        cssClass: 'mbsc-no-padding md-vertical-list'
    }).mobiscroll('getInst');
    SortByDropdown = $('#SortByDropdownDiv').mobiscroll().popup({
        display: 'bubble',
        anchor: '#ShowSortByDropdown',
        buttons: [],
        cssClass: 'mbsc-no-padding md-vertical-list'
    }).mobiscroll('getInst');  

    $('#EquipmentViewDropdown').mobiscroll().listview({
        enhance: true,
        swipe: false,
    });
    $('#SortByDropdown').mobiscroll().listview({
        enhance: true,
        swipe: false
    });

    $('#ShowEquipmentViewDropdown').click(function () {
        EquipmentViewDropdown.show();
        return false;
    });
    $('#ShowSortByDropdown').click(function () {
        SortByDropdown.show();
        return false;
    });

    SetControls_Mobile();
    
});


function ShowCardView() {
    $.ajax({
        "url": "/Equipment/GetequipmentGridData_Mobile",
        type: 'POST',
        dataType: 'html',
        //contentType: "application/json; charset=utf-8",
        data: {
            currentpage: grdcardcurrentpage,
            start: cardviewstartvalue,
            length: cardviewlength,
            CustomQueryDisplayId: localStorage.getItem("EquipmentViewstatusMobile"),
            ClientLookupId: LRTrim($("#EquipmentID").val()),
            Name: LRTrim($("#Name").val()),
            Location: LRTrim($("#Location").val()),
            AssetGroup1Id: LRTrim($('#AssetGroup1Id').val()),
            AssetGroup2Id: LRTrim($("#AssetGroup2Id").val()),
            AssetGroup3Id: LRTrim($("#AssetGroup3Id").val()),
            LaborAccountClientLookupId: LRTrim($("#AccountSearchId").val()),
            SerialNumber: LRTrim($("#SerialNumber").val()),
            Type: LRTrim($('#ddlType').val()),
            Make: LRTrim($("#Make").val()),
            Model: LRTrim($("#ModelNumber").val()),
            AssetNumber: LRTrim($("#AssetsNumber").val()),
            txtSearchval: LRTrim($(document).find('#txtColumnCompSearch').val()),
            AssetAvailability : (AssetAvailability != null) ? AssetAvailability : LRTrim($('#ddlassetAvailability').val()),
            Order: equipOrder,
            orderDir: equipDir
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#ActiveCard').html(data).show();
            $(document).find('#tblEquipmentMobileSearch_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == grdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
        },
        complete: function () {
            var sortClass = '';
            if (equipDir == 'asc') {
                sortClass = 'sorting_asc_mobile';
            }
            else if (equipDir == 'desc') {
                sortClass = 'sorting_desc_mobile';
            }
            $(document).find('#SortByDropdown li').removeClass('active sorting_asc_mobile sorting_desc_mobile');
            $(document).find('#SortByDropdown li[data-value="' + equipOrder + '"]').addClass('active').addClass(sortClass);
           
            $(document).find('#searchcardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(cardviewlength).trigger('change.select2');
            //HidebtnLoader("btnsortmenu");
            //HidebtnLoader("layoutsortmenu");
            HidebtnLoader("SrchBttnNew");
            //HidebtnLoader("sidebarCollapse");
            HidebtnLoader("txtColumnCompSearch");
            //HidebtnLoader("btnWoAdd");
            titleText = $("#EquipmentViewDropdown li[data-value='" + CustomQueryDisplayId + "']").text();
            $('#Equipmentmobilesearchtitle').text(titleText);
            //#region V2-1023 Page Navigation Show Hide
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
$(document).on('click', '#showEquipmentfloatbtn', function () {
    $(document).find("#showAddEquip_Mobile").css('display', 'inline-block');
});
$(document).on('click', '#tblEquipmentMobileSearch_paginate .paginate_button', function () {
   

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

    if (equipOrder == $(this).data('value')) {
        if (equipDir == 'asc') {
            equipDir = 'desc';
        }
        else if (equipDir == 'desc') {
            equipDir = 'asc';
        }
    }
    else {
        equipDir = 'asc';
    }

    equipOrder = $(this).data('value');
    grdcardcurrentpage = 1;
    cardviewstartvalue = 0;

    LayoutUpdate('column');
    ShowCardView();
});

function filterinfoMain(id, value) {
    this.key = id;
    this.value = value;
}
function getfilterinfoarray(txtsearchelement, advsearchcontainer) {
    var filterinfoarray = [];
    var f = new filterinfoMain('searchstringMaintComp', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parents('div').is(":visible")) {
            f = new filterinfoMain($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);
        }
        if ($(this).parent('div').find('div').hasClass('range-timeperiod')) {
            if ($(this).parents('div').find('input').val() !== '' && $(this).val() == '10') {
                f = new filterinfoMain('this-' + $(this).attr('id'), $(this).parent('div').find('input').val());
                filterinfoarray.push(f);
            }
        }
    });
    return filterinfoarray;
}
function setsearchui(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if (item.key == 'searchstringMaintComp' && item.value) {
            var txtSearchval = item.value;
            if (item.value) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossMain" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parents('div').is(":visible")) {
                $('#' + item.key).val(item.value).trigger('change');
                if (item.value) {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("#" + item.key + "").parents('label').children('span').eq(0).text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossMain" aria-hidden="true"></a></span>';
                }
            }
            advcountercontainer.text(selectCount);
        }
        
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}
//#region Search
$(document).mouseup(function (e) {
    var container = $(document).find('#compsearchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});
$(document).on('click', "#WoCompSrchBttn", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: gridName },
        beforeSend: function () {
            ShowbtnLoader("WoCompSrchBttn");
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)" id= "mem_' + i + '"' + '><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchList.innerHTML = str;
            $(document).find('#compsearchBttnNewDrop').show("slideToggle");
        },
        complete: function () {
            HidebtnLoader("WoCompSrchBttn");
        },
        error: function () {
            HidebtnLoader("WoCompSrchBttn");
        }
    });
});
function GenerateSearchListMain(txtSearchval, isClear) {
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
$(document).on('keyup', '#txtColumnCompSearch', function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == 13) {
        TextSearchMain();
    }
    else {
        event.preventDefault();
    }
});
function TextSearchMain() {
    run = true;
     clearAdvanceSearch();
    var txtSearchval = LRTrim($(document).find('#txtColumnCompSearch').val());
    if (txtSearchval) {
        GenerateSearchListMain(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnCompSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossMain" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#searchfilteritems").html(searchitemhtml);
    }
    else {
        cardviewstartvalue = 0;
        cardviewlength = 10;
        grdcardcurrentpage = 1;
        ShowCardView();
    }
    var container = $(document).find('#compsearchBttnNewDrop');
    container.hide("slideToggle");
}
$(document).on('click', '.btnCrossMain', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    //if (searchtxtId == "gridadvsearchstatus") {
    //    $(document).find("#gridadvsearchstatus").val("").trigger('change.select2'); 
    //}

    //ProjectAdvSearch();
    if (searchtxtId == "ddlassetAvailability") {
        AssetAvailability = null;
    }
    EquipmentAdvSearch();
    cardviewstartvalue = 0;
    cardviewlength = 10;
    grdcardcurrentpage = 1;

    LayoutFilterinfoUpdate();
    ShowCardView();
});
$(document).on('click', '#UlSearchList li', function () {
    var v = LRTrim($(this).text());
    $(document).find('#txtColumnCompSearch').val(v);
    TextSearchMain();
});
$(document).on('click', '#cancelText', function () {
    $(document).find('#txtColumnCompSearch').val('');
});
$(document).on('click', '#clearText', function () {
    GenerateSearchListMain('', true);
});
$(document).on('click', '.txtSearchClickComp', function () {
    TextSearchMain();
});
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
                    gridstate.order[0] = [equipOrder, equipDir];
                }
                else if (area === 'pagination') {//
                }
                else if (area === 'pagelength' || area === 'optionDropdownChange') {
                    gridstate.length = cardviewlength;
                }

                if (json.FilterInfo !== '') {
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnCompSearch"), $("#spnControlCounter"), $("#searchfilteritems"));
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
                var filterinfoarray = getfilterinfoarray($("#txtColumnCompSearch"), $('#advsearchsidebar'));
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
                //var info = workOrdersSearchdt.page.info();
                var pageclicked = (LayoutInfo.start / LayoutInfo.length);
                cardviewlength = LayoutInfo.length;
                cardviewstartvalue = cardviewlength * pageclicked;
                grdcardcurrentpage = pageclicked + 1;
                equipOrder = LayoutInfo.order[0][0]; //$('#tblworkorders').dataTable().fnSettings().aaSorting[0][0];
                equipDir = LayoutInfo.order[0][1]; //$('#tblworkorders').dataTable().fnSettings().aaSorting[0][1];

                if (json.FilterInfo !== '') {
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnCompSearch"), $("#spnControlCounter"), $("#searchfilteritems"));
                }
            }
            else {
                DefaultLayoutInfo = DefaultLayoutInfo.replace('currentTime', new Date().getTime());
                var filterinfoarray = getfilterinfoarray($("#txtColumnCompSearch"), $('#advsearchsidebar'));
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
//#endregion
//#endregion

//#region View dropdown change

$(document).on('click', '#EquipmentViewDropdown li', function (e) {
    if ($(this).hasClass('active') == true) {
        EquipmentViewDropdown.hide();
        return false;
    }

    $(this).addClass('active').siblings().removeClass('active');
    EquipmentViewDropdown.hide();

    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    


    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).data('value');
    localStorage.setItem("EquipmentViewstatusMobile", optionval);
    CustomQueryDisplayId = optionval;


    if ($(document).find('#txtColumnCompSearch').val() !== '') {
        $('#searchfilteritems').find('span').html('');
        $('#searchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnCompSearch').val('');


    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
       
        cardviewstartvalue = 0;
        //cardviewlength = 10;
        grdcardcurrentpage = 1;
        //LayoutUpdate('optionDropdownChange');
        LayoutFilterinfoUpdate();
        ShowCardView();

    }
});

//#endregion

//#region Redirect to details
$(document).on('click', '.lnk_Equipment_mobile', function (e) {
    e.preventDefault();
    var EquipmentId = $(this).attr('id');
    var ClientLookupId = $(this).attr('clientlookupid');
    $.ajax({
        url: "/Equipment/EquipmentDetails_Mobile",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { EquipmentId: EquipmentId/* ClientLookupId: ClientLookupId*/ },
        success: function (data) {
            $('#MobileEquipmentmaincontainer').html(data);
            $(document).find('#divImageGrid').hide();
            LoadAssetDetails(EquipmentId);
            if (titleText != "") {
                $('#spnlinkToSearch').text(titleText);
                localStorage.setItem("EquipmentViewstatusTextMobile", titleText);
            }
            
        },
        complete: function () {
            SetEquipmentDetailEnvironment();
            SetFixedHeadStyle();

           

        }
    });

});
function ZoomImage(element) {
    element.elevateZoom(ZoomConfig);
}
function SetEquipmentDetailEnvironment() {
    CloseLoader();
    ZoomImage($(document).find('#EquipZoom'));

    SetFixedHeadStyle();
    $('#tabscroll').mobiscroll().nav({
        type: 'tab'
    });
}

function RedirectionToDetails_Mobile(EquipmentId) {
    $.ajax({
        url: "/Equipment/EquipmentDetails_Mobile",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { EquipmentId: EquipmentId },
        success: function (data) {
            $('#MobileEquipmentmaincontainer').html(data);
            $(document).find(".m-portlet").find("button.active").removeClass('active');
            $(document).find(".m-portlet").find("[data-tab='" + tab + "']").addClass('active');
            $(document).find("#" + tab).css("display", "block");
            loadTabDetails(tab);
            if (titleText != "") {
                $('#spnlinkToSearch').text(titleText);
            }
        },
        complete: function () {
            SetEquipmentDetailEnvironment();
            SetFixedHeadStyle();
        }
    });

}
//endregion



//#region Add QR scanner

//#region Parts QR Reader


function QrScannerEquipmentSearch_Mobile() {
   
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').addClass("slide-active");
        StartQRReaderSearch_Mobile();
    }
}
$(document).on('click', '#closeQrScanner_Mobile', function () {
    if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').removeClass('slide-active');
    }
    if (!$(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
        $(document).find('#QrCodeReaderModal_Mobile').removeClass('slide-active');
        StopCamera(); // using same method from somax_main.js
    }
});
function StartQRReaderSearch_Mobile(Module) {
    Html5Qrcode.getCameras().then(devices => {
        if (devices && devices.length) {
            scanner = new Html5Qrcode('reader', false);
            scanner.start({ facingMode: "environment" }, {
                fps: 10,
                qrbox: 150,
                //aspectRatio: aspectratio //1.7777778
            }, success => {

                onScanSuccessEquipmentSearch_Mobile(success);

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

//#region Equipment qr scanner
function onScanSuccessEquipmentSearch_Mobile(decodedText) {
    $.ajax({
        url: "/WorkOrder/GetEquipmentIdByClientLookUpId?clientLookUpId=" + decodedText,
        type: "GET",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if ($(document).find('#QrCodeReaderModal_Mobile').hasClass('slide-active')) {
                $(document).find('#QrCodeReaderModal_Mobile').removeClass("slide-active");
            }
            if (data.EquipmentId > 0) {
                $(document).find('#txtColumnCompSearch').val(decodedText);
                TextSearchMain();
            }
            else {
                //Show Error Swal
                ShowErrorAlert(getResourceValue('spnInvalidQrCodeMsgforEquipment').replace('${decodedText}', decodedText));
            }
        },
        complete: function () {
            StopCamera();
            CloseLoader();
        },
        error: function (xhr) {
            ShowErrorAlert(getResourceValue("somethingWentWrongAlert"));
            CloseLoader();
        }
    });
}
//#endregion
//#endregion Add QR scanner

//#region Description
$(document).on('click', '.equipmentCardViewMoreDetailsPopup', function () {
    $(document).find('#equipmentdetaildesmodaltext').text($(this).find("span").text());
    $(document).find('#summarydescriptionmodaltitle').text($(this).find("p").text());
    $(document).find('#equipmentdetaildesmodal').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', '#equipmentdetaildesmodalHide', function () {
    $(document).find('#equipmentdetaildesmodal').removeClass('slide-active');
});

//#endregion
//#region V2-853 Reset Grid
$('#ResetGridBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("EquipmentViewstatusMobile");
        localstorageKeys.push("EquipmentViewstatusTextMobile");
        DeleteGridLayout_Mobile('Equipment_Mobile_Search', localstorageKeys);
        GenerateSearchListMain('', true);
        window.location.href = "../Equipment/Index?page=Maintenance_Assets";
    });
});
//#endregion

//#region V2-957
$(document).on('click', '#sidebarCollapse', function () {
    $('#sidebar').addClass('slide-active').trigger('mbsc-enhance');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    //$(document).find("#AssetGroup1Id").rules('remove', 'required');
    //$(document).find("#ddlType").rules('remove', 'required');
});
$(document).on('click', '.EqpDataAdvSrchclearstate, .overlay', function () {
    $('#sidebar').removeClass('slide-active');
    $('.overlay').fadeOut();
});
$("#btnEqpDataAdvSrch").on('click', function (e) {
    //run = true;
    $(document).find('#txtColumnCompSearch').val('');
    //searchresult = [];
    $('#sidebar').removeClass('slide-active');
    $('.overlay').fadeOut();
    DepartmentValEquipment = $("#ddlDepartment").val();
    LineValEquipment = $("#ddlLine").val();
    SystemInfoValEquipment = $("#ddlSystemInfo").val();
    AssetAvailability = $("#ddlassetAvailability").val();
    EquipmentAdvSearch();
    cardviewstartvalue = 0;
    grdcardcurrentpage = 1;
    LayoutFilterinfoUpdate();
    ShowCardView();
});

function EquipmentAdvSearch() {
    var InactiveFlag = false;
    $('#txtColumnCompSearch').val('');
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("#" + this.id + "").parents('label').children('span').eq(0).text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossMain" aria-hidden="true"></a></span>';
        }
        if ($(this).attr('id') == "ddlassetAvailability") {
            if ($(this).val() == null && AssetAvailability != null) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("#" + this.id + "").parents('label').children('span').eq(0).text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossMain" aria-hidden="true"></a></span>';
            }
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#searchfilteritems").html(searchitemhtml);
    $("#spnControlCounter").text(selectCount);
    //$('#liSelectCount').text(selectCount + ' filters applied');
}
function clearAdvanceSearch() {
    $("#ddlType").val("").trigger('change');
    $("#ddlDepartment").val("").trigger('change');
    $("#ddlLine").val("").trigger('change');
    $("#ddlSystemInfo").val("").trigger('change');
    $("#ddlassetAvailability").val("").trigger('change');
    $('.adv-item').val("");

    $("#AssetGroup1Id").val("").trigger('change');
    $("#AssetGroup2Id").val("").trigger('change');
    $("#AssetGroup3Id").val("").trigger('change');
    $("#AccountSearchId").val("").trigger('change');

    selectCount = 0;
    $("#spnControlCounter").text(selectCount);
    //$('#liSelectCount').text(selectCount + ' filters applied');
    newEle = "";
    $('#dvFilterSearchSelect2').find('span').html('');
    $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');

    DepartmentValEquipment = $("#ddlDepartment").val();
    LineValEquipment = $("#ddlLine").val();
    SystemInfoValEquipment = $("#ddlSystemInfo").val();
    AssetAvailability = $("#ddlassetAvailability").val();
}
//#endregion
//#region V2-1023
$(document).on('keyup', '#tblEquipmentMobileSearch_paginate .paginate_input', function () {
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