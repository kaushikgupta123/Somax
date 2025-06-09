var order = '1';
var orderDir = 'asc';
var gridName = "Sanitation_Mobile_Search";
var titleText = "";
var DefaultLayoutInfo = '{"time":currentTime,"start":0,"length":10,"order":[[1,"asc"]],"search":{"search":"","smart":true,"regex":false,"caseInsensitive":true},"columns":[],"ColReorder":[]}';
var ZoomConfig = { zoomType: "window", lensShape: "round", lensSize: 1000, zoomWindowFadeIn: 500, zoomWindowFadeOut: 500, lensFadeIn: 100, lensFadeOut: 100, easing: true, scrollZoom: true, zoomWindowWidth: 450, zoomWindowHeight: 450 };
var run = false;

var cardviewstartvalue = 0;
var cardviewlength = 10;
var grdcardcurrentpage = 1;
var CustomQueryDisplayId;

var SortByDropdown;
var AllStatusPopup;
var PassedPopup;
var FailedPopup;
var CompletedPopup;
var CustomQueryDropdown;
var CreateStartDateVw = '';
var CreateEndDateVw = '';
var CompleteStartDateVw = '';
var CompleteEndDateVw = '';
var FailedStartDateVw = '';
var FailedEndDateVw = '';
var PassedStartDateVw = '';
var PassedEndDateVw = '';
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
var mobilecurrenttoday = [new Date(), new Date()];
var now = new Date();
var selectCount = 0;
$(document).ready(function () {
    var MaintCompstatus = localStorage.getItem("SANITATIONMOBILESTATUS");
    if (MaintCompstatus) {
        CustomQueryDisplayId = MaintCompstatus;
    }
    else {
        MaintCompstatus = "1";
        CustomQueryDisplayId = MaintCompstatus;
        localStorage.setItem("SANITATIONMOBILESTATUS", "1");
    }
    $("#SanCustomQueryDropdown li").removeClass('active');
    $("#SanCustomQueryDropdown li[data-value='" + CustomQueryDisplayId + "']").addClass('active');
    titleText = $("#SanCustomQueryDropdown li[data-value='" + CustomQueryDisplayId + "']").text();
    $('#sanitationsearchtitle').text(titleText);
    if (localStorage.getItem("sanjobstatustext") === null) {
        localStorage.setItem("sanjobstatustext", titleText);
    }

    var strCreateStartDateVw = localStorage.getItem('SJCreateStartDateVw');
    if (strCreateStartDateVw) {
        CreateStartDateVw = strCreateStartDateVw;
    }
    var endCreateEndDateVw = localStorage.getItem('SJCreateEndDateVw');
    if (endCreateEndDateVw) {
        CreateEndDateVw = endCreateEndDateVw;
    }

    var strCompleteStartDateVw = localStorage.getItem('SJCompleteStartDateVw');
    if (strCompleteStartDateVw) {
        CompleteStartDateVw = strCompleteStartDateVw;
    }
    var endCompleteEndDateVw = localStorage.getItem('SJCompleteEndDateVw');
    if (endCompleteEndDateVw) {
        CompleteEndDateVw = endCompleteEndDateVw;
    }

    var strFailedStartDateVw = localStorage.getItem('SJFailedStartDateVw');
    if (strFailedStartDateVw) {
        FailedStartDateVw = strFailedStartDateVw;
    }
    var endFailedEndDateVw = localStorage.getItem('SJFailedEndDateVw');
    if (endFailedEndDateVw) {
        FailedEndDateVw = endFailedEndDateVw;
    }

    var strPassedStartDateVw = localStorage.getItem('SJPassedStartDateVw');
    if (strPassedStartDateVw) {
        PassedStartDateVw = strPassedStartDateVw;
    }
    var endPassedEndDateVw = localStorage.getItem('SJPassedEndDateVw');
    if (endPassedEndDateVw) {
        PassedEndDateVw = endPassedEndDateVw;
    }

    var sanitationjobstatus = localStorage.getItem("SANITATIONMOBILESTATUS");
    if (sanitationjobstatus) {
        var text = "";
        CustomQueryDisplayId = sanitationjobstatus;

        if (sanitationjobstatus === '11' || sanitationjobstatus === '12' || sanitationjobstatus === '13' ||
            sanitationjobstatus === '14' || sanitationjobstatus === '15' || sanitationjobstatus === '16') {
            $('#cmbcreateview').val(sanitationjobstatus).trigger('change');
            $("#SanCustomQueryDropdown li").removeClass("active");
            $("#SanCustomQueryDropdown li[data-value='0']").addClass("active");
            text = $("#SanCustomQueryDropdown li[data-value='0']").text();
            $('#sanitationsearchtitle').text(text);
        }
        else if (sanitationjobstatus === '17' || sanitationjobstatus === '18' || sanitationjobstatus === '19' ||
            sanitationjobstatus === '20' || sanitationjobstatus === '21' || sanitationjobstatus === '22') {
            $('#cmbcompletedview').val(sanitationjobstatus).trigger('change');
            $("#SanCustomQueryDropdown li").removeClass("active");
            $("#SanCustomQueryDropdown li[data-value='9']").addClass("active");
            text = $("#SanCustomQueryDropdown li[data-value='9']").text();
            $('#sanitationsearchtitle').text(text);
        }
        else if (sanitationjobstatus === '23' || sanitationjobstatus === '24' || sanitationjobstatus === '25' ||
            sanitationjobstatus === '26' || sanitationjobstatus === '27' || sanitationjobstatus === '28') {
            $('#cmbfailedview').val(sanitationjobstatus).trigger('change');
            $("#SanCustomQueryDropdown li").removeClass("active");
            $("#SanCustomQueryDropdown li[data-value='8']").addClass("active");
            text = $("#SanCustomQueryDropdown li[data-value='8']").text();
            $('#sanitationsearchtitle').text(text);
        }
        else if (sanitationjobstatus === '29' || sanitationjobstatus === '30' || sanitationjobstatus === '31' ||
            sanitationjobstatus === '32' || sanitationjobstatus === '33' || sanitationjobstatus === '34') {
            $('#cmbpassedview').val(sanitationjobstatus).trigger('change');
            $("#SanCustomQueryDropdown li").removeClass("active");
            $("#SanCustomQueryDropdown li[data-value='10']").addClass("active");
            text = $("#SanCustomQueryDropdown li[data-value='10']").text();
            $('#sanitationsearchtitle').text(text);
        }
        GetDatatableLayout();
        ShowCardView();

        $('#sanitationJobSearchModel_TextSearchList').val(sanitationjobstatus).trigger('change.select2');

        $('#SanCustomQueryDropdown li').each(function (index, value) {
            if ($(this).attr('id') == CustomQueryDisplayId) {

                text = $(this).text();
                $('#sanitationsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
        if (sanitationjobstatus === '11' || sanitationjobstatus === '12' || sanitationjobstatus === '13' ||
            sanitationjobstatus === '14' || sanitationjobstatus === '15' || sanitationjobstatus === '16') {
            if (sanitationjobstatus === '16')
                text = text + " - " + $('#createdaterange').val();
            else
                text = text + " - " + $(document).find('#cmbcreateview option[value=' + sanitationjobstatus + ']').text();
            $('#sanitationsearchtitle').text(text);
        }
        else if (sanitationjobstatus === '17' || sanitationjobstatus === '18' || sanitationjobstatus === '19' ||
            sanitationjobstatus === '20' || sanitationjobstatus === '21' || sanitationjobstatus === '22') {
            if (sanitationjobstatus === '22')
                text = text + " - " + $('#completedaterange').val();
            else
                text = text + " - " + $(document).find('#cmbcompletedview option[value=' + sanitationjobstatus + ']').text();
            $('#sanitationsearchtitle').text(text);
        }
        else if (sanitationjobstatus === '23' || sanitationjobstatus === '24' || sanitationjobstatus === '25' ||
            sanitationjobstatus === '26' || sanitationjobstatus === '27' || sanitationjobstatus === '28') {
            if (sanitationjobstatus === '28')
                text = text + " - " + $('#faileddaterange').val();
            else
                text = text + " - " + $(document).find('#cmbfailedview option[value=' + sanitationjobstatus + ']').text();
            $('#sanitationsearchtitle').text(text);
        }
        else if (sanitationjobstatus === '29' || sanitationjobstatus === '30' || sanitationjobstatus === '31' ||
            sanitationjobstatus === '32' || sanitationjobstatus === '33' || sanitationjobstatus === '34') {
            if (sanitationjobstatus === '34')
                text = text + " - " + $('#passedaterange').val();
            else
                text = text + " - " + $(document).find('#cmbpassedview option[value=' + sanitationjobstatus + ']').text();
            $('#sanitationsearchtitle').text(text);
        }

    }


    mobiscroll.settings = {
        lang: 'en',                                       // Specify language like: lang: 'pl' or omit setting to use default
        theme: 'ios',                                     // Specify theme like: theme: 'ios' or omit setting to use default
        themeVariant: 'light'                             // More info about themeVariant: https://docs.mobiscroll.com/4-10-9/javascript/popup#opt-themeVariant
    };

    CustomQueryDropdown = $('#SanCustomQueryDropdownDiv').mobiscroll().popup({
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

    $('#SanCustomQueryDropdown').mobiscroll().listview({
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
    AllStatusPopup = $('#AllStatusDateRangeModal').mobiscroll().popup({
        display: 'center',
        anchor: '#AllStatusDateRangeModal',
        buttons: ['close', 
            {
                text: 'OK',
                icon: 'checkmark',
                handler: function (event) {
                    //alert('Custom button clicked!');
                    run = true;
                    var daterangeval = $(document).find('#cmbcreateview').val();
                    if (daterangeval == '') {
                        return;
                    }
                    CustomQueryDisplayId = daterangeval;
                    if (daterangeval != '16') {
                        CreateStartDateVw = '';
                        CreateEndDateVw = '';
                        localStorage.removeItem('SJCreateStartDateVw');
                        localStorage.removeItem('SJCreateEndDateVw');
                    }
                    else {
                        localStorage.setItem('SJCreateStartDateVw', CreateStartDateVw);
                        localStorage.setItem('SJCreateEndDateVw', CreateEndDateVw);
                    }
                    AllStatusPopup.hide();
                    var text = $('#SanCustomQueryDropdown').find('li').eq(0).text();

                    if (daterangeval != '16')

                        text = text + " - " + $(document).find('#cmbcreateview option[value=' + daterangeval + ']').text();
                    else
                        text = text + " - " + $('#createdaterange').val();

                    $('#sanitationsearchtitle').text(text);
                    localStorage.setItem("sanjobstatustext", text);
                    localStorage.setItem("SANITATIONMOBILESTATUS", daterangeval);
                    $(document).find('#txtColumnSearch').val('');
                    CustomQueryDisplayId = daterangeval;
                    localStorage.setItem("SANITATIONMOBILESTATUS", CustomQueryDisplayId);
                    //if (daterangeval.length !== 0) {
                        ShowbtnLoaderclass("LoaderDrop");
                        ShowCardView();
                    //}
                }
            }],
        cssClass: 'mbsc-no-padding md-vertical-list modal-pop-adj'
    }).mobiscroll('getInst');

    PassedPopup = $('#PassedDateRangeModal').mobiscroll().popup({
        display: 'center',
        anchor: '#PassedDateRangeModal',
        buttons: ['close',
            {
                text: 'OK',
                icon: 'checkmark',
                handler: function (event) {
                    run = true;
                    var daterangeval = $(document).find('#cmbpassedview').val();
                    if (daterangeval == '') {
                        return;
                    }
                    CustomQueryDisplayId = daterangeval;
                    if (daterangeval != '34') {
                        PassedStartDateVw = '';
                        PassedEndDateVw = '';
                        localStorage.removeItem('SJPassedStartDateVw');
                        localStorage.removeItem('SJPassedEndDateVw');
                    }
                    else {
                        localStorage.setItem('SJPassedStartDateVw', PassedStartDateVw);
                        localStorage.setItem('SJPassedEndDateVw', PassedEndDateVw);
                    }
                    PassedPopup.hide();
                    var text = $('#SanCustomQueryDropdown').find('li').eq(2).text();

                    //-------------------------------------------------------
                    if (daterangeval != '34')
                        text = text + " - " + $(document).find('#cmbpassedview option[value=' + daterangeval + ']').text();
                    else
                        text = text + " - " + $('#passedaterange').val();
                    //-------------------------------------------------------

                    $('#sanitationsearchtitle').text(text);
                    localStorage.setItem("sanjobstatustext", text);
                    localStorage.setItem("SANITATIONMOBILESTATUS", daterangeval);
                    
                    $(document).find('#txtColumnSearch').val('');
                    CustomQueryDisplayId = daterangeval;
                    localStorage.setItem("SANITATIONMOBILESTATUS", CustomQueryDisplayId);
                    
                        ShowbtnLoaderclass("LoaderDrop");
                        ShowCardView();
                }
            }],
        cssClass: 'mbsc-no-padding md-vertical-list modal-pop-adj'
    }).mobiscroll('getInst');

    FailedPopup = $('#FailedDateRangeModal').mobiscroll().popup({
        display: 'center',
        anchor: '#FailedDateRangeModal',
        buttons: ['close',
            {
                text: 'OK',
                icon: 'checkmark',
                handler: function (event) {
                        run = true;
                        var daterangeval = $(document).find('#cmbfailedview').val();
                        if (daterangeval == '') {
                            return;
                        }
                        CustomQueryDisplayId = daterangeval;
                        if (daterangeval != '28') {
                            FailedStartDateVw = '';
                            FailedEndDateVw = '';
                            localStorage.removeItem('SJFailedStartDateVw');
                            localStorage.removeItem('SJFailedEndDateVw');
                        }
                        else {
                            localStorage.setItem('SJFailedStartDateVw', FailedStartDateVw);
                            localStorage.setItem('SJFailedEndDateVw', FailedEndDateVw);
                    }
                    FailedPopup.hide();
                    var text = $('#SanCustomQueryDropdown').find('li').eq(5).text();

                        //-------------------------------------------------------
                        if (daterangeval != '28')
                            text = text + " - " + $(document).find('#cmbfailedview option[value=' + daterangeval + ']').text();
                        else
                            text = text + " - " + $('#faileddaterange').val();
                        //-------------------------------------------------------

                    $('#sanitationsearchtitle').text(text);
                        localStorage.setItem("sanjobstatustext", text);
                    localStorage.setItem("SANITATIONMOBILESTATUS", daterangeval);
                        
                        $(document).find('#txtColumnSearch').val('');
                        CustomQueryDisplayId = daterangeval;
                    localStorage.setItem("SANITATIONMOBILESTATUS", CustomQueryDisplayId);
                        
                            ShowbtnLoaderclass("LoaderDrop");
                            ShowCardView();
                       
                }
            }],
        cssClass: 'mbsc-no-padding md-vertical-list modal-pop-adj'
    }).mobiscroll('getInst');

    CompletedPopup = $('#CompletedDateRangeModal').mobiscroll().popup({
        display: 'center',
        anchor: '#CompletedDateRangeModal',
        buttons: ['close',
            {
                text: 'OK',
                icon: 'checkmark',
                handler: function (event) {
                    
                        run = true;
                        var daterangeval = $(document).find('#cmbcompletedview').val();
                        if (daterangeval == '') {
                            return;
                        }
                        CustomQueryDisplayId = daterangeval;
                        if (daterangeval != '22') {
                            CompleteStartDateVw = '';
                            CompleteEndDateVw = '';
                            localStorage.removeItem('SJCompleteStartDateVw');
                            localStorage.removeItem('SJCompleteEndDateVw');
                        }
                        else {
                            localStorage.setItem('SJCompleteStartDateVw', CompleteStartDateVw);
                            localStorage.setItem('SJCompleteEndDateVw', CompleteEndDateVw);
                    }
                    CompletedPopup.hide();
                        var text = $('#SanCustomQueryDropdown').find('li').eq(6).text();

                        //-------------------------------------------------------
                        if (daterangeval != '22')
                            text = text + " - " + $(document).find('#cmbcompletedview option[value=' + daterangeval + ']').text();
                        else
                        text = text + " - " + $('#completedaterange').val();
                        //-------------------------------------------------------

                    $('#sanitationsearchtitle').text(text);
                        localStorage.setItem("sanjobstatustext", text);
                    localStorage.setItem("SANITATIONMOBILESTATUS", daterangeval);
                        
                        CustomQueryDisplayId = daterangeval;
                    localStorage.setItem("SANITATIONMOBILESTATUS", CustomQueryDisplayId);
                       
                            ShowbtnLoaderclass("LoaderDrop");
                            ShowCardView();
                      
                }
            }],
        cssClass: 'mbsc-no-padding md-vertical-list modal-pop-adj'
    }).mobiscroll('getInst');


    $('.rangepicker').click(function () {
        let id = this.id;
        $('#' + id).mobiscroll('show');
        return false;
    });

    //#region V2-1048
    if ($('#IsJobAddFromDashboard').val() != undefined && $('#IsJobAddFromDashboard').val().toLowerCase() === 'true') {
        SetControls();
        $('#AddSanitationModalpopup').addClass('slide-active').trigger('mbsc-enhance');
    }
//#endregion
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
    $('.mobidtpicker').mobiscroll().calendar({
        display: 'bottom',
        touchUi: true,
        /* weekDays: 'min',*/
        //yearChange: false,
        //max: new Date(),
        /* months: 1,*/
    }).inputmask('mm/dd/yyyy');
});

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
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#advsearchfilteritems"), $(".filteritemcount"));
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

function ShowCardView() {
    $.ajax({
        "url": "/SanitationJob/GetSanitationJobMainGridMobile",
        type: 'POST',
        dataType: 'html',
        data: {
            currentpage: grdcardcurrentpage,
            start: cardviewstartvalue,
            length: cardviewlength,
            CustomQueryDisplayId: CustomQueryDisplayId,
            SearchText: LRTrim($(document).find('#txtColumnSearch').val()),
            ClientLookupId : LRTrim($("#JobId").val()),
            Description: LRTrim($("#Description").val()),
            ChargeTo_ClientLookupId: LRTrim($("#ChargeTo").val()),
            ChargeTo_Name: LRTrim($('#ChargeToName').val()),
            AssetLocation: LRTrim($('#AssetLocation').val()),
            Status: LRTrim($("#Status").val()),
            Shift: LRTrim($("#Shift").val()),
            AssetGroup1_ClientLookUpId: LRTrim($('#AssetGroup1ClientLookUpId').val()),
            AssetGroup2_ClientLookUpId: LRTrim($("#AssetGroup2ClientLookUpId").val()),
            AssetGroup3_ClientLookUpId: LRTrim($("#AssetGroup3ClientLookUpId").val()),
            CreateDate: ValidateDate($("#Created").val()),
            CreateBy: LRTrim($("#CreateBy").val()),
            Assigned:LRTrim($("#Assigned").val()),
            CompleteDate: LRTrim(ValidateDate($("#CompleteDate").val())),
            VerifiedBy: LRTrim($("#VeryfiedBy").val()),
            VerifiedDate: LRTrim(ValidateDate($("#VeryfiedDate").val())),
            Extracted: LRTrim($("#Extracted").val()),
            ScheduledDate: LRTrim(ValidateDate($("#ScheduledDate").val())),
            CreateStartDateVw: CreateStartDateVw,
            CreateEndDateVw: CreateEndDateVw,
            CompleteStartDateVw: CompleteStartDateVw,
            CompleteEndDateVw: CompleteEndDateVw,
            FailedStartDateVw: FailedStartDateVw,
            FailedEndDateVw: FailedEndDateVw,
            PassedStartDateVw:PassedStartDateVw,
            PassedEndDateVw: PassedEndDateVw,
            Order: order,
            orderDir: orderDir
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#ActiveCard').html(data).show();
            $(document).find('#tblsanitationjob_paginate li').each(function (index, value) {
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
            $(document).find('#cardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(cardviewlength).trigger('change.select2');
            HidebtnLoader("SrchBttnNew");
            HidebtnLoader("txtColumnSearch");

            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', '#tblsanitationjob_paginate .paginate_button', function () {
    var currentselectedpage = parseInt($(document).find('#tblsanitationjob_paginate .pagination').find('.active').text());
    cardviewlength = $(document).find('#cardviewpagelengthdrp').val();
    cardviewstartvalue = cardviewlength * (parseInt($(this).find('.page-link').text()) - 1);
    var lastpage = parseInt($(this).prev('li').data('currentpage'));

    if ($(this).attr('id') == 'tbl_previous') {
        if (currentselectedpage == 1) {
            return false;
        }
        cardviewstartvalue = cardviewlength * (currentselectedpage - 2);
        grdcardcurrentpage = grdcardcurrentpage - 1;
    }
    else if ($(this).attr('id') == 'tbl_next') {
        if (currentselectedpage == lastpage) {
            return false;
        }
        cardviewstartvalue = cardviewlength * (currentselectedpage);
        grdcardcurrentpage = grdcardcurrentpage + 1;
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
        grdcardcurrentpage = parseInt($(this).prevAll('li').eq(1).text());
        cardviewstartvalue = cardviewlength * (grdcardcurrentpage - 1);
    }
    else {
        grdcardcurrentpage = $(this).data('currentpage');
    }
    LayoutUpdate('pagination');
    ShowCardView();
});

//#region Card view griddatalayout update
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
function getfilterinfoarray(txtsearchelement,advsearchcontainer) {
    var filterinfoarray = [];
    var f = new filterinfo('searchstring', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parents('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);
        }
        if ($(this).parent('div').find('div').hasClass('range-timeperiod')) {
            if ($(this).parents('div').find('input').val() !== '' && $(this).val() == '10') {
                f = new filterinfo('this-' + $(this).attr('id'), $(this).parent('div').find('input').val());
                filterinfoarray.push(f);
            }
        }
    });
    return filterinfoarray;
}
function setsearchui(data, txtsearchelement, searchstringcontainer, advcountercontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if (item.key == 'searchstring' && item.value) {
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
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#advsearchfilteritems"));
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
//function GetDatatableLayout() {
//    $.ajax({
//        "url": "/Base/GetLayout",
//        "data": {
//            GridName: gridName
//        },
//        "async": false,
//        "dataType": "json",
//        "success": function (json) {
//            if (json.LayoutInfo !== '') {
//                var LayoutInfo = JSON.parse(json.LayoutInfo);
//                var pageclicked = (LayoutInfo.start / LayoutInfo.length);
//                cardviewlength = LayoutInfo.length;
//                cardviewstartvalue = cardviewlength * pageclicked;
//                grdcardcurrentpage = pageclicked + 1;
//                order = LayoutInfo.order[0][0];
//                orderDir = LayoutInfo.order[0][1];

//                if (json.FilterInfo !== '') {
//                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#advsearchfilteritems"));
//                }
//            }
//            else {
//                DefaultLayoutInfo = DefaultLayoutInfo.replace('currentTime', new Date().getTime());
//                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), '');
//                $.ajax({
//                    "url": gridStateSaveUrl,
//                    "data": {
//                        GridName: gridName,
//                        LayOutInfo: (DefaultLayoutInfo),
//                        FilterInfo: JSON.stringify(filterinfoarray)
//                    },
//                    "async": false,
//                    "dataType": "json",
//                    "type": "POST",
//                    "success": function () { return; }
//                });
//            }
//        }
//    });
//}

$(document).on('change', '#cardviewpagelengthdrp', function () {
    cardviewlength = $(this).val();
    grdcardcurrentpage = parseInt(cardviewstartvalue / cardviewlength) + 1;
    cardviewstartvalue = parseInt((grdcardcurrentpage - 1) * cardviewlength);

    LayoutUpdate('pagelength');
    ShowCardView();
});

//#endregion

//#region View dropdown change
$(document).on('click', '#SanCustomQueryDropdown li', function (e) {
    
    var optionval = $(this).data('value');
    $(this).addClass('active').siblings().removeClass('active');
    CustomQueryDropdown.hide();
    $(".updateArea").hide();
    $(".actionBar").fadeIn();

    run = true;
    if (optionval == '0') {
        var val = localStorage.getItem("SANITATIONMOBILESTATUS");
        if (val == '11' || val == '12' || val == '13' || val == '14' || val == '15' || val == '16') {
            $('#cmbcreateview').val(val).trigger('change');
        }
        AllStatusPopup.show();
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        FailedStartDateVw = '';
        FailedEndDateVw = '';
        PassedStartDateVw = '';
        PassedEndDateVw = '';
        return;
    }
    else {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('SJCreateStartDateVw');
        localStorage.removeItem('SJCreateEndDateVw');
        $(document).find('#cmbcreateview').val('').trigger('change');
    }



    if (optionval == '8') {
        var val = localStorage.getItem("SANITATIONMOBILESTATUS");
        if (val == '23' || val == '24' || val == '25' || val == '26' || val == '27' || val == '28') {
            $('#cmbfailedview').val(val).trigger('change');
        }
        FailedPopup.show();

        CreateStartDateVw = '';
        CreateEndDateVw = '';
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        PassedStartDateVw = '';
        PassedEndDateVw = '';
        return;
    }

    else {
        FailedStartDateVw = '';
        FailedEndDateVw = '';
        localStorage.removeItem('SJFailedStartDateVw');
        localStorage.removeItem('SJFailedEndDateVw');
        $(document).find('#cmbfailedview').val('').trigger('change');
    }
    if (optionval == '9') {
        var val = localStorage.getItem("SANITATIONMOBILESTATUS");
        if (val == '17' || val == '18' || val == '19' || val == '20' || val == '21' || val == '22') {
            $('#cmbcompletedview').val(val).trigger('change');
        }
        CompletedPopup.show();
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        FailedStartDateVw = '';
        FailedEndDateVw = '';
        PassedStartDateVw = '';
        PassedEndDateVw = '';
        return;
    }

    else {
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        localStorage.removeItem('SJCompleteStartDateVw');
        localStorage.removeItem('SJCompleteEndDateVw');
        $(document).find('#cmbcompletedview').val('').trigger('change');
    }
    if (optionval == '10') {
        var val = localStorage.getItem("SANITATIONMOBILESTATUS");
        if (val == '29' || val == '30' || val == '31' || val == '32' || val == '33' || val == '34') {
            $('#cmbpassedview').val(val).trigger('change');
        }
        PassedPopup.show();
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        CompleteStartDateVw = '';
        CompleteEndDateVw = '';
        FailedStartDateVw = '';
        FailedEndDateVw = '';

        return;
    }

    else {
        PassedStartDateVw = '';
        PassedEndDateVw = '';
        localStorage.removeItem('SJPassedStartDateVw');
        localStorage.removeItem('SJPassedEndDateVw');
        $(document).find('#cmbpassedview').val('').trigger('change');
    }

    if (optionval != '0') {
        $('#sanitationsearchtitle').text($(this).text());
        localStorage.setItem("sanjobstatustext", $(this).text());
    }
    else {
        $('#sanitationsearchtitle').text(getResourceValue("OpenSanitationJobsAlert"));
        localStorage.setItem("sanjobstatustext", getResourceValue("OpenSanitationJobsAlert"));
    }




    $(document).find('#searcharea').hide("slide");
    localStorage.setItem("SANITATIONMOBILESTATUS", optionval);
    CustomQueryDisplayId = optionval;
    titleText = $("#SanCustomQueryDropdown li[data-value='" + CustomQueryDisplayId + "']").text();
    $('#sanitationsearchtitle').text(titleText);

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


$(document).on('change', '#cmbcreateview', function (e) {
    var thielement = $(this);
    CustomQueryDisplayId = thielement.val();
    if (thielement.val() == '16') {
        var strtlocal = localStorage.getItem('SJCreateStartDateVw');
        if (strtlocal) {
            CreateStartDateVw = strtlocal;
        }
        else {
            CreateStartDateVw = now;
        }
        var endlocal = localStorage.getItem('SJCreateEndDateVw');
        if (endlocal) {
            CreateEndDateVw = endlocal;
        }
        else {
            CreateEndDateVw = now;
        }
        $(document).find('#timeperiodcontainerForCreateDate').show();
        week = [CreateStartDateVw, CreateEndDateVw];
        $('#createdaterange').mobiscroll().range({
            showOnTap: false,
            showOnFocus: false,
            showSelector: false,
            returnFormat: 'mm/dd/yyyy',
            //touchUi: true,
            //startInput: CreateStartDateVw,
            //endInput: CreateEndDateVw,
            onInit: function (event, inst) {
                inst.setVal(week, true);
            },
            onSet: function (event, inst) {
                var values = inst.getVal();
                var dates = event.valueText.split(' - ');
                var dates = event.valueText.split(' - ');
                CreateStartDateVw = dates[0];
                CreateEndDateVw = dates[1];

            }
        });
    }
    else {
        CreateStartDateVw = '';
        CreateEndDateVw = '';
        localStorage.removeItem('SJCreateStartDateVw');
        localStorage.removeItem('SJCreateEndDateVw');
        $(document).find('#timeperiodcontainerForCreateDate').hide();
    }
});

$(document).on('change', '#cmbcompletedview', function (e) {
    var thielement = $(this);
    sjStatus = thielement.valGetSanitationJobMainGridMobile

    if (thielement.val() == '22') {
        CompleteStartDateVw = today;
        CompleteEndDateVw = today;
        var strtlocal = localStorage.getItem('SJCompleteStartDateVw');
        if (strtlocal) {
            CompleteStartDateVw = strtlocal;
        }
        else {
            CompleteStartDateVw = now;
        }
        var endlocal = localStorage.getItem('SJCompleteEndDateVw');
        if (endlocal) {
            CompleteEndDateVw = endlocal;
        }
        else {
            CompleteEndDateVw = now;
        }
        $(document).find('#completedtimeperiodcontainer').show();
        week = [CompleteStartDateVw, CompleteEndDateVw];
        $('#completedaterange').mobiscroll().range({
            showOnTap: false,
            showOnFocus: false,
            showSelector: false,
            returnFormat: 'mm/dd/yyyy',
            onInit: function (event, inst) {
                inst.setVal(week, true);
            },
            onSet: function (event, inst) {
                var values = inst.getVal();
                var dates = event.valueText.split(' - ');
                CompleteStartDateVw = dates[0];
                CompleteEndDateVw = dates[1];

            }
        });
    }
    else {
        $(document).find('#completedtimeperiodcontainer').hide();
    }
});

$(document).on('change', '#cmbfailedview', function (e) {
    var thielement = $(this);
    sjStatus = thielement.val();

    if (thielement.val() == '28') {
        FailedStartDateVw = today;
        FailedEndDateVw = today;
        var strtlocal = localStorage.getItem('SJFailedStartDateVw');
        if (strtlocal) {
            FailedStartDateVw = strtlocal;
        }
        else {
            FailedStartDateVw = now;
        }
        var endlocal = localStorage.getItem('SJFailedEndDateVw');
        if (endlocal) {
            FailedEndDateVw = endlocal;
        }
        else {
            FailedEndDateVw = now;
        }
        $(document).find('#failedtimeperiodcontainer').show();
        
        week = [FailedStartDateVw, FailedEndDateVw];
        $('#faileddaterange').mobiscroll().range({
            showOnTap: false,
            showOnFocus: false,
            showSelector: false,
            returnFormat: 'mm/dd/yyyy',
            onInit: function (event, inst) {
                inst.setVal(week, true);
            },
            onSet: function (event, inst) {
                var values = inst.getVal();
                var dates = event.valueText.split(' - ');
                FailedStartDateVw = dates[0];
                FailedEndDateVw = dates[1];

            }
        });
       
    }
    else {
        $(document).find('#failedtimeperiodcontainer').hide();
    }
});

$(document).on('change', '#cmbpassedview', function (e) {
    var thielement = $(this);
    sjStatus = thielement.val();

    if (thielement.val() == '34') {
        PassedStartDateVw = today;
        PassedEndDateVw = today;
        var strtlocal = localStorage.getItem('SJPassedStartDateVw');
        if (strtlocal) {
            PassedStartDateVw = strtlocal;
        }
        else {
            PassedStartDateVw = now;
        }
        var endlocal = localStorage.getItem('SJPassedEndDateVw');
        if (endlocal) {
            PassedEndDateVw = endlocal;
        }
        else {
            PassedEndDateVw = now;
        }
        $(document).find('#passedtimeperiodcontainer').show();
        
        week = [PassedStartDateVw, PassedEndDateVw];
        $('#passedaterange').mobiscroll().range({
            showOnTap: false,
            showOnFocus: false,
            showSelector: false,
            returnFormat: 'mm/dd/yyyy',
            onInit: function (event, inst) {
                inst.setVal(week, true);
            },
            onSet: function (event, inst) {
                var values = inst.getVal();
                var dates = event.valueText.split(' - ');
                PassedStartDateVw = dates[0];
                PassedEndDateVw = dates[1];

            }
        });
       
    }
    else {
        $(document).find('#passedtimeperiodcontainer').hide();
    }
});

//#endregion

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
    // clearAdvanceSearch();
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



//#region Sort
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
//#endregion

//#region Description
var summarydescriptionmodaltitle = '';
$(document).on('click', '.sanitationCardViewDescription', function () {
    $(document).find('#sanitationdetaildesmodaltext').text($(this).find("span").text());
    $(document).find('#sanitationdetaildesmodal').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', '#sanitationdetaildesmodalHide', function () {
    if (summarydescriptionmodaltitle != "") {
        $(document).find('.summarydescriptionmodaltitle').text(summarydescriptionmodaltitle);
    }
    $(document).find('#sanitationdetaildesmodal').removeClass('slide-active');
});
$(document).on('click', '.sanitationCardViewDescription', function () {
    summarydescriptionmodaltitle = $(document).find('.summarydescriptionmodaltitle').text();
    var modaltitle = $(this).closest('.gridviewcardspan').text();
    modaltitle = modaltitle.split(':')[0];
    $(document).find('.summarydescriptionmodaltitle').text(modaltitle);
    $(document).find('#sanitationdetaildesmodaltext').text($(this).find("span").text());
    $(document).find('#sanitationdetaildesmodal').addClass('slide-active').trigger('mbsc-enhance');
});

//#endregion
//#region Reset Grid
$('#ResetGridBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("SANITATIONMOBILESTATUS");
        DeleteGridLayout_Mobile('Sanitation_Mobile_Search', localstorageKeys);
        GenerateSearchList('', true);
        window.location.href = "../SanitationJob/Index";
    });
});
//#endregion

//#region AddSanitation
$(document).on('click', '#AddSanitationBtn', function () {
    $(document).find("#AddSanitationRequestBtn_Mobile").css('display', 'inline-block');
    $(document).find("#AddSanitationJobBtn_Mobile").css('display', 'inline-block');
});

//#region AddSanitationRequest
$(document).on('click', "#AddSanitationRequestBtn_Mobile", function (e) {
    $('#SanitationRequestModal_Mobile').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', "#hideSanitationRequestModal_Mobile", function (e) {
    $('#SanitationRequestModal_Mobile').removeClass('slide-active');
});


$(document).on('click', "#santitationRequestOnDemand_mobile,#santitationRequestDescribe_mobile", function (e) {
    e.preventDefault();
    var id = this.id;
    $('#SanitationRequestModal_Mobile').removeClass('slide-active');
    GoSanitationRequestDemandAndDescribe(id);
});
function GoSanitationRequestDemandAndDescribe(id) {
    var url;
    if (id == 'santitationRequestOnDemand_mobile')
        url = "/SanitationJob/AddDemandSR_Mobile";
    else
        url = "/SanitationJob/AddDescribeSR_Mobile";
    $.ajax({
        url: url,
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddSanitationModal').html(data);
        },
        complete: function () {
            SetControls();
            $('#AddSanitationModalpopup').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
}


$(document).on('click', '.btnCancelSR', function () {

    var areaChargeToId = "";
    $(document).find('#AddSanitationModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
    $('#AddSanitationModalpopup').removeClass('slide-active');
    $('#AddSanitationModal').html('');
});
//#endregion
//#region AddSanitationJob
$(document).on('click', "#AddSanitationJobBtn_Mobile", function (e) {
    $('#SanitationJobModal_Mobile').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', "#hideSanitationJobModal_Mobile", function (e) {
    $('#SanitationJobModal_Mobile').removeClass('slide-active');
});

$(document).on('click', "#santitationJobOnDemand_mobile,#santitationJobDescribe_mobile", function (e) {
    e.preventDefault();
    var id = this.id;
    $('#SanitationJobModal_Mobile').removeClass('slide-active');
    GoSanitationJobDemandAndDescribe(id);
});
function GoSanitationJobDemandAndDescribe(id) {
    var url;
    if (id == 'santitationJobOnDemand_mobile')
        url = "/SanitationJob/AddDemandSJ_Mobile";
    else
        url = "/SanitationJob/AddDescribeSJ_Mobile";
    $.ajax({
        url: url,
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddSanitationModal').html(data);
        },
        complete: function () {
            SetControls();
            $('#AddSanitationModalpopup').addClass('slide-active').trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
}


$(document).on('click', '.btnCancelSJ', function () {
    if ($('#IsJobAddFromDashboard').val() != undefined && $('#IsJobAddFromDashboard').val().toLowerCase() === 'true') {
        window.location.href = "../SanitationJob/Index?page=Sanitation_Jobs_Search";
    }
    else {
        var areaChargeToId = "";
        $(document).find('#AddSanitationModalpopup select').each(function (i, item) {
            areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
            $('#' + areaChargeToId).hide();
        });
        $('#AddSanitationModalpopup').removeClass('slide-active');
        $('#AddSanitationModal').html('');
    }
});

//#endregion

// adding an error class and valid class should be done manually
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

function SanitationUpdateOnSuccess(data) {
    CloseLoader();
    if (data.data === "success") {
        if (data.Command === "save" || data.Command === "complete") {
            var message;
            if (data.mode === "add") {
                SuccessAlertSetting.text = getResourceValue("SanitationAddAlert");
            }
            else if (data.mode === "complete") {
                SuccessAlertSetting.text = getResourceValue("SanitationCompleteAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("SanitationUpdateAlert");
            }
            swal(SuccessAlertSetting, function () {
                RedirectToSaDetail(data.SanitationJobId, '')
            });
        }
        else {
            ResetErrorDiv();
            SuccessAlertSetting.text = getResourceValue("SanitationAddAlert");

            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        if (data.Command === "complete") {
            message = getResourceValue(data.Result);
            ShowGenericErrorOnAddUpdate(message);
        }
        else {
            ShowGenericErrorOnAddUpdate(data.Result);
        }
    }
}

//#endregion
//#region V2-1058
$(document).on('click', '#sidebarCollapse', function () {
    $('#sidebar').addClass('slide-active').trigger('mbsc-enhance');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('click', '.SanitationJobDataAdvSrchclearstate, .overlay', function () {
    $('#sidebar').removeClass('slide-active');
    $('.overlay').fadeOut();
});
$("#btnSanitationJobDataAdvSrch").on('click', function (e) {
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
        //if ($(this).attr('id') == "ddlassetAvailability") {
        //    if ($(this).val() == null && AssetAvailability != null) {
        //        selectCount++;
        //        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("#" + this.id + "").parents('label').children('span').eq(0).text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        //    }
        //}
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#advsearchfilteritems").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
}
function clearAdvanceSearch() {
    selectCount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $(document).find("#Status").val("").trigger('change');
    $(document).find("#Shift").val("").trigger('change');
    $(document).find("#CreateBy").val("").trigger('change');
    $(document).find("#Assigned").val("").trigger('change');
    $(document).find("#VeryfiedBy").val("").trigger('change');
    $(document).find("#Extracted").val("").trigger('change'); 
    $('.filteritemcount').text(selectCount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion

