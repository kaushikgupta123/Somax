var WOCMainorder = '0';
var WOCMainorderDir = 'asc';
var gridName = "MaintenanceCompletionWorkbench_Mobile_Search";
var DefaultLayoutInfo = '{"time":currentTime,"start":0,"length":10,"order":[[0,"asc"]],"search":{"search":"","smart":true,"regex":false,"caseInsensitive":true},"columns":[],"ColReorder":[]}';
var dtMaintenanceCompletionWorkbench;
var ZoomConfig = { zoomType: "window", lensShape: "round", lensSize: 1000, zoomWindowFadeIn: 500, zoomWindowFadeOut: 500, lensFadeIn: 100, lensFadeOut: 100, easing: true, scrollZoom: true, zoomWindowWidth: 450, zoomWindowHeight: 450 };
var run = false;

var cardviewstartvalue = 0;
var cardviewlength = 10;
var grdcardcurrentpage = 1;
//var currentorderedcolumn = 1;
//var currentorder = 'asc';
var CustomQueryDisplayId;

var SortByDropdown;
var MaintenanceCompDropdown;

$(document).ready(function () {
    var MaintCompstatus = localStorage.getItem("MaintCompstatusMobile");
    if (MaintCompstatus) {
        CustomQueryDisplayId = MaintCompstatus;
    }
    else {
        MaintCompstatus = "0";
        CustomQueryDisplayId = MaintCompstatus;
        localStorage.setItem("MaintCompstatusMobile", "0");
    }
    $("#MaintenanceCompDropdown li").removeClass('active');
    $("#MaintenanceCompDropdown li[data-value='" + CustomQueryDisplayId + "']").addClass('active');

    GetDatatableLayout();
    ShowCardView();

    ////--- debashis
    mobiscroll.settings = {
        lang: 'en',                                       // Specify language like: lang: 'pl' or omit setting to use default
        theme: 'ios',                                     // Specify theme like: theme: 'ios' or omit setting to use default
        themeVariant: 'light'                             // More info about themeVariant: https://docs.mobiscroll.com/4-10-9/javascript/popup#opt-themeVariant
    };

    MaintenanceCompDropdown = $('#MaintenanceCompDropdownDiv').mobiscroll().popup({
        display: 'bubble',
        anchor: '#ShowMaintenanceCompDropdown',
        buttons: [],
        cssClass: 'mbsc-no-padding md-vertical-list'
    }).mobiscroll('getInst');
    SortByDropdown = $('#SortByDropdownDiv').mobiscroll().popup({
        display: 'bubble',
        anchor: '#ShowSortByDropdown',
        buttons: [],
        cssClass: 'mbsc-no-padding md-vertical-list'
    }).mobiscroll('getInst');

    $('#MaintenanceCompDropdown').mobiscroll().listview({
        enhance: true,
        swipe: false,
    });
    $('#SortByDropdown').mobiscroll().listview({
        enhance: true,
        swipe: false
    });

    $('#ShowMaintenanceCompDropdown').click(function () {
        MaintenanceCompDropdown.show();
        return false;
    });
    $('#ShowSortByDropdown').click(function () {
        SortByDropdown.show();
        return false;
    });

    ////--- debashis

});


function ShowCardView() {
    console.log(WOCMainorder);
    console.log(WOCMainorderDir);
    $.ajax({
        "url": "/DashBoard/GetWorkOrderCompletionWorkbenchCardViewMobile",
        type: 'POST',
        dataType: 'html',
        //contentType: "application/json; charset=utf-8",
        data: {
            currentpage: grdcardcurrentpage,
            start: cardviewstartvalue,
            length: cardviewlength,
            CustomQueryDisplayId: localStorage.getItem("MaintCompstatusMobile"),
            txtSearchval: LRTrim($(document).find('#txtColumnCompSearch').val()),
            Order: WOCMainorder,
            orderDir: WOCMainorderDir
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#ActiveCard').html(data).show();
            $(document).find('#tblMaintenanceCompletionWorkbench_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == grdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
        },
        complete: function () {
            var sortClass = '';
            if (WOCMainorderDir == 'asc') {
                sortClass = 'sorting_asc_mobile';
            }
            else if (WOCMainorderDir == 'desc') {
                sortClass = 'sorting_desc_mobile';
            }
            $(document).find('#SortByDropdown li').removeClass('active sorting_asc_mobile sorting_desc_mobile');
            $(document).find('#SortByDropdown li[data-value="' + WOCMainorder + '"]').addClass('active').addClass(sortClass);
            //if ($(document).find('#spnNoData').length > 0) {
            //    $(document).find('.import-export').prop('disabled', true);
            //}
            //else {
            //    $(document).find('.import-export').prop('disabled', false);
            //}
            //$('#ActiveCard').find('.chksearch').each(function (i) {
            //    var value = $(this).val();
            //    var found = SelectedWoIdToCancel.some(function (el) {
            //        return el.WorkOrderId == value;
            //    });
            //    if (found) {
            //        $(this).prop('checked', true);
            //    }
            //    else {
            //        $(this).prop('checked', false);
            //    }
            //});
            //if (SelectedWoIdToCancel.length > 0) {
            //    $(document).find('#cardScheduleDrop,#cardApproveDrop,.wobtngrdcomplete,.wobtngrdcancel').hide();
            //}
            //else {
            //    $(document).find('#cardScheduleDrop,#cardApproveDrop,.wobtngrdcomplete,.wobtngrdcancel').show();
            //}
            $(document).find('#Dashboardcardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(cardviewlength).trigger('change.select2');
            //HidebtnLoader("btnsortmenu");
            //HidebtnLoader("layoutsortmenu");
            HidebtnLoader("SrchBttnNew");
            //HidebtnLoader("sidebarCollapse");
            HidebtnLoader("txtColumnCompSearch");
            //HidebtnLoader("btnWoAdd");

            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//function generateMaintenanceCompletionWorkbench() {
//    var EquipmentId = 0;//$('#FleetAssetModel_EquID').val();
//    var rCount = 0;
//    var visibility;
//    var duration = parseInt($(document).find('#MaintenanceDropdown').val());
//    if ($(document).find('#tblMaintenanceCompletionWorkbench').hasClass('dataTable')) {
//        dtMaintenanceCompletionWorkbench.destroy();
//    }
//    dtMaintenanceCompletionWorkbench = $("#tblMaintenanceCompletionWorkbench").DataTable({
//        colReorder: true,
//        rowGrouping: true,
//        searching: true,
//        serverSide: true,
//        "pagingType": "full_numbers",
//        "bProcessing": true,
//        "bDeferRender": true,
//        "order": [[0, "asc"]],
//        stateSave: true,
//        "stateSaveCallback": function (settings, data) {
//            if (run == true) {
//                if (data.order) {
//                    data.order[0][0] = WOCMainorder;
//                    data.order[0][1] = WOCMainorderDir;
//                }
//                var filterinfoarray = getfilterinfoarrayMainComp($("#txtColumnCompSearch"));
//                $.ajax({
//                    "url": "/Base/CreateUpdateState",
//                    "data": {
//                        GridName: gridname,
//                        LayOutInfo: JSON.stringify(data),
//                        FilterInfo: JSON.stringify(filterinfoarray)
//                    },
//                    "dataType": "json",
//                    "type": "POST",
//                    "success": function () { return; }
//                });
//            }
//            run = false;
//        },
//        "stateLoadCallback": function (settings, callback) {
//            var o;
//            $.ajax({
//                "url": "/Base/GetLayout",
//                "data": {
//                    GridName: gridname
//                },
//                "async": false,
//                "dataType": "json",
//                "success": function (json) {
//                    if (json.LayoutInfo) {
//                        var LayoutInfo = JSON.parse(json.LayoutInfo);
//                        WOCMainorder = LayoutInfo.order[0][0];
//                        WOCMainorderDir = LayoutInfo.order[0][1];
//                        callback(JSON.parse(json.LayoutInfo));
//                        if (json.FilterInfo) {
//                            setsearchuiMainComp(JSON.parse(json.FilterInfo), $("#txtColumnCompSearch"), $("#searchfilteritems"));
//                            //GetCreateDateRangeFilterData();
//                            //GetProcessedDateRangeFilterData();

//                        }
//                    }
//                    else {
//                        callback(json);
//                    }

//                }
//            });
//            //return o;
//        },
//        scrollX: false,
//        language: {
//            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
//        },
//        sDom: 'Btlipr',
//        buttons: [],
//        "orderMulti": true,
//        "ajax": {
//            "url": "/DashBoard/GetWorkOrderCompletionWorkbenchGrid",
//            data: function (d) {
//                d.CustomQueryDisplayId = localStorage.getItem("MaintCompstatus");
//                d.txtSearchval = LRTrim($(document).find('#txtColumnCompSearch').val());
//                d.Order = WOCMainorder;
//            },
//            "type": "POST",
//            "datatype": "json",
//            "dataSrc": function (json) {
//                let colOrder = dtMaintenanceCompletionWorkbench.order();
//                //order = colOrder[0][0];
//                WOCMainorderDir = colOrder[0][1];
//                visibility = json.partSecurity;
//                rCount = json.data.length;
//                return json.data;
//            }
//        },
//        columnDefs: [
//            {
//                targets: [0],
//                className: 'noVis'
//            }
//        ],
//        "columns":
//            [
//                {
//                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
//                    "mRender": function (data, type, row) {
//                        return '<a class=lnk_Wosearch data-WoId="' + row.WorkOrderId + '" href="javascript:void(0)">' + data + '</a>';
//                    }
//                },
//                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
//                { "data": "EquipmentClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
//                { "data": "AssetName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
//                {
//                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5",
//                    render: function (data, type, row, meta) {
//                        if (data == statusCode.Approved) {
//                            return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + (data) + "</span >";
//                        }
//                        else if (data == statusCode.Canceled) {
//                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
//                        }
//                        else if (data == statusCode.Complete) {
//                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
//                        }
//                        else if (data == statusCode.Denied) {
//                            return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
//                        }
//                        else if (data == statusCode.Scheduled) {
//                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
//                        }
//                        else if (data == statusCode.WorkRequest) {
//                            return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide' style='width:95px;' >" + getStatusValue(data) + "</span >";
//                        }
//                        else if (data == statusCode.Planning) {
//                            return "<span class='m-badge m-badge-grid-cell m-badge--wide' style='width:95px;' >" + getStatusValue(data) + "</span >";
//                        }

//                        else {
//                            return getStatusValue(data);
//                        }
//                    }
//                },
//                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
//                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7" },
//                { "data": "Priority", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8" },
//                { "data": "ScheduledStartDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "9" },
//                { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "10" },
//            ],
//        initComplete: function () {
//            SetPageLengthMenu();
//        }
//    });
//}
$(document).on('click', '#tblMaintenanceCompletionWorkbench_paginate .paginate_button', function () {
    var currentselectedpage = parseInt($(document).find('#tblMaintenanceCompletionWorkbench_paginate .pagination').find('.active').text());
    cardviewlength = $(document).find('#Dashboardcardviewpagelengthdrp').val();
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
//$(document).on('change', '#tblMaintenanceCompletionWorkbench_length .searchdt-menu', function () {
//    run = true;
//});
$(document).on('change', '#Dashboardcardviewpagelengthdrp', function () {
    cardviewlength = $(this).val();
    grdcardcurrentpage = parseInt(cardviewstartvalue / cardviewlength) + 1;
    cardviewstartvalue = parseInt((grdcardcurrentpage - 1) * cardviewlength);

    LayoutUpdate('pagelength');
    ShowCardView();
});
$(document).find('#SortByDropdown li').on('click', function () {
    $(this).addClass('active').siblings().removeClass('active');
    SortByDropdown.hide();

    if (WOCMainorder == $(this).data('value')) {
        if (WOCMainorderDir == 'asc') {
            WOCMainorderDir = 'desc';
        }
        else if (WOCMainorderDir == 'desc') {
            WOCMainorderDir = 'asc';
        }
    }
    else {
        WOCMainorderDir = 'asc';
    }

    WOCMainorder = $(this).data('value');
    grdcardcurrentpage = 1;
    cardviewstartvalue = 0;

    LayoutUpdate('column');
    ShowCardView();
});
//$('#tblMaintenanceCompletionWorkbench').find('th').click(function () {
//    run = true;
//    WOCMainorder = $(this).data('col');
//});
function filterinfoMain(id, value) {
    this.key = id;
    this.value = value;
}
function getfilterinfoarray(txtsearchelement) {
    var filterinfoarray = [];
    var f = new filterinfoMain('searchstringMaintComp', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    //advsearchcontainer.find('.adv-item').each(function (index, item) {
    //    if ($(this).parent('div').is(":visible")) {
    //        f = new filterinfo($(this).attr('id'), $(this).val());
    //        filterinfoarray.push(f);
    //    }
    //    if ($(this).parent('div').find('div').hasClass('range-timeperiod')) {
    //        if ($(this).parent('div').find('input').val() !== '' && $(this).val() == '10') {
    //            f = new filterinfo('this-' + $(this).attr('id'), $(this).parent('div').find('input').val());
    //            filterinfoarray.push(f);
    //        }
    //    }
    //});
    return filterinfoarray;
}
function setsearchui(data, txtsearchelement, searchstringcontainer) {
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
        //else {
        //    if ($('#' + item.key).parent('div').is(":visible")) {
        //        $('#' + item.key).val(item.value);
        //        if (item.value && item.value.length > 0) {
        //            selectCount++;
        //            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossMain" aria-hidden="true"></a></span>';
        //        }
        //        //if ($('#' + item.key).hasClass('has-dtrangepicker') && item.value !== '') {
        //        //    $('#' + item.key).val(item.value).trigger('change');
        //        //    var datarangeval = data.filter(function (val) { return val.key === 'this-' + item.key; });
        //        //    if (datarangeval.length > 0) {
        //        //        if (datarangeval[0].value) {
        //        //            var rangeid = $('#' + item.key).parent('div').find('input').attr('id');
        //        //            $('#' + rangeid).css('display', 'block');
        //        //            $('#' + rangeid).val(datarangeval[0].value);
        //        //            if (item.key === 'dtgridadvsearchReadingDate') {
        //        //                StartReadingDate = datarangeval[0].value.split(' - ')[0];
        //        //                EndReadingDate = datarangeval[0].value.split(' - ')[1];
        //        //                $(document).find('#advreadingDatedaterange').daterangepicker(daterangepickersetting, function (start, end, label) {
        //        //                    StartReadingDate = start.format('MM/DD/YYYY');
        //        //                    EndReadingDate = end.format('MM/DD/YYYY');
        //        //                });
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //        //else {
        //        //    $('#' + item.key).val(item.value);
        //        //}
        //    }
        //    advcountercontainer.text(selectCount);
        //}
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
    // clearAdvanceSearch();
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
    //selectCount--;
    //if (searchtxtId == "gridadvsearchstatus") {
    //    $(document).find("#gridadvsearchstatus").val("").trigger('change.select2'); 
    //}

    //ProjectAdvSearch();
    cardviewstartvalue = 0;
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
                    gridstate.order[0] = [WOCMainorder, WOCMainorderDir];
                }
                else if (area === 'pagination') {//
                }
                else if (area === 'pagelength' || area === 'optionDropdownChange') {
                    gridstate.length = cardviewlength;
                }

                if (json.FilterInfo !== '') {
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnCompSearch"), $("#searchfilteritems"));
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
                var filterinfoarray = getfilterinfoarray($("#txtColumnCompSearch"), '');
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
                WOCMainorder = LayoutInfo.order[0][0]; //$('#tblworkorders').dataTable().fnSettings().aaSorting[0][0];
                WOCMainorderDir = LayoutInfo.order[0][1]; //$('#tblworkorders').dataTable().fnSettings().aaSorting[0][1];

                if (json.FilterInfo !== '') {
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnCompSearch"), $("#searchfilteritems"));
                }
            }
            else {
                DefaultLayoutInfo = DefaultLayoutInfo.replace('currentTime', new Date().getTime());
                var filterinfoarray = getfilterinfoarray($("#txtColumnCompSearch"), '');
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
/*$(document).on('change', '#MaintenanceCompDropdown', function (e) {*/
$(document).on('click', '#MaintenanceCompDropdown li', function (e) {
    if ($(this).hasClass('active') == true) {
        MaintenanceCompDropdown.hide();
        return false;
    }

    $(this).addClass('active').siblings().removeClass('active');
    MaintenanceCompDropdown.hide();

    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    //run = true;

    //if ($(this).attr('id') != '8') {
    //    $('#projectsearchtitle').text($(this).text());
    //    localStorage.setItem("Projstatustext", $(this).text());
    //}
    //else {
    //    $('#projectsearchtitle').text(getResourceValue("OpenProjectsAlert"));
    //    localStorage.setItem("Projstatustext", getResourceValue("OpenProjectsAlert"));
    //}

    //$(".searchList li").removeClass("activeState");
    //$(this).addClass('active');


    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).data('value');
    localStorage.setItem("MaintCompstatusMobile", optionval);
    CustomQueryDisplayId = optionval;


    if ($(document).find('#txtColumnCompSearch').val() !== '') {
        $('#searchfilteritems').find('span').html('');
        $('#searchfilteritems').find('span').removeClass('tagTo');
    }
    $(document).find('#txtColumnCompSearch').val('');


    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        //if (typeof (dtMaintenanceCompletionWorkbench) === "undefined") {
        //    generateMaintenanceCompletionWorkbench();
        //}
        //else {
        //    dtMaintenanceCompletionWorkbench.page('first').draw('page');
        //}
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
$(document).on('click', '.lnk_wocompletion_mobile', function (e) {
    e.preventDefault();
    var WorkOrderId = $(this).attr('id');
    var ClientLookupId = $(this).attr('clientlookupid');
    $.ajax({
        url: "/Dashboard/CompletionWorkbench_Details_Mobile",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { WorkOrderId: WorkOrderId, ClientLookupId: ClientLookupId },
        success: function (data) {
            $('#mainwidget').html(data);
        },
        complete: function () {
            SetWOCompletionDetailEnvironment();
            SetFixedHeadStyle();

            // Mobiscroll Navigation initialization
            //$('.md-apps-list').mobiscroll().listview({
            //    swipe: false,
            //    enhance: true
            //});

            if ($('#Labor').length > 0) {
                LoadLaborTab();
            }
            else if ($('#Parts').length > 0) {
                $('[data-tab="Parts"]').trigger('click');
            }
            InitWebCam();
            
        }
    });

});




var WoId;
$(document).on('mouseenter', '.assignedItem.hasMultipleAssigned', function (e) {
    var thise = $(this);
    WoId = $(this).data('woid');
    if (LRTrim($('#assigned' + WoId + ' span').html()).length > 0) {
        $('#assigned' + WoId).mobiscroll('show');
        return;
    }
    
    var waPersonnelId = $(this).attr('waPersonnelId');
    if (waPersonnelId == -1) {
        ShowAssignedPopOver(WoId, thise);
    }
});
function ShowAssignedPopOver(WoId, thise) {
    $.ajax({
        "url": "/DashBoard/PopulateHover",
        "data": {
            workOrderId: WoId
        },
        "dataType": "json",
        "type": "POST",
        "beforeSend": function (data) {
            thise.find('.loadingImg').show();
        },
        "success": function (data) {
            if (data.personnelList != null) {
                $('#assigned' + WoId + ' span').html(data.personnelList);
            }
        },
        "complete": function () {
            thise.find('.loadingImg').hide();
            //thise.find('.tooltipcards').attr('style', 'display :block !important;');
            $('#assigned' + WoId).trigger('mbsc-enhance');
            $('#assigned' + WoId).mobiscroll().popup({
                display: 'bubble',
                anchor: '#assignedItem' + WoId,
                buttons: []
            });
            $('#assigned' + WoId).mobiscroll('show');
            return false;
        }
    });
}
$(document).on('mouseleave', '.assignedItem.hasMultipleAssigned', function (e) {
    WoId = $(this).attr('id');
    $('#assigned' + WoId).mobiscroll('hide');
    return false;
});
function ZoomImage(element) {
    element.elevateZoom(ZoomConfig);
}
function SetWOCompletionDetailEnvironment() {
    CloseLoader();
    ZoomImage($(document).find('#EquipZoom'));

    SetFixedHeadStyle();
    $('#tabscroll').mobiscroll().nav({
        type: 'tab'
    });
}

$(document).on('click', '#linkToSearchWorkbench', function () {
    var DashboardId = $('#DashboardlistingId').val();
    if (DashboardId == null || DashboardId == "") {
        DashboardId = 0;
    }
    window.location = '/Dashboard/RedirectfromDashboardChange?DashboardId=' + DashboardId;
});
//#endregion

//#region Description Summary
$(document).on('click', '#wocreaddescription', function () {
    $(document).find('#wocdetaildesmodaltext').text($(this).data("des"));
    $(document).find('#wocdetaildesmodal').addClass('slide-active').trigger('mbsc-enhance');

});
$(document).on('click', '.woCardViewDescription', function () {
    $(document).find('#wocdetaildesmodaltext').text($(this).find("span").text());
    $(document).find('#wocdetaildesmodal').addClass('slide-active').trigger('mbsc-enhance');
});
$(document).on('click', '#wocdetaildesmodalHide', function () {
    $(document).find('#wocdetaildesmodal').removeClass('slide-active');
});
//#endregion

//#region v2-735 show WO Request and unplanned button
$(document).on('click', '#showWORequestUnplannedbtn', function () {
    $(document).find("#AddWorkRequestBtn_Mobile").css('display', 'inline-block');
    $(document).find("#showUnplannedWoPopup_Mobile").css('display', 'inline-block');
});
//#endregion
//#region V2-853 Reset Grid
$('#ResetGridBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("MaintCompstatusMobile");
        DeleteGridLayout_Mobile('MaintenanceCompletionWorkbench_Mobile_Search', localstorageKeys);
        GenerateSearchListMain('', true);
        window.location.href = "../Dashboard/Dashboard";
    });
});
//#endregion